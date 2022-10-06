using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.ViewModels
{
    public class UserContext
    {
        private const int _systemAdminUserId = 1;
        public UserContext()
        {
        }

        public UserContext(int id, string name, string userName, string email, int companyId, string companyName,
            CompanyType companyTypeId, CompanyType companySubTypeId, List<int> roles, bool isFirstLogin,
            bool isSalesCalculatorAllowed, bool isImpersonated, int impersonatedBy, string cxmlFormPost = "",
            string cxmlBuyerCookie = "", int applicationTemplateId = 1, int brandedCompanyId = 0)
        {
            Id = id;
            Name = name;
            UserName = userName;
            Email = email;
            CompanyId = companyId;
            CompanyName = companyName;
            CompanyTypeId = companyTypeId;
            CompanySubTypeId = companySubTypeId;
            Roles = roles;
            IsFirstLogin = isFirstLogin;
            IsSalesCalculatorAllowed = isSalesCalculatorAllowed;
            IsImpersonated = isImpersonated;
            ImpersonatedBy = impersonatedBy;
            CxmlFormPost = cxmlFormPost;
            CxmlBuyerCookie = cxmlBuyerCookie;
            ApplicationTemplateId = applicationTemplateId;
            BrandedCompanyId = brandedCompanyId;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public CompanyType CompanyTypeId { get; set; }

        public CompanyType CompanySubTypeId { get; set; }

        public List<int> Roles { get; set; } = new List<int>();

        public bool IsFirstLogin { get; }

        public bool IsSalesCalculatorAllowed { get; }

        public bool IsImpersonated { get; }

        public int ImpersonatedBy { get; }

        public string CxmlFormPost { get; set; }

        public string CxmlBuyerCookie { get; set; }

        public int ApplicationTemplateId { get; }

        public int BrandedCompanyId { get; set; }


        #region User Roles

        public bool IsSuperAdmin
        {
            get
            {
                return Roles.Any(t => t == (int)UserRoles.SuperAdmin);
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

        public bool IsCarrierAdmin
        {
            get
            {
                return (IsCarrierCompany && IsAdmin);
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
                            ((CompanyTypeId == CompanyType.BuyerAndSupplier || CompanyTypeId == CompanyType.SupplierAndCarrier || CompanyTypeId == CompanyType.BuyerSupplierAndCarrier) && CompanySubTypeId == CompanyType.Supplier)
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

        public bool IsCarrierCompany
        {
            get
            {
                return (
                            (CompanyTypeId == CompanyType.Carrier) ||
                            (CompanyTypeId == CompanyType.SupplierAndCarrier && CompanySubTypeId == CompanyType.Carrier)
                       );
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


        public string ActingCompanyType { get; set; }

        public CompanyType ActingCompanyTypeId
        {
            get
            {
                CompanyType companyType;
                return Enum.TryParse(ActingCompanyType, out companyType) ? companyType : CompanyTypeId;
            }
        }

        #endregion

        public static UserContext GetSystemUserContext()
        {
            var userContext = new UserContext()
            {
                Id = _systemAdminUserId,
                Name = "System",
                UserName = "Automatic System"
            };
            userContext.Roles.Add((int)UserRoles.SuperAdmin);
            return userContext;
        }
    }
}
