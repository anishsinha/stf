using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Forcasting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetViewModel : BaseViewModel
    {
        public AssetViewModel()
        {
            InstanceInitialize();
        }

        public AssetViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            FuelType = new AssetFuelTypeViewModel();
            AssetAdditionalDetail = new AssetAdditionalDetailViewModel(status);
            FuelTypes = new List<DropdownDisplayItem>();
            //ForcastingPreference = new ForcastingPreferenceViewModel();
        }

        public int UserId { get; set; }

        public int Id { get; set; }

        public int CompanyId { get; set; }

        [Display(Name = nameof(Resource.lblName), ResourceType = typeof(Resource))]
        [Remote("IsValidAssetName", "Validation", AreaReference.UseRoot, AdditionalFields = "Id,CompanyId,Type,JobId", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string Name { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        [Display(Name = nameof(Resource.lblProductType), ResourceType = typeof(Resource))]
        public AssetFuelTypeViewModel FuelType { get; set; }

        public ImageViewModel Image { get; set; }

        public AssetAdditionalDetailViewModel AssetAdditionalDetail { get; set; }

        public Nullable<int> JobId { get; set; }

        public string JobDisplayId { get; set; }

        public int MaxAllowedFileSize { get; set; }

        public bool IsAssetAssigned { get; set; }

        public int Type { get; set; }

        public bool IsJobDetails { get; set; }

        public UoM UoM { get; set; }

        public string JobName { get; set; }

        public ForcastingPreferenceViewModel ForcastingPreference { get; set; } = new ForcastingPreferenceViewModel();

        [Display(Name = nameof(Resource.lblMarinAsset), ResourceType = typeof(Resource))]
        public bool IsMarine { get; set; }

        [Display(Name = nameof(Resource.lblFuelType), ResourceType = typeof(Resource))]
        public int? AssetTankFuelTypeId { get; set; }
        public List<DropdownDisplayItem> FuelTypes { get; set; }
        public string TFXFuelType { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }
        public string IMONumber { get; set; }
        public string Flag { get; set; }

        public int CountryId { get; set; }
    }
}
