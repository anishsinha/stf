using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class HistoricalPriceViewModel
    {
        public string IndexPeriod { get; set; }

        public string IndexProduct { get; set; }

        public string IndexArea { get; set; }

        public string IndexTypeName { get; set; }

        public string PeriodName { get; set; }

        public int IndexType { get; set; }

        public string ManualIndexPrice { get; set; }

        public string ManualIndexPriceDate { get; set; }

        public List<HistoricalPriceDetailsViewModel> HistoricalPriceDetails { get; set; }
    }
}
