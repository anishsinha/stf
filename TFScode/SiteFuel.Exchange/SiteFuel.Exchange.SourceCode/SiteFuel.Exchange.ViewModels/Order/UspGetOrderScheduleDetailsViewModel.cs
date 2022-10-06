using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspGetOrderScheduleDetailsViewModel
    {
        public int Version { get; set; }
        public int? DriverId { get; set; }
        public int OrderId { get; set; }
        public string AdditionalNotes { get; set; }
        public int? Id { get; set; }
        public DateTimeOffset? Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal? Quantity { get; set; }
        public int? WeekDayId { get; set; }
        public int? Type { get; set; }
        public int? GroupId { get; set; }
        public int? CreatedBy { get; set; }
        public int? StatusId { get; set; }
        public bool? IsRescheduled { get; set; }
        public int? RescheduledTrackableId { get; set; }
        public UoM? UoM { get; set; }
        public string LoadCode { get; set; }
        public int? CarrierId { get; set; }
        public string SupplierContract { get; set; }
        public int? SupplierSourceId { get; set; }
        public string WeekDayCode { get; set; }
        public string ScheduleTypeName { get; set; }
        public string CarrierName { get; set; }
        public string SourceName { get; set; }
        public string DriverName { get; set; }
        public string PhoneNumber { get; set; }
        public int? TrackableScheduleId { get; set; }
        public int? ParentId { get; set; }
        public bool IsSkipped { get; set; }
        public bool IsActive { get; set; }
        public int? LocationId { get;set;}
        public string SiteName { get; set; }
        public string TimeZoneName { get; set; }
        public string CountyName { get; set; }
        public string CountryCode { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public int? StateId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public Currency? Currency { get; set; }
        public bool IsDeliveryIn24Hrs { get; set; }
        public ScheduleQuantityType ScheduleQuantityTypeId { get; set; }
    }
}