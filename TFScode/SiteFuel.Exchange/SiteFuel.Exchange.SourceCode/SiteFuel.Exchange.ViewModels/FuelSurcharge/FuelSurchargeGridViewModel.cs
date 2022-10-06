using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelSurchargeGridViewModel 
    {
        public int Id { get; set; }

        public string DateRange { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string TableName { get; set; }

        public string Customer { get; set; }

        public string Carrier { get; set; }

        public string SourceRegion { get; set; }

        public string Terminal { get; set; }

        public string BulkPlant { get; set; }

        public string IndexProduct { get; set; }

        public string IndexPeriod { get; set; }

        public string IndexArea { get; set; }

        public string IndexType { get; set; }

        public string ProductType { get; set; }

        public TableTypes TableType { get; set; }

        public string TableTypeNew { get; set; }

        public string StartValue { get; set; }

        public string EndValue { get; set; }

        public decimal PriceInterval { get; set; }

        public decimal SurchargePercentage { get; set; }

        public bool IsArchived { get; set; }

        public string StatusName { get; set; }
    }
}
