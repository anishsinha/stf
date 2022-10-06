import * as moment from 'moment';
import { DateFilter, DeliveryGroupStatus, DeliveryReqPriority, DeliveryRequestFor, DeliveryRequestTypes, ObjectFilter, PickupLocationType, QueueFilter, RegionFilter, TripStatus, WindowModeFilter } from 'src/app/app.enum';
import { TfxCarrierDropdownDisplayItem } from 'src/app/company-addresses/region/model/region';
import { DropdownItem, DropdownItemExt } from 'src/app/statelist.service';
import { JobRegionModel, RouteInfoDetails } from './location';


export class BlendedRequest {
    public Berth: string;
    public Id: string;
    public OrderId: number;
    public PoNumber: string;
    public ProductType: string;
    public ProductTypeId: number;
    public RequiredQuantity: number;
    public QuantityInPercent: number;
    public PickupLocationType: number;
    public StatusClassId: number;
    public ScheduleStatus: number;
    public SchedulePreviousStatus: number;
    public Terminal: DropdownItem;
    public BulkPlant: DropAddressModel;
    public IsAdditive: boolean;
    public DeliveryScheduleId: number;
    public TrackableScheduleId: number;
    public TrackScheduleStatus: number;
    public TrackScheduleStatusName: string;
    public TrackScheduleEnrouteStatus: number;
    public UoM: number;
    public JobId: number;
    public CarrierStatus: number;
    public IsBlendedRequest: boolean;
    public IsCommonPickupForBlend: boolean;
    public BlendedGroupId: string;
    public BlendedProductName: string;
    public AdditiveProductName: string;
    public IsBlendedDrParent: boolean;
    public TotalBlendedQuantity: number;
    public BlendParentProductTypeId: number;
    public SelectedDate: string;
    public IsFutureDR: boolean;
    public IsCalendarView: boolean
    public IsDispatcherDragDrop: boolean = false;
    public DispatcherDragDropSequence: number;
    public DeliveryLevelPO: string;
    public ScheduleStartTime: Date;
    public ScheduleEndTime: Date;
    public IndicativePrice: number;

    constructor(isAdditive: boolean) {
        this.IsBlendedRequest = false;
        this.IsCommonPickupForBlend = false;
        this.BlendedGroupId = null;
        this.PickupLocationType = 1;
        this.Terminal = new DropdownItem();
        this.BulkPlant = new DropAddressModel();
        this.IsAdditive = isAdditive;
        this.SelectedDate =  moment(new Date()).format('DD/MM/YYYY')
    }
}

export class DipTestViewModel {
    //public SiteName: string;
    //public CompanyName: string;
    //public AssetId: number;
    public Id: number;
    public SiteId: string;
    public TankId: string;
    public TankName: string;
    public StorageId: string;
    //public Level: number;
    public Ullage: number;
    public ScheduleQuantityType: number;
    //public GrossVolume: number;
    public NetVolume: number;
    //public WaterNetLevel: number;
    //public WaterGrossLevel: number;
    //public CaptureTime: string;
    public FuelTypeId: number;
    public ProductName: string;
    public ProductTypeId: number;
    //public DataSourceTypeId: number;
    public BuyerCompanyId: number;
    public BuyerCompanyName: string;
    public JobName: string;
    public UoM: string;
    public JobId: number;
    public CreatedByRegionId: string;
    public IsDRExists: boolean;
    public ExistingDR: PartialDRDetails[] = [];
    public DisplayDRDetails: boolean;
    public TankCapacity: number;
    //public TankMinFill: number;
    public TankMaxFill: number;
    public IsReAssignToCarrier: boolean = false;
    public IsTankAndAssetAvailableForJob: boolean;
    public CurrentThreshold: number;
    //public FillType: number;
    public ReorderPercent: number;
    public OrderId: number;
    public ReorderQuantity: number;
    public Priority: DeliveryReqPriority;
    public RecurringDRScheduleDetails: RecurringDRSchedule[] = [];
    public isRecurringSchedule: boolean;
    public PoNumber: string;
    public DisplayCaptureTime: string;
    public SupplierCompanyId: number;
    public SupplierCompanies: DropdownItem[];
    public DispactherNote: string;
    public IsCommonBadge: string;
    public Badge1: string;
    public Badge2: string;
    public Badge3: string;
    //public FuelTypeId: string;
    public OrderPickupDetails: OrderPickupDetailModel[];
    public BlendOrderPickupDetails: OrderPickupDetailModel[];
    public Tanks: TankDipTestViewModel[];
    // BLENDED DR
    // public IsBlendedRequest: boolean;
    // public BlendedRequests: BlendedRequest[] = [];
    // public IsCommonPickupForBlend: boolean;
    // public BlendedGroupId: string;
    public SelectedDate: string;
    public ScheduleStartTime: Date;
    public ScheduleEndTime: Date;
}

export class CompatibleProductModel {
    public ProductTypeId: number;
    public MappedToProductTypeIds: number[];
}

export class TankDipTestViewModel {
    public TankId: string;
    public TankName: string;
    public StorageId: string;
    // public Level: number;
    public Ullage: number;
    //public GrossVolume: number;
    public NetVolume: number;
    public WaterLevel: number;
    // public WaterGrossLevel: number;
    // public CaptureTime: string;
    // public DataSourceTypeId: number;
    public TankCapacity: number;
    //public TankMinFill: number;
    public TankMaxFill: number;
    public CurrentThreshold: number;
    // public FillType: number;
    public ReorderPercent: number;
    public ReorderQuantity: number;
    public Priority: DeliveryReqPriority;
    public DisplayCaptureTime: string;
    // for UI dropdown
    public IsShowMore: boolean = false;
}

export class TankCapacityForDR {
    public Priority: DeliveryReqPriority;
    public MaxPercent: number;
    public MinPercent: number;
}

export class ModifiedTripInfo {
    public ShiftId: string;
    public ShiftIndex: number;
    public DriverRowIndex: number;
    public DriverColIndex: number;
}

export class CreateDeliveryRequestViewModel {
    public SiteId: string;
    public TankId: string;
    public StorageId: string;
    public RequiredQuantity: number;
    public Priority: DeliveryReqPriority;
}

