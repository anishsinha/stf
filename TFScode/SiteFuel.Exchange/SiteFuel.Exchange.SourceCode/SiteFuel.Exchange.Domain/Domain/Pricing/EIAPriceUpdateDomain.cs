using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Easy.Common;
using Easy.Common.Interfaces;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.EIAPriceData;

namespace SiteFuel.Exchange.Domain
{
    public class EIAPriceUpdateDomain : BaseDomain
    {
        public EIAPriceUpdateDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public EIAPriceUpdateDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<bool> GetEIAPriceUpdates()
        {
            try
            {
                var appDomain = new ApplicationDomain(this);
                DateTime lastUpdatedOn = DateTime.Now;
                if (!IsEaiHoliday(lastUpdatedOn))
                {
                    var areaMappingDetails = Context.DataContext.EIAAreaMappings.Where(t => t.IsActive).ToList();
                    foreach (var item in areaMappingDetails)
                    {
                        if (CanCallEaiService(item.FuelSurchageArea, item.ProductType, item.PricingType, ref lastUpdatedOn))
                        {
                            var updatedPriceList = await GetEaiDataAsync(item.SeriesId, appDomain);
                            bool result = await SaveEIAData(updatedPriceList, item.FuelSurchageArea, item.ProductType, item.PricingType, lastUpdatedOn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("EIAPriceUpdateDomain", "GetEIAPriceUpdates", ex.Message, ex);
            }
            return true;
        }

        private async Task<bool> SaveEIAData(EIAPriceData updatedPriceList, FuelSurchageArea fuelSurchageArea, SurchargeProductTypes productType, FuelSurchagePricingType pricingType, DateTime lastUpdatedateTime)
        {
            var response = false;

            var priceList = updatedPriceList.ToEntity(fuelSurchageArea, productType, pricingType, lastUpdatedateTime);
            if (priceList != null && priceList.Any())
            {
                var year = 2016;
                if (fuelSurchageArea != FuelSurchageArea.US)
                    year = 2021;
                priceList = priceList.Where(t => t.EffectiveToDate.Year >= year).OrderByDescending(t => t.EffectiveFromDate).ToList();

                //save data to table
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        Context.DataContext.EIAPriceUpdates.AddRange(priceList);
                        await Context.CommitAsync();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("EIAPriceUpdateDomain", "SaveEIAData", ex.Message, ex);
                    }
                }
            }
            return response;
        }

        private bool CanCallEaiService(FuelSurchageArea fuelSurchageArea, SurchargeProductTypes productType, FuelSurchagePricingType pricingType, ref DateTime lastUpdatedDate)
        {
            var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset("Eastern Standard Time").Date;
            lastUpdatedDate = GetLastUpdatedDate(fuelSurchageArea, productType, pricingType);
            var daysFromLastUpdate = (currentDate.Date - lastUpdatedDate.Date).TotalDays;

            if (pricingType == FuelSurchagePricingType.Weekly)
            {
                if (currentDate.IsFirstDayOfWeek() && lastUpdatedDate.Date != currentDate)
                    return true;
                else
                {
                    if (daysFromLastUpdate >= 7)
                        return true;
                }
            }
            else if (pricingType == FuelSurchagePricingType.Monthly)
            {
                if (currentDate.IsFirstDayOfMonth() && lastUpdatedDate.Date != currentDate)
                    return true;
                else
                {
                    if (daysFromLastUpdate >= DateTime.DaysInMonth(currentDate.Year, currentDate.Month) + DateTime.DaysInMonth(lastUpdatedDate.Date.Year, lastUpdatedDate.Date.Month))
                        return true;
                }
            }
            else if (pricingType == FuelSurchagePricingType.Annualy)
            {
                if (currentDate.IsFirstDayOfYear() && lastUpdatedDate.Date != currentDate)
                    return true;
                else
                {
                    if (daysFromLastUpdate >= currentDate.DaysInYear() + lastUpdatedDate.Date.DaysInYear())
                        return true;
                }
            }

            return false;
        }

        private DateTime GetLastUpdatedDate(FuelSurchageArea fuelSurchageArea,SurchargeProductTypes productType, FuelSurchagePricingType pricingType)
        {
            return Context.DataContext.EIAPriceUpdates.Where(t => t.FuelSurchageArea == fuelSurchageArea && t.ProductType == productType && t.PricingType == pricingType)
                                .OrderByDescending(t => t.EffectiveToDate)
                                .Select(t => t.EffectiveFromDate)
                                .FirstOrDefault();
        }

        private bool IsEaiHoliday(DateTimeOffset currentDate)
        {
            var holidayDates = new MasterDomain().GetHolidayList(Constants.EaiHolidayList);

            if (holidayDates.Any() && !holidayDates.Contains(currentDate.Date))
                    return false;

            return true;
        }

        private async Task<EIAPriceData> GetEaiDataAsync(string seriesId, ApplicationDomain appDomain)
        {
            EIAPriceData priceList = new EIAPriceData();
            var Url = GetUrl(seriesId, appDomain);
            if (!string.IsNullOrWhiteSpace(Url))
            {
                using (IRestClient client = new RestClient())
                using (var request = new HttpRequestMessage(HttpMethod.Get, Url))
                using (var response = await client.SendAsync(request))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    priceList = JsonConvert.DeserializeObject<EIAPriceData>(content);
                }
            }
            return priceList;
        }

        private string GetUrl(string seriesId, ApplicationDomain appDomain)
        {
            var urlWithToken = appDomain.GetApplicationSettingValue<string>(Constants.EaiUrlKey, Constants.EaiUrlKey);
            if (!string.IsNullOrWhiteSpace(urlWithToken))
            {
                urlWithToken = urlWithToken + seriesId;

                return urlWithToken;
            }
            return string.Empty;
        }

        public decimal GetEIAPrice(FuelSurchagePricingType surchargePricingType, SurchargeProductTypes surchargeProductType, DateTime dropDate, FuelSurchageArea fuelSurchageArea = FuelSurchageArea.US)
        {
            var latestPrice = Context.DataContext.EIAPriceUpdates.Where(t => t.FuelSurchageArea == fuelSurchageArea && t.ProductType == surchargeProductType && t.IsActive
                                && t.PricingType == surchargePricingType
                                && t.EffectiveFromDate <= dropDate
                                && t.EffectiveToDate >= dropDate)
                                .OrderByDescending(t => t.EffectiveToDate)
                                .Select(t => t.Price)
                                .FirstOrDefault();

            if (latestPrice == 0)
                latestPrice = Context.DataContext.EIAPriceUpdates.Where(t => t.FuelSurchageArea == fuelSurchageArea && t.ProductType == surchargeProductType && t.IsActive
                                && t.PricingType == surchargePricingType
                                && t.EffectiveFromDate <= dropDate)
                                .OrderByDescending(t => t.EffectiveToDate)
                                .Select(t => t.Price)
                                .FirstOrDefault();
            return latestPrice;
        }
    }
}
