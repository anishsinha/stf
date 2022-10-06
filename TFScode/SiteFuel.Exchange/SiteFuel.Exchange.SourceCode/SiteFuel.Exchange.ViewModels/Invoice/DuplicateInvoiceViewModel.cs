using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DuplicateInvoiceViewModel
    {
        public int OrderId { get; set; }
        public int InvoiceId { get; set; }
        public int InvoiceNumberId { get; set; }
        public string PoNumber { get; set; }
        public string DisplayInvoiceNumber { get; set; }
        public decimal DropQuantity { get; set; }
        public decimal PricePerGallon { get; set; }
        public DateTimeOffset DropDate { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
    }
}
