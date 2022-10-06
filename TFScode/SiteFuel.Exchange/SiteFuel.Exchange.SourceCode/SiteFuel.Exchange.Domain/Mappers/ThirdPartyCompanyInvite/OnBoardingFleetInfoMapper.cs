using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class OnBoardingFleetInfoMapper
    {
        public static List<FleetInformation> ToFleetEntity(this FleetInfo viewModel, List<FleetInformation> entity = null)
        {
            if (entity == null)
            {
                entity = new List<FleetInformation>();
            }
            foreach (var item in viewModel.DefAssets)
            {
                //if (item.FleetType == FleetType.DEF)
                //{
                    var defAsset = new FleetInformation();
                    defAsset.FleetType = FleetType.DEF;
                    defAsset.TrailerServiceType = (int)item.DEFTrailerServiceType;
                    defAsset.Capacity = item.Capacity;
                    defAsset.DoesTrailerHasPump = item.TrailerHasPump;
                    defAsset.IsTrailerMetered = item.IsTrailerMetered;
                    defAsset.Count = item.Count;
                    defAsset.IsPackagedGoods = item.PackagedGoods;
                    entity.Add(defAsset);
                //}
            }
            foreach (var item in viewModel.FuelAssets)
            {
                //if (item.FleetType != FleetType.DEF)
                //{
                    var fuelAsset = new FleetInformation();
                    fuelAsset.FleetType = FleetType.FuelAsset;
                    fuelAsset.TrailerServiceType = (int)item.FuelTrailerServiceTypeFTL;
                    fuelAsset.Capacity = item.Capacity;
                    fuelAsset.DoesTrailerHasPump = item.TrailerHasPump;
                    fuelAsset.IsTrailerMetered = item.IsTrailerMetered;
                    fuelAsset.Count = item.Count;
                    entity.Add(fuelAsset);
                //}
            }
            return entity;
        }
    }
}
