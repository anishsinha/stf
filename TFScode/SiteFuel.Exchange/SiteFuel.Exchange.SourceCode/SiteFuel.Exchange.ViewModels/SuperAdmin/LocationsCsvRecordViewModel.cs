using FileHelpers;

namespace SiteFuel.Exchange.ViewModels
{
    [DelimitedRecord(",")]
    public class LocationsCsvRecordViewModel
    {
        [FieldQuoted]
        public string Address { get; set; }

        [FieldQuoted]
        public string City { get; set; }

        [FieldQuoted]
        public string State { get; set; }

        [FieldQuoted]
        public string ZipCode { get; set; }

        [FieldQuoted]
        public string PhoneNo { get; set; }

        [FieldQuoted]
        public string MondayOpeningHours { get; set; }

        [FieldQuoted]
        public string MondayStartTime { get; set; }

        [FieldQuoted]
        public string MondayEndTime { get; set; }

        [FieldQuoted]
        public string TuesdayOpeningHours { get; set; }

        [FieldQuoted]
        public string TuesdayStartTime { get; set; }

        [FieldQuoted]
        public string TuesdayEndTime { get; set; }

        [FieldQuoted]
        public string WednesdayOpeningHours { get; set; }

        [FieldQuoted]
        public string WednesdayStartTime { get; set; }

        [FieldQuoted]
        public string WednesdayEndTime { get; set; }

        [FieldQuoted]
        public string ThursdayOpeningHours { get; set; }

        [FieldQuoted]
        public string ThursdayStartTime { get; set; }

        [FieldQuoted]
        public string ThursdayEndTime { get; set; }

        [FieldQuoted]
        public string FridayOpeningHours { get; set; }

        [FieldQuoted]
        public string FridayStartTime { get; set; }

        [FieldQuoted]
        public string FridayEndTime { get; set; }

        [FieldQuoted]
        public string SaturdayOpeningHours { get; set; }

        [FieldQuoted]
        public string SaturdayStartTime { get; set; }

        [FieldQuoted]
        public string SaturdayEndTime { get; set; }

        [FieldQuoted]
        public string SundayOpeningHours { get; set; }

        [FieldQuoted]
        public string SundayStartTime { get; set; }

        [FieldQuoted]
        public string SundayEndTime { get; set; }

        [FieldQuoted]
        public string FuelType { get; set; }

        [FieldQuoted]
        public string DBE { get; set; }

        [FieldQuoted]
        public string HedgeOrders { get; set; }

        [FieldQuoted]
        public string WaterRefueling { get; set; }

        [FieldQuoted]
        public string States { get; set; }

        [FieldQuoted]
        public string SpecificRadius { get; set; }
    }
}
