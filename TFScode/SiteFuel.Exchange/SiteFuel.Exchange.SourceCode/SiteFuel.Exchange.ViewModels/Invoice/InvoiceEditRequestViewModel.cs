using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceEditRequestViewModel: StatusViewModel
    {
        public int JobId { get; set; }
        public int JobCompanyId { get; set; }
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public int InvoiceId { get; set; }
        public int DiscountId { get; set; }
        public int? PreviousTrackableScheduleId { get; set; }
        public int PreviousTrackableScheduleStatusId { get; set; }
        public int? CurrentTrackableScheduleId { get; set; }
        public int CurrentTrackableScheduleStatusId { get; set; }
        public bool IsTaxManuallyModified { get; set; }
        public bool IsTaxServiceSucceeded { get; set; }
        public int DeliveryTypeId { get; set; }
        public decimal OrderMaxQuantity { get; set; }
        public InvoiceModel InvoiceModel { get; set; }
        public string TimeZoneName { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public bool IsInvoiceImagesAvailable { get; set; }
        public int InvoiceHeaderVersion { get; set; }
        public decimal  OriginalDroppedGallons { get; set; }
        public string DeliveryLevelPO { get; set; }
    }

    public class EditInvoiceProcessorModel
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string FileUploadPath { get; set; }
    }
}
