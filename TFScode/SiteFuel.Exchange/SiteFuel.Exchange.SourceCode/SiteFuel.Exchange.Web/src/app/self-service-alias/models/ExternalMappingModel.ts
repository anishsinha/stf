
export class ExternalCustomerMappingViewModel {
    public Id: number;
    public CustomerId: number;
    public CustomerName: string;
    public TargetCustomerValue: string;
    public ThirdPartyId: number;
}

export class ExternalCustomerLocationMappingViewModel {
    public Id: number;
    public CustomerLocationId: number;
    public CustomerLocationName: string;
    public TargetCustomerLocationValue: string;
    public ThirdPartyId: number;
    public CompanyName: string;
    public TargetCustomerValue: string;
}
export class ExternalProductMappingViewModel {
    public Id: number;
    public ProductId?: number;
    public ProductName: string;
    public TargetProductValue: string;
    public ThirdPartyId: number;
    public OtherProductId?: number;
}
export class ExternalTerminalMappingViewModel {
    public Id: number;
    public TerminalId: number;
    public TerminalName: string;
    public ControlNumber: string;
    public TargetTerminalValue: string;
    public ThirdPartyId: number;
}
export class ExternalSupplierMappingViewModel {
    public Id: number;
    public SupplierId: number;
    public SupplierName: string;
    public TargetSupplierValue: string;
    public ThirdPartyId: number;
}
export class ExternalBulkPlantMappingViewModel {
    public Id: number;
    public BulkPlantId: number;
    public BulkPlantName: string;
    public TargetBulkPlantValue: string;
    public ThirdPartyId: number;
}
export class ExternalCarrierMappingViewModel {
    public Id: number;
    public CarrierId: number;
    public CarrierName: string;
    public TargetCarrierValue: string;
    public ThirdPartyId: number;
}
export class ExternalDriverMappingViewModel {
    public Id: number;
    public DriverId: number;   
    public FirstName: string;
    public LastName: string;
    public Email: string;
    public TargetDriverValue: string;
    public ThirdPartyId: number;
}
export class ExternalVehicleMappingViewModel {
    public Id: string;
    public TruckId: number;
    public TruckName: string;
    public TargetVehicleValue: string;
    public ThirdPartyId: number;
}