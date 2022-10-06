using Newtonsoft.Json;
using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightApi.Attributes;
using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using DropdownDisplayItem = SiteFuel.Exchange.Utilities.DropdownDisplayItem;

namespace TrueFill.FreightApi.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif
    public class DemandCaptureController : ApiController
    {
        private readonly IDemandCaptureDomain demandCapture;

        public DemandCaptureController(IDemandCaptureDomain _demandCapture)
        {
            demandCapture = _demandCapture;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadData(List<DemandModel> demandList)
        {
            using (var tracer = new Tracer("DemandCaptureController", "UploadData"))
            {
                HttpResponseMessage result = null;
                string msg = string.Empty;
                msg = await SaveDemandListToDb(msg, demandList);
                result = Request.CreateResponse(HttpStatusCode.Created, new { Message = msg });
                return result;
            }
        }

        [ValidateToken]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateDemands(int sourceTypeId = (int)DipTestMethod.Incon)
        {
            using (var tracer = new Tracer("DemandCaptureController", $"CreateDemands(sourceTypeId:{sourceTypeId})"))
            {
                HttpResponseMessage result = null;
                string msg = string.Empty;
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        try
                        {
                            var postedFile = httpRequest.Files[file];
                            var demandLines = new List<DemandModel>();
                            msg = ProcessFileToDemandList(sourceTypeId, msg, postedFile, demandLines);
                            msg = await SaveDemandList(msg, postedFile, demandLines);
                        }
                        catch (Exception ex)
                        {
                            LogManager.Logger.WriteException("DemandCapture", "CreateDemands", ex.Message, ex);
                            msg = "Service error";
                            result = Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = msg });
                        }
                    }
                    result = Request.CreateResponse(HttpStatusCode.Created, new { Message = msg });
                }
                else
                {
                    msg = "No valid file found to process";
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, new { Message = msg });
                }
                return result;
            }
        }

        private async Task<string> SaveDemandList(string error, HttpPostedFile postedFile, List<DemandModel> demandLines)
        {
            using (var tracer = new Tracer("DemandCaptureController", $"SaveDemandList(postedFile: {postedFile?.FileName})"))
            {
                error = await SaveDemandListToDb(error, demandLines);
                error = await SaveFileToAzure(error, postedFile);
                return error;
            }
        }

        private async Task<string> SaveDemandListToDb(string error, List<DemandModel> demandLines, int supplierId = 1)
        {
            using (var tracer = new Tracer("DemandCaptureController", $"SaveDemandListToDb(supplierId:{supplierId})"))
            {
                int.TryParse(ConfigurationManager.AppSettings["DemandProcessCount"].ToString(), out int processCount);
                //TODO: SupplierId need to be calculated.
                var response = await demandCapture.CreateDemand(demandLines, processCount, supplierId);
                if (response.StatusCode >= 1)
                    error = "All data from file processed successfully";
                return error;
            }
        }

        [HttpPost]
        public async Task<StatusModel> CreateTankDipTest(List<DemandModel> demandLines, int supplierId = 0)
        {
            var response = new StatusModel();

            try
            {
                using (var tracer = new Tracer("DemandCaptureController", $"CreateTankDipTest(supplierId:{supplierId})"))
                {
                    response = await demandCapture.CreateTankDipTest(demandLines, supplierId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureController", "CreateTankDipTest", ex.Message, ex);
            }
            return response;
        }
        [HttpPost]
        public async Task<List<List<DemandCaptureChartViewModel>>> GetDemandCaptureChartDataByTankAndSite(List<int?> assetIds, string siteId, int noOfDays)
        {
            var response = new List<List<DemandCaptureChartViewModel>>();
            try
            {
                using (var tracer = new Tracer("DemandCaptureController", "GetDemandCaptureChartDataByTankAndSite"))
                {
                    response = await demandCapture.GetDemandCaptureChartDataByTankAndSite(assetIds, siteId, noOfDays);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureController", "GetDemandCaptureChartDataByTankAndSite", ex.Message, ex);
            }
            return response;
        }
        private static async Task<string> SaveFileToAzure(string error, HttpPostedFile postedFile)
        {
            using (var tracer = new Tracer("DemandCaptureController", $"SaveFileToAzure(request: {postedFile?.FileName})"))
            {
                var azureBlob = new AzureBlobStorage();
                var filePath = await azureBlob.SaveBlobAsync(postedFile.InputStream, $"{DateTime.Now.Ticks}-{postedFile.FileName}", "dcincon");
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    error = $"{error} - Failed to save file on Azure blob";
                }
                return error;
            }
        }

        private string ProcessFileToDemandList(int sourceTypeId, string error, HttpPostedFile postedFile, List<DemandModel> demandLines)
        {
            using (var tracer = new Tracer("DemandCaptureController", $"ProcessFileToDemandList(postedFile:{postedFile?.FileName})"))
            {
                using (StreamReader reader = new StreamReader(postedFile.InputStream))
                {
                    string line;
                    int lineCount = 1;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (lineCount > 1)
                        {
                            var demand = demandCapture.ProcessData(sourceTypeId, line, lineCount, ref error);
                            if (demand != null)
                                demandLines.Add(demand);
                        }
                        lineCount++;
                    }
                }
                return error;
            }
        }

        [HttpPost]
        public async Task<CreateDRTankModel> GetDemands(DemandInputModel model)
        {
            using (var tracer = new Tracer("DemandCaptureController", $"GetDemands(companyId:{model.CompanyId}, jobId:{model.JobId}, regionId: {model.RegionId},buyerJobs: {model.BuyerJobs}, sourceTypeId:{model.SourceTypeId})"))
            {
                string msg = string.Empty;
                var response = await demandCapture.GetDemands(model.CompanyId, model.JobId, model.RegionId, model.BuyerJobs, model.SourceTypeId, model.IsCreateDR);
                return response;
            }
        }

        [HttpGet]
        public async Task<List<DropdownDisplayExtendedItem>> GetSites(int companyId, string regionId, string siteId = "", int sourceTypeId = 1)
        {
            using (var tracer = new Tracer("DemandCaptureController", $"GetSites(companyId:{companyId}, siteId:{siteId}, regionId: {regionId}, sourceTypeId:{sourceTypeId})"))
            {
                string msg = string.Empty;
                var response = await demandCapture.GetSiteList(companyId, regionId, siteId, sourceTypeId);
                return response;
            }
        }

        [HttpGet]
        public async Task<List<CustomerJobForCarrierViewModel>> GetJobListForCarrier(int companyId, string regionId, string siteId = "", int sourceTypeId = 1)
        {
            using (var tracer = new Tracer("DemandCaptureController", $"GetJobListForCarrier(companyId:{companyId}, siteId:{siteId}, regionId: {regionId}, sourceTypeId:{sourceTypeId})"))
            {
                string msg = string.Empty;
                var response = await demandCapture.GetJobListForCarrier(companyId, regionId, siteId, sourceTypeId);
                return response;
            }
        }

        [HttpPost]
        public async Task<bool> ProcessPedigree(PedegreeConfigurationModel model)
        {
            bool response = false;


            using (var tracer = new Tracer("DemandCaptureController", $"ProcessPedigree"))
            {
                response = await demandCapture.ProcessPedigreeData(model);
            }
            return response;
        }
        [HttpGet]
        public async Task<List<CustomerJobForCarrierViewModel>> GetBrokerJobListForCarrier(int companyId, string regionId)
        {
            using (var tracer = new Tracer("DemandCaptureController", $"GetBrokerJobListForCarrier(companyId:{companyId},regionId: {regionId}"))
            {
                string msg = string.Empty;
                var response = await demandCapture.GetBrokerJobListForCarrier(companyId, regionId);
                return response;
            }
        }
        [HttpPost]
        public async Task<List<int>> GetBrokerJobOrderDetails(int companyId, List<int> OrderId)
        {
            using (var tracer = new Tracer("DemandCaptureController", $"GetBrokerJobOrderDetails(companyId:{companyId},OrderIdCount: {OrderId.Count}"))
            {
                var response = await demandCapture.GetBrokerJobOrderDetails(companyId, OrderId);
                return response;
            }
        }
        
        [HttpPost]
        public async Task<bool> ProcessSkybitz(List<DropdownDisplayExtendedItem> model)
        {
            bool response = false;
            using (var tracer = new Tracer("DemandCaptureController", $"ProcessSkybitz"))
            {
                if(model !=null & model.Any())
                {
                    var config = JsonConvert.DeserializeObject<List<FTPConfig>>(model.FirstOrDefault().Code);
                    response = await demandCapture.ProcessSkybitzData(model, config);
                }
                
            }
            return response;
        }
        public async Task<bool> ProcessSkybitzAPI(List<DropdownDisplayExtendedItem> model)
        {
            bool response = false;
            using (var tracer = new Tracer("DemandCaptureController", $"ProcessSkybitzAPI"))
            {
                if (model != null & model.Any())
                {
                    var config = JsonConvert.DeserializeObject<List<FTPConfig>>(model.FirstOrDefault().Code);
                    response = await demandCapture.InvokeSkyBitzService(model, config);
                }

            }
            return response;
        }
        [HttpPost]
        public async Task<bool> ProcessIs360(ExternalTankConfigurationModel model)
        {
            bool response = false;
            using (var tracer = new Tracer("DemandCaptureController", $"ProcessIs360"))
            {
                if (model != null)
                {
                    response = await demandCapture.ProcessIS360(model);
                }

            }
            return response;
        }
    }
}
