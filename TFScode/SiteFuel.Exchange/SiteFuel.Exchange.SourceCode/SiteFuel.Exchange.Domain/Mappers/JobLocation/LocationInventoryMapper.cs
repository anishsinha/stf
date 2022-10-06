using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class LocationInventoryMapper
    {
        public static LocationInventoryModel ToLocationInventoryModel(this SalesDataModel Model, LocationInventoryModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new LocationInventoryModel();
            viewModel.SiteId = Model.SiteId;
            viewModel.Location = Model.Location;
            viewModel.TankName = Model.TankName;
            viewModel.AvgSale = Model.AvgSale;
            viewModel.DaysRemaining = Model.DaysRemaining;
            viewModel.Inventory = Model.Inventory;
            viewModel.Priority = Model.Priority;
            return viewModel;
        }
    }
}
