using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardQuoteRequestViewModel
    {       
        public int TotalQuoteRequestCount { get; set; }

        public int OpenQuoteRequestCount { get; set; }

        public int CompletedQuoteRequestCount { get; set; }

        public int ExpiredQuoteRequestCount { get; set; }

        public int CancelledQuoteRequestCount { get; set; }

        public List<QuoteRequestGridViewModel> RecentQuoteRequests { get; set; }

        public int SelectedJobId { get; set; }        

        public bool IsFRQuoteTileCollapsed { get; set; }
    }
}
