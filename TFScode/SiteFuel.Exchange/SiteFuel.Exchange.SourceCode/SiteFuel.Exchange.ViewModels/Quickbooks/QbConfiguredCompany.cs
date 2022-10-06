using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Quickbooks
{
    public class QbCompanyViewModel
    {
        public int Id { get; set; }

        public string SyncReportTime { get; set; }

        public string ReportTimeZone { get; set; }
    }
}
