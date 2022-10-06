import { DropdownItem } from 'src/app/statelist.service';
import { FreightPricingMethod, FreightRateRuleType, TableType, WaitingAction } from 'src/app/app.enum';

export class InvoiceDetailModel {
    constructor() {
        this.FuelDropLocation = new AddressModel();
        this.AccessorialFeeDetails = [];
    }
    public PaymentTerm: PaymentTermModel;
    public Customer: CustomerModel;
    public BolDetails: BolDetail[] = [];
    public TicketDetails: LiftTicketDetail[] = [];
    public Drops: DropDetailModel[];
    public FuelDropLocation: AddressModel;
    public Carrier: string;
    public Driver: DropdownItem;
    public SupplierInvoiceNumber: string;
    public IsAssetTracked: boolean;
    public InvoiceTypeId: number;
    public IsVariousOrigin: boolean;
    public InvoiceNotes: string;
    public Fees: FeeModel[];
    public InvoiceImage: ImageModel;
    public InvoiceImages: ImageModel[] = [];
    public AdditionalImage: ImageModel;
    public AdditionalImages: ImageModel[] = [];
    public SignatureImage: ImageModel;
    public SignatureImages: ImageModel[] = [];
    public TaxAffidavitImage:ImageModel;
    public TaxAffidavitImages:ImageModel[] = [];
    public BDNImage : ImageModel;
    public BDNImages :ImageModel[] = [];
    public CoastGuardInspectionImage:ImageModel;
    public CoastGuardInspectionImages:ImageModel[] = [];
    public InspectionRequestVoucherImage:ImageModel;
    public InspectionRequestVoucherImages:ImageModel[] = [];
    public OriginalInvoiceHeaderId: number;
    public IsRebillInvoice: boolean;
    public WaitingFor: WaitingAction;
    public BrokerChainId: string;
    public PreferencesSettingId: number;
    public IsSupressOrderPricing: boolean;
    public IsBadgeMandatory: boolean;
    public ExistingHeaderId: number;
    public AccessorialFeeDetails: AccessorialFeeTableDetailViewModel[];
}
export class BlendedScheduleDetail {
    public BlendId: string;
    public Schedules: BlendedSchedules[] = [];
}
export class BlendedSchedules {
    public Id: number;
    public OrderId: number;
    public DeliveryLevelPO: string;
}

export class ImageModel {
    public Name: string;
    public IsPdf: boolean;
    public FilePath: string = "";
    public Data: string = "0x20"
    public ImageData: any
    public AzurePath: string;

}

export class DropDetailModel {
    public OrderId: number;
    public PoNumber: string;
    public TypeOfFuel: number;
    public FuelTypeId: number;
    public FuelTypeName: string;
    public ActualDropQuantity: number;
    public DropDate: string;
    public DisplayDropDate: string;
    public StartTime: string;
    public EndTime: string;
    public Allowance: number;
    public UserPriceToOverride: number;
    public MinDropDate: string;
    public DisplayMinDropDate: string;
    public MaxDropDate: string;
    public TrackableScheduleId: number;
    public TerminalId: number;
    public TerminalName: string;
    public IsAssetTracked: boolean;
    public Assets: AssetDropModel[];
    public PickupLocationType: number;
    public PickUpAddress: BulkPlantAddress;
    public Currency: string;
    public UoM: string;
    public Index: number;
    public OtherTaxDetails: OtherProductTaxModel[];
    public FuelSurchargeFreightFee: SurchargeModel;
    public IsSignatureRequired: boolean;
    public IsBOLImageRequired: boolean;
    public IsDropImageRequired: boolean;
    public IsBolDetailsRequired: boolean;
    public IsFTL: boolean;
    public QuantityIndicatorTypeId: number;
    public BrokerChainId: string;
    public IsDipDataRequired: boolean;
    public IsFreightOnlyOrder: boolean;
    public ExceptionId: number;
    public DropEndDate: string;
    public Gravity: number;
    public ConversionFactor: number;
    public ConvertedQuantity: number;
    public JobCountryId: number;
    public IsSupressOrderPricing: boolean;
    public BdrDetails: BDRModel;
    public IsMarineLocation: boolean;
    public BlendedScheduleId: string;
    public IsBdrDetailsAdded: boolean;// Used only for UI purpose
    public FreightPricingMethod: FreightPricingMethod;
    public DeliveryLevelPO: string;
    public InvoiceId: number;
}

