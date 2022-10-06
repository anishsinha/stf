using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.InvoiceException
{
    public class MissedDeliveryExceptionModel
    {
        public int Id { get; set; }
        public string PoNumber { get; set; }
        public string JobName { get; set; }
        public string DropDate { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public int StatusId { get; set; }   
        public string StatusName { get; set; }
        public string CarrierName { get; set; }
    }
}
