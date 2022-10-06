using Newtonsoft.Json;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetResponseModel : StatusViewModel
    {
    }

    public class AssetModel : AssetTankModel
    {
        public int AssetId { get; set; }
        public string AssetName { get; set; }
        public int? AssetCompanyId { get; set; }
        public string AssetCompany { get; set; }
        public bool IsMarine { get; set; }
    }

    public class TankModel : AssetTankModel
    {
        [JsonIgnore]
        public int AssetId { get; set; }
        [JsonProperty(PropertyName = "TankName")]
        public string AssetName { get; set; }
        [JsonIgnore]
        public TankType? TankTypeId { get; set; }
        public string TankType { get; set; }
        public string TankChart { get; set; }
        public string TanksConnectedNames { get; set; }
        public int? TankSequence { get; set; }
        public string Manufacturer { get; set; }
        public string IsManifold { get; set; }
        public string ConstructionType { get; set; }
        public decimal? MinFill { get; set; }
        public decimal? MaxFill { get; set; }
        [JsonProperty(PropertyName = "TankCompanyId")]
        public int? AssetCompanyId { get; set; }
        [JsonProperty(PropertyName = "TankCompany")]
        public string AssetCompany { get; set; }
    }

    public class AssetTankModel
    {        
        public int? ProductTypeId { get; set; }
        public string ProductType { get; set; }
        public string Class { get; set; }
        public decimal? FuelCapacity { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string LicensePlate { get; set; }
        public int? LicensePlateStateId { get; set; }
        public string LicensePlateStateCode { get; set; }
        public string LicensePlateState { get; set; }
        public int? CurrentAssignedLocationId { get; set; }
        public string CurrentAssignedLocation { get; set; }
        public DateTimeOffset? LocationAssignedDate { get; set; }
        public int? LocationAssignedByUserId { get; set; }
        public string LocationAssignedByUser { get; set; }
        public string Description { get; set; }
        public string TelematicsProvider { get; set; }
        [JsonIgnore]
        public int? DipTestMethod { get; set; }
        public string DipTestMethodType { get; set; }
        [JsonProperty(PropertyName = "SiteID")]
        public string DisplayJobId { get; set; }
        public string Color { get; set; }
        [JsonProperty(PropertyName = "StorageID")]
        public string Vendor { get; set; }
        [JsonProperty(PropertyName = "TankID")]
        public string VehicleId { get; set; }        
        public string PedigreeAssetDBId { get; set; }       
        public string Threshold { get; set; }
        [JsonIgnore]
        public int? ImageId { get; set; }
        public string CreatedByUser { get; set; }
        public int? SubcontractorId { get; set; }
        public string Subcontractor { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        [JsonIgnore]
        public int? RemovedBy { get; set; }
        public string RemovedByUser { get; set; }
        public DateTimeOffset? RemovedDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class VesselModel
    {
        public int AssetId { get; set; }
        public string AssetName { get; set; }
        public string AssetCompany { get; set; }
        public bool IsMarine { get; set; }
        public string IMONumber { get; set; }
        public string Flag { get; set; }
    }
}
