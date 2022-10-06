import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl, FormArray } from '@angular/forms';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { Declarations } from 'src/app/declarations.module';
import { DropdownItem, StatelistService } from 'src/app/statelist.service';
import { CarrierRegionModel, Region, RegionModel, TfxCarrierDropdownDisplayItem, TfxCarrierRegionDetailsModel } from '../model/region';
import { Shift } from '../model/shift';
import { RegionService } from '../service/region.service';
declare var isSalesUser;

@Component({
  selector: 'app-dispatch-region',
  templateUrl: './dispatch-region.component.html',
    styleUrls: ['./dispatch-region.component.css']
})
export class DispatchRegionComponent extends Region implements OnInit, AfterViewInit {

    public _opened: boolean = false;
    public _animate: boolean = true;
    public _positionNum: number = 1;
    public _POSITIONS: Array<string> = ['left', 'right', 'top', 'bottom'];

    public StateList: DropdownItem[];
    public JobList: DropdownItem[];
    public DriverList: DropdownItem[];
    public DispatcherList: DropdownItem[];
    public TrailerList: DropdownItem[];

    public multiselectSettingsByCode: IDropdownSettings;
    public multiselectSettingsById: IDropdownSettings;

    public region: Region;
    public regions: Region[];
    public rcForm: FormGroup;
    public serviceResponse: any;
    public IsUpdate: boolean = false;

    public CurrentUserId: number;
    public CurrentCompanyId: number;
    public CountryId: number;
    public DefaultSlotPeriod: number;
    public SelectedRegionToDelete: string = null;
    public IsLoading: boolean = true;
    public IsEmpty: boolean = false;
    public CarrierList: DropdownItem[];
    //public IsSuccess: boolean = false;
    //carrier with region sequencing
    public CarrierRegions: CarrierRegionModel[] = [];
    public carrierDdlSetting: IDropdownSettings = {};
    formSubmitted: boolean = false;
    public isSalesUser: boolean = false;
    public ProductTypeList: DropdownItem[] = [];
    public FuelTypeList: DropdownItem[] = [];
   
    public selectedProdTypeList: DropdownItem[] = [];
    previousSelectedList: number[];
    removedProductNameString: string = '';
    IsPublishedDR: boolean = false;
    PastProduct : any = {};
    showAddFavOptn : boolean
    
    constructor(private fb: FormBuilder,
        private regionService: RegionService,
        private stateService: StatelistService) {
        super();
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

        this.carrierDdlSetting = {
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
        };

        this.getRegions();
        this.fillDropdowns();
        this.rcForm = this.createForm();
        
        this.makeCarrierUIsortable()
    }

    ngAfterViewInit() {
        if (typeof isSalesUser !== 'undefined' && isSalesUser) {
            this.isSalesUser = isSalesUser;
        }
    }

    createForm() {
        if (this.region == undefined || this.region == null)
            this.region = new Region();
        return this.fb.group({
            Id: new FormControl(''),
            Name: new FormControl(this.region.Name, [Validators.required]),
            SlotPeriod: new FormControl(this.DefaultSlotPeriod, [Validators.required]),
            Description: new FormControl(this.region.Description),
            Jobs: new FormControl(this.region.Jobs),
            Drivers: new FormControl(this.region.Drivers),
            Dispatchers: new FormControl(this.region.Dispatchers, [Validators.required]),
            Trailers: new FormControl(this.region.Trailers),
            States: new FormControl(this.region.States, [Validators.required]),
            Carriers: new FormArray([]),
            SelectedCarrier: this.fb.control(null),
            CreatedOn: new FormControl(''),
            Shifts: this.fb.array([]),
            ProductTypeIds: new FormControl(this.region.ProductTypeIds),
            FuelTypeIds: new FormControl(this.region.FuelTypeIds),
            IsSelectAllProductTypes: new FormControl(false),
            FormProductTypeIds: new FormControl([]),
            FavProductTypeId: new FormControl(this.region.FavProductTypeId.toString()),
            IsAddFavoriteProduct : new FormControl(false)
        });
    }

    clearShift() {
        const shifts = this.rcForm.controls.Shifts as FormArray;
        shifts.clear();
    }

    clearCarriers() {
        let carriers = this.rcForm.controls.Carriers as FormArray;
        carriers.clear();
    }

