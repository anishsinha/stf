export class JobBuyerDashboardViewModel {
    CustomerName?: string;
    CustomerId?: number;
    CountryId?: number;
    CountryCode?: string;
    JobID?: number;
    JobName?: string;
    Latitude?: string;
    Longitude?: string;
    Address?: string;
    City?: string;
    CityId?: number;
    State?: string;
    StateID?: number;
    ZipCode?: string;
    ContactPersonName?: string;
    ContactPhoneNumber?: string;
    jobDeliveryRequests?: JobDRDetailsModel[];
}

export class JobDRDetailsModel {
    JobId?: number;
    Priority?: number;
}

export class BuyerLoadsForDashboardInputModel {
    CountryId?: number;
    FromDate?: Date;
    ToDate?: Date;
}
export class InventoryForDashboardInputModel {
    CountryId?: number;
    FromDate?: Date;
    ToDate?: Date;
}

export class BuyerLoadsForDashboardViewModel {
    PoNumber?: string; // PoNumber
    Location?: string; // Drop location
    Product?: string; // Product name
    Quantity?: string; // Drop quantity
    Dispatcher?: string; // Dispatcher name
    Status?: string; // Delivery current status
    Priority?: number; // Load priority
}

export interface InventoryViewModel {
    SiteId?: string;
    Location?: string;
    TankName?: string;
    AvgSale?: string;
    Inventory?: string;
    DaysRemaining?: string;
    Priority?: number;
}

export class InvoiceGridBuyerDashboardInputModel {
    UserId?: number;
    CompanyId?: number;
    IsBuyerAdmin?: boolean;
    CountryId?: number;
    CurrencyTypeId?: number;
    GroupIds?: string;
    InvoiceTypeId?: number;
    BrandedCompanyId?: number;

}


export class InvoiceGridBuyerDashboardModel {
    Id?: number;
    InvoiceNumber?: string;
    Supplier?: string;
    PoNumber?: string;
    DropDate?: string;
    DropTime?: string;
    Status?: string;
    CreatedDate?: Date | string;
    IsSupressOrderPricing?: boolean;
}

export class DashboardTileViewModel {
        TileName?: string;

        RowIdx?: number;

        ColIdx?: number;

        IsCollapsed?: boolean;

        IsClosed?: boolean;

        TileDisplayName?: string;
    }