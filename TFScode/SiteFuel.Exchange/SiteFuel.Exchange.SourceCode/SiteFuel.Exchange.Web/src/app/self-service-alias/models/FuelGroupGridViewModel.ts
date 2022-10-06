
import { FreightTableStatus, TableType } from "src/app/app.enum";
import { DropdownItem } from "../../statelist.service";

export class FuelGroupGridViewModel {
    Id: number;
    GroupName: string;
    TableType: string;
    FuelGroupType: string;    
    Customer: string;
    Carrier: string;
    ProductType: string;
    StatusName: string;
}

export class FuelGroupViewModel {
    Id?: number;
    GroupName: string;
    FuelGroupType: FuelGroupType;//GroupTypeStandard,GroupTypeCustom
    TableType: TableType; // customer or carrier single selction
    AssignedCompanyId?: number;
    ProductTypes: DropdownItem[];
    TableTypes: DropdownItem[];
    Customers?: DropdownItem[];
    Carriers?: DropdownItem[];
    ProductTypeIds: number[];
    ProductTypesToString: string;
    FuelTypes: DropdownItem[];
    FuelTypeIds: number[];
    FuelTypesToString: string;
    FreightTableStatus: FreightTableStatus; // publish or arcived
    IsActive?: boolean;
    AddedBy?: number;
    AddedDate?: Date | string;
    UpdatedBy?: number;
    UpdatedDate?: Date | string;
}


export enum FuelGroupType {
        Standard = 1,
        Custom = 2
}

