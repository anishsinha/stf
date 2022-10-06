﻿using SiteFuel.Exchange.ViewModels.Notification;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationCarrierExceedsDeliveryViewModel : BaseNotificationViewModel
    {
        public NotificationCarrierExceedsDeliveryViewModel()
        {
        }

        public int SupplierCompanyId { get; set; }
        
        public string Location { get; set; }

        public int OrderId { get; set; }

        public int CarrierCompanyId { get; set; }
        
        public string Carrier { get; set; }

        public string Month { get; set; }

        public string Product { get; set; }

        public int EstimatedDelivery { get; set; }

        public int ActualDelivery { get; set; }

        public string EstimatedQuantity { get; set; }

        public string ActualQuantity { get; set; }

        public string CurrentInventory { get; set; }

        public string Ullage { get; set; }

        public int DaysRemaining { get; set; }
    }
}
