using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.ViewModels.AccountingEvent;
using SiteFuel.Exchange.ViewModels.Queue;
using System.Data.Entity;
using SiteFuel.Exchange.Domain.Domain;
using System.Net.Http;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;

namespace SiteFuel.Exchange.Domain.Domain.ThirdParty
{
    public class SAPEnterpriseDomain : BaseDomain
    {
        public SAPEnterpriseDomain(BaseDomain domain)
           : base(domain)
        {
        }
        public SAPEnterpriseDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public async Task<List<string>> ProcessDeliveryDetailsToSAP(PDIAPIRequestViewModel viewModel)
        {
            var errorInfo = new List<string>();
            using (var tracer = new Tracer("PDIEnterpriseDomain", "ProcessDeliveryDetailsToPDI"))
            {
                
                try
                {
                    if (viewModel !=null && viewModel.InvoiceHeaderId > 0)
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        var deliveryDetails = await spDomain.GetDeliveryDetailsForSAP(viewModel.InvoiceHeaderId);
                        if (deliveryDetails!=null && deliveryDetails.Any())
                        {
                            SAPDrXml convertToxmlViewModel = ConvertToSAPViewModel(deliveryDetails);
                            bool isSent =  await PostSAPDelivertDetails(convertToxmlViewModel, viewModel.InvoiceHeaderId);
                        }

                    }
                }
                catch(Exception ex)
                {
                    LogManager.Logger.WriteException("SAPEnterpriseDomain", "ProcessDeliveryDetailsToSAP", ex.Message, ex);
                }
                return errorInfo;
            }
        }

        private SAPDrXml ConvertToSAPViewModel(List<SAPDeliveryDetailsViewModel> deliveryDetails)
        {
            SAPDrXml response = new SAPDrXml();

            var groupByBol = deliveryDetails.GroupBy(t => new { t.LiftTicketNo, t.TFXOrderNo }).ToList();
            foreach (var bolgroup in groupByBol)
            {
                var distinctBol = bolgroup.ToList();
                foreach (var uniqueBol in distinctBol)
                {
                    SAPDeliveryDetailsXMLViewModel detailsViewModel = new SAPDeliveryDetailsXMLViewModel
                    {
                        ExternalOrderNo = uniqueBol.TFXOrderNo,
                        DropTicketNo = uniqueBol.DropTicketNo,
                        LiftTicketNo = uniqueBol.LiftTicketNo,
                        TerminalControl = uniqueBol.TerminalControl,
                        CustomerID = uniqueBol.CustomerID,
                        LocationID = uniqueBol.LocationID,
                        LiftDate = uniqueBol.LiftDate,
                        LiftStartTime = uniqueBol.LiftStartTime,
                        LiftEndTime = uniqueBol.LiftEndTime,
                        SAP_DocNumber = uniqueBol.SAPOrdNumber,
                        TruckID = uniqueBol.TruckId
                    };

                    detailsViewModel.Products.Add(new SAPXmlProductModel()
                    {
                        Price = uniqueBol.Price.GetPreciseValue(6),
                        ProductID = uniqueBol.ProductID,
                        TotalDropQtyGross = uniqueBol.TotalDropQuantity,
                        TotalDropQtyNet = uniqueBol.TotalDropQuantity,
                    });

                    response.DeliveryProcessingDetails.Add(detailsViewModel);
                }
            }

            return response;
        }

