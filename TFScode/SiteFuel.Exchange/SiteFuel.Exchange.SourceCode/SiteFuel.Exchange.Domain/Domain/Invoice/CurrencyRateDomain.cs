using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class CurrencyRateDomain : PricingApiDomain
    {
        private static readonly int cacheTimePeriodInSecs = 14400; //4 hours

        public CurrencyRateDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public CurrencyRateDomain(BaseDomain domain) : base(domain)
        {
        }

        public decimal GetCurrencyRate(Currency fromCurrency, Currency toCurrency, DateTimeOffset dateTimeOffset)
        {
            decimal response = 1;
            if (fromCurrency == toCurrency)
                return response;

            CurrencyRate currencyRate = GetCurrencyRateObject(fromCurrency, toCurrency, dateTimeOffset);
            if (currencyRate != null)
            {
                response = currencyRate.ExchangeRate;
            }
            else
            {
                LogCurrencyWarning(fromCurrency, toCurrency, dateTimeOffset);
            }
            return response;
        }

        public void SaveExchangeRate(List<Tuple<string, string, decimal>> conversionRates, DateTimeOffset exchangeDate)
        {
            foreach (var item in conversionRates)
            {
                Context.DataContext.CurrencyRates.Add(
                    new DataAccess.Entities.CurrencyRate
                    {
                        CreatedDate = exchangeDate,
                        ExchangeRate = item.Item3,
                        FromCurrency = item.Item1,
                        ToCurrency = item.Item2
                    }
                    );
            }

            Context.DataContext.SaveChanges();
        }

        public async Task<StatusViewModel> SaveExchangeRateInPricing(List<Tuple<string, string, decimal>> conversionRates, DateTimeOffset exchangeDate)
        {
            StatusViewModel response = null;
            try
            {
                var apiUrl = ApplicationConstants.UrlSaveExchangeRates;
                var requestModel = new { ConversionRates = conversionRates, ExchangeDate = exchangeDate };
                response = await ApiPostCall<StatusViewModel>(apiUrl, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CurrencyRateDomain", "SaveExchangeRateInPricing", ex.Message, ex);
            }
            return response;
        }

        public DateTimeOffset? GetLatestConversionEntryDate()
        {
            var currencyRate = Context.DataContext.CurrencyRates
                                    .OrderByDescending(t => t.Id).FirstOrDefault();
            if (currencyRate != null)
            {
                return currencyRate.CreatedDate;
            }

            return null;
        }

        public decimal Convert(Currency fromCurrency, Currency toCurrency, decimal pricePerQty, DateTimeOffset dateTime)
        {
            decimal currencyXRate = GetCurrencyRate(fromCurrency, toCurrency, dateTime);
            pricePerQty = Math.Round(pricePerQty * currencyXRate, 8);

            return pricePerQty;
        }

        public List<CurrencyRateViewModel> GetCurrencyRatesForAvalara(Currency fromCurrency, Currency toCurrency, DateTimeOffset dateTimeOffset, Currency jobCurrency)
        {
            var response = new List<CurrencyRateViewModel>();
            var currencyRate1 = GetCurrencyRateObject(fromCurrency, toCurrency, dateTimeOffset);
            if (currencyRate1 != null)
            {
                currencyRate1.CreatedDate = dateTimeOffset;
                response.Add(currencyRate1.ToViewModel());
            }
            var currencyRate2 = GetCurrencyRateObject(toCurrency, fromCurrency, dateTimeOffset);
            if (currencyRate2 != null)
            {
                currencyRate2.CreatedDate = dateTimeOffset;
                response.Add(currencyRate2.ToViewModel());
            }

            // #13857 - send exchange rate as 1 to Avalara, if job is from Canada.
            if (jobCurrency == Currency.CAD || fromCurrency == toCurrency)
            {
                foreach (var currencyRateObj in response)
                {
                    currencyRateObj.ExchangeRate = 1;
                }
            }

            return response;
        }

        private CurrencyRate GetCurrencyRateObject(Currency fromCurrency, Currency toCurrency, DateTimeOffset dateTimeOffset)
        {
            int period = dateTimeOffset.Hour / 4;
            var cacheId = $"{fromCurrency}-{toCurrency}_{dateTimeOffset.Date.ToString("dd-MM")}_{period}";
            CurrencyRate currencyRate = CacheManager.Get<CurrencyRate>(cacheId);
            if (currencyRate == null)
            {
                currencyRate = GetCurrencyRateQuery(fromCurrency, toCurrency, dateTimeOffset);
                if (currencyRate != null)
                {
                    period = currencyRate.CreatedDate.Hour / 4;
                    cacheId = $"{fromCurrency}-{toCurrency}_{currencyRate.CreatedDate.ToString("dd-MM")}_{period}";
                    CacheManager.Set(cacheId, currencyRate, cacheTimePeriodInSecs);
                }
            }
            return currencyRate;
        }

        private CurrencyRate GetCurrencyRateQuery(Currency fromCurrency, Currency toCurrency, DateTimeOffset dateTimeOffset)
        {
            var currencyRate = Context.DataContext.CurrencyRates
                    .Where(t => t.FromCurrency.Equals(fromCurrency.ToString())
                    && t.ToCurrency.Equals(toCurrency.ToString())
                    && t.CreatedDate.Day == dateTimeOffset.Day && t.CreatedDate.Month == dateTimeOffset.Month &&
                    t.CreatedDate.Year == dateTimeOffset.Year).OrderByDescending(t => t.Id).FirstOrDefault();

            if (currencyRate == null)
            {
                currencyRate = Context.DataContext.CurrencyRates
                    .Where(t => t.FromCurrency.Equals(fromCurrency.ToString())
                    && t.ToCurrency.Equals(toCurrency.ToString()))
                    .OrderByDescending(t => t.Id).FirstOrDefault();
            }

            return currencyRate;
        }

        private static void LogCurrencyWarning(Currency fromCurrency, Currency toCurrency, DateTimeOffset dateTimeOffset)
        {
            Logger.LogManager.Logger.WriteException("CurrencyRateDomain", "GetCurrencyRate", $"Exchange rate for {fromCurrency} => {toCurrency} not found and will use 1 as currency rate ",
                                new Exception($"Exchange rate for {fromCurrency} => {toCurrency} not found and will use 1 as currency rate "));

            AuditLogger.AddAuditLog(new UserContext(), new AuditLogViewModel()
            {
                Message = $"Exchange rate for {fromCurrency} => {toCurrency} not found for {dateTimeOffset} and will use 1 as currency rate ",
                CallSite = "CurrencyRateDomain : GetCurrencyRate",
                AuditEventType = AuditEventType.CurrencyConversion.ToString()
            });
        }
    }
}
