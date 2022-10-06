using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    [DelimitedRecord(",")]
    public class InvoiceReportCsvViewModel
    {
        public InvoiceReportCsvViewModel()
        {
        }

        [FieldOrder(1), FieldQuoted]
        public string InvoiceAmount { get; set; }

        [FieldOrder(2), FieldQuoted]
        public string FuelAmount { get; set; }

        [FieldOrder(3), FieldQuoted]
        public string StateSalesTax { get; set; }

        [FieldOrder(4), FieldQuoted]
        public string StateTax { get; set; }

        [FieldOrder(5), FieldQuoted]
        public string FederalTax { get; set; }

        [FieldOrder(6), FieldQuoted]
        public string DeliveryAmount { get; set; }

        [FieldOrder(7), FieldQuoted]
        public string InvoiceNumber { get; set; }

        [FieldOrder(8), FieldQuoted]
        public string InvoiceDate { get; set; }

        [FieldOrder(9), FieldQuoted]
        public string JobName { get; set; }

        [FieldOrder(10), FieldQuoted]
        public string Description { get; set; }
    }
}
