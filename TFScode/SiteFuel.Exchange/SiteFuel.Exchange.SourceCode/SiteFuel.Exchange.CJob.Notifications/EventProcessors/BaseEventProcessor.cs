using Easy.Common;
using Easy.Common.Interfaces;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.SmsManager;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public abstract class BaseEventProcessor
    {
        protected string _serverUrl;
        protected string _emailTemplate;
        protected bool _doNotSendInvoiceAttachment;
        protected string _pdfserverUrl;
        public NotificationEventViewModel NotificationEventViewModel { get; set; }

        protected BaseEventProcessor()
        {
            var helperDomain = new HelperDomain();
            _serverUrl = helperDomain.GetServerUrl();
            _pdfserverUrl = helperDomain.GetPdfServerUrl();
            _emailTemplate = helperDomain.GetApplicationEventNotificationTemplate();
            _doNotSendInvoiceAttachment = false;
        }

        public abstract void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers);
        public abstract void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers);

        public abstract List<NotificationUserViewModel> LoadSupplierDefaultEmailReceivers(NotificationEventViewModel notificationEventViewModel);
        public abstract List<NotificationUserViewModel> LoadBuyerDefaultEmailReceivers(NotificationEventViewModel notificationEventViewModel);

        public abstract void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel);

        public void UpdateNotificationStatus(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
			var status = false;
            try
            {
                var defaultSupplierReceiver = LoadSupplierDefaultEmailReceivers(notificationEventViewModel);
                var defaultBuyerReceiver = LoadBuyerDefaultEmailReceivers(notificationEventViewModel);
                SendDefaultBuyerEmailForEvent(notificationEventViewModel, defaultBuyerReceiver);
                SendDefaultSupplierEmailForEvent(notificationEventViewModel, defaultSupplierReceiver);
				status = true;
			}
			catch (Exception ex)
            {
                LogManager.Logger.WriteException("BaseEventProcessor", "UpdateNotificationStatus", $"Exception Details Notification Id: {notificationEventViewModel.Id}, Event Type: {notificationEventViewModel.EventType} ", ex);
            }
			finally
			{
				notificationDomain.UpdatePendingNotificationEvent(notificationEventViewModel.Id, notificationEventViewModel.NotificationType, true, status);
			}
		}

        protected bool SendNotification(string email, NotificationViewModel viewModel, bool isAdmin = false)
        {
            var notificationDomain = new NotificationDomain();
            var appDomain = new ApplicationDomain();
            viewModel.NotificationId = NotificationEventViewModel.Id;

            if (viewModel.EventTypeId != null)
            {
                var emailSendingEnabled = appDomain.GetApplicationSettingValue<bool>(Constants.EmailSendingEnabled);
                var viewModelUserSubscribed = notificationDomain.IsUserSubscribedForEvent(email, viewModel.EventTypeId);
                if (viewModelUserSubscribed.IsEmailSubscribed && emailSendingEnabled)
                {
                    viewModel.To = new List<string> { email };
                    SendEmailNotification(viewModel);
                }

                if (viewModelUserSubscribed.IsSmsSubscribed && viewModelUserSubscribed.IsPhoneNumberConfirmed)
                {
                    var smsSendingCountryCode = appDomain.GetApplicationSettingValue<string>(Constants.SmsSendingCountryCode);
                    var phoneNumber = $"{smsSendingCountryCode}{viewModelUserSubscribed.ToPhoneNumber}";
                    viewModel.Subject = viewModelUserSubscribed.EventName;
                    viewModel.To = new List<string> { phoneNumber };
                    SendSmsNotification(viewModel);
                }
            }
        
            return true;
        }

        private void SendEmailNotification(NotificationViewModel viewModel)
        {
            NotificationLogDomain notificationLogDomain = new NotificationLogDomain();
            var appTemplateDetails = ContextFactory.Current.GetDomain<NotificationDomain>().GetApplicationTemplate(viewModel.ApplicationTemplateId);
            var emailModel = new ApplicationEventNotificationViewModel
            {
                To = viewModel.To,
                Bcc = viewModel.BCC,
                Subject = viewModel.Subject,
                CompanyLogo = appTemplateDetails.CompanyLogo,
                CompanyText = viewModel.CompanyText,
                BodyLogo = viewModel.BodyLogo,
                BodyText = viewModel.BodyText,
                BodyButtonText = viewModel.BodyButtonText,
                BodyButtonUrl = viewModel.BodyButtonUrl,
                ServerUrl = viewModel.ServerUrl,
                Attachments = viewModel.Attachments,
                From = appTemplateDetails.FromEmail,
                SenderName = appTemplateDetails.SenderName,
                ApplicationTemplateId = viewModel.ApplicationTemplateId
            };

            var response = Email.GetClient().Send(appTemplateDetails.Template, emailModel);
            viewModel.SendNotificationType = (int)NotificationType.Email;
            notificationLogDomain.SaveEmailNotificationLog(viewModel, response);
        }

        private void SendSmsNotification(NotificationViewModel viewModel)
        {
            var appDomain = new ApplicationDomain();
            var SmsSendingEnabled = appDomain.GetApplicationSettingValue<bool>(Constants.SmsSendingEnabled);
            NotificationLogDomain notificationLogDomain = new NotificationLogDomain();
            var smsModel = new ApplicationEventSmsNotificationViewModel
            {
                To = viewModel.To,
                SmsText = viewModel.SmsText,
            };

            bool isSmsTextLog = false;
            bool response = false;
            if (SmsSendingEnabled)
            {
                smsModel.TwilioAccountSid = appDomain.GetApplicationSettingValue<string>(Constants.TwilioAccountSid);
                smsModel.TwilioAuthToken = appDomain.GetApplicationSettingValue<string>(Constants.TwilioAuthToken);
                smsModel.TwilioFromPhoneNumber = appDomain.GetApplicationSettingValue<string>(Constants.TwilioFromPhoneNumber);
                smsModel.Url = appDomain.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingSiteFuelExchangeUrl);
                var messageSid = Sms.GetClient().Send(smsModel);
                if (messageSid != string.Empty)
                    response = true;
                viewModel.CC = new List<string> { messageSid };
                isSmsTextLog = true;
            }

            viewModel.SendNotificationType = (int)NotificationType.Sms;
            notificationLogDomain.SaveSmsNotificationLog(viewModel, response, isSmsTextLog);
        }

        protected bool SendNotification(NotificationViewModel viewModel, bool isAdmin = false)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            NotificationLogDomain notificationLogDomain = ContextFactory.Current.GetDomain<NotificationLogDomain>();
            var appDomain = new ApplicationDomain();

            viewModel.NotificationId = NotificationEventViewModel.Id;
            var emailSendingEnabled = appDomain.GetApplicationSettingValue<bool>(Constants.EmailSendingEnabled);
            var finalEmails = new List<string>();
            if (viewModel.EventTypeId != null && emailSendingEnabled)
            {
                foreach (var email in viewModel.To)
                {
                    var viewModelUserSubscribed = notificationDomain.IsUserSubscribedForEvent(email, viewModel.EventTypeId);
                    if (viewModelUserSubscribed.IsEmailSubscribed)
                    {
                        finalEmails.Add(email);
                    }
                }
            }

            if (!finalEmails.Any() && !viewModel.BCC.Any())
            {
                return true;
            }

            var appTemplateDetails = ContextFactory.Current.GetDomain<NotificationDomain>().GetApplicationTemplate(viewModel.ApplicationTemplateId);
            var emailModel = new ApplicationEventNotificationViewModel
            {
                To = finalEmails,
                Bcc = viewModel.BCC,
                Subject = viewModel.Subject,
                CompanyLogo = appTemplateDetails.CompanyLogo,
                CompanyText = viewModel.CompanyText,
                BodyLogo = viewModel.BodyLogo,
                BodyText = viewModel.BodyText,
                BodyButtonText = viewModel.BodyButtonText,
                BodyButtonUrl = viewModel.BodyButtonUrl,
                ServerUrl = viewModel.ServerUrl,
                Attachments = viewModel.Attachments,
                From = appTemplateDetails.FromEmail,
                SenderName = appTemplateDetails.SenderName,
                ApplicationTemplateId = viewModel.ApplicationTemplateId
            };

            var response = Email.GetClient().Send(appTemplateDetails.Template, emailModel);
            notificationLogDomain.SaveEmailNotificationLog(viewModel, response);
            return response;
        }

        protected bool SendNotificationForDefaultEvent(string email, NotificationViewModel viewModel)
        {
            var response = true;
            var appDomain = new ApplicationDomain();
            var notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var emailSendingEnabled = appDomain.GetApplicationSettingValue<bool>(Constants.EmailSendingEnabled);
            if (emailSendingEnabled)
            {
                NotificationLogDomain notificationLogDomain = ContextFactory.Current.GetDomain<NotificationLogDomain>();
                var appTemplateDetails = notificationDomain.GetApplicationTemplate(viewModel.ApplicationTemplateId);
                viewModel.NotificationId = NotificationEventViewModel.Id;
                viewModel.To = email.Split(';').ToList();

                var emailModel = new ApplicationEventNotificationViewModel
                {
                    To = viewModel.To,
                    Bcc = viewModel.BCC,
                    Subject = viewModel.Subject,
                    CompanyLogo = appTemplateDetails.CompanyLogo,
                    CompanyText = viewModel.CompanyText,
                    BodyLogo = viewModel.BodyLogo,
                    BodyText = viewModel.BodyText,
                    BodyButtonText = viewModel.BodyButtonText,
                    BodyButtonUrl = viewModel.BodyButtonUrl,
                    ServerUrl = viewModel.ServerUrl,
                    Attachments = viewModel.Attachments,
                    ShowHelpLineInfo = viewModel.ShowHelpLineInfo,
                    From = appTemplateDetails.FromEmail,
                    SenderName = appTemplateDetails.SenderName,
                    ApplicationTemplateId = viewModel.ApplicationTemplateId
                };
                response = Email.GetClient().Send(appTemplateDetails.Template, emailModel);
                notificationLogDomain.SaveEmailNotificationLog(viewModel, response);
            }

            if (NotificationEventViewModel.NotificationType == (int)NotificationType.Sms || NotificationEventViewModel.NotificationType == (int)NotificationType.EmailAndSms)
            {
                var viewModelUserSubscribed = notificationDomain.GetUserDetailsForSms(email);
                //if (viewModelUserSubscribed.IsPhoneNumberConfirmed) //For Old Users we don't maintain this flag.
                {
                    var smsSendingCountryCode = appDomain.GetApplicationSettingValue<string>(Constants.SmsSendingCountryCode);
                    var phoneNumber = $"{smsSendingCountryCode}{viewModelUserSubscribed.ToPhoneNumber}";
                    //viewModel.Subject = viewModelUserSubscribed.EventName; //Need to check if subject is needed for sms
                    viewModel.To = new List<string> { phoneNumber };
                    SendSmsNotification(viewModel);
                }
            }

            return response;
        }

        protected bool SendEmailCsvFile(Stream contentStream, string emailTo, string subject, string body, string fileName, int notificationId)
        {
            var response = false;
            NotificationLogDomain notificationLogDomain = ContextFactory.Current.GetDomain<NotificationLogDomain>();
            try
            {
                var appDomain = new ApplicationDomain();
                var emailSendingEnabled = appDomain.GetApplicationSettingValue<bool>(Constants.EmailSendingEnabled);
                if (emailSendingEnabled)
                {
                    HelperDomain helperDomain = new HelperDomain();
                    var serverUrl = helperDomain.GetServerUrl();
                    Attachment file = new Attachment(contentStream, fileName, Core.Utilities.MediaType.Text);

                    var attachements = new List<Attachment>() { file };
                    var emails = emailTo.Split(';').ToList();
                    var companyLogo = helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.email_HeaderLogo);

                    var emailModel = new ApplicationEventNotificationViewModel
                    {
                        To = emails,
                        Subject = subject,
                        CompanyLogo = companyLogo,
                        BodyText = body,
                        Attachments = attachements,
                        ShowFooterContent = false,
                        ShowHelpLineInfo = false
                    };

                    response = Email.GetClient().Send(_emailTemplate, emailModel);

                    var notificationModel = new NotificationViewModel
                    {
                        Subject = subject,
                        BodyText = body,
                        To = emails,
                        NotificationId = notificationId
                    };

                    notificationLogDomain.SaveEmailNotificationLog(notificationModel, response);
                }
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BaseEventProcessor", "SendEmailCsvFile", ex.Message, ex);
            }
            return response;
        }

        protected byte[] GetPdfFileContent(int invoiceId, int companyType, bool sendPDF = false)
        {
            if (sendPDF)
            {
                byte[] mybytearray = null;
                try
                {
                    if (_doNotSendInvoiceAttachment)
                        return mybytearray;

                    var url = $"{_pdfserverUrl}/InvoiceBase/GetPdfViewModelAsByetArray?id={invoiceId}&companyType={companyType}";
                    using (IRestClient client = new RestClient())
                    {

                        var response = client.GetAsync(url).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            mybytearray = JsonConvert.DeserializeObject<byte[]>(jsonString);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("BaseEventProcessor", "GetPdfFileContent", ex.Message, ex);
                }
                return mybytearray;
            }
            return null;
        }

        protected byte[] GetPdfContentForBDR(int invoiceHeaderId, int companyType)
        {
            //return null;

            byte[] mybytearray = null;
            try
            {
                if (_doNotSendInvoiceAttachment)
                    return mybytearray;

                var url = $"{_serverUrl}/InvoiceBase/GetBDRPdfViewModelAsByteArray?id={invoiceHeaderId}&companyType={companyType}";
                using (IRestClient client = new RestClient())
                {
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync().Result;
                        mybytearray = JsonConvert.DeserializeObject<byte[]>(jsonString);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BaseEventProcessor", "GetPdfFileContent", ex.Message, ex);
            }
            return mybytearray;
        }

        protected byte[] GetMarineTaxAffidavitPdfFileContent(int invoiceHeaderId)
        {
            //return null;

            byte[] mybytearray = null;
            try
            {
                if (_doNotSendInvoiceAttachment)
                    return mybytearray;

                var url = $"{_serverUrl}/InvoiceBase/GetMarineTaxAffidavitPdfViewModelAsByetArray?invoiceHeaderId={invoiceHeaderId}";
                using(IRestClient client = new RestClient())
                {
                    var response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync().Result;
                        mybytearray = JsonConvert.DeserializeObject<byte[]>(jsonString);
                    }
                }

                
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BaseEventProcessor", "GetMarineTaxAffidavitPdfFileContent", ex.Message, ex);
            }
            return mybytearray;
        }

        protected byte[] GetMarineBDNPdfFileContent(int invoiceHeaderId)
        {
            return null;

            //byte[] mybytearray = null;
            //try
            //{
            //    if (_doNotSendInvoiceAttachment)
            //        return mybytearray;

            //    var url = $"{_serverUrl}/InvoiceBase/GetMarineBDNPdfViewModelAsByetArray?invoiceHeaderId={invoiceHeaderId}";
            //    var client = new HttpClient();
            //    var response = client.GetAsync(url).Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonString = response.Content.ReadAsStringAsync().Result;
            //        mybytearray = JsonConvert.DeserializeObject<byte[]>(jsonString);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogManager.Logger.WriteException("BaseEventProcessor", "GetMarineTaxAffidavitPdfFileContent", ex.Message, ex);
            //}
            //return mybytearray;
        }

        protected byte[] GetMarineCGInspPdfFileContent(int invoiceHeaderId)
        {
            //return null;

            byte[] mybytearray = null;
            try
            {
                if (_doNotSendInvoiceAttachment)
                    return mybytearray;

                var url = $"{_serverUrl}/InvoiceBase/GetMarineCGInspPdfViewModelAsByetArray?invoiceHeaderId={invoiceHeaderId}";
                using (IRestClient client = new RestClient())
                {
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync().Result;
                        mybytearray = JsonConvert.DeserializeObject<byte[]>(jsonString);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BaseEventProcessor", "GetMarineCGInspPdfFileContent", ex.Message, ex);
            }
            return mybytearray;
        }
        protected byte[] GetMarineInspRequestVouchersPdfFileContent(int invoiceHeaderId)
        {
            //return null;

            byte[] mybytearray = null;
            try
            {
                if (_doNotSendInvoiceAttachment)
                    return mybytearray;

                var url = $"{_serverUrl}/InvoiceBase/GetMarineInspRequestVoucherViewModelsAsByetArray?invoiceHeaderId={invoiceHeaderId}";
                using (IRestClient client = new RestClient())
                {
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync().Result;
                        mybytearray = JsonConvert.DeserializeObject<byte[]>(jsonString);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BaseEventProcessor", "GetMarineInspRequestVouchersPdfFileContent", ex.Message, ex);
            }
            return mybytearray;
        }
        protected byte[] GetPdfContentForInvoiceSummary(int invoiceId)
        {
            //return null;

            byte[] mybytearray = null;
            try
            {
                if (_doNotSendInvoiceAttachment)
                    return mybytearray;

                var url = $"{_serverUrl}/InvoiceBase/GetInvoiceSummaryPdfModelAsByteArray?id={invoiceId}";
                using (IRestClient client = new RestClient())
                {
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync().Result;
                        mybytearray = JsonConvert.DeserializeObject<byte[]>(jsonString);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BaseEventProcessor", "GetPdfContentForInvoiceSummary", ex.Message, ex);
            }
            return mybytearray;
        }

        protected byte[] GetDdtPdfFileFromInvoice(int invoiceId, int companyType, bool sendPDF = false)
        {
            if (sendPDF)
            {
                byte[] mybytearray = null;
                try
                {
                    if (_doNotSendInvoiceAttachment)
                        return mybytearray;

                    var url = $"{_pdfserverUrl}/InvoiceBase/GetAndConvertInvoicePdfToDdtPdf?id={invoiceId}&companyType={companyType}";
                    using (IRestClient client = new RestClient())
                    { 
                        var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync().Result;
                        mybytearray = JsonConvert.DeserializeObject<byte[]>(jsonString);
                    }
                }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("BaseEventProcessor", "GetDdtPdfFileFromInvoice", ex.Message, ex);
                }
                return mybytearray;
            }
            return null;
        }

        protected byte[] GetStatementPdfFileContent(int statementId)
        {
            return null;

            //byte[] mybytearray = null;
            //try
            //{
            //    var url = $"{_serverUrl}/InvoiceBase/GetStatementPdfViewModelAsByetArray?statementId={statementId}";
            //    var client = new HttpClient();
            //    var response = client.GetAsync(url).Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonString = response.Content.ReadAsStringAsync().Result;
            //        mybytearray = JsonConvert.DeserializeObject<byte[]>(jsonString);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogManager.Logger.WriteException("BaseEventProcessor", "GetStatementPdfFileContent", ex.Message, ex);
            //}
            //return mybytearray;
        }

        protected List<Attachment> GetAttachments(byte[] content, string invoiceNumber)
        {
            string fileName = invoiceNumber + ".pdf";
            Attachment file = new Attachment(new System.IO.MemoryStream(content), fileName, "application/pdf");

            var attachements = new List<Attachment>();
            attachements.Add(file);

            return attachements;
        }

        protected Attachment GetPdfAttachment(MemoryStream content, string pdfName,string extension)
        {
            string fileName = pdfName + "."+ extension;
            Attachment file = new Attachment(content, fileName);

            return file;
        }

        protected Attachment GetPdfAttachment(byte[] content, string pdfName)
        {
            string fileName = pdfName + ".pdf";
            Attachment file = new Attachment(new System.IO.MemoryStream(content), fileName, "application/pdf");

            return file;
        }
        protected Attachment GetExcelAttachment(byte[] content, string excelName)
        {
            string fileName = excelName + ".xlsx";
            Attachment file = new Attachment(new System.IO.MemoryStream(content), fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            return file;
        }
        protected Attachment GetImageAttachment(byte[] content, string fileName)
        {
            fileName = fileName + ".png";
            Attachment file = new Attachment(new System.IO.MemoryStream(content), fileName);

            return file;
        }

        protected byte[] GetOrderPdfFileContent(int orderId, bool sendPDF)
        {
            if (sendPDF)
            {
                byte[] mybytearray = null;
                try
                {
                    var url = $"{_pdfserverUrl}/OrderBase/GetOrderPdfViewModelAsByetArray?orderId={orderId}";
                    using (IRestClient client = new RestClient())
                    {
                        var response = client.GetAsync(url).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            mybytearray = JsonConvert.DeserializeObject<byte[]>(jsonString);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("BaseEventProcessor", "GetOrderPdfFileContent", ex.Message, ex);
                }
                return mybytearray;
            }
            return null;
        }
       
    }
}
