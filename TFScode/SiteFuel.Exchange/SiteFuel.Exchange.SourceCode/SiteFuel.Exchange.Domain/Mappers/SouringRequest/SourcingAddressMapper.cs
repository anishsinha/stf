using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class SourcingAddressMapper
    {
        public static SourcingAddressViewModel ToSourceViewModel(this Job entity, SourcingAddressViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new SourcingAddressViewModel(Status.Success);
            viewModel.JobId = entity.Id;
            viewModel.JobName = entity.Name;
            viewModel.DisplayJobID = entity.DisplayJobID;
            viewModel.Address = entity.Address;
            viewModel.City = entity.City;
            viewModel.StateId = entity.StateId;
            viewModel.CountryId = entity.CountryId;
            viewModel.Currency = entity.Currency;
            if (!entity.IsMarine)
                viewModel.UOM = entity.UoM;
            viewModel.ZipCode = entity.ZipCode;
            viewModel.CountyName = entity.CountyName;
            viewModel.Latitude = Convert.ToString(entity.Latitude);
            viewModel.Longitude = Convert.ToString(entity.Longitude);
            viewModel.TimeZoneName = entity.TimeZoneName;
            viewModel.IsGeocodeUsed = entity.IsGeocodeUsed;
            viewModel.SignatureEnabled = entity.SignatureEnabled;
            viewModel.IsProFormaPoEnabled = entity.IsProFormaPoEnabled;
            viewModel.IsRetailJob = entity.IsRetailJob;
            viewModel.SignatureEnabled = entity.SignatureEnabled;
            viewModel.IsMarineLocation = entity.IsMarine;
            viewModel.LocationManagedType = entity.LocationManagedType;
            viewModel.InventoryDataCaptureType = entity.InventoryDataCaptureType;
            return viewModel;
        }
    }
}
