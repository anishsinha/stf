using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspStatementInvoiceDetails
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }
        public DateTimeOffset DropDate { get; set; }
        public string PoNumber { get; set; }
        public decimal DroppedGallons { get; set; }
        public decimal TotalAmount { get; set; }
        public string FuelType { get; set; }
    }
}
