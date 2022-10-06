
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class AuditDataTableViewModel : DataTableAjaxPostModel
    {
        public int JobId { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int PriceType { get; set; }
    }
}
