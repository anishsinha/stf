using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.FilldService
{
    public class FilldAssetDropViewModel
    {
        public long id { get; set; }
        public int? asset_id { get; set; }
        public string asset_name { get; set; }
        public decimal final_units { get; set; }
        public DateTimeOffset? created_at { get; set; }
        public DateTimeOffset? start_time { get; set; }
        public DateTimeOffset? end_time { get; set; }
        public int fuel_type_id { get; set; }
        public string status { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string year { get; set; }
    }

    public class FilldDropResponseModel : FilldStatusViewModel
    {
        public List<FilldAssetDropViewModel> Data { get; set; } = new List<FilldAssetDropViewModel>();
    }

    public class FilldMobileDropModel
    {
        public int JobXAssetId { get; set; }
        public int TfxFueltypeId { get; set; }
        public string Fueltype { get; set; }
        public decimal DroppedQuantity { get; set; }
    }
    public class FilldMobileDropRespoonseModel : StatusViewModel
    {
        public List<FilldMobileDropModel> Data { get; set; } = new List<FilldMobileDropModel>();
    }
}
