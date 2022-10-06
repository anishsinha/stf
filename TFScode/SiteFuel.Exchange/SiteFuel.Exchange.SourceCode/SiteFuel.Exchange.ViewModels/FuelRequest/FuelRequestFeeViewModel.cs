using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelRequestFeeViewModel : StatusViewModel
    {
        public FuelRequestFeeViewModel()
        {
            InstanceInitialize();
        }

        public FuelRequestFeeViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            AdditionalFee = new List<AdditionalFeeViewModel>();
            ResaleFee = new List<FuelRequestResaleFeeViewModel>();
            DeliveryFeeByQuantity = new List<DeliveryFeeByQuantityViewModel>();
            DeliveryFeeTypeId = (int)FeeType.DeliveryFee;
            WetHoseFeeTypeId = (int)FeeType.WetHoseFee;
            OverWaterFeeTypeId = (int)FeeType.OverWaterFee;
            DryRunFeeTypeId = (int)FeeType.DryRunFee;
            FreightFeeTypeId = (int)FeeType.FreightFee;
            AdditionalFeeTypeId = (int)FeeType.AdditionalFee;
            UnderGallonFeeTypeId = (int)FeeType.UnderGallonFee;

            DeliveryFeeSubTypeId = (int)FeeSubType.NoFee;
            WetHoseFeeSubTypeId = (int)FeeSubType.NoFee;
            OverWaterFeeSubTypeId = (int)FeeSubType.NoFee;
            DryRunFeeSubTypeId = (int)FeeSubType.NoFee;
            FreightFeeSubTypeId = (int)FeeSubType.NoFee;
            UnderGallonFeeSubTypeId = (int)FeeSubType.NoFee;
        }

        public int FuelRequestId { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblDeliveryFee), ResourceType = typeof(Resource))]
        public int DeliveryFeeSubTypeId { get; set; }

        public string DeliveryFeeType { get; set; }

        public string DeliveryFeeSubType { get; set; }

        //[RequiredIfMultiple("DeliveryFeeSubTypeId", (int)FeeSubType.FlatFee, (int)FeeSubType.PerGallon, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFee), ResourceType = typeof(Resource))]
        public decimal DeliveryFee { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblWetHoseFee), ResourceType = typeof(Resource))]
        public int WetHoseFeeSubTypeId { get; set; }

        public string WetHoseFeeType { get; set; }

        public string WetHoseFeeSubType { get; set; }

        [Display(Name = nameof(Resource.lblFee), ResourceType = typeof(Resource))]
        public decimal WetHoseFee { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblOverWaterFee), ResourceType = typeof(Resource))]
        public int OverWaterFeeSubTypeId { get; set; }

        public string OverWaterFeeType { get; set; }

        public string OverWaterFeeSubType { get; set; }

        [Display(Name = nameof(Resource.lblFee), ResourceType = typeof(Resource))]
        public decimal OverWaterFee { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblDryRunFee), ResourceType = typeof(Resource))]
        public int DryRunFeeSubTypeId { get; set; }

        public string DryRunFeeType { get; set; }

        public string DryRunFeeSubType { get; set; }

        //[RequiredIf("DryRunFeeSubTypeId", (int)FeeSubType.FlatFee, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFlatFee), ResourceType = typeof(Resource))]
        public decimal DryRunFee { get; set; }

        public List<AdditionalFeeViewModel> AdditionalFee { get; set; }

        //[RequiredIf("DeliveryFeeSubTypeId", (int)FeeSubType.ByQuantity, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblByQuantity), ResourceType = typeof(Resource))]
        public List<DeliveryFeeByQuantityViewModel> DeliveryFeeByQuantity { get; set; }

        public int DeliveryFeeTypeId { get; set; }

        public bool DeliveryFeeIncludeInPPG { get; set; }

        public int WetHoseFeeTypeId { get; set; }

        public bool WetHoseFeeIncludeInPPG { get; set; }

        public int OverWaterFeeTypeId { get; set; }

        public bool OverWaterFeeIncludeInPPG { get; set; }

        public int DryRunFeeTypeId { get; set; }

        public bool DryRunFeeIncludeInPPG { get; set; }

        public int FreightFeeTypeId { get; set; }

        public string FreightFeeType { get; set; }

        public int FreightFeeSubTypeId { get; set; }

        public string FreightFeeSubType { get; set; }

        public decimal FreightFee { get; set; }

        public int UnderGallonFeeTypeId { get; set; }

        public string UnderGallonFeeType { get; set; }

        [Display(Name = nameof(Resource.lblMinimumGallonFee), ResourceType = typeof(Resource))]
        public int UnderGallonFeeSubTypeId { get; set; }

        public string UnderGallonFeeSubType { get; set; }

        //[RequiredIf("UnderGallonFeeSubTypeId", (int)FeeSubType.FlatFee, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFee), ResourceType = typeof(Resource))]
        public decimal? UnderGallonFee { get; set; }

        //[RequiredIf("UnderGallonFeeSubTypeId", (int)FeeSubType.FlatFee, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblMinGallons), ResourceType = typeof(Resource))]
        public decimal? MinimumGallons { get; set; }

        public bool UnderGallonFeeIncludeInPPG { get; set; }

        public int AdditionalFeeTypeId { get; set; }

        public List<FuelRequestResaleFeeViewModel> ResaleFee { get; set; }
    }
}
