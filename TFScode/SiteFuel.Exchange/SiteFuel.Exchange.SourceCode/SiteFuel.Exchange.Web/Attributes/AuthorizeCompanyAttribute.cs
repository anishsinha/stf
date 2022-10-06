using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SiteFuel.Exchange.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    class AuthorizeCompanyAttribute : BaseAuthorizeAttribute
    {
        private readonly List<int> allowedCompanies;

        public AuthorizeCompanyAttribute(params CompanyType[] companies)
        {
            allowedCompanies = companies.Where(t => t != CompanyType.BuyerAndSupplier && t != CompanyType.SupplierAndCarrier && t != CompanyType.BuyerSupplierAndCarrier).Select(r => ((int)r)).ToList();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                base.AuthorizeCore(httpContext);

                if (httpContext.Request.IsAuthenticated)
                {
                    var user = new ApplicationUser((ClaimsPrincipal)httpContext.User);
                    if (user.IsSuperAdmin || user.IsInternalSalesPerson || user.IsAccountSpecialist)
                    {
                        return true;
                    }
                    else if (user.IsBuyerCompany)
                    {
                        return allowedCompanies.Contains((int)CompanyType.Buyer);
                    }
                    else if (user.IsSupplierCompany)
                    {
                        return allowedCompanies.Contains((int)CompanyType.Supplier);
                    }
                    else if (user.IsCarrierCompany)
                    {
                        return allowedCompanies.Contains((int)CompanyType.Carrier);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AuthorizeCompanyAttribute", "AuthorizeCore", ex.Message, ex);
            }
            return false;
        }
    }
}