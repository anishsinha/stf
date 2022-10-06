using SiteFuel.FreightModels;
using SiteFuel.FreightRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public class TractorDomain : ITractorDomain
    {
        readonly ITractorRepository _tractorRepository;

        public TractorDomain(ITractorRepository tractorRepository)
        {
            _tractorRepository = tractorRepository;
        }

        public async Task<StatusModel> SaveTractorDetails(TractorDetailViewModel model)
        {
            var result = await _tractorRepository.SaveTractorDetails(model);
            return result;
        }
        public async Task<StatusModel> UpdateTractorDetails(TractorDetailViewModel model)
        {
            var result = await _tractorRepository.UpdateTractorDetails(model);
            return result;
        }
        public async Task<List<TractorDetailViewModel>> GetAllTractorDetails(int companyId)
        {
            var result = await _tractorRepository.GetAllTractorDetails(companyId);
            return result;
        }

        public async Task<StatusModel> DeleteTractor(TractorDetailViewModel requestModel)
        {
            var result = await _tractorRepository.DeleteTractor(requestModel);
            return result;
        }
        public async Task<List<Exchange.Utilities.DropdownDisplayItem>> GetAllDrivers(int companyId, string trailerTypeId)
        {
            var result = await _tractorRepository.GetAllDrivers(companyId, trailerTypeId);
            return result;
        }

       
    }
}