export class CustomerJobsForCarrier {
    CompanyId: number;
    CompanyName: string;
    RegionIds: string[];
    Jobs: JobRegionModel[];
}

export class PartialDRDetails {
    public Priority: DeliveryReqPriority;
    public ScheduleStatusName: string;
    public RequiredQuantity: number;
    public CreatedOn: string;
    public ScheduleQuantityType: number;
    public ScheduleQuantityTypeName: string;
    public IsMissedDr: boolean;
}

export class DelRequestByTimeModel {
    constructor() {
        this.DeliveryRequests = [];
    }
    public DeliveryRequests: DeliveryRequestViewModel[];
    public StartTime: number;
    public EndTime: number;
}

export class DelRequestsByJobModel {
    constructor() {
        this.DeliveryRequests = [];
        this.IsBlendedRequest = false;
        this.IsCommonPickupForBlend = false;
        this.BlendedGroupId = null;
    }
    public JobId: number;
    public JobAddress: string;
    public JobName: string;
    public CustomerCompany: string;
    public TrailerCompatibility: string;
    public HoursToCoverDistance: string;
    public _HoursToCoverDistance: number;
    public IsOnlyNightDelivery: boolean;
    public DeliveryRequestType: DeliveryRequestTypes;
    public Priority: DeliveryReqPriority;
    public CarrierStatus: number;
    public DeliveryRequests: DeliveryRequestViewModel[];
    public LoadQueueAttributes: LoadQueueAttributes;
    public DRQueueAttributes: DRQueueAttributes;
    public DeliveryWindow: any
    public JobCity: string;
    public CustomerBrandId: string;
    public IsTBD: boolean;
    public TBDGroupId: string;
    public ProductType: string;
    public ProductTypeId: number;
    public UoM: number;
    public RequiredQuantity: number;
    public ScheduleQuantityTypeText: string;
    public DeliveryDateStartTime: string;
    public Vessel: string;
    public Berth: string;
    public IsMarine: boolean;
    //BLENDED DR
    public IsBlendedRequest: boolean;
    public IsCommonPickupForBlend: boolean;
    public BlendedGroupId: string;
}

export class SplitBlendDRModel {
    public OrderId: number;
    public RequiredQuantity: number;
    public ParentDrId: string;
    public ProductType: string;
    public FuelType: string;
    public UoM: number;
}

export class DeliveryRequestViewModel extends BlendedRequest {
    constructor(isAdditive: boolean) {
        super(isAdditive);
        this.WindowMode = 1;
        this.QueueMode = 1;
        this.Compartments = [];
        this.IsRetainFuelLoaded = false;
    };
    public JobAddress: string;
    public JobCity: string;
    public JobName: string;
    public SiteId: string;
    public Priority: number;
    public AssignedToCompanyId: number;
    public CreatedByCompanyId: number;
    public SupplierCompanyId: number;
    public Status: number;
    public PreviousStatus: number;
    public CreatedByRegionId: string;
    public AssignedToRegionId: string;
    public IsDeleted: boolean;
    public DeliveryGroupId: number;
    public ParentId: string;
    public CustomerCompany: string;
    public WindowMode: number;
    public QueueMode: number;
    public TankId: string;
    public StorageId: string;
    public TankMaxFill: number;
    public IsNotCompatible: boolean;
    public IsAutoCreatedDR: boolean;
    public BadgeNo1: string;
    public BadgeNo2: string;
    public BadgeNo3: string;
    public SourceTripId: string;
    public IsCommonBadge: boolean;
    public DispactherNote: string;
    public PreLoadedFor: string;
    public PreLoadInfo: LoadInfo;
    public PostLoadedFor: string;
    public PostLoadInfo: LoadInfo;
    public IsDRExists: boolean;
    public IsDRMissed: boolean;
    public isRecurringSchedule: boolean;
    public RecurringScheduleId: string;
    public ScheduleQuantityType: number;
    public RecurringScheduleInfo: string;
    public ScheduleQuantityTypeText: string;
    public RouteInfo: RouteInfoDetails;
    public Compartments: CompartmentInfo[];
    public DeliveryRequestFor: DeliveryRequestFor
    public IsFilldInvoke: boolean;
    public Notes: string;
    public IsRetainFuelLoaded: boolean;
    public DeliveryWindow: string;
    public DelReqSource: number;
    public IsPreloadDisable: boolean;
    public IsSpiltDRIconVisible: boolean;
    public GroupParentDRId: string;
    public BlendParentProductTypeId: number;
    public GroupChildDRs: string[];
    public ProductSequence: number;
    public IsBrokered: boolean;
    public TrailerTypes: DropdownItem[];
    public IsAcceptNightDeliveries: boolean = false;
    public LoadQueueAttributes: LoadQueueAttributes;
    public DRQueueAttributes: DRQueueAttributes;
    public HoursToCoverDistance: string;
    public CustomerBrandId: string;
    public IsJobFilter: boolean = false;
    public IsMaxFillAllowed: boolean = false;
    public CarrierRejected: string;
    public CurrentCarrier: string;
    public UpcomingCarrier: string;
    public IsBrokeredCarrierDR: boolean = false;
    public CurrentInventory: string;
    public Ullage: string;
    public AssignedOn: Date;
    public StringAssignedOn: string;
    public IsTBD: boolean;
    public TBDGroupId: string;
    public FuelTypeId: number;
    public FuelType: string;
    public TBDLocations: any[] = [];
    public IsSpiltDRAdded: boolean = false;
    public SpiltDRs: SpiltDRsModel[] = [];
    public OrderType: number = 0;
    public DeliveryDateStartTime: string;
    public Vessel: string;
    public IsMarine: boolean = false;
    public BlendDrScheduleStatus: number;
    public SelectedDate: string;
    public IsFutureDR: boolean = false;
    public IsCalendarView: boolean = false;
    public ScheduleStartTime: Date;
    public ScheduleEndTime: Date;
    public Sap_OrderNo: string;
    public UniqueOrderNo: string;
    public CreditApprovalFilePath: string;
}

export class CompartmentModel {
    public TrailerId: string;
    public CompartmentId: string;
    public Quantity: number;
}

