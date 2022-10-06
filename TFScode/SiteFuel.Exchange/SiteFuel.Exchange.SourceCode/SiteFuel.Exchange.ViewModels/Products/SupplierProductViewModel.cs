using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class SupplierProductViewModel : StatusViewModel
    {
        public SupplierProductViewModel()
        {
            ProductMappingFuelTypeDetailsViewModels = new List<ProductMappingFuelTypeDetailsViewModel>();
            DisplayMode = PageDisplayMode.Create;
            StatusCode = Status.Success;
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblAssignName), ResourceType = typeof(Resource))]
        public string AssignedName { get; set; }
        public string MyProductId { get; set; }
        public string BackOfficeProductId { get; set; }

        public string DriverProductId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblTerminalName), ResourceType = typeof(Resource))]
        public int TerminalId { get; set; }
        public int FuelTypeId { get; set; }
        public int AssignCompanyId { get; set; }

        public List<ProductMappingFuelTypeDetailsViewModel> ProductMappingFuelTypeDetailsViewModels { get; set; } = new List<ProductMappingFuelTypeDetailsViewModel>();
    }
}
