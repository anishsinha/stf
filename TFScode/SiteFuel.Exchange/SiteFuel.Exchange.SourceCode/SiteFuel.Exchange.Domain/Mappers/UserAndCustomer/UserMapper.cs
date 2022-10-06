using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class UserMapper
    {
        public static UserViewModel ToViewModel(this User entity, UserViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new UserViewModel(AuthStatus.Success);

            viewModel.Id = entity.Id;
            viewModel.FirstName = entity.FirstName;
            viewModel.LastName = entity.LastName;
            viewModel.FullName = $"{entity.FirstName.Trim()} {entity.LastName.Trim()}";
            viewModel.Email = entity.Email;
            viewModel.IsEmailConfirmed = entity.IsEmailConfirmed;
            viewModel.PhoneNumber = entity.PhoneNumber;
            viewModel.IsPhoneNumberConfirmed = entity.IsPhoneNumberConfirmed;
            viewModel.IsTwoFactorEnabled = entity.IsTwoFactorEnabled;
            viewModel.AccessFailedCount = entity.AccessFailedCount;
            viewModel.IsLockoutEnabled = entity.IsLockoutEnabled;
            viewModel.LockoutEndDateUtc = entity.LockoutEndDateUtc;
            viewModel.FingerPrint = entity.FingerPrint;
            viewModel.IsOnboardingComplete = entity.IsOnboardingComplete;
            viewModel.OnboardedDate = entity.OnboardedDate;
            viewModel.IsApiAccessAllowed = entity.IsApiAccessAllowed;
            viewModel.IsActive = entity.IsActive;
            viewModel.IsFirstLogin = entity.IsFirstLogin;
            viewModel.IsSalesCalculatorAllowed = entity.IsSalesCalculatorAllowed;
            viewModel.Title = entity.Title;

            viewModel.Roles = entity.MstRoles.Where(t => t.IsActive).Select(t => new RoleViewModel()
            {
                Id = t.Id,
                Name = t.Name
            }).ToList();

            if (entity.Company != null)
            {
                viewModel.CompanyId = entity.Company.Id;
                viewModel.CompanyTypeId = entity.Company.CompanyTypeId;
                viewModel.CompanyName = entity.Company.Name;
                viewModel.CompanyLogoId = entity.Company.CompanyLogoId;
                if (entity.Company.Image == null || string.IsNullOrEmpty(entity.Company.Image.FilePath))
                {
                    viewModel.CompanyLogo = string.Empty;
                }
                else
                {
                    viewModel.CompanyLogo = entity.Company.Image.ToViewModel().GetAzureFilePath(BlobContainerType.CompanyProfile);
                }
                if (entity.Company.CompanyAddresses.Any(t => t.IsDefault && t.IsActive))
                {
                    var defaultAddress = entity.Company.CompanyAddresses.Where(t => t.IsDefault && t.IsActive).Select(t => t.CountryId).FirstOrDefault();
                    if(defaultAddress > 0)
                    {
                        viewModel.CompanyDefaultCountry = defaultAddress;
                    }
                }
            }
            return viewModel;
        }

        public static User ToEntity(this UserViewModel viewModel, User entity = null)
        {
            if (entity == null)
                entity = new User();

            entity.Id = viewModel.Id;
            entity.FirstName = viewModel.FirstName;
            entity.LastName = viewModel.LastName;
            entity.IsPhoneNumberConfirmed = viewModel.IsPhoneNumberConfirmed;
            entity.PhoneNumber = viewModel.PhoneNumber;
            entity.Email = viewModel.Email.Trim().ToLower();
            entity.UserName = viewModel.Email.Trim().ToLower();
            entity.IsTwoFactorEnabled = viewModel.IsTwoFactorEnabled;
            entity.IsLockoutEnabled = viewModel.IsLockoutEnabled;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.IsFirstLogin = viewModel.IsFirstLogin;
            entity.IsSalesCalculatorAllowed = viewModel.IsSalesCalculatorAllowed;
            entity.IsApiAccessAllowed = viewModel.IsApiAccessAllowed;
            entity.Title = viewModel.Title;
            return entity;
        }

        public static UserViewModel ToUserViewModel(this User entity, UserViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new UserViewModel(AuthStatus.Success);

            viewModel.Id = entity.Id;
            viewModel.FirstName = entity.FirstName;
            viewModel.LastName = entity.LastName;
            viewModel.FullName = $"{entity.FirstName.Trim()} {entity.LastName.Trim()}";
            viewModel.Email = entity.Email;
            viewModel.IsEmailConfirmed = entity.IsEmailConfirmed;
            viewModel.PhoneNumber = entity.PhoneNumber;
            viewModel.IsPhoneNumberConfirmed = entity.IsPhoneNumberConfirmed;
            viewModel.IsTwoFactorEnabled = entity.IsTwoFactorEnabled;
            viewModel.AccessFailedCount = entity.AccessFailedCount;
            viewModel.IsLockoutEnabled = entity.IsLockoutEnabled;
            viewModel.LockoutEndDateUtc = entity.LockoutEndDateUtc;
            viewModel.FingerPrint = entity.FingerPrint;
            viewModel.IsOnboardingComplete = entity.IsOnboardingComplete;
            viewModel.OnboardedDate = entity.OnboardedDate;
            viewModel.IsApiAccessAllowed = entity.IsApiAccessAllowed;
            viewModel.IsActive = entity.IsActive;
            viewModel.IsFirstLogin = entity.IsFirstLogin;
            viewModel.IsSalesCalculatorAllowed = entity.IsSalesCalculatorAllowed;
            viewModel.Title = entity.Title;

            return viewModel;
        }

        public static AdditionalUsersViewModel ToDriverViewModel(this DriverObjectModel entity, AdditionalUsersViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new AdditionalUsersViewModel();

            viewModel.UserId = entity.CreatedBy;

            if (viewModel.AdditionalUsers == null)
                viewModel.AdditionalUsers = new List<AdditionalUserViewModel>();

            AdditionalUserViewModel obj = new AdditionalUserViewModel();
            obj.Id = entity.UserId;
            obj.DriverUserId = entity.DriverId;
            obj.FirstName = entity.FirstName;
            obj.LastName = entity.LastName;
            obj.CompanyName = entity.CompanyName;
            obj.PhoneNumber = entity.ContactNumber;
            obj.Email = entity.Email;
            obj.RoleIds.Add((int)UserRoles.Driver);
            obj.CompanyId = entity.CompanyId;
            obj.InvitedBy = entity.InvitedBy;
            obj.IsActive = entity.IsActive;

            if (obj.DriverInfo == null)
                obj.DriverInfo = new DriverInformationViewModel();

            obj.DriverInfo.CompanyName = entity.CompanyName;
            obj.DriverInfo.ExpiryDate = entity.ExpiryDate;
            obj.DriverInfo.LicenseNumber = entity.LicenseNumber;
            obj.DriverInfo.LicenseTypeId = entity.LicenseTypeId;
            obj.DriverInfo.ShiftId = entity.ShiftId;
            obj.DriverInfo.TrailerType = entity.TrailerType;
            obj.DriverInfo.Regions = entity.Regions;
            //if (obj.DriverInfo.TrailerType == null)
            //    obj.DriverInfo.TrailerType = new List<TrailerTypeStatus>();
            //foreach (var id in entity.TrailerTypeId)
            //{
            //    var type = (TrailerTypeStatus)id;
            //    obj.DriverInfo.TrailerType.Add(type);
            //}
            obj.DriverInfo.IsFilldAuthorized = entity.IsFilldAuthorized;
            viewModel.AdditionalUsers.Add(obj);

            return viewModel;
        }

        public static DriverObjectModel ToDriverModel(this AdditionalUserViewModel entity, DriverObjectModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new DriverObjectModel();

            viewModel.UserId = entity.Id;
            viewModel.DriverId = entity.DriverUserId;
            viewModel.FirstName = entity.FirstName;
            viewModel.LastName = entity.LastName;
            viewModel.CompanyName = entity.CompanyName;
            viewModel.Email = entity.Email;
            viewModel.ContactNumber = entity.PhoneNumber == Constants.DummyPhoneNumber ? "NA" : entity.PhoneNumber;
            viewModel.CompanyId = entity.CompanyId;
            viewModel.InvitedBy = entity.InvitedBy;
            viewModel.IsActive = entity.IsActive;

            if (entity.DriverInfo != null)
            {
                viewModel.CompanyName = entity.DriverInfo.CompanyName;
                viewModel.ExpiryDate = entity.DriverInfo.ExpiryDate;
                viewModel.LicenseNumber = entity.DriverInfo.LicenseNumber;
                viewModel.LicenseTypeId = entity.DriverInfo.LicenseTypeId;

                if (viewModel.LicenseTypeId != null)
                {
                    var licenseTypeId = 0;
                    int.TryParse(viewModel.LicenseTypeId, out licenseTypeId);
                    if (licenseTypeId > 0)
                    {
                        viewModel.DisplayLicenseType = licenseTypeId == 0 ? null : ((LicenceRequirement)licenseTypeId).GetDisplayName();
                        viewModel.LicenseTypeId = ((LicenceRequirement)licenseTypeId).ToString();
                    }
                    else
                    {
                        licenseTypeId = (int)System.Enum.Parse(typeof(LicenceRequirement), viewModel.LicenseTypeId);
                        viewModel.DisplayLicenseType = licenseTypeId == 0 ? null : ((LicenceRequirement)licenseTypeId).GetDisplayName();
                    }
                }

                if (viewModel.ShiftId == null)
                    viewModel.ShiftId = new List<string>();
                viewModel.ShiftId = entity.DriverInfo.ShiftId;

                if (viewModel.Regions == null)
                    viewModel.Regions = new List<string>();
                viewModel.Regions = entity.DriverInfo.Regions;

                if (entity.DriverInfo.TrailerType != null)
                {
                    if (viewModel.TrailerType == null)
                        viewModel.TrailerType = new List<TrailerTypeStatus>();

                    foreach (var trailerType in entity.DriverInfo.TrailerType)
                    {
                        var type = (TrailerTypeStatus)trailerType;
                        viewModel.TrailerType.Add(type);
                        viewModel.DisplayTrailerTypes += type.ToString() + ",";
                    }
                    viewModel.DisplayTrailerTypes = viewModel.DisplayTrailerTypes != null ? viewModel.DisplayTrailerTypes.TrimEnd(',') : null;
                }
                viewModel.IsFilldAuthorized = entity.DriverInfo.IsFilldAuthorized;
            }
            viewModel.IsScheduleExists = entity.IsScheduleExists;
            viewModel.ScheduleBuilderIds = entity.ScheduleBuilderIds;
            return viewModel;
        }
    }
}
