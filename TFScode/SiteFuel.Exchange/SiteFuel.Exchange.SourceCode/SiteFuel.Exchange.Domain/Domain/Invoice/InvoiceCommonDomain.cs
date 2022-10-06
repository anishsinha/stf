using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.FilldService;
using SiteFuel.Exchange.ViewModels.FuelPricingDatail;
using SiteFuel.Exchange.ViewModels.InvoiceExceptions;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using SiteFuel.Exchange.ViewModels.NewsfeedRequest;
using SiteFuel.Exchange.ViewModels.Queue;
//using SiteFuel.Exchange.ViewModels.WebNotification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class InvoiceCommonDomain : InvoiceBaseDomain
    {
        public InvoiceCommonDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public InvoiceCommonDomain(string connectionString) : base(connectionString)
        {
        }

        public InvoiceCommonDomain(BaseDomain domain) : base(domain)
        {
        }

        public void SetManualInputsToEditInvoiceModel(ManualInvoiceViewModel manualInvoiceModel, InvoiceModel invoiceModel, string timeZoneName)
        {
            DateTimeOffset dropStartDate = manualInvoiceModel.DeliveryDate.Date.Add(Convert.ToDateTime(manualInvoiceModel.StartTime).TimeOfDay);
            DateTimeOffset dropEndDate = manualInvoiceModel.DeliveryDate.Date.Add(Convert.ToDateTime(manualInvoiceModel.EndTime).TimeOfDay);
            if (manualInvoiceModel.DropEndDate != null)
                dropEndDate = manualInvoiceModel.DropEndDate.Value.Date.Add(Convert.ToDateTime(manualInvoiceModel.EndTime).TimeOfDay);

            var timeZoneOffset = dropEndDate.GetOffset(timeZoneName);
            invoiceModel.DropStartDate = dropStartDate.AttachOffset(timeZoneOffset);
            invoiceModel.DropEndDate = dropEndDate.AttachOffset(timeZoneOffset);

            invoiceModel.Id = manualInvoiceModel.InvoiceId;
            invoiceModel.DroppedGallons = manualInvoiceModel.FuelDropped ?? 0.0M;
            invoiceModel.IsWetHosingDelivery = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.WetHoseFee).ToString() && t.DiscountLineItemId == null);
            invoiceModel.IsOverWaterDelivery = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.OverWaterFee).ToString() && t.DiscountLineItemId == null);

            invoiceModel.OrderId = manualInvoiceModel.OrderId;
            invoiceModel.PoNumber = manualInvoiceModel.PoNumber;
            if (manualInvoiceModel.InvoiceImage != null && !manualInvoiceModel.InvoiceImage.IsRemoved)
            {
                invoiceModel.Image = manualInvoiceModel.InvoiceImage;
            }
            if (manualInvoiceModel.BolImage != null && !manualInvoiceModel.BolImage.IsRemoved)
            {
                invoiceModel.BolImage = manualInvoiceModel.BolImage;
            }
            if (manualInvoiceModel.SignatureImage != null && !manualInvoiceModel.SignatureImage.IsRemoved)
            {
                invoiceModel.Signature = manualInvoiceModel.SignatureImage.ToCustomerSignature();
            }
            //marine
            if (manualInvoiceModel.TaxAffidavitImage != null && !manualInvoiceModel.TaxAffidavitImage.IsRemoved)
            {
                invoiceModel.TaxAffidavitImage = manualInvoiceModel.TaxAffidavitImage;
            }
            if (manualInvoiceModel.BDNImage != null && !manualInvoiceModel.BDNImage.IsRemoved)
            {
                invoiceModel.BDNImage = manualInvoiceModel.BDNImage;
            }
            if (manualInvoiceModel.CoastGuardInspectionImage != null && !manualInvoiceModel.CoastGuardInspectionImage.IsRemoved)
            {
                invoiceModel.CoastGuardInspectionImage = manualInvoiceModel.CoastGuardInspectionImage;
            }
            if (manualInvoiceModel.InspectionRequestVoucherImage != null && !manualInvoiceModel.InspectionRequestVoucherImage.IsRemoved)
            {
                invoiceModel.InspectionRequestVoucherImage = manualInvoiceModel.InspectionRequestVoucherImage;
            }
            if (manualInvoiceModel.AdditionalImage != null && !manualInvoiceModel.AdditionalImage.IsRemoved)
            {
                invoiceModel.AdditionalImage = manualInvoiceModel.AdditionalImage;
            }
            invoiceModel.DriverId = manualInvoiceModel.DriverId;
            invoiceModel.InvoiceNumberId = manualInvoiceModel.InvoiceNumber.Id;
            invoiceModel.InvoiceTypeId = manualInvoiceModel.InvoiceTypeId;
            invoiceModel.PaymentTermId = manualInvoiceModel.PaymentTermId;
            invoiceModel.NetDays = manualInvoiceModel.PaymentTermId == (int)PaymentTerms.NetDays ? manualInvoiceModel.NetDays : 0;

            invoiceModel.FilePath = manualInvoiceModel.CsvFilePath;
            invoiceModel.IsBuyPriceInvoice = manualInvoiceModel.IsBuyPriceInvoice;
            invoiceModel.SpecialInstructions = manualInvoiceModel.SpecialInstructions;
            invoiceModel.SignatureId = manualInvoiceModel.SignatureId;
            invoiceModel.StatusId = manualInvoiceModel.StatusId;

            invoiceModel.BolDetails.Add(manualInvoiceModel.BolDetails);
            invoiceModel.IsVariousFobOrigin = manualInvoiceModel.IsVariousFobOrigin;

            if (invoiceModel.IsVariousFobOrigin || !string.IsNullOrEmpty(manualInvoiceModel.SplitLoadChainId))
            {
                invoiceModel.FuelDropLocation = manualInvoiceModel.ToDropLocation();
                SetFirstZipCodeOfState(manualInvoiceModel.DropAddress.State.Id, manualInvoiceModel.DropAddress.State.Code, out string newStateCode);
                invoiceModel.FuelDropLocation.StateCode = newStateCode;
            }
            if (manualInvoiceModel.PickUpAddress != null && manualInvoiceModel.PickUpAddress.IsAddressAvailable)
            {
                invoiceModel.FuelPickLocation = manualInvoiceModel.ToPickUpLocation();
            }

            invoiceModel.SurchargeFreightFeeViewModel = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee;
            invoiceModel.SurchargeFreightFeeViewModel.GallonsDelivered = invoiceModel.DroppedGallons;
            invoiceModel.Gravity = manualInvoiceModel.Gravity;
            invoiceModel.ConvertedQuantity = manualInvoiceModel.ConvertedQuantity;
            invoiceModel.ConversionFactor = manualInvoiceModel.ConvertionFactor;
            if (invoiceModel.ConversionFactor.HasValue && invoiceModel.ConversionFactor.Value > 0)
            {
                invoiceModel.DroppedGallons = manualInvoiceModel.FuelDropped.Value * invoiceModel.ConversionFactor.Value;
                invoiceModel.ConvertedQuantity = manualInvoiceModel.FuelDropped;
            }
        }

        public void SetManualInputsToConsolidatedEditInvoiceModel(ManualInvoiceViewModel manualInvoiceModel, InvoiceModel invoiceModel, string timeZoneName)
        {
            DateTimeOffset dropStartDate = manualInvoiceModel.DeliveryDate.Date.Add(Convert.ToDateTime(manualInvoiceModel.StartTime).TimeOfDay);
            DateTimeOffset dropEndDate = manualInvoiceModel.DeliveryDate.Date.Add(Convert.ToDateTime(manualInvoiceModel.EndTime).TimeOfDay);
            var timeZoneOffset = dropEndDate.GetOffset(timeZoneName);
            invoiceModel.DropStartDate = dropStartDate.AttachOffset(timeZoneOffset);
            invoiceModel.DropEndDate = dropEndDate.AttachOffset(timeZoneOffset);

            invoiceModel.Id = manualInvoiceModel.InvoiceId;
            invoiceModel.DroppedGallons = manualInvoiceModel.FuelDropped ?? 0.0M;
            invoiceModel.IsWetHosingDelivery = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.WetHoseFee).ToString() && t.DiscountLineItemId == null);
            invoiceModel.IsOverWaterDelivery = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.OverWaterFee).ToString() && t.DiscountLineItemId == null);

            invoiceModel.OrderId = manualInvoiceModel.OrderId;
            invoiceModel.PoNumber = manualInvoiceModel.PoNumber;
            if (manualInvoiceModel.InvoiceImage != null && !manualInvoiceModel.InvoiceImage.IsRemoved)
            {
                invoiceModel.Image = manualInvoiceModel.InvoiceImage;
            }
            if (manualInvoiceModel.BolImage != null && !manualInvoiceModel.BolImage.IsRemoved)
            {
                invoiceModel.BolImage = manualInvoiceModel.BolImage;
            }
            if (manualInvoiceModel.SignatureImage != null && !manualInvoiceModel.SignatureImage.IsRemoved)
            {
                invoiceModel.Signature = manualInvoiceModel.SignatureImage.ToCustomerSignature();
            }
            if (manualInvoiceModel.AdditionalImage != null && !manualInvoiceModel.AdditionalImage.IsRemoved)
            {
                invoiceModel.AdditionalImage = manualInvoiceModel.AdditionalImage;
            }
            //marine
            if (manualInvoiceModel.TaxAffidavitImage != null && !manualInvoiceModel.TaxAffidavitImage.IsRemoved)
            {
                invoiceModel.TaxAffidavitImage = manualInvoiceModel.TaxAffidavitImage;
            }
            if (manualInvoiceModel.BDNImage != null && !manualInvoiceModel.BDNImage.IsRemoved)
            {
                invoiceModel.BDNImage = manualInvoiceModel.BDNImage;
            }
            if (manualInvoiceModel.CoastGuardInspectionImage != null && !manualInvoiceModel.CoastGuardInspectionImage.IsRemoved)
            {
                invoiceModel.CoastGuardInspectionImage = manualInvoiceModel.CoastGuardInspectionImage;
            }
            if (manualInvoiceModel.InspectionRequestVoucherImage != null && !manualInvoiceModel.InspectionRequestVoucherImage.IsRemoved)
            {
                invoiceModel.InspectionRequestVoucherImage = manualInvoiceModel.InspectionRequestVoucherImage;
            }
            invoiceModel.DriverId = manualInvoiceModel.DriverId;
            invoiceModel.InvoiceNumberId = manualInvoiceModel.InvoiceNumber.Id;
            invoiceModel.InvoiceTypeId = manualInvoiceModel.InvoiceTypeId;
            invoiceModel.PaymentTermId = manualInvoiceModel.PaymentTermId;
            invoiceModel.NetDays = manualInvoiceModel.PaymentTermId == (int)PaymentTerms.NetDays ? manualInvoiceModel.NetDays : 0;

            invoiceModel.FilePath = manualInvoiceModel.CsvFilePath;
            invoiceModel.IsBuyPriceInvoice = manualInvoiceModel.IsBuyPriceInvoice;
            invoiceModel.SpecialInstructions = manualInvoiceModel.SpecialInstructions;
            invoiceModel.SignatureId = manualInvoiceModel.SignatureId;
            invoiceModel.StatusId = manualInvoiceModel.StatusId;
            invoiceModel.BolDetails = manualInvoiceModel.BolDetailsNew;
            invoiceModel.IsVariousFobOrigin = manualInvoiceModel.IsVariousFobOrigin;

            if (invoiceModel.IsVariousFobOrigin || !string.IsNullOrEmpty(manualInvoiceModel.SplitLoadChainId))
            {
                invoiceModel.FuelDropLocation = manualInvoiceModel.ToDropLocation();
                SetFirstZipCodeOfState(manualInvoiceModel.DropAddress.State.Id, manualInvoiceModel.DropAddress.State.Code, out string newStateCode);
                invoiceModel.FuelDropLocation.StateCode = newStateCode;
            }
            if (manualInvoiceModel.PickUpAddress != null && manualInvoiceModel.PickUpAddress.IsAddressAvailable)
            {
                invoiceModel.FuelPickLocation = manualInvoiceModel.ToPickUpLocation();
            }

            invoiceModel.SurchargeFreightFeeViewModel = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee;
            invoiceModel.SurchargeFreightFeeViewModel.GallonsDelivered = invoiceModel.DroppedGallons;
        }

        public async Task<InvoiceModel> GetInvoiceViewModel(InvoiceCreateViewModel invoiceRequestModel, InvoiceModel invoiceModel, int assetCount)
        {
            var timeZoneName = invoiceRequestModel.TimeZoneName;
            invoiceModel.UoM = invoiceRequestModel.UoM;
            invoiceModel.Currency = invoiceRequestModel.Currency;
            invoiceModel.SupplierPreferredInvoiceTypeId = invoiceRequestModel.SupplierPreferredInvoiceTypeId;

            CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
            invoiceModel.ExchangeRate = currencyRateDomain.GetCurrencyRate(invoiceRequestModel.Currency, Currency.USD, DateTimeOffset.Now);
            invoiceModel.PaymentDueDate = GetPaymentDueDate(invoiceRequestModel.PaymentTermId, invoiceRequestModel.NetDays, timeZoneName, invoiceRequestModel.DropEndDate, PaymentDueDateType.InvoiceCreationDate);
            invoiceModel.CreatedBy = invoiceRequestModel.UserId;

            var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
            invoiceModel.CreatedDate = currentDate;
            invoiceModel.UpdatedBy = invoiceRequestModel.UserId;
            invoiceModel.UpdatedDate = currentDate;
            invoiceModel.TerminalId = invoiceRequestModel.TerminalId;
            invoiceModel.CityGroupTerminalId = invoiceRequestModel.CityGroupTerminalId;

            // Set invoice type and waiting action if approval workflow is enabled
            if (invoiceModel.WaitingFor == WaitingAction.Nothing)
            {
                invoiceModel.InvoiceTypeId = CheckApprovalWorkflowAndGetInvoiceType(invoiceRequestModel.IsApprovalWorkflowEnabledForJob, invoiceRequestModel.InvoiceTypeId);
                invoiceModel.WaitingFor = GetApprovalWorkflowWaitingForAction(invoiceRequestModel.IsApprovalWorkflowEnabledForJob, invoiceRequestModel.InvoiceStatusId);
            }
            await SetInvoiceAdditionDetailToInvoiceModel(invoiceModel, 0, invoiceRequestModel.OrderId, invoiceRequestModel.IsBuySellOrder);
            if (invoiceModel.AdditionalDetail == null)
            {
                invoiceModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
            }
            invoiceModel.AdditionalDetail.SplitLoadChainId = invoiceRequestModel.AdditionalDetail?.SplitLoadChainId;
            invoiceModel.AdditionalDetail.SplitLoadSequence = invoiceRequestModel.AdditionalDetail?.SplitLoadSequence;
            invoiceModel.AdditionalDetail.Notes = invoiceRequestModel.AdditionalDetail?.Notes;
            invoiceModel.AdditionalDetail.PaymentMethod = (invoiceRequestModel.AdditionalDetail?.PaymentMethod ?? PaymentMethods.None);
            invoiceModel.AdditionalDetail.TruckNumber = invoiceRequestModel.AdditionalDetail.TruckNumber;
            invoiceModel.AdditionalDetail.DropTicketNumber = invoiceRequestModel.AdditionalDetail.DropTicketNumber;
            SetFuelPickupLocationDetails(invoiceRequestModel, invoiceModel);
            return invoiceModel;
        }

        public async Task<StatusViewModel> AddCreateInvioceToQueue(UserContext userContext, InvoiceViewModelNew invoiceViewModelNew)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            if (invoiceViewModelNew != null)
            {
                try
                {
                    invoiceViewModelNew.InvoiceChainId = Guid.NewGuid().ToString();
                    await SaveJsonAsFileContent(userContext, invoiceViewModelNew, response);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceCreateDomain", "AddCreateInvioceToQueue", ex.Message + "brokerChain:" + invoiceViewModelNew.Drops.Select(t => t.BrokerChainId).FirstOrDefault(), ex);
                }
            }
            return response;
        }

        private async Task SaveJsonAsFileContent(UserContext userContext, InvoiceViewModelNew invoiceViewModelNew, StatusViewModel response)
        {
            string json = JsonConvert.SerializeObject(invoiceViewModelNew);
            byte[] byteArray = Encoding.ASCII.GetBytes(json);
            MemoryStream stream = new MemoryStream(byteArray);
            var azureBlob = new AzureBlobStorage();
            var fileName = await azureBlob.SaveBlobAsync(stream, $"{userContext.Id}-{DateTime.Now.Ticks}.json", BlobContainerType.CreateInvoice.ToString().ToLower());

            QueueProcessType queueProcessType = QueueProcessType.CreateInvoiceUsingJsonFile;
            if (invoiceViewModelNew.CreationMethod == CreationMethod.Mobile)
                queueProcessType = QueueProcessType.CreateMobileInvoiceUsingJsonFile;

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var queueDomain = new QueueMessageDomain();
                var queueRequest = GetQueueEventForCreateInvoice(userContext, queueProcessType, fileName);
                var queueId = queueDomain.EnqeueMessage(queueRequest);
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.succcessMsgInvoiceDDTSubmitted;
            }
            else
            {
                response.StatusMessage = response.StatusMessage = Resource.errMessageErrorInAzureServer;
            }
        }

        private QueueMessageViewModel GetQueueEventForCreateInvoice(UserContext userContext, QueueProcessType queueProcessType, string blobStoragePath)
        {
            var jsonViewModel = new InvoiceBulkUploadProcessorReqViewModel();
            jsonViewModel.FileLineNumberToStart = 0;
            jsonViewModel.SupplierId = userContext.Id;
            jsonViewModel.SupplierCompanyId = userContext.CompanyId;
            jsonViewModel.FileUploadPath = blobStoragePath;

            string json = JsonConvert.SerializeObject(jsonViewModel);

            return new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = queueProcessType,
                JsonMessage = json
            };
        }

        private void SetFuelPickupLocationDetails(InvoiceCreateViewModel manualInvoiceCreateRequestViewModel, InvoiceModel invoiceModel)
        {
            if (manualInvoiceCreateRequestViewModel.TypeOfFuel != (int)ProductDisplayGroups.OtherFuelType)
            {
                var pickUpLocation = new DispatchLocationViewModel();
                if (invoiceModel.InvoiceTypeId != (int)InvoiceType.MobileApp && invoiceModel.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    SetFuelPickupLocationDetails(manualInvoiceCreateRequestViewModel, pickUpLocation);
                }
                else if (manualInvoiceCreateRequestViewModel.PickupLocation != null)
                {
                    pickUpLocation = manualInvoiceCreateRequestViewModel.PickupLocation;
                }
                else
                {
                    pickUpLocation.Address = manualInvoiceCreateRequestViewModel.TerminalAddress.Address;
                    pickUpLocation.City = manualInvoiceCreateRequestViewModel.TerminalAddress.City;
                    pickUpLocation.CountryCode = manualInvoiceCreateRequestViewModel.TerminalAddress.CountryCode;
                    pickUpLocation.CountyName = manualInvoiceCreateRequestViewModel.TerminalAddress.CountyName;
                    pickUpLocation.StateCode = manualInvoiceCreateRequestViewModel.TerminalAddress.StateCode;
                    var stateId = Context.DataContext.MstStates.Where(t => t.Code == pickUpLocation.StateCode && t.IsActive).Select(t => t.Id).FirstOrDefault();
                    pickUpLocation.StateId = stateId;
                    pickUpLocation.ZipCode = manualInvoiceCreateRequestViewModel.TerminalAddress.ZipCode;
                    pickUpLocation.PickupLocationType = PickupLocationType.Terminal;
                }

                pickUpLocation.TrackableScheduleId = manualInvoiceCreateRequestViewModel.TrackableScheduleId;
                pickUpLocation.IsVariousFobOriginType = manualInvoiceCreateRequestViewModel.IsVariousFobOrigin;
                pickUpLocation.LocationType = (int)LocationType.PickUp;
                pickUpLocation.OrderId = manualInvoiceCreateRequestViewModel.OrderId;
                pickUpLocation.CreatedBy = invoiceModel.CreatedBy;
                pickUpLocation.CreatedDate = invoiceModel.CreatedDate;
                pickUpLocation.Currency = invoiceModel.Currency;
                pickUpLocation.IsValidAddress = true;

                manualInvoiceCreateRequestViewModel.IsDirectTaxCompany = Context.DataContext.DirectTaxes.Any(t => t.CompanyId == manualInvoiceCreateRequestViewModel.BuyerCompanyId && t.StateId == manualInvoiceCreateRequestViewModel.JobStateId && t.IsActive);
                invoiceModel.FuelPickLocation = pickUpLocation;
            }
        }

        private static void SetFuelPickupLocationDetails(InvoiceCreateViewModel manualInvoiceCreateRequestViewModel, DispatchLocationViewModel pickUpLocation)
        {
            if (manualInvoiceCreateRequestViewModel.PickupLocation != null && !string.IsNullOrEmpty(manualInvoiceCreateRequestViewModel.PickupLocation.Address))
            {
                pickUpLocation.Address = manualInvoiceCreateRequestViewModel.PickupLocation.Address;
                pickUpLocation.City = manualInvoiceCreateRequestViewModel.PickupLocation.City;
                pickUpLocation.CountryCode = manualInvoiceCreateRequestViewModel.PickupLocation.CountryCode;
                pickUpLocation.CountyName = manualInvoiceCreateRequestViewModel.PickupLocation.CountyName;
                pickUpLocation.StateCode = manualInvoiceCreateRequestViewModel.PickupLocation.StateCode;
                pickUpLocation.StateId = manualInvoiceCreateRequestViewModel.PickupLocation.StateId;
                pickUpLocation.ZipCode = manualInvoiceCreateRequestViewModel.PickupLocation.ZipCode;
                pickUpLocation.SiteName = manualInvoiceCreateRequestViewModel.PickupLocation.SiteName;
                pickUpLocation.PickupLocationType = PickupLocationType.BulkPlant;
            }
            else
            {
                pickUpLocation.Address = manualInvoiceCreateRequestViewModel.TerminalAddress.Address;
                pickUpLocation.City = manualInvoiceCreateRequestViewModel.TerminalAddress.City;
                pickUpLocation.CountryCode = manualInvoiceCreateRequestViewModel.TerminalAddress.CountryCode;
                pickUpLocation.CountyName = manualInvoiceCreateRequestViewModel.TerminalAddress.CountyName;
                pickUpLocation.StateCode = manualInvoiceCreateRequestViewModel.TerminalAddress.StateCode;
                pickUpLocation.StateId = manualInvoiceCreateRequestViewModel.TerminalStateId;
                pickUpLocation.ZipCode = manualInvoiceCreateRequestViewModel.TerminalAddress.ZipCode;
                pickUpLocation.PickupLocationType = PickupLocationType.Terminal;
            }
        }

        public async Task GetDraftDdtInvoiceViewModel(InvoiceCreateViewModel invoiceRequestModel, InvoiceModel invoiceModel, int assetCount)
        {
            var timeZoneName = invoiceRequestModel.TimeZoneName;
            invoiceModel.UoM = invoiceRequestModel.UoM;
            invoiceModel.Currency = invoiceRequestModel.Currency;
            invoiceModel.SupplierPreferredInvoiceTypeId = invoiceRequestModel.InvoiceTypeId;

            CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
            invoiceModel.ExchangeRate = currencyRateDomain.GetCurrencyRate(invoiceRequestModel.Currency, Currency.USD, DateTimeOffset.Now);
            invoiceModel.PaymentDueDate = GetPaymentDueDate(invoiceRequestModel.PaymentTermId, invoiceRequestModel.NetDays, timeZoneName, invoiceRequestModel.DropEndDate, PaymentDueDateType.InvoiceCreationDate);
            invoiceModel.CreatedBy = invoiceRequestModel.UserId;

            var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
            invoiceModel.CreatedDate = currentDate;
            invoiceModel.UpdatedBy = invoiceRequestModel.UserId;
            invoiceModel.UpdatedDate = currentDate;
            invoiceModel.TerminalId = invoiceRequestModel.TerminalId;
            invoiceModel.CityGroupTerminalId = invoiceRequestModel.CityGroupTerminalId;

            // Set invoice type and waiting action if approval workflow is enabled
            invoiceModel.InvoiceTypeId = CheckApprovalWorkflowAndGetInvoiceType(invoiceRequestModel.IsApprovalWorkflowEnabledForJob, invoiceRequestModel.InvoiceTypeId);
            invoiceModel.WaitingFor = GetApprovalWorkflowWaitingForAction(invoiceRequestModel.IsApprovalWorkflowEnabledForJob, invoiceRequestModel.InvoiceStatusId);
            await SetInvoicePricingToInvoiceModel(invoiceRequestModel, invoiceModel);
            invoiceRequestModel.BolDetails.PricePerGallon = invoiceModel.PricePerGallon;
            invoiceRequestModel.BolDetails.RackPrice = invoiceModel.RackPrice;
            SetInvoiceAmountAndAllowanceToInvoiceModel(invoiceRequestModel, invoiceModel);

            // Set fees with calculation to invoice model
            RemoveDryRunFeeFromManualInvoiceModel(invoiceRequestModel);
            ProcessInvoiceFuelFeesAndSetCalculatedValues(invoiceRequestModel, invoiceModel, invoiceModel.DropEndDate, assetCount);
            await SetInvoiceAdditionDetailToInvoiceModel(invoiceModel, 0, invoiceRequestModel.OrderId);
        }

        public async Task SetInvoiceAdditionDetailToInvoiceModel(InvoiceModel invoiceModel, int previousInvoiceId, int orderId, bool isSellInvoice = false)
        {
            StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
            var additionalDetail = new InvoiceXAdditionalDetailViewModel();

            additionalDetail = await storedProcedureDomain.GetInvoiceAdditionalDetailAsync(orderId, previousInvoiceId, isSellInvoice);
            if (additionalDetail != null)
            {
                if (!string.IsNullOrWhiteSpace(invoiceModel.AdditionalDetail?.Notes))
                    additionalDetail.Notes = invoiceModel.AdditionalDetail.Notes;

                invoiceModel.QuantityIndicatorTypeId = additionalDetail.QuantityIndicatorTypeId;
                var existingAllowance = invoiceModel.AdditionalDetail?.SupplierAllowance ?? 0;
                invoiceModel.AdditionalDetail = additionalDetail.Clone(invoiceModel.AdditionalDetail);
                var allowance = Math.Round(existingAllowance, ApplicationConstants.InvoiceSuppplierAllowanceUnitPriceDecimalDisplay);
                invoiceModel.AdditionalDetail.TotalAllowance = Math.Round(invoiceModel.DroppedGallons * allowance, ApplicationConstants.InvoiceSuppplierAllowanceTotalDecimalDisplay);
            }
            if (invoiceModel.TaxDetails != null && invoiceModel.TaxDetails.IsTrueFillTax)
            {
                invoiceModel.AdditionalDetail.CustomAttributeViewModel.IsTrueFillTax = invoiceModel.TaxDetails.IsTrueFillTax;
                invoiceModel.AdditionalDetail.CustomAttribute = invoiceModel.AdditionalDetail.CustomAttributeViewModel.ToString();
            }
        }

        public void SetInvoiceBaseAmounts(InvoiceModel viewModel, decimal exchangeRate)
        {
            viewModel.BaseDroppedQuntity = VolumeConverter.GetBaseQuantity(viewModel.UoM, viewModel.DroppedGallons);
            viewModel.BasePrice = MoneyConverter.GetBaseAmount(viewModel.Currency, viewModel.PricePerGallon, exchangeRate);
            viewModel.BaseStateTax = MoneyConverter.GetBaseAmount(viewModel.Currency, viewModel.StateTax, exchangeRate);
            viewModel.BaseFedTax = MoneyConverter.GetBaseAmount(viewModel.Currency, viewModel.FedTax, exchangeRate);
            viewModel.BaseSalesTax = MoneyConverter.GetBaseAmount(viewModel.Currency, viewModel.SalesTax, exchangeRate);
            viewModel.BaseBasicAmount = MoneyConverter.GetBaseAmount(viewModel.Currency, viewModel.BasicAmount, exchangeRate);
            viewModel.BaseTotalTaxAmount = MoneyConverter.GetBaseAmount(viewModel.Currency, viewModel.TotalTaxAmount, exchangeRate);
            viewModel.BaseRackPrice = MoneyConverter.GetBaseAmount(viewModel.Currency, viewModel.RackPrice, exchangeRate);
            if (viewModel.TotalFeeAmount.HasValue)
            {
                viewModel.BaseTotalFeeAmount = MoneyConverter.GetBaseAmount(viewModel.Currency, viewModel.TotalFeeAmount.Value, exchangeRate);
            }
        }

        public InvoiceTaxDetailsViewModel GetTaxDetailsFromInputs(List<TaxViewModel> applicableTaxes, Currency currency, int tranId, decimal basicAmount, decimal gallons)
        {
            var taxDetailsViewModel = new InvoiceTaxDetailsViewModel()
            {
                TranId = tranId,
                ReturnCode = tranId
            };

            taxDetailsViewModel.AvaTaxDetails = new List<TaxDetailsViewModel>();

            foreach (var item in applicableTaxes)
            {
                //tax details from add and edit.
                var totalTaxAmount = CalculateTaxAmountForNonStandardFuelType(item.TaxAmount, item.TaxPricingTypeId, basicAmount, gallons);
                taxDetailsViewModel.AvaTaxDetails.Add(new TaxDetailsViewModel()
                {
                    CalculationTypeInd = ApplicationConstants.CalculationTypeForNonStandard,
                    Currency = currency.ToString(),
                    ProductCategory = 1,
                    RateDescription = item.TaxDescription,
                    RateSubtype = ApplicationConstants.RateSubTypeForNonStandard,
                    RateType = ApplicationConstants.ExternalTaxRateTypeTAX,
                    SalesTaxBaseAmount = item.TaxAmount,
                    TaxAmount = totalTaxAmount,
                    TaxExemptionInd = ApplicationConstants.TaxExemptionInd,
                    TaxRate = item.TaxAmount,
                    TaxType = ApplicationConstants.ExternalTaxTypeFUEL,
                    TaxingLevel = ApplicationConstants.ExternalTaxingLevelSTA,
                    UnitOfMeasure = ApplicationConstants.UnitOfMeasure,
                    TaxPricingTypeId = item.TaxPricingTypeId,
                    TradingTaxAmount = totalTaxAmount,
                    TradingCurrency = currency.ToString(),
                    ExchangeRate = 1
                });

                taxDetailsViewModel.TotalTaxAmount += totalTaxAmount;
            }

            return taxDetailsViewModel;
        }

        protected void UpdateWaitingTimeInFeeDetails(List<FeesViewModel> brokerInvoiceFees, List<FeesViewModel> invoiceFees)
        {
            foreach (var fee in brokerInvoiceFees)
            {
                int feeTypeId;
                bool success = Int32.TryParse(fee.FeeTypeId, out feeTypeId);
                if (success)
                {
                    switch (feeTypeId)
                    {
                        case (int)FeeType.DemurrageFeeDestination:
                        case (int)FeeType.DemurrageFeeTerminal:
                        case (int)FeeType.DemurrageOther:
                        case (int)FeeType.Retain:
                        case (int)FeeType.SplitTank:
                            var invoiceFee = invoiceFees.FirstOrDefault(t => (fee.FeeConstraintTypeId == null || t.FeeConstraintTypeId == fee.FeeConstraintTypeId) && t.FeeTypeId == fee.FeeTypeId);
                            if (invoiceFee != null)
                            {
                                fee.TimeInMinutes = invoiceFee.TimeInMinutes;
                            }
                            break;
                    }
                }
            }
        }

        protected void SetStatusMessage(int invoiceTypeId, WaitingAction waitingFor, StatusViewModel response, bool isDtnUploaded, int ddtConversionReason = 0)
        {
            switch (waitingFor)
            {
                case WaitingAction.AvalaraTax:
                    if (ddtConversionReason == (int)DDTConversionReason.AvalaraProductNotMapped)
                        response.StatusMessage = Resource.successMessageDDTCreatedDueToProductNotMappedInAva;
                    else
                        response.StatusMessage = Resource.successMessageDDTCreatedDueToAvaTaxFailure;
                    break;
                case WaitingAction.CustomerApproval:
                    response.StatusMessage = Resource.SuccessMessgeDDTCreatedAsApprovalWorkflowIsEnabled;
                    break;
                case WaitingAction.UpdatedPrice:
                    response.StatusMessage = Resource.errMessageDDTCreatedWaitingForUpdatedPrice;
                    break;
                case WaitingAction.ExceptionApproval:
                    response.StatusMessage = Resource.errMessageDDTCreatedWaitingForExceptionApproval;
                    break;
                case WaitingAction.Images:
                    response.StatusMessage = Resource.errMessageDDTCreatedWaitingForImages;
                    break;
                case WaitingAction.PrePostDipData:
                    response.StatusMessage = Resource.errMessageDDTCreatedWaitingForPrePostDipData;
                    break;
                default:
                    response.StatusMessage = IsDigitalDropTicket(invoiceTypeId) ? Resource.errMessageDropTicketCreateSuccess : Resource.errMessageInvoiceCreateSuccess;
                    break;
            }
            UpdateInvoiceActionResponseStatus(isDtnUploaded, response);
        }

        protected string GetEddtStatusMessage(int invoiceTypeId, int waitingFor, int ddtConversionReason = 0)
        {
            string response = string.Empty;
            switch ((WaitingAction)waitingFor)
            {
                case WaitingAction.AvalaraTax:
                    if (ddtConversionReason == (int)DDTConversionReason.AvalaraProductNotMapped)
                        response = Resource.successMessageDDTCreatedDueToProductNotMappedInAva;
                    else
                        response = Resource.successMessageDDTCreatedDueToAvaTaxFailure;
                    break;
                case WaitingAction.CustomerApproval:
                    response = Resource.SuccessMessgeDDTCreatedAsApprovalWorkflowIsEnabled;
                    break;
                case WaitingAction.UpdatedPrice:
                    response = Resource.errMessageDDTCreatedWaitingForUpdatedPrice;
                    break;
                case WaitingAction.ExceptionApproval:
                    response = Resource.errMessageDDTCreatedWaitingForExceptionApproval;
                    break;
                case WaitingAction.Images:
                    response = Resource.errMessageDDTCreatedWaitingForImages;
                    break;
                default:
                    response = IsDigitalDropTicket(invoiceTypeId) ? Resource.successMessageDdtCreatedFromEddt : Resource.successMessageInvoiceCreatedFromEddt;
                    break;
            }
            return response;
        }

        protected void SetStatusCustomMessage(List<InvoiceCreateResponseViewModel> invoiceCreateResponses, StatusViewModel response)
        {
            foreach (var item in invoiceCreateResponses)
            {
                response.EntityNumber = response.EntityNumber + " " + item.InvoiceNumber;
            }
        }

        protected void SetStatusCustomMessage(string displayInvoiceNumber, StatusViewModel response)
        {
            response.EntityNumber = response.EntityNumber + " " + displayInvoiceNumber;
        }

        //protected QueueMessageViewModel GetEnqueueMessageRequestViewModel(InvoiceCreateResponseViewModel invoiceCreateResponse)
        //{
        //    var createdByUser = Context.DataContext.Users.Where(t => t.Id == invoiceCreateResponse.UserId)
        //                .Select(t => new { CompanyName = t.Company.Name, t.FirstName, t.LastName }).First();

        //    var jsonViewModel = new NotificationInvoiceQueMsg();
        //    jsonViewModel.CreatedByCompanyId = invoiceCreateResponse.SupplierCompanyId;
        //    jsonViewModel.InvoiceId = invoiceCreateResponse.InvoiceId;
        //    jsonViewModel.InvoiceNumber = invoiceCreateResponse.InvoiceNumber;
        //    jsonViewModel.OrderNumber = invoiceCreateResponse.PoNumber;
        //    jsonViewModel.CreatedByCompanyName = createdByUser.CompanyName;
        //    //we are not using this name anywhere..so for now assigning from order as Invoice.user is NULL in dry run and ddt to invoice conversion
        //    jsonViewModel.CreatedByUserName = $"{createdByUser.FirstName} {createdByUser.LastName}";

        //    string json = JsonConvert.SerializeObject(jsonViewModel);

        //    return new QueueMessageViewModel()
        //    {
        //        CreatedBy = invoiceCreateResponse.UserId,
        //        QueueProcessType = QueueProcessType.InvoiceCreated,
        //        JsonMessage = json
        //    };
        //}

        protected static DigitalDropTicketNewsfeedModel GetDigitalDropTicketApprovalNewsfeedModel(CreateSplitLoadInvoiceOutputViewModel invoiceCreateResponse)
        {
            return new DigitalDropTicketNewsfeedModel
            {
                InvoiceId = invoiceCreateResponse.InvoiceId,
                InvoiceNumber = invoiceCreateResponse.InvoiceNumber,
                OrderId = invoiceCreateResponse.OrderId,
                PoNumber = invoiceCreateResponse.PoNumber,
                BuyerCompanyId = invoiceCreateResponse.BuyerCompanyId,
                SupplierCompanyId = invoiceCreateResponse.SupplierCompanyId,
                CreatedDate = invoiceCreateResponse.CreatedDate,
                DriverId = invoiceCreateResponse.DriverId ?? 0,
                DropStartDate = invoiceCreateResponse.DropStartDate,
                DropEndDate = invoiceCreateResponse.DropEndDate,
                DroppedGallons = invoiceCreateResponse.DroppedGallons,
                ApprovalUserId = invoiceCreateResponse.ApprovalUserId ?? 0,
                UoM = invoiceCreateResponse.UoM
            };
        }

        protected static DigitalDropTicketApprovalNewsfeedModel GetApprovalWorkflowNewsfeedModel(UserContext user, CreateSplitLoadInvoiceOutputViewModel invoiceCreateResponse)
        {
            return new DigitalDropTicketApprovalNewsfeedModel
            {
                InvoiceId = invoiceCreateResponse.InvoiceId,
                InvoiceNumber = invoiceCreateResponse.InvoiceNumber,
                OrderId = invoiceCreateResponse.OrderId,
                PoNumber = invoiceCreateResponse.PoNumber,
                BuyerCompanyId = invoiceCreateResponse.BuyerCompanyId,
                SupplierCompanyId = invoiceCreateResponse.SupplierCompanyId,
                ApprovalUserId = invoiceCreateResponse.ApprovalUserId ?? 0,
                TimeZoneName = invoiceCreateResponse.TimeZoneName,
                IsBrokeredOrder = invoiceCreateResponse.IsBrokeredOrder,
                SupplierPreferredInvoiceTypeId = invoiceCreateResponse.SupplierPreferredInvoiceTypeId.Value,
                CreatedBy = user.Id,
                ApprovalUserCompanyId = invoiceCreateResponse.JobCompanyId,
                ApprovalUserCompany = invoiceCreateResponse.JobCompanyName,
                JobId = invoiceCreateResponse.JobId,
                UserName = user.Name,
                SupplierCompanyName = user.CompanyName,
                ApprovalUserName = invoiceCreateResponse.ApprovalUserName,
                InvoiceTypeId = invoiceCreateResponse.InvoiceTypeId
            };
        }

        protected static ManualInvoiceCreatedNewsfeedModel GetManualInvoiceCreatedNewsfeedModel(CreateSplitLoadInvoiceOutputViewModel invoiceCreateResponse)
        {
            return new ManualInvoiceCreatedNewsfeedModel
            {
                InvoiceId = invoiceCreateResponse.InvoiceId,
                InvoiceNumber = invoiceCreateResponse.InvoiceNumber,
                OrderId = invoiceCreateResponse.OrderId,
                PoNumber = invoiceCreateResponse.PoNumber,
                BuyerCompanyId = invoiceCreateResponse.BuyerCompanyId,
                SupplierCompanyId = invoiceCreateResponse.SupplierCompanyId,
                JobId = invoiceCreateResponse.JobId,
                InvoiceTypeId = invoiceCreateResponse.InvoiceTypeId,
                TimeZoneName = invoiceCreateResponse.TimeZoneName,
                DeliveryTypeId = invoiceCreateResponse.DeliveryTypeId,
                OrderCloseDate = invoiceCreateResponse.DropEndDate,
                DropPercentage = invoiceCreateResponse.DropPercentPerDelivery,
                WaitingFor = invoiceCreateResponse.WaitingFor
            };
        }

        private decimal CalculateTaxAmountForNonStandardFuelType(decimal tax, int taxPricingTypeId, decimal invoiceBasicAmount, decimal totalGallonsDropped)
        {
            decimal response = 0;
            switch (taxPricingTypeId)
            {
                case (int)OtherProductTaxPricingType.DollarOnTotalAmount:
                    response = Math.Round(tax, ApplicationConstants.InvoiceTaxTotalAmountDecimalDisplay);
                    break;
                case (int)OtherProductTaxPricingType.DollarPerGallon:
                    response = Math.Round(tax * totalGallonsDropped, ApplicationConstants.InvoiceTaxTotalAmountDecimalDisplay);
                    break;
                case (int)OtherProductTaxPricingType.PercentageOnTotalAmount:
                    response = Math.Round(tax * invoiceBasicAmount / 100, ApplicationConstants.InvoiceTaxTotalAmountDecimalDisplay);
                    break;
                case (int)OtherProductTaxPricingType.PercentagePerGallon:
                    response = Math.Round(tax * totalGallonsDropped / 100, ApplicationConstants.InvoiceTaxTotalAmountDecimalDisplay);
                    break;
                default:
                    response = Math.Round(tax, ApplicationConstants.InvoiceTaxTotalAmountDecimalDisplay);
                    break;
            }
            return response;
        }

        public TaxResponseViewModel GetFtlTaxForStandardProduct(AvalaraServiceViewModel serviceViewModel, ManualInvoiceViewModel manualInvoiceModel, List<FuelFeeViewModel> fuelFees)
        {

            var response = new TaxResponseViewModel();

            try
            {
                if (manualInvoiceModel.IsVariousFobOrigin)
                {
                    if (serviceViewModel.DestinationJobAddress == null)
                    {
                        serviceViewModel.DestinationJobAddress = new AddressViewModel();
                    }
                    serviceViewModel.DestinationJobAddress.City = null;
                    serviceViewModel.DestinationJobAddress.CountyName = null;
                    string statecode;
                    serviceViewModel.DestinationJobAddress.ZipCode = SetFirstZipCodeOfState(manualInvoiceModel.DropAddress.State.Id, serviceViewModel.DestinationJobAddress.StateCode, out statecode);
                    serviceViewModel.DestinationJobAddress.StateCode = statecode;
                }

                var avaTaxInputViewModel = GetAvalaraTaxInputViewModel(serviceViewModel, new InvoiceCreateViewModel(), new BolDetailViewModel(), fuelFees);
                avaTaxInputViewModel.SupplierAllowance = manualInvoiceModel.SupplierAllowance ?? 0;
                avaTaxInputViewModel.IsDirectTaxCompany = manualInvoiceModel.IsDirectTaxCompany;
                avaTaxInputViewModel.IsFobOrigin = manualInvoiceModel.IsVariousFobOrigin;

                avaTaxInputViewModel.NetUnitsDropped = manualInvoiceModel.BolDetails.NetQuantity ?? serviceViewModel.DroppedGallons;
                avaTaxInputViewModel.GrossUnitsDropped = manualInvoiceModel.BolDetails.GrossQuantity ?? serviceViewModel.DroppedGallons;
                avaTaxInputViewModel.BilledUnitsDropped = serviceViewModel.DroppedGallons;

                var taxDetail = InvokeFtlAvalaraTaxService(avaTaxInputViewModel, serviceViewModel.IsSalesTaxExempted);

                if (taxDetail != null)
                {
                    response.StatusCode = taxDetail.StatusCode;
                    response.TaxDetailsViewModel = taxDetail;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCommonDomain", "GetFtlTaxForStandardProduct -InvoiceNumber" + serviceViewModel.InvoiceNumber, ex.Message, ex);
            }

            return response;
        }

        public TaxExclusionType GetTaxEclusionIfExist(int userId)
        {
            var exclusion = Context.DataContext.TaxExclusions.Where(t => t.Company.Users.Any(t1 => t1.Id == userId) && t.IsActive)
                            .Select(t => t.ExclusionType).FirstOrDefault();
            return exclusion;
        }

        public TaxResponseViewModel GetTaxForStandardProduct(AvalaraServiceViewModel serviceViewModel, InvoiceCreateViewModel invoiceCreateViewModel, BolDetailViewModel bolDetails, List<FuelFeeViewModel> fuelFees)
        {
            var response = new TaxResponseViewModel();
            var taxDetail = GetAvalaraTaxes(serviceViewModel, invoiceCreateViewModel, bolDetails, fuelFees);
            if (taxDetail != null)
            {
                response.StatusCode = taxDetail.StatusCode;
                response.FailedStatusCode = taxDetail.FailedStatusCode;
                response.TaxDetailsViewModel = taxDetail;
            }
            return response;
        }

        public TaxResponseViewModel GetFtlOrderTaxForStandardProduct(AvalaraServiceViewModel serviceViewModel, InvoiceCreateViewModel invoiceCreateViewModel, BolDetailViewModel bolDetails, List<FuelFeeViewModel> fuelFees)
        {
            var response = new TaxResponseViewModel();
            var taxDetail = GetFtlAvalaraTaxes(serviceViewModel, invoiceCreateViewModel, bolDetails, fuelFees);
            if (taxDetail != null)
            {
                response.StatusCode = taxDetail.StatusCode;
                response.FailedStatusCode = taxDetail.FailedStatusCode;
                response.TaxDetailsViewModel = taxDetail;
            }
            return response;
        }

        public void SetTaxesToInvoiceCreateViewModel(InvoiceCreateViewModel invoiceCreateModel, InvoiceModel invoiceModel)
        {
            if (invoiceCreateModel.InvoiceStatusId != (int)InvoiceStatus.Draft)
            {
                SetTaxesToCreateInvoiceModel(invoiceCreateModel, invoiceModel);
            }
            if (string.IsNullOrWhiteSpace(invoiceModel.TransactionId) || invoiceModel.TransactionId == "0")
            {
                invoiceModel.TransactionId = invoiceModel.DisplayInvoiceNumber;
            }
        }

        public void SetTaxesToFtlInvoiceCreateViewModel(InvoiceCreateViewModel invoiceCreateModel, InvoiceModel invoiceModel)
        {
            if (invoiceCreateModel.InvoiceStatusId != (int)InvoiceStatus.Draft || !string.IsNullOrEmpty(invoiceCreateModel.AdditionalDetail.SplitLoadChainId))
            {
                SetTaxesToFtlCreateInvoiceModel(invoiceCreateModel, invoiceModel);
            }
            if (string.IsNullOrWhiteSpace(invoiceModel.TransactionId) || invoiceModel.TransactionId == "0")
            {
                invoiceModel.TransactionId = invoiceModel.DisplayInvoiceNumber;
            }
        }

        public void SetAssetDropsToInvoiceModel(ManualInvoiceViewModel manualInvoiceModel, InvoiceModel invoiceModel, string timeZoneName, Job job, int droppedByUserId)
        {
            if (manualInvoiceModel.Assets.Any())
            {
                manualInvoiceModel.Assets.ForEach(t => t.DropDate = invoiceModel.DropStartDate);
                var timeZoneOffset = invoiceModel.DropEndDate.GetOffset(timeZoneName);
                var assetDrops = SetJobAssetId(manualInvoiceModel.Assets, manualInvoiceModel.userId, job);
                var assetDropsWithAdditionDetail = GetAssetDropsWithAdditionalInformation(assetDrops, droppedByUserId, manualInvoiceModel.userId, invoiceModel.DropEndDate);
                invoiceModel.AssetDrops = assetDropsWithAdditionDetail.Select(t => t.ToAssetDropModel(timeZoneOffset)).ToList();
            }
        }

        public List<AssetDropViewModel> SetJobAssetId(List<AssetDropViewModel> assetDrops, int userId, Job job)
        {
            var unSavedAssets = assetDrops.Where(t => t.JobXAssetId == 0).ToList();
            foreach (var newAsset in unSavedAssets)
            {
                var assignToJob = false;
                var asset = job.Company.Assets.FirstOrDefault(t => t.Name.Equals(newAsset.AssetName, StringComparison.InvariantCultureIgnoreCase));
                if (asset != null) //existing asset
                {
                    var jobXasset = job.JobXAssets.LastOrDefault(t => t.AssetId == asset.Id);
                    if (jobXasset != null) //assigned/removed to current job atlease one time
                    {
                        newAsset.JobXAssetId = jobXasset.Id;
                    }
                    else
                    {
                        var assignedToAnotherJob = asset.JobXAssets.FirstOrDefault(t => t.RemovedBy == null && t.RemovedDate == null);
                        if (assignedToAnotherJob != null) // assigned to another job and never assigned to current job
                        {
                            var dropStartDate = newAsset.DropDate.Add(Convert.ToDateTime(newAsset.StartTime).TimeOfDay);
                            var dropEndDate = newAsset.DropDate.Add(Convert.ToDateTime(newAsset.EndTime).TimeOfDay);
                            if (assignedToAnotherJob.AssignedDate >= dropEndDate)
                            {
                                JobXAsset jobXnAsset = new JobXAsset() { AssetId = asset.Id, AssignedBy = userId, AssignedDate = dropStartDate, RemovedBy = userId, RemovedDate = dropEndDate };
                                job.JobXAssets.Add(jobXnAsset);
                                Context.Commit();
                                newAsset.JobXAssetId = jobXnAsset.Id;
                            }
                            else
                            {
                                assignedToAnotherJob.RemovedBy = userId;
                                assignedToAnotherJob.RemovedDate = DateTimeOffset.Now;
                                assignToJob = true;
                            }
                        }
                        else // not assigned to any job
                        {
                            assignToJob = true;
                        }
                    }
                }
                else //new asset
                {
                    AssetViewModel assetModel = new AssetViewModel() { Name = newAsset.AssetName, UserId = userId, IsActive = true, CreatedDate = DateTimeOffset.Now, UpdatedDate = DateTimeOffset.Now, Type = (int)AssetType.Asset };
                    asset = assetModel.ToEntity();
                    job.Company.Assets.Add(asset);
                    assignToJob = true;
                }
                if (assignToJob)
                {
                    JobXAsset jobXnAsset = new JobXAsset() { AssetId = asset.Id, AssignedBy = userId, AssignedDate = DateTimeOffset.Now };
                    job.JobXAssets.Add(jobXnAsset);
                    Context.Commit();
                    newAsset.JobXAssetId = jobXnAsset.Id;
                }
            }
            return assetDrops;
        }

        public InvoiceCreateRequestViewModel SetTaxesToPartialCreditInvoiceModel(InvoiceCreateViewModel invoiceCreateModel, InvoiceModel invoiceModel, ManualInvoiceViewModel manualInputViewModel)
        {
            var response = new InvoiceCreateRequestViewModel();
            try
            {
                manualInputViewModel.TaxType = TaxType.Manual;
                if (manualInputViewModel.TaxDetails.AvaTaxDetails != null && manualInputViewModel.TaxDetails.AvaTaxDetails.Any())
                {
                    manualInputViewModel.TaxDetails.AvaTaxDetails.ForEach(t => t.TradingTaxAmount *= -1);
                    var taxResponse = new InvoiceEditDomain().GetTaxesForPartialCreditInvoice(manualInputViewModel);
                    invoiceModel.TaxDetails = taxResponse.TaxDetailsViewModel;
                }
                if (invoiceModel.TaxDetails != null)
                {
                    invoiceModel.TotalTaxAmount = invoiceModel.TaxDetails.TotalTaxAmount;
                    invoiceModel.TransactionId = invoiceModel.TaxDetails.TranId.ToString();
                }

                invoiceModel.StatusId = GetInvoiceStatusId(invoiceModel.WaitingFor, invoiceCreateModel.InvoiceStatusId);
                SetInvoiceBaseAmounts(invoiceModel, invoiceModel.ExchangeRate);
                //CheckForProcessingFeeOnTotalAmount(invoiceModel);
                response = GetInvoiceCreateRequestViewModel(invoiceCreateModel, invoiceModel);
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCommonDomain", "SetTaxesToPartialCreditInvoiceModel", ex.Message, ex);
            }
            return response;
        }

        public InvoiceCreateRequestViewModel SetTaxesToInvoiceModel(InvoiceCreateViewModel invoiceCreateModel, InvoiceModel invoiceModel)
        {
            var response = new InvoiceCreateRequestViewModel();
            try
            {
                SetTaxesToInvoiceCreateViewModel(invoiceCreateModel, invoiceModel);
                if (invoiceModel.TaxDetails != null && invoiceModel.TaxDetails.IsTrueFillTax)
                {
                    if (invoiceModel.AdditionalDetail.CustomAttributeViewModel == null)
                    {
                        invoiceModel.AdditionalDetail.CustomAttributeViewModel = new InvoiceCustomAttributeViewModel();
                    }
                    invoiceModel.AdditionalDetail.CustomAttributeViewModel.IsTrueFillTax = invoiceModel.TaxDetails.IsTrueFillTax;
                    invoiceModel.AdditionalDetail.CustomAttribute = invoiceModel.AdditionalDetail.CustomAttributeViewModel.ToString();
                }

                if (invoiceModel.FuelFees.Any(t => t.IncludeInPPG))
                {
                    invoiceModel.BasicAmount += invoiceModel.FuelFees.Where(t => t.IncludeInPPG).Select(t => t.TotalFee ?? 0).Sum();
                    invoiceModel.PricePerGallon = invoiceModel.BasicAmount / invoiceModel.DroppedGallons;
                }
                invoiceModel.StatusId = GetInvoiceStatusId(invoiceModel.WaitingFor, invoiceCreateModel.InvoiceStatusId);
                SetInvoiceBaseAmounts(invoiceModel, invoiceModel.ExchangeRate);
                CheckForProcessingFeeOnTotalAmount(invoiceModel);
                response = GetInvoiceCreateRequestViewModel(invoiceCreateModel, invoiceModel);
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCommonDomain", "SetTaxesToInvoiceModel", ex.Message, ex);
            }
            return response;
        }

        public InvoiceCreateRequestViewModel SetTaxesToFtlInvoiceModel(InvoiceCreateViewModel invoiceCreateModel, InvoiceModel invoiceModel)
        {
            var response = new InvoiceCreateRequestViewModel();
            try
            {
                SetTaxesToFtlInvoiceCreateViewModel(invoiceCreateModel, invoiceModel);
                if (invoiceModel.FuelFees.Any(t => t.IncludeInPPG))
                {
                    invoiceModel.BasicAmount += invoiceModel.FuelFees.Where(t => t.IncludeInPPG).Select(t => t.TotalFee ?? 0).Sum();
                    invoiceModel.PricePerGallon = invoiceModel.BasicAmount / invoiceModel.DroppedGallons;
                }
                if (invoiceModel.TaxDetails != null && invoiceModel.TaxDetails.IsTrueFillTax)
                {
                    if (invoiceModel.AdditionalDetail.CustomAttributeViewModel == null)
                    {
                        invoiceModel.AdditionalDetail.CustomAttributeViewModel = new InvoiceCustomAttributeViewModel();
                    }
                    invoiceModel.AdditionalDetail.CustomAttributeViewModel.IsTrueFillTax = invoiceModel.TaxDetails.IsTrueFillTax;
                    invoiceModel.AdditionalDetail.CustomAttribute = invoiceModel.AdditionalDetail.CustomAttributeViewModel.ToString();
                }
                invoiceModel.StatusId = GetInvoiceStatusId(invoiceModel.WaitingFor, invoiceCreateModel.InvoiceStatusId);
                SetInvoiceBaseAmounts(invoiceModel, invoiceModel.ExchangeRate);
                CheckForProcessingFeeOnTotalAmount(invoiceModel);
                response = GetInvoiceCreateRequestViewModel(invoiceCreateModel, invoiceModel);
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCommonDomain", "SetTaxesToFtlInvoiceModel", ex.Message, ex);
            }
            return response;
        }

        protected static void CheckForProcessingFeeOnTotalAmount(InvoiceModel invoiceModel)
        {
            var percentProcessingFee = invoiceModel.FuelFees.FirstOrDefault(t => t.FeeTypeId == (int)FeeType.ProcessingFee && t.FeeSubTypeId == (int)FeeSubType.Percent);
            if (percentProcessingFee != null && invoiceModel.StatusId != (int)InvoiceStatus.Draft)
            {
                var totalAmount = (invoiceModel.BasicAmount + (invoiceModel.TotalFeeAmount ?? 0) + invoiceModel.TotalTaxAmount - invoiceModel.TotalDiscountAmount);
                percentProcessingFee.TotalFee = Math.Round(totalAmount * percentProcessingFee.Fee / 100, ApplicationConstants.InvoiceFeeTotalAmountDecimalDisplay);
                invoiceModel.TotalFeeAmount += percentProcessingFee.TotalFee;
            }
        }

        protected static void CheckForProcessingFeeOnTotalAmount(List<InvoiceModel> invoiceModels)
        {
            var percentProcessingFee = invoiceModels.SelectMany(t => t.FuelFees).FirstOrDefault(t => t.FeeTypeId == (int)FeeType.ProcessingFee && t.FeeSubTypeId == (int)FeeSubType.Percent);
            if (percentProcessingFee != null && invoiceModels.All(t => t.StatusId != (int)InvoiceStatus.Draft))
            {
                var totalAmount = invoiceModels.Sum(t => t.BasicAmount) + invoiceModels.Sum(t => t.TotalFeeAmount ?? 0) + invoiceModels.Sum(t => t.TotalTaxAmount) - invoiceModels.Sum(t => t.TotalDiscountAmount);
                percentProcessingFee.TotalFee = Math.Round(totalAmount * percentProcessingFee.Fee / 100, ApplicationConstants.InvoiceFeeTotalAmountDecimalDisplay);
                invoiceModels.FirstOrDefault().TotalFeeAmount += percentProcessingFee.TotalFee;
            }
        }

        public InvoiceCreateRequestViewModel GetInvoiceCreateRequestViewModelWithTaxWaitingStatus(InvoiceCreateViewModel invoiceCreateModel, InvoiceModel invoiceModel)
        {
            var response = new InvoiceCreateRequestViewModel();
            try
            {
                if (invoiceModel.WaitingFor == WaitingAction.Nothing && invoiceCreateModel.TypeOfFuel != (int)ProductDisplayGroups.OtherFuelType)
                {
                    invoiceModel.WaitingFor = WaitingAction.AvalaraTax;
                    invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
                }

                invoiceModel.TransactionId = invoiceModel.DisplayInvoiceNumber;

                if (invoiceModel.FuelFees.Any(t => t.IncludeInPPG))
                {
                    invoiceModel.BasicAmount += Math.Round(invoiceModel.FuelFees.Where(t => t.IncludeInPPG).Select(t => t.TotalFee ?? 0).Sum(), ApplicationConstants.InvoiceBasicAmountDecimalDisplay);
                    invoiceModel.PricePerGallon = invoiceModel.BasicAmount / invoiceModel.DroppedGallons;
                }
                invoiceModel.StatusId = GetInvoiceStatusId(invoiceModel.WaitingFor, invoiceCreateModel.InvoiceStatusId);
                SetInvoiceBaseAmounts(invoiceModel, invoiceModel.ExchangeRate);
                response = GetInvoiceCreateRequestViewModel(invoiceCreateModel, invoiceModel);
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCommonDomain", "SetTaxesToInvoiceModel", ex.Message, ex);
            }
            return response;
        }

        private InvoiceCreateRequestViewModel GetInvoiceCreateRequestViewModel(InvoiceCreateViewModel invoiceCreateModel, InvoiceModel invoiceModel)
        {
            invoiceModel.DisplayInvoiceNumber = invoiceModel.DisplayInvoiceNumber.FormattedInvoiceNumber(invoiceModel.InvoiceTypeId);
            invoiceModel.CreationMethod = invoiceCreateModel.CreationMethod;

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
                CurrentTrackableScheduleId = invoiceCreateModel.TrackableScheduleId,
                CreationMethod = invoiceCreateModel.CreationMethod,
                CurrentTrackableScheduleStatusId = GetDeliveryScheduleStatus(invoiceCreateModel.TrackableScheduleId, invoiceCreateModel.InvoiceStatusId, invoiceCreateModel.DropEndDate),
                InvoiceModel = invoiceModel
            };
            return response;
        }

        protected void UpdateHedgeAndSpotData(Invoice invoice, int customerCompany, FuelRequest fuelRequest, Job job)
        {
            UpdateHedgeSpotQuantity(invoice, customerCompany, fuelRequest, job);
            if (!IsDigitalDropTicket(invoice.InvoiceTypeId))
            {
                UpdateHedgeSpotAmount(invoice, customerCompany, fuelRequest, job);
            }
        }

        private void UpdateHedgeSpotAmount(Invoice invoice, int customerCompany, FuelRequest fuelRequest, Job job)
        {
            var fees = invoice.TotalFeeAmount ?? 0;
            var previousDroppedAmount = invoice.BasicAmount + fees + invoice.TotalTaxAmount - invoice.TotalDiscountAmount;
            var previousBaseDroppedAmount = invoice.BaseBasicAmount + invoice.BaseTotalTaxAmount
                + MoneyConverter.GetBaseAmount(invoice.Currency, fees, invoice.ExchangeRate)
                - MoneyConverter.GetBaseAmount(invoice.Currency, invoice.TotalDiscountAmount, invoice.ExchangeRate);
            var orderTypeId = fuelRequest.OrderTypeId;
            if (orderTypeId == (int)OrderType.Hedge)
            {
                fuelRequest.HedgeDroppedAmount -= previousDroppedAmount;
                fuelRequest.BaseHedgeDroppedAmount -= previousBaseDroppedAmount;
                if (job.CompanyId == customerCompany)
                {
                    job.HedgeDroppedAmount -= previousDroppedAmount;
                    job.BaseHedgeDroppedAmount -= previousBaseDroppedAmount;
                }
            }
            else
            {
                fuelRequest.SpotDroppedAmount -= previousDroppedAmount;
                fuelRequest.BaseSpotDroppedAmount -= previousBaseDroppedAmount;
                if (job.CompanyId == customerCompany)
                {
                    job.SpotDroppedAmount -= previousDroppedAmount;
                    job.BaseSpotDroppedAmount -= previousBaseDroppedAmount;
                }
            }
        }

        private static void UpdateHedgeSpotQuantity(Invoice invoice, int customerCompany, FuelRequest fuelRequest, Job job)
        {
            var invoiceBaseQuantity = VolumeConverter.GetBaseQuantity(invoice.UoM, invoice.DroppedGallons);
            var orderTypeId = fuelRequest.OrderTypeId;
            if (orderTypeId == (int)OrderType.Hedge)
            {
                fuelRequest.HedgeDroppedGallons -= invoice.DroppedGallons;
                fuelRequest.BaseHedgeDroppedQuantity -= invoiceBaseQuantity;
                if (job.CompanyId == customerCompany)
                {
                    job.HedgeDroppedGallons -= invoice.DroppedGallons;
                    job.BaseHedgeDroppedQuantity -= invoiceBaseQuantity;
                }
            }
            else
            {
                fuelRequest.SpotDroppedGallons -= invoice.DroppedGallons;
                fuelRequest.BaseSpotDroppedQuantity -= invoiceBaseQuantity;
                if (job.CompanyId == customerCompany)
                {
                    job.SpotDroppedGallons -= invoice.DroppedGallons;
                    job.BaseSpotDroppedQuantity -= invoiceBaseQuantity;
                }
            }
        }

        protected int GetDeliveryScheduleStatus(DeliveryScheduleXTrackableSchedule deliveryScheduleXTrackableSchedule, string timeZoneName, int invoiceStatusId, DateTimeOffset dropEndDate, bool isEditing = false)
        {
            int deliveryStatusId = (int)TrackableDeliveryScheduleStatus.Accepted;
            var statusId = deliveryScheduleXTrackableSchedule.DeliverySchedule.StatusId;

            if (isEditing)
            {
                var currentDateTime = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
                if (currentDateTime.Date > deliveryScheduleXTrackableSchedule.Date.Date)
                {
                    return (int)TrackableDeliveryScheduleStatus.Missed;
                }
                else if (currentDateTime.Date == deliveryScheduleXTrackableSchedule.Date.Date)
                {
                    if (currentDateTime.Hour > deliveryScheduleXTrackableSchedule.EndTime.Hours)
                    {
                        return (int)TrackableDeliveryScheduleStatus.Missed;
                    }
                    else
                    {
                        return (int)TrackableDeliveryScheduleStatus.Accepted;
                    }
                }
            }
            else
            {
                if (invoiceStatusId == (int)InvoiceStatus.Draft)
                {
                    return (int)TrackableDeliveryScheduleStatus.Discontinued;
                }
                else
                {
                    if (dropEndDate.Date <= deliveryScheduleXTrackableSchedule.Date.Date)
                    {
                        if (dropEndDate.Hour <= deliveryScheduleXTrackableSchedule.EndTime.Hours)
                        {
                            return statusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ? (int)TrackableDeliveryScheduleStatus.RescheduledCompleted : (int)TrackableDeliveryScheduleStatus.Completed;
                        }
                        else
                        {
                            return statusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ? (int)TrackableDeliveryScheduleStatus.RescheduledLate : (int)TrackableDeliveryScheduleStatus.CompletedLate;
                        }
                    }
                    else if (dropEndDate.Date > deliveryScheduleXTrackableSchedule.Date.Date)
                    {
                        return statusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ? (int)TrackableDeliveryScheduleStatus.RescheduledLate : (int)TrackableDeliveryScheduleStatus.CompletedLate;
                    }
                }
            }
            return deliveryStatusId;
        }

        protected int GetDeliveryScheduleStatus(int? trackableScheduleId, int invoiceStatusId, DateTimeOffset dropEndDate)
        {
            int deliveryStatusId = (int)TrackableDeliveryScheduleStatus.Accepted;
            if (trackableScheduleId.HasValue && trackableScheduleId.Value > 0)
            {
                var trackableSchedule = Context.DataContext.DeliveryScheduleXTrackableSchedules
                                    .Where(t => t.Id == trackableScheduleId.Value)
                                    .Select(t => new
                                    {
                                        t.DeliverySchedule.StatusId,
                                        ScheduleDate = t.Date,
                                        t.EndTime
                                    }).FirstOrDefault();
                if (trackableSchedule != null)
                {
                    var statusId = trackableSchedule.StatusId;
                    if (invoiceStatusId == (int)InvoiceStatus.Draft)
                    {
                        return (int)TrackableDeliveryScheduleStatus.Discontinued;
                    }
                    else if (trackableScheduleId.HasValue)
                    {
                        if (dropEndDate.Date <= trackableSchedule.ScheduleDate.Date)
                        {
                            if (dropEndDate.Hour <= trackableSchedule.EndTime.Hours)
                            {
                                return statusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ? (int)TrackableDeliveryScheduleStatus.RescheduledCompleted : (int)TrackableDeliveryScheduleStatus.Completed;
                            }
                            else
                            {
                                return statusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ? (int)TrackableDeliveryScheduleStatus.RescheduledLate : (int)TrackableDeliveryScheduleStatus.CompletedLate;
                            }
                        }
                        else if (dropEndDate.Date > trackableSchedule.ScheduleDate.Date)
                        {
                            return statusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ? (int)TrackableDeliveryScheduleStatus.RescheduledLate : (int)TrackableDeliveryScheduleStatus.CompletedLate;
                        }
                    }
                }
            }
            return deliveryStatusId;
        }

        protected static int GetInvoiceStatusId(WaitingAction waitingFor, int mobileInvoiceStatusId)
        {
            if (mobileInvoiceStatusId == (int)InvoiceStatus.Draft)
            {
                return (int)InvoiceStatus.Draft;
            }
            else if (waitingFor == WaitingAction.CustomerApproval)
            {
                return (int)InvoiceStatus.WaitingForApproval;
            }
            else
            {
                return (int)InvoiceStatus.Received;
            }
        }

        private AvalaraServiceViewModel GetAvalaraServiceViewModel(InvoiceCreateViewModel invoiceCreateModel, decimal pricePerGallon, string invoiceNumber)
        {
            var avalaraServiceModel = new AvalaraServiceViewModel
            {
                FuelTypeId = invoiceCreateModel.MappedParentFuelTypeId ?? invoiceCreateModel.FuelTypeId,
                FuelProductCode = invoiceCreateModel.FuelProductCode,
                JobUoM = invoiceCreateModel.UoM,
                JobCurrency = invoiceCreateModel.Currency,
                CountryCurrency = invoiceCreateModel.CountryCurrency,
                IsSalesTaxExempted = invoiceCreateModel.IsSalesTaxExempted,
                DestinationJobAddress = invoiceCreateModel.JobAddess,
                SourceTerminalAddress = invoiceCreateModel.PickupLocation != null ? invoiceCreateModel.PickupLocation.ToAddressViewModel() : invoiceCreateModel.TerminalAddress,
                InvoiceNumber = invoiceNumber,
                DroppedGallons = invoiceCreateModel.FuelDropped ?? 0,
                PricePerGallon = pricePerGallon,
                DropEndDate = invoiceCreateModel.DropStartDate,
                Exclusions = GetTaxEclusionIfExist(invoiceCreateModel.UserId),
                BuyerCompanyId = invoiceCreateModel.BuyerCompanyId,
                SupplierCompanyId = invoiceCreateModel.AcceptedCompanyId,
                JobId = invoiceCreateModel.JobId
            };
            return avalaraServiceModel;
        }

        public async Task EditBillingStatement(string chainId, string timeZoneName, int supplierCompanyId)
        {
            try
            {
                var splitInvoices = Context.DataContext.Invoices.Include(t => t.BillingStatementXInvoices).Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == chainId).ToList();
                var activeInvoices = splitInvoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive).ToList();
                var invoices = activeInvoices.Where(t => t.InvoiceTypeId == (int)InvoiceType.Manual).ToList();
                if (invoices.Any() && invoices.Count == activeInvoices.Count)
                {
                    BillingStatement oldStatement = activeInvoices.SelectMany(t => t.BillingStatementXInvoices.Select(t1 => t1.BillingStatement)).FirstOrDefault(t => t != null && t.IsActive);
                    if (oldStatement == null)
                    {
                        oldStatement = splitInvoices.SelectMany(t => t.BillingStatementXInvoices.Select(t1 => t1.BillingStatement)).FirstOrDefault(t => t != null && t.IsActive);
                    }
                    BillingStatementDomain statementDomain = new BillingStatementDomain(this);
                    await statementDomain.GeneateBillingStatementForSplitLoadInvoice(invoices, timeZoneName, supplierCompanyId, oldStatement);
                    if (oldStatement != null)
                    {
                        oldStatement.IsActive = false;
                    }
                    await Context.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceEditDomain", "EditBillingStatement", ex.Message, ex);
            }
        }

        private void SetTaxesToCreateInvoiceModel(InvoiceCreateViewModel invoiceCreateModel, InvoiceModel invoiceModel)
        {
            if (invoiceCreateModel.TypeOfFuel == (int)ProductDisplayGroups.OtherFuelType)
            {
                invoiceModel.TaxDetails = GetTaxDetailsFromInputs(invoiceCreateModel.OtherProductTaxes, invoiceModel.Currency, invoiceModel.Id, invoiceModel.BasicAmount, invoiceModel.DroppedGallons);
            }
            else if (invoiceModel.WaitingFor == WaitingAction.Nothing && !invoiceModel.IsDigitalDropTicket()
                && (invoiceModel.TaxDetails == null || !invoiceModel.TaxDetails.AvaTaxDetails.Any()))
            {
                // Get taxes if end supplier is selected ddt as default invoice type 
                // but intermediate supplier are selected invoice as default invoice type
                var avalaraServiceModel = GetAvalaraServiceViewModel(invoiceCreateModel, invoiceModel.PricePerGallon, invoiceModel.DisplayInvoiceNumber);
                avalaraServiceModel.InvoiceDate = invoiceModel.CreatedDate;
                avalaraServiceModel.BuyerCustomId = invoiceCreateModel.BuyerCustomId;
                avalaraServiceModel.SellerCustomId = invoiceCreateModel.SellerCustomId;
                avalaraServiceModel.IsDirectTaxCompany = invoiceCreateModel.IsDirectTaxCompany;
                avalaraServiceModel.SupplierAllowance = invoiceCreateModel.SupplierAllowance;

                if (invoiceCreateModel.IsVariousFobOrigin && invoiceModel.FuelDropLocation != null)
                {
                    avalaraServiceModel.DestinationJobAddress.Address = null;
                    avalaraServiceModel.DestinationJobAddress.City = null;
                    avalaraServiceModel.DestinationJobAddress.CountyName = null;
                    string stateCode = avalaraServiceModel.DestinationJobAddress.StateCode;
                    avalaraServiceModel.DestinationJobAddress.ZipCode = SetFirstZipCodeOfState(invoiceModel.FuelDropLocation.StateId, avalaraServiceModel.DestinationJobAddress.StateCode, out stateCode);
                    avalaraServiceModel.DestinationJobAddress.StateCode = stateCode;
                    invoiceModel.FuelDropLocation.StateCode = stateCode;
                }
                var taxResponse = GetTaxForStandardProduct(avalaraServiceModel, invoiceCreateModel, invoiceModel.BolDetails.First(), invoiceModel.FuelFees);
                invoiceModel.TaxDetails = taxResponse.TaxDetailsViewModel;
                if (taxResponse.StatusCode == Status.Failed)
                {
                    invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
                    invoiceModel.WaitingFor = WaitingAction.AvalaraTax;
                    invoiceModel.DDTConversionReason = taxResponse.FailedStatusCode;
                }
            }
            if (invoiceModel.TaxDetails != null)
            {
                invoiceModel.TotalTaxAmount = invoiceModel.TaxDetails.TotalTaxAmount;
                invoiceModel.TransactionId = invoiceModel.TaxDetails.TranId.ToString();
            }
        }

        private void SetTaxesToFtlCreateInvoiceModel(InvoiceCreateViewModel invoiceCreateModel, InvoiceModel invoiceModel)
        {
            if (invoiceCreateModel.TypeOfFuel == (int)ProductDisplayGroups.OtherFuelType)
            {
                invoiceModel.TaxDetails = GetTaxDetailsFromInputs(invoiceCreateModel.OtherProductTaxes, invoiceModel.Currency, invoiceModel.Id, invoiceModel.BasicAmount, invoiceModel.DroppedGallons);
            }
            else if (invoiceModel.WaitingFor == WaitingAction.Nothing && !invoiceModel.IsDigitalDropTicket()
                && (invoiceModel.TaxDetails == null || !invoiceModel.TaxDetails.AvaTaxDetails.Any()))
            {
                // Get taxes if end supplier is selected ddt as default invoice type 
                // but intermediate supplier are selected invoice as default invoice type
                //NEED TO GENERATE LIST OF service models
                var avalaraServiceModel = GetAvalaraServiceViewModel(invoiceCreateModel, invoiceModel.PricePerGallon, invoiceModel.DisplayInvoiceNumber);
                avalaraServiceModel.InvoiceDate = invoiceModel.CreatedDate;
                avalaraServiceModel.BuyerCustomId = invoiceCreateModel.BuyerCustomId;
                avalaraServiceModel.SellerCustomId = invoiceCreateModel.SellerCustomId;
                avalaraServiceModel.IsDirectTaxCompany = invoiceCreateModel.IsDirectTaxCompany;
                avalaraServiceModel.SupplierAllowance = invoiceCreateModel.SupplierAllowance;

                if (invoiceCreateModel.IsVariousFobOrigin && invoiceModel.FuelDropLocation != null)
                {
                    avalaraServiceModel.DestinationJobAddress.Address = null;
                    avalaraServiceModel.DestinationJobAddress.City = null;
                    avalaraServiceModel.DestinationJobAddress.CountyName = null;
                    string stateCode = avalaraServiceModel.DestinationJobAddress.StateCode;
                    avalaraServiceModel.DestinationJobAddress.ZipCode = SetFirstZipCodeOfState(invoiceModel.FuelDropLocation.StateId, avalaraServiceModel.DestinationJobAddress.StateCode, out stateCode);
                    avalaraServiceModel.DestinationJobAddress.StateCode = stateCode;
                    invoiceModel.FuelDropLocation.StateCode = stateCode;
                }

                var taxResponse = GetFtlOrderTaxForStandardProduct(avalaraServiceModel, invoiceCreateModel, invoiceModel.BolDetails.First(), invoiceModel.FuelFees);
                invoiceModel.TaxDetails = taxResponse.TaxDetailsViewModel;
                if (taxResponse.StatusCode == Status.Failed)
                {
                    invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
                    invoiceModel.WaitingFor = WaitingAction.AvalaraTax;
                    invoiceModel.DDTConversionReason = taxResponse.FailedStatusCode;
                }
            }
            if (invoiceModel.TaxDetails != null)
            {
                invoiceModel.TotalTaxAmount = invoiceModel.TaxDetails.TotalTaxAmount;
                invoiceModel.TransactionId = invoiceModel.TaxDetails.TranId.ToString();
            }
        }

        private InvoiceTaxDetailsViewModel GetAvalaraTaxes(AvalaraServiceViewModel serviceViewModel, InvoiceCreateViewModel invoiceCreateViewModel, BolDetailViewModel bolDetails, List<FuelFeeViewModel> fuelFees)
        {
            InvoiceTaxDetailsViewModel response = null;
            try
            {
                var avaTaxInputViewModel = GetAvalaraTaxInputViewModel(serviceViewModel, invoiceCreateViewModel, bolDetails, fuelFees);
                response = InvokeAvalaraTaxService(avaTaxInputViewModel, serviceViewModel.IsSalesTaxExempted);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCommonDomain", "GetAvalaraTaxes -InvoiceNumber" + serviceViewModel.InvoiceNumber, ex.Message, ex);
            }
            return response;
        }

        public TaxResponseViewModel SetConsolidatedAvalaraTaxes(List<InvoiceModel> invoiceModels)
        {
            var response = new TaxResponseViewModel();

            if (invoiceModels.All(t => t.FuelDropLocation.CountryCode.IsValidCountryForTax()) && !invoiceModels.Any(t => t.IsProcessWithoutTax))
            {
                var taxDetails = GetConsolidatedAvalaraTaxes(invoiceModels);

                if (taxDetails != null)
                {
                    var firstInvoice = invoiceModels.FirstOrDefault(t => t.TypeOfFuel != (int)ProductTypes.NonStandardFuel);
                    firstInvoice.TaxDetails = taxDetails;
                    firstInvoice.TotalTaxAmount = firstInvoice.TaxDetails.TotalTaxAmount;
                    firstInvoice.TransactionId = taxDetails.TranId.ToString();
                    if (taxDetails.IsTrueFillTax)
                    {
                        if (firstInvoice.AdditionalDetail == null)
                        {
                            firstInvoice.AdditionalDetail = new InvoiceXAdditionalDetailViewModel() { CustomAttributeViewModel = new InvoiceCustomAttributeViewModel() };
                        }
                        else if (firstInvoice.AdditionalDetail.CustomAttributeViewModel == null)
                        {
                            firstInvoice.AdditionalDetail.CustomAttributeViewModel = new InvoiceCustomAttributeViewModel();
                        }

                        firstInvoice.AdditionalDetail.CustomAttributeViewModel.IsTrueFillTax = firstInvoice.TaxDetails.IsTrueFillTax;
                        firstInvoice.AdditionalDetail.CustomAttribute = firstInvoice.AdditionalDetail.CustomAttributeViewModel.ToString();
                    }
                    response.StatusCode = taxDetails.StatusCode;
                    response.FailedStatusCode = taxDetails.FailedStatusCode;
                    response.TaxDetailsViewModel = taxDetails;

                    foreach (var item in invoiceModels)
                    {
                        SetInvoiceBaseAmounts(item, item.ExchangeRate);
                    }
                }
            }
            else
                response.StatusCode = Status.Success;

            if (response.StatusCode == Status.Success)
            {
                CheckForProcessingFeeOnTotalAmount(invoiceModels);
            }

            return response;
        }
        protected InvoiceTaxDetailsViewModel GetConsolidatedAvalaraTaxes(List<InvoiceModel> invoiceModels)
        {
            InvoiceTaxDetailsViewModel response = null;
            try
            {
                var newInvoiceModels = invoiceModels.Where(t => t.TypeOfFuel != (int)ProductTypes.NonStandardFuel && t.TypeOfFuel != (int)ProductTypes.Additives).ToList();
                if (newInvoiceModels != null && newInvoiceModels.Any())
                {
                    newInvoiceModels = UpdateInvoiceModelAddressForVariousJob(newInvoiceModels);
                    var serviceViewModel = GetAvalaraServiceViewModel(newInvoiceModels.FirstOrDefault().OrderId.Value); //common details for Job
                    var avaTaxMultipleInputViewModel = GetAvalaraTaxMultipleInputViewModel(newInvoiceModels, serviceViewModel);
                    response = InvokeAvalaraTaxMultipleInputService(avaTaxMultipleInputViewModel, serviceViewModel.IsSalesTaxExempted);
                }
            }
            catch (Exception ex)
            {
                if (response != null)
                    response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("InvoiceCommonDomain", "GetConsolidatedAvalaraTaxes -InvoiceNumber" + invoiceModels.FirstOrDefault()?.DisplayInvoiceNumber, ex.Message, ex);
            }
            return response;
        }

        private List<InvoiceModel> UpdateInvoiceModelAddressForVariousJob(List<InvoiceModel> newInvoiceModels)
        {
            foreach (var item in newInvoiceModels)
            {
                if (item.IsVariousFobOrigin && item.FuelDropLocation != null && !item.FuelDropLocation.IsAddressAvailable)
                {
                    item.FuelDropLocation.CountyName = null;
                    item.FuelDropLocation.Address = null;
                    item.FuelDropLocation.City = null;
                    string stateCode = item.FuelDropLocation.StateCode;
                    item.FuelDropLocation.ZipCode = SetFirstZipCodeOfState(item.FuelDropLocation.StateId, stateCode, out stateCode);
                    item.FuelDropLocation.StateCode = stateCode;
                }
            }
            return newInvoiceModels;
        }

        private AvalaraServiceViewModel GetAvalaraServiceViewModel(int orderId)
        {
            return Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => new AvalaraServiceViewModel()
            {
                BuyerCompanyId = t.BuyerCompanyId,
                SupplierCompanyId = t.AcceptedCompanyId,
                Exclusions = t.Company.TaxExclusions.Where(t1 => t1.IsActive).Select(t1 => t1.ExclusionType).FirstOrDefault(),
                IsDirectTaxCompany = t.BuyerCompany.DirectTaxes.Any(t1 => t1.StateId == t.FuelRequest.Job.StateId && t1.IsActive),
                IsSalesTaxExempted = t.FuelRequest.Job.JobBudget.IsTaxExempted,
                JobCurrency = t.FuelRequest.Job.Currency,
                CountryCurrency = t.FuelRequest.Job.MstCountry.Currency,
                BuyerCustomId = t.BuyerCompany.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive).EntityCustomId,
                SellerCustomId = t.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive).EntityCustomId
            }).FirstOrDefault();
        }

        private InvoiceTaxDetailsViewModel GetFtlAvalaraTaxes(AvalaraServiceViewModel serviceViewModel, InvoiceCreateViewModel invoiceCreateViewModel, BolDetailViewModel bolDetails, List<FuelFeeViewModel> fuelFees)
        {
            InvoiceTaxDetailsViewModel response = null;
            try
            {
                var avaTaxInputViewModel = GetFtlAvalaraTaxInputViewModel(serviceViewModel, invoiceCreateViewModel, bolDetails, fuelFees);
                response = InvokeFtlAvalaraTaxService(avaTaxInputViewModel, serviceViewModel.IsSalesTaxExempted);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCommonDomain", "GetFtlAvalaraTaxes -InvoiceNumber" + serviceViewModel.InvoiceNumber, ex.Message, ex);
            }
            return response;
        }

        private AvalaraTaxInputViewModel GetAvalaraTaxInputViewModel(AvalaraServiceViewModel serviceViewModel, InvoiceCreateViewModel invoiceCreateViewModel, BolDetailViewModel bolDetails, List<FuelFeeViewModel> fuelFees)
        {
            var avaTaxInputViewModel = serviceViewModel.ToAvalaraTaxViewModel(invoiceCreateViewModel, bolDetails, fuelFees);
            CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
            avaTaxInputViewModel.CurrencyRates = currencyRateDomain.GetCurrencyRatesForAvalara(Currency.USD, serviceViewModel.CountryCurrency, serviceViewModel.DropEndDate, avaTaxInputViewModel.Currency);
            return avaTaxInputViewModel;
        }

        private AvalaraTaxInputViewModel GetFtlAvalaraTaxInputViewModel(AvalaraServiceViewModel serviceViewModel, InvoiceCreateViewModel invoiceCreateViewModel, BolDetailViewModel bolDetails, List<FuelFeeViewModel> fuelFees)
        {
            var avaTaxInputViewModels = serviceViewModel.ToFtlAvalaraTaxViewModel(invoiceCreateViewModel, bolDetails, fuelFees);
            CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
            avaTaxInputViewModels.CurrencyRates = currencyRateDomain.GetCurrencyRatesForAvalara(Currency.USD, serviceViewModel.CountryCurrency, serviceViewModel.DropEndDate, avaTaxInputViewModels.Currency);
            return avaTaxInputViewModels;
        }
        private AvalaraTaxMultipleInputViewModel GetAvalaraTaxMultipleInputViewModel(List<InvoiceModel> invoiceModels, AvalaraServiceViewModel serviceViewModel)
        {
            var avaTaxInputViewModel = invoiceModels.ToAvalaraTaxMultipleInputViewModel(serviceViewModel);
            CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
            avaTaxInputViewModel.CurrencyRates = currencyRateDomain.GetCurrencyRatesForAvalara(Currency.USD, serviceViewModel.CountryCurrency, invoiceModels.FirstOrDefault().DropEndDate.Date, avaTaxInputViewModel.Currency);
            return avaTaxInputViewModel;
        }
        private static InvoiceTaxDetailsViewModel InvokeAvalaraTaxService(AvalaraTaxInputViewModel avaTaxInputViewModel, bool isSalesTaxExempted)
        {
            InvoiceTaxDetailsViewModel response;
            using (var tracer = new Tracer("InvoiceCommonDomain", "InvokeAvalaraTaxService"))
            {
                var avaDomain = AvalaraDomain.InvokeProcessTransactions_5_27_0(avaTaxInputViewModel);
                var currencyRate = avaTaxInputViewModel.CurrencyRates.FirstOrDefault(t => t.ToCurrency == avaTaxInputViewModel.Currency.ToString());
                var exchangeRate = currencyRate == null || avaTaxInputViewModel.Currency == Currency.CAD ? 1 : currencyRate.ExchangeRate;
                response = avaDomain.ToResponseViewModel(isSalesTaxExempted, exchangeRate);
            }
            return response;
        }

        private static InvoiceTaxDetailsViewModel InvokeFtlAvalaraTaxService(AvalaraTaxInputViewModel avaTaxInputViewModel, bool isSalesTaxExempted)
        {
            InvoiceTaxDetailsViewModel response = null;
            using (var tracer = new Tracer("InvoiceCommonDomain", "InvokeFtlAvalaraTaxService"))
            {
                var avaDomain = AvalaraDomain.InvokeProcessTransactions_5_27_0_For_FTL(avaTaxInputViewModel);
                if (avaDomain.Result != null)
                {
                    var currencyRate = avaTaxInputViewModel.CurrencyRates.FirstOrDefault(t => t.ToCurrency == avaTaxInputViewModel.Currency.ToString());
                    var exchangeRate = currencyRate == null || avaTaxInputViewModel.Currency == Currency.CAD ? 1 : currencyRate.ExchangeRate;
                    response = avaDomain.ToResponseViewModel(isSalesTaxExempted, exchangeRate);
                }
            }
            return response;
        }
        private static InvoiceTaxDetailsViewModel InvokeAvalaraTaxMultipleInputService(AvalaraTaxMultipleInputViewModel avaTaxInputViewModel, bool isSalesTaxExempted)
        {
            InvoiceTaxDetailsViewModel response = null;
            using (var tracer = new Tracer("InvoiceCommonDomain", "InvokeAvalaraTaxMultipleInputService"))
            {
                var avaDomain = AvalaraDomain.InvokeProcessTransactions_5_27_0_New(avaTaxInputViewModel);
                if (avaDomain.Result != null)
                {
                    var currencyRate = avaTaxInputViewModel.CurrencyRates.FirstOrDefault(t => t.ToCurrency == avaTaxInputViewModel.Currency.ToString());
                    var exchangeRate = currencyRate == null || avaTaxInputViewModel.Currency == Currency.CAD ? 1 : currencyRate.ExchangeRate;
                    response = avaDomain.ToResponseViewModel(isSalesTaxExempted, exchangeRate, null);
                }
            }
            return response;
        }
        private static int CheckApprovalWorkflowAndGetInvoiceType(bool isApprovalWorkflowEnabled, int invoiceTypeId)
        {
            if (isApprovalWorkflowEnabled)
            {
                invoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceTypeId);
            }
            return invoiceTypeId;
        }

        public static int GetInvoiceCreationTypeToDdt(int invoiceTypeId)
        {
            if (!IsDigitalDropTicket(invoiceTypeId))
            {
                if (invoiceTypeId == (int)InvoiceType.MobileApp)
                    invoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
                else
                    invoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
            }
            return invoiceTypeId;
        }

        private static WaitingAction GetApprovalWorkflowWaitingForAction(bool isApprovalWorkflowEnabled, int invoiceStatusId)
        {
            var response = WaitingAction.Nothing;
            if (isApprovalWorkflowEnabled && invoiceStatusId != (int)InvoiceStatus.Draft)
            {
                response = WaitingAction.CustomerApproval;
            }
            return response;
        }

        protected async Task SetInvoicePricingToInvoiceModel(InvoiceCreateViewModel invoiceRequestModel, InvoiceModel invoiceModel)
        {
            if (invoiceRequestModel.InvoiceStatusId != (int)InvoiceStatus.Draft && !invoiceRequestModel.IsApprovalWorkflowEnabledForJob)
            {
                // Get fuel pricing based on selected pricing type
                var fuelPricingRequestViewModel = GetFuelPricingRequestViewModel(invoiceRequestModel, invoiceModel.WaitingFor);
                var pricingData = await GetFuelPriceByPricingTypeAsync(fuelPricingRequestViewModel, invoiceRequestModel.TypeOfFuel);
                if (pricingData.WaitingFor != WaitingAction.UpdatedPrice)
                {
                    invoiceRequestModel.SupplierCost = pricingData.FuelCost;
                    invoiceRequestModel.SupplierCostTypeId = pricingData.FuelCostTypeId;
                    invoiceModel.PricePerGallon = pricingData.PricePerGallon;
                    invoiceModel.RackPrice = pricingData.TerminalPrice;
                    if (invoiceRequestModel.IsBuySellOrder)
                    {
                        invoiceModel.BuySellBasePPG = pricingData.PricePerGallon;
                    }
                }
                // Set invoice type and waiting action if price is not available for drop date
                invoiceModel.InvoiceTypeId = CheckUpdatedPriceWaitingActionAndGetInvoiceType(pricingData, invoiceModel.InvoiceTypeId);
                invoiceModel.WaitingFor = pricingData.WaitingFor;
            }
            else
            {
                invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
            }
        }

        protected void SetInvoiceAmountAndAllowanceToInvoiceModel(InvoiceCreateViewModel invoiceRequestModel, InvoiceModel invoiceModel)
        {
            if (!invoiceRequestModel.IsApprovalWorkflowEnabledForJob && invoiceModel.WaitingFor != WaitingAction.UpdatedPrice)
            {
                if (invoiceRequestModel.IsBuySellOrder)
                {
                    invoiceModel.PricePerGallon = AddMarkupToPricePerGallon(invoiceRequestModel, invoiceModel.BuySellBasePPG);
                }
                invoiceModel.BasicAmount = Math.Round(invoiceModel.DroppedGallons * invoiceModel.PricePerGallon, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);

                SetTotalAllowanceToInvoiceModel(invoiceModel);
            }
        }

        public async Task SetEditInvoicePricingToInvoiceModel(dynamic order, InvoiceModel invoiceModel, bool isAssetAvailable = false)
        {
            invoiceModel.PricePerGallon = 0;
            invoiceModel.RackPrice = 0;
            if (invoiceModel.StatusId != (int)InvoiceStatus.Draft && !order.IsApprovalWorkflowEnabled
                || (invoiceModel.TerminalId != 0 && invoiceModel.TerminalId != order.OriginalInvoice.TerminalId))
            {
                var fuelPricingModel = new FuelPricingRequestViewModel
                {
                    FuelTypeId = order.FuelTypeId,
                    TerminalId = invoiceModel.TerminalId ?? order.OriginalInvoice.TerminalId,
                    CityGroupTerminalId = order.OriginalInvoice.CityGroupTerminalId,
                    PricingTypeId = order.PricingTypeId,
                    DropEndDate = invoiceModel.DropEndDate,
                    Currency = order.Currency,
                    WaitingFor = invoiceModel.WaitingFor,
                    DroppedQuantity = invoiceModel.DroppedGallons
                };

                var droppedQty = invoiceModel.DroppedGallons;
                if ((invoiceModel.PricingTypeId == (int)PricingType.PricePerGallon || invoiceModel.PricingTypeId == (int)PricingType.Suppliercost) && invoiceModel.IsMarineLocation &&
                    invoiceModel.ConvertedQuantity != null && (invoiceModel.UoM == UoM.MetricTons || invoiceModel.UoM == UoM.Barrels))
                {
                    fuelPricingModel.DroppedQuantity = invoiceModel.ConvertedQuantity.Value;
                    droppedQty = invoiceModel.ConvertedQuantity.Value;
                }

                SetTierPricingDetailsFromOriginalInvoice(order, fuelPricingModel);

                if (order.IsFTL && order.FuelRequestPricingDetail != null)
                {
                    fuelPricingModel.FuelRequestPricingDetails.FuelRequestId = order.FuelRequestPricingDetail.FuelRequestId;
                    fuelPricingModel.FuelRequestPricingDetails.PricingQuantityIndicatorTypeId = order.PricingQuantityIndicatorTypeId;
                    fuelPricingModel.FuelRequestPricingDetails.TruckLoadTypeId = order.TruckLoadTypeId;
                    fuelPricingModel.FuelRequestPricingDetails.StateDefaultQuantityIndicatorId = order.QuantityIndicatorTypeId;
                }
                fuelPricingModel.FuelRequestPricingDetails.RequestPriceDetailId = order.FuelRequestPricingDetail.RequestPriceDetailId;
                // Get fuel pricing based on selected pricing type
                var pricingData = await GetFuelPriceByPricingTypeAsync(fuelPricingModel, order.ProductDisplayGroupId);
                if (pricingData.WaitingFor != WaitingAction.UpdatedPrice)
                {
                    invoiceModel.PricePerGallon = pricingData.PricePerGallon;
                    invoiceModel.RackPrice = pricingData.TerminalPrice;

                    invoiceModel.BasicAmount = Math.Round(droppedQty * invoiceModel.PricePerGallon, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);
                    SetTotalAllowanceToInvoiceModel(invoiceModel);

                    //if (order.IsFTL)
                    //{
                    //    if (!string.IsNullOrEmpty(order.OriginalInvoice.SplitLoadChainId))
                    //    {
                    //        SetTotalAllowanceToInvoiceModel(invoiceModel);
                    //    }
                    //    else
                    //    {
                    //        SetFTLPricingToInvoiceModel(invoiceModel, isAssetAvailable);
                    //    }
                    //}

                    // MFN - update fixed pricing for UoM = MT or Barrel to convert pricing into per gallon/litre
                    if (invoiceModel.IsMarineLocation)
                    {
                        if (invoiceModel.PricingTypeId == (int)PricingType.PricePerGallon || invoiceModel.PricingTypeId == (int)PricingType.Suppliercost)
                        {
                            var invoiceDomain = new InvoiceDomain(this);
                            var modelForConversion = new MFNConversionRequestViewModel() { DroppedGallons = invoiceModel.PricePerGallon, JobCountryId = order.Job.CountryId, UoM = invoiceModel.UoM };
                            var gravity = invoiceModel.Gravity.HasValue && invoiceModel.Gravity > 0 ? invoiceModel.Gravity.Value : 0;
                            modelForConversion.ConversionFactor = gravity;
                            var conversionResponse = await invoiceDomain.ValidateGravityAndConvertForMFN(modelForConversion);

                            // update the pricing to per gallon/litre for avalara
                            invoiceModel.ConvertedPricing = conversionResponse.ConvertedQty;
                        }
                        else if (invoiceModel.PricingTypeId == (int)PricingType.RackAverage || invoiceModel.PricingTypeId == (int)PricingType.RackHigh || invoiceModel.PricingTypeId == (int)PricingType.RackLow)
                        {
                            // for market based pricing, update only converted pricing value
                            var invoiceDomain = new InvoiceDomain(this);
                            var modelForConversion = new MFNConversionRequestViewModel() { DroppedGallons = invoiceModel.PricePerGallon, JobCountryId = order.Job.CountryId, UoM = invoiceModel.UoM };
                            var gravity = invoiceModel.Gravity.HasValue && invoiceModel.Gravity > 0 ? invoiceModel.Gravity.Value : 0;
                            modelForConversion.ConversionFactor = gravity;
                            var conversionResponse = await invoiceDomain.ValidateAndConvertPricingForMarketBasedMFN(modelForConversion);
                            invoiceModel.ConvertedPricing = conversionResponse.ConvertedQty;
                        }
                    }
                }
                // Set invoice type and waiting action if price is not available for drop date
                invoiceModel.InvoiceTypeId = CheckUpdatedPriceWaitingActionAndGetInvoiceType(pricingData, invoiceModel.InvoiceTypeId);
                invoiceModel.WaitingFor = pricingData.WaitingFor;
            }
            else
            {
                invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
            }
        }

        private static void SetTierPricingDetailsFromOriginalInvoice(dynamic order, FuelPricingRequestViewModel fuelPricingModel)
        {
            if (order.OriginalInvoice.BolWithTier != null)
            {
                fuelPricingModel.TierMaxQuantity = order.OriginalInvoice.BolWithTier.TierMaxQuantity;
                fuelPricingModel.TierMinQuantity = order.OriginalInvoice.BolWithTier.TierMinQuantity;
            }
        }

        public async Task SetPricingToInvoiceModelForEditBrokeredOrder(dynamic order, InvoiceModel invoiceModel)
        {
            invoiceModel.PricePerGallon = 0;
            invoiceModel.RackPrice = 0;
            if (invoiceModel.StatusId != (int)InvoiceStatus.Draft && !order.IsApprovalWorkflowEnabled
                || (invoiceModel.TerminalId != 0 && invoiceModel.TerminalId != order.OriginalInvoice.TerminalId))
            {
                var fuelPricingModel = new FuelPricingRequestViewModel
                {
                    FuelTypeId = order.FuelTypeId,
                    TerminalId = invoiceModel.TerminalId ?? order.OriginalInvoice.TerminalId,
                    CityGroupTerminalId = order.OriginalInvoice.CityGroupTerminalId,
                    PricingTypeId = order.PricingTypeId,
                    DropEndDate = invoiceModel.DropEndDate,
                    Currency = order.Currency,
                    WaitingFor = invoiceModel.WaitingFor,
                    DroppedQuantity = invoiceModel.DroppedGallons
                };

                var droppedQty = invoiceModel.DroppedGallons;
                if ((invoiceModel.PricingTypeId == (int)PricingType.PricePerGallon || invoiceModel.PricingTypeId == (int)PricingType.Suppliercost) && invoiceModel.IsMarineLocation &&
                    invoiceModel.ConvertedQuantity != null && (invoiceModel.UoM == UoM.MetricTons || invoiceModel.UoM == UoM.Barrels))
                {
                    fuelPricingModel.DroppedQuantity = invoiceModel.ConvertedQuantity.Value;
                    droppedQty = invoiceModel.ConvertedQuantity.Value;
                }

                SetTierPricingDetailsFromOriginalInvoice(order, fuelPricingModel);

                if (order.IsFTL && order.FuelRequestPricingDetail != null)
                {
                    fuelPricingModel.FuelRequestPricingDetails.FuelRequestId = order.FuelRequestPricingDetail.FuelRequestId;
                    fuelPricingModel.FuelRequestPricingDetails.PricingQuantityIndicatorTypeId = order.PricingQuantityIndicatorTypeId;
                    fuelPricingModel.FuelRequestPricingDetails.TruckLoadTypeId = order.TruckLoadTypeId;
                    fuelPricingModel.FuelRequestPricingDetails.StateDefaultQuantityIndicatorId = order.QuantityIndicatorTypeId;
                }
                fuelPricingModel.FuelRequestPricingDetails.RequestPriceDetailId = order.FuelRequestPricingDetail.RequestPriceDetailId;
                // Get fuel pricing based on selected pricing type
                var pricingData = await GetFuelPriceByPricingTypeAsync(fuelPricingModel, order.ProductDisplayGroupId);
                if (pricingData.WaitingFor != WaitingAction.UpdatedPrice)
                {
                    invoiceModel.PricePerGallon = pricingData.PricePerGallon;
                    invoiceModel.RackPrice = pricingData.TerminalPrice;
                    invoiceModel.BasicAmount = Math.Round(droppedQty * invoiceModel.PricePerGallon, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);
                    invoiceModel.WaitingFor = pricingData.WaitingFor;
                    SetTotalAllowanceToInvoiceModel(invoiceModel);
                    //if (order.IsFTL)
                    //{
                    //    if (!string.IsNullOrEmpty(order.OriginalInvoice.SplitLoadChainId))
                    //    {
                    //        SetTotalAllowanceToInvoiceModel(invoiceModel);
                    //    }
                    //    else
                    //    {
                    //        SetFTLPricingToInvoiceModel(invoiceModel);
                    //    }
                    //}

                    // MFN - update fixed pricing for UoM = MT or Barrel to convert pricing into per gallon/litre
                    if (invoiceModel.IsMarineLocation && (invoiceModel.PricingTypeId == (int)PricingType.PricePerGallon || invoiceModel.PricingTypeId == (int)PricingType.Suppliercost))
                    {
                        var invoiceDomain = new InvoiceDomain(this);
                        //if (invoiceModel.PricingTypeId == (int)PricingType.Suppliercost && invoiceModel.UoM == UoM.MetricTons)
                        //{

                        //}
                        var modelForConversion = new MFNConversionRequestViewModel() { DroppedGallons = invoiceModel.PricePerGallon, JobCountryId = order.Job.CountryId, UoM = invoiceModel.UoM };
                        var gravity = invoiceModel.Gravity.HasValue && invoiceModel.Gravity > 0 ? invoiceModel.Gravity.Value : 0;
                        modelForConversion.ConversionFactor = gravity;
                        var conversionResponse = await invoiceDomain.ValidateGravityAndConvertForMFN(modelForConversion);

                        // update the pricing to per gallon/litre for avalara
                        invoiceModel.ConvertedPricing = conversionResponse.ConvertedQty;
                    }
                }
            }
        }

        protected async Task SetEditInvoicePricingToInvoiceModel(ManualInvoiceViewModel manualInvoiceModel, InvoiceCreateViewModel invoiceRequestModel, InvoiceModel invoiceModel)
        {
            if (invoiceRequestModel.InvoiceStatusId != (int)InvoiceStatus.Draft && !invoiceRequestModel.IsApprovalWorkflowEnabledForJob)
            {
                // Get fuel pricing based on selected pricing type
                var fuelPricingRequestViewModel = GetFuelPricingRequestViewModel(invoiceRequestModel, invoiceModel.WaitingFor);
                var pricingData = await GetFuelPriceByPricingTypeAsync(fuelPricingRequestViewModel, invoiceRequestModel.TypeOfFuel);
                if (pricingData.WaitingFor != WaitingAction.UpdatedPrice)
                {
                    invoiceModel.PricePerGallon = pricingData.PricePerGallon;
                    invoiceModel.RackPrice = pricingData.TerminalPrice;
                    invoiceModel.BasicAmount = Math.Round(invoiceModel.DroppedGallons * invoiceModel.PricePerGallon, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);

                    if (invoiceRequestModel.IsFTL)
                    {
                        if (!string.IsNullOrEmpty(invoiceRequestModel.AdditionalDetail?.SplitLoadChainId))
                        {
                            SetTotalAllowanceToInvoiceModel(invoiceModel);
                        }
                        else
                        {
                            SetFTLPricingToInvoiceModel(invoiceModel);
                        }
                    }
                }
                // Set invoice type and waiting action if price is not available for drop date
                invoiceModel.InvoiceTypeId = CheckUpdatedPriceWaitingActionAndGetInvoiceType(pricingData, invoiceModel.InvoiceTypeId);
                invoiceModel.WaitingFor = pricingData.WaitingFor;
            }
            else
            {
                invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
            }
        }

        protected async Task CheckAndSetInvoiceExceptions(InvoiceCreateViewModel invoiceCreateViewModel, InvoiceModel invoiceModel)
        {
            if (invoiceCreateViewModel.IsFTL && !invoiceModel.IsVariousFobOrigin && invoiceModel.WaitingFor != WaitingAction.BolDetails)
            {
                var exceptionModel = invoiceCreateViewModel.ToExceptionRequestModel();
                exceptionModel.InvoiceNumber = invoiceModel.DisplayInvoiceNumber;
                invoiceModel.InvoiceExceptions = await CheckAndGetInvoiceExceptions(exceptionModel);
                if (invoiceModel.InvoiceExceptions.Any())
                {
                    invoiceModel.WaitingFor = WaitingAction.ExceptionApproval;
                    invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
                    invoiceModel.DisplayInvoiceNumber = invoiceModel.DisplayInvoiceNumber.FormattedInvoiceNumber(invoiceModel.InvoiceTypeId, invoiceModel.WaitingFor);
                    invoiceModel.DroppedGallons = invoiceCreateViewModel.ActualDropQuantity ?? 0;
                }
            }
        }

        protected List<InvoiceExceptionRequestModel> CheckAndSetInvoiceExceptions(List<DropAdditionalDetailsModel> dropModels, List<InvoiceModel> invoiceModels)
        {
            List<InvoiceExceptionRequestModel> response = new List<InvoiceExceptionRequestModel>();
            foreach (var invoiceModel in invoiceModels)
            {
                var dropModel = dropModels.FirstOrDefault(t => t.OrderId == invoiceModel.OrderId);
                if (dropModel != null)
                {
                    if (dropModel.IsFtl && !invoiceModel.IsVariousFobOrigin && invoiceModel.WaitingFor != WaitingAction.BolDetails)
                    {
                        var exceptionModel = dropModel.ToExceptionRequestModel(invoiceModel);
                        if (exceptionModel.UserId > 0)
                        {
                            var driverDetails = Context.DataContext.Users.Where(top => top.Id == exceptionModel.DriverId && top.IsActive && top.IsOnboardingComplete && top.IsEmailConfirmed).FirstOrDefault();
                            if (driverDetails != null)
                            {
                                exceptionModel.DriverName = driverDetails.FirstName + " " + driverDetails.LastName;
                                exceptionModel.DriverId = invoiceModel.CreatedBy;
                            }
                        }
                        exceptionModel.InvoiceNumber = invoiceModel.DisplayInvoiceNumber;
                        if (exceptionModel.DroppedQuantity < exceptionModel.BolQuantity)
                        {
                            exceptionModel.ExceptionTypeId = (int)ExceptionType.DeliveredQuantityVariance;
                            response.Add(exceptionModel);
                        }
                    }
                }
            }
            return response;
        }

        protected async Task CheckAndSetInvoiceExceptions(DropAdditionalDetailsModel dropModel, InvoiceModel invoiceModel)
        {
            if (dropModel.IsFtl && !invoiceModel.IsVariousFobOrigin && invoiceModel.WaitingFor != WaitingAction.BolDetails)
            {
                var exceptionModel = dropModel.ToExceptionRequestModel(invoiceModel);
                if (exceptionModel.UserId > 0)
                {
                    var driverDetails = Context.DataContext.Users.Where(top => top.Id == exceptionModel.DriverId && top.IsActive && top.IsOnboardingComplete && top.IsEmailConfirmed).FirstOrDefault();
                    if (driverDetails != null)
                    {
                        exceptionModel.DriverName = driverDetails.FirstName + " " + driverDetails.LastName;
                        exceptionModel.DriverId = invoiceModel.CreatedBy;
                    }
                }
                exceptionModel.InvoiceNumber = invoiceModel.DisplayInvoiceNumber;
                invoiceModel.InvoiceExceptions = await CheckAndGetInvoiceExceptions(exceptionModel);
                if (invoiceModel.InvoiceExceptions.Any())
                {
                    invoiceModel.WaitingFor = WaitingAction.ExceptionApproval;
                    invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
                    invoiceModel.DisplayInvoiceNumber = invoiceModel.DisplayInvoiceNumber.FormattedInvoiceNumber(invoiceModel.InvoiceTypeId, invoiceModel.WaitingFor);
                }
            }
        }

        protected async Task CheckAndSetDuplicateInvoiceException(InvoiceCreateViewModel invoiceCreateViewModel, InvoiceModel invoiceModel)
        {
            if (!invoiceModel.IsVariousFobOrigin && invoiceModel.WaitingFor != WaitingAction.BolDetails)
            {
                var exceptionModel = invoiceCreateViewModel.ToExceptionModel();
                exceptionModel.DroppedQuantity = invoiceCreateViewModel.IsFTL ? invoiceCreateViewModel.ActualDropQuantity ?? 0 : invoiceModel.DroppedGallons;
                exceptionModel.UserId = invoiceModel.CreatedBy;
                exceptionModel.InvoiceNumber = invoiceModel.DisplayInvoiceNumber;

                var invoiceExceptions = await CheckAndGetDuplicateInvoiceException(exceptionModel);
                invoiceModel.InvoiceExceptions.AddRange(invoiceExceptions);

                if (invoiceModel.InvoiceExceptions.Any())
                {
                    invoiceModel.WaitingFor = WaitingAction.ExceptionApproval;
                    invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
                    invoiceModel.DisplayInvoiceNumber = invoiceModel.DisplayInvoiceNumber.FormattedInvoiceNumber(invoiceModel.InvoiceTypeId, invoiceModel.WaitingFor);
                }
            }
        }

        protected async Task CheckAndSetDuplicateInvoiceException(DropAdditionalDetailsModel dropModel, InvoiceModel invoiceModel)
        {
            if (!invoiceModel.IsVariousFobOrigin && invoiceModel.WaitingFor != WaitingAction.BolDetails)
            {
                var exceptionModel = dropModel.ToExceptionModel(invoiceModel);
                exceptionModel.DroppedQuantity = invoiceModel.DroppedGallons;
                exceptionModel.UserId = invoiceModel.CreatedBy;
                exceptionModel.InvoiceNumber = invoiceModel.DisplayInvoiceNumber.FormattedInvoiceNumber(invoiceModel.InvoiceTypeId, WaitingAction.ExceptionApproval);

                var invoiceExceptions = await CheckAndGetDuplicateInvoiceException(exceptionModel);
                if (invoiceExceptions.Any())
                {
                    invoiceModel.WaitingFor = WaitingAction.ExceptionApproval;
                    invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
                    invoiceModel.DisplayInvoiceNumber = invoiceModel.DisplayInvoiceNumber.FormattedInvoiceNumber(invoiceModel.InvoiceTypeId, invoiceModel.WaitingFor);
                    invoiceModel.InvoiceExceptions.AddRange(invoiceExceptions);
                }
            }
        }

        public async Task<List<InvoiceExceptionViewModel>> CheckAndGetInvoiceExceptions(InvoiceExceptionRequestModel exceptionModel)
        {
            var response = new List<InvoiceExceptionViewModel>();
            if (exceptionModel.DroppedQuantity < exceptionModel.BolQuantity)
            {
                var exceptionDomain = new ExceptionDomain(this);
                var exceptionResult = await exceptionDomain.CheckExceptions(new List<InvoiceExceptionRequestModel> { exceptionModel });
                if (exceptionResult != null && exceptionResult.Exceptions != null && exceptionResult.IsExceptionsRaised)
                {
                    response.AddRange(exceptionResult.Exceptions.Select(t => t.ToInvoiceExceptionModel()));
                }
            }
            return response;
        }

        public async Task<List<InvoiceExceptionViewModel>> CheckAndGetDuplicateInvoiceException(InvoiceExceptionRequestModel exceptionModel)
        {
            var storedProcedureDomain = new StoredProcedureDomain(this);
            var model = exceptionModel.ToViewModel();
            var origionalInvoice = await storedProcedureDomain.CheckForDuplicateInvoiceAsync(model);

            var response = new List<InvoiceExceptionViewModel>();
            if (origionalInvoice != null)
            {
                exceptionModel.ExceptionTypeId = (int)ExceptionType.DuplicateInvoice;
                exceptionModel.OrigionalInvoice = origionalInvoice.ToViewModel();
                exceptionModel.ParameterJson = JsonConvert.SerializeObject(exceptionModel.OrigionalInvoice);
                var exceptionDomain = new ExceptionDomain(this);
                var exceptionResult = await exceptionDomain.CheckExceptions(new List<InvoiceExceptionRequestModel> { exceptionModel });
                if (exceptionResult != null && exceptionResult.Exceptions != null && exceptionResult.IsExceptionsRaised)
                {
                    response.AddRange(exceptionResult.Exceptions.Select(t => t.ToInvoiceExceptionModel()));
                }
            }
            return response;
        }

        protected async Task CheckAndSetInvoiceApiException(bool apiRequestAlreadyExistsByRefId, InvoiceModel invoiceModel, InvoiceDropViewModel drop, TPDInvoiceViewModel apiRequestModel, InvoiceViewModelNew invoiceViewModel, UserContext apiUserContext)
        {
            if (apiRequestAlreadyExistsByRefId)
            {
                var exceptionModel = invoiceModel.ToExceptionModel(drop, apiRequestModel);
                exceptionModel.UserId = invoiceModel.CreatedBy;
                exceptionModel.SupplierCompanyId = apiUserContext.CompanyId;
                exceptionModel.SupplierCompanyName = apiUserContext.CompanyName;
                exceptionModel.BuyerCompanyId = invoiceViewModel.Customer != null ? invoiceViewModel.Customer.CompanyId : 0;
                exceptionModel.BuyerCompanyName = invoiceViewModel.Customer != null ? invoiceViewModel.Customer.CompanyName : null;
                exceptionModel.UserName = apiUserContext.UserName;
                exceptionModel.UserId = apiUserContext.Id;
                exceptionModel.DriverId = apiRequestModel.DriverId;
                exceptionModel.DriverName = $"{ apiRequestModel.DriverFirstName } {apiRequestModel.DriverLastName}";
                exceptionModel.ExceptionTypeId = (int)ExceptionType.InvoiceApiException;

                var otherDetails = new { apiRequestModel.ExternalRefID, apiRequestModel.SupplierInvoiceNumber, drop.LoadingBadge, drop.CarrierOrder, drop.CarrierOrderId };
                exceptionModel.ParameterJson = JsonConvert.SerializeObject(otherDetails);

                var invoiceApiExceptions = await GenerateInvoiceApiException(exceptionModel);
                invoiceModel.InvoiceExceptions.AddRange(invoiceApiExceptions);
            }
        }

        public async Task<List<InvoiceExceptionViewModel>> GenerateInvoiceApiException(InvoiceExceptionRequestModel exceptionModel)
        {
            var response = new List<InvoiceExceptionViewModel>();

            var exceptionDomain = new ExceptionDomain(this);
            var exceptionResult = await exceptionDomain.CheckInvoiceApiExceptions(exceptionModel);
            if (exceptionResult != null && exceptionResult.Exceptions != null && exceptionResult.IsExceptionsRaised)
            {
                response.AddRange(exceptionResult.Exceptions.Select(t => t.ToInvoiceExceptionModel()));
            }

            return response;
        }

        private decimal AddMarkupToPricePerGallon(InvoiceCreateViewModel invoiceRequestModel, decimal pricePerGallon)
        {
            if (invoiceRequestModel.IsBuySellOrder)
            {
                if (invoiceRequestModel.IsBuyPrice)
                {
                    //buy price for supplier
                    pricePerGallon = pricePerGallon + invoiceRequestModel.BrokerMarkUp;
                }
                else
                {
                    //sell price for supplier
                    pricePerGallon = pricePerGallon + invoiceRequestModel.BrokerMarkUp + invoiceRequestModel.SupplierMarkUp;
                }
            }
            return pricePerGallon;
        }

        protected FuelPricingRequestViewModel GetFuelPricingRequestViewModel(InvoiceCreateViewModel invoiceCreateModel, WaitingAction waitingFor)
        {
            var fuelPricingModel = new FuelPricingRequestViewModel
            {
                FuelTypeId = invoiceCreateModel.FuelTypeId,
                TerminalId = invoiceCreateModel.TerminalId,
                CityGroupTerminalId = invoiceCreateModel.CityGroupTerminalId,
                PricingTypeId = invoiceCreateModel.PricingTypeId,
                DropEndDate = invoiceCreateModel.DropEndDate,
                Currency = invoiceCreateModel.Currency,
                RackAvgTypeId = invoiceCreateModel.RackAvgTypeId,
                SupplierCost = invoiceCreateModel.SupplierCost,
                PricePerGallon = invoiceCreateModel.PricePerGallon,
                WaitingFor = waitingFor
            };

            if (invoiceCreateModel.IsFTL && invoiceCreateModel.FuelRequestPricingDetail != null)
            {
                fuelPricingModel.FuelRequestPricingDetails = invoiceCreateModel.FuelRequestPricingDetail.Clone();
            }
            fuelPricingModel.FuelRequestPricingDetails.PricingCode.Id = invoiceCreateModel.FuelRequestPricingDetail.PricingCode.Id;
            fuelPricingModel.FuelRequestPricingDetails.PricingCode.Code = invoiceCreateModel.FuelRequestPricingDetail.PricingCode.Code;
            fuelPricingModel.FuelRequestPricingDetails.RequestPriceDetailId = invoiceCreateModel.FuelRequestPricingDetail.RequestPriceDetailId;
            return fuelPricingModel;
        }

        protected void SetDropLocation(ManualInvoiceViewModel invoiceInputViewModel, InvoiceModel invoiceModel)
        {
            if (invoiceInputViewModel.IsExistingDropLocation)
            {
                var dispatchLocation = new DispatchLocationViewModel();
                if (invoiceInputViewModel.ExistingDropLocationId == 0)
                {
                    var job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == invoiceInputViewModel.JobId);
                    dispatchLocation.Address = job.Address;
                    dispatchLocation.City = job.City;
                    dispatchLocation.CountryCode = job.MstCountry.Code;
                    dispatchLocation.CountyName = job.CountyName;
                    dispatchLocation.Latitude = job.Latitude;
                    dispatchLocation.LocationType = (int)LocationType.Drop;
                    dispatchLocation.Longitude = job.Longitude;
                    dispatchLocation.StateCode = job.MstState.Code;
                    dispatchLocation.StateId = job.StateId;
                    dispatchLocation.ZipCode = job.ZipCode;
                    dispatchLocation.SiteName = job.Name;
                }
                else
                {
                    var location = Context.DataContext.FuelDispatchLocations.FirstOrDefault(t => t.Id == invoiceInputViewModel.ExistingDropLocationId);
                    dispatchLocation.Address = location.Address;
                    dispatchLocation.City = location.City;
                    dispatchLocation.CountryCode = location.CountryCode;
                    dispatchLocation.CountyName = location.CountyName;
                    dispatchLocation.Latitude = location.Latitude;
                    dispatchLocation.LocationType = (int)LocationType.Drop;
                    dispatchLocation.Longitude = location.Longitude;
                    dispatchLocation.StateCode = location.StateCode;
                    dispatchLocation.StateId = location.StateId.Value;
                    dispatchLocation.ZipCode = location.ZipCode;
                    dispatchLocation.SiteName = location.SiteName;
                }
                dispatchLocation.OrderId = invoiceInputViewModel.OrderId;
                dispatchLocation.CreatedBy = invoiceInputViewModel.userId;
                dispatchLocation.CreatedDate = DateTimeOffset.Now;
                invoiceModel.FuelDropLocation = dispatchLocation;
            }
        }

        private static int CheckUpdatedPriceWaitingActionAndGetInvoiceType(FuelPricingResponseViewModel fuelPricing, int invoiceTypeId)
        {
            if (fuelPricing.WaitingFor == WaitingAction.UpdatedPrice)
            {
                invoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceTypeId);
            }
            return invoiceTypeId;
        }

        public async Task<FuelPricingResponseViewModel> GetFuelPriceByPricingTypeAsync(FuelPricingRequestViewModel viewModel, int productDisplayGroupId)
        {
            var response = new FuelPricingResponseViewModel() { WaitingFor = viewModel.WaitingFor };
            if (viewModel.WaitingFor != WaitingAction.BolDetails)
            {
                response = await GetFuelPriceAsync(viewModel);
                var currentPricingDate = response.PriceLastUpdatedDate;
                if (viewModel.IsTerminalPrice() && (currentPricingDate.Date < viewModel.DropEndDate.Date || response.TerminalPrice == 0))
                {
                    var holidayList = new MasterDomain(this).GetHolidayList(ApplicationConstants.PublicHolidayList);
                    if (!(viewModel.DropEndDate.IsWeekEnd() || holidayList.Contains(viewModel.DropEndDate.Date)) || response.TerminalPrice == 0)
                        response.WaitingFor = WaitingAction.UpdatedPrice;
                }
            }
            return response;
        }

        public async Task<FuelPricingRequestViewModel> GetFuelPricingRequestViewModel(int orderId, DateTimeOffset deliveryDate)
        {
            FuelPricingRequestViewModel response = null;
            try
            {
                var order = await Context.DataContext.Orders.Where(t => t.Id == orderId)
                            .Select(t => new
                            {
                                t.TerminalId,
                                t.CityGroupTerminalId,
                                t.FuelRequest.FuelTypeId,
                                t.FuelRequest.PricingTypeId,
                                t.FuelRequest.Currency,
                                t.IsFTL,
                                t.FuelRequest.FuelRequestPricingDetail,
                                t.FuelRequest.FuelRequestDetail,

                            }).FirstOrDefaultAsync();
                if (order != null)
                {
                    response = new FuelPricingRequestViewModel
                    {
                        FuelTypeId = order.FuelTypeId,
                        TerminalId = order.TerminalId,
                        CityGroupTerminalId = order.CityGroupTerminalId,
                        PricingTypeId = order.PricingTypeId,
                        DropEndDate = deliveryDate,
                        Currency = order.Currency,
                        WaitingFor = WaitingAction.Nothing,
                        FuelRequestPricingDetails = new FuelRequestPricingDetailsViewModel() { RequestPriceDetailId = order.FuelRequestPricingDetail.RequestPriceDetailId }
                    };
                    if (order.IsFTL && order.FuelRequestPricingDetail != null)
                    {
                        response.FuelRequestPricingDetails.FuelRequestId = order.FuelRequestPricingDetail.FuelRequestId;
                        response.FuelRequestPricingDetails.PricingQuantityIndicatorTypeId = order.FuelRequestDetail.PricingQuantityIndicatorTypeId;
                        response.FuelRequestPricingDetails.PricingCode = new PricingCodeDetailViewModel { Code = order.FuelRequestPricingDetail.PricingCode };
                        response.FuelRequestPricingDetails.TruckLoadTypeId = order.FuelRequestDetail.TruckLoadTypeId;
                        response.FuelRequestPricingDetails.StateDefaultQuantityIndicatorId = order.FuelRequestDetail.PricingQuantityIndicatorTypeId ?? 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async Task<FuelPricingResponseViewModel> GetFuelPriceAsync(FuelPricingRequestViewModel viewModel)
        {
            var response = new FuelPricingResponseViewModel() { WaitingFor = viewModel.WaitingFor };

            ExternalPricingDomain externalPricingDomain = new ExternalPricingDomain(this);
            var priceResponse = await externalPricingDomain.GetFuelPriceAsync(viewModel);
            if (priceResponse != null)
            {
                response = priceResponse;
                if (viewModel.IsFixedPrice() || viewModel.IsSupplierCostPrice())
                {
                    response.WaitingFor = viewModel.WaitingFor;
                }
            }

            return response;
        }

        protected void ProcessInvoiceFuelFeesAndSetCalculatedValues(InvoiceCreateViewModel invoiceCreateModel, InvoiceModel invoiceModel, DateTimeOffset dropEndDate, int assetCount, bool isFeeTypeBasedCalculationRequired = true)
        {
            if (invoiceCreateModel.IsThirdPartyHardwareUsed)
            {
                invoiceModel.FuelFees = invoiceCreateModel.ExternalBrokeredOrder.BrokeredOrderFee.ToInvoiceModelFuelFees();
            }
            else
            {
                invoiceModel.FuelFees = invoiceCreateModel.FuelDeliveryDetails.FuelFees.ToInvoiceModelFuelFees(dropEndDate);
            }
            invoiceModel.FuelFees.ForEach(t => { t.Currency = invoiceModel.Currency; t.UoM = invoiceModel.UoM; });
            invoiceModel.FuelFees.SelectMany(t => t.FeeByQuantities).ToList().ForEach(t =>
            {
                t.Currency = invoiceModel.Currency;
                t.UoM = invoiceModel.UoM;
            });

            if (isFeeTypeBasedCalculationRequired)
            {
                FuelFeesDomain fuelFeesDomain = new FuelFeesDomain(this);
                fuelFeesDomain.CalculateAndSetTotalFeeAndQuantityToFuelFees(invoiceModel, assetCount);
            }
            else
            {
                // negate for credit partial invoice
                invoiceModel.FuelFees.ForEach(t => { t.TotalFee = -1 * t.Fee; t.FeeSubTypeId = (int)FeeSubType.FlatFee; t.FeeSubQuantity = -1; });
            }

            invoiceModel.TotalFeeAmount = invoiceModel.FuelFees.Where(t => t.DiscountLineItemId == null && !t.IncludeInPPG).Sum(t => Math.Round(t.TotalFee ?? 0, 2));
            invoiceModel.TotalDiscountAmount = invoiceModel.FuelFees.Where(t => t.DiscountLineItemId != null).Sum(t => t.TotalFee ?? 0);
        }

        protected void RemoveDryRunFeeFromManualInvoiceModel(InvoiceCreateViewModel invoiceCreateModel)
        {
            if (invoiceCreateModel.FuelDeliveryDetails != null && invoiceCreateModel.FuelDeliveryDetails.FuelFees != null)
            {
                invoiceCreateModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.RemoveAll(t => !string.IsNullOrWhiteSpace(t.FeeTypeId) && t.FeeTypeId.Equals(((int)FeeType.DryRunFee).ToString()));
            }
        }

        protected void SetTotalAllowanceToInvoiceModel(InvoiceModel invoiceModel)
        {
            if (invoiceModel.AdditionalDetail != null)
            {
                var allowance = Math.Round(invoiceModel.AdditionalDetail.SupplierAllowance ?? 0, ApplicationConstants.InvoiceSuppplierAllowanceUnitPriceDecimalDisplay);
                invoiceModel.AdditionalDetail.TotalAllowance = Math.Round(invoiceModel.DroppedGallons * allowance, ApplicationConstants.InvoiceSuppplierAllowanceTotalDecimalDisplay);
                invoiceModel.BasicAmount = invoiceModel.BasicAmount - invoiceModel.AdditionalDetail.TotalAllowance ?? 0;
            }
        }

        protected async Task UpdateCommonDetailsForOtherSplitInvoices(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel, int originalInvoiceId, InvoiceEditResponseViewModel responseViewModel)
        {
            var invoices = Context.DataContext.Invoices.Where(t => t.Id == manualInvoiceModel.InvoiceId || t.Id == originalInvoiceId).Select(t => new { Invoice = t, InvoiceFtlDetail = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail).FirstOrDefault(), t.Order.FuelRequest.Job.TimeZoneName }).ToList();
            var updatedInvoice = invoices.FirstOrDefault(t => t.Invoice.Id == manualInvoiceModel.InvoiceId);
            var originalInvoice = invoices.FirstOrDefault(t => t.Invoice.Id == originalInvoiceId);
            if (originalInvoice != null && updatedInvoice != null)
            {
                bool isCommonDetailsChanged = IsCommonDetailsChangedForSplitInvoice(originalInvoice.Invoice, updatedInvoice.Invoice, originalInvoice.InvoiceFtlDetail, updatedInvoice.InvoiceFtlDetail);

                if (isCommonDetailsChanged)
                {
                    var otherSplitInvoices = GetAllSplitLoadInvoices(manualInvoiceModel.SplitLoadChainId, originalInvoiceId, manualInvoiceModel.InvoiceId);
                    foreach (var splitInvoice in otherSplitInvoices)
                    {
                        if (splitInvoice.Invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && splitInvoice.Invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp)
                        {
                            List<int> trackableScheduleIds = null;
                            if (splitInvoice.Invoice.TrackableScheduleId.HasValue)
                            {
                                trackableScheduleIds = new List<int>() { splitInvoice.Invoice.TrackableScheduleId.Value };
                            }
                            AddRebillInvoiceToQueueServiceAsync(splitInvoice.Invoice.Id, trackableScheduleIds, manualInvoiceModel.InvoiceId, userContext);
                        }
                        else
                        {
                            await UpdateCommonDetailsForSplitInvoice(splitInvoice.Invoice, updatedInvoice.Invoice, userContext, responseViewModel, updatedInvoice.TimeZoneName);
                            NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                            await newsfeedDomain.SetSplitDropTicketModifiedNewsfeed(userContext, splitInvoice.Invoice, responseViewModel);
                        }
                    }
                }
            }
        }

        private List<GetRelatedSplitInvoicesInputViewModel> GetAllSplitLoadInvoices(string splitLoadChainId, int originalInvoiceId, int updatedInvoiceId)
        {
            splitLoadChainId = splitLoadChainId.Split('C')[0];
            List<GetRelatedSplitInvoicesInputViewModel> splitInvoices = Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId.Contains(splitLoadChainId)
                                                                                                                    && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                                                                    .Select(t => new GetRelatedSplitInvoicesInputViewModel()
                                                                                                                    {
                                                                                                                        Invoice = t,
                                                                                                                        OriginalInvoiceId = t.InvoiceXAdditionalDetail.OriginalInvoiceId,
                                                                                                                        SplitLoadChainId = t.InvoiceXAdditionalDetail.SplitLoadChainId
                                                                                                                    }).ToList();
            var orginalInvoices = splitInvoices.Where(t => t.OriginalInvoiceId != null).Select(t => t.OriginalInvoiceId).ToList();
            splitInvoices = splitInvoices.Where(t => t.Invoice.Id != originalInvoiceId && t.Invoice.Id != updatedInvoiceId
                                                     && !orginalInvoices.Contains(t.Invoice.Id) && t.Invoice.InvoiceTypeId != (int)InvoiceType.CreditInvoice
                                                     && t.Invoice.InvoiceTypeId != (int)InvoiceType.PartialCredit).ToList();
            return splitInvoices;
        }

        private bool IsCommonDetailsChangedForSplitInvoice(Invoice originalInvoice, Invoice updatedInvoice, InvoiceFtlDetail originalInvoiceBol, InvoiceFtlDetail updatedInvoiceBol)
        {
            bool isCommonDetailsChanged = false;
            if (originalInvoiceBol != null && updatedInvoiceBol != null)
            {
                if (originalInvoice.PaymentTermId != updatedInvoice.PaymentTermId
                          || originalInvoice.NetDays != updatedInvoice.NetDays
                          || originalInvoice.TrackableScheduleId != updatedInvoice.TrackableScheduleId
                          || originalInvoice.DriverId != updatedInvoice.DriverId
                          || originalInvoiceBol.BolNumber != updatedInvoiceBol.BolNumber
                          || originalInvoiceBol.GrossQuantity != updatedInvoiceBol.GrossQuantity
                          || originalInvoiceBol.NetQuantity != updatedInvoiceBol.NetQuantity
                          || originalInvoiceBol.Carrier != updatedInvoiceBol.Carrier
                          || originalInvoiceBol.ImageId != updatedInvoiceBol.ImageId
                          || originalInvoiceBol.LiftDate != updatedInvoiceBol.LiftDate
                          || originalInvoiceBol.LiftTicketNumber != updatedInvoiceBol.LiftTicketNumber
                          || originalInvoiceBol.LiftQuantity != updatedInvoiceBol.LiftQuantity
                          || originalInvoiceBol.ImageId != updatedInvoiceBol.ImageId)
                {
                    isCommonDetailsChanged = true;
                }
            }
            return isCommonDetailsChanged;
        }

        private async Task UpdateCommonDetailsForSplitInvoice(Invoice splitDdt, Invoice rebilledInvoice, UserContext userContext, InvoiceEditResponseViewModel responseViewModel, string timeZone)
        {
            splitDdt.PaymentTermId = rebilledInvoice.PaymentTermId;
            splitDdt.NetDays = rebilledInvoice.NetDays;
            splitDdt.TrackableScheduleId = rebilledInvoice.TrackableScheduleId;
            splitDdt.DriverId = rebilledInvoice.DriverId;
            //splitDdt.InvoiceXBolDetails = rebilledInvoice.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail);
            splitDdt.UpdatedBy = userContext.Id;
            splitDdt.UpdatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZone);
            //splitDdt.InvoiceBolDetails.First().TerminalId = rebilledInvoice.InvoiceBolDetails.First().TerminalId;
            await Context.CommitAsync();
        }

        protected static string GetBrokeredChainId(string brokeredChainId, int createdBy)
        {
            if (string.IsNullOrWhiteSpace(brokeredChainId))
            {
                brokeredChainId = DateTimeOffset.Now.ToString("yyyyMMddHHmmssFFFFFF") + createdBy;
            }
            return brokeredChainId;
        }

        protected Dictionary<int, int> GetBrokerChainOrderListTillOriginalOrder(int endSupplierOrderId, Dictionary<int, int> orderList)
        {
            var thisOrder = Context.DataContext.Orders.Where(t => t.Id == endSupplierOrderId)
                            .Select(t => new
                            {
                                t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId,
                                t.FuelRequest,
                                t.FuelRequest.FuelRequestDetail.DeliveryTypeId
                            }).FirstOrDefault();

            if (thisOrder.FuelRequest.GetParentFuelRequest().FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && thisOrder.FuelRequest.FuelRequest1 != null && thisOrder.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
            {
                bool isSingleDeliveryClosedOrder = false;
                if (thisOrder.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery
                    && (thisOrder.StatusId == (int)OrderStatus.PartiallyClosed || thisOrder.StatusId == (int)OrderStatus.Closed))
                {
                    isSingleDeliveryClosedOrder = true;
                }
                var childRequest = thisOrder.FuelRequest.GetParentFuelRequest().FuelRequest1;
                var brokeredOrder = childRequest.Orders.LastOrDefault();
                brokeredOrder = (brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open || isSingleDeliveryClosedOrder) ? brokeredOrder : GetConnectingBuyerOrder(brokeredOrder);
                if (brokeredOrder != null && (brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open || isSingleDeliveryClosedOrder))
                {
                    orderList.Add(brokeredOrder.Id, brokeredOrder.AcceptedBy);
                    return GetBrokerChainOrderListTillOriginalOrder(brokeredOrder.Id, orderList);
                }
                else
                {
                    return orderList;
                }
            }
            else
            {
                return orderList;
            }
        }

        public List<BrokeredOrdersModel> GetBrokerOrderListTillOriginalOrder(int endSupplierOrderId, List<BrokeredOrdersModel> orderList, int i = 1)
        {
            i++;
            var thisOrder = Context.DataContext.Orders.Where(t => t.Id == endSupplierOrderId)
                            .Select(t => new
                            {
                                t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId,
                                t.FuelRequest,
                                t.FuelRequest.FuelRequestDetail.DeliveryTypeId
                            }).FirstOrDefault();

            if ((thisOrder.FuelRequest.GetParentFuelRequest().FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest || thisOrder.FuelRequest.GetParentFuelRequest().FuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest)
                && thisOrder.FuelRequest.FuelRequest1 != null
                && thisOrder.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
            {
                bool isSingleDeliveryClosedOrder = false;
                if (thisOrder.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery
                    && (thisOrder.StatusId == (int)OrderStatus.PartiallyClosed || thisOrder.StatusId == (int)OrderStatus.Closed))
                {
                    isSingleDeliveryClosedOrder = true;
                }
                var childRequest = thisOrder.FuelRequest.GetParentFuelRequest().FuelRequest1;
                var brokeredOrder = childRequest.Orders.LastOrDefault();
                brokeredOrder = (brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open || isSingleDeliveryClosedOrder) ? brokeredOrder : GetConnectingBuyerOrder(brokeredOrder);
                if (brokeredOrder != null && (brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open || isSingleDeliveryClosedOrder))
                {
                    orderList.Add(new BrokeredOrdersModel() { OrderId = brokeredOrder.Id, SupplierCompanyId = brokeredOrder.AcceptedCompanyId, BuyerCompanyId = brokeredOrder.BuyerCompanyId, SequenceFromEndSupplier = i });
                    return GetBrokerOrderListTillOriginalOrder(brokeredOrder.Id, orderList);
                }
                else
                {
                    return orderList;
                }
            }
            else
            {
                return orderList;
            }
        }

        private Order GetConnectingBuyerOrder(Order order)
        {
            if (order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
            {
                return order;
            }
            else
            {
                var parentRequest = order.FuelRequest.FuelRequest1;
                if (parentRequest != null && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
                {
                    var parentOrder = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.OrderByDescending(t => t.Id).FirstOrDefault();
                    return GetConnectingBuyerOrder(parentOrder);
                }
                else
                {
                    return null;
                }
            }
        }

        private void AddRebillInvoiceToQueueServiceAsync(int invoiceId, List<int> trackableScheduleId, int rebilledInvoiceId, UserContext userContext)
        {
            RebillInvoiceQueueServiceInputViewModel viewModel = new RebillInvoiceQueueServiceInputViewModel();
            viewModel.InvoiceId = invoiceId;
            viewModel.RebilledInvoiceId = rebilledInvoiceId;
            viewModel.UserId = userContext.Id;
            viewModel.TrackableScheduleIds = trackableScheduleId;
            viewModel.CompanyId = userContext.CompanyId;
            viewModel.UserName = userContext.Name;
            viewModel.CompanyName = userContext.CompanyName;
            string json = JsonConvert.SerializeObject(viewModel);

            var queueRequest = new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = QueueProcessType.RebillInvoiceCreation,
                JsonMessage = json
            };
            var queueDomain = new QueueMessageDomain();
            var queueId = queueDomain.EnqeueMessage(queueRequest);
        }

        protected void CheckRequiredImagesAndSetWaitingForImageAction(InvoiceModel invoiceModel)
        {
            var isBolImgProvided = false;
            var isDropImgProvided = false;
            var issiggImgProvided = false;

            if (invoiceModel.IsSignatureReq || invoiceModel.IsBOLImageReq || invoiceModel.IsDropImageReq)
            {
                if (invoiceModel.IsSignatureReq)
                {
                    if ((invoiceModel.Signature != null && invoiceModel.Signature?.Image != null && !string.IsNullOrWhiteSpace(invoiceModel.Signature.Image?.FilePath)) || invoiceModel.Signature?.Id > 0)
                        issiggImgProvided = true;
                }
                else
                    issiggImgProvided = true;

                if (invoiceModel.IsBOLImageReq)
                {
                    if ((invoiceModel.BolImage != null && !string.IsNullOrWhiteSpace(invoiceModel.BolImage?.FilePath)) || invoiceModel.BolImage?.Id > 0)
                        isBolImgProvided = true;
                }
                else
                    isBolImgProvided = true;

                if (invoiceModel.IsDropImageReq)
                {
                    if ((invoiceModel.Image != null && !string.IsNullOrWhiteSpace(invoiceModel.Image?.FilePath)) || invoiceModel.Image?.Id > 0)
                        isDropImgProvided = true;
                }
                else
                    isDropImgProvided = true;

                var canSetToNothing = (isBolImgProvided && issiggImgProvided && isDropImgProvided);

                if (canSetToNothing)
                {
                    SetWaitingActionToNothing(invoiceModel);
                    invoiceModel.IsInvoiceImagesAvailable = true;
                }
                else
                    SetWaitingActionToImages(invoiceModel);
            }
        }


        public bool IsWaitingForImage(InvoiceModel invoiceModel)
        {
            bool waitingForImages = false;
            if (invoiceModel.IsSignatureReq && !((invoiceModel.Signature != null && invoiceModel.Signature?.Image != null && !string.IsNullOrWhiteSpace(invoiceModel.Signature.Image?.FilePath)) || invoiceModel.Signature?.Id > 0))
            {
                waitingForImages = true;
            }
            else if (invoiceModel.IsDropImageReq && !((invoiceModel.Image != null && !string.IsNullOrWhiteSpace(invoiceModel.Image?.FilePath)) || invoiceModel.Image?.Id > 0))
            {
                waitingForImages = true;
            }
            else if (invoiceModel.IsBOLImageReq)
            {
                foreach (var bol in invoiceModel.BolDetails)
                {
                    if (!((bol.Image != null && !string.IsNullOrWhiteSpace(bol.Image?.FilePath)) || bol.Image?.Id > 0))
                    {
                        waitingForImages = true;
                        break;
                    }
                }
            }

            return waitingForImages;
        }

        private static void SetWaitingActionToImages(InvoiceModel invoiceModel)
        {
            invoiceModel.WaitingFor = WaitingAction.Images;
            invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
        }

        private static void SetWaitingActionToNothing(InvoiceModel invoiceModel)
        {
            invoiceModel.WaitingFor = WaitingAction.Nothing;
            invoiceModel.InvoiceTypeId = (int)InvoiceType.Manual;
        }

        public TaxResponseViewModel GetTaxesForPartialCreditInvoice(ManualInvoiceViewModel manualInvoiceModel)
        {
            TaxResponseViewModel taxResponse = new TaxResponseViewModel();


            var taxDetailsViewModel = new InvoiceTaxDetailsViewModel()
            {
                TranId = manualInvoiceModel.InvoiceId,
                ReturnCode = manualInvoiceModel.InvoiceId
            };

            taxDetailsViewModel.AvaTaxDetails = new List<TaxDetailsViewModel>();

            foreach (var item in manualInvoiceModel.TaxDetails.AvaTaxDetails)
            {
                //tax details from edit.
                taxDetailsViewModel.AvaTaxDetails.Add(new TaxDetailsViewModel()
                {
                    CalculationTypeInd = ApplicationConstants.CalculationType,
                    Currency = item.Currency,
                    ProductCategory = 1,
                    RateDescription = item.RateDescription,
                    RateSubtype = item.RateSubtype,
                    RateType = ApplicationConstants.ExternalTaxRateTypeTAX,
                    SalesTaxBaseAmount = item.SalesTaxBaseAmount,
                    TaxAmount = item.TaxAmount,
                    TaxExemptionInd = ApplicationConstants.TaxExemptionInd,
                    TaxRate = 0,
                    TaxType = item.TaxType,
                    TaxingLevel = item.TaxingLevel,
                    UnitOfMeasure = ApplicationConstants.UnitOfMeasure,
                    TradingTaxAmount = item.TradingTaxAmount,
                    TradingCurrency = item.TradingCurrency,
                    ExchangeRate = item.ExchangeRate,
                    RelatedLineItem = item.RelatedLineItem,
                    IsModified = false
                });

                taxDetailsViewModel.TotalTaxAmount += item.TradingTaxAmount;
            }


            taxResponse.TaxDetailsViewModel = taxDetailsViewModel;
            taxResponse.StatusCode = Status.Success;

            return taxResponse;
        }

        protected async Task SetEddtNewsfeeds(UserContext userContext, EddtToInvoiceCreatedNewsfeedModel newsfeedModel, string ddtNumber)
        {
            NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
            if (newsfeedModel.IsDigitalDropTicket)
            {
                if (userContext.IsSuperAdmin)
                    await newsfeedDomain.SetEddtToAutoDdtCreatedNewsfeed(userContext, newsfeedModel, ddtNumber);
                else
                    await newsfeedDomain.SetEddtToDdtCreatedNewsfeed(userContext, newsfeedModel, ddtNumber);

            }
            else
            {
                if (userContext.IsSuperAdmin)
                    await newsfeedDomain.SetEddtToAutoInvoiceCreatedNewsfeed(userContext, newsfeedModel, ddtNumber);
                else
                    await newsfeedDomain.SetEddtToInvoiceCreatedNewsfeed(userContext, newsfeedModel, ddtNumber);
            }
        }

        protected void CheckForWaitingForApproval(InvoiceModel invoiceModel, bool isApprovalWorkflowEnabled, int invoiceTypeId, int invoiceStatusId)
        {
            if (invoiceModel.WaitingFor == WaitingAction.Nothing)
            {
                invoiceModel.InvoiceTypeId = CheckApprovalWorkflowAndGetInvoiceType(isApprovalWorkflowEnabled, invoiceTypeId);
                invoiceModel.WaitingFor = GetApprovalWorkflowWaitingForAction(isApprovalWorkflowEnabled, invoiceStatusId);
            }
        }

        protected StatusViewModel AddCreditInvoiceToQueueServiceAsync(int invoiceId, List<int> trackableScheduleIds, int userId)
        {
            var response = new StatusViewModel();
            try
            {
                CreditInvoiceQueueServiceInputViewModel viewModel = new CreditInvoiceQueueServiceInputViewModel();
                viewModel.InvoiceId = invoiceId;
                viewModel.UserId = userId;
                viewModel.TrackableScheduleIds = trackableScheduleIds;
                string json = JsonConvert.SerializeObject(viewModel);

                var queueRequest = new QueueMessageViewModel()
                {
                    CreatedBy = userId,
                    QueueProcessType = QueueProcessType.CreditInvoiceCreation,
                    JsonMessage = json
                };
                var queueDomain = new QueueMessageDomain();
                var queueId = queueDomain.EnqeueMessage(queueRequest);
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errCreditInvoiceCreationFailed;
                LogManager.Logger.WriteException("InvoiceCommonDomain", "AddCreditInvoiceToQueueServiceAsync", ex.Message + " InvoiceId:" + invoiceId, ex);
            }

            return response;
        }

        public async Task<DropAddressViewModel> GetBulkplantAddress(int trackableScheduleId, int orderId)
        {
            DropAddressViewModel response = new DropAddressViewModel();
            var pickupAddresses = await Context.DataContext.FuelDispatchLocations.Where(t => t.TrackableScheduleId == trackableScheduleId && t.LocationType == (int)LocationType.PickUp && t.IsActive && t.OrderId == orderId).ToListAsync();
            if (pickupAddresses.Any())
            {
                var pickupAddress = pickupAddresses.FirstOrDefault(t => t.TerminalId == null);
                if (pickupAddress != null)
                    response = pickupAddress.ToPickUpLocation();
                else
                {
                    response.IsAddressAvailable = false;
                }
            }
            else
            {
                var pickupAddress = await Context.DataContext.FuelDispatchLocations.FirstOrDefaultAsync(t => t.LocationType == (int)LocationType.PickUp && t.IsActive && t.OrderId == orderId && t.DeliveryScheduleId == null && t.TrackableScheduleId == null && t.TerminalId == null);
                if (pickupAddress != null)
                    response = pickupAddress.ToPickUpLocation();
            }
            return response;
        }

        protected FuelFeesViewModel GetInvoiceFuelFees(ICollection<FuelFee> fuelFees,decimal droppedGallons)
        {
            FuelFeesViewModel response = new FuelFeesViewModel();
            try
            {
                response.FuelRequestFees = fuelFees.Where(t => t.FeeSubTypeId != (int)FeeSubType.NoFee && t.FeeTypeId != (int)FeeType.DryRunFee).OrderBy(t => t.FeeTypeId).ToList().ToFeesViewModel();
                if (response.FuelRequestFees != null && response.FuelRequestFees.Any())
                {
                    foreach (var fee in response.FuelRequestFees)
                    {
                        if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate && (fee.FeeSubQuantity != null && fee.FeeSubQuantity.Value > 0))
                        {
                            fee.TotalHours = GetHosingTimeInHours(fee.FeeSubQuantity.Value.ToString());
                        }
                        else if (fee.FeeSubTypeId == (int)FeeSubType.ByAssetCount && (fee.FeeSubQuantity != null && fee.FeeSubQuantity.Value > 0))
                        {
                            fee.TotalAssetQty = Convert.ToInt64(fee.FeeSubQuantity.Value);
                        }
                    }
                }

                response.DiscountLineItems = fuelFees.Where(t => t.DiscountLineItemId != null).OrderBy(t => t.FeeTypeId).ToList().ToDiscountFeesViewModel();
                response.FuelSurchargeFreightFee = fuelFees.ToSurchargeFreightFeesViewModel();
                response.FreightCostFee = fuelFees.ToFreightCostFeesViewModel();
                response.FuelSurchargeFreightFee.GallonsDelivered = droppedGallons;
                response.FuelSurchargeFreightFee.IsFreightCostApplicable = response.FreightCostFee.IsFreightCostApplicable; 
                if (response.FuelSurchargeFreightFee.FeeSubQuantity.HasValue && response.FuelSurchargeFreightFee.GallonsDelivered != 0)
                    response.FuelSurchargeFreightFee.SurchargePercentage = response.FuelSurchargeFreightFee.FeeSubQuantity.Value / response.FuelSurchargeFreightFee.GallonsDelivered;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCommonDomain", "GetInvoiceFuelFees", ex.Message, ex);
            }
            return response;
        }

        public string GetHosingTimeInHours(string timeInSeconds)
        {
            if (!string.IsNullOrWhiteSpace(timeInSeconds))
            {
                var totalSeconds = Convert.ToDouble(timeInSeconds);
                var hours = (int)(totalSeconds / 3600);
                var mins = (totalSeconds - (hours * 3600)) / 60;
                return hours != 0 ? string.Format(Resource.constTimeFormat, hours, string.Format(Resource.constFormatDecimal2, mins))
                                                                : string.Format(Resource.constMinuteFormat, string.Format(Resource.constFormatDecimal2, mins));
            }
            return string.Empty;
        }

        protected Dictionary<int, DateTimeOffset> GetScheduleDatesForBrokerChain(List<int> trackableScheduleIds)
        {
            Dictionary<int, DateTimeOffset> trackableSchedules = new Dictionary<int, DateTimeOffset>();
            var schedules = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => trackableScheduleIds.Contains(t.Id)).Select(t => new { t.DeliveryScheduleId, t.Date }).ToList();
            schedules.ForEach(t => trackableSchedules.Add(t.DeliveryScheduleId, t.Date));
            return trackableSchedules;
        }

        protected Dictionary<int, int> GetTrackableSchedulesForBrokers(List<int> brokeredOrderIds, Dictionary<int, DateTimeOffset> deliverySchedules)
        {
            var trackableScheduleIds = new Dictionary<int, int>();
            var scheduleIds = Context.DataContext.DeliveryScheduleXTrackableSchedules
                                .Where(t => t.OrderId != null && brokeredOrderIds.Contains(t.OrderId.Value))
                                .Select(t => new { t.OrderId, t.Id, t.DeliveryScheduleId, t.Date }).Distinct().ToList();
            scheduleIds = scheduleIds.Where(t => deliverySchedules.Any(t1 => t1.Key == t.DeliveryScheduleId && t1.Value == t.Date)).ToList();
            scheduleIds.ForEach(t => trackableScheduleIds.Add(t.OrderId.Value, t.Id));

            return trackableScheduleIds;
        }

        protected InvoiceModel GetInvoiceModelFromOriginalInvoice(Invoice originalInvoice)
        {
            if (originalInvoice != null)
            {
                var invoiceModel = new InvoiceModel
                {
                    Id = originalInvoice.Id,
                    InvoiceHeaderId = originalInvoice.InvoiceHeaderId,
                    OrderId = originalInvoice.OrderId,
                    InvoiceNumberId = originalInvoice.InvoiceHeader.InvoiceNumberId,
                    InvoiceVersionStatusId = originalInvoice.InvoiceVersionStatusId,
                    Version = originalInvoice.Version,
                    InvoiceTypeId = originalInvoice.InvoiceTypeId,
                    DroppedGallons = originalInvoice.DroppedGallons,
                    PricePerGallon = originalInvoice.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail.PricePerGallon).FirstOrDefault(),
                    PricingTypeId = originalInvoice.Order != null ? originalInvoice.Order.FuelRequest.PricingTypeId : (int)PricingType.PricePerGallon,
                    DropStartDate = originalInvoice.DropStartDate,
                    DropEndDate = originalInvoice.DropEndDate,
                    PoNumber = originalInvoice.PoNumber,
                    StateTax = originalInvoice.StateTax,
                    FedTax = originalInvoice.FedTax,
                    SalesTax = originalInvoice.SalesTax,
                    BasicAmount = originalInvoice.BasicAmount,
                    IsOverWaterDelivery = originalInvoice.IsOverWaterDelivery,
                    IsWetHosingDelivery = originalInvoice.IsWetHosingDelivery,
                    PaymentTermId = originalInvoice.PaymentTermId,
                    NetDays = originalInvoice.NetDays,
                    PaymentDueDate = originalInvoice.PaymentDueDate,
                    PaymentDate = originalInvoice.PaymentDate,
                    TerminalId = originalInvoice.InvoiceXBolDetails.Where(t => t.InvoiceFtlDetail.TerminalId.HasValue).Select(t => t.InvoiceFtlDetail.TerminalId).FirstOrDefault(),
                    ParentId = originalInvoice.ParentId,
                    CreatedBy = originalInvoice.CreatedBy,
                    CreatedDate = originalInvoice.CreatedDate,
                    IsActive = originalInvoice.IsActive,
                    UpdatedBy = originalInvoice.UpdatedBy,
                    UpdatedDate = originalInvoice.UpdatedDate,
                    TotalTaxAmount = originalInvoice.TotalTaxAmount,
                    TransactionId = originalInvoice.TransactionId,
                    RackPrice = originalInvoice.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail.RackPrice).FirstOrDefault(),
                    DriverId = originalInvoice.DriverId,
                    TraceId = originalInvoice.TraceId,
                    SignatureId = originalInvoice.SignatureId,
                    FilePath = originalInvoice.FilePath,
                    WaitingFor = (WaitingAction)originalInvoice.WaitingFor,
                    CityGroupTerminalId = originalInvoice.InvoiceXBolDetails.Where(t => t.InvoiceFtlDetail.CityGroupTerminalId.HasValue).Select(t => t.InvoiceFtlDetail.CityGroupTerminalId).FirstOrDefault(),
                    SupplierPreferredInvoiceTypeId = originalInvoice.SupplierPreferredInvoiceTypeId,
                    IsBuyPriceInvoice = originalInvoice.IsBuyPriceInvoice,
                    TotalFeeAmount = originalInvoice.TotalFeeAmount,
                    BrokeredChainId = originalInvoice.BrokeredChainId,
                    BaseDroppedQuntity = originalInvoice.BaseDroppedQuntity,
                    BasePrice = originalInvoice.BasePrice,
                    BaseStateTax = originalInvoice.BaseStateTax,
                    BaseFedTax = originalInvoice.BaseFedTax,
                    BaseSalesTax = originalInvoice.BaseStateTax,
                    BaseBasicAmount = originalInvoice.BaseBasicAmount,
                    BaseTotalTaxAmount = originalInvoice.BaseTotalTaxAmount,
                    BaseRackPrice = originalInvoice.BaseRackPrice,
                    BaseTotalFeeAmount = originalInvoice.BaseTotalFeeAmount,
                    Currency = originalInvoice.Currency,
                    ExchangeRate = originalInvoice.ExchangeRate,
                    UoM = originalInvoice.UoM,
                    TrackableScheduleId = originalInvoice.TrackableScheduleId,
                    DisplayInvoiceNumber = originalInvoice.DisplayInvoiceNumber,
                    ReferenceId = originalInvoice.ReferenceId,
                    QbInvoiceNumber = originalInvoice.QbInvoiceNumber,
                    TotalDiscountAmount = originalInvoice.TotalDiscountAmount,
                    DDTConversionReason = originalInvoice.DDTConversionReason,
                    IsBOLImageReq = originalInvoice.IsBolImageReq,
                    IsSignatureReq = originalInvoice.IsSignatureReq,
                    IsDropImageReq = originalInvoice.IsDropImageReq,
                    IsVariousFobOrigin = originalInvoice.Order != null && originalInvoice.Order.FuelRequest.FreightOnBoardTypeId == (int)FreightOnBoardTypes.Terminal
                                                && originalInvoice.Order.FuelRequest.Job.LocationType == JobLocationTypes.Various,
                    TaxQuantityIndicatorTypeId = originalInvoice.Order.FuelRequest.Job.MstState.QuantityIndicatorTypeId,
                    QuantityIndicatorTypeId = originalInvoice.QuantityIndicatorTypeId,
                    GroupParentDrId = originalInvoice.GroupParentDrId,
                    ConvertedQuantity = originalInvoice.ConvertedQuantity,
                    ConvertedPricing = originalInvoice.ConvertedPricing,
                    Gravity = originalInvoice.Gravity,
                    ConversionFactor = originalInvoice.ConversionFactor,
                    PDIInvoiceNo = originalInvoice.PDIInvoiceNumber,
                    IsMarineLocation = originalInvoice.Order != null ? originalInvoice.Order.FuelRequest.Job.IsMarine : false,
                    JobCountryId = originalInvoice.Order != null ? originalInvoice.Order.FuelRequest.Job.CountryId : 0,
                    IsPdieTaxRequired = originalInvoice.Order.OrderAdditionalDetail != null && originalInvoice.Order.OrderAdditionalDetail.IsPDITaxRequired
                };

                if (originalInvoice.InvoiceXSpecialInstructions.Any())
                    invoiceModel.SpecialInstructions = originalInvoice.InvoiceXSpecialInstructions.ToList().Select(t => t.ToViewModel()).ToList();
                if (originalInvoice.InvoiceXAdditionalDetail != null)
                    invoiceModel.AdditionalDetail = originalInvoice.InvoiceXAdditionalDetail.ToViewModel();

                if (originalInvoice.BDRDetail != null)
                {
                    var bdrNumber = originalInvoice.InvoiceHeader.Invoices.OrderBy(t => t.OrderId).Select(t => t.OrderId).FirstOrDefault();
                    invoiceModel.BDRDetails = originalInvoice.BDRDetail.ToViewModel();
                    invoiceModel.BDRDetails.BDRNumber = bdrNumber.HasValue ? bdrNumber.ToString() : "";
                }

                if (originalInvoice.InvoiceXInvoiceStatusDetails.Any(t => t.IsActive))
                {
                    var invoiceStatus = originalInvoice.InvoiceXInvoiceStatusDetails.OrderByDescending(t => t.Id).FirstOrDefault(t => t.IsActive);
                    if (invoiceStatus != null)
                        invoiceModel.StatusId = invoiceStatus.StatusId;
                    else
                        invoiceModel.StatusId = (int)InvoiceStatus.Received;
                }
                if (originalInvoice.InvoiceExceptions.Any(t => t.IsActive))
                    invoiceModel.InvoiceExceptions = originalInvoice.InvoiceExceptions.Where(t => t.IsActive).ToList().Select(t => t.ToViewModel()).ToList();

                //missing things from mapper
                invoiceModel.CreationMethod = invoiceModel.AdditionalDetail.CreationMethod;

                if (originalInvoice.ImageId.HasValue)
                    invoiceModel.Image = new ImageViewModel() { Id = originalInvoice.ImageId.Value };

                if (originalInvoice.SignatureId.HasValue)
                {
                    invoiceModel.Signature = new CustomerSignatureViewModel() { Id = originalInvoice.SignatureId.Value };
                    invoiceModel.SignatureId = originalInvoice.SignatureId.Value;
                }

                invoiceModel.AssetDrops = originalInvoice.AssetDrops.ToList().Select(t => t.ToViewModel()).ToList();

                FuelFeesViewModel tofeesviewmodel = GetInvoiceFuelFees(originalInvoice.FuelRequestFees, originalInvoice.DroppedGallons);

                if (originalInvoice.InvoiceXAdditionalDetail.FreightPricingMethod == FreightPricingMethod.Auto)
                {
                    var lstAccessorialFees = originalInvoice.InvoiceXAccessorialFees.ToList();
                    foreach (var item in lstAccessorialFees)
                    {
                        invoiceModel.AccessorialFeeDetails.Add(item.ToViewModel());
                    }
                }

                if (originalInvoice.InvoiceXAdditionalDetail.IsFreightCostApplicable || originalInvoice.InvoiceXAdditionalDetail.IsSurchargeApplicable)
                {
                    InvoiceDomain invoiceDomain = new InvoiceDomain(this);
                    invoiceModel.SurchargeFreightFeeViewModel = new FuelSurchargeFreightFeeViewModel();
                    invoiceDomain.SetSurchargeDetails(originalInvoice.InvoiceXAdditionalDetail, invoiceModel.SurchargeFreightFeeViewModel, originalInvoice.Order.FuelRequest.MstProduct.ProductTypeId, originalInvoice.DroppedGallons);
                }
                invoiceModel.FuelFees = tofeesviewmodel.ToInvoiceModelFuelFees(originalInvoice.DropEndDate);

                invoiceModel.Discounts = originalInvoice.Discounts.ToList().Select(t => t.ToViewModel()).ToList();

                if (originalInvoice.TaxDetails.Any())
                {
                    invoiceModel.TaxDetails = originalInvoice.TaxDetails.ToList().ToViewModel();
                }
                //else part will be in use only for conversion case - Waiting for updated price
                else if (originalInvoice.Order.FuelRequest.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
                {
                    List<TaxViewModel> applicableTaxes = originalInvoice.Order.OrderTaxDetails.Where(x => x.IsActive).ToTaxViewModel();
                    var taxDetailsViewModel = GetTaxDetailsFromInputs(applicableTaxes, originalInvoice.Currency, originalInvoice.Id, originalInvoice.BasicAmount, originalInvoice.DroppedGallons);
                    invoiceModel.TaxDetails = taxDetailsViewModel;
                }

                if (originalInvoice.InvoiceXBolDetails.Any())
                {
                    foreach (var item in originalInvoice.InvoiceXBolDetails)
                    {
                        invoiceModel.BolDetails.Add(item.InvoiceFtlDetail.ToViewModel());
                    }
                }

                if (originalInvoice.InvoiceXAdditionalDetail != null && originalInvoice.InvoiceXAdditionalDetail.AdditionalImageId.HasValue)
                {
                    invoiceModel.AdditionalImage = new ImageViewModel
                    {
                        Id = originalInvoice.InvoiceXAdditionalDetail.AdditionalImageId.Value
                    };
                }
                if (originalInvoice.InvoiceXAdditionalDetail != null && originalInvoice.InvoiceXAdditionalDetail.TaxAffidavitImageId.HasValue)
                {
                    invoiceModel.TaxAffidavitImage = new ImageViewModel
                    {
                        Id = originalInvoice.InvoiceXAdditionalDetail.TaxAffidavitImageId.Value
                    };
                }
                if (originalInvoice.InvoiceXAdditionalDetail != null && originalInvoice.InvoiceXAdditionalDetail.BDNImageId.HasValue)
                {
                    invoiceModel.BDNImage = new ImageViewModel
                    {
                        Id = originalInvoice.InvoiceXAdditionalDetail.BDNImageId.Value
                    };
                }
                if (originalInvoice.InvoiceXAdditionalDetail != null && originalInvoice.InvoiceXAdditionalDetail.CoastGuardInspectionImageId.HasValue)
                {
                    invoiceModel.CoastGuardInspectionImage = new ImageViewModel
                    {
                        Id = originalInvoice.InvoiceXAdditionalDetail.CoastGuardInspectionImageId.Value
                    };
                }
                if (originalInvoice.InvoiceXAdditionalDetail != null && originalInvoice.InvoiceXAdditionalDetail.InspectionRequestVoucherImageId.HasValue)
                {
                    invoiceModel.InspectionRequestVoucherImage = new ImageViewModel
                    {
                        Id = originalInvoice.InvoiceXAdditionalDetail.InspectionRequestVoucherImageId.Value
                    };
                }
                if (originalInvoice.InvoiceDispatchLocation.Any(t => t.LocationType == (int)LocationType.PickUp))
                {
                    var pickupAddress = originalInvoice.InvoiceDispatchLocation.FirstOrDefault(t => t.LocationType == (int)LocationType.PickUp);
                    if (pickupAddress != null && pickupAddress.StateCode != null)
                    {
                        invoiceModel.FuelPickLocation = pickupAddress.ToDispatchLocation();
                    }
                }

                if (originalInvoice.InvoiceDispatchLocation.Any(t => t.LocationType == (int)LocationType.Drop))
                {
                    var dropAddress = originalInvoice.InvoiceDispatchLocation.FirstOrDefault(t => t.LocationType == (int)LocationType.Drop);
                    if (dropAddress != null && dropAddress.StateCode != null)
                    {
                        invoiceModel.FuelDropLocation = dropAddress.ToDispatchLocation();
                    }
                }
                else
                {
                    invoiceModel.FuelDropLocation = originalInvoice.Order.FuelRequest.Job.ToDispatchViewModel();
                    invoiceModel.FuelDropLocation.OrderId = originalInvoice.OrderId.Value;
                    invoiceModel.FuelDropLocation.CreatedBy = originalInvoice.CreatedBy;
                    invoiceModel.FuelDropLocation.CreatedDate = originalInvoice.CreatedDate;
                    invoiceModel.FuelDropLocation.PickupLocationType = PickupLocationType.None;
                }

                invoiceModel.AssetDrops = originalInvoice.AssetDrops.ToList().Select(t => t.ToViewModel()).ToList();

                return invoiceModel;
            }
            return null;
        }

        public List<FuelPriceRequestModel> SetPriceRequestDetails(List<BolDetailViewModel> bolDetails, int requestPriceDetailId, InvoiceModel invoice)
        {
            List<FuelPriceRequestModel> priceRequestModels = new List<FuelPriceRequestModel>();

            foreach (var bol in bolDetails)
            {
                FuelPriceRequestModel priceRequestModel = new FuelPriceRequestModel()
                {
                    TerminalId = bol.TerminalId,
                    ProductId = bol.FuelTypeId,
                    CityGroupTerminalId = bol.CityGroupTerminalId,
                    PriceDate = invoice.DropEndDate.Date,
                    SupplierCost = invoice.CurrentCost,
                    RequestPriceDetailId = requestPriceDetailId,
                    Guid = bol.Guid,
                    WaitingFor = invoice.WaitingFor,
                    DroppedQuantity = invoice.DroppedGallons,
                    //if true then Tier pricing Terminal will be used to get price
                    CanForceTerminalForTierPricing = string.IsNullOrWhiteSpace(bol.BolNumber) && string.IsNullOrWhiteSpace(bol.LiftTicketNumber),
                    UoM = invoice.UoM,
                    PricePerGallonToOverride = invoice.IsMarineLocation && invoice.UserPriceToOverride.HasValue && invoice.UserPriceToOverride.Value > 0 
                                                    ? invoice.UserPriceToOverride 
                                                    : null //edit price is only for Marine
                };
                // In case of MFN for fixed pricing set converted qty in UOM for Barrel/MT
                if ((invoice.PricingTypeId == (int)PricingType.PricePerGallon || invoice.PricingTypeId == (int)PricingType.Suppliercost) && invoice.IsMarineLocation &&
                    invoice.ConvertedQuantity != null && (invoice.UoM == UoM.MetricTons || invoice.UoM == UoM.Barrels))
                {
                    priceRequestModel.DroppedQuantity = invoice.ConvertedQuantity.Value;
                }
                if (bol.TierPricingForBol.FirstOrDefault() != null)
                {
                    priceRequestModel.TierMaxQuantity = bol.TierPricingForBol.FirstOrDefault().TierMaxQuantity;
                    priceRequestModel.TierMinQuantity = bol.TierPricingForBol.FirstOrDefault().TierMinQuantity;
                }
                priceRequestModels.Add(priceRequestModel);
            }
            return priceRequestModels;
        }

        public async Task GetPriceDetails(List<FuelPriceRequestModel> priceRequestModels, List<InvoiceModel> invoices, bool isIncludePricingForExtObj = false)
        {
            ExternalPricingDomain externalPricingDomain = new ExternalPricingDomain(this);
            var invoiceDomain = new InvoiceDomain(externalPricingDomain);
            var response = await externalPricingDomain.GetFuelPriceAsync(priceRequestModels);

            List<InvoiceModel> additionalModelsForTier = null;
            List<BolDetailViewModel> bolsToRemove = null;

            try
            {
                foreach (var invoice in invoices)
                {
                    decimal ppg = 0;
                    bool isFixedOrSupplierCostPricing = false;

                    foreach (var bol in invoice.BolDetails)
                    {
                        bool isInvoiceWithTierPrice = false;
                        var pricingDataList = response.Where(t => t.Guid == bol.Guid).ToList();
                        if (pricingDataList.Count < 2)
                        {
                            var pricingData = response.FirstOrDefault(t => t.Guid == bol.Guid);
                            if (pricingData != null)
                            {
                                ppg = SetPricingDetailsFromPriceToBol(invoice, bol, pricingData);
                                if (invoice.IsMarineLocation && (pricingData.PricingTypeId == (int)PricingType.PricePerGallon || pricingData.PricingTypeId == (int)PricingType.Suppliercost))
                                {
                                    isFixedOrSupplierCostPricing = true;
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                //if its tier pricing then only this block will execute
                                pricingDataList = pricingDataList.OrderBy(t => t.TierPricingDetails.MinQuantity).ToList();
                                var invoiceDroppedGallon = invoice.DroppedGallons;

                                foreach (var pricingData in pricingDataList)
                                {
                                    isInvoiceWithTierPrice = true;
                                    var newInvoiceModelWithTier = invoice.CopyObject(invoice);
                                    newInvoiceModelWithTier.BolDetails.Clear();

                                    var bolForTier = bol.CopyObject(bol);
                                    newInvoiceModelWithTier.BolDetails.Add(bolForTier);
                                    newInvoiceModelWithTier.DroppedGallons = invoiceDroppedGallon;

                                    if (pricingData.TierPricingDetails.MaxQuantity.HasValue && pricingData.TierPricingDetails.MaxQuantity.Value <= 0)
                                        newInvoiceModelWithTier.DroppedGallons = invoiceDroppedGallon;
                                    else
                                    {
                                        var tierQuantity = pricingData.TierPricingDetails.MaxQuantity.Value - pricingData.TierPricingDetails.MinQuantity.Value;
                                        if ((invoiceDroppedGallon - tierQuantity) <= 0)
                                            newInvoiceModelWithTier.DroppedGallons = invoiceDroppedGallon;
                                        else
                                        {
                                            newInvoiceModelWithTier.DroppedGallons = tierQuantity;
                                            invoiceDroppedGallon -= tierQuantity;
                                        }
                                    }

                                    var tierPpg = SetPricingDetailsFromPriceToBol(newInvoiceModelWithTier, bolForTier, pricingData);
                                    if (additionalModelsForTier == null) additionalModelsForTier = new List<InvoiceModel>();
                                    if (additionalModelsForTier.Any())
                                    {
                                        newInvoiceModelWithTier.AssetDrops.Clear();
                                        newInvoiceModelWithTier.AdditionalDetail.TotalAllowance = 0;
                                    }
                                    SetInvoiceBasicAmount(newInvoiceModelWithTier, tierPpg);

                                    // MFN - update fixed pricing for UoM = MT or Barrel to convert pricing into per gallon/litre
                                    if (invoice.IsMarineLocation)
                                    {
                                        if (pricingData.PricingTypeId == (int)PricingType.PricePerGallon || pricingData.PricingTypeId == (int)PricingType.Suppliercost)
                                        {
                                            var modelForConversion = new MFNConversionRequestViewModel() { DroppedGallons = tierPpg, JobCountryId = invoice.JobCountryId, UoM = invoice.UoM, UserProvidedConversionFactor = invoice.ConversionFactor };
                                            var gravity = invoice.Gravity.HasValue && invoice.Gravity > 0 ? invoice.Gravity.Value : 0;
                                            modelForConversion.ConversionFactor = gravity;
                                            var conversionResponse = await invoiceDomain.ValidateGravityAndConvertForMFN(modelForConversion);

                                            // update the pricing to per gallon/litre for avalara
                                            newInvoiceModelWithTier.PricePerGallon = conversionResponse.ConvertedQty;
                                            newInvoiceModelWithTier.ConvertedPricing = conversionResponse.ConvertedQty;
                                        }
                                        else if (pricingData.PricingTypeId == (int)PricingType.RackAverage || pricingData.PricingTypeId == (int)PricingType.RackHigh || pricingData.PricingTypeId == (int)PricingType.RackLow)
                                        {
                                            // for market based pricing, update only converted pricing value
                                            var modelForConversion = new MFNConversionRequestViewModel() { DroppedGallons = ppg, JobCountryId = invoice.JobCountryId, UoM = invoice.UoM, UserProvidedConversionFactor = invoice.ConversionFactor };
                                            var gravity = invoice.Gravity.HasValue && invoice.Gravity > 0 ? invoice.Gravity.Value : 0;
                                            modelForConversion.ConversionFactor = gravity;
                                            var conversionResponse = await invoiceDomain.ValidateAndConvertPricingForMarketBasedMFN(modelForConversion);
                                            invoice.ConvertedPricing = conversionResponse.ConvertedQty;
                                        }
                                    }

                                    additionalModelsForTier.Add(newInvoiceModelWithTier);

                                    if (bolsToRemove == null) bolsToRemove = new List<BolDetailViewModel>();
                                    bolsToRemove.Add(bol);
                                }
                            }
                            catch (Exception ex)
                            {
                                LogManager.Logger.WriteException("InvoiceCommonDomain", "GetPriceDetails", ex.Message, ex);
                                throw;
                            }
                        }

                        if (isInvoiceWithTierPrice)
                            break;
                    }
                    SetInvoiceBasicAmount(invoice, ppg);

                    // MFN - update fixed pricing for UoM = MT or Barrel to convert pricing into per gallon/litre
                    if (invoice.IsMarineLocation)
                    {
                        if (invoice.UoM == UoM.MetricTons || invoice.UoM == UoM.Barrels)
                        {
                            if (isFixedOrSupplierCostPricing)
                            {
                                var modelForConversion = new MFNConversionRequestViewModel() { DroppedGallons = ppg, JobCountryId = invoice.JobCountryId, UoM = invoice.UoM, UserProvidedConversionFactor = invoice.ConversionFactor };
                                var gravity = invoice.Gravity.HasValue && invoice.Gravity > 0 ? invoice.Gravity.Value : 0;
                                modelForConversion.ConversionFactor = gravity;
                                var conversionResponse = await invoiceDomain.ValidateGravityAndConvertForMFN(modelForConversion);
                                // update the pricing to per gallon/litre for avalara
                                invoice.PricePerGallon = conversionResponse.ConvertedQty;
                                invoice.ConvertedPricing = conversionResponse.ConvertedQty;
                            }
                            else // for market based pricing, update only convereted pricing value
                            {
                                var modelForConversion = new MFNConversionRequestViewModel() { DroppedGallons = ppg, JobCountryId = invoice.JobCountryId, UoM = invoice.UoM, UserProvidedConversionFactor = invoice.ConversionFactor };
                                var gravity = invoice.Gravity.HasValue && invoice.Gravity > 0 ? invoice.Gravity.Value : 0;
                                modelForConversion.ConversionFactor = gravity;
                                var conversionResponse = await invoiceDomain.ValidateAndConvertPricingForMarketBasedMFN(modelForConversion);
                                invoice.ConvertedPricing = conversionResponse.ConvertedQty; // represnts converted pricing
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCommonDomain", "GetPriceDetails", ex.Message, ex);
                throw;
            }

            if (bolsToRemove != null)
            {
                foreach (var invoice in invoices)
                {
                    foreach (var bol in bolsToRemove)
                    {
                        invoice.BolDetails.Remove(bol);
                    }
                }
            }

            invoices.RemoveAll(t => !t.BolDetails.Any());

            if (additionalModelsForTier != null)
            {
                invoices.AddRange(additionalModelsForTier);
            }

            if (response.Any(t => t.WaitingFor == WaitingAction.UpdatedPrice))
            {
                invoices.ForEach(t => { t.InvoiceTypeId = GetInvoiceCreationTypeToDdt(t.InvoiceTypeId); t.WaitingFor = WaitingAction.UpdatedPrice; });
                return;
            }
            else
            {
                if (!isIncludePricingForExtObj)
                    invoices.Where(t => t.WaitingFor == WaitingAction.UpdatedPrice).ToList().ForEach(t => t.WaitingFor = WaitingAction.Nothing);
            }
        }

        private static void SetInvoiceBasicAmount(InvoiceModel newInvoiceModelWithTier, decimal tierPpg)
        {
            var droppedQty = newInvoiceModelWithTier.DroppedGallons;
            if ((newInvoiceModelWithTier.PricingTypeId == (int)PricingType.PricePerGallon || newInvoiceModelWithTier.PricingTypeId == (int)PricingType.Suppliercost) && newInvoiceModelWithTier.IsMarineLocation &&
                    newInvoiceModelWithTier.ConvertedQuantity != null && (newInvoiceModelWithTier.UoM == UoM.MetricTons || newInvoiceModelWithTier.UoM == UoM.Barrels))
            {
                droppedQty = Math.Round(newInvoiceModelWithTier.ConvertedQuantity.Value, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);
            }
            newInvoiceModelWithTier.BasicAmount = (newInvoiceModelWithTier.IsMarineLocation ? Math.Round(tierPpg * droppedQty, ApplicationConstants.MarineInvoiceBasicAmountDecimalDisplay) : Math.Round(tierPpg * droppedQty, ApplicationConstants.InvoiceBasicAmountDecimalDisplay)) - (newInvoiceModelWithTier.AdditionalDetail.TotalAllowance ?? 0);
        }

        private static decimal SetPricingDetailsFromPriceToBol(InvoiceModel invoice, BolDetailViewModel bol, FuelPricingResponseViewModel pricingData)
        {
            decimal ppg;
            pricingData.PricePerGallon = invoice.UoM == UoM.MetricTons ? Math.Round(pricingData.PricePerGallon, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay) : Math.Round(pricingData.PricePerGallon, ApplicationConstants.InvoicePricePerGallonDecimalDisplay);
            pricingData.TerminalPrice = invoice.UoM == UoM.MetricTons ? Math.Round(pricingData.TerminalPrice, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay) : Math.Round(pricingData.TerminalPrice, ApplicationConstants.InvoicePricePerGallonDecimalDisplay);
            if (pricingData.FuelCost.HasValue)
            {
                pricingData.FuelCost = invoice.UoM == UoM.MetricTons ? Math.Round(pricingData.FuelCost.Value, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay) : Math.Round(pricingData.FuelCost.Value, ApplicationConstants.InvoicePricePerGallonDecimalDisplay);
            }
            bol.PricePerGallon = pricingData.PricePerGallon;
            bol.RackPrice = pricingData.TerminalPrice;
            invoice.PricePerGallon = pricingData.PricePerGallon;
            invoice.SupplierCost = pricingData.FuelCost;
            invoice.FuelCostTypeId = pricingData.FuelCostTypeId;
            invoice.RackPrice = pricingData.TerminalPrice;
            ppg = bol.PricePerGallon;

            //only in case of tier pricing Minquantity will have value
            if (pricingData.TierPricingDetails.MinQuantity.HasValue)
            {
                bol.TierPricingForBol.Add(new TierPricingForBol()
                {
                    PricePerGallon = ppg,
                    Quantity = invoice.DroppedGallons,
                    TierMinQuantity = pricingData.TierPricingDetails.MinQuantity.Value,
                    TierMaxQuantity = pricingData.TierPricingDetails.MaxQuantity.Value,
                });
            }

            return ppg;
        }

        protected async Task<InvoiceHeaderDetail> AddNewInvoiceHeader(List<InvoiceModel> invoices, InvoiceHeaderDetail existingHeaderDetails, int invoiceNumberId)
        {
            var invoice = invoices.First();

            InvoiceHeaderDetail entity = new InvoiceHeaderDetail();
            entity.InvoiceNumberId = invoiceNumberId;
            entity.TotalDroppedGallons = invoices.Sum(t => t.DroppedGallons);
            entity.TotalBasicAmount = invoices.Sum(t => t.BasicAmount);
            entity.TotalFeeAmount = invoices.Sum(t => t.TotalFeeAmount ?? 0);
            entity.TotalTaxAmount = invoices.Sum(t => t.TotalTaxAmount);
            entity.TotalDiscountAmount = invoices.Sum(t => t.TotalDiscountAmount);
            entity.Version = existingHeaderDetails.Version + 1;
            entity.CreatedBy = entity.UpdatedBy = invoice.CreatedBy;
            entity.CreatedDate = entity.UpdatedDate = invoice.CreatedDate;
            entity.IsActive = true;
            Context.DataContext.InvoiceHeaderDetails.Add(entity);
            await Context.CommitAsync();

            return entity;
        }

        //protected void UpdateExistingInvoiceHeader(InvoiceHeaderDetail existingHeaderDetails, InvoiceHeaderDetail newInvoiceHeader)
        //{
        //    existingHeaderDetails.IsActive = false;
        //    existingHeaderDetails.UpdatedBy = newInvoiceHeader.CreatedBy;
        //    existingHeaderDetails.UpdatedDate = newInvoiceHeader.CreatedDate;
        //    Context.DataContext.Entry(existingHeaderDetails).State = EntityState.Modified;
        //}

        protected bool GetTaxServiceEnabledFlag()
        {
            bool btaxServiceEnabled = true;
            var taxServiceEnabled = Context.DataContext.MstAppSettings
                .Where(t => t.Key == ApplicationConstants.KeyAppSettingTaxServiceEnabled)
                 .Select(t => t.Value).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(taxServiceEnabled))
            {
                try
                {
                    btaxServiceEnabled = Convert.ToBoolean(taxServiceEnabled);
                }
                catch
                {
                    btaxServiceEnabled = true;
                }
            }
            return btaxServiceEnabled;
        }

        protected void SetBolDetails(InvoiceViewModelNew createInvoiceViewModel, InvoiceDropViewModel dropViewModel, int? cityGroupTerminalId, InvoiceModel invoice)
        {
            SetBolDetails(createInvoiceViewModel, dropViewModel.FuelTypeId, cityGroupTerminalId, invoice);
        }

        public void SetBolDetails(InvoiceViewModelNew createInvoiceViewModel, int fuelTypeId, int? cityGroupTerminalId, InvoiceModel invoice)
        {
            var bolInfo = createInvoiceViewModel.BolDetails.Where(t => t.Products.Any(t1 => t1.ProductId == fuelTypeId)).ToList();
            var terminalIds = bolInfo.SelectMany(t => t.Products.Select(t1 => t1.TerminalId)).ToList();
            var terminals = Context.DataContext.MstExternalTerminals.Where(t => terminalIds.Contains(t.Id)).ToList();
            foreach (var bol in bolInfo)
            {
                foreach (var product in bol.Products.Where(t => t.ProductId == fuelTypeId))
                {
                    var terminal = terminals.FirstOrDefault(t => t.Id == product.TerminalId);
                    BolDetailViewModel bolDetail = GetBolDetails(createInvoiceViewModel.Carrier, fuelTypeId, cityGroupTerminalId, bol, product, terminal);
                    invoice.BolDetails.Add(bolDetail);
                }
            }
        }


        private static BolDetailViewModel GetBolDetails(string carrier, int fuelTypeId, int? cityGroupTerminalId, InvoiceBolViewModel bol, BolProductViewModel product, MstExternalTerminal terminal)
        {
            var bolDetail = new BolDetailViewModel()
            {
                NetQuantity = product.NetQuantity,
                DeliveredQuantity = product.DeliveredQuantity,
                GrossQuantity = product.GrossQuantity,
                BolNumber = bol.BolNumber,
                Carrier = carrier,
                LiftDate = bol.LiftDate,
                LiftStartTime= string.IsNullOrEmpty(bol.LiftStartTime) ? (TimeSpan?)null : Convert.ToDateTime(bol.LiftStartTime).TimeOfDay,
                LiftEndTime= string.IsNullOrEmpty(bol.LiftEndTime) ? (TimeSpan?)null : Convert.ToDateTime(bol.LiftEndTime).TimeOfDay,
                 PickupLocationType = PickupLocationType.Terminal,
                CityGroupTerminalId = cityGroupTerminalId,
                FuelTypeId = fuelTypeId,

                IsActive = true,
                IsDeleted = false,
                RackPrice = 0,
                BadgeNumber = bol.BadgeNumber
            };
            if (bol.Images != null && !string.IsNullOrWhiteSpace(bol.Images.FilePath))
            {
                bolDetail.Image = bol.Images;
            }
            if (terminal != null)
            {
                bolDetail.TerminalId = product.TerminalId;
                bolDetail.TerminalName = terminal.Name;
                bolDetail.Address = terminal.Address;
                bolDetail.City = terminal.City;
                bolDetail.StateCode = terminal.StateCode;
                bolDetail.StateId = terminal.StateId;
                bolDetail.ZipCode = terminal.ZipCode;
                bolDetail.CountryCode = terminal.CountryCode;
                bolDetail.Latitude = terminal.Latitude;
                bolDetail.Longitude = terminal.Longitude;
                bolDetail.CountyName = terminal.CountyName;
                bolDetail.SiteName = terminal.Name;
            }
            return bolDetail;
        }

        protected void SetLiftInformation(InvoiceViewModelNew createInvoiceViewModel, InvoiceDropViewModel dropViewModel, DropAdditionalDetailsModel deliveryDetails, InvoiceModel invoice, UserContext userContext)
        {
            SetLiftInformation(createInvoiceViewModel, dropViewModel.FuelTypeId, deliveryDetails, invoice, userContext);
        }

        public void SetLiftInformation(InvoiceViewModelNew createInvoiceViewModel, int fuelTypeId, DropAdditionalDetailsModel deliveryDetails, InvoiceModel invoice, UserContext userContext = null)
        {
            var liftInfo = createInvoiceViewModel.TicketDetails.Where(t => t.Products.Any(t1 => t1.ProductId == fuelTypeId)).ToList();
            foreach (var bol in liftInfo)
            {
                foreach (var product in bol.Products.Where(t => t.ProductId == fuelTypeId))
                {
                    if (product.Address == null && !string.IsNullOrEmpty(product.BulkPlantName) && userContext != null)
                    {
                        product.Address = getBulkPlantDetails(product, userContext);
                    }

                    if (product.Address != null)
                    {
                        BolDetailViewModel bolDetail = GetLiftInformation(createInvoiceViewModel.Carrier, fuelTypeId, deliveryDetails, bol, product);
                        invoice.BolDetails.Add(bolDetail);
                    }
                }
            }
        }
        private DropAddressViewModel getBulkPlantDetails(LiftProductViewModel liftProductViewModel, UserContext userContext)
        {
            DropAddressViewModel dropAddressViewModel = new DropAddressViewModel();
            var bulkPlantdetails = ContextFactory.Current.GetDomain<DispatchDomain>().GetBulkPlantDetailsByName(liftProductViewModel.BulkPlantName.Trim(), userContext.CompanyId);
            //FALL BACK FOR MAPPING
            if (bulkPlantdetails != null && bulkPlantdetails.SiteId == 0)
            {
                var bulkplantMapping = Context.DataContext.TerminalCompanyAliases.Where(t => t.IsActive
                                            && t.CreatedByCompanyId == userContext.CompanyId
                                        && t.AssignedTerminalId.ToLower() == liftProductViewModel.BulkPlantName.Trim().ToLower() && t.BulkPlantId != null)
                            .Select(t => new
                            {
                                t.BulkPlantLocation.City,
                                t.BulkPlantLocation.CountyName,
                                t.BulkPlantLocation.Latitude,
                                t.BulkPlantLocation.Longitude,
                                t.BulkPlantLocation.StateCode,
                                t.BulkPlantLocation.Address,
                                t.BulkPlantLocation.AddressLine2,
                                t.BulkPlantLocation.AddressLine3,
                                t.BulkPlantLocation.ZipCode,
                                t.BulkPlantLocation.CountryCode,
                                t.BulkPlantLocation.Name
                            })
                            .FirstOrDefault();
                if (bulkplantMapping != null)
                {
                    dropAddressViewModel.City = bulkplantMapping.City;
                    dropAddressViewModel.Latitude = bulkplantMapping.Latitude;
                    dropAddressViewModel.Longitude = bulkplantMapping.Longitude;
                    if (!string.IsNullOrEmpty(bulkplantMapping.StateCode))
                    {
                        var stateDetails = Context.DataContext.MstStates.Where(x => x.Code == bulkplantMapping.StateCode).FirstOrDefault();

                        if (stateDetails != null)
                        {
                            dropAddressViewModel.Country = new CountryViewModel();
                            dropAddressViewModel.State = new StateViewModel();
                            dropAddressViewModel.State.Code = stateDetails.Code;
                            dropAddressViewModel.State.Id = stateDetails.Id;
                            dropAddressViewModel.State.Name = stateDetails.Name;
                            dropAddressViewModel.Country.Code = stateDetails.MstCountry.Code;
                            dropAddressViewModel.Country.Id = stateDetails.MstCountry.Id;
                            dropAddressViewModel.Country.Name = stateDetails.MstCountry.Name;
                        }
                    }

                    dropAddressViewModel.Address = bulkplantMapping.Address;
                    dropAddressViewModel.AddressLine2 = bulkplantMapping.AddressLine2;
                    dropAddressViewModel.AddressLine3 = bulkplantMapping.AddressLine3;
                    dropAddressViewModel.ZipCode = bulkplantMapping.ZipCode;
                }
            }

            if (bulkPlantdetails != null && bulkPlantdetails.SiteId > 0 && !string.IsNullOrWhiteSpace(bulkPlantdetails.ZipCode))
            {
                dropAddressViewModel.City = bulkPlantdetails.City;
                dropAddressViewModel.Latitude = bulkPlantdetails.Latitude;
                dropAddressViewModel.Longitude = bulkPlantdetails.Longitude;
                if (!string.IsNullOrEmpty(bulkPlantdetails.State.Code))
                {
                    var stateDetails = Context.DataContext.MstStates.Where(x => x.Code == bulkPlantdetails.State.Code).FirstOrDefault();

                    if (stateDetails != null)
                    {
                        dropAddressViewModel.Country = new CountryViewModel();
                        dropAddressViewModel.State = new StateViewModel();
                        dropAddressViewModel.State.Code = stateDetails.Code;
                        dropAddressViewModel.State.Id = stateDetails.Id;
                        dropAddressViewModel.State.Name = stateDetails.Name;
                        dropAddressViewModel.Country.Code = stateDetails.MstCountry.Code;
                        dropAddressViewModel.Country.Id = stateDetails.MstCountry.Id;
                        dropAddressViewModel.Country.Name = stateDetails.MstCountry.Name;
                    }
                }

                dropAddressViewModel.Address = bulkPlantdetails.Address;
                dropAddressViewModel.AddressLine2 = bulkPlantdetails.AddressLine2;
                dropAddressViewModel.AddressLine3 = bulkPlantdetails.AddressLine3;
                dropAddressViewModel.ZipCode = bulkPlantdetails.ZipCode;
            }
            return dropAddressViewModel;
        }
        private static BolDetailViewModel GetLiftInformation(string carrier, int productId, DropAdditionalDetailsModel deliveryDetails, InvoiceLiftTicketViewModel bol, LiftProductViewModel product)
        {
            return new BolDetailViewModel()
            {
                LiftQuantity = product.LiftQuantity,
                GrossQuantity = product.GrossQuantity,
                NetQuantity = product.NetQuantity,
                DeliveredQuantity = product.DeliveredQuantity,
                LiftTicketNumber = bol.LiftTicketNumber,
                Carrier = carrier,
                LiftDate = bol.LiftDate,
                LiftStartTime = string.IsNullOrEmpty(bol.LiftStartTime) ? (TimeSpan?)null : Convert.ToDateTime(bol.LiftStartTime).TimeOfDay,
                LiftEndTime = string.IsNullOrEmpty(bol.LiftEndTime) ? (TimeSpan?)null : Convert.ToDateTime(bol.LiftEndTime).TimeOfDay,
                LiftArrivalTime = bol.LiftArrivalTime,
                BolCreationTime = bol.BolCreationTime,
                TerminalId = deliveryDetails.TerminalId,
                TerminalName = deliveryDetails.TerminalName,
                PickupLocationType = PickupLocationType.BulkPlant,
                CityGroupTerminalId = deliveryDetails.CityGroupTerminalId,
                FuelTypeId = productId,
                Address = product.Address.Address,
                City = product.Address.City,
                StateCode = product.Address.State.Code,
                StateId = product.Address.State.Id,
                ZipCode = product.Address.ZipCode,
                CountryCode = product.Address.Country.Code,
                Latitude = product.Address.Latitude,
                Longitude = product.Address.Longitude,
                CountyName = product.Address.CountyName,
                SiteName = product.BulkPlantName,
                Image = bol.Images != null && !string.IsNullOrWhiteSpace(bol.Images.FilePath) ? bol.Images : null,
                IsActive = true,
                IsDeleted = false,
                RackPrice = 0,
                BadgeNumber = bol.BadgeNumber
            };
        }

        protected bool IsPickupFromMultipleTerminals(InvoiceModel invoice, int pricingTypeId)
        {
            bool isRackprice = pricingTypeId == (int)PricingType.RackAverage || pricingTypeId == (int)PricingType.RackHigh || pricingTypeId == (int)PricingType.RackLow;
            bool isMultipleTerminals = invoice.BolDetails.Where(t => t.TerminalId != null).Select(t => t.TerminalId).Distinct().Count() > 1;
            return isRackprice && isMultipleTerminals;
        }

        protected void GetInvoicesForSameProduct(InvoiceModel invoice, List<InvoiceModel> invoices)
        {
            if (invoice.BolDetails.Any(t => t.CityGroupTerminalId == null))
            {
                var droppedQuantity = invoice.DroppedGallons;
                var terminals = invoice.BolDetails.GroupBy(t => t.TerminalId);
                var bolDetails = invoice.BolDetails.Where(t => t.TerminalId == terminals.FirstOrDefault().Key).Select(t => t);
                var terminalQuantity = bolDetails.Sum(t => t.LiftQuantity ?? 0) + bolDetails.Select(t => invoice.QuantityIndicatorTypeId == (int)QuantityIndicatorTypes.Net ? t.NetQuantity ?? 0 : t.GrossQuantity ?? 0).Sum();
                if (droppedQuantity > terminalQuantity && terminalQuantity != 0 && terminals.Count() > 1)
                {
                    var invoiceModel = invoice.CopyObject(invoice);
                    decimal remainingGallons = droppedQuantity - terminalQuantity;
                    var terminal = terminals.FirstOrDefault();
                    invoice.DroppedGallons = terminalQuantity;
                    invoice.BolDetails = invoice.BolDetails.Where(t => t.TerminalId == terminal.Key).ToList();
                    invoice.AdditionalDetail.TotalAllowance = 0;
                    invoice.AdditionalDetail.SupplierAllowance = null;
                    if (invoice.SurchargeFreightFeeViewModel != null && invoice.SurchargeFreightFeeViewModel.IsSurchargeApplicable)
                    {
                        invoice.SurchargeFreightFeeViewModel.IsSurchargeApplicable = false;
                    }
                    invoiceModel.BolDetails.RemoveAll(t => t.TerminalId == terminal.Key);
                    invoiceModel.DroppedGallons = remainingGallons;
                    if (!string.IsNullOrWhiteSpace(invoice.BrokeredChainId))
                    {
                        invoiceModel.BrokeredChainId = invoice.BrokeredChainId.Substring(0, invoice.BrokeredChainId.Length - 1) + invoices.Count;
                    }
                    AssignAssets(invoice, terminalQuantity, invoiceModel);
                    if (remainingGallons > 0)
                    {
                        invoices.Add(invoiceModel);
                        GetInvoicesForSameProduct(invoiceModel, invoices);
                    }
                }
            }
        }

        private static void AssignAssets(InvoiceModel invoice, decimal terminalQuantity, InvoiceModel invoiceModel)
        {
            decimal assetDroppedGallons = 0;
            List<AssetDropModel> assets = new List<AssetDropModel>();
            foreach (var asset in invoiceModel.AssetDrops)
            {
                if (assetDroppedGallons < terminalQuantity)
                {
                    assets.Add(asset);
                }
                else
                {
                    break;
                }
                assetDroppedGallons += asset.DropGallons ?? 0;
            }
            invoice.AssetDrops = new List<AssetDropModel>();
            foreach (var asset in assets)
            {
                invoice.AssetDrops.Add(asset);
                invoiceModel.AssetDrops.Remove(asset);
            }
        }

        protected void AddHedgeSpotAmountsOfInvoiceCreatedFromDDT(Order order, Invoice invoice, bool isRecursiveCallFromBrokered)
        {
            // Add taxes into hedge and spot amount in FR and Job table
            var orderTypeId = order.FuelRequest.OrderTypeId;

            decimal totalAmount = invoice.BasicAmount + invoice.TotalTaxAmount + invoice.TotalFeeAmount ?? 0 - invoice.TotalDiscountAmount;
            var baseTotalAmount = MoneyConverter.GetBaseAmount(invoice.Currency, totalAmount, invoice.ExchangeRate);

            if (orderTypeId == (int)OrderType.Hedge)
            {
                order.FuelRequest.HedgeDroppedAmount += totalAmount;
                order.FuelRequest.BaseHedgeDroppedAmount += baseTotalAmount;

                if (order.FuelRequest.Job.CompanyId == order.BuyerCompanyId)
                {
                    order.FuelRequest.Job.HedgeDroppedAmount += totalAmount;
                    order.FuelRequest.Job.BaseHedgeDroppedAmount += baseTotalAmount;
                }
            }
            else
            {
                order.FuelRequest.SpotDroppedAmount += totalAmount;
                order.FuelRequest.BaseSpotDroppedAmount += baseTotalAmount;

                if (order.FuelRequest.Job.CompanyId == order.BuyerCompanyId)
                {
                    order.FuelRequest.Job.SpotDroppedAmount += totalAmount;
                    order.FuelRequest.Job.BaseSpotDroppedAmount += baseTotalAmount;
                }
            }
        }
        public bool IsBadgeNumberMandatory(int orderId, int companyId)
        {
            bool response = false;
            try
            {
                var order = Context.DataContext.Orders.Where(t => t.Id == orderId).FirstOrDefault();
                if (order != null)
                {
                    var supplierXBuyerDetails = Context.DataContext.SupplierXBuyerDetails.FirstOrDefault(t => t.SupplierCompanyId == companyId
                                                                                           && t.BuyerCompanyId == order.BuyerCompanyId
                                                                                           && t.JobId == order.FuelRequest.JobId && t.IsActive);
                    if (supplierXBuyerDetails != null)
                    {
                        response = supplierXBuyerDetails.IsBadgeMandatory;
                    }
                    else
                    {
                        var onboardingPreferences = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == companyId && t.IsActive).FirstOrDefault();
                        if (onboardingPreferences != null)
                        {
                            response = onboardingPreferences.IsBadgeMandatory;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "IsBadgeNumberMandatory", ex.Message, ex);
            }
            return response;
        }

        protected async Task UpdateExceptionStatus(List<InvoiceExceptionViewModel> models)
        {
            try
            {
                var generatedExceptionIds = models.Select(t => t.GeneratedExceptionId).ToList();
                var exceptionDetails = await Context.DataContext.InvoiceExceptions.Where(t => generatedExceptionIds.Contains(t.GeneratedExceptionId) && t.IsActive).ToListAsync();
                exceptionDetails = exceptionDetails.Where(t => models.Any(t1 => t.InvoiceId == t1.InvoiceId && t.ExceptionTypeId == t1.ExceptionTypeId)).ToList();
                if (exceptionDetails != null && exceptionDetails.Any())
                {
                    foreach (var exceptionDetail in exceptionDetails)
                    {
                        exceptionDetail.StatusId = models[0].StatusId;
                        if (exceptionDetail.StatusId != (int)ExceptionStatus.Raised)
                            exceptionDetail.IsActive = false;
                        exceptionDetail.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.Entry(exceptionDetail).State = EntityState.Modified;
                        await Context.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCommonDomain", "UpdateExceptionStatus", ex.Message, ex);
            }
        }

        protected async Task<StatusViewModel> UpdateInvoiceStatusToDiscard(List<InvoiceExceptionViewModel> models)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var generatedExceptionIds = models.Select(t => t.GeneratedExceptionId).ToList();
                var exceptionDetails = await Context.DataContext.InvoiceExceptions.Where(t => generatedExceptionIds.Contains(t.GeneratedExceptionId) && t.IsActive).ToListAsync();
                exceptionDetails = exceptionDetails.Where(t => models.Any(t1 => t.InvoiceId == t1.InvoiceId && t.ExceptionTypeId == t1.ExceptionTypeId)).ToList();
                if (exceptionDetails != null && exceptionDetails.Any())
                {
                    foreach (var exceptionDetail in exceptionDetails)
                    {
                        // update invoice exception table
                        exceptionDetail.StatusId = models[0].StatusId;
                        if (exceptionDetail.StatusId != (int)ExceptionStatus.Raised)
                            exceptionDetail.IsActive = false;
                        exceptionDetail.UpdatedDate = DateTimeOffset.Now;

                        // update invoice
                        var invoice = exceptionDetail.Invoice;
                        invoice.InvoiceVersionStatusId = (int)InvoiceVersionStatus.InActive;
                        invoice.UpdatedDate = DateTimeOffset.Now;

                        var invoiceStatus = invoice.InvoiceXInvoiceStatusDetails.OrderByDescending(t => t.Id).FirstOrDefault(t => t.IsActive);
                        if (invoiceStatus != null)
                            invoiceStatus.StatusId = (int)InvoiceStatus.Rejected;

                        Context.DataContext.Entry(exceptionDetail).State = EntityState.Modified;
                        await Context.CommitAsync();
                    }

                    response.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCommonDomain", "UpdateInvoiceStatusToDiscard", ex.Message, ex);
            }

            return response;
        }

        protected async Task<int> AddAssetFromFilldDrop(int driverId, string timeZone, int jobId, int tfxFueltypeId, FilldAssetDropViewModel dropData)
        {
            int response = 0;
            AssetDomain assetDomain = new AssetDomain(this);
            var createdDate = (dropData.created_at ?? DateTimeOffset.Now).ToTargetDateTimeOffset(timeZone);
            var assetviewModel = new AssetViewModel
            {
                UserId = driverId,
                UpdatedBy = driverId,
                Name = dropData.asset_name,
                CreatedDate = createdDate,
                UpdatedDate = createdDate,
                FuelType = (dropData.fuel_type_id == 0 || tfxFueltypeId == 0) ? null : new AssetFuelTypeViewModel { Id = tfxFueltypeId },
                Type = (int)AssetType.Asset,
                AssetAdditionalDetail = new AssetAdditionalDetailViewModel
                {
                    Make = dropData.make,
                    Model = dropData.model,
                    Year = dropData.year
                }
            };
            var result = await assetDomain.SaveMobileAssetAsync(assetviewModel, jobId);
            if (result.StatusCode == Status.Success)
            {
                var jobXAsset = new AssetJobAssignmentViewModel
                {
                    JobId = jobId,
                    AssetId = assetviewModel.Id,
                    AssignedBy = assetviewModel.UserId,
                    AssignedDate = DateTimeOffset.Now
                };
                var userContext = new UserContext() { Id = assetviewModel.UserId };
                result = await assetDomain.AssignToJobAsync(userContext, jobXAsset);
                response = jobXAsset.Id;
            }
            return response;
        }

        protected async Task<TaxResponseViewModel> CalculateSetPDITaxesToInvoice(List<InvoiceModel> invoiceModels, List<PDITaxDetailsViewModel> pdiTaxes)
        {
            var response = new TaxResponseViewModel();
            //  var taxDetails = GetConsolidatedAvalaraTaxes(invoiceModels);
            if (invoiceModels != null && invoiceModels.Any())
            {
                var pdiOrderId = string.Empty;
                if (invoiceModels.FirstOrDefault().AdditionalDetail != null)
                {
                    // TAKE FIRST PDIOrderiD AS One invoiceheader = one pdiorderid
                    pdiOrderId = invoiceModels.FirstOrDefault().AdditionalDetail.PDIOrderId;

                }
                if (pdiTaxes != null && pdiTaxes.Any() && !string.IsNullOrWhiteSpace(pdiOrderId))
                {
                    var taxes = pdiTaxes.Where(t => t.PDIOrderNumber.Trim().ToLower() == pdiOrderId.Trim().ToLower()).ToList();
                    if (taxes != null && taxes.Any())
                    {
                        //set PDI invoice number
                        invoiceModels.ForEach(t => t.PDIInvoiceNo = taxes.FirstOrDefault().PDIInvoiceNo);
                        LogManager.Logger.WriteDebug("PDITaxController", "CalculateSetPDITaxesToInvoice", taxes.FirstOrDefault().PDIInvoiceNo);
                        var invoiceTaxDetails = new InvoiceTaxDetailsViewModel();
                        if (invoiceTaxDetails.AvaTaxDetails == null)
                            invoiceTaxDetails.AvaTaxDetails = new List<TaxDetailsViewModel>();
                        foreach (var taxItem in taxes)
                        {

                            var taxDetails = new TaxDetailsViewModel();
                            taxDetails.CalculationTypeInd = ApplicationConstants.CalculationTypeForNonStandard;
                            taxDetails.Currency = invoiceModels.FirstOrDefault().Currency.ToString();
                            taxDetails.TaxingLevel = ApplicationConstants.ExternalTaxingLevelSTA;
                            taxDetails.TaxType = ApplicationConstants.ExternalTaxTypeFUEL;
                            taxDetails.ProductCategory = 1;
                            taxDetails.RateType = ApplicationConstants.ExternalTaxRateTypeTAX;
                            taxDetails.RateSubtype = ApplicationConstants.RateSubTypeForNonStandard;
                            taxDetails.TaxRate = taxItem.TaxRate;
                            taxDetails.TaxAmount = taxItem.TaxAmount;
                            taxDetails.SalesTaxBaseAmount = taxItem.TaxAmount;
                            taxDetails.TaxExemptionInd = ApplicationConstants.PDITaxInd;
                            //taxDetails.RateDescription = taxItem.TaxRate >0 ? taxItem.TaxDescription + "("+taxItem.TaxRate.GetPreciseValue(6).ToString()+")": taxItem.TaxDescription;
                            taxDetails.RateDescription = GetCustomDisplayRateDescriptionForPDITax(taxItem.TaxRate, taxItem.TaxDescription, taxItem.TaxPricingTypeId);
                            taxDetails.UnitOfMeasure = ApplicationConstants.UnitOfMeasure;
                            taxDetails.IsModified = false;
                            taxDetails.TaxPricingTypeId = taxItem.TaxPricingTypeId;
                            taxDetails.TradingCurrency = invoiceModels.FirstOrDefault().Currency.ToString();
                            taxDetails.ExchangeRate = 1;
                            taxDetails.TradingTaxAmount = taxItem.TaxAmount;


                            invoiceTaxDetails.AvaTaxDetails.Add(taxDetails);
                        }
                        if (invoiceTaxDetails.AvaTaxDetails != null)
                        {
                            var totalTaxAmount = invoiceTaxDetails.AvaTaxDetails.Sum(t => t.TaxAmount);
                            invoiceTaxDetails.TotalTaxAmount = totalTaxAmount;
                            var firstInvoice = invoiceModels.FirstOrDefault();

                            firstInvoice.TaxDetails = invoiceTaxDetails;
                            firstInvoice.TotalTaxAmount = firstInvoice.TaxDetails.TotalTaxAmount;
                            firstInvoice.TransactionId = firstInvoice.DisplayInvoiceNumber;

                            invoiceTaxDetails.StatusCode = Status.Success;
                            response.TaxDetailsViewModel = invoiceTaxDetails;

                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.FailedStatusCode = (int)DDTConversionReason.PDITaxFailed;
                    }

                    foreach (var item in invoiceModels)
                    {
                        SetInvoiceBaseAmounts(item, item.ExchangeRate);
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.FailedStatusCode = (int)DDTConversionReason.PDITaxFailed;
                }
            }
            CheckForProcessingFeeOnTotalAmount(invoiceModels);
            return response;
        }

        public StatusViewModel RetrySendingDetailsToPdi(int headerId, string displayNumber, int userId)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var jsonViewModel = new PDIAPIRequestViewModel();
                jsonViewModel.InvoiceHeaderId = headerId;
                jsonViewModel.InvoiceNumber = displayNumber;
                AddQueueEventPDIAPI(jsonViewModel, userId);
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.msgPdiRetryRequestSuccess;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCommonDomain", "RetrySendingDetailsToPdi", ex.Message, ex);
                response.StatusMessage = ex.Message;
            }
            return response;
        }

        public static void SetQuantityFromConversionFactorUserValue(InvoiceModel invoice)
        {
            if (invoice.ConvertedQuantity == null || invoice.ConvertedQuantity == 0) //for broker case, it will not calculate again
            {
                invoice.ConvertedQuantity = invoice.DroppedGallons;
                invoice.DroppedGallons = invoice.DroppedGallons * invoice.ConversionFactor.Value;
                // invoice.AssetDrops.ForEach(asset => asset.DropGallons = asset.DropGallons * invoice.ConversionFactor.Value);
            }
        }

        public void SetConvertedQuantitiesAndGravityForMFN(InvoiceModel invoice, int jobCountryId, bool isFromGridEdit = false)
        {
            if (invoice.UoM == UoM.MetricTons || invoice.UoM == UoM.Barrels)
            {
                var modelForConversion = new MFNConversionRequestViewModel() { DroppedGallons = invoice.DroppedGallons, JobCountryId = jobCountryId, UoM = invoice.UoM };

                if (invoice.UoM == UoM.MetricTons)
                {
                    if (invoice.ConversionFactor.HasValue && invoice.ConversionFactor.Value > 0)
                    {
                        SetQuantityFromConversionFactorUserValue(invoice);
                    }
                    else
                    {
                        var gravity = invoice.Gravity.HasValue && invoice.Gravity > 0 ? invoice.Gravity.Value : 0;
                        if (!isFromGridEdit)
                        {
                            if (invoice.AdditionalDetail.CreationMethod == CreationMethod.Mobile && invoice.AssetDrops != null && invoice.AssetDrops.Any())
                            {
                                // taking gravity of first asset always 
                                gravity = invoice.AssetDrops.FirstOrDefault().Gravity.HasValue ? Math.Round(invoice.AssetDrops.FirstOrDefault().Gravity.Value, 1) : 0;
                                //set first assetdrop's gravity in invoice
                                invoice.Gravity = gravity;
                            }
                        }
                        modelForConversion.ConversionFactor = gravity;

                        if (modelForConversion.ConversionFactor > 0)
                        {
                            var invoiceDomain = new InvoiceDomain(this);
                            var conversionResponse = Task.Run(() => invoiceDomain.ValidateGravityAndConvertForMFN(modelForConversion)).Result;
                            invoice.ConvertedQuantity = conversionResponse.ConvertedQty;

                            if (invoice.AdditionalDetail.CreationMethod != CreationMethod.Mobile && invoice.AssetDrops != null && invoice.AssetDrops.Any())
                            {
                                invoice.AssetDrops.ForEach(asset => asset.Gravity = invoice.Gravity);
                                foreach (var asset in invoice.AssetDrops)
                                {
                                    if (asset.Gravity.HasValue && asset.Gravity.Value > 0)
                                    {
                                        var conversionRequest = new MFNConversionRequestViewModel() { DroppedGallons = asset.DropGallons.Value, ConversionFactor = asset.Gravity.Value, JobCountryId = jobCountryId, UoM = invoice.UoM };
                                        var response = Task.Run(() => new InvoiceDomain(this).ValidateGravityAndConvertForMFN(conversionRequest)).Result;
                                        asset.DropGallons = response.ConvertedQty;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (invoice.UoM == UoM.Barrels)
                {
                    var invoiceDomain = new InvoiceDomain(this);
                    var conversionResponse = Task.Run(() => invoiceDomain.ValidateGravityAndConvertForMFN(modelForConversion)).Result;
                    invoice.ConvertedQuantity = conversionResponse.ConvertedQty;

                    if (invoice.AdditionalDetail.CreationMethod != CreationMethod.Mobile && invoice.AssetDrops != null && invoice.AssetDrops.Any())
                    {
                        foreach (var asset in invoice.AssetDrops)
                        {
                            var conversionRequest = new MFNConversionRequestViewModel() { DroppedGallons = asset.DropGallons.Value, JobCountryId = jobCountryId, UoM = invoice.UoM };
                            var response = Task.Run(() => new InvoiceDomain(this).ValidateGravityAndConvertForMFN(conversionRequest)).Result;
                            asset.DropGallons = response.ConvertedQty;
                        }
                    }
                }
            }
        }

        public string GetCustomDisplayRateDescriptionForPDITax(decimal taxRate, string taxDescription, int? taxPricingTypeId = (int)OtherProductTaxPricingType.DollarOnTotalAmount)
        {
            var customString = string.Empty;
            try
            {
                if (taxRate > 0)
                {
                    var displayTaxRate = (taxPricingTypeId.HasValue && taxPricingTypeId.Value == 2) ? string.Format("({0}%) ", taxRate) : string.Format("({0})", taxRate);
                    customString = taxDescription + "" + displayTaxRate;
                }
                else
                {
                    return taxDescription;
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCommonDomain", "GetCustomDisplayRateDescriptionForPDITax", ex.Message, ex);
            }
            return customString;
        }
    }
}
