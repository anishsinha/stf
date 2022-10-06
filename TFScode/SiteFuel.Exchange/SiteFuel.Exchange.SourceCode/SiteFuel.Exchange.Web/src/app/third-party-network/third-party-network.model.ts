export interface CarrierDetailsModel {
    Id: number;
    CompanyName: string;
    Email: string;
    PhoneNumber: string;
    ContactInformation: string;
    CompanyAddress: string;
    ServiceOffered: string;
    FtlTrailers: number;
    LtlTrailers: number;
    DefTrailers: number;
}


export interface ThirdPartyCompanyFilter {
    CountryId: number;
    States: string;
    ZipCodes: string;
    ServicesOffered: string;
    IsPump: boolean;
    IsMetered: boolean;
    IsPackagedGoods: boolean;
}

