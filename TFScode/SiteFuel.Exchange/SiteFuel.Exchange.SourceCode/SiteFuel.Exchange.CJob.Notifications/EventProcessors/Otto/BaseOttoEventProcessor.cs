﻿using SiteFuel.Exchange.Core;
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
	public abstract class BaseOttoEventProcessor : BaseEventProcessor
	{
		protected NotificationDomain notificationDomain;

        public override List<NotificationUserViewModel> LoadBuyerDefaultEmailReceivers(NotificationEventViewModel notificationEventViewModel)
		{
			return new List<NotificationUserViewModel>();
        }

		public override List<NotificationUserViewModel> LoadSupplierDefaultEmailReceivers(NotificationEventViewModel notificationEventViewModel)
		{
			return new List<NotificationUserViewModel>();
		}
    }
}