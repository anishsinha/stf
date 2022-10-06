using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.FilldService
{
    public class FilldFillRequestViewModel
    {
        public long waypoint_id { get; set; }
        public long user_id { get; set; }
        public long territory_id { get; set; }
        public List<FilldAssetViewModel> assets { get; set; }
    }
    public class FilldAssetViewModel
    {
        public string asset_id { get; set; }
        public string asset_name { get; set; }
        public decimal? capacity { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public int? year { get; set; }
        public string license { get; set; }
        public string note { get; set; }
        public long fuel_type_id { get; set; }
    }

}
