using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface ISalesDomain
    {
        Task<SalesDataResponseModel> GetSalesData(SalesDataRequestModel requestModel);
        Task<LocationTanksResponseModel> GetLocationTanks(LocationTanksRequestModel requestModel);
        Task<SalesGraphRespDataModel> GetSalesGraphData(int jobId, int noOfDays);
        Task<DeliveryDetailsRespModel> GetExistingSchedules(int jobId, int productTypeId, int companyId);
        Task<InventoryDataResponseModel> GetInventoryDataForDashboard(InventoryDataViewModel model);
    }
}
