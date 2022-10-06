using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SiteFuel.Exchange.ViewModels
{
    public class FeesViewModel : StatusViewModel
    {
        public FeesViewModel()
        {
            InstanceInitialize();
        }

        public FeesViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            DeliveryFeeByQuantity = new List<DeliveryFeeByQuantityViewModel>();
            CommonFee = true;
            Margin = 0;
            MarginTypeId = (int)MarginType.NoChange;
        }

        public int Id { get; set; }

        public bool CommonFee { get; set; }

        public int TruckLoadType { get; set; } = (int)TruckLoadTypes.LessTruckLoad;

        public int? TruckLoadCategoryId { get; set; }

        [Display(Name = nameof(Resource.lblFeeType), ResourceType = typeof(Resource))]
        [RequiredIfTrue("CommonFee", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string FeeTypeId { get; set; }

        public string FeeTypeName { get; set; }

        public int FeeSubTypeId { get; set; }

        public string FeeSubTypeName { get; set; }

        [RequiredIfNot("FeeSubTypeId", null, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal? Fee { get; set; }

        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public int? WaiveOffTime { get; set; }

        [RequiredIf("FeeTypeId", (int)FeeType.UnderGallonFee, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal? MinimumGallons { get; set; }

        public bool IncludeInPPG { get; set; }

        [Display(Name = nameof(Resource.lblFeeName), ResourceType = typeof(Resource))]
        [RequiredIfFalse("CommonFee", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string OtherFeeDescription { get; set; }

        public bool AddToCommonFees { get; set; }

        public int CompanyId { get; set; }

        public int? OtherFeeTypeId { get; set; }

        public decimal TotalFee { get; set; }

        public decimal? FeeSubQuantity { get; set; }

        public string TotalHours { get; set; }

        public decimal TotalAssetQty { get; set; }

        public List<DeliveryFeeByQuantityViewModel> DeliveryFeeByQuantity { get; set; }

        public int MarginTypeId { get; set; }

        [RequiredIfNot("MarginTypeId", (int)MarginType.NoChange, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal? Margin { get; set; }

        public int? FeeConstraintTypeId { get; set; }

        [RequiredIf("FeeConstraintTypeId", (int)FeeConstraintType.SpecialDate, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public DateTimeOffset? SpecialDate { get; set; }

        public string SpecialDateValue { get; set; }
        
        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public int? DiscountLineItemId { get; set; }

        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public string DemmurageStart { get; set; }

        public string DemmurageEnd { get; set; }

        [Display(Name = nameof(Resource.lblTime), ResourceType = typeof(Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public int? TimeInMinutes { get; set; }

        public FeeTaxDetails FeeTaxDetails { get; set; }

        public int OrderId { get; set; }

        public int InvoiceId { get; set; }

        public int InvoiceTypeId { get; set; }

        public decimal DroppedGallons { get; set; }

        public string DisplayFeeType { get; set; }
        public string DisplayFeeName { get; set; }
        public string DisplayDroppedGallons { get; set; }
        public string DisplayFee { get; set; }
        public string DisplayTotalFee { get; set; }

        public bool IsSurchargeApplicable { get; set; }
        public int SurchargePricingType { get; set; }
        public decimal? SurchargePercentage { get; set; }
        public decimal? SurchargeEIAPrice { get; set; }
        public bool IsFreightCostApplicable { get; set; }
        public int? FreightRateRuleType { get; set; }
        public decimal? Distance { get; set; }

        public override string ToString()
        {
            var feeValue = string.Empty;
            if (Fee != 0)
            {
                if (FeeTypeId == ((int)FeeType.OtherFee).ToString())
                    feeValue = $"{OtherFeeDescription}- {FeeSubTypeName}: ${Fee}";
                else
                    feeValue = $"{FeeTypeName}- {FeeSubTypeName}: ${Fee}";
            }
            else
            {
                feeValue = $"{FeeTypeName}- {FeeSubTypeName}: ${string.Join("/", DeliveryFeeByQuantity.Select(x => x.ToString()))}";
            }
            return feeValue;
        }

        public string GetQuntityInTime()
        {
            if (FeeSubQuantity.HasValue)
            {
                var totalSeconds = Convert.ToDouble(FeeSubQuantity ?? 0);
                var hours = (int)(totalSeconds / 3600);
                var mins = (totalSeconds - (hours * 3600)) / 60;
                return hours != 0 ? string.Format(Resource.constTimeFormat, hours, string.Format(Resource.constFormatDecimal2, mins))
                                : string.Format(Resource.constMinuteFormat, string.Format(Resource.constFormatDecimal2, mins));
            }
            return string.Empty;
        }

        public FeesViewModel Clone()
        {
            var thisObject = (FeesViewModel)this.MemberwiseClone();
            return thisObject;
        }
    }

    public class FeeTaxDetails
    {
        [Display(Name = nameof(Resource.lblTaxPercentage), ResourceType = typeof(Resource))]
        public decimal? Percentage { get; set; }

        [Display(Name = nameof(Resource.lblOnlyTaxAmount), ResourceType = typeof(Resource))]
        public decimal? Amount { get; set; }

        [Display(Name = nameof(Resource.lblTaxDescription), ResourceType = typeof(Resource))]
        public string Description { get; set; }
    }
}
