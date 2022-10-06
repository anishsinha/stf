import { FreightTableStatus, TableType } from 'src/app/app.enum';
import { DropdownItem, DropdownItemExt } from 'src/app/statelist.service';
import { FeeModel } from '../../invoice/models/DropDetail';

export class ViewAccessorialFeeModel {
    AccessorialFeeId?: number;
    TableTypes: DropdownItem[] = [];
    Customers: DropdownItem[] = [];
    Carriers: DropdownItem[] = [];
    SourceRegions: DropdownItem[] = [];
    TerminalsAndBulkPlants: DropdownItemExt[] = [];
}

export class AccessorialFeeInputModel {
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

export class AccessorialFeeGridModel {
    public Id: number;
    public DateRange: string;
    public TableType: string;
    public TableName: number;
    public Customer: string;
    public Carrier: string;
    public SourceRegion: string;
    public Terminal: string;
    public BulkPlant: string;
    public StatusName: string;
    public IsArchived: boolean;
     // For UI Dropdown
    public IsShowMore : boolean;
}

export class CreateAccessorialFeeModel
{
    constructor() {
        this.CustomerIds = [];
        this.CarrierIds = [];
        this.SourceRegionIds = [];
        this.Fees = [];
    }
    public Id?: number;
    public Name: string;
    public Status: FreightTableStatus;
    public StartDate: Date;
    public EndDate: Date;
    public TableType :TableType
    public CustomerIds: number[] = [];
    public CarrierIds: number[] = [];
    public SourceRegionIds: number[] = [];
    public TerminalsAndBulkPlants: DropdownItemExt[] = [];
    public Fees: FeeModel[] = [];
    public CountryId: number;
}
