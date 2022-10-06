using SiteFuel.Exchange.ViewModels;
using System;
using System.Linq;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class OldMobileApiMapper
    {
        public static AssetDropDetails ToAssetDropDetails(this DriverOrderAssetViewModel entity, AssetDropDetails viewModel = null)
        {
            if (viewModel == null)
                viewModel = new AssetDropDetails();

            viewModel.AssetId = entity.Asset.Id;
            viewModel.AssetDropId = entity.FuelDrop.AssetDropId;
            viewModel.OrderId = entity.FuelDrop.OrderId;
            viewModel.InvoiceId = entity.FuelDrop.InvoiceId;
            viewModel.AssetName = entity.Asset.Name;
            viewModel.AssetType = entity.Asset.AssetAdditionalDetail.Class;
            viewModel.VehicalId = entity.Asset.AssetAdditionalDetail.VehicleId;
            viewModel.LicensePlateState = entity.Asset.AssetAdditionalDetail.LicensePlateState;
            viewModel.LicensePlate = entity.Asset.AssetAdditionalDetail.LicensePlate;
            viewModel.Make = entity.Asset.AssetAdditionalDetail.Make;
            viewModel.Model = entity.Asset.AssetAdditionalDetail.Model;
            viewModel.Year = entity.Asset.AssetAdditionalDetail.Year;
            viewModel.Color = entity.Asset.AssetAdditionalDetail.Color;
            viewModel.AssetDropId = entity.FuelDrop.AssetDropId;
            viewModel.FuelType = entity.Asset.FuelType.Name;
            viewModel.FuelCapacity = entity.Asset.AssetAdditionalDetail.FuelCapacity ?? 0.0M;
            viewModel.primaryGallonsDropped = entity.FuelDrop.PrimaryDrop;
            viewModel.PrimaryMeterStartReading = entity.FuelDrop.PrimaryMeterStartReading;
            viewModel.PrimaryMeterEndReading = entity.FuelDrop.PrimaryMeterEndReading;
            viewModel.secondaryGallonsDropped = entity.FuelDrop.SecondaryDrop;
            viewModel.SecondaryMeterStartReading = entity.FuelDrop.SecondaryMeterStartReading;
            viewModel.SecondaryMeterReading = entity.FuelDrop.SecondaryMeterEndReading;
            viewModel.isNoFuelNeeded = entity.FuelDrop.IsNoFuelNeeded;
            viewModel.SpillOccurred = entity.FuelDrop.IsSpillOccurred;
            viewModel.SpillId = entity.FuelDrop.SpillId;
            viewModel.JobXAssignmentId = entity.FuelDrop.JobXAssetId;
            viewModel.AssetImageId = entity.Asset.Image == null ? 0 : entity.Asset.Image.Id;
            viewModel.DroppedGallons = entity.FuelDrop.DroppedGallons;
            viewModel.DropStatus = entity.FuelDrop.DropStatus;
            viewModel.AssetDropDetail = entity.FuelDrop.AssetDropDetail;
            return viewModel;
        }

        public static DriverFuelSpillViewModel ToDriverFuelSpillViewModel(this SpillFuelDetails entity, DriverFuelSpillViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new DriverFuelSpillViewModel(Utilities.Status.Success);

            viewModel.Id = entity.SpillId;
            viewModel.SpillDate = DateTimeOffset.FromUnixTimeMilliseconds(entity.SpillTime);
            viewModel.SpilledBy = entity.UserId;
            viewModel.Notes = entity.Notes;
            viewModel.AssetId = entity.AssetId;
            viewModel.OrderId = entity.OrderId;
            if (entity.InvoiceId > 0)
                viewModel.InvoiceId = entity.InvoiceId;

            if (entity.ImageList != null)
                viewModel.SpillImages = entity.ImageList.Select(t => new ImageViewModel { Id = t.Id, Data = t.Image }).ToList();

            return viewModel;
        }

        public static SpillFuelDetails ToSpillFuelDetailsViewModel(this DriverFuelSpillViewModel entity, SpillFuelDetails viewModel = null)
        {
            if (viewModel == null)
                viewModel = new SpillFuelDetails();

            viewModel.SpillId = entity.Id;
            viewModel.SpillTime = entity.SpillDate.ToUnixTimeMilliseconds();
            viewModel.UserId = entity.SpilledBy;
            viewModel.Notes = entity.Notes;
            viewModel.AssetId = entity.AssetId;
            viewModel.OrderId = entity.OrderId;
            if (entity.InvoiceId > 0)
                viewModel.InvoiceId = entity.InvoiceId ?? 0;

            if (entity.SpillImages != null)
                viewModel.ImageList = entity.SpillImages.Select(t => new AppImageViewModel { Id = t.Id, Image = t.Data }).ToList();

            return viewModel;
        }

        public static FilledAssetDetails ToFilledAssetDetails(this DriverOrderAssetViewModel entity, FilledAssetDetails viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FilledAssetDetails();

            viewModel.AssetId = entity.Asset.Id;
            viewModel.primaryGallonsDropped = entity.FuelDrop.PrimaryDrop;
            viewModel.PrimaryMeterStartReading = entity.FuelDrop.PrimaryMeterStartReading;
            viewModel.PrimaryMeterEndReading = entity.FuelDrop.PrimaryMeterEndReading;
            viewModel.primaryAssetDropId = entity.FuelDrop.PrimaryDropId;
            viewModel.secondaryGallonsDropped = entity.FuelDrop.SecondaryDrop;
            viewModel.SecondaryMeterStartReading = entity.FuelDrop.SecondaryMeterStartReading;
            viewModel.SecondaryMeterReading = entity.FuelDrop.SecondaryMeterEndReading;
            viewModel.secondaryAssetDropId = entity.FuelDrop.SecondaryDropId;
            viewModel.isNoFuelNeeded = entity.FuelDrop.IsNoFuelNeeded;
            viewModel.SpillOccurred = entity.FuelDrop.IsSpillOccurred;
            viewModel.spillID = entity.FuelDrop.SpillId;
            viewModel.DroppedGallons = entity.FuelDrop.DroppedGallons;

            if (entity.Asset.Image != null)
                viewModel.AssetImage = entity.Asset.Image.Data;

            return viewModel;
        }

        public static AssetDropHistory ToDriverAssetHistory(this DriverAssetDropHistoryViewModel entity, AssetDropHistory viewModel = null)
        {
            if (viewModel == null)
                viewModel = new AssetDropHistory();

            viewModel.AssetId = entity.Asset.Id;
            viewModel.Type = entity.Asset.AssetAdditionalDetail.Class;
            viewModel.DropList = entity.DropHistory.Select(t => new DropHistory
            {
                DropDateTime = DateTimeOffset.Parse($"{t.DropDate} {t.DropTime}").ToUnixTimeMilliseconds(),
                DropDate = t.DropDate,
                DropTime = t.DropTime,
                DroppedGallons = t.DropAmount,
                TimeZoneName = t.TimeZoneName
            }).ToList();

            if (entity.Asset.Image != null)
                viewModel.Image = entity.Asset.Image.Data;

            return viewModel;
        }

        public static List<DriverAssetFuelDropViewModel> ToDriverAssetFuelDropViewModel(this AssetDropRequestModel entity, List<DriverAssetFuelDropViewModel> viewModel = null)
        {
            if (viewModel == null)
                viewModel = new List<DriverAssetFuelDropViewModel>();

            if(entity.data.AssetDropDetail != null && entity.data.AssetDropDetail.Count > 0)
            {
                foreach (var item in entity.data.AssetDropDetail)
                {
                    var assetFuelDropModel = SetDriverAssetFuelDropDetails(entity, item.AssetDropId, item.OrderId, item.Gravity, Convert.ToString(item.Quantity), item.JobXAssetId);

                    viewModel.Add(assetFuelDropModel);
                }
            }
            else
            {
                var assetFuelDropModel = SetDriverAssetFuelDropDetails(entity, entity.data.AssetDropId, entity.data.OrderId, entity.data.Gravity, entity.data.quantity, entity.data.JobXAssignmentId);
                viewModel.Add(assetFuelDropModel);
            }

            return viewModel;
        }

        private static DriverAssetFuelDropViewModel SetDriverAssetFuelDropDetails(AssetDropRequestModel entity, int assetDropId, int orderId, string gravity, string quantity,int jobXAssetId)
        {
            var assetFuelDropModel = new DriverAssetFuelDropViewModel();
            assetFuelDropModel.FuelDrop.AssetDropId = assetDropId;
            assetFuelDropModel.FuelDrop.InvoiceId = entity.data.InvoiceId;
            assetFuelDropModel.FuelDrop.OrderId = orderId;
            assetFuelDropModel.FuelDrop.JobXAssetId = jobXAssetId;
            assetFuelDropModel.FuelDrop.DropDate = entity.data.timeStamps.assetEndDropTime.GetDateTimeOffsetWithTimeZone();

            entity.data.Gravity = entity.data.Gravity == null ? string.Empty : entity.data.Gravity.Replace(",", string.Empty);
            assetFuelDropModel.FuelDrop.RunningMeterMode = entity.data.RunningMeterMode;
            assetFuelDropModel.FuelDrop.PrimaryDrop = Convert.ToDecimal(quantity);
            assetFuelDropModel.FuelDrop.Gravity = string.IsNullOrEmpty(gravity) ? (decimal?)null : Convert.ToDecimal(gravity);
            assetFuelDropModel.FuelDrop.PrimaryMeterStartReading = entity.data.PrimaryMeterStartReading;
            assetFuelDropModel.FuelDrop.PrimaryMeterEndReading = entity.data.PrimaryMeterEndReading;
            assetFuelDropModel.FuelDrop.SecondaryDrop = entity.data.AdditionalDrop;
            assetFuelDropModel.FuelDrop.SecondaryMeterStartReading = entity.data.SecondaryMeterStartReading;
            assetFuelDropModel.FuelDrop.SecondaryMeterEndReading = entity.data.SecondaryMeterReading;
            assetFuelDropModel.FuelDrop.DroppedBy = entity.data.userid;
            assetFuelDropModel.FuelDrop.IsNoFuelNeeded = entity.data.IsNoFuelNeeded;
            assetFuelDropModel.FuelDrop.DropStatus = entity.data.DropStatus;
            assetFuelDropModel.FuelDrop.IsNewAsset = entity.data.IsNewAsset;

            assetFuelDropModel.Driver.UserId = entity.data.userid;
            assetFuelDropModel.Driver.CompanyId = entity.data.companyid;
            assetFuelDropModel.Driver.Latitude = Convert.ToDecimal(entity.data.Latitude);
            assetFuelDropModel.Driver.Longitude = Convert.ToDecimal(entity.data.Longitude);
            assetFuelDropModel.DropStartDate = DateTimeOffset.FromUnixTimeMilliseconds(entity.data.timeStamps.assetStartDropTime);
            assetFuelDropModel.DropEndDate = DateTimeOffset.FromUnixTimeMilliseconds(entity.data.timeStamps.assetEndDropTime);

            if (!string.IsNullOrWhiteSpace(entity.receipt))
                assetFuelDropModel.FuelDrop.Image = new ImageViewModel { Data = Convert.FromBase64String(entity.receipt) };

            return assetFuelDropModel;
        }

        public static DriverDropOrderViewModel ToDriverDropOrderViewModel(this NewOrderRequestModel entity, DriverDropOrderViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new DriverDropOrderViewModel();

            entity.data.quantity = entity.data.quantity.Replace(",", "");
            viewModel.Driver = new DriverViewModel
            {
                UserId = entity.data.userid,
                CompanyId = entity.data.companyid,
                Latitude = Convert.ToDecimal(entity.data.Latitude),
                Longitude = Convert.ToDecimal(entity.data.Longitude),
                Comment = entity.data.customerName,
                AssetFilled = entity.data.assetCount
            };

            var timeZoneName = GoogleApiDomain.GetTimeZone(viewModel.Driver.Latitude, viewModel.Driver.Longitude);

            viewModel.IsWetHosingDelivery = entity.data.wetHosing;
            viewModel.IsOverWaterDelivery = entity.data.overWater;
            viewModel.DropStartDate = DateTimeOffset.FromUnixTimeMilliseconds(entity.data.timeStamps.startDropTime).ToTargetDateTimeOffset(timeZoneName);
            viewModel.DropEndDate = DateTimeOffset.FromUnixTimeMilliseconds(entity.data.timeStamps.endDropTime).ToTargetDateTimeOffset(timeZoneName);
            viewModel.TraceId = entity.data.TraceId;
            viewModel.UnitOfMeasurement = entity.data.UnitOfMeasurement;
            viewModel.Quantity = Convert.ToDecimal(entity.data.quantity);

            if (!string.IsNullOrWhiteSpace(entity.receipt))
                viewModel.Image = new ImageViewModel { Data = Convert.FromBase64String(entity.receipt) };
            if (entity.SignatureData != null)
                viewModel.CustomerSignatureViewModel = entity.SignatureData.ToViewModel();

            return viewModel;
        }

        public static DriverDropOrderViewModel ToDriverDropOrderViewModel(this DropRequestModel entity, DriverDropOrderViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new DriverDropOrderViewModel();

            entity.data.orderId = entity.data.orderId.Replace(ApplicationConstants.SFPO, "");
            entity.data.quantity = entity.data.quantity.Replace(",", "");
            entity.data.fuelId = entity.data.fuelId.Replace("SFRQ", "");

            viewModel.Driver.UserId = entity.data.userid;
            viewModel.Driver.CompanyId = entity.data.companyid;
            viewModel.Driver.Latitude = Convert.ToDecimal(entity.data.Latitude);
            viewModel.Driver.Longitude = Convert.ToDecimal(entity.data.Longitude);
            viewModel.Driver.FCMAppId = entity.data.FCMAppId;
            viewModel.Driver.AssetFilled = entity.data.assetCount;
            viewModel.TraceId = entity.data.TraceId;

            viewModel.OrderId = Convert.ToInt32(entity.data.orderId);
            viewModel.FuelId = Convert.ToInt32(entity.data.fuelId);
            viewModel.DropStartDate = DateTimeOffset.FromUnixTimeMilliseconds(entity.data.timeStamps.startDropTime);
            viewModel.DropEndDate = DateTimeOffset.FromUnixTimeMilliseconds(entity.data.timeStamps.endDropTime);

            viewModel.Quantity = Convert.ToDecimal(entity.data.quantity);
            viewModel.SpecialInstructions = entity.specialInstructions;
            viewModel.TrackableScheduleId = entity.data.TrackableScheduleId;
            viewModel.DeliveryScheduleId = entity.data.DeliveryScheduleId;
            viewModel.CreationMethod = CreationMethod.Mobile;

            if (entity.data.InvoiceStatusId != 0)
                viewModel.InvoiceStatusId = entity.data.InvoiceStatusId;
            if (!string.IsNullOrWhiteSpace(entity.receipt))
                viewModel.Image = new ImageViewModel { Data = Convert.FromBase64String(entity.receipt) };
            if (entity.SignatureData != null)
                viewModel.CustomerSignatureViewModel = entity.SignatureData.ToViewModel();
        
            if (entity.AdditionalImageId != null && entity.AdditionalImageId.HasValue && entity.AdditionalImageId.Value > 0)
                viewModel.AdditionalImage = new ImageViewModel { Id = entity.AdditionalImageId.Value };

            if (entity.FuelPickLocation != null)
                viewModel.FuelPickLocation = entity.FuelPickLocation.ToDispatchLocationViewModel();

            if (entity.BolDetails != null)
                viewModel.BolDetails = entity.ToBolDetailsViewModel();

            return viewModel;
        }

        public static FtlDriverDropOrderViewModel ToFtlDriverDropOrderViewModel(this FtlDropRequestModel entity, FtlDriverDropOrderViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FtlDriverDropOrderViewModel();

            entity.data.orderId = entity.data.orderId.Replace(ApplicationConstants.SFPO, "");
            entity.data.quantity = entity.data.quantity.Replace(",", "");
            entity.data.fuelId = entity.data.fuelId.Replace(ApplicationConstants.SFRQ, "");

            viewModel.Driver.UserId = entity.data.userid;
            viewModel.Driver.CompanyId = entity.data.companyid;
            viewModel.Driver.Latitude = Convert.ToDecimal(entity.data.Latitude);
            viewModel.Driver.Longitude = Convert.ToDecimal(entity.data.Longitude);
            viewModel.Driver.FCMAppId = entity.data.FCMAppId;
            viewModel.Driver.AssetFilled = entity.data.assetCount;
            viewModel.TraceId = entity.data.TraceId;

            viewModel.OrderId = Convert.ToInt32(entity.data.orderId);
            viewModel.FuelId = Convert.ToInt32(entity.data.fuelId);
            viewModel.DropStartDate = DateTimeOffset.FromUnixTimeMilliseconds(entity.data.timeStamps.startDropTime);
            viewModel.DropEndDate = DateTimeOffset.FromUnixTimeMilliseconds(entity.data.timeStamps.endDropTime);

            viewModel.Quantity = Convert.ToDecimal(entity.data.quantity);
            viewModel.SpecialInstructions = entity.specialInstructions;
            viewModel.TrackableScheduleId = entity.data.TrackableScheduleId;
            viewModel.DeliveryScheduleId = entity.data.DeliveryScheduleId;

            viewModel.FuelSurchargeDistance = entity.FuelSurchargeDistance;
            if (entity.data.InvoiceStatusId != 0)
                viewModel.InvoiceStatusId = entity.data.InvoiceStatusId;

            if (!string.IsNullOrWhiteSpace(entity.receipt))
                viewModel.Image = new ImageViewModel { Data = Convert.FromBase64String(entity.receipt) };

            if (entity.SignatureData != null)
                viewModel.CustomerSignatureViewModel = entity.SignatureData.ToViewModel();

            // set additional image 
            viewModel.AdditionalImage = new ImageViewModel { Id = entity.AdditionalImageId ?? 0 };

            if (entity.DemurrageDetails != null)
            {
                var demurrageDetailsList = entity.DemurrageDetails.Select(t => t.ToDemurrageDetailsViewModel()).ToList();
                viewModel.DemurrageDetails = demurrageDetailsList;
            }

            viewModel.IsSplitTank = entity.IsSplitTank;
            viewModel.IsSplitLoad = entity.IsSplitLoad;
            viewModel.SplitLoadChainId = entity.SplitLoadChainId;
            
            if (entity.FuelTruckRetainDetails != null)
                viewModel.FuelTruckRetainDetails = entity.FuelTruckRetainDetails.ToFuelTruckRetainDetailsViewModel();

            if (entity.FuelPickLocation != null)
                viewModel.FuelPickLocation = entity.FuelPickLocation.ToDispatchLocationViewModel();

            if (entity.FuelDropLocation != null)
                viewModel.FuelDropLocation = entity.FuelDropLocation.ToDispatchLocationViewModel();

            if (entity.BolDetails != null)
                viewModel.BolDetails = entity.ToBolDetailsViewModel();

            return viewModel;
        }
    }
}
