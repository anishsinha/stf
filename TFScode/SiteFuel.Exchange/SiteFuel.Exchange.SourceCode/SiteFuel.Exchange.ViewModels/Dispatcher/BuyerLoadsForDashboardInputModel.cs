using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Dispatcher
{
    public class BuyerLoadsForDashboardInputModel
    {
        public Country CountryId { get; set; } = Country.USA;
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
    }
}
