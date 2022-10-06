using SiteFuel.BAL;
using SiteFuel.DAL;
using SiteFuel.DataAccess.Entities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public class CurrencyRateDomain : ICurrencyRateDomain
    {
        private static readonly int cacheTimePeriodInSecs = 14400; //4 hours   
        readonly ICurrencyRateRepository _currencyRepository;
        public CurrencyRateDomain()
        {
            _currencyRepository = new CurrencyRateRepository();
        }

        private decimal GetCurrencyRate(int fromCurrency, int toCurrency, DateTimeOffset dateTimeOffset)
        {
            decimal response = 1;
            if (fromCurrency == toCurrency)
                return response;

            CurrencyRate currencyRate = GetCurrencyRateObject((Currency)fromCurrency, (Currency)toCurrency, dateTimeOffset);
            if (currencyRate != null)
            {
                response = currencyRate.ExchangeRate;
            }
            else
            {
                LogManager.Logger.WriteException("CurrencyRateDomain", "GetCurrencyRate", $"Exchange rate for {(Currency)fromCurrency} => {(Currency)toCurrency} not found and will use 1 as currency rate ",
                                new Exception($"Exchange rate for {(Currency)fromCurrency} => {(Currency)toCurrency} not found and will use 1 as currency rate "));
            }
            return response;
        }

        public async Task<BaseResponseModel> SaveExchangeRate(List<CurrencyRateInputModel> conversionRates, DateTimeOffset exchangeDate)
        {
            BaseResponseModel baseResponseModel = new BaseResponseModel(Status.Failed);
            try
            {
                await _currencyRepository.SaveExchangeRate(conversionRates, exchangeDate);
                baseResponseModel.Status = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CurrencyRateDomain", "SaveExchangeRate", ex.Message, ex);
            }
            return baseResponseModel;
        }

        public decimal Convert(int fromCurrency, int toCurrency, decimal pricePerQty, DateTimeOffset dateTime)
        {
            decimal currencyXRate = GetCurrencyRate(fromCurrency, toCurrency, dateTime);
            pricePerQty = Math.Round(pricePerQty * currencyXRate, 8);

            return pricePerQty;
        }      

        private CurrencyRate GetCurrencyRateObject(Currency fromCurrency, Currency toCurrency, DateTimeOffset dateTimeOffset)
        {
            int period = dateTimeOffset.Hour / 4;
            var cacheId = $"{fromCurrency}-{toCurrency}_{dateTimeOffset.Date.ToString("dd-MM")}_{period}";
            CurrencyRate currencyRate = CacheManager.Get<CurrencyRate>(cacheId);
            if (currencyRate == null)
            {
                currencyRate = _currencyRepository.GetCurrencyRateQuery(fromCurrency, toCurrency, dateTimeOffset).FirstOrDefault();
                if (currencyRate == null)
                {
                    currencyRate = _currencyRepository.GetCurrencyRateQuery(fromCurrency, toCurrency, dateTimeOffset.AddDays(-1)).FirstOrDefault();
                }
                if (currencyRate != null)
                {
                    period = currencyRate.CreatedDate.Hour / 4;
                    cacheId = $"{fromCurrency}-{toCurrency}_{currencyRate.CreatedDate.ToString("dd-MM")}_{period}";
                    CacheManager.Set(cacheId, currencyRate, cacheTimePeriodInSecs);
                }
            }
            return currencyRate;
        }

    }
}
