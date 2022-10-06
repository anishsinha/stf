using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Offer
{
    public class QuickUpdatedItemDataTableViewModel : DataTableAjaxPostModel
    {
        public QuickUpdatedItemDataTableViewModel()
        {
            Currency = Currency.USD;
            CountryId = (int)Country.USA;
        }
        public Currency Currency { get; set; }
        public int CountryId { get; set; }
        public int CommandId { get; set; }
    }
}
