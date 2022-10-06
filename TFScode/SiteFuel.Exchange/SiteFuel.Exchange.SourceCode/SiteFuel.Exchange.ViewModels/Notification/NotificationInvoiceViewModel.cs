using System;
using System.Collections.Generic;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Notification;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationInvoiceViewModel : BaseNotificationViewModel
    {
        public NotificationInvoiceViewModel()
        {
            BuyerUser = new NotificationUserViewModel();
            SupplierUser = new NotificationUserViewModel();
            ResaleCustomer = new List<FuelRequestResaleCustomerViewModel>();
            Attachments = new List<System.Net.Mail.Attachment>();
            DropAdditionalDetails = new List<NotificationDropAdditionalViewModel>();
        }

        public int Id { get; set; }
        public List<NotificationDropAdditionalViewModel> DropAdditionalDetails { get; set; }

        public NotificationUserViewModel BuyerUser { get; set; }
        public NotificationUserViewModel SupplierUser { get; set; }
        public NotificationUserViewModel InvoiceApprover { get; set; }
        public int SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
        public bool IsUpdatedByBuyer { get; set; }
        public DateTime DueDate { get; set; }
        public DateTimeOffset ApprovedOn { get; set; }
        public int InvoiceType { get; set; }
        public string JobName { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string InvoiceNumber { get; set; }
        public bool IsResaleEnabled { get; set; }
        public List<FuelRequestResaleCustomerViewModel> ResaleCustomer { get; set; }

        public string UpdatedDate { get; set; }
        public string UpdatedTime { get; set; }

        public string DriverName { get; set; }
        public string UpdatedByUserName { get; set; }

        public string DdtNumberOfInvoice { get; set; }

        public List<System.Net.Mail.Attachment> Attachments { get; set; }        

        public List<NotificationUserViewModel> UsersAssignedToJob { get; set; }

        public List<NotificationUserViewModel> OnsitePersons { get; set; }

        public List<NotificationUserViewModel> SupplierAccountingUsers { get; set; }
        public bool IsProFormaPo { get; set; }
        public bool IsBrokeredInvoice { get; set; }
        public bool IsPartOfStatement { get; set; }
        public bool ReplaceInvoiceWithDdt { get; set; }
        public bool DeliveryInstructionsExists { get; set; }
        public int? InvoiceNotificationPreferenceId { get; set; }
        public bool SendAttachmentToBuyer { get; set; }
        public bool SendAttachmentToSupplier { get; set; }
        public bool IsInvoice { get; set; }
        public string DayOfWeek { get; set; }
        public string DropStartTime { get; set; }
        public string DropEndTime { get; set; }
        public string DropDate { get; set; }
        public bool IsDigitalDropTicket()
        {
            return (InvoiceType == (int)Utilities.InvoiceType.DigitalDropTicketManual || InvoiceType == (int)Utilities.InvoiceType.DigitalDropTicketMobileApp);
        }
        public bool IsBillingFileRequired()
        {
            return InvoiceNotificationPreferenceId != (int)InvoiceNotificationPreferenceTypes.SendInvoiceDDTWithoutBillingFile && InvoiceNotificationPreferenceId != (int)InvoiceNotificationPreferenceTypes.SendPDIDeliveryDetails;
        }
    }

    public class NotificationDropAdditionalViewModel
    {
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public int FuelRequestTypeId { get; set; }
        public string FuelType { get; set; }
        public bool IsTpoOrder { get; set; }

        public UoM UoM { get; set; }
        public decimal DropQuantity { get; set; }
        public string ConvertedQuantity { get; set; }


        public bool IsExceedingQuantity { get; set; }
    }
}