export class FreightTableNameModel {
    public FreightRateRuleType: string;
    public TableType: string;
    public TerminalId: number;
    public OrderId: number;
    public bulkPlantId: number;
}

export class AccessorialFeeInvoiceInputViewModel {
    public OrderIds: number;
    public TerminalId: number;
    public BulkPlantId: number;
    public SupplierId: number;
    public CustomerId: number;
}

export class AssetDropModel {
    public Id: number;
    public OrderId: number;
    public InvoiceId: number;
    public AssetName: string;
    public JobXAssetId: number;
    public DropGallons: number;
    public StartTime: string;
    public EndTime: string;
    public IsNewAsset: boolean = false;
    public PreDip: number;
    public PostDip: number;
    public TankMakeModel: string;
    // public UoMType: number;
    public TankScaleMeasurement: number;
    public AssetType: number;

}

export class PaymentTermModel {
    public TermId: string;
    public NetDays: number;
}

export class CustomerModel {
    public CompanyId: number;
    public CompanyName: string;
    public Location: LocationModel;
    public ContactName: string;
    public ContactPhone: string;
    public ContactEmail: string;
}

export class LocationModel {
    public JobId: number;
    public SiteName: string;
    public Address: string;
    public City: string;
    public StateCode: string;
    public ZipCode: string;
    public CountryId: number;
    public IsMarineLocation: boolean = false;
}

export class BolDetail {
    public Id: number;
    public BolNumber: string;
    public LiftDate: string;
    public BadgeNumber: string;
    public DisplayLiftDate: string;
    public Products: BolProductModel[];
    public Images: ImageModel;
    public ImageList: ImageModel[] = [];
    public LiftStartTime: string;
    public LiftEndTime: string;
    public CommonTerminalId: number;
    public CommonTerminalName: string;
}

export class BolProductModel {
    public ProductId: number;
    public ProductName: string;
    public NetQuantity: number;
    public GrossQuantity: number;
    public Terminal: DropdownItem;
    public TerminalId: number;
    public TerminalName: string;
    public QuantityIndicatorTypeId: number;
    public DeliveredQuantity: number;

}

export class LiftTicketDetail {
    constructor() {
        this.Products = [];
    }
    public Id: number;
    public LiftTicketNumber: string;
    public LiftDate: string;
    public BadgeNumber: string;
    public DisplayLiftDate: string;
    public Products: LiftProductModel[];
    public Images: ImageModel;
    public ImageList: ImageModel[] = [];
    public LiftStartTime: string;
    public LiftEndTime: string;
    public CommonBulkPlantId: number;
    public CommonBulkPlantName: string;
    public CommonAddress: AddressModel;
}

export class LiftProductModel {
    public ProductId: number;
    public ProductName: string;
    public NetQuantity: number;
    public GrossQuantity: number;
    public BulkPlantId: number;
    public BulkPlantName: string;
    public Address: AddressModel;
    public QuantityIndicatorTypeId: number;
    public DeliveredQuantity: number;
}

export class FeeModel {
    public OrderId: number;
    public Currency: number;
    public TruckLoadType: number;
    public TruckLoadCategoryId: number;
    public CommonFee: boolean;
    public FeeConstraintTypeId: number;
    public SpecialDate: string;
    public FeeTypeId: string;
    public FeeSubTypeId: number;
    public OtherFeeDescription: string;
    public MinimumGallons: number;
    public DeliveryFeeByQuantity: ByQuantityModel[];
    public Fee: number;
    public IncludeInPPG: boolean;
}

export class ByQuantityModel {
    Currency: number;
    MinQuantity: number = 0;
    MaxQuantity: number;
    Fee: number;
}

