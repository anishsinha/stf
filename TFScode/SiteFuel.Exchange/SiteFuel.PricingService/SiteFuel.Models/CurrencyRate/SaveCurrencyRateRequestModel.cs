using System;
using System.Collections.Generic;

namespace SiteFuel.Models
{
    public class SaveCurrencyRateRequestModel
    {
        public List<CurrencyRateInputModel> ConversionRates { get; set; }
        public DateTimeOffset ExchangeDate { get; set; }
    }

    public class CurrencyRateInputModel
    {
        public string Item1 { get; set; }
        public string Item2 { get; set; }
        public decimal Item3 { get; set; }
    }
}
