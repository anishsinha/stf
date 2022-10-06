import { Component, OnInit, OnDestroy, AfterViewInit, Input, Output, EventEmitter, OnChanges, SimpleChanges, ChangeDetectorRef, ViewChild, ElementRef, ViewEncapsulation } from '@angular/core';
import { tap, debounceTime, distinctUntilChanged, switchMap, catchError, first } from 'rxjs/operators';
import { of, iif, Subscription, Subject } from 'rxjs';
import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms';
import * as moment from 'moment';
import { Declarations } from 'src/app/declarations.module';
import { TripModel, DeliveryRequestViewModel, DropAddressModel, OrderPickupDetailModel, OrderPickupLocationModel, OrderFuelDetails, OptionalPickupDetailModel, JobDetailsWithOrders, CancelDeliverySchedule, CancelDSDeliveryScheduleInfo, CancelDSDeliverySchedule, CancelDSDeliveryScheduleViewModel, SubDRStatus, SpiltDRsModel, CompatibleProductModel, BlendedRequest, ResetDeliveryGroupScheduleModel } from '../models/DispatchSchedulerModels';
import { DropdownItem, StatelistService, StateDropdownExtendedItem, AdditiveOrderViewModel } from 'src/app/statelist.service';
import { ScheduleBuilderService } from '../service/schedule-builder.service';
import { AddressService } from 'src/app/address.service';
import { AddressModel } from 'src/app/invoice/models/DropDetail';
import { DataService } from 'src/app/services/data.service';
import { CustomAbstractControls } from '../customAbstractControl';
import { UtilService } from 'src/app/services/util.service';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { DeliveryGroupStatus, DeliveryReqStatus, TripStatus } from 'src/app/app.enum';
import { RegExConstants, SBConstants } from 'src/app/app.constants';
import { groupBy } from 'src/app/my.functions';