    addShift() {
        const shifts = this.rcForm.controls.Shifts as FormArray;
        if (shifts.length == 0) {
            shifts.push(this.fb.group({
                Id: new FormControl(''),
                Name: new FormControl('Morning', [Validators.required]),
                StartTime: new FormControl('02:00 AM', [Validators.required]),
                EndTime: new FormControl('04:00 PM', [Validators.required])
            }));
            shifts.push(this.fb.group({
                Id: new FormControl(''),
                Name: new FormControl('Evening', [Validators.required]),
                StartTime: new FormControl('02:00 PM', [Validators.required]),
                EndTime: new FormControl('04:00 AM', [Validators.required])
            }));
        }
        else {
            shifts.push(this.fb.group({
                Id: new FormControl(''),
                Name: new FormControl('', [Validators.required]),
                StartTime: new FormControl('', [Validators.required]),
                EndTime: new FormControl('', [Validators.required])
            }));
        }
    }

    editShifts(_shifts: Shift[]) {
        var formB = this.fb;
        const shifts = this.rcForm.controls.Shifts as FormArray;
        shifts.clear();
        _shifts.forEach(function (shift, idx) {
            shifts.push(formB.group({
                Id: new FormControl(shift.Id),
                Name: new FormControl(shift.Name, [Validators.required]),
                StartTime: new FormControl(shift.StartTime, [Validators.required]),
                EndTime: new FormControl(shift.EndTime, [Validators.required])
            }));
        });
    }

    removeShift(i: number): void {
        const shifts = this.rcForm.get('Shifts') as FormArray;
        shifts.removeAt(i);
    }

    isInvalid(name: string, i: number): boolean {
        var shifts = this.getShifts();
        var result = shifts.controls[i].get(name).invalid
            &&
            (
                shifts.controls[i].get(name).dirty
                ||
                shifts.controls[i].get(name).touched
            )
        return result;
    }

    isRequired(name: string, i: number): boolean {
        var shifts = this.getShifts();
        return shifts.controls[i].get(name).errors.required;
    }

    getShifts(): FormArray {
        return this.rcForm.get('Shifts') as FormArray;
    }

    editRegion(_region: Region) {
        this.region = _region;
        this.formSubmitted = false;
        this._toggleOpened(true, this.region.Id);
        this.IsUpdate = true;
        if (this.rcForm) {
            this.rcForm.controls['Id'].setValue(this.region.Id);
            this.rcForm.controls['Name'].setValue(this.region.Name);
            this.rcForm.controls['Description'].setValue(this.region.Description);
            this.rcForm.controls['SlotPeriod'].setValue(this.region.SlotPeriod);
            this.rcForm.controls['Jobs'].setValue(this.region.Jobs);
            this.rcForm.controls['Drivers'].setValue(this.region.Drivers);
            this.rcForm.controls['Dispatchers'].setValue(this.region.Dispatchers);
            this.rcForm.controls['Trailers'].setValue(this.region.Trailers);
            this.rcForm.controls['States'].setValue(this.region.States);
            this.setCarrierRegions(this.region.Carriers);

            if (this.region.ProductTypeIds.length > 0 || this.region.FuelTypeIds.length > 0) {
                this.rcForm.controls['IsAddFavoriteProduct'].setValue(true);
                this.rcForm.controls['FavProductTypeId'].setValue(this.region.FavProductTypeId.toString());

                if (this.rcForm.controls['FavProductTypeId'].value == 1) {
                    this.rcForm.controls['ProductTypeIds'].setValue(this.region.ProductTypeIds);
                    let formProductTypes = [];
                    if (this.region.ProductTypeIds)
                        formProductTypes = this.ProductTypeList.filter(x => this.region.ProductTypeIds.indexOf(x.Id) > -1);
                    this.rcForm.controls['FormProductTypeIds'].setValue(formProductTypes);
                }
                else if (this.rcForm.controls['FavProductTypeId'].value == 2) {
                    this.rcForm.controls['FuelTypeIds'].setValue(this.region.FuelTypeIds);
                }
            }
            else {
                this.rcForm.controls['IsAddFavoriteProduct'].patchValue(false);
            }


            this.editShifts(_region.Shifts);
        }
    }

