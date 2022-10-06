using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.FtpReports
{
    public class TriggeredFtpReports
    {
        public TriggeredFtpReports()
        {
            //Register Context
            ContextFactory.Register(new ApplicationContext());
        }

        public async Task<bool> ProcessFtpReports()
        {
            using (var tracer = new Tracer("TriggeredFtpReports", "ProcessDailyReports"))
            {
                var response = false;
                try
                {
                    response = await SendAtlasOilReport();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("TriggeredFtpReports", "ProcessFtpReports", "Exception Details : ", ex);
                }
                return response;
            }
        }

        public async Task<bool> SendAtlasOilReport()
        {
            var response = false;
            try
            {
                var appDomain = new ApplicationDomain();
                var ftpUrl = appDomain.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingAtlasOilReportFtpUrl);
                bool isProductionEnvironment = appDomain.IsItProductionEnvironment();
                if (isProductionEnvironment && !string.IsNullOrWhiteSpace(ftpUrl))
                {
                    var strStartTime = appDomain.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingAtlasOilReportTime);
                    TimeSpan startTime;
                    if (!TimeSpan.TryParse(strStartTime, out startTime))
                    {
                        startTime = new TimeSpan(0, 0, 0);
                    }
                    var timeZone = appDomain.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingAtlasOilReportTimeZone);

                    int companyId = 559;
                    var offset = TimeZoneInfo.FindSystemTimeZoneById(timeZone).BaseUtcOffset;
                    var now = DateTimeOffset.Now.ToOffset(offset).AddDays(-1);
                    var startDate = new DateTimeOffset(now.Year, now.Month, now.Day, startTime.Hours, startTime.Minutes, startTime.Seconds, offset);
                    var endDate = startDate.AddDays(1).AddSeconds(-1);

                    var companyDomain = new CompanyDomain();

                    // AR Template
                    var csvMemoryStream = await companyDomain.GetAtlasOilReportCsvStream(companyId, startDate, endDate);
                    var fileName = string.Format("AtlasOil_{0}.csv", startDate.ToString("MM_dd_yyyy"));
                    var ftpUserName = appDomain.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingAtlasOilReportFtpUserName);
                    var ftpPassword = appDomain.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingAtlasOilReportFtpPassword);
                    var emailAddress = appDomain.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingAtlasOilReportEmail);

                    response = await UploadFileUsingFtp(ftpUrl + fileName, ftpUserName, ftpPassword, csvMemoryStream);
                    if (!response)
                    {
                        response = await SendCsvFileToAtlasOil(csvMemoryStream, emailAddress);
                    }

                    // AP Template
                    csvMemoryStream = await companyDomain.GetAtlasOilCarrierReportCsvStream(companyId, startDate, endDate);
                    fileName = string.Format("AtlasOil_Carrier_{0}.csv", startDate.ToString("MM_dd_yyyy"));

                    response = await UploadFileUsingFtp(ftpUrl + fileName, ftpUserName, ftpPassword, csvMemoryStream);
                    if (!response)
                    {
                        response = await SendCsvFileToAtlasOil(csvMemoryStream, emailAddress);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TriggeredFtpReports", "SendAtlasOilReport", "Exception Details : ", ex);
            }
            return response;
        }

        public async Task<bool> SendCsvFileToAtlasOil(Stream stream, string toEmail)
        {
            var response = false;
            try
            {
                HelperDomain helperDomain = new HelperDomain();
                var serverUrl = helperDomain.GetServerUrl();
                var emailTemplate = helperDomain.GetApplicationEventNotificationTemplate();
                string fileName = string.Format("AtlasOil_{0}.csv", DateTime.Now.ToString("MM_dd_yyyy"));
                System.Net.Mail.Attachment file = new System.Net.Mail.Attachment(stream, fileName, Core.Utilities.MediaType.Text);

                var attachements = new List<System.Net.Mail.Attachment>() { file };
                var customerEmail = toEmail;
                var emails = customerEmail.Split(';').ToList(); // no need now
                var companyLogo = helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.email_HeaderLogo);
                var subjectLine = Resource.emailAtlasOil_SubjectText;
                var bodyText = Resource.emailAtlasOil_BodyText;

                var emailModel = new ApplicationEventNotificationViewModel
                {
                    To = emails,
                    Subject = subjectLine,
                    CompanyLogo = companyLogo,
                    BodyText = bodyText,
                    Attachments = attachements,
                    ShowFooterContent = false,
                    ShowHelpLineInfo = false
                };

                response = await ContextFactory.Current.GetDomain<EmailDomain>().SendEmail(emailTemplate, emailModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "SendCsvFileToAtlasOil", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> UploadFileUsingFtp(string ftpUrl, string ftpUsername, string ftpPassword, Stream fileStream)
        {
            var response = false;
            try
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                byte[] fileContents;
                using (StreamReader sourceStream = new StreamReader(fileStream))
                {
                    fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                }
                request.ContentLength = fileContents.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    await requestStream.WriteAsync(fileContents, 0, fileContents.Length);
                }
                using (FtpWebResponse ftpResponse = (FtpWebResponse)request.GetResponse())
                {
                    var uploadStatus = $"Atlas Oil File Upload Completed, Status: {ftpResponse.StatusDescription}";
                    LogManager.Logger.WriteException("TriggeredFtpReports", "UploadFileUsingFtp", uploadStatus, null);
                }
                response = true;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TriggeredFtpReports", "UploadFileUsingFtp", "Exception Details : ", ex);
            }
            return response;
        }
    }
}
