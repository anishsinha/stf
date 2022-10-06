using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public partial class ImpersonationActivityLogViewModel
    {
        public int Id { get; set; }

        public string TimeStamp { get; set; }

        public string ImpersonatedUser { get; set; }

        public string ImpersonatedByUser { get; set; }

        public string Description { get; set; }

        public string Data { get; set; }

        public int TotalCount { get; set; }
    }
}
