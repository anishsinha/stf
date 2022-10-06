using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.DeliveryAlerts.Firebase
{
    public static class MobilePreLoadBolMapper
    {
        public static PreLoadBolViewModel ToPreLoadBolViewModel(this MobilePreLoadBolViewModel model)
        {
            var entity = new PreLoadBolViewModel();
            if (model.Products != null)
            {
                entity.Products = model.Products.Select(t => t.ToPreLoadProductDetail(model.IsBulkPlant)).ToList();
            }

            entity.Id = model.Id;
            entity.Driver = new Utilities.DropdownDisplayItem() { Id = model.DriverId };
            entity.SupplierCompanyId = model.SupplierCompanyId;
            entity.Carrier = model.Carrier;
            entity.IsBulkPlant = model.IsBulkPlant;
            if (!model.IsBulkPlant)
            {
                entity.BolNumber = model.BolNumber;
            }
            else
            {
                entity.LiftTicketNumber = model.LiftTicketNumber;
            }
            entity.BadgeNumber = model.BadgeNumber;
            entity.Images = model.Images?.ToPreLoadImageViewModel();
            entity.LiftDate = DateTimeOffset.FromUnixTimeMilliseconds(model.LiftDate);
            entity.PickupDate = DateTimeOffset.FromUnixTimeMilliseconds(model.PickupDate);
            entity.TraceId = model.TraceId;
            entity.IsPreLoadBolCompleted = model.IsPreLoadBolCompleted;
            if(model.LiftStartTime.HasValue && model.LiftStartTime.Value>0)
                entity.LiftStartTime = DateTimeOffset.FromUnixTimeMilliseconds(model.LiftStartTime.Value).ToString();
            if (model.LiftEndTime.HasValue && model.LiftEndTime.Value > 0)
                entity.LiftEndTime = DateTimeOffset.FromUnixTimeMilliseconds(model.LiftEndTime.Value).ToString();
            return entity;
        }
        public static PreLoadBolViewModel ToPickupBolRetainViewModel(this MobilePreLoadBolViewModel model)
        {
            var entity = new PreLoadBolViewModel();
            if (model.Products != null)
            {
                entity.Products = model.Products.Select(t => t.ToPreLoadProductDetail(model.IsBulkPlant)).ToList();
            }
            entity.Id = model.Id;
            entity.Driver = new Utilities.DropdownDisplayItem() { Id = model.DriverId };
            entity.SupplierCompanyId = model.SupplierCompanyId;
            entity.Carrier = model.Carrier;
            entity.IsBulkPlant = model.IsBulkPlant;
            if (!model.IsBulkPlant)
            {
                entity.BolNumber = model.BolNumber;
            }
            else
            {
                entity.LiftTicketNumber = model.LiftTicketNumber;
            }
            entity.BadgeNumber = model.BadgeNumber;
            entity.Images = model.Images?.ToPreLoadImageViewModel();
            entity.LiftDate = DateTimeOffset.FromUnixTimeMilliseconds(model.LiftDate);
            entity.PickupDate = DateTimeOffset.FromUnixTimeMilliseconds(model.PickupDate);
            entity.TraceId = model.TraceId;
            entity.IsPreLoadBolCompleted = model.IsPreLoadBolCompleted;
            entity.IsPickupBOLRetain = true;
            return entity;
        }
        public static PreLoadProductViewModel ToPreLoadProductDetail(this MobilePreLoadProductViewModel model,bool isBulkPlant)
        {
            var entity = new PreLoadProductViewModel();
            entity.OrderId = model.OrderId;
            entity.DeliveryScheduleId = model.DeliveryScheduleId;
            entity.TrackableScheduleId = model.TrackableScheduleId;

            entity.FuelTypeId = model.FuelTypeId;

            if (model.LiftQuantity.HasValue && model.LiftQuantity > 0)
            {
                entity.LiftQuantity = Convert.ToDecimal(model.LiftQuantity);
            }
            if (model.NetQuantity.HasValue)
            {
                entity.NetQuantity = Convert.ToDecimal(model.NetQuantity.Value);
            }
            if (model.GrossQuantity.HasValue)
            {
                entity.GrossQuantity = Convert.ToDecimal(model.GrossQuantity);
            }

            if (!isBulkPlant)
            {
                entity.TerminalId = model.TerminalId;
                entity.TerminalName = model.TerminalName;
            }
            else
            {
                entity.BulkPlantId = model.BulkPlantId; //For this file,Check with kailash in MobileInvoiceMapper.cs file
                entity.BulkPlantName = model.BulkPlantName;
                entity.Address = model.Address?.ToPreLoadAddressViewModel();
            }
            if (model.CompartmentInfo != null)
            {
                entity.CompartmentInfo = model.CompartmentInfo.Select(t => t.ToPreLoadCompartementInfoViewModel()).ToList();
            }
            entity.ProductId = model.ProductId;
            entity.ProductType = model.ProductType;
            entity.FuelType = model.FuelType;
            return entity;
        }

        public static PreLoadCompartmentInfoViewModel ToPreLoadCompartementInfoViewModel(this MobilePreLoadCompartmentInfoViewModel model)
        {
            var entity = new PreLoadCompartmentInfoViewModel();
            entity.CompartmentId = model.CompartmentId;
            entity.TrailerId = model.TrailerId;
            entity.Quantity = Convert.ToDecimal(model.Quantity);
            entity.UOM = model.UOM;
            return entity;
        }

        public static DropAddressViewModel ToPreLoadAddressViewModel(this MobilePreLoadDropAddressViewModel model)
        {
            var entity = new DropAddressViewModel();
            entity.Address = model.Address;
            entity.City = model.City;
            if (model.StateId > 0)
            {
                entity.State = new StateViewModel() { Id = model.StateId };
            }
            if (model.CountryId > 0)
            {
                entity.Country = new CountryViewModel { Id = model.CountryId };
            }
            entity.ZipCode = model.ZipCode;
            entity.CountyName = model.CountyName;
            entity.Latitude = Convert.ToDecimal(model.Latitude);
            entity.Longitude = Convert.ToDecimal(model.Longitude);
            entity.SiteId = model.JobId;
            entity.SiteName = model.JobName;
            return entity;
        }

        public static ImageViewModel ToPreLoadImageViewModel(this MobilePreLoadImageViewModel model)
        {
            var entity = new ImageViewModel();
            entity.Id = model.Id;
            entity.FilePath = model.FilePath;
            entity.IsPdf = model.IsPdf;
            return entity;
        }
    }
}
