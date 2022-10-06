using SiteFuel.Exchange.Utilities;
using SiteFuel.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface ICurrencyRateDomain
    {
        Task<BaseResponseModel> SaveExchangeRate(List<CurrencyRateInputModel> conversionRates, DateTimeOffset exchangeDate);
        decimal Convert(int fromCurrency, int toCurrency, decimal pricePerQty, DateTimeOffset dateTime);
    }
}
