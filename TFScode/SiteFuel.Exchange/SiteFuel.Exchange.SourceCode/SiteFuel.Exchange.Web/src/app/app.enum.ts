
export enum SelectedTabEnum {
    Priority = 1,
    Tanks = 2,
    Location = 3
}

export enum DeliveryReqPriority {
    None = 0,
    MustGo = 1,
    ShouldGo = 2,
    CouldGo = 3,
}

export enum DeliveryReqStatus {
    None = 0,
    Pending = 1,
    Assigned = 2,
    ScheduleCreated = 3,
    Deleted = 4,
    Draft = 5,
    InUse = 6
}

export enum DeliveryGroupStatus {
    None = 0,
    Draft = 1,
    Published = 2,
    Deleted = 3
}

export enum DeliveryRequestTypes {
    None = 0,
    MustGo = 1,
    ShouldGo = 2,
    CouldGo = 3,
    Missed = 4,
    AssignedToMe = 5,
    AssignedByMe = 6,
}

export enum DSBMethodd {
    None = 0,
    Save = 1,
    DriverAssignment = 2,
    Publish = 3,
    Delete = 4,
    DragDrop = 5
}

export enum TripStatus {
    None = 0,
    Added = 1,
    Modified = 2,
    Deleted = 3
}

export enum ObjectFilter {
    None = 0,
    Driver = 1,
    Trailer = 2
}
export enum WindowModeFilter {
    None = 0,
    Single = 1,
    Multi = 2
}
export enum QueueFilter {
    DRs = 1,
    Queued = 2
}
export enum RegionFilter {
    None = 0,
    MyRegion = 1,
    OtherRegion = 2
}

export enum DateFilter {
    None = 0,
    Today = 1,
    Tomorrow = 2,
    Date = 3,
    YesterDay = 4
}

export enum UoM {
    None = 0,
    Gallons = 1,
    Litres = 2,
    Barrels = 3,
    MetricTons = 4
}

export enum DeliveryRequestFor {
    UnKnown = 0,
    Tank = 1,
    Order = 2,
    ProductType = 3
}

export enum PickupLocationType {
    None = 0,
    Terminal = 1,
    BulkPlant = 2
}

export enum BrokeredDrCarrierStatus {
    None = 0,
    Pending = 1,
    Accepted = 2,
    Rejected = 3,
    Recalled = 4
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
    AssignedByMeDeliveryRequests
}

export enum TruckStatus {
    Active = 1,
    InActive = 2,
    UnderMaintenance = 3
}

export enum TrailerType {
    Lead = 1,
    Pup = 2,
    Quad = 3,
    Tandem = 4,
    Tridem = 5
}

export enum LicenceRequirement {
    Class1 = 1,
    Class3 = 2,
}

export enum SelectedMapLabelEnum {
    OnTheWayToTerminal = 11,
    ArrivedAtTerminal = 12,
    OnTheWayToLocation = 1,
    ArrivedAtLocation = 18
}

export enum TrailerTypeEnum {
    Lead = 1,
    Pup = 2,
    Quad = 3,
    Tandem = 4,
    Tridem = 5
}

export enum DeliveryShiftEnum {
    None = 0,
    Morning = 1,
    Evening = 2
}

export class DrFilterPreferencesModel {
    Id: string;
    UserId: number;
    CompanyId: number;
    Date: Date;
    FilterData: string;
    StatusCode: number;
    StatusMessage: string;
    RegionId: string;
}

export enum OrderType {
    ALL = 0,
    FTL = 1,
    LTL = 2,
}

export enum CumulationType {
    Unknown = 0,
    Weekly = 1,
    BiWeekly = 2,
    Monthly = 3,
    SpecificDates = 4
}

export enum WeekDay {
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6,
    Sunday = 7
}

export enum TierPricingType {
    Unknown = 0,
    VolumeBased = 1,
    DeliveryQuantityBased = 2
}

export enum PricingType {
    RackAverage = 1,
    PricePerGallon = 2,
    Tier = 3,
    Suppliercost = 4,
    RackLow = 5,
    RackHigh = 6
}

export enum ColumnStatusEnum {
    None = 0,
    Completed = 1,
    Partial = 2,
    Empty = 3,
    Published = 4,
}

export enum DSBLoadQueueStatus {
    New = 0,
    InProgress,
    Success,
    Failed
}

