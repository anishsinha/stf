using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Offer
{
    public class OfferStatsViewModel
    {
        public int OfferId { get; set; }
        public string CustomeName { get; set; }
        public string Action { get; set; }
        public DateTimeOffset ActionDateTime { get; set; }
        public string Reason { get; set; }
    }
}