export class RegionDetailModel {
    constructor() {
        this.Drivers = [];
        this.Trailers = [];
        this.Shifts = [];
    }
    public Id: string;
    public Drivers: DriverAdditionalDetailModel[]
    public Trailers: TrailerViewModel[]
    public Shifts: ShiftModel[]
}

export class ShiftModel {
    public Id: string;
    public Name: string;
    public CompanyId: number;
    public StartTime: string;
    public DisplayStartTime: string;
    public EndTime: string;
    public DisplayEndTime: string;
    public CreatedOn: string;
    public CreatedBy: number;
    public IsDeleted: boolean;
    public IsActive: boolean;
}

export class ScheduleBuilderModel {
    constructor() {
        this.Shifts = [];
        this.Trailers = [];
    }
    public Id: string;
    public Date: string;
    public RegionId: string;
    public ObjectFilter: ObjectFilter;
    public RegionFilter: RegionFilter;
    public DateFilter: DateFilter;
    public DSBFilter: number;
    public IsNoDriverShiftFound: number;
    public Shifts: ScheduleShiftModel[];
    public Trailers: TrailerViewModel[];
    public TimeStamp: number;
    public Status: DeliveryGroupStatus;
    public DeletedTripId: string;
    public DeletedGroupId: number;
    public IsLoadReset: boolean;
    public WindowMode: WindowModeFilter;
    public ToggleRequestMode: QueueFilter;
    public IsDsbDriverSchedule: boolean;
}

export class DSBSaveModel extends ScheduleBuilderModel {
    constructor() {
        super();
        this.Trips = [];
    }
    public Trips: TripModel[];
    public PreloadedDRs: any[] = [];
    public PostloadedDRs: any[] = [];
    public StatusCode: number;
    public StatusMessage: string;
}
export class CalendarScheduleModel {
    public Date: string;
    public RegionId: string;
    public ShiftId: string;
    public DriverRowIndex: number;
    public DriverColIndex: number;
    public DeliveryRequests: DeliveryRequestViewModel[];
}
export class DRDragDropModel extends ScheduleBuilderModel {
    public DeliveryRequests: DeliveryRequestViewModel[];
    public SourceTrip: TripModel;
    public DestinationTrip: TripModel;
    public StatusCode: number;
    public StatusMessage: string;
}

export class SbDriverViewModel extends ScheduleBuilderModel {
    constructor() {
        super();
        this.Shifts = [];
    }
    public Shifts: ScheduleShiftModel[];
}

export class SbTrailerViewModel extends ScheduleBuilderModel {
    constructor() {
        super();
        this.Trailers = [];
    }
    public Trailers: TrailerViewModel[];
}

export class TrailerViewModel {
    public Id: string;
    public TrailerId: string;
    public StartTime: string;
    public EndTime: string;
    public Compartments: number;
    public FuelCapacity: number;
    public TrailerType: string;
    public Shifts: TrailerShiftModel[];
    public IsCollapsed: boolean;
    public IsFilldCompatible: boolean;
    public RetainFuel: any[];
    public OptimizedCapacity: number;
}

export class TrailerShiftModel {
    public ShiftId: string;
    public StartTime: string;
    public EndTime: string;
    public SlotPeriod: number;
    public Trips: TripModel[];
}

export class ScheduleShiftModel {
    constructor() {
        this.Schedules = [];
        this.IsCollapsed = false;
    }
    public Id: string;
    public StartTime: string;
    public EndTime: string;
    public SlotPeriod: number;
    public Schedules: TrailerModel[];
    public IsCollapsed: boolean;
    public IsVisible: boolean = true;
    public OrderNo: number;
}

export class ShiftDetailModel {
    public Id: string;
    public StartTime: string;
    public EndTime: string;
    public SlotPeriod: number;
}

export class FiltersData {
    constructor() { }
    public IsShowCarrierManaged: boolean;
    public SelectedTab: number;
    public SelectedCarrierId: string;
    public SelectedRegionId: string;
    public SelectedCustomerId: string;
    public SelectedLocationId: string;
    public SelectedPrioritiesId: string;
}

export class TrailerModel {
    constructor() {
        this.Drivers = [];
        this.Trailers = [];
        this.Trips = [];
    }
    public AllowDriverChange: boolean;
    public Trips: TripModel[];
    public Drivers: DriverAdditionalDetailModel[];
    public Trailers: TrailerViewModel[];
    public LoadQueueId: string;//mongo object id
    public IsLoadQueueCollapsed: boolean;//is moved to load queue
    public IsColumnSelected: boolean;//in load queue panel
    public IsLoadQueueColumnBlocked: boolean;
    public IsIncludeAllRegionDriver: boolean;
    public LoadQueueColumnStatus: boolean;
    public IsDriverScheduleExists: boolean;
    public DriverRowIndex: number;
}

export class DriverAdditionalDetailModel {
    public Id: number;
    public Name: string;
    public IsFilldCompatible: boolean;
    public Shifts: string;
}

export class TripModel {
    constructor() {
        this.DeliveryRequests = [];
        this.Terminal = new DropdownItem();
        this.BulkPlant = new DropAddressModel();
        this.Drivers = [];
        this.Trailers = [];
    }
    public TripId: string;
    public GroupId: number;
    public StartDate: string;
    public DeliveryRequests: DeliveryRequestViewModel[];
    public StartTime: string;
    public EndTime: string;
    public LoadCode: string;
    public RouteInfo: string;
    public SupplierSource: string;
    public Carrier: string;
    public TimeStamp: number;
    public DriverRowIndex: number;
    public DriverColIndex: number;
    public TrailerRowIndex: number;
    public TrailerColIndex: number;
    public ShiftIndex: number;
    public TripStatus: TripStatus;
    public ShiftId: string;
    public ShiftStartTime: string;
    public ShiftEndTime: string;
    public SlotPeriod: number;
    public TripPrevStatus: TripStatus;
    public DeliveryGroupStatus: DeliveryGroupStatus;
    public DeliveryGroupPrevStatus: DeliveryGroupStatus;
    public PickupLocationType: PickupLocationType;
    public IsCommonPickup: boolean;
    public Terminal: DropdownItem;
    public BulkPlant: DropAddressModel;
    public Drivers: DropdownItem[];
    public Trailers: TrailerViewModel[];
    public IsEditable: boolean;
    public DriverScheduleMappingId?: string;
    public BadgeNo1: string;
    public BadgeNo2: string;
    public BadgeNo3: string;
    public IsCommonBadge: boolean;
    public isRecurringSchedule: boolean;
    public RecurringScheduleId: string;
    public ScheduleQuantityType: number;
    public ScheduleQuantityTypeText: string;
    public IsTrailerExists: boolean;
    public IsPreLoadInfo: boolean = false;
    public IsPreloadDisable: boolean = false;
    public IsDriverScheduleExists: boolean;
    public IsIncludeAllRegionDriver: boolean;
    public IsDispatcherDragDropSequence: boolean = false;
    public IsDispatcherDragDropSequenceModified: boolean = false;
}

