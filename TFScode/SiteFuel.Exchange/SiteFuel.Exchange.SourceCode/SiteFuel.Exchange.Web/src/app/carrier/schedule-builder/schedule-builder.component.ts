import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, Validators, FormControl } from '@angular/forms';
import { CarrierService } from '../service/carrier.service';
import { ScheduleBuilderService } from '../service/schedule-builder.service';
import { DropdownItemExt, DropdownItem } from 'src/app/statelist.service';
import { DeliveryRequestViewModel, SbDriverViewModel, RegionDetailModel, ScheduleShiftModel, TrailerViewModel, SbFilterModel, TrailerViewFilterModel, DropAddressModel, TripModel, DriverViewFilterModel, ModifiedTripInfo, DSBSaveModel, DeliveryRequestBrokerInfoViewModel, ShiftViewModel, DelRequestsByJobModel, OrderPartialDetailModel, BrokerDrModel, JobDetailsWithOrders, CustomerJobsForCarrier, RegionDSBModel, SpiltDRsModel, JobLocationDetailsModal } from '../models/DispatchSchedulerModels';
import { Declarations } from 'src/app/declarations.module';
import { Subject, Subscription } from 'rxjs';
import { DeliveryGroupComponent } from '../delivery-group/delivery-group.component';
import { DataService } from 'src/app/services/data.service';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { SendbirdComponent } from 'src/app/shared-components/sendbird//sendbird.component';
import { chatService } from 'src/app/shared-components/sendbird/services/sendbird.service';
import { MemberInfo } from 'src/app/shared-components/sendbird/sendbirdCommon';
import { DeliveryRequestService } from 'src/app/delivery-request-display/services/DeliveryRequestService';
import { RouteInfoComponent } from './route-info/route-info.component';
import { RouteInfoService } from '../service/route-info.service';
import { RouteInformationModel, RouteTfxJobsList } from '../models/location';
import { MyLocalStorage } from 'src/app/my.localstorage';
import { DispatcherService } from '../service/dispatcher.service';
import { SplitDeliveryRequestComponent } from '../delivery-request/split-delivery-request/split-delivery-request.component';
import { DeliveryrequestService } from '../service/deliveryrequest.service';
import { DrFilterModel, DrFilterService } from './dr-filter.utility';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { DropDownItem, SalesTankFilterModal, TfxModule } from 'src/app/buyer-wally-board/Models/BuyerWallyBoard';
import * as moment from 'moment';
import { CarrierRegionModel, TfxCarrierDropdownDisplayItem, TfxCarrierRegionDetailsModel } from 'src/app/company-addresses/region/model/region';
import { RegionService } from 'src/app/company-addresses/region/service/region.service';
import { UtilService } from 'src/app/services/util.service';
import { sortArrayTwice, sortBy } from 'src/app/my.functions';
import { BrokeredDrCarrierStatus, DateFilter, DeliveryReqPriority, DeliveryReqStatus, DeliveryRequestTypes, DrFilterPreferencesModel, ObjectFilter, OrderType, PickupLocationType, QueueFilter, WindowModeFilter } from 'src/app/app.enum';
declare function closeSlidePanel(): any;
declare var currentUserCompanyId: number;
declare var IsCarrierCompany: boolean;
declare var currentUserCompanyType: number;
declare var mapsApiKey: string;
declare var isSupplierCompany: boolean;
declare var isSupplierOrDispatcher: boolean;
declare function IsUserActive(): boolean;

@Component({
    selector: 'app-schedule-builder',
    templateUrl: './schedule-builder.component.html',
    styleUrls: ['./schedule-builder.component.scss']
})
export class ScheduleBuilderComponent implements OnInit {
    public MinDate: Date = new Date();
    public MaxDate: Date = new Date();
    public _toggle_dr_panel: boolean = false;
    public _toggle_search: boolean = false;
    public _toggle_header: boolean = true;
    public _loading_dr_panel: boolean = false;
    public _isTrailerExists: boolean = false;
    public drRequestClick: boolean = true;
    public intervalTime: any;
    public intervalTimeQueue: any;
    public _mapsApiKey: string = mapsApiKey ? mapsApiKey : '';
    public _refreshQueue: boolean = false;
    public _mustGoRefresh: number = 0;
    public _shouldGoRefresh: number = 0;
    public _couldGoRefresh: number = 0;
    public _missedRefresh: number = 0;
    public _loadingRegions = false;
    public _loadingRequests = false;
    public _loadingDrRequests = false;
    public _loadingBuilder = false;
    public _loadingCarriers = false;
    public _acceptRejectDr = false;
    public _loadingRejectDR = false;
    public _unsavedChanges: ModifiedTripInfo[] = [];
    public _pubWindowmode: number = 1;
    public Regions: DropdownItemExt[] = [];
    public RegionDetail: RegionDetailModel;
    public ScheduleBuilder: SbDriverViewModel;
    public DriverViewShifts: ScheduleShiftModel[] = [];
    public TripDriverInfo = [];
    public TrailerViewTrailers: TrailerViewModel[] = [];
    public SbFilter: SbFilterModel = new SbFilterModel();
    public UpdateSbFilter: boolean = false;
    public TrailerViewFilter: TrailerViewFilterModel = new TrailerViewFilterModel;
    public DriverViewFilter: DriverViewFilterModel = new DriverViewFilterModel;
    public SelectedRegionId: string;
    public MaxInputDate: Date = new Date();
    public regionName: string = '';
    public OrderList: OrderPartialDetailModel[] = [];
    public CarrierRegions: CarrierRegionModel[] = [];
    public RegionCarriers: CarrierRegionModel[] = [];
    public AssignDrForm: FormGroup;
    public carrierDdlSetting: IDropdownSettings = {};
    //public CustomerLabel: string = "Carrier";
    public IsNoDriverShiftFound: number = 0;
    public ScheduleBuilderFilters = {
        ObjectFilter: ObjectFilter.Driver,
        DateFilter: DateFilter.Tomorrow,
        WindowMode: WindowModeFilter.Single,
        ToggleRequestMode: QueueFilter.DRs,
        RegionId: '',
        Date: '',
        DSBFilter: 2,
    };

    public allApiDeliveryRequest: DeliveryRequestViewModel[] = [];
    //public originalDRs_OnPageRefresh: DeliveryRequestViewModel[] = [];
    //unique products for product filter ddl
    public loadUniqueProducts: any[] = [];
    public deliveryRequests: DeliveryRequestViewModel[] = [];
    public activeQueueList: DeliveryRequestViewModel[] = [];
    public mustGoRequests: DelRequestsByJobModel[] = [];
    public tempmustGoRequests: DelRequestsByJobModel[] = [];
    public localmustGoRequests: DelRequestsByJobModel[] = [];
    public shouldGoRequests: DelRequestsByJobModel[] = [];
    public tempshouldGoRequests: DelRequestsByJobModel[] = [];
    public localshouldGoRequests: DelRequestsByJobModel[] = [];
    public couldGoRequests: DelRequestsByJobModel[] = [];
    public missedRequests: DelRequestsByJobModel[] = [];
    public tempMissedRequests: DelRequestsByJobModel[] = [];
    public localMissedRequests: DelRequestsByJobModel[] = [];
    public tempcouldGoRequests: DelRequestsByJobModel[] = [];
    public localcouldGoRequests: DelRequestsByJobModel[] = [];
    public tempDraggedRequest: DeliveryRequestViewModel[] = [];
    public memberInfo: MemberInfo[] = [];
    public requestToUpdate: DeliveryRequestViewModel = new DeliveryRequestViewModel(false);
    public blendRequestsToUpdate: DeliveryRequestViewModel[] = [];
    public blendTotalQuantity: number = 0;
    public blendAddRequestToUpdate: DeliveryRequestViewModel[] = [];
    public blendedProducts = "";
    public requestToBroker: DeliveryRequestViewModel = new DeliveryRequestViewModel(false);

    public assignedByOtherRegionRequests: DeliveryRequestViewModel[] = [];
    public assignedByOtherOperatorRequests: DeliveryRequestViewModel[] = [];
    public assignedToOtherRegionRequests: DeliveryRequestViewModel[] = [];
    public assignedToOtherOperatorRequests: DeliveryRequestViewModel[] = [];
    public SbForm: FormGroup;
    public updateDrForm: FormGroup;
    public DateChangeSubscription: Subscription;
    public UnsavedChangesSubscription: Subscription;
    public SavedChangesSubscription: Subscription;
    public EmptyUnSavedChangesSubscription: Subscription;
    public RegionChangeSubscription: Subscription;
    public RestoreDeletedRequestSubscription: Subscription;
    public DrUpdatedSubscription: Subscription;

    public RemoveDroppedRequestSubscription: Subscription;
    public DSBFilterChangeSubscription: Subscription;
    public tempChangeRegionChange: any;
    @ViewChild(DeliveryGroupComponent) deliveryGroupComponent: DeliveryGroupComponent;
    @ViewChild(SendbirdComponent) sendbirdComponent: SendbirdComponent;
    public clickMultiRequest: boolean = false;
    public disableControl: boolean = false;

    //route information
    public RouteName: string;
    public ShiftInfo: string;
    public RouteLocationList: RouteTfxJobsList[] = [];
    //schedule quantity type
    public ScheduleQuantityTypes: any = [];

    //otto setting
    public isDisplayOttoButton = false;
    public MinInputDate: Date = new Date();
    public IsOttoBuilderOpened: boolean = false;
    public IsOttoNotificationOpened: boolean = false;
    public OttoNotificationCount: number = 0;
    @ViewChild(RouteInfoComponent) routeInfoComponent: RouteInfoComponent;
    @ViewChild(SplitDeliveryRequestComponent) splitDeliveryRequestComponent: SplitDeliveryRequestComponent;

    //
    public shiftFilterSettings = {};
    public Shifts: ShiftViewModel[] = [];
    //dr filter 
    public drFilterForm: FormGroup;
    public terminalsForFilter: DropDownItem[] = [];
    public bulkPlantsForFilter: DropDownItem[] = [];
    public bulkCustomersForFilter = [];
    public settings: IDropdownSettings = {};
    public drFilterModel: DrFilterModel = new DrFilterModel();
    //private drFilterPreferences: any;
    private filterDataFromAPI = { FormData: null, RegionId: null };
    public IsDeliveryRequestRecieved: boolean = false;
    private drQueueChangesSubscription: Subscription;

    // filter aside
    public _opened: boolean = false;
    public _animate: boolean = true;
    public _positionNum: number = 0;
    public _POSITIONS: Array<string> = ['left', 'right', 'top', 'bottom'];
    public _showBackdrop: boolean = true;
    public _isInitiated: boolean = false;
    public IsCarrierCompanyStatus: boolean = false;
    //tpo location
    public currentUserCompanyType: number = currentUserCompanyType ? currentUserCompanyType : 0;
    public isSupplierCompany: boolean = isSupplierCompany;
    public isSupplierOrDispatcher: boolean = isSupplierOrDispatcher;

    public multiDropdownSettings = {};
    public CustomerSettings = {};
    public CustomerFilterSettings = {};
    public SiteddlSettings = {};
    public companyList = [];
    public SiteList = [];
    public AddDropLocationLoader: boolean = false;
    public Orders = [];
    public selectedOrder: any[] = [];
    public selectedCustomer: any[] = [];
    public selectedSite: any[] = [];
    public SelectedLocationId: number;
    public SelectedCustomerId: string;
    public SelectedFuelId: number;
    public CurrentDr: FormGroup;
    public currentJobDetails: JobDetailsWithOrders;
    public drOrders: JobDetailsWithOrders[];
    public isGroupDelivery: boolean = false;
    public regionDSBModel: RegionDSBModel = new RegionDSBModel();
    //spilt schedule quantity type
    public SpiltScheduleQuantityTypes: any = [];
    //show blinker if DR's of todays date.
    public isDrScheduledforToday: boolean = false;
    public TfxCarrierDropdownDisplayItem: TfxCarrierDropdownDisplayItem[] = [];
    public CompletedScheduleStatuses: number[] = [7, 8, 9, 10, 24, 5, 23];
    constructor(private fb: FormBuilder, private carrierService: CarrierService,
        private sbService: ScheduleBuilderService, private dataService: DataService,
        public deliveryRequestService: DeliveryRequestService, private chatService: chatService,
        private routeService: RouteInfoService, private cdRef: ChangeDetectorRef,
        private dispatcherService: DispatcherService, private deliveryReqService: DeliveryrequestService,
        private regionService: RegionService,
        private drFilterService: DrFilterService,
        public utilService: UtilService) {
        this.MaxInputDate.setFullYear(this.MaxInputDate.getFullYear() + 1);
        this.MinInputDate.setDate(this.MaxInputDate.getDate() + 1);
        this.drFilterForm = this.drFilterService.getDrFilterForm(true);
        this.drFilterModel = this.drFilterService.drFilterFormToModel(this.drFilterForm, IsCarrierCompany);
        this.initilizeAssignDrForm();
        this.IsCarrierCompanyStatus = IsCarrierCompany;
    }

