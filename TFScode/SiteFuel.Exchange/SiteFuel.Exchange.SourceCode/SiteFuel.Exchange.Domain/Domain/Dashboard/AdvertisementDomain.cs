using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class AdvertisementDomain : BaseDomain
    {
        public AdvertisementDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public AdvertisementDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<RequestPriceOutputViewModel> RequestPriceAsync(RequestPriceViewModel viewModel)
        {
            var response = new RequestPriceOutputViewModel();
            try
            {

                int externalProductId = 0;
                decimal latitude = 0, longitude = 0;
                string countryCode = Constants.CountryUSA;

                var geoCodes = GoogleApiDomain.GetGeocode(viewModel.ZipCode);
                if (geoCodes != null)
                {
                    latitude = Convert.ToDecimal(geoCodes.Latitude);
                    longitude = Convert.ToDecimal(geoCodes.Longitude);
                    countryCode = geoCodes.CountryCode;
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageInvalidZipCode;
                }

                var productMap = await Context.DataContext.MstProductMappings.FirstOrDefaultAsync(t => t.ProductId == viewModel.ProductId);
                if (productMap != null)
                {
                    externalProductId = productMap.ExternalProductId;
                }

                if (externalProductId > 0 && latitude != 0 && longitude != 0)
                {
                    var inputModel = new SalesCalculatorInputViewModel
                    {
                        PricingDate = viewModel.RequestDateTime,
                        ExternalProductId = externalProductId,
                        SrcLatitude = latitude,
                        SrcLongitude = longitude,
                        RecordCount = 1,
                        CountryCode = countryCode
                    };

                    var pricingDomain = new PricingServiceDomain(this);
                    var pricingData = await pricingDomain.GetTerminalPricesForCalculator(inputModel);
                    if (pricingData.Count == 1)
                    {
                        var pricing = pricingData.First();
                        var tierPrice = GetTierPrice(viewModel.Quantity, pricing.PriceAvg);

                        var entity = new RequestPrice();
                        entity.ZipCode = viewModel.ZipCode;
                        entity.Quantity = viewModel.Quantity;
                        entity.ProductId = viewModel.ProductId;
                        entity.RequestDateTime = viewModel.RequestDateTime;
                        entity.TerminalId = pricing.TerminalId;
                        entity.PricePerGallon = tierPrice.GetPreciseValue(2);
                        entity.PricingDate = pricing.PricingDate;

                        Context.DataContext.RequestPrices.Add(entity);
                        await Context.CommitAsync();

                        response.Id = entity.Id;
                        response.PricePerGallon = entity.PricePerGallon;
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageSuccess;
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageUnableToFetchPricePerGallon;
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AdvertisementDomain", "RequestPriceAsync", ex.Message, ex);
            }
            return response;
        }

        private decimal GetTierPrice(decimal quantity, decimal ppg)
        {
            decimal response = ppg;
            var tierPricing = Context
                                .DataContext
                                .MstAdvertisementTierPricings
                                .FirstOrDefault
                                (
                                    t =>
                                    t.MinQuantity <= (int)quantity &&
                                    (t.MaxQuantity == null || t.MaxQuantity >= (int)quantity)
                                );
            if (tierPricing != null)
            {
                response = response + tierPricing.Amount;
            }
            return response;
        }

        public async Task<ResponseViewModel> RequestFuelAsync(RequestFuelViewModel viewModel)
        {
            var response = new ResponseViewModel(Status.Success);
            try
            {
                var entity = new RequestFuel
                {
                    RequestPriceId = viewModel.RequestPriceId,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    PhoneNumber = viewModel.PhoneNumber,
                    Email = string.IsNullOrWhiteSpace(viewModel.Email) ? null : viewModel.Email,
                    CompanyName = string.IsNullOrWhiteSpace(viewModel.CompanyName) ? null : viewModel.CompanyName,
                    RequestDateTime = viewModel.RequestDateTime,
                    IsEmailSentToSales = false,
                    EmailSentDateTime = null,
                    IsCustomerContacted = false,
                    CustomerContactedDateTime = null,
                    IsBusinessDone = false
                };

                Context.DataContext.RequestFuels.Add(entity);
                await Context.CommitAsync();

                await ContextFactory.Current.GetDomain<NotificationDomain>()
                                            .AddNotificationEventAsync(
                                                EventType.NeedFuelIntimationCreatedUsingAdvertisementWidget,
                                                entity.Id,
                                                (int)SystemUser.System);
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageFailed;
                LogManager.Logger.WriteException("AdvertisementDomain", "RequestFuelAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<RequestPriceGridViewModel>> GetRequestedPricesAsync(string fromDate, string toDate)
        {
            var response = new List<RequestPriceGridViewModel>();
            try
            {
                DateTimeOffset startDate = DateTimeOffset.Now.Date.AddDays(ApplicationConstants.DateFilterDefaultDays);
                DateTimeOffset endDate = DateTimeOffset.Now.Date.AddDays(1);
                var helperDomain = new HelperDomain(this);
                if (!string.IsNullOrEmpty(fromDate))
                {
                    startDate = Convert.ToDateTime(fromDate).Date;
                }
                if (!string.IsNullOrEmpty(toDate))
                {
                    endDate = Convert.ToDateTime(toDate).Date.AddDays(1);
                }

                var requestedPrices = Context.DataContext.RequestPrices
                                                .Include(t => t.MstProduct)
                                                .Include(t => t.MstExternalTerminal)
                                                .Where(t => t.RequestDateTime >= startDate && t.RequestDateTime < endDate)
                                                .OrderByDescending(t => t.Id);

                await requestedPrices.ForEachAsync(t => response.Add(new RequestPriceGridViewModel
                {
                    Id = t.Id,
                    ZipCode = t.ZipCode,
                    Quantity = t.Quantity,
                    ProductId = t.ProductId,
                    ProductName = helperDomain.GetProductName(t.MstProduct),
                    RequestDateTime = t.RequestDateTime.ToString(@Resource.constFormatDate),
                    TerminalId = t.TerminalId,
                    TerminalName = t.MstExternalTerminal.Name,
                    PricePerGallon = t.PricePerGallon,
                    PricingDate = t.PricingDate.ToString(@Resource.constFormatDate)
                }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AdvertisementDomain", "GetRequestedPricesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<RequestFuelGridViewModel>> GetRequestedFuelsAsync(RequestFuelFilterType filter, string fromDate, string toDate)
        {
            var response = new List<RequestFuelGridViewModel>();
            try
            {
                DateTimeOffset startDate = DateTimeOffset.Now.Date.AddDays(ApplicationConstants.DateFilterDefaultDays);
                DateTimeOffset endDate = DateTimeOffset.Now.Date.AddDays(1);

                if (!string.IsNullOrEmpty(fromDate))
                {
                    startDate = Convert.ToDateTime(fromDate).Date;
                }
                if (!string.IsNullOrEmpty(toDate))
                {
                    endDate = Convert.ToDateTime(toDate).Date.AddDays(1);
                }

                var requestedPrices = Context.DataContext.RequestFuels
                                                .Where(t => t.RequestDateTime >= startDate &&
                                                            t.RequestDateTime < endDate).OrderByDescending(t => t.Id);
                switch (filter)
                {
                    case RequestFuelFilterType.CustomerContacted:
                        requestedPrices = requestedPrices.Where(t => t.IsCustomerContacted).OrderByDescending(t => t.Id);
                        break;
                    case RequestFuelFilterType.BusinessDone:
                        requestedPrices = requestedPrices.Where(t => t.IsBusinessDone).OrderByDescending(t => t.Id);
                        break;
                }

                await requestedPrices.ForEachAsync(t => response.Add(new RequestFuelGridViewModel
                {
                    Id = t.Id,
                    RequestPriceId = t.RequestPriceId,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    PhoneNumber = t.PhoneNumber,
                    Email = t.Email ?? Resource.lblPlaceholderText,
                    CompanyName = t.CompanyName ?? Resource.lblPlaceholderText,
                    RequestDateTime = t.RequestDateTime.ToString(@Resource.constFormatDate),
                    IsEmailSentToSales = t.IsEmailSentToSales,
                    EmailSentDateTime = t.EmailSentDateTime.HasValue ? t.EmailSentDateTime.Value.ToString(@Resource.constFormatDate) : Resource.lblPlaceholderText,
                    IsCustomerContacted = t.IsCustomerContacted,
                    CustomerContactedDateTime = t.CustomerContactedDateTime.HasValue ? t.CustomerContactedDateTime.Value.ToString(@Resource.constFormatDate) : Resource.lblPlaceholderText,
                    IsBusinessDone = t.IsBusinessDone
                }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AdvertisementDomain", "GetRequestedFuelsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ChangeRequestFuelStatusAsync(int id, RequestFuelStatusType type, bool isDone)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var requestFuel = await Context.DataContext.RequestFuels.SingleOrDefaultAsync(t => t.Id == id);
                    if (requestFuel != null)
                    {
                        switch (type)
                        {
                            case RequestFuelStatusType.IsCustomerContacted:
                                requestFuel.IsCustomerContacted = isDone;
                                requestFuel.CustomerContactedDateTime = isDone ? DateTimeOffset.Now : (DateTimeOffset?)null;
                                if (!isDone)
                                {
                                    requestFuel.IsBusinessDone = false;
                                }
                                Context.DataContext.Entry(requestFuel).State = EntityState.Modified;
                                break;
                            case RequestFuelStatusType.IsBusinessDone:
                                if (isDone)
                                {
                                    requestFuel.IsCustomerContacted = isDone;
                                    requestFuel.CustomerContactedDateTime = DateTimeOffset.Now;
                                }
                                requestFuel.IsBusinessDone = isDone;
                                Context.DataContext.Entry(requestFuel).State = EntityState.Modified;
                                break;
                        }
                        await Context.CommitAsync();

                        transaction.Commit();

                        //Send response
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageSuccess;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("AdvertisementDomain", "ChangeRequestFuelStatusAsync", ex.Message, ex);
                }
            }
            return response;
        }
    }
}
