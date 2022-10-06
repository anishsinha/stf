using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.InvoiceExceptions;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobTankDetailsViewModel : DeliveredQuantityVarianceModel
    {
        public int CustomerId { get; set; }

        public int JobId { get; set; }

        public string ProductName { get; set; }

        public string JobAddress { get; set; }

        public string TankName { get; set; }

        public decimal? TankCapacity { get; set; }

        public decimal DroppedGallons { get; set; }

        public string SiteId { get; set; }

        public string TankId { get; set; }

        public string StorageId { get; set; }

        public string OrderAndInvoiceIds { get; set; }

        public string Ullage { get; set; }

        public decimal PrevUllage { get; set; }

        public DateTimeOffset CaptureTime { get; set; }

        public long? SourceFileId { get; set; }

        public string ReasonOfFailure { get; set; }

        public UoM QuantityUoM { get; set; }

        public int RowNum { get; set; }
    }
}
