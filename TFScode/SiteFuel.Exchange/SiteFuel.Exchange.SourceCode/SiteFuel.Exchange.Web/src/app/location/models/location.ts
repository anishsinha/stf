export class LocationDetailsModel {
    public Id: number;
    public Name: string;
    public Abbreviation: string;
    public ControlNumber: string;
    public Address: string;
    public StateCode: string;
    public Latitude: number;
    public Longitude: number;
    public City: string;
    public County: string;
    public ZipCode: string;
    public StateId: number;
    public CountryId: number;
    public CountryCode: string;
    public CountryGroupId: number;
}

export enum Country {
    "USA" = 1,
    "CAN" = 2,
    "CAR" = 4
}