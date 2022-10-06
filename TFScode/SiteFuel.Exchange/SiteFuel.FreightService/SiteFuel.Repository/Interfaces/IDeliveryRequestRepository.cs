using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.DeliveryRequest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface IDeliveryRequestRepository
    {
        Task<DeliveryRequestsViewModel> CreateDeliveryRequest(List<DeliveryRequestViewModel> model);
        Task<StatusModel> ReCreateDeliveryRequestAsync(ReCreateDeliveryRequestsViewModel model);
        Task<List<TBDRequestDetailModel>> GetTbdDeliveryRequestDetails(List<string> deliveryRequestIds);
        Task<StatusModel> UpdateDeliveryRequest(List<DeliveryRequestViewModel> model);
        Task<DeliveryRequestViewModel> GetDeliveryRequestById(string deliveryReqId);
        List<DeliveryRequestViewModel> GetCalendarDeliveryRequests(int companyId, CalendarFilterModel inputModel);
        Task<List<DeliveryRequestViewModel>> GetDeliveryRequests(int companyId, string regionId,string selectedDate);
        Task<List<DeliveryRequestViewModel>> GetBrokeredDrRequestedToMe(int companyId, string regionId, string selectedDate);
        Task<List<DeliveryRequestViewModel>> GetBrokeredDrRequestedByMe(int companyId, string regionId,string selectedDate);
        List<DeliveryRequestViewModel> GetDeliveryRequestsbyPriority(DeliveryReqPriority priority, int companyId);
        List<DemandCaptureChartViewModel> GetDemandCaptureChartData(string SiteId, int noOfDays, int tfxJobId, int companyId);
        Task ScheduleDeliveryRequest();
        Task<StatusModel> UpdateDeliveryRequestStatus(List<DeliveryReqStatusUpdateModel> delReqs);
        Task<StatusModel> UpdateDeliveryRequestCompartmentInfo(List<DeliveryRequestCompartmentInfoModel> delReqs);
        List<DeliveryRequestDetail> GetDeliveryRequestDetailsByIds(List<string> DrIds);
        DeliveryRequestViewModel GetDeliveryRequestDetailsById(string deliveryRequestId);
        List<DipatchersRegionDetails> GetRegionDispactherDetails(int driverId, int companyId, string regionId);
        Task<List<DeliveryRequestViewModel>> CloneDrsForPreload(List<string> drIds);
        Task<StatusModel> CreateRecurringSchedules(List<DeliveryRequestViewModel> model);
        List<RecurringDRSchdule> GetRecurringSchedule(int JobId);
        Task<StatusModel> DeleteRecurringSchedule(string Id, int UserId);
        Task<List<DeliveryRequestViewModel>> CreateDrForRecurringSchedule(List<DeliveryRequestViewModel> deliveryRequest);
        Task<StatusModel> ChangeBrokeredDrStatus(string ChildDrId,string ParentDrId, BrokeredDrCarrierStatus status);
        Task<string> GetParentDeliveryRequestId(string childDrId);
        Task<ChildDeliveryRequestInfoViewModel> GetChildDeliveryRequestInfo(string drId);
        Task<StatusModel> RecallDeliveryRequest(string parentDrId, string childDrId, int tfxUserId);
        StatusModel AssignRegionForDelRequest(List<DeliveryRequestViewModel> model);
        Task ScheduleOttoDeliveryRequest();
        Task<List<DeliveryRequestViewModel>> GetOttoDeliveryRequests(int companyId, string regionId, DateTime startDate, DateTime endDate);
        Task<int> GetShiftSlotPeriod(string regionId);
        Task<DeliveryRequestsViewModel> CreateSplitDeliveryRequests(SplitDeliveryRequestModel model);
        Task<DeliveryRequestsViewModel> CreateSplitBlendDeliveryRequests(List<SplitDrArray> splitDrArray);
        Task<StatusModel> ProcessCarrierDeliveyForOttoAlerts();

        Task<DRReportFilterViewModel> GetDRReportDropDownFilters(int userId);
        Task<List<DeliveryRequestReportGridViewModel>> GetAllDeliveryRequests(DRReportFilterInputViewModel inputData);
        Task<StatusModel> UpdateBrokerDeliveryRequestInfo(BrokeredDeliveryRequestInput model);
        Task<StatusModel> UpdateSpiltDRs(SpiltDeliveryRequestViewModel model);
        Task<List<CancelDeliverySchedule>> UpdateDeliveryRequestCancelStatus(List<DeliveryReqCancelScheduleUpdateModel> delReqs);
        Task<List<CancelDSDeliveryScheduleViewModel>> GetSubDRInfoCancelDS(CancelDSDeliveryScheduleInfo delReqs);
        CarrierXDeliveryRequestInfo GetCompanyDeliveryRequestsDetails(List<int> companyId);
        List<DeliveryRequestViewModel> GetDeliveryRequestDetails(List<int> trackableSCId);
         Task<StatusModel> UpdateDeliveryRequestStatusByTrackableScheduleId(List<DeliveryReqStatusUpdateModel> delReqs);
        Task<StatusModel> DeleteDeliveryRequestOnOrderClose(List<int> OrderIds);
        Task<StatusModel> ValidateDeliveryRequestInUse(List<string> DeliveryReqIds);
        Task<List<DeliveryRequestViewModel>> GetBlendedGroupDeliveryRequestDetails(List<string> BlendedGroupIds);
        Task<List<ChildDeliveryRequestInfoViewModel>> GetBlendedChildDeliveryRequestInfo(string blendedGroupId);
        Task<StatusModel> DeleteDeliveryRequest(List<string> delReqId);
        Task<bool> MoveCalendarDeliveryRequest();
        Task<DeliveryReqPriority> GetPriorityForSalesDR(TankDetailsModel tank);
    }
}