export class DropAddressModel {
    constructor() {
        this.State = new DropdownItem();
        this.Country = new DropdownItem();
        this.CountryGroup = new DropdownItem();
    }
    public Address: string;
    public City: string;
    public State: DropdownItem;
    public Country: DropdownItem;
    public CountryGroup: DropdownItem;
    public ZipCode: string;
    public CountyName: string;
    public Latitude: number;
    public Longitude: number;
    public SiteName: string;
    public SiteId: number;
}

export class OrderPickupDetailModel {
    public OrderId: number;
    public OrderName: string;
    public PoNumber: string;
    public TerminalName: string;
    public TerminalId: number;
    public PickupLocationType: number = 1;
    public BulkplantName: string;
    public SiteId: number;
    public Address: string;
    public City: string;
    public StateCode: string;
    public StateId: number;
    public CountryCode: string;
    public ZipCode: string;
    public Latitude: number;
    public Longitude: number;
    public CountyName: string;
    public TimeZoneName: string;
    public TruckLoadType: number;
    public DRNote: string;
    public UoM: string;
    public Badge1: string;
    public Badge2: string;
    public Badge3: string;
}

export class OrderPickupLocationModel {
    public PickupLocationType: PickupLocationType;
    public Terminal: DropdownItem;
    public BulkPlant: DropAddressModel;
    public static ToLocation(orderPickupDetail: OrderPickupDetailModel): OrderPickupLocationModel {
        let location = new OrderPickupLocationModel();
        location.PickupLocationType = orderPickupDetail.PickupLocationType;
        location.Terminal = {
            Id: orderPickupDetail.TerminalId,
            Name: orderPickupDetail.TerminalName,
            Code: ''
        };
        location.BulkPlant = {
            Address: orderPickupDetail.Address,
            City: orderPickupDetail.City,
            State: { Id: orderPickupDetail.StateId, Code: orderPickupDetail.StateCode, Name: null },
            Country: { Id: 0, Code: orderPickupDetail.CountryCode, Name: null },
            CountryGroup: { Id: 0, Code: "", Name: null },
            ZipCode: orderPickupDetail.ZipCode,
            CountyName: orderPickupDetail.CountyName,
            Latitude: orderPickupDetail.Latitude,
            Longitude: orderPickupDetail.Longitude,
            SiteName: orderPickupDetail.BulkplantName,
            SiteId: null
        };
        return location;
    }
}

export class WhereIsMyDriverModel {
    public Id: number;
    public Name: string;
    public Intl: string;
    public PhNo: string;
    public Lat: number;
    public Lng: number;
    public LicNo: string;
    public LdPri: number;
    public RgId: string;
    public UniqueOrderNo: string;
    public RgName: string;
    public RgStates: DropdownItem[];
    public StId: number;
    public StName: string;
    public PoNum: string;
    public Pckup: string;
    public Loc: string;
    public dLat: number
    public dLng: number
    public PrdtNm: string;
    public Qty: string;
    public LdDate: string;
    public SttsId: number;
    public Status: string;
    public DrId: string;
    public statusColor: string;
    public DROPTicketNum: string;
    public ListDROPTicketNum: [];
    public TrailerDisplayId: string;
    public routeShow: boolean = false;
    public AppLastUpdatedDate: string;
    public IsOnline: number;
    public AllowCustomerDriverChat: boolean;
    InventoryDataCaptureTypeName?: string;
    public FuelRetainCount: number;
}

export class DistatcherRegionModel {
    public Id: string;
    public Name: string;
    public States: DropdownItem[];
    public Dispatchers: DropdownItem[];

}

export interface JobDeliveryRequest {
    Id: string;
    TfxProductType: string;
    TfxUoM: number;
    RequiredQuantity: number;
    Priority: number;
    DeliveryReqPriority: string;
    Status: number;
    DeliveryReqStatus: string;
    StorageTypeId: string;
    StorageId: string;
    CreatedRegionId?: any;
    TfxJobId: number;
}

export interface Country {
    Id: number;
    Code: string;
    Name: string;
}

export interface State {
    Id: number;
    Name: string;
}

export interface City {
    Id: number;
    Name: string;
}

export interface Priority {
    Id: number;
    Name: string;
}

export interface Product {
    Id: number;
    Name: string;
}

export interface SelectedItem {
    Id: number;
    Name: string;
}

export interface Customer {
    Name: string;
    Id: number;
}

export interface vessel {
    Name: string;
    Id: number;
}
export class Filter {
    'state': State[] = [];
    'city': City[] = [];
    'product': SelectedItem[] = [];
    'priority': Priority[] = [];
    'customer': Customer[] = [];
    'supplier': SelectedItem[] = [];
    'carrier': SelectedItem[] = [];
    'fuelType': SelectedItem[] = [];
}

export interface JobAssetDetail {
    AssetId: number;
    JobId: number;
    AssetName: string;
    AssetType: number;
    AssetTypeName: string;
    FuelCapacity: number;
    ProductType: string;
    TankType: number;
    TankTypeName: string;
    MinFill: number;
    MaxFill: number;
    ThresholdDeliveryRequest: number;
    DipTestMethod: number;
    DipTestMethodName: string;
    jobTankAdditionalDetails: JobTankAdditionalDetails[];
    TfxProductTypeName: string;
    LastReading: string;
    CaptureTime: string;

}

