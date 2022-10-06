using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class LoadOptimizationUserViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public List<int> DistributedUsers { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    }
}
