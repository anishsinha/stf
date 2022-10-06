using SiteFuel.Exchange.Utilities;
using SiteFuel.Models.ApiModels;
using SiteFuel.Models.InvoiceException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL.Mappers
{
    public static class GeneratedExceptionMapper
    {
        public static GeneratedExceptionModel ToGeneratedExceptionModel(this InvoiceExceptionRequestModel entity, GeneratedExceptionModel model = null)
        {
            if (model == null)
                model = new GeneratedExceptionModel();

            model.InvoiceId = entity.InvoiceId;
            model.BuyerCompanyId = entity.BuyerCompanyId;
            model.SupplierCompanyId = entity.SupplierCompanyId;
            model.GeneratedOn = DateTimeOffset.Now;

            return model;
        }

        public static GeneratedExceptionDetailModel ToGeneratedExceptionDetailModel(this InvoiceExceptionRequestModel entity, GeneratedExceptionDetailModel model = null)
        {
            if (model == null)
                model = new GeneratedExceptionDetailModel();

            model.DropDate = entity.DropDate;
            model.BuyerCompanyName = entity.BuyerCompanyName;
            model.SupplierCompanyName = entity.SupplierCompanyName;
            model.PoNumber = entity.PoNumber;
            model.InvoiceNumber = entity.InvoiceNumber?.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);
            model.JobName = entity.JobName;
            model.OrderedQuantity = entity.OrderedQuantity;
            model.BolQuantity = entity.BolQuantity;
            model.DeliveredQuantity = entity.DroppedQuantity;
            model.ScheduledLocation = entity.ScheduledLocation;
            model.DroppedLocation = entity.DroppedLocation;
            model.UserName = entity.UserName;
            model.UserId = entity.UserId;
            model.DriverId = entity.DriverId;
            model.OrderId = entity.OrderId;
            model.DriverName = entity.DriverName;
            model.CarrierName = entity.CarrierName;
            model.UOM= entity.UOM;
            model.ParameterJson = entity.ParameterJson;
            return model;
        }

        public static GeneratedExceptionDetailModel ToInvoiceExceptionModel(this InvoiceExceptionRequestModel entity, GeneratedExceptionDetailModel model = null)
        {
            if (model == null)
                model = new GeneratedExceptionDetailModel();

            model.DropDate = entity.DropDate;
            model.BuyerCompanyName = entity.BuyerCompanyName;
            model.SupplierCompanyName = entity.SupplierCompanyName;
            model.PoNumber = entity.PoNumber;
            model.InvoiceNumber = entity.InvoiceNumber;
            model.JobName = entity.JobName;
            model.DeliveredQuantity = entity.DroppedQuantity;
            model.OrderId = entity.OrderId;
            model.PricePerGallon = entity.PricePerGallon;
            model.ScheduledLocation = entity.ScheduledLocation;
            model.DroppedLocation = entity.DroppedLocation;
            model.UserId = entity.UserId;
            model.UserName = entity.UserName;
            model.DriverId = entity.DriverId;
            model.DriverName = entity.DriverName;
            model.ParameterJson = entity.ParameterJson;
            return model;
        }

        public static GeneratedExceptionViewModel ToInvoiceExceptionModel(this InvoiceExceptionModel entity, GeneratedExceptionViewModel model = null)
        {
            if (model == null)
                model = new GeneratedExceptionViewModel();

            model.InvoiceId = entity.InvoiceId;
            model.DropDate = entity.DropDate;
            model.PoNumber = entity.PoNumber;
            model.InvoiceNumber = entity.InvoiceNumber;
            model.DeliveredQuantity = entity.DroppedQuantity;
            model.OrderId = entity.OrderId;
            model.PricePerGallon = entity.PricePerGallon;
            model.UserId = entity.UserId;
            model.UserName = entity.UserName;
            return model;
        }
    }
}
