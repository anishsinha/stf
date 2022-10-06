using ExcelDataReader;
using FileHelpers;
using Newtonsoft.Json;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SiteFuel.BAL
{
    public class DemandCaptureDomain : IDemandCaptureDomain
    {
        private readonly DemandCaptureRepository _demandCaptureRepository;
        public DemandCaptureDomain(DemandCaptureRepository demandCaptureRepository)
        {
            _demandCaptureRepository = demandCaptureRepository;
        }

        public List<DemandModel> ProcessCsvFileContent(string csvText, int sourceTypeId)
        {
            var demands = new List<DemandModel>();
            try
            {
                var engine = new FileHelperEngine<DemandCsvModel>();
                var demandCsvModelList = engine.ReadString(csvText).ToList();
                if (demandCsvModelList.Count > 1)
                {
                    demandCsvModelList.Remove(demandCsvModelList.FirstOrDefault());
                    demands = demandCsvModelList.Select(x => new DemandModel()
                    {
                        SiteId = x.SiteId,
                        TankId = x.TankId,
                        StorageId = x.StorageId,
                        CaptureTime = Convert.ToDateTime(x.CaptureTime),
                        ProductName = x.ProductName,
                        GrossVolume = float.Parse(string.IsNullOrEmpty(x.GrossVolume) ? "0" : x.GrossVolume),
                        NetVolume = float.Parse(string.IsNullOrEmpty(x.NetVolume) ? "0" : x.NetVolume),
                        DipTestValue = float.Parse(string.IsNullOrEmpty(x.NetVolume) ? "0" : x.NetVolume),
                        DipTestUoM = TankScaleMeasurement.Litres,
                        Ullage = float.Parse(string.IsNullOrEmpty(x.Ullage) ? "0" : x.Ullage),
                        Level = decimal.Parse(string.IsNullOrEmpty(x.Level) ? "0" : x.Level),
                        // Ignoring temprature at 9
                        WaterGrossLevel = float.Parse(string.IsNullOrEmpty(x.WaterGrossLevel) ? "0" : x.WaterGrossLevel),
                        WaterNetLevel = float.Parse(string.IsNullOrEmpty(x.WaterNetLevel) ? "0" : x.WaterNetLevel),
                        DataSourceTypeId = sourceTypeId,

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureDomain", "ProcessCsvFileContent", ex.Message + " : " + csvText, ex);
            }
            return demands.ToList();
        }

        public DemandModel ProcessData(int sourceTypeId, string lineText, int lineCount, ref string error)
        {
            var demandLines = new List<DemandModel>();
            string originalLineText = lineText;
            DemandModel response = null;
            if (!string.IsNullOrWhiteSpace(lineText))
            {
                try
                {
                    if (lineText.Contains('"'))
                    {
                        var collection = lineText.Split('"');
                        var prodType = collection[2].Replace(',', '-');
                        lineText = $"{collection[0]}{prodType}{collection[3]}";
                    }
                    var dataModel = lineText.Split(',');
                    if (dataModel.Length > 11)
                    {
                        response = new DemandModel
                        {
                            SiteId = dataModel[0],
                            TankId = dataModel[1],
                            StorageId = dataModel[2],
                            CaptureTime = Convert.ToDateTime(dataModel[3]),
                            ProductName = dataModel[4],
                            GrossVolume = float.Parse(string.IsNullOrEmpty(dataModel[5]) ? "0" : dataModel[5]),
                            NetVolume = float.Parse(string.IsNullOrEmpty(dataModel[6]) ? "0" : dataModel[6]),
                            Ullage = float.Parse(string.IsNullOrEmpty(dataModel[7]) ? "0" : dataModel[7]),
                            Level = decimal.Parse(string.IsNullOrEmpty(dataModel[8]) ? "0" : dataModel[8]),
                            // Ignoring temprature at 9
                            WaterGrossLevel = float.Parse(string.IsNullOrEmpty(dataModel[10]) ? "0" : dataModel[10]),
                            WaterNetLevel = float.Parse(string.IsNullOrEmpty(dataModel[11]) ? "0" : dataModel[11]),
                            DataSourceTypeId = sourceTypeId,
                        };
                    }
                    else
                    {
                        LogManager.Logger.WriteDebug("DemandCaptureDomain", "ProcessData", "Skipped Line:" + originalLineText);
                    }
                }
                catch (Exception ex)
                {
                    error = $"Invalid data at line {lineCount} {ex.Message}";
                    LogManager.Logger.WriteException("DemandCaptureDomain", "ProcessData", ex.Message + " : " + error, ex);
                }

            }
            return response;
        }

        public async Task<StatusModel> CreateDemand(List<DemandModel> demandList, int demandCount, int supplierId)
        {
            StatusModel status = new StatusModel();
            var startCount = 0;
            var takeCount = demandCount > 0 ? demandCount : 1000;
            var updatedCount = 0;
            try
            {
                while (startCount < demandList.Count)
                {
                    var demandBunchToSave = demandList.Skip(startCount).Take(takeCount).ToList();
                    var savedCount = await _demandCaptureRepository.CreateDemand(demandBunchToSave, supplierId, null);
                    startCount += savedCount;
                    updatedCount += savedCount;
                }
                status = new StatusModel() { StatusCode = updatedCount, StatusMessage = "Success" };
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureDomain", "CreateDemand", ex.Message, ex);
            }
            return status;
        }

        public async Task<StatusModel> CreateTankDipTest(List<DemandModel> dipTestList, int supplierId)
        {
            StatusModel status = new StatusModel();

            try
            {
                var savedCount = await _demandCaptureRepository.CreateTankDipTest(dipTestList, supplierId);

                if (savedCount > 0)
                {
                    status.StatusMessage = "Dip test successfully created.";
                    status.StatusCode = (int)Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureDomain", "CreateBuyerDipTestDemand", ex.Message, ex);
            }
            return status;
        }
        public async Task<List<TankModel>> GetProcessTanks(DipTestMethod dipTestMethod)
        {
            return await _demandCaptureRepository.GetProcessTanks(DipTestMethod.FranklinFuelSystem);
        }
        public async Task<StatusModel> CreateDemandFromCsv(List<DemandModel> demandList, int demandCount, string fileName, int supplierId, long uid)
        {
            var startCount = 0;
            var updatedCount = 0;
            try
            {
                var takeCount = demandCount > 0 ? demandCount : 1000;
                var fileId = await _demandCaptureRepository.SaveDemandFileInfo(fileName, uid,DipTestMethod.FranklinFuelSystem);
                while (startCount < demandList.Count)
                {
                    var demandBunchToSave = demandList.Skip(startCount).Take(takeCount).ToList();
                    var savedCount = await _demandCaptureRepository.CreateDemand(demandBunchToSave, supplierId, fileId);
                    startCount += savedCount;
                    updatedCount += savedCount;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureDomain", "CreateDemandFromCsv", ex.Message, ex);
            }
            return new StatusModel() { StatusCode = updatedCount, StatusMessage = "Success" };
        }

        public async Task<List<List<DemandCaptureChartViewModel>>> GetDemandCaptureChartDataByTankAndSite(List<int?> assetIds, string siteId, int noOfDays)
        {
            var response = new List<List<DemandCaptureChartViewModel>>();
            try
            {
                response = await _demandCaptureRepository.GetDemandCaptureChartDataByTankAndSite(assetIds, siteId, noOfDays);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureDomain", "GetDemandCaptureChartDataByTank", ex.Message, ex);
            }
            return response;
        }
        public async Task<LongStatusModel> GetLastProcessedUid()
        {
            var response = new LongStatusModel();
            try
            {
                response = await _demandCaptureRepository.GetLastProcessedUid();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureDomain", "GetLastProcessedUid", ex.Message, ex);
            }
            return response;
        }

        public async Task<CreateDRTankModel> GetDemands(int companyId, int? jobId, string regionId, string buyerJobs, int sourceTypeId, bool isCreateDR)
        {
            CreateDRTankModel response = new CreateDRTankModel();
            try
            {
                if (jobId != null && jobId > 0)
                {
                    response = await GetDemandsForSite(jobId.Value, sourceTypeId, companyId, isCreateDR);
                }
                else
                {
                    //get demands for all sites
                    response = await GetDemandsForRegion(companyId, regionId, buyerJobs, sourceTypeId, isCreateDR);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureDomain", "GetDemands", ex.Message, ex);
            }
            return response;
        }

        private async Task<CreateDRTankModel> GetDemandsForSite(int jobId, int sourceTypeId, int companyId, bool isCreateDR)
        {
            var allSitesDemands = new CreateDRTankModel(); 
            try
            {
                allSitesDemands = await _demandCaptureRepository.GetTankDetailsWithDipTestData(jobId, sourceTypeId, companyId, isCreateDR);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureDomain", "GetDemandsForSite", ex.Message, ex);
            }
            return allSitesDemands;
        }

        private async Task<CreateDRTankModel> GetDemandsForRegion(int companyId, string regionId, string buyerJobs, int sourceTypeId, bool isCreateDR)
        {
            var allSitesDemands = new CreateDRTankModel();
            try
            {
                allSitesDemands = await _demandCaptureRepository.GetTankDetailsForRegion(companyId, regionId, buyerJobs,sourceTypeId, isCreateDR);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureDomain", "GetDemandsForRegion", ex.Message, ex);
            }
            return allSitesDemands;
        }

        public async Task<List<Exchange.Utilities.DropdownDisplayExtendedItem>> GetSiteList(int companyId, string regionId, string siteId, int sourceTypeId)
        {
            List<Exchange.Utilities.DropdownDisplayExtendedItem> response = new List<Exchange.Utilities.DropdownDisplayExtendedItem>();
            try
            {
                response = await _demandCaptureRepository.GetSiteList(companyId, regionId, siteId, sourceTypeId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "GetSiteList", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<CustomerJobForCarrierViewModel>> GetJobListForCarrier(int companyId, string regionId, string siteId, int sourceTypeId)
        {
            var response = new List<CustomerJobForCarrierViewModel>();
            try
            {
                response = await _demandCaptureRepository.GetJobListForCarrier(companyId, regionId, siteId, sourceTypeId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "GetJobListForCarrier", ex.Message, ex);
            }
            return response;
        }
        
        public async Task<bool> ProcessPedigreeData(PedegreeConfigurationModel model)
        {
            var response = false;
            try
            {
                response = await _demandCaptureRepository.ProcessPedigreeTanks(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "ProcessPedigreeData", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<CustomerJobForCarrierViewModel>> GetBrokerJobListForCarrier(int companyId,string regionId)
        {
            var response = new List<CustomerJobForCarrierViewModel>();
            try
            {
                response = await _demandCaptureRepository.GetBrokerJobListForCarrier(companyId, regionId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "GetBrokerJobListForCarrier", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<int>> GetBrokerJobOrderDetails(int companyId, List<int> OrderId)
        {
            var response = new List<int>();
            try
            {
                response = await _demandCaptureRepository.GetBrokerJobOrderDetails(companyId, OrderId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "GetBrokerJobOrderDetails", ex.Message, ex);
            }
            return response;
        }
        private async Task<int> CreateDemandFromTropicOilModel(List<TropicOilCompanyDemandModel> TropicdemandList, List<DropdownDisplayExtendedItem> jobWithTimezone, string fileName="TropicOilCompany", int supplierId=0)
        {
            var savedCount = 0;
            try
            {
                //saving file name only for reference
                var fileId = await _demandCaptureRepository.SaveDemandFileInfo(fileName, 0,DipTestMethod.Skybitz);
                if (fileId > 0)
                {
                    var demandList = await _demandCaptureRepository.GetTropicOilTanksDemandModel(TropicdemandList, jobWithTimezone);
                    savedCount = await _demandCaptureRepository.CreateDemand(demandList, supplierId, fileId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureDomain", "CreateDemandFromTropicOilModel", ex.Message, ex);
            }
            return savedCount;
        }

        public async Task<bool> ProcessSkybitzData(List<DropdownDisplayExtendedItem> jobWithTimezone, List<FTPConfig> config)
        {
            var res = false;
            try
            {

                LogManager.Logger.WriteDebug("DemandCaptureDomain", "ProcessSkybitzData", "Start Processing Demand Capture Skybitz");
                //Read file content from ftp server
                foreach (var item in config)
                {
                    if (!string.IsNullOrWhiteSpace(item.Host))
                    {
                        Stream responseStream = ReadFTPRemoteFile(item.Host, item.UserName, item.Password);
                        string csvText = new StreamReader(responseStream).ReadToEnd();

                        if (!string.IsNullOrEmpty(csvText))
                        {
                            return await SaveTropicOilList(csvText, jobWithTimezone);
                        }
                    }
                }               
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureDomain", "ProcessData", ex.Message, ex);
            }
            return res;
        }
        //skybitz
        private Stream ReadFTPRemoteFile(string hostname, string username, string password)
        {
            Stream responseStream = new MemoryStream();
            try
            {
                //connecting with the ftp server
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(hostname);
                //connection establish
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(username, password);
                request.KeepAlive = false;
                request.UseBinary = true;
                request.UsePassive = true;
                //get response from the remote path
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                responseStream = response.GetResponseStream();

                return responseStream;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //stkbitz
        private async Task<bool> SaveTropicOilList(string csvText, List<DropdownDisplayExtendedItem> jobWithTimezone)
        {
            try
            {
                Console.WriteLine("Save TropicOilCompanyCSV content- Start");
                var engine = new FileHelperEngine<TropicOilCompanyDemandModel>();
                var csvList = engine.ReadString(csvText).ToList();
                csvList.RemoveAt(0);//TO REMOVE CSV HEADER
                var demandCaptureDomain = new DemandCaptureDomain(new DemandCaptureRepository());
                var result = await demandCaptureDomain.CreateDemandFromTropicOilModel(csvList, jobWithTimezone);
                Console.WriteLine("Save TropicOilCompanyCSV content- End");
                if (result > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureDomain", "SaveTropicOilList", ex.Message, ex);
            }
            return false;
        }

        public HttpWebRequest CreateSOAPWebRequestForSkybitz(FTPConfig config)
        {
            //Making Web Request    
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@""+config.Url);
            //SOAPAction    
            Req.Headers.Add(@"SOAPAction:"+config.SoapAction);
            //Content_type    
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            //HTTP method    
            Req.Method ="POST";
            //return HttpWebRequest    
            return Req;
        }

        private Root GetSkybitzAPIResponse(FTPConfig config,int transactionId=0 )
        {
                //Calling CreateSOAPWebRequest method    
                    HttpWebRequest request = CreateSOAPWebRequestForSkybitz(config);
                    XmlDocument SOAPReqBody = new XmlDocument();
                    //SOAP Body Request    
                    SOAPReqBody.LoadXml(@"<?xml version='1.0' encoding='utf-8'?>
                    <soap:Envelope xmlns:soap = 'http://schemas.xmlsoap.org/soap/envelope/'
                    xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
                    xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                    <soap:Body>
                    <GetInventory xmlns='http://tempuri.org/'>
                       <iConsumerId>"+ config.ConsumerId + @"</iConsumerId>
                       <sAuthentication>"+ config.Authentication + @"</sAuthentication>
                       <iAckTransactionID>"+transactionId+ @"</iAckTransactionID>
                    </GetInventory> 
                    </soap:Body>   
                    </soap:Envelope>");

                    using (Stream stream = request.GetRequestStream())
                    {
                        SOAPReqBody.Save(stream);
                    }
                    //Geting response from request    
                    using (WebResponse Serviceres = request.GetResponse())
                    {
                        using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                        {
                            //reading stream    
                            var ServiceResult = rd.ReadToEnd();
                            //writting stream result on console    
                            XDocument xdoc = XDocument.Parse(ServiceResult);
                            XElement xEle = XDocument.Parse(ServiceResult).Root;
                            XmlNode node = new XmlDocument().ReadNode(xEle.CreateReader()) as XmlNode;
                            string jsonText = JsonConvert.SerializeXmlNode(node);
                            return JsonConvert.DeserializeObject<Root>(jsonText);
                        }
                    }           
        }
        //skybitz api process starts here
        public async Task<bool>  InvokeSkyBitzService(List<DropdownDisplayExtendedItem> jobWithTimezone,List<FTPConfig>config)
        {
         try {
                var inventoryList = new List<Inventory>();

                foreach (var item in config)
                {
                    if (!string.IsNullOrWhiteSpace(item.Url))
                    {
                        bool allpagesDone = false;
                        int transactionId = 0;
                        while (!allpagesDone)
                        {
                            var apiResponse = GetSkybitzAPIResponse(item, transactionId);
                            if (apiResponse != null)
                            {
                                inventoryList.AddRange(apiResponse.SoapEnvelope.SoapBody.GetInventoryResponse.GetInventoryResult.Inventory);
                                if (transactionId == Convert.ToInt32(apiResponse.SoapEnvelope.SoapBody.GetInventoryResponse.iTransactionId))
                                {
                                    allpagesDone = true;
                                    break;
                                }
                                transactionId = Convert.ToInt32(apiResponse.SoapEnvelope.SoapBody.GetInventoryResponse.iTransactionId);
                            }

                        }
                    }
                    
                }                
                await processSkybitzResponse(inventoryList, jobWithTimezone);
             }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureDomain", "InvokeSkyBitzService", ex.Message, ex);
                return false;
            }
            return true;
        }

        private  async Task processSkybitzResponse(List<Inventory> apiResponse,List<DropdownDisplayExtendedItem> jobWithTimezone)
        {
            var demandList = await _demandCaptureRepository.GetTropicOilAPITanksDemandModel(apiResponse, jobWithTimezone);
            await _demandCaptureRepository.CreateDemand(demandList,0);
        }
        //process is360 datasource tanks
        public async Task<bool> ProcessIS360(ExternalTankConfigurationModel model)
        {
            LogManager.Logger.WriteDebug("DC", "ProcessIS360", "connectionInFO=>"+ model.ConnectionInfo);
            FTPConfig config = JsonConvert.DeserializeObject<FTPConfig>(model.ConnectionInfo);
            List<Is360DemandModel> demandList = new List<Is360DemandModel>();
            using (SftpClient sftp = new SftpClient(config.Host, config.UserName, config.Password))
            {
                try
                {
                    sftp.Connect();
                    LogManager.Logger.WriteDebug("DC", "ProcessIS360", "connection establish");
                    var files = sftp.ListDirectory(config.RemoteDirectory);
                    LogManager.Logger.WriteDebug("DC", "ProcessIS360", "files");
                    if (files != null && files.Any())
                    {
                        var file = files.Where(w => w.Name != null && w.Name != "" && w.Name != "." && w.Name != "..").OrderByDescending(o => o.LastWriteTimeUtc).FirstOrDefault();
                      bool res=  await _demandCaptureRepository.IsIS360FileExists(file.Name,DipTestMethod.Insight360);
                        if (!res)
                        {
                            await ReadIS360File(sftp, file, demandList);

                            //foreach (var file in files)
                            //{
                            //    if (!string.IsNullOrEmpty(file.Name) && file.Name != "." && file.Name != "..")
                            //      await ReadIS360File(sftp, file, demandList);
                            //}
                            sftp.Disconnect();
                            LogManager.Logger.WriteDebug("DC", "ProcessIS360", "Disconnected");
                            await CreateDemandFromIS360Model(demandList, model, file.Name);
                            return true;
                        }
                }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DemandCaptureDomain", "ProcessIS360", ex.Message, ex);
                }
                return true;
            }
        }
        
        private  async Task ReadIS360File(SftpClient client, SftpFile file, List<Is360DemandModel> demandList)
        {
            var stream = new MemoryStream();
         //   Console.WriteLine(@"sftp.fuelquest.com{0}",file.FullName);
            using (Stream remoteTempFile = client.OpenWrite(file.FullName))
            {
                client.DownloadFile(file.FullName, stream);
                LogManager.Logger.WriteDebug("DC", "ProcessIS360", "readed");
                //  Console.WriteLine("readed");
                stream.Seek(0, SeekOrigin.Begin);
              await ConvertExcelToList(file.FullName, stream, demandList);
                LogManager.Logger.WriteDebug("DC", "ProcessIS360", "ConvertExcelToList");
            }
        }

        private  async Task ConvertExcelToList(string filePath, Stream stream, List<Is360DemandModel> demandList)
        {
            IExcelDataReader excelReader = null;
            DataTable dataTable = null;
            excelReader =  ExcelReaderFactory.CreateBinaryReader(stream);
            DataSet result = excelReader.AsDataSet();
            if (result != null && result.Tables.Count > 0)
            {
                dataTable = result.Tables[0];
                DataRow fdr = dataTable.Rows[0];
                int i=0;//index
                foreach (DataColumn col in fdr.Table.Columns)
                {
                    col.ColumnName = fdr[i].ToString();
                    i++;
                }
                List<Is360DemandModel> lst = (from DataRow dr in dataTable.Rows
                                              select new Is360DemandModel()
                                              {
                                                  CustomerName = dr["Customer Name"].ToString(),
                                                  TankName = dr["Tank Name"].ToString(),
                                                  InventoryReadingDate = dr["Inventory Reading Date"].ToString(),
                                                  InventoryVolume = dr["Inventory Volume"].ToString(),
                                                  TankLegacyId = dr["Tank Legacy Id"].ToString(),
                                                  WaterLevel = dr["Water Level"].ToString(),
                                              }).ToList();
                lst.RemoveAt(0);//Remove header
             demandList.AddRange(lst);
            }
        }

        private async Task<int> CreateDemandFromIS360Model(List<Is360DemandModel> IS360demandList, ExternalTankConfigurationModel model, string fileName = "IS360", int supplierId = 0)
        {
            var savedCount = 0;
            try
            {
                //saving file name only for reference
                var fileId = await _demandCaptureRepository.SaveDemandFileInfo(fileName, 0, DipTestMethod.Insight360);
                if (fileId > 0)
                {
                    var demandList = await _demandCaptureRepository.GetIS360TanksDemandModel(IS360demandList, model);
                    savedCount = await _demandCaptureRepository.CreateDemand(demandList, supplierId, fileId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureDomain", "CreateDemandFromIS360Model", ex.Message, ex);
            }
            return savedCount;
        }

        public async Task<bool> ProcessVedorRoot()
        {
           return await _demandCaptureRepository.ProcessVedorRootTanks();
        }
    }
}
