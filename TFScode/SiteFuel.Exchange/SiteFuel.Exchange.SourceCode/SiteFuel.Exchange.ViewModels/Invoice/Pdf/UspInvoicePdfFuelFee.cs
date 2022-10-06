using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Invoice.Pdf
{
    public class UspInvoicePdfFuelFee
    {
        public int FeeTypeId { get; set; }
        public int FeeSubTypeId { get; set; }
        public int? OtherFeeTypeId { get; set; }
        public string FeeTypeName { get; set; }
        public string OtherFeeName { get; set; }
        public string FeeSubTypeName { get; set; }
        public decimal Fee { get; set; }
        public decimal? FeeSubQuantity { get; set; }
        public decimal? TotalFee { get; set; }
        public bool IncludeInPPG { get; set; }
        public decimal? MinimumGallons { get; set; }
        public int? FeeConstraintTypeId { get; set; }
        public DateTimeOffset? SpecialDate { get; set; }
        public int? DiscountLineItemId { get; set; }
        public string OtherFeeCode { get; set; }
        public int? FeeByQuantityId { get; set; }
        public int? FeeByQuantityTypeId { get; set; }
        public int? FeeByQuantitySubTypeId { get; set; }
        public decimal? FeeByQuantityMinQuantity { get; set; }
        public decimal? FeeByQuantityMaxQuantity { get; set; }
        public decimal? FeeByQuantityFee { get; set; }
        public int UoM { get; set; }
        public int Currency { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public int? WaiveOffTime { get; set; }
        public int? TruckLoadCategoryId { get; set; }
        public int InvoiceId { get; set; }
        public int InvoiceTypeId { get; set; }
        public decimal DroppedGallons { get; set; }
        public bool IsSurchargeApplicable { get; set; }
        public int SurchargePricingType { get; set; }
        public decimal? SurchargePercentage { get; set; }
        public decimal? SurchargeEIAPrice { get; set; }
        public bool IsFreightCostApplicable { get; set; }
        public int? FreightRateRuleType { get; set; }
        public decimal? Distance { get; set; }
    }
}
