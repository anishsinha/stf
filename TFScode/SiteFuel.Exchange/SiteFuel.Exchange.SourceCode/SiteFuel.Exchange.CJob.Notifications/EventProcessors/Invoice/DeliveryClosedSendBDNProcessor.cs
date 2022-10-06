using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
	public class DeliveryClosedSendBDNProcessor : BaseDeliveryScheduleEventProcessor, IEmailProcessor
    {
		private NotificationBDNViewModel viewModel;

        public EventType EventType => EventType.DeliveryClosedSendBDN;

        public DeliveryClosedSendBDNProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
			notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetBDNNotificationDetails(notificationEventViewModel.EntityId);
        }

		public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
        }

		public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
            viewModel.DefaultSupplierEmailRecievers = defaultRecievers;
        }

		public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
		{
            try
            {
                ProcessDeliveryClosedSendBDNDetails(notificationEventViewModel, viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryClosedSendBDNProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        public void ProcessDeliveryClosedSendBDNDetails(NotificationEventViewModel notificationEvent, NotificationBDNViewModel viewModel)
        {
            try
            {
                NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

                if (viewModel.Id > 0)
                {
                    //Get Email Details
                    var notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryClosedSendBDN, _serverUrl, string.Empty, (int)notificationEvent.EventType);
                    var bolsForEmail = string.Empty;
                    var bolsForSms = string.Empty;
                    string bodyText = notification.BodyText;
                    string smsText = notification.SmsText;
                    string UoM = viewModel.UoM == Utilities.UoM.Litres ? "ltrs" : "gals";
                    foreach (var item in viewModel.BDNBolDetails)
                    {
                        string bol = "BOL[" + item.BolNumber + "]- " + $"{item.NetQuantity} {UoM}" + " Net /" + $"{item.GrossQuantity} {UoM}" + " Gross";
                        bolsForEmail += bol + "<br/>";
                        bolsForSms += bol + "\n";
                    }

                    var pdfContent = GetPdfContentForBDR(notificationEvent.EntityId, (int)CompanyType.Supplier);
                    if (pdfContent != null)
                    {
                        notification.Attachments = GetAttachments(pdfContent, viewModel.InvoiceNumber);
                    }

                    foreach (var item in viewModel.Users)
                    {
                        notification.BodyText = string.Format(bodyText,
                                                    $"{item.FirstName} {item.LastName}",
                                                    viewModel.BuyerCompanyName,
                                                    viewModel.Vessle == string.Empty ? viewModel.JobName : $"{viewModel.JobName}{"-"}{viewModel.Vessle}",
                                                    viewModel.CreatedOn.ToString(Resource.constFormatDate),
                                                    $"{viewModel.DroppedQuantity} {UoM}",
                                                    viewModel.TotalBolCount,
                                                    bolsForEmail,
                                                    viewModel.CalculatedAPIGravity,
                                                    viewModel.DensityInVaccum,
                                                    viewModel.ObservedTemperature,
                                                    viewModel.SulphurContent,
                                                    viewModel.Viscosity,
                                                    viewModel.FlashPoint);

                        notification.Subject = string.Format(notification.Subject, viewModel.BuyerCompanyName, viewModel.JobName, viewModel.CreatedOn.ToString(Resource.constFormatDate));

                        //Get SMS Details
                        notification.SmsText = string.Format(smsText,
                                                    viewModel.BuyerCompanyName,
                                                    viewModel.Vessle == string.Empty ? viewModel.JobName : $"{viewModel.JobName}{"-"}{viewModel.Vessle}",
                                                    viewModel.CreatedOn.ToString(Resource.constFormatDate),
                                                    $"{viewModel.DroppedQuantity} {UoM}",
                                                    viewModel.TotalBolCount,
                                                    bolsForSms,
                                                    viewModel.CalculatedAPIGravity,
                                                    viewModel.DensityInVaccum,
                                                    viewModel.ObservedTemperature,
                                                    viewModel.SulphurContent,
                                                    viewModel.Viscosity,
                                                    viewModel.FlashPoint,
                                                    "\n");

                        SendNotificationForDefaultEvent(item.Email, notification);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryClosedSendBDNProcessor", "ProcessDeliveryClosedSendBDNDetails", "Exception Details : ", ex);
            }
        }
    }
}
