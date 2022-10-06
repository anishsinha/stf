using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFill.DemandCaptureDataAccess.Entities;

namespace SiteFuel.Repository
{
    public interface IDemandCaptureRepository
    {
        Task<int> CreateDemand(List<DemandModel> demandModels, int supplierId , long? fileId = null);
        Task<List<SiteFuel.Exchange.Utilities.DropdownDisplayExtendedItem>> GetSiteList(int companyId, string regionId, string siteId, int sourceTypeId);
        Task<long> SaveDemandFileInfo(string fileName, long uid, DipTestMethod dipTestMethod);
        Task<LongStatusModel> GetLastProcessedUid();
        Task<List<List<DemandCaptureChartViewModel>>> GetDemandCaptureChartDataByTankAndSite(List<int?> assetIds, string siteId, int noOfDays);
        Task<CreateDRTankModel> GetTankDetailsForRegion(int companyId, string regionId, string buyerJobs, int sourceTypeId, bool isCreateDR);
        Task<CreateDRTankModel> GetTankDetailsWithDipTestData(int jobId, int sourceTypeId, int companyId, bool isCreateDR);
        Task<List<CustomerJobForCarrierViewModel>> GetBrokerJobListForCarrier(int companyId, string regionId);
        Task<List<int>> GetBrokerJobOrderDetails(int companyId, List<int> OrderId);
         Task<List<DemandModel>> GetTropicOilTanksDemandModel(List<TropicOilCompanyDemandModel> tropicOilList, List<DropdownDisplayExtendedItem> jobWithTimezone);
        Task<List<DemandModel>> GetIS360TanksDemandModel(List<Is360DemandModel> Is360DemandList, ExternalTankConfigurationModel model);

        Task<bool> ProcessVedorRootTanks();
        Task<List<DemandModel>> GetTropicOilAPITanksDemandModel(List<Inventory> tropicOilList, List<DropdownDisplayExtendedItem> jobWithTimezone);
        Task<bool> IsIS360FileExists(string fileName, DipTestMethod dipTestMethod);
    }
}
