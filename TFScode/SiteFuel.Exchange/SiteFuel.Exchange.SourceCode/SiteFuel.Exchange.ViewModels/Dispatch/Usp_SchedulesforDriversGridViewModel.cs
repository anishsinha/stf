using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class Usp_SchedulesforDriversGridViewModel
    {
        public Usp_SchedulesforDriversGridViewModel()
        {
        }
        public Usp_SchedulesforDriversGridViewModel(Usp_SchedulesforDriversGridViewModel originalObj)
        {
            Id = originalObj.Id;
            TrackableScheduleId = originalObj.TrackableScheduleId;
            OrderId = originalObj.OrderId;
            DriverId = originalObj.DriverId;
            JobId = originalObj.JobId;
            DriverName = originalObj.DriverName;
            PhoneNumber = originalObj.PhoneNumber;
            PoNumber = originalObj.PoNumber;
            DeliveryWindow = originalObj.DeliveryWindow;
            FuelType = originalObj.FuelType;
            Quantity = originalObj.Quantity;
            Customer = originalObj.Customer;
            Location = originalObj.Location;
            Status = originalObj.Status;
            MaxQuantity = originalObj.MaxQuantity;
            OrderClosingThreshold = originalObj.OrderClosingThreshold;
            DeliveryEndDate = originalObj.DeliveryEndDate;
            JobEndDate = originalObj.JobEndDate;
            ScheduleTypeId = originalObj.ScheduleTypeId;
            ScheduleDate = originalObj.ScheduleDate;
            WeekDayId = originalObj.WeekDayId;
            IsPastSchedule = originalObj.IsPastSchedule;
            Latitude = originalObj.Latitude;
            Longitude = originalObj.Longitude;
            CountryId = originalObj.CountryId;
            CountryCode = originalObj.CountryCode;
            DeliverySchedule = originalObj.DeliverySchedule;
            QuantityTypeId = originalObj.QuantityTypeId;
            Currency = originalObj.Currency;
            DriverAcknowledgementStatus = originalObj.DriverAcknowledgementStatus;
            PickupLocation = originalObj.PickupLocation;
            DeliveryScheduleType = originalObj.DeliveryScheduleType;
        }
        public Nullable<int> Id { get; set; }

        public Nullable<int> TrackableScheduleId { get; set; }

        public int OrderId { get; set; }

        public int DriverId { get; set; }

        public string DriverName { get; set; }

        public int JobId { get; set; }

        public int QuantityTypeId { get; set; }

        public string PhoneNumber { get; set; }

        public string PoNumber { get; set; }

        public string Date { get; set; }

        public string DeliveryWindow { get; set; }

        public string FuelType { get; set; }

        public decimal Quantity { get; set; }

        public string Customer { get; set; }

        public string Location { get; set; }

        public string Status { get; set; }

        public decimal MaxQuantity { get; set; }

        public Nullable<int> OrderClosingThreshold { get; set; }

        public int ParentId { get; set; }

        public Nullable<DateTimeOffset> DeliveryEndDate { get; set; }

        public Nullable<DateTimeOffset> JobEndDate { get; set; }

        public int ScheduleTypeId { get; set; }

        public DateTimeOffset ScheduleDate { get; set; }

        public int WeekDayId { get; set; }

        public int IsPastSchedule { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string DeliverySchedule { get; set; }

        public string JobTimeZone { get; set; }

        public TimeSpan ScheduleEndTime { get; set; }

        public Currency Currency { get; set; }

        public int DeliveryScheduleId { get; set; }

        public string CountryCode { get; set; }

        public int CountryId { get; set; }

        public bool IsFtlOrder { get; set; }

        public Nullable<int> DeliveryGroupId { get; set; }

        public DriverAcknowledgementStatus DriverAcknowledgementStatus { get; set; } = DriverAcknowledgementStatus.Unknown;

        public String PickupLocation { get; set; }
        public string DeliveryScheduleType { get; set; }

    }
}
