using FileHelpers;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace SiteFuel.Exchange.Domain
{
    public class PoNumberBulkUploadDomain : InvoiceCommonDomain
    {
        public PoNumberBulkUploadDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public PoNumberBulkUploadDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<StatusViewModel> ValidatePoNumberBulkFile(UserContext userContext, string csvText, string csvFilePath, CompanyType companyType)
        {
            using (var tracer = new Tracer("PoNumberBulkUploadDomain", "ValidatePoNumberBulkFile"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    var csvHeaderLine = Regex.Matches(csvText.Trim(), @"\A.*").Cast<Match>().FirstOrDefault();
                    string[] lines = File.ReadAllLines(csvFilePath);
                    //header validations
                    string headerLine = lines.FirstOrDefault();
                    if (csvHeaderLine.Value.Trim() != headerLine)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageBulkUploadHeaderMismatch;
                        return response;
                    }

                    csvText = RemoveHeaderAndGuidelinesFromFile(csvText);

                    var engine = new FileHelperEngine<PoNumberBulkCsvViewModel>();
                    var csvOrderList = engine.ReadString(csvText).ToList();

                    List<string> invoiceNumbers = csvOrderList.Select(t => t.InvoiceNumber.Trim()).ToList();
                    var allInvoices = await Context.DataContext.Invoices.Where(t => t.IsActive
                                        && invoiceNumbers.Contains(t.DisplayInvoiceNumber)
                                        && t.Order.IsProFormaPo && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                        && (t.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && t.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp &&
                                                t.InvoiceTypeId != (int)InvoiceType.CreditInvoice && t.InvoiceTypeId != (int)InvoiceType.PartialCredit)
                                        && (companyType == CompanyType.Buyer ? t.Order.BuyerCompanyId == userContext.CompanyId : t.Order.AcceptedCompanyId == userContext.CompanyId)
                                        && (companyType == CompanyType.Buyer ? t.Order.FuelRequest.FuelRequestTypeId != (int)FuelRequestType.ThirdPartyRequest : t.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest))
                                        .Select(t => new
                                        {
                                            t.DisplayInvoiceNumber,
                                            t.Id
                                        }).ToListAsync();

                    int lineNumberOfCSV = 1;
                    foreach (var record in csvOrderList)
                    {
                        if (CheckIfItsEmptyLine(record))
                            break;

                        lineNumberOfCSV++;

                        //Required field validation
                        if (IsRequiredFieldMissing(record))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageBulkUploadRequiredFieldsAreMissing, lineNumberOfCSV);
                            return response;
                        }

                        var invoice = allInvoices.FirstOrDefault(t => t.DisplayInvoiceNumber == record.InvoiceNumber.Trim());
                        if (invoice == null)
                        {
                            response.StatusCode = Status.Failed;
                            if (record.InvoiceNumber.StartsWith(ApplicationConstants.SFDD) || record.InvoiceNumber.StartsWith("SFDD"))
                                response.StatusMessage = string.Format(Resource.errMessageInvalidDDTNumber, record.InvoiceNumber);
                            else
                                response.StatusMessage = string.Format(Resource.errMessageInvalidInvoiceNumber, record.InvoiceNumber);
                            return response;
                        }
                    }

                    if (lineNumberOfCSV - 1 > 0)
                    {
                        response.StatusCode = Status.Success;
                        response.StatusMessage = string.Format(Resource.successMessageForBulkUpload, (lineNumberOfCSV - 1));
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageCanNotProcessZeroRecords;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("PoNumberBulkUploadDomain", "ValidatePoNumberBulkFile", ex.Message, ex);
                }

                return response;
            }
        }

        private bool CheckIfItsEmptyLine(PoNumberBulkCsvViewModel record)
        {
            return string.IsNullOrWhiteSpace(record.InvoiceNumber) && string.IsNullOrWhiteSpace(record.NewPoNumber);
        }

        private bool IsRequiredFieldMissing(PoNumberBulkCsvViewModel record)
        {
            return string.IsNullOrWhiteSpace(record.InvoiceNumber) || string.IsNullOrWhiteSpace(record.NewPoNumber);
        }

        public string RemoveHeaderAndGuidelinesFromFile(string csvText)
        {
            csvText = Regex.Replace(csvText.Trim(), @"\A.*", string.Empty, RegexOptions.IgnoreCase);
            csvText = Regex.Replace(csvText.Trim(), @",\n", string.Empty, RegexOptions.IgnoreCase);
            csvText = csvText.TrimEnd(',');
            return csvText;
        }

        public async Task<StatusViewModel> UploadFileToBlob(UserContext userContext, Stream fileStream, string fileName, CompanyType companyType)
        {
            using (var tracer = new Tracer("PoNumberBulkUploadDomain", "UploadFileToBlob"))
            {
                var response = new StatusViewModel(Status.Failed);
                try
                {
                    var azureBlob = new AzureBlobStorage();

                    var filePath = await azureBlob.SaveBlobAsync(fileStream, GenerateFileName(userContext.Id), BlobContainerType.PoNumberBulkUpload.ToString().ToLower());
                    if (!string.IsNullOrWhiteSpace(filePath))
                    {
                        var queueDomain = new QueueMessageDomain();
                        var queueRequest = GetEnqueueMessageRequestViewModel(userContext, filePath, companyType);
                        var queueId = queueDomain.EnqeueMessage(queueRequest);

                        response.StatusCode = Status.Success;
                        response.StatusMessage = string.Format(Resource.successMessageOrderBulkWithRequestNo, string.Concat(Constants.SFXOrderBulkUploadSuffix, queueId.ToString("000")));
                    }
                    else
                        response.StatusMessage = Resource.errMessageErrorInAzureServer;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageErrorInAzureServer;
                    LogManager.Logger.WriteException("PoNumberBulkUploadDomain", "UploadFileToStorage", ex.Message, ex);
                }
                return response;
            }
        }

        private QueueMessageViewModel GetEnqueueMessageRequestViewModel(UserContext userContext, string blobStoragePath, CompanyType companyType)
        {
            var jsonViewModel = new PoNumberBulkUploadProcessorReqViewModel();
            jsonViewModel.FileLineNumberToStart = 0;
            jsonViewModel.UserId = userContext.Id;
            jsonViewModel.CompanyId = userContext.CompanyId;
            jsonViewModel.FileUploadPath = blobStoragePath;
            jsonViewModel.CompanyType = companyType;

            string json = JsonConvert.SerializeObject(jsonViewModel);

            return new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = QueueProcessType.PoNumberBulkUpload,
                JsonMessage = json
            };
        }

        private string GenerateFileName(int userId)
        {
            return string.Concat(values: Constants.PoNumberBulk + Resource.lblSingleHyphen + userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + ".csv");
        }

        public string ProcessBulkUploadJsonMessage(PoNumberBulkUploadProcessorReqViewModel bulkRequestViewModel, List<string> errorInfo)
        {
            using (var tracer = new Tracer("PoNumberBulkUploadDomain", "ProcessBulkUploadJsonMessage"))
            {
                StringBuilder processMessage = new StringBuilder();

                try
                {
                    if (!string.IsNullOrWhiteSpace(bulkRequestViewModel.FileUploadPath))
                    {
                        //processing actual bulk file
                        var azureBlob = new AzureBlobStorage();
                        var fileStream = azureBlob.DownloadBlob(bulkRequestViewModel.FileUploadPath, BlobContainerType.PoNumberBulkUpload.ToString().ToLower());
                        if (fileStream != null)
                        {
                            string csvText = new StreamReader(fileStream).ReadToEnd();
                            if (!string.IsNullOrWhiteSpace(csvText))
                            {
                                var filteredCsvText = RemoveHeaderAndGuidelinesFromFile(csvText);
                                var engine = new FileHelperEngine<PoNumberBulkCsvViewModel>();
                                var csvList = engine.ReadString(filteredCsvText).ToList();

                                AuthenticationDomain authenticationDomain = new AuthenticationDomain(this);
                                var context = authenticationDomain.GetUserContextAsync(bulkRequestViewModel.UserId, bulkRequestViewModel.CompanyType).Result;
                                ProcessOrderList(errorInfo, csvList, context, processMessage, bulkRequestViewModel.CompanyType);
                            }
                            else
                            {
                                processMessage.Append(Resource.errMessageFailedToReadFileContent);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                        LogManager.Logger.WriteException("PoNumberBulkUploadDomain", "ProcessBulkUploadJsonMessage", ex.Message, ex);
                    if (processMessage.Length == 0)
                    {
                        processMessage.Append(Constants.ErrorWhileProcessingBulkOrder);
                        errorInfo.Add(processMessage.ToString());
                    }
                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                }
                return processMessage.ToString();
            }
        }

        private void ProcessOrderList(List<string> errorInfo, List<PoNumberBulkCsvViewModel> csvList, UserContext context, StringBuilder processMessage, CompanyType companyType)
        {
            StatusViewModel result = new StatusViewModel();
            var orderDomain = ContextFactory.Current.GetDomain<OrderDomain>();
            var invoiceDomain = ContextFactory.Current.GetDomain<InvoiceDomain>();
            foreach (var item in csvList)
            {
                processMessage.Clear();
                var invoiceNumber = item.InvoiceNumber.Trim();
                var poNumber = item.NewPoNumber.Trim();
                var invoice = Context.DataContext.Invoices.Where(t => t.DisplayInvoiceNumber == invoiceNumber && t.IsActive
                    && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.Order.IsProFormaPo
                     && (t.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && t.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp &&
                                                t.InvoiceTypeId != (int)InvoiceType.CreditInvoice && t.InvoiceTypeId != (int)InvoiceType.PartialCredit)
                    && (companyType == CompanyType.Buyer ? t.Order.BuyerCompanyId == context.CompanyId : t.Order.AcceptedCompanyId == context.CompanyId)
                    && (companyType == CompanyType.Buyer ? t.Order.FuelRequest.FuelRequestTypeId != (int)FuelRequestType.ThirdPartyRequest : t.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest))
                    .Select(t => new
                    {
                        t.Id,
                        t.OrderId,
                        t.PoNumber,
                        t.Order.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                    }).SingleOrDefault();

                try
                {
                    if (invoice != null)
                    {
                        if (invoice.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery && invoice.OrderId != null)
                        {
                            result = orderDomain.EditProFormaPoNumberAsync(context, invoice.OrderId.Value, poNumber).Result;
                        }
                        else if (invoice.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries)
                        {
                            result = invoiceDomain.EditInvoicePoNumberAsync(context, invoice.Id, poNumber).Result;
                        }

                        if (result.StatusCode == Status.Success)
                            errorInfo.Add(SetSuccessProcessMessage(invoiceNumber));
                        else
                        {
                            SetFailedProcessMessage(processMessage, invoiceNumber, result.StatusMessage);
                            errorInfo.Add(processMessage.ToString());
                            throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!errorInfo.Any())
                    {
                        SetFailedProcessMessage(processMessage, invoiceNumber, Constants.ErrorWhileProcessingBulkOrder);
                        errorInfo.Add(processMessage.ToString());
                    }
                    LogManager.Logger.WriteException("PoNumberBulkUploadDomain", "ProcessBulkUploadJsonMessage", "Po Number bulkupload failed", ex);
                }
            }
        }

        private static void SetFailedProcessMessage(StringBuilder processMessage, string invoiceNumber, string message)
        {
            processMessage.Append("<p class='color-maroon'>").Append("<b>Info: </b>")
                        .Append($"Invoice #: {invoiceNumber} <br><b>Processing failed Reason:</b> {message}</p><br>");
        }

        private static string SetSuccessProcessMessage(string invoiceNumber)
        {
            StringBuilder processMessage = new StringBuilder();
            processMessage.Append("<p class='color-green'>").Append("<b>Info: </b>")
                        .Append($"Number(s): Invoice #: {invoiceNumber} <br><b>processed successfully</b></p><br>");
            return processMessage.ToString();
        }

        public async Task<StatusViewModel> ProcessUploadedPoFile(HttpPostedFileBase poCsvFile, UserContext userContext)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    // read file
                    var poList = new List<PoFileCsvViewModel>();
                    using (var stream = new MemoryStream())
                    {
                        poCsvFile.InputStream.CopyTo(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(stream))
                        {
                            using (var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                            {
                                csv.Configuration.RegisterClassMap<PoFileCsvViewModelMap>();
                                try
                                {
                                    poList = csv.GetRecords<PoFileCsvViewModel>().ToList();
                                }
                                catch (Exception)
                                {
                                    poList = new List<PoFileCsvViewModel>();
                                }
                            }
                        }


                        // save po details
                        int processCount = 0;
                        int totalCount = poList.Count;
                        poList = poList.Where(t => t.SelfHaulingPoNumber != "" && t.SelfHaulingPoNumber != null).ToList();
                        if (poList.Any())
                        {
                            var entityList = new List<LiftFilePONumbers>();
                            var addedByCompanyId = userContext.CompanyId;
                            var isActive = true;
                            var addedDate = DateTimeOffset.Now;

                            foreach (var poModel in poList)
                            {
                                // check if record already exists
                                var existingPo = await Context.DataContext.LiftFilePONumbers.Where(t => t.SelfHaulingPoNumber.ToLower() == poModel.SelfHaulingPoNumber.ToLower().Trim() && t.AddedByCompanyId == addedByCompanyId)
                                                                                               .OrderByDescending(t => t.AddedDate)
                                                                                               .FirstOrDefaultAsync();
                                if (existingPo != null)
                                {
                                    if (!existingPo.IsActive)
                                    {
                                        existingPo.IsActive = true;
                                        Context.DataContext.Entry(existingPo).State = EntityState.Modified;
                                        await Context.CommitAsync();

                                        processCount++;
                                    }
                                }
                                
                                else
                                {
                                    var entity = new LiftFilePONumbers();
                                    entity.SelfHaulingPoNumber = poModel.SelfHaulingPoNumber.Trim();
                                    entity.AddedByCompanyId = addedByCompanyId;
                                    entity.IsActive = isActive;
                                    entity.AddedDate = addedDate;
                                    entityList.Add(entity);

                                    processCount++;
                                }
                            }
                            if (entityList.Any())
                            {
                                Context.DataContext.LiftFilePONumbers.AddRange(entityList);
                                await Context.CommitAsync();
                            }                            
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = string.Format(Resource.successMessageLiftDataPoNumbersCSVUpload, processCount, totalCount);

                            // upload to blob
                            var azureBlob = new AzureBlobStorage();
                            var filePath = await azureBlob.SaveBlobAsync(poCsvFile.InputStream, GenerateFileName(userContext.Id), BlobContainerType.LiftDataProcessingPoFile.ToString().ToLower());
                        }

                        if (processCount == 0)
                        {
                            response.StatusMessage = Resource.errMessagePOFileEmpty;
                            response.StatusCode = Status.Failed;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("PoNumberBulkUploadDomain", "ProcessUploadedPoFile", "Lift data processing Po Number CSV upload failed", ex);
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorMessageProcessLiftDataPoNumberCSVFile;
                }
            }
            return response;
        }
    }
}
