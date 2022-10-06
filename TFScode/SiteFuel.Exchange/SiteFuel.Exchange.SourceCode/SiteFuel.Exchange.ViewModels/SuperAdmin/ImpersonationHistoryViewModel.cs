using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public partial class ImpersonationHistoryViewModel
    {
        public int Id { get; set; }
        public string ImpersonatedUser { get; set; }
        public string ImpersonatedBy { get; set; }
        public string ImpersonatedStartTime { get; set; }
        public string ImpersonatedEndTime { get; set; }
        public string TerminatedBy { get; set; }
    }
}
