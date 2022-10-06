import { DropdownItem, DropdownItemExt } from 'src/app/statelist.service';
import * as moment from 'moment';

export class FreightRateModel {
    FreightRateId?: number;
    FreightRateRuleTypes: DropdownItem[] = [];
    TableTypes: DropdownItem[] = [];
    Customers: DropdownItem[] = [];
    Carriers: DropdownItem[] = [];
    SourceRegions: DropdownItem[] = [];
    TerminalsAndBulkPlants: DropdownItemExt[] = [];
}

export class FreightRateInputModel {
    public FreightRateRuleTypeIds: string;
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

export class FreightRateGridModel {
    public Id: number;
    public DateRange: string;
    public FreightRateRuleType: string;
    public TableType: string;
    public TableName: number;
    public Customer: string;
    public Carrier: string;
    public SourceRegion: string;
    public Terminal: string;
    public BulkPlant: string;
    public FuelGroup: string;
    public StatusName: string;
    public IsArchived: boolean;
    public FreightRateRuleTypeValue: number;
    // For UI Dropdown
    public IsShowMore : boolean;

}