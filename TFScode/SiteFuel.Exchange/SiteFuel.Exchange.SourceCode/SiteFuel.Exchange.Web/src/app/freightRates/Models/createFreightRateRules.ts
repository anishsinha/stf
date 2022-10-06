import { DropdownItem, DropdownItemExt } from 'src/app/statelist.service';
import { FreightRateCalcPreferenceType, FreightRateRuleType, FreightTableStatus, FuelGroupType, TableType } from 'src/app/app.enum';

export class FreightRateFuelGroupViewModel {
    MinQuantity: number;
    FuelGroupId: number;
}

export class FreightRateRouteTableViewModel {
    StartQuantity: number;
    EndQuantity?: number;
    RateValue: number;
    FuelGroupId: number;
}
export class FreightRatePtoPTable {
    TerminalAndBulkPlantName : string;
    LocationName : string;
    LocationAddress  : string;
    LaneID: string;
    AssumedMiles: number;
    IsLaneRequired: boolean;
    RateValue: number;
    FuelGroupName: string;
    FuelGroupId : number;
    JobId: number;
    TerminalId?: number;
    BulkPlantId?: number;

}
export class FreightRateRangeTableViewModel {
    UptoQuantity: number;
    RateValue: number;
    FuelGroupId: number;
}





export class FreightRateViewModel {

    Id?: number;
    Name: string;
    Status: FreightTableStatus;
    FreightRateRuleType: FreightRateRuleType;
    FuelGroupType: FuelGroupType;
    MixLoadMinValue: number;
    FreightRateCalcPreferenceType: FreightRateCalcPreferenceType;
    FreightRateCalcPrefFuelGroupId?: number;
    StartDate: Date;
    EndDate: Date;
    TableType: TableType;
    CustomerIds: number[] = [];
    CarrierIds: number[] = [];
    JobIds: number[] = [];
    FuelGroupIds: number[] = [];
    SourceRegionIds: number[] = [];
    TerminalsAndBulkPlants: DropdownItemExt[] = [];
    FreightRateFuelGroups: FreightRateFuelGroupViewModel[] = [];
    FreightRateRouteTables: FreightRateRouteTableViewModel[] = [];
    FreightRateRangeTables: FreightRateRangeTableViewModel[] = [];
    FreightRatePtoPTables: FreightRatePtoPTable[] = [];
}


export class FreightPricingRulesViewModel {
    
    TableTypes: DropdownItem[] = [];
    Customers: DropdownItem[] = [];
    Locations: DropdownItem[] = [];
    Carriers: DropdownItem[] = [];
    FuelGroups: DropdownItem[] = [];
    SourceRegions: DropdownItem[] = [];
    TerminalsAndBulkPlants: DropdownItemExt[] = [];

    TableTypeId: number;
    FuelGroupType: FuelGroupType = FuelGroupType.Standard;
    public StartDate: Date;
    public EndDate: Date;
    TableName: string;
    StatusId: number = 2;
}



