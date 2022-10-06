using Newtonsoft.Json;
using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.DeliveryRequest;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Linq;
using SiteFuel.FreightModels.Otto;
using System;

namespace TrueFill.FreightApi.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
        [ApiExplorerSettings(IgnoreApi = true)]
#endif

    public class DeliveryRequestController : ApiController
    {
        private readonly IDeliveryRequestDomain _requestDomain;
        private readonly IDRCarrierSequenceDomain _drCarrierDomain;

        public DeliveryRequestController(IDeliveryRequestDomain requestDomain, IDRCarrierSequenceDomain drCarrierDomain)
        {
            _requestDomain = requestDomain;
            _drCarrierDomain = drCarrierDomain;
        }

        [HttpPost]
        public async Task<DeliveryRequestsViewModel> Create(List<DeliveryRequestViewModel> deliveryRequests)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "Create"))
            {
                var response = await _requestDomain.CreateDeliveryRequest(deliveryRequests);
                var reDRresponse = await _requestDomain.CreateRecurringSchedules(deliveryRequests);
                if (string.IsNullOrEmpty(response.StatusMessage))
                {
                    if (reDRresponse.StatusCode == (int)Status.Failed)
                    {
                        response.StatusMessage = ApplicationConstants.failedMessageIdentification + reDRresponse.StatusMessage + ApplicationConstants.failedMessageIdentification;
                    }
                    else
                    {
                        if (response.StatusCode == (int)Status.Warning && reDRresponse.StatusCode == (int)Status.Success)
                        {
                            response.StatusCode = (int)Status.Success;
                            response.StatusMessage = reDRresponse.StatusMessage;
                        }
                    }

                }
                else
                {
                    SetResponseMessage(response, reDRresponse);

                }
                return response;
            }
        }

        [HttpPost]
        public async Task<DeliveryRequestsViewModel> CreateFromBuyerApp(List<DeliveryRequestViewModel> deliveryRequests)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "CreateFromBuyerApp"))
            {
                var response = await _requestDomain.CreateDeliveryRequestFromBuyerApp(deliveryRequests);
                var reDRresponse = new StatusModel();
                if (response.StatusCode != (int)Status.Failed && deliveryRequests.Any(t => t.isRecurringSchedule && t.RecurringSchdule != null && t.RecurringSchdule.Any()))
                {
                    reDRresponse = await _requestDomain.CreateRecurringSchedules(deliveryRequests);
                    response.StatusCode = reDRresponse.StatusCode;
                    response.StatusMessage = reDRresponse.StatusMessage;
                }
                else
                {
                    return response;
                }
                return response;
            }
        }

        private static void SetResponseMessage(StatusModel response, StatusModel reDRresponse)
        {
            if (response.StatusCode == (int)Status.Failed && reDRresponse.StatusCode == (int)Status.Success)
            {
                response.StatusCode = (int)Status.Success;
                response.StatusMessage = ApplicationConstants.failedMessageIdentification + response.StatusMessage + ApplicationConstants.failedMessageIdentification + ApplicationConstants.messageSplitTag + reDRresponse.StatusMessage;
            }
            else if (reDRresponse.StatusCode == (int)Status.Failed && response.StatusCode == (int)Status.Success)
            {
                response.StatusCode = (int)Status.Success;
                response.StatusMessage = response.StatusMessage + ApplicationConstants.messageSplitTag + ApplicationConstants.failedMessageIdentification + reDRresponse.StatusMessage + ApplicationConstants.failedMessageIdentification;
            }
            else if (response.StatusCode == (int)Status.Failed && reDRresponse.StatusCode == (int)Status.Failed)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = ApplicationConstants.failedMessageIdentification + response.StatusMessage + ApplicationConstants.failedMessageIdentification + ApplicationConstants.messageSplitTag + ApplicationConstants.failedMessageIdentification + reDRresponse.StatusMessage + ApplicationConstants.failedMessageIdentification;
            }
            else if (response.StatusCode == (int)Status.Success && reDRresponse.StatusCode == (int)Status.Success)
            {
                response.StatusCode = (int)Status.Success;
                response.StatusMessage = response.StatusMessage + ApplicationConstants.messageSplitTag + reDRresponse.StatusMessage;
            }
            else if (response.StatusCode == (int)Status.Success && reDRresponse.StatusCode == (int)Status.Warning)
            {
                response.StatusCode = (int)Status.Success;
                response.StatusMessage = response.StatusMessage;
            }
            else if (response.StatusCode == (int)Status.Warning && reDRresponse.StatusCode == (int)Status.Success)
            {
                response.StatusCode = (int)Status.Success;
                response.StatusMessage = reDRresponse.StatusMessage;
            }
            else
            {
                if (response.StatusCode == (int)Status.Warning && reDRresponse.StatusCode == (int)Status.Success)
                {
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = reDRresponse.StatusMessage;
                }
                else if (response.StatusCode == (int)Status.Success && reDRresponse.StatusCode == (int)Status.Warning)
                {
                    response.StatusCode = (int)Status.Success;
                }
            }
        }

        [HttpPost]
        public async Task<StatusModel> ReCreate(ReCreateDeliveryRequestsViewModel viewModel)
        {
            var json = JsonConvert.SerializeObject(viewModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::DeliveryRequestController", $"ReCreate(deliveryRequests:{json})"))
            {
                viewModel.DeliveryRequests.ForEach(t => t.CreatedBy = viewModel.UserId);
                var response = await _requestDomain.ReCreateDeliveryRequest(viewModel);
                return response;
            }
        }

        [HttpPost]
        public async Task<List<TBDRequestDetailModel>> GetTbdDeliveryRequestDetails(List<string> deliveryRequestIds)
        {
            var response = await _requestDomain.GetTbdDeliveryRequestDetails(deliveryRequestIds);
            return response;
        }

        [HttpGet]
        public async Task<List<DeliveryRequestViewModel>> GetDeliveryRequests(int companyId, string regionId, string selectedDate = null)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "GetDeliveryRequests"))
            {
                var response = await _requestDomain.GetDeliveryRequests(companyId, regionId, selectedDate);
                return response;
            }
        }
        [HttpPost]
        public List<DeliveryRequestViewModel> GetCalendarDeliveryRequests(int companyId, CalendarFilterModel inputModel)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "GetCalendarDeliveryRequests"))
            {
                var response = _requestDomain.GetCalendarDeliveryRequests(companyId, inputModel);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> ProcessCarrierDeliveyForOttoAlerts()
        {
            using (var tracer = new Tracer("DeliveryRequestController", "ProcessCarrierDeliveyForOttoAlerts"))
            {
                var response = await _requestDomain.ProcessCarrierDeliveyForOttoAlerts();
                return response;
            }
        }


        [HttpGet]
        public async Task<List<DeliveryRequestViewModel>> GetBrokeredDrRequestedByMe(int companyId, string regionId, string selectedDate = null)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "GetBrokeredDrRequestedByMe"))
            {
                var response = await _requestDomain.GetBrokeredDrRequestedByMe(companyId, regionId, selectedDate);
                return response;
            }
        }

        [HttpGet]
        public async Task<List<DeliveryRequestViewModel>> GetBrokeredDrRequestedToMe(int companyId, string regionId, string selectedDate = null)
        {
            var response = await _requestDomain.GetBrokeredDrRequestedToMe(companyId, regionId, selectedDate);
            return response;
        }

        [HttpGet]
        public async Task<DeliveryRequestViewModel> GetDeliveryRequestById(string deliveryRequestId)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "GetDeliveryRequestById"))
            {
                var response = await _requestDomain.GetDeliveryRequestById(deliveryRequestId);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> Update(List<DeliveryRequestViewModel> deliveryRequest)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "Update"))
            {
                var response = await _requestDomain.UpdateDeliveryRequest(deliveryRequest);
                return response;
            }
        }

        [HttpGet]
        public List<DeliveryRequestViewModel> GetDeliveryRequestsbyPriority(DeliveryReqPriority priority, int companyId)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "GetDeliveryRequestsbyPriority"))
            {
                var response = _requestDomain.GetDeliveryRequestsbyPriority(priority, companyId);
                return response;
            }
        }

        [HttpGet]
        public List<DemandCaptureChartViewModel> GetDemandCaptureChartdata(string SiteId, int noOfDays, int tfxJobId, int companyId)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "GetDemandCaptureChartdata"))
            {
                var response = _requestDomain.GetDemandCaptureChartData(SiteId, noOfDays, tfxJobId, companyId);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> UpdateDeliveryRequestCompartmentInfo(List<DeliveryRequestCompartmentInfoModel> delReqs)
        {
            var json = JsonConvert.SerializeObject(delReqs);
            using (var tracer = new Tracer("TrueFill.FreightApi::DeliveryRequestController", $"UpdateDeliveryRequestCompartmentInfo(delReqs:{json})"))
            {
                var response = await _requestDomain.UpdateDeliveryRequestCompartmentInfo(delReqs);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> UpdateDeliveryRequestStatus(List<DeliveryReqStatusUpdateModel> delReqs)
        {
            var json = JsonConvert.SerializeObject(delReqs);
            using (var tracer = new Tracer("TrueFill.FreightApi::DeliveryRequestController", $"UpdateDeliveryRequestStatus(delReqs:{json})"))
            {
                var response = await _requestDomain.UpdateDeliveryRequestStatus(delReqs);
                return response;
            }
        }
        [HttpPost]
        public async Task<StatusModel> UpdateDeliveryRequestStatusByTrackableScheduleId(List<DeliveryReqStatusUpdateModel> delReqs)
        {
            var json = JsonConvert.SerializeObject(delReqs);
            using (var tracer = new Tracer("TrueFill.FreightApi::DeliveryRequestController", $"UpdateDeliveryRequestStatusByTrackableScheduleId(delReqs:{json})"))
            {
                var response = await _requestDomain.UpdateDeliveryRequestStatusByTrackableScheduleId(delReqs);
                return response;
            }
        }

        [HttpPost]
        public List<DeliveryRequestDetail> GetDeliveryRequestDetailsByIds(List<string> DrIds)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "GetDeliveryRequestDetailsByIds"))
            {
                var response = _requestDomain.GetDeliveryRequestDetailsByIds(DrIds);
                return response;
            }
        }

        [HttpGet]
        public DeliveryRequestViewModel GetDeliveryRequestDetailsById(string deliveryRequestId)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "GetDeliveryRequestsDetailsById"))
            {
                var response = _requestDomain.GetDeliveryRequestDetailsById(deliveryRequestId);
                return response;
            }
        }

        [HttpGet]
        public List<DipatchersRegionDetails> GetRegionDispactherDetails(int driverId, int companyId, string regionId)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "GetRegionDispactherDetails"))
            {
                var response = _requestDomain.GetRegionDispactherDetails(driverId, companyId, regionId);
                return response;
            }
        }

        [HttpPost]
        public async Task<List<DeliveryRequestViewModel>> CloneDrsForPreload(List<string> drIds)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "CloneDrsForPreload"))
            {
                var response = await _requestDomain.CloneDrsForPreload(drIds);
                return response;
            }
        }
        [HttpGet]
        public List<RecurringDRSchdule> GetRecurringScheduleDetails(int JobId)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "GetRecurringScheduleDetails"))
            {
                var response = _requestDomain.GetRecurringSchedule(JobId);
                return response;
            }
        }
        [HttpGet]
        public Task<StatusModel> DeleteRecurringSchedule(string Id, int userId)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "DeleteRecurringSchedule"))
            {
                var response = _requestDomain.DeleteRecurringSchedule(Id, userId);
                return response;
            }
        }
        [HttpPost]
        public async Task<List<DeliveryRequestViewModel>> CreateDrForRecurringSchedule(List<DeliveryRequestViewModel> deliveryRequest)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "CreateDrForRecurringSchedule"))
            {
                var response = await _requestDomain.CreateDrForRecurringSchedule(deliveryRequest);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> ChangeBrokeredDrStatus(string drId, BrokeredDrCarrierStatus status)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "ProceedBrokeredDrRequest"))
            {
                var response = await _requestDomain.ChangeBrokeredDrStatus(drId, status);
                return response;
            }
        }

        [HttpGet]
        public async Task<ChildDeliveryRequestInfoViewModel> GetChildDeliveryRequestInfo(string drId)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "GetChildDeliveryRequestInfo(drId:" + drId + ")"))
            {
                var response = await _requestDomain.GetChildDeliveryRequestInfo(drId);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> RecallDeliveryRequest(string parentDrId, string childDrId, int tfxUserId)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "RecallDeliveryRequest(parentDrId:" + parentDrId + ", childDrId:" + childDrId + ", tfxUserId:" + tfxUserId + ")"))
            {
                var response = await _requestDomain.RecallDeliveryRequest(parentDrId, childDrId, tfxUserId);
                return response;
            }
        }


        [HttpGet]
        public async Task<List<OttoTripModel>> GetOttoScheduleDetails(int companyId, string regionId, string shiftStartTime, string shiftEndTime, string date)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"GetOttoScheduleDetails"))
            {
                var response = await _requestDomain.GetOttoScheduleDetails(companyId, regionId, shiftStartTime, shiftEndTime, date);
                return response;
            }
        }
        [HttpPost]
        public async Task<DeliveryRequestsViewModel> CreateSplitDeliveryRequests(SplitDeliveryRequestModel splitDeliveryRequest)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "CreateSplitDeliveryRequests"))
            {
                var response = await _requestDomain.CreateSplitDeliveryRequests(splitDeliveryRequest);
                return response;
            }
        }

        [HttpPost]
        public async Task<DeliveryRequestsViewModel> CreateSplitBlendDeliveryRequests(List<SplitDrArray> splitDrArray)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "CreateSplitBlendDeliveryRequests"))
            {
                var response = await _requestDomain.CreateSplitBlendDeliveryRequests(splitDrArray);
                return response;
            }
        }

        [HttpGet]
        public async Task<DRReportFilterViewModel> GetDRReportDropDownFilters(int userId)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "GetDRReportDropDownFilters"))
            {
                var response = await _requestDomain.GetDRReportDropDownFilters(userId);
                return response;
            }
        }

        [HttpPost]
        public async Task<List<DeliveryRequestReportGridViewModel>> GetAllDeliveryRequests(DRReportFilterInputViewModel inputData)
        {
            using (var tracer = new Tracer("DeliveryRequestController", "GetAllDeliveryRequests"))
            {
                var response = await _requestDomain.GetAllDeliveryRequests(inputData);
                return response;
            }
        }
        [HttpPost]
        public async Task<StatusModel> SaveDRCarrierSequence(List<DRCarrierSequenceModel> inputData)
        {
            var response = await _drCarrierDomain.SaveDRCarrierSequence(inputData);
            return response;
        }
        [HttpPost]
        public async Task<StatusModel> UpdateDRCarrierSequence(List<DRCarrierSequenceModel> inputData)
        {
            var response = await _drCarrierDomain.UpdateDRCarrierSequence(inputData);
            return response;
        }
        [HttpGet]
        public async Task<DRCarrierSequenceModel> GetDRCarrierSequenceDetails(string deliveryReqId)
        {
            var response = await _drCarrierDomain.GetDRCarrierSequence(deliveryReqId);
            return response;
        }
        [HttpPost]
        public async Task<StatusModel> UpdateDRCarrierRejectList(DRCarrierRejectInfoModel model)
        {
            var response = await _drCarrierDomain.UpdateDRCarrierRejectList(model);
            return response;
        }
        [HttpGet]
        public async Task<TfxDRAvailableCarrierInfoModel> GetAvailableDRCarrierDetails(string deliveryReqId)
        {
            var response = await _drCarrierDomain.GetAvailableDRCarrierDetails(deliveryReqId);
            return response;
        }
        [HttpPost]
        public async Task<StatusModel> UpdateBrokerDeliveryRequestInfo(BrokeredDeliveryRequestInput model)
        {
            var response = await _requestDomain.UpdateBrokerDeliveryRequestInfo(model);
            return response;
        }
        [HttpPost]
        public async Task<StatusModel> UpdateSpiltDRs(SpiltDeliveryRequestViewModel model)
        {
            var response = await _requestDomain.UpdateSpiltDRs(model);
            return response;
        }
        [HttpPost]
        public async Task<List<CancelDeliverySchedule>> UpdateDeliveryRequestCancelStatus(List<DeliveryReqCancelScheduleUpdateModel> delReqs)
        {
            var response = await _requestDomain.UpdateDeliveryRequestCancelStatus(delReqs);
            return response;

        }
        [HttpPost]
        public async Task<List<CancelDSDeliveryScheduleViewModel>> GetSubDRInfoCancelDS(CancelDSDeliveryScheduleInfo delReqs)
        {

            var response = await _requestDomain.GetSubDRInfoCancelDS(delReqs);
            return response;
        }
        [HttpPost]
        public CarrierXDeliveryRequestInfo GetCompanyDeliveryRequestsDetails(List<int> companyId)
        {

            var response = _requestDomain.GetCompanyDeliveryRequestsDetails(companyId);
            return response;
        }
        [HttpPost]
        public List<DeliveryRequestViewModel> GetDeliveryRequestDetails(List<int> trackableSCId)
        {
            var response = _requestDomain.GetDeliveryRequestDetails(trackableSCId);
            return response;
        }
        [HttpPost]
        public async Task<StatusModel> DeleteDeliveryRequestOnOrderClose(List<int> OrderIds)
        {
            var response = await _requestDomain.DeleteDeliveryRequestOnOrderClose(OrderIds);
            return response;
        }
        [HttpPost]
        public async Task<StatusModel> ValidateDeliveryRequestInUse(List<string> DeliveryReqIds)
        {
            var response = await _requestDomain.ValidateDeliveryRequestInUse(DeliveryReqIds);
            return response;
        }
        [HttpPost]
        public async Task<List<DeliveryRequestViewModel>> GetBlendedGroupDeliveryRequestDetails(List<string> BlendedGroupIds)
        {
            var response = await _requestDomain.GetBlendedGroupDeliveryRequestDetails(BlendedGroupIds);
            return response;
        }
        [HttpGet]
        public async Task<List<ChildDeliveryRequestInfoViewModel>> GetBlendedChildDeliveryRequestInfo(string blendedGroupId)
        {
            var response = await _requestDomain.GetBlendedChildDeliveryRequestInfo(blendedGroupId);
            return response;
        }
        [HttpPost]
        public async Task<StatusModel> DeleteDeliveryRequest(List<string> delReqId)
        {
            return await _requestDomain.DeleteDeliveryRequest(delReqId);
        }


        [HttpPost]
        public async Task<DeliveryReqPriority> GetPriorityForSalesDR(TankDetailsModel tank)
        {
            return await _requestDomain.GetPriorityForSalesDR(tank);
        }

    }
}
