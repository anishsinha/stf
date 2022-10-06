using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SiteFuel.Exchange.Web.Common
{
    public class ApplicationUser : ClaimsPrincipal
    {
        public ApplicationUser(ClaimsPrincipal principal) : base(principal)
        {
        }
        private static int cacheTimePeriodInSecs = 28800; //8 hours

        #region Claims

        private string GetClaim(string key)
        {
            string respone = "";
            try
            {
                var claim = FindFirst(key);
                if (claim != null)
                    respone = claim.Value;
            }
            catch (Exception)
            {
            }
            return respone;
        }
        /// <summary>
        /// refresh the supplier logo in multiple window open if any user update the logo it reflect the other users also.
        /// refresh buyer dashboard logo if any change detect happen in supplier side.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetSupplierLogo(string supplierURL, int companyID, int userID)
        {
            var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
            try
            {
                var key = $"{ApplicationConstants.CompanyLogoCacheName}_{GetSupplierKey(supplierURL, companyID)}";
                var logoPath = string.Empty;
                if (!CacheManager.IsSet(key))
                {
                    var websiteLogoPath = domain.WebSiteConfigurationDetails(supplierURL, companyID, userID);
                    if (!string.IsNullOrEmpty(websiteLogoPath.Item1))
                    {
                        ImageViewModel imageViewModel = new ImageViewModel();
                        imageViewModel.FilePath = websiteLogoPath.Item1;
                        logoPath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                    }
                    CacheManager.Set(key, logoPath, cacheTimePeriodInSecs);
                }
                else
                {
                    logoPath = CacheManager.Get<string>(key);
                }
                return logoPath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string GetSupplierFavicon(string supplierURL, int companyID, int userID)
        {
            var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
            try
            {
                var key = $"{ApplicationConstants.CompanyFaviconCacheName}_{GetSupplierKey(supplierURL, companyID)}";
                var faviconPath = string.Empty;
                if (!CacheManager.IsSet(key))
                {
                    var websiteLogoPath = domain.WebSiteConfigurationDetails(supplierURL, companyID, userID);
                    if (!string.IsNullOrEmpty(websiteLogoPath.Item5))
                    {
                        ImageViewModel imageViewModel = new ImageViewModel();
                        imageViewModel.FilePath = websiteLogoPath.Item5;
                        faviconPath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                    }
                    CacheManager.Set(key, faviconPath, cacheTimePeriodInSecs);
                }
                else
                {
                    faviconPath = CacheManager.Get<string>(key);
                }
                return faviconPath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        private string GetSupplierKey(string supplierURL, int companyID)
        {
            if (!string.IsNullOrEmpty(supplierURL))
            {
                return supplierURL;
            }
            else
            {
                return companyID.ToString();
            }
        }
        public int Id
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.UserId).GetValue<int>();
            }
        }

        public string Name
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.UserName);
            }
        }

        public string UserName
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.Email);
            }
        }

        public string Email
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.Email);
            }
        }

        public DateTimeOffset AutheticationRevalidationTime
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.CookieTimeStamp).GetDateTimeOffsetValue(ApplicationSecurityConstants.CookieTimeStampFormat);
            }
        }

        public string AutheticationFingerPrint
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.UserFingerPrint);
            }
        }

        public int CompanyId
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.CompanyId).GetValue<int>();
            }
        }

        public CompanyType CompanyTypeId
        {
            get
            {
                return (CompanyType)GetClaim(ApplicationSecurityConstants.CompanyTypeId).GetValue<int>();
            }
        }

        public string CompanyName
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.CompanyName);
            }
        }

        public int CompanyLogoId
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.CompanyLogoId).GetValue<int>();
            }
        }
        public string CompanyLogo
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.CompanyLogo).GetValue<string>();
            }
        }

        public CompanyType CompanySubTypeId
        {
            get
            {
                return (CompanyType)GetClaim(ApplicationSecurityConstants.SelectedProfile).GetValue<int>();
            }
        }

        public List<int> Roles
        {
            get
            {
                var claims = Claims.Where(c => c.Type == ApplicationSecurityConstants.UserRole);
                var roles = claims.Select(c => c.Value.GetValue<int>()).ToList();
                return roles;
            }
        }

        public bool IsFirstLogin
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.IsFirstLogin).GetValue<bool>();
            }
        }

        public bool IsSalesCalculatorAllowed
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.IsSalesCalculatorAllowed).GetValue<bool>();
            }
        }

        public bool IsImpersonated
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.IsImpersonated).GetValue<bool>();
            }
        }

        public int ImpersonatedBy
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.ImpersonatedBy).GetValue<int>();
            }
        }

        public string CxmlFormPost
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.CxmlFormPost);
            }
        }

        public string CxmlBuyerCookie
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.CxmlBuyerCookie);
            }
        }

        public string NameInitials
        {
            get
            {
                var names = Name.Split(' ');
                if (names.Length > 1)
                {
                    return $"{names[0][0]}{names[names.Length - 1][0]}";
                }
                else
                {
                    return $"{names[0][0]}";
                }
            }
        }
        #endregion

        #region User Roles

        public bool IsSuperAdmin
        {
            get
            {
                return Roles.Any(t => t == (int)UserRoles.SuperAdmin);
            }
        }

        public bool IsAccountSpecialist
        {
            get
            {
                return Roles.Any(t => t == (int)UserRoles.AccountSpecialist);
            }
        }

        private bool IsAdmin
        {
            get
            {
                return Roles.Any(t => t == (int)UserRoles.Admin);
            }
        }

        public bool IsBuyerAdmin
        {
            get
            {
                return (IsBuyerCompany && IsAdmin);
            }
        }

        public bool IsSupplierAdmin
        {
            get
            {
                return (IsSupplierCompany && IsAdmin);
            }
        }

        public bool IsCarrierAdmin
        {
            get
            {
                return (IsCarrierCompany && IsAdmin);
            }
        }

        public bool IsBuyer
        {
            get
            {
                return Roles.Any(t => t == (int)UserRoles.Buyer);
            }
        }

        public bool IsSupplier
        {
            get
            {
                return Roles.Any(t => t == (int)UserRoles.Supplier);
            }
        }

        public bool IsDriver
        {
            get
            {
                return Roles.Any(t => t == (int)UserRoles.Driver);
            }
        }

        public bool IsOnsitePerson
        {
            get
            {
                return Roles.Any(t => t == (int)UserRoles.OnsitePerson);
            }
        }

        public bool IsAccountingPerson
        {
            get
            {
                return Roles.Any(t => t == (int)UserRoles.AccountingPerson);
            }
        }

        public bool IsReportingPerson
        {
            get
            {
                return Roles.Any(t => t == (int)UserRoles.ReportingPerson);
            }
        }

        public bool IsInternalSalesPerson
        {
            get
            {
                return Roles.Any(t => t == (int)UserRoles.InternalSalesPerson);
            }
        }

        public bool IsExternalVendor
        {
            get
            {
                return Roles.Any(t => t == (int)UserRoles.ExternalVendor);
            }
        }

        public bool IsCarrier
        {
            get
            {
                return Roles.Any(t => t == (int)UserRoles.Carrier);
            }
        }

        public bool IsDispatcher
        {
            get
            {
                return Roles.Any(t => t == (int)UserRoles.Dispatcher);
            }
        }
        public bool IsSalesUser
        {
            get
            {
                return Roles.Any(t => t == (int)UserRoles.Sales);
            }
        }
        #endregion

        #region User Company

        public bool IsBuyerCompany
        {
            get
            {
                return (
                            (CompanyTypeId == CompanyType.Buyer) ||
                            (CompanyTypeId == CompanyType.BuyerAndSupplier && CompanySubTypeId == CompanyType.Buyer) ||
                            (CompanyTypeId == CompanyType.BuyerSupplierAndCarrier && CompanySubTypeId == CompanyType.Buyer)
                       );
            }
        }

        public bool IsSupplierCompany
        {
            get
            {
                return (
                            (CompanyTypeId == CompanyType.Supplier) ||
                            ((CompanyTypeId == CompanyType.BuyerAndSupplier || CompanyTypeId == CompanyType.SupplierAndCarrier) && CompanySubTypeId == CompanyType.Supplier) ||
                            ((CompanyTypeId == CompanyType.SupplierAndCarrier || CompanyTypeId == CompanyType.BuyerSupplierAndCarrier) && CompanySubTypeId == CompanyType.Supplier)
                       );
            }
        }

        public bool IsCarrierCompany
        {
            get
            {
                return (
                            (CompanyTypeId == CompanyType.Carrier) ||
                            (CompanyTypeId == CompanyType.SupplierAndCarrier && CompanySubTypeId == CompanyType.Carrier) ||
                            (CompanyTypeId == CompanyType.BuyerSupplierAndCarrier && CompanySubTypeId == CompanyType.Carrier)
                       );
            }
        }

        public bool IsBuyerAndSupplierCompany
        {
            get
            {
                return CompanyTypeId == CompanyType.BuyerAndSupplier;
            }
        }

        public bool IsSupplierAndCarrierCompany
        {
            get
            {
                return CompanyTypeId == CompanyType.SupplierAndCarrier;
            }
        }

        public bool IsBuyerSupplierAndCarrierCompany
        {
            get
            {
                return CompanyTypeId == CompanyType.BuyerSupplierAndCarrier;
            }
        }
        public string SupplierURL
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.SupplierURL);
            }
        }
        public string SupplierLogoPath
        {
            get
            {
                return GetSupplierLogo(GetClaim(ApplicationSecurityConstants.SupplierURL), Convert.ToInt32(GetClaim(ApplicationSecurityConstants.CompanyId)), Convert.ToInt32(GetClaim(ApplicationSecurityConstants.UserId)));
            }
        }

        public string SupplierFaviconPath
        {
            get
            {
                return GetSupplierFavicon(GetClaim(ApplicationSecurityConstants.SupplierURL), Convert.ToInt32(GetClaim(ApplicationSecurityConstants.CompanyId)), Convert.ToInt32(GetClaim(ApplicationSecurityConstants.UserId)));
            }
        }

        public string ButtonColor
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.ButtonColor);
            }
        }
        public int BrandedCompanyId
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.BrandedCompanyId).GetValue<int>();
            }
        }

        public int ApplicationTemplateId
        {
            get
            {
                return GetClaim(ApplicationSecurityConstants.ApplicationTemplateId).GetValue<int>();
            }
        }
        #endregion
    }
}