using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface IShiftRepository
    {
        Task<bool> DeleteAllShifts();
        Task<ShiftResponseModel> CreateShift(ShiftViewModel model);
        Task<List<ShiftViewModel>> CreateShifts(List<ShiftViewModel> models);
        Task<ShiftResponseModel> UpdateShift(ShiftViewModel model);
        Task<ShiftResponseModel> DeleteShift(int companyId, string shiftId);
        Task<ShiftResponseModel> DeleteShifts(int companyId, string regionId);
        Task<ShiftViewModel> GetShift(int companyId, string shiftId);
        Task<List<ShiftViewModel>> GetShifts(int companyId);
        Task<List<ShiftViewModel>> GetShifts(int companyId, string regionId);
        List<DropdownDisplayExtendedItem> GetShiftDdl(int companyId);
        Task<List<ShiftViewModel>> GetDriversShifts(int companyId, string regionId, string SelectedDate,bool IsDsbDriverSchedule);
    }
}
