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
    public class ViewFuelSurchargeInputViewModel
    {
        public ViewFuelSurchargeInputViewModel()
        {
            TableTypes = new List<TableTypes>();
        }

        [Display(Name = nameof(Resource.lblTableType), ResourceType = typeof(Resource))]
        public List<TableTypes> TableTypes { get; set; }

        [Display(Name = nameof(Resource.lblProductType), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public List<SurchargeProductTypes> ProductTypes { get; set; } 

        [Display(Name = nameof(Resource.lblStartDate), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public DateTimeOffset? StartDate { get; set; }

        [Display(Name = nameof(Resource.lblEndDate), ResourceType = typeof(Resource))]
        [GreaterThanOrEqualTo("StartDate", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        public DateTimeOffset? EndDate { get; set; }

        [Display(Name = nameof(Resource.lblPriceRangeStartValue), ResourceType = typeof(Resource))]
        [LessThanOrEqualTo("PriceRangeEndValue", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        public decimal? PriceRangeStartValue { get; set; }

        [Display(Name = nameof(Resource.lblPriceRangeEndValue), ResourceType = typeof(Resource))]
        [Range(typeof(Decimal), ApplicationConstants.DecimalMinValue, ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [GreaterThanOrEqualTo("PriceRangeStartValue", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        public decimal? PriceRangeEndValue { get; set; }

        [Display(Name = nameof(Resource.lblPriceRangeInterval), ResourceType = typeof(Resource))]
        [Range(typeof(Decimal), ApplicationConstants.DecimalMinValue, ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public decimal? PriceRangeInterval { get; set; }

        [Display(Name = nameof(Resource.lblFuelSurchargeStartPercentage), ResourceType = typeof(Resource))]
        [Range(typeof(Decimal), ApplicationConstants.DecimalMinValue, ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public decimal? FuelSurchargeStartPercentage { get; set; }

        [Display(Name = nameof(Resource.lblSurchargeInterval), ResourceType = typeof(Resource))]
        [Range(typeof(Decimal), ApplicationConstants.DecimalMinValue, ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public decimal? SurchargeInterval { get; set; }

        public string TableTypeIds { get; set; }

        public string CustomerIds { get; set; }

        public string CarrierIds { get; set; }

        public string TerminalIds { get; set; }

        public string SourceRegionIds { get; set; }

        public string BulkPlantIds { get; set; }

        public bool IsArchived { get; set; }
    }
}
