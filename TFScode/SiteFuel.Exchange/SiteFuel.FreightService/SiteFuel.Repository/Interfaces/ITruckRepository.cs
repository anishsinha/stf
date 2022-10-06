using MongoDB.Bson;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface ITruckRepository
    {
        Task<StatusModel> SaveTruckDetails(TruckDetailViewModel model);

        Task<StatusModel> UpdateTruckDetails(TruckDetailViewModel model);
        Task<List<TruckDetailViewModel>> GetAllTruckDetails(int companyId);
        List<ExternalVehicleMappingViewModel> GetVehiclesForExternalMapping(int companyId);
        Task<StatusModel> SaveExternalVehicleMapping(ExternalVehicleMappingViewModel viewModel);
        Task<StatusModel> SaveBulkUploadVehicleMapping(int userId, List<ExternalVehicleMappingViewModel> listExternalVehicles);
        Task<TruckDetailViewModel> GetTruckDetails(string truckId);
        Task<DropdownDisplayExtended> GetTruckRegionDetails(string truckId);
        Task<StatusModel> DeleteTruckAsync(TruckDetailViewModel requestModel);
        Task<List<TruckDetailViewModel>> GetAllTruckRetainFuelDetails(int companyId);

    }
}
