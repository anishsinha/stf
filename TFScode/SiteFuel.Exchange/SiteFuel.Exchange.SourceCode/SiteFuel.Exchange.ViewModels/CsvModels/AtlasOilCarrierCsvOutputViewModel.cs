using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    [DelimitedRecord(",")]
    public class AtlasOilCarrierCsvOutputViewModel
    {
        public AtlasOilCarrierCsvOutputViewModel()
        {
        }

        [FieldOrder(1), FieldQuoted]
        public string InvoiceNumber { get; set; }

        [FieldOrder(2), FieldQuoted]
        public string LoadingStartDate { get; set; }

        [FieldOrder(3), FieldQuoted]
        public string LoadingStartTime { get; set; }

        [FieldOrder(4), FieldQuoted]
        public string LoadingEndDate { get; set; }

        [FieldOrder(5), FieldQuoted]
        public string LoadingEndTime { get; set; }

        [FieldOrder(6), FieldQuoted]
        public string CarrierName { get; set; }

        [FieldOrder(7), FieldQuoted]
        public string SupplierName { get; set; }

        [FieldOrder(8), FieldQuoted]
        public string Supplier_StateCode { get; set; }

        [FieldOrder(9), FieldQuoted]
        public string Product { get; set; }

        [FieldOrder(10)]
        public decimal GrossVolume { get; set; }

        [FieldOrder(11)]
        public decimal NetVolume { get; set; }

        [FieldOrder(12), FieldQuoted]
        public string Source { get; set; }

        [FieldOrder(13), FieldQuoted]
        public string TransactionNumber { get; set; }
    }
}
