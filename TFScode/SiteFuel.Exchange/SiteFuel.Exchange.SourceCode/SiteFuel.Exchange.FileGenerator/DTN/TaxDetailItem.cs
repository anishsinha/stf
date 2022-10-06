using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.FileGenerator.DTN
{
    public class TaxDetailItem
    {
        private string RateFormat { get; set; }
        public TaxDetailItem()
        {
            RateFormat = DtnConstants.NumberFormat4;
        }

        public TaxDetailItem(string rateFormat)
        {
            RateFormat = rateFormat;
        }

        public string RecordType { get { return "TXDL"; } }
        public decimal Rate { get; set; }
        public decimal LineTotal { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal QuantityBilled { get; set; }
        /// <summary>
        /// BR – Barrel
        /// CA – Case
        /// DO – Dollars – US
        /// DR – Drums
        /// EA – Each
        /// GA – Gallons
        /// GD – Gross Barrel
        /// GN – Gross Gallons
        /// LB – Pound
        /// LT – Liter
        /// ND – Net Barrels
        /// NG – Net Gallons
        /// NL – Load
        /// TN – Net Ton
        /// </summary>
        public string UoM { get; set; } = "GA";
        /// <summary>
        /// "Y" = Yes
        /// "N" = No
        /// </summary>
        public string Deferred { get; set; } = "N";
        /// <summary>
        /// "N" = Net
        /// "G" = Gross
        /// </summary>
        public string BilledQuantityIndicator { get; set; } = "N";
        public override string ToString()
        {
            var values = new StringBuilder();
            values.Append($"\"{RecordType}\",{Rate.ToString(RateFormat)},{LineTotal.ToString(DtnConstants.NumberFormat2)},");
            values.Append($"\"{Description}\",{QuantityBilled.ToString(DtnConstants.NumberFormat0)},\"{UoM}\",");
            values.Append($"\"{Deferred}\",\"{BilledQuantityIndicator}\"");
            return values.ToString();
        }
    }
}
