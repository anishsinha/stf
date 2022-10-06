using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.DeliveryRequest;
using SiteFuel.FreightModels.Otto;
using SiteFuel.FreightRepository;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public class DeliveryRequestDomain : IDeliveryRequestDomain
    {
        private IDeliveryRequestRepository _drRepository;
        public DeliveryRequestDomain(IDeliveryRequestRepository regionRepository)
        {
            _drRepository = regionRepository;
        }

        public async Task<DeliveryRequestsViewModel> CreateDeliveryRequest(List<DeliveryRequestViewModel> deliveryRequests)
        {
            var response = new DeliveryRequestsViewModel();
            try
            {
                response.StatusCode = (int)Status.Success;
                deliveryRequests = deliveryRequests.Where(top => !top.isRecurringSchedule).ToList();
                var valResult = ValidateRegionModel(deliveryRequests);
                response.StatusMessage = valResult.Message;
                if (valResult.IsValid)
                {
                    if (deliveryRequests.Count > 0)
                    {
                        response = await _drRepository.CreateDeliveryRequest(deliveryRequests);
                    }
                    else
                    {
                        response.StatusCode = (int)Status.Warning;
                    }
                }
                else
                {
                    response.StatusCode = (int)Status.Failed;
                    response.StatusMessage = valResult.Message;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.msgDelReqCreationFailed;
                LogManager.Logger.WriteException("DeliveryRequestDomain", "CreateDeliveryRequest", ex.Message, ex);
            }
            return response;
        }

        public async Task<DeliveryRequestsViewModel> CreateDeliveryRequestFromBuyerApp(List<DeliveryRequestViewModel> deliveryRequests)
        {
            var response = new DeliveryRequestsViewModel();
            try
            {
                response.StatusCode = (int)Status.Success;
                var normalDeliveryRequests = deliveryRequests.Where(top => !top.isRecurringSchedule).ToList();
                var valResult = ValidateRegionModel(normalDeliveryRequests);
                response.StatusMessage = valResult.Message;
                if (valResult.IsValid)
                {
                    var statusModel = _drRepository.AssignRegionForDelRequest(deliveryRequests);
                    if (statusModel.StatusCode == (int)Status.Success)
                    {
                        if (normalDeliveryRequests.Count > 0)
                        {
                            response = await _drRepository.CreateDeliveryRequest(normalDeliveryRequests);
                        }
                        else
                        {
                            response.StatusCode = (int)Status.Warning;
                        }
                    }
                    else
                    {
                        response.StatusCode = statusModel.StatusCode;
                        response.StatusMessage = statusModel.StatusMessage;
                        response.DeliveryRequests = normalDeliveryRequests;
                    }
                }
                else
                {
                    response.StatusCode = (int)Status.Failed;
                    response.StatusMessage = valResult.Message;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.msgDelReqCreationFailed;
                LogManager.Logger.WriteException("DeliveryRequestDomain", "CreateDeliveryRequestFromBuyerApp", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<TBDRequestDetailModel>> GetTbdDeliveryRequestDetails(List<string> deliveryRequestIds)
        {
            var response = new List<TBDRequestDetailModel>();
            try
            {
                response = await _drRepository.GetTbdDeliveryRequestDetails(deliveryRequestIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetTbdDeliveryRequestDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> ReCreateDeliveryRequest(ReCreateDeliveryRequestsViewModel viewModel)
        {
            var response = new StatusModel();
            try
            {
                var valResult = ValidateRegionModel(viewModel.DeliveryRequests);
                response.StatusMessage = valResult.Message;
                if (valResult.IsValid)
                {
                    response = await _drRepository.ReCreateDeliveryRequestAsync(viewModel);
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.msgDelReqCreationFailed;
                LogManager.Logger.WriteException("DeliveryRequestDomain", "ReCreateDeliveryRequest", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DeliveryRequestViewModel>> GetDeliveryRequests(int companyId, string regionId, string selectedDate = null)
        {
            var response = new List<DeliveryRequestViewModel>();
            try
            {
                response = await _drRepository.GetDeliveryRequests(companyId, regionId, selectedDate);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetDeliveryRequests", ex.Message, ex);
            }
            return response;
        }

        public List<DeliveryRequestViewModel> GetCalendarDeliveryRequests(int companyId, CalendarFilterModel inputModel)
        {
            var response = new List<DeliveryRequestViewModel>();
            try
            {
                response = _drRepository.GetCalendarDeliveryRequests(companyId, inputModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetCalendarDeliveryRequests", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> ProcessCarrierDeliveyForOttoAlerts()
        {
            var response = new StatusModel();
            try
            {
                response = await _drRepository.ProcessCarrierDeliveyForOttoAlerts();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "ProcessCarrierDeliveyForOttoAlerts", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DeliveryRequestViewModel>> GetBrokeredDrRequestedToMe(int companyId, string regionId, string selectedDate = null)
        {
            var response = new List<DeliveryRequestViewModel>();
            try
            {
                response = await _drRepository.GetBrokeredDrRequestedToMe(companyId, regionId, selectedDate);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetAcceptedBrokerDeliveryRequests", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DeliveryRequestViewModel>> GetBrokeredDrRequestedByMe(int companyId, string regionId, string selectedDate = null)
        {
            var response = new List<DeliveryRequestViewModel>();
            try
            {
                response = await _drRepository.GetBrokeredDrRequestedByMe(companyId, regionId, selectedDate);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetBrokeredDrRequestedByMe", ex.Message, ex);
            }
            return response;
        }
        public List<DeliveryRequestViewModel> GetDeliveryRequestsbyPriority(DeliveryReqPriority priority, int companyId)
        {
            var response = new List<DeliveryRequestViewModel>();

            try
            {
                response = _drRepository.GetDeliveryRequestsbyPriority(priority, companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetDeliveryRequestsbyPriority", ex.Message, ex);
            }

            return response;
        }

        public async Task<DeliveryRequestViewModel> GetDeliveryRequestById(string deliveryRequestId)
        {
            var response = new DeliveryRequestViewModel();
            try
            {
                response = await _drRepository.GetDeliveryRequestById(deliveryRequestId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetDeliveryRequestById", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> UpdateDeliveryRequest(List<DeliveryRequestViewModel> deliveryRequest)
        {
            var response = new StatusModel();
            try
            {
                response = await _drRepository.UpdateDeliveryRequest(deliveryRequest);
            }
            catch (Exception ex)
            {
                response.StatusMessage = deliveryRequest.Any(t => t.IsDeleted) ? Resource.msgDelReqDeleteFailed : Resource.msgDelReqEditFailed;
                LogManager.Logger.WriteException("DeliveryRequestDomain", "UpdateDeliveryRequest", ex.Message, ex);
            }
            return response;
        }

        public async Task ScheduleUpdateDeliveryRequest()
        {
            try
            {
                await _drRepository.ScheduleDeliveryRequest();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "ScheduleUpdateDeliveryRequest", ex.Message, ex);
            }
        }

        private ValidatationResult ValidateRegionModel(List<DeliveryRequestViewModel> deliveryRequests)
        {
            var result = new ValidatationResult() { IsValid = true };
            var messages = new List<string>();
            foreach (var model in deliveryRequests)
            {
                if (model.CreatedByCompanyId <= 0)
                    messages.Add("CreatedByCompanyId");

                if (model.CreatedOn == DateTimeOffset.MinValue)
                    messages.Add("CreatedOn");

                if (model.CreatedBy <= 0)
                    messages.Add("CreatedBy");

                if (model.DeliveryRequestFor == DeliveryRequestFor.ProductType)
                {
                    if (model.ProductTypeId <= 0)
                        messages.Add("ProductTypeId");
                }
                else if (model.OrderId == null)
                {
                    if (!model.IsTBD)
                    {
                        if (string.IsNullOrWhiteSpace(model.TankId))
                            messages.Add("TankId");

                        if (string.IsNullOrWhiteSpace(model.StorageId))
                            messages.Add("StorageId");
                    }
                }

                if (model.SupplierCompanyId <= 0)
                    messages.Add("SupplierCompanyId");
            }

            if (messages.Any())
            {
                result.IsValid = false;
                result.Message = "Invalid " + string.Join(", ", messages);
            }
            return result;
        }

        public List<DemandCaptureChartViewModel> GetDemandCaptureChartData(string SiteId, int noOfDays, int tfxJobId, int companyId)
        {
            var response = new List<DemandCaptureChartViewModel>();
            try
            {
                response = _drRepository.GetDemandCaptureChartData(SiteId, noOfDays, tfxJobId, companyId);
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetDemandCaptureChartData", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> UpdateDeliveryRequestCompartmentInfo(List<DeliveryRequestCompartmentInfoModel> delReqs)
        {
            var status = new StatusModel();
            try
            {
                status = await _drRepository.UpdateDeliveryRequestCompartmentInfo(delReqs);
            }
            catch (Exception ex)
            {
                status.StatusCode = (int)Status.Failed;
                status.StatusMessage = string.Format(Resource.ValMsgDelReqStatusUpdateFailedForCompartmentInfo, string.Join(",", delReqs.Select(t => t.DeliveryRequestId)));
                LogManager.Logger.WriteException("DeliveryRequestDomain", "UpdateDeliveryRequestCompartmentInfo", ex.Message, ex);
            }
            return status;
        }

        public async Task<StatusModel> UpdateDeliveryRequestStatus(List<DeliveryReqStatusUpdateModel> delReqs)
        {
            var status = new StatusModel();
            try
            {
                status = await _drRepository.UpdateDeliveryRequestStatus(delReqs);
            }
            catch (Exception ex)
            {
                status.StatusCode = (int)Status.Failed;
                status.StatusMessage = string.Format(Resource.ValMsgDelReqStatusUpdateFailed, string.Join(",", delReqs.Select(t => t.DeliveryRequestId)), string.Join(",", delReqs.Select(t => (EnrouteDeliveryStatus)t.ScheduleStatusId)));
                LogManager.Logger.WriteException("DeliveryRequestDomain", "UpdateDeliveryRequestStatus", ex.Message, ex);
            }
            return status;
        }

        public List<DeliveryRequestDetail> GetDeliveryRequestDetailsByIds(List<string> DrIds)
        {
            var response = new List<DeliveryRequestDetail>();
            try
            {
                response = _drRepository.GetDeliveryRequestDetailsByIds(DrIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetDeliveryRequestDetailsByIds", ex.Message, ex);
            }
            return response;
        }

        public DeliveryRequestViewModel GetDeliveryRequestDetailsById(string deliveryRequestId)
        {
            var response = new DeliveryRequestViewModel();
            try
            {
                response = _drRepository.GetDeliveryRequestDetailsById(deliveryRequestId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetDeliveryRequestDetailsById", ex.Message, ex);
            }
            return response;
        }

        public List<DipatchersRegionDetails> GetRegionDispactherDetails(int driverId, int companyId, string regionId)
        {
            var response = new List<DipatchersRegionDetails>();
            try
            {
                response = _drRepository.GetRegionDispactherDetails(driverId, companyId, regionId);
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetRegionDispactherDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DeliveryRequestViewModel>> CloneDrsForPreload(List<string> drIds)
        {
            var response = new List<DeliveryRequestViewModel>();
            try
            {
                response = await _drRepository.CloneDrsForPreload(drIds);
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "CloneDrsForPreload", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusModel> CreateRecurringSchedules(List<DeliveryRequestViewModel> deliveryRequests)
        {
            var response = new StatusModel();
            try
            {
                response.StatusCode = (int)Status.Success;
                deliveryRequests = deliveryRequests.Where(top => top.isRecurringSchedule && top.RecurringSchdule != null && top.RecurringSchdule.Count > 0).ToList();
                var valResult = ValidateRegionModel(deliveryRequests);
                if (valResult.IsValid)
                {
                    if (deliveryRequests.Count > 0)
                    {
                        response = await _drRepository.CreateRecurringSchedules(deliveryRequests);
                    }
                    else
                    {
                        response.StatusCode = (int)Status.Warning;
                        response.StatusMessage = Resource.msgforEmptyRecordRecurringSchedule;
                    }
                }
                else
                {
                    response.StatusCode = (int)Status.Failed;
                    response.StatusMessage = valResult.Message;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.msgDelReqCreationFailedRecurring;
                LogManager.Logger.WriteException("DeliveryRequestDomain", "CreateRecurringSchedules", ex.Message, ex);
            }
            return response;
        }
        public List<RecurringDRSchdule> GetRecurringSchedule(int JobId)
        {
            var response = new List<RecurringDRSchdule>();
            try
            {
                response = _drRepository.GetRecurringSchedule(JobId);
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetRecurringSchedule", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusModel> DeleteRecurringSchedule(string Id, int userId)
        {
            var status = new StatusModel();
            try
            {
                status = await _drRepository.DeleteRecurringSchedule(Id, userId);
            }
            catch (Exception ex)
            {
                status.StatusCode = (int)Status.Failed;
                status.StatusMessage = Resource.msgDelReqDeleteErrorRecurring;
                LogManager.Logger.WriteException("DeliveryRequestDomain", "DeleteRecurringSchedule", ex.Message, ex);
            }
            return status;
        }
        public async Task<List<DeliveryRequestViewModel>> CreateDrForRecurringSchedule(List<DeliveryRequestViewModel> deliveryRequest)
        {
            var status = new List<DeliveryRequestViewModel>();
            try
            {
                status = await _drRepository.CreateDrForRecurringSchedule(deliveryRequest);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "CreateDrForRecurringSchedule", ex.Message, ex);
            }
            return status;
        }

        public async Task<StatusModel> ChangeBrokeredDrStatus(string drId, BrokeredDrCarrierStatus status)
        {
            var response = new StatusModel();
            try
            {
                var drIds = new List<string>() { drId };
                var parentId = await _drRepository.GetParentDeliveryRequestId(drId);
                drIds.Add(parentId);
                response = await _drRepository.ChangeBrokeredDrStatus(drId, parentId, status);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "ChangeBrokeredDrStatus", ex.Message, ex);
            }
            return response;
        }

        public async Task<ChildDeliveryRequestInfoViewModel> GetChildDeliveryRequestInfo(string drId)
        {
            var response = new ChildDeliveryRequestInfoViewModel();
            try
            {
                response = await _drRepository.GetChildDeliveryRequestInfo(drId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetChildDeliveryRequestInfo", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> RecallDeliveryRequest(string parentDrId, string childDrId, int tfxUserId)
        {
            var response = new StatusModel();
            try
            {
                if (string.IsNullOrWhiteSpace(parentDrId))
                {
                    throw new ArgumentNullException("parentDrId is null or empty");
                }
                if (string.IsNullOrWhiteSpace(childDrId))
                {
                    throw new ArgumentNullException("childDrId is null or empty");
                }
                response = await _drRepository.RecallDeliveryRequest(parentDrId, childDrId, tfxUserId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "RecallDeliveryRequest", ex.Message, ex);
            }
            return response;
        }
        public async Task ScheduleOttoDeliveryRequest()
        {
            try
            {
                await _drRepository.ScheduleOttoDeliveryRequest();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "ScheduleOttoDeliveryRequest", ex.Message, ex);
            }
        }

        public async Task<List<OttoTripModel>> GetOttoScheduleDetails(int companyId, string regionId, string shiftStartTime, string shiftEndTime, string date)
        {
            var response = new List<OttoTripModel>();
            try
            {
                bool bStartDate = DateTime.TryParse(date + " " + shiftStartTime, out DateTime startDate);
                bool bEndDate = DateTime.TryParse(date + " " + shiftEndTime, out DateTime endDate);
                if (bStartDate && bEndDate)
                {
                    if (endDate < startDate)
                    {
                        endDate = endDate.AddDays(1);
                    }
                    var AllDRs = await _drRepository.GetOttoDeliveryRequests(companyId, regionId, startDate, endDate);
                    var slotPeriod = await _drRepository.GetShiftSlotPeriod(regionId);
                    slotPeriod = slotPeriod <= 0 ? 4 : slotPeriod;

                    while (startDate < endDate)
                    {
                        var loadEndDate = startDate.AddHours(slotPeriod);
                        var ottoTrip = new OttoTripModel();
                        ottoTrip.StartTime = startDate.ToString(Resource.constFormat12HourTime);
                        ottoTrip.EndTime = loadEndDate.ToString(Resource.constFormat12HourTime);
                        ottoTrip.DeliveryRequests = AllDRs.Where(t => t.DeliveryWindowInfo != null
                                                    && t.DeliveryWindowInfo.StartDate.Add(DateTime.Parse(t.DeliveryWindowInfo.StartTime).TimeOfDay) >= startDate
                                                    && t.DeliveryWindowInfo.StartDate.Add(DateTime.Parse(t.DeliveryWindowInfo.StartTime).TimeOfDay) <= loadEndDate)
                                                    .OrderBy(t => t.JobName).ThenBy(t => t.ProductType).ToList();
                        response.Add(ottoTrip);
                        startDate = startDate.AddHours(slotPeriod);
                    }
                }
                else
                {
                    LogManager.Logger.WriteException("DeliveryRequestDomain", "GetOttoScheduleDetails", $"Invalid dateTime passed: date:{date}, shiftStartTime:{shiftStartTime}, shiftEndTime:{shiftEndTime}", new ArgumentException("Invalid dateTime passed"));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetOttoScheduleDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<DeliveryRequestsViewModel> CreateSplitDeliveryRequests(SplitDeliveryRequestModel splitDeliveryRequest)
        {
            var response = new DeliveryRequestsViewModel();
            try
            {
                response = await _drRepository.CreateSplitDeliveryRequests(splitDeliveryRequest);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.msgDelReqCreationFailed;
                LogManager.Logger.WriteException("DeliveryRequestDomain", "CreateSplitDeliveryRequests", ex.Message, ex);
            }
            return response;
        }

        public async Task<DeliveryRequestsViewModel> CreateSplitBlendDeliveryRequests(List<SplitDrArray> splitDrArray)
        {
            var response = new DeliveryRequestsViewModel();
            try
            {
                response = await _drRepository.CreateSplitBlendDeliveryRequests(splitDrArray);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.msgDelReqCreationFailed;
                LogManager.Logger.WriteException("DeliveryRequestDomain", "CreateSplitBlendDeliveryRequests", ex.Message, ex);
            }
            return response;
        }

        public async Task<DRReportFilterViewModel> GetDRReportDropDownFilters(int userId)
        {
            var response = new DRReportFilterViewModel();
            try
            {
                response = await _drRepository.GetDRReportDropDownFilters(userId);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetDRReportDropDownFilters", ex.Message, ex);
            }
            return response;

        }

        public async Task<List<DeliveryRequestReportGridViewModel>> GetAllDeliveryRequests(DRReportFilterInputViewModel inputData)
        {
            var response = new List<DeliveryRequestReportGridViewModel>();
            try
            {
                response = await _drRepository.GetAllDeliveryRequests(inputData);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetAllDeliveryRequests", ex.Message, ex);
            }
            return response;

        }
        public async Task<StatusModel> UpdateBrokerDeliveryRequestInfo(BrokeredDeliveryRequestInput model)
        {
            var response = new StatusModel();
            try
            {
                response = await _drRepository.UpdateBrokerDeliveryRequestInfo(model);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "UpdateBrokerDeliveryRequestInfo", ex.Message, ex);
            }
            return response;

        }

        public async Task<StatusModel> UpdateSpiltDRs(SpiltDeliveryRequestViewModel model)
        {
            var response = new StatusModel();
            try
            {
                response = await _drRepository.UpdateSpiltDRs(model);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "UpdateSpiltDRs", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<CancelDeliverySchedule>> UpdateDeliveryRequestCancelStatus(List<DeliveryReqCancelScheduleUpdateModel> delReqs)
        {
            var status = new List<CancelDeliverySchedule>();
            try
            {
                status = await _drRepository.UpdateDeliveryRequestCancelStatus(delReqs);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "UpdateDeliveryRequestStatus", ex.Message, ex);
            }
            return status;
        }

        public async Task<List<CancelDSDeliveryScheduleViewModel>> GetSubDRInfoCancelDS(CancelDSDeliveryScheduleInfo delReqs)
        {
            var response = new List<CancelDSDeliveryScheduleViewModel>();
            try
            {
                response = await _drRepository.GetSubDRInfoCancelDS(delReqs);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetSubDRInfoCancelDS", ex.Message, ex);
            }
            return response;
        }

        public CarrierXDeliveryRequestInfo GetCompanyDeliveryRequestsDetails(List<int> companyId)
        {
            var response = new CarrierXDeliveryRequestInfo();
            try
            {
                response = _drRepository.GetCompanyDeliveryRequestsDetails(companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetCompanyDeliveryRequestsDetails", ex.Message, ex);
            }
            return response;

        }
        public List<DeliveryRequestViewModel> GetDeliveryRequestDetails(List<int> trackableSCId)
        {
            var response = new List<DeliveryRequestViewModel>();
            try
            {
                response = _drRepository.GetDeliveryRequestDetails(trackableSCId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetDeliveryRequestDetails", ex.Message, ex);
            }
            return response;

        }
        public async Task<StatusModel> UpdateDeliveryRequestStatusByTrackableScheduleId(List<DeliveryReqStatusUpdateModel> delReqs)
        {
            var response = new StatusModel();
            try
            {
                response = await _drRepository.UpdateDeliveryRequestStatusByTrackableScheduleId(delReqs);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "UpdateDeliveryRequestStatusByTrackableScheduleId", ex.Message, ex);
            }
            return response;

        }
        public async Task<StatusModel> DeleteDeliveryRequestOnOrderClose(List<int> OrderIds)
        {
            var response = new StatusModel();
            try
            {
                response = await _drRepository.DeleteDeliveryRequestOnOrderClose(OrderIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "DeleteDeliveryRequestOnOrderClose", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusModel> ValidateDeliveryRequestInUse(List<string> DeliveryReqIds)
        {
            var response = new StatusModel();
            try
            {
                response = await _drRepository.ValidateDeliveryRequestInUse(DeliveryReqIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "ValidateDeliveryRequestInUse", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DeliveryRequestViewModel>> GetBlendedGroupDeliveryRequestDetails(List<string> BlendedGroupIds)
        {
            var response = new List<DeliveryRequestViewModel>();
            try
            {
                response = await _drRepository.GetBlendedGroupDeliveryRequestDetails(BlendedGroupIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetBlendedGroupDeliveryRequestDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ChildDeliveryRequestInfoViewModel>> GetBlendedChildDeliveryRequestInfo(string blendedGroupId)
        {
            var response = new List<ChildDeliveryRequestInfoViewModel>();
            try
            {
                response = await _drRepository.GetBlendedChildDeliveryRequestInfo(blendedGroupId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetBlendedChildDeliveryRequestInfo", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> DeleteDeliveryRequest(List<string> delReqId)
        {
            var response = new StatusModel();
            try
            {
                response = await _drRepository.DeleteDeliveryRequest(delReqId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "DeleteDeliveryRequest", ex.Message, ex);
            }
            return response;
        }
        public async Task<bool> MoveCalendarDeliveryRequest()
        {
            var response = false;
            try
            {
                response = await _drRepository.MoveCalendarDeliveryRequest();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "SaveUnscheduledDrsToMissed", ex.Message, ex);
            }
            return response;
        }

        public async Task<DeliveryReqPriority> GetPriorityForSalesDR(TankDetailsModel tank)
        {
            var response = DeliveryReqPriority.None;
            try
            {
                response = await _drRepository.GetPriorityForSalesDR(tank);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestDomain", "GetPriorityForSalesDR", ex.Message, ex);
            }
            return response;
        }
    }
}
