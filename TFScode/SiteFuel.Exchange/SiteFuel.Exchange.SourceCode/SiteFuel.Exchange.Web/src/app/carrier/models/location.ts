import { DropdownItem, LocationDropdownItem } from 'src/app/statelist.service';
import { DropDownItem } from 'src/app/buyer-wally-board/Models/BuyerWallyBoard';

export class LocationDetailsModel {
    public Id: number;
    public ControlNumber: string;
    public TerminalId: number;
    public TerminalName: string;
    public AssignedTerminalId: string;
    public CreatedByCompanyId: number;
    public IsBulkPlant: boolean;
    public TerminalSupplierId: number;
    public TerminalSupplierName: string;
}

export enum Country {
    "USA" = 1,
    "CAN" = 2,
    "CAR" = 4
}




export class TerminalMappingModel {
    constructor() {
        this.StateList = [];
        this.CityList = [];
        this.TerminalList = [];
    }

    public Id: number;
    public CompanyId: number;
    public CountryId: number;
    public MyProductId: string;
    public BackOfficeProductId: string;
    public DriverProductId: string;
    public TerminalItemCode: string;
    public StateList: DropdownItem[];
    public CityList: DropdownItem[];
    public TerminalList: DropdownItem[];
    public FuelType: DropdownItem;
    public StateIds: string;
    public CityIds: string;
    public TerminalIds: string;
    public FuelTypeIds: string;
    public IsActive: boolean;
}

export class TerminalMappingGridModel {
    constructor() {

    }

    public Id: number;
    public CompanyId: number;
    public TerminalItemCode: string;

    public StateId: number;
    public StateCode: string;
    public CityId: number;
    public City: string;
    public FuelTypeId: number;
    public FuelType: string;
    public TerminalId: number;
    public TerminalName: string;
    public TerminalAddress: string;
    public CountryCode: string;
}

export class RouteInformationModel {
    Id: string;
    Name: string;
    TfxJobs: RouteTfxJobsList[];
    RegionId: string;
    TfxCompanyId: number;
    CreatedBy: number;
    ShiftInfoDetails: string;
}
export class RouteInfoDetails {
    Id: string;
    Name: string;
    LocationSeqNo: number;
}
export class ShiftInfoViewModel {
    Id: string;
    TripId: string;
    DriverRowIndex: number;
    DriverColIndex: number;
    ShiftIndex: number;
}
export class RouteTfxJobsList {
    Id: number;
    Code: string;
    Name: string;
    SequenceNo: number;
}
export class JobWithSequence {
    JobDetails: DropDownItem;
    SequenceNo: number;
}
export class JobRegionModel extends LocationDropdownItem{
    RegionId: string;
    LocationManagedType: number;
}