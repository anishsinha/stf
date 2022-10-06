using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class ProductTypeMappingViewModel
    {
        public int ProductTypeId { get; set; }

        [Display(Name = nameof(Resource.lblProductType), ResourceType = typeof(Resource))]
        public string ProductType { get; set; }

        public string MappedToProductType { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.gridColumnMappedToProductType), ResourceType = typeof(Resource))]
        public List<int> MappedToProductTypeIds { get; set; } = new List<int>();

        public bool IsBlend { get; set; }
    }
}
