using SiteFuel.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.DAL
{
    public class CurrencyRateRepository : ICurrencyRateRepository
    {
        private DataContext dbContext;
        public CurrencyRateRepository()
        {
            dbContext = new DataContext();
        }

        public async Task SaveExchangeRate(List<CurrencyRateInputModel> conversionRates, DateTimeOffset exchangeDate)
        {
            foreach (var item in conversionRates)
            {
                dbContext.CurrencyRates.Add(
                    new CurrencyRate
                    {
                        CreatedDate = exchangeDate,
                        ExchangeRate = item.Item3,
                        FromCurrency = item.Item1,
                        ToCurrency = item.Item2
                    });
            }

            await dbContext.SaveChangesAsync();
        }

        public IQueryable<CurrencyRate> GetCurrencyRateQuery(Currency fromCurrency, Currency toCurrency, DateTimeOffset dateTimeOffset)
        {
            return dbContext.CurrencyRates
                    .Where(t => t.FromCurrency.Equals(fromCurrency.ToString())
                    && t.ToCurrency.Equals(toCurrency.ToString())
                    && t.CreatedDate.Day == dateTimeOffset.Day && t.CreatedDate.Month == dateTimeOffset.Month &&
                    t.CreatedDate.Year == dateTimeOffset.Year).OrderByDescending(t => t.Id);
        }
    }
}
