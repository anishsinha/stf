using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface ITankDomain
    {
        Task<bool> SaveTankDetails(TankDetailsModel model);

        Task<bool> UpdateTankDetails(TankDetailsModel model);

        Task<TankDetailsModel> GetTankDetails(int id);
    }
}
