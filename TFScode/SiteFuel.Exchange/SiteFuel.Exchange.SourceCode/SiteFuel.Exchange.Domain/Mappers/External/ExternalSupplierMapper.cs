using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class ExternalSupplierMapper
    {
        public static ExternalSupplierAddressDetail ToViewModel(this ExternalSupplierAddress entity, ExternalSupplierAddressDetail viewModel = null)
        {
            if (viewModel == null)
                viewModel = new ExternalSupplierAddressDetail();

            if (entity != null)
            {
                viewModel.Address = entity.Address;
                viewModel.City = entity.City;
                viewModel.ZipCode = entity.ZipCode;
                //viewModel.State =  entity.StateId;
                //viewModel.PhoneType = entity.PhoneTypeId;
                viewModel.PhoneNumber = entity.PhoneNumber;
                viewModel.NumberOfTrucks = entity.NumberOfTrucks;
                viewModel.SupplierDBE = entity.MstSupplierQualifications.Select(t => t.Name).ToList();
                viewModel.SupplierProductTypes = string.Join(", ", entity.MstProductTypes.Select(t => t.Name).ToList());
                viewModel.SupplierTrucks = entity.ExternalSupplierAddressTruckTypes.Select(t => t.TruckTypeId).ToList();
                viewModel.SupplierServingStates = string.Join(", ", entity.MstStates.Select(t => t.Name).ToList());
            }

            return viewModel;
        }

        public static ServicesViewModel ToViewModel(this ExternalSupplierAddress entity, ServicesViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new ServicesViewModel();

            if (entity != null)
            {
                viewModel.NumberOfTrucks = entity.NumberOfTrucks;
                viewModel.IsStateWideService = entity.IsStateWideService;
                viewModel.Radius = entity.Radius;
                viewModel.ServingStates = entity.MstStates.Select(t => t.Id).ToList();
                viewModel.NumberOfTrucks = entity.NumberOfTrucks;
                viewModel.SupplierQualifications = entity.MstSupplierQualifications.Select(t => t.Id).ToList();
                viewModel.BobtailTransportTankWagon = entity.ExternalSupplierAddressTruckTypes.Select(t => t.TruckTypeId).ToList();
                viewModel.AddressId = entity.Id;
            }

            return viewModel;
        }

        public static List<ExternalSupplierAddressDetail> ToViewModel(this ICollection<ExternalSupplierAddress> entity, List<ExternalSupplierAddressDetail> viewModel = null)
        {
            if (viewModel == null)
                viewModel = new List<ExternalSupplierAddressDetail>();

            if (entity != null && entity.Count > 0)
            {
                foreach (var item in entity)
                {
                    viewModel.Add(new ExternalSupplierAddressDetail()
                    {
                        Address = item.Address,
                        City = item.City,
                        ZipCode = item.ZipCode,
                        NumberOfTrucks = item.NumberOfTrucks,
                        PhoneNumber = item.PhoneNumber,
                        SupplierDBE = item.MstSupplierQualifications.Select(t => t.Name).ToList(),
                        SupplierProductTypes = string.Join(", ", item.MstProductTypes.Select(t => t.Name).ToList()),
                        SupplierTrucks = item.ExternalSupplierAddressTruckTypes.Select(t => t.TruckTypeId).ToList(),
                        SupplierServingStates = string.Join(", ", item.MstStates.Select(t => t.Name).ToList()),
                    });
                }
            }

            return viewModel;
        }

        public static List<LocationsViewModel> ToViewModel(this ICollection<ExternalSupplierAddress> entity, List<LocationsViewModel> viewModel = null)
        {
            if (viewModel == null)
                viewModel = new List<LocationsViewModel>();

            if (entity != null && entity.Count > 0)
            {
                foreach (var item in entity)
                {
                    viewModel.Add(new LocationsViewModel()
                    {
                        Id = item.Id,
                        Address = item.Address,
                        City = item.City,
                        ZipCode = item.ZipCode,
                        StateId = item.StateId,
                        SupplierProductTypes = item.MstProductTypes.Select(t => t.Id).ToList(),
                        SupplierProfile = item.ToViewModel(new ServicesViewModel()),
                        PhoneNumber = item.PhoneNumber,
                        PhoneType = item.PhoneTypeId
                    });
                }
            }

            return viewModel;
        }

        public static LocationsViewModel ToViewModel(this ExternalSupplierAddress item, LocationsViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new LocationsViewModel();

            if (item != null)
            {
                viewModel.Id = item.Id;
                viewModel.Address = item.Address;
                viewModel.City = item.City;
                viewModel.ZipCode = item.ZipCode;
                viewModel.StateId = item.StateId;
                viewModel.SupplierProductTypes = item.MstProductTypes.Select(t => t.Id).ToList();
                viewModel.SupplierProfile = item.ToViewModel(new ServicesViewModel());
                viewModel.PhoneNumber = item.PhoneNumber;
                viewModel.PhoneType = item.PhoneTypeId;
            }
            return viewModel;
        }

        public static CompanyViewModel ToCompanyViewModel(this ExternalSupplier entity)
        {
            CompanyViewModel company = new CompanyViewModel();
            company.CompanyTypeId = (int)CompanyType.Supplier;
            company.Name = entity.Name;
            return company;
        }

        public static RegisterViewModel ToUserViewModel(this ExternalSupplier entity)
        {
            RegisterViewModel user = new RegisterViewModel();
            string staticPassword = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingApplicationUserDefaultPassword, "First#1234");         
            user.FirstName = entity.ContactPersonName;
            user.Email = entity.ContactPersonEmail;
            user.MobileNumber = entity.ContactPersonPhoneNumber;
            user.Password = staticPassword;
            user.ConfirmPassword = staticPassword;
            return user;
        }

        public static CompanyAddressViewModel ToCompanyAddressViewModel(this ExternalSupplierAddress entity)
        {
            CompanyAddressViewModel address = new CompanyAddressViewModel();
            address.Address = entity.Address;
            address.City = entity.City;
            address.State = new StateViewModel() { Id = entity.StateId };
            address.ZipCode = entity.ZipCode;
            address.Country = new CountryViewModel() { Id = entity.CountryId };
            address.Latitude = entity.Latitude;
            address.Longitude = entity.Longitude;
            address.Phone = new PhoneViewModel() { PhoneType = entity.PhoneTypeId, PhoneNumber = entity.PhoneNumber };
            address.SupplierProductTypes = entity.MstProductTypes.Select(t => t.Id).ToList();

            return address;
        }
    }
}
