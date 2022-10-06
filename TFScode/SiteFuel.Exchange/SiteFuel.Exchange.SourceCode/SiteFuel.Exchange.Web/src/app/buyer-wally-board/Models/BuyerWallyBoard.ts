import { DateFilter, DeliveryGroupStatus, DeliveryReqPriority, ObjectFilter, PickupLocationType, QueueFilter, RegionFilter, TripStatus } from 'src/app/app.enum';
import { DropdownItem } from 'src/app/statelist.service';

export class LoadFilterModel {
    public Id: string;
    public Name: string;
    public States: DropdownItem[];
    public Suppliers: DropdownItem[];
    public Carriers: DropdownItem[];
}

export class DipTestViewModel {
    public SiteName: string;
    public CompanyName: string;
    public Id: number;
    public SiteId: string;
    public TankId: string;
    public TankName: string;
    public StorageId: string;
    public Level: number;
    public Ullage: number;
    public GrossVolume: number;
    public NetVolume: number;
    public WaterNetLevel: number;
    public WaterGrossLevel: number;
    public CaptureTime: string;
    public ProductName: string;
    public DataSourceTypeId: number;
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
    public TankMinFill: number;
    public TankMaxFill: number;
    public CurrentThreshold: number;
    public FillType: number;
    public ReorderPercent: number;
    public OrderId: number;
    public ReorderQuantity: number;
    public Priority: DeliveryReqPriority;
}

export class TankCapacityForDR {
    public Priority: DeliveryReqPriority;
    public MaxPercent: number;
    public MinPercent: number;
}

export class ModifiedTripInfo {
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
    Jobs: DropdownItem[];
}

export class PartialDRDetails {
    public Priority: DeliveryReqPriority;
    public ScheduleStatusName: string;
    public RequiredQuantity: number;
    public CreatedOn: string;
}


export enum TfxModule {
    None,
    SupplierWallyboardLocation,
    SupplierWallyboardLoads,
    SupplierWallyboardSales,
    SupplierWallyboardSalesPriority,
    SupplierWallyboardSalesTanks,
    BuyerWallyboardLocation,
    BuyerWallyboardLoads,
    BuyerWallyboardSales,
    BuyerWallyboardSalesPriority,
    BuyerWallyboardSalesTanks,
    BuyerWallyboardSalesLocation,
    AssignedByMeDeliveryRequests,
    DSBShift
}

export enum WindowModeFilter {
    Single = 1,
    Multi = 2
}

export enum UoM {
    None = 0,
    Gallons = 1,
    Litres = 2
}

export class DeliveryRequestViewModel {
    constructor() {
        this.Terminal = new DropdownItem();
        this.BulkPlant = new DropAddressModel();
        this.PickupLocationType = 1;
        this.WindowMode = 1;
        this.QueueMode = 1;
    }
    public Id: string;
    public JobId: number;
    public JobAddress: string;
    public JobCity: string;
    public JobName: string;
    public ProductType: string;
    public FuelTypeId: number;
    public FuelType: string;
    public SiteId: string;
    public UoM: number;
    public RequiredQuantity: number;
    public Priority: number;
    public AssignedToCompanyId: number;
    public CreatedByCompanyId: number;
    public SupplierCompanyId: number;
    public Status: number;
    public PreviousStatus: number;
    public ScheduleStatus: number;
    public SchedulePreviousStatus: number;
    public OrderId: number;
    public CreatedByRegionId: string;
    public AssignedToRegionId: string;
    public PickupLocationType: number;
    public Terminal: DropdownItem;
    public BulkPlant: DropAddressModel;
    public IsDeleted: boolean;
    public DeliveryGroupId: number;
    public DeliveryScheduleId: number;
    public TrackableScheduleId: number;
    public ParentId: string;
    public CustomerCompany: string;
    public WindowMode: number;
    public QueueMode: number;
    public TrackScheduleStatus: number;
    public TrackScheduleStatusName: string;
    public TrackScheduleEnrouteStatus: number;
    public StatusClassId: number;
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
    public ProductSequence: number;
}

