import { ServiceOfferingType } from "src/app/app.enum";

export class UserInfo {
    public Id: number;
    public Title: number;
    public FirstName: string;
    public LastName: string;
    public Email: string;
    public IsNewCompany: boolean;
    public CompanyName: string;
    public CompanyType: string;
    public CompanyAddress: string;
    public CountryId: number;
    public StateId: number;
    public City: string;
    public Zip: string;
    public PhoneNumber: number;
    public PhoneType: string;
}
export class FleetTrailers {
    public TrailerServiceType: string;
    public Capacity: number;
    public TrailerHasPump: boolean;
    public IsTrailerMetered: boolean;
    public Count: number;
    public PackagedGoods: boolean;

}
export class FleetInfo {
    public FuelAssets: FleetTrailers[];
    public DefAssets: FleetTrailers[];
}
export class ServiceOffering {
    public IsDeliver: boolean;
    public ServiceDeliveryType: ServiceOfferingType;
    public ServiceAreas: ServiceArea[];
}

export class ServiceArea {
    StateId: number;
    CityId: number | null;
    CityName: string;
    ZipCode: string;
}

export class StateListViewModel {
    Id: number;
    Code: string;
    CountryId: number;
    Name: string;
}

export class CarrierOnboardingBrandingModel {
    IsBrandMyWebsite: boolean;
    ImageFilePath: string;
    CarrierOnboardingImageFilePath: string;
    FaviconFilePath: string;
    ButtonColor: string;
    BackgroundColor: string;
    ForegroundColor: string;
    IconColor: string;
    FontColor: string;
    HeaderColor: string;
}


