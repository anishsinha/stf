using Newtonsoft.Json;
using SiteFuel.Exchange.Api.Mobile.Attributes;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace SiteFuel.Exchange.Api.Mobile.Controllers
{
    public class ApiBaseController : ApiController
    {
        protected void SaveDebugInfo<T>(T infoObject)
        {
            var json = JsonConvert.SerializeObject(infoObject, Formatting.Indented);
            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
            LogManager.Logger.WriteDebug(controllerName, actionName, json);
        }

        protected async Task<UserContext> GetUserContext(int userId, CompanyType companyType = CompanyType.Unknown)
        {
            var response = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetUserContextAsync(userId, companyType);
            return response;
        }

        protected int GetBrandedSupplierCompId()
        {
            try
            {
                var brandCompanyId = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.BrandedCompanyId.ToLower());
                if (brandCompanyId.Value != null)
                {
                    int.TryParse(brandCompanyId.Value.First(), out int supplierBrandedCompId);
                    return supplierBrandedCompId;
                }
            }
            catch (System.Exception ex)
            {
                LogManager.Logger.WriteException("ApiBaseController", "GetSupplierBrandedCompId", ex.Message + "BrandedCompanyId parameter not found in header", ex);
            }
            return 0;
        }
    }
}
