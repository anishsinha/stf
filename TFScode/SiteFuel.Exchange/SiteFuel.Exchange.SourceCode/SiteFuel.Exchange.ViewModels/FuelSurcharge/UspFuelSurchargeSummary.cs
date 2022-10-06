using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspFuelSurchargeSummary
    {
        public int Id { get; set; }

        public string TableName { get; set; }

        public TableTypes TableType { get; set; }

        public string IndexProduct { get; set; }

        public string IndexPeriod { get; set; }

        public string IndexArea { get; set; }

        public IndexType IndexType { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public string Company { get; set; }

        public string SourceRegion { get; set; }

        public string Terminal { get; set; }

        public string BulkPlant { get; set; }

        public SurchargeProductTypes ProductType { get; set; }

        public FreightTableStatus StatusId { get; set; }
    }
}
