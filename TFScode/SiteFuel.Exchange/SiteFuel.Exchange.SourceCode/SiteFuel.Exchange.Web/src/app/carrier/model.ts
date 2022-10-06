import { LicenceRequirement, TrailerType, TruckStatus } from 'src/app/app.enum';
import { DropdownItem } from '../carrier-companies/service/assigncarrier.service';

export class Compartment {
    public CompartmentId: string;
    public FuelType: number;
    public Capacity: number;
    public PumpId: string;
}

export class TruckDetailsModel {
    public Id: string;
    public Name: string;
    public TruckId: string;
    public Owner: string;
    public FuelCapacity: number;
    public ContractNumber: string;
    public Compartments: Compartment[];
    public CreatedBy: number;
    public CompanyId: number;
    public CreatedDate: Date;
    public Status: TruckStatus;
    public TrailerType: TrailerType;
    public LicenceRequirement: LicenceRequirement;
    public LicencePlate: string;
    public ExpirationDate: string;
    public IsDeleted: boolean;
    public Drivers: DropdownItem[];
    public IsPump: string;
    public IsFilldCompatible: boolean;
}

