using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.InvoiceExceptions;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class MissedDeliveryExceptionOtherDetails
    {
        public int OrderId { get; set; }

        public string PoNumber { get; set; }

        public int InvoiceId { get; set; }

        public int TfxProductId { get; set; }

        public string DisplayInvoiceNumber { get; set; }

        public string ProductName { get; set; }

        public string JobAddress { get; set; }

        public List<MissedDeliveryTankDetails> TankDetails { get; set; }
    }

    public class MissedDeliveryTankDetails
    {
        public string TankId { get; set; }

        public string StorageId { get; set; }

        public string TankName { get; set; }

        public decimal? TankCapacity { get; set; }

        public string SiteId { get; set; }

        public decimal Ullage { get; set; }

        public decimal PrevUllage { get; set; }

        public DateTimeOffset CaptureTime { get; set; }
    }
}
