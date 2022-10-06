using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspGetGeneralNotesViewModel
    {
        public int Id { get; set; }
        public int LeadRequestId { get; set; }
        public string UserName { get; set; }
        public string GeneralNote { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
    }
}
