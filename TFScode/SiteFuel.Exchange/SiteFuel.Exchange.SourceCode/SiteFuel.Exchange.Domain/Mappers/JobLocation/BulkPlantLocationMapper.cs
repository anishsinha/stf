using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class BulkPlantLocationMapper
    {
        public static BulkPlantLocation ToBulkPlantLocationEntity(this BolDetailViewModel viewModel, int countryId, int companyId)
        {
            var entity = new BulkPlantLocation()
            {
                Name = viewModel.SiteName,
                Latitude = viewModel.Latitude,
                Longitude = viewModel.Longitude,
                Address = viewModel.Address,
                City = viewModel.City,
                StateCode = viewModel.StateCode,
                StateId = viewModel.StateId ?? 0,
                CountryCode = viewModel.CountryCode,
                CountyName = viewModel.CountyName,
                CreatedBy = viewModel.CreatedBy,
                CreatedDate = viewModel.CreatedDate,
                ZipCode = viewModel.ZipCode,
                CountryId = countryId,
                CompanyId = companyId,
                IsActive = true,
            };
            return entity;
        }

        public static BulkPlantLocation ToBulkPlantLocationEntity(this PreLoadBolDetail viewModel, int countryId, int companyId)
        {
            var entity = new BulkPlantLocation()
            {
                Name = viewModel.SiteName,
                Latitude = viewModel.Latitude,
                Longitude = viewModel.Longitude,
                Address = viewModel.Address,
                City = viewModel.City,
                StateCode = viewModel.StateCode,
                StateId = viewModel.StateId ?? 0,
                CountryCode = viewModel.CountryCode,
                CountyName = viewModel.CountyName,
                CreatedBy = viewModel.CreatedBy,
                CreatedDate = viewModel.CreatedDate,
                ZipCode = viewModel.ZipCode,
                CountryId = countryId,
                CompanyId = companyId,
                IsActive = true,
            };
            return entity;
        }

        public static BulkPlantLocation ToBulkPlantLocationEntity(this FuelDispatchLocation viewModel, int countryId, int companyId)
        {
            var entity = new BulkPlantLocation()
            {
                Name = viewModel.SiteName,
                Latitude = viewModel.Latitude,
                Longitude = viewModel.Longitude,
                Address = viewModel.Address,
                AddressLine2 = viewModel.AddressLine2,
                AddressLine3 = viewModel.AddressLine3,
                City = viewModel.City,
                StateCode = viewModel.StateCode,
                StateId = viewModel.StateId ?? 0,
                CountryCode = viewModel.CountryCode,
                CountyName = viewModel.CountyName,
                CreatedBy = viewModel.CreatedBy,
                CreatedDate = viewModel.CreatedDate,
                ZipCode = viewModel.ZipCode,
                CountryId = countryId,
                CompanyId = companyId,
                IsActive = true,
            };
            return entity;
        }

        public static BulkPlantLocation ToBulkPlantLocationEntity(this PickupLocationDetailViewModel viewModel, UserContext userContext)
        {
            var entity = new BulkPlantLocation()
            {
                Name = viewModel.Name,
                Latitude = viewModel.Latitude,
                Longitude = viewModel.Longitude,
                Address = viewModel.Address,
                City = viewModel.City,
                StateCode = viewModel.StateCode,
                StateId = viewModel.StateId,
                CountryCode = viewModel.CountryCode,
                CountyName = viewModel.County,
                CreatedBy = userContext.Id,
                CreatedDate = DateTimeOffset.Now,
                ZipCode = viewModel.ZipCode,
                CountryId = viewModel.CountryId,
                CompanyId = userContext.CompanyId,
                IsActive = true,
            };
            return entity;
        }

        public static MstExternalTerminal ToTerminalEntity(this PickupLocationDetailViewModel viewModel, UserContext userContext, MstExternalTerminal entity = null)
        {
            if (entity == null)
                entity = new MstExternalTerminal();

                entity.Id = viewModel.Id;
                entity.Name = viewModel.Name;
                entity.Latitude = viewModel.Latitude;
                entity.Longitude = viewModel.Longitude;
                entity.Address = viewModel.Address;
                entity.City = viewModel.City;
                entity.StateCode = viewModel.StateCode;
                entity.StateId = viewModel.StateId;
                entity.CountryCode = viewModel.CountryCode;
                entity.CountyName = viewModel.County;
                entity.ZipCode = viewModel.ZipCode;
                entity.Abbreviation = viewModel.Abbreviation;
                entity.Code = viewModel.Abbreviation;
                entity.ControlNumber = viewModel.ControlNumber;
                entity.Currency = viewModel.CountryCode.ToLower().Equals("usa") || viewModel.CountryCode.ToLower().Equals("us") ? Currency.USD : Currency.CAD;
                entity.PricingSourceId = (int)PricingSource.Axxis;
                entity.UpdatedBy = userContext.Id;
                entity.UpdatedDate = DateTimeOffset.Now;
                entity.IsActive = true;

            return entity;
        }
    }
}
