using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Domain.Mappers.TankRental;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using SiteFuel.Exchange.ViewModels.TankRental;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class TankRentalInvoiceDomain : InvoiceCommonDomain
    {
        public TankRentalInvoiceDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public TankRentalInvoiceDomain(BaseDomain domain) : base(domain)
        {
        }

        private InvoiceCreateRequestViewModel GetInvoiceCreateRequestViewModel(InvoiceCreateViewModel invoiceCreateModel, InvoiceModel invoiceModel)
        {
            invoiceModel.DisplayInvoiceNumber = invoiceModel.DisplayInvoiceNumber.FormattedInvoiceNumber(invoiceModel.InvoiceTypeId);
            var response = new InvoiceCreateRequestViewModel
            {
                JobId = invoiceCreateModel.JobId,
                JobCompanyId = invoiceCreateModel.JobCompanyId,
                OrderId = invoiceCreateModel.OrderId,
                PoNumber = invoiceCreateModel.PoNumber,
                DeliveryTypeId = invoiceCreateModel.DeliveryTypeId,
                OrderMaxQuantity = invoiceCreateModel.MaxQuantity,
                OrderAcceptedBy = invoiceCreateModel.OrderAcceptedBy,
                TimeZoneName = invoiceCreateModel.TimeZoneName,
                BuyerCompanyId = invoiceCreateModel.BuyerCompanyId,
                SupplierCompanyId = invoiceCreateModel.AcceptedCompanyId,
                InvoiceModel = invoiceModel
            };
            return response;
        }

        public async Task<ManualInvoiceViewModel> GetManualInvoiceAsync(int orderId)
        {
            var response = new ManualInvoiceViewModel();
            try
            {
                string SupplierEmail = string.Empty;
                string SupplierPhone = string.Empty;
                string SupplierName = string.Empty;
                string BuyerCompanyName = string.Empty;
                decimal avgGallonsPercentagePerDelivery = 0;
                HelperDomain helperDomain = new HelperDomain(this);
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open);
                //// check if order is closed order and gallon delivery is 0 percent drop/0 gallons drop
                if (order == null)
                {
                    order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId &&
                                                                                       t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed);
                    avgGallonsPercentagePerDelivery = helperDomain.GetAverageFuelDropPercentagePerOrder(order);
                }

                if (order != null || avgGallonsPercentagePerDelivery <= 0)
                {
                    var user = order.FuelRequest.User;
                    if (order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest)
                    {
                        var counterOffer = order.FuelRequest.CounterOffers.FirstOrDefault(t => t.BuyerStatus == (int)CounterOfferStatus.Accepted);
                        if (counterOffer != null)
                        {
                            user = Context.DataContext.Users.SingleOrDefault(t => t.Id == counterOffer.BuyerId);
                        }
                    }
                    SupplierEmail = user.Email;
                    SupplierPhone = user.PhoneNumber;
                    SupplierName = $"{user.FirstName} {user.LastName}";
                    BuyerCompanyName = user.Company.Name;

                    response = new ManualInvoiceViewModel(Status.Success)
                    {
                        OrderId = orderId,
                        FuelRemaining = ((order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity) - order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice).Sum(t => t.DroppedGallons)),
                        PoNumber = order.PoNumber,
                        OrderTypeId = order.FuelRequest.OrderTypeId,
                        SupplierEmail = SupplierEmail,
                        SupplierPhone = SupplierPhone,
                        SupplierName = SupplierName,
                        FuelId = order.FuelRequest.FuelTypeId,
                        FuelType = helperDomain.GetProductName(order.FuelRequest.MstProduct),
                        OrderTotal = order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity,
                        PaymentTermId = order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).PaymentTermId,
                        NetDays = order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).NetDays,
                        IsMulitpleDelivery = order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries,
                        IsBackDatedJob = order.FuelRequest.Job.IsBackdatedJob,
                        TypeofFuel = order.FuelRequest.MstProduct.ProductDisplayGroupId,
                        BuyerCompanyName = BuyerCompanyName,
                        ProductDescription = order.FuelRequest.FuelDescription,
                        FuelRequestId = order.FuelRequest.Id,
                        AvgGallonsPercentagePerDelivery = avgGallonsPercentagePerDelivery,
                        Currency = order.FuelRequest.Currency,
                        UoM = order.FuelRequest.UoM,
                        PricingType = (PricingType)order.FuelRequest.PricingTypeId,
                        QuantityTypeId = order.FuelRequest.QuantityTypeId,
                        IsFTL = order.IsFTL,
                        PaymentMethod = order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).PaymentMethod,
                        TerminalName = order.TerminalId.HasValue ? order.MstExternalTerminal.Name : (order.FuelRequest.TerminalId.HasValue ? order.FuelRequest.MstExternalTerminal.Name : string.Empty)
                    };

                    if (order.ExternalBrokerOrderDetail != null)
                    {
                        response.ExternalBrokerId = order.ExternalBrokerId ?? 0;
                        response.ExternalBrokeredOrder.CustomerId = order.ExternalBrokerId ?? 0;
                        response.ExternalBrokeredOrder = order.ExternalBrokerOrderDetail.ToViewModel();
                        response.ExternalBrokeredOrder.BrokeredOrderFee = order.FuelRequest.FuelRequestFees.ToExternalBrokerViewModel();
                        response.IsThirdPartyHardwareUsed = true;
                    }

                    if (order.ExternalBrokerBuySellDetail != null)
                    {
                        response.ExternalBrokerId = order.ExternalBrokerId ?? 0;
                        response.ExternalBrokeredOrder.CustomerId = order.ExternalBrokerId ?? 0;
                        response.IsBuySellOrder = true;
                    }

                    if (order.IsFTL && order.FuelRequest.FreightOnBoardTypeId.HasValue)
                    {
                        response.IsVariousFobOrigin = order.FuelRequest.FreightOnBoardTypeId == (int)FreightOnBoardTypes.Terminal && order.FuelRequest.Job.LocationType == JobLocationTypes.Various;
                    }

                    if (order.OrderAdditionalDetail != null)
                    {
                        response.Notes = order.OrderAdditionalDetail.Notes;
                    }

                    response.FuelDeliveryDetails.FuelFees.FuelRequestFees = order.FuelRequest.FuelRequestFees.Where(t => t.FeeTypeId == (int)FeeType.ProcessingFee).ToList().ToFeesViewModel();

                    response.FuelDeliveryDetails.FuelFees.Currency = order.FuelRequest.Currency;
                    response.FuelDeliveryDetails.FuelFees.UoM = order.FuelRequest.UoM;
                    response.ExternalBrokeredOrder.BrokeredOrderFee.Currency = order.FuelRequest.Currency;
                    response.ExternalBrokeredOrder.BrokeredOrderFee.UoM = order.FuelRequest.UoM;
                    response.TankFrequency = new TankRentalFrequencyViewModel();
                    if (order.FuelRequest.TankRentals != null)
                    {
                        response.TankFrequency = order.FuelRequest.TankRentals.FirstOrDefault().ToViewModel();
                        response.TankFrequency.Tanks.ForEach(t => { t.UoM = order.FuelRequest.UoM; t.Currency = order.FuelRequest.Currency; t.IsToBeIncludedInInvoice = true; t.BillingFrequencyId = response.TankFrequency.TankRentalFrequencyId; });
                        response.TankRentalFrequencyTypes = new List<DropdownDisplayItem>();
                        response.TankRentalFrequencyTypes = order.FuelRequest.TankRentals
                        .Where(t => t.ActivationStatusId == (int)ActivationStatus.Active).Select(t => new DropdownDisplayItem()
                        {
                            Id = t.FrequencyTypeId,
                            Name = ((FrequencyTypes)t.FrequencyTypeId).GetDisplayName()
                        }).GroupBy(t => t.Id).Select(g => g.First()).ToList();
                    }
                    response.InvoiceTypeId = (int)InvoiceType.TankRental;
                    response = response.CorrectValues();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankRentalInvoiceDomain", "GetManualInvoiceAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<InvoiceCreateResponseViewModel> CreateTankRentalInvoiceAsync(UserContext userContext, ManualInvoiceViewModel invoiceViewModel)
        {
            InvoiceCreateResponseViewModel invoiceCreateResponse = new InvoiceCreateResponseViewModel();
            using (var tracer = new Tracer("TankRentalInvoiceDomain", "CreateTankRentalInvoiceAsync"))
            {
                try
                {
                    InvoiceModel invoiceModel = new InvoiceModel();

                    if (invoiceViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees == null)
                    {
                        invoiceViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = new List<FeesViewModel>();
                    }

                    var manualInvoiceCreateRequestViewModel = await GetCreateManualInvoiceRequestViewModelAsync(invoiceViewModel, invoiceModel);

                    //------------Set Invoice fees from manual invoice view model------------------------
                    var invoiceCreateRequestViewModel = GetInvoiceCreateRequestViewModel(manualInvoiceCreateRequestViewModel, invoiceModel);
                    invoiceCreateResponse = await GenerateManualInvoice(invoiceCreateRequestViewModel, invoiceViewModel);

                    await SetTankRentalInvoiceCreatedPostEvents(userContext, manualInvoiceCreateRequestViewModel, invoiceModel, invoiceCreateResponse);
                    invoiceCreateResponse.StatusMessage = Resource.valMessageTankRentalInvoiceCreatedSuccessfully;
                }
                catch (Exception ex)
                {
                    invoiceCreateResponse.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                    LogManager.Logger.WriteException("TankRentalInvoiceDomain", "CreateTankRentalInvoiceAsync", ex.Message, ex);
                }
            }
            return invoiceCreateResponse;
        }

        public async Task<InvoiceCreateResponseViewModel> RebillTankRentalInvoiceAsync(UserContext userContext, ManualInvoiceViewModel invoiceViewModel)
        {
            var invoiceCreateResponse = new InvoiceCreateResponseViewModel();
            using (var tracer = new Tracer("TankRentalInvoiceDomain", "RebillTankRentalInvoiceAsync"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        InvoiceModel invoiceModel = new InvoiceModel();

                        if (invoiceViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees == null)
                        {
                            invoiceViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = new List<FeesViewModel>();
                        }

                        var manualInvoiceCreateRequestViewModel = await GetCreateManualInvoiceRequestViewModelAsync(invoiceViewModel, invoiceModel);

                        //------------Set Invoice fees from manual invoice view model------------------------
                        var invoiceCreateRequestViewModel = GetInvoiceCreateRequestViewModel(manualInvoiceCreateRequestViewModel, invoiceModel);
                        invoiceCreateResponse = await TankRentalRebillInvoice(invoiceCreateRequestViewModel, invoiceViewModel);
                        StatusViewModel creditInvoiceResponse = AddCreditInvoiceToQueueServiceAsync(invoiceViewModel.InvoiceId, null, userContext.Id);
                        transaction.Commit();
                        invoiceCreateResponse.StatusCode = Status.Success;
                        await SetTankRentalInvoiceRebilledPostEvents(userContext, manualInvoiceCreateRequestViewModel, invoiceModel, invoiceCreateResponse);

                        invoiceCreateResponse.StatusMessage = Resource.errMessageRebilledInvoiceUpdatedSuccess;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        invoiceCreateResponse.StatusMessage = Resource.errMessageCreditRebillFailed;
                        LogManager.Logger.WriteException("TankRentalInvoiceDomain", "RebillTankRentalInvoiceAsync", ex.Message, ex);
                    }
                }
            }
            return invoiceCreateResponse;
        }

        private async Task<InvoiceCreateResponseViewModel> GenerateManualInvoice(InvoiceCreateRequestViewModel invoiceCreateRequestViewModel, ManualInvoiceViewModel invoiceViewModel)
        {
            var brokeredinvoiceCreateRequestModels = new InvoiceCreateRequestViewModel();
            var invoiceCreateResponses = await CreateTankRentalInvoiceAsync(invoiceCreateRequestViewModel, brokeredinvoiceCreateRequestModels, invoiceViewModel);
            return invoiceCreateResponses;
        }

        private async Task<InvoiceCreateResponseViewModel> GenerateAutoTankRentalInvoice(InvoiceCreateRequestViewModel invoiceCreateRequestViewModel, ManualInvoiceViewModel invoiceViewModel)
        {
            var brokeredinvoiceCreateRequestModels = new InvoiceCreateRequestViewModel();
            var invoiceCreateResponses = await CreateAutoTankRentalInvoiceAsync(invoiceCreateRequestViewModel, brokeredinvoiceCreateRequestModels, invoiceViewModel);
            return invoiceCreateResponses;
        }

        private async Task<ManualInvoiceCreateRequestViewModel> GetCreateManualInvoiceRequestViewModelAsync(ManualInvoiceViewModel manualInvoiceModel, InvoiceModel invoiceModel)
        {
            var viewModel = new ManualInvoiceCreateRequestViewModel();
            try
            {
                viewModel = manualInvoiceModel.ToManualInvoiceViewModel();
                var order = await Context.DataContext.Orders.Where(t => t.Id == manualInvoiceModel.OrderId)
                            .Select(t => new
                            {
                                t.PoNumber,
                                t.BuyerCompanyId,
                                t.AcceptedCompanyId,
                                t.ExternalBrokerId,
                                t.ExternalBrokerOrderDetail,
                                t.AcceptedBy,
                                t.IsEndSupplier,
                                IsBuySellOrder = t.ExternalBrokerBuySellDetail != null,
                                t.ExternalBrokerBuySellDetail,
                                t.IsFTL,
                                Job = new
                                {
                                    JobAddress = new AddressViewModel
                                    {
                                        Address = t.FuelRequest.Job.Address,
                                        City = t.FuelRequest.Job.City,
                                        StateCode = t.FuelRequest.Job.MstState.Code,
                                        CountryCode = t.FuelRequest.Job.MstCountry.Code,
                                        ZipCode = t.FuelRequest.Job.ZipCode,
                                        CountyName = t.FuelRequest.Job.CountyName
                                    },
                                    t.FuelRequest.Job.StateId,
                                    t.FuelRequest.Job.MstState.QuantityIndicatorTypeId,
                                    t.FuelRequest.Job.ZipCode,
                                    t.FuelRequest.Job.TimeZoneName,
                                    t.FuelRequest.Job.CompanyId,
                                    t.FuelRequest.Job.IsApprovalWorkflowEnabled,
                                    t.FuelRequest.Job.Id,
                                    t.FuelRequest.Job.LocationType,
                                    CompanyName = t.FuelRequest.Job.Company.Name,
                                    CountryCurrency = t.FuelRequest.Job.MstCountry.Currency,
                                    t.FuelRequest.Job.JobBudget.IsAssetTracked,
                                    BuyerTaxExemptLicence = t.FuelRequest.Job.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                    ApprovalUser = t.FuelRequest.Job.JobXApprovalUsers.FirstOrDefault(t1 => t1.IsActive),
                                    t.FuelRequest.Job.JobBudget.IsTaxExempted
                                },
                                FuelRequest = new
                                {
                                    t.FuelRequest.Id,
                                    t.FuelRequest.JobId,
                                    t.FuelRequest.UoM,
                                    t.FuelRequest.Currency,
                                    t.FuelRequest.FuelTypeId,
                                    t.FuelRequest.MstProduct.ProductDisplayGroupId,
                                    t.FuelRequest.MstProduct.MappedParentId,
                                    t.FuelRequest.MstProduct.ProductCode,
                                    t.FuelRequest.PricingTypeId,
                                    t.FuelRequest.FuelDescription,
                                    t.FuelRequest.MaxQuantity,
                                    t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                    t.FuelRequest.FuelRequestDetail.StartDate,
                                    t.FuelRequest.FuelRequestDetail.EndTime,
                                    t.FuelRequest.FreightOnBoardTypeId
                                },
                                t.FuelRequest.FuelRequestPricingDetail,
                            }).FirstOrDefaultAsync();
                if (order != null)
                {
                    var timeZoneName = order.Job.TimeZoneName;
                    viewModel.UoM = order.FuelRequest.UoM;
                    viewModel.Currency = order.FuelRequest.Currency;
                    viewModel.PoNumber = order.PoNumber;
                    viewModel.TimeZoneName = timeZoneName;
                    viewModel.FuelTypeId = order.FuelRequest.FuelTypeId;
                    viewModel.MappedParentFuelTypeId = order.FuelRequest.MappedParentId;
                    viewModel.FuelProductCode = order.FuelRequest.ProductCode;
                    viewModel.TypeOfFuel = order.FuelRequest.ProductDisplayGroupId;

                    viewModel.JobCompanyName = order.Job.CompanyName;
                    viewModel.IsBuySellOrder = order.IsBuySellOrder;
                    if (viewModel.IsBuySellOrder)
                    {
                        viewModel.BrokerMarkUp = order.ExternalBrokerBuySellDetail.BrokerMarkUp;
                        viewModel.SupplierMarkUp = order.ExternalBrokerBuySellDetail.SupplierMarkUp;
                    }
                    viewModel.CountryCurrency = order.Job.CountryCurrency;
                    viewModel.BuyerCompanyId = order.BuyerCompanyId;
                    viewModel.JobId = order.FuelRequest.JobId;
                    viewModel.JobStateId = order.Job.StateId;
                    viewModel.JobCompanyId = order.Job.CompanyId;
                    viewModel.AcceptedCompanyId = order.AcceptedCompanyId;
                    viewModel.DeliveryStartDate = order.FuelRequest.StartDate;
                    viewModel.DeliveryEndTime = order.FuelRequest.EndTime;

                    viewModel.OrderAcceptedBy = order.AcceptedBy;
                    if (order.Job.ApprovalUser != null)
                    {
                        viewModel.ApprovalUserId = order.Job.ApprovalUser.Id;
                        viewModel.ApprovalUserOnboardedType = order.Job.ApprovalUser.User.OnboardedTypeId;
                        viewModel.ApprovalUserName = $"{order.Job.ApprovalUser.User.FirstName} {order.Job.ApprovalUser.User.LastName}";
                    }
                    viewModel.IsApprovalWorkflowEnabledForJob = order.Job.IsApprovalWorkflowEnabled;
                    viewModel.ProductDescription = order.FuelRequest.FuelDescription;
                    viewModel.ExternalBrokerId = order.ExternalBrokerId ?? 0;
                    viewModel.IsThirdPartyHardwareUsed = order.ExternalBrokerOrderDetail != null;
                    if (order.ExternalBrokerOrderDetail != null)
                    {
                        viewModel.ExternalBrokeredOrder = order.ExternalBrokerOrderDetail.ToViewModel(viewModel.ExternalBrokeredOrder);
                    }
                    viewModel.IsBrokeredChainOrder = order.BuyerCompanyId != order.Job.CompanyId;
                    viewModel.JobAddess = order.Job.JobAddress;

                    viewModel.IsFTL = order.IsFTL;
                    if (order.FuelRequestPricingDetail != null)
                    {
                        viewModel.FuelRequestPricingDetail = order.FuelRequestPricingDetail.ToViewModel(viewModel.FuelRequestPricingDetail);
                    }
                    viewModel.SupplierPreferredInvoiceTypeId = (int)InvoiceType.Manual;
                    SetInputsToCreateManualInvoiceModel(viewModel, invoiceModel);
                    var payduedatebasis = Context.DataContext.OnboardingPreferences.Where(t => t.IsActive && t.CompanyId == order.AcceptedCompanyId)
                                            .Select(t => t.PaymentDueDateType).FirstOrDefault();
                    await GetInvoiceViewModel(viewModel, invoiceModel, payduedatebasis);
                    viewModel.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreateDomain", "GetCreateManualInvoiceRequestViewModelAsync", ex.Message, ex);
            }
            return viewModel;
        }

        private void SetInputsToCreateManualInvoiceModel(ManualInvoiceCreateRequestViewModel manualInvoiceModel, InvoiceModel invoiceModel)
        {
            var offset = manualInvoiceModel.DropEndDate.GetOffset(manualInvoiceModel.TimeZoneName);
            invoiceModel.DropStartDate = manualInvoiceModel.DropStartDate.AttachOffset(offset);
            invoiceModel.DropEndDate = manualInvoiceModel.DropEndDate.AttachOffset(offset);

            invoiceModel.DroppedGallons = manualInvoiceModel.FuelDropped ?? 0.0M;
            invoiceModel.IsWetHosingDelivery = manualInvoiceModel.IsWetHosingDelivery;
            invoiceModel.IsOverWaterDelivery = manualInvoiceModel.IsOverWaterDelivery;

            invoiceModel.OrderId = manualInvoiceModel.OrderId;
            invoiceModel.PoNumber = manualInvoiceModel.PoNumber;
            invoiceModel.InvoiceTypeId = manualInvoiceModel.InvoiceTypeId;
            invoiceModel.PaymentTermId = manualInvoiceModel.PaymentTermId;
            invoiceModel.NetDays = manualInvoiceModel.NetDays;
            invoiceModel.FilePath = manualInvoiceModel.CsvFilePath;
            if (manualInvoiceModel.IsVariousFobOrigin || !string.IsNullOrEmpty(manualInvoiceModel.AdditionalDetail?.SplitLoadChainId))
            {
                invoiceModel.FuelDropLocation = manualInvoiceModel.DropLocation;
                SetFirstZipCodeOfState(manualInvoiceModel.DropLocation.StateId, manualInvoiceModel.DropLocation.StateCode, out string stateCode);
                invoiceModel.FuelDropLocation.StateCode = stateCode;
            }
        }

        public async Task GetInvoiceViewModel(InvoiceCreateViewModel invoiceRequestModel, InvoiceModel invoiceModel, PaymentDueDateType paymentDueDateType)
        {
            var timeZoneName = invoiceRequestModel.TimeZoneName;
            invoiceModel.UoM = invoiceRequestModel.UoM;
            invoiceModel.Currency = invoiceRequestModel.Currency;
            invoiceModel.SupplierPreferredInvoiceTypeId = invoiceRequestModel.SupplierPreferredInvoiceTypeId;

            CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
            invoiceModel.ExchangeRate = currencyRateDomain.GetCurrencyRate(invoiceRequestModel.Currency, Currency.USD, DateTimeOffset.Now);
            invoiceModel.PaymentDueDate = GetPaymentDueDate(invoiceRequestModel.PaymentTermId, invoiceRequestModel.NetDays, timeZoneName, invoiceModel.DropEndDate, paymentDueDateType);
            invoiceModel.CreatedBy = invoiceRequestModel.UserId;

            var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
            invoiceModel.CreatedDate = currentDate;
            invoiceModel.UpdatedBy = invoiceRequestModel.UserId;
            invoiceModel.UpdatedDate = currentDate;
            invoiceModel.TerminalId = invoiceRequestModel.TerminalId;
            invoiceModel.CityGroupTerminalId = invoiceRequestModel.CityGroupTerminalId;

            // Set invoice type and waiting action if approval workflow is enabled
            invoiceModel.InvoiceTypeId = (int)InvoiceType.TankRental;
            ProcessInvoiceFuelFeesAndSetCalculatedValues(invoiceRequestModel, invoiceModel, invoiceModel.DropEndDate, 0);
            await SetInvoiceAdditionDetailToInvoiceModel(invoiceModel, 0, invoiceRequestModel.OrderId, invoiceRequestModel.IsBuySellOrder);

            // set tank details into invoice fees here
            invoiceModel.TankFrequency = invoiceRequestModel.TankFrequency;
            if (invoiceRequestModel.TankFrequency.Tanks != null && invoiceRequestModel.TankFrequency.Tanks.Any())
            {
                foreach (var item in invoiceRequestModel.TankFrequency.Tanks)
                {
                    if (item.FeeTaxDetails != null && item.FeeTaxDetails.Amount.HasValue)
                    {
                        invoiceModel.BaseTotalTaxAmount += Math.Round(item.FeeTaxDetails.Amount.Value, ApplicationConstants.InvoiceTaxTotalAmountDecimalDisplay);
                    }
                    invoiceModel.BasicAmount += Math.Round(item.RentalFee, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);
                }
            }

            if (invoiceModel.AdditionalDetail == null)
            {
                invoiceModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
            }

            if (invoiceRequestModel.AdditionalDetail != null && !string.IsNullOrWhiteSpace(invoiceRequestModel.AdditionalDetail.Notes))
            {
                invoiceModel.AdditionalDetail.Notes = invoiceRequestModel.AdditionalDetail.Notes;
            }
            invoiceModel.AdditionalDetail.PaymentMethod = invoiceRequestModel.AdditionalDetail.PaymentMethod;
            invoiceModel.AdditionalDetail.TankFrequencyId = invoiceRequestModel.AdditionalDetail.TankFrequencyId;
            var invoiceNumber = await GenerateInvoiceNumber();
            invoiceModel.InvoiceNumberId = invoiceNumber.Id;
            invoiceModel.DisplayInvoiceNumber = invoiceNumber.Number;
        }

        public async Task<List<TankDetailsViewModel>> GetRentalTanks(int billingFrequencyId, int fuelRequestId)
        {
            List<TankDetailsViewModel> tanks = new List<TankDetailsViewModel>();
            var orderTanks = await Context.DataContext.TankDetails
                            .Where(t => t.RentalFrequency.FrequencyTypeId == billingFrequencyId
                            && t.RentalFrequency.FuelRequestId == fuelRequestId
                            && t.ActivationStatusId == (int)ActivationStatus.Active).ToListAsync();

            foreach (var item in orderTanks)
            {
                var tank = item.ToViewModel();
                tank.IsToBeIncludedInInvoice = true;
                tanks.Add(tank);
            }

            var currency = await Context.DataContext.FuelRequests.Where(t => t.Id == fuelRequestId).Select(t => t.Currency).ToListAsync();
            if (currency != null && currency.Any())
            {
                tanks.ForEach(t => t.Currency = currency.First());
            }

            return tanks;
        }

        public async Task<bool> DeactivateTankRentalFrequencies(int fuelRequestId, int userId)
        {
            var response = false;
            try
            {
                var result = Context.DataContext.Database
                            .ExecuteSqlCommand("UPDATE FuelRequestTankRentalFrequencies SET ActivationStatusId = {0}, DeactivationDate = {1}, UpdatedBy = {2}, UpdatedDate= {3} WHERE FuelRequestId = {4}"
                                , (int)ActivationStatus.Deleted, DateTimeOffset.Now, userId, DateTimeOffset.Now, fuelRequestId);

                var frequencyIds = await Context.DataContext.FuelRequestTankRentalFrequencies
                                    .Where(t => t.FuelRequestId == fuelRequestId).Select(t => t.Id).ToListAsync();

                foreach (var frequencyId in frequencyIds)
                {
                    Context.DataContext.Database
                    .ExecuteSqlCommand("UPDATE TankDetails SET ActivationStatusId = {0}, DeactivationDate = {1}, UpdatedBy = {2}, UpdatedDate= {3} WHERE RentalFrequencyId = {4}"
                     , (int)ActivationStatus.Deleted, DateTimeOffset.Now, userId, DateTimeOffset.Now, frequencyId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankRentalInvoiceDomain", "DeactivateTankRentalFrequencies", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> AddTankRentalInvoiceCreateMessage(int userId, int orderId)
        {
            // This method should be called only after order has been closed
            var response = false;
            try
            {
                
                //var queueMsgs = await spDomain.GetActiveTankRentalInvoiceMassage(currentDate, (int)OrderStatus.Closed, orderId);
                var queueMsgs = await GetTankRentals(orderId, IsClosedOrder: true);
                if (queueMsgs.Any())
                {
                    response = SaveTankRentalInvoiceCreateMessage(queueMsgs, (int)SystemUser.System);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankRentalInvoiceDomain", "AddTankRentalInvoiceCreateMessage", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> AutoAddTankRentalInvoiceCreateMessage()
        {
            var response = false;
            try
            {
                //var queueMsgs = await spDomain.GetActiveTankRentalInvoiceMassage(currentDate, (int)OrderStatus.Open);
                var queueMsgs = await GetTankRentals(OrderStatus: (int)OrderStatus.Open);
                if (queueMsgs.Any())
                {
                    response = SaveTankRentalInvoiceCreateMessage(queueMsgs, (int)SystemUser.System);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankRentalInvoiceDomain", "AutoAddTankRentalInvoiceCreateMessage", ex.Message, ex);
            }
            return response;
        }

        private async Task<List<TankRentalQueueMessage>> GetTankRentals(int orderId = 0, int OrderStatus = (int)OrderStatus.Open, bool IsClosedOrder = false)
        {
            var response = new List<TankRentalQueueMessage>();
            try
            {
                var tankFrequencies = await Context.DataContext.FuelRequestTankRentalFrequencies
                                .Where(t => t.ActivationStatusId == (int)ActivationStatus.Active && t.FuelRequest.Orders.Any()
                                            && ((orderId == 0 && t.FuelRequest.Orders.Any(t1 => t1.OrderXStatuses.Any(t2 => t2.IsActive && t2.StatusId == OrderStatus)))
                                                || t.FuelRequest.Orders.Any(t1 => t1.Id == orderId))
                                            && t.TankDetails.Any(t1 => t1.RentalFee > 0))
                                .Select(t =>
                                new
                                {
                                    TankFrequency = t,
                                    OrderId = t.FuelRequest.Orders.FirstOrDefault().Id,
                                    t.FuelRequest.Job.TimeZoneName,
                                    UserFirstName = t.FuelRequest.Orders.FirstOrDefault().User.FirstName,
                                    UserLastName = t.FuelRequest.Orders.FirstOrDefault().User.LastName,
                                    CompanyName = t.FuelRequest.Orders.FirstOrDefault().User.Company.Name,
                                    lastInvoiceDate = t.FuelRequest.Orders.Select(t1 => t1.Invoices.Where(t2 => t2.InvoiceXAdditionalDetail.TankFrequencyId == t.Id
                                                                                                        && t2.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                                                                        && t2.InvoiceXAdditionalDetail.Notes == "Auto generated Tank Rental Invoice")
                                                                                                        .OrderByDescending(t2 => t2.Id)
                                                                                                        .Select(t2 => t2.DropEndDate).FirstOrDefault()
                                                                                  ).FirstOrDefault()
                                })
                                .ToListAsync();

                var filterWithEndDate = tankFrequencies.Where(t => t.TankFrequency.TankDetails.Any(t1 => !t1.EndDate.HasValue || t1.EndDate.Value >= DateTimeOffset.Now.ToTargetDateTimeOffset(t.TimeZoneName))).ToList();

                foreach (var item in filterWithEndDate)
                {
                    var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(item.TimeZoneName);
                    if (item.lastInvoiceDate.Date == currentDate.Date)
                    { // how to check is invoice created from web service
                        continue;
                    }

                    var tankFrequency = item.TankFrequency;
                    var tankRentalStartDate = tankFrequency.ScheduleStartDate;
                    var tankRentalEndDate = GetTankScheduleEndDate(tankFrequency.ScheduleStartDate, tankFrequency.FrequencyTypeId);

                    if ((currentDate.Date >= tankRentalStartDate.Date && currentDate.Date >= tankRentalEndDate.Date && tankRentalStartDate.Date != new DateTime().Date) || IsClosedOrder)
                    {
                        var intervalList = GetListForPendingTankRentalInvoices(tankFrequency.FrequencyTypeId, currentDate, tankRentalStartDate, item.lastInvoiceDate, IsClosedOrder);
                        var intervalEndDates = intervalList.Select(t => t.EndDate).ToList();
                        if (IsValidTankSchedule(currentDate, item.lastInvoiceDate, intervalEndDates))
                        {
                            //get intervals for schedules to create rental invoices
                            foreach (var tankInterval in intervalList)
                            {
                                var queueMsg = new TankRentalQueueMessage
                                {
                                    OrderId = item.OrderId,
                                    RentalFrequencyId = item.TankFrequency.Id,
                                    TimeZoneName = item.TimeZoneName,
                                    UserFirstName = item.UserFirstName,
                                    UserLastName = item.UserLastName,
                                    CompanyName = item.CompanyName,
                                    IsClosedOrder = IsClosedOrder,
                                    StartDate = tankInterval.StartDate,
                                    EndDate = tankInterval.EndDate
                                };
                                response.Add(queueMsg);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankRentalInvoiceDomain", "GetTankRentals", ex.Message, ex);
            }
            return response;
        }

        public bool SaveTankRentalInvoiceCreateMessage(List<TankRentalQueueMessage> queueMsgs, int userId)
        {
            var response = false;
            try
            {
                foreach (var item in queueMsgs)
                {
                    var jsonMessage = JsonConvert.SerializeObject(item);
                    var message = new QueueMessageViewModel()
                    {
                        QueueProcessType = QueueProcessType.TankRentalInvoice,
                        CreatedBy = userId,
                        UpdatedBy = userId,
                        JsonMessage = jsonMessage
                    };
                    QueueMessageDomain queueMessageDomain = new QueueMessageDomain(this);
                    queueMessageDomain.EnqeueMessage(message);
                }
                response = true;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankRentalInvoiceDomain", "SaveTankRentalInvoiceCreateMessage", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> AutoGenerateTankRentalInvoice1(TankRentalQueueMessage tankRental)
        {
            bool response = false;
            try
            {
                //will convert to SP to get invoice date as well
                var tankFrequency = await Context.DataContext.FuelRequestTankRentalFrequencies
                                    .FirstOrDefaultAsync(t => t.Id == tankRental.RentalFrequencyId);

                if (tankFrequency != null)
                {
                    if (IsValidFrequencyToGenerateInvoice(tankFrequency))
                    {
                        var tankRentalViewModel = tankFrequency.ToViewModel();
                        var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(tankRental.TimeZoneName);
                        var tankRentalStartDate = tankFrequency.ScheduleStartDate;
                        tankRentalViewModel.StartDate = tankRentalStartDate;

                        ManualInvoiceViewModel invoiceViewModel = new ManualInvoiceViewModel();
                        InvoiceModel invoiceModel = new InvoiceModel();

                        SetTankRentalFeesToInvoice(invoiceModel, tankRentalViewModel, currentDate, tankRental.StartDate, tankRental.EndDate);

                        if (invoiceModel.BasicAmount > 0)
                        {
                            var manualInvoiceCreateRequestViewModel = await GetCreateViewModelForAutoTankInvoiceAsync(tankRental.OrderId, tankRentalViewModel, invoiceModel, invoiceViewModel, tankRental.EndDate);
                            //-------------Set tank rental amount to Invoice

                            //------------Set Invoice fees from manual invoice view model------------------------
                            var invoiceCreateRequestViewModel = GetInvoiceCreateRequestViewModel(manualInvoiceCreateRequestViewModel, invoiceModel);
                            var invoiceCreateResponse = await GenerateAutoTankRentalInvoice(invoiceCreateRequestViewModel, invoiceViewModel);
                            UserContext userContext = new UserContext()
                            {
                                Id = invoiceModel.CreatedBy,
                                CompanyId = invoiceCreateRequestViewModel.SupplierCompanyId,
                                CompanyName = tankRental.CompanyName,
                                Name = $"{tankRental.UserFirstName} {tankRental.UserLastName}",
                                UserName = $"{tankRental.UserFirstName} {tankRental.UserLastName}"
                            };
                            await SetTankRentalInvoiceCreatedPostEvents(userContext, manualInvoiceCreateRequestViewModel, invoiceModel, invoiceCreateResponse);
                            if (invoiceCreateResponse.StatusCode == Status.Success)
                            {
                                if (tankRental.IsClosedOrder)
                                {
                                    await DeactivateTankRentalFrequencies(tankFrequency.FuelRequestId, (int)SystemUser.System);
                                }
                                LogManager.Logger.WriteDebug("TankRentalInvoiceDomain", "AutoGenerateTankRentalInvoices", "Auto Tank rental invoice created for -" + tankFrequency.Id);
                                response = true;
                            }
                            else
                            {
                                LogManager.Logger.WriteDebug("TankRentalInvoiceDomain", "AutoGenerateTankRentalInvoices", "Failed to create Auto Tank rental invoice for -" + tankFrequency.Id);
                            }
                        }
                        else
                        {
                            LogManager.Logger.WriteDebug("TankRentalInvoiceDomain", "AutoGenerateTankRentalInvoices", "Failed to create Auto Tank rental invoice for -" + tankFrequency.Id + " as Tax amt is < 0");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankRentalInvoiceDomain", "AutoGenerateTankRentalInvoices", ex.Message, ex);
            }
            return response;
        }

        private bool IsValidFrequencyToGenerateInvoice(FuelRequestTankRentalFrequency tankFrequency)
        {
            bool isValidFrequency = true;
            var lastInvoiceDate = Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.TankFrequencyId == tankFrequency.Id && t.InvoiceXAdditionalDetail.Notes == "Auto generated Tank Rental Invoice")
                                    .OrderBy(t => t.DropEndDate)
                                    .Select(t => new { t.DropEndDate }).FirstOrDefault();
            if (lastInvoiceDate != null)
            {
                var daysFromLastInvoice = (DateTimeOffset.Now.Date - lastInvoiceDate.DropEndDate).Days;
                switch (tankFrequency.FrequencyTypeId)
                {
                    case (int)FrequencyTypes.Daily:
                        isValidFrequency = !((1 - daysFromLastInvoice) >= 1);
                        break;
                    case (int)FrequencyTypes.Weekly:
                        isValidFrequency = !((7 - daysFromLastInvoice) >= 1);
                        break;
                    case (int)FrequencyTypes.Biweekly:
                        isValidFrequency = !((14 - daysFromLastInvoice) >= 1);
                        break;
                    case (int)FrequencyTypes.Monthly:
                        isValidFrequency = !((DateTimeOffset.Now.LastDayOfMonth().Day - 1 - daysFromLastInvoice) >= 1);
                        break;
                    default:
                        break;
                }
            }
            return isValidFrequency;
        }

        private List<TankRentalInterval> GetListForPendingTankRentalInvoices(int frequencyTypeId, DateTimeOffset currentDate, DateTimeOffset tankRentalStartDate
                                                                            , DateTimeOffset scheduleLastInvoiceDate, bool IsClosedOrder)
        {
            var intervalList = new List<TankRentalInterval>();
            int intervalCount = 0;
            try
            {
                DateTimeOffset newintervalEndDate = scheduleLastInvoiceDate.Date != new DateTimeOffset().Date ? scheduleLastInvoiceDate.AddDays(1) : tankRentalStartDate;
                var newIntervalStartDate = newintervalEndDate;

                var endDate = GetTankScheduleEndDate(newintervalEndDate, frequencyTypeId);
                if (IsClosedOrder && currentDate.Date < endDate.Date)
                {
                    newintervalEndDate = currentDate;
                }
                else
                {
                    newintervalEndDate = endDate;
                }

                switch (frequencyTypeId)
                {
                    case (int)FrequencyTypes.Daily:
                        while (currentDate.Date >= newintervalEndDate.Date && intervalCount < 31)
                        {
                            intervalList.Add(AddTankINterval(newIntervalStartDate, newintervalEndDate, currentDate, IsClosedOrder));
                            intervalCount++;
                            newIntervalStartDate = newintervalEndDate.Date.AddDays(1);
                            newintervalEndDate = newintervalEndDate.Date.AddDays(1);
                        }
                        break;

                    case (int)FrequencyTypes.Weekly:
                        while (currentDate.Date >= newintervalEndDate.Date && intervalCount < 31)
                        {
                            intervalList.Add(AddTankINterval(newIntervalStartDate, newintervalEndDate, currentDate, IsClosedOrder));
                            intervalCount++;
                            newIntervalStartDate = newintervalEndDate.Date.AddDays(1);
                            newintervalEndDate = newintervalEndDate.Date.AddDays(7);
                        }
                        break;

                    case (int)FrequencyTypes.Biweekly:
                        while (currentDate.Date >= newintervalEndDate.Date && intervalCount < 31)
                        {
                            intervalList.Add(AddTankINterval(newIntervalStartDate, newintervalEndDate, currentDate, IsClosedOrder));
                            intervalCount++;
                            newIntervalStartDate = newintervalEndDate.Date.AddDays(1);
                            newintervalEndDate = newintervalEndDate.Date.AddDays(14);
                        }
                        break;

                    case (int)FrequencyTypes.Monthly:
                        while (currentDate.Date >= newintervalEndDate.Date && intervalCount < 31)
                        {
                            intervalList.Add(AddTankINterval(newIntervalStartDate, newintervalEndDate, currentDate, IsClosedOrder));
                            intervalCount++;
                            newIntervalStartDate = newintervalEndDate.Date.AddDays(1);
                            newintervalEndDate = newintervalEndDate.AddDays(newIntervalStartDate.LastDayOfMonth().Day);
                        }
                        break;

                    default:
                        break;
                }

                if (intervalCount > 31)
                {
                    intervalList.Clear();
                    LogManager.Logger.WriteException("TankRentalInvoiceDomain", "GetListForPendingTankRentalInvoices", "Auto Count is more than 31 -" + " FrequencyId - " + frequencyTypeId + " currentDate -" + currentDate + " TankStartDate - " + tankRentalStartDate, new Exception());
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankRentalInvoiceDomain", "GetListForPendingTankRentalInvoices", ex.Message + " FrequencyId - " + frequencyTypeId + " currentDate -" + currentDate + " TankStartDate - " + tankRentalStartDate, ex);
            }
            return intervalList;
        }

        private TankRentalInterval AddTankINterval(DateTimeOffset StartDate, DateTimeOffset EndDate, DateTimeOffset CurrentDate, bool IsClosedOrder)
        {
            var response = new TankRentalInterval();
            if (IsClosedOrder && CurrentDate < EndDate)
            {
                response.StartDate = StartDate;
                response.EndDate = CurrentDate;
            }
            else
            {
                response.StartDate = StartDate;
                response.EndDate = EndDate;
            }
            return response;
        }

        private bool IsValidTankSchedule(DateTimeOffset currentDate, DateTimeOffset scheduleLastInvoiceDate, List<DateTimeOffset> intervalList)
        {
            if (scheduleLastInvoiceDate.Date == new DateTimeOffset().Date)
                return true;

            return currentDate.Date >= scheduleLastInvoiceDate.Date && intervalList.Contains(currentDate.Date);
        }

        private DateTimeOffset GetTankScheduleEndDate(DateTimeOffset scheduleStartDate, int frequencyTypeId)
        {
            var endDate = scheduleStartDate;
            switch (frequencyTypeId)
            {
                //daily already considered as defualt.
                case (int)FrequencyTypes.Weekly:
                    endDate = scheduleStartDate.FirstDayOfWeek().AddDays(6);
                    break;

                case (int)FrequencyTypes.Biweekly:
                    endDate = scheduleStartDate.FirstDayOfWeek().AddDays(13);
                    break;

                case (int)FrequencyTypes.Monthly:
                    endDate = scheduleStartDate.LastDayOfMonth();
                    break;

                default:
                    break;
            }
            return endDate;
        }

        private async Task<ManualInvoiceCreateRequestViewModel> GetCreateViewModelForAutoTankInvoiceAsync(int orderId, TankRentalFrequencyViewModel tankRentalFrequency, InvoiceModel invoiceModel, ManualInvoiceViewModel invoiceViewModel, DateTimeOffset tankIntervalEndDate)
        {
            var viewModel = new ManualInvoiceCreateRequestViewModel();

            try
            {
                var order = await Context.DataContext.Orders.Where(t => t.Id == orderId
                                                                    && t.IsActive)
                            .Select(t => new
                            {
                                t.PoNumber,
                                t.BuyerCompanyId,
                                t.AcceptedCompanyId,
                                t.ExternalBrokerId,
                                t.ExternalBrokerOrderDetail,
                                t.AcceptedBy,
                                t.IsEndSupplier,
                                IsBuySellOrder = t.ExternalBrokerBuySellDetail != null,
                                t.ExternalBrokerBuySellDetail,
                                t.IsFTL,
                                OrderDetailVersion = t.OrderDetailVersions.FirstOrDefault(t1 => t1.IsActive),
                                Job = new
                                {
                                    JobAddress = new AddressViewModel
                                    {
                                        Address = t.FuelRequest.Job.Address,
                                        City = t.FuelRequest.Job.City,
                                        StateCode = t.FuelRequest.Job.MstState.Code,
                                        CountryCode = t.FuelRequest.Job.MstCountry.Code,
                                        ZipCode = t.FuelRequest.Job.ZipCode,
                                        CountyName = t.FuelRequest.Job.CountyName
                                    },
                                    t.FuelRequest.Job.StateId,
                                    t.FuelRequest.Job.MstState.QuantityIndicatorTypeId,
                                    t.FuelRequest.Job.ZipCode,
                                    t.FuelRequest.Job.TimeZoneName,
                                    t.FuelRequest.Job.CompanyId,
                                    t.FuelRequest.Job.IsApprovalWorkflowEnabled,
                                    t.FuelRequest.Job.Id,
                                    t.FuelRequest.Job.LocationType,
                                    CompanyName = t.FuelRequest.Job.Company.Name,
                                    CountryCurrency = t.FuelRequest.Job.MstCountry.Currency,
                                    t.FuelRequest.Job.JobBudget.IsAssetTracked,
                                    BuyerTaxExemptLicence = t.FuelRequest.Job.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                    ApprovalUser = t.FuelRequest.Job.JobXApprovalUsers.FirstOrDefault(t1 => t1.IsActive),
                                    t.FuelRequest.Job.JobBudget.IsTaxExempted
                                },
                                FuelRequest = new
                                {
                                    t.FuelRequest.Id,
                                    t.FuelRequest.JobId,
                                    t.FuelRequest.UoM,
                                    t.FuelRequest.Currency,
                                    t.FuelRequest.FuelTypeId,
                                    t.FuelRequest.MstProduct.ProductDisplayGroupId,
                                    t.FuelRequest.MstProduct.MappedParentId,
                                    t.FuelRequest.MstProduct.ProductCode,
                                    t.FuelRequest.PricingTypeId,
                                    t.FuelRequest.FuelDescription,
                                    t.FuelRequest.MaxQuantity,
                                    t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                    t.FuelRequest.FuelRequestDetail.StartDate,
                                    t.FuelRequest.FuelRequestDetail.EndTime,
                                    t.FuelRequest.FreightOnBoardTypeId,
                                    t.FuelRequest.PaymentTermId,
                                    t.FuelRequest.NetDays,
                                },
                                t.FuelRequest.FuelRequestFees,
                                t.OrderDetailVersions.FirstOrDefault(t1 => t1.IsActive).PaymentMethod,
                            }).FirstOrDefaultAsync();

                if (order != null)
                {
                    //from set ToManualInvoiceViewModel start
                    viewModel.DropStartDate = tankIntervalEndDate;
                    viewModel.DropEndDate = tankIntervalEndDate;
                    viewModel.FuelDropped = 0.0M;

                    viewModel.PaymentTermId = order.OrderDetailVersion?.PaymentTermId ?? 0;
                    viewModel.NetDays = order.OrderDetailVersion?.NetDays ?? 0;
                    viewModel.InvoiceTypeId = (int)InvoiceType.Manual;
                    viewModel.UserId = order.AcceptedBy; // NEED TO CONFIRM
                    viewModel.OrderId = orderId;
                    viewModel.InvoiceStatusId = (int)InvoiceStatus.Received;

                    if (viewModel.AdditionalDetail == null)
                        viewModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();

                    viewModel.AdditionalDetail.Notes = "Auto generated Tank Rental Invoice";

                    viewModel.TankFrequency = tankRentalFrequency;
                    //set ToManualInvoiceViewModel end

                    var timeZoneName = order.Job.TimeZoneName;
                    viewModel.UoM = order.FuelRequest.UoM;
                    viewModel.Currency = order.FuelRequest.Currency;
                    viewModel.PoNumber = order.PoNumber;
                    viewModel.TimeZoneName = timeZoneName;
                    viewModel.FuelTypeId = order.FuelRequest.FuelTypeId;
                    invoiceViewModel.FuelId = order.FuelRequest.FuelTypeId;
                    viewModel.MappedParentFuelTypeId = order.FuelRequest.MappedParentId;
                    viewModel.FuelProductCode = order.FuelRequest.ProductCode;
                    viewModel.TypeOfFuel = order.FuelRequest.ProductDisplayGroupId;

                    viewModel.JobCompanyName = order.Job.CompanyName;
                    viewModel.IsBuySellOrder = order.IsBuySellOrder;
                    if (viewModel.IsBuySellOrder)
                    {
                        viewModel.BrokerMarkUp = order.ExternalBrokerBuySellDetail.BrokerMarkUp;
                        viewModel.SupplierMarkUp = order.ExternalBrokerBuySellDetail.SupplierMarkUp;
                    }
                    viewModel.CountryCurrency = order.Job.CountryCurrency;
                    viewModel.BuyerCompanyId = order.BuyerCompanyId;
                    viewModel.JobId = order.FuelRequest.JobId;
                    viewModel.JobStateId = order.Job.StateId;
                    viewModel.JobCompanyId = order.Job.CompanyId;
                    viewModel.AcceptedCompanyId = order.AcceptedCompanyId;
                    viewModel.DeliveryStartDate = order.FuelRequest.StartDate;
                    viewModel.DeliveryEndTime = order.FuelRequest.EndTime;

                    viewModel.OrderAcceptedBy = order.AcceptedBy;
                    if (order.Job.ApprovalUser != null)
                    {
                        viewModel.ApprovalUserId = order.Job.ApprovalUser.Id;
                        viewModel.ApprovalUserOnboardedType = order.Job.ApprovalUser.User.OnboardedTypeId;
                        viewModel.ApprovalUserName = $"{order.Job.ApprovalUser.User.FirstName} {order.Job.ApprovalUser.User.LastName}";
                    }
                    viewModel.IsApprovalWorkflowEnabledForJob = order.Job.IsApprovalWorkflowEnabled;
                    viewModel.ProductDescription = order.FuelRequest.FuelDescription;
                    viewModel.ExternalBrokerId = order.ExternalBrokerId ?? 0;
                    viewModel.IsThirdPartyHardwareUsed = order.ExternalBrokerOrderDetail != null;
                    viewModel.IsBrokeredChainOrder = order.BuyerCompanyId != order.Job.CompanyId;
                    viewModel.JobAddess = order.Job.JobAddress;

                    viewModel.IsFTL = order.IsFTL;
                    viewModel.SupplierPreferredInvoiceTypeId = (int)InvoiceType.Manual;
                    if (order.FuelRequestFees != null && order.FuelRequestFees.Any())
                    {
                        var filteredFees = order.FuelRequestFees.Where(t => t.FeeSubTypeId != (int)FeeSubType.NoFee && t.FeeTypeId == (int)FeeType.ProcessingFee).ToList();
                        if (filteredFees.Any())
                        {
                            viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = filteredFees.ToFeesViewModel();
                            viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.RemoveAll(t => t.FeeTypeId.Equals(((int)FeeType.DryRunFee).ToString()));
                        }
                    }

                    SetInputsToCreateManualInvoiceModel(viewModel, invoiceModel);
                    var payduedatebasis = Context.DataContext.OnboardingPreferences.Where(t => t.IsActive && t.CompanyId == order.AcceptedCompanyId)
                        .Select(t => t.PaymentDueDateType).FirstOrDefault();
                    await GetInvoiceViewModelForAutoInvoice(viewModel, invoiceModel, payduedatebasis);
                    invoiceModel.AdditionalDetail.TankFrequencyId = tankRentalFrequency.TankRentalFrequencyId;
                    invoiceModel.AdditionalDetail.PaymentMethod = order.PaymentMethod;
                    viewModel.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreateDomain", "GetCreateViewModelForAutoTankInvoiceAsync", ex.Message, ex);
            }
            return viewModel;
        }

        private void SetTankRentalFeesToInvoice(InvoiceModel invoiceModel, TankRentalFrequencyViewModel tankRentalFrequency,
                                                    DateTimeOffset currentDate, DateTimeOffset tankRentalStartDate, DateTimeOffset tankEndDate)
        {
            invoiceModel.TankFrequency = tankRentalFrequency;
            if (tankRentalFrequency.Tanks != null && tankRentalFrequency.Tanks.Any())
            {
                foreach (var tankDetails in tankRentalFrequency.Tanks)
                {
                    tankDetails.IntervalStartDate = tankDetails.IntervalEndDate = null;
                    //get pro-rated tank fee
                    //var tankFee = CalculateTankRentalAmount(tankDetails, tankRentalStartDate, tankEndDate, currentDate, tankRentalFrequency.FrequencyTypes);
                    var tankFee = CalculateTankRentalInvoiceAmount(tankDetails, tankRentalStartDate, tankEndDate, currentDate, tankRentalFrequency.FrequencyTypes);
                    if (tankFee.TotalFee > 0)
                    {
                        invoiceModel.TotalTaxAmount += tankFee.TotalTax;
                        invoiceModel.BasicAmount += tankFee.TotalFee;
                        tankDetails.IsToBeIncludedInInvoice = true;
                        tankDetails.RentalFee = tankFee.TotalFee;
                        tankDetails.FeeTaxDetails.Amount = tankFee.TotalTax;
                    }
                }
            }
        }

        private TankFeeAndTax CalculateTankRentalAmount(TankDetailsViewModel tankDetails, DateTimeOffset freqStartDate,
                                                    DateTimeOffset tankEndDate, DateTimeOffset currentDate, FrequencyTypes frequencyType)
        {
            var response = new TankFeeAndTax();
            if (tankDetails.StartDate.Date <= freqStartDate.Date)
            {
                int totalDaysInFrequency = GetTotalDaysInFrequency(tankDetails.StartDate, frequencyType);
                var totalDaysOfTank = (int)(tankDetails.EndDate.HasValue ? (tankDetails.EndDate.Value.Date - tankDetails.StartDate.Date).TotalDays : totalDaysInFrequency);

                if (totalDaysInFrequency - totalDaysOfTank <= 0)
                {
                    response.TotalFee = tankDetails.RentalFee;
                    if (tankDetails.FeeTaxDetails.Percentage.HasValue && tankDetails.FeeTaxDetails.Percentage.Value > 0)
                        response.TotalTax = response.TotalFee * tankDetails.FeeTaxDetails.Percentage.Value / 100;
                    tankDetails.IntervalDays = 1;
                }
                else
                {
                    var proRatedFee = tankDetails.RentalFee / totalDaysInFrequency;
                    response.TotalFee = proRatedFee * totalDaysOfTank;
                    if (tankDetails.FeeTaxDetails.Percentage.HasValue && tankDetails.FeeTaxDetails.Percentage.Value > 0)
                    {
                        response.TotalTax = response.TotalFee * tankDetails.FeeTaxDetails.Percentage.Value / 100;
                    }
                    tankDetails.IntervalDays = totalDaysOfTank / totalDaysInFrequency;
                }

                if (response.TotalFee > 0)
                {
                    if (frequencyType == FrequencyTypes.Daily)
                    {
                        tankDetails.IntervalStartDate = tankEndDate;
                        tankDetails.IntervalEndDate = tankEndDate;
                    }
                    else
                    {
                        var days = totalDaysOfTank + (totalDaysInFrequency - totalDaysOfTank) - (frequencyType == FrequencyTypes.Monthly ? 1 : 0);
                        tankDetails.IntervalStartDate = tankEndDate.AddDays(-days);
                        tankDetails.IntervalEndDate = tankEndDate;
                    }
                }
            }

            return response;
        }

        private TankFeeAndTax CalculateTankRentalInvoiceAmount(TankDetailsViewModel tankDetails, DateTimeOffset tankRentalStartDate,
                                                   DateTimeOffset tankEndDate, DateTimeOffset currentDate, FrequencyTypes frequencyType)
        {
            var response = new TankFeeAndTax();
            if (tankDetails.StartDate.Date <= tankRentalStartDate.Date)
            {
                int totalDaysInFrequency = GetTotalDaysInFrequency(tankRentalStartDate, frequencyType);
                var totalDaysOfTank = (tankEndDate.Date - tankRentalStartDate.Date).Days + 1;

                if (totalDaysInFrequency == totalDaysOfTank)
                {
                    response.TotalFee = tankDetails.RentalFee;
                    if (tankDetails.FeeTaxDetails.Percentage.HasValue && tankDetails.FeeTaxDetails.Percentage.Value > 0)
                        response.TotalTax = response.TotalFee * tankDetails.FeeTaxDetails.Percentage.Value / 100;
                    tankDetails.IntervalDays = 1;
                }
                else
                {
                    var proRatedFee = tankDetails.RentalFee / totalDaysInFrequency;
                    response.TotalFee = proRatedFee * totalDaysOfTank;
                    if (tankDetails.FeeTaxDetails.Percentage.HasValue && tankDetails.FeeTaxDetails.Percentage.Value > 0)
                    {
                        response.TotalTax = response.TotalFee * tankDetails.FeeTaxDetails.Percentage.Value / 100;
                    }
                    tankDetails.IntervalDays = totalDaysOfTank;
                }

                if (response.TotalFee > 0)
                {
                    tankDetails.IntervalStartDate = tankRentalStartDate;
                    tankDetails.IntervalEndDate = tankEndDate;
                }
            }

            return response;
        }


        private int GetTotalDaysInFrequency(DateTimeOffset tankStartDate, FrequencyTypes frequencyType)
        {
            switch (frequencyType)
            {
                case FrequencyTypes.Daily:
                    return 1;
                case FrequencyTypes.Weekly:
                    return 7;
                case FrequencyTypes.Biweekly:
                    return 14;
                case FrequencyTypes.Monthly:
                    return tankStartDate.LastDayOfMonth().Day;

                default:
                    return 1;
            }
        }

        public async Task GetInvoiceViewModelForAutoInvoice(InvoiceCreateViewModel invoiceRequestModel, InvoiceModel invoiceModel, PaymentDueDateType paymentDueDateType)
        {
            var timeZoneName = invoiceRequestModel.TimeZoneName;
            invoiceModel.UoM = invoiceRequestModel.UoM;
            invoiceModel.Currency = invoiceRequestModel.Currency;
            invoiceModel.SupplierPreferredInvoiceTypeId = invoiceRequestModel.SupplierPreferredInvoiceTypeId;

            CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
            invoiceModel.ExchangeRate = currencyRateDomain.GetCurrencyRate(invoiceRequestModel.Currency, Currency.USD, DateTimeOffset.Now);
            invoiceModel.PaymentDueDate = GetPaymentDueDate(invoiceRequestModel.PaymentTermId, invoiceRequestModel.NetDays, timeZoneName, invoiceModel.DropEndDate, paymentDueDateType);
            invoiceModel.CreatedBy = invoiceRequestModel.UserId;

            var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
            invoiceModel.CreatedDate = currentDate;
            invoiceModel.UpdatedBy = invoiceRequestModel.UserId;
            invoiceModel.UpdatedDate = currentDate;
            invoiceModel.TerminalId = invoiceRequestModel.TerminalId;
            invoiceModel.CityGroupTerminalId = invoiceRequestModel.CityGroupTerminalId;

            // Set invoice type and waiting action if approval workflow is enabled
            invoiceModel.InvoiceTypeId = (int)InvoiceType.TankRental;
            ProcessInvoiceFuelFeesAndSetCalculatedValues(invoiceRequestModel, invoiceModel, invoiceModel.DropEndDate, 0);
            await SetInvoiceAdditionDetailToInvoiceModel(invoiceModel, 0, invoiceRequestModel.OrderId, invoiceRequestModel.IsBuySellOrder);

            if (invoiceModel.AdditionalDetail == null)
            {
                invoiceModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
            }

            if (invoiceRequestModel.AdditionalDetail != null && !string.IsNullOrWhiteSpace(invoiceRequestModel.AdditionalDetail.Notes))
            {
                invoiceModel.AdditionalDetail.Notes = invoiceRequestModel.AdditionalDetail.Notes;
            }

            var invoiceNumber = await GenerateInvoiceNumber();
            invoiceModel.InvoiceNumberId = invoiceNumber.Id;
            invoiceModel.DisplayInvoiceNumber = invoiceNumber.Number;
        }

        private async Task SetTankRentalInvoiceCreatedPostEvents(UserContext userContext, ManualInvoiceCreateRequestViewModel requestViewModel, InvoiceModel invoiceModel, InvoiceCreateResponseViewModel invoiceCreateResponse)
        {
            var newsfeedDomain = new NewsfeedDomain(this);
            if (invoiceCreateResponse.StatusCode == Status.Success)
            {
                var newsfeedRequestModel = GetTankRentalInvoiceCreatedNewsfeedModel(requestViewModel, invoiceCreateResponse, invoiceModel);
                await newsfeedDomain.SetTankRentalInvoiceCreatedNewsfeed(userContext, newsfeedRequestModel);

                await AddNotificationEventForManualInvoice(invoiceCreateResponse, invoiceModel);
                //AddWebNotificationEventForMobileInvoice(invoiceCreateResponse, queueDomain);
            }
        }

        private async Task SetTankRentalInvoiceRebilledPostEvents(UserContext userContext, ManualInvoiceCreateRequestViewModel requestViewModel, InvoiceModel invoiceModel, InvoiceCreateResponseViewModel invoiceCreateResponse)
        {
            var newsfeedDomain = new NewsfeedDomain(this);
            if (invoiceCreateResponse.StatusCode == Status.Success)
            {
                var newsfeedRequestModel = GetTankRentalInvoiceCreatedNewsfeedModel(requestViewModel, invoiceCreateResponse, invoiceModel);
                await newsfeedDomain.SetRebillInvoiceCreatedNewsfeed(userContext, newsfeedRequestModel);
                var notificationDomain = new NotificationDomain(this);
                await notificationDomain.AddNotificationEventAsync(EventType.RebilledInvoiceCreated, invoiceCreateResponse.InvoiceHeaderId, userContext.Id);
            }
        }

        private static ManualInvoiceCreatedNewsfeedModel GetTankRentalInvoiceCreatedNewsfeedModel(ManualInvoiceCreateRequestViewModel viewModel, InvoiceCreateResponseViewModel invoiceCreateResponse, InvoiceModel invoiceModel)
        {
            return new ManualInvoiceCreatedNewsfeedModel
            {
                InvoiceId = invoiceCreateResponse.InvoiceId,
                InvoiceNumber = invoiceCreateResponse.InvoiceNumber,
                OrderId = invoiceCreateResponse.OrderId,
                PoNumber = invoiceCreateResponse.PoNumber,
                BuyerCompanyId = invoiceCreateResponse.BuyerCompanyId,
                SupplierCompanyId = invoiceCreateResponse.SupplierCompanyId,
                JobId = viewModel.JobId,
                InvoiceTypeId = invoiceCreateResponse.InvoiceTypeId,
                TimeZoneName = viewModel.TimeZoneName,
                WaitingFor = invoiceModel.WaitingFor,
                OriginalInvoiceNumber = invoiceCreateResponse.OriginalInvoiceNumber
            };
        }

        private async Task AddNotificationEventForManualInvoice(InvoiceCreateResponseViewModel invoiceModel, InvoiceModel invoiceViewModel)
        {
            var notificationEvent = EventType.InvoiceCreated;
            NotificationDomain notificationDomain = new NotificationDomain(this);
            await notificationDomain.AddNotificationEventAsync(notificationEvent, invoiceModel.InvoiceHeaderId, invoiceViewModel.CreatedBy);
        }

        //private void AddWebNotificationEventForMobileInvoice(InvoiceCreateResponseViewModel invoiceCreateResponse, QueueMessageDomain queueDomain)
        //{
        //    if (!IsDigitalDropTicket(invoiceCreateResponse.InvoiceTypeId))
        //    {
        //        var queueRequest = GetEnqueueMessageRequestViewModel(invoiceCreateResponse);
        //        var queueId = queueDomain.EnqeueMessage(queueRequest);
        //        //MIGHT BE USE QUEUEID FOR  reporting purpose
        //    }
        //}
    }
}
