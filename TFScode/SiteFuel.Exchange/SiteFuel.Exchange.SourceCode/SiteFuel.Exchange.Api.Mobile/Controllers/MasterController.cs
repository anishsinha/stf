using Easy.Common;
using Easy.Common.Interfaces;
using Newtonsoft.Json;
using SiteFuel.Exchange.Api.Mobile.Attributes;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Domain;
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
using System.Web.Http.Description;
using System.Web.Mvc;
using System.Web.UI;

namespace SiteFuel.Exchange.Api.Mobile.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif

    //This controller needs to be removed, in next build
    public class MasterController : ApiBaseController
    {
        public List<DropdownDisplayItem> GetFuelProducts(ProductDisplayGroups displayGroupId = ProductDisplayGroups.CommonFuelType, int companyId = 0, PricingSource source = PricingSource.Axxis)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var pricingDomain = ContextFactory.Current.GetDomain<ExternalPricingDomain>();
                if (source == PricingSource.OPIS)
                {
                    response = pricingDomain.GetSourceBasedFuelProducts(source);
                }
                else
                {
                    response = Task.Run(() => pricingDomain.GetAxxisFuelProducts(displayGroupId, companyId)).Result;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterController", "GetFuelProducts", ex.Message, ex);
            }

            return response;
        }

        [OutputCache(Duration = 86400, VaryByParam = "id", Location = OutputCacheLocation.ServerAndClient)]
        public HttpResponseMessage GetImage(int imageId)
        {
            BaseResponse returnObj = new BaseResponse
            {
                Message = imageId > 0 ? string.Empty : Constants.DataMissing
            };
            try
            {
                if (string.IsNullOrWhiteSpace(returnObj.Message))
                {
                    var helperDomain = new HelperDomain();
                    var result = Task.Run(() => helperDomain.GetImage(imageId)).Result;
                    if (result.StatusCode == Status.Success)
                    {
                        MemoryStream memoryStream = new MemoryStream(result.Data);
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                        response.Content = new StreamContent(memoryStream);
                        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(MediaType.Png);

                        return response;
                    }
                    else
                    {
                        returnObj.Message = Constants.NoImageFound;

                        return Request.CreateResponse(HttpStatusCode.OK, returnObj);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterController", "GetImage", ex.Message, ex);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
        }

        [OutputCache(Duration = 86400, VaryByParam = "invoiceId", Location = OutputCacheLocation.ServerAndClient)]
        public HttpResponseMessage GetInvoicePdf(int invoiceId)
        {
            BaseResponse returnObj = new BaseResponse
            {
                Message = invoiceId > 0 ? string.Empty : Constants.DataMissing
            };

            var helperDomain = new HelperDomain();
            if (string.IsNullOrWhiteSpace(returnObj.Message))
            {
                var serverUrl = helperDomain.GetPdfServerUrl();
                var url = $"{serverUrl}/InvoiceBase/GetPdfViewModelAsByetArray?id={invoiceId}&companyType={(int)CompanyType.Buyer}";
                byte[] mybytearray = null;
                using (IRestClient client = new RestClient())
                {
                    var responsePdf = client.GetAsync(url).Result;

                    if (responsePdf.IsSuccessStatusCode)
                    {
                        var jsonString = responsePdf.Content.ReadAsStringAsync().Result;
                        mybytearray = JsonConvert.DeserializeObject<byte[]>(jsonString);

                        MemoryStream memoryStream = new MemoryStream(mybytearray);
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                        response.Content = new StreamContent(memoryStream);
                        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(MediaType.Pdf);

                        return response;
                    }
                    else
                    {
                        returnObj.Message = Constants.PdfNotAvailable;

                        return Request.CreateResponse(HttpStatusCode.OK, returnObj);
                    }
                }
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
        }

        public HttpResponseMessage GetFile(string fileName)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                try
                {
                    var azureBlob = new Core.Infrastructure.AzureBlobStorage();
                    var fileStream = azureBlob.DownloadBlob(fileName, BlobContainerType.SpecialInstructions.ToString().ToLower());

                    var memoryStream = fileStream as MemoryStream;
                    var fileExtension = fileName.Split('.').LastOrDefault();
                    string mimeType = MediaType.Pdf;

                    switch (fileExtension)
                    {
                        case "pdf": mimeType = MediaType.Pdf; break;
                        case "doc": mimeType = MediaType.Doc; break;
                        case "docx": mimeType = MediaType.Docx; break;
                        case "bmp": mimeType = MediaType.Bmp; break;
                        case "png": mimeType = MediaType.Png; break;
                        case "jpeg": mimeType = MediaType.Jpeg; break;
                        case "jpg": mimeType = MediaType.Jpg; break;
                    }

                    response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new StreamContent(memoryStream);
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("MasterController", "GetFile", ex.Message, ex);
                }
            }
            return response;
        }

        public HttpResponseMessage GetFileDetails(string fileName, BlobContainerType blobContainerType)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                try
                {
                    var azureBlob = new Core.Infrastructure.AzureBlobStorage();
                    var fileStream = azureBlob.DownloadBlob(fileName, blobContainerType.ToString().ToLower());

                    var memoryStream = fileStream as MemoryStream;
                    var fileExtension = !string.IsNullOrWhiteSpace(fileName.Split('.').LastOrDefault()) ? fileName.Split('.').LastOrDefault().ToLower():"unknown";
                    string mimeType = MediaType.Pdf;

                    switch (fileExtension)
                    {
                        case "pdf": mimeType = MediaType.Pdf; break;
                        case "doc": mimeType = MediaType.Doc; break;
                        case "docx": mimeType = MediaType.Docx; break;
                        case "bmp": mimeType = MediaType.Bmp; break;
                        case "png": mimeType = MediaType.Png; break;
                        case "jpeg": mimeType = MediaType.Jpeg; break;
                        case "jpg": mimeType = MediaType.Jpg; break;
                    }

                    response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new StreamContent(memoryStream);
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("MasterController", "GetFileDetails", ex.Message, ex);
                }
            }
            return response;
        }

        [OutputCache(Duration = 86400, VaryByParam = "invoiceHeaderId", Location = OutputCacheLocation.ServerAndClient)]
        public HttpResponseMessage GetConsolidatedInvoicePdf(int invoiceHeaderId)
        {
            BaseResponse returnObj = new BaseResponse
            {
                Message = invoiceHeaderId > 0 ? string.Empty : Constants.DataMissing
            };

            var helperDomain = new HelperDomain();
            if (string.IsNullOrWhiteSpace(returnObj.Message))
            {
                var serverUrl = helperDomain.GetPdfServerUrl();
                var url = $"{serverUrl}/InvoiceBase/GetMobileAppPdfViewModelAsByteArray?invoiceHeaderId={invoiceHeaderId}&companyType={(int)CompanyType.Buyer}";

                using (IRestClient client = new RestClient())
                {
                    var responsePdf = client.GetAsync(url).Result;
                    byte[] mybytearray = null;

                    if (responsePdf.IsSuccessStatusCode)
                    {
                        var jsonString = responsePdf.Content.ReadAsStringAsync().Result;
                        mybytearray = JsonConvert.DeserializeObject<byte[]>(jsonString);

                        MemoryStream memoryStream = new MemoryStream(mybytearray);
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                        response.Content = new StreamContent(memoryStream);
                        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(MediaType.Pdf);

                        return response;
                    }
                    else
                    {
                        returnObj.Message = Constants.PdfNotAvailable;

                        return Request.CreateResponse(HttpStatusCode.OK, returnObj);
                    }
                }
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
        }
    }
}
