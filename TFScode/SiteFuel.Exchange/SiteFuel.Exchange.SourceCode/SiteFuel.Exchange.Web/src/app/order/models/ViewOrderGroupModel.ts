import { DropdownItemExt, DropdownItem } from 'src/app/statelist.service';

export class ViewOrderGroupModel {
    public BuyerCompanyId: number;
    public JobId: number;
    public SupplierCompanyId: number;   
    public GroupId: number;
    public CustomerId: number;
    public StateId: number;
    public ProductCategoryId: number; 
    public OrderGroupDetails: OrderGroupDetailModel[] = [];
}

export class OrderGroupDetailModel {
	public OrderGroupId: number;
    public CustomerPoNumber: string;
    public FuelType: string;
    public JobName: string;
    public JobAddress: string;
    public RenewalFrequency: string;
    public GroupType: number;
    public DisplayGroupType: string;    
    public DisplayProductType: string;
    public BlendedGroupWeightedPPG: number;
    public DisplayBlendedGroupWeightedPPG: string;
    public OrderDrops: OrderDropDetailModel[] = [];
}

export class OrderDropDetailModel {
    public TfxPoNumber: string;
    public FuelType: string;
    public DroppedGallons: number;
    public FuelDeliveredPercentage: number;
    public DropPercentage: string;
    public BlendRatioPercentage: number;
    public MinVolume: number;
    public MaxVolume: number;
    public PPG: number;
    public DisplayPPG: string;
    public UoM: string;
    public QuantityType: number;    
    public IsEditOrDeleteAllowed: boolean;
    public CanCurrentUserEditOrDeleteGroup: boolean;
}

export class AddressModel {   
    public Address: string;
    public Latitude: number;
    public Longitude: number;
    public CountyName: string;
    public StateCode: string;
    public City: string;
    public CountryCode: string;
    public ZipCode: string;    
}