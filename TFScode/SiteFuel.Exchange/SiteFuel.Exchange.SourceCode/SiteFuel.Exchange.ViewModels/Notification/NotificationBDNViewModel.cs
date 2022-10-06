using System;
using System.Collections.Generic;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Notification;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationBDNViewModel : BaseNotificationViewModel
    {
        public NotificationBDNViewModel()
        {
            BuyerUser = new NotificationUserViewModel();
            SupplierUser = new NotificationUserViewModel();
            Attachments = new List<System.Net.Mail.Attachment>();
        }

        public int Id { get; set; }
        public List<NotificationBDNBolDetailViewModel> BDNBolDetails { get; set; }
        public NotificationUserViewModel BuyerUser { get; set; }
        public NotificationUserViewModel SupplierUser { get; set; }
        public int SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
        public int InvoiceType { get; set; }
        public string JobName { get; set; }
        public string Vessle { get; set; }
        public UoM UoM { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string DropDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string DroppedQuantity { get; set; }
        public decimal TotalBolCount { get; set; }
        public string ObservedTemperature { get; set; }
        public string CalculatedAPIGravity { get; set; }
        public string SulphurContent { get; set; }
        public string Viscosity { get; set; }
        public string FlashPoint { get; set; }
        public string DensityInVaccum { get; set; }
        public List<System.Net.Mail.Attachment> Attachments { get; set; }
        public List<NotificationUserViewModel> Users { get; set; }
    }

    public class NotificationBDNBolDetailViewModel
    {
        public string BolNumber { get; set; }
        public string NetQuantity { get; set; }
        public string GrossQuantity { get; set; }
    }
}
