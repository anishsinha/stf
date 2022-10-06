using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.ViewModels
{
    public class CsvOrderTaxes
    {
        public CsvOrderTaxes(string orderTaxes)
        {
            Taxes = new List<CsvOrderTax>();
            if (!string.IsNullOrWhiteSpace(orderTaxes))
            {
                var fields = orderTaxes.Split(';');
                foreach (var field in fields)
                {
                    var feeRecord = field.Split(' ');
                    var description = feeRecord.Length > 2 ? string.Join(" ", feeRecord.Skip(2)) : string.Empty;
                    if (feeRecord.Length > 1)
                        Taxes.Add(new CsvOrderTax(feeRecord[0], feeRecord[1], description));
                }
            }
        }

        public List<CsvOrderTax> Taxes { get; set; }
    }

    public class CsvOrderTax
    {
        public CsvOrderTax(string taxType, string taxRate, string taxDescription)
        {
            TaxRate = decimal.Parse(taxRate.Replace("$", string.Empty).Replace("%", string.Empty));
            TaxDescription = taxDescription;
            switch (taxType.ToLower())
            {
                case "dollarontotalamount":
                    TaxPricingType = OtherProductTaxPricingType.DollarOnTotalAmount;
                    break;

                case "perncentageontotalamount":
                    TaxPricingType = OtherProductTaxPricingType.PercentageOnTotalAmount;
                    break;

                case "dollarpergallon":
                case "dollarperlitre":
                    TaxPricingType = OtherProductTaxPricingType.DollarPerGallon;
                    break;

                case "percentagepergallon":
                case "percentageperlitre":
                    TaxPricingType = OtherProductTaxPricingType.PercentagePerGallon;
                    break;

                default:
                    throw new FormatException("Order tax is not in proper format");
            }
        }

        public OtherProductTaxPricingType TaxPricingType { get; set; }

        public decimal TaxRate { get; set; }

        public string TaxDescription { get; set; }
    }
}
