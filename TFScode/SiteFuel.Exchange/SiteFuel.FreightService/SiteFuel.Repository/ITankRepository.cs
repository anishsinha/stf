using MongoDB.Bson;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface ITankRepository
    {
        Task<bool> SaveTankDetails(TankDetailsModel model);

        Task<bool> UpdateTankDetails(TankDetailsModel model);

        Task<TankDetailsModel> GetTankDetails(int id);
    }
}
