import { DropdownItem } from 'src/app/statelist.service';
import { Shift } from './shift';

export class RegionModel {
    public UserId: number;
    public CompanyId: number;
    public CountryId: number;
    public DefaultSlotPeriod: number;
    public Regions: Region[];
}

export class Region {
    public Id: string;
    public Name: string;
    public SlotPeriod: number;
    public Description: string;
    public States: DropdownItem[];    
    public Jobs: DropdownItem[];
    public Drivers: DropdownItem[];
    public Dispatchers: DropdownItem[];
    public Trailers: DropdownItem[];
    public Carriers: TfxCarrierDropdownDisplayItem[];
    public Shifts: Shift[];
    public ProductTypeIds: number[];
    public FuelTypeIds: DropdownItem[];
    public FavProductTypeId: number = FavProductType.ProductType;
}

export class TfxCarrierDropdownDisplayItem {
    Id: number;
    Code: string;
    Name: string;
    SequenceNo: number;
    RegionId: string;
}
export class CarrierRegionModel {
    Id: number;
    Name: string;
    Regions: TfxCarrierRegionDetailsModel[];
}
export class TfxCarrierRegionDetailsModel {
    Id: string;
    SequenceNumber: number;
    Name: string;
    Code: string;
}

export class SourceRegionModel {
    public UserId: number;
    public CompanyId: number;
    public CountryId: number;
    public Regions: SourceRegion[];
}

export class SourceRegion {
    public Id: string;
    public Name: string;
    public Description: string;
    public States: DropdownItem[];
    public Carriers: DropdownItem[];
    public Cities: DropdownItem[];
    public Terminals: DropdownItem[];
    public BulkPlants: DropdownItem[];
    public StateIds: string;
    public CityIds: string;
    public TerminalIds: string;
    public BulkPlantIds: string;
    public CompanyId: number;
}
export enum FavProductType {
    None,
    ProductType,
    FuelType,
}
