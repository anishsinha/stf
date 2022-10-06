using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class CompanyAddressMapper
    {
        public static CompanyAddressViewModel ToViewModel(this CompanyAddress entity, CompanyAddressViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new CompanyAddressViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.Address = entity.Address;
            viewModel.Address2 = entity.AddressLine2;
            viewModel.Address3 = entity.AddressLine3;
            viewModel.City = entity.City;
            viewModel.State = entity.MstState.ToViewModel();
            viewModel.Country = entity.MstCountry.ToViewModel();
            viewModel.ZipCode = entity.ZipCode;
            viewModel.Latitude = entity.Latitude;
            viewModel.Longitude = entity.Longitude;
            viewModel.Phone = new PhoneViewModel(Status.Success)
            {
                PhoneType = entity.PhoneTypeId,
                PhoneNumber = entity.PhoneNumber
            };
            viewModel.CompanyId = entity.CompanyId;
            viewModel.IsDefault = entity.IsDefault;
            viewModel.IsActive = entity.IsActive;
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;
            viewModel.CompanyTypeId = (CompanyType)entity.Company.CompanyTypeId;
            return viewModel;
        }

        public static SupplierProfileViewModel ToSupplierProfileViewModel(this CompanyAddress entity, SupplierProfileViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new SupplierProfileViewModel(Status.Success);

            if (entity.MstStates.Any())
            {
                viewModel.ServingStates = entity.MstStates.Select(t => t.Id).ToList();
            }
            if (entity.MstSupplierQualifications.Any())
            {
                viewModel.SupplierQualifications = entity.MstSupplierQualifications.Select(t => t.Id).ToList();
            }
            if (entity.SupplierAddressXSetting != null)
            {
                viewModel.AddressId = entity.SupplierAddressXSetting.AddressId;
                viewModel.IsStateWideService = entity.SupplierAddressXSetting.IsStateWideService;
                viewModel.Radius = entity.SupplierAddressXSetting.Radius;
                viewModel.IsHedgeOrderAllowed = entity.SupplierAddressXSetting.IsHedgeOrderAllowed;
                viewModel.IsLocationOwned = entity.SupplierAddressXSetting.IsLocationOwned;
                viewModel.IsOverWaterRefuelingAllowed = entity.SupplierAddressXSetting.IsOverWaterRefuelingAllowed;
            }

            return viewModel;
        }

        public static CompanyAddress ToEntity(this CompanyAddressViewModel viewModel, CompanyAddress entity = null)
        {
            if (entity == null)
                entity = new CompanyAddress();

            entity.Id = viewModel.Id;
            entity.Address = viewModel.Address;
            entity.City = viewModel.City;
            entity.StateId = viewModel.State.Id;
            entity.CountryId = viewModel.Country.Id;
            entity.ZipCode = viewModel.ZipCode;
            entity.Latitude = viewModel.Latitude;
            entity.Longitude = viewModel.Longitude;
            entity.PhoneTypeId = viewModel.Phone.PhoneType;
            entity.PhoneNumber = viewModel.Phone.PhoneNumber;
            entity.CompanyId = viewModel.CompanyId;
            entity.IsDefault = viewModel.IsDefault;

            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.AddressLine2 = viewModel.Address2;
            entity.AddressLine3 = viewModel.Address3;

            return entity;
        }

        public static BillingAddress ToEntity(this BillingAddressViewModel viewModel, BillingAddress entity = null)
        {
            if (entity == null)
                entity = new BillingAddress();

            entity.Id = viewModel.Id;
            entity.Name = viewModel.Name;
            entity.Address = viewModel.Address;
            entity.City = viewModel.City;
            entity.County = viewModel.County;
            entity.StateName = viewModel.State.Name;
            //entity.StateId = viewModel.State.Id;
            //entity.CountryId = viewModel.Country.Id;
            entity.CountryName = viewModel.Country.Name;
            entity.ZipCode = viewModel.ZipCode;
            entity.Latitude = viewModel.Latitude;
            entity.Longitude = viewModel.Longitude;
            entity.PhoneTypeId = viewModel.Phone.PhoneType;
            entity.PhoneNumber = viewModel.Phone.PhoneNumber;
            entity.CompanyId = viewModel.CompanyId;

            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.IsDefault = viewModel.IsDefault;
            entity.AddressLine2 = viewModel.Address2;
            entity.AddressLine3 = viewModel.Address3;

            return entity;
        }

        public static BillingAddressViewModel ToViewModel(this BillingAddress entity, BillingAddressViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new BillingAddressViewModel();

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.Address = entity.Address;
            viewModel.City = entity.City;
            viewModel.County = entity.County;
            //viewModel.State.Id = entity.StateId ?? 0;
            viewModel.State.Name = entity.StateName;
            //viewModel.Country.Id = entity.CountryId ?? 0;
            viewModel.Country.Name = entity.CountryName;
            viewModel.ZipCode = entity.ZipCode;
            viewModel.Latitude = entity.Latitude ?? 0;
            viewModel.Longitude = entity.Longitude ?? 0;
            viewModel.Phone.PhoneType = entity.PhoneTypeId;
            viewModel.Phone.PhoneNumber = entity.PhoneNumber;
            viewModel.CompanyId = entity.CompanyId;
            viewModel.IsActive = entity.IsActive;
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;
            viewModel.IsDefault = entity.IsDefault;
            viewModel.Address2 = entity.AddressLine2;
            viewModel.Address3 = entity.AddressLine3;


            return viewModel;
        }

        public static BillingAddress ToBillingAddressEntityFromTPO(this JobSpecificBillToViewModel viewModel, BillingAddress entity = null)
        {
            if (entity == null)
                entity = new BillingAddress();

            var isBillingToExists = (!string.IsNullOrWhiteSpace(viewModel.Address));
            if (isBillingToExists)
            {
                entity.Name = viewModel.Name;
                entity.Address = viewModel.Address;
                entity.AddressLine2 = viewModel.AddressLine2;
                entity.AddressLine3 = viewModel.AddressLine3;
                entity.City = viewModel.City;
                entity.StateName = viewModel.State.Name;
                entity.StateId = viewModel.State.Id;
                entity.ZipCode = viewModel.ZipCode;
                entity.County = viewModel.County;
                entity.CountryName = viewModel.Country.Name;
                entity.CountryId = viewModel.Country.Id;
                entity.CompanyId = viewModel.CompanyId.Value;
                entity.PhoneTypeId = 1;
                entity.IsActive = true;
            }
            return entity;
        }

        public static SupplierAddressXSetting ToEntity(this SupplierProfileViewModel viewModel, SupplierAddressXSetting entity = null)
        {
            if (entity == null)
                entity = new SupplierAddressXSetting();

            entity.AddressId = viewModel.AddressId;
            entity.IsHedgeOrderAllowed = viewModel.IsHedgeOrderAllowed;
            entity.IsLocationOwned = viewModel.IsLocationOwned;
            entity.IsOverWaterRefuelingAllowed = viewModel.IsOverWaterRefuelingAllowed;
            entity.IsStateWideService = viewModel.IsStateWideService;
            entity.Radius = viewModel.Radius;

            return entity;
        }

        public static CompanyAddress ToCompanyAddressEntity(this ThirdPartyOrderViewModel viewModel, CompanyAddress entity = null)
        {
            if (entity == null)
                entity = new CompanyAddress();

            if (viewModel.AddressDetails.IsVarious && viewModel.AddressDetails.Country.Id != (int)Country.CAR)
            {
                viewModel.AddressDetails.Address = viewModel.BillingAddress.Address;
                viewModel.AddressDetails.AddressLine2 = viewModel.BillingAddress.AddressLine2;
                viewModel.AddressDetails.AddressLine3 = viewModel.BillingAddress.AddressLine3;
                viewModel.AddressDetails.City = viewModel.BillingAddress.City;
                viewModel.AddressDetails.ZipCode = viewModel.BillingAddress.ZipCode;
                viewModel.AddressDetails.CountyName = viewModel.BillingAddress.County;
                entity.StateId = viewModel.AddressDetails.State.Id;
            }
            else if(viewModel.AddressDetails.Country.Id == (int)Country.CAR)
            {
                if(string.IsNullOrWhiteSpace(viewModel.AddressDetails.Address))
                    viewModel.AddressDetails.Address = viewModel.BillingAddress.Address ?? viewModel.AddressDetails.State.Name ?? Resource.lblCaribbean;
                if (string.IsNullOrWhiteSpace(viewModel.AddressDetails.AddressLine2))
                    viewModel.AddressDetails.Address = viewModel.BillingAddress.AddressLine2 ?? viewModel.AddressDetails.State.Name ?? Resource.lblCaribbean;
                if (string.IsNullOrWhiteSpace(viewModel.AddressDetails.AddressLine3))
                    viewModel.AddressDetails.Address = viewModel.BillingAddress.AddressLine3 ?? viewModel.AddressDetails.State.Name ?? Resource.lblCaribbean;
                if (string.IsNullOrWhiteSpace(viewModel.AddressDetails.City))
                    viewModel.AddressDetails.City = viewModel.BillingAddress.City ?? viewModel.AddressDetails.State.Name ?? Resource.lblCaribbean;
                if (string.IsNullOrWhiteSpace(viewModel.AddressDetails.ZipCode))
                    viewModel.AddressDetails.ZipCode = viewModel.BillingAddress.ZipCode ?? viewModel.AddressDetails.State.Name ?? Resource.lblCaribbean;
                if (string.IsNullOrWhiteSpace(viewModel.AddressDetails.CountyName))
                    viewModel.AddressDetails.CountyName = viewModel.BillingAddress.County ?? viewModel.AddressDetails.CountyName ?? Resource.lblCaribbean;
                entity.StateId = viewModel.AddressDetails.State.Id;
            }
            else
            {
                entity.StateId = viewModel.AddressDetails.State.Id;
            }
            entity.Id = viewModel.CustomerDetails.CompanyId ?? 0;
            entity.Address = viewModel.AddressDetails.Address;
            entity.AddressLine2 = viewModel.AddressDetails.AddressLine2;
            entity.AddressLine3 = viewModel.AddressDetails.AddressLine3;
            entity.City = viewModel.AddressDetails.City;

            entity.CountryId = viewModel.AddressDetails.Country.Id;
            entity.ZipCode = viewModel.AddressDetails.ZipCode;
            entity.Latitude = viewModel.AddressDetails.Latitude;
            entity.Longitude = viewModel.AddressDetails.Longitude;
            entity.PhoneTypeId = 1;
            entity.PhoneNumber = viewModel.CustomerDetails.PhoneNumber;
            entity.CompanyId = viewModel.CustomerDetails.CompanyId ?? 0;
            entity.IsDefault = true;

            entity.IsActive = true;
            entity.UpdatedBy = (int)viewModel.CustomerDetails.UserId;
            entity.UpdatedDate = viewModel.UpdatedDate;

            return entity;
        }

    }
}