export interface JobTankAdditionalDetails {
    TfxAssetId: number;
    TankId: string;
    TankName: string;
    TankNumber: string;
    StorageId: string;
    ThresholdDeliveryRequest: number;
    FillType: number;
    MaxFill: number;
    MinFill: number;
    MaxFillPercent: number;
    MinFillPercent: number;
    FuelCapacity: number;
    FillTypeStatus: string;
    DipTestMethod: number;
    DipTestMethodName: string;
    ManiFolded: number;
    ManiFoldedName: string;
    TfxProductTypeName?: any;
    SiteId: string;
    LastReading: number;
    CaptureTime: Date;
    dipChartDetails: any[];
    TankChartPath: string;
}


export interface JobLocationDetailsModal {
    CustomerName: string;
    CustomerId: number;
    JobID: number;
    JobName: string;
    JobLocationType: number;
    JobLocationTypeName: string;
    Latitude: string;
    Longitude: string;
    Address: string;
    CountryCode: string;
    City: string;
    CityId: number;
    State: string;
    StateID: number;
    ZipCode: string;
    ContractNumber: string;
    IsContactDetailsAvailable: number;
    ContactPersonName: string;
    ContactPhoneNumber: string;
    IsPhoneNumberConfirmed: number;
    SiteImageFilePath: string;
    SiteAvailabilityArray: string[];
    SiteAvailabilityTotalDays: number;
    SiteAvailabilityTiming: string;
    SiteInstructions: string;
    TankCount: number;
    AssetCount: number;
    TotalCount: number;
    jobDeliveryRequests: JobDeliveryRequest[];
    jobAssetDetails: JobAssetDetail[];
    FuleTypeID?: any;
    FuelTypeName: string;
    FuelTypeNameList: string[];
    ScheduleStatus: string;
    highestPriority: number,
    iconUrl: string;
    RegionId: string;
    CarrierDetails: DropdownItem[];
}


export interface JobDetailsWithOrders {
    OrderId: number;
    JobId: number;
    FuelTypeId: number;
    PoNumber: string;
    DisplayJobID: string;
    JobName: string;
    Address: string;
    City: string;
    UoM: number;
    CompanyId: number;
    CompanyName: string;
    BadgeNo1: string;
    BadgeNo2: string;
    BadgeNo3: string;
}

export interface JobLocationData {
    jobLocationDetails: JobLocationDetailsModal[];
    citiesDetails: City[];
    stateDetails: State[];
    customerDetails: Customer[];
    fuelTypeDetails: string[];
}

export class SbFilterModel {
    constructor() {
        this.Drivers = [];
        this.Trailers = [];
        this.Pickups = [];

        this.SelectedDrivers = [];
        this.SelectedPickups = [];
        this.SelectedTrailers = [];
    }
    public Drivers: DriverAdditionalDetailModel[];
    public Trailers: TrailerViewModel[];
    public Pickups: DropdownItem[];

    public SelectedPickups?: DropdownItem[];
    public SelectedDrivers?: DriverAdditionalDetailModel[];
    public SelectedTrailers?: TrailerViewModel[];
}

export class TrailerViewFilterModel {
    public Shifts?: any = {};
    public Trailers?: any = {};
    public Pickups?: any = {};
    public Drivers?: any = {};
}

export class DriverViewFilterModel {
    public Shifts?: any = {};
}
export class CompanyUsers {
    FirstName: string;
    LastName: string;
    FullName: string;
    PhoneNumber: string;
    IsPhoneNumberConfirmed: boolean;
    EmailAddress: string;
    SendbirdUserName: string;
    UserName: string;
    UserId: number;
    Role: number;
    RegionID: string;
    SendBirdRegionID: string;
    RegionName: string;
    RegionDescription: string;
    //sendbird propery
    ProfileURL: string;

}

export class TankMinMaxFill {
    MinFill: number;
    MaxFill: number;
    MinFillPercent: number;
    MaxFillPercent: number;
}

export class TankChartHeight {
    ShouldBeEmptyPercent: number;
    ShouldBeFilledPercent: number;
    CurrentInventoryPercent: number;
    sbf_percent: number;
    ci_percent: number;
}

export class DipTest {
    TankId: string
    SiteId: string
    Ullage: number
    GrossVolume: number
    NetVolume: number
    CaptureTime: string
    CaptureTimeString: string
}

export interface Compartment {
    CompartmentId: string;
    FuelType: number;
    Capacity: number;
}

export interface Trailer {
    Id: string;
    Name: string;
    Owner?: any;
    TruckId: string;
    FuelCapacity: number;
    ContractNumber: string;
    Compartments: Compartment[];
    TfxCreatedBy: number;
    TfxCompanyId: number;
    CreatedDate: Date;
    Status: number;
    LicenceRequirement: number;
    LicencePlate: string;
    ExpirationDate: string;
    IsPump: string;
    TrailerType: number;
    DefaultUOM: number;
    IsDeleted: boolean;
    TrailerFuelRetains: TrailerFuelRetainViewModel[];
    OngoingData: WhereIsMyDriverModel[];
}

export class TrailerFuelRetainViewModel {
    public Id: string
    public TrailerId: string
    public CompartmentId: string
    public CompartmentCapacity: number;
    public Quantity: number;
    public UOM: number;
    public DeliveryRequestId: string
    public ProductType: string
    public ProductId: number;
    public OrderId: string
    public CreatedDate: string
    public UpdatedOn: string
    public TfxDriverId: number;

}

export interface IDriverAdditionalDetails {
    Id: string;
    Name: string;
    License: string;
    ContactNumnber: string;
    Shifts: string[];
    Trailers: Trailer[];
}

export class DriverAdditionalDetails {
    Id: string;
    Name: string;
    License: string;
    ContactNumnber: string;
    Shifts: string[];
    Trailers: Trailer[];
    constructor(data?: IDriverAdditionalDetails) {
        this.Id = data && data['Id'] || null;
        this.Name = data && data['Name'] || null;
        this.License = data && data['License'] || null;
        this.ContactNumnber = data && data['ContactNumnber'] || null;
        this.Shifts = data && data['Shifts'] || [];
        this.Trailers = data && data['Trailers'] || [];
    }
}

