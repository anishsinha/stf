import { Component, OnInit, ViewChild, ElementRef, Input, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { DropdownItem } from 'src/app/statelist.service';
import { Declarations } from 'src/app/declarations.module';
import { LocationDetailsModel, TerminalMappingModel } from 'src/app/carrier/models/location';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { LocationService } from 'src/app/location/services/location.service';
import { DropDownItem } from 'src/app/buyer-wally-board/Models/BuyerWallyBoard';
declare var currentCompanyId: any;
declare var IsLiftFileValidationEnabled: any;

@Component({
  selector: 'app-terminal-mapping',
  templateUrl: './terminal-mapping.component.html',
  styleUrls: ['./terminal-mapping.component.css']
})
export class TerminalMappingComponent implements OnInit, AfterViewInit  {
    public TerminalMappingForm: FormGroup;
    public dtOptions: any = {};
    public dtTrigger: Subject<any> = new Subject();
    public selected: number = 0;
    public TerminalsList: LocationDetailsModel[] = [];
    public StateList: DropdownItem[] = [];
    public CityList: DropdownItem[] = [];
    public TerminalList: DropdownItem[] = [];
    public IsLoading: boolean;
    public SelectedCountryId: number;
    public SelectedTerminalId: DropdownItem[] = [];
    public CurrentCompanyId: any;
    public ddlSettingsById = {};
    public ddlSettingsByIdSingleSelect = {};
    public ddlSettingsByCode = {};
    public ddlSettingsForTerminal = {};
    public IsValidForm: boolean = true;
    public existingAssignedTerminalId: string = '';
    public nameToUpdate: string = '';
    public existingId: number = 0;
    public popoverSaveTitle: string = 'Save the change(s)?';
    public popoverDeleteTitle: string = 'Are you sure, want to delete?';
    public confirmButtonText: string = 'Yes';
    public cancelButtonText: string = 'No';
    @Input() countryId: number;

    public TerminalSupplierList: DropDownItem[];
    public IsLiftFileValidationEnabled: boolean = false;

    constructor(private fb: FormBuilder,private carrierService: CarrierService, private locationService: LocationService) {

        this.TerminalMappingForm = this.fb.group({
            States: this.fb.control([], [Validators.required]),
            Cities: this.fb.control([]),
            SelectedTerminal: this.fb.control([], [Validators.required]),
            MyTerminalId: this.fb.control('', [Validators.required]),
            CompanyId: this.fb.control(0),
            Terminals: this.fb.control(0),
            SelectedTerminalSupplier: this.fb.control([]),
        });
    };

    ngOnInit() {

        this.ddlSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };

        this.ddlSettingsForTerminal = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };

        this.ddlSettingsByCode = {
            singleSelection: false,
            idField: 'Code',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };

        this.ddlSettingsByIdSingleSelect = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };
        this.initializeUserTerminalData();
       // this.loadData();
        this.CurrentCompanyId = Number(currentCompanyId);
        this.IsLiftFileValidationEnabled = IsLiftFileValidationEnabled;
    }

    ngOnChanges() {
        this.clearForm();
    }
   
    
    loadData() {
        this.SelectedCountryId = Number(this.countryId);

        if (isNaN(this.SelectedCountryId) || this.SelectedCountryId == 0) {
            this.getDefaultServingCountry();
        }
        else {
            this.getStates();
            this.getAllUserTerminalData();
        }
    }


    getDefaultServingCountry() {
        this.IsLoading = true;
        this.carrierService.getDefaultServingCountry(this.CurrentCompanyId).subscribe(data => {
            this.IsLoading = false;
            this.SelectedCountryId = Number(data.DefaultCountryId);
            this.getStates();
            this.getAllUserTerminalData();
        });
    }

    filterGrid() {
        $("#terminal-grid-datatable").DataTable().clear().destroy();
    }

    onStateSelect(state: any) {
        this.getCitiesByStateId();
        this.getTerminals();
    }

    onStateDeSelect(state: any) {
        this.TerminalMappingForm.get('Cities').patchValue([]);
        this.TerminalMappingForm.get('SelectedTerminal').patchValue([]);
        this.TerminalMappingForm.get('SelectedTerminalSupplier').patchValue([]);
        this.getCitiesByStateId();
        this.getTerminals();
    }

    onStateSelectAll(states: any) {
        this.TerminalMappingForm.get('States').patchValue(states);
        this.getCitiesByStateId();
        this.getTerminals();
    }

    getTerminals() {
        var selectedStates = this.TerminalMappingForm.get('States').value as DropdownItem[];
        var selectedCities = this.TerminalMappingForm.get('Cities').value as DropdownItem[];

        if (selectedStates.length == 0) {
            this.TerminalMappingForm.get('States').patchValue([]);
            this.TerminalList = [];
            this.TerminalSupplierList = [];
            return;
        }

        var input = new TerminalMappingModel();
        if (selectedStates != null && selectedStates != undefined && selectedStates.length > 0) {
            var stateIds = selectedStates.map(s => s.Id);
            input.StateIds = stateIds.join(',');
        }
        if (selectedCities != null && selectedCities != undefined && selectedCities.length > 0) {
            var cityIds = selectedCities.map(s => s.Name);
            input.CityIds = cityIds.join(',');
        }
        input.CountryId = this.SelectedCountryId;
        this.IsLoading = true;
        this.carrierService.getTerminalsForMapping(input).subscribe(data => {
            this.IsLoading = false;
            this.TerminalList = data as DropdownItem[];
        });
        this.getTerminalSuppliers();
    }

    onStateDeSelectAll() {
        this.TerminalMappingForm.get('Cities').patchValue([]);
        this.TerminalMappingForm.get('SelectedTerminal').patchValue([]);
        this.TerminalMappingForm.get('SelectedTerminalSupplier').patchValue([]);
        this.CityList = [];
        this.TerminalList = [];
        this.TerminalSupplierList = [];
        
    }

    getCitiesByStateId() {
        var selectedStates = this.TerminalMappingForm.get('States').value as DropdownItem[];

        if (selectedStates != null && selectedStates != undefined && selectedStates.length > 0) {
            var stateIds = selectedStates.map(m => m.Id);
            this.getCities(stateIds);
        }
        else {
            this.CityList = [];
        }
    }



    getCountryFilter(): any {
        return (localStorage.getItem('countryFilterType')) ? (localStorage.getItem('countryFilterType')) : (localStorage.getItem('countryIdForDashboard') ? localStorage.getItem('countryIdForDashboard') : 1);
    }

    getStates() {
        this.IsLoading = true;
        if (this.SelectedCountryId != undefined && this.SelectedCountryId > 0) {
            this.carrierService.getStates(this.SelectedCountryId).subscribe(data => {
                this.IsLoading = false;
                this.StateList = data as DropdownItem[];
            });
        }
    }

    getCities(stateId: number[]) {
        
        this.IsLoading = true;
        this.carrierService.getCities(stateId).subscribe(data => {
            this.IsLoading = false;
            this.CityList = data as DropdownItem[];
        });
    }

    onCitySelect(city: any) {
        this.getTerminals();
    }

    onCityDeSelect(city: any) {
        this.TerminalMappingForm.get('SelectedTerminal').patchValue([]);
        this.getTerminals();
    }

    onCitySelectAll(cities: any) {
        this.TerminalMappingForm.get('SelectedTerminal').patchValue([]);
        this.TerminalMappingForm.get('Cities').patchValue(cities);
        this.getTerminals();
    }

    onCityDeSelectAll() {
        this.TerminalMappingForm.get('SelectedTerminal').setValue([]);
        this.getTerminals();
    }

    clearFilter() {
        this.clearForm();
    }

    clearForm() {
        this.TerminalMappingForm.reset();
        this.TerminalMappingForm.get('States').setValue([]);
        this.TerminalMappingForm.get('Cities').setValue([]);
        this.TerminalMappingForm.get('SelectedTerminal').setValue([]);
        this.TerminalMappingForm.get('SelectedTerminalSupplier').setValue([]);

        this.existingAssignedTerminalId = '';
        this.nameToUpdate = '';
        this.TerminalList = [];
        this.CityList = [];
        this.TerminalSupplierList = [];
        $("#terminal-grid-datatable").DataTable().clear().destroy();
        this.loadData();
    }

    selectchange(args) {
        this.SelectedTerminalId = args.target.value;
    } 

    onSubmit() {
        let TermSupplierId = 0;
        let TerminalId = 0;
        var AssignedTerminalId = this.TerminalMappingForm.get('MyTerminalId').value;
        var objTerminalId = this.TerminalMappingForm.get('SelectedTerminal').value as DropdownItem[];
        var SelectedTermSupplier = this.TerminalMappingForm.get('SelectedTerminalSupplier').value as DropDownItem[];

        if (SelectedTermSupplier != null && SelectedTermSupplier.length> 0) {
            TermSupplierId = SelectedTermSupplier[0].Id;
        }
        if (objTerminalId.length) {
             TerminalId = objTerminalId[0].Id;
        }
        if ((AssignedTerminalId == undefined || AssignedTerminalId == null || AssignedTerminalId.trim() == '' || TerminalId == 0)
        ) {
            if (TerminalId == 0) {
                Declarations.msgerror('Please provide Terminal Name', undefined, undefined);
                return;
            } else {
                Declarations.msgerror('Please provide Terminal ID', undefined, undefined);
                return;
            }

        } else {
            this.IsValidForm = true;
        }
        var TerminalMappingViewModel =
        {
            Id: 0,
            AssignedTerminalId: AssignedTerminalId,
            TerminalId: TerminalId,
            IsBulkPlant: this.TerminalList.find(f => f.Id == TerminalId).Code == '1' ? true : false,           
            TerminalSupplierId: TermSupplierId
        }

        this.TerminalMappingForm.get("CompanyId").patchValue(this.CurrentCompanyId);
        
        if (!this.IsValidForm) {
            this.IsValidForm = false;
        }
        else {
            this.checkDuplicateTerminalId(TerminalMappingViewModel);
        }
    }

    submitForm(TerminalMappingViewModel) {
        this.IsLoading = true;
        this.carrierService.saveTerminalMapping(TerminalMappingViewModel).subscribe(data => {
            if (data.StatusCode == 0) {
                this.IsLoading = false;
                this.clearForm();
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
            }
            else if (data.StatusCode == 2) {
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
                
            }
            else {
                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
               
            }
        });
    }

    editTerminalId(mapping: LocationDetailsModel, key: string, $event: any) {
        this.nameToUpdate = $event.target.innerText;
        this.existingAssignedTerminalId = mapping.AssignedTerminalId;
        this.existingId = mapping.Id;
    }

    updateTerminalId(mapping: LocationDetailsModel) {
        if (mapping.Id == this.existingId) {
            mapping.AssignedTerminalId = this.nameToUpdate;
        }

        if (mapping.AssignedTerminalId.trim() == '') {
            Declarations.msgerror('Please provide My Terminal ID', undefined, undefined);
            return;
        }
        this.checkDuplicateTerminalId(mapping);
    }

    updateTerminal(mapping: LocationDetailsModel) {

        this.IsLoading = true;
        this.carrierService.updateTerminalId(mapping).subscribe(data => {
            this.IsLoading = false;
            if (data.StatusCode == 0) {
                
                this.clearForm();
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
            }
            else if (data.StatusCode == 2) {
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
            }
            else {
                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
        });
    }

    
    checkDuplicateTerminalId(TerminalMappingViewModel) {
        this.carrierService.checkDuplicateTerminalId(TerminalMappingViewModel).subscribe(data => {
            if (data.StatusCode == 0) {

                if (TerminalMappingViewModel.Id == 0) {
                    this.submitForm(TerminalMappingViewModel);
                } else {
                    this.updateTerminal(TerminalMappingViewModel);
                }
            }

            if (data.StatusCode == 2) {
                this.existingAssignedTerminalId = '';
                this.nameToUpdate = '';
                $("#terminal-grid-datatable").DataTable().clear().destroy();
                this.getAllUserTerminalData();
                Declarations.msgerror(data.StatusMessage, undefined, undefined);
            }
        });
    }

    deleteMapping(mappingId: number, companyId: number) {
        if (mappingId == undefined || mappingId <= 0)
            return;
        var model = new LocationDetailsModel();
        model.Id = mappingId;
        this.IsLoading = true;
        this.carrierService.postDeleteTerminalMappingById(model).subscribe(data => {
            this.IsLoading = false;
            if (data.StatusCode == 0) {
                this.clearForm();
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
            }
            else if (data.StatusCode == 2) {
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
            }
            else {
                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
        });
    }

    cancelUpdateTerminalNames(mapping: LocationDetailsModel) {
        //mapping.AssignedTerminalId = this.existingAssignedTerminalId;
        this.clearForm();
    }


    initializeUserTerminalData() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'Terminal Details', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'Terminal Details', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            aaSorting: [],
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }

    getAllUserTerminalData() {
        this.IsLoading = true;
        var SelectedCountryId = this.SelectedCountryId;
        this.carrierService.getTerminalMappingGrid(SelectedCountryId).subscribe(data => {
            this.IsLoading = false;
            this.TerminalsList = data as LocationDetailsModel[];
            this.dtTrigger.next();
        });
    }

    ngAfterViewInit() {
      //  this.dtTrigger.next();
    }

    public getTerminalSuppliers() {
        this.IsLoading = true;
        var selectedCountryId = this.SelectedCountryId;
        this.carrierService.getTerminalSupplier(selectedCountryId).subscribe(data => {
            this.IsLoading = false;
            this.TerminalSupplierList = data as DropdownItem[];
        });
    }

}
