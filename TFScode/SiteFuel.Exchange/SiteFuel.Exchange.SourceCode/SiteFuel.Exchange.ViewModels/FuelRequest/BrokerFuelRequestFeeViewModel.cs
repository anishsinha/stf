using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class BrokerFuelRequestFeeViewModel : StatusViewModel
    {
        public BrokerFuelRequestFeeViewModel()
        {
            InstanceInitialize();
        }

        public BrokerFuelRequestFeeViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            AdditionalFee = new List<AdditionalFeeViewModel>();
            DeliveryFeeByQuantity = new List<DeliveryFeeByQuantityViewModel>();

            DeliveryFeeTypeId = (int)FeeType.DeliveryFee;
            WetHoseFeeTypeId = (int)FeeType.WetHoseFee;
            OverWaterFeeTypeId = (int)FeeType.OverWaterFee;
            DryRunFeeTypeId = (int)FeeType.DryRunFee;
            UnderGallonFeeTypeId = (int)FeeType.UnderGallonFee;
            AdditionalFeeTypeId = (int)FeeType.AdditionalFee;

            DeliveryFeeSubTypeId = (int)FeeSubType.NoFee;
            WetHoseFeeSubTypeId = (int)FeeSubType.NoFee;
            OverWaterFeeSubTypeId = (int)FeeSubType.NoFee;
            DryRunFeeSubTypeId = (int)FeeSubType.NoFee;
            UnderGallonFeeSubTypeId = (int)FeeSubType.NoFee;
            DeliveryTypeId = (int)DeliveryType.OneTimeDelivery;

            DeliveryMargin = new BrokerMarginViewModel();
            OverWaterMargin = new BrokerMarginViewModel();
            WetHoseMargin = new BrokerMarginViewModel();
            DryRunMargin = new BrokerMarginViewModel();
            AdditionalFeeMargin = new BrokerMarginViewModel();
            UnderGallonMargin = new BrokerMarginViewModel();
        }

        public int FuelRequestId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblDeliveryFee), ResourceType = typeof(Resource))]
        public int DeliveryFeeSubTypeId { get; set; }

        [RequiredIf("DeliveryFeeSubTypeId", (int)FeeSubType.FlatFee, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFlatFee), ResourceType = typeof(Resource))]
        public decimal DeliveryFee { get; set; }

        public BrokerMarginViewModel DeliveryMargin { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblWetHoseFee), ResourceType = typeof(Resource))]
        public int WetHoseFeeSubTypeId { get; set; }

        [RequiredIfMultiple("WetHoseFeeSubTypeId", (int)FeeSubType.ByAssetCount, (int)FeeSubType.HourlyRate, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblPerAsset), ResourceType = typeof(Resource))]
        public decimal WetHoseFee { get; set; }

        public BrokerMarginViewModel WetHoseMargin { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblOverWaterFee), ResourceType = typeof(Resource))]
        public int OverWaterFeeSubTypeId { get; set; }

        [RequiredIfMultiple("OverWaterFeeSubTypeId", (int)FeeSubType.ByAssetCount, (int)FeeSubType.HourlyRate, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblPerAsset), ResourceType = typeof(Resource))]
        public decimal OverWaterFee { get; set; }

        public BrokerMarginViewModel OverWaterMargin { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblDryRunFee), ResourceType = typeof(Resource))]
        public int DryRunFeeSubTypeId { get; set; }

        [RequiredIf("DryRunFeeSubTypeId", (int)FeeSubType.FlatFee, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFlatFee), ResourceType = typeof(Resource))]
        public decimal DryRunFee { get; set; }

        public BrokerMarginViewModel DryRunMargin { get; set; }

        public int DeliveryTypeId { get; set; }

        public BrokerMarginViewModel AdditionalFeeMargin { get; set; }
        public List<AdditionalFeeViewModel> AdditionalFee { get; set; }

        [RequiredIf("DeliveryFeeSubTypeId", (int)FeeSubType.ByQuantity, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblByQuantity), ResourceType = typeof(Resource))]
        public List<DeliveryFeeByQuantityViewModel> DeliveryFeeByQuantity { get; set; }

        public int UnderGallonFeeTypeId { get; set; }

        [Display(Name = nameof(Resource.lblMinimumGallonFee), ResourceType = typeof(Resource))]
        public int UnderGallonFeeSubTypeId { get; set; }

        [RequiredIf("UnderGallonFeeSubTypeId", (int)FeeSubType.FlatFee, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFee), ResourceType = typeof(Resource))]
        public decimal? UnderGallonFee { get; set; }

        [RequiredIf("UnderGallonFeeSubTypeId", (int)FeeSubType.FlatFee, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblMinGallons), ResourceType = typeof(Resource))]
        public decimal? MinimumGallons { get; set; }

        public BrokerMarginViewModel UnderGallonMargin { get; set; }

        public int DeliveryFeeTypeId { get; set; }
        public int WetHoseFeeTypeId { get; set; }
        public int OverWaterFeeTypeId { get; set; }
        public int DryRunFeeTypeId { get; set; }
        public int AdditionalFeeTypeId { get; set; }
    }
}