    makeCarrierUIsortable() {
        var _this = this;
        $(function () {
            let sortable: any = $("#sortableRegionCarriers");
            sortable.sortable({
                stop: function(event, ui) {
                        var carrierIds = sortable.sortable("toArray") as number[];
                        _this.updateSequence(carrierIds); sortable.click();
                    }
                });
        });
    }
    updateSequence(carrIds: number[]){
        let _formArray = this.rcForm.controls['Carriers'] as FormArray;
        let carriers = _formArray. value as TfxCarrierDropdownDisplayItem[]
        _formArray.clear();

        carrIds.forEach(id => {
            let carr = carriers.find(c=>c.Id == id);
            this.pushRowInCarrierForm(_formArray, carr);
        });
    }
    pushRowInCarrierForm(_formArray: FormArray, data: TfxCarrierDropdownDisplayItem){
        _formArray.push(this.fb.group({
            Id: this.fb.control(data.Id),
            Name: this.fb.control(data.Name),
            RegionId: this.fb.control(this.CarrierRegions.some(f => f.Regions.some(r=>r.Id == data.RegionId))? data.RegionId: null, [Validators.required]),
            SequenceNo: this.fb.control(data.SequenceNo),
        }));
    }
    setCarrierRegions(_carrRegions: TfxCarrierDropdownDisplayItem[]) {

        let _formArray = this.rcForm.controls['Carriers'] as FormArray;
        _formArray.clear();

        _carrRegions.forEach(carr => {
            this.pushRowInCarrierForm(_formArray, carr);
        });
        this.rcForm.get('SelectedCarrier').patchValue(_formArray.value);
    }
    removeCarrier(id: number){
        //remove from form
        let _formArray = this.rcForm.controls['Carriers'] as FormArray;
        _formArray.removeAt(_formArray.value.findIndex(carr => carr.Id == id));
        //remove from dropdown
        let currentSelection: any[] = this.rcForm.get('SelectedCarrier').value || [];
        currentSelection.splice(currentSelection.findIndex(carr => carr.Id == id), 1);
        this.rcForm.get('SelectedCarrier').patchValue(currentSelection);
    }
    getRegionsByCarrierId(id: number): TfxCarrierRegionDetailsModel[]{
        let response = []
        if (this.CarrierRegions) {
            let carr = this.CarrierRegions.find(f => f.Id == id);
            if (carr != null && carr.Regions)
                response = carr.Regions;
        }
        return response;
    }

    onCarrierSelect(item: any, isSelect: boolean){
        if(isSelect) {
            let selection = this.CarrierRegions.find(c=>c.Id == item.Id);
            if(selection) {
                let _formArray = this.rcForm.controls['Carriers'] as FormArray;
                this.pushRowInCarrierForm(_formArray, { Id: selection.Id, Code: null, Name: selection.Name, RegionId: null, SequenceNo: null});
            }
        }
        else{
            let _formArray = this.rcForm.controls['Carriers'] as FormArray;
            _formArray.removeAt(_formArray.value.findIndex(carr => carr.Id == item.Id));
        }
    }
    
    public onCarrierSelectAll(items: any, isSelectAll: boolean) {
        let _formArray = this.rcForm.controls['Carriers'] as FormArray;
        if(isSelectAll){
            let existingFormCarriers = _formArray.value as any[] || [];
            this.CarrierRegions.forEach((carrierRegion: CarrierRegionModel) => {
                if(!existingFormCarriers.some(c=>c.Id == carrierRegion.Id)) {
                    let _formArray = this.rcForm.controls['Carriers'] as FormArray;
                    this.pushRowInCarrierForm(_formArray, { Id: carrierRegion.Id, Code: null, Name: carrierRegion.Name, RegionId: null, SequenceNo: null});
                }
            });
        }
        else {
            _formArray.clear();
        }
    }

    setRegionIdToDelete(id: string) {
        this.SelectedRegionToDelete = id;
    }

    deleteRegion() {
        if (this.SelectedRegionToDelete == null) { return; }

        this.IsLoading = true;
        this.regionService.deleteRegion(this.SelectedRegionToDelete)
            .subscribe((response: any) => {
                this.IsLoading = false;
                if (response != null && response.StatusCode == 0) {
                    Declarations.msgsuccess('Deleted successfully', undefined, undefined);
                    this.getRegions();
                }
                else {
                    Declarations.msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                }
            });
        this.SelectedRegionToDelete == null;
    }

    fillDropdowns() {
        this.getJobs();
        this.getDrivers("");
        this.getDispatchers();
        this.getTrailers();
        this.getCarriers();
        this.getCarrierRegions();
        this.getProductType();
        this.getProducts();
    }

    getJobs() {
        this.IsLoading = true;
        this.regionService.getJobs()
            .subscribe((jobs: DropdownItem[]) => {
                this.JobList = jobs;
                this.IsLoading = false;
            });
    }

    getDrivers(regionId) {
        this.IsLoading = true;
        this.regionService.getRegionDrivers(regionId)
            .subscribe((drivers: DropdownItem[]) => {
                this.DriverList = drivers;
                this.IsLoading = false;
            });
    }

