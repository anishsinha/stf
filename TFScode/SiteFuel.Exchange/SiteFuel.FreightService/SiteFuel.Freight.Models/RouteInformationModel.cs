using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class RouteInformationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<TfxJobsDetailsModel> TfxJobs { get; set; } = new List<TfxJobsDetailsModel>();
        public string RegionId { get; set; }
        public int TfxCompanyId { get; set; }
        public int CreatedBy { get; set; }
        public ShiftInfoViewModel ShiftInfo { get; set; } = null;
        public string ShiftInfoDetails { get; set; }
    }
    public class RouteCustomerLocationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<TfxJobsDetailsModel> TfxJobs { get; set; } = new List<TfxJobsDetailsModel>();
        public string RegionId { get; set; }
    }
    public class ShiftInfoViewModel
    {
        public string Id { get; set; }
        public string TripId { get; set; }
        public int DriverRowIndex { get; set; }
        public int DriverColIndex { get; set; }
        public int ShiftIndex { get; set; }
    }
    public class InvoiceRouteInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DeliveryReqId { get; set; }
    }
}