    ngOnInit() {
        this.getDrFilterPreferences();
        this.subscribeDrQueueChanges();
        this.multiSelectSettings();
        this.subscribeLoadLocationSequenceSubject();
        this.getPreferenceSetting();

        this.settings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
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

        //this.resetLocalStorage();
        //this.resetTimer();
        this.tempDraggedRequest = [];
        this.tempChangeRegionChange = null;
        this.clickMultiRequest = false;
        this.SbForm = this.initForm();
        this._loadingRegions = true;

        this.updateFiltersFromLocalStorage();
        if (this.ScheduleBuilderFilters.Date == '') {
            this.ScheduleBuilderFilters.Date = moment().add(1, 'days').format('MM/DD/YYYY');
        }
        this.sbService.getRegions().subscribe(data => {
            this._loadingRegions = false;
            if (data != null && data != undefined) {
                this.Regions = data;
                if (data.length == 0) {
                    jQuery('#btnno_regions').click();
                }
                if (data.length > 0) {
                    this.refreshScheduleBuilder(this.ScheduleBuilderFilters.RegionId, this.ScheduleBuilderFilters.Date, this.ScheduleBuilderFilters.ObjectFilter, this.ScheduleBuilderFilters.DSBFilter, this.initScheduleBuilder);
                }
            }
        });
        this.SbForm.get('searchDRS').valueChanges
            .pipe(
                debounceTime(500),
                distinctUntilChanged()
            )
            .subscribe(searchText => {
                if (searchText != undefined) {
                    this.searchRecords(searchText);
                }
            });

        this.deliveryRequestService.localStroage.subscribe((data) => {
            if (data != undefined) {
                this.MoveActiveQueue(data);
            }
        });
        this.chatService.driverDetails.subscribe((data) => {
            if (data != undefined) {
                this.IntializeChat(data);
            }
        });
        this.chatService.loaderDetails.subscribe((data) => {
            if (data != undefined) {
                this._loadingBuilder = data;
            }
        });
        this.chatService.memberInfoDetails.subscribe((data) => {
            if (data != undefined) {
                this.memberInfo = data as MemberInfo[];
                this._loadingBuilder = false;
                jQuery('#btnconfirm-memberInfo').click();
            }
        });
        this.routeService.routeInfoDetails.subscribe((data) => {
            if (data != undefined) {
                var route = data as RouteInformationModel;
                this.RouteName = route.Name;
                this.ShiftInfo = route.ShiftInfoDetails;
                this.RouteLocationList = route.TfxJobs;
                jQuery('#btnconfirm-routeListInfo').click();
            }
        });
        this.chatService.defaultdriverChatDetails.subscribe((data) => {
            if (data != undefined) {
                let regionId = data.regionId;
                let driverArray = [];
                driverArray.push(data.driverId);
                this.IntializeSendBirdAccount(driverArray, regionId);
            }
        });
        this.SbForm.get('Shifts').valueChanges
            .pipe(
                debounceTime(500),
                distinctUntilChanged()
            )
            .subscribe(x => {
                this.setProductsForFilterDr(x);
            });
        this.activeQueueIcon();
        this.getOttoSetting();
        this.dataService.OpenDsbOttoNotificationSubject.subscribe(x => {
            this.IsOttoNotificationOpened = x;
        });
        this.dataService.DsbOttoNotificationCountSubject.subscribe(x => {
            this.OttoNotificationCount = x;
        });
        this.dataService.SplitDrsInfoSubject.subscribe(x => {
            this.splitDeliveryRequestComponent.getDeliveryRequestInfo(x);
        });
        this.shiftFilterSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'ShiftInfo',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
        };
        this.makeCarrierUIsortable()
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
        this.CustomerFilterSettings = {
            closeDropDownOnSelection: true,
            idField: 'CompanyId',
            textField: 'CompanyName',
            enableCheckAll: true,
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

    public _toggleOpened(shouldOpen: boolean): void {
        if (shouldOpen) {
            this._opened = true;
        }
        else {
            this._opened = !this._opened;
        }
    }

    public _toggleClosed(): void {
        this._opened = false;
    }

    public hideHeader() {
        this._toggle_header = !this._toggle_header;
    }

    public toggleDrPanel() {
        this._toggle_dr_panel = !this._toggle_dr_panel;
    }

    public toggleSearch() {

        this._toggle_search ? this.SbForm.get('searchDRS').setValue('') : null;
        this._toggle_search = !this._toggle_search;
    }
    clearDrFilterForm(flag: boolean) {
        this.drFilterForm = this.drFilterService.getDrFilterForm();
        this.drFilterForm.get('IsFilterApplied').setValue(flag);
        this.drFilterModel = this.drFilterService.drFilterFormToModel(this.drFilterForm, IsCarrierCompany);
    }
    openDrFilterForm() {
        this._isInitiated = true;
        this._toggleOpened(false);
        //close serach if open
        this._toggle_search ? this.toggleSearch() : null;

        this.validateFilterLocations();
        this.getCompaniesByRegion();
    }
    validateFilterLocations() {
        //get unique locations from dr
        let locations = this.drFilterService.getLocationsFromDr(this.allApiDeliveryRequest);
        this.terminalsForFilter = locations.terminals || [];
        this.bulkPlantsForFilter = locations.bulkPlants || [];
        this.bulkCustomersForFilter = locations.customerCompanies || [];
        //remove
        let selectedTerminals = this.drFilterForm.get('Terminals').value as DropDownItem[];
        let selectedBulkPlants = this.drFilterForm.get('BulkPlants').value as DropDownItem[];
        let selectedCustomers = this.drFilterForm.get('Customers').value as any[];

        if (selectedTerminals.length && this.IsDeliveryRequestRecieved && (this.SbForm.get('RegionId').value != this.filterDataFromAPI.RegionId)) {
            if (!selectedTerminals.some(selected => this.terminalsForFilter.some(term => term.Id == selected.Id))) {
                this.drFilterForm.get('Terminals').patchValue([]);
            }
        }
        if (selectedBulkPlants.length && this.IsDeliveryRequestRecieved && (this.SbForm.get('RegionId').value != this.filterDataFromAPI.RegionId)) {
            if (!selectedBulkPlants.some(selected => this.bulkPlantsForFilter.some(bulk => bulk.Id == selected.Id))) {
                this.drFilterForm.get('BulkPlants').patchValue([]);
            }
        }

        //filter customers from drs
        if (selectedCustomers.length && this.IsDeliveryRequestRecieved && (this.SbForm.get('RegionId').value != this.filterDataFromAPI.RegionId)) {
            if (!selectedCustomers.some(selected => this.bulkCustomersForFilter.some(bulk => bulk.Id == selected.CompanyId))) {
                this.drFilterForm.get('Customers').patchValue([]);
            }
        }
    }

    //Get Customers by Region
    getCompaniesByRegion() {
        if (this.SelectedRegionId != null && this.SelectedRegionId != undefined && this.SelectedRegionId != '') {
            this.carrierService.getJobListForCarrier(this.SelectedRegionId).subscribe(t2 => {
                this.bulkCustomersForFilter = t2 as CustomerJobsForCarrier[];
            });
        }
        else {
            Declarations.msgerror('No order exists for this Customers ', 'error', 2500)
        }
    }
    setLastDrFilterForm() {
        //if filter panel closed
        if (this.drFilterModel.Priority.length == 0 && this.drFilterForm.get('IsFilterApplied').value) {
            this.drFilterForm.get('Priority').get('MustGo').setValue(true);
            this.drFilterForm.get('IsFilterApplied').setValue(true);
            this.drFilterModel = this.drFilterService.drFilterFormToModel(this.drFilterForm, IsCarrierCompany);
        }
        else {
            this.drFilterForm = this.drFilterService.drFilterFormFromModel(this.drFilterModel);
        }
    }
    applyDrFilterInitiated() {
        this.drFilterModel = this.drFilterService.drFilterFormToModel(this.drFilterForm, IsCarrierCompany);
        if (this.drFilterModel.Priority.length == 0) {
            Declarations.msgerror("Please select at least one priority.", null, null, "bottom-right");
            return;
        }
        //close panel
        this._toggleClosed();
        //apply filter to dr
        this.applyDrFilter();
        //api call to save user preferences
        this.saveDrFilterPreferences();
    }
    applyDrFilter() {
        this.validateFilterLocations();
        this.drFilterForm.get('IsFilterApplied').setValue(true);
        this.drFilterModel = this.drFilterService.drFilterFormToModel(this.drFilterForm, IsCarrierCompany);
        var toggleRequestMode = this.SbForm.get('ToggleRequestMode').value;
        if (this._pubWindowmode == 1) {
            this.filterRequests(toggleRequestMode, this._pubWindowmode);
        }
        else {
            this.filterMultiWindowRequests();
        }
    }

    clearDrFiltersAndSearches() {
        let toggleRequestMode = this.SbForm.get('ToggleRequestMode').value;
        if (this._pubWindowmode == 1)
            this.assignTempRequest(toggleRequestMode, this._pubWindowmode);
        else
            this.assignMultiWindowRequest();
        //this.assignAssignedTabDrs();
    }
    saveDrFilterPreferences(): void {
        let data = new DrFilterPreferencesModel();
        data.Date = new Date();
        data.RegionId = this.SbForm.get('RegionId').value;
        data.FilterData = JSON.stringify(this.drFilterForm.value);
        this.filterDataFromAPI.FormData = this.drFilterForm.value;
        this.filterDataFromAPI.RegionId = this.SbForm.get('RegionId').value;
        this.drFilterService.saveDrFilterPreferences(data).subscribe();
    }

    getDrFilterPreferences(): void {
        this.drFilterService.getDrFilterPreferences().subscribe((setting: DrFilterPreferencesModel) => {
            if (setting && setting.StatusCode == 0 && setting.FilterData && this.drFilterService.validateFilterForm(JSON.parse(setting.FilterData))) {
                this.filterDataFromAPI.RegionId = setting.RegionId;
                this.filterDataFromAPI.FormData = JSON.parse(setting.FilterData);
                this.drFilterForm.patchValue(this.filterDataFromAPI.FormData);
                this.drFilterModel = this.drFilterService.drFilterFormToModel(this.drFilterForm, IsCarrierCompany);
            }
        });
    }
    searchRecords(terms: string) {
        if (terms == undefined || terms == null) { terms = ''; }
        terms = terms.toLowerCase().trim();
        let toggleRequestMode = this.SbForm.get('ToggleRequestMode').value;
        if (this._pubWindowmode == 1) {
            if (terms == '') {
                this.assignTempRequest(toggleRequestMode, this._pubWindowmode);
                //this.assignAssignedTabDrs();
            }
            else {
                this.searchRequests(terms, toggleRequestMode, this._pubWindowmode);
                //this.searchAssignedTabDr(terms);
            }
        }
        else {
            if (terms == '') {
                this.assignMultiWindowRequest();
                //this.assignAssignedTabDrs();
            }
            else {
                this.searchMultiWindowRequests(terms, toggleRequestMode, this._pubWindowmode);
                //this.searchAssignedTabDr(terms);
            }
        }
    }

    ngOnDestroy() {
        this.resetTimer();
        if (this.drQueueChangesSubscription) {
            this.drQueueChangesSubscription.unsubscribe();
        }

        if (this.locationsequenceSubscription) {
            this.locationsequenceSubscription.unsubscribe();
        }
    }

    assignTempRequest(queueMode: number, windowMode: number) {
        if (this.tempDraggedRequest.length > 0) {
            this.deliveryRequests = this.deliveryRequests.filter(({ Id: id1 }) => !this.tempDraggedRequest.some(({ Id: id2 }) => id2 === id1));
            this.deliveryRequests.slice();//update delivery request by removing draggred dr.
        }
        var priorityRequests = this.deliveryRequests.filter(element => element.WindowMode == windowMode && element.QueueMode === queueMode && element.ParentId == null);
        var missedRequests = this.deliveryRequests.filter(element => element.WindowMode == windowMode && element.QueueMode === queueMode && element.ParentId != null);
        this.getRequestsByPriority(priorityRequests, missedRequests);
    }

    assignMultiWindowRequest() {
        this.getLocalStorageQueueData();
    }

    resetTimer() {
        if (this.intervalTime) {
            clearInterval(this.intervalTime);
        }
    }

    resetLocalStorage() {
        localStorage.setItem("mustGoDeliveryRequest", JSON.stringify([]));
        localStorage.setItem("shouldGoDeliveryRequest", JSON.stringify([]));
        localStorage.setItem("couldGoDeliveryRequest", JSON.stringify([]));
        localStorage.setItem("missedDeliveryRequest", JSON.stringify([]));
        this.localmustGoRequests = [];
        this.localshouldGoRequests = [];
        this.localcouldGoRequests = [];
        this.localMissedRequests = [];
        //this.tempDraggedRequest = [];
        localStorage.setItem("updateRequest", JSON.stringify(false));
        localStorage.setItem("refreshRegion", JSON.stringify(false));
        localStorage.setItem("recordPriorityChange", JSON.stringify([]));
        this._refreshQueue = false;
    }

    initForm(): FormGroup {
        let date = this.getDayAfterTomorrowsDate();
        let _form = this.fb.group({
            Id: this.fb.control(''),
            Date: this.fb.control(date),
            RegionId: this.fb.control(''),
            ObjectFilter: this.fb.control(''),
            WindowMode: this.fb.control(''),
            ToggleRequestMode: this.fb.control(''),
            searchDRS: this.fb.control(''),
            selectedProduct: this.fb.control('-1'),
            RegionFilter: this.fb.control(''),
            DateFilter: this.fb.control(''),
            TimeStamp: this.fb.control(''),
            Status: this.fb.control(0),
            DeletedTripId: this.fb.control(null),
            DeletedGroupId: this.fb.control(null),
            Shifts: this.fb.array([]),
            Trailers: this.fb.array([]),
            DSBFilter: this.fb.control(2),
            SelectedShifts: this.fb.control([]),
            IsDsbDriverSchedule: this.fb.control(null),
            IsAssignDrEnabled: this.fb.control(null),
            PreferenceSetting: this.fb.control(null),
        });
        return _form;
    }
    subscribeEventSubscription(): void {
        this.subscribeDateChange();
        this.subscribeRegionChange();
        this.subscribeUnsavedChangesSubject();
        this.subscribeSavedChangesSubject();
        this.subscribeEmptyUnsavedChangesSubject();
        this.subscribeRestoreDeletedRequestSubject();
        this.subscribeDrUpdatedSubject();
        this.subscribeRemoveDroppedRequestSubject();
        this.subscribeDSBFilterChange();
    }

    unsubscribeEventSubscription(): void {
        if (this.DateChangeSubscription) {
            this.DateChangeSubscription.unsubscribe();
        }
        if (this.UnsavedChangesSubscription) {
            this.UnsavedChangesSubscription.unsubscribe();
        }
        if (this.SavedChangesSubscription) {
            this.SavedChangesSubscription.unsubscribe();
        }
        if (this.EmptyUnSavedChangesSubscription) {
            this.EmptyUnSavedChangesSubscription.unsubscribe();
        }
        if (this.RegionChangeSubscription) {
            this.RegionChangeSubscription.unsubscribe();
        }
        if (this.RestoreDeletedRequestSubscription) {
            this.RestoreDeletedRequestSubscription.unsubscribe();
        }
        if (this.DrUpdatedSubscription) {
            this.DrUpdatedSubscription.unsubscribe();
        }
        if (this.RemoveDroppedRequestSubscription) {
            this.RemoveDroppedRequestSubscription.unsubscribe();
        }
        if (this.DSBFilterChangeSubscription) {
            this.DSBFilterChangeSubscription.unsubscribe();
        }
    }

    subscribeDateChange(): void {
        this.DateChangeSubscription = this.SbForm.get('Date').valueChanges.subscribe(x => {
            if (x != null && x != undefined && x.trim() != '') {
                let regionId = this.SbForm.get('RegionId').value;
                let sbViewId = this.SbForm.get('ObjectFilter').value;
                let selectedDate = moment(x).format('MM/DD/YYYY');
                let sbDsbView = this.SbForm.get('DSBFilter').value;
                MyLocalStorage.setData(MyLocalStorage.DSB_DATE_KEY, selectedDate);
                this.refreshScheduleBuilder(regionId, selectedDate, sbViewId, sbDsbView, null);
                this.dataService.setDisableDSBControls(this.disableControl);
            }
        });
    }
    subscribeDSBFilterChange(): void {
        this.DSBFilterChangeSubscription = this.SbForm.get('DSBFilter').valueChanges.subscribe(x => {
            if (x != null && x != undefined) {
                let regionId = this.SbForm.get('RegionId').value;
                let sbViewId = this.SbForm.get('ObjectFilter').value;
                let selectedDate = this.SbForm.get('Date').value;
                let sbDsbView = this.SbForm.get('DSBFilter').value;
                this.refreshScheduleBuilder(regionId, selectedDate, sbViewId, sbDsbView, null);
            }
        });
    }

    subscribeUnsavedChangesSubject(): void {
        this.UnsavedChangesSubscription = this.dataService.UnsavedChangesSubject.subscribe(x => {
            if (x) {
                var existingObjectIndex = this._unsavedChanges.findIndex(t => t.ShiftIndex == x.ShiftIndex && t.DriverRowIndex == x.DriverRowIndex && t.DriverColIndex == x.DriverColIndex);
                if (existingObjectIndex == -1 && x.ShiftIndex != null && x.DriverRowIndex != null && x.DriverColIndex != null) {
                    let unSavedTrip: ModifiedTripInfo = new ModifiedTripInfo();
                    unSavedTrip.ShiftIndex = x.ShiftIndex;
                    unSavedTrip.DriverRowIndex = x.DriverRowIndex;
                    unSavedTrip.DriverColIndex = x.DriverColIndex;
                    this._unsavedChanges.push(unSavedTrip);
                }
            }
        });
    }

    subscribeSavedChangesSubject(): void {
        this.SavedChangesSubscription = this.dataService.SavedChangesSubject.subscribe(x => {
            if (x) {
                var existingObjectIndex = this._unsavedChanges.findIndex(t => t.ShiftIndex == x.ShiftIndex && t.DriverRowIndex == x.DriverRowIndex && t.DriverColIndex == x.DriverColIndex);
                if (existingObjectIndex > -1) {
                    this._unsavedChanges.splice(existingObjectIndex, 1);
                }
            };
        });
    }

    subscribeEmptyUnsavedChangesSubject(): void {
        this.EmptyUnSavedChangesSubscription = this.dataService.EmptyUnsavedChangesSubject.subscribe(x => {
            this._unsavedChanges = [];
        });
    }

    subscribeRegionChange(): void {
        this.RegionChangeSubscription = this.SbForm.get('RegionId').valueChanges.subscribe(x => {
            if (this.SbForm.get('WindowMode').value == 2 && this.clickMultiRequest == true) {
                var elementFound = this.Regions.find(element => element.Id === x);
                if (elementFound) {
                    this.regionName = elementFound.Name;
                }
                this.tempChangeRegionChange = x;
                jQuery('#btnconfirm-regionunsavedchanges').click();
            }
            else {
                this.onRegionChange(x);
                this.dataService.setShowDeliveryGroupSubject(false);
                MyLocalStorage.setData(MyLocalStorage.DSB_REGION_KEY, x);
            }
        });
    }

    subscribeDrUpdatedSubject(): void {
        this.DrUpdatedSubscription = this.dataService.DrUpdatedSubject.subscribe(x => {
            if (x) {
                let updatedDrs: DeliveryRequestViewModel[] = x;
                updatedDrs.forEach(
                    t => {
                        var dr = this.deliveryRequests.find(t1 => t1.Id == t.Id);
                        if (dr) {
                            dr.RequiredQuantity = t.RequiredQuantity;
                        }
                    }
                )
            }
        })
    }

    subscribeRestoreDeletedRequestSubject(): void {
        this.RestoreDeletedRequestSubscription = this.dataService.RestoreDeletedRequestSubject.subscribe(x => {
            if (x) {
                let newDrs: DeliveryRequestViewModel[] = x;
                newDrs.forEach(t => {
                    t.StatusClassId = 0;
                    t.Status = 2;
                    t.PreviousStatus = 0;
                    t.ScheduleStatus = 0;
                    t.SchedulePreviousStatus = 0;
                    t.DeliveryScheduleId = null;
                    t.DeliveryGroupId = null;
                    t.TrackableScheduleId = null;
                    t.GroupParentDRId = t.GroupParentDRId == null ? '' : t.GroupParentDRId;
                    t.TrackScheduleStatusName = '';
                });
                this.restoreDeleteDRs(newDrs);
                this.allApiDeliveryRequest = this.allApiDeliveryRequest.concat(newDrs);
                this.dataService.setAllDeliveryRequestsSubject(this.allApiDeliveryRequest);
            }
        });
    }

    subscribeRemoveDroppedRequestSubject(): void {
        this.RemoveDroppedRequestSubscription = this.dataService.RemoveDroppedRequestSubject.subscribe(x => {
            if (x) { // x will be array of objects
                for (var index = 0; index < x.length; index++) {
                    let dr = x[index];
                    var drIndex = this.tempDraggedRequest.findIndex(data => data.Id == dr.Id);
                    if (drIndex == -1) {
                        this.tempDraggedRequest.push(dr);
                    }
                    this.allApiDeliveryRequest = this.allApiDeliveryRequest.filter(t => t.Id != dr.Id);
                }
                //this.removeRequestFromLeftPanel(x);
                this.dataService.setAllDeliveryRequestsSubject(this.allApiDeliveryRequest);
            }
        });
    }

    isTrailerAssignedToDriverView(): boolean {
        let _assigned = true;
        let _shifts = this.SbForm.get('Shifts') as FormArray;
        _shifts.controls.forEach(x => {
            let _schedules = x.get('Schedules') as FormArray;
            _schedules.controls.forEach(y => {
                let _trips = y.get('Trips') as FormArray;
                let _trailers = y.get('Trailers').value;
                _trips.controls.forEach(z => {
                    if (_trailers.length == 0 && z.get('DeliveryRequests').value.length > 0) {
                        _assigned = false;
                    }
                });
            });
        });
        return _assigned;
    }

    isDriverAssignedToTrailerView(): boolean {
        let _assigned = true;
        let _trailers = this.SbForm.get('Trailers') as FormArray;
        _trailers.controls.forEach(x => {
            let _shifts = x.get('Shifts') as FormArray;
            _shifts.controls.forEach(y => {
                let _trips = y.get('Trips') as FormArray;
                _trips.controls.forEach(z => {
                    if (z.get('Drivers').value.length == 0 && z.get('DeliveryRequests').value.length > 0) {
                        _assigned = false;
                    }
                });
            });
        });
        return _assigned;
    }

    isDriverTrailerAssigned(filter: ObjectFilter): boolean {
        let _driverTrailerAssigned = false;
        if (filter == ObjectFilter.Driver) {
            _driverTrailerAssigned = this.isDriverAssignedToTrailerView();
        } else {
            _driverTrailerAssigned = this.isTrailerAssignedToDriverView();
        }
        return _driverTrailerAssigned;
    }

    onObjectFilterChange(filter: ObjectFilter): void {
        this.resetSbFilter();
        if (this.disableControl === false) {
            let _driverTrailedAssigned = this.isDriverTrailerAssigned(filter);
            if (!_driverTrailedAssigned) {
                jQuery('#btndriverTrailerAssignemtMsg').click();
                return;
            }
        }
        this.ScheduleBuilderFilters.ObjectFilter = filter;
        if (this._unsavedChanges.length > 0) {
            jQuery('#btnconfirm-unsavedchanges').click();
        } else {
            this.setObjectFilter(this.ScheduleBuilderFilters.ObjectFilter);
            let regionId = this.SbForm.get('RegionId').value;
            let sbViewId = this.SbForm.get('ObjectFilter').value;
            let date = this.SbForm.get('Date').value;
            let selectedDate = moment(date).format('MM/DD/YYYY');
            let sbDsbView = this.SbForm.get('DSBFilter').value;
            MyLocalStorage.setData(MyLocalStorage.DSB_OBJECTFILTER_KEY, filter);
            this.refreshScheduleBuilder(regionId, selectedDate, sbViewId, sbDsbView, null);
        }
    }

    onWindowFilterChange(filter: WindowModeFilter): void {
        this._refreshQueue = false;
        var previousWindowMode = this.ScheduleBuilderFilters.WindowMode;
        this.ScheduleBuilderFilters.WindowMode = filter;
        this._pubWindowmode = filter;
        this.setWindowFilter(this.ScheduleBuilderFilters.WindowMode);
        if (filter == 2) {
            this.updateLocalStorageActiveQueue();
            this.onQueueFilterChange(2);
            localStorage.setItem("refreshLocalStorage", 'true');
            this.activeTimer();
        }
        if (filter == 1 && previousWindowMode == 2 && this.clickMultiRequest == true) {
            //update local storage data.
            this.refreshSingleWindow();
        }
        MyLocalStorage.setData(MyLocalStorage.DSB_WINDOWMODE_KEY, filter);
    }
    updateLocalStorageActiveQueue() {
        var mustShouldCouldgoRecords = this.deliveryRequests.filter(top => top.ParentId == null && top.WindowMode === 1 && top.QueueMode === 2);
        if (mustShouldCouldgoRecords.length > 0) {
            mustShouldCouldgoRecords.forEach(function (value, index) {
                mustShouldCouldgoRecords[index].WindowMode = 2;
                mustShouldCouldgoRecords[index].QueueMode = 2;
            });
            var groupedDrs = this.deliveryReqService.groupDrsByJob(mustShouldCouldgoRecords);
            var mustGoRecords = this.deliveryReqService.getMustGoRequests(groupedDrs);
            if (mustGoRecords && mustGoRecords.length > 0)
                localStorage.setItem("mustGoDeliveryRequest", JSON.stringify(mustGoRecords));
            var shouldGoRecords = this.deliveryReqService.getShouldGoRequests(groupedDrs);
            if (shouldGoRecords && shouldGoRecords.length > 0)
                localStorage.setItem("shouldGoDeliveryRequest", JSON.stringify(shouldGoRecords));
            var couldGoRecords = this.deliveryReqService.getCouldGoRequests(groupedDrs);
            if (couldGoRecords && couldGoRecords.length > 0)
                localStorage.setItem("couldGoDeliveryRequest", JSON.stringify(couldGoRecords));
        }

        var missedRecords = this.deliveryRequests.filter(top => top.ParentId != null && top.WindowMode === 1 && top.QueueMode === 2);
        if (missedRecords.length > 0) {
            missedRecords.forEach(function (value, index) {
                missedRecords[index].WindowMode = 2;
                missedRecords[index].QueueMode = 2;
            });
            var missedReqLocations = this.deliveryReqService.groupDrsByJob(missedRecords, DeliveryRequestTypes.Missed);
            if (missedReqLocations && missedReqLocations.length > 0)
                localStorage.setItem("missedDeliveryRequest", JSON.stringify(missedReqLocations));
        }
    }
    onQueueFilterChange(filter: QueueFilter): void {
        this._loading_dr_panel = true;
        this.ScheduleBuilderFilters.ToggleRequestMode = filter;
        this.setQueueFilter(this.ScheduleBuilderFilters.ToggleRequestMode);
        if (filter == 1) {
            this.deliveryRequestService.ToggleQueueIcon(true);
            this._loading_dr_panel = false;
        } else {
            this.deliveryRequestService.ToggleQueueIcon(false);
            this._loading_dr_panel = false;
        }
        if (filter == 2 && this._pubWindowmode == 1) {
            this._refreshQueue = false;
            this._loading_dr_panel = false;
        }
        this.updateRecordsToggleRequest(filter);
        //this.SbForm.get('searchDRS').setValue('');
        this.clearDrFiltersAndSearches();
        MyLocalStorage.setData(MyLocalStorage.DSB_TOGGLEREQUESTMODE_KEY, filter);

        if (this.drFilterForm.get('IsFilterApplied').value) {
            this.dataService.setDrQueueChangesForFilter(true);
        }
    }

    onDateFilterChange(filter: DateFilter): void {
        this.resetSbFilter();
        this.ScheduleBuilderFilters.DateFilter = filter;
        if (this._unsavedChanges.length > 0) {
            jQuery('#btnconfirm-unsavedchanges').click();
        } else {
            this.setFilterDate(this.ScheduleBuilderFilters.DateFilter);
            if (filter == DateFilter.Date) {
                jQuery('#dateInput').focus();
            }
            MyLocalStorage.setData(MyLocalStorage.DSB_DATEFILTER_KEY, filter);
        }
    }

    filterChangeConfirmation(saveChanges: boolean): void {
        if (saveChanges) {
            return this.dataService.setSaveModifiedLoadsSubject(this._unsavedChanges);
        }
        else {
            this._unsavedChanges = [];
            //jQuery('#dateInput').click();
            //jQuery('#dateInput').focus();
        }
        this.dataService.setShowDeliveryGroupSubject(false);
        this.setObjectFilter(this.ScheduleBuilderFilters.ObjectFilter);
        this.setFilterDate(this.ScheduleBuilderFilters.DateFilter);
        this.setWindowFilter(this.ScheduleBuilderFilters.WindowMode);
        this.setQueueFilter(this.ScheduleBuilderFilters.ToggleRequestMode);
        MyLocalStorage.setData(MyLocalStorage.DSB_OBJECTFILTER_KEY, this.ScheduleBuilderFilters.ObjectFilter);
        MyLocalStorage.setData(MyLocalStorage.DSB_DATEFILTER_KEY, this.ScheduleBuilderFilters.DateFilter);
        MyLocalStorage.setData(MyLocalStorage.DSB_WINDOWMODE_KEY, this.ScheduleBuilderFilters.WindowMode);
    }

    filterChangeRegionConfirmation(saveChanges: boolean): void {
        if (saveChanges) {
            this.onRegionChange(this.tempChangeRegionChange);
            this.dataService.setShowDeliveryGroupSubject(false);
            localStorage.setItem("refreshRegion", JSON.stringify(true));
            MyLocalStorage.setData(MyLocalStorage.DSB_REGION_KEY, this.tempChangeRegionChange);
        }
        else {
            localStorage.setItem("refreshRegion", JSON.stringify(false));
            var tempRegionId = JSON.parse(localStorage.getItem("regionId"));
            if (tempRegionId != '') {
                this.SbForm.get('RegionId').setValue(tempRegionId);
                MyLocalStorage.setData(MyLocalStorage.DSB_REGION_KEY, tempRegionId);
            }
        }
    }
    filterChangeWindowConfirmation(saveChanges: boolean): void {
        if (saveChanges) {
            let regionId = this.SbForm.get('RegionId').value;
            let todaysDate = moment().add(1, 'days').format('MM/DD/YYYY');
            let sbViewId = this.SbForm.get('ObjectFilter').value;
            let sbDsbView = this.SbForm.get('DSBFilter').value;
            this.refreshScheduleBuilder(regionId, todaysDate, sbViewId, sbDsbView, this.initScheduleBuilder);
            localStorage.setItem("refreshRegion", JSON.stringify(true));
        }
        else {
            this.ScheduleBuilderFilters.WindowMode = 2;
            this._pubWindowmode = 2;
            this.setWindowFilter(this.ScheduleBuilderFilters.WindowMode);
            localStorage.setItem("refreshRegion", JSON.stringify(false));
        }
    }
    setFilterDate(filter: DateFilter): void {
        this.SbForm.get('DateFilter').setValue(filter);
        if (filter == DateFilter.Today) {
            this.SbForm.get('Date').setValue(this.getTodaysDate());
        } else if (filter == DateFilter.Tomorrow) {
            this.SbForm.get('Date').setValue(this.getTomorrowsDate());
        } else if (filter == DateFilter.YesterDay) {
            this.SbForm.get('Date').setValue(this.getYesterDayDate());
        }
        this.setReadOnlyControlsValue();
    }

    setObjectFilter(filter: ObjectFilter): void {
        let _objectFilter = this.SbForm.get('ObjectFilter');
        if (_objectFilter.value != filter) {
            _objectFilter.setValue(filter);
        }
    }

    setWindowFilter(filter: WindowModeFilter): void {
        let _windowFilter = this.SbForm.get('WindowMode');
        if (_windowFilter.value != filter) {
            _windowFilter.setValue(filter);
        }
    }

    setQueueFilter(filter: QueueFilter): void {
        let _queueFilter = this.SbForm.get('ToggleRequestMode');
        if (_queueFilter.value != filter) {
            _queueFilter.setValue(filter);
        }
    }

    getTodaysDate(): string {
        this.disableControl = false;
        return moment().format('MM/DD/YYYY');
    }

    getTomorrowsDate(): string {
        var current = new Date();
        current.setDate(current.getDate() + 1);
        this.disableControl = false;
        return moment(current).format('MM/DD/YYYY');
    }
    getYesterDayDate(): string {
        var current = new Date();
        current.setDate(current.getDate() - 1);
        this.disableControl = false;
        return moment(current).format('MM/DD/YYYY');
    }
    getDayAfterTomorrowsDate(): string {
        var current = new Date();
        current.setDate(current.getDate() + 2);
        this.disableControl = false;
        return moment(current).format('MM/DD/YYYY');
    }

    setSelectedDate(date: string) {
        if (date != null && date != undefined && date.trim() != '') {
            var _date = this.SbForm.controls['Date'];
            if (_date.value != date) {
                _date.setValue(date);
            }
            let status = this.disableControls(date);
            this.setReadOnlyControlsValue();
            this.dataService.setDisableDSBControls(status);
        }
    }
    disableControls(date: any): boolean {
        var currentDate = moment(date, "MM/DD/YYYY");
        let status = false;
        if (moment().diff(currentDate, 'days') > 1) {
            status = true;
        }
        this.disableControl = status;
        return status;
    }
    onRegionChange(regionId: any): void {
        //var regionId = event.target.selectedOptions[0].value;
        this.IsDeliveryRequestRecieved = false;
        this.resetSbFilter();
        let selectedDate = moment(this.SbForm.get('Date').value).format('MM/DD/YYYY');
        let sbViewId = this.SbForm.get('ObjectFilter').value;
        let sbDsbView = this.SbForm.get('DSBFilter').value;
        this.refreshScheduleBuilder(regionId, selectedDate, sbViewId, sbDsbView, null);
        if (regionId != null) {
            this.getRegiondetail(regionId);
        }
        this.resetTimer();
        this.resetLocalStorage();
    }

    private dateFormat = new RegExp("^((0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](20)?[0-9]{4})*$");
    refreshScheduleBuilder(regionId: string, selectedDate: string, sbViewId: number, sbDsbView: number, callback: any): void {
        if (this.dateFormat.test(selectedDate)) {
            this.getScheduleBuilder(regionId, selectedDate, sbViewId, sbDsbView, callback);
        }
    }

    getRegiondetail(regionId: string): void {
        this.SelectedRegionId = regionId;
        MyLocalStorage.setData(MyLocalStorage.DSB_REGION_KEY, this.SelectedRegionId);
        this.sbService.getRegionDetails(regionId)
            .subscribe(detail => {
                this.RegionDetail = detail;
                if (detail && detail.Trailers && detail.Trailers.length == 0) {
                    this._isTrailerExists = false;
                }
                else {
                    this._isTrailerExists = true;
                }
                this.setSbFilter();
            });
        this.getOttoNotificationCount();
    }

    initScheduleBuilder(self: any, sbModel): void {
        self.SbForm.patchValue({
            Id: sbModel.Id,
            Date: sbModel.Date,
            RegionId: sbModel.RegionId,
            ObjectFilter: sbModel.ObjectFilter,
            RegionFilter: sbModel.RegionFilter,
        });
        var _dateFilter = self.SbForm.get('DateFilter');
        if (_dateFilter.value == null || _dateFilter.value == undefined || _dateFilter.value == '') {
            _dateFilter.setValue(sbModel.DateFilter);
        }
        if (sbModel.RegionId != null)
            self.getRegiondetail(sbModel.RegionId);
    }

    //get unique products for dr filter
    setProductsForFilterDr(formDr: ScheduleShiftModel[]) {
        this._loadingBuilder = true;
        this.SbForm.get('selectedProduct').setValue('-1');
        this.loadUniqueProducts = []
        let sbDr = this.ScheduleBuilder.Shifts;
        let finalDr: ScheduleShiftModel[] = [];

        if (formDr && formDr.length) { finalDr.push(...formDr); }
        if (sbDr && sbDr.length) { finalDr.push(...sbDr); }

        if (finalDr && finalDr.length) {
            finalDr.forEach(shft => {
                if (shft && shft.Schedules) {
                    shft.Schedules.forEach(schdl => {
                        if (schdl && schdl.Trips) {
                            schdl.Trips.forEach(trp => {
                                if (trp && trp.DeliveryRequests) {
                                    trp.DeliveryRequests.forEach(dlReq => {
                                        if (dlReq && dlReq.ProductType) {
                                            if (this.loadUniqueProducts.indexOf(dlReq.ProductType) === -1) {
                                                this.loadUniqueProducts.push(dlReq.ProductType);
                                            }
                                        }
                                    });
                                }
                            });
                        }
                    });
                }
            });
        }
        this.cdRef.detectChanges();
        if (this.drFilterForm.get('IsFilterApplied').value != true) {
            this.clearDrFiltersAndSearches();
        }
        else {
            this.applyDrFilter();
        }
        this._loadingBuilder = false;
    }

    private getDateFilter(date: string): number {
        var today = this.getTodaysDate();
        var tomorrow = this.getTomorrowsDate();
        var yesterday = this.getYesterDayDate();
        let dateFilter = 3;
        if (date == today)
            dateFilter = 1;
        else if (date == tomorrow)
            dateFilter = 2;
        else if (date == yesterday)
            dateFilter = 4;
        return dateFilter;
    }

    getScheduleBuilder(regionId: string, date: string, sbViewId: number, sbDsbView: number, callback: any): void {
        //if region id null or dropdown of region id empty then set first region id value and fetch the schedule builder.
        if (this.Regions.length > 0 && this.Regions.findIndex(x => x.Id == regionId) == -1) {
            this.SbForm.controls['RegionId'].setValue(this.Regions[0].Id);
            regionId = this.Regions[0].Id;
        }
        this._loadingBuilder = true;
        this._unsavedChanges = [];
        this.activeQueueList = [];
        this.tempDraggedRequest = [];
        this.tempmustGoRequests = [];
        this.tempshouldGoRequests = [];
        this.tempcouldGoRequests = [];
        this.tempMissedRequests = [];
        this.unsubscribeEventSubscription();
        this.dataService.FormChangeSubscription && this.dataService.FormChangeSubscription.forEach(t => {
            if (t) {
                t.unsubscribe();
            }
        });

        this.disableControls(date);
        if (this.disableControl == false) {
            this.getDeliveryRequests(regionId, sbViewId, null, date);
        }

        this.sbService.getScheduleBuilder(regionId, date, sbViewId, sbDsbView)
            .subscribe(data => {
                //this.resetLocalStorage();
                //this.resetTimer();
                data.DateFilter = this.getDateFilter(data.Date);
                this._loadingBuilder = false;
                this.ScheduleBuilder = data;
                if (this.ScheduleBuilder.ObjectFilter == 2) {
                    this.dataService.setTrailerShiftsSubject(this.ScheduleBuilder.Shifts);
                }
                this.DriverViewShifts = this.ScheduleBuilder.Shifts;
                this.TrailerViewTrailers = this.ScheduleBuilder.Trailers;
                this.SbForm.controls['Id'].patchValue(data.Id);
                this.SbForm.controls['TimeStamp'].patchValue(data.TimeStamp);
                this.SbForm.controls['IsDsbDriverSchedule'].patchValue(data.IsDsbDriverSchedule);
                if (data.WindowMode == 0) {
                    data.WindowMode = 1;
                }
                let _windowMode = MyLocalStorage.getData(MyLocalStorage.DSB_WINDOWMODE_KEY);
                if (_windowMode != '') { data.WindowMode = _windowMode; }
                this.SbForm.controls['WindowMode'].patchValue(data.WindowMode);
                let _toggleReqMode = MyLocalStorage.getData(MyLocalStorage.DSB_TOGGLEREQUESTMODE_KEY);
                if (_toggleReqMode != '') { data.ToggleRequestMode = _toggleReqMode; }
                if (data.DSBFilter != 0) {
                    MyLocalStorage.setData(MyLocalStorage.DSB_FILTER_KEY, data.DSBFilter);
                }
                let _dsbFilterKey = MyLocalStorage.getData(MyLocalStorage.DSB_FILTER_KEY);
                if (_dsbFilterKey != '') { data.DSBFilter = _dsbFilterKey; }
                this.SbForm.controls['ToggleRequestMode'].patchValue(data.ToggleRequestMode);
                this.SbForm.controls['DateFilter'].patchValue(data.DateFilter);
                this.SbForm.controls['DSBFilter'].patchValue(data.DSBFilter);
                let status = this.disableControls(data.Date);
                this.dataService.setDisableDSBControls(status);
                this._pubWindowmode = data.WindowMode;
                this.resetSbFilter();
                this.setSbFilter();

                if (callback) {
                    callback(this, this.ScheduleBuilder);
                }
                //this._loadingBuilder = false;
                this.subscribeEventSubscription();
                let objectFilter = this.SbForm.get('ObjectFilter').value
                this.IsNoDriverShiftFound = this.ScheduleBuilder.IsNoDriverShiftFound;
                if ((this.Regions && this.Regions.length > 0) && (objectFilter == 1) && (this.DriverViewShifts && this.DriverViewShifts.length == 0)) {
                    //IsNoDriverShiftFound verify driver does not have schedules for that shifts.
                    //but regions has shifts here.
                    if (this.ScheduleBuilder.DSBFilter == 2) {
                        if (this.ScheduleBuilder.IsNoDriverShiftFound == 0) {
                            jQuery('#btnno_shifts').click();
                        }
                        else {
                            if (this.DriverViewShifts.length == 0) {
                                this.Shifts = [];
                                this.SbForm.get('SelectedShifts').patchValue(this.Shifts);
                            }
                        }
                    }
                    else {
                        jQuery('#btnno_shifts').click();
                    }
                }
                if (this.RegionDetail && this.RegionDetail.Trailers && this.RegionDetail.Trailers.length == 0) {
                    this._isTrailerExists = false;
                }
                else {
                    this._isTrailerExists = true;
                }

                //assign driver information
                if (this.ScheduleBuilder.ObjectFilter == 1) {
                    let driverdetails = this.LoadDriverDetails(this.DriverViewShifts) as [];
                    if (driverdetails.length > 0) {
                        this.TripDriverInfo.push.apply(driverdetails);
                        this.IntializeSendBirdAccount(driverdetails, regionId);
                    }
                }
                else {
                    let driverdetails = this.LoadDriverDetailsShift(this.TrailerViewTrailers) as [];
                    if (driverdetails.length > 0) {
                        this.TripDriverInfo.push.apply(driverdetails);
                        this.IntializeSendBirdAccount(driverdetails, regionId);
                    }
                }
                //get shift information for region.. used for filter.
                this.getShiftInformation(this.ScheduleBuilder.Shifts, data.RegionId);
            });
    }
    isCalendarDrPresent(drs: any, date?: string) {
        return drs.some(p => p.IsCalendarView == true && p.SelectedDate == date) ? true : false
    }
    getDeliveryRequests(regionId: string, sbViewId: number, status: number = 0, date?: string): void {
        if (date == null || date == undefined) {
            date = this.ScheduleBuilderFilters.Date;
        }
        this._loadingDrRequests = true;
        this.carrierService.getDeliveryRequests(regionId, date).subscribe((drs: any[]) => {
            if (drs != null && drs != undefined) {
                this.isDrScheduledforToday = this.isCalendarDrPresent(drs, date); // verify before filtering Dr
                drs = drs.filter(t => !t.IsCalendarView);
                if (sbViewId == 1) {
                    drs = this.filterDrByScheduleBuilder(drs);
                }
                drs = this.updateRequest(drs, status);
                this.allApiDeliveryRequest = drs;
                //this.originalDRs_OnPageRefresh = drs;
                this.filterRequest(drs);
                this.refreshUpdateData(status);
                if (this._pubWindowmode == 2) {
                    this.getLocalStorageQueueData();
                    localStorage.setItem("refreshLocalStorage", 'true');
                    this.activeTimer();
                } else {
                    this.resetTimer();
                }
                this.dataService.setAllDeliveryRequestsSubject(drs);
            }
            this.IsDeliveryRequestRecieved = true;
            if (this.drFilterForm.get('IsFilterApplied').value) {
                this.dataService.setDrQueueChangesForFilter(true);
            }
            this._loadingDrRequests = false;
        });
    }

    subscribeDrQueueChanges() {
        this.drQueueChangesSubscription = this.dataService.drQueueChangesForFilter
            //.pipe(debounceTime(250))
            .subscribe(value => {
                if (value) {
                    this._toggle_search = false;
                    this.applyDrFilter();
                }
            })
    }

    filterDrByProductType(productType: string) {
        this.drFilterForm.get('IsFilterApplied').setValue(false);
        this.drFilterModel = this.drFilterService.drFilterFormToModel(this.drFilterForm, IsCarrierCompany);

        //if 'all product' selected
        if (productType == "-1") {
            this.searchRecords('');
        }
        //else filter dr by product
        else {
            var priorityRequests = this.allApiDeliveryRequest.filter(t => t.ProductType == productType && t.ParentId == null && t.AssignedToCompanyId == currentUserCompanyId);
            var missedRequests = this.allApiDeliveryRequest.filter(t => t.ProductType == productType && t.ParentId != null && t.AssignedToCompanyId == currentUserCompanyId);
            this.getRequestsByPriority(priorityRequests, missedRequests);
        }
    }

    filterRequest(dr: DeliveryRequestViewModel[]) {
        this.deliveryRequests = dr;
        var priorityRequests = this.allApiDeliveryRequest.filter(t => t.ParentId == null && t.AssignedToCompanyId == currentUserCompanyId);
        var missedRequests = dr.filter(t => t.ParentId != null && t.AssignedToCompanyId == currentUserCompanyId);
        this.getRequestsByPriority(priorityRequests, missedRequests);

        this.tempshouldGoRequests = this.shouldGoRequests;
        this.tempcouldGoRequests = this.couldGoRequests;
        this.tempmustGoRequests = this.mustGoRequests;
        this.tempMissedRequests = this.missedRequests;
        this.assignedByOtherRegionRequests = dr.filter(t => t.AssignedToCompanyId == currentUserCompanyId && t.SupplierCompanyId == currentUserCompanyId && t.SchedulePreviousStatus == 2);
        this.assignedByOtherOperatorRequests = dr.filter(t => t.AssignedToCompanyId == currentUserCompanyId && t.SupplierCompanyId != currentUserCompanyId && t.SchedulePreviousStatus == 2);
        this.assignedToOtherRegionRequests = dr.filter(t => t.AssignedToCompanyId == currentUserCompanyId && t.SchedulePreviousStatus == 2);
        this.assignedToOtherOperatorRequests = dr.filter(t => t.AssignedToCompanyId != currentUserCompanyId && t.SupplierCompanyId == currentUserCompanyId && t.SchedulePreviousStatus == 2);
    }

    removeRequestFromLeftPanel(draggedRequests: DeliveryRequestViewModel[]): void {
        var deliveryRequests = this.getAllRequests();
        deliveryRequests = deliveryRequests.filter(({ Id: id1 }) => !draggedRequests.some(({ Id: id2 }) => id2 === id1));
        var priorityReqs = deliveryRequests.filter(t => t.ParentId = null);
        var missedReqs = deliveryRequests.filter(t => t.ParentId != null);
        this.getRequestsByPriority(priorityReqs, missedReqs);
    }

    restoreDeleteDRs(requests: any) {
        var delReqs: DeliveryRequestViewModel[] = requests;
        delReqs.forEach(x => {
            var originalDr = this.tempDraggedRequest.find(data => data.Id == x.Id);
            if (originalDr) {
                x['WindowMode'] = originalDr.WindowMode;
                x['QueueMode'] = originalDr.QueueMode;
            }
        });
        delReqs = delReqs.filter(x => x.SchedulePreviousStatus != DeliveryReqStatus.ScheduleCreated);
        if (this._pubWindowmode == 2) {
            var localStorageRequests = this.getLocalStorageRequests();
            delReqs.forEach(x => {
                var isDrExists = localStorageRequests.findIndex(t => t.Id == x.Id) != -1;
                if (isDrExists) {
                    if (x.ParentId != null) {
                        this.pushItemsInMissedAndUpdateDrPanel(x);
                    }
                    else {
                        this.pushItemsAndUpdateDrPanel(x);
                    }
                }
                else {
                    Declarations.msgwarning("The reset item already removed from the session.so we are not moving that item in active queue.", undefined, undefined);
                }
                this.removeItemFromTempDrageQueue(x.Id, x.Priority, x.ParentId);
            });
        }
        else {
            delReqs.forEach(x => {
                if (this.deliveryRequests.findIndex(r => r.Id == x.Id) == -1) {
                    this.deliveryRequests.push(x);
                }
                else {
                    var deliveryRequests = this.deliveryRequests.find(r => r.Id == x.Id);
                    if (deliveryRequests != null) {
                        deliveryRequests.DeliveryLevelPO = x.DeliveryLevelPO;
                        deliveryRequests.Notes = x.Notes;
                        deliveryRequests.RequiredQuantity = x.RequiredQuantity;
                        deliveryRequests.ScheduleQuantityType = x.ScheduleQuantityType;
                        deliveryRequests.Priority = x.Priority;
                        deliveryRequests.ScheduleStartTime = x.ScheduleStartTime;
                        deliveryRequests.ScheduleEndTime = x.ScheduleEndTime;
                        deliveryRequests.SelectedDate = x.SelectedDate;
                    }
                }
                this.removeItemFromTempDrageQueue(x.Id, x.Priority, x.ParentId);
                let toggleRequestMode = this.SbForm.get('ToggleRequestMode').value;
                this.updateRecordsToggleRequest(toggleRequestMode);
            });
        }
    }

    pushLocation(job: DelRequestsByJobModel) {
        switch (job.Priority) {
            case DeliveryReqPriority.MustGo:
                job.DeliveryRequestType = DeliveryRequestTypes.MustGo;
                this.mustGoRequests.push(job);
                this.mustGoRequests.slice();
                break;
            case DeliveryReqPriority.ShouldGo:
                job.DeliveryRequestType = DeliveryRequestTypes.ShouldGo;
                this.shouldGoRequests.push(job);
                this.shouldGoRequests.slice();
                break;
            case DeliveryReqPriority.CouldGo:
                job.DeliveryRequestType = DeliveryRequestTypes.CouldGo;
                this.couldGoRequests.push(job);
                this.couldGoRequests.slice();
                break;
        }
    }

    pushItemsAndUpdateDrPanel(dr: DeliveryRequestViewModel) {
        var jobIndex = this.mustGoRequests.findIndex(t => t.JobId == dr.JobId);
        if (jobIndex != -1) {
            var job = this.mustGoRequests[jobIndex];
            job.DeliveryRequests.push(dr);
            job.Priority = this.deliveryReqService.getPriority(job.DeliveryRequests);
            if (job.Priority == DeliveryReqPriority.MustGo) {
                this.mustGoRequests[jobIndex] = job;
                this.mustGoRequests = this.mustGoRequests.slice();
            }
            else {
                this.pushLocation(job);
                this.mustGoRequests.splice(jobIndex, 1);
            }
        }
        else {
            jobIndex = this.shouldGoRequests.findIndex(t => t.JobId == dr.JobId);
            if (jobIndex != -1) {
                var job = this.shouldGoRequests[jobIndex];
                job.DeliveryRequests.push(dr);
                job.Priority = this.deliveryReqService.getPriority(job.DeliveryRequests);
                if (job.Priority == DeliveryReqPriority.ShouldGo) {
                    this.shouldGoRequests[jobIndex] = job;
                    this.shouldGoRequests = this.shouldGoRequests.slice();
                }
                else {
                    this.pushLocation(job);
                    this.shouldGoRequests.splice(jobIndex, 1);
                }
            }
            else {
                jobIndex = this.couldGoRequests.findIndex(t => t.JobId == dr.JobId);
                if (jobIndex != -1) {
                    var job = this.mustGoRequests[jobIndex];
                    job.DeliveryRequests.push(dr);
                    job.Priority = this.deliveryReqService.getPriority(job.DeliveryRequests);
                    if (job.Priority == DeliveryReqPriority.MustGo) {
                        this.couldGoRequests[jobIndex] = job;
                        this.couldGoRequests = this.couldGoRequests.slice();
                    }
                    else {
                        this.pushLocation(job);
                        this.couldGoRequests.splice(jobIndex, 1);
                    }
                }
            }
        }
    }

    pushItemsInMissedAndUpdateDrPanel(dr: DeliveryRequestViewModel) {
        var jobIndex = this.missedRequests.findIndex(t => t.JobId == dr.JobId);
        if (jobIndex != -1) {
            var job = this.missedRequests[jobIndex];
            job.DeliveryRequests.push(dr);
            job.Priority = this.deliveryReqService.getPriority(job.DeliveryRequests);
            this.missedRequests[jobIndex] = job;
            this.missedRequests = this.missedRequests.slice();
        }
    }

    removeItemFromTempDrageQueue(Id: string, Priority: number, ParentId: string) {
        var index = this.tempDraggedRequest.findIndex(data => data.Id == Id);
        if (index != -1) {
            this.tempDraggedRequest.splice(index, 1);
            this.tempDraggedRequest.slice(); //refresh tempDragged Queue
        }
    }

    getTotalBlendQuantity(): number {
        return this.blendRequestsToUpdate.map(t => t.RequiredQuantity).reduce((a, b) => a + b, 0);
    }
    toggleBlendQuantity(req: DeliveryRequestViewModel, isPercent: boolean) {
        if (isPercent) {
            req.RequiredQuantity = (this.blendTotalQuantity * req.QuantityInPercent) / 100;
        } else {
            req.QuantityInPercent = (req.RequiredQuantity / this.blendTotalQuantity) * 100;
        }
    }
    toggleBlendTotalQuantity() {
        this.blendRequestsToUpdate.forEach(t => {
            this.toggleBlendQuantity(t, true);
        });
    }
    IsValidBlendQuantity(): boolean {
        return this.blendRequestsToUpdate.map(t => t.QuantityInPercent).reduce((a, b) => a + b, 0) == 100;
    }

    onDeliveryReqUpdate(status: number) {
        //VALIDATION
        if (status == 1) {
            var tnkRequiredQuantity: number = this.requestToUpdate.RequiredQuantity;
            if (this.requestToUpdate.IsBlendedRequest) {
                tnkRequiredQuantity = this.getTotalBlendQuantity();
                if (this.blendAddRequestToUpdate)
                    tnkRequiredQuantity = (+tnkRequiredQuantity) + (+this.blendAddRequestToUpdate.map(t => t.RequiredQuantity).reduce((a, b) => a + b, 0));
            }
            if (this.requestToUpdate.ScheduleQuantityType == 1 && (!(tnkRequiredQuantity > 0) || tnkRequiredQuantity < 0.00001)) {
                Declarations.msgerror("Invalid required quantity.", undefined, undefined); return;
            }
            else if (this.requestToUpdate.ScheduleQuantityType == 1 && this.requestToUpdate.TankMaxFill && this.requestToUpdate.TankMaxFill > 0 && !this.requestToUpdate.IsMaxFillAllowed) {
                if (tnkRequiredQuantity > this.requestToUpdate.TankMaxFill) {
                    Declarations.msgerror("Should not exceed max fill. (" + this.requestToUpdate.TankMaxFill + ")", undefined, undefined); return;
                }
            }
        }
        if (this.requestToUpdate.IsSpiltDRAdded) {
            var filter = this.requestToUpdate.SpiltDRs.filter(x => !(x.RequiredQuantity > 0)).length;
            if (filter > 0) {
                Declarations.msgerror("Quantity required for Spilt DRs.", undefined, undefined); return;
            }
        }
        jQuery('#closeEditDrPanel').click();
        this._loadingDrRequests = true;
        if (this.requestToUpdate.ScheduleQuantityType > 1) { this.requestToUpdate.RequiredQuantity = 0; }
        let updateRequests = [this.requestToUpdate];
        if (this.requestToUpdate.IsBlendedRequest) {
            if (status == 1) {
                let drNotes = this.requestToUpdate.Notes;
                let drSelectedDate = this.requestToUpdate.SelectedDate;
                let drScheduleStartTime = this.requestToUpdate.ScheduleStartTime;
                let drScheduleEndTime = this.requestToUpdate.ScheduleEndTime;
                let deliveryLevelPO = this.requestToUpdate.DeliveryLevelPO;
                let drPriority = this.requestToUpdate.Priority;
                $.each(this.blendRequestsToUpdate, function () {
                    this.Notes = drNotes; this.Priority = drPriority; this.DeliveryLevelPO = deliveryLevelPO;
                    this.SelectedDate = drSelectedDate; this.ScheduleStartTime = drScheduleStartTime; this.ScheduleEndTime = drScheduleEndTime;
                });
            }
            updateRequests = this.blendRequestsToUpdate;
            if (this.blendAddRequestToUpdate) {
                this.blendAddRequestToUpdate.forEach(t => {
                    t.SelectedDate = this.requestToUpdate.SelectedDate;
                    t.ScheduleStartTime = this.requestToUpdate.ScheduleStartTime;
                    t.ScheduleEndTime = this.requestToUpdate.ScheduleEndTime;
                    t.DeliveryLevelPO = this.requestToUpdate.DeliveryLevelPO;
                    if (t.RequiredQuantity > 0 || t.ScheduleQuantityType != 1)
                        updateRequests.push(t);
                })
            }
        }
        this.carrierService.updateDeliveryRequest(updateRequests)
            .subscribe((data: any) => {
                this._loadingDrRequests = false;
                if (data.StatusCode == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                    this.refreshDeliveryRequests(status);

                }
                else if (data.StatusCode == 2) {
                    Declarations.msgwarning(data.StatusMessage, undefined, undefined);
                    this.refreshDeliveryRequests(status);
                }
                else {
                    Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                }
            });

    }

    refreshUpdateData(status: number) {
        if (this._pubWindowmode == 1) {
            if (this.SbForm.get('ToggleRequestMode').value != 2) {
                this.onQueueFilterChange(1);
            }
            else {
                this.onQueueFilterChange(2);
            }
        }
        else {
            this.setQueueFilter(2);
            this.refreshUpdateRecordMultiwindow(this.requestToUpdate, status);
        }
    }

    public refreshDeliveryRequests(status: number) {
        let regionId = this.SbForm.get('RegionId').value;
        let sbViewId = this.SbForm.get('ObjectFilter').value;
        let selectedDate = this.SbForm.get('Date').value;
        this.getDeliveryRequests(regionId, sbViewId, status, selectedDate);
    }

    getScheduleQuantityType() {
        if (this.ScheduleQuantityTypes.length == 0) {
            this.dispatcherService.GetScheduleQtyType().subscribe((SQT: any[]) => {
                this.ScheduleQuantityTypes = SQT || [];
                this.SpiltScheduleQuantityTypes = SQT.filter(x => x.Id == 1) || [];
            });
        }
    }

    public BrokeredDrStatus: BrokeredDrCarrierStatus = null;
    getDeliveryReq(delReqForm: any) {
        let delReq = delReqForm.deliveryRequest.value as DeliveryRequestViewModel;
        this.BrokeredDrStatus = delReqForm.CarrierStatus;
        var allDrs = this.getAllRequests();
        var drToupdate = allDrs.find(t => t.Id == delReq.Id);
        this.blendRequestsToUpdate = [];
        this.blendTotalQuantity = 0;
        this.blendAddRequestToUpdate = [];
        this.requestToUpdate = Object.assign({}, drToupdate);
        if (this.requestToUpdate.IsBlendedRequest) {
            var tempBlendDrs = allDrs.filter(t => t.BlendedGroupId == delReq.BlendedGroupId);
            this.blendedProducts = tempBlendDrs.map(t => t.ProductType).join(", ");
            this.blendRequestsToUpdate = tempBlendDrs.filter(t => !t.IsAdditive);
            this.blendAddRequestToUpdate = tempBlendDrs.filter(t => t.IsAdditive);
            this.blendTotalQuantity = this.getTotalBlendQuantity();
        }
        this.AssignDrForm.get('DispatcherNote').setValue('');
        if (delReqForm.isDeleted) {
            this.requestToUpdate.IsDeleted = true;
            if (this.requestToUpdate.IsBlendedRequest) {
                $.each(this.blendRequestsToUpdate, function () { this.IsDeleted = true; });
                if (this.blendAddRequestToUpdate)
                    $.each(this.blendAddRequestToUpdate, function () { this.IsDeleted = true; });
            }
            let element = document.getElementById("openDeleteDeliveryRequestModal");
            element ? element.click() : null;
        }
        else if (delReqForm.isAssignCarrier) {
            this.getAllCarrierRegions();
            this.cancelAssignDrsToCarrier()
            this.initilizeAssignDrForm();
            if (drToupdate != null) {
                delReq.UniqueOrderNo = drToupdate.UniqueOrderNo;
            }
            this.getOrdersAndCarrierDetails(delReq);
        }
        else if (delReqForm.isApproveRejectBrokeredDr) {
            this.requestToUpdate = Object.assign({}, delReq);
            let element = document.getElementById("openProceedBrokeredDrModal");
            element ? element.click() : null;
        }
        else if (delReqForm.isCreateDeliveryForTBD) {
            this.isGroupDelivery = delReqForm.isDeliveryGroup;
            let elem = document.getElementById('open-CreateDelivery'); elem.click();
            this.getDropLocationDetails(delReqForm.deliveryRequest);
        }
        else {
            this.getScheduleQuantityType();
            if (this.requestToUpdate.ScheduleQuantityType == 0) { this.requestToUpdate.ScheduleQuantityType = 1 }
            let element = document.getElementById("openUpdateDrModal");
            element ? element.click() : null;
        }
        this.requestToUpdate.IsSpiltDRAdded = false;
        this.requestToUpdate.SpiltDRs = [];
    }

    public changeBrokeredDrStatus(drId: string, blendedGroupId: string, status: BrokeredDrCarrierStatus) {
        this._acceptRejectDr = true;
        this._loadingCarriers = true;
        this.carrierService.changeBrokeredDrStatus(drId, blendedGroupId, status).subscribe((data: any) => {
            this._acceptRejectDr = false;
            this._loadingCarriers = false;
            if (data.StatusCode != 1) {
                if (status == BrokeredDrCarrierStatus.Accepted) {
                    this.requestToUpdate.CarrierStatus = BrokeredDrCarrierStatus.Accepted;
                    //this.dataService.setRestoreDeletedRequestSubject([this.requestToUpdate]);
                    this.getDeliveryRequests(this.SelectedRegionId, this.SbForm.get('ObjectFilter').value, null, this.ScheduleBuilderFilters.Date)
                }
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                Declarations.hideModal('#proceedBrokeredDrModal');
                if (this.drFilterForm.get('IsFilterApplied').value) {
                    this.dataService.setDrQueueChangesForFilter(true);
                }
                //this.getBrokeredDrAssignedToMe(this.SelectedRegionId, this.ScheduleBuilderFilters.Date);
            } else {
                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
        });
    }

    public async changeCarrierBrokeredDrStatus(drId: any) {
        this._acceptRejectDr = true;
        let status = drId.status;
        let _id = drId.drId;
        let blendedGroupId = drId.blendedGroupId;
        await this.carrierService.changeBrokeredDrStatus(_id, blendedGroupId, status).subscribe(data => {
            this._acceptRejectDr = false;
            if (data.StatusCode == 0) {
                if (status == BrokeredDrCarrierStatus.Accepted) {
                    this.requestToUpdate.CarrierStatus = BrokeredDrCarrierStatus.Accepted;
                    this.getDeliveryRequests(this.SelectedRegionId, this.SbForm.get('ObjectFilter').value, null, this.ScheduleBuilderFilters.Date)
                }
                else {
                    this.requestToUpdate.CarrierStatus = BrokeredDrCarrierStatus.Rejected;
                }
                this.dataService.setAcceptRejectDRSubject({ 'RegionId': this.SelectedRegionId, 'Date': this.ScheduleBuilderFilters.Date });
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                Declarations.hideModal('#proceedBrokeredDrModal');
                if (this.drFilterForm.get('IsFilterApplied').value) {
                    this.dataService.setDrQueueChangesForFilter(true);
                }

            } else {
                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
        });
    }

    private getAllRequests() {
        let allDrs = [] as DeliveryRequestViewModel[];

        let missedReqs = this.missedRequests.map(item => item.DeliveryRequests).reduce((a, c) => a.concat(c), []);
        let mustGoReqs = this.mustGoRequests.map(item => item.DeliveryRequests).reduce((a, c) => a.concat(c), []);
        let shouldGoReqs = this.shouldGoRequests.map(item => item.DeliveryRequests).reduce((a, c) => a.concat(c), []);
        let couldGoReqs = this.couldGoRequests.map(item => item.DeliveryRequests).reduce((a, c) => a.concat(c), []);
        allDrs = [].concat(missedReqs, mustGoReqs, shouldGoReqs, couldGoReqs);
        return allDrs;
    }

    updateRequest(drs: DeliveryRequestViewModel[], status: number) {
        if (IsCarrierCompany) {
            drs.forEach(function (element, index) {
                if (element.CarrierStatus == 2) {
                    drs[index].IsBrokeredCarrierDR = true;
                }
            });
        }
        if (status == 0) {
            drs.forEach(function (element, index) {
                drs[index].WindowMode = 1;
                drs[index].QueueMode = 1;
            });
            return drs;
        }
        else {
            let pubWindowmode = this._pubWindowmode;
            if (pubWindowmode == 1) {
                var tempmustGoRequests = this.activeQueueList.filter(top => top.Priority == 1 && top.ParentId == null);
                var tempshouldGoRequests = this.activeQueueList.filter(top => top.Priority == 2 && top.ParentId == null);
                var tempcouldGoRequests = this.activeQueueList.filter(top => top.Priority == 3 && top.ParentId == null);
                var tempMissedRequests = this.activeQueueList.filter(top => top.ParentId != null);
                drs.forEach(function (element, index) {
                    if (tempmustGoRequests.length > 0 || tempshouldGoRequests.length > 0 || tempcouldGoRequests.length > 0 || tempMissedRequests.length > 0) {
                        var mustrecordFound = tempmustGoRequests.find(top => top.Id === element.Id);
                        var shouldrecordFound = tempshouldGoRequests.find(top => top.Id === element.Id);
                        var couldrecordFound = tempcouldGoRequests.find(top => top.Id === element.Id);
                        var missedrecordFound = tempMissedRequests.find(top => top.Id === element.Id);
                        if (missedrecordFound) {
                            drs[index].WindowMode = missedrecordFound.WindowMode;
                            drs[index].QueueMode = missedrecordFound.QueueMode;
                        }
                        else if (mustrecordFound) {
                            drs[index].WindowMode = mustrecordFound.WindowMode;
                            drs[index].QueueMode = mustrecordFound.QueueMode;
                        }
                        else if (shouldrecordFound) {
                            drs[index].WindowMode = shouldrecordFound.WindowMode;
                            drs[index].QueueMode = shouldrecordFound.QueueMode;
                        }
                        else if (couldrecordFound) {
                            drs[index].WindowMode = couldrecordFound.WindowMode;
                            drs[index].QueueMode = couldrecordFound.QueueMode;
                        }
                        else {
                            drs[index].WindowMode = 1;
                            drs[index].QueueMode = 1;
                        }
                    }
                    else {
                        drs[index].WindowMode = 1;
                        drs[index].QueueMode = 1;
                    }
                });
            }
            else {
                drs.forEach(function (element, index) {
                    drs[index].WindowMode = 2;
                    drs[index].QueueMode = 2;
                });
            }
            return drs;
        }
    }

    filterDrByScheduleBuilder(drs: DeliveryRequestViewModel[]): DeliveryRequestViewModel[] {

        var _scheduleRequests = [];
        var scheduleBuilder = this.SbForm;
        if (scheduleBuilder != undefined && scheduleBuilder != null) {
            scheduleBuilder.value.Shifts.forEach(s => {
                s.Schedules.forEach(sc => {
                    sc.Trips.forEach(t => {
                        t.DeliveryRequests.forEach(d => {
                            _scheduleRequests.push(d.Id);
                        });
                    });
                });
            });
            drs = drs.filter(x => {
                return _scheduleRequests.find(y => y == x.Id) == undefined;
            });
        }
        return drs;
    }

    toggledrRequestClick(status: boolean) {
        this.drRequestClick = status;
    }

    updateRecordsToggleRequest(status: number) {
        if (this._pubWindowmode == 1) {

            let draggedRequests = this.tempDraggedRequest.filter(t => t.ParentId == null && t.AssignedToCompanyId == currentUserCompanyId);
            let draggedmissedRequests = this.tempDraggedRequest.filter(t => t.ParentId != null && t.AssignedToCompanyId == currentUserCompanyId);
            var priorityRequests = [] as DeliveryRequestViewModel[];
            var missedRequests = [] as DeliveryRequestViewModel[];
            if (status == 1) {
                priorityRequests = this.deliveryRequests.filter(t => t.ParentId == null && t.WindowMode == 1 && t.QueueMode == 1);
                missedRequests = this.deliveryRequests.filter(top => top.WindowMode == 1 && top.QueueMode == 1 && top.ParentId != null);
            }
            else {
                priorityRequests = this.deliveryRequests.filter(t => t.ParentId == null && t.WindowMode == 1 && t.QueueMode == 2);
                missedRequests = this.deliveryRequests.filter(top => top.WindowMode == 1 && top.QueueMode == 2 && top.ParentId != null);
            }
            if (draggedRequests.length > 0) {
                priorityRequests = priorityRequests.filter(({ Id: id1 }) => !draggedRequests.some(({ Id: id2 }) => id2 === id1));
            }

            if (draggedmissedRequests.length > 0) {
                missedRequests = missedRequests.filter(({ Id: id1 }) => !draggedmissedRequests.some(({ Id: id2 }) => id2 === id1));
            }
            this.getRequestsByPriority(priorityRequests, missedRequests);
        }
        else {

            let draggedRequests = this.tempDraggedRequest.filter(t => t.AssignedToCompanyId == currentUserCompanyId && t.ParentId == null);
            let draggedmissedRequests = this.tempDraggedRequest.filter(t => t.AssignedToCompanyId == currentUserCompanyId && t.ParentId != null);

            //must go requests
            var mustGodeliveryRequests = JSON.parse(localStorage.getItem("mustGoDeliveryRequest")) as DelRequestsByJobModel[];
            if (mustGodeliveryRequests != null) {
                this.mustGoRequests = mustGodeliveryRequests;
            }
            if (draggedRequests.length > 0) {
                this.mustGoRequests.forEach(t => t.DeliveryRequests = t.DeliveryRequests.filter(({ Id: id1 }) => !draggedRequests.some(({ Id: id2 }) => id2 === id1)));
                this.mustGoRequests = this.mustGoRequests.filter(t => t.DeliveryRequests && t.DeliveryRequests.length > 0);
            }
            this.mustGoRequests.slice();

            //should go requests
            var shouldGodeliveryRequests = JSON.parse(localStorage.getItem("shouldGoDeliveryRequest")) as DelRequestsByJobModel[];
            if (shouldGodeliveryRequests != null) {
                this.shouldGoRequests = shouldGodeliveryRequests;
            }
            if (draggedRequests.length > 0) {
                this.shouldGoRequests.forEach(t => t.DeliveryRequests = t.DeliveryRequests.filter(({ Id: id1 }) => !draggedRequests.some(({ Id: id2 }) => id2 === id1)));
                this.shouldGoRequests = this.shouldGoRequests.filter(t => t.DeliveryRequests && t.DeliveryRequests.length > 0);
            }
            this.shouldGoRequests.slice();

            //could go records filters
            var couldGodeliveryRequests = JSON.parse(localStorage.getItem("couldGoDeliveryRequest")) as DelRequestsByJobModel[];
            if (couldGodeliveryRequests != null) {
                this.couldGoRequests = couldGodeliveryRequests;

            }
            if (draggedRequests.length > 0) {
                this.couldGoRequests.forEach(t => t.DeliveryRequests = t.DeliveryRequests.filter(({ Id: id1 }) => !draggedRequests.some(({ Id: id2 }) => id2 === id1)));
                this.couldGoRequests = this.couldGoRequests.filter(t => t.DeliveryRequests && t.DeliveryRequests.length > 0);
            }
            this.couldGoRequests.slice();

            var misseddeliveryRequests = JSON.parse(localStorage.getItem("missedDeliveryRequest")) as DelRequestsByJobModel[];
            if (misseddeliveryRequests != null) {
                this.missedRequests = misseddeliveryRequests;
            }
            if (draggedmissedRequests.length > 0) {
                this.missedRequests.forEach(t => t.DeliveryRequests = t.DeliveryRequests.filter(({ Id: id1 }) => !draggedmissedRequests.some(({ Id: id2 }) => id2 === id1)));
                this.missedRequests = this.missedRequests.filter(t => t.DeliveryRequests && t.DeliveryRequests.length > 0);
            }
            this.missedRequests.slice();
        }
    }

    getLocalStorageQueueData() {
        this._loadingRequests = true;
        let draggedRequests = this.tempDraggedRequest.filter(t => t.AssignedToCompanyId == currentUserCompanyId);
        var missedLocations = JSON.parse(localStorage.getItem("missedDeliveryRequest")) as DelRequestsByJobModel[] || [];
        var mustGoLocations = JSON.parse(localStorage.getItem("mustGoDeliveryRequest")) as DelRequestsByJobModel[] || [];
        var shouldGoLocations = JSON.parse(localStorage.getItem("shouldGoDeliveryRequest")) as DelRequestsByJobModel[] || [];
        var couldGoLocations = JSON.parse(localStorage.getItem("couldGoDeliveryRequest")) as DelRequestsByJobModel[] || [];
        if (draggedRequests && draggedRequests.length > 0) {
            let deliveryRequests = [] as DeliveryRequestViewModel[];

            let missedReqs = missedLocations.map(item => item.DeliveryRequests).reduce((a, c) => a.concat(c), []);
            let mustGoReqs = mustGoLocations.map(item => item.DeliveryRequests).reduce((a, c) => a.concat(c), []);
            let shouldGoReqs = shouldGoLocations.map(item => item.DeliveryRequests).reduce((a, c) => a.concat(c), []);
            let couldGoReqs = couldGoLocations.map(item => item.DeliveryRequests).reduce((a, c) => a.concat(c), []);
            deliveryRequests = [].concat(missedReqs, mustGoReqs, shouldGoReqs, couldGoReqs);

            deliveryRequests = deliveryRequests.filter(({ Id: id1 }) => !draggedRequests.some(({ Id: id2 }) => id2 === id1));

            var priorityRequests = deliveryRequests.filter(t => t.ParentId == null);
            var missedRequests = deliveryRequests.filter(t => t.ParentId != null);
            this.getRequestsByPriority(priorityRequests, missedRequests);
        }
        else {
            this.mustGoRequests = mustGoLocations;
            this.mustGoRequests.slice();
            this.shouldGoRequests = shouldGoLocations;
            this.shouldGoRequests.slice();
            this.couldGoRequests = couldGoLocations;
            this.couldGoRequests.slice();
            this.missedRequests = missedLocations;
            this.missedRequests.slice();
        }
        this.localmustGoRequests = this.mustGoRequests;
        this.localmustGoRequests.slice();  //refresh the data
        this._mustGoRefresh = this.mustGoRequests.length;

        this.localshouldGoRequests = this.shouldGoRequests;
        this.localshouldGoRequests.slice();//refresh the data
        this._shouldGoRefresh = this.shouldGoRequests.length;

        this.localcouldGoRequests = this.couldGoRequests;
        this.localcouldGoRequests.slice();//refresh the data
        this._couldGoRefresh = this.couldGoRequests.length;

        this.localMissedRequests = this.missedRequests;
        this.localMissedRequests.slice();//refresh the data
        this._missedRefresh = this.missedRequests.length;

        localStorage.setItem("refreshLocalStorage", 'false');
        this._loadingRequests = false;

        if (this.drFilterForm.get('IsFilterApplied').value) {
            this.dataService.setDrQueueChangesForFilter(true);
        }
    }

    activeTimer() {
        if (this.intervalTime) {
            clearInterval(this.intervalTime);
        }
        this.intervalTime = setInterval(() => {
            if (IsUserActive()) {
                if (this._pubWindowmode == 2) {
                    const refreshLocalStorage = localStorage.getItem("refreshLocalStorage");

                    if (refreshLocalStorage != null && refreshLocalStorage == 'true') {
                        this.getLocalStorageQueueData();
                    }
                }
            }
        }, 3000);
    }

    activeQueueIcon() {
        this.intervalTimeQueue = setInterval(() => {

            if (IsUserActive()) {
                this._refreshQueue = false;
                if (this._pubWindowmode == 1) {
                    var deliveryRequests = this.deliveryRequests;
                    if (this.tempDraggedRequest.length > 0) {
                        deliveryRequests = deliveryRequests.filter(({ Id: id1 }) => !this.tempDraggedRequest.some(({ Id: id2 }) => id2 === id1));
                    }
                    if (deliveryRequests.filter(top => top.QueueMode == 2).length > 0) {
                        this._refreshQueue = true;
                    }
                }
                else {
                    if (this.mustGoRequests.filter(jobObj => jobObj.DeliveryRequests && jobObj.DeliveryRequests.findIndex(t1 => t1.QueueMode == 2) != -1).length > 0) {
                        this._refreshQueue = true;
                    }
                    if (this.shouldGoRequests.filter(jobObj => jobObj.DeliveryRequests && jobObj.DeliveryRequests.findIndex(t1 => t1.QueueMode == 2) != -1).length > 0) {
                        this._refreshQueue = true;
                    }
                    if (this.couldGoRequests.filter(jobObj => jobObj.DeliveryRequests && jobObj.DeliveryRequests.findIndex(t1 => t1.QueueMode == 2) != -1).length > 0) {
                        this._refreshQueue = true;
                    }
                    if (this.missedRequests.filter(jobObj => jobObj.DeliveryRequests && jobObj.DeliveryRequests.findIndex(t1 => t1.QueueMode == 2) != -1).length > 0) {
                        this._refreshQueue = true;
                    }
                }
            }
        }, 3000);
    }

    onRedirectMultiWindowScreen(filter: QueueFilter): void {
        this.clickMultiRequest = true;
        this.ScheduleBuilderFilters.ToggleRequestMode = 2;
        this.setQueueFilter(this.ScheduleBuilderFilters.ToggleRequestMode);
        this.updateRecordsToggleRequest(2);
        if (this.ScheduleBuilderFilters.WindowMode === 2 && filter === 1) {
            this.deliveryRequestService.ToggleQueueIcon(false);
            localStorage.setItem("refreshLocalStorage", 'true');
            this.activeTimer();
            window.open("/Carrier/ScheduleBuilder/DeliveryRequests?regionId=" + this.SbForm.get('RegionId').value + '&selectedDate=' + this.SbForm.controls['Date'].value, "_blank");
        }
    }


    searchRequests(terms: string, queueMode: number, windowMode: number) {

        //must should could go requests
        let dragged_must_should_could = this.tempDraggedRequest.filter(t => t.AssignedToCompanyId == currentUserCompanyId);
        let searchedRecords = this.drFilterService.searchRequestsWithParams(this.deliveryRequests, terms, queueMode, windowMode, DeliveryRequestTypes.MustGo);

        if (dragged_must_should_could.length > 0) {
            searchedRecords = searchedRecords.filter(({ Id: id1 }) => !dragged_must_should_could.some(({ Id: id2 }) => id2 === id1));
        }

        //missed requests
        let draggedMissedRequests = this.tempDraggedRequest.filter(t => t.ParentId != null && t.AssignedToCompanyId == currentUserCompanyId);
        let missedrecords = this.drFilterService.searchRequestsWithParams(this.deliveryRequests, terms, queueMode, windowMode, DeliveryRequestTypes.Missed);

        if (draggedMissedRequests.length > 0) {
            missedrecords = missedrecords.filter(({ Id: id1 }) => !draggedMissedRequests.some(({ Id: id2 }) => id2 === id1));
        }


        if (this.drFilterForm.get('IsFilterApplied').value) {
            this.drFilterModel = this.drFilterService.drFilterFormToModel(this.drFilterForm, IsCarrierCompany);
            searchedRecords = this.drFilterService.applyFilterToDrs(searchedRecords, this.drFilterModel);
            missedrecords = this.drFilterService.applyFilterToDrs(missedrecords, this.drFilterModel);
        }

        let groupedDrs = this.deliveryReqService.groupDrsByJob(searchedRecords);

        this.mustGoRequests = this.deliveryReqService.getMustGoRequests(groupedDrs);
        this.shouldGoRequests = this.deliveryReqService.getShouldGoRequests(groupedDrs);
        this.couldGoRequests = this.deliveryReqService.getCouldGoRequests(groupedDrs);

        this.missedRequests = this.deliveryReqService.groupDrsByJob(missedrecords, DeliveryRequestTypes.Missed);

    }
    filterRequests(queueMode: number, windowMode: number) {

        this._loadingDrRequests = true;
        let _mustShoulCouldGoRecords = this.deliveryRequests.filter((dr: DeliveryRequestViewModel) => (
            dr.WindowMode == windowMode && dr.QueueMode === queueMode && dr.ParentId == null)
        );

        let _draggedRequests = this.tempDraggedRequest.filter(
            t => t.ParentId == null && t.AssignedToCompanyId == currentUserCompanyId
        );

        if (_draggedRequests.length > 0) {
            _mustShoulCouldGoRecords = _mustShoulCouldGoRecords.filter(({ Id: id1 }) => !_draggedRequests.some(({ Id: id2 }) => id2 === id1));
        }

        let _filteredMustShoulCouldGoRecords = this.drFilterService.applyFilterToDrs(_mustShoulCouldGoRecords, this.drFilterModel);
        if (IsCarrierCompany) {
            if (this.drFilterModel.IsBrokeredDRs) {
                _filteredMustShoulCouldGoRecords = _filteredMustShoulCouldGoRecords.filter(x => x.IsBrokeredCarrierDR == true);
            }
        }
        if (this.drFilterModel.IsTBDRequest == true) {
            _filteredMustShoulCouldGoRecords = _filteredMustShoulCouldGoRecords.filter(t => t.IsTBD == true);
        }
        //filter records based on Order Type.
        if (this.drFilterModel.OrderType.length > 1) {
            if (this.drFilterModel.OrderType.includes(OrderType.FTL && OrderType.LTL)) {
                _filteredMustShoulCouldGoRecords = _filteredMustShoulCouldGoRecords.filter(t => t.OrderType == OrderType.FTL || t.OrderType == OrderType.LTL);
            }
        }
        else if (this.drFilterModel.OrderType.includes(OrderType.LTL)) {
            _filteredMustShoulCouldGoRecords = _filteredMustShoulCouldGoRecords.filter(t => t.OrderType == OrderType.LTL);
        }
        else if (this.drFilterModel.OrderType.includes(OrderType.FTL)) {
            _filteredMustShoulCouldGoRecords = _filteredMustShoulCouldGoRecords.filter(t => t.OrderType == OrderType.FTL);
        }
        let groupedDrs = this.deliveryReqService.groupDrsByJob(_filteredMustShoulCouldGoRecords)

        if (this.drFilterModel.Priority.includes(DeliveryRequestTypes.MustGo)) {
            this.mustGoRequests = this.deliveryReqService.getMustGoRequests(groupedDrs);
        }
        if (this.drFilterModel.Priority.includes(DeliveryRequestTypes.ShouldGo)) {
            this.shouldGoRequests = this.deliveryReqService.getShouldGoRequests(groupedDrs);
        }
        if (this.drFilterModel.Priority.includes(DeliveryRequestTypes.CouldGo)) {
            this.couldGoRequests = this.deliveryReqService.getCouldGoRequests(groupedDrs);
        }
        if (this.drFilterModel.Priority.includes(DeliveryRequestTypes.Missed)) {

            let draggedRequests = this.tempDraggedRequest.filter(t => t.ParentId != null && t.AssignedToCompanyId == currentUserCompanyId);

            let _missedDrs = this.deliveryRequests.filter(dr => (dr.WindowMode == windowMode && dr.QueueMode === queueMode && dr.ParentId != null));
            if (IsCarrierCompany) {
                if (this.drFilterModel.IsBrokeredDRs) {
                    _missedDrs = _missedDrs.filter(x => x.IsBrokeredCarrierDR == true);
                }
            }
            if (this.drFilterModel.IsTBDRequest == true) {
                _missedDrs = _missedDrs.filter(x => x.IsTBD == true);
            }
            if (draggedRequests.length > 0) {
                _missedDrs = _missedDrs.filter(({ Id: id1 }) => !draggedRequests.some(({ Id: id2 }) => id2 === id1));
            }

            _missedDrs = this.drFilterService.applyFilterToDrs(_missedDrs, this.drFilterModel);

            //filter records based on Order Type.
            if (_missedDrs.length > 0) {
                if (this.drFilterModel.OrderType.length > 1) {
                    if (this.drFilterModel.OrderType.includes(OrderType.FTL && OrderType.LTL)) {
                        _missedDrs = _missedDrs.filter(t => t.OrderType == OrderType.FTL || t.OrderType == OrderType.LTL);
                    }
                }
                else if (this.drFilterModel.OrderType.includes(OrderType.LTL)) {
                    _missedDrs = _missedDrs.filter(t => t.OrderType == OrderType.LTL);
                }
                else if (this.drFilterModel.OrderType.includes(OrderType.FTL)) {
                    _missedDrs = _missedDrs.filter(t => t.OrderType == OrderType.FTL);
                }
            }
            this.missedRequests = this.deliveryReqService.groupDrsByJob(_missedDrs, DeliveryRequestTypes.Missed);
        }
        this._loadingDrRequests = false;
    }

    searchMultiWindowRequests(terms: string, queueMode: number, windowMode: number) {
        let dragged_requests = this.tempDraggedRequest.filter(t => t.AssignedToCompanyId == currentUserCompanyId);

        let _localmustGoRequests = this.drFilterService.getDrsFromJob(this.localmustGoRequests);
        let _localShouldGoRequests = this.drFilterService.getDrsFromJob(this.localshouldGoRequests);
        let _localCouldGoRequests = this.drFilterService.getDrsFromJob(this.localcouldGoRequests);
        let _localMissedDr = this.drFilterService.getDrsFromJob(this.localMissedRequests);

        let mustGorecords = this.drFilterService.searchRequests(_localmustGoRequests, terms);
        let shouldGorecords = this.drFilterService.searchRequests(_localShouldGoRequests, terms);
        let couldGorecords = this.drFilterService.searchRequests(_localCouldGoRequests, terms);
        let missedrecords = this.drFilterService.searchRequests(_localMissedDr, terms);


        if (dragged_requests.length > 0) {
            mustGorecords = mustGorecords.filter(({ Id: id1 }) => !dragged_requests.some(({ Id: id2 }) => id2 === id1));
            shouldGorecords = shouldGorecords.filter(({ Id: id1 }) => !dragged_requests.some(({ Id: id2 }) => id2 === id1));
            couldGorecords = couldGorecords.filter(({ Id: id1 }) => !dragged_requests.some(({ Id: id2 }) => id2 === id1));
            missedrecords = missedrecords.filter(({ Id: id1 }) => !dragged_requests.some(({ Id: id2 }) => id2 === id1));
        }

        let mustGoGroupedDrs = this.deliveryReqService.groupDrsByJob(mustGorecords);
        this.mustGoRequests = this.deliveryReqService.getMustGoRequests(mustGoGroupedDrs);

        let shouldGoGroupedDrs = this.deliveryReqService.groupDrsByJob(shouldGorecords);
        this.mustGoRequests = this.deliveryReqService.getShouldGoRequests(shouldGoGroupedDrs);

        let couldGoGroupedDrs = this.deliveryReqService.groupDrsByJob(couldGorecords);
        this.mustGoRequests = this.deliveryReqService.getCouldGoRequests(couldGoGroupedDrs);

        this.missedRequests = this.deliveryReqService.groupDrsByJob(missedrecords, DeliveryRequestTypes.Missed);
    }


    filterMultiWindowRequests() {

        if (this.drFilterModel.Priority.includes(DeliveryRequestTypes.MustGo)) {

            let _draggedRequests = this.tempDraggedRequest.filter(t =>
                t.Priority == 1 && t.ParentId == null && t.AssignedToCompanyId == currentUserCompanyId
            );

            let _localmustGoRequests = this.drFilterService.getDrsFromJob(this.localmustGoRequests);

            if (_draggedRequests.length > 0) {
                _localmustGoRequests = _localmustGoRequests.filter(({ Id: id1 }) => !_draggedRequests.some(({ Id: id2 }) => id2 === id1));
            }

            _localmustGoRequests = this.drFilterService.applyFilterToDrs(_localmustGoRequests, this.drFilterModel);

            if (IsCarrierCompany) {
                if (this.drFilterModel.IsBrokeredDRs) {
                    _localmustGoRequests = _localmustGoRequests.filter(x => x.IsBrokeredCarrierDR == true);
                }
            }
            if (this.drFilterModel.IsTBDRequest == true) {
                _localmustGoRequests = _localmustGoRequests.filter(x => x.IsTBD == true);
            }
            let groupedDrs = this.deliveryReqService.groupDrsByJob(_localmustGoRequests);
            this.mustGoRequests = this.deliveryReqService.getMustGoRequests(groupedDrs);

        }
        if (this.drFilterModel.Priority.includes(DeliveryRequestTypes.ShouldGo)) {

            let _draggedRequests = this.tempDraggedRequest.filter(
                t => t.Priority == 2 && t.ParentId == null && t.AssignedToCompanyId == currentUserCompanyId
            );

            let _localshouldGoRequests = this.drFilterService.getDrsFromJob(this.localshouldGoRequests);

            if (IsCarrierCompany) {
                if (this.drFilterModel.IsBrokeredDRs) {
                    _localshouldGoRequests = _localshouldGoRequests.filter(x => x.IsBrokeredCarrierDR == true);
                }
            }
            if (this.drFilterModel.IsTBDRequest == true) {
                _localshouldGoRequests = _localshouldGoRequests.filter(x => x.IsTBD == true);
            }
            if (_draggedRequests.length > 0) {
                _localshouldGoRequests = _localshouldGoRequests.filter(({ Id: id1 }) => !_draggedRequests.some(({ Id: id2 }) => id2 === id1));
            }

            _localshouldGoRequests = this.drFilterService.applyFilterToDrs(_localshouldGoRequests, this.drFilterModel);

            let groupedDrs = this.deliveryReqService.groupDrsByJob(_localshouldGoRequests);
            this.shouldGoRequests = this.deliveryReqService.getShouldGoRequests(groupedDrs);
        }
        if (this.drFilterModel.Priority.includes(DeliveryRequestTypes.CouldGo)) {

            let _draggedRequests = this.tempDraggedRequest.filter(
                t => t.Priority == 3 && t.ParentId == null && t.AssignedToCompanyId == currentUserCompanyId
            );

            let _localcouldGoRequests = this.drFilterService.getDrsFromJob(this.localcouldGoRequests);
            if (IsCarrierCompany) {
                if (this.drFilterModel.IsBrokeredDRs) {
                    _localcouldGoRequests = _localcouldGoRequests.filter(x => x.IsBrokeredCarrierDR == true);
                }
            }
            if (this.drFilterModel.IsTBDRequest == true) {
                _localcouldGoRequests = _localcouldGoRequests.filter(x => x.IsTBD == true);
            }
            if (_draggedRequests.length > 0) {
                _localcouldGoRequests = _localcouldGoRequests.filter(({ Id: id1 }) => !_draggedRequests.some(({ Id: id2 }) => id2 === id1));
            }

            _localcouldGoRequests = this.drFilterService.applyFilterToDrs(_localcouldGoRequests, this.drFilterModel);

            let groupedDrs = this.deliveryReqService.groupDrsByJob(_localcouldGoRequests);
            this.couldGoRequests = this.deliveryReqService.getCouldGoRequests(groupedDrs);
        }
        if (this.drFilterModel.Priority.includes(DeliveryRequestTypes.Missed)) {

            let _draggedMissedRequests = this.tempDraggedRequest.filter(
                t => t.ParentId != null && t.AssignedToCompanyId == currentUserCompanyId
            );

            let _missedrecords = this.drFilterService.getDrsFromJob(this.localMissedRequests);
            if (IsCarrierCompany) {
                if (this.drFilterModel.IsBrokeredDRs) {
                    _missedrecords = _missedrecords.filter(x => x.IsBrokeredCarrierDR == true);
                }
            }
            if (this.drFilterModel.IsTBDRequest == true) {
                _missedrecords = _missedrecords.filter(x => x.IsTBD == true);
            }
            if (_draggedMissedRequests.length > 0) {
                _missedrecords = _missedrecords.filter(({ Id: id1 }) => !_draggedMissedRequests.some(({ Id: id2 }) => id2 === id1));
            }

            _missedrecords = this.drFilterService.applyFilterToDrs(_missedrecords, this.drFilterModel);

            this.missedRequests = this.deliveryReqService.groupDrsByJob(_missedrecords, DeliveryRequestTypes.Missed);
        }
    }


    // searchAssignedTabDr(terms: string) {

    // }
    // assignAssignedTabDrs() {

    // }

    RedirectToCreateTrailer() {
        window.location.href = "/Carrier/Tractor/View";
    }
    RedirectToCreateRegion() {
        window.location.href = "/Supplier/Region/View";
    }

    MoveActiveQueue(dr: DeliveryRequestViewModel[]): void {
        if (dr != null) {
            this.updateDrRequest(dr.map(t => t.Id));
            this.deliveryRequests.slice(); // refresh the delivery request.
            this.assignTempRequest(1, 1);
            Declarations.msgsuccess(dr[0].CustomerCompany + " moved to clipboard.", undefined, undefined);
            this._refreshQueue = true;
        }
    }

    updateDrRequest(drID: string[]) {
        let deliveryRequest = this.deliveryRequests.filter(t => drID.some(t1 => t1 == t.Id)) as DeliveryRequestViewModel[];
        if (deliveryRequest != null) {
            deliveryRequest.forEach(t => {
                t.WindowMode = 1;
                t.QueueMode = 2;
                this.activeQueueList.push(t);
                var foundIndex = this.deliveryRequests.findIndex(x => x.Id == t.Id);
                this.deliveryRequests[foundIndex] = t;
            });
        }
    }

    updateDrRequestMultiScreenMode(drArray: DeliveryRequestViewModel[]) {
        drArray.forEach(t => {
            let foundIndex = this.deliveryRequests.findIndex(x => x.Id == t.Id);
            if (foundIndex != -1) {
                this.deliveryRequests[foundIndex].WindowMode = 1;
                this.deliveryRequests[foundIndex].QueueMode = 2;
            }
        });
    }

    refreshLeftPanelData() {
        var toggleRequestMode = this.SbForm.get('ToggleRequestMode').value;
        if (this._pubWindowmode == 1) {
            if (toggleRequestMode == 1) {
                this.mustGoRequests.forEach(t => t.DeliveryRequests = t.DeliveryRequests.filter(top => top.QueueMode === 1 && top.WindowMode === 1));
                this.mustGoRequests.slice();

                this.shouldGoRequests.forEach(t => t.DeliveryRequests = t.DeliveryRequests.filter(top => top.QueueMode === 1 && top.WindowMode === 1));
                this.shouldGoRequests.slice();

                this.couldGoRequests.forEach(t => t.DeliveryRequests = t.DeliveryRequests.filter(top => top.QueueMode === 1 && top.WindowMode === 1));
                this.couldGoRequests.slice();

                this.missedRequests.forEach(t => t.DeliveryRequests = t.DeliveryRequests.filter(top => top.QueueMode === 1 && top.WindowMode === 1));
                this.missedRequests.slice();
            }
        }
    }

    refreshSingleWindow() {
        var status = false;
        if (localStorage.getItem("missedDeliveryRequest") != null) {
            var locations = JSON.parse(localStorage.getItem("missedDeliveryRequest")) as DelRequestsByJobModel[] || [];
            if (locations.length > 0)
                status = true;
            locations.forEach(x => {
                this.updateDrRequestMultiScreenMode(x.DeliveryRequests);
            });
        }
        if (localStorage.getItem("mustGoDeliveryRequest") != null) {
            var locations = JSON.parse(localStorage.getItem("mustGoDeliveryRequest")) as DelRequestsByJobModel[] || [];
            if (locations.length > 0)
                status = true;
            locations.forEach(x => {
                this.updateDrRequestMultiScreenMode(x.DeliveryRequests);
            });
        }
        if (localStorage.getItem("shouldGoDeliveryRequest") != null) {
            var locations = JSON.parse(localStorage.getItem("shouldGoDeliveryRequest")) as DelRequestsByJobModel[] || [];
            if (locations.length > 0)
                status = true;

            locations.forEach(x => {
                this.updateDrRequestMultiScreenMode(x.DeliveryRequests);
            });
        }
        if (localStorage.getItem("couldGoDeliveryRequest") != null) {
            var locations = JSON.parse(localStorage.getItem("couldGoDeliveryRequest")) as DelRequestsByJobModel[];
            if (locations.length > 0)
                status = true;
            locations.forEach(x => {
                this.updateDrRequestMultiScreenMode(x.DeliveryRequests);
            });
        };
        if (status) {
            Declarations.msgwarning("Due to window mode change all items are moved to clipboard from the session.", undefined, undefined);
        }
        this.resetLocalStorage();
        this.resetTimer();
        this.onQueueFilterChange(1);
        localStorage.setItem("refreshRegion", JSON.stringify(true));
    }


    getLocalStorageRequests() {
        var missedLocations = JSON.parse(localStorage.getItem("missedDeliveryRequest")) as DelRequestsByJobModel[] || [];
        var mustGoLocations = JSON.parse(localStorage.getItem("mustGoDeliveryRequest")) as DelRequestsByJobModel[] || [];
        var shouldGoLocations = JSON.parse(localStorage.getItem("shouldGoDeliveryRequest")) as DelRequestsByJobModel[] || [];
        var couldGoLocations = JSON.parse(localStorage.getItem("couldGoDeliveryRequest")) as DelRequestsByJobModel[] || [];

        let deliveryRequests = [] as DeliveryRequestViewModel[];

        let missedReqs = missedLocations.map(item => item.DeliveryRequests).reduce((a, c) => a.concat(c), []);
        let mustGoReqs = mustGoLocations.map(item => item.DeliveryRequests).reduce((a, c) => a.concat(c), []);
        let shouldGoReqs = shouldGoLocations.map(item => item.DeliveryRequests).reduce((a, c) => a.concat(c), []);
        let couldGoReqs = couldGoLocations.map(item => item.DeliveryRequests).reduce((a, c) => a.concat(c), []);
        deliveryRequests = [].concat(missedReqs, mustGoReqs, shouldGoReqs, couldGoReqs);

        return deliveryRequests;
    }

    refreshUpdateRecordMultiwindow(requestToUpdate: DeliveryRequestViewModel, status: number) {
        let draggedRequests = this.tempDraggedRequest.filter(t => t.AssignedToCompanyId == currentUserCompanyId);
        if (requestToUpdate != null) {
            var deliveryRequests = this.getLocalStorageRequests();
            var updatedDrIndex = deliveryRequests.findIndex(t => t.Id == requestToUpdate.Id);
            if (updatedDrIndex != -1) {
                if (status == 1) {
                    deliveryRequests[updatedDrIndex] = requestToUpdate;
                    localStorage.setItem("recordPriorityChange", JSON.stringify(requestToUpdate));
                }
                else {
                    deliveryRequests.splice(updatedDrIndex, 1);
                }
            }
            if (draggedRequests && draggedRequests.length > 0) {
                deliveryRequests = deliveryRequests.filter(({ Id: id1 }) => !draggedRequests.some(({ Id: id2 }) => id2 === id1));
            }
            var priorityRequests = deliveryRequests.filter(t => t.ParentId == null);
            var missedRequests = deliveryRequests.filter(t => t.ParentId != null);
            this.getRequestsByPriority(priorityRequests, missedRequests);

            localStorage.setItem("missedDeliveryRequest", JSON.stringify(this.missedRequests));
            localStorage.setItem("mustGoDeliveryRequest", JSON.stringify(this.mustGoRequests));
            localStorage.setItem("shouldGoDeliveryRequest", JSON.stringify(this.shouldGoRequests));
            localStorage.setItem("couldGoDeliveryRequest", JSON.stringify(this.couldGoRequests));
            localStorage.setItem("updateRequest", JSON.stringify(true));
        }
    }

    getRequestsByPriority(priorityReqs: DeliveryRequestViewModel[], missedReqs: DeliveryRequestViewModel[]) {
        var groupedDrs = this.deliveryReqService.groupDrsByJob(priorityReqs);
        this.mustGoRequests = this.deliveryReqService.getMustGoRequests(groupedDrs);
        this.mustGoRequests.slice();
        this.shouldGoRequests = this.deliveryReqService.getShouldGoRequests(groupedDrs);
        this.shouldGoRequests.slice();
        this.couldGoRequests = this.deliveryReqService.getCouldGoRequests(groupedDrs);
        this.couldGoRequests.slice();
        this.missedRequests = this.deliveryReqService.groupDrsByJob(missedReqs, DeliveryRequestTypes.Missed);
        this.missedRequests.slice();

        if (this.drFilterForm.get('IsFilterApplied').value) {
            this.dataService.setDrQueueChangesForFilter(true);
        }
    }

    async onApplyFilter($event) {
        if ($event.SelectedDrivers == undefined)
            $event.SelectedDrivers = [];
        if ($event.SelectedTrailers == undefined)
            $event.SelectedTrailers = [];
        if ($event.SelectedPickups == undefined)
            $event.SelectedPickups = [];

        let sbViewId = this.SbForm.get('ObjectFilter').value;
        if (sbViewId == 1) {
            // driver view
            await this.filterDriverView($event)
        }
        else if (sbViewId == 2) {
            // trailer view
            await this.filterTrailerView($event);
        }
    }

    setSbFilter() {
        if (this.RegionDetail == undefined || this.RegionDetail == null) {
            return;
        }

        this.UpdateSbFilter = false;
        this.SbFilter.Drivers = this.RegionDetail.Drivers;
        this.SbFilter.Trailers = this.RegionDetail.Trailers;
        this.SbFilter.Pickups = this.getPickups();
        this.UpdateSbFilter = true;
    }

    resetSbFilter() {
        this.UpdateSbFilter = false;

        this.DriverViewFilter = null;
        this.TrailerViewFilter = null;
        this.SbFilter.SelectedDrivers = [];
        this.SbFilter.SelectedTrailers = [];
        this.SbFilter.SelectedPickups = [];

        this.UpdateSbFilter = true;
    }

    getPickups() {
        var pickups: DropdownItem[] = [];
        if (this.ScheduleBuilder.ObjectFilter == 1) {
            // driver view
            if (this.DriverViewShifts != undefined && this.DriverViewShifts != null && this.DriverViewShifts.length > 0) {
                this.DriverViewShifts.map((shift, shiftIdx) => {
                    shift.Schedules.map((schedule, scheduleIdx) => {
                        schedule.Trips.map((trip, tripIdx) => {
                            this.addPickupLocation(pickups, trip);
                        });
                    });
                });
            }
        }
        else {
            // trailer view
            if (this.TrailerViewTrailers != undefined && this.TrailerViewTrailers != null && this.TrailerViewTrailers.length > 0) {
                this.TrailerViewTrailers.map((trailer, trailerIdx) => {
                    trailer.Shifts.map((shift, shiftIdx) => {
                        shift.Trips.map((trip, tripIdx) => {
                            this.addPickupLocation(pickups, trip);
                        });
                    });
                });
            }
        }
        return pickups;
    }

    addPickupLocation(pickups: DropdownItem[], trip: TripModel) {
        if (trip.IsCommonPickup) {
            if (trip.PickupLocationType == PickupLocationType.BulkPlant) {
                this.addBulkplantAddress(pickups, trip.BulkPlant);
            }
            else if (trip.PickupLocationType == PickupLocationType.Terminal) {
                this.addTerminalAddress(pickups, trip.Terminal);
            }
        }
        else {
            trip.DeliveryRequests.map((dr, drIdx) => {
                if (dr.PickupLocationType == PickupLocationType.BulkPlant) {
                    this.addBulkplantAddress(pickups, dr.BulkPlant);
                }
                else if (dr.PickupLocationType == PickupLocationType.Terminal) {
                    this.addTerminalAddress(pickups, dr.Terminal);
                }
            });
        }
    }

    addBulkplantAddress(pickups: DropdownItem[], bulkplant: DropAddressModel) {
        var bulkplantTxt = 'Bulkplant: ';
        var siteName = bulkplant.SiteName.replace(' ', '');

        var pickup = pickups.filter(p => p.Code == siteName && p.Name.search(bulkplantTxt) != -1);
        if (pickup.length == 0 && bulkplant.SiteName != undefined && bulkplant.SiteName != null) {
            pickups.push({ "Id": 0, "Code": siteName, "Name": bulkplantTxt + bulkplant.Address + "," + bulkplant.City + "," + bulkplant.State.Code + "," + bulkplant.ZipCode });
        }
    }

    addTerminalAddress(pickups: DropdownItem[], terminal: DropdownItem) {
        var terminalTxt = 'Terminal: ';
        var pickup = pickups.filter(p => p.Code == terminal.Id.toString() && p.Name.search(terminalTxt) != -1);
        if (pickup.length == 0) {
            pickups.push({ "Id": 0, "Code": terminal.Id.toString(), "Name": terminalTxt + terminal.Name });
        }

    }

    async filterDriverView($event) {
        var shifts = this.SbForm.get('Shifts').value;
        var shiftIndex = 0;
        this.DriverViewFilter = new DriverViewFilterModel;
        for (let shift of shifts) {
            var schedules = shift.Schedules;
            await schedules != undefined && schedules != null &&
                schedules.map((sche, scheduleIdx) => {
                    var isPickup = false;
                    if ($event.SelectedPickups.length > 0) {
                        var trips = sche.Trips;
                        trips != undefined && trips != null &&
                            trips.map((trip, tripIdx) => {
                                var code = "";
                                var isExit = false;
                                if (!isExit) {
                                    if (trip.IsCommonPickup) {
                                        if (trip.PickupLocationType == PickupLocationType.BulkPlant) {
                                            code = trip.BulkPlant.SiteName;
                                        }
                                        else if (trip.PickupLocationType == PickupLocationType.Terminal) {
                                            code = trip.Terminal.Id.toString();
                                        }
                                    }
                                    else {
                                        trip.DeliveryRequests.map((dr, drIdx) => {
                                            if (dr.PickupLocationType == PickupLocationType.BulkPlant) {
                                                code = dr.BulkPlant.SiteName;
                                            }
                                            else if (dr.PickupLocationType == PickupLocationType.Terminal) {
                                                code = dr.Terminal.Id.toString();
                                            }
                                        });
                                    }
                                    if (code != undefined && code != "") {
                                        var pickup = $event.SelectedPickups.some(pick => pick.Code == code.replace(' ', ''));
                                        if (pickup) {
                                            isExit = true;
                                            isPickup = true;
                                        }
                                    }
                                }
                            });
                    }

                    var isDriver = sche.Drivers.filter(d => $event.SelectedDrivers.some(driver => driver.Id == d.Id)).length > 0;
                    var isTrailer = sche.Trailers.filter(t => $event.SelectedTrailers.some(trailer => trailer.Id == t.Id)).length > 0;
                    var isShowShift = false;
                    if ($event.SelectedDrivers.length > 0 && $event.SelectedTrailers.length > 0 && $event.SelectedPickups.length > 0) {
                        if (isDriver && isTrailer && isPickup) {
                            isShowShift = true;
                        }
                    }
                    else if ($event.SelectedDrivers.length > 0 && $event.SelectedTrailers.length == 0 && $event.SelectedPickups.length == 0) {
                        if (isDriver) {
                            isShowShift = true;
                        }
                    }
                    else if ($event.SelectedDrivers.length == 0 && $event.SelectedTrailers.length > 0 && $event.SelectedPickups.length == 0) {
                        if (isTrailer) {
                            isShowShift = true;
                        }
                    }
                    else if ($event.SelectedDrivers.length == 0 && $event.SelectedTrailers.length == 0 && $event.SelectedPickups.length > 0) {
                        if (isPickup) {
                            isShowShift = true;
                        }
                    }
                    else if ($event.SelectedDrivers.length > 0 && $event.SelectedTrailers.length > 0 && $event.SelectedPickups.length == 0) {
                        if (isDriver && isTrailer) {
                            isShowShift = true;
                        }
                    }
                    else if ($event.SelectedDrivers.length == 0 && $event.SelectedTrailers.length > 0 && $event.SelectedPickups.length > 0) {
                        if (isTrailer && isPickup) {
                            isShowShift = true;
                        }
                    }
                    else if ($event.SelectedDrivers.length > 0 && $event.SelectedTrailers.length == 0 && $event.SelectedPickups.length > 0) {
                        if (isDriver && isPickup) {
                            isShowShift = true;
                        }
                    }
                    else if ($event.SelectedDrivers.length == 0 && $event.SelectedTrailers.length == 0 && $event.SelectedPickups.length == 0) {
                        isShowShift = true;
                    }

                    if (isShowShift) {
                        this.DriverViewFilter.Shifts[shiftIndex + "-" + scheduleIdx] = true;
                    }
                    else {
                        this.DriverViewFilter.Shifts[shiftIndex + "-" + scheduleIdx] = false;
                    }
                });
            shiftIndex++;
        }
    }

    async filterTrailerView($event) {
        this.TrailerViewFilter = new TrailerViewFilterModel;
        var trailers = this.SbForm.get('Trailers').value;
        if (trailers == null || trailers == undefined || trailers.length == 0) {
            return;
        }
        if ($event.SelectedTrailers.length == 0 && $event.SelectedDrivers.length == 0 && $event.SelectedPickups.length == 0) {
            return;
        }

        var trailerIdx = 0;
        for (let trailer of trailers) {
            var shifts = trailer.Shifts;
            await shifts != undefined && shifts != null &&
                shifts.map((shift, shiftIdx) => {
                    var trips = shift.Trips;
                    var isExitLoop = false;
                    var isPickup = false;
                    var isDriver = false;
                    trips != undefined && trips != null &&
                        trips.map((trip, tripIdx) => {
                            if ($event.SelectedDrivers.length > 0) {
                                if (!isExitLoop) {
                                    var drivers = trip.Drivers;
                                    var driver = drivers.filter(d => $event.SelectedDrivers.some(driver => driver.Id == d.Id));
                                    if (driver != null && driver.length > 0) {
                                        isExitLoop = true;
                                        isDriver = true;
                                    }
                                }
                            }

                            var code = "";
                            if ($event.SelectedPickups.length > 0) {
                                if (!isExitLoop) {
                                    if (trip.IsCommonPickup) {
                                        if (trip.PickupLocationType == PickupLocationType.BulkPlant) {
                                            code = trip.BulkPlant.SiteName;
                                        }
                                        else if (trip.PickupLocationType == PickupLocationType.Terminal) {
                                            code = trip.Terminal.Id.toString();
                                        }
                                    }
                                    else {
                                        trip.DeliveryRequests.map((dr, drIdx) => {
                                            if (dr.PickupLocationType == PickupLocationType.BulkPlant) {
                                                code = dr.BulkPlant.SiteName;
                                            }
                                            else if (dr.PickupLocationType == PickupLocationType.Terminal) {
                                                code = dr.Terminal.Id.toString();
                                            }
                                        });
                                    }

                                    if (code != undefined && code != "") {
                                        var pickup = $event.SelectedPickups.some(pick => pick.Code == code.replace(' ', ''));
                                        if (pickup) {
                                            isExitLoop = true;
                                            isPickup = true;
                                        }
                                    }
                                }
                            }
                        });

                    var isShiftShow = false;
                    if ($event.SelectedDrivers.length > 0 && $event.SelectedPickups.length > 0) {
                        if (isDriver && isPickup) {
                            isShiftShow = true;
                        }
                    }
                    else if ($event.SelectedDrivers.length > 0 && $event.SelectedPickups.length == 0) {
                        if (isDriver) {
                            isShiftShow = true;
                        }
                    }
                    else if ($event.SelectedDrivers.length > 0 && $event.SelectedPickups.length == 0) {
                        if (isDriver) {
                            isShiftShow = true;
                        }
                    }
                    else if ($event.SelectedDrivers.length == 0 && $event.SelectedPickups.length > 0) {
                        if (isPickup) {
                            isShiftShow = true;
                        }
                    }
                    else if ($event.SelectedDrivers.length == 0 && $event.SelectedPickups.length == 0) {
                        isShiftShow = true;
                    }

                    if (isShiftShow)
                        this.TrailerViewFilter.Shifts[trailerIdx + "-" + shiftIdx] = true;
                    else
                        this.TrailerViewFilter.Shifts[trailerIdx + "-" + shiftIdx] = false;
                });

            if ($event.SelectedTrailers.length > 0) {
                var trail = $event.SelectedTrailers.some(tr => tr.Id == trailer.Id);
                if (!trail) {
                    this.TrailerViewFilter.Trailers[trailerIdx] = false;
                }
                else {
                    this.TrailerViewFilter.Trailers[trailerIdx] = true;
                }
            }
            trailerIdx++;
        }
    }

    IntializeChat(driverId) {
        this.sendbirdComponent.IntializeDriverChat(driverId, this.SelectedRegionId);
    }
    LoadDriverDetails(driverViewShifts: ScheduleShiftModel[]) {
        let driverinfo = [];
        driverViewShifts.forEach(xshiftItem => {
            var items = xshiftItem.Schedules.filter(top => top.Drivers.length > 0);
            items.forEach(driverItem => {
                driverItem.Drivers.forEach(drItem => {
                    if (driverinfo.findIndex(top => top === drItem.Id) === -1) {
                        driverinfo.push(drItem.Id);
                    }
                });

            });
        });
        return driverinfo;
    }
    LoadDriverDetailsShift(driverViewShifts: TrailerViewModel[]) {
        let driverinfo = [];
        driverViewShifts.forEach(xshiftItem => {
            if (xshiftItem.Shifts.length > 0) {
                xshiftItem.Shifts.forEach(shiftItem => {
                    var items = shiftItem.Trips.filter(top => top.Drivers.length > 0);
                    items.forEach(driverItem => {
                        driverItem.Drivers.forEach(drItem => {
                            if (driverinfo.findIndex(top => top === drItem.Id) === -1) {
                                driverinfo.push(drItem.Id);
                            }
                        });

                    });
                });
            }
        });
        return driverinfo;
    }
    IntializeSendBirdAccount(driverInfo: any[], regionId) {
        this.sendbirdComponent.IntializeChatDefault(driverInfo, regionId);
    }

    updateFiltersFromLocalStorage(): void {
        let _regionId = MyLocalStorage.getData(MyLocalStorage.DSB_REGION_KEY);
        if (_regionId != '') {
            this.ScheduleBuilderFilters.RegionId = _regionId;
        }
        let _dateFilter = MyLocalStorage.getData(MyLocalStorage.DSB_DATEFILTER_KEY);
        if (_dateFilter != '') {
            this.ScheduleBuilderFilters.DateFilter = _dateFilter;
        }
        let _objectFilter = MyLocalStorage.getData(MyLocalStorage.DSB_OBJECTFILTER_KEY);
        if (_objectFilter != '') {
            this.ScheduleBuilderFilters.ObjectFilter = _objectFilter;
        }
        let _windowFilter = MyLocalStorage.getData(MyLocalStorage.DSB_WINDOWMODE_KEY);
        if (_windowFilter != '') {
            this.ScheduleBuilderFilters.WindowMode = _windowFilter;
        }
        let _date = MyLocalStorage.getData(MyLocalStorage.DSB_DATE_KEY);
        if (_date != '') {
            this.ScheduleBuilderFilters.Date = _date;
        }
        let _dsbViewFilter = MyLocalStorage.getData(MyLocalStorage.DSB_FILTER_KEY);
        if (_dsbViewFilter != '') {
            this.ScheduleBuilderFilters.DSBFilter = _dsbViewFilter;
        }
        this.disableControlsonLoad();
    }

    disableControlsonLoad() {
        if (this.ScheduleBuilderFilters.DateFilter == 1) {
            this.disableControl = false;
        }
        else if (this.ScheduleBuilderFilters.DateFilter == 2) {
            this.disableControl = false;
        }
        else if (this.ScheduleBuilderFilters.DateFilter == 3) {
            let status = this.disableControls(this.ScheduleBuilderFilters.Date);
            this.disableControl = status;
        }
        else if (this.ScheduleBuilderFilters.DateFilter == 4) {
            this.disableControl = true;
        }
        MyLocalStorage.setData(MyLocalStorage.DSB_READONLY_KEY, this.disableControl);
        this.dataService.setDisableDSBControls(this.disableControl);
    }

    onRedirectWallyBoardPage() {
        MyLocalStorage.setData(MyLocalStorage.DSB_READONLY_KEY, this.disableControl);
        MyLocalStorage.setData(MyLocalStorage.DSB_REGION_KEY, this.SelectedRegionId);
        let date = this.SbForm.get('Date').value;
        let selectedDate = moment(date).format('MM/DD/YYYY');
        MyLocalStorage.setData(MyLocalStorage.DSB_DATE_KEY, selectedDate);
        window.open("/Dispatcher/Dashboard", "_blank");
    }

    setReadOnlyControlsValue() {
        MyLocalStorage.setData(MyLocalStorage.DSB_READONLY_KEY, this.disableControl);
        MyLocalStorage.setData(MyLocalStorage.DSB_REGION_KEY, this.SelectedRegionId);
        MyLocalStorage.setData(MyLocalStorage.DSB_DATE_KEY, this.SbForm.get('Date').value);
    }

    setRegionForRoute() {
        this.routeInfoComponent.setRegionId(currentUserCompanyId, this.SelectedRegionId);
    }

    private getOttoSetting(): void {
        this.IsOttoBuilderOpened = false;
        this.isDisplayOttoButton = false;
        this.sbService.getOttoSetting().subscribe(data => {
            if (data.StatusCode == 0) {
                this.isDisplayOttoButton = true;
            }
        });
        this.dataService.OpenDsbOttoBuilderSubject.subscribe(x => {
            this.IsOttoBuilderOpened = x;
            if (!x) {
                Declarations.closeSlidePanel();
            }
        });
        this.dataService.RefreshDsbOttoBuilderSubject.subscribe(x => {
            if (x) {
                this.refreshScheduleBuilder(this.ScheduleBuilderFilters.RegionId, this.ScheduleBuilderFilters.Date, this.ScheduleBuilderFilters.ObjectFilter, this.ScheduleBuilderFilters.DSBFilter, this.initScheduleBuilder);
            }
        });
    }

    public openOttoBuilder(): void {
        this.IsOttoBuilderOpened = true;
    }
    public openOttoNotification(): void {
        this.IsOttoNotificationOpened = true;
    }
    public getOttoNotificationCount() {
        this.sbService.getOttoNotificationCount(this.SelectedRegionId)
            .subscribe(detail => {
                if (detail.StatusCode == 0) {
                    this.OttoNotificationCount = detail.OttoNotificationCount;
                }
                else {
                    this.OttoNotificationCount = 0;
                }
            });
    }
    onDSBFilterChange(filter: number): void {
        this.resetSbFilter();
        this.ScheduleBuilderFilters.DSBFilter = filter;
        MyLocalStorage.setData(MyLocalStorage.DSB_FILTER_KEY, filter);
        let _dSBFilter = this.SbForm.get('DSBFilter');
        if (_dSBFilter.value != filter) {
            _dSBFilter.setValue(filter);
        }
    }
    onShiftsSelect(shift: any) {
        if (shift != null && shift.ShiftInfo != '') {
            var shiftStartInfo = shift.ShiftInfo.split('-')[1].trim();
            let _shifts = this.SbForm.get('Shifts') as FormArray;
            _shifts.controls.forEach(x => {
                let shiftId = x.get('Id').value;
                let startTime = x.get('StartTime').value;
                if (shiftId == shift.Id && startTime == shiftStartInfo) {
                    x.get('IsVisible').patchValue(true);
                }
            });
            if (this.regionDSBModel != null) {
                var shiftInfo = this.regionDSBModel.DSBShiftInfo.findIndex(x => x.Id == shift.Id);
                if (shiftInfo == -1) {
                    var shiftmodel = new ShiftViewModel();
                    shiftmodel.Id = shift.Id;
                    shiftmodel.ShiftInfo = shift.ShiftInfo;
                    this.regionDSBModel.DSBShiftInfo.push(shiftmodel);
                    var filterModel = JSON.stringify(this.regionDSBModel);
                    this.dispatcherService.postFiltersData(TfxModule.DSBShift, filterModel).subscribe(res => {
                        if (res) {

                        }
                    });
                }
            }
        }

        this.dataService.setShiftVisibility(true);
    }
    onShiftsDeSelect(shift: any) {
        if (shift != null && shift.ShiftInfo != '') {
            var shiftStartInfo = shift.ShiftInfo.split('-')[1].trim();
            let _shifts = this.SbForm.get('Shifts') as FormArray;
            _shifts.controls.forEach(x => {
                let shiftId = x.get('Id').value;
                let startTime = x.get('StartTime').value;
                if (shiftId == shift.Id && startTime == shiftStartInfo) {
                    x.get('IsVisible').patchValue(false);
                }
            });
            if (this.regionDSBModel != null) {
                var shiftInfo = this.regionDSBModel.DSBShiftInfo.findIndex(x => x.Id == shift.Id);
                if (shiftInfo >= 0) {
                    this.regionDSBModel.DSBShiftInfo.splice(shiftInfo, 1);
                    var filterModel = JSON.stringify(this.regionDSBModel);
                    this.dispatcherService.postFiltersData(TfxModule.DSBShift, filterModel).subscribe(res => {
                        if (res) {

                        }
                    });
                }
            }
        }
        this.dataService.setShiftVisibility(true);
    }
    onShiftsSelectAll() {
        let DSBShiftInfo: ShiftViewModel[] = [];
        let _shifts = this.SbForm.get('Shifts') as FormArray;
        _shifts.controls.forEach(x => {
            x.get('IsVisible').patchValue(true);
            let shiftViewModel = new ShiftViewModel();
            shiftViewModel.Id = x.get('Id').value;
            var shiftInfo = this.Shifts.find(x => x.Id == shiftViewModel.Id);
            if (shiftInfo != null) {
                shiftViewModel.ShiftInfo = shiftInfo.ShiftInfo;
            }
            DSBShiftInfo.push(shiftViewModel);
        });
        if (this.regionDSBModel != null) {
            this.regionDSBModel.DSBShiftInfo = DSBShiftInfo;
            var filterModel = JSON.stringify(this.regionDSBModel);
            this.dispatcherService.postFiltersData(TfxModule.DSBShift, filterModel).subscribe(res => {
                if (res) {

                }
            });
        }
        this.dataService.setShiftVisibility(true);
    }
    onShiftsDeSelectAll() {
        let _shifts = this.SbForm.get('Shifts') as FormArray;
        _shifts.controls.forEach(x => {
            x.get('IsVisible').patchValue(false);
        });
        if (this.regionDSBModel != null) {
            this.regionDSBModel.DSBShiftInfo = [];
            var filterModel = JSON.stringify(this.regionDSBModel);
            this.dispatcherService.postFiltersData(TfxModule.DSBShift, filterModel).subscribe(res => {
                if (res) {

                }
            });
        }
        this.dataService.setShiftVisibility(true);
    }
    getShiftInformation(shiftInfo: ScheduleShiftModel[], regionId: string) {
        this._loadingBuilder = true;
        this.regionDSBModel.RegionId = regionId;
        this.regionDSBModel.DSBShiftInfo = [];
        this.dispatcherService.getDSBShiftFilters(TfxModule.DSBShift, regionId).subscribe(res => {
            if (res && res.length > 0) {
                this.SetFilters(res);
            }
            if (shiftInfo.length > 0) {
                this.Shifts = [];
                shiftInfo.forEach(x => {
                    var shiftExists = this.Shifts.findIndex(top => top.Id == x.Id);
                    if (shiftExists == -1) {
                        var shiftModel = new ShiftViewModel();
                        shiftModel.Id = x.Id;
                        shiftModel.ShiftInfo = "Shift - " + x.StartTime + " - " + x.EndTime;
                        shiftModel.StartTime = x.StartTime;
                        shiftModel.EndTime = x.EndTime;
                        this.Shifts.push(shiftModel);
                    }
                });
                this.SbForm.get('SelectedShifts').patchValue(this.Shifts);
                if (this.regionDSBModel != null && this.regionDSBModel.DSBShiftInfo.length > 0) {
                    var shiftChangeDetect = false;
                    this.regionDSBModel.DSBShiftInfo.forEach(x => {
                        var shiftExists = this.Shifts.find(x1 => x1.Id == x.Id);
                        if (shiftExists == null) {
                            shiftChangeDetect = true;
                            return;
                        }
                    });
                    if (shiftChangeDetect) {
                        this.regionDSBModel.DSBShiftInfo = this.Shifts;
                        var filterModel = JSON.stringify(this.regionDSBModel);
                        this.dispatcherService.postFiltersData(TfxModule.DSBShift, filterModel).subscribe(res => {
                            if (res) {

                            }
                        });
                        this.SbForm.get('SelectedShifts').patchValue(this.Shifts);
                    }
                    else {
                        this.SbForm.get('SelectedShifts').patchValue(this.regionDSBModel.DSBShiftInfo);
                    }
                }
                else {
                    this.regionDSBModel.DSBShiftInfo = this.Shifts;
                    var filterModel = JSON.stringify(this.regionDSBModel);
                    this.dispatcherService.postFiltersData(TfxModule.DSBShift, filterModel).subscribe(res => {
                        if (res) {

                        }
                    });
                }

            }
            else {
                this.Shifts = [];
                this.SbForm.get('SelectedShifts').patchValue(this.Shifts);
                this.regionDSBModel.DSBShiftInfo = [];
                var filterModel = JSON.stringify(this.regionDSBModel);
                this.dispatcherService.postFiltersData(TfxModule.DSBShift, filterModel).subscribe(res => {
                    if (res) {
                    }
                });
            }
            let _shifts = this.SbForm.get('Shifts') as FormArray;
            _shifts.controls.forEach(x => {
                if (this.regionDSBModel.DSBShiftInfo.length > 0) {
                    var shiftIndex = this.regionDSBModel.DSBShiftInfo.findIndex(x1 => x1.Id == x.get('Id').value);
                    if (shiftIndex >= 0) {
                        x.get('IsVisible').patchValue(true);
                    }
                    else {
                        x.get('IsVisible').patchValue(false);
                    }
                }
                else {
                    x.get('IsVisible').patchValue(true);
                }
            });
            this._loadingBuilder = false;
        });

    }
    onApplyGridViewFilter($event) {
        if ($event.SearchLocation == undefined)
            $event.SearchLocation = '';
        let sbViewId = this.SbForm.get('ObjectFilter').value;
        if (sbViewId == 1) {
            // driver view
            this.filterGridViewDriverView($event)
        }

    }
    filterGridViewDriverView($event) {
        var searchLocation = $event.SearchLocation;
        let data = {
            searchLocation: searchLocation
        }
        this.dataService.setGridViewSearch(data);


    }
    openDRReportGrid() {
        window.open("Carrier/ScheduleBuilder/DeliveryRequestsReport", "_blank");
    }
    // assign dr start
    getOrdersAndCarrierDetails(delReq: DeliveryRequestViewModel): void {

        document.getElementById('openAssignCarrierRequestModal').click();
        this.requestToBroker = delReq;
        this._loadingCarriers = true;
        this.OrderList = [];

        this.sbService.getAssignCarrierDetails(delReq.CreatedByRegionId, delReq.JobId, delReq.ProductTypeId).subscribe(data => {
            this._loadingCarriers = false;
            if (data != null && data != undefined) {
                this.TfxCarrierDropdownDisplayItem = data.CarrierDetails;
                this.setCarrierRegions(data.CarrierDetails);
                if (data.OrderDetails && data.OrderDetails.length > 0) {
                    this.OrderList = data.OrderDetails;
                    this.AssignDrForm.controls.BrokerOrderId.setValue(this.OrderList[0].Id);
                    this.AssignDrForm.controls.DispatcherNote.setValue(this.OrderList[0].DRNote);
                }
                if (delReq.OrderId != null) {
                    this.OrderList = this.OrderList.filter(top => top.Id == delReq.OrderId);
                    if (this.OrderList.length > 0) {
                        this.AssignDrForm.controls.BrokerOrderId.setValue(this.OrderList[0].Id);
                        this.AssignDrForm.controls.DispatcherNote.setValue(this.OrderList[0].DRNote);
                    }
                }
            }
        });
    }
    getAllCarrierRegions() {
        if (this.CarrierRegions.length > 0)
            return false;
        this.CarrierRegions = [];
        this.regionService.getCarrierRegions().subscribe(response => {
            if (response && response.length > 0) {
                this.CarrierRegions = response;
            }
        });
    }

    onCarrierSelect(data: any, isSelect: boolean) {
        if (isSelect) {
            let selection = this.CarrierRegions.find(c => c.Id == data.Id);
            if (selection) {
                let _formArray = this.AssignDrForm.controls['Carriers'] as FormArray;
                _formArray.push(this.fb.group({
                    Id: this.fb.control(selection.Id),
                    Name: this.fb.control(selection.Name),
                    RegionId: this.fb.control(null, [Validators.required]),
                    SequenceNo: this.fb.control(null),
                }));
            }
        }
        else {
            let _formArray = this.AssignDrForm.controls['Carriers'] as FormArray;
            _formArray.removeAt(_formArray.value.findIndex(carr => carr.Id == data.Id));
        }
    }
    public onCarrierSelectAll(items: any, isSelect: boolean) {
        if (this.SbForm.get('IsAssignDrEnabled').value != true) {
            if (isSelect) {
                this.setCarrierRegions(this.TfxCarrierDropdownDisplayItem);
            }
            else {
                let _formArray = this.AssignDrForm.controls['Carriers'] as FormArray;
                _formArray.clear();
            }
        }
        else {
            let _formArray = this.AssignDrForm.controls['Carriers'] as FormArray;
            if (isSelect) {
                let existingFormCarriers = _formArray.value as any[] || [];
                this.CarrierRegions.forEach((carrierRegion: CarrierRegionModel) => {
                    if (!existingFormCarriers.some(c => c.Id == carrierRegion.Id)) {
                        let _formArray = this.AssignDrForm.controls['Carriers'] as FormArray;
                        this.pushRowInCarrierForm(_formArray, { Id: carrierRegion.Id, Code: null, Name: carrierRegion.Name, RegionId: null, SequenceNo: null });
                    }
                });
            }
            else {
                _formArray.clear();
            }
        }
    }
    initilizeAssignDrForm() {
        this.AssignDrForm = this.fb.group({
            BrokerOrderId: this.fb.control(null),
            DispatcherNote: this.fb.control(null),
            SelectedCarrier: this.fb.control(null),
            Carriers: this.fb.array([]),
            DeliveryRequestWithOrder: this.fb.array([]),
        })
    }
    setCarrierRegions(_carrRegions: TfxCarrierDropdownDisplayItem[]) {
        let _formArray = this.AssignDrForm.controls['Carriers'] as FormArray;
        _formArray.clear();
        _carrRegions.forEach(carr => {
            this.pushRowInCarrierForm(_formArray, carr);
        });
        this.AssignDrForm.get('SelectedCarrier').setValue(_formArray.value);
        this.RegionCarriers = _formArray.value;
        $("#sortableDrCarriers").click()
    }
    removeCarrier(id: number) {
        //remove from form
        let _formArray = this.AssignDrForm.controls['Carriers'] as FormArray;
        _formArray.removeAt(_formArray.value.findIndex(carr => carr.Id == id));
        //remove from dropdown
        let currentSelection: any[] = this.AssignDrForm.get('SelectedCarrier').value || [];
        currentSelection.splice(currentSelection.findIndex(carr => carr.Id == id), 1);
        this.AssignDrForm.get('SelectedCarrier').patchValue(currentSelection);
    }
    getRegionsByCarrierId(id: number): TfxCarrierRegionDetailsModel[] {
        let response = []
        let carr = this.CarrierRegions.find(f => f.Id == id);
        if (carr != null && carr.Regions)
            response = carr.Regions;
        return response;
    }
    makeCarrierUIsortable() {
        var _this = this;
        $(function () {
            let sortable: any = $("#sortableDrCarriers");
            sortable.sortable({
                stop: function (event, ui) {
                    var carrierIds = sortable.sortable("toArray") as number[];
                    _this.updateSequence(carrierIds); sortable.click();
                }
            });
        });
    }
    updateSequence(carrIds: number[]) {
        let _formArray = this.AssignDrForm.controls['Carriers'] as FormArray;
        let carriers = _formArray.value as TfxCarrierDropdownDisplayItem[]
        _formArray.clear();

        carrIds.forEach(id => {
            let carr = carriers.find(c => c.Id == id);
            this.pushRowInCarrierForm(_formArray, carr);
        });
    }
    pushRowInCarrierForm(_formArray: FormArray, data: TfxCarrierDropdownDisplayItem) {
        _formArray.push(this.fb.group({
            Id: this.fb.control(data.Id),
            Name: this.fb.control(data.Name),
            RegionId: this.fb.control(this.CarrierRegions.some(f => f.Regions.some(r => r.Id == data.RegionId)) ? data.RegionId : null, [Validators.required]),
            SequenceNo: this.fb.control(data.SequenceNo),
        }));
    }
    SaveAssignCarrierDetails(): void {

        let assignDrModel = this.AssignDrForm.value;
        if (assignDrModel.Carriers && assignDrModel.Carriers.length > 0) {
            assignDrModel.Carriers.forEach((carr, index: number) => {
                carr.SequenceNo = (index + 1)
            })
        }
        if (assignDrModel.Carriers.length > 0) {
            let model = new DeliveryRequestBrokerInfoViewModel();
            model.CarrierRegionId = assignDrModel.Carriers[0].RegionId;
            model.CarrierCompanyId = assignDrModel.Carriers[0].Id;
            model.DeliveryRequest = this.requestToBroker;
            model.OrderId = assignDrModel.BrokerOrderId;
            model.DispatcherNote = assignDrModel.DispatcherNote;
            model.CarrierInfo = assignDrModel.Carriers;
            model.CarrierInfoJson = JSON.stringify(assignDrModel.Carriers);
            model.IsDispatchRetainedByCustomer = false;
            model.BlendedGroupId = this.requestToBroker.BlendedGroupId;
            model.UniqueOrderNo = this.requestToBroker.UniqueOrderNo;
            var orderInfo = this.OrderList.find(top => top.Id == assignDrModel.BrokerOrderId);
            if (orderInfo != null && orderInfo.Code == "1") {
                model.IsDispatchRetainedByCustomer = true;
            }

            this._loadingCarriers = true;
            this.sbService.BrokerDeliveryRequestToCarrier(model).subscribe(data => {
                Declarations.hideModal('#assignCarrierRequestModal');
                this._loadingCarriers = false;
                if (data.StatusCode != 1) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                    if (this.requestToBroker.BlendedGroupId) {
                        var blendDrs = this.allApiDeliveryRequest.filter(t => t.BlendedGroupId == this.requestToBroker.BlendedGroupId);
                        this.dataService.setRemoveDroppedRequestSubject(blendDrs);
                    }
                    else {
                        this.dataService.setRemoveDroppedRequestSubject([this.requestToBroker]);
                    }
                    this.dataService.setDrQueueChangesForFilter(true);
                } else {
                    Declarations.msgerror(data.StatusMessage, undefined, undefined);
                }
            });
        }
    }
    assignDrsToCarrier() {
        this.SbForm.get('IsAssignDrEnabled').setValue(true);
        this.dataService.setScheduleChangeDetectSubject(true);
        //GET ALL CARRIERS FOR REGION
        this.getAllCarrierRegions();
    }
    async cancelAssignDrsToCarrier() {
        this.SbForm.get('IsAssignDrEnabled').setValue(false);
        let _shifts = this.SbForm.get('Shifts') as FormArray;
        _shifts.controls.forEach(shift => {
            let _schedules = shift.get('Schedules') as FormArray;
            _schedules.controls.forEach(schedule => {
                let _trips = schedule.get('Trips') as FormArray;
                _trips.controls.forEach(trip => {
                    let _drs = trip.get('DeliveryRequests') as FormArray;
                    _drs.controls.forEach(dr => {
                        if (dr.get('IsSelectedForAssignment').value == true) {
                            dr.get('IsSelectedForAssignment').setValue(false);
                        }
                    })
                });
            });
        });
        this.dataService.setScheduleChangeDetectSubject(true);
    }
    getSelectedCarriersByRegion() {
        this.regionService.getSelectedCarriersByRegion(this.SbForm.get('RegionId').value).subscribe(response => {
            if (response && response.length > 0) {
                this.TfxCarrierDropdownDisplayItem = response;
                this.setCarrierRegions(response);
            }
        });
    }

    assignCarriersToDrs() {
        document.getElementById('openAssignCarrierRequestModal').click();
        //INITILIZE FORM
        this.initilizeAssignDrForm();
        //GET SELECTED CARRIERS IN REGION
        this.getSelectedCarriersByRegion();
        this.setDeliveryRequestWithOrder();
    }
    setDeliveryRequestWithOrder() {

        //GET SELECTED DRS
        let selectedDrs = [];

        this.SbForm.get('Shifts')['controls'].forEach(shift => {
            shift.get('Schedules')['controls'].forEach(schedule => {
                schedule.get('Trips')['controls'].forEach(trip => {
                    trip.get('DeliveryRequests')['controls'].forEach(dr => {
                        if (dr.get('IsSelectedForAssignment').value == true) {
                            if (dr.get('IsBlendedRequest').value == false && this.CompletedScheduleStatuses.filter(x1 => x1 == dr.get('TrackScheduleStatus').value).length == 0) {
                                selectedDrs.push(dr.value);
                            }
                            else if (dr.get('IsBlendedRequest').value == true && dr.get('IsBlendedDrParent').value == true && this.CompletedScheduleStatuses.filter(x1 => x1 == dr.get('TrackScheduleStatus').value).length == 0) {
                                selectedDrs.push(dr.value);
                            }

                        }
                    })
                });
            });
        });

        if (selectedDrs.length == 0) {
            return false;
        }
        //GET ORDERS BY JOB AND FUEL TYPE
        let uniqueJobAndFuelType: any[] = []
        selectedDrs.forEach(dr => {
            if (!uniqueJobAndFuelType.includes(x => x.JobId == dr.JobId && x.ProductTypeId == dr.ProductTypeId)) {
                uniqueJobAndFuelType.push({ JobId: dr.JobId, ProductTypeId: dr.ProductTypeId, BlendedGroupId: dr.BlendedGroupId, IsBlendedDrParent: dr.IsBlendedDrParent, IsBlendedRequest: dr.IsBlendedRequest, IsAdditive: dr.IsAdditive, OrderId: dr.OrderId })
            }
        });

        this._loadingCarriers = true;
        this.sbService.getOrdersByDeliveryRequests(uniqueJobAndFuelType).subscribe((response) => {
            this._loadingCarriers = false;
            if (response != null && response != undefined) {
                this.OrderList = response;

                let fmArr = this.AssignDrForm.get('DeliveryRequestWithOrder') as FormArray;
                selectedDrs.forEach(dr => {
                    let orders = this.OrderList.filter(o => o.JobId == dr.JobId && o.FuelTypeId == dr.ProductTypeId);
                    if (orders.length > 0) {
                        fmArr.push(
                            this.fb.group({
                                DeliveryRequest: this.fb.control(dr),
                                Orders: this.fb.control(dr.IsBlendedDrParent == false ? orders : orders.filter(x => x.Id == dr.OrderId)),
                                OrderId: this.fb.control((dr.OrderId > 0 ? dr.OrderId : (orders.length > 0 ? orders[0].Id : null)), Validators.required)
                            })
                        );
                    }
                    if (!uniqueJobAndFuelType.includes(x => x.JobId == dr.JobId && x.ProductTypeId == dr.ProductTypeId)) {
                        uniqueJobAndFuelType.push({ JobId: dr.JobId, ProductTypeId: dr.ProductTypeId })
                    }
                });
            }
        });
    }

    removeDrSelection(index: number, drId: string) {
        let formArray = this.AssignDrForm.get('DeliveryRequestWithOrder') as FormArray;
        if (formArray.length > 1 && drId) {
            formArray.removeAt(index)
            this.unselectDrById(drId);
        }
        else {
            Declarations.msgerror("Minimum one delivery request is required.", undefined, undefined);
        }
    }

    unselectDrById(drId: string) {
        this.SbForm.get('Shifts')['controls'].forEach(shift => {
            shift.get('Schedules')['controls'].forEach(schedule => {
                schedule.get('Trips')['controls'].forEach(trip => {
                    trip.get('DeliveryRequests')['controls'].forEach(dr => {
                        if (dr.get('IsSelectedForAssignment').value && dr.get('Id').value == drId)
                            dr.get('IsSelectedForAssignment').setValue(false);
                    })
                });
            });
        });
        this.dataService.setScheduleChangeDetectSubject(true);
    }

    resetLoad(trip: FormGroup) {
        if (trip) {

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
                IsPreloadDisable: false,
                TimeStamp: trip.controls['TimeStamp'].value
            }
            trip.reset();
            (trip.controls['DeliveryRequests'] as FormArray).clear();
            trip.reset(reserveValue);
        }
    }

    saveMultipleDrCarrierSequence() {

        let formModel = this.AssignDrForm.value;

        let input = new DeliveryRequestBrokerInfoViewModel();

        input.ScheduleBuilderId = this.SbForm.get('Id').value;
        input.BrokerDrModel = [];

        //set carr sequence
        if (formModel.Carriers && formModel.Carriers.length > 0) {
            formModel.Carriers.forEach((carr, index: number) => {
                carr.SequenceNo = (index + 1)
            })
        }
        else {
            Declarations.msgerror("Assign at least one carrier.", undefined, undefined);
            return;
        }

        formModel.DeliveryRequestWithOrder.forEach(drObj => {
            let drDetails = new BrokerDrModel();
            drDetails.CarrierRegionId = formModel.Carriers[0].RegionId;
            drDetails.CarrierCompanyId = formModel.Carriers[0].Id;
            drDetails.DeliveryRequest = drObj.DeliveryRequest;
            drDetails.OrderId = drObj.OrderId;
            drDetails.IsDispatchRetainedByCustomer = false;
            if (drDetails.DeliveryRequest.IsBlendedRequest) {
                drDetails.BlendedGroupId = drDetails.DeliveryRequest.BlendedGroupId;
                drDetails.UniqueOrderNo = drDetails.DeliveryRequest.UniqueOrderNo;
                drDetails.IsBlendedGroupProduct = true;
            }
            else {
                drDetails.UniqueOrderNo = drDetails.DeliveryRequest.UniqueOrderNo;
            }
            var orderInfo = this.OrderList.find(top => top.Id == drObj.OrderId);
            if (orderInfo != null && orderInfo.Code == "1")
                drDetails.IsDispatchRetainedByCustomer = true;
            var checkBlendedProduct = input.BrokerDrModel.filter(x => x.IsBlendedGroupProduct == true);
            if (checkBlendedProduct.length > 0) {
                var blendedDRExists = input.BrokerDrModel.find(x => x.BlendedGroupId == drDetails.BlendedGroupId);
                if (blendedDRExists == null) {
                    if (this.CompletedScheduleStatuses.filter(x1 => x1 == drDetails.DeliveryRequest.TrackScheduleStatus).length == 0) {
                        input.BrokerDrModel.push(drDetails);
                    }
                }
            }
            else {
                if (this.CompletedScheduleStatuses.filter(x1 => x1 == drDetails.DeliveryRequest.TrackScheduleStatus).length == 0) {
                    input.BrokerDrModel.push(drDetails);
                }
            }
        });

        input.DispatcherNote = formModel.DispatcherNote;
        input.CarrierInfo = formModel.Carriers;
        input.CarrierInfoJson = JSON.stringify(formModel.Carriers);

        this._loadingCarriers = true;
        this.sbService.BrokerDeliveryRequestsToCarriers(input).subscribe(data => {
            Declarations.hideModal('#assignCarrierRequestModal');
            this._loadingCarriers = false;
            if (data && data.StatusCode != 1) {
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                //REMOVE DR FROM DSB FORM
                this.SbForm.get('Shifts')['controls'].forEach((shift: FormGroup) => {
                    shift.get('Schedules')['controls'].forEach((schedule: FormGroup) => {
                        schedule.get('Trips')['controls'].forEach((trip: FormGroup) => {
                            if (trip.get('DeliveryRequests')['controls'].length > 0) {

                                let allDrs = trip.get('DeliveryRequests').value as any[];
                                let existingDrs = allDrs.filter(dr => dr.IsSelectedForAssignment != true);

                                let deliveryRequests = trip.get('DeliveryRequests') as FormArray;
                                deliveryRequests.clear();

                                if (existingDrs.length > 0) {

                                    existingDrs = sortArrayTwice(existingDrs, 'JobId', 'ProductSequence');
                                    existingDrs.forEach(x => {
                                        var _form = this.utilService.getDeliveryRequestForm(x, trip.get('IsCommonPickup').value);
                                        deliveryRequests.push(_form);
                                    });
                                }
                                else {
                                    this.resetLoad(trip);
                                }
                            }
                        });
                    });
                });
                this.dataService.setScheduleChangeDetectSubject(true);
            } else {
                Declarations.msgerror(data.StatusMessage, undefined, undefined);
            }
            this.cancelAssignDrsToCarrier();
        });
    }
    // assign dr end   

    //Add Drop location   

    clearDropLocationControls() {
        this.Orders = [];
        this.SiteList = [];
        this.companyList = [];
        this.selectedOrder = [];
        this.selectedCustomer = [];
        this.selectedSite = [];
    }

    getDropLocationDetails(tdr: FormGroup): void {
        this.CurrentDr = tdr;
        this.SelectedFuelId = tdr.controls['FuelTypeId'].value;
        let productTypeId = tdr.controls['ProductTypeId'].value;
        let dr = tdr.value as DeliveryRequestViewModel;
        let terminalId = null, bulkplantId = null;
        if ((dr.BadgeNo1 == null || dr.BadgeNo1 == '') && (dr.BadgeNo2 == null || dr.BadgeNo2 == '') && (dr.BadgeNo3 == null || dr.BadgeNo3 == '')) {
            if (dr.Terminal && dr.Terminal.Id > 0) {
                terminalId = dr.Terminal.Id;
            }
            if (dr.BulkPlant && dr.BulkPlant.SiteId > 0) {
                bulkplantId = dr.BulkPlant.SiteId;
            }
        }
        if (this.SelectedRegionId != null && this.SelectedRegionId != undefined && this.SelectedRegionId != '') {
            this.sbService.getJobDetailsWithOrders(this.SelectedRegionId, this.SelectedFuelId, productTypeId, terminalId, bulkplantId, this.SbForm.get('Date').value).subscribe(response => {
                if (response) {
                    this.drOrders = response as JobDetailsWithOrders[];
                    this.companyList = response.map((element) => ({ CompanyId: element.CompanyId, CompanyName: element.CompanyName }));
                    this.companyList = this.GetUniqueLocations(this.companyList.reduce((p, n) => p.concat(n), []));
                    this.SiteList = response.map((element) => ({ Id: element.JobId, Name: element.JobName }));
                    this.SiteList = this.GetUniqueLocations(this.SiteList.reduce((p, n) => p.concat(n), []));
                }
                else {
                    Declarations.msgerror('No order exists for this fuel type', 'error', 2500)
                }
            });
        }
    }

    public onSiteSelect(item: any): void {
        this.SelectedLocationId = item.Id
        let orders = this.drOrders.filter(t => t.JobId == item.Id);
        if (orders && orders.length > 0) {
            this.Orders = orders.map((element) => ({ Id: element.OrderId, Name: element.PoNumber }));;
        }
    }

    public onSiteDeSelect(item: DropdownItem): void {
        this.Orders = [];
    }

    public onCustomerSelect(item: any): void {
        this.selectedSite = [];
        this.selectedOrder = [];
        this.SelectedCustomerId = item.CompanyId;
        this.SiteList = this.drOrders.filter(x => x.CompanyId == item.CompanyId).map((element) => ({ Id: element.JobId, Name: element.JobName }));
        this.SiteList = this.GetUniqueLocations(this.SiteList.reduce((p, n) => p.concat(n), []));
    }

    public onCustomerDeSelect(item: any): void {
        this.SelectedCustomerId = null;
        this.selectedSite = [];
        this.selectedOrder = [];
        this.SiteList = this.drOrders.map((element) => ({ Id: element.JobId, Name: element.JobName }));
        this.SiteList = this.GetUniqueLocations(this.SiteList.reduce((p, n) => p.concat(n), []));
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

    AddDropLocation() {
        if (this.selectedOrder.length <= 0) {
            Declarations.msgerror('please select order', 'Validation', 2500);
            return false;
        }
        let order = this.drOrders.filter(t => t.OrderId == this.selectedOrder[0].Id)[0];
        if (order) {
            this.CurrentDr.get('JobId').setValue(order.JobId);
            this.CurrentDr.get('OrderId').setValue(order.OrderId);
            this.CurrentDr.get('JobName').setValue(order.JobName);
            this.CurrentDr.get('JobAddress').setValue(order.Address);
            this.CurrentDr.get('JobCity').setValue(order.City)
            this.CurrentDr.get('UoM').setValue(order.UoM);
            this.CurrentDr.get('CustomerCompany').setValue(order.CompanyName);
            this._loadingDrRequests = true;
            let input = this.CurrentDr.value as DeliveryRequestViewModel;
            this.carrierService.updateDeliveryRequest([input])
                .subscribe((data: any) => {
                    this._loadingDrRequests = false;
                    if (data.StatusCode == 0) {
                        Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                        var elem = document.getElementById('TBD-Drop-location-Close-popUp'); elem.click();
                        this.carrierService.getDeliveryRequestById(this.CurrentDr.value.ParentId).subscribe(x => {
                            if (order.JobId && x.TrackableScheduleId != null) {
                                let dropUrl = '/Supplier/Invoice/CreateNew?orderId=' + order.OrderId + '&trackableScheduleId=' + x.TrackableScheduleId;
                                window.location.href = dropUrl;
                            }
                        })
                    }
                });
        }
    }
    SetFilters(input: any) {
        this.regionDSBModel = JSON.parse(input);
    }
    addSpiltDR() {
        if (this.requestToUpdate != null) {
            this.requestToUpdate.IsSpiltDRAdded = true;
            let spiltDRModel = new SpiltDRsModel();
            spiltDRModel.ScheduleQuantityType = 1;
            this.requestToUpdate.SpiltDRs.push(spiltDRModel);
        }
    }
    openCalendarView() {
        window.location.href = '/Carrier/Calendar';
    }

    //LOCATION SEQUENCING IN LOAD
    public _loader: boolean = false;
    public TripLocations: any[] = [];
    //public LoadLocationsequence: string[] = [];
    public _isSequencePanelInitiated: boolean = false;
    private locationsequenceSubscription: Subscription = new Subscription();
    public _locSequenceOpened: boolean = false;
    public loadForSequence: FormGroup;
    public lat = 47.962741;
    public long = -96.050422;

    subscribeLoadLocationSequenceSubject() {

        this.locationsequenceSubscription = this.dataService.LoadLocationSequenceSubject.subscribe(tripFormGroup => {
            if (tripFormGroup) {

                this.TripLocations = [];
                this.loadForSequence = tripFormGroup;
                let drs = tripFormGroup.controls['DeliveryRequests'].value as DeliveryRequestViewModel[];

                if (drs && drs.length > 1) {

                    let uniqueByJobId = [...new Map(drs.map((item) => [item.JobId, item])).values()];
                    let uniqueByTbdGroupId = [...new Map(drs.map((item) => [item.TBDGroupId, item])).values()];
                    //remove null values
                    uniqueByJobId = uniqueByJobId.filter(j => j.JobId);
                    uniqueByTbdGroupId = uniqueByTbdGroupId.filter(j => j.TBDGroupId);
                    //set default sequence
                    uniqueByJobId.forEach((element, index) => {
                        element['Sequence'] = (+index + 1).toString();
                    });
                    uniqueByTbdGroupId.forEach((element) => {
                        element['Sequence'] = '999';
                    });
                    uniqueByTbdGroupId = uniqueByTbdGroupId.filter(j => j.TBDGroupId && !j.JobId);//0, null, undefined

                    if (uniqueByJobId && uniqueByJobId.length > 1) {

                        this.TripLocations = [...uniqueByJobId, ...uniqueByTbdGroupId];
                        let jobIds = uniqueByJobId.map(item => item.JobId) as number[];

                        if (jobIds && jobIds.length > 1) {

                            this.openPanel();
                            this._loader = true;

                            this.sbService.getJobCoordinates(jobIds).subscribe(data => {
                                this._loader = false;

                                if (data && data.length > 0) {
                                    data.forEach((element, index) => {
                                        let _location = this.TripLocations.find(loc => loc.JobId == element.JobId);
                                        if (_location) {
                                            _location.Latitude = element.Latitude;
                                            _location.Longitude = element.Longitude;
                                        }
                                    });
                                }
                            });
                        }
                    }
                }
            }
        });
    }

    openPanel() {
        this._locSequenceOpened = true;
        if (!this._isSequencePanelInitiated) {
            this._isSequencePanelInitiated = true;
        }
        this.makeLocationsSrtable();
    }

    makeLocationsSrtable() {
        var _this = this;
        $(function () {
            let sortable: any = $("#sortableLocations");
            sortable.sortable({
                stop: (event, ui) => {
                    let locationIds = sortable.sortable("toArray") as string[];

                    if (locationIds && locationIds.length > 0) {

                        locationIds.forEach((locId, index) => {
                            let _location = _this.TripLocations.find(loc => loc.JobId == locId);
                            if (_location) {
                                _location.Sequence = (+index + 1).toString();
                            }
                        });
                    }
                    sortable.click();
                }
            });
        });
    }


    saveLocationSequence() {

        this._locSequenceOpened = false;
        this.TripLocations = sortBy(this.TripLocations, 'Sequence');
        let data = { JobIds: this.TripLocations, trip: this.loadForSequence };
        this.dataService.setDispatcherLoadDragDropMapSubject(data);
    }

    getPreferenceSetting() {
        this.carrierService.getCreateDrSetting().subscribe(response => {
            this.SbForm.get('PreferenceSetting').setValue(response);
        });
    }
}



