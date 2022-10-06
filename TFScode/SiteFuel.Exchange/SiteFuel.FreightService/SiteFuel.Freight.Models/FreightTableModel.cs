using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class FreightTableModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TableType Type { get; set; }
        public int FuelType { get; set; }
        public int CompanyId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
    }
}
