using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.BuyerDipTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface IDemandCaptureDomain
    {
        Task<StatusModel> CreateDemand(List<DemandModel> demandList, int demandCount, int supplierId);
        Task<StatusModel> CreateTankDipTest(List<DemandModel> dipTestList, int supplierId);
        DemandModel ProcessData(int sourceTypeId, string lineText, int lineCount, ref string error);
        Task<CreateDRTankModel> GetDemands(int companyId, int? jobId, string regionId, string buyerJobs, int sourceTypeId, bool isCreateDR);
        Task<List<SiteFuel.Exchange.Utilities.DropdownDisplayExtendedItem>> GetSiteList(int companyId, string regionId, string siteId, int sourceTypeId);
        Task<List<CustomerJobForCarrierViewModel>> GetJobListForCarrier(int companyId, string regionId, string siteId, int sourceTypeId);
        Task<List<List<DemandCaptureChartViewModel>>> GetDemandCaptureChartDataByTankAndSite(List<int?> assetIds, string siteId, int noOfDays);
         Task<bool> ProcessPedigreeData(PedegreeConfigurationModel model);
        Task<List<CustomerJobForCarrierViewModel>> GetBrokerJobListForCarrier(int companyId, string regionId);
        Task<List<int>> GetBrokerJobOrderDetails(int companyId, List<int> OrderId);
        Task<bool> ProcessSkybitzData(List<DropdownDisplayExtendedItem> model, List<FTPConfig> config);
        Task<bool> ProcessIS360(ExternalTankConfigurationModel model);
        Task<bool> InvokeSkyBitzService(List<DropdownDisplayExtendedItem> jobWithTimezone, List<FTPConfig> config);
    }
}