        public bool CreateSAPWorkflow(Invoice invoice)
        {
            bool isRequestProcessed = true;
            try
            {
               
                if (invoice != null && invoice.Order != null && invoice.TrackableScheduleId.HasValue && invoice.TrackableScheduleId.Value > 0)
                {
                    var creditCheckType = Context.DataContext.OnboardingPreferences.Where(t => t.IsActive && t.CompanyId == invoice.Order.AcceptedCompanyId).Select(t => t.CreditCheckType).FirstOrDefault();
                    if (creditCheckType == CreditCheckTypes.SAP)
                    {
                        var jsonViewModel = new PDIAPIRequestViewModel();
                        jsonViewModel.InvoiceHeaderId = invoice.InvoiceHeaderId;
                        jsonViewModel.InvoiceNumber = invoice.DisplayInvoiceNumber;
                        //jsonViewModel.OrderId = order.Id;
                        AddQueueEventCreateSAP(jsonViewModel, invoice.UpdatedBy);
                    }
                }
            }
            catch (Exception ex)
            {
                isRequestProcessed = false;
                LogManager.Logger.WriteException("SAPEnterpriseDomain", "CreateSAPWorkflow", ex.Message, ex);
            }
            return isRequestProcessed;
        }
        public void AddQueueEventCreateSAP(PDIAPIRequestViewModel viewModel, int userId)
        {
            QueueMessageDomain queueMessageDomain = new QueueMessageDomain();
            string json = JsonConvert.SerializeObject(viewModel);
            var queueRequest = new QueueMessageViewModel()
            {
                CreatedBy = userId,
                QueueProcessType = QueueProcessType.SAPAPIDeliveryDetails,
                JsonMessage = json
            };
            queueMessageDomain.EnqeueMessage(queueRequest);
        }


