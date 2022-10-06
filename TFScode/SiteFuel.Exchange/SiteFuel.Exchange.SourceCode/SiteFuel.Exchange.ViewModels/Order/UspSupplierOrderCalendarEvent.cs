using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspSupplierOrderCalendarEvent
    {
        public UspSupplierOrderCalendarEvent()
        {

        }
        public UspSupplierOrderCalendarEvent(UspSupplierOrderCalendarEvent model, DateTimeOffset scheduleDate)
        {
            Id = model.Id;
            StartDate = scheduleDate;
            Quantity = model.Quantity;
            CompanyName = model.CompanyName;
            StartTime = model.StartTime;
            EndTime = model.EndTime;
            DriverId = model.DriverId;
            DriverFirstName = model.DriverFirstName;
            DriverLastName = model.DriverLastName;
            JobCompanyName = model.JobCompanyName;
            PoNumber = model.PoNumber;
            JobStateCode = model.JobStateCode;
            JobZipCode = model.JobZipCode;
            StatusId = model.StatusId;
            UoM = model.UoM;
        }
        public int Id { get; set; }

        public int? InvoiceId { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public int DeliveryTypeId { get; set; }

        public string PoNumber { get; set; }

        public decimal Quantity { get; set; }

        public decimal Delivered { get; set; }

        public int StatusId { get; set; }

        public string OrderStatus { get; set; }

        public int? DriverId { get; set; }

        public string DriverFirstName { get; set; }

        public string DriverLastName { get; set; }

        public string CompanyName { get; set; }

        public string JobStateCode { get; set; }

        public string JobZipCode { get; set; }

        public string TimeZone { get; set; }

        public int CalendarEventType { get; set; }

        public int ScheduleType { get; set; }

        public DateTimeOffset? OrderEndDate { get; set; }

        public decimal OrderQuantity { get; set; }

        public string JobCompanyName { get; set; }

        public UoM UoM { get; set; }
    }
}
