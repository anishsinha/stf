using SiteFuel.FreightModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface ITrailerFuelRetainDomain
    {
        Task<StatusModel> SaveTrailerFuelRetain(List<TrailerFuelRetainViewModel> model);
        Task<List<TrailerFuelRetainViewModel>> GetTrailerFuelRetainDetails(string trailerId);
        Task<StatusModel> ResetTrailerFuelRetained(TruckDetailViewModel truckDetailViewModel);
        Task<StatusModel> UpdateTrailerFuelRetained(TruckDetailViewModel truckDetailViewModel);
        Task<StatusModel> ConfirmTrailerFuelRetainedException(TruckDetailViewModel truckDetailViewModel);
    }
}
