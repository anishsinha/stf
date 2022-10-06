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
    class AuthorizeCalculatorAttribute : BaseAuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                base.AuthorizeCore(httpContext);

                if (httpContext.Request.IsAuthenticated)
                {
                    var user = new ApplicationUser((ClaimsPrincipal)httpContext.User);
                    if (user.IsSalesCalculatorAllowed)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AuthorizeCalculatorAttribute", "AuthorizeCore", ex.Message, ex);
            }
            return false;
        }
    }
}