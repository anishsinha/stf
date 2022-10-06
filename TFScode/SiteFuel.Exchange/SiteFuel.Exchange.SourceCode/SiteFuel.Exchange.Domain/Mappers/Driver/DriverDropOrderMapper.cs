using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class DriverDropOrderMapper
    {
        public static InvoiceViewModel ToInvoiceViewModel(this DriverDropOrderViewModel entity, InvoiceViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new InvoiceViewModel(Status.Success);

            viewModel.OrderId = entity.OrderId;
            viewModel.Image = entity.Image;
            viewModel.DroppedGallons = entity.Quantity;
            viewModel.DropStartDate = entity.DropStartDate;
            viewModel.DropEndDate = entity.DropEndDate;
            viewModel.IsWetHosingDelivery = entity.IsWetHosingDelivery;
            viewModel.IsOverWaterDelivery = entity.IsOverWaterDelivery;
            viewModel.CreatedBy = entity.Driver.UserId;
            viewModel.CreatedDate = DateTimeOffset.Now;
            viewModel.UpdatedBy = entity.Driver.UserId;
            viewModel.UpdatedDate = DateTimeOffset.Now;
            viewModel.UserId = entity.Driver.UserId;
            viewModel.DriverId = entity.Driver.UserId;
            viewModel.TraceId = entity.TraceId;

            if (entity.InvoiceStatusId != 0)
                viewModel.StatusId = entity.InvoiceStatusId;
            return viewModel;
        }

        public static MobileInvoiceCreateRequestViewModel ToMobileInvoiceViewModel(this DriverDropOrderViewModel entity)
        {
            var viewModel = new MobileInvoiceCreateRequestViewModel(Status.Success);
            viewModel.OrderId = entity.OrderId;
            viewModel.InvoiceImage = entity.Image;
            viewModel.FuelDropped = entity.Quantity;
            viewModel.DropStartDate = entity.DropStartDate;
            viewModel.DropEndDate = entity.DropEndDate;
            viewModel.IsWetHosingDelivery = entity.IsWetHosingDelivery;
            viewModel.IsOverWaterDelivery = entity.IsOverWaterDelivery;
            viewModel.DriverId = entity.Driver.UserId;
            viewModel.UserId = entity.Driver.UserId;
            viewModel.TraceId = entity.TraceId;
            viewModel.TrackableScheduleId = entity.TrackableScheduleId;
            viewModel.CustomerSignature = entity.CustomerSignatureViewModel;
            viewModel.AdditionalDetail = entity.ToInvoiceXAdditionalDetailsViewModel();
            viewModel.InvoiceStatusId = entity.InvoiceStatusId;
            viewModel.CreationMethod = entity.CreationMethod;
            viewModel.BolDetails = entity.BolDetails;

            if (entity.AdditionalImage != null && entity.AdditionalImage?.Id > 0)
            {
                viewModel.AdditionalImage.Id = entity.AdditionalImage.Id;
            }

            return viewModel;
        }

        public static InvoiceXAdditionalDetailViewModel ToInvoiceXAdditionalDetailsViewModel(this DriverDropOrderViewModel entity, InvoiceXAdditionalDetailViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new InvoiceXAdditionalDetailViewModel();

            viewModel.Latitude = entity.Driver.Latitude;
            viewModel.Longitude = entity.Driver.Longitude;
            viewModel.AssignedBy = entity.Driver.UserId;
            viewModel.DriverComment = entity.Driver.Comment;
            viewModel.AssetFilled = entity.Driver.AssetFilled;
            return viewModel;
        }
    }
}
