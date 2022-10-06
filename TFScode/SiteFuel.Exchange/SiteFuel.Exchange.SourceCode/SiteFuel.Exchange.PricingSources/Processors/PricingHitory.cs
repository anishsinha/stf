using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.PricingSources.Processors
{
    public class PricingHitory
    {
        public string Symbol { get; set; }
        public DateTimeOffset? LoadDate { get; set; }
        public string Feed { get; set; }
        public DateTimeOffset? ReportedDate { get; set; }
        public decimal? Price { get; set; }
        public string Unit { get; set; }
        public string Currency { get; set; }
        public int? SupplierNumber { get; set; }
        public string Supplier { get; set; }
        public string SupplierBrand { get; set; }
        public string PriceType { get; set; }
        public string LiftPoint { get; set; }
        public int? Product_ID { get; set; }
        public string ProductGroup { get; set; }
        public string ProductDescription { get; set; }
        public string Source { get; set; }
        public string StateCode { get; set; }
		public int FileId { get; internal set; }
	}
}
