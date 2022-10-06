using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface IRegionDomain
    {
        Task<bool> DeleteAllRegions();
        Task<RegionCreateResponseModel> CreateRegion(RegionViewModel model);
        Task<StatusModel> UpdateRegion(RegionViewModel model);
        Task<RegionViewModel> GetRegion(int companyId, string regionId);
        Task<List<RegionViewModel>> GetRegions(int companyId);
    }
}
