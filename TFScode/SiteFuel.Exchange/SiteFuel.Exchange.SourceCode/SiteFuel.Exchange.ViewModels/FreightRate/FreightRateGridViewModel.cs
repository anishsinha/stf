using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class FreightRateGridViewModel
    {
        public int Id { get; set; }

        public string DateRange { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string TableName { get; set; }

        public string FreightRateRuleType { get; set; }

        public int FreightRateRuleTypeValue { get; set; }

        public string Customer { get; set; }

        public string Carrier { get; set; }

        public string SourceRegion { get; set; }

        public string Terminal { get; set; }

        public string BulkPlant { get; set; }

        public string FuelGroup { get; set; }

        public string TableType { get; set; }

        public bool IsArchived { get; set; }

        public string StatusName { get; set; }
    }
}
