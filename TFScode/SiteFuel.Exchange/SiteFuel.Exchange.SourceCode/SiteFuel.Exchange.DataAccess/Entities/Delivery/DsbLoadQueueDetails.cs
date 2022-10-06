using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class DsbLoadQueueDetails
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DsbLoadQueueDetails()
        {
        }
        public int Id { get; set; }
        public string ScheduleBuilderId { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string RegionId { get; set; } = string.Empty;
        public string ShiftId { get; set; } = string.Empty;
        public int ShiftIndex { get; set; }
        public int DriverColIndex { get; set; }
        public string TrailerInfo { get; set; } = string.Empty;
        public int TfxDriverId { get; set; }
        public string DeliveryRequestInfo { get; set; } = string.Empty;
        public string DriverColJsonInfo { get; set; } = string.Empty;
        public int TfxUserId { get; set; }
        public int TfxCompanyId { get; set; }
        public string UserLanguage { get; set; }
        public int ProcessStatus { get; set; } = (int)DsbLoadQueueStatus.New;
        public int NotifyStatus { get; set; } = (int)DsbLoadQueueNotifyStatus.New;
        public string DriverColJsonResponse { get; set; } = string.Empty;
        public int FailedCount { get; set; } = 0;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
    }
}
