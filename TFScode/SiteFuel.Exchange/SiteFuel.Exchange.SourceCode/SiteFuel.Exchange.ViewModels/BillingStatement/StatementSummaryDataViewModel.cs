using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class StatementSummaryDataViewModel : DataTableAjaxPostModel
    {
        public int CustomerId { get; set; }
        public string StatementId { get; set; }
        public Currency Currency { get; set; }
        public int CountryId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
