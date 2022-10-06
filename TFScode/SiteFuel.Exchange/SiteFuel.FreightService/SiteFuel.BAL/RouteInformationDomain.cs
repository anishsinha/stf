using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public class RouteInformationDomain : IRouteInformationDomain
    {
        private IRouteInformationRepository _routeRepository;
        public RouteInformationDomain(IRouteInformationRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task<StatusModel> CreateRouteInformation(RouteInformationModel routeInformation)
        {
            var response = new StatusModel();
            try
            {
                response = await _routeRepository.CreateRouteInformation(routeInformation);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.routeInfoCreationFailed;
                LogManager.Logger.WriteException("RouteInformationDomain", "CreateRouteInformation", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> DeleteRouteInformation(string RouteId, string RegionId, int CreatedBy)
        {
            var response = new StatusModel();
            try
            {
                response = await _routeRepository.DeleteRouteInformation(RouteId, RegionId, CreatedBy);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.routeDeletionErrorMsg;
                LogManager.Logger.WriteException("RouteInformationDomain", "DeleteRouteInformation", ex.Message, ex);
            }
            return response;
        }

        public List<FreightModels.DropdownDisplayItem> GetRegionLocationDetails(int companyId, string regionId)
        {
            var response = new List<FreightModels.DropdownDisplayItem>();
            try
            {
                response = _routeRepository.GetRegionLocationDetails(companyId, regionId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RouteInformationDomain", "GetRegionLocationDetails", ex.Message, ex);
            }
            return response;
        }

        public List<RouteInformationModel> GetRouteInformations(int companyId, string regionId)
        {
            var response = new List<RouteInformationModel>();
            try
            {
                response = _routeRepository.GetRouteInformations(companyId, regionId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RouteInformationDomain", "GetRouteInformations", ex.Message, ex);
            }
            return response;
        }
        public List<DropdownDisplayExtended> GetRouteInformations(string regionId)
        {
            var response = new List<DropdownDisplayExtended>();
            try
            {
                response = _routeRepository.GetRouteInformations(regionId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RouteInformationDomain", "GetRouteInformations", ex.Message, ex);
            }
            return response;
        }
        public List<RouteCustomerLocationModel> GetRouteInformations(List<string> regionId)
        {
            var response = new List<RouteCustomerLocationModel>();
            try
            {
                response = _routeRepository.GetRouteInformations(regionId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RouteInformationDomain", "GetRouteInformations", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusModel> UpdateRouteInformation(RouteInformationModel routeInformation)
        {
            var response = new StatusModel();
            try
            {
                response = await _routeRepository.UpdateRouteInformation(routeInformation);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.routeUpdateErrorMsg;
                LogManager.Logger.WriteException("RouteInformationDomain", "UpdateRouteInformation", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusModel> AssignTPOJobToRoute(JobToRegionAssignViewModel jobToUpdate)
        {
            var response = new StatusModel();
            try
            {
                response = await _routeRepository.AssignTPOJobToRoute(jobToUpdate);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.routeUpdateErrorMsg;
                LogManager.Logger.WriteException("RouteInformationDomain", "AssignTPOJobToRoute", ex.Message, ex);
            }
            return response;
        }
        public List<FreightModels.DropdownDisplayItem> GetRouteLocationDetails(string Id, string regionId)
        {
            var response = new List<FreightModels.DropdownDisplayItem>();
            try
            {
                response = _routeRepository.GetRouteLocationDetails(Id, regionId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RouteInformationDomain", "GetRegionLocationDetails-Edit", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusModel> UpdateShiftInfo(RouteInformationModel jobToUpdate)
        {
            var response = new StatusModel();
            try
            {
                response = await _routeRepository.UpdateShiftInfo(jobToUpdate);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.routeShiftUpdateErrorMsg;
                LogManager.Logger.WriteException("RouteInformationDomain", "UpdateShiftInfo", ex.Message, ex);
            }
            return response;
        }
        public List<InvoiceRouteInfo> GetInvoiceRouteInfo(List<string> DelReqId)
        {
            var response = new List<InvoiceRouteInfo>();
            try
            {
                response = _routeRepository.GetInvoiceRouteInfo(DelReqId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RouteInformationDomain", "GetInvoiceRouteInfo", ex.Message, ex);
            }
            return response;
        }
        public async Task<string> GetRouteIdForJob(int jobId, int companyId,string regionId)
        {
            string response = string.Empty;
            try
            {
                response = await _routeRepository.GetRouteIdForJob(jobId, companyId, regionId);
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("RouteInformationDomain", "GetRouteIdForJob", ex.Message, ex);
            }
            return response;
        }
    }
}
