using SiteFuel.Exchange.Utilities;
using System;


namespace SiteFuel.Exchange.ViewModels
{
    public class ManualInvoiceCreateRequestViewModel : InvoiceCreateViewModel
    {
        public ManualInvoiceCreateRequestViewModel()
        {
        }

        public ManualInvoiceCreateRequestViewModel(Status status) : base(status)
        {
        }

        public int? DriverId { get; set; }
        public DateTimeOffset DeliveryStartDate { get; set; }
        public TimeSpan DeliveryEndTime { get; set; }
        public int ApprovalUserOnboardedType { get; set; }
        public bool IsBrokeredOrder { get; set; }
        public string JobCompanyName { get; set; }
        public string ApprovalUserName { get; set; }

        public ManualInvoiceCreateRequestViewModel Clone()
        {
            return (ManualInvoiceCreateRequestViewModel)this.MemberwiseClone();
        }
    }
}
