using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class InvoicePdfEmailProcessor : BaseEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.PdfEmailAttachment;
        EmailDocumentViewModel pdfEmailModel;
        List<int> invoiceIds;
        int invoiceAttachmentsPerEmail = 5;
        int loopCount = 0;

        public InvoicePdfEmailProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                pdfEmailModel = new EmailDocumentViewModel();
                var masterDomain = ContextFactory.Current.GetDomain<MasterDomain>();
                int maxInvoicesEmailToSent;

                int.TryParse(masterDomain.GetApplicationSettingValue(Constants.MaxInvoiceAttachmentsPerEmailKey), out invoiceAttachmentsPerEmail);
                int.TryParse(masterDomain.GetApplicationSettingValue(Constants.MaxInvoiceCountPerSessionKey), out maxInvoicesEmailToSent);

                pdfEmailModel = JsonConvert.DeserializeObject<EmailDocumentViewModel>(notificationEventViewModel.JsonMessage);

                if (pdfEmailModel.InvoiceIds != null && pdfEmailModel.InvoiceIds.Length > 0)
                {
                    invoiceIds = new List<int>();
                    invoiceIds = pdfEmailModel.InvoiceIds.Split(',').Select(int.Parse).ToList();

                    if (invoiceIds.Count > maxInvoicesEmailToSent)
                    {
                        invoiceIds = invoiceIds.Take(maxInvoicesEmailToSent).ToList();
                        loopCount = maxInvoicesEmailToSent / invoiceAttachmentsPerEmail;
                    }
                    else
                    {
                        loopCount = (Int32)Math.Ceiling((decimal)invoiceIds.Count / invoiceAttachmentsPerEmail);
                    }

                    loopCount = loopCount <= 0 ? 0 : loopCount;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "Initialize", ex.Message, ex);
            }
        }

        private void GetPdfAttachmentsForInvoice(int invoiceId, string invoiceNumber, NotificationViewModel notification, bool isManualTrigger)
        {
            try
            {
                var pdfContent = GetPdfFileContent(invoiceId, (int)pdfEmailModel.CompanyType, isManualTrigger);
                if (pdfContent != null)
                {
                    notification.Attachments.Add(GetPdfAttachment(pdfContent, invoiceNumber));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "GetPdfAttachments", ex.Message, ex);
            }
        }

        private void GetPdfForBDR(int invoiceHeaderId, string invoiceNumber, NotificationViewModel notification)
        {
            try
            {
                var pdfContent = GetPdfContentForBDR(invoiceHeaderId, (int)pdfEmailModel.CompanyType);
                if (pdfContent != null)
                {
                    notification.Attachments.Add(GetPdfAttachment(pdfContent, invoiceNumber));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "GetPdfAttachments", ex.Message, ex);
            }
        }

        private void GetPdfForInvoiceSummary(int invoiceId, string invoiceNumber, NotificationViewModel notification)
        {
            try
            {
                var pdfContent = GetPdfContentForInvoiceSummary(invoiceId);
                if (pdfContent != null)
                {
                    notification.Attachments.Add(GetPdfAttachment(pdfContent, invoiceNumber));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "GetPdfForInvoiceSummary", ex.Message, ex);
            }
        }
        private void GetPdfForMarineTaxAffidavit(int invoiceHeaderId, string invoiceNumber, NotificationViewModel notification)
        {
            try
            {
                var pdfContent = GetMarineTaxAffidavitPdfFileContent(invoiceHeaderId);
                if (pdfContent != null)
                {
                    notification.Attachments.Add(GetPdfAttachment(pdfContent, "Tax Affidavit-"+invoiceNumber));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "GetPdfForMarineTaxAffidavit", ex.Message, ex);
            }
        }

        private void GetPdfForMarineBDNImage(int invoiceHeaderId, string invoiceNumber, NotificationViewModel notification)
        {
            try
            {
                var pdfContent = GetMarineBDNPdfFileContent(invoiceHeaderId);
                if (pdfContent != null)
                {
                    notification.Attachments.Add(GetPdfAttachment(pdfContent, "BDN Image-" + invoiceNumber));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "GetPdfForMarineBDNImage", ex.Message, ex);
            }
        }

        private void GetPdfForMarineCGInspection(int invoiceHeaderId, string invoiceNumber, NotificationViewModel notification)
        {
            try
            {
                var pdfContent = GetMarineCGInspPdfFileContent(invoiceHeaderId);
                if (pdfContent != null)
                {
                    notification.Attachments.Add(GetPdfAttachment(pdfContent, "Cost Guard Insp-" + invoiceNumber));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "GetPdfForMarineCGInspection", ex.Message, ex);
            }
        }
        private void GetPdfForMarineInspVouchers(int invoiceHeaderId, string invoiceNumber, NotificationViewModel notification)
        {
            try
            {
                var pdfContent = GetMarineInspRequestVouchersPdfFileContent(invoiceHeaderId);
                if (pdfContent != null)
                {
                    notification.Attachments.Add(GetPdfAttachment(pdfContent, "Insp. Request Voucher-" + invoiceNumber));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "GetPdfForMarineInspVouchers", ex.Message, ex);
            }
        }
        private void GetPdfAttachmentsForPO(int orderId, string poNumber, NotificationViewModel notification)
        {
            try
            {
                var pdfContent = GetOrderPdfFileContent(orderId, true);
                if (pdfContent != null)
                {
                    notification.Attachments.Add(GetPdfAttachment(pdfContent, poNumber));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "GetPdfAttachmentsForPO", ex.Message, ex);
            }
        }

        private NotificationViewModel GetEmailContentForInvoice(List<int> invoicesToProcess, bool isManualTrigger = false)
        {
            var notification = new NotificationViewModel();
            if (invoicesToProcess.Count <= 0)
                return null;

            try
            {
                var notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
                string invNumber = "";
                for (int i = 0; i < invoicesToProcess.Count; i++)
                {
                    invNumber = notificationDomain.GetDisplayInvoiceNumberById(invoicesToProcess[i]);
                    GetPdfAttachmentsForInvoice(invoicesToProcess[i], invNumber, notification, isManualTrigger);
                }

                var invDdtText = !string.IsNullOrEmpty(invNumber) && invNumber.Contains(ApplicationConstants.SFDD) ? "Drop Ticket(s) " : "Invoice(s) ";
                if (!string.IsNullOrWhiteSpace(pdfEmailModel.StartDate) && !string.IsNullOrWhiteSpace(pdfEmailModel.EndDate) && !string.IsNullOrWhiteSpace(pdfEmailModel.PoNumber))
                {
                    notification.Subject = invDdtText + "from " + pdfEmailModel.StartDate + " to " + pdfEmailModel.EndDate + " for PO# " + pdfEmailModel.PoNumber;
                }
                else if (!string.IsNullOrWhiteSpace(pdfEmailModel.PoNumber))
                {
                    notification.Subject = invDdtText + "for PO# " + pdfEmailModel.PoNumber;
                }
                else
                {
                    notification.Subject = "Invoice(s) from " + pdfEmailModel.StartDate + " to " + pdfEmailModel.EndDate;
                }

                notification.BodyText = pdfEmailModel.EmailBody;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "GetEmailContentForInvoice", ex.Message, ex);
            }
             
            return notification;
        }

        private NotificationViewModel GetEmailContentForBDRPdf(int invoiceHeaderId)
        {
            var notification = new NotificationViewModel();
            if (invoiceHeaderId <= 0)
                return null;

            try
            {
                var notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
                string invNumber = "";

                invNumber = notificationDomain.GetDisplayInvoiceNumberById(0, invoiceHeaderId);
                GetPdfForBDR(invoiceHeaderId, invNumber, notification);
                notification.Subject = "BDN for invoice number " + invNumber;
                notification.BodyText = pdfEmailModel.EmailBody;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "GetEmailContentForInvoice", ex.Message, ex);
            }

            return notification;
        }

        private NotificationViewModel GetEmailContentForInvoiceSummaryPdf(int invoiceId)
        {
            var notification = new NotificationViewModel();
            if (invoiceId <= 0)
                return null;

            try
            {
                var notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
                string invNumber = "";

                invNumber = notificationDomain.GetDisplayInvoiceNumberById(invoiceId);
                GetPdfForInvoiceSummary(invoiceId, invNumber, notification);
                notification.Subject = invNumber + " Invoice Summary";
                notification.BodyText = pdfEmailModel.EmailBody;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "GetEmailContentForInvoiceSummaryPdf", ex.Message, ex);
            }

            return notification;
        }
        private NotificationViewModel GetEmailContentForMarineTaxAffidavit(int invoiceHeaderId)
        {
            var notification = new NotificationViewModel();
            if (invoiceHeaderId <= 0)
                return null;
            try
            {
                var notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
                string invNumber = "";

                invNumber = notificationDomain.GetDisplayInvoiceNumberById(0, invoiceHeaderId);
                GetPdfForMarineTaxAffidavit(invoiceHeaderId, invNumber, notification);
                notification.Subject = "Tax Affidavit for invoice number " + invNumber;
                notification.BodyText = pdfEmailModel.EmailBody;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "GetEmailContentForMarineTaxAffidavit", ex.Message, ex);
            }

            return notification;
        }

        private NotificationViewModel GetEmailContentForMarineBDNImage(int invoiceHeaderId)
        {
            var notification = new NotificationViewModel();
            if (invoiceHeaderId <= 0)
                return null;
            try
            {
                var notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
                string invNumber = "";

                invNumber = notificationDomain.GetDisplayInvoiceNumberById(0, invoiceHeaderId);
                GetPdfForMarineBDNImage(invoiceHeaderId, invNumber, notification);
                notification.Subject = "BDN Image for invoice number " + invNumber;
                notification.BodyText = pdfEmailModel.EmailBody;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "GetEmailContentForMarineTaxAffidavit", ex.Message, ex);
            }

            return notification;
        }


        private NotificationViewModel GetEmailContentForMarineCGInspection(int invoiceHeaderId)
        {
            var notification = new NotificationViewModel();
            if (invoiceHeaderId <= 0)
                return null;
            try
            {
                var notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
                string invNumber = "";

                invNumber = notificationDomain.GetDisplayInvoiceNumberById(0, invoiceHeaderId);
                GetPdfForMarineCGInspection(invoiceHeaderId, invNumber, notification);
                notification.Subject = "Coast Guard Insp for invoice number " + invNumber;
                notification.BodyText = pdfEmailModel.EmailBody;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "GetEmailContentForMarineCGInspection", ex.Message, ex);
            }

            return notification;
        }
        private NotificationViewModel GetEmailContentForMarineInspVouchers(int invoiceHeaderId)
        {
            var notification = new NotificationViewModel();
            if (invoiceHeaderId <= 0)
                return null;
            try
            {
                var notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
                string invNumber = "";

                invNumber = notificationDomain.GetDisplayInvoiceNumberById(0, invoiceHeaderId);
                GetPdfForMarineInspVouchers(invoiceHeaderId, invNumber, notification);
                notification.Subject = "Inspection Request Voucher(s) for invoice number " + invNumber;
                notification.BodyText = pdfEmailModel.EmailBody;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "GetEmailContentForMarineInspVouchers", ex.Message, ex);
            }

            return notification;
        }

        private NotificationViewModel GetEmailContentForPO()
        {
            var notification = new NotificationViewModel();
            if (pdfEmailModel.OrderId <= 0)
                return null;
            try
            {
                GetPdfAttachmentsForPO(pdfEmailModel.OrderId, pdfEmailModel.PoNumber, notification);
                notification.Subject = "PO# " + pdfEmailModel.PoNumber;

                notification.BodyText = pdfEmailModel.EmailBody;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "GetEmailContentForPO", ex.Message, ex);
            }

            return notification;
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                NotificationViewModel notificationModel = new NotificationViewModel();

                int completedCount = 0;
                if (pdfEmailModel.InvoiceId > 0 && pdfEmailModel.DocumentName == DocumentName.Invoice)
                {
                    invoiceIds = new List<int>();
                    invoiceIds.Add(pdfEmailModel.InvoiceId);
                    notificationModel = GetEmailContentForInvoice(invoiceIds, notificationEventViewModel.IsManualTrigger);

                    if (pdfEmailModel.IncludeImagesInAttachment)
                    {
                        GetInvoiceImages(notificationDomain, notificationModel);
                    }

                    SendNotificationForDefaultEvent(pdfEmailModel.ToEmailAddress, notificationModel);
                }
                else if (pdfEmailModel.OrderId > 0 && pdfEmailModel.DocumentName == DocumentName.PO)
                {
                    notificationModel = GetEmailContentForPO();
                    SendNotificationForDefaultEvent(pdfEmailModel.ToEmailAddress, notificationModel);
                }
                else if (pdfEmailModel.InvoiceHeaderId > 0 && pdfEmailModel.DocumentName == DocumentName.BDR)
                {                    
                    notificationModel = GetEmailContentForBDRPdf(pdfEmailModel.InvoiceHeaderId);
                    SendNotificationForDefaultEvent(pdfEmailModel.ToEmailAddress, notificationModel);
                }
                else if (pdfEmailModel.InvoiceId > 0 && pdfEmailModel.DocumentName == DocumentName.InvoiceSummary)
                {
                    notificationModel = GetEmailContentForInvoiceSummaryPdf(pdfEmailModel.InvoiceId);
                    SendNotificationForDefaultEvent(pdfEmailModel.ToEmailAddress, notificationModel);
                }
                else if (pdfEmailModel.InvoiceHeaderId > 0 && pdfEmailModel.DocumentName == DocumentName.MarineTaxAffidavit)
                {
                    notificationModel = GetEmailContentForMarineTaxAffidavit(pdfEmailModel.InvoiceHeaderId);
                    if (pdfEmailModel.IncludeImagesInAttachment)
                    {                      
                        GetMarineTaxAffidavitImages(notificationDomain, notificationModel);
                    }
                    SendNotificationForDefaultEvent(pdfEmailModel.ToEmailAddress, notificationModel);
                }
                else if (pdfEmailModel.InvoiceHeaderId > 0 && pdfEmailModel.DocumentName == DocumentName.BDNImage)
                {
                    notificationModel = GetEmailContentForMarineBDNImage(pdfEmailModel.InvoiceHeaderId);
                    if (pdfEmailModel.IncludeImagesInAttachment)
                    {
                        GetMarineBDNImages(notificationDomain, notificationModel);
                    }
                    SendNotificationForDefaultEvent(pdfEmailModel.ToEmailAddress, notificationModel);
                }
                else if (pdfEmailModel.InvoiceHeaderId > 0 && pdfEmailModel.DocumentName == DocumentName.CGInspection)
                {
                    notificationModel = GetEmailContentForMarineCGInspection(pdfEmailModel.InvoiceHeaderId);
                    if (pdfEmailModel.IncludeImagesInAttachment)
                    {

                        GetMarineCGInspImages(notificationDomain, notificationModel);
                    }
                    SendNotificationForDefaultEvent(pdfEmailModel.ToEmailAddress, notificationModel);
                }
                else if (pdfEmailModel.InvoiceHeaderId > 0 && pdfEmailModel.DocumentName == DocumentName.InspRequestVoucher)
                {
                    notificationModel = GetEmailContentForMarineInspVouchers(pdfEmailModel.InvoiceHeaderId);
                    if (pdfEmailModel.IncludeImagesInAttachment)
                    {

                        GetMarineInspRequestVoucherImages(notificationDomain, notificationModel);
                    }
                    SendNotificationForDefaultEvent(pdfEmailModel.ToEmailAddress, notificationModel);
                }
                else if (invoiceIds != null && invoiceIds.Count > 0)
                {
                    for (int i = 1; i <= loopCount; i++)
                    {
                        var nextInvoicesToProcess = invoiceIds.Skip(completedCount).Take(invoiceAttachmentsPerEmail).ToList();
                        notificationModel = GetEmailContentForInvoice(nextInvoicesToProcess, notificationEventViewModel.IsManualTrigger);

                        SendNotificationForDefaultEvent(pdfEmailModel.ToEmailAddress, notificationModel);
                        completedCount += invoiceAttachmentsPerEmail;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoicePdfEmailProcessor", "SendEmail", ex.Message, ex);
            }
        }

        private void GetInvoiceImages(NotificationDomain notificationDomain, NotificationViewModel notificationModel)
        {
            var invImages = Task.Run(() => notificationDomain.GetInvoiceImagesById(pdfEmailModel.InvoiceId)).Result;
            if (invImages != null && invImages.Count > 0)
            {
                foreach (var img in invImages)
                {
                    if (string.IsNullOrWhiteSpace(img.FilePath))
                        notificationModel.Attachments.Add(GetImageAttachment(img.Data, img.Name));
                    else
                    {
                        //get pdf from azure
                        var azureBlob = new Core.Infrastructure.AzureBlobStorage();
                        if (img.FilePath.Contains("||"))
                        {
                            List<ImageViewModel> images;
                            {
                                images = img.FilePath.Split(ApplicationConstants.FilePathSeparation, System.StringSplitOptions.RemoveEmptyEntries).
                                    Select(x => new ImageViewModel { FilePath = x, IsPdf = ImageViewModel.IsFilePathTypeIsNonImage(x) }).ToList();
                            }
                            foreach (var item in images)
                            {
                                var fileStream = azureBlob.DownloadBlob(item.FilePath, BlobContainerType.InvoicePdfFiles.ToString().ToLower());
                                var memoryStream = fileStream as MemoryStream;
                                if (!item.FilePath.ToLower().Contains(".pdf"))
                                {
                                    var extension = item.FilePath.Split('.')[1];
                                    notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, extension));
                                }
                                else
                                {
                                    notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, "pdf"));
                                }
                            }
                        }
                        else
                        {
                            var fileStream = azureBlob.DownloadBlob(img.FilePath, BlobContainerType.InvoicePdfFiles.ToString().ToLower());
                            var memoryStream = fileStream as MemoryStream;
                            if (!img.FilePath.ToLower().Contains(".pdf"))
                            {
                                var extension = img.FilePath.Split('.')[1];
                                notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, extension));
                            }
                            else
                            {
                                notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, "pdf"));
                            }
                        }
                    }
                }
            }
        }
        private void GetMarineTaxAffidavitImages(NotificationDomain notificationDomain, NotificationViewModel notificationModel)
        {
            var domain = new InvoiceDomain();
            var affidavitImages = domain.GetMarineTaxAffidavitImages(pdfEmailModel.InvoiceHeaderId).Result;
            if (affidavitImages != null && affidavitImages.Count > 0)
            {
                foreach (var img in affidavitImages)
                {
                    if (string.IsNullOrWhiteSpace(img.FilePath))
                        notificationModel.Attachments.Add(GetImageAttachment(img.Data, img.Name));
                    else
                    {
                        //get pdf from azure
                        var azureBlob = new Core.Infrastructure.AzureBlobStorage();
                        if (img.FilePath.Contains("||"))
                        {
                            List<ImageViewModel> images;
                            {
                                images = img.FilePath.Split(ApplicationConstants.FilePathSeparation, System.StringSplitOptions.RemoveEmptyEntries).
                                    Select(x => new ImageViewModel { FilePath = x, IsPdf = ImageViewModel.IsFilePathTypeIsNonImage(x) }).ToList();
                            }
                            foreach (var item in images)
                            {
                                var fileStream = azureBlob.DownloadBlob(item.FilePath, BlobContainerType.InvoicePdfFiles.ToString().ToLower());
                                var memoryStream = fileStream as MemoryStream;
                                if (!item.FilePath.ToLower().Contains(".pdf"))
                                {
                                    var extension = item.FilePath.Split('.')[1];
                                    notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, extension));
                                }
                                else
                                {
                                    notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, "pdf"));
                                }
                            }
                        }
                        else
                        {
                            var fileStream = azureBlob.DownloadBlob(img.FilePath, BlobContainerType.InvoicePdfFiles.ToString().ToLower());
                            var memoryStream = fileStream as MemoryStream;
                            if (!img.FilePath.ToLower().Contains(".pdf"))
                            {
                                var extension = img.FilePath.Split('.')[1];
                                notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, extension));
                            }
                            else
                            {
                                notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, "pdf"));
                            }
                        }
                    }
                }
            }
        }

        private void GetMarineBDNImages(NotificationDomain notificationDomain, NotificationViewModel notificationModel)
        {
            var domain = new InvoiceDomain();
            var BDNImages = domain.GetMarineBDNImages(pdfEmailModel.InvoiceHeaderId).Result;
            if (BDNImages != null && BDNImages.Count > 0)
            {
                foreach (var img in BDNImages)
                {
                    if (string.IsNullOrWhiteSpace(img.FilePath))
                        notificationModel.Attachments.Add(GetImageAttachment(img.Data, img.Name));
                    else
                    {
                        //get pdf from azure
                        var azureBlob = new Core.Infrastructure.AzureBlobStorage();
                        if (img.FilePath.Contains("||"))
                        {
                            List<ImageViewModel> images;
                            {
                                images = img.FilePath.Split(ApplicationConstants.FilePathSeparation, System.StringSplitOptions.RemoveEmptyEntries).
                                    Select(x => new ImageViewModel { FilePath = x, IsPdf = ImageViewModel.IsFilePathTypeIsNonImage(x) }).ToList();
                            }
                            foreach (var item in images)
                            {
                                var fileStream = azureBlob.DownloadBlob(item.FilePath, BlobContainerType.InvoicePdfFiles.ToString().ToLower());
                                var memoryStream = fileStream as MemoryStream;
                                if (!item.FilePath.ToLower().Contains(".pdf"))
                                {
                                    var extension = item.FilePath.Split('.')[1];
                                    notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, extension));
                                }
                                else
                                {
                                    notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, "pdf"));
                                }
                            }
                        }
                        else
                        {
                            var fileStream = azureBlob.DownloadBlob(img.FilePath, BlobContainerType.InvoicePdfFiles.ToString().ToLower());
                            var memoryStream = fileStream as MemoryStream;
                            if (!img.FilePath.ToLower().Contains(".pdf"))
                            {
                                var extension = img.FilePath.Split('.')[1];
                                notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, extension));
                            }
                            else
                            {
                                notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, "pdf"));
                            }
                        }
                    }
                }
            }
        }

        private void GetMarineCGInspImages(NotificationDomain notificationDomain, NotificationViewModel notificationModel)
        {
            var domain = new InvoiceDomain();
            var cgImages = domain.GetMarineTaxCGInspectionImages(pdfEmailModel.InvoiceHeaderId).Result;
            if (cgImages != null && cgImages.Count > 0)
            {
                foreach (var img in cgImages)
                {
                    if (string.IsNullOrWhiteSpace(img.FilePath))
                        notificationModel.Attachments.Add(GetImageAttachment(img.Data, img.Name));
                    else
                    {
                        //get pdf from azure
                        var azureBlob = new Core.Infrastructure.AzureBlobStorage();
                        if (img.FilePath.Contains("||"))
                        {
                            List<ImageViewModel> images;
                            {
                                images = img.FilePath.Split(ApplicationConstants.FilePathSeparation, System.StringSplitOptions.RemoveEmptyEntries).
                                    Select(x => new ImageViewModel { FilePath = x, IsPdf = ImageViewModel.IsFilePathTypeIsNonImage(x) }).ToList();
                            }
                            foreach (var item in images)
                            {
                                var fileStream = azureBlob.DownloadBlob(item.FilePath, BlobContainerType.InvoicePdfFiles.ToString().ToLower());
                                var memoryStream = fileStream as MemoryStream;
                                if (!item.FilePath.ToLower().Contains(".pdf"))
                                {
                                    var extension = item.FilePath.Split('.')[1];
                                    notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, extension));
                                }
                                else
                                {
                                    notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, "pdf"));
                                }
                            }
                        }
                        else
                        {
                            var fileStream = azureBlob.DownloadBlob(img.FilePath, BlobContainerType.InvoicePdfFiles.ToString().ToLower());
                            var memoryStream = fileStream as MemoryStream;
                            if (!img.FilePath.ToLower().Contains(".pdf"))
                            {
                                var extension = img.FilePath.Split('.')[1];
                                notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, extension));
                            }
                            else
                            {
                                notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, "pdf"));
                            }
                        }
                    }
                }
            }
        }

        private void GetMarineInspRequestVoucherImages(NotificationDomain notificationDomain, NotificationViewModel notificationModel)
        {
            var domain = new InvoiceDomain();
                        
            var voucherImages = domain.GetMarineInspRequestVoucherImages(0,pdfEmailModel.InvoiceHeaderId).Result;
            if (voucherImages != null && voucherImages.Count > 0)
            {
                foreach (var img in voucherImages)
                {
                    if (string.IsNullOrWhiteSpace(img.FilePath))
                        notificationModel.Attachments.Add(GetImageAttachment(img.Data, img.Name));
                    else
                    {
                        //get pdf from azure
                        var azureBlob = new Core.Infrastructure.AzureBlobStorage();
                        if (img.FilePath.Contains("||"))
                        {
                            List<ImageViewModel> images;
                            {
                                images = img.FilePath.Split(ApplicationConstants.FilePathSeparation, System.StringSplitOptions.RemoveEmptyEntries).
                                    Select(x => new ImageViewModel { FilePath = x, IsPdf = ImageViewModel.IsFilePathTypeIsNonImage(x) }).ToList();
                            }
                            foreach (var item in images)
                            {
                                var fileStream = azureBlob.DownloadBlob(item.FilePath, BlobContainerType.InvoicePdfFiles.ToString().ToLower());
                                var memoryStream = fileStream as MemoryStream;
                                if (!item.FilePath.ToLower().Contains(".pdf"))
                                {
                                    var extension = item.FilePath.Split('.')[1];
                                    notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, extension));
                                }
                                else
                                {
                                    notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, "pdf"));
                                }
                            }
                        }
                        else
                        {
                            var fileStream = azureBlob.DownloadBlob(img.FilePath, BlobContainerType.InvoicePdfFiles.ToString().ToLower());
                            var memoryStream = fileStream as MemoryStream;
                            if (!img.FilePath.ToLower().Contains(".pdf"))
                            {
                                var extension = img.FilePath.Split('.')[1];
                                notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, extension));
                            }
                            else
                            {
                                notificationModel.Attachments.Add(GetPdfAttachment(memoryStream, img.Name, "pdf"));
                            }
                        }
                    }
                }
            }
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
        }

        public override List<NotificationUserViewModel> LoadSupplierDefaultEmailReceivers(NotificationEventViewModel notificationEventViewModel)
        {
            return new List<NotificationUserViewModel>();
        }

        public override List<NotificationUserViewModel> LoadBuyerDefaultEmailReceivers(NotificationEventViewModel notificationEventViewModel)
        {
            return new List<NotificationUserViewModel>();
        }
    }
}
