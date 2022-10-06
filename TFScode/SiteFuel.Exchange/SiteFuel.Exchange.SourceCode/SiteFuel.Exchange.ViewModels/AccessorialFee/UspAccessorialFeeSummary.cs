using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspAccessorialFeeSummary
    {
        public int Id { get; set; }

        public string TableName { get; set; }

        public TableTypes TableType { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public string Company { get; set; }

        public string SourceRegion { get; set; }

        public string Terminal { get; set; }

        public string BulkPlant { get; set; }

        public FreightTableStatus StatusId { get; set; }
    }
}
