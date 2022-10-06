using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class RequestStatusViewModel
    {
        public int RequestId { get; set; }

        public string RequestNumber { get; set; }

        public string Status { get; set; }

        public string Details { get; set; }

        public int UploadedBy { get; set; }

        public DateTimeOffset TimeRequested { get; set; }

        public string QueueProcessoryType { get; set; }

        public string UploadedDateTime { get; set; }
    }
}
