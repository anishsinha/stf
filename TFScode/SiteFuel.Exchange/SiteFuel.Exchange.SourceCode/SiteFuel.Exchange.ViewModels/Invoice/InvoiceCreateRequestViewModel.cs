using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceCreateRequestViewModel: StatusViewModel
    {
        public int JobId { get; set; }
        public int JobCompanyId { get; set; }
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public int? CurrentTrackableScheduleId { get; set; }
        public int CurrentTrackableScheduleStatusId { get; set; }
        public bool IsTaxServiceSucceeded { get; set; }
        public int DeliveryTypeId { get; set; }
        public decimal OrderMaxQuantity { get; set; }
        public int OrderAcceptedBy { get; set; }
        public InvoiceModel InvoiceModel { get; set; }
        public string TimeZoneName { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public string FCMAppId { get; set; }
        public bool IsSplitLoad { get; set; }
        public CreationMethod CreationMethod { get; set; } = CreationMethod.SFX;
    }
}
