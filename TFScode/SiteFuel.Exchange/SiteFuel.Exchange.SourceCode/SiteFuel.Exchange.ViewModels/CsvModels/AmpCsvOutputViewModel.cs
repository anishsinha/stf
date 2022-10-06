using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    [DelimitedRecord(",")]
    public class AmpCsvOutputViewModel
    {
        public AmpCsvOutputViewModel()
        {
            ProductUOM = "G";
            CurrencyCode = "USD";
        }
        [FieldQuoted]
        public string VendorId { get; set; }
        [FieldQuoted]
        public string TransationId { get; set; }
        [FieldQuoted]
        public string ShipDate { get; set; }
        [FieldQuoted]
        public string VendorName { get; set; }
        [FieldQuoted]
        public string VendorInvNumber { get; set; }
        [FieldQuoted]
        public string CustomerNumber { get; set; }
        [FieldQuoted]
        public string ShipTo { get; set; }
        [FieldQuoted]
        public string Source { get; set; }
        [FieldQuoted]
        public string ProductCode { get; set; }
        [FieldQuoted]
        public string CustomerName { get; set; }
        [FieldQuoted]
        public string ShipToAddress { get; set; }
        [FieldQuoted]
        public string ShipToCity { get; set; }
        [FieldQuoted]
        public string ShipToState { get; set; }
        [FieldQuoted]
        public string ShipToZip { get; set; }
        [FieldQuoted]
        public string ProductQuantity { get; set; }
        [FieldQuoted]
        public string ProductUOM { get; set; }
        [FieldQuoted]
        public string ProductPrice { get; set; }
        [FieldQuoted]
        public string Freight { get; set; }
        [FieldQuoted]
        public string Additive { get; set; }
        [FieldQuoted]
        public string Taxes { get; set; }
        [FieldQuoted]
        public string OtherFlatFee { get; set; }
        [FieldQuoted]
        public string ProductAmount { get; set; }
        [FieldQuoted]
        public string FreightAmount { get; set; }
        [FieldQuoted]
        public string AdditiveAmount { get; set; }
        [FieldQuoted]
        public string TaxAmount { get; set; }
        [FieldQuoted]
        public string AmountDue { get; set; }
        [FieldQuoted]
        public string CurrencyCode { get; set; }
        [FieldQuoted]
        public string Vehicle { get; set; }
    }
}
