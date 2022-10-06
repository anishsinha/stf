using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
	public abstract class BaseInvoiceEventProcessor : BaseEventProcessor
	{
		protected NotificationDomain notificationDomain;
        protected NotificationInvoiceViewModel viewModel;

        public override List<NotificationUserViewModel> LoadBuyerDefaultEmailReceivers(NotificationEventViewModel notificationEventViewModel)
		{
			return notificationDomain.GetBuyerDefaultRecievers(notificationEventViewModel.TriggeredByCompanyId, notificationEventViewModel.EventType);
		}

		public override List<NotificationUserViewModel> LoadSupplierDefaultEmailReceivers(NotificationEventViewModel notificationEventViewModel)
		{
			//throw new NotImplementedException();
			return new List<NotificationUserViewModel>();
		}

        public virtual void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetInvoiceNotificationDetails(notificationEventViewModel.EntityId);
            _doNotSendInvoiceAttachment = viewModel.IsPartOfStatement || viewModel.IsProFormaPo || (!viewModel.SendAttachmentToBuyer && !viewModel.SendAttachmentToSupplier);
        }

        public void GetInvoiceTypeId(NotificationInvoiceViewModel viewModel)
        {
            if (viewModel.ReplaceInvoiceWithDdt)
            {
                viewModel.InvoiceNumber = viewModel.InvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);
                if (viewModel.InvoiceType == (int)InvoiceType.Manual)
                    viewModel.InvoiceType = (int)InvoiceType.DigitalDropTicketManual;
                if (viewModel.InvoiceType == (int)InvoiceType.MobileApp)
                    viewModel.InvoiceType = (int)InvoiceType.DigitalDropTicketMobileApp;
            }
        }

        public NotificationViewModel ReplaceInvoiceContentWithDdt(NotificationViewModel viewModel)
        {
            viewModel.BodyButtonUrl = string.Empty;
            viewModel.BodyButtonText = string.Empty;
            viewModel.Subject = viewModel.Subject.Replace(Resource.lblInvoice, Resource.lblDropTicket);
            viewModel.BodyText = viewModel.BodyText.Replace(Resource.lblInvoice, Resource.lblDropTicket);

            return viewModel;
        }
    }
}
