

export class MarinePortModel {
    constructor() {
        this.IsGeoCoded = false;
        this.Latitude = null;
        this.Longitude = null;
    }
    CustomerName?: string;
    CustomerId?: number;
    CountryId?: number;
    CountryCode?: string;
    JobID?: number;
    JobName?: string;
    Latitude?: number;
    Longitude?: number;
    Address?: string;
    City?: string;
    CityId?: number;
    State?: string;
    StateID?: number;
    ZipCode?: string;
    ContactPersonName?: string;
    ContactPhoneNumber?: string;
    IsGeoCoded: boolean;
    CountyName?: string;
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
export class MapIconUrl {
    url: string;
    scaledSize: MapIconSize;
}
export class MapIconSize {
    width: number;
    height: number
}
export class MapConstants {
    CenterLat: number;
    CenterLon: number;
    ZoomArea: number;
    IconUrl: MapIconUrl;

    constructor() {
        this.CenterLat = 38;
        this.CenterLon = -98.35;
        this.ZoomArea = 15;
        this.IconUrl = { url: 'https://maps.google.com/mapfiles/ms/icons/blue-dot.png', scaledSize: { width: 40, height: 40 } }
    }
}

export class MarineVesselsModel {
    Id: number;
    Name: string;
    IMONumber: string;
    Flag: string;
    //Customers: DropdownItem[];
    CountryId: number;
}
