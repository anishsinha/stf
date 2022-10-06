import { Component, OnInit, AfterViewInit } from '@angular/core';
import { RegionService } from '../service/region.service';
import { FormBuilder, FormGroup, Validators, FormControl, FormArray } from '@angular/forms';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { DropdownItem,StatelistService } from 'src/app/statelist.service';
import { SourceRegion, SourceRegionModel } from '../model/region';
import { Declarations } from 'src/app/declarations.module';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
declare var currentCompanyId: any;
declare var isSalesUser;

@Component({
    selector: 'app-source-region',
    templateUrl: './source-region.component.html',
    styleUrls: ['./source-region.component.css']
})
export class SourceRegionComponent implements OnInit {

    public _opened: boolean = false;
    public _animate: boolean = true;
    public _positionNum: number = 1;
    public _POSITIONS: Array<string> = ['left', 'right', 'top', 'bottom'];

    public States: DropdownItem[];
    public Cities: DropdownItem[] = [];
    public Terminals: DropdownItem[] = [];
    public BulkPlants: DropdownItem[] = [];
    public CarrierList: DropdownItem[];
    public multiselectSettingsByCode: IDropdownSettings;
    public multiselectSettingsById: IDropdownSettings;

    public region: SourceRegion;
    public regions: SourceRegion[];
    public rcForm: FormGroup;
    public serviceResponse: any;
    public IsUpdate: boolean = false;
    public SelectedCountryId: number;
    public CurrentCompanyId: number;
    public SelectedRegionToDelete: string = null;
    public IsLoading: boolean = true;
    public IsEmpty: boolean = false;
    public IsSourceRegionExist: any;
    public isSalesUser: boolean = false;

    constructor(private fb: FormBuilder, private regionService: RegionService, private carrierService: CarrierService) {
        //super();
    }

    ngOnInit() {

        this.multiselectSettingsByCode = {
            singleSelection: false,
            idField: 'Code',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };

        this.multiselectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };

