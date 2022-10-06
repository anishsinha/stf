import { Component, OnInit, Input, SimpleChanges, ChangeDetectionStrategy, ChangeDetectorRef, ViewChildren, QueryList, NgZone, ViewEncapsulation, ViewChild, Optional } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, Validators, FormControl } from '@angular/forms';
import { DeliveryRequestViewModel, ScheduleShiftModel, TrailerModel, TripModel, RegionDetailModel, TrailerViewModel, DriverViewFilterModel, DSBSaveModel, PreLoadDrViewModel, PreLoadDrResponseViewModel, ModifiedTripInfo, LoadInfo, CompartmentModel, TrailerCompartment, DriverAdditionalDetailModel, DropAddressModel, TransferDSInfo, ShiftColumnInfo, ShiftViewModel, ShiftLoadInfo, DragDSInfo, OrderPickupDetailModel, OptionalPickupDetailModel, OrderFuelInfo, OptionalPickupLocationInfo, OrderFuelDetails, CancelDeliverySchedule, CancelDSDeliveryScheduleInfo, CancelDSDeliverySchedule, CancelDSDeliveryScheduleViewModel, ResetDeliveryGroupScheduleModel, DispatcherMapSeq } from '../../models/DispatchSchedulerModels';
import { Declarations } from 'src/app/declarations.module';
import { Subscription, BehaviorSubject, interval } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { CustomAbstractControls } from '../../customAbstractControl';
import { chatService } from 'src/app/shared-components/sendbird/services/sendbird.service';
import * as moment from 'moment';
import { DriverScheduleColumnViewComponent } from '../child-components/driver-schedule-column-view.component';
import { UtilService } from 'src/app/services/util.service';
import { TrailerFuelRetainModel } from 'src/app/tractor/model';
import { sortArrayTwice, groupDrsByProduct, sortBy, sortByKeyAsc } from 'src/app/my.functions';
import { ScheduleBuilderService } from '../../service/schedule-builder.service';
import { DropdownItem, StatelistService } from 'src/app/statelist.service';
import { MyLocalStorage } from 'src/app/my.localstorage';
import { LoadQueueService } from '../dsb-load-queue/load-queue.service';
import { DSBLoadQueueModel, DSBLoadQueueNotificationModel } from '../dsb-load-queue/dsb-load.model';
import { AddressService } from '../../../address.service';
import { AddressModel } from '../../../invoice/models/DropDetail';
import { OptimizedCapacityConfirmation, RegExConstants, SBConstants } from '../../../app.constants';
import { DeliveryGroupStatus, DeliveryReqStatus, DSBLoadQueueStatus, TripStatus } from 'src/app/app.enum';
import { ConfirmationDialogService } from 'src/app/shared-components/confirmation-dialog/confirmation-dialog.service';

