using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SubcontractorListViewModel
    {
        public int Id { get; set; }
        public string ContractNum { get; set; }
        public string Timezone { get; set; }
        public DateTimeOffset? AssignedDate { get; set; }
        public DateTimeOffset? RemovedDate { get; set; }
        public string SubName { get; set; }
        public int SubId { get; set; }
        public DateTimeOffset? AddedDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
    }
}
