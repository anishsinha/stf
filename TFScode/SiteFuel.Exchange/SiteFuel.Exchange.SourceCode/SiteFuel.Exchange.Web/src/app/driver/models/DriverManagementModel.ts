import { DropdownItem } from 'src/app/statelist.service';

export class DriverManagementModel {
    constructor() {
        this.Drivers = [];
        this.LicenseTypes = [];
        this.TrailerTypes = [];
    }

    public Drivers: DriverViewModel[];
    public LicenseTypes: DropdownItem[];
    public TrailerTypes: DropdownItem[];
}

export class DriverViewModel {
    constructor() {
        this.LicenseType = new DropdownItem();
        this.TrailerType = {};
        this.Status = new DropdownItem();
        this.ShiftId = [];
    }

    public DriverId: string;
    public FirstName: string;
    public LastName: string;
    public CompanyName: string;
    public Email: string;
    public LicenseNumber: string;
    public ExpiryDate: string;
    public ContactNumber: string;
    public DisplayTrailerTypes: string;
    public DisplayLicenseType: string;
    public LicenseTypeId: string;
    public LicenseType: DropdownItem;
    public TrailerType: any;
    public TrailerTypeId: number[] = []; 
    public ShiftId: string[];
    public Regions: string[];
    public InvitedBy: string;
    public UserId: number;
    public Status: DropdownItem;
    public IsActive: boolean;
    public IsScheduleExists: boolean;
    public ScheduleBuilderIdInfo : string; 
}

export class DriverShiftModel {
    constructor() {
        this.Shifts = [];
    }
    public DriverName: string;
    public Shifts: ShiftDetailModel[];
}

export class ShiftDetailModel {
    public FromDate: string;
    public ToDate: string;
    public ShiftFrom: any;
    public ShiftTo: any;
}
