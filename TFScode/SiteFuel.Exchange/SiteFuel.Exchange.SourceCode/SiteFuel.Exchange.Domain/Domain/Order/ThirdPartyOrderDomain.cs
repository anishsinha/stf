using FileHelpers;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.FileGenerator;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Offer;
using SiteFuel.Exchange.ViewModels.Queue;
using SiteFuel.Exchange.ViewModels.ThirdPartyOrder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
namespace SiteFuel.Exchange.Domain
{
    public class ThirdPartyOrderDomain : BaseDomain
    {
        public ThirdPartyOrderDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public ThirdPartyOrderDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<StatusViewModel> UpdateThirdPartyOrder(OrderDetailsViewModel viewModel, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                //non tier
                if (viewModel.FuelDetails.IsMarineLocation && !viewModel.IsSupressOrderPricing && viewModel.FuelDetails.FuelPricing.PricingTypeId == (int)PricingType.Suppliercost
                    && viewModel.TfxFuelTypeId.HasValue)
                {
                    var uom = viewModel.FuelDetails.FuelQuantity.UoM;

                    var availableGlobalCost = await new CurrentCostDomain(this).GetGlobalCost(userContext, viewModel.TfxFuelTypeId.Value, viewModel.JobStateId, uom, viewModel.Country.Currency);
                    if (availableGlobalCost == 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.ErrorGlobalCostNotProvided;
                        return response;
                    }
                    else
                    {
                        viewModel.FuelDetails.FuelPricing.SupplierCost = availableGlobalCost;
                    }
                }
                var orderDomain = new OrderDomain(this);
                var helperDomain = new HelperDomain(this);
                var notificationDomain = new NotificationDomain(this);
                var order = Context.DataContext.Orders.First(t => t.Id == viewModel.Id);
                var fuelRequestDetails = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == viewModel.FuelRequestId);
                JobXAsset jobXAsset = new JobXAsset();
                Asset vessle = new Asset();
                bool oldFuelSurchargeStatus = false;
                if (fuelRequestDetails != null && order != null)
                {
                    oldFuelSurchargeStatus = order.OrderAdditionalDetail.IsFuelSurcharge;
                    //Save NonStandardProduct
                    if (viewModel.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
                    {
                        var productDomain = new ProductDomain(this);
                        var productId = await productDomain.SaveNonStandardProduct(viewModel.FuelDetails.NonStandardFuelName, viewModel.FuelDetails.CreatedBy, fuelRequestDetails.Job.Company, viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId);
                        viewModel.FuelDetails.FuelTypeId = productId;
                    }
                    var isSalesTaxExempted = fuelRequestDetails.Job.JobBudget.IsTaxExempted;
                    if (isSalesTaxExempted != viewModel.IsTaxExempted)
                    {
                        AddAuditLogForSalesTaxExempt(userContext, fuelRequestDetails.JobId, isSalesTaxExempted, viewModel.IsTaxExempted);
                    }

                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (viewModel.CustomerContacts != null && viewModel.CustomerContacts.Any())
                            {

                                int LengthOfPassword = Constants.LengthOfPassword;
                                var RandomPassword = CryptoHelperMethods.GenerateRandomPassword(LengthOfPassword);
                                var TempPassword = CryptoHelperMethods.EncryptPassword(Constants.Key.ToString(), RandomPassword);

                                var orderUsers = order.OrderXUsers.ToList();
                                var existingUsers = orderUsers.Select(t => t.Id).ToList();
                                var newUsers = viewModel.CustomerContacts.Where(t => !existingUsers.Contains(t.Id));
                                var currentUsers = viewModel.CustomerContacts.Select(t => t.Id).ToList();
                                orderUsers.Where(t => !currentUsers.Contains(t.Id)).ToList().ForEach(t => order.OrderXUsers.Remove(t));
                                foreach (var newUser in newUsers)
                                {
                                    newUser.CompanyId = order.BuyerCompanyId;
                                    var user = await CreateUserForThridPartyOrderAsync(newUser, viewModel.IsInvitationEnabled, order.IsFTL, userContext, RandomPassword, TempPassword);
                                    if (user.CompanyId != order.BuyerCompanyId)
                                    {
                                        transaction.Rollback();
                                        response.StatusCode = Status.Failed;
                                        response.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageUserAlreadyExistsWithAnotherCompanyInSFX, new object[] { user.Email, user.Company.Name });
                                        return response;
                                    }
                                    order.OrderXUsers.Add(user);
                                }
                                foreach (var modifiedUser in viewModel.CustomerContacts.Where(t => existingUsers.Contains(t.Id)))
                                {
                                    var userDetails = orderUsers.FirstOrDefault(testc => testc.Id == modifiedUser.Id);
                                    var name = modifiedUser.Name.Trim().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                                    string firstName = string.Empty;
                                    string lastName = string.Empty;
                                    switch (name.Length)
                                    {
                                        case 1:
                                            firstName = name[0];
                                            lastName = name[0].Substring(0, 1);
                                            break;
                                        default:
                                            firstName = name[0];
                                            lastName = name[1];
                                            break;
                                    }

                                    if (userDetails != null)
                                    {
                                        userDetails.FirstName = firstName;
                                        userDetails.LastName = lastName;
                                        userDetails.PhoneNumber = modifiedUser.PhoneNumber;
                                        userDetails.Email = modifiedUser.Email;
                                    }
                                }
                                await Context.CommitAsync();
                            }
                            fuelRequestDetails.User.Company.Name = viewModel.BuyerCompanyName;
                            fuelRequestDetails.User.FirstName = viewModel.BuyerUserFirstName;
                            fuelRequestDetails.User.LastName = viewModel.BuyerUserLastName;
                            fuelRequestDetails.User.PhoneNumber = viewModel.Supplier.PhoneNumber;
                            fuelRequestDetails.User.Email = viewModel.BuyerUserEmail;
                            if (fuelRequestDetails.QuantityTypeId != (int)QuantityType.NotSpecified)
                            {
                                fuelRequestDetails.MaxQuantity = viewModel.GallonsOrdered;
                            }
                            //fuelRequestDetails.ExternalPoNumber = viewModel.PoNumber;
                            fuelRequestDetails.FuelRequestDetail.StartDate = viewModel.FuelDeliveryDetails.StartDate;
                            fuelRequestDetails.FuelRequestDetail.EndDate = viewModel.FuelDeliveryDetails.EndDate; // only in case of single-delivery date range
                            fuelRequestDetails.FuelRequestDetail.StartTime = Convert.ToDateTime(viewModel.FuelDeliveryDetails.StartTime).TimeOfDay;
                            fuelRequestDetails.FuelRequestDetail.EndTime = Convert.ToDateTime(viewModel.FuelDeliveryDetails.EndTime).TimeOfDay;

                            fuelRequestDetails.Job.DisplayJobID = viewModel.DisplayJobID;
                            fuelRequestDetails.Job.JobBudget.IsTaxExempted = viewModel.IsTaxExempted;
                            //remove existing sp instruction
                            fuelRequestDetails.SpecialInstructions.Clear();
                            viewModel.FuelDeliveryDetails.SpecialInstructions.ForEach(t => { if (!string.IsNullOrWhiteSpace(t.Instruction)) { fuelRequestDetails.SpecialInstructions.Add(t.ToEntity()); } });

                            fuelRequestDetails.FuelRequestFees.Clear();
                            if (!viewModel.IsThirdPartyHardwareUsed)
                            {
                                if (viewModel.OrderAdditionalDetails.FreightPricingMethod == FreightPricingMethod.Manual)
                                {
                                    FuelFeesDomain fuelFeesDomain = new FuelFeesDomain(this);
                                    await fuelFeesDomain.SaveFuelFees(viewModel.FuelDeliveryDetails, fuelRequestDetails, userContext, viewModel.OrderAdditionalDetails.IsFuelSurcharge);
                                }
                            }
                            else
                            {
                                fuelRequestDetails.FuelRequestFees = viewModel.ExternalBrokeredOrder.BrokeredOrderFee.ToEntity();
                            }
                            if (viewModel.FuelDetails.IsMarineLocation && !viewModel.IsSupressOrderPricing && !viewModel.FuelDetails.IsTierPricing)
                            {
                                if (viewModel.FuelDetails.FuelPricing.PricingTypeId == (int)PricingType.RackAverage || viewModel.FuelDetails.FuelPricing.PricingTypeId == (int)PricingType.RackHigh ||
                                    viewModel.FuelDetails.FuelPricing.PricingTypeId == (int)PricingType.RackLow)
                                {
                                    fuelRequestDetails.TerminalId = viewModel.FuelDetails.FuelPricing.TerminalId;
                                    fuelRequestDetails.CityGroupTerminalId = viewModel.FuelDetails.FuelPricing.CityGroupTerminalId;
                                    order.CityGroupTerminalId = viewModel.FuelDetails.FuelPricing.CityGroupTerminalId;
                                    order.TerminalId = viewModel.FuelDetails.FuelPricing.TerminalId;
                                }
                                var pricingInfo = await new PricingServiceDomain().UpdatePricingDetails(viewModel.FuelDetails.FuelPricing, viewModel.FuelDetails.FuelQuantity.UoM);
                                fuelRequestDetails.PricingTypeId = viewModel.FuelDetails.FuelPricing.PricingTypeId;
                                fuelRequestDetails.FuelRequestPricingDetail.RequestPriceDetailId = pricingInfo.Result;
                                fuelRequestDetails.FuelRequestPricingDetail.DisplayPrice = pricingInfo.CustomString1;
                                fuelRequestDetails.FuelRequestPricingDetail.DisplayPriceCode = pricingInfo.CustomString2;
                            }
                            if (viewModel.IsInvitationEnabled)
                            {
                                await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.TPOUserInvitedForEULAAcceptance, viewModel.BuyerUserId, userContext.Id);
                                await SaveInvitationSendDetailsAsync(viewModel.BuyerUserEmail, viewModel.BuyerUserId, userContext.Id, viewModel.BuyerUserFirstName, viewModel.BuyerUserLastName);
                            }
                            SetFuelDetailsToFuelRequestEntity(fuelRequestDetails, viewModel.FuelDetails);
                            await SetJobSpecificAddress(fuelRequestDetails, viewModel.BillToInfo, userContext.Id);
                            Context.DataContext.Entry(fuelRequestDetails).State = EntityState.Modified;

                            if (viewModel.TerminalId > 0)
                            {
                                order.TerminalId = viewModel.TerminalId;
                            }
                            order.DefaultInvoiceType = viewModel.IsDefaultInvoiceTypeManual ? (int)InvoiceType.Manual : (int)InvoiceType.DigitalDropTicketManual;
                            if (order.OrderXTogglePricingDetail != null)
                            {
                                order.OrderXTogglePricingDetail.IsHidePricingEnabledForSupplier = viewModel.IsHidePricingEnabled;
                            }
                            else
                            {
                                order.OrderXTogglePricingDetail = new OrderXTogglePricingDetail() { OrderId = viewModel.Id, IsHidePricingEnabledForBuyer = false, IsHidePricingEnabledForSupplier = viewModel.IsHidePricingEnabled };
                            }

                            //// Update external broker order details
                            if (order.ExternalBrokerOrderDetail != null)
                            {
                                var externalBrokerOrderDetail = order.ExternalBrokerOrderDetail;
                                //As we are not changing thse values from Edit order, dont need to assign them again.
                                externalBrokerOrderDetail.UpdatedBy = userContext.Id;
                                externalBrokerOrderDetail.UpdatedDate = DateTimeOffset.Now;
                                Context.DataContext.Entry(externalBrokerOrderDetail).State = EntityState.Modified;
                            }

                            //Update carrier/allowance/invoicePreference details
                            var orderAdditionalDetails = order.OrderAdditionalDetail;
                            if (orderAdditionalDetails != null)
                            {
                                orderAdditionalDetails.DRNotes = viewModel.OrderAdditionalDetails.DRNotes;
                                if (order.IsFTL)
                                {
                                    await SetCarrierAndSourceDetails(viewModel.OrderAdditionalDetails, userContext, orderAdditionalDetails);
                                    orderAdditionalDetails.Allowance = viewModel.OrderAdditionalDetails.Allowance;
                                    orderAdditionalDetails.BOLInvoicePreferenceId = (int)viewModel.OrderAdditionalDetails.BOLInvoicePreferenceTypes;
                                    orderAdditionalDetails.Notes = viewModel.OrderAdditionalDetails.Notes;
                                    orderAdditionalDetails.IsFuelSurcharge = viewModel.OrderAdditionalDetails.IsFuelSurcharge;
                                    orderAdditionalDetails.FuelSurchagePricingType = viewModel.OrderAdditionalDetails.IsFuelSurcharge ? (int)viewModel.OrderAdditionalDetails.FuelSurchagePricingType : (int?)null;
                                    Context.DataContext.Entry(orderAdditionalDetails).State = EntityState.Modified;
                                }
                                else if (!string.IsNullOrWhiteSpace(viewModel.OrderAdditionalDetails.Notes))
                                {
                                    orderAdditionalDetails.Notes = viewModel.OrderAdditionalDetails.Notes;
                                    Context.DataContext.Entry(orderAdditionalDetails).State = EntityState.Modified;
                                }
                                else if (orderAdditionalDetails.Notes != null && viewModel.OrderAdditionalDetails.Notes == string.Empty)
                                {
                                    orderAdditionalDetails.Notes = string.Empty;
                                    Context.DataContext.Entry(orderAdditionalDetails).State = EntityState.Modified;
                                }
                                if (viewModel.FuelDetails != null)
                                {
                                    orderAdditionalDetails.Berth = string.IsNullOrWhiteSpace(viewModel.FuelDetails.Berth) ? null : viewModel.FuelDetails.Berth.Trim();
                                }
                            }

                            // order version for payment terms
                            bool isPaymentTermChange = false;
                            var currentActiveVersion = order.OrderDetailVersions.LastOrDefault();
                            if (currentActiveVersion != null)
                            {
                                order.OrderDetailVersions.Add(new OrderDomain(this).GetNewOrderDetailVersion(currentActiveVersion, userContext.Id, string.Empty, viewModel.PaymentTermId, viewModel.NetDays, viewModel.PaymentMethod, EditPropertyType.UpdateTPO, viewModel));

                                if (currentActiveVersion.PaymentTermId != viewModel.PaymentTermId || currentActiveVersion.PaymentMethod != viewModel.PaymentMethod || currentActiveVersion.NetDays != viewModel.NetDays)
                                {
                                    // create new payment terms version only if payment terms are modified
                                    //order.OrderDetailVersions.Add(new OrderDomain(this).GetNewOrderDetailVersion(currentActiveVersion, userContext.Id, string.Empty, viewModel.PaymentTermId, viewModel.NetDays, viewModel.PaymentMethod));
                                    helperDomain.CheckForProcessingFee(viewModel.PaymentMethod, order, currentActiveVersion);
                                    if (currentActiveVersion != null && (currentActiveVersion.PaymentTermId != viewModel.PaymentTermId || currentActiveVersion.NetDays != viewModel.NetDays))
                                    {
                                        isPaymentTermChange = true;
                                    }
                                }
                            }

                            // update FTL - full truck order details
                            //order.IsFTL = viewModel.IsFTLEnabled;
                            Context.DataContext.Entry(order).State = EntityState.Modified;

                            //asign driver
                            var lastDriverAssigned = order.OrderXDrivers.FirstOrDefault(t => t.IsActive)?.DriverId;
                            if (viewModel.DriverId != lastDriverAssigned)
                            {
                                response = await orderDomain.AssignDriverAsync(userContext, order.Id, viewModel.DriverId);
                                if (response.StatusCode == Status.Failed)
                                {
                                    transaction.Rollback();
                                    response.StatusMessage = Resource.errMessageAssignDrivertoOrderFailed;
                                    return response;
                                }
                            }
                            if (viewModel.FuelDetails.IsMarineLocation)
                            {
                                if (viewModel.FuelDetails.VessleId.HasValue && viewModel.FuelDetails.VessleId.Value > 0)
                                {
                                    jobXAsset = Context.DataContext.JobXAssets.Where(t => t.OrderId == order.Id).FirstOrDefault();
                                    vessle = Context.DataContext.Assets.Where(t => t.Id == viewModel.FuelDetails.VessleId && t.IsMarine && t.Type == (int)AssetType.Vessle && t.IsActive)
                                                .FirstOrDefault();
                                    if (jobXAsset != null)
                                    {
                                        if (jobXAsset.AssetId != vessle.Id) // new vessel assigned to order
                                        {
                                            // set orderid null for previously assigned vessel.
                                            jobXAsset.OrderId = null;
                                            // add new JobXAsset entry for order
                                            Context.DataContext.JobXAssets.Add(new JobXAsset() { AssetId = viewModel.FuelDetails.VessleId.Value, JobId = order.FuelRequest.Job.Id, AssignedBy = userContext.Id, AssignedDate = DateTime.Now, OrderId = order.Id });

                                        }
                                        if (vessle != null && vessle.AssetAdditionalDetail != null)
                                        {
                                            vessle.AssetAdditionalDetail.IMONumber = !string.IsNullOrWhiteSpace(viewModel.FuelDetails.IMONumber) ? viewModel.FuelDetails.IMONumber.Trim() : null;
                                            vessle.AssetAdditionalDetail.Flag = !string.IsNullOrWhiteSpace(viewModel.FuelDetails.Flag) ? viewModel.FuelDetails.Flag.Trim() : null;
                                            Context.DataContext.Entry(vessle).State = EntityState.Modified;
                                        }

                                    }
                                }
                                else
                                {
                                    transaction.Rollback();
                                    response.StatusMessage = string.Format(Resource.valMessageRequired, Resource.lblVessle);
                                    return response;
                                }

                            }

                            await Context.CommitAsync();
                            transaction.Commit();

                            if (isPaymentTermChange)
                            {
                                var newsfeedDomain = new NewsfeedDomain(this);
                                await newsfeedDomain.SetOrderPaymentTermsUpdatedNewsfeed(userContext, order);

                                await notificationDomain.AddNotificationEventAsync(EventType.OrderPaymentTermsUpdated, order.Id, userContext.Id);
                            }
                            else
                            {
                                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                                await newsfeedDomain.SetThirdPartyOrderModifiedNewsfeed(userContext, order, order.PoNumber);
                            }

                            if (viewModel.OrderAdditionalDetails.FreightPricingMethod == FreightPricingMethod.Manual && viewModel.OrderAdditionalDetails.IsFuelSurcharge != oldFuelSurchargeStatus)
                            {
                                await AddFuelSurchargeNotificationEvent(viewModel.OrderAdditionalDetails.IsFuelSurcharge, order.Id, userContext.Id);
                                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                                await newsfeedDomain.SetFuelSurchargeEnableOrDisabledNewsfeed(order.Id, order.BuyerCompanyId, order.FuelRequest.Job.TimeZoneName, order.PoNumber, viewModel.OrderAdditionalDetails.IsFuelSurcharge, userContext);
                            }
                            else if (viewModel.OrderAdditionalDetails.FreightPricingMethod == FreightPricingMethod.Auto && viewModel.OrderAdditionalDetails.IsFuelSurchargeAuto != oldFuelSurchargeStatus)
                            {
                                await AddFuelSurchargeNotificationEvent(viewModel.OrderAdditionalDetails.IsFuelSurchargeAuto, order.Id, userContext.Id);
                                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                                await newsfeedDomain.SetFuelSurchargeEnableOrDisabledNewsfeed(order.Id, order.BuyerCompanyId, order.FuelRequest.Job.TimeZoneName, order.PoNumber, viewModel.OrderAdditionalDetails.IsFuelSurchargeAuto, userContext);
                            }
                            if (viewModel.IsInvitationEnabled)
                            {
                                //insert into supplier invitation - website branding if branding enable.
                                await SaveSupplierInvitationDetailsAsync(userContext.CompanyId, userContext.Id, viewModel.BuyerUserId);
                            }
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageForTPOEdit;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogManager.Logger.WriteException("ThirdPartyDomain", "UpdateThirdPartyOrder", ex.Message + " Order Id: " + viewModel.Id, ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyDomain", "UpdateThirdPartyOrder", ex.Message + " Order Id: " + viewModel.Id, ex);
            }

            return response;
        }

        public StatusViewModel ValidateFuelType(int? jobId, int fuelTypeId, bool requestCheck, int pricingSourceId = 0, bool isOffer = false)
        {
            using (var tracer = new Tracer("ThirdPartyDomain", "ValidateFuelType"))
            {
                var response = new StatusViewModel();
                int FuelTye;
                if (jobId != null && jobId > 0)
                {
                    var job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == jobId);


                    if (job != null && job.IsRetailJob)
                    {
                        FuelRequestDomain frdObject = new FuelRequestDomain();

                        if (!isOffer)
                        {
                            FuelTye = frdObject.GetFuelTypeId(fuelTypeId, pricingSourceId);
                        }
                        else
                        {
                            FuelTye = fuelTypeId;
                        }


                        if (requestCheck)
                        {
                            if (job.FuelRequests.Where(x => x.FuelTypeId == FuelTye).Any(t => t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Accepted))
                            {
                                var orders = job.FuelRequests
                                                   .Where(t => t.FuelTypeId == FuelTye && t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Accepted)
                                                   .Select(t => t.Orders.Last())
                                                   .ToList();
                                if (orders.Any(t1 => t1.OrderXStatuses.FirstOrDefault(t2 => t2.IsActive).StatusId == (int)OrderStatus.Open))
                                {
                                    response.StatusCode = Status.Warning;
                                    response.StatusMessage = Resource.warningMessageFuelTypeAlreadyExist;
                                    return response;
                                }
                            }

                            if (job.FuelRequests.Where(x => x.FuelTypeId == FuelTye).Any(t => t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Open))
                            {
                                response.StatusCode = Status.Warning;
                                response.StatusMessage = Resource.warningMessageFuelTypeAlreadyExist;
                                return response;
                            }

                        }
                        else
                        {
                            var fuelrequestList = job.FuelRequests.Where(x => x.FuelTypeId == FuelTye && x.Orders != null && x.Orders.Any()).ToList();
                            foreach (var jobFuelrequest in fuelrequestList)
                            {
                                var status = Context.DataContext.Orders.FirstOrDefault(t => t.FuelRequestId == jobFuelrequest.Id).OrderXStatuses.OrderByDescending(x => x.Id).FirstOrDefault();

                                if (status != null && status.StatusId == (int)OrderStatus.Open)
                                {
                                    response.StatusCode = Status.Warning;
                                    response.StatusMessage = Resource.warningMessageFuelTypeOrderAlreadyExist;
                                    return response;
                                }
                            }
                        }
                    }
                }
                return response;
            }
        }

        private async Task SetJobSpecificAddress(FuelRequest fuelRequestDetails, JobSpecificBillToViewModel billToInfo, int userId)
        {
            if (!billToInfo.IsExistingBillAddress)
            {
                var isBillingToExists = (!string.IsNullOrWhiteSpace(billToInfo.Address));
                if (isBillingToExists)
                {
                    billToInfo.CompanyId = fuelRequestDetails.Job.CompanyId;
                    var billingAddress = billToInfo.ToBillingAddressEntityFromTPO();

                    var isBillingAddressExits = await Context.DataContext.BillingAddresses.Where(t => t.CompanyId == billToInfo.CompanyId && t.IsActive).ToListAsync();
                    if (isBillingAddressExits == null || isBillingAddressExits.Count == 0)
                    {
                        billingAddress.IsDefault = true; //When user adds first billing address set as default.
                    }

                    billingAddress.UpdatedBy = userId;
                    billingAddress.UpdatedDate = DateTimeOffset.Now;
                    Context.DataContext.BillingAddresses.Add(billingAddress);
                    await Context.CommitAsync();

                    billToInfo.BillingAddressId = billingAddress.Id;
                }
            }

            if (billToInfo.BillingAddressId.HasValue) //For Existing Job If Billing Address is changed
            {
                fuelRequestDetails.Job.IsBillToEnabled = true;
                fuelRequestDetails.Job.BillingAddressId = billToInfo.BillingAddressId;
            }

            //if (!string.IsNullOrWhiteSpace(billToInfo.Address) && !string.IsNullOrWhiteSpace(billToInfo.Name))
            //{
            //    fuelRequestDetails.Job.BillToAddress = billToInfo.Address;
            //    fuelRequestDetails.Job.BillToCity = billToInfo.City;
            //    fuelRequestDetails.Job.BillToCounty = billToInfo.County;
            //    fuelRequestDetails.Job.BillToCountryId = billToInfo.Country.Id;
            //    fuelRequestDetails.Job.BillToCountryName = billToInfo.Country.Name;
            //    fuelRequestDetails.Job.BillToName = billToInfo.Name;
            //    fuelRequestDetails.Job.BillToStateId = billToInfo.State.Id;
            //    fuelRequestDetails.Job.BillToStateName = billToInfo.State.Name;
            //    fuelRequestDetails.Job.BillToZipCode = billToInfo.ZipCode;
            //    fuelRequestDetails.Job.IsBillToEnabled = !string.IsNullOrWhiteSpace(billToInfo.Address);
            //}
        }

        public async Task SetCarrierAndSourceDetails(OrderAdditionalDetailsViewModel orderAdditionalDetails, UserContext userContext, OrderAdditionalDetail additionalDetails)
        {
            var dispatchDomain = new DispatchDomain(this);
            if (orderAdditionalDetails.Carrier != null)
            {
                orderAdditionalDetails.Carrier = await dispatchDomain.AddCarrierIfNotExists(orderAdditionalDetails.Carrier.Name, userContext.Id, userContext.CompanyId);
                if (orderAdditionalDetails.Carrier.Id > 0)
                {
                    additionalDetails.CarrierId = orderAdditionalDetails.Carrier.Id;
                }
            }
            if (orderAdditionalDetails.SupplierSource != null)
            {
                orderAdditionalDetails.SupplierSource = await dispatchDomain.AddSupplierSourceIfNotExists(orderAdditionalDetails.SupplierSource, userContext.Id, userContext.CompanyId);
                additionalDetails.SupplierContract = orderAdditionalDetails.SupplierSource.ContractNumber;
                if (orderAdditionalDetails.SupplierSource.Id > 0)
                {
                    additionalDetails.SupplierSourceId = orderAdditionalDetails.SupplierSource.Id;
                }
            }
            additionalDetails.IsDriverToUpdateBOL = orderAdditionalDetails.IsDriverToUpdateBOL;
            additionalDetails.LoadCode = orderAdditionalDetails.LoadCode;
        }

        private void AddAuditLogForSalesTaxExempt(UserContext userContext, int jobId, bool from, bool to)
        {
            AuditLogger.AddAuditLog(userContext, new AuditLogViewModel()
            {
                CallSite = "ThirdPartyDomain : UpdateThirdPartyOrder",
                Message = $"{userContext.Name} modify the sales tax exempt from {from} to {to}",
                AuditEntityId = jobId,
                AuditEntityType = AuditEntityType.Job.ToString(),
                AuditEventType = AuditEventType.Update.ToString()
            });
        }

        public async Task<StatusViewModel> CreateThirdPartyOrder(UserContext userContext, ThirdPartyOrderViewModel thirdPartyOrderViewModel)
        {
            using (var tracer = new Tracer("ThirdPartyDomain", "CreateThirdPartyOrder"))
            {
                var response = new StatusViewModel();

                var fuelrequestDomain = new FuelRequestDomain(this);
                thirdPartyOrderViewModel.FuelDeliveryDetails.IsDriverToUpdateBOL = thirdPartyOrderViewModel.OrderAdditionalDetailsViewModel.IsDriverToUpdateBOL;

                //validate source region and terminal /bulkplant
                if (thirdPartyOrderViewModel.OrderAdditionalDetailsViewModel.FreightPricingMethod == FreightPricingMethod.Auto)
                {
                    if (!thirdPartyOrderViewModel.IsSupressOrderPricing && thirdPartyOrderViewModel.SourceRegion.SelectedSourceRegions.Count == 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageSourceRegionRequired;
                        return response;
                    }
                    if (!thirdPartyOrderViewModel.IsSupressOrderPricing && !(thirdPartyOrderViewModel.SourceRegion.SelectedTerminals.Any() || thirdPartyOrderViewModel.SourceRegion.SelectedBulkPlants.Any()))
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageTerminalOrBulkPlantRequired;
                        return response;
                    }
                }
                if (thirdPartyOrderViewModel.FuelDetails.IsTierPricing && thirdPartyOrderViewModel.FuelDetails.TierPricing.Pricings == null)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageTierPricingRequired;
                    return response;
                }

                if (thirdPartyOrderViewModel.CustomerDetails.IsNewCompany)
                {
                    var companyDomain = new CompanyDomain(fuelrequestDomain);
                    var isCompanyExists = await companyDomain.IsCompanyExist(thirdPartyOrderViewModel.CustomerDetails.CompanyName);
                    if (isCompanyExists)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageCompanyAlreadyExists;
                        return response;
                    }
                }

                if (thirdPartyOrderViewModel.FuelDetails.IsTierPricing && thirdPartyOrderViewModel.FuelDetails.TierPricing.Pricings.Any())
                {
                    thirdPartyOrderViewModel.FuelDetails.TierPricing.Pricings.ForEach(e => e.FuelTypeId = thirdPartyOrderViewModel.FuelDetails.FuelTypeId);
                    thirdPartyOrderViewModel.PricingDetails.TierPricing = thirdPartyOrderViewModel.FuelDetails.TierPricing;
                    thirdPartyOrderViewModel.PricingDetails.PricingTypeId = (int)PricingType.Tier;
                }

                if (thirdPartyOrderViewModel.AddressDetails.IsVarious)
                {
                    thirdPartyOrderViewModel.AddressDetails.JobLocationType = JobLocationTypes.Various;
                }

                thirdPartyOrderViewModel.FuelDeliveryDetails.TruckLoadTypes = thirdPartyOrderViewModel.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypes;

                //tier [fuel cost pricing type]
                if (thirdPartyOrderViewModel.FuelDetails.IsTierPricing && thirdPartyOrderViewModel.PricingDetails.TierPricing.Pricings != null &&
                 thirdPartyOrderViewModel.PricingDetails.TierPricing.Pricings.Where(w => w.PricingTypeId == (int)PricingType.Suppliercost).Any() && thirdPartyOrderViewModel.FuelDetails.FuelTypeId.HasValue)
                {
                    var uom = thirdPartyOrderViewModel.AddressDetails.Country.UoM;
                    if (thirdPartyOrderViewModel.FuelDetails.IsMarineLocation && thirdPartyOrderViewModel.FuelDetails.FuelQuantity.QuantityTypeId == (int)QuantityType.NotSpecified)
                    {
                        uom = thirdPartyOrderViewModel.AddressDetails.MarineUoM;
                    }
                    else if (thirdPartyOrderViewModel.FuelDetails.IsMarineLocation && thirdPartyOrderViewModel.FuelDetails.FuelQuantity.QuantityTypeId != (int)QuantityType.NotSpecified)
                    {
                        uom = thirdPartyOrderViewModel.FuelDetails.FuelQuantity.UoM;
                    }

                    var availableGlobalCost = await new CurrentCostDomain(this).GetGlobalCost(userContext, thirdPartyOrderViewModel.FuelDetails.FuelTypeId.Value, thirdPartyOrderViewModel.AddressDetails.State.Id, uom, thirdPartyOrderViewModel.AddressDetails.Country.Currency);
                    if (availableGlobalCost == 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.ErrorGlobalCostNotProvided;
                        return response;
                    }
                    else
                    {
                        thirdPartyOrderViewModel.PricingDetails.TierPricing.Pricings.Where(w => w.PricingTypeId == (int)PricingType.Suppliercost).Select(s => s.SupplierCost = availableGlobalCost);
                    }
                }

                //non tier
                if (thirdPartyOrderViewModel.PricingDetails.PricingTypeId == (int)PricingType.Suppliercost
                    && thirdPartyOrderViewModel.FuelDetails.FuelTypeId.HasValue)
                {
                    // UoM value posts in same property for both TPO and nomination form
                    var uom = thirdPartyOrderViewModel.FuelDetails.FuelQuantity.UoM;

                    //if (thirdPartyOrderViewModel.FuelDetails.IsMarineLocation && thirdPartyOrderViewModel.FuelDetails.FuelQuantity.QuantityTypeId == (int)QuantityType.NotSpecified)
                    //{
                    //    uom = thirdPartyOrderViewModel.AddressDetails.MarineUoM;
                    //}
                    //else if (thirdPartyOrderViewModel.FuelDetails.IsMarineLocation && thirdPartyOrderViewModel.FuelDetails.FuelQuantity.QuantityTypeId != (int)QuantityType.NotSpecified)
                    //{
                    //    uom = thirdPartyOrderViewModel.FuelDetails.FuelQuantity.UoM;
                    //}

                    var availableGlobalCost = await new CurrentCostDomain(this).GetGlobalCost(userContext, thirdPartyOrderViewModel.FuelDetails.FuelTypeId.Value, thirdPartyOrderViewModel.AddressDetails.State.Id, uom, thirdPartyOrderViewModel.AddressDetails.Country.Currency);
                    if (availableGlobalCost == 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.ErrorGlobalCostNotProvided;
                        return response;
                    }
                    else
                    {
                        thirdPartyOrderViewModel.PricingDetails.SupplierCost = availableGlobalCost;
                    }
                }
                // Fee by quantity validation
                if (thirdPartyOrderViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t =>
                        t.FeeSubTypeId == (int)FeeSubType.ByQuantity && !t.DeliveryFeeByQuantity.Any()))
                {
                    response.StatusMessage = Resource.errMessageFeeByQuantityRequired;
                    return response;
                }

                int LengthOfPassword = Constants.LengthOfPassword;
                var RandomPassword = CryptoHelperMethods.GenerateRandomPassword(LengthOfPassword);
                var TempPassword = CryptoHelperMethods.EncryptPassword(Constants.Key.ToString(), RandomPassword);

                var buyerUser = CreateUsersForThridPartyOrderAsync(thirdPartyOrderViewModel, userContext, RandomPassword);
                var buyerCompany = CreateCompanyForThridPartyOrder(thirdPartyOrderViewModel);

                // by various + empty billing address combination
                if (thirdPartyOrderViewModel.AddressDetails.IsVarious && thirdPartyOrderViewModel.AddressDetails.Country.Id != (int)Country.CAR &&
                    !IsBillingAddressProvided(thirdPartyOrderViewModel.BillingAddress))
                {
                    response.StatusMessage = Resource.errMessageBillingAddressEmptyForVarious;
                    return response;
                }
                bool isNewJob = false;
                var Company = new Company();
                string emailIds = "";
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (buyerUser.Any())
                        {
                            emailIds = string.Join(",", buyerUser.Select(p => p.Email.ToString()));
                            foreach (var user in buyerUser)
                            {
                                Company = user.Company;

                                if (Company != null && Company.Id != 0 && Company.Id != buyerCompany.Id)
                                {
                                    transaction.Rollback();
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageUserAlreadyExistsWithAnotherCompanyInSFX, new object[] { emailIds, Company.Name });
                                    return response;
                                }
                            }
                        }
                        if (thirdPartyOrderViewModel.IsBuyAndSellOrder)
                        {
                            var exisitngJob = Context.DataContext.Jobs.Where(t => t.CompanyId != buyerCompany.Id
                                                && t.Name.Equals(thirdPartyOrderViewModel.AddressDetails.JobName, StringComparison.OrdinalIgnoreCase)
                                                && t.FuelRequests.SelectMany(t1 => t1.Orders).Any(t2 => t2.ExternalBrokerId != null))
                                                .Select(t => new { CompanyName = t.Company.Name }).FirstOrDefault();
                            if (exisitngJob != null)
                            {
                                transaction.Rollback();
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageJobAlreadyExistsWithAnotherCompanyInSFX, new object[] { thirdPartyOrderViewModel.AddressDetails.JobName, exisitngJob.CompanyName });
                                return response;
                            }
                        }
                        //for broker customer
                        if (thirdPartyOrderViewModel.IsThirdPartyHardwareUsed || thirdPartyOrderViewModel.IsBuyAndSellOrder)
                        {
                            if (thirdPartyOrderViewModel.IsSendFileToBroker)
                            {
                                thirdPartyOrderViewModel.ExternalBrokeredOrder.InvoicePreferenceId = (int)InvoicePreference.SendBrokerDataFileToBroker;
                            }
                            response = await AddTPOBrokerCustomerDetails(thirdPartyOrderViewModel, userContext);
                            if (response.StatusCode == Status.Failed)
                            {
                                transaction.Rollback();
                                return response;
                            }
                        }

                        //if new driver added
                        if (thirdPartyOrderViewModel.IsNewDriver)
                        {
                            if (thirdPartyOrderViewModel.DriverFirstName.Trim() != "" && thirdPartyOrderViewModel.DriverLastName.Trim() != "" && thirdPartyOrderViewModel.DriverEmail.Trim() != "")
                            {
                                var driverResponse = await AddDriverForTPO(userContext, thirdPartyOrderViewModel);
                                if (driverResponse.StatusCode == Status.Failed)
                                {
                                    transaction.Rollback();

                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = driverResponse.StatusMessages.FirstOrDefault();
                                    return response;
                                }
                            }
                            else
                            {
                                transaction.Rollback();

                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessageValidDriverDetails;
                                return response;
                            }
                        }

                        //Add user and default role as admin
                        NotificationDomain notificationDomain = new NotificationDomain(this);
                        if (buyerUser.Any())
                        {
                            foreach (var oUsers in buyerUser)
                            {
                                if (oUsers.Id == 0)
                                {
                                    oUsers.MstRoles.Add(Context.DataContext.MstRoles.Single(t => t.Id == (int)UserRoles.Admin));
                                    Context.DataContext.Users.Add(oUsers);
                                    await Context.CommitAsync();

                                    string eventTypeId = string.Empty;
                                    if (thirdPartyOrderViewModel.IsInvitationEnabled && !oUsers.IsEmailConfirmed && !oUsers.IsOnboardingComplete)
                                    {
                                        var authDomain = new AuthenticationDomain(fuelrequestDomain);

                                        var userRoleIds = oUsers.MstRoles.Select(t => t.Id).ToList();
                                        var defaultEnabled = authDomain.GetDefaultEnabledNotifications(userRoleIds, buyerCompany.CompanyTypeId);
                                        if (defaultEnabled != null && defaultEnabled.Any())
                                        {
                                            eventTypeId = string.Join(",", defaultEnabled);
                                        }
                                    }
                                    if (thirdPartyOrderViewModel.IsNotifyDeliveries)
                                    {
                                        eventTypeId = eventTypeId + "," + ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingNotifyDeliveries);
                                    }
                                    if (thirdPartyOrderViewModel.IsNotifySchedules)
                                    {
                                        eventTypeId = eventTypeId + "," + ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingNotifySchedules);
                                    }
                                    if (thirdPartyOrderViewModel.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypes == TruckLoadTypes.FullTruckLoad)
                                    {
                                        eventTypeId = eventTypeId + "," + ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingNotificationPreferencesForFtlOrder);
                                    }

                                    var eventTypeIds = eventTypeId.TrimStart(',').Split(',').Distinct().ToList();
                                    var sqlQuery = Context.DataContext.MstCompanyUserRoleXEventTypes.Where(t => t.NotificationType != (int)NotificationType.Nothing
                                                    && t.RoleId == (int)UserRoles.Admin && t.CompanyTypeId == buyerCompany.CompanyTypeId)
                                                    .Select(t => new { t.EventTypeId });

                                    var notificationSettings = new List<UserXNotificationSetting>();
                                    foreach (var item in sqlQuery)
                                    {
                                        bool isEmail = false;
                                        if (eventTypeIds.Contains(item.EventTypeId.ToString()))
                                        {
                                            isEmail = true;
                                        }
                                        var setting = new AuthenticationDomain(this).GetNotificationSetting(oUsers.Id, item.EventTypeId, isEmail);
                                        notificationSettings.Add(setting);
                                    }
                                    oUsers.UserXNotificationSettings = notificationSettings.Distinct().ToList();
                                }
                                thirdPartyOrderViewModel.CustomerDetails.UserId = oUsers.Id;

                                if (buyerCompany.Id == 0)
                                {
                                    buyerCompany.CreatedBy = oUsers.Id;
                                    buyerCompany.UpdatedBy = oUsers.Id;
                                    Context.DataContext.Companies.Add(buyerCompany);
                                    await Context.CommitAsync();

                                    var companyAddress = thirdPartyOrderViewModel.ToCompanyAddressEntity();
                                    if (companyAddress != null && companyAddress.StateId > 0)
                                    {
                                        buyerCompany.CompanyAddresses.Add(companyAddress);
                                        Context.DataContext.Entry(buyerCompany).State = EntityState.Modified;
                                    }
                                }

                                if (thirdPartyOrderViewModel.IsInvitationEnabled && !oUsers.IsEmailConfirmed && !oUsers.IsOnboardingComplete)
                                {
                                    await notificationDomain.AddNotificationEventAsync(EventType.TPOUserInvitedForEULAAcceptance, oUsers.Id, userContext.Id, null, TempPassword);
                                    // send buyer onboard invitation email
                                    await SaveInvitationSendDetailsAsync(oUsers.Email, oUsers.Id, userContext.Id, oUsers.FirstName, oUsers.LastName);
                                    // update buyer user for onboarding
                                    oUsers.IsEmailConfirmed = true;
                                    oUsers.IsOnboardingComplete = true;
                                    oUsers.IsActive = true;
                                }

                                if (oUsers.Company == null)
                                {
                                    oUsers.Company = buyerCompany;
                                }
                            }
                        }

                        thirdPartyOrderViewModel.CustomerDetails.CompanyId = buyerCompany.Id;

                        //CHECK PO NUMBER UNIQUENESS
                        if (!string.IsNullOrWhiteSpace(thirdPartyOrderViewModel.PONumber))
                        {
                            var isPOExists = new HelperDomain(this).IsValidPONumber(0, thirdPartyOrderViewModel.PONumber, buyerCompany.Id);
                            if (!isPOExists)
                            {
                                transaction.Rollback();
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessagePoNumberAlreadyExists;
                                return response;
                            }
                        }

                        if (!thirdPartyOrderViewModel.BillToInfo.IsExistingBillAddress)
                        {
                            var isBillingToExists = (!string.IsNullOrWhiteSpace(thirdPartyOrderViewModel.BillToInfo.Address));
                            if (isBillingToExists)
                            {
                                thirdPartyOrderViewModel.BillToInfo.CompanyId = thirdPartyOrderViewModel.CustomerDetails.CompanyId;
                                var billingAddress = thirdPartyOrderViewModel.BillToInfo.ToBillingAddressEntityFromTPO();

                                var isBillingAddressExits = await Context.DataContext.BillingAddresses.Where(t => t.CompanyId == thirdPartyOrderViewModel.CustomerDetails.CompanyId && t.IsActive).ToListAsync();
                                if (isBillingAddressExits == null || isBillingAddressExits.Count == 0)
                                {
                                    billingAddress.IsDefault = true; //When user adds first billing address set as default.
                                }

                                billingAddress.UpdatedBy = userContext.Id;
                                billingAddress.UpdatedDate = DateTimeOffset.Now;
                                Context.DataContext.BillingAddresses.Add(billingAddress);
                                await Context.CommitAsync();

                                thirdPartyOrderViewModel.BillToInfo.BillingAddressId = billingAddress.Id;
                            }
                        }

                        //CREATE OR USE EXISTING JOB
                        var buyerJob = CreateJobFromTPOAsync(thirdPartyOrderViewModel);

                        if (buyerJob == null)
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = thirdPartyOrderViewModel.IsPortUsedByAnotherCompany ? "Port is already used by Another Company." : Resource.errMessageJobCreateFailedInvalidAddress;
                            return response;
                        }

                        if (buyerJob.TerminalId == null || buyerJob.TerminalId == 0)
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageJobCreateNoTerminalFound;
                            return response;
                        }

                        if ((buyerJob.Latitude == 0 || buyerJob.Longitude == 0) && thirdPartyOrderViewModel.AddressDetails.IsVarious)
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageJobCreateFailedInvalidAddress;
                            return response;
                        }

                        if (buyerJob.Id == 0)
                        {
                            isNewJob = true;

                            buyerJob.CreatedByCompanyId = userContext.CompanyId;
                            buyerCompany.Jobs.Add(buyerJob);
                            Context.DataContext.Entry(buyerCompany).State = EntityState.Modified;
                            await Context.CommitAsync();
                            if (thirdPartyOrderViewModel.AddressDetails.IsCompanyOwned)
                            {
                                new JobDomain(this).SetCompanyOwnedLocation(userContext.CompanyId, buyerCompany.Id, buyerJob.Id, thirdPartyOrderViewModel.AddressDetails.IsCompanyOwned, OrderCreationMethod.FromTPO, userContext);
                            }
                        }

                        thirdPartyOrderViewModel.AddressDetails.JobId = buyerJob.Id;

                        if (thirdPartyOrderViewModel.AddressDetails.IsOnsiteContactAvailable())
                        {
                            var buyerOnsiteContact = GetOnsiteContactUser(userContext, thirdPartyOrderViewModel);
                            if (buyerOnsiteContact != null && buyerOnsiteContact.Id == 0)
                            {
                                buyerCompany.Users.Add(buyerOnsiteContact);
                                Context.DataContext.Entry(buyerCompany).State = EntityState.Modified;

                                buyerJob.Users1.Add(buyerOnsiteContact);
                                Context.DataContext.Entry(buyerJob).State = EntityState.Modified;
                                await Context.CommitAsync();

                                thirdPartyOrderViewModel.AddressDetails.OnsiteContactUserId = buyerOnsiteContact.Id;
                            }
                            else
                            {
                                if (buyerOnsiteContact.CompanyId != null && buyerOnsiteContact.CompanyId != buyerCompany.Id)
                                {
                                    transaction.Rollback();
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageUserAlreadyExistsWithAnotherCompanyInSFX, new object[] { buyerOnsiteContact.Email, buyerOnsiteContact.Company.Name });
                                    return response;
                                }
                            }
                        }

                        //check if current job state is included in supplier's serving state
                        response = await new CompanyDomain(this).AddServingStateAsync(userContext, buyerJob.CountryId, buyerJob.StateId);
                        if (response.StatusCode == Status.Failed)
                        {
                            transaction.Rollback();
                            return response;
                        }

                        if (isNewJob && thirdPartyOrderViewModel.AssignedCarrierCompId != null && thirdPartyOrderViewModel.AssignedCarrierCompId > 0)
                        {
                            if (thirdPartyOrderViewModel.AddressDetails.LocationManagedType == LocationManagedType.FullyCarrierManaged)
                            {
                                var JCDresponse = await SaveJobCarrierDetailTPO(userContext.Id, buyerJob.Id, thirdPartyOrderViewModel.AssignedCarrierCompId, userContext.CompanyId, thirdPartyOrderViewModel.CarrierUserEmails);

                                if (JCDresponse.StatusCode == Status.Failed)
                                {
                                    transaction.Rollback();
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = Resource.errFailedToSaveJobCarrierDetails;
                                    return response;
                                }
                            }
                        }

                        //add assets to job
                        if (thirdPartyOrderViewModel.Assets != null && thirdPartyOrderViewModel.Assets.Count > 0)
                        {
                            foreach (var asset in thirdPartyOrderViewModel.Assets)
                            {
                                foreach (var oUser in buyerUser)
                                {
                                    var assignedAsset = oUser.Company.Assets.Where(t => t.Name.ToLower() == asset.Name.ToLower()).Select(t => new { t.Id }).FirstOrDefault();
                                    if (assignedAsset != null)
                                    {
                                        var jobAssets = buyerJob.JobXAssets.Where(t => t.Asset.Id == assignedAsset.Id && t.RemovedBy == null && t.RemovedDate == null).Select(t => new { t.Id }).FirstOrDefault();
                                        if (jobAssets == null)
                                        {
                                            asset.UserId = oUser.Id;
                                            Context.DataContext.JobXAssets.Add(new JobXAsset() { AssetId = assignedAsset.Id, JobId = buyerJob.Id, AssignedBy = buyerJob.Id, AssignedDate = DateTime.Now });
                                        }
                                    }
                                    else
                                    {
                                        asset.UserId = oUser.Id;
                                        var assetEntity = asset.ToEntity();
                                        oUser.Company.Assets.Add(assetEntity);
                                        Context.DataContext.JobXAssets.Add(new JobXAsset() { Asset = assetEntity, JobId = buyerJob.Id, AssignedBy = buyerJob.Id, AssignedDate = DateTime.Now });
                                    }
                                }
                            }
                            await Context.CommitAsync();
                        }

                        //check if product is available in region for specific location
                        if (thirdPartyOrderViewModel.FuelDetails.FuelTypeId.HasValue && thirdPartyOrderViewModel.FuelDetails.FuelTypeId > 0)
                        {
                            var isProductValid = new StatusViewModel();
                            if (thirdPartyOrderViewModel.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.AdditiveFuelType
                                || thirdPartyOrderViewModel.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
                            {
                                isProductValid = await IsValidProductForRegion(thirdPartyOrderViewModel.AddressDetails.JobId, userContext.CompanyId, thirdPartyOrderViewModel.RegionId, thirdPartyOrderViewModel.FuelDetails.FuelTypeId.Value, 0);
                            }
                            else
                            {
                                isProductValid = await IsValidProductForRegion(thirdPartyOrderViewModel.AddressDetails.JobId, userContext.CompanyId, thirdPartyOrderViewModel.RegionId, 0, thirdPartyOrderViewModel.FuelDetails.FuelTypeId.Value);
                            }
                            if (isProductValid.StatusCode == Status.Failed)
                            {
                                response.StatusCode = isProductValid.StatusCode;
                                response.StatusMessage = isProductValid.StatusMessage;
                                return response;
                            }
                        }

                        //Save NonStandardProduct
                        if (thirdPartyOrderViewModel.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
                        {
                            var productDomain = new ProductDomain(this);
                            int productId = 0;
                            foreach (var oUser in buyerUser)
                            {
                                productId = await productDomain.SaveNonStandardProduct(thirdPartyOrderViewModel.FuelDetails.NonStandardFuelName, oUser.Id, buyerJob.Company, thirdPartyOrderViewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId);
                            }
                            thirdPartyOrderViewModel.FuelDetails.FuelTypeId = productId;
                            thirdPartyOrderViewModel.FuelDetails.FuelPricing.FuelTypeId = productId;
                        }

                        //CREATE FR FOR THIS JOB
                        var fuelRequest = await AddFuelRequestFromTPO(thirdPartyOrderViewModel, buyerJob, userContext.CompanyId);

                        buyerJob.FuelRequests.Add(fuelRequest);
                        await Context.CommitAsync();

                        //tier [check pricing data available or not]
                        if (thirdPartyOrderViewModel.FuelDetails.IsTierPricing)
                        {
                            foreach (var item in thirdPartyOrderViewModel.PricingDetails.TierPricing.Pricings)
                            {
                                var cityGroupTerminalId = item.CityGroupTerminalId ?? 0;
                                if (buyerJob.CountryId == (int)Country.USA && cityGroupTerminalId > 0)
                                {
                                    var sourceId = (PricingSource)item.PricingSourceId;
                                    int productId = item.FuelTypeId.Value;  //check code here
                                    if (sourceId == PricingSource.Axxis)
                                    {
                                        productId = GetExternalProductId(item.FuelTypeId.Value);
                                    }
                                    var pricingDataAvailable = await new PricingServiceDomain(this).IsCityRackPriceAvailable(productId, cityGroupTerminalId, sourceId, DateTimeOffset.UtcNow.DateTime);

                                    if (!pricingDataAvailable)
                                    {
                                        transaction.Rollback();
                                        response.StatusCode = Status.Failed;
                                        response.StatusMessage = Resource.errMessageTerminalPriceNotAvailable;
                                        return response;
                                    }
                                }
                            }
                        }
                        else  //non tier
                        {
                            var cityGroupTerminalId = thirdPartyOrderViewModel.PricingDetails.CityGroupTerminalId ?? 0;
                            if (buyerJob.CountryId == (int)Country.USA && cityGroupTerminalId > 0)
                            {
                                var sourceId = (PricingSource)thirdPartyOrderViewModel.PricingDetails.FuelPricingDetails.PricingSourceId;
                                int productId = fuelRequest.FuelTypeId;
                                if (sourceId == PricingSource.Axxis)
                                {
                                    productId = GetExternalProductId(fuelRequest.FuelTypeId);
                                }
                                var pricingDataAvailable = await new PricingServiceDomain(this).IsCityRackPriceAvailable(productId, cityGroupTerminalId, sourceId, DateTimeOffset.UtcNow.DateTime);

                                if (!pricingDataAvailable)
                                {
                                    transaction.Rollback();
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = Resource.errMessageTerminalPriceNotAvailable;
                                    return response;
                                }
                            }
                        }


                        if (thirdPartyOrderViewModel.FuelDeliveryDetails.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery)
                        {
                            thirdPartyOrderViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.RemoveAll(t => t.FeeConstraintTypeId.HasValue);
                        }
                        thirdPartyOrderViewModel.FuelDeliveryDetails.FuelRequestFee.FuelRequestId = fuelRequest.Id;
                        fuelRequest = await SaveFuelRequestFromTPOAsync(thirdPartyOrderViewModel, fuelRequest, userContext);

                        Context.DataContext.Entry(fuelRequest).State = EntityState.Modified;
                        await Context.CommitAsync();

                        var buyerOrder = await AcceptFuelRequestFromTPO(userContext, fuelRequest, thirdPartyOrderViewModel);
                        if (buyerOrder != null)
                        {
                            if (thirdPartyOrderViewModel.CustomerDetails.ContactPersons != null && thirdPartyOrderViewModel.CustomerDetails.ContactPersons.Any())
                            {
                                buyerUser.Where(t => t.Id != thirdPartyOrderViewModel.CustomerDetails.UserId).ToList().ForEach(t => buyerOrder.OrderXUsers.Add(t));
                            }
                            if (thirdPartyOrderViewModel.IsOtherFuelTypeTaxesGiven)
                            {
                                await AddTaxDetailsForOtherFuelType(thirdPartyOrderViewModel, buyerOrder, fuelRequest);
                            }

                            Context.DataContext.Entry(fuelRequest).State = EntityState.Modified;

                            var isNewServingStateAdded = response.StatusCode == Status.Success && !string.IsNullOrWhiteSpace(response.StatusMessage)
                                                        && response.StatusMessage.Equals(Resource.SuccessServingStateAddedToYourAddress, StringComparison.OrdinalIgnoreCase);

                            //if driver selected
                            if (thirdPartyOrderViewModel.IsNewDriver || thirdPartyOrderViewModel.DriverId != null)
                            {
                                var orderDomain = new OrderDomain(this);
                                if (thirdPartyOrderViewModel.IsNewDriver)
                                {
                                    var driverId = Context.DataContext.Users.Where(t => t.Email == thirdPartyOrderViewModel.DriverEmail.Trim().ToLower()).Select(t => t.Id).FirstOrDefault();
                                    response = await orderDomain.AssignDriverAsync(userContext, buyerOrder.Id, driverId);
                                }
                                else
                                {
                                    response = await orderDomain.AssignDriverAsync(userContext, buyerOrder.Id, thirdPartyOrderViewModel.DriverId);
                                }
                                if (response.StatusCode == Status.Failed)
                                {
                                    transaction.Rollback();
                                    response.StatusCode = Status.Failed;
                                    return response;
                                }
                            }

                            await Context.CommitAsync();
                            var fsresponse = await UpdateJobRegionCarrierDetails(userContext, thirdPartyOrderViewModel, isNewJob, buyerJob, buyerOrder);
                            if (fsresponse.StatusCode == Status.Failed)
                            {
                                transaction.Rollback();
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errFailedToSaveJobDetails;
                                return response;
                            }

                            if (thirdPartyOrderViewModel.FuelDetails.IsMarineLocation)
                            {
                                var vessle = Context.DataContext.Assets.Where(t => t.Id == thirdPartyOrderViewModel.AddressDetails.VessleId && t.IsMarine && t.Type == (int)AssetType.Vessle).FirstOrDefault();
                                if (vessle != null && vessle.AssetAdditionalDetail != null)
                                {
                                    vessle.AssetAdditionalDetail.IMONumber = thirdPartyOrderViewModel.AddressDetails.IMONumber;
                                    vessle.AssetAdditionalDetail.Flag = thirdPartyOrderViewModel.AddressDetails.Flag;

                                    Context.DataContext.JobXAssets.Add(new JobXAsset() { AssetId = vessle.Id, JobId = buyerJob.Id, AssignedBy = buyerJob.Id, AssignedDate = DateTime.Now, OrderId = buyerOrder.Id });
                                    Context.DataContext.Entry(vessle).State = EntityState.Modified;

                                    await Context.CommitAsync();
                                }
                            }

                            transaction.Commit();
                            if (!isNewJob)
                            {
                                JobCarrierDetail jobCarrierDetail = new JobCarrierDetail();
                                var preferenceSeting = await ContextFactory.Current.GetDomain<CompanyDomain>().GetPreferencesSettingAsync(0, userContext);
                                if (preferenceSeting.IsFreightOnlyOrderEnabled)
                                {
                                    jobCarrierDetail = await Context.DataContext.JobCarrierDetails.Where(j => j.IsActive &&
                                                                                                                j.JobId == buyerJob.Id &&
                                                                                                                j.CarrierCompanyId == thirdPartyOrderViewModel.AssignedCarrierCompId &&
                                                                                                                j.CreatedByCompanyId == userContext.CompanyId).FirstOrDefaultAsync();
                                    if (jobCarrierDetail != null)
                                    {
                                        var JobIdsToEdit = new EditFreightOnlyOrderViewModel();
                                        JobIdsToEdit.newJobsIds = new List<int>();
                                        JobIdsToEdit.newJobsIds.Add(buyerJob.Id);
                                        JobIdsToEdit.CarrierCompanyId = (int)thirdPartyOrderViewModel.AssignedCarrierCompId;
                                        JobIdsToEdit.IsCreateOrder = true;
                                        var status = new CarrierDomain().EditFreightOnlyOrders(JobIdsToEdit, userContext);
                                    }
                                }
                            }

                            var settingsDomain = new SettingsDomain(this);
                            settingsDomain.SetBuyerSupplierInformation(userContext.CompanyId, buyerCompany.Id, buyerJob.Id, thirdPartyOrderViewModel.AccountingCompanyId, false, OrderCreationMethod.FromTPO, userContext);
                            settingsDomain.SetSupplierIsBadgeMandatory(userContext.CompanyId, buyerCompany.Id, buyerJob.Id, thirdPartyOrderViewModel.IsBadgeMandatory, OrderCreationMethod.FromTPO, userContext);

                            var newsfeedDomain = new NewsfeedDomain(this);
                            await newsfeedDomain.SetThirdPartyOrderCreatedNewsfeed(userContext, fuelRequest, buyerOrder);

                            if (thirdPartyOrderViewModel.AddressDetails.IsProFormaPoEnabled)
                            {
                                await newsfeedDomain.SetProFormaEnableDisableForOrderNewsfeed(userContext, buyerOrder);
                                await notificationDomain.AddNotificationEventAsync(EventType.ProFormaEnabledForOrder, buyerOrder.Id, userContext.Id);
                            }

                            if (thirdPartyOrderViewModel.OrderAdditionalDetailsViewModel.FreightPricingMethod == FreightPricingMethod.Manual && thirdPartyOrderViewModel.OrderAdditionalDetailsViewModel.IsFuelSurcharge)
                            {
                                await AddFuelSurchargeNotificationEvent(thirdPartyOrderViewModel.OrderAdditionalDetailsViewModel.IsFuelSurcharge, buyerOrder.Id, userContext.Id);
                                await newsfeedDomain.SetFuelSurchargeEnableOrDisabledNewsfeed(buyerOrder.Id, buyerOrder.BuyerCompanyId, buyerJob.TimeZoneName, buyerOrder.PoNumber, thirdPartyOrderViewModel.OrderAdditionalDetailsViewModel.IsFuelSurcharge, userContext);
                            }
                            else if (thirdPartyOrderViewModel.OrderAdditionalDetailsViewModel.FreightPricingMethod == FreightPricingMethod.Auto && thirdPartyOrderViewModel.OrderAdditionalDetailsViewModel.IsFuelSurchargeAuto)
                            {
                                await AddFuelSurchargeNotificationEvent(thirdPartyOrderViewModel.OrderAdditionalDetailsViewModel.IsFuelSurchargeAuto, buyerOrder.Id, userContext.Id);
                                await newsfeedDomain.SetFuelSurchargeEnableOrDisabledNewsfeed(buyerOrder.Id, buyerOrder.BuyerCompanyId, buyerJob.TimeZoneName, buyerOrder.PoNumber, thirdPartyOrderViewModel.OrderAdditionalDetailsViewModel.IsFuelSurchargeAuto, userContext);
                            }

                            response.StatusCode = Status.Success;
                            thirdPartyOrderViewModel.OrderId = buyerOrder.Id;
                            thirdPartyOrderViewModel.ExternalOrderNumber = buyerOrder.PoNumber;
                            response.StatusMessage = isNewServingStateAdded ? $"{Resource.errMessageThirdPartyOrderCreatedSuccess}. {Resource.SuccessServingStateAddedToYourAddress}" : Resource.errMessageThirdPartyOrderCreatedSuccess;
                            if (buyerJob.LocationType == JobLocationTypes.Various && (fuelRequest.FuelRequestPricingDetail != null && !fuelRequest.FuelRequestPricingDetail.DisplayPrice.ToLower().Contains("rack")))
                            {
                                response.StatusMessage = String.Concat(response.StatusMessage, Resource.SuccessReviewTerminal);
                            }

                            if (thirdPartyOrderViewModel.AssignedCarrierCompId.HasValue && thirdPartyOrderViewModel.AssignedCarrierCompId.Value > 0 && thirdPartyOrderViewModel.AddressDetails.LocationManagedType == LocationManagedType.FullyCarrierManaged)
                            {
                                await fuelrequestDomain.CreateFreightOnlyOrderQueueMessage(buyerOrder, thirdPartyOrderViewModel.AssignedCarrierCompId.Value);
                            }
                            foreach (var oUser in buyerUser)
                            {
                                if (thirdPartyOrderViewModel.IsInvitationEnabled && oUser.IsEmailConfirmed && oUser.IsOnboardingComplete)
                                {
                                    //insert into supplier invitation - website branding if branding enable.
                                    await SaveSupplierInvitationDetailsAsync(userContext.CompanyId, userContext.Id, oUser.Id);
                                }
                            }


                            if (thirdPartyOrderViewModel.FuelDetails.IsMarineLocation && thirdPartyOrderViewModel.AddressDetails.Country.Id == (int)Country.USA && thirdPartyOrderViewModel.FuelDeliveryDetails.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery)
                            {
                                AddQueueforDRCreation(thirdPartyOrderViewModel, buyerJob, fuelRequest, buyerOrder, buyerUser[0], userContext);
                            }
                            //if (thirdPartyOrderViewModel.OrderAdditionalDetailsViewModel.LocationManagedType == LocationManagedType.FullyCarrierManaged ||
                            //    thirdPartyOrderViewModel.OrderAdditionalDetailsViewModel.LocationManagedType == LocationManagedType.PartialCarrierManaged)
                            //{
                            //    await notificationDomain.AddNotificationEventAsync(EventType.CarrierEmailOrderCreated, buyerOrder.Id, userContext.Id);
                            //}
                        }

                        var orderdtlVersion = buyerOrder.OrderDetailVersions.FirstOrDefault(t => t.IsActive);
                        orderdtlVersion.JsonOrderHistory = JsonConvert.SerializeObject(thirdPartyOrderViewModel);
                        orderdtlVersion.EditPropertyType = EditPropertyType.CreateTPO;
                        Context.DataContext.Entry(orderdtlVersion).State = EntityState.Modified;
                        Context.Commit();

                        response.EntityId = buyerJob.Id;
                        response.CustomerCompanyId = buyerJob.CompanyId;
                        response.CustomerId = buyerUser[0].Id;  // will change it latter, will check use of this field as well
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains(Resource.errMessageFailedSaveFuelPricing.ToLower()))
                        {
                            response.StatusMessage = Resource.errMessageFailedSaveFuelPricing;
                        }
                        else
                        {
                            response.StatusMessage = Resource.errMessageThirdPartyOrderCreationFailed;
                        }
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        LogManager.Logger.WriteException("ThirdPartyDomain", "CreateThirdPartyOrder", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StatusViewModel> CreateOnTheFlyLocation(UserContext userContext, OnTheFlyLocationModel model)
        {
            using (var tracer = new Tracer("ThirdPartyDomain", "CreateOnTheFlyLocation"))
            {
                var response = new StatusViewModel();
                if (!string.IsNullOrWhiteSpace(model.AddressDetails.TimeZoneName))
                {
                    model.FuelDeliveryDetails.StartDate = DateTimeOffset.Now.ToTargetDateTimeOffset(model.AddressDetails.TimeZoneName).Date;
                }

                if (model.AddressDetails.CountryId == (int)Country.CAR)
                {
                    var countryViewModel = Context.DataContext.MstCountries.Single(t => t.Id == model.AddressDetails.CountryId).ToViewModel();
                    var stateViewModel = Context.DataContext.MstStates.Single(t => t.Id == model.AddressDetails.StateId).ToViewModel();
                    model.AddressDetails.Country = countryViewModel;
                    model.AddressDetails.State = stateViewModel;
                    if (string.IsNullOrWhiteSpace(model.AddressDetails.Address))
                        model.AddressDetails.Address = stateViewModel.Name ?? Resource.lblCaribbean;
                    if (string.IsNullOrWhiteSpace(model.AddressDetails.City))
                        model.AddressDetails.City = stateViewModel.Name ?? Resource.lblCaribbean;
                    if (string.IsNullOrWhiteSpace(model.AddressDetails.CountyName))
                        model.AddressDetails.CountyName = stateViewModel.Name ?? Resource.lblCaribbean;
                    if (string.IsNullOrWhiteSpace(model.AddressDetails.ZipCode))
                        model.AddressDetails.ZipCode = stateViewModel.Name ?? Resource.lblCaribbean;
                }

                //check if product is available in region for specific location
                if (model.FuelDetails.FuelTypeId.HasValue && model.FuelDetails.FuelTypeId > 0)
                {
                    var isProductValid = new StatusViewModel();
                    if (model.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.AdditiveFuelType
                                || model.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
                    {
                        isProductValid = await IsValidProductForRegion(model.AddressDetails.JobId, userContext.CompanyId, model.RegionId, model.FuelDetails.FuelTypeId.Value, 0);
                    }
                    else
                    {
                        isProductValid = await IsValidProductForRegion(model.AddressDetails.JobId, userContext.CompanyId, model.RegionId, 0, model.FuelDetails.FuelTypeId.Value);
                    }
                    if (isProductValid.StatusCode == Status.Failed)
                    {
                        response.StatusCode = isProductValid.StatusCode;
                        response.StatusMessage = isProductValid.StatusMessage;
                        return response;
                    }

                }
                var thirdPartyOrderViewModel = model.ToThirdPartyOrderViewModel();

                var fuelrequestDomain = new FuelRequestDomain(this);
                if (thirdPartyOrderViewModel.FuelDetails.IsTierPricing && thirdPartyOrderViewModel.FuelDetails.TierPricing.Pricings == null)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageTierPricingRequired;
                    return response;
                }
                if (thirdPartyOrderViewModel.FuelDetails.IsTierPricing && thirdPartyOrderViewModel.FuelDetails.TierPricing.Pricings.Any())
                {
                    thirdPartyOrderViewModel.FuelDetails.TierPricing.Pricings.ForEach(e => e.FuelTypeId = thirdPartyOrderViewModel.FuelDetails.FuelTypeId);
                    thirdPartyOrderViewModel.PricingDetails.TierPricing = thirdPartyOrderViewModel.FuelDetails.TierPricing;
                    thirdPartyOrderViewModel.PricingDetails.PricingTypeId = (int)PricingType.Tier;
                }


                thirdPartyOrderViewModel.FuelDeliveryDetails.TruckLoadTypes = thirdPartyOrderViewModel.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypes;

                //tier [fuel cost pricing type]
                if (thirdPartyOrderViewModel.FuelDetails.IsTierPricing && thirdPartyOrderViewModel.PricingDetails.TierPricing.Pricings != null &&
                 thirdPartyOrderViewModel.PricingDetails.TierPricing.Pricings.Where(w => w.PricingTypeId == (int)PricingType.Suppliercost).Any() && thirdPartyOrderViewModel.FuelDetails.FuelTypeId.HasValue)
                {
                    var uom = thirdPartyOrderViewModel.AddressDetails.Country.UoM;
                    if (thirdPartyOrderViewModel.FuelDetails.IsMarineLocation && thirdPartyOrderViewModel.FuelDetails.FuelQuantity.QuantityTypeId == (int)QuantityType.NotSpecified)
                    {
                        uom = thirdPartyOrderViewModel.AddressDetails.MarineUoM;
                    }
                    else if (thirdPartyOrderViewModel.FuelDetails.IsMarineLocation && thirdPartyOrderViewModel.FuelDetails.FuelQuantity.QuantityTypeId != (int)QuantityType.NotSpecified)
                    {
                        uom = thirdPartyOrderViewModel.FuelDetails.FuelQuantity.UoM;
                    }

                    var availableGlobalCost = await new CurrentCostDomain(this).GetGlobalCost(userContext, thirdPartyOrderViewModel.FuelDetails.FuelTypeId.Value, thirdPartyOrderViewModel.AddressDetails.State.Id, uom, thirdPartyOrderViewModel.AddressDetails.Country.Currency);
                    if (availableGlobalCost == 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.ErrorGlobalCostNotProvided;
                        return response;
                    }
                    else
                    {
                        thirdPartyOrderViewModel.PricingDetails.TierPricing.Pricings.Where(w => w.PricingTypeId == (int)PricingType.Suppliercost).Select(s => s.SupplierCost = availableGlobalCost);
                    }
                }

                //non tier
                if (thirdPartyOrderViewModel.PricingDetails.PricingTypeId == (int)PricingType.Suppliercost && thirdPartyOrderViewModel.FuelDetails.FuelTypeId.HasValue)
                {
                    var uom = thirdPartyOrderViewModel.AddressDetails.Country.UoM;
                    //if (thirdPartyOrderViewModel.FuelDetails.IsMarineLocation && thirdPartyOrderViewModel.FuelDetails.FuelQuantity.QuantityTypeId == (int)QuantityType.NotSpecified)
                    //    uom = thirdPartyOrderViewModel.AddressDetails.MarineUoM;
                    //else if (thirdPartyOrderViewModel.FuelDetails.IsMarineLocation && thirdPartyOrderViewModel.FuelDetails.FuelQuantity.QuantityTypeId != (int)QuantityType.NotSpecified)
                    //    uom = thirdPartyOrderViewModel.FuelDetails.FuelQuantity.UoM;
                    var availableGlobalCost = await new CurrentCostDomain(this).GetGlobalCost(userContext, thirdPartyOrderViewModel.FuelDetails.FuelTypeId.Value, thirdPartyOrderViewModel.AddressDetails.State.Id, uom, thirdPartyOrderViewModel.AddressDetails.Country.Currency);
                    if (availableGlobalCost == 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.ErrorGlobalCostNotProvided;
                        return response;
                    }
                    else
                    {
                        thirdPartyOrderViewModel.PricingDetails.SupplierCost = availableGlobalCost;
                    }
                }

                int LengthOfPassword = Constants.LengthOfPassword;
                var RandomPassword = CryptoHelperMethods.GenerateRandomPassword(LengthOfPassword);
                var TempPassword = CryptoHelperMethods.EncryptPassword(Constants.Key.ToString(), RandomPassword);

                var buyerUser = CreateUserForThridPartyOrderAsync(thirdPartyOrderViewModel, userContext, RandomPassword);
                var buyerCompany = CreateCompanyForThridPartyOrder(thirdPartyOrderViewModel);

                //bool isNewJob = false;
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {

                        if (buyerUser.CompanyId != null && buyerUser.CompanyId != buyerCompany.Id)
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageUserAlreadyExistsWithAnotherCompanyInSFX, new object[] { buyerUser.Email, buyerUser.Company.Name });
                            return response;
                        }

                        //Add user and default role as admin
                        if (buyerUser.Id == 0)
                        {
                            buyerUser.MstRoles.Add(Context.DataContext.MstRoles.Single(t => t.Id == (int)UserRoles.Admin));
                            Context.DataContext.Users.Add(buyerUser);
                            await Context.CommitAsync();

                            string eventTypeId = string.Empty;
                            if (thirdPartyOrderViewModel.IsInvitationEnabled && !buyerUser.IsEmailConfirmed && !buyerUser.IsOnboardingComplete)
                            {
                                var authDomain = new AuthenticationDomain(fuelrequestDomain);

                                var userRoleIds = buyerUser.MstRoles.Select(t => t.Id).ToList();
                                var defaultEnabled = authDomain.GetDefaultEnabledNotifications(userRoleIds, buyerCompany.CompanyTypeId);
                                if (defaultEnabled != null && defaultEnabled.Any())
                                {
                                    eventTypeId = string.Join(",", defaultEnabled);
                                }
                            }

                            if (thirdPartyOrderViewModel.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypes == TruckLoadTypes.FullTruckLoad)
                            {
                                eventTypeId = eventTypeId + "," + ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingNotificationPreferencesForFtlOrder);
                            }

                            var eventTypeIds = eventTypeId.TrimStart(',').Split(',').Distinct().ToList();
                            var sqlQuery = Context.DataContext.MstCompanyUserRoleXEventTypes.Where(t => t.NotificationType != (int)NotificationType.Nothing
                                            && t.RoleId == (int)UserRoles.Admin && t.CompanyTypeId == buyerCompany.CompanyTypeId)
                                            .Select(t => new { t.EventTypeId });

                            var notificationSettings = new List<UserXNotificationSetting>();
                            foreach (var item in sqlQuery)
                            {
                                bool isEmail = false;
                                if (eventTypeIds.Contains(item.EventTypeId.ToString()))
                                {
                                    isEmail = true;
                                }
                                var setting = new AuthenticationDomain(this).GetNotificationSetting(buyerUser.Id, item.EventTypeId, isEmail);
                                notificationSettings.Add(setting);
                            }
                            buyerUser.UserXNotificationSettings = notificationSettings.Distinct().ToList();
                        }

                        thirdPartyOrderViewModel.CustomerDetails.UserId = buyerUser.Id;

                        NotificationDomain notificationDomain = new NotificationDomain(this);
                        if (buyerCompany.Id == 0)
                        {
                            buyerCompany.CreatedBy = buyerUser.Id;
                            buyerCompany.UpdatedBy = buyerUser.Id;
                            Context.DataContext.Companies.Add(buyerCompany);
                            await Context.CommitAsync();

                            var companyAddress = thirdPartyOrderViewModel.ToCompanyAddressEntity();
                            if (companyAddress != null && companyAddress.StateId > 0)
                            {
                                buyerCompany.CompanyAddresses.Add(companyAddress);
                                Context.DataContext.Entry(buyerCompany).State = EntityState.Modified;
                            }
                        }

                        if (thirdPartyOrderViewModel.IsInvitationEnabled && !buyerUser.IsEmailConfirmed && !buyerUser.IsOnboardingComplete)
                        {
                            await notificationDomain.AddNotificationEventAsync(EventType.TPOUserInvitedForEULAAcceptance, buyerUser.Id, userContext.Id, null, TempPassword);
                            // send buyer onboard invitation email
                            await SaveInvitationSendDetailsAsync(buyerUser.Email, buyerUser.Id, userContext.Id, buyerUser.FirstName, buyerUser.LastName);
                            // update buyer user for onboarding
                            buyerUser.IsEmailConfirmed = true;
                            buyerUser.IsOnboardingComplete = true;
                            buyerUser.IsActive = true;
                            buyerCompany.IsActive = true;
                        }

                        if (buyerUser.Company == null)
                        {
                            buyerUser.Company = buyerCompany;
                        }

                        thirdPartyOrderViewModel.CustomerDetails.CompanyId = buyerCompany.Id;

                        //CREATE OR USE EXISTING JOB
                        var buyerJob = CreateJobFromTPOAsync(thirdPartyOrderViewModel);

                        if (buyerJob == null)
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageJobCreateFailedInvalidAddress;
                            return response;
                        }

                        if (buyerJob.TerminalId == null || buyerJob.TerminalId == 0)
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageJobCreateNoTerminalFound;
                            return response;
                        }

                        if ((buyerJob.Latitude == 0 || buyerJob.Longitude == 0) && thirdPartyOrderViewModel.AddressDetails.IsVarious)
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageJobCreateFailedInvalidAddress;
                            return response;
                        }

                        if (buyerJob.Id == 0)
                        {
                            //isNewJob = true;
                            buyerJob.CreatedByCompanyId = userContext.CompanyId;
                            buyerUser.Company.Jobs.Add(buyerJob);
                            Context.DataContext.Entry(buyerUser).State = EntityState.Modified;
                            await Context.CommitAsync();
                        }

                        thirdPartyOrderViewModel.AddressDetails.JobId = buyerJob.Id;

                        //Save NonStandardProduct
                        if (thirdPartyOrderViewModel.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
                        {
                            var productDomain = new ProductDomain(this);
                            var productId = await productDomain.SaveNonStandardProduct(thirdPartyOrderViewModel.FuelDetails.NonStandardFuelName, buyerUser.Id, buyerJob.Company, thirdPartyOrderViewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId);
                            thirdPartyOrderViewModel.FuelDetails.FuelTypeId = productId;
                            thirdPartyOrderViewModel.FuelDetails.FuelPricing.FuelTypeId = productId;
                        }

                        thirdPartyOrderViewModel.FuelDetails.FuelQuantity.UoM = thirdPartyOrderViewModel.AddressDetails.Country.UoM;
                        //CREATE FR FOR THIS JOB
                        var fuelRequest = await AddFuelRequestFromTPO(thirdPartyOrderViewModel, buyerJob, userContext.CompanyId);

                        buyerJob.FuelRequests.Add(fuelRequest);
                        await Context.CommitAsync();

                        //tier [check pricing data available or not]
                        if (thirdPartyOrderViewModel.FuelDetails.IsTierPricing)
                        {
                            foreach (var item in thirdPartyOrderViewModel.PricingDetails.TierPricing.Pricings)
                            {
                                var cityGroupTerminalId = item.CityGroupTerminalId ?? 0;
                                if (buyerJob.CountryId == (int)Country.USA && cityGroupTerminalId > 0)
                                {
                                    var sourceId = (PricingSource)item.PricingSourceId;
                                    int productId = item.FuelTypeId.Value;  //check code here
                                    if (sourceId == PricingSource.Axxis)
                                    {
                                        productId = GetExternalProductId(item.FuelTypeId.Value);
                                    }
                                    var pricingDataAvailable = await new PricingServiceDomain(this).IsCityRackPriceAvailable(productId, cityGroupTerminalId, sourceId, DateTimeOffset.UtcNow.DateTime);

                                    if (!pricingDataAvailable)
                                    {
                                        transaction.Rollback();
                                        response.StatusCode = Status.Failed;
                                        response.StatusMessage = Resource.errMessageTerminalPriceNotAvailable;
                                        return response;
                                    }
                                }
                            }
                        }
                        else  //non tier
                        {
                            var cityGroupTerminalId = thirdPartyOrderViewModel.PricingDetails.CityGroupTerminalId ?? 0;
                            if (buyerJob.CountryId == (int)Country.USA && cityGroupTerminalId > 0)
                            {
                                var sourceId = (PricingSource)thirdPartyOrderViewModel.PricingDetails.FuelPricingDetails.PricingSourceId;
                                int productId = fuelRequest.FuelTypeId;
                                if (sourceId == PricingSource.Axxis)
                                {
                                    productId = GetExternalProductId(fuelRequest.FuelTypeId);
                                }
                                var pricingDataAvailable = await new PricingServiceDomain(this).IsCityRackPriceAvailable(productId, cityGroupTerminalId, sourceId, DateTimeOffset.UtcNow.DateTime);

                                if (!pricingDataAvailable)
                                {
                                    transaction.Rollback();
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = Resource.errMessageTerminalPriceNotAvailable;
                                    return response;
                                }
                            }
                        }


                        if (thirdPartyOrderViewModel.FuelDeliveryDetails.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery)
                        {
                            thirdPartyOrderViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.RemoveAll(t => t.FeeConstraintTypeId.HasValue);
                        }
                        thirdPartyOrderViewModel.FuelDeliveryDetails.FuelRequestFee.FuelRequestId = fuelRequest.Id;
                        fuelRequest = await SaveFuelRequestFromTPOAsync(thirdPartyOrderViewModel, fuelRequest, userContext);

                        Context.DataContext.Entry(fuelRequest).State = EntityState.Modified;
                        await Context.CommitAsync();

                        var buyerOrder = await AcceptFuelRequestFromTPO(userContext, fuelRequest, thirdPartyOrderViewModel);
                        if (buyerOrder != null)
                        {
                            Context.DataContext.Entry(fuelRequest).State = EntityState.Modified;

                            //check if current job state is included in supplier's serving state
                            response = await new CompanyDomain(this).AddServingStateAsync(userContext, buyerJob.CountryId, buyerJob.StateId);
                            if (response.StatusCode == Status.Failed)
                            {
                                transaction.Rollback();
                                return response;
                            }

                            var isNewServingStateAdded = response.StatusCode == Status.Success && !string.IsNullOrWhiteSpace(response.StatusMessage)
                                                        && response.StatusMessage.Equals(Resource.SuccessServingStateAddedToYourAddress, StringComparison.OrdinalIgnoreCase);

                            await Context.CommitAsync();

                            var freightserviceDomain = new FreightServiceDomain(this);
                            if (buyerOrder.TerminalId > 0 && (model.DeliveryRequest.Terminal == null || model.DeliveryRequest.Terminal.Id == 0) && (model.DeliveryRequest.Bulkplant == null || string.IsNullOrWhiteSpace(model.DeliveryRequest.Bulkplant.Address)))
                            {
                                var terminal = Context.DataContext.MstExternalTerminals.Where(t => t.Id == buyerOrder.TerminalId.Value).Select(t => new { t.Id, t.Name }).FirstOrDefault();
                                model.DeliveryRequest.Terminal = new DropdownDisplayItem() { Id = terminal.Id, Name = terminal.Name };
                            }
                            var fsresponse = await UpdateJobRegionCarrierDetails(userContext, thirdPartyOrderViewModel, true, buyerJob, buyerOrder);
                            if (fsresponse.StatusCode == Status.Failed)
                            {
                                transaction.Rollback();
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errFailedToSaveJobDetails;
                                return response;
                            }
                            transaction.Commit();
                            if (!model.IsTBDRequest)
                            {
                                var raiseDrInput = model.ToRaiseDeliveryRequestInput();
                                raiseDrInput.OrderId = buyerOrder.Id;
                                raiseDrInput.PoNumber = buyerOrder.PoNumber;
                                var createDrStatus = await freightserviceDomain.RaiseDeliveryRequests(new List<RaiseDeliveryRequestInput>() { raiseDrInput }, userContext);
                                if (createDrStatus.StatusCode == Status.Failed)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = createDrStatus.StatusMessage;
                                    return response;
                                }
                            }
                            var settingsDomain = new SettingsDomain(this);
                            settingsDomain.SetBuyerSupplierInformation(userContext.CompanyId, buyerCompany.Id, buyerJob.Id, thirdPartyOrderViewModel.AccountingCompanyId, false, OrderCreationMethod.FromTPO, userContext);
                            settingsDomain.SetSupplierIsBadgeMandatory(userContext.CompanyId, buyerCompany.Id, buyerJob.Id, thirdPartyOrderViewModel.IsBadgeMandatory, OrderCreationMethod.FromTPO, userContext);

                            var newsfeedDomain = new NewsfeedDomain(this);
                            await newsfeedDomain.SetThirdPartyOrderCreatedNewsfeed(userContext, fuelRequest, buyerOrder);
                            response.StatusCode = Status.Success;
                            thirdPartyOrderViewModel.OrderId = buyerOrder.Id;
                            thirdPartyOrderViewModel.ExternalOrderNumber = buyerOrder.PoNumber;
                            response.StatusMessage = isNewServingStateAdded ? $"{Resource.errMessageThirdPartyOrderCreatedSuccess}. {Resource.SuccessServingStateAddedToYourAddress}" : Resource.errMessageThirdPartyOrderCreatedSuccess;
                            if (buyerJob.LocationType == JobLocationTypes.Various && (fuelRequest.FuelRequestPricingDetail != null && !fuelRequest.FuelRequestPricingDetail.DisplayPrice.ToLower().Contains("rack")))
                            {
                                response.StatusMessage = String.Concat(response.StatusMessage, Resource.SuccessReviewTerminal);
                            }
                            if (thirdPartyOrderViewModel.IsInvitationEnabled && buyerUser.IsEmailConfirmed && buyerUser.IsOnboardingComplete)
                            {
                                //insert into supplier invitation - website branding if branding enable.
                                await SaveSupplierInvitationDetailsAsync(userContext.CompanyId, userContext.Id, buyerUser.Id);
                            }
                        }
                        if (response.StatusCode != Status.Failed)
                        {
                            response.StatusMessage = string.Format(Resource.errMessageJobCreateSuccess, model.AddressDetails.JobName);
                        }
                        response.EntityId = buyerJob.Id;
                        response.CustomerCompanyId = buyerJob.CompanyId;
                        response.CustomerId = buyerUser.Id;
                        response.ResponseData = new
                        {
                            OrderId = buyerOrder.Id,
                            JobId = buyerJob.Id,
                            JobName = buyerJob.Name,
                            CustomerCompanyId = buyerJob.CompanyId,
                            CustomerCompanyName = model.CustomerDetails.CompanyName,
                            FuelTypeId = fuelRequest.FuelTypeId,
                            UoM = buyerJob.UoM,
                            Currency = buyerJob.Currency,
                            Address = buyerJob.Address,
                            City = buyerJob.City
                        };
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains(Resource.errMessageFailedSaveFuelPricing.ToLower()))
                        {
                            response.StatusMessage = Resource.errMessageFailedSaveFuelPricing;
                        }
                        else
                        {
                            response.StatusMessage = Resource.errMessageThirdPartyOrderCreationFailed;
                        }
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        LogManager.Logger.WriteException("ThirdPartyDomain", "CreateOnTheFlyLocation", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StatusViewModel> SaveJobCarrierDetailTPO(int userId, int buyerJobId, int? AssignedCarrierCompId, int CreatedByCompanyId, List<int> CarrierUserEmailIds)
        {
            StatusViewModel status = new StatusViewModel();

            if (buyerJobId > 0)
            {
                JobCarrierDetail jobCarrierDetail = new JobCarrierDetail();
                jobCarrierDetail.JobId = buyerJobId;
                jobCarrierDetail.CarrierCompanyId = (int)AssignedCarrierCompId;
                jobCarrierDetail.IsActive = true;
                jobCarrierDetail.CreatedBy = userId;
                jobCarrierDetail.CreatedByCompanyId = CreatedByCompanyId;
                jobCarrierDetail.CreatedDate = DateTimeOffset.Now;

                Context.DataContext.JobCarrierDetails.Add(jobCarrierDetail);
                await Context.CommitAsync();

                if (CarrierUserEmailIds != null && CarrierUserEmailIds.Count > 0)
                {
                    foreach (var carrieruserId in CarrierUserEmailIds)
                    {
                        CarrierEmailSetting carrierEmailSetting = new CarrierEmailSetting();
                        carrierEmailSetting.JobId = buyerJobId;
                        carrierEmailSetting.CarrierCompanyId = (int)AssignedCarrierCompId;
                        carrierEmailSetting.UserId = carrieruserId;
                        carrierEmailSetting.IsActive = true;
                        carrierEmailSetting.CreatedBy = userId;
                        carrierEmailSetting.UpdatedBy = userId;
                        carrierEmailSetting.CreatedDate = DateTimeOffset.Now;
                        carrierEmailSetting.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.CarrierEmailSettings.Add(carrierEmailSetting);
                        await Context.CommitAsync();
                    }

                    Context.DataContext.JobCarrierDetails.Add(jobCarrierDetail);
                    await Context.CommitAsync();
                }

                status.StatusCode = Status.Success;
            }
            return status;
        }


        public async Task<StatusViewModel> SaveJobCarrierDetailJob(List<SupplierCarrierViewModel> carriers, int userId, int CreatedByCompanyId)
        {
            StatusViewModel status = new StatusViewModel();
            List<JobCarrierDetail> objJobCarrierList = new List<JobCarrierDetail>();

            if (carriers.Count > 0)
            {
                foreach (var carrier in carriers)
                {
                    foreach (var job in carrier.Jobs)
                    {
                        JobCarrierDetail jobCarrierDetail = new JobCarrierDetail();
                        jobCarrierDetail.CarrierCompanyId = (int)carrier.Carrier.Id;
                        jobCarrierDetail.JobId = job.Job.Id;
                        jobCarrierDetail.IsActive = true;
                        jobCarrierDetail.CreatedBy = (int)userId;
                        jobCarrierDetail.CreatedByCompanyId = CreatedByCompanyId;
                        jobCarrierDetail.CreatedDate = DateTimeOffset.Now;
                        objJobCarrierList.Add(jobCarrierDetail);

                        if (job.Job.Emails != null && job.Job.Emails.Count > 0)
                        {
                            foreach (var carrieruser in job.Job.Emails)
                            {
                                CarrierEmailSetting carrierEmailSetting = new CarrierEmailSetting();
                                carrierEmailSetting.JobId = job.Job.Id;
                                carrierEmailSetting.CarrierCompanyId = carrier.Carrier.Id;
                                carrierEmailSetting.UserId = carrieruser.Id;
                                carrierEmailSetting.IsActive = true;
                                carrierEmailSetting.CreatedBy = userId;
                                carrierEmailSetting.UpdatedBy = userId;
                                carrierEmailSetting.CreatedDate = DateTimeOffset.Now;
                                carrierEmailSetting.UpdatedDate = DateTimeOffset.Now;
                                Context.DataContext.CarrierEmailSettings.Add(carrierEmailSetting);
                                //await Context.CommitAsync();
                            }

                            //Context.DataContext.JobCarrierDetails.Add(jobCarrierDetail);
                            //await Context.CommitAsync();
                        }
                    }
                }
                Context.DataContext.JobCarrierDetails.AddRange(objJobCarrierList);
                await Context.CommitAsync();

                status.StatusCode = Status.Success;
            }
            return status;
        }

        private async Task<StatusViewModel> UpdateJobRegionCarrierDetails(UserContext userContext, ThirdPartyOrderViewModel thirdPartyOrderViewModel, bool isNewJob, Job buyerJob, Order buyerOrder)
        {
            StatusViewModel status = new StatusViewModel();
            JobToRegionAssignViewModel jobToRegion = null;
            JobViewModel jobModel = null;
            List<SupplierCarrierViewModel> supplierCarriers = null;
            if (!string.IsNullOrWhiteSpace(thirdPartyOrderViewModel.RegionId))
            {
                jobToRegion = new JobToRegionAssignViewModel()
                {
                    RegionId = thirdPartyOrderViewModel.RegionId, //RegionId is not mandatory field.If its null that logic is handld in Region Repo
                    JobId = buyerJob.Id,
                    JobName = buyerJob.Name,
                    UpdatedBy = userContext.Id,
                    CompanyId = userContext.CompanyId,
                    RouteId = thirdPartyOrderViewModel.RouteId
                };
            }
            if (isNewJob)
            {
                jobModel = new JobViewModel()
                {
                    Id = buyerJob.Id,
                    JobID = thirdPartyOrderViewModel.AddressDetails.DisplayJobID,
                    Name = thirdPartyOrderViewModel.AddressDetails.JobName,
                    IsAutoCreateDREnable = thirdPartyOrderViewModel.AddressDetails.IsAutomateDeliveryRequest,
                    TrailerType = thirdPartyOrderViewModel.TrailerType,
                    // save the third party site/additional image details.
                    SiteImage = new ImageViewModel()
                    {
                        FilePath = thirdPartyOrderViewModel.ImageDetails.SiteImage?.FilePath
                    },
                    AdditionalImage = new AdditionalSiteImage()
                    {
                        SiteImage = new ImageViewModel()
                        {
                            FilePath = thirdPartyOrderViewModel.ImageDetails.AdditionalImage.SiteImage?.FilePath,
                        },
                        Description = thirdPartyOrderViewModel.ImageDetails.AdditionalImage?.Description
                    },
                    DistanceCovered = string.IsNullOrEmpty(thirdPartyOrderViewModel.AddressDetails.DistanceCovered) ? null : thirdPartyOrderViewModel.AddressDetails.DistanceCovered
                };
            }
            if (thirdPartyOrderViewModel.AssignedCarrierCompId.HasValue && thirdPartyOrderViewModel.AssignedCarrierCompId.Value > 0)
            {
                supplierCarriers = AssignJobToCarrier(userContext, thirdPartyOrderViewModel.AssignedCarrierCompId.Value, buyerOrder);
            }

            if (jobToRegion != null || jobModel != null || supplierCarriers != null)
            {
                status = await new FreightServiceDomain().SaveJobRegionCarrierDetails(jobToRegion, jobModel, supplierCarriers);
            }
            else
            {
                status.StatusCode = Status.Success;
            }
            return status;
        }

        private List<SupplierCarrierViewModel> AssignJobToCarrier(UserContext userContext, int carrierCompId, Order buyerOrder)
        {
            //add job to carrier assignment in freight service - do we need this??
            List<SupplierCarrierViewModel> carriers = new List<SupplierCarrierViewModel>();
            var carrierdropdown = Context.DataContext.Companies.Where(t => t.Id == carrierCompId)
                                    .Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.Name }).FirstOrDefault();
            var supCarViewModel = new SupplierCarrierViewModel()
            {
                Carrier = carrierdropdown,
                CreatedBy = userContext.Id,
                CreatedOn = DateTimeOffset.Now,
                IsActive = true,
                IsDeleted = false,
                SupplierCompanyId = userContext.CompanyId,
                SupplierCompanyName = userContext.CompanyName
            };
            supCarViewModel.Jobs = new List<CarrierJobViewModel>();
            supCarViewModel.Jobs.Add(new CarrierJobViewModel()
            {
                BuyerCompanyId = buyerOrder.BuyerCompanyId,
                BuyerCompanyName = buyerOrder.BuyerCompany.Name,
                Job = new JobWithEmails() { Id = buyerOrder.FuelRequest.JobId, Name = buyerOrder.FuelRequest.Job.Name, Emails = null }
            });

            carriers.Add(supCarViewModel);
            return carriers;
        }

        public async Task AddFuelSurchargeNotificationEvent(bool isFuelSurcharge, int orderId, int userId)
        {
            NotificationDomain notificationDomain = new NotificationDomain(this);
            string fuelSurchargeStatus = Resource.lblDisabled;
            if (isFuelSurcharge)
            {
                fuelSurchargeStatus = Resource.lblEnabled;
            }

            var message = new OrderFuelSurchargeMessageViewModel { FuelSurchargeStatus = fuelSurchargeStatus };
            var jsonMessage = new JavaScriptSerializer().Serialize(message);
            await notificationDomain.AddNotificationEventAsync(EventType.FuelSurchargeStatusChangedForOrder, orderId, userId, null, jsonMessage);
        }

        private async Task<OrderTaxDetail> AddTaxDetailsForOtherFuelType(ThirdPartyOrderViewModel thirdPartyOrderViewModel, Order buyerOrder, FuelRequest fuelRequest)
        {
            if (fuelRequest.FuelTypeId > 0 && buyerOrder.Id > 0)
            {
                var listOfTaxDetails = new List<OrderTaxDetail>();
                foreach (var item in thirdPartyOrderViewModel.TaxDetailsViewModel)
                {
                    var orderTaxDetails = new OrderTaxDetail();
                    orderTaxDetails.AddedBy = buyerOrder.AcceptedBy;
                    orderTaxDetails.AddedByCompanyId = buyerOrder.AcceptedCompanyId;
                    orderTaxDetails.AddedDate = DateTimeOffset.Now;
                    orderTaxDetails.IsActive = true;
                    orderTaxDetails.Order = buyerOrder;
                    orderTaxDetails.OtherFuelTypeId = fuelRequest.FuelTypeId;
                    orderTaxDetails.TaxDescription = item.TaxDescription;
                    orderTaxDetails.TaxPricingTypeId = item.TaxPricingTypeId;
                    orderTaxDetails.TaxRate = item.TaxRate;
                    listOfTaxDetails.Add(orderTaxDetails);
                }

                Context.DataContext.OrderTaxDetails.AddRange(listOfTaxDetails);
                await Context.CommitAsync();

                return null;
            }
            return null;
        }

        public async Task<Response> AddDriverForTPO(UserContext userContext, ThirdPartyOrderViewModel thirdPartyOrderViewModel)
        {
            var response = new Response(Status.Failed);
            response.StatusMessages.Add(ResourceMessages.GetMessage(Resource.valMessageAlreadyExist, new object[] { thirdPartyOrderViewModel.DriverEmail }));
            var existingUser = Context.DataContext.Users.Any(t => t.Email.ToLower().Equals(thirdPartyOrderViewModel.DriverEmail.ToLower().Trim()));
            var isInvited = Context.DataContext.CompanyXAdditionalUserInvites.Any(t => t.Email.ToLower().Equals(thirdPartyOrderViewModel.DriverEmail.ToLower().Trim()));

            if (!existingUser && !isInvited)
            {
                var roleId = new List<int> { (int)UserRoles.Driver };
                var drivers = new List<AdditionalUserViewModel>();
                var driver = new AdditionalUserViewModel()
                {
                    CompanyId = userContext.CompanyId,
                    FirstName = thirdPartyOrderViewModel.DriverFirstName.Trim(),
                    LastName = thirdPartyOrderViewModel.DriverLastName.Trim(),
                    Email = thirdPartyOrderViewModel.DriverEmail.ToLower().Trim(),
                    RoleIds = roleId,
                    DisplayMode = PageDisplayMode.Create,
                    UpdatedDate = DateTimeOffset.Now,
                    IsInvitationSent = false
                };
                drivers.Add(driver);
                var newdrivers = new AdditionalUsersViewModel() { UserId = userContext.Id, AdditionalUsers = drivers };
                var settingDomain = new SettingsDomain(this);
                response = await settingDomain.AddCompanyUser(newdrivers);
            }
            return response;
        }
        public async Task<StatusViewModel> UpdateOrderSourceRegionsAsync(SourceRegionsViewModel viewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (viewModel.FreightPricingMethod == FreightPricingMethod.Auto)
                    {
                        if (viewModel.SelectedSourceRegions.Count == 0)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageSourceRegionRequired;
                            return response;
                        }
                        if (!(viewModel.SelectedTerminals.Count > 0 || viewModel.SelectedBulkPlants.Count > 0))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageTerminalOrBulkPlantRequired;
                            return response;
                        }
                        var order = Context.DataContext.Orders.Where(w => w.Id == viewModel.OrderId).FirstOrDefault();
                        if (order != null)
                        {
                            order.OrderAdditionalDetail.FreightPricingMethod = viewModel.FreightPricingMethod;
                            order.FuelDispatchLocations.ToList().ForEach(s => s.IsActive = false);
                            if (viewModel.SelectedBulkPlants != null && viewModel.ApprovedBulkPlantId != 0)
                            {
                                var bulkPlant = Context.DataContext.BulkPlantLocations.Where(w => w.Id == viewModel.ApprovedBulkPlantId && w.IsActive).FirstOrDefault();
                                if (bulkPlant != null)
                                {
                                    SetFuelDispatchLocation(userContext, order.FuelRequest, order, bulkPlant);
                                }
                            }

                            if (viewModel.ApprovedTerminalId != 0)
                            {
                                order.TerminalId = viewModel.ApprovedTerminalId;
                            }
                            else if (viewModel.ApprovedTerminalId == 0 && viewModel.SelectedTerminals.Count == 1)
                            {
                                order.TerminalId = viewModel.SelectedTerminals.FirstOrDefault();
                            }
                            else if (viewModel.ApprovedTerminalId == 0 && viewModel.SelectedTerminals.Count > 1)
                            {
                                var IsSupressPricing = Context.DataContext.OnboardingPreferences
                                                                                      .Where(t => t.IsActive && t.CompanyId == userContext.CompanyId)
                                                                                      .OrderByDescending(t => t.Id).FirstOrDefault().IsSupressOrderPricing;

                                var inputModel = new SourceRegionRequestModel()
                                {
                                    TerminalIds = viewModel.SelectedTerminals,
                                    FuelTypeId = order.FuelRequest.FuelTypeId,
                                    PricingCodeId = order.FuelRequest.FuelRequestPricingDetail.PricingCodeId,
                                    Latitude = order.FuelRequest.Job.Latitude,
                                    Longitude = order.FuelRequest.Job.Longitude,
                                    IsSupressPricing = IsSupressPricing,
                                };
                                ExternalPricingDomain externalPricingDomain = new ExternalPricingDomain();
                                int companyCountryId = await Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetDefaultServingCountry(userContext.CompanyId));
                                var terminals = await externalPricingDomain.GetClosestTerminalsForSourceRegions(userContext.CompanyId, companyCountryId, inputModel);
                                if (terminals != null && terminals.Any())
                                {
                                    order.TerminalId = terminals.OrderBy(o => Convert.ToDouble(o.Code)).FirstOrDefault().Id;
                                }
                            }
                            else
                            {
                                var terminalId = viewModel.ApprovedTerminalId.HasValue ? viewModel.ApprovedTerminalId.Value : 0;
                                if (order.FuelRequest.ProductDisplayGroupId != (int)ProductDisplayGroups.OtherFuelType)
                                {
                                    if (terminalId == 0)
                                    {
                                        ExternalPricingDomain externalPricingDomain = new ExternalPricingDomain();
                                        var externalPricingData = await externalPricingDomain.GetClosestTerminalPriceAsync(order.FuelRequest.Job.Latitude, order.FuelRequest.Job.Longitude, order.FuelRequest.Job.MstCountry.Code, order.FuelRequest.FuelTypeId, order.FuelRequest.FuelRequestPricingDetail.PricingCodeId);
                                        if (externalPricingData.TerminalId != 0)
                                        {
                                            order.TerminalId = externalPricingData.TerminalId;
                                        }
                                        else if (order.FuelRequest.MstProduct.PricingSourceId != (int)PricingSource.Axxis)
                                        {
                                            var terminals = await externalPricingDomain.GetClosestTerminals(order.FuelRequest.FuelTypeId, order.FuelRequest.Job.Latitude, order.FuelRequest.Job.Longitude, order.FuelRequest.Job.CountryId, string.Empty, order.FuelRequest.FuelRequestPricingDetail.PricingCodeId);
                                            var terminal = terminals.FirstOrDefault(t => t.Id > 0);
                                            if (terminal != null)
                                            {
                                                order.TerminalId = terminal.Id;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        order.TerminalId = viewModel.ApprovedTerminalId;
                                    }
                                }
                            }
                            //update requestPricedetailId
                            var paramterJSON = new SourceRegionJSONParameter()
                            {
                                SourceRegion = string.Join(",", viewModel.SelectedSourceRegions.Select(t => t)),
                                SelectedTerminals = string.Join(",", viewModel.SelectedTerminals.Select(t => t)),
                                SelectedBulkPlants = string.Join(",", viewModel.SelectedBulkPlants.Select(t => t))
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
                            await Context.CommitAsync();
                            transaction.Commit();
                            response.StatusCode = Status.Success;
                            response.StatusMessage = string.Format(Resource.errMessageTerminalAssignmentSuccess, string.Empty);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ThirdPartyDomain", "UpdateOrderSupplierRegionsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> AddTPOBrokerCustomerDetails(ThirdPartyOrderViewModel thirdPartyOrderViewModel, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Success);
            using (var tracer = new Tracer("ThirdPartyDomain", "AddTPOBrokerCustomerDetails"))
            {
                //Insert Broker Company
                //verify is company exist
                var brokerCompany = thirdPartyOrderViewModel.ExternalBrokeredOrder.TPOBrokeredCustomerDetails;

                if (brokerCompany.IsNewCompany)
                {
                    var brokerExist = Context.DataContext.ExternalBrokers.Where(t => (t.SupplierCompanyId == userContext.CompanyId || t.SupplierCompanyId == null)
                                        && (t.CompanyName == brokerCompany.CustomerCompanyName || t.Email == brokerCompany.CustomerEmail))
                                        .Select(t => new { t.CompanyName, t.Email }).FirstOrDefault();
                    if (brokerExist == null)
                    {
                        ExternalBroker Broker = new ExternalBroker
                        {
                            CompanyName = brokerCompany.CustomerCompanyName,
                            Email = brokerCompany.CustomerEmail,
                            Address = brokerCompany.Address,
                            City = brokerCompany.City,
                            CountryId = brokerCompany.CountryId.Value,
                            StateId = brokerCompany.StateId.Value,
                            ZipCode = brokerCompany.ZipCode,
                            SupplierCompanyId = userContext.CompanyId,
                            UpdatedDate = DateTimeOffset.Now,
                            PhoneNumber = brokerCompany.PhoneNumber,
                            IsActive = true
                        };

                        Context.DataContext.ExternalBrokers.Add(Broker);
                        await Context.CommitAsync();
                        thirdPartyOrderViewModel.ExternalBrokeredOrder.CustomerId = Broker.Id;
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        if (brokerCompany.CustomerCompanyName.ToLower() == brokerExist.CompanyName.ToLower() && brokerCompany.CustomerEmail.ToLower() == brokerExist.Email.ToLower())
                        {
                            response.StatusMessage = Resource.errMsgBrokerCustomerAlreadyExists;
                        }
                        else if (brokerCompany.CustomerCompanyName.ToLower() == brokerExist.CompanyName.ToLower())
                        {
                            response.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageUserAlreadyExistsWithAnotherCompanyInSFX, new object[] { brokerExist.CompanyName, brokerExist.Email });
                        }
                        else
                        {
                            response.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageUserAlreadyExistsWithAnotherCompanyInSFX, new object[] { brokerExist.Email, brokerExist.CompanyName });
                        }
                    }
                }
            }
            return response;
        }

        public async Task<Status> SaveInvitationSendDetailsAsync(string email, int buyerUserId, int invitedBy, string firstName, string lastName)
        {
            using (var tracer = new Tracer("ThirdPartyDomain", "SaveInvitationSendDetailsAsync"))
            {
                var response = Status.Failed;
                try
                {
                    var userXInvite = Context.DataContext.UserXInvites.SingleOrDefault(t => t.InvitedToUserId == buyerUserId);
                    if (userXInvite == null)
                    {
                        string[] emails = email.Split(',');
                        if (emails.Length > 0)
                        {
                            foreach (var em in emails)
                            {
                                if (ValidateEmail(em))
                                {
                                    userXInvite = new UserXInvite();
                                    userXInvite.FirstName = firstName;
                                    userXInvite.LastName = lastName;
                                    userXInvite.Email = em;
                                    userXInvite.InvitedToUserId = buyerUserId;
                                    userXInvite.InvitedBy = invitedBy;
                                    userXInvite.CreatedDate = DateTime.Now;
                                    userXInvite.IsInvitationSent = false;
                                    userXInvite.IsOnboarded = false;
                                    userXInvite.UpdatedDate = DateTime.Now;

                                    Context.DataContext.UserXInvites.Add(userXInvite);
                                    await Context.CommitAsync();
                                }
                            }
                        }
                    }
                    else
                    {
                        userXInvite.UpdatedDate = DateTime.Now;
                        Context.DataContext.Entry(userXInvite).State = EntityState.Modified;

                        await Context.CommitAsync();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ThirdPartyDomain", "SaveInvitationSendDetailsAsync", ex.Message, ex);
                }
                return response;
            }
        }

        private bool ValidateEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<Status> SaveSupplierInvitationDetailsAsync(int companyId, int supplierId, int userId)
        {
            using (var tracer = new Tracer("ThirdPartyDomain", "SaveSupplierInvitationDetailsAsync"))
            {
                var response = Status.Failed;
                try
                {
                    var websiteBrandEnable = await Context.DataContext.OnboardingPreferences.Where(top => top.CompanyId == companyId && top.IsActive).OrderByDescending(top => top.Id).FirstOrDefaultAsync();
                    if (websiteBrandEnable != null)
                    {
                        if (websiteBrandEnable.IsBrandMyWebsite)
                        {
                            var userXInvite = await Context.DataContext.SupplierInvitationDetails.SingleOrDefaultAsync(t => t.UserId == userId);
                            if (userXInvite == null)
                            {
                                userXInvite = new SupplierInvitationDetails();
                                userXInvite.CompanyId = companyId;
                                userXInvite.SupplierId = supplierId;
                                userXInvite.UserId = userId;
                                userXInvite.CreatedDate = DateTime.Now;
                                Context.DataContext.SupplierInvitationDetails.Add(userXInvite);
                                await Context.CommitAsync();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ThirdPartyDomain", "SaveSupplierInvitationDetailsAsync", ex.Message, ex);
                }
                return response;
            }
        }
        public async Task<StatusViewModel> CreateAssetsAsync(UserContext userContext, AssetViewModel viewModel, int jobId)
        {
            using (var tracer = new Tracer("ThirdPartyDomain", "CreateAssetsAsync"))
            {
                StatusViewModel response = new StatusViewModel();

                var jobDetails = Context.DataContext.Jobs.SingleOrDefault(t => t.Id == jobId);
                if (jobDetails != null)
                {
                    bool isValidAssetName = new HelperDomain(this).IsValidAssetName(viewModel.Id, viewModel.Name, jobDetails.CompanyId, viewModel.Type, jobId);
                    if (!isValidAssetName)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = string.Format(Resource.errMessageAssetOrTankNameAlreadyExists, ((AssetType)viewModel.Type).GetDisplayName());
                        return response;
                    }

                    // validate pedigree asset DB ID already exists for tank - Pedigree Dip
                    if (!string.IsNullOrWhiteSpace(viewModel.AssetAdditionalDetail.PedigreeAssetDBID))
                    {
                        var existingPedigreeAssetDBIdTank = Context.DataContext.Assets.Where(t => t.CompanyId == jobDetails.CompanyId && t.IsActive && (t.AssetAdditionalDetail.PedigreeAssetDBId != null && t.AssetAdditionalDetail.PedigreeAssetDBId != "" && t.AssetAdditionalDetail.PedigreeAssetDBId.ToLower() == viewModel.AssetAdditionalDetail.PedigreeAssetDBID.ToLower())).FirstOrDefault();
                        if (existingPedigreeAssetDBIdTank != null)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessagePedigreeAssetDBIDAlreadyExists, existingPedigreeAssetDBIdTank.Name);
                            return response;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(viewModel.AssetAdditionalDetail.SkyBitzRTUID))
                    {
                        var existingSkyBitzRTUID = Context.DataContext.Assets.Where(t => t.CompanyId == jobDetails.CompanyId && t.IsActive && (t.AssetAdditionalDetail.SkyBitzRTUID != null && t.AssetAdditionalDetail.SkyBitzRTUID != "" && t.AssetAdditionalDetail.SkyBitzRTUID.ToLower() == viewModel.AssetAdditionalDetail.SkyBitzRTUID.ToLower())).FirstOrDefault();
                        if (existingSkyBitzRTUID != null)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageSkyBitzRTUIDAlreadyExists, existingSkyBitzRTUID.Name);
                            return response;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(viewModel.AssetAdditionalDetail.Insight360TankId))
                    {
                        var existingInsightTankId = Context.DataContext.Assets.Where(t => t.CompanyId == jobDetails.CompanyId && t.IsActive && (t.AssetAdditionalDetail.DipTestMethod != null && t.AssetAdditionalDetail.DipTestMethod.Value == (int)DipTestMethod.Insight360) && (t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" && t.AssetAdditionalDetail.ExternalTankId.ToLower() == viewModel.AssetAdditionalDetail.Insight360TankId.ToLower())).FirstOrDefault();

                        if (existingInsightTankId != null)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInsight360TankIDAlreadyExists, existingInsightTankId.Name);
                            return response;
                        }
                    }

                    if (viewModel.AssetAdditionalDetail.DipTestMethod == DipTestMethod.VeederRoot)
                    {
                        var existingVeederRootTank = Context.DataContext.Assets
                                                   .Where(t => t.CompanyId == jobDetails.CompanyId && t.IsActive && (t.AssetAdditionalDetail.DipTestMethod != null &&
                                                               t.AssetAdditionalDetail.DipTestMethod.Value == (int)DipTestMethod.VeederRoot) &&
                                                               (
                                                                t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" &&
                                                                t.AssetAdditionalDetail.ExternalTankId.ToLower() == viewModel.AssetAdditionalDetail.VeederRootTankID.ToLower() &&
                                                                t.AssetAdditionalDetail.VeederRootIPAddress == viewModel.AssetAdditionalDetail.VeederRootIPAddress &&
                                                                t.AssetAdditionalDetail.Port == viewModel.AssetAdditionalDetail.Port
                                                               )
                                                        )
                                                   .Select(t => new { t.Name, t.AssetAdditionalDetail.ExternalTankId, t.AssetAdditionalDetail.VeederRootIPAddress, t.AssetAdditionalDetail.Port })
                                                   .FirstOrDefault();

                        if (existingVeederRootTank != null)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageVeederRootTankIDAlreadyExists, existingVeederRootTank.Name, existingVeederRootTank.ExternalTankId, existingVeederRootTank.VeederRootIPAddress, existingVeederRootTank.Port);
                            return response;
                        }
                    }

                    viewModel.UserId = jobDetails.CreatedBy;
                    var asset = viewModel.ToEntity();
                    bool freightResponse = true;
                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            jobDetails.Company.Assets.Add(asset);
                            await Context.CommitAsync();

                            Context.DataContext.JobXAssets.Add(new JobXAsset() { AssetId = asset.Id, JobId = jobId, AssignedBy = jobDetails.CreatedBy, AssignedDate = DateTime.Now });

                            if (!jobDetails.JobBudget.IsTankAvailable && viewModel.Type == (int)AssetType.Tank)
                            {
                                jobDetails.JobBudget.IsTankAvailable = true;
                            }
                            await Context.CommitAsync();

                            if (viewModel.Type == (int)AssetType.Tank)
                            {
                                viewModel.AssetAdditionalDetail.AssetId = asset.Id;
                                viewModel.JobDisplayId = jobDetails.DisplayJobID;
                                viewModel.JobName = jobDetails.Name;
                                freightResponse = await new FreightServiceDomain().SaveTankDetails(viewModel);
                            }

                            if (freightResponse)
                            {
                                transaction.Commit();

                                response.EntityId = asset.Id;
                                response.CustomerCompanyId = asset.CompanyId != null ? asset.CompanyId.Value : 0;
                                if (jobDetails.User != null)
                                {
                                    response.CustomerId = jobDetails.User.Id;
                                }
                                response.StatusCode = Status.Success;
                                response.StatusMessage = string.Format(Resource.errMessageAssetCreatedSuccess, asset.Name, ((AssetType)viewModel.Type).GetDisplayName());
                            }
                            else
                            {
                                transaction.Rollback();
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageAssetCreatedFailed, ((AssetType)viewModel.Type).GetDisplayName());
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogManager.Logger.WriteException("ThirdPartyDomain", "CreateAssetsAsync", ex.Message, ex);
                        }
                    }
                }
                return response;
            }
        }

        public async Task<StatusViewModel> UpdateAssetAsync(UserContext userContext, AssetViewModel viewModel)
        {
            using (var tracer = new Tracer("ThirdPartyDomain", "UpdateAssetAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                bool freightResponse = true;
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var asset = Context.DataContext.Assets.FirstOrDefault(t => t.Id == viewModel.Id);
                        if (asset != null)
                        {
                            // validate pedigree asset DB ID already exists for tank - Pedigree Dip
                            if (!string.IsNullOrWhiteSpace(viewModel.AssetAdditionalDetail.PedigreeAssetDBID))
                            {
                                var existingPedigreeAssetDBIdTank = Context.DataContext.Assets.Where(t => t.Id != asset.Id && t.IsActive && t.CompanyId == asset.CompanyId && (t.AssetAdditionalDetail.PedigreeAssetDBId != null && t.AssetAdditionalDetail.PedigreeAssetDBId != "" && t.AssetAdditionalDetail.PedigreeAssetDBId.ToLower() == viewModel.AssetAdditionalDetail.PedigreeAssetDBID.ToLower())).FirstOrDefault();
                                if (existingPedigreeAssetDBIdTank != null)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessagePedigreeAssetDBIDAlreadyExists, existingPedigreeAssetDBIdTank.Name);
                                    return response;
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(viewModel.AssetAdditionalDetail.SkyBitzRTUID))
                            {
                                var existingSkyBitzRTUID = Context.DataContext.Assets.Where(t => t.Id != asset.Id && t.IsActive && t.CompanyId == asset.CompanyId && (t.AssetAdditionalDetail.SkyBitzRTUID != null && t.AssetAdditionalDetail.SkyBitzRTUID != "" && t.AssetAdditionalDetail.SkyBitzRTUID.ToLower() == viewModel.AssetAdditionalDetail.SkyBitzRTUID.ToLower())).FirstOrDefault();
                                if (existingSkyBitzRTUID != null)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessageSkyBitzRTUIDAlreadyExists, existingSkyBitzRTUID.Name);
                                    return response;
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(viewModel.AssetAdditionalDetail.Insight360TankId))
                            {

                                var existingInsightTankId = Context.DataContext.Assets.Where(t => t.Id != asset.Id && t.IsActive && t.CompanyId == asset.CompanyId && (t.AssetAdditionalDetail.DipTestMethod != null && t.AssetAdditionalDetail.DipTestMethod.Value == (int)DipTestMethod.Insight360) && (t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" && t.AssetAdditionalDetail.ExternalTankId.ToLower() == viewModel.AssetAdditionalDetail.Insight360TankId.ToLower())).FirstOrDefault();
                                if (existingInsightTankId != null)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessageInsight360TankIDAlreadyExists, existingInsightTankId.Name);
                                    return response;
                                }
                            }
                            if (viewModel.AssetAdditionalDetail.DipTestMethod == DipTestMethod.VeederRoot)
                            {
                                var existingVeederRootTank = Context.DataContext.Assets
                                                           .Where(t => t.Id != asset.Id && t.IsActive && t.CompanyId == asset.CompanyId && (t.AssetAdditionalDetail.DipTestMethod != null &&
                                                                       t.AssetAdditionalDetail.DipTestMethod.Value == (int)DipTestMethod.VeederRoot) &&
                                                                       (
                                                                        t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" &&
                                                                        t.AssetAdditionalDetail.ExternalTankId.ToLower() == viewModel.AssetAdditionalDetail.VeederRootTankID.ToLower() &&
                                                                        t.AssetAdditionalDetail.VeederRootIPAddress == viewModel.AssetAdditionalDetail.VeederRootIPAddress &&
                                                                        t.AssetAdditionalDetail.Port == viewModel.AssetAdditionalDetail.Port
                                                                       )
                                                                )
                                                           .Select(t => new { t.Name, t.AssetAdditionalDetail.ExternalTankId, t.AssetAdditionalDetail.VeederRootIPAddress, t.AssetAdditionalDetail.Port })
                                                           .FirstOrDefault();

                                if (existingVeederRootTank != null)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessageVeederRootTankIDAlreadyExists, existingVeederRootTank.Name, existingVeederRootTank.ExternalTankId, existingVeederRootTank.VeederRootIPAddress, existingVeederRootTank.Port);
                                    return response;
                                }
                            }

                            asset = viewModel.ToEntity(asset);
                            if (viewModel.Image != null && viewModel.Image.IsRemoved)
                            {
                                var image = await Context.DataContext.Images.FirstOrDefaultAsync(t => t.Id == viewModel.Image.Id);
                                if (image != null)
                                {
                                    asset.Image = null;
                                    Context.DataContext.Images.Remove(image);
                                }
                            }

                            Context.DataContext.Entry(asset).State = EntityState.Modified;
                            await Context.CommitAsync();

                            if (asset.Type == (int)AssetType.Tank)
                            {
                                var jobAsset = asset.JobXAssets.FirstOrDefault(t => t.RemovedBy == null);
                                if (jobAsset != null)
                                {
                                    viewModel.JobDisplayId = jobAsset.Job.DisplayJobID;
                                }

                                freightResponse = await new FreightServiceDomain().UpdateTankDetails(viewModel);
                            }

                            if (freightResponse)
                            {
                                transaction.Commit();

                                if (asset.JobXAssets.Any(t => t.RemovedBy == null))
                                {
                                    await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetAssetModifiedNewsfeed(userContext, asset.JobXAssets.First(t => t.RemovedBy == null).Job, 1);
                                }
                                response.EntityId = asset.Id;
                                response.CustomerCompanyId = asset.CompanyId != null ? asset.CompanyId.Value : 0;
                                if (asset.JobXAssets != null)
                                {
                                    if (asset.JobXAssets.FirstOrDefault().Job != null && asset.JobXAssets.FirstOrDefault().Job.User != null)
                                    {
                                        response.CustomerId = asset.JobXAssets.FirstOrDefault().Job.User.Id;
                                    }
                                }
                                response.StatusCode = Status.Success;
                                response.StatusMessage = string.Format(Resource.errMessageAssetUpdateSuccess, asset.Name, ((AssetType)viewModel.Type).GetDisplayName());
                            }
                            else
                            {
                                transaction.Rollback();
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageAssetUpdatedFailed, ((AssetType)viewModel.Type).GetDisplayName());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("ThirdPartyDomain", "UpdateAssetAsync", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StatusViewModel> ValidateOrderBulkFile(UserContext userContext, string csvText, string csvFilePath)
        {
            using (var tracer = new Tracer("ThirdPartyDomain", "ValidateOrderBulkCsvHeader"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    var csvHeaderLine = Regex.Matches(csvText.Trim(), @"^.*Company Name.*\n").Cast<Match>().FirstOrDefault();
                    string[] lines = File.ReadAllLines(csvFilePath);
                    //header validations
                    string headerLine = lines.FirstOrDefault();
                    if (csvHeaderLine.Value.Trim() != headerLine)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageBulkUploadHeaderMismatch;
                        return response;
                    }

                    csvText = RemoveHeaderAndGuidelinesFromFile(csvText);

                    var engine = new FileHelperEngine<ThirdPartyOrderCsvViewModel>();
                    var csvOrderList = engine.ReadString(csvText).ToList();

                    var allStates = Context.DataContext.MstStates.Where(t => t.IsActive).Select(t => t.Code.ToLower().Trim()).ToList();
                    var allCountries = Context.DataContext.MstCountries.Where(t => t.IsActive).Select(t => new { t.Code, t.Currency, t.DefaultUoM }).ToList();
                    var allFuelTypes = Context.DataContext.MstProducts.Where(t => t.IsActive).Select(t => t.Name.ToLower().Trim()).ToList();
                    var allCompaines = Context.DataContext.Companies.Where(t => t.IsActive).Select(t => t.Name.ToLower().Trim()).ToList();
                    var allEmails = Context.DataContext.Users.Where(t => t.IsActive && t.IsEmailConfirmed).Select(t => t.Email.ToLower().Trim()).ToList();
                    var zipcodeRegEx = ApplicationConstants.ZipValidationRegex;
                    int lineNumberOfCSV = 1;

                    foreach (var record in csvOrderList)
                    {
                        if (CheckIfItsEmptyLine(record))
                        {
                            break;
                        }

                        lineNumberOfCSV++;

                        //Required field validation
                        if (IsRequiredFieldMissing(record))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageBulkUploadRequiredFieldsAreMissing, lineNumberOfCSV);
                            return response;
                        }

                        //validate registered/active companies of sfx
                        if (allCompaines.Contains(record.CompanyName.ToLower().Trim()))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageAlreadyActiveOnSFX, record.CompanyName);
                            return response;
                        }

                        //validate registered/active email ids
                        if (allEmails.Contains(record.ContactPersonEmail.ToLower().Trim()))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageAlreadyActiveOnSFX, record.ContactPersonEmail);
                            return response;
                        }

                        //validate State
                        if (!allStates.Contains(record.State.ToLower().Trim()))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInvalidStateCode, record.State);
                            return response;
                        }

                        //validate State
                        if (!allCountries.Select(t => t.Code.ToLower()).Contains(record.CountryCode.ToLower().Trim()))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInvalidCountryCode, record.CountryCode);
                            return response;
                        }

                        //validate Zip
                        if (!Regex.Match(record.Zip.ToLower().Trim(), zipcodeRegEx).Success)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInvalidSpecificZipCode, record.Zip);
                            return response;
                        }

                        //validate fuel type
                        if (!string.IsNullOrWhiteSpace(record.StandardFuelType) && !allFuelTypes.Contains(record.StandardFuelType.ToLower().Trim()))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInvalidFuelType, record.StandardFuelType);
                            return response;
                        }

                        if (!string.IsNullOrWhiteSpace(record.MinQuantity) && !Regex.IsMatch(record.MinQuantity, @"^[0-9]*$"))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInvalidQuantity, record.MinQuantity);
                            return response;
                        }

                        //validate total quantity
                        if (!double.TryParse(record.MaxQuantity, out double d))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInvalidQuantity, record.MaxQuantity);
                            return response;
                        }

                        //validate PricePerGallon
                        if (record.PricePerGallon.Trim().ToLower().Contains("fuelcost"))
                        {
                            var fuelTypeId = Context.DataContext.MstProducts.SingleOrDefault(t => t.Name.ToLower().Equals(record.StandardFuelType.ToLower()));
                            if (fuelTypeId != null)
                            {
                                var globalCost = Context.DataContext.CurrentCosts.Any(t => t.CreatedBy == userContext.Id && t.IsActive
                                                && t.IsGlobleCost && t.FuelTypeId == fuelTypeId.Id);
                                if (!globalCost)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = Resource.ErrorGlobalCostNotProvided;
                                    return response;
                                }
                            }
                        }
                        //validate city rack terminals
                        if (record.PricePerGallon.Trim().ToLower().Contains("rack") && !string.IsNullOrWhiteSpace(record.StandardFuelType) && !string.IsNullOrWhiteSpace(record.CityRackTerminal))
                        {
                            var rackWithState = record.CityRackTerminal.Split(' ');
                            var cityRack = new HelperDomain(this).GetExternalTerminal(record.CityRackTerminal.Replace(rackWithState.Last(), string.Empty), rackWithState.Last());
                            if (cityRack == null)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.ErrorCityRackTerminalNotFound, record.CityRackTerminal);
                                return response;
                            }
                            else
                            {
                                //check price available or not for this city rack
                                var fuelTypeId = Context.DataContext.MstProducts.SingleOrDefault(t => t.Name.ToLower().Equals(record.StandardFuelType.ToLower()));

                                var productMapping = Context.DataContext.MstProductMappings.FirstOrDefault(t => t.ProductId == fuelTypeId.Id);
                                if (productMapping != null)
                                {
                                    PricingServiceDomain pricingServiceDomain = new PricingServiceDomain(this);
                                    var pricingDataAvailable = await pricingServiceDomain.IsCityRackPriceAvailable(productMapping.ExternalProductId, cityRack.Id, PricingSource.Axxis, DateTime.Now);
                                    if (!pricingDataAvailable)
                                    {
                                        response.StatusCode = Status.Failed;
                                        response.StatusMessage = string.Format(Resource.ErrorCityRackPriceNotAvailable, record.CityRackTerminal);
                                        return response;
                                    }
                                }
                            }
                        }
                        //validate delivery data
                        if (!IsValidDeliveryDetails(record))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInvalidDeliveryDateOrTime, record.DeliveryDate + Resource.lblSingleHyphen + record.DeliveryStartTime + Resource.lblSingleHyphen + record.DeliveryEndTime);
                            return response;
                        }

                        if (!(string.Equals("multiple", record.DeliveryType.ToLower().Trim()) || string.Equals("single", record.DeliveryType.ToLower().Trim())))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInvalidDeliveryType, record.DeliveryType);
                            return response;
                        }

                        if (record.DeliveryType != null && string.Equals("yes", record.IsBuyAndSellOrder.ToLower()) && (string.Equals("single", record.DeliveryType.ToLower().Trim())))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInvalidDeliveryTypeForBrokeredOrder, record.DeliveryType);
                            return response;
                        }

                        if ((!string.IsNullOrWhiteSpace(record.IsBuyAndSellOrder)) && string.Equals("yes", record.IsBuyAndSellOrder.ToLower().Trim()))
                        {
                            if (!IsValidExternalBrokeredAdditionalFee(record))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageInValidAtLine, record.BrokeredOrderAdditionalFees, lineNumberOfCSV);
                                return response;
                            }

                            if (!IsValidExternalCustomer(record))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageInValidAtLine, record.BrokeredCustomer, lineNumberOfCSV);
                                return response;
                            }
                        }

                        if ((!string.IsNullOrWhiteSpace(record.IsThirdPartyHardwareUsed)) && string.Equals("yes", record.IsThirdPartyHardwareUsed.ToLower().Trim()))
                        {
                            //// check if tpo is of external brokered type
                            if (!IsValidFreightFee(record))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageInValidAtLine, record.FreightFee, lineNumberOfCSV);
                                return response;
                            }

                            if (!IsValidInvoicePreference(record))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageInValidAtLine, record.InvoicePreference, lineNumberOfCSV);
                                return response;
                            }

                            if (string.IsNullOrWhiteSpace(record.VendorId))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageNameIsRequired, record.VendorId);
                                return response;
                            }

                            if (string.IsNullOrWhiteSpace(record.CustomerNumber))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageNameIsRequired, record.CustomerNumber);
                                return response;
                            }

                            if (string.IsNullOrWhiteSpace(record.ShipTo))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageNameIsRequired, record.ShipTo);
                                return response;
                            }

                            if (string.IsNullOrWhiteSpace(record.Source))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageNameIsRequired, record.Source);
                                return response;
                            }

                            if (string.IsNullOrWhiteSpace(record.ProductCode))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageNameIsRequired, record.ProductCode);
                                return response;
                            }
                        }
                        else
                        {
                            //// check if tpo is not of external brokered type
                            if (!IsValidCommonFees(record))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageInValidAtLine, record.CommonFees, lineNumberOfCSV);
                                return response;
                            }

                            if (!IsValidOtherFee(record))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageInValidAtLine, record.OtherFees, lineNumberOfCSV);
                                return response;
                            }
                        }

                        if (!IsValidOrderTax(record))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInValidAtLine, record.OrderTaxes, lineNumberOfCSV);
                            return response;
                        }

                        //validate onsite contact
                        if (!string.IsNullOrWhiteSpace(record.OnsiteContactEmail.Trim()) && (string.IsNullOrWhiteSpace(record.OnsiteContactName.Trim()) || string.IsNullOrWhiteSpace(record.OnsiteContactPhone.Trim())))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageNameIsRequired, record.OnsiteContactEmail);
                            return response;
                        }

                        //validate onsite contact
                        if (string.IsNullOrWhiteSpace(record.OnsiteContactEmail.Trim()) && (!string.IsNullOrWhiteSpace(record.OnsiteContactName.Trim()) || !string.IsNullOrWhiteSpace(record.OnsiteContactPhone.Trim())))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageEmailIsRequired, record.OnsiteContactName);
                            return response;
                        }

                        //validate onsite contact email ids
                        if (!string.IsNullOrWhiteSpace(record.OnsiteContactEmail.Trim()) && allEmails.Contains(record.OnsiteContactEmail.ToLower().Trim()))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageAlreadyActiveOnSFX, record.OnsiteContactEmail);
                            return response;
                        }

                        //validate schedule data
                        if (IsDeliveryScheduleDataMissing(record))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInvalidDeliveryScheduleData, lineNumberOfCSV);
                            return response;
                        }

                        //validate schedule quantity
                        if (!string.IsNullOrWhiteSpace(record.DeliverySchedulesQuantity) && !double.TryParse(record.DeliverySchedulesQuantity, out double deliveryQuantity))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInvalidQuantity, record.DeliverySchedulesQuantity);
                            return response;
                        }

                        //validate asset data
                        if (IsAssetRequiredDataMissing(record))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageAssetNameIsRequired, lineNumberOfCSV);
                            return response;
                        }

                        //validate driver details
                        if ((string.IsNullOrWhiteSpace(record.DriverFirstName) || string.IsNullOrWhiteSpace(record.DriverLastName) || string.IsNullOrWhiteSpace(record.DriverEmail)) && !(string.IsNullOrWhiteSpace(record.DriverFirstName) && string.IsNullOrWhiteSpace(record.DriverLastName) && string.IsNullOrWhiteSpace(record.DriverEmail)))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInsufficientDriverData, lineNumberOfCSV);
                            return response;
                        }
                        else if (IsInValidDriverDetails(record))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInvalidDriverDetails, lineNumberOfCSV);
                            return response;
                        }
                    }

                    if (lineNumberOfCSV - 1 > 0)
                    {
                        response.StatusCode = Status.Success;
                        response.StatusMessage = string.Format(Resource.successMessageForBulkUpload, (lineNumberOfCSV - 1));
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageCanNotProcessZeroRecords;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ThirdPartyOrderDomain", "ValidateOrderBulkCsvHeader", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<JobStepsViewModel> GetJobDetails(string jobName, string companyName, UserContext userContext)
        {
            JobStepsViewModel response = new JobStepsViewModel();
            try
            {
                var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.IsActive && t.Name == jobName && t.Company.Name == companyName);
                if (job != null)
                {
                    response.Job = job.ToViewModel();
                    response.Job.IsCompanyOwned = Context.DataContext.SupplierXBuyerDetails.Any(t => t.JobId == job.Id && t.BuyerCompanyId == job.CompanyId && t.SupplierCompanyId == userContext.CompanyId && t.CompanyOwnedLocation && t.IsActive);
                    var freightServiceDomain = new FreightServiceDomain(this);
                    var regionId = await freightServiceDomain.GetRegionIdForJob(job.Id, userContext.CompanyId);
                    if ((!string.IsNullOrWhiteSpace(regionId)))
                    {
                        response.RegionId = regionId;
                    }
                    var routeId = await freightServiceDomain.GetRouteIdForJob(job.Id, userContext.CompanyId, regionId);
                    if ((!string.IsNullOrWhiteSpace(routeId)))
                    {
                        response.RouteId = routeId;
                    }
                    response.JobBudget.IsTaxExempted = job.JobBudget.IsTaxExempted;
                    response.Job.AssignedCarrierCompId = await freightServiceDomain.GetAssignedCarrierCompanyId(userContext.CompanyId, job.Id);
                    response.Job.AccountingCompanyId = Context.DataContext.SupplierXBuyerDetails.Where(t => t.JobId == job.Id && t.SupplierCompanyId == userContext.CompanyId && t.BuyerCompanyId == job.CompanyId).Select(t => t.AccountingCompanyId).FirstOrDefault();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                    var jobAdditionalDetail = await freightServiceDomain.GetAdditionalJobDetails(job.Id, 0);
                    if (jobAdditionalDetail != null)
                    {
                        response.Job.DistanceCovered = string.IsNullOrEmpty(jobAdditionalDetail.DistanceCovered) ? "00:00" : jobAdditionalDetail.DistanceCovered;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetJobDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<JobStepsViewModel> GetPortDetails(int portId, string portName, UserContext userContext)
        {
            JobStepsViewModel response = new JobStepsViewModel();
            try
            {
                var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.Id == portId && t.IsActive);
                if (job != null)
                {
                    var sJob = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.IsMarine && t.IsActive && t.CreatedByCompanyId == userContext.CompanyId && t.Name == job.Name);
                    if (sJob != null)
                    {
                        response.Job = sJob.ToViewModel();
                        response.Job.IsCompanyOwned = Context.DataContext.SupplierXBuyerDetails.Any(t => t.JobId == sJob.Id && t.BuyerCompanyId == sJob.CompanyId && t.SupplierCompanyId == userContext.CompanyId && t.CompanyOwnedLocation && t.IsActive);
                        response.JobBudget.IsTaxExempted = sJob.JobBudget.IsTaxExempted;
                        response.RegionId = await new FreightServiceDomain().GetRegionByJobAndCompanyId(sJob.Id, userContext.CompanyId);
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageSuccess;
                    }
                    else
                    {
                        response.Job = job.ToViewModel();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetPortDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> GetVesselDetails(int vesselId, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var vesselDetail = await Context.DataContext.Assets.Where(t => t.Id == vesselId && t.Type == (int)AssetType.Vessle)
                                                             .Select(t => new VesselModel()
                                                             {
                                                                 AssetId = t.Id,
                                                                 AssetName = t.Name,
                                                                 IMONumber = t.AssetAdditionalDetail != null ? t.AssetAdditionalDetail.IMONumber : null,
                                                                 Flag = t.AssetAdditionalDetail != null ? t.AssetAdditionalDetail.Flag : null,
                                                             }).FirstOrDefaultAsync();

                if (vesselDetail != null)
                {
                    response.ResponseData = vesselDetail;
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Status.Success.ToString();
                }
                else
                {
                    response.ResponseData = null;
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Status.Failed.ToString();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetVesselDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> ValidateJobNameByCompanyId(string jobName, int companyId)
        {
            bool isJobExist = true;
            try
            {
                isJobExist = await Context.DataContext.Jobs.AnyAsync(t => t.IsActive && t.Name.Trim().ToLower() == jobName.Trim().ToLower() && t.Company.Id == companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "ValidateJobNameByCompanyId", ex.Message, ex);
            }
            return isJobExist;
        }

        public List<DropdownDisplayItem> GetTPOCompanyContactPersons(int companyId)
        {
            List<DropdownDisplayItem> dropdownDisplayItems = new List<DropdownDisplayItem>();
            try
            {
                dropdownDisplayItems = (from u in Context.DataContext.Users.Where(t => t.CompanyId == companyId)
                                        select new DropdownDisplayItem
                                        {
                                            Id = u.Id,
                                            Name = u.FirstName + " " + u.LastName
                                        }).OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetTPOCompanyContactPersonsAsync", ex.Message, ex);
            }
            return dropdownDisplayItems;
        }

        public ContactPersonViewModel GetTPOContactPersonDetails(int userId)
        {
            ContactPersonViewModel contactPersonViewModel = new ContactPersonViewModel();
            try
            {
                var user = Context.DataContext.Users.Where(t => t.Id == userId).FirstOrDefault();
                if (user != null)
                {
                    contactPersonViewModel.PhoneNumber = user.PhoneNumber;
                    contactPersonViewModel.IsPhoneNumberConfirmed = user.IsPhoneNumberConfirmed;
                    contactPersonViewModel.Email = user.Email;
                    contactPersonViewModel.Name = $"{user.FirstName} {user.LastName}";
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetTPOCompanyContactPersonsAsync", ex.Message, ex);
            }
            return contactPersonViewModel;
        }

        public JobSpecificBillToViewModel GetJobSpecificBillingAddressDetails(int jobId, int customerId)
        {
            var response = new JobSpecificBillToViewModel();
            try
            {
                Job job = null;
                if (customerId > 0)
                {
                    var jobName = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(s => s.Name).FirstOrDefault();
                    job = Context.DataContext.Jobs.Where(t => t.CompanyId == customerId && t.Name == jobName).FirstOrDefault();
                }
                else
                {
                    job = Context.DataContext.Jobs.Where(t => t.Id == jobId).FirstOrDefault();
                }

                BillingAddress billingAddress = new BillingAddress();
                if (job != null && job.BillingAddressId != null)
                {
                    billingAddress = Context.DataContext.BillingAddresses.Where(t => t.Id == job.BillingAddressId).FirstOrDefault();
                }
                else
                {
                    billingAddress = Context.DataContext.BillingAddresses.Where(t => t.CompanyId == customerId && t.IsActive && t.IsDefault).FirstOrDefault();
                }

                if (billingAddress != null)
                {
                    response.Id = billingAddress.Id;
                    response.BillingAddressId = billingAddress.Id;
                    response.Name = billingAddress.Name;
                    response.Address = billingAddress.Address;
                    response.AddressLine2 = billingAddress.AddressLine2;
                    response.AddressLine3 = billingAddress.AddressLine3;
                    response.City = billingAddress.City;
                    response.State.Name = billingAddress.StateName;
                    response.ZipCode = billingAddress.ZipCode;
                    response.County = billingAddress.County;
                    response.Country.Name = billingAddress.CountryName;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetJobSpecificBillingAddressDetails", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetJobList(string buyerCompanyName, bool isFtl, bool isFobAsTerminal, int supplierUserId, int supplierCompanyId, bool isPort = false, int countryId = (int)Country.USA)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (isPort)
                {
                    var result = Context.DataContext.Jobs.Where(t => t.IsActive && t.LocationType == JobLocationTypes.Port && (t.CreatedByCompanyId == ApplicationConstants.SuperAdminCompanyId || t.CompanyId == supplierCompanyId) &&
                                                                    t.IsMarine && t.CountryId == countryId && t.Address != Resource.lblVarious
                                                       ).Select(t => new DropdownDisplayExtendedItem
                                                       {
                                                           Id = t.Id,
                                                           Name = t.Name,
                                                           Code = t.CompanyId.ToString()
                                                       }).OrderByDescending(t => t.Id).ToList();
                    List<DropdownDisplayItem> jobList = result.Where(w => w.Code != ApplicationConstants.SuperAdminCompanyId.ToString()).Select(s => new DropdownDisplayItem() { Id = s.Id, Name = s.Name }).ToList();
                    List<DropdownDisplayItem> superAdminJobList = result.Where(w => w.Code == ApplicationConstants.SuperAdminCompanyId.ToString()).Select(s => new DropdownDisplayItem() { Id = s.Id, Name = s.Name }).ToList();
                    jobList.ForEach(t => superAdminJobList.RemoveAll(w => w.Name.ToLower() == t.Name.ToLower()));
                    jobList.AddRange(superAdminJobList);
                    response.AddRange(jobList);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(buyerCompanyName))
                    {
                        response = Context.DataContext.Jobs.Where(t => t.IsActive && t.Company.Name.Trim() == buyerCompanyName.Trim()
                                    && ((t.User.OnboardedTypeId != (int)OnboardedType.ThirdPartyOrderOnboarded)
                                        || (t.FuelRequests.Any(t1 => t1.Orders.Any(t2 => t2.AcceptedCompanyId == supplierCompanyId))
                                        || t.CreatedByCompanyId == supplierCompanyId)
                                        )
                                    && ((isFtl && isFobAsTerminal) || t.Address != Resource.lblVarious)).
                            Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).OrderByDescending(t => t.Id).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetJobList", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetTPOCompanyList(int userId, int companyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = (from supplierUser in Context.DataContext.Users
                            join buyerUser in Context.DataContext.Users on supplierUser.Id equals buyerUser.CreatedBy
                            join company in Context.DataContext.Companies on buyerUser.CompanyId equals company.Id
                            where company.IsDeleted == false && supplierUser.CompanyId == companyId && buyerUser.OnboardedTypeId == (int)OnboardedType.ThirdPartyOrderOnboarded
                            select new DropdownDisplayItem()
                            {
                                Id = company.Id,
                                Name = company.Name
                            }
                            ).Distinct().OrderByDescending(t => t.Id).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetTPOCompanyList", ex.Message, ex);
            }
            return response;
        }

        private bool IsValidDeliveryDetails(ThirdPartyOrderCsvViewModel record)
        {
            return DateTime.TryParse(record.DeliveryDate, out DateTime dateTime)
                    && (!string.IsNullOrWhiteSpace(record.DeliveryStartTime) && !string.IsNullOrWhiteSpace(record.DeliveryEndTime));
        }

        private bool CheckIfItsEmptyLine(ThirdPartyOrderCsvViewModel record) => string.IsNullOrEmpty(record.CompanyName) && string.IsNullOrEmpty(record.ContactPersonEmail)
                        && string.IsNullOrEmpty(record.ContactPersonName) && string.IsNullOrEmpty(record.ConactPersonPhone) && string.IsNullOrEmpty(record.StandardFuelType)
                        && string.IsNullOrEmpty(record.MaxQuantity) && string.IsNullOrEmpty(record.State) && string.IsNullOrEmpty(record.Zip) && string.IsNullOrEmpty(record.DeliveryDate)
            && string.IsNullOrEmpty(record.DeliveryStartTime) && string.IsNullOrEmpty(record.DeliverySchedulesEndTime) && string.IsNullOrEmpty(record.ExternalPONumber);

        public string RemoveHeaderAndGuidelinesFromFile(string csvText)
        {
            csvText = Regex.Replace(csvText.Trim(), @"^.*Company Name.*\n", string.Empty, RegexOptions.IgnoreCase);
            csvText = Regex.Replace(csvText.Trim(), @",,,,,,,,,,,,,,,,,,,,,,,,,,\n", string.Empty, RegexOptions.IgnoreCase);
            var indexOfGuideline = csvText.IndexOf("SFX Guidelines");
            if (indexOfGuideline > 0)
            {
                return csvText.Substring(0, indexOfGuideline);
            }

            return csvText;
        }

        public async Task<StatusViewModel> UploadFileToBlob(UserContext userContext, Stream fileStream, string fileName)
        {
            using (var tracer = new Tracer("ThirdPartyDomain", "UploadFileToBlob"))
            {
                var response = new StatusViewModel(Status.Failed);
                try
                {
                    var azureBlob = new AzureBlobStorage();

                    var filePath = await azureBlob.SaveBlobAsync(fileStream, GenerateFileName(userContext.Id), BlobContainerType.Orderbulkupload.ToString().ToLower());
                    if (!string.IsNullOrWhiteSpace(filePath))
                    {
                        var queueDomain = new QueueMessageDomain();
                        var queueRequest = GetEnqueueMessageRequestViewModel(userContext, filePath);
                        var queueId = queueDomain.EnqeueMessage(queueRequest);

                        response.StatusCode = Status.Success;
                        response.StatusMessage = string.Format(Resource.successMessageOrderBulkWithRequestNo, string.Concat(Constants.SFXOrderBulkUploadSuffix, queueId.ToString("000")));
                    }
                    else
                    {
                        response.StatusMessage = Resource.errMessageErrorInAzureServer;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageErrorInAzureServer;
                    LogManager.Logger.WriteException("ThirdPartyOrderDomain", "UploadFileToStorage", ex.Message, ex);
                }
                return response;
            }
        }
        
        public bool ProcessOrderBulkUploadJsonMessage(int messageId, ThirdPartyBulkUploadQueueMsg bulkUploadMsg, List<string> errorInfo)
        {
            using (var tracer = new Tracer("ThirdPartyDomain", "ProcessOrderBulkUploadJsonMessage"))
            {
                StringBuilder processMessage = new StringBuilder();
                List<BulkOrderDetails> unprocessedOrders;
                bool isFileReadAndInsertTo;
                bool errToReadAtleaseOneRecord = true;
                try
                {
                    //Step 1: Read stream from Blob and insert records in BulkOrderDetails table
                    //and avoid re-entry if in previous file process has inserted few records or all records
                    isFileReadAndInsertTo = InsertInProcessQueueTPOToBulkOrderDetails(messageId, bulkUploadMsg); //to be converted to async task/process

                    //Insert Bulk Orders, QueueMessage Status is inProcess
                    if (isFileReadAndInsertTo)
                    {
                        //STEP 2: Read pending records to be processed from the BulkOrderDetails table
                        //QueueMessage id is used as FileId in BulkOrderDetails. NOTE: Ideally we should change FileId to MessageId in BulkOrderDetails
                        unprocessedOrders = GetUnprocessedBulkOrders(messageId);
                        if (unprocessedOrders.Any())
                        {
                            //Step 3: Get User Context
                            AuthenticationDomain authenticationDomain = new AuthenticationDomain(this);
                            var context = authenticationDomain.GetUserContextAsync(bulkUploadMsg.SupplierId, CompanyType.Supplier).Result;

                            //Step 4: Get Fueltypes for the company and all pricing codes
                            //var allFuelTypes = new MasterDomain(this).GetAllFuelProducts(false, context.CompanyId);
                            var allFuelTypes = new MasterDomain(this).GetAllProductsWithAdditives(context.CompanyId);
                            var allPricingCodes = Task.Run(() => new PricingServiceDomain(this).GetPricingCodesAsync(new PricingCodesRequestViewModel())).Result;

                            var isSuppressPricing = false;
                            var onboardingprefId = 0;

                            //Step 5: Get OnBoarding Preferences
                            #region Get OnBoardingPreferences for the Current context company

                            var onboardingPreference = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == context.CompanyId && t.IsActive)
                                                                                                .OrderByDescending(top => top.Id)
                                                                                                .Select(t => new { t.Id, t.IsSupressOrderPricing })
                                                                                                .FirstOrDefault();

                            if (onboardingPreference != null)
                            {
                                isSuppressPricing = onboardingPreference.IsSupressOrderPricing;
                                onboardingprefId = onboardingPreference.Id;
                            }

                            #endregion

                            while (unprocessedOrders.Any())
                            {
                                //Get Bulk Order Ids List
                                var order2List = unprocessedOrders.Select(t => new { tpoCSVViewModel = JsonConvert.DeserializeObject<ThirdPartyOrderCsvViewModelNew>(t.FileData), BulkOrderId = t.Id }).ToList();
                                
                                if (order2List != null && order2List.Any())
                                {
                                    var csvBuyerCompanies = order2List.Where(t => !string.IsNullOrEmpty(t.tpoCSVViewModel.CompanyName)).Select(t => t.tpoCSVViewModel.CompanyName.ToLower()).Distinct().ToList();
                                    var existingNonTpoCompanies = Context.DataContext.Companies.Where(t => csvBuyerCompanies.Contains(t.Name.ToLower()) && t.Users.Any(t1 => t1.OnboardedTypeId == (int)OnboardedType.Direct))
                                                                .Select(t => new ThirdPartyCompaniesFilter
                                                                {
                                                                    CompanyName = t.Name.ToLower(),
                                                                    Emails = t.Users.Select(t1 => t1.Email.ToLower()).ToList(),
                                                                    JobNames = t.Jobs.Select(t1 => t1.Name.ToLower()).ToList()
                                                                }).ToList();

                                    //STEP 6: Process each order to validate and create TPO
                                    foreach (var item in order2List)
                                    {
                                        //ProcessMessage in use is cleared and appended with message for each order in the list and added to error info when needed.
                                        processMessage.Clear();
                                        //Can context loading time create issue?
                                        var bulkOrderDetail = Context.DataContext.BulkOrderDetails.Where(t => t.Id == item.BulkOrderId).FirstOrDefault(); //ToDo: possibility of data not returned (technical service failure? then what should be the business continuity here?
                                        if (bulkOrderDetail != null)
                                        {
                                            try
                                            {
                                                bulkOrderDetail.IsOrderProcessed = true;
                                                Context.DataContext.Entry(bulkOrderDetail).State = EntityState.Modified;
                                                Context.Commit();

                                                //STEP 7: Validate TPO
                                                bulkOrderDetail.Status = (int)BulkOrderDetailsStatus.TPOValidation;
                                                Context.DataContext.Entry(bulkOrderDetail).State = EntityState.Modified;
                                                Context.Commit();
                                                var result = ValidateTPOModel(item.tpoCSVViewModel, existingNonTpoCompanies, allFuelTypes, allPricingCodes.PricingCodes, bulkOrderDetail.CsvLineNumber ?? 0, isSuppressPricing, context.CompanyId);

                                                if (result.StatusCode == Status.Failed)
                                                {
                                                    bulkOrderDetail.Status = (int)BulkOrderDetailsStatus.Failed;
                                                    Context.DataContext.Entry(bulkOrderDetail).State = EntityState.Modified;
                                                    Context.Commit();
                                                    SetFailedProcessMessage(processMessage, item.tpoCSVViewModel, result.StatusMessage);
                                                }
                                                else //use of else gives better readability, then practicing continue, break
                                                {
                                                    //STEP 8: Once validation is sucessfull, Create TPO
                                                    bulkOrderDetail.Status = (int)BulkOrderDetailsStatus.TPOCreation;
                                                    Context.DataContext.Entry(bulkOrderDetail).State = EntityState.Modified;
                                                    Context.Commit();

                                                    var thirdPartyModel = GetThirdPartyOrderViewModelNew(context, item.tpoCSVViewModel, allPricingCodes.PricingCodes, isSuppressPricing, onboardingprefId);

                                                    //to do
                                                    result = CreateThirdPartyOrder(context, thirdPartyModel).Result;

                                                    if (result.StatusCode == Status.Success)
                                                    {
                                                        processMessage.Append(SetSuccessProcessMessage(item.tpoCSVViewModel));
                                                        bulkOrderDetail.Status = (int)BulkOrderDetailsStatus.Completed;
                                                        Context.DataContext.Entry(bulkOrderDetail).State = EntityState.Modified;
                                                        Context.Commit();
                                                    }
                                                    else
                                                    {
                                                        bulkOrderDetail.Status = (int)BulkOrderDetailsStatus.Failed;
                                                        Context.DataContext.Entry(bulkOrderDetail).State = EntityState.Modified;
                                                        Context.Commit();
                                                        var failedMsg = $"{result.StatusMessage} at line {bulkOrderDetail.CsvLineNumber}. ";
                                                        SetFailedProcessMessage(processMessage, item.tpoCSVViewModel, failedMsg);
                                                        throw new QueueMessageFatalException(processMessage.ToString(), errorInfo);
                                                    }
                                                }
                                            }

                                            catch (Exception ex)
                                            {
                                                string failedMsg = "";
                                                //update status to failed
                                                bulkOrderDetail.Status = (int)BulkOrderDetailsStatus.Failed;
                                                Context.DataContext.Entry(bulkOrderDetail).State = EntityState.Modified;
                                                Context.Commit();
                                                if (processMessage.Length == 0)
                                                {
                                                    if (!(ex is TfxException))
                                                    {
                                                        failedMsg = $"{Constants.ErrorWhileProcessingBulkOrder} at line {bulkOrderDetail.CsvLineNumber}. ";
                                                        SetFailedProcessMessage(processMessage, item.tpoCSVViewModel, failedMsg);
                                                    }
                                                    else
                                                    {
                                                        failedMsg = $"{ex.Message} at line {bulkOrderDetail.CsvLineNumber}. ";
                                                        SetFailedProcessMessage(processMessage, item.tpoCSVViewModel, failedMsg);
                                                    }
                                                }
                                                
                                                LogManager.Logger.WriteException("ThirdPartyDomain", "ProcessOrderBulkUploadJsonMessage", "TPO bulkupload failed, BulkOrderDetails Id is " + item.BulkOrderId.ToString(), ex);
                                            }

                                            if (context != null)
                                            {
                                                QueueMessageDomain queueMessage = new QueueMessageDomain();
                                                queueMessage.SetInProcessQueueMessageStatus(messageId, context.Id, processMessage.ToString());
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    errToReadAtleaseOneRecord = false;
                                }

                                //Get Next Bulk Orders
                                unprocessedOrders = GetUnprocessedBulkOrders(messageId);
                            }
                        }
                        else
                        {
                            errToReadAtleaseOneRecord = false;
                        }
                        
                        if (!errToReadAtleaseOneRecord)
                        {
                            errorInfo.Add(Resource.errMessageFailedToReadFileContent);
                        }
                    }
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                    {
                        LogManager.Logger.WriteException("ThirdPartyOrderDomain", "ProcessOrderBulkUploadJsonMessage", ex.Message, ex);
                    }

                    if (processMessage.Length == 0)
                    {
                        if (!(ex is TfxException))
                        {
                            processMessage.Append(Constants.ErrorWhileProcessingBulkOrder);
                        }else
                        {
                            processMessage.Append(ex.Message);
                        }
                        errorInfo.Add(processMessage.ToString());
                    }
                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                }
                unprocessedOrders = GetUnprocessedBulkOrders(messageId);
                return isFileReadAndInsertTo && !unprocessedOrders.Any(); //record inserted and nothing to process.
            }
        }

        public bool InsertInProcessQueueTPOToBulkOrderDetails(int messageId, ThirdPartyBulkUploadQueueMsg bulkUploadMsg)
        {
            var azureBlob = new AzureBlobStorage();
            var fileStream = azureBlob.DownloadBlob(bulkUploadMsg.FileUploadPath, BlobContainerType.Orderbulkupload.ToString().ToLower());
            if (fileStream != null)
            {
                var csvOrderList = ReadCSVFile<ThirdPartyOrderCsvViewModelNew>(fileStream, true); //The whole file is already read into memory here.

                var bkOrderDetailsCount = Context.DataContext.BulkOrderDetails.Where(t => t.FileID == messageId)?.Count();
                int startFrom = 0; //to avoid job stop and restart, leading to insertion of same record multiple times.

                if (bkOrderDetailsCount > 0)
                    startFrom = (int)bkOrderDetailsCount;

                if (csvOrderList?.Count == startFrom)
                    return true;
                else if (csvOrderList != null && csvOrderList.Any())
                {
                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        //Context.DataContext.Configuration.AutoDetectChangesEnabled = false;
                        //Save 50 orders at a time to improve insert performance
                        int ordersCount = 50;
                        for (int i = startFrom; i < csvOrderList.Count; i++)
                        {
                            var entity = new BulkOrderDetails();
                            string strOrder = JsonConvert.SerializeObject(csvOrderList[i]);
                            //entity = order.ToEntity(entity);
                            entity.FileData = strOrder;
                            entity.FileID = messageId; //item.Id;
                            entity.CsvLineNumber = i+1;
                            Context.DataContext.BulkOrderDetails.Add(entity);
                            --ordersCount;
                            if (ordersCount <= 0)
                            {
                                ordersCount = 50;
                                Context.Commit();
                            }
                        }
                        Context.Commit(); //TODO: check lock if any, as updates are in bulk through context.
                        transaction.Commit();
                    };
                }
                return true;
            }
            return false; //if no file found
        }

        private List<BulkOrderDetails> GetUnprocessedBulkOrders(int fileId)
        {
            //Check if insertions are pending
            var inputmodel = new { FileId = fileId };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetBulkOrders", inputmodel); //pass the messageid to get records of only the specific messageid mapped to file id records
            Context.DataContext.Database.CommandTimeout = 30; 
            var response = Context.DataContext.Database.SqlQuery<BulkOrderDetails>(input.Query, input.Params.ToArray()).ToList();
            return response;
        }

        
        //public string ProcessBulkUploadJsonMessage(ThirdPartyBulkUploadQueueMsg bulkUploadMsg, List<string> errorInfo)
        //{
        //    using (var tracer = new Tracer("ThirdPartyDomain", "ProcessBulkUploadJsonMessage"))
        //    {
        //        StringBuilder processMessage = new StringBuilder();

        //        try
        //        {
        //            var azureBlob = new AzureBlobStorage();
        //            var fileStream = azureBlob.DownloadBlob(bulkUploadMsg.FileUploadPath, BlobContainerType.Orderbulkupload.ToString().ToLower());
        //            if (fileStream != null)
        //            {
        //                string csvText = new StreamReader(fileStream).ReadToEnd();
        //                if (!string.IsNullOrWhiteSpace(csvText))
        //                {
        //                    var filteredCsvText = RemoveHeaderAndGuidelinesFromFile(csvText);
        //                    var engine = new FileHelperEngine<ThirdPartyOrderCsvViewModel>();
        //                    var csvOrderList = engine.ReadString(filteredCsvText).ToList();

        //                    List<ThirdPartyOrderViewModel> viewModel = GetThirdPartyOrderViewModel(csvOrderList);
        //                    AuthenticationDomain authenticationDomain = new AuthenticationDomain(this);
        //                    var context = authenticationDomain.GetUserContextAsync(bulkUploadMsg.SupplierId, CompanyType.Supplier).Result;

        //                    foreach (var item in viewModel)
        //                    {
        //                        processMessage.Clear();
        //                        try
        //                        {
        //                            var result = CreateThirdPartyOrder(context, item).Result;
        //                            if (result.StatusCode == Status.Success)
        //                                errorInfo.Add(SetSuccessProcessMessage(item));
        //                            else
        //                            {
        //                                SetFailedProcessMessage(processMessage, item, result.StatusMessage);
        //                                errorInfo.Add(processMessage.ToString());
        //                                throw new QueueMessageFatalException(errorInfo[0], errorInfo);
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            if (!errorInfo.Any())
        //                            {
        //                                SetFailedProcessMessage(processMessage, item, Constants.ErrorWhileProcessingBulkOrder);
        //                                errorInfo.Add(processMessage.ToString());
        //                            }
        //                            LogManager.Logger.WriteException("ThirdPartyDomain", "ProcessBulkUploadJsonMessage", "TPO bulkupload failed", ex);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    processMessage.Append(Resource.errMessageFailedToReadFileContent);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            if (!(ex is QueueMessageFatalException))
        //                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "ProcessBulkUploadJsonMessage", ex.Message, ex);
        //            if (processMessage.Length == 0)
        //            {
        //                processMessage.Append(Constants.ErrorWhileProcessingBulkOrder);
        //                errorInfo.Add(processMessage.ToString());
        //            }
        //            throw new QueueMessageFatalException(errorInfo[0], errorInfo);
        //        }
        //        return processMessage.ToString();
        //    }
        //}
        #region Validate New TPO Bulk Order

        private StatusViewModel ValidateTPOModel(ThirdPartyOrderCsvViewModelNew viewModel, List<ThirdPartyCompaniesFilter> existingNonTpoCompanies, List<DropdownDisplayItem> allFuelTypes, List<PricingCodesViewModel> pricingCodes, int CsvLineNumber, bool isSuppressPricing, int companyId)
        {
            var response = new StatusViewModel(Status.Success);
            try
            {
                var existingBuyerCompany = existingNonTpoCompanies.FirstOrDefault(t => t.CompanyName == viewModel.CompanyName.ToLower());
                var isPricingNeeded = !isSuppressPricing;
                if (existingBuyerCompany != null && !isSuppressPricing)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = string.Format(Resource.errMsgNonTpoCompanyPricingRequired, viewModel.CompanyName, CsvLineNumber);
                    return response;
                }

                if (IsRequiredFieldMissing(viewModel, isPricingNeeded))
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = string.Format(Resource.errMsgRequiredFieldAreMissing, CsvLineNumber);
                    return response;
                }

                if (IsInvalidChoiceField(viewModel, isSuppressPricing))
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = string.Format(Resource.errMessageInvalidRecordAtLine, CsvLineNumber);
                    return response;
                }
                // Is Existing buyer Company
                //if (existingNonTpoCompanies.Contains(viewModel.CompanyName))
                //{
                //    response.StatusCode = Status.Failed;
                //    response.StatusMessage = $"{Resource.errMessageCompanyAlreadyExists} {string.Format(Resource.errMessageInvalidRecordAtTPOBulkOrder, lineNumberOfCSV)}";
                //    return response;
                //}
                if (existingBuyerCompany != null)
                {
                    // Company User verification
                    if (!existingBuyerCompany.Emails.Contains(viewModel.Email.ToLower()))
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = $"{Resource.errMessageUserNotExist} {string.Format(Resource.errMessageInvalidRecordAtLine, CsvLineNumber)}";
                        return response;
                    }
                    // existing job validation
                    if (!existingBuyerCompany.JobNames.Contains(viewModel.LocationName.ToLower()))
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = string.Format(Resource.errMessageAlreadyJobNameExists, viewModel.LocationName, CsvLineNumber);
                        return response;
                    }
                }
                // validate Fueltype
                if (!string.IsNullOrWhiteSpace(viewModel.FuelType) && !allFuelTypes.Any(t => t.Name.ToLower().Equals(viewModel.FuelType.ToLower())))
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = string.Format(Resource.errMessageInvalidFuelTypeAtLine, viewModel.FuelType, CsvLineNumber);
                    return response;
                }
                // pricing validation
                if (!string.IsNullOrEmpty(viewModel.PricingType) && viewModel.PricingType.ToLower().Equals("market based") && !pricingCodes.Any(t => t.Code.ToLower().Equals(viewModel.PricingCode.ToLower())))
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = string.Format(Resource.errMessageInvalidPricingCodeAtLine, viewModel.FuelType, CsvLineNumber);
                    return response;
                }
                if (viewModel.AutoFreightPricingMethod !=null){
                    if (string.IsNullOrEmpty(viewModel.AutoFreightPricingMethod) || (!viewModel.AutoFreightPricingMethod.ToLower().Trim().Equals("yes")
                        && !viewModel.AutoFreightPricingMethod.ToLower().Trim().Equals("no")))
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = string.Format(Resource.errMessageInvalidRecordAtLine, CsvLineNumber);
                        return response;
                    }
                    if (viewModel.AutoFreightPricingMethod.ToLower().Trim().Equals("yes"))
                    {
                        if (string.IsNullOrEmpty(viewModel.SourceRegionNames.Trim()))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInvalidRecordAtLine, CsvLineNumber);
                            return response;
                        }

                        if (string.IsNullOrEmpty(viewModel.TerminalNames))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageInvalidRecordAtLine, CsvLineNumber);
                            return response;
                        }


                        if (!viewModel.TerminalNames.ToLower().Contains("all") && !string.IsNullOrWhiteSpace(viewModel.ApprovedSourceTerminal))
                        {
                            if (!viewModel.TerminalNames.ToLower().Trim().Split('|').ToList().Contains(viewModel.ApprovedSourceTerminal.Trim().ToLower()))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageInvalidRecordAtLine, CsvLineNumber);
                                return response;
                            }
                        }


                        if (!viewModel.BulkPlantNames.ToLower().Contains("all") && !string.IsNullOrWhiteSpace(viewModel.ApprovedBulkPlant))
                        {
                            if (!viewModel.BulkPlantNames.ToLower().Trim().Split('|').ToList().Contains(viewModel.ApprovedBulkPlant.Trim().ToLower()))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageInvalidRecordAtLine, CsvLineNumber);
                                return response;
                            }
                        }
                        List<string> selectedNames = new List<string>();
                        List<string> sourceNames = new List<string>();
                        List<SourceRegion> sourceRegions = new List<SourceRegion>();


                        if (!string.IsNullOrWhiteSpace(viewModel.SourceRegionNames))
                        {
                            sourceNames = viewModel.SourceRegionNames.Trim().Split('|').ToList();
                            if (!sourceNames.First().Trim().ToLower().Contains("all"))
                            {

                                sourceRegions = (from t in Context.DataContext.SourceRegions
                                                 join i in sourceNames
                                                 on t.Name.ToUpper() equals i.ToUpper()
                                                 where t.CompanyId == companyId && t.IsActive
                                                 select t).ToList();

                                if (sourceRegions == null || !sourceNames.All(sourceRegions.Select(t1 => t1.Name).ToList().Contains))
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessageInvalidRecordAtLine, CsvLineNumber);
                                    return response;
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(viewModel.TerminalNames) && sourceRegions != null && sourceRegions.Count > 0)
                        {

                            sourceNames = viewModel.TerminalNames.Trim().Split('|').ToList();

                            var items = sourceRegions.SelectMany(t1 => t1.SourceRegionPickupLocation).Select(t2 => t2.MstExternalTerminal).Where(t1 => t1 != null).ToList();

                            if (items != null && items.Count > 0)
                            {
                                if (!sourceNames.First().Trim().ToLower().Contains("all"))
                                {
                                    selectedNames = items.Where(t1 => t1 != null && sourceNames.Contains(t1.Name)).Select(t1 => t1.Name).Distinct().ToList();

                                    if (!sourceNames.All(selectedNames.Contains))
                                    {
                                        response.StatusCode = Status.Failed;
                                        response.StatusMessage = string.Format(Resource.errMessageInvalidRecordAtLine, CsvLineNumber);
                                        return response;
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(viewModel.BulkPlantNames) && sourceRegions != null && sourceRegions.Count > 0)
                        {

                            var lst = viewModel.BulkPlantNames.Trim().Split('|').ToList();
                            var items = sourceRegions.SelectMany(t1 => t1.SourceRegionPickupLocation).Select(t2 => t2.BulkPlantLocation).Where(t1 => t1 != null).ToList();


                            if (items != null && items.Count > 0)
                            {
                                if (!lst.First().Trim().ToLower().Contains("all"))
                                {
                                    selectedNames = items.Where(t3 => t3 != null && lst.Contains(t3.Name)).Select(t3 => t3.Name).Distinct().ToList();
                                    if (!selectedNames.All(lst.Contains))
                                    {
                                        response.StatusCode = Status.Failed;
                                        response.StatusMessage = string.Format(Resource.errMessageInvalidRecordAtLine, CsvLineNumber);
                                        return response;
                                    }
                                }
                            }
                        }


                        if (!string.IsNullOrWhiteSpace(viewModel.ApprovedSourceTerminal) && sourceRegions != null && sourceRegions.Count > 0)
                        {
                            var items = sourceRegions.SelectMany(t1 => t1.SourceRegionPickupLocation).Select(t2 => t2.MstExternalTerminal).Where(t1 => t1 != null).ToList();

                            if (items != null && items.Count > 0)
                            {
                                selectedNames = items.Where(t3 => t3 != null && t3.Name.Contains(viewModel.ApprovedSourceTerminal)).Select(t3 => t3.Name).Distinct().ToList();
                                if (!selectedNames.Contains(viewModel.ApprovedSourceTerminal))
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessageInvalidRecordAtLine, CsvLineNumber);
                                    return response;
                                }
                            }
                        }


                        if (!string.IsNullOrWhiteSpace(viewModel.ApprovedBulkPlant) && sourceRegions != null && sourceRegions.Count > 0)
                        {
                            var items = sourceRegions.SelectMany(t1 => t1.SourceRegionPickupLocation).Select(t2 => t2.BulkPlantLocation).Where(t1 => t1 != null).ToList();
                            if (items != null)
                            {
                                selectedNames = items.Where(t3 => t3 != null && t3.Name.Contains(viewModel.ApprovedBulkPlant)).Select(t3 => t3.Name).Distinct().ToList();
                                if (!selectedNames.Contains(viewModel.ApprovedBulkPlant))
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessageInvalidRecordAtLine, CsvLineNumber);
                                    return response;
                                }
                            }

                        }

                    }
                }
                response.StatusCode = Status.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Constants.ErrorWhileProcessingBulkOrder;
                LogManager.Logger.WriteException("ThirdPartyDomain", "ValidateTPOModel", ex.Message, ex);
            }
            return response;
        }
        private static string SetFailedProcessMessage(StringBuilder processMessage, ThirdPartyOrderCsvViewModelNew item, string message)
        {
            processMessage.Append("<p class='color-maroon'>").Append("<b>Order Info: </b>")
                            .Append($"Customer: {item.CompanyName}, " +
                            $"Location: {item.Address}, FuelType: {item.FuelType}, PO#: {item.PONumber} <br><b>Processing failed Reason:</b> {message}</p><br>");
            return processMessage.ToString();
        }


        private static string SetSuccessProcessMessage(ThirdPartyOrderCsvViewModelNew item)
        {
            StringBuilder processMessage = new StringBuilder();
            processMessage.Append("<p class='color-green'>").Append("<b>Order Info: </b>")
                            .Append($"Customer: {item.CompanyName}, " +
                            $"Location: {item.Address}, FuelType: {item.FuelType}, PO#: {item.PONumber} <br><b>Order created successfully</b></p><br>");
            return processMessage.ToString();

        }

        private bool IsRequiredFieldMissing(ThirdPartyOrderCsvViewModelNew record, bool isPricingNeeded) => string.IsNullOrWhiteSpace(record.CompanyName)
           || string.IsNullOrWhiteSpace(record.ContactPerson) || string.IsNullOrWhiteSpace(record.Email)
           || string.IsNullOrWhiteSpace(record.MobileNumber) || string.IsNullOrWhiteSpace(record.TruckLoadType) || IsRequiredFOBMissing(record)
           || string.IsNullOrWhiteSpace(record.LocationName) || string.IsNullOrWhiteSpace(record.GeoCoded) || IsRequiredAddressFieldMissing(record)
            || string.IsNullOrWhiteSpace(record.Currency) || string.IsNullOrWhiteSpace(record.UOM) || string.IsNullOrWhiteSpace(record.DeliveryType)
            || string.IsNullOrWhiteSpace(record.DeliveryStartDate) || string.IsNullOrWhiteSpace(record.DeliveryStartTime)
            || string.IsNullOrWhiteSpace(record.DeliveryEndTime) || (string.IsNullOrWhiteSpace(record.FuelType) && string.IsNullOrWhiteSpace(record.NonStandardFuelType))
            || string.IsNullOrWhiteSpace(record.QuantityType) || IsRequiredQuantityMissing(record)
            || (isPricingNeeded && (string.IsNullOrWhiteSpace(record.Price) || string.IsNullOrEmpty(record.PricingType) || IsPricingCodeMissing(record) || IsCityRackMissing(record) || IsRequiredPaymentTermMissing(record)))
            || IsCarrierDetailMissing(record) || IsFeeDetailMissing(record);

        private bool IsRequiredAddressFieldMissing(ThirdPartyOrderCsvViewModelNew record)
        {
            if (record.GeoCoded.ToLower() == "yes")
            {
                return string.IsNullOrWhiteSpace(record.Latitude) || string.IsNullOrWhiteSpace(record.Longitude);
            }
            else
            {
                return string.IsNullOrWhiteSpace(record.Address) || string.IsNullOrWhiteSpace(record.LocationCity)
                       || string.IsNullOrWhiteSpace(record.LocationState) || string.IsNullOrWhiteSpace(record.ZipCode)
                       || string.IsNullOrWhiteSpace(record.Country);
            }
        }

        private bool IsCityRackMissing(ThirdPartyOrderCsvViewModelNew record)
        {
            return !string.IsNullOrEmpty(record.PricingType) && record.PricingType.ToLower().Equals("market based") && !record.PricingCode.ToLower().StartsWith("a") && string.IsNullOrEmpty(record.CityRackTerminal);
        }
        private bool IsRequiredPaymentTermMissing(ThirdPartyOrderCsvViewModelNew record)
        {
            return string.IsNullOrEmpty(record.PaymentTerm) || (record.PaymentTerm.ToLower() == "net" && string.IsNullOrEmpty(record.NetDays));
        }
        private bool IsPricingCodeMissing(ThirdPartyOrderCsvViewModelNew record)
        {
            return (record.PricingType.ToLower().Equals("market based") && string.IsNullOrEmpty(record.PricingCode));
        }
        private bool IsCarrierDetailMissing(ThirdPartyOrderCsvViewModelNew record)
        {
            return !string.IsNullOrEmpty(record.LocationInventoryManagementType) && record.LocationInventoryManagementType.ToLower().Equals("fully carrier managed") && (string.IsNullOrEmpty(record.CarrierCompanyName) || string.IsNullOrEmpty(record.CarrierEmailAddress));
        }
        private bool IsRequiredFOBMissing(ThirdPartyOrderCsvViewModelNew record)
        {
            return string.IsNullOrEmpty(record.TruckLoadType) || (record.TruckLoadType.ToLower() == "FTL" && string.IsNullOrEmpty(record.FreightOnBoardType));
        }
        private bool IsFeeDetailMissing(ThirdPartyOrderCsvViewModelNew record)
        {
            return !string.IsNullOrEmpty(record.MinimumGallonFee) && string.IsNullOrEmpty(record.MinimumGallons);
        }
        private bool IsRequiredQuantityMissing(ThirdPartyOrderCsvViewModelNew record)
        {
            return (record.QuantityType.ToLower().Equals("Fixed") && string.IsNullOrWhiteSpace(record.Quantity))
                || (record.QuantityType.ToLower().Equals("Range") && string.IsNullOrWhiteSpace(record.MaximumQuantity));
        }

        private bool IsInvalidChoiceField(ThirdPartyOrderCsvViewModelNew record, bool isSuppressPricing)
        {
            return !(record.GeoCoded.IsKeyMatchAnyValue(ApplicationConstants.Yes, ApplicationConstants.No)
                     && record.Currency.IsKeyMatchAnyValue(Currency.USD.ToString(), Currency.CAD.ToString())
                     && record.Country.IsKeyMatchAnyValue(Country.USA.ToString(), Country.CAN.ToString(), Country.CAR.ToString())
                     && record.TruckLoadType.IsKeyMatchAnyValue("Ltl", "Ftl") && record.DeliveryType.IsKeyMatchAnyValue("Single", "Multiple")
                     && record.QuantityIndicatorType.IsKeyMatchAnyValue("Net", "Gross") && record.QuantityType.IsKeyMatchAnyValue("Fixed", "Range", "Not Specified")
                     && (string.IsNullOrEmpty(record.AutomateDeliveryRequest) || record.AutomateDeliveryRequest.IsKeyMatchAnyValue(ApplicationConstants.Yes, ApplicationConstants.No))
                     && (isSuppressPricing || (record.PricingType.IsKeyMatchAnyValue("Fixed", "Market Based", "FuelCost") && record.PaymentTerm.IsKeyMatchAnyValue("Net", "DOR", "Prepaid"))));
        }
        #endregion

        private static string SetAmpSuccessProcessMessage(AmpJobViewModel item, Job jobDetails)
        {
            StringBuilder processMessage = new StringBuilder();
            processMessage.Append("<p class='color-green'>").Append("<b>Job Info: </b>")
                            .Append($"Customer: {jobDetails.Company.Name}, " +
                            $"Job: {item.JobName}, YardNumber: {item.YardNumber} <br><b>Job processed successfully</b></p><br>");
            return processMessage.ToString();
        }

        private static string SetAmpFailedProcessMessage(AmpJobViewModel item)
        {
            StringBuilder processMessage = new StringBuilder();
            processMessage.Append("<p class='color-maroon'>").Append("<b>Job Info: </b>")
                            .Append($"Job: {item.JobName}, <br><b>Processing failed Reason:</b> {item.JobName} not found.</p><br>");
            return processMessage.ToString();
        }

        private static string SetAmpOrderFailedProcessMessage(AmpJobViewModel item)
        {
            StringBuilder processMessage = new StringBuilder();
            processMessage.Append("<p class='color-maroon'>").Append("<b>Job Info: </b>")
                            .Append($"Job: {item.JobName}, FuelType: {item.AmpProductType} <br><b>Processing failed Reason:</b>Order with {item.AmpProductType} not found.</p><br>");
            return processMessage.ToString();

        }
        public List<RequestStatusViewModel> GetUploadDetails(int userId, List<QueueProcessType> processTypes)
        {
            using (var tracer = new Tracer("ThirdPartyDomain", "GetUploadDetails"))
            {
                var response = new List<RequestStatusViewModel>();
                try
                {
                    var queueMessageDomain = new QueueMessageDomain();
                    var queueMessages = queueMessageDomain.GetMessagesRequestedByUser(userId, processTypes);
                    queueMessages = queueMessages.OrderByDescending(t => t.MessageId).ToList();
                    var requestType = Constants.SFXOrderBulkUploadSuffix;
                    if (processTypes != null && processTypes.Any(t => t == QueueProcessType.ExternalMeterDataUpload))
                    {
                        requestType = Constants.SFXAMPDataUploadSuffix;
                    }
                    foreach (var item in queueMessages)
                    {
                        var messageDetails = new StringBuilder();
                        var message = new RequestStatusViewModel();
                        message.RequestNumber = string.Concat(requestType, item.MessageId.ToString("000"));
                        message.Status = item.Status.ToString();
                        message.TimeRequested = item.TimeRequested;
                        message.QueueProcessoryType = item.QueueProcessType.GetDisplayName();
                        message.UploadedDateTime = item.TimeRequested.ToClientDateTime();
                        if (item.MessageResults.Count == 0)
                        {
                            messageDetails.Append("Request under process. Please visit after some time");
                        }
                        else
                        {
                            foreach (var result in item.MessageResults)
                            {
                                messageDetails.Append(result.ErrorInfo);
                            }
                        }
                        message.Details = messageDetails.ToString();
                        messageDetails.Clear();
                        response.Add(message);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetUploadDetails", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<List<string>> ProcessAmpDataStreamMessage(ExternalMeterDataUploadQueueMsg queueMsg)
        {
            var errorInfo = new List<string>();
            using (var tracer = new Tracer("ThirdPartyDomain", "ProcessAmpDataStreamMessage"))
            {
                StringBuilder processMessage = new StringBuilder();
                try
                {
                    var azureBlob = new AzureBlobStorage();
                    var fileStream = azureBlob.DownloadBlob(queueMsg.FileUploadPath, BlobContainerType.ExternalMeterFeed.ToString().ToLower());
                    if (fileStream != null)
                    {
                        string plainText = new StreamReader(fileStream).ReadToEnd();
                        if (!string.IsNullOrWhiteSpace(plainText))
                        {
                            var ampJobList = GetAmpJobViewModels(plainText);
                            var fuelTypeJobList = SetFuelTypeId(ampJobList, errorInfo);
                            foreach (var job in fuelTypeJobList)
                            {
                                job.SupplierCompanyId = queueMsg.SupplierCompanyId;
                            }

                            await ProcessAmpJobList(fuelTypeJobList, errorInfo);
                            if (!errorInfo.Any())
                            {
                                throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                            }
                        }
                        else
                        {
                            processMessage.Append(Resource.errMessageFailedToReadFileContent);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                    {
                        LogManager.Logger.WriteException("ThirdPartyOrderDomain", "ProcessAmpDataStreamMessage", ex.Message, ex);
                    }

                    if (processMessage.Length == 0)
                    {
                        if (!(ex is TfxException))
                        {
                            processMessage.Append(Constants.ErrorWhileProcessingBulkOrder);
                            errorInfo.Add(processMessage.ToString());
                        }
                        else
                        {
                            errorInfo.Add(processMessage.ToString());
                        }
                    }


                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                }
                return errorInfo;
            }
        }

        public async Task<List<string>> UploadDtnFileToFtpLocation(DtnFileProcessingRequestViewModel viewModel)
        {
            var errorInfo = new List<string>();
            using (var tracer = new Tracer("ThirdPartyDomain", "ProcessDtnFile"))
            {
                StringBuilder processMessage = new StringBuilder();
                try
                {
                    var invoiceReadDomain = new InvoiceReadDomain(this);
                    string dtnFileCsvString = await invoiceReadDomain.GenerateDtnFileAsync(viewModel.InvoiceId, viewModel.RefId, viewModel.Password, viewModel.SiteNumber);
                    if (!string.IsNullOrWhiteSpace(dtnFileCsvString))
                    {
                        FtpFileUploader ftpFileUploader = new FtpFileUploader();
                        ftpFileUploader.UploadDtnFile(viewModel.InvoiceNumber, dtnFileCsvString, viewModel.FtpUserName, viewModel.FtpPassword, viewModel.FtpUrl, viewModel.PathToUpload);
                        if (viewModel.ReceiversEmail.Any() && !string.IsNullOrEmpty(viewModel.FtpUrl))
                        {
                            await SendDtnFileUploadedNotification(viewModel.ReceiversEmail, viewModel.InvoiceNumber);
                        }
                    }
                    else
                    {
                        errorInfo.Add(Resource.errWhileGeneratingDtnFile);
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                    {
                        LogManager.Logger.WriteException("ThirdPartyOrderDomain", "ProcessDtnFile", ex.Message, ex);
                    }

                    if (processMessage.Length == 0)
                    {
                        processMessage.Append(Constants.RequestError);
                        errorInfo.Add(processMessage.ToString());
                    }
                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                }
                return errorInfo;
            }
        }

        private async Task SendDtnFileUploadedNotification(List<string> emailReceivers, string invoiceNumber)
        {
            var helperDomain = new HelperDomain(this);
            var notificationDomain = new NotificationDomain(this);
            var serverUrl = helperDomain.GetServerUrl();
            var notification = notificationDomain.GetNotificationContent(EventSubType.DtnFileUploaded, serverUrl, string.Empty);
            var emailTemplate = helperDomain.GetApplicationEventNotificationTemplate();
            var emailModel = new ApplicationEventNotificationViewModel
            {
                To = emailReceivers,
                Subject = string.Format(notification.Subject, invoiceNumber),
                CompanyLogo = notification.CompanyLogo,
                BodyText = string.Format(notification.BodyText, invoiceNumber),
                ShowFooterContent = false,
                ShowHelpLineInfo = false,
                ShowUserSettingsLink = false
            };
            await new EmailDomain(this).SendEmail(emailTemplate, emailModel);
        }

        #region TelaFuelService
        public async Task<List<string>> ProcessTelapointOrderAdd(TelaFuelServiceRequestViewModel viewModel)
        {
            var errorInfo = new List<string>();
            using (var tracer = new Tracer("ThirdPartyDomain", "ProcessTelapointApi"))
            {
                StringBuilder processMessage = new StringBuilder();
                try
                {
                    int telaFuelServiceOrderNumber = 0;
                    if (viewModel.LocationInventoryManagedBy == LocationInventoryManagedBy.Telapoint)
                    {
                        telaFuelServiceOrderNumber = await GetTelaFuelServiceOrders(viewModel);
                    }

                    if (telaFuelServiceOrderNumber <= 0)
                    {
                        telaFuelServiceOrderNumber = await AddOrderToTelaFuelService(viewModel);
                    }

                    if (telaFuelServiceOrderNumber > 0)
                    {
                        UpdateInvoicePONumber(viewModel.InvoiceId, telaFuelServiceOrderNumber);
                    }

                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                    {
                        LogManager.Logger.WriteException("ThirdPartyOrderDomain", "ProcessTelapointApi", ex.Message, ex);
                    }

                    if (processMessage.Length == 0)
                    {
                        processMessage.Append(Constants.RequestError);
                        errorInfo.Add(processMessage.ToString());
                    }
                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                }
                return errorInfo;
            }
        }
        public async Task<int> GetTelaFuelServiceOrders(TelaFuelServiceRequestViewModel viewModel)
        {
            using (var tracer = new Tracer("ThirdPartyDomain", "GetTelaFuelServiceOrdersByStatus"))
            {
                
                int telaFuelServiceOrderNumber = 0;
                try
                {
                    var invoiceDomain = new InvoiceDomain(this);
                    ConsolidatedInvoicePdfViewModel consolidatedInvoicePdfViewModel = await invoiceDomain.GetConsolidatedInvoicePdfAsync(viewModel.InvoiceId, CompanyType.Supplier);
                    ConsolidatedInvoiceViewModel consolidatedInvoiceViewModel = consolidatedInvoicePdfViewModel.Invoices[0];

                    TelaFuelServiceDomain telaFuelServiceDomain = new TelaFuelServiceDomain(viewModel.UserName, viewModel.Password);
                    var orderIds = telaFuelServiceDomain.GetOrdersByStatus(consolidatedInvoiceViewModel.DropStartDate.Date, consolidatedInvoiceViewModel.DropEndDate.Date);

                    if (orderIds != null && orderIds.Any())
                    {
                        foreach (var orderId in orderIds)
                        {
                            int telaFuelOrderId;
                            if (int.TryParse(orderId, out telaFuelOrderId))
                            {
                                telaFuelServiceOrderNumber = telaFuelServiceDomain.GetTelaFuelServiceOrderNumber(telaFuelOrderId, consolidatedInvoicePdfViewModel, viewModel.SupplierLookup);
                                if (telaFuelServiceOrderNumber > 0)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return telaFuelServiceOrderNumber;
            }
        }
        public async Task<int> AddOrderToTelaFuelService(TelaFuelServiceRequestViewModel viewModel)
        {
            using (var tracer = new Tracer("ThirdPartyDomain", "AddOrderToTelaFuelService"))
            {
                
                int telaFuelServiceOrderNumber = 0;
                try
                {
                    var jobTimeZoneName = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == viewModel.InvoiceId).Select(t => t.Order.FuelRequest.Job.TimeZoneName).FirstOrDefault();
                    var invoiceDomain = new InvoiceDomain(this);
                    ConsolidatedInvoicePdfViewModel consolidatedInvoicePdfViewModel = await invoiceDomain.GetConsolidatedInvoicePdfAsync(viewModel.InvoiceId, CompanyType.Supplier);
                    var jsonViewModel = new TelaFuelServiceViewModel();
                    jsonViewModel.BillToName = consolidatedInvoicePdfViewModel.InvoicePdfHeaderDetail.BuyerCompanyName;
                    jsonViewModel.CarrierLookup = viewModel.CarrierLookup;
                    jsonViewModel.FreightLaneLookup = viewModel.FreightLaneLookup;
                    jsonViewModel.IsDelivered = true;
                    jsonViewModel.ReferenceNumber = consolidatedInvoicePdfViewModel.InvoicePdfHeaderDetail.DisplayInvoiceNumber;
                    if (consolidatedInvoicePdfViewModel.LiftDetails != null && consolidatedInvoicePdfViewModel.LiftDetails.Any())
                    {
                        jsonViewModel.OrderLifts = consolidatedInvoicePdfViewModel.LiftDetails.Select((t, i) => new OrderLift()
                        {
                            BillOfLadingNumber = !string.IsNullOrEmpty(t.LiftTicketNumber) ? t.LiftTicketNumber : consolidatedInvoicePdfViewModel.InvoicePdfHeaderDetail.DisplayInvoiceNumber,
                            LiftDateTimeLocal = GetLiftDate(consolidatedInvoicePdfViewModel, t),
                            LiftDateTimeLocalTimeZone = jobTimeZoneName,
                            SupplierLookup = viewModel.SupplierLookup,
                            SequenceNumber = jsonViewModel.OrderLifts != null ? Convert.ToInt16(jsonViewModel.OrderLifts.Count + 1) : Convert.ToInt16(1),
                            InvoiceId = t.InvoiceId,
                            LiftProducts = new List<LiftProduct>
                        {
                          new LiftProduct()
                          {
                            GrossQuantity = GetGrossQuantity(consolidatedInvoicePdfViewModel, t),
                            NetQuantity = GetNetQuantity(consolidatedInvoicePdfViewModel, t),
                            ProductLookup = t.FuelType,
                            TMWProductId = t.FuelType,
                            SequenceNumber = 1
                          }
                        },
                        }).ToList();
                    }
                    if (consolidatedInvoicePdfViewModel.BolDetails != null && consolidatedInvoicePdfViewModel.BolDetails.Any())
                    {
                        if (jsonViewModel.OrderLifts == null)
                        {
                            jsonViewModel.OrderLifts = new List<OrderLift>();
                        }
                        foreach (var t in consolidatedInvoicePdfViewModel.BolDetails)
                        {
                            OrderLift orderLift = new OrderLift()
                            {
                                BillOfLadingNumber = !string.IsNullOrEmpty(t.LiftTicketNumber) ? t.LiftTicketNumber : consolidatedInvoicePdfViewModel.InvoicePdfHeaderDetail.DisplayInvoiceNumber,
                                LiftDateTimeLocal = GetLiftDate(consolidatedInvoicePdfViewModel, t),
                                LiftDateTimeLocalTimeZone = jobTimeZoneName,
                                SupplierLookup = viewModel.SupplierLookup,
                                SequenceNumber = jsonViewModel.OrderLifts != null ? Convert.ToInt16(jsonViewModel.OrderLifts.Count + 1) : Convert.ToInt16(1),
                                InvoiceId = t.InvoiceId,
                                LiftProducts = new List<LiftProduct>
                                {
                                    new LiftProduct()
                                    {
                                        GrossQuantity =  t.GrossQuantity,
                                        NetQuantity =  t.NetQuantity,
                                        ProductLookup = t.FuelType,
                                        TMWProductId = t.FuelType,
                                        SequenceNumber = 1,
                                    }
                                },
                            };
                            jsonViewModel.OrderLifts.Add(orderLift);

                        }
                    }
                    if (consolidatedInvoicePdfViewModel.Invoices != null)
                    {
                        jsonViewModel.OrderDrops = consolidatedInvoicePdfViewModel.Invoices.Select((t, i) => new OrderDrop()
                        {
                            DroppedDateTimeLocal = t.DropEndDate,
                            DroppedDateTimeLocalTimeZone = jobTimeZoneName,
                            EarliestDateTimeLocal = t.DropStartDate,
                            EarliestDateTimeLocalTimeZone = jobTimeZoneName,
                            LatestDateTimeLocal = t.DropEndDate,
                            LatestDateTimeLocalTimeZone = jobTimeZoneName,
                            ScheduledDateTimeLocal = t.DropEndDate,
                            ScheduledDateTimeLocalTimeZone = jobTimeZoneName,
                            SequenceNumber = Convert.ToInt16(i + 1),
                            SiteLookup = consolidatedInvoicePdfViewModel.InvoicePdfHeaderDetail.JobName,
                            TMWSiteId = consolidatedInvoicePdfViewModel.InvoicePdfHeaderDetail.JobName,
                            DropProducts = GetDropProducts(consolidatedInvoicePdfViewModel, t, jsonViewModel.OrderLifts, viewModel.SupplierLookup, jobTimeZoneName)
                        }).ToList();
                    }
                    TelaFuelServiceDomain telaFuelServiceDomain = new TelaFuelServiceDomain(viewModel.UserName, viewModel.Password);
                    telaFuelServiceOrderNumber = telaFuelServiceDomain.OrderAdd(jsonViewModel);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return telaFuelServiceOrderNumber;
            }
        }

        #endregion
        public void UpdateInvoicePONumber(int InvoiceHeaderId, int telaFuelServiceOrderId)
        {
            InvoiceDomain invoiceDomain = new InvoiceDomain();
            var response = invoiceDomain.UpdateInvoicePONumber(InvoiceHeaderId, telaFuelServiceOrderId);
            var invoiceHeaderDetails = Context.DataContext.InvoiceHeaderDetails.Where(t => t.Id == InvoiceHeaderId).FirstOrDefault();
            if (invoiceHeaderDetails != null && response.StatusCode == Status.Success)
            {
                var invoice = invoiceHeaderDetails.Invoices.FirstOrDefault();
                if (invoice != null)
                {
                    var invoiceBaseDomain = new InvoiceBaseDomain(this);
                    invoiceBaseDomain.CreateDtnFileGenerationWorkflowFromQueueService(invoice, invoice.Order);
                }
            }
        }

        public DateTimeOffset? GetLiftDate(ConsolidatedInvoicePdfViewModel consolidatedInvoicePdfViewModel, BolDetailViewModel bolDetailViewModel)
        {
            DateTimeOffset? LiftDateTime;
            var invoice = consolidatedInvoicePdfViewModel.Invoices.SingleOrDefault(t => t.Id == bolDetailViewModel.InvoiceId);
            if (invoice.InvoiceTypeId == (int)InvoiceType.MobileApp || invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
            {
                LiftDateTime = bolDetailViewModel.LiftDate != null ? bolDetailViewModel.LiftDate : invoice.DropStartDate;
            }
            else
            {
                LiftDateTime = invoice.DropStartDate;
            }
            return LiftDateTime;
        }

        public decimal GetGrossQuantity(ConsolidatedInvoicePdfViewModel consolidatedInvoicePdfViewModel, BolDetailViewModel bolDetailViewModel)
        {
            var invoice = consolidatedInvoicePdfViewModel.Invoices.SingleOrDefault(t => t.Id == bolDetailViewModel.InvoiceId);
            decimal grossQuantity;
            grossQuantity = bolDetailViewModel.GrossQuantity ?? invoice.DroppedGallons;
            return grossQuantity;
        }

        public decimal GetNetQuantity(ConsolidatedInvoicePdfViewModel consolidatedInvoicePdfViewModel, BolDetailViewModel bolDetailViewModel)
        {
            var invoice = consolidatedInvoicePdfViewModel.Invoices.SingleOrDefault(t => t.Id == bolDetailViewModel.InvoiceId);
            decimal netQuantity;
            netQuantity = bolDetailViewModel.NetQuantity ?? invoice.DroppedGallons;
            return netQuantity;
        }

        public List<DropProduct> GetDropProducts(ConsolidatedInvoicePdfViewModel consolidatedInvoicePdfViewModel, ConsolidatedInvoiceViewModel consolidatedInvoiceViewModel, List<OrderLift> OrderLifts, string supplierLookup, string jobTimeZoneName)
        {
            List<AssetDropViewModel> assetLists = new List<AssetDropViewModel>();
            if (consolidatedInvoicePdfViewModel.Assets != null && consolidatedInvoicePdfViewModel.Assets.Any())
            {
                assetLists = consolidatedInvoicePdfViewModel.Assets.SelectMany(t => t).ToList();
            }
            List<DropProduct> dropProducts = new List<DropProduct>();
            if (OrderLifts == null)
            {
                OrderLifts = new List<OrderLift>();
            }
            var orderLiftsByInvoiceId = OrderLifts.FindAll(t => t.InvoiceId == consolidatedInvoiceViewModel.Id);
            var assetByInvoice = assetLists.GroupBy(t => t.InvoiceId);

            //If no LiftDetails added for Drop then add lift details
            if (orderLiftsByInvoiceId == null || !orderLiftsByInvoiceId.Any())
            {
                OrderLift orderLift = new OrderLift()
                {
                    BillOfLadingNumber = consolidatedInvoicePdfViewModel.InvoicePdfHeaderDetail.DisplayInvoiceNumber,
                    LiftDateTimeLocal = consolidatedInvoiceViewModel.DropStartDate,
                    LiftDateTimeLocalTimeZone = jobTimeZoneName,
                    SupplierLookup = supplierLookup,
                    SequenceNumber = OrderLifts != null ? Convert.ToInt16(OrderLifts.Count + 1) : Convert.ToInt16(1),
                    InvoiceId = consolidatedInvoiceViewModel.Id,
                    LiftProducts = new List<LiftProduct>
                                {
                                    new LiftProduct()
                                    {
                                        GrossQuantity =  consolidatedInvoiceViewModel.GrossQuantity,
                                        NetQuantity =  consolidatedInvoiceViewModel.NetQuantity,
                                        ProductLookup = consolidatedInvoiceViewModel.FuelType,
                                        TMWProductId = consolidatedInvoiceViewModel.FuelType,
                                        SequenceNumber = 1
                                    }
                                },
                };
                if (OrderLifts == null)
                {
                    OrderLifts = new List<OrderLift>();
                }
                OrderLifts.Add(orderLift);
                orderLiftsByInvoiceId = OrderLifts.FindAll(t => t.InvoiceId == consolidatedInvoiceViewModel.Id);
            }
            //End lift details add

            if (orderLiftsByInvoiceId != null && orderLiftsByInvoiceId.Any())
            {
                foreach (var orderLift in orderLiftsByInvoiceId)
                {
                    var assets = assetByInvoice.Where(t => t.Key == orderLift.InvoiceId).FirstOrDefault();
                    if (assets != null && assets.Any())
                    {
                        var assetDroppedGallons = assets.Sum(t => t.DropGallons);
                        DropProduct DropProducts = new DropProduct()
                        {
                            GrossQuantity = assetDroppedGallons > 0 ? assetDroppedGallons : orderLift.LiftProducts[0].GrossQuantity,
                            NetQuantity = assetDroppedGallons > 0 ? assetDroppedGallons : orderLift.LiftProducts[0].NetQuantity,
                            OrderQuantity = assetDroppedGallons > 0 ? assetDroppedGallons : orderLift.LiftProducts[0].GrossQuantity,
                            ProductLookup = orderLift.LiftProducts[0].ProductLookup,
                            TMWProductId = orderLift.LiftProducts[0].ProductLookup,
                            SourceLiftSequenceNumber = orderLift.SequenceNumber,
                            TankLookup = assets.Count() > 1 ? "multi" : assetLists[0].AssetName,
                            TankNumber = assets.Count() > 1 ? "multi" : assetLists[0].AssetName,
                            TMWTankId = assets.Count() > 1 ? "multi" : assetLists[0].AssetName,
                        };
                        dropProducts.Add(DropProducts);
                    }
                    else
                    {
                        DropProduct DropProducts = new DropProduct()
                        {
                            GrossQuantity = orderLift.LiftProducts[0].GrossQuantity,
                            NetQuantity = orderLift.LiftProducts[0].NetQuantity,
                            OrderQuantity = orderLift.LiftProducts[0].GrossQuantity,
                            ProductLookup = orderLift.LiftProducts[0].ProductLookup,
                            TMWProductId = orderLift.LiftProducts[0].ProductLookup,
                            SourceLiftSequenceNumber = orderLift.SequenceNumber,
                            TankLookup = "unknown",
                            TankNumber = "unknown",
                            TMWTankId = "unknown",
                        };
                        dropProducts.Add(DropProducts);
                    }
                }
            }
            return dropProducts;
        }
        private static string SetFailedProcessMessage(string message, string jobName, string fuelType)
        {
            return $"<p class='color-maroon'>{ message} <b>Job Name: {jobName}, Fuel Type: {fuelType}</b></p>";
        }

        private List<AmpJobViewModel> SetFuelTypeId(List<AmpJobViewModel> ampJobsWithDrops, List<string> errors)
        {
            var applicationDomain = new ApplicationDomain(this);
            var fuelTYpeMapping = applicationDomain.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingAmpFuelTypes);
            Dictionary<string, int> differentMappedFuelTypes = ParseAllowedFuelTypes(fuelTYpeMapping);
            var finalList = new List<AmpJobViewModel>();
            foreach (var item in differentMappedFuelTypes)
            {
                var fuelMappedJobs = ampJobsWithDrops.Where(x => string.Equals(x.AmpProductType, item.Key, StringComparison.OrdinalIgnoreCase)).ToList();
                fuelMappedJobs.ForEach(x => x.ProductTypeId = item.Value);
                finalList.AddRange(fuelMappedJobs);
            }
            LogUnMatchedFuelTypeJobsInfo(ampJobsWithDrops, errors);
            return finalList;
        }

        private static void LogUnMatchedFuelTypeJobsInfo(List<AmpJobViewModel> ampJobsWithDrops, List<string> errors)
        {
            var unMatchedFuelTypeJobs = ampJobsWithDrops.Where(x => x.ProductTypeId == 0);
            foreach (var item in unMatchedFuelTypeJobs)
            {
                errors.Add(SetFailedProcessMessage("Could not find any order matching with", item.JobName, item.AmpProductType));
            }
        }

        private Dictionary<string, int> ParseAllowedFuelTypes(string fuelTYpeMapping)
        {
            var fuelTypeCombos = fuelTYpeMapping.Split(';');
            Dictionary<string, int> fuelMapping = new Dictionary<string, int>();
            try
            {
                foreach (var item in fuelTypeCombos)
                {
                    var fuelTypeInfo = item.Split('-');
                    fuelMapping.Add(fuelTypeInfo[0], int.Parse(fuelTypeInfo[1]));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "ParseAllowedFuelTypes", "Parsing of Amp AllowedFuelTypes failed due to wrong format", ex);
            }
            return fuelMapping;
        }

        public List<AmpJobViewModel> GetAmpJobViewModels(string inputString)
        {
            var response = new List<AmpJobViewModel>();
            var inputRecords = inputString.Split('\n');
            var inputFinalRecords = new List<string>();
            var lastRecord = string.Empty;
            for (int i = 0; i < inputRecords.Length; i++)
            {
                var currentRecord = inputRecords[i];
                if (lastRecord.StartsWith("y", StringComparison.OrdinalIgnoreCase)
                    && currentRecord.StartsWith("y", StringComparison.OrdinalIgnoreCase))
                {
                    inputFinalRecords.Remove(lastRecord);
                }
                inputFinalRecords.Add(currentRecord);
                lastRecord = currentRecord;
            }
            var singleJobRecords = new List<string>();
            foreach (var input in inputFinalRecords.Where(t => !string.IsNullOrWhiteSpace(t)))
            {
                singleJobRecords.Add(input);
                if (input.StartsWith("f", StringComparison.OrdinalIgnoreCase))
                {
                    var ampJobViewModel = new AmpJobViewModel(singleJobRecords);
                    response.Add(ampJobViewModel);
                    singleJobRecords.Clear();
                }
            }
            var uniqueJobNames = response.Select(x => x.JobName).Distinct();
            var uniqueJobs = new List<AmpJobViewModel>();
            var finalJobList = new List<AmpJobViewModel>();
            foreach (var item in uniqueJobNames)
            {
                var jobs = response.Where(x => x.JobName == item).ToList();
                var job = jobs[0];
                for (int i = 1; i < jobs.Count; i++)
                {
                    job.Drops.AddRange(jobs[i].Drops);
                }
                uniqueJobs.Add(job);
            }
            foreach (var job in uniqueJobs)
            {
                var productTypeDrops = job.Drops.GroupBy(x => x.AmpProductType).ToList();
                foreach (var x in productTypeDrops)
                {
                    var clonedJob = job.Clone();
                    clonedJob.Drops = x.ToList();
                    clonedJob.AmpProductType = x.Key;
                    finalJobList.Add(clonedJob);
                }
            }

            return finalJobList;
        }

        public int GetExternalProductId(int fuelTypeId)
        {
            int productId = 0;
            var productMapping = Context.DataContext.MstProductMappings.FirstOrDefault(t => t.ProductId == fuelTypeId);
            if (productMapping != null)
            {
                productId = productMapping.ExternalProductId;
            }
            return productId;
        }

        #region private methods

        //private List<ThirdPartyOrderViewModel> GetThirdPartyOrderViewModel(List<ThirdPartyOrderCsvViewModel> csvOrderList)
        //{
        //    var response = new List<ThirdPartyOrderViewModel>();
        //    if (csvOrderList != null && csvOrderList.Count > 0)
        //    {
        //        var originalList = csvOrderList;
        //        foreach (var item in originalList)
        //        {
        //            if (!string.IsNullOrWhiteSpace(item.CompanyName))
        //            {
        //                var customerDetails = GetCustomerDetails(item);
        //                var addressDetails = GetAddressDetails(item);
        //                var fuelDetails = GetFuelDetails(item);
        //                var fuelDeliveryDetails = GetFuelDelieryDetails(item);
        //                var pricingDetails = GetPricingDetails(item);
        //                var externalBrokerOrderDetails = GetExternalBrokerOrderDetails(item);
        //                var orderTaxes = GetOrderTaxes(item);

        //                var orderModel = new ThirdPartyOrderViewModel();
        //                orderModel.IsOtherFuelTypeTaxesGiven = orderTaxes?.Any() ?? false;

        //                bool isExists = CheckIfAlreadyExistsInTpoList(response, customerDetails, addressDetails, fuelDetails, pricingDetails, fuelDeliveryDetails, item.ExternalPONumber);
        //                if (isExists)
        //                {
        //                    var existingOrder = response.FirstOrDefault(t => t.CustomerDetails.CompanyName == item.CompanyName);
        //                    if (existingOrder != null)
        //                    {
        //                        if (!string.IsNullOrWhiteSpace(item.DeliverySchedulesDate))
        //                        {
        //                            AddDeliveryScheduleDetails(existingOrder.FuelDeliveryDetails, item);
        //                        }
        //                        if (!string.IsNullOrWhiteSpace(item.AssetName))
        //                        {
        //                            AddAssetDetails(existingOrder.Assets, item);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    orderModel.AddressDetails = addressDetails;
        //                    orderModel.CustomerDetails = customerDetails;
        //                    orderModel.FuelDeliveryDetails = fuelDeliveryDetails;
        //                    orderModel.FuelDetails = fuelDetails;
        //                    orderModel.PONumber = item.ExternalPONumber;
        //                    orderModel.PricingDetails = pricingDetails;
        //                    orderModel.TaxDetailsViewModel = orderTaxes;
        //                    orderModel.IsAssetTracked = true;

        //                    if (externalBrokerOrderDetails == null)
        //                    {
        //                        orderModel.IsBuyAndSellOrder = false;
        //                        orderModel.IsThirdPartyHardwareUsed = false;
        //                    }
        //                    else
        //                    {
        //                        orderModel.ExternalBrokeredOrder = externalBrokerOrderDetails;

        //                        if ((!string.IsNullOrWhiteSpace(item.IsThirdPartyHardwareUsed)) && item.IsThirdPartyHardwareUsed.ToLower() == "yes")
        //                            orderModel.IsThirdPartyHardwareUsed = true;
        //                        if ((!string.IsNullOrWhiteSpace(item.IsBuyAndSellOrder)) && item.IsBuyAndSellOrder.ToLower() == "yes")
        //                        {
        //                            orderModel.IsBuyAndSellOrder = true;
        //                            orderModel.PricingDetails.BrokerMarkUp = externalBrokerOrderDetails.BrokerMarkUp;
        //                            orderModel.PricingDetails.SupplierMarkUp = externalBrokerOrderDetails.SupplierMarkUp;
        //                        }
        //                    }

        //                    if (!string.IsNullOrWhiteSpace(item.DeliverySchedulesDate))
        //                    {
        //                        AddDeliveryScheduleDetails(orderModel.FuelDeliveryDetails, item);
        //                    }

        //                    if (!string.IsNullOrWhiteSpace(item.AssetName))
        //                    {
        //                        AddAssetDetails(orderModel.Assets, item);
        //                    }

        //                    if (!string.IsNullOrWhiteSpace(item.DriverFirstName) && !string.IsNullOrWhiteSpace(item.DriverLastName) && !string.IsNullOrWhiteSpace(item.DriverEmail))
        //                    {
        //                        var driverId = ExistingDriver(item);
        //                        if (driverId > 0)
        //                        {
        //                            orderModel.IsNewDriver = false;
        //                            orderModel.DriverId = driverId;
        //                        }
        //                        else
        //                        {
        //                            orderModel.IsNewDriver = true;
        //                            orderModel.DriverFirstName = item.DriverFirstName.Trim();
        //                            orderModel.DriverLastName = item.DriverLastName.Trim();
        //                            orderModel.DriverEmail = item.DriverEmail.Trim().ToLower();
        //                        }
        //                    }
        //                    response.Add(orderModel);
        //                }
        //            }
        //        }
        //    }
        //    return response;
        //}

        private TPOBrokeredOrderViewModel GetExternalBrokerOrderDetails(ThirdPartyOrderCsvViewModel item)
        {
            TPOBrokeredOrderViewModel response = null;
            if (!string.IsNullOrWhiteSpace(item.IsBuyAndSellOrder) && item.IsBuyAndSellOrder.ToLower() == "yes")
            {
                int customerId = 1;
                decimal brokerMarkUp = 0;
                decimal supplierMarkup = 0;
                var externalCustomer = Context.DataContext.ExternalBrokers.SingleOrDefault(t => t.CompanyName.Equals(item.BrokeredCustomer, StringComparison.OrdinalIgnoreCase));
                if (externalCustomer != null)
                {
                    customerId = externalCustomer.Id;
                    decimal.TryParse(item.BrokerMarkup.Replace("$", string.Empty), out brokerMarkUp);
                    decimal.TryParse(item.SupplierMarkup.Replace("$", string.Empty), out supplierMarkup);

                    response = new TPOBrokeredOrderViewModel
                    {
                        CustomerId = customerId,
                        BrokerMarkUp = brokerMarkUp,
                        SupplierMarkUp = supplierMarkup,
                    };
                }
            }

            if ((!string.IsNullOrWhiteSpace(item.IsThirdPartyHardwareUsed)) && item.IsThirdPartyHardwareUsed.ToLower() == "yes")
            {
                int invoicePrefId = 1;
                var lstInvoicePref = (InvoicePreference[])Enum.GetValues(typeof(InvoicePreference));
                var isInvoicePrefExist = lstInvoicePref.FirstOrDefault(t => t.ToString().ToLower() == item.InvoicePreference.ToLower());
                invoicePrefId = (int)isInvoicePrefExist;

                if (response == null)
                {
                    response = new TPOBrokeredOrderViewModel();
                }

                response.InvoicePreferenceId = invoicePrefId;
                response.CustomerNumber = item.CustomerNumber;
                response.ProductCode = item.ProductCode;
                response.ShipTo = item.ShipTo;
                response.Source = item.Source;
                response.VendorId = item.VendorId;
                response.BrokeredOrderFee = GetBrokeredOrderFee(item);
            }

            return response;
        }

        private bool CheckIfAlreadyExistsInTpoList(List<ThirdPartyOrderViewModel> response, TPOCustomerViewModel customerDetails, TPOAddressViewModel addressDetails, FuelDetailsViewModel fuelDetails, FuelPricingViewModel pricingDetails, FuelDeliveryDetailsViewModel fuelDeliveryDetails, string ExternalPoNumber)
        {
            if (response.Count > 0
                && response.Any(t => t.CustomerDetails.CompanyName == customerDetails.CompanyName
                && t.FuelDetails.FuelTypeId == fuelDetails.FuelTypeId
                && t.AddressDetails.JobName == addressDetails.JobName
                && t.AddressDetails.Address == addressDetails.Address && t.AddressDetails.City == addressDetails.City
                && t.AddressDetails.State == addressDetails.State && t.AddressDetails.ZipCode == addressDetails.ZipCode
                && t.PricingDetails.PricePerGallon == pricingDetails.PricePerGallon
                && t.FuelDeliveryDetails.StartDate == fuelDeliveryDetails.StartDate
                && t.FuelDetails.FuelQuantity.MaximumQuantity == fuelDetails.FuelQuantity.MaximumQuantity
                && t.PONumber == ExternalPoNumber && fuelDeliveryDetails.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries
                ))
            {
                return true;
            }

            return false;
        }

        private void AddAssetDetails(List<AssetViewModel> viewModel, ThirdPartyOrderCsvViewModel item)
        {
            decimal.TryParse(item.DeliverySchedulesQuantity, out decimal scheduleQuanity);

            var asset = new AssetViewModel()
            {
                Name = item.AssetName
            };
            asset.AssetAdditionalDetail.Class = item.CatClass;
            asset.AssetAdditionalDetail.Make = item.Make;
            asset.AssetAdditionalDetail.Model = item.Model;
            asset.AssetAdditionalDetail.Year = item.Year;
            asset.CreatedDate = DateTimeOffset.Now;
            viewModel.Add(asset);
        }
        private void AddDeliveryScheduleDetails(FuelDeliveryDetailsViewModel viewModel, ThirdPartyOrderCsvViewModel item)
        {
            decimal.TryParse(item.DeliverySchedulesQuantity, out decimal scheduleQuanity);

            viewModel.DeliverySchedules.Add(new DeliveryScheduleViewModel()
            {
                ScheduleQuantity = scheduleQuanity,
                ScheduleDate = Convert.ToDateTime(item.DeliverySchedulesDate, new CultureInfo("en-US")),
                ScheduleStartTime = item.DeliverySchedulesStartTime,
                ScheduleEndTime = item.DeliverySchedulesEndTime
            });
        }

        private FuelPricingViewModel GetPricingDetails(ThirdPartyOrderCsvViewModelNew item, List<PricingCodesViewModel> pricingCodes, int pricingSourceId, bool isSuppressPricing)
        {
            var response = new FuelPricingViewModel();
            var defaultPricing = pricingCodes.FirstOrDefault(t => t.PricingSourceId == pricingSourceId);
            response.FuelPricingDetails.PricingSourceId = defaultPricing.PricingSourceId;
            response.FuelPricingDetails.PricingCode.Id = defaultPricing.Id;
            response.FuelPricingDetails.PricingCode.Code = defaultPricing.Code;
            response.PricingTypeId = (int)PricingType.PricePerGallon;
            if (!isSuppressPricing)
            {
                if (!string.IsNullOrEmpty(item.PricingCode))
                {
                    var pricingCode = pricingCodes.Where(t => t.Code.ToLower().Equals(item.PricingCode.ToLower())).FirstOrDefault();
                    response.FuelPricingDetails.PricingCode.Id = pricingCode.Id;
                    response.FuelPricingDetails.PricingCode.Code = pricingCode.Code;
                    response.FuelPricingDetails.PricingSourceId = pricingCode.PricingSourceId;
                }

                if (item.PricingType.ToLower().Trim().Equals("market based"))
                {
                    response.PricingTypeId = (int)PricingType.RackAverage;
                    if (item.RackPricingType.Trim() == "+$")
                    {
                        response.RackAvgTypeId = (int)RackPricingType.PlusDollar;
                    }
                    if (item.RackPricingType.Trim() == "-$")
                    {
                        response.RackAvgTypeId = (int)RackPricingType.MinusDollar;
                    }
                    if (item.RackPricingType.Trim() == "+%")
                    {
                        response.RackAvgTypeId = (int)RackPricingType.PlusPercent;
                    }
                    if (item.RackPricingType.Trim() == "-%")
                    {
                        response.RackAvgTypeId = (int)RackPricingType.MinusPercent;
                    }

                    response.RackPrice = Convert.ToDecimal(item.Price);
                    if (!string.IsNullOrWhiteSpace(item.CityRackTerminal))
                    {
                        GetCityRackTerminal(item, response);
                    }
                }
                if (!string.IsNullOrWhiteSpace(item.ApprovedTerminal))
                {
                    GetApprovedTerminal(item, response);
                }
                if (item.PricingType.ToLower().Trim().Equals("fuelcost"))
                {
                    response.PricingTypeId = (int)PricingType.Suppliercost;

                    response.SupplierCostMarkupValue = Convert.ToDecimal(item.Price);
                    if (item.RackPricingType.Trim() == "+$")
                    {
                        response.SupplierCostMarkupTypeId = (int)RackPricingType.PlusDollar;
                    }
                    if (item.RackPricingType.Trim() == "-$")
                    {
                        response.SupplierCostMarkupTypeId = (int)RackPricingType.MinusDollar;
                    }
                    if (item.RackPricingType.Trim() == "+%")
                    {
                        response.SupplierCostMarkupTypeId = (int)RackPricingType.PlusPercent;
                    }
                    if (item.RackPricingType.Trim() == "-%")
                    {
                        response.SupplierCostMarkupTypeId = (int)RackPricingType.MinusPercent;
                    }
                }
                if (item.PricingType.ToLower().Trim().Equals("fixed"))
                {
                    response.PricingTypeId = (int)PricingType.PricePerGallon;
                    response.PricePerGallon = Convert.ToDecimal(item.Price);
                }
            }
            return response;
        }

        private int GetPricingSourceId(string pricingCode, List<PricingCodesViewModel> pricingCodes)
        {
            int pricingsourceId = (int)PricingSource.Axxis;
            if (!string.IsNullOrEmpty(pricingCode))
            {
                var dpricingCode = pricingCodes.Where(t => t.Code.ToLower().Equals(pricingCode.ToLower())).FirstOrDefault();
                pricingsourceId = dpricingCode.PricingSourceId;
            }
            return pricingsourceId;
        }
        private void GetCityRackTerminal(ThirdPartyOrderCsvViewModelNew item, FuelPricingViewModel response)
        {
            if (!string.IsNullOrWhiteSpace(item.CityRackTerminal))
            {
                var rackWithState = item.CityRackTerminal.Split(' ');
                var cityRack = new HelperDomain(this).GetExternalTerminal(item.CityRackTerminal.Replace(rackWithState.Last(), string.Empty), rackWithState.Last());
                if (cityRack != null)
                {
                    response.CityGroupTerminalId = cityRack.Id;
                    response.CityGroupTerminalName = cityRack.Name;
                }
            }
        }

        private void FreightPricingMethodAutoResponse(ThirdPartyOrderCsvViewModelNew csvOrder, ThirdPartyOrderViewModel response, UserContext context)
        {
            response.OrderAdditionalDetailsViewModel.FreightPricingMethod = FreightPricingMethod.Auto;

            List<SourceRegion> sourceRegions = new List<SourceRegion>();
            List<int> selectedIds;
            List<string> selectedNames = new List<string>();

            if (!string.IsNullOrWhiteSpace(csvOrder.SourceRegionNames))
            {
                selectedNames = csvOrder.SourceRegionNames.Trim().Split('|').ToList();
                if (!selectedNames.First().Trim().ToLower().Contains("all"))
                {

                    sourceRegions = (from t in Context.DataContext.SourceRegions
                                     join i in selectedNames
                                     on t.Name.ToUpper() equals i.ToUpper()
                                     where t.CompanyId == context.CompanyId && t.IsActive
                                     select t).ToList();
                }
                else
                {
                    sourceRegions = Context.DataContext.SourceRegions.Where(t => t.CompanyId == context.CompanyId && t.IsActive).ToList();
                }

                if (sourceRegions != null && sourceRegions.Count > 0)
                {
                    response.SourceRegion.SelectedSourceRegions = sourceRegions.Select(t1 => t1.Id).Distinct().ToList();
                }
            }

            if (!string.IsNullOrWhiteSpace(csvOrder.TerminalNames) && sourceRegions != null && sourceRegions.Count > 0)
            {

                selectedNames = csvOrder.TerminalNames.Trim().Split('|').ToList();

                var items = sourceRegions.SelectMany(t1 => t1.SourceRegionPickupLocation).Select(t2 => t2.MstExternalTerminal).Where(t1 => t1 != null).ToList();

                if (items != null && items.Count > 0)
                {
                    if (!selectedNames.First().Trim().ToLower().Contains("all"))
                    {
                        selectedIds = items.Where(t1 => t1 != null && selectedNames.Contains(t1.Name)).Select(t1 => t1.Id).Distinct().ToList();
                    }
                    else
                    {
                        selectedIds = items.Where(t1 => t1 != null).Select(t2 => t2.Id).Distinct().ToList();
                    }
                    response.SourceRegion.SelectedTerminals = selectedIds;
                }
            }

            if (!string.IsNullOrWhiteSpace(csvOrder.BulkPlantNames) && sourceRegions != null && sourceRegions.Count > 0)
            {

                var lst = csvOrder.BulkPlantNames.Trim().Split('|').ToList();
                var items = sourceRegions.SelectMany(t1 => t1.SourceRegionPickupLocation).Select(t2 => t2.BulkPlantLocation).Where(t1 => t1 != null).ToList();


                if (items != null && items.Count > 0)
                {
                    if (!lst.First().Trim().ToLower().Contains("all"))
                    {
                        selectedIds = items.Where(t3 => t3 != null && lst.Contains(t3.Name)).Select(t3 => t3.Id).Distinct().ToList();
                    }
                    else
                    {
                        selectedIds = items.Where(t1 => t1 != null).Select(t1 => t1.Id).Distinct().ToList();
                    }
                    response.SourceRegion.SelectedBulkPlants = selectedIds;
                }
            }
            response.SourceRegion.ApprovedTerminalId = 0;

            if (!string.IsNullOrWhiteSpace(csvOrder.ApprovedSourceTerminal) && sourceRegions != null && sourceRegions.Count > 0)
            {
                var items = sourceRegions.SelectMany(t1 => t1.SourceRegionPickupLocation).Select(t2 => t2.MstExternalTerminal).Where(t1 => t1 != null).ToList();

                if (items != null && items.Count > 0)
                {
                    response.SourceRegion.ApprovedTerminalId = items.Where(t3 => t3 != null && t3.Name.Contains(csvOrder.ApprovedSourceTerminal)).Select(t3 => t3.Id).Distinct().FirstOrDefault();
                }
            }
            response.SourceRegion.ApprovedBulkPlantId = null;

            if (!string.IsNullOrWhiteSpace(csvOrder.ApprovedBulkPlant) && sourceRegions != null && sourceRegions.Count > 0)
            {
                var items = sourceRegions.SelectMany(t1 => t1.SourceRegionPickupLocation).Select(t2 => t2.BulkPlantLocation).Where(t1 => t1 != null).ToList();
                if (items != null)
                {
                    response.SourceRegion.ApprovedBulkPlantId = items.Where(t3 => t3 != null && t3.Name.Contains(csvOrder.ApprovedBulkPlant)).Select(t3 => t3.Id).Distinct().FirstOrDefault();
                }

            }
        }

        private void GetApprovedTerminal(ThirdPartyOrderCsvViewModelNew item, FuelPricingViewModel response)
        {
            if (!string.IsNullOrWhiteSpace(item.ApprovedTerminal))
            {
                var terminal = Context.DataContext.MstExternalTerminals.Where(t => t.Name.ToLower().Equals(item.ApprovedTerminal.ToLower()) && t.PricingSourceId == response.FuelPricingDetails.PricingSourceId)
                                                                       .Select(t => new { t.Id, t.Name }).FirstOrDefault();
                if (terminal != null)
                {
                    response.TerminalId = terminal.Id;
                    response.TerminalName = terminal.Name;
                }
            }
        }

        private static void GetPricingFormat(ThirdPartyOrderCsvViewModel item, FuelPricingViewModel response)
        {
            if (item.PricePerGallon.Trim().Contains("+") && item.PricePerGallon.Trim().Contains("$"))
            {
                response.RackAvgTypeId = (int)RackPricingType.PlusDollar;
            }
            else if (item.PricePerGallon.Trim().Contains("-") && item.PricePerGallon.Trim().Contains("$"))
            {
                response.RackAvgTypeId = (int)RackPricingType.MinusDollar;
            }
            else if (item.PricePerGallon.Trim().Contains("-") && item.PricePerGallon.Trim().Contains("%"))
            {
                response.RackAvgTypeId = (int)RackPricingType.MinusPercent;
            }
            else if (item.PricePerGallon.Trim().Contains("+") && item.PricePerGallon.Trim().Contains("%"))
            {
                response.RackAvgTypeId = (int)RackPricingType.PlusPercent;
            }

            var ppg = item.PricePerGallon.ToLower().Replace("rackavg", string.Empty)
                            .Replace("racklow", string.Empty).Replace("rackhigh", string.Empty)
                            .Replace("fuelcost", string.Empty)
                            .Replace("+", string.Empty).Replace("-", string.Empty)
                            .Replace("$", string.Empty).Replace("%", string.Empty);

            decimal.TryParse(ppg, out decimal pricepergallon);
            response.PricePerGallon = pricepergallon;
        }


        private FuelDetailsViewModel GetFuelDetails(ThirdPartyOrderCsvViewModelNew item, int pricingSourceId, UserContext context)
        {
            var response = new FuelDetailsViewModel();
            if (!string.IsNullOrWhiteSpace(item.FuelType))
            {
                //Exclude older product mapping 'Regular Gas','Premium Gas','Midgrade Gas'
                var olderProductIds = new List<int> { 14, 15, 16 };
                var tfxfuelType = Context.DataContext.MstTfxProducts.Where(t => t.Name.ToLower().Equals(item.FuelType.ToLower()) && !olderProductIds.Contains(t.MstProductType.Id) && t.IsActive)
                                                   .Select(t1 => new { t1.Name, t1.Id, t1.ProductDisplayGroupId, t1.MstProducts.FirstOrDefault().PricingSourceId, productTypeName = t1.MstProductType.Name }
                                                      ).FirstOrDefault();
                if (tfxfuelType != null)
                {
                    response.FuelType = tfxfuelType.Name;
                    response.FuelTypeId = tfxfuelType.Id;//tfxproductId
                    response.FuelDisplayGroupId = tfxfuelType.ProductDisplayGroupId;
                    if (tfxfuelType.productTypeName == ApplicationConstants.Marine)
                    {
                        response.IsMarineLocation = true;
                    }
                    pricingSourceId = tfxfuelType.PricingSourceId;
                }
                else
                {
                    var additiveFuelType = Context.DataContext.MstProducts.Where(t => t.IsActive && t.ProductDisplayGroupId == (int)ProductDisplayGroups.AdditiveFuelType
                                                     && t.CompanyId == context.CompanyId && t.Name.Trim().ToLower().Equals(item.FuelType.Trim().ToLower()))
                                                     .Select(t1 => new { t1.Name, t1.Id, t1.ProductDisplayGroupId, t1.ProductTypeId, t1.PricingSourceId, productTypeName = t1.MstProductType.Name }
                                                        ).FirstOrDefault();
                    if (additiveFuelType != null)
                    {
                        response.FuelType = additiveFuelType.Name;
                        response.FuelTypeId = additiveFuelType.Id;//mstproductId
                        response.FuelDisplayGroupId = additiveFuelType.ProductDisplayGroupId;
                        response.ProductTypeId = additiveFuelType.ProductTypeId;
                        pricingSourceId = additiveFuelType.PricingSourceId;
                    }
                }
            }
            else
            {
                response.NonStandardFuelName = item.NonStandardFuelType;
                response.NonStandardFuelDescription = item.NonStandardFuelDescription;
                response.FuelDisplayGroupId = (int)ProductDisplayGroups.OtherFuelType;
            }

            //decimal.TryParse(item.MaxQuantity, out decimal quantity);
            //response.FuelQuantity.Quantity = quantity;
            //response.FuelQuantity.MaximumQuantity = quantity;
            //if (!string.IsNullOrWhiteSpace(item.MinQuantity) && Regex.IsMatch(item.MinQuantity, @"^[0-9]*$"))
            //{
            //    decimal.TryParse(item.MinQuantity, out decimal minQuantity);
            //    response.FuelQuantity.MinimumQuantity = minQuantity;
            //}
            if (item.QuantityType.ToLower().Trim().Equals(QuantityType.SpecificAmount.GetDisplayName().ToLower()))
            {
                decimal.TryParse(item.Quantity, out decimal quantity);
                response.FuelQuantity.Quantity = quantity;
                response.FuelQuantity.MaximumQuantity = quantity.GetPreciseValue(6);
                response.FuelQuantity.QuantityTypeId = (int)QuantityType.SpecificAmount;
            }
            if (item.QuantityType.ToLower().Trim().Equals(QuantityType.Range.GetDisplayName().ToLower()))
            {
                decimal.TryParse(item.MinimumQuantity, out decimal minQuantity);
                decimal.TryParse(item.MaximumQuantity, out decimal maxQuantity);
                response.FuelQuantity.MinimumQuantity = minQuantity.GetPreciseValue(6);
                response.FuelQuantity.MaximumQuantity = maxQuantity.GetPreciseValue(6);
                response.FuelQuantity.QuantityTypeId = (int)QuantityType.Range;
            }
            if (item.QuantityType.ToLower().Trim().Equals(QuantityType.NotSpecified.GetDisplayName().ToLower()))
            {
                response.FuelQuantity.MaximumQuantity = ApplicationConstants.QuantityNotSpecified;
                response.FuelQuantity.QuantityTypeId = (int)QuantityType.NotSpecified;
            }
            //  response.FuelQuantity.UoM = item.UOM.ToLower().Trim().Equals(UoM.Gallons.GetDisplayName().ToLower()) ? UoM.Gallons : UoM.Litres;
            if (item.UOM.ToLower().Trim().Equals(UoM.Gallons.GetDisplayName().ToLower()))
            {
                response.FuelQuantity.UoM = UoM.Gallons;
            }
            if (item.UOM.ToLower().Trim().Equals(UoM.Litres.GetDisplayName().ToLower()))
            {
                response.FuelQuantity.UoM = UoM.Litres;
            }
            if (item.UOM.ToLower().Trim().Equals(UoM.Barrels.GetDisplayName().ToLower()))
            {
                response.FuelQuantity.UoM = UoM.Barrels;
            }
            if (item.UOM.ToLower().Trim().Equals(UoM.MetricTons.GetDisplayName().ToLower()))
            {
                response.FuelQuantity.UoM = UoM.MetricTons;
            }
            response.FuelQuantity.QuantityIndicatorTypes = item.QuantityIndicatorType.ToLower().Trim().Equals(QuantityIndicatorTypes.Net.GetDisplayName().ToLower()) ? QuantityIndicatorTypes.Net : QuantityIndicatorTypes.Gross;
            response.FuelPricing.FuelPricingDetails.TruckLoadTypes = item.TruckLoadType.ToLower().Trim().Equals("ftl") ? TruckLoadTypes.FullTruckLoad : TruckLoadTypes.LessTruckLoad;
            response.FuelPricing.FuelPricingDetails.FreightOnBoardTypes = item.FreightOnBoardType.ToLower().Trim().Equals("terminal") ? FreightOnBoardTypes.Terminal : FreightOnBoardTypes.Destination;
            response.FuelPricing.FuelPricingDetails.PricingSourceId = pricingSourceId;

            return response;
        }

        private FuelDeliveryDetailsViewModel GetFuelDelieryDetails(ThirdPartyOrderCsvViewModelNew item)
        {
            var response = new FuelDeliveryDetailsViewModel
            {
                DeliveryTypeId = item.DeliveryType.ToLower().Equals("single") ? (int)DeliveryType.OneTimeDelivery : (int)DeliveryType.MultipleDeliveries,
                StartDate = Convert.ToDateTime(item.DeliveryStartDate, new CultureInfo("en-US")),
                StartTime = item.DeliveryStartTime,
                EndTime = item.DeliveryEndTime
            };
            if (!string.IsNullOrWhiteSpace(item.DeliveryEndDate))
            {
                response.EndDate = Convert.ToDateTime(item.DeliveryEndDate, new CultureInfo("en-US"));
            }
            if (item.DeliveryType.ToLower().Equals("single") && !string.IsNullOrWhiteSpace(item.DeliveryEndDate))
            {
                DateTime dt;
                if (DateTime.TryParse(item.DeliveryEndDate, out dt))
                {
                    response.EndDate = dt;
                }
            }
            response.FuelFees.FuelRequestFees = GetFuelRequestFeeNew(item);
            response.TruckLoadTypes = item.TruckLoadType.ToLower().Trim().Equals("ftl") ? TruckLoadTypes.FullTruckLoad : TruckLoadTypes.LessTruckLoad;
            if (!string.IsNullOrWhiteSpace(item.WBSNumber))
            {
                response.CustomAttributeViewModel.WBSNumber = item.WBSNumber;

            }
            response.PaymentMethods = GetPaymentMethods(item);
            response.OrderEnforcementId = GetOrderEnforcementId(item);

            return response;
        }

        private TPOCustomerViewModel GetCustomerDetails(ThirdPartyOrderCsvViewModelNew item)
        {
            return new TPOCustomerViewModel()
            {
                CompanyName = item.CompanyName,
                Email = item.Email,
                Name = item.ContactPerson,
                PhoneNumber = item.MobileNumber,
                IsNewCompany = false
            };
        }

        private TPOAddressViewModel GetAddressDetails(ThirdPartyOrderCsvViewModelNew item)
        {
            try
            {
                //var state = Context.DataContext.MstStates.SingleOrDefault(t => t.Code.Equals(item.LocationState, StringComparison.OrdinalIgnoreCase));
                var contry = Context.DataContext.MstCountries.SingleOrDefault(t => t.Code.Equals(item.Country, StringComparison.OrdinalIgnoreCase));

                var contryModel = contry.ToViewModel();

                var response = new TPOAddressViewModel()
                {
                    JobName = item.LocationName,
                    DisplayJobID = item.DisplayJobID,
                    Address = item.Address,
                    //City = item.LocationCity,
                    //ZipCode = item.ZipCode,
                    //State = stateModel,
                    Country = contryModel,
                    OnsiteContactEmail = item.OnsiteContactEmail,
                    OnsiteContactName = item.OnsiteContactName,
                    OnsiteContactPhone = item.OnsiteContactPhoneNumber,
                    IsRetailJob = (string.IsNullOrWhiteSpace(item.RetailLocation) || item.RetailLocation.ToLower().Equals("no")) ? false : true,
                    IsAutomateDeliveryRequest = !string.IsNullOrWhiteSpace(item.AutomateDeliveryRequest) && item.AutomateDeliveryRequest.ToLower().Equals("yes")
                };

                if (response.IsOnsiteContactAvailable())
                {
                    response.IsNewContactPerson = true;
                }
                bool IsGeocodeUsed = item.GeoCoded.ToLower().Equals("no") ? false : true;
                response.Country.UoM = item.UOM.ToLower().Trim().Equals(UoM.Gallons.GetDisplayName().ToLower()) ? UoM.Gallons : UoM.Litres;
                if (IsGeocodeUsed) //fetch address based on lat long
                {
                    var geoAddress = GoogleApiDomain.GetAddress(Convert.ToDouble(item.Latitude), Convert.ToDouble(item.Longitude));
                    if (geoAddress != null)
                    {
                        response.Address = string.IsNullOrWhiteSpace(item.Address) ? geoAddress.Address : item.Address;
                        response.City = string.IsNullOrWhiteSpace(item.LocationCity) ? geoAddress.City : item.LocationCity;
                        response.Latitude = Convert.ToDecimal(item.Latitude);
                        response.Longitude = Convert.ToDecimal(item.Longitude);
                        response.CountyName = string.IsNullOrWhiteSpace(item.CountyName) ? geoAddress.CountyName : item.CountyName;
                        response.ZipCode = string.IsNullOrWhiteSpace(item.ZipCode) ? geoAddress.ZipCode : item.ZipCode;
                        var state = Context.DataContext.MstStates.SingleOrDefault(t => t.CountryId == contryModel.Id
                                                                                && (string.IsNullOrEmpty(geoAddress.CountryGroupCode) || (t.CountryGroupId.HasValue && t.MstCountryAsGroup.Code.Equals(geoAddress.CountryGroupCode, StringComparison.OrdinalIgnoreCase)))
                                                                                && t.Code.Equals(geoAddress.StateCode, StringComparison.OrdinalIgnoreCase));
                        if (state == null && contryModel.Id == (int)Country.CAR)
                        {
                            state = Context.DataContext.MstStates.SingleOrDefault(t => t.CountryId == contryModel.Id
                                                                              && t.Name.Equals(geoAddress.StateName, StringComparison.OrdinalIgnoreCase));
                        }

                        if (state == null)
                        {
                            throw new TfxException("Unable to find state of given Latitude and Longitude.");
                        }

                        response.State = state.ToViewModel();
                    }
                }
                else //fetch lat long based on address and zip 
                {
                    if (contryModel.Id == (int)Country.CAR)
                    {
                        response.City = item.LocationCity;
                        response.ZipCode = item.ZipCode;
                        response.CountyName = item.CountyName;
                        // state name must exists in  mststate table
                        var state = Context.DataContext.MstStates.SingleOrDefault(t => t.CountryId == contryModel.Id
                                                                                    && t.Name.Equals(item.LocationState, StringComparison.OrdinalIgnoreCase));
                        if (state == null)
                        {
                            throw new TfxException("Unable to find state of given Country.");
                        }
                        response.State = state.ToViewModel();
                    }
                    else
                    {
                        var geoCodes = GoogleApiDomain.GetGeocode(item.ZipCode);
                        if (geoCodes != null)
                        {
                            //response.Address = item.Address;
                            response.City = item.LocationCity;
                            response.ZipCode = item.ZipCode;
                            response.CountyName = string.IsNullOrWhiteSpace(item.CountyName) ? geoCodes.CountyName : item.CountyName;
                            response.Latitude = string.IsNullOrWhiteSpace(item.Latitude) ? Convert.ToDecimal(geoCodes.Latitude) : Convert.ToDecimal(item.Latitude);
                            response.Longitude = string.IsNullOrWhiteSpace(item.Longitude) ? Convert.ToDecimal(geoCodes.Longitude) : Convert.ToDecimal(item.Longitude);
                            var state = Context.DataContext.MstStates.SingleOrDefault(t => t.CountryId == contryModel.Id
                                                                                    && (string.IsNullOrEmpty(geoCodes.CountryGroupCode) || (t.CountryGroupId.HasValue && t.MstCountryAsGroup.Code.Equals(geoCodes.CountryGroupCode, StringComparison.OrdinalIgnoreCase)))
                                                                                    && t.Code.Equals(geoCodes.StateCode, StringComparison.OrdinalIgnoreCase));
                            if (state == null)
                            {
                                throw new TfxException("Unable to find state of given ZipCode.");
                            }

                            response.State = state.ToViewModel();
                        }
                    }

                }
                response.TimeZoneName = GoogleApiDomain.GetTimeZone(response.Latitude, response.Longitude);
                response.InventoryDataCaptureType = GetInventoryDataCaptureType(item);
                response.LocationManagedType = GetLocationManagedType(item);
                response.IsProFormaPoEnabled =
                    (!string.IsNullOrWhiteSpace(item.EnableAfterTheFactPO) && item.EnableAfterTheFactPO.ToLower().Trim()
                    .Equals("yes")) ? true : false;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public OfferAddressViewModel GetTPOAddressDetails(int stateId, string address, string city, string zipCode)
        {
            var state = Context.DataContext.MstStates.SingleOrDefault(t => t.Id == stateId);
            var contry = Context.DataContext.MstCountries.SingleOrDefault(t => t.Id == state.CountryId);
            var stateModel = state.ToViewModel();
            var contryModel = contry.ToViewModel();

            return new OfferAddressViewModel()
            {
                Address = address,
                City = city,
                ZipCode = zipCode,
                State = stateModel,
                Country = contryModel,
                IsGeocodeUsed = false
            };
        }

        private int ExistingDriver(ThirdPartyOrderCsvViewModel item)
        {
            var driverId = 0;
            var driver = Context.DataContext.Users.Where(t => t.Email == item.DriverEmail && t.FirstName == item.DriverFirstName && t.LastName == item.DriverLastName).FirstOrDefault();
            if (driver != null)
            {
                driverId = driver.Id;
            }
            return driverId;
        }


        private QueueMessageViewModel GetEnqueueMessageRequestViewModel(UserContext userContext, string blobStoragePath)
        {
            var jsonViewModel = new ThirdPartyBulkUploadQueueMsg();
            jsonViewModel.FileLineNumberToStart = 0;
            jsonViewModel.SupplierId = userContext.Id;
            jsonViewModel.SupplierCompanyId = userContext.CompanyId;
            jsonViewModel.FileUploadPath = blobStoragePath;

            string json = JsonConvert.SerializeObject(jsonViewModel);

            return new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = QueueProcessType.ThirdPartyOrderBulkUpload,
                JsonMessage = json
            };
        }

        private string GenerateFileName(int userId)
        {
            return string.Concat(values: Constants.OrderBulk + Resource.lblSingleHyphen + userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + ".csv");
        }

        private bool IsAssetRequiredDataMissing(ThirdPartyOrderCsvViewModel record)
        {
            if (string.IsNullOrWhiteSpace(record.AssetName) && (!string.IsNullOrWhiteSpace(record.CatClass) || !string.IsNullOrWhiteSpace(record.Make) || !string.IsNullOrWhiteSpace(record.Model) || !string.IsNullOrWhiteSpace(record.Year)))
            {
                return true;
            }

            return false;
        }

        private bool IsDeliveryScheduleDataMissing(ThirdPartyOrderCsvViewModel record)
        {
            if (string.IsNullOrWhiteSpace(record.DeliverySchedulesDate)
                && string.IsNullOrWhiteSpace(record.DeliverySchedulesStartTime)
                && string.IsNullOrWhiteSpace(record.DeliverySchedulesEndTime)
                && string.IsNullOrWhiteSpace(record.DeliverySchedulesQuantity))
            {
                return false;
            }
            if (!string.IsNullOrWhiteSpace(record.DeliverySchedulesDate)
                && !string.IsNullOrWhiteSpace(record.DeliverySchedulesStartTime)
                && !string.IsNullOrWhiteSpace(record.DeliverySchedulesEndTime)
                && !string.IsNullOrWhiteSpace(record.DeliverySchedulesQuantity))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool IsRequiredFieldMissing(ThirdPartyOrderCsvViewModel record) => string.IsNullOrWhiteSpace(record.CompanyName)
            || string.IsNullOrWhiteSpace(record.ContactPersonName) || string.IsNullOrWhiteSpace(record.ContactPersonEmail)
            || string.IsNullOrWhiteSpace(record.ConactPersonPhone) || string.IsNullOrWhiteSpace(record.JobName)
            || string.IsNullOrWhiteSpace(record.Address) || string.IsNullOrWhiteSpace(record.City)
            || string.IsNullOrWhiteSpace(record.State) || string.IsNullOrWhiteSpace(record.Zip)
            || (string.IsNullOrWhiteSpace(record.StandardFuelType) && string.IsNullOrWhiteSpace(record.NonStandardFuelType))
            || string.IsNullOrWhiteSpace(record.MaxQuantity) || string.IsNullOrWhiteSpace(record.PricePerGallon)
            || string.IsNullOrWhiteSpace(record.DeliveryDate) || string.IsNullOrWhiteSpace(record.DeliveryStartTime)
            || string.IsNullOrWhiteSpace(record.DeliveryEndTime) || string.IsNullOrWhiteSpace(record.DeliveryType);


        private List<User> CreateUsersForThridPartyOrderAsync(ThirdPartyOrderViewModel viewModel, UserContext userContext, string RandomPassword)
        {
            var response = viewModel.CustomerDetails;
            response.StatusCode = Status.Failed;

            var users = new List<User>();
            viewModel.CustomerDetails.ContactPersons.Add(new ContactPersonViewModel()
            {
                Name = viewModel.CustomerDetails.Name,
                Email = viewModel.CustomerDetails.Email,
                PhoneNumber = viewModel.CustomerDetails.PhoneNumber
            });
            if (!string.IsNullOrEmpty(viewModel.CustomerDetails.Email))
            {
                foreach (var customer in viewModel.CustomerDetails.ContactPersons)
                {
                    var existingUser = Context.DataContext.Users.FirstOrDefault(t => t.Email.ToLower().Equals(customer.Email.ToLower().Trim()));
                    if (existingUser == null)
                    {
                        var salt = CryptoHelperMethods.GenerateSalt();
                        var name = customer.Name.Trim().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                        switch (name.Length)
                        {
                            case 1:
                                viewModel.CustomerDetails.FirstName = name[0];
                                viewModel.CustomerDetails.LastName = name[0].Substring(0, 1);
                                break;
                            default:
                                viewModel.CustomerDetails.FirstName = name[0];
                                viewModel.CustomerDetails.LastName = name[1];
                                break;
                        }

                        User user = new User
                        {
                            FirstName = viewModel.CustomerDetails.FirstName,
                            LastName = viewModel.CustomerDetails.LastName,
                            UserName = customer.Email.Trim().ToLower(), //viewModel.CustomerDetails.Email.Trim().ToLower(),
                            Email = customer.Email.Trim().ToLower(),//viewModel.CustomerDetails.Email.Trim().ToLower(),
                            IsEmailConfirmed = false,
                            PhoneNumber = customer.PhoneNumber,
                            IsPhoneNumberConfirmed = ContextFactory.Current.GetDomain<NotificationDomain>().IsPhoneNumberValid(customer.PhoneNumber),
                            IsTwoFactorEnabled = false,
                            AccessFailedCount = 0,
                            IsLockoutEnabled = true,
                            LockoutEndDateUtc = null,
                            PasswordHash = CryptoHelperMethods.GenerateHash(RandomPassword, salt),
                            SecurityStamp = salt,
                            FingerPrint = CryptoHelperMethods.GenerateHash(customer.Email, CryptoHelperMethods.GenerateSalt()),
                            IsOnboardingComplete = false,
                            IsActive = false,
                            CreatedBy = userContext.Id,
                            CreatedDate = DateTimeOffset.Now,
                            UpdatedBy = userContext.Id,
                            UpdatedDate = DateTimeOffset.Now,
                            OnboardedTypeId = (int)OnboardedType.ThirdPartyOrderOnboarded
                        };
                        users.Add(user);
                    }
                    else
                    {
                        users.Add(existingUser);
                    }
                }
            }

            return users;
        }

        private async Task<User> CreateUserForThridPartyOrderAsync(ContactPersonViewModel viewModel, bool isInvitationEnabled, bool isFtl, UserContext userContext, string RandomPassword, string TempPassword)
        {
            var existingUser = Context.DataContext.Users.FirstOrDefault(t => t.Email.ToLower().Equals(viewModel.Email.ToLower().Trim()));
            if (existingUser == null)
            {
                var salt = CryptoHelperMethods.GenerateSalt();
                var name = viewModel.Name.Trim().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                string firstName = string.Empty;
                string lastName = string.Empty;
                switch (name.Length)
                {
                    case 1:
                        firstName = name[0];
                        lastName = name[0].Substring(0, 1);
                        break;
                    default:
                        firstName = name[0];
                        lastName = name[1];
                        break;
                }

                User user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = viewModel.Email.Trim().ToLower(),
                    Email = viewModel.Email.Trim().ToLower(),
                    IsEmailConfirmed = false,
                    PhoneNumber = viewModel.PhoneNumber,
                    IsPhoneNumberConfirmed = ContextFactory.Current.GetDomain<NotificationDomain>().IsPhoneNumberValid(viewModel.PhoneNumber),
                    IsTwoFactorEnabled = false,
                    AccessFailedCount = 0,
                    IsLockoutEnabled = true,
                    LockoutEndDateUtc = null,
                    CompanyId = viewModel.CompanyId,
                    PasswordHash = CryptoHelperMethods.GenerateHash(RandomPassword, salt),
                    SecurityStamp = salt,
                    FingerPrint = CryptoHelperMethods.GenerateHash(viewModel.Email, CryptoHelperMethods.GenerateSalt()),
                    IsOnboardingComplete = false,
                    IsActive = false,
                    CreatedBy = userContext.Id,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedBy = userContext.Id,
                    UpdatedDate = DateTimeOffset.Now,
                    OnboardedTypeId = (int)OnboardedType.ThirdPartyOrderOnboarded
                };

                user.MstRoles.Add(Context.DataContext.MstRoles.Single(t => t.Id == (int)UserRoles.Admin));
                Context.DataContext.Users.Add(user);
                await Context.CommitAsync();

                string eventTypeId = string.Empty;
                if (isInvitationEnabled && !user.IsEmailConfirmed && !user.IsOnboardingComplete)
                {
                    var authDomain = new AuthenticationDomain(this);

                    var userRoleIds = user.MstRoles.Select(t => t.Id).ToList();
                    var defaultEnabled = authDomain.GetDefaultEnabledNotifications(userRoleIds, (int)CompanyType.Buyer);
                    if (defaultEnabled != null && defaultEnabled.Any())
                    {
                        eventTypeId = string.Join(",", defaultEnabled);
                    }
                }
                if (isInvitationEnabled)
                {
                    eventTypeId = eventTypeId + "," + ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingNotifyDeliveries);
                }
                if (isInvitationEnabled)
                {
                    eventTypeId = eventTypeId + "," + ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingNotifySchedules);
                }
                if (isFtl)
                {
                    eventTypeId = eventTypeId + "," + ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingNotificationPreferencesForFtlOrder);
                }
                var eventTypeIds = eventTypeId.TrimStart(',').Split(',').Distinct().ToList();
                var sqlQuery = Context.DataContext.MstCompanyUserRoleXEventTypes.Where(t => t.NotificationType != (int)NotificationType.Nothing
                                && t.RoleId == (int)UserRoles.Admin && t.CompanyTypeId == (int)CompanyType.Buyer)
                                .Select(t => new { t.EventTypeId });

                var notificationSettings = new List<UserXNotificationSetting>();
                foreach (var item in sqlQuery)
                {
                    bool isEmail = false;
                    if (eventTypeIds.Contains(item.EventTypeId.ToString()))
                    {
                        isEmail = true;
                    }
                    var setting = new AuthenticationDomain(this).GetNotificationSetting(user.Id, item.EventTypeId, isEmail);
                    notificationSettings.Add(setting);
                }
                user.UserXNotificationSettings = notificationSettings.Distinct().ToList();
                if (isInvitationEnabled && !user.IsEmailConfirmed && !user.IsOnboardingComplete)
                {
                    await new NotificationDomain(this).AddNotificationEventAsync(EventType.TPOUserInvitedForEULAAcceptance, user.Id, userContext.Id, null, TempPassword);
                    // send buyer onboard invitation email
                    await SaveInvitationSendDetailsAsync(user.Email, user.Id, userContext.Id, user.FirstName, user.LastName);
                    // update buyer user for onboarding
                    user.IsEmailConfirmed = true;
                    user.IsOnboardingComplete = true;
                    user.IsActive = true;
                }
                return user;
            }
            else
            {
                return existingUser;
            }
        }

        private User CreateUserForThridPartyOrderAsync(ThirdPartyOrderViewModel viewModel, UserContext userContext, string RandomPassword)
        {
            var existingUser = Context.DataContext.Users.FirstOrDefault(t => t.Email.ToLower().Equals(viewModel.CustomerDetails.Email.ToLower().Trim()));
            if (existingUser == null)
            {
                var salt = CryptoHelperMethods.GenerateSalt();
                var name = viewModel.CustomerDetails.Name.Trim().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                switch (name.Length)
                {
                    case 1:
                        viewModel.CustomerDetails.FirstName = name[0];
                        viewModel.CustomerDetails.LastName = name[0].Substring(0, 1);
                        break;
                    default:
                        viewModel.CustomerDetails.FirstName = name[0];
                        viewModel.CustomerDetails.LastName = name[1];
                        break;
                }

                User user = new User
                {
                    FirstName = viewModel.CustomerDetails.FirstName,
                    LastName = viewModel.CustomerDetails.LastName,
                    UserName = viewModel.CustomerDetails.Email.Trim().ToLower(),
                    Email = viewModel.CustomerDetails.Email.Trim().ToLower(),
                    IsEmailConfirmed = false,
                    PhoneNumber = viewModel.CustomerDetails.PhoneNumber,
                    IsPhoneNumberConfirmed = ContextFactory.Current.GetDomain<NotificationDomain>().IsPhoneNumberValid(viewModel.CustomerDetails.PhoneNumber),
                    IsTwoFactorEnabled = false,
                    AccessFailedCount = 0,
                    IsLockoutEnabled = true,
                    LockoutEndDateUtc = null,
                    PasswordHash = CryptoHelperMethods.GenerateHash(RandomPassword, salt),
                    SecurityStamp = salt,
                    FingerPrint = CryptoHelperMethods.GenerateHash(viewModel.CustomerDetails.Email, CryptoHelperMethods.GenerateSalt()),
                    IsOnboardingComplete = false,
                    IsActive = false,
                    CreatedBy = userContext.Id,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedBy = userContext.Id,
                    UpdatedDate = DateTimeOffset.Now,
                    OnboardedTypeId = (int)OnboardedType.ThirdPartyOrderOnboarded
                };
                return user;
            }
            else
            {
                return existingUser;
            }
        }

        private Company CreateCompanyForThridPartyOrder(ThirdPartyOrderViewModel thirdPartyOrderViewModel)
        {
            Company existingCompany = null;
            if (thirdPartyOrderViewModel.CustomerDetails.CompanyId.HasValue)
            {
                existingCompany = Context.DataContext.Companies
                                    .SingleOrDefault(t => t.Id == thirdPartyOrderViewModel.CustomerDetails.CompanyId);
            }
            else if (!string.IsNullOrEmpty(thirdPartyOrderViewModel.CustomerDetails.CompanyName))
            {
                existingCompany = Context.DataContext.Companies
                    .SingleOrDefault(t => t.Name.Trim().Equals(thirdPartyOrderViewModel.CustomerDetails.CompanyName.Trim()));
            }

            if (existingCompany == null)
            {
                return CreateNewBuyerCompany(thirdPartyOrderViewModel.CustomerDetails.CompanyName);
            }
            else
            {
                return existingCompany;
            }
        }

        public Company CreateNewBuyerCompany(string companyName)
        {
            return new Company
            {
                Name = companyName,
                CompanyTypeId = (int)CompanyType.Buyer,
                CompanySizeId = 1,
                BusinessTenureId = 1,
                FuelQuantityId = 1,
                IsActive = false,
                CreatedDate = DateTimeOffset.Now,
                UpdatedDate = DateTimeOffset.Now
            };
        }

        private Job CreateJobFromTPOAsync(ThirdPartyOrderViewModel viewModel)
        {
            Job existingJob = null;

            existingJob = Context.DataContext.Jobs.SingleOrDefault(t => t.Name.ToLower() == viewModel.AddressDetails.JobName.Trim().ToLower()
                                       && t.Company.Id == viewModel.CustomerDetails.CompanyId && t.IsActive);
            if (existingJob == null)
            {
                var helperDomain = new HelperDomain(this);

                if (viewModel.AddressDetails.Latitude == 0 || viewModel.AddressDetails.Longitude == 0)
                {
                    var point = GoogleApiDomain.GetGeocode($"{viewModel.AddressDetails.Address} {viewModel.AddressDetails.City} {viewModel.AddressDetails.State.Code} {viewModel.AddressDetails.Country.Code} {viewModel.AddressDetails.ZipCode}");
                    if (point != null)
                    {
                        viewModel.AddressDetails.Latitude = Convert.ToDecimal(point.Latitude);
                        viewModel.AddressDetails.Longitude = Convert.ToDecimal(point.Longitude);
                        viewModel.AddressDetails.CountyName = point.CountyName;
                        string timeZoneName = GoogleApiDomain.GetTimeZone(viewModel.AddressDetails.Latitude, viewModel.AddressDetails.Longitude);
                        if (!string.IsNullOrEmpty(timeZoneName))
                        {
                            viewModel.AddressDetails.TimeZoneName = timeZoneName;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }

                if (string.IsNullOrEmpty(viewModel.AddressDetails.CountyName))
                {
                    var geoAddress = GoogleApiDomain.GetAddress(Convert.ToDouble(viewModel.AddressDetails.Latitude), Convert.ToDouble(viewModel.AddressDetails.Longitude));
                    if (geoAddress != null)
                    {
                        viewModel.AddressDetails.CountyName = geoAddress.CountyName;
                    }
                }

                if (viewModel.AddressDetails.IsVarious && viewModel.AddressDetails.Country.Id != (int)Country.CAR)
                {
                    viewModel.AddressDetails.Address = Resource.lblVarious;
                    viewModel.AddressDetails.City = Resource.lblVarious;
                    viewModel.AddressDetails.ZipCode = Resource.lblVarious;
                    viewModel.AddressDetails.CountyName = Resource.lblVarious;
                }
                else if (viewModel.AddressDetails.Country.Id == (int)Country.CAR)
                {
                    if (string.IsNullOrWhiteSpace(viewModel.AddressDetails.Address))
                    {
                        viewModel.AddressDetails.Address = viewModel.AddressDetails.State.Name ?? Resource.lblCaribbean;
                    }

                    if (string.IsNullOrWhiteSpace(viewModel.AddressDetails.City))
                    {
                        viewModel.AddressDetails.City = viewModel.AddressDetails.State.Name ?? Resource.lblCaribbean;
                    }

                    if (string.IsNullOrWhiteSpace(viewModel.AddressDetails.ZipCode))
                    {
                        viewModel.AddressDetails.ZipCode = viewModel.AddressDetails.State.Name ?? Resource.lblCaribbean;
                    }

                    if (string.IsNullOrWhiteSpace(viewModel.AddressDetails.CountyName))
                    {
                        viewModel.AddressDetails.CountyName = viewModel.AddressDetails.State.Name ?? Resource.lblCaribbean;
                    }
                }

                var job = viewModel.ToEntityFromTPO();

                job.JobBudget = viewModel.ToBudgetEntityFromTPO();
                if (viewModel.IsBuyAndSellOrder)
                {
                    job.JobBudget.IsAssetTracked = true;
                }

                var currencyRateDomain = new CurrencyRateDomain(this);
                job.JobBudget.ExchangeRate = currencyRateDomain.GetCurrencyRate(Currency.USD, job.Currency, DateTimeOffset.Now);

                var approvalUsers = new List<JobXApprovalUser>();
                approvalUsers.Add(new JobXApprovalUser { UserId = (int)viewModel.CustomerDetails.UserId, AssignedDate = DateTimeOffset.Now, IsActive = true });
                job.JobXApprovalUsers = approvalUsers;
                var terminalId = (viewModel.PricingDetails.TerminalId != null) ? viewModel.PricingDetails.TerminalId.Value : 0;
                if (terminalId == 0)
                {
                    job.TerminalId = helperDomain.GetClosestTerminalId(job.Latitude, job.Longitude, job.StateId);
                }
                else
                {
                    job.TerminalId = terminalId;
                }

                return job;
            }
            else
            {
                viewModel.AddressDetails.Latitude = existingJob.Latitude;
                viewModel.AddressDetails.Longitude = existingJob.Longitude;
                viewModel.AddressDetails.CountyName = existingJob.CountyName;
                viewModel.AddressDetails.Country = existingJob.MstCountry.ToViewModel();

                ////update bill to info for existing job
                //var isbillToAvailable = !string.IsNullOrWhiteSpace(viewModel.BillToInfo.Address);
                //if (isbillToAvailable)
                //{
                //    existingJob.BillToAddress = viewModel.BillToInfo.Address;
                //    existingJob.BillToCity = viewModel.BillToInfo.City;
                //    existingJob.BillToCounty = viewModel.BillToInfo.County;
                //    existingJob.BillToCountryId = viewModel.BillToInfo.Country.Id;
                //    existingJob.BillToCountryName = viewModel.BillToInfo.Country.Name;
                //    existingJob.BillToName = viewModel.BillToInfo.Name;
                //    existingJob.BillToStateId = viewModel.BillToInfo.State.Id;
                //    existingJob.BillToStateName = viewModel.BillToInfo.State.Name;
                //    existingJob.BillToZipCode = viewModel.BillToInfo.ZipCode;
                //    existingJob.IsBillToEnabled = isbillToAvailable;
                //}

                if (viewModel.BillToInfo.BillingAddressId.HasValue) //For Existing Job If Billing Address is changed
                {
                    existingJob.IsBillToEnabled = true;
                    existingJob.BillingAddressId = viewModel.BillToInfo.BillingAddressId;
                }

                return existingJob;
            }
        }

        private User GetOnsiteContactUser(UserContext userContext, ThirdPartyOrderViewModel viewModel)
        {
            //check if onsite contact is provided
            if (viewModel.AddressDetails.IsOnsiteContactAvailable() && viewModel.AddressDetails.IsNewContactPerson)
            {
                var onsiteContact = Context.DataContext.Users.Where(t => t.Email.ToLower().Equals(viewModel.AddressDetails.OnsiteContactEmail.ToLower())).FirstOrDefault();
                if (onsiteContact == null)
                {
                    //add user to company
                    var name = viewModel.AddressDetails.OnsiteContactName.Trim().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    switch (name.Length)
                    {
                        case 1:
                            viewModel.AddressDetails.OnsiteFirstName = name[0];
                            viewModel.AddressDetails.OnsiteLastName = name[0].Substring(0, 1);
                            break;
                        default:
                            viewModel.AddressDetails.OnsiteFirstName = name[0];
                            viewModel.AddressDetails.OnsiteLastName = name[1];
                            break;
                    }

                    var salt = CryptoHelperMethods.GenerateSalt();

                    User user = new User
                    {
                        FirstName = viewModel.AddressDetails.OnsiteFirstName,
                        LastName = viewModel.AddressDetails.OnsiteLastName,
                        UserName = viewModel.AddressDetails.OnsiteContactEmail.Trim().ToLower(),
                        Email = viewModel.AddressDetails.OnsiteContactEmail.Trim().ToLower(),
                        IsEmailConfirmed = false,
                        PhoneNumber = viewModel.AddressDetails.OnsiteContactPhone,
                        IsPhoneNumberConfirmed = ContextFactory.Current.GetDomain<NotificationDomain>().IsPhoneNumberValid(viewModel.AddressDetails.OnsiteContactPhone),
                        IsTwoFactorEnabled = false,
                        AccessFailedCount = 0,
                        IsLockoutEnabled = true,
                        LockoutEndDateUtc = null,
                        PasswordHash = CryptoHelperMethods.GenerateHash(Constants.DefaultPassword, salt),
                        SecurityStamp = salt,
                        FingerPrint = CryptoHelperMethods.GenerateHash(viewModel.AddressDetails.OnsiteContactEmail, CryptoHelperMethods.GenerateSalt()),
                        IsOnboardingComplete = true,
                        OnboardedDate = DateTimeOffset.Now,
                        IsFirstLogin = true,
                        IsTaxExemptDisplayed = true,
                        IsActive = true,
                        IsSalesCalculatorAllowed = false,

                        CreatedBy = userContext.Id,
                        CreatedDate = DateTimeOffset.Now,

                        UpdatedBy = userContext.Id,
                        UpdatedDate = DateTimeOffset.Now,
                        OnboardedTypeId = (int)OnboardedType.ThirdPartyOrderOnboarded

                    };

                    return user;
                }
                else
                {
                    viewModel.AddressDetails.OnsiteContactUserId = onsiteContact.Id;
                    return onsiteContact;
                }
            }
            return null;
        }

        private async Task<FuelRequest> AddFuelRequestFromTPO(ThirdPartyOrderViewModel thirdPartyOrderViewModel, Job buyerJob, int companyId)
        {
            //FuelRequests Entity
            var fuelRequest = buyerJob.FuelRequests.SingleOrDefault(t => t.Id == thirdPartyOrderViewModel.FuelRequestId);
            if (fuelRequest == null)
            {
                decimal latitude = buyerJob.Latitude, longitude = buyerJob.Longitude;
                if (buyerJob.LocationType == JobLocationTypes.Various && thirdPartyOrderViewModel.CityGroupTerminalId.HasValue && thirdPartyOrderViewModel.CityGroupTerminalId.Value > 0)
                {
                    var cityGroupTerminal = Context.DataContext.MstExternalTerminals.Where(t => t.Id == thirdPartyOrderViewModel.CityGroupTerminalId).Select(t => new { t.Latitude, t.Longitude }).FirstOrDefault();
                    if (cityGroupTerminal != null)
                    {
                        latitude = cityGroupTerminal.Latitude; longitude = cityGroupTerminal.Longitude;
                    }
                }
                fuelRequest = await thirdPartyOrderViewModel.ToFuelRequestEntityFromTPO(buyerJob, latitude, longitude, fuelRequest, companyId);
                fuelRequest.CreatedBy = thirdPartyOrderViewModel.CustomerDetails.UserId ?? buyerJob.CreatedBy;
                fuelRequest.FuelRequestTypeId = (int)FuelRequestType.ThirdPartyRequest;
            }
            fuelRequest.Currency = buyerJob.Currency;

            fuelRequest.UoM = thirdPartyOrderViewModel.FuelDetails.FuelQuantity.UoM; // thirdPartyOrderViewModel.AddressDetails.Country.UoM;

            thirdPartyOrderViewModel.FuelDeliveryDetails.SpecialInstructions.ForEach(t => { if (!string.IsNullOrWhiteSpace(t.Instruction)) { fuelRequest.SpecialInstructions.Add(t.ToEntity()); } });
            fuelRequest.OrderClosingThreshold = thirdPartyOrderViewModel.FuelOfferDetails.OrderClosingThreshold;
            fuelRequest.PaymentTermId = thirdPartyOrderViewModel.FuelOfferDetails.PaymentTermId;
            fuelRequest.NetDays = thirdPartyOrderViewModel.FuelOfferDetails.NetDays;
            //SupplierQualifications Entity
            if (thirdPartyOrderViewModel.FuelOfferDetails.SupplierQualifications.Count > 0)
            {
                var fuelRequestXSupplierQualifications = Context.DataContext.MstSupplierQualifications.Where(t => thirdPartyOrderViewModel.FuelOfferDetails.SupplierQualifications.Contains(t.Id)).ToList();
                fuelRequest.MstSupplierQualifications = fuelRequestXSupplierQualifications;
            }

            if (thirdPartyOrderViewModel.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypes == TruckLoadTypes.FullTruckLoad)
            {
                fuelRequest.FreightOnBoardTypeId = (int)thirdPartyOrderViewModel.FuelDetails.FuelPricing.FuelPricingDetails.FreightOnBoardTypes;
            }

            return fuelRequest;
        }

        private async Task<FuelRequest> SaveFuelRequestFromTPOAsync(ThirdPartyOrderViewModel viewModel, FuelRequest fuelRequest, UserContext userContext)
        {
            //FuelRequestXDeliveryDetail Entity
            viewModel.FuelRequestId = fuelRequest.Id;
            viewModel.FuelDeliveryDetails.PricingQuantityIndicatorTypeId = (int)viewModel.FuelDetails.FuelQuantity.QuantityIndicatorTypes;
            fuelRequest.FuelRequestDetail = viewModel.ToDeliveryDetailsEntityForTPO();
            fuelRequest.FuelRequestPricingDetail = viewModel.ToPricingDetailsEntityForTPO();
            viewModel.PricingDetails.Currency = viewModel.AddressDetails.Country.Currency;
            var paramterJSON = new SourceRegionJSONParameter()
            {
                SourceRegion = string.Join(",", viewModel.SourceRegion.SelectedSourceRegions.Select(t => t)),
                SelectedTerminals = string.Join(",", viewModel.SourceRegion.SelectedTerminals.Select(t => t)),
                SelectedBulkPlants = string.Join(",", viewModel.SourceRegion.SelectedBulkPlants.Select(t => t))
            };
            var serializer = new JavaScriptSerializer();
            viewModel.PricingDetails.ParameterJSON = serializer.Serialize(paramterJSON);
            //update requestPricedetailId
            var pricingDetailId = await new PricingServiceDomain().SavePricingDetails(viewModel.PricingDetails, fuelRequest.UoM);
            if (pricingDetailId == null || pricingDetailId.Result == 0)
            {
                throw new ArgumentNullException(Resource.errMessageFailedSaveFuelPricing);
            }
            if (pricingDetailId != null)
            {
                fuelRequest.FuelRequestPricingDetail.RequestPriceDetailId = pricingDetailId.Result;
                fuelRequest.FuelRequestPricingDetail.DisplayPrice = pricingDetailId.CustomString1;
                fuelRequest.FuelRequestPricingDetail.DisplayPriceCode = pricingDetailId.CustomString2;
            }

            //FuelRequestFee Entity
            if (!viewModel.IsThirdPartyHardwareUsed)
            {
                FuelFeesDomain fuelFeesDomain = new FuelFeesDomain(this);

                if (viewModel.OrderAdditionalDetailsViewModel.FreightPricingMethod == FreightPricingMethod.Auto)
                {
                    await SetAutoFreightForTPOOrder(viewModel);
                    await fuelFeesDomain.SaveFuelFees(viewModel.FuelDeliveryDetails, fuelRequest, userContext,
                                viewModel.OrderAdditionalDetailsViewModel.IsFuelSurchargeAuto, viewModel.OrderAdditionalDetailsViewModel.IsFreightCost);
                }
                else
                {
                    await fuelFeesDomain.SaveFuelFees(viewModel.FuelDeliveryDetails, fuelRequest, userContext, viewModel.OrderAdditionalDetailsViewModel.IsFuelSurcharge);
                }
            }
            else
            {
                fuelRequest.FuelRequestFees = viewModel.ExternalBrokeredOrder.BrokeredOrderFee.ToEntity();
            }

            //Delivery schedule entity
            if (viewModel.FuelDeliveryDetails.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries && viewModel.FuelDeliveryDetails.DeliverySchedules != null)
            {
                viewModel.FuelDeliveryDetails.DeliverySchedules.ForEach(t => t.UoM = fuelRequest.UoM);
                AddDeliveryscheduleToFuelRequest(fuelRequest, viewModel.FuelDeliveryDetails.DeliverySchedules, viewModel.CustomerDetails.UserId.Value, viewModel.CustomerDetails.CompanyId.Value);
            }

            return fuelRequest;
        }

        private async Task SetAutoFreightForTPOOrder(ThirdPartyOrderViewModel viewModel)
        {
            if (viewModel.OrderAdditionalDetailsViewModel.AccessorialFeeId.HasValue && viewModel.OrderAdditionalDetailsViewModel.AccessorialFeeId.Value > 0)
            {
                var userAddedFees = viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees;
                var accessorialFees = await GetAccessorialFee(viewModel.OrderAdditionalDetailsViewModel.AccessorialFeeId.Value);
                viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = ProcessAndRemoveDuplicateFees(userAddedFees, accessorialFees);
            }

            if (viewModel.OrderAdditionalDetailsViewModel.IsFuelSurchargeAuto && viewModel.OrderAdditionalDetailsViewModel.FuelSurchargeTableId.HasValue && viewModel.OrderAdditionalDetailsViewModel.FuelSurchargeTableId.Value > 0)
            {
                var fuelSurcharge = Context.DataContext.FuelSurchargeIndexes.Where(t => t.Id == viewModel.OrderAdditionalDetailsViewModel.FuelSurchargeTableId).FirstOrDefault();
                if (fuelSurcharge != null)
                {
                    viewModel.OrderAdditionalDetailsViewModel.FuelSurchagePricingType = (FuelSurchagePricingType)fuelSurcharge.FuelSurchargePeriod.Value;
                    viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    viewModel.OrderAdditionalDetailsViewModel.IsFuelSurcharge = true;
                }
            }

            if (viewModel.OrderAdditionalDetailsViewModel.IsFreightCost && viewModel.OrderAdditionalDetailsViewModel.FreightRateRuleId.HasValue && viewModel.OrderAdditionalDetailsViewModel.FreightRateRuleId.Value > 0)
            {
                viewModel.FuelDeliveryDetails.FuelFees.FreightCostFee.FeeSubTypeId = (int)FeeSubType.FlatFee;
                viewModel.OrderAdditionalDetailsViewModel.IsFreightCost = true;
            }
        }

        public List<FeesViewModel> ProcessAndRemoveDuplicateFees(List<FeesViewModel> userAddedFees, List<FeesViewModel> accessorialFees)
        {
            List<FeesViewModel> response = accessorialFees;
            foreach (var item in userAddedFees)
            {
                var isCommonFee = int.TryParse(item.FeeTypeId, out int feeTypeId);
                if (!isCommonFee)
                {
                    response.Add(item);
                }
                else
                {
                    var isFeePresent = accessorialFees.Any(t => t.FeeTypeId == item.FeeTypeId);
                    if (!isFeePresent)
                    {
                        response.Add(item);
                    }
                }
            }
            return response;
        }

        public async Task<List<FeesViewModel>> GetAccessorialFee(int id)
        {
            List<FeesViewModel> viewModel = new List<FeesViewModel>();
            var entity = await Context.DataContext.AccessorialFees.Where(t => t.Id == id).SingleOrDefaultAsync();
            if (entity != null)
            {
                viewModel = entity.FuelFees.ToFeesViewModel();
            }
            return viewModel;
        }

        public FuelRequest AddDeliveryscheduleToFuelRequest(FuelRequest fuelRequest, List<DeliveryScheduleViewModel> deliverySchedules, int userId, int companyId)
        {
            HelperDomain helperDomain = new HelperDomain(this);
            DispatchDomain dispatchDomain = new DispatchDomain(this);
            int latestGroupNumber = 0;
            if (Context.DataContext.DeliverySchedules.Count() > 0)
            {
                latestGroupNumber = Context.DataContext.DeliverySchedules.Max(t => t.GroupId);
            }
            var jobTime = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName);
            var currentDate = fuelRequest.FuelRequestDetail.StartDate < jobTime.Date ? jobTime.Date : fuelRequest.FuelRequestDetail.StartDate;
            foreach (var schedule in deliverySchedules)
            {
                schedule.CreatedBy = userId;
                schedule.GroupId = latestGroupNumber + 1;

                if (schedule.Carrier != null && !string.IsNullOrWhiteSpace(schedule.Carrier.Name))
                {
                    schedule.Carrier = Task.Run(() => dispatchDomain.AddCarrierIfNotExists(schedule.Carrier.Name, userId, companyId)).Result;
                }
                if (!string.IsNullOrEmpty(schedule.SupplierSource.Name))
                {
                    schedule.SupplierSource = Task.Run(() => dispatchDomain.AddSupplierSourceIfNotExists(schedule.SupplierSource, userId, companyId)).Result;
                }
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
            return fuelRequest;
        }

        public async Task<Order> AcceptFuelRequestFromTPO(UserContext userContext, FuelRequest fuelRequest, ThirdPartyOrderViewModel tpoViewModel, User user = null)
        {
            if (fuelRequest != null)
            {
                Order order = await AcceptFRAndCreateOrder(userContext, fuelRequest, tpoViewModel, user);

                var qbWorkflowDomain = new QbWorkflowDomain(this);
                qbWorkflowDomain.CreateSalesOrderWorkflow(userContext, fuelRequest, order);
                qbWorkflowDomain.CreatePurchaseOrderWorkflow(userContext, fuelRequest, order, null);

                return order;
            }
            return null;
        }

        private async Task<Order> AcceptFRAndCreateOrder(UserContext userContext, FuelRequest fuelRequest, ThirdPartyOrderViewModel tpoViewModel, User user)
        {
            SetFuelRequestStatus(userContext, fuelRequest);

            int? externalBrokerId = null;
            OrderXTogglePricingDetail orderXTogglePricingDetail = null;
            if (tpoViewModel.IsBuyAndSellOrder)
            {
                externalBrokerId = tpoViewModel.ExternalBrokeredOrder.CustomerId;
                orderXTogglePricingDetail = new OrderXTogglePricingDetail
                {
                    IsHidePricingEnabledForBuyer = true,
                    IsHidePricingEnabledForSupplier = true
                };
            }


            //create order
            Order order = await SetOrderDetailsAndStatus(userContext, fuelRequest, tpoViewModel, user, externalBrokerId, orderXTogglePricingDetail);
            //order.OrderDeliverySchedules.Add(orderDeliverySchedules);
            fuelRequest.Orders.Add(order);

            await Context.CommitAsync();
            await SetOrderBadgeDetails(userContext, tpoViewModel, order);
            SetOrderPONumber(userContext, fuelRequest, tpoViewModel, order);
            await SetOrderBuyAndSellDetails(userContext, fuelRequest, tpoViewModel, order);

            var appSetting = Context.DataContext.MstAppSettings.Where(t => t.Key.Equals(ApplicationConstants.KeyAppSettingTankOrderAssignement) && t.IsActive).Select(t => new { t.Key, t.Value }).FirstOrDefault();
            if (appSetting != null && bool.TryParse(appSetting.Value, out bool flag) && flag)
            {
                CreateQueueServiceEntryToMapTankWithOrders(userContext, order, fuelRequest, true);
            }

            return order;
        }

        private async Task SetOrderBadgeDetails(UserContext userContext, ThirdPartyOrderViewModel tpoViewModel, Order order)
        {
            order.OrderBadgeDetails = new List<OrderBadgeDetail>();
            var entity = new OrderBadgeDetail();
            if (!string.IsNullOrEmpty(tpoViewModel.OrderBadgeDetails.BadgeNo1) || !string.IsNullOrEmpty(tpoViewModel.OrderBadgeDetails.BadgeNo2) ||
                !string.IsNullOrEmpty(tpoViewModel.OrderBadgeDetails.BadgeNo3))
            {
                entity.OrderId = order.Id;
                entity.BadgeNo1 = tpoViewModel.OrderBadgeDetails.BadgeNo1;
                entity.BadgeNo2 = tpoViewModel.OrderBadgeDetails.BadgeNo2;
                entity.BadgeNo3 = tpoViewModel.OrderBadgeDetails.BadgeNo3;
                entity.IsCommonBadge = true;
                entity.IsActive = true;
                entity.CreatedBy = userContext.Id;
                entity.CreatedDate = DateTimeOffset.Now;
                entity.PickupLocationType = PickupLocationType.None;
                entity.UpdatedBy = userContext.Id;
                entity.UpdatedDate = DateTimeOffset.Now;
                order.OrderBadgeDetails.Add(entity);
                await Context.CommitAsync();
            }

            if (tpoViewModel.OrderBadgeDetails.TerminalBulkBadge != null)
            {
                foreach (var item in tpoViewModel.OrderBadgeDetails.TerminalBulkBadge)
                {
                    if ((item.TerminalId.HasValue || item.BulkPlantId.HasValue) &&
                        !string.IsNullOrEmpty(item.BadgeNo1) || !string.IsNullOrEmpty(item.BadgeNo2) || !string.IsNullOrEmpty(item.BadgeNo3))
                    {
                        entity = new OrderBadgeDetail();
                        entity.OrderId = order.Id;
                        entity.BadgeNo1 = item.BadgeNo1;
                        entity.BadgeNo2 = item.BadgeNo2;
                        entity.BadgeNo3 = item.BadgeNo3;
                        entity.IsCommonBadge = false;
                        entity.IsActive = true;
                        if (item.IsPickupTerminal)
                        {
                            entity.TerminalId = item.TerminalId;
                            entity.PickupLocationType = PickupLocationType.Terminal;
                        }
                        else
                        {
                            entity.BulkPlantId = item.BulkPlantId;
                            entity.PickupLocationType = PickupLocationType.BulkPlant;
                        }
                        entity.CreatedBy = userContext.Id;
                        entity.CreatedDate = DateTimeOffset.Now;
                        entity.UpdatedBy = userContext.Id;
                        entity.UpdatedDate = DateTimeOffset.Now;
                        order.OrderBadgeDetails.Add(entity);
                        await Context.CommitAsync();
                    }
                }
            }
        }

        private async Task SetOrderBuyAndSellDetails(UserContext userContext, FuelRequest fuelRequest, ThirdPartyOrderViewModel tpoViewModel, Order order)
        {
            //insert trackable schedules
            TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);
            await trackableScheduleDomain.ProcessTrackableSchedules(fuelRequest.DeliverySchedules, order);

            if (fuelRequest.DeliverySchedules.Any())
            {
                await Context.CommitAsync();
                //insert notifications
                var notificationDomain = new NotificationDomain(this);
                await notificationDomain.AddNotificationEventAsync(EventType.DeliveryRequestCreated, order.OrderDeliverySchedules.Max(t => t.Id), userContext.Id);
            }

            // save external broker order details
            if (tpoViewModel.AddressDetails.Country.Id == (int)Country.USA && tpoViewModel.IsThirdPartyHardwareUsed)
            {
                ExternalBrokerOrderDetail entity = new ExternalBrokerOrderDetail
                {
                    CustomerNumber = tpoViewModel.ExternalBrokeredOrder.CustomerNumber,
                    InvoicePreferenceId = tpoViewModel.ExternalBrokeredOrder.InvoicePreferenceId,
                    OrderId = order.Id,
                    ProductCode = tpoViewModel.ExternalBrokeredOrder.ProductCode,
                    ShipTo = tpoViewModel.ExternalBrokeredOrder.ShipTo,
                    Source = tpoViewModel.ExternalBrokeredOrder.Source,
                    VendorId = tpoViewModel.ExternalBrokeredOrder.VendorId,
                    ThirdPartyNozzleId = tpoViewModel.ExternalBrokeredOrder.ThirdPartyNozzleId,
                    UpdatedBy = userContext.Id,
                    UpdatedDate = DateTimeOffset.Now,
                    IsActive = true,
                };

                Context.DataContext.ExternalBrokerOrderDetails.Add(entity);
                await Context.CommitAsync();
            }

            if (tpoViewModel.AddressDetails.Country.Id == (int)Country.USA && tpoViewModel.IsBuyAndSellOrder)
            {
                ExternalBrokerBuySellDetail brokerBuySellDetail = new ExternalBrokerBuySellDetail();
                brokerBuySellDetail.OrderId = order.Id;
                brokerBuySellDetail.BrokerMarkUp = tpoViewModel.PricingDetails.BrokerMarkUp;
                brokerBuySellDetail.SupplierMarkUp = tpoViewModel.PricingDetails.SupplierMarkUp;
                brokerBuySellDetail.ExternalBrokerId = tpoViewModel.ExternalBrokeredOrder.CustomerId.Value;
                brokerBuySellDetail.Currency = fuelRequest.Currency;
                brokerBuySellDetail.IsActive = true;

                Context.DataContext.ExternalBrokerBuySellDetails.Add(brokerBuySellDetail);
                await Context.CommitAsync();
            }
        }

        private void SetOrderPONumber(UserContext userContext, FuelRequest fuelRequest, ThirdPartyOrderViewModel tpoViewModel, Order order)
        {
            var helperDomain = new HelperDomain(this);

            order.PoNumber = helperDomain.GetPoNumber(fuelRequest, tpoViewModel.AddressDetails.IsProFormaPoEnabled, order.Id);
            order.TfxPoNumber = order.PoNumber;
            var orderDetailVersion = helperDomain.GetOrderDetailVersion(order, fuelRequest, userContext.Id);
            order.OrderDetailVersions.Add(orderDetailVersion);

            if (order.FuelRequest.FuelRequestDetail.PaymentMethod == PaymentMethods.CreditCard)
            {
                helperDomain.AddCreditCardProcessingFee(order);
            }
        }

        private async Task<Order> SetOrderDetailsAndStatus(UserContext userContext, FuelRequest fuelRequest, ThirdPartyOrderViewModel tpoViewModel, User user, int? externalBrokerId, OrderXTogglePricingDetail orderXTogglePricingDetail)
        {
            var dispatchDomain = new DispatchDomain(this);
            Order order = new Order
            {
                PoNumber = ApplicationConstants.PoNumberPrefix,
                IsProFormaPo = tpoViewModel.AddressDetails.IsProFormaPoEnabled,
                SignatureEnabled = tpoViewModel.AddressDetails.SignatureEnabled,
                AcceptedCompanyId = user == null ? userContext.CompanyId : user.CompanyId.Value,
                AcceptedBy = user == null ? userContext.Id : user.Id,
                AcceptedDate = DateTimeOffset.Now,
                TerminalId = fuelRequest.TerminalId,
                BuyerCompanyId = fuelRequest.User == null ? tpoViewModel.CustomerDetails.CompanyId.Value : fuelRequest.User.CompanyId.Value,
                IsActive = true,
                UpdatedBy = userContext.Id,
                UpdatedDate = DateTimeOffset.Now,
                DefaultInvoiceType = tpoViewModel.DefaultInvoiceType,
                IsEndSupplier = true,
                ExternalBrokerId = externalBrokerId,
                CityGroupTerminalId = fuelRequest.CityGroupTerminalId,
                OrderXTogglePricingDetail = orderXTogglePricingDetail,
                IsFTL = tpoViewModel.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypes == TruckLoadTypes.FullTruckLoad,
                Name = tpoViewModel.OrderName
            };

            OrderXStatus orderStatus = new OrderXStatus();
            orderStatus.StatusId = (int)OrderStatus.Open;
            orderStatus.IsActive = true;
            orderStatus.UpdatedBy = userContext.Id;
            orderStatus.UpdatedDate = DateTimeOffset.Now;
            order.OrderXStatuses.Add(orderStatus);

            var carrier = new CarrierViewModel();

            if (!string.IsNullOrEmpty(tpoViewModel.Carrier.Name))
            {
                carrier = await dispatchDomain.AddCarrierIfNotExists(tpoViewModel.Carrier.Name, userContext.Id, userContext.CompanyId);
            }
            if (tpoViewModel.OrderAdditionalDetailsViewModel.SupplierSource == null || string.IsNullOrEmpty(tpoViewModel.OrderAdditionalDetailsViewModel.SupplierSource.Name)) // when award a quote and clone order
            {
                tpoViewModel.OrderAdditionalDetailsViewModel.SupplierSource = new SupplierSourceViewModel();
            }
            else if (!string.IsNullOrEmpty(tpoViewModel.OrderAdditionalDetailsViewModel.SupplierSource.Name))
            {
                tpoViewModel.OrderAdditionalDetailsViewModel.SupplierSource = await dispatchDomain.AddSupplierSourceIfNotExists(tpoViewModel.OrderAdditionalDetailsViewModel.SupplierSource, userContext.Id, userContext.CompanyId);
            }
            order.OrderAdditionalDetail = tpoViewModel.ToOrderAdditionalDetailsEntityForTPO();
            if (carrier.Id > 0 && order.OrderAdditionalDetail != null)
            {
                order.OrderAdditionalDetail.CarrierId = carrier.Id;
            }

            if (tpoViewModel.IsOnboardingPreferenceExists && tpoViewModel.PreferencesSetting != null && tpoViewModel.PreferencesSetting.Id > 0)
            {
                order.OrderAdditionalDetail.PreferencesSettingId = tpoViewModel.PreferencesSetting.Id;
                // var tfxProductTypeId = Context.DataContext.MstProducts.Where(t => t.Id == tpoViewModel.FuelDetails.FuelTypeId && t.IsActive).FirstOrDefault()?.TfxProductId ?? 0;
                // order.OrderAdditionalDetail.SupplierAssignedProductName = helperDomain.GetSupplierAssignedProductName(userContext.CompanyId, tfxProductTypeId, order.TerminalId ?? 0);
            }

            if (fuelRequest.DeliverySchedules.Count > 0)
            {
                fuelRequest.DeliverySchedules.ToList().ForEach(t =>
                                            order.OrderDeliverySchedules.Add(
                                                new OrderVersionXDeliverySchedule()
                                                {
                                                    DeliveryRequestId = t.Id,
                                                    Version = 1,
                                                    CreatedBy = userContext.Id,
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
                                                    CreatedBy = userContext.Id,
                                                    CreatedDate = fuelRequest.CreatedDate,
                                                    IsActive = true
                                                });
            }
            #region check bulk plant is selected for the source region
            if (tpoViewModel.SourceRegion.SelectedBulkPlants != null && tpoViewModel.SourceRegion.ApprovedBulkPlantId != 0)
            {
                var bulkPlant = Context.DataContext.BulkPlantLocations.Where(w => w.Id == tpoViewModel.SourceRegion.ApprovedBulkPlantId && w.IsActive).FirstOrDefault();
                if (bulkPlant != null)
                {
                    SetFuelDispatchLocation(userContext, fuelRequest, order, bulkPlant);
                }
            }
            #endregion

            if (tpoViewModel.LeadRequestId != null && tpoViewModel.LeadRequestId.Value > 0)
            {
                order.LeadRequestId = tpoViewModel.LeadRequestId;
            }

            return order;
        }

        public void SetFuelDispatchLocation(UserContext userContext, FuelRequest fuelRequest, Order order, BulkPlantLocation bulkPlant)
        {
            order.FuelDispatchLocations.Add(new FuelDispatchLocation()
            {
                LocationType = (int)LocationType.PickUp,
                Address = bulkPlant.Address,
                City = bulkPlant.City,
                StateCode = bulkPlant.StateCode,
                StateId = bulkPlant.StateId,
                ZipCode = bulkPlant.ZipCode,
                CountryCode = bulkPlant.CountryCode,
                Latitude = bulkPlant.Latitude,
                Longitude = bulkPlant.Longitude,
                IsActive = true,
                CreatedBy = userContext.Id,
                CreatedDate = fuelRequest.CreatedDate,
                CountyName = bulkPlant.CountyName,
                Currency = bulkPlant.CountryId == (int)Country.USA ? Currency.USD : Currency.CAD,
                TimeZoneName = fuelRequest.Job.TimeZoneName,
                IsJobLocation = true,
                SiteName = fuelRequest.Job.DisplayJobID,
                BulkPlantId = bulkPlant.Id
            });
        }
        public void SetFuelRequestStatus(UserContext userContext, FuelRequest fuelRequest)
        {
            //update fuelrequest status
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

        public void CreateQueueServiceEntryToMapTankWithOrders(UserContext userContext, Order order, FuelRequest fuelRequest, bool needMapping)
        {
            QueueProcessType queueProcessType = QueueProcessType.CreateTankOrderMappingInFreightService;

            var queueDomain = new QueueMessageDomain();
            var jsonViewModel = new OrderTankMappingProcessorReqViewModel();
            jsonViewModel.OrderId = order.Id;
            jsonViewModel.JobId = fuelRequest.JobId;
            jsonViewModel.DisplayJobId = fuelRequest.Job.DisplayJobID;
            jsonViewModel.CreatedBy = userContext.Id;
            jsonViewModel.IsAssetTrackingEnabled = needMapping;
            jsonViewModel.SupplierCompanyId = order.AcceptedCompanyId;

            string json = JsonConvert.SerializeObject(jsonViewModel);

            var queueRequest = new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = queueProcessType,
                JsonMessage = json
            };
            var queueId = queueDomain.EnqeueMessage(queueRequest);
        }

        public List<DropdownDisplayItem> GetAllTPOCompanies(string companyName = "", bool isMarineNomination = false)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (!isMarineNomination)
                {
                    response = (from buyerUser in Context.DataContext.Users
                                join company in Context.DataContext.Companies on buyerUser.CompanyId equals company.Id
                                join job in Context.DataContext.Jobs on company.Id equals job.CompanyId into j
                                from job in j.DefaultIfEmpty()
                                where !buyerUser.Company.Name.ToLower().Contains("delete") && buyerUser.OnboardedTypeId == (int)OnboardedType.ThirdPartyOrderOnboarded && company.CompanyTypeId == (int)CompanyType.Buyer && !company.IsDeleted &&
                                     (companyName == "" || company.Name.Contains(companyName.Trim()))
                                select new DropdownDisplayItem()
                                {
                                    Id = company.Id,
                                    Name = company.Name.Trim()
                                }
                            ).Distinct().OrderBy(t => t.Name).ToList();
                }
                else if (isMarineNomination)
                {
                    /// commented job/location join as ports company id is not matching wuth buyer company id. Port created by Super Admin
                    response = (from buyerUser in Context.DataContext.Users
                                join company in Context.DataContext.Companies on buyerUser.CompanyId equals company.Id
                                //join job in Context.DataContext.Jobs on company.Id equals job.CompanyId
                                where !buyerUser.Company.Name.ToLower().Contains("delete") && buyerUser.OnboardedTypeId == (int)OnboardedType.ThirdPartyOrderOnboarded && company.CompanyTypeId == (int)CompanyType.Buyer && !company.IsDeleted &&
                                     (companyName == "" || company.Name.Contains(companyName.Trim()))
                                select new DropdownDisplayItem()
                                {
                                    Id = company.Id,
                                    Name = company.Name.Trim()
                                }
                            ).Distinct().OrderBy(t => t.Name).ToList();
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetAllTPOCompanies", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetAllBuyerCompanies(string companyName = "")
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = (from buyerUser in Context.DataContext.Users
                            join company in Context.DataContext.Companies on buyerUser.CompanyId equals company.Id
                            where !company.Name.ToLower().Contains("delete") && company.CompanyTypeId == (int)CompanyType.Buyer
                                  && !company.IsDeleted && (companyName == "" || company.Name.Contains(companyName.Trim()))
                            select new DropdownDisplayItem()
                            {
                                Id = company.Id,
                                Name = company.Name.Trim()
                            }
                        ).Distinct().OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetAllBuyerCompanies", ex.Message, ex);
            }
            return response;
        }

        public bool IsValidTpoCompany(int companyId)
        {
            var response = false;
            try
            {
                response = Context.DataContext.Companies.Where(c => c.Id == companyId).FirstOrDefault()
                    .Users.Any(u => u.OnboardedTypeId == (int)OnboardedType.ThirdPartyOrderOnboarded);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "IsTpoCompany", ex.Message, ex);
            }
            return response;
        }

        private bool IsValidCommonFees(ThirdPartyOrderCsvViewModel viewModel)
        {
            bool response = false;
            try
            {
                var fees = GetFuelRequestFee(viewModel);
                response = fees != null;
                if (response)
                {
                    var anyDuplicate = fees.Where(t => t.FeeTypeId != ((int)FeeType.OtherFee).ToString()).GroupBy(x => new { FeeTypeId = x.FeeTypeId, FeeConstraintTypeId = x.FeeConstraintTypeId, SpecialDate = x.SpecialDate }).Any(g => g.Count() > 1);
                    if (anyDuplicate)
                    {
                        throw new FormatException("Can't apply duplicate fees on single order");
                    }
                    if (fees.Any(t => t.DeliveryFeeByQuantity.Count > 0))
                    {
                        var deliveryFeeByQuantity = fees.FirstOrDefault(t2 => t2.FeeTypeId == ((int)FeeType.DeliveryFee).ToString() && t2.DeliveryFeeByQuantity != null).DeliveryFeeByQuantity;
                        decimal lastMaxQuantity = 0;
                        foreach (var fee in deliveryFeeByQuantity)
                        {
                            if (fee.MinQuantity == (lastMaxQuantity + 1) && fee.MinQuantity < fee.MaxQuantity)
                            {
                                lastMaxQuantity = fee.MaxQuantity.Value;
                            }
                            else
                            {
                                throw new FormatException("Delivery fee By quantity not in proper format");
                            }
                        }
                    }
                    if (fees.Any(t => t.FeeConstraintTypeId.HasValue) && viewModel.DeliveryType.Equals("Single"))
                    {
                        throw new FormatException("Special fees only apply on multiple delivery order");
                    }
                }
            }
            catch (Exception ex)
            {
                response = false;
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetFuelRequestFee", ex.Message, ex);
            }
            return response;
        }

        private bool IsValidFreightFee(ThirdPartyOrderCsvViewModel viewModel)
        {
            bool response;
            try
            {
                var freightFee = new CsvFreightFee(viewModel.FreightFee);
                response = freightFee != null;
            }
            catch
            {
                response = false;
            }
            return response;
        }


        private bool IsValidOtherFee(ThirdPartyOrderCsvViewModel viewModel)
        {
            bool response;
            try
            {
                var csvOtherFees = new CsvOtherFees(viewModel.OtherFees);
                response = csvOtherFees != null;
            }
            catch
            {
                response = false;
            }
            return response;
        }

        private bool IsValidOrderTax(ThirdPartyOrderCsvViewModel viewModel)
        {
            bool response;
            try
            {
                if (!string.IsNullOrWhiteSpace(viewModel.OrderTaxes) && string.IsNullOrWhiteSpace(viewModel.NonStandardFuelType))
                {
                    throw new FormatException("Order taxes should be applicable on other products only");
                }
                var orderTaxes = new CsvOrderTaxes(viewModel.OrderTaxes);
                response = orderTaxes != null;
            }
            catch
            {
                response = false;
            }
            return response;
        }

        private bool IsValidExternalBrokeredAdditionalFee(ThirdPartyOrderCsvViewModel viewModel)
        {
            bool response;
            try
            {
                var additionalFees = new CsvBrokeredOtherFees(viewModel.BrokeredOrderAdditionalFees);
                response = additionalFees != null;
            }
            catch
            {
                response = false;
            }
            return response;
        }

        private bool IsValidInvoicePreference(ThirdPartyOrderCsvViewModel viewModel)
        {
            bool response = false;
            try
            {
                if (string.IsNullOrWhiteSpace(viewModel.InvoicePreference))
                {
                    return response;
                }

                var lstInvoicePreference = (InvoicePreference[])Enum.GetValues(typeof(InvoicePreference));
                var isInvoicePrefExist = lstInvoicePreference.FirstOrDefault(t => t.ToString().ToLower() == viewModel.InvoicePreference.ToLower());
                response = isInvoicePrefExist > 0;
            }
            catch
            {
                response = false;
            }
            return response;
        }

        private bool IsValidExternalCustomer(ThirdPartyOrderCsvViewModel viewModel)
        {
            bool response = false;
            try
            {
                if (string.IsNullOrWhiteSpace(viewModel.BrokeredCustomer))
                {
                    return response;
                }

                var externalCustomer = Context.DataContext.ExternalBrokers
                                        .FirstOrDefault(t => t.CompanyName.Equals(viewModel.BrokeredCustomer, StringComparison.OrdinalIgnoreCase));
                response = externalCustomer != null;
            }
            catch
            {
                response = false;
            }
            return response;
        }

        private bool IsInValidDriverDetails(ThirdPartyOrderCsvViewModel viewModel)
        {
            bool response = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(viewModel.DriverFirstName) && !string.IsNullOrWhiteSpace(viewModel.DriverLastName) && !string.IsNullOrWhiteSpace(viewModel.DriverEmail))
                {
                    var existingUser = Context.DataContext.Users.Where(t => t.Email == viewModel.DriverEmail).FirstOrDefault();
                    if (existingUser != null && (existingUser.FirstName != viewModel.DriverFirstName || existingUser.LastName != viewModel.DriverLastName))
                    {
                        response = true;
                    }
                }
            }
            catch
            {
                response = true;
            }
            return response;
        }

        private List<FeesViewModel> GetFuelRequestFee(ThirdPartyOrderCsvViewModel TpoCsvViewModel)
        {
            var response = new List<FeesViewModel>();
            try
            {

                if (!string.IsNullOrWhiteSpace(TpoCsvViewModel.CommonFees))
                {
                    var fields = TpoCsvViewModel.CommonFees.Split(';');
                    foreach (var field in fields)
                    {
                        if (string.IsNullOrEmpty(field)) { continue; }
                        var feeRecord = field.Split(' ');
                        if (feeRecord.Length < 4)
                        {
                            throw new FormatException("Common Fee is not in proper format");
                        }

                        switch (feeRecord[0].ToLower())
                        {
                            case "wethosefee":
                                var wetHoseFee = new CsvWetHoseFee(field);
                                response.Add(new FeesViewModel()
                                {
                                    FeeTypeId = ((int)FeeType.WetHoseFee).ToString(),
                                    FeeSubTypeId = (int)wetHoseFee.FeeSubType,
                                    Fee = wetHoseFee.Fee,
                                    IncludeInPPG = wetHoseFee.IncludeInPPG,
                                    FeeConstraintTypeId = (int?)wetHoseFee.FeeConstraintTypeId,
                                    SpecialDate = wetHoseFee.SpecialDate
                                });
                                break;

                            case "overwaterfee":
                                var overWaterFee = new CsvOverWaterFee(field);
                                response.Add(new FeesViewModel()
                                {
                                    FeeTypeId = ((int)FeeType.OverWaterFee).ToString(),
                                    FeeSubTypeId = (int)overWaterFee.FeeSubType,
                                    Fee = overWaterFee.Fee,
                                    IncludeInPPG = overWaterFee.IncludeInPPG,
                                    FeeConstraintTypeId = (int?)overWaterFee.FeeConstraintTypeId,
                                    SpecialDate = overWaterFee.SpecialDate
                                });
                                break;

                            case "dryrunfee":
                                var dryRunFee = new CsvDryRunFee(field);
                                response.Add(new FeesViewModel()
                                {
                                    FeeTypeId = ((int)FeeType.DryRunFee).ToString(),
                                    FeeSubTypeId = (int)dryRunFee.FeeSubType,
                                    Fee = dryRunFee.Fee,
                                    IncludeInPPG = dryRunFee.IncludeInPPG,
                                    FeeConstraintTypeId = (int?)dryRunFee.FeeConstraintTypeId,
                                    SpecialDate = dryRunFee.SpecialDate
                                });
                                break;

                            case "under":
                                var underGallonFee = new CsvUnderGallonRunFee(field);
                                response.Add(new FeesViewModel()
                                {
                                    FeeTypeId = ((int)FeeType.UnderGallonFee).ToString(),
                                    FeeSubTypeId = (int)underGallonFee.FeeSubType,
                                    MinimumGallons = underGallonFee.MinimumGallons,
                                    Fee = underGallonFee.Fee,
                                    IncludeInPPG = underGallonFee.IncludeInPPG,
                                    FeeConstraintTypeId = (int?)underGallonFee.FeeConstraintTypeId,
                                    SpecialDate = underGallonFee.SpecialDate
                                });
                                break;

                            case "deliveryfee":
                                var deliveryFee = new CsvDeliveryFee(field, TpoCsvViewModel.MaxQuantity);
                                response.Add(new FeesViewModel()
                                {
                                    FeeTypeId = ((int)FeeType.DeliveryFee).ToString(),
                                    FeeSubTypeId = (int)deliveryFee.FeeSubType,
                                    Fee = deliveryFee.Fee,
                                    IncludeInPPG = deliveryFee.IncludeInPPG,
                                    FeeConstraintTypeId = (int?)deliveryFee.FeeConstraintTypeId,
                                    SpecialDate = deliveryFee.SpecialDate
                                });
                                var feeByQuanityList = deliveryFee.FeesByQuantity.OrderBy(t => t.MinQuantity).Select(t => new DeliveryFeeByQuantityViewModel
                                {
                                    FeeTypeId = (int)FeeType.DeliveryFee,
                                    FeeSubTypeId = (int)FeeSubType.ByQuantity,
                                    MinQuantity = t.MinQuantity,
                                    MaxQuantity = t.MaxQuantity,
                                    Fee = t.Fee
                                }).ToList();
                                response.Where(t => t.FeeTypeId == ((int)FeeType.DeliveryFee).ToString()).ToList().ForEach(t => t.DeliveryFeeByQuantity.AddRange(feeByQuanityList));
                                break;


                            default:
                                var otherCommonFees = new CsvAdditionalFee(field);
                                response.Add(new FeesViewModel()
                                {
                                    CommonFee = true,
                                    FeeTypeId = ((int)otherCommonFees.FeeType).ToString(),
                                    FeeSubTypeId = (int)otherCommonFees.FeeSubType,
                                    IncludeInPPG = otherCommonFees.IncludeInPPG,
                                    Fee = otherCommonFees.Fee,
                                    FeeConstraintTypeId = (int?)otherCommonFees.FeeConstraintTypeId,
                                    SpecialDate = otherCommonFees.SpecialDate
                                });
                                break;
                        }
                    }
                }

                var otherFee = new CsvOtherFees(TpoCsvViewModel.OtherFees);
                response.AddRange(otherFee.Fees.Select(t => new FeesViewModel
                {
                    FeeTypeId = ((int)FeeType.OtherFee).ToString(),
                    OtherFeeDescription = t.FeeDescription,
                    FeeSubTypeId = (int)t.FeeSubType,
                    IncludeInPPG = t.IncludeInPPG,
                    Fee = t.Fee,
                    CommonFee = false,
                    FeeConstraintTypeId = (int?)t.FeeConstraintTypeId,
                    SpecialDate = t.SpecialDate
                }).ToList());
            }
            catch (Exception ex)
            {
                response = null;
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetFuelRequestFee", ex.Message, ex);
            }

            return response;
        }

        private List<OrderTaxDetailsViewModel> GetOrderTaxes(ThirdPartyOrderCsvViewModel csvRecord)
        {
            var response = new List<OrderTaxDetailsViewModel>();
            if (string.IsNullOrWhiteSpace(csvRecord.StandardFuelType))
            {
                var orderTax = new CsvOrderTaxes(csvRecord.OrderTaxes);
                if (orderTax != null && orderTax.Taxes != null && orderTax.Taxes.Count > 0)
                {
                    orderTax.Taxes.ForEach(tax => response.Add(new OrderTaxDetailsViewModel { TaxDescription = tax.TaxDescription, TaxPricingTypeId = (int)tax.TaxPricingType, TaxRate = tax.TaxRate }));
                }
            }
            return response;
        }

        private TPOBrokeredOrderFeeViewModel GetBrokeredOrderFee(ThirdPartyOrderCsvViewModel TpoCsvViewModel)
        {
            var response = new TPOBrokeredOrderFeeViewModel();

            var freightFee = new CsvFreightFee(TpoCsvViewModel.FreightFee);
            response.FreightFeeTypeId = (int)FeeType.FreightFee;
            response.FreightFeeSubTypeId = (int)freightFee.FeeSubType;
            response.FreightFee = freightFee.Fee;

            var additionalOtherFee = new CsvBrokeredOtherFees(TpoCsvViewModel.BrokeredOrderAdditionalFees);
            response.AdditionalFees = additionalOtherFee.Fees.Select(t => new BrokeredOrderFeeViewModel
            {
                FeeTypeId = (int)FeeType.OtherFee,
                FeeDetails = t.FeeDescription,
                FeeSubTypeId = (int)t.FeeSubType,
                Fee = t.Fee
            }).ToList();

            return response;
        }

        private void SetFuelDetailsToFuelRequestEntity(FuelRequest fuelRequest, FuelDetailsViewModel viewModel)
        {
            fuelRequest.QuantityTypeId = viewModel.FuelQuantity.QuantityTypeId;
            fuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId = (int)viewModel.FuelQuantity.QuantityIndicatorTypes;

            if (viewModel.FuelQuantity.QuantityTypeId == (int)QuantityType.Range)
            {
                fuelRequest.MinQuantity = viewModel.FuelQuantity.MinimumQuantity;
                fuelRequest.MaxQuantity = viewModel.FuelQuantity.MaximumQuantity;
            }
            else if (viewModel.FuelQuantity.QuantityTypeId == (int)QuantityType.SpecificAmount)
            {
                fuelRequest.MaxQuantity = viewModel.FuelQuantity.Quantity;
            }
            fuelRequest.EstimateGallonsPerDelivery = viewModel.FuelQuantity.EstimatedGallonsPerDelivery.HasValue ? viewModel.FuelQuantity.EstimatedGallonsPerDelivery.Value : 0;
            fuelRequest.UoM = viewModel.FuelQuantity.UoM;
        }

        public async Task ProcessAmpJobList(List<AmpJobViewModel> dataSource, List<string> errorInfo)
        {
            Dictionary<int, bool> brokerCompanyIddict = new Dictionary<int, bool>();
            foreach (var jobViewModel in dataSource)
            {
                var emailCsvFile = false;
                var supplierOrders = Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == jobViewModel.SupplierCompanyId && t.ExternalBrokerId != null && t.FuelRequest.Job.Name.Equals(jobViewModel.JobName, StringComparison.OrdinalIgnoreCase)).OrderByDescending(x => x.Id).ToList();
                var order = supplierOrders.FirstOrDefault(x => x.FuelRequest.FuelTypeId == jobViewModel.ProductTypeId);
                if (order == null)
                {
                    var supplierCompany = Context.DataContext.Companies.FirstOrDefault(x => x.Id == jobViewModel.SupplierCompanyId);
                    errorInfo.Add(SetErrorMsg($"Could not find any order with selected Supplier: {supplierCompany.Name}, Job Name: {jobViewModel.JobName} && fuel type: {jobViewModel.AmpProductType}"));
                    jobViewModel.JobOrOrderNotExists = true;
                    continue;
                }
                else
                {
                    jobViewModel.BuyerCompanyId = order.FuelRequest.Job.CompanyId;
                    if (order.ExternalBrokerOrderDetail.InvoicePreferenceId == (int)InvoicePreference.SendBrokerDataFileToBroker)
                    {
                        emailCsvFile = true;
                    }
                }
                var extBrokerId = order.ExternalBrokerId.Value;
                if (brokerCompanyIddict.ContainsKey(extBrokerId))
                {
                    if (!brokerCompanyIddict[extBrokerId] && emailCsvFile) //if value is false and need to send email
                    {
                        brokerCompanyIddict[extBrokerId] = emailCsvFile;
                    }
                }
                else
                {
                    brokerCompanyIddict.Add(extBrokerId, emailCsvFile);
                }
                jobViewModel.BrokerCompanyId = extBrokerId;
                var invoiceDomain = new InvoiceDomain(this);
                await invoiceDomain.CreateInvoiceForAmpAsync(jobViewModel, order, order.FuelRequest.Job, string.Empty, errorInfo);
            }
            var tpoBrokerCompanieslist = brokerCompanyIddict.Keys.ToList();
            foreach (var brokerCompanyId in tpoBrokerCompanieslist)
            {
                var dataSource1 = dataSource.Where(t => t.BrokerCompanyId == brokerCompanyId).ToList();
                var csvModel = await CreateCsvViewModel(dataSource1, errorInfo);
                StatusViewModel response = new StatusViewModel(); //need to modify
                response = await ConvertCsvToStreamAndUploadToBlob(csvModel, errorInfo, brokerCompanyIddict[brokerCompanyId], brokerCompanyId);
                var brokerCompany = Context.DataContext.ExternalBrokers.Where(c => c.Id == brokerCompanyId).Select(t => t.CompanyName).FirstOrDefault();
                if (response.StatusCode == Status.Success)
                {
                    errorInfo.Add(SetSuccessMsg($"{brokerCompany} data generation, upload successful with blob internal path: {response.StatusMessage}"));
                }
                else
                {
                    errorInfo.Add(SetErrorMsg($"CSV Upload failed to Azure storage for {brokerCompany}. Please try again or contact support"));
                }
                foreach (var jobViewModel in dataSource1)
                {
                    if (!jobViewModel.JobOrOrderNotExists)
                    {
                        UpdateInvoiceWithPath(response.StatusMessage, jobViewModel);
                    }
                }
            }
        }

        private void UpdateInvoiceWithPath(string statusMessage, AmpJobViewModel ampJobViewModel)
        {
            var invoice = Context.DataContext.Invoices.FirstOrDefault(x => x.Id == ampJobViewModel.InvoiceViewModel.Id);
            invoice.FilePath = statusMessage;
            Context.Commit();
        }

        private string SetSuccessMsg(string message)
        {
            return $"<p class='color-green'><b>{message}</b>";
        }

        private string SetErrorMsg(string message)
        {
            return $"<p class='color-maroon'><b>{message}</b>";
        }

        private async Task<List<AmpCsvOutputViewModel>> CreateCsvViewModel(List<AmpJobViewModel> dataSource, List<string> errorInfo)
        {
            List<AmpCsvOutputViewModel> csvRecordList = new List<AmpCsvOutputViewModel>();
            StringBuilder processMessage = new StringBuilder();
            try
            {
                csvRecordList.Add(GetHeaderRowOfInvoiceCsv());

                foreach (var ampJobViewModel in dataSource.FindAll(x => !x.JobOrOrderNotExists))
                {
                    var jobDetails = Context.DataContext.Jobs.SingleOrDefault(t => t.CompanyId == ampJobViewModel.BuyerCompanyId && t.Name.Equals(ampJobViewModel.JobName, StringComparison.OrdinalIgnoreCase));
                    if (jobDetails != null)
                    {
                        //get fuel type
                        var fuelRequest = Context.DataContext.FuelRequests.OrderByDescending(t => t.Id).FirstOrDefault(t => t.JobId == jobDetails.Id && t.FuelTypeId == ampJobViewModel.ProductTypeId);
                        var supplierOrders = Context.DataContext.Orders
                                            .Where(t => t.AcceptedCompanyId == ampJobViewModel.SupplierCompanyId && t.FuelRequest.JobId == jobDetails.Id)
                                            .OrderByDescending(x => x.Id).ToList();
                        var order = supplierOrders.FirstOrDefault(x => x.FuelRequest.FuelTypeId == ampJobViewModel.ProductTypeId);
                        var freightPerDropFeeAdded = false;
                        var otherFlatPerDropFeeAdded = false;
                        var invoiceViewModel = ampJobViewModel.InvoiceViewModel;
                        var totalGallon = dataSource.SelectMany(x => x.Drops).Sum(x => x.DropQuantity);
                        var totalTax = invoiceViewModel.TotalTaxAmount;
                        var taxPerGallon = totalTax / totalGallon;
                        var ppg = ampJobViewModel.InvoiceViewModel.PricePerGallon;

                        if (fuelRequest != null && order != null)
                        {
                            var externalDetails = order.ExternalBrokerOrderDetail;

                            foreach (var drop in ampJobViewModel.Drops)
                            {
                                AmpCsvOutputViewModel csvRecord = new AmpCsvOutputViewModel();
                                //get data from new table order external details
                                csvRecord.VendorId = externalDetails.VendorId;
                                csvRecord.TransationId = order.PoNumber;
                                csvRecord.ShipDate = ampJobViewModel.StartDate.Date.ToShortDateString();
                                csvRecord.VendorName = "AMP";
                                csvRecord.VendorInvNumber = order.Id.ToString();
                                csvRecord.CustomerNumber = externalDetails.CustomerNumber;
                                csvRecord.ShipTo = externalDetails.ShipTo;
                                csvRecord.Source = externalDetails.Source;
                                csvRecord.ProductCode = externalDetails.ProductCode;
                                csvRecord.CustomerName = jobDetails.Company.Name;
                                csvRecord.ShipToAddress = jobDetails.Address;
                                csvRecord.ShipToCity = jobDetails.City;
                                csvRecord.ShipToState = jobDetails.MstState.Code;
                                csvRecord.ShipToZip = jobDetails.ZipCode;
                                csvRecord.ProductQuantity = drop.DropQuantity.GetPreciseValue(2).ToString();
                                csvRecord.ProductUOM = "G";
                                csvRecord.ProductPrice = ppg.GetPreciseValue(4).ToString();

                                var freightFeeObj = fuelRequest.FuelRequestFees.FirstOrDefault(t => t.FeeTypeId == (int)FeeType.FreightFee);
                                decimal freightFee = 0.0M;
                                decimal freightAmount = 0.0M;
                                SetFreightFeeAndAmount(ref freightPerDropFeeAdded, drop, freightFeeObj, ref freightFee, ref freightAmount);
                                csvRecord.Freight = freightFee.GetPreciseValue(2).ToString();

                                var otherFlatFeeObj = fuelRequest.FuelRequestFees.Where(t => t.FeeTypeId == (int)FeeType.OtherFee && t.FeeSubTypeId == (int)FeeSubType.FlatFee).Sum(t1 => t1.Fee);
                                decimal otherFlatFee = 0.0M;
                                decimal otherFlatAmount = 0.0M;
                                SetotherFlatFeeAndAmount(ref otherFlatPerDropFeeAdded, otherFlatFeeObj, ref otherFlatFee, ref otherFlatAmount);

                                csvRecord.Additive = "0";

                                csvRecord.Taxes = taxPerGallon.GetPreciseValue(9).ToString();
                                csvRecord.OtherFlatFee = otherFlatFee.GetPreciseValue(2).ToString();
                                csvRecord.ProductAmount = (ppg * drop.DropQuantity).GetPreciseValue(2).ToString();
                                csvRecord.FreightAmount = freightAmount.GetPreciseValue(2).ToString();
                                csvRecord.AdditiveAmount = "0";
                                csvRecord.TaxAmount = (drop.DropQuantity * taxPerGallon).GetPreciseValue(2).ToString();
                                csvRecord.AmountDue = ((ppg * drop.DropQuantity) + freightAmount + otherFlatAmount + (drop.DropQuantity * taxPerGallon)).GetPreciseValue(2).ToString();
                                csvRecord.CurrencyCode = order.FuelRequest.Currency.ToString();
                                csvRecord.Vehicle = drop.AssetName;
                                csvRecordList.Add(csvRecord);
                            }
                            errorInfo.Add(SetAmpSuccessProcessMessage(ampJobViewModel, jobDetails));
                        }
                        else
                        {
                            errorInfo.Add(SetAmpOrderFailedProcessMessage(ampJobViewModel));
                        }
                    }
                    else
                    {
                        errorInfo.Add(SetAmpFailedProcessMessage(ampJobViewModel));
                    }
                }
            }
            catch (Exception ex)
            {
                if (!(ex is QueueMessageFatalException))
                {
                    LogManager.Logger.WriteException("ThirdPartyOrderDomain", "CreateCsvViewModel", ex.Message, ex);
                }

                if (processMessage.Length == 0)
                {
                    processMessage.Append(Constants.ErrorWhileProcessingAMPJobData);
                    errorInfo.Add(processMessage.ToString());
                }
                throw new QueueMessageFatalException(errorInfo[0], errorInfo);
            }
            return csvRecordList;
        }

        private static void SetotherFlatFeeAndAmount(ref bool otherFlatPerDropFeeAdded, decimal otherTotalFlatFee, ref decimal otherFlatFee, ref decimal otherFlatAmount)
        {
            if (otherTotalFlatFee > 0)
            {
                if (!otherFlatPerDropFeeAdded)
                {
                    otherFlatFee = otherTotalFlatFee;
                    otherFlatAmount = otherTotalFlatFee;
                    otherFlatPerDropFeeAdded = true;
                }
                else
                {
                    otherFlatFee = 0;
                    otherFlatAmount = 0;
                }
            }
        }

        private static void SetFreightFeeAndAmount(ref bool freightPerDropFeeAdded, AmpAssetDropViewModel drop, FuelFee freightFeeObj, ref decimal freightFee, ref decimal freightAmount)
        {
            if (freightFeeObj != null)
            {
                if (freightFeeObj.FeeSubTypeId == (int)FeeSubType.PerGallon)
                {
                    freightFee = freightFeeObj.Fee;
                    freightAmount = freightFeeObj.Fee * drop.DropQuantity;
                }
                else if (freightFeeObj.FeeSubTypeId == (int)FeeSubType.ByAssetCount)
                {
                    freightFee = freightFeeObj.Fee;
                    freightAmount = freightFeeObj.Fee;
                }
                else
                {
                    if (!freightPerDropFeeAdded)
                    {
                        freightFee = freightFeeObj.Fee;
                        freightAmount = freightFeeObj.Fee;
                        freightPerDropFeeAdded = true;
                    }
                    else
                    {
                        freightFee = 0;
                        freightAmount = 0;
                    }
                }
            }
        }

        private AmpCsvOutputViewModel GetHeaderRowOfInvoiceCsv()
        {
            var headerRow = new AmpCsvOutputViewModel();
            headerRow.VendorId = nameof(headerRow.VendorId);
            headerRow.TransationId = nameof(headerRow.TransationId);
            headerRow.ShipDate = nameof(headerRow.ShipDate);
            headerRow.VendorName = nameof(headerRow.VendorName);
            headerRow.VendorInvNumber = nameof(headerRow.VendorInvNumber);
            headerRow.CustomerNumber = nameof(headerRow.CustomerNumber);
            headerRow.ShipTo = nameof(headerRow.ShipTo);
            headerRow.Source = nameof(headerRow.Source);
            headerRow.ProductCode = nameof(headerRow.ProductCode);
            headerRow.CustomerName = nameof(headerRow.CustomerName);
            headerRow.ShipToAddress = nameof(headerRow.ShipToAddress);
            headerRow.ShipToCity = nameof(headerRow.ShipToCity);
            headerRow.ShipToState = nameof(headerRow.ShipToState);
            headerRow.ShipToZip = nameof(headerRow.ShipToZip);
            headerRow.ProductQuantity = nameof(headerRow.ProductQuantity);
            headerRow.ProductUOM = nameof(headerRow.ProductUOM);
            headerRow.ProductPrice = nameof(headerRow.ProductPrice);
            headerRow.Freight = nameof(headerRow.Freight);
            headerRow.Additive = nameof(headerRow.Additive);
            headerRow.Taxes = nameof(headerRow.Taxes);
            headerRow.OtherFlatFee = nameof(headerRow.OtherFlatFee);
            headerRow.ProductAmount = nameof(headerRow.ProductAmount);
            headerRow.FreightAmount = nameof(headerRow.FreightAmount);
            headerRow.AdditiveAmount = nameof(headerRow.AdditiveAmount);
            headerRow.TaxAmount = nameof(headerRow.TaxAmount);
            headerRow.AmountDue = nameof(headerRow.AmountDue);
            headerRow.CurrencyCode = nameof(headerRow.CurrencyCode);
            headerRow.Vehicle = nameof(headerRow.Vehicle);
            return headerRow;
        }

        private async Task<StatusViewModel> ConvertCsvToStreamAndUploadToBlob(List<AmpCsvOutputViewModel> transactionsArray, List<string> errorInfo, bool emailCsvFile, int brokerCompanyId)
        {
            var response = new StatusViewModel();
            var docStream = new MemoryStream();
            using (var flatFileWriter = new StreamWriter(docStream, Encoding.ASCII))
            {
                var fileWriterEngine = new FileHelperEngine(typeof(AmpCsvOutputViewModel));
                fileWriterEngine.WriteStream(flatFileWriter, transactionsArray);

                // Flush contents of fileWriterStream to underlying docStream:
                flatFileWriter.Flush();

                try
                {
                    var azureBlob = new AzureBlobStorage();
                    var fileName = string.Concat(values: Constants.InvoiceCSV + DateTime.Now.Date.ToString(Resource.lblDateFormat) + Resource.lblSingleHyphen + DateTime.Now.Ticks + ".csv");
                    var filePath = await azureBlob.SaveBlobAsync(docStream, fileName, BlobContainerType.MansfieldAMPInvoiceCsv.ToString().ToLower());
                    if (!string.IsNullOrWhiteSpace(filePath))
                    {
                        response.StatusCode = Status.Success;
                        response.StatusMessage = filePath;
                    }
                    else
                    {
                        response.StatusMessage = Resource.errMessageErrorInAzureServer;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageErrorInAzureServer;
                    LogManager.Logger.WriteException("ThirdPartyOrderDomain", "ConvertCsvToStreamAndUploadToBlob", ex.Message, ex);
                }
                if (docStream.Length > 0 && emailCsvFile)
                {
                    await SendCsvFileToMansfield(docStream, brokerCompanyId, errorInfo);
                }
            }
            return response;
        }

        public async Task<bool> SendCsvFileToMansfield(Stream stream, int brokerCompanyId, List<string> errorInfo = null)
        {
            var response = false;
            try
            {
                HelperDomain helperDomain = new HelperDomain(this);
                var serverUrl = helperDomain.GetServerUrl();
                var emailTemplate = helperDomain.GetApplicationEventNotificationTemplate();
                var brokerDetails = Context.DataContext.ExternalBrokers.Where(t => t.Id == brokerCompanyId).FirstOrDefault();
                stream.Position = 0;
                string fileName = string.Format("AMP_{0}_{1}.csv", brokerDetails.CompanyName, DateTime.Now.ToString("MM_dd_yyyy"));
                System.Net.Mail.Attachment file = new System.Net.Mail.Attachment(stream, fileName, Core.Utilities.MediaType.Text);

                var attachements = new List<System.Net.Mail.Attachment>() { file };
                var customerName = brokerDetails.CompanyName;
                var customerEmail = brokerDetails.Email;
                var emails = customerEmail.Split(';').ToList(); // no need now
                var companyLogo = helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.email_HeaderLogo);
                var subjectLine = Resource.emailMansfield_SubjectText;
                var bodyText = string.Format(Resource.emailMansfield_BodyText, customerName);

                var emailModel = new ApplicationEventNotificationViewModel
                {
                    To = emails,
                    Subject = subjectLine,
                    CompanyLogo = companyLogo,
                    BodyText = bodyText,
                    Attachments = attachements,
                    ShowFooterContent = false,
                    ShowHelpLineInfo = false
                };

                response = await ContextFactory.Current.GetDomain<EmailDomain>().SendEmail(emailTemplate, emailModel);
                if (response && errorInfo != null)
                {
                    errorInfo.Add(SetSuccessMsg($"{customerName} Data File sent to {customerEmail} email address"));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "SendCsvFileToMansfield", ex.Message, ex);
            }
            return response;
        }
        #endregion

        #region Billing Address
        public async Task<BillingAddressViewModel> GetCompanyBillingAddress(int companyId)
        {
            var response = new BillingAddressViewModel();
            var billingAddress = await Context.DataContext.BillingAddresses.FirstOrDefaultAsync(t => t.CompanyId == companyId && t.IsActive);
            if (billingAddress != null)
            {
                response.Id = billingAddress.Id;
                response.Address = billingAddress.Address;
                response.City = billingAddress.City;
                response.County = billingAddress.County;
                response.State = new StateViewModel { Id = 0, Name = billingAddress.StateName };
                response.Country = new CountryViewModel { Id = 0, Name = billingAddress.CountryName };
                response.ZipCode = billingAddress.ZipCode;
            }
            return response;
        }

        private bool IsBillingAddressProvided(TPOBillingAddressViewModel address)
        {
            if (address == null)
            {
                return false;
            }

            return address.IsBillingAddressRequired &&
                    (!string.IsNullOrWhiteSpace(address.Address) ||
                    !string.IsNullOrWhiteSpace(address.City) ||
                    !string.IsNullOrWhiteSpace(address.ZipCode) ||
                    !string.IsNullOrWhiteSpace(address.CountryName) ||
                    !string.IsNullOrWhiteSpace(address.StateName));
        }
        #endregion  

        private ThirdPartyOrderViewModel GetThirdPartyOrderViewModelNew(UserContext context, ThirdPartyOrderCsvViewModelNew csvOrder, List<PricingCodesViewModel> pricingCodes, bool isSuppressPricing, int onboardingPrefId)
        {
            var response = new ThirdPartyOrderViewModel();
            if (csvOrder != null)
            {
                if (!string.IsNullOrWhiteSpace(csvOrder.CompanyName))
                {
                    var pricingCodeVal = isSuppressPricing ? string.Empty : csvOrder.PricingCode;
                    var pricingSourceId = GetPricingSourceId(pricingCodeVal, pricingCodes);
                    response.CustomerDetails = GetCustomerDetails(csvOrder);
                    response.AddressDetails = GetAddressDetails(csvOrder);
                    response.FuelDetails = GetFuelDetails(csvOrder, pricingSourceId, context);
                    if (response.FuelDetails.IsMarineLocation)
                    {
                        response.AddressDetails.IsMarineLocation = true;
                        response.AddressDetails.MarineUoM = response.FuelDetails.FuelQuantity.UoM;
                    }
                    response.PricingDetails = GetPricingDetails(csvOrder, pricingCodes, pricingSourceId, isSuppressPricing);
                    response.FuelDeliveryDetails = GetFuelDelieryDetails(csvOrder);
                    //var externalBrokerOrderDetails = GetExternalBrokerOrderDetails(item);
                    //var orderTaxes = GetOrderTaxes(item);
                    //set payment terms 
                    if (csvOrder.PaymentTerm.ToLower().Trim().Equals("net"))
                    {
                        response.FuelOfferDetails.PaymentTermId = (int)PaymentTerms.NetDays;
                        response.FuelOfferDetails.NetDays = Convert.ToInt32(csvOrder.NetDays);
                    }
                    if (csvOrder.PaymentTerm.ToLower().Trim().Equals("prepaid"))
                    {
                        response.FuelOfferDetails.PaymentTermId = (int)PaymentTerms.PrePaidInFull;
                    }
                    if (csvOrder.PaymentTerm.ToLower().Trim().Equals("dor"))
                    {
                        response.FuelOfferDetails.PaymentTermId = (int)PaymentTerms.DueOnReceipt;
                    }

                    response.IsInvitationEnabled = (!string.IsNullOrWhiteSpace(csvOrder.SendInvitationLink) && csvOrder.SendInvitationLink.ToLower().Trim().Equals("yes")) ? true : false;
                    response.IsNotifyDeliveries = (!string.IsNullOrWhiteSpace(csvOrder.ProvideDeliveryDetailsToCustomer) && csvOrder.ProvideDeliveryDetailsToCustomer.ToLower().Trim().
                                                  Equals("yes")) ? true : false;
                    response.IsAssetTracked = (!string.IsNullOrWhiteSpace(csvOrder.EnableAssetLevelTracking) && csvOrder.EnableAssetLevelTracking.ToLower().Trim().Equals("yes")) ? true : false;
                    if (!string.IsNullOrWhiteSpace(csvOrder.SupplierAllowance))
                    {
                        response.OrderAdditionalDetailsViewModel.Allowance = Convert.ToDecimal(csvOrder.SupplierAllowance);
                    }
                    if (!string.IsNullOrWhiteSpace(csvOrder.Notes))
                    {
                        response.OrderAdditionalDetailsViewModel.Notes = csvOrder.Notes;
                    }

                    //routine when pricing method is auto 
                    if (csvOrder.AutoFreightPricingMethod != null)
                    {
                        if (!string.IsNullOrWhiteSpace(csvOrder.AutoFreightPricingMethod) && csvOrder.AutoFreightPricingMethod.ToLower().Trim().Equals("yes"))
                        {
                            FreightPricingMethodAutoResponse(csvOrder, response, context);
                        }
                    }

                    if (onboardingPrefId > 0)
                    {
                        response.IsSupressOrderPricing = isSuppressPricing;
                        response.IsOnboardingPreferenceExists = true;
                        response.PreferencesSetting = new OnboardingPreferenceViewModel { Id = onboardingPrefId, IsSupressOrderPricing = isSuppressPricing };
                    }
                    if (!string.IsNullOrWhiteSpace(csvOrder.PONumber))
                    {
                        response.PONumber = csvOrder.PONumber;
                    }
                    if (!string.IsNullOrWhiteSpace(csvOrder.OrderName))
                    {
                        response.OrderName = csvOrder.OrderName;
                    }

                    //set carrierid and carrier email for fully carrier managed location
                    if (!string.IsNullOrWhiteSpace(csvOrder.LocationInventoryManagementType)
                        && csvOrder.LocationInventoryManagementType.ToLower().Equals("fully carrier managed"))
                    {
                        response.AssignedCarrierCompId = GetAssignedCarrierIdByName(context.CompanyId, csvOrder.CarrierCompanyName).Result;
                        response.CarrierUserEmails.Add(GetUserIdByEMail(csvOrder.CarrierEmailAddress).Result);
                    }
                    response.TrailerType = Enum.GetValues(typeof(TrailerTypeStatus)).Cast<TrailerTypeStatus>().ToList();

                }
            }
            return response;
        }
        private List<FeesViewModel> GetFuelRequestFeeNew(ThirdPartyOrderCsvViewModelNew csvOrder)
        {
            var response = new List<FeesViewModel>();
            try
            {
                if (!string.IsNullOrWhiteSpace(csvOrder.DeliveryFee))
                {
                    var deliveryFee = GetFeesViewModel(FeeType.DeliveryFee, csvOrder.DeliveryFee);
                    if (deliveryFee != null)
                    {
                        response.Add(deliveryFee);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.WetHoseFee))
                {
                    var wetHoseFee = GetFeesViewModel(FeeType.WetHoseFee, csvOrder.WetHoseFee);
                    if (wetHoseFee != null)
                    {
                        response.Add(wetHoseFee);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.OverWaterFee))
                {
                    var overWaterFee = GetFeesViewModel(FeeType.OverWaterFee, csvOrder.OverWaterFee);
                    if (overWaterFee != null)
                    {
                        response.Add(overWaterFee);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.DryRunFee))
                {
                    var dryRunFee = GetFeesViewModel(FeeType.DryRunFee, csvOrder.DryRunFee);
                    if (dryRunFee != null)
                    {
                        response.Add(dryRunFee);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.FreightFee))
                {
                    var freightFee = GetFeesViewModel(FeeType.FreightFee, csvOrder.DryRunFee);
                    if (freightFee != null)
                    {
                        response.Add(freightFee);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.MinimumGallonFee))
                {
                    var minimumGallonFee = GetFeesViewModel(FeeType.UnderGallonFee, csvOrder.MinimumGallonFee);

                    if (minimumGallonFee != null)
                    {
                        minimumGallonFee.MinimumGallons = Convert.ToDecimal(csvOrder.MinimumGallons);
                        response.Add(minimumGallonFee);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.EnvironmentalFee))
                {
                    var environmentalFee = GetFeesViewModel(FeeType.EnvironmentalFee, csvOrder.EnvironmentalFee);
                    if (environmentalFee != null)
                    {
                        response.Add(environmentalFee);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.ServiceFee))
                {
                    var serviceFee = GetFeesViewModel(FeeType.ServiceFee, csvOrder.ServiceFee);
                    if (serviceFee != null)
                    {
                        response.Add(serviceFee);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.LoadFee))
                {
                    var loadFee = GetFeesViewModel(FeeType.LoadFee, csvOrder.LoadFee);
                    if (loadFee != null)
                    {
                        response.Add(loadFee);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.SurchargeFee))
                {
                    var surchargeFee = GetFeesViewModel(FeeType.SurchargeFee, csvOrder.SurchargeFee);
                    if (surchargeFee != null)
                    {
                        response.Add(surchargeFee);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.StopOffFee))
                {
                    var stopOffFee = GetFeesViewModel(FeeType.StopOffFee, csvOrder.StopOffFee);
                    if (stopOffFee != null)
                    {
                        response.Add(stopOffFee);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.DemurrageFeeDestination))
                {
                    var demurrageFeeDestination = GetFeesViewModel(FeeType.DemurrageFeeDestination, csvOrder.DemurrageFeeDestination);
                    if (demurrageFeeDestination != null)
                    {
                        demurrageFeeDestination.WaiveOffTime = Convert.ToInt32(csvOrder.EmbeddedTime);
                        response.Add(demurrageFeeDestination);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.DemurrageFeeOther))
                {
                    var demurrageFeeOther = GetFeesViewModel(FeeType.DemurrageOther, csvOrder.DemurrageFeeOther);
                    if (demurrageFeeOther != null)
                    {
                        demurrageFeeOther.WaiveOffTime = Convert.ToInt32(csvOrder.EmbeddedTime);
                        response.Add(demurrageFeeOther);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.DemurrageFeeTerminal))
                {
                    var demurrageFeeTerminal = GetFeesViewModel(FeeType.DemurrageFeeTerminal, csvOrder.DemurrageFeeTerminal);
                    if (demurrageFeeTerminal != null)
                    {
                        demurrageFeeTerminal.WaiveOffTime = Convert.ToInt32(csvOrder.EmbeddedTime);
                        response.Add(demurrageFeeTerminal);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.PumpChargeFee))
                {
                    var pumpChargeFee = GetFeesViewModel(FeeType.PumpCharge, csvOrder.PumpChargeFee);
                    if (pumpChargeFee != null)
                    {
                        response.Add(pumpChargeFee);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.SplitTankFee))
                {
                    var splitTankFee = GetFeesViewModel(FeeType.SplitTank, csvOrder.SplitTankFee);
                    if (splitTankFee != null)
                    {
                        response.Add(splitTankFee);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.RetainFee))
                {
                    var retainFee = GetFeesViewModel(FeeType.Retain, csvOrder.RetainFee);
                    if (retainFee != null)
                    {
                        response.Add(retainFee);
                    }
                }
                if (!string.IsNullOrWhiteSpace(csvOrder.AdditiveFee))
                {
                    var additiveFee = GetFeesViewModel(FeeType.AdditiveFee, csvOrder.AdditiveFee);
                    if (additiveFee != null)
                    {
                        response.Add(additiveFee);
                    }
                }
            }
            catch (Exception ex)
            {
                response = null;
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetFuelRequestFeeNew", ex.Message, ex);
            }
            return response;
        }

        private FeesViewModel GetFeesViewModel(FeeType feeType, string fee)
        {
            var result = new FeesViewModel();
            try
            {
                result.FeeTypeId = ((int)feeType).ToString();
                result.FeeSubTypeId = (int)FeeSubType.FlatFee;
                result.Fee = Convert.ToDecimal(fee);
                result.IncludeInPPG = false;
                //FeeConstraintTypeId = (int?)wetHoseFee.FeeConstraintTypeId,
                //SpecialDate = wetHoseFee.SpecialDate

            }
            catch (Exception ex)
            {
                result = null;
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetFeesViewModel", ex.Message, ex);

            }
            return result;
        }


        private PaymentMethods GetPaymentMethods(ThirdPartyOrderCsvViewModelNew csvOrder)
        {
            var paymentMethod = PaymentMethods.None;
            try
            {
                if (!string.IsNullOrWhiteSpace(csvOrder.PaymentMethod))
                {
                    if (csvOrder.PaymentMethod.ToLower().Trim().Equals("credit card"))
                    {
                        paymentMethod = PaymentMethods.CreditCard;
                    }
                    if (csvOrder.PaymentMethod.ToLower().Trim().Equals("bank check"))
                    {
                        paymentMethod = PaymentMethods.BankCheck;
                    }
                    if (csvOrder.PaymentMethod.ToLower().Trim().Equals("draft"))
                    {
                        paymentMethod = PaymentMethods.Draft;
                    }
                    if (csvOrder.PaymentMethod.ToLower().Trim().Equals("bank wire"))
                    {
                        paymentMethod = PaymentMethods.BankWire;
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetPaymentMethods", ex.Message, ex);
            }
            return paymentMethod;
        }

        private OrderEnforcement GetOrderEnforcementId(ThirdPartyOrderCsvViewModelNew csvOrder)
        {
            var orderEnforcementId = OrderEnforcement.EnforceOrderLevelValues;
            try
            {
                if (!string.IsNullOrWhiteSpace(csvOrder.InvoiceCreationPreference))
                {
                    if (csvOrder.InvoiceCreationPreference.ToLower().Trim().Equals("not specified"))
                    {
                        orderEnforcementId = OrderEnforcement.EnforceOrderLevelValues;
                    }
                    else if (csvOrder.InvoiceCreationPreference.ToLower().Trim().Equals("manage exception"))
                    {
                        orderEnforcementId = OrderEnforcement.ManageException;
                    }
                    else if (csvOrder.InvoiceCreationPreference.ToLower().Trim().Equals("no enforcement"))
                    {
                        orderEnforcementId = OrderEnforcement.NoEnforcement;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetOrderEnforcementId", ex.Message, ex);
            }
            return orderEnforcementId;
        }

        private InventoryDataCaptureType GetInventoryDataCaptureType(ThirdPartyOrderCsvViewModelNew csvOrder)
        {
            var inventoryDataCaptureType = InventoryDataCaptureType.NotSpecified;
            try
            {
                if (string.IsNullOrWhiteSpace(csvOrder.InventoryDataCaptureMethod))
                {
                    inventoryDataCaptureType = InventoryDataCaptureType.NotSpecified;
                }
                else if (csvOrder.InventoryDataCaptureMethod.ToLower().Trim().Equals("not specified"))
                {
                    inventoryDataCaptureType = InventoryDataCaptureType.NotSpecified;
                }
                else if (csvOrder.InventoryDataCaptureMethod.ToLower().Trim().Equals("connected"))
                {
                    inventoryDataCaptureType = InventoryDataCaptureType.Connected;
                }
                else if (csvOrder.InventoryDataCaptureMethod.ToLower().Trim().Equals("manual"))
                {
                    inventoryDataCaptureType = InventoryDataCaptureType.Manual;
                }
                else if (csvOrder.InventoryDataCaptureMethod.ToLower().Trim().Equals("call-in"))
                {
                    inventoryDataCaptureType = InventoryDataCaptureType.CallIn;
                }
                else if (csvOrder.InventoryDataCaptureMethod.ToLower().Trim().Equals("mixed"))
                {
                    inventoryDataCaptureType = InventoryDataCaptureType.Mixed;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetInventoryDataCaptureType", ex.Message, ex);
            }
            return inventoryDataCaptureType;
        }

        private LocationManagedType GetLocationManagedType(ThirdPartyOrderCsvViewModelNew csvOrder)
        {
            var locationManagedBy = LocationManagedType.NotSpecified;
            try
            {
                if (string.IsNullOrWhiteSpace(csvOrder.LocationInventoryManagementType))
                {
                    locationManagedBy = LocationManagedType.NotSpecified;
                }
                else if (csvOrder.LocationInventoryManagementType.ToLower().Equals("supplier managed"))
                {
                    locationManagedBy = LocationManagedType.SupplierManaged;
                }
                else if (csvOrder.LocationInventoryManagementType.ToLower().Equals("partial carrier managed"))
                {
                    locationManagedBy = LocationManagedType.PartialCarrierManaged;
                }
                else if (csvOrder.LocationInventoryManagementType.ToLower().Equals("fully carrier managed"))
                {
                    locationManagedBy = LocationManagedType.FullyCarrierManaged;
                }

            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetLocationManagedType", ex.Message, ex);
            }
            return locationManagedBy;
        }

        public async Task<int> GetAssignedCarrierIdByName(int companyId, string carrierName)
        {
            int carrierCompanyId = 0;
            try
            {
                var Id = await Context.DataContext.Companies.Where(t => t.Id != companyId &&
                                                          t.Name.ToLower().Trim() == carrierName.ToLower().Trim() &&
                                                         (t.CompanyTypeId == (int)CompanyType.BuyerSupplierAndCarrier ||
                                                                                      t.CompanyTypeId == (int)CompanyType.Carrier ||
                                                                                      t.CompanyTypeId == (int)CompanyType.SupplierAndCarrier) && t.IsActive)
                                                          .OrderByDescending(t => t.Id)
                                                          .Select(t => t.Id).FirstOrDefaultAsync();
                if (Id > 0)
                {
                    carrierCompanyId = Id;
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetAssignedCarrierIdByName", ex.Message, ex);

            }
            return carrierCompanyId;

        }

        public async Task<int> GetUserIdByEMail(string email)
        {
            int userId = 0;
            try
            {
                var Id = await Context.DataContext.Users.Where(t => t.Email.ToLower().Trim() == email.ToLower().Trim()
                                && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.Carrier || t1.Id == (int)UserRoles.Admin))
                           .Select(t => t.Id).FirstOrDefaultAsync();

                if (Id > 0)
                {
                    userId = Id;
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetUserIdByEMail", ex.Message, ex);

            }
            return userId;

        }

        #region DR creation

        private void AddQueueforDRCreation(ThirdPartyOrderViewModel thirdPartyOrderViewModel, Job buyerJob, FuelRequest fuelRequest, Order buyerOrder, User buyerUser, UserContext userContext)
        {
            
            var request = new RaiseDeliveryRequestInput();
            request.Priority = DeliveryReqPriority.MustGo;
            if (thirdPartyOrderViewModel.FuelDetails.FuelQuantity.QuantityTypeId == 1)
            {
                request.ScheduleQuantityType = (int)ScheduleQuantityType.Quantity;
                request.RequiredQuantity = thirdPartyOrderViewModel.FuelDetails.FuelQuantity.Quantity;
            }
            else if (thirdPartyOrderViewModel.FuelDetails.FuelQuantity.QuantityTypeId == 2)
            {
                request.RequiredQuantity = 0;
                request.ScheduleQuantityType = (int)ScheduleQuantityType.Balance;
            }
            else
            {
                request.RequiredQuantity = 0;
                request.ScheduleQuantityType = (int)ScheduleQuantityType.NotSpecified;
            }
            request.SiteId = buyerJob.DisplayJobID;
            request.JobId = buyerJob.Id;

            int productTypeId = 0;
            if (fuelRequest.MstProduct == null)
            {
                productTypeId = Context.DataContext.MstProducts.Where(t => t.Id == fuelRequest.FuelTypeId).Select(t => t.ProductTypeId).FirstOrDefault();
            }
            else
            {
                productTypeId = fuelRequest.MstProduct.ProductTypeId;
            }
            request.ProductTypeId = productTypeId;
            request.FuelTypeId = fuelRequest.FuelTypeId;
            request.OrderId = buyerOrder.Id;
            request.Terminal = new DropdownDisplayItem();
            if (buyerOrder.TerminalId.HasValue && buyerOrder.TerminalId > 0)
            {
                request.Terminal.Id = buyerOrder.TerminalId.Value;
                if (buyerOrder.MstExternalTerminal != null)
                    request.Terminal.Name = buyerOrder.MstExternalTerminal.Name;
                else
                    request.Terminal.Name = Context.DataContext.MstExternalTerminals.Where(t => t.Id == request.Terminal.Id).Select(t => t.Name).FirstOrDefault();
            }
            request.BadgeNo1 = thirdPartyOrderViewModel.OrderBadgeDetails.BadgeNo1;
            request.BadgeNo2 = thirdPartyOrderViewModel.OrderBadgeDetails.BadgeNo2;
            request.BadgeNo3 = thirdPartyOrderViewModel.OrderBadgeDetails.BadgeNo3;

            request.SupplierCompanyId = userContext.CompanyId;
            request.UserId = userContext.Id;
            request.BuyerCompanyId = (int)buyerUser.CompanyId;
            request.Notes = thirdPartyOrderViewModel.OrderAdditionalDetailsViewModel.DRNotes;
            request.CustomerCompany = buyerUser.Company.Name;
            UoM uoM = ContextFactory.Current.GetDomain<MasterDomain>().GetCompanyDefaultCurrency(userContext.CompanyId);

            if (thirdPartyOrderViewModel.AddressDetails.MarineUoM != UoM.Gallons && thirdPartyOrderViewModel.AddressDetails.MarineUoM != UoM.Litres)
            {
                var RaiseDeliveryRequest = new RaiseDeliveryRequestViewModel();
                if (thirdPartyOrderViewModel.FuelDetails.FuelQuantity.QuantityTypeId == 1)
                {
                    RaiseDeliveryRequest.UoM = (int)thirdPartyOrderViewModel.AddressDetails.MarineUoM;
                    RaiseDeliveryRequest.RequiredQuantity = request.RequiredQuantity;
                    //ContextFactory.Current.GetDomain<FreightServiceDomain>().SetUomConversionForMarine(RaiseDeliveryRequest, uoM, false);
                    //request.UoM = RaiseDeliveryRequest.UoM;
                    //request.RequiredQuantity = RaiseDeliveryRequest.RequiredQuantity;
                }
                else
                {
                    request.UoM = (int)uoM;
                }
            }
            else
            {
                request.UoM = (int)thirdPartyOrderViewModel.AddressDetails.MarineUoM;
            }

            SetMarineNominationRelatedInfo(thirdPartyOrderViewModel, request, userContext);
            if (thirdPartyOrderViewModel.NumOfSubDrs > 0 && thirdPartyOrderViewModel.FuelDetails.FuelQuantity.QuantityTypeId == 1 && request.RequiredQuantity > 0)
            {
                request.NumOfSubDrs = thirdPartyOrderViewModel.NumOfSubDrs.Value;
            }
            string json = JsonConvert.SerializeObject(request);
            var queueRequest = new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = QueueProcessType.DRCreation,
                JsonMessage = json
            };
            var queueId = new QueueMessageDomain().EnqeueMessage(queueRequest);
        }

        private static void SetMarineNominationRelatedInfo(ThirdPartyOrderViewModel thirdPartyOrderViewModel, RaiseDeliveryRequestInput request, UserContext userContext)
        {
            if (thirdPartyOrderViewModel.OrderAdditionalDetailsViewModel != null)
            {
                request.IsMarine = true;
                request.Berth = thirdPartyOrderViewModel.OrderAdditionalDetailsViewModel.Berth;
                if (thirdPartyOrderViewModel.AddressDetails.VessleId != null)
                {
                    var veeselInfo = Task.Run(() => ContextFactory.Current.GetDomain<CompanyDomain>().GetVesselList(userContext.CompanyId)).Result;
                    if (veeselInfo != null && veeselInfo.Any())
                    {
                        request.Vessel = veeselInfo.FirstOrDefault(x => x.Id == thirdPartyOrderViewModel.AddressDetails.VessleId.Value) != null ? veeselInfo.FirstOrDefault(x => x.Id == thirdPartyOrderViewModel.AddressDetails.VessleId.Value).Name : string.Empty;
                    }
                }
                if (thirdPartyOrderViewModel.FuelDeliveryDetails.SingleDeliverySubTypes == SingleDeliverySubTypes.DeliveryDate)
                {
                    request.DeliveryDateStartTime = thirdPartyOrderViewModel.FuelDeliveryDetails.StartDate.ToString(Resource.constFormatDate);
                }
                if (thirdPartyOrderViewModel.FuelDeliveryDetails.SingleDeliverySubTypes == SingleDeliverySubTypes.DeliveryDateRange)
                {
                    request.DeliveryDateStartTime = thirdPartyOrderViewModel.FuelDeliveryDetails.StartDate.ToString(Resource.constFormatDate);
                    if (thirdPartyOrderViewModel.FuelDeliveryDetails.EndDate != null)
                    {
                        request.DeliveryDateStartTime += "-" + thirdPartyOrderViewModel.FuelDeliveryDetails.EndDate.Value.ToString(Resource.constFormatDate);
                    }
                }
                if (!string.IsNullOrEmpty(thirdPartyOrderViewModel.FuelDeliveryDetails.StartTime))
                {
                    request.DeliveryDateStartTime += "(" + thirdPartyOrderViewModel.FuelDeliveryDetails.StartTime;
                }
                if (!string.IsNullOrEmpty(thirdPartyOrderViewModel.FuelDeliveryDetails.EndTime))
                {
                    request.DeliveryDateStartTime += "-" + thirdPartyOrderViewModel.FuelDeliveryDetails.EndTime + ")";
                }
                if (!string.IsNullOrEmpty(thirdPartyOrderViewModel.FuelDeliveryDetails.StartTime) && string.IsNullOrEmpty(thirdPartyOrderViewModel.FuelDeliveryDetails.EndTime))
                {
                    request.DeliveryDateStartTime += ")";
                }
            }
        }

        public async Task<List<string>> CreateDRFromQueueService(RaiseDeliveryRequestInput viewModel)
        {
            var errorInfo = new List<string>();
            using (var tracer = new Tracer("CreateDRFromQueueService", "ThirdPartyOrderDomain"))
            {
                StringBuilder processMessage = new StringBuilder();
                try
                {
                    var response = new StatusViewModel();
                    var freightserviceDomain = new FreightServiceDomain(this);
                    var userContext = new UserContext();
                    var user = Context.DataContext.Users.Where(t => t.Id == viewModel.UserId).FirstOrDefault();
                    if (user != null)
                    {
                        userContext = new UserContext() { Id = user.Id, CompanyId = user.Company.Id, CompanyTypeId = (CompanyType)user.Company.CompanyTypeId };
                    }
                    else
                        userContext = new UserContext() { Id = viewModel.UserId, CompanyId = viewModel.SupplierCompanyId.Value };

                    var createDrStatus = await freightserviceDomain.RaiseDeliveryRequests(new List<RaiseDeliveryRequestInput>() { viewModel }, userContext);
                    if (createDrStatus.StatusCode == Status.Failed)
                    {
                        errorInfo.Add(createDrStatus.StatusMessage);
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = createDrStatus.StatusMessage;
                    }
                }

                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                    {
                        LogManager.Logger.WriteException("CreateDRFromQueueService", "ThirdPartyOrderDomain", ex.Message, ex);
                    }

                    if (processMessage.Length == 0)
                    {
                        processMessage.Append(Constants.RequestError);
                        errorInfo.Add(processMessage.ToString());
                    }
                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                }
                return errorInfo;
            }
        }

        #endregion DR creation 

        public async Task<StatusViewModel> IsValidProductForRegion(int? jobId, int companyId, string regionId, int productId, int tfxProductId)
        {
            var response = new StatusViewModel(Status.Success);
            if ((productId > 0 || tfxProductId > 0))
            {
                try
                {
                    if (jobId > 0 || !string.IsNullOrEmpty(regionId))
                    {
                        var favProduct = await new FreightServiceDomain().GetRegionFavouriteProducts(jobId, regionId, companyId);
                        if (favProduct != null && favProduct.TfxFavProductTypeId != RegionFavProductType.None)
                        {
                            var isProductAvailable = true;
                            if (favProduct.TfxFavProductTypeId == RegionFavProductType.ProductType && favProduct.TfxProductTypeIds != null && favProduct.TfxProductTypeIds.Any())
                            {
                                var productTypeId = Context.DataContext.MstProducts.Where(t => (productId > 0 && t.Id == productId) || (tfxProductId > 0 && t.TfxProductId == tfxProductId))
                                                                                    .Select(t => t.TfxProductId.HasValue ? t.MstTFXProduct.ProductTypeId : t.ProductTypeId).FirstOrDefault();
                                isProductAvailable = favProduct.TfxProductTypeIds.Contains(productTypeId);
                            }
                            else if (favProduct.TfxFavProductTypeId == RegionFavProductType.FuelType && favProduct.TfxFuelTypeIds != null && favProduct.TfxFuelTypeIds.Any())
                            {
                                var tfxProdId = tfxProductId;
                                if (productId > 0)
                                {
                                    tfxProdId = Context.DataContext.MstProducts.Where(t => t.Id == productId).Select(t => t.TfxProductId ?? 0).FirstOrDefault();
                                }
                                isProductAvailable = favProduct.TfxFuelTypeIds.Any(t => t.Id == tfxProdId);
                            }
                            if (!isProductAvailable)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMsgProductNotAvailable;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ThirdPartyOrderDomain", "IsValidProductForRegion", ex.Message, ex);
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMsgProductNotAvailable;
                }
            }
            return response;
        }
    }
}