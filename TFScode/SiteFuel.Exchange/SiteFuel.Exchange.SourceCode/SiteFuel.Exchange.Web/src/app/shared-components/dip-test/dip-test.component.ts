import { Component, OnInit, Output, EventEmitter, Input, OnChanges, SimpleChanges, ChangeDetectionStrategy, ChangeDetectorRef, ElementRef, ViewChild, OnDestroy, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { FormBuilder, FormArray, FormGroup, Validators } from '@angular/forms';
import { AdditiveOrderViewModel, DropdownItem, StateDropdownExtendedItem, StatelistService, TBDDropdownItem } from 'src/app/statelist.service';
import { Declarations } from 'src/app/declarations.module';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { BehaviorSubject, Subscription, Subject, of, iif } from 'rxjs';
import { DipTestViewModel, CustomerJobsForCarrier, SalesDataModel, DropAddressModel, OrderPickupLocationModel, OrderPickupDetailModel, RaiseTBDDeliveryRequest, TBDRaiseDRDeliveryRequests, BlendedRequest, CompatibleProductModel, DeliveryRequestViewModel } from 'src/app/carrier/models/DispatchSchedulerModels';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { DataService } from 'src/app/services/data.service';
import * as moment from 'moment';
import { DataTableDirective } from 'angular-datatables';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import { UtilService } from 'src/app/services/util.service';
import { AddressService } from 'src/app/address.service';
import { ScheduleBuilderService } from 'src/app/carrier/service/schedule-builder.service';
import { AddressModel } from 'src/app/invoice/models/DropDetail';
import { catchError, debounceTime, distinctUntilChanged, switchMap, tap } from 'rxjs/operators';
import { JobRegionModel } from 'src/app/carrier/models/location';
import { DeliveryReqPriority } from 'src/app/app.enum';
import { additiveProductTypeId, DropDown, NumberConstants, RegExConstants, ScheduleDays, ScheduleQuantityType, ScheduleTypes } from 'src/app/app.constants';
import { getRecurringUniqueId, getUniqueId } from 'src/app/my.functions';
declare function closeSlidePanel(): any;
declare var IsBuyerCompany: boolean;

@Component({
    selector: 'app-dip-test',
    templateUrl: './dip-test.component.html',
    styleUrls: ['./dip-test.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    encapsulation: ViewEncapsulation.None
})

export class DipTestComponent implements OnInit, OnChanges, OnDestroy, AfterViewInit {

    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
    @ViewChild('elementFilter') elementFilter: ElementRef;
    //keyword = 'Name';
    public IsCommonBadge = false
    public height_Panel: any;
    public DipTestsForEachTank: DipTestViewModel[] = [];
    public OtherProductDipTestsForEachTank: DipTestViewModel[] = [];
    public selectedLocation: JobRegionModel[] = [];
    public SiteList: JobRegionModel[] = [];
    public selectedCustomer: CustomerJobsForCarrier[] = [];
    public companySiteList: CustomerJobsForCarrier[] = [];
    public SiteddlSettings = {};
    public CustomerSettings = {};
    public DaySetting = {};
    // public DeliveryRequests: FormArray;
    public fmGroup: FormGroup;
    public IsLoading: boolean = false;
    public DelReqPriority: typeof DeliveryReqPriority = DeliveryReqPriority;
    public dipTestLoader: boolean = false;
    public showForm: boolean = false;
    public IsDrFromMultiWindow: boolean = false;
    public OrderList: DropdownItem[] = [];
    public selectedOrder: DropdownItem[] = [];
    public isTankExists: boolean = true;
    public OrderDetails: DipTestViewModel[] = [];
    public multiDropdownSettings: IDropdownSettings;
    public chartdata: any;
    public isChartDataExistSubject: BehaviorSubject<any>;
    public disableControl: boolean = false;
    public loadingCustomers: boolean = true;
    public otherProductTypeId: number;
    public companyUoM: number;
    public CompatibleProductTypeList: CompatibleProductModel[] = [];
    public MinDate: Date = new Date();
    public MaxDate: Date = new Date();
    public StartTime: any;
    public EndTime: any;

    @Input() IsThisFromDrDisplay: boolean;
    @Input() public isDisableControl: boolean;
    @Input() SelectedRegionId: string;
    @Input() IsSalesPage: boolean = false;
    @Input() RequestFromBuyerWallyBoard: boolean;
    @Output() onRaiseDR: EventEmitter<any> = new EventEmitter<any>();
    @Output() OnRaiseDRFromMultiWindow: EventEmitter<any> = new EventEmitter<any>();

    public DisableControlsSubscription: Subscription;
    public FormValidationMessage: string = "";

    //recuring schedules
    public ScheduleTypes: DropdownItem[] = ScheduleTypes.filter(x => x.Id != 4) as DropdownItem[];;
    public ScheduleQuantityTypeDetails: DropdownItem[] = ScheduleQuantityType;
    //public RecurringDRSchedule: RecurringDRSchedule[] = [];
    //public isRecurringSchedule: boolean;
    public MaxInputDate: Date = new Date();
    public currentdate: string;
    // public options: Options;
    public ScheduleDaysDetails: Array<DropDown> = ScheduleDays;
    public failedMessageIdentification: string = "<failed>";
    public messageSplitTag: string = "<split>";
    //public PoNumber: string;
    //public TankName: string;
    public RetainTime: string;
    public RetainDate: string;
    public WindowStartTime: string;
    public WindowStartDate: string;
    public WindowEndTime: string;
    public WindowEndDate: string;
    public IsRetainButtonClick: boolean = false;
    SelectedCustomerId: string;
    SelectedLocationId: string;
    SalesData: SalesDataModel[] = [];
    public dtTrigger: Subject<any> = new Subject();
    public dtOptions: any = {};
    public isLoadingSubject: BehaviorSubject<any>;


    public StateList: StateDropdownExtendedItem[] = [];
    public CountryList: DropdownItem[] = [];
    public CountryGroupList: DropdownItem[] = [];
    public ChangePickupForOrders: number[] = [];
    public BulkPlants: DropdownItem[];
    public BulkplantList: DropdownItem[];
    private DGSubscription: Subscription = new Subscription();
    public LocationType: number = 1;
    public SearchTerminalFuelType: string;
    public Terminals = [];
    public TBDTerminals = [];
    public isReadOnly: boolean = false;
    public PickupForm: FormGroup;
    public SelectedDRForEditPickup: FormGroup;
    public SelectedDRForEditPickupIndex: number;
    public CountryBasedZipcodeLabel: string = "Zip";
    public _loadingTerminals: boolean = false;
    public SearchTerminalKey: string;
    public minCharRequired = false;
    public _loadingAddress: boolean = false;
    public noTerminalFound = false;
    public searchError = false;

    public activePriorityTab = DeliveryReqPriority.MustGo;
    public isUserSubmit: boolean = false;
    public fmTBDGroup: FormGroup;
    public FuelTypeDdlSettings = {};
    public FuelTypeDetails: TBDDropdownItem[] = [];
    public OtherFuelTypeDetails: TBDDropdownItem[] = [];
    public SelectedTBDForEditPickup: FormGroup;
    public SelectedTBDForEditPickupIndex: number;
    public TBDDeliveryRequestViewModel: RaiseTBDDeliveryRequest = new RaiseTBDDeliveryRequest();
    public isTBDUserSubmit: boolean = false;
    public TBDdipTestLoader: boolean = false;

    //BLENDED REQUEST
    public DrForEditBlendedRequestIndex: number = 0;
    public DrForEditBlendedRequest: FormGroup;
    public BlendDrIndex: number = 0;
    public IsPickupForBlendRequest: boolean = false;
    public blendRequestForm: FormGroup = this.fb.group({ BlendedRequests: this.fb.array([]) });
    public additiveOrders: AdditiveOrderViewModel[] = [];
    public preferenceSetting: any = null;


    constructor(private fb: FormBuilder, private carrierService: CarrierService,
        private utilService: UtilService,
        private stateService: StatelistService,
        private addresService: AddressService,
        private sbService: ScheduleBuilderService,
        private dispatcherService: DispatcherService,
        private changeDetectorRef: ChangeDetectorRef,
        private dataService: DataService) {
        this.fmGroup = this.fb.group({
            DeliveryRequests: this.fb.array([]),
            IsTankNotAvailableForOrderProducts: this.fb.control(false),
            //new controls
            PickupLocationType: this.fb.control(null),
            IsCommonPickup: this.fb.control(null),
            Terminal: this.utilService.getTerminalForm(new DropdownItem(), false),
            BulkPlant: this.utilService.getBulkPlantForm(new DropAddressModel(), false),
            BadgeNo1: this.fb.control(null),
            BadgeNo2: this.fb.control(null),
            BadgeNo3: this.fb.control(null),
            LoadCode: this.fb.control(null),
            IsCommonBadge: this.fb.control(null),
            IsPreLoadInfo: this.fb.control(null),
            ProductTypeId: this.fb.control(null)
        });
        this.isChartDataExistSubject = new BehaviorSubject(false);
        this.MaxInputDate.setFullYear(this.MaxInputDate.getFullYear() + 1);
        this.currentdate = moment(new Date()).format('MM/DD/YYYY');
        this.isLoadingSubject = new BehaviorSubject(false);
        //TBD
        this.fmTBDGroup = this.fb.group({
            ScheduleTBDForm: this.fb.array([]),
        });
        this.FuelTypeDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
        };
    }

    ngOnChanges(change: SimpleChanges) {
        if (change.IsThisFromDrDisplay && change.IsThisFromDrDisplay.currentValue != null) {
            this.IsDrFromMultiWindow = change.IsThisFromDrDisplay.currentValue;
        }
        this.subscribeDisableControlsSubject();
    }
    ngOnDestroy(): void {
        this.dtTrigger.unsubscribe();
        this.unsubscribeAllSubscriptions_();
    }
    ngOnInit() {
        //  this.initializeSalesGrid();
        this.SiteddlSettings = {
            singleSelection: true,
            closeDropDownOnSelection: true,
            idField: 'Id',
            textField: 'DisplayName',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true

        };
        this.multiDropdownSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            allowSearchFilter: true,
            itemsShowLimit: 1
        };
        this.CustomerSettings = {
            singleSelection: true,
            closeDropDownOnSelection: true,
            idField: 'CompanyId',
            textField: 'CompanyName',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.DaySetting = {
            singleSelection: false,
            closeDropDownOnSelection: false,
            idField: 'id',
            textField: 'text',
            enableCheckAll: false,
            itemsShowLimit: 1,
        };
        // this.options = {
        //     multiple: true,
        //     closeOnSelect: false,
        //     tags: true
        // };
        this.additiveOrders = [];
        let element: HTMLElement = document.getElementById('idTankTab') as HTMLElement;
        element ? element.click() : '';
        this.subscribeDisableControlsSubject();
        this.height_Panel = this.calHeight();
        this.PickupForm = this.initPickupForm_(new OrderPickupDetailModel());
        this.PickupLocationTypeChange_();
        this.MinDate = new Date(this.MinDate.getFullYear(), this.MinDate.getMonth(), this.MinDate.getDate(), 0, 0, 0);
        this.MaxDate.setFullYear(this.MinDate.getFullYear() + 30);
        this.StartTime = new Date(0, 0, 0, null, null, null);
        this.EndTime = new Date(0, 0, 0, null, null, null);
    }

    onDaySelect(event: any, recurringSchedule: FormGroup, isSelected: boolean) {
    }
    // start
    getBulkPlantAddress_(bulkPlantName: string) {
        this.DGSubscription.add(this.addresService.GetBulkPlantDetails(bulkPlantName).subscribe(response => {
            this.PickupForm.controls['BulkPlant'].patchValue({
                Address: response.Address,
                City: response.City,
                State: response.State,
                Country: { Code: response.Country.Code },
                ZipCode: response.ZipCode,
                CountyName: response.CountyName,
                Latitude: response.Latitude,
                Longitude: response.Longitude,
                SiteId: response.SiteId
            });
            if (!this.changeDetectorRef['destroyed'])
                this.changeDetectorRef.detectChanges();
        }));
        this.PickupForm.markAllAsTouched();
        this.PickupForm.markAsDirty();
    }
    changeTerminal_(delReq: FormGroup, event: any, drIndex: number): void {
        let _orderId = event.target.selectedOptions[0].value;
        let orders = delReq.get('OrderPickupDetails').value as any[] || [];
        if (orders.length > 0) {
            let selectedOrder = orders.find(x => x.OrderId == _orderId);
            if (!selectedOrder) {
                var order = this.OrderDetails.find(t => t.OrderId == _orderId);
                if (order && order.OrderPickupDetails) {
                    selectedOrder = order.OrderPickupDetails.find(x => x.OrderId == _orderId);
                }
            }
            //set badges
            delReq.get('BadgeNo1').patchValue(selectedOrder.Badge1);
            delReq.get('BadgeNo2').patchValue(selectedOrder.Badge2);
            delReq.get('BadgeNo3').patchValue(selectedOrder.Badge3);

            let location = OrderPickupLocationModel.ToLocation(selectedOrder);
            this.setPickupLocation_(delReq, location);
            this.findDrWithSameProductAndJob(delReq, _orderId, location, drIndex);
        }
    }
    findDrWithSameProductAndJob(selectedDrForm: FormGroup, seletedOrderId: number, location: OrderPickupLocationModel, drIndex: number) {
        if (selectedDrForm === null || drIndex === null) {
            return;
        }

        let _delReqs = this.fmGroup.controls['DeliveryRequests'] as FormArray;
        let selectedDrObj = selectedDrForm ? selectedDrForm.value : null;

        for (let index = 0; index < _delReqs.length; index++) {
            let _drForm = _delReqs.controls[index] as FormGroup;
            let _drObj = _drForm ? _drForm.value : null;
            if (_drObj && selectedDrObj && _drObj.ProductTypeId == selectedDrObj.ProductTypeId && _drObj.JobId == selectedDrObj.JobId && index != drIndex) {
                //set pickup details
                this.setPickupLocation_(_drForm, location);
                //set other details
                _drForm.get('ScheduleQuantityType').patchValue(selectedDrObj.ScheduleQuantityType);
                _drForm.get('Priority').patchValue(selectedDrObj.Priority);
                _drForm.get('IsCommonBadge').patchValue(selectedDrObj.IsCommonBadge);
                _drForm.get('BadgeNo1').patchValue(selectedDrObj.BadgeNo1);
                _drForm.get('BadgeNo2').patchValue(selectedDrObj.BadgeNo2);
                _drForm.get('BadgeNo3').patchValue(selectedDrObj.BadgeNo3);
                _drForm.get('OrderId').patchValue(selectedDrObj.OrderId);
            }
        }
        if (!this.changeDetectorRef['destroyed'])
            this.changeDetectorRef.detectChanges();
    }
    onBulkPlantSelected_(event: any): void {
        this.PickupForm.get('BulkPlant.SiteName').setValue(event.Name);
        this.PickupForm.get('BulkPlant.SiteId').setValue(event.Id);
        this.BulkPlants = this.BulkplantList.slice();
        this.getBulkPlantAddress_(event.Name);
        if (this.PickupForm.get('BulkPlant.SiteName').valid) {
            this.isReadOnly = true;
        }
    }
    onBulkPlantSearched_(event: any): void {
        let keyword = event.target.value.toLowerCase();
        this.BulkPlants = this.BulkplantList.slice().filter(function (elem) {
            return elem.Name && elem.Name.toLowerCase().indexOf(keyword) >= 0;
        });
        let bulkPlantName = this.PickupForm.get('BulkPlant.SiteName').value;
        this.isReadOnly = this.BulkPlants.filter(t => t.Name == bulkPlantName).length > 0;
    }
    onBulkPlantBlur_(event: any): void {
        if (this.BulkPlants.filter(t => t.Name && t.Name.toLowerCase() == event.target.value.toLowerCase()).length > 0) {
            let bulkPlant = this.BulkPlants.find(t => t.Name.toLowerCase() == event.target.value.toLowerCase());
            this.PickupForm.get('BulkPlant.SiteName').setValue(bulkPlant.Name);
            this.PickupForm.get('BulkPlant.SiteId').setValue(bulkPlant.Id);
            this.getBulkPlantAddress_(bulkPlant.Name);
            let bulkPlantName = this.PickupForm.get('BulkPlant.SiteName').value;
            this.isReadOnly = this.BulkPlants.filter(t => t.Name == bulkPlantName).length > 0;
        }
    }
    setCommonPickupFlag_() {
        if (this.fmGroup.get('IsCommonPickup').value) {
            this.PickupForm.reset();
        } else {
            this.fmGroup.controls['Terminal'].reset()
            this.fmGroup.controls['BulkPlant'].reset();
        }
    }
    setPickupLocation_(form: FormGroup, order: OrderPickupLocationModel): void {
        form.controls['PickupLocationType'].patchValue(order.PickupLocationType);
        if (order.PickupLocationType != 2) {
            if (order.Terminal) {
                form.controls['Terminal'].patchValue(order.Terminal);
            }

            this.PickupForm.controls['PickupLocationType'].patchValue(1);
            this.PickupForm.controls['Terminal'].patchValue(order.Terminal);
        } else {
            if (order.BulkPlant) {
                if (order.BulkPlant.ZipCode) {
                    order.BulkPlant.ZipCode = order.BulkPlant.ZipCode.toUpperCase();
                }
                form.controls['BulkPlant'].patchValue(order.BulkPlant);
            }
            this.PickupForm.controls['PickupLocationType'].patchValue(2);
            this.PickupForm.controls['BulkPlant'].patchValue(order.BulkPlant);
        }
    }


    RemovePickupLocation(tdr: FormGroup, drIndex: number): void {

        if (tdr != null || tdr != undefined) {
            this.SelectedDRForEditPickupIndex = drIndex;
            this.ChangePickupForOrders = [];
            this.SelectedDRForEditPickup = tdr;
            let _pickupType = this.SelectedDRForEditPickup.controls['PickupLocationType'].value;
            _pickupType = _pickupType == 0 ? 1 : _pickupType;

            let terminal = this.utilService.getTerminalForm(new DropdownItem(), false);
            let bulkPlant = this.utilService.getBulkPlantForm(new DropAddressModel(), false);
            let _terminal = terminal.value;
            let _bulkPlant = bulkPlant.value;
            let location = {
                PickupLocationType: _pickupType,
                Terminal: _terminal,
                BulkPlant: _bulkPlant
            };
            this.setPickupLocation_(this.SelectedDRForEditPickup, location);
        }
    }

    get StatesListByCountry(): any[] {
        let countryCode = this.PickupForm.controls['BulkPlant']['controls'].Country.get("Code").value;
        if (countryCode != "" && countryCode != undefined && this.CountryList != undefined && this.CountryList.length > 0) {
            let country = this.CountryList.filter(c => c.Code == countryCode);
            let countryId = 0;
            if (country) {
                countryId = country[0].Id;
            }
            if (countryId == 4) {
                let countryGroupCode = this.PickupForm.controls['BulkPlant']['controls'].CountryGroup.get("Id").value;
                return this.StateList.filter(t => t.CountryId == 4 && t.CountryGroupId == countryGroupCode);
            }
            else {
                return this.StateList.filter(t => t.CountryId == countryId);
            }
        }

    }

    editPickupLocation_(tdr: FormGroup, drIndex: number): void {

        this.getRequiredDataForPickupDetails();
        this.SelectedDRForEditPickupIndex = drIndex;
        this.ChangePickupForOrders = [];
        this.SelectedDRForEditPickup = tdr;

        if (tdr && tdr.get('IsBlendedRequest').value) {

            let blendRequests = tdr.get('BlendedRequests').value as BlendedRequest[];

            if (blendRequests && blendRequests.length > 0) {
                blendRequests.forEach(blend => {
                    if (!blend.IsAdditive && blend.OrderId)
                        this.ChangePickupForOrders.push(blend.OrderId);
                });
            }
        }
        else if (tdr == null || tdr == undefined) {
            this.SelectedDRForEditPickup = this.fmGroup
            let _delReqs = this.fmGroup.controls['DeliveryRequests'] as FormArray;
            _delReqs.controls.forEach((x: FormGroup) => {
                let _orderId = x.controls['OrderId'].value as number;
                if (_orderId > 0) {
                    this.ChangePickupForOrders.push(_orderId);
                }
            });
        } else {
            let _orderId = this.SelectedDRForEditPickup.controls['OrderId'].value as number;
            if (_orderId > 0) {
                this.ChangePickupForOrders.push(_orderId);
                let existingOrders = tdr.get('OrderPickupDetails').value || [];
                let orders = existingOrders.filter(t => t.OrderId == _orderId);
                if (orders.length > 0) {
                    let lastIndex = orders[0].PoNumber.lastIndexOf(" - ");
                    this.SearchTerminalFuelType = orders[0].PoNumber.slice(0, lastIndex + 1);
                }
            }
        }
        let _pickupType = this.SelectedDRForEditPickup.controls['PickupLocationType'].value;
        _pickupType = _pickupType == 0 ? 1 : _pickupType;
        let _terminal = this.SelectedDRForEditPickup.controls['Terminal'].value;
        let _bulkPlant = this.SelectedDRForEditPickup.controls['BulkPlant'].value;
        let location = {
            PickupLocationType: _pickupType,
            Terminal: _terminal,
            BulkPlant: _bulkPlant
        };
        this.setPickupLocation_(this.SelectedDRForEditPickup, location);

        //to get bulk plant for auto/manual order
        let orderForBulkPlant = 0;
        if (tdr)
            orderForBulkPlant = this.SelectedDRForEditPickup.controls['OrderId'].value as number;

        this.addresService.getBulkPlants('', orderForBulkPlant).subscribe(data => {
            this.BulkPlants = data.slice();
            this.BulkplantList = data;

            let bulkPlantName = this.PickupForm.get('BulkPlant.SiteName').value;
            this.isReadOnly = this.BulkPlants.some(t => t.Name == bulkPlantName);
        });

        if (this.ChangePickupForOrders.length > 0) {
            this.DGSubscription.add(this.sbService.getPickupTerminals(this.ChangePickupForOrders, '').subscribe((data: DropdownItem[]) => {
                this.Terminals = data;
            }));
        }
        else {
            this.Terminals = [];
        }

        if (this.BulkPlants) {
            let bulkPlantName = this.PickupForm.get('BulkPlant.SiteName').value;
            this.isReadOnly = this.BulkPlants.filter(t => t.Name == bulkPlantName).length > 0;
        }
        if (location.PickupLocationType == 2) {
            if (this.SelectedDRForEditPickup.controls['BulkPlant'].value) {
                let bulkPlant = this.SelectedDRForEditPickup.controls['BulkPlant'].value;
                if (bulkPlant.Country) {
                    let countryCode = bulkPlant.Country.Code
                    if (countryCode) {
                        this.setBulkPlantAddressValidators(this.getCountryIdFromCode(countryCode));
                    }
                }
            }
        }
    }


    setBulkPlantValidators_(form: FormGroup, required: boolean) {
        form.get('BulkPlant.Address').setValidators(required ? [Validators.required] : null);
        form.get('BulkPlant.Address').updateValueAndValidity();
        form.get('BulkPlant.City').setValidators(required ? [Validators.required] : null);
        form.get('BulkPlant.City').updateValueAndValidity();
        form.get('BulkPlant.State.Id').setValidators(required ? [Validators.required] : null);
        form.get('BulkPlant.State.Id').updateValueAndValidity();
        form.get('BulkPlant.Country.Code').setValidators(required ? [Validators.required] : null);
        form.get('BulkPlant.Country.Code').updateValueAndValidity();
        form.get('BulkPlant.ZipCode').setValidators(required ? [Validators.required] : null);
        form.get('BulkPlant.ZipCode').updateValueAndValidity();
        form.get('BulkPlant.CountyName').setValidators(required ? [Validators.required] : null);
        form.get('BulkPlant.CountyName').updateValueAndValidity();
        form.get('BulkPlant.Latitude').setValidators(required ? [Validators.required, Validators.pattern('^[0-9.-]*$')] : null);
        form.get('BulkPlant.Latitude').updateValueAndValidity();
        form.get('BulkPlant.Longitude').setValidators(required ? [Validators.required, Validators.pattern('^[0-9.-]*$')] : null);
        form.get('BulkPlant.Longitude').updateValueAndValidity();
        form.get('BulkPlant.SiteName').setValidators(required ? [Validators.required] : null);
        form.get('BulkPlant.SiteName').updateValueAndValidity();
    }
    setTerminalValidators_(form: FormGroup, required: boolean) {
        form.get('Terminal.Name').setValidators(required ? [Validators.required] : null);
        form.get('Terminal.Name').updateValueAndValidity();
        form.get('Terminal.Id').setValidators(required ? [Validators.required] : null);
        form.get('Terminal.Id').updateValueAndValidity();
    }
    setPickupValidators_(form: FormGroup, pickupType: number): void {
        if (pickupType != 2) {
            this.setBulkPlantValidators_(form, false);
            this.setTerminalValidators_(form, true);
        } else {
            this.setTerminalValidators_(form, false);
            this.setBulkPlantValidators_(form, true);
        }
    }
    initPickupForm_(order: OrderPickupDetailModel): FormGroup {
        let isTerminalPickup = order && order.PickupLocationType != 2;
        let _pForm = this.fb.group({
            PickupLocationType: this.fb.control(order.PickupLocationType),
            Terminal: this.utilService.getTerminalForm(null, isTerminalPickup),
            BulkPlant: this.utilService.getBulkPlantForm(null, !isTerminalPickup)
        });
        if (order != null || order != undefined) {
            if (order.PickupLocationType != 2) {
                _pForm.controls['Terminal'].patchValue({ Id: order.TerminalId, Name: order.TerminalName });
            } else {
                _pForm.controls['BulkPlant'].patchValue({
                    Address: order.Address,
                    City: order.City,
                    State: { Id: order.StateId, Code: order.StateCode },
                    Country: { Code: order.CountryCode },
                    ZipCode: order.ZipCode,
                    CountyName: order.CountyName,
                    Latitude: order.Latitude,
                    Longitude: order.Longitude,
                    SiteName: order.BulkplantName
                });
            }
        }
        this.DGSubscription.add(_pForm.controls['PickupLocationType'].valueChanges.subscribe(x => { this.LocationType = x; }));
        return _pForm;
    }
    setStateCode_(event: any) {
        this.PickupForm.get('BulkPlant.State.Code').setValue(event.target.selectedOptions[0].text);
    }
    onTerminalSelected_(event: any): void {
        this.PickupForm.get('Terminal').patchValue({ Id: event.Id, Name: event.Name });
        this.changeDetectorRef.detectChanges();
    }
    onTerminalSearched_(event: any): void {
        let keyword = event.target.value.toLowerCase();
        this.SearchTerminalKey = keyword;
        let searchKeyword$ = of(keyword);
        this.DGSubscription.add(searchKeyword$.pipe(
            debounceTime(500),
            distinctUntilChanged(),
            tap((data) => {
                this._loadingTerminals = true
                if (data.length < 3 && data.length != 0) {
                    this.minCharRequired = true;
                } else {
                    this.minCharRequired = false;
                }
            }),
            switchMap((term) => iif(
                () => (term.length < 3 && term.length != 0),
                of([]), this.sbService.getPickupTerminals(this.ChangePickupForOrders, keyword)
                    .pipe(tap(() => { this._loadingTerminals = false; }),
                        catchError(() => {
                            this._loadingTerminals = false;
                            this.searchError = true;
                            return of([]);
                        })
                    ))),
            tap(() => this._loadingTerminals = false)).subscribe((data) => {
                this._loadingTerminals = false;
                if (data.length === 0) {
                    this.noTerminalFound = true;
                } else {
                    this.noTerminalFound = false;
                }
                this.Terminals = data;
                this.changeDetectorRef.detectChanges();
            },
                (err) => {
                    console.log(err);
                }));
    }
    public validateTerminal_(terminal: any, event: any): void {
        if (!terminal.get('Id').value) {
            terminal.get('Name').patchValue('');
        }
    }
    addressMapper_(data: any): AddressModel {
        let state = this.StateList.find(x => x.Code == data.StateCode);
        let country = this.CountryList.find(x => x.Code == data.CountryCode);
        let _address = new AddressModel();
        _address.Address = data.Address;
        _address.City = data.City;
        _address.CountyName = data.CountyName
        _address.Latitude = data.Latitude;
        _address.Longitude = data.Longitude;
        _address.ZipCode = data.ZipCode;
        _address.State = state;
        _address.Country = country;
        return _address;
    }
    getAddressByZip_(event: any): void {
        let zipCode = event.target.value;
        let regexUsa = new RegExp(RegExConstants.UsaZip);
        let regexCan = new RegExp(RegExConstants.CanZip);
        if (regexUsa.test(zipCode) || regexCan.test(zipCode)) {
            this._loadingAddress = true;
            this.DGSubscription.add(this.addresService.getAddress(zipCode)
                .subscribe(data => {
                    this._loadingAddress = false;
                    if (data != null && data != undefined && data.StateCode != null) {
                        if (data.CountryCode != 'CAR') {
                            data.CountryCode == 'US' || data.CountryCode == 'USA' ? data.CountryCode = 'USA' : data.CountryCode = 'CAN';
                        }
                        let addressModel = this.addressMapper_(data);
                        this.PickupForm.get('BulkPlant').patchValue({
                            City: addressModel.City,
                            State: addressModel.State,
                            Country: { Code: addressModel.Country.Code },
                            ZipCode: addressModel.ZipCode,
                            CountyName: addressModel.CountyName,
                            Latitude: addressModel.Latitude,
                            Longitude: addressModel.Longitude
                        });
                        this.PickupForm.markAllAsTouched();
                        this.PickupForm.markAsDirty();
                        if (addressModel.Country.Code != "USA" && addressModel.Country.Code != "US") {
                            this.CountryBasedZipcodeLabel = "Postal Code";
                        }
                        else {
                            this.CountryBasedZipcodeLabel = "Zip";
                        }
                    }
                }));
        }
    }
    updatePickupLocation_() {
        if (this.SelectedDRForEditPickup != null) {
            let isCommon = false;
            let pickupLoc = this.PickupForm.value;
            this.SelectedDRForEditPickup.patchValue(pickupLoc);
            this.setPickupLocation_(this.SelectedDRForEditPickup, pickupLoc);

            //set badges for dr by terminal and order
            let _orderId = this.SelectedDRForEditPickup.value.OrderId;
            let _pickupLocationType = pickupLoc.PickupLocationType;
            let _pickupLocationId = (_pickupLocationType != 2) ? (pickupLoc.Terminal.Id) : (pickupLoc.BulkPlant.SiteId);
            let orderIds = [];
            let selectedFormValue = this.SelectedDRForEditPickup.value;
            if (selectedFormValue.hasOwnProperty('DeliveryRequests')) {
                isCommon = true;
                if (selectedFormValue.DeliveryRequests.length > 0) {
                    orderIds = selectedFormValue.DeliveryRequests.map(function (a) { return a["OrderId"]; });
                }
            }
            else {
                orderIds.push(_orderId);
            }
            //this.dipTestLoader = true;
            this.DGSubscription.add(this.sbService.getOrderBadgesByTerminal(orderIds, _pickupLocationType, _pickupLocationId).subscribe((data: any) => {
                //this.dipTestLoader = false;
                if (data && data.length > 0) {
                    if (!isCommon) {
                        if (this.SelectedDRForEditPickup.value.IsCommonBadge == false || (data[0].BadgeNo1 && data[0].BadgeNo1.length > 0) || (data[0].BadgeNo2 && data[0].BadgeNo2.length > 0) || (data[0].BadgeNo3 && data[0].BadgeNo3.length > 0)) {
                            this.SelectedDRForEditPickup.controls['BadgeNo1'].setValue(data[0].BadgeNo1);
                            this.SelectedDRForEditPickup.controls['BadgeNo2'].setValue(data[0].BadgeNo2);
                            this.SelectedDRForEditPickup.controls['BadgeNo3'].setValue(data[0].BadgeNo3);
                            this.SelectedDRForEditPickup.controls['IsCommonBadge'].setValue(false);
                        }
                    }
                    else {
                        let _drArray = this.SelectedDRForEditPickup.controls['DeliveryRequests'] as FormArray;
                        _drArray.controls.forEach((_drForm: FormGroup) => {
                            if (_drForm.value.OrderId && _drForm.value.OrderId > 0) {
                                let badgeInfo = data.find(t => t.OrderId == _drForm.value.OrderId);
                                if (_drForm.value.IsCommonBadge == false || (badgeInfo.BadgeNo1 && badgeInfo.BadgeNo1.length > 0) || (badgeInfo.BadgeNo2 && badgeInfo.BadgeNo2.length > 0) || (badgeInfo.BadgeNo3 && badgeInfo.BadgeNo3.length > 0)) {
                                    _drForm.controls['BadgeNo1'].setValue(badgeInfo.BadgeNo1);
                                    _drForm.controls['BadgeNo2'].setValue(badgeInfo.BadgeNo2);
                                    _drForm.controls['BadgeNo3'].setValue(badgeInfo.BadgeNo3);
                                    _drForm.controls['IsCommonBadge'].setValue(false);
                                }
                            }
                        });
                    }
                }
                this.findDrWithSameProductAndJob(this.SelectedDRForEditPickup, _orderId, pickupLoc, this.SelectedDRForEditPickupIndex);
            }));
        }

        let dismiss = document.getElementById('btnDrPickupClose') as HTMLElement;
        dismiss.click();
    }

    private subscribeCommonPickupLocationChange_(): void {
        this.DGSubscription.add(this.fmGroup.controls['IsCommonPickup'].valueChanges
            .subscribe(x => {
                x ? this.disableDeliveryGroupPickups_() : this.enableDeliveryGroupPickups_();
                this.setCommonPickupFlag_();
                this.setPickupValidators_(this.fmGroup, x);
            }));
    }
    private PickupLocationTypeChange_(): void {
        this.DGSubscription.add(this.PickupForm.controls['PickupLocationType'].valueChanges.subscribe((data) => {
            this.PickupForm.markAllAsTouched;
            this.PickupForm.markAsDirty();
            this.setPickupValidators_(this.PickupForm, data);
            let commonPickup = this.fmGroup.controls['IsCommonPickup'].value;
            commonPickup ? this.disableDeliveryGroupPickups_() : this.enableDeliveryGroupPickups_();
        }));
    }
    private disableDeliveryGroupPickups_(): void {
        let _drArray = this.fmGroup.controls['DeliveryRequests'] as FormArray;
        if (_drArray) {
            _drArray.controls.forEach((x: FormGroup) => {
                x.controls['Terminal'].disable();
                x.controls['BulkPlant'].disable();
            });
        }
        if (this.fmGroup.controls['PickupLocationType'].value == 2) {
            this.fmGroup.controls['BulkPlant'].enable();
            this.fmGroup.controls['Terminal'].disable();

        } else {
            this.fmGroup.controls['BulkPlant'].disable();
            this.fmGroup.controls['Terminal'].enable();
        }
    }
    private enableDeliveryGroupPickups_(): void {
        let _drArray = this.fmGroup.controls['DeliveryRequests'] as FormArray;
        if (_drArray) {
            _drArray.controls.forEach((x: FormGroup) => {
                x.controls['Terminal'].enable();
                x.controls['BulkPlant'].enable();
            });
        }
        this.fmGroup.controls['BulkPlant'].disable();
        this.fmGroup.controls['Terminal'].disable();
    }
    private unsubscribeAllSubscriptions_(): void {
        if (this.DGSubscription) {
            this.DGSubscription.unsubscribe();
        }
    }
    ngAfterViewInit(): void {
        this.subscribeCommonPickupLocationChange_();
    }
    // end
    getSiteList() {
        this.selectedCustomer = [];
        this.selectedLocation = [];
        this.companySiteList = [];
        this.SiteList = [];
        this.OrderList = [];
        this.selectedOrder = [];

        if (this.SelectedRegionId != null && this.SelectedRegionId != undefined && this.SelectedRegionId != '') {
            this.loadingCustomers = true;
            this.carrierService.getJobListForCarrier(this.SelectedRegionId).subscribe(t2 => {
                this.companySiteList = t2 as CustomerJobsForCarrier[];
                this.loadingCustomers = false;
                let ele = document.getElementById('loadingCustomers');
                if (ele) {
                    ele.click()
                }
                this.loadDefaultLocations();
            });
        }
    }

    getAllDipTest() {
        if (this.SelectedRegionId != null && this.SelectedRegionId != undefined && this.SelectedRegionId != '') {
            this.clearRaiseRequests();
            let regionId = this.SelectedRegionId;
            this.dipTestLoader = true;
            const customerId = this.SelectedCustomerId == null ? 0 : parseInt(this.SelectedCustomerId);
            this.carrierService.getDipTests(null, this.SelectedRegionId, customerId, this.RequestFromBuyerWallyBoard, this.showForm).subscribe(data => {
                this.DipTestsForEachTank = data;
                this.dipTestLoader = false;
                let companyId = this.getCustomerForJob();
                if (data.length == 0 && companyId != "") {
                    this.isTankExists = false;
                    this.getOrdersForJob(0, companyId, regionId);
                }
                else {
                    this.isTankExists = true;
                    this.fillRaiseDrForm();
                }
            });
        }
    }

    getDipTestForSite(_jobId: any, _regionId: any) {
        this.clearRaiseRequests();
        this.dipTestLoader = true;
        const customerId = this.SelectedCustomerId == null ? 0 : parseInt(this.SelectedCustomerId);
        this.carrierService.getDipTests(_jobId, _regionId, customerId, this.RequestFromBuyerWallyBoard, this.showForm).subscribe(data => {
            this.DipTestsForEachTank = data;
            let companyId = this.getCustomerForJob();

            if (data.length == 0 && companyId != "") {
                this.isTankExists = false;
                this.getOrdersForJob(_jobId, companyId, _regionId);
            }
            else {
                this.isTankExists = true;
                let productIds: number[] = this.DipTestsForEachTank.map(x => x.ProductTypeId);
                this.getProductExcludedOrdersForJob(this.SelectedLocationId, companyId, this.SelectedRegionId, productIds);
            }
        });
    }

    getOrdersForJob(_jobId: any, _customerId: any, _regionId: any) {
        this.dipTestLoader = true;
        let skipMarineConversion = false;
        this.carrierService.getOrdersForJob(_jobId, _customerId, _regionId, skipMarineConversion).subscribe(data => {
            if (data != null) {
                
                //US to USA
                if (data.DeliveryReqInput && data.DeliveryReqInput.length > 0 && data.DeliveryReqInput.some(x => x.OrderPickupDetails && x.OrderPickupDetails.length > 0 && x.OrderPickupDetails.some(y => y.CountryCode == "US"))) {
                    data.DeliveryReqInput.forEach((request: DipTestViewModel) => {
                        request.OrderPickupDetails.forEach(p => {
                            if (p.CountryCode == "US") 
                                p.CountryCode = "USA";
                        })
                    });
                }

                this.OrderList = data.OrderList;
                this.selectedOrder = [];
                this.OrderDetails = data.DeliveryReqInput;
                this.DipTestsForEachTank = data.DeliveryReqInput.filter(t => t.ProductTypeId != additiveProductTypeId);
            }
            this.dipTestLoader = false;
            this.showForm = true;
            this.fillRaiseDrForm();
        });
    }

    getUoM(_orderId: any, blendedRequest: FormGroup) {
        if (_orderId) {
            let order = this.additiveOrders.find(t => t.Id == _orderId);
            if (order) {
                blendedRequest.get('UoM').setValue(order.UoM);
            }
            else {
                blendedRequest.get('UoM').setValue(this.DipTestsForEachTank[this.DrForEditBlendedRequestIndex]?.UoM);
            }
        }
        else {
            blendedRequest.get('UoM').setValue(this.DipTestsForEachTank[this.DrForEditBlendedRequestIndex]?.UoM);
        }
    }

    //GET ORDERS EXCLUDING TANK PRODUCTS
    getProductExcludedOrdersForJob(_jobId: any, _customerId: any, _regionId: any, productsToExclude: number[]) {
        this.dipTestLoader = true;
        let skipMarineConversion = false;
        this.carrierService.getOrdersForJob(_jobId, _customerId, _regionId, skipMarineConversion, 0, productsToExclude).subscribe(data => {
            if (data != null) {

                //US to USA
                if (data.DeliveryReqInput && data.DeliveryReqInput.length > 0 && data.DeliveryReqInput.some(x => x.OrderPickupDetails && x.OrderPickupDetails.length > 0 && x.OrderPickupDetails.some(y => y.CountryCode == "US"))) {
                    data.DeliveryReqInput.forEach((request: DipTestViewModel) => {
                        request.OrderPickupDetails.forEach(p => {
                            if (p.CountryCode == "US")
                                p.CountryCode = "USA";
                        })
                    });
                }

                this.OrderList = data.OrderList;
                this.selectedOrder = [];
                this.OrderDetails = data.DeliveryReqInput;
                this.OtherProductDipTestsForEachTank = data.DeliveryReqInput;
                //this.DipTestsForEachTank = this.DipTestsForEachTank.concat(data.DeliveryReqInput)
            }
            this.dipTestLoader = false;
            this.showForm = true;
            this.fillOtherProductRaiseDrForm();
        });
    }
    //FILL DR FORM WITH OTHER PRODUCTS
    fillOtherProductRaiseDrForm() {
        let prods = this.fmGroup.get('DeliveryRequests') as FormArray;
        let currentObj = this;
        if (this.OtherProductDipTestsForEachTank != null && this.OtherProductDipTestsForEachTank != undefined && this.OtherProductDipTestsForEachTank.length > 0) {

            this.DipTestsForEachTank = this.DipTestsForEachTank.concat(this.OtherProductDipTestsForEachTank);

            this.DipTestsForEachTank.filter(t => t.ProductTypeId != additiveProductTypeId).forEach(function (element: DipTestViewModel, index: number) {
                prods.push(currentObj.buildRaiseDRForm(element));
            });
            if (!this.changeDetectorRef['destroyed']) {
                this.changeDetectorRef.detectChanges();
            }
        }
        else {
            this.fillRaiseDrForm();
        }
    }
    
    fillRaiseDrForm() {
        let prods = this.fmGroup.get('DeliveryRequests') as FormArray;
        let currentObj = this;
        if (this.DipTestsForEachTank != null && this.DipTestsForEachTank != undefined) {
            prods.clear();
            this.DipTestsForEachTank.forEach(function (element: DipTestViewModel, index: number) {
                prods.push(currentObj.buildRaiseDRForm(element));
            });
            if (!this.changeDetectorRef['destroyed']) {
                this.changeDetectorRef.detectChanges();
            }
        }
    }

    openRaiseDrPanel() {
        this.selectedLocation = [];
        //this.BuyerCompanyName = "";
        this.clearRaiseRequests();
        this.getSiteList();
        this.getAllDipTest();
        this.getCreateDrSetting();
    }

    clearRaiseRequests() {
        let controls = <FormArray>this.fmGroup.controls['DeliveryRequests'];
        controls.clear();
    }

    createDR() {
        this.showForm = true;
        this.getAllDipTest();
    }

    loadTankDR(sales: SalesDataModel) {
        this.SelectedRegionId = sales.RegionId;
        this.SelectedLocationId = sales.TfxJobId.toString();
        this.getSiteList();
        this.selectedLocation = [];
        this.clearRaiseRequests();
        let job = new JobRegionModel();
        job.Id = sales.TfxJobId;
        job.Code = sales.SiteId;
        job.Name = sales.SiteId;
        job.LocationManagedType = sales.LocationManagedType;
        this.selectedLocation = [job];
        this.showForm = true;
        this.getDipTestForSite(sales.TfxJobId, sales.RegionId);
    }

    buildRaiseDRForm(model: DipTestViewModel): FormGroup {
        const orderPickupDetailsExist = (model.OrderPickupDetails && model.OrderPickupDetails.length > 0) ? true : false;
        const blendOrderPickupDetailsExist = (model.BlendOrderPickupDetails && model.BlendOrderPickupDetails.length > 0) ? true : false;
        const isTerminalPickup = orderPickupDetailsExist && model.OrderPickupDetails[0].PickupLocationType != 2;
        let tanks = (model && model.Tanks && model.Tanks.length > 0) ? model.Tanks : [];
        let _form = this.fb.group({
            Id: this.fb.control(model.Id),
            SiteId: this.fb.control(model.SiteId == null ? model.JobId.toString() : model.SiteId),
            JobId: this.fb.control(model.JobId),

            TankId: this.fb.control(model.TankId),
            StorageId: this.fb.control(model.StorageId),
            ScheduleQuantityType: this.fb.control(1),
            RequiredQuantity: this.fb.control(''),
            IsTankAndAssetAvailableForJob: this.fb.control(model.IsTankAndAssetAvailableForJob),
            IsMaxFillAllowed: this.fb.control(false),
            IsReAssignToCarrier: this.fb.control(model.IsReAssignToCarrier),
            Priority: this.fb.control(model.Priority.toLocaleString()),
            CreatedByRegionId: this.fb.control(this.SelectedRegionId),
            CurrentThreshold: this.fb.control(model.CurrentThreshold),
            TankMaxFill: this.fb.control(model.TankMaxFill),
            BuyerCompanyId: this.fb.control(model.BuyerCompanyId),
            RecurringSchdule: this.fb.array([]),
            CustomerCompany: this.fb.control(model.BuyerCompanyName),
            isRecurringSchedule: this.fb.control(model.isRecurringSchedule),
            PoNumber: this.fb.control(model.PoNumber),
            TankName: this.fb.control(model.TankName),
            Notes: this.fb.control(orderPickupDetailsExist ? model.OrderPickupDetails[0].DRNote : ''),
            isRetainInfo: this.fb.control(false),
            RetainInfo: this.fb.array([]),
            IsRetainButtonClick: this.fb.control(false),
            //isTankExists: this.fb.control(this.isTankExists),
            SupplierCompanyId: this.fb.control((model.SupplierCompanies.length > 0) ? model.SupplierCompanies[0].Id : model.SupplierCompanyId),
            SupplierCompanies: this.fb.control(model.SupplierCompanies),
            RequestFromBuyerWallyBoard: this.fb.control(this.RequestFromBuyerWallyBoard),
            IsCommonBadge: this.fb.control(model.IsCommonBadge),
            BadgeNo1: this.fb.control(orderPickupDetailsExist ? model.OrderPickupDetails[0].Badge1 : null),
            BadgeNo2: this.fb.control(orderPickupDetailsExist ? model.OrderPickupDetails[0].Badge2 : null),
            BadgeNo3: this.fb.control(orderPickupDetailsExist ? model.OrderPickupDetails[0].Badge3 : null),
            OrderId: this.fb.control(orderPickupDetailsExist ? model.OrderPickupDetails[0].OrderId : model.OrderId),
            PickupLocationType: this.fb.control(orderPickupDetailsExist ? model.OrderPickupDetails[0].PickupLocationType : null),
            ProductTypeId: this.fb.control(model.ProductTypeId),
            FuelTypeId: this.fb.control(model.FuelTypeId),
            Terminal: this.utilService.getTerminalForm(null, false),
            BulkPlant: this.utilService.getBulkPlantForm(null, false),
            OrderPickupDetails: this.fb.control(orderPickupDetailsExist ? model.OrderPickupDetails : []),
            BlendOrderPickupDetails: this.fb.control(blendOrderPickupDetailsExist ? model.BlendOrderPickupDetails : []),
            //BLENDED DR
            IsBlendedRequest: this.fb.control(false),
            BlendedRequests: this.fb.array([]),
            IsCommonPickupForBlend: this.fb.control(false),
            BlendedGroupId: this.fb.control(null),
            Tanks: this.fb.control(tanks),
            SelectedDate: this.fb.control(model.SelectedDate || moment(new Date()).format('MM/DD/YYYY')),
            IsFutureDR: this.fb.control(false),
            IsCalendarView: this.fb.control(false),
            DeliveryLevelPO: this.fb.control(null),
            ScheduleStartTime: this.fb.control(null),
            ScheduleEndTime: this.fb.control(null),
            IndicativePrice: this.fb.control(null),
            //bind required API response in the form
            ProductName: this.fb.control(model.ProductName),
            JobName: this.fb.control(model.JobName),
            DisplayCaptureTime: this.fb.control(model.DisplayCaptureTime),
            TankCapacity: this.fb.control(model.TankCapacity),
            UoM: this.fb.control(model.UoM),
            NetVolume: this.fb.control(model.NetVolume),
            Ullage: this.fb.control(model.Ullage),
            ReorderQuantity: this.fb.control(model.ReorderQuantity),
            DisplayDRDetails: this.fb.control(model.DisplayDRDetails),
            ExistingDR: this.fb.control(model.ExistingDR),
            BuyerCompanyName: this.fb.control(model.BuyerCompanyName),
            IsDRExists: this.fb.control(model.IsDRExists),

        });

        if (orderPickupDetailsExist && isTerminalPickup) {
            _form.controls['Terminal'].patchValue({ Id: model.OrderPickupDetails[0].TerminalId, Name: model.OrderPickupDetails[0].TerminalName });
        }
        else if (orderPickupDetailsExist) {
            _form.controls['BulkPlant'].patchValue({
                Address: model.OrderPickupDetails[0].Address,
                City: model.OrderPickupDetails[0].City,
                State: { Id: model.OrderPickupDetails[0].StateId, Code: model.OrderPickupDetails[0].StateCode },
                Country: { Code: model.OrderPickupDetails[0].CountryCode },
                ZipCode: model.OrderPickupDetails[0].ZipCode,
                CountyName: model.OrderPickupDetails[0].CountyName,
                Latitude: model.OrderPickupDetails[0].Latitude,
                Longitude: model.OrderPickupDetails[0].Longitude,
                SiteName: model.OrderPickupDetails[0].BulkplantName,
                SiteId: model.OrderPickupDetails[0].SiteId
            });
        }
        return _form;
    }
    buildRecurringSchedule(model: any) {

        let selected = []
        if (model.WeekDayId && model.WeekDayId.length > 0) {
            selected = this.ScheduleDaysDetails.filter(s => model.WeekDayId.includes(s.id));
        }
        return this.fb.group({
            Id: this.fb.control(model.Id),
            ScheduleType: this.fb.control(model.ScheduleType),
            WeekDayId: this.fb.control(model.WeekDayId),
            TempWeekDayId: this.fb.control(selected),
            MonthDayId: this.fb.control(model.MonthDayId),
            Date: this.fb.control(model.Date == '' ? this.currentdate : model.Date),
            ScheduleQuantityType: this.fb.control(model.ScheduleQuantityType),
            RequiredQuantity: this.fb.control(model.RequiredQuantity),
            TankName: this.fb.control(model.TankName),
            IsBlendedProduct: this.fb.control(model.IsBlendedRequest),
            RecurringBlendedGroupId: this.fb.control(getRecurringUniqueId()),
            DeliveryLevelPO: this.fb.control(model.DeliveryLevelPO),
        });
    }
    getDefaultRecurringScheduleDetails(TankName: string, IsBlendedRequest: boolean) {
        return this.fb.group({
            Id: this.fb.control(''),
            ScheduleType: this.fb.control(1),
            WeekDayId: this.fb.control([]),
            TempWeekDayId: this.fb.control([]),
            MonthDayId: this.fb.control(''),
            Date: this.fb.control(this.currentdate),
            ScheduleQuantityType: this.fb.control(1),
            RequiredQuantity: this.fb.control(''),
            TankName: this.fb.control(TankName),
            IsBlendedProduct: this.fb.control(IsBlendedRequest),
            RecurringBlendedGroupId: this.fb.control(getRecurringUniqueId()),
            DeliveryLevelPO: this.fb.control(''),
        });
    }


    public loadChart() {
        // let siteId = this.SiteList.filter(t => t.Id == item.Id)[0].Code;
        if (this.selectedLocation && this.selectedLocation.length > 0) {
            this.chartdata = null;
            this.isChartDataExistSubject.next(false);
            let siteId = this.SiteList.filter(t => t.Id == this.selectedLocation[0].Id)[0].Code;
            this.chartdata = { siteId: siteId, noOfDays: 3, tfxJobId: this.selectedLocation[0].Id };
            this.isChartDataExistSubject.next(true);
        }
    }

    public checkLocationAssignment(jobId: number) {
        this.dipTestLoader = true;
        this.carrierService.checkLocationAssignedToCarrier(jobId)
            .subscribe((data: any) => {
                this.dipTestLoader = false;
                let result = data != null ? data.Result : null;
                if (result != null && result.StatusCode == 0 && result.ResponseData != null && result.ResponseData != undefined) {
                    if (result.ResponseData.IsLocationAssignedToCarrier) {
                        this.FormValidationMessage = result.StatusMessage;
                    }
                    else {
                        this.FormValidationMessage = "";
                    }
                }
                else {
                    this.FormValidationMessage = "";
                }
            });
    }

    public changeActiveTab(priority) {
        this.activePriorityTab = priority;
    }

    public onSiteSelect(item: DropdownItem): void {
        this.OtherProductDipTestsForEachTank = [];
        //this.showOtherProducts = false;
        this.fmGroup.controls['IsTankNotAvailableForOrderProducts'].setValue(false);
        this.showForm = true;
        this.SelectedLocationId = item.Id.toString();
        this.getDipTestForSite(item.Id, this.SelectedRegionId);

        let element: HTMLElement = document.getElementById('idTankTab') as HTMLElement;
        element ? element.click() : '';
        this.checkLocationAssignment(item.Id);
        //this.getSalesData();
        this.height_Panel = this.calHeight();
        this.setLocationManagedType();
    }

    LocationManagedType = 0;
    public setLocationManagedType() {
        this.LocationManagedType = 0;
        if (this.SiteList.length > 0 && this.selectedLocation.length > 0) {
            this.LocationManagedType = this.SiteList.filter(t => t.Id == this.selectedLocation[0].Id)[0].LocationManagedType;
        }
    }

    public calHeight() {
        return this.viewportToPixels('100vh') - (this.elementFilter ? this.elementFilter.nativeElement.offsetHeight + 160 : 160);
    }

    public viewportToPixels(value) {
        let parts = value.match(/([0-9\.]+)(vh|vw)/)
        let q = Number(parts[1])
        let side = window[['innerHeight', 'innerWidth'][['vh', 'vw'].indexOf(parts[2])]]
        return side * (q / 100)
    }

    public onSiteDeSelect(item: any): void {
        this.OtherProductDipTestsForEachTank = [];
        //this.showOtherProducts = false;
        this.fmGroup.controls['IsTankNotAvailableForOrderProducts'].setValue(false);
        this.selectedLocation = [];
        this.selectedOrder = [];
        this.OrderList = [];
        //this.BuyerCompanyName = "";
        this.clearRaiseRequests();
        this.showForm = !(this.selectedLocation.length == 0)
        this.chartdata = null;
        this.isChartDataExistSubject.next(false);
        let element: HTMLElement = document.getElementById('idTankTab') as HTMLElement;
        element ? element.click() : '';
        this.FormValidationMessage = "";
        this.SelectedLocationId = null;
        this.getAllDipTest();
        this.LocationManagedType = 0;
    }
    public onSelectAll(item: any): void {
        this.selectedOrder = item;
        this.onOrderChange();
    }

    public onOrderChange(): void {
        let ids = this.selectedOrder.map(t => t.Id);
        this.clearRaiseRequests();
        if (ids.length > 0) {
            this.DipTestsForEachTank = this.OrderDetails.filter(t => ids.indexOf(t.OrderId) != -1);
        }
        else {
            this.DipTestsForEachTank = this.OrderDetails;
        }
        this.fillRaiseDrForm();
        this.showForm = true;
        this.height_Panel = this.calHeight();
    }

    public onCustomerSelect(item: any): void {
        this.OtherProductDipTestsForEachTank = [];
        //this.showOtherProducts = false;
        this.fmGroup.controls['IsTankNotAvailableForOrderProducts'].setValue(false);
        this.selectedLocation = [];
        this.selectedOrder = [];
        this.SelectedCustomerId = item.CompanyId;
        this.SiteList = this.companySiteList.find(x => x.CompanyId == item.CompanyId).Jobs;
        this.getAllDipTest();
        this.height_Panel = this.calHeight();
    }

    public onCustomerDeSelect(item: any): void {
        this.OtherProductDipTestsForEachTank = [];
        //this.showOtherProducts = false;
        this.fmGroup.controls['IsTankNotAvailableForOrderProducts'].setValue(false);
        this.SelectedCustomerId = null;
        this.loadDefaultLocations();
        this.OrderList = [];
        this.onSiteDeSelect(item);
    }

    loadDefaultLocations() {
        this.SiteList = [];
        this.SiteList = this.companySiteList.reduce((j, cj) => [...j, ...cj.Jobs], []);
    }

    getCustomerForJob(): string {
        let customerCompanyId: string = "";
        if (this.SelectedCustomerId != null) {
            customerCompanyId = this.SelectedCustomerId;
        }
        else if (this.SelectedLocationId != null) {
            let jobId = parseInt(this.SelectedLocationId);
            let company = this.companySiteList.find(t => t.Jobs.some(t1 => t1.Id == jobId));
            if (company != undefined && company != null) {
                customerCompanyId = company.CompanyId.toString();
            }
        }
        return customerCompanyId;
    }

    public UpdateHeldDrCount() {
        this.carrierService.updateHeldDrCount().subscribe((data: any) => {
            if (data && data > 0) {
                $("#heldDrIcon").removeClass('hide');
                $("#heldDrsCount").text(data);
            }
        })
    }

    public onSubmit(): void {
        this.isUserSubmit = true;
        let isSapRequest = false;
        let _requests = this.fmGroup.get("DeliveryRequests").value as any[];

        if (_requests.length <= 0) {
            Declarations.msgerror('No Tank found to raise DR', undefined, undefined);
            return;
        }
        var delRequests: DeliveryRequestViewModel[] = [];
        //TANK MAX FILL VALIDATION
        let _invalidQuantityRequests = 0;
        //VALID DR COUNT
        let _validRequests = 0;
        //INVALID DAYS FOR RECURRING DR
        let _rsInvalidDays = 0;
        //INVALID DAYS FOR RECURRING DR
        let _rsInvalidDate = 0;
        //INVALID DAYS FOR RECURRING DR
        let _rsInvalidQuantity = 0;
        _requests.forEach(function (request) {
            if (request.ScheduleQuantityType == 1 && request.TankMaxFill > 0 && request.RequiredQuantity > request.TankMaxFill && !request.IsMaxFillAllowed) {
                _invalidQuantityRequests++;
                delRequests.push(request);
            }
            else if (request.ScheduleQuantityType > 1 || request.isRecurringSchedule || request.RequiredQuantity > 0) {
                _validRequests++;
            }
            if (request.isRecurringSchedule) {
                if (request.RecurringSchdule.length > 0) {
                    request.RecurringSchdule.forEach(function (rcs) {
                        if ((rcs.ScheduleType == 1 || rcs.ScheduleType == 2) && (rcs.TempWeekDayId.length == 0)) {
                            _rsInvalidDays++;
                        }
                        else if (rcs.ScheduleType == 3 && !rcs.Date) {
                            _rsInvalidDate++;
                        }
                        else if (!(rcs.RequiredQuantity > 0) && rcs.ScheduleQuantityType == 1) {
                            _rsInvalidQuantity++;
                        }
                    });
                }
            }
        });
        if (_invalidQuantityRequests > 0) {
            if (delRequests.length > 0) {
                this.sbService.postValidateDRMaxFill(delRequests).subscribe(response => {
                });
            }
            this.isUserSubmit = false;
            Declarations.msgerror('Required quantity is more than Max Fill.', undefined, undefined); return;

        }
        else if (_validRequests == 0) {
            this.isUserSubmit = false;
            Declarations.msgerror('Please enter valid required quantity for at least one tank/order.', undefined, undefined); return;
        }
        else if (_rsInvalidDays > 0) {
            this.isUserSubmit = false;
            Declarations.msgerror('Please select valid days for recurring schedule.', undefined, undefined); return;
        }
        else if (_rsInvalidDate > 0) {
            this.isUserSubmit = false;
            Declarations.msgerror('Please select valid date for recurring schedule.', undefined, undefined); return;
        }
        else if (_rsInvalidQuantity > 0) {
            this.isUserSubmit = false;
            Declarations.msgerror('Invalid required quantity for recurring schedule.', undefined, undefined); return;
        }
        if (this.preferenceSetting?.CreditCheckType == 1) {
            let isValid = true;
            isSapRequest = true;
            _requests.forEach(function (request) {
                if (request.ScheduleQuantityType == 1 && request.RequiredQuantity > 0 && (request.IndicativePrice <= 0 || request.IndicativePrice == null)) {
                    Declarations.msgerror('Please enter Indicative Price.', undefined, undefined);
                    isValid = false;
                }
                if (request.ScheduleQuantityType == 1 && request.RequiredQuantity > 0) {
                    if (request.OrderId == null || request.OrderId == 0) {
                        Declarations.msgerror('Please select Order', undefined, undefined);
                        isValid = false
                    }
                    if ((request.Terminal == null || request.Terminal.Id == 0) && (request.BulkPlant == null || request.BulkPlant.SiteId == 0)) {
                        Declarations.msgerror('Please select Pick up location', undefined, undefined);
                        isValid = false
                    }
                }
            });
            if (!isValid) {
                this.isUserSubmit = false;
                return;
            }
        }
        if (this.fmGroup.valid) {
            this.dipTestLoader = true;
            //bind common pickup to model
            let model = this.fmGroup.getRawValue();
            let drList = model.DeliveryRequests as any[] || [];
            const isCommonPickup = model.IsCommonPickup;

            drList.forEach(dr => {
                if (isCommonPickup) {
                    dr.PickupLocationType = model.PickupLocationType;
                    if (model.PickupLocationType != 2)
                        dr.Terminal = model.Terminal;
                    else
                        dr.BulkPlant = model.BulkPlant;
                    if (dr.BlendedRequests) {
                        dr.BlendedRequests.forEach(blendDr => {
                            blendDr.PickupLocationType = model.PickupLocationType;
                            if (model.PickupLocationType != 2)
                                blendDr.Terminal = model.Terminal;
                            else
                                blendDr.BulkPlant = model.BulkPlant;
                        })
                    }
                }
                if (dr.IsCommonBadge) {
                    dr.BadgeNo1 = model.BadgeNo1;
                    dr.BadgeNo2 = model.BadgeNo2;
                    dr.BadgeNo3 = model.BadgeNo3;
                }
                if (dr.PickupLocationType != 2) {
                    dr.BulkPlant = null;
                }
                else {
                    dr.Terminal = null;
                }
                dr.BlendedRequests.forEach(blendDr => {
                    if (blendDr.PickupLocationType != 2)
                        blendDr.BulkPlant = null;
                    else
                        blendDr.Terminal = null;
                })
                // reccuring dr week days
                if (dr.isRecurringSchedule && dr.RecurringSchdule && dr.RecurringSchdule.length > 0) {
                    dr.RecurringSchdule.forEach(rcs => {
                        if ((rcs.ScheduleType == 1 || rcs.ScheduleType == 2) && (rcs.TempWeekDayId.length > 0)) {
                            rcs.WeekDayId = rcs.TempWeekDayId.map((day: any) => day.id.toString());
                        }
                    });
                }
            });


            this.carrierService.postRaiseRequests(model).subscribe((data: any) => {
                this.isUserSubmit = false;
                if (data != null && parseInt(data.StatusCode) == 0) {
                    this.displayMessage(data);
                    closeSlidePanel();
                    this.clearForm();
                    this.getSiteList();
                    if (!isSapRequest) {
                        this.onRaiseDR.emit();
                        this.emitDataForDRDisplay();
                    }
                    else {
                        this.UpdateHeldDrCount();
                    }
                    this.dipTestLoader = false;
                }
                else {
                    this.dipTestLoader = false;
                    const statusMessage = data == null ? 'Failed' : data.StatusMessage;
                    Declarations.msgerror(statusMessage, undefined, undefined);
                }
                if (!this.changeDetectorRef['destroyed']) {
                    this.changeDetectorRef.detectChanges();
                }
            })
        }
        else {
            this.isUserSubmit = false;
            this.IsLoading = false;
            this.fmGroup.markAllAsTouched();
        }
    }
    private displayMessage(data: any) {
        if (data.StatusMessage.indexOf(this.messageSplitTag) != -1) {
            let dataMessage = data.StatusMessage.split(this.messageSplitTag);
            // Iterate through each value
            for (var i = 0; i < dataMessage.length; i++) {
                // Alert each number
                if (dataMessage[i].indexOf(this.failedMessageIdentification) != -1) {
                    let message = dataMessage[i].replace(this.failedMessageIdentification, '');
                    Declarations.msgerror(message, undefined, undefined);
                }
                else {
                    Declarations.msgsuccess(dataMessage[i], undefined, undefined);
                }
            }
        }
        else {
            Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
        }
    }
    private emitDataForDRDisplay() {
        if (this.IsDrFromMultiWindow) {
            this.OnRaiseDRFromMultiWindow.emit();
        }
    }

    public onSingleSubmit(form: any): void {
        let isSapRequest = false;
        let request = form.value;
        let _rsInvalidDays = 0;
        let _rsInvalidDate = 0;
        let _rsInvalidQuantity = 0;
        let _rsInvalidQuantityMaxFill = 0;

        if (request.isRecurringSchedule != true) {
            if (request.ScheduleQuantityType == 1 && request.TankMaxFill > 0 && request.RequiredQuantity > request.TankMaxFill && !request.IsMaxFillAllowed) {
                Declarations.msgerror('Required quantity is more than Max Fill.', undefined, undefined); return;
            }
            else if (request.ScheduleQuantityType == 1 && !(request.RequiredQuantity > 0)) {
                Declarations.msgerror('Invalid required quantity.', undefined, undefined); return;
            }
            if (this.preferenceSetting?.CreditCheckType == 1) {
                let isValid = true;
                isSapRequest = true;
                if (request.ScheduleQuantityType == 1 && request.RequiredQuantity > 0 && (request.IndicativePrice <= 0 || request.IndicativePrice == null)) {
                    Declarations.msgerror('Please enter Indicative Price.', undefined, undefined);
                    isValid = false;
                }
                if (request.ScheduleQuantityType == 1 && request.RequiredQuantity > 0) {
                    if (request.OrderId == null || request.OrderId == 0) {
                        Declarations.msgerror('Please select Order', undefined, undefined);
                        isValid = false
                    }
                    if ((request.Terminal == null || request.Terminal.Id == 0) && (request.BulkPlant == null || request.BulkPlant.SiteId == 0)) {
                        Declarations.msgerror('Please select Pick up location', undefined, undefined);
                        isValid = false
                    }
                }
                if (!isValid) {
                    return;
                }
            }
        }
        else if (request.isRecurringSchedule == true) {

            if (request.RecurringSchdule.length > 0) {
                request.RecurringSchdule.forEach(function (rcs) {
                    if ((rcs.ScheduleType == 1 || rcs.ScheduleType == 2) && (rcs.WeekDayId.length == 0)) {
                        _rsInvalidDays++;
                    }
                    else if (rcs.ScheduleType == 3 && !rcs.Date) {
                        _rsInvalidDate++;
                    }
                    else if (!(rcs.RequiredQuantity > 0) && rcs.ScheduleQuantityType == 1) {
                        _rsInvalidQuantity++;
                    }
                    else if (rcs.ScheduleQuantityType == 1 && request.TankMaxFill > 0 && rcs.RequiredQuantity > request.TankMaxFill && !request.IsMaxFillAllowed && !request.IsMaxFillAllowed) {
                        _rsInvalidQuantityMaxFill++;
                    }
                });
            }
            else {
                Declarations.msgerror('Invalid recurring schdule.', undefined, undefined); return;
            }
        }

        if (_rsInvalidDays > 0) {
            Declarations.msgerror('Please select valid days for recurring schedule.', undefined, undefined); return;
        }
        else if (_rsInvalidDate > 0) {
            Declarations.msgerror('Please select valid date for recurring schedule.', undefined, undefined); return;
        }
        else if (_rsInvalidQuantity > 0) {
            Declarations.msgerror('Invalid required quantity for recurring schedule.', undefined, undefined); return;
        }
        else if (_rsInvalidQuantityMaxFill) {
            Declarations.msgerror('Required quantity is more than Max Fill for recurring schedule.', undefined, undefined); return;
        }

        this.dipTestLoader = true;
        this.carrierService.postRaiseRequest(request)
            .subscribe((data: any) => {
                if (data.StatusCode == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                    if (!isSapRequest) {
                        this.onRaiseDR.emit();
                    }
                    else {
                        this.UpdateHeldDrCount();
                    }
                    this.dipTestLoader = false;
                    //disable current submitt buttton
                }
                else {
                    Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                    this.dipTestLoader = false;
                }
            })
    }
    clearForm() {
        this.fmGroup.reset();
        this.clearRaiseRequests();
        this.showForm = false;
        this.FormValidationMessage = "";
        // destroy chart
        this.chartdata = null;
        let element: HTMLElement = document.getElementById('idTankTab') as HTMLElement;
        element ? element.click() : '';
    }

    removeTank(formIndex: number) {
        let control = this.fmGroup.controls['DeliveryRequests'] as FormArray;
        control.removeAt(formIndex);
    }
    subscribeDisableControlsSubject(): void {
        if (this.DisableControlsSubscription) {
            this.DisableControlsSubscription.unsubscribe();
        }
        this.DisableControlsSubscription = this.dataService.DisableDSBControlsSubject.subscribe(x => {
            this.disableControl = x;
        });
    }
    public addNewSchedule(product: any): void {
        product.get("RecurringSchdule").push(this.getDefaultRecurringScheduleDetails(product.get('TankName').value, product.get('IsBlendedRequest').value));
        if (product.get('IsBlendedRequest').value) {
            var totalQty = 0;
            var blendedRequests = <FormArray>product.get('BlendedRequests');
            blendedRequests.controls.forEach(x => {
                var requiredQty = x.get('RequiredQuantity').value;
                totalQty = totalQty + requiredQty;
            });
            var deliveryReqControl = <FormArray>product.get('RecurringSchdule');
            deliveryReqControl.controls.forEach(x => {
                if (x.get('Id').value == '' || x.get('Id').value == null) {
                    x.get('RequiredQuantity').setValue(totalQty);
                }
            });
        }
    }
    public enableSchedule(product: any, index: any): void {
        if (!product.get('IsBlendedRequest').value) {
            product.get('RequiredQuantity').setValue(null);
            if (product.get('isRecurringSchedule').value === true) {
                product.get('RequiredQuantity').disable();
                product.get('ScheduleQuantityType').disable();
                product.get('Priority').setValue(1);
                $("#mustgo" + index).prop("checked", true);
                let PoNumber = product.get('PoNumber').value == null ? "" : product.get('PoNumber').value;
                let JobId = product.get('JobId').value;
                let ProductTypeId = product.get('ProductTypeId').value;
                let JobSiteId = product.get('SiteId').value == null ? "" : product.get('SiteId').value;
                this.getRecurringSchedule(product, PoNumber, JobId, JobSiteId, ProductTypeId, index, product.get('IsBlendedRequest').value);

            } else {
                product.get('isRecurringSchedule').setValue(false);
                product.get('RequiredQuantity').enable();
                product.get('ScheduleQuantityType').enable();
                $(".showHide" + index).hide();
            }

        }
        else {
            if (product.get('isRecurringSchedule').value === true) {
                let PoNumber = product.get('PoNumber').value == null ? "" : product.get('PoNumber').value;
                let JobId = product.get('JobId').value;
                let ProductTypeId = product.get('ProductTypeId').value;
                let JobSiteId = product.get('SiteId').value == null ? "" : product.get('SiteId').value;
                this.getRecurringSchedule(product, PoNumber, JobId, JobSiteId, ProductTypeId, index, product.get('IsBlendedRequest').value);

            }
            else {
                $(".showHide" + index).hide();
            }

        }
    }
    public DeleteSchedule(product: any, index: any, Id: any) {
        let recurringSchedule = <FormArray>product.get("RecurringSchdule");
        if (Id == '') {
            if (index > -1) {
                recurringSchedule.removeAt(index);
                if (recurringSchedule.length == 0) {
                    product.get('RequiredQuantity').enable();
                    product.get('ScheduleQuantityType').enable();
                    product.get('isRecurringSchedule').patchValue(false);
                    $(".showHide" + index).hide();
                }
            }
        }
        else {
            this.dipTestLoader = true;
            this.carrierService.deleteRecurringScheduleDetails(Id).subscribe(data => {
                if (data.StatusCode == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                    this.dipTestLoader = false;
                    if (index > -1) {
                        recurringSchedule.removeAt(index);
                        if (recurringSchedule.length == 0) {
                            product.get('RequiredQuantity').enable();
                            product.get('ScheduleQuantityType').enable();
                            product.get('isRecurringSchedule').patchValue(false);
                            $(".showHide" + index).hide();
                        }
                    }
                    this.onRaiseDR.emit();
                    this.emitDataForDRDisplay();
                }
                else {
                    Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                    this.dipTestLoader = false;
                }
                if (!this.changeDetectorRef['destroyed'])
                    this.changeDetectorRef.detectChanges();
            });
        }
    }
    public setSelectedDate($event: any, recurringSchedule: any) {
        if ($event != '') {
            recurringSchedule.get('Date').patchValue($event);
        }
    }
    public onTagChanged($event: any, recurringSchedule: any) {
        recurringSchedule.get('WeekDayId').patchValue($event);
    }
    public getRecurringSchedule(product: any, PoNumber: string, JobId: number, JobSiteId: string, productTypeId: number, index: number, IsBlendedRequest: false) {
        //let AssetId = product.get('AssetId').value = null ? 0 : parseInt(product.get('AssetId').value);
        let recurringScheduleControls = product.get('RecurringSchdule') as FormArray;
        recurringScheduleControls.clear();//removing all controls from the RecurringSchdule from array
        this.dipTestLoader = true;
        this.carrierService.getRecurringScheduleDetails(JobId, PoNumber, JobSiteId, productTypeId).subscribe((data: any) => {
            let dataLength = data.length;
            $(".showHide" + index).show();
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    recurringScheduleControls.push(this.buildRecurringSchedule(data[i]));
                    if (dataLength == i + 1) {
                        this.dipTestLoader = false;
                        if (!this.changeDetectorRef['destroyed'])
                            this.changeDetectorRef.detectChanges();

                    }
                }
            }
            else {
                this.dipTestLoader = false;
                recurringScheduleControls.push(this.getDefaultRecurringScheduleDetails(product.get('TankName').value, IsBlendedRequest));
                if (!this.changeDetectorRef['destroyed'])
                    this.changeDetectorRef.detectChanges();
            }
            var totalQty = 0;
            var blendedRequests = <FormArray>product.get('BlendedRequests');
            blendedRequests.controls.forEach(x => {
                var requiredQty = x.get('RequiredQuantity').value;
                totalQty = totalQty + requiredQty;
            });
            var deliveryReqControl = <FormArray>product.get('RecurringSchdule');
            deliveryReqControl.controls.forEach(x => {
                if (x.get('Id').value == '' || x.get('Id').value == null) {
                    x.get('RequiredQuantity').setValue(totalQty);
                }
            });
        });
    }

    initializeSalesGrid() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'Sales Details', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'Sales Details', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],

            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
        if (this.dtTrigger) {
            this.dtTrigger.unsubscribe();
            this.dtTrigger = new Subject();
        }
    }

    public getSalesData() {
        this.SalesData = [];
        if ((this.datatableElement && this.datatableElement.dtInstance)) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => { dtInstance.destroy(); });
        }
        this.initializeSalesGrid();
        let inputs = {
            RegionId: this.SelectedRegionId,
            Priority: DeliveryReqPriority.None,
            CustomerId: this.getCustomerForJob(),
            LocationId: this.SelectedLocationId,
            SelectedTab: '',
            Carriers: '',
            IsShowCarrierManaged: '',
            IsShowRetailJobs: '',
            InventoryCaptureType: '',
        };
        this.isLoadingSubject.next(true);
        this.dispatcherService.getSalesData(inputs).subscribe((resp: SalesDataModel[]) => {
            this.SalesData = resp;
            this.isLoadingSubject.next(false);
            this.dtTrigger.next();
            // this.datatableRerender();
        });
    }

    public onCaculateRetainWindow() {
        if (this.isTankExists) {
            let data = this.fmGroup.get('DeliveryRequests').value as any[];
            if (data != null) {
                // let tankinfo = data.filter(x => x.RequiredQuantity != '').map(x => {
                //     return {
                //         JobId: x.JobId,
                //         siteId: x.SiteId == null ? x.JobId : x.SiteId,
                //         tankId: x.TankId,
                //         storageId: x.StorageId,
                //         Id: x.AssetId,
                //         Quantity: x.RequiredQuantity,
                //     };
                // });
                let tankinfo = []
                data.forEach((x) => {
                    if (x && x.Tanks && x.Tanks.length > 0) {
                        x.Tanks.forEach((tank: any) => {
                            tankinfo.push({
                                //JobId: x.JobId,
                                Id: tank.AssetId,
                                siteId: x.SiteId,
                                storageId: tank.StorageId,
                                tankId: tank.TankId,
                                TankName: tank.TankName,
                                Quantity: x.RequiredQuantity,
                            })
                        })
                    }
                });

                if (tankinfo.length > 0) {
                    this.dipTestLoader = true;
                    if (IsBuyerCompany == false) {
                        this.dispatcherService.calculateTankDetailsRetainWindowInfo(tankinfo).subscribe((resp: any[]) => {
                            this.dipTestLoader = false;
                            if (resp != null && resp.length > 0) {
                                let prods = this.fmGroup.get('DeliveryRequests') as FormArray;
                                this.updateRetainInfo(resp, prods);
                            }
                            if (!this.changeDetectorRef['destroyed'])
                                this.changeDetectorRef.detectChanges();
                        });
                    }
                    else {
                        this.dispatcherService.calculateBuyerTankDetailsRetainWindowInfo(tankinfo).subscribe((resp: any) => {
                            this.dipTestLoader = false;
                            if (resp != null && resp.length > 0) {
                                let prods = this.fmGroup.get('DeliveryRequests') as FormArray;
                                this.updateRetainInfo(resp, prods);
                            }
                            if (!this.changeDetectorRef['destroyed'])
                                this.changeDetectorRef.detectChanges();
                        });
                    }
                }
            }
        }
    }

    public onSingleCaculateRetainWindow(form: any) {

        let _product = form.value;
        let Qty = _product["RequiredQuantity"];

        if ((Qty != '' || Qty != null) && _product.Tanks && _product.Tanks.length > 0) {

            let tanks = _product.Tanks.map((tank: any) => {
                return {
                    Id: tank.AssetId,
                    siteId: _product.SiteId,
                    storageId: tank.StorageId,
                    tankId: tank.TankId,
                    TankName: tank.TankName,
                    Quantity: _product.RequiredQuantity,
                }
            });

            this.dipTestLoader = true;
            if (IsBuyerCompany == false) {
                this.dispatcherService.calculateTankDetailsRetainWindowInfo(tanks).subscribe((resp: any[]) => {
                    this.dipTestLoader = false;
                    if (resp != null && resp.length > 0) {
                        this.updateRetainSingleInfo(resp, form);
                    }
                    if (!this.changeDetectorRef['destroyed'])
                        this.changeDetectorRef.detectChanges();
                });
            }
            else {
                this.dispatcherService.calculateBuyerTankDetailsRetainWindowInfo(tanks).subscribe((resp: any) => {
                    this.dipTestLoader = false;
                    if (resp != null && resp.length > 0) {
                        this.updateRetainSingleInfo(resp, form);
                    }
                    if (!this.changeDetectorRef['destroyed'])
                        this.changeDetectorRef.detectChanges();
                });
            }
        }
    }

    public updateRetainInfo(resp: any[], products: FormArray) {

        for (let i = 0; i < products.length; i++) {

            let productForm = products.controls[i];
            let productObj = productForm.value;

            let SiteId = productObj.SiteId;
            let assetIds: number[] = productObj.Tanks.map((tank: any) => { return tank.AssetId });
            let TankIds: number[] = productObj.Tanks.map((tank: any) => { return tank.TankId });
            let StorageIds: number[] = productObj.Tanks.map((tank: any) => { return tank.StorageId });

            let tankDetails = resp.filter(top => top.siteId == SiteId && TankIds.includes(top.tankId) && StorageIds.includes(top.storageId) && assetIds.includes(top.Id));

            let controls = <FormArray>productForm.get('RetainInfo');
            controls.clear();

            let currentObj = this;

            if (tankDetails != null) {
                productForm.get('isRetainInfo').setValue(true);

                tankDetails.forEach(function (tank) {
                    controls.push(currentObj.buildRetailInfoForm(tank));
                });
            }
            else {
                productForm.get('isRetainInfo').setValue(false);
            }
            productForm.get('IsRetainButtonClick').setValue(true);
        }
    }

    buildRetailInfoForm(tank: any): FormGroup {
        return this.fb.group({
            RetainTime: this.fb.control(tank.RetainTime),
            RetainDate: this.fb.control(tank.RetainDate),
            WindowStartTime: this.fb.control(tank.WindowStartTime),
            WindowStartDate: this.fb.control(tank.WindowStartDate),
            WindowEndTime: this.fb.control(tank.WindowEndTime),
            WindowEndDate: this.fb.control(tank.WindowEndDate),
            TankName: this.fb.control(tank.TankName)
        });
    }

    public updateRetainSingleInfo(resp: any[], productForm: any) {


        let productObj = productForm.value;
        let assetIds: number[] = productObj.Tanks.map((tank: any) => { return tank.AssetId });
        let TankIds: number[] = productObj.Tanks.map((tank: any) => { return tank.TankId });
        let StorageIds: number[] = productObj.Tanks.map((tank: any) => { return tank.StorageId });
        let SiteId = productObj.SiteId == null ? productObj.JobId : productObj.SiteId;

        let tankDetails = resp.filter(top => top.siteId == SiteId && TankIds.includes(top.tankId) && StorageIds.includes(top.storageId) && assetIds.includes(top.Id));

        let controls = <FormArray>productForm.get('RetainInfo');
        controls.clear();

        if (tankDetails != null) {
            productForm.controls['isRetainInfo'].setValue(true);

            let _this = this;
            tankDetails.forEach(function (tank) {
                controls.push(_this.buildRetailInfoForm(tank));
            });
        }
        else {
            productForm.controls['isRetainInfo'].setValue(false);
        }
        productForm.controls['IsRetainButtonClick'].setValue(true);
    }

    public hideShowCommonBadgeArea() {

        let deliveryRequests: any[] = this.fmGroup.get('DeliveryRequests').value as any[] || [];
        if (deliveryRequests.some(dr => dr.IsCommonBadge)) {
            this.IsCommonBadge = true;
            this.fmGroup.controls['IsCommonBadge'].setValue(true);

        } else {
            this.IsCommonBadge = false;
            this.fmGroup.controls['IsCommonBadge'].setValue(false);
        }
    }
    getDefaultTBDForm() {
        let TBDGroupID = "TBD_" + new Date().getUTCMilliseconds();
        let _form = this.fb.group({
            SelectedFuelType: this.fb.control([]),
            SelectedOtherFuelType: this.fb.control([]),
            Priority: this.fb.control(1),
            PickupLocationType: this.fb.control(0),
            Terminal: this.utilService.getTerminalForm(new DropdownItem(), false),
            BulkPlant: this.utilService.getBulkPlantForm(new DropAddressModel(), false),
            TBDGroupId: this.fb.control(TBDGroupID),
            DeliveryRequests: this.fb.array([]),
        });
        return _form;
    }
    getTBDDeliveryRequestForm(x: any, productType: any) {
        let _form = this.fb.group({
            FuelTypeId: this.fb.control(x.Id),
            FuelTypeName: this.fb.control(x.Name),
            DRScheduleQuantityType: this.fb.control(1),
            ProductTypeId: this.fb.control(productType.ProductTypeId),
            ProductTypeName: this.fb.control(productType.ProductTypeName),
            RequiredQty: this.fb.control(''),
            BadgeNo1: this.fb.control(''),
            BadgeNo2: this.fb.control(''),
            BadgeNo3: this.fb.control(''),
            Notes: this.fb.control(''),
            DeliveryLevelPO: this.fb.control(''),
        });
        return _form;
    }
    getTBDDefaultData() {
        this.Terminals = [];
        this.StateList = [];
        this.CountryList = [];
        this.BulkPlants = [];
        this.FuelTypeDetails = [];
        this.OtherFuelTypeDetails = [];
        this.TBDDeliveryRequestViewModel = new RaiseTBDDeliveryRequest();
        this.isTBDUserSubmit = false;
        this.TBDdipTestLoader = false;
        this.getRequiredDataForPickupDetails();

        this.addresService.getBulkPlants('').subscribe(data => {
            this.BulkPlants = data.slice();
            this.BulkplantList = data;
        });
      
        this.DGSubscription.add(this.sbService.getTBDPickupTerminals('').subscribe((data: DropdownItem[]) => {
            this.Terminals = data;
        }));
        let prods = this.fmTBDGroup.get('ScheduleTBDForm') as FormArray;
        prods.clear();
        this.carrierService.getDefaultTBDScheduleData().subscribe(data => {
            this.FuelTypeDetails = data.MstProductTypes;
            this.companyUoM = data.UoM;
            this.OtherFuelTypeDetails = data.OtherProducts;
            if (data.OtherProducts && data.OtherProducts.length > 0) {
                this.otherProductTypeId = data.OtherProducts[0].ProductTypeId;
            }
            let prods = this.fmTBDGroup.get('ScheduleTBDForm') as FormArray;
            prods.push(this.getDefaultTBDForm());
            if (!this.changeDetectorRef['destroyed'])
                this.changeDetectorRef.detectChanges();
        });
    }
    openRaiseTBDDrPanel() {
        this.getTBDDefaultData();
    }
    public addTBDDR() {
        let prods = this.fmTBDGroup.get('ScheduleTBDForm') as FormArray;
        prods.push(this.getDefaultTBDForm());
        if (!this.changeDetectorRef['destroyed'])
            this.changeDetectorRef.detectChanges();
    }
    onFuelTypeSelect(item: any, product: FormGroup, isOtherProduct: boolean) {
        let productType = this.getProductType(isOtherProduct, item);
        let prodDeliveryRequests = product.get('DeliveryRequests').value;
        let prodDeliveryRequestsFormArray = product.get('DeliveryRequests') as FormArray;
        var prodExists = prodDeliveryRequests.find(x => x.FuelTypeId == item.Id && x.ProductTypeId == productType.ProductTypeId);
        if (prodExists == null) {
            prodDeliveryRequestsFormArray.push(this.getTBDDeliveryRequestForm(item, productType));
        }
        if (!this.changeDetectorRef['destroyed'])
            this.changeDetectorRef.detectChanges();
    }
    onFuelTypeDeSelect(item: any, product: FormGroup, isOtherProduct: boolean) {
        let productType = this.getProductType(isOtherProduct, item);
        let prodDeliveryRequests = product.get('DeliveryRequests') as FormArray;
        prodDeliveryRequests.controls.forEach((x1: FormGroup, index) => {
            if (item.Id == x1.get('FuelTypeId').value && productType.ProductTypeId == x1.get('ProductTypeId').value) {
                prodDeliveryRequests.removeAt(index);
            }
        });
        if (!this.changeDetectorRef['destroyed'])
            this.changeDetectorRef.detectChanges();
    }

    onFuelTypeSelectAll(items: any, product: FormGroup, isOtherProduct: boolean): void {
        let prodDeliveryRequests = product.get('DeliveryRequests') as FormArray;
        let drs = prodDeliveryRequests.value;
        items.forEach(x => {
            let productType = this.getProductType(isOtherProduct, x);
            if (drs.find(t => t.FuelTypeId == x.Id && t.ProductTypeId == productType.ProductTypeId) == null) {
                prodDeliveryRequests.push(this.getTBDDeliveryRequestForm(x, productType));
            }
        });
        if (!this.changeDetectorRef['destroyed'])
            this.changeDetectorRef.detectChanges();
    }
    onFuelTypeDeSelectAll(product: FormGroup, isOtherProduct: boolean): void {
        let prodDeliveryRequests = product.get('DeliveryRequests') as FormArray;
        let selectedDrs = this.fb.array([]);
        prodDeliveryRequests.controls.forEach((x1: FormGroup, index) => {
            let removeFuel = false;
            if (isOtherProduct == true && x1.get('ProductTypeId').value == this.otherProductTypeId) {
                removeFuel = true;
            }
            if (isOtherProduct == false && x1.get('ProductTypeId').value != this.otherProductTypeId) {
                removeFuel = true;
            }
            if (removeFuel == false) {
                selectedDrs.push(x1);
            }
        });
        prodDeliveryRequests.clear();
        selectedDrs.controls.forEach(t => {
            prodDeliveryRequests.push(t);
        });
        if (!this.changeDetectorRef['destroyed'])
            this.changeDetectorRef.detectChanges();
    }

    private getProductType(isOtherProduct: boolean, item: any) {
        let productType;
        if (isOtherProduct == false)
            productType = this.FuelTypeDetails.find(t => t.Id == item.Id);

        else
            productType = this.OtherFuelTypeDetails.find(t => t.Id == item.Id);
        return productType;
    }

    editTBDPickupLocation_(tdr: FormGroup, drIndex: number): void {
        //this.PickupForm.get('Terminal').get('SelectedTerminal').setValue('');
        this.SelectedTBDForEditPickupIndex = drIndex;
        this.ChangePickupForOrders = [];
        this.SelectedTBDForEditPickup = tdr;
        let tempPickupLocationType = this.SelectedTBDForEditPickup.controls['PickupLocationType'].value;
        if (tdr == null || tdr == undefined || tempPickupLocationType == null) {
            this.SelectedTBDForEditPickup = this.fmTBDGroup;
        }
        let _pickupType = this.SelectedTBDForEditPickup.controls['PickupLocationType'].value;
        _pickupType = _pickupType == 0 ? 1 : _pickupType;
        let _terminal = this.SelectedTBDForEditPickup.controls['Terminal'].value;
        let _bulkPlant = this.SelectedTBDForEditPickup.controls['BulkPlant'].value;
        let location = {
            PickupLocationType: _pickupType,
            Terminal: _terminal,
            BulkPlant: _bulkPlant
        };
        this.setPickupLocation_(this.SelectedTBDForEditPickup, location);
        if (this.BulkPlants) {
            let bulkPlantName = this.PickupForm.get('BulkPlant.SiteName').value;
            this.isReadOnly = this.BulkPlants.filter(t => t.Name == bulkPlantName).length > 0;
        }
    }
    updateTBDPickupLocation_() {
        if (this.SelectedTBDForEditPickup != null) {
            let pickupLoc = this.PickupForm.value;
            this.SelectedTBDForEditPickup.patchValue(pickupLoc);
            this.setPickupLocation_(this.SelectedTBDForEditPickup, pickupLoc);
            let dismiss = document.getElementById('btnTBDDrPickupClose') as HTMLElement;
            dismiss.click();
            if (!this.changeDetectorRef['destroyed'])
                this.changeDetectorRef.detectChanges();
        }
    }
    removeTBDDR(product: FormGroup, index: number) {
        let prods = this.fmTBDGroup.get('ScheduleTBDForm') as FormArray;
        prods.removeAt(index);
        if (!this.changeDetectorRef['destroyed'])
            this.changeDetectorRef.detectChanges();
    }
    removeSubTBDDR(product: FormGroup, index: number, FuelTypeId: number, productTypeId: number) {
        let prods = product.get('DeliveryRequests') as FormArray;
        prods.removeAt(index);
        let selectedProductIds = [];
        if (productTypeId == this.otherProductTypeId) {
            let selectedOtherFuelType = product.get('SelectedOtherFuelType').value as [];
            selectedOtherFuelType.forEach((xf1: any) => {
                if (xf1.Id != FuelTypeId) {
                    selectedProductIds.push(xf1);
                }
            });
            product.get('SelectedOtherFuelType').setValue(selectedProductIds);
        }
        else {
            let selectedFuelType = product.get('SelectedFuelType').value as [];
            selectedFuelType.forEach((xf1: any) => {
                if (xf1.Id != FuelTypeId) {
                    selectedProductIds.push(xf1);
                }
            });
            product.get('SelectedFuelType').setValue(selectedProductIds);
        }
        if (!this.changeDetectorRef['destroyed'])
            this.changeDetectorRef.detectChanges();
    }
    clearTBDForm() {
        this.fmTBDGroup.reset();
        this.clearRaiseRequests();
    }
    clearTBDRaiseRequests() {
        let controls = <FormArray>this.fmTBDGroup.controls['ScheduleTBDForm'];
        controls.clear();
    }
    SubmitTBDData() {
        this.TBDDeliveryRequestViewModel.DeliveryRequests = [];
        var isValid = true;
        let tbdForm = this.fmTBDGroup.get('ScheduleTBDForm') as FormArray;
        tbdForm.controls.forEach((product: FormGroup) => {
            let prodDeliveryReq = product.get('DeliveryRequests') as FormArray;
            let TBDGroupID = product.get('TBDGroupId').value;
            if (prodDeliveryReq.controls.length > 0) {
                let pickupLocationType = product.get('PickupLocationType').value;
                if (pickupLocationType != null && pickupLocationType != 0) {
                    prodDeliveryReq.controls.forEach((prodDR: FormGroup) => {
                        let deliveryReq = new TBDRaiseDRDeliveryRequests();
                        deliveryReq.FuelTypeId = prodDR.get('FuelTypeId').value;
                        deliveryReq.FuelType = prodDR.get('FuelTypeName').value;
                        deliveryReq.ProductType = prodDR.get('ProductTypeName').value;
                        deliveryReq.ProductTypeId = prodDR.get('ProductTypeId').value;
                        deliveryReq.Priority = product.get('Priority').value;
                        deliveryReq.DeliveryLevelPO = prodDR.get('DeliveryLevelPO').value;
                        let fuelTypeExists = this.TBDDeliveryRequestViewModel.DeliveryRequests.findIndex(x => x.FuelTypeId == deliveryReq.FuelTypeId && x.TBDGroupId == TBDGroupID);
                        if (fuelTypeExists == -1) {
                            deliveryReq.ScheduleQuantityType = prodDR.get('DRScheduleQuantityType').value;
                            if (prodDR.get('DRScheduleQuantityType').value == 1) {
                                if (prodDR.get('RequiredQty').value == '' || prodDR.get('RequiredQty').value == 0) {
                                    isValid = false;
                                    Declarations.msgerror('Quantity required for fuel type.' + prodDR.get('FuelTypeName').value, undefined, undefined);
                                    return;
                                }
                                else {
                                    deliveryReq.RequiredQuantity = prodDR.get('RequiredQty').value;
                                }
                            }
                            deliveryReq.TBDGroupId = TBDGroupID;
                            deliveryReq.CreatedByRegionId = this.SelectedRegionId;
                            deliveryReq.AssignedToRegionId = this.SelectedRegionId;
                            deliveryReq.UoM = this.companyUoM;
                            deliveryReq.PickupLocationType = pickupLocationType;
                            if (deliveryReq.PickupLocationType == 1) {
                                deliveryReq.Terminal = product.get('Terminal').value;
                                if (deliveryReq.Terminal.Id <= 0 || deliveryReq.Terminal.Name == null || deliveryReq.Terminal.Name == '') {
                                    isValid = false;
                                    Declarations.msgerror('Pickup location information is required.', undefined, undefined);
                                    return;
                                }
                            }
                            else {
                                deliveryReq.Bulkplant = product.get('BulkPlant').value;
                                if (deliveryReq.Bulkplant == null || deliveryReq.Bulkplant.SiteName == null || deliveryReq.Bulkplant.SiteName == '' ||
                                    deliveryReq.Bulkplant.Address == null || deliveryReq.Bulkplant.ZipCode == null) {
                                    isValid = false;
                                    Declarations.msgerror('Pickup location information is required.', undefined, undefined);
                                    return;
                                }
                            }
                            deliveryReq.BadgeNo1 = prodDR.get('BadgeNo1').value;
                            deliveryReq.BadgeNo2 = prodDR.get('BadgeNo2').value;
                            deliveryReq.BadgeNo3 = prodDR.get('BadgeNo3').value;
                            deliveryReq.Notes = prodDR.get('Notes').value;
                            this.TBDDeliveryRequestViewModel.DeliveryRequests.push(deliveryReq);
                        }
                        else {
                            isValid = false;
                            Declarations.msgerror('Fuel type already exists : ' + prodDR.get('FuelTypeName').value, undefined, undefined);
                            return;
                        }
                    });
                }
                else {
                    isValid = false;
                    Declarations.msgerror('Pickup location information is required.', undefined, undefined);
                    return;
                }
            }

        });
        if (isValid) {
            if (this.TBDDeliveryRequestViewModel.DeliveryRequests.length > 0) {
                this.isTBDUserSubmit = true;
                this.TBDdipTestLoader = true;
                let inputdata = { DeliveryRequests: this.TBDDeliveryRequestViewModel.DeliveryRequests };
                this.carrierService.postRaiseRequests(inputdata as any).subscribe((data: any) => {
                    this.isTBDUserSubmit = false;
                    if (data != null && parseInt(data.StatusCode) == 0) {
                        this.displayMessage(data);
                        closeSlidePanel();
                        this.clearTBDForm();
                        this.onRaiseDR.emit();
                        this.emitDataForDRDisplay();
                        this.TBDdipTestLoader = false;
                    }
                    else {
                        this.TBDdipTestLoader = false;
                        const statusMessage = data == null ? 'Failed' : data.StatusMessage;
                        Declarations.msgerror(statusMessage, undefined, undefined);
                    }
                    if (!this.changeDetectorRef['destroyed']) {
                        this.changeDetectorRef.detectChanges();
                    }
                })
            }
            else {
                Declarations.msgerror('TBD DRs required for creating delivery requests.', undefined, undefined);
            }
        }
    }
    public deletePickupInfo(terminalInfo: any) {
        if (terminalInfo.get('Id').value) {
            terminalInfo.get('Id').patchValue(0);
            terminalInfo.get('Name').patchValue('');
        }
    }
    OnCountryChanged(eventData: any) {
        let selectedCountryCode = eventData.target.value;
        let selectedCountryId = this.getCountryIdFromCode(selectedCountryCode);
        this.setBulkPlantAddressValidators(selectedCountryId);
    }

    getCountryIdFromCode(countryCode: any): number {
        let selectedCountryId = 1;
        if (countryCode == "CAN" || countryCode == "CA") {
            selectedCountryId = 2;
        }
        else if (countryCode == "CAR") {
            selectedCountryId = 4;
        }
        return selectedCountryId;
    }

    setBulkPlantAddressValidators(countryId: number) {
        let isCarribean = countryId == 4 ? true : false;
        let form = this.PickupForm;
        form.get('BulkPlant.Address').setValidators(!isCarribean ? [Validators.required] : null);
        form.get('BulkPlant.Address').updateValueAndValidity();

        form.get('BulkPlant.City').setValidators(!isCarribean ? [Validators.required] : null);
        form.get('BulkPlant.City').updateValueAndValidity();

        form.get('BulkPlant.ZipCode').setValidators(!isCarribean ? [Validators.required] : null);
        form.get('BulkPlant.ZipCode').updateValueAndValidity();

        form.get('BulkPlant.CountyName').setValidators(!isCarribean ? [Validators.required] : null);
        form.get('BulkPlant.CountyName').updateValueAndValidity();

        form.get('BulkPlant.Latitude').setValidators(!isCarribean ? [Validators.required, Validators.pattern('^[0-9.-]*$')] : null);
        form.get('BulkPlant.Latitude').updateValueAndValidity();

        form.get('BulkPlant.Longitude').setValidators(!isCarribean ? [Validators.required, Validators.pattern('^[0-9.-]*$')] : null);
        form.get('BulkPlant.Longitude').updateValueAndValidity();
    }
    public deleteTBDPickupInfo(terminalInfo: any) {
        if (terminalInfo.get('Id').value) {
            terminalInfo.get('Id').patchValue(0);
            terminalInfo.get('Name').patchValue('');
        }
    }
    onTBDTerminalSearched_(event: any): void {

        let keyword = event.target.value.toLowerCase();

        this.TBDTerminals = this.Terminals;
        if (keyword != '') {
            this.TBDTerminals = this.Terminals.filter(tr => tr.Name.toLowerCase().indexOf(keyword) != -1);
        }
    }

    ///////////////////////////BLENDED REQUEST START///////////////////////////

    getUnUsedOrdersForBlendedRequest(currentOrder: number) {
        let currentDrForm = this.blendRequestForm;
        let drValue = this.fmGroup.get('DeliveryRequests')['controls'][this.DrForEditBlendedRequestIndex].value;
        let isTank = drValue.StorageId != null && drValue.StorageId != '';
        let orderList: any[] = [];
        if (isTank == false) {
            let compatibleProductTypesModel = this.CompatibleProductTypeList.find(t => t.ProductTypeId == drValue.ProductTypeId);
            let compatibleProductTypes: number[] = [drValue.ProductTypeId];
            if (compatibleProductTypesModel && compatibleProductTypesModel.MappedToProductTypeIds) {
                compatibleProductTypes = compatibleProductTypes.concat(compatibleProductTypesModel.MappedToProductTypeIds);
            }
            orderList = this.OrderDetails.filter(t => compatibleProductTypes.includes(t.ProductTypeId));
        }
        else {
            orderList = this.fmGroup.get('DeliveryRequests')['controls'][this.DrForEditBlendedRequestIndex].controls['BlendOrderPickupDetails'].value as any[];
        }
        let ordersToRemove = currentDrForm.get('BlendedRequests').value as BlendedRequest[];//orders selected in blend form
        ordersToRemove = ordersToRemove.filter(function (obj) { return obj.OrderId != currentOrder; });
        let productToRemove = orderList.filter(s => ordersToRemove.find(s2 => s2.OrderId == s.OrderId));
        return orderList.filter((x => currentOrder == x.OrderId || !productToRemove.some(y => y.FuelTypeId == x.FuelTypeId)));
    };

    getUnUsedBlendOrders(currentOrder: number) {

        let blendRequests = this.blendRequestForm.get('BlendedRequests').value as BlendedRequest[];
        blendRequests = blendRequests.filter(b => b.IsAdditive && b.OrderId && b.OrderId != currentOrder);
        let ordersToRemove = blendRequests.map(b => b.OrderId);

        return this.additiveOrders.filter((x => currentOrder == x.Id || !ordersToRemove.some(orderId => orderId == x.Id)));
    };

    openBlendRequestForm(dr: FormGroup, drIndex: number) {
        this.DrForEditBlendedRequestIndex = drIndex;
        this.copyBlendForm(<FormArray>dr.get('BlendedRequests'), <FormArray>this.blendRequestForm.get('BlendedRequests'));
        document.getElementById('openBlendModalButton').click();
    }

    orderChangedForBlendRequest(_orderId: any, blendedRequest: FormGroup): void {
        let parentDr = this.fmGroup.get('DeliveryRequests')['controls'][this.DrForEditBlendedRequestIndex];
        let orders = parentDr.get('BlendOrderPickupDetails').value as any[] || [];
        blendedRequest.get('ProductTypeId').setValue(null);
        var order = this.OrderDetails.find(t => t.OrderId == _orderId);
        if (order) {
            blendedRequest.get('UoM').setValue(order.UoM);
        }
        else {
            order = orders.find(t => t.OrderId == _orderId);
            if (order) {
                blendedRequest.get('UoM').setValue(order.UoM);
            }
        }
        let selectedOrder = null;
        if (!orders || orders.length == 0) {
            if (order) {
                orders = order.OrderPickupDetails as any[] || [];
                if (orders && orders.length > 0) {
                    selectedOrder = orders[0];
                }
            }

        }
        else if (orders.length > 0) {
            selectedOrder = orders.find(x => x.OrderId == _orderId);
            if (!selectedOrder) {
                if (order && order.BlendOrderPickupDetails) {
                    selectedOrder = order.BlendOrderPickupDetails.find(x => x.OrderId == _orderId);
                }
            }
        }
        if (selectedOrder) {
            let location = OrderPickupLocationModel.ToLocation(selectedOrder);
            this.setPickupLocationForBlendRequest(blendedRequest, location);
            if (selectedOrder.ProductTypeId) {
                blendedRequest.get('ProductTypeId').setValue(selectedOrder.ProductTypeId);
            }
            else {
                blendedRequest.get('ProductTypeId').setValue(parentDr.get('ProductTypeId').value);
            }
        }
    }

    getCreateDrSetting() {
        if (!this.preferenceSetting) {
            this.carrierService.getCreateDrSetting().subscribe(response => {
                this.preferenceSetting = response;
            });
        }
    }

    getCompatibleProducts() {
        if (this.CompatibleProductTypeList.length == 0) {
            this.carrierService.getCompatibleProductTypes().subscribe(response => {
                if (response && response.length > 0) {
                    this.CompatibleProductTypeList = response;
                }
            });
        }
    }

    getAdditiveOrders() {
        if (this.additiveOrders.length == 0) {
            this.carrierService.getAdditiveOrders(this.SelectedRegionId).subscribe(response => {
                if (response && response.length > 0) {
                    this.additiveOrders = response;
                }
            });
        }
    }

    setPickupLocationForBlendRequest(delReq: FormGroup, order: OrderPickupLocationModel): void {
        delReq.controls['PickupLocationType'].patchValue(order.PickupLocationType);
        if (order.PickupLocationType != 2) {
            if (order.Terminal) {
                delReq.controls['Terminal'].patchValue(order.Terminal);
            }
            // this.PickupForm.controls['PickupLocationType'].patchValue(1);
            // this.PickupForm.controls['Terminal'].patchValue(order.Terminal);
        } else {
            if (order.BulkPlant) {
                if (order.BulkPlant.ZipCode) {
                    order.BulkPlant.ZipCode = order.BulkPlant.ZipCode.toUpperCase();
                }
                delReq.controls['BulkPlant'].patchValue(order.BulkPlant);
            }
            // this.PickupForm.controls['PickupLocationType'].patchValue(2);
            // this.PickupForm.controls['BulkPlant'].patchValue(order.BulkPlant);
        }
    }

    onBlendChange(IsBlendedRequest: boolean, dr: FormGroup) {

        let blendedRequests = dr.get('BlendedRequests') as FormArray;
        blendedRequests.clear();

        if (IsBlendedRequest) {
            this.getCompatibleProducts();
            this.getAdditiveOrders();

            blendedRequests.push(this.utilService.getBlendRequestFormGroup(new BlendedRequest(false)));
            blendedRequests.push(this.utilService.getBlendRequestFormGroup(new BlendedRequest(true)));

            dr.get('RequiredQuantity').setValidators([Validators.required, Validators.min(NumberConstants.MinQuantity)]);
            dr.get('RequiredQuantity').updateValueAndValidity();

            dr.get('BlendedGroupId').setValue(getUniqueId());
            if (dr.get('isRecurringSchedule').value) {
                dr.get('isRecurringSchedule').setValue(false);
                dr.get('RequiredQuantity').enable();
                dr.get('ScheduleQuantityType').enable();
            }
        }
        else {
            dr.get('RequiredQuantity').setValidators([]);
            dr.get('RequiredQuantity').updateValueAndValidity();

            dr.get('BlendedGroupId').setValue(null);
        }
        /*dr.get('SelectedDate').setValue(moment(new Date()).format('MM/DD/YYYY'));*/
        dr.get('ScheduleStartTime').setValue(null);
        dr.get('ScheduleEndTime').setValue(null);

    }

    getRequiredDataForPickupDetails() {

        if (!this.CountryList || this.CountryList.length == 0) {
            this.stateService.getCountries().subscribe(x => this.CountryList = x);
        }
        if (!this.CountryGroupList || this.CountryGroupList.length == 0) {
            this.stateService.getCountryGroup(4).subscribe(x => this.CountryGroupList = x);
        }
        if (!this.StateList || this.StateList.length == 0) {
            this.stateService.getStates().subscribe(x => { this.StateList = x; this.changeDetectorRef.detectChanges(); })
        }
    }

    editPickupLocationForBlend(blendDrIndex: number): void {

        this.BlendDrIndex = blendDrIndex;
        this.SelectedDRForEditPickupIndex = this.DrForEditBlendedRequestIndex;
        let tdr = this.fmGroup.get('DeliveryRequests')['controls'][this.DrForEditBlendedRequestIndex];
        let blendDr = this.blendRequestForm.get('BlendedRequests')['controls'][this.BlendDrIndex];

        this.getRequiredDataForPickupDetails();

        this.ChangePickupForOrders = [];
        this.SelectedDRForEditPickup = blendDr;


        let _orderId = blendDr.controls['OrderId'].value as number;
       
        if (_orderId > 0) {
            this.ChangePickupForOrders.push(_orderId);
            let existingOrders = tdr.get('OrderPickupDetails').value || [];
            let orders = existingOrders.filter(t => t.OrderId == _orderId);
            if (orders.length > 0) {
                let lastIndex = orders[0].PoNumber.lastIndexOf(" - ");
                this.SearchTerminalFuelType = orders[0].PoNumber.slice(0, lastIndex + 1);
            }
        }


        let _pickupType = blendDr.controls['PickupLocationType'].value;
        _pickupType = _pickupType == 0 ? 1 : _pickupType;
        let _terminal = blendDr.controls['Terminal'].value;
        let _bulkPlant = blendDr.controls['BulkPlant'].value;
        let location = {
            PickupLocationType: _pickupType,
            Terminal: _terminal,
            BulkPlant: _bulkPlant
        };

        this.setPickupLocation_(this.SelectedDRForEditPickup, location);

        //to get bulk plant for auto order
        let orderForBulkPlant = 0;
        if (tdr) {
            orderForBulkPlant = this.SelectedDRForEditPickup.controls['OrderId'].value as number;
        }

        this.addresService.getBulkPlants('', orderForBulkPlant).subscribe(data => {
            this.BulkPlants = data.slice();
            this.BulkplantList = data;

            let bulkPlantName = this.PickupForm.get('BulkPlant.SiteName').value;
            this.isReadOnly = this.BulkPlants.some(t => t.Name == bulkPlantName);
        });
        
        if (this.ChangePickupForOrders.length > 0) {
            this.DGSubscription.add(this.sbService.getPickupTerminals(this.ChangePickupForOrders, '').subscribe((data: DropdownItem[]) => {
                this.Terminals = data;
            }));
        }
        else {
            this.Terminals = [];
        }

        if (this.BulkPlants) {
            let bulkPlantName = this.PickupForm.get('BulkPlant.SiteName').value;
            this.isReadOnly = this.BulkPlants.filter(t => t.Name == bulkPlantName).length > 0;
        }

        if (location.PickupLocationType == 2) {
            if (this.SelectedDRForEditPickup.controls['BulkPlant'].value) {
                let bulkPlant = this.SelectedDRForEditPickup.controls['BulkPlant'].value;
                if (bulkPlant.Country) {
                    let countryCode = bulkPlant.Country.Code
                    if (countryCode) {
                        this.setBulkPlantAddressValidators(this.getCountryIdFromCode(countryCode));
                    }
                }
            }
        }
    }

    updatePickupLocationForBlend() {

        let blendDr = this.blendRequestForm.get('BlendedRequests')['controls'][this.BlendDrIndex];
        this.DrForEditBlendedRequest = null;

        if (this.SelectedDRForEditPickup != null && blendDr != null) {
            this.setPickupLocation_(blendDr, this.PickupForm.value);
        }

        this.IsPickupForBlendRequest = false;
        let dismiss = document.getElementById('btnDrPickupClose') as HTMLElement;
        dismiss.click();
    }

    addRemoveBlendedProduct(addNewRow: boolean, index: number, isAdditive: boolean = false) {

        let _blendedRequests = <FormArray>this.blendRequestForm.get('BlendedRequests');

        if (addNewRow) {

            let _model = new BlendedRequest(isAdditive);
            _model.IsBlendedRequest = true;
            let additiveCount = this.getAdditiveCountInBlend();

            if (!isAdditive && additiveCount > 0) {
                _blendedRequests.insert(+_blendedRequests.controls.length - additiveCount, this.utilService.getBlendRequestFormGroup(_model));
            } else {
                _blendedRequests.push(this.utilService.getBlendRequestFormGroup(_model));
            }
        }
        else {
            _blendedRequests.removeAt(index);
        }
    }

    getAdditiveCountInBlend() {
        let _blendedRequests = <any[]>this.blendRequestForm.get('BlendedRequests').value;
        return _blendedRequests.filter(b => b.IsAdditive).length;
    }

    blendDrQuantityChanged(enteredQuantity: number, blendIndex: number) {
        let parentQuantity = <number>this.fmGroup.get('DeliveryRequests')['controls'][this.DrForEditBlendedRequestIndex].get('RequiredQuantity').value;
        let blendedRequest = <FormGroup>this.blendRequestForm.get('BlendedRequests')['controls'][blendIndex];
        blendedRequest.get('QuantityInPercent').setValue((+enteredQuantity / parentQuantity) * 100);
    }

    blendDrPercentChanged(percent: number, blendIndex: number) {
        let parentQuantity = <number>this.fmGroup.get('DeliveryRequests')['controls'][this.DrForEditBlendedRequestIndex].get('RequiredQuantity').value;
        let blendedRequest = <FormGroup>this.blendRequestForm.get('BlendedRequests')['controls'][blendIndex];
        blendedRequest.get('RequiredQuantity').setValue((+percent / 100) * parentQuantity);
    }

    clearQuantityAndPercentage(blendIndex?: number) {
        if (blendIndex == null) {
            blendIndex = -1;
        }
        let _blendedRequests = <FormArray>this.blendRequestForm.get('BlendedRequests');
        _blendedRequests.controls.forEach((b: FormGroup, index: number) => {
            if (index > blendIndex && !b.get('IsAdditive').value) {
                b.get('RequiredQuantity').setValue(null);
                b.get('QuantityInPercent').setValue(null);
            }
        })
    }

    isBlendedRequestQuantityValid() {

        if (this.blendRequestForm.valid) {

            let totalQuantity = 0;
            let _blendedRequests: BlendedRequest[] = this.blendRequestForm.get('BlendedRequests').value;
            _blendedRequests.forEach((b: BlendedRequest, index: number) => {
                if (!b.IsAdditive) {
                    totalQuantity = +totalQuantity + +b.RequiredQuantity;
                }
            });

            let RequiredQuantity = <number>this.fmGroup.get('DeliveryRequests')['controls'][this.DrForEditBlendedRequestIndex].get('RequiredQuantity').value;
            return (totalQuantity == RequiredQuantity) || (Math.abs(RequiredQuantity - totalQuantity) < 1);
        }
        return false;
    }

    onBlendTotalQuantityChange(totalQuantity: number, blendedRequests: FormArray) {

        if (totalQuantity) {

            let blendedRequests = <FormArray>this.blendRequestForm.get('BlendedRequests');

            blendedRequests.controls.forEach((b: FormGroup) => {
                if (!b.get('IsAdditive').value && +b.get('QuantityInPercent').value > 0) {
                    b.get('RequiredQuantity').setValue((+b.get('QuantityInPercent').value / 100) * totalQuantity);
                }
            })
        }
        else {
            this.clearQuantityAndPercentage();
        }
    }

    copyBlendForm(formA: FormArray, formB: FormArray) {
        formB.clear();
        formA.controls.forEach(a => {
            formB.push(this.utilService.getBlendRequestFormGroup(a.value));
        })
    }

    OnSelectedDateChange(dr: FormGroup, scheduleDate: string) {
        if (scheduleDate != dr.controls['SelectedDate'].value) {
            dr.controls['SelectedDate'].setValue(scheduleDate);
            this.SetFutureDR(dr, scheduleDate);
        }
    }
    getTomorrowsDate(): Date {
        var current = new Date();
        current.setDate(current.getDate() + 1);
        return current;
    }
    SetFutureDR(dr: FormGroup, scheduleDate: string) {
        let tomorrowDate = this.getTomorrowsDate();
        var selectedDate = new Date(scheduleDate);
        let isCalendarDr = selectedDate > tomorrowDate;
        dr.controls['IsFutureDR'].setValue(isCalendarDr);
        dr.controls['IsCalendarView'].setValue(isCalendarDr);
    }
    submitBlendedForm() {
        if (this.blendRequestForm.valid) {
            this.copyBlendForm(<FormArray>this.blendRequestForm.get('BlendedRequests'), <FormArray>this.fmGroup.get('DeliveryRequests')['controls'][this.DrForEditBlendedRequestIndex].get('BlendedRequests'));
        }
        var totalQty = 0;
        var blendedRequests = <FormArray>this.blendRequestForm.get('BlendedRequests');
        if (blendedRequests.controls.length > 0) {
            blendedRequests.controls.forEach(x => {
                var requiredQty = x.get('RequiredQuantity').value;
                totalQty = totalQty + requiredQty;
            });
            var deliveryReqControl = <FormArray>this.fmGroup.get('DeliveryRequests')['controls'][this.DrForEditBlendedRequestIndex].get('RecurringSchdule');
            if (deliveryReqControl.controls.length > 0 && totalQty > 0) {
                deliveryReqControl.controls.forEach(x => {
                    if (x.get('Id').value == '' || x.get('Id').value == null) {
                        x.get('RequiredQuantity').setValue(totalQty);
                    }
                });
            }
        }
    }
    ///////////////////////////BLENDED REQUEST END///////////////////////////
}

