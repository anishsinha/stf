import { PickupLocationType } from 'src/app/app.enum';

export class LFRecordGridModel {
    public LiftFileRecordId: number;//refers Id of LiftFileValidationRecords table
    public bol: string;
    public TerminalName: string; // Represents terminal code as received in Parkland API Json
    public Terminals: string;
    public correctedQuantity: number;
    public RecordDate: string;
    public statusChangeDate: string = "";
    public Status: string;
    public TerminalItemCode: string;
    public ProductType: string;
    public LoadDate: string;
    public InvFtlDetailId: number;
    public InvId: number;
    public LiftTicketNumber: string;
    public Reason: string;
    public CarrierID: string;
    public FileName: string;
    public CallId: string;
    public Terminal: string //== mapped terminal/Bulk plant as per terminal mapping.
    public CarrierName: string;
    public recordStatus: string; // string denoting current status of records like "No Match" ,"Active Exception""
    public IsFromScratchReport: boolean;
    public ProductTypeId: number;
    public CIN: string;
    public GrossQuantity: number;
    public ModifiedDate: string;
    public Username: string;
    public ReasonCode: string;
    public ReasonCategory: string;
    public TimeToBol: string;
}

export class LFBolEditModel {
    constructor() {
        this.LiftRecord = new LFRecordGridModel();
    }
    public LiftRecord: LFRecordGridModel;
    public LiftDate: string;
    public GrossQuantity: number;
    public NetQuantity: number;
    public BadgeNumber: string;
    public FuelTypeId: number;
    public TerminalId: number;
    public Notes: string;
    public BolNumber: string;
    public InvoiceFtlDetailId: number;
    //public int InvoiceFtlDetailIdFromList { get; set; }
    public InvoiceFtlDetailsList: DropDownItem[];
    public OrderId: number;
    public DisplayTerminalName: string;
    public PricingSourceId: number;
    public PickupLocationType: PickupLocationType;
    public IsBulkPlantLift: boolean;
    public DisplayLiftDate: string;
    public SelectedTerminal: DropDownItem;
    public TerminalList: DropDownItem[];
    public SelectedFuelType: DropDownItem;
    public FuelTypeList: DropDownItem[];
    public SelectedInvoiceFtlDetailId: DropDownItem;
}

export class DropDownItem {
    Id: number;
    Name: string;
    Code:string;
    DisplayName:string;
}

export class LFValidationGridViewModel {
    CallId?: number; // Refers LF DETAILS table Id
    TotalRecordCount?: number;
    MatchedRecordCount?: number;
    ActiveExceptionRecordCount: number;
    NoMatchRecordCount?: number;
    RecordDate?: string; //REFERES AddedDate column in LF DETAILS TABLE
    IgnoredMatchRecordCount?: number;
    UnmatchedRecordCount?: number;
    UpdatedOn?: string;
    PendingMatchCount?: number;
    DuplicateRecordCount?: number;
    PartialMatchRecordCount?: number;
    CarrierName?: string;
    CarrierID?: string;
    ForcedIgnoredMatchRecordCount?: number;
}

export interface LFRecordsGridViewModel {
    LiftFileRecordId: number; //refers Id of LiftFileValidationRecords column
    bol: string;
    TerminalName: string; // Represents terminal code as received in Parkland API Json
    correctedQuantity: number;
    // validatedQuantity: number;
    RecordDate: string;
    statusChangeDate: string;
    Status: number;
    TerminalItemCode: string;
    ProductType: string;
    LoadDate: string;
    InvFtlDetailId: number | null;
    // DisplayTerminalName: string;
    InvId: number | null;
    LiftTicketNumber: string;
    Reason: string;
    CarrierID: string;
    FileName: string;
    CallId: number;
    recordStatus: string; // used in searchby bol filename grid
    Terminal: string; //== mapped terminal/Bulk plant as per terminal mapping.
    CarrierName: string;
    IsInvoiceFtlDetailListRequired: boolean;
    IsRecordPushedToExternalApi: boolean;
    NetQuantity: number;
    GrossQuantity: number;
    BadgeNumber: string;
    IsFromScratchReport: boolean;
    LfvValidationParameters: LFVValidationParameters;
    IsAdminUser: boolean;
    CIN: string;
    Username: string;
    ModifiedDate: string;
    ReasonCode: string;
    ReasonCategory: string;
    TimeToBol: string;
}

//LiftFileRecordsWithMissingTFXDeliveryDetails
export class SupplierBOLReport {
    public CallId: number;
    public BOL: string;
    public TerminalCode: string; //Represents terminal code as received in Parkland API Json
    public CorrectedQuanity: number;
    public TerminalItemCode: string;
    public LoadDate: string;
    public RecordStatus: string;
    public Reason: string;
    public CarrierID: string;
    public RecordDate: string;
    public CarrierName: string;
    public Terminal: string;
    public FileName: string;
    public Status: number;
    public ProductType: string;
    public ReasonCode: string;
    public ReasonCategory: string;
}

//TFXDeliveryDetailsWithMissingLiftFileRecords
export class CarrierBOLReport {
    public BOL: string;
    public TerminalName: string;
    public LoadDate: string;
    public NetQuantity: number;
    public GrossQuantity: number;
    public BadgeNumber: string;
    public CarrierName: string;
    public CarrierID: string;
    public FuelTypeName: string;
}

export class LFRecordsGridExport {  
    bol: string;
    TerminalName: string;
    Terminals: string;
    correctedQuantity: number;
     RecordDate: string;
    TerminalItemCode: string;
    ProductType: string;
    LoadDate: string;
    Reason: string;
    CarrierID: string;
    CarrierName: string;
    Username: string;
    ModifiedDate: string;
    ReasonCode: string;
    ReasonCategory: string;
    LFVResolutionTime: string;
    TimeToBol: string;
}

export class LFVValidationParameters {
    public IsTerminalCodeReq: boolean = true;
    public IsBolReq: boolean = true;
    public IsCINReq: boolean;
    public IsCarrierNameReq: boolean;
    public IsLoadDateReq: boolean;
    public IsTermItemCodeReq: boolean;
    public IsCorrectedQtyRes: boolean;
    public IsGrossReq: boolean;
    public IsCorrectedQtyOrGrossReq: boolean;
}