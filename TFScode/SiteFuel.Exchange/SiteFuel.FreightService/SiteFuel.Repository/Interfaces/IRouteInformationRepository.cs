using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface IRouteInformationRepository
    {
        Task<StatusModel> CreateRouteInformation(RouteInformationModel routeInformations);
        Task<StatusModel> UpdateRouteInformation(RouteInformationModel routeInformations);
        Task<StatusModel> DeleteRouteInformation(string RouteId, string RegionId, int CreatedBy);
        List<RouteInformationModel> GetRouteInformations(int companyId, string regionId);
        List<DropdownDisplayExtended> GetRouteInformations(string regionId);
        List<RouteCustomerLocationModel> GetRouteInformations(List<string> regionId);
        List<FreightModels.DropdownDisplayItem> GetRegionLocationDetails(int companyId, string regionId);
        Task<StatusModel> AssignTPOJobToRoute(JobToRegionAssignViewModel jobToUpdate);
        List<FreightModels.DropdownDisplayItem> GetRouteLocationDetails(string Id, string regionId);
        Task<StatusModel> UpdateShiftInfo(RouteInformationModel jobToUpdate);
        List<InvoiceRouteInfo> GetInvoiceRouteInfo(List<string> deliveryReqId);
        Task<string> GetRouteIdForJob(int jobId, int companyId, string regionId);
    }
}
