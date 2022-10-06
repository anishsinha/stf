using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspGetFuelRequestFeeDetailViewModel
    {
        public int FeeId { get; set; }
        public int FeeTypeId { get; set; }
        public decimal Fee { get; set; }
        public int FeeSubTypeId { get; set; }
        public bool IncludeInPPG { get; set; }
        public string FeeTypeName { get; set; }
        public string FeeSubTypeName { get; set; }
        public decimal? MinimumGallons { get; set; }
        public decimal? FeeByQuantityMinQuantity { get; set; }
        public int? FeeByQuantityId { get; set; }
        public int? FeeByQuantityTypeId { get; set; }
        public int? FeeByQuantitySubTypeId { get; set; }
        public decimal? FeeByQuantityMaxQuantity { get; set; }
        public decimal? FeeByQuantityFee { get; set; }
        public int? DiscountLineItemId { get; set; }
        public Currency Currency { get; set; }
        public UoM UoM { get; set; }
        public int? OtherFeeTypeId { get; set; }
        public decimal? TotalFee { get; set; }
        public decimal? FeeSubQuantity { get; set; }
        public int? FeeConstraintTypeId { get; set; }
        public DateTimeOffset? SpecialDate { get; set; }
        public int? WaiveOffTime { get; set; }
        public string FeeDetails { get; set; }
        public int? TruckLoadCategoryId { get; set; }
        public string OtherFeeDescription { get; set; }
    }
}