        this.getRegions();
        this.getCarriers();
        this.rcForm = this.createForm();
        this.CurrentCompanyId = Number(currentCompanyId);  
        this.getDefaultServingCountry();
    }

    ngAfterViewInit() {
        if (typeof isSalesUser !== 'undefined' && isSalesUser) {
            this.isSalesUser = isSalesUser;
        }
    }

    fillDropdowns() {
        this.getCarriers();
        this.getDefaultServingCountry();
        this.getCitiesByStateId();
        this.getTerminals();
        this.getBulkPlants();
    }

    createForm() {
        if (this.region == undefined || this.region == null)
            this.region = new SourceRegion();
        return this.fb.group({
            Id: new FormControl(0),
            Name: new FormControl(this.region.Name, [Validators.required]),
            Description: new FormControl(this.region.Description),
            Carriers: new FormControl(this.region.Carriers),
            States: new FormControl(this.region.States, [Validators.required]),
            Cities: new FormControl(this.region.Cities),
            Terminals: new FormControl(this.region.Terminals, [Validators.required]),
            BulkPlants: new FormControl(this.region.BulkPlants)
        });
    }

    onSubmit() {
        this.region = this.rcForm.value;
        if (this.rcForm.valid && !this.IsSourceRegionExist) {
            if (this.IsUpdate) {
                this.updateRegion();
            }
            else {
                this.createRegion();
            }
        }
        else {
            this.rcForm.markAllAsTouched();
        }
    }

    isSourceRegionAvailable(name: string, id: string) {
        if (name != null) {
            this.regionService.isSourceRegionAvailable(name, id).subscribe(data => {
                this.IsSourceRegionExist = data;
            },
                error => {
                    this.IsSourceRegionExist = false;
                });
        }
    }

    createRegion() {
        if (!this.IsSourceRegionExist) {
            this.IsLoading = true;
            this.regionService.createSourceRegion(this.region)
                .subscribe((response: any) => {
                    this.serviceResponse = response;
                    if (response != null && response.StatusCode == 0) {
                        Declarations.msgsuccess('Source Region created successfully', undefined, undefined);
                        this.IsLoading = false;
                        this._toggleOpened(false);
                        this.getRegions();
                        
                    }
                    else {
                        this.IsLoading = false;
                        Declarations.msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                    }
                });
        }
    }

    editRegion(_region: SourceRegion) {
        this.region = _region;
        this._toggleOpened(true);
        this.IsUpdate = true;
        if (this.rcForm) {
            this.rcForm.controls['Id'].setValue(this.region.Id);
            this.rcForm.controls['Name'].setValue(this.region.Name);
            this.rcForm.controls['Description'].setValue(this.region.Description);
            this.rcForm.controls['Carriers'].setValue(this.region.Carriers);
            this.rcForm.controls['States'].setValue(this.region.States);
            if (this.region.Cities != null)
                this.region.Cities.forEach(s => s.Code = s.Code.replace(" ", ""));
            this.rcForm.controls['Cities'].setValue(this.region.Cities);
            this.rcForm.controls['Terminals'].setValue(this.region.Terminals);
            this.rcForm.controls['BulkPlants'].setValue(this.region.BulkPlants);
        }
        this.fillDropdowns();
    }

    updateRegion() {
        this.IsLoading = true;
        this.regionService.updateSourceRegion(this.region)
            .subscribe((response: any) => {
                this.serviceResponse = response;
                if (response != null && response.StatusCode == 0) {
                    Declarations.msgsuccess('Source Region updated successfully', undefined, undefined);
                    this.IsLoading = false;
                    this._toggleOpened(false);
                    this.getRegions();
                }
                else {
                    this.IsLoading = false;
                    Declarations.msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                }
            });
    }

    _toggleOpened(shouldOpen: boolean) {
        this.IsSourceRegionExist = false;
        this.rcForm.controls['Id'].setValue(0);
        if (shouldOpen) {
            this._opened = true;
            this.Terminals = [];
            this.BulkPlants = [];
        }
        else {
            this._opened = !this._opened;
            this.rcForm.reset();

            this.IsUpdate = false;
        }
    }

    getRegions() {
        this.IsLoading = true;
        this.regionService.getSourceRegions()
            .subscribe((response: SourceRegionModel) => {
                this.regions = response.Regions;
                if (this.regions != null && this.regions != undefined && this.regions.length == 0) {
                    this.IsEmpty = true;
                }
                this.IsLoading = false;
            });
    }

    setRegionIdToDelete(id: string) {
        this.SelectedRegionToDelete = id;
    }

     deleteRegion() {
        if (this.SelectedRegionToDelete == null) { return; }

        this.IsLoading = true;
         this.regionService.deleteSourceRegion(this.SelectedRegionToDelete)
            .subscribe((response: any) => {
                this.IsLoading = false;
                if (response != null && response.StatusCode == 0) {
                    Declarations.msgsuccess('Deleted successfully', undefined, undefined);
                    this.getRegions();
                }
                else{

                    Declarations.msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                }
            });
        this.SelectedRegionToDelete == null;
    }

    getDefaultServingCountry() {
        this.IsLoading = true;
        this.carrierService.getDefaultServingCountry(this.CurrentCompanyId).subscribe(data => {
            this.IsLoading = false;
            this.SelectedCountryId = Number(data.DefaultCountryId);
            this.getStates();
        });
    }

    getStates() {
        this.IsLoading = true;
        if (this.SelectedCountryId != undefined && this.SelectedCountryId > 0) {
            this.carrierService.getStates(this.SelectedCountryId)
                .subscribe((states: DropdownItem[]) => {
                    this.States = states;
                    this.IsLoading = false;
                });
        }
    }

    getCities(stateIds: number[]) {
        this.IsLoading = true;
        this.carrierService.getCities(stateIds).subscribe(data => {
            this.IsLoading = false;
            this.Cities = data as DropdownItem[];
            this.Cities.forEach(s => s.Code = s.Code.replace(" ", ""));
        });
    }

    getCarriers() {
        this.IsLoading = true;
        this.regionService.getCarriers()
            .subscribe((carriers: DropdownItem[]) => {
                this.CarrierList = carriers;
                this.IsLoading = false;
            });
    }

    getTerminals() {
        var selectedStates = this.rcForm.get('States').value as DropdownItem[];
        var selectedCities = this.rcForm.get('Cities').value as DropdownItem[];

        if (selectedStates.length == 0) {
            this.rcForm.get('States').patchValue([]);
            this.Terminals = [];
            return;
        }

        this.IsLoading = true;
        var input = new SourceRegion();
        if (selectedStates != null && selectedStates != undefined && selectedStates.length > 0) {
            var stateIds = selectedStates.map(s => s.Id);
            input.StateIds = stateIds.join(',');
        }
        if (selectedCities != null && selectedCities != undefined && selectedCities.length > 0) {
            var cityIds = selectedCities.map(s => s.Name);
            input.CityIds = cityIds.join(',');
        }
        input.CompanyId = this.CurrentCompanyId;
        this.carrierService.getTerminals(input).subscribe(data => {
            this.IsLoading = false;
            this.Terminals = data as DropdownItem[];
        });
    }

    getBulkPlants() {
        var selectedStates = this.rcForm.get('States').value as DropdownItem[];
        var selectedCities = this.rcForm.get('Cities').value as DropdownItem[];

        if (selectedStates.length == 0) {
            this.rcForm.get('States').patchValue([]);
            this.BulkPlants = [];
            return;
        }

        this.IsLoading = true;
        var input = new SourceRegion();
        if (selectedStates != null && selectedStates != undefined && selectedStates.length > 0) {
            var stateIds = selectedStates.map(s => s.Id);
            input.StateIds = stateIds.join(',');
        }
        if (selectedCities != null && selectedCities != undefined && selectedCities.length > 0) {
            var cityIds = selectedCities.map(s => s.Name);
            input.CityIds = cityIds.join(',');
        }
        input.CompanyId = this.CurrentCompanyId;
        this.carrierService.getBulkPlants(input).subscribe(data => {
            this.IsLoading = false;
            this.BulkPlants = data as DropdownItem[];
        });
    }

    onStateSelect(state: any) {
        this.getCitiesByStateId();
        this.getTerminals();
        this.getBulkPlants();
    }

    onStateDeSelect(state: any) {
        this.rcForm.get('Cities').patchValue([]);
        this.rcForm.get('Terminals').patchValue([]);
        this.rcForm.get('BulkPlants').patchValue([]);
        this.getCitiesByStateId();
        this.getTerminals();
        this.getBulkPlants();
    }

    onStateSelectAll(states: any) {
        this.rcForm.get('States').patchValue(states);
        this.getCitiesByStateId();
        this.getTerminals();
        this.getBulkPlants();
    }

    onStateDeSelectAll() {
        this.rcForm.get('Cities').patchValue([]);
        this.rcForm.get('Terminals').patchValue([]);
        this.rcForm.get('BulkPlants').patchValue([]);
        this.Cities = [];
        this.Terminals = [];
        this.BulkPlants = [];
    }

    getCitiesByStateId() {
        var selectedStates = this.rcForm.get('States').value as DropdownItem[];
        if (selectedStates != null && selectedStates != undefined && selectedStates.length > 0) {
            var stateIds = selectedStates.map(m => m.Id);
            this.getCities(stateIds);
        }
        else {
            this.Cities = [];
        }
    }

    onCitySelect(city: any) {
        this.rcForm.get('Terminals').patchValue([]);
        this.rcForm.get('BulkPlants').patchValue([]);
        this.getTerminals();
        this.getBulkPlants();
    }

    onCityDeSelect(city: any) {
        this.rcForm.get('Terminals').patchValue([]);
        this.rcForm.get('BulkPlants').patchValue([]);
        this.getTerminals();
        this.getBulkPlants();
    }

    onCitySelectAll(cities: any) {
        this.rcForm.get('Cities').patchValue(cities);
        this.getTerminals();
        this.getBulkPlants();
    }

    onCityDeSelectAll() {
        this.rcForm.get('Terminals').patchValue([]);
        this.rcForm.get('BulkPlants').patchValue([]);
        this.rcForm.get('Cities').patchValue([]);
        this.getTerminals();
        this.getBulkPlants();
    }
}
