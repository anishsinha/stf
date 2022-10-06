using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceBulkUploadProcessorReqViewModel
    {
        public int SupplierId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int FileLineNumberToStart { get; set; }
        public string FileUploadPath { get; set; }
    }

    public class InvoiceImageProcessorReqViewModel : InvoiceBulkUploadProcessorReqViewModel
    {
        public int InvoiceId { get; set; }
        public InvoiceImageType ImageType { get; set; } = InvoiceImageType.Drop;
    }
         
    public class InvoiceBulkUploadErrorProcessorViewModel : InvoiceBulkUploadProcessorReqViewModel
    {
        public string Errors { get; set; }
    }

}