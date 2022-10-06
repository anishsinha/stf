using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class DifferentFuelPriceViewModel : StatusViewModel
    {
        public DifferentFuelPriceViewModel()
        {
            InstanceInitialize();
        }

        public DifferentFuelPriceViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            MinQuantity = 1;
            PricePerGallon = 0;
            PricingTypeId = (int)PricingType.RackAverage;
        }

        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblFrom), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal MinQuantity { get; set; }

        [Display(Name = nameof(Resource.lblUpto), ResourceType = typeof(Resource))]
        [GreaterThan("MinQuantity", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        public decimal? MaxQuantity { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int PricingTypeId { get; set; }

        [Display(Name = nameof(Resource.lblRackAvgType), ResourceType = typeof(Resource))]
        [RequiredIf("PricingTypeId", (int)PricingType.RackAverage, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public Nullable<int> RackAvgTypeId { get; set; }

        [RegularExpression(@"^((\d+)|(\d+(\.\d{1,8})))$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Display(Name = nameof(Resource.lblPricePerGallon), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal? PricePerGallon { get; set; }

        public int? OfferPricingId { get; set; }
    }
}
