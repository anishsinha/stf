import { Country } from "src/app/app.enum";

export interface TerminalSupplierModel  
{
    Id?: number;

    Code?: string;

    Name?: string;

    ProductTypeId?: number;

    Country?: Country;

    IsActive?: boolean;

    AddedBy?: number;

    AddedDate?: Date | string;

    UpdatedBy?: number;

    UpdatedDate?: Date | string;
    ProductTypeName?:string;
}
