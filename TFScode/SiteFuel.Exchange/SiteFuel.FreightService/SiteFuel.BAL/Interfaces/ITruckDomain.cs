using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface ITruckDomain
    {
        Task<StatusModel> SaveTruckDetails(TruckDetailViewModel model);

        Task<StatusModel> UpdateTruckDetails(TruckDetailViewModel model);

        Task<List<TruckDetailViewModel>> GetAllTruckDetails(int companyId);
        Task<DropdownDisplayExtended> GetTruckRegionDetails(string truckId);
        List<ExternalVehicleMappingViewModel> GetVehiclesForExternalMapping(int companyId);
        Task<StatusModel> SaveExternalVehicleMapping(ExternalVehicleMappingViewModel viewModel);
        Task<StatusModel> SaveBulkUploadVehicleMapping(int userId, List<ExternalVehicleMappingViewModel> listExternalVehicles);
        Task<StatusModel> DeleteTruck(TruckDetailViewModel requestModel);        
        Task<TruckDetailViewModel> GetTruckDetails(string truckId);
        Task<List<TruckDetailViewModel>> GetAllTruckRetainFuelDetails(int companyId);
    }
}
