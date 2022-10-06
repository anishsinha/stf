using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.EIAPriceData;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class EaiPriceMapper
    {
        public static List<EIAPriceUpdate> ToEntity(this EIAPriceData viewModel, FuelSurchageArea fuelSurchageArea, SurchargeProductTypes productType, FuelSurchagePricingType pricingType, DateTime lastUpdatedOn, List<EIAPriceUpdate> entities = null)
        {
            if (entities == null)
                entities = new List<EIAPriceUpdate>();

            try
            {
                foreach (var series in viewModel.series)
                {
                    foreach (var data in series.data)
                    {
                        var entity = new EIAPriceUpdate();
                        entity.FuelSurchageArea = fuelSurchageArea;
                        entity.ProductType = productType;
                        entity.PricingType = pricingType;
                        entity.IsActive = true;
                        entity.PriceAddedDate = DateTimeOffset.Now;
                        entity.SeriesId = series.series_id;
                        entity.SeriesName = series.name;

                        entity.Price = Convert.ToDecimal(data[1]);
                        entity.EffectiveFromDate = GetFromDate((string)data[0], pricingType);
                        entity.EffectiveToDate = GetToDate((string)data[0], pricingType);
                        entity.PublishDate = GetPublishDate((string)data[0], pricingType);
                        
                        if (entity.EffectiveToDate.Date > lastUpdatedOn.Date || lastUpdatedOn.Date == DateTime.Now.Date)
                        {
                            entities.Add(entity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("EaiPriceMapper", "ToEntity", ex.Message, ex);
            }

            return entities;
        }

        private static DateTime GetFromDate(string dateTime, FuelSurchagePricingType pricingType)
        {
            DateTime response;
            switch (pricingType)
            {
                case FuelSurchagePricingType.Weekly:
                    response = DateTime.ParseExact(dateTime, "yyyyMMdd", CultureInfo.InvariantCulture);
                    response = response.AddDays(1);
                    break;
                case FuelSurchagePricingType.Monthly:
                    DateTime monthlydate = DateTime.ParseExact(dateTime, "yyyyMM", CultureInfo.InvariantCulture);
                    response = new DateTime(monthlydate.Year, monthlydate.Month, 1);
                    break;
                case FuelSurchagePricingType.Annualy:
                    DateTime annualyDate = DateTime.ParseExact(dateTime, "yyyy", CultureInfo.InvariantCulture);
                    response = new DateTime(annualyDate.Year, 1, 1);
                    break;
                default:
                    response = DateTime.Now;
                    break;
            }

            return response;
        }

        private static DateTime GetPublishDate(string dateTime, FuelSurchagePricingType pricingType)
        {
            DateTime response;
            switch (pricingType)
            {
                case FuelSurchagePricingType.Weekly:
                    response = DateTime.ParseExact(dateTime, "yyyyMMdd", CultureInfo.InvariantCulture);
                    break;
                case FuelSurchagePricingType.Monthly:
                    DateTime monthlydate = DateTime.ParseExact(dateTime, "yyyyMM", CultureInfo.InvariantCulture);
                    response = new DateTime(monthlydate.Year, monthlydate.Month, 1);
                    break;
                case FuelSurchagePricingType.Annualy:
                    DateTime annualyDate = DateTime.ParseExact(dateTime, "yyyy", CultureInfo.InvariantCulture);
                    response = new DateTime(annualyDate.Year, 1, 1);
                    break;
                default:
                    response = DateTime.Now;
                    break;
            }

            return response;
        }

        private static DateTime GetToDate(string dateTime, FuelSurchagePricingType pricingType)
        {
            DateTime response;
            switch (pricingType)
            {
                case FuelSurchagePricingType.Weekly:
                    response = DateTime.ParseExact(dateTime, "yyyyMMdd", CultureInfo.InvariantCulture);
                    //if monday - add 6 days //if tuesday - add 5 days and so on
                    switch (response.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            response = response.AddDays(6);
                            break;
                        case DayOfWeek.Tuesday:
                            response = response.AddDays(5);
                            break;
                        case DayOfWeek.Wednesday:
                            response = response.AddDays(4);
                            break;
                        case DayOfWeek.Thursday:
                            response = response.AddDays(3);
                            break;
                        case DayOfWeek.Friday:
                            response = response.AddDays(2);
                            break;
                        case DayOfWeek.Saturday:
                            response = response.AddDays(1);
                            break;
                    }
                    response = response.AddDays(1);
                    break;
                case FuelSurchagePricingType.Monthly:
                    response = DateTime.ParseExact(dateTime, "yyyyMM", CultureInfo.InvariantCulture);
                    response = response.AddDays(DateTime.DaysInMonth(response.Year, response.Month) - 1);
                    break;
                case FuelSurchagePricingType.Annualy: 
                    response = DateTime.ParseExact(dateTime, "yyyy", CultureInfo.InvariantCulture);
                    response = response.AddMonths(11);
                    response = response.AddDays(DateTime.DaysInMonth(response.Year, 12) - 1);
                    break;

                default:
                    response = DateTime.Now;
                    break;
            }

            return response;
        }
    }
}
