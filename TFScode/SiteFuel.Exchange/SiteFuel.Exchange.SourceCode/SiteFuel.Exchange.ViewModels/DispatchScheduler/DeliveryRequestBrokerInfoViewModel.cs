using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.DispatchScheduler
{
    public class DeliveryRequestBrokerInfoViewModel
    {
        public int OrderId { get; set; }
        public int CarrierCompanyId { get; set; }
        public string CarrierRegionId { get; set; }
        public string DispatcherNote { get; set; }
        public DeliveryRequestViewModel DeliveryRequest { get; set; }
        public bool IsDispatchRetainedByCustomer { get; set; }
        public List<TfxCarrierDropdownDisplayItem> CarrierInfo = new List<TfxCarrierDropdownDisplayItem>();
        public string CarrierInfoJson { get; set; }
        public string CarrierCompanyName { get; set; }
        //broker multiple drs to carrier
        public string ScheduleBuilderId { get; set; }
        public List<BrokerDrModel> BrokerDrModel { get; set; }
        public string BlendedGroupId { get; set; }
        public string UniqueOrderNo { get; set; }

    }
  
    public class DeliveryRequestBrokerResponseViewModel : StatusViewModel
    {
        public int OrderId { get; set; }
        public string DeliveryRequestId { get; set; }
    }

    public class BrokerDrModel
    {
        public int OrderId { get; set; }
        public int CarrierCompanyId { get; set; }
        public string CarrierRegionId { get; set; }
        public DeliveryRequestViewModel DeliveryRequest { get; set; }
        public bool IsDispatchRetainedByCustomer { get; set; }
        public string BlendedGroupId { get; set; }
        public string UniqueOrderNo { get; set; }
    }
}
