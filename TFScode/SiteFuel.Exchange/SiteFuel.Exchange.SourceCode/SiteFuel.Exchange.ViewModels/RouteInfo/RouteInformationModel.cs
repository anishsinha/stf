using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels.RouteInfo
{
    public class RouteInformationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<TfxJobsDetails> TfxJobs { get; set; } = new List<TfxJobsDetails>();
        public string RegionId { get; set; }
        public int TfxCompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string ShiftInfoDetails { get; set; }
    }
    public class RouteCustomerLocationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<DropdownDisplayItem> TfxJobs { get; set; } = new List<DropdownDisplayItem>();
        public string RegionId { get; set; }
    }
    public class InvoiceRouteInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DeliveryReqId { get; set; }
    }
}
