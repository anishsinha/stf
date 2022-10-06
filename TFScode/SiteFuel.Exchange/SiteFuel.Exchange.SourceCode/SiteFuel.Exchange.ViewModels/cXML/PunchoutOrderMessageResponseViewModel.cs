using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class PunchoutOrderMessageResponseViewModel
    {
        public string PunchoutCxml { get; set; }

        public DateTimeOffset? CxmlCheckOutDate { get; set; }
    }
}
