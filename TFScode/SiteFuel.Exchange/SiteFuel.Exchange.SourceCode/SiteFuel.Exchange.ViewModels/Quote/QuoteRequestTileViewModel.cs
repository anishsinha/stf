using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class QuoteRequestTileViewModel
    {
        public QuoteRequestTileViewModel()
        {
            Quotations = new List<QuotationsGridViewModel>();
        }
        public List<QuotationsGridViewModel> Quotations { get; set; }

        public int QuoteRequestId { get; set; }
    }
}
