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
    public static class MobileFuelRetainMapper
    {
        public static FuelRetainViewModel ToFuelRetainViewModel(this MobileFuelRetainViewModel model)
        {
            var entity = new FuelRetainViewModel();
            entity.TrailerFuelRetain = model.TrailerFuelRetain.Select(t => t.ToTrailerFuelRetainViewModel(model.DriverId)).ToList();
            var bolDetails = model.BolDetails.ToRetainViewModel(entity.TrailerFuelRetain, entity.DriverId);
            var liftTikets = model.TicketDetails.ToRetainViewModel(entity.TrailerFuelRetain, entity.DriverId);
            var distinctBols = bolDetails.Concat(liftTikets).Distinct().ToList();
            entity.BolDetails.AddRange(distinctBols);

            //Assign trackableScheduleId to the bol products
            for (int i = 0; i < entity.BolDetails.Count; i++)
            {
                var item = entity.BolDetails[i];
                item.TraceId = model.TraceId;
                for (int j = 0; j < item.Products.Count; j++)
                {
                    var product = item.Products[j];
                    var retainProduct = entity.TrailerFuelRetain.FirstOrDefault(t => t.ProductId == product.FuelTypeId);
                    if (retainProduct != null)
                    {
                        product.TrackableScheduleId = retainProduct.TrackableScheduleId;
                    }
                }
            }
            return entity;
        }

        public static TrailerFuelRetainViewModel ToTrailerFuelRetainViewModel(this MobileTrailerFuelRetainViewModel model, int driverId)
        {
            var entity = new TrailerFuelRetainViewModel();
            entity.CompartmentId = model.CompartmentId;
            entity.TrailerId = model.TrailerId;
            entity.Quantity = Convert.ToDecimal(model.Quantity);
            entity.ProductType = model.ProductType;
            entity.ProductId = model.ProductId;
            entity.UOM = model.UoM;
            entity.TrackableScheduleId = model.TrackableScheduleId;
            entity.TfxDriverId = driverId;
            return entity;
        }

        public static List<PreLoadBolViewModel> ToRetainViewModel(this List<MobileInvoiceBolViewModel> entity, List<TrailerFuelRetainViewModel> retainedFuels, int driverId)
        {
            var models = new List<PreLoadBolViewModel>();
            foreach (var retainedFuel in retainedFuels.Distinct())
            {
                var fuelBOLs = entity.Where(t1 => t1.Products.Any(t2 => t2.ProductId == retainedFuel.ProductId)).ToList();
                foreach (var item in fuelBOLs)
                {
                    var bol = new PreLoadBolViewModel
                    {
                        Driver = new DropdownDisplayItem { Id = driverId },
                        Products = item.Products.Select(t => t.ToProductDetail()).ToList(),
                        //SupplierCompanyId
                        //Carrier = item.
                        BolNumber = item.BolNumber,
                        IsBulkPlant = false,
                        BadgeNumber = item.BadgeNumber,
                        Images = item.Images.ToImageViewModel(),
                        LiftDate = DateTimeOffset.FromUnixTimeMilliseconds(item.LiftDate),
                        PickupDate = DateTimeOffset.FromUnixTimeMilliseconds(item.LiftDate),
                        IsPreLoadBolCompleted = true,
                    };
                    models.Add(bol);
                }
            }
            return models;
        }

        public static List<PreLoadBolViewModel> ToRetainViewModel(this List<MobileInvoiceLiftTicketViewModel> entity, List<TrailerFuelRetainViewModel> retainedFuels, int driverId)
        {
            var models = new List<PreLoadBolViewModel>();
            foreach (var retainedFuel in retainedFuels.Distinct())
            {
                var fuelBOLs = entity.Where(t1 => t1.Products.Any(t2 => t2.ProductId == retainedFuel.ProductId)).ToList();
                foreach (var item in fuelBOLs)
                {
                    var bol = new PreLoadBolViewModel
                    {
                        Driver = new DropdownDisplayItem { Id = driverId },
                        Products = item.Products.Select(t => t.ToProductDetail()).ToList(),
                        //SupplierCompanyId
                        //Carrier = item.
                        LiftTicketNumber = item.LiftTicketNumber,
                        IsBulkPlant = true,
                        BadgeNumber = item.BadgeNumber,
                        Images = item.Images.ToImageViewModel(),
                        LiftDate = DateTimeOffset.FromUnixTimeMilliseconds(item.LiftDate),
                        PickupDate = DateTimeOffset.FromUnixTimeMilliseconds(item.LiftDate),
                        IsPreLoadBolCompleted = true,
                    };
                    models.Add(bol);
                }
            }
            return models;
        }

        public static PreLoadProductViewModel ToProductDetail(this MobileBolProductViewModel model)
        {
            var entity = new PreLoadProductViewModel();
            entity.FuelTypeId = model.ProductId;
            if (model.NetQuantity.HasValue)
            {
                entity.NetQuantity = Convert.ToDecimal(model.NetQuantity.Value);
            }
            if (model.GrossQuantity.HasValue)
            {
                entity.GrossQuantity = Convert.ToDecimal(model.GrossQuantity);
            }
            entity.TerminalId = model.TerminalId;
            entity.TerminalName = model.TerminalName;
            if (model.CompartmentInfo != null)
            {
                entity.CompartmentInfo = model.CompartmentInfo.Select(t => t.ToCompartementInfoViewModel()).ToList();
            }
            return entity;
        }

        public static PreLoadProductViewModel ToProductDetail(this MobileLiftProductViewModel model)
        {
            var entity = new PreLoadProductViewModel();
            entity.FuelTypeId = model.ProductId;
            if (model.NetQuantity.HasValue)
            {
                entity.NetQuantity = Convert.ToDecimal(model.NetQuantity.Value);
            }
            if (model.GrossQuantity.HasValue)
            {
                entity.GrossQuantity = Convert.ToDecimal(model.GrossQuantity);
            }
            entity.BulkPlantId = model.BulkPlantId;
            entity.BulkPlantName = model.BulkPlantName;
            entity.Address = model.Address?.ToAddressViewModel();
            if (model.CompartmentInfo != null)
            {
                entity.CompartmentInfo = model.CompartmentInfo.Select(t => t.ToCompartementInfoViewModel()).ToList();
            }
            return entity;
        }

        public static PreLoadCompartmentInfoViewModel ToCompartementInfoViewModel(this MobileCompartmentInfoViewModel model)
        {
            var entity = new PreLoadCompartmentInfoViewModel();
            entity.CompartmentId = model.CompartmentId;
            entity.TrailerId = model.TrailerId;
            entity.Quantity = Convert.ToDecimal(model.Quantity);
            return entity;
        }
    }
}
