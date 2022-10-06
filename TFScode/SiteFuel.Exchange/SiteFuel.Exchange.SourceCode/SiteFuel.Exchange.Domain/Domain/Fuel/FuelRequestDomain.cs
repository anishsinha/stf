using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.FreightOnlyOrder;
using SiteFuel.Exchange.ViewModels.Queue;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SiteFuel.Exchange.Domain
{
    public class FuelRequestDomain : BaseDomain
    {
        private static List<int> goingToExpireNotifiedFRIds = new List<int>();

        public FuelRequestDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public FuelRequestDomain(BaseDomain domain) : base(domain)
        {
        }

        public FuelRequestDomain(string connectionString) : base(connectionString)
        {
        }

        public async Task<bool> IsCounterOfferExist(int fuelRequestId = 0)
        {
            var response = false;
            try
            {
                List<int> counterOffers = await ContextFactory.Current.GetDomain<HelperDomain>().GetCounterOffers(fuelRequestId);
                response = Context.DataContext.CounterOffers.Any(
                                                        m =>
                                                        counterOffers.Contains(m.FuelRequestId) &&
                                                        m.FuelRequest.IsActive
                                                    );
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "IsCounterOfferExist", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> IsSupplierCounterOfferExist(int fuelRequestId, int supplierId)
        {
            var response = false;
            try
            {
                List<int> counterOffers = await ContextFactory.Current.GetDomain<HelperDomain>().GetCounterOffers(fuelRequestId);
                response = Context.DataContext.CounterOffers.Any(
                                                        m =>
                                                        counterOffers.Contains(m.FuelRequestId) &&
                                                        m.FuelRequest.IsActive &&
                                                        m.SupplierId == supplierId
                                                    );
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "IsSupplierCounterOfferExist", ex.Message, ex);
            }
            return response;
        }

        public int GetFuelTypeId(int tfxProductId, int pricingSourceId)
        {
            var response = 0;
            try
            {
                var products = Context.DataContext.MstProducts.FirstOrDefault(t => t.TfxProductId == tfxProductId && (pricingSourceId == 0 || t.PricingSourceId == pricingSourceId));
                if (products != null)
                    response = products.Id;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetFuelTypeId", ex.Message, ex);
            }
            return response;
        }

        public int GetFuelTypeId(int tfxProductId, int pricingSourceId, int pricingType)
        {
            var response = 0;
            try
            {
                if (pricingType != (int)PricingType.Suppliercost && pricingType != (int)PricingType.PricePerGallon)
                    response = GetFuelTypeId(tfxProductId, pricingSourceId);
                else
                    response = GetFuelTypeId(tfxProductId, 0);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetFuelTypeId", ex.Message, ex);
            }
            return response;
        }

        public async Task<FuelRequestViewModel> GetFuelRequestAsync(int fuelRequestId = 0, int supplierId = 0, int userId = 0, int companyId = 0)
        {
            FuelRequestViewModel response = new FuelRequestViewModel();
            try
            {
                var fuelRequest = await Context.DataContext.FuelRequests.FirstOrDefaultAsync(t => t.Id == fuelRequestId && t.IsActive);
                if (fuelRequest != null)
                {
                    response = fuelRequest.ToViewModel();
                    if (userId > 0)
                    {
                        var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId);
                        if (user != null && user.MstRoles.Any(t => t.Id == (int)UserRoles.Buyer) && !fuelRequest.Job.Users.Any(t => t.Id == userId))
                        {
                            response.DisplayMode = PageDisplayMode.None;
                        }

                        if (response.FuelDetails.StatusId == (int)FuelRequestStatus.Accepted || response.FuelDetails.StatusId == (int)FuelRequestStatus.CounterOfferAccepted)
                        {
                            var order = await Context.DataContext.Orders.Where(t => t.FuelRequestId == fuelRequestId).
                                Select(t => new
                                {
                                    t.Id,
                                    t.PoNumber,
                                    t.IsActive
                                }).FirstOrDefaultAsync();
                            if (order != null)
                            {
                                response.OrderId = order.Id;
                                response.PoNumber = order.PoNumber;
                                response.IsOrderActive = order.IsActive;
                            }
                        }
                    }
                    if (supplierId > 0)
                    {
                        response.IsSupplierCounterOfferExists = await IsSupplierCounterOfferExist(fuelRequestId, supplierId);
                    }
                    response.IsCounterOfferExists = await IsCounterOfferExist(fuelRequestId);
                }
                setSelectedFavoriteFuelType(response, companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetFuelRequestViewModelAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveFuelRequestAsync(FuelRequestViewModel viewModel, bool isBuyer, int supplierId, UserContext userContext,bool isPortRequired=false)
        {
            StatusViewModel response = new StatusViewModel();
            bool IsPricingDetailIdNull = false;
            if (viewModel.FuelDetails.IsTierPricing
                && viewModel.FuelDetails.TierPricing.Pricings == null)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageTierPricingRequired;
                return response;
            }

            //if (viewModel.FuelDetails.FuelPricing.PricingTypeId != (int)PricingType.PricePerGallon && viewModel.FuelDetails.FuelTypeId.HasValue && viewModel.FuelDetails.FuelTypeId.Value > 0 && IsOtherFuelType(viewModel.FuelDetails.FuelTypeId.Value))
            //{
            //    response.StatusCode = Status.Failed;
            //    response.StatusMessage = Resource.errMessagePricingTypeIdInvalid;
            //    return response;
            //}

            if (viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t =>
                t.FeeSubTypeId == (int)FeeSubType.ByQuantity && !t.DeliveryFeeByQuantity.Any()))
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageFeeByQuantityRequired;
                return response;
            }
            Job job = null;
            try
            {
                if (viewModel.Job.IsMarineLocation && isPortRequired)
                {
                    var jobName = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == viewModel.Job.JobId).Name;
                    var existingJob = Context.DataContext.Jobs.
                         Where(t => t.IsActive && t.IsMarine && t.Name.Trim().ToLower() == jobName.Trim().ToLower())
                          .ToList();
                    if (!existingJob.Where(w => w.CreatedByCompanyId == userContext.CompanyId).Any())
                    {
                        JobStepsViewModel jobStepViewModel = existingJob.FirstOrDefault().ToGetJobStepViewModel();
                        jobStepViewModel.CompanyId = userContext.CompanyId;
                        jobStepViewModel.UserId = userContext.Id;
                        if (jobStepViewModel != null && jobStepViewModel.Job != null)
                        {
                            var domain = new JobDomain(this);
                            var jobSaveStatus = new StatusViewModel();
                            jobSaveStatus = await domain.SaveJobStepsAsync(userContext, jobStepViewModel, true);
                            if (jobSaveStatus.StatusCode == Status.Failed)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = jobSaveStatus.StatusMessage;
                                return response;
                            }
                            job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == jobStepViewModel.Job.Id);
                            viewModel.Job.JobId = job.Id;
                        }
                    }
                    else
                        job = existingJob.Where(w => w.CreatedByCompanyId == userContext.CompanyId).FirstOrDefault();
                    
                }
                else
                    job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == viewModel.Job.JobId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    CustomResponseModel pricingDetailId = null;
                    List<int> companiesForNotification = null;
                    HelperDomain helperDomain = new HelperDomain(this);
                    NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);                    
                    //if (job.IsRetailJob && !viewModel.IsCounterOffer)
                    //{
                    //    response = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().ValidateFuelType(Convert.ToInt32(viewModel.Job.JobId), Convert.ToInt32(viewModel.FuelDetails.FuelTypeId), true, viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId);
                    //    if (response.StatusCode == Status.Warning)
                    //    {
                    //        response.StatusCode = Status.Failed;
                    //        return response;
                    //    }

                    //}
                    if(job!=null) viewModel.FuelDetails.FuelPricing.Currency = job.Currency;

                    CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
                    if (job != null)
                        viewModel.FuelDetails.FuelPricing.ExchangeRate = currencyRateDomain.GetCurrencyRate(job.Currency, Currency.USD, DateTimeOffset.Now);
                  
                    //FuelRequests Entity
                    var fuelRequest = job?.FuelRequests?.SingleOrDefault(t => t.Id == viewModel.Id);
                    int buyerId = 0;
                    //Save NonStandardProduct
                    if (viewModel.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType && !viewModel.IsCounterOffer)
                    {
                        var productDomain = new ProductDomain(this);
                        var productId = await productDomain.SaveNonStandardProduct(viewModel.FuelDetails.NonStandardFuelName, viewModel.FuelDetails.CreatedBy, job?.Company, viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId);
                        viewModel.FuelDetails.FuelTypeId = productId;
                        viewModel.FuelDetails.FuelPricing.FuelTypeId = productId;
                    }

                    if (fuelRequest == null)
                    {
                        fuelRequest = new FuelRequest();
                        fuelRequest = viewModel.ToEntity(fuelRequest);
                        pricingDetailId= await initializeFuelPricingDetails(viewModel, fuelRequest, job);
                        if (pricingDetailId == null || pricingDetailId.Result == 0)
                        {
                            IsPricingDetailIdNull = true;
                            throw new ArgumentNullException(Resource.errMessageFailedSaveFuelPricing);
                        }

                        if (pricingDetailId != null)
                        {
                            viewModel.FuelDetails.FuelPricing.FuelPricingDetails.RequestPriceDetailId = pricingDetailId.Result;
                        }

                        if (viewModel.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.FavoriteFuelType)
                        {
                            var nonStandardProduct = Context.DataContext.FuelRequests.Where(t => t.FuelTypeId == (int)viewModel.FuelDetails.FuelTypeId
                                                                            && t.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
                                                                            .OrderByDescending(t => t.Id).FirstOrDefault();
                            if (nonStandardProduct != null)
                            {
                                fuelRequest.FuelDescription = nonStandardProduct.FuelDescription;
                            }
                        }
                        fuelRequest.FuelRequestTypeId = (int)FuelRequestType.FuelRequest;
                        job.FuelRequests.Add(fuelRequest);
                        await Context.CommitAsync();

                        await newsfeedDomain.SetFuelRequestCreatedNewsfeed(fuelRequest.CreatedBy, fuelRequest);
                    }
                    else if (viewModel.IsCounterOffer) // if counter offer then create FR
                    {
                        if (fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).StatusId != (int)FuelRequestStatus.Open)
                        {
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageSaveCounterOfferFailedFuelRequestNotOpen;
                            return response;
                        }
                        if (viewModel.IsBrokeredCounterOffer)
                        {
                            viewModel.FuelOfferDetails.SupplierQualifications = fuelRequest.MstSupplierQualifications.Select(t => t.Id).ToList();
                        }
                        var parentFuelRequestId = await helperDomain.GetFuelRequestIdFromCounterOffer(viewModel.Id);

                        var parentFuelRequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == (parentFuelRequestId == 0 ? viewModel.Id : parentFuelRequestId));
                        if (parentFuelRequest != null)
                        {
                            buyerId = parentFuelRequest.CreatedBy;
                        }

                        await newsfeedDomain.SetCounterOfferFuelRequestNewsfeed(isBuyer ? buyerId : supplierId, isBuyer, fuelRequest);
                        fuelRequest = viewModel.ToEntity();
                        //start
                       
                        pricingDetailId = await initializeFuelPricingDetails(viewModel, fuelRequest, job);
                        if (pricingDetailId == null || pricingDetailId.Result == 0)
                        {
                            IsPricingDetailIdNull = true;
                            throw new ArgumentNullException(Resource.errMessageFailedSaveFuelPricing);
                        }

                        if (pricingDetailId != null)
                        {
                            viewModel.FuelDetails.FuelPricing.FuelPricingDetails.RequestPriceDetailId = pricingDetailId.Result;
                        }
                        //end
                        fuelRequest.CreatedBy = isBuyer ? buyerId : supplierId;
                        fuelRequest.UpdatedBy = isBuyer ? buyerId : supplierId;
                        fuelRequest.CreatedDate = DateTimeOffset.Now;
                        fuelRequest.FuelRequestTypeId = (int)FuelRequestType.CounteredFuelRequest;
                        fuelRequest.Comment = viewModel.FuelDetails.Comment;
                        fuelRequest.ParentId = viewModel.FuelDeliveryDetails.FuelRequestId;
                        job.FuelRequests.Add(fuelRequest);
                        await Context.CommitAsync();

                        if (job.IsResaleEnabled && !isBuyer)
                        {
                            var resaleFuelRequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == fuelRequest.ParentId);
                            if (resaleFuelRequest != null)
                            {
                                var fuelRequestViewModel = resaleFuelRequest.ToViewModel();

                                List<Resale> resaleList = new List<Resale> { fuelRequestViewModel.ToResaleEntity() };
                                fuelRequest.Resales = resaleList;

                                if (fuelRequestViewModel.FuelRequestResale.IsDropTicketEnabled)
                                {
                                    fuelRequestViewModel.FuelRequestResale.ResaleCustomer.ForEach(t => fuelRequest.ResaleCustomers.Add(t.ToEntity()));
                                }

                                viewModel.FuelDeliveryDetails.FuelRequestFee.ResaleFee = resaleFuelRequest.FuelRequestFees.Where(t => t.FeeTypeId == (int)FeeType.ResaleFee).Select(t => t.ToResaleFeeViewModel()).ToList();
                            }
                        }
                    }
                  
                    //FuelRequestXDeliveryDetail Entity
                    viewModel.FuelDeliveryDetails.FuelRequestId = fuelRequest.Id;
                    viewModel.FuelDeliveryDetails.PricingQuantityIndicatorTypeId = (int)viewModel.FuelDetails.FuelQuantity.QuantityIndicatorTypes;
                    fuelRequest.FuelRequestDetail = viewModel.FuelDeliveryDetails.ToEntity(fuelRequest.FuelRequestDetail);

                    //// FuelRequestFee Entity - save fr fee details
                    FuelFeesDomain fuelFeesDomain = new FuelFeesDomain(this);
                    if (fuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery)
                    {
                        viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.RemoveAll(t => t.FeeConstraintTypeId.HasValue);
                    }
                    await fuelFeesDomain.SaveFuelFees(viewModel.FuelDeliveryDetails, fuelRequest, userContext);

                    //Specail Instruction Entity
                    viewModel.FuelDeliveryDetails.SpecialInstructions.ForEach(t => { if (!string.IsNullOrWhiteSpace(t.Instruction)) fuelRequest.SpecialInstructions.Add(t.ToEntity()); });

                    //Delivery schedule entity
                    if (viewModel.FuelDeliveryDetails.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries && viewModel.FuelDeliveryDetails.DeliverySchedules != null)
                    {
                        if (fuelRequest.GetParentFuelRequest().FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest) // in case of counter offer creation for a broker request don't add
                        {
                            int latestGroupNumber = 0;
                            if (Context.DataContext.DeliverySchedules.Any())
                            {
                                latestGroupNumber = Context.DataContext.DeliverySchedules.Max(t => t.GroupId);
                            }
                            var jobTime = DateTimeOffset.Now.ToTargetDateTimeOffset(job.TimeZoneName);
                            var currentDate = fuelRequest.FuelRequestDetail.StartDate < jobTime.Date ? jobTime.Date : fuelRequest.FuelRequestDetail.StartDate;
                            viewModel.FuelDeliveryDetails.DeliverySchedules.ForEach(t => t.UoM = fuelRequest.UoM);
                            foreach (var schedule in viewModel.FuelDeliveryDetails.DeliverySchedules)
                            {
                                schedule.GroupId = latestGroupNumber + 1;
                                if (schedule.ScheduleType == (int)DeliveryScheduleType.Weekly || schedule.ScheduleType == (int)DeliveryScheduleType.BiWeekly)
                                {
                                    foreach (var day in schedule.ScheduleDays)
                                    {
                                        schedule.ScheduleDay = day;
                                        int daysToAdd = ((int)(WeekDay)schedule.ScheduleDay - (int)currentDate.DayOfWeek + 7) % 7;
                                        if (daysToAdd == 0 && Convert.ToDateTime(schedule.ScheduleEndTime).TimeOfDay < jobTime.DateTime.TimeOfDay)
                                        {
                                            daysToAdd = 7;
                                        }
                                        schedule.ScheduleDate = currentDate.AddDays(daysToAdd);
                                        fuelRequest.DeliverySchedules.Add(schedule.ToEntity());
                                    }
                                }
                                else
                                {
                                    schedule.ScheduleDay = helperDomain.GetWeekDayId(schedule.ScheduleDate);
                                    fuelRequest.DeliverySchedules.Add(schedule.ToEntity());
                                }
                                latestGroupNumber++;
                            }
                        }
                        else if (fuelRequest.GetParentFuelRequest().DeliverySchedules.Any())
                        {
                            fuelRequest.DeliverySchedules = fuelRequest.GetParentFuelRequest().DeliverySchedules;
                        }
                    }

                    //PaymentDiscounts Entity
                    if (viewModel.FuelOfferDetails.PaymentDiscount.IsDiscountOnEarlyPayment)
                    {
                        List<PaymentDiscount> paymentDiscountList = new List<PaymentDiscount> { viewModel.FuelOfferDetails.PaymentDiscount.ToEntity() };
                        fuelRequest.PaymentDiscounts = paymentDiscountList;
                    }

                    //SupplierQualifications Entity
                    if (viewModel.FuelOfferDetails.SupplierQualifications.Count > 0)
                    {
                        var fuelRequestXSupplierQualifications = Context.DataContext.MstSupplierQualifications.Where(t => viewModel.FuelOfferDetails.SupplierQualifications.Contains(t.Id)).ToList();
                        fuelRequest.MstSupplierQualifications.ToList().RemoveAll(t => t.Id > 0);
                        fuelRequest.MstSupplierQualifications = fuelRequestXSupplierQualifications;
                    }

                    if (job.IsResaleEnabled && isBuyer)
                    {
                        //Resale
                        List<Resale> resaleList = new List<Resale> { viewModel.ToResaleEntity() };
                        fuelRequest.Resales = resaleList;

                        //Resale-DifferentFuelPrice
                        var resale = fuelRequest.Resales.FirstOrDefault();
                        if (resale != null)
                        {
                            AddLastTierWithMaxQuantityNull(viewModel.FuelRequestResale.DifferentFuelPrices);
                            viewModel.FuelRequestResale.DifferentFuelPrices.ForEach(t => resale.DifferentFuelPrices.Add(t.ToEntity()));
                        }

                        if (viewModel.FuelRequestResale.IsDropTicketEnabled)
                        {
                            //Resale-Customer
                            viewModel.FuelRequestResale.ResaleCustomer.ForEach(t => fuelRequest.ResaleCustomers.Add(t.ToEntity()));
                        }
                    }

                    if (viewModel.TPOSupplierId != 0)
                    {
                        List<int> privateSupplierIds = await SavePrivateSupplierListForTPOAsync(viewModel);
                        SetPrivateListToFuelRequest(privateSupplierIds, fuelRequest, out companiesForNotification);
                    }
                    else
                    {
                        if (!viewModel.FuelOfferDetails.PrivateSupplierList.IsPublicRequest)
                        {
                            SetPrivateListToFuelRequest(viewModel.FuelOfferDetails.PrivateSupplierList.PrivateSupplierIds, fuelRequest, out companiesForNotification);
                        }
                        else
                        {
                            fuelRequest.PrivateSupplierLists.Clear();
                        }
                    }

                    if (pricingDetailId == null)
                    {
                        pricingDetailId = await new PricingServiceDomain().SavePricingDetails(viewModel.FuelDetails.FuelPricing, viewModel.FuelDetails.FuelQuantity.UoM);
                        if(pricingDetailId == null)
                        {
                            IsPricingDetailIdNull = true;
                            throw new ArgumentNullException(Resource.errMessageFailedSaveFuelPricing);
                        }
                    }
                    fuelRequest.FuelRequestPricingDetail.RequestPriceDetailId = pricingDetailId.Result;
                    fuelRequest.FuelRequestPricingDetail.DisplayPrice = pricingDetailId.CustomString1;
                    fuelRequest.FuelRequestPricingDetail.DisplayPriceCode = pricingDetailId.CustomString2;
                    Context.DataContext.Entry(fuelRequest).State = EntityState.Modified;

                    if (viewModel.IsCounterOffer)
                    {
                        List<int> counterOffers = await helperDomain.GetCounterOffers(viewModel.Id);
                        if (isBuyer)
                        {
                            if (counterOffers != null && counterOffers.Any())
                            {
                                var prevCounterOfferByBuyer = Context.DataContext.CounterOffers.Where(t => counterOffers.Contains(t.FuelRequestId)
                                                                                                            && t.SupplierId == supplierId && t.BuyerId == buyerId &&
                                                                                                            t.FuelRequest.CreatedBy == supplierId).OrderByDescending(t => t.Id).FirstOrDefault();
                                if (prevCounterOfferByBuyer != null)
                                {
                                    prevCounterOfferByBuyer.BuyerStatus = (int)CounterOfferStatus.Countered;
                                    Context.DataContext.Entry(prevCounterOfferByBuyer).State = EntityState.Modified;
                                }

                                var counterOffer = new CounterOffer()
                                {
                                    BuyerId = buyerId,
                                    FuelRequestId = fuelRequest.Id,
                                    SupplierStatus = (int)CounterOfferStatus.Pending,
                                    SupplierId = supplierId,
                                    OriginalFuelRequestId = viewModel.Id
                                };
                                Context.DataContext.CounterOffers.Add(counterOffer);
                            }
                        }
                        else
                        {
                            if (counterOffers != null && counterOffers.Count > 1)
                            {
                                var prevCounterOfferByBuyer = Context.DataContext
                                                                .CounterOffers
                                                                .Where
                                                                (
                                                                    t => counterOffers.Contains(t.FuelRequestId) &&
                                                                    t.SupplierId == supplierId &&
                                                                    t.BuyerId == buyerId &&
                                                                    t.FuelRequest.CreatedBy == buyerId
                                                                )
                                                                .OrderByDescending(t => t.Id).FirstOrDefault();
                                if (prevCounterOfferByBuyer != null)
                                {
                                    prevCounterOfferByBuyer.SupplierStatus = (int)CounterOfferStatus.Countered;
                                    Context.DataContext.Entry(prevCounterOfferByBuyer).State = EntityState.Modified;
                                }
                            }

                            var counterOffer = new CounterOffer()
                            {
                                BuyerId = buyerId,
                                FuelRequestId = fuelRequest.Id,
                                BuyerStatus = (int)CounterOfferStatus.Pending,
                                SupplierId = supplierId,
                                OriginalFuelRequestId = viewModel.Id
                            };
                            Context.DataContext.CounterOffers.Add(counterOffer);
                        }
                    }

                    if (viewModel.FuelDetails.AddToFavorite && viewModel.FuelDetails.FuelTypeId != null && viewModel.FuelDetails.FuelTypeId > 0)
                    {
                        var user = Context.DataContext.Users.Single(t => t.Id == viewModel.FuelDetails.UpdatedBy);
                        AddToFavoriteFuels(user.Company.Id, user.Id, viewModel.FuelDetails.FuelTypeId ?? 0);
                    }

                    //Add vessel
                    if (viewModel.Job.IsMarineLocation && viewModel.Job.VessleId > 0)
                    {
                        var vessle = Context.DataContext.Assets.Where(t => t.Id == viewModel.Job.VessleId && t.IsMarine && t.Type == (int)AssetType.Vessle && t.IsActive)
                                            .FirstOrDefault();
                        if (vessle != null && vessle.AssetAdditionalDetail != null)
                        {
                            vessle.AssetAdditionalDetail.IMONumber = !string.IsNullOrWhiteSpace(viewModel.Job.IMONumber) ? viewModel.Job.IMONumber.Trim() : null;
                            vessle.AssetAdditionalDetail.Flag = !string.IsNullOrWhiteSpace(viewModel.Job.Flag) ? viewModel.Job.Flag.Trim() : null;
                            Context.DataContext.Entry(vessle).State = EntityState.Modified;
                            var jobxAsset = Context.DataContext.JobXAssets.Where(w => w.FuelRequestId == fuelRequest.Id).FirstOrDefault();
                            if (jobxAsset != null)
                            {
                                jobxAsset.AssetId = viewModel.Job.VessleId.Value;
                                jobxAsset.AssignedBy = fuelRequest.UpdatedBy;
                                jobxAsset.AssignedDate = fuelRequest.UpdatedDate;
                                Context.DataContext.Entry(jobxAsset).State = EntityState.Modified;
                            }
                            else
                            Context.DataContext.JobXAssets.Add(new JobXAsset() { AssetId = viewModel.Job.VessleId.Value, JobId = viewModel.Job.JobId, AssignedBy = userContext.Id, AssignedDate = DateTime.Now, FuelRequestId =fuelRequest.Id });
                        }
                        else
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errVesselNotFound;
                            return response;
                        }

                    }
                    await Context.CommitAsync();
                    transaction.Commit();

                    await UpdateNotifications(fuelRequest, companiesForNotification);

                    response.StatusCode = Status.Success;
                    if (viewModel.IsCounterOffer)
                    {
                        response.StatusMessage = Resource.errMessageCreateCounterOfferSuccess;
                    }
                    else
                    {
                        viewModel.Id = fuelRequest.Id;
                        response.StatusMessage = Resource.errMessageCreateFuelRequestSuccess;
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
                        response.StatusMessage = Resource.errMessageCreateRequestFailed;
                    }

                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelRequestDomain", "SaveFuelRequestAsync", ex.Message, ex);
                }
            }

            return response;
        }

        private bool IsOtherFuelType(int fuelTypeId)
        {
            return Context.DataContext.MstProducts.Any(t => t.Id == fuelTypeId && t.ProductTypeId == 10);
        }

        public async Task<List<int>> SavePrivateSupplierListForTPOAsync(FuelRequestViewModel viewModel)
        {
            List<int> response = new List<int>();

            try
            {
                var privateList = await Context.DataContext.PrivateSupplierLists.FirstOrDefaultAsync(t => t.CompanyId == viewModel.CompanyId
                                                                                   && t.ListType == (int)PrivateSupplierListType.TPOCreated && t.IsActive
                                                                                   && t.Companies.Select(t1 => t1.Id).Contains(viewModel.TPOSupplierId));
                if (privateList == null)
                {
                    List<int> suppliers = new List<int>();
                    suppliers.Add(viewModel.TPOSupplierId);

                    var supplierCompany = await Context.DataContext.Companies.FirstOrDefaultAsync(t => t.Id == viewModel.TPOSupplierId);

                    PrivateSupplierList privateSupplierList = new PrivateSupplierList()
                    {
                        AddedBy = viewModel.FuelDetails.CreatedBy,
                        CompanyId = viewModel.CompanyId,
                        Name = Resource.lblPrivateList + "_" + supplierCompany.Name,
                        Companies = Context.DataContext.Companies.Where(t => suppliers.Contains(t.Id)).ToList(),
                        UpdatedBy = viewModel.FuelDetails.CreatedBy,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        IsActive = true,
                        ListType = (int)PrivateSupplierListType.TPOCreated
                    };

                    Context.DataContext.PrivateSupplierLists.Add(privateSupplierList);
                    await Context.CommitAsync();

                    response.Add(privateSupplierList.Id);
                }
                else
                {
                    response.Add(privateList.Id);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "SavePrivateSupplierListForTPOAsync", ex.Message, ex);
            }

            return response;
        }

        private void AddLastTierWithMaxQuantityNull(List<DifferentFuelPriceViewModel> differentFuelPrices)
        {
            var lastTier = differentFuelPrices.LastOrDefault();
            if (lastTier != null && lastTier.MaxQuantity != null)
            {
                var newTier = new DifferentFuelPriceViewModel()
                {
                    MinQuantity = lastTier.MaxQuantity.Value + 1,
                    MaxQuantity = null,
                    PricingTypeId = lastTier.PricingTypeId,
                    RackAvgTypeId = lastTier.RackAvgTypeId,
                    PricePerGallon = lastTier.PricePerGallon
                };
                differentFuelPrices.Add(newTier);
            }
        }

        public async Task<FuelRequestDetailsViewModel> GetFuelRequestDetailsAsync(int fuelRequestId, int companyId)
        {
            FuelRequestDetailsViewModel response = new FuelRequestDetailsViewModel();

            try
            {
                var fuelRequest = await Context.DataContext.FuelRequests.FirstOrDefaultAsync(t => t.Id == fuelRequestId && t.IsActive);
                if (fuelRequest != null)
                {
                    var job = fuelRequest.Job;
                    response.FuelRequest = fuelRequest.ToViewModel();
                    response.FuelRequest.Job.IsTaxExempted = job.JobBudget.IsTaxExempted;
                    response.Supplier.Name = $"{fuelRequest.User.FirstName} {fuelRequest.User.LastName}";
                    response.Supplier.Email = fuelRequest.User.Email;
                    response.Supplier.PhoneNumber = fuelRequest.User.PhoneNumber;
                    response.ContactPersons = job.Users1.Select(t => new ContactPersonViewModel()
                    {
                        Id = t.Id,
                        Name = $"{t.FirstName} {t.LastName}",
                        Email = t.Email,
                        PhoneNumber = t.PhoneNumber
                    }).ToList();

                    if(job.IsMarine)
                    {
                        var ackId = await Context.DataContext.Acknowledgements.Where(t => t.IsActive && t.IsSent && t.EntityId == fuelRequest.Id)
                                                                               .Select(t => t.Id)
                                                                               .FirstOrDefaultAsync();
                        if(ackId > 0)
                        {
                            response.FuelRequest.Job.AcknowledgementId = ackId;
                        }
                    }

                    if (response.FuelRequest.FuelDetails.StatusId == (int)FuelRequestStatus.Accepted || response.FuelRequest.FuelDetails.StatusId == (int)FuelRequestStatus.CounterOfferAccepted)
                    {
                        var acceptedFrIdInOrder = fuelRequestId;

                        var counterOffers = await new HelperDomain(this).GetCounterOffersWithStatus(fuelRequestId);
                        var acceptedCO = counterOffers.SingleOrDefault(t => t.Item2 == (int)CounterOfferStatus.Accepted || t.Item3 == (int)CounterOfferStatus.Accepted);
                        if (acceptedCO != null)
                        {
                            acceptedFrIdInOrder = acceptedCO.Item1;
                        }

                        var order = await Context.DataContext.Orders.Where(t => t.FuelRequestId == acceptedFrIdInOrder).
                                Select(t => new
                                {
                                    t.Id,
                                    t.PoNumber,
                                    t.IsActive
                                }).FirstOrDefaultAsync();
                        if (order != null)
                        {
                            response.FuelRequest.OrderId = order.Id;
                            response.FuelRequest.PoNumber = order.PoNumber;
                            response.FuelRequest.IsOrderActive = order.IsActive;
                        }
                    }


                    var fuelDeliveryDetails = await Context.DataContext.FuelRequestDetails.Where(t => t.FuelRequestId == fuelRequest.Id).
                            Select(t => new
                            {
                                t.FuelRequestId,
                                t.IsPrePostDipRequired,
                                t.OrderEnforcementId
                            }).FirstOrDefaultAsync();
                    if (fuelDeliveryDetails != null)
                    {
                        response.FuelDeliveryDetails.OrderEnforcementId = fuelDeliveryDetails.OrderEnforcementId;
                        response.FuelDeliveryDetails.IsPrePostDipRequired = fuelDeliveryDetails.IsPrePostDipRequired;
                    }

                    response.User = new UserViewModel()
                    {
                        Id = fuelRequest.User.Id,
                        IsOnboardingComplete = fuelRequest.User.IsOnboardingComplete,
                        OnboardedTypeId = fuelRequest.User.OnboardedTypeId,
                    };

                    //// get fuel request status
                    response.FuelRequest.FuelDetails.StatusName = GetFuelRequestStatusName(fuelRequest, companyId);
                    if (response.FuelRequest.FuelDetails.StatusName == ApplicationConstants.Declined)
                    {
                        response.FuelRequest.FuelDetails.StatusId = (int)FuelRequestStatus.Declined;
                    }
                    else if (response.FuelRequest.FuelDetails.StatusName == ApplicationConstants.Missed)
                    {
                        response.FuelRequest.FuelDetails.StatusId = (int)FuelRequestStatus.Missed;
                    }

                    HelperDomain helperDomain = new HelperDomain(this);
                    response.FuelRequest.FuelDeliveryDetails.DeliverySchedules = helperDomain.GetUndeliveredSchedules(response.FuelRequest.FuelDeliveryDetails.DeliverySchedules);

                    var fuelRequestOrder = fuelRequest.Orders.LastOrDefault();
                    if (fuelRequestOrder != null && fuelRequestOrder.ExternalBrokerId.HasValue)
                    {
                        response.ExternalBrokerId = fuelRequestOrder.ExternalBrokerId ?? 0;
                        response.BrokeredOrder = fuelRequestOrder.ExternalBrokerOrderDetail.ToViewModel();
                        response.BrokeredOrder.BrokeredOrderFee = fuelRequestOrder.FuelRequest.FuelRequestFees.ToExternalBrokerViewModel();
                        response.BrokeredOrder.BrokeredOrderFee.Currency = job.Currency;
                        response.BrokeredOrder.BrokeredOrderFee.UoM = job.UoM;
                    }
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.errMessageSuccess;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetFuelRequestDetailsAsync", ex.Message, ex);
            }

            return response;
        }

        private string GetFuelRequestStatusName(FuelRequest fuelRequest, int companyId)
        {
            string StatusName;
            var frStatus = fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive);

            if (fuelRequest.Users.Any(t => t.Company.Id == companyId))
            {
                StatusName = ApplicationConstants.Declined;
            }
            else if ((fuelRequest.Orders.Any(t => t.AcceptedCompanyId != companyId) && frStatus.StatusId == (int)FuelRequestStatus.Accepted) || frStatus.StatusId == (int)FuelRequestStatus.CounterOfferAccepted)
            {
                StatusName = ApplicationConstants.Missed;
            }
            else
            {
                StatusName = frStatus.MstFuelRequestStatus.Name;
            }

            return StatusName;
        }

        public async Task<StatusViewModel> UpdateFuelRequestAsync(FuelRequestViewModel viewModel, UserContext userContext,bool isPortRequired=false)
        {
            StatusViewModel response = new StatusViewModel();
            bool IsPricingDetailIdNull = false;

            if (viewModel.FuelDetails.IsTierPricing
                && viewModel.FuelDetails.TierPricing.Pricings == null)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageTierPricingRequired;
                return response;
            }

            if (viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t =>
                t.FeeSubTypeId == (int)FeeSubType.ByQuantity && !t.DeliveryFeeByQuantity.Any()))
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageFeeByQuantityRequired;
                return response;
            }
            Job job = null;
            try
            {
                if (viewModel.Job.IsMarineLocation && isPortRequired)
                {
                    var jobName = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == viewModel.Job.JobId).Name;
                    var existingJob = Context.DataContext.Jobs.
                         Where(t => t.IsActive && t.IsMarine && t.Name.Trim().ToLower() == jobName.Trim().ToLower())
                          .ToList();
                    if (!existingJob.Where(w => w.CreatedByCompanyId == userContext.CompanyId).Any())
                    {
                        JobStepsViewModel jobStepViewModel = existingJob.FirstOrDefault().ToGetJobStepViewModel();
                        jobStepViewModel.CompanyId = userContext.CompanyId;
                        jobStepViewModel.UserId = userContext.Id;
                        if (jobStepViewModel != null && jobStepViewModel.Job != null)
                        {
                            var domain = new JobDomain(this);
                            var jobSaveStatus = new StatusViewModel();
                            jobSaveStatus = await domain.SaveJobStepsAsync(userContext, jobStepViewModel, true);
                            if (jobSaveStatus.StatusCode == Status.Failed)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = jobSaveStatus.StatusMessage;
                                return response;
                            }
                            job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == jobStepViewModel.Job.Id);
                            viewModel.Job.JobId = job.Id;
                        }
                    }
                    else
                        job = existingJob.Where(w => w.CreatedByCompanyId == userContext.CompanyId).FirstOrDefault();

                }
                else
                    job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == viewModel.Job.JobId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    HelperDomain helperDomain = new HelperDomain(this);                    
                    List<int> companiesForNotification = null;
                    //FuelRequests Entity
                    var fuelRequest = Context.DataContext.FuelRequests.FirstOrDefault(t => t.Id == viewModel.Id);
                    if (fuelRequest != null && fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).StatusId != (int)FuelRequestStatus.Open)
                    {
                        //var job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == viewModel.Job.JobId);
                        job.FuelRequests.Add(fuelRequest);

                        viewModel.FuelDetails.FuelPricing.Currency = job.Currency;
                        viewModel.FuelDetails.FuelQuantity.UoM = job.UoM;

                        //Save NonStandardProduct
                        if (viewModel.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
                        {
                            var productDomain = new ProductDomain(this);
                            var productId = await productDomain.SaveNonStandardProduct(viewModel.FuelDetails.NonStandardFuelName, viewModel.FuelDetails.CreatedBy, job.Company, viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId);
                            viewModel.FuelDetails.FuelTypeId = productId;
                        }

                        CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
                        viewModel.FuelDetails.FuelPricing.ExchangeRate = currencyRateDomain.GetCurrencyRate(job.Currency, Currency.USD, DateTimeOffset.Now);

                        if (!viewModel.FuelDetails.IsTierPricing &&  await new PricingServiceDomain().FuelCostCompare(fuelRequest, viewModel))
                        {
                            fuelRequest = viewModel.ToEntity(fuelRequest);
                            goto skipPricingDetailId;
                        }
                        //initialize Tier Pricing Details
                        //start
                        fuelRequest = viewModel.ToEntity(fuelRequest);
                        var pricingDetailId = await initializeFuelPricingDetails(viewModel, fuelRequest, job);
                        if (pricingDetailId == null || pricingDetailId.Result == 0)
                        {
                            IsPricingDetailIdNull = true;
                            throw new ArgumentNullException(Resource.errMessageFailedSaveFuelPricing);
                        }

                        if (pricingDetailId != null)
                        {
                            viewModel.FuelDetails.FuelPricing.FuelPricingDetails.RequestPriceDetailId = pricingDetailId.Result;
                        }
                        //end

                        fuelRequest.FuelRequestPricingDetail.RequestPriceDetailId = pricingDetailId.Result;
                        fuelRequest.FuelRequestPricingDetail.DisplayPrice = pricingDetailId.CustomString1;
                        fuelRequest.FuelRequestPricingDetail.DisplayPriceCode = pricingDetailId.CustomString2;

                        skipPricingDetailId:

                        if (viewModel.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.FavoriteFuelType)
                        {
                            var nonStandardProduct = Context.DataContext.FuelRequests.Where(t => t.FuelTypeId == (int)viewModel.FuelDetails.FuelTypeId
                                                                            && t.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
                                                                            .OrderByDescending(t => t.Id).FirstOrDefault();
                            if (nonStandardProduct != null)
                            {
                                fuelRequest.FuelDescription = nonStandardProduct.FuelDescription;
                            }
                        }

                        Context.DataContext.DifferentFuelPrices.RemoveRange(fuelRequest.DifferentFuelPrices);
                        AddLastTierWithMaxQuantityNull(viewModel.FuelDetails.DifferentFuelPrices);
                        viewModel.FuelDetails.DifferentFuelPrices.ForEach(t => fuelRequest.DifferentFuelPrices.Add(t.ToEntity()));

                        //FuelRequestXDeliveryDetail Entity
                        viewModel.FuelDeliveryDetails.PricingQuantityIndicatorTypeId = (int)viewModel.FuelDetails.FuelQuantity.QuantityIndicatorTypes;
                        //end

                        fuelRequest.FuelRequestDetail = viewModel.FuelDeliveryDetails.ToEntity(fuelRequest.FuelRequestDetail);

                        // FuelRequestFee Entity
                        Context.DataContext.FuelRequestFees.RemoveRange(fuelRequest.FuelRequestFees);

                        // FuelRequestFee Entity - save fr fee details
                        FuelFeesDomain fuelFeesDomain = new FuelFeesDomain(this);
                        await fuelFeesDomain.SaveFuelFees(viewModel.FuelDeliveryDetails, fuelRequest, userContext);

                        //Specail Instruction Entity
                        Context.DataContext.SpecialInstructions.RemoveRange(fuelRequest.SpecialInstructions);
                        viewModel.FuelDeliveryDetails.SpecialInstructions.ForEach(t => { if (!string.IsNullOrWhiteSpace(t.Instruction)) fuelRequest.SpecialInstructions.Add(t.ToEntity()); });

                        //DeliverySchedule Entity
                        Context.DataContext.DeliverySchedules.RemoveRange(fuelRequest.DeliverySchedules);
                        if (viewModel.FuelDeliveryDetails.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries && viewModel.FuelDeliveryDetails.DeliverySchedules != null)
                        {
                          
                            int latestGroupNumber = 0;
                            if (Context.DataContext.DeliverySchedules.Any())
                            {
                                latestGroupNumber = Context.DataContext.DeliverySchedules.Max(t => t.GroupId);
                            }
                            var jobTime = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName);
                            var currentDate = fuelRequest.FuelRequestDetail.StartDate < jobTime.Date ? jobTime.Date : fuelRequest.FuelRequestDetail.StartDate;
                            viewModel.FuelDeliveryDetails.DeliverySchedules.ForEach(t => t.UoM = fuelRequest.UoM);
                            foreach (var schedule in viewModel.FuelDeliveryDetails.DeliverySchedules)
                            {
                                schedule.GroupId = latestGroupNumber + 1;
                                if (schedule.ScheduleType == (int)DeliveryScheduleType.Weekly || schedule.ScheduleType == (int)DeliveryScheduleType.BiWeekly)
                                {
                                    foreach (var day in schedule.ScheduleDays)
                                    {
                                        schedule.ScheduleDay = day;
                                        int daysToAdd = ((int)(WeekDay)schedule.ScheduleDay - (int)currentDate.DayOfWeek + 7) % 7;
                                        if (daysToAdd == 0 && Convert.ToDateTime(schedule.ScheduleEndTime).TimeOfDay < jobTime.DateTime.TimeOfDay)
                                        {
                                            daysToAdd = 7;
                                        }
                                        schedule.ScheduleDate = currentDate.AddDays(daysToAdd);
                                        fuelRequest.DeliverySchedules.Add(schedule.ToEntity());
                                    }
                                }
                                else
                                {
                                    schedule.ScheduleDay = helperDomain.GetWeekDayId(schedule.ScheduleDate);
                                    fuelRequest.DeliverySchedules.Add(schedule.ToEntity());
                                }
                                latestGroupNumber++;
                            }
                        }

                        //PaymentDiscounts Entity
                        Context.DataContext.PaymentDiscounts.RemoveRange(fuelRequest.PaymentDiscounts);
                        if (viewModel.FuelOfferDetails.PaymentDiscount.IsDiscountOnEarlyPayment)
                        {
                            List<PaymentDiscount> paymentDiscountList = new List<PaymentDiscount> { viewModel.FuelOfferDetails.PaymentDiscount.ToEntity() };
                            fuelRequest.PaymentDiscounts = paymentDiscountList;
                        }

                        //SupplierQualifications Entity
                        if (viewModel.FuelOfferDetails.SupplierQualifications.Count > 0)
                        {
                            var fuelRequestXSupplierQualifications = Context.DataContext.MstSupplierQualifications.Where(t => viewModel.FuelOfferDetails.SupplierQualifications.Contains(t.Id)).ToList();
                            fuelRequest.MstSupplierQualifications.ToList().RemoveAll(t => t.Id > 0);
                            fuelRequest.MstSupplierQualifications = fuelRequestXSupplierQualifications;
                        }

                        //Resale-DifferentFuelPrice
                        var resale = fuelRequest.Resales.FirstOrDefault();
                        if (resale != null)
                        {
                            Context.DataContext.DifferentFuelPrices.RemoveRange(resale.DifferentFuelPrices);
                        }

                        //Resale
                        Context.DataContext.Resales.RemoveRange(fuelRequest.Resales);
                        List<Resale> resaleList = new List<Resale> { viewModel.ToResaleEntity() };
                        fuelRequest.Resales = resaleList;

                        //Resale-DifferentFuelPrice
                        resale = fuelRequest.Resales.FirstOrDefault();
                        if (resale != null)
                        {
                            AddLastTierWithMaxQuantityNull(viewModel.FuelRequestResale.DifferentFuelPrices);
                            viewModel.FuelRequestResale.DifferentFuelPrices.ForEach(t => resale.DifferentFuelPrices.Add(t.ToEntity()));
                        }

                        //Resale-Customer
                        Context.DataContext.ResaleCustomers.RemoveRange(fuelRequest.ResaleCustomers);
                        if (viewModel.FuelRequestResale.IsDropTicketEnabled)
                        {
                            viewModel.FuelRequestResale.ResaleCustomer.ForEach(t => fuelRequest.ResaleCustomers.Add(t.ToEntity()));
                        }

                        if (!viewModel.FuelOfferDetails.PrivateSupplierList.IsPublicRequest)
                        {
                            SetPrivateListToFuelRequest(viewModel.FuelOfferDetails.PrivateSupplierList.PrivateSupplierIds, fuelRequest, out companiesForNotification);
                        }
                        else
                        {
                            fuelRequest.PrivateSupplierLists.Clear();
                        }


                        Context.DataContext.Entry(fuelRequest).State = EntityState.Modified;
                    }
                    else if (fuelRequest != null && fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).StatusId == (int)FuelRequestStatus.Open)
                    {
                        if (!viewModel.FuelOfferDetails.PrivateSupplierList.IsPublicRequest)
                        {
                            SetPrivateListToFuelRequest(viewModel.FuelOfferDetails.PrivateSupplierList.PrivateSupplierIds, fuelRequest, out companiesForNotification);
                        }
                        else
                        {
                            fuelRequest.IsPublicRequest = true;
                            fuelRequest.PrivateSupplierLists.Clear();
                        }
                    }

                    if (viewModel.FuelDetails.AddToFavorite && viewModel.FuelDetails.FuelTypeId != null && viewModel.FuelDetails.FuelTypeId > 0)
                    {
                        var user = Context.DataContext.Users.Single(t => t.Id == viewModel.FuelDetails.UpdatedBy);
                        AddToFavoriteFuels(user.Company.Id, user.Id, viewModel.FuelDetails.FuelTypeId ?? 0);
                    }

                    //add vessel
                    if (viewModel.Job.IsMarineLocation && viewModel.Job.VessleId > 0)
                    {
                        var vessle = Context.DataContext.Assets.Where(t => t.Id == viewModel.Job.VessleId && t.IsMarine && t.Type == (int)AssetType.Vessle && t.IsActive)
                                            .FirstOrDefault();
                        if (vessle != null && vessle.AssetAdditionalDetail != null)
                        {
                            vessle.AssetAdditionalDetail.IMONumber = !string.IsNullOrWhiteSpace(viewModel.Job.IMONumber) ? viewModel.Job.IMONumber.Trim() : null;
                            vessle.AssetAdditionalDetail.Flag = !string.IsNullOrWhiteSpace(viewModel.Job.Flag) ? viewModel.Job.Flag.Trim() : null;
                            Context.DataContext.Entry(vessle).State = EntityState.Modified;
                            var jobxAsset = Context.DataContext.JobXAssets.Where(w => w.FuelRequestId == fuelRequest.Id).FirstOrDefault();
                            if (jobxAsset != null)
                            {
                                jobxAsset.AssetId = viewModel.Job.VessleId.Value;
                                jobxAsset.AssignedBy = fuelRequest.UpdatedBy;
                                jobxAsset.AssignedDate = fuelRequest.UpdatedDate;
                                Context.DataContext.Entry(jobxAsset).State = EntityState.Modified;
                            }
                            else
                                Context.DataContext.JobXAssets.Add(new JobXAsset() { AssetId = viewModel.Job.VessleId.Value, JobId = viewModel.Job.JobId, AssignedBy = userContext.Id, AssignedDate = DateTime.Now, FuelRequestId = fuelRequest.Id });
                        }
                        else
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errVesselNotFound;
                            return response;
                        }
                    }
                        await Context.CommitAsync();
                    transaction.Commit();

                    //Add an entry to notifications
                    if (fuelRequest.FuelRequestTypeId == (int)FuelRequestType.FuelRequest &&
                        fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).StatusId != (int)FuelRequestStatus.Draft)
                    {
                        await ContextFactory.Current.GetDomain<NotificationDomain>()
                                                    .AddNotificationEventAsync(
                                                        EventType.FuelRequestCreated,
                                                        fuelRequest.Id,
                                                        fuelRequest.CreatedBy,
                                                        companiesForNotification);
                        viewModel.Id = fuelRequest.Id;
                    }

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageUpdateFuelRequestSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageUpdateFuelRequestFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelRequestDomain", "UpdateFuelRequestAsync", ex.Message, ex);
                }
            }

            return response;
        }

        private void SetPrivateListToFuelRequest(IList<int> privateSupplierIds, FuelRequest fuelRequest, out List<int> companiesForNotification)
        {

            companiesForNotification = new List<int>();
            if (privateSupplierIds != null)
            {
                //existing supplier list
                var existingSupplierList = fuelRequest.PrivateSupplierLists.ToList();
                var existingCompanies = existingSupplierList.SelectMany(t => t.Companies.Select(t1 => t1.Id)).ToList();

                var supplierList = Context.DataContext.PrivateSupplierLists.Where(t => privateSupplierIds.Contains(t.Id)).ToList();
                if (supplierList != null)
                {
                    fuelRequest.IsPublicRequest = false;
                    fuelRequest.PrivateSupplierLists = supplierList;
                    var currentSupplierlist = supplierList.SelectMany(t => t.Companies.Select(t1 => t1.Id)).ToList();
                    if (existingSupplierList.Count > 0)
                        companiesForNotification = currentSupplierlist.Except(existingCompanies).ToList();
                    else
                        companiesForNotification = currentSupplierlist;
                }
            }
        }

        public async Task<DashboardFuelRequestViewModel> GetBuyerDashboardFuelRequestAsync(int companyId, int userId, int jobId = 0, int countryId = (int)Country.All, int currency = (int)Currency.None, string groupIds = "")
        {
            var response = new DashboardFuelRequestViewModel();
            try
            {
                var filter = new FuelRequestFilterViewModel { JobId = jobId };
                var fuelRequests = await GetBuyerFuelRequestGridAsync(companyId, userId, null, filter, countryId, currency, groupIds);
                if (fuelRequests != null)
                {
                    response.TotalFuelRequestCount = fuelRequests.Count;
                    response.DraftFuelRequestCount = fuelRequests.Count(t => t.StatusId == (int)FuelRequestStatus.Draft);
                    response.OpenFuelRequestCount = fuelRequests.Count(t => t.StatusId == (int)FuelRequestStatus.Open);
                    response.AcceptedFuelRequestCount = fuelRequests.Count(t => t.StatusId == (int)FuelRequestStatus.Accepted);
                    response.ExpiredFuelRequestCount = fuelRequests.Count(t => t.StatusId == (int)FuelRequestStatus.Expired);
                    response.CancelledFuelRequestCount = fuelRequests.Count(t => t.StatusId == (int)FuelRequestStatus.Canceled);
                    response.RecentFuelRequests = fuelRequests.OrderByDescending(t => t.FuelRequestId).Take(5).ToList();
                    var openFuelRequest = response.RecentFuelRequests.Where(t => t.StatusId == (int)FuelRequestStatus.Open);
                    var otherStatusFuelRequest = response.RecentFuelRequests.Where(t => t.StatusId != (int)FuelRequestStatus.Open);
                    response.RecentFuelRequests = openFuelRequest.Union(otherStatusFuelRequest).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetBuyerDashboardFuelRequestAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<FuelRequestGridViewModel>> GetBuyerFuelRequestGridAsync(int companyId, int userId, DataTableSearchModel statusSearchTypes, FuelRequestFilterViewModel filter, int countryId = (int)Country.All, int currency = (int)Currency.None, string groupIds = "")
        {
            var response = new List<FuelRequestGridViewModel>();
            try
            {
                List<CounterOfferGridViewModel> counterOffers = new List<CounterOfferGridViewModel>();
                DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);

                if (!string.IsNullOrEmpty(filter.StartDate))
                {
                    StartDate = Convert.ToDateTime(filter.StartDate).Date;
                }
                if (!string.IsNullOrEmpty(filter.EndDate))
                {
                    EndDate = Convert.ToDateTime(filter.EndDate).Date.AddDays(1);
                }

                if (filter.JobId > 0)
                {
                    var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.Id == filter.JobId);
                    if (job != null)
                    {
                        currency = (int)job.Currency;
                        countryId = job.CountryId;
                    }
                }

                var helperDomain = new HelperDomain(this);
                var groupIdslist = helperDomain.GetGroupList(groupIds);

                var blacklistCompanyIds = await new SettingsDomain(helperDomain).GetBlacklistedCompanyIdsAsync(companyId, groupIdslist);
                var allCounterOffers = Context.DataContext.CounterOffers.Where
                                    (
                                        t =>
                                        t.FuelRequest.IsActive &&
                                        ((groupIdslist.Count == 0 && t.User.Company.Id == companyId) || (groupIdslist.Count > 0 && t.User.Company.SubCompanies.Any(t1 => t1.SubCompanyId == t.User.Company.Id && groupIdslist.Contains(t1.CompanyGroupId)))) &&
                                        !blacklistCompanyIds.Contains(t.User.Company.Id) &&
                                        !blacklistCompanyIds.Contains(t.User1.Company.Id)
                                    );

                if (filter != null && filter.JobId > 0)
                {
                    allCounterOffers = allCounterOffers.Where(t => t.FuelRequest.Job.Id == filter.JobId);
                }

                await allCounterOffers.OrderByDescending(t => t.Id).ForEachAsync(t => counterOffers.Add(new CounterOfferGridViewModel(Status.Success)
                {
                    FuelRequestId = t.OriginalFuelRequestId,
                    SupplierId = t.SupplierId,
                    BuyerStatus = t.BuyerStatus,
                    SupplierStatus = t.SupplierStatus
                }));

                var latestCounterOffers = from offer in counterOffers
                                          group offer by new { offer.FuelRequestId, offer.SupplierId } into grp
                                          select grp.First();
                counterOffers = latestCounterOffers.ToList();

                var jobId = filter == null ? 0 : filter.JobId;
                var storedProcedureDomain = new StoredProcedureDomain(helperDomain);
                var fuelRequestStat = new USP_BuyerFRRequestViewModel()
                {
                    CompanyId = companyId,
                    Broadcast = (int)filter.BrodcastType,
                    UserId = userId,
                    dataTableSearchValues = statusSearchTypes,
                    JobId = jobId,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    CountryId = countryId,
                    CurrencyType = currency,
                    GroupIds = groupIds,
                    StatusId = filter == null ? 0 : (int)filter.Filter
                };
                var fuelRequests = await storedProcedureDomain.GetBuyerFuelRequestGrid(fuelRequestStat);
                foreach (var item in fuelRequests)
                {
                    var fuelRequest = new FuelRequestGridViewModel();
                    fuelRequest.FuelRequestId = item.FuelRequestId;
                    fuelRequest.RequestNumber = item.RequestNumber;
                    fuelRequest.StartDate = item.CreatedDate.Date.ToString(Resource.constFormatDate);
                    fuelRequest.JobName = item.JobName;
                    fuelRequest.JobId = item.JobId;
                    fuelRequest.Address = $"{item.Address}, {item.City}, {item.StateCode} {item.ZipCode}";                    
                    fuelRequest.PricePerGallon = item.PricePerGallon;
                    fuelRequest.FuelType = item.FuelType;
                    fuelRequest.ContactPerson = item.ContactName;
                    fuelRequest.IsCounterOfferAvailable = item.IsCounterOfferAvailable;
                    fuelRequest.IsCounterOfferPendingOnBuyer = counterOffers.Any(t => t.FuelRequestId == item.FuelRequestId && t.BuyerStatus == (int)CounterOfferStatus.Pending);
                    fuelRequest.IsCounterOfferPendingOnSupplier = counterOffers.Any(t => t.FuelRequestId == item.FuelRequestId && t.SupplierStatus == (int)CounterOfferStatus.Pending);
                    fuelRequest.StatusId = item.StatusId;
                    fuelRequest.Status = item.Status;
                    fuelRequest.TotalCount = item.TotalCount;
                    fuelRequest.DeliveryType = item.DeliveryType;
                    fuelRequest.UoM = item.UoM;
                    fuelRequest.AcceptedCompanyId = item.AcceptedCompanyId;
                    var fuelQuantity = helperDomain.GetQuantityRequested(item.Gallons);
                    fuelRequest.GallonsNeeded = item.Gallons != ApplicationConstants.QuantityNotSpecified ? $"{fuelQuantity} {item.UoM.GetDisplayName()}" : fuelQuantity;
                    response.Add(fuelRequest);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetFuelRequestGridAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<MapViewModel>> GetMap(int userId)
        {
            var response = new List<MapViewModel>();
            try
            {
                var helperDomain = new HelperDomain(this);
                var jobIds = await helperDomain.GetJobIdsAsync(userId);
                if (jobIds != null)
                {
                    var jobs = Context.DataContext.Jobs.Where(t => jobIds.Contains(t.Id) && t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open);
                    await jobs.ForEachAsync(t => response.AddRange(t.FuelRequests.Where(y => y.IsActive).Select(x => x.ToMapViewModel()).ToList()));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetMap", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<PendingRequestViewModel>> GetPendingRequest(int jobId)
        {
            var response = new List<PendingRequestViewModel>();
            try
            {
                var allFuelRequest = await Context.DataContext.FuelRequests.Include(t => t.Job)
                                                                .Include(t => t.MstProduct).Include(t => t.FuelRequestDetail)
                                                                .Where(t => t.IsActive
                                                                        && t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Open
                                                                        && t.FuelRequestTypeId == (int)FuelRequestType.FuelRequest
                                                                        && t.Job.IsActive && t.Job.Id == jobId)
                                                                .ToListAsync();

                response = allFuelRequest.Select(t => t.ToPendingRequestViewModel()).OrderByDescending(t => t.FuelRequestId).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetPendingRequest", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CancelFuelRequestAsync(int fuelRequestId, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var fuelRequest = await Context.DataContext.FuelRequests.FirstOrDefaultAsync(t => t.Id == fuelRequestId);
                    if (fuelRequest != null)
                    {
                        fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                        FuelRequestXStatus frStatus = new FuelRequestXStatus();
                        frStatus.StatusId = (int)FuelRequestStatus.Canceled;
                        frStatus.IsActive = true;
                        frStatus.UpdatedBy = (int)SystemUser.System;
                        frStatus.UpdatedDate = DateTimeOffset.Now;
                        fuelRequest.FuelRequestXStatuses.Add(frStatus);

                        Context.DataContext.Entry(fuelRequest).State = EntityState.Modified;

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageCancelFuelRequestSuccess;

                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetFuelRequestCanceledNewsfeed(userContext, fuelRequest);

                        //Close the ZTR Loop
                        HelperDomain helperDomain = new HelperDomain(this);
                        helperDomain.CloseZTRFuelRequestLoop(fuelRequest.Id);
                    }

                    await Context.CommitAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageCancelFuelRequestFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelRequestDomain", "CancelFuelRequestAsync", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> DeleteFuelRequestAsync(int fuelRequestId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var fuelRequest = await Context.DataContext.FuelRequests.FirstOrDefaultAsync(t => t.Id == fuelRequestId);
                    if (fuelRequest != null)
                    {
                        fuelRequest.IsActive = false;
                        Context.DataContext.Entry(fuelRequest).State = EntityState.Modified;

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageDeleteFuelRequestSuccess;
                    }

                    await Context.CommitAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageDeleteFuelRequestFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelRequestDomain", "DeleteFuelRequestAsync", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<FuelRequestViewModel> GetLastFuelRequestAsync(int jobId, int companyId, int userId, int fuelTypeId,
            int fuelDisplayGroupId, int truckLoadTypeId, int pricingSourceId, int pricingTypeId, string pricingCode, int pricingCodeId, string pricingCodeDesc, bool isMarineLocation = false)
        {
            FuelRequestViewModel response = new FuelRequestViewModel(Status.Success);
            try
            {
                if (jobId == 0)
                {
                    MasterDomain masterDomain = new MasterDomain(this);
                    var firstJob = masterDomain.GetJobs(userId).FirstOrDefault();
                    if (firstJob != null && firstJob.Id > 0)
                    {
                        jobId = firstJob.Id;
                    }
                }
                var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.Id == jobId && t.IsMarine == isMarineLocation);
                if (job != null)
                {
                    var fuelRequest = job.FuelRequests.Where(t => t.IsActive && (fuelTypeId == 0 || t.FuelTypeId == fuelTypeId) && t.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest)
                                                            .OrderByDescending(t => t.Id).FirstOrDefault();
                    if (fuelRequest != null)
                    {
                        response = fuelRequest.ToViewModel();
                        if (fuelRequest.Job.IsResaleEnabled)
                            response.FuelRequestResale.ResaleCustomer = fuelRequest.Job.ResaleCustomers.Select(t => t.ToViewModel()).ToList();
                        response.ExternalPoNumber = string.Empty;
                        response.Id = 0;
                        response.FuelDeliveryDetails.FuelRequestId = 0;
                        response.FuelDetails.StatusId = (int)FuelRequestStatus.Draft;
                        response.FuelDeliveryDetails.DeliverySchedules.ForEach(t =>
                            {
                                t.DriverName = string.Empty; t.Id = 0; t.GroupId = 0;
                            });
                    }
                    else
                    {
                        if (fuelTypeId > 0)
                        {
                            response.FuelDetails.FuelTypeId = fuelTypeId;
                            if (fuelDisplayGroupId == (int)ProductDisplayGroups.FuelTypesInYourArea)
                            {
                                var products = await Context.DataContext.MstProducts.FirstOrDefaultAsync(t => t.Id == fuelTypeId);
                                if (products != null)
                                {
                                    response.FuelDetails.FuelDisplayGroupId = products.ProductDisplayGroupId;
                                }
                            }
                            else
                            {
                                response.FuelDetails.FuelDisplayGroupId = fuelDisplayGroupId;
                            }
                        }
                        response.FuelDetails.FuelPricing.Currency = job.Currency;
                        response.FuelDetails.FuelQuantity.UoM = job.UoM;
                    }

                    response.Job = new JobSelectionViewModel(Status.Success) { JobId = job.Id, Name = job.Name, IsMarineLocation = job.IsMarine };
                    response.Job.Country = new CountryViewModel(Status.Success) { Id = job.CountryId, Name = job.CountyName };
                    response.JobCountryId = job.CountryId;
                    response.Job.State = new StateViewModel(Status.Success) { QuantityIndicatorTypeId = job.MstState.QuantityIndicatorTypeId };
                    response.FuelDetails.FuelDisplayJobId = job.Id;
                    response.FuelDetails.FuelPricing.CityGroupTerminalStateId = job.StateId;

                    if (truckLoadTypeId > 0)
                    {
                        response.FuelDeliveryDetails.TruckLoadTypes = (TruckLoadTypes)truckLoadTypeId;
                    }
                    if (pricingCodeId > 0)
                    {
                        response.FuelDetails.FuelPricing.FuelPricingDetails.PricingCode = new PricingCodeDetailViewModel { Id = pricingCodeId, Code = pricingCode, Description = pricingCodeDesc };
                    }
                    if (pricingTypeId > 0)
                    {
                        response.FuelDetails.FuelPricing.PricingTypeId = pricingTypeId;
                    }
                    response.FuelDeliveryDetails.FuelFees.Currency = job.Currency;
                    response.FuelDeliveryDetails.FuelFees.UoM = job.UoM;
                    response.FuelDeliveryDetails.CustomAttributeViewModel.WBSNumber = string.Empty;
                    response.FuelDeliveryDetails.CustomAttribute = string.Empty;
                    response.FuelRequestResale.FuelPricing.Currency = job.Currency;
                    response.FuelDeliveryDetails.FuelFees.FuelRequestFees.RemoveAll(t => t.FeeTypeId.Equals(((int)FeeType.ProcessingFee).ToString()));
                    if (IsOtherFuelType(response.FuelDetails.FuelTypeId ?? 0))
                        response.FuelDetails.IsOtherFuelTypeInFavorite = true;
                }
                setSelectedFavoriteFuelType(response, companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetLastCreatedFuelRequestAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<JobSelectionViewModel> GetSelectedJobDatesAsync(int jobId)
        {
            JobSelectionViewModel response = new JobSelectionViewModel(Status.Success);
            try
            {
                var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.Id == jobId);
                if (job != null)
                {
                    response.JobStartDate = Convert.ToString(job.StartDate.Date);
                    response.JobEndDate = job.EndDate != null ? job.EndDate.Value.Date.ToString(Resource.constFormatDate) : string.Empty;
                    response.IsAssetTracked = job.JobBudget.IsAssetTracked;
                    response.IsResaleEnabled = job.IsResaleEnabled;
                    response.IsProFormaPoEnabled = job.IsProFormaPoEnabled;
                    response.LocationType = (int)job.LocationType;
                    var resale = job.ResaleCustomers.FirstOrDefault();
                    if (resale != null)
                    {
                        response.CustomerName = resale.Name;
                        response.CustomerEmail = resale.Email;
                    }

                    response.JobLocationCurrentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(job.TimeZoneName).DateTime.ToString();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetSelectedJobDatesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<FuelRequestViewModel> GetFuelRequestCounterOfferAsync(int fuelReqeustId, int supplierId, int companyId)
        {
            FuelRequestViewModel response = new FuelRequestViewModel();
            try
            {
                //FuelRequests Entity
                HelperDomain helperDomain = new HelperDomain(this);
                var blacklistedCompanyIds = await ContextFactory.Current.GetDomain<SettingsDomain>().GetBlacklistedCompanyIdsAsync(companyId);
                var fuelRequest = await Context.DataContext.FuelRequests.FirstOrDefaultAsync(t => t.Id == fuelReqeustId && t.IsActive);
                int originalFuelRequest = helperDomain.GetFuelRequestFromCounterOffer(fuelReqeustId);
                if (fuelRequest != null && !blacklistedCompanyIds.Any(t => t == fuelRequest.User.Company.Id))
                {
                    response = fuelRequest.ToViewModel();
                    response.FuelDeliveryDetails.DeliverySchedules.ForEach(t => t.DriverName = null);
                    response.IsCounterOffer = true; // counter offer flag
                    response.Id = originalFuelRequest; // set id of the main fuel request here
                    response.CounterOfferSupplierId = supplierId;
                    FuelRequest originalRequest = Context.DataContext.FuelRequests.FirstOrDefault(t => t.Id == originalFuelRequest);
                    if (originalRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest)
                    {
                        response.IsBrokeredCounterOffer = true;
                    }
                    if (IsOtherFuelType(response.FuelDetails.FuelTypeId ?? 0))
                    {
                        response.FuelDetails.FuelDisplayGroupId = (int)ProductDisplayGroups.OtherFuelType;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetLastCreatedFuelRequestAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<int> GetJobIdAsync(int fuelRequestId)
        {
            int response = 0;
            try
            {
                var fuelRequest = await Context.DataContext.FuelRequests.FirstOrDefaultAsync(t => t.Id == fuelRequestId);
                if (fuelRequest != null)
                {
                    response = fuelRequest.Job.Id;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetJobId", ex.Message, ex);
            }
            return response;
        }

        public async Task<int> GetFuelRequestIdAsync(int orderId)
        {
            int response = 0;
            try
            {
                var orders = await Context.DataContext.Orders.FirstOrDefaultAsync(t => t.Id == orderId);
                if (orders != null)
                {
                    response = orders.FuelRequestId;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetFuelRequestIdAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<CloneRequestViewModel> GetCloneRequestAsync(int fuelRequestId = 0)
        {
            CloneRequestViewModel response = new CloneRequestViewModel();
            try
            {
                var fuelRequest = await Context.DataContext.FuelRequests.FirstOrDefaultAsync(t => t.Id == fuelRequestId && t.IsActive);
                if (fuelRequest != null)
                {
                    response = fuelRequest.ToCloneRequestViewModel();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetCloneRequestAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveCloneFuelRequestAsync(CloneRequestViewModel viewModel, int userId, bool isBrokeredFuelRequest = false)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var fuelRequest = await Context.DataContext.FuelRequests.FirstOrDefaultAsync(t => t.Id == viewModel.Id && t.IsActive);
                    if (fuelRequest != null)
                    {
                        FuelRequest cloneFuelRequest = await CreateFRClone(viewModel, userId, fuelRequest);
                        if (isBrokeredFuelRequest)
                        {
                            cloneFuelRequest.FuelRequestTypeId = (int)FuelRequestType.BrokeredFuelRequest;
                            cloneFuelRequest.ParentId = fuelRequest.GetParentId();
                        }

                        Context.DataContext.Entry(cloneFuelRequest).State = EntityState.Modified;
                        await Context.CommitAsync();

                        transaction.Commit();

                        NotificationDomain notificationDomain = new NotificationDomain(this);
                        await notificationDomain.AddNotificationEventAsync(
                                                        EventType.FuelRequestCreated,
                                                        cloneFuelRequest.Id,
                                                        cloneFuelRequest.CreatedBy);
                        viewModel.Id = cloneFuelRequest.Id;
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageCloneFuelReuestSuccess;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelRequestDomain", "SaveCloneFuelRequestAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<FuelRequest> CreateFRClone(CloneRequestViewModel viewModel, int userId, FuelRequest fuelRequest)
        {
            FuelRequestViewModel fuelRequestViewModel = fuelRequest.ToViewModel();

            var job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == fuelRequestViewModel.Job.JobId);

            ////FuelRequests Entity
            var cloneFuelRequest = new FuelRequest();
            fuelRequestViewModel.Id = 0;
            fuelRequestViewModel.FuelDetails.UpdatedBy = userId;
            fuelRequestViewModel.FuelDetails.CreatedBy = userId;
            fuelRequestViewModel.FuelDetails.CreatedDate = DateTimeOffset.Now;
            fuelRequestViewModel.ExternalPoNumber = viewModel.ExternalPoNumber;
            if (fuelRequestViewModel.FuelDetails.FuelQuantity.QuantityTypeId == (int)QuantityType.SpecificAmount)
            {
                fuelRequestViewModel.FuelDetails.FuelQuantity.Quantity = viewModel.Quantity;
            }
            else if (fuelRequestViewModel.FuelDetails.FuelQuantity.QuantityTypeId == (int)QuantityType.Range)
            {
                fuelRequestViewModel.FuelDetails.FuelQuantity.MaximumQuantity = viewModel.Quantity;
            }
            else if (fuelRequestViewModel.FuelDetails.FuelQuantity.QuantityTypeId == (int)QuantityType.NotSpecified)
            {
                fuelRequestViewModel.FuelDetails.FuelQuantity.MaximumQuantity = ApplicationConstants.QuantityNotSpecified;
            }
            fuelRequestViewModel.FuelDeliveryDetails.ExpirationDate = viewModel.ExpirationDate;
            fuelRequestViewModel.FuelDetails.StatusId = (int)FuelRequestStatus.Open;
            fuelRequestViewModel.FuelDetails.FuelPricing.SupplierCost = null;
            cloneFuelRequest = fuelRequestViewModel.ToEntity(cloneFuelRequest);
            if (cloneFuelRequest.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
            {
                cloneFuelRequest.FuelTypeId = fuelRequestViewModel.FuelDetails.FuelTypeId.Value;
            }
            else
            {

                cloneFuelRequest.FuelTypeId = GetFuelTypeId(fuelRequestViewModel.FuelDetails.FuelTypeId.Value, fuelRequestViewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId, fuelRequestViewModel.FuelDetails.FuelPricing.PricingTypeId);
            }
            cloneFuelRequest.FuelRequestPricingDetail.DisplayPrice = fuelRequest.FuelRequestPricingDetail.DisplayPrice;
            cloneFuelRequest.FuelRequestPricingDetail.DisplayPriceCode = fuelRequest.FuelRequestPricingDetail.DisplayPriceCode;
            //ExternalPricingDomain externalPricingDomain = new ExternalPricingDomain(this);
            //await externalPricingDomain.GetTerminalWithPrice(fuelRequestViewModel.FuelDetails.FuelPricing.FuelPricingDetails, cloneFuelRequest, job.Id, cloneFuelRequest.Currency);

            cloneFuelRequest.FuelRequestTypeId = (int)FuelRequestType.FuelRequest;
            job.FuelRequests.Add(cloneFuelRequest);
            await Context.CommitAsync();

            //DifferentFuelPrice Entity
            fuelRequestViewModel.FuelDetails.DifferentFuelPrices.ForEach(t => cloneFuelRequest.DifferentFuelPrices.Add(t.ToEntity()));

            //FuelRequestXDeliveryDetail Entity
            fuelRequestViewModel.FuelDeliveryDetails.FuelRequestId = cloneFuelRequest.Id;
            fuelRequestViewModel.FuelDeliveryDetails.StartDate = viewModel.StartDate;
            fuelRequestViewModel.FuelDeliveryDetails.EndDate = viewModel.EndDate;
            fuelRequestViewModel.FuelDeliveryDetails.StartTime = viewModel.StartTime;
            fuelRequestViewModel.FuelDeliveryDetails.EndTime = viewModel.EndTime;
            cloneFuelRequest.FuelRequestDetail = fuelRequestViewModel.FuelDeliveryDetails.ToEntity(cloneFuelRequest.FuelRequestDetail);

            //FuelRequestFee Entity
            fuelRequestViewModel.FuelDeliveryDetails.FuelRequestFee.FuelRequestId = cloneFuelRequest.Id;
            cloneFuelRequest.FuelRequestFees = fuelRequestViewModel.FuelDeliveryDetails.FuelFees.ToEntity();

            //PaymentDiscounts Entity
            if (fuelRequestViewModel.FuelOfferDetails.PaymentDiscount.IsDiscountOnEarlyPayment)
            {
                List<PaymentDiscount> paymentDiscountList = new List<PaymentDiscount> { fuelRequestViewModel.FuelOfferDetails.PaymentDiscount.ToEntity() };
                cloneFuelRequest.PaymentDiscounts = paymentDiscountList;
            }

            //Specail Instruction Entity
            fuelRequestViewModel.FuelDeliveryDetails.SpecialInstructions.ForEach(t => { if (!string.IsNullOrWhiteSpace(t.Instruction)) cloneFuelRequest.SpecialInstructions.Add(t.ToEntity()); });
            if (fuelRequestViewModel.FuelDeliveryDetails.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries && fuelRequestViewModel.FuelDeliveryDetails.DeliverySchedules != null)
            {
                HelperDomain helperDomain = new HelperDomain(this);
                int latestGroupNumber = 0;
                if (Context.DataContext.DeliverySchedules.Any())
                {
                    latestGroupNumber = Context.DataContext.DeliverySchedules.Max(t => t.GroupId);
                }
                var jobCurrentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(job.TimeZoneName);
                var currentDate = cloneFuelRequest.FuelRequestDetail.StartDate < jobCurrentDate.Date ? jobCurrentDate.Date : cloneFuelRequest.FuelRequestDetail.StartDate;
                foreach (var schedule in fuelRequestViewModel.FuelDeliveryDetails.DeliverySchedules)
                {
                    schedule.StatusId = (int)DeliveryScheduleStatus.New;
                    schedule.GroupId = latestGroupNumber + 1;
                    if (schedule.ScheduleType == (int)DeliveryScheduleType.Weekly || schedule.ScheduleType == (int)DeliveryScheduleType.BiWeekly)
                    {
                        foreach (var day in schedule.ScheduleDays)
                        {
                            schedule.ScheduleDay = day;
                            int daysToAdd = ((int)(WeekDay)schedule.ScheduleDay - (int)currentDate.DayOfWeek + 7) % 7;
                            if (daysToAdd == 0 && Convert.ToDateTime(schedule.ScheduleEndTime).TimeOfDay < jobCurrentDate.DateTime.TimeOfDay)
                            {
                                daysToAdd = 7;
                            }
                            schedule.ScheduleDate = currentDate.AddDays(daysToAdd);
                            cloneFuelRequest.DeliverySchedules.Add(schedule.ToEntity());
                        }
                    }
                    else if (schedule.ScheduleType == (int)DeliveryScheduleType.SpecificDates)
                    {
                        var scheduleEndTime = Convert.ToDateTime(schedule.ScheduleEndTime).TimeOfDay;
                        if (schedule.ScheduleDate.Add(scheduleEndTime) >= jobCurrentDate.DateTime)
                        {
                            schedule.ScheduleDay = helperDomain.GetWeekDayId(schedule.ScheduleDate);
                            cloneFuelRequest.DeliverySchedules.Add(schedule.ToEntity());
                        }
                    }
                    else if (schedule.ScheduleType == (int)DeliveryScheduleType.Monthly)
                    {
                        var scheduleEndTime = Convert.ToDateTime(schedule.ScheduleEndTime).TimeOfDay;
                        if (schedule.ScheduleDate.Add(scheduleEndTime).DateTime < jobCurrentDate.DateTime)
                        {
                            int daysToAdd = (jobCurrentDate.Subtract(schedule.ScheduleDate).Days) % 30;
                            if (daysToAdd > 0)
                            {
                                schedule.ScheduleDate = currentDate.AddDays(30 - daysToAdd);
                            }
                        }
                        schedule.ScheduleDay = helperDomain.GetWeekDayId(schedule.ScheduleDate);
                        cloneFuelRequest.DeliverySchedules.Add(schedule.ToEntity());
                    }
                    latestGroupNumber++;
                }
            }

            //SupplierQualifications Entity
            if (fuelRequestViewModel.FuelOfferDetails.SupplierQualifications.Count > 0)
            {
                var fuelRequestXSupplierQualifications = Context.DataContext.MstSupplierQualifications.Where(t => fuelRequestViewModel.FuelOfferDetails.SupplierQualifications.Contains(t.Id)).ToList();
                cloneFuelRequest.MstSupplierQualifications.ToList().RemoveAll(t => t.Id > 0);
                cloneFuelRequest.MstSupplierQualifications = fuelRequestXSupplierQualifications;
            }

            if (fuelRequestViewModel.FuelOfferDetails.PrivateSupplierList.PrivateSupplierIds != null)
            {
                var supplierList = Context.DataContext.PrivateSupplierLists.Where(t => fuelRequestViewModel.FuelOfferDetails.PrivateSupplierList.PrivateSupplierIds.Contains(t.Id)).ToList();
                if (supplierList != null)
                {
                    cloneFuelRequest.PrivateSupplierLists = supplierList;
                }
            }

            if (job.IsResaleEnabled)
            {
                //Resale
                List<Resale> resaleList = new List<Resale> { fuelRequestViewModel.ToResaleEntity() };
                cloneFuelRequest.Resales = resaleList;

                //Resale-DifferentFuelPrice
                var resale = cloneFuelRequest.Resales.FirstOrDefault();
                if (resale != null)
                {
                    AddLastTierWithMaxQuantityNull(fuelRequestViewModel.FuelRequestResale.DifferentFuelPrices);
                    fuelRequestViewModel.FuelRequestResale.DifferentFuelPrices.ForEach(t => resale.DifferentFuelPrices.Add(t.ToEntity()));
                }

                if (fuelRequestViewModel.FuelRequestResale.IsDropTicketEnabled)
                {
                    //Resale-Customer
                    fuelRequestViewModel.FuelRequestResale.ResaleCustomer.ForEach(t => cloneFuelRequest.ResaleCustomers.Add(t.ToEntity()));
                }
            }

            return cloneFuelRequest;
        }

        public async Task<int> SavePrivateSupplierListAsync(int companyId, string name, List<int> suppliers, int userId)
        {
            int response = 0;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var company = await Context.DataContext.Companies.FirstOrDefaultAsync(t => t.Id == companyId && t.IsActive);
                    if (company != null)
                    {
                        PrivateSupplierList privateSupplierList = new PrivateSupplierList()
                        {
                            AddedBy = userId,
                            CompanyId = companyId,
                            Name = name,
                            Companies = Context.DataContext.Companies.Where(t => suppliers.Contains(t.Id)).ToList(),
                            UpdatedBy = userId,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true
                        };

                        Context.DataContext.PrivateSupplierLists.Add(privateSupplierList);
                        await Context.CommitAsync();
                        transaction.Commit();

                        response = privateSupplierList.Id;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelRequestDomain", "SavePrivateSupplierListAsync", ex.Message, ex);
                }
            }
            return response;
        }

        private int GetCounterOfferSupplierStatus(FuelRequest fuelRequest, int supplierId)
        {
            int statusId = 0;
            try
            {
                var counteredFuelRequest = fuelRequest.FuelRequests1.OrderByDescending(t => t.Id).FirstOrDefault(t => t.IsActive && t.CounterOffers.FirstOrDefault().SupplierId == supplierId);
                if (counteredFuelRequest == null)
                {
                    return statusId;
                }
                else if (counteredFuelRequest.CounterOffers != null && counteredFuelRequest.CounterOffers.Any())
                {
                    if (counteredFuelRequest.CounterOffers.FirstOrDefault().SupplierStatus == (int)CounterOfferStatus.Pending)
                    {
                        statusId = counteredFuelRequest.CounterOffers.FirstOrDefault().SupplierStatus.Value;
                        return statusId;
                    }
                    return GetCounterOfferSupplierStatus(counteredFuelRequest, supplierId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetCounterOfferSupplierStatus", ex.Message, ex);
            }
            return statusId;
        }

        public async Task<List<FuelRequestGridViewModel>> GetSupplierFuelReqestGridAsync(USP_SupplierRequestsViewModel fuelRequestStat, string fromDate = "", string toDate = "")
        {
            using (var tracer = new Tracer("FuelRequestDomain", "GetSupplierFuelReqestGridAsync"))
            {
                var response = new List<FuelRequestGridViewModel>();
                try
                {
                    var storedProcedureDomain = new StoredProcedureDomain(this);
                    var helperDomain = new HelperDomain(this);
                    DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                    DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                    if (!string.IsNullOrEmpty(fromDate))
                    {
                        StartDate = Convert.ToDateTime(fromDate).Date;
                    }
                    if (!string.IsNullOrEmpty(toDate))
                    {
                        EndDate = Convert.ToDateTime(toDate).Date.AddDays(1);
                    }

                    fuelRequestStat.StartDate = StartDate;
                    fuelRequestStat.EndDate = EndDate;
                    var fuelRequests = await storedProcedureDomain.GetSupplierFuelRequestGrid(fuelRequestStat);

                    foreach (var item in fuelRequests)
                    {
                        var fuelRequest = new FuelRequestGridViewModel(Status.Success);
                        fuelRequest.FuelRequestId = item.FuelRequestId;
                        fuelRequest.Customer = item.Customer;
                        fuelRequest.Address = $"{item.City}, {item.State} {item.ZipCode}";
                        fuelRequest.StateAndZip = $"{item.State}, {item.ZipCode}";
                        fuelRequest.FuelType = item.FuelType;
                        fuelRequest.FuelTypeId = item.FuelTypeId;                        
                        fuelRequest.PricePerGallon = item.PricePerGallon;
                        fuelRequest.StartDate = item.StartDate.ToString(Resource.constFormatDate);
                        fuelRequest.RequestNumber = item.RequestNumber;
                        fuelRequest.IsCounterOfferAvailable = item.IsCounterOfferAvailable;
                        fuelRequest.IsCounterOfferDeclinedByBuyer = item.IsCounterOfferDeclinedByBuyer;
                        fuelRequest.Distance = item.Distance.ToString(ApplicationConstants.IntegerFormat) + " " + item.UoD;
                        fuelRequest.Status = item.StatusName;
                        fuelRequest.TotalGallonsDeliveredTillNow = item.DeliveredTillNow;
                        fuelRequest.TotalCount = item.TotalCount;
                        fuelRequest.FrTotalDollarValue = item.FrTotalDollarValue;
                        fuelRequest.IsCounterOfferPendingOnBuyer = item.CounterOfferBuyerStatus == (int)CounterOfferStatus.Pending || item.CounterOfferSupplierStatus == (int)CounterOfferStatus.Countered;
                        fuelRequest.IsCounterOfferPendingOnSupplier = item.CounterOfferSupplierStatus == (int)CounterOfferStatus.Pending || item.CounterOfferBuyerStatus == (int)CounterOfferStatus.Countered;
                        fuelRequest.DeliveryType = item.DeliveryType;
                        fuelRequest.IsOnboardingComplete = item.IsOnboardingComplete;
                        fuelRequest.OnboardedTypeId = item.OnboardedTypeId;
                        fuelRequest.IsMarineLocation = item.IsMarineLocation;
                        fuelRequest.AcknowledgementId = item.AcknowledgementId;
                        var fuelQuantity = helperDomain.GetQuantityRequested(item.GallonsNeeded);
                        fuelRequest.GallonsNeeded = item.GallonsNeeded != ApplicationConstants.QuantityNotSpecified ? $"{fuelQuantity} {item.UoM.GetDisplayName()}" : fuelQuantity;

                        response.Add(fuelRequest);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("FuelRequestDomain", "GetSupplierFuelReqestGridAsync", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<StatusViewModel> DeclineFuelRequest(int fuelRequestId, int userId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var fuelRequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == fuelRequestId && t.IsActive);
                    if (fuelRequest != null)
                    {
                        var blacklistedCompanyIds = await ContextFactory.Current.GetDomain<SettingsDomain>().GetBlacklistedCompanyIdsAsync(fuelRequest.User.Company.Id);
                        var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == userId && t.IsActive);
                        if (user != null && !blacklistedCompanyIds.Any(t => t == user.Company.Id))
                        {
                            fuelRequest.Users.Add(user);
                            await Context.CommitAsync();
                            transaction.Commit();

                            //Send response
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageDeclineFuelRequestSuccess;
                        }
                        else
                        {
                            response.StatusMessage = Resource.errMessageDeclineFuelRequestFailed;
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageDeclineFuelRequestFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelRequestDomain", "DeclineFuelRequest", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<FuelRequestStatusViewModel> AcceptFuelRequest(UserContext userContext, int fuelRequestId, SourceRegionsViewModel sourceRegionModel = null, bool isAutoAccept = false,int? existingOrderPreferenceId = null) 
        {
            FuelRequestStatusViewModel response = new FuelRequestStatusViewModel();
            var helperDomain = new HelperDomain(this);
            var notificationDomain = new NotificationDomain(this);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var fuelRequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == fuelRequestId && t.IsActive);
                    if (fuelRequest != null)
                    {
                        var settingsDomain = new SettingsDomain(this);
                        var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == userContext.Id);
                        var blacklistedCompanyIds = await settingsDomain.GetBlacklistedCompanyIdsAsync(fuelRequest.User.CompanyId ?? 0);
                        if (blacklistedCompanyIds.Any(t => t == userContext.CompanyId))
                        {
                            response.StatusMessage = Resource.errMessageAcceptFuelRequestFailed;
                            return response;
                        }

                        //request status filter
                        if (fuelRequest.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)FuelRequestStatus.Open)
                        {
                            response.StatusMessage = Resource.errMessageAcceptFuelRequestFailedFuelRequestNotOpen;
                            return response;
                        }
                       
                        var sc_resposnse = await SetSuppliercostToFuelRequest(userContext, fuelRequest);
                        if (sc_resposnse.StatusCode == Status.Failed)
                        {
                            response.StatusCode = sc_resposnse.StatusCode;
                            response.StatusMessage = sc_resposnse.StatusMessage;
                            return response;
                        }

                        if (fuelRequest.FuelRequestTypeId != (int)FuelRequestType.FreightOnlyRequest)
                        {
                            bool isSupplierQualified = false;
                            //supplier qualification check
                            foreach (var address in user.Company.CompanyAddresses)
                            {
                                if (!fuelRequest.MstSupplierQualifications.Except(address.MstSupplierQualifications).Any())
                                {
                                    isSupplierQualified = true;
                                    break;
                                }
                            }
                            if (!isSupplierQualified)
                            {
                                response.StatusMessage = Resource.errMessageUserNotQualifiedForFuelRequest;
                                return response;
                            }

                            if (!fuelRequest.IsPublicRequest)
                            {
                                isSupplierQualified = false;

                                foreach (var item in fuelRequest.PrivateSupplierLists)
                                {
                                    foreach (var company in item.Companies)
                                    {
                                        if (company.Id == userContext.CompanyId)
                                        {
                                            isSupplierQualified = true;
                                            break;
                                        }
                                    }
                                    if (isSupplierQualified)
                                        break;
                                }
                                if (!isSupplierQualified)
                                {
                                    response.StatusMessage = Resource.errMessageUserNotQualifiedForFuelRequest;
                                    return response;
                                }
                            }
                        }

                        //check if product is available in region for specific location
                        //check if product is available in region for specific location
                        if (fuelRequest.FuelTypeId > 0 && fuelRequest.JobId > 0)
                        {
                            var tpOrderDomain = new ThirdPartyOrderDomain(this);
                            var isProductValid = await tpOrderDomain.IsValidProductForRegion(fuelRequest.JobId, userContext.CompanyId, null, fuelRequest.FuelTypeId, 0);
                            if (isProductValid.StatusCode == Status.Failed)
                            {
                                response.StatusCode = isProductValid.StatusCode;
                                response.StatusMessage = isProductValid.StatusMessage;
                                return response;
                            }
                        }
                        //update fuelrequest status
                        SetFuelRequestStatusToAccepted(userContext, fuelRequest);

                        //disable override of IsDropImageRequired flag (in case of broke order)
                        //fuelRequest.FuelRequestDetail.IsDropImageRequired = helperDomain.GetDropImageRequired(userContext.CompanyId);

                        //get Order Level Terminal FromFR terminal details
                        int orderTerminalId = GetOrderLevelTerminalFromFR(fuelRequest);

                        #region new source region case to assigned terminal
                        if (sourceRegionModel != null && sourceRegionModel.FreightPricingMethod == FreightPricingMethod.Auto)
                        {
                            orderTerminalId = await SetTerminalFromSourceRegion(sourceRegionModel, userContext.CompanyId);
                        }
                        #endregion

                        //create order
                        Order order = new Order
                        {
                            PoNumber = ApplicationConstants.PoNumberPrefix,
                            IsProFormaPo = fuelRequest.Job.IsProFormaPoEnabled,
                            SignatureEnabled = fuelRequest.Job.SignatureEnabled,
                            AcceptedCompanyId = userContext.CompanyId,
                            AcceptedBy = userContext.Id,
                            AcceptedDate = DateTimeOffset.Now,
                            TerminalId = orderTerminalId > 0 ? orderTerminalId : fuelRequest.TerminalId,
                            BuyerCompanyId = fuelRequest.User.Company.Id,
                            IsActive = true,
                            UpdatedBy = userContext.Id,
                            UpdatedDate = DateTimeOffset.Now,
                            DefaultInvoiceType = isAutoAccept ? (int)InvoiceType.DigitalDropTicketManual : (int)InvoiceType.Manual, //|| fuelRequest.Job.IsMarine
                            IsEndSupplier = true,
                            CityGroupTerminalId = fuelRequest.CityGroupTerminalId,
                            IsFTL = fuelRequest.FuelRequestDetail != null && fuelRequest.FuelRequestDetail.TruckLoadTypeId == (int)TruckLoadTypes.FullTruckLoad
                        };

                        var onboardingPreferenceSetting =existingOrderPreferenceId.HasValue ? Context.DataContext.OnboardingPreferences.Where(t => t.Id==existingOrderPreferenceId.Value && t.IsActive)
                                                                             .Select(t => new { t.Id, t.IsSupressOrderPricing })
                                                                             .FirstOrDefault():
                                                                             Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == order.AcceptedCompanyId && t.IsActive)
                                                                             .OrderByDescending(t => t.Id)
                                                                             .Select(t => new { t.Id, t.IsSupressOrderPricing })
                                                                             .FirstOrDefault();
                        if (fuelRequest.FuelRequestPricingDetail != null || onboardingPreferenceSetting != null)
                        {
                            order.OrderAdditionalDetail = new OrderAdditionalDetail
                            {

                                //IsDriverToUpdateBOL = false,
                                IsDriverToUpdateBOL = (order.IsFTL && !isAutoAccept) ? true : (fuelRequest.FuelRequestDetail != null ? fuelRequest.FuelRequestDetail.IsDriverToUpdateBOL : false),
                                BOLInvoicePreferenceId = fuelRequest.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest && order.IsFTL ? (int)InvoiceNotificationPreferenceTypes.None : (int)InvoiceNotificationPreferenceTypes.SendInvoiceDDTWithoutBillingFile,
                                PreferencesSettingId = onboardingPreferenceSetting?.Id ?? null,
                                FreightPricingMethod = sourceRegionModel != null ? sourceRegionModel.FreightPricingMethod : FreightPricingMethod.Manual
                                //SupplierAssignedProductName = helperDomain.GetSupplierAssignedProductName(userContext.CompanyId, fuelRequest.MstProduct?.TfxProductId ?? 0 , fuelRequest?.TerminalId ?? 0),
                            };

                            var isSupressOrderPricing = onboardingPreferenceSetting != null ? onboardingPreferenceSetting.IsSupressOrderPricing : false;
                            order.OrderAdditionalDetail.IsSupressPricingEnabled = isSupressOrderPricing;
                            if (isSupressOrderPricing && fuelRequest.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest)
                            {
                                order.DefaultInvoiceType = (int)InvoiceType.DigitalDropTicketManual;
                            }
                        }
                        order.TaxExemptLicenses = user.Company.TaxExemptLicenses.Where(t => t.IsDefault).ToList();

                        // get special instruction file upload details
                        await SetSpecialInstructionFileDetailsFromParentFR(fuelRequest, order);

                        OrderXStatus orderStatus = new OrderXStatus();
                        orderStatus.StatusId = (int)OrderStatus.Open;
                        orderStatus.IsActive = true;
                        orderStatus.UpdatedBy = userContext.Id;
                        orderStatus.UpdatedDate = DateTimeOffset.Now;
                        order.OrderXStatuses.Add(orderStatus);

                        if (fuelRequest.DeliverySchedules.Count > 0)
                        {
                            fuelRequest.DeliverySchedules.ToList().ForEach(t =>
                                                                  order.OrderDeliverySchedules.Add(
                                                                      new OrderVersionXDeliverySchedule()
                                                                      {
                                                                          DeliveryRequestId = t.Id,
                                                                          Version = 1,
                                                                          CreatedBy = fuelRequest.CreatedBy,
                                                                          CreatedDate = fuelRequest.CreatedDate,
                                                                          IsActive = true
                                                                      }));
                        }
                        else
                        {
                            order.OrderDeliverySchedules.Add(
                                                            new OrderVersionXDeliverySchedule()
                                                            {
                                                                DeliveryRequestId = null,
                                                                Version = 1,
                                                                CreatedBy = fuelRequest.CreatedBy,
                                                                CreatedDate = fuelRequest.CreatedDate,
                                                                IsActive = true
                                                            });
                        }
                        fuelRequest.Orders.Add(order);
                        await Context.CommitAsync();

                        order.PoNumber = helperDomain.GetPoNumber(fuelRequest, order.IsProFormaPo, order.Id);
                        order.TfxPoNumber = order.PoNumber;

                        var orderDetailVersion = helperDomain.GetOrderDetailVersion(order, fuelRequest, userContext.Id);
                        order.OrderDetailVersions.Add(orderDetailVersion);

                        if (order.FuelRequest.FuelRequestDetail.PaymentMethod == PaymentMethods.CreditCard)
                            helperDomain.AddCreditCardProcessingFee(order);

                        var jobCarrierDetail = Context.DataContext.JobCarrierDetails.Where(t => t.JobId == order.FuelRequest.JobId && t.IsActive).FirstOrDefault();
                        if (jobCarrierDetail != null)
                        {
                            await notificationDomain.AddNotificationEventAsync(EventType.CarrierEmailOrderCreated, order.Id, userContext.Id);
                        }

                        #region check bulk plant is selected for the source region
                        if (sourceRegionModel != null && sourceRegionModel.FreightPricingMethod == FreightPricingMethod.Auto && sourceRegionModel.SelectedBulkPlants != null && sourceRegionModel.ApprovedBulkPlantId != 0)
                        {
                            var bulkPlant = Context.DataContext.BulkPlantLocations.Where(w => w.Id == sourceRegionModel.ApprovedBulkPlantId && w.IsActive).FirstOrDefault();
                            if (bulkPlant != null)
                                ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().SetFuelDispatchLocation(userContext, fuelRequest, order, bulkPlant);
                        }
                        #endregion

                        //insert trackable schedules
                        TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);
                        await trackableScheduleDomain.ProcessTrackableSchedules(fuelRequest.DeliverySchedules, order);

                        if (fuelRequest.PaymentTermId == (int)PaymentTerms.NetDays &&
                            user.Company.IsCreditAppEnabled && user.Company.CreditAppDocuments.Count > 0 &&
                            !user.Company.Orders.Any(t => t.FuelRequest.User.Company.Id == fuelRequest.User.Company.Id &&
                            t.FuelRequest.PaymentTermId == (int)PaymentTerms.NetDays))
                        {
                            response.IsFirstTimeBuyer = true;
                            response.ToUserEmail = fuelRequest.User.Email;
                            response.ToUser = $"{fuelRequest.User.FirstName} {fuelRequest.User.LastName}";
                        }

                        if (fuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest)
                        {
                            var parentOrder = Context.DataContext.Orders.Where(t => t.FuelRequestId == fuelRequest.FuelRequest1.Id).FirstOrDefault();
                            if (parentOrder != null)
                            {
                                if (parentOrder.IsEndSupplier)
                                    parentOrder.IsEndSupplier = false;

                                if (order.OrderAdditionalDetail != null)
                                {
                                    order.OrderAdditionalDetail.DRNotes = parentOrder.OrderAdditionalDetail != null ? parentOrder.OrderAdditionalDetail.DRNotes : string.Empty;
                                    order.OrderAdditionalDetail.IsManualBDNConfirmationRequired = parentOrder.OrderAdditionalDetail == null ? false : parentOrder.OrderAdditionalDetail.IsManualBDNConfirmationRequired;
                                    order.OrderAdditionalDetail.IsManualInvoiceConfirmationRequired = parentOrder.OrderAdditionalDetail == null ? false : parentOrder.OrderAdditionalDetail.IsManualInvoiceConfirmationRequired;
                                    if(parentOrder.OrderAdditionalDetail != null && parentOrder.OrderAdditionalDetail.BOLInvoicePreferenceId == (int)InvoiceNotificationPreferenceTypes.SendPDIDeliveryDetails)
                                    {
                                        order.OrderAdditionalDetail.BOLInvoicePreferenceId = (int)InvoiceNotificationPreferenceTypes.SendPDIDeliveryDetails;
                                        order.OrderAdditionalDetail.IsIncludePricingInExternalObj = parentOrder.OrderAdditionalDetail.IsIncludePricingInExternalObj;
                                    }
                                }
                            }

                            if (parentOrder != null && fuelRequest.FuelRequestDetail.IsDispatchRetainedByCustomer)
                            {
                                parentOrder.FuelRequest.FuelRequestDetail.IsDispatchRetainedByCustomer = true;
                            }

                            var deliveredSchedules = fuelRequest.DeliverySchedules.Where(t => !t.DeliveryScheduleXTrackableSchedules.
                                                                                                    Any(t1 => t1.DeliverySchedule.Type == (int)DeliveryScheduleType.SpecificDates
                                                                                                                && t1.DeliveryScheduleId == t.Id
                                                                                                                && t1.Invoices.Any(t2 => t2.IsActiveInvoice)));
                            foreach (var schedule in deliveredSchedules)
                            {
                                // Passing driver-id null will remove driver from delivery-schedule
                                helperDomain.AssignDeliveryLevelDriver(schedule, fuelRequest.CreatedBy, null, order.Id, true);
                            }
                        }

                        if (fuelRequest.Job.IsMarine)
                        {
                            var jobXAsset = Context.DataContext.JobXAssets.Where(t => t.FuelRequestId == fuelRequest.Id).FirstOrDefault();
                            if (jobXAsset != null)
                            {
                                jobXAsset.OrderId = order.Id;
                                Context.DataContext.Entry(jobXAsset).State = EntityState.Modified;
                            }
                        }
                        await Context.CommitAsync();
                        transaction.Commit();

                        int? brokeredOrderId = null;
                        //When new order is created, we need to udate terminal to previous brokered orders
                        if (order.FuelRequest.FuelRequest1 != null && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
                        {
                            var orderDomain = new OrderDomain(this);
                            var brokeredOrder = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.LastOrDefault();
                            if (brokeredOrder != null)
                            {
                                brokeredOrderId = brokeredOrder.Id;
                                //assing new terminal to all chained orders in broker case
                                await orderDomain.AssignNewTerminalToOrderAsync(order.TerminalId ?? 0, brokeredOrder.Id);
                                if (brokeredOrder.OrderTaxDetails != null && brokeredOrder.OrderTaxDetails.Count > 0)
                                {
                                    await orderDomain.CopyBrokeredOrderTaxesToNewOrder(order, brokeredOrder, userContext);
                                }
                            }
                        }


                        await notificationDomain.AddNotificationEventAsync(EventType.FuelRequestAccepted, fuelRequest.Id, userContext.Id);

                        var newsfeedDomain = new NewsfeedDomain(this);
                        await newsfeedDomain.SetFuelRequestAcceptedNewsfeed(userContext, fuelRequest, order);
                        //settingsDomain.SetBuyerSupplierInformation(userContext.CompanyId, fuelRequest.User.Company.Id, string.Empty, false, OrderCreationMethod.FromFuelRequest, userContext);
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageFuelRequestAccepted;
                        response.OrderId = order.Id;
                        if (order.TerminalId == null)
                        {
                            response.StatusCode = Status.Warning;
                            response.StatusMessage = response.StatusMessage + Resource.errMessageTerminalIsNotAssigned;
                        }
                        var qbWorkflowDomain = new QbWorkflowDomain(this);
                        qbWorkflowDomain.CreateSalesOrderWorkflow(userContext, fuelRequest, order);
                        qbWorkflowDomain.CreatePurchaseOrderWorkflow(userContext, fuelRequest, order, brokeredOrderId);

                        var appSetting = Context.DataContext.MstAppSettings.Where(t => t.Key.Equals(ApplicationConstants.KeyAppSettingTankOrderAssignement) && t.IsActive).Select(t => new { t.Key, t.Value }).FirstOrDefault();
                        if (appSetting != null && bool.TryParse(appSetting.Value, out bool flag) && flag)
                        {
                            var thirdPartyDomain = new ThirdPartyOrderDomain(this);
                            thirdPartyDomain.CreateQueueServiceEntryToMapTankWithOrders(userContext, order, fuelRequest, true);
                        }

                        if (sourceRegionModel != null && sourceRegionModel.FreightPricingMethod == FreightPricingMethod.Auto)
                        {
                            await UpdateFuelRequestSourceRegions(sourceRegionModel, order);
                        }

                        if (!isAutoAccept)
                            await CreateFreightOnlyOrderQueueMessage(order);
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageAcceptFuelRequestFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelRequestDomain", "AcceptFuelRequest", ex.Message, ex);
                }
            }
            return response;
        }

        private async Task<int> SetTerminalFromSourceRegion(SourceRegionsViewModel sourceRegionModel, int companyId)
        {
            int orderTerminalId = 0;
            if (sourceRegionModel.ApprovedTerminalId.Value != 0)
            {
                orderTerminalId = sourceRegionModel.ApprovedTerminalId.Value;
            }
            else if (sourceRegionModel.SelectedTerminals.Count == 1 && sourceRegionModel.ApprovedTerminalId.Value == 0)
            {
                orderTerminalId = sourceRegionModel.SelectedTerminals.FirstOrDefault();
            }
            else if (sourceRegionModel.SelectedTerminals.Count > 1 && sourceRegionModel.ApprovedTerminalId == 0)
            {
                var inputModel = new SourceRegionRequestModel()
                {
                    TerminalIds = sourceRegionModel.SelectedTerminals,
                    FuelTypeId = sourceRegionModel.FuelTypeId.Value,
                    PricingCodeId = sourceRegionModel.PricingCodeId,
                    Latitude = sourceRegionModel.Latitude,
                    Longitude = sourceRegionModel.Longitude,
                    IsSupressPricing = false,
                };

                ExternalPricingDomain externalPricingDomain = new ExternalPricingDomain();
                int companyCountryId = await Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetDefaultServingCountry(companyId));
                var terminals = await externalPricingDomain.GetClosestTerminalsForSourceRegions(companyId, companyCountryId, inputModel);
                if (terminals != null && terminals.Any())
                {
                    orderTerminalId = terminals.OrderBy(o => Convert.ToDouble(o.Code)).FirstOrDefault().Id;
                }
            }
            return orderTerminalId;
        }

        private async Task UpdateFuelRequestSourceRegions(SourceRegionsViewModel sourceRegionModel, Order order)
        {
            //update requestPricedetailId
            var paramterJSON = new SourceRegionJSONParameter()
            {
                SourceRegion = string.Join(",", sourceRegionModel.SelectedSourceRegions.Select(t => t)),
                SelectedTerminals = string.Join(",", sourceRegionModel.SelectedTerminals.Select(t => t)),
                SelectedBulkPlants = string.Join(",", sourceRegionModel.SelectedBulkPlants.Select(t => t))
            };
            var serializer = new JavaScriptSerializer();
            var srcRegion = new SourceRegionPricingRequestModel()
            {
                RequestPricingDetailId = order.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId,
                TerminalId = order.TerminalId,
                ParameterJSON = serializer.Serialize(paramterJSON),
            };
            var pricingDetailId = await new PricingServiceDomain().UpdateSourceRegion(srcRegion);
            if (pricingDetailId == null || pricingDetailId.Result == 0)
            {
                throw new ArgumentNullException(Resource.errMessageFailedSaveFuelPricing);
            }
        }

        public async Task<FuelRequestStatusViewModel> AcceptFreightOnlyFuelRequest(UserContext supplierUserContext, UserContext carrierUserContext, int fuelRequestId)
        {
            FuelRequestStatusViewModel response = new FuelRequestStatusViewModel();
            var helperDomain = new HelperDomain(this);
            var notificationDomain = new NotificationDomain(this);
            try
            {
                var fuelRequest = Context.DataContext.FuelRequests.Include(t => t.User).SingleOrDefault(t => t.Id == fuelRequestId && t.IsActive);
                if (fuelRequest != null)
                {
                    var sc_resposnse = await SetSuppliercostToFuelRequest(carrierUserContext, fuelRequest);
                    if (sc_resposnse.StatusCode == Status.Failed)
                    {
                        response.StatusCode = sc_resposnse.StatusCode;
                        response.StatusMessage = sc_resposnse.StatusMessage;
                        return response;
                    }

                    //update fuelrequest status
                    SetFuelRequestStatusToAccepted(carrierUserContext, fuelRequest);

                    //disable override of IsDropImageRequired flag (in case of broke order)
                    fuelRequest.FuelRequestDetail.IsDropImageRequired = helperDomain.GetDropImageRequired(carrierUserContext.CompanyId);

                    //get Order Level Terminal FromFR terminal details
                    int orderTerminalId = GetOrderLevelTerminalFromFR(fuelRequest);

                    //create order
                    Order order = new Order
                    {
                        PoNumber = ApplicationConstants.PoNumberPrefix,
                        IsProFormaPo = fuelRequest.Job.IsProFormaPoEnabled,
                        SignatureEnabled = fuelRequest.Job.SignatureEnabled,
                        AcceptedCompanyId = carrierUserContext.CompanyId,
                        AcceptedBy = carrierUserContext.Id,
                        AcceptedDate = DateTimeOffset.Now,
                        TerminalId = orderTerminalId > 0 ? orderTerminalId : fuelRequest.TerminalId,
                        BuyerCompanyId = supplierUserContext.CompanyId,
                        IsActive = true,
                        UpdatedBy = carrierUserContext.Id,
                        UpdatedDate = DateTimeOffset.Now,
                        DefaultInvoiceType = (int)InvoiceType.DigitalDropTicketManual,
                        IsEndSupplier = true,
                        CityGroupTerminalId = fuelRequest.CityGroupTerminalId,
                        IsFTL = fuelRequest.FuelRequestDetail != null && fuelRequest.FuelRequestDetail.TruckLoadTypeId == (int)TruckLoadTypes.FullTruckLoad
                    };

                    var onboardingPreferenceSetting = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == order.AcceptedCompanyId && t.IsActive)
                                                                         .OrderByDescending(t => t.Id)
                                                                         .Select(t => new { t.Id, t.IsSupressOrderPricing })
                                                                         .FirstOrDefault();
                    if (fuelRequest.FuelRequestPricingDetail != null || onboardingPreferenceSetting != null)
                    {
                        string drNote = string.Empty;
                        if(fuelRequest.ParentId != null)
                        {
                            drNote = await Context.DataContext.Orders.Where(t => t.FuelRequestId == fuelRequest.ParentId).Select(t => t.OrderAdditionalDetail != null ? t.OrderAdditionalDetail.DRNotes : "" ).FirstOrDefaultAsync();
                        }
                        order.OrderAdditionalDetail = new OrderAdditionalDetail
                        {

                            //IsDriverToUpdateBOL = false,
                            DRNotes = drNote,
                            IsDriverToUpdateBOL = fuelRequest.FuelRequestDetail != null ? fuelRequest.FuelRequestDetail.IsDriverToUpdateBOL : false,
                            BOLInvoicePreferenceId = fuelRequest.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest && order.IsFTL ? (int)InvoiceNotificationPreferenceTypes.None : (int)InvoiceNotificationPreferenceTypes.SendInvoiceDDTWithoutBillingFile,
                            PreferencesSettingId = onboardingPreferenceSetting?.Id ?? null,
                            //SupplierAssignedProductName = helperDomain.GetSupplierAssignedProductName(userContext.CompanyId, fuelRequest.MstProduct?.TfxProductId ?? 0 , fuelRequest?.TerminalId ?? 0),
                        };

                        var isSupressOrderPricing = onboardingPreferenceSetting != null ? onboardingPreferenceSetting.IsSupressOrderPricing : false;
                        order.OrderAdditionalDetail.IsSupressPricingEnabled = isSupressOrderPricing;
                        if (isSupressOrderPricing && fuelRequest.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest)
                        {
                            order.DefaultInvoiceType = (int)InvoiceType.DigitalDropTicketManual;
                        }
                    }
                    order.TaxExemptLicenses = Context.DataContext.TaxExemptLicenses.Where(t => t.CompanyId == carrierUserContext.CompanyId && t.IsDefault).ToList();

                    // get special instruction file upload details
                    await SetSpecialInstructionFileDetailsFromParentFR(fuelRequest, order);

                    OrderXStatus orderStatus = new OrderXStatus();
                    orderStatus.StatusId = (int)OrderStatus.Open;
                    orderStatus.IsActive = true;
                    orderStatus.UpdatedBy = carrierUserContext.Id;
                    orderStatus.UpdatedDate = DateTimeOffset.Now;
                    order.OrderXStatuses.Add(orderStatus);

                    if (fuelRequest.DeliverySchedules.Count > 0)
                    {
                        fuelRequest.DeliverySchedules.ToList().ForEach(t =>
                                                              order.OrderDeliverySchedules.Add(
                                                                  new OrderVersionXDeliverySchedule()
                                                                  {
                                                                      DeliveryRequestId = t.Id,
                                                                      Version = 1,
                                                                      CreatedBy = fuelRequest.CreatedBy,
                                                                      CreatedDate = fuelRequest.CreatedDate,
                                                                      IsActive = true
                                                                  }));
                    }
                    else
                    {
                        order.OrderDeliverySchedules.Add(
                                                        new OrderVersionXDeliverySchedule()
                                                        {
                                                            DeliveryRequestId = null,
                                                            Version = 1,
                                                            CreatedBy = fuelRequest.CreatedBy,
                                                            CreatedDate = fuelRequest.CreatedDate,
                                                            IsActive = true
                                                        });
                    }
                    fuelRequest.Orders.Add(order);
                    await Context.CommitAsync();

                    order.PoNumber = helperDomain.GetPoNumber(fuelRequest, order.IsProFormaPo, order.Id);
                    order.TfxPoNumber = order.PoNumber;

                    var orderDetailVersion = helperDomain.GetOrderDetailVersion(order, fuelRequest, carrierUserContext.Id);
                    order.OrderDetailVersions.Add(orderDetailVersion);

                    if (order.FuelRequest.FuelRequestDetail.PaymentMethod == PaymentMethods.CreditCard)
                        helperDomain.AddCreditCardProcessingFee(order);

                    var jobCarrierDetail = Context.DataContext.JobCarrierDetails.Where(t => t.JobId == order.FuelRequest.JobId && t.IsActive).FirstOrDefault();
                    if (jobCarrierDetail != null)
                    {
                        await notificationDomain.AddNotificationEventAsync(EventType.CarrierEmailOrderCreated, order.Id, carrierUserContext.Id);
                    }

                    ////insert trackable schedules
                    //TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);
                    //await trackableScheduleDomain.ProcessTrackableSchedules(fuelRequest.DeliverySchedules, order);

                    await Context.CommitAsync();

                    int? brokeredOrderId = null;
                    //When new order is created, we need to udate terminal to previous brokered orders
                    if (fuelRequest.FuelRequest1 != null)
                    {   
                        var brokeredOrder = fuelRequest.FuelRequest1.Orders.LastOrDefault();
                        if (brokeredOrder != null)
                        {
                            brokeredOrderId = brokeredOrder.Id;
                            //Carry forward the terminal to the new order in the chain
                            order.TerminalId = brokeredOrder.TerminalId;
                            order.CityGroupTerminalId = brokeredOrder.CityGroupTerminalId;
                            //if (brokeredOrder.OrderTaxDetails != null && brokeredOrder.OrderTaxDetails.Count > 0)
                            //{
                            //    await orderDomain.CopyBrokeredOrderTaxesToNewOrder(order, brokeredOrder, userContext);
                            //}
                            await Context.CommitAsync();
                        }
                    }
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageFuelRequestAccepted;
                    response.OrderId = order.Id;
                    if (order.TerminalId == null)
                    {
                        response.StatusCode = Status.Warning;
                        response.StatusMessage = response.StatusMessage + Resource.errMessageTerminalIsNotAssigned;
                    }
                    var qbWorkflowDomain = new QbWorkflowDomain(this);
                    qbWorkflowDomain.CreateSalesOrderWorkflow(carrierUserContext, fuelRequest, order);
                    qbWorkflowDomain.CreatePurchaseOrderWorkflow(carrierUserContext, fuelRequest, order, brokeredOrderId);

                    var appSetting = Context.DataContext.MstAppSettings.Where(t => t.Key.Equals(ApplicationConstants.KeyAppSettingTankOrderAssignement) && t.IsActive).Select(t => new { t.Key, t.Value }).FirstOrDefault();
                    if (appSetting != null && bool.TryParse(appSetting.Value, out bool flag) && flag)
                    {
                        var thirdPartyDomain = new ThirdPartyOrderDomain(this);
                        thirdPartyDomain.CreateQueueServiceEntryToMapTankWithOrders(carrierUserContext, order, fuelRequest, true);
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageAcceptFuelRequestFailed;
                LogManager.Logger.WriteException("FuelRequestDomain", "AcceptFuelRequest", ex.Message, ex);
            }
            return response;
        }

        private static void SetFuelRequestStatusToAccepted(UserContext userContext, FuelRequest fuelRequest)
        {
            fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
            FuelRequestXStatus frStatus = new FuelRequestXStatus();
            frStatus.StatusId = (int)FuelRequestStatus.Accepted;
            frStatus.IsActive = true;
            frStatus.UpdatedBy = userContext.Id;
            frStatus.UpdatedDate = DateTimeOffset.Now;
            fuelRequest.FuelRequestXStatuses.Add(frStatus);
            fuelRequest.UpdatedBy = userContext.Id;
            fuelRequest.UpdatedDate = DateTimeOffset.Now;
        }

        private async Task<StatusViewModel> SetSuppliercostToFuelRequest(UserContext userContext, FuelRequest fuelRequest)
        {
            var response = new StatusViewModel() { StatusCode = Status.Success };
            //get pricing Data
            var request = new PricingDetailRequestViewModel { Id = fuelRequest.FuelRequestPricingDetail.RequestPriceDetailId, Currency = (int)fuelRequest.Job.Currency };
            PricingRequestDetailApiResponse pricingDtls = Task.Run(() => new PricingServiceDomain(this).GetPricingRequestDetailByIdAsync(request)).Result;
            if (pricingDtls == null)
            {
                response.StatusMessage = Resource.ErrorPricingDetails;//change msg
                response.StatusCode = Status.Failed;
                return response;
            }
            //tier
            int fuelcostTierCount = pricingDtls.PricingRequestDetail.TierPricings.Where(w => w.PricingTypeId == (int)PricingType.Suppliercost).ToList().Count;
             if (fuelcostTierCount > 0)
            {
                foreach (var item in pricingDtls.PricingRequestDetail.TierPricings)
                {
                    if (item.PricingTypeId == (int)PricingType.Suppliercost)
                    {
                        int? tfxProductId = Context.DataContext.MstProducts.Where(w => w.Id == item.FuelTypeId).FirstOrDefault().TfxProductId;
                        var currentCostDomain = new CurrentCostDomain(this);
                        var globalFuelCost = await currentCostDomain.GetFuelCostForFuelRequest(userContext.CompanyId, tfxProductId ?? 0, fuelRequest.Job.StateId,fuelRequest.UoM, fuelRequest.Currency);
                        if (globalFuelCost.HasValue)
                        {
                            var pricingDetailids = new List<int>();
                            pricingDetailids.Add(fuelRequest.FuelRequestPricingDetail.RequestPriceDetailId);
                            await currentCostDomain.UpdateFuelCostForFuelRequest(pricingDetailids, globalFuelCost.Value, (int)SupplierCostTypes.GlobalCost);
                        }
                        else
                        {
                            if (fuelRequest.FuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest)
                            {
                                var pricingDetailids = new List<int>();
                                pricingDetailids.Add(fuelRequest.FuelRequestPricingDetail.RequestPriceDetailId);
                                await currentCostDomain.UpdateFuelCostForFuelRequest(pricingDetailids, 0, (int)SupplierCostTypes.GlobalCost);
                            }
                            else
                            {
                                response.StatusMessage = Resource.ErrorGlobalCostNotProvidedForFuelType;
                                response.StatusCode = Status.Failed;
                            }
                        }
                    }
                }
            }
            //non tier
            else if (fuelRequest.FuelRequestPricingDetail.DisplayPrice.Contains(PricingType.Suppliercost.GetDisplayName()))
            {
                var currentCostDomain = new CurrentCostDomain(this);
                var globalFuelCost = await currentCostDomain.GetFuelCostForFuelRequest(userContext.CompanyId, fuelRequest.MstProduct.TfxProductId ?? 0, fuelRequest.Job.StateId,fuelRequest.UoM, fuelRequest.Currency);
                if (globalFuelCost.HasValue)
                {
                    var pricingDetailids = new List<int>();
                    pricingDetailids.Add(fuelRequest.FuelRequestPricingDetail.RequestPriceDetailId);
                    await currentCostDomain.UpdateFuelCostForFuelRequest(pricingDetailids, globalFuelCost.Value, (int)SupplierCostTypes.GlobalCost);
                }
                else
                {
                    if (fuelRequest.FuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest)
                    {
                        var pricingDetailids = new List<int>();
                        pricingDetailids.Add(fuelRequest.FuelRequestPricingDetail.RequestPriceDetailId);
                        await currentCostDomain.UpdateFuelCostForFuelRequest(pricingDetailids, 0, (int)SupplierCostTypes.GlobalCost);
                    }
                    else
                    {
                        response.StatusMessage = Resource.ErrorGlobalCostNotProvidedForFuelType;
                        response.StatusCode = Status.Failed;
                    }
                }
            }
         
            return response;
        }

        private int GetOrderLevelTerminalFromFR(FuelRequest fuelRequest)
        {
            int orderTerminalId = 0;
            if (fuelRequest.ParentId != null)
            {
                var terminalId = Context.DataContext.Orders.Where(top => top.FuelRequestId == fuelRequest.ParentId)
                                .Select(top => top.TerminalId).FirstOrDefault();
                if (terminalId != null)
                {
                    orderTerminalId = terminalId.Value;

                }
            }
            return orderTerminalId;
        }

        public async Task CreateFreightOnlyOrderQueueMessage(Order order, int carrierCompId = 0)
        {
            if (order.FuelRequest.FuelRequestTypeId != (int)FuelRequestType.FreightOnlyRequest)
            {
                //check if flag is on to create freight only orders
                var freightOnlySetting = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == order.AcceptedCompanyId && t.IsActive).OrderByDescending(t => t.Id).Select(x => new { x.IsFreightOnlyOrderEnabled }).FirstOrDefault();
                if (freightOnlySetting != null && freightOnlySetting.IsFreightOnlyOrderEnabled)
                {
                    if (carrierCompId == 0) //fro FR accept 
                    {
                        //check carrier company for this job
                        var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
                        var carrierCompanyid = await fsDomain.GetAssignedCarrierCompanyId(order.AcceptedCompanyId, order.FuelRequest.JobId);
                        if (carrierCompanyid > 0)
                        {
                            AddQueueMsgToCreateFreightOnlyOrder(order, carrierCompanyid);
                        }
                    }
                    else //from TPO
                        AddQueueMsgToCreateFreightOnlyOrder(order, carrierCompId);
                }
                else
                    LogManager.Logger.WriteDebug("FuelRequestDomain", "CreateFreightOnlyOrderQueueMessage", "IsFreightOnlyOrderEnabled is False");
            }
        }

        public void AddQueueMsgToCreateFreightOnlyOrder(Order order, int carrierCompanyid)
        {
            var queueDomain = new QueueMessageDomain(this);
            var queueRequest = GetEnqueueMessageRequestViewModel(carrierCompanyid, order);
            var queueId = queueDomain.EnqeueMessage(queueRequest);
        }

        public void AddQueueMsgToCreateFreightOnlyOrder1(dynamic order, int carrierCompanyid)
        {
            var queueDomain = new QueueMessageDomain(this);
            var queueRequest = GetEnqueueMessageRequestViewModel1(carrierCompanyid, order);
            var queueId = queueDomain.EnqeueMessage(queueRequest);
        }
        private QueueMessageViewModel GetEnqueueMessageRequestViewModel(int carrierCompanyId, Order order)
        {
            var jsonViewModel = new CreateFreightOnlyOrderQueueMsg();
            jsonViewModel.CarrierCompanyId = carrierCompanyId;
            jsonViewModel.FuelRequestId = order.FuelRequestId;
            jsonViewModel.OrderId = order.Id;
            jsonViewModel.PONumber = order.PoNumber;
            jsonViewModel.SupplierCompanyId = order.AcceptedCompanyId;
            jsonViewModel.SupplierUserId = order.AcceptedBy;

            string json = JsonConvert.SerializeObject(jsonViewModel);

            return new QueueMessageViewModel()
            {
                CreatedBy = order.AcceptedCompanyId,
                QueueProcessType = QueueProcessType.CreateFreightOnlyOrder,
                JsonMessage = json
            };
        }
        private QueueMessageViewModel GetEnqueueMessageRequestViewModel1(int carrierCompanyId, dynamic order)
        {
            var jsonViewModel = new CreateFreightOnlyOrderQueueMsg();
            jsonViewModel.CarrierCompanyId = carrierCompanyId;
            jsonViewModel.FuelRequestId = order.FuelRequestId;
            jsonViewModel.OrderId = order.Id;
            jsonViewModel.PONumber = order.PoNumber;
            jsonViewModel.SupplierCompanyId = order.AcceptedCompanyId;
            jsonViewModel.SupplierUserId = order.AcceptedBy;

            string json = JsonConvert.SerializeObject(jsonViewModel);

            return new QueueMessageViewModel()
            {
                CreatedBy = order.AcceptedCompanyId,
                QueueProcessType = QueueProcessType.CreateFreightOnlyOrder,
                JsonMessage = json
            };
        }
        private async Task SetSpecialInstructionFileDetailsFromParentFR(FuelRequest fuelRequest, Order order)
        {
            var parentFuelRequestId = await Context.DataContext.FuelRequests.Where(t => t.Id == (fuelRequest.ParentId ?? 0) && t.IsActive).Select(t => t.Id).SingleOrDefaultAsync();
            if (parentFuelRequestId > 0)
            {
                var parentOrderFileDetails = await Context.DataContext.Orders.Where(t => t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open && t.FuelRequestId == parentFuelRequestId)
                                                                  .Select(t => t.OrderAdditionalDetail.FileDetails).FirstOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(parentOrderFileDetails))
                {
                    if (order.OrderAdditionalDetail == null)
                        order.OrderAdditionalDetail = new OrderAdditionalDetail();

                    order.OrderAdditionalDetail.FileDetails = parentOrderFileDetails;
                }
            }
        }

        public async Task<BrokerFuelRequestViewModel> GetBrokerFuelRequestAsync(int fuelRequestId, int companyId, bool isNewRequest)
        {
            var response = new BrokerFuelRequestViewModel(Status.Success);

            try
            {
                var fuelRequest = await Context.DataContext.FuelRequests.FirstOrDefaultAsync(t => t.Id == fuelRequestId && t.IsActive);
                if (fuelRequest != null)
                {
                    response = await GetBrokerFrModel(fuelRequestId, companyId, isNewRequest, fuelRequest);
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.errMessageLoadFuelRequestDetailsSuccess;
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageLoadFuelRequestDetailsFailed;
                LogManager.Logger.WriteException("FuelRequestDomain", "GetBrokerFuelRequestViewModel", ex.Message, ex);
            }

            return response;
        }

        public async Task<BrokerFuelRequestViewModel> GetBrokerFrModel(int fuelRequestId, int companyId, bool isNewRequest, FuelRequest fuelRequest)
        {
            BrokerFuelRequestViewModel response = GetBrokerFrDetails(companyId, fuelRequest, isNewRequest, true);
            response.IsCounterOfferExists = await IsCounterOfferExist(fuelRequestId);
            response.ParentId = fuelRequest.GetParentId();

            var helperDomain = ContextFactory.Current.GetDomain<HelperDomain>();
            response.Details.FuelDeliveryDetails.DeliverySchedules = helperDomain.GetUndeliveredSchedules(response.Details.FuelDeliveryDetails.DeliverySchedules);
            return response;
        }

        public BrokerFuelRequestViewModel GetBrokerFrDetails(int companyId, FuelRequest fuelRequest, bool isNewRequest, bool setDeliverySchedules)
        {
            BrokerFuelRequestViewModel response = fuelRequest.ToBrokerViewModel(setMargin: companyId == fuelRequest.User.Company.Id, isNewRequest: isNewRequest, setDeliverySchedules: setDeliverySchedules);
            var pricingDetails = GetRequestPricingDetail(fuelRequest.FuelRequestPricingDetail.RequestPriceDetailId);
            if (pricingDetails.TierPricings != null && pricingDetails.TierPricings.Any())
                SetTierPricingDetailsToBrokerViewModel(response, pricingDetails.TierPricings, fuelRequest.FuelRequestPricingDetail.DisplayPriceCode);
            else
                SetPricingDetailsToBrokerViewModel(response, pricingDetails, fuelRequest.FuelRequestPricingDetail.DisplayPriceCode);

            response.ParentId = fuelRequest.Id;
            response.ParentStartDate = fuelRequest.FuelRequestDetail.StartDate;
            response.ParentEndDate = fuelRequest.FuelRequestDetail.EndDate;
            response.ParentExpiryDate = fuelRequest.ExpirationDate;
            response.Terms.CompanyId = response.CompanyId = companyId;
            return response;
        }

        private void SetPricingDetailsToBrokerViewModel(BrokerFuelRequestViewModel viewModel, PricingRequestDetailResponseViewModel entity, string pricingCodeDesc)
        {
            viewModel.Details.FuelPricing.FuelPricingDetails.PricingCode.Id = entity.PricingCodeId;
            viewModel.Details.FuelPricing.FuelPricingDetails.PricingCode.Code = entity.PricingCode;
            viewModel.Details.FuelPricing.FuelPricingDetails.PricingCode.Description = pricingCodeDesc;

            viewModel.Details.FuelPricing.PricingTypeId = entity.PricingTypeId;
            viewModel.Details.FuelPricing.FuelPricingDetails.PricingSourceId = entity.PricingSourceId;
            viewModel.Details.FuelPricing.RackAvgTypeId = entity.RackAvgTypeId;
            if (entity.PricingTypeId == (int)PricingType.PricePerGallon)
            {
                viewModel.Details.FuelPricing.PricePerGallon = entity.PricePerGallon.GetPreciseValue(6);
            }
            else if (entity.PricingTypeId == (int)PricingType.RackAverage)
            {
                viewModel.Details.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackAverage;
            }
            else if (entity.PricingTypeId == (int)PricingType.RackHigh)
            {
                viewModel.Details.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackHigh;
                viewModel.Details.FuelPricing.PricingTypeId = (int)PricingType.RackAverage; //Do not change this to Rack High
            }
            else if (entity.PricingTypeId == (int)PricingType.RackLow)
            {
                viewModel.Details.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackLow;
                viewModel.Details.FuelPricing.PricingTypeId = (int)PricingType.RackAverage; //Do not change this to Rack Low
            }
            else if (entity.PricingTypeId == (int)PricingType.Suppliercost)
            {
                viewModel.Details.FuelPricing.SupplierCost = entity.SupplierCost;
                viewModel.Details.FuelPricing.SupplierCostMarkupTypeId = entity.RackAvgTypeId;
                viewModel.Details.FuelPricing.SupplierCostMarkupValue = entity.PricePerGallon.GetPreciseValue(6);
            }

            if (entity.PricingTypeId == (int)PricingType.RackAverage || entity.PricingTypeId == (int)PricingType.RackHigh
                || entity.PricingTypeId == (int)PricingType.RackLow)
            {
                viewModel.Details.FuelPricing.RackPrice = entity.PricePerGallon.GetPreciseValue(6);
            }
        }

       //tier
        private void SetTierPricingDetailsToBrokerViewModel(BrokerFuelRequestViewModel viewModel, List<TierPricing> entities, string pricingCodeDesc)
        {
            var helperDomain = new HelperDomain();
            int i = 1;
            viewModel.Details.FuelPricing.TierPricing.Pricings = new List<PricingViewModel>();
            foreach (var item in entities)
            {
                PricingViewModel pricingDtl = new PricingViewModel();
                pricingDtl.PricingCode.Id = item.PricingCodeId;
                pricingDtl.PricingCode.Code = item.PricingCode;
                pricingDtl.PricingCode.Description = pricingCodeDesc;

                pricingDtl.PricingTypeId = item.PricingTypeId;
                pricingDtl.PricingSourceId = item.PricingSourceId;
                pricingDtl.RackAvgTypeId = item.RackAvgTypeId;
              
                var rackAvgTypeId = item.RackAvgTypeId.HasValue ? item.RackAvgTypeId.Value : 0;
                var pricingTypeId = item.PricingTypeId;
                if (item.PricingSourceId == (int)PricingSource.Axxis && item.PricingTypeId == (int)PricingType.RackAverage)
                {
                    pricingTypeId = item.RackTypeId;
                }
                else if (item.PricingTypeId == (int)PricingType.Suppliercost)
                {
                    rackAvgTypeId = item.SupplierCostTypeId ?? 0;
                }

                pricingDtl.DisplayPrice = helperDomain.GetPricePerGallon(item.PricePerGallon, pricingTypeId, rackAvgTypeId);
                if (item.PricingTypeId == (int)PricingType.PricePerGallon)
                {
                    pricingDtl.PricePerGallon = item.PricePerGallon.GetPreciseValue(6);
                }
                else if (item.PricingTypeId == (int)PricingType.RackAverage)
                {
                    pricingDtl.MarkertBasedPricingTypeId = (int)PricingType.RackAverage;
                }
                else if (item.PricingTypeId == (int)PricingType.RackHigh)
                {
                    pricingDtl.MarkertBasedPricingTypeId = (int)PricingType.RackHigh;
                    pricingDtl.PricingTypeId = (int)PricingType.RackAverage; //Do not change this to Rack High
                }
                else if (item.PricingTypeId == (int)PricingType.RackLow)
                {
                    pricingDtl.MarkertBasedPricingTypeId = (int)PricingType.RackLow;
                    pricingDtl.PricingTypeId = (int)PricingType.RackAverage; //Do not change this to Rack Low
                }
                else if (item.PricingTypeId == (int)PricingType.Suppliercost)
                {
                    pricingDtl.SupplierCost = item.SupplierCost;
                    pricingDtl.SupplierCostMarkupTypeId = item.RackAvgTypeId;
                    pricingDtl.SupplierCostMarkupValue = item.PricePerGallon.GetPreciseValue(6);
                }

                if (item.PricingTypeId == (int)PricingType.RackAverage || item.PricingTypeId == (int)PricingType.RackHigh
                    || item.PricingTypeId == (int)PricingType.RackLow)
                {
                    pricingDtl.RackPrice = item.PricePerGallon.GetPreciseValue(6);
                }

                if (item.MaxQuantity != 0)
                {
                    // model.ToQuantity = item.MaxQuantity;
                    pricingDtl.ToQuantity = decimal.Parse(item.MaxQuantity.ToString("0.00"));
                    pricingDtl.RowIndex = i;
                }
                else
                {
                    pricingDtl.Quantity = item.MinQuantity;
                    pricingDtl.IsAboveQuantity = true;
                    pricingDtl.RowIndex = 2;
                }

                pricingDtl.FromQuantity = item.MinQuantity;
                //pricingDtl.ToQuantity = item.MaxQuantity;
                pricingDtl.FuelTypeId = item.FuelTypeId;
                if (item.TerminalId.HasValue)
                    pricingDtl.TerminalId = item.TerminalId.Value;
                if(item.CityRackTerminalId.HasValue)
                   pricingDtl.CityGroupTerminalId = item.CityRackTerminalId.Value;
                pricingDtl.Margin = item.Margin;
                pricingDtl.MarginTypeId = item.MarginTypeId;
                viewModel.Details.FuelPricing.TierPricing.Pricings.Add(pricingDtl);
                if ((i - 1) == 0)
                {
                    viewModel.Details.FuelPricing.FuelPricingDetails.PricingCode.Id = item.PricingCodeId;
                    viewModel.Details.FuelPricing.FuelPricingDetails.PricingCode.Code = item.PricingCode;
                    viewModel.Details.FuelPricing.FuelPricingDetails.PricingCode.Description = pricingCodeDesc;
                    viewModel.Details.FuelPricing.PricingTypeId = (int)PricingType.Tier;
                    viewModel.Details.FuelPricing.FuelPricingDetails.PricingSourceId = item.PricingSourceId;
                    viewModel.Details.FuelPricing.RackAvgTypeId = item.RackAvgTypeId;
                    viewModel.Details.FuelPricing.PricePerGallon = item.PricePerGallon;
                    viewModel.Details.FuelPricing.SupplierCost = item.SupplierCost;
                    viewModel.Details.FuelPricing.SupplierCostMarkupTypeId = item.RackAvgTypeId;
                    viewModel.Details.FuelPricing.SupplierCostMarkupValue = item.PricePerGallon.GetPreciseValue(6);

                    viewModel.Details.FuelPricing.TierPricing.TierPricingType =(TierPricingType)item.TierTypeId;
                    if (item.CumulationTypeId !=null)
                    {
                        viewModel.Details.FuelPricing.TierPricing.ResetCumulationSetting.CumulationType = (CumulationType)item.CumulationTypeId;
                        viewModel.Details.FuelPricing.TierPricing.ResetCumulationSetting.Day = (WeekDay)item.CumulationResetDay;
                        viewModel.Details.FuelPricing.TierPricing.ResetCumulationSetting.Date = item.CumulationResetDate;
                    }
                }
                i++;
                if (i == 2)
                    i = 3;
            }
            viewModel.Details.FuelPricing.IsTierPricing = true;
          
        }
        public async Task<StatusViewModel> SaveBrokerFuelRequestAsync(UserContext userContext, BrokerFuelRequestViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            bool IsPricingDetailIdNull = false;
            if (viewModel.Details.FuelPricing.IsTierPricing && viewModel.Details.FuelPricing.TierPricing ==null &&
               viewModel.Details.FuelPricing.TierPricing.Pricings==null && !viewModel.Details.FuelPricing.TierPricing.Pricings.Any())
            {
                response.StatusMessage = Resource.errMessageTierPricingRequired;
                return response;
            }

            if (viewModel.Details.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t =>
                t.FeeSubTypeId == (int)FeeSubType.ByQuantity && !t.DeliveryFeeByQuantity.Any()))
            {
                response.StatusMessage = Resource.errMessageFeeByQuantityRequired;
                return response;
            }

            using (var trasaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    List<int> companiesForNotification = null;
                    NotificationDomain notificationDomain = new NotificationDomain(this);
                    OrderDomain orderDomain = new OrderDomain(this);
                    Order order = null; FuelRequest fuelRequest = null;
                    if (viewModel.FuelRequestId == 0 && viewModel.Details.FuelDeliveryDetails.FuelRequestId > 0)
                    {
                        viewModel.UpdatedBy = userContext.Id;
                        viewModel.Details.CreatedBy = userContext.Id;
                        viewModel.Terms.CreatedBy = userContext.Id;
                        viewModel.CompanyId = userContext.CompanyId;
                        viewModel.Details.PrivateSupplierList.AddedById = userContext.Id;
                        viewModel.Details.PrivateSupplierList.CompanyId = userContext.CompanyId;

                        order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == viewModel.Details.OrderId);
                        var job = Context.DataContext.Jobs.SingleOrDefault(t => t.Id == viewModel.Details.JobId);
                        viewModel.Details.FuelPricing.Currency = job.Currency;
                        viewModel.Details.FuelQuantity.UoM = order.FuelRequest.UoM;

                        CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
                        viewModel.Details.FuelPricing.ExchangeRate = currencyRateDomain.GetCurrencyRate(job.Currency, Currency.USD, DateTimeOffset.Now);

                        fuelRequest = viewModel.ToEntity();
                        CustomResponseModel pricingDetailId =await initializeBrokerFuelPricingDetails(viewModel, fuelRequest, job);
                        if (pricingDetailId == null || pricingDetailId.Result == 0)
                        {
                            IsPricingDetailIdNull = true;
                            throw new ArgumentNullException(Resource.errMessageFailedSaveFuelPricing);
                        }

                        if (pricingDetailId != null)
                        {
                            viewModel.Details.FuelPricing.FuelPricingDetails.RequestPriceDetailId = pricingDetailId.Result;
                        }
                        else
                        {
                            response.StatusMessage = Resource.errMessageCreateBrokeredFuelRequestFailed;
                            return response;
                        }
                        //var pricingDetailId = await new PricingServiceDomain().SavePricingDetails(null, viewModel.Details.FuelQuantity.UoM, viewModel.Details);
                        //if (pricingDetailId == null || pricingDetailId.Result == 0)
                        //{
                        //    IsPricingDetailIdNull = true;
                        //    throw new ArgumentNullException(Resource.errMessageCreateCounterOfferSuccess);
                        //}

                        //if (pricingDetailId != null)
                        //{
                        //    viewModel.Details.FuelPricing.FuelPricingDetails.RequestPriceDetailId = pricingDetailId.Result;
                        //}
                        //else
                        //{
                        //    response.StatusMessage = Resource.errMessageCreateBrokeredFuelRequestFailed;
                        //    return response;
                        //}

                        //fuelRequest = viewModel.ToEntity();
                        fuelRequest.FuelRequestPricingDetail.DisplayPrice = pricingDetailId.CustomString1;
                        fuelRequest.FuelRequestPricingDetail.DisplayPriceCode = pricingDetailId.CustomString2;
                        fuelRequest.FuelRequestPricingDetail.RequestPriceDetailId = pricingDetailId.Result;
                        
                        //ExternalPricingDomain externalPricingDomain = new ExternalPricingDomain(this);
                        //await externalPricingDomain.GetTerminalWithPrice(viewModel.Details.FuelPricing.FuelPricingDetails, fuelRequest, job.Id, fuelRequest.Currency);
                         if (fuelRequest != null)
                        {
                            if (order != null)
                            {
                                fuelRequest.ParentId = order.FuelRequest.Id;
                            }
                            Context.DataContext.FuelRequests.Add(fuelRequest);
                            job.FuelRequests.Add(fuelRequest);
                            await Context.CommitAsync();
                        }
                        if (job.IsMarine)
                        {
                            var existingVessel = Context.DataContext.JobXAssets.Where(t => t.OrderId == order.Id && t.RemovedBy == null && t.RemovedDate ==null).FirstOrDefault();
                            if (existingVessel !=null)
                            {
                                var jobXAsset = new JobXAsset();
                                jobXAsset.AssetId = existingVessel.AssetId;
                                jobXAsset.JobId = job.Id;
                                jobXAsset.AssignedBy = userContext.Id;
                                jobXAsset.AssignedDate = DateTime.Now;
                                jobXAsset.FuelRequestId = fuelRequest.Id;
                                Context.DataContext.JobXAssets.Add(jobXAsset);
                            }
                        }
                        

                        //DifferentFuelPrice Entity
                        if (viewModel.Details.FuelPricing.PricingTypeId == (int)PricingType.Tier)
                        {
                            AddLastTierWithMaxQuantityNull(viewModel.Details.DifferentFuelPrices);
                            if (viewModel.Details.FuelPriceMargin.MarginTypeId == (int)MarginType.Percent)
                            {
                                viewModel.Details.DifferentFuelPrices.ForEach((d) =>
                                {
                                    d.PricePerGallon = HelperDomain.GetPriceWithMargin(viewModel.Details.FuelPriceMargin.Margin, d.PricePerGallon.Value);
                                });
                            }
                            viewModel.Details.DifferentFuelPrices.ForEach(t => fuelRequest.DifferentFuelPrices.Add(t.ToEntity(margin: viewModel.Details.FuelPriceMargin)));
                            //Added for dashboard performance to calculate totalFrValue
                            var lastRecord = viewModel.Details.DifferentFuelPrices.LastOrDefault();
                            if (lastRecord != null && lastRecord.PricingTypeId == (int)PricingType.PricePerGallon)
                            {
                                if (lastRecord.PricePerGallon.HasValue)
                                    fuelRequest.CreationTimeRackPPG = lastRecord.PricePerGallon.Value;
                            }
                        }

                        //FuelRequestXDeliveryDetail Entity
                        if (order.FuelRequest.FuelRequestDetail != null)
                        {
                            viewModel.Details.FuelDeliveryDetails.IsBolImageRequired = order.FuelRequest.FuelRequestDetail.IsBolImageRequired;
                            viewModel.Details.FuelDeliveryDetails.IsDriverToUpdateBOL = order.FuelRequest.FuelRequestDetail.IsDriverToUpdateBOL;
                            viewModel.Details.FuelDeliveryDetails.IsDropImageRequired = order.FuelRequest.FuelRequestDetail.IsDropImageRequired;
                            viewModel.Details.FuelDeliveryDetails.OrderEnforcementId = order.FuelRequest.FuelRequestDetail.OrderEnforcementId;
                            viewModel.Details.FuelDeliveryDetails.IsPrePostDipRequired = order.FuelRequest.FuelRequestDetail.IsPrePostDipRequired;
                        }
                        fuelRequest.FuelRequestDetail = viewModel.Details.FuelDeliveryDetails.ToEntity(fuelRequest.FuelRequestDetail);
                        fuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId = (int)viewModel.Details.FuelQuantity.QuantityIndicatorTypes;
                        FuelFeesDomain fuelFeesDomain = new FuelFeesDomain(this);
                        await fuelFeesDomain.SaveFuelFees(viewModel.Details.FuelDeliveryDetails, fuelRequest, userContext);

                        //Specail Instruction Entity
                        viewModel.Terms.SpecialInstructions.ForEach(t => { if (!string.IsNullOrWhiteSpace(t.Instruction)) fuelRequest.SpecialInstructions.Add(t.ToEntity()); });

                        //Delivery schedule entity -- now on BFR delivery schedule is in lable format, so we need to take data from order/FR
                        if (order != null && viewModel.Details.FuelDeliveryDetails.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries)
                        {
                            var orderDeliverySchedules = orderDomain.GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules).Where(t => t.DeliveryRequestId.HasValue).Select(t => t.DeliverySchedule).ToList();

                            if (orderDeliverySchedules != null && orderDeliverySchedules.Count > 0)
                            {
                                fuelRequest.DeliverySchedules = orderDeliverySchedules;
                            }
                            else
                            {
                                var deliverySchedulesFromFR = order.FuelRequest.DeliverySchedules.ToList();
                                if (deliverySchedulesFromFR.Any())
                                {
                                    fuelRequest.DeliverySchedules = deliverySchedulesFromFR;
                                }
                            }
                            fuelRequest.DeliverySchedules = fuelRequest.DeliverySchedules.Where(t => !(t.Type == (int)DeliveryScheduleType.SpecificDates
                                                                                                        && t.DeliveryScheduleXTrackableSchedules.Any(t1 => t1.Invoices.Any(t2 => t2.IsActiveInvoice)))).ToList();
                        }

                        //PaymentDiscounts Entity
                        if (viewModel.Terms.PaymentDiscount.IsDiscountOnEarlyPayment)
                        {
                            List<PaymentDiscount> paymentDiscountList = new List<PaymentDiscount> { viewModel.Terms.PaymentDiscount.ToEntity() };
                            fuelRequest.PaymentDiscounts = paymentDiscountList;
                        }

                        //SupplierQualifications Entity
                        if (viewModel.Terms.SupplierQualifications.Count > 0)
                        {
                            var fuelRequestXSupplierQualifications = Context.DataContext.MstSupplierQualifications.Where(t => viewModel.Terms.SupplierQualifications.Contains(t.Id)).ToList();
                            fuelRequest.MstSupplierQualifications.ToList().RemoveAll(t => t.Id > 0);
                            fuelRequest.MstSupplierQualifications = fuelRequestXSupplierQualifications;
                        }

                        //PrivateSupplierList Entity
                        if (!viewModel.Details.PrivateSupplierList.IsPublicRequest)
                        {
                            SetPrivateListToFuelRequest(viewModel.Details.PrivateSupplierList.PrivateSupplierIds, fuelRequest, out companiesForNotification);
                        }
                        else
                        {
                            fuelRequest.PrivateSupplierLists.Clear();
                        }

                        Context.DataContext.Entry(fuelRequest).State = EntityState.Modified;


                        await notificationDomain.AddNotificationEventAsync(
                                                        EventType.BrokerFuelRequestCreated,
                                                        fuelRequest.Id,
                                                        fuelRequest.CreatedBy,
                                                        companiesForNotification);
                        viewModel.FuelRequestId = fuelRequest.Id;
                    }
                    else
                    {
                        fuelRequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == viewModel.Details.FuelDeliveryDetails.FuelRequestId);
                        if (fuelRequest != null && fuelRequest.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Open)
                        {
                            if (!viewModel.Details.PrivateSupplierList.IsPublicRequest)
                            {
                                SetPrivateListToFuelRequest(viewModel.Details.PrivateSupplierList.PrivateSupplierIds, fuelRequest, out companiesForNotification);
                            }
                            else
                            {
                                fuelRequest.IsPublicRequest = true;
                                fuelRequest.PrivateSupplierLists.Clear();
                            }

                            Context.DataContext.Entry(fuelRequest).State = EntityState.Modified;

                            await notificationDomain.AddNotificationEventAsync(
                                                            EventType.BrokerFuelRequestCreated,
                                                            fuelRequest.Id,
                                                            fuelRequest.CreatedBy,
                                                            companiesForNotification);
                        }
                        viewModel.FuelRequestId = fuelRequest.Id;
                    }

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageCreateBrokeredFuelRequestSuccess;

                    await Context.CommitAsync();
                    trasaction.Commit();

                    if (order != null && fuelRequest != null)
                    {
                        NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                        await newsfeedDomain.SetOrderBrokeredNewsfeed(userContext, fuelRequest, order);
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
                        response.StatusMessage = Resource.errMessageCreateBrokeredFuelRequestFailed;
                    }
                    trasaction.Rollback();
                    LogManager.Logger.WriteException("FuelRequestDomain", "SaveFuelRequestAsync", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> SaveFreightOnlyBrokerFuelRequestAsync(UserContext userContext, BrokerFuelRequestViewModel viewModel, int? parentFrId)
        {
            StatusViewModel response = new StatusViewModel();
            bool IsPricingDetailIdNull = false;
            try
            {
                List<int> companiesForNotification = null;
                OrderDomain orderDomain = new OrderDomain(this);

                viewModel.UpdatedBy = userContext.Id;
                viewModel.Details.CreatedBy = userContext.Id;
                viewModel.Terms.CreatedBy = userContext.Id;
                viewModel.CompanyId = userContext.CompanyId;
                viewModel.Details.PrivateSupplierList.AddedById = userContext.Id;
                viewModel.Details.PrivateSupplierList.CompanyId = userContext.CompanyId;

                var job = Context.DataContext.Jobs.SingleOrDefault(t => t.Id == viewModel.Details.JobId);
                viewModel.Details.FuelPricing.Currency = job.Currency;
                viewModel.Details.FuelQuantity.UoM = job.UoM;

                CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
                viewModel.Details.FuelPricing.ExchangeRate = currencyRateDomain.GetCurrencyRate(job.Currency, Currency.USD, DateTimeOffset.Now);

                var fuelRequest = viewModel.ToEntity();
                CustomResponseModel pricingDetailId = await initializeBrokerFuelPricingDetails(viewModel, fuelRequest, job);
                if (pricingDetailId == null || pricingDetailId.Result == 0)
                {
                    IsPricingDetailIdNull = true;
                    throw new ArgumentNullException(Resource.errMessageFailedSaveFuelPricing);
                }

                if (pricingDetailId != null)
                {
                    viewModel.Details.FuelPricing.FuelPricingDetails.RequestPriceDetailId = pricingDetailId.Result;
                }
                else
                {
                    response.StatusMessage = Resource.errMessageCreateBrokeredFuelRequestFailed;
                    return response;
                }
                
                fuelRequest.FuelRequestPricingDetail.DisplayPrice = pricingDetailId.CustomString1;
                fuelRequest.FuelRequestPricingDetail.DisplayPriceCode = pricingDetailId.CustomString2;
                fuelRequest.FuelRequestPricingDetail.RequestPriceDetailId = pricingDetailId.Result;
                //  ExternalPricingDomain externalPricingDomain = new ExternalPricingDomain(this);
                //  await externalPricingDomain.GetTerminalWithPrice(viewModel.Details.FuelPricing.FuelPricingDetails, fuelRequest, job.Id, fuelRequest.Currency);
                if (fuelRequest != null)
                {
                    if (parentFrId.HasValue)
                    {
                        fuelRequest.ParentId = parentFrId.Value;
                    }
                    Context.DataContext.FuelRequests.Add(fuelRequest);
                    job.FuelRequests.Add(fuelRequest);
                    await Context.CommitAsync();
                }

                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == viewModel.Details.OrderId);
                if (order.FuelRequest.FuelRequestDetail != null)
                {
                    viewModel.Details.FuelDeliveryDetails.IsBolImageRequired = order.FuelRequest.FuelRequestDetail.IsBolImageRequired;
                    viewModel.Details.FuelDeliveryDetails.IsDriverToUpdateBOL = order.FuelRequest.FuelRequestDetail.IsDriverToUpdateBOL;
                    viewModel.Details.FuelDeliveryDetails.IsDropImageRequired = order.FuelRequest.FuelRequestDetail.IsDropImageRequired;
                    viewModel.Details.FuelDeliveryDetails.OrderEnforcementId = order.FuelRequest.FuelRequestDetail.OrderEnforcementId;
                    viewModel.Details.FuelDeliveryDetails.IsPrePostDipRequired = order.FuelRequest.FuelRequestDetail.IsPrePostDipRequired;
                }
                fuelRequest.FuelRequestDetail = viewModel.Details.FuelDeliveryDetails.ToEntity(fuelRequest.FuelRequestDetail);
                fuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId = (int)viewModel.Details.FuelQuantity.QuantityIndicatorTypes;
                FuelFeesDomain fuelFeesDomain = new FuelFeesDomain(this);
                await fuelFeesDomain.SaveFuelFees(viewModel.Details.FuelDeliveryDetails, fuelRequest, userContext);

                //Specail Instruction Entity
                viewModel.Terms.SpecialInstructions.ForEach(t => { if (!string.IsNullOrWhiteSpace(t.Instruction)) fuelRequest.SpecialInstructions.Add(t.ToEntity()); });

                //Delivery schedule entity -- now on BFR delivery schedule is in lable format, so we need to take data from order/FR
                if (order != null && viewModel.Details.FuelDeliveryDetails.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries)
                {
                    var orderDeliverySchedules = orderDomain.GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules).Where(t => t.DeliveryRequestId.HasValue).Select(t => t.DeliverySchedule).ToList();

                    if (orderDeliverySchedules != null && orderDeliverySchedules.Count > 0)
                    {
                        fuelRequest.DeliverySchedules = orderDeliverySchedules;
                    }
                    else
                    {
                        var deliverySchedulesFromFR = order.FuelRequest.DeliverySchedules.ToList();
                        if (deliverySchedulesFromFR.Count > 0)
                        {
                            fuelRequest.DeliverySchedules = deliverySchedulesFromFR;
                        }
                    }
                    fuelRequest.DeliverySchedules = fuelRequest.DeliverySchedules.Where(t => !(t.Type == (int)DeliveryScheduleType.SpecificDates
                                                                                                && t.DeliveryScheduleXTrackableSchedules.Any(t1 => t1.Invoices.Any(t2 => t2.IsActiveInvoice)))).ToList();
                }

                //PaymentDiscounts Entity
                if (viewModel.Terms.PaymentDiscount.IsDiscountOnEarlyPayment)
                {
                    List<PaymentDiscount> paymentDiscountList = new List<PaymentDiscount> { viewModel.Terms.PaymentDiscount.ToEntity() };
                    fuelRequest.PaymentDiscounts = paymentDiscountList;
                }

                //PrivateSupplierList Entity
                if (!viewModel.Details.PrivateSupplierList.IsPublicRequest)
                {
                    SetPrivateListToFuelRequest(viewModel.Details.PrivateSupplierList.PrivateSupplierIds, fuelRequest, out companiesForNotification);
                }
                else
                {
                    fuelRequest.PrivateSupplierLists.Clear();
                }

                Context.DataContext.Entry(fuelRequest).State = EntityState.Modified;
                viewModel.FuelRequestId = fuelRequest.Id;

                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.errMessageCreateBrokeredFuelRequestSuccess;

                await Context.CommitAsync();
            }
            catch (Exception ex)
            {
                if (IsPricingDetailIdNull)
                {
                    response.StatusMessage = Resource.errMessageFailedSaveFuelPricing;
                }
                else
                {
                    response.StatusMessage = Resource.errMessageCreateBrokeredFuelRequestFailed;
                }
                LogManager.Logger.WriteException("FuelRequestDomain", "SaveFreightOnlyBrokerFuelRequestAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ReSubmitFuelRequestAsync(int orderId, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();

            try
            {
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                if (order != null && order.FuelRequest != null)
                {
                    var fuelRequest = order.FuelRequest.ToViewModel();
                    fuelRequest.Id = 0;
                    fuelRequest.FuelDetails.StatusId = (int)FuelRequestStatus.Open;
                    fuelRequest.FuelDetails.CreatedDate = DateTimeOffset.Now;
                    fuelRequest.FuelDetails.CreatedBy = userContext.Id;

                    if (fuelRequest.FuelDeliveryDetails.ExpirationDate <= DateTimeOffset.Now)
                    {
                        fuelRequest.FuelDeliveryDetails.ExpirationDate = null;
                    }
                    if (fuelRequest.FuelDeliveryDetails.EndDate <= DateTimeOffset.Now)
                    {
                        fuelRequest.FuelDeliveryDetails.EndDate = null;
                    }
                    response = await SaveFuelRequestAsync(fuelRequest, true, 0, userContext);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "ReSubmitFuelRequestAsync", ex.Message, ex);
            }

            return response;
        }

        public List<CounterOfferGridViewModel> GetSupplierCounterOfferGridAsync(int companyId, int userId, int fuelRequestId = 0, FuelRequestFilterViewModel filter = null, string fromDate = "", string toDate = "")
        {
            List<CounterOfferGridViewModel> response = new List<CounterOfferGridViewModel>();
            HelperDomain helperDomain = new HelperDomain(this);
            try
            {
                if ((int)filter.Filter != 0 && filter.Filter != FuelRequestFilterType.Open)
                {
                    return response;
                }
                DateTimeOffset StartDate = DateTimeOffset.Now.Date.AddDays(ApplicationConstants.DateFilterDefaultDays);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(fromDate))
                {
                    StartDate = Convert.ToDateTime(fromDate).Date;
                }
                if (!string.IsNullOrEmpty(toDate))
                {
                    EndDate = Convert.ToDateTime(toDate).Date.AddDays(1);
                }

                var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == userId && t.IsActive);
                var blacklistCompanyIds = Task.Run(() => ContextFactory.Current.GetDomain<SettingsDomain>().GetBlacklistedCompanyIdsAsync(companyId)).Result;
                if (user != null && user.Company != null)
                {
                    var counterOffers = Context.DataContext.CounterOffers
                                    .Where
                                    (
                                        t =>
                                        t.FuelRequest.IsActive &&
                                        !blacklistCompanyIds.Contains(t.User.Company.Id) &&
                                        !blacklistCompanyIds.Contains(t.User1.Company.Id) &&
                                        ((fuelRequestId > 0 && t.OriginalFuelRequestId == fuelRequestId) ||
                                        (t.SupplierId == userId))
                                    );

                    if (filter != null && (int)filter.Currency > 0)
                    {
                        counterOffers = counterOffers.Where(t => t.FuelRequest.Currency == filter.Currency);
                    }

                    if (filter != null && filter.CountryId > 0)
                    {
                        counterOffers = counterOffers.Where(t => t.FuelRequest.Job.CountryId == filter.CountryId);
                    }

                    var co = counterOffers.OrderByDescending(t => t.Id);
                    foreach (var item in co)
                    {
                        response.Add(new CounterOfferGridViewModel(Status.Success)
                        {
                            CounterOfferRequestNumber = item.FuelRequest.RequestNumber,
                            FuelRequestId = item.OriginalFuelRequestId,
                            RequestNumber = helperDomain.GetFuelRequestNumberFromCounterOffer(item.OriginalFuelRequestId),
                            CreatedDate = item.FuelRequest.CreatedDate.ToString(Resource.constFormatDate),
                            Job = item.FuelRequest.Job.Name,
                            Price = item.FuelRequest.FuelRequestPricingDetail.DisplayPrice,
                            TotalGallons = helperDomain.GetQuantityRequested(item.FuelRequest.MaxQuantity),
                            Status = GetCounterOfferStatus(item.Id, false),
                            OriginalFuelRequestId = item.OriginalFuelRequestId
                        });
                    }

                    response = (from offer in response
                                group offer by offer.FuelRequestId into grp
                                select grp.First()).ToList();

                    if (fuelRequestId > 0)
                    {
                        response = response.Where(t => t.FuelRequestId == fuelRequestId).ToList();
                    }

                    for (int cntCounterOffer = 0; cntCounterOffer < response.Count; cntCounterOffer++)
                    {
                        var originalFuelRequestId = response[cntCounterOffer].OriginalFuelRequestId;
                        var isFuelRequestInDateRange = helperDomain.ApplyDateRangeFilterToFuelRequest(originalFuelRequestId, StartDate, EndDate);

                        if (!isFuelRequestInDateRange)
                        {
                            response.Remove(response[cntCounterOffer]);
                            cntCounterOffer--;
                        }
                    }

                    foreach (var item in response)
                    {
                        var fuelRequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == item.FuelRequestId);
                        if (fuelRequest != null)
                        {
                            item.Buyer = $"{fuelRequest.User.Company.Name}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetSupplierCounterOfferGridAsync", ex.Message, ex);
            }
            return response;
        }

        public List<CounterOfferGridViewModel> GetBrokeredCounterOfferGridAsync(UserContext userContext, string fromDate = "", string toDate = "", int fuelRequestId = 0, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            List<CounterOfferGridViewModel> response = new List<CounterOfferGridViewModel>();
            HelperDomain helperDomain = new HelperDomain(this);
            try
            {
                DateTimeOffset StartDate = DateTimeOffset.Now.Date.AddDays(ApplicationConstants.DateFilterDefaultDays);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(fromDate))
                {
                    StartDate = Convert.ToDateTime(fromDate).Date;
                }
                if (!string.IsNullOrEmpty(toDate))
                {
                    EndDate = Convert.ToDateTime(toDate).Date.AddDays(1);
                }

                var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == userContext.Id && t.IsActive);
                var blacklistCompanyIds = Task.Run(() => ContextFactory.Current.GetDomain<SettingsDomain>().GetBlacklistedCompanyIdsAsync(userContext.CompanyId)).Result;
                if (user != null && user.Company != null)
                {
                    var sqlQuery = Context.DataContext.CounterOffers
                                    .Where
                                    (
                                        t =>
                                        t.FuelRequest.IsActive &&
                                        (fuelRequestId == 0 || t.OriginalFuelRequestId == fuelRequestId) &&
                                        (fuelRequestId > 0 || (t.FuelRequest.Currency == currency && t.FuelRequest.Job.CountryId == countryId)) &&
                                        !blacklistCompanyIds.Contains(t.User.Company.Id) &&
                                        !blacklistCompanyIds.Contains(t.User1.Company.Id) &&
                                        t.BuyerId == userContext.Id &&
                                        (t.FuelRequest.CreatedDate >= StartDate || t.FuelRequest.FuelRequestDetail.StartDate >= StartDate) &&
                                        (t.FuelRequest.CreatedDate < EndDate || t.FuelRequest.FuelRequestDetail.StartDate < EndDate)
                                    )
                                    .OrderByDescending(t => t.Id);

                    foreach (var item in sqlQuery)
                    {
                        response.Add(new CounterOfferGridViewModel(Status.Success)
                        {
                            CounterOfferRequestNumber = item.FuelRequest.RequestNumber,
                            FuelRequestId = item.OriginalFuelRequestId,
                            RequestNumber = helperDomain.GetFuelRequestNumberFromCounterOffer(item.OriginalFuelRequestId),
                            CreatedDate = item.FuelRequest.CreatedDate.ToString(Resource.constFormatDate),
                            Job = item.FuelRequest.Job.Name,
                            Price = item.FuelRequest.FuelRequestPricingDetail.DisplayPrice,
                            TotalGallons = helperDomain.GetQuantityRequested(item.FuelRequest.MaxQuantity),
                            SupplierId = item.SupplierId,
                            Status = GetCounterOfferStatus(item.Id, true),
                            OriginalFuelRequestId = item.OriginalFuelRequestId
                        });
                    }

                    response = (from offer in response
                                group offer by new { offer.FuelRequestId, offer.SupplierId } into grp
                                select grp.First()).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetBrokeredCounterOfferGridAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<CounterOfferGridViewModel>> GetBuyerCounterOfferGridAsync(UserContext userContext, int fuelRequestId = 0, FuelRequestFilterViewModel filter = null)
        {
            List<CounterOfferGridViewModel> response = new List<CounterOfferGridViewModel>();
            HelperDomain helperDomain = new HelperDomain(this);
            try
            {
                if (filter != null && filter.Filter != FuelRequestFilterType.All && filter.Filter != FuelRequestFilterType.Open)
                {
                    return response;
                }

                DateTimeOffset startDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTimeOffset endDate = DateTimeOffset.Now.Date.AddDays(1);

                if (!string.IsNullOrEmpty(filter.StartDate))
                {
                    startDate = Convert.ToDateTime(filter.StartDate).Date;
                }
                if (!string.IsNullOrEmpty(filter.EndDate))
                {
                    endDate = Convert.ToDateTime(filter.EndDate).Date.AddDays(1);
                }
                if (filter.JobId > 0)
                {
                    var job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == filter.JobId);
                    if (job != null)
                    {
                        filter.Currency = job.Currency;
                        filter.CountryId = job.CountryId;
                    }
                }

                var blacklistCompanyIds = await new SettingsDomain(helperDomain).GetBlacklistedCompanyIdsAsync(userContext.CompanyId);
                var counterOffers = Context.DataContext.CounterOffers.Where
                                    (
                                        t =>
                                        t.FuelRequest.IsActive &&
                                        t.User.CompanyId == userContext.CompanyId &&
                                        t.User.Id == userContext.Id &&
                                        !blacklistCompanyIds.Contains(t.User.Company.Id) &&
                                        !blacklistCompanyIds.Contains(t.User1.Company.Id)
                                    );

                if (filter != null && filter.JobId > 0)
                {
                    counterOffers = counterOffers.Where(t => t.FuelRequest.Job.Id == filter.JobId);
                }

                if (filter != null && (int)filter.Currency > 0)
                {
                    counterOffers = counterOffers.Where(t => t.FuelRequest.Currency == filter.Currency);
                }

                if (filter != null && filter.CountryId > 0)
                {
                    counterOffers = counterOffers.Where(t => t.FuelRequest.Job.CountryId == filter.CountryId);
                }

                await counterOffers.OrderByDescending(t => t.Id).ForEachAsync(t => response.Add(new CounterOfferGridViewModel(Status.Success)
                {
                    CounterOfferRequestNumber = t.FuelRequest.RequestNumber,
                    FuelRequestId = t.OriginalFuelRequestId,
                    RequestNumber = helperDomain.GetFuelRequestNumberFromCounterOffer(t.OriginalFuelRequestId),
                    CreatedDate = t.FuelRequest.CreatedDate.ToString(Resource.constFormatDate),
                    Job = t.FuelRequest.Job.Name,
                    Price = t.FuelRequest.FuelRequestPricingDetail.DisplayPrice,
                    TotalGallons = helperDomain.GetQuantityRequested(t.FuelRequest.MaxQuantity),
                    SupplierId = t.SupplierId,
                    Status = GetCounterOfferStatus(t.Id, true),
                    OriginalFuelRequestId = t.OriginalFuelRequestId
                }));

                var latestCounterOffers = from offer in response
                                          group offer by new { offer.FuelRequestId, offer.SupplierId } into grp
                                          select grp.First();
                response = latestCounterOffers.ToList();

                if (fuelRequestId > 0)
                {
                    response = response.Where(t => t.FuelRequestId == fuelRequestId).ToList();
                }
                else if (filter.FuelRequestId > 0)
                {
                    // For CO's tab on FR details page
                    response = response.Where(t => t.FuelRequestId == filter.FuelRequestId).ToList();
                }

                foreach (var item in response)
                {
                    var supplier = Context.DataContext.Users.SingleOrDefault(t => t.Id == item.SupplierId);
                    if (supplier != null)
                    {
                        item.Supplier = $"{supplier.FirstName} {supplier.LastName}";
                    }
                }

                if (filter.FuelRequestId == 0)
                {
                    for (int cntCounterOffer = 0; cntCounterOffer < response.Count; cntCounterOffer++)
                    {
                        var originalFuelRequestId = response[cntCounterOffer].OriginalFuelRequestId;
                        var isFuelRequestInDateRange = helperDomain.ApplyDateRangeFilterToFuelRequest(originalFuelRequestId, startDate, endDate);

                        if (!isFuelRequestInDateRange)
                        {
                            response.Remove(response[cntCounterOffer]);
                            cntCounterOffer--;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetBuyerCounterOfferGridAsync", ex.Message, ex);
            }
            return response;
        }

        public string GetCounterOfferStatus(int counterOfferId, bool isBuyer)
        {
            var response = string.Empty;
            try
            {   
                var entity = Context.DataContext.CounterOffers.SingleOrDefault(t => t.Id == counterOfferId);
                var originalFuelRequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == entity.OriginalFuelRequestId);
                if (originalFuelRequest != null && originalFuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).StatusId == (int)FuelRequestStatus.Expired)
                {
                    response = Resource.lblExpired;
                }
                else if (entity != null)
                {
                    if (entity.BuyerStatus == (int)CounterOfferStatus.Cancelled || entity.SupplierStatus == (int)CounterOfferStatus.Cancelled)
                        response = Resource.lblCancelled;
                    else if (entity.BuyerStatus == (int)CounterOfferStatus.Declined || entity.SupplierStatus == (int)CounterOfferStatus.Declined)
                        response = Resource.lblDeclined;
                    else if (entity.BuyerStatus == (int)CounterOfferStatus.Accepted || entity.SupplierStatus == (int)CounterOfferStatus.Accepted)
                        response = Resource.lblAccepted;
                    else if (entity.BuyerStatus == (int)CounterOfferStatus.Pending)
                        response = isBuyer ? Resource.lblOpen : Resource.lblPending;
                    else if (entity.SupplierStatus == (int)CounterOfferStatus.Pending || entity.BuyerStatus == (int)CounterOfferStatus.Countered)
                        response = isBuyer ? Resource.lblPending : Resource.lblOpen;
                    else if (entity.SupplierStatus == (int)CounterOfferStatus.Countered)
                        response = isBuyer ? Resource.lblOpen : Resource.lblPending;
                    else if (entity.BuyerStatus == (int)CounterOfferStatus.Countered)
                        response = isBuyer ? Resource.lblPending : Resource.lblOpen;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetCounterOfferStatus", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<int>> GetOpenFuelRequestsWithExpirationDateAsync()
        {
            var response = new List<int>();
            try
            {
                response = await Context.DataContext.FuelRequests.Where(
                                                t =>
                                                t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Open &&
                                                t.FuelRequestTypeId == (int)FuelRequestType.FuelRequest &&
                                                t.IsActive).Select(t => t.Id).ToListAsync();                
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetOpenFuelRequestsWithExpirationDateAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task ProcessFuelRequestAsync(int id, int FRExpirationReminderTime)
        {
            var entity = await Context.DataContext.FuelRequests.SingleOrDefaultAsync(t => t.Id == id);
            if (entity != null)
            {
                await ProcessNewIncomingFuelRequestAsync(entity);
                await ProcessFuelRequestExpirationAsync(entity, FRExpirationReminderTime);
            }
        }

        public async Task ProcessFuelRequestExpirationAsync(FuelRequest entity, int FRExpirationReminderTime)
        {
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var currentTime = DateTimeOffset.Now.ToTargetDateTimeOffset(entity.Job.TimeZoneName);
                    bool isExpired = false;
                    if (entity.ExpirationDate != null)
                    {
                        if (entity.ExpirationDate.Value.Date < currentTime.Date)
                        {
                            isExpired = true;
                        }
                        else if (entity.ExpirationDate.Value.Date.Subtract(currentTime.Date).TotalHours < 24)
                        {
                            await GoingToExpireNewsfeed(entity);
                        }

                        if (!isExpired && entity.ExpirationDate.Value.Date.Subtract(currentTime.Date).TotalHours < FRExpirationReminderTime)
                        {
                            await FREmailToSuperAdmin(entity, EventType.FuelRequestToExpireWithExpirationDate);
                        }
                    }
                    else
                    {
                        var dateNeeded = entity.FuelRequestDetail.StartDate.Add(entity.FuelRequestDetail.EndTime);
                        if (dateNeeded.DateTime < currentTime.DateTime)
                        {
                            isExpired = true;
                        }
                        else if (dateNeeded.DateTime.Date.Subtract(currentTime.DateTime.Date).TotalHours < 24)
                        {
                            await GoingToExpireNewsfeed(entity);
                        }

                        if (!isExpired && dateNeeded.DateTime.Subtract(currentTime.DateTime).TotalHours < FRExpirationReminderTime)
                        {
                            await FREmailToSuperAdmin(entity, EventType.FuelRequestToExpireWithOrderStartDate);
                        }
                    }
                    if (isExpired)
                    {
                        entity.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                        FuelRequestXStatus frStatus = new FuelRequestXStatus();
                        frStatus.StatusId = (int)FuelRequestStatus.Expired;
                        frStatus.IsActive = true;
                        frStatus.UpdatedBy = (int)SystemUser.System;
                        frStatus.UpdatedDate = DateTimeOffset.Now;
                        entity.FuelRequestXStatuses.Add(frStatus);

                        Context.DataContext.Entry(entity).State = EntityState.Modified;
                        await Context.CommitAsync();

                        transaction.Commit();

                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetSystemFuelRequestExpiredNewsfeed(entity);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelRequestDomain", "ProcessFuelRequestExpirationAsync", ex.Message, ex);
                }
            }
        }

        public async Task ProcessNewIncomingFuelRequestAsync(FuelRequest entity)
        {
            try
            {
                var currentTime = DateTimeOffset.Now;
                if (currentTime.DateTime.Subtract(entity.CreatedDate.DateTime).TotalHours >= 4)
                {
                    await FREmailToSuperAdmin(entity, EventType.NewIncomingFuelRequest);
                }
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("FuelRequestDomain", "ProcessNewIncomingFuelRequestAsync", ex.Message, ex);
            }
        }

        private async Task GoingToExpireNewsfeed(FuelRequest fuelRequest)
        {
            if (!(goingToExpireNotifiedFRIds.Contains(fuelRequest.Id)))
            {
                if (!(Context.DataContext.Newsfeeds.Any(t => t.EventId == (int)NewsfeedEvent.FuelRequestExpireSoon && t.TargetEntityId == fuelRequest.Id)))
                {
                    goingToExpireNotifiedFRIds.Add(fuelRequest.Id);
                    await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetSystemFuelRequestGoingToExpiredNewsfeed(fuelRequest);
                }
            }
        }

        private async Task FREmailToSuperAdmin(FuelRequest fuelRequest, EventType eventTypeId)
        {
            if (!Context.DataContext.Notifications.Any(t => t.EventTypeId == (int)eventTypeId && t.EntityId == fuelRequest.Id))
            {
                await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(eventTypeId, fuelRequest.Id, (int)SystemUser.System);
            }
        }

        public FuelRequestFilterViewModel GetFuelRequestFilter(int jobId, FuelRequestFilterType filter, string groupIds = "")
        {
            var response = new FuelRequestFilterViewModel();
            try
            {
                response.JobId = jobId;
                response.GroupIds = groupIds;
                if (filter > 0)
                {
                    response.Filter = filter;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetFuelRequestFilterAsync", ex.Message, ex);
            }
            return response;
        }

        public MasterDataViewModel GetMasterData(int companyId = 0)
        {
            var response = new MasterDataViewModel();
            MasterDomain masterDomain = new MasterDomain(this);
            StoredProcedureDomain spDomain = new StoredProcedureDomain(this);
            var pricingDomain = new ExternalPricingDomain(this);
            try
            {
                response.ProductTypes = masterDomain.GetProductTypes();
                response.ProductDisplayGroups = masterDomain.GetProductDisplayGroups(companyId);
                response.FuelTypes = spDomain.GetFuelProducts();
                response.OpisFuelTypes = pricingDomain.GetSourceBasedFuelProducts(PricingSource.OPIS);
                response.PlattsFuelTypes = pricingDomain.GetSourceBasedFuelProducts(PricingSource.PLATTS);
                response.RackAvgPricingTypes = masterDomain.GetRackAvgPricingTypes();
                response.DeliveryScheduleTypes = masterDomain.GetMstScheduleTypes();
                response.SupplierQualifications = masterDomain.GetSupplierQualifications();
                response.PaymentTerms = masterDomain.GetPaymentTerms();
                response.OverWaterFeeTypes = masterDomain.GetFeeSubTypes((int)FeeType.OverWaterFee);
                response.WetHoseFeeTypes = masterDomain.GetFeeSubTypes((int)FeeType.WetHoseFee);
                response.DryRunFeeTypes = masterDomain.GetFeeSubTypes((int)FeeType.DryRunFee);
                response.DeliveryFeeTypes = masterDomain.GetFeeSubTypes((int)FeeType.DeliveryFee);
                response.AdditionalFeeTypes = masterDomain.GetAdditionalFeeSubTypes((int)FeeType.AdditionalFee);
                response.UnderGallonFeeTypes = masterDomain.GetFeeSubTypes((int)FeeType.UnderGallonFee);
                response.FeeTypes = masterDomain.GetFeeTypes();
                response.WeekDays = masterDomain.GetMstWeekDays();
                response.DeliveryTypes = masterDomain.GetDeliveryTypes();

                response.States = masterDomain.GetStatesEx();
                response.Countries = masterDomain.GetCountriesEx();
                response.PricingTypes = masterDomain.GetPricingTypes();
                response.DeliveryScheduleStatuses = masterDomain.GetDeliveryScheduleStatuses();
                response.JobStatuses = masterDomain.GetJobStatuses();
                response.OrderStatuses = masterDomain.GetOrderStatuses();
                response.OrderTypes = masterDomain.GetOrderTypes();
                response.FuelRequestStatuses = masterDomain.GetFuelRequestStatuses();
                response.QuantityTypes = masterDomain.GetQuantityTypes();
                response.InvoiceDeclineReasons = masterDomain.GetInvoiceDeclineReasons();
                response.FeeConstraintTypes = masterDomain.GetAllFeeConstraintTypes();
                response.StateList = masterDomain.GetStateList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetMasterDataAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateOpenFuelRequestAsync(FuelRequestViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var fuelRequest = await Context.DataContext.FuelRequests.SingleOrDefaultAsync(t => t.Id == viewModel.Id);
                    if (fuelRequest != null && fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).StatusId == (int)FuelRequestStatus.Open)
                    {
                        bool isAlertSent = fuelRequest.IsPublicRequest;
                        List<int> companiesForNotification = null;
                        fuelRequest.IsPublicRequest = viewModel.FuelOfferDetails.PrivateSupplierList.IsPublicRequest;
                        if (!viewModel.FuelOfferDetails.PrivateSupplierList.IsPublicRequest)
                        {
                            var existingEvent = Context.DataContext.Notifications.Where(t => (t.EventTypeId == (int)EventType.FuelRequestCreated
                                                        || t.EventTypeId == (int)EventType.FuelRequestUpdated)
                                                        && t.EntityId == fuelRequest.Id)
                                            .OrderByDescending(t => t.Id)
                                            .FirstOrDefault();
                            if (existingEvent != null && existingEvent.Companies.Count == 0)
                            {
                                isAlertSent = true;
                            }
                            SetPrivateListToFuelRequest(viewModel.FuelOfferDetails.PrivateSupplierList.PrivateSupplierIds, fuelRequest, out companiesForNotification);
                        }
                        else
                        {
                            fuelRequest.PrivateSupplierLists.Clear();
                        }

                        fuelRequest.UpdatedBy = viewModel.FuelDetails.UpdatedBy;
                        fuelRequest.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.Entry(fuelRequest).State = EntityState.Modified;

                        await Context.CommitAsync();
                        transaction.Commit();

                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetFuelRequestCreatedNewsfeed(fuelRequest.CreatedBy, fuelRequest, true);


                        if (!isAlertSent)
                        {
                            await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(
                                                                                        EventType.FuelRequestUpdated,
                                                                                        fuelRequest.Id,
                                                                                        fuelRequest.UpdatedBy,
                                                                                        companiesForNotification);
                        }
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageUpdateFuelRequestSuccess;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusMessage = Resource.errMessageUpdateFuelRequestFailed;
                    LogManager.Logger.WriteException("FuelRequestDomain", "UpdateOpenFuelRequestAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateOpenBrokerFuelRequestAsync(BrokerFuelRequestViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var fuelRequest = await Context.DataContext.FuelRequests.SingleOrDefaultAsync(t => t.Id == viewModel.Details.FuelDeliveryDetails.FuelRequestId);
                    if (fuelRequest != null && fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).StatusId == (int)FuelRequestStatus.Open)
                    {
                        bool isAlertSent = fuelRequest.IsPublicRequest;
                        List<int> companiesForNotification = null;
                        if (!viewModel.Details.PrivateSupplierList.IsPublicRequest)
                        {
                            var existingEvent = Context.DataContext.Notifications.Where(t => (t.EventTypeId == (int)EventType.BrokerFuelRequestCreated
                                                                                    || t.EventTypeId == (int)EventType.BrokerFuelRequestUpdated)
                                                                                    && t.EntityId == fuelRequest.Id)
                                                                        .OrderByDescending(t => t.Id)
                                                                        .FirstOrDefault();
                            if (existingEvent != null && existingEvent.Companies.Count == 0)
                            {
                                isAlertSent = true;
                            }
                            fuelRequest.IsPublicRequest = false;
                            SetPrivateListToFuelRequest(viewModel.Details.PrivateSupplierList.PrivateSupplierIds, fuelRequest, out companiesForNotification);
                        }
                        else
                        {
                            fuelRequest.IsPublicRequest = true;
                            fuelRequest.PrivateSupplierLists.Clear();
                        }
                        fuelRequest.UpdatedBy = viewModel.UpdatedBy;
                        fuelRequest.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.Entry(fuelRequest).State = EntityState.Modified;

                        await Context.CommitAsync();
                        transaction.Commit();

                        if (!isAlertSent)
                        {
                            await ContextFactory.Current.GetDomain<NotificationDomain>()
                                                        .AddNotificationEventAsync(
                                                            EventType.BrokerFuelRequestUpdated,
                                                            fuelRequest.Id,
                                                            fuelRequest.UpdatedBy,
                                                            companiesForNotification);
                        }
                    }

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageCreateBrokeredFuelRequestSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusMessage = Resource.errMessageUpdateBrokerFuelRequestFailed;
                    LogManager.Logger.WriteException("FuelRequestDomain", "UpdateOpenBrokerFuelRequestAsync", ex.Message, ex);
                }
            }
            return response;
        }

        private void setSelectedFavoriteFuelType(FuelRequestViewModel viewModel, int companyId)
        {
            if (companyId > 0)
            {
                var favoriteFuels = Context.DataContext.CompanyFavoriteFuels.Where(t => t.CompanyId == companyId && t.RemovedBy == null).Select(t => t.FuelTypeId).ToList();
                if (favoriteFuels.Count > 0 || (viewModel.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.FavoriteFuelType && viewModel.FuelDetails.FuelTypeId > 0))
                {
                    if (viewModel.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.FavoriteFuelType && viewModel.FuelDetails.FuelTypeId != 0 &&
                        !favoriteFuels.Any(t => t == viewModel.FuelDetails.FuelTypeId))
                    {
                        var product = Context.DataContext.MstTfxProducts.FirstOrDefault(t => t.Id == viewModel.FuelDetails.FuelTypeId);
                        if (product != null)
                        {
                            viewModel.FuelDetails.FuelDisplayGroupId = product.ProductDisplayGroupId;
                        }
                    }
                }
                else if (viewModel.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.FavoriteFuelType && (viewModel.FuelDetails.FuelTypeId == null || viewModel.FuelDetails.FuelTypeId == 0))
                    viewModel.FuelDetails.FuelDisplayGroupId = (int)ProductDisplayGroups.FuelTypesInYourArea;
            }
        }

        private async Task UpdateNotifications(FuelRequest fuelRequest, List<int> companiesForNotification)
        {
            //Add an entry to notifications
            if (fuelRequest.FuelRequestTypeId == (int)FuelRequestType.FuelRequest &&
                fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).StatusId != (int)FuelRequestStatus.Draft)
            {
                await ContextFactory.Current.GetDomain<NotificationDomain>()
                                            .AddNotificationEventAsync(
                                                EventType.FuelRequestCreated,
                                                fuelRequest.Id,
                                                fuelRequest.CreatedBy,
                                                companiesForNotification);
            }
            else if (fuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest)
            {
                await ContextFactory.Current.GetDomain<NotificationDomain>()
                                            .AddNotificationEventAsync(
                                                EventType.CounterOfferCreated,
                                                fuelRequest.Id,
                                                fuelRequest.CreatedBy);
            }
        }

        private void AddToFavoriteFuels(int companyId, int userId, int fuelId)
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

        public bool GetFuelRequestExpiringIn7DaysStatus(string timeZoneName, DateTimeOffset? expirationDate, DateTimeOffset startDate, TimeSpan endTime)
        {
            bool isExpiring = false;
            try
            {
                var currentTime = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
                if (expirationDate != null)
                {
                    if (expirationDate.Value.Date < currentTime.Date)
                    {
                        // already expired
                        isExpiring = false;
                    }
                    else if (expirationDate.Value.Date < currentTime.Date.AddDays(7) && (expirationDate.Value.Date - currentTime.Date).TotalHours <= ApplicationConstants.FuelRequestExpiringDefaultHours) // expiring in 7 days
                    {
                        isExpiring = true;
                    }
                }
                else
                {
                    var dateNeeded = startDate.Add(endTime);
                    if (dateNeeded.DateTime < currentTime.DateTime)
                    {
                        isExpiring = false;
                    }
                    else if ((dateNeeded.DateTime - currentTime.DateTime).TotalHours < ApplicationConstants.FuelRequestExpiringDefaultHours)
                    {
                        isExpiring = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetFuelRequestExpiringIn7DaysStatus", ex.Message, ex);
            }

            return isExpiring;
        }

        public List<FeesViewModel> GetFuelRequestFees(int fuelRequestId)
        {
            var fuelRequestFees = new List<FeesViewModel>();
            var fuelRequest = Context.DataContext.FuelRequests.Include(t => t.FuelRequestFees).FirstOrDefault(t => t.Id == fuelRequestId);
            if (fuelRequest != null)
            {
                fuelRequestFees.AddRange(fuelRequest.FuelRequestFees.ToFeesViewModel());
                //var deliveryFeeByQuantity = fuelRequest.FeeByQuantities.Select(t => t.ToViewModel()).ToList();
                //foreach (var item in fuelRequestFees)
                //{
                //    var range = deliveryFeeByQuantity.Where(t => t.FeeTypeId.ToString() == item.FeeTypeId && t.FeeSubTypeId == item.FeeSubTypeId);
                //    item.DeliveryFeeByQuantity.AddRange(range);
                //}
            }
            return fuelRequestFees;
        }

        public List<FeesViewModel> ToFuelRequestFeesViewModel(FuelRequestFeeViewModel viewModel)
        {
            var response = new List<FeesViewModel>();

            //Delivery Fee
            if (viewModel.DeliveryFeeSubTypeId != (int)FeeSubType.NoFee)
            {
                FeesViewModel feeViewModel = new FeesViewModel();
                feeViewModel.FeeTypeId = viewModel.DeliveryFeeTypeId.ToString();
                feeViewModel.Fee = viewModel.DeliveryFeeSubTypeId == (int)FeeSubType.NoFee ? 0 : viewModel.DeliveryFee;
                feeViewModel.FeeSubTypeId = viewModel.DeliveryFeeSubTypeId;
                feeViewModel.IncludeInPPG = viewModel.DeliveryFeeIncludeInPPG;
                response.Add(feeViewModel);
            }

            //WetHose Fee
            if (viewModel.WetHoseFeeSubTypeId != (int)FeeSubType.NoFee)
            {
                FeesViewModel feeViewModel = new FeesViewModel();
                feeViewModel.FeeTypeId = viewModel.WetHoseFeeTypeId.ToString();
                feeViewModel.Fee = viewModel.WetHoseFeeSubTypeId == (int)FeeSubType.NoFee ? 0 : viewModel.WetHoseFee;
                feeViewModel.FeeSubTypeId = viewModel.WetHoseFeeSubTypeId;
                feeViewModel.IncludeInPPG = viewModel.WetHoseFeeIncludeInPPG;
                response.Add(feeViewModel);
            }

            //OverWater Fee
            if (viewModel.OverWaterFeeSubTypeId != (int)FeeSubType.NoFee)
            {
                FeesViewModel feeViewModel = new FeesViewModel();
                feeViewModel.FeeTypeId = viewModel.OverWaterFeeTypeId.ToString();
                feeViewModel.Fee = viewModel.OverWaterFeeSubTypeId == (int)FeeSubType.NoFee ? 0 : viewModel.OverWaterFee;
                feeViewModel.FeeSubTypeId = viewModel.OverWaterFeeSubTypeId;
                feeViewModel.IncludeInPPG = viewModel.OverWaterFeeIncludeInPPG;
                response.Add(feeViewModel);
            }

            //DryRun Fee
            if (viewModel.DryRunFeeSubTypeId != (int)FeeSubType.NoFee)
            {
                FeesViewModel feeViewModel = new FeesViewModel();
                feeViewModel.FeeTypeId = viewModel.DryRunFeeTypeId.ToString();
                feeViewModel.Fee = viewModel.DryRunFeeSubTypeId == (int)FeeSubType.NoFee ? 0 : viewModel.DryRunFee;
                feeViewModel.FeeSubTypeId = viewModel.DryRunFeeSubTypeId;
                feeViewModel.IncludeInPPG = viewModel.DryRunFeeIncludeInPPG;
                response.Add(feeViewModel);
            }

            //Under Gallon Fee
            if (viewModel.UnderGallonFeeSubTypeId != (int)FeeSubType.NoFee)
            {
                FeesViewModel feeViewModel = new FeesViewModel();
                feeViewModel.FeeTypeId = viewModel.UnderGallonFeeTypeId.ToString();
                feeViewModel.Fee = viewModel.UnderGallonFeeSubTypeId == (int)FeeSubType.NoFee ? 0 : viewModel.UnderGallonFee ?? 0;
                feeViewModel.FeeSubTypeId = viewModel.UnderGallonFeeSubTypeId;
                feeViewModel.IncludeInPPG = viewModel.UnderGallonFeeIncludeInPPG;
                feeViewModel.MinimumGallons = viewModel.MinimumGallons;
                response.Add(feeViewModel);
            }

            if (viewModel.AdditionalFee != null)
            {
                List<AdditionalFeeViewModel> additionalFees = viewModel.AdditionalFee;
                var additionalFeeMapping = new Dictionary<int, int>() { { 6, 14 }, { 7, 9 }, { 8, 10 }, { 9, 11 }, { 10, 12 }, { 11, 13 } };
                foreach (AdditionalFeeViewModel additionalFee in additionalFees)
                {
                    var mappedFeeTypeId = additionalFeeMapping[additionalFee.FeeSubTypeId];
                    FeesViewModel feeViewModel = new FeesViewModel();
                    feeViewModel.FeeTypeId = mappedFeeTypeId.ToString();
                    feeViewModel.Fee = additionalFee.Fee;
                    feeViewModel.OtherFeeDescription = mappedFeeTypeId == (int)FeeType.OtherFee ? "Other Fee" : additionalFee.FeeDetails;
                    feeViewModel.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeViewModel.IncludeInPPG = additionalFee.IncludeInPPG;
                    feeViewModel.CommonFee = mappedFeeTypeId != (int)FeeType.OtherFee;
                    response.Add(feeViewModel);
                }
            }

            return response;
        }

        protected async Task<List<UspGetFuelRequestFeeDetailViewModel>> GetFuelRequestFeeDetailsAsync(int fuelrequestId)
        {
            StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
            return await storedProcedureDomain.GetFuelRequestFeeDetailsAsync(fuelrequestId);
        }

        protected async Task<List<SpecialInstructionViewModel>> GetFuelRequestSpecialInstructions(int fuelrequestId)
        {
            StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
            return await storedProcedureDomain.GetFuelRequestSpecialInstructionsAsync(fuelrequestId);
        }


        public PricingRequestDetailResponseViewModel GetRequestPricingDetail(int requestPriceDetailId, int? fuelTypeId = null, int? stateId = null, int currency = (int)Currency.USD)
        {
            var pricingDetails = new PricingRequestDetailResponseViewModel();
            try
            {
                var request = new PricingDetailRequestViewModel { Id = requestPriceDetailId, FuelTypeId = fuelTypeId, StateId = stateId, Currency = currency };
                var result = Task.Run(() => new PricingServiceDomain(this).GetPricingRequestDetailByIdAsync(request)).Result;
                if (result != null && result.Status == Status.Success)
                {
                    pricingDetails = result.PricingRequestDetail;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetRequestPricingDetail", ex.Message, ex);
            }
            return pricingDetails;
        }

        public void AddQueueMsgToCloseFreightOnlyOrder(CloseFreightOnlyOrderQueueMsg queueMsg)
        {
            var queueDomain = new QueueMessageDomain(this);
            var queueRequest = GetQueueMsgToCloseFreightOnlyOrder(queueMsg);
            var queueId = queueDomain.EnqeueMessage(queueRequest);
        }

        private QueueMessageViewModel GetQueueMsgToCloseFreightOnlyOrder(CloseFreightOnlyOrderQueueMsg queueMsg)
        {
            var jsonViewModel = new CloseFreightOnlyOrderQueueMsg();
            jsonViewModel.CarrierCompanyId = queueMsg.CarrierCompanyId;
            jsonViewModel.SupplierCompanyId = queueMsg.SupplierCompanyId;
            jsonViewModel.OrderIds = queueMsg.OrderIds;
            jsonViewModel.SupplierUserId = queueMsg.SupplierUserId;
            string json = JsonConvert.SerializeObject(jsonViewModel);

            return new QueueMessageViewModel()
            {
                CreatedBy = queueMsg.SupplierCompanyId,
                QueueProcessType = QueueProcessType.CloseFreightOnlyOrder,
                JsonMessage = json
            };
        }

        public void SetTierPricingResetCommulation(CumulationSetting viewModel,Job job) {
           
                var jobTime = DateTimeOffset.Now.ToTargetDateTimeOffset(job.TimeZoneName);
                var currentDate = jobTime.Date;
                if (viewModel.CumulationType == CumulationType.Weekly)
                {
                    int daysToAdd = ((int)(WeekDay)viewModel.Day - (int)currentDate.DayOfWeek + 7) % 7;
                    if (daysToAdd == 0)
                    {
                        daysToAdd = 7;
                    }
                  viewModel.Date = currentDate.AddDays(daysToAdd);
                }
                else if (viewModel.CumulationType == CumulationType.BiWeekly)
                {
                    currentDate = currentDate.AddDays(7);
                    int daysToAdd = ((int)(WeekDay)viewModel.Day - (int)currentDate.DayOfWeek + 7) % 7;
                    if (daysToAdd == 0)
                    {
                        daysToAdd = 7;
                    }
                    viewModel.Date = currentDate.AddDays(daysToAdd);
                }
                else
                {
                    HelperDomain helperDomain = new HelperDomain(this);
                    viewModel.Day = (WeekDay)helperDomain.GetWeekDayId(viewModel.Date ?? currentDate);
                }
            
        }

        private async Task<CustomResponseModel> initializeFuelPricingDetails(FuelRequestViewModel viewModel,FuelRequest fuelRequest,Job job)
        {
            ExternalPricingDomain externalPricingDomain = new ExternalPricingDomain(this);
            if (viewModel.FuelDetails.IsTierPricing)
            {
                //initialize Tier Pricing Details
                viewModel.FuelDetails.FuelPricing.TierPricing = viewModel.FuelDetails.TierPricing;
                viewModel.FuelDetails.FuelPricing.PricingTypeId = (int)PricingType.Tier;

                int i = 0;
                var terminalSourceDtls = new Dictionary<int, int>(); // fueltypeID,terminalid
                foreach (var item in viewModel.FuelDetails.FuelPricing.TierPricing.Pricings)
                {
                    //set  Fuel  type Id for multiple tier
                   
                    if (viewModel.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
                    {
                        item.FuelTypeId = viewModel.FuelDetails.FuelTypeId.Value;
                    }else
                    item.FuelTypeId = GetFuelTypeId(item.FuelTypeId.Value, item.PricingSourceId, item.PricingTypeId);
                    //set Tier Terminal
                    if (terminalSourceDtls.Keys.FirstOrDefault(w => w == item.FuelTypeId.Value) > 0)
                    {
                        item.TerminalId = terminalSourceDtls.FirstOrDefault(w => w.Key == item.FuelTypeId.Value).Value;
                    }
                    else
                    {
                        await externalPricingDomain.GetTerminalWithPriceForTierItems(item, job, fuelRequest.Currency);
                        if (item.TerminalId != null)
                            terminalSourceDtls.Add(item.FuelTypeId.Value, item.TerminalId.Value);
                        if (i == 0)
                        {
                            //set default terminal id in case of multiple tier in fuel request table
                            fuelRequest.TerminalId = item.TerminalId;
                            fuelRequest.CreationTimeRackPPG = item.CreationTimeRackPPG ?? 0;
                            fuelRequest.CreationTimeRackPPG = new CurrencyRateDomain().Convert(fuelRequest.Currency, job.Currency, fuelRequest.CreationTimeRackPPG, DateTimeOffset.Now);
                        }
                    }
                    i++;
                }

                //Reset cumulation

                if (viewModel.FuelDetails.FuelPricing.TierPricing.IsResetCumulation && viewModel.FuelDetails.FuelPricing.TierPricing.TierPricingType == TierPricingType.VolumeBased)
                {
                    SetTierPricingResetCommulation(viewModel.FuelDetails.FuelPricing.TierPricing.ResetCumulationSetting, job);
                }

                //set default fuel type id in fuel request table
                viewModel.FuelDetails.FuelPricing.FuelTypeId = viewModel.FuelDetails.FuelPricing.TierPricing.Pricings.FirstOrDefault().FuelTypeId;

                //set default TerminalId in fuel request table
                fuelRequest.TerminalId = viewModel.FuelDetails.FuelPricing.TierPricing.Pricings.FirstOrDefault().TerminalId;

                //set default CreationTimeRackPPG in fuel request table
                fuelRequest.CreationTimeRackPPG = viewModel.FuelDetails.FuelPricing.TierPricing.Pricings.FirstOrDefault().CreationTimeRackPPG ?? 0;
            }
            else  // non tier
            {
                if (viewModel.FuelDetails.FuelDisplayGroupId != (int)ProductDisplayGroups.OtherFuelType)
                {
                    viewModel.FuelDetails.FuelPricing.FuelTypeId = GetFuelTypeId(viewModel.FuelDetails.FuelTypeId.Value, viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId, viewModel.FuelDetails.FuelPricing.PricingTypeId);
                }
                else
                {
                    viewModel.FuelDetails.FuelPricing.FuelTypeId = viewModel.FuelDetails.FuelTypeId;
                }
              
                fuelRequest.FuelTypeId = viewModel.FuelDetails.FuelPricing.FuelTypeId.Value;
                await externalPricingDomain.GetTerminalWithPrice(viewModel.FuelDetails.FuelPricing.FuelPricingDetails, fuelRequest, job.Id, fuelRequest.Currency);
                viewModel.FuelDetails.FuelPricing.TerminalId = fuelRequest.TerminalId;
            }
           
             fuelRequest.FuelTypeId = viewModel.FuelDetails.FuelPricing.FuelTypeId.Value;

            //NEED TO DISCUSS WITH TEAM
            return await new PricingServiceDomain().SavePricingDetails(viewModel.FuelDetails.FuelPricing, viewModel.FuelDetails.FuelQuantity.UoM);
           
        }

        private async Task<CustomResponseModel> initializeBrokerFuelPricingDetails(BrokerFuelRequestViewModel viewModel, FuelRequest fuelRequest, Job job)
        {
            ExternalPricingDomain externalPricingDomain = new ExternalPricingDomain(this);
            if (viewModel.Details.FuelPricing.IsTierPricing)
            {
                //initialize Tier Pricing Details
               // viewModel.Details.FuelPricing.TierPricing = viewModel.Details.TierPricing;
                viewModel.Details.FuelPricing.PricingTypeId = (int)PricingType.Tier;

                int i = 0;
                var terminalSourceDtls = new Dictionary<int, int>(); // fueltypeID,terminalid
                foreach (var item in viewModel.Details.FuelPricing.TierPricing.Pricings)
                {
                    //set  Fuel  type Id for multiple tier

                    if (viewModel.Details.FuelDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
                    {
                        item.FuelTypeId = viewModel.Details.FuelTypeId;
                    }
                    else
                        item.FuelTypeId = GetFuelTypeId(viewModel.Details.FuelTypeId, item.PricingSourceId, item.PricingTypeId);
                    //set Tier Terminal
                    if (terminalSourceDtls.Keys.FirstOrDefault(w => w == item.FuelTypeId.Value) > 0)
                    {
                        item.TerminalId = terminalSourceDtls.FirstOrDefault(w => w.Key == item.FuelTypeId.Value).Value;
                    }
                    else
                    {
                        await externalPricingDomain.GetTerminalWithPriceForTierItems(item, job, fuelRequest.Currency);
                        if (item.TerminalId != null)
                            terminalSourceDtls.Add(item.FuelTypeId.Value, item.TerminalId.Value);
                        if (i == 0)
                        {
                            //set default terminal id in case of multiple tier in fuel request table
                            fuelRequest.TerminalId = item.TerminalId;
                            fuelRequest.CreationTimeRackPPG = item.CreationTimeRackPPG ?? 0;
                            fuelRequest.CreationTimeRackPPG = new CurrencyRateDomain().Convert(fuelRequest.Currency, job.Currency, fuelRequest.CreationTimeRackPPG, DateTimeOffset.Now);
                            fuelRequest.FuelRequestPricingDetail.PricingCode = item.PricingCode.Code;
                            fuelRequest.FuelRequestPricingDetail.PricingCodeId = item.PricingCode.Id;
;                        }
                    }
                    i++;
                }

                //Reset cumulation

                if (viewModel.Details.FuelPricing.TierPricing.IsResetCumulation && viewModel.Details.FuelPricing.TierPricing.TierPricingType == TierPricingType.VolumeBased)
                {
                    SetTierPricingResetCommulation(viewModel.Details.FuelPricing.TierPricing.ResetCumulationSetting, job);
                }

                //set default fuel type id in fuel request table
                viewModel.Details.FuelTypeId = viewModel.Details.FuelPricing.TierPricing.Pricings.FirstOrDefault().FuelTypeId.Value;

                //set default TerminalId in fuel request table
                fuelRequest.TerminalId = viewModel.Details.FuelPricing.TierPricing.Pricings.FirstOrDefault().TerminalId;

                //set default CreationTimeRackPPG in fuel request table
                fuelRequest.CreationTimeRackPPG = viewModel.Details.FuelPricing.TierPricing.Pricings.FirstOrDefault().CreationTimeRackPPG ?? 0;
            }
            else  // non tier
            {
                if (viewModel.Details.FuelDisplayGroupId != (int)ProductDisplayGroups.OtherFuelType && viewModel.Details.FuelDisplayGroupId != (int)ProductDisplayGroups.AdditiveFuelType)
                {
                    viewModel.Details.FuelTypeId = GetFuelTypeId(viewModel.Details.FuelTypeId, viewModel.Details.FuelPricing.FuelPricingDetails.PricingSourceId, viewModel.Details.FuelPricing.PricingTypeId);
                }
                
                fuelRequest.FuelTypeId = viewModel.Details.FuelTypeId;
                await externalPricingDomain.GetTerminalWithPrice(viewModel.Details.FuelPricing.FuelPricingDetails, fuelRequest, job.Id, fuelRequest.Currency);
                viewModel.Details.TerminalId = fuelRequest.TerminalId;
            }

            fuelRequest.FuelTypeId = viewModel.Details.FuelTypeId;

            //NEED TO DISCUSS WITH TEAM
            return await new PricingServiceDomain().SavePricingDetails(null, viewModel.Details.FuelQuantity.UoM, viewModel.Details); 
        }
        

        public async Task<bool> ResetCommulation()
        {
            using (var tracer = new Tracer("FuelRequestDomain", "ResetCommulation"))
            {
                bool response = false;
                try
                {
                    response = await new PricingServiceDomain().ResetCommulation();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("FuelRequestDomain", "ResetCommulation", ex.Message, ex);                    
                }
                return response;
            }                      
        }

        public async Task<int> GetDefaultNominationJobDetails(int companyId)
        {
            var jobId = 0;
            using (var tracer = new Tracer("FuelRequestDomain", "GetDefaultNominationJobDetails"))
            {
                try
                {
                    var fuelRequest = await Context.DataContext.FuelRequests.Where(t => t.IsActive && t.Job.IsMarine && t.Job.CompanyId == companyId).OrderByDescending(t1 => t1.Id).Select(t2 => new { t2.Id, JobId = t2.Job.Id }).FirstOrDefaultAsync();
                    if (fuelRequest != null)
                    {
                        jobId = fuelRequest.JobId;
                    }
                    else
                    {
                        jobId = await Context.DataContext.Jobs.Where(t => t.IsMarine && t.IsActive && t.CompanyId == companyId).OrderByDescending(t1 => t1.Id).Select(t2 => t2.Id).FirstOrDefaultAsync();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("FuelRequestDomain", "GetDefaultNominationJobDetails", ex.Message, ex);
                }
                return jobId;
            }
        }

        public async Task<StatusViewModel> AcknowledgeNomination(int nominationId, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            using (var tracer = new Tracer("FuelRequestDomain", "AcknowledgeNomination"))
            {
                try
                {
                    // check if nomination already exists
                    var isNominationExists = await Context.DataContext.Acknowledgements.AnyAsync(t => t.IsActive && t.EntityId == nominationId && t.UserCompanyId == userContext.CompanyId);
                    if (!isNominationExists)
                    {
                        var entity = new Acknowledgement();
                        entity.EntityId = nominationId;
                        entity.IsSent = false;
                        entity.UserId = userContext.Id;
                        entity.UserCompanyId = userContext.CompanyId;
                        entity.IsActive = true;
                        entity.UpdatedBy = userContext.Id;
                        entity.UpdatedDate = DateTimeOffset.Now;

                        Context.DataContext.Acknowledgements.Add(entity);
                        await Context.CommitAsync();

                        // Add notification
                        await ContextFactory.Current.GetDomain<NotificationDomain>()
                                            .AddNotificationEventAsync(
                                                EventType.NominationAcknowledgement,
                                                nominationId,
                                                userContext.Id);

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.successMessageAcknowledgeProcess;
                    }
                    else
                    {
                        response.StatusMessage = Resource.errorMessageAcknowledgementAlreadySent;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Status.Failed.ToString();
                    LogManager.Logger.WriteException("FuelRequestDomain", "AcknowledgeNomination", "NominationId => " + nominationId + ". " + ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<FuelRequestResponseModel> GetFuelRequestsAsBuyer(string token, string fromDate = "", string toDate = "", int userId = 0, FuelRequestType type = FuelRequestType.All)
        {
            var response = new FuelRequestResponseModel();
            try
            {
                var authDomain = new AuthenticationDomain(this);
                var apiUserContext = Task.Run(() => authDomain.GetUserContextFromTokenAsync(token)).Result;
                if (apiUserContext != null)
                {
                    if (apiUserContext.CompanyTypeId == CompanyType.Buyer || apiUserContext.CompanyTypeId == CompanyType.BuyerAndSupplier || apiUserContext.CompanyTypeId == CompanyType.BuyerSupplierAndCarrier || apiUserContext.CompanyTypeId == CompanyType.Supplier || apiUserContext.CompanyTypeId == CompanyType.Carrier || apiUserContext.CompanyTypeId == CompanyType.SupplierAndCarrier)
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        var result = await spDomain.GetFuelRequestsAsBuyer(apiUserContext.CompanyId, userId, fromDate, toDate, type);

                        response.StatusCode = Status.Success;
                        if (result != null && result.Any())
                        {
                            var ids = result.Select(t => t.RequestPriceDetailId).ToList();
                            var pricingDtl = await new PricingServiceDomain(this).GetPricingDetailsByIdList(ids);
                            if (pricingDtl == null || pricingDtl.PricingDetails == null || !pricingDtl.PricingDetails.Any())
                            {
                                response.ResponseData = result;
                                response.StatusMessage = Resource.ErrorPricingDetails;//change msg
                                response.StatusCode = Status.Failed;
                                return response;
                            }
                            else
                            {
                                foreach (var item in result)
                                {
                                    item.Pricing = new List<PricingModel>();
                                    var pricingList = pricingDtl.PricingDetails.Where(t => t.RequestPriceDetailId == item.RequestPriceDetailId).ToList();
                                    if(pricingList != null && pricingList.Any())
                                    {
                                        foreach (var pricing in pricingList)
                                        {
                                            var model = new PricingModel();
                                            model.RequestPriceDetailId = item.RequestPriceDetailId;
                                            model.Pricing = pricing.PricePerGallon;
                                            model.DisplayPrice = item.DisplayPrice;
                                            model.MinQuantity = pricing.MinQuantity;
                                            model.MaxQuantity = pricing.MaxQuantity;
                                            model.PricingCode = pricing.PricingCode;
                                            model.PricingSource = item.PricingSource;
                                            model.DisplayPricingCode = item.DisplayPriceCode;

                                            item.Pricing.Add(model);
                                        }
                                    }
                                }
                            }

                            response.ResponseData = result;
                            response.StatusMessage = Status.Success.ToString();
                        }
                        else
                        {
                            response.ResponseData = new List<FuelRequestModel>();
                            response.StatusMessage = Resource.lblNoDataAvailable;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMsgAccessDenied;
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMsgInvalidToken;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetFuelRequestsAsBuyer", ex.Message, ex);
                response.StatusCode = Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }

            return response;
        }

        public async Task<FuelRequestResponseModel> GetFuelRequestsAsSupplier(string token, string fromDate = "", string toDate = "", int userId = 0, FuelRequestType type = FuelRequestType.All)
        {
            var response = new FuelRequestResponseModel();
            try
            {
                var authDomain = new AuthenticationDomain(this);
                var apiUserContext = Task.Run(() => authDomain.GetUserContextFromTokenAsync(token)).Result;
                if (apiUserContext != null)
                {
                    if (apiUserContext.CompanyTypeId != CompanyType.Buyer)
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        var result = await spDomain.GetFuelRequestsAsSupplier(apiUserContext.CompanyId, userId, fromDate, toDate, type);

                        response.StatusCode = Status.Success;
                        if (result != null && result.Any())
                        {
                            var ids = result.Select(t => t.RequestPriceDetailId).ToList();
                            var pricingDtl = await new PricingServiceDomain(this).GetPricingDetailsByIdList(ids);
                            if (pricingDtl == null || pricingDtl.PricingDetails == null || !pricingDtl.PricingDetails.Any())
                            {
                                response.ResponseData = result;
                                response.StatusMessage = Resource.ErrorPricingDetails;//change msg
                                response.StatusCode = Status.Failed;
                                return response;
                            }
                            else
                            {
                                foreach (var item in result)
                                {
                                    item.Pricing = new List<PricingModel>();
                                    var pricingList = pricingDtl.PricingDetails.Where(t => t.RequestPriceDetailId == item.RequestPriceDetailId).ToList();
                                    if (pricingList != null && pricingList.Any())
                                    {
                                        foreach (var pricing in pricingList)
                                        {
                                            var model = new PricingModel();
                                            model.RequestPriceDetailId = item.RequestPriceDetailId;
                                            model.Pricing = pricing.PricePerGallon;
                                            model.DisplayPrice = item.DisplayPrice;
                                            model.MinQuantity = pricing.MinQuantity;
                                            model.MaxQuantity = pricing.MaxQuantity;
                                            model.PricingCode = pricing.PricingCode;
                                            model.PricingSource = item.PricingSource;
                                            model.DisplayPricingCode = item.DisplayPriceCode;

                                            item.Pricing.Add(model);
                                        }
                                    }
                                }
                            }

                            response.ResponseData = result;
                            response.StatusMessage = Status.Success.ToString();
                        }
                        else
                        {
                            response.ResponseData = new List<FuelRequestModel>();
                            response.StatusMessage = Resource.lblNoDataAvailable;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMsgAccessDenied;
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMsgInvalidToken;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetFuelRequestsAsSupplier", ex.Message, ex);
                response.StatusCode = Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }

            return response;
        }
        public async Task<StatusViewModel> CancelBrokeredFuelRequestAsync(int fuelRequestId, int userId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var fuelRequest = await Context.DataContext.FuelRequests.FirstOrDefaultAsync(t => t.Id == fuelRequestId);
                    if (fuelRequest != null)
                    {
                        fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                        FuelRequestXStatus frStatus = new FuelRequestXStatus();
                        frStatus.StatusId = (int)FuelRequestStatus.Canceled;
                        frStatus.IsActive = true;
                        frStatus.UpdatedBy = userId;
                        frStatus.UpdatedDate = DateTimeOffset.Now;
                        fuelRequest.FuelRequestXStatuses.Add(frStatus);

                        Context.DataContext.Entry(fuelRequest).State = EntityState.Modified;

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageCancelFuelRequestSuccess;

                    }
                    await Context.CommitAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageCancelFuelRequestFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelRequestDomain", "CancelBrokeredFuelRequestAsync", ex.Message, ex);
                }
            }

            return response;
        }
        public async Task<int> GetBrokeredFuelDetailsAsync(int OrderId)
        {
            int fuelRequestId = 0;

            try
            {
                var orderDetails = await Context.DataContext.Orders.FirstOrDefaultAsync(t => t.Id == OrderId && t.IsActive && 
                            t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open);
                if (orderDetails != null)
                {
                    fuelRequestId = orderDetails.FuelRequestId;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetBrokeredFuelDetailsAsync", ex.Message, ex);
            }

            return fuelRequestId;
        }

        public async Task<SourceRegionsViewModel> GetFRDetailsById(int fuelRequestId, int companyId)
        {
            var model = new SourceRegionsViewModel();
            try
            {
                var frDetail = await Context.DataContext.FuelRequests.Where(t => t.Id == fuelRequestId)
                                                         .Select(t => new
                                                         {
                                                             t.JobId,
                                                             FuelTypeId = t.MstProduct.TfxProductId,
                                                             t.Job.Latitude,
                                                             t.Job.Longitude,
                                                             t.Job.CountryId,
                                                             t.Job.Currency,
                                                             t.Job.StateId,
                                                             RequestPriceDetailId = t.FuelRequestPricingDetail != null ? t.FuelRequestPricingDetail.RequestPriceDetailId : 0
                                                         }).FirstOrDefaultAsync();
                if(frDetail != null)
                {
                    var pricingResponse = GetRequestPricingDetail(frDetail.RequestPriceDetailId, frDetail.FuelTypeId, frDetail.StateId, (int)frDetail.Currency);
                    if(pricingResponse != null)
                    {
                        model.JobId = frDetail.JobId;
                        model.FuelTypeId = frDetail.FuelTypeId;
                        model.Latitude = frDetail.Latitude;
                        model.Longitude = frDetail.Longitude;
                        model.CountryId = frDetail.CountryId;
                        model.PricingCodeId = pricingResponse.PricingCodeId;
                        model.PricingSourceId = pricingResponse.PricingSourceId;
                        model.PricingTypeId = pricingResponse.PricingTypeId;

                        var onboardingPreferences = await Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == companyId && t.IsActive)
                                                         .Select(t => new
                                                         {
                                                             t.FreightPricingMethod,
                                                         }).FirstOrDefaultAsync();
                        model.FreightPricingMethod = onboardingPreferences.FreightPricingMethod;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetFRDetailsById", ex.Message, ex);
            }

            return model;
        }
    }
}
