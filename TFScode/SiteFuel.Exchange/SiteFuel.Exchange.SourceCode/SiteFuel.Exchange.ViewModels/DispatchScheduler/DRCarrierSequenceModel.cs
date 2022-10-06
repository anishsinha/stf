using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SiteFuel.Exchange.ViewModels.DispatchScheduler
{
    public class DRCarrierSequenceModel
    {
        public string DeliveryRequestId { get; set; }
        public string BlendedGroupId { get; set; }
        public string RegionId { get; set; }
        public int TfxSupplierCompanyId { get; set; }
        public int TfxSupplierOrderId { get; set; }
        public List<TfxCarrierDropdownDisplayItem> CarrierInfo { get; set; } = new List<TfxCarrierDropdownDisplayItem>();
       
    }
    public class DRAvailableCarrierSequenceModel
    {
        public string DeliveryRequestId { get; set; }
        public string RegionId { get; set; }
        public int TfxSupplierCompanyId { get; set; }
        public int TfxSupplierOrderId { get; set; }
        public TfxCarrierDropdownDisplayItem CarrierInfo { get; set; } = new TfxCarrierDropdownDisplayItem();
        public DeliveryRequestViewModel DeliveryRequest = new DeliveryRequestViewModel();
        public int StatusCode { get; set; }
    }
    public class DRCarrierRejectInfoModel
    {
        public string DeliveryRequestId { get; set; }
        public TfxCarrierRejectInfoModel CarrierRejectInfo = new TfxCarrierRejectInfoModel();
    }
    public class TfxCarrierRejectInfoModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime RejectDate { get; set; } = DateTime.Now;
        public TimeSpan RejectTime { get; set; } = new TimeSpan();
        public int RejectedBy { get; set; }
    }
}
