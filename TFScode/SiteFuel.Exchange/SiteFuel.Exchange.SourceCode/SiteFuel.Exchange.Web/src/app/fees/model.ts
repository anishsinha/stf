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

export function IsMatched(x: FeeModel, y: FeeModel) {
    return y.FeeTypeId == x.FeeTypeId
        && y.FeeSubTypeId == x.FeeSubTypeId
        && y.CommonFee == x.CommonFee
        && y.FeeConstraintTypeId == x.FeeConstraintTypeId;
};

