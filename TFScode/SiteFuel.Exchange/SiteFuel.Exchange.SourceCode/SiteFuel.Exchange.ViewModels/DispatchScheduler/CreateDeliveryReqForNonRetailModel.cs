using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class CreateDeliveryReqForNonRetailModel
    {
        public List<DropdownDisplayItem> OrderList { get; set; } = new List<DropdownDisplayItem>();
        public List<DemandModel> DeliveryReqInput { get; set; } = new List<DemandModel>();
    }
    public class DefaultTBDScheduleData
    {
        public int UoM { get; set; }
        public List<TBDDropdownDisplayItem> MstProductTypes { get; set; } = new List<TBDDropdownDisplayItem>();
        public List<TBDDropdownDisplayItem> OtherProducts { get; set; } = new List<TBDDropdownDisplayItem>();
    }
}
