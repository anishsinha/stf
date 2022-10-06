using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Invoice.Pdf;
using System;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class FtlMapper
    {
        public static DemurrageDetailsViewModel ToDemurrageDetailsViewModel(this DemurrageDetailsData entity, DemurrageDetailsViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new DemurrageDetailsViewModel();

            viewModel.StartTime = DateTimeOffset.FromUnixTimeMilliseconds(entity.StartTime);
            viewModel.StartTime = viewModel.StartTime.AttachOffset(new TimeSpan(0, entity.StartOffset, 0));

            viewModel.EndTime = DateTimeOffset.FromUnixTimeMilliseconds(entity.EndTime);
            viewModel.EndTime = viewModel.EndTime.AttachOffset(new TimeSpan(0, entity.EndOffset, 0));
            viewModel.FeeTypeId = entity.FeeTypeId;
            return viewModel;
        }

        public static FuelTruckRetainDetailsViewModel ToFuelTruckRetainDetailsViewModel(this FuelTruckRetainDetailsData entity, FuelTruckRetainDetailsViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FuelTruckRetainDetailsViewModel();

            viewModel.StartTime = DateTimeOffset.FromUnixTimeMilliseconds(entity.StartTime);
            viewModel.StartTime = viewModel.StartTime.AttachOffset(new TimeSpan(0, entity.StartOffset, 0));

            viewModel.EndTime = DateTimeOffset.FromUnixTimeMilliseconds(entity.EndTime);
            viewModel.EndTime = viewModel.EndTime.AttachOffset(new TimeSpan(0, entity.EndOffset, 0));
            return viewModel;
        }

        public static DispatchLocationViewModel ToDispatchLocationViewModel(this DisptachLocationData entity, DispatchLocationViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new DispatchLocationViewModel();

            viewModel.Latitude = entity.Latitude;
            viewModel.Longitude = entity.Longitude;
            viewModel.PickupLocationType = entity.IsBulkPlant ? PickupLocationType.BulkPlant : PickupLocationType.Terminal;
            return viewModel;
        }

        public static BolDetailViewModel ToBolDetailsViewModel(this FtlDropRequestModel entity, BolDetailViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new BolDetailViewModel();

            viewModel.BolNumber = entity.BolDetails.BolNumber;
            viewModel.Carrier = entity.BolDetails.Carrier;
            viewModel.GrossQuantity = entity.BolDetails.GrossQuantity;
            viewModel.NetQuantity = entity.BolDetails.NetQuantity;
            viewModel.CreatedBy = entity.data.userid;
            viewModel.CreatedDate = DateTimeOffset.Now;
            viewModel.LiftDate = entity.BolDetails.LiftDate;
            viewModel.Id = entity.BolDetails.Id;
            viewModel.ImageId = entity.BolDetails.ImageId;

            if (!string.IsNullOrWhiteSpace(entity.BolDetails.BolImage))
                viewModel.Image = new ImageViewModel { Data = Convert.FromBase64String(entity.BolDetails.BolImage) };
            return viewModel;
        }

        public static BolDetailViewModel ToBolViewModel(this UspBolDetail entity, BolDetailViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new BolDetailViewModel();

            viewModel.Id = entity.Id;
            viewModel.BolNumber = entity.BolNumber;
            viewModel.GrossQuantity = entity.GrossQuantity;
            viewModel.NetQuantity = entity.NetQuantity;
            viewModel.DeliveredQuantity = entity.DeliveredQuantity;
            viewModel.Carrier = entity.Carrier;
            viewModel.FuelTypeId = entity.FuelTypeId;
            viewModel.TerminalName = entity.TerminalName;
            viewModel.FuelType = entity.FuelType;

            viewModel.SiteName = entity.LocationName;
            viewModel.Address = entity.PickupAddress;
            viewModel.City = entity.PickupCity;
            viewModel.StateCode = entity.PickupStateCode;
            viewModel.CountryCode = entity.CountryCode;
            viewModel.ZipCode = entity.PickupZipCode;
            viewModel.PickupLocationType = entity.PickupLocationType;

            viewModel.ImageId = entity.ImageId ?? 0;
            viewModel.InvoiceId = entity.InvoiceId;
            viewModel.BadgeNumber = entity.BadgeNumber;
            viewModel.IsBolEditForLfv = entity.IsBolEditedForLfv;
            viewModel.BolEditedNotes = entity.BolEditedNotes;
            return viewModel;
        }

        public static BolDetailViewModel ToLiftTicketViewModel(this UspBolDetail entity, BolDetailViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new BolDetailViewModel();

            viewModel.Id = entity.Id;
            viewModel.Carrier = entity.Carrier;
            viewModel.LiftTicketNumber = entity.LiftTicketNumber;
            viewModel.LiftQuantity = entity.LiftQuantity;
            viewModel.NetQuantity = entity.NetQuantity;
            viewModel.GrossQuantity = entity.GrossQuantity;
            viewModel.DeliveredQuantity = entity.DeliveredQuantity;
            viewModel.LiftDate = entity.LiftDate;
            viewModel.FuelTypeId = entity.FuelTypeId;
            viewModel.TerminalName = entity.TerminalName;
            viewModel.FuelType = entity.FuelType;

            viewModel.SiteName = entity.LocationName;
            viewModel.Address = entity.PickupAddress;
            viewModel.City = entity.PickupCity;
            viewModel.StateCode = entity.PickupStateCode;
            viewModel.CountryCode = entity.CountryCode;
            viewModel.ZipCode = entity.PickupZipCode;
            viewModel.PickupLocationType = entity.PickupLocationType;

            viewModel.ImageId = entity.ImageId ?? 0;
            viewModel.InvoiceId = entity.InvoiceId;
            viewModel.BadgeNumber = entity.BadgeNumber;
            viewModel.IsBolEditForLfv = entity.IsBolEditedForLfv;
            viewModel.BolEditedNotes = entity.BolEditedNotes;
            return viewModel;
        }

        public static BolDetailViewModel ToBolDetailsViewModel(this DropRequestModel entity, BolDetailViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new BolDetailViewModel();

            viewModel.BolNumber = entity.BolDetails.BolNumber;
            viewModel.Carrier = entity.BolDetails.Carrier;
            viewModel.GrossQuantity = entity.BolDetails.GrossQuantity;
            viewModel.NetQuantity = entity.BolDetails.NetQuantity;
            viewModel.CreatedBy = entity.data.userid;
            viewModel.CreatedDate = DateTimeOffset.Now;
            viewModel.LiftDate = entity.BolDetails.LiftDate;
            viewModel.Id = entity.BolDetails.Id;
            viewModel.ImageId = entity.BolDetails.ImageId;

            if (!string.IsNullOrWhiteSpace(entity.BolDetails.BolImage))
                viewModel.Image = new ImageViewModel { Data = Convert.FromBase64String(entity.BolDetails.BolImage) };
            return viewModel;
        }
    }
}
