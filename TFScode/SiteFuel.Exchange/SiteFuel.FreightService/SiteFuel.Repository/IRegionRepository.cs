using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface IRegionRepository
    {
        Task<bool> DeleteAllRegions();
        Task<RegionCreateResponseModel> CreateRegion(RegionViewModel model);
        Task<RegionCreateResponseModel> UpdateRegion(RegionViewModel model);
        Task<RegionViewModel> GetRegion(int companyId, string regionId);
        Task<List<RegionViewModel>> GetRegions(int companyId);
    }
}
