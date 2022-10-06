using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.DispatchScheduler
{
    public class ScheduleAdditionalInfo
    {
        public int FsPriority { get; set; }
        public string FsRegionId { get; set; }
        public string FsTrailerDisplayId { get; set; }
        public string LoadNumber { get; set; }
        public string FsAssignedRegionId { get; set; }
        public string ScheduleBuilderId { get; set; } = string.Empty;
        public string ShiftId { get; set; } = string.Empty;
        public int ShiftIndex { get; set; } = 0;
        public int DriverColIndex { get; set; } = 0;
        public string UniqueOrderNo { get; set; }
        public string CreditApprovalFilePath { get; set; }
        public string Sap_OrderNo { get; set; }
        public string DR_Price { get; set; }
        public string TripId { get; set; }
    }
}
