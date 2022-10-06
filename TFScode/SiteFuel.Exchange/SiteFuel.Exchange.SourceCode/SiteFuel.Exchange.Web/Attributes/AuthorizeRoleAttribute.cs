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
    class AuthorizeRoleAttribute : BaseAuthorizeAttribute
    {
        private readonly List<int> allowedRoles;

        public AuthorizeRoleAttribute(params UserRoles[] roles)
        {
            //Collect all decorated roles and exclude Admin, this is to force use of BuyerAdmin/SupplierAdmin
            allowedRoles = roles.Where(t => t != UserRoles.Admin).Select(r => ((int)r)).ToList();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                base.AuthorizeCore(httpContext);

                if (httpContext.Request.IsAuthenticated)
                {
                    if (allowedRoles.Count == 0)
                    {
                        return true;
                    }

                    var user = new ApplicationUser((ClaimsPrincipal)httpContext.User);
                    if (user.IsSuperAdmin || user.IsAccountSpecialist)
                    {
                        return true;
                    }
                    else if (user.IsBuyerAdmin)
                    {
                        return allowedRoles.Contains((int)UserRoles.BuyerAdmin);
                    }
                    else if (user.IsSupplierAdmin)
                    {
                        return allowedRoles.Contains((int)UserRoles.SupplierAdmin);
                    }
                    else if (user.IsCarrierAdmin)
                    {
                        return allowedRoles.Contains((int)UserRoles.CarrierAdmin);
                    }
                    else
                    {
                        return user.Roles.Intersect(allowedRoles).Any();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AuthorizeRoleAttribute", "AuthorizeCore", ex.Message, ex);
            }
            return false;
        }
    }
}