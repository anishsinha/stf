using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Notification;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobMessageViewModel
    {
        public string ProFormaPoStatus { get; set; }
    }

    public class OrderMessageViewModel
    {
        public string PreviousPoNumber { get; set; }

        public int InvoiceId { get; set; }

        public string InvoiceNumber { get; set; }
    }

    public class OrderFuelSurchargeMessageViewModel
    {
        public string FuelSurchargeStatus { get; set; }
    }

    public class TankDeliveryRequestMessageViewModel
    {
        public string EntityId { get; set; }
    }

    public class BrokerDeliveryRequestMessageViewModel
    {
        public string EntityId { get; set; }

        public int AssignedTo { get; set; }

        public int AssignedToCompanyId { get; set; }

        public CompanyType CompanyType { get; set; }
        public string BlendedGroupId { get; set; }
    }

    public class AddUserMessageViewModel
    {
        public string SupplierURL { get; set; }
    }
}