@Component({
    selector: 'app-delivery-group',
    templateUrl: './delivery-group.component.html',
    styleUrls: ['./delivery-group.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class DeliveryGroupComponent implements OnInit, OnChanges, AfterViewInit, OnDestroy {
    @ViewChild('idCommonPickUpBtn') idCommonPickUpBtn: ElementRef;
    //@ViewChild('terminal_AutoComplete') terminal_AutoComplete;

    public MinDate: Date = new Date();
    public MaxDate: Date = new Date();
    keyword = 'Name';
    public initialValue = '';
    public _loadingAddress: boolean = false;
    public _loadingTerminals: boolean = false;
    public _currentTrip: FormGroup;
    private _shiftIndex: number;
    private _rowIndex: number;
    private _tripIndex: number;
    private Schedule: any;

    public selectedViewNote: any;

    public StateList: StateDropdownExtendedItem[] = [];
    public CountryList: DropdownItem[] = [];
    public CountryGroupList: DropdownItem[] = [];
    public Terminals = [];
    public minCharRequired = false;
    public searchError = false;
    public noTerminalFound = false;
    public BulkPlants: DropdownItem[];
    public BulkplantList: DropdownItem[];
    public OrderList: any = {};
    public TbdOrderList: any = {};
    public fuelTypeOrderList: any = {};
    public TbdCustomerList: any = {};
    public TbdLocationList: any = {};
    public ChangePickupForOrders: number[] = [];
    public SearchTerminalFuelType: string;
    public SearchTerminalKey: string;
    public LocationType: number = 1;

    public DelGroupForm: FormGroup;
    public PickupForm: FormGroup;
    public SelectedDRForEditPickup: FormGroup;
    public addSubDrModel: DeliveryRequestViewModel = new DeliveryRequestViewModel(false);
    public requestToUpdate: DeliveryRequestViewModel = new DeliveryRequestViewModel(false);
    public blendAddRequestToUpdate: DeliveryRequestViewModel[] = [];

    public CompletedScheduleStatuses: number[] = [7, 8, 9, 10];
    //public OnTheWayScheduleStatuses: number[] = [1, 3, 9, 11, 12, 13, 15, 16, 17, 18, 19, 20];
    private selectedDeliveryRequest: DeliveryRequestViewModel;
    private DeletedDrIndex: number;

    public MaxStartDate: Date = new Date();
    public validStartDate: Boolean = true;
    private StartTimeSubscription: Subscription;
    private StartDateSubscription: Subscription;
    private PickupLocationSubscription: Subscription;
    private DeleteDRRequestSubject: Subscription;
    private DGSubscription: Subscription = new Subscription();
    public HideDeliveryGroupSubject: Subscription;
    public UnchangedTrip: FormGroup;
    public MaxCalendarDate: Date = new Date();
    public MinCalendarDate: Date = new Date();
    public isReadOnly: boolean = false;
    @Input() private SelectedDate: Date = new Date();
    @Input() private SelectedRegionId: string;
    @Input() isSupplierCompany: boolean = false;
    @Input() private ScheduleBuilderId: string;
    public CountryBasedZipcodeLabel: string = "Zip";
    public RouteResetGroupSubject: Subscription;
    public RouteListForTrip: any[] = [];
    public isOptionalPickup: boolean = false;
    public ScheduleOptionalPickupDetailModel: OptionalPickupDetailModel[] = [];
    public ScheduleOrderFuelInfo: OrderFuelDetails[] = [];
    public multiDropdownSettings = {};
    public CustomerSettings = {};
    public SiteddlSettings = {};

    @Output() onRaiseSubDR: EventEmitter<any> = new EventEmitter<any>();
    public AddDropLocationLoader: boolean = false;
    public currentJobDetails: JobDetailsWithOrders;
    public drOrders: JobDetailsWithOrders[];
    public SubDrsStatusWithParentIds: SubDRStatus[] = [];
    public EnrouteInCompleted: number[] = [3, 15, 23];
    public EnrouteInProgress: number[] = [1, 3, 11, 12, 13, 14, 15, 16, 17, 18];
    public SubDrOtherThanCancellStatus: number[] = [3, 23, 17, 11, 20, 15, 14, 1, 16, 6, 12]
    public CancelDSViewModel: DeliveryRequestViewModel;
    public DeliveryReqCancelScheduleUpdateModel: CancelDeliverySchedule[] = [];
    public cancelDSDeliveryScheduleViewModel: CancelDSDeliveryScheduleViewModel[] = [];
    constructor(private fb: FormBuilder, private sbService: ScheduleBuilderService, private carrierService: CarrierService,
        private addresService: AddressService, private stateService: StatelistService,
        private dataService: DataService, private changeDetectorRef: ChangeDetectorRef, private utilService: UtilService) {
        this.MaxStartDate.setFullYear(this.MaxStartDate.getFullYear() + 1);
    }

    ngOnInit(): void {
        this.DelGroupForm = this.utilService.getTripFormGroup(new TripModel());
        this.PickupForm = this.initPickupForm(new OrderPickupDetailModel());
        this.additiveOrders = [];
        this.PickupLocationTypeChange();
        this.zipcodeConditionallyValidator();
        this.multiSelectSettings();
        this.subscribeGetOrderSubject()
        this.MinDate = new Date(this.MinDate.getFullYear(), this.MinDate.getMonth(), this.MinDate.getDate(), 0, 0, 0);
        this.MaxDate.setFullYear(this.MinDate.getFullYear() + 30);
    }

    multiSelectSettings() {
        this.multiDropdownSettings = {
            singleSelection: true,
            closeDropDownOnSelection: true,
            idField: 'Id',
            textField: 'Name',
            allowSearchFilter: true,
            itemsShowLimit: 1
        };
        this.CustomerSettings = {
            singleSelection: true,
            closeDropDownOnSelection: true,
            idField: 'CompanyId',
            textField: 'CompanyName',
            enableCheckAll: false,
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.SiteddlSettings = {
            singleSelection: true,
            closeDropDownOnSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            itemsShowLimit: 1,
            allowSearchFilter: true

        };
    }
    ngAfterViewInit(): void {
        this.subscribeEditGroupSubject();
        this.subscribeShowDeliveryGroupSubject();
        this.subscribeShowOpenedDeliveryGroupSubject();
        this.subscribeCommonPickupLocationChange();
        this.subscribeHideDeliveryGroupSubject();
        this.subscribeRouteResetGroupSubject();
    }

    ngOnChanges(change: SimpleChanges): void {
        if (change.SelectedDate && change.SelectedDate.currentValue) {
            if (this.MaxCalendarDate < this.SelectedDate) {
                this.MaxCalendarDate = moment(new Date(this.SelectedDate)).add(1, 'day').toDate();
                this.MinCalendarDate = moment(new Date(this.SelectedDate)).toDate();
            } else {
                this.MinCalendarDate = moment(new Date(this.SelectedDate)).toDate();
                this.MaxCalendarDate = moment(new Date(this.SelectedDate)).add(1, 'day').toDate();
            }

        }
    }

    ngOnDestroy(): void {
        this.unsubscribeAllSubscriptions();
    }

    private unsubscribeAllSubscriptions(): void {
        if (this.DGSubscription) {
            this.DGSubscription.unsubscribe();
        }
        if (this.HideDeliveryGroupSubject) {
            this.HideDeliveryGroupSubject.unsubscribe();
        }
        if (this.RouteResetGroupSubject) {
            this.RouteResetGroupSubject.unsubscribe();
        }
    }

    private subscribeEditGroupSubject(): void {
        let subs = this.dataService.EditDeliveryGroupSubject.subscribe(x => {
            if (x) {
                if (x.routeName != null) {
                    this.RouteListForTrip = x.routeName;
                }
                this.ScheduleOptionalPickupDetailModel = [];
                this.ScheduleOrderFuelInfo = null;
                if (x.isOptionalPickup) {
                    this.ScheduleOptionalPickupDetailModel = x.OptionalPickupInfo;
                    this.ScheduleOrderFuelInfo = x.OrderFuelInfo;
                }
                this.editGroup(x.trip, x.shiftIndex, x.rowIndex, x.tripIndex, x.schedule, x.isPublishLoadInvalid, x.isOptionalPickup);
            }
        });
        this.DGSubscription.add(subs);
    }

    private subscribeShowDeliveryGroupSubject(): void {
        let subs = this.dataService.ShowDeliveryGroupSubject.subscribe(x => {
            if (x != null && x != undefined) {
                x ? this.showDeliveryGroup() : this.hideDeliveryGroup();
            }
        });
        this.subscribeDeleteDRRequestSubject();
        this.DGSubscription.add(subs);
    }

    private subscribeShowOpenedDeliveryGroupSubject(): void {
        let subs = this.dataService.ShowOpenedDeliveryGroupSubject.subscribe(x => {
            if (x != null && x != undefined) {
                x ? this.showOpenedDeliveryGroup() : this.hideDeliveryGroup();
            }
        });
        this.subscribeDeleteDRRequestSubject();
        this.DGSubscription.add(subs);
    }

    private PickupLocationTypeChange(): void {
        this.DGSubscription.add(this.PickupForm.controls['PickupLocationType'].valueChanges.subscribe((data) => {
            this.PickupForm.markAllAsTouched;
            this.PickupForm.markAsDirty();
            this.setPickupValidators(this.PickupForm, data);
            let commonPickup = this.DelGroupForm.controls['IsCommonPickup'].value;
            commonPickup ? this.disableDeliveryGroupPickups() : this.enableDeliveryGroupPickups();
        }));
    }
    public subscribeHideDeliveryGroupSubject(): void {
        this.HideDeliveryGroupSubject = this.dataService.HideDeliveryGroupSubject.subscribe(x => {
            if (x && x == true) {
                this.hideDeliveryGroup();
            }
        });
    }
    initPickupForm(order: OrderPickupDetailModel): FormGroup {
        let isTerminalPickup = order && order.PickupLocationType != 2;
        var _pForm = this.fb.group({
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

    setPickupLocation(form: FormGroup, order: OrderPickupLocationModel): void {
        form.controls['PickupLocationType'].patchValue(order.PickupLocationType);
        if (order.PickupLocationType != 2) {
            if (order.Terminal) {
                form.controls['Terminal'].patchValue(order.Terminal);
                this.dataService.setScheduleChangeDetectSubject(true);
            }
            this.PickupForm.controls['PickupLocationType'].patchValue(1);
            this.PickupForm.controls['Terminal'].patchValue(order.Terminal);
        } else {
            if (order.BulkPlant) {
                if (order.BulkPlant.ZipCode) {
                    order.BulkPlant.ZipCode = order.BulkPlant.ZipCode.toUpperCase();
                }
                form.controls['BulkPlant'].patchValue(order.BulkPlant);
                this.dataService.setScheduleChangeDetectSubject(true);
            }
            this.PickupForm.controls['PickupLocationType'].patchValue(2);
            this.PickupForm.controls['BulkPlant'].patchValue(order.BulkPlant);
        }
    }

    setPickupValidators(form: FormGroup, pickupType: number): void {
        if (form.get('IsAdditive') && form.get('IsAdditive').value) {
            form.get('BulkPlant.Address').setValidators(null);
            form.get('BulkPlant.Address').updateValueAndValidity();
            form.get('BulkPlant.City').setValidators(null);
            form.get('BulkPlant.City').updateValueAndValidity();
            form.get('BulkPlant.State.Id').setValidators(null);
            form.get('BulkPlant.State.Id').updateValueAndValidity();
            form.get('BulkPlant.Country.Code').setValidators(null);
            form.get('BulkPlant.Country.Code').updateValueAndValidity();
            form.get('BulkPlant.ZipCode').setValidators(null);
            form.get('BulkPlant.ZipCode').updateValueAndValidity();
            form.get('BulkPlant.CountyName').setValidators(null);
            form.get('BulkPlant.CountyName').updateValueAndValidity();
            form.get('BulkPlant.Latitude').setValidators(null);
            form.get('BulkPlant.Latitude').updateValueAndValidity();
            form.get('BulkPlant.Longitude').setValidators(null);
            form.get('BulkPlant.Longitude').updateValueAndValidity();
            form.get('BulkPlant.SiteName').setValidators(null);
            form.get('BulkPlant.SiteName').updateValueAndValidity();
            form.get('Terminal.Name').setValidators(null);
            form.get('Terminal.Name').updateValueAndValidity();
            form.get('Terminal.Id').setValidators(null);
            form.get('Terminal.Id').updateValueAndValidity();
        }
        else if (pickupType != 2) {
            form.get('BulkPlant.Address').setValidators(null);
            form.get('BulkPlant.Address').updateValueAndValidity();
            form.get('BulkPlant.City').setValidators(null);
            form.get('BulkPlant.City').updateValueAndValidity();
            form.get('BulkPlant.State.Id').setValidators(null);
            form.get('BulkPlant.State.Id').updateValueAndValidity();
            form.get('BulkPlant.Country.Code').setValidators(null);
            form.get('BulkPlant.Country.Code').updateValueAndValidity();
            form.get('BulkPlant.ZipCode').setValidators(null);
            form.get('BulkPlant.ZipCode').updateValueAndValidity();
            form.get('BulkPlant.CountyName').setValidators(null);
            form.get('BulkPlant.CountyName').updateValueAndValidity();
            form.get('BulkPlant.Latitude').setValidators(null);
            form.get('BulkPlant.Latitude').updateValueAndValidity();
            form.get('BulkPlant.Longitude').setValidators(null);
            form.get('BulkPlant.Longitude').updateValueAndValidity();
            form.get('BulkPlant.SiteName').setValidators(null);
            form.get('BulkPlant.SiteName').updateValueAndValidity();
            form.get('Terminal.Name').setValidators([Validators.required]);
            form.get('Terminal.Name').updateValueAndValidity();
            form.get('Terminal.Id').setValidators([Validators.required]);
            form.get('Terminal.Id').updateValueAndValidity();
        } else {
            form.get('Terminal.Name').setValidators(null);
            form.get('Terminal.Name').updateValueAndValidity();
            form.get('Terminal.Id').setValidators(null);
            form.get('Terminal.Id').updateValueAndValidity();
            form.get('BulkPlant.Address').setValidators([Validators.required]);
            form.get('BulkPlant.Address').updateValueAndValidity();
            form.get('BulkPlant.City').setValidators([Validators.required]);
            form.get('BulkPlant.City').updateValueAndValidity();
            form.get('BulkPlant.State.Id').setValidators([Validators.required]);
            form.get('BulkPlant.State.Id').updateValueAndValidity();
            form.get('BulkPlant.Country.Code').setValidators([Validators.required]);
            form.get('BulkPlant.Country.Code').updateValueAndValidity();
            form.get('BulkPlant.ZipCode').setValidators([Validators.required]);
            form.get('BulkPlant.ZipCode').updateValueAndValidity();
            form.get('BulkPlant.CountyName').setValidators([Validators.required]);
            form.get('BulkPlant.CountyName').updateValueAndValidity();
            form.get('BulkPlant.Latitude').setValidators([Validators.required, Validators.pattern('^[0-9.-]*$')]);
            form.get('BulkPlant.Latitude').updateValueAndValidity();
            form.get('BulkPlant.Longitude').setValidators([Validators.required, Validators.pattern('^[0-9.-]*$')]);
            form.get('BulkPlant.Longitude').updateValueAndValidity();
            form.get('BulkPlant.SiteName').setValidators([Validators.required]);
            form.get('BulkPlant.SiteName').updateValueAndValidity();
        }
    }


    removePickupValidators(form: FormGroup, pickupType: number): void {
        if (pickupType != 2) {
            form.get('BulkPlant.Address').setValidators(null);
            form.get('BulkPlant.Address').updateValueAndValidity();
            form.get('BulkPlant.City').setValidators(null);
            form.get('BulkPlant.City').updateValueAndValidity();
            form.get('BulkPlant.State.Id').setValidators(null);
            form.get('BulkPlant.State.Id').updateValueAndValidity();
            form.get('BulkPlant.Country.Code').setValidators(null);
            form.get('BulkPlant.Country.Code').updateValueAndValidity();
            form.get('BulkPlant.ZipCode').setValidators(null);
            form.get('BulkPlant.ZipCode').updateValueAndValidity();
            form.get('BulkPlant.CountyName').setValidators(null);
            form.get('BulkPlant.CountyName').updateValueAndValidity();
            form.get('BulkPlant.Latitude').setValidators(null);
            form.get('BulkPlant.Latitude').updateValueAndValidity();
            form.get('BulkPlant.Longitude').setValidators(null);
            form.get('BulkPlant.Longitude').updateValueAndValidity();
            form.get('BulkPlant.SiteName').setValidators(null);
            form.get('BulkPlant.SiteName').updateValueAndValidity();
            form.get('Terminal.Name').clearValidators();
            form.get('Terminal.Name').updateValueAndValidity();
            form.get('Terminal.Id').clearValidators();
            form.get('Terminal.Id').updateValueAndValidity();
        } else {
            form.get('Terminal.Name').setValidators(null);
            form.get('Terminal.Name').updateValueAndValidity();
            form.get('Terminal.Id').setValidators(null);
            form.get('Terminal.Id').updateValueAndValidity();
            form.get('BulkPlant.Address').clearValidators();
            form.get('BulkPlant.Address').updateValueAndValidity();
            form.get('BulkPlant.City').clearValidators();
            form.get('BulkPlant.City').updateValueAndValidity();
            form.get('BulkPlant.State.Id').clearValidators();
            form.get('BulkPlant.State.Id').updateValueAndValidity();
            form.get('BulkPlant.Country.Code').clearValidators();
            form.get('BulkPlant.Country.Code').updateValueAndValidity();
            form.get('BulkPlant.ZipCode').clearValidators();
            form.get('BulkPlant.ZipCode').updateValueAndValidity();
            form.get('BulkPlant.CountyName').clearValidators();
            form.get('BulkPlant.CountyName').updateValueAndValidity();
            form.get('BulkPlant.Latitude').clearValidators();
            form.get('BulkPlant.Latitude').updateValueAndValidity();
            form.get('BulkPlant.Longitude').clearValidators();
            form.get('BulkPlant.Longitude').updateValueAndValidity();
            form.get('BulkPlant.SiteName').clearValidators();
            form.get('BulkPlant.SiteName').updateValueAndValidity();
        }
    }

    getOrderList(delReq: FormGroup, isCommonPickup: boolean, startDate: string): void {
        let _jobId = delReq.controls['JobId'].value;
        let _productTypeId = delReq.controls['ProductTypeId'].value;
        let _orderId = delReq.controls['OrderId'];
        let _carrierStatus = delReq.controls['CarrierStatus'].value;
        let isBlendReq = delReq.controls['IsBlendedRequest'].value;
        let isDrForTank = delReq.controls['TankId'].value && delReq.controls['StorageId'].value;
        let existing = this.OrderList[_jobId.toString() + '_' + _productTypeId.toString() + '_' + (isBlendReq ? 1 : 0)];
        if (existing == undefined || existing == null || existing.length == 0) {
            this.DGSubscription.add(this.sbService.getOrders(_jobId, _productTypeId, startDate, _carrierStatus, isBlendReq).subscribe(data => {
                this.OrderList[_jobId.toString() + '_' + _productTypeId.toString() + '_' + (isBlendReq ? 1 : 0)] = data;
                if (data.length > 0) {
                    let order = data[0];
                    if (_orderId && _orderId.value > 0) {
                        var orderFromData = data.filter(t => t.OrderId == _orderId.value);
                        if (orderFromData != null && orderFromData.length > 0) {
                            order = orderFromData[0];
                        }
                    }
                    if (_orderId.value == null || _orderId.value == 0 || (_orderId.value > 0 && _orderId.value != order.OrderId && isDrForTank)) {
                        delReq.controls['OrderId'].setValue(order.OrderId);
                        this.onDeliveryRequestChange(delReq);
                    }
                }
                else {
                    //No Order Found then set order id 0.
                    delReq.controls['OrderId'].setValue(0);
                }
                this.setDRPickupValidators(delReq, delReq.value.PickupLocationType);
            }));
        }
        else {
            var order = existing[0];
            if (_orderId.value > 0) {
                var orderFromData = existing.filter(t => t.OrderId == _orderId.value);
                if (orderFromData != null && orderFromData.length > 0) {
                    order = orderFromData[0];
                } 
            }
            if (_orderId.value == undefined || _orderId.value == null || _orderId.value == '' || _orderId.value == 0 || (_orderId.value > 0 && _orderId.value != order.OrderId && isDrForTank)) {
                delReq.controls['OrderId'].setValue(order.OrderId);
                this.onDeliveryRequestChange(delReq);                
            }      
            this.setDRPickupValidators(delReq, delReq.value.PickupLocationType);
        }
    }

    editGroup(trip: FormGroup, shiftIndex: number, rowIndex: number, tripIndex: number, schedule: any, isPublishLoadInvalid = false, isOptionalPickup = false) {
        this.SubDrsStatusWithParentIds = [];
        this.PickupForm.reset();
        this.DelGroupForm = trip;
        this.fuelTypeOrderList = [];
        this.UnchangedTrip = CustomAbstractControls.cloneForm(trip);
        this.changeDetectorRef.markForCheck();
        this.DelGroupForm.markAllAsTouched();
        this.PickupForm.markAllAsTouched();
        this._shiftIndex = shiftIndex;
        this._rowIndex = rowIndex;
        this._tripIndex = tripIndex;
        this.Schedule = schedule;
        this.isOptionalPickup = isOptionalPickup;
        var isCommonPickup: boolean = trip.controls['IsCommonPickup'].value;
        var delReqs = trip.controls['DeliveryRequests'] as FormArray;
        let groupDRParentIds: SubDRStatus[] = [];
        delReqs.controls.forEach((x: FormGroup) => {
            if (x.controls["GroupParentDRId"].value == null) {
                x.controls["GroupParentDRId"].setValue('');
            }
            if (x.controls['JobId'].value > 0 && x.controls['IsTBD'].value == false)
                this.getOrderList(x, isCommonPickup, trip.controls['StartDate'].value);
            else
                this.getDropLocationDetails(x);

            if (x.controls['GroupParentDRId'].value != null && x.controls['GroupParentDRId'].value != "")
                groupDRParentIds.push(x.controls['GroupParentDRId'].value);
        });

        if (groupDRParentIds.length > 0) {
            groupDRParentIds = groupDRParentIds.filter((n, i) => groupDRParentIds.indexOf(n) === i);
            this.sbService.getSubDRStatus(groupDRParentIds).subscribe((data: SubDRStatus[]) => {
                if (data && data.length > 0)
                    this.SubDrsStatusWithParentIds = data;
            });
        }
        this.validateOptionalPickup();
        for (var i = 0; i < delReqs.length; i++) {
            if (delReqs.controls[i].get('IsCommonBadge').value == true) {
                trip.controls['IsCommonBadge'].setValue(true);
                break;
            }
        }
        for (var i = 0; i < delReqs.length; i++) {
            if (delReqs.controls[i].get('PreLoadInfo') != null && delReqs.controls[i].get('PreLoadInfo').value != null) {
                trip.controls['IsPreLoadInfo'].setValue(true);
                break;
            }
        }
        if (trip.controls['IsCommonPickup'].value == true) {
            this.disableDeliveryGroupPickups();
        }
        else {
            this.enableDeliveryGroupPickups();
        }

        //get additive product orders if required
        let drs = delReqs.value as DeliveryRequestViewModel[];
        if (drs.some(dr => dr.IsBlendedRequest)) {
            this.getAdditiveOrders();
        }
        this.deletedBlend_all = [];
        //if (drs.some(dr => dr.IsBlendedRequest && !dr.StorageId)) {
        //    this.getCompatibleProducts();
        //}
    }

    private subscribeCommonPickupLocationChange(): void {
        this.PickupLocationSubscription = this.DelGroupForm.controls['IsCommonPickup'].valueChanges
            .subscribe(x => {
                x ? this.disableDeliveryGroupPickups() : this.enableDeliveryGroupPickups();
                this.setCommonPickupFlag();
            });
        this.DGSubscription.add(this.PickupLocationSubscription);
    }

    public validateTerminal(terminal: any, event: any): void {
        if (!terminal.get('Id').value) {
            terminal.get('Name').patchValue('');
        }
    }

    private disableDeliveryGroupPickups(): void {
        let _drArray = this.DelGroupForm.controls['DeliveryRequests'] as FormArray;
        if (_drArray) {
            _drArray.controls.forEach((x: FormGroup) => {
                x.controls['Terminal'].disable();
                x.controls['BulkPlant'].disable();
            });
        }
        if (this.DelGroupForm.controls['PickupLocationType'].value == 2) {
            this.DelGroupForm.controls['BulkPlant'].enable();
            this.DelGroupForm.controls['Terminal'].disable();
        } else {
            this.DelGroupForm.controls['BulkPlant'].disable();
            this.DelGroupForm.controls['Terminal'].enable();
        }
    }

    private enableDeliveryGroupPickups(): void {
        let _drArray = this.DelGroupForm.controls['DeliveryRequests'] as FormArray;
        if (_drArray) {
            _drArray.controls.forEach((x: FormGroup) => {
                if (!x.get('IsAdditive').value) {
                    x.controls['Terminal'].enable();
                    x.controls['BulkPlant'].enable();
                }
            });
        }
        this.DelGroupForm.controls['BulkPlant'].disable();
        this.DelGroupForm.controls['Terminal'].disable();
    }

    public setSelectedDate(date: string): void {
        let _date = this.DelGroupForm.controls['StartDate'];
        if (_date.value != date) {
            _date.patchValue(date);
        }
    }

    public setScheduleSelectedDate(ScheduleDate: string, dr: FormGroup): void {
        if (ScheduleDate != dr.controls['SelectedDate'].value) {
            dr.controls['SelectedDate'].setValue(ScheduleDate);
            this.setSheduleSelectedDateForBlendGroup(dr);
        }
    }
    public setScheduleStartTime(StartTime: string, dr: FormGroup): void {
        if (StartTime != dr.controls['ScheduleStartTime'].value) {
            dr.controls['ScheduleStartTime'].setValue(StartTime);
            this.setScheduleStartTimeForBlendGroup(dr)
        }
    }
    public setScheduleEndTime(EndTime: string, dr: FormGroup): void {
        if (EndTime != dr.controls['ScheduleEndTime'].value) {
            dr.controls['ScheduleEndTime'].setValue(EndTime);
            this.setScheduleEndTimeForBlendGroup(dr)
        }
    }

    public setBadgeForBlendGroup(tdr: FormGroup, badgeNum: number) {
        if (tdr.get('IsBlendedRequest').value) {
            let drs = this.DelGroupForm.controls['DeliveryRequests'] as FormArray;
            let blendGroupId = tdr.get('BlendedGroupId').value;
            drs.controls.forEach(t => {
                if (t.get('BlendedGroupId').value == blendGroupId) {
                    t.get('BadgeNo' + badgeNum).setValue(tdr.get('BadgeNo' + badgeNum).value);
                    this.onDeliveryRequestChange(t as FormGroup);
                }
            })
        }
    }

    public setCommonBadgeForBlendGroup(tdr: FormGroup) {
        let drs = this.DelGroupForm.controls['DeliveryRequests'] as FormArray;
        if (tdr.get('IsBlendedRequest').value) {            
            let blendGroupId = tdr.get('BlendedGroupId').value;
            drs.controls.forEach(t => {
                if (t.get('BlendedGroupId').value == blendGroupId) {
                    t.get('IsCommonBadge').setValue(tdr.get('IsCommonBadge').value);
                    this.onDeliveryRequestChange(t as FormGroup);
                }
            })
        }
        else {
            for (var i = 0; i < drs.length; i++) {
                if (drs.controls[i].get('IsCommonBadge').value == true) {
                    this.DelGroupForm.controls['IsCommonBadge'].setValue(true);
                    break;
                }
            }
        }
    }
    public setNoteForBlendGroup(tdr: FormGroup) {
        if (tdr.get('IsBlendedRequest').value) {
            let drs = this.DelGroupForm.controls['DeliveryRequests'] as FormArray;
            let blendGroupId = tdr.get('BlendedGroupId').value;
            drs.controls.forEach(t => {
                if (t.get('BlendedGroupId').value == blendGroupId) {
                    t.get('Notes').setValue(tdr.get('Notes').value);
                    this.onDeliveryRequestChange(t as FormGroup);
                }
            })
        }
    }

    public setSheduleSelectedDateForBlendGroup(tdr:FormGroup) {
        if (tdr.get('IsBlendedRequest').value) {
            let drs = this.DelGroupForm.controls['DeliveryRequests'] as FormArray;
                let blendGroupId = tdr.get('BlendedGroupId').value;
                drs.controls.forEach(t => {
                    if (t.get('BlendedGroupId').value == blendGroupId){
                        t.get('SelectedDate').setValue(tdr.get('SelectedDate').value);
                        this.onDeliveryRequestChange(t as FormGroup);
                    }
                })
        }        
    }

    public setDeliveryLevelPOForBlendGroup(tdr:FormGroup) {
        if (tdr.get('IsBlendedRequest').value) {
            let drs = this.DelGroupForm.controls['DeliveryRequests'] as FormArray;
                let blendGroupId = tdr.get('BlendedGroupId').value;
                drs.controls.forEach(t => {
                    if (t.get('BlendedGroupId').value == blendGroupId) {
                        t.get('DeliveryLevelPO').setValue(tdr.get('DeliveryLevelPO').value);
                        this.onDeliveryRequestChange(t as FormGroup);
                    }
                })
        }    
    }

    public setScheduleStartTimeForBlendGroup(tdr:FormGroup) {
        if (tdr.get('IsBlendedRequest').value) {
            let drs = this.DelGroupForm.controls['DeliveryRequests'] as FormArray;
                let blendGroupId = tdr.get('BlendedGroupId').value;
                drs.controls.forEach(t => {
                    if (t.get('BlendedGroupId').value == blendGroupId) {
                        t.get('ScheduleStartTime').setValue(tdr.get('ScheduleStartTime').value);
                        this.onDeliveryRequestChange(t as FormGroup);
                    }
                })
        }    
    }
    public setScheduleEndTimeForBlendGroup(tdr:FormGroup) {
        if (tdr.get('IsBlendedRequest').value) {
            let drs = this.DelGroupForm.controls['DeliveryRequests'] as FormArray;
                let blendGroupId = tdr.get('BlendedGroupId').value;
                drs.controls.forEach(t => {
                    if (t.get('BlendedGroupId').value == blendGroupId) {
                        t.get('ScheduleEndTime').setValue(tdr.get('ScheduleEndTime').value);
                        this.onDeliveryRequestChange(t as FormGroup);
                    }
                })
        }    
    }

    showOpenedDeliveryGroup(): void {
        Declarations.showOpenDG();
    }

    showDeliveryGroup(): void {
        Declarations.showDG();
    }

    viewNotes(tdr): void {
        if (this.selectedViewNote != tdr.value.Id) {
            this.selectedViewNote = tdr.value.Id;
        } else {
            this.selectedViewNote = undefined;
        }
    }


    hideDeliveryGroup(): void {
        Declarations.closeDG();
        this.OrderList = {};
        if (this.StartDateSubscription) {
            this.StartDateSubscription.unsubscribe();
        }
        if (this.StartTimeSubscription) {
            this.StartTimeSubscription.unsubscribe();
        }
        if (this.PickupLocationSubscription) {
            this.PickupLocationSubscription.unsubscribe();
        }
        if (this.DeleteDRRequestSubject) {
            this.DeleteDRRequestSubject.unsubscribe();
        }
    }

    private setDeletePostloadedDrsSubject(drs: DeliveryRequestViewModel[]): void {
        let preloadedDrInfo = drs.filter(x => x.PreLoadInfo);
        let postloadedDrInfo = drs.filter(x => x.PostLoadInfo);
        let prepostloadedInfo = preloadedDrInfo.concat(postloadedDrInfo);
        if (prepostloadedInfo.length > 0) {
            this.dataService.setDeletePostloadSubject(prepostloadedInfo);
        }
    }

    deleteDeliveryGroup(isReset: boolean) {
        this.hideDeliveryGroup();
        this.setDeletePostloadedDrsSubject(this.DelGroupForm.controls['DeliveryRequests'].value);
        this.dataService.setDeleteDeliveryGroupSubject(this.DelGroupForm);
        this.dataService.setDeliveryPreloadOption({ ShiftIndex: this._shiftIndex, ScheduleIndex: this._rowIndex, Reset: true });
    }

    deleteSchedule(tripDetails: any): void {
        var _delReqs = tripDetails.trip.get('DeliveryRequests') as FormArray;
        this._currentTrip = tripDetails.trip;
        this.DeletedDrIndex = tripDetails.index;

        this.selectedDeliveryRequest = _delReqs.controls[this.DeletedDrIndex].value;
        this.selectedDeliveryRequest.WindowMode = 1;
        this.selectedDeliveryRequest.QueueMode = 1;


        if (this.selectedDeliveryRequest.TrackableScheduleId != null && this.selectedDeliveryRequest.TrackableScheduleId > 0) {
            var scheduleIds: number[] = [];
            scheduleIds.push(this.selectedDeliveryRequest.TrackableScheduleId);

            //FOR BLENDED REQUESTS - DELETE BLEND GROUP
            if (this.selectedDeliveryRequest.IsBlendedRequest) {
                scheduleIds = [];
                let tripDrs = this.filterAndGetDrsByBlendGroupId(<DeliveryRequestViewModel[]>_delReqs.value, this.selectedDeliveryRequest.BlendedGroupId);
                tripDrs.forEach(dr => {
                    if (dr.TrackableScheduleId != null && dr.TrackableScheduleId > 0) {
                        scheduleIds.push(dr.TrackableScheduleId);
                    }
                });
            }

            this.sbService.getScheduleStatus(scheduleIds).subscribe(response => {
                if (response != null && response != undefined && response.length > 0) {
                    var data = response[0];
                    if (this.CompletedScheduleStatuses.indexOf(data.ScheduleStatusId) !== -1 || data.ScheduleEnrouteStatusId == 4) {
                        this.selectedDeliveryRequest = null;
                        this.DeletedDrIndex = null;
                        Declarations.msgerror("Can't delete as drop has been made already", 'Warning', 2500);
                        return;
                    }
                    else if (SBConstants.OnTheWayScheduleStatuses.indexOf(data.ScheduleEnrouteStatusId) !== -1) {
                        this.sbService.setConfirmationHeadingForDR(data.ScheduleEnrouteStatusId);
                        document.getElementById('btnconfirm-delete-dr').click();
                        return;
                    }
                    else {
                        this.deleteDeliveryRequestFromLoad();
                    }
                }
                else {
                    this.deleteDeliveryRequestFromLoad();
                }
            });
        }
        else {
            this.deleteDeliveryRequestFromLoad();
        }
    }

    public checkScheduleEditStatus(tdr: FormGroup) {
        let dr = tdr.value as DeliveryRequestViewModel;
        if (dr.Status != 3) {
            return true;
        }
        else {
            if (SBConstants.OnTheWayScheduleStatuses.indexOf(dr.TrackScheduleEnrouteStatus) !== -1 && dr.IsTBD == false) {
                return false;
            }
            if (this.CompletedScheduleStatuses.indexOf(dr.TrackScheduleStatus) !== -1 || dr.TrackScheduleEnrouteStatus == 4) {
                return false;
            }
        }
        return true;
    }

    public deleteDeliveryRequestFromLoad() {
        Declarations.hideModal('#confirm-delete-dr');
        let deletedRequests: DeliveryRequestViewModel[] = [];

        //FOR BLENDED REQUESTS
        if (this.selectedDeliveryRequest.IsBlendedRequest) {
            deletedRequests = this.filterAndGetDrsByBlendGroupId(this._currentTrip.get('DeliveryRequests').value, this.selectedDeliveryRequest.BlendedGroupId);
            deletedRequests.forEach(dr => { dr.Compartments = []; })
        }
        else {
            this.selectedDeliveryRequest.Compartments = [];
            deletedRequests.push(this.selectedDeliveryRequest);
        }

        this.setDeletePostloadedDrsSubject(deletedRequests);
        this.dataService.setRestoreDeletedRequestSubject(deletedRequests);
        let _delReqs = this._currentTrip.get('DeliveryRequests') as FormArray;

        //FOR BLENDED REQUESTS
        if (this.selectedDeliveryRequest.IsBlendedRequest) {
            let length = _delReqs.length;
            while (length > 0) {
                let index = _delReqs.value.findIndex(r => r.BlendedGroupId && r.BlendedGroupId == this.selectedDeliveryRequest.BlendedGroupId);
                if (index != -1) {
                    _delReqs.removeAt(index);
                }
                length--;
            }
        }
        else {
            _delReqs.removeAt(this.DeletedDrIndex);
        }
        //if (deletedRequests.length > 0) {
        //    var resetDeliveryModel = new ResetDeliveryGroupScheduleModel();
        //    resetDeliveryModel.DeliveryRequestIds = [];
        //    deletedRequests.forEach(x => {
        //        resetDeliveryModel.DeliveryRequestIds.push(x.Id);
        //    });
        //    this.dataService.setDeliveryScheduleRemoveSubject(resetDeliveryModel);
        //}
        this.selectedDeliveryRequest = null;
        this.DeletedDrIndex = null;
        this.dataService.setScheduleChangeDetectSubject(true);

    }

    public deleteDeliveryRequestFromLoadNo() {
        Declarations.hideModal('#confirm-delete-dr');
        this.selectedDeliveryRequest = null;
        this.DeletedDrIndex = null;
    }

    getAddressByZip(event: any): void {
        var zipCode = event.target.value;
        var regexUsa = new RegExp(RegExConstants.UsaZip);
        var regexCan = new RegExp(RegExConstants.CanZip);
        if (regexUsa.test(zipCode) || regexCan.test(zipCode)) {
            this._loadingAddress = true;
            this.DGSubscription.add(this.addresService.getAddress(zipCode)
                .subscribe(data => {
                    this._loadingAddress = false;
                    if (data != null && data != undefined && data.StateCode != null) {
                        if (data.CountryCode != 'CAR') {
                            data.CountryCode == 'US' || data.CountryCode == 'USA' ? data.CountryCode = 'USA' : data.CountryCode = 'CAN';
                        }
                        var addressModel = this.addressMapper(data);
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

    addressMapper(data: any): AddressModel {
        var state = this.StateList.find(x => x.Code == data.StateCode);
        var country = this.CountryList.find(x => x.Code == data.CountryCode);
        var _address = new AddressModel();
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

    onBulkPlantSelected(event: any): void {
        this.PickupForm.get('BulkPlant.SiteName').setValue(event.Name);
        this.PickupForm.get('BulkPlant.SiteId').setValue(event.Id);
        this.BulkPlants = this.BulkplantList.slice();
        this.getBulkPlantAddress(event.Name);
        if (this.PickupForm.get('BulkPlant.SiteName').valid) {
            this.isReadOnly = true;
        }
    }

    onBulkPlantSearched(event: any): void {
        var keyword = event.target.value.toLowerCase();
        this.BulkPlants = this.BulkplantList.slice().filter(function (elem) {
            return elem.Name && elem.Name.toLowerCase().indexOf(keyword) >= 0;
        });
        let bulkPlantName = this.PickupForm.get('BulkPlant.SiteName').value;
        this.isReadOnly = this.BulkPlants.filter(t => t.Name == bulkPlantName).length > 0;
    }

    onBulkPlantBlur(event: any): void {
        if (this.BulkPlants.filter(t => t.Name && t.Name.toLowerCase() == event.target.value.toLowerCase()).length > 0) {
            var bulkPlant = this.BulkPlants.find(t => t.Name.toLowerCase() == event.target.value.toLowerCase());
            this.PickupForm.get('BulkPlant.SiteName').setValue(bulkPlant.Name);
            this.PickupForm.get('BulkPlant.SiteId').setValue(bulkPlant.Id);
            this.getBulkPlantAddress(bulkPlant.Name);
            let bulkPlantName = this.PickupForm.get('BulkPlant.SiteName').value;
            this.isReadOnly = this.BulkPlants.filter(t => t.Name == bulkPlantName).length > 0;
        }
    }

    onTerminalSearched(event: any): void {
        var keyword = event.target.value.toLowerCase();
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
                of([])
                , this.sbService.getPickupTerminals(this.ChangePickupForOrders, keyword).pipe(
                    tap(() => {
                        this._loadingTerminals = false;
                    }),
                    catchError(() => {
                        this._loadingTerminals = false;
                        this.searchError = true;
                        return of([]);
                    })
                ))),
            tap(() => this._loadingTerminals = false)
        ).subscribe((data) => {
            this._loadingTerminals = false;
            if (data.length === 0) {
                this.noTerminalFound = true;
            } else {
                this.noTerminalFound = false;
            }
            this.Terminals = data;
        }, (err) => {
            console.log(err);
        }));
    }
    onTerminalSelected(event: any): void {
        this.PickupForm.get('Terminal').patchValue({ Id: event.Id, Name: event.Name });
    }

    changeTerminal(delReq: FormGroup, event: any): void {
        if (delReq.controls['IsTBD'].value == true) {
            return;
        }
        var _jobId = delReq.controls['JobId'].value;
        var _productTypeId = delReq.controls['ProductTypeId'].value;
        var _orderId = event.target.selectedOptions[0].value;
        let isBlend = delReq.controls['IsBlendedRequest'].value;
        var orders = this.OrderList[_jobId.toString() + '_' + _productTypeId.toString() + '_' + (isBlend ? 1 : 0)] as OrderPickupDetailModel[];
        if (orders.length > 0) {
            var selectedOrder = orders.find(x => x.OrderId == _orderId);
            if (selectedOrder) {
                if (delReq.value.IsCommonBadge == false || (selectedOrder.Badge1 && selectedOrder.Badge1.length > 0) || (selectedOrder.Badge2 && selectedOrder.Badge2.length > 0) || (selectedOrder.Badge3 && selectedOrder.Badge3.length > 0)) {
                    delReq.get('BadgeNo1').patchValue(selectedOrder.Badge1);
                    delReq.get('BadgeNo2').patchValue(selectedOrder.Badge2);
                    delReq.get('BadgeNo3').patchValue(selectedOrder.Badge3);
                    delReq.get('IsCommonBadge').patchValue(false);
                }
                let location = OrderPickupLocationModel.ToLocation(selectedOrder);
                this.setPickupLocation(delReq, location);
                this.setPickupValidators(delReq, location.PickupLocationType);
            }
        }
        this.onDeliveryRequestChange(delReq);
    }

    setStateCode(event: any) {
        this.PickupForm.get('BulkPlant.State.Code').setValue(event.target.selectedOptions[0].text);
    }

    getBulkPlantAddress(bulkPlantName: string) {
        this.DGSubscription.add(this.addresService.GetBulkPlantDetails(bulkPlantName).subscribe(response => {
            this.PickupForm.controls['BulkPlant'].patchValue({
                Address: response.Address,
                City: response.City,
                State: response.State,
                Country: { Code: response.Country.Code },
                ZipCode: response.ZipCode,
                CountyName: response.CountyName,
                Latitude: response.Latitude,
                Longitude: response.Longitude
            });
        }));
        this.PickupForm.markAllAsTouched();
        this.PickupForm.markAsDirty();
    }

    getDropLocationDetails(tdr: FormGroup): void {
        let tfxProductId = tdr.controls['FuelTypeId'].value;
        let productTypeId = tdr.controls['ProductTypeId'].value;
        let dr = tdr.value as DeliveryRequestViewModel;
        let terminalId = null, bulkplantId = null;
        let pickupId = '';
        if (dr.Terminal && dr.Terminal.Id > 0) {
            terminalId = dr.Terminal.Id;
            pickupId = 'T_' + terminalId.toString();
        }
        if (dr.BulkPlant && dr.BulkPlant.SiteId > 0) {
            bulkplantId = dr.BulkPlant.SiteId;
            pickupId = 'B_' + bulkplantId.toString();
        }
        if (this.fuelTypeOrderList[dr.PickupLocationType.toString() + '_' + pickupId + '_' + dr.FuelTypeId.toString()] == undefined) {
            if (this.SelectedRegionId != null && this.SelectedRegionId != undefined && this.SelectedRegionId != '') {
                this.sbService.getJobDetailsWithOrders(this.SelectedRegionId, tfxProductId, productTypeId, terminalId, bulkplantId, this.DelGroupForm.controls['StartDate'].value).subscribe(response => {
                    if (response) {
                        this.fuelTypeOrderList[dr.PickupLocationType.toString() + '_' + pickupId + '_' + dr.FuelTypeId.toString()] = response || [] as JobDetailsWithOrders[];
                        let companyList = response.map((element) => ({ CompanyId: element.CompanyId, CompanyName: element.CompanyName }));
                        this.TbdCustomerList[dr.Id] = this.GetUniqueCustomers(companyList.reduce((p, n) => p.concat(n), []));
                        let siteList = response.map((element) => ({ Id: element.JobId, Name: element.JobName }));
                        this.TbdLocationList[dr.Id] = this.GetUniqueLocations(siteList.reduce((p, n) => p.concat(n), []));
                        if (dr.JobId > 0) {
                            this.TbdOrderList[dr.Id] = response.map((element) => ({ Id: element.OrderId, Name: element.PoNumber }));
                        }
                    }
                    //else {
                    //    Declarations.msgerror('No order exists for this fuel type', 'error', 2500)
                    //}
                });
            }
        }
        else {
            let orders = this.fuelTypeOrderList[dr.PickupLocationType.toString() + '_' + pickupId + '_' + dr.FuelTypeId.toString()];
            let companyList = orders.map((element) => ({ CompanyId: element.CompanyId, CompanyName: element.CompanyName }));
            this.TbdCustomerList[dr.Id] = this.GetUniqueCustomers(companyList.reduce((p, n) => p.concat(n), []));
            let siteList = orders.map((element) => ({ Id: element.JobId, Name: element.JobName }));
            this.TbdLocationList[dr.Id] = this.GetUniqueLocations(siteList.reduce((p, n) => p.concat(n), []));
        }
    }

    public onSiteSelect_TBD(item: any, tdr: FormGroup): void {
        let dr = tdr.value as DeliveryRequestViewModel;
        tdr.get('JobId').setValue(item.Id);
        let orders = this.getOrdersOfDeliveryReq(dr);
        if (orders && orders.length > 0) {
            let drOrders = orders.filter(t => t.JobId == item.Id).map((element) => ({ Id: element.OrderId, Name: element.PoNumber }));
            this.TbdOrderList[dr.Id] = drOrders;
            if (drOrders && drOrders.length > 0) {
                tdr.get('OrderId').setValue(drOrders[0].Id);
                this.updateDrForm(tdr, null);
            }
        }
    }

    public onSiteDeSelect_TBD(item: DropdownItem): void {

    }

    public updateDrForm(delReq: FormGroup, event: any): void {
        let dr = delReq.value;
        var _orderId = 0;
        if (event)
            _orderId = event.target.selectedOptions[0].value;
        else
            _orderId = delReq.get('OrderId').value;
        let orders = this.getOrdersOfDeliveryReq(dr);
        let order = orders.find(t => t.OrderId == _orderId);
        if (order) {
            delReq.get('JobId').setValue(order.JobId);
            delReq.get('OrderId').setValue(order.OrderId);
            delReq.get('JobName').setValue(order.JobName);
            delReq.get('JobAddress').setValue(order.Address);
            delReq.get('JobCity').setValue(order.City)
            delReq.get('UoM').setValue(order.UoM);
            delReq.get('CustomerCompany').setValue(order.CompanyName);
            delReq.get('ScheduleStartTime').setValue(order.ScheduleEndTime);
            delReq.get('ScheduleEndTime').setValue(order.ScheduleEndTime);
            if ((dr.BadgeNo1 == null || dr.BadgeNo1 == '') && (dr.BadgeNo2 == null || dr.BadgeNo2 == '') && (dr.BadgeNo3 == null || dr.BadgeNo3 == '')) {
                let trip = this.DelGroupForm.value;
                if ((trip.BadgeNo1 == null || trip.BadgeNo1 == '') && (trip.BadgeNo2 == null || trip.BadgeNo2 == '') && (trip.BadgeNo3 == null || trip.BadgeNo3 == '')) {
                    delReq.get('BadgeNo1').setValue(order.BadgeNo1);
                    delReq.get('BadgeNo2').setValue(order.BadgeNo2);
                    delReq.get('BadgeNo3').setValue(order.BadgeNo3);
                    delReq.get('IsCommonBadge').setValue(false);
                }
            }
            this.onDeliveryRequestChange(delReq);
        }
    }

    public onCustomerSelect_TBD(item: any, tdr: FormGroup): void {
        let dr = tdr.value;
        let orders = this.getOrdersOfDeliveryReq(dr);
        tdr.get('TBDLocations').setValue([]);
        let siteList = orders.filter(x => x.CompanyId == item.CompanyId).map((element) => ({ Id: element.JobId, Name: element.JobName }));
        this.TbdLocationList[dr.Id] = this.GetUniqueLocations(siteList.reduce((p, n) => p.concat(n), []));
    }

    private getOrdersOfDeliveryReq(dr: DeliveryRequestViewModel) {

        let pickupId = '';
        let orders: any = {};
        if (dr.Terminal && dr.Terminal.Id > 0) {
            pickupId = 'T_' + dr.Terminal.Id.toString();
        }
        if (dr.BulkPlant && dr.BulkPlant.SiteId > 0) {
            pickupId = 'B_' + dr.BulkPlant.SiteId.toString();
        }
        if (dr.FuelTypeId) {
            orders = this.fuelTypeOrderList[dr.PickupLocationType.toString() + '_' + pickupId + '_' + dr.FuelTypeId.toString()];
        }
        else {
            orders= this.fuelTypeOrderList[dr.PickupLocationType.toString() + '_' + pickupId + '_' + dr.ProductTypeId.toString()];
        }
        return orders || [];
    }

    public onCustomerDeSelect_TBD(item: any, tdr: FormGroup): void {
        let dr = tdr.value;
        let orders = this.getOrdersOfDeliveryReq(dr);
        tdr.get('TBDLocations').setValue([]);
        let siteList = orders.map((element) => ({ Id: element.JobId, Name: element.JobName }));
        this.TbdLocationList[dr.Id] = this.GetUniqueLocations(siteList.reduce((p, n) => p.concat(n), []));
    }

    GetUniqueLocations(items) {
        const ids = [];
        var uniqueItems = items.filter(item => ids.includes(item.Id) ? false : ids.push(item.Id));
        return uniqueItems.sort((a, b) => a.Name.localeCompare(b.Name));
    }

    GetUniqueCustomers(items) {
        const ids = [];
        var uniqueItems = items.filter(item => ids.includes(item.CompanyId) ? false : ids.push(item.CompanyId));
        return uniqueItems.sort((a, b) => a.CompanyName.localeCompare(b.CompanyName));
    }

    clearDropLocationControls() {
        this.drOrders = [];
    }

    RemovePickupLocation(tdr: FormGroup): void {
        if (tdr != null || tdr != undefined) {
            this.SelectedDRForEditPickup = tdr;
            var _pickupType = this.SelectedDRForEditPickup.controls['PickupLocationType'].value;
            _pickupType = _pickupType == 0 ? 1 : _pickupType;
            this.PickupForm.reset();
            let terminal = this.utilService.getTerminalForm(new DropdownItem(), false);
            let bulkPlant = this.utilService.getBulkPlantForm(new DropAddressModel(), false);
            var _terminal = terminal.value;
            var _bulkPlant = bulkPlant.value;
            let location = {
                PickupLocationType: _pickupType,
                Terminal: _terminal,
                BulkPlant: _bulkPlant
            };
            this.setPickupLocation(this.SelectedDRForEditPickup, location);
            this.removePickupValidators(this.SelectedDRForEditPickup, _pickupType);
            this.onDeliveryRequestChange(this.SelectedDRForEditPickup);
        }
    }

    get StatesListByCountry(): any[] {
        let countryCode = this.PickupForm.controls['BulkPlant']['controls'].Country.get("Code").value;
        if (countryCode && this.CountryList && this.CountryList.length > 0) {

            countryCode = countryCode == "US" ? "USA" : countryCode;
            let countryId = 0;
            let county = this.CountryList.find(c => c.Code == countryCode)
            if (county && county.Id)
                countryId = county.Id;

            if (countryId == 4) {
                let countryGroupCode = this.PickupForm.controls['BulkPlant']['controls'].CountryGroup.get("Id").value;
                return this.StateList.filter(t => t.CountryId == 4 && t.CountryGroupId == countryGroupCode);
            }
            else {
                return this.StateList.filter(t => t.CountryId == countryId);
            }
        }

    }


    editPickupLocation(tdr: FormGroup): void {
        this.initialValue = '';
        //this.clearAutoComplete();
        if (!this.StateList || this.StateList.length == 0) {
            this.stateService.getStates().subscribe(x => this.StateList = x);
        }
        if (!this.CountryList || this.CountryList.length == 0) {
            this.stateService.getCountries().subscribe(x => this.CountryList = x);
        }

        if (!this.CountryGroupList || this.CountryGroupList.length == 0) {
            this.stateService.getCountryGroup(4).subscribe(x => this.CountryGroupList = x);
        }

        this.ChangePickupForOrders = [];
        this.SelectedDRForEditPickup = tdr;
        if (tdr == null || tdr == undefined) {
            this.SelectedDRForEditPickup = this.DelGroupForm
            var _delReqs = this.DelGroupForm.controls['DeliveryRequests'] as FormArray;
            _delReqs.controls.forEach((x: FormGroup) => {
                var _orderId = x.controls['OrderId'].value as number;
                if (_orderId > 0) {
                    this.ChangePickupForOrders.push(_orderId);
                }
            });
        } else {
            var _orderId = this.SelectedDRForEditPickup.controls['OrderId'].value as number;
            if (_orderId > 0) {
                this.ChangePickupForOrders.push(_orderId);
                var _jobId = tdr.get('JobId').value;
                var _productTypeId = tdr.get('ProductTypeId').value;
                let isBlend = tdr.get('IsBlendedRequest').value;
                var existing = this.OrderList[_jobId.toString() + '_' + _productTypeId.toString() + '_' + (isBlend ? 1 : 0)];
                var orders = existing.filter(t => t.OrderId == _orderId);
                if (orders.length > 0) {
                    var lastIndex = orders[0].PoNumber.lastIndexOf(" - ");
                    this.SearchTerminalFuelType = orders[0].PoNumber.slice(0, lastIndex + 1);
                }
            }
        }
        var _pickupType = this.SelectedDRForEditPickup.controls['PickupLocationType'].value;
        _pickupType = _pickupType == 0 ? 1 : _pickupType;
        var _terminal = this.SelectedDRForEditPickup.controls['Terminal'].value;
        var _bulkPlant = this.SelectedDRForEditPickup.controls['BulkPlant'].value;
        let location = {
            PickupLocationType: _pickupType,
            Terminal: _terminal,
            BulkPlant: _bulkPlant
        };
        this.setPickupLocation(this.SelectedDRForEditPickup, location);
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
        //this.setPickupValidators(this.SelectedDRForEditPickup, _pickupType);
        this.DGSubscription.add(this.sbService.getPickupTerminals(this.ChangePickupForOrders, '').subscribe((data: DropdownItem[]) => {
            this.Terminals = data;
        }));
        if (location.PickupLocationType != 2 && _terminal && _terminal.Name != null) {
            this.initialValue = _terminal.Name;
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

    onCommonPickupChkChange() {
        var isCommonPickup = this.DelGroupForm.controls['IsCommonPickup'].value;
        if (!isCommonPickup) {
            var deliveryRequests = this.DelGroupForm.controls['DeliveryRequests'] as FormArray;
            deliveryRequests.controls.forEach((x: FormGroup) => {
                var _jobId = x.controls['JobId'].value;
                var _productTypeId = x.controls['ProductTypeId'].value;
                var _orderId = x.controls['OrderId'].value;
                let isBlend = x.controls['IsBlendedRequest'].value;
                var existing = this.OrderList[_jobId.toString() + '_' + _productTypeId.toString() + '_' + (isBlend ? 1 : 0)];
                var orders = existing.filter(t => t.OrderId == _orderId);
                if (orders != null && orders.length > 0) {
                    let location = OrderPickupLocationModel.ToLocation(orders[0]);
                    this.setPickupLocation(x, location);
                    this.onDeliveryRequestChange(x);
                }
            });
        }
    }

    updatePickupLocation() {
        if (this.SelectedDRForEditPickup != null) {
            var isTrip = false;
            let existingLocationType = this.SelectedDRForEditPickup.controls['PickupLocationType'].value;
            let existingTerminal = this.SelectedDRForEditPickup.get('Terminal.Id').value;
            let existingBulkplant = this.SelectedDRForEditPickup.get('BulkPlant.SiteName').value;
            let pickupLoc = this.PickupForm.value;
            this.SelectedDRForEditPickup.patchValue(pickupLoc);
            this.setPickupLocation(this.SelectedDRForEditPickup, pickupLoc);
            this.setPickupValidators(this.SelectedDRForEditPickup, pickupLoc.PickupLocationType);
            if (pickupLoc.PickupLocationType == 2) {
                if (this.SelectedDRForEditPickup.get('BulkPlant').value) {
                    let bulkPlant = this.SelectedDRForEditPickup.controls['BulkPlant'].value;
                    if (bulkPlant.Country) {
                        let countryCode = bulkPlant.Country.Code
                        if (countryCode) {
                            this.setBulkPlantAddressValidators(this.getCountryIdFromCode(countryCode));
                        }
                    }
                }
            }
            if (existingLocationType != pickupLoc.PickupLocationType || (pickupLoc.PickupLocationType == 2 && pickupLoc.BulkPlant.SiteName != existingBulkplant) || (pickupLoc.PickupLocationType != 2 && pickupLoc.Terminal.Id != existingTerminal)) {
                let _orderId = this.SelectedDRForEditPickup.value.OrderId;
                let _pickupLocationType = pickupLoc.PickupLocationType;
                let _pickupLocationId = (_pickupLocationType != 2) ? (pickupLoc.Terminal.Id) : (pickupLoc.BulkPlant.SiteId);
                let orderIds = [];
                var selectedFormValue = this.SelectedDRForEditPickup.value;
                if (selectedFormValue.hasOwnProperty('TripId')) {
                    isTrip = true;
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
                        if (!isTrip) {
                            if (this.SelectedDRForEditPickup.value.IsCommonBadge == false || (data[0].BadgeNo1 && data[0].BadgeNo1.length > 0) || (data[0].BadgeNo2 && data[0].BadgeNo2.length > 0) || (data[0].BadgeNo3 && data[0].BadgeNo3.length > 0)) {
                                this.SelectedDRForEditPickup.controls['BadgeNo1'].setValue(data[0].BadgeNo1);
                                this.SelectedDRForEditPickup.controls['BadgeNo2'].setValue(data[0].BadgeNo2);
                                this.SelectedDRForEditPickup.controls['BadgeNo3'].setValue(data[0].BadgeNo3);
                                this.SelectedDRForEditPickup.controls['IsCommonBadge'].setValue(false);
                            }
                        }
                        else {
                            var _drArray = this.SelectedDRForEditPickup.controls['DeliveryRequests'] as FormArray;
                            _drArray.controls.forEach((_drForm: FormGroup) => {
                                if (_drForm.value.OrderId && _drForm.value.OrderId > 0) {
                                    var badgeInfo = data.find(t => t.OrderId == _drForm.value.OrderId);
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
                }));
                this.onDeliveryRequestChange(this.SelectedDRForEditPickup);
            }
        }
        var dismiss = document.getElementById('btnPickupClose') as HTMLElement;
        dismiss.click();
    }
    //clearAutoComplete(): void {
    //    //e.stopPropagation();
    //    this.terminal_AutoComplete.clear();
    //}
    setCommonPickupFlag() {
        if (this.DelGroupForm.get('IsCommonPickup').value) {
            this.PickupForm.reset();
        } else {
            this.DelGroupForm.controls['Terminal'].reset()
            this.DelGroupForm.controls['BulkPlant'].reset();
        }
    }

    setTripStatus(): void {
        var tripPrevStatusId = this.DelGroupForm.controls['TripPrevStatus'].value as TripStatus;
        var tripStatusId = TripStatus.Added;
        if (tripPrevStatusId == TripStatus.None) {
            tripStatusId = TripStatus.Added;
        } else if (tripPrevStatusId == TripStatus.Added || tripPrevStatusId == TripStatus.Modified) {
            tripStatusId = TripStatus.Modified;
        }
        this.DelGroupForm.controls['TripStatus'].setValue(tripStatusId);
    }

    setDeliveryGroupStatus(statusId: DeliveryGroupStatus): void {
        this.DelGroupForm.controls['DeliveryGroupStatus'].setValue(statusId);
    }

    setDeliveryRequestStatus(statusId: DeliveryReqStatus): void {
        if (this.DelGroupForm != null && this.DelGroupForm != undefined) {
            var deliveryRequests = this.DelGroupForm.controls['DeliveryRequests'] as FormArray;
            for (var i = 0; i < deliveryRequests.length; i++) {
                deliveryRequests.controls[i].get('Status').setValue(statusId);
                if (deliveryRequests.controls[i].get('PreviousStatus').value != DeliveryReqStatus.ScheduleCreated && deliveryRequests.controls[i].get('ScheduleStatus').value == 0) {
                    deliveryRequests.controls[i].get('ScheduleStatus').setValue(14);
                }
            }
        }
    }

    processPostloadedDrsUpdatedValues(): void {
        let updatedPostloadedDrs = this.DelGroupForm.controls['DeliveryRequests'].value;
        if (updatedPostloadedDrs.length > 0) {
            updatedPostloadedDrs = updatedPostloadedDrs.filter(x => x.PostLoadInfo);
            if (updatedPostloadedDrs.length > 0) {
                this.dataService.setUpdatePostloadSubject(updatedPostloadedDrs);
            }
        }
    }

    draftDeliveryGroup(): void {
        var isValidLoad = this.validateDraftLoad();
        if (isValidLoad) {
            this.setBadgeAndNotesToBlendGroup();
            this.processPostloadedDrsUpdatedValues();
            this.dataService.setGroupChangeDetectSubject(true);
            this.dataService.setDraftDeliveryGroupSubject({ trip: this.DelGroupForm, filterChanged: false });
            this.hideDeliveryGroup();
            this.changeDetectorRef.markForCheck();
        }
    }

    publishDeliveryGroup(): void {
        var isValidLoad = this.validatePublishLoad();
        if (isValidLoad) {
            if (this.DelGroupForm.valid && this.DelGroupForm.controls.DeliveryRequests.value.length > 0 && this.DelGroupForm.controls.DeliveryRequests.valid) {
                this.setBadgeAndNotesToBlendGroup();
                this.processPostloadedDrsUpdatedValues();
                let data = { shiftIndex: this._shiftIndex, rowIndex: this._rowIndex, colIndex: this._tripIndex, schedule: this.Schedule, trip: this.DelGroupForm }
                if (this.DelGroupForm.value.StartTime != this.UnchangedTrip.value.StartTime || this.DelGroupForm.value.EndTime != this.UnchangedTrip.value.EndTime || this.DelGroupForm.value.StartDate != this.UnchangedTrip.value.StartDate) {
                    this.onDeliveryGroupChange();
                }
                this.dataService.setPublishDeliveryGroupSubject(data);
                this.hideDeliveryGroup();
                this.changeDetectorRef.markForCheck();
                this.dataService.setGroupChangeDetectSubject(true);
            } else {
                let error = " Invalid";
                for (let c in this.DelGroupForm.controls) {
                    this.DelGroupForm.controls[c].markAsTouched();
                    if (!(this.DelGroupForm.controls['DeliveryRequests'].valid || this.DelGroupForm.controls['Terminal'].valid || this.DelGroupForm.controls['BulkPlant'].valid))
                        error = "Choose Pickup Location";
                    this.DelGroupForm.controls[c].valid ? '' : error += "  " + ((c == 'DeliveryRequests' || c == 'Terminal' || c == 'BulkPlant') ? "" : c) + "  ";
                }
                Declarations.msgerror('', error, 5000);
            }
        }
    }

    setBadgeAndNotesToBlendGroup() {

        let deliveryRequests = <DeliveryRequestViewModel[]>this.DelGroupForm.get('DeliveryRequests').value;

        if (deliveryRequests.some(dr => dr.IsBlendedRequest)) {

            let drFormArray = <FormArray>this.DelGroupForm.get('DeliveryRequests');
            //only blended requests
            deliveryRequests = deliveryRequests.filter(dr => dr.IsBlendedRequest);
            //grouped by blend group
            let groupedDrs = groupBy(deliveryRequests, 'BlendedGroupId');

            drFormArray.controls.forEach(drForm => {

                if (drForm.get('IsBlendedRequest').value) {

                    let blendDrGroup = <DeliveryRequestViewModel[]>groupedDrs[drForm.get('BlendedGroupId').value];
                    let lastDr = blendDrGroup[blendDrGroup.length - 1];

                    if (lastDr) {

                        drForm.get('Notes').setValue(lastDr.Notes);
                        drForm.get('IsCommonBadge').setValue(lastDr.IsCommonBadge);
                        drForm.get('BadgeNo1').setValue(lastDr.BadgeNo1);
                        drForm.get('BadgeNo2').setValue(lastDr.BadgeNo2);
                        drForm.get('BadgeNo3').setValue(lastDr.BadgeNo3);
                    }
                }
            })
        }
    }

    validateDraftLoad() {
        var isValid = true;
        if (this.DelGroupForm.controls['StartTime'].invalid || this.DelGroupForm.controls['EndTime'].invalid || this.DelGroupForm.controls['StartDate'].invalid) {
            isValid = false;
            this.DelGroupForm.controls['StartDate'].touched;
            !this.DelGroupForm.controls['StartDate'].value ? Declarations.msgerror('', 'Invalid Date', 5000) : Declarations.msgerror('', 'Please fill required field', 5000);
        }
        return isValid;
    }

    validatePublishLoad() {
        var isValid = true;
        if (this.DelGroupForm.controls['StartTime'].invalid || this.DelGroupForm.controls['EndTime'].invalid || this.DelGroupForm.controls['StartDate'].invalid) {
            isValid = false;
            this.DelGroupForm.controls['StartDate'].touched;
            !this.DelGroupForm.controls['StartDate'].value ? Declarations.msgerror('', 'Invalid Date', 5000) : Declarations.msgerror('', 'Please fill required field', 5000);
        }
        if (this.DelGroupForm.controls.IsCommonPickup.value && !(this.DelGroupForm.controls.Terminal.valid || this.DelGroupForm.controls.BulkPlant.valid)) {
            isValid = false;
            Declarations.msgerror('', 'Please fill required field', 5000);
            this.idCommonPickUpBtn.nativeElement.click();
        }
        if (this.isOptionalPickup) {
            var delControlReqs = this.DelGroupForm.controls['DeliveryRequests'] as FormArray;
            delControlReqs.controls.forEach((x: FormGroup) => {
                let OrderId = x.get('OrderId').value;
                let ProductType = x.get('ProductType').value;
                let terminalInfo = x.get('Terminal').value;
                let bulkplantInfo = x.get('BulkPlant').value;
                let fuelTypeInfo = this.ScheduleOrderFuelInfo.find(x => x.OrderId == OrderId);
                if (terminalInfo.Id == 0 && (bulkplantInfo.Address == null || bulkplantInfo.Address == '')) {
                    if (fuelTypeInfo != null) {
                        var fuelTypeDetails = fuelTypeInfo.FuelTypeDetails;
                        let result = this.ScheduleOptionalPickupDetailModel.filter(o1 => fuelTypeDetails.some(o2 => o1.TfxFuelTypeId === o2.Id));
                        if (result.length == 0) {
                            Declarations.msgerror('Please assign optional pickup/pickup location for product type : ' + ProductType, 'Pickup Location Required', 2500);
                            isValid = false;
                        }
                    }
                }
            });
        }
        return isValid;
    }

    cancelDeliveryGroupChanges() {
        //this.DelGroupForm = this.UnchangedTrip;
        let data = { shiftIndex: this._shiftIndex, rowIndex: this._rowIndex, tripIndex: this._tripIndex, trip: this.UnchangedTrip }
        this.dataService.setCancelDeliveryGroupSubject(data);
        this.hideDeliveryGroup();
        this.dataService.setGroupChangeDetectSubject(true);
    }

    onDeliveryRequestChange(delReq: FormGroup) {
        if (delReq.controls['TripId'] != null && delReq.controls['TripId'] != undefined) {
            this.onDeliveryGroupChange();
        }
        else {
            let prevStatus = delReq.controls['SchedulePreviousStatus'];
            let status = delReq.controls['ScheduleStatus'];
            let trackScheduleStatus = delReq.controls['TrackScheduleStatus'];
            if (trackScheduleStatus.value != 5) {
                if (prevStatus && prevStatus.value != 0 && prevStatus.value != '') {
                    status.patchValue(15);
                }
                else if (status) {
                    status.patchValue(14);
                }
            }
            else {
                if (trackScheduleStatus.value == 5) {
                    delReq.controls['ScheduleStatus'].setValue(5);
                }
            }
        }
        this.updateBadgeNumberDetails();
        this.hideShowCommonBadgeArea();
    }
    updateBadgeNumberDetails() {
        let commonBadgeCount = 0;
        let commonBadgeCheckStatus = true;
        var deliveryRequests = this.DelGroupForm.controls['DeliveryRequests'] as FormArray;
        if (deliveryRequests != null) {
            for (var i = 0; i < deliveryRequests.length; i++) {
                if (deliveryRequests.controls[i].get('IsCommonBadge').value == false) {
                    commonBadgeCount = commonBadgeCount + 1;
                    commonBadgeCheckStatus = false;
                    break;
                }
            }
        }
        if (!commonBadgeCheckStatus && commonBadgeCount == deliveryRequests.length) {
            this.DelGroupForm.controls['BadgeNo1'].setValue('');
            this.DelGroupForm.controls['BadgeNo2'].setValue('');
            this.DelGroupForm.controls['BadgeNo3'].setValue('');
            //this.DelGroupForm.controls['RouteInfo'].setValue('');
        }
    }
    hideShowCommonBadgeArea() {
        let commonBadgeCheckStatus = false;
        var deliveryRequests = this.DelGroupForm.controls['DeliveryRequests'] as FormArray;
        if (deliveryRequests != null) {
            for (var i = 0; i < deliveryRequests.length; i++) {
                if (deliveryRequests.controls[i].get('IsCommonBadge').value == true) {
                    commonBadgeCheckStatus = true;
                    break;
                }
            }
        }
        this.DelGroupForm.controls['IsCommonBadge'].setValue(commonBadgeCheckStatus);
    }

    onDeliveryGroupChange() {
        var deliveryRequests = this.DelGroupForm.controls['DeliveryRequests'] as FormArray;
        for (var i = 0; i < deliveryRequests.length; i++) {
            this.onDeliveryRequestChange(deliveryRequests.controls[i] as FormGroup);
        }
    }

    private zipcodeConditionallyValidator() {
        this.PickupForm.get('BulkPlant.Country.Code').valueChanges.subscribe((country) => {
            this.PickupForm.get('BulkPlant.ZipCode').clearValidators();
            this.PickupForm.get('BulkPlant.ZipCode').updateValueAndValidity();
            if (country === 'USA') {
                this.PickupForm.get('BulkPlant.ZipCode').setValidators([Validators.required, Validators.pattern('^[0-9]{5}(?:-[0-9]{4})?$')]);
                this.PickupForm.get('BulkPlant.ZipCode').updateValueAndValidity();
                this.CountryBasedZipcodeLabel = "Zip";
            }
            else if (country === 'CAN') {
                this.PickupForm.get('BulkPlant.ZipCode').setValidators([Validators.required, Validators.pattern(/^[A-Za-z]\d[A-Za-z][ ]\d[A-Za-z]\d$/)]);
                this.PickupForm.get('BulkPlant.ZipCode').updateValueAndValidity();
                this.CountryBasedZipcodeLabel = "Postal Code";
            }
            else if (country === 'IND') {
                this.PickupForm.get('BulkPlant.ZipCode').setValidators([Validators.required, Validators.pattern('([ABCEGHJKLMNPRSTVXYabceghjklmnprstvxy][0-9][ABCEGHJKLMNPRSTVWXYZabceghjklmnprstvwxyz]) ?([0-9][ABCEGHJKLMNPRSTVWXYZabceghjklmnprstvwxyz][0-9])')]);
                this.PickupForm.get('BulkPlant.ZipCode').updateValueAndValidity();
            }
            else if (country === 'CAR') {
                this.PickupForm.get('BulkPlant.ZipCode').clearValidators();
                this.PickupForm.get('BulkPlant.ZipCode').updateValueAndValidity();
            }
        });
    }

    private subscribeDeleteDRRequestSubject(): void {
        this.DeleteDRRequestSubject = this.dataService.DeleteDRRequestSubject.subscribe(x => {
            if (x) {
                this.deleteSchedule(x);
                Declarations.closeDG();
            }
        });
        this.DGSubscription.add(this.DeleteDRRequestSubject);
    }

    public subscribeRouteResetGroupSubject(): void {
        this.RouteResetGroupSubject = this.dataService.RouteResetGroupSubject.subscribe(x => {
            if (x) {
                this.hideDeliveryGroup();
                this.setDeletePostloadedDrsSubject(this.DelGroupForm.controls['DeliveryRequests'].value);
                let input = {
                    DelGroupForm: this.DelGroupForm,
                    routeName: x
                }
                this.dataService.setRouteDeleteDeliveryGroupSubject(input);
                this.dataService.setDeliveryPreloadOption({ ShiftIndex: this._shiftIndex, ScheduleIndex: this._rowIndex, Reset: true });
            }
        });
    }
    setDRPickupValidators(form: FormGroup, pickupType: number): void {
        if (!this.isOptionalPickup && !form.get('IsAdditive').value) {
            if (pickupType != 2) {
                form.get('BulkPlant.Address').setValidators(null);
                form.get('BulkPlant.Address').updateValueAndValidity();
                form.get('BulkPlant.City').setValidators(null);
                form.get('BulkPlant.City').updateValueAndValidity();
                form.get('BulkPlant.State.Id').setValidators(null);
                form.get('BulkPlant.State.Id').updateValueAndValidity();
                form.get('BulkPlant.Country.Code').setValidators(null);
                form.get('BulkPlant.Country.Code').updateValueAndValidity();
                form.get('BulkPlant.ZipCode').setValidators(null);
                form.get('BulkPlant.ZipCode').updateValueAndValidity();
                form.get('BulkPlant.CountyName').setValidators(null);
                form.get('BulkPlant.CountyName').updateValueAndValidity();
                form.get('BulkPlant.Latitude').setValidators(null);
                form.get('BulkPlant.Latitude').updateValueAndValidity();
                form.get('BulkPlant.Longitude').setValidators(null);
                form.get('BulkPlant.Longitude').updateValueAndValidity();
                form.get('BulkPlant.SiteName').setValidators(null);
                form.get('BulkPlant.SiteName').updateValueAndValidity();
                form.get('Terminal.Name').setValidators([Validators.required]);
                form.get('Terminal.Name').updateValueAndValidity();
                form.get('Terminal.Id').setValidators([Validators.required]);
                form.get('Terminal.Id').updateValueAndValidity();
            } else {
                form.get('Terminal.Name').setValidators(null);
                form.get('Terminal.Name').updateValueAndValidity();
                form.get('Terminal.Id').setValidators(null);
                form.get('Terminal.Id').updateValueAndValidity();
                form.get('BulkPlant.Address').setValidators([Validators.required]);
                form.get('BulkPlant.Address').updateValueAndValidity();
                form.get('BulkPlant.City').setValidators([Validators.required]);
                form.get('BulkPlant.City').updateValueAndValidity();
                form.get('BulkPlant.State.Id').setValidators([Validators.required]);
                form.get('BulkPlant.State.Id').updateValueAndValidity();
                form.get('BulkPlant.Country.Code').setValidators([Validators.required]);
                form.get('BulkPlant.Country.Code').updateValueAndValidity();
                form.get('BulkPlant.ZipCode').setValidators([Validators.required]);
                form.get('BulkPlant.ZipCode').updateValueAndValidity();
                form.get('BulkPlant.CountyName').setValidators([Validators.required]);
                form.get('BulkPlant.CountyName').updateValueAndValidity();
                form.get('BulkPlant.Latitude').setValidators([Validators.required, Validators.pattern('^[0-9.-]*$')]);
                form.get('BulkPlant.Latitude').updateValueAndValidity();
                form.get('BulkPlant.Longitude').setValidators([Validators.required, Validators.pattern('^[0-9.-]*$')]);
                form.get('BulkPlant.Longitude').updateValueAndValidity();
                form.get('BulkPlant.SiteName').setValidators([Validators.required]);
                form.get('BulkPlant.SiteName').updateValueAndValidity();
            }
        }
        else {
            form.get('BulkPlant.Address').setValidators(null);
            form.get('BulkPlant.Address').updateValueAndValidity();
            form.get('BulkPlant.City').setValidators(null);
            form.get('BulkPlant.City').updateValueAndValidity();
            form.get('BulkPlant.State.Id').setValidators(null);
            form.get('BulkPlant.State.Id').updateValueAndValidity();
            form.get('BulkPlant.Country.Code').setValidators(null);
            form.get('BulkPlant.Country.Code').updateValueAndValidity();
            form.get('BulkPlant.ZipCode').setValidators(null);
            form.get('BulkPlant.ZipCode').updateValueAndValidity();
            form.get('BulkPlant.CountyName').setValidators(null);
            form.get('BulkPlant.CountyName').updateValueAndValidity();
            form.get('BulkPlant.Latitude').setValidators(null);
            form.get('BulkPlant.Latitude').updateValueAndValidity();
            form.get('BulkPlant.Longitude').setValidators(null);
            form.get('BulkPlant.Longitude').updateValueAndValidity();
            form.get('BulkPlant.SiteName').setValidators(null);
            form.get('BulkPlant.SiteName').updateValueAndValidity();
            form.get('Terminal.Name').setValidators(null);
            form.get('Terminal.Name').updateValueAndValidity();
            form.get('Terminal.Id').setValidators(null);
            form.get('Terminal.Id').updateValueAndValidity();
        }
    }
    validateOptionalPickup() {
        if (this.isOptionalPickup) {
            this.DelGroupForm.controls['Terminal'].setValidators(null);
            this.DelGroupForm.controls['Terminal'].updateValueAndValidity();
            this.DelGroupForm.controls['BulkPlant'].setValidators(null);
            this.DelGroupForm.controls['BulkPlant'].updateValueAndValidity();
            this.changeDetectorRef.markForCheck();
            this.DelGroupForm.markAllAsTouched();
        }

    }
    //TBD DR - CREATE ON THE FLY LOCATION
    getLocationDetailsSubscriptions: Subscription = new Subscription();
    createOnTheFlyLocation(tdr: FormGroup) {
        var _this = this;
        //OPEN PANEL
        this.dataService.setOpenOnTheFlyLocationFormSubject(tdr.value);
        //UN-SUBSCRIBE IF ALREADY SUBSCRIBED
        this.getLocationDetailsSubscriptions ? this.getLocationDetailsSubscriptions.unsubscribe() : null;
        //GET LOCATION DETAILS
        this.getLocationDetailsSubscriptions = this.dataService.getOnTheFlyLocationDetailsSubject.pipe(first()).subscribe((response: any) => {
            //SET DETAILS
            _this.DelGroupForm.controls['DeliveryRequests']['controls'].forEach((drForm: FormGroup) => {
                if (drForm.controls['Id'].value == tdr.controls['Id'].value) {
                    drForm.get('JobId').setValue(response.ResponseData.JobId);
                    drForm.get('JobName').setValue(response.ResponseData.JobName);
                    drForm.get('OrderId').setValue(response.ResponseData.OrderId);
                    drForm.get('JobAddress').setValue(response.ResponseData.Address);
                    drForm.get('JobCity').setValue(response.ResponseData.City)
                    drForm.get('UoM').setValue(response.ResponseData.UoM);
                    drForm.get('CustomerCompany').setValue(response.ResponseData.CustomerCompanyName);
                    drForm.get('ScheduleStartTime').setValue(response.ResponseData.ScheduleStartTime);
                    drForm.get('ScheduleEndTime').setValue(response.ResponseData.ScheduleEndTime);
                }
            });
        })
    }
    DoNotShowCancelButton(dsValue: DeliveryRequestViewModel) {
        let cancelDRButtonDisplayStatus = false;
        if (this.EnrouteInProgress.indexOf(dsValue.TrackScheduleEnrouteStatus) >= 0 || this.EnrouteInCompleted.indexOf(dsValue.TrackScheduleStatus) >= 0) {
            cancelDRButtonDisplayStatus = true;
        }
        return cancelDRButtonDisplayStatus;
    }

    onSubDrCreate() {
        if (this.addSubDrModel.IsSpiltDRAdded) {
            var filter = this.addSubDrModel.SpiltDRs.filter(x => !(x.RequiredQuantity > 0)).length;
            if (filter > 0) {
                Declarations.msgerror("Quantity required for Sub DR(s).", undefined, undefined); return;
            }
        }
        var dismiss = document.getElementById('closeSubDrPanel') as HTMLElement;
        dismiss.click();
        this.dataService.setScheduleLoaderSubject(true);
        this.carrierService.addSubDrs(this.addSubDrModel)
            .subscribe((data: any) => {
                this.dataService.setScheduleLoaderSubject(false);
                if (data.StatusCode == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                    this.onRaiseSubDR.emit();
                }
                else if (data.StatusCode == 2) {
                    Declarations.msgwarning(data.StatusMessage, undefined, undefined);
                    this.onRaiseSubDR.emit();
                }
                else {
                    Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                }
            });

    }

    public AddSubDR(tdr: FormGroup) {
        this.addSubDrModel = tdr.value as DeliveryRequestViewModel;
        this.addSubDrModel.IsSpiltDRAdded = false;
        this.addSubDrModel.SpiltDRs = [];
        this.addEmptySubDR();
    }

    addEmptySubDR() {
        if (this.addSubDrModel != null) {
            this.addSubDrModel.IsSpiltDRAdded = true;
            let spiltDRModel = new SpiltDRsModel();
            spiltDRModel.ScheduleQuantityType = 1;
            this.addSubDrModel.SpiltDRs.push(spiltDRModel);
        }
    }

    removeItem(j: number) {
        this.addSubDrModel.SpiltDRs.splice(j, 1);
    }

    CheckSubDrStatus(dsValue: DeliveryRequestViewModel) {

        if (this.SubDrsStatusWithParentIds.length > 0 && dsValue.GroupParentDRId) {
            let DRs = this.SubDrsStatusWithParentIds.filter(x => x.GroupParentDRId == dsValue.GroupParentDRId);
            if (DRs.length > 0) {
                let NonCancelledSchedules = this.sbService.returnSubDrStatusOtherthaCancel(this.SubDrOtherThanCancellStatus, DRs);
                if (NonCancelledSchedules <= 0) {
                    return false;
                }
            }
        }
        return true;
    }

    public CancelDS(tdr: FormGroup) {
        this.CancelDSViewModel = tdr.value as DeliveryRequestViewModel;
        if (this.CancelDSViewModel.GroupParentDRId != '' && this.CancelDSViewModel.GroupParentDRId != null) {
            this.cancelDSDeliveryScheduleViewModel = [];
            this.dataService.setScheduleLoaderSubject(true);
            this.hideDeliveryGroup();
            //intialize the model.
            let cancelDSDeliveryScheduleInfo = new CancelDSDeliveryScheduleInfo();
            cancelDSDeliveryScheduleInfo.RegionId = this.SelectedRegionId;
            cancelDSDeliveryScheduleInfo.CancelDSDeliverySchedules = [];

            //normal DR
            let cancelDSDeliverySchedule = new CancelDSDeliverySchedule();
            cancelDSDeliverySchedule.DeliveryReqId = this.CancelDSViewModel.Id;
            cancelDSDeliverySchedule.IsSubDR = false;
            cancelDSDeliveryScheduleInfo.CancelDSDeliverySchedules.push(cancelDSDeliverySchedule);
            //sub DR
            let cancelsubDSDeliverySchedule = new CancelDSDeliverySchedule();
            cancelsubDSDeliverySchedule.DeliveryReqId = this.CancelDSViewModel.GroupParentDRId;
            cancelsubDSDeliverySchedule.IsSubDR = true;
            cancelDSDeliveryScheduleInfo.CancelDSDeliverySchedules.push(cancelsubDSDeliverySchedule);
            this.sbService.GetSubDRInfoCancelDS(cancelDSDeliveryScheduleInfo).subscribe(response => {
                this.dataService.setScheduleLoaderSubject(false);
                if (response != null && response.length > 0) {
                    $("#btn-group-cancel-ds-info").click();
                    this.cancelDSDeliveryScheduleViewModel = response as CancelDSDeliveryScheduleViewModel[];
                }
            });
            this.changeDetectorRef.markForCheck();
            this.dataService.setGroupChangeDetectSubject(true);
        }
        else {
            $("#btn-group-cancel-ds").click();
        }
    }
    public RejectDSGroupDriverSchedule() {
        this.DeliveryReqCancelScheduleUpdateModel = [];
        if (this.Schedule != null && this.CancelDSViewModel != null) {
            let delRequestCancelModel = new CancelDeliverySchedule();
            delRequestCancelModel.ScheduleBuilderId = this.ScheduleBuilderId;
            delRequestCancelModel.DeliveryReqId = this.CancelDSViewModel.Id;
            delRequestCancelModel.DriverColIndex = this.DelGroupForm.value.DriverColIndex;
            if (this.Schedule.value != null && this.Schedule.value.Drivers.length > 0) {
                delRequestCancelModel.DriverId = this.Schedule.value.Drivers[0].Id;
            }
            delRequestCancelModel.DriverRowIndex = this.DelGroupForm.value.DriverRowIndex;
            delRequestCancelModel.ShiftId = this.DelGroupForm.value.ShiftId;
            delRequestCancelModel.ShiftIndex = this.DelGroupForm.value.ShiftIndex;
            delRequestCancelModel.TrackableScheduleId = this.CancelDSViewModel.TrackableScheduleId;
            this.DeliveryReqCancelScheduleUpdateModel.push(delRequestCancelModel);
            //post load DS
            if (this.CancelDSViewModel.PostLoadInfo != null) {
                let postLoaddelRequestCancelModel = new CancelDeliverySchedule();
                postLoaddelRequestCancelModel.ScheduleBuilderId = this.ScheduleBuilderId;
                postLoaddelRequestCancelModel.DeliveryReqId = this.CancelDSViewModel.PostLoadInfo.DrId;
                postLoaddelRequestCancelModel.DriverColIndex = this.CancelDSViewModel.PostLoadInfo.TripIndex;
                postLoaddelRequestCancelModel.DriverId = 0;
                postLoaddelRequestCancelModel.DriverRowIndex = this.CancelDSViewModel.PostLoadInfo.ScheduleIndex;
                postLoaddelRequestCancelModel.ShiftId = this.CancelDSViewModel.PostLoadInfo.ShiftId;
                postLoaddelRequestCancelModel.ShiftIndex = this.CancelDSViewModel.PostLoadInfo.ShiftIndex;
                postLoaddelRequestCancelModel.TrackableScheduleId = -1;
                this.DeliveryReqCancelScheduleUpdateModel.push(postLoaddelRequestCancelModel);
            }

            //blended DR
            if (this.CancelDSViewModel.IsBlendedRequest) {
                let tripDrs = this.filterAndGetDrsByBlendGroupId(<DeliveryRequestViewModel[]>this.DelGroupForm.get('DeliveryRequests').value, this.CancelDSViewModel.BlendedGroupId);
                tripDrs.forEach(dr => {
                    if (dr.Id != this.CancelDSViewModel.Id) {//this dr already added
                        let delRequestCancelModel = new CancelDeliverySchedule();
                        delRequestCancelModel.ScheduleBuilderId = this.ScheduleBuilderId;
                        delRequestCancelModel.DeliveryReqId = dr.Id;
                        delRequestCancelModel.DriverColIndex = this.DelGroupForm.value.DriverColIndex;
                        if (this.Schedule.value != null && this.Schedule.value.Drivers.length > 0) {
                            delRequestCancelModel.DriverId = this.Schedule.value.Drivers[0].Id;
                        }
                        delRequestCancelModel.DriverRowIndex = this.DelGroupForm.value.DriverRowIndex;
                        delRequestCancelModel.ShiftId = this.DelGroupForm.value.ShiftId;
                        delRequestCancelModel.ShiftIndex = this.DelGroupForm.value.ShiftIndex;
                        delRequestCancelModel.TrackableScheduleId = dr.TrackableScheduleId;
                        this.DeliveryReqCancelScheduleUpdateModel.push(delRequestCancelModel);
                    }
                });
            }

            this.dataService.setScheduleLoaderSubject(true);
            this.sbService.CancelDeliveryGroupSchedule(this.DeliveryReqCancelScheduleUpdateModel).subscribe(response => {
                if (response != null && response != undefined && response.length > 0) {
                    this.dataService.setCancelDSGroupNormalSubDSSubject(response);
                    this.dataService.setScheduleLoaderSubject(false);
                    this.hideDeliveryGroup();
                    this.changeDetectorRef.markForCheck();
                    this.dataService.setGroupChangeDetectSubject(true);
                    Declarations.msgsuccess("Delivery schedule cancelled successfully.", 'Success', 2500);
                }
                else {
                    this.dataService.setScheduleLoaderSubject(false);
                }
            });
        }
    }
    public CancelDSGroupDriverSchedule() {
        this.DeliveryReqCancelScheduleUpdateModel = [];
        this.CancelDSViewModel = null;
    }
    public CancelSelectedDS() {
        this.DeliveryReqCancelScheduleUpdateModel = [];
        if (this.cancelDSDeliveryScheduleViewModel.filter(x => x.IsChecked == true).length > 0) {
            let cancelDSfinalInfo = this.cancelDSDeliveryScheduleViewModel.filter(x => x.IsChecked == true);
            cancelDSfinalInfo.forEach(x => {
                let delRequestCancelModel = new CancelDeliverySchedule();
                delRequestCancelModel.ScheduleBuilderId = x.ScheduleBuilderId;
                delRequestCancelModel.DeliveryReqId = x.DeliveryReqId;
                delRequestCancelModel.DriverColIndex = x.DriverColIndex;
                delRequestCancelModel.DriverId = x.DriverId;
                delRequestCancelModel.DriverRowIndex = x.DriverRowIndex;
                delRequestCancelModel.ShiftId = x.ShiftId;
                delRequestCancelModel.ShiftIndex = x.ShiftIndex;
                delRequestCancelModel.TrackableScheduleId = x.TrackableScheduleId;
                this.DeliveryReqCancelScheduleUpdateModel.push(delRequestCancelModel);
            });
            if (this.DeliveryReqCancelScheduleUpdateModel.length > 0) {
                $("#confirm-group-cancel-ds-dismiss").click();
                this.dataService.setScheduleLoaderSubject(true);
                this.sbService.CancelDeliveryGroupSchedule(this.DeliveryReqCancelScheduleUpdateModel).subscribe(response => {
                    if (response != null && response != undefined && response.length > 0) {
                        this.dataService.setCancelDSGroupNormalSubDSSubject(response);
                        this.dataService.setScheduleLoaderSubject(false);
                        this.hideDeliveryGroup();
                        this.changeDetectorRef.markForCheck();
                        this.dataService.setGroupChangeDetectSubject(true);
                        Declarations.msgsuccess("Delivery schedule cancelled successfully.", 'Success', 2500);
                    }
                    else {
                        this.dataService.setScheduleLoaderSubject(false);
                    }
                });
            }
        }
        else {
            Declarations.msgerror("Select at least one DS for cancel schedule.", 'Success', 2500);
        }
    }
    public checkAll() {
        if ($("#selectAllDSBGroupCancel").is(":checked")) {
            this.cancelDSDeliveryScheduleViewModel.forEach(x => {
                x.IsChecked = true;
            });
            this.changeDetectorRef.markForCheck();
        } else {
            this.cancelDSDeliveryScheduleViewModel.forEach(x => {
                x.IsChecked = false;
            });
            this.changeDetectorRef.markForCheck();
        }
    }
    public checkDSChange(ele: number, ds: CancelDSDeliveryScheduleViewModel) {
        var eleId = "dsCheck" + ele;
        var eleIdDom = $("#" + eleId).is(":checked");
        if (eleIdDom) {
            var deliveryReq = this.cancelDSDeliveryScheduleViewModel.find(x1 => x1.DeliveryReqId == ds.DeliveryReqId);
            if (deliveryReq != null) {
                deliveryReq.IsChecked = true;
            }
            this.changeDetectorRef.markForCheck();
        } else {
            var deliveryReq = this.cancelDSDeliveryScheduleViewModel.find(x1 => x1.DeliveryReqId == ds.DeliveryReqId);
            if (deliveryReq != null) {
                deliveryReq.IsChecked = false;
            }
            this.changeDetectorRef.markForCheck();
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
        let delGroupform = this.DelGroupForm;
        let pickUpForm = this.PickupForm;
        //del group
        delGroupform.get('BulkPlant.Address').setValidators(!isCarribean ? [Validators.required] : null);
        delGroupform.get('BulkPlant.Address').updateValueAndValidity();
        delGroupform.get('BulkPlant.City').setValidators(!isCarribean ? [Validators.required] : null);
        delGroupform.get('BulkPlant.City').updateValueAndValidity();
        delGroupform.get('BulkPlant.ZipCode').setValidators(!isCarribean ? [Validators.required] : null);
        delGroupform.get('BulkPlant.ZipCode').updateValueAndValidity();
        delGroupform.get('BulkPlant.CountyName').setValidators(!isCarribean ? [Validators.required] : null);
        delGroupform.get('BulkPlant.CountyName').updateValueAndValidity();
        delGroupform.get('BulkPlant.Latitude').setValidators(!isCarribean ? [Validators.required, Validators.pattern('^[0-9.-]*$')] : null);
        delGroupform.get('BulkPlant.Latitude').updateValueAndValidity();
        delGroupform.get('BulkPlant.Longitude').setValidators(!isCarribean ? [Validators.required, Validators.pattern('^[0-9.-]*$')] : null);
        delGroupform.get('BulkPlant.Longitude').updateValueAndValidity();

        //pick up form 
        pickUpForm.get('BulkPlant.Address').setValidators(!isCarribean ? [Validators.required] : null);
        pickUpForm.get('BulkPlant.Address').updateValueAndValidity();
        pickUpForm.get('BulkPlant.City').setValidators(!isCarribean ? [Validators.required] : null);
        pickUpForm.get('BulkPlant.City').updateValueAndValidity();
        pickUpForm.get('BulkPlant.ZipCode').setValidators(!isCarribean ? [Validators.required] : null);
        pickUpForm.get('BulkPlant.ZipCode').updateValueAndValidity();
        pickUpForm.get('BulkPlant.CountyName').setValidators(!isCarribean ? [Validators.required] : null);
        pickUpForm.get('BulkPlant.CountyName').updateValueAndValidity();
        pickUpForm.get('BulkPlant.Latitude').setValidators(!isCarribean ? [Validators.required, Validators.pattern('^[0-9.-]*$')] : null);
        pickUpForm.get('BulkPlant.Latitude').updateValueAndValidity();
        pickUpForm.get('BulkPlant.Longitude').setValidators(!isCarribean ? [Validators.required, Validators.pattern('^[0-9.-]*$')] : null);
        pickUpForm.get('BulkPlant.Longitude').updateValueAndValidity();
    }

    //BLENDED REQUEST
    public blendRequestForm: FormGroup = this.fb.group({ BlendedRequests: this.fb.array([]) });
    public additiveOrders: AdditiveOrderViewModel[] = [];
    public CompatibleProductTypeList: CompatibleProductModel[] = [];
    public DrForEditBlendedRequestIndex: number = 0;
    public BlendDrIndex: number = 0;
    public IsPickupForBlendRequest: boolean = false;
    public totalBlendedQuantity: number = 0;
    public tankMaxFill: number = 0;
    public getOrderSubject: Subject<any> = new Subject<any>();
    public deletedBlend_temp: DeliveryRequestViewModel[] = [];
    public deletedBlend_all: DeliveryRequestViewModel[] = [];

    editBlendedSchedule(index: number, trip: FormGroup) {

        this.deletedBlend_temp = [];
        this._currentTrip = trip;
        this.DeletedDrIndex = index;
        this.DrForEditBlendedRequestIndex = index;

        let tripDrs = trip.get('DeliveryRequests').value as DeliveryRequestViewModel[];

        this.selectedDeliveryRequest = tripDrs[this.DeletedDrIndex];
        this.selectedDeliveryRequest.WindowMode = 1;
        this.selectedDeliveryRequest.QueueMode = 1;

        let blendRequests = <FormArray>this.blendRequestForm.get('BlendedRequests');
        blendRequests.clear();

        let blendeDdrGroup = this.filterAndGetDrsByBlendGroupId(tripDrs, this.selectedDeliveryRequest.BlendedGroupId);
        this.totalBlendedQuantity = <number>blendeDdrGroup.find(dr => dr.IsBlendedDrParent).TotalBlendedQuantity;
        let additives = blendeDdrGroup.filter(dr => dr.IsAdditive);
        if (additives && additives.length > 0) {
            this.totalBlendedQuantity = this.totalBlendedQuantity - additives.reduce((a, b) => +a + +b.RequiredQuantity, 0);
        }

        blendeDdrGroup.forEach(dr => {
            dr.QuantityInPercent = ((+dr.RequiredQuantity / +this.totalBlendedQuantity) * 100);
            blendRequests.push(this.utilService.getBlendRequestFormGroup(dr));
        })

        //max fill validation
        let request = blendeDdrGroup[0];
        if (request.ScheduleQuantityType == 1 && request.TankMaxFill > 0 && !request.IsMaxFillAllowed) {
            this.tankMaxFill = request.TankMaxFill;
        } else {
            this.tankMaxFill = 0;
        }
        document.getElementById('_openBlendModalButton').click();
    }

    getOrdersByDr(dr: DeliveryRequestViewModel): any[] {

        let response: any[] = [];

        let _jobId = dr.JobId || '';
        let _productTypeId;
        if (dr.BlendParentProductTypeId > 0)
            _productTypeId = dr.BlendParentProductTypeId;
        else
            _productTypeId = dr.ProductTypeId || '';
        let _carrierStatus = dr.CarrierStatus || '';

        let existing = [];

        if (_productTypeId) {
            existing = this.OrderList[_jobId.toString() + '_' + _productTypeId.toString() + '_' + (dr.IsBlendedRequest ? 1 : 0)];
        }
        else {
            existing = this.OrderList[_jobId.toString()];
        }

        if (existing == undefined || existing == null || existing.length == 0) {
            this.getOrderSubject.next({ _jobId: _jobId.toString(), _productTypeId: _productTypeId.toString(), _carrierStatus: _carrierStatus.toString(), _isBlend: dr.IsBlendedRequest });
        }
        else if (existing.length > 0) {
            response = existing;
        }

        return response;
    }

    subscribeGetOrderSubject() {
        this.getOrderSubject.pipe(debounceTime(1000), distinctUntilChanged((a, b) => JSON.stringify(a) === JSON.stringify(b))).subscribe((x: any) => {
            this.sbService.getOrders(x._jobId, x._productTypeId ? x._productTypeId.toString() : null, null, x._carrierStatus, x._isBlend).subscribe(data => {
                this.OrderList[x._jobId.toString() + '_' + x._productTypeId.toString() + '_' + (x._isBlend ? 1 : 0)] = data;
                //location all orders
                this.OrderList[x._jobId.toString()] = data;
            });
        })
    }

    getUnUsedOrdersForBlendedRequest(blendedRequest: FormGroup) {

        let currentOrder = blendedRequest.get('OrderId').value;
        let orderList: any[] = this.getOrdersByDr(blendedRequest.value);

        if (orderList && orderList.length > 0) {
            let ordersToRemove = this.blendRequestForm.get('BlendedRequests').value as DeliveryRequestViewModel[];//orders selected in blend form
            ordersToRemove = ordersToRemove.filter(obj => obj.OrderId != currentOrder);
            let productToRemove = orderList.filter(s => ordersToRemove.find(s2 => s2.OrderId == s.OrderId));
            return orderList.filter((x => currentOrder == x.OrderId || !productToRemove.some(y => y.FuelTypeId == x.FuelTypeId)));
        } else {
            return [];
        }
    };

    getUnUsedBlendOrders(currentOrder: number) {

        let blendRequests = this.blendRequestForm.get('BlendedRequests').value as BlendedRequest[];//orders selected in blend form
        blendRequests = blendRequests.filter(b => b.IsAdditive && b.OrderId && b.OrderId != currentOrder);
        let ordersToRemove = blendRequests.map(b => b.OrderId);

        return this.additiveOrders.filter((x => currentOrder == x.Id || !ordersToRemove.some(orderId => orderId == x.Id)));
    };

    orderChangedForBlendRequest(_orderId: string, blendedRequest: FormGroup): void {

        blendedRequest.get('ProductTypeId').setValue(null);
        let orders = this.getOrdersByDr(blendedRequest.value);
        if (orders.length > 0) {
            let selectedOrder = orders.find(x => x.OrderId == _orderId);
            if (selectedOrder) {
                let location = OrderPickupLocationModel.ToLocation(selectedOrder);
                this.setPickupLocationForBlendRequest(blendedRequest, location);

                if (selectedOrder.ProductTypeId) {
                    blendedRequest.get('ProductTypeId').setValue(selectedOrder.ProductTypeId);
                }
            }
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
        } else {
            if (order.BulkPlant) {
                if (order.BulkPlant.ZipCode) {
                    order.BulkPlant.ZipCode = order.BulkPlant.ZipCode.toUpperCase();
                }
                delReq.controls['BulkPlant'].patchValue(order.BulkPlant);
            }
        }
    }

    RemoveBlendPickupLocation(tdr: FormGroup): void {

        if (tdr) {
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
            this.setPickupLocation(this.SelectedDRForEditPickup, location);
        }
    }

    getRequiredDataForPickupDetails() {
        if (!this.StateList || this.StateList.length == 0) {
            this.stateService.getStates().subscribe(x => this.StateList = x);
        }
        if (!this.CountryList || this.CountryList.length == 0) {
            this.stateService.getCountries().subscribe(x => this.CountryList = x);
        }
        if (!this.CountryGroupList || this.CountryGroupList.length == 0) {
            this.stateService.getCountryGroup(4).subscribe(x => this.CountryGroupList = x);
        }
    }

    editPickupLocationForBlend(blendDrIndex: number): void {

        this.BlendDrIndex = blendDrIndex;
        let blendDr = this.blendRequestForm.get('BlendedRequests')['controls'][this.BlendDrIndex];

        this.getRequiredDataForPickupDetails();

        this.ChangePickupForOrders = [];
        this.SelectedDRForEditPickup = blendDr;


        let _orderId = blendDr.controls['OrderId'].value as number;
        if (_orderId > 0) {
            this.ChangePickupForOrders.push(_orderId);
            let existingOrders = this.OrderList[blendDr.value.JobId.toString() + '_' + blendDr.value.ProductTypeId.toString() + '_' + (blendDr.value.IsBlendedRequest ? 1 : 0)];
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

        this.setPickupLocation(this.SelectedDRForEditPickup, location);

        //to get bulk plant for auto/manual order
        this.addresService.getBulkPlants('', _orderId > 0 ? _orderId : 0).subscribe(data => {
            this.BulkPlants = data.slice();
            this.BulkplantList = data;

            let bulkPlantName = this.PickupForm.get('BulkPlant.SiteName').value;
            this.isReadOnly = this.BulkPlants.some(t => t.Name == bulkPlantName);
        });
        this.DGSubscription.add(this.sbService.getPickupTerminals(this.ChangePickupForOrders, '').subscribe((data: DropdownItem[]) => {
            this.Terminals = data;
        }));


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

        if (this.SelectedDRForEditPickup != null && blendDr != null) {
            this.setPickupLocation(blendDr, this.PickupForm.value);
        }

        this.IsPickupForBlendRequest = false;
        let dismiss = document.getElementById('btnPickupClose') as HTMLElement;
        dismiss.click();
    }

    addRemoveBlendedProduct(addNewRow: boolean, index: number, isAdditive: boolean = false) {

        let _blendedRequests = <FormArray>this.blendRequestForm.get('BlendedRequests');

        if (addNewRow) {
            //set details to new dr from parent dr
            let additiveCount = this.getAdditiveCountBlend();

            let _model = new BlendedRequest(isAdditive);
            _model.UoM = this.selectedDeliveryRequest.UoM;
            _model.BlendedGroupId = this.selectedDeliveryRequest.BlendedGroupId;
            _model.JobId = this.selectedDeliveryRequest.JobId;
            _model.ProductTypeId = null;
            _model.IsBlendedRequest = true;
            _model.OrderId = null;
            _model.BlendParentProductTypeId = this.selectedDeliveryRequest.BlendParentProductTypeId;
            if (!isAdditive && additiveCount > 0) {
                _blendedRequests.insert(+_blendedRequests.controls.length - additiveCount, this.utilService.getBlendRequestFormGroup(_model));
            } else {
                _blendedRequests.push(this.utilService.getBlendRequestFormGroup(_model));
            }
        }
        else {
            let dr = <DeliveryRequestViewModel>_blendedRequests.at(index).value;
            //add to array if its existing dr
            if (dr.Id) {
                dr.Compartments = []; dr.IsDeleted = true;
                this.deletedBlend_temp.push(dr);
                this.deletedBlend_all.push(dr);
            }
            _blendedRequests.removeAt(index);
        }
    }

    blendDrQuantityChanged(enteredQuantity: number, blendIndex: number) {
        let blendedRequest = <FormGroup>this.blendRequestForm.get('BlendedRequests')['controls'][blendIndex];
        blendedRequest.get('QuantityInPercent').setValue((+enteredQuantity / this.totalBlendedQuantity) * 100);
    }

    onBlendTotalQuantityChange(totalQuantity: number) {

        if (totalQuantity) {

            let blendedRequests = <FormArray>this.blendRequestForm.get('BlendedRequests');

            blendedRequests.controls.forEach((b: FormGroup) => {
                if (!b.get('IsAdditive').value && +b.get('QuantityInPercent').value > 0) {
                    b.get('RequiredQuantity').setValue((+b.get('QuantityInPercent').value / 100) * totalQuantity);
                    this.onDeliveryRequestChange(b);
                }
            })
        }
        else {
            this.clearQuantityAndPercentage();
        }
    }

    addRemoveAdditiveProduct(isAddNew: boolean) {

        let _blendedRequests = <FormArray>this.blendRequestForm.get('BlendedRequests');

        if (isAddNew) {
            _blendedRequests.push(this.utilService.getBlendRequestFormGroup(new BlendedRequest(true)));
        }
        else {
            _blendedRequests.removeAt(_blendedRequests.value.findIndex(r => r.IsAdditive));
        }
    }


    getAdditiveCountBlend() {
        let _blendedRequests = <DeliveryRequestViewModel[]>this.blendRequestForm.get('BlendedRequests').value;
        return _blendedRequests.filter(b => b.IsAdditive).length;
    }

    blendDrPercentChanged(percent: number, blendIndex: number) {
        let blendedRequest = <FormGroup>this.blendRequestForm.get('BlendedRequests')['controls'][blendIndex];
        blendedRequest.get('RequiredQuantity').setValue((+percent / 100) * this.totalBlendedQuantity);
        this.onDeliveryRequestChange(blendedRequest);
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

            return (totalQuantity == this.totalBlendedQuantity) || (Math.abs(this.totalBlendedQuantity - totalQuantity) < 1);
        }
        return false;
    }

    isTankMaxFillValid(): boolean {
        if (this.tankMaxFill > 0 && this.totalBlendedQuantity > this.tankMaxFill)
            return false;
        else
            return true;
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

    submitBlendedForm() {

        if (this.blendRequestForm.valid) {

            ////////////VALIDATE///////////
            let scheduleIds = this.getSchedulesFromDr(this.deletedBlend_temp);

            if (scheduleIds && scheduleIds.length > 0) {

                let scheduleUpdatwAllowded = true;
                this.dataService.setScheduleLoaderSubject(true);
                this.sbService.getScheduleStatus(scheduleIds).subscribe(response => {
                    this.dataService.setScheduleLoaderSubject(false);
                    if (response != null && response != undefined && response.length > 0) {
                        let data = response[0];
                        if (this.CompletedScheduleStatuses.indexOf(data.ScheduleStatusId) !== -1 || data.ScheduleEnrouteStatusId == 4) {
                            Declarations.msgerror("Can't update as drop has been made already", 'Warning', 2500);
                            scheduleUpdatwAllowded = false;
                        }
                    }
                    if (scheduleUpdatwAllowded) {
                        this.updateDeliveryGroupBlendByPopup();
                    }
                });
            }
            ////////////NO DELETE///////////
            else {
                this.updateDeliveryGroupBlendByPopup();
            }
        }
    }

    updateDeliveryGroupBlendByPopup() {
        let blendedrequests = this.blendRequestForm.get('BlendedRequests').value as DeliveryRequestViewModel[];

        // 1.DELETE 2.ADD 3.UPDATE
        if (this.deletedBlend_temp.length > 0 && blendedrequests.some(dr => !dr.Id)) {
            //remove dr from ui //removed dr get deleted from api
            //create new dr api
            //create new dr ui (load or dr group)
            //update existing drs
            this.createBlendRequests(blendedrequests.filter(dr => !dr.Id), TaskId.Add_Delete_Update);

        }
        // 1.ADD 2.UPDATE
        else if (this.deletedBlend_temp.length == 0 && blendedrequests.some(dr => !dr.Id)) {
            //create new dr api
            //create new dr ui (load or dr group)
            //update existing drs
            this.createBlendRequests(blendedrequests.filter(dr => !dr.Id), TaskId.Add_Update);

        }
        // 1.DELETE 2.UPDATE
        else if (this.deletedBlend_temp.length > 0 && blendedrequests.every(dr => dr.Id)) {
            //remove dr from ui //removed dr get deleted from api
            //update existing drs
            this.deleteBlendedDeliveryRequestFromLoad();
            this.updateBlendRequests();
            this.setParentDrAndItsQuantity();
        }
        // 1.UPDATE
        else if (this.deletedBlend_temp.length == 0 && blendedrequests.every(dr => dr.Id)) {
            this.updateBlendRequests();
            this.setParentDrAndItsQuantity();
        }
        else {
            Declarations.msgerror("Failed to update.", 'Error', 2500);
        }
    }

    createBlendRequests(deliveryRequests: DeliveryRequestViewModel[], taskId: TaskId) {

        this.dataService.setScheduleLoaderSubject(true);

        this.selectedDeliveryRequest['BlendedRequests'] = deliveryRequests;

        this.carrierService.postRaiseRequests({ DeliveryRequests: [this.selectedDeliveryRequest] }).subscribe((data: any) => {

            this.selectedDeliveryRequest['BlendedRequests'] = [];

            this.dataService.setScheduleLoaderSubject(false);

            if (data != null && parseInt(data.StatusCode) == 0 && data.DeliveryRequests.length > 0) {
                this.pushNewBlendRequestsToLoad(<any[]>data.DeliveryRequests);

                if (taskId == TaskId.Add_Delete_Update) {
                    this.deleteBlendedDeliveryRequestFromLoad();
                }

                this.updateBlendRequests();
                this.setParentDrAndItsQuantity();
            }
            else {
                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
        })
    }

    pushNewBlendRequestsToLoad(newBlendRequests: any[]) {

        let loadDrFormArray = <FormArray>this.DelGroupForm.get('DeliveryRequests');
        newBlendRequests.sort((a, b) => Number(!a.IsAdditive) - Number(!b.IsAdditive));

        let blendGroupEndIndex = null;

        loadDrFormArray.controls.forEach((dr: FormGroup, index: number) => {
            if (dr.get('BlendedGroupId').value && dr.get('BlendedGroupId').value == this.selectedDeliveryRequest.BlendedGroupId) {
                if (!dr.get('IsAdditive').value) {
                    blendGroupEndIndex = index;
                }
            }
        });

        blendGroupEndIndex == null ? blendGroupEndIndex = loadDrFormArray.length : blendGroupEndIndex = blendGroupEndIndex;

        newBlendRequests.forEach(newRequest => {
            loadDrFormArray.insert((+blendGroupEndIndex + 1), this.utilService.getDeliveryRequestForm(newRequest, this.DelGroupForm.controls['IsCommonPickup'].value))
        });

    }

    setParentDrAndItsQuantity() {

        let loadDrFormArray = <FormArray>this.DelGroupForm.get('DeliveryRequests')
        let _blendLoadDrs = loadDrFormArray.value.filter(l => l.BlendedGroupId && l.BlendedGroupId == this.selectedDeliveryRequest.BlendedGroupId);

        loadDrFormArray.controls.forEach((dr: FormGroup, index: number) => {
            if (dr.get('BlendedGroupId').value && dr.get('BlendedGroupId').value == this.selectedDeliveryRequest.BlendedGroupId) {
                if (!dr.get('IsAdditive').value && index == 0) {
                    dr.get('IsBlendedDrParent').setValue(true);
                    dr.get('TotalBlendedQuantity').setValue(_blendLoadDrs.reduce(function (sum, current) { return sum + current.RequiredQuantity; }, 0));
                }
            }
        });
    }

    getSchedulesFromDr(drs: DeliveryRequestViewModel[]): number[] {
        let scheduleIds: number[] = [];
        drs.forEach(dr => {
            if (dr.TrackableScheduleId != null && dr.TrackableScheduleId > 0) {
                scheduleIds.push(dr.TrackableScheduleId);
            }
        });
        return scheduleIds;
    }

    updateBlendRequests() {

        let blendedrequests = this.blendRequestForm.get('BlendedRequests').value as BlendedRequest[];

        this._currentTrip.get('DeliveryRequests')['controls'].forEach((dr: FormGroup) => {
            if (dr.get('BlendedGroupId').value && dr.get('BlendedGroupId').value == this.selectedDeliveryRequest.BlendedGroupId) {

                let _blend = blendedrequests.find(bl => bl.Id == dr.get('Id').value);

                if (_blend) {
                    dr.get('OrderId').setValue(_blend.OrderId);
                    dr.get('SchedulePreviousStatus').setValue(_blend.SchedulePreviousStatus);
                    dr.get('ScheduleStatus').setValue(_blend.ScheduleStatus);
                    dr.get('TrackScheduleStatus').setValue(_blend.TrackScheduleStatus);
                    dr.get('ProductType').setValue(_blend.ProductType);
                    dr.get('ProductTypeId').setValue(_blend.ProductTypeId);
                    dr.get('RequiredQuantity').setValue(_blend.RequiredQuantity);
                    dr.get('PickupLocationType').setValue(_blend.PickupLocationType);
                    dr.get('Terminal').patchValue({ Id: _blend.Terminal.Id, Name: _blend.Terminal.Name });
                    dr.get('BulkPlant').patchValue({
                        SiteName: _blend.BulkPlant.SiteName, Address: _blend.BulkPlant.Address, City: _blend.BulkPlant.City, State: _blend.BulkPlant.State, Country: { Code: _blend.BulkPlant.Country.Code, Id: _blend.BulkPlant.Country.Id, Name: _blend.BulkPlant.Country.Name }, ZipCode: _blend.BulkPlant.ZipCode, CountyName: _blend.BulkPlant.CountyName, Latitude: _blend.BulkPlant.Latitude, Longitude: _blend.BulkPlant.Longitude
                    });
                }
            }
        });
    }

    pushBlendedRequestsToLoad() {

        let blendedrequests = this.blendRequestForm.get('BlendedRequests').value as BlendedRequest[];

        this._currentTrip.get('DeliveryRequests')['controls'].forEach((dr: FormGroup) => {
            if (dr.get('BlendedGroupId').value && dr.get('BlendedGroupId').value == this.selectedDeliveryRequest.BlendedGroupId) {

                let _blend = blendedrequests.find(bl => bl.Id == dr.get('Id').value);

                if (_blend) {
                    dr.get('OrderId').setValue(_blend.OrderId);
                    dr.get('SchedulePreviousStatus').setValue(_blend.SchedulePreviousStatus);
                    dr.get('ScheduleStatus').setValue(_blend.ScheduleStatus);
                    dr.get('TrackScheduleStatus').setValue(_blend.TrackScheduleStatus);
                    dr.get('ProductType').setValue(_blend.ProductType);
                    dr.get('ProductTypeId').setValue(_blend.ProductTypeId);
                    dr.get('RequiredQuantity').setValue(_blend.RequiredQuantity);
                    dr.get('PickupLocationType').setValue(_blend.PickupLocationType);
                    dr.get('Terminal').patchValue({ Id: _blend.Terminal.Id, Name: _blend.Terminal.Name });
                    dr.get('BulkPlant').patchValue({
                        SiteName: _blend.BulkPlant.SiteName, Address: _blend.BulkPlant.Address, City: _blend.BulkPlant.City, State: _blend.BulkPlant.State, Country: { Code: _blend.BulkPlant.Country.Code, Id: _blend.BulkPlant.Country.Id, Name: _blend.BulkPlant.Country.Name }, ZipCode: _blend.BulkPlant.ZipCode, CountyName: _blend.BulkPlant.CountyName, Latitude: _blend.BulkPlant.Latitude, Longitude: _blend.BulkPlant.Longitude
                    });
                }
                if (dr.get('IsBlendedDrParent').value) {
                    dr.get('TotalBlendedQuantity').setValue(blendedrequests.reduce(function (sum, current) { return sum + current.RequiredQuantity; }, 0));
                }
            }
        });
    }

    deleteRequests(drs: DeliveryRequestViewModel[]) {

        this.carrierService.updateDeliveryRequest(drs).subscribe((data: any) => {
            if (data.StatusCode == 0) {
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

    public deleteBlendedDeliveryRequestFromLoad() {

        this.setDeletePostloadedDrsSubject(this.deletedBlend_temp);

        let allDrs = this.DelGroupForm.get('DeliveryRequests') as FormArray;
        let length = allDrs.length;

        while (length > 0) {

            let index = allDrs.value.findIndex(dr => this.deletedBlend_temp.some(dr1 => dr1.Id == dr.Id));

            if (index != -1) {
                allDrs.removeAt(index);
            }
            length--;
        }

        this.dataService.setScheduleChangeDetectSubject(true);
    }

    setBlendClass(dr: DeliveryRequestViewModel, index: number): string {

        if (dr && dr.IsBlendedRequest) {
            if (dr.IsBlendedDrParent)
                return "blend-first"
            else if (!dr.IsBlendedDrParent && <number>this.DelGroupForm.get('DeliveryRequests')['controls'].length == (index + 1))
                return "blend-last"
            else if (!dr.IsBlendedDrParent && <number>this.DelGroupForm.get('DeliveryRequests')['controls'].length > (index + 1) && dr.BlendedGroupId != this.DelGroupForm.controls['DeliveryRequests']['controls'][(index + 1)].get('BlendedGroupId').value)
                return "blend-last"
            else
                return "blend-middle"
        }
        return "";
    }

    isLastDrOfBlendGroup(dr: DeliveryRequestViewModel, index: number): boolean {
        if ((<number>this.DelGroupForm.get('DeliveryRequests')['controls'].length == (index + 1)) || (<number>this.DelGroupForm.get('DeliveryRequests')['controls'].length > (index + 1) && dr.BlendedGroupId != this.DelGroupForm.controls['DeliveryRequests']['controls'][(index + 1)].get('BlendedGroupId').value))
            return true
        else
            return false
    }

    filterAndGetDrsByBlendGroupId(drs: DeliveryRequestViewModel[], blendGroupId: string): DeliveryRequestViewModel[] {
        if (!drs || drs.length == 0 || !blendGroupId)
            return [];
        return drs.filter(dr => dr.IsBlendedRequest && dr.BlendedGroupId && dr.BlendedGroupId == blendGroupId)
    }
    DoNotShowCancelICON(dsValue: DeliveryRequestViewModel) {
        let cancelDRButtonDisplayStatus = false;
        if (this.EnrouteInProgress.indexOf(dsValue.TrackScheduleEnrouteStatus) >= 0 || this.EnrouteInCompleted.indexOf(dsValue.TrackScheduleStatus) >= 0) {
            cancelDRButtonDisplayStatus = true;
        }
        else if ((dsValue.TrackScheduleEnrouteStatus == 0 || dsValue.TrackScheduleStatus == 0) && (dsValue.Status == 2 || dsValue.Status == 3 || dsValue.Status == 5)) {
            cancelDRButtonDisplayStatus = true;
        }
        return cancelDRButtonDisplayStatus;
    }
}
export enum TaskId {
    Update = 1,
    Add_Update = 2,
    Delete_Update = 3,
    Add_Delete_Update = 4,
}
