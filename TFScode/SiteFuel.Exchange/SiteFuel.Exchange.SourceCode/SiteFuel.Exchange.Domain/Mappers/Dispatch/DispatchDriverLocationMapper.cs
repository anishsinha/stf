using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Dispatcher;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public static class DispatchDriverLocationMapper
    {
        public static WhereIsMyDriverViewModel ToViewModel(this UspGetDispatcherDriverLocation entity, WhereIsMyDriverViewModel model)
        {
            if (model == null)
                model = new WhereIsMyDriverViewModel();

            model.Id = entity.Id;
            if (!string.IsNullOrWhiteSpace(entity.FirstName) && !string.IsNullOrWhiteSpace(entity.LastName))
            {
                model.Name = $"{entity.FirstName} {entity.LastName}";
                model.Intl = $"{entity.FirstName.First()}{entity.LastName.First()}".ToUpper();
            }
            model.DName = entity.Dispatcher;
            model.CName = entity.Customer;
            model.PhNo = entity.PhoneNumber;
            model.Lat = entity.Latitude;
            model.Lng = entity.Longitude;
            model.Pckup = entity.Pickup;
            model.Loc = entity.Location;
            model.StId = entity.StateId;
            model.dLat = entity.JobLatitude;
            model.dLng = entity.JobLongitude;
            model.PoNum = entity.PoNumber;
            model.LdDate = entity.Date;
            model.PrdtNm = entity.ProductName;
            if (entity.Quantity == ApplicationConstants.QuantityNotSpecified)
            {
                model.Qty = Resource.lblNotSpecified;
            }
            else
            {
                if (entity.QuantityTypeId == (int)ScheduleQuantityType.Quantity || entity.QuantityTypeId == 0)
                {
                    model.Qty = entity.Quantity.GetPreciseValue(2).GetCommaSeperatedValue()
                    + " " + (entity.UoM == UoM.Gallons ? "G" : "L");
                }
                else
                {
                    model.Qty = EnumHelperMethods.GetDisplayName((ScheduleQuantityType)entity.QuantityTypeId);
                }
            }
            model.SttsId = entity.StatusId;
            model.Status = entity.Status;
            model.DrId = entity.DrId;
            model.RgId = entity.RgId;
            model.LdPri = entity.LdPri;
            model.TotalCount = entity.TotalCount;
            model.FilteredCount = entity.FilteredCount;
            model.AppLastUpdatedDate = entity.AppLastUpdatedDate;
            model.DROPTicketNum = entity.DROPTicketNum;
            model.IsOnline = entity.IsOnline;
            model.AllowCustomerDriverChat = entity.AllowCustomerDriverChat;
            model.InventoryDataCaptureType = entity.InventoryDataCaptureType;
            model.InventoryDataCaptureTypeName = entity.InventoryDataCaptureType.GetDisplayName();
            model.TrailerDisplayId = entity.TrailerDisplayId;
            model.UniqueOrderNo = entity.UniqueOrderNo;
            return model;
        }

        public static WhereIsMyDriverViewModel ToDriverModel(this FreightDeliveryRequestDetail entity, WhereIsMyDriverViewModel model)
        {
            if (model == null)
                model = new WhereIsMyDriverViewModel();

            model.RgId = entity.RgId;
            model.RgName = entity.RgName;
            model.LdPri = entity.LdPri;
            model.RgStates = entity.States;

            return model;
        }
        
    }
}
