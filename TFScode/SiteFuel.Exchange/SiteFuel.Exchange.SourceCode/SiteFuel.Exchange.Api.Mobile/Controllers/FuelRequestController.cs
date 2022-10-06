using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Web.Http.ModelBinding;
using SiteFuel.Exchange.Api.Mobile.Attributes;
using System.Web.Http.Description;
using SiteFuel.Exchange.Api.Mobile.Common;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Core.Logger;
using System.Linq;

namespace SiteFuel.Exchange.Api.Mobile.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif

    [ValidateToken]
    public class FuelRequestController : ApiBaseController
    {
        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<CreateFuelRequestOutputViewModel> Create(FuelRequestViewModel fuelRequestViewModel)
        {
            using (var tracer = new Tracer("FuelRequestController", "Create"))
            {
                var response = new CreateFuelRequestOutputViewModel();
                try
                {
                    if (ModelState.IsValid)
                    {
                        var userContext = await GetUserContext(fuelRequestViewModel.UpdatedBy, CompanyType.Buyer);
                        var result = await ContextFactory.Current.GetDomain<FuelRequestDomain>().SaveFuelRequestAsync(fuelRequestViewModel, true, fuelRequestViewModel.CounterOfferSupplierId, userContext);
                        response.FuelRequestId = fuelRequestViewModel.Id;
                        response.StatusCode = result.StatusCode;
                        response.StatusMessage = result.StatusMessage;
                    }
                    else
                    {
                        var geterror = new CommonMethods().GetErrorMessage(ModelState);
                        response.StatusCode = geterror.StatusCode;
                        response.StatusMessage = geterror.StatusMessage;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("FuelRequestController", "Create", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<CreateFuelRequestOutputViewModel> CreateFuelRequest(BuyerAppFuelRequestViewModel fuelRequestViewModel)
        {
            using (var tracer = new Tracer("FuelRequestController", "CreateFuelRequest"))
            {
                var response = new CreateFuelRequestOutputViewModel();
                try
                {
                    if (ModelState.IsValid)
                    {
                        response = await CheckValidation(fuelRequestViewModel);

                        if (response.StatusCode == Status.Success)
                        {
                            FuelRequestViewModel viewModel = ToFuelRequestViewModel(fuelRequestViewModel);
                            var userContext = await GetUserContext(fuelRequestViewModel.UserId, CompanyType.Buyer);
                            FuelRequestDomain fuelRequestDomain = new FuelRequestDomain();

                            if (fuelRequestViewModel.FuelFees == null || fuelRequestViewModel.FuelFees.FuelRequestFees.Count == 0)
                                viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = fuelRequestDomain.ToFuelRequestFeesViewModel(viewModel.FuelDeliveryDetails.FuelRequestFee);

                            var pricingTypeId = (fuelRequestViewModel.PricingTypeId == (int)PricingType.RackHigh || fuelRequestViewModel.PricingTypeId == (int)PricingType.RackLow) ? (int)PricingType.RackAverage : fuelRequestViewModel.PricingTypeId;
                            var codeViewModel = new PricingCodesRequestViewModel() { PricingSourceId = fuelRequestViewModel.PricingSourceId, PricingTypeId = pricingTypeId, RackTypeId = fuelRequestViewModel.PricingTypeId };
                            var codeList = await ContextFactory.Current.GetDomain<PricingServiceDomain>().GetPricingCodesAsync(codeViewModel);
                            if (codeList.PricingCodes != null && codeList.PricingCodes.Any())
                            {
                                var code = codeList.PricingCodes.First();
                                viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingCode.Id = code.Id;
                                viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingCode.Code = code.Code;
                                
                                var jobResponse = await fuelRequestDomain.SaveFuelRequestAsync(viewModel, true, viewModel.CounterOfferSupplierId, userContext);

                                response.FuelRequestId = viewModel.Id;
                                response.StatusCode = jobResponse.StatusCode;
                                response.StatusMessage = jobResponse.StatusMessage;
                            }
                        }
                    }
                    else
                    {
                        var geterror = new CommonMethods().GetErrorMessage(ModelState);
                        response.StatusCode = geterror.StatusCode;
                        response.StatusMessage = geterror.StatusMessage;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("FuelRequestController", "CreateFuelRequest", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        public MasterDataViewModel GetMasterData(int companyId = 0)
        {
            using (var tracer = new Tracer("FuelRequestController", "GetMasterData"))
            {
                MasterDataViewModel response = new MasterDataViewModel();
                try
                {
                    response = ContextFactory.Current.GetDomain<FuelRequestDomain>().GetMasterData(companyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("FuelRequestController", "GetMasterData", ex.Message, ex);
                }
                return response;
            }
        }

        private FuelRequestViewModel ToFuelRequestViewModel(BuyerAppFuelRequestViewModel fuelRequest)
        {
            var viewModel = new FuelRequestViewModel();

            viewModel.FuelDetails.FuelTypeId = fuelRequest.FuelTypeId;

            viewModel.FuelDetails.FuelQuantity.QuantityTypeId = fuelRequest.QuantityTypeId;

            if (fuelRequest.QuantityTypeId == (int)QuantityType.SpecificAmount)
            {
                viewModel.FuelDetails.FuelQuantity.Quantity = fuelRequest.MaximumQuantity.GetPreciseValue(6);
            }
            else if (fuelRequest.QuantityTypeId == (int)QuantityType.Range)
            {
                viewModel.FuelDetails.FuelQuantity.MinimumQuantity = fuelRequest.MinimumQuantity.GetPreciseValue(6);
                viewModel.FuelDetails.FuelQuantity.MaximumQuantity = fuelRequest.MaximumQuantity.GetPreciseValue(6);
            }

            viewModel.FuelDetails.FuelPricing.PricingTypeId = fuelRequest.PricingTypeId;
            viewModel.FuelDetails.FuelPricing.RackAvgTypeId = fuelRequest.RackAvgTypeId;

            if (fuelRequest.PricingTypeId == (int)PricingType.PricePerGallon)
            {
                viewModel.FuelDetails.FuelPricing.PricePerGallon = fuelRequest.PricePerGallon.GetPreciseValue(6);
            }
            else if (fuelRequest.PricingTypeId == (int)PricingType.Suppliercost)
            {
                viewModel.FuelDetails.FuelPricing.SupplierCostMarkupTypeId = fuelRequest.RackAvgTypeId;
                viewModel.FuelDetails.FuelPricing.SupplierCostMarkupValue = fuelRequest.PricePerGallon.GetPreciseValue(6);
            }
            else
            {
                viewModel.FuelDetails.FuelPricing.RackPrice = fuelRequest.PricePerGallon.GetPreciseValue(6);
            }

            viewModel.TPOSupplierId = fuelRequest.TPOSupplierId;
            if (fuelRequest.TPOSupplierId == 0)
            {
                viewModel.FuelOfferDetails.PrivateSupplierList.IsPublicRequest = fuelRequest.IsPublicRequest;
                if (!fuelRequest.IsPublicRequest)
                {
                    viewModel.FuelOfferDetails.PrivateSupplierList.Id = fuelRequest.PrivateSupplierListId;
                    int PrivateSupplierListId = fuelRequest.PrivateSupplierListId.HasValue ? fuelRequest.PrivateSupplierListId.Value : 0;

                    if (PrivateSupplierListId > 0)
                    {
                        List<int> supplierIds = new List<int>();
                        supplierIds.Add(PrivateSupplierListId);
                        viewModel.FuelOfferDetails.PrivateSupplierList.PrivateSupplierIds = supplierIds;
                    }
                }
            }

            viewModel.FuelDetails.StatusId = (int)FuelRequestStatus.Open;

            viewModel.FuelDetails.CreatedBy = fuelRequest.UserId;
            viewModel.FuelDetails.CreatedDate = DateTime.Now;

            viewModel.FuelDeliveryDetails.DeliveryTypeId = fuelRequest.DeliveryTypeId;
            viewModel.FuelDeliveryDetails.StartDate = fuelRequest.StartDate;
            viewModel.FuelDeliveryDetails.EndDate = fuelRequest.EndDate;
            viewModel.FuelDeliveryDetails.StartTime = Convert.ToDateTime(fuelRequest.StartTime.ToString()).ToShortTimeString();
            viewModel.FuelDeliveryDetails.EndTime = Convert.ToDateTime(fuelRequest.EndTime.ToString()).ToShortTimeString();

            viewModel.FuelDeliveryDetails.FuelRequestFee.DeliveryFeeByQuantity = fuelRequest.FuelRequestFee.DeliveryFeeByQuantity;
            viewModel.FuelDeliveryDetails.FuelRequestFee = fuelRequest.FuelRequestFee;
            viewModel.FuelDeliveryDetails.FuelRequestFee.AdditionalFee = fuelRequest.FuelRequestFee.AdditionalFee;
            viewModel.FuelDeliveryDetails.SpecialInstructions = fuelRequest.SpecialInstructions;

            viewModel.FuelDeliveryDetails.FuelFees = fuelRequest.FuelFees;

            if (fuelRequest.DeliverySchedules != null)
            {
                fuelRequest.DeliverySchedules.ForEach(t => t.CreatedBy = fuelRequest.UserId);
            }
            viewModel.FuelDeliveryDetails.DeliverySchedules = fuelRequest.DeliverySchedules;

            viewModel.FuelOfferDetails.PaymentTermId = fuelRequest.PaymentTermId;
            viewModel.FuelOfferDetails.NetDays = fuelRequest.NetDays;
            viewModel.FuelDeliveryDetails.ExpirationDate = fuelRequest.ExpirationDate;
            viewModel.ExternalPoNumber = string.IsNullOrWhiteSpace(fuelRequest.ExternalPoNumber) ? null : fuelRequest.ExternalPoNumber;

            viewModel.CompanyId = fuelRequest.CompanyId;
            viewModel.Job.JobId = fuelRequest.JobId;

            viewModel.FuelDetails.IsOverageAllowed = fuelRequest.IsOverageAllowed;
            viewModel.FuelDetails.OverageAllowedPercent = fuelRequest.OverageAllowedPercent.GetPreciseValue(6);
            viewModel.FuelDetails.OrderTypeId = fuelRequest.OrderTypeId;
            viewModel.FuelOfferDetails.OrderClosingThreshold = fuelRequest.OrderClosingThreshold;
            if (fuelRequest.SupplierQualifications.Count > 0)
            {
                viewModel.FuelOfferDetails.SupplierQualifications = fuelRequest.SupplierQualifications;
            }

            if (fuelRequest.TruckLoadTypeId == (int)TruckLoadTypes.FullTruckLoad)
            {
                viewModel.FuelDeliveryDetails.TruckLoadTypes = (TruckLoadTypes)fuelRequest.TruckLoadTypeId;
                viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId = fuelRequest.PricingSourceId;
                viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceQuantityIndicatorTypes = (QuantityIndicatorTypes)(fuelRequest.PricingQuantityIndicatorTypeId == null ? 0 : fuelRequest.PricingQuantityIndicatorTypeId);
                viewModel.FuelDeliveryDetails.PricingQuantityIndicatorTypeId = fuelRequest.PricingQuantityIndicatorTypeId; // Gross/Net FuelReq Quantity type
                viewModel.FuelDetails.FuelQuantity.QuantityIndicatorTypes = (QuantityIndicatorTypes)(fuelRequest.FrQuantityIndicatorTypeId==null?0:fuelRequest.FrQuantityIndicatorTypeId);   // Gross/Net FuelReq Quantity type
                viewModel.FuelDetails.FuelPricing.FuelPricingDetails.FeedTypeId = fuelRequest.FeedTypeId;
                viewModel.FuelDetails.FuelPricing.FuelPricingDetails.FreightOnBoardTypes = (FreightOnBoardTypes)fuelRequest.FobTypeId;
                viewModel.FuelDetails.FuelPricing.FuelPricingDetails.FuelClassTypes = (FuelClassTypes)(fuelRequest.FuelClassId==null?0:fuelRequest.FuelClassId);
            }
            return viewModel;
        }

        private BuyerAppFuelRequestViewModel ToBuyerAppFuelRequestViewModel(FuelRequestViewModel fuelRequest)
        {
            var viewModel = new BuyerAppFuelRequestViewModel();

            viewModel.FuelTypeId = fuelRequest.FuelDetails.FuelTypeId.Value;

            viewModel.QuantityTypeId = fuelRequest.FuelDetails.FuelQuantity.QuantityTypeId;

            if (fuelRequest.FuelDetails.FuelQuantity.QuantityTypeId == (int)QuantityType.SpecificAmount)
            {
                viewModel.MaximumQuantity = fuelRequest.FuelDetails.FuelQuantity.Quantity.GetPreciseValue(6);
            }
            else if (fuelRequest.FuelDetails.FuelQuantity.QuantityTypeId == (int)QuantityType.Range)
            {
                viewModel.MinimumQuantity = fuelRequest.FuelDetails.FuelQuantity.MinimumQuantity.GetPreciseValue(6);
                viewModel.MaximumQuantity = fuelRequest.FuelDetails.FuelQuantity.MaximumQuantity.GetPreciseValue(6);
            }

            viewModel.PricingTypeId = fuelRequest.FuelDetails.FuelPricing.PricingTypeId;
            viewModel.RackAvgTypeId = fuelRequest.FuelDetails.FuelPricing.RackAvgTypeId;

            if (fuelRequest.FuelDetails.FuelPricing.PricingTypeId == (int)PricingType.PricePerGallon)
            {
                viewModel.PricePerGallon = fuelRequest.FuelDetails.FuelPricing.PricePerGallon.GetPreciseValue(6);
            }
            else if (fuelRequest.FuelDetails.FuelPricing.PricingTypeId == (int)PricingType.RackAverage)
            {
                viewModel.PricePerGallon = fuelRequest.FuelDetails.FuelPricing.RackPrice.GetPreciseValue(6);
            }

            viewModel.IsPublicRequest = fuelRequest.FuelOfferDetails.PrivateSupplierList.IsPublicRequest;
            if (fuelRequest.FuelOfferDetails.PrivateSupplierList.IsPublicRequest == false)
            {
                viewModel.PrivateSupplierListId = fuelRequest.FuelOfferDetails.PrivateSupplierList.Id;
            }

            viewModel.DeliveryTypeId = fuelRequest.FuelDeliveryDetails.DeliveryTypeId;
            if (viewModel.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries && fuelRequest.FuelDeliveryDetails.StartDate.Date < DateTime.Now.Date)
            {
                viewModel.StartDate = DateTime.Now;
            }
            else
            {
                viewModel.StartDate = fuelRequest.FuelDeliveryDetails.StartDate;
            }
            viewModel.EndDate = fuelRequest.FuelDeliveryDetails.EndDate;
            viewModel.StartTime = Convert.ToDateTime(fuelRequest.FuelDeliveryDetails.StartTime.ToString()).ToShortTimeString();
            viewModel.EndTime = Convert.ToDateTime(fuelRequest.FuelDeliveryDetails.EndTime.ToString()).ToShortTimeString();

            viewModel.FuelRequestFee.DeliveryFeeByQuantity = fuelRequest.FuelDeliveryDetails.FuelRequestFee.DeliveryFeeByQuantity;
            viewModel.FuelRequestFee = fuelRequest.FuelDeliveryDetails.FuelRequestFee;
            viewModel.FuelRequestFee.AdditionalFee = fuelRequest.FuelDeliveryDetails.FuelRequestFee.AdditionalFee;
            viewModel.SpecialInstructions = fuelRequest.FuelDeliveryDetails.SpecialInstructions;
            //viewModel.DeliverySchedules = fuelRequest.FuelDeliveryDetails.DeliverySchedules;

            var jobCurrentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName);
            foreach (var schedule in fuelRequest.FuelDeliveryDetails.DeliverySchedules)
            {
                if (schedule.ScheduleType == (int)DeliveryScheduleType.SpecificDates)
                {
                    var scheduleEndTime = Convert.ToDateTime(schedule.ScheduleEndTime).TimeOfDay;
                    if (schedule.ScheduleDate.Add(scheduleEndTime) >= jobCurrentDate.DateTime)
                    {
                        viewModel.DeliverySchedules.Add(schedule);
                    }
                }
                else
                {
                    viewModel.DeliverySchedules.Add(schedule);
                }
            }

            viewModel.PaymentTermId = fuelRequest.FuelOfferDetails.PaymentTermId;
            viewModel.NetDays = fuelRequest.FuelOfferDetails.NetDays;
            viewModel.ExpirationDate = fuelRequest.FuelDeliveryDetails.ExpirationDate;
            viewModel.ExternalPoNumber = fuelRequest.ExternalPoNumber;

            viewModel.CompanyId = fuelRequest.CompanyId;
            viewModel.JobId = fuelRequest.Job.JobId;

            viewModel.IsOverageAllowed = fuelRequest.FuelDetails.IsOverageAllowed;
            viewModel.OverageAllowedPercent = fuelRequest.FuelDetails.OverageAllowedPercent.GetPreciseValue(6);
            viewModel.OrderTypeId = fuelRequest.FuelDetails.OrderTypeId;
            viewModel.OrderClosingThreshold = fuelRequest.FuelOfferDetails.OrderClosingThreshold;

            if (fuelRequest.FuelOfferDetails.SupplierQualifications.Count > 0)
            {
                viewModel.SupplierQualifications = fuelRequest.FuelOfferDetails.SupplierQualifications;
            }

            return viewModel;
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<BuyerAppFuelRequestViewModel> GetCloneOrderDetails(int orderId)
        {
            using (var tracer = new Tracer("FuelRequestController", "GetCloneOrderDetails"))
            {
                var response = new BuyerAppFuelRequestViewModel();
                try
                {
                    int fuelRequestId = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestIdAsync(orderId);
                    var fuelRequestResponse = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestAsync(fuelRequestId);
                    response = ToBuyerAppFuelRequestViewModel(fuelRequestResponse);
                    response.ExternalPoNumber = string.Empty;
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("FuelRequestController", "GetCloneOrderDetails", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public List<DropdownDisplayItem> GetPrivateSupplierList(int companyId)
        {
            using (var tracer = new Tracer("FuelRequestController", "GetPrivateSupplierList"))
            {
                var response = new List<DropdownDisplayItem>();
                try
                {
                    return ContextFactory.Current.GetDomain<MasterDomain>().GetPrivateSupplierList(companyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("FuelRequestController", "GetPrivateSupplierList", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<DropdownDisplayItem>> GetSupplierFuelTypes(int companyId)
        {
            using (var tracer = new Tracer("FuelRequestController", "GetSupplierFuelTypes"))
            {
                var response = new List<DropdownDisplayItem>();
                try
                {
                    response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetSupplierFuelTypesForOpenOrder(companyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("FuelRequestController", "GetSupplierFuelTypes", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<PendingRequestViewModel>> GetPendingRequest(int jobId)
        {
            using (var tracer = new Tracer("FuelRequestController", "GetPendingRequest"))
            {
                var response = new List<PendingRequestViewModel>();
                try
                {
                    return await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetPendingRequest(jobId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("FuelRequestController", "GetPendingRequest", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        public async Task<List<MobileFeeType>> GetFeeTypes(int companyId)
        {
            var response = await ContextFactory.Current.GetDomain<MasterDomain>().GetMobileFeeTypes(companyId);
            return response;
        }

        private async Task<CreateFuelRequestOutputViewModel> CheckValidation(BuyerAppFuelRequestViewModel fuelRequestViewModel)
        {
            using (var tracer = new Tracer("FuelRequestController", "CheckValidation"))
            {
                var response = new CreateFuelRequestOutputViewModel();
                try
                {
                    if (!string.IsNullOrEmpty(fuelRequestViewModel.ExternalPoNumber))
                    {
                        bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidPONumber(0, fuelRequestViewModel.ExternalPoNumber, fuelRequestViewModel.CompanyId);
                        if (!result)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.valMessageAlreadyExist, fuelRequestViewModel.ExternalPoNumber);
                            return response;
                        }
                    }

                    var jobDetails = await ContextFactory.Current.GetDomain<JobDomain>().GetJobDetailsAsync(fuelRequestViewModel.JobId);
                    if (fuelRequestViewModel.StartDate.Date < jobDetails.StartDate.Date)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.valMessageLessThanJobStartDate;
                        return response;
                    }

                    if (jobDetails.EndDate != null && fuelRequestViewModel.StartDate.Date > jobDetails.EndDate.Value)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.valMessageLessThanJobEndDate;
                        return response;
                    }

                    if (fuelRequestViewModel.FuelFees.FuelRequestFees.Any(t =>
                        t.FeeSubTypeId == (int)FeeSubType.ByQuantity && !t.DeliveryFeeByQuantity.Any()))
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageFeeByQuantityRequired;
                        return response;
                    }

                    if (fuelRequestViewModel.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries)
                    {
                        foreach (var schedule in fuelRequestViewModel.DeliverySchedules)
                        {
                            if (schedule.ScheduleType == (int)DeliveryScheduleType.SpecificDates ||
                                    schedule.ScheduleType == (int)DeliveryScheduleType.Monthly)
                            {
                                //Compare with FR Start and End Date
                                if (schedule.ScheduleDate.Date < fuelRequestViewModel.StartDate.Date)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = Resource.valMessageDeliveryScheduleDateLessThanFRStartDate;
                                    return response;
                                }

                                if (fuelRequestViewModel.EndDate != null && schedule.ScheduleDate.Date > fuelRequestViewModel.EndDate.Value)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = Resource.valMessageDeliveryScheduleDateGreaterThanFRStartDate;
                                    return response;
                                }
                                //Compare with FR Start and End Date


                                //Compare with Job Start and End Date
                                if (schedule.ScheduleDate.Date < jobDetails.StartDate.Date)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = Resource.valMessageDeliveryScheduleDateLessThanJobStartDate;
                                    return response;
                                }

                                if (jobDetails.EndDate != null && schedule.ScheduleDate.Date > jobDetails.EndDate.Value)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = Resource.valMessageDeliveryScheduleDateGreaterThanJobStartDate;
                                    return response;
                                }
                                //Compare with Job Start and End Date
                            }
                        }

                        if (fuelRequestViewModel.QuantityTypeId != (int)QuantityType.NotSpecified)
                        {
                            decimal totalQuantity = 0;
                            foreach (var schedule in fuelRequestViewModel.DeliverySchedules)
                            {
                                if (schedule.ScheduleType == (int)DeliveryScheduleType.Weekly || schedule.ScheduleType == (int)DeliveryScheduleType.BiWeekly)
                                {
                                    totalQuantity = +schedule.ScheduleQuantity * schedule.ScheduleDays.Count;
                                }
                                else
                                {
                                    totalQuantity = +schedule.ScheduleQuantity;
                                }
                            }

                            if (totalQuantity > fuelRequestViewModel.MaximumQuantity)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.valMessageScheduleQuantity;
                                return response;
                            }
                        }
                    }

                    response.StatusCode = Status.Success;
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("FuelRequestController", "CheckValidation", ex.Message, ex);
                }
                return response;
            }
        }
    }
}