    getDispatchers() {
        this.IsLoading = true;
        this.regionService.getDispatchers()
            .subscribe((dispatchers: DropdownItem[]) => {
                this.DispatcherList = dispatchers;
                this.IsLoading = false;
            });
    }

    getTrailers() {
        this.IsLoading = true;
        this.regionService.getTrailers()
            .subscribe((trailers: DropdownItem[]) => {
                this.TrailerList = trailers;
                this.IsLoading = false;
            });
    }

    getStates() {
        this.IsLoading = true;
        this.regionService.getStates(this.CountryId)
            .subscribe((states: DropdownItem[]) => {
                this.StateList = states;
                this.IsLoading = false;
            });
    }

    getRegions() {
        this.IsLoading = true;
        this.regionService.getRegions()
            .subscribe((response: RegionModel) => {
                this.CurrentUserId = response.UserId;
                this.CurrentCompanyId = response.CompanyId;
                this.CountryId = response.CountryId;
                this.DefaultSlotPeriod = response.DefaultSlotPeriod;
                this.rcForm.controls['SlotPeriod'].setValue(response.DefaultSlotPeriod);
                this.regions = response.Regions;
                if (this.regions != null && this.regions != undefined && this.regions.length == 0) {
                    this.IsEmpty = true;
                }
                this.IsLoading = false;
                this.getStates();
            });
    }

    validateAndSubmitForm() {
        this.formSubmitted = true;
        let selectedProductType = this.rcForm.controls["FavProductTypeId"].value;
        if (selectedProductType == 1) {
            let newProducts = this.rcForm.get('FormProductTypeIds').value || [];
            if (newProducts && newProducts.length > 0) {
                this.rcForm.get('ProductTypeIds').patchValue(newProducts.map(x => x.Id));
            } else {
                this.rcForm.get('ProductTypeIds').patchValue([]);
            }
        }
        else if (selectedProductType == 2) {
            let newProducts: DropdownItem[] = this.rcForm.get('FuelTypeIds').value || [];
            let filterProducts = this.FuelTypeList.filter(t => newProducts.some(t1 => t1.Id == t.Id));
            if (filterProducts && filterProducts.length > 0)
                this.rcForm.get('FuelTypeIds').patchValue(filterProducts);
            else
                this.rcForm.get('FuelTypeIds').patchValue([]);
        }

        // pop up for removed validate product types
        let submitWithConfirmation = false;
        if (this.IsUpdate) {
            this.PastProduct = { FavProductTypeId: this.region.FavProductTypeId.toString(), ProductTypeIds: this.region.ProductTypeIds, FuelTypeIds: this.region.FuelTypeIds };
            var removedList = this.getRemovedList();
            if (removedList && removedList.length > 0) {
                this.removedProductNameString = removedList.map(t => t.Name).join(",");
                this.IsLoading = true;
                var productIds = (+selectedProductType == 1) ? removedList.map(t => t.Id).join(",") : "";
                var fuelTypeIds = (+selectedProductType == 2) ? removedList.map(t => t.Id).join(",") : "";
                this.regionService.isPublishedDR(productIds, fuelTypeIds)
                    .subscribe((response: any) => {
                        this.IsLoading = false;
                        let elem = document.getElementById('openConfirmationModel01'); elem.click();
                        this.IsPublishedDR = response;
                    });
                this.formSubmitted = false;
                submitWithConfirmation = true;
                return;
            }
        }
        if (!submitWithConfirmation) {
            this.onSubmit();
        }
    }

    getRemovedList(): DropdownItem[] {
        var rList = [];
        let selectedProductType = this.rcForm.controls["FavProductTypeId"].value;
        let newProducts = this.rcForm.get('ProductTypeIds').value as number[] || [];
        let newFuelTypes = this.rcForm.get('FuelTypeIds').value as DropdownItem[] || [];
        if (selectedProductType == 1 && newProducts && newProducts.length > 0) { // ProductType
            if (this.region.FavProductTypeId == 2) {
                return this.region.FuelTypeIds;
            } else {
                // filter different productTypes
                let removedProductTypeIds: number[] = this.region.ProductTypeIds.filter((o1) => { return !newProducts.some(o2 => o1 == o2); }) || [];
                if (removedProductTypeIds && removedProductTypeIds.length > 0) {
                    return this.ProductTypeList.filter(x => removedProductTypeIds.includes(x.Id));
                }
            }
        } else if (selectedProductType == 2 && newFuelTypes && newFuelTypes.length > 0) {
            if (this.region.FavProductTypeId == 1) {
                return this.ProductTypeList.filter(t => this.region.ProductTypeIds.includes(t.Id));
            } else {
                // filter different productTypes
                return this.region.FuelTypeIds.filter((o1) => { return !newFuelTypes.some(o2 => o1.Id == o2.Id); }) || [];
            }
        }
        return rList;
    }

