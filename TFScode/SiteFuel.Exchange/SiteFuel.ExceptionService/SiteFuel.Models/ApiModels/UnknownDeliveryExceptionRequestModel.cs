using System;

namespace SiteFuel.Models.ApiModels
{
    public class UnknownDeliveryExceptionRequestModel : InvoiceExceptionModel
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public int JobId { get; set; }

        public string JobName { get; set; }

        public string ProductName { get; set; }

        public string JobAddress { get; set; }

        public string TankName { get; set; }

        public string SiteId { get; set; }

        public string TankId { get; set; }

        public string StorageId { get; set; }

        public decimal Ullage { get; set; }

        public decimal PrevUllage { get; set; }

        public DateTimeOffset CaptureTime { get; set; }

        public int SourceFileId { get; set; }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }
    }
}
