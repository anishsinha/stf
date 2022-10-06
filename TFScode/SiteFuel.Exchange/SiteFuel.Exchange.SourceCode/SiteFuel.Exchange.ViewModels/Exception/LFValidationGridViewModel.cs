using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public  class LFValidationGridViewModel
    {
        public int CallId { get; set; } // Refers LF DETAILS table Id

        public int TotalRecordCount { get; set; }

        public int MatchedRecordCount { get; set; }

        public int ActiveExceptionRecordCount { get; set; }

        public int NoMatchRecordCount { get; set; }

        public string RecordDate { get  ; set; } //REFERES AddedDate column in LF DETAILS TABLE

        public int IgnoredMatchRecordCount { get; set; }

        public int UnmatchedRecordCount { get; set; }

        public string UpdatedOn { get; set; }

        public int PendingMatchCount { get; set; }

        public int DuplicateRecordCount { get; set; }
        public int PartialMatchRecordCount { get; set; }
        public string CarrierName { get; set; }
        public string CarrierID { get; set; }
        public int ForcedIgnoredMatchRecordCount { get; set; }
    }
}
