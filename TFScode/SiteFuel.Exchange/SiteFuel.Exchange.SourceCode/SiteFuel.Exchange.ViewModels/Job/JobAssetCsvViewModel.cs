using FileHelpers;

namespace SiteFuel.Exchange.ViewModels
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class JobAssetCsvViewModel
    {
        [FieldQuoted]
        public string Branch { get; set; }

        [FieldQuoted]
        public string Account { get; set; }

        [FieldQuoted]
        public string Contract { get; set; }

        [FieldQuoted]
        public string Subcontractor { get; set; }

        [FieldQuoted]
        public string Location { get; set; }

        [FieldQuoted]
        public string OrderedBy { get; set; }

        [FieldQuoted]
        public string PONumber { get; set; }

        [FieldQuoted]
        public string EquipmentType { get; set; }

        [FieldQuoted]
        public string EquipmentId { get; set; }

        [FieldQuoted]
        public string Make { get; set; }

        [FieldQuoted]
        public string Model { get; set; }

        [FieldQuoted]
        public string Quantity { get; set; }

        [FieldQuoted]
        public string ReturnDate { get; set; }

        [FieldQuoted]
        public string DayRate { get; set; }

        [FieldQuoted]
        public string WeekRate { get; set; }

        [FieldQuoted]
        public string FourWeekRate { get; set; }

        [FieldQuoted]
        public string BilledThrough { get; set; }

        [FieldQuoted]
        public string RentedOn { get; set; }
    }
}
