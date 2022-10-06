using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class SalesCalculatorInputViewModel
    {
        public DateTimeOffset PricingDate { get; set; }

        public int ExternalProductId { get; set; }

        public decimal SrcLatitude { get; set; }

        public decimal SrcLongitude { get; set; }

        public int RecordCount { get; set; }

        public int CityGroupTerminalId { get; set; }

        public string CountryCode { get; set; }

        public int? PricingCodeId { get; set; }

        public int CompanyCountryId { get; set; }
    }

    public class SalesCalculatorGridViewModel
    {
        public int TerminalId { get; set; }

        public string Abbreviation { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string Address { get; set; } 

        public string ZipCode { get; set; }

        public string StateCode { get; set; }

        public decimal PriceAvg { get; set; }

        public double Distance { get; set; }

        public DateTimeOffset PricingDate { get; set; }

        public decimal TaxValue { get; set; }

        public decimal PriceLow { get; set; }

        public decimal PriceHigh { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Currency Currency { get; set; }

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

        public string UoD { get; set; }
    }

    public class SalesCalculatorApiResponse
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public List<SalesCalculatorGridViewModel> TerminalPrices { get; set; } = new List<SalesCalculatorGridViewModel>();
    }

    public class GetTerminalApiResponse
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public List<TerminalDetails> Terminals { get; set; } = new List<TerminalDetails>();
    }
}
