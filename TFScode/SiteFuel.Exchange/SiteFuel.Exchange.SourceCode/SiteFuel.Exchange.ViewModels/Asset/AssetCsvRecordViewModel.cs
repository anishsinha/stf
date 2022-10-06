using FileHelpers;

namespace SiteFuel.Exchange.ViewModels
{
    [DelimitedRecord(",")]
    public class AssetCsvRecordViewModel
    {
        [FieldQuoted]
        public string Name { get; set; }

        [FieldQuoted]
        public string Make { get; set; }

        [FieldQuoted]
        public string Model { get; set; }

        [FieldQuoted]
        public string Year { get; set; }

        [FieldQuoted]
        public string Class { get; set; }

        [FieldQuoted]
        public string Subcontractor { get; set; }

        [FieldQuoted]
        public string Color { get; set; }

        [FieldQuoted]
        public string FuelType { get; set; }

        public string FuelCapacity { get; set; }

        [FieldQuoted]
        public string AssetID { get; set; }

        [FieldQuoted]
        public string TelematicsProvider { get; set; }

        [FieldQuoted]
        public string LicensePlateState { get; set; }

        [FieldQuoted]
        public string LicensePlate { get; set; }

        [FieldQuoted]
        public string Vendor { get; set; }

        [FieldQuoted]
        public string Description { get; set; }
    }
}
