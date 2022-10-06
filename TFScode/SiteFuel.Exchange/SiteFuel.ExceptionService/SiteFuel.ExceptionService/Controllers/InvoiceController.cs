using SiteFuel.BAL.InvoiceException;
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
    public class InvoiceController : ApiController
    {
        [HttpPost]
        public async Task<InvoiceExceptionResponseModel> CheckExceptions(List<InvoiceExceptionRequestModel> models)
        {
            InvoiceException invoiceException = new InvoiceException();
            var response = await invoiceException.CheckExceptions(models);
            return response;
        }

        [HttpPost]
        public async Task<InvoiceExceptionResponseModel> CheckInvoiceApiExceptions(InvoiceExceptionRequestModel model)
        {
            InvoiceException invoiceException = new InvoiceException();
            var response = await invoiceException.RaiseException(model);
            return response;
        }       
    }
}
