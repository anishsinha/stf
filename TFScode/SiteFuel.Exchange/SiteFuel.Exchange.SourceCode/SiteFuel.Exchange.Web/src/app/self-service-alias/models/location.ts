import { DropdownItem } from 'src/app/statelist.service';

export class LocationDetailsModel {
    public Id: number;
    public ControlNumber: string;
    public TerminalId: number;
    public TerminalName: string;
    public AssignedTerminalId: string;
    public CreatedByCompanyId: number;
}

export enum Country {
    "USA" = 1,
    "CAN" = 2
}




export class TerminalMappingModel {
    constructor() {
        this.StateList = [];
        this.CityList = [];
        this.TerminalList = [];
        this.FuelType = new DropdownItem();
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
    public MyProductId: string;
    public BackOfficeProductId: string;
    public DriverProductId: string;
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
