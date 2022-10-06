using MongoDB.Bson;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface ISalesRepository
    {
        Task<LocationTanksResponseModel> GetLocationTanks(LocationTanksRequestModel requestModel);

        Task<SalesDataResponseModel> GetSalesDataAsync(SalesDataRequestModel requestModel);
        Task<SalesGraphRespDataModel> GetSalesGraphDataAsync(int jobId, int noOfDays);
        Task<DeliveryDetailsRespModel> GetExistingSchedulesAsync(int jobId, int productTypeId, int companyId);
        Task<InventoryDataResponseModel> GetInventoryDataForDashboard(InventoryDataViewModel model);
    }
}
