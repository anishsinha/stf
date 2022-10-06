using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelQuantityViewModel : StatusViewModel
    {
        public FuelQuantityViewModel()
        {
            InstanceInitialize();
        }

        public FuelQuantityViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            QuantityTypeId = (int)QuantityType.SpecificAmount;
        }

        [Display(Name = nameof(Resource.lblQuantity), ResourceType = typeof(Resource))]
        public int QuantityTypeId { get; set; }

        [Range(typeof(Decimal), "0", ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RequiredIf("QuantityTypeId", (int)QuantityType.SpecificAmount, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal Quantity { get; set; }

        [Range(typeof(Decimal), "0", ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RequiredIf("QuantityTypeId", (int)QuantityType.Range, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [LessThanOrEqualTo("MaximumQuantity", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageLessThan), PassOnNull = true)]
        [Display(Name = nameof(Resource.lblMin), ResourceType = typeof(Resource))]
        public decimal MinimumQuantity { get; set; }

        [Range(typeof(Decimal), "0", ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RequiredIf("QuantityTypeId", (int)QuantityType.Range, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [GreaterThanOrEqualTo("MinimumQuantity", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        [Display(Name = nameof(Resource.lblMax), ResourceType = typeof(Resource))]
        public decimal MaximumQuantity { get; set; }

        [Display(Name = nameof(Resource.lblEstimatedQuantityPerDelivery), ResourceType = typeof(Resource))]
        //[LessThanOrEqualTo("MaximumQuantity", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageLessThan), PassOnNull = true)]
        public int? EstimatedGallonsPerDelivery { get; set; }

        public UoM UoM { get; set; }

        [Display(Name = nameof(Resource.lblType), ResourceType = typeof(Resource))]
        public QuantityIndicatorTypes QuantityIndicatorTypes { get; set; }
    }
}
