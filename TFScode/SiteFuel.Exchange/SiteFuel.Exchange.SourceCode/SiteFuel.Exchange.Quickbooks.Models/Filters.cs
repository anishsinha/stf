using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Quickbooks.SharedEnums;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public class NameFilter
    {
        public NameFilter()
        {
            MatchCriterion = MatchCriterion.StartsWith;
        }

        public MatchCriterion MatchCriterion { get; set; }

        public string Name { get; set; }
    }

    public class ModifiedDateRangeFilter
    {
        public string FromModifiedDate { get; set; }

        public string ToModifiedDate { get; set; }
    }

    public class TxnDateRangeFilter
    {
        public string FromTxnDate { get; set; }

        public string ToTxnDate { get; set; }
    }

    public class CommonFilter
    {
        public string ListID { get; set; }

        public string FullName { get; set; }

        public string ListIDWithChildren { get; set; }

        public string FullNameWithChildren { get; set; }
    }

    public class CompanySyncDateRangeFilter
    {
        public int CompanyId { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }
    }
}
