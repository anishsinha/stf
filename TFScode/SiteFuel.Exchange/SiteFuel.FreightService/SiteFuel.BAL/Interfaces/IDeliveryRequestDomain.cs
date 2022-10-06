using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.DeliveryRequest;
using SiteFuel.FreightModels.Otto;
using SiteFuel.MdbDataAccess.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface IDeliveryRequestDomain
    {
        Task<DeliveryRequestsViewModel> CreateDeliveryRequest(List<DeliveryRequestViewModel> deliveryRequests);        
        Task<StatusModel> ReCreateDeliveryRequest(ReCreateDeliveryRequestsViewModel viewModel);
        Task<List<TBDRequestDetailModel>> GetTbdDeliveryRequestDetails(List<string> deliveryRequestIds);
        Task<StatusModel> UpdateDeliveryRequest(List<DeliveryRequestViewModel> deliveryRequest);
        List<DeliveryRequestViewModel> GetCalendarDeliveryRequests(int companyId, CalendarFilterModel inputModel);
        Task<List<DeliveryRequestViewModel>> GetDeliveryRequests(int companyId, string regionId, string selectedDate);
        Task<List<DeliveryRequestViewModel>> GetBrokeredDrRequestedToMe(int companyId, string regionId, string selectedDate);
        Task<List<DeliveryRequestViewModel>> GetBrokeredDrRequestedByMe(int companyId, string regionId, string selectedDate);
        Task<DeliveryRequestViewModel> GetDeliveryRequestById(string deliveryRequestId);
        List<DeliveryRequestViewModel> GetDeliveryRequestsbyPriority(DeliveryReqPriority priority, int companyId);
        List<DemandCaptureChartViewModel> GetDemandCaptureChartData(string SiteId, int noOfDays, int tfxJobId, int companyId);
        Task<StatusModel> UpdateDeliveryRequestStatus(List<DeliveryReqStatusUpdateModel> delReqs);
        Task<StatusModel> UpdateDeliveryRequestCompartmentInfo(List<DeliveryRequestCompartmentInfoModel> delReqs);
        List<DeliveryRequestDetail> GetDeliveryRequestDetailsByIds(List<string> DrIds);
        DeliveryRequestViewModel GetDeliveryRequestDetailsById(string deliveryRequestId);
        List<DipatchersRegionDetails> GetRegionDispactherDetails(int driverId, int companyId, string regionId);
        Task<List<DeliveryRequestViewModel>> CloneDrsForPreload(List<string> drIds);
        Task<StatusModel> CreateRecurringSchedules(List<DeliveryRequestViewModel> deliveryRequests);
        List<RecurringDRSchdule> GetRecurringSchedule(int JobId);
        Task<StatusModel> DeleteRecurringSchedule(string Id, int userId);
        Task<List<DeliveryRequestViewModel>> CreateDrForRecurringSchedule(List<DeliveryRequestViewModel> deliveryRequest);
        Task<StatusModel> ChangeBrokeredDrStatus(string drId, BrokeredDrCarrierStatus status);
        Task<ChildDeliveryRequestInfoViewModel> GetChildDeliveryRequestInfo(string drId);
        Task<StatusModel> RecallDeliveryRequest(string parentDrId, string childDrId, int tfxUserId);
        Task<DeliveryRequestsViewModel> CreateDeliveryRequestFromBuyerApp(List<DeliveryRequestViewModel> deliveryRequests);
        Task<List<OttoTripModel>> GetOttoScheduleDetails(int companyId, string regionId, string shiftStartTime, string shiftEndTime, string date);
        Task<DeliveryRequestsViewModel> CreateSplitDeliveryRequests(SplitDeliveryRequestModel splitDeliveryRequest);
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
        Task<DeliveryReqPriority> GetPriorityForSalesDR(TankDetailsModel tank);
    }
}
