using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.FileGenerator.DTN
{
    public class BinaryData
    {
        public InvoiceHeader InvoiceHeader { get; set; } = new InvoiceHeader();
        public List<DetailItem> DetailItems { get; set; } = new List<DetailItem>();
        public List<TaxDetailItem> TaxDetailItems { get; set; } = new List<TaxDetailItem>();
        public List<FreightDetailItem> FreightDetailItems { get; set; } = new List<FreightDetailItem>();
        public override string ToString()
        {
            var values = new StringBuilder();
            values.Append(DtnConstants.BeginBinaryData + DtnConstants.CRLF);
            values.Append(InvoiceHeader.ToString() + DtnConstants.CRLF);
            foreach (var item in DetailItems)
            {
                values.Append(item.ToString() + DtnConstants.CRLF);
            }
            foreach (var item in TaxDetailItems)
            {
                values.Append(item.ToString() + DtnConstants.CRLF);
            }
            foreach (var item in FreightDetailItems)
            {
                values.Append(item.ToString() + DtnConstants.CRLF);
            }
            values.Append(DtnConstants.EndBinaryData);
            return values.ToString();
        }
    }
}
