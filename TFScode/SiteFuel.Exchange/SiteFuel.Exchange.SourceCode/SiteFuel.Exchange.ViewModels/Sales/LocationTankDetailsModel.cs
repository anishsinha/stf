using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SiteFuel.Exchange.ViewModels
{
    public class LocationTanksRequestModel
    {
        public List<int> JobIds { get; set; }
        public string RegionIds { get; set; }
        public bool IsRateOfConsumption { get; set; }
        public int CompanyId { get; set; }
    }
    public class LocationTanksResponseModel : StatusViewModel
    {
        public List<LocationTankDetailsModel> LocationDetails { get; set; }
    }

    public class LocationTankDetailsModel
    {
        public int JobId { get; set; }
        public string SiteId { get; set; }
        public string LocationName { get; set; }
        public string SiteIdDetails { get { return Regex.Replace(SiteId, @"\s+", ""); } set { Regex.Replace(SiteId, @"\s+", ""); } }
        public string Status { get; set; } = Resource.valMessageNoDR;
        public double? DaysRemaining { get; set; }
        public string CustomerInfo { get; set; } = string.Empty;
        public List<TankDetailModel> Tanks { get; set; }
        public string RegionId { get; set; }
        public int LocationManagedType { get; set; }
    }

    public class TankDetailModel
    {
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; } = Resource.valMessageNoDR;
        public double? DaysRemaining { get; set; }
        public int? TankSequence { get; set; }
    }
}
