using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverDropOrderViewModel
    {
        public DriverDropOrderViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            DropStartDate = DateTimeOffset.Now;
            DropEndDate = DateTimeOffset.Now;
            Driver = new DriverViewModel();
            SpecialInstructions = new Dictionary<string, bool>();
        }

        public int OrderId { get; set; }

        public int FuelId { get; set; }

        public decimal Quantity { get; set; }

        public DriverViewModel Driver { get; set; }

        public ImageViewModel Image { get; set; }

        public ImageViewModel AdditionalImage { get; set; }

        public Dictionary<string, bool> SpecialInstructions { get; set; }

        public DateTimeOffset DropStartDate { get; set; }

        public DateTimeOffset DropEndDate { get; set; }

        public bool IsWetHosingDelivery { get; set; }

        public bool IsOverWaterDelivery { get; set; }

        public int InvoiceId { get; set; }

        public int? TrackableScheduleId { get; set; }

        public int? DeliveryScheduleId { get; set; }

        public string TraceId { get; set; }

        public int InvoiceStatusId { get; set; }

        public int UnitOfMeasurement { get; set; }

        public DispatchLocationViewModel FuelPickLocation { get; set; }

        public BolDetailViewModel BolDetails { get; set; }

        public CustomerSignatureViewModel CustomerSignatureViewModel { get; set; }

        public CreationMethod CreationMethod { get; set; }

        public DriverDropOrderViewModel Clone(int orderId = 0)
        {
            var thisObject = (DriverDropOrderViewModel)this.MemberwiseClone();
            thisObject.Driver = thisObject.Driver.Clone();
            if (orderId > 0)
            {
                thisObject.OrderId = orderId;
            }
            return thisObject;
        }
    }
}
