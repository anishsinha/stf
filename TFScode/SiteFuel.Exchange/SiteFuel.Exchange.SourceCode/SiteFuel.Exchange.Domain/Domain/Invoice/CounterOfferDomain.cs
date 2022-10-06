using SiteFuel.Exchange.Core;
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
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class CounterOfferDomain : BaseDomain
    {
        public CounterOfferDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public CounterOfferDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public async Task<CounterOfferViewModel> GetBuyerCounterOfferDetailsAsync(int fuelRequestId, int userId, int supplierId)
        {
            CounterOfferViewModel response = new CounterOfferViewModel();
            try
            {
                CounterOffer latestCounterOffer = null;
                CounterOffer previousCounterOffer = null;
                HelperDomain helperDomain = new HelperDomain(this);
                response.PreviousCounterOfferDetails = null;

                var fuelRequest = await Context.DataContext.FuelRequests.FirstOrDefaultAsync(t => t.Id == fuelRequestId && t.IsActive);
                var supplier = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == supplierId);
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId);
                response.Supplier = $"{supplier.Company.Name}";

                if (fuelRequest != null)
                {
                    response.FuelRequest = fuelRequest.ToViewModel();

                    List<int> counterOffers = await helperDomain.GetCounterOffers(fuelRequest.Id);

                    if (counterOffers != null && counterOffers.Any())
                    {
                        var supplierCounterOffers = Context.DataContext.CounterOffers.Where(x => counterOffers.Contains(x.FuelRequestId)
                                                                                                    && x.User.Company.Id == user.Company.Id
                                                                                                    && x.SupplierId == supplierId
                                                                                                    && x.FuelRequest.IsActive)
                                                                                    .OrderByDescending(t => t.Id)
                                                                                    .ToList();

                        if (supplierCounterOffers != null && supplierCounterOffers.Any())
                        {
                            latestCounterOffer = supplierCounterOffers.FirstOrDefault();
                            response.CurrentCounterOfferDetails = latestCounterOffer.FuelRequest.ToViewModel();
                            if (response.CurrentCounterOfferDetails != null)
                            {
                                response.CurrentCounterOfferDetails.CounterOfferSupplierId = supplierId;
                                if (latestCounterOffer.FuelRequest.User.Company.Id == user.Company.Id)
                                {
                                    response.CurrentCounterOfferDetails.CounterOfferDetails.CreatedBy = Resource.btnLabelBuyer;
                                }
                                else
                                {
                                    response.CurrentCounterOfferDetails.CounterOfferDetails.CreatedBy = Resource.btnLabelSupplier;
                                }
                            }

                            #region Counter offer status & Buttons visibility

                            if (latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Cancelled || latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Cancelled)
                                response.Status = (int)CounterOfferStatus.Cancelled;
                            else if (latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Declined || latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Declined)
                                response.Status = (int)CounterOfferStatus.Declined;
                            else if (latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Accepted || latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Accepted)
                                response.Status = (int)CounterOfferStatus.Accepted;
                            else if (latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Pending)
                                response.Status = (int)CounterOfferStatus.Open;
                            else if (latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Pending)
                                response.Status = (int)CounterOfferStatus.Pending;
                            else if (latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Countered)
                                response.Status = (int)CounterOfferStatus.Pending;
                            else if (latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Countered)
                                response.Status = (int)CounterOfferStatus.Open;

                            if (latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Pending || latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Countered)
                            {
                                response.IsCancelVisible = true;
                                response.IsAcceptVisible = false;
                                response.IsDeclineVisible = false;
                                response.IsCounterOfferVisible = false;
                            }
                            else if (latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Declined)
                            {
                                response.IsCancelVisible = true;
                                response.IsAcceptVisible = false;
                                response.IsDeclineVisible = false;
                                response.IsCounterOfferVisible = true;
                            }
                            else if (latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Pending || latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Countered)
                            {
                                response.IsAcceptVisible = true;
                                response.IsDeclineVisible = true;
                                response.IsCounterOfferVisible = true;
                                response.IsCancelVisible = false;
                            }
                            else if (latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Accepted || latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Declined)
                            {
                                response.IsAcceptVisible = false;
                                response.IsDeclineVisible = false;
                                response.IsCounterOfferVisible = false;
                                response.IsCancelVisible = false;
                            }

                            #endregion

                            if (supplierCounterOffers.Count > 1)
                            {
                                previousCounterOffer = supplierCounterOffers.Skip(1).FirstOrDefault();
                                response.PreviousCounterOfferDetails = previousCounterOffer.FuelRequest.ToViewModel();

                                if (previousCounterOffer.FuelRequest.User.Company.Id == user.Company.Id)
                                {
                                    response.PreviousCounterOfferDetails.CounterOfferDetails.CreatedBy = Resource.btnLabelBuyer;
                                }
                                else
                                {
                                    response.PreviousCounterOfferDetails.CounterOfferDetails.CreatedBy = Resource.btnLabelSupplier;
                                }
                            }
                            else
                            {
                                previousCounterOffer = null;
                                response.PreviousCounterOfferDetails = null;
                            }
                        }
                        else
                        {
                            response.IsCancelVisible = false;
                            response.IsAcceptVisible = false;
                            response.IsDeclineVisible = false;
                            // buyer should not see any button if no counter offer is there
                            // as buyer cannot initiate counter offer
                            response.IsCounterOfferVisible = false;
                            response.CurrentCounterOfferDetails = fuelRequest.ToViewModel();
                        }
                    }

                    int fuelRequestCurrentStatus = fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).StatusId;
                    if (fuelRequestCurrentStatus != (int)FuelRequestStatus.Open)
                    {
                        if (fuelRequestCurrentStatus == (int)FuelRequestStatus.Expired)
                        {
                            response.Status = (int)FuelRequestStatus.Expired;
                        }
                        response.IsCancelVisible = false;
                        response.IsAcceptVisible = false;
                        response.IsDeclineVisible = false;
                        response.IsCounterOfferVisible = false;
                    }

                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;

                    response.FuelRequest.Job.IsTaxExempted = fuelRequest.Job.JobBudget.IsTaxExempted;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CounterOfferDomain", "GetBuyerCounterOfferDetailsAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<CounterOfferViewModel> GetSupplierCounterOfferDetailsAsync(int fuelRequestId, int userId)
        {
            CounterOfferViewModel response = new CounterOfferViewModel();
            try
            {
                CounterOffer latestCounterOffer = null;
                CounterOffer previousCounterOffer = null;
                HelperDomain helperDomain = new HelperDomain(this);
                response.PreviousCounterOfferDetails = null;

                var fuelRequest = await Context.DataContext.FuelRequests.Include(t => t.FuelRequestFees)
                                        .Include("FuelRequestFees.MstFeeType").Include("FuelRequestFees.MstFeeSubType")
                                        .FirstOrDefaultAsync(t => t.Id == fuelRequestId && t.IsActive);
                response.Buyer = $"{fuelRequest.User.Company.Name}";

                if (fuelRequest != null)
                {
                    response.FuelRequest = fuelRequest.ToViewModel();

                    var counterOfferWithStatus = await helperDomain.GetCounterOffersWithStatus(fuelRequest.Id);
                    List<int> counterOffers = counterOfferWithStatus.Select(x => x.Item1).ToList();

                    if (counterOffers != null && counterOffers.Any())
                    {
                        var supplierCounterOffers = Context.DataContext.CounterOffers.Include(t => t.FuelRequest).Include(t => t.FuelRequest.FuelRequestFees)
                                                    .Include("FuelRequest.FuelRequestFees.MstFeeType").Include("FuelRequest.FuelRequestFees.MstFeeSubType")
                                                    .Where
                                                    (
                                                        t => counterOffers.Contains(t.FuelRequestId) &&
                                                        t.SupplierId == userId &&
                                                        t.FuelRequest.IsActive
                                                    )
                                                    .OrderByDescending(t => t.Id).ToList();

                        if (supplierCounterOffers != null && supplierCounterOffers.Any())
                        {
                            latestCounterOffer = supplierCounterOffers.FirstOrDefault();
                            response.CurrentCounterOfferDetails = latestCounterOffer.FuelRequest.ToViewModel();

                            if (latestCounterOffer.FuelRequest.CreatedBy == userId)
                            {
                                response.CurrentCounterOfferDetails.CounterOfferDetails.CreatedBy = Resource.btnLabelSupplier;
                            }
                            else
                            {
                                response.CurrentCounterOfferDetails.CounterOfferDetails.CreatedBy = Resource.btnLabelBuyer;
                            }

                            #region Counter offer status & Buttons visibility
                            var acceptedOffer = counterOfferWithStatus.FirstOrDefault(x => x.Item2 == (int)CounterOfferStatus.Accepted || x.Item3 == (int)CounterOfferStatus.Accepted);
                            if (acceptedOffer != null && acceptedOffer.Item1 != latestCounterOffer.FuelRequestId)
                            {
                                response.Status = (int)CounterOfferStatus.Declined;
                            }

                            else if (latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Cancelled || latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Cancelled)
                                response.Status = (int)CounterOfferStatus.Cancelled;
                            else if (latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Declined || latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Declined)
                                response.Status = (int)CounterOfferStatus.Declined;
                            else if (latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Accepted || latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Accepted)
                                response.Status = (int)CounterOfferStatus.Accepted;
                            else if (latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Pending)
                                response.Status = (int)CounterOfferStatus.Pending;
                            else if (latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Pending)
                                response.Status = (int)CounterOfferStatus.Open;
                            else if (latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Countered)
                                response.Status = (int)CounterOfferStatus.Open;
                            else if (latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Countered)
                                response.Status = (int)CounterOfferStatus.Pending;

                            if (latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Pending || latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Countered)
                            {
                                // latest is countered means - very last CO is inactive and has been cancelled by supplier - actual latest CO is still pending on supplier
                                response.IsCancelVisible = true;
                                response.IsAcceptVisible = false;
                                response.IsDeclineVisible = false;
                                response.IsCounterOfferVisible = false;
                            }
                            else if (latestCounterOffer.BuyerStatus == (int)CounterOfferStatus.Declined)
                            {
                                response.IsCancelVisible = false;
                                response.IsAcceptVisible = false;
                                response.IsDeclineVisible = false;
                                response.IsCounterOfferVisible = true;
                            }
                            else if (latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Declined)
                            {
                                response.IsCancelVisible = false;
                                response.IsAcceptVisible = false;
                                response.IsDeclineVisible = false;
                                response.IsCounterOfferVisible = false;
                            }
                            else if (latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Pending || latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Countered)
                            {
                                // latest is countered means - very last CO is inactive and has been cancelled by supplier - actual latest CO is still pending on supplier
                                response.IsAcceptVisible = true;
                                response.IsDeclineVisible = true;
                                response.IsCounterOfferVisible = true;
                                response.IsCancelVisible = false;
                            }
                            else if (latestCounterOffer.SupplierStatus == (int)CounterOfferStatus.Accepted)
                            {
                                response.IsAcceptVisible = false;
                                response.IsDeclineVisible = false;
                                response.IsCounterOfferVisible = false;
                                response.IsCancelVisible = false;
                            }

                            #endregion

                            if (supplierCounterOffers.Count > 1)
                            {
                                previousCounterOffer = supplierCounterOffers.Skip(1).FirstOrDefault();
                                response.PreviousCounterOfferDetails = previousCounterOffer.FuelRequest.ToViewModel();

                                if (previousCounterOffer.FuelRequest.CreatedBy == userId)
                                {
                                    response.PreviousCounterOfferDetails.CounterOfferDetails.CreatedBy = Resource.btnLabelSupplier;
                                }
                                else
                                {
                                    response.PreviousCounterOfferDetails.CounterOfferDetails.CreatedBy = Resource.btnLabelBuyer;
                                }
                            }
                            else
                            {
                                previousCounterOffer = null;
                                response.PreviousCounterOfferDetails = null;
                            }
                        }
                        else
                        {
                            response.IsCancelVisible = false;
                            response.IsAcceptVisible = false;
                            response.IsDeclineVisible = false;
                            // supplier should see only counter offer button if no counter offer is there
                            // as supplier can initiate counter offer
                            response.IsCounterOfferVisible = true;
                            response.CurrentCounterOfferDetails = fuelRequest.ToViewModel();
                        }
                    }
                    response.FuelRequest.Job.IsTaxExempted = fuelRequest.Job.JobBudget.IsTaxExempted;
                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }

                int fuelRequestCurrentStatus = fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).StatusId;
                if (fuelRequestCurrentStatus != (int)FuelRequestStatus.Open)
                {
                    if (fuelRequestCurrentStatus == (int)FuelRequestStatus.Expired)
                    {
                        response.Status = (int)FuelRequestStatus.Expired;
                    }
                    response.IsCancelVisible = false;
                    response.IsAcceptVisible = false;
                    response.IsDeclineVisible = false;
                    response.IsCounterOfferVisible = false;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CounterOfferDomain", "GetSupplierCounterOfferDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<FuelRequestStatusViewModel> AcceptCounterOfferByBuyerAsync(UserContext userContext, int supplierId, int fuelRequestId)
        {
            FuelRequestStatusViewModel response = new FuelRequestStatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    HelperDomain helperDomain = new HelperDomain(this);

                    var fuelRequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == fuelRequestId && t.IsActive);
                    if (fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).StatusId != (int)FuelRequestStatus.Open)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageAcceptCounterOfferFailedFuelRequestNotOpen;
                        return response;
                    }

                    //check if product is available in region for specific location
                    if (supplierId > 0)
                    {
                        var companyId = Context.DataContext.Users.Where(t => t.Id == supplierId).Select(c => c.CompanyId).FirstOrDefault();
                        if (fuelRequest.FuelTypeId > 0 && fuelRequest.JobId > 0 && companyId.HasValue && companyId.Value > 0)
                        {
                            var tpOrderDomain = new ThirdPartyOrderDomain(this);
                            var isProductValid = await tpOrderDomain.IsValidProductForRegion(fuelRequest.JobId, companyId.Value, null, fuelRequest.FuelTypeId, 0);
                            if (isProductValid.StatusCode == Status.Failed)
                            {
                                response.StatusCode = isProductValid.StatusCode;
                                response.StatusMessage = isProductValid.StatusMessage;
                                return response;
                            }
                        }
                    }
                    var counterOffers = await helperDomain.GetCounterOffers(fuelRequestId);
                    if (counterOffers != null && counterOffers.Any())
                    {
                        var lastCounterOffer = Context.DataContext.CounterOffers.Where(x => counterOffers.Contains(x.FuelRequestId)
                                                                                            && x.SupplierId == supplierId
                                                                                            && x.FuelRequest.IsActive)
                                                                                .OrderByDescending(co => co.Id)
                                                                                .FirstOrDefault();

                        if (lastCounterOffer != null && fuelRequest != null)
                        {
                            // get suppliers information here
                            var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == supplierId);

                            #region Check current Qualifications of supplier
                            bool isSupplierQualified = false;
                            foreach (var address in user.Company.CompanyAddresses)
                            {
                                if (!fuelRequest.MstSupplierQualifications
                                                .Except(address.MstSupplierQualifications).Any())
                                {
                                    isSupplierQualified = true;
                                    break;
                                }
                            }
                            if (!isSupplierQualified)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessageSupplierNotQualifiedForFuelRequest;
                                return response;
                            }
                            #endregion
                            var currentCostDomain = new CurrentCostDomain(this);
                            var globalCost = await currentCostDomain.GetFuelCostForFuelRequest(user.Company.Id, lastCounterOffer.FuelRequest.MstProduct.TfxProductId ?? 0, lastCounterOffer.FuelRequest.Job.StateId, lastCounterOffer.FuelRequest.UoM, lastCounterOffer.FuelRequest.Currency);
                            if (lastCounterOffer.FuelRequest.FuelRequestPricingDetail.DisplayPrice.Contains(PricingType.Suppliercost.GetDisplayName()))
                            {
                                if (globalCost.HasValue)
                                {
                                    var priceDetailIds = new List<int>();
                                    priceDetailIds.Add(lastCounterOffer.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId);
                                    await currentCostDomain.UpdateFuelCostForFuelRequest(priceDetailIds, globalCost.Value, (int)SupplierCostTypes.GlobalCost);
                                }
                            }
                            else
                            {
                                var request = new PricingDetailRequestViewModel { Id = lastCounterOffer.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId, Currency = (int)fuelRequest.Job.Currency };
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
                                            if (globalCost.HasValue)
                                            {
                                                var pricingDetailids = new List<int>();
                                                pricingDetailids.Add(lastCounterOffer.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId);
                                                await currentCostDomain.UpdateFuelCostForFuelRequest(pricingDetailids, globalCost.Value, (int)SupplierCostTypes.GlobalCost);
                                            }
                                        }
                                    }
                                }
                            }
                            //update fuelrequest status
                            fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                            FuelRequestXStatus frStatus = new FuelRequestXStatus();
                            frStatus.StatusId = (int)FuelRequestStatus.CounterOfferAccepted;
                            frStatus.IsActive = true;
                            frStatus.UpdatedBy = userContext.Id;
                            frStatus.UpdatedDate = DateTimeOffset.Now;
                            fuelRequest.FuelRequestXStatuses.Add(frStatus);


                            fuelRequest.UpdatedBy = userContext.Id;
                            fuelRequest.UpdatedDate = DateTimeOffset.Now;

                            lastCounterOffer.FuelRequest.FuelRequestDetail.IsDropImageRequired = helperDomain.GetDropImageRequired(user.Company.Id);

                            lastCounterOffer.FuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                            FuelRequestXStatus cfrStatus = new FuelRequestXStatus();
                            cfrStatus.StatusId = (int)FuelRequestStatus.Accepted;
                            cfrStatus.IsActive = true;
                            cfrStatus.UpdatedBy = userContext.Id;
                            cfrStatus.UpdatedDate = DateTimeOffset.Now;
                            lastCounterOffer.FuelRequest.FuelRequestXStatuses.Add(cfrStatus);

                            lastCounterOffer.BuyerStatus = (int)CounterOfferStatus.Accepted;
                            lastCounterOffer.SupplierStatus = (int)CounterOfferStatus.Accepted;

                            List<CounterOffer> counterOffersToMarkDeclined = await GetCounterOffersToMarkDeclined(fuelRequestId, lastCounterOffer.Id);
                            foreach (var item in counterOffersToMarkDeclined)
                            {
                                item.SupplierStatus = (int)CounterOfferStatus.Declined;
                                item.BuyerStatus = (int)CounterOfferStatus.Declined;
                                Context.DataContext.Entry(item).State = EntityState.Modified;
                                await Context.CommitAsync();
                            }

                            //create order
                            Order order = new Order
                            {
                                PoNumber = ApplicationConstants.PoNumberPrefix,
                                IsProFormaPo = fuelRequest.Job.IsProFormaPoEnabled,
                                SignatureEnabled = fuelRequest.Job.SignatureEnabled,
                                AcceptedCompanyId = user.Company.Id,
                                AcceptedBy = user.Id,
                                AcceptedDate = DateTimeOffset.Now,
                                TerminalId = fuelRequest.TerminalId,
                                BuyerCompanyId = fuelRequest.User.Company.Id,
                                IsActive = true,
                                UpdatedBy = userContext.Id,//buyer
                                UpdatedDate = DateTimeOffset.Now,
                                DefaultInvoiceType = fuelRequest.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType ? (int)InvoiceType.DigitalDropTicketManual : (int)InvoiceType.Manual,
                                IsEndSupplier = true,
                                CityGroupTerminalId = lastCounterOffer.FuelRequest.CityGroupTerminalId,
                                IsFTL = fuelRequest.FuelRequestDetail != null && fuelRequest.FuelRequestDetail.TruckLoadTypeId == (int)TruckLoadTypes.FullTruckLoad
                            };

                            var onboardingPreferenceSetting = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == order.AcceptedCompanyId && t.IsActive).OrderByDescending(t => t.Id).FirstOrDefault();
                            order.OrderAdditionalDetail = new OrderAdditionalDetail
                            {
                                PreferencesSettingId = onboardingPreferenceSetting?.Id ?? null,
                                // SupplierAssignedProductName = helperDomain.GetSupplierAssignedProductName(order.AcceptedCompanyId, fuelRequest.MstProduct?.TfxProductId ?? 0, fuelRequest?.TerminalId ?? 0),
                            };

                            if (order.OrderXStatuses.FirstOrDefault(t => t.IsActive) != null)
                            {
                                order.OrderXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                            }

                            OrderXStatus orderStatus = new OrderXStatus();
                            orderStatus.StatusId = (int)OrderStatus.Open;
                            orderStatus.IsActive = true;
                            orderStatus.UpdatedBy = userContext.Id;
                            orderStatus.UpdatedDate = DateTimeOffset.Now;
                            order.OrderXStatuses.Add(orderStatus);


                            if (lastCounterOffer.FuelRequest.DeliverySchedules.Any())
                            {
                                lastCounterOffer.FuelRequest.DeliverySchedules.ToList().ForEach(t =>
                                                            order.OrderDeliverySchedules.Add(
                                                                new OrderVersionXDeliverySchedule()
                                                                {
                                                                    DeliveryRequestId = t.Id,
                                                                    Version = 1,
                                                                    CreatedBy = supplierId,
                                                                    CreatedDate = lastCounterOffer.FuelRequest.CreatedDate,
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
                                        CreatedBy = supplierId,
                                        CreatedDate = lastCounterOffer.FuelRequest.CreatedDate,
                                        IsActive = true
                                    });
                            }
                            lastCounterOffer.FuelRequest.Orders.Add(order);

                            var supplier = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == supplierId);
                            if (supplier != null)
                            {
                                order.TaxExemptLicenses = supplier.Company.TaxExemptLicenses.Where(t => t.IsDefault).ToList();
                            }
                            await Context.CommitAsync();

                            order.PoNumber = helperDomain.GetPoNumber(fuelRequest, order.IsProFormaPo, order.Id);
                            order.TfxPoNumber = order.PoNumber;
                            var orderDetailVersion = helperDomain.GetOrderDetailVersion(order, fuelRequest, userContext.Id);
                            order.OrderDetailVersions.Add(orderDetailVersion);

                            if (order.FuelRequest.FuelRequestDetail.PaymentMethod == PaymentMethods.CreditCard)
                                helperDomain.AddCreditCardProcessingFee(order);

                            TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);
                            await trackableScheduleDomain.ProcessTrackableSchedules(lastCounterOffer.FuelRequest.DeliverySchedules, order);

                            if (lastCounterOffer.FuelRequest.PaymentTermId == (int)PaymentTerms.NetDays &&
                                                        user.Company.IsCreditAppEnabled && user.Company.CreditAppDocuments.Count > 0 &&
                                                        !user.Company.Orders.Any(t => t.FuelRequest.User.Company.Id == fuelRequest.User.Company.Id &&
                                                        t.FuelRequest.PaymentTermId == (int)PaymentTerms.NetDays))
                            {
                                response.IsFirstTimeBuyer = true;
                                response.ToUserEmail = fuelRequest.User.Email;
                                response.ToUser = $"{fuelRequest.User.FirstName} {fuelRequest.User.LastName}";
                            }

                            if (fuelRequest.GetParentFuelRequest().FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest)
                            {
                                var parentFuelRequest = fuelRequest.GetParentFuelRequest();
                                var parentOrder = Context.DataContext.Orders.Where(t => t.FuelRequestId == parentFuelRequest.FuelRequest1.Id).FirstOrDefault();
                                if (parentOrder != null && parentOrder.IsEndSupplier)
                                {
                                    parentOrder.IsEndSupplier = false;
                                }

                                var deliveredSchedules = fuelRequest.GetParentFuelRequest().DeliverySchedules.Where(t => !t.DeliveryScheduleXTrackableSchedules.
                                                                                                    Any(t1 => t1.DeliverySchedule.Type == (int)DeliveryScheduleType.SpecificDates
                                                                                                                && t1.DeliveryScheduleId == t.Id
                                                                                                                && t1.Invoices.Any(t2 => t2.IsActiveInvoice)));
                                foreach (var schedule in deliveredSchedules)
                                {
                                    // Passing driver-id null will remove driver from delivery-schedule
                                    helperDomain.AssignDeliveryLevelDriver(schedule, fuelRequest.GetParentFuelRequest().CreatedBy, null, order.Id, true);
                                }
                            }

                            await Context.CommitAsync();
                            transaction.Commit();

                            if (fuelRequest.GetParentFuelRequest().FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest)
                            {
                                var parentFuelRequest = fuelRequest.GetParentFuelRequest();
                                var parentOrder = Context.DataContext.Orders.Where(t => t.FuelRequestId == parentFuelRequest.FuelRequest1.Id).FirstOrDefault();
                                if (parentOrder.OrderTaxDetails != null && parentOrder.OrderTaxDetails.Count > 0)
                                {
                                    var orderDomain = new OrderDomain(this);
                                    await orderDomain.CopyBrokeredOrderTaxesToNewOrder(order, parentOrder, userContext);
                                }
                            }

                            NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                            await newsfeedDomain.SetCounterOfferAcceptedNewsfeed(userContext, fuelRequest, order);

                            int? brokeredOrderId = null;
                            //When new order is created, we need to udate terminal to previous brokered orders
                            if (order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
                            {
                                var orderDomain = new OrderDomain(this);
                                var brokeredOrder = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.LastOrDefault();
                                if (brokeredOrder != null)
                                {
                                    brokeredOrderId = brokeredOrder.Id;
                                    //assing new terminal to all chained orders in broker case
                                    await orderDomain.AssignNewTerminalToOrderAsync(order.TerminalId ?? 0, brokeredOrder.Id);
                                }
                            }

                            //Send response
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageCounterOfferAcceptSuccess;

                            QbWorkflowDomain qbWorkflowDomain = new QbWorkflowDomain(this);
                            qbWorkflowDomain.CreateSalesOrderWorkflow(userContext, fuelRequest, order);
                            qbWorkflowDomain.CreatePurchaseOrderWorkflow(userContext, fuelRequest, order, brokeredOrderId);
                        }
                    }
                    else
                    {
                        //Send response
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageNoCounterOfferFoundToAccept;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("CounterOfferDomain", "AcceptCounterOfferByBuyerAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<FuelRequestStatusViewModel> AcceptCounterOfferBySupplierAsync(UserContext userContext, int fuelRequestId)
        {
            FuelRequestStatusViewModel response = new FuelRequestStatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    HelperDomain helperDomain = new HelperDomain(this);

                    var fuelRequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == fuelRequestId && t.IsActive);
                    var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == userContext.Id);                   
                    if (fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).StatusId != (int)FuelRequestStatus.Open)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageAcceptCounterOfferFailedFuelRequestNotOpen;
                        return response;
                    }

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
                    var counterOffers = await helperDomain.GetCounterOffers(fuelRequestId);
                    if (counterOffers != null && counterOffers.Any())
                    {
                        var lastCounterOffer = Context.DataContext.CounterOffers.Where(x => counterOffers.Contains(x.FuelRequestId) &&
                        x.SupplierId == userContext.Id && x.FuelRequest.IsActive)
                        .OrderByDescending(co => co.Id).FirstOrDefault();

                        if (lastCounterOffer != null && fuelRequest != null)
                        {
                            // get suppliers information here
                            #region Check current Qualifications of supplier
                            bool isSupplierQualified = false;
                            foreach (var address in user.Company.CompanyAddresses)
                            {
                                if (!fuelRequest.MstSupplierQualifications
                                                .Except(address.MstSupplierQualifications).Any())
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
                            #endregion
                            if (user.Company.CurrentCosts == null)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errCurrentCostNotFound;
                                return response;
                            }
                            var availableGlobalCost = user.Company.CurrentCosts.FirstOrDefault(t => t.IsActive && t.IsGlobleCost && t.FuelTypeId == lastCounterOffer.FuelRequest.FuelTypeId);
                           
                            if (lastCounterOffer.FuelRequest.FuelRequestPricingDetail.DisplayPrice.Contains(PricingType.Suppliercost.GetDisplayName()))
                            {
                                if (availableGlobalCost == null)
                                {
                                    response.StatusMessage = Resource.ErrorGlobalCostNotProvidedForAcceptCounterOffer;
                                    return response;
                                }
                                var currentCostDomain = new CurrentCostDomain(this);
                                    var priceDetailIds = new List<int>();
                                    priceDetailIds.Add(lastCounterOffer.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId);
                                    await currentCostDomain.UpdateFuelCostForFuelRequest(priceDetailIds, availableGlobalCost.Cost, (int)SupplierCostTypes.GlobalCost);
                            }
                            else
                            {
                                var request = new PricingDetailRequestViewModel { Id = lastCounterOffer.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId, Currency = (int)fuelRequest.Job.Currency };
                                PricingRequestDetailApiResponse pricingDtls = Task.Run(() => new PricingServiceDomain(this).GetPricingRequestDetailByIdAsync(request)).Result;
                                if (pricingDtls == null)
                                {
                                    response.StatusMessage = Resource.ErrorPricingDetails;//change msg
                                    response.StatusCode = Status.Failed;
                                    return response;
                                }
                                //tier
                                var currentCostDomain = new CurrentCostDomain(this);
                                int fuelcostTierCount = pricingDtls.PricingRequestDetail.TierPricings.Where(w => w.PricingTypeId == (int)PricingType.Suppliercost).ToList().Count;
                                if (fuelcostTierCount > 0)
                                {
                                    foreach (var item in pricingDtls.PricingRequestDetail.TierPricings)
                                    {
                                        if (item.PricingTypeId == (int)PricingType.Suppliercost)
                                        {
                                            if (availableGlobalCost == null)
                                            {
                                                response.StatusMessage = Resource.ErrorGlobalCostNotProvidedForAcceptCounterOffer;
                                                return response;
                                            }
                                            
                                            if (availableGlobalCost.IsGlobleCost)
                                            {
                                                var pricingDetailids = new List<int>();
                                                pricingDetailids.Add(lastCounterOffer.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId);
                                                await currentCostDomain.UpdateFuelCostForFuelRequest(pricingDetailids, availableGlobalCost.Cost, (int)SupplierCostTypes.GlobalCost);
                                            }
                                        }
                                    }
                                }
                            }
                            //update fuelrequest status
                            fuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                            FuelRequestXStatus frStatus = new FuelRequestXStatus();
                            frStatus.StatusId = (int)FuelRequestStatus.CounterOfferAccepted;
                            frStatus.IsActive = true;
                            frStatus.UpdatedBy = userContext.Id;
                            frStatus.UpdatedDate = DateTimeOffset.Now;
                            fuelRequest.FuelRequestXStatuses.Add(frStatus);

                            lastCounterOffer.FuelRequest.FuelRequestDetail.IsDropImageRequired = helperDomain.GetDropImageRequired(user.Company.Id);

                            fuelRequest.UpdatedBy = userContext.Id;
                            fuelRequest.UpdatedDate = DateTimeOffset.Now;

                            lastCounterOffer.FuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                            FuelRequestXStatus cfrStatus = new FuelRequestXStatus();
                            cfrStatus.StatusId = (int)FuelRequestStatus.Accepted;
                            cfrStatus.IsActive = true;
                            cfrStatus.UpdatedBy = userContext.Id;
                            cfrStatus.UpdatedDate = DateTimeOffset.Now;
                            lastCounterOffer.FuelRequest.FuelRequestXStatuses.Add(cfrStatus);

                            lastCounterOffer.BuyerStatus = (int)CounterOfferStatus.Accepted;
                            lastCounterOffer.SupplierStatus = (int)CounterOfferStatus.Accepted;

                            List<CounterOffer> counterOffersToMarkDeclined = await GetCounterOffersToMarkDeclined(fuelRequestId, lastCounterOffer.Id);
                            foreach (var item in counterOffersToMarkDeclined)
                            {
                                item.SupplierStatus = (int)CounterOfferStatus.Declined;
                                item.BuyerStatus = (int)CounterOfferStatus.Declined;
                                Context.DataContext.Entry(item).State = EntityState.Modified;
                                await Context.CommitAsync();
                            }

                            //create order
                            Order order = new Order
                            {
                                PoNumber = ApplicationConstants.PoNumberPrefix,
                                IsProFormaPo = fuelRequest.Job.IsProFormaPoEnabled,
                                SignatureEnabled = fuelRequest.Job.SignatureEnabled,
                                AcceptedCompanyId = user.Company.Id,
                                AcceptedBy = userContext.Id,
                                AcceptedDate = DateTimeOffset.Now,
                                TerminalId = fuelRequest.TerminalId,
                                BuyerCompanyId = fuelRequest.User.Company.Id,
                                IsActive = true,
                                UpdatedBy = userContext.Id,
                                UpdatedDate = DateTimeOffset.Now,
                                DefaultInvoiceType = fuelRequest.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType ? (int)InvoiceType.DigitalDropTicketManual : (int)InvoiceType.Manual,
                                IsEndSupplier = true,
                                CityGroupTerminalId = lastCounterOffer.FuelRequest.CityGroupTerminalId,
                                IsFTL = fuelRequest.FuelRequestDetail != null && fuelRequest.FuelRequestDetail.TruckLoadTypeId == (int)TruckLoadTypes.FullTruckLoad
                            };

                            var onboardingPreferenceSetting = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == order.AcceptedCompanyId && t.IsActive).OrderByDescending(t => t.Id).FirstOrDefault();
                            order.OrderAdditionalDetail = new OrderAdditionalDetail
                            {
                                PreferencesSettingId = onboardingPreferenceSetting?.Id ?? null,
                                // SupplierAssignedProductName = helperDomain.GetSupplierAssignedProductName(order.AcceptedCompanyId, fuelRequest.MstProduct?.TfxProductId ?? 0, fuelRequest?.TerminalId ?? 0),
                            };

                            OrderXStatus orderStatus = new OrderXStatus();
                            orderStatus.StatusId = (int)OrderStatus.Open;
                            orderStatus.IsActive = true;
                            orderStatus.UpdatedBy = userContext.Id;
                            orderStatus.UpdatedDate = DateTimeOffset.Now;
                            order.OrderXStatuses.Add(orderStatus);


                            if (lastCounterOffer.FuelRequest.DeliverySchedules.Any())
                            {
                                lastCounterOffer.FuelRequest.DeliverySchedules.ToList().ForEach(t =>
                                                            order.OrderDeliverySchedules.Add(
                                                                new OrderVersionXDeliverySchedule()
                                                                {
                                                                    DeliveryRequestId = t.Id,
                                                                    Version = 1,
                                                                    CreatedBy = lastCounterOffer.FuelRequest.CreatedBy,
                                                                    CreatedDate = lastCounterOffer.FuelRequest.CreatedDate,
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
                                                                    CreatedBy = lastCounterOffer.FuelRequest.CreatedBy,
                                                                    CreatedDate = lastCounterOffer.FuelRequest.CreatedDate,
                                                                    IsActive = true
                                                                });
                            }
                            lastCounterOffer.FuelRequest.Orders.Add(order);
                            order.TaxExemptLicenses = user.Company.TaxExemptLicenses.Where(t => t.IsDefault).ToList();
                            await Context.CommitAsync();

                            order.PoNumber = helperDomain.GetPoNumber(fuelRequest, order.IsProFormaPo, order.Id);
                            order.TfxPoNumber = order.PoNumber;
                            var orderDetailVersion = helperDomain.GetOrderDetailVersion(order, fuelRequest, userContext.Id);
                            order.OrderDetailVersions.Add(orderDetailVersion);

                            if (order.FuelRequest.FuelRequestDetail.PaymentMethod == PaymentMethods.CreditCard)
                                helperDomain.AddCreditCardProcessingFee(order);

                            TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);
                            await trackableScheduleDomain.ProcessTrackableSchedules(lastCounterOffer.FuelRequest.DeliverySchedules, order);
                            if (lastCounterOffer.FuelRequest.PaymentTermId == (int)PaymentTerms.NetDays &&
                                                        user.Company.IsCreditAppEnabled && user.Company.CreditAppDocuments.Count > 0 &&
                                                        !user.Company.Orders.Any(t => t.FuelRequest.User.Company.Id == fuelRequest.User.Company.Id &&
                                                        t.FuelRequest.PaymentTermId == (int)PaymentTerms.NetDays))
                            {
                                response.IsFirstTimeBuyer = true;
                                response.ToUserEmail = fuelRequest.User.Email;
                                response.ToUser = $"{fuelRequest.User.FirstName} {fuelRequest.User.LastName}";
                            }

                            if (fuelRequest.GetParentFuelRequest().FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest)
                            {
                                var parentFuelRequest = fuelRequest.GetParentFuelRequest();
                                var parentOrder = Context.DataContext.Orders.Where(t => t.FuelRequestId == parentFuelRequest.FuelRequest1.Id).FirstOrDefault();
                                if (parentOrder != null && parentOrder.IsEndSupplier)
                                {
                                    parentOrder.IsEndSupplier = false;
                                }

                                var deliveredSchedules = fuelRequest.GetParentFuelRequest().DeliverySchedules.Where(t => !t.DeliveryScheduleXTrackableSchedules.
                                                                                                    Any(t1 => t1.DeliverySchedule.Type == (int)DeliveryScheduleType.SpecificDates
                                                                                                                && t1.DeliveryScheduleId == t.Id
                                                                                                                && t1.Invoices.Any(t2 => t2.IsActiveInvoice)));
                                foreach (var schedule in deliveredSchedules)
                                {
                                    // Passing driver-id null will remove driver from delivery-schedule
                                    helperDomain.AssignDeliveryLevelDriver(schedule, fuelRequest.GetParentFuelRequest().CreatedBy, null, order.Id, true);
                                }
                            }

                            await Context.CommitAsync();
                            transaction.Commit();

                            if (fuelRequest.GetParentFuelRequest().FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest)
                            {
                                var parentFuelRequest = fuelRequest.GetParentFuelRequest();
                                var parentOrder = Context.DataContext.Orders.Where(t => t.FuelRequestId == parentFuelRequest.FuelRequest1.Id).FirstOrDefault();
                                if (parentOrder.OrderTaxDetails != null && parentOrder.OrderTaxDetails.Count > 0)
                                {
                                    var orderDomain = new OrderDomain(this);
                                    await orderDomain.CopyBrokeredOrderTaxesToNewOrder(order, parentOrder, userContext);
                                }
                            }

                            NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                            await newsfeedDomain.SetCounterOfferAcceptedNewsfeed(userContext, fuelRequest, order);

                            int? brokeredOrderId = null;
                            //When new order is created, we need to udate terminal to previous brokered orders
                            if (order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
                            {
                                var orderDomain = new OrderDomain(this);
                                var brokeredOrder = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.LastOrDefault();
                                if (brokeredOrder != null)
                                {
                                    brokeredOrderId = brokeredOrder.Id;
                                    //assing new terminal to all chained orders in broker case
                                    await orderDomain.AssignNewTerminalToOrderAsync(order.TerminalId ?? 0, brokeredOrder.Id);
                                }
                            }

                            //Send response
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageCounterOfferAcceptSuccess;

                            QbWorkflowDomain qbWorkflowDomain = new QbWorkflowDomain(this);
                            qbWorkflowDomain.CreateSalesOrderWorkflow(userContext, fuelRequest, order);
                            qbWorkflowDomain.CreatePurchaseOrderWorkflow(userContext, fuelRequest, order, brokeredOrderId);
                        }
                        else
                        {
                            //Send response
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageNoCounterOfferFoundToAccept;
                        }
                    }
                    else
                    {
                        //Send response
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageNoCounterOfferFoundToAccept;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageCounterOfferAcceptFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("CounterOfferDomain", "AcceptCounterOfferBySupplierAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<CounterOffer>> GetCounterOffersToMarkDeclined(int fuelRequestId, int id)
        {
            using (var tracer = new Tracer("CounterOfferDomain", "GetCounterOffersToMarkDeclined"))
            {
                List<CounterOffer> counterOffers = new List<CounterOffer>();
                try
                {
                    var offers = await Context.DataContext.CounterOffers.Where(t => t.OriginalFuelRequestId == fuelRequestId && t.Id != id).ToListAsync();
                    if (offers != null)
                    {
                        foreach (var item in offers)
                        {
                            counterOffers.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("CounterOfferDomain", "GetCounterOffersToMarkDeclined", ex.Message, ex);
                }

                return counterOffers;
            }
        }

        public async Task<StatusViewModel> DeclineCounterOfferByBuyerAsync(int fuelRequestId, UserContext userContext, int supplierId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    HelperDomain helperDomain = new HelperDomain(this);

                    var fuelRequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == fuelRequestId && t.IsActive);
                    var counterOffers = await helperDomain.GetCounterOffers(fuelRequestId);
                    if (fuelRequest != null && counterOffers != null && counterOffers.Any())
                    {
                        var lastCounterOffer = Context.DataContext.CounterOffers.Where(x => counterOffers.Contains(x.FuelRequestId) &&
                        x.SupplierId == supplierId && x.FuelRequest.IsActive).OrderByDescending(co => co.Id).FirstOrDefault();

                        if (lastCounterOffer != null)
                        {
                            lastCounterOffer.BuyerStatus = (int)CounterOfferStatus.Declined;
                            await Context.CommitAsync();
                        }

                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetCounterOfferDeclinedNewsfeed(userContext, fuelRequest);
                    }

                    transaction.Commit();

                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageDeclineCounterOfferSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("CounterOfferDomain", "DeclineCounterOfferByBuyerAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> DeclineCounterOfferBySupplierAsync(int fuelRequestId, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var fuelRequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == fuelRequestId && t.IsActive);
                    if (fuelRequest != null)
                    {
                        var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == userContext.Id && t.IsActive);
                        if (user != null)
                        {
                            fuelRequest.Users.Add(user);
                            await Context.CommitAsync();
                        }
                    }

                    // Update Counter Offer status also to maintain log
                    HelperDomain helperDomain = new HelperDomain(this);
                    var counterOffers = await helperDomain.GetCounterOffers(fuelRequestId);
                    if (fuelRequest != null && counterOffers != null && counterOffers.Any())
                    {
                        var lastCounterOffer = Context.DataContext.CounterOffers.Where(x => counterOffers.Contains(x.FuelRequestId) &&
                         x.SupplierId == userContext.Id && x.FuelRequest.CreatedBy == fuelRequest.CreatedBy && x.FuelRequest.IsActive).OrderByDescending(co => co.Id).FirstOrDefault();

                        if (lastCounterOffer != null)
                        {
                            lastCounterOffer.SupplierStatus = (int)CounterOfferStatus.Declined;
                            await Context.CommitAsync();
                        }

                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetCounterOfferDeclinedNewsfeed(userContext, fuelRequest);

                    }

                    transaction.Commit();

                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageDeclineCounterOfferSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("CounterOfferDomain", "DeclineCounterOfferBySupplierAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> CancelCounterOfferByBuyerAsync(int fuelRequestId, int supplierId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    HelperDomain helperDomain = new HelperDomain(this);

                    var counterOffers = await helperDomain.GetCounterOffers(fuelRequestId);
                    var fuelRequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == fuelRequestId);
                    if (counterOffers != null && counterOffers.Any() && fuelRequest != null)
                    {
                        var lastCounterOffer = Context.DataContext
                                                    .CounterOffers
                                                    .Where
                                                    (
                                                        t => counterOffers.Contains(t.FuelRequestId) &&
                                                        t.SupplierId == supplierId &&
                                                        t.FuelRequest.IsActive &&
                                                        t.FuelRequest.CreatedBy == fuelRequest.CreatedBy
                                                    )
                                                    .OrderByDescending(co => co.Id).FirstOrDefault();

                        if (lastCounterOffer != null)
                        {
                            lastCounterOffer.BuyerStatus = (int)CounterOfferStatus.Cancelled;
                            lastCounterOffer.FuelRequest.IsActive = false;
                            await Context.CommitAsync();
                        }
                    }

                    transaction.Commit();

                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageCancelCounterOfferSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("CounterOfferDomain", "CancelCounterOfferByBuyerAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> CancelCounterOfferBySupplierAsync(int fuelRequestId, int supplierId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    HelperDomain helperDomain = new HelperDomain(this);
                    var counterOffers = await helperDomain.GetCounterOffers(fuelRequestId);
                    if (counterOffers != null && counterOffers.Any())
                    {
                        var lastCounterOffer = Context.DataContext.CounterOffers.Where
                                                (t => counterOffers.Contains(t.FuelRequestId) &&
                                                t.SupplierId == supplierId &&
                                                t.FuelRequest.IsActive &&
                                                t.FuelRequest.CreatedBy == supplierId)
                                                .OrderByDescending(t => t.Id).FirstOrDefault();

                        if (lastCounterOffer != null)
                        {
                            lastCounterOffer.SupplierStatus = (int)CounterOfferStatus.Cancelled;
                            lastCounterOffer.FuelRequest.IsActive = false;
                            await Context.CommitAsync();
                        }
                    }

                    transaction.Commit();

                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageCancelCounterOfferSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("CounterOfferDomain", "CancelCounterOfferBySupplierAsync", ex.Message, ex);
                }
            }
            return response;
        }
    }
}