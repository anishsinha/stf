using SiteFuel.BAL.CompanyException;
using SiteFuel.BAL.InvoiceException;
using SiteFuel.Models.ApiModels;
using SiteFuel.Models.CompanyException;
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
    public class ExceptionController : ApiController
    {
        [HttpGet]
        public async Task<CompanyApprovalExceptionModel> GetMyApprovals(int approvalCompanyId)
        {
            InvoiceException invoiceException = new InvoiceException();
            var response = await invoiceException.GetMyApprovalExceptions(approvalCompanyId);
            return response;
        }

        [HttpPost]
        public async Task<bool> ApproveException(ExceptionApprovalRequestModel model)
        {
            InvoiceException invoiceException = new InvoiceException();
            var response = await invoiceException.ApproveException(model);
            return response;
        }
        [HttpGet]
        public async Task<List<DeliveredQuantityVarianceExceptionModel>> GetBuyerApprovalExceptions(int supplierCompanyId, string exceptionTypes)
        {
            InvoiceException invoiceException = new InvoiceException();
            var response = await invoiceException.GetBuyerApprovalExceptions(supplierCompanyId, exceptionTypes);
            return response;
        }

        [HttpGet]
        public async Task<CompanyApprovalExceptionModel> GetSupplierApprovalExceptions(int buyerCompanyId, string exceptionTypes)
        {
            InvoiceException invoiceException = new InvoiceException();
            var response = await invoiceException.GetSupplierApprovalExceptions(buyerCompanyId, exceptionTypes);
            return response;
        }

        [HttpGet]
        public async Task<List<AutoApprovalExceptionModel>> GetAutoApprovalExceptions(string holidayList = "", bool isSaturdayOff = false)
        {
            InvoiceException invoiceException = new InvoiceException();
            var response = await invoiceException.GetAutoApprovalExceptions(holidayList, isSaturdayOff);
            return response;
        }

        [HttpGet]
        public async Task<CompanyApprovalExceptionModel> GetExceptionsForApproval(int approvalCompanyId, string exceptionTypes)
        {
            InvoiceException invoiceException = new InvoiceException();
            var response = await invoiceException.GetExceptionsForApproval(approvalCompanyId, exceptionTypes);
            return response;
        }

        [HttpGet]
        public async Task<List<int>> GetExceptionIdsforAutoRejection(int exceptionTypeId, int statusId)
        {
            InvoiceException invoiceException = new InvoiceException();
            var response = await invoiceException.GetExceptionIdsForAutoRejection(exceptionTypeId, statusId);
            return response;
        }

        [HttpGet]
        public async Task<List<RaisedExceptionModel>> GetRaisedExceptions(string exceptionTypeIds, int companyId, bool isBuyerCompany)
        {
            CompanyException companyException = new CompanyException();
            var response = await companyException.GetRaisedException(exceptionTypeIds, companyId, isBuyerCompany);
            return response;
        }

        [HttpPost]
        public async Task<bool> UpdateExceptionIdForAutoReject(List<int> generatedExceptionList)
        {
            InvoiceException invoiceException = new InvoiceException();
            var response = await invoiceException.UpdateExceptionIdForAutoReject(generatedExceptionList);
            return response;
        }

        [HttpPost]
        public async Task<InvoiceExceptionResponseModel> RaiseDeliveryMismatchExceptions(InvoiceExceptionRequestModel model)
        {
            InvoiceException invoiceException = new InvoiceException();
            var response = await invoiceException.RaiseException(model);
            return response;
        }
    }
}
