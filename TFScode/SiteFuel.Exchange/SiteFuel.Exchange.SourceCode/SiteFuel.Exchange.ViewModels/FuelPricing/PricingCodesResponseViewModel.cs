using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class PricingCodesApiResponse
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public List<PricingCodesViewModel> PricingCodes { get; set; }
    }

    public class PricingCodesViewModel
    {
        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblPricingCode), ResourceType = typeof(Resource))]
        public string Code { get; set; }

        public int PricingSourceId { get; set; }

        public int PricingTypeId { get; set; }

        public int FeedTypeId { get; set; }

        public int QuantityIndicatorId { get; set; }

        public int FuelClassTypeId { get; set; }

        public int RackTypeId { get; set; }

        public int WeekendPricingTypeId { get; set; }

        public string PricingSource { get; set; }

        public string PricingType { get; set; }

        public string FeedType { get; set; }

        public string FuelClassType { get; set; }

        public string QuantityIndicator { get; set; }

        public string RackAvgPricingType { get; set; }

        public string WeekendPricingDay { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
    }

    public class PricingCodeDetailViewModel
    {
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int Id { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblPricingCode), ResourceType = typeof(Resource))]
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
