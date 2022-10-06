using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Dispatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class BuyerLoadsForDashboardMapper
    {
        public static BuyerLoadsForDashboardViewModel ToViewModel(this UspBuyerLoadsForDashboard entity, BuyerLoadsForDashboardViewModel model)
        {
            if (model == null)
                model = new BuyerLoadsForDashboardViewModel();
            model.PoNumber = entity.PoNumber;
            model.Location = entity.Location;
            model.Priority = entity.Priority;
            model.Product = entity.ProductName;
            model.Status = entity.Status;
            model.Date = entity.Date;
            model.Dispatcher = entity.Dispatcher;
            if (entity.Quantity == ApplicationConstants.QuantityNotSpecified)
            {
                model.Quantity = Resource.lblNotSpecified;
            }
            else
            {
                if (entity.QuantityTypeId == (int)ScheduleQuantityType.Quantity || entity.QuantityTypeId == 0)
                {
                    model.Quantity = entity.Quantity.GetPreciseValue(2).GetCommaSeperatedValue()
                    + " " + (entity.UoM == UoM.Gallons ? "G" : "L");
                }
                else
                {
                    model.Quantity = EnumHelperMethods.GetDisplayName((ScheduleQuantityType)entity.QuantityTypeId);
                }
            }   
            return model;
        }
    }
}
