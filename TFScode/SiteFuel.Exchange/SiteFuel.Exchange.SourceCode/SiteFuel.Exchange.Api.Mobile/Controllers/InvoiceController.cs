using Newtonsoft.Json;
using SiteFuel.Exchange.Api.Mobile.Attributes;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace SiteFuel.Exchange.Api.Mobile.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif

    [ValidateToken]
    public class InvoiceController : ApiBaseController
    {

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<DdtApprovalListViewModel>> GetInvoicesToBeApproved(int userId)
        {
            var brandedSupCompId = GetBrandedSupplierCompId();
            var userContext = await GetUserContext(userId);
            var invoices = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetDdtApprovalListAsync(userContext, userId, brandedSupCompId);
            var response = invoices.Where(t => t.IsApprovalUser).ToList();
            return response;
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<ApiInvoiceDetailViewModel> GetBuyerInvoiceDetail(int invoiceId)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetBuyerInvoiceDetail(invoiceId);
            return response;
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> ApproveInvoice(int userId, int invoiceId)
        {
            var userContext = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetUserContextAsync(userId, CompanyType.Buyer);
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().ApproveInvoiceAsync(userContext, invoiceId);
            return response;
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> DeclineInvoice(DeclineInvoiceViewModel viewModel)
        {
            var userContext = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetUserContextAsync(viewModel.UserId, CompanyType.Buyer);
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().DeclineInvoiceAsync(userContext, viewModel);
            return response;
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<List<InvoiceGridViewModel>> GetInvoiceList(InvoiceListViewModel requestModel)
        {
            var dashboardDomain = new DashboardDomain();
            var invoiceDomain = new InvoiceDomain(dashboardDomain);
            var invoiceDataTableViewModel = new InvoiceDataTableViewModel()
            {
                start = requestModel.Start,
                length = requestModel.Length,
                Filter = requestModel.Filter,
                Currency = requestModel.Currency,
                CountryId = requestModel.CountryId,
                GroupIds = requestModel.GroupIds,
            };
            var brandedSupCompanyId = GetBrandedSupplierCompId();
            var invoiceTypeIdFilter = "";
            return await invoiceDomain.GetBuyerInvoiceGridAsync(requestModel.UserId, requestModel.CompanyId, true, invoiceDataTableViewModel, 0, brandedSupCompanyId > 0 ? brandedSupCompanyId : -1, invoiceTypeIdFilter);
        }


        [HttpPost]
        [ApiLog(Enabled = true, TPDLogEnabled = true)]
        public async Task<ApiResponseViewModel> Create(TPDInvoiceViewModel viewModel)
        {
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            var response = await ContextFactory.Current.GetDomain<InvoiceTPDDomain>().ValidateAndProcessApi(viewModel, token);
            return response;
        }

        [HttpPost]
        [ApiLog(Enabled = true, TPDLogEnabled = true)]
        public async Task<ApiResponseViewModel> UpdateImages(TPDImageFileUploadViewModel request)
        {
            var response = new ApiResponseViewModel();
            var requestJson = string.Empty;
            if (request == null)
            {
                requestJson = Convert.ToString(HttpContext.Current.Request.Form["request"]) ?? string.Empty;
                request = JsonConvert.DeserializeObject<TPDImageFileUploadViewModel>(requestJson);
            }

            if (request != null)
            {
                var uploadedFiles = HttpContext.Current.Request.Files;
                if (uploadedFiles != null)
                {
                    if (uploadedFiles.Count > 0)
                    {
                        request.DropFile = HttpContext.Current.Request.Files["dropFile"];
                        request.BolFile = HttpContext.Current.Request.Files["bolFile"];
                        request.SignatureFile = HttpContext.Current.Request.Files["signatureFile"];
                        request.AdditionalFile = HttpContext.Current.Request.Files["additionalFile"];
                        //CHECK FILE SIZE HERE
                        if (request.DropFile != null || request.BolFile != null || request.SignatureFile != null)
                        {
                            CheckFileFormatAndSize(request, response);

                            if (!response.Messages.Any())
                            {
                                var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
                                response = await ContextFactory.Current.GetDomain<InvoiceTPDDomain>().ValidateAndProcessImages(request, token);
                            }
                        }
                        else
                            response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ04, Message = Resource.errMsgParmeterNotMatch });
                    }
                    else
                        response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ04, Message = Resource.errAtLeastOneImageToUpload });
                }
                else
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ04, Message = Resource.errMsgParmeterNotMatch });
            }
            else
                response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ04, Message = Resource.errMsgParmeterNotMatch });

            return response;
        }

        private static void CheckFileFormatAndSize(TPDImageFileUploadViewModel request, ApiResponseViewModel response)
        {
            if (request.DropFile != null && string.IsNullOrWhiteSpace(request.DropFile.FileName))
                response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ04, Message = string.Format(Resource.errMsgParameterIsRequired, "Drop File") });
            if (request.DropFile != null && request.DropFile.ContentLength > ApplicationConstants.TFXImageAndPdfAllowedFileUploadSizeInBytes)
                response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ04, Message = $"Drop {Resource.errFileSizeMessage }" });
            if (request.DropFile != null && !string.IsNullOrWhiteSpace(request.DropFile.FileName))
            {
                var fileFormat = System.IO.Path.GetExtension(request.DropFile.FileName).ToLower();
                if (!(fileFormat == ".jpg" || fileFormat == ".png" || fileFormat == ".pdf"))
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ05, Message = string.Format(Resource.errFileFormatMessage, "Drop") });
            }

            if (request.BolFile != null && string.IsNullOrWhiteSpace(request.BolFile.FileName))
                response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ04, Message = string.Format(Resource.errMsgParameterIsRequired, "BOL File") });
            if (request.BolFile != null && request.BolFile.ContentLength > ApplicationConstants.TFXImageAndPdfAllowedFileUploadSizeInBytes)
                response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ04, Message = $"BOL {Resource.errFileSizeMessage }" });
            if (request.BolFile != null && !string.IsNullOrWhiteSpace(request.BolFile.FileName))
            {
                var fileFormat = System.IO.Path.GetExtension(request.BolFile.FileName).ToLower();
                if (!(fileFormat == ".jpg" || fileFormat == ".png" || fileFormat == ".pdf"))
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ05, Message = string.Format(Resource.errFileFormatMessage, "BOL") });
            }

            if (request.SignatureFile != null && string.IsNullOrWhiteSpace(request.SignatureFile.FileName))
                response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ04, Message = string.Format(Resource.errMsgParameterIsRequired, "Signature File") });
            if (request.SignatureFile != null && request.SignatureFile.ContentLength > ApplicationConstants.TFXImageAndPdfAllowedFileUploadSizeInBytes)
                response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ04, Message = $"Signature {Resource.errFileSizeMessage }" });
            if (request.SignatureFile != null && !string.IsNullOrWhiteSpace(request.SignatureFile.FileName))
            {
                var fileFormat = System.IO.Path.GetExtension(request.SignatureFile.FileName).ToLower();
                if (!(fileFormat == ".jpg" || fileFormat == ".png" || fileFormat == ".pdf"))
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ05, Message = string.Format(Resource.errFileFormatMessage, "Signature") });
            }

            if (request.AdditionalFile != null && string.IsNullOrWhiteSpace(request.AdditionalFile.FileName))
                response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ04, Message = string.Format(Resource.errMsgParameterIsRequired, "Additional File") });
            if (request.AdditionalFile != null && request.AdditionalFile.ContentLength > ApplicationConstants.TFXImageAndPdfAllowedFileUploadSizeInBytes)
                response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ04, Message = $"Additional {Resource.errFileSizeMessage }" });
            if (request.AdditionalFile != null && !string.IsNullOrWhiteSpace(request.AdditionalFile.FileName))
            {
                var fileFormat = System.IO.Path.GetExtension(request.AdditionalFile.FileName).ToLower();
                if (!(fileFormat == ".jpg" || fileFormat == ".png" || fileFormat == ".pdf"))
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ05, Message = string.Format(Resource.errFileFormatMessage, "Additional") });
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<DropQuantityByPrePostDipResponseModel> GetDropQuantityByPrePostDip(DropQuantityByPrePostDipRequestModel request)
        {   
            DropQuantityByPrePostDipResponseModel response = new DropQuantityByPrePostDipResponseModel();
            try
            {
                var apiResponse = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetDropQuantityByPrePostDip(new List<DropQuantityByPrePostDipRequestModel> { request });
                if (apiResponse != null)
                {
                    response = apiResponse.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceController", "GetDropQuantityByPrePostDip", ex.Message, ex);
            }
            return response;
        }

        [HttpGet]
        public async Task<List<Usp_CompanySpecificDeliveryDetails>> GetDailyDRReport()
        { 
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().CreateDeliveryDetailsDailyDataDumpReport();
            return new List<Usp_CompanySpecificDeliveryDetails>();
        }

    }
}