export const routesColor = {
    1: '#5f4aa8',
    11: '#c4c105',
    12: '#d3950f',
    18: '#19953f',
    20: '#e3584d'
}

export class DemandModel {
    Id: number;
    JobId: number;
    JobName: string;
    Level: number;
    NetVolume: number;
    OrderId: number
    Priority: DeliveryReqPriority;
    ProductName: string;
    ProductType: string;
    SiteId: string;
    StorageId: string;
    TankCapacity: number;
    TankId: string;
    TankMaxFill: number
    IsMaxFillAllowed: boolean = false;
    //IsReAssignToCarrier: boolean = false;
    TankMinFill: number
    TankName: string;
    Ullage: number;
    UoM: string;
    RequiredQuantity: number;
    IsDRExists: boolean;
    IsDRMissed: boolean;
    BuyerCompanyName: string;
    ExistingDR: PartialDRDetail[];
    RecurringDRSchedule: RecurringDRSchedule[];
    isRecurringSchedule: boolean = false;
    ScheduleQuantityType: number = 0;
    ScheduleQuantityTypeText: string;
    DeliveryReqId: string;
    CarrierStatus: number;
    DeliveryRequestFor: DeliveryRequestFor;
    IsEndSupplier: boolean = false;
    IsDispatchRetained: boolean = false;
    Notes: string;
    GroupParentDRId: string;
    ProductSequence: number;
    CurrentThreshold: number;
    BadgeNo1: string;
    BadgeNo2: string;
    BadgeNo3: string;
    DispatcherNote: string;
    Terminal: DropdownItem;
    BulkPlant: DropAddressModel;
    PickupLocationType: number;
    public TrailerTypes: DropdownItem[];
    public IsAcceptNightDeliveries: boolean = false;
    public LoadQueueAttributes: LoadQueueAttributes;
    public DRQueueAttributes: DRQueueAttributes;
    public HoursToCoverDistance: string;
    public ScheduleStartTime: Date;
    public ScheduleEndTime: Date;
}

export class PartialDRDetail {
    DeliveryReqPriority: DeliveryReqPriority;
    ScheduleStatusName: string;
    ScheduleStatusId: number;
    RequiredQuantity: number;
    CreatedOn: string;
    CreatedDate: string;
    Id: string;
    IsRecurringSchedule: boolean;
}

export class LoadInfo {
    ShiftId: string;
    ShiftIndex?: number;
    ScheduleIndex: number;
    TripIndex: number;
    DrId: string;
}

export class PreLoadDrViewModel {
    public SbView: number;
    public RegionId: string;
    public ShiftEndDate: string
    public ShiftId: string
    public PreloadTrailers: TrailerModel[];
    public PreloadDrivers: DropdownItem[];
    public IsTrailerExists: boolean;
    public PreloadDrs: any[];
    public SbDsbView: number;
}

export class PreLoadDrResponseViewModel {
    public StatusCode: number;
    public StatusMessage: string;
    public PreloadDrs: PreLoadDrModel[];
}

export class PreLoadDrModel {
    public Id: string;
    public PreLoadedForId: string;
}
export class RecurringDRSchedule {
    public Id: string;
    public ScheduleType: number;
    public WeekDayId: string[];
    public MonthDayId: number;
    public Date: string;
    public ScheduleQuantityType: number;
    public RequiredQuantity: number;
    public OrderId: number;
    public SiteId: number;
    public JobId: number;
    public TfxSupplierCompanyId: number;
    public TfxCompanyName: string;
    public TfxUserId: number;
    public AssignedToCompanyId: number;
}

export class UnAssignDriverFromShiftModel {
    public sbId: string;
    public DriverRowIdx: number;
    public Trips: TripModel[];
    public Drivers: DropdownItem[];
    public Trailers: TrailerViewModel[];
    public TimeStamp: number;
    public shiftIdx: number;

}

export class SalesDataModel {
    public CompanyId?: number;
    public CompanyName?: string;
    public SiteId?: string;
    public TankId?: string;
    public StorageId?: string;
    public TankName?: string;
    public AvgSale?: string;
    public PrevSale?: string;
    public WeekAgoSale?: string;
    public Inventory?: string;
    public Ullage?: string;
    public LastDeliveryDate?: string;
    public LastDeliveredQuantity?: string;
    public DaysRemaining?: string;
    public Status?: string;
    public Priority: DeliveryReqPriority;
    public TfxJobId: number;
    public ProductTypeId: number;
    public MaxFillQuantity?: number;
    public IsUnknownOrMissing?: boolean;
    public RegionId: string;
    public LastReadingTime: string;
    public LocationManagedType: number;
    public UOM: number;
}

export class DeliveryDetailsModel {
    public TrackableScheduleId: number;
    public ScheduleDate: string;
    public ScheduleTime: string;
    public Driver: string;
    public Carrier: string;
    public Quantity: number;
    public QuantityTypeId: number;
    public QuantityTypeName: string;
}

export class DeliveryRequestBrokerInfoViewModel {
    //single DR
    public OrderId: number;
    public CarrierCompanyId: number;
    public DispatcherNote: string;
    public CarrierRegionId: string;
    public DeliveryRequest: DeliveryRequestViewModel;
    public IsDispatchRetainedByCustomer: boolean;
    //multiple dr
    public BrokerDrModel: BrokerDrModel[];
    public ScheduleBuilderId: string;
    //carriers
    public CarrierInfo: TfxCarrierDropdownDisplayItem[];
    public CarrierInfoJson: string;
    public BlendedGroupId: string;
    public UniqueOrderNo: string;
}

export class BrokerDrModel {
    public OrderId: number;
    public CarrierCompanyId: number;
    public CarrierRegionId: string;
    public DeliveryRequest: DeliveryRequestViewModel;
    public IsDispatchRetainedByCustomer: boolean;
    public BlendedGroupId: string;
    public IsBlendedGroupProduct: boolean = false;
    public UniqueOrderNo: string;
}

