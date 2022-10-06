using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class StatementSummaryFilterViewModel : BaseInputViewModel
    {
        public StatementSummaryFilterViewModel()
        {
            Currency = Currency.USD;
            CountryId = (int)Utilities.Country.USA;
        }
        public int CustomerId { get; set; }
        public string StatementId { get; set; }
        public Currency Currency { get; set; }
        public int CountryId { get; set; }
    }
}
