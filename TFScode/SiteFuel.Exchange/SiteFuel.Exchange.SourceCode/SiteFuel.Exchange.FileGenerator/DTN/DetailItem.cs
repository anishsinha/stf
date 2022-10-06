using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.FileGenerator.DTN
{
    public class DetailItem
    {
        public string RecordType { get { return "ITMD"; } }
        public string BolNumber { get; set; } = string.Empty;
        public decimal NetQuantity { get; set; }
        public decimal GrossQuantity { get; set; }
        public decimal Rate { get; set; }
        public decimal LineTotal { get; set; }
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Only used with custom map when applicable
        /// </summary>
        public string ProductCode { get; set; } = string.Empty;
        /// <summary>
        /// "N" = Net
        /// "G" = Gross
        /// </summary>
        public string BilledQuantityIndicator { get; set; } = "N";
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
        /// Start Load Date not End Date
        /// </summary>
        public DateTime ShipDateTime { get; set; }
        /// <summary>
        /// Start Load Time not End Time
        /// </summary>
        public string CarrierDescription { get; set; } = string.Empty;
        public string CarrierFEINNumber { get; set; } = string.Empty;
        /// <summary>
        /// Customer sales order number
        /// </summary>
        public string OrderNumber { get; set; } = string.Empty;
        public string ContractNumber { get; set; } = string.Empty;
        public override string ToString()
        {
            var values = new StringBuilder();
            values.Append($"\"{RecordType}\",\"{BolNumber}\",{NetQuantity.ToString(DtnConstants.NumberFormat0)},");
            values.Append($"{GrossQuantity.ToString(DtnConstants.NumberFormat0)},{Rate.ToString(DtnConstants.NumberFormat4)},");
            values.Append($"{LineTotal.ToString(DtnConstants.NumberFormat2)},\"{Description}\",\"{ProductCode}\",\"{BilledQuantityIndicator}\",");
            values.Append($"{QuantityBilled.ToString(DtnConstants.NumberFormat0)},\"{UoM}\",{ShipDateTime.ToString(DtnConstants.DateFormat)},");
            values.Append($"{ShipDateTime.ToString(DtnConstants.TimeFormat)},\"{CarrierDescription}\",\"{CarrierFEINNumber}\",\"{OrderNumber}\",\"{ContractNumber}\"");
            return values.ToString();
        }
    }
}
