using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Notification;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationDeliveryRequestViewModel : BaseNotificationViewModel
    {
        public NotificationDeliveryRequestViewModel()
        {
            CurrentSchedules = new List<DeliveryScheduleDetail>();
            PreviousSchedules = new List<DeliveryScheduleDetail>();
        }
        public int Id { get; set; }

        public string PoNumber { get; set; }

        public int JobId { get; set; }
        
        public string JobName { get; set; }

        public string TankName { get; set; }

        public string ProductType { get; set; }

        public string FuelType { get; set; }
        
        public int OrderId { get; set; }

        public int SupplierCompanyId { get; set; }

        public string SupplierCompanyName { get; set; }

        public int BuyerCompanyId { get; set; }

        public string BuyerCompanyName { get; set; }

        public int QuantityId { get; set; }

        public decimal Quantity { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public string WeekDay { get; set; }

        public UserRoles UserRole { get; set; }

        public NotificationUserViewModel Buyer { get; set; }

        public NotificationUserViewModel Supplier { get; set; }

        public string SupplierContactNumber { get; set; }

        public string DriverName { get; set; }

        public IEnumerable<DeliveryScheduleDetail> CurrentSchedules { get; set; }

        public IEnumerable<DeliveryScheduleDetail> PreviousSchedules { get; set; }

        public int FuelRequestTypeId { get; set; }

        public UoM UoM { get; set; }
        public string UniqueOrderNo { get; set; }
        public string URLDetails { get; set; }
        public bool IsBlendedRequest { get; set; } = false;
        public List<BlendedProductDetails> BlendedProductDetails { get; set; } = new List<BlendedProductDetails>();
    }
    public class BlendedProductDetails
    {
        public string ProductType { get; set; }
        public string FuelType  { get; set; }
        public string Quantity { get; set; }

    }
}
