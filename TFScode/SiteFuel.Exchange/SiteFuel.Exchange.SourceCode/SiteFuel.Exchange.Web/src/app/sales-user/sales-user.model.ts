import * as internal from "stream";
import { DropDown } from "../app.constants";
import { UoM } from "../app.enum";

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

export class FeeModel {
    public OrderId: number;
    public Currency: number;
    public TruckLoadType: number;
    public TruckLoadCategoryId: number;
    public CommonFee: boolean;
    public FeeConstraintTypeId: number;
    public SpecialDate: string;
    public FeeTypeId: string;
    public FeeSubTypeId: number;
    public OtherFeeDescription: string;
    public MinimumGallons: number;
    public DeliveryFeeByQuantity: ByQuantityModel[];
    public Fee: number;
    public IncludeInPPG: boolean;
}

export class ByQuantityModel {
    Currency: number;
    MinQuantity: number = 0;
    MaxQuantity: number;
    Fee: number;
}

export class ContactPersonModel {
    public ContactPersonModel() {
        this.IsPhoneNumberConfirmed = true;
    }
    public Name: string;
    public PhoneNumber: string;
    public Email: string;
    public IsPhoneNumberConfirmed: boolean;
}

export class GeneralNote {
    UserName: string;
    Note: string;
    CreatedDate: string;
}

export class SalesDRModel {
    constructor() {
        this.CompanyId = 0;
        this.CompanyId = 0;
        this.CompanyName = "";
        this.DRNotes = "";
        this.JobId = 0;
        this.JobName = "";
        this.Products = [];
    }
    CompanyId: number;
    CompanyName: string;
    JobId: number;
    JobName: string;
    Products: ProductsGroup[];
    DRNotes: string;
}

export class CustomersAndJobs
{
    CustomerId:number
    CustomerName:string
    JobId:number
    JobName:string
}

export class DDL {
    Id: number;
    Name: string;
}
export class ProductsGroup {
    Quantity: number;
    UoM: UoM;
    StartDate: string;
    StartTime: string;
    EndTime: string;
    DRPO: string;
    FuelTypes: {Id: number, Name: string};
    FuelTypeId: number;
    FuelName: string;
}

export class CustomersModel
{
    regionsAndJobsModels: RegionsAndJobsModel[] = [];
    customersandJobs: CustomersAndJobs[] = [];
}
export class RegionsAndJobsModel
{
    RegionId: string;
    CompanyId: number;
    Jobs: DDL[];
}

export enum SalesUserDRStatus{
    Success = 1,
    Error = 2,
    RegionNotFound = 3,
    FuelRequestNotFound = 4,
    OrderNotFound = 5
}

export enum ConfirmDRStatus
{
    Success = 0,
    Failed = 1,
    Warning = 2,
}

export class SalesUserDRStatusModel
{
    State: SalesUserDRStatus;
    Code: number;
    Message:string;
}
export class SalesUserDRProductStatus
{
     Status:SalesUserDRStatusModel;
     Product: ProductsGroup;
}

export class ValidateDREntryResponse {

    ProductStatuses: SalesUserDRProductStatus[];
    RaiseDeliveryRequestInput: DeliveryRequestInputModel[];
}

export class DeliveryRequestInputModel {
    JobId: number;
    RequiredQuantity: number;
    UserId: number;
    AssignedToRegionId: string;
    BuyerCompanyId: number;
    UoM: number;
    FuelType: string;
    ScheduleQuantityType: number;
    CustomerCompany: string;
    DeliveryDateStartTime: string;
    WindowStartTime:string;
    WindowEndTime: string;
    DispatcherNote: string;
    WindowStartDate: string;
    JobName: string;
    SupplierCompanyId: number;
    DeliveryRequestFor: number;
    OrderId: number;
    TankId: string;
    StorageId: string;
    SelectedDate: string;
}

export class CustomCompanyModel {
    CompanyId: number;
    CompanyName: string;
}