export class CompartmentInfo {
    public TrailerId: string;
    public CompartmentId: string;
    public Quantity: number;
    public RetainInfo: RetainInfo
}

export class TrailerCompartment {
    constructor() {
        this.Compartments = [];
    }
    public TrailerId: string;
    public IsFuelRetain: boolean;
    public IsCompartmentAvailable: boolean;
    public Compartments: CompartmentInfo[];
}

export class RetainInfo {
    public OrderId: number;
    public ProductId: number;
    public DeliveryReqId: string;
    public TfxTerminal: DropdownItem;
    public TfxBulkPlant: DropAddressModel;
    public PickupLocationType: PickupLocationType;
    public Quantity: number;
}

export class TrailerRetainCompartment {
    public TrailerId: string;
    public CompartmentId: string;
    public Qty: number;
}

export class ForecastingTankViewModel {
    public TankName: string;
    public TankFill: number;
    public TankFillRemaining: number;
    public LastInventoryReading: string;
    public UllageSinceLastReading: string;
    public LastReadingTime: string;
    public EstimatedCurrentInventory: string;
    public DeliverySinceLastReading: string;
    public DaysLeft: number;
    public ProductType: string;
    public RegionId: string;
    public SiteId: string;
    public TankId: string;
    public StorageId: string;
    TankInventoryDiffinHrs?: number;
    public TfxProductTypeId: number;
    public Status: string;
    public MaxFillQuantity: string;
    public AvgSale: string;
    public PrevSale: string;
    public WeekAgoSale: string;
    public SiteInstructions: string;
    public LocationManagedType: number;
}

export class ForecastingInventoryViewModel {
    public TankName: string;
    public InventoryLevel: number;
    public InventoryLevelQty: string;
    public Ullage: string;
    public PrevInventoryReading: string;
    public SafetyStockQty: string;
    public SafetyStock: number;
    public RunOutLevel: number;
    public RunOutLevelQty: string;
    public PhysicalPumpStop: number;
    public PhysicalPumpStopQty: string;
}

export class ForecastingEstimatedUsageViewModel {
    public StartDate: string;
    public EndDate: string;
    public TankName: string;
    public UsagePeriod: string;
    public AverageBusinessDayUsage: string;
    public TotalExceptedUsage: string;
    public MaximumBusinessdayUsage: string;
}

export class ForecastingDeliveryViewModel {
    public TankName: string;
    public NoOfDeliveries: number;
    public LastDeliveredQty: string;
    public LastDeliveredDate: string;
}

export class ForecastingExistingScheduleViewModel {
    public TankName: string;
    public ExistingDeliverySchedule: number;
    public DeliveryRequest: number;
}

export class ForecastingTankChartViewModel {
    public XAxisTimeSpan: string[];
    public TankDetailsForChart: TankDetailsForChartModel[];
    public TankLevels: TankLevelModel[];
}

export class TankDetailsForChartModel {
    public TankName: string;
    public Data: number[];
}

export class TankLevelModel {
    public Time: string;
    public Quantity: number;
    public Type: number;
}

export class OttoDeliveryRequests {
    public Date: string;
    public ShiftTime: string;
    public Priority: number;
    public CustomerName: string;
    public ProductName: string;
    public Quantity: string;
    public QuantityUOM: string;
    public TankName: string;
    public LocationName: string;
    public TankCapacity: string;
}

export class OttoScheduleDetails {
    public ShiftId: string;
    public ShiftName: string;
    public OttoDeliveryRequests: OttoDeliveryRequests[];
}

export class OttoBuilder {
    constructor() {
        this.Loads = [];
    }
    public Date: string;
    public RegionId: string;
    public ShiftId: string;
    public Loads: OttoTripModel[];
}

export class OttoTripModel {
    public StartTime: string;
    public EndTime: string
    public DriverRowIndex: number;
    public DriverColIndex: number;
    public DeliveryRequests: DeliveryRequestViewModel[];
}

export class ShiftViewModel {
    public Id: string;
    public Name: string;
    public CompanyId: number;
    public RegionId: string;
    public StartTime: string;
    public EndTime: string;
    public RegionName: string;
    public ShiftInfo: string;
    public StartTimespan: number;
}

export class OttoNotifications {
    public Id: string;
    public TfxJobId: number;
    public Message: string;
    public ScheduleBuilderId: string;
    public Status: string;
}
export class SplitDeliveryRequestModel {
    public ParentDRId: string;
    public RequiredQtyDetails: [];
}
export class RequiredQtyDetails {
    public RequiredQty: number;
}
export class BlendDRDetails {
    //Marine Nomination
    public DeliveryDateStartTime: string;
    public TrackScheduleStatusName: string;
    public Vessel: string;
    public Berth: string;
    //added for dr carrier assignment
    public Id: string;
    public ProductName: string;
    public RequiredQuantity: number;
    public PickupLocationType: number;
    public Address: string;
    public City: string;
    public Code: string;
    public ZipCode: string;
    public PickupLocationName: string;
    public IsAdditive: boolean;
    public IsBlendedRequest: boolean;
    public Priority: number;
    public StatusClassId: number;
    public IsBlinkDR: boolean;
    public ScheduleStartTime: Date;
    public ScheduleEndTime: Date;


}
export class ProductDeliveryScheduleInfo  {
    public LocationName: string;
    public CustomerName: string;
    public Status: number;
    public StatusClassId: number;
    public Priority: number;
    public IsBlinkDR: boolean;
    public BlendGroupId: string;
    public ScheduleQuantityType: number;
    public UoM: number;
    public IsAutoCreatedDR: boolean;
    public DelReqSource: number;
    public isRecurringSchedule: boolean;
    public IsCommonPickup: boolean;
    public CommonPickupLocationType: number;
    public IsFilldInvoke: boolean;
    public BadgeNo1: string;
    public BadgeNo2: string;
    public BadgeNo3: string;
    public BadgeNoInfo: string;
    public TankMaxFill: number;
    public RouteName: string;
    public isPreload: boolean = false;
    public isPostload: boolean = false;
    public TripIndex: number;
    public IsSelectedForAssignment: boolean;
    public ScheduleQuantityTypeText: string;
    public IsMarine: boolean = false;
    public DrInfo: BlendDRDetails[] = [];
    public Address: string;
    public City: string;
    public Code: string;
    public ZipCode: string;
    public PickupLocationName: string;
    public ScheduleStartTime: Date;
    public ScheduleEndTime: Date;
}

