using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Invoice.Pdf
{
    public class UspInvoicePdfTaxDetail
    {
        public int Id { get; set; }
        public string RateDescription { get; set; }
        public decimal TradingTaxAmount { get; set; }
        public bool IsModified { get; set; }
        public int TaxPricingTypeId { get; set; }
    }
}
