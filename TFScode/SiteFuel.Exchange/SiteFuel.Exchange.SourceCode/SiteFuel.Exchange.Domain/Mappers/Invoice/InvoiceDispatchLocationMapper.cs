using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class InvoiceDispatchLocationMapper
    {
        public static InvoiceDispatchLocation ToEntity(this DispatchLocationViewModel viewModel, InvoiceDispatchLocation entity = null)
        {
            if (entity == null)
                entity = new InvoiceDispatchLocation();

            entity.Latitude = viewModel.Latitude;
            entity.Longitude = viewModel.Longitude;
            entity.Address = viewModel.Address;
            entity.AddressLine2 = viewModel.AddressLine2;
            entity.AddressLine3 = viewModel.AddressLine3;
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
            entity.SiteName = viewModel.SiteName;
            entity.PickupLocation = viewModel.PickupLocationType;
            return entity;
        }

        public static DropAddressViewModel ToDispatchAddress(this InvoiceDispatchLocation entity, DropAddressViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new DropAddressViewModel();

            viewModel.Address = entity.Address;
            viewModel.AddressLine2 = entity.AddressLine2;
            viewModel.AddressLine3 = entity.AddressLine3;
            viewModel.City = entity.City;
            viewModel.CountyName = entity.CountyName;
            viewModel.Country.Code = entity.CountryCode;
            viewModel.Country.Name = entity.CountyName;
            viewModel.Latitude = entity.Latitude;
            viewModel.Longitude = entity.Longitude;
            viewModel.State.Id = entity.StateId ?? 0;
            viewModel.State.Code = entity.StateCode;
            viewModel.ZipCode = entity.ZipCode;
            viewModel.IsAddressAvailable = !string.IsNullOrWhiteSpace(entity.Address) && !string.IsNullOrWhiteSpace(entity.City) && !string.IsNullOrWhiteSpace(entity.ZipCode);
            return viewModel;
        }

        public static DropAddressViewModel ToDispatchAddress(this InvoiceXAdditionalDetail entity, DropAddressViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new DropAddressViewModel();

            viewModel.Country.Code = entity.JobCountryCode;
            viewModel.Country.Name = entity.JobCountryName;
            viewModel.Latitude = entity.Latitude;
            viewModel.Longitude = entity.Longitude;
            //viewModel.State.Id = entity
            viewModel.State.Code = entity.JobStateCode;

            return viewModel;
        }

        public static DropAddressViewModel ToPickUpLocation(this InvoiceDispatchLocation entity)
        {
            if (entity != null)
            {
                var pickupLocation = new DropAddressViewModel();

                pickupLocation.Address = entity.Address;
                pickupLocation.AddressLine2 = entity.AddressLine2;
                pickupLocation.AddressLine3 = entity.AddressLine3;
                pickupLocation.City = entity.City;
                pickupLocation.Country = new CountryViewModel() { Code = entity.CountryCode };
                pickupLocation.CountyName = entity.CountyName;
                pickupLocation.Latitude = entity.Latitude;
                pickupLocation.Longitude = entity.Longitude;
                pickupLocation.State = new StateViewModel() { Code = entity.StateCode, Id = entity.StateId.Value };
                pickupLocation.ZipCode = entity.ZipCode;
                pickupLocation.IsAddressAvailable = true;               
                pickupLocation.SiteName = entity.SiteName;
                
                return pickupLocation;
            }
            return null;
        }
        public static DropAddressViewModel ToPickUpLocation(this BolDetailViewModel entity, DropAddressViewModel pickupLocation)
        {
            if (pickupLocation == null)
                pickupLocation = new DropAddressViewModel();

            if (entity != null)
            {
                pickupLocation.Address = entity.Address;
                pickupLocation.AddressLine2 = entity.AddressLine2;
                pickupLocation.AddressLine3 = entity.AddressLine3;
                pickupLocation.City = entity.City;
                pickupLocation.Country = new CountryViewModel() { Id=entity.CountryId, Code = entity.CountryCode };
                pickupLocation.CountyName = entity.CountyName;
                pickupLocation.Latitude = entity.Latitude;
                pickupLocation.Longitude = entity.Longitude;
                pickupLocation.State = new StateViewModel() { Code = entity.StateCode, Id = entity.StateId.Value };
                pickupLocation.ZipCode = entity.ZipCode;
                pickupLocation.IsAddressAvailable = entity.IsLiftInfoAvailable();
                pickupLocation.SiteName = entity.SiteName;
            }
            return pickupLocation;
        }
        public static DispatchLocationViewModel ToPickUpLocation(this DropAddressViewModel entity)
        {
            if (entity != null)
            {
                var pickupLocation = new DispatchLocationViewModel();

                pickupLocation.Address = entity.Address;
                pickupLocation.AddressLine2 = entity.AddressLine2;
                pickupLocation.AddressLine3 = entity.AddressLine3;
                pickupLocation.City = entity.City;
                pickupLocation.CountyName = entity.CountyName;
                pickupLocation.Latitude = entity.Latitude;
                pickupLocation.Longitude = entity.Longitude;
                pickupLocation.ZipCode = entity.ZipCode;
                pickupLocation.CountryCode = entity.Country?.Code;
                pickupLocation.StateCode = entity.State?.Code;
                pickupLocation.StateId = entity.State.Id;
                pickupLocation.CreatedDate = DateTimeOffset.Now;
                return pickupLocation;
            }
            return null;
        }

        public static AddressViewModel ToAddressViewModel(this DispatchLocationViewModel entity)
        {
            AddressViewModel viewModel = new AddressViewModel();

            viewModel.Address = entity.Address;
            viewModel.AddressLine2 = entity.AddressLine2;
            viewModel.AddressLine3 = entity.AddressLine3;
            viewModel.City = entity.City;
            viewModel.CountryCode = entity.CountryCode;
            viewModel.StateCode = entity.StateCode;
            viewModel.ZipCode = entity.ZipCode;
            viewModel.CountyName = entity.CountyName;

            return viewModel;
        }

        public static DispatchLocationViewModel ToDispatchLocation(this InvoiceDispatchLocation entity)
        {
            if (entity != null)
            {
                var location = new DispatchLocationViewModel
                {
                    Address = entity.Address,
                    AddressLine2 = entity.AddressLine2,
                    AddressLine3 = entity.AddressLine3,
                    City = entity.City,
                    CountryCode = entity.CountryCode,
                    CountyName = entity.CountyName,
                    CreatedBy = entity.CreatedBy,
                    CreatedDate = entity.CreatedDate,
                    Latitude = entity.Latitude,
                    LocationType = entity.LocationType,
                    Longitude = entity.Longitude,
                    OrderId = entity.OrderId,
                    PickupLocationType = entity.PickupLocation,
                    SiteName = entity.SiteName,
                    StateCode = entity.StateCode,
                    StateId= entity.StateId ?? 0,
                    ZipCode = entity.ZipCode,
                };
                return location;
            }
            return null;
        }
    }
}
