using SiteFuel.FreightModels;
using SiteFuel.FreightModels.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface IDriverDomain
    {
        Task<StatusModel> CreateDriver(DriverObjectModel model);
        Task<StatusModel> UpdateDriver(DriverObjectModel model);
        Task<StatusModel> DeleteDriver(int driverId, int companyId);
        Task<DriverObjectModel> GetDriver(int driverId, int companyId);
    }
}
