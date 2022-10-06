using SiteFuel.BAL.CustomerException;
using SiteFuel.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace SiteFuel.ExceptionService.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif
    public class CustomerController : ApiController
    {
        [HttpGet]
        public async Task<ManageCustomerExceptionModel> GetExceptions(int companyId, int enabledForCompanyId)
        {
            CustomerException customerException = new CustomerException();
            var response = await customerException.GetExceptions(companyId, enabledForCompanyId);
            return response;
        }

        [HttpPost]
        public async Task<ManageCustomerExceptionModel> SaveExceptions(ManageCustomerExceptionModel model)
        {
            CustomerException customerException = new CustomerException();
            var response = await customerException.SaveExceptions(model);
            return response;
        }

        [HttpGet]
        public async Task<bool> IsExceptionsEnabled(int ownerCompanyId, int enabledForCompanyId)
        {
            CustomerException customerException = new CustomerException();
            var response = await customerException.IsExceptionsEnabled(ownerCompanyId, enabledForCompanyId);
            return response;
        }
    }
}
