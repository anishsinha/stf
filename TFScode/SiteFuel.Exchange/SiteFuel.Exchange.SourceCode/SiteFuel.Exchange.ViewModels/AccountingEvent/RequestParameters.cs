using System;

namespace SiteFuel.Exchange.ViewModels.AccountingEvent
{
    public class RequestParameters
    {
        public int CompanyId { get; set; }

        public int CustomerCompanyId { get; set; }

        public int CustomerId { get; set; }

        public int JobId { get; set; }

        public int FuelRequestId { get; set; }

        public int OrderId { get; set; }

        public int InvoiceId { get; set; }

        public int InvoiceNumberId { get; set; }

        public string InvoiceBrokeredChainId { get; set; }

        public string ClassName { get; set; }

        public int? ParentOrderId { get; set; }

        public bool IsEndSupplier { get; set; }

		public DateTimeOffset? InvoiceCreationDate { get; set; }

        public DateTimeOffset? FromModifiedDate { get; set; }

        public DateTimeOffset? ToModifiedDate { get; set; }
    }
}
