import { DropdownItem, DropdownItemExt} from 'src/app/statelist.service';

export class FuelSurchargeTableModel {    
    public StartDate: Date;
    public EndDate: Date;

    public PriceRangeStartValue: number;
    public PriceRangeEndValue: number;
    public PriceRangeInterval: number;

    public FuelSurchargeStartPercentage: number;     
    public SurchargeInterval: number;

    public Id: string;
    public SupplierId?: number;

    constructor(values: Object = {}) {
        Object.assign(this, values);
    }
}
export class FuelSurchargeIndexViewModel {    
    FuelSurchargeIndexId?: number;
    TableTypes: DropdownItem[] = [];
    Customers: DropdownItem[] = [];    
    Carriers: DropdownItem[] = [];
    SourceRegions: DropdownItem[] = [];
    TerminalsAndBulkPlants: DropdownItemExt[] = [];
    FuelSurchargeProducts: DropdownItemExt[] = [];
    FuelSurchargePeriods: DropdownItemExt[] = [];
    FuelSurchargeAreas: DropdownItemExt[] = [];

    ProductId?: number;
    PeriodId?: number;
    TableTypeId: number;
    AreaId?: number;
    WeekDay: string="Mon";
    Weeks: DropdownItemExt[] = [];
    Months: DropdownItemExt[] = [];
    SourceMonths: DropdownItemExt[] = [];
    Annualy: DropdownItemExt[] = [];
    SourceAnnualy: DropdownItemExt[] = [];
    
    APILatestIndexPrice: number;
    ApiAdjustIndexPriceDate: Date;
    ApiEffectiveDate: any;
    ManualLatestIndexPrice: number;
    ManualEffectiveDate: Date;
    IsManualUpdate: boolean=false;
    FuelSurchargeTable: FuelSurchargeTableModel = new FuelSurchargeTableModel(); //input model
    TableName: string;
    Notes: string;
    IndexPriceDate: Date;
    StatusId: number=2;
    GeneratedSurchargeTable: FuelSurchargeTableModel[]; // FS table genearted
}

export class FuelSurchargeInputModel {
    public TableTypeIds: string;
    public CustomerIds: string;
    public CarrierIds: string;
    public SourceRegionIds: string;
    public TerminalIds: string;
    public BulkPlantIds: string;
    public StartDate: string;
    public EndDate: string;
    public IsArchived: boolean;
}

export class FuelSurchargeGridModel {
    public Id: number;
    public DateRange: string;
    public TableTypeNew: string;
    public TableName: number;
    public Customer: string;
    public Carrier: string;
    public SourceRegion: string;
    public Terminal: string;
    public BulkPlant: string;
    public IndexProduct: number;
    public IndexArea: string;
    public IndexPeriod: string;
    public IndexType: string;
    public StatusName: string;
    public IsArchived: boolean;
    // For UI Dropdown
    public IsShowMore : boolean;
}

export class FuelSurchargePricingModel {
    public PriceRangeStartValue: number;
    public PriceRangeEndValue: number;
    public FuelSurchargeStartPercentage: string;
}

export class HistoricalPriceModel {
    public IndexPeriod: string;
    public IndexProduct: string;
    public IndexArea: string;
    public IndexTypeName: string;
    public IndexType: number;
    public ManualIndexPrice: string;
    public ManualIndexPriceDate: string;
    public PeriodName: string;
    HistoricalPriceDetails: HistoricalPriceDetailsModel[]; 
}

export class HistoricalPriceDetailsModel {
    public PublishDate: string;
    public Price: string;
}


