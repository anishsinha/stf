using SiteFuel.BAL.CompanyException;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models.ApiModels;
using SiteFuel.Models.InvoiceException;
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
    public class CompanyController : ApiController
    {
        [HttpGet]
        public async Task<ManageExceptionModel> GetExceptions(int companyId)
        {
            CompanyException companyException = new CompanyException();
            var response = await companyException.GetExceptions(companyId);
            return response;
        }

        [HttpPost]
        public async Task<ManageExceptionModel> SaveExceptions(ManageExceptionModel model)
        {
            CompanyException companyException = new CompanyException();
            var response = await companyException.SaveExceptions(model);
            return response;
        }

        [HttpGet]
        public async Task<bool> IsExceptionsEnabled(int companyId)
        {
            CompanyException companyException = new CompanyException();
            var response = await companyException.IsExceptionsEnabled(companyId);
            return response;
        }

        [HttpGet]
        public async Task<bool> IsExceptionEnabledByType(int ownerCompanyId, int exceptionType)
        {
            CompanyException companyException = new CompanyException();
            var response = await companyException.IsExceptionsEnabledByType(ownerCompanyId, exceptionType);
            return response;
        }

        [HttpGet]
        public async Task<List<EnabledExceptionModel>> GetCompaniesForEnabledException(int exceptionTypeId)
        {
            CompanyException companyException = new CompanyException();
            var response = await companyException.GetCompaniesForEnabledException(exceptionTypeId);
            return response;
        }

        [HttpPost]
        public async Task<List<DropdownDisplayExtendedId>> GetDelayInvoiceCreationTimeByCompany(List<int> companyIds)
        {
            CompanyException companyException = new CompanyException();
            var response = await companyException.GetDelayInvoiceCreationTimeByCompany(companyIds);
            return response;
        }
    }
}
