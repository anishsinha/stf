using System.Collections.Generic;
using System.Threading.Tasks;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository;

namespace SiteFuel.BAL
{
    public class TrailerFuelRetainDomain : ITrailerFuelRetainDomain
    {
        readonly ITrailerFuelRetainRepository _trailerFuelRetainRepository;
        public TrailerFuelRetainDomain(ITrailerFuelRetainRepository trailerFuelRetainRepository)
        {
            _trailerFuelRetainRepository = trailerFuelRetainRepository;
        }
        public async Task<List<TrailerFuelRetainViewModel>> GetTrailerFuelRetainDetails(string trailerId)
        {
            var result = await _trailerFuelRetainRepository.GetTrailerFuelRetainDetails(trailerId);
            return result;
        }

        public async Task<StatusModel> SaveTrailerFuelRetain(List<TrailerFuelRetainViewModel> model)
        {
            var result = await _trailerFuelRetainRepository.SaveTrailerFuelRetain(model);
            return result;
        }

        public async Task<StatusModel> ResetTrailerFuelRetained(TruckDetailViewModel truckDetailViewModel)
        {
            var result = await _trailerFuelRetainRepository.ResetTrailerFuelRetained(truckDetailViewModel);
            return result;
        }
        public async Task<StatusModel> UpdateTrailerFuelRetained(TruckDetailViewModel truckDetailViewModel)
        {
            var result = await _trailerFuelRetainRepository.UpdateTrailerFuelRetainDetails(truckDetailViewModel);
            return result;
        }

        public async Task<StatusModel> ConfirmTrailerFuelRetainedException(TruckDetailViewModel truckDetailViewModel)
        {
            var result = await _trailerFuelRetainRepository.ConfirmTrailerFuelRetainException(truckDetailViewModel);
            return result;
        }
    }
}
