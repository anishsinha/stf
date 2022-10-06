using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetAdditionalDetailViewModel : BaseViewModel
    {
        public AssetAdditionalDetailViewModel()
        {
           
        }

        public AssetAdditionalDetailViewModel(Status status)
            : base(status)
        {
            
        }


        public int AssetId { get; set; }

        [Display(Name = nameof(Resource.lblMake), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Make { get; set; }

        [Display(Name = nameof(Resource.lblModel), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Model { get; set; }

        [Display(Name = nameof(Resource.lblYear), ResourceType = typeof(Resource))]
        [RegularExpression(@"^((\d{4})|(\d{2}-\d{2})|(\d{4}-\d{4}))$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Year { get; set; }

        [Display(Name = nameof(Resource.lblContractNumber), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Color { get; set; }

        [Display(Name = nameof(Resource.lblTelematicsProvider), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string TelematicsProvider { get; set; }

        [Display(Name = nameof(Resource.lblLicensePlateState), ResourceType = typeof(Resource))]
        public Nullable<int> LicensePlateStateId { get; set; }

        [Display(Name = nameof(Resource.lblLicensePlateState), ResourceType = typeof(Resource))]
        public string LicensePlateState { get; set; }

        [Display(Name = nameof(Resource.lblLicensePlate), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string LicensePlate { get; set; }

        [Display(Name = nameof(Resource.lblClass), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Class { get; set; }

        [Display(Name = nameof(Resource.lblVendor), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Vendor { get; set; }

        [Display(Name = nameof(Resource.lblAssetId), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string VehicleId { get; set; }

        [Display(Name = nameof(Resource.lblTankID), ResourceType = typeof(Resource))]
        [StringLength(64, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        //[RequiredIf("Type", (int)AssetType.Tank, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        //[Remote("IsValidTankId", "Validation", AreaReference.UseRoot, AdditionalFields = "AssetId,StorageId", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.errMessageTankIdAndStorageIdAlreadyExists))]
        public string TankId { get; set; }

        [Display(Name = nameof(Resource.lblStorageId), ResourceType = typeof(Resource))]
        [StringLength(64, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        //[RequiredIf("Type", (int)AssetType.Tank, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        //[Remote("IsValidTankId", "Validation", AreaReference.UseRoot, AdditionalFields = "AssetId,TankId", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.errMessageTankIdAndStorageIdAlreadyExists))]
        public string StorageId { get; set; }

        [Display(Name = nameof(Resource.lblTankNumber), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string TankNumber { get; set; }

        [Display(Name = nameof(Resource.lblManufacturer), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Manufacturer { get; set; }

        [Display(Name = nameof(Resource.lblDescription), ResourceType = typeof(Resource))]
        [StringLength(512, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Description { get; set; }

        [Display(Name = nameof(Resource.lblFuelCapacity), ResourceType = typeof(Resource))]
        [RequiredIf("Type", (int)AssetType.Tank, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(0, 9999999999, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public Nullable<decimal> FuelCapacity { get; set; }

        [Display(Name = nameof(Resource.lblSubcontractor), ResourceType = typeof(Resource))]
        public Nullable<int> SubContractorId { get; set; }

        [Display(Name = nameof(Resource.lblThresholdDeliveryRequest), ResourceType = typeof(Resource))]
        [RequiredIf("Type", (int)AssetType.Tank, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public Nullable<decimal> ThresholdDeliveryRequest { get; set; }

        [Display(Name = nameof(Resource.lblMinFill), ResourceType = typeof(Resource))]
        [RequiredIf("Type", (int)AssetType.Tank, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [LessThan("MaxFill", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageLessThan), PassOnNull = true)]
        public Nullable<decimal> MinFill { get; set; }

        [Display(Name = nameof(Resource.lblFillType), ResourceType = typeof(Resource))]
        public Nullable<FillType> FillType { get; set; }

        [Display(Name = nameof(Resource.lblMaxFill), ResourceType = typeof(Resource))]
        [RequiredIf("Type", (int)AssetType.Tank, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        //[GreaterThan("MinFill", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        public Nullable<decimal> MaxFill { get; set; }

        //[Display(Name = nameof(Resource.lblMaxFill), ResourceType = typeof(Resource))]
        //[GreaterThan("MinFillPercent", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        //public Nullable<decimal> MaxFillPercent { get; set; }

        //[Display(Name = nameof(Resource.lblMinFill), ResourceType = typeof(Resource))]
        //[LessThan("MaxFillPercent", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        // public Nullable<decimal> MinFillPercent { get; set; }

        [Display(Name = nameof(Resource.lblPhysicalPumpStop), ResourceType = typeof(Resource))]
        public Nullable<decimal> PhysicalPumpStop { get; set; }

        [Display(Name = nameof(Resource.lblRunOutLevel), ResourceType = typeof(Resource))]
        [RequiredIf("Type", (int)AssetType.Tank, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public Nullable<decimal> RunOutLevel { get; set; }

        [Display(Name = nameof(Resource.lblNotificationUponUsageSwing), ResourceType = typeof(Resource))]
        public Nullable<decimal> NotificationUponUsageSwing { get; set; }

        [Display(Name = nameof(Resource.lblNotificationUponUsageSwingValue), ResourceType = typeof(Resource))]
        public Nullable<decimal> NotificationUponUsageSwingValue { get; set; }

        [Display(Name = nameof(Resource.lblNotificationUponInventorySwing), ResourceType = typeof(Resource))]
        public Nullable<decimal> NotificationUponInventorySwing { get; set; }

        [Display(Name = nameof(Resource.lblNotificationUponInventorySwingValue), ResourceType = typeof(Resource))]
        public Nullable<decimal> NotificationUponInventorySwingValue { get; set; }

        [Display(Name = nameof(Resource.lblTankType), ResourceType = typeof(Resource))]
        public Nullable<TankType> TankType { get; set; }

        [Display(Name = nameof(Resource.lblInventoryDataCaptureMethod), ResourceType = typeof(Resource))]
        public Nullable<DipTestMethod> DipTestMethod { get; set; }

        [Display(Name = nameof(Resource.lblManifolded), ResourceType = typeof(Resource))]
        public Nullable<ManiFolded> ManiFolded { get; set; }

        [Display(Name = nameof(Resource.lblTankConstruction), ResourceType = typeof(Resource))]
        public Nullable<TankConstruction> TankConstruction { get; set; }

        public int Type { get; set; }

        [Display(Name = nameof(Resource.lblTankModelType), ResourceType = typeof(Resource))]
        public string TankModelTypeId { get; set; }

        public string TankTypeName { get; set; }

        public string Threshold { get; set; }

        public string DipTestMethodName { get; set; }

        [Display(Name = nameof(Resource.lblTankAcceptsDeliveries), ResourceType = typeof(Resource))]
        public List<int> TankAcceptDelivery { get; set; }

        public string TankChart { get; set; }

        [Display(Name = nameof(Resource.lblTanksConnected), ResourceType = typeof(Resource))]
        public List<int> TanksConnected { get; set; }
        public string TanksConnectedNames { get; set; }

        [Display(Name = nameof(Resource.lblTankSequence), ResourceType = typeof(Resource))] 
        [Range(1, 99999, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public Nullable <int> TankSequence { get; set; }

        [Display(Name = nameof(Resource.lblPedigreeAssetDBID), ResourceType = typeof(Resource))]
        public string PedigreeAssetDBID { get; set; }

        [Display(Name = nameof(Resource.lblSkyBitzRTUID), ResourceType = typeof(Resource))]
        public string SkyBitzRTUID { get; set; }

        [Display(Name = nameof(Resource.lblInSight360ID), ResourceType = typeof(Resource))]
        public string Insight360TankId { get; set; }
        [Display(Name = nameof(Resource.lblWaterLevel), ResourceType = typeof(Resource))]
        public Nullable<decimal> WaterLevel { get; set; } = 0;
        [Display(Name = nameof(Resource.lblVeederRootIpAddress), ResourceType = typeof(Resource))]
        public string VeederRootIPAddress { get; set; }

        [Display(Name = nameof(Resource.lblPort), ResourceType = typeof(Resource))]
        public string Port { get; set; }

        [Display(Name = nameof(Resource.lblVeederRootTankID), ResourceType = typeof(Resource))]
        public string VeederRootTankID { get; set; }

        public string Flag { get; set; }

        public string IMONumber { get; set; }
        [Display(Name = "Stop ATG Polling")]
        public bool IsStopATGPolling { get; set; }
    }
}
