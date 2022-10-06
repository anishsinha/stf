using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.DataAccess.Entities
{
    public class GeneratedExceptionDetail
    {
        public int Id { get; set; }
        public DateTimeOffset DropDate { get; set; }
        public string BuyerCompanyName { get; set; }
        public string SupplierCompanyName { get; set; }
        public string PoNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string JobName { get; set; }
        public decimal OrderedQuantity { get; set; }
        public decimal BolQuantity { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public decimal Tolerance { get; set; }
        public decimal Varience { get; set; }
        public string ScheduledLocation { get; set; }
        public string DroppedLocation { get; set; }
        public decimal PricePerGallon  { get; set; }
        public string UserName { get; set; }
        public string ParameterJson { get; set; }
        public string DriverName { get; set; }

        public string CarrierName { get; set; }
        public int? DriverId { get; set; }
        public string UOM { get; set; }
    }
}
