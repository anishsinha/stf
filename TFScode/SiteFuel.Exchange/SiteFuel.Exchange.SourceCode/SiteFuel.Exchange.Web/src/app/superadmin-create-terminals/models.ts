import { DropDownItem } from "../buyer-wally-board/Models/BuyerWallyBoard";


export class TerminalDetailsModel {

    constructor() {
        this.IsGeoCoded = false;
    }
    Id: number;
    Name?: string;
    Abbreviation?: number;
    ControlNumber?: number;
    CountryCode?: string;   
    Latitude?: number;
    Longitude?: number;
    Address?: string;
    City?: string;
    CityId?: number;
    StateCode?: string;
    StateId?: number;
    ZipCode?: string;
    IsGeoCoded: boolean;
    County?: string;
    TerminalOwner?: string;
    CountryId?: number;
}

export class Geocode {
    Address: string;
    Latitude: number;
    Longitude: number;
    CountyName: string;
    StateCode: string;
    StateName: string;
    City: string;
    CountryCode: string;
    CountryName: string;
    ZipCode: string;
}


export class TerminalMappedProductsGridModel {
    constructor() {
        this.MappedProducts = [];
    }
    TerminalId: number;
    TerminalControlNumber: string;
    TerminalName: string;
    AssignedProducts: string;
    MappedProductCount: number;
    MappedProducts: DropDownItem[];
}