using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Models
{
    public class PricingCodesResponseModel : BaseResponseModel
    {
        public PricingCodesResponseModel()
        {

        }

        public PricingCodesResponseModel(Status status) : base(status)
        {
            Status = status;
        }

        public List<PricingCodesModel> PricingCodes { get; set; }
    }

    public class PricingCodesModel
    {
        public int Id { get; set; }

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
}
