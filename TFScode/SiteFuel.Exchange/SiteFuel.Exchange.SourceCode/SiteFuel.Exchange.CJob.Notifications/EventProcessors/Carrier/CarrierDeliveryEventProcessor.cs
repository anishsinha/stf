using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Notification;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class CarrierDeliveryEventProcessor : BaseDeliveryScheduleEventProcessor, IEmailProcessor
    {
        private CarrierDeliveryNotificationViewModel carrierDeliveryViewModel;
        public EventType EventType => EventType.CarrierDeliveries;
        private List<LfRecordsCarrierReportsViewModel> lfvDashboardRecords = new List<LfRecordsCarrierReportsViewModel>();
        private List<TerminalMappingDetails> terminalMappingDetails = new List<TerminalMappingDetails>();
        private List<CarrierMappingDetails> carrierMappingDetails = new List<CarrierMappingDetails>();
        public CarrierDeliveryEventProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            carrierDeliveryViewModel = notificationDomain.GetCarrierDeliveryDetails(3000);
            lfvDashboardRecords = Task.Run(() => notificationDomain.GetLFVDashboardData()).Result;
            if (lfvDashboardRecords.Any())
            {
                //if any data in ForcedIgnoreReason replace with IgnoredReason with string.Empty
                lfvDashboardRecords.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x.ForcedIgnoreReason) && !x.ForcedIgnoreReason.Contains("--"))
                    {
                        x.IgnoredReason = string.Empty;
                    }
                });
                terminalMappingDetails = notificationDomain.GetTerminalMappingDetails(lfvDashboardRecords.Select(x => x.Terminal).Distinct().ToList());
                carrierMappingDetails = notificationDomain.GetCarrierMappingDetails(lfvDashboardRecords.Select(x => x.Terminal).Distinct().ToList(), lfvDashboardRecords.Select(x => x.CarrierID).Distinct().ToList());
            }
        }
        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            carrierDeliveryViewModel.DefaultSupplierEmailRecievers = defaultRecievers;
        }
        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            carrierDeliveryViewModel.DefaultBuyerEmailRecievers = defaultRecievers;
        }

        public void ProcessCarrierDeliveryDetails(NotificationEventViewModel notificationEvent, CarrierDeliveryNotificationViewModel viewModel)
        {
            try
            {
                NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
                if (carrierDeliveryViewModel != null)
                {
                    foreach (var item in carrierDeliveryViewModel.CarrierDeliveryViewModel)
                    {
                        if (item.CarrierDelXUserViewModel.Any())
                        {
                            item.UoM = item.CarrierDelXUserViewModel.FirstOrDefault().UoM;
                        }
                        var notification = notificationDomain.GetNotificationContent(EventSubType.CarrierDeliveries, _serverUrl, string.Empty, (int)notificationEvent.EventType);
                        
                        var emailClient = Email.GetClient();
                        var mailBody = emailClient.GetCarrierHtml<CarrierDeliveryViewModel>(notification.BodyText, item, "CarrierEmailDelivery");
                        notification.BodyText = mailBody;
                        notification.Subject = string.Format(notification.Subject, item.SupplierCompanyName, item.ReportGenerateDate);
                        List<DeliveryUploadFailure> deliveryUploadFailure = new List<DeliveryUploadFailure>();
                        try
                        {
                            //remove pdf attchement as per discussion with Mridul
                            //var pdfAttachment = emailClient.GetCarrierHtml<CarrierDeliveryViewModel>(Resource.carrierdeliveriesPdfAttachment, item, "CarrierEmailPdfDelivery");
                            //byte[] pdfAttachement = GetEmailByteArray(pdfAttachment);


                            //if (pdfAttachement != null && pdfAttachement.Length > 0)
                            //{
                            //    notification.Attachments = GetAttachments(pdfAttachement, string.Format(notification.Subject, item.SupplierCompanyName, item.ReportGenerateDate));
                            //}

                            foreach (var carrierDelXUserInfo in item.CarrierDelXUserViewModel)
                            {
                                if (carrierDelXUserInfo.DeliveryUploadFailure.Any())
                                {
                                    deliveryUploadFailure.AddRange(carrierDelXUserInfo.DeliveryUploadFailure);
                                }
                            }
                            GetAPIExceptionExcelData(item, notification, deliveryUploadFailure);
                            GetLFVDashboardExcelData(item, notification);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        //send email if records found.
                        if (deliveryUploadFailure.Any() || lfvDashboardRecords.Where(x => x.CompanyId == item.SupplierCompanyId).Any())
                        {
                            SendNotificationForDefaultEvent(string.Join(";", item.EmailAddress), notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDeliveryEventProcessor", "ProcessCarrierDeliveryDetails", "Exception Details : ", ex);
            }
        }

        private void GetLFVDashboardExcelData(CarrierDeliveryViewModel item, NotificationViewModel notification)
        {
            if (lfvDashboardRecords.Any())
            {

                if (lfvDashboardRecords.Where(x => x.CompanyId == item.SupplierCompanyId).Any())
                {
                    var emailList = lfvDashboardRecords.Where(x => x.CompanyId == item.SupplierCompanyId).FirstOrDefault().EmailList;
                    if (emailList.Any())
                    {
                        item.EmailAddress.AddRange(emailList);
                        item.EmailAddress = item.EmailAddress.Distinct().ToList();
                    }
                    var companylfvDashboardRecords = lfvDashboardRecords.Where(x => x.CompanyId == item.SupplierCompanyId).ToList();
                    var finalLFVReportData = (from lfvDashboardItem in companylfvDashboardRecords
                                              select new
                                              {
                                                  BOL = lfvDashboardItem.Bol,
                                                  Terminal = lfvDashboardItem.Terminal,
                                                  TerminalORBulkPlant = terminalMappingDetails.FirstOrDefault(x => x.TerminalId == lfvDashboardItem.Terminal) != null ? terminalMappingDetails.FirstOrDefault(x => x.TerminalId == lfvDashboardItem.Terminal).TerminalORBulkPlanName : string.Empty,
                                                  CorrectedQuantity = lfvDashboardItem.CorrectedQuantity,
                                                  TerminalItemCode = lfvDashboardItem.TerminalItemCode,
                                                  ProductType = lfvDashboardItem.ProductType,
                                                  LoadDate = lfvDashboardItem.LoadDate,
                                                  RecordDate = lfvDashboardItem.RecordDate,
                                                  CarrierID = lfvDashboardItem.CarrierID,
                                                  CarrierName = carrierMappingDetails.FirstOrDefault(x => x.TerminalId == lfvDashboardItem.Terminal && x.CarrierId == lfvDashboardItem.CarrierID) != null ? carrierMappingDetails.FirstOrDefault(x => x.TerminalId == lfvDashboardItem.Terminal && x.CarrierId == lfvDashboardItem.CarrierID).CarrierName : !string.IsNullOrEmpty(lfvDashboardItem.CarrierName) ? lfvDashboardItem.CarrierName : string.Empty,
                                                  IgnoredReason = lfvDashboardItem.IgnoredReason,
                                                  ForcedIgnoreReason = lfvDashboardItem.ForcedIgnoreReason,
                                                  ReasonCode = lfvDashboardItem.ReasonCode,
                                                  ReasonCategory = lfvDashboardItem.ReasonCategory,
                                                  Username = lfvDashboardItem.Username,
                                                  ModifiedDate = lfvDashboardItem.LFVCarrierPerModifiedDate,
                                                  StatusChangeDate = lfvDashboardItem.statusChangeDate
                                              }).ToList();
                    DataTable dtLFVReportData = ConvertToDataTable(finalLFVReportData);
                    byte[] apiLFVReportData = GenerateExcel(dtLFVReportData, "LFV Dashboard");
                    if (apiLFVReportData != null && apiLFVReportData.Length > 0)
                    {
                        if (notification.Attachments == null)
                        {
                            notification.Attachments = new List<System.Net.Mail.Attachment>();
                            notification.Attachments.Add(GetExcelAttachment(apiLFVReportData, "LFV Dashboard"));
                        }
                        else
                        {
                            notification.Attachments.Add(GetExcelAttachment(apiLFVReportData, "LFV Dashboard"));
                        }

                    }
                }
            }
        }

        private void GetAPIExceptionExcelData(CarrierDeliveryViewModel item, NotificationViewModel notification, List<DeliveryUploadFailure> deliveryUploadFailure)
        {
            if (deliveryUploadFailure.Any())
            {
                var finalReportData = (from delFailureItem in deliveryUploadFailure
                                       select new
                                       {
                                           CarrierName = delFailureItem.Carriername,
                                           APIName = delFailureItem.APIName,
                                           ExternalRefID = delFailureItem.ExternalRefID,
                                           DateTime = delFailureItem.DateTime,
                                           Status = delFailureItem.Status,
                                           LocationID = delFailureItem.LocationID,
                                           BOL = delFailureItem.BOL,
                                           DropDate = delFailureItem.DropDate,
                                           LiftDate = delFailureItem.LiftDate,
                                           Request = delFailureItem.Request,
                                           Response = delFailureItem.Response
                                       }).ToList();
                DataTable dt = ConvertToDataTable(finalReportData);
                byte[] apiExceptionCSVData = GenerateExcel(dt, "API Exception");
                if (apiExceptionCSVData != null && apiExceptionCSVData.Length > 0)
                {
                    if (notification.Attachments == null)
                    {
                        notification.Attachments = new List<System.Net.Mail.Attachment>();
                        notification.Attachments.Add(GetExcelAttachment(apiExceptionCSVData, "API Exception"));
                    }
                    else
                    {
                        notification.Attachments.Add(GetExcelAttachment(apiExceptionCSVData, "API Exception"));
                    }

                }


            }
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                ProcessCarrierDeliveryDetails(notificationEventViewModel, carrierDeliveryViewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDeliveryEventProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        public byte[] GetEmailByteArray(string html)
        {
            StringReader sr = new StringReader(html);
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                return bytes;
            }
        }
        public byte[] GenerateExcel(DataTable DT, string fullFileName)
        {
            var file = new FileInfo(fullFileName);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage(file);
            excel.Workbook.Properties.Title = "Attempts";
            var sheetcreate = excel.Workbook.Worksheets.Add(fullFileName);

            int col = 0;
            foreach (DataColumn column in DT.Columns)  //printing column headings
            {
                sheetcreate.Cells[1, ++col].Value = GetHeaderDisplayName(column.ColumnName);
                sheetcreate.Cells[1, col].Style.Font.Bold = true;
                sheetcreate.Cells[1, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheetcreate.Cells[1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            if (DT.Rows.Count > 0)
            {
                int row = 1;
                decimal checkDecimal;
                for (int eachRow = 0; eachRow < DT.Rows.Count;)    //looping each row
                {

                    for (int eachColumn = 1; eachColumn <= col; eachColumn++)   //looping each column in a row
                    {
                        var eachRowObject = sheetcreate.Cells[row + 1, eachColumn];
                        eachRowObject.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        eachRowObject.Value = DT.Rows[eachRow][(eachColumn - 1)].ToString();

                        if (decimal.TryParse(DT.Rows[eachRow][(eachColumn - 1)].ToString(), out checkDecimal))      //verifying value is number to make it right align
                        {
                            eachRowObject.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        eachRowObject.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);     // adding border to each cells

                        if (eachRow % 2 == 0)       //alternatively adding color to each cell.
                            eachRowObject.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#e0e0e0"));
                        else
                            eachRowObject.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                    }
                    eachRow++;
                    row++;

                }
            }
            sheetcreate.Cells.AutoFitColumns();
            return excel.GetAsByteArray();
        }
        public DataTable ConvertToDataTable<T>(IList<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;


        }
        public string GetHeaderDisplayName(string headerName)
        {
            if (headerName == "TerminalORBulkPlant")
            {
                return "Terminal/BulkPlant Name";
            }
            else if (headerName == "StatusChangeDate")
            {
                return "Last Date Updated(MST)";
            }
            else if (headerName == "IgnoredReason")
            {
                return "Ignored Reason";
            }
            else if (headerName == "ForcedIgnoreReason")
            {
                return "Forced Ignore Reason";
            }
            else if (headerName == "ReasonCode")
            {
                return "Reason Code";
            }
            else if (headerName == "ReasonCategory")
            {
                return "Reason Category";
            }
            else if (headerName == "ModifiedDate")
            {
                return "Modified Date(MST)";
            }
            return headerName;
        }
    }

}

