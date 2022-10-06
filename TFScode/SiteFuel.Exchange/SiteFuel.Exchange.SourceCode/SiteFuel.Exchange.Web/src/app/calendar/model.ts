export class CalendarFilterModel {
    constructor() {
        this.Customers = [];
        this.Locations = [];
        this.Vessels = [];
        this.Priorities = [];
        this.FromDate = new Date();
        this.ToDate = new Date();
    }
    public Customers: any[];
    public Locations: any[];
    public Vessels: any[];
    public LocationType: boolean;
    public Priorities: any[];
    public FromDate: Date;
    public ToDate: Date;
}

export class ShiftModel {
    public Id: string;
    public Name: string;
    public Indexes: IndexModel[];
}

export class IndexModel {
    public LoadIndex: number;
    public ColumnIndex: number;
    public LoadTime: string;
    public Driver: string;
}