export class ShiftColumnInfo {
    public ColIndex: number;
    public ColIndexName: string;
}
export class ShiftLoadInfo {
    public LoadIndex: number;
    public LoadName: string;
}
export class TransferDSInfo {
    public dragData: DragDSInfo;
}
export class DragDSInfo {
    public Data: any;
    public TripFrom: any;
    public DrIndex: any;
    public ShiftIndex: any;
    public RowIndex: any;
    public ColIndex: any;
    public Schedule: any;
    public DropTrip: any;
    public DropSchedule: any;
}
export class DRQueueAttributes {
    public CustomerName: boolean;
    public DeliveryShift: boolean;
    public TrailerCompatibility: boolean;
    public HoursToCoverDistance: boolean
}
export class LoadQueueAttributes {
    public LocationName: boolean;
    public CustomerName: boolean
    public Driver: boolean
    public TrailerName: boolean;
}
export class SbGridViewFilterModel {
    public SearchLocation: string = "";
}

export class DRReportFilterViewModel {
    public Regions: DropdownItemExt[] = [];
    public Locations: DropdownItem[] = [];
}

export class DeliveryRequestReportGridModel {

    public Location: string;
    public RegionName: string;
    public CustomerName: string;
    public CustomerBrandID: string;
    public ProductType: string;
    public RequestedQuantity: number;
    public LocationId: string;
    public PoNumber: string;
    public DrId: string;
    public TfxJobId: number;
    public Priority: number;
    public RegionId: string;
    public IsRecurringSchedule: boolean;
    public IsAutoDR: boolean;
    public UoM: string;

}

export class DRReportFilterInputViewModel {
    RegionIds: string;
    LocationIds: string;
    FromDate: string;
    ToDate: string;
}

export class OrderPartialDetailModel {
    Id: number;
    Code: string;
    Name: string;
    FuelTypeId: number;
    JobId: number;
    DRNote: string;
}
export class OptionalPickupDetailModel {
    public incId: number;
    public Id: string;
    public RegionId: string;
    public ScheduleBuilderId: string;
    public ShiftId: string;
    public ShiftIndex: number;
    public ShiftOrderNumber: number;
    public DriverColIndex: number;
    public TfxFuelTypeId: number;
    public TfxFuelTypeName: string;
    public isAdded: number;
    public DriverId: number = 0;
    public DSBPickupLocationInfo: OptionalPickupLocationInfo = new OptionalPickupLocationInfo();

}
export class OrderFuelInfo {
    public StatusCode: number;
    public OrderFuelDetails: OrderFuelDetails[] = [];
}
export class OrderFuelDetails {
    public OrderId: number;
    public FuelTypeDetails: DropdownItem[] = [];
}
export class OptionalPickupLocationInfo {
    public PickupLocationType: number;
    public TfxTerminal: DropdownItem = new DropdownItem();
    public TfxBulkPlant: DropAddressModel = new DropAddressModel();
    public BadgeNo1: string;
    public BadgeNo2: string;
    public BadgeNo3: string;
    public BadgeNoInfo: string;
}
export class RaiseTBDDeliveryRequest {
    public DeliveryRequests: TBDRaiseDRDeliveryRequests[] = [];
}
export class TBDRaiseDRDeliveryRequests {
    public ScheduleQuantityType: number;
    public RequiredQuantity: number;
    public FuelTypeId: number;
    public FuelType: string;
    public Priority: number;
    public DelReqSource: number = 1;
    public BadgeNo1: string;
    public BadgeNo2: string;
    public BadgeNo3: string;
    public DispactherNote: string;
    public Notes: string;
    public PickupLocationType: number;
    public Terminal: DropdownItem;
    public Bulkplant: DropAddressModel;
    public IsTBD: boolean = true;
    public TBDGroupId: string;
    public CreatedByRegionId: string;
    public AssignedToRegionId: string;
    public ProductTypeId: number;
    public ProductType: string;
    public UoM: number;
    public DeliveryLevelPO: string;
}
export class RegionDSBModel {
    RegionId: string;
    DSBShiftInfo: ShiftViewModel[];
}
export class SpiltDRsModel {
    public ScheduleQuantityType: number;
    public RequiredQuantity: number;
}
export class CancelDeliverySchedule {
    ScheduleBuilderId: string;
    DriverId: number;
    ShiftIndex: number;
    ShiftId: string;
    DriverRowIndex: number;
    DriverColIndex: number;
    DeliveryReqId: string;
    TrackableScheduleId: number;
    ScheduleStatus: number;
    TrackScheduleStatus: number;
    TrackScheduleStatusName: string;
    StatusClassId: number;
}
export class CancelDSDeliveryScheduleInfo {
    TfxCompanyId: number;
    RegionId: string;
    CancelDSDeliverySchedules: CancelDSDeliverySchedule[] = [];
}
export class CancelDSDeliverySchedule {
    DeliveryReqId: string;
    IsSubDR: boolean = false;
    IsPreLoadDR: boolean = false;
}
export class CancelDSDeliveryScheduleViewModel {
    IsChecked: boolean = false;
    ScheduleBuilderDate: string;
    ScheduleBuilderId: string;
    DriverId: number;
    DriverName: string;
    Quantity: string;
    FuelType: string;
    CurrentState: string;
    ShiftId: string;
    ShiftIndex: number;
    DriverRowIndex: number;
    DriverColIndex: number;
    DeliveryReqId: string;
    TrackableScheduleId: number;
    IsPreLoadDS: boolean = false;
}

export class SubDRStatus {
    GroupParentDRId: string;
    DeliveryScheduleStatusId: number
}
export class ResetDeliveryGroupScheduleModel {
    public ScheduleBuilderId: string;
    public DeliveryRequestIds: string[];
}
export class DispatcherMapSeq {
    JobId: number;
    IsTBD: boolean = false;
    TBDGroupId: string;
}
