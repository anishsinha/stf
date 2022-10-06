using System.Collections.Generic;

namespace SiteFuel.FreightModels
{
    public class JobSummaryModel
    {
        public int JobId { get; set; }
        public string RegionId { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public string CarrierId { get; set; } = string.Empty;
        public string CarrierName { get; set; } = string.Empty;
        public string RouteId { get; set; } = string.Empty;
        public string RouteName { get; set; } = string.Empty;
        public string DistanceCovered { get; set; } = string.Empty;
    }

    public class JobAdditionalDetailsForSummary : StatusModel
    {
        public List<JobSummaryModel> JobDetails { get; set; } = new List<JobSummaryModel>();
    }

    public class JobSummaryRequestModel
    {
        public int CompanyId { get; set; }
        public List<int> JobIds { get; set; } = new List<int>();
    }
}
