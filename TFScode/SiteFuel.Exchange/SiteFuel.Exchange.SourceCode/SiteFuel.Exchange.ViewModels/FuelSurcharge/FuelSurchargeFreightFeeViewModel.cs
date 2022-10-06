using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelSurchargeFreightFeeViewModel
    {
        public FuelSurchargeFreightFeeViewModel()
        {
            Currency = Currency.USD;
            UoM = UoM.Gallons;
            DeliveryFeeByQuantity = new List<DeliveryFeeByQuantityViewModel>();
            FeeTypeId = (int)FeeType.SurchargeFreightFee;
            FeeSubTypeId = (int)FeeSubType.FlatFee;
        }

        public int Id { get; set; }

        public int FeeTypeId { get; set; }

        public int FeeSubTypeId { get; set; }

        public string FeeSubTypeName { get; set; }

        [RequiredIfMultiple("FeeSubTypeId", (int)FeeSubType.FlatFee, (int)FeeSubType.ByQuantity, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal Fee { get; set; }

        public int? FeeConstraintTypeId { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public List<DeliveryFeeByQuantityViewModel> DeliveryFeeByQuantity { get; set; }

        //for invoice
        [Display(Name = nameof(Resource.lblApplyFuelSurchage), ResourceType = typeof(Resource))]
        public bool IsSurchargeApplicable { get; set; }

        [Display(Name = nameof(Resource.lblApplyFreightCost), ResourceType = typeof(Resource))]
        public bool IsFreightCostApplicable { get; set; }

        [Display(Name = nameof(Resource.lblFuelSurchargePricingType), ResourceType = typeof(Resource))]
        public FuelSurchagePricingType SurchargePricingType { get; set; }

        public SurchargeProductTypes SurchargeProductType { get; set; }

        [Display(Name = nameof(Resource.headingFreightCost), ResourceType = typeof(Resource))]
        public decimal SurchargeFreightCost { get; set; }

        [Display(Name = nameof(Resource.lblSurchargePercent), ResourceType = typeof(Resource))]
        public decimal SurchargePercentage { get; set; }

        [Display(Name = nameof(Resource.lblFuelSurchargeEIAPrice), ResourceType = typeof(Resource))]
        public decimal SurchargeEiaPrice { get; set; }

        [Display(Name = nameof(Resource.lblSurchargeInterval), ResourceType = typeof(Resource))]
        public decimal SurchargeTableRangeStart { get; set; }
        public decimal SurchargeTableRangeEnd { get; set; }
        public decimal? FeeSubQuantity { get; set; }
        public bool IsFeeByDistance { get; set; }

        public decimal AutoFreightDistance { get; set; }

        [Display(Name = nameof(Resource.lblDistance), ResourceType = typeof(Resource))]
        public int Distance { get; set; }
        public int DistanceUoM { get; set; } = 1; //1 for miles, 2 for km

        [Display(Name = nameof(Resource.lblTotal), ResourceType = typeof(Resource))]
        public decimal TotalFuelSurchargeFee { get; set; }
        public decimal GallonsDelivered { get; set; }
        public int BuyerCompanyId { get; set; }
        public FreightPricingMethod FreightPricingMethod { get; set; } = FreightPricingMethod.Manual;
        public TableTypes? FuelSurchargeTableType { get; set; }
        public int? FuelSurchargeTableId { get; set; }
        public FreightRateRuleType? FreightRateRuleType { get; set; }
        public TableTypes? FreightRateTableType { get; set; }
        public int? FreightRateRuleId { get; set; }
    }
}
