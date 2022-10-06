using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class TankAdditionalDetailViewModel : BaseViewModel
    {
        public TankAdditionalDetailViewModel()
        {
            InstanceInitialize();
        }

        public TankAdditionalDetailViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            ThresholdDeliveryRequest = 30;
        }

        public int AssetId { get; set; }

        [Display(Name = nameof(Resource.lblTankID), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string TankId { get; set; }

        [Display(Name = nameof(Resource.lblStorageId), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string StorageId { get; set; }

        [Display(Name = nameof(Resource.lblTankType), ResourceType = typeof(Resource))]
        public Nullable<TankType> TankType { get; set; }

        public string TankModelTypeId { get; set; }

        [Display(Name = nameof(Resource.lblTankNumber), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string TankNumber { get; set; }

        [Display(Name = nameof(Resource.lblInventoryDataCaptureMethod), ResourceType = typeof(Resource))]
        public Nullable<DipTestMethod> DipTestMethod { get; set; }

        [Display(Name = nameof(Resource.lblFuelCapacity), ResourceType = typeof(Resource))]
        [Range(0, 9999999999, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public Nullable<decimal> FuelCapacity { get; set; }

        [Display(Name = nameof(Resource.lblManufacturer), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Manufacturer { get; set; }

        [Display(Name = nameof(Resource.lblThresholdDeliveryRequest), ResourceType = typeof(Resource))]
        public Nullable<decimal> ThresholdDeliveryRequest { get; set; }

        [Display(Name = nameof(Resource.lblMinFill), ResourceType = typeof(Resource))]
        public Nullable<decimal> MinFill { get; set; }

        [Display(Name = nameof(Resource.lblFillType), ResourceType = typeof(Resource))]
        public Nullable<FillType> FillType { get; set; }

        [Display(Name = nameof(Resource.lblMaxFill), ResourceType = typeof(Resource))]
        public Nullable<decimal> MaxFill { get; set; }

        [Display(Name = nameof(Resource.lblMaxFill), ResourceType = typeof(Resource))]
        public Nullable<decimal> MaxFillPercent { get; set; }

        [Display(Name = nameof(Resource.lblMinFill), ResourceType = typeof(Resource))]
        public Nullable<decimal> MinFillPercent { get; set; }

        [Display(Name = nameof(Resource.lblPhysicalPumpStop), ResourceType = typeof(Resource))]
        public Nullable<decimal> PhysicalPumpStop { get; set; }

        [Display(Name = nameof(Resource.lblRunOutLevel), ResourceType = typeof(Resource))]
        public Nullable<decimal> RunOutLevel { get; set; }

        [Display(Name = nameof(Resource.lblNotificationUponUsageSwing), ResourceType = typeof(Resource))]
        public Nullable<decimal> NotificationUponUsageSwing { get; set; }

        [Display(Name = nameof(Resource.lblNotificationUponUsageSwingValue), ResourceType = typeof(Resource))]
        public Nullable<decimal> NotificationUponUsageSwingValue { get; set; }

        [Display(Name = nameof(Resource.lblNotificationUponInventorySwing), ResourceType = typeof(Resource))]
        public Nullable<decimal> NotificationUponInventorySwing { get; set; }

        [Display(Name = nameof(Resource.lblNotificationUponInventorySwingValue), ResourceType = typeof(Resource))]
        public Nullable<decimal> NotificationUponInventorySwingValue { get; set; }

        [Display(Name = nameof(Resource.lblManifolded), ResourceType = typeof(Resource))]
        public Nullable<ManiFolded> ManiFolded { get; set; }

        [Display(Name = nameof(Resource.lblTankConstruction), ResourceType = typeof(Resource))]
        public Nullable<TankConstruction> TankConstruction { get; set; }

        [Display(Name = nameof(Resource.lblTankAcceptsDeliveries), ResourceType = typeof(Resource))]
        public string TankAcceptDelivery { get; set; }
        [Display(Name = nameof(Resource.lblTankName), ResourceType = typeof(Resource))]
        public string TankName { get; set; }
        public List<int> TanksConnected { get; set; }

        [Display(Name = nameof(Resource.lblTankSequence), ResourceType = typeof(Resource))]
        [Range(0, 99999, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public Nullable<int> TankSequence { get; set; }
        public string PedigreeAssetDBID { get; set; }

        public string SkyBitzRTUID { get; set; }

        public string ExternalTankId { get; set; }

        [Display(Name = nameof(Resource.lblWaterLevel), ResourceType = typeof(Resource))]
        public Nullable<decimal> WaterLevel { get; set; } = 0;
        public bool IsStopATGPolling { get; set; }
    }
}
