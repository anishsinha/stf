using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class HaulerPricingMatrixViewModel
    {
        public HaulerPricingMatrixViewModel()
        {
            MinGallons = 1;
        }

        public int Id { get; set; }

        public int CompanyId { get; set; }

        [Display(Name = nameof(Resource.lblMinGallons), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal MinGallons { get; set; }

        [Display(Name = nameof(Resource.lblMaxGallons), ResourceType = typeof(Resource))]
        [GreaterThan("MinGallons", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        public decimal? MaxGallons { get; set; }

        [RegularExpression(@"^((\d+)|(\d+(\.\d{1,8})))$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}
