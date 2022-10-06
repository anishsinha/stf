using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
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
    public class BalanceInvoiceDomain : InvoiceCommonDomain
    {
        public BalanceInvoiceDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public BalanceInvoiceDomain(BaseDomain domain) : base(domain)
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
                //CurrentTrackableScheduleId = invoiceCreateModel.TrackableScheduleId,
                //CurrentTrackableScheduleStatusId = GetDeliveryScheduleStatus(invoiceCreateModel.TrackableScheduleId, invoiceCreateModel.InvoiceStatusId, invoiceCreateModel.DropEndDate),
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
                var helperDomain = new HelperDomain(this);
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open);
                //// check if order is closed order and gallon delivery is 0 percent drop/0 gallons drop
                if (order == null)
                {
                    order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId &&
                                                                                       t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed);
                    avgGallonsPercentagePerDelivery = helperDomain.GetAverageFuelDropPercentagePerOrder(order);
                }

                if (order != null && avgGallonsPercentagePerDelivery <= 0)
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
                        TerminalName = order.TerminalId.HasValue ? order.MstExternalTerminal.Name : (order.FuelRequest.TerminalId.HasValue ? order.FuelRequest.MstExternalTerminal.Name : string.Empty),
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
                        PaymentMethod = order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).PaymentMethod
                    };

                    if (order.FuelRequest.FuelRequestFees != null && order.FuelRequest.FuelRequestFees.Any())
                    {
                        response.FuelDeliveryDetails.FuelFees.FuelRequestFees = order.FuelRequest.FuelRequestFees.ToFeesViewModel();
                        response.FuelDeliveryDetails.FuelFees.FuelRequestFees.ForEach(t => t.FeeTaxDetails = new FeeTaxDetails());
                        response.FuelDeliveryDetails.FuelFees.FuelRequestFees.RemoveAll(t => t.FeeTypeId.Equals(((int)FeeType.DryRunFee).ToString()));
                    }

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

                    if (order.OrderTaxDetails != null && order.OrderTaxDetails.Count > 0)
                    {
                        response.Taxes = order.OrderTaxDetails.Where(t => t.IsActive).ToTaxViewModel();
                        response.IsOtherFuelTypeTaxesGiven = response.Taxes.Count > 0;
                    }

                    response.FuelDeliveryDetails.FuelFees.TruckLoadType = TruckLoadTypes.LessTruckLoad;

                    if (order.IsFTL)
                    {
                        response.FuelDeliveryDetails.FuelFees.FuelRequestFees.ForEach((t) => t.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad);
                        response.FuelDeliveryDetails.FuelFees.TruckLoadType = TruckLoadTypes.FullTruckLoad;

                        if (order.FuelRequest.FreightOnBoardTypeId.HasValue)
                            response.IsVariousFobOrigin = order.FuelRequest.FreightOnBoardTypeId == (int)FreightOnBoardTypes.Terminal && order.FuelRequest.Job.LocationType == JobLocationTypes.Various;
                    }

                    if (order.OrderAdditionalDetail != null)
                    {
                        response.Notes = order.OrderAdditionalDetail.Notes;
                    }

                    response.FuelDeliveryDetails.FuelFees.Currency = order.FuelRequest.Currency;
                    response.FuelDeliveryDetails.FuelFees.UoM = order.FuelRequest.UoM;
                    response.ExternalBrokeredOrder.BrokeredOrderFee.Currency = order.FuelRequest.Currency;
                    response.ExternalBrokeredOrder.BrokeredOrderFee.UoM = order.FuelRequest.UoM;

                    response.InvoiceTypeId = (int)InvoiceType.Balance;
                    response = response.CorrectValues();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BalanceInvoiceDomain", "GetManualInvoiceAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<InvoiceCreateResponseViewModel> CreateBalanceInvoiceAsync(UserContext userContext, ManualInvoiceViewModel invoiceViewModel)
        {
            var invoiceCreateResponse = new InvoiceCreateResponseViewModel();
            using (var tracer = new Tracer("BalanceInvoiceDomain", "CreateBalanceInvoiceAsync"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        InvoiceModel invoiceModel = new InvoiceModel();
                        var manualInvoiceCreateRequestViewModel = await GetCreateManualInvoiceRequestViewModelAsync(invoiceViewModel, invoiceModel);
                        var invoiceCreateRequestViewModel = GetInvoiceCreateRequestViewModel(manualInvoiceCreateRequestViewModel, invoiceModel);
                        invoiceCreateResponse = await GenerateManualInvoice(invoiceCreateRequestViewModel, invoiceViewModel);
                        transaction.Commit();
                        await SetBalanceInvoiceCreatedPostEvents(userContext, manualInvoiceCreateRequestViewModel, invoiceModel, invoiceCreateResponse);
                        invoiceCreateResponse.StatusCode = Status.Success;
                        invoiceCreateResponse.StatusMessage = Resource.valMessageBalanceInvoiceCreatedSuccessfully;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        invoiceCreateResponse.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                        LogManager.Logger.WriteException("BalanceInvoiceDomain", "CreateBalanceInvoiceAsync", ex.Message, ex);
                    }
                }
            }
            return invoiceCreateResponse;
        }

        public async Task<InvoiceCreateResponseViewModel> CreditRebillBalanceInvoiceAsync(UserContext userContext, ManualInvoiceViewModel invoiceViewModel)
        {
            var invoiceCreateResponse = new InvoiceCreateResponseViewModel();
            using (var tracer = new Tracer("BalanceInvoiceDomain", "CreditRebillBalanceInvoiceAsync"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        InvoiceModel invoiceModel = new InvoiceModel();
                        var manualInvoiceCreateRequestViewModel = await GetCreateManualInvoiceRequestViewModelAsync(invoiceViewModel, invoiceModel);
                        var invoiceCreateRequestViewModel = GetInvoiceCreateRequestViewModel(manualInvoiceCreateRequestViewModel, invoiceModel);
                        invoiceCreateResponse = await BalanceRebillInvoice(invoiceCreateRequestViewModel, invoiceViewModel);
                        StatusViewModel creditInvoiceResponse =AddCreditInvoiceToQueueServiceAsync(invoiceViewModel.InvoiceId, null, userContext.Id);
                        transaction.Commit();
                        invoiceCreateResponse.StatusCode = Status.Success;
                        await SetBalanceInvoiceRebilledPostEvents(userContext, manualInvoiceCreateRequestViewModel, invoiceModel, invoiceCreateResponse);
                        invoiceCreateResponse.StatusMessage = Resource.valMessageBalanceInvoiceCreatedSuccessfully;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        invoiceCreateResponse.StatusMessage = Resource.errMessageCreditRebillFailed;
                        LogManager.Logger.WriteException("BalanceInvoiceDomain", "CreditRebillBalanceInvoiceAsync", ex.Message, ex);
                    }
                }
            }
            return invoiceCreateResponse;
        }

        private async Task<InvoiceCreateResponseViewModel> GenerateManualInvoice(InvoiceCreateRequestViewModel invoiceCreateRequestViewModel, ManualInvoiceViewModel invoiceViewModel)
        {
            var brokeredinvoiceCreateRequestModels = new InvoiceCreateRequestViewModel();
            var invoiceCreateResponses = await CreateBalanceInvoiceAsync(invoiceCreateRequestViewModel, brokeredinvoiceCreateRequestModels, invoiceViewModel);
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
                                    t.FuelRequest.FuelDescription,
                                    t.FuelRequest.MaxQuantity,
                                    t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                    t.FuelRequest.FuelRequestDetail.StartDate,
                                    t.FuelRequest.FuelRequestDetail.EndTime,
                                    t.FuelRequest.FuelRequestDetail.IsBolImageRequired,
                                    t.FuelRequest.FuelRequestDetail.IsDropImageRequired,
                                    t.FuelRequest.Job.SignatureEnabled,
                                    t.FuelRequest.FreightOnBoardTypeId
                                },
                                t.FuelRequest.FuelRequestPricingDetail,
                                t.FuelRequest.FuelRequestDetail
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
                    viewModel.IsBOLImageReq = order.FuelRequest.IsBolImageRequired;
                    viewModel.IsDropImageReq = order.FuelRequest.IsDropImageRequired;
                    viewModel.IsSignatureReq = order.FuelRequest.SignatureEnabled;
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
                    await GetInvoiceViewModel(viewModel, invoiceModel, manualInvoiceModel.SupplierInvoiceNumber);

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

        public async Task GetInvoiceViewModel(InvoiceCreateViewModel invoiceRequestModel, InvoiceModel invoiceModel, string supplierInvoiceNumber)
        {
            var timeZoneName = invoiceRequestModel.TimeZoneName;
            invoiceModel.UoM = invoiceRequestModel.UoM;
            invoiceModel.Currency = invoiceRequestModel.Currency;
            invoiceModel.SupplierPreferredInvoiceTypeId = invoiceRequestModel.SupplierPreferredInvoiceTypeId;

            CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
            invoiceModel.ExchangeRate = currencyRateDomain.GetCurrencyRate(invoiceRequestModel.Currency, Currency.USD, DateTimeOffset.Now);
            invoiceModel.PaymentDueDate = GetPaymentDueDate(invoiceRequestModel.PaymentTermId, invoiceRequestModel.NetDays, timeZoneName, invoiceRequestModel.DropEndDate, PaymentDueDateType.InvoiceCreationDate );
            invoiceModel.CreatedBy = invoiceRequestModel.UserId;

            var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
            invoiceModel.CreatedDate = currentDate;
            invoiceModel.UpdatedBy = invoiceRequestModel.UserId;
            invoiceModel.UpdatedDate = currentDate;
            invoiceModel.TerminalId = invoiceRequestModel.TerminalId;
            invoiceModel.CityGroupTerminalId = invoiceRequestModel.CityGroupTerminalId;

            // Set invoice type and waiting action if approval workflow is enabled
            invoiceModel.InvoiceTypeId = (int)InvoiceType.Balance;
            RemoveDryRunFeeFromManualInvoiceModel(invoiceRequestModel);
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
            invoiceModel.AdditionalDetail.PaymentMethod = invoiceRequestModel.AdditionalDetail.PaymentMethod;
            var invoiceNumber = await GenerateInvoiceNumber();
            invoiceModel.InvoiceNumberId = invoiceNumber.Id;

            if (string.IsNullOrWhiteSpace(supplierInvoiceNumber))
                invoiceModel.DisplayInvoiceNumber = invoiceNumber.Number;
            else
            {
                invoiceModel.DisplayInvoiceNumber = supplierInvoiceNumber;
                invoiceModel.ReferenceId = invoiceNumber.Number;
            }
        }

        private async Task SetBalanceInvoiceCreatedPostEvents(UserContext userContext, ManualInvoiceCreateRequestViewModel requestViewModel, InvoiceModel invoiceModel, InvoiceCreateResponseViewModel invoiceCreateResponse)
        {
            var newsfeedDomain = new NewsfeedDomain(this);
            if (invoiceCreateResponse.StatusCode == Status.Success)
            {
                var newsfeedRequestModel = GetBalalnceInvoiceCreatedNewsfeedModel(requestViewModel, invoiceCreateResponse, invoiceModel);
                await newsfeedDomain.SetBalanceInvoiceCreatedNewsfeed(userContext, newsfeedRequestModel);

                //var queueDomain = new QueueMessageDomain();

                await AddNotificationEventForManualInvoice(invoiceCreateResponse, invoiceModel);
                //AddWebNotificationEventForMobileInvoice(invoiceCreateResponse, queueDomain);
            }
        }

        private async Task SetBalanceInvoiceRebilledPostEvents(UserContext userContext, ManualInvoiceCreateRequestViewModel requestViewModel, InvoiceModel invoiceModel, InvoiceCreateResponseViewModel invoiceCreateResponse)
        {
            var newsfeedDomain = new NewsfeedDomain(this);
            if (invoiceCreateResponse.StatusCode == Status.Success)
            {
                var newsfeedRequestModel = GetBalalnceInvoiceCreatedNewsfeedModel(requestViewModel, invoiceCreateResponse, invoiceModel);
                await newsfeedDomain.SetRebillInvoiceCreatedNewsfeed(userContext, newsfeedRequestModel);
                var notificationDomain = new NotificationDomain(this);
                await notificationDomain.AddNotificationEventAsync(EventType.RebilledInvoiceCreated, invoiceCreateResponse.InvoiceHeaderId, userContext.Id);
            }
        }

        private static ManualInvoiceCreatedNewsfeedModel GetBalalnceInvoiceCreatedNewsfeedModel(ManualInvoiceCreateRequestViewModel viewModel, InvoiceCreateResponseViewModel invoiceCreateResponse, InvoiceModel invoiceModel)
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

