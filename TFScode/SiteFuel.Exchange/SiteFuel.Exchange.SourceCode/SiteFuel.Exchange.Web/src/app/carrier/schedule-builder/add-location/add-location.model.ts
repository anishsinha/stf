import { DropdownItem } from "src/app/statelist.service";

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

export class StateModel {
    CountryCode: string;
    CountryId: number;
    CountryGroupId?: number;
    QuantityIndicatorId: number;
    StateCode: string;
    StateId: number;
    StateName: string;
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

    constructor(countryId?: number) {
        
        this.ZoomArea = 15;
        this.IconUrl = { url: 'https://maps.google.com/mapfiles/ms/icons/blue-dot.png', scaledSize: { width: 40, height: 40 } }

        if (countryId == 2) {
            this.CenterLat = 56.14;
            this.CenterLon = -106.34;
        }
        else if (countryId == 3) {
            this.CenterLat = 28.61;
            this.CenterLon = 77.23;
        }
        else {
            this.CenterLat = 38;
            this.CenterLon = -98.35;
        }
    }
}
export const RackAvgTypes: DropdownItem[] = [
    { Id: 1, Name: '+$', Code: '' },
    { Id: 2, Name: '-$', Code: '' },
    { Id: 3, Name: '+%', Code: '' },
    { Id: 4, Name: '-%', Code: '' }
]