export class RegionDetailModel {
    constructor() {
        this.Drivers = [];
        this.Trailers = [];
        this.Shifts = [];
    }
    public Id: string;
    public Drivers: DropdownItem[]
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
    public Shifts: ScheduleShiftModel[];
    public Trailers: TrailerViewModel[];
    public TimeStamp: number;
    public Status: DeliveryGroupStatus;
    public DeletedTripId: string;
    public DeletedGroupId: number;
    public IsLoadReset: boolean;
    public WindowMode: WindowModeFilter;
    public ToggleRequestMode: QueueFilter;
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
    public TrailerType: string;
    public Shifts: TrailerShiftModel[];
    public IsCollapsed: boolean;
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
}

export class ShiftDetailModel {
    public Id: string;
    public StartTime: string;
    public EndTime: string;
    public SlotPeriod: number;
}

export class TrailerModel {
    constructor() {
        this.Drivers = [];
        this.Trailers = [];
        this.Trips = [];
    }
    public Trips: TripModel[];
    public Drivers: DropdownItem[];
    public Trailers: TrailerViewModel[];
}

export class LocationFilters {
    public IsShowCarrierManaged: boolean;
    public state: number[] = [];
    public city: number[] = [];
    public product: string[] = [];
    public priority: number[] = [];
    public customer: number[] = [];
    public supplier: SelectedItem[] = [];
    public carrier: SelectedItem[] = [];
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
}

export class DropAddressModel {
    constructor() {
        this.State = new DropdownItem();
        this.Country = new DropdownItem();
    }
    public Address: string;
    public City: string;
    public State: DropdownItem;
    public Country: DropdownItem;
    public ZipCode: string;
    public CountyName: string;
    public Latitude: number;
    public Longitude: number;
    public SiteName: string;
    public SiteId: number;
}

export class OrderPickupDetailModel {
    public OrderId: number;
    public PoNumber: string;
    public TerminalName: string;
    public TerminalId: number;
    public PickupLocationType: number = 1;
    public BulkplantName: string;
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

    public routeShow: boolean = false;
    public AppLastUpdatedDate: string;
    public IsOnline: number;

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

export interface DropDownItem {
    Id: number;
    Name: string;
}

export interface Customer {
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
    MaxFillPercent: number;
    MinFill: number;
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
   // CustomerName: string;
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
    iconUrl: string,
    supplierDetails: SelectedItem[];
    carrierDetails: SelectedItem[];
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
    public Drivers: DropdownItem[];
    public Trailers: TrailerViewModel[];
    public Pickups: DropdownItem[];

    public SelectedPickups?: DropdownItem[];
    public SelectedDrivers?: DropdownItem[];
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
    IsDeleted: boolean;
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
    TankMinFill: number
    TankName: string;
    Ullage: number;
    UoM: string;
    RequiredQuantity: number;
    IsDRExists: boolean;
    IsDRMissed: boolean;
    ExistingDR: PartialDRDetail[];
}

export class PartialDRDetail {
    DeliveryReqPriority: DeliveryReqPriority;
    ScheduleStatusName: string;
    ScheduleStatusId: number;
    RequiredQuantity: number;
    CreatedOn: string;
    CreatedDate: string;
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
    public ShiftIndex: number;
    public ScheduleIndex: number;
    public TripIndex: number;
    public PreloadTrailers: TrailerModel[];
    public PreloadDrs: any[];
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

export interface Supplier {
    Name: string;
    Id: number;
}

export interface Carrier {
    Name: string;
    Id: number;
}

export class SalesTankFilterModal {
    public selectedLocAttributeData:any = [];
}

export class SalesFilterModal {
    public SelectedLocationId: string;
    public selectedLocAttributeId: string;
    
}

