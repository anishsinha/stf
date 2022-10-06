using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelSurchargeTableViewModel
    {      
        [Display(Name = nameof(Resource.lblPriceRangeStartValue), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal PriceRangeStartValue { get; set; }

        [Display(Name = nameof(Resource.lblPriceRangeEndValue), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(typeof(Decimal), ApplicationConstants.DecimalMinValue, ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public decimal PriceRangeEndValue { get; set; }

        [Display(Name = nameof(Resource.lblFuelSurchargeStartPercentage), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(typeof(Decimal), ApplicationConstants.DecimalMinValue, ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public decimal FuelSurchargeStartPercentage { get; set; }

        public decimal PriceRangeInterval { get; set; }

        public int Id { get; set; }

        public int? SupplierId { get; set; }
    }
}
