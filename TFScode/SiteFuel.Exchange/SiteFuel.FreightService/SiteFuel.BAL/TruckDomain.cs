using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public class TruckDomain : ITruckDomain
    {
        readonly ITruckRepository _truckRepository;
        public TruckDomain(ITruckRepository TruckRepository)
        {
            _truckRepository = TruckRepository;
        }

        public async Task<StatusModel> SaveTruckDetails(TruckDetailViewModel model)
        {
            var result = new StatusModel();
            try
            {
                result = await _truckRepository.SaveTruckDetails(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TruckDomain", "SaveTruckDetails", ex.Message, ex);
            }
            return result;
        }

        public async Task<StatusModel> UpdateTruckDetails(TruckDetailViewModel model)
        {
            var result = new StatusModel();
            try
            {
                result = await _truckRepository.UpdateTruckDetails(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TruckDomain", "UpdateTruckDetails", ex.Message, ex);
            }
            return result;
        }
        
        public async Task<List<TruckDetailViewModel>> GetAllTruckDetails(int companyId)
        {
            var result =  new List<TruckDetailViewModel>();
            try
            {
                result = await _truckRepository.GetAllTruckDetails(companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TruckDomain", "GetAllTruckDetails", ex.Message, ex);
            }
            return result;
        }

        public async Task<DropdownDisplayExtended> GetTruckRegionDetails(string truckId)
        {
            var result = new DropdownDisplayExtended();
            try
            {
                result = await _truckRepository.GetTruckRegionDetails(truckId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TruckDomain", "GetTruckRegionDetails", ex.Message, ex);
            }
            return result;
        }


        public async Task<List<TruckDetailViewModel>> GetAllTruckRetainFuelDetails(int companyId)
        {
            var result = new List<TruckDetailViewModel>();
            try
            {
                result = await _truckRepository.GetAllTruckRetainFuelDetails(companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TruckDomain", "GetAllTruckRetainFuelDetails", ex.Message, ex);
            }
            return result;
        }

        public List<ExternalVehicleMappingViewModel> GetVehiclesForExternalMapping(int companyId)
        {
            var result = new List<ExternalVehicleMappingViewModel>();
            try
            {
                result =  _truckRepository.GetVehiclesForExternalMapping(companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TruckDomain", "GetVehiclesForExternalMapping", ex.Message, ex);
            }
            return result;
        }
       public async Task<StatusModel> SaveExternalVehicleMapping(ExternalVehicleMappingViewModel viewModel)
        {
            var result = new StatusModel();
            try
            {
                result = await _truckRepository.SaveExternalVehicleMapping(viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TruckDomain", "SaveExternalVehicleMapping", ex.Message, ex);
            }
            return result;
        }
        public async Task<StatusModel> SaveBulkUploadVehicleMapping(int userId, List<ExternalVehicleMappingViewModel> listExternalVehicles)
        {
            var result = new StatusModel();
            try
            {
                result = await _truckRepository.SaveBulkUploadVehicleMapping(userId,listExternalVehicles);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TruckDomain", "SaveBulkUploadVehicleMapping", ex.Message, ex);
            }
            return result;
        }
        public async Task<TruckDetailViewModel> GetTruckDetails(string truckId)
        {
            var result = new TruckDetailViewModel();
            try
            {
                result = await _truckRepository.GetTruckDetails(truckId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TruckDomain", "GetTruckDetails", ex.Message, ex);
            }
            return result;
        }
        public async Task<StatusModel> DeleteTruck(TruckDetailViewModel requestModel)
        {
            var result = new StatusModel();
            try
            {
                result = await _truckRepository.DeleteTruckAsync(requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TruckDomain", "DeleteTruck", ex.Message, ex);
            }
            return result;
        }
    
    }
}
