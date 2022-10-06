using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspDuplicateInvoice
    {
        public int OrderId { get; set; }
        public int InvoiceId { get; set; }
        public int InvoiceNumberId { get; set; }
        public string PoNumber { get; set; }
        public string DisplayInvoiceNumber { get; set; }
        public decimal PricePerGallon { get; set; }
        public decimal DroppedGallons { get; set; }
        public DateTimeOffset DropDate { get; set; }
        public string CreationMethod { get; set; }
        public int CreatedBy { get; set; }
        public string UserName { get; set; }
    }
}
