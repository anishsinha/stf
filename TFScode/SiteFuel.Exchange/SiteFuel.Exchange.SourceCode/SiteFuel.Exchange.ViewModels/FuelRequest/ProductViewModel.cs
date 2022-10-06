using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
        }

        public int Id { get; set; }

        public string ProductName { get; set; }

        public string ProductType { get; set; }

        public string ProductDisplayGroup { get; set; }

        public string PricingCode { get; set; }

        [StringLength(256)]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblProductDisplayName), ResourceType = typeof(Resource))]
        [Remote("IsValidProductDisplayName", "Validation", AreaReference.UseRoot, AdditionalFields = "Id", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        public string DisplayName { get; set; }

        [Display(Name = nameof(Resource.lblAxxisProductName), ResourceType = typeof(Resource))]
        public int? AxxisProductId { get; set; }

        [Display(Name = nameof(Resource.lblParklandProductName), ResourceType = typeof(Resource))]
        public int? ParklandProductId { get; set; }

        public bool IsNewAxxisProduct { get; set; } = true;

        public bool IsNewOpisProduct { get; set; } = true;

        public bool IsNewPlattsProduct { get; set; } = true;

        [Display(Name = nameof(Resource.lblPlattsProductName), ResourceType = typeof(Resource))]
        public int? PlattsProductId { get; set; }

        [Display(Name = nameof(Resource.lblOpisProductName), ResourceType = typeof(Resource))]
        public int? OpisProductId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblProductCode), ResourceType = typeof(Resource))]
        public string ProductCode { get; set; }

        [Display(Name = nameof(Resource.lblProductType), ResourceType = typeof(Resource))]
        public int ProductTypeId { get; set; }

        [Display(Name = nameof(Resource.gridColumnProductDisplayGroup), ResourceType = typeof(Resource))]
        public int ProductDisplayGroupId { get; set; }

        public int TotalCount { get; set; }

        public string AxxisProductName { get; set; }

        public string OpisProductName { get; set; }

        public string PlattsProductName { get; set; }

        public string ParklandProductName { get; set; }

        [MaxLength(250, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageProductDescriptionGreaterThan))]
        [Display(Name = nameof(Resource.lblProductDescription), ResourceType = typeof(Resource))]
        public string ProductDescription { get; set; }

        public int UpdatedBy { get; set; }

        public bool isNewAxissProductAdded() 
        {
            if(IsNewAxxisProduct && !string.IsNullOrWhiteSpace(AxxisProductName) && (AxxisProductId == null || AxxisProductId <= 0)) 
            {
                return true;
            }
            return false;
        }

        public bool isNewOpisProductAdded()
        {
            if (IsNewOpisProduct && !string.IsNullOrWhiteSpace(OpisProductName) && (OpisProductId == null || OpisProductId <= 0) )
            {
                return true;
            }
            return false;
        }

        public bool isNewPlattsProductAdded()
        {
            if (IsNewPlattsProduct && !string.IsNullOrWhiteSpace(PlattsProductName) && (PlattsProductId == null || PlattsProductId <= 0))
            {
                return true;
            }
            return false;
        }
        public bool shouldAddNewAxxisProduct()
        {
            if (IsNewAxxisProduct && string.IsNullOrWhiteSpace(AxxisProductName) && (AxxisProductId == null || AxxisProductId <= 0) 
                && IsNewOpisProduct && string.IsNullOrWhiteSpace(OpisProductName) && (OpisProductId == null || OpisProductId <= 0)
                && IsNewPlattsProduct && string.IsNullOrWhiteSpace(PlattsProductName) && (PlattsProductId == null || PlattsProductId <= 0))
            {
                return true;
            }
            return false;
        }
    }

    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PricingSourceId { get; set; }
        public int DisplayGroupId { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public int? TfxProductId { get; set; }
    }
}
