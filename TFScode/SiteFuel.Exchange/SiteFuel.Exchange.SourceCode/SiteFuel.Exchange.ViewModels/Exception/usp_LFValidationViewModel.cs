using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class usp_LFValidationViewModel
    {
        public int CallId { get; set; }

        public int RecordStatus { get; set; }

        public string RecordDate { get; set; }

        public string RecordCleanDate { get; set; }
        public string CarrierName { get; set; }
        public string CarrierID { get; set; }
    }
}
