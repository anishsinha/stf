using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspCompletedDeliveriesViewModel : StatusViewModel
    {
        public UspCompletedDeliveriesViewModel()
        {
            InstanceInitialize();
        }

        public UspCompletedDeliveriesViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            // Varaiable of type object should be initialized here
        }

        public int InvoiceId { get; set; }

        public string PoNumber { get; set; }

        public string Number { get; set; }

        public string ScheduledDate { get; set; }

        public string DropDate { get; set; }

        public string ScheduledTime { get; set; }

        public string DroppedTime { get; set; }

        public decimal QuantityScheduled { get; set; }

        public decimal QuanityDropped { get; set; }

        public string ScheduledDriver { get; set; }

        public string Driver { get; set; }

        public decimal Overage { get; set; }

        public string ScheduleStatus { get; set; }

        public string IsOverageAllowed { get; set; }

        public int IsDropTimeLate { get; set; }

        public int QuantityTypeId { get; set; }

        public int IsDropDateLate { get; set; }

        public string IsDeliverySchedule { get; set; }

        public string Customer { get; set; }

        public Nullable<int> OrderId { get; set; }

        public int ScheduleStatusId { get; set; }

        public string AppManual { get; set; }

        public UoM UoM { get; set; }

        public string DisplayUoM { get; set; }
    }
}