        private async Task<bool> PostSAPDelivertDetails(SAPDrXml deliveryDetails, int invoiceHeaderId)
        {
            try
            {

                var connectionData = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.SAPDrDetailSendUrl
                       || t.Key == ApplicationConstants.SAPUserId
                       || t.Key == ApplicationConstants.SAPPassword).Select(t => new { Key = t.Key, Value = t.Value }).ToList();
                if (connectionData != null && connectionData.Count >= 3)
                {
                    string SAPUrl = connectionData.Where(w => w.Key == ApplicationConstants.SAPDrDetailSendUrl).FirstOrDefault().Value;
                    string userId = connectionData.Where(w => w.Key == ApplicationConstants.SAPUserId).FirstOrDefault().Value;
                    string password = connectionData.Where(w => w.Key == ApplicationConstants.SAPPassword).FirstOrDefault().Value;

                    if (!string.IsNullOrEmpty(SAPUrl) && !string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(password))
                    {
                        using (var client = new HttpClient())
                        {
                            string json = string.Empty;
                            try
                            {
                                client.Timeout = TimeSpan.FromSeconds(300);
                                var byteArray = Encoding.ASCII.GetBytes(userId + ":" + password);
                                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));


                                json = JsonConvert.SerializeObject(deliveryDetails, Newtonsoft.Json.Formatting.Indented);
                                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                                HttpResponseMessage apiResponse = await client.PostAsync(SAPUrl, stringContent);
                                var apiResult = await apiResponse.Content.ReadAsStringAsync();

                                if (apiResponse.IsSuccessStatusCode)
                                {
                                    AddApiLogEntryForSAP(1, SAPUrl, json, apiResult, apiResponse.IsSuccessStatusCode, invoiceHeaderId);
                                }
                                else
                                {
                                    AddApiLogEntryForSAP(1, SAPUrl, json, "PostSAPDelivertDetails failed", false, invoiceHeaderId);
                                }
                            }
                            catch (Exception)
                            {
                                AddApiLogEntryForSAP(1, SAPUrl, json, "PostSAPDelivertDetails failed", false, invoiceHeaderId);
                                throw;
                            }
                        }
                    }
                }
                return false;
            }

            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SAPEnterpriseDomain", "PostSAPDelivertDetails", ex.Message, ex);
            }
            return false;
        }

        private static void AddApiLogEntryForSAP(int companyId, string requestUrl, string json, string apiResult, bool isSucess,int invoiceHeaderId)
        {
            var apiLog = new ApiLog();
            apiLog.Request = json;
            apiLog.Response = string.IsNullOrWhiteSpace(apiResult) ? "Received Empty response from SAP for invoiceHeaderId+ "+invoiceHeaderId.ToString() : apiResult.ToString();
            apiLog.CreatedBy = 1;
            apiLog.Url = requestUrl;
            apiLog.CompanyId = companyId;
            apiLog.ExternalRefID = DateTimeOffset.Now.ToString();
            apiLog.CreatedDate = DateTimeOffset.Now;
            apiLog.Message = "PostSAPDelivertDetails called for invoiceHeaderId"+ invoiceHeaderId.ToString();
            apiLog.IsSuccessStatusCode = isSucess;

            var logDomain = new ExceptionLogDomain();
            logDomain.AddApiLogs(apiLog);
        }

        #region Retry Failed Location-create requests
        public async Task<bool> ProcessFailedLocationCreateRequests()
        {
            //get app settings for company ids
            var companyIdsForFailedReqs = await Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.SAPFailedRequestRetryForCompanies)
                .Select(t => t.Value).FirstOrDefaultAsync();

            if (!string.IsNullOrWhiteSpace(companyIdsForFailedReqs))
            {
                var validCompanyIds = companyIdsForFailedReqs.TrimStart(',').Split(',').Distinct()
                                            .Where(t => int.TryParse(t, out int testInt))
                                            .Select(t => int.Parse(t))
                                            .Where(t => t > 0)
                                            .ToList();

                var jobDomain = new JobDomain(this);

                foreach (var compId in validCompanyIds)
                {
                    //get failed requests of each company
                    var failedRequests = Context.DataContext.ApiLogs.Where(t => t.CompanyId == compId
                                                                    && t.Url == "Location-Create"
                                                                    && t.Response.Contains("Invalid External Ref Id")
                                                                    && t.Message == "1"
                                                                    && t.RetryCount == 0)
                                                                    .Select(t => new { t.Id, t.Request, t.CreatedBy }).ToList();
                    if (failedRequests != null)
                    {
                        var groupbyCreatedUser = failedRequests.GroupBy(t => t.CreatedBy).ToList();
                        foreach (var groupByUsr in groupbyCreatedUser)
                        {
                            var eachUsersRequests = groupByUsr.ToList();
                            if (eachUsersRequests.Any())
                            {
                                var activeToken = Context.DataContext.UserTokens.Where(t => t.UserId == eachUsersRequests.FirstOrDefault().CreatedBy)
                                                                                .OrderByDescending(t => t.Id).Select(t => t.Token).FirstOrDefault();

                                foreach (var item in eachUsersRequests)
                                {
                                    //process those failed requests again
                                    if (!string.IsNullOrWhiteSpace(activeToken))
                                    {
                                        try
                                        {
                                            var locationCreateModel = JsonConvert.DeserializeObject<TPDLocationCreateModel>(item.Request);
                                            if (locationCreateModel != null)
                                            {
                                                var result = await jobDomain.CreateLocationFromAPI(activeToken, locationCreateModel);
                                                await UpdateApilogEntry(item.Id, JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented), result.Status);
                                                LogManager.Logger.WriteDebug("SAPEnterpriseDomain", "ProcessFailedLocationCreateRequests", $"Failed request processed. Requestid = {item.Id}");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            LogManager.Logger.WriteException("SAPEnterpriseDomain", "ProcessFailedLocationCreateRequests", ex.Message, ex);
                                        }
                                    }
                                    else
                                        LogManager.Logger.WriteDebug("SAPEnterpriseDomain", "ProcessFailedLocationCreateRequests", $"Failed request processed. No Token found for UserId = {item.CreatedBy} Requestid = {item.Id}");
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        private async Task UpdateApilogEntry(int id, string response, Status status)
        {
            var apiLogEntry = Context.DataContext.ApiLogs.Where(t => t.Id == id).FirstOrDefault();
            apiLogEntry.Response = JsonConvert.SerializeObject(response, Newtonsoft.Json.Formatting.Indented);
            apiLogEntry.Message = ((int)status).ToString();
            apiLogEntry.RetryCount = apiLogEntry.RetryCount + 1;
            Context.DataContext.Entry(apiLogEntry).State = EntityState.Modified;
            await Context.CommitAsync();
        }

        #endregion
    }
}
