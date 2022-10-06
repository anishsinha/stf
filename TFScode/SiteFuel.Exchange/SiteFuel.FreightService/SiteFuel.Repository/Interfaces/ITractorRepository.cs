using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface ITractorRepository
    {
        Task<StatusModel> SaveTractorDetails(TractorDetailViewModel model);

        Task<StatusModel> UpdateTractorDetails(TractorDetailViewModel model);

        Task<List<TractorDetailViewModel>> GetAllTractorDetails(int companyId);

        Task<StatusModel> DeleteTractor(TractorDetailViewModel requestModel);

        Task<List<Exchange.Utilities.DropdownDisplayItem>> GetAllDrivers(int companyId, string trailerTypeId);
    }
}
