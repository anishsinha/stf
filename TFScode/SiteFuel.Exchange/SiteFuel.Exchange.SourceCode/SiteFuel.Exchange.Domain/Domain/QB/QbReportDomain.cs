using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Quickbooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class QbReportDomain : BaseDomain
    {
        public QbReportDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public QbReportDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<QbSyncReportViewModel> GetQuickBooksSyncReportViewModel(int companyId, DateTimeOffset reportDate)
        {
            var response = new QbSyncReportViewModel();
            try
            {
                var qbProfileInfo = Context.DataContext.QbProfiles.Where(t => t.CompanyId == companyId)
                    .Select(t => new { t.SyncReportTime, t.ReportTimeZone }).FirstOrDefault();
                if (qbProfileInfo != null)
                {
                    var timeZone = qbProfileInfo.ReportTimeZone;
                    var offset = reportDate.GetOffset(timeZone);
                    var startTime = TimeSpan.Parse(qbProfileInfo.SyncReportTime);

                    DateTimeOffset endDate = reportDate, startDate = reportDate.AddDays(-1);
                    startDate = new DateTimeOffset(startDate.Year, startDate.Month, startDate.Day, startTime.Hours, startTime.Minutes, 0, 0, offset);
                    endDate = new DateTimeOffset(endDate.Year, endDate.Month, endDate.Day, startTime.Hours, startTime.Minutes, 0, 0, offset);

                    response.Report = await GetQuickBooksSyncReportAsync(companyId, startDate, endDate, timeZone);
                    response.ReportDate = endDate.ToString(Resource.constFormatQbSyncDate);
                    response.SyncStartDate = startDate.ToString(Resource.constFormatQbSyncDateTime);
                    response.SyncEndDate = endDate.ToString(Resource.constFormatQbSyncDateTime);
                    response.TimeZone = string.Join("", timeZone.Split(' ').Select(t => t.First()));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbReportDomain", "GetQuickBooksSyncReportViewModel", ex.Message, ex);
            }
            return response;
        }

        public async Task<QbSyncReport> GetQuickBooksSyncReportAsync(int companyId, DateTimeOffset startDate, DateTimeOffset endDate, string timeZone)
        {
            var response = new QbSyncReport();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var reportData = await spDomain.GetQbSyncedReportData(companyId, startDate, endDate, timeZone);
                response = ProcessQbReportDataAndGenerateRoport(reportData);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbReportDomain", "GetQuickBooksSyncReport", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SendQbSyncReportEmailAsync(QbSyncReportViewModel reportViewModel, List<string> toEmails)
        {
            var response = new StatusViewModel();
            try
            {
                var helperDomain = new HelperDomain(this);
                var serverUrl = helperDomain.GetServerUrl();

                var callbackUrl = "~/Supplier/QuickBooks";
                var notificationDomain = new NotificationDomain(this);
                var notification = notificationDomain.GetNotificationContent(EventSubType.QuickBooksSyncReport, serverUrl, callbackUrl);

                var emailTemplate = helperDomain.GetApplicationEventNotificationTemplate();
                reportViewModel.ReportImageUrl = helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.email_QbSyncReport_Logo);

                var emailClient = Email.GetClient();
                var mailBody = emailClient.GetHtml(notification.BodyText, reportViewModel);

                var emailModel = new ApplicationEventNotificationViewModel
                {
                    To = toEmails,
                    Subject = notification.Subject,
                    //CompanyLogo = notification.CompanyLogo,
                    BodyText = mailBody,
                    ShowHelpLineInfo = true,
                    BodyButtonText = notification.BodyButtonText,
                    BodyButtonUrl = notification.BodyButtonUrl,
                    BodyMaxWidth = "1000px"
                };
                var emailDomain = new EmailDomain(this);
                var isEmailSent = await emailDomain.SendEmail(emailTemplate, emailModel);
                if (isEmailSent)
                {
                    response.StatusCode = Status.Success;
                    response.StatusMessage = $"QuickBooks sync report has been sent to {string.Join(", ", toEmails)} email address.";
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetProgressReport", ex.Message, ex);
            }

            return response;
        }

        private QbSyncReport ProcessQbReportDataAndGenerateRoport(List<UspQbSyncedReportData> reportData)
        {
            var response = new QbSyncReport();
            try
            {
                response.QbSalesOders = GetOrderReport(reportData, QbEntityType.SalesOrder, QbXmlType.SalesOrderAdd, QbXmlType.SalesOrderMod);
                response.QbPurchaseOrders = GetOrderReport(reportData, QbEntityType.PurchaseOrder, QbXmlType.PurchaseOrderAdd, QbXmlType.PurchaseOrderMod);
                response.QbInvoices = GetInvoiceReport(reportData, QbEntityType.Invoice, QbXmlType.InvoiceAdd, QbXmlType.InvoiceMod);
                response.QbBills = GetInvoiceReport(reportData, QbEntityType.Bill, QbXmlType.BillAdd, QbXmlType.BillMod);
                response.QbCreditMemos = GetInvoiceReport(reportData, QbEntityType.CreditMemo, QbXmlType.CreditMemoAdd, QbXmlType.Unknown);
                response.QbVendorCredits = GetInvoiceReport(reportData, QbEntityType.VendorCredit, QbXmlType.VendorCreditAdd, QbXmlType.Unknown);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbReportDomain", "ProcessQbReportDataAndGenerateRoport", ex.Message, ex);
            }
            return response;
        }

        private static QbReport GetOrderReport(List<UspQbSyncedReportData> reportData, QbEntityType entityType, QbXmlType qbXmlAdd, QbXmlType qbXmlMod)
        {
            var qbOrderReport = new QbReport();
            try
            {
                var orders = reportData.Where(t => t.EntityType == entityType.ToString());
                var orderSynced = orders.Where(t => t.Status == (int)QbRequestStatus.Completed);
                var orderNotSynced = orders.Except(orderSynced);

                //Order created/modified
                qbOrderReport.Created = orderSynced.Where(t => t.QbXmlType == (int)qbXmlAdd).Select(t => t.PoNumber)
                                        .Distinct().OrderBy(t => t).ToList();
                qbOrderReport.Modified = orderSynced.Where(t => t.QbXmlType == (int)qbXmlMod).Select(t => t.PoNumber)
                                        .Distinct().OrderBy(t => t).ToList();

                //Order skipped/failed
                qbOrderReport.Skipped = orderNotSynced.Where(t => t.QbXmlType == (int)qbXmlMod
                                        && t.Status == (int)QbRequestStatus.SyncSkip).Select(t => t.PoNumber)
                                        .Distinct().OrderBy(t => t).ToList();

                qbOrderReport.Failed = orderNotSynced.Where(t => (t.QbXmlType == (int)qbXmlAdd || t.QbXmlType == (int)qbXmlMod)
                                                            && t.Status == (int)QbRequestStatus.Failed).Select(t => t.PoNumber)
                                                            .Distinct().OrderBy(t => t).ToList();

                qbOrderReport.TotalSynced = qbOrderReport.Created.Count + qbOrderReport.Modified.Count;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbReportDomain", "GetOrderReport", ex.Message, ex);
            }
            return qbOrderReport;
        }

        private static QbReport GetInvoiceReport(List<UspQbSyncedReportData> reportData, QbEntityType entityType, QbXmlType qbXmlAdd, QbXmlType qbXmlMod)
        {
            var qbInvoiceReport = new QbReport();
            try
            {
                var invoices = reportData.Where(t => t.EntityType == entityType.ToString());
                var invoiceSynced = invoices.Where(t => t.Status == (int)QbRequestStatus.Completed);
                var invoiceNotSynced = invoices.Except(invoiceSynced);

                //Invoice created/modified
                qbInvoiceReport.Created = invoiceSynced.Where(t => t.QbXmlType == (int)qbXmlAdd)
                                            .Select(t => FormatInvoiceNumber(t.InvoiceNumber, t.QbInvoiceNumber))
                                            .Distinct().OrderBy(t => t).ToList();

                if (qbXmlMod != QbXmlType.Unknown)
                {
                    qbInvoiceReport.Modified = invoiceSynced.Where(t => t.QbXmlType == (int)qbXmlMod)
                                                .Select(t => FormatInvoiceNumber(t.InvoiceNumber, t.QbInvoiceNumber))
                                                .Distinct().OrderBy(t => t).ToList();

                    //Invoice skipped/failed
                    qbInvoiceReport.Skipped = invoiceNotSynced.Where(t => t.QbXmlType == (int)qbXmlMod
                                            && t.Status == (int)QbRequestStatus.SyncSkip).Select(t => t.InvoiceNumber)
                                            .Distinct().OrderBy(t => t).ToList();
                }

                qbInvoiceReport.Failed = invoiceNotSynced.Where(t => (t.QbXmlType == (int)qbXmlAdd || (qbXmlMod != QbXmlType.Unknown && t.QbXmlType == (int)qbXmlMod))
                                                            && t.Status == (int)QbRequestStatus.Failed).Select(t => t.InvoiceNumber)
                                                            .Distinct().OrderBy(t => t).ToList();

                qbInvoiceReport.TotalSynced = qbInvoiceReport.Created.Count + qbInvoiceReport.Modified.Count;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbReportDomain", "GetInvoiceReport", ex.Message, ex);
            }
            return qbInvoiceReport;
        }

        private static string FormatInvoiceNumber(string value, string valueToAdd)
        {
            var response = value;
            try
            {
                if (!string.IsNullOrWhiteSpace(valueToAdd))
                {
                    response = $"{value} (QB#: {valueToAdd})";
                }
            }
            catch
            {
                response = value;
            }
            return response;
        }
    }
}
