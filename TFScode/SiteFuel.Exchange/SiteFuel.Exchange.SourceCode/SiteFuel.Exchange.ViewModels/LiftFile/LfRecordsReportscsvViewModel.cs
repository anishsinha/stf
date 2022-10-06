using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    [DelimitedRecord(",")]
    public class LfRecordsReportscsvViewModel
    {
        [FieldOrder(0), FieldQuoted]
        public string FileName { get; set; }

        [FieldOrder(1), FieldQuoted]
        public string CallID { get; set; }

        [FieldOrder(2), FieldQuoted]
        public string Bol { get; set; }

        [FieldOrder(3), FieldQuoted]
        public string Terminal { get; set; }

        [FieldOrder(4), FieldQuoted]
        public string CorrectedQuantity { get; set; }

        [FieldOrder(5), FieldQuoted]
        public string TerminalItemCode { get; set; }

        [FieldOrder(6), FieldQuoted]
        public string ProductType { get; set; }

        [FieldOrder(7), FieldQuoted]
        public string LoadDate { get; set; }

        [FieldOrder(8), FieldQuoted]
        public string RecordDate { get; set; }

        [FieldOrder(9), FieldQuoted]
        public string CarrierID { get; set; }

        [FieldOrder(10), FieldQuoted]
        public string CarrierName { get; set; }

        [FieldOrder(11), FieldQuoted]
        public string Status { get; set; }

        [FieldOrder(12), FieldQuoted]
        public string Reason { get; set; }

        [FieldOrder(13), FieldQuoted]
        public string Username { get; set; }

        [FieldOrder(14), FieldQuoted]
        public string ModifiedDate { get; set; }
        [FieldOrder(15), FieldQuoted]
        public string ReasonCode { get; set; }
        [FieldOrder(16), FieldQuoted]
        public string ReasonCategory { get; set; }

        [FieldOrder(17), FieldQuoted]
        public string ResolutionTime { get; set; }

        [FieldOrder(18), FieldQuoted]
        public string TimeToBOL { get; set; }



    }
    public class LfRecordsCarrierReportsViewModel
    {

        public string FileName { get; set; }


        public string CallID { get; set; }


        public string Bol { get; set; }


        public string Terminal { get; set; }

        public string CorrectedQuantity { get; set; }


        public string TerminalItemCode { get; set; }


        public string ProductType { get; set; }

        public string LoadDate { get; set; }


        public string RecordDate { get; set; }


        public string CarrierID { get; set; }


        public string CarrierName { get; set; }


        public string Status { get; set; }


        public string Reason { get; set; }
        public int CompanyId { get; set; }
        public List<string> EmailList { get; set; } = new List<string>();
        public string statusChangeDate { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonCategory { get; set; }
        public string Username { get; set; }
        public string ModifiedDate { get; set; }
        public string LFVCarrierPerModifiedDate { get; set; }
        public string IgnoredReason { get; set; }
        public string ForcedIgnoreReason { get; set; }
        public string LFVResolutionTime { get; set; }

    }
}

