using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace TrueFill.FreightApi.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
        [ApiExplorerSettings(IgnoreApi = true)]
#endif
    public class ShiftController : ApiController
    {
        private readonly IShiftDomain _shiftDomain;

        public ShiftController(IShiftDomain shiftDomain)
        {
            _shiftDomain = shiftDomain;
        }

        [HttpGet]
        public async Task<ShiftViewModel> GetShift(int companyId, string shiftId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ShiftController", $"GetShift(companyId:{companyId}, regionId:{shiftId})"))
            {
                var response = await _shiftDomain.GetShift(companyId, shiftId);
                return response;
            }
        }

        [HttpGet]
        public async Task<List<ShiftViewModel>> GetShifts(int companyId, string regionId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ShiftController", $"GetShifts(companyId:{companyId}, regionId:{regionId})"))
            {
                var response = await _shiftDomain.GetShifts(companyId, regionId);
                return response;
            }
        }
        [HttpGet]
        public async Task<List<ShiftViewModel>> GetDriversShifts(int companyId, string regionId, string SelectedDate,bool IsDsbDriverSchedule)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ShiftController", $"GetDriversShifts(companyId:{companyId}, regionId:{regionId},SelectedDate:{SelectedDate})"))
            {
                var response = await _shiftDomain.GetDriversShifts(companyId, regionId, SelectedDate, IsDsbDriverSchedule);
                return response;
            }
        }
    }
}