    resetProductType() {
        this.setPastValues();
        Declarations.msgsuccess('No product have been removed from the region.', undefined, undefined);
        this.removedProductNameString = '';
    }
    toggleFavProducts(event) {
       this.clearAllProducts();
    }
    onSubmit() {
        this.region = this.rcForm.value;
       
        if(this.region.Carriers && this.region.Carriers.length>0){
            this.region.Carriers.forEach((carr, index: number)=>{
                carr.SequenceNo = (index + 1)
            })
        }

        if (this.rcForm.valid) {
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
    private IdInComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other == current.Id
            }).length == 1;
        }
    }

    
    createRegion() {
        this.IsLoading = true;
        this.regionService.createRegion(this.region)
            .subscribe((response: any) => {
                this.formSubmitted = false;
                this.serviceResponse = response;
                if (response != null && response.StatusCode == 0) {
                    Declarations.msgsuccess('Dispatch Region created successfully', undefined, undefined);
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

    updateRegion() {
        this.IsLoading = true;
        this.regionService.updateRegion(this.region)
            .subscribe((response: any) => {
                this.serviceResponse = response;
                this.formSubmitted = false;
                if (response != null && response.StatusCode == 0) {
                    Declarations.msgsuccess('Dispatch Region updated successfully', undefined, undefined);
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

    _toggleOpened(shouldOpen: boolean, regionId: string = "") {
        if (shouldOpen) {
            this.getDrivers(regionId);
            this._opened = true;
        }
        else {
            this._opened = !this._opened;
            this.rcForm.reset();
            this.rcForm.controls['SlotPeriod'].setValue(this.DefaultSlotPeriod);

            this.IsUpdate = false;
            this.clearShift();
            this.clearCarriers();
        }
    }

    _getJobsForRegion(IsCreateRegion: boolean) {
        if (IsCreateRegion) {
            this.getJobs();
        }
        else {
            let currentJobsInRegion = new Array<DropdownItem>();
            currentJobsInRegion = this.rcForm.controls['Jobs'].value;
            this.regionService.getJobs()
                .subscribe((data: DropdownItem[]) => {
                    if (currentJobsInRegion != null && currentJobsInRegion != undefined && currentJobsInRegion.length > 0)
                    {
                        currentJobsInRegion.forEach(item => {
                            data.push(item);
                        });
                    }
                    this.JobList = data;
                });                 
        }
    }
    getCarriers() {
        this.IsLoading = true;
        this.regionService.getCarriers()
            .subscribe((carriers: DropdownItem[]) => {
                this.CarrierList = carriers;
                this.IsLoading = false;
        });
    }
    getCarrierRegions() {
        this.IsLoading = true;
        this.regionService.getCarrierRegions().subscribe((data) => {
            this.CarrierRegions = data;
            this.IsLoading = false;
        });
    }

    getProductType() {
        this.IsLoading = true;
        this.regionService.getProductType()
            .subscribe((productType: DropdownItem[]) => {
                this.ProductTypeList = productType;
        });
    }
    getProducts() {
        this.IsLoading = true;
        this.regionService.getFuelProducts().subscribe((products: DropdownItem[]) => {
            this.FuelTypeList = products;
        })
    }
    clearAllProducts() {
        this.rcForm.controls['FuelTypeIds'].patchValue([]);
        this.rcForm.controls['ProductTypeIds'].patchValue([]);
        this.rcForm.controls['FormProductTypeIds'].patchValue([]);
    }
    setPastValues() {
        this.rcForm.controls['FavProductTypeId'].patchValue(this.PastProduct['FavProductTypeId'] || '1');
        this.rcForm.controls['FuelTypeIds'].patchValue(this.PastProduct['FuelTypeIds'] || []);
        let selProductTypeIds = this.PastProduct['ProductTypeIds'] || [];
        this.rcForm.controls['ProductTypeIds'].patchValue(this.PastProduct['ProductTypeIds'] || []);
        this.rcForm.controls['FormProductTypeIds'].patchValue(this.ProductTypeList.filter(x => selProductTypeIds.indexOf(x.Id) > -1));
    }
}