export class AddressModel {
    constructor() {
        this.State = new DropdownItem();
        this.Country = new DropdownItem();
        this.CountryGroup = new DropdownItem();
    }
    public Address: string;
    public BulkPlantId: number;
    public Latitude: number;
    public Longitude: number;
    public CountyName: string;
    public State: DropdownItem;
    public City: string;
    public Country: DropdownItem;
    public CountryGroup: DropdownItem;
    public ZipCode: string;
}

export class BulkPlantAddress extends AddressModel {
    public SiteName: string;
    public SiteId: number;
}

export class OtherProductTaxModel {
    public OrderId: number;
    public TaxId: number;
    public TaxPricingTypeId: number;
    public TaxDescription: string;
    public TaxAmount: number;
}

export class SurchargeModel {
    public IsSurchargeApplicable: boolean;
    public IsFreightCostApplicable: boolean;
    public SurchargePricingType: number;
    public SurchargeProductType: number;
    public SurchargeFreightCost: number;
    public SurchargePercentage: number;
    public SurchargeEiaPrice: number;
    public SurchargeTableRangeStart: number;
    public SurchargeTableRangeEnd: number;
    public IsFeeByDistance: boolean = false;
    public Distance: number;
    public TotalFuelSurchargeFee: number;
    public DeliveryFeeByQuantity: ByQuantityModel[];
    public BuyerCompanyId: number;
    public FreightRateRuleType: FreightRateRuleType;
    public FreightRateTableType: TableType;
    public FreightRateRuleId: number;
    public FuelSurchargeTableType: TableType;
    public FuelSurchargeTableId: number;
    public FreightPricingMethod: FreightPricingMethod;
    public AutoSurchargeFreightCost: number;
}

export class FreightCostInputViewModel {
    public FreightRateRuleId: number;
    public OrderId: number;
    public TerminalId: number;
    public BulkPlantId: number;
    public SupplierId: number;       
    public DeliveredQuantity: number;
    public Distance: number;
}

export class AccessorialFeeTableDetailViewModel {
    public AccessorialFeeTableType: TableType;
    public AccessorialFeeId: number;
}

export class BDRModel {
    //public Id: number;
    public BDRNumber: string;
    public PumpingStartTime: string;
    public PumpingStopTime: string;
    public OpenMeterReading: string;
    public CloseMeterReading: string;
    public Viscosity: string;
    public SulphurContent: string;
    public FlashPoint: string;
    public DensityInVaccum: string;
    public ObservedTemperature: string;
    public MeasuredVolume: string;
    public StandardVolume: string;
    public MarpolSampleNumbers: string;
    public IsEngineerInvitedToWitnessSample: boolean = false;
    public IsNoticeToProtestIssued: boolean = false;
}

export function IsDuplicate(x: FeeModel, y: FeeModel) {
    return y.FeeTypeId == x.FeeTypeId
        && y.FeeSubTypeId == x.FeeSubTypeId
        && y.CommonFee == x.CommonFee
        && y.FeeConstraintTypeId == x.FeeConstraintTypeId;
};

export function DeDuplicateFees(existingFees: FeeModel[], newFees: FeeModel[]): FeeModel[] {
    if ((existingFees == undefined || existingFees == null)
        && (newFees == undefined || newFees == null)) {
        return [];
    }
    if (existingFees == undefined || existingFees == null) {
        return newFees;
    }
    if (newFees == undefined || newFees == null) {
        return existingFees;
    }
    var _combined: FeeModel[] = [];
    //1. Get higher from existing
    var f1 = getHigherFromFirst(existingFees, newFees);
    _combined = _combined.concat(f1);

    //2. Get unmatched from existing
    var f2 = getUnmatchedFromFirst(existingFees, newFees);
    _combined = _combined.concat(f2);

    //3. Take higher from new fees
    var f3 = getHigherFromFirst(newFees, _combined);
    _combined = _combined.concat(f3);

    //4. Take unmatched from new fees
    var f4 = getUnmatchedFromFirst(newFees, _combined);
    _combined = _combined.concat(f4);

    return _combined.sort(function (x, y) { return compare(x, y); });
}

