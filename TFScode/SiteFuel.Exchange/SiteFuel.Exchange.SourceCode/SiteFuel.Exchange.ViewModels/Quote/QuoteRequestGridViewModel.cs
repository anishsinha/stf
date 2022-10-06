using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class QuoteRequestGridViewModel
    {
        public int Id { get; set; }

        public string QuoteNumber { get; set; }

        public int JobId { get; set; }

        public string JobName { get; set; }

        public string Address { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string FuelType { get; set; }

        public string GallonsRequested { get; set; }

        public string QuoteDueDate { get; set; }

        public string QuotesReceived { get; set; }

        public string QuotesNeeded { get; set; }

        public string Status { get; set; }

        public string QuotationStatusName { get; set; }

        public int StatusId { get; set; }

        public bool IsQuotationCreated { get; set; }
    }
}
