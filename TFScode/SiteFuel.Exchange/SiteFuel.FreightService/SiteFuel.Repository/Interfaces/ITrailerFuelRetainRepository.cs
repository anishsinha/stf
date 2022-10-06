using SiteFuel.FreightModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface ITrailerFuelRetainRepository
    {
        Task<StatusModel> SaveTrailerFuelRetain(List<TrailerFuelRetainViewModel> model);
        Task<List<TrailerFuelRetainViewModel>> GetTrailerFuelRetainDetails(string trailerId);
        Task<StatusModel> ResetTrailerFuelRetained(TruckDetailViewModel truckDetailViewModel);
        Task<StatusModel> UpdateTrailerFuelRetainDetails(TruckDetailViewModel truckDetailViewModel);     
        Task<StatusModel> ConfirmTrailerFuelRetainException(TruckDetailViewModel truckDetailViewModel);
    }
}