export function IsMatched(x: FeeModel, y: FeeModel) {
    return y.FeeTypeId == x.FeeTypeId
        && y.FeeSubTypeId == x.FeeSubTypeId
        && y.CommonFee == x.CommonFee
        && y.FeeConstraintTypeId == x.FeeConstraintTypeId;
};

function getHigherFromFirst(array1: FeeModel[], array2: FeeModel[]): FeeModel[] {
    var _highers = [];
    array1.forEach(function (first) {
        var _higher = array2.find(function (second) {
            //1. Flat Fee: Remove if duplicate or take higher fee
            //2. Per Hour: Remove if duplicate or take higher fee
            //3. Percent: Always take higher fee
            //4. Per Asset: Don't allow duplicate, take higher fee
            //5. Per Gallon: Apply product-wise : No need to check for duplicates
            return IsMatched(first, second) && (
                (isFlatFee(first, second) && first.Fee > second.Fee) ||
                (isPerHourFee(first, second) /*&& first.Fee > second.Fee*/) ||
                (isPercentFee(first, second) && first.Fee > second.Fee) ||
                (isPerAssetFee(first, second) /*&& first.Fee > second.Fee*/) ||
                isPerGallonFee(first, second));
        });
        if (_higher != undefined && _higher != null) {
            _highers.push(first);
        }
    });
    return _highers;
}

function getUnmatchedFromFirst(array1: FeeModel[], array2: FeeModel[]): FeeModel[] {
    var _unmatched = [];
    array1.forEach(function (first) {
        var _unmatch = array2.find(function (second) {
            return IsMatched(first, second);
        });
        if (_unmatch == undefined && _unmatch == null) {
            _unmatched.push(first);
        }
    });
    return _unmatched;
}

function isFlatFee(x: FeeModel, y: FeeModel): boolean {
    return x.FeeSubTypeId == 2 && y.FeeSubTypeId == 2;
}

function isPerAssetFee(x: FeeModel, y: FeeModel): boolean {
    return x.FeeSubTypeId == 4 && y.FeeSubTypeId == 4;
}

function isPerHourFee(x: FeeModel, y: FeeModel): boolean {
    return x.FeeSubTypeId == 5 && y.FeeSubTypeId == 5;
}

function isPerGallonFee(x: FeeModel, y: FeeModel): boolean {
    return x.FeeSubTypeId == 17 && y.FeeSubTypeId == 17;
}

function isPercentFee(x: FeeModel, y: FeeModel): boolean {
    return x.FeeSubTypeId == 18 && y.FeeSubTypeId == 18;
}

function compare(a: FeeModel, b: FeeModel) {
    if (a.FeeTypeId > b.FeeTypeId) return 1;
    if (b.FeeTypeId > a.FeeTypeId) return -1;
    return 0;
}

export class QuantityInfo {
    ProductId: number;
    TotalDroppedQuantity: number;
}

export class DropQuantityByPrePostDipRequestModel
{
    public TankId: number;
    public ScaleMeasurement: number;
    public PreDipValue: number;
    public PostDipValue: number;
    public JobId: number;
    public JobxAssetId: number;
}
export class  DropQuantityByPrePostDipResponseModel
{       
        public DropQuantity: number;
    public JobxAssetId: number;
    public StatusCode: number;
    public StatusMessage: string;
}

export class MFNConversionResponseViewModel {
    public IsValidGravity: boolean;
    public ConvertedQty: number;
    public UoM: any;
    }

export class MFNConversionRequestViewModel {
    public DroppedGallons: number;
    public UoM: any;
    //Could be gravity or hardcoded 42 for barrel conversion
    public ConversionFactor: number;
    public JobCountryId: number;
}
//[Validators.pattern(/^(\0*[1-9]*[1-9][0-9]*(\.[0-9]+)?|[0]*\.[0-9]*[1-9][0-9]*)$/)]),
//ActualDropQuantity: this.fb.control(quantity, [Validators.required, Validators.min(0.00000001), Validators.pattern(/^[0-9]\d*(\.\d+)?$/)]),