using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Quickbooks
{
    public class QbSyncReportViewModel
    {
        public string ReportDate { get; set; }

        public string SyncStartDate { get; set; }

        public string SyncEndDate { get; set; }

        public string TimeZone { get; set; }

        public string ReportImageUrl { get; set; }

        public bool ShowLoginNote { get; set; } = true;

        public QbSyncReport Report { get; set; }
    }
}
