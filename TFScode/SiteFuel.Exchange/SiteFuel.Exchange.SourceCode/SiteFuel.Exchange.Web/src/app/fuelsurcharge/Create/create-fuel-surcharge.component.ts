import { Component, OnInit, Input, ChangeDetectorRef } from '@angular/core';
import { FuelSurchargeIndexViewModel } from 'src/app/fuelsurcharge/models/CreateFuelSurcharge';
import { FuelSurchargeService } from 'src/app/fuelsurcharge/services/fuelsurcharge.service'
import { Subject, forkJoin, merge, BehaviorSubject } from 'rxjs';
import { distinctUntilChanged, map, mergeMap } from 'rxjs/operators';
import { FormBuilder, FormGroup, Validators, FormControl, FormArray, AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { RegionService } from 'src/app/company-addresses/region/service/region.service';
import { DropdownItem, DropdownItemExt } from 'src/app/statelist.service';
import { Declarations } from 'src/app/declarations.module';
import * as moment from 'moment';
import { HttpClient } from '@angular/common/http';
import { pairwise, startWith } from 'rxjs/operators';
import { CarrierService } from '../../carrier/service/carrier.service';
import { AnnualyEnum, FreightTableStatus, MonthEnum, PeriodEnum, SourceRegionInputModel, TableType, WeekDays, WeekEnum } from 'src/app/app.enum';
declare var currentUserCompanyId: any;

@Component({
    selector: 'app-create-fuel-surcharge',
    templateUrl: './create-fuel-surcharge.component.html',
    styleUrls: ['./create-fuel-surcharge.component.css']
})
export class CreateFuelSurchargeComponent implements OnInit {

    public rcForm: FormGroup;
    public DtTrigger: Subject<any> = new Subject();
    //public isLoadingSubject: BehaviorSubject<any>;

    public SingleSelectSettingsById = {};
    public MultiSelectSettingsById: IDropdownSettings;
    public MultiSelectSettingsByGroup = {};

    public IsLoading: boolean = false;
    public Fsmodel: FuelSurchargeIndexViewModel;
    public CurrentCompanyId: number;
    public SelectedCountryId: number = -1;

    public TableTypeList: DropdownItem[];
    public CustomerList: DropdownItem[];
    public CarrierList: DropdownItem[];
    public SourceRegionList: DropdownItem[];
    public TerminalsAndBulkPlantList: DropdownItemExt[] = [];
    public FuelSurchargeProductList: DropdownItemExt[];
    public FuelSurchargePeriodList: DropdownItemExt[];
    public FuelSurchargeAreaList: DropdownItemExt[];
    public APILatestIndexPrice: number;

    public IsEditLoaded = true;
    public IsCustomerSelected = false;
    public IsMasterSelected = false;
    public IsCarrierSelected = false;
    public IsSourceRegionSelected:boolean = false;

    public IsMonthlyVisible = false;
    public IsWeeklyVisible = false;
    public IsAnnualyVisible = false;

    public IsGeneratedSurchargeTable = false;
    public ShowMessage = false;

    public ServiceResponse: any;
    public viewType = 1;

    public fuelsurchargeId?: number;
    public fuelsurchargeMode?: string;
    public disableInputControls: boolean = false;
    public AllowTableName: boolean;
    //min max date
    public MinStartDate = new Date();
    public MaxStartDate = new Date();
    public MinToDate = new Date();
    public MinFromDate = new Date();
    public decimalSupportedRegx = /^(?:(?:0|[1-9][0-9]*)(?:\.[0-9]*)?|\.[0-9]+)$/;
    public SelectedTerminalsAndBulkPlants: DropdownItemExt[] = [];
    public WeekList: DropdownItemExt[];
    public MonthList: DropdownItemExt[];
    public SourceMonthList: DropdownItemExt[];
    public AnnualyList: DropdownItemExt[];
    public SourceAnnualyList: DropdownItemExt[];

    constructor(private fb: FormBuilder, private fuelsurchargeService: FuelSurchargeService,
        private regionService: RegionService, private http: HttpClient, private carrierService: CarrierService,private cdr:ChangeDetectorRef) {
    }

    ngOnInit() {
        //this.isLoadingSubject = new BehaviorSubject(false);
        this.CurrentCompanyId = Number(currentUserCompanyId);
        this.getDefaultServingCountry();

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
            closeDropDownOnSelection: true,
            text: "Select Terminal(s) and Bulk Plant(s)",
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            searchPlaceholderText: 'Search',
            primaryKey: "Id",
            labelKey: "Name",
            enableSearchFilter: true,
            badgeShowLimit: 5,
            groupBy: "Code"
        };

        this.rcForm = this.createForm();
        this.getTableTypes();
        this.rcForm.controls['TableTypes'].patchValue([{ Id: TableType.Master, Name: "Master" }]);// default will master
        this.IsMasterSelected = true;
        this.getSourceRegions(TableType.Master.toString());

        var dt = moment(new Date()).toDate();
        this.MaxStartDate.setFullYear(this.MaxStartDate.getFullYear() + 20);
        this.MinFromDate.setFullYear(this.MinFromDate.getFullYear() - 20);
        this.rcForm.controls.IndexPriceDate.setValue(moment(dt).format('MM/DD/YYYY'));
        //this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(moment(dt).format('MM/DD/YYYY'));
        //default view type =1 so need to add required validations.
        this.AddRemoveValidations([this.rcForm.get('FuelSurchargeProducts'), this.rcForm.get('FuelSurchargePeriods'), this.rcForm.get('FuelSurchargeAreas'), this.rcForm.get('ApiAdjustIndexPriceDate')],
            [this.rcForm.get('ManualLatestIndexPrice'), this.rcForm.get('ManualEffectiveDate')]);

        this.fuelsurchargeService.onSelectedFuelSurchargeId.subscribe(s => {
            if (s) {
                let stringify = JSON.parse(s);
                this.fuelsurchargeId = stringify.FsurcharId;
                this.fuelsurchargeMode = stringify.Mode;
            }
        });

        // with order page integration
        let id = localStorage.getItem("FuelSurchargeTabId");
        if (id && +id > 0) {
            this.fuelsurchargeId = Number(id);
            this.fuelsurchargeMode = "VIEW";
            localStorage.removeItem("FuelSurchargeTabId");
        }


        var WeekList: DropdownItemExt[] = [];
        var MonthList: DropdownItemExt[] = [];
        var sourceMonthList: DropdownItemExt[] = [];
        var AnnualyList: DropdownItemExt[] = [];
        var sourceAnnualyList: DropdownItemExt[] = [];

        for (let element in WeekEnum) {
            WeekList.push({ Id: element, Name: WeekEnum[element], Code: "" });
        }
        this.WeekList = WeekList;
        this.rcForm.controls['Weeks'].setValue(this.WeekList.slice(0, 1));

        for (let element in MonthEnum) {

            MonthList.push({ Id: element, Name: MonthEnum[element], Code: "" });
        }

        this.MonthList = MonthList;
        this.rcForm.controls['Months'].setValue(this.MonthList.slice(0, 1)); 
        this.IsWeeklyVisible = true;
       
        for (let i = 6; i >=-6; i--) {
            let m = new Date().setMonth(new Date().getMonth() + i, 1);
            sourceMonthList.push({ Id: moment(m).format(), Name: moment(m).format('MMMM YYYY'), Code: ""});
        }

        this.SourceMonthList = sourceMonthList;
        
        this.rcForm.controls['SourceMonths'].setValue(this.SourceMonthList.slice(5, 6));
       
        for (let i = 1; i >=-1; i--) {
            let y = new Date().setFullYear(new Date().getFullYear() + i, 1);
            sourceAnnualyList.push({ Id: moment(y).format(), Name: moment(y).format('YYYY'), Code: "" });
        }

        this.SourceAnnualyList = sourceAnnualyList;
        this.rcForm.controls['SourceAnnualy'].setValue(this.SourceAnnualyList.slice(0, 1));
       
        for (let element in AnnualyEnum) {

            AnnualyList.push({ Id: element, Name: AnnualyEnum[element], Code: "" });
        }

        this.AnnualyList = AnnualyList;
        this.rcForm.controls['Annualy'].setValue(this.AnnualyList.slice(0, 1));

           
        this.rcForm.get('FuelSurchargePeriods').valueChanges.pipe(distinctUntilChanged())
            .subscribe(([prev, next]: [any, any]) => {
                if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded) this.setValidFromDate();
            });
        this.rcForm.get('ApiAdjustIndexPriceDate').valueChanges.pipe(distinctUntilChanged())
            .subscribe(([prev, next]: [any, any]) => {
                if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded) this.setValidFromDate();
            });
        this.rcForm.get('WeekDay').valueChanges.pipe(distinctUntilChanged())
            .subscribe(([prev, next]: [any, any]) => {
                if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded) this.setValidFromDate();
            });
            this.rcForm.get('Weeks').valueChanges.pipe(distinctUntilChanged())
            .subscribe(([prev, next]: [any, any]) => {
                if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded) this.setValidFromDate();
            });
        this.rcForm.get('Annualy').valueChanges.pipe(distinctUntilChanged())
            .subscribe(([prev, next]: [any, any]) => {
                if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded) this.setValidFromDate();
            });
        this.rcForm.get('SourceAnnualy').valueChanges.pipe(distinctUntilChanged())
            .subscribe(([prev, next]: [any, any]) => {
                if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded) this.setValidFromDate();
            });
        this.rcForm.get('Months').valueChanges.pipe(distinctUntilChanged())
            .subscribe(([prev, next]: [any, any]) => {
                if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded) this.setValidFromDate();
            });
            this.rcForm.get('SourceMonths').valueChanges.pipe(distinctUntilChanged())
            .subscribe(([prev, next]: [any, any]) => {
                if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded) this.setValidFromDate();
            });

        merge(
            this.rcForm.get('SourceRegions').valueChanges
        )
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]: [any, any]) => {
                if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded) this.getTerminalsBulkPlant();
            });

        merge(
            this.rcForm.get('Carriers').valueChanges
        )
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]: [any, any]) => {
                if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded) this.onCarriersChange(prev, next);
            });

        merge(
            this.rcForm.get('Customers').valueChanges
        )
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]: [any, any]) => {
                if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded) this.onCustomersChange(prev, next);
            });

        merge(
            this.rcForm.get('SourceRegions').valueChanges
        )
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]: [any, any]) => {
                if (JSON.stringify(prev) != JSON.stringify(next) && this.IsEditLoaded) this.SourceRegionChange(prev, next);
            });

        if (this.SelectedCountryId == 2) {
            this.changeViewType(this.SelectedCountryId);
        }
    }

    ngAfterViewInit() {
        if (this.fuelsurchargeId != null && this.fuelsurchargeId != undefined) {
            //this.isLoadingSubject = new BehaviorSubject(false);
            this.IsEditLoaded = false;            
            this.getFuelSurchargeTable(this.fuelsurchargeId); //existing fuel charge.
        }
    }

    getDefaultServingCountry() {
        this.carrierService.getDefaultServingCountry(this.CurrentCompanyId).subscribe(data => {
            this.SelectedCountryId = Number(data.DefaultCountryId);
            this.getFuelSurchargeProducts(this.SelectedCountryId);
            this.getFuelSurchargePeriods(this.SelectedCountryId);
            this.getFuelSurchargeArea(this.SelectedCountryId);
        });
    }

    private modeChangeApiORmanualValidators(IsManualUpdate: boolean) {
        if (!IsManualUpdate) {
            var selectedTableType = this.rcForm.controls['TableTypes'].value as DropdownItem[];
            this.Fsmodel.TableTypeId = selectedTableType[0].Id;
            if (selectedTableType[0].Id == TableType.Master) {
                this.AddRemoveValidations([this.rcForm.get('TableTypes')], [this.rcForm.get('Customers'), this.rcForm.get('Carriers')]) //, this.rcForm.get('Carriers')
            } else if (selectedTableType[0].Id == TableType.Customer) {
                this.AddRemoveValidations([this.rcForm.get('Customers')], [this.rcForm.get('Carriers')]);
            } else {
                this.AddRemoveValidations([this.rcForm.get('Carriers')], [this.rcForm.get('Customers')]);
            }
            
            this.AddRemoveValidations([this.rcForm.get('SourceRegions'), this.rcForm.get('TableTypes'), this.rcForm.get('FuelSurchargeProducts'), this.rcForm.get('FuelSurchargePeriods'), this.rcForm.get('FuelSurchargeAreas')],
                [this.rcForm.get('ManualLatestIndexPrice'), this.rcForm.get('ManualEffectiveDate')]);

            if (this.IsWeeklyVisible) {
                this.AddRemoveValidations([this.rcForm.get('ApiAdjustIndexPriceDate')], [this.rcForm.get('SourceAnnualy'), this.rcForm.get('SourceMonths')]);
            } else if (this.IsMonthlyVisible) {
                this.AddRemoveValidations([this.rcForm.get('SourceMonths')], [this.rcForm.get('ApiAdjustIndexPriceDate'), this.rcForm.get('SourceAnnualy')]);
            } else if (this.IsAnnualyVisible) {
                this.AddRemoveValidations([this.rcForm.get('SourceAnnualy')], [this.rcForm.get('ApiAdjustIndexPriceDate'), this.rcForm.get('SourceMonths')]);
            }
        }
        if (IsManualUpdate) {
            this.rcForm.get('ManualLatestIndexPrice').setValidators([Validators.required, Validators.pattern(this.decimalSupportedRegx)]);
            this.rcForm.get('ManualLatestIndexPrice').updateValueAndValidity();
            this.AddRemoveValidations([this.rcForm.get('ManualEffectiveDate')],
                [this.rcForm.get('FuelSurchargeProducts'), this.rcForm.get('FuelSurchargePeriods'), this.rcForm.get('FuelSurchargeAreas'), this.rcForm.get('ApiAdjustIndexPriceDate'), this.rcForm.get('SourceMonths'), this.rcForm.get('SourceAnnualy')]);
        }
    }

    private modeChangePublishORdraftValidators(statusId: number) {
        this.AddRemoveValidations([this.rcForm.controls.TableName], null); // minimum validation for all mode

        if (statusId == FreightTableStatus.Draft) {
            this.clearAllValidations(this.rcForm); // clear all validation
            this.AddRemoveValidations([this.rcForm.controls.TableName], null); // minimum validation for draft 
        }
        else if (statusId == FreightTableStatus.Published) {
            this.modeChangeApiORmanualValidators(this.rcForm.get('IsManualUpdate').value);
            this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValidators([Validators.required]);
            this.rcForm.controls.FuelSurchargeTable.get('StartDate').updateValueAndValidity();
            this.rcForm.controls.FuelSurchargeTable.get('PriceRangeStartValue').setValidators([Validators.required, Validators.pattern(this.decimalSupportedRegx)]);
            this.rcForm.controls.FuelSurchargeTable.get('PriceRangeStartValue').updateValueAndValidity();
            this.rcForm.controls.FuelSurchargeTable.get('PriceRangeEndValue').setValidators([Validators.required, Validators.pattern(this.decimalSupportedRegx)]);
            this.rcForm.controls.FuelSurchargeTable.get('PriceRangeEndValue').updateValueAndValidity();
            this.rcForm.controls.FuelSurchargeTable.get('PriceRangeInterval').setValidators([Validators.required, Validators.pattern(this.decimalSupportedRegx)]);
            this.rcForm.controls.FuelSurchargeTable.get('PriceRangeInterval').updateValueAndValidity();
            this.rcForm.controls.FuelSurchargeTable.get('FuelSurchargeStartPercentage').setValidators([Validators.required, Validators.pattern(this.decimalSupportedRegx)]);
            this.rcForm.controls.FuelSurchargeTable.get('FuelSurchargeStartPercentage').updateValueAndValidity();
            this.rcForm.controls.FuelSurchargeTable.get('SurchargeInterval').setValidators([Validators.required, Validators.pattern(this.decimalSupportedRegx)]);
            this.rcForm.controls.FuelSurchargeTable.get('SurchargeInterval').updateValueAndValidity();
        }
    }

    public changeViewType(value) {
        this.viewType = value;
        this.rcForm.get('IsManualUpdate').setValue(value == 1 ? false : true);
        this.modeChangeApiORmanualValidators(value == 1 ? false : true);
    }

    private createForm() {
        if (this.Fsmodel == undefined || this.Fsmodel == null) {
            this.Fsmodel = new FuelSurchargeIndexViewModel();
        }

        return this.fb.group({
            IsManualUpdate: new FormControl(this.Fsmodel.IsManualUpdate),
            ProductId: new FormControl(this.Fsmodel.ProductId),
            PeriodId: new FormControl(this.Fsmodel.PeriodId),
            TableTypeId: new FormControl(this.Fsmodel.TableTypeId),
            AreaId: new FormControl(this.Fsmodel.AreaId),
            FuelSurchargeIndexId: new FormControl(this.Fsmodel.FuelSurchargeIndexId),
            TableName: new FormControl(this.Fsmodel.TableName, Validators.required),
            Notes: new FormControl(this.Fsmodel.Notes),
            IndexPriceDate: new FormControl(this.Fsmodel.IndexPriceDate),
            TableTypes: new FormControl(this.Fsmodel.TableTypes, [Validators.required]),
            Customers: new FormControl(this.Fsmodel.Customers),
            Carriers: new FormControl(this.Fsmodel.Carriers),
            SourceRegions: new FormControl(this.Fsmodel.SourceRegions, [Validators.required]),
            WeekDay: new FormControl(this.Fsmodel.WeekDay),
            Weeks: new FormControl(this.Fsmodel.Weeks),
            Months: new FormControl(this.Fsmodel.Months),
            SourceMonths: new FormControl(this.Fsmodel.SourceMonths),
            Annualy: new FormControl(this.Fsmodel.Annualy),
            SourceAnnualy: new FormControl(this.Fsmodel.SourceAnnualy),
            ApiEffectiveDate: new FormControl(''),
            TerminalsAndBulkPlants: new FormControl(this.SelectedTerminalsAndBulkPlants),
            FuelSurchargeProducts: new FormControl(this.Fsmodel.FuelSurchargeProducts),
            FuelSurchargePeriods: new FormControl(this.Fsmodel.FuelSurchargePeriods),
            FuelSurchargeAreas: new FormControl(this.Fsmodel.FuelSurchargeAreas),
            APILatestIndexPrice: new FormControl(this.Fsmodel.APILatestIndexPrice),
            ApiAdjustIndexPriceDate: new FormControl(this.Fsmodel.ApiAdjustIndexPriceDate),
            ManualLatestIndexPrice: new FormControl(this.Fsmodel.ManualLatestIndexPrice),
            ManualEffectiveDate: new FormControl(this.Fsmodel.ManualEffectiveDate),
            StatusId: new FormControl(this.Fsmodel.StatusId),
            FuelSurchargeTable: new FormGroup({
                StartDate: new FormControl(this.Fsmodel.FuelSurchargeTable.StartDate, [Validators.required]),
                EndDate: new FormControl(this.Fsmodel.FuelSurchargeTable.EndDate),
                PriceRangeStartValue: new FormControl(this.Fsmodel.FuelSurchargeTable.PriceRangeStartValue, [Validators.required, Validators.pattern(this.decimalSupportedRegx)]), //, this.lowerThan('PriceRangeEndValue')
                PriceRangeEndValue: new FormControl(this.Fsmodel.FuelSurchargeTable.PriceRangeEndValue, [Validators.required, Validators.pattern(this.decimalSupportedRegx)]),
                PriceRangeInterval: new FormControl(this.Fsmodel.FuelSurchargeTable.PriceRangeInterval, [Validators.required, Validators.pattern(this.decimalSupportedRegx)]),
                FuelSurchargeStartPercentage: new FormControl(this.Fsmodel.FuelSurchargeTable.FuelSurchargeStartPercentage, [Validators.required, Validators.pattern(this.decimalSupportedRegx)]),
                SurchargeInterval: new FormControl(this.Fsmodel.FuelSurchargeTable.SurchargeInterval, [Validators.required, Validators.pattern(this.decimalSupportedRegx)])
            }),
            GeneratedSurchargeTable: new FormArray([
                new FormControl(this.Fsmodel.GeneratedSurchargeTable)
            ])
        }, { validator: RangeValidator });
    }

    //#region start : calander related functionality 
    public setApiAdjustIndexPriceDate(event: any): void {
        this.IsMonthlyVisible = false;
        this.IsWeeklyVisible = false;
        this.IsAnnualyVisible = false;
        if (event!="") this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(event);
        if (this.rcForm.controls.ApiAdjustIndexPriceDate.value != "" && this.rcForm.controls.ApiAdjustIndexPriceDate.value != null && this.rcForm.controls.ApiAdjustIndexPriceDate != undefined) {
            let selectedPeriod = this.rcForm.get('FuelSurchargePeriods').value as DropdownItemExt[];
            if (selectedPeriod != null && selectedPeriod != undefined && selectedPeriod.length > 0) {
                if (selectedPeriod[0].Name.toLowerCase() ==  "Weekly".toLowerCase()) {
                    this.IsWeeklyVisible = true;
                }
                else if (selectedPeriod[0].Name.toLowerCase() == "Monthly".toLowerCase()) {
                    this.IsMonthlyVisible = true;
                }

                else if (selectedPeriod[0].Name.toLowerCase() == "Annualy".toLowerCase()) {
                    this.IsAnnualyVisible = true;
                }
                this.setValidFromDate();
            }
        }
    }

    public onFuelSurchargePeriodsSelect(item: any): void {
        this.IsMonthlyVisible = false;
        this.IsWeeklyVisible = false;
        this.IsAnnualyVisible = false;
        //
       // if (this.rcForm.controls.ApiAdjustIndexPriceDate.value != null && this.rcForm.controls.ApiAdjustIndexPriceDate != undefined) {
            let selectedPeriod = this.rcForm.get('FuelSurchargePeriods').value as DropdownItemExt[];
            if (selectedPeriod[0].Name.toLowerCase() == "Weekly".toLowerCase()) {
                this.IsWeeklyVisible = true;
                this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(moment().weekday(1).format('MM/DD/YYYY'));
            }
            if (selectedPeriod[0].Name.toLowerCase() == "Monthly".toLowerCase()) {
                this.IsMonthlyVisible = true;
                this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(moment().startOf('month').format('MM/DD/YYYY'));
            }
            if (selectedPeriod[0].Name.toLowerCase() == "Annualy".toLowerCase()) {
                this.IsAnnualyVisible = true;
                this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(moment().startOf('year').format('MM/DD/YYYY'));
            }
        //}
    }


    public setManualEffectiveDate(event: any): void {
        if (event != "") {
            this.rcForm.controls.ManualEffectiveDate.setValue(event);
            this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValue(moment(event).format('MM/DD/YYYY'));
        }
    }

    public setStartDate(event: any): void {
        this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValue(event);
        if (this.rcForm.controls.FuelSurchargeTable.get('StartDate').value != null && this.rcForm.controls.FuelSurchargeTable.get('StartDate').value != undefined && this.rcForm.controls.FuelSurchargeTable.get('StartDate').value != "")
            this.MinToDate = this.rcForm.controls.FuelSurchargeTable.get('StartDate').value;
    }

    public setEndDate(event: any): void {
        this.rcForm.controls.FuelSurchargeTable.get('EndDate').setValue(event);
    }
    // end : calander related functionality

    public onTableTypeDeSelect(item: any): void {
        var selectedTableType = this.rcForm.get('TableTypes').value as DropdownItem[];
        if (selectedTableType.length == 0) {
            this.IsMasterSelected = true;
            this.rcForm.get('Carriers').patchValue([]);
            this.rcForm.get('Customers').patchValue([]);
            this.rcForm.get('TerminalsAndBulkPlants').patchValue([]);
            this.rcForm.get('SourceRegions').patchValue([]);
            this.IsSourceRegionSelected = false;
            this.AddRemoveValidations([this.rcForm.get('TableTypes')], [this.rcForm.get('Customers'),
                this.rcForm.get('Carriers')]);
        }
    }

    public onTableTypeSelect(item: any): void {
        this.IsMasterSelected = false;
        this.IsCustomerSelected = false;
        this.IsCarrierSelected = false;
        this.rcForm.get('Carriers').patchValue([]);
        this.rcForm.get('Customers').patchValue([]);
        
        switch (item.Id) {
            case 1: //master
                this.IsMasterSelected = true;
                this.AddRemoveValidations([this.rcForm.get('TableTypes')], [this.rcForm.get('Customers'), this.rcForm.get('Carriers')]); //"Carriers,Customers"
                break;
            case 2: // customer
                this.getCarriers();
                this.getSupplierCustomers();
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
        this.rcForm.get('SourceRegions').patchValue([]);
        this.getSourceRegions(item.Id);
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

    public onCarriersChange(prev: any, next: any): void {
        this.onCarriersOrCustomersChange(prev, next);
    }

    public onCustomersChange(prev: any, next: any): void {
        this.onCarriersOrCustomersChange(prev, next);
    }

    public onCarriersOrCustomersChange(prev: any, next: any): void {
        if (this.IsMasterSelected) return;
        this.rcForm.get('SourceRegions').patchValue([]);
        var selectedTableType = this.rcForm.get('TableTypes').value as DropdownItem[];
        this.getSourceRegions(selectedTableType[0].Id.toString());         
    }
   
    public onFetchLastIndexPrice() {
        var selectedArea = this.rcForm.get('FuelSurchargeAreas').value as DropdownItemExt[];
        var selectedProduct = this.rcForm.get('FuelSurchargeProducts').value as DropdownItem[];
        var selectedPeriod = this.rcForm.get('FuelSurchargePeriods').value as DropdownItemExt[];
        //if (selectedProduct != undefined && selectedPeriod != undefined && selectedProduct.length == 1 && selectedPeriod.length == 1)
        //    this.getFuelIndexPrice(selectedPeriod[0].Code, selectedProduct[0].Code, moment().format("MM-DD-YYYY"));
        if (selectedArea != undefined && selectedProduct != undefined && selectedPeriod != undefined && selectedArea != undefined && selectedProduct.length == 1 && selectedPeriod.length == 1 && selectedArea.length == 1) {
            var selectedAreaCode = this.FuelSurchargeAreaList.filter(x => x.Id == selectedArea[0].Id)[0].Code;
            var selectedPeriodCode = this.FuelSurchargePeriodList.filter(x => x.Id == selectedPeriod[0].Id)[0].Code;
            this.getFuelIndexPrice(selectedPeriodCode, selectedProduct[0].Code, moment().format("MM-DD-YYYY"), selectedAreaCode);
        }
    }

    public onGenerateSurchargeTable(): void {
        this.ShowMessage = false;
        this.IsGeneratedSurchargeTable = false;
        var gst = this.rcForm.controls['GeneratedSurchargeTable'] as FormArray
        var fst = this.rcForm.controls['FuelSurchargeTable'] as FormGroup
        gst.clear();
        this.modeChangePublishORdraftValidators(FreightTableStatus.Published);
        this.modeChangeApiORmanualValidators(this.rcForm.get("IsManualUpdate").value);
        this.markFormGroupTouched(this.rcForm)

        if (!fst.valid) return;
        this.IsLoading = true;
        this.fuelsurchargeService.getGenerateSurchargeTable(fst.get('PriceRangeStartValue').value,
            fst.get('PriceRangeEndValue').value, fst.get('PriceRangeInterval').value,
            fst.get('SurchargeInterval').value, fst.get('FuelSurchargeStartPercentage').value).subscribe(async (data) => {
                var dt = await (data);
                dt.forEach(res => {
                    gst.push(new FormControl({
                        FuelSurchargeStartPercentage: res.FuelSurchargeStartPercentage,
                        PriceRangeStartValue: res.PriceRangeStartValue,
                        PriceRangeEndValue: res.PriceRangeEndValue
                    }))
                });

                this.DtTrigger.next();
                this.IsLoading = false;
            });

        this.IsGeneratedSurchargeTable = true;
    }

    private markFormGroupTouched(formGroup: FormGroup) {
        (<any>Object).values(formGroup.controls).forEach(control => {

            control.markAsTouched();
            if (control.controls) {
                this.markFormGroupTouched(control);
            }
        });
    }

    private clearAllControlValue(formGroup: FormGroup) {
        (<any>Object).values(formGroup.controls).forEach(control => {

            control.patchValue([]);;
            if (control.controls) {
                this.clearAllControlValue(control);
            }
        });
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

    public onSubmit(fuelSurchageStatus: number): void {
        this.ShowMessage = false;
        this.rcForm.get('StatusId').setValue(fuelSurchageStatus);
        this.modeChangeApiORmanualValidators(this.rcForm.get("IsManualUpdate").value);
        this.modeChangePublishORdraftValidators(fuelSurchageStatus);
        this.markFormGroupTouched(this.rcForm)
        if (!this.IsGeneratedSurchargeTable && fuelSurchageStatus == 2) {
            this.ShowMessage = true;
            return;
        }

        if (this.rcForm.valid) {
            this.Save(fuelSurchageStatus);
        }
    }
    public clearForm() {

        this.rcForm.get('TableName').patchValue('');   
        this.rcForm.get('SourceRegions').patchValue(''); 
        this.rcForm.get('TerminalsAndBulkPlants').patchValue('');  

        this.rcForm.controls.FuelSurchargeTable.get('EndDate').patchValue('');
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeStartValue').patchValue('');
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeEndValue').patchValue('');
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeInterval').patchValue('');
        this.rcForm.controls.FuelSurchargeTable.get('FuelSurchargeStartPercentage').patchValue('');
        this.rcForm.controls.FuelSurchargeTable.get('SurchargeInterval').patchValue('');
        this.rcForm.get('ApiAdjustIndexPriceDate').patchValue('');
        this.AllowTableName = true
        this.disableInputControls = false;
        this.IsGeneratedSurchargeTable = false;

        this.rcForm.get('TableName').clearValidators();
        this.rcForm.get('TableName').updateValueAndValidity();
        this.rcForm.get('SourceRegions').clearValidators(); 
        this.rcForm.get('SourceRegions').updateValueAndValidity();
        this.rcForm.get('ApiAdjustIndexPriceDate').clearValidators();
        this.rcForm.get('ApiAdjustIndexPriceDate').updateValueAndValidity();
        this.rcForm.controls.FuelSurchargeTable.get('EndDate').clearValidators();
        this.rcForm.controls.FuelSurchargeTable.get('EndDate').updateValueAndValidity();
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeStartValue').clearValidators();
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeStartValue').updateValueAndValidity();
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeEndValue').clearValidators();
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeEndValue').updateValueAndValidity();
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeInterval').clearValidators();
        this.rcForm.controls.FuelSurchargeTable.get('PriceRangeInterval').updateValueAndValidity();
        this.rcForm.controls.FuelSurchargeTable.get('FuelSurchargeStartPercentage').clearValidators();
        this.rcForm.controls.FuelSurchargeTable.get('FuelSurchargeStartPercentage').updateValueAndValidity();
        this.rcForm.controls.FuelSurchargeTable.get('SurchargeInterval').clearValidators();
        this.rcForm.controls.FuelSurchargeTable.get('SurchargeInterval').updateValueAndValidity();
       
    }

    public onCancel() {
        if (this.fuelsurchargeMode!=null) {
            this.changeToViewTab();
        }
        else {
            this.clearAllControlValue(this.rcForm)
            this.rcForm.controls['TableTypes'].setValue(this.TableTypeList.filter(x => x.Id == TableType.Master));// default will master
            this.rcForm.controls['FuelSurchargeProducts'].setValue(this.FuelSurchargeProductList.slice(0, 1));
            this.rcForm.controls['FuelSurchargeAreas'].setValue(this.FuelSurchargeAreaList.slice(0, 1));

            this.IsGeneratedSurchargeTable = false;
            this.IsCustomerSelected = false;
            this.IsMasterSelected = true;
            this.IsCarrierSelected = false;
            this.IsSourceRegionSelected = false;

            this.IsAnnualyVisible = false;
            this.IsMonthlyVisible = false;
            this.rcForm.get('SourceMonths').setValue(null);
            this.rcForm.get('SourceAnnualy').setValue(null);
            
            this.rcForm.controls['WeekDay'].setValue("Mon");
            this.rcForm.controls['Weeks'].setValue(this.WeekList.slice(0, 1));
            this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(moment().weekday(1).format('MM/DD/YYYY'));
            this.rcForm.controls['FuelSurchargePeriods'].setValue(this.FuelSurchargePeriodList.slice(0, 1));            
            this.IsWeeklyVisible = true;
            this.rcForm.controls['FuelSurchargeTable'].get('StartDate').setValue(moment().weekday(1).format('MM/DD/YYYY'));
            
            this.changeViewType(1);
            this.ShowMessage = false;
        }
    }

    public ngOnDestroy(): void {
        this.DtTrigger.unsubscribe();
    }

    //#region webserivce call
    private getCarriers(): void {
        this.IsLoading = true;
        this.regionService.getCarriers()
            .subscribe(async (carriers: DropdownItem[]) => {
                this.CarrierList = await carriers;
                this.DtTrigger.next();
                this.IsLoading = false;
            });
    }

    private getTableTypes(): void {
        this.fuelsurchargeService.getTableTypes().subscribe(async (data) => {
            this.TableTypeList = await (data);
            this.DtTrigger.next();
        });
    }

    private getSupplierCustomers(): void {
        this.IsLoading = true;
        this.fuelsurchargeService.getSupplierCustomers().subscribe(async (data) => {
            this.CustomerList = await (data);
            this.DtTrigger.next();
            this.IsLoading = false;
        });
    }

    //companyIds : Based on tableType we will be call API , if tableType master or customer or carrier, full source region,customers,carriers loads will populated.
    private getSourceRegions(tableType: string): void {

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
            this.SourceRegionList.sort((a, b) => (a.Name > b.Name) ? 1 : -1)
            this.DtTrigger.next();
        });
    }

    private getTerminalsBulkPlant(): void {
        this.IsLoading = true;
        var selectedSourceRegions = this.rcForm.get('SourceRegions').value as DropdownItem[];
        this.rcForm.get('TerminalsAndBulkPlants').patchValue([]);
        this.IsSourceRegionSelected = false;
        if (selectedSourceRegions != undefined && selectedSourceRegions != null && selectedSourceRegions.length > 0) {
            this.IsSourceRegionSelected = true;
            this.fuelsurchargeService.getTerminalsAndBulkPlants(selectedSourceRegions.map(s => s.Id).join(',')).subscribe(async (data) => {
                this.TerminalsAndBulkPlantList = await (data);
                this.DtTrigger.next();
                this.IsLoading = false;
            });
        } else {
            this.IsLoading = false;
        }
    }

    public SourceRegionChange(prev: any, next: any) {
        if (prev == null && next.length == 0) return;
        this.IsLoading = true;
        this.rcForm.controls.TerminalsAndBulkPlants.patchValue([]);
        this.IsSourceRegionSelected = false
        var ids = [];

        let selectedSourceRegions = this.rcForm.get('SourceRegions').value as DropdownItem[];
        if (selectedSourceRegions.length > 0) {
            selectedSourceRegions.forEach(s => ids.push(s.Id));
            this.fuelsurchargeService.getTerminalsAndBulkPlants(ids.join(',')).subscribe(async (data) => {
                this.TerminalsAndBulkPlantList = await (data);
                this.rcForm.controls.TerminalsAndBulkPlants.setValue(this.TerminalsAndBulkPlantList);
                this.IsSourceRegionSelected = true;
                this.DtTrigger.next();
            });
        }
        this.IsLoading = false;
    }

    private getFuelSurchargeProducts(countryId: any): void {
        this.fuelsurchargeService.getFuelSurchargeProducts(countryId).subscribe(async (data) => {
            this.FuelSurchargeProductList = await (data);
            this.rcForm.controls['FuelSurchargeProducts'].setValue(this.FuelSurchargeProductList.slice(0, 1));
            this.DtTrigger.next();
        });
    }

    private getFuelSurchargePeriods(countryId: any): void {
        this.fuelsurchargeService.getFuelSurchargePeriod(countryId).subscribe(async (data) => {
            this.FuelSurchargePeriodList = await (data);
            this.rcForm.controls['FuelSurchargePeriods'].setValue(this.FuelSurchargePeriodList.slice(0, 1));
            //var dt = moment(new Date()).toDate();
            //this.setApiAdjustIndexPriceDate(moment(dt).format('MM/DD/YYYY'));
            this.DtTrigger.next();
        });
    }

    private getFuelSurchargeArea(countryId: any): void {;
        this.fuelsurchargeService.getFuelSurchargeArea(countryId).subscribe(async (data) => {
            this.FuelSurchargeAreaList = await (data);
            this.rcForm.controls['FuelSurchargeAreas'].setValue(this.FuelSurchargeAreaList.slice(0, 1));
            this.DtTrigger.next();
        });
    }

    private getFuelIndexPrice(periodId: any, productType: any, fetchDate: any, areaId: any): void {
        this.IsLoading = true;
        if (this.SelectedCountryId == 1) {
            this.fuelsurchargeService.getEIAIndexPrice(periodId, productType, fetchDate, areaId).subscribe(async (data) => {
                this.APILatestIndexPrice = await (data);
                this.rcForm.controls['APILatestIndexPrice'].patchValue(this.APILatestIndexPrice);
                this.DtTrigger.next();
                this.IsLoading = false;
            });
        }
        else {
            this.fuelsurchargeService.getNRCIndexPrice(periodId, productType, fetchDate).subscribe(async (data) => {
                this.APILatestIndexPrice = await (data);
                this.rcForm.controls['APILatestIndexPrice'].patchValue(this.APILatestIndexPrice);
                this.DtTrigger.next();
                this.IsLoading = false;
            });

        }
    }

    IdInComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other.Id == current.Id
            }).length == 1;
        }
    }

    //GET
    private getFuelSurchargeTable(fuelSurchargeTableId: number): void {
        //this.isLoadingSubject.next(true);;
        let sorceRegionIds: string = "";
        this.IsLoading = true
        this.cdr.detectChanges()
        this.http.get(this.fuelsurchargeService.getFuelSurchargeTableUrl + fuelSurchargeTableId).pipe(
            map(fsIndex => {
                const fsModel = fsIndex as FuelSurchargeIndexViewModel;
                return fsModel;
            }),
            mergeMap(fsModel => {
                //this.isLoadingSubject.next(true);
                this.Fsmodel = fsModel;
                //let companyIds: number[] = [];
                if (this.fuelsurchargeId != null && this.fuelsurchargeMode.toUpperCase() == "COPY") { // on copy 
                    this.Fsmodel.FuelSurchargeIndexId = null;
                    this.Fsmodel.TableName = "";
                }
                const customers = this.http.get(this.fuelsurchargeService.getSupplierCustomersUrl);
                const carriers = this.http.get(this.regionService.getCarriersUrl);

                let customerIds: number[] = [];
                let carrierIds: number[] = [];
                if (this.Fsmodel.Customers.length > 0) {
                    customerIds = this.Fsmodel.Customers.map(s => s.Id);
                }
                if (this.Fsmodel.Carriers.length > 0) {
                    carrierIds = this.Fsmodel.Carriers.map(s => s.Id);
                }

                var sourceRegionInput = new SourceRegionInputModel();
                sourceRegionInput.TableType = this.Fsmodel.TableTypeId.toString();
                sourceRegionInput.CustomerId = customerIds;
                sourceRegionInput.CarrierId = carrierIds;
                const sourceRegions = this.http.post(this.fuelsurchargeService.getSourceRegionsUrl, sourceRegionInput);
                const tableTypes = this.http.get(this.fuelsurchargeService.getTableTypesUrl);
                if (this.Fsmodel.SourceRegions != null && this.Fsmodel.SourceRegions != undefined && this.Fsmodel.SourceRegions.length > 0) {
                    sorceRegionIds = this.Fsmodel.SourceRegions.map(s => s.Id).join(',');
                    this.IsSourceRegionSelected = true;
                }
                const terminalAndBulkPlans = this.http.get(this.fuelsurchargeService.getTerminalsAndBulkPlantsUrl + sorceRegionIds)
                return forkJoin([customers, carriers, sourceRegions, terminalAndBulkPlans, tableTypes]);
            })).subscribe(result => {
                this.IsLoading = false;
                //this.isLoadingSubject.next(true);
                this.IsMasterSelected = false;
                this.IsCustomerSelected = false;
                this.IsCarrierSelected = false;

                if (this.Fsmodel.TableTypeId == TableType.Master) {
                    this.IsMasterSelected = true;
                }
                else if (this.Fsmodel.TableTypeId == TableType.Customer) {
                    this.IsCustomerSelected = true;
                }
                else if (this.Fsmodel.TableTypeId == TableType.Carrier) {
                    this.IsCarrierSelected = true;
                }

                if (this.Fsmodel.TableTypeId != TableType.Master) {
                    this.CustomerList = result[0] as DropdownItem[];
                    this.CarrierList = result[1] as DropdownItem[];
                }

                this.SourceRegionList = result[2] as DropdownItem[];
                if (this.Fsmodel.SourceRegions != null && this.Fsmodel.SourceRegions != undefined && this.Fsmodel.SourceRegions.length > 0) {
                    this.TerminalsAndBulkPlantList = result[3] as DropdownItemExt[];
                }         
                this.TableTypeList = result[4] as DropdownItem[];
                //this.isLoadingSubject.next(true);
                this.Edit(this.Fsmodel);
               
            });
    }

    //Edit
    private Edit(_fs: FuelSurchargeIndexViewModel) {
        if (this.rcForm) {
            //this.isLoadingSubject.next(true);
            this.IsLoading = true;
            this.IsWeeklyVisible = false;
            this.IsAnnualyVisible = false;
            this.IsMonthlyVisible = false;
            this.rcForm.controls['ProductId'].setValue(_fs.ProductId);
            this.rcForm.controls['PeriodId'].setValue(_fs.PeriodId);
            this.rcForm.controls['TableTypeId'].setValue(_fs.TableTypeId);
            this.rcForm.controls['AreaId'].setValue(_fs.AreaId);
            this.rcForm.controls['FuelSurchargeIndexId'].setValue(_fs.FuelSurchargeIndexId);
            this.rcForm.controls['TableName'].setValue(_fs.TableName);
            this.rcForm.controls['Notes'].setValue(_fs.Notes);
            this.rcForm.controls['IndexPriceDate'].setValue(moment(_fs.IndexPriceDate).format('MM/DD/YYYY'));
            this.rcForm.controls['TableTypes'].setValue(this.TableTypeList.filter(x => x.Id == _fs.TableTypeId));
            _fs.IsManualUpdate ? this.changeViewType("2") : this.changeViewType("1");
            let stringify = JSON.parse(_fs.ApiEffectiveDate);
            if (!_fs.IsManualUpdate && stringify != null && stringify != undefined && stringify != "") {
                this.IsLoading = true;
                if (stringify.WeekDay != null && stringify.WeekDay != undefined && stringify.WeekDay != "") {
                    this.rcForm.controls['WeekDay'].setValue(stringify.WeekDay);
                    this.IsWeeklyVisible = true;
                }
                if (stringify.Weeks != null && stringify.Weeks != undefined && stringify.Weeks != "") {
                    let weeks : DropdownItemExt[] = [];
                    weeks.push({ Id: JSON.parse(stringify.Weeks).Id, Name: JSON.parse(stringify.Weeks).Name, Code: "" });
                    this.rcForm.controls['Weeks'].setValue(weeks);
                    this.IsWeeklyVisible = true;
                }
                if (stringify.Months != null && stringify.Months != undefined && stringify.Months != "") {
                    let months:DropdownItemExt[]=[];
                    months.push({ Id: JSON.parse(stringify.Months).Id, Name: JSON.parse(stringify.Months).Name, Code: "" });
                    this.rcForm.controls['Months'].setValue(months);
                    this.IsMonthlyVisible = true;
                }
                if (stringify.Annualy != null && stringify.Annualy != undefined && stringify.Annualy != "") {
                   let annualy:DropdownItemExt[]=[];
                    annualy.push({ Id: JSON.parse(stringify.Annualy).Id, Name: JSON.parse(stringify.Annualy).Name, Code: "" });
                    this.rcForm.controls['Annualy'].setValue(annualy);
                    this.IsAnnualyVisible = true;
                }
            }
            //this.isLoadingSubject.next(true);
            this.IsLoading = true;
            if (_fs.Customers != null && this.CustomerList != undefined && this.CustomerList != null ) {
                if (this.CustomerList.length > 0 && _fs.Customers.length > 0) this.rcForm.controls['Customers'].setValue(this.CustomerList.filter(this.IdInComparer(_fs.Customers)));
                
            }
            if (_fs.Carriers != null && this.CarrierList != undefined && this.CarrierList != null ) {
                if (this.CarrierList.length > 0 && _fs.Carriers.length > 0) this.rcForm.controls['Carriers'].setValue(this.CarrierList.filter(this.IdInComparer(_fs.Carriers)));
                
            }
            if (this.SourceRegionList != null && this.SourceRegionList != undefined && _fs.SourceRegions != null && _fs.SourceRegions != undefined && _fs.SourceRegions.length > 0) {
                if (this.SourceRegionList.length > 0 && _fs.SourceRegions.length > 0)
                    this.rcForm.controls['SourceRegions'].setValue(this.SourceRegionList.filter(this.IdInComparer(_fs.SourceRegions)));
            }
            if (this.TerminalsAndBulkPlantList != null && this.TerminalsAndBulkPlantList != undefined && _fs.TerminalsAndBulkPlants != null && _fs.TerminalsAndBulkPlants != undefined && _fs.TerminalsAndBulkPlants.length > 0) {
                if (this.TerminalsAndBulkPlantList.length > 0 && _fs.TerminalsAndBulkPlants.length > 0) {
                    this.rcForm.controls.TerminalsAndBulkPlants.setValue(this.TerminalsAndBulkPlantList.filter(this.IdInComparer(_fs.TerminalsAndBulkPlants)));
                }
            }
            if (_fs.ProductId!=null && this.FuelSurchargeProductList != null && this.FuelSurchargeProductList != undefined && this.FuelSurchargeProductList.length > 0) {
                this.rcForm.controls['FuelSurchargeProducts'].setValue(this.FuelSurchargeProductList.filter(x => x.Id == _fs.ProductId.toString()));
            } 
            if (_fs.PeriodId!=null && this.FuelSurchargePeriodList != null && this.FuelSurchargePeriodList != undefined && this.FuelSurchargePeriodList.length > 0) {
                this.rcForm.controls['FuelSurchargePeriods'].setValue(this.FuelSurchargePeriodList.filter(x => x.Id == _fs.PeriodId.toString()));
            } 
            if (_fs.AreaId!=null && this.FuelSurchargeAreaList != null && this.FuelSurchargeAreaList != undefined && this.FuelSurchargeAreaList.length > 0) {
                this.rcForm.controls['FuelSurchargeAreas'].setValue(this.FuelSurchargeAreaList.filter(x => x.Id == _fs.AreaId.toString()));
            } 
            this.rcForm.controls['APILatestIndexPrice'].setValue(_fs.APILatestIndexPrice);
            if (_fs.ApiAdjustIndexPriceDate != null && _fs.ApiAdjustIndexPriceDate != undefined) {
                if (this.IsWeeklyVisible) {
                    this.rcForm.controls['ApiAdjustIndexPriceDate'].setValue(moment(_fs.ApiAdjustIndexPriceDate).format('MM/DD/YYYY'));
                } else if (this.IsMonthlyVisible) {
                    this.rcForm.controls['SourceMonths'].setValue(this.SourceMonthList.filter(x => x.Name == moment(_fs.ApiAdjustIndexPriceDate).format('MMMM YYYY')));

                } else if (this.IsAnnualyVisible) {
                    this.rcForm.controls['SourceAnnualy'].setValue(this.SourceAnnualyList.filter(x => x.Name == moment(_fs.ApiAdjustIndexPriceDate).format('YYYY')));
                }
            }

            this.rcForm.controls['ManualLatestIndexPrice'].setValue(_fs.ManualLatestIndexPrice);
            if (_fs.ManualEffectiveDate != null && _fs.ManualEffectiveDate != undefined) {
                this.rcForm.controls['ManualEffectiveDate'].setValue(moment(_fs.ManualEffectiveDate).format('MM/DD/YYYY'));
            } else {
                this.rcForm.get('ManualEffectiveDate').patchValue([]);
            }
            this.rcForm.controls['StatusId'].setValue(_fs.StatusId);

            if (_fs.FuelSurchargeTable != null && _fs.FuelSurchargeTable != undefined) {
                if (_fs.FuelSurchargeTable.StartDate != null && _fs.FuelSurchargeTable.StartDate != undefined) {
                    this.rcForm.controls['FuelSurchargeTable'].get('StartDate').setValue(moment(_fs.FuelSurchargeTable.StartDate).format('MM/DD/YYYY'));
                } else {
                    this.rcForm.controls['FuelSurchargeTable'].get('StartDate').patchValue([]);
                }
                if (_fs.FuelSurchargeTable.EndDate != null && _fs.FuelSurchargeTable.EndDate != undefined) {
                    this.rcForm.controls['FuelSurchargeTable'].get('EndDate').setValue(moment(_fs.FuelSurchargeTable.EndDate).format('MM/DD/YYYY'));
                } else {
                    this.rcForm.controls['FuelSurchargeTable'].get('EndDate').patchValue([]);
                }
                this.rcForm.controls['FuelSurchargeTable'].get('PriceRangeStartValue').setValue(_fs.FuelSurchargeTable.PriceRangeStartValue);
                this.rcForm.controls['FuelSurchargeTable'].get('PriceRangeEndValue').setValue(_fs.FuelSurchargeTable.PriceRangeEndValue);
                this.rcForm.controls['FuelSurchargeTable'].get('PriceRangeInterval').setValue(_fs.FuelSurchargeTable.PriceRangeInterval);
                this.rcForm.controls['FuelSurchargeTable'].get('FuelSurchargeStartPercentage').setValue(_fs.FuelSurchargeTable.FuelSurchargeStartPercentage);
                this.rcForm.controls['FuelSurchargeTable'].get('SurchargeInterval').setValue(_fs.FuelSurchargeTable.SurchargeInterval);
            }
            //this.isLoadingSubject.next(true);
            this.IsLoading = true;
            var gst = this.rcForm.controls['GeneratedSurchargeTable'] as FormArray
            if (_fs.GeneratedSurchargeTable != null && _fs.GeneratedSurchargeTable.length > 0) {
                gst.clear();
                _fs.GeneratedSurchargeTable.forEach(res => {
                    gst.push(new FormControl({
                        FuelSurchargeStartPercentage: res.FuelSurchargeStartPercentage,
                        PriceRangeStartValue: res.PriceRangeStartValue,
                        PriceRangeEndValue: res.PriceRangeEndValue
                    }))
                });
                
            }     
            this.onGenerateSurchargeTable();
            this.IsEditLoaded = true;
            //this.isLoadingSubject.next(false);
            this.IsLoading = false;
        }
        if (this.fuelsurchargeMode == "VIEW") {
            this.disableInputControls = true;
        } else {
            this.disableInputControls = false;
        }
        this.AllowTableName = false;
    }
    
    private nextDate(givenDate:Date, dayIndex:number): Date {
        var today = new Date(givenDate);
        //var a = today.getDate();
        //var e = today.getDay();
        //var b = (dayIndex - 1 - today.getDay() + 7);
        //var c = b % 7;
        //var d = today.getDate() + (dayIndex - 1 - today.getDay() + 7) % 7 + 1;
        //don't find next date in case in edage case . like on same day and having fine arrangement not to crash when more than 1 week, and arrangment is to first add required days and than call current method.
        if (dayIndex != today.getDay()) today.setDate(today.getDate() + (dayIndex - 1 - today.getDay() + 7) % 7 + 1);
        return today;
    }

    private setValidFromDate() {
        this.IsLoading = true;

        var selectedPeriod = this.rcForm.get('FuelSurchargePeriods').value as DropdownItemExt[];
        var selectedSourceMonth = this.rcForm.get('SourceMonths').value as DropdownItemExt[];
        var selectedSourceAnnualy = this.rcForm.get('SourceAnnualy').value as DropdownItemExt[];
        var ApiAdjustIndexPriceDate = this.rcForm.controls.ApiAdjustIndexPriceDate.value;
        //if (!this.IsWeeklyVisible) this.rcForm.controls.ApiAdjustIndexPriceDate.setValue(null);
       // this.AddRemoveValidations(null,[this.rcForm.get('Weeks'), this.rcForm.get('Months'), this.rcForm.get('Annualy')]);
        if (selectedPeriod != null && selectedPeriod!=undefined && selectedPeriod.length > 0 ) {

            let effectiveDate= new Date();
            if (ApiAdjustIndexPriceDate != null && ApiAdjustIndexPriceDate != undefined && selectedPeriod[0].Name.toUpperCase() == PeriodEnum.WEEKLY) {

                this.AddRemoveValidations([this.rcForm.get('Weeks')],
                    [this.rcForm.get('Months'), this.rcForm.get('Annualy')]);

                effectiveDate= this.rcForm.controls.ApiAdjustIndexPriceDate.value;
                effectiveDate = new Date(effectiveDate);
                let WeekDay = this.IsWeeklyVisible && this.rcForm.controls.WeekDay.value != null && this.rcForm.controls.WeekDay.value != undefined && this.rcForm.controls.WeekDay.value != "" ? this.rcForm.controls.WeekDay.value : "";
                let Weeks = this.IsWeeklyVisible && this.rcForm.controls.Weeks.value != null && this.rcForm.controls.Weeks.value != undefined && this.rcForm.controls.Weeks.value != "" ? this.rcForm.controls.Weeks.value : "";
                
                var selectedWeeks = this.rcForm.get('Weeks').value as DropdownItemExt[];
                if (selectedWeeks != null && selectedWeeks != undefined && selectedWeeks.length>0) {
                    if (selectedWeeks[0].Name == WeekEnum.Same_Week) {
                       // default will handle
                    } else if (selectedWeeks[0].Name == WeekEnum.Week_Later_1) {
                        effectiveDate.setDate(effectiveDate.getDate() + 7);
                    } else if (selectedWeeks[0].Name == WeekEnum.Week_Later_2) {
                        effectiveDate.setDate(effectiveDate.getDate() + 14);
                    } else if (selectedWeeks[0].Name == WeekEnum.Week_Later_3) {
                        effectiveDate.setDate(effectiveDate.getDate() + 21);
                    }
                    if (WeekDay != "" && Weeks != "") {
                        switch (WeekDay.toUpperCase()) {
                            case "SUN": {
                                effectiveDate = this.nextDate(effectiveDate, WeekDays.Sun)
                                break;
                            }
                            case "MON": {
                                effectiveDate = this.nextDate(effectiveDate, WeekDays.Mon)
                                break;
                            }
                            case "TUE": {
                                effectiveDate = this.nextDate(effectiveDate, WeekDays.Tue)
                                break;
                            }
                            case "WED": {
                                effectiveDate = this.nextDate(effectiveDate, WeekDays.Wed)
                                break;
                            }
                            case "THU": {
                                effectiveDate = this.nextDate(effectiveDate, WeekDays.Thu)
                                break;
                            }
                            case "FRI": {
                                effectiveDate = this.nextDate(effectiveDate, WeekDays.Fri)
                                break;
                            }
                            case "SAT": {
                                effectiveDate = this.nextDate(effectiveDate, WeekDays.Sat)
                                break;
                            }
                            default: {
                                //statements; 
                                break;
                            }
                        }
                        this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValue(moment(effectiveDate).format('MM/DD/YYYY'));
                        this.MinToDate = moment(effectiveDate).toDate();
                        this.IsWeeklyVisible = true;
                    }
                }
            }
            else if (selectedSourceMonth != null && selectedSourceMonth!=undefined && selectedSourceMonth.length > 0 && selectedPeriod[0].Name.toUpperCase() == PeriodEnum.MONTHLY) {
                var selectedSourceMonth = this.rcForm.get('SourceMonths').value as DropdownItemExt[];
                this.AddRemoveValidations([this.rcForm.get('Months')],
                    [this.rcForm.get('Weeks'), this.rcForm.get('Annualy')]);
                 effectiveDate = new Date(selectedSourceMonth[0].Id);

                var selectedMonths = this.rcForm.get('Months').value as DropdownItemExt[];
                if (selectedMonths != null && selectedMonths != undefined && selectedMonths.length > 0) {
                    if (selectedMonths[0].Name == MonthEnum.Month_Later_1) {
                        effectiveDate.setMonth(effectiveDate.getMonth() + 1, 1);
                    } else if (selectedMonths[0].Name == MonthEnum.Month_Later_2) {
                        effectiveDate.setMonth(effectiveDate.getMonth() + 2, 1);
                    } else if (selectedMonths[0].Name == MonthEnum.Month_Later_3) {
                        effectiveDate.setMonth(effectiveDate.getMonth() + 3, 1);
                    }
                    this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValue(moment(effectiveDate).format('MM/DD/YYYY'));
                    this.MinToDate = moment(effectiveDate).toDate();
                    this.IsMonthlyVisible = true;
                }
            }

            else if (selectedSourceAnnualy != null && selectedSourceAnnualy != undefined && selectedSourceAnnualy.length>0 && selectedPeriod[0].Name.toUpperCase() == PeriodEnum.ANNUALY) {
                effectiveDate = new Date(selectedSourceAnnualy[0].Id);
                this.AddRemoveValidations([this.rcForm.get('Annualy')],
                    [this.rcForm.get('Weeks'), this.rcForm.get('Months')]);
                var selectedYear = this.rcForm.get('Annualy').value as DropdownItemExt[];
                if (selectedYear != null && selectedYear != undefined && selectedYear.length > 0) {
                    if (selectedYear[0].Name == AnnualyEnum.Year_Later_1) {
                        effectiveDate.setFullYear(effectiveDate.getFullYear() + 1, 0, 1);
                    } else if (selectedYear[0].Name == AnnualyEnum.Year_Later_2) {
                        effectiveDate.setFullYear(effectiveDate.getFullYear() + 2, 0, 1);
                    } else if (selectedYear[0].Name == AnnualyEnum.Year_Later_3) {
                        effectiveDate.setFullYear(effectiveDate.getFullYear() + 3, 0, 1);
                    }
                    this.rcForm.controls.FuelSurchargeTable.get('StartDate').setValue(moment(effectiveDate).format('MM/DD/YYYY'));
                    this.MinToDate = moment(effectiveDate).toDate();
                    this.IsAnnualyVisible = true;
                }
            }
        }
        this.IsLoading = false;
    }

    //Save
    private Save(fuelSurchageStatus: FreightTableStatus): void {
        this.IsLoading = true;
        this.Fsmodel = this.rcForm.value;
        if (this.rcForm.controls.ApiAdjustIndexPriceDate.value != null && this.rcForm.controls.ApiAdjustIndexPriceDate.value != undefined && this.rcForm.controls.ApiAdjustIndexPriceDate.value != "")
            this.Fsmodel.ApiAdjustIndexPriceDate = new Date(moment(this.rcForm.controls.ApiAdjustIndexPriceDate.value).format());
        if (this.rcForm.controls.ManualEffectiveDate.value != null && this.rcForm.controls.ManualEffectiveDate.value != undefined)
            this.Fsmodel.ManualEffectiveDate = new Date(moment(this.rcForm.controls.ManualEffectiveDate.value).format());
        if (this.rcForm.controls.FuelSurchargeTable.get('StartDate').value != null && this.rcForm.controls.FuelSurchargeTable.get('StartDate').value != undefined)
            this.Fsmodel.FuelSurchargeTable.StartDate = this.rcForm.controls.FuelSurchargeTable.get('StartDate').value;
        if (this.rcForm.controls.FuelSurchargeTable.get('EndDate').value != null && this.rcForm.controls.FuelSurchargeTable.get('EndDate').value != undefined)
            this.Fsmodel.FuelSurchargeTable.EndDate = this.rcForm.controls.FuelSurchargeTable.get('EndDate').value;

        if (this.Fsmodel.TerminalsAndBulkPlants != null || this.Fsmodel.TerminalsAndBulkPlants != undefined)
            this.Fsmodel.TerminalsAndBulkPlants.forEach(res => {
                res.Code = this.TerminalsAndBulkPlantList.find(c => c.Id == res.Id && c.Name == res.Name).Code;
            });
        if (this.IsMonthlyVisible) {
            var sourceMonths = this.rcForm.get('SourceMonths').value as DropdownItemExt[];
            if (sourceMonths != null && sourceMonths.length > 0) {
                this.Fsmodel.ApiAdjustIndexPriceDate = new Date(moment(sourceMonths[0].Id).format());
            } else {
                this.Fsmodel.ApiAdjustIndexPriceDate = null;
            }

        }
        if (this.IsAnnualyVisible) {
            var sourceAnnualy = this.rcForm.get('SourceAnnualy').value as DropdownItemExt[];
            if (sourceAnnualy != null && sourceAnnualy.length > 0) {
                this.Fsmodel.ApiAdjustIndexPriceDate = new Date(moment(sourceAnnualy[0].Id).format());
            } else {
                this.Fsmodel.ApiAdjustIndexPriceDate = null;
            }
        }

        this.Fsmodel.IndexPriceDate = this.rcForm.get('IndexPriceDate').value;
        this.Fsmodel.IsManualUpdate = this.viewType == 2 ? true : false;
        this.Fsmodel.ProductId = null;
        this.Fsmodel.PeriodId = null;
        this.Fsmodel.TableTypeId = null;
        this.Fsmodel.AreaId = null;
        this.Fsmodel.ApiEffectiveDate = "";

        if (!this.Fsmodel.IsManualUpdate) {
            var selectedProduct = this.rcForm.get('FuelSurchargeProducts').value as DropdownItem[];
            var selectedPeriod = this.rcForm.get('FuelSurchargePeriods').value as DropdownItemExt[];
            var selectedArea = this.rcForm.get('FuelSurchargeAreas').value as DropdownItem[];
            this.Fsmodel.ProductId = selectedProduct != null && selectedProduct != undefined ? selectedProduct[0].Id : null;
            this.Fsmodel.PeriodId = selectedPeriod != null && selectedPeriod != undefined ? +selectedPeriod[0].Id : null;
            this.Fsmodel.AreaId = selectedArea != null && selectedArea != undefined ? selectedArea[0].Id : null;
            let ApiEffectiveDate = {
                "WeekDay": this.IsWeeklyVisible && this.rcForm.controls.WeekDay.value != null && this.rcForm.controls.WeekDay.value != undefined && this.rcForm.controls.WeekDay.value != "" ? this.rcForm.controls.WeekDay.value : "",
                "Weeks": this.IsWeeklyVisible && this.rcForm.controls.Weeks.value != null && this.rcForm.controls.Weeks.value != undefined && this.rcForm.controls.Weeks.value != "" ? JSON.stringify(this.rcForm.controls.Weeks.value[0]) : "",
                "Months": this.IsMonthlyVisible && this.rcForm.controls.Months.value != null && this.rcForm.controls.Months.value != undefined && this.rcForm.controls.Months.value != "" ? JSON.stringify(this.rcForm.controls.Months.value[0]) : "",
                "Annualy": this.IsAnnualyVisible && this.rcForm.controls.Annualy.value != null && this.rcForm.controls.Annualy.value != undefined && this.rcForm.controls.Annualy.value != "" ? JSON.stringify(this.rcForm.controls.Annualy.value[0]) : ""
            };
            this.Fsmodel.ApiEffectiveDate = JSON.stringify(ApiEffectiveDate);
        }
        var selectedTableType = this.rcForm.get('TableTypes').value as DropdownItem[];
        this.Fsmodel.TableTypeId = selectedTableType[0].Id;

        this.Fsmodel.StatusId = fuelSurchageStatus;

        if (!this.IsGeneratedSurchargeTable) this.Fsmodel.GeneratedSurchargeTable = null;

        this.fuelsurchargeService.createFuelSurcharge(this.Fsmodel)
            .subscribe((response: any) => {
                this.ServiceResponse = response;
                if (response != null && response.StatusCode == 0) {
                    let message = "";
                    if (this.Fsmodel.FuelSurchargeIndexId != null) {
                        message = "Fuel Surcharge table edit successfully.";
                    }
                    else if (this.Fsmodel.StatusId == FreightTableStatus.Published) {
                        message = "Fuel Surcharge table created successfully.";
                    } else if (this.Fsmodel.StatusId == FreightTableStatus.Draft) {
                        message = "Fuel Surcharge draft saved successfully.";
                    }
                    Declarations.msgsuccess(message, undefined, undefined);
                    this.IsLoading = false;
                    this.changeToViewTab();
                }
                else {
                    this.IsLoading = false;
                    Declarations.msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                }
            });
    }


    public changeToViewTab() {
        this.fuelsurchargeService.onSelectedTabChanged.next(2);
    }

    //#endregion    
}

const RangeValidator: ValidatorFn = (fg: FormGroup) => {
    const fst = fg.get('FuelSurchargeTable').value;
    const start = fst.PriceRangeStartValue;
    const end = fst.PriceRangeEndValue;
    const statusId = fg.get('StatusId').value;
    return statusId==1 || start != null && end != null && +end > +start
        ? null
        : { range: true };
};




