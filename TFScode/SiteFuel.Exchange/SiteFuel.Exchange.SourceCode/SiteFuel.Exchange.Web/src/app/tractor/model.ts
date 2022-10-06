
import { DropdownItem } from 'src/app/statelist.service';
import { DriverModel } from 'src/app/carrier-companies/service/assigncarrier.service';
import { TractorStatus, TrailerType, TruckStatus } from 'src/app/app.enum';

export class Compartment {
    public CompartmentId: string;
    public Capacity: number;
}

export class TruckDetailsModel {
    public Id: string;
    public Name: string;
    public TruckId: string;
    public FuelCapacity: number;
    public ContractNumber: string;
    public Compartments: Compartment[];
    public CreatedBy: number;
    public CompanyId: number;
    public CreatedDate: Date;
    public Status: TruckStatus;
    public TrailerType: TrailerType;
    public IsDeleted: boolean;
    public Drivers: DropdownItem[];
    public TrailerFuelRetains: TrailerFuelRetainModel[];
}
export class TractorDetailsModel {
    public Id: string;
    public TruckId: string;
    public VIN: string;
    public Plate: string;
    public TrailerType: [];
    public Owner: string;
    public CompanyId: number;
    public CreatedDate: Date;
    public Status: TractorStatus;
    public IsDeleted: boolean;
    public Drivers: DriverModel[];
    public OptimizedCapacity: number;
}



export class TrailerFuelRetainModel {
    public Id: string;
    public TrailerId: string;
    public CompartmentId: string;
    public Quantity: number;
    public ProductType: string;
    public UOM: number;
    public DeliveryRequestId: string;
}