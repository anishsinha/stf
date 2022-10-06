using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.FreightModels
{
    public class LocationTanksRequestModel
    {
        public List<int> JobIds { get; set; }
        public string RegionIds { get; set; }
        public bool IsRateOfConsumption { get; set; }
        public int CompanyId { get; set; }
    }
    public class LocationTanksResponseModel : StatusModel
    {
        public List<LocationTankDetailsModel> LocationDetails { get; set; }
    }

    public class LocationTankDetailsModel
    {
        public int JobId { get; set; }
        public string SiteId { get; set; }
        public string Status { get; set; } = Resource.valMessageNoDR;
        public double? DaysRemaining { get; set; } = null;
        public List<TankDetailModel> Tanks { get; set; }
        public string LocationName { get; set; }
        public string RegionId { get; set; }
    }

    public class TankDetailModel
    {
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; } = Resource.valMessageNoDR;
        public int TfxProductTypeId { get; set; }
        public double? DaysRemaining { get; set; } = null;
        public int? TankSequence { get; set; }
    }
}
