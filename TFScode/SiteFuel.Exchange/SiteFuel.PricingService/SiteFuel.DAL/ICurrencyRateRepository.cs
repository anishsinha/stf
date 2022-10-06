using SiteFuel.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.DAL
{
    public interface ICurrencyRateRepository
    {
        Task SaveExchangeRate(List<CurrencyRateInputModel> conversionRates, DateTimeOffset exchangeDate);
        IQueryable<CurrencyRate> GetCurrencyRateQuery(Currency fromCurrency, Currency toCurrency, DateTimeOffset dateTimeOffset);
    }
}
