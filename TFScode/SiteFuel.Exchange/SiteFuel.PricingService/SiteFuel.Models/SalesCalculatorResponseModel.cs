using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models
{
    public class SalesCalculatorResponseModel : BaseResponseModel
    {
        public SalesCalculatorResponseModel()
        {
        }
        public SalesCalculatorResponseModel(Status status) : base(status)
        {
            Status = status;
        }
        public List<PricingData> TerminalPrices { get; set; }
    }

    public class PricingData
    {
        public int TerminalId { get; set; }

        public string Abbreviation { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string StateCode { get; set; }

        public decimal PriceAvg { get; set; }

        public double Distance { get; set; }

        public DateTimeOffset PricingDate { get; set; }

        public decimal TaxValue { get; set; }

        public decimal PriceLow { get; set; }

        public decimal PriceHigh { get; set; }

        public int Currency { get; set; }

        public decimal Price { get; set; }

        public string PriceLoadDate { get; set; }

        public string ReportedDate { get; set; }

        public int TotalCount { get; set; }

        public string RackTypeName { get; set; }

        public string FeedTypeName { get; set; }

        public string FuelClassTypeName { get; set; }

        public string PriceTypeName { get; set; }

        public string CurrencyName { get; set; }

        public string Amount { get; set; }

        public string Address { get; set; }
    }
}
