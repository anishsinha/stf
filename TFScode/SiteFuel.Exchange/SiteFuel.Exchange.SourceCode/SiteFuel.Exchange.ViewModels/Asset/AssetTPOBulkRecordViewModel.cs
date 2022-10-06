using CsvHelper.Configuration.Attributes;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetTPOBulkRecordViewModel
    {
        [Name("Customer*")]
        public string Customer { get; set; }
        [Name("Site Name*")]
        public string SiteName { get; set; }

        [Name("Asset Name*")]
        public string Name { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Year { get; set; }

        public string Class { get; set; }

        [Name("Sub Contractor")]
        public string Subcontractor { get; set; }
        [Name("Contract #")]
        public string Contract { get; set; }
        public string FuelType { get; set; }

        public string FuelCapacity { get; set; }

        public string AssetID { get; set; }

        public string TelematicsProvider { get; set; }

        public string LicensePlateState { get; set; }

        public string LicensePlate { get; set; }

        [Name("Supplier/Vendor")]
        public string Vendor { get; set; }

        [Name("AdditionalDetails")]
        public string Description { get; set; }
    }
}
