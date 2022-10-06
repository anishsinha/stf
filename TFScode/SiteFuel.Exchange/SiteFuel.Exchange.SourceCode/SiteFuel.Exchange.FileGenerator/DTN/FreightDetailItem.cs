using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.FileGenerator.DTN
{
    public class FreightDetailItem
    {
        public string RecordType { get { return "ITMF"; } }
        public decimal Rate { get; set; }
        public decimal LineTotal { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal QuantityBilled { get; set; }
        public override string ToString()
        {
            var values = new StringBuilder();
            values.Append($"\"{RecordType}\",{Rate.ToString(DtnConstants.NumberFormat4)},{LineTotal.ToString(DtnConstants.NumberFormat2)},");
            values.Append($"\"{Description}\",{QuantityBilled.ToString(DtnConstants.NumberFormat0)}");
            return values.ToString();
        }
    }
}
