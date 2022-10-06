export class OrderDetailModel {
	public OrderId: number;
	public TfxPoNumber: string;
	public FuelType: string;
	public Quantity: string;
	public DisplayPrice: string;
}

export class OrderList {
	constructor() {
		this.Order = new OrderDetailModel();
	}
	MinVolume: number;
	MaxVolume: number;
	OrderId: number;
	Order: OrderDetailModel;
}

export class CurrentUser {
	IsBuyerCompany: boolean;
	IsSupplierCompany: boolean;
}

export class OrderGroupViewModel {
	Id: number;
	BuyerCompanyId: number;
	JobId: number;
	SupplierCompanyId: number;
	ProductType: number;
	StartDate: string;
	RenewalFrequency: number;
	RenewalCount: number;
	GroupType: number;
	GroupPoNumber: string;

	OrderList: OrderList[];
}