@Component({
    selector: 'app-driver-column-view',
    templateUrl: './driver-column-view.component.html',
    styleUrls: ['./driver-column-view.component.scss'],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class DriverColumnViewComponent implements OnInit {
    @Input() public SbForm: FormGroup;
    @Input() public Shifts: ScheduleShiftModel[];
    @Input() public RegionDetail: RegionDetailModel;
    @Input() public SelectedRegionId: string;
    @Input() public IsTrailerExists: boolean;
    @Input() public HeaderToggle: boolean;
    @Input() public IsNoDriverShiftFound: number = 0;
    public _loadingRequests = false;
    public _loadingDrivers = false;
    public _savingBuilder = false;
    public _loadingCmprts = false;
    //public _otherRegionDriver = false;
    public _otherRegionDriverSubject: BehaviorSubject<any>;
    public _publishedRequestExists = false;

    public PreLoadInfo: any = null;
    private preloadSelectedTrailerId: string;

    public SelectedDriverName: string = '';

    public SelectedTrailers: TrailerViewModel[] = [];
    public UnassignedTrailers: TrailerViewModel[] = [];
    public UnassignedDrivers: DriverAdditionalDetailModel[] = [];
    public AllUnassignedDrivers: DriverAdditionalDetailModel[] = [];
    public chatDriverDetails = [];
    public TrailerDdlSettings = {};
    public Collapsed = [];
    public CollapsedIcons = [];
    public draggedDelReqData: any;
    public droppedTrip: any;
    public CompletedScheduleStatuses: number[] = [7, 8, 9, 10, 24];
    public OnTheWayScheduleStatuses: number[] = [1, 3, 9, 11, 12, 13, 15, 16, 17, 18, 19, 20];
    public ScheduleStatuses: number[] = [3, 15, 11]; /*if any scheduled status or missed status*/
    public CompartmentEditModel: any = {};
    public TrailerCompartments: any = {};
    public TrailerCompartmentRetains: TrailerCompartment[] = [];
    public CompartmentDetails: TrailerFuelRetainModel[];
    public CompartmentViewFormGroup: FormGroup;
    public selTrailerIndex: number;
    public selTrailerlist: [];
    public updateDrFormGroup: FormGroup;
    public DriverTrailerForm: FormGroup;
    public DraftDeliveryGroupSubscription: Subscription;
    public SaveModifiedLoadsSubscription: Subscription;
    public PublishDeliveryGroupSubscription: Subscription;
    public CancelDeliveryGroupSubscription: Subscription;
    public DeleteDeliveryGroupSubscription: Subscription;
    public DeliveryScheduleRemoveSubject: Subscription;
    private DeletedGroup: any;
    private PublishedGroup: any;
    private EditDriverData: any;
    private EditDriverTrailerSubscription: Subscription;
    private SaveDriverAssignmentSubscription: Subscription;
    private PublishEntireRowSubscription: Subscription;
    private DragDropItemSubscription: Subscription;
    private CreateLoadCancelSubscription: Subscription;
    private CreateLoadSuccessSubscription: Subscription;
    private CreatePreloadSubscription: Subscription;
    private DisableControlsSubscription: Subscription;
    private UpdatePostloadSubscription: Subscription;
    private DeletePostloadSubscription: Subscription;
    public disableControl: boolean = false;
    private TrasnferDSSubscription: Subscription;
    private TrailerInfoDSSubscription: Subscription;
    private RemoveTrailerGroupSubject: Subscription;
    private RouteInfoDSSubscription: Subscription;
    public DSShifts: ShiftViewModel[] = [];
    public SelectedShift: ShiftViewModel;
    public SelectedShiftIndex: number;
    public ShiftColumn: ShiftColumnInfo[] = [];
    public SelectedShiftColumn: ShiftColumnInfo;
    public ShiftLoad: ShiftLoadInfo[] = [];
    public SelectedLoad: ShiftLoadInfo;
    public _loadingDSData: boolean = false;
    public SelectedDSDriverName: string = '';
    public $eventDataTransfer: TransferDSInfo = new TransferDSInfo();
    public selectedDefaultShiftIndex: number = -1;
    public selectedRowIndex: number = -1;
    public selectedColIndex: number = -1;
    public scheduleTrailerInfo: any;
    public _loadingRemoveTrailer: boolean = false;
    SelectedTrip: any = null;
    RouteListForTrip: any[] = [];
    private ShiftVisibility: Subscription;
    public RouteDeleteDeliveryGroupSubject: Subscription;
    private DsbLoadQueueNotification: Subscription;
    public routeInfo: any = null;
    public _loadingRemoveRoute: boolean = false;
    @ViewChildren(DriverScheduleColumnViewComponent) driverSchedules: QueryList<DriverScheduleColumnViewComponent>;
    DriverColCDRSubscription: Subscription;
    public PickupForm: FormGroup;
    public LocationType: number = 1;
    private DGSubscription: Subscription = new Subscription();
    public FuelTypeDetails: DropdownItem[] = [];
    public SelectedFuelTypeDetails: DropdownItem[] = [];
    public FuelTypeDdlSettings = {};
    public BulkPlants: DropdownItem[];
    public BulkplantList: DropdownItem[];
    public isReadOnly: boolean = false;
    public Terminals = [];
    public OptionalPickupLocationInfo: OptionalPickupDetailModel[] = [];
    public _loadingOptionTerminal: boolean = true;
    private OptionalPickupSubscription: Subscription;
    public StateList: DropdownItem[] = [];
    public CountryList: DropdownItem[] = [];
    public CountryBasedZipcodeLabel: string = "Zip";
    public _loadingAddress: boolean = false;
    public OptionPickupOrderIds = [];
    public OptionalPickupShiftIndex = 0;
    public OptionalPickupShiftOrderNo = 0;
    public OptionalPickupDriverColIndex = 0;
    public OptionalPickupShiftId: string = '';
    public OptionalPickupRegionId: string = '';
    public OptionalPickupScheduleBuilderId: string = '';
    public OrderFuelDetails: OrderFuelDetails[] = [];
    public OptionPickupScheduleGroup: FormGroup;
    public OptionalPickupLocationDeleteInfo: OptionalPickupDetailModel;
    public cancelDSScheduleInfo: any = {};
    private CancelEntireRowSubscription: Subscription;
    public CancelDSScheduleLoadInfo: any = {};
    keyword = 'Name';
    @ViewChildren('autoCompleteInput') autoCompleteInput: QueryList<DriverScheduleColumnViewComponent>;
    public ScheduleOptionalPickupDetailModel: OptionalPickupDetailModel[] = [];
    public ScheduleOrderFuelInfo: OrderFuelDetails[] = [];
    public CancelDSDeliveryGroupSubscription: Subscription;
    public CancelDSDeliveryGroupNormalGroupDSSubject: Subscription;
    public CancelDSLocationGroupSubscription: Subscription;
    public cancelDSDeliveryScheduleViewModel: CancelDSDeliveryScheduleViewModel[] = [];
    public DeliveryReqCancelScheduleUpdateModel: CancelDeliverySchedule[] = [];
    public DispatcherLoadDragDropSubscription: Subscription;
    public DispatcherLoadDragDropMapSubscription: Subscription;

    constructor(private fb: FormBuilder,
        private sbService: ScheduleBuilderService,
        public dataService: DataService,
        private chatService: chatService,
        private utilService: UtilService,
        private changeDetectorRef: ChangeDetectorRef,
        private loadQueueService: LoadQueueService,
        private addresService: AddressService,
        private stateService: StatelistService,
        private confirmationDialogService: ConfirmationDialogService) {
        this._otherRegionDriverSubject = new BehaviorSubject(false);
    }

    ngOnInit() {
        this.DriverTrailerForm = this.utilService.getDriverTrailerForm();
        this.TrailerDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'TrailerId',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
        };
        this.CompartmentViewFormGroup = this.utilService.getCompartmentViewForm([], null);
        this.dataService.setShowDeliveryGroupSubject(false);
        this.subscribeDeliveryGroupEvents();
        this.subscribeSaveModifiedLoadsSubject();
        this.subscribeEditDriverTrailerEvents();
        this.subscribeDraftAndPublishEvents();
        this.subscribeDragDropItemSubject();
        this.subscribeDisableControlsSubject();
        this.subscriberCreateLoadCancelSubject();
        this.subscribeCreateLoadSuccessSubject();
        this.subscribeCreatePreloadSubject();
        this.subscribeUpdatePostloadSubject();
        this.subscribeDeletePostloadSubject();
        this.subscribeEditCompartmentAssigmentSubject();
        this.CompartmentDetails = [];
        this.TrailerCompartmentRetains = [];
        this.subscribeTransferDSEvents();
        this.subscribeTrailerInfoDSEvents();
        this.subscribeRouteInfoDSEvents();
        this.subscribeTrailerRemoveDSEvents();
        this.subscribeLoadQueueNotifications();
        this.subscribeChangeDetectDsbLoadQueueNotification();
        this.subscribeDriverColumnChangeDetect();
        this.FuelTypeDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
        };
        this.subscribeOptionPickupEvents();
        this.PickupForm = this.initOptionalPickupForm(new OrderPickupDetailModel());
        this.subscribeShowScheduleBuilderLoadingSubject();
        this.subscribeCancelScheduleEvents();
        this.subscribeCancelDSLocationGroupSubject();
        this.subscribeDispatcherLoadDragDropSubscription();
        this.subscribeDispatcherLoadDragDropMapSubscription();
    }

    ngOnChanges(change: SimpleChanges) {
        if (change.Shifts && change.Shifts.currentValue) {
            this.dataService.setShowDeliveryGroupSubject(false);
            this.subscribeDisableControlsSubject();
            this.initShifts(this.Shifts);
            this.dataService.setAllShiftsSubject(this.Shifts);
            this.resetDriverTrailerForm();
        }
        if (change.RegionDetail && change.RegionDetail.currentValue) {
            this.RegionDetail = change.RegionDetail.currentValue;
            this.dataService.setAllTrailersSubject(this.RegionDetail.Trailers);
        }
    }

    ngAfterViewInit(): void {
        this.dataService.setUnsavedChangesAsEmptySubject();
        this.setUnsavedChanges();
        this.subscribeOptionalPickupLocationChange();
    }

    ngOnDestroy(): void {
        this.unsubscribeDeliveryGroupEvents();
        this.unsubscribeEditDriverTrailerEvents();
        this.unsubscribeDraftAndPublishEvents();
        this.unsubscribeDragDropItemSubject();
        this.unsubscriberCreateLoadCancelSubject();
        this.unsubscribeCreateLoadSuccessSubject();
        this.unsubscribeCreatePreloadSubject();
        this.unsubscribeUpdatePostloadSubject();
        this.unsubscribeDeletePostloadSubject();
        var _shifts = this.SbForm.get('Shifts') as FormArray;
        if (_shifts) {
            _shifts.clear(); // Clear shifts of driver view
        }
        if (this.DisableControlsSubscription) {
            this.DisableControlsSubscription.unsubscribe();
        }
        if (this.TrasnferDSSubscription) {
            this.TrasnferDSSubscription.unsubscribe();
        }
        if (this.TrailerInfoDSSubscription) {
            this.TrailerInfoDSSubscription.unsubscribe();
        }
        if (this.RouteInfoDSSubscription) {
            this.RouteInfoDSSubscription.unsubscribe();
        }
        if (this.RemoveTrailerGroupSubject) {
            this.RemoveTrailerGroupSubject.unsubscribe();
        }
        if (this.ShiftVisibility) {
            this.ShiftVisibility.unsubscribe();
        }
        if (this.DriverColCDRSubscription) {
            this.DriverColCDRSubscription.unsubscribe();
        }
        if (this.OptionalPickupSubscription) {
            this.OptionalPickupSubscription.unsubscribe();
        }
        this.unsubscribeAllDGSubscriptions();
        this.unsubscribeCancelScheduleEvents();
        this.unsubscribeCancelLocationScheduleEvents();
        if (this.DispatcherLoadDragDropSubscription) {
            this.DispatcherLoadDragDropSubscription.unsubscribe();
        }
        if (this.DispatcherLoadDragDropMapSubscription) {
            this.DispatcherLoadDragDropMapSubscription.unsubscribe();
        }
    }

    subscribeDeliveryGroupEvents(): void {
        this.subscribeDraftDeliveryGroupSubject();
        this.subscribePublishDeliveryGroupSubject();
        this.subscribeCancelDeliveryGroupSubject();
        this.subscribeDeleteDeliveryGroupSubject();
        this.subscribeRouteDeleteDeliveryGroupSubject();
        this.subscribeCancelDSDeliveryGroupSubject();
        this.subscribeCancelDeliveryGroupNormalGroupDSSubject();
        this.subscribeDeliveryScheduleRemoveSubject();
    }

    unsubscribeDeliveryGroupEvents(): void {
        if (this.DraftDeliveryGroupSubscription) {
            this.DraftDeliveryGroupSubscription.unsubscribe();
        }
        if (this.SaveModifiedLoadsSubscription) {
            this.SaveModifiedLoadsSubscription.unsubscribe();
        }
        if (this.PublishDeliveryGroupSubscription) {
            this.PublishDeliveryGroupSubscription.unsubscribe();
        }
        if (this.DeleteDeliveryGroupSubscription) {
            this.DeleteDeliveryGroupSubscription.unsubscribe();
        }
        if (this.CancelDeliveryGroupSubscription) {
            this.CancelDeliveryGroupSubscription.unsubscribe();
        }
        if (this.CancelDSDeliveryGroupSubscription) {
            this.CancelDSDeliveryGroupSubscription.unsubscribe();
        }
        if (this.CancelDSDeliveryGroupNormalGroupDSSubject) {
            this.CancelDSDeliveryGroupNormalGroupDSSubject.unsubscribe();
        }
        if (this.DeliveryScheduleRemoveSubject) {
            this.DeliveryScheduleRemoveSubject.unsubscribe();
        }
    }

    unsubscribeEditDriverTrailerEvents(): void {
        if (this.EditDriverTrailerSubscription) {
            this.EditDriverTrailerSubscription.unsubscribe();
        }
        if (this.SaveDriverAssignmentSubscription) {
            this.SaveDriverAssignmentSubscription.unsubscribe();
        }
    }

    unsubscribeDraftAndPublishEvents(): void {
        if (this.PublishEntireRowSubscription) {
            this.PublishEntireRowSubscription.unsubscribe();
        }
    }

    unsubscribeDragDropItemSubject(): void {
        if (this.DragDropItemSubscription) {
            this.DragDropItemSubscription.unsubscribe();
        }
    }

    unsubscriberCreateLoadCancelSubject(): void {
        if (this.CreateLoadCancelSubscription) {
            this.CreateLoadCancelSubscription.unsubscribe();
        }
    }

    unsubscribeCreateLoadSuccessSubject(): void {
        if (this.CreateLoadSuccessSubscription) {
            this.CreateLoadSuccessSubscription.unsubscribe();
        }
    }

    unsubscribeCreatePreloadSubject(): void {
        if (this.CreatePreloadSubscription) {
            this.CreatePreloadSubscription.unsubscribe();
        }
    }

    unsubscribeUpdatePostloadSubject(): void {
        if (this.UpdatePostloadSubscription) {
            this.UpdatePostloadSubscription.unsubscribe();
        }
    }

    unsubscribeDeletePostloadSubject(): void {
        if (this.DeletePostloadSubscription) {
            this.DeletePostloadSubscription.unsubscribe();
        }
    }

    subscribeDraftDeliveryGroupSubject(): void {
        this.DraftDeliveryGroupSubscription = this.dataService.DraftDeliveryGroupSubject.subscribe(x => {
            this.draftScheduleBuilder(x.trip, x.filterChanged);
        });
    }

    subscribeSaveModifiedLoadsSubject(): void {
        this.SaveModifiedLoadsSubscription = this.dataService.SaveModifiedLoadsSubject.subscribe(x => {
            if (x && x.length > 0) {
                this.saveScheduleBuilderData(x, true);
            }
        });
    }

    subscribePublishDeliveryGroupSubject(): void {
        this.PublishDeliveryGroupSubscription = this.dataService.PublishDeliveryGroupSubject.subscribe(x => {
            if (x) {
                this.ScheduleOptionalPickupDetailModel = [];
                this.ScheduleOrderFuelInfo = null;
                if (x.isOptionalPickup) {
                    this.ScheduleOptionalPickupDetailModel = x.OptionalPickupInfo;
                    this.ScheduleOrderFuelInfo = x.OrderFuelInfo;
                }
                this.publishScheduleBuilder(x.shiftIndex, x.rowIndex, x.colIndex, x.schedule, x.trip, x.isOptionalPickup);
            }
        });
    }

    subscribeDisableControlsSubject(): void {
        this.DisableControlsSubscription = this.dataService.DisableDSBControlsSubject.subscribe(x => {
            this.disableControl = x;
        });
    }

    subscribeCancelDeliveryGroupSubject(): void {
        this.CancelDeliveryGroupSubscription = this.dataService.CancelDeliveryGroupSubject.subscribe(x => {
            if (x) {
                this.cancelScheduleBuilder(x.shiftIndex, x.rowIndex, x.tripIndex, x.trip);
            }
        });
    }

    subscribeDeleteDeliveryGroupSubject(): void {
        this.DeleteDeliveryGroupSubscription = this.dataService.DeleteDeliveryGroupSubject.subscribe(x => {
            if (x) {
                this.deleteGroup(x);
            }
        });
    }

    subscribeEditDriverTrailerEvents(): void {
        this.EditDriverTrailerSubscription = this.dataService.EditDriverTrailerSubject.subscribe(x => {
            if (x) {
                this.editDriverTrailers(x);
            }
        });
        this.SaveDriverAssignmentSubscription = this.dataService.SaveDriverAssignmentSubject.subscribe(x => {
            if (x) {
                this.saveDriverAssignment(x.Index1, x.Index2);
            }
        });
    }

    subscribeDraftAndPublishEvents(): void {
        this.PublishEntireRowSubscription = this.dataService.PublishEntireRowSubject.subscribe(x => {
            if (x) {
                this.ScheduleOptionalPickupDetailModel = [];
                this.ScheduleOrderFuelInfo = null;
                if (x.IsOptionalPickup) {
                    this.ScheduleOptionalPickupDetailModel = x.OptionalPickupInfo;
                    this.ScheduleOrderFuelInfo = x.OrderFuelInfo;
                }
                this.validateRowPublish(x.ShiftIndex, x.ScheduleIndex, x.IsOptionalPickup);
            }
        });
    }

    subscribeDragDropItemSubject(): void {
        this.DragDropItemSubscription = this.dataService.DragDropItemSubject.subscribe(x => {
            if (x) {
                this.onItemDrop(x.trip, x.event, x.shiftIndex, x.rowIndex, x.colIndex, x.schedule);
            }
        });
    }

    subscriberCreateLoadCancelSubject(): void {
        this.CreateLoadCancelSubscription = this.dataService.CreateLoadCancelSubject.subscribe(x => {
            if (x) {
                this.cancelCreateLoad(x);
            }
        });
    }

    subscribeCreateLoadSuccessSubject(): void {
        this.CreateLoadSuccessSubscription = this.dataService.CreateLoadSuccessSubject.subscribe(x => {
            if (x) {
                this.updateTripOnCreateLoadSucess(x);
            }
        });
    }

    subscribeCreatePreloadSubject(): void {
        this.CreatePreloadSubscription = this.dataService.CreatePreloadSubject.subscribe(x => {
            if (x) {
                this.processPreloadDeliveryCreation(x);
            }
        });
    }

    subscribeUpdatePostloadSubject(): void {
        this.UpdatePostloadSubscription = this.dataService.UpdatePostloadSubject.subscribe(x => {
            if (x) {
                this.updateEditedPostloadDrs(x);
            }
        });
    }

    subscribeDeletePostloadSubject(): void {
        this.DeletePostloadSubscription = this.dataService.DeletePostloadSubject.subscribe(x => {
            if (x) {
                this.deletePreAndPostloadDrs(x);
            }
        });
    }

    subscribeEditCompartmentAssigmentSubject(): void {
        this.dataService.EditCompartmentAssigmentSubject.subscribe(x => {
            if (x) {
                this.editCompartmentAssignments(x);
            }
        });
    }
    subscribeDeliveryScheduleRemoveSubject(): void {
        this.DeliveryScheduleRemoveSubject = this.dataService.DeliveryScheduleRemoveSubject.subscribe(x => {
            this.deleteSingleLoad(x);
        });
    }
    initShifts(shifts: ScheduleShiftModel[]): void {

        let _shiftArray = this.SbForm.controls['Shifts'] as FormArray;
        _shiftArray.clear();

        shifts.forEach((x, i) => {

            let _shiftForm = this.fb.group({
                Id: this.fb.control(x.Id),
                Schedules: this.fb.array([]), // Rows in the shift 
                StartTime: this.fb.control(x.StartTime),
                EndTime: this.fb.control(x.EndTime),
                SlotPeriod: this.fb.control(x.SlotPeriod),
                IsCollapsed: this.fb.control(x.IsCollapsed),
                IsVisible: this.fb.control(true),
                OrderNo: this.fb.control(x.OrderNo),
            })

            let _sArray = this.getSchedulesFormArray(x.Schedules);
            let _schedules = _shiftForm.controls['Schedules'] as FormArray;
            _sArray.controls.forEach(x => _schedules.push(x));

            _shiftArray.push(_shiftForm);

            this.Collapsed.push((x.IsCollapsed ? '' : 'show'));
            this.CollapsedIcons.push((x.IsCollapsed ? 'collapsed' : ''));
        });
    }
    getSchedulesFormArray(schedules: TrailerModel[]): FormArray {
        var _sArray = this.fb.array([]);
        schedules.forEach(x => {
            _sArray.push(this.fb.group({
                AllowDriverChange: this.fb.control(x.AllowDriverChange),
                Drivers: this.utilService.getDriversFormArray(x.Drivers),
                Trailers: this.utilService.getTrailersFormArray(x.Trailers),
                Trips: this.utilService.getTripsFormArray(x.Trips),
                LoadQueueId: this.fb.control(x.LoadQueueId),
                IsLoadQueueCollapsed: this.fb.control(x.IsLoadQueueCollapsed),
                IsColumnSelected: this.fb.control(x.IsColumnSelected),
                IsLoadQueueColumnBlocked: this.fb.control(x.IsLoadQueueColumnBlocked),
                IsIncludeAllRegionDriver: this.fb.control(x.IsIncludeAllRegionDriver),
                LoadQueueColumnStatus: this.fb.control(x.LoadQueueColumnStatus),
                LoadQueueFilterVisibility: this.fb.control(true),
                IsDriverScheduleExists: this.fb.control(x.IsDriverScheduleExists),
                DriverRowIndex: this.fb.control(x.DriverRowIndex),
            }));
        });
        return _sArray;
    }

    private resetDriverTrailerForm(): void {
        this.SelectedTrailers = [];
        this.UnassignedTrailers = [];
        if (this.DriverTrailerForm) {
            this.DriverTrailerForm.reset();
        }
    }

    validateTrailerJobCompatibility(drData: any, schedule: FormGroup): void {
        var _deliveryRequests = drData.Data.value;
        let _selectedTrailers = schedule.controls["Trailers"].value;
        var trips = schedule.controls['Trips'] as FormArray;
        if ((_selectedTrailers && _selectedTrailers.length > 0) && (_deliveryRequests && _deliveryRequests.length > 0)) {
            this.sbService.validateTrailerJobCompatibility(_selectedTrailers, _deliveryRequests)
                .subscribe(data => {
                    if (data && data.deliveryRequestsNotCompatible && data.deliveryRequestsNotCompatible.length > 0) {
                        this.highLightDRDiv(trips, data);
                        Declarations.msgerror("This job location is not compatible with trailer type", undefined, undefined);
                    } else {
                        this.highLightDRDiv(trips, null);
                    }
                });
        }
    }

    highLightDRDiv(trips: FormArray, data) {
        for (var i = 0; i < trips.length; i++) {
            var trip = trips.controls[i] as FormGroup;
            var deliveryRequests = trip.controls["DeliveryRequests"] as FormArray;
            if (deliveryRequests) {
                for (var j = 0; j < deliveryRequests.length; j++) {
                    var deliveryRequest = deliveryRequests.controls[j] as FormGroup;
                    deliveryRequest.controls["IsNotCompatible"].patchValue(false);
                    deliveryRequest.controls["IsBlinkDR"].patchValue(false);
                    if (data) {
                        if (data.deliveryRequestsNotCompatible.find(t => t.Id == deliveryRequest.controls["Id"].value)) {
                            deliveryRequest.controls["IsNotCompatible"].patchValue(true);
                            deliveryRequest.controls["IsBlinkDR"].patchValue(true);
                            setTimeout(() => {
                                this.removeClassForAllLoad(trips);
                            }, 10000);
                        }
                    }

                }
            }
        }
        this.changeDetectorRef.detectChanges();
    }
    highLightDRDivOneLoad(trip: FormGroup, data) {
        var deliveryRequests = trip.get("DeliveryRequests") as FormArray;
        if (deliveryRequests) {
            for (var j = 0; j < deliveryRequests.length; j++) {
                var deliveryRequest = deliveryRequests.controls[j] as FormGroup;
                deliveryRequest.get("IsNotCompatible").patchValue(false);
                deliveryRequest.get("IsBlinkDR").patchValue(false);
                if (data && data.deliveryRequestsNotCompatible) {
                    if (data.deliveryRequestsNotCompatible.find(t => t.Id == deliveryRequest.get("Id").value)) {
                        deliveryRequest.get("IsNotCompatible").patchValue(true);
                        deliveryRequest.get("IsBlinkDR").patchValue(true);
                        setTimeout(() => {
                            this.removeClassForLoad(trip);
                        }, 10000);
                    }
                }

            }
        }
        this.changeDetectorRef.detectChanges();
    }
    removeClassForAllLoad(trips: FormArray) {
        for (var i = 0; i < trips.length; i++) {
            var trip = trips.controls[i] as FormGroup;
            var deliveryRequests = trip.get("DeliveryRequests") as FormArray;
            if (deliveryRequests) {
                for (var j = 0; j < deliveryRequests.length; j++) {
                    var deliveryRequest = deliveryRequests.controls[j] as FormGroup;
                    deliveryRequest.get("IsBlinkDR").patchValue(false);
                }
            }
        }
    }
    removeClassForLoad(trip: FormGroup) {
        var deliveryRequests = trip.get("DeliveryRequests") as FormArray;
        if (deliveryRequests) {
            for (var j = 0; j < deliveryRequests.length; j++) {
                var deliveryRequest = deliveryRequests.controls[j] as FormGroup;
                deliveryRequest.get("IsBlinkDR").patchValue(false);
            }
        }
    }
    IsDRCompatible(trip: FormGroup) {
        var isDRCompatible = true;
        var deliveryRequests = trip.get("DeliveryRequests") as FormArray;
        if (deliveryRequests) {
            for (var j = 0; j < deliveryRequests.length; j++) {
                var deliveryRequest = deliveryRequests.controls[j] as FormGroup;
                if (deliveryRequest && deliveryRequest.get("IsNotCompatible").value) {
                    isDRCompatible = false;
                }
            }
        }
        return isDRCompatible;
    }

    onItemDrop(trip: FormGroup, event: any, shiftIndex: number, rowIndex: number, colIndex: number, schedule: any): void {
        let drData = event.dragData;
        let dragToLoad = false;
        if (drData && drData.Data && drData.Data.length == 0) {
            return; //There is no dr in dragged data, so don't take any action.
        }
        if (drData.TripFrom) {
            if (drData.TripFrom.controls['DriverRowIndex'].value == trip.controls['DriverRowIndex'].value && drData.TripFrom.controls['DriverColIndex'].value == trip.controls['DriverColIndex'].value && drData.TripFrom.controls['ShiftIndex'].value == trip.controls['ShiftIndex'].value)
                return;
        }
        this._savingBuilder = true;
        this.changeDetectorRef.detectChanges();
        let drDataCopied = JSON.parse(JSON.stringify(drData.Data));
        if (drData.TripFrom) {
            let isCommonPickup = trip.get('IsCommonPickup').value as boolean;
            let IsDispatcherDragDropSequence = trip.get('IsDispatcherDragDropSequence').value as boolean;
            if ((drData.TripFrom.controls['DriverRowIndex'].value != trip.controls['DriverRowIndex'].value) || (drData.TripFrom.controls['ShiftIndex'].value == trip.controls['ShiftIndex'].value)) {
                drData.Data = this.utilService.getDeliveryRequestFormArray(drData.Data, isCommonPickup, IsDispatcherDragDropSequence, 1, drData.TripFrom);
            }
            else {
                drData.Data = this.utilService.getDeliveryRequestFormArray(drData.Data, isCommonPickup, IsDispatcherDragDropSequence, 0, drData.TripFrom);
            }
            this.draggedDelReqData = event.dragData;
            this.droppedTrip = { Trip: trip, ShiftIndex: shiftIndex, RowIndex: rowIndex, ColIndex: colIndex, Schedule: schedule };
            if (this.isDraggedDRsPublished(drData.Data.value) || (drData.TripFrom.value && drData.TripFrom.value.DeliveryRequests.length > 1 && drData.TripFrom.controls['DeliveryGroupPrevStatus'].value == 2) || trip.controls['DeliveryGroupPrevStatus'].value == 2) {
                var scheduleIds: number[] = this.getPublishedDRsTrackableScheduleIds(drData.Data.value);
                if (scheduleIds.length > 0) {
                    this.dragDelReqToAnotherLoad(scheduleIds);
                } else {
                    this._savingBuilder = false;
                    this.changeDetectorRef.detectChanges();
                    jQuery('#btnconfirm-dragdrop-dr').click();
                }
            }
            else {
                dragToLoad = true;
            }
        } else {
            let isCommonPickup = trip.get('IsCommonPickup').value as boolean;
            let IsDispatcherDragDropSequence = trip.get('IsDispatcherDragDropSequence').value as boolean;
            drData.Data = this.utilService.getDeliveryRequestFormArray(drData.Data, isCommonPickup, IsDispatcherDragDropSequence, 0, drData.TripFrom);
            trip.controls['IsEditable'].setValue(true);
            dragToLoad = true;
        }
        if (dragToLoad == true) {
            this._savingBuilder = true;
            this.createLoad({
                RegionId: this.SelectedRegionId,
                Customer: drDataCopied[0].CustomerCompany,
                JobId: drDataCopied[0].JobId,
                JobName: drDataCopied[0].JobName,
                Drs: drDataCopied,
                ShiftIndex: shiftIndex,
                RowIndex: rowIndex,
                ColIndex: colIndex,
                Schedule: schedule,
                Trip: trip,
                DrData: drData,
                IsDragFromLoad: drData.TripFrom != null
            });
        }
        this.dataService.setDeliveryPreloadOption({ ShiftIndex: shiftIndex, ScheduleIndex: colIndex, Reset: true });
    }

    isDraggedDRsPublished(delRequests: DeliveryRequestViewModel[]) {
        return delRequests.filter(t => t.PreviousStatus == 3).length > 0;
    }

    getPublishedDRsTrackableScheduleIds(delRequests: DeliveryRequestViewModel[]) {
        return delRequests.filter(t => t.PreviousStatus == 3).map(t => t.TrackableScheduleId);
    }

    dragDelReqToAnotherLoad(scheduleIds: any) {
        this._savingBuilder = true;
        this.sbService.getScheduleStatus(scheduleIds).subscribe(response => {
            this._savingBuilder = false;
            if (response != null && response != undefined && response.length > 0) {
                let completedSchedules = this.sbService.returnCommonElements(this.CompletedScheduleStatuses, response, true);
                let isCompletedSchedule = completedSchedules.length > 0;
                let onTheWaySchedules = this.sbService.returnCommonElements(this.OnTheWayScheduleStatuses, response, false);
                let cancelledSchedules = this.sbService.returnCommonElements(SBConstants.CancelledScheduleStatuses, response, true);
                if (isCompletedSchedule || response.filter(t => t.ScheduleEnrouteStatusId == 4).length > 0) {
                    Declarations.msgerror("Can't delete from this load as drop has been made already", 'Warning', 2500);
                    this.droppedTrip = null;
                    this.draggedDelReqData = null;
                    return;
                } else if (onTheWaySchedules.length > 0) {
                    this.sbService.setConfirmationHeadingForDR(onTheWaySchedules[0]);
                    jQuery('#btnconfirm-dragdrop-dr').click();
                    return;
                } else if (cancelledSchedules.length > 0) {
                    Declarations.msgerror("Can't drag from this load as drop has been cancelled", 'Warning', 2500);
                    this.droppedTrip = null;
                    this.draggedDelReqData = null;
                    return;
                }
                else {
                    if (this.draggedDelReqData.TripFrom.get('DeliveryGroupPrevStatus').value == 2 || this.draggedDelReqData.get('DeliveryGroupPrevStatus').value == 2) {
                        jQuery('#btnconfirm-dragdrop-dr').click();
                    } else {
                        this.dragDelReqToAnotherLoadYes();
                    }
                }
            } else {
                if (this.draggedDelReqData.TripFrom.get('DeliveryGroupPrevStatus').value == 2 || this.draggedDelReqData.get('DeliveryGroupPrevStatus').value == 2) {
                    jQuery('#btnconfirm-dragdrop-dr').click();
                } else {
                    this.dragDelReqToAnotherLoadYes();
                }
            }
            this.changeDetectorRef.detectChanges();
        });
    }

    private DropDelReqToLoad(droppedToSchedule: any, droppedToTrip: FormGroup, drData: any, drsToRemove: any) {
        var isDraggedFromLoad = drData.TripFrom != undefined && drData.TripFrom != null;
        let isCommonPickup = droppedToTrip.controls['IsCommonPickup'].value as boolean;
        var locationType = droppedToTrip.controls['PickupLocationType'].value;
        var pickupLocation = null;
        if (isCommonPickup) {
            if (locationType != 2)
                pickupLocation = droppedToTrip.controls['Terminal'].value
            else
                pickupLocation = droppedToTrip.controls['BulkPlant'].value;
        }

        var _drArray = droppedToTrip.controls['DeliveryRequests'] as FormArray;
        var _drArrayValue = droppedToTrip.controls['DeliveryRequests'].value as DeliveryRequestViewModel[];
        var _drFormArray = drData.Data as FormArray;
        _drFormArray.controls.forEach((_drForm: FormGroup) => {
            if (isCommonPickup) {
                _drForm.controls['Terminal'].disable();
                _drForm.controls['BulkPlant'].disable();
            }
            else {
                var drValue = _drForm.value;
                var isDRPickupExists = (drValue.PickupLocationType != 2 && drValue.Terminal && drValue.Terminal.Id > 0) || (drValue.PickupLocationType == 2 && drValue.BulkPlant && drValue.BulkPlant.SiteName && drValue.BulkPlant.SiteName.length > 0);
                if (pickupLocation) {
                    if (!isDRPickupExists) {
                        if (locationType != 2) {
                            _drForm.controls['Terminal'].patchValue(pickupLocation);
                            _drForm.controls['PickupLocationType'].patchValue(1);
                        }
                        else {
                            _drForm.controls['BulkPlant'].patchValue(pickupLocation);
                            _drForm.controls['PickupLocationType'].patchValue(2);
                        }
                    }
                }
            }
            if (!isDraggedFromLoad) {
                _drForm.controls['ScheduleStatus'].setValue(14);
                _drForm.controls['Status'].setValue(5);
            }
            if (droppedToTrip.controls['IsDispatcherDragDropSequence'].value == true) {
                var drFormJobId = _drForm.controls['JobId'].value;
                if (_drArrayValue.find(x => x.JobId == drFormJobId) != null) {
                    var jobInfoIndex = _drArrayValue.findIndex(x => x.JobId == drFormJobId);
                    _drArray.insert(jobInfoIndex, _drForm);
                }
                else {
                    _drArray.push(_drForm);
                }
            }
            else {
                _drArray.push(_drForm);
            }
        });

        if (isDraggedFromLoad) {
            var drInFromTrip = drData.TripFrom.controls['DeliveryRequests'] as FormArray;
            for (let idx = 0; idx < _drFormArray.controls.length; idx++) {
                let drIndex = drInFromTrip.controls.findIndex((x: FormGroup) => {
                    return x.controls['Id'].value == _drFormArray.controls[idx]['controls'].Id.value
                });
                _drFormArray.controls[idx].get('SourceTripId').setValue(drData.TripFrom.controls['TripId'].value);
                drInFromTrip.removeAt(drIndex);
            }
            if (droppedToTrip.controls['IsDispatcherDragDropSequence'].value == true) {
                droppedToTrip.controls['IsDispatcherDragDropSequenceModified'].setValue(true);
            }
            this.dragDropDeliveryRequestsSave(drData.TripFrom, droppedToTrip, drData.Schedule, droppedToSchedule);
        } else {
            if (droppedToTrip.controls['IsDispatcherDragDropSequence'].value == true) {
                droppedToTrip.controls['IsDispatcherDragDropSequenceModified'].setValue(true);
            }
            this.dataService.setRemoveDroppedRequestSubject(drsToRemove);
            if (droppedToTrip.value && (this.isDraggedDRsPublished(droppedToTrip.value.DeliveryRequests) || (droppedToTrip.value.DeliveryRequests.length > 1 && droppedToTrip.controls['DeliveryGroupPrevStatus'].value == 2))) {
                this.publishScheduleBuilder(droppedToTrip.controls['ShiftIndex'].value, droppedToTrip.controls['DriverRowIndex'].value, droppedToTrip.controls['DriverColIndex'].value, droppedToSchedule, droppedToTrip);
            }
            else {
                this.draftScheduleBuilder(droppedToTrip);
            }
        }
        this.validateTrailerJobCompatibility(drData, droppedToSchedule);
        this._savingBuilder = false;
        this.changeDetectorRef.markForCheck();
        this.dataService.setScheduleChangeDetectSubject(true);

    }

    dragDelReqToAnotherLoadYes() {
        this._savingBuilder = true;
        let isCommonPickup = this.droppedTrip.Trip.controls['IsCommonPickup'].value as boolean;
        var locationType = this.droppedTrip.Trip.controls['PickupLocationType'].value;
        var pickupLocation = null;
        if (isCommonPickup) {
            if (locationType != 2)
                pickupLocation = this.droppedTrip.Trip.controls['Terminal'].value
            else
                pickupLocation = this.droppedTrip.Trip.controls['BulkPlant'].value;
        }
        var _drArray = this.droppedTrip.Trip.controls['DeliveryRequests'] as FormArray;
        var _drArrayValue = this.droppedTrip.Trip.controls['DeliveryRequests'].value as DeliveryRequestViewModel[];
        let isDraggedFromLoad = this.draggedDelReqData != null && this.draggedDelReqData.TripFrom != undefined && this.draggedDelReqData.TripFrom != null;
        var _drFormArray = this.draggedDelReqData.Data as FormArray;
        _drFormArray.controls.forEach((_drForm: FormGroup) => {
            if (isCommonPickup) {
                _drForm.controls['Terminal'].disable();
                _drForm.controls['BulkPlant'].disable();
            }
            else {
                var drValue = _drForm.value;
                var isDRPickupExists = (drValue.PickupLocationType != 2 && drValue.Terminal && drValue.Terminal.Id > 0) || (drValue.PickupLocationType == 2 && drValue.BulkPlant && drValue.BulkPlant.SiteName && drValue.BulkPlant.SiteName.length > 0);
                if (pickupLocation) {
                    if (!isDRPickupExists) {
                        if (locationType != 2) {
                            _drForm.controls['Terminal'].patchValue(pickupLocation);
                            _drForm.controls['PickupLocationType'].patchValue(1);
                        }
                        else {
                            _drForm.controls['BulkPlant'].patchValue(pickupLocation);
                            _drForm.controls['PickupLocationType'].patchValue(2);
                        }
                    }
                }
            }
            if (this.droppedTrip.Trip.controls['IsDispatcherDragDropSequence'].value == true) {
                var drFormJobId = _drForm.controls['JobId'].value;
                if (_drArrayValue.find(x => x.JobId == drFormJobId) != null) {
                    var jobInfoIndex = _drArrayValue.findIndex(x => x.JobId == drFormJobId);
                    _drArray.insert(jobInfoIndex, _drForm);
                }
                else {
                    _drArray.push(_drForm);
                }
            }
            else {
                _drArray.push(_drForm);
            }
        });
        if (isDraggedFromLoad) {
            var drInFromTrip = this.draggedDelReqData.TripFrom.controls['DeliveryRequests'] as FormArray;
            for (let idx = 0; idx < _drFormArray.controls.length; idx++) {
                let drIndex = drInFromTrip.controls.findIndex((x: FormGroup) => {
                    return x.controls['Id'].value == _drFormArray.controls[idx]['controls'].Id.value
                });
                _drFormArray.controls[idx].get('SourceTripId').setValue(this.draggedDelReqData.TripFrom.controls['TripId'].value);
                drInFromTrip.removeAt(drIndex);
            }
        }
        if (this.droppedTrip.Trip.controls['IsDispatcherDragDropSequence'].value == true) {
            this.droppedTrip.Trip.controls['IsDispatcherDragDropSequenceModified'].setValue(true);
        }
        this._savingBuilder = false;
        this.validateTrailerJobCompatibility(this.draggedDelReqData, this.draggedDelReqData.Schedule);
        this.draggedDelReqData.TripFrom.controls['ShiftIndex'].setValue(this.draggedDelReqData.ShiftIndex);
        this.droppedTrip.Trip.controls['ShiftIndex'].setValue(this.droppedTrip.ShiftIndex);
        this.dragDropDeliveryRequestsPublish(this.draggedDelReqData.TripFrom, this.droppedTrip.Trip, this.draggedDelReqData.Schedule, this.droppedTrip.Schedule);
        this.dataService.setScheduleChangeDetectSubject(true);
    }

    dragDropDeliveryRequestsSave(tripFrom: FormGroup, tripTo: FormGroup, tripFromSchedule: FormGroup, tripToSchedule: FormGroup): void {
        this._savingBuilder = true;
        if (tripFrom.controls['DeliveryRequests'].value.length == 0) {
            tripFrom.controls['IsCommonPickup'].setValue(false);
            tripFrom.controls['Terminal'].disable();
            tripFrom.controls['BulkPlant'].disable();
        }
        this.changeDetectorRef.detectChanges();
        var sourceTrip = tripFrom.value;
        var destTrip = tripTo.value;
        this.setTripStatus(sourceTrip);
        this.setDeliveryGroupStatus(sourceTrip, DeliveryGroupStatus.Draft);
        this.setTripStatus(destTrip);
        this.setDeliveryGroupStatus(destTrip, DeliveryGroupStatus.Draft);
        var dataToSave = this.getDSBSaveModel();
        sourceTrip.Drivers = tripFromSchedule.get('Drivers').value;
        sourceTrip.Trailers = tripFromSchedule.get('Trailers').value;
        this.setDeliveryRequestStatusAsDraft(sourceTrip.DeliveryRequests);
        dataToSave.Trips.push(sourceTrip);
        destTrip.Drivers = tripToSchedule.get('Drivers').value;
        destTrip.Trailers = tripToSchedule.get('Trailers').value;
        this.setDeliveryRequestStatusAsDraft(destTrip.DeliveryRequests);
        dataToSave.Trips.push(destTrip);
        dataToSave.Status = 5;
        this.dataService.setShowDeliveryGroupSubject(false);
        this.sbService.saveDriverView(dataToSave).subscribe(data => {
            this._savingBuilder = false;
            var trips = new FormArray([]);;
            trips.push(tripFrom);
            trips.push(tripTo);
            this.updateLoadForm(data, trips);
        });
    }

    dragDropDeliveryRequestsPublish(tripFrom: FormGroup, tripTo: FormGroup, tripFromSchedule: FormGroup, tripToSchedule: FormGroup): void {
        var isDraggedDRsPublished = this.isDraggedDRsPublished(this.draggedDelReqData.Data.value), isDestTripPublished = false;
        if (tripFrom != null && tripFrom != undefined) {
            if (tripFrom.get('GroupId').value > 0) {
                if (tripFrom && tripFrom.invalid) {
                    var i = tripFrom.get('ShiftIndex').value;
                    var j = tripFrom.get('DriverRowIndex').value;
                    var k = tripFrom.get('DriverColIndex').value;
                    if (tripFrom.get('DeliveryRequests').value.length > 0) {
                        this.editExisingGroup(tripFrom, i, j, k, tripFromSchedule, true);
                        this.dataService.setShowOpenedDeliveryGroupSubject(true);
                        return;
                    }
                    else {
                        tripFrom.controls['IsCommonPickup'].setValue(false);
                        tripFrom.controls['Terminal'].disable();
                        tripFrom.controls['BulkPlant'].disable();
                    }
                }
                else {
                    this.dataService.setShowDeliveryGroupSubject(false);
                }
            }
        }
        if (tripTo != null && tripTo != undefined) {
            if (tripTo.get('GroupId').value > 0) {
                isDestTripPublished = true;
                if (tripTo.invalid) {
                    var i = tripTo.get('ShiftIndex').value;
                    var j = tripTo.get('DriverRowIndex').value;
                    var k = tripTo.get('DriverColIndex').value;
                    if (tripTo.get('DeliveryRequests').value.length > 0) {
                        this.editExisingGroup(tripTo, i, j, k, tripToSchedule, true);
                        this.dataService.setShowOpenedDeliveryGroupSubject(true);
                        return;
                    }
                } else {
                    this.dataService.setShowDeliveryGroupSubject(false);
                }
            }
        }

        this._savingBuilder = true;
        this.changeDetectorRef.detectChanges();
        var dsbModel = this.getDSBSaveModel();
        var sourceTripValue = tripFrom.value;
        var destTripValue = tripTo.value;
        if (sourceTripValue.GroupId > 0) {
            this.setTripStatus(sourceTripValue);
            this.setDeliveryGroupStatus(sourceTripValue, DeliveryGroupStatus.Published);
        }
        else {
            this.setTripStatus(sourceTripValue);
            this.setDeliveryGroupStatus(sourceTripValue, DeliveryGroupStatus.Draft);
        }
        sourceTripValue.Drivers = tripFromSchedule.get('Drivers').value;
        sourceTripValue.Trailers = tripFromSchedule.get('Trailers').value;
        if (destTripValue.GroupId > 0) {
            this.setTripStatus(destTripValue);
            this.setDeliveryGroupStatus(destTripValue, DeliveryGroupStatus.Published);
        }
        else {
            this.setTripStatus(destTripValue);
            this.setDeliveryGroupStatus(destTripValue, DeliveryGroupStatus.Draft);
        }
        destTripValue.Drivers = tripToSchedule.get('Drivers').value;
        destTripValue.Trailers = tripToSchedule.get('Trailers').value;

        if (isDraggedDRsPublished && destTripValue.DeliveryGroupPrevStatus != 2) {
            this.setDeliveryRequestStatusAsDraft(destTripValue.DeliveryRequests);
        }
        else if (!isDraggedDRsPublished && destTripValue.DeliveryGroupPrevStatus == 2) {
            this.setDeliveryRequestStatusAsPublished(this.draggedDelReqData.Data.value, destTripValue.DeliveryRequests);
        }
        else if (isDraggedDRsPublished && destTripValue.DeliveryGroupPrevStatus == 2) {
            this.setDeliveryRequestStatusAsPublished(this.draggedDelReqData.Data.value, destTripValue.DeliveryRequests);
        }
        dsbModel.Trips.push(sourceTripValue);
        dsbModel.Trips.push(destTripValue);
        dsbModel.Status = 5;
        var tripArray = new FormArray([]);
        tripArray.push(tripFrom);
        tripArray.push(tripTo);
        if (isDraggedDRsPublished || isDestTripPublished) {

            this.publishDriverViewToAPI(dsbModel, tripArray);
        }
        else {
            this.sbService.saveDriverView(dsbModel).subscribe(data => {
                this._savingBuilder = false;
                this.updateLoadForm(data, tripArray);
            });
        }
    }

    setDeliveryRequestStatusAsDraft(deliveryRequests: DeliveryRequestViewModel[]) {
        deliveryRequests.forEach(t => {
            t.Status = 5;
            t.ScheduleStatus = 14;
            t.DeliveryGroupId = null;
            t.DeliveryScheduleId = null;
            t.TrackableScheduleId = null;
            t.TrackScheduleStatus = 0;
            t.TrackScheduleEnrouteStatus = 0;
            t.TrackScheduleStatusName = '';
            t.StatusClassId = 0;
        });
    }

    setDeliveryRequestStatusAsPublished(draggedDeliveryRequests: DeliveryRequestViewModel[], destTripDeliveryRequests: DeliveryRequestViewModel[]) {
        for (var i = 0; i < draggedDeliveryRequests.length; i++) {
            var destLoadDR = destTripDeliveryRequests.filter(t => t.Id == draggedDeliveryRequests[i].Id);
            destLoadDR.forEach(t => {
                t.Status = 3;
                t.ScheduleStatus = 14;
            });
        }
    }

    dragDelReqToAnotherLoadNo(): void {
        this.droppedTrip = null;
        this.draggedDelReqData = null;
    }

    resetLoad(trip: FormGroup) {
        if (trip) {
            if (this.routeInfo == null) {
                let reserveValue = {
                    TripId: trip.controls['TripId'].value,
                    ShiftId: trip.controls['ShiftId'].value,
                    ShiftStartTime: trip.controls['ShiftStartTime'].value,
                    ShiftEndTime: trip.controls['ShiftEndTime'].value,
                    SlotPeriod: trip.controls['SlotPeriod'].value,
                    StartDate: trip.controls['StartDate'].value,
                    StartTime: trip.controls['StartTime'].value,
                    EndTime: trip.controls['EndTime'].value,
                    Carrier: trip.controls['Carrier'].value,
                    ShiftIndex: trip.controls['ShiftIndex'].value,
                    DriverRowIndex: trip.controls['DriverRowIndex'].value,
                    DriverColIndex: trip.controls['DriverColIndex'].value,
                    TrailerRowIndex: trip.controls['TrailerRowIndex'].value,
                    TrailerColIndex: trip.controls['TrailerColIndex'].value,
                    IsEditable: true,
                    IsPreloadDisable: false
                }
                trip.reset();
                (trip.controls['DeliveryRequests'] as FormArray).clear();
                trip.reset(reserveValue);
                this.dataService.setScheduleChangeDetectSubject(true);
            }
            if (this.routeInfo != null) {
                this._loadingRemoveRoute = false;
            }
            this.changeDetectorRef.detectChanges();
        }
    }
    deleteGroup(data: any) {
        this.routeInfo = null;
        this.dataService.setShowDeliveryGroupSubject(false);
        if (data.get('GroupId').value != null && data.get('GroupId').value > 0) {
            this._savingBuilder = true;
            this.changeDetectorRef.detectChanges();
            var delRequests = data.get('DeliveryRequests') as FormArray;
            var scheduleIds = delRequests.value.filter(top => top.TrackScheduleStatus != 5).map(t => t.TrackableScheduleId);
            this.sbService.getScheduleStatus(scheduleIds).subscribe(response => {
                this._savingBuilder = false;
                if (response != null && response != undefined && response.length > 0) {
                    var completedSchedules = this.sbService.returnCommonElements(this.CompletedScheduleStatuses, response, true);
                    var isCompletedSchedule = completedSchedules.length > 0;
                    var onTheWaySchedules = this.sbService.returnCommonElements(this.OnTheWayScheduleStatuses, response, false);
                    if (isCompletedSchedule || response.filter(t => t.ScheduleEnrouteStatusId == 4).length > 0) {
                        var scheduleStatuses = this.sbService.returnCommonElements(this.ScheduleStatuses, response, true);
                        if (scheduleStatuses.length > 0) {
                            this.deleteLoad(data);
                        }
                        else {
                            Declarations.msgerror("Can't delete/reset group. For one or more schedule(s) drop has been made already", 'Warning', 2500);
                            return;
                        }
                    } else if (onTheWaySchedules.length > 0) {
                        this.sbService.setConfirmationHeadingForDeleteGroup(onTheWaySchedules[0]);
                        this.DeletedGroup = data;
                        jQuery('#btnconfirm-delete-delgrp').click();
                        return;
                    } else {
                        this.deleteLoad(data);
                    }
                } else {
                    this.deleteLoad(data);
                }
                //this.changeDetectorRef.detectChanges();
            });
        }
        else {
            this.deleteLoad(data);
        }
    }

    public deleteLoadYes() {
        Declarations.hideModal('#confirm-delete-delgrp');
        if (this.routeInfo == null) {
            this.deleteLoad(this.DeletedGroup);
        }
        else {
            this.deleteRouteLoad(this.DeletedGroup);
        }
        this.DeletedGroup = null;
    }

    public deleteLoadNo() {
        Declarations.hideModal('#confirm-delete-delgrp');
        this.DeletedGroup = null;
        if (this.routeInfo != null) {
            this._loadingRemoveRoute = false;
            this.changeDetectorRef.detectChanges();
        }
    }

    deleteLoad(trip: FormGroup) {
        var tripId = trip.controls['TripId'].value;
        var delRequests = trip.controls['DeliveryRequests'] as FormArray;
        delRequests.value.forEach(t => { t.WindowMode = 1; t.QueueMode = 1; t.Compartments = [] });
        if (tripId != null && tripId != undefined && tripId != '') {            
            var dsbModel = this.getDSBSaveModel();
            dsbModel.DeletedGroupId = trip.controls['GroupId'].value;
            dsbModel.DeletedTripId = tripId;
            let preloadInfo = this.getPreloadedInfoFromLoad(trip);
            let preloadedTrips = this.getPrePostLoadedTrips(preloadInfo);
            let postloadInfo = this.getPostloadedInfoFromLoad(trip);
            let postloadedTrips = this.getPrePostLoadedTrips(postloadInfo);
            dsbModel.PreloadedDRs = this.getPreloadAcrossTheDateInfoFromLoad(trip);
            dsbModel.PostloadedDRs = this.getPostloadAcrossTheDateInfoFromLoad(trip);
            var schedule = this.SbForm.get('Shifts.' + trip.controls['ShiftIndex'].value + '.Schedules.' + trip.controls['DriverRowIndex'].value) as FormGroup;
            trip.value.Drivers = schedule.controls['Drivers'].value;
            trip.value.Trailers = schedule.controls['Trailers'].value;
            dsbModel.Trips.push(trip.value);
            dsbModel.Status = 4;

            let tripArray = new FormArray([]);;
            tripArray.push(trip);
            preloadedTrips.forEach(x => {
                dsbModel.Trips.push(x.value);
                tripArray.push(x);
            });
            postloadedTrips.forEach(x => {
                dsbModel.Trips.push(x.value);
                tripArray.push(x);
            });
            this._savingBuilder = true;
            this.sbService.deleteGroup(dsbModel).subscribe(response => {
                if (response != null) {
                    if (response.StatusCode == 0 || response.StatusCode == 2) {
                        if (response.Trips && response.Trips.length > 0) {
                            for (var i = 0; i < response.Trips.length; i++) {
                                var tripDrs = response.Trips[i].DeliveryRequests.map(t => t.Id);
                                let drsToRestore = delRequests.value.filter(t => tripDrs.indexOf(t.Id) == -1);
                                this.dataService.setRestoreDeletedRequestSubject(drsToRestore);
                            }
                        }
                    }
                    this.updateLoadForm(response, tripArray);
                }
                else {
                    Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
                }
                this._savingBuilder = false;
            });
        } else {
            this.dataService.setRestoreDeletedRequestSubject(delRequests.value);
            this.resetLoad(trip);
            this.dataService.setScheduleChangeDetectSubject(true);
        }
    }

    private getPreloadedInfoFromLoad(trip: FormGroup): LoadInfo[] {
        let _drs = trip.controls['DeliveryRequests'].value;
        let preloadInfo = _drs.filter(x => x.PreLoadInfo).map(m => m.PreLoadInfo);
        return preloadInfo;
    }

    private getPostloadedInfoFromLoad(trip: FormGroup): LoadInfo[] {
        let _drs = trip.controls['DeliveryRequests'].value;
        let postloadInfo = _drs.filter(x => x.PostLoadInfo).map(m => m.PostLoadInfo);
        return postloadInfo;
    }

    private getPreloadAcrossTheDateInfoFromLoad(trip: FormGroup): any[] {
        let _drs = trip.controls['DeliveryRequests'].value;
        let preloadInfo = _drs.filter(x => !x.PostLoadInfo && x.PostLoadedFor && !x.IsRetainFuelLoaded).map(m => m.PostLoadedFor);
        preloadInfo = preloadInfo.filter((value, index, self) => self.indexOf(value) === index);
        return preloadInfo;
    }

    private getPostloadAcrossTheDateInfoFromLoad(trip: FormGroup): any[] {
        let _drs = trip.controls['DeliveryRequests'].value;
        let preloadInfo = _drs.filter(x => !x.PreLoadInfo && x.PreLoadedFor).map(m => m.PreLoadedFor);
        return preloadInfo;
    }

    private getPrePostLoadedTrips(loadInfo: LoadInfo[]): FormGroup[] {
        let loadedTrips = [];
        loadInfo.forEach(x => {
            let thisShift = this.SbForm.controls['Shifts']['controls'].find((f: FormGroup) => f.controls['Id'].value == x.ShiftId) as FormGroup;
            if (thisShift) {
                let thisSchedule = thisShift.get('Schedules.' + x.ScheduleIndex) as FormGroup;
                let thisTrip = thisSchedule.get("Trips." + x.TripIndex) as FormGroup;
                if (thisTrip) {
                    thisTrip.value.Drivers = thisSchedule.controls['Drivers'].value;
                    thisTrip.value.Trailers = thisSchedule.controls['Trailers'].value;
                    loadedTrips.push(thisTrip);
                }
            }
        });
        loadedTrips = loadedTrips.filter((value, index, self) => self.indexOf(value) === index);
        return loadedTrips;
    }

    getAssignedDrivers(shiftIdx: number): DriverAdditionalDetailModel[] {
        var _drivers = [];
        var _shift = this.SbForm.controls['Shifts']['controls'][shiftIdx] as FormGroup;
        if (_shift != null && _shift != undefined) {
            var _schArray = _shift.controls['Schedules'] as FormArray;
            _schArray.controls.forEach((sc: FormGroup) => {
                var _dArray = sc.controls['Drivers'] as FormArray;
                if (_dArray && _dArray.length > 0) {
                    _drivers = _drivers.concat(_dArray.value);
                }
            });
        }
        return _drivers;
    }

    getAssignedTrailers(shiftIdx: number): TrailerViewModel[] {
        var _trailers = [];
        var _shift = this.SbForm.get('Shifts.' + shiftIdx) as FormGroup;
        if (_shift != null && _shift != undefined) {
            var _schArray = _shift.get('Schedules') as FormArray;
            _schArray.controls.forEach(sc => {
                var _tArray = sc.get('Trailers') as FormArray;
                if (_tArray && _tArray.length > 0) {
                    _trailers = _trailers.concat(_tArray.value);
                }
            });
        }
        return _trailers;
    }

    getUnassignedDrivers(isFilldToggle = false): DriverAdditionalDetailModel[] {
        let currentDriverId = this.DriverTrailerForm.controls['Driver'].value;
        let drivers = this.AllUnassignedDrivers;
        if (drivers && drivers.length > 0) {
            this._loadingDrivers = true;
            let shiftIdx = this.DriverTrailerForm.controls['Index1'].value;
            let assigned = this.getAssignedDrivers(shiftIdx);
            if (!isFilldToggle) {
                assigned = assigned.filter(x => x.Id != currentDriverId);
                drivers = drivers.filter(this.IdNotInComparer(assigned));
            }
            let isFilldCompatible = this.DriverTrailerForm.get('IsFilldCompatible').value;
            if (isFilldCompatible != undefined) {
                assigned = assigned.filter(x => x.Id != currentDriverId && x.IsFilldCompatible == isFilldCompatible);
                drivers = drivers.filter(t => t.IsFilldCompatible == isFilldCompatible);
                drivers = drivers.filter(this.IdNotInComparer(assigned));
            }
            this._loadingDrivers = false;
        }
        return drivers;
    }

    getUnassignedTrailers(shiftIdx: number, currentTrailers: TrailerViewModel[], IsFilldToggle: boolean = false): TrailerViewModel[] {
        let _trailers = this.RegionDetail.Trailers;
        if (!IsFilldToggle) {
            let assignedTrailers = this.getAssignedTrailers(shiftIdx);
            assignedTrailers = assignedTrailers.filter(this.IdNotInComparer(currentTrailers));
            _trailers = _trailers.filter(this.IdNotInComparer(assignedTrailers));
        }
        let isFilldCompatible = this.DriverTrailerForm.get('IsFilldCompatible').value;
        if (isFilldCompatible != undefined) {
            _trailers = _trailers.filter(t => t.IsFilldCompatible == isFilldCompatible);
        }
        return _trailers;
    }

    editDriverTrailers(data: any): void {
        //this.changeDetectorRef.detach();
        this.EditDriverData = data;
        var driverId = data.Driver;
        var trailers = data.Trailers;

        this.DriverTrailerForm.patchValue({
            Index1: data.Index1,
            Index2: data.Index2,
            Driver: driverId,
            Trailers: trailers,
            IsIncludeAllRegionDriver: data.IsIncludeAllRegionDriver
        });
        this.SelectedTrailers = trailers;
        if (this.SelectedTrailers.length > 0) {
            let trailer1 = trailers[0];
            this.DriverTrailerForm.get('IsFilldCompatible').patchValue(trailer1.IsFilldCompatible);
        }
        this.selTrailerIndex = data.Index1;
        this.selTrailerlist = trailers;
        this.UnassignedTrailers = this.getUnassignedTrailers(data.Index1, trailers);
        this._loadingDrivers = true;
        this.getDriverdetails();
        this.changeDetectorRef.detectChanges();
        // this._otherRegionDriver = false;
        this._otherRegionDriverSubject.next(false);
        this._publishedRequestExists = false;
        //this.subscribeFormChange();
    }

    checkOtherRegionDriver(): void {
        if (this.DriverTrailerForm.get('Driver').value == "null") {
            this.DriverTrailerForm.get('Driver').setErrors({ 'incorrect': true });
        }
        this.DriverTrailerForm.markAllAsTouched();

        if (this.DriverTrailerForm.valid) {
            var _form = this.DriverTrailerForm.value;
            var status = this.validatePrePostLoadTrailer(_form);
            if (status) {
                var shiftId = this.SbForm.get('Shifts.' + _form.Index1 + '.Id').value;
                //newly added
                var driverObj = this.RegionDetail.Drivers.find(x => x.Id == _form.Driver);

                if (this.DriverTrailerForm.get('IsIncludeAllRegionDriver').value) {
                    var assignedDrivers = this.AllUnassignedDrivers.find(t => t.Id == _form.Driver);
                    driverObj = assignedDrivers;
                }

                this.dataService.IsLoadingButtonSubject.next(true);
                this.sbService.getSelectedDateDriverScheduleByDriverIdGridView(_form.Driver, new Date(this.SbForm.controls.Date.value).toUTCString(), shiftId).subscribe(data => {
                    if (data && data.filter(f => f.ShiftId == shiftId).length > 0) {
                        var schedule = this.SbForm.get('Shifts.' + _form.Index1 + '.Schedules.' + _form.Index2 + '') as FormGroup;
                        if (!this.EditDriverData.IsPublishedRequestsExists) {
                            this.assignDriverToSchedule(driverObj, schedule, _form);
                        }
                        else {
                            var schedule = this.SbForm.get('Shifts.' + _form.Index1 + '.Schedules.' + _form.Index2 + '') as FormGroup;
                            var trips = schedule.controls['Trips'] as FormArray;
                            var _deliveryRequests = this.GetAllLoadsDR(trips);
                            var scheduleIds: number[] = this.getPublishedDRsTrackableScheduleIds(_deliveryRequests);
                            this.sbService.getScheduleStatus(scheduleIds).subscribe(response => {
                                if (response != null && response != undefined && response.length > 0) {
                                    let isDriverChange = false, isTrailerChange = false;
                                    let _selectedTrailers = _form.Trailers.map(y => y.Id); var trips = schedule.controls['Trips'] as FormArray;
                                    let currentTrailers = schedule.get('Trailers').value.map(y => y.Id);
                                    let currentDriver = schedule.get('Drivers').value;
                                    isTrailerChange = currentTrailers.filter(x => {
                                        return _selectedTrailers.find(y => y == x) != undefined;
                                    }).length != currentTrailers.length || (currentTrailers.length == 0 && _selectedTrailers.length > 0);
                                    isDriverChange = (currentDriver.length == 0 && driverObj.Id > 0) || driverObj.Id != currentDriver[0].Id;
                                    var completedSchedules = this.sbService.returnCommonElements(this.CompletedScheduleStatuses, response, true);
                                    var isCompletedSchedule = completedSchedules.length > 0;
                                    var onTheWaySchedules = this.sbService.returnCommonElements(this.OnTheWayScheduleStatuses, response, false);
                                    if (isCompletedSchedule || response.filter(t => t.ScheduleEnrouteStatusId == 4).length > 0) {
                                        Declarations.msgerror("Can't assign driver/trailer for this column as drop has been made already", 'Warning', 2500);
                                        this.dataService.IsLoadingButtonSubject.next(false);
                                        return;
                                    } else if (isDriverChange && onTheWaySchedules.length > 0) {
                                        Declarations.msgerror("Can't assign driver for this column as driver is on the way to job", 'Warning', 2500);
                                        this.dataService.IsLoadingButtonSubject.next(false);
                                        return;
                                    }
                                    else if (isDriverChange && _deliveryRequests.filter(t => t.TrackScheduleStatus == 23).length > 0) {
                                        Declarations.msgerror("Can't assign driver for this column as driver accepted the schedule.", 'Warning', 2500);
                                        this.dataService.IsLoadingButtonSubject.next(false);
                                        return;
                                    }
                                    else {
                                        this.assignDriverToSchedule(driverObj, schedule, _form);
                                        this.dataService.IsLoadingButtonSubject.next(false);
                                    }
                                }
                            });
                        }
                    } else {
                        var schedule = this.SbForm.get('Shifts.' + _form.Index1 + '.Schedules.' + _form.Index2 + '') as FormGroup;
                        if (!this.EditDriverData.IsPublishedRequestsExists || this.DriverTrailerForm.get('IsIncludeAllRegionDriver').value) {
                            this.assignDriverToSchedule(driverObj, schedule, _form);
                        }
                        else if (this.EditDriverData.IsPublishedRequestsExists) {
                            var schedule = this.SbForm.get('Shifts.' + _form.Index1 + '.Schedules.' + _form.Index2 + '') as FormGroup;

                            let isDriverChange = false, isTrailerChange = false;
                            let _selectedTrailers = _form.Trailers.map(y => y.Id); var trips = schedule.controls['Trips'] as FormArray;
                            let currentTrailers = schedule.get('Trailers').value.map(y => y.Id);
                            let currentDriver = schedule.get('Drivers').value;
                            isTrailerChange = currentTrailers.filter(x => {
                                return _selectedTrailers.find(y => y == x) != undefined;
                            }).length != currentTrailers.length || (currentTrailers.length == 0 && _selectedTrailers.length > 0);
                            isDriverChange = (currentDriver.length == 0 && driverObj.Id > 0) || driverObj.Id != currentDriver[0].Id;
                            var _deliveryRequests = this.GetAllLoadsDR(trips);
                            var scheduleIds: number[] = this.getPublishedDRsTrackableScheduleIds(_deliveryRequests);
                            this.sbService.getScheduleStatus(scheduleIds).subscribe(response => {
                                if (response != null && response != undefined && response.length > 0) {
                                    var completedSchedules = this.sbService.returnCommonElements(this.CompletedScheduleStatuses, response, true);
                                    var isCompletedSchedule = completedSchedules.length > 0;
                                    var onTheWaySchedules = this.sbService.returnCommonElements(this.OnTheWayScheduleStatuses, response, false);
                                    if (isCompletedSchedule || response.filter(t => t.ScheduleEnrouteStatusId == 4).length > 0) {
                                        Declarations.msgerror("Can't assign driver/trailer for this column as drop has been made already", 'Warning', 2500);
                                        this.dataService.IsLoadingButtonSubject.next(false);
                                        return;
                                    } else if (onTheWaySchedules.length > 0 && isDriverChange) {
                                        Declarations.msgerror("Can't assign driver for this column as driver is on the way to job", 'Warning', 2500);
                                        this.dataService.IsLoadingButtonSubject.next(false);
                                        return;
                                    }
                                    else if (isDriverChange && _deliveryRequests.filter(t => t.TrackScheduleStatus == 23).length > 0) {
                                        Declarations.msgerror("Can't assign driver for this column as driver accepted the schedule.", 'Warning', 2500);
                                        this.dataService.IsLoadingButtonSubject.next(false);
                                        return;
                                    }
                                    else {
                                        this.assignDriverToSchedule(driverObj, schedule, _form);
                                        this.dataService.IsLoadingButtonSubject.next(false);
                                    }
                                }
                            });
                        }
                        //this._otherRegionDriverSubject.next(true);//never come due to driver's load based on shift.

                    }
                    this.dataService.IsLoadingButtonSubject.next(false);
                });
            }
        }
    }

    checkForPublishedRequests(schedule: FormGroup): void {
        //this._otherRegionDriver = false;
        this._otherRegionDriverSubject.next(false);
        if (this.checkAnyPublishedTrip(schedule)) {
            this._publishedRequestExists = true;
        } else {
            this._publishedRequestExists = false;
        }
    }

    editDriverCancel(): void {
        if (this.driverSchedules) {
            this.driverSchedules.forEach(t => t.unsubscribeUpdateDriverTrailerSubject());
        }
    }

    onPublishChangesNo(): void {
        this._publishedRequestExists = false;
    }

    onPublishChangesYes(): void {
        var _form = this.DriverTrailerForm.value;
        var schedule = this.SbForm.get('Shifts.' + _form.Index1 + '.Schedules.' + _form.Index2 + '') as FormGroup;
        if (schedule != null && schedule != undefined) {
            this.EditDriverData.IsPublishedRequestsExists = false;
            var driverObj = this.RegionDetail.Drivers.find(x => x.Id == _form.Driver);
            this.assignDriverToSchedule(driverObj, schedule, _form);
        }
    }


    onOtherRegionDriverNo(): void {
        //this._otherRegionDriver = false;
        this._otherRegionDriverSubject.next(false);
    }

    onOtherRegionDriverYes(): void {
        if (this.DriverTrailerForm.valid) {
            var _form = this.DriverTrailerForm.value;
            var schedule = this.SbForm.get('Shifts.' + _form.Index1 + '.Schedules.' + _form.Index2 + '') as FormGroup;
            if (schedule != null && schedule != undefined) {
                var driverObj = this.RegionDetail.Drivers.find(x => x.Id == _form.Driver);
                //this.checkForPublishedRequests(schedule);
                this._otherRegionDriverSubject.next(false);
                this._publishedRequestExists = this.EditDriverData.IsPublishedRequestsExists;
                if (!this.EditDriverData.IsPublishedRequestsExists) {
                    this.assignDriverToSchedule(driverObj, schedule, _form);
                    let driverInfo = {
                        driverId: driverObj.Id,
                        regionId: this.SelectedRegionId
                    };
                    this.chatService.intializeChatDefault(driverInfo);
                }
            }
        }
    }

    GetAllLoadsDR(trips: FormArray) {
        var _deliveryRequests = [];
        if (trips) {
            for (var i = 0; i < trips.length; i++) {
                var trip = trips.controls[i] as FormGroup;
                var deliveryRequests = trip.controls["DeliveryRequests"].value;
                if (deliveryRequests) {
                    for (var j = 0; j < deliveryRequests.length; j++) {
                        var deliveryRequest = deliveryRequests[j];
                        if (deliveryRequest) {
                            _deliveryRequests.push(deliveryRequest);
                        }
                    }
                }
            }
        }
        return _deliveryRequests;
    }

    allowDriverChange(data, driverObj: DriverAdditionalDetailModel) {
        var isAllow = true;
        if (data && data.deliverySchedules && data.deliverySchedules.length > 0) {
            if (driverObj) {
                if (data.deliverySchedules.find(driverId => driverObj.Id != driverId)) {
                    isAllow = false;
                }
            }
        }
        return isAllow;
    }

    assignDriverToSchedule(driverObj: DriverAdditionalDetailModel, schedule: FormGroup, _form: any): void {
        var trips = schedule.controls['Trips'] as FormArray;
        schedule.get('IsIncludeAllRegionDriver').patchValue(_form.IsIncludeAllRegionDriver);
        let _selectedTrailers = this.RegionDetail.Trailers.filter(x => {
            return _form.Trailers.find(y => y.TrailerId == x.TrailerId) != undefined;
        });
        var _deliveryRequests = this.GetAllLoadsDR(trips);
        // Validate Delivery Requests with trailer type
        if (_deliveryRequests && _deliveryRequests.length > 0) {
            this.sbService.validateTrailerJobCompatibility(_selectedTrailers, _deliveryRequests)
                .subscribe(data => {

                    if (data && data.deliveryRequestsNotCompatible && data.deliveryRequestsNotCompatible.length > 0) {
                        this.highLightDRDiv(trips, data);
                        Declarations.hideModal('#driverTrailerModel');
                        Declarations.msgerror("This job location is not compatible with trailer type", undefined, undefined);
                        this.editDriverCancel();

                    } else if (!this.allowDriverChange(data, driverObj)) {
                        Declarations.msgerror("The user cannot change the driver once drop is done", undefined, undefined);
                        this.editDriverCancel();
                    }
                    else {
                        this.assignDriverToScheduleSave(driverObj, schedule, _form);
                        this.highLightDRDiv(trips, null);
                    }
                    if (_selectedTrailers.length > 0) {
                        var trailerIds = [];
                        _selectedTrailers.forEach(x => {
                            trailerIds.push(x.Id);
                        });
                        this.GetfuelRetainDetails(trailerIds);
                    }
                });
        } else {
            this.assignDriverToScheduleSave(driverObj, schedule, _form);
            this.highLightDRDiv(trips, null);
            if (_selectedTrailers.length > 0) {
                var trailerIds = [];
                _selectedTrailers.forEach(x => {
                    trailerIds.push(x.Id);
                });
                this.GetfuelRetainDetails(trailerIds);
            }
        }
    }

    assignDriverToScheduleSave(driverObj: DriverAdditionalDetailModel, schedule: FormGroup, _form: any): void {
        Declarations.hideModal('#driverTrailerModel');
        let _selectedTrailers = this.RegionDetail.Trailers.filter(x => {
            return _form.Trailers.find(y => y.TrailerId == x.TrailerId) != undefined;
        });
        let data = {
            Index1: _form.Index1,
            Index2: _form.Index2,
            Driver: driverObj,
            Trailers: _selectedTrailers,
            IsIncludeAllRegionDriver: _form.IsIncludeAllRegionDriver
        };
        this.dataService.setUpdateDriverTrailerSubject(data);
    }

    public saveDriverAssignment(shiftIndex: number, scheduleIndex: number): void {
        var schedule = this.SbForm.get('Shifts.' + shiftIndex + '.Schedules.' + scheduleIndex) as FormGroup;
        var drivers = schedule.get('Drivers').value;
        var trailers = schedule.get('Trailers').value;
        var trips = this.SbForm.get('Shifts.' + shiftIndex + '.Schedules.' + scheduleIndex + '.Trips') as FormArray;

        this._savingBuilder = true;
        this.changeDetectorRef.detectChanges();
        var dataToSave = this.getDSBSaveModel();
        for (var i = 0; i < trips.length; i++) {
            trips.value[i].Drivers = drivers;
            trips.value[i].Trailers = trailers;
            trips.value[i].IsIncludeAllRegionDriver = schedule.get('IsIncludeAllRegionDriver').value;
            dataToSave.Trips.push(trips.value[i]);
        }
        dataToSave.Status = 2;
        this.dataService.setShowDeliveryGroupSubject(false);
        this._savingBuilder = true;
        this.sbService.assignDriverAndTrailer(dataToSave).subscribe(data => {
            this._savingBuilder = false;
            this.updateLoadsFromRow(data, trips);
        });
    }

    getDSBSaveModel() {
        var sbModel = this.SbForm.value;
        var dataToSave = new DSBSaveModel();
        dataToSave.Id = sbModel.Id;
        dataToSave.Date = sbModel.Date;
        dataToSave.RegionId = sbModel.RegionId;
        dataToSave.ObjectFilter = sbModel.ObjectFilter;
        dataToSave.RegionFilter = sbModel.RegionFilter;
        dataToSave.DateFilter = sbModel.DateFilter;
        dataToSave.DSBFilter = sbModel.DSBFilter;
        dataToSave.TimeStamp = sbModel.TimeStamp;
        dataToSave.Status = sbModel.Status;
        dataToSave.WindowMode = sbModel.WindowMode;
        dataToSave.ToggleRequestMode = sbModel.ToggleRequestMode;
        if (sbModel.Id == null) {
            for (var i = 0; i < sbModel.Shifts.length; i++) {
                var shift = new ScheduleShiftModel();
                shift.Id = sbModel.Shifts[i].Id;
                shift.StartTime = sbModel.Shifts[i].StartTime;
                shift.EndTime = sbModel.Shifts[i].EndTime;
                shift.SlotPeriod = sbModel.Shifts[i].SlotPeriod;
                dataToSave.Shifts.push(shift);
            }
        }
        return dataToSave;
    }

    setTripStatus(trip: TripModel): void {
        if (trip) {
            var tripPrevStatusId = trip.TripPrevStatus;
            var tripStatusId = TripStatus.Added;
            if (tripPrevStatusId == TripStatus.None) {
                tripStatusId = TripStatus.Added;
            } else if (tripPrevStatusId == TripStatus.Added || tripPrevStatusId == TripStatus.Modified) {
                tripStatusId = TripStatus.Modified;
            }
            trip.TripStatus = tripStatusId;
        }
    }

    setDeliveryGroupStatus(trip: TripModel, statusId: DeliveryGroupStatus): void {
        if (trip) {
            trip.DeliveryGroupStatus = statusId;
        }
    }

    setDeliveryRequestStatus(trip: TripModel, statusId: DeliveryReqStatus, updateScheduleStatus: boolean = false): void {
        if (trip) {
            var deliveryRequests = trip.DeliveryRequests;
            for (var i = 0; i < deliveryRequests.length; i++) {
                deliveryRequests[i].Status = statusId;
                //let isCompletedDrop = deliveryRequests.controls[i].get('StatusClassId').value == 4;
                if (updateScheduleStatus) {
                    var scheduleStatus = deliveryRequests[i].ScheduleStatus;
                    if (scheduleStatus == 5) {
                        continue;
                    }
                    else if (scheduleStatus == 14) {
                        deliveryRequests[i].ScheduleStatus = 15;
                    }
                    else {
                        deliveryRequests[i].ScheduleStatus = 14;
                    }
                }
            }
        }
    }

    private processPostloadedDrToSaveEditedData(postloadInfo: LoadInfo[], modifiedTrips: ModifiedTripInfo[]): void {
        let shifts = this.SbForm.controls['Shifts'].value;
        let postloadInfoClone = JSON.parse(JSON.stringify(postloadInfo)) as LoadInfo[];
        postloadInfoClone.forEach(f => { f.ShiftIndex = shifts.indexOf(shifts.find(t => t.Id == f.ShiftId)), f.DrId = ''; f.ShiftId = '' });
        let postloadInfoArray: LoadInfo[] = [];
        postloadInfoClone.forEach(t => {
            if (!postloadInfoArray.find(x => x.ShiftIndex == t.ShiftIndex && x.ScheduleIndex == t.ScheduleIndex && x.TripIndex == t.TripIndex)) {
                postloadInfoArray.push({ ShiftIndex: t.ShiftIndex, ScheduleIndex: t.ScheduleIndex, TripIndex: t.TripIndex, DrId: '', ShiftId: t.ShiftId });
            }
        });
        for (var index = 0; index < postloadInfoArray.length; index++) {
            let modifiedPreloadedTripInfo = {
                ShiftIndex: postloadInfoArray[index].ShiftIndex,
                DriverRowIndex: postloadInfoArray[index].ScheduleIndex,
                DriverColIndex: postloadInfoArray[index].TripIndex,
                ShiftId: postloadInfoArray[index].ShiftId,
            };
            modifiedTrips.push(modifiedPreloadedTripInfo);
        }
    }

    draftScheduleBuilder(trip: FormGroup, filterChanged = false): void {
        let shiftIndex = trip.controls['ShiftIndex'].value;
        let shiftId = trip.controls['ShiftId'].value;
        let rowIndex = trip.controls['DriverRowIndex'].value;
        let loadIndex = trip.controls['DriverColIndex'].value;
        let modifiedTrips = [{ ShiftIndex: shiftIndex, DriverRowIndex: rowIndex, DriverColIndex: loadIndex, ShiftId: shiftId }] as ModifiedTripInfo[];
        let postloadInfo = this.getPostloadedInfoFromLoad(trip);
        if (postloadInfo && postloadInfo.length > 0) {
            this.processPostloadedDrToSaveEditedData(postloadInfo, modifiedTrips);
        }
        this.draftScheduleBuilderData(modifiedTrips, filterChanged)
    }

    public draftScheduleBuilderData(_unsavedChanges: ModifiedTripInfo[], isDateChange: boolean) {
        if (_unsavedChanges.length > 0) {
            var isValidTrips = this.validateTrips(_unsavedChanges);
            if (!isValidTrips) {
                return;
            }
            this._savingBuilder = true;
            let trips = new FormArray([]);
            var dataToSave = this.getDSBSaveModel();
            _unsavedChanges.forEach(t => {
                let shiftInfo = this.SbForm.controls['Shifts']['controls'].find((f: FormGroup) => f.controls['Id'].value == t.ShiftId) as FormGroup;
                if (shiftInfo != null) {
                    let schedule = shiftInfo.controls['Schedules']['controls'][t.DriverRowIndex] as FormGroup;
                    var trip = schedule.get('Trips.' + t.DriverColIndex) as FormGroup;
                    trips.push(trip);
                    var tripModel = trip.value;
                    this.setTripStatus(tripModel);
                    this.setDeliveryGroupStatus(tripModel, DeliveryGroupStatus.Draft);
                    tripModel.Drivers = schedule.controls['Drivers'].value;
                    tripModel.Trailers = schedule.controls['Trailers'].value;
                    dataToSave.Trips.push(tripModel);
                }
            });
            this.changeDetectorRef.detectChanges();
            this.dataService.setShowDeliveryGroupSubject(false);
            if (dataToSave.Trips.length > 0) {
                dataToSave.Status = 1;
                this.sbService.saveDriverView(dataToSave).subscribe(data => {
                    this._savingBuilder = false;
                    this.updateLoadForm(data, trips, isDateChange);
                });
            }
        }
    }

    setAllowDriverChange(i: number, j: number) {
        let _thisRow = this.SbForm.get('Shifts.' + i + '.Schedules.' + j) as FormGroup;
        var schedule = _thisRow.value as TrailerModel;
        var allowDriverchange = true;
        for (var k = 0; k < schedule.Trips.length; k++) {
            let thisTrip = schedule.Trips[k] as TripModel;
            if (thisTrip.DeliveryGroupStatus == DeliveryGroupStatus.Published || thisTrip.DeliveryGroupPrevStatus == DeliveryGroupStatus.Published) {
                allowDriverchange = false;
                this.SbForm.get('Shifts.' + i + '.Schedules.' + j).get('AllowDriverChange').setValue(allowDriverchange);
                return;
            }
            var deliveryReqs = thisTrip.DeliveryRequests as DeliveryRequestViewModel[];
            if (deliveryReqs.findIndex(t => (t.PreLoadedFor != null && t.PreLoadedFor.trim() != '') || (t.PostLoadedFor != null && t.PostLoadedFor.trim() != '')) != -1) {
                allowDriverchange = false;
                this.SbForm.get('Shifts.' + i + '.Schedules.' + j).get('AllowDriverChange').setValue(allowDriverchange);
                return;
            }
        }
        this.SbForm.get('Shifts.' + i + '.Schedules.' + j).get('AllowDriverChange').setValue(allowDriverchange);
    }

    cancelScheduleBuilder(i: number, j: number, k: number, trip: FormGroup): void {
        let _thisTrip = this.SbForm.get('Shifts.' + i + '.Schedules.' + j + '.Trips.' + k) as FormGroup;
        let _thisDrArray = _thisTrip.get('DeliveryRequests') as FormArray;
        _thisDrArray.clear();
        let oldTripValue = trip.value;
        trip.value.DeliveryRequests.forEach(x => {
            _thisDrArray.push(this.utilService.getDeliveryRequestForm(x, oldTripValue.IsCommonPickup));
        });
        _thisTrip.patchValue(trip.value);
        this.changeDetectorRef.detectChanges();
    }

    publishScheduleBuilder(i: number, j: number, k: number, schedule: any, trip: FormGroup, isOptionalPickup: boolean = false): void {
        this._savingBuilder = true;
        var drivers = schedule.get('Drivers').value;
        var trailers: TrailerViewModel[] = schedule.get('Trailers').value;
        if (drivers == null || drivers == undefined || drivers.length == 0 || ((this.IsTrailerExists) && (trailers == null || trailers == undefined || trailers.length == 0))) {
            if (this.IsTrailerExists) {
                Declarations.msgwarning('Please assign a driver / trailer to publish Load ' + (k + 1), 'Driver/Trailer Required', 2500);
            }
            else {
                Declarations.msgwarning('Please assign a driver to publish Load ' + (k + 1), 'Driver Required', 2500);
            }
            this._savingBuilder = false;
            return;
        }
        if (drivers != null && drivers.length > 0) {
            var delReqs = trip.get('DeliveryRequests').value as DeliveryRequestViewModel[];
            if (delReqs.length > 0 && delReqs.some(t => t.IsFilldInvoke == true)) {
                if (this.IsTrailerExists && trailers.some(t => t.IsFilldCompatible == false)) {
                    Declarations.msgwarning('Please assign a Filld compatible driver / trailer to publish Load ' + (k + 1), 'Driver/Trailer Required', 2500);
                    this._savingBuilder = false;
                    return;
                }
                else if (drivers.some(t => t.IsFilldCompatible == false)) {
                    Declarations.msgwarning('Please assign a Filld compatible driver to publish Load ' + (k + 1), 'Driver Required', 2500);
                    this._savingBuilder = false;
                    return;
                }
            }
        }
        if (isOptionalPickup) {
            let isValid = true;
            var delControlReqs = trip.controls['DeliveryRequests'] as FormArray;
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
            if (isValid) {
                delControlReqs.controls.forEach((x: FormGroup) => {
                    this.setDRPickupValidators(x);
                });
            }
        }
        if (trip != null && trip != undefined) {
            if (trip.invalid || !this.validatePublishLoad(trip)) {
                this.editExisingGroup(trip, i, j, k, schedule, true);
                this.dataService.setShowOpenedDeliveryGroupSubject(true);
                this._savingBuilder = false;
                return;
            } else {
                this.dataService.setShowDeliveryGroupSubject(false);
            }
        }
        //Check Job and Trailer Compatibility
        var _deliveryRequests = trip.get('DeliveryRequests').value as DeliveryRequestViewModel[];
        let _selectedTrailers = this.RegionDetail.Trailers.filter(x => {
            return trailers.find(y => y.TrailerId == x.TrailerId) != undefined;
        });

        if (!this.validateLoadWithTrailerCapacity(_deliveryRequests, _selectedTrailers)) {
            this._savingBuilder = false;
            return;
        }

        if (_deliveryRequests && _deliveryRequests.length > 0) {
            this.sbService.validateTrailerJobCompatibility(_selectedTrailers, _deliveryRequests)
                .subscribe(data => {
                    if (data) {
                        if (data.trackableScheduleStatuses && data.trackableScheduleStatuses.length > 0) {
                            var completedSchedules = this.sbService.returnCommonElements(this.CompletedScheduleStatuses, data.trackableScheduleStatuses, true);
                            var isCompletedSchedule = completedSchedules.length > 0;
                            if (isCompletedSchedule || data.trackableScheduleStatuses.filter(t => t.ScheduleEnrouteStatusId == 4).length > 0) {
                                var scheduleStatuses = this.sbService.returnCommonElements(this.ScheduleStatuses, data.trackableScheduleStatuses, true);
                                if (scheduleStatuses.length == 0) {
                                    this._savingBuilder = false;
                                    this.changeDetectorRef.detectChanges();
                                    Declarations.msgerror("Can't edit group. For one or more schedule(s) drop has been made already", 'Warning', 2500);
                                    return;
                                }
                            }
                        }
                        if (data.deliveryRequestsNotCompatible && data.deliveryRequestsNotCompatible.length > 0) {
                            this.highLightDRDivOneLoad(trip, data);
                            this._savingBuilder = false;
                            this.changeDetectorRef.detectChanges();
                            Declarations.msgerror("This job location is not compatible with trailer type", undefined, undefined);
                            return;
                        }
                        if (data.trackableScheduleStatuses && data.trackableScheduleStatuses.length > 0) {
                            var onTheWaySchedules = this.sbService.returnCommonElements(this.OnTheWayScheduleStatuses, data.trackableScheduleStatuses, false);
                            if (onTheWaySchedules.length > 0) {
                                this.sbService.setConfirmationHeadingForDeleteGroup(onTheWaySchedules[0]);
                                this.PublishedGroup = { shiftIndex: i, rowIndex: j, colIndex: k, schedule: schedule, trip: trip };
                                this._savingBuilder = false;
                                jQuery('#btnconfirm-publish-delgrp').click();
                                return;
                            }
                        }

                        if (this.IsTrailerExists
                            && this.SbForm.get('PreferenceSetting').value.IsLoadOptimization
                            && !this.utilService.isValidOptimizedCapacityTrip(schedule.get('Trailers').value, this.RegionDetail.Trailers, trip.value)) {

                            this._savingBuilder = false;
                            this.changeDetectorRef.detectChanges();

                            this.confirmationDialogService.confirm('', OptimizedCapacityConfirmation, 'Continue', 'Correct').then((confirm) => {

                                if (confirm) {
                                    this.highLightDRDivOneLoad(trip, data);
                                    this.publishLoadSave(i, j, k, schedule, trip, isOptionalPickup);
                                }
                                else {
                                    this.editExisingGroup(trip, i, j, k, schedule, true);
                                    this.dataService.setShowOpenedDeliveryGroupSubject(true);
                                    return;
                                }
                            }).catch(() => console.log('User dismissed the dialog.'));

                        }
                        else {
                            this.highLightDRDivOneLoad(trip, data);
                            this.publishLoadSave(i, j, k, schedule, trip, isOptionalPickup);
                        }

                        //if (delReqs.some(t => t.IsFilldInvoke == true)) {
                        //    var delReqOrderIds = delReqs.map(t => { if (t.IsFilldInvoke == true) return t.OrderId; });
                        //    this.sbService.validateFilldOrderCompatibility(delReqOrderIds)
                        //        .subscribe(data => {
                        //            if (data && data.Result == false) {
                        //                this._savingBuilder = false;
                        //                Declarations.msgerror(data.Message, undefined, undefined);
                        //                this.changeDetectorRef.detectChanges();
                        //                return;
                        //            }
                        //            this.highLightDRDivOneLoad(trip, data);
                        //            this.publishLoadSave(i, j, k, schedule, trip, isOptionalPickup);
                        //        });
                        //} else {
                        //    this.highLightDRDivOneLoad(trip, data);
                        //    this.publishLoadSave(i, j, k, schedule, trip, isOptionalPickup);
                        //}
                    }
                });
        }
    }

    validateDraftLoad(trip: FormGroup) {
        var isValid = true;
        if (trip.controls['StartTime'].invalid || trip.controls['EndTime'].invalid || trip.controls['StartDate'].invalid) {
            isValid = false;
            trip.controls['StartDate'].touched;
            !trip.controls['StartDate'].value ? Declarations.msgerror('', 'Invalid Date', 5000) : Declarations.msgerror('', 'Please fill required field', 5000);
        }
        return isValid;
    }

    validatePublishLoad(trip: FormGroup) {
        var isValid = true;
        if (trip.controls.IsCommonPickup.value && !(trip.controls.Terminal.valid || trip.controls.BulkPlant.valid)) {
            isValid = false;
            Declarations.msgerror('', 'Please select common pickup location', 5000);
        }
        var _deliveryRequests = trip.get('DeliveryRequests').value as DeliveryRequestViewModel[];
        if (_deliveryRequests.length > 0) {
            _deliveryRequests.forEach(x => {
                if (x.OrderId == 0 || x.OrderId == null) {
                    isValid = false;
                    Declarations.msgerror('', 'Please select an order to publish the load', 5000);
                }
            });
        }
        return isValid;
    }

    public publishLoadYes() {
        Declarations.hideModal('#confirm-publish-delgrp');
        this.publishLoadSave(this.PublishedGroup.shiftIndex, this.PublishedGroup.rowIndex, this.PublishedGroup.colIndex, this.PublishedGroup.schedule, this.PublishedGroup.trip);
        this.PublishedGroup = null;
    }

    public publishLoadNo() {
        Declarations.hideModal('#confirm-publish-delgrp');
        this.PublishedGroup = null;
    }

    publishLoadSave(i: number, j: number, k: number, schedule: any, trip: FormGroup, isOptionalPickup: boolean = false) {
        this._savingBuilder = true;
        this.changeDetectorRef.detectChanges();
        var dsbModel = this.getDSBSaveModel();
        var tripValue = trip.value;
        this.setTripStatus(tripValue);
        this.setDeliveryGroupStatus(tripValue, DeliveryGroupStatus.Published);
        this.setDeliveryRequestStatus(tripValue, DeliveryReqStatus.ScheduleCreated);
        tripValue.Drivers = schedule.get('Drivers').value;
        tripValue.Trailers = schedule.get('Trailers').value;
        tripValue.IsIncludeAllRegionDriver = schedule.get('IsIncludeAllRegionDriver').value;
        dsbModel.Trips.push(tripValue);
        dsbModel.Status = 3;

        this.publishDriverViewToAPI(dsbModel, trip);
    }

    validateLoadWithTrailerCapacity(delReqs: any[], _selectedTrailers: any[]) {
        if (delReqs.length > 0 && _selectedTrailers.length > 0) {
            let terminal = [];
            let bulkPlant = [];
            let FuelRequestQty = 0;
            delReqs.forEach(t => {
                if (t.Terminal && t.Terminal.Id > 0) {
                    terminal.push(t.Terminal);
                }
                if (t.bulkPlant && t.bulkPlant.SiteId > 0 && t.bulkPlant.SiteName != null) {
                    bulkPlant.push(t.bulkPlant);
                }
                if (t.ScheduleQuantityType == 1) {
                    FuelRequestQty += t.RequiredQuantity;
                }
            });
            let trailerCapacity = 0;
            if (_selectedTrailers.length > 0) {
                for (let Capacity of _selectedTrailers) {
                    trailerCapacity += Capacity.FuelCapacity;
                }
            }
            if (trailerCapacity > 0) {
                if (terminal.length > 0 && bulkPlant.length == 0) {
                    let Uniqueterminals = terminal.map(item => item.Id).filter((value, index, self) => self.indexOf(value) === index);
                    if (Uniqueterminals.length == 1) {
                        if (_selectedTrailers.length > 0) {
                            if (FuelRequestQty > trailerCapacity) {
                                Declarations.msgerror("Fuel request quantity is greater than trailer capacity", undefined, undefined);
                                return false;
                            }
                        }
                    }
                }
                else {
                    if (bulkPlant.length > 0 && terminal.length == 0) {

                        let UniquebulkPlants = bulkPlant.map(item => item.SiteId).filter((value, index, self) => self.indexOf(value) === index);
                        if (UniquebulkPlants.length == 1) {
                            if (FuelRequestQty > trailerCapacity) {
                                Declarations.msgerror("Fuel request quantity is greater than trailer capacity", undefined, undefined);
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
        return true;
    }

    validateRowPublish(shiftIndex: number, rowIndex: number, isOptionalPickup: boolean): void {
        this.dataService.setShowDeliveryGroupSubject(false);
        let schedule = this.SbForm.get('Shifts.' + shiftIndex + '.Schedules.' + rowIndex) as FormGroup;
        var drivers = schedule.controls['Drivers'].value;
        var trailers = schedule.controls['Trailers'].value;
        if (drivers == null || drivers == undefined || drivers.length == 0 || (this.IsTrailerExists && (trailers == null || trailers == undefined || trailers.length == 0))) {
            if (this.IsTrailerExists)
                Declarations.msgwarning('Please assign a driver / trailer to publish', 'Driver/Trailer Required', 2500);
            else
                Declarations.msgwarning('Please assign a driver to publish', 'Driver Required', 2500);
            return;
        }

        //Check Job and Trailer Compatibility
        var trips = schedule.controls['Trips'] as FormArray;
        var _deliveryRequests = this.GetAllLoadsDR(trips);
        let _selectedTrailers = this.RegionDetail.Trailers.filter(x => {
            return trailers.find(y => y.TrailerId == x.TrailerId) != undefined;
        });

        if (drivers != null && drivers.length > 0) {
            var delReqs = _deliveryRequests;
            if (delReqs.length > 0 && delReqs.some(t => t.IsFilldInvoke == true)) {
                if (this.IsTrailerExists && trailers.some(t => t.IsFilldCompatible == false)) {
                    Declarations.msgwarning('Please assign a Filld compatible driver / trailer to publish', 'Driver/Trailer Required', 2500);
                    this._savingBuilder = false;
                    return;
                }
                else if (drivers.some(t => t.IsFilldCompatible == false)) {
                    Declarations.msgwarning('Please assign a Filld compatible driver to publish', 'Driver Required', 2500);
                    this._savingBuilder = false;
                    return;
                }
            }
            for (var i = 0; i < trips.length; i++) {
                var load = trips.controls[i] as FormGroup;
                var drs = load.controls["DeliveryRequests"].value
                if (!this.validateLoadWithTrailerCapacity(drs, _selectedTrailers)) {
                    this._savingBuilder = false;
                    return;
                }
            }
        }
        if (_deliveryRequests && _deliveryRequests.length > 0) {
            this.sbService.validateTrailerJobCompatibility(_selectedTrailers, _deliveryRequests)
                .subscribe(data => {
                    if (data && data.deliveryRequestsNotCompatible && data.deliveryRequestsNotCompatible.length > 0) {
                        this.highLightDRDiv(trips, data);
                        Declarations.msgerror("This job location is not compatible with trailer type", undefined, undefined);
                        //} else if (_deliveryRequests.some(t => t.IsFilldInvoke == true)) {
                        //    var delReqOrderIds = _deliveryRequests.map(t => { if (t.IsFilldInvoke == true) return t.OrderId; });

                        //    this.sbService.validateFilldOrderCompatibility(delReqOrderIds)
                        //        .subscribe(data => {
                        //            if (data && data.Result == false) {
                        //                Declarations.msgerror(data.Message, undefined, undefined);

                        //            } else {
                        //                this.publishEntireRow(schedule, shiftIndex, rowIndex, trips, isOptionalPickup);
                        //                this.highLightDRDiv(trips, null);
                        //            }
                        //        });
                    } else {
                        this.publishEntireRow(schedule, shiftIndex, rowIndex, trips, isOptionalPickup);
                        this.highLightDRDiv(trips, null);
                    }
                });
        } else {
            this.publishEntireRow(schedule, shiftIndex, rowIndex, trips, isOptionalPickup);
            this.highLightDRDiv(trips, null);
        }
        //End Check Job and Trailer Compatibility
    }
    publishEntireRow(schedule: FormGroup, shiftIndex: number, rowIndex: number, trips: FormArray, isOptionalPickup: boolean = false): void {
        let validTrips = [];
        let firstInvalidOptimizedCapacityTripIndex: number = null;

        for (var i = 0, j = 1; i < trips.length; i++, j++) {
            let thisTrip = trips.controls[i] as FormGroup;
            if (isOptionalPickup) {
                let isValid = true;
                var delReqs = thisTrip.controls['DeliveryRequests'] as FormArray;
                delReqs.controls.forEach((x: FormGroup) => {
                    let OrderId = x.get('OrderId').value;
                    let ProductType = x.get('ProductType').value;
                    let terminalInfo = x.get('Terminal').value;
                    let bulkplantInfo = x.get('BulkPlant').value;
                    if (terminalInfo.Id == 0 && (bulkplantInfo.Address == null || bulkplantInfo.Address == '')) {
                        let fuelTypeInfo = this.ScheduleOrderFuelInfo.find(x => x.OrderId == OrderId);
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
                if (isValid) {
                    var delReqs = thisTrip.controls['DeliveryRequests'] as FormArray;
                    delReqs.controls.forEach((x: FormGroup) => {
                        this.setDRPickupValidators(x);
                    });
                }
            }
            if (thisTrip && (thisTrip.invalid || !this.validatePublishLoad(thisTrip))) {
                if (thisTrip.get('DeliveryRequests').value.length > 0) {
                    this.editExisingGroup(thisTrip, shiftIndex, rowIndex, i, schedule, true);
                    this.dataService.setShowOpenedDeliveryGroupSubject(true);
                    return;
                }
            } else {
                validTrips.push(i);

                //get first in-valid trip that exceeded optimized capacity limit
                if (this.IsTrailerExists
                    && this.SbForm.get('PreferenceSetting').value.IsLoadOptimization
                    && firstInvalidOptimizedCapacityTripIndex == null
                    && !this.utilService.isValidOptimizedCapacityTrip(schedule.get('Trailers').value, this.RegionDetail.Trailers, thisTrip.value)) {
                    firstInvalidOptimizedCapacityTripIndex = i;
                }
            }
        }

        if (firstInvalidOptimizedCapacityTripIndex == null) {
            this.createDsbModelAndPublish(schedule, trips, validTrips);
        }
        else {

            this.confirmationDialogService.confirm('', OptimizedCapacityConfirmation, 'Continue', 'Correct').then((confirm) => {
                if (confirm) {
                    this.createDsbModelAndPublish(schedule, trips, validTrips);
                }
                else {
                    this.editExisingGroup(trips.at(firstInvalidOptimizedCapacityTripIndex) as FormGroup, shiftIndex, rowIndex, i, schedule, true);
                    this.dataService.setShowOpenedDeliveryGroupSubject(true);
                    return;
                }
            }).catch(() => console.log('User dismissed the dialog.'));
        }
    }

    createDsbModelAndPublish(schedule: FormGroup, trips: FormArray, validTrips: any[]) {
        this._savingBuilder = true;
        this.changeDetectorRef.detectChanges();
        var dsbModel = this.getDSBSaveModel();
        var drivers = schedule.controls['Drivers'].value;
        var trailers = schedule.controls['Trailers'].value;
        for (var i = 0; i < trips.length; i++) {
            var tripValue = trips.value[i];
            if (validTrips.includes(tripValue.DriverColIndex)) {
                this.setTripStatusToPublish(tripValue);
            }
            dsbModel.Trips.push(tripValue);
        }
        dsbModel.Trips.forEach(t => { t.Drivers = drivers, t.Trailers = trailers, t.IsIncludeAllRegionDriver = schedule.get('IsIncludeAllRegionDriver').value });
        dsbModel.Status = 3;

        this.publishDriverViewToAPI(dsbModel, trips);
    }

    publishDriverViewToAPI(dsbModel: DSBSaveModel, trips: FormGroup | FormArray) {
        this.sbService.publishDriverView(dsbModel).subscribe(data => {
            this._savingBuilder = false;
            this.updateLoadForm(data, trips);
        });
    }

    setTripStatusToPublish(trip: any) {
        this.setTripStatus(trip);
        this.setDeliveryGroupStatus(trip, DeliveryGroupStatus.Published);
        this.setDeliveryRequestStatus(trip, DeliveryReqStatus.ScheduleCreated);
    }

    updateLoadForm(data: DSBSaveModel, trip: FormGroup | FormArray, isDateChange: boolean = false): void {
        if (data == null) {
            Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
            return;
        }
        if (data.StatusCode == 0 || data.StatusCode == 2) {
            this.droppedTrip = null;
            this.draggedDelReqData = null;
            this.setUnsavedChanges();
            if (!isDateChange) {
                this.SbForm.patchValue({
                    Id: data.Id,
                    TimeStamp: data.TimeStamp,
                    Status: 0
                });
                if (data && data.Trips && data.Trips.length > 0) {
                    if (trip instanceof FormGroup) {
                        data.Trips[0].DeliveryRequests = this.sorDrByProductSequence(data.Trips[0].IsDispatcherDragDropSequence, data.Trips[0].DeliveryRequests);
                        trip.patchValue(data.Trips[0]);

                        //remove existing drs from the trip
                        let tripDrArray = trip.get('DeliveryRequests') as FormArray;
                        tripDrArray.clear();
                        //insert newly sorted drs in trip
                        let newDrArray = this.utilService.getDeliveryRequestFormArray(data.Trips[0].DeliveryRequests, data.Trips[0].IsCommonPickup, data.Trips[0].IsDispatcherDragDropSequence, 0);
                        newDrArray.controls.forEach(drForm => {
                            tripDrArray.push(drForm);
                        });

                        let sIndex = trip.controls['ShiftIndex'].value;
                        let rIndex = trip.controls['DriverRowIndex'].value;
                        this.setAllowDriverChange(sIndex, rIndex);
                        this.dataService.setSavedChangesSubject(trip.value);
                    }
                    else if (trip instanceof FormArray) {
                        for (let i = 0; i < trip.length; i++) {
                            let thisTrip = trip.controls[i] as FormGroup;
                            let shiftId = thisTrip.controls['ShiftId'].value;
                            let shiftIndex = thisTrip.controls['ShiftIndex'].value;
                            let rowIndex = thisTrip.controls['DriverRowIndex'].value;
                            let colIndex = thisTrip.controls['DriverColIndex'].value;
                            let savedTrip = data.Trips.find(t => t.ShiftId == shiftId && t.DriverRowIndex == rowIndex && t.DriverColIndex == colIndex);
                            savedTrip.DeliveryRequests = this.sorDrByProductSequence(savedTrip.IsDispatcherDragDropSequence, savedTrip.DeliveryRequests);
                            if (savedTrip) {
                                thisTrip.patchValue(savedTrip);

                                //remove existing drs from the trip
                                let tripDrArray = thisTrip.get('DeliveryRequests') as FormArray;
                                tripDrArray.clear();
                                //insert newly sorted drs in trip
                                let newDrArray = this.utilService.getDeliveryRequestFormArray(savedTrip.DeliveryRequests, savedTrip.IsCommonPickup, savedTrip.IsDispatcherDragDropSequence, 0);
                                newDrArray.controls.forEach(drForm => {
                                    tripDrArray.push(drForm);
                                });

                                this.setAllowDriverChange(shiftIndex, rowIndex);
                                this.dataService.setSavedChangesSubject(thisTrip.value);
                            }
                        }
                    }
                }
            }
            if (data.StatusCode == 0)
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
            else
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
            if (isDateChange) {
                this.dataService.setUnsavedChangesAsEmptySubject();
            }
        }
        else {
            Declarations.msgerror(data.StatusMessage, undefined, undefined);
        }

        this._savingBuilder = false;
        this.changeDetectorRef.detectChanges();
        this.dataService.setScheduleChangeDetectSubject(true);
        //this.dataService.FormUnsavedChangesSubject.unsubscribe();
    }

    updateLoadsFromRow(data: DSBSaveModel, trips: FormArray): void {
        if (data == null) {
            Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
            this.editDriverCancel();
            return;
        }
        if (data.StatusCode == 0 || data.StatusCode == 2) {
            this.setUnsavedChanges();
            this.SbForm.patchValue({
                Id: data.Id,
                TimeStamp: data.TimeStamp,
                Status: 0
            });
            for (var i = 0, j = 1; i < trips.length; i++, j++) {
                let thisTrip = trips.controls[i] as FormGroup;
                var shiftId = thisTrip.get('ShiftId').value;
                var rowIndex = thisTrip.get('DriverRowIndex').value;
                var colIndex = thisTrip.get('DriverColIndex').value;
                var savedTrip = data.Trips.filter(t => t.ShiftId == shiftId && t.DriverRowIndex == rowIndex && t.DriverColIndex == colIndex);
                if (savedTrip && savedTrip != null && savedTrip.length > 0) {
                    thisTrip.patchValue({
                        TripId: savedTrip[0].TripId,
                        TimeStamp: savedTrip[0].TimeStamp
                    });
                }
            }
            if (data.StatusCode == 0)
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
            else
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
        }
        else {
            Declarations.msgerror(data.StatusMessage, undefined, undefined);
            this.editDriverCancel();
        }

        this._savingBuilder = false;
        this.changeDetectorRef.detectChanges();
        this.dataService.setScheduleChangeDetectSubject(true);
        //this.dataService.FormUnsavedChangesSubject.unsubscribe();
    }

    isTrailersSelected(): boolean {
        let _trailers = this.DriverTrailerForm.get('Trailers').value
        return _trailers && _trailers.length > 0;
    }

    addShift(shiftIdx: number, scheduleIdx: number): void {
        var _tArray = this.SbForm.get('Shifts.' + shiftIdx + '.Schedules.' + scheduleIdx + '.Trips') as FormArray;
        let _startDate = this.SbForm.get('Date').value;
        var shift = this.SbForm.get('Shifts.' + shiftIdx) as FormGroup;
        let shiftId = shift.get('Id').value;
        let selectedShift = this.Shifts.find(x => x.Id == shiftId);
        let _startTime = selectedShift.StartTime;
        if (_tArray.controls.length > 0) {
            let lastTripStartTime = _tArray.controls[_tArray.controls.length - 1].get('StartTime').value;
            let lastTripStartDate = _tArray.controls[_tArray.controls.length - 1].get('StartDate').value;
            _startTime = _tArray.controls[_tArray.controls.length - 1].get('EndTime').value;
            _startDate = this.getNewLoadStartDate(moment(lastTripStartDate + ' ' + lastTripStartTime).toDate(), moment(lastTripStartDate + ' ' + _startTime).toDate());
        }
        let startTime = moment(_startDate + ' ' + _startTime).toDate();
        let trip = this.getTrailerTrip(startTime, selectedShift.SlotPeriod, scheduleIdx, _tArray.controls.length);
        trip.ShiftStartTime = shift.get('StartTime').value;
        trip.ShiftEndTime = shift.get('EndTime').value;
        trip.SlotPeriod = shift.get('SlotPeriod').value;
        trip.ShiftId = shiftId;
        trip.ShiftIndex = shiftIdx;
        _tArray.push(this.utilService.getTripFormGroup(trip));
    }

    getNewLoadStartDate(startDateTime: Date, endDateTime: Date) {
        if (startDateTime > endDateTime) {
            endDateTime.setDate(endDateTime.getDate() + 1);
        }
        return moment(endDateTime).format('MM/DD/YYYY');
    }

    getTrailerTrip(startTime: Date, slotPeriod: number, rowIndex: number, colIndex: number): TripModel {
        if (slotPeriod <= 0) { slotPeriod = 3; }
        let trip = new TripModel();
        trip.StartDate = moment(startTime).format('MM/DD/YYYY');
        trip.StartTime = moment(startTime).format('hh:mm A');
        let endTime = moment(startTime).add(slotPeriod, 'hours').toDate();
        trip.EndTime = moment(endTime).format('hh:mm A');
        trip.IsEditable = true;
        trip.IsPreloadDisable = false;
        trip.DriverRowIndex = rowIndex;
        trip.DriverColIndex = colIndex;
        return trip;
    }


    onTrailerSelectAll(items: any): void {
        this.DriverTrailerForm.get('Trailers').setValue(items);
    }
    onTrailerDeSelectAll(): void {
        this.DriverTrailerForm.get('Trailers').setValue([]);
    }

    getDriverdetails() {
        let shift = this.SbForm.controls['Shifts']['controls'][this.EditDriverData.Index1] as FormGroup;
        if (shift != null) {
            let ShiftId = shift.get('Id').value;
            this.sbService.getShiftCompanyDrivers(this.SelectedRegionId, this.DriverTrailerForm.get('IsIncludeAllRegionDriver').value ? "AllowOtherRegion" : "", this.SbForm.controls.Date.value, ShiftId).subscribe((drivers: DriverAdditionalDetailModel[]) => {
                this.AllUnassignedDrivers = drivers;
                this.UnassignedDrivers = this.getUnassignedDrivers();
                if (this.UnassignedDrivers.length == 0) {
                    this.UnassignedDrivers = [];
                }
                this._loadingDrivers = false;
                this.changeDetectorRef.detectChanges();
            });
        }
    }

    IdNotInComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other.Id == current.Id
            }).length == 0;
        }
    }

    CodeComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other.Code == current.Code
            }).length == 0;
        }
    }

    public checkAnyPublishedTrip(schedule: FormGroup): Boolean {
        let isPublished = false;
        var trips = schedule.get('Trips') as FormArray;
        for (var i = 0; i < trips.length; i++) {
            if (trips[i] != null && trips[i] != undefined && !isPublished) {
                var drs = trips[i].get('DeliveryRequests') as FormArray;
                for (let k = 0; k < drs.length; k++) {
                    if (drs.controls[k].get('PreviousStatus').value === 3) {
                        isPublished = true;
                        break;
                    }
                }
                if (isPublished)
                    break;
            }
        }
        return isPublished;
    }

    editExisingGroup(_trip: FormGroup, _shiftIndex: number, _rowIndex: number, _tripIndex: number, _schedule: any, _isPublishLoadInvalid = false) {
        this.editGroup(_trip, _shiftIndex, _rowIndex, _tripIndex, _schedule, _isPublishLoadInvalid);
        this.dataService.setShowOpenedDeliveryGroupSubject(true);
    }

    editDroppedGroup(_trip: FormGroup, _shiftIndex: number, _rowIndex: number, _tripIndex: number, _schedule: any, _isPublishLoadInvalid = false) {
        this.editGroup(_trip, _shiftIndex, _rowIndex, _tripIndex, _schedule, _isPublishLoadInvalid);
        this.dataService.setShowDeliveryGroupSubject(true);
    }

    editGroup(_trip: FormGroup, _shiftIndex: number, _rowIndex: number, _tripIndex: number, _schedule: any, _isPublishLoadInvalid = false) {

        const drsFormArray = _trip.controls['DeliveryRequests'] as FormArray;
        let drs = drsFormArray.value || [];
        drs = sortArrayTwice(drs, 'JobId', 'ProductSequence');
        drsFormArray.patchValue(drs);

        this.dataService.setEditDeliveryGroupSubject({
            trip: _trip,
            shiftIndex: _shiftIndex,
            rowIndex: _rowIndex,
            tripIndex: _tripIndex,
            schedule: _schedule,
            isPublishLoadInvalid: _isPublishLoadInvalid
        });
    }

    private cancelCreateLoad(data: any): void {
        let trip = this.SbForm.get('Shifts.' + data.ShiftIndex + '.Schedules.' + data.RowIndex + '.Trips.' + data.ColIndex) as FormGroup;
        if (trip) {
            let _drArray = trip.controls['DeliveryRequests'] as FormArray;
            if (_drArray) {
                data.Drs.forEach(x => {
                    let drIndex = _drArray.controls.findIndex((y: FormGroup) => y.controls['Id'].value == x.Id);
                    if (drIndex >= 0) {
                        _drArray.removeAt(drIndex);
                    }
                });
            }
            this.changeDetectorRef.markForCheck();
            this.dataService.setRestoreDeletedRequestSubject(data.Drs);
            this.dataService.setScheduleChangeDetectSubject(true);
        }
    }

    private updateTripOnCreateLoadSucess(data: any): void {
        let schedule = this.SbForm.get('Shifts.' + data.ShiftIndex + '.Schedules.' + data.RowIndex) as FormGroup;
        let trip = schedule.get('Trips.' + data.ColIndex) as FormGroup;
        if (trip && data.Drs.length > 0) {
            data.Drs.forEach(t => t.Status = 5);
            let groupParentDrs = data.Drs.filter(x => x.GroupParentDRId != '').length;
            let jobId = data.Drs[0].JobId;
            let _drArray = trip.controls['DeliveryRequests'] as FormArray;
            if (_drArray) {
                let existingDrsIndexes = [];
                let existingDrs = _drArray.controls.filter((x: FormGroup, index: number) => {
                    let groupParentDRId = x.controls['GroupParentDRId'].value;
                    let drId = x.controls['Id'].value;
                    if (groupParentDrs > 0) {
                        if (data.Drs.find(x => x.GroupParentDRId == groupParentDRId && x.Id == drId)) {
                            existingDrsIndexes.push(index);
                        }
                    }
                    else if (x.controls['JobId'].value == jobId && (x.controls['Status'].value == 0 || x.controls['Status'].value == 1 || x.controls['Status'].value == 2 || x.controls['Status'].value == 5)) {
                        existingDrsIndexes.push(index);
                    }
                    return x.controls['JobId'].value == jobId;
                });
                for (let index = existingDrsIndexes.length - 1; index >= 0; index--) {
                    _drArray.removeAt(existingDrsIndexes[index]);
                }
                let isCommonPickup = trip.controls['IsCommonPickup'].value as boolean;
                if (existingDrsIndexes.length > 0 && _drArray.controls.length > 0) {
                    for (let index = data.Drs.length - 1; index >= 0; index--) {
                        let _drForm = this.utilService.getDeliveryRequestForm(data.Drs[index], isCommonPickup);
                        _drArray.insert(existingDrsIndexes[0], _drForm);
                    }
                } else {
                    for (let index = 0; index < data.Drs.length; index++) {
                        let _drForm = this.utilService.getDeliveryRequestForm(data.Drs[index], isCommonPickup);
                        _drArray.push(_drForm);
                    }
                }
            }
            this.changeDetectorRef.markForCheck();
            this.dataService.setScheduleChangeDetectSubject(true);
            this.addNewDrsInDataService(jobId, data);
        }
        this._savingBuilder = false;
        this.changeDetectorRef.detectChanges();
        // if (trip && data.Drs) {
        //    this.editDroppedGroup(trip, data.ShiftIndex, data.RowIndex, data.ColIndex, schedule, false);
        //}
    }

    private addNewDrsInDataService(jobId: number, data): void {
        let AllDrs = this.dataService.AllDeliveryRequestsSubject.value as DeliveryRequestViewModel[];
        //let drsToRemove = AllDrs.filter(t => t.JobId == jobId);
        let subDRs = AllDrs.filter(t => t.GroupParentDRId && t.GroupParentDRId != '' && t.JobId == jobId);
        let subJobDRs = AllDrs.filter(t => t.GroupParentDRId == '' && t.JobId == jobId);
        AllDrs = AllDrs.filter(t => t.JobId != jobId);
        AllDrs = AllDrs.concat(data.Drs);
        AllDrs = AllDrs.concat(subDRs);
        if (subDRs.length > 0) {
            AllDrs = AllDrs.concat(subJobDRs);
        }
        this.dataService.setAllDeliveryRequestsSubject(AllDrs);
        //this.dataService.setRemoveDroppedRequestSubject(drsToRemove);
    }

    private setUnsavedChanges(): void {
        if (this.driverSchedules) {
            this.driverSchedules.forEach(x => x.unsubscribeFormChange());
        }
    }

    private getPreloadDrViewModel(preLoadInfo: any, drIds: any, shiftEndDate: string): PreLoadDrViewModel {
        var model = new PreLoadDrViewModel();
        model.PreloadDrs = drIds;
        model.PreloadTrailers = preLoadInfo.PreloadTrailers;
        model.PreloadDrivers = preLoadInfo.PreloadDrivers;
        model.IsTrailerExists = preLoadInfo.IsTrailerExists;
        model.RegionId = this.SbForm.controls['RegionId'].value;
        model.SbView = this.SbForm.controls['ObjectFilter'].value;
        model.ShiftEndDate = shiftEndDate;
        model.ShiftId = preLoadInfo.ShiftId;
        model.SbDsbView = this.SbForm.controls['DSBFilter'].value;
        return model;
    }

    private getModifiedLoadTripInfo(loadInfo: any): any {
        let _modifiedTripInfo = new ModifiedTripInfo();
        _modifiedTripInfo.ShiftIndex = loadInfo.ShiftIndex;
        _modifiedTripInfo.DriverRowIndex = loadInfo.ScheduleIndex;
        _modifiedTripInfo.DriverColIndex = loadInfo.TripIndex;
        return _modifiedTripInfo;
    }

    private createPreloadForAcrossTheDate(preLoadInfo: any, shiftEndDate: string): void {
        // shift cross the date, need to create a draft schedule builder (or update existing)
        var model = this.getPreloadDrViewModel(preLoadInfo, preLoadInfo.PreloadDrs, shiftEndDate);
        this._savingBuilder = true;
        this.sbService.createPreloadForAcrossTheDate(model).subscribe(data => {
            if (data == null) {
                Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
            } else if (data.StatusCode == 0) {
                this.updateAcrossTheDateDrsPreloadInfo(preLoadInfo, data);
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
            } else {
                this._savingBuilder = false;
                Declarations.msgerror(data.StatusMessage, undefined, undefined);
            }
            if (data.StatusCode == 0) {
                let _modifiedTripInfo = this.getModifiedLoadTripInfo(preLoadInfo);
                this.saveScheduleBuilderData([_modifiedTripInfo], false);
            }
            this.changeDetectorRef.detectChanges();
        });
    }

    private createPreloadForSameDate(preLoadInfo: any, shifts: any, shiftEndDate: string): void {
        let postLoadInfo = this.getShiftInfoWithDriverTrailerInOtherShift(preLoadInfo, shifts);
        if (postLoadInfo == undefined) {
            if (preLoadInfo.IsTrailerExists) {
                let trailerNames = preLoadInfo.PreloadTrailers.map(t => t.TrailerId).join(", ");
                trailerNames = preLoadInfo.PreloadTrailers.length > 1 ? 'trailers ' + trailerNames + ' are' : 'trailer ' + trailerNames + ' is'
                Declarations.msgerror('Preload can not be done as ' + trailerNames + ' not assigned to any other shift', undefined, undefined);
            } else {
                Declarations.msgerror('Preload can not be done as driver is not assigned to any other shift', undefined, undefined);
            }
        } else {
            this._savingBuilder = true;
            let drIds = preLoadInfo.PreloadDrs.map(t => t.Id);
            this.sbService.cloneDrsForPreload(drIds).subscribe(data => {
                if (data == null) {
                    Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
                } else if (data.length > 0) {
                    postLoadInfo["PostloadDrs"] = data;
                    this.updatePreloadDrsOnSuccess(preLoadInfo, postLoadInfo);
                    Declarations.msgsuccess('Preload created successfully for the date ' + shiftEndDate, undefined, undefined);

                    let _modifiedPreloadTripInfo = this.getModifiedLoadTripInfo(preLoadInfo);
                    let _modifiedPostloadTripInfo = this.getModifiedLoadTripInfo(postLoadInfo);
                    this.saveScheduleBuilderData([_modifiedPreloadTripInfo, _modifiedPostloadTripInfo], false);

                    this.dataService.setDeliveryPreloadOption({ ShiftIndex: preLoadInfo.ShiftIndex, ScheduleIndex: preLoadInfo.ScheduleIndex, Reset: false });
                } else {
                    Declarations.msgsuccess('Failed to create preload. Please contact the administrator.', undefined, undefined);
                }
                this.changeDetectorRef.detectChanges();
            });
        }
    }

    public onPreloadTrailerChange(event: any): void {
        let trailerId = event.target.value;
        if (trailerId) {
            this.preloadSelectedTrailerId = trailerId;
        }
    }

    public onPreloadTrailerSubmit(): void {
        if (this.preloadSelectedTrailerId) {
            Declarations.hideModal("#select-preload-trailer");
            let selectedTrailer = this.PreLoadInfo.PreloadTrailers.find(x => x.Id == this.preloadSelectedTrailerId);
            this.PreLoadInfo.PreloadTrailers = [selectedTrailer];
            this.processPreloadDeliveryCreation(this.PreLoadInfo);
            this.preloadSelectedTrailerId = null;
        }
    }

    private processPreloadDeliveryCreation(preLoadInfo: any): void {
        let shifts = this.SbForm.controls['Shifts'].value as ScheduleShiftModel[];
        let preloadShift = shifts.find(x => x.Id == preLoadInfo.ShiftId) as ScheduleShiftModel;
        let result = this.isAcrossTheDateShift(preloadShift);
        if (result.IsAcrossTheDateShift) {
            // shift cross the date, need to create a draft schedule builder (or update existing)
            this.createPreloadForAcrossTheDate(preLoadInfo, result.ShiftEndDate);
        } else {
            // shift is ending in same date, so search for same trailers
            this.createPreloadForSameDate(preLoadInfo, shifts, result.ShiftEndDate);
        }
    }

    private isAcrossTheDateShift(preloadShift: ScheduleShiftModel): any {
        let shiftStartTime = preloadShift.StartTime;
        let shifEndTime = preloadShift.EndTime;
        let dsbDate = this.SbForm.controls['Date'].value;
        let startDate = moment(dsbDate + ' ' + shiftStartTime).toDate();
        let endDate = moment(dsbDate + ' ' + shifEndTime).toDate();
        if (startDate > endDate) {
            endDate.setDate(endDate.getDate() + 1);
        }
        let shiftEndDate = moment(endDate).format('MM/DD/YYYY');
        let _isAcrossTheDateShift = dsbDate != shiftEndDate;
        return {
            IsAcrossTheDateShift: _isAcrossTheDateShift,
            ShiftEndDate: shiftEndDate
        };
    }

    private validateTrips(_unsavedChanges: ModifiedTripInfo[]) {
        var isValidTrips = true;
        _unsavedChanges.forEach(t => {
            let schedule = this.SbForm.get('Shifts.' + t.ShiftIndex + '.Schedules.' + t.DriverRowIndex) as FormGroup;
            var trip = schedule.get('Trips.' + t.DriverColIndex) as FormGroup;
            if (trip.get('GroupId').value > 0) {
                if (!trip.valid) {
                    if (trip.get('DeliveryRequests').value.length > 0) {
                        //log max fill validation issue.
                        this.logDRMaxFillIssue(trip);
                        this.editExisingGroup(trip, t.ShiftIndex, t.DriverRowIndex, t.DriverColIndex, schedule, true);
                        this.dataService.setShowOpenedDeliveryGroupSubject(true);
                        isValidTrips = false;
                        return isValidTrips;
                    }
                }
            }
            else if (trip.invalid && trip.get('DeliveryRequests').value.length > 0) {
                //log max fill validation issue.
                this.logDRMaxFillIssue(trip);
            }

        });
        return isValidTrips;
    }

    public saveScheduleBuilderData(_unsavedChanges: ModifiedTripInfo[], isDateChange: boolean) {
        if (_unsavedChanges.length > 0) {
            var isValidTrips = this.validateTrips(_unsavedChanges);
            if (!isValidTrips) {
                return;
            }
            this._savingBuilder = true;
            var isPublish = false;
            var trips = new FormArray([]);
            var dataToSave = this.getDSBSaveModel();
            _unsavedChanges.forEach(t => {
                let schedule = this.SbForm.get('Shifts.' + t.ShiftIndex + '.Schedules.' + t.DriverRowIndex) as FormGroup;
                var trip = schedule.get('Trips.' + t.DriverColIndex) as FormGroup;
                trips.push(trip);
                var tripModel = trip.value;
                if (trip.controls['GroupId'].value > 0) {
                    this.setTripStatus(tripModel);
                    this.setDeliveryGroupStatus(tripModel, DeliveryGroupStatus.Published);
                    this.setDeliveryRequestStatus(tripModel, DeliveryReqStatus.ScheduleCreated);
                    isPublish = true;
                }
                else {
                    this.setTripStatus(tripModel);
                    this.setDeliveryGroupStatus(tripModel, DeliveryGroupStatus.Draft);
                }

                tripModel.Drivers = schedule.controls['Drivers'].value;
                tripModel.Trailers = schedule.controls['Trailers'].value;
                tripModel.IsIncludeAllRegionDriver = schedule.controls['IsIncludeAllRegionDriver'].value;
                dataToSave.Trips.push(tripModel);
            });
            this.changeDetectorRef.detectChanges();
            this.dataService.setShowDeliveryGroupSubject(false);
            if (dataToSave.Trips.length > 0) {
                if (isPublish) {
                    dataToSave.Status = 3;
                    this.sbService.publishDriverView(dataToSave).subscribe(data => {
                        this._savingBuilder = false;
                        this.updateLoadForm(data, trips, isDateChange);
                    });
                }
                else {
                    dataToSave.Status = 1;
                    this.sbService.saveDriverView(dataToSave).subscribe(data => {
                        this._savingBuilder = false;
                        this.updateLoadForm(data, trips, isDateChange);
                    });
                }
            }
        }
    }

    private getShiftInfoWithDriverTrailerInOtherShift(data: any, allShifts: ScheduleShiftModel[]): any {
        let shiftWithSameDriverTrailer = undefined;
        for (var shiftIdx = data.ShiftIndex + 1; shiftIdx < allShifts.length; shiftIdx++) {
            let schedules = allShifts[shiftIdx].Schedules;
            for (var scheduleIdx = 0; scheduleIdx < schedules.length; scheduleIdx++) {
                shiftWithSameDriverTrailer = this.getMatchedDriverTrailerInfo(data, allShifts, shiftIdx, scheduleIdx);
                if (shiftWithSameDriverTrailer != undefined) {
                    break;
                }
            }
            if (shiftWithSameDriverTrailer != undefined) {
                break;
            }
        }
        return shiftWithSameDriverTrailer;
    }

    private getMatchedDriverTrailerInfo(data: any, allShifts: ScheduleShiftModel[], shiftIdx: number, scheduleIdx: number): any {
        let shiftWithSameTrailer = undefined;
        let schedules = allShifts[shiftIdx].Schedules;
        let matchedDriverTrailers = [];
        if (data.IsTrailerExists) {
            matchedDriverTrailers = schedules[scheduleIdx].Trailers.filter(this.IdInComparer(data.PreloadTrailers));
        } else {
            matchedDriverTrailers = schedules[scheduleIdx].Drivers.filter(this.IdInComparer(data.PreloadDrivers));
        }
        if (matchedDriverTrailers.length > 0) {
            shiftWithSameTrailer = {
                Shift: allShifts[shiftIdx],
                ShiftIndex: shiftIdx,
                ShiftId: allShifts[shiftIdx].Id,
                Schedule: schedules[scheduleIdx],
                ScheduleIndex: scheduleIdx,
                TripIndex: 0
            };
        }
        return shiftWithSameTrailer;
    }

    IdInComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other.Id == current.Id
            }).length > 0;
        }
    }

    private updatePreloadDrsOnSuccess(preLoadInfo: any, postLoadInfo: any): void {
        this.updatePostLoadedForIds(preLoadInfo, postLoadInfo);
        this.updatePreloadDrs(preLoadInfo, postLoadInfo);
        this.updatePostloadDrs(preLoadInfo, postLoadInfo);
        this.dataService.setScheduleChangeDetectSubject(true);
    }

    private updatePostLoadedForIds(preLoadInfo: any, postLoadInfo: any): void {
        preLoadInfo.PreloadDrs.forEach(x => {
            var postLoadedDr = postLoadInfo.PostloadDrs.find(y => y.PostLoadedFor == x.Id);
            if (postLoadedDr) {
                x['PreLoadedFor'] = postLoadedDr.Id;
            }
        });
    }

    private updatePreloadDrs(preLoadInfo: any, postLoadInfo: any): void {
        let trip = this.SbForm.get('Shifts.' + preLoadInfo.ShiftIndex + '.Schedules.' + preLoadInfo.ScheduleIndex + '.Trips.' + preLoadInfo.TripIndex) as FormGroup;
        if (trip) {
            let preloadDrIds = preLoadInfo.PreloadDrs.map(t => t.Id);
            let preloadDrs = trip.controls['DeliveryRequests'] as FormArray;
            let drsToUpdate = preloadDrs.controls.filter((x: FormGroup) => preloadDrIds.indexOf(x.controls['Id'].value) >= 0) as FormGroup[];
            drsToUpdate.forEach((x: FormGroup) => {
                let postLoadDr = postLoadInfo.PostloadDrs.find(y => y.PostLoadedFor == x.controls['Id'].value);
                if (postLoadDr) { //Add post load refernce to preload DRs
                    x.addControl('PreLoadedFor', this.fb.control(postLoadDr.Id));
                    x.addControl('PostLoadInfo', this.utilService.getLoadInfoForm({
                        ShiftId: postLoadInfo.ShiftId,
                        ScheduleIndex: postLoadInfo.ScheduleIndex,
                        TripIndex: postLoadInfo.TripIndex,
                        DrId: postLoadDr.Id
                    }));
                    x.controls['ScheduleStatus'].patchValue(15);
                }
            });
        }
    }

    private updatePostloadDrs(preLoadInfo: any, postLoadInfo: any): void {
        let trip = this.SbForm.get('Shifts.' + postLoadInfo.ShiftIndex + '.Schedules.' + postLoadInfo.ScheduleIndex + '.Trips.' + postLoadInfo.TripIndex) as FormGroup;
        if (trip) {
            let postloadDrs = trip.controls['DeliveryRequests'] as FormArray;
            for (var index = postLoadInfo.PostloadDrs.length - 1; index >= 0; index--) {
                postLoadInfo.PostloadDrs[index].Status = 5; // New DR
                postLoadInfo.PostloadDrs[index].ScheduleStatus = 14; // New DR
                let preLoadDr = preLoadInfo.PreloadDrs.find(y => y.PreLoadedFor == postLoadInfo.PostloadDrs[index].Id);
                let postLoadDr = this.utilService.getDeliveryRequestForm(postLoadInfo.PostloadDrs[index], false);
                postLoadDr.patchValue({
                    Terminal: preLoadDr.Terminal,
                    BulkPlant: preLoadDr.BulkPlant,
                    OrderId: preLoadDr.OrderId,
                    PickupLocationType: preLoadDr.PickupLocationType
                });
                //Add pre load refernce to postload DRs
                postLoadDr.addControl('PreLoadInfo', this.utilService.getLoadInfoForm({
                    ShiftId: preLoadInfo.ShiftId,
                    ScheduleIndex: preLoadInfo.ScheduleIndex,
                    TripIndex: preLoadInfo.TripIndex,
                    DrId: preLoadDr.Id
                }));
                postloadDrs.insert(0, postLoadDr);
            }
        }
    }

    private updateAcrossTheDateDrsPreloadInfo(preLoadInfo: any, data: PreLoadDrResponseViewModel): void {
        let trip = this.SbForm.get('Shifts.' + preLoadInfo.ShiftIndex + '.Schedules.' + preLoadInfo.ScheduleIndex + '.Trips.' + preLoadInfo.TripIndex) as FormGroup;
        if (trip) {
            let preloadDrIds = preLoadInfo.PreloadDrs.map(t => t.Id);
            let preloadDrs = trip.controls['DeliveryRequests'] as FormArray;
            let drsToUpdate = preloadDrs.controls.filter((x: FormGroup) => preloadDrIds.indexOf(x.controls['Id'].value) >= 0) as FormGroup[];
            drsToUpdate.forEach((x: FormGroup) => {
                let preLoadDrModel = data.PreloadDrs.find(y => y.Id == x.controls['Id'].value);
                if (preLoadDrModel) { //Add post load refernce to preload DRs
                    x.addControl('PreLoadedFor', this.fb.control(preLoadDrModel.PreLoadedForId));
                    x.controls['ScheduleStatus'].patchValue(15);
                    trip.controls['TripStatus'].patchValue(TripStatus.Modified);
                }
            });
        }
    }

    private updateEditedPostloadDrs(drs: DeliveryRequestViewModel[]): void {
        let _shiftArray = this.SbForm.controls['Shifts'] as FormArray;
        drs.forEach(x => {
            let postLoadInfo = x.PostLoadInfo;
            if (postLoadInfo) {
                let thisShift = _shiftArray.controls.find((s: FormGroup) => s.controls['Id'].value == postLoadInfo.ShiftId) as FormGroup;
                if (thisShift) {
                    let thisTrip = thisShift.get('Schedules.' + postLoadInfo.ScheduleIndex + ".Trips." + postLoadInfo.TripIndex) as FormGroup;
                    if (thisTrip) {
                        let thisDr = thisTrip.controls['DeliveryRequests']['controls'].find((d: FormGroup) => d.controls['Id'].value == postLoadInfo.DrId) as FormGroup;
                        if (thisDr) {
                            let updatedValues = {
                                RequiredQuantity: x.RequiredQuantity,
                                ScheduleStatus: 15,
                                Terminal: x.Terminal == undefined ? new DropdownItem() : x.Terminal,
                                BulkPlant: x.BulkPlant == undefined ? new DropAddressModel : x.BulkPlant,
                                OrderId: x.OrderId
                            };
                            if (!thisDr.controls['IsCommonBadge'].value && !x.IsCommonBadge) {
                                updatedValues['BadgeNo1'] = x.BadgeNo1 == null ? "" : x.BadgeNo1;
                                updatedValues['BadgeNo2'] = x.BadgeNo2 == null ? "" : x.BadgeNo2;
                                updatedValues['BadgeNo3'] = x.BadgeNo3 == null ? "" : x.BadgeNo3;
                                updatedValues['DispactherNote'] = x.DispactherNote == null ? "" : x.DispactherNote;
                            }
                            thisDr.patchValue(updatedValues);
                        }
                    }
                }
            }
        });
        this.changeDetectorRef.detectChanges();
    }
    private updateCompartmentPostloadDrs(drs: DeliveryRequestViewModel[], prepostloadStatus: number): void {
        var posttripInfo = null;
        let _shiftArray = this.SbForm.controls['Shifts'] as FormArray;
        drs.forEach(x => {
            let postLoadInfo = null;
            if (prepostloadStatus == 1) {
                postLoadInfo = x.PostLoadInfo;
            }
            else {
                postLoadInfo = x.PreLoadInfo;
            }
            if (postLoadInfo) {
                let thisShift = _shiftArray.controls.find((s: FormGroup) => s.controls['Id'].value == postLoadInfo.ShiftId) as FormGroup;
                if (thisShift) {
                    let thisTrip = thisShift.get('Schedules.' + postLoadInfo.ScheduleIndex + ".Trips." + postLoadInfo.TripIndex) as FormGroup;
                    if (thisTrip) {
                        posttripInfo = new ModifiedTripInfo();
                        posttripInfo.ShiftIndex = thisTrip['controls']['ShiftIndex'].value;
                        posttripInfo.DriverRowIndex = thisTrip['controls']['DriverRowIndex'].value;
                        posttripInfo.DriverColIndex = thisTrip['controls']['DriverColIndex'].value;
                        let thisDr = thisTrip.controls['DeliveryRequests']['controls'].find((d: FormGroup) => d.controls['Id'].value == postLoadInfo.DrId) as FormGroup;
                        if (thisDr) {
                            let compartmentInfo = x.Compartments;
                            let compartmentArray = thisDr.get('Compartments') as FormArray;
                            let prevCompartment = compartmentArray.getRawValue();
                            compartmentArray.clear();
                            let disabledControl = false;
                            if (x.TrackScheduleEnrouteStatus == 16 || x.TrackScheduleStatus == 7 || x.TrackScheduleStatus == 8 || x.TrackScheduleStatus == 9) {
                                disabledControl = true;
                            }
                            for (var i = 0; i < compartmentInfo.length; i++) {
                                if (prevCompartment.length > 0) {
                                    var trailerExists = prevCompartment.find(top => top.TrailerId == compartmentInfo[i].TrailerId);
                                    if (trailerExists) {
                                        compartmentArray.push(this.utilService.getCompartmentsFormGroup(x, compartmentInfo[i], disabledControl));
                                    }
                                    else {
                                        compartmentArray.push(this.utilService.getCompartmentsFormGroup(x, compartmentInfo[i], disabledControl));
                                    }
                                }
                                else {
                                    if (compartmentInfo[i].TrailerId != null && compartmentInfo[i].CompartmentId != null) {
                                        compartmentArray.push(this.utilService.getCompartmentsFormGroup(x, compartmentInfo[i], disabledControl));
                                    }
                                }
                            }
                            for (var i = 0; i < compartmentInfo.length; i++) {

                            }
                            window.setTimeout(() => { this.validateDrQuantity(thisDr.controls['RequiredQuantity']); }, 1);
                            thisDr.controls['ScheduleStatus'].patchValue(15);
                        }
                    }
                }
            }
        });
        this.changeDetectorRef.detectChanges();
        return posttripInfo;

    }
    private deletePreAndPostloadDrs(drs: DeliveryRequestViewModel[]): void {
        let drsToRestore = [];
        let _shiftArray = this.SbForm.controls['Shifts'] as FormArray;
        let postloadInfo = drs.filter(t => t.PostLoadInfo).map(m => m.PostLoadInfo);
        let preloadInfo = drs.filter(t => t.PreLoadInfo).map(m => m.PreLoadInfo);
        let prepostloadInfo = postloadInfo.concat(preloadInfo);
        prepostloadInfo = prepostloadInfo.filter((value, index, self) => self.indexOf(value) === index);
        prepostloadInfo.forEach(x => {
            if (x) {
                let thisShift = _shiftArray.controls.find((s: FormGroup) => s.controls['Id'].value == x.ShiftId) as FormGroup;
                if (thisShift) {
                    let thisTrip = thisShift.get('Schedules.' + x.ScheduleIndex + ".Trips." + x.TripIndex) as FormGroup;
                    if (thisTrip) {
                        let _drArray = thisTrip.controls['DeliveryRequests'] as FormArray;
                        let thisDr = _drArray.controls.find((d: FormGroup) => d.controls['Id'].value == x.DrId) as FormGroup;
                        if (thisDr) {
                            _drArray.removeAt(_drArray.controls.indexOf(thisDr));
                            if (!thisDr.controls['PostLoadedFor'] || !thisDr.controls['PostLoadedFor'].value) {
                                thisDr.value.Compartments = [];
                                drsToRestore.push(thisDr.value);
                            }
                        }
                    }
                }
            }
        });
        this.dataService.setScheduleChangeDetectSubject(true);
        if (drsToRestore && drsToRestore.length > 0) {
            this.dataService.setRestoreDeletedRequestSubject(drsToRestore);
        }
    }

    private editCompartmentAssignments(model: any): void {
        this.CompartmentDetails = [];
        this.CompartmentEditModel = model;
        let trailerIds = this.CompartmentEditModel.Schedule.Trailers.map(t => t.Id);
        let selectedTrailerId = 'null';
        if (trailerIds.length == 1) {
            selectedTrailerId = trailerIds[0];
        }
        this.CompartmentViewFormGroup = this.utilService.getCompartmentViewForm(this.CompartmentEditModel.Schedule.Trips, selectedTrailerId);
        this._loadingCmprts = true;
        this.sbService.getTrailerCompartments(trailerIds).subscribe(data => {
            this._loadingCmprts = false;
            if (data) {
                this.TrailerCompartmentRetains = data;
                for (var idx = 0; idx < data.length; idx++) {
                    if (data[idx].IsCompartmentAvailable) {
                        this.TrailerCompartments[data[idx].TrailerId] = data[idx].Compartments;
                    }
                }
                this.intializeTrailerRetainVal(trailerIds);
            }
            this.changeDetectorRef.detectChanges();

        });
    }
    intializeTrailerRetainVal(trailerIds: []) {
        if (trailerIds.length > 0) {
            var tripsArray = this.CompartmentViewFormGroup.controls['Trips'] as FormArray;
            tripsArray.controls.forEach((trips: FormGroup) => {
                var deliveryReqArray = trips.controls['DeliveryRequests'] as FormArray;
                deliveryReqArray.controls.forEach((delReq: FormGroup) => {
                    var compartmentInfo = delReq.controls['Compartments'] as FormArray;
                    if (compartmentInfo.length == 1) {
                        compartmentInfo.controls.forEach((comp: FormGroup) => {
                            var compTrailerId = comp.controls["TrailerId"].value;
                            var compQuantity = comp.controls["Quantity"].value;
                            var trailerExists = this.TrailerCompartmentRetains.find(x => x.TrailerId == compTrailerId);
                            if (trailerExists != null && trailerExists.IsCompartmentAvailable == false) {
                                if (compQuantity <= 0) {
                                    this.getRetainTrailerInfo(comp, delReq);
                                }


                            }
                        });


                    }
                });
            });

        }
    }
    public addCompartment(dr: FormGroup, trailerId: string): void {
        let compartments = dr.controls['Compartments'] as FormArray;
        if (!trailerId) {
            let trailerIds = this.CompartmentEditModel.Schedule.Trailers.map(t => t.Id);
            trailerId = trailerIds[0];
        }
        let model = new CompartmentModel();
        model.TrailerId = trailerId;
        let newComp = this.utilService.getCompartmentsFormGroup(dr.value, model, false);
        compartments.push(newComp);
    }

    public removeCompartment(dr: FormGroup, index: number): void {
        let compartments = dr.controls['Compartments'] as FormArray;
        compartments.removeAt(index);
        window.setTimeout(() => { this.validateDrQuantity(dr.controls['RequiredQuantity']); }, 1);
    }

    public saveCompartmentAssignment(): void {
        let trips = this.CompartmentViewFormGroup.controls['Trips'] as FormArray;
        trips.controls.forEach((x: FormGroup) => {
            if (!x.valid) {
                var delRequest = x.controls["DeliveryRequests"] as FormArray;
                delRequest.controls.forEach((_drForm: FormGroup) => {
                    let isRetainFuelLoaded = _drForm.controls['IsRetainFuelLoaded'].value;
                    let compartments = _drForm.controls["Compartments"] as FormArray;
                    compartments.controls.forEach((comp: FormGroup) => {
                        let compId = comp.get('CompartmentId').value;
                        if (compId == null || compId == '') {
                            comp.get('CompartmentId').setValidators(null);
                            comp.get('CompartmentId').updateValueAndValidity();
                            comp.get('Quantity').setValidators(null);
                            comp.get('Quantity').updateValueAndValidity();
                        }
                        else if (isRetainFuelLoaded) {
                            comp.get('Quantity').setValidators(null);
                            comp.get('Quantity').updateValueAndValidity();
                        }
                    });
                });
                x.markAllAsTouched();
            }
        });
        this.CompartmentViewFormGroup.markAllAsTouched();
        if (this.CompartmentViewFormGroup.invalid) {
            Declarations.msgerror('Please resolve highlighted errors', undefined, undefined);
            return;
        } else {
            let modifiedTrips = this.setCompartmentInfoToRow();
            if (modifiedTrips.length > 0) {
                this.saveScheduleBuilderData(modifiedTrips, false);
            }
        }
    }

    private setCompartmentInfoToRow(): ModifiedTripInfo[] {
        let modifiedTrips = [];
        let shiftIndex = this.CompartmentEditModel.ShiftIndex;
        let rowIndex = this.CompartmentEditModel.RowIndex;
        let targetRow = this.SbForm.get('Shifts.' + shiftIndex + '.Schedules.' + rowIndex) as FormGroup;
        if (targetRow) {
            let sourceLoads = this.CompartmentViewFormGroup.controls['Trips']['controls'];
            let targetLoads = targetRow.controls['Trips']['controls'];
            for (var loadIdx = 0; loadIdx < targetLoads.length; loadIdx++) {
                let sourceLoad = sourceLoads[loadIdx];
                let targetLoad = targetLoads[loadIdx];
                let sourceDrs = sourceLoad['controls']['DeliveryRequests']['controls'];
                let targetDrs = targetLoad['controls']['DeliveryRequests']['controls'];
                let tripInfo = null;
                for (var drIdx = 0; drIdx < targetDrs.length; drIdx++) {
                    let sourceDr = sourceDrs[drIdx];
                    let compartments = sourceDr['controls'].Compartments.getRawValue();
                    let targetDr = targetDrs[drIdx] as FormGroup;
                    let targetDrCompArray = targetDr.controls['Compartments'] as FormArray;

                    let delRequestUpdate = this.checkDeliveryRequestStatus(targetDr.controls['TrackScheduleStatus'].value, targetDr.controls['TrackScheduleEnrouteStatus'].value);
                    if (delRequestUpdate == false) {
                        targetDrCompArray.clear();
                        for (var cIdx = 0; cIdx < compartments.length; cIdx++) {
                            let compartment = compartments[cIdx];
                            targetDrCompArray.push(this.utilService.getCompartmentsFormGroupForDR(compartment));
                            if (sourceDr['controls'].IsRetainFuelLoaded.value && sourceDr['controls'].PostLoadedFor) {
                                targetDr.controls["IsRetainFuelLoaded"].patchValue(sourceDr['controls'].IsRetainFuelLoaded.value);
                                this.setDrValues(targetDr, sourceDr['controls'].RequiredQuantity.value, 1);
                                this.setPostLoadedFor(targetDr, sourceDr['controls'].PostLoadedFor.value);
                                this.setDrPickupLocation(targetDr, sourceDr.value);
                            }
                            targetDr.controls['ScheduleStatus'].patchValue(15);
                            tripInfo = new ModifiedTripInfo();
                            tripInfo.ShiftIndex = targetLoad['controls']['ShiftIndex'].value;
                            tripInfo.DriverRowIndex = targetLoad['controls']['DriverRowIndex'].value;
                            tripInfo.DriverColIndex = targetLoad['controls']['DriverColIndex'].value;
                        }
                    }
                }
                if (tripInfo) {
                    modifiedTrips.push(tripInfo);
                }
                var pretripInfo = this.intializePrePostLoadCompartmentInfo(targetLoad, 1);
                if (pretripInfo) {
                    modifiedTrips.push(pretripInfo);
                }
                //var posttripInfo = this.intializePreLoadCompartmentInfo(targetLoad); // not required - due to not provide edit value on postload DR.
                //if (posttripInfo) {
                //    modifiedTrips.push(posttripInfo);
                //}
            }
            Declarations.closeSlidePanel();
        }
        return modifiedTrips;
    }

    validateDrQuantity(drQty: any): void {
        drQty.updateValueAndValidity();
        this.changeDetectorRef.detectChanges();
    }

    trackByShiftId(index: number, shift: any): any {
        return shift.controls["Id"].value;
    }

    trackByScheduleIndex(index: number, schedule: any): any {
        return index;
    }

    trackByTripIndex(index: number, schedule: any): any {
        return index;
    }

    trackByDrId(index: number, dr: any): any {
        return dr.controls["Id"].value;
    }

    trackByDriverId(index: number, driver: any): any {
        return driver.controls["Id"].value;
    }

    trackByTrailerId(index: number, trailer: any): any {
        return trailer.controls["TrailerId"].value;
    }

    intializePrePostLoadCompartmentInfo(targetLoad: any, prepostDRStatus: number) {
        var tripInfo = null;
        let updatedPrePostloadedDrs = targetLoad['controls']['DeliveryRequests'].value;
        if (updatedPrePostloadedDrs.length > 0) {
            var prepostloadedDrsStatus = null;
            if (prepostDRStatus) {
                prepostloadedDrsStatus = updatedPrePostloadedDrs.filter(x => x.PostLoadInfo);
            }
            else {
                prepostloadedDrsStatus = updatedPrePostloadedDrs.filter(x => x.PreLoadInfo);
            }
            if (prepostloadedDrsStatus.length > 0) {
                tripInfo = this.updateCompartmentPostloadDrs(prepostloadedDrsStatus, prepostDRStatus);
            }
        }
        return tripInfo;
    }

    public TrailerRetainDetails(TrailerId: string) {
        var Id = [TrailerId];
        this.CompartmentDetails = [];
        this._loadingCmprts = true;
        this.sbService.getTrailerFuelRetain(Id).subscribe(data => {
            this.CompartmentDetails = data;
            this._loadingCmprts = false;
            this.changeDetectorRef.detectChanges();
        });
    }

    public closeRetainInfo(): void {
        this.CompartmentDetails = [];
    }

    GetfuelRetainDetails(trailerIds: any[]): void {
        this.sbService.getTrailerFuelRetain(trailerIds).subscribe(data => {
            if (data != null && data.length > 0) {
                var trailerFuelRetainInfo = data as TrailerFuelRetainModel[];
                var trailerNames = '';
                trailerFuelRetainInfo.forEach(x => {
                    if (trailerNames == '') {
                        trailerNames = x.TrailerId;
                    }
                    else {
                        if (trailerNames.indexOf(x.TrailerId) == -1) {
                            trailerNames = trailerNames + ", " + x.TrailerId;
                        }
                    }
                });
                Declarations.msgwarning("There is some fuel retained in the " + trailerNames + ".", undefined, undefined);
            }
        });
    }

    checkIfTrailerFuelRetainExists(trailerId: string): boolean {
        let status = false;
        var trailerInfo = this.TrailerCompartmentRetains.find(x => x.TrailerId.toString() == trailerId);
        if (trailerInfo != null) {
            status = trailerInfo.IsFuelRetain;
        }
        return status;
    }

    checkDeliveryRequestStatus(delStatus: any, delEncStatus: any) {
        if (delEncStatus == 16 || this.CompletedScheduleStatuses.filter(x => x == delStatus).length > 0) {
            return true;
        }
        return false;
    }

    toggleFilldCompatible() {
        this.SelectedTrailers = [];
        this.UnassignedDrivers = [];
        this.DriverTrailerForm.get('Driver').patchValue(null);
        this.UnassignedTrailers = this.getUnassignedTrailers(this.selTrailerIndex, this.selTrailerlist)
        this.UnassignedDrivers = this.getUnassignedDrivers(true);
    }

    getRetainCompartmentInfo(comp: FormGroup, dr: any) {
        let compartmentCtrl = comp.value;
        let delRequestCtrl = dr.value;
        var fuelRetain = this.TrailerCompartmentRetains.find(x => x.IsFuelRetain == true && x.TrailerId == compartmentCtrl.TrailerId);
        if (fuelRetain) {
            var compartment = fuelRetain.Compartments.find(x => x.CompartmentId == compartmentCtrl.CompartmentId);
            if (compartment && compartment.RetainInfo) {
                comp.controls["Quantity"].patchValue(compartment.RetainInfo.Quantity);
                dr.controls["IsRetainFuelLoaded"].patchValue(true);
                let drQty = this.getDRTotalQuantity(dr);
                this.setDrValues(dr, drQty, 1);
                this.setPostLoadedFor(dr, compartment.RetainInfo.DeliveryReqId);
                this.setDrPickupLocation(dr, {
                    PickupLocationType: compartment.RetainInfo.PickupLocationType,
                    Terminal: compartment.RetainInfo.TfxTerminal,
                    BulkPlant: compartment.RetainInfo.TfxBulkPlant
                });
            } else {
                comp.controls["Quantity"].enable();
                comp.controls["Quantity"].patchValue(0);
                this.resetQuantity(dr, delRequestCtrl);
            }
        } else {
            this.resetQuantity(dr, delRequestCtrl);
        }
        delRequestCtrl = dr.value;
        if (delRequestCtrl.ScheduleQuantityType <= 1) {
            let quantityValidators = [Validators.required, Validators.pattern(RegExConstants.DecimalNumber), Validators.min(0.00000001), Validators.max(delRequestCtrl.RequiredQuantity)];
            comp.controls['Quantity'].setValidators(quantityValidators);
            comp.updateValueAndValidity();
        }
        this.changeDetectorRef.detectChanges();
    }
    getRetainTrailerInfo(comp: FormGroup, dr: any) {
        let compartmentCtrl = comp.value;
        let delRequestCtrl = dr.value;
        var fuelRetain = this.TrailerCompartmentRetains.find(x => x.IsFuelRetain == true && x.TrailerId == compartmentCtrl.TrailerId);
        if (fuelRetain && fuelRetain.IsCompartmentAvailable == false && fuelRetain.IsFuelRetain) {
            comp.controls["CompartmentId"].disable();
            comp.controls["Quantity"].disable();
            var compartment = fuelRetain.Compartments[0];
            if (compartment && compartment.RetainInfo) {
                comp.controls["Quantity"].patchValue(compartment.RetainInfo.Quantity);
                dr.controls["IsRetainFuelLoaded"].patchValue(true);
                this.setDrValues(dr, compartment.RetainInfo.Quantity, 1);
                this.setPostLoadedFor(dr, compartment.RetainInfo.DeliveryReqId);
                this.setDrPickupLocation(dr, {
                    PickupLocationType: compartment.RetainInfo.PickupLocationType,
                    Terminal: compartment.RetainInfo.TfxTerminal,
                    BulkPlant: compartment.RetainInfo.TfxBulkPlant
                });

            } else {
                comp.controls["CompartmentId"].enable();
                comp.controls["Quantity"].enable();
                comp.controls["Quantity"].patchValue(0);
                this.resetQuantity(dr, delRequestCtrl);
            }
        } else {
            comp.controls["CompartmentId"].enable();
            comp.controls["Quantity"].enable();
            comp.controls["Quantity"].patchValue(0);
            this.resetQuantity(dr, delRequestCtrl);
        }
        this.changeDetectorRef.detectChanges();
    }
    private getDRTotalQuantity(dr: any): number {
        let drValue = dr.value; let quantity = 0;
        let assignedComps = drValue.Compartments.map(x => { return { TrailerId: x.TrailerId, CompartmentId: x.CompartmentId }; });
        for (var i = 0; i < this.TrailerCompartmentRetains.length; i++) {
            let trailer = this.TrailerCompartmentRetains[i];
            for (var j = 0; j < trailer.Compartments.length; j++) {
                let comp = trailer.Compartments[j];
                let assigned = assignedComps.find(x => x.TrailerId == trailer.TrailerId && x.CompartmentId == comp.CompartmentId);
                if (assigned && comp.RetainInfo) {
                    quantity += comp.RetainInfo.Quantity;
                }
            }
        }
        return quantity;
    }

    private resetQuantity(dr: any, delRequestCtrl: any): void {
        if (delRequestCtrl.ScheduleQuantityTypeText == 'Not Specified') {
            this.setDrValues(dr, 0, 5);
        } else if (delRequestCtrl.ScheduleQuantityTypeText == 'Small Compartment') {
            this.setDrValues(dr, 0, 4);
        } else if (delRequestCtrl.ScheduleQuantityTypeText == 'Full Load') {
            this.setDrValues(dr, 0, 3);
        } else if (delRequestCtrl.ScheduleQuantityTypeText == 'Balance') {
            this.setDrValues(dr, 0, 2);
        } else {
            this.setDrValues(dr, delRequestCtrl.RequiredQuantity, 1);
        }
        dr.controls["IsRetainFuelLoaded"].patchValue(false);
        let drQuantity = dr.controls["DrQuantity"].value;
        dr.patchValue({ IsRetainFuelLoaded: false, RequiredQuantity: drQuantity });
    }

    private setDrValues(dr: any, quantity: number, type: number): void {
        dr.patchValue({
            ScheduleQuantityType: type,
            RequiredQuantity: quantity
        });
    }

    private setDrPickupLocation(dr: any, source: any): void {
        if (source.PickupLocationType == 2 && source.BulkPlant) {
            dr.patchValue({
                BulkPlant: source.BulkPlant,
                PickupLocationType: source.PickupLocationType,
            });
        } else if (source.Terminal) {
            dr.patchValue({
                Terminal: source.Terminal,
                PickupLocationType: source.PickupLocationType,
            });
        }
    }

    private setPostLoadedFor(dr: any, drId: string): void {
        if (dr.controls['PostLoadedFor']) {
            dr.controls['PostLoadedFor'].patchValue(drId);
        } else {
            dr.addControl("PostLoadedFor", this.fb.control(drId));
        }
    }

    validatePrePostLoadTrailer(_form: any) {
        var status = true;
        var trailerDetails = this.DriverTrailerForm.controls['Trailers'].value;
        var schedule = this.SbForm.get('Shifts.' + _form.Index1 + '.Schedules.' + _form.Index2 + '') as FormGroup;
        var assignedTrailer = schedule.get('Trailers').value;
        var trips = schedule.controls['Trips'] as FormArray;
        var _deliveryRequests = this.GetAllLoadsDR(trips);
        var PreLoadInfoLength = _deliveryRequests.filter(x => x.PreLoadedFor).length;
        var PostLoadInfoLength = _deliveryRequests.filter(x => x.PostLoadedFor).length;
        if (PostLoadInfoLength > 0 || PreLoadInfoLength > 0) {
            for (var i = 0; i < assignedTrailer.length; i++) {
                if (trailerDetails.findIndex(x => x.Id == assignedTrailer[i].Id) == -1) {
                    Declarations.msgerror('There is Preload/Postload DS in the shift. Please remove the Preload/Postload in order to change the assignment.', undefined, undefined);
                    status = false;
                    return status;
                }
            }
        }
        return status;
    }
    TransferDS(jobId: number, trip: FormGroup, DrIndex: number, ShiftIndex: number, RowIndex: number, ColIndex: number, schedule: any) {
        var schgeduleFormGroup = schedule as FormGroup;
        this.selectedColIndex = ColIndex;
        this.selectedRowIndex = RowIndex;
        this.selectedDefaultShiftIndex = ShiftIndex;
        let drs = trip.controls['DeliveryRequests'].value;
        let filteredDRs = drs.filter(x => x.JobId == jobId && !x.PreLoadedFor && !x.PostLoadedFor);
        this.$eventDataTransfer.dragData = new DragDSInfo();
        this.$eventDataTransfer.dragData.Data = filteredDRs;
        this.$eventDataTransfer.dragData.TripFrom = trip;
        this.$eventDataTransfer.dragData.DrIndex = DrIndex;
        this.$eventDataTransfer.dragData.ShiftIndex = ShiftIndex;
        this.$eventDataTransfer.dragData.RowIndex = RowIndex;
        this.$eventDataTransfer.dragData.ColIndex = ColIndex;
        this.$eventDataTransfer.dragData.Schedule = schgeduleFormGroup;
        this.GetShiftColumnInformation();

    }
    GetShiftColumnInformation() {
        this._loadingDSData = true;
        this.SelectedRegionId = MyLocalStorage.getData(MyLocalStorage.DSB_REGION_KEY);
        var selectedDate = MyLocalStorage.getData(MyLocalStorage.DSB_DATE_KEY);
        this.sbService.getDriversShifts(this.SelectedRegionId, selectedDate).subscribe(x => {
            this._loadingDSData = false;
            if (x) {
                this.DSShifts = x;
                this.DSShifts.forEach(top => {
                    top.ShiftInfo = "Shift - " + top.StartTime + " - " + top.EndTime;
                });
                if (this.DSShifts.length > 0) {
                    this.SelectedShift = this.DSShifts[0];
                    this.GetShiftColInfo(this.SelectedShift);
                }
            }
        });
        this.changeDetectorRef.detectChanges();
    }
    TransferDSInfo() {
        let shiftInfoDetails = this.DSShifts.find(top => top.Id == this.SelectedShift.Id && top.StartTime == this.SelectedShift.StartTime);
        if (shiftInfoDetails != null) {
            let shiftInfo = this.SbForm.controls['Shifts']['controls'].find((f: FormGroup) => f.controls['Id'].value == shiftInfoDetails.Id) as FormGroup;
            let shiftColIndexDetails = this.ShiftColumn.find(top => top.ColIndex == this.SelectedShiftColumn.ColIndex);
            if (shiftColIndexDetails != null) {
                this.$eventDataTransfer.dragData.DropSchedule = shiftInfo.controls["Schedules"]["controls"][shiftColIndexDetails.ColIndex];
                let shiftLoadIndexDetails = this.ShiftLoad.find(top => top.LoadIndex == this.SelectedLoad.LoadIndex);
                if (shiftLoadIndexDetails != null) {
                    this.$eventDataTransfer.dragData.DropTrip = shiftInfo.controls["Schedules"]["controls"][shiftColIndexDetails.ColIndex]["controls"]["Trips"]["controls"][shiftLoadIndexDetails.LoadIndex];
                    this.onItemDrop(this.$eventDataTransfer.dragData.DropTrip, this.$eventDataTransfer, this.SelectedShiftIndex, this.SelectedShiftColumn.ColIndex, this.SelectedLoad.LoadIndex, this.$eventDataTransfer.dragData.DropSchedule);

                }
            }
        }
    }
    GetShiftColInfo(shift: ShiftViewModel) {
        this.ShiftColumn = [];
        let shiftInfoDetails = this.DSShifts.findIndex(top => top.Id == shift.Id && top.StartTime == shift.StartTime);
        if (shiftInfoDetails != -1) {
            this.SelectedShift = this.DSShifts[shiftInfoDetails];
            let shiftInfo = this.SbForm.controls['Shifts']['controls'].find((f: FormGroup) => f.controls['Id'].value == this.SelectedShift.Id) as FormGroup;
            this.SelectedShiftIndex = this.SbForm.controls['Shifts']['controls'].findIndex((f: FormGroup) => f.controls['Id'].value == this.SelectedShift.Id);
            var columnInfo = shiftInfo.controls["Schedules"].value.length;
            var scheduleInfo = shiftInfo.controls["Schedules"].value as any[];
            for (var i = 0; i < columnInfo; i++) {
                var scheduleColIndex = scheduleInfo.findIndex(x => x.DriverRowIndex == i);
                if (scheduleColIndex >= 0) {
                    var shiftColumnInfo = new ShiftColumnInfo();
                    shiftColumnInfo.ColIndex = i;
                    shiftColumnInfo.ColIndexName = "C" + (i + 1);
                    this.ShiftColumn.push(shiftColumnInfo);
                }
            }
            if (this.ShiftColumn.length > 0) {
                this.SelectedShiftColumn = this.ShiftColumn[0];
            }
            this.GetShiftLoadInfo(this.SelectedShiftColumn);
        }
    }
    GetShiftLoadInfo(shiftColumn: ShiftColumnInfo, colIndex: number = -1, rowIndex: number = -1) {
        this.SelectedDriverName = '';
        this.ShiftLoad = [];
        let shiftInfo = this.SbForm.controls['Shifts']['controls'].find((f: FormGroup) => f.controls['Id'].value == this.SelectedShift.Id) as FormGroup;
        let shiftColIndexDetails = this.ShiftColumn.find(top => top.ColIndex == shiftColumn.ColIndex);
        if (shiftColIndexDetails != null) {
            var loadDetails = shiftInfo.controls["Schedules"]["controls"][shiftColIndexDetails.ColIndex]["controls"]["Trips"].value.length;
            for (var i = 0; i < loadDetails; i++) {
                var shiftLoadInfo = new ShiftLoadInfo();
                if (this.selectedRowIndex == shiftColIndexDetails.ColIndex && this.selectedDefaultShiftIndex == this.SelectedShiftIndex) {
                    if (this.selectedColIndex != i) {
                        shiftLoadInfo.LoadIndex = i;
                        this.initShiftLoad(shiftLoadInfo, i);
                    }
                }
                else {
                    shiftLoadInfo.LoadIndex = i;
                    this.initShiftLoad(shiftLoadInfo, i);
                }
            }
            let driverDetails = shiftInfo.controls["Schedules"]["controls"][shiftColIndexDetails.ColIndex]["controls"]["Drivers"].value;
            if (driverDetails) {
                this.SelectedDriverName = $.map(driverDetails, function (obj) {
                    return obj.Name
                }).join(',');
            }
        }
        if (this.ShiftLoad.length > 0) {
            this.SelectedLoad = this.ShiftLoad[0];
        }
        this.changeDetectorRef.detectChanges();
    }
    subscribeTransferDSEvents(): void {
        this.TrasnferDSSubscription = this.dataService.TransferDSInfoSubject.subscribe(x => {
            if (x) {
                this.TransferDS(x.jobId, x.trip, x.DrIndex, x.ShiftIndex, x.RowIndex, x.ColIndex, x.schedule);
            }
        });
    }
    initShiftLoad(shiftLoadInfo: any, i: number) {
        if (i < 10) {
            shiftLoadInfo.LoadName = "0" + (i + 1);
        }
        else {
            shiftLoadInfo.LoadName = (i + 1).toString();
        }
        this.ShiftLoad.push(shiftLoadInfo);
    }
    subscribeTrailerInfoDSEvents(): void {
        this.TrailerInfoDSSubscription = this.dataService.TrailerDSInfoSubject.subscribe(x => {
            if (x) {
                this.scheduleTrailerInfo = x;
                this.changeDetectorRef.detectChanges();
            }
        });
    }
    subscribeTrailerRemoveDSEvents(): void {
        this.RemoveTrailerGroupSubject = this.dataService.RemoveTrailerGroupSubject.subscribe(x => {
            if (x) {
                this._loadingRemoveTrailer = false;
                this.changeDetectorRef.detectChanges();
                //hide trailer information popup if no trailer found.
                setTimeout(() => {
                    if (this.scheduleTrailerInfo != null) {
                        var scInfo = this.scheduleTrailerInfo.controls['Trips']['controls'][0];
                        if (scInfo != null) {
                            var schedule = this.SbForm.get('Shifts.' + scInfo.controls['ShiftIndex'].value + '.Schedules.' + scInfo.controls['DriverRowIndex'].value) as FormGroup;
                            if (schedule != null) {
                                if (schedule.controls['Trailers'].value.length == 0) {
                                    $("#closeTrailerInformationModal").click();
                                }
                            }
                        }
                    }
                }, 1000);

            }
        });
    }
    subscribeRouteInfoDSEvents(): void {
        this.RouteInfoDSSubscription = this.dataService.RouteDetailsSubject.subscribe(x => {
            if (x) {
                this.SelectedTrip = x.SelectedTrip;
                this.RouteListForTrip = x.RouteListForTrip;
            }
        });
    }

    UnAssignTrailerFromShift(schedule: any, type: any, trailerId: string = '') {
        let data = {
            schedule: schedule,
            type: type,
            trailerId: trailerId
        }
        this._loadingRemoveTrailer = true;
        this.changeDetectorRef.detectChanges();
        this.dataService.unassignTrailerInformationSubject(data);
    }
    public deleteDrsForRoute(route: DropdownItem) {
        this._loadingRemoveRoute = true;
        this.changeDetectorRef.detectChanges();
        this.dataService.setRouteResetGroupSubject(route);
    }
    public hideDeliveryGroup() {
        this.dataService.setHideDeliveryGroupSubject(true);
    }
    subscribeRouteDeleteDeliveryGroupSubject(): void {
        this.RouteDeleteDeliveryGroupSubject = this.dataService.RouteDeleteDeliveryGroupSubject.subscribe(x => {
            if (x) {
                this.deleteRouteGroup(x);
                this.changeDetectorRef.detectChanges();

            }
        });
    }
    deleteRouteGroup(routeDelete: any) {
        var data = routeDelete.DelGroupForm;
        this.routeInfo = routeDelete.routeName;
        this.dataService.setShowDeliveryGroupSubject(false);
        if (data.get('GroupId').value != null && data.get('GroupId').value > 0) {
            this._savingBuilder = true;
            this.changeDetectorRef.detectChanges();
            var delRequests = data.get('DeliveryRequests') as FormArray;
            var scheduleIds;
            if (this.routeInfo != null) {
                scheduleIds = delRequests.value.filter(top => top.RouteInfo != null && top.RouteInfo.Id == this.routeInfo.Id).map(t => t.TrackableScheduleId);
            }
            else {
                scheduleIds = delRequests.value.map(t => t.TrackableScheduleId)
            }
            this.sbService.getScheduleStatus(scheduleIds).subscribe(response => {
                this._savingBuilder = false;
                if (response != null && response != undefined && response.length > 0) {
                    var completedSchedules = this.sbService.returnCommonElements(this.CompletedScheduleStatuses, response, true);
                    var isCompletedSchedule = completedSchedules.length > 0;
                    var onTheWaySchedules = this.sbService.returnCommonElements(this.OnTheWayScheduleStatuses, response, false);
                    if (isCompletedSchedule || response.filter(t => t.ScheduleEnrouteStatusId == 4).length > 0) {
                        Declarations.msgerror("Can't delete/reset group. For one or more schedule(s) drop has been made already", 'Warning', 2500);
                        return;
                    } else if (onTheWaySchedules.length > 0) {
                        this.sbService.setConfirmationHeadingForDeleteGroup(onTheWaySchedules[0]);
                        this.DeletedGroup = data;
                        jQuery('#btnconfirm-delete-delgrp').click();
                        return;
                    } else {
                        this.deleteRouteLoad(data);
                    }
                } else {
                    this.deleteRouteLoad(data);
                }

            });
        }
        else {
            this.deleteRouteLoad(data);
        }
    }
    deleteRouteLoad(trip: FormGroup) {
        var delRequestIndex = [];
        var tempDeliveryReq = [];
        var delRequests = trip.controls['DeliveryRequests'] as FormArray;
        delRequests.value.forEach(t => { t.WindowMode = 1; t.QueueMode = 1; t.Compartments = [] });
        if (this.routeInfo != null) {
            delRequests.controls.forEach((element: any) => {
                var drRouteInfo = element.controls['RouteInfo'].value;
                if (drRouteInfo != null && drRouteInfo.Id == this.routeInfo.Id) {
                    tempDeliveryReq.push(element.value);
                    delRequestIndex.push(element.controls['Id'].value);
                }
            });
            if (delRequestIndex.length > 0) {
                delRequestIndex.forEach(x => {
                    var drIndex = delRequests.controls.findIndex(top => top.get('Id').value == x);
                    if (drIndex >= 0) {
                        delRequests.removeAt(drIndex);
                    }
                });
            }
        }
        var tripId = trip.controls['TripId'].value;
        if (tripId != null && tripId != undefined && tripId != '') {
            var deliveryRequestsValue;
            if (this.routeInfo != null) {
                deliveryRequestsValue = tempDeliveryReq;
            }
            else {
                deliveryRequestsValue = delRequests.value;
            }
            var dsbModel = this.getDSBSaveModel();
            dsbModel.DeletedGroupId = trip.controls['GroupId'].value;
            dsbModel.DeletedTripId = tripId;

            let preloadInfo = this.getPreloadedInfoFromLoad(trip);
            let preloadedTrips = this.getPrePostLoadedTrips(preloadInfo);
            let postloadInfo = this.getPostloadedInfoFromLoad(trip);
            let postloadedTrips = this.getPrePostLoadedTrips(postloadInfo);
            dsbModel.PreloadedDRs = this.getPreloadAcrossTheDateInfoFromLoad(trip);
            dsbModel.PostloadedDRs = this.getPostloadAcrossTheDateInfoFromLoad(trip);
            if (this.routeInfo == null) {
                delRequests.clear();
            }
            var schedule = this.SbForm.get('Shifts.' + trip.controls['ShiftIndex'].value + '.Schedules.' + trip.controls['DriverRowIndex'].value) as FormGroup;
            trip.value.Drivers = schedule.controls['Drivers'].value;
            trip.value.Trailers = schedule.controls['Trailers'].value;
            if (this.routeInfo != null) {
                let isCommonPickup = trip.controls['IsCommonPickup'].value;
                let IsDispatcherDragDropSequence = trip.get('IsDispatcherDragDropSequence').value as boolean;
                var drForm = trip.controls['DeliveryRequests'] as FormArray;
                let tripDeliveryRequests = drForm.value;
                drForm.clear();
                var _drFormArray = this.utilService.getDeliveryRequestFormArray(tripDeliveryRequests, isCommonPickup, IsDispatcherDragDropSequence, 0);
                _drFormArray.controls.forEach((_drForm: FormGroup) => {
                    drForm.push(_drForm);
                });
                trip.value.DeliveryRequests = trip.controls['DeliveryRequests'].value;
            }
            dsbModel.Trips.push(trip.value);
            dsbModel.Status = 4;

            let tripArray = new FormArray([]);
            tripArray.push(trip);
            preloadedTrips.forEach(x => {
                dsbModel.Trips.push(x.value);
                tripArray.push(x);
            });
            postloadedTrips.forEach(x => {
                dsbModel.Trips.push(x.value);
                tripArray.push(x);
            });

            this._savingBuilder = true;
            this.sbService.deleteGroup(dsbModel).subscribe(response => {
                if (response != null) {
                    if (response.StatusCode == 0 || response.StatusCode == 2) {
                        let drsToRestore = deliveryRequestsValue.filter(t => !t.PostLoadedFor);
                        this.dataService.setRestoreDeletedRequestSubject(drsToRestore);
                        this.resetLoad(trip);
                    }
                    this.updateLoadForm(response, tripArray);
                    if (this.routeInfo != null) {
                        this.RouteListForTrip = [];
                        let drValue = trip.controls['DeliveryRequests'].value as DeliveryRequestViewModel[];
                        drValue.forEach(dr => {
                            if (dr && dr.RouteInfo && dr.PostLoadedFor == undefined) {
                                let drIndex = this.RouteListForTrip.findIndex(a => a.Id == dr.RouteInfo.Id);
                                if (drIndex === -1) { this.RouteListForTrip.push(dr.RouteInfo); }
                            }
                        });
                        if (this.RouteListForTrip.length == 0) {
                            $("#closeResetDrByRouteModal").click();
                        }
                    }
                }
                else {
                    Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
                }
                this._savingBuilder = false;
            });
        } else {
            this.RouteListForTrip = [];
            var deliveryRequestsValue;
            if (this.routeInfo != null) {
                deliveryRequestsValue = tempDeliveryReq;
            }
            else {
                deliveryRequestsValue = delRequests.value;
            }
            this.dataService.setRestoreDeletedRequestSubject(deliveryRequestsValue);
            let drValue = trip.controls['DeliveryRequests'].value as DeliveryRequestViewModel[];
            drValue.forEach(dr => {
                if (dr && dr.RouteInfo && dr.PostLoadedFor == undefined) {
                    let drIndex = this.RouteListForTrip.findIndex(a => a.Id == dr.RouteInfo.Id);
                    if (drIndex === -1) { this.RouteListForTrip.push(dr.RouteInfo); }
                }
            });
            if (this.RouteListForTrip.length == 0) {
                $("#closeResetDrByRouteModal").click();
            }
            this.resetLoad(trip);
            this.dataService.setScheduleChangeDetectSubject(true);
        }
    }

    public createLoad(x: any) {
        if (x.IsDragFromLoad == true) {
            this.DropDelReqToLoad(x.Schedule, x.Trip, x.DrData, x.Drs);
            return;
        }
        if (x && x.Drs.length > 0) {
            let drs = x.Drs as DeliveryRequestViewModel[];
            var recurringDeliveryRequests = drs.filter(top => top.isRecurringSchedule == true && top.IsBlendedRequest == false);
            recurringDeliveryRequests = recurringDeliveryRequests.filter((el, i, a) => i === a.indexOf(el));
            var recurringBlendedDeliveryRequests = drs.filter(top => top.isRecurringSchedule == true && top.IsBlendedRequest == true);
            recurringBlendedDeliveryRequests = recurringBlendedDeliveryRequests.filter((el, i, a) => i === a.indexOf(el));

            var deliveryRequests = sortBy(drs.filter(top => top.isRecurringSchedule == false && top.GroupParentDRId == '' && (top.CarrierStatus == 0 || top.CarrierStatus == 3 || top.CarrierStatus == 4)), 'ProductType');
            var brokeredDeliveryRequests = drs.filter(top => top.isRecurringSchedule == false && top.GroupParentDRId == '' && top.CarrierStatus == 2);
            var splitDeliveryRequests = drs.filter(top => top.GroupParentDRId != '' && top.IsBlendedRequest === false);
            var tbdDrs = deliveryRequests.filter(t => t.IsTBD == true);
            var groupedDeliveryRequests = groupDrsByProduct(deliveryRequests.filter(t => t.IsTBD == false && t.IsBlendedRequest === false && t.IsMarine === false && (t.IndicativePrice == null || t.IndicativePrice == 0)));
            var splitBlendedDeliveryRequests = drs.filter(top => top.GroupParentDRId != '' && top.IsBlendedRequest === true);
            deliveryRequests.filter(t => (t.IsMarine === true || (t.IndicativePrice != null && t.IndicativePrice > 0)) && t.GroupParentDRId == '' && t.IsTBD == false && t.IsBlendedRequest === false).forEach(x => {
                groupedDeliveryRequests.push(x);
            });
            deliveryRequests.filter(t => t.IsBlendedRequest === true).forEach(x => {
                if (!groupedDeliveryRequests.some(g => g.BlendedGroupId == x.BlendedGroupId)) {
                    var blendDrs = deliveryRequests.filter(b => b.BlendedGroupId == x.BlendedGroupId);
                    blendDrs.filter(b => !b.IsAdditive && b.IsBlendedDrParent).forEach(p => { groupedDeliveryRequests.push(p); });
                    blendDrs.filter(b => !b.IsAdditive && !b.IsBlendedDrParent).forEach(p => { groupedDeliveryRequests.push(p); });
                    blendDrs.filter(b => b.IsAdditive && b.IsBlendedDrParent).forEach(p => { groupedDeliveryRequests.push(p); });
                    blendDrs.filter(b => b.IsAdditive && !b.IsBlendedDrParent).forEach(p => { groupedDeliveryRequests.push(p); });
                }
            });
            //recurring schedule blended DRs.
            if (recurringBlendedDeliveryRequests.length > 0) {
                recurringBlendedDeliveryRequests.forEach(x => {
                    if (!groupedDeliveryRequests.some(g => g.BlendedGroupId == x.BlendedGroupId)) {
                        var blendDrs = recurringBlendedDeliveryRequests.filter(b => b.BlendedGroupId == x.BlendedGroupId);
                        blendDrs.filter(b => !b.IsAdditive && b.IsBlendedDrParent).forEach(p => { groupedDeliveryRequests.push(p); });
                        blendDrs.filter(b => !b.IsAdditive && !b.IsBlendedDrParent).forEach(p => { groupedDeliveryRequests.push(p); });
                        blendDrs.filter(b => b.IsAdditive).forEach(p => { groupedDeliveryRequests.push(p); });
                    }
                });
            }
            tbdDrs.forEach(x => {
                groupedDeliveryRequests.push(x);
            });
            recurringDeliveryRequests.forEach(x => {
                groupedDeliveryRequests.push(x);
            });

            if (groupedDeliveryRequests.length == 0) {
                brokeredDeliveryRequests.forEach(x => {
                    groupedDeliveryRequests.push(x);
                });
            }
            if (groupedDeliveryRequests.length == 0) {
                splitDeliveryRequests.forEach(x => {
                    groupedDeliveryRequests.push(x);
                });
            }
            splitBlendedDeliveryRequests.filter(t => t.IsBlendedRequest === true).forEach(x => {
                if (!groupedDeliveryRequests.some(g => g.BlendedGroupId == x.BlendedGroupId)) {
                    var blendDrs = splitBlendedDeliveryRequests.filter(b => b.BlendedGroupId == x.BlendedGroupId);
                    blendDrs.filter(b => !b.IsAdditive && b.IsBlendedDrParent).forEach(p => { groupedDeliveryRequests.push(p); });
                    blendDrs.filter(b => !b.IsAdditive && !b.IsBlendedDrParent).forEach(p => { groupedDeliveryRequests.push(p); });
                    blendDrs.filter(b => b.IsAdditive).forEach(p => { groupedDeliveryRequests.push(p); });
                }
            });
            groupedDeliveryRequests.slice();
            let drIds = deliveryRequests.filter(t => t.isRecurringSchedule == false && (t.CarrierStatus == 0 || t.CarrierStatus == 3 || t.CarrierStatus == 4)).map(m => m.Id);
            var input = { jobId: x.JobId, regionId: x.RegionId, customer: x.Customer, deliveryReqs: groupedDeliveryRequests, existingDrIds: drIds };
            this.sbService.getDeliveryReqDemands(input).subscribe(response => {
                if (response == null) {
                    this._savingBuilder = false;
                    this.changeDetectorRef.detectChanges();
                    Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
                }
                else if (response.Status && response.Status.StatusCode != 0) {
                    this._savingBuilder = false;
                    this.changeDetectorRef.detectChanges();
                    Declarations.msgerror(response.Status.StatusMessage, undefined, undefined);
                }
                else {
                    if (response.Data && response.Data.length > 0) {
                        let isCommonPickup = x.Trip.get('IsCommonPickup').value as boolean;
                        let IsDispatcherDragDropSequence = x.Trip.get('IsDispatcherDragDropSequence').value as boolean;
                        x.DrData.Data = new FormArray([]);
                        x.DrData.Data = this.utilService.getDeliveryRequestFormArray(response.Data, isCommonPickup, IsDispatcherDragDropSequence, 0);
                        x.Drs.forEach((req, i) => {
                            if (response.Data.findIndex(t => t.Id == req.Id || (t.ProductTypeId == req.ProductTypeId
                                && (t.CarrierStatus == 0 || t.CarrierStatus == 3 || t.CarrierStatus == 4))) == -1) {
                                x.Drs.splice(i, 1);
                            }
                        })
                        this.DropDelReqToLoad(x.Schedule, x.Trip, x.DrData, x.Drs);
                        this.dataService.setDrUpdatedSubject(response.Data);
                    }
                    else {
                        this._savingBuilder = false;
                        this.changeDetectorRef.detectChanges();
                    }
                }
            });
        }
    }
    //if all columns collapsed, then dont show shift
    checkShiftVisibility(shift: FormGroup) {
        const _schedules = shift.controls['Schedules'].value as any[];
        if (_schedules.every(x => x.IsLoadQueueCollapsed == true))
            return false;
        else
            return true;
    }
    //collapse all columns of shift
    moveShiftToLoadQueue(shiftIndex: number, shift: FormGroup) {

        let data: DSBLoadQueueModel[] = [];
        let _Schedules = shift.controls['Schedules'] as FormArray;

        _Schedules.controls.forEach((schedule: FormGroup, scheduleIndex: number) => {
            //all columns (one shift) collapsed in client side
            schedule.get('IsLoadQueueCollapsed').patchValue(true);
            data.push({
                Id: null,
                RegionId: this.SbForm.get('RegionId').value,
                ShiftIndex: shiftIndex,
                Date: this.SbForm.get('Date').value,
                DriverRowIndex: scheduleIndex,
                ScheduleBuilderId: '',
                ShiftId: shift.controls['Id'].value,
            })
        });

        this.loadQueueService.setLoadQueueColumnMoved(true);
        this.changeDetectorRef.markForCheck();
        this.dataService.setDriverColumnChangeDetect(true);

        //collapsed column data sent to api
        this.loadQueueService.createLoadQueue(data).subscribe((data) => {
            if (data && data.StatusCode == 0) {
                if (data.DsbLoadQueueSuccess && data.DsbLoadQueueSuccess.length > 0) {
                    _Schedules.controls.forEach((schedule: FormGroup, scheduleIndex: number) => {
                        schedule.get('LoadQueueId').patchValue(data.DsbLoadQueueSuccess[scheduleIndex].Id);
                    });
                }
            }
            else {
                Declarations.msgerror("Unable to collapse shift. Please try later.", undefined, undefined);
                //if api call fails, revert changes in client side
                _Schedules.controls.forEach((schedule: FormGroup) => {
                    schedule.get('IsLoadQueueCollapsed').patchValue(false);
                });
                this.loadQueueService.setLoadQueueColumnMoved(true);
                this.changeDetectorRef.markForCheck();
                this.dataService.setDriverColumnChangeDetect(true);
            }
        });
    }
    getLoadQueueNotifications() {
        let refreshScheduleBuilder = false;
        let dsbNotificationModel: DSBLoadQueueNotificationModel[] = [];
        let _shifts = this.SbForm.controls['Shifts'] as FormArray;
        _shifts.controls.forEach((shift: FormGroup, shiftIndex: number) => {
            let schedules = shift.controls['Schedules'] as FormArray;
            schedules.controls.forEach((schedule: FormGroup, scheduleIndex: number) => {
                if (schedule.get('IsLoadQueueCollapsed').value && (schedule.get('LoadQueueColumnStatus').value == DSBLoadQueueStatus.New || schedule.get('LoadQueueColumnStatus').value == DSBLoadQueueStatus.InProgress)) {
                    let dsbNotification = new DSBLoadQueueNotificationModel();
                    dsbNotification.Date = this.SbForm.get('Date').value;
                    dsbNotification.RegionId = this.SbForm.get('RegionId').value;
                    dsbNotification.ScheduleBuilderId = this.SbForm.get('Id').value;
                    dsbNotification.ShiftId = shift.get('Id').value;
                    dsbNotification.ShiftIndex = shiftIndex;
                    dsbNotification.DriverColIndex = scheduleIndex;
                    dsbNotificationModel.push(dsbNotification);
                }
            });
        });
        if (dsbNotificationModel.length > 0) {
            this.loadQueueService.getLoadQueueNotifications(dsbNotificationModel).subscribe(response => {
                if (response.length > 0) {
                    var publishColResponse = response.filter(x => x.Status == DSBLoadQueueStatus.Success);
                    if (publishColResponse.length > 0) {
                        publishColResponse.forEach(x => {
                            var shift = this.SbForm.controls['Shifts']['controls'][x.ShiftIndex] as FormGroup;
                            if (shift != null) {
                                let schedule = shift.controls['Schedules']['controls'][x.DriverColIndex] as FormGroup;
                                if (schedule != null) {
                                    var trips = schedule.controls['Trips'] as FormArray;
                                    for (var i = 0; i < trips.length; i++) {
                                        var tripValue = trips.value[i];
                                        this.setTripStatusToPublish(tripValue);
                                    }
                                    this.updateDsbLoadQueueForm(x.DSBSaveModel, trips);
                                    schedule.get('LoadQueueColumnStatus').patchValue(DSBLoadQueueStatus.Success);
                                    schedule.get('IsLoadQueueColumnBlocked').patchValue(false);
                                    this.changeDetectorRef.detectChanges();
                                }
                                else {
                                    refreshScheduleBuilder = true;
                                }
                            }
                        });
                    }
                    else {
                        var scheduleColResponse = response.filter(x => x.Status != DSBLoadQueueStatus.Success);
                        if (scheduleColResponse.length > 0) {
                            scheduleColResponse.forEach(x => {
                                var shift = this.SbForm.controls['Shifts']['controls'][x.ShiftIndex] as FormGroup;
                                if (shift != null) {
                                    let schedule = shift.controls['Schedules']['controls'][x.DriverColIndex] as FormGroup;
                                    if (schedule != null && schedule.get('LoadQueueColumnStatus').value != x.Status) {
                                        schedule.get('LoadQueueColumnStatus').patchValue(x.Status);
                                        if (x.Status == DSBLoadQueueStatus.Failed) {
                                            schedule.get('IsLoadQueueColumnBlocked').patchValue(false);
                                        }
                                    }
                                }
                            });
                            this.changeDetectorRef.detectChanges();
                        }
                    }
                }
            });
        }

        if (refreshScheduleBuilder) {
            Declarations.msgerror("Please refresh the schedule builder as load queue column(s) published successfully.", undefined, undefined);
        }
    }
    subscribeLoadQueueNotifications() {
        interval(15000) //every 15 seconds
            .pipe()
            .subscribe(() => {
                this.getLoadQueueNotifications();
            });

    }
    updateDsbLoadQueueForm(data: DSBSaveModel, trip: any, isDateChange: boolean = false): void {
        if (data == null) {
            Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
            return;
        }
        if (data.StatusCode == 0 || data.StatusCode == 2) {
            this.droppedTrip = null;
            this.draggedDelReqData = null;
            this.setUnsavedChanges();
            if (!isDateChange) {
                this.SbForm.patchValue({
                    Id: data.Id,
                    TimeStamp: data.TimeStamp,
                    Status: 0
                });
                if (data && data.Trips && data.Trips.length > 0) {
                    if (trip instanceof FormGroup) {
                        trip.patchValue(data.Trips[0]);
                        var sIndex = trip.controls['ShiftIndex'].value;
                        var rIndex = trip.controls['DriverRowIndex'].value;
                        this.setAllowDriverChange(sIndex, rIndex);
                        this.dataService.setSavedChangesSubject(trip.value);
                    }
                    else if (trip instanceof FormArray) {
                        for (var i = 0, j = 1; i < trip.length; i++, j++) {
                            let thisTrip = trip.controls[i] as FormGroup;
                            var shiftId = thisTrip.controls['ShiftId'].value;
                            var shiftIndex = thisTrip.controls['ShiftIndex'].value;
                            var rowIndex = thisTrip.controls['DriverRowIndex'].value;
                            var colIndex = thisTrip.controls['DriverColIndex'].value;
                            var savedTrip = data.Trips.find(t => t.ShiftId == shiftId && t.DriverRowIndex == rowIndex && t.DriverColIndex == colIndex);
                            if (savedTrip) {
                                trip.controls[i].patchValue(savedTrip);
                                this.setAllowDriverChange(shiftIndex, rowIndex);
                                this.dataService.setSavedChangesSubject(trip.controls[i].value);
                            }
                        }
                    } else {
                        for (var i = 0, j = 1; i < trip.length; i++, j++) {
                            let thisTrip = trip[i] as FormGroup;
                            var shiftId = thisTrip.controls['ShiftId'].value;
                            var shiftIndex = thisTrip.controls['ShiftIndex'].value;
                            var rowIndex = thisTrip.controls['DriverRowIndex'].value;
                            var colIndex = thisTrip.controls['DriverColIndex'].value;
                            var savedTrip = data.Trips.find(t => t.ShiftId == shiftId && t.DriverRowIndex == rowIndex && t.DriverColIndex == colIndex);
                            if (savedTrip) {
                                trip[i].patchValue(savedTrip);
                                this.setAllowDriverChange(shiftIndex, rowIndex);
                                this.dataService.setSavedChangesSubject(trip[i].value);
                            }
                        }
                    }
                }
            }
        }
        else {
            Declarations.msgerror(data.StatusMessage, undefined, undefined);
        }

        this._savingBuilder = false;
        this.changeDetectorRef.detectChanges();
        this.dataService.setScheduleChangeDetectSubject(true);
    }
    subscribeChangeDetectDsbLoadQueueNotification(): void {
        this.DsbLoadQueueNotification = this.dataService.DsbQueueChangesForNotification.subscribe(x => {
            if (x) {
                this.subscribeLoadQueueNotifications();
            }
        });
    }

    private subscribeDriverColumnChangeDetect(): void {
        this.DriverColCDRSubscription = this.dataService.DriverColumnChangeDetect.subscribe(x => {
            if (x) {
                this.changeDetectorRef.detectChanges();
            }
        });
    }

    initOptionalPickupForm(order: OrderPickupDetailModel): FormGroup {

        let isTerminalPickup = order && order.PickupLocationType != 2;
        let _pForm = this.fb.group({
            SelectedFuelTypeDetails: this.fb.control([], [Validators.required]),
            PickupLocationType: this.fb.control(order.PickupLocationType),
            Terminal: this.utilService.getTerminalForm(null, isTerminalPickup),
            BulkPlant: this.utilService.getBulkPlantForm(null, !isTerminalPickup),
            BadgeNo1: this.fb.control(''),
            BadgeNo2: this.fb.control(''),
            BadgeNo3: this.fb.control('')
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
    private subscribeOptionalPickupLocationChange(): void {
        this.DGSubscription.add(this.PickupForm.controls['PickupLocationType'].valueChanges.subscribe((data) => {
            this.PickupForm.markAllAsTouched;
            this.PickupForm.markAsDirty();
            this.setPickupValidators_(this.PickupForm, data);
        }));
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
    onTerminalSelected_(event: any): void {
        this.PickupForm.get('Terminal').patchValue({ Id: event.Id, Name: event.Name });
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
    getBulkPlantAddress_(bulkPlantName: string) {
        this.DGSubscription.add(this.addresService.GetBulkPlantDetails(bulkPlantName).subscribe(response => {
            this.PickupForm.controls['BulkPlant'].patchValue({
                Address: response.Address,
                City: response.City,
                State: response.State,
                Country: { Code: response.Country.Code, Id: response.Country.Id, Name: response.CountyName },
                ZipCode: response.ZipCode,
                CountyName: response.CountyName,
                Latitude: response.Latitude,
                Longitude: response.Longitude
            });
            this.changeDetectorRef.detectChanges();
        }));
        this.PickupForm.markAllAsTouched();
        this.PickupForm.markAsDirty();
    }
    private unsubscribeAllDGSubscriptions(): void {
        if (this.DGSubscription) {
            this.DGSubscription.unsubscribe();
        }
    }
    onFuelTypeSelect(item: any) {
        this.SelectedFuelTypeDetails = this.PickupForm.get('SelectedFuelTypeDetails').value;
        this.getTerimalDetails(this.OptionPickupOrderIds, this.SelectedFuelTypeDetails.map(x => x.Id));
    }
    onFuelTypeDeSelect(item: any) {
        this.SelectedFuelTypeDetails = this.PickupForm.get('SelectedFuelTypeDetails').value;
        this.getTerimalDetails(this.OptionPickupOrderIds, this.SelectedFuelTypeDetails.map(x => x.Id));
    }
    onFuelTypeSelectAll(items: any): void {
        this.SelectedFuelTypeDetails = items;
        this.getTerimalDetails(this.OptionPickupOrderIds, this.SelectedFuelTypeDetails.map(x => x.Id));
    }
    onFuelTypeDeSelectAll(): void {
        this.SelectedFuelTypeDetails = [];
        this.PickupForm.get('SelectedFuelTypeDetails').setValue([]);
        this.getTerimalDetails(this.OptionPickupOrderIds, []);
    }
    addOptionalPickupLocation() {
        if (this.PickupForm.valid) {
            let xNumber = this.OptionalPickupLocationInfo.length;
            this.SelectedFuelTypeDetails.forEach(x => {
                let isDuplicateRecord = false;
                if (this.PickupForm.get('PickupLocationType').value == 1) {
                    let TfxTerminal = this.PickupForm.get('Terminal').get('Name').value as DropdownItem;
                    let optionalPickupIndex = this.OptionalPickupLocationInfo.findIndex(x1 => x1.TfxFuelTypeId == x.Id && x1.DSBPickupLocationInfo.PickupLocationType == 1 && x1.DSBPickupLocationInfo.TfxTerminal.Id == TfxTerminal.Id);
                    if (optionalPickupIndex >= 0) {
                        isDuplicateRecord = true;
                    }
                }
                else {
                    let TfxBulkPlant = this.PickupForm.get('BulkPlant').value as DropAddressModel
                    let optionalPickupIndex = this.OptionalPickupLocationInfo.findIndex(x1 => x1.TfxFuelTypeId == x.Id && x1.DSBPickupLocationInfo.PickupLocationType == 2 && x1.DSBPickupLocationInfo.TfxBulkPlant.SiteName == TfxBulkPlant.SiteName);
                    if (optionalPickupIndex >= 0) {
                        isDuplicateRecord = true;
                    }
                }
                if (!isDuplicateRecord) {
                    xNumber = xNumber + 1;
                    let pickupModel = new OptionalPickupDetailModel();
                    pickupModel.incId = xNumber;
                    pickupModel.isAdded = 1;
                    pickupModel.Id = '';
                    pickupModel.RegionId = this.OptionalPickupRegionId;
                    pickupModel.ScheduleBuilderId = this.OptionalPickupScheduleBuilderId;
                    pickupModel.ShiftId = this.OptionalPickupShiftId;
                    pickupModel.ShiftIndex = this.OptionalPickupShiftIndex;
                    pickupModel.ShiftOrderNumber = this.OptionalPickupShiftOrderNo;
                    pickupModel.DriverColIndex = this.OptionalPickupDriverColIndex;
                    pickupModel.TfxFuelTypeId = x.Id;
                    pickupModel.TfxFuelTypeName = x.Name;
                    pickupModel.DSBPickupLocationInfo.PickupLocationType = this.PickupForm.get('PickupLocationType').value;
                    if (pickupModel.DSBPickupLocationInfo.PickupLocationType == 1) {
                        pickupModel.DSBPickupLocationInfo.TfxTerminal = this.PickupForm.get('Terminal').get('Name').value as DropdownItem;
                    }
                    else {
                        pickupModel.DSBPickupLocationInfo.TfxBulkPlant = this.PickupForm.get('BulkPlant').value as DropAddressModel;
                    }
                    pickupModel.DSBPickupLocationInfo.BadgeNo1 = this.PickupForm.get('BadgeNo1').value;
                    pickupModel.DSBPickupLocationInfo.BadgeNo2 = this.PickupForm.get('BadgeNo2').value;
                    pickupModel.DSBPickupLocationInfo.BadgeNo3 = this.PickupForm.get('BadgeNo3').value;
                    var BadgeNoInfo = [pickupModel.DSBPickupLocationInfo.BadgeNo1, pickupModel.DSBPickupLocationInfo.BadgeNo2, pickupModel.DSBPickupLocationInfo.BadgeNo3];
                    pickupModel.DSBPickupLocationInfo.BadgeNoInfo = BadgeNoInfo.filter(Boolean).join(',');
                    if (this.OptionPickupScheduleGroup.controls['Drivers']['controls'].length > 0) {
                        pickupModel.DriverId = this.OptionPickupScheduleGroup.controls['Drivers']["controls"][0]["controls"]["Id"].value;
                    }
                    this.OptionalPickupLocationInfo.push(pickupModel);
                }
            });
            this.changeDetectorRef.detectChanges();
            this.resetPickupForm();
        }
    }
    resetPickupForm() {
        this.PickupForm.get('SelectedFuelTypeDetails').setValue([]);
        this.PickupForm.get('PickupLocationType').setValue(1);
        this.PickupForm.get('Terminal').setValue({
            Id: 0,
            Name: ''
        });
        this.PickupForm.get('BulkPlant').setValue({
            Address: '',
            City: '',
            State: { Id: 0, Code: '' },
            Country: { Id: 0, Code: '' },
            CountryGroup: { Id: 0, Code: '' },
            ZipCode: '',
            CountyName: '',
            Latitude: '',
            Longitude: '',
            SiteName: '',
            SiteId: ''
        });
        this.PickupForm.get('BadgeNo1').setValue('');
        this.PickupForm.get('BadgeNo2').setValue('');
        this.PickupForm.get('BadgeNo3').setValue('');
    }
    getBulkPlanDetails() {
        if (!this.StateList || this.StateList.length == 0) {
            this.stateService.getStates().subscribe(x => this.StateList = x);
        }
        if (!this.CountryList || this.CountryList.length == 0) {
            this.stateService.getCountries().subscribe(x => this.CountryList = x);
        }
        if (!this.BulkPlants || this.BulkPlants.length == 0) {
            this.addresService.getBulkPlants('').subscribe(data => {
                this.BulkPlants = data.slice();
                this.BulkplantList = data;
            });
        }

    }
    setStateCode_(event: any) {
        this.PickupForm.get('BulkPlant.State.Code').setValue(event.target.selectedOptions[0].text);
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

        if (RegExConstants.UsaZip.test(zipCode) || RegExConstants.CanZip.test(zipCode)) {
            this._loadingAddress = true;
            this.DGSubscription.add(this.addresService.getAddress(zipCode)
                .subscribe(data => {
                    this._loadingAddress = false;
                    if (data != null && data != undefined && data.StateCode != null) {
                        data.CountryCode == 'US' || data.CountryCode == 'USA' ? data.CountryCode = 'USA' : data.CountryCode = 'CAN';
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
                    this.changeDetectorRef.detectChanges();
                }));
        }
    }
    getTerimalDetails(orderIds: number[], fuelTypeIds: number[]) {
        this._loadingOptionTerminal = true;
        this.DGSubscription.add(this.sbService.getOptioanlPickupTerminals(orderIds, fuelTypeIds, '').subscribe((data: DropdownItem[]) => {
            this._loadingOptionTerminal = false;
            this.Terminals = data;
            this.changeDetectorRef.detectChanges();
        }));
    }
    subscribeOptionPickupEvents(): void {
        this.OptionalPickupSubscription = this.dataService.OptionalPickupSubject.subscribe(x => {
            if (x) {
                this.OptionPickupScheduleGroup = x.scheduleInfo;
                this.OptionalPickupShiftIndex = x.shiftIndex;
                this.OptionalPickupDriverColIndex = x.ColIndex;
                this.OptionalPickupShiftId = x.ShiftId;
                this.OptionalPickupRegionId = x.RegionId;
                this.OptionalPickupScheduleBuilderId = x.ScheduleBuilderId;
                this.OptionalPickupShiftOrderNo = x.shiftOrderNo;
                this.OptionalPickupLocationInfo = [];
                this.OrderFuelDetails = [];
                this.SelectedFuelTypeDetails = [];
                this.OptionPickupOrderIds = [];
                this.FuelTypeDetails = [];
                this.OptionalPickupLocationDeleteInfo = null;
                this.PickupForm.get('SelectedFuelTypeDetails').setValue([]);
                this.PickupForm.get('PickupLocationType').setValue(1);
                this.PickupForm.get('Terminal').setValue({ Id: 0, Name: "" });
                var BulkPlantForm = {
                    Address: '',
                    City: '',
                    State: { Id: 0, Code: '' },
                    Country: { Id: 0, Code: '' },
                    CountryGroup: { Id: 0, Code: '' },
                    ZipCode: '',
                    CountyName: '',
                    Latitude: '',
                    Longitude: '',
                    SiteName: '',
                    SiteId: ''
                };
                this.PickupForm.get('BulkPlant').setValue(BulkPlantForm);
                this.PickupForm.get('BadgeNo1').setValue('');
                this.PickupForm.get('BadgeNo2').setValue('');
                this.PickupForm.get('BadgeNo3').setValue('');
                if (x.FuelTypeInfo != null) {
                    var orderInfo = x.FuelTypeInfo as OrderFuelInfo;
                    if (orderInfo.StatusCode == 0) {
                        this.OrderFuelDetails = orderInfo.OrderFuelDetails;
                        orderInfo.OrderFuelDetails.forEach(x => {
                            this.OptionPickupOrderIds.push(x.OrderId);
                            x.FuelTypeDetails.forEach(xItem => {
                                var fuelIndex = this.FuelTypeDetails.findIndex(x => x.Id == xItem.Id);
                                if (fuelIndex == -1) {
                                    var fuelDetails = new DropdownItem();
                                    fuelDetails.Id = xItem.Id;
                                    fuelDetails.Name = xItem.Name;
                                    this.FuelTypeDetails.push(fuelDetails);
                                }
                            });
                        });
                        this.getBulkPlanDetails();
                        this.getOptionalPickupInfo(x, x.scheduleInfo);
                    }
                    this._loadingOptionTerminal = false;
                    this.changeDetectorRef.detectChanges();
                }
                else {
                    this._loadingOptionTerminal = false;
                    this.changeDetectorRef.detectChanges();
                }
            }
        });
    }
    submitOptionalPickupLocation() {
        if (this.OptionalPickupLocationInfo.length > 0) {
            this._loadingOptionTerminal = true;
            this.sbService.addOptionalPickup(this.OptionalPickupLocationInfo).subscribe((data: any) => {
                this._loadingOptionTerminal = false;
                if (data.StatusCode == 0) {
                    $("#optional-pickup-location").click();
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                }
                else {
                    Declarations.msgerror(data.StatusMessage, undefined, undefined);
                }
            });
            this.changeDetectorRef.detectChanges();
        }
    }
    //carrier assignmnet
    isDrExistInShift(_schedules: FormArray) {
        let schedules = _schedules.value as any[] || [];
        return schedules.some(schedule => schedule.Trips.some(trip => trip.DeliveryRequests.length > 0));
    }
    isAllShiftDrSelected(_schedules: FormArray) {
        let schedules = _schedules.value as any[] || [];
        return schedules.every(schedule => schedule.Trips.every(trip => trip.DeliveryRequests.every(dr => dr.IsSelectedForAssignment == true)));
    }
    shiftSelectedForCarrierAssignment(event, _schedules: FormArray) {
        _schedules.controls.forEach((schedule: FormControl) => {
            let trips = schedule['controls']['Trips'] as FormArray;
            trips.controls.forEach((trip: FormControl) => {
                let drs = trip['controls']['DeliveryRequests'] as FormArray;
                drs.controls.forEach((drForm: FormControl) => {
                    if (drForm.get('IsSelectedForAssignment').value != event.target.checked) {
                        drForm.get('IsSelectedForAssignment').setValue(event.target.checked);
                    }
                })
            })
        })
        this.dataService.setScheduleChangeDetectSubject(true);
    }
    getOptionalPickupInfo(x: any, scheduleInfo: any) {
        this._loadingOptionTerminal = true;
        this.OptionalPickupLocationInfo = [];
        var optionalPickupModel = new OptionalPickupDetailModel();
        optionalPickupModel.ScheduleBuilderId = x.ScheduleBuilderId;
        optionalPickupModel.ShiftId = x.ShiftId;
        optionalPickupModel.DriverColIndex = x.ColIndex;
        this.sbService.getOptionalPickup(optionalPickupModel).subscribe(response => {
            this._loadingOptionTerminal = false;
            if (response != null) {
                response.forEach(x1 => {
                    var BadgeNoInfo = [x1.DSBPickupLocationInfo.BadgeNo1, x1.DSBPickupLocationInfo.BadgeNo2, x1.DSBPickupLocationInfo.BadgeNo3];
                    x1.DSBPickupLocationInfo.BadgeNoInfo = BadgeNoInfo.filter(Boolean).join(',');
                    x1.ShiftOrderNumber = x.shiftOrderNo;
                    if (scheduleInfo.controls['Drivers']['controls'].length > 0) {
                        x1.DriverId = scheduleInfo.controls['Drivers']["controls"][0]["controls"]["Id"].value;
                    }
                    this.OptionalPickupLocationInfo.push(x1);
                });
            }
            this.changeDetectorRef.detectChanges();
        });

    }
    deleteOptionPickup(optionalPickup: OptionalPickupDetailModel) {
        let enrouteInProgress = [3, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];
        let IsDSCompleted = false;
        let IsDSProgress = false;
        if (optionalPickup != null) {
            if (optionalPickup.Id != '') {
                this._loadingOptionTerminal = true;
                let OrderIds = [];
                this.OrderFuelDetails.forEach(x => {
                    if (x.FuelTypeDetails.findIndex(x => x.Id == optionalPickup.TfxFuelTypeId) >= 0) {
                        if (OrderIds.findIndex(x1 => x1 == x.OrderId) == -1) {
                            OrderIds.push(x.OrderId);
                        }
                    }
                });
                if (OrderIds.length > 0) {
                    let trips = this.OptionPickupScheduleGroup['controls']['Trips'] as FormArray;
                    trips.controls.forEach((trip: FormControl) => {
                        let drs = trip['controls']['DeliveryRequests'] as FormArray;
                        if (drs.controls.length > 0) {
                            drs.controls.forEach((drForm: FormControl) => {
                                if (!IsDSCompleted && !IsDSProgress) {
                                    var OrderId = drForm.get('OrderId').value;
                                    var StatusClassId = drForm.get('StatusClassId').value;
                                    if (StatusClassId == 4 || StatusClassId == 5)
                                        if (OrderIds.findIndex(x => x == OrderId) >= 0) {
                                            IsDSCompleted = true;
                                            return false;
                                        }
                                    if (enrouteInProgress.findIndex(x => x == StatusClassId) >= 0) {
                                        this.OptionalPickupLocationDeleteInfo = optionalPickup;
                                        IsDSProgress = true;
                                        return false;
                                    }
                                }
                            })
                        }
                    });
                }
                if (!IsDSCompleted && !IsDSProgress) {
                    var findIndex = this.OptionalPickupLocationInfo.findIndex(x => x.incId == optionalPickup.incId);
                    if (findIndex >= 0) {
                        this.OptionalPickupLocationInfo.splice(findIndex, 1);
                    }
                    this.sbService.deleteOptionalPickup(optionalPickup.Id, optionalPickup.DriverId).subscribe(x => {
                        this._loadingOptionTerminal = false;
                        if (x != null && x.StatusCode == 0) {
                            Declarations.msgsuccess(x.StatusMessage, undefined, undefined);
                        }
                        else {
                            Declarations.msgerror(x.StatusMessage, undefined, undefined);
                        }
                        this.changeDetectorRef.detectChanges();
                    });
                }
                else {
                    this._loadingOptionTerminal = false;
                    if (IsDSProgress) {
                        $("#btn-inprogress-onthewayoptionpickup").click();
                    }
                    else {
                        $("#btn-completed-optionpickup").click();
                    }
                    this.changeDetectorRef.detectChanges();
                }
            }
            else {
                var findIndex = this.OptionalPickupLocationInfo.findIndex(x => x.incId == optionalPickup.incId);
                if (findIndex >= 0) {
                    this.OptionalPickupLocationInfo.splice(findIndex, 1);
                }
                this.changeDetectorRef.detectChanges();
            }

        }
    }
    deleteOptionalPickupNo() {
        this.OptionalPickupLocationDeleteInfo = null;
    }
    deleteOptionalPickupYes() {
        if (this.OptionalPickupLocationDeleteInfo != null) {
            var findIndex = this.OptionalPickupLocationInfo.findIndex(x => x.incId == this.OptionalPickupLocationDeleteInfo.incId);
            if (findIndex >= 0) {
                this.OptionalPickupLocationInfo.splice(findIndex, 1);
            }
            this.sbService.deleteOptionalPickup(this.OptionalPickupLocationDeleteInfo.Id, this.OptionalPickupLocationDeleteInfo.DriverId).subscribe(x => {
                if (x != null && x.StatusCode == 0) {
                    Declarations.msgsuccess(x.StatusMessage, undefined, undefined);
                }
                else {
                    Declarations.msgerror(x.StatusMessage, undefined, undefined);
                }
                $("#btn-completed-onthewayoptionpickup").click();
                this.changeDetectorRef.detectChanges();
            });
        }
    }
    setDRPickupValidators(form: FormGroup): void {
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
    private subscribeShowScheduleBuilderLoadingSubject(): void {
        let subs = this.dataService.IsScheduleBuilderSubject.subscribe(x => {
            if (x != null && x != undefined) {
                if (x) {
                    this._savingBuilder = true;
                }
                else {
                    this._savingBuilder = false;
                }
                this.changeDetectorRef.detectChanges();
            }

        });
        this.DGSubscription.add(subs);
    }

    get getTrailerByIdFrolList() {
        return this.UnassignedTrailers.reduce((prev, curr) => {
            prev[curr.Id] = curr;
            return prev;
        }, {});
    }
    hoveredTrailer: TrailerViewModel = new TrailerViewModel();
    bindRetainInfo(id: string) {
        this.hoveredTrailer = this.getTrailerByIdFrolList[id];
    }
    subscribeCancelScheduleEvents(): void {
        this.CancelEntireRowSubscription = this.dataService.CancelEntireRowSubject.subscribe(x => {
            if (x) {
                this.CancelDSScheduleLoadInfo = null;
                let cancelDSinfo = { ShiftIndex: x.ShiftIndex, ScheduleIndex: x.ScheduleIndex };
                this.cancelDSScheduleInfo = cancelDSinfo;
                $("#btn-cancel-ds").click();

            }
        });
    }
    unsubscribeCancelScheduleEvents(): void {
        if (this.CancelEntireRowSubscription) {
            this.CancelEntireRowSubscription.unsubscribe();
        }
    }
    cancelRowPublish(shiftIndex: number, rowIndex: number): void {
        let schedule = this.SbForm.get('Shifts.' + shiftIndex + '.Schedules.' + rowIndex) as FormGroup;
        var trips = schedule.controls['Trips'] as FormArray;
        this.cancelEntireRow(schedule, shiftIndex, rowIndex, trips);
    }
    cancelEntireRow(schedule: FormGroup, shiftIndex: number, rowIndex: number, trips: FormArray): void {
        var validTrips = [];
        for (var i = 0, j = 1; i < trips.length; i++, j++) {
            let thisTrip = trips.controls[i] as FormGroup;
            if (thisTrip && (thisTrip.valid || this.validatePublishLoad(thisTrip))) {
                if (thisTrip.get('DeliveryRequests').value.length > 0) {
                    validTrips.push(i);
                }
            }
        }
        this._savingBuilder = true;
        this.changeDetectorRef.detectChanges();
        var dsbModel = this.getDSBSaveModel();
        var drivers = schedule.controls['Drivers'].value;
        var trailers = schedule.controls['Trailers'].value;
        for (var i = 0; i < trips.length; i++) {
            var tripValue = trips.value[i];
            if (validTrips.includes(tripValue.DriverColIndex)) {
                this.setTripStatusToCancel(tripValue);
            }
            dsbModel.Trips.push(tripValue);
        }
        dsbModel.Trips.forEach(t => { t.Drivers = drivers, t.Trailers = trailers, t.IsIncludeAllRegionDriver = schedule.get('IsIncludeAllRegionDriver').value });
        dsbModel.Status = 3;
        if (dsbModel.Trips.length > 0) {
            this.sbService.CancelDriverSchedule(dsbModel).subscribe(data => {
                this._savingBuilder = false;
                this.updateLoadForm(data, trips);
            });
        }
        else {
            this._savingBuilder = false;
        }
    }
    setTripStatusToCancel(trip: any) {
        this.setTripStatus(trip);
        this.setDeliveryGroupStatus(trip, DeliveryGroupStatus.Published);
        this.setDeliveryRequestStatus(trip, DeliveryReqStatus.ScheduleCreated);
    }
    public cancelDriverSchedule() {
        if (Object.keys(this.cancelDSScheduleInfo).length > 0) {
            this.cancelRowPublish(this.cancelDSScheduleInfo.ShiftIndex, this.cancelDSScheduleInfo.ScheduleIndex);
        }
        else if (Object.keys(this.CancelDSScheduleLoadInfo).length > 0) {
            this.cancelDSScheduleBuilder(this.CancelDSScheduleLoadInfo.shiftIndex, this.CancelDSScheduleLoadInfo.rowIndex, this.CancelDSScheduleLoadInfo.colIndex, this.CancelDSScheduleLoadInfo.schedule, this.CancelDSScheduleLoadInfo.trip);
        }
    }
    public rejectDriverSchedule() {
        this.cancelDSScheduleInfo == null;
        this.CancelDSScheduleLoadInfo = null;
    }
    subscribeCancelDSDeliveryGroupSubject(): void {
        this.CancelDSDeliveryGroupSubscription = this.dataService.CancelDSDeliveryGroupSubject.subscribe(x => {
            if (x) {
                this.cancelDSScheduleInfo == null;
                $("#btn-cancel-ds").click();
                this.CancelDSScheduleLoadInfo = { shiftIndex: x.shiftIndex, rowIndex: x.rowIndex, colIndex: x.colIndex, schedule: x.schedule, trip: x.trip };
            }
        });
    }
    cancelDSScheduleBuilder(i: number, j: number, k: number, schedule: any, trip: FormGroup): void {
        if (trip != null && trip != undefined) {
            if (trip.valid || this.validatePublishLoad(trip)) {
                this.dataService.setShowDeliveryGroupSubject(false);
                this.cancelDSLoadSave(i, j, k, schedule, trip);
            }
        }
    }
    cancelDSLoadSave(i: number, j: number, k: number, schedule: any, trip: FormGroup) {
        this._savingBuilder = true;
        this.changeDetectorRef.detectChanges();
        var dsbModel = this.getDSBSaveModel();
        var tripValue = trip.value;
        this.setTripStatus(tripValue);
        this.setDeliveryGroupStatus(tripValue, DeliveryGroupStatus.Published);
        this.setDeliveryRequestStatus(tripValue, DeliveryReqStatus.ScheduleCreated);
        tripValue.Drivers = schedule.get('Drivers').value;
        tripValue.Trailers = schedule.get('Trailers').value;
        tripValue.IsIncludeAllRegionDriver = schedule.get('IsIncludeAllRegionDriver').value;
        dsbModel.Trips.push(tripValue);
        dsbModel.Status = 3;
        this.sbService.CancelDriverSchedule(dsbModel).subscribe(data => {
            this._savingBuilder = false;
            this.updateLoadForm(data, trip);
        });
    }
    subscribeCancelDeliveryGroupNormalGroupDSSubject(): void {
        this.CancelDSDeliveryGroupNormalGroupDSSubject = this.dataService.CancelDSDeliveryGroupNormalGroupDSSubject.subscribe(x1 => {
            if (x1) {
                this.CancelDSInfo(x1);
            }
        });
    }
    subscribeCancelDSLocationGroupSubject(): void {
        this.CancelDSLocationGroupSubscription = this.dataService.CancelDSLocationSubject.subscribe(x => {
            if (x) {
                this.GetCancelDS(x.jobId, x.trip, x.isTBD, x.TBDGroupId);
            }
        });
    }
    subscribeDispatcherLoadDragDropSubscription(): void {
        this.DispatcherLoadDragDropSubscription = this.dataService.DispatcherLoadDragDropSubject.subscribe(x => {
            if (x) {

                let trip = this.SbForm.get('Shifts.' + x.trip.value.ShiftIndex + '.Schedules.' + x.trip.value.DriverRowIndex + '.Trips.' + x.trip.value.DriverColIndex) as FormGroup;
                if (trip) {
                    trip.controls['IsDispatcherDragDropSequence'].setValue(true);
                    trip.controls['IsDispatcherDragDropSequenceModified'].setValue(true);
                    this._savingBuilder = true;
                    let isCommonPickup = trip.controls['IsCommonPickup'].value;
                    let _drArray = trip.controls['DeliveryRequests'] as FormArray;
                    _drArray.clear();
                    let DRsDetails = x.DRs as DeliveryRequestViewModel[];
                    var _drFormArray = this.utilService.getDispatcherDeliveryRequestFormArray(DRsDetails, isCommonPickup, 0);
                    _drFormArray.controls.forEach((_drForm: FormGroup) => {
                        _drArray.push(_drForm);
                    });
                    this._savingBuilder = false;
                    this.changeDetectorRef.markForCheck();
                    this.dataService.setScheduleChangeDetectSubject(true);
                }
            }
        });
    }
    subscribeDispatcherLoadDragDropMapSubscription(): void {
        this.DispatcherLoadDragDropMapSubscription = this.dataService.DispatcherLoadDragDropMapSubject.subscribe(x => {
            if (x) {

                let trip = this.SbForm.get('Shifts.' + x.trip.value.ShiftIndex + '.Schedules.' + x.trip.value.DriverRowIndex + '.Trips.' + x.trip.value.DriverColIndex) as FormGroup;
                if (trip) {
                    this._savingBuilder = true;
                    let sortedDRs = [];
                    let LocationSeqInfo = x.JobIds as DispatcherMapSeq[];
                    trip.controls['IsDispatcherDragDropSequence'].setValue(true);
                    trip.controls['IsDispatcherDragDropSequenceModified'].setValue(true);
                    let deliveryRequests = trip.controls['DeliveryRequests'].value as DeliveryRequestViewModel[];
                    if (LocationSeqInfo.length > 0) {
                        LocationSeqInfo.forEach(x1 => {
                            if (x1.IsTBD == false) {
                                var drDetails = deliveryRequests.filter(x => x.JobId == x1.JobId);
                                drDetails.forEach(x2 => {
                                    sortedDRs.push(x2);
                                });
                            }
                            else {
                                var drDetails = deliveryRequests.filter(x => x.TBDGroupId == x1.TBDGroupId);
                                drDetails.forEach(x2 => {
                                    sortedDRs.push(x2);
                                });
                            }
                        });
                    }
                    let isCommonPickup = trip.controls['IsCommonPickup'].value;
                    let _drArray = trip.controls['DeliveryRequests'] as FormArray;
                    _drArray.clear();
                    var _drFormArray = this.utilService.getDispatcherDeliveryRequestFormArray(sortedDRs, isCommonPickup, 0);
                    _drFormArray.controls.forEach((_drForm: FormGroup) => {
                        _drArray.push(_drForm);
                    });
                    this.changeDetectorRef.markForCheck();
                    if ((trip.controls['TripId'] != null || trip.controls['DeliveryRequests'].value.length > 0) && trip.controls['DeliveryGroupPrevStatus'].value != 2) {
                        this.draftScheduleBuilder(trip);
                    }
                    else {
                        var schedule = this.SbForm.get('Shifts.' + x.trip.value.ShiftIndex + '.Schedules.' + x.trip.value.DriverRowIndex) as FormGroup;
                        if (schedule) {
                            this.publishScheduleBuilder(x.trip.value.ShiftIndex, x.trip.value.DriverRowIndex, x.trip.value.DriverColIndex, schedule, trip);
                        }
                    }
                }
            }
        });
    }
    unsubscribeCancelLocationScheduleEvents(): void {
        if (this.CancelDSLocationGroupSubscription) {
            this.CancelDSLocationGroupSubscription.unsubscribe();
        }
    }
    GetCancelDS(jobId: number, trip: FormGroup, isTBD: number, TBDGroupId: string) {
        this.cancelDSDeliveryScheduleViewModel = [];
        var isGroupDRs = false;
        let drs = trip.controls['DeliveryRequests'].value;
        let filteredDRs = [];
        if (isTBD == 0) {
            filteredDRs = drs.filter(x => x.JobId == jobId && x.TrackableScheduleId > 0) as DeliveryRequestViewModel[];
            if (filteredDRs.filter(x => x.GroupParentDRId != '' && x.GroupParentDRId != null).length > 0) {
                isGroupDRs = true;
            }
        }
        else {
            filteredDRs = drs.filter(x => x.TBDGroupId == TBDGroupId && x.TrackableScheduleId > 0) as DeliveryRequestViewModel[];
            if (filteredDRs.filter(x => x.GroupParentDRId != '' && x.GroupParentDRId != null).length > 0) {
                isGroupDRs = true;
            }
        }
        if (!isGroupDRs) {
            if (filteredDRs.length > 0) {
                //intialize the model.
                this._savingBuilder = true;
                let cancelDSDeliveryScheduleInfo = new CancelDSDeliveryScheduleInfo();
                cancelDSDeliveryScheduleInfo.RegionId = this.SelectedRegionId;
                cancelDSDeliveryScheduleInfo.CancelDSDeliverySchedules = [];
                filteredDRs.forEach(x1 => {
                    //normal DR
                    let cancelDSDeliverySchedule = new CancelDSDeliverySchedule();
                    cancelDSDeliverySchedule.DeliveryReqId = x1.Id;
                    cancelDSDeliverySchedule.IsSubDR = false;
                    if (x1.PostLoadInfo != null) {
                        cancelDSDeliverySchedule.IsPreLoadDR = true;
                    }
                    cancelDSDeliveryScheduleInfo.CancelDSDeliverySchedules.push(cancelDSDeliverySchedule);
                    //post load info
                    if (x1.PostLoadInfo != null) {
                        let cancelDSPostLoadDeliverySchedule = new CancelDSDeliverySchedule();
                        cancelDSPostLoadDeliverySchedule.DeliveryReqId = x1.PostLoadInfo.DrId;
                        cancelDSPostLoadDeliverySchedule.IsSubDR = false;
                        cancelDSDeliveryScheduleInfo.CancelDSDeliverySchedules.push(cancelDSPostLoadDeliverySchedule);
                    }
                });
                this.sbService.GetSubDRInfoCancelDS(cancelDSDeliveryScheduleInfo).subscribe(response => {
                    if (response != null && response != undefined && response.length > 0) {
                        this.cancelDSDeliveryScheduleViewModel = response as CancelDSDeliveryScheduleViewModel[];
                        this.cancelDSDeliveryScheduleViewModel.forEach(x1 => {
                            x1.IsChecked = true;
                        });
                        $("#btn-cancel-location-ds").click();
                    }
                    this._savingBuilder = false;
                    this.changeDetectorRef.markForCheck();
                    this.dataService.setGroupChangeDetectSubject(true);
                });

            }
        }
        else {
            if (filteredDRs.length > 0) {
                this._savingBuilder = true;
                let cancelDSDeliveryScheduleInfo = new CancelDSDeliveryScheduleInfo();
                cancelDSDeliveryScheduleInfo.RegionId = this.SelectedRegionId;
                cancelDSDeliveryScheduleInfo.CancelDSDeliverySchedules = [];
                filteredDRs.forEach(x => {
                    let cancelDSDeliverySchedule = new CancelDSDeliverySchedule();
                    cancelDSDeliverySchedule.DeliveryReqId = x.Id;
                    cancelDSDeliverySchedule.IsSubDR = false;
                    cancelDSDeliveryScheduleInfo.CancelDSDeliverySchedules.push(cancelDSDeliverySchedule);
                    if (x.GroupParentDRId != '' && cancelDSDeliveryScheduleInfo.CancelDSDeliverySchedules.findIndex(x1 => x1.DeliveryReqId == x.GroupParentDRId) == -1) {
                        let cancelDSDeliverySchedule = new CancelDSDeliverySchedule();
                        cancelDSDeliverySchedule.DeliveryReqId = x.GroupParentDRId;
                        cancelDSDeliverySchedule.IsSubDR = true;
                        cancelDSDeliveryScheduleInfo.CancelDSDeliverySchedules.push(cancelDSDeliverySchedule);
                    }
                });
                this.sbService.GetSubDRInfoCancelDS(cancelDSDeliveryScheduleInfo).subscribe(response => {
                    if (response != null && response != undefined && response.length > 0) {
                        this.cancelDSDeliveryScheduleViewModel = response as CancelDSDeliveryScheduleViewModel[];
                        $("#btn-location-cancel-ds-info").click();
                    }
                    this._savingBuilder = false;
                    this.changeDetectorRef.markForCheck();
                    this.dataService.setGroupChangeDetectSubject(true);
                });
            }
        }
    }
    cancelLocationDriverSchedule() {
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
                $("#cancelLocationDS").click();
                this._savingBuilder = true;
                this.sbService.CancelDeliveryGroupSchedule(this.DeliveryReqCancelScheduleUpdateModel).subscribe(response => {
                    this._savingBuilder = false;
                    this.changeDetectorRef.markForCheck();
                    this.dataService.setGroupChangeDetectSubject(true);
                    if (response != null && response != undefined && response.length > 0) {
                        this.CancelDSInfo(response);
                        Declarations.msgsuccess("Delivery schedule cancelled successfully.", 'Success', 2500);
                    }
                });
            }
        }
    }
    CancelDSInfo(x1: any) {
        var cancelDsData = x1 as CancelDeliverySchedule[];
        cancelDsData.forEach(x => {
            var scheduleBuilderId = this.SbForm.controls["Id"].value;
            if (scheduleBuilderId == x.ScheduleBuilderId) {
                let trip = this.SbForm.get('Shifts.' + x.ShiftIndex + '.Schedules.' + x.DriverRowIndex + '.Trips.' + x.DriverColIndex) as FormGroup;
                if (trip != null) {
                    var deliveryRequests = trip.controls["DeliveryRequests"] as FormArray;
                    if (deliveryRequests) {
                        for (var j = 0; j < deliveryRequests.length; j++) {
                            var deliveryRequest = deliveryRequests.controls[j] as FormGroup;
                            if (x.DeliveryReqId == deliveryRequest.controls["Id"].value) {
                                deliveryRequest.controls["ScheduleStatus"].patchValue(x.ScheduleStatus);
                                deliveryRequest.controls["TrackScheduleStatus"].patchValue(x.TrackScheduleStatus);
                                deliveryRequest.controls["TrackScheduleStatusName"].patchValue(x.TrackScheduleStatusName);
                                deliveryRequest.controls["StatusClassId"].patchValue(x.StatusClassId);
                            }
                        }
                    }
                }
            }
        });
        this.changeDetectorRef.detectChanges();
        this.dataService.setScheduleChangeDetectSubject(true);
    }
    public checkAllLocationDS() {
        if ($("#selectAllLocationDSBGroupCancel").is(":checked")) {
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
    public checkLocationDSChange(ele: number, ds: CancelDSDeliveryScheduleViewModel) {
        var eleId = "dsLocationCheck" + ele;
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
    CancelSelectedLocationDS() {
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
                $("#confirm-location-cancel-ds-dismiss").click();
                this._savingBuilder = true;
                this.sbService.CancelDeliveryGroupSchedule(this.DeliveryReqCancelScheduleUpdateModel).subscribe(response => {
                    this._savingBuilder = false;
                    this.changeDetectorRef.markForCheck();
                    this.dataService.setGroupChangeDetectSubject(true);
                    if (response != null && response != undefined && response.length > 0) {
                        this.CancelDSInfo(response);
                        Declarations.msgsuccess("Delivery schedule cancelled successfully.", 'Success', 2500);
                    }
                });
            }
        }
        else {
            Declarations.msgerror("Select at least one DS for cancel schedule.", 'Success', 2500);
        }
    }
    deleteSingleLoad(dsbModel: ResetDeliveryGroupScheduleModel) {
        dsbModel.ScheduleBuilderId = this.SbForm.get('Id').value;
        this._savingBuilder = true;
        this.sbService.deleteDeliverySchedule(dsbModel).subscribe(response => {
            if (response != null) {
                this._savingBuilder = false;
            }
            else {
                Declarations.msgerror("One of the services did not respond. Please contact the administrator.", undefined, undefined);
            }
            this._savingBuilder = false;
            this.changeDetectorRef.markForCheck();
            this.dataService.setScheduleChangeDetectSubject(true);
        });
    }
    private sorDrByProductSequence(IsDispatcherDragDropSequence: boolean, drList: DeliveryRequestViewModel[]): DeliveryRequestViewModel[] {
        drList = sortArrayTwice(drList, 'JobId', 'ProductSequence');
        if (IsDispatcherDragDropSequence) {
            drList = sortByKeyAsc(drList, 'DispatcherDragDropSequence');
        }
        return drList;
    }
    private logDRMaxFillIssue(trip: any) {
        //log max fill validation issue.
        var delRequests: DeliveryRequestViewModel[] = [];
        var _thisDrArray = trip.get('DeliveryRequests') as FormArray;
        _thisDrArray.controls.forEach((delReq: FormGroup) => {
            if (delReq.invalid) {
                var invalidctrls = CustomAbstractControls.findErrors(delReq);
                if (invalidctrls.indexOf("RequiredQuantity") > -1) {
                    //call api for debug log.
                    delRequests.push(delReq.value);

                }
            }
        });
        if (delRequests.length > 0) {
            this.sbService.postValidateDRMaxFill(delRequests).subscribe(response => {
            });
        }
    }
}

