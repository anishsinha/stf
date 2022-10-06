using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetDropViewModel : BaseViewModel
    {
        public AssetDropViewModel()
        {
        }

        public AssetDropViewModel(Status status)
            : base(status)
        {
            Image = new ImageViewModel();
            DropStatusName = Constants.AssetFilled;
            TankScaleMeasurement = TankScaleMeasurement.None;
        }

        public int Id { get; set; }

        public int OrderId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblName), ResourceType = typeof(Resource))]        
        public string AssetName { get; set; }

        public int JobXAssetId { get; set; }

        public int InvoiceId { get; set; }

        [Display(Name = nameof(Resource.lblDropStatus), ResourceType = typeof(Resource))]
        public int DropStatusId { get; set; }

        public string DropStatusName { get; set; }

        [Display(Name = nameof(Resource.lblFuelQuantity), ResourceType = typeof(Resource))]
        //[Range(typeof(Decimal), ApplicationConstants.DecimalMinValue, ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RequiredIf("DropStatusId", (int)DropStatus.None, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal? DropGallons { get; set; }

        public decimal PricePerGallon { get; set; }

        public DateTimeOffset DropDate { get; set; }

        public DateTimeOffset? DropEndDate { get; set; }

        [Display(Name = nameof(Resource.lblStartTime), ResourceType = typeof(Resource))]
        [RequiredIfNotEmpty("DropGallons", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string StartTime { get; set; }

        [Display(Name = nameof(Resource.lblEndTime), ResourceType = typeof(Resource))]
        [RequiredIfNotEmpty("DropGallons", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string EndTime { get; set; }

        public int DroppedBy { get; set; }

        public int? SubcontractorId { get; set; }

        public string SubcontractorName { get; set; }

        public string ContractNumber { get; set; }

        public ImageViewModel Image { get; set; }

        public decimal MeterStartReading { get; set; }

        public decimal MeterEndReading { get; set; }

        public bool IsDropMade { get; set; }

        public bool IsNewAsset { get; set; }

        public string VehicleId { get; set; }

        public UoM UoM { get; set; }

        public string SpillNotes { get; set; }

        [Display(Name = nameof(Resource.lblPreDip), ResourceType = typeof(Resource))]
        [RequiredIfNotEmpty("PostDip", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal? PreDip { get; set; }

        [Display(Name = nameof(Resource.lblPostDip), ResourceType = typeof(Resource))]
        [RequiredIfNotEmpty("PreDip", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal? PostDip { get; set; }

        [Display(Name = nameof(Resource.lblUomShort), ResourceType = typeof(Resource))]
        public TankScaleMeasurement TankScaleMeasurement { get; set; }
        public bool IsOfflineMode { get; set; }

        public string TankMakeModel { get; set; }

        public int  AssetType { get; set; }

        public bool IsDipDataRequired { get; set; }

        public decimal? Gravity { get; set; }

        public int JobCountryId { get; set; }

        public string IMONumber { get; set; }
        public string DeliveryLevelPO { get; set; }

    }
}
