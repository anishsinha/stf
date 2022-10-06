using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.MFN
{
    public class UspMfnGetDraftDDT
    {
        public int TrackableScheduleId { get; set; }
        public string GroupParentDRId { get; set; }
        public int OrderId { get; set; }
        public int? InvoiceId { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string TimeZoneName { get; set; }
        public int DeliveryScheduleStatusId { get; set; }

    }
}
