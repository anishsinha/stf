export interface TerminalItemCodeMappingModel 
    {
        Id?: number;

        TerminalSupplierId?: number;

        TerminalSupplier?: string;

        ItemDescriptionId?: number;

        ItemDescription?: string;

        ProductType?: string;

        ItemCode?: string;

        EffectiveDate?: Date | string;

        ExpiryDate?: Date | string | null;

        CompanyId?: number;

        IsActive?: boolean;

        AddedBy?: number;

        AddedDate?: Date | string;

        UpdatedBy?: number;

        UpdatedDate?: Date | string;
    }