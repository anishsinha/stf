export class ForecastingLocationFilter {
    RegionId: string;
    CustomerIds: string;
    IsShowCarrierManaged: boolean;
    Carriers: string;
    InventoryCaptureType: string;
    IsRateOfConsumption: boolean;
}

export class LocationFilterModal {
    public SelectedRegionId: string;
    public SelectedStatusId: string;
    public SelectedCustomerId: string;
    public SelectedLocationId: string;
    public SelectedPrioritiesId: string;
    public selectedLocAttributeId: string;
    
}

export class SalesFilterModal {
    public SelectedRegionId: string;
    public SelectedStatusId: string;
    public SelectedCustomerId: string;
    public SelectedLocationId: string;
    public SelectedPrioritiesId: string;
    public selectedLocAttributeId: string;
    
}

export class SalesTankFilterModal {
    public SelectedRegionsData: any = [];
    public SelectedCustomerListData:any = [];
    public selectedLocAttributeData:any = [];
}
