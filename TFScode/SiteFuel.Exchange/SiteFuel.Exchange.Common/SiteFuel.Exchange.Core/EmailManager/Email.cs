using RazorEngine;
using RazorEngine.Templating;
using SendGrid;
using SendGrid.Helpers.Mail;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.EmailManager
{
    public class Email
    {
        public Email(SmtpClient smtpClient = null)
        {
        }

        public static Email GetClient()
        {
            return new Email();
        }

        public bool Send(string emailTemplate, ApplicationEventNotificationViewModel model)
        {
            bool response = false;
            try
            {
                string emailHtmlBody = Engine.Razor.RunCompile(emailTemplate, "ApplicationEventNotification" + model.ApplicationTemplateId, null, model);
                response = Send(model.From, model.To.Where(x => !string.IsNullOrEmpty(x)).ToArray(), model.Subject, emailHtmlBody, model.Cc.ToArray(), model.Bcc.ToArray(), model.ReplyTo.ToArray(), model.Attachments, model.SenderName);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("Email", "Send", "Exception Details : ", ex);
            }
            return response;
        }

        public bool Send(string emailTemplate, WindowsServiceStatusNotificationViewModel model)
        {
            bool response = false;
            try
            {
                string emailHtmlBody = Engine.Razor.RunCompile(emailTemplate, "WindowsServiceStatusNotification", null, model);
                response = Send(model.From, model.To.ToArray(), model.Subject, emailHtmlBody, model.Cc.ToArray(), model.ReplyTo.ToArray(), model.Bcc.ToArray());
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("Email", "Send", "Exception Details : ", ex);
            }
            return response;
        }


        public Task<bool> SendAsync(string emailTemplate, ApplicationEventNotificationViewModel model)
        {
            return Task.FromResult(Send(emailTemplate, model));
        }

        public Task<bool> SendAsync(string emailTemplate, WindowsServiceStatusNotificationViewModel model)
        {
            return Task.FromResult(Send(emailTemplate, model));
        }

        public string GetHtml<T>(string emailTemplate, T model)
        {
            string response = string.Empty;
            try
            {
                response = Engine.Razor.RunCompile(emailTemplate, "ApplicationNotification", null, model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("Email", "GetHtml", "Exception Details : ", ex);
            }
            return response;
        }
        public string GetCarrierHtml<T>(string emailTemplate, T model,string templateName)
        {
            string response = string.Empty;
            try
            {
                response = Engine.Razor.RunCompile(emailTemplate, templateName, null, model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("Email", "GetHtml", "Exception Details : ", ex);
            }
            return response;
        }
        private MailMessage ConfigureEmail(MailAddress fromMail, MailAddress[] toMails, string subject, string body, MailAddress[] ccMails, MailAddress[] bccMails, MailAddress[] replyToMail, List<System.Net.Mail.Attachment> attachments)
        {
            var email = new MailMessage()
            {
                Body = body,
                IsBodyHtml = true,
                Subject = subject
            };

            email.BodyEncoding = System.Text.Encoding.UTF8;
            email.SubjectEncoding = System.Text.Encoding.UTF8;

            foreach (var attachment in attachments ?? Enumerable.Empty<System.Net.Mail.Attachment>())
            {
                email.Attachments.Add(attachment);
            }

            foreach (var toMail in toMails ?? Enumerable.Empty<MailAddress>())
            {
                email.To.Add(toMail);
            }

            foreach (var ccMail in ccMails ?? Enumerable.Empty<MailAddress>())
            {
                email.CC.Add(ccMail);
            }

            foreach (var bccMail in bccMails ?? Enumerable.Empty<MailAddress>())
            {
                email.Bcc.Add(bccMail);
            }

            foreach (var replyMail in replyToMail ?? Enumerable.Empty<MailAddress>())
            {
                email.Bcc.Add(replyMail);
            }

            if (fromMail != null)
            {
                email.From = new MailAddress(fromMail.Address, fromMail.DisplayName);
            }

            return email;
        }

        public bool Send(string fromMailAddress, string[] toMailAddresses, string subject, string body, string[] ccMailAddresses = null, string[] bccMailAddresses = null, string[] replyTo = null, List<System.Net.Mail.Attachment> attachments = null, string senderName = "")
        {
            var response = false;
            try
            {
                var sendGridSender = SiteFuel.Exchange.Core.Utilities.AppSettings.SendGridSender; // Default [0 = SMTP]
                if (sendGridSender == 0)//0 = SMTP, 1 = API
                {
                    var mailMessage = GetSmtpMailMessage(fromMailAddress, toMailAddresses, subject, body, ccMailAddresses, bccMailAddresses, replyTo, attachments, senderName);
                    if (mailMessage != null)
                    {
                        response = SendUsingSmtp(mailMessage);
                    }
                }
                else
                {
                    var mailMessage = GetSendGridMailMessage(fromMailAddress, toMailAddresses, subject, body, ccMailAddresses, bccMailAddresses, replyTo, attachments);
                    if (mailMessage != null)
                    {
                        response = SendUsingApi(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SiteFuel.Exchange.EmailManager", "Send", ex.Message, ex);
            }

            return response;
        }

        private bool SendUsingSmtp(MailMessage mailMessage)
        {
            var response = false;
            try
            {
                var host = SiteFuel.Exchange.Core.Utilities.AppSettings.Host;
                var port = SiteFuel.Exchange.Core.Utilities.AppSettings.Port;
                var smtpServerUserName = SiteFuel.Exchange.Core.Utilities.AppSettings.SmtpServerUserName;
                var smtpServerPassword = SiteFuel.Exchange.Core.Utilities.AppSettings.SendGridApiKey;

                if (!int.TryParse(port, out int sendGridPort))
                    sendGridPort = 587;

                var client = new SmtpClient(host, sendGridPort);
                client.Credentials = new NetworkCredential(smtpServerUserName, smtpServerPassword);
                client.Send(mailMessage);
                response = true;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SiteFuel.Exchange.EmailManager", "SendUsingSmtp", ex.Message, ex);
            }

            return response;
        }

        private bool SendUsingApi(SendGridMessage mailMessage)
        {
            var response = false;
            try
            {
                var apiKey = SiteFuel.Exchange.Core.Utilities.AppSettings.SendGridApiKey;
                var client = new SendGridClient(apiKey);
                var result = Task.Run(() => client.SendEmailAsync(mailMessage)).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    response = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SiteFuel.Exchange.EmailManager", "SendUsingApi", ex.Message, ex);
            }

            return response;
        }

        private MailMessage GetSmtpMailMessage(string fromMailAddress, string[] toMailAddresses, string subject, string body, string[] ccMailAddresses = null, string[] bccMailAddresses = null, string[] replyTo = null, List<System.Net.Mail.Attachment> attachments = null, string senderName = "")
        {
            MailMessage response = null;
            try
            {
                if (string.IsNullOrWhiteSpace(senderName))
                {
                    senderName = SiteFuel.Exchange.Core.Utilities.AppSettings.EmailDisplayName;
                }
                if (string.IsNullOrWhiteSpace(fromMailAddress))
                {
                    fromMailAddress = SiteFuel.Exchange.Core.Utilities.AppSettings.EmailFromAddress;
                }

                var from = string.IsNullOrWhiteSpace(fromMailAddress) ? null : new MailAddress(fromMailAddress, senderName);
                var to = toMailAddresses.Select(item => new MailAddress(item)).ToArray();
                var cc = ccMailAddresses?.Select(item => new MailAddress(item)).ToArray();
                var bcc = bccMailAddresses?.Select(item => new MailAddress(item)).ToArray();
                var replyto = replyTo?.Select(item => new MailAddress(item)).ToArray();
                response = ConfigureEmail(from, to, subject, body, cc, bcc, replyto, attachments);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SiteFuel.Exchange.EmailManager", "GetSmtpMailMessage", ex.Message, ex);
            }

            return response;
        }

        private SendGridMessage GetSendGridMailMessage(string fromMailAddress, string[] toMailAddresses, string subject, string body, string[] ccMailAddresses = null, string[] bccMailAddresses = null, string[] replyTo = null, List<System.Net.Mail.Attachment> attachments = null)
        {
            SendGridMessage response = null;
            try
            {
                var name = SiteFuel.Exchange.Core.Utilities.AppSettings.EmailDisplayName;
                if (string.IsNullOrWhiteSpace(fromMailAddress))
                {
                    fromMailAddress = SiteFuel.Exchange.Core.Utilities.AppSettings.EmailFromAddress;
                }

                var from = string.IsNullOrWhiteSpace(fromMailAddress) ? null : new EmailAddress(fromMailAddress, name);
                var to = toMailAddresses.Select(item => new EmailAddress(item)).ToList();
                var replyto = replyTo?.Select(item => new EmailAddress(item)).ToArray();

                response = MailHelper.CreateSingleEmailToMultipleRecipients(from, to, subject, null, body);
                response.ReplyTo = replyto.Count() > 0 ? replyto.First() : null;
                if (attachments != null)
                {
                    response.Attachments = attachments.Select(t => GetSendGridAttachment(t)).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SiteFuel.Exchange.EmailManager", "GetSendGridMailMessage", ex.Message, ex);
            }

            return response;
        }

        private SendGrid.Helpers.Mail.Attachment GetSendGridAttachment(System.Net.Mail.Attachment attachment)
        {
            using (var stream = new MemoryStream())
            {
                attachment.ContentStream.CopyTo(stream);
                return new SendGrid.Helpers.Mail.Attachment()
                {
                    Disposition = attachment.ContentDisposition.DispositionType,
                    Type = attachment.ContentType.MediaType,
                    Filename = attachment.Name,
                    ContentId = attachment.ContentId,
                    Content = Convert.ToBase64String(stream.ToArray())
                };
            }
        }
    }
}

