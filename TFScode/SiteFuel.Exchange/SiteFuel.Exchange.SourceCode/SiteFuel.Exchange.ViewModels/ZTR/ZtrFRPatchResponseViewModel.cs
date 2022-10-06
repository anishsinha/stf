using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class ZtrFRPatchResponseViewModel
    {
        public string Href { get; set; }
        public string VendorRequestId { get; set; }
        public string Vendor { get; set; }
        public string OpenedAt { get; set; }
        public List<AssetRequestData> RequestData{ get; set; }
        public List<AssetFuelRequestPatchViewModel> ResponseData { get; set; }
    }

    public class AssetRequestData
    {
        public string EquipmentId { get; set; }
        public string FuelRequired { get; set; }
        public string FuelLevel { get; set; }
    }
}
