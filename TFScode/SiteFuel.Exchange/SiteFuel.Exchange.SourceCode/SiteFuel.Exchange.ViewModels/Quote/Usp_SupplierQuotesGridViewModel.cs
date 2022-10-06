using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspSupplierQuotesGridViewModel
    {
        public int Id { get; set; }

        public string QuoteNumber { get; set; }

        public string JobName { get; set; }

        public string Address { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string FuelType { get; set; }

        public decimal GallonsRequested { get; set; }

        public DateTimeOffset QuoteDueDate { get; set; }

        public string QuotesReceived { get; set; }

        public string Status { get; set; }

        public int StatusId { get; set; }

        public string QuotationStatusName { get; set; }

        public bool IsQuotationCreated { get; set; }
    }
}
