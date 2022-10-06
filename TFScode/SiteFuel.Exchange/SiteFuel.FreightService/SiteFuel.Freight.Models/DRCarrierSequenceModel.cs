using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class DRCarrierSequenceModel
    {
        public string Id { get; set; }
        public string DeliveryRequestId { get; set; }
        public string RegionId { get; set; }
        public int TfxSupplierCompanyId { get; set; }
        public int TfxSupplierOrderId { get; set; }
        public List<TfxCarrierDropdownDisplayViewModelItem> CarrierInfo { get; set; } = new List<TfxCarrierDropdownDisplayViewModelItem>();
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
    public class TfxDRAvailableCarrierInfoModel
    {
        public string DeliveryRequestId { get; set; }
        public string RegionId { get; set; }
        public int TfxSupplierCompanyId { get; set; }
        public int TfxSupplierOrderId { get; set; }
        public TfxCarrierDropdownDisplayViewModelItem CarrierInfo { get; set; } = new TfxCarrierDropdownDisplayViewModelItem();
        public DeliveryRequestViewModel DeliveryRequest = new DeliveryRequestViewModel();
        public int StatusCode { get; set; } = (int)Status.Success;
    }
}
