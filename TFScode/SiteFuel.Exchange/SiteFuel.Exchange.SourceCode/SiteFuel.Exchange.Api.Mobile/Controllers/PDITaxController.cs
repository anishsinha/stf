using SiteFuel.Exchange.Api.Mobile.Attributes;
using SiteFuel.Exchange.Api.Mobile.Common;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;

namespace SiteFuel.Exchange.Api.Mobile.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif

    //[ValidateToken]
    public class PDITaxController : ApiBaseController
    {
        [AllowAnonymous]
        [HttpPost]
        
        public async Task<bool> ProcessPDITaxes(List<PDITaxFTPViewModel> taxList)
        {
            using (var tracer = new Tracer("PDITaxController", "ProcessPDITaxes"))
            {
                bool response = false;
                try
                {
                    LogManager.Logger.WriteDebug("PDITaxController", "ProcessPDITaxes", taxList != null && taxList.Any() ? taxList.FirstOrDefault().PDIInvoiceNo : "no data");
                    if (taxList != null && taxList.Any())
                    {
                        int generatedInvoice = await ContextFactory.Current.GetDomain<ConsolidatedDdtToInvoiceDomain>().ProcessInvoicesWaitingForPDITax(taxList);
                        if (generatedInvoice > 0)
                            return true;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("PDITaxController", "ProcessPDITaxes", ex.Message, ex);
                }
                return response;
            }
        }
    }
}