export enum PanelStatus {
    ShowRouteList = 1,
    ShowForm = 2
}

export enum Exception {
    UnKnowDelivery = 4,
    MissingDelivery = 5
}

export enum WeekDays {
    SUN = 0,
    MON = 1,
    TUE = 2,
    WED = 3,
    THU = 4,
    FRI = 5,
    SAT = 6
}

export enum Status {
    InActive = 0,
    Active = 1
}

export enum DeleteDriverSchedule {
    Single = 1,
    Range = 2,
    Whole = 3
}

export enum FreightRateRuleType {
    Unknown = 0,
    P2P = 1,
    Range = 2,
    Route = 3
}

export enum FuelGroupType {
    Unknown = 0,
    Standard = 1,
    Custom = 2
}

export enum FreightPricingMethod {
        Manual = 1,
        Auto = 2,
}

export enum FreightRateCalcPreferenceType {
    Unknown = 0,
    Round = 1,
    Proportion = 2
}

export enum TableType {
    Unknown = 0,
    Master = 1,
    Customer = 2,
    Carrier = 3
}

export enum PeriodEnum {
    WEEKLY = "WEEKLY",
    MONTHLY = "MONTHLY",
    ANNUALY = "ANNUALY"
}

export enum WeekDays {
    Sun = 0,
    Mon = 1,
    Tue = 2,
    Wed = 3,
    Thu = 4,
    Fri = 5,
    Sat = 6
}

export enum WeekEnum {
    Same_Week = "Same Week",
    Week_Later_1 = "1 Week Later",
    Week_Later_2 = "2 Weeks Later",
    Week_Later_3 = "3 Weeks Later"
}

export enum MonthEnum {
    Month_Later_1 = "1 Month Later",
    Month_Later_2 = "2 Months Later",
    Month_Later_3 = "3 Months Later"
}

export enum AnnualyEnum {
    Year_Later_1 = "1 Year Later",
    Year_Later_2 = "2 Years Later",
    Year_Later_3 = "3 Years Later"
}

export class SourceRegionInputModel {
    TableType: string;
    CustomerId: number[] = [];
    CarrierId: number[] = [];
}

export enum FreightTableStatus {
    Draft = 1,
    Published = 2,
    Archived = 3
}

export enum ServiceOfferingType {
    FTL = 1,
    LTL,
    LTLWetHosing,
    DEF,
    DEFWetHosing
}

export enum WizardTabEnum {
    None = 0,
    ContactInfo,
    CompanyInfo,
    FleetInfo,
    ServiceOfferings
}

export enum WaitingAction {
    Nothing = 0,
    UpdatedPrice = 1,
    AvalaraTax = 2,
    CustomerApproval = 3,
    BolDetails = 4,
    ExceptionApproval = 5,
    Images = 6,
    PrePostDipData = 7
}

export enum LFVRecordStatus {
    None = 0,
    Clean, //Matched Records
    PendingMatch,
    NoMatch,
    IgnoreMatch,
    UnMatched,
    ActiveExceptions,
    ReprocessSubmitted,
    Duplicate,
    PartialMatch,
    ForcedIgnore
}

export enum OrderGroupType {
    MultiProducts = 1,
    Tier = 2,
    Blend = 3
}

export enum SourcingRequestStatus {
    Submitted = 1,
    Modified = 2,
    Accepted = 3,
    AcceptedAndModified = 4,
    OrderCreated = 5,
    Lost = 6
}

export enum TractorStatus {
    Active = 1,
    InActive = 2,
    UnderMaintenance = 3
}

export enum ApiResultType {
    Total = 3,
    Success = 0,
    Exception = 1,
}

export enum ReqResType {
    Response = 1,
    Request = 2,
}

export enum DefTrailerAssetType {
    "Box Truck" = 1,
    "Tank Wagon" = 2,
    "Stake Bed" = 3,
    "Transport" = 4,
}

export enum FuelTrailerAssetType {
    FTL = 1,
    LTL = 2
}

export enum Country {
    All = 0,
    USA = 1,
    CAN = 2,
    CAR = 4
}
export enum SourcingRequestDisplayStatus {
    All = 1,
    New = 2,
    WIP = 3,
    Sourced = 4,
    Lost = 5
}
export enum SalesTabViewType {
    PriorityTab = 1,
    TanksTab = 2,
}
