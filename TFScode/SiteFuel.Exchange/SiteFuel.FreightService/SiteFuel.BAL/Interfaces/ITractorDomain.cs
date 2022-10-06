using SiteFuel.FreightModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface ITractorDomain
    {
        Task<StatusModel> SaveTractorDetails(TractorDetailViewModel model);

        Task<StatusModel> UpdateTractorDetails(TractorDetailViewModel model);

        Task<List<TractorDetailViewModel>> GetAllTractorDetails(int companyId);

        Task<StatusModel> DeleteTractor(TractorDetailViewModel requestModel);
        Task<List<Exchange.Utilities.DropdownDisplayItem>> GetAllDrivers(int companyId, string trailerTypeId);

        
    }
}
