using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class FuelDispatchLocationMapper
    {
        public static FuelDispatchLocation ToFuelDispatchLocationEntity(this DispatchLocationViewModel viewModel, FuelDispatchLocation entity = null)
        {
            if (entity == null)
                entity = new FuelDispatchLocation();

            entity.IsActive = true;
            entity.Latitude = viewModel.Latitude;
            entity.Longitude = viewModel.Longitude;
            entity.Address = viewModel.Address;
            entity.City = viewModel.City;
            entity.StateCode = viewModel.StateCode;
            entity.StateId = viewModel.StateId;
            entity.CountryCode = viewModel.CountryCode;
            entity.CountyName = viewModel.CountyName;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.OrderId = viewModel.OrderId;
            entity.LocationType = viewModel.LocationType;
            entity.ZipCode = viewModel.ZipCode;
            entity.Currency = viewModel.Currency;
            entity.DeliveryScheduleId = viewModel.DeliveryScheduleId;
            entity.TrackableScheduleId = viewModel.TrackableScheduleId;
            entity.TimeZoneName = viewModel.TimeZoneName;
            return entity;
        }

        public static FuelDispatchLocation ToFuelDispatchLocationEntity(this SplitLoadAddressViewModel viewModel, FuelDispatchLocation entity = null)
        {
            if (entity == null)
                entity = new FuelDispatchLocation();

            entity.Id = viewModel.Id;
            entity.Latitude = viewModel.Latitude;
            entity.Longitude = viewModel.Longitude;
            entity.Address = viewModel.Address;
            entity.City = viewModel.City;
            entity.StateCode = viewModel.StateCode;
            entity.StateId = viewModel.StateId;
            entity.CountryCode = viewModel.CountryCode;
            entity.DropStatus = DropAddressStatus.Pending;
            entity.IsActive = true;
            entity.CreatedDate = DateTimeOffset.Now;
            entity.SiteName = viewModel.SiteName;
            entity.CountyName = viewModel.CountyName;
            entity.LocationType = viewModel.LocationTypeId;
            entity.ZipCode = viewModel.ZipCode;
            entity.TimeZoneName = viewModel.TimeZoneName;
            return entity;
        }

        public static SplitLoadAddressViewModel ToViewModel(this FuelDispatchLocation entity)
        {
            SplitLoadAddressViewModel viewModel = new SplitLoadAddressViewModel();

            viewModel.Id = entity.Id;
            viewModel.Address = entity.Address;
            viewModel.City = entity.City;
            viewModel.StateCode = entity.StateCode;
            viewModel.StateId = entity.StateId.Value;
            viewModel.ZipCode = entity.ZipCode;
            viewModel.CountryCode = entity.CountryCode;
            viewModel.Latitude = entity.Latitude;
            viewModel.Longitude = entity.Longitude;
            viewModel.CountyName = entity.CountyName;
            viewModel.Currency = entity.Currency;
            viewModel.TimeZoneName = entity.TimeZoneName;
            viewModel.SiteName = entity.SiteName;
            return viewModel;
        }

        public static SplitLoadAddressViewModel ToViewModel(this UspGetOrderScheduleDetailsViewModel entity)
        {
            SplitLoadAddressViewModel viewModel = new SplitLoadAddressViewModel();

            viewModel.Id = entity.LocationId.Value;
            viewModel.Address = entity.Address;
            viewModel.City = entity.City;
            viewModel.StateCode = entity.StateCode;
            viewModel.StateId = entity.StateId.Value;
            viewModel.ZipCode = entity.ZipCode;
            viewModel.CountryCode = entity.CountryCode;
            viewModel.Latitude = entity.Latitude.Value;
            viewModel.Longitude = entity.Longitude.Value;
            viewModel.CountyName = entity.CountyName;
            viewModel.Currency = entity.Currency.Value;
            viewModel.TimeZoneName = entity.TimeZoneName;
            viewModel.SiteName = entity.SiteName;
            return viewModel;
        }

        public static FuelDispatchLocation ToFuelDispatchLocationEntity(this PickUpAddressViewModel viewModel, FuelDispatchLocation entity = null)
        {
            if (entity == null)
                entity = new FuelDispatchLocation();

            entity.IsActive = true;
            entity.Latitude = viewModel.Address.Latitude;
            entity.Longitude = viewModel.Address.Longitude;
            entity.Address = viewModel.Address.Address;
            entity.City = viewModel.Address.City;
            entity.StateCode = viewModel.Address.State.Code;
            entity.StateId = viewModel.Address.State.Id;
            entity.CountryCode = viewModel.Address.Country.Code;
            entity.CountyName = viewModel.Address.CountyName;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.OrderId = viewModel.OrderId;
            entity.LocationType = (int)LocationType.PickUp;
            entity.ZipCode = viewModel.Address.ZipCode;
            entity.Currency = viewModel.Currency;
            entity.TimeZoneName = viewModel.Address.TimeZoneName;
            entity.SiteName = viewModel.Address.SiteName;
            return entity;
        }

        public static PickUpAddressViewModel ToPickUpAddressViewModel(this FuelDispatchLocation entity, PickUpAddressViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new PickUpAddressViewModel() { Address = new DispatchAddressViewModel() };

            viewModel.Address.Latitude = entity.Latitude;
            viewModel.Address.Longitude = entity.Longitude;
            viewModel.Address.Address = entity.Address;
            viewModel.Address.City = entity.City;
            viewModel.Address.State = new StateViewModel { Code = entity.StateCode, Id = entity.StateId ?? 0 };
            viewModel.Address.Country = new CountryViewModel { Code = entity.CountryCode, Id = entity.MstState?.CountryId ?? 1 };
            viewModel.Address.CountyName = entity.CountyName;
            viewModel.CreatedBy = entity.CreatedBy;
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.OrderId = entity.OrderId ?? 0;
            viewModel.Address.ZipCode = entity.ZipCode;
            viewModel.Currency = entity.Currency;
            viewModel.Address.TimeZoneName = entity.TimeZoneName;
            viewModel.Address.SiteName = entity.SiteName;
            viewModel.Address.SiteId = entity.Id;
            viewModel.Address.CountryGroup = new DropdownDisplayExtendedItem { Id= entity.MstState.CountryGroupId ?? 0};
            return viewModel;
        }

        public static FuelDispatchLocation ToFuelDispatchLocationEntity(this DeliveryGroupPickupViewModel viewModel, FuelDispatchLocation entity = null)
        {
            if (entity == null)
                entity = new FuelDispatchLocation();

            entity.IsActive = true;
            entity.Latitude = viewModel.PickupLatitude ?? 0;
            entity.Longitude = viewModel.PickupLongitude ?? 0;
            entity.Address = viewModel.PickupAddress;
            entity.City = viewModel.PickupCity;
            entity.StateCode = viewModel.PickupStateCode;
            entity.StateId = viewModel.PickupStateId;
            entity.CountryCode = viewModel.PickupCountryCode;
            entity.CountyName = viewModel.PickupCountyName;
            entity.LocationType = (int)LocationType.PickUp;
            entity.ZipCode = viewModel.PickupZipCode;
            entity.Currency = Currency.USD;
            entity.TimeZoneName = viewModel.PickupTimeZone;
            entity.TerminalId = viewModel.TerminalId;
            entity.SiteName = viewModel.SiteName;
            return entity;
        }

        public static DeliveryGroupPickupViewModel ToPickUpLocationViewModel(this FuelDispatchLocation entity, string terminalName = null, DeliveryGroupPickupViewModel viewModel = null)
        {

            if (viewModel == null)
                viewModel = new DeliveryGroupPickupViewModel();

            if (entity == null)
                return null;

            viewModel.PickupLatitude = entity.Latitude;
            viewModel.PickupLongitude = entity.Longitude;
            viewModel.PickupAddress = entity.Address;
            viewModel.PickupCity = entity.City;
            viewModel.PickupStateCode = entity.StateCode;
            viewModel.PickupStateId = entity.StateId;
            viewModel.PickupCountryCode = entity.CountryCode;
            viewModel.PickupCountryId = entity.CountryCode.ToLower().Contains("ca") ? (int)Country.CAN : (int)Country.USA;
            viewModel.PickupCountyName = entity.CountyName;
            viewModel.PickupZipCode = entity.ZipCode;
            viewModel.PickupTimeZone = entity.TimeZoneName;
            viewModel.TerminalId = entity.TerminalId;
            viewModel.TerminalName = terminalName;
            viewModel.SiteName = entity.SiteName;
            viewModel.IsPickupLocation = !entity.TerminalId.HasValue;

            return viewModel;
        }

        public static FuelDispatchLocation ToDispatchLocationEntity(this DropAddressViewModel viewModel, FuelDispatchLocation entity = null)
        {
            if (entity == null)
                entity = new FuelDispatchLocation();

            entity.IsActive = true;
            entity.Latitude = viewModel.Latitude;
            entity.Longitude = viewModel.Longitude;
            entity.Address = viewModel.Address;
            entity.City = viewModel.City;
            entity.StateCode = viewModel.State.Code;
            entity.StateId = viewModel.State.Id;
            entity.CountryCode = viewModel.Country.Code;
            entity.CountyName = viewModel.CountyName;
            entity.LocationType = (int)LocationType.PickUp;
            entity.ZipCode = viewModel.ZipCode;
            entity.SiteName = viewModel.SiteName;
            return entity;
        }

        public static FuelDispatchLocation ToEntity(this FuelDispatchLocationViewModel viewModel, FuelDispatchLocation entity = null)
        {
            if (entity == null)
                entity = new FuelDispatchLocation();
            //var pickupAddress = viewModel.PickupLocation.Address;

            entity.IsActive = true;
            entity.LocationType = (int)viewModel.LocationType;
            entity.OrderId = viewModel.OrderId;
            entity.DeliveryScheduleId = viewModel.DeliveryScheduleId;
            entity.TrackableScheduleId = viewModel.TrackableScheduleId;
            entity.TerminalId = viewModel.TerminalId;
            entity.IsFuturePickUp = viewModel.IsFuturePickUp;
            entity.SiteName = viewModel.SiteName;
            entity.Currency = viewModel.Currency;
            entity.TimeZoneName = viewModel.TimeZoneName;
            entity.DropStatus = viewModel.DropStatus;
            entity.IsJobLocation = viewModel.IsJobLocation;
            entity.ParentId = viewModel.ParentId;
            entity.IsSkipped = viewModel.IsSkipped;
            entity.DeliveryGroupId = viewModel.DeliveryGroupId;
            //entity.Latitude = pickupAddress.Latitude;
            //entity.Longitude = pickupAddress.Longitude;
            //entity.Address = pickupAddress.Address;
            //entity.City = pickupAddress.City;
            //entity.StateCode = pickupAddress.State.Code;
            //entity.StateId = pickupAddress.State.Id;
            //entity.CountryCode = pickupAddress.Country.Code;
            //entity.CountyName = pickupAddress.CountyName;            
            //entity.ZipCode = pickupAddress.ZipCode;
            entity.CreatedDate = DateTimeOffset.Now;
            entity.IsActive = true;

            return entity;
        }

        public static FuelDispatchLocationViewModel ToViewModel(this FuelDispatchLocation entity, FuelDispatchLocationViewModel viewModel)
        {
            viewModel.PickupLocation = entity.ToPickUpAddressViewModel();

            viewModel.LocationType = (LocationType)entity.LocationType;
            viewModel.OrderId = entity.OrderId;
            viewModel.DeliveryScheduleId = entity.DeliveryScheduleId;
            viewModel.TrackableScheduleId = entity.TrackableScheduleId;
            viewModel.TerminalId = entity.TerminalId;
            viewModel.IsFuturePickUp = entity.IsFuturePickUp;
            viewModel.SiteName = entity.SiteName;
            viewModel.Currency = entity.Currency;
            viewModel.TimeZoneName = entity.TimeZoneName;
            viewModel.DropStatus = entity.DropStatus;
            viewModel.IsJobLocation = entity.IsJobLocation;
            viewModel.ParentId = entity.ParentId;
            viewModel.IsSkipped = entity.IsSkipped;
            viewModel.DeliveryGroupId = entity.DeliveryGroupId;
            viewModel.CreatedDate = entity.CreatedDate;

            return viewModel;
        }
        public static FuelDispatchLocation ToOptionalDispatchLocationEntity(this OptionalBulkPlantAddressModel viewModel, FuelDispatchLocation entity = null)
        {
            if (entity == null)
                entity = new FuelDispatchLocation();

            entity.IsActive = true;
            entity.Latitude = viewModel.Latitude;
            entity.Longitude = viewModel.Longitude;
            entity.Address = viewModel.Address;
            entity.City = viewModel.City;
            entity.StateCode = viewModel.State.Code;
            entity.StateId = viewModel.State.Id;
            entity.CountryCode = viewModel.Country.Code;
            entity.CountyName = viewModel.CountyName;
            entity.LocationType = (int)LocationType.PickUp;
            entity.ZipCode = viewModel.ZipCode;
            entity.SiteName = viewModel.SiteName;
            return entity;
        }
    }
}
