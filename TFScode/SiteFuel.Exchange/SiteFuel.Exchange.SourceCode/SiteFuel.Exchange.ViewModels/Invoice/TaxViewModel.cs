using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace SiteFuel.Exchange.ViewModels
{
    public class TaxViewModel
    {
        public TaxViewModel()
        {
            TaxPricingTypeId = (int)OtherProductTaxPricingType.DollarOnTotalAmount;
        }

        public int? OrderId { get; set; }

        public int TaxId { get; set; }

        public int TaxPricingTypeId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMaximumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblTaxDescription), ResourceType = typeof(Resource))]
        public string TaxDescription { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblTaxAmount), ResourceType = typeof(Resource))]
        public decimal TaxAmount { get; set; }
    }
}
