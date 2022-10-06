using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverAdditionalDetailModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string License { get; set; }

        public string ContactNumnber { get; set; }

        public List<string> Shifts { get; set; }

        public List<TruckDetailViewModel> Trailers { get; set; } = new List<TruckDetailViewModel>();
    }
}
