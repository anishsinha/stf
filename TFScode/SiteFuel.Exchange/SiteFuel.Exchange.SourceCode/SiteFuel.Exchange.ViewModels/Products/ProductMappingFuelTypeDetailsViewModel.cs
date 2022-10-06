using SiteFuel.Exchange.Core.StringResources;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class ProductMappingFuelTypeDetailsViewModel
    {
        public ProductMappingFuelTypeDetailsViewModel()
        {
        }

        [Display(Name = nameof(Resource.lblBackOfficeProductCode), ResourceType = typeof(Resource))]
        public string BackOfficeProductCode { get; set; }

        [Display(Name = nameof(Resource.lblSeaboardProductCode), ResourceType = typeof(Resource))]
        public string SeaboardProductCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFuelType), ResourceType = typeof(Resource))]
        public int FuelTypeId { get; set; }
        public int Id { get; set; }
        public int CompanyId { get; set; }

        public string CollectionHtmlPrefix { get; set; }
    }
}
