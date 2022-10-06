import { Component, OnInit, Input, Output, ChangeDetectionStrategy, ViewEncapsulation, EventEmitter, OnChanges, SimpleChanges, ElementRef, ViewChild } from '@angular/core';
import { FreightPricingRulesViewModel } from 'src/app/freightRates/Models/createFreightRateRules'
import { ExternalMappingsService } from 'src/app/self-service-alias/service/externalmappings.service';
import { MyLocalStorage } from 'src/app/my.localstorage'
import { FuelSurchargeService } from 'src/app/fuelsurcharge/services/fuelsurcharge.service'
import { Subject, forkJoin, merge, BehaviorSubject } from 'rxjs';
import { FormBuilder, FormGroup, Validators, FormControl, FormArray, AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { RegionService } from 'src/app/company-addresses/region/service/region.service';
import { DropdownItem, DropdownItemExt } from 'src/app/statelist.service';
import { Declarations } from 'src/app/declarations.module';
import * as moment from 'moment';
import { pairwise, startWith } from 'rxjs/operators';
import { FreightRateRulesService } from '../../freightRates/Services/freight-rate-rules.service';
import { FreightRateRuleType, FreightTableStatus, FuelGroupType, SourceRegionInputModel, TableType } from 'src/app/app.enum';



@Component({
    selector: 'app-freight',
    templateUrl: './freight.component.html',
    styleUrls: ['./freight.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    encapsulation: ViewEncapsulation.None
})

export class FreightComponent implements OnInit, OnChanges {
    @Input() PricingRuleType: number;
    @Input() IsEditable: boolean;
    @Output() onGenerateTable = new EventEmitter<any>();
    @Output() onGenerateFuelGroup = new EventEmitter<any>();
    @Output() onImportClick = new EventEmitter<any>();
    @ViewChild('btnCloseBulkUploadPopup') CloseBulkUploadPopup: ElementRef;

    public rcForm: FormGroup;

    public DtTrigger: Subject<any> = new Subject();

    public SingleSelectSettingsById = {};
    public MultiSelectSettingsById: IDropdownSettings;
    public MultiSelectSettingsByGroup = {};

    public isLoadingSubject: BehaviorSubject<any>;

    public IsLoaded: boolean = true;

    public Fsmodel: FreightPricingRulesViewModel;
    public CountryId: any;

    public TableTypeList: DropdownItem[];
    public FuelGroupsList: DropdownItem[];
    public CustomerList: DropdownItem[];
    public LocationList: DropdownItem[];
    public CarrierList: DropdownItem[];
    public SourceRegionList: DropdownItem[];
    public TerminalsAndBulkPlantList: DropdownItemExt[] = [];
    public SelectedTerminalsAndBulkPlants: DropdownItemExt[] = [];

    public IsCustomerSelected = false;
    public IsMasterSelected = false;
    public IsCarrierSelected = false;
    public IsSourceRegionSelected = false;

    public SelectedFiles: File[] = [];
    public IsShowBulkUploadPopup: boolean = false;
    public MaxFileUploadSize: number = 1048576; // 1MB
    public confirmButtonText: string = 'Yes';
    public cancelButtonText: string = 'No';
    public disableInputControls: boolean = false;

    public ShowMessage = false;

    public decimalSupportedRegx = /^(?:(?:0|[1-9][0-9]*)(?:\.[0-9]*)?|\.[0-9]+)$/;
    public intGreaterThanZeroRegx = /^[1-9][0-9]*$/;

    public MinStartDate = new Date();
    public MaxStartDate = new Date();
    public MinToDate = new Date();
    public MinFromDate = new Date();

    constructor(private fb: FormBuilder, private fuelsurchargeService: FuelSurchargeService, private freightRateRulesService: FreightRateRulesService,
        private regionService: RegionService, private externalMappingsService: ExternalMappingsService) { }

    ngOnInit() {
        this.isLoadingSubject = new BehaviorSubject(false);
        this.CountryId = MyLocalStorage.getData("countryIdForDashboard");
        this.SingleSelectSettingsById = {
            singleSelection: true,
            closeDropDownOnSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.MultiSelectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };

        this.MultiSelectSettingsByGroup = {
            singleSelection: false,
            text: "Select Terminals or Bulk Plants",
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            searchPlaceholderText: 'Search',
            primaryKey: "Id",
            labelKey: "Name",
            enableSearchFilter: true,
            badgeShowLimit: 5,
            groupBy: "Code"
        };

        var dt = moment(new Date()).toDate();
        this.rcForm = this.createForm();
        this.getTableTypes();
        this.rcForm.get('FuelGroupType').setValue(FuelGroupType.Standard);
        this.getFuelGroups(FuelGroupType.Standard, "");
        if (this.PricingRuleType != FreightRateRuleType.P2P) {
            this.rcForm.controls['TableTypes'].patchValue([{ Id: TableType.Master, Name: "Master" }]);// default will master
            this.IsMasterSelected = true;
        } else {
            this.rcForm.controls['TableTypes'].patchValue([{ Id: TableType.Customer, Name: "Customer Specific" }]);
            this.IsCustomerSelected = true;
            //this.IsMasterSelected = false;
        }
        this.getSourceRegions(TableType.Master.toString());

        merge(
            this.rcForm.get('Customers').valueChanges
        )
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]: [any, any]) => {
                if (this.IsLoaded && JSON.stringify(prev) != JSON.stringify(next)) this.CustomerChange(prev, next);
            });


        merge(
            this.rcForm.get('Locations').valueChanges
        )
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]: [any, any]) => {
                if (this.IsLoaded && JSON.stringify(prev) != JSON.stringify(next)) this.LocationChange(prev, next);
            });

        merge(
            this.rcForm.get('Carriers').valueChanges
        )
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]: [any, any]) => {
                if (this.IsLoaded && JSON.stringify(prev) != JSON.stringify(next)) this.CarrierChange(prev, next);
            });

        merge(
            this.rcForm.get('SourceRegions').valueChanges
        )
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]: [any, any]) => {
                if (this.IsLoaded && JSON.stringify(prev) != JSON.stringify(next)) this.SourceRegionChange(prev, next);
            });

        merge(
            this.rcForm.get('TerminalsAndBulkPlants').valueChanges
        )
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]: [any, any]) => {
                if (this.IsLoaded && JSON.stringify(prev) != JSON.stringify(next)) this.TerminalsAndBulkPlantsChange(prev, next);
            });


        merge(
            this.rcForm.get('FuelGroups').valueChanges
        )
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]: [any, any]) => {
                if (this.IsLoaded && JSON.stringify(prev) != JSON.stringify(next)) this.FuelGroupsChange(prev, next);
            });

        
        this.MaxStartDate.setFullYear(this.MaxStartDate.getFullYear() + 20);
        this.MinStartDate.setFullYear(this.MinStartDate.getFullYear() - 20);
        this.rcForm.get('StartDate').setValue(moment(dt).format('MM/DD/YYYY'));

    }


    ngOnChanges(change: SimpleChanges) {
        if (change.PricingRuleType && change.PricingRuleType.currentValue) {
            this.PricingRuleType = change.PricingRuleType.currentValue;
            if (change.PricingRuleType.previousValue && change.PricingRuleType.currentValue != change.PricingRuleType.previousValue) {
               
            }
        }

        if (change.IsEditable && change.IsEditable.currentValue) {
            this.IsEditable = change.IsEditable.currentValue;
            if (change.IsEditable.previousValue && change.IsEditable.currentValue != change.IsEditable.previousValue) {
                // todo : will progress
            }
        }

    }

    private createForm() {
        if (this.Fsmodel == undefined || this.Fsmodel == null) {
            this.Fsmodel = new FreightPricingRulesViewModel();
        }

        return this.fb.group({
            FuelGroupType: new FormControl(this.Fsmodel.FuelGroupType),
            TableTypeId: new FormControl(this.Fsmodel.TableTypeId),
            TableName: new FormControl(this.Fsmodel.TableName, Validators.required),
            TableTypes: new FormControl(this.Fsmodel.TableTypes, [Validators.required]),
            Customers: new FormControl(this.Fsmodel.Customers),
            Locations: new FormControl(this.Fsmodel.Locations),
            Carriers: new FormControl(this.Fsmodel.Carriers),
            FuelGroups: new FormControl(this.Fsmodel.FuelGroups, [Validators.required]),
            SourceRegions: new FormControl(this.Fsmodel.SourceRegions, [Validators.required]),
            TerminalsAndBulkPlants: new FormControl(this.SelectedTerminalsAndBulkPlants),
            StatusId: new FormControl(this.Fsmodel.StatusId),
            StartDate: new FormControl(this.Fsmodel.StartDate, [Validators.required]),
            EndDate: new FormControl(this.Fsmodel.EndDate),
            RangeStartValue: new FormControl('', [Validators.required, Validators.pattern(this.intGreaterThanZeroRegx)]),
            RangeEndValue: new FormControl('', [Validators.required, Validators.pattern(this.intGreaterThanZeroRegx)]),
            RangeInterval: new FormControl('', [Validators.required, Validators.pattern(this.intGreaterThanZeroRegx)])

        });
    }

    public setStartDate(event: any): void {
        this.rcForm.get('StartDate').setValue(event);
        if (this.rcForm.get('StartDate').value != null && this.rcForm.get('StartDate').value != undefined && this.rcForm.get('StartDate').value != "") {
            this.MinToDate = new Date(moment(this.rcForm.get('StartDate').value).format());
        }
    }

    public setEndDate(event: any): void {
        this.rcForm.get('EndDate').setValue(event);
    }
   

    public FuelGroupTypeChange(fgt: number): void {
        this.isLoadingSubject.next(true);
        //this.ViewOrEdit = "VIEW";
        this.rcForm.get('FuelGroups').patchValue([]);
        let selectedCustomers: DropdownItem[] = [];
        let selectedCarriers: DropdownItem[] = [];
        
        selectedCustomers = this.rcForm.get('Customers').value as DropdownItem[];        
        selectedCarriers = this.rcForm.get('Carriers').value as DropdownItem[];

        if (selectedCustomers.length > 1 || selectedCarriers.length > 1 || (selectedCustomers.length > 0 && selectedCarriers.length > 0)) {
            this.getFuelGroups(FuelGroupType.Standard, "");
        } else {
            this.getFuelGroups(fgt, selectedCustomers.length > 0 ? selectedCustomers.map(s => s.Id).join(',') : selectedCarriers.map(s => s.Id).join(','));
        }

        this.isLoadingSubject.next(false);

    }
    public onTableTypeSelect(item: any): void {
        this.IsMasterSelected = false;
        this.IsCustomerSelected = false;
        this.IsCarrierSelected = false;
        this.rcForm.get('Carriers').patchValue([]);
        this.rcForm.get('Customers').patchValue([]);
        this.rcForm.controls.TerminalsAndBulkPlants.patchValue([]);
        this.rcForm.controls.SourceRegions.patchValue([]);

        switch (item.Id) {
            case 1: //master
                this.IsMasterSelected = true;
                //this.IsCarrierSelected = true;
                //this.getCarriers();
                this.AddRemoveValidations([this.rcForm.get('TableTypes')], [this.rcForm.get('Customers'), this.rcForm.get('Carriers')]); //"Carriers,Customers"
                this.getSourceRegions(item.Id);
                this.rcForm.get('FuelGroupType').setValue(FuelGroupType.Standard);
                
                break;
            case 2: // customer
                this.getSupplierCustomers();
                this.getCarriers();
                this.IsCustomerSelected = true;
                this.AddRemoveValidations([this.rcForm.get('Customers'), this.rcForm.get('TableTypes')], [this.rcForm.get('Carriers')]);
                
                break;
            case 3: //carrier
                this.getCarriers();
                this.getSupplierCustomers();
                this.IsCarrierSelected = true;
                this.AddRemoveValidations([this.rcForm.get('Carriers'), this.rcForm.get('TableTypes')], [this.rcForm.get('Customers')]);
                
                break;
        }
       
        this.getFuelGroups(+this.rcForm.get('FuelGroupType').value, "");
       
    }

    public LocationChange(prev: any, next: any) {
        if (prev == null && next.length == 0) return;
        this.ShowMessage = true;
        this.rcForm.controls.TerminalsAndBulkPlants.patchValue([]);
        this.rcForm.controls.SourceRegions.patchValue([]);
        //this.rcForm.controls.FuelGroups.patchValue([]);
    }
    public CustomerChange(prev: any, next: any) {
        if (prev == null && next.length == 0) return;
        this.rcForm.controls.Locations.patchValue([]);
        this.rcForm.controls.TerminalsAndBulkPlants.patchValue([]);
        this.rcForm.controls.SourceRegions.patchValue([]);
        //this.rcForm.controls.FuelGroups.patchValue([]);
        //this.FuelGroupsList = [];

        let selectedCustomers = this.rcForm.get('Customers').value as DropdownItem[];
        let selectedCarriers = this.rcForm.get('Carriers').value as DropdownItem[];

        if (this.IsCustomerSelected) {
            this.getSourceRegions(TableType.Customer.toString()); 
        }
        if (this.IsCarrierSelected) {
            this.getSourceRegions(TableType.Carrier.toString());
        }        

        if (selectedCustomers.length > 1 || selectedCarriers.length > 1 || (selectedCustomers.length > 0 && selectedCarriers.length > 0)) {
            this.getFuelGroups(FuelGroupType.Standard, "");
        } else {
            this.getFuelGroups(+this.rcForm.get('FuelGroupType').value, selectedCustomers.length > 0 ? selectedCustomers.map(s => s.Id).join(',') : selectedCarriers.map(s => s.Id).join(','));
        }

        if (this.PricingRuleType == FreightRateRuleType.P2P) {
            if (selectedCustomers.length > 0) {
                this.freightRateRulesService.getCustomerJobs(selectedCustomers[0].Id).subscribe(async (data) => {
                    this.LocationList = await (data);
                    this.DtTrigger.next();
                    this.isLoadingSubject.next(false);
                });
            }
            this.AddRemoveValidations([this.rcForm.get('Locations')], null);
        } else {
            this.AddRemoveValidations(null, [this.rcForm.get('Locations')]);
        }

    }

    public CarrierChange(prev: any, next: any) {
        if (prev == null && next.length == 0) return;
        this.ShowMessage = true;
        this.CustomerChange(prev, next);
    }


    public FuelGroupsChange(prev: any, next: any) {
        if (prev == null && next.length == 0) return;
        this.ShowMessage = true;
        this.onGenerateFuelGroup.emit(true);
    }
    public TerminalsAndBulkPlantsChange(prev: any, next: any) {
        if (prev == null && next.length == 0) return;
        //this.rcForm.controls.FuelGroups.patchValue([]);
    }

    public SourceRegionChange(prev: any, next: any) {
        if (prev == null && next.length == 0) return;
        this.rcForm.controls.TerminalsAndBulkPlants.patchValue([]);
        //this.FuelGroupsList = [];
        this.IsSourceRegionSelected = false
        var ids = [];
        this.ShowMessage = true;

        let selectedSourceRegions = this.rcForm.get('SourceRegions').value as DropdownItem[];
        if (selectedSourceRegions.length > 0) {
            this.isLoadingSubject.next(true);
            selectedSourceRegions.forEach(s => ids.push(s.Id));
            this.fuelsurchargeService.getTerminalsAndBulkPlants(ids.join(',')).subscribe(async (data) => {
                this.TerminalsAndBulkPlantList = await (data);
                this.rcForm.controls.TerminalsAndBulkPlants.setValue(this.TerminalsAndBulkPlantList);
                this.IsSourceRegionSelected = true;
                this.DtTrigger.next();
                this.isLoadingSubject.next(false);
            });
        }

    }

    public ValidateOnSubmit(freightTableStatus: number): boolean {
        this.isLoadingSubject.next(true);
        this.ShowMessage = false;
        // publish or draft
        this.modeChangePublishORdraftValidators(freightTableStatus);
        this.markFormGroupTouched(this.rcForm)
        this.isLoadingSubject.next(false);

        if (!this.rcForm.valid) {
            //let selectedSourceRegion = this.rcForm.get('FuelGroups').value as DropdownItem[];
            this.ShowMessage = true;
        }

        return this.ShowMessage;
    }

    public ValidateOnGenearteRow(): boolean {
        this.isLoadingSubject.next(true);
        this.ShowMessage = false;
        // publish or draft
        this.clearAllValidations(this.rcForm); // clear all validation
        this.AddRemoveValidations([this.rcForm.controls.TableName], null); // minimum validation for all mode

        if (this.PricingRuleType == FreightRateRuleType.Range) {
            this.AddRemoveValidations([this.rcForm.controls.RangeStartValue], null);
            this.AddRemoveValidations([this.rcForm.controls.RangeEndValue],null);
            this.AddRemoveValidations([this.rcForm.controls.RangeInterval], null);
        }

        this.AddRemoveValidations([this.rcForm.controls.FuelGroups], null);

        this.AddRemoveValidations([this.rcForm.controls.TerminalsAndBulkPlants], null);
        if (this.PricingRuleType == FreightRateRuleType.P2P) {
            this.AddRemoveValidations([this.rcForm.controls.Locations], null);
            this.AddRemoveValidations([this.rcForm.controls.Customers], null);
        }

        this.rcForm.get('StartDate').setValidators([Validators.required]);
        this.rcForm.get('StartDate').updateValueAndValidity();
        this.rcForm.get('StartDate').markAsTouched();
        this.markFormGroupTouched(this.rcForm)
        this.isLoadingSubject.next(false);

        if (!this.rcForm.valid) {
            this.ShowMessage = true;
        }
        
        return this.ShowMessage;
    }

    public generateTable(): void {
        this.ValidateOnGenearteRow();
        this.rcForm.get('SourceRegions').setValidators([Validators.required]);
        this.rcForm.get('SourceRegions').updateValueAndValidity();
        this.onGenerateTable.emit(true);
    }



    private AddRemoveValidations(requiredControls: AbstractControl[], notRequiredControls: AbstractControl[]) {
        if (requiredControls != null && requiredControls != undefined && requiredControls.length > 0) {

            requiredControls.forEach(ctrl => {
                ctrl.setValidators([Validators.required]);
                ctrl.updateValueAndValidity();
            });
        }
        if (notRequiredControls != null && notRequiredControls != undefined && notRequiredControls.length > 0) {

            notRequiredControls.forEach(ctrl => {
                ctrl.clearValidators();
                ctrl.updateValueAndValidity();
            });
        }

    }

    private modeChangePublishORdraftValidators(statusId: number) {

        this.AddRemoveValidations([this.rcForm.controls.TableName], null); // minimum validation for all mode

        if (statusId == FreightTableStatus.Draft) {
            this.clearAllValidations(this.rcForm); // clear all validation
            this.AddRemoveValidations([this.rcForm.controls.TableName], null); // minimum validation for draft 
        }
        else if (statusId == FreightTableStatus.Published) {            
            this.AddRemoveValidations([this.rcForm.controls.FuelGroups], null);
            this.AddRemoveValidations([this.rcForm.controls.TerminalsAndBulkPlants], null);
            this.AddRemoveValidations(null, [this.rcForm.controls.RangeStartValue]);
            this.AddRemoveValidations(null, [this.rcForm.controls.RangeEndValue]);
            this.AddRemoveValidations(null, [this.rcForm.controls.RangeInterval]);
            if (this.PricingRuleType == FreightRateRuleType.P2P) {
                this.AddRemoveValidations([this.rcForm.controls.Locations], null);
                this.AddRemoveValidations([this.rcForm.controls.Customers], null);
            }

            this.rcForm.get('StartDate').setValidators([Validators.required]);
            this.rcForm.get('StartDate').updateValueAndValidity();
            this.rcForm.get('StartDate').markAsTouched();

        }
    }

    private clearAllValidations(formGroup: FormGroup) {
        (<any>Object).values(formGroup.controls).forEach(control => {
            control.clearValidators();
            control.updateValueAndValidity();
            control.markAsTouched();
            if (control.controls) {
                this.clearAllValidations(control);
            }
        });
    }

    private markFormGroupTouched(formGroup: FormGroup) {
        (<any>Object).values(formGroup.controls).forEach(control => {

            control.markAsTouched();
            if (control.controls) {
                this.markFormGroupTouched(control);
            }
        });
    }


    public ngOnDestroy(): void {
        this.DtTrigger.unsubscribe();
    }


    private getCarriers(): void {
        this.isLoadingSubject.next(true);
        this.regionService.getCarriers()
            .subscribe(async (carriers: DropdownItem[]) => {
                this.CarrierList = await carriers;
                this.DtTrigger.next();
                this.isLoadingSubject.next(false);
            });
    }

    private getTableTypes(): void {
        this.isLoadingSubject.next(true);
        this.fuelsurchargeService.getTableTypes().subscribe(async (data) => {
            if (this.PricingRuleType == FreightRateRuleType.P2P) {
               let tableType = await (data);
                this.TableTypeList = tableType.filter(s => s.Id == FreightRateRuleType.P2P)
            } else {
                this.TableTypeList = await (data);
            }
            this.DtTrigger.next();
            this.isLoadingSubject.next(false);
        });
    }

    private getSupplierCustomers(): void {
        this.isLoadingSubject.next(true);
        this.fuelsurchargeService.getSupplierCustomers().subscribe(async (data) => {
            this.CustomerList = await (data);
            this.DtTrigger.next();
            this.isLoadingSubject.next(false);
        });
    }
   
    private getFuelGroups(fuelGroupType:number, companyIds: string): void {
        this.isLoadingSubject.next(true);
        this.rcForm.get('FuelGroupType').setValue(fuelGroupType);
        this.externalMappingsService.getFuelGroups(fuelGroupType,companyIds).subscribe(async (data) => {
            this.FuelGroupsList = await (data);
            this.DtTrigger.next();
            this.isLoadingSubject.next(false);
        });
    }

    //companyIds : Based on tableType we will be call API , if tableType master or customer or carrier, full source region,customers,carriers loads will populated.
    private getSourceRegions(tableType: string): void {
        this.isLoadingSubject.next(true);
        let customerIds: number[] = [];
        let carrierIds: number[] = [];

        let selectedCustomers = this.rcForm.get('Customers').value as DropdownItem[];
        if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            customerIds = selectedCustomers.map(s => s.Id);
        }

        let selectedCarriers = this.rcForm.get('Carriers').value as DropdownItem[];
        if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            carrierIds = selectedCarriers.map(s => s.Id);
        }

        var sourceRegionInput = new SourceRegionInputModel();
        sourceRegionInput.TableType = tableType;
        sourceRegionInput.CustomerId = customerIds;
        sourceRegionInput.CarrierId = carrierIds;
        this.fuelsurchargeService.getSourceRegions(sourceRegionInput).subscribe(async (data) => {
            this.SourceRegionList = await (data);
            this.DtTrigger.next();
            this.isLoadingSubject.next(false);
        });
    }

    public importClick() {
        this.generateTable();
        if (!this.ShowMessage) this.IsShowBulkUploadPopup = true;
    }

    public closePopup() {
        this.IsShowBulkUploadPopup = false;
    }

    get IsRangePopupOpen(){
        let selectedFuelGroups = this.rcForm.get('FuelGroups').value as DropdownItem[];
       
        return selectedFuelGroups.length > 0 && !this.ShowMessage && (this.PricingRuleType == FreightRateRuleType.P2P || (this.rcForm.get('RangeStartValue').value > 0 && this.rcForm.get('RangeEndValue').value > 0 && this.rcForm.get('RangeInterval').value > 0));
        
    }

    public selectFiles(files: File[]) {
        if (files != null && files != undefined && files.length > 0)
            this.SelectedFiles = files;
    }

    private GenerateRange(start: number, end: number, step: number = 0) {
        let output = [];
        if (typeof end === 'undefined') {
            end = start;
            start = 0;
        }
        for (let i = start; i <= end; i += step) {
            output.push(i);
        }
        return output;
    }
    public downloadCsvTemplate() {
        this.isLoadingSubject.next(true);
        const a = document.createElement('a');
        var columnName = [];
        var template: string;
        if (this.PricingRuleType == FreightRateRuleType.Range) {
            columnName.push("Upto");
        }
        else if (this.PricingRuleType == FreightRateRuleType.P2P) {
            columnName.push("Terminal/Bulk Plants");
            columnName.push("Location Name");
            columnName.push("Location Address");
            columnName.push("LaneId");            
            columnName.push("Assumed Miles");
        }
        let selectedFuelGroups = this.rcForm.get('FuelGroups').value as DropdownItem[];
        let rep = ",";
        selectedFuelGroups.forEach(s => columnName.push(s.Name));
        template = columnName.join(',');
        if (this.PricingRuleType == FreightRateRuleType.P2P) {            
            template = template.concat(",IsLaneRequired") + "\n";
            var cols = [];
            let trbls = this.rcForm.get('TerminalsAndBulkPlants').value as DropdownItemExt[];
            let sLocations = this.rcForm.get('Locations').value as DropdownItem[];
            let JobIds: number[] = [];
            sLocations.forEach(s => JobIds.push(s.Id));
            let locs = this.LocationList.filter(this.IdInComparer(JobIds));
            for (var i = 0; i < trbls.length; i++) {
                for (var j = 0; j < locs.length; j++) {                  
                    template = template.concat(trbls[i].Id + "|" + trbls[i].Name + "," + locs[j].Id + "|" + locs[j].Name + "," + locs[j].Code + ',' + trbls[i].Id.split("_")[1] + " " + locs[j].Id ) + rep.repeat(selectedFuelGroups.length+2) + "1"  + "\n";
                }
               
            }
        }
        
        if (this.PricingRuleType == FreightRateRuleType.Range) {
            let start = this.rcForm.get('RangeStartValue').value;
            let stop = this.rcForm.get('RangeEndValue').value;
            let step = this.rcForm.get('RangeInterval').value;
            template = template + "\n";
            this.GenerateRange(+start, +stop, +step).forEach(res => {
                template = template + ",".repeat(selectedFuelGroups.length) + "\n";
            });
        }


        var blob = new Blob(["\ufeff", template]);
        const objectUrl = URL.createObjectURL(blob);
        a.href = objectUrl;
        if (this.PricingRuleType == FreightRateRuleType.Range) {
            a.download = 'FreightRate_Range_Table_Template.csv';
        }
        else if (this.PricingRuleType == FreightRateRuleType.P2P) {
            a.download = 'FreightRate_P2P_Table_Template.csv';
        }
        a.click();
        URL.revokeObjectURL(objectUrl);
        this.isLoadingSubject.next(false);
    }



    private getExtension(fileName) {
        // extract file name from full path ...
        var basename = fileName.split(/[\\/]/).pop();

        // (supports `\\` and `/` separators)
        var pos = basename.lastIndexOf(".");       // get last position of `.`

        if (basename === "" || pos < 1)            // if file name is empty or ...
            return "";                             //  `.` not found (-1) or comes first (0)

        return basename.slice(pos + 1);            // extract extension ignoring `.`
    }


    public uploadRangeTableFile() {
        var files = this.SelectedFiles;
        if (files.length === 0)
            return;

        const formData = new FormData();
        for (var file of files) {
            if (!this.isValidFile(file)) {
                return;
            }
            formData.append(file.name, file);
        }

        this.isLoadingSubject.next(true);

        let reader: FileReader = new FileReader();
        reader.readAsText(file);
        reader.onload = (e) => {
            this.CloseBulkUploadPopup.nativeElement.click();
            this.onImportClick.emit(reader.result as string);
            this.SelectedFiles = [];
            this.isLoadingSubject.next(false);
        }
        
    }

    public isValidFile(file: File) {
        var isValid = true;
        var extension = this.getExtension(file.name);
        if (extension == undefined || extension == null || extension == '' || extension.toLowerCase() != 'csv') {
            Declarations.msgerror('Invalid file, only csv files are allowed', undefined, undefined);
            isValid = false;
            return isValid;
        }

        if (file.size > this.MaxFileUploadSize) {
            Declarations.msgerror('Invalid file size, file size should not be greater than 1 MB', undefined, undefined);
            isValid = false;
            return isValid;
        }

        return isValid;
    }

    private IdInComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other == current.Id
            }).length == 1;
        }
    }
}


