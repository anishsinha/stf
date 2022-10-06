using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class CompanyExternalIdGridViewModel
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string ExternalId { get; set; }

        public string AddedBy { get; set; }

        public string AddedDate { get; set; }

        public string UpdatedBy { get; set; }

        public string UpdatedDate { get; set; }
    }
}
