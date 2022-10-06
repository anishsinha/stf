using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class TelaFuelServiceViewModel
    {
        public Guid BillToGUID { get; set; }
        public string BillToName { get; set; }
        public string CarrierLookup { get; set; }
        public string FreightLaneLookup { get; set; }
        public System.Nullable<bool> IsDelivered { get; set; }
        public List<OrderDrop> OrderDrops { get; set; }
        public List<OrderLift> OrderLifts { get; set; }
        public string ReferenceNumber { get; set; }
    }
    public class OrderDrop
    {
        public List<DropProduct> DropProducts { get; set; }
        public System.Nullable<System.DateTimeOffset> DroppedDateTimeLocal { get; set; }
        public System.Nullable<System.DateTimeOffset> DroppedDateTime
        {
            get
            {
                if (DroppedDateTimeLocal.HasValue)
                {
                    return DroppedDateTimeLocal.Value.ToUniversalTime();
                }
                else
                {
                    return null;
                }
            }
        }
        public string DroppedDateTimeLocalTimeZone { get; set; }
        public System.DateTimeOffset EarliestDateTimeLocal { get; set; }
        public System.DateTimeOffset EarliestDateTime
        {
            get
            {
                return DroppedDateTimeLocal.Value.ToUniversalTime();
            }
        }

        public string EarliestDateTimeLocalTimeZone { get; set; }
        public System.DateTimeOffset LatestDateTimeLocal { get; set; }
        public System.DateTimeOffset LatestDateTime
        {
            get
            {

                return LatestDateTimeLocal.ToUniversalTime();
            }
        }

        public string LatestDateTimeLocalTimeZone { get; set; }
        public System.Nullable<System.DateTimeOffset> ScheduledDateTimeLocal { get; set; }
        public System.Nullable<System.DateTimeOffset> ScheduleDateTime
        {
            get
            {
                if (ScheduledDateTimeLocal.HasValue)
                {
                    return ScheduledDateTimeLocal.Value.ToUniversalTime();
                }
                else
                {
                    return null;
                }
            }
        }

        public string ScheduledDateTimeLocalTimeZone { get; set; }
        public short SequenceNumber { get; set; }
        public string SiteLookup { get; set; }
        public string TMWSiteId { get; set; }
    }

    public class DropProduct
    {
        public System.Nullable<decimal> GrossQuantity { get; set; }
        public System.Nullable<decimal> NetQuantity { get; set; }
        public System.Nullable<decimal> OrderQuantity { get; set; }
        public string ProductLookup { get; set; }
        public string TMWProductId { get; set; }
        public int SourceLiftSequenceNumber { get; set; }
        public string TankLookup { get; set; }
        public string TankNumber { get; set; }
        public string TMWTankId { get; set; }
    }

    public class OrderLift
    {
        public string BillOfLadingNumber { get; set; }
        public System.Nullable<System.DateTimeOffset> LiftDateTimeLocal { get; set; }
        public System.Nullable<System.DateTimeOffset> LiftDateTime
        {
            get
            {
                if (LiftDateTimeLocal.HasValue)
                {
                    return LiftDateTimeLocal.Value.ToUniversalTime();
                }
                else
                {
                    return null;
                }
            }
        }

        public string LiftDateTimeLocalTimeZone { get; set; }
        public List<LiftProduct> LiftProducts { get; set; }
        public short SequenceNumber { get; set; }
        public string SupplierLookup { get; set; }
        public string TerminalName { get; set; }
        public int InvoiceId { get; set; }
    }
    public class LiftProduct
    {
        public System.Nullable<decimal> GrossQuantity { get; set; }
        public System.Nullable<decimal> NetQuantity { get; set; }
        public string ProductLookup { get; set; }
        public int SequenceNumber { get; set; }
        public string TMWProductId { get; set; }
    }

}
