using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class QuoteRequestDomain : BaseDomain
    {
        public QuoteRequestDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public QuoteRequestDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<QuoteRequestViewModel> GetQuoteRequestAsync(int jobId)
        {
            QuoteRequestViewModel response = new QuoteRequestViewModel(Status.Success);
            try
            {
                if (jobId > 0)
                {
                    var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.Id == jobId);

                    response.Job = new JobSelectionViewModel(Status.Success) { JobId = job.Id, Name = job.Name };
                    response.Job.Country = job.MstCountry.ToViewModel();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QuoteRequestDomain", "GetQuoteRequestAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<QuoteResponseViewModel> GetQuoteResponseAsync(int quoteRequestId, UserContext userContext)
        {
            QuoteResponseViewModel response = new QuoteResponseViewModel(Status.Success);
            try
            {
                if (quoteRequestId > 0)
                {
                    var existingQuotation = await Context.DataContext.Quotations
                                        .FirstOrDefaultAsync(t => t.QuoteRequestId == quoteRequestId && t.IsActive && t.CreatedBy == userContext.Id);
                    if (existingQuotation != null)
                    {
                        response = existingQuotation.ToViewModel();
                        if (IsOtherFuelType(existingQuotation.QuoteRequest.FuelTypeId))
                            response.IsOtherFuelTypeInFavorite = true;
                    }
                    else
                    {
                        var quoteRequest = await Context.DataContext.QuoteRequests
                                            .FirstOrDefaultAsync(t => t.Id == quoteRequestId);
                        response.QuoteRequest = quoteRequest.ToViewModel();
                        response.FuelDeliveryDetails.DeliveryTypeId = quoteRequest.DeliveryTypeId;

                        if (IsOtherFuelType(quoteRequest.FuelTypeId))
                            response.IsOtherFuelTypeInFavorite = true;
                    }

                    response.FuelDeliveryDetails.FuelFees.Currency = response.QuoteRequest.Currency;
                    response.FuelDeliveryDetails.FuelFees.UoM = response.QuoteRequest.UoM;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QuoteRequestDomain", "GetQuoteResponseAsync", ex.Message, ex);
            }
            return response;
        }

        private bool IsOtherFuelType(int fuelTypeId)
        {
            return Context.DataContext.MstProducts.Any(t => t.Id == fuelTypeId && t.ProductTypeId == 10);
        }

        public QuoteRequestDataTableModel GetQuoteRequestFilter(int jobId, QuoteRequestFilterType filter)
        {
            var response = new QuoteRequestDataTableModel();
            try
            {
                response.JobId = jobId;
                response.Filter = filter;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetQuoteRequestFilter", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<QuoteRequestGridViewModel>> GetAllQuoteRequestsBuyerAsync(UserContext userContext, string startDate, string endDate, int jobId = 0, QuoteRequestFilterType filter = (int)QuoteRequestFilterType.All, int countryId = (int)Country.USA, Currency currency = Currency.USD, string groupIds = "")
        {
            using (var tracer = new Tracer("QuoteRequestDomain", "GetAllQuoteRequestsBuyerAsync"))
            {
                var response = new List<QuoteRequestGridViewModel>();
                try
                {
                    DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                    DateTimeOffset EndDate = DateTimeOffset.MaxValue.Date;
                    if (!string.IsNullOrEmpty(startDate))
                    {
                        StartDate = Convert.ToDateTime(startDate).Date;
                    }
                    if (!string.IsNullOrEmpty(endDate))
                    {
                        EndDate = Convert.ToDateTime(endDate).Date;
                    }
                    if (jobId > 0)
                    {
                        var job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == jobId);
                        if (job != null)
                        {
                            currency = job.Currency;
                            countryId = job.CountryId;
                        }
                    }

                    var helperDomain = new HelperDomain(this);
                    var jobIds = await helperDomain.GetJobIdsAsync(userContext.Id, groupIds);
                    var quoteRequests = await Context.DataContext.QuoteRequests.Where(t => (jobId == 0 || t.JobId == jobId) && jobIds.Contains(t.JobId)
                                                                                        && t.Job.CompanyId == userContext.CompanyId
                                                                                        && t.IsActive
                                                                                        && (countryId == (int)Country.All || (t.Job.CountryId == countryId && t.Currency == currency))
                                                                                        && t.QuoteDueDate >= StartDate && t.QuoteDueDate <= EndDate)
                                                                                        .OrderByDescending(t => t.CreatedDate).ToListAsync();
                    if (quoteRequests.Any())
                    {
                        foreach (var item in quoteRequests)
                        {
                            response.Add(new QuoteRequestGridViewModel()
                            {
                                Id = item.Id,
                                QuoteNumber = item.RequestNumber,
                                JobName = item.Job.Name,
                                JobId = item.Job.Id,
                                Address = $"{item.Job.MstState.Code}, {item.Job.ZipCode}",
                                FuelType = helperDomain.GetProductName(item.MstProduct),
                                GallonsRequested = item.Quantity.GetCommaSeperatedValue(),
                                QuoteDueDate = item.QuoteDueDate.ToString(Resource.constFormatDate),
                                QuotesNeeded = Convert.ToString(item.QuotesNeeded),
                                QuotesReceived = Convert.ToString(item.Quotations.Count(t => t.IsActive)),
                                Status = item.Quotations.Count(t => t.IsActive) >= item.QuotesNeeded ? Resource.lblCompleted : item.QuoteDueDate.Date < DateTimeOffset.Now.ToTargetDateTimeOffset(item.Job.TimeZoneName).Date ? Resource.lblExpired : item.QuoteRequestStatuses.First(t => t.IsActive).MstQuoteRequestStatus.Name,
                                StatusId = item.Quotations.Count(t => t.IsActive) >= item.QuotesNeeded ? (int)QuoteRequestStatuses.Completed : item.QuoteDueDate.Date < DateTimeOffset.Now.ToTargetDateTimeOffset(item.Job.TimeZoneName).Date ? (int)QuoteRequestStatuses.Expired : item.QuoteRequestStatuses.First(t => t.IsActive).MstQuoteRequestStatus.Id
                            });
                        }
                    }

                    if (filter == QuoteRequestFilterType.Open)
                    {
                        response = response.Where(t => t.StatusId == (int)QuoteRequestStatuses.Open).ToList();
                    }
                    else if (filter == QuoteRequestFilterType.Expired)
                    {
                        response = response.Where(t => t.StatusId == (int)QuoteRequestStatuses.Expired).ToList();
                    }
                    else if (filter == QuoteRequestFilterType.Canceled)
                    {
                        response = response.Where(t => t.StatusId == (int)QuoteRequestStatuses.Canceled).ToList();
                    }
                    else if (filter == QuoteRequestFilterType.Completed)
                    {
                        response = response.Where(t => t.StatusId == (int)QuoteRequestStatuses.Completed).ToList();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("QuoteRequestDomain", "GetAllQuoteRequestsAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<List<int>> GetOpenQuoteRequestsAsync()
        {
            var response = new List<int>();
            try
            {
                var entities = Context.DataContext.QuoteRequests.Where(
                                                t =>
                                                t.QuoteRequestStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)QuoteRequestStatuses.Open &&
                                                t.IsActive);
                await entities.ForEachAsync(t => response.Add(t.Id));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QuoteRequestDomain", "GetOpenQuoteRequestsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task ProcessQuoteRequestAsync(int id, int qrExpirationReminderTime)
        {
            var entity = await Context.DataContext.QuoteRequests.SingleOrDefaultAsync(t => t.Id == id);
            if (entity != null)
            {
                await ProcessNewIncomingQuoteRequestAsync(entity);
                await ProcessQuoteRequestExpirationAsync(entity, qrExpirationReminderTime);
            }
        }

        public async Task ProcessNewIncomingQuoteRequestAsync(QuoteRequest entity)
        {
            try
            {
                var currentTime = DateTimeOffset.Now;
                if (currentTime.DateTime.Subtract(entity.CreatedDate.DateTime).TotalHours >= 4)
                {
                    await QREmailToSuperAdmin(entity, EventType.NewIncomingQuoteRequest);
                }
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("QuoteRequestDomain", "ProcessNewIncomingQuoteRequestAsync", ex.Message, ex);
            }
        }

        public async Task ProcessQuoteRequestExpirationAsync(QuoteRequest entity, int qrExpirationReminderTime)
        {
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var currentTime = DateTimeOffset.Now.ToTargetDateTimeOffset(entity.Job.TimeZoneName);

                    if (entity.QuoteDueDate.Date.Subtract(currentTime.Date).TotalHours < qrExpirationReminderTime)
                    {
                        await QREmailToSuperAdmin(entity, EventType.QuoteRequestExpired);
                    }

                    if (entity.QuoteDueDate.Date < currentTime.Date)
                    {
                        var quoteRequestCurrentStatus = entity.QuoteRequestStatuses.FirstOrDefault(t => t.IsActive);
                        quoteRequestCurrentStatus.IsActive = false;
                        QuoteRequestStatus quoteRequestStatus = new QuoteRequestStatus();
                        quoteRequestStatus.QuoteRequestId = entity.Id;
                        quoteRequestStatus.StatusId = (int)QuoteRequestStatuses.Expired;
                        quoteRequestStatus.IsActive = true;
                        quoteRequestStatus.UpdatedBy = (int)SystemUser.System;
                        quoteRequestStatus.UpdatedDate = DateTimeOffset.Now;
                        entity.QuoteRequestStatuses.Add(quoteRequestStatus);

                        await Context.CommitAsync();
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("QuoteRequestDomain", "ProcessQuoteRequestExpirationAsync", ex.Message, ex);
                }
            }
        }

        private async Task QREmailToSuperAdmin(QuoteRequest quoteRequest, EventType eventTypeId)
        {
            if (!Context.DataContext.Notifications.Any(t => t.EventTypeId == (int)eventTypeId && t.EntityId == quoteRequest.Id))
            {
                await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(eventTypeId, quoteRequest.Id, (int)SystemUser.System);
            }
        }

        public async Task<DashboardSupplierQuoteRequestGridViewModel> GetAllQuoteRequestsSupplierAsync(USP_SupplierRequestsViewModel quoteRequestStat, string fromDate = "", string toDate = "")
        {
            using (var tracer = new Tracer("QuoteRequestDomain", "GetAllQuoteRequestsSupplierAsync"))
            {
                var response = new DashboardSupplierQuoteRequestGridViewModel();
                try
                {
                    var storedProcedureDomain = new StoredProcedureDomain(this);

                    DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                    DateTimeOffset EndDate = DateTimeOffset.MaxValue.Date;
                    if (!string.IsNullOrEmpty(fromDate))
                    {
                        StartDate = Convert.ToDateTime(fromDate).Date;
                    }
                    if (!string.IsNullOrEmpty(toDate))
                    {
                        EndDate = Convert.ToDateTime(toDate).Date;
                    }
                    quoteRequestStat.StartDate = StartDate;
                    quoteRequestStat.EndDate = EndDate;

                    var quoteRequests = await storedProcedureDomain.GetSupplierQuotesGrid(quoteRequestStat);

                    foreach (var item in quoteRequests)
                    {
                        response.RecentQuoteRequests.Add(new QuoteRequestGridViewModel
                        {
                            Id = item.Id,
                            Address = item.Address,
                            State = item.State,
                            ZipCode = item.ZipCode,
                            FuelType = item.FuelType,
                            GallonsRequested = item.GallonsRequested.GetCommaSeperatedValue(),
                            JobName = item.JobName,
                            QuoteDueDate = item.QuoteDueDate.ToString(Resource.constFormatDate),
                            QuoteNumber = item.QuoteNumber,
                            Status = item.Status,
                            StatusId = item.StatusId,
                            IsQuotationCreated = item.IsQuotationCreated,
                            QuotationStatusName = item.QuotationStatusName
                        });
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("QuoteRequestDomain", "GetAllQuoteRequestsSupplierAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<List<QuotationsGridViewModel>> GetAllQuotationsSupplierAsync(int userId, QuoteRequestDataTableModel model)
        {
            using (var tracer = new Tracer("QuoteRequestDomain", "GetAllQuotationsSupplierAsync"))
            {
                var response = new List<QuotationsGridViewModel>();
                try
                {
                    DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                    DateTimeOffset EndDate = DateTimeOffset.MaxValue.Date;
                    if (!string.IsNullOrEmpty(model.StartDate))
                    {
                        StartDate = Convert.ToDateTime(model.StartDate).Date;
                    }
                    if (!string.IsNullOrEmpty(model.EndDate))
                    {
                        EndDate = Convert.ToDateTime(model.EndDate).Date;
                    }

                    var helperDomain = new HelperDomain(this);
                    var quotations = await Context.DataContext.Quotations.Include(t => t.QuotationStatuses).Where(t =>
                                                        t.CreatedBy == userId && t.QuoteRequest.CreatedDate >= StartDate &&
                                                        t.QuoteRequest.CreatedDate <= EndDate && t.Currency == model.Currency && t.QuoteRequest.Job.CountryId == model.CountryId)
                                                      .OrderByDescending(t => t.CreatedDate)
                                                      .ToListAsync();
                    foreach (var item in quotations)
                    {
                        response.Add(new QuotationsGridViewModel
                        {
                            Id = item.Id,
                            CustomerQuoteRequestId = item.QuoteRequest.Id,
                            CustomerQuoteNumber = item.QuoteRequest.RequestNumber,
                            QuoteNumber = item.QuoteNumber,
                            RackPPG = helperDomain.GetPricePerGallon(item.PricePerGallon, item.PricingTypeId, item.RackAvgTypeId ?? 0),
                            CreatedDate = item.CreatedDate.ToString(Resource.constFormatDateTime),
                            Status = item.QuotationStatuses.Last(t => t.IsActive).MstQuoteRequestStatus.Name,
                        });
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("QuoteRequestDomain", "GetAllQuotationsSupplierAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<QuoteResponseViewModel> SaveQuotationAsync(UserContext userContext, QuoteResponseViewModel viewModel)
        {
            var newsfeedDomain = new NewsfeedDomain(this);
            using (var tracer = new Tracer("QuoteRequestDomain", "SaveQuotationAsync"))
            {
                if (string.IsNullOrWhiteSpace(viewModel.SupplierQuoteNumber))
                {
                    Random rndQuoteSuffix = new Random();
                    int suffix = rndQuoteSuffix.Next(1, 1000);
                    viewModel.SupplierQuoteNumber = viewModel.QuoteRequest.QuoteNumber + Resource.lblSingleHyphen + suffix.ToString("D4");
                }

                if (viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t =>
                        t.FeeSubTypeId == (int)FeeSubType.ByQuantity && !t.DeliveryFeeByQuantity.Any()))
                {
                    viewModel.StatusMessage = Resource.errMessageFeeByQuantityRequired;
                    return viewModel;
                }

                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (viewModel.QuotationId > 0)
                        {
                            // update quotation
                            var quotation = await Context.DataContext.Quotations.SingleOrDefaultAsync(t => t.Id == viewModel.QuotationId);
                            Context.DataContext.FuelRequestFees.RemoveRange(quotation.QuotationFees);
                            viewModel.ExchangeRate = quotation.ExchangeRate;
                            quotation = viewModel.ToEntity(quotation);
                            Context.DataContext.Entry(quotation).State = EntityState.Modified;
                            await Context.CommitAsync();
                            transaction.Commit();
                        }
                        else
                        {
                            CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
                            viewModel.ExchangeRate = currencyRateDomain.GetCurrencyRate(viewModel.QuoteRequest.Currency, Currency.USD, DateTimeOffset.Now);

                            // new quotation
                            var quote = viewModel.ToEntity();

                            // FuelRequestFee Entity - save fr fee details
                            FuelFeesDomain fuelFeesDomain = new FuelFeesDomain(this);
                            await fuelFeesDomain.SaveQuotationFuelFees(viewModel.FuelDeliveryDetails, quote, userContext);

                            QuotationStatus quoteStatus = new QuotationStatus();
                            quoteStatus.StatusId = (int)QuotationStatuses.Open;
                            quoteStatus.IsActive = true;
                            quoteStatus.UpdatedBy = userContext.Id;
                            quoteStatus.UpdatedDate = DateTimeOffset.Now;

                            quote.QuotationStatuses.Add(quoteStatus);
                            Context.DataContext.Quotations.Add(quote);

                            await Context.CommitAsync();
                            transaction.Commit();
                            quote = Context.DataContext.Quotations.Include(t1 => t1.QuoteRequest).First(t => t.Id == quote.Id);
                            await newsfeedDomain.SetQuotationCreatedNewsfeed(userContext, quote);

                            viewModel.StatusCode = Status.Success;
                            viewModel.QuotationId = quote.Id;
                            viewModel.StatusMessage = Resource.errMessageCreateQuotationSuccess;
                        }
                    }
                    catch (Exception ex)
                    {
                        viewModel.StatusMessage = Resource.errMessageCreateQuotationFailed;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("QuoteRequestDomain", "SaveQuotationAsync", ex.Message, ex);
                    }
                }
            }
            return viewModel;
        }

        public async Task<StatusViewModel> ExcludeQuote(int quoteId, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("QuoteRequestDomain", "SaveQuotationAsync"))
            {
                var quote = await Context.DataContext.Quotations.SingleOrDefaultAsync(t => t.Id == quoteId);
                var quoteCurrentStatus = quote.QuotationStatuses.FirstOrDefault(t => t.IsActive);
                quoteCurrentStatus.IsActive = false;

                QuotationStatus quoteStatus = new QuotationStatus();
                quoteStatus.QuotationId = quote.Id;
                quoteStatus.StatusId = quoteCurrentStatus.StatusId == (int)QuotationStatuses.Excluded ? (int)QuotationStatuses.Open : (int)QuotationStatuses.Excluded;
                quoteStatus.IsActive = true;
                quoteStatus.UpdatedBy = userContext.Id;
                quoteStatus.UpdatedDate = DateTimeOffset.Now;

                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        quote.QuotationStatuses.Add(quoteStatus);
                        await Context.CommitAsync();
                        transaction.Commit();
                        response.StatusCode = Status.Success;
                        response.StatusMessage = quoteCurrentStatus.StatusId == (int)QuotationStatuses.Excluded ? Resource.errMessageOpenQuoteSuccess : Resource.errMessageExcludeQuoteSuccess;
                    }
                    catch (Exception ex)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageExcludeQuoteFailed;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("QuoteRequestDomain", "ExcludeQuote", ex.Message, ex);
                    }
                }
            }
            return response;
        }

        public async Task<StatusViewModel> AwardQuote(int quoteId, UserContext userContext)
        {
            var quote = await Context.DataContext.Quotations.SingleOrDefaultAsync(t => t.Id == quoteId);
            var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userContext.Id);
            var quotationSupplier = quote.User;
            var fuelRequest = new FuelRequest();
            StatusViewModel response = new StatusViewModel();
            var helperDomain = new HelperDomain(this);
            bool IsPricingDetailIdNull = false;

            // basic FR details
            fuelRequest.JobId = quote.QuoteRequest.JobId;
            fuelRequest.ProductDisplayGroupId = quote.QuoteRequest.ProductDisplayGroupId;
            fuelRequest.FuelTypeId = quote.QuoteRequest.FuelTypeId;
            fuelRequest.FuelDescription = quote.QuoteRequest.FuelDescription;
            fuelRequest.OrderTypeId = quote.QuoteRequest.OrderTypeId;
            fuelRequest.IsPublicRequest = quote.QuoteRequest.IsPublicRequest;
            fuelRequest.QuantityTypeId = (int)QuantityType.SpecificAmount;
            fuelRequest.MaxQuantity = quote.QuoteRequest.Quantity;
            fuelRequest.EstimateGallonsPerDelivery = quote.QuoteRequest.EstimatedGallonsPerDelivery;
            fuelRequest.FuelRequestTypeId = (int)FuelRequestType.QuoteRequest;
            fuelRequest.User = user;

            fuelRequest.IsOverageAllowed = true;
            fuelRequest.OverageAllowedAmount = 0;
            fuelRequest.PaymentTermId = (int)PaymentTerms.DueOnReceipt;
            fuelRequest.NetDays = 0;
            fuelRequest.UoM = quote.UoM;
            fuelRequest.Currency = quote.Currency;
            fuelRequest.ExchangeRate = quote.ExchangeRate;

            fuelRequest.CreatedBy = userContext.Id;
            fuelRequest.CreatedDate = DateTimeOffset.Now;
            fuelRequest.UpdatedBy = userContext.Id;
            fuelRequest.UpdatedDate = DateTimeOffset.Now;
            fuelRequest.IsActive = true;
            fuelRequest.CreatedDate = DateTimeOffset.Now;

            ExternalPricingDomain externalPricingDomain = new ExternalPricingDomain(this);
            ExternalPricingDataViewModel externalPricingData = await externalPricingDomain.GetClosestTerminalPriceAsync(quote.QuoteRequest.JobId, fuelRequest.FuelTypeId);

            if (externalPricingData.TerminalId != 0)
            {
                CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(externalPricingDomain);
                fuelRequest.TerminalId = externalPricingData.TerminalId;
                if (quote.PricingTypeId == (int)PricingType.RackAverage)
                {
                    fuelRequest.CreationTimeRackPPG = currencyRateDomain.Convert(externalPricingData.Currency, quote.Currency, externalPricingData.AvgPrice, DateTimeOffset.Now);
                }
                else if (quote.PricingTypeId == (int)PricingType.RackLow)
                {
                    fuelRequest.CreationTimeRackPPG = currencyRateDomain.Convert(externalPricingData.Currency, quote.Currency, externalPricingData.LowPrice, DateTimeOffset.Now);
                }
                else if (quote.PricingTypeId == (int)PricingType.RackHigh)
                {
                    fuelRequest.CreationTimeRackPPG = currencyRateDomain.Convert(externalPricingData.Currency, quote.Currency, externalPricingData.HighPrice, DateTimeOffset.Now);
                }
            }

            // delivery details
            fuelRequest.FuelRequestDetail = new FuelRequestDetail();
            fuelRequest.FuelRequestDetail.DeliveryTypeId = quote.QuoteRequest.DeliveryTypeId;
            fuelRequest.FuelRequestDetail.StartDate = quote.QuoteRequest.StartDate;
            fuelRequest.FuelRequestDetail.EndDate = quote.QuoteRequest.EndDate;
            fuelRequest.FuelRequestDetail.StartTime = fuelRequest.FuelRequestDetail.EndTime = new TimeSpan(0, 0, 0);
            fuelRequest.FuelRequestDetail.DeliveryTypeId = quote.QuoteRequest.DeliveryTypeId;
            fuelRequest.FuelRequestDetail.IsDropImageRequired = helperDomain.GetDropImageRequired(quotationSupplier.CompanyId.Value);

            // fr status
            FuelRequestXStatus fuelRequestStatus = new FuelRequestXStatus();
            fuelRequestStatus.StatusId = (int)FuelRequestStatus.Open;
            fuelRequestStatus.IsActive = true;
            fuelRequestStatus.UpdatedBy = userContext.Id;
            fuelRequestStatus.UpdatedDate = DateTimeOffset.Now;
            fuelRequest.FuelRequestXStatuses.Add(fuelRequestStatus);

            // private supplier list
            quote.QuoteRequest.PrivateSupplierLists.ToList().ForEach(t => fuelRequest.PrivateSupplierLists.Add(t));

            // fees
            quote.QuotationFees.ToList().ForEach(t => fuelRequest.FuelRequestFees.Add(t));

            // dbe
            quote.QuoteRequest.MstSupplierQualifications.ToList().ForEach(t => fuelRequest.MstSupplierQualifications.Add(t));
            quote.QuoteRequest.Job.FuelRequests.Add(fuelRequest);
            var rackTypeId = quote.PricingTypeId;
            var pricingTypeId = (rackTypeId == (int)PricingType.RackHigh || rackTypeId == (int)PricingType.RackLow) ? (int)PricingType.RackAverage : rackTypeId;
            //get pricing code from api
            var codeViewModel = new PricingCodesRequestViewModel() { PricingSourceId = (int)PricingSource.Axxis, PricingTypeId = pricingTypeId, RackTypeId = rackTypeId };
            var codeList = await ContextFactory.Current.GetDomain<PricingServiceDomain>().GetPricingCodesAsync(codeViewModel);
            if (codeList.PricingCodes != null && codeList.PricingCodes.Any())
            {
                var code = codeList.PricingCodes.First();
                //PRICING SERVICE CALL
                var pricingViewModel = new FuelPricingViewModel() { PricingTypeId = pricingTypeId, PricePerGallon = quote.PricePerGallon, RackPrice = quote.PricePerGallon, Currency = quote.Currency, ExchangeRate = quote.ExchangeRate, RackAvgTypeId = quote.RackAvgTypeId, SupplierCost = quote.SupplierCost, SupplierCostTypeId = quote.SupplierCostTypeId, SupplierCostMarkupTypeId = quote.RackAvgTypeId, SupplierCostMarkupValue = quote.PricePerGallon };
                pricingViewModel.FuelPricingDetails.PricingCode.Id = code.Id;
                pricingViewModel.FuelTypeId= quote.QuoteRequest.FuelTypeId;
                pricingViewModel.TerminalId = fuelRequest.TerminalId;
                var pricingDetailId = await new PricingServiceDomain().SavePricingDetails(pricingViewModel, quote.UoM);
               
                if (pricingDetailId == null || pricingDetailId.Result == 0)
                {
                    IsPricingDetailIdNull = true;
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageFailedSaveFuelPricing;
                    return response;
                }

                if (pricingDetailId != null)
                {
                    fuelRequest.FuelRequestPricingDetail = new FuelRequestPricingDetail();
                    fuelRequest.FuelRequestPricingDetail.DisplayPrice = pricingDetailId.CustomString1;
                    fuelRequest.FuelRequestPricingDetail.DisplayPriceCode = pricingDetailId.CustomString2;
                    fuelRequest.FuelRequestPricingDetail.PricingCodeId = code.Id;
                    fuelRequest.FuelRequestPricingDetail.PricingCode = code.Code;
                    fuelRequest.FuelRequestPricingDetail.RequestPriceDetailId = pricingDetailId.Result;
                    fuelRequest.PricingTypeId = quote.PricingTypeId;

                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            Context.DataContext.FuelRequests.Add(fuelRequest);
                            var quoteRequestCurrentStatus = quote.QuoteRequest.QuoteRequestStatuses.FirstOrDefault(t => t.IsActive);
                            quoteRequestCurrentStatus.IsActive = false;
                            QuoteRequestStatus quoteRequestStatus = new QuoteRequestStatus();
                            quoteRequestStatus.QuoteRequestId = quote.QuoteRequest.Id;
                            quoteRequestStatus.StatusId = (int)QuoteRequestStatuses.Awarded;
                            quoteRequestStatus.IsActive = true;
                            quoteRequestStatus.UpdatedBy = userContext.Id;
                            quoteRequestStatus.UpdatedDate = DateTimeOffset.Now;
                            quote.QuoteRequest.QuoteRequestStatuses.Add(quoteRequestStatus);
                            await Context.CommitAsync();

                            quote.FuelRequestId = fuelRequest.Id;
                            var quoteCurrentStatus = quote.QuotationStatuses.FirstOrDefault(t => t.IsActive);
                            quoteCurrentStatus.IsActive = false;
                            QuotationStatus quotationStatus = new QuotationStatus();
                            quotationStatus.QuotationId = quote.Id;
                            quotationStatus.StatusId = (int)QuoteRequestStatuses.Awarded;
                            quotationStatus.IsActive = true;
                            quotationStatus.UpdatedBy = userContext.Id;
                            quotationStatus.UpdatedDate = DateTimeOffset.Now;
                            quote.QuotationStatuses.Add(quotationStatus);
                            await Context.CommitAsync();

                            ThirdPartyOrderViewModel tpoViewModel = new ThirdPartyOrderViewModel();
                            ThirdPartyOrderDomain tpoDomain = new ThirdPartyOrderDomain(this);
                            LogManager.Logger.WriteInfo("QuoteRequestDomain", "AwardQuote", "Awarding Quote - Calling Accept Fuel Request of TPODomain");
                            tpoViewModel.AddressDetails.IsProFormaPoEnabled = fuelRequest.Job.IsProFormaPoEnabled;
                            tpoViewModel.AddressDetails.SignatureEnabled = fuelRequest.Job.SignatureEnabled;
                            tpoViewModel.DefaultInvoiceType = (int)InvoiceType.Manual;
                            var order = await tpoDomain.AcceptFuelRequestFromTPO(userContext, fuelRequest, tpoViewModel, quotationSupplier);

                            transaction.Commit();
                            if (order != null)
                            {
                                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                                NotificationDomain notificationDomain = new NotificationDomain(this);
                                //settingsDomain.SetBuyerSupplierInformation(quote.User.Company.Id,userContext.CompanyId,string.Empty,false,OrderCreationMethod.FromQuotes,userContext);
                                await newsfeedDomain.BuyerAwardedToSupplierOnQuoteNewsfeed(fuelRequest, userContext, quote, order);
                                await notificationDomain.AddNotificationEventAsync(EventType.QuotationAwarded, quote.QuoteRequestId, userContext.Id);

                                var notAwarded = Context.DataContext.Quotations.Where(t => t.QuoteRequestId == quote.QuoteRequestId &&
                                                    t.QuotationStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)QuotationStatuses.Awarded).ToList();
                                if (notAwarded.Any())
                                {
                                    await notificationDomain.AddNotificationEventAsync(EventType.QuotationNotAwarded, quote.QuoteRequestId, userContext.Id);
                                }
                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.errMessageAwardQuoteSuccess;
                            }
                            else
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessageAwardQuoteFailed;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (IsPricingDetailIdNull)
                            {
                                response.StatusMessage = Resource.errMessageFailedSaveFuelPricing;
                            }
                            else
                            {
                                response.StatusMessage = Resource.errMessageAwardQuoteFailed;
                            }
                            response.StatusCode = Status.Failed;
                           
                            transaction.Rollback();
                            LogManager.Logger.WriteException("QuoteRequestDomain", "AwardQuote", ex.Message, ex);
                        }
                    }
                }
            }
            return response;
        }

        public async Task<StatusViewModel> DeclineQuoteRequest(int quoteRequestId, int userId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var quoteRequest = Context.DataContext.QuoteRequests.SingleOrDefault(t => t.Id == quoteRequestId && t.IsActive);
                    if (quoteRequest != null)
                    {
                        SettingsDomain settingsDomain = new SettingsDomain(this);
                        var blacklistedCompanyIds = await settingsDomain.GetBlacklistedCompanyIdsAsync(quoteRequest.User.Company.Id);
                        var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == userId && t.IsActive);
                        if (user != null && !blacklistedCompanyIds.Any(t => t == user.Company.Id))
                        {
                            quoteRequest.DeclinedUsers.Add(user);
                            await Context.CommitAsync();
                            transaction.Commit();

                            //Send response
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageDeclineQuoteRequestSuccess;
                        }
                        else
                        {
                            response.StatusMessage = Resource.errMessageDeclineQuoteRequestFailed;
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageDeclineQuoteRequestFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("QuoteRequestDomain", "DeclineQuoteRequest", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<QuotationsGridViewModel>> GetAllQuotationsAsync(int quoteRequestId)
        {
            using (var tracer = new Tracer("QuoteRequestDomain", "GetAllQuotationsAsync"))
            {
                var response = new List<QuotationsGridViewModel>();
                try
                {
                    var quotations = await Context.DataContext.Quotations.Where(t => t.QuoteRequestId == quoteRequestId).ToListAsync();
                    foreach (var item in quotations)
                    {
                        var quotation = new QuotationsGridViewModel();
                        quotation.Id = item.Id;
                        quotation.CustomerQuoteRequestId = item.QuoteRequestId;
                        quotation.SupplierName = $"{item.User.Company.Name}";
                        quotation.DeliveryFee = GetDeliveryFee(item);
                        quotation.OtherFees = GetOtherFees(item.QuotationFees);
                        quotation.RackPPG = new HelperDomain(this).GetPricePerGallon(item.PricePerGallon, item.PricingTypeId, item.RackAvgTypeId ?? 0);
                        quotation.CreatedDate = item.CreatedDate.ToString(Resource.constFormatDate);
                        quotation.CreatedBy = $"{item.User.FirstName} {item.User.LastName}";
                        quotation.Status = item.QuotationStatuses.FirstOrDefault(t => t.IsActive).MstQuoteRequestStatus.Name;
                        quotation.QuoteRequestStatusId = item.QuoteRequest.QuoteRequestStatuses.FirstOrDefault(t => t.IsActive).StatusId;
                        quotation.QuotationStatusId = item.QuotationStatuses.FirstOrDefault(t => t.IsActive).StatusId;
                        quotation.IsExcluded = item.QuotationStatuses.FirstOrDefault(t => t.IsActive).StatusId == (int)QuotationStatuses.Excluded;
                        quotation.Priority = item.Priority;
                        quotation.Documents = new List<DocumentViewModel>();
                        foreach (var doc in item.QuoteRequestDocuments.Where(t => t.IsActive))
                        {
                            var documentDetails = new DocumentViewModel();
                            documentDetails.Id = doc.Id;
                            documentDetails.AddedBy = doc.CreatedBy;
                            documentDetails.FileName = doc.FileName;
                            documentDetails.ModifiedFileName = doc.ModifiedFileName;
                            quotation.Documents.Add(documentDetails);
                        }
                        response.Add(quotation);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("QuoteRequestDomain", "GetAllQuotationsAsync", ex.Message, ex);
                }

                var orderedResponse = response.OrderBy(t => t.IsExcluded).ThenBy(t => t.Priority).ToList();
                return orderedResponse;
            }
        }

        private string GetDeliveryFee(Quotation quotation)
        {
            StringBuilder delievryFee = new StringBuilder();
            delievryFee.Append($"{Resource.lblHyphen} <br/>");
            foreach (var fee in quotation.QuotationFees)
            {
                if (fee.FeeTypeId == (int)FeeType.DeliveryFee && fee.FeeSubTypeId != (int)FeeSubType.NoFee)
                {
                    var specialDateText = string.Empty;
                    if (fee.SpecialDate.HasValue)
                        specialDateText = $"({fee.SpecialDate.Value.Date.ToShortDateString()})";
                    else if (fee.FeeConstraintTypeId.HasValue)
                        specialDateText = $"({Resource.lblWeekend})";

                    if (fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                    {
                        var feesByQuantity = fee.FeeByQuantities.ToList();
                        delievryFee.Append($"{Resource.lblByQuantity} {specialDateText} <br/>");
                        foreach (var quantityfee in feesByQuantity)
                        {
                            delievryFee.Append($"{quantityfee.MinQuantity.GetPreciseValue(2)} - {quantityfee.MaxQuantity.Value.GetPreciseValue(2)} : {Resource.constSymbolCurrency}{quantityfee.Fee.GetPreciseValue(6)} <br/>");
                        }
                    }
                    else
                    {
                        delievryFee.Append($"{fee.MstFeeSubType.Name} {specialDateText} : {Resource.constSymbolCurrency}{fee.Fee.GetPreciseValue(6)} <br/>");
                    }
                }
            }
            return delievryFee.Equals($"{Resource.lblHyphen} <br/>") ? delievryFee.ToString() : delievryFee.Replace($"{Resource.lblHyphen} <br/>", string.Empty).ToString();
        }

        private string GetOtherFees(ICollection<FuelFee> fees)
        {
            StringBuilder otherFees = new StringBuilder();
            foreach (var fee in fees)
            {
                if (fee.FeeTypeId != (int)FeeType.DeliveryFee && fee.MstFeeSubType.Id != (int)FeeSubType.NoFee)
                {
                    if (!string.IsNullOrWhiteSpace(fee.FeeDetails))
                    {
                        otherFees.Append($"{fee.FeeDetails}: {Resource.constSymbolCurrency}{fee.Fee.GetPreciseValue(6)} <br/>");
                    }
                    else if (fee.OtherFeeTypeId.HasValue)
                    {
                        otherFees.Append($"{fee.MstOtherFeeType.Name}: {Resource.constSymbolCurrency}{fee.Fee.GetPreciseValue(6)} <br/>");
                    }
                    else
                    {
                        otherFees.Append($"{fee.MstFeeType.Name}: {Resource.constSymbolCurrency}{fee.Fee.GetPreciseValue(6)} <br/>");
                    }
                }
            }
            return otherFees.Length > 0 ? otherFees.ToString() : Resource.lblHyphen;
        }

        public async Task<QuoteRequestViewModel> SaveQuoteRequestAsync(UserContext userContext, QuoteRequestViewModel viewModel)
        {
            using (var tracer = new Tracer("QuoteRequestDomain", "SaveQuoteRequestAsync"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        Job job;
                        if (viewModel.IsExistingJob)
                        {
                            job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == viewModel.Job.JobId);


                            if (job.IsRetailJob)
                            {
                                StatusViewModel status = new StatusViewModel();
                                status = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().ValidateFuelType(Convert.ToInt32(viewModel.Job.JobId), Convert.ToInt32(viewModel.FuelDetails.FuelTypeId), true, viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId);
                                if (status.StatusCode == Status.Warning)
                                {
                                    viewModel.StatusCode = Status.Failed;
                                    viewModel.StatusMessage = status.StatusMessage;
                                    return viewModel;
                                }

                            }
                        }
                        else
                        {
                            var jobViewModel = new JobStepsViewModel() { UserId = userContext.Id };

                            Geocode point = new Geocode();
                            int stateId = Convert.ToInt32(viewModel.StateId);
                            var state = Context.DataContext.MstStates.Where(t => t.Id == stateId).First();
                            if (!string.IsNullOrWhiteSpace(viewModel.ZipCode))
                            {
                                point = GoogleApiDomain.GetGeocode(viewModel.ZipCode);
                            }
                            else if (viewModel.CountryId == (int)Country.CAR && string.IsNullOrWhiteSpace(viewModel.ZipCode))
                            {                              
                                var countryCode = Context.DataContext.MstCountries.First(t => t.Id == viewModel.CountryId).Code;
                                var address = state.Name; var city = state.Name; var zipcode = state.Name;
                                point = GoogleApiDomain.GetGeocode($"{address} {city} {state.Code} {countryCode} {zipcode}");
                            }
                             
                            if (point != null)
                            {
                                var isCarribean = viewModel.CountryId == (int)Country.CAR ? true : false;
                                
                                jobViewModel.Job.IsActive = true;
                                jobViewModel.Job.Latitude = Convert.ToDecimal(point.Latitude);
                                jobViewModel.Job.Longitude = Convert.ToDecimal(point.Longitude);
                                jobViewModel.Job.Name = viewModel.NewJobName;
                                jobViewModel.Job.Address = string.IsNullOrWhiteSpace (point.Address)&& isCarribean ? (state.Name ?? Resource.lblCaribbean): point.Address;
                                jobViewModel.Job.City = string.IsNullOrWhiteSpace(point.City) && isCarribean ? (state.Name ?? Resource.lblCaribbean) : point.City;
                                jobViewModel.Job.State.Id = Convert.ToInt32(viewModel.StateId);
                                jobViewModel.Job.Country.Id = viewModel.CountryId;
                                jobViewModel.Job.ZipCode = string.IsNullOrWhiteSpace(viewModel.ZipCode) && isCarribean ? (state.Name ?? Resource.lblCaribbean) : viewModel.ZipCode;
                                jobViewModel.Job.CountyName = string.IsNullOrWhiteSpace(point.CountyName) && isCarribean ? (state.Name ?? Resource.lblCaribbean) : point.CountyName;
                                jobViewModel.Job.CreatedBy = viewModel.CreatedBy;
                                jobViewModel.Job.UpdatedBy = viewModel.CreatedBy;
                                jobViewModel.Job.StatusId = (int)JobStatus.Pending;
                                if (!string.IsNullOrWhiteSpace(viewModel.CountyName)) { jobViewModel.Job.CountyName = viewModel.CountyName; }
                                    
                                if (string.IsNullOrWhiteSpace(jobViewModel.Job.Address) ||
                                    string.IsNullOrWhiteSpace(jobViewModel.Job.City) ||
                                    jobViewModel.Job.State.Id == 0 ||
                                    jobViewModel.Job.Country.Id == 0 ||
                                    string.IsNullOrWhiteSpace(jobViewModel.Job.ZipCode) ||
                                    string.IsNullOrWhiteSpace(jobViewModel.Job.CountyName))
                                {
                                    var pointNew = GoogleApiDomain.GetAddress(Convert.ToDouble(jobViewModel.Job.Latitude), Convert.ToDouble(jobViewModel.Job.Longitude));
                                    if (pointNew != null && pointNew.Address != null)
                                    {
                                        jobViewModel.Job.Address = pointNew.Address;
                                        jobViewModel.Job.City = pointNew.City;
                                        jobViewModel.Job.ZipCode = pointNew.ZipCode;
                                        jobViewModel.Job.Country = Context.DataContext.MstCountries.Single(t => t.Name.ToLower().Contains(pointNew.CountryName.ToLower())).ToViewModel();
                                        jobViewModel.Job.State = Context.DataContext.MstStates.Single(t => t.Code.ToLower() == pointNew.StateCode.ToLower()).ToViewModel();
                                    }
                                    else
                                    {
                                        if (pointNew.Address == null)
                                        {
                                            viewModel.StatusMessage = Resource.errMessageInvalidAddressCreateJobFirst;
                                        }
                                        else
                                        {
                                            viewModel.StatusMessage = Resource.errMessageJobCreateFailedInvalidAddress;
                                        }
                                        transaction.Rollback();
                                        viewModel.StatusCode = Status.Failed;
                                        return viewModel;
                                    }
                                }

                                string timeZoneName = GoogleApiDomain.GetTimeZone(jobViewModel.Job.Latitude, jobViewModel.Job.Longitude);
                                if (!string.IsNullOrEmpty(timeZoneName))
                                {
                                    viewModel.Job.TimeZoneName = timeZoneName;
                                }
                                else
                                {
                                    transaction.Rollback();
                                    viewModel.StatusCode = Status.Failed;
                                    return viewModel;
                                }

                                jobViewModel.Job.Country.Currency = viewModel.Job.Country.Currency;
                                jobViewModel.Job.Country.UoM = viewModel.Job.Country.UoM;
                                jobViewModel.Job.TimeZoneName = viewModel.Job.TimeZoneName.ParseTimeZone();
                                var user = Context.DataContext.Users.FirstOrDefault(t => t.Id == jobViewModel.UserId);
                                var helperDomain = new HelperDomain(this);

                                var newjob = jobViewModel.Job.ToEntity();
                                newjob.CreatedByCompanyId = userContext.CompanyId;
                                newjob.JobBudget = jobViewModel.JobBudget.ToEntity(newjob.JobBudget);

                                newjob.TerminalId = helperDomain.GetClosestTerminalId(newjob.Latitude, newjob.Longitude, newjob.StateId);
                                if (newjob.TerminalId == null || newjob.TerminalId == 0)
                                {
                                    transaction.Rollback();
                                    viewModel.StatusCode = Status.Failed;
                                    viewModel.StatusMessage = Resource.errMessageJobCreateNoTerminalFound;
                                    return viewModel;
                                }
                                user.Company.Jobs.Add(newjob);

                                await Context.CommitAsync();

                                await ContextFactory.Current.GetDomain<NotificationDomain>()
                                      .AddNotificationEventAsync(EventType.JobCreated, newjob.Id, jobViewModel.UserId);
                                await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetNewJobCreationNewsFeed(userContext, newjob, false);

                                jobViewModel.Job.Id = newjob.Id;

                                job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == jobViewModel.Job.Id);
                            }
                            else
                            {
                                transaction.Rollback();
                                viewModel.StatusCode = Status.Failed;
                                viewModel.StatusMessage = Resource.errMessageJobCreateFailedInvalidAddress;
                                return viewModel;
                            }
                        }
                        viewModel.FuelDetails.FuelQuantity.UoM = job.UoM;
                        viewModel.FuelDetails.FuelPricing.Currency = job.Currency;

                        //QuoteRequests Entity
                        var quoteRequest = job.QuoteRequests.SingleOrDefault(t => t.Id == viewModel.Id);
                        //Save NonStandardProduct
                        if (viewModel.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
                        {
                            var productDomain = new ProductDomain(this);
                            var productId = await productDomain.SaveNonStandardProduct(viewModel.FuelDetails.NonStandardFuelName, userContext.Id, job.Company, viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId);
                            viewModel.FuelDetails.FuelTypeId = productId;
                        }
                        else
                        {
                            FuelRequestDomain fuelRequest = new FuelRequestDomain(this);
                            viewModel.FuelDetails.FuelTypeId = fuelRequest.GetFuelTypeId(viewModel.FuelDetails.FuelTypeId.Value, viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId, viewModel.FuelDetails.FuelPricing.PricingTypeId);
                        }

                        if (quoteRequest == null)
                        {
                            quoteRequest = viewModel.ToEntity(quoteRequest);
                            if (viewModel.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.FavoriteFuelType)
                            {
                                var nonStandardProduct = Context.DataContext.FuelRequests.Where(t => t.FuelTypeId == viewModel.FuelDetails.FuelTypeId
                                                                                && t.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
                                                                                .OrderByDescending(t => t.Id).FirstOrDefault();
                                if (nonStandardProduct != null)
                                {
                                    quoteRequest.FuelDescription = nonStandardProduct.FuelDescription;
                                }
                            }
                            job.QuoteRequests.Add(quoteRequest);
                            await Context.CommitAsync();

                            await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetQuoteRequestCreatedCanceledNewsfeed(quoteRequest.CreatedBy, quoteRequest, NewsfeedEvent.QuoteRequestCreated);
                        }

                        //SupplierQualifications Entity
                        if (viewModel.Qualifications.Count > 0)
                        {
                            var fuelRequestXSupplierQualifications = Context.DataContext.MstSupplierQualifications.Where(t => viewModel.Qualifications.Contains(t.Id)).ToList();
                            quoteRequest.MstSupplierQualifications.ToList().RemoveAll(t => t.Id > 0);
                            quoteRequest.MstSupplierQualifications = fuelRequestXSupplierQualifications;
                        }

                        if (!viewModel.PrivateSupplierList.IsPublicRequest)
                        {
                            var supplierList = Context.DataContext.PrivateSupplierLists.Where(t => viewModel.PrivateSupplierList.PrivateSupplierIds.Contains(t.Id)).ToList();
                            if (supplierList.Any())
                            {
                                quoteRequest.IsPublicRequest = false;
                                quoteRequest.PrivateSupplierLists = supplierList;
                            }
                        }
                        else
                        {
                            quoteRequest.PrivateSupplierLists.Clear();
                        }

                        await Context.CommitAsync();
                        transaction.Commit();

                        await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.QuoteRequestCreated, quoteRequest.Id, userContext.Id);

                        viewModel.StatusCode = Status.Success;
                        viewModel.Id = quoteRequest.Id;
                        viewModel.StatusMessage = Resource.errMessageCreateQuoteRequestSuccess;
                    }
                    catch (Exception ex)
                    {
                        viewModel.StatusMessage = Resource.errMessageCreateRequestFailed;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("QuoteRequestDomain", "SaveQuoteRequestAsync", ex.Message, ex);
                    }
                }
            }
            return viewModel;
        }

        public async Task UploadQuoteDocumentsToBlob(UserContext userContext, BlobContainerType quoteType, int Id, Stream fileStream, string fileName)
        {
            try
            {
                using (var tracer = new Tracer("QuoteRequestDomain", "UploadQuoteDocumentsToBlob"))
                {
                    var azureBlob = new AzureBlobStorage();
                    string modifiedFilename = string.Concat(values: quoteType.ToString() + Resource.lblSingleHyphen + Id + Resource.lblSingleHyphen + DateTime.Now.Ticks + fileName);
                    await azureBlob.SaveBlobAsync(fileStream, modifiedFilename, quoteType.ToString().ToLower());
                    await SaveFilenameToLoclDBAsync(userContext, quoteType, Id, fileName, modifiedFilename);
                    if (quoteType == BlobContainerType.QuoteRequest)
                    {
                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetQuoteRequestAttachmentUpdatedNewsfeed(userContext, fileName, Id, NewsfeedEvent.RFQAttachmentUploaded);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QuoteRequestDomain", "UploadQuoteDocumentsToBlob", ex.Message, ex);
            }
        }

        private async Task SaveFilenameToLoclDBAsync(UserContext userContext, BlobContainerType quoteType, int Id, string fileName, string modifiedFileName)
        {
            try
            {
                using (var tracer = new Tracer("QuoteRequestDomain", "SaveFilenameToLoclDBAsync"))
                {
                    var quoteDocument = new QuoteRequestDocument();
                    quoteDocument.FileName = fileName;
                    quoteDocument.ModifiedFileName = modifiedFileName;
                    quoteDocument.CreatedBy = userContext.Id;
                    if (quoteType == BlobContainerType.QuoteRequest)
                    {
                        quoteDocument.QuoteRequestId = Id;
                    }
                    else
                    {
                        quoteDocument.QuotationId = Id;
                    }
                    quoteDocument.CreatedDate = DateTimeOffset.Now;
                    quoteDocument.IsActive = true;
                    Context.DataContext.QuoteRequestDocuments.Add(quoteDocument);
                    await Context.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QuoteRequestDomain", "UploadQuoteDocumentsToBlob", ex.Message, ex);
            }
        }

        public async Task<StatusViewModel> UpdateQuoteRequestAsync(UserContext userContext, QuoteRequestDetailsViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("QuoteRequestDomain", "UpdateQuoteRequestAsync"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var quoteRequest = Context.DataContext.QuoteRequests.SingleOrDefault(t => t.Id == viewModel.Id);
                        quoteRequest.UpdatedDate = DateTimeOffset.Now;
                        quoteRequest.UpdatedBy = viewModel.UpdatedBy;

                        NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                        if (quoteRequest.QuoteDueDate != viewModel.QuoteDueDateUpdated)
                        {
                            quoteRequest.QuoteDueDate = viewModel.QuoteDueDateUpdated;
                            await newsfeedDomain.SetQuoteRequestDueDateQtyNeededUpdatedNewsfeed(quoteRequest.CreatedBy, viewModel.QuoteDueDate, quoteRequest, NewsfeedEvent.RFQDueDateModified);
                        }
                        if (quoteRequest.QuotesNeeded != viewModel.QuotesNeededUpdated)
                        {
                            quoteRequest.QuotesNeeded = viewModel.QuotesNeededUpdated;
                            await newsfeedDomain.SetQuoteRequestDueDateQtyNeededUpdatedNewsfeed(quoteRequest.CreatedBy, Convert.ToString(viewModel.QuotesNeeded), quoteRequest, NewsfeedEvent.QuotesNeededModified);
                        }
                        if (quoteRequest.Notes != viewModel.Notes)
                        {
                            quoteRequest.Notes = viewModel.Notes;
                            await newsfeedDomain.SetQuoteRequestCreatedCanceledNewsfeed(quoteRequest.CreatedBy, quoteRequest, NewsfeedEvent.RFQNoteModified);
                        }

                        Context.DataContext.Entry(quoteRequest).State = EntityState.Modified;

                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Status.Success;

                        viewModel.Id = quoteRequest.Id;
                        response.StatusMessage = Resource.errMessageUpdateQuoteRequestSuccess;
                    }
                    catch (Exception ex)
                    {
                        response.StatusMessage = Resource.errMessageCreateRequestFailed;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("QuoteRequestDomain", "SaveFuelRequestAsync", ex.Message, ex);
                    }
                }
            }
            return response;
        }

        public async Task<QuoteRequestDetailsViewModel> GetQuoteRequestDetailsAsync(int quoteRequestId, UserContext userContext)
        {
            QuoteRequestDetailsViewModel response = new QuoteRequestDetailsViewModel();
            using (var tracer = new Tracer("QuoteRequestDomain", "GetQuoteRequestDetailsAsync"))
            {
                try
                {
                    var quoteRequest = await Context.DataContext.QuoteRequests.FirstOrDefaultAsync(t => t.Id == quoteRequestId && t.IsActive);
                    response.Culture = new HelperDomain(this).SetEntityThreadCulture(quoteRequest.Currency);
                    if (quoteRequest != null)
                    {
                        response = quoteRequest.ToViewModel();
                        response.IsQuotationCreated = quoteRequest.Quotations.Any(t => t.User.Id == userContext.Id);
                        response.IsQuoteRequestDeclined = quoteRequest.DeclinedUsers.Any(t => t.Id == userContext.Id);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("QuoteRequestDomain", "GetQuoteRequestDetailsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<QuoteDetailsViewModel> GetSupplierQuoteDetailsAsync(int quoteId)
        {
            QuoteDetailsViewModel response = new QuoteDetailsViewModel();
            using (var tracer = new Tracer("QuoteRequestDomain", "GetSupplierQuoteDetailsAsync"))
            {
                try
                {
                    var quote = await Context.DataContext.Quotations.FirstOrDefaultAsync(t => t.Id == quoteId && t.IsActive);
                    if (quote != null)
                    {
                        response = quote.ToDetailsViewModel();
                        response.Culture = new HelperDomain(this).SetEntityThreadCulture(quote.QuoteRequest.Currency);
                        if (quote.FuelRequest != null && quote.FuelRequest.Orders != null && quote.FuelRequest.Orders.Any())
                        {
                            var order = quote.FuelRequest.Orders.
                                Select(t => new
                                {
                                    t.Id,
                                    t.PoNumber,
                                    t.IsActive
                                }).FirstOrDefault();
                            if (order != null)
                            {
                                response.OrderId = order.Id;
                                response.PoNumber = order.PoNumber;
                                response.IsOrderActive = order.IsActive;
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("QuoteRequestDomain", "GetSupplierQuoteDetailsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<String>> GetQuoteDocumentfilenamesAsync(int quotationId)
        {
            var filenames = new List<String>();
            using (var tracer = new Tracer("QuoteRequestDomain", "GetSupplierQuoteDetailsAsync"))
            {
                try
                {
                    var documents = await Context.DataContext.Quotations.Where(t => t.Id == quotationId && t.IsActive).Select(t => t.QuoteRequestDocuments).FirstOrDefaultAsync();
                    if (documents.Any(t => t.IsActive))
                    {
                        foreach (var document in documents)
                        {
                            filenames.Add(document.ModifiedFileName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("QuoteRequestDomain", "GetSupplierQuoteDetailsAsync", ex.Message, ex);
                }
            }
            return filenames;
        }

        public async Task<QuoteRequestViewModel> GetQuoteRequestDetailsForEditAsync(int quoteRequestId, int companyId)
        {
            QuoteRequestViewModel response = new QuoteRequestViewModel();
            using (var tracer = new Tracer("QuoteRequestDomain", "GetQuoteRequestDetailsForEditAsync"))
            {
                try
                {
                    var quoteRequest = await Context.DataContext.QuoteRequests.FirstOrDefaultAsync(t => t.Id == quoteRequestId && t.IsActive);
                    if (quoteRequest != null)
                    {
                        response = quoteRequest.ToQuoteRequestViewModel();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("QuoteRequestDomain", "GetQuoteRequestDetailsForEditAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public void AddToFavoriteFuels(int companyId, int userId, int fuelId)
        {
            if (!Context.DataContext.CompanyFavoriteFuels.Any(t => t.CompanyId == companyId
                                             && t.RemovedBy == null && t.FuelTypeId == fuelId))
            {
                var favoriteFuel = new CompanyFavoriteFuel
                {
                    FuelTypeId = 1,
                    TfxFuelTypeId = fuelId,
                    CompanyId = companyId,
                    AddedBy = userId,
                    AddedDate = DateTimeOffset.Now
                };
                Context.DataContext.CompanyFavoriteFuels.Add(favoriteFuel);
            }
        }

        public async Task<DashboardSupplierQuoteRequestGridViewModel> GetBuyerDashboardQuoteRequestAsync(UserContext userContext, int jobId, int countryId, Currency currency, string groupIds = "")
        {
            var response = new DashboardSupplierQuoteRequestGridViewModel();
            try
            {
                var quoteRequests = await GetAllQuoteRequestsBuyerAsync(userContext, null, null, jobId, QuoteRequestFilterType.All, countryId, currency, groupIds);
                if (quoteRequests != null)
                {
                    response.TotalQuoteRequestCount = quoteRequests.Count;
                    response.OpenQuoteRequestCount = quoteRequests.Count(t => t.StatusId == (int)QuoteRequestStatuses.Open);
                    response.CompletedQuoteRequestCount = quoteRequests.Count(t => t.StatusId == (int)QuoteRequestStatuses.Completed);
                    response.ExpiredQuoteRequestCount = quoteRequests.Count(t => t.StatusId == (int)QuoteRequestStatuses.Expired);
                    response.CancelledQuoteRequestCount = quoteRequests.Count(t => t.StatusId == (int)QuoteRequestStatuses.Canceled);
                    response.RecentQuoteRequests = quoteRequests.OrderByDescending(t => t.Id).Take(5).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QuoteRequestDomain", "GetBuyerDashboardQuoteRequestAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task UpdateQuoteRequestStatus(int userId, int quoteRequestId, QuoteRequestStatuses status)
        {
            var newsfeedDomain = new NewsfeedDomain(this);
            using (var tracer = new Tracer("QuoteRequestDomain", "UpdateQuoteRequestStatus"))
            {
                try
                {
                    var quoteRequest = Context.DataContext.QuoteRequests.FirstOrDefault(t => t.Id == quoteRequestId);
                    if (quoteRequest != null)
                    {
                        if (quoteRequest.QuoteRequestStatuses.Count > 0)
                        {
                            quoteRequest.QuoteRequestStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                            if (status == QuoteRequestStatuses.Canceled)
                            {
                                await newsfeedDomain.SetQuoteRequestCreatedCanceledNewsfeed(quoteRequest.CreatedBy, quoteRequest, NewsfeedEvent.QuoteRequestCanceled);
                            }
                        }
                        QuoteRequestStatus quoteRequestStatus = new QuoteRequestStatus();
                        quoteRequestStatus.QuoteRequestId = quoteRequest.Id;
                        quoteRequestStatus.StatusId = (int)status;
                        quoteRequestStatus.IsActive = true;
                        quoteRequestStatus.UpdatedBy = userId;
                        quoteRequestStatus.UpdatedDate = DateTimeOffset.Now;
                        quoteRequest.QuoteRequestStatuses.Add(quoteRequestStatus);
                        Context.DataContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("QuoteRequestDomain", "UpdateQuoteRequestStatus", ex.Message, ex);
                }
            }
        }

        public async Task RemoveDocument(UserContext userContext, int documentId)
        {
            using (var tracer = new Tracer("QuoteRequestDomain", "RemoveDocument"))
            {
                try
                {
                    var quoteDocument = Context.DataContext.QuoteRequestDocuments.FirstOrDefault(t => t.Id == documentId);
                    if (quoteDocument != null)
                    {
                        quoteDocument.IsActive = false;
                        quoteDocument.UpdatedBy = userContext.Id;
                        quoteDocument.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.SaveChanges();
                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetQuoteRequestAttachmentUpdatedNewsfeed(userContext, quoteDocument.FileName, quoteDocument.QuoteRequest.Id, NewsfeedEvent.RFQAttachmentRemoved);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("QuoteRequestDomain", "RemoveDocument", ex.Message, ex);
                }
            }
        }

        public async Task UpdateQuotesPriorityAsync(int quoteRequestId, int[] ids)
        {
            using (var tracer = new Tracer("QuoteRequestDomain", "UpdateQuotesPriorityAsync"))
            {
                try
                {
                    var quotations = await Context.DataContext.Quotations.Where(t => t.QuoteRequestId == quoteRequestId).ToListAsync();
                    var i = 1;
                    foreach (var id in ids.Where(t => t > 0))
                    {
                        quotations.FirstOrDefault(t => t.Id == id).Priority = i++;
                    }
                    Context.DataContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("QuoteRequestDomain", "UpdateQuotesPriorityAsync", ex.Message, ex);
                }
            }
        }
    }
}

