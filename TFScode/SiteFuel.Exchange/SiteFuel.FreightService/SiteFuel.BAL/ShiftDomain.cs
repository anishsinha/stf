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
    public class ShiftDomain : IShiftDomain
    {
        private IShiftRepository _shiftRepository;
        public ShiftDomain(IShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        public async Task<bool> DeleteAllShifts()
        {
            var response = false;
            try
            {
                response = await _shiftRepository.DeleteAllShifts();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ShiftDomain", "DeleteAllShifts", ex.Message, ex);
            }
            return response;
        }

        public async Task<ShiftResponseModel> CreateShift(ShiftViewModel model)
        {
            var response = new ShiftResponseModel();
            try
            {
                response = await _shiftRepository.CreateShift(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ShiftDomain", "CreateShift", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ShiftViewModel>> CreateShifts(List<ShiftViewModel> models)
        {
            var response = new List<ShiftViewModel>();
            try
            {
                response = await _shiftRepository.CreateShifts(models);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ShiftDomain", "CreateShifts", ex.Message, ex);
            }
            return response;
        }

        public async Task<ShiftResponseModel> UpdateShift(ShiftViewModel model)
        {
            var response = new ShiftResponseModel();
            try
            {
                response = await _shiftRepository.UpdateShift(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ShiftDomain", "UpdateShift", ex.Message, ex);
            }
            return response;
        }

        public async Task<ShiftResponseModel> DeleteShift(int companyId, string shiftId)
        {
            var response = new ShiftResponseModel();
            try
            {
                response = await _shiftRepository.DeleteShift(companyId, shiftId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ShiftDomain", "DeleteShift", ex.Message, ex);
            }
            return response;
        }

        public async Task<ShiftResponseModel> DeleteShifts(int companyId, string regionId)
        {
            var response = new ShiftResponseModel();
            try
            {
                response = await _shiftRepository.DeleteShifts(companyId, regionId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ShiftDomain", "DeleteShifts", ex.Message, ex);
            }
            return response;
        }

        public async Task<ShiftViewModel> GetShift(int companyId, string shiftId)
        {
            ShiftViewModel response = null;
            try
            {
                response = await _shiftRepository.GetShift(companyId, shiftId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ShiftDomain", "GetShift", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ShiftViewModel>> GetShifts(int companyId)
        {
            var response = new List<ShiftViewModel>();
            try
            {
                response = await _shiftRepository.GetShifts(companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ShiftDomain", "GetShift", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ShiftViewModel>> GetShifts(int companyId, string regionId)
        {
            var response = new List<ShiftViewModel>();
            try
            {
                response = await _shiftRepository.GetShifts(companyId, regionId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ShiftDomain", "GetShift", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetShiftDdl(int companyId)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = _shiftRepository.GetShiftDdl(companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ShiftDomain", "GetShift", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ShiftViewModel>> GetDriversShifts(int companyId, string regionId, string SelectedDate,bool IsDsbDriverSchedule)
        {
            var response = new List<ShiftViewModel>();
            try
            {
                response = await _shiftRepository.GetDriversShifts(companyId, regionId, SelectedDate, IsDsbDriverSchedule);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ShiftDomain", "GetDriversShifts", ex.Message, ex);
            }
            return response;
        }
    }
}
