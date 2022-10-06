using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public class SalesDomain : ISalesDomain
    {
        private ISalesRepository _salesRepository;
        public SalesDomain(ISalesRepository salesRepository)
        {
            _salesRepository = salesRepository;
        }

        public async Task<SalesDataResponseModel> GetSalesData(SalesDataRequestModel requestModel)
        {
            var response = new SalesDataResponseModel();
            try
            {
                response = await _salesRepository.GetSalesDataAsync(requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetSalesData", ex.Message, ex);
            }
            return response;
        }

        public async Task<LocationTanksResponseModel> GetLocationTanks(LocationTanksRequestModel requestModel)
        {
            var response = new LocationTanksResponseModel();
            try
            {
                response = await _salesRepository.GetLocationTanks(requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetLocationTanks", ex.Message, ex);
            }
            return response;
        }

        public async Task<SalesGraphRespDataModel> GetSalesGraphData(int jobId, int noOfDays)
        {
            var response = new SalesGraphRespDataModel();
            try
            {
                response = await _salesRepository.GetSalesGraphDataAsync(jobId, noOfDays);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetSalesGraphData", ex.Message, ex);
            }
            return response;
        }
        public async Task<DeliveryDetailsRespModel> GetExistingSchedules(int jobId, int productTypeId, int companyId)
        {
            var response = new DeliveryDetailsRespModel();
            try
            {
                response = await _salesRepository.GetExistingSchedulesAsync(jobId, productTypeId,companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetExistingSchedules", ex.Message, ex);
            }
            return response;
        }

        public async Task<InventoryDataResponseModel> GetInventoryDataForDashboard(InventoryDataViewModel model)
        {
            var response = new InventoryDataResponseModel();
            try
            {
                response = await _salesRepository.GetInventoryDataForDashboard(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetInventoryDataForDashboard", ex.Message, ex);
            }
            return response;
        }
    }
}
