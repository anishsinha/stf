using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class NoDataExceptionDataTableViewModel : DataTableAjaxPostModel
    {

        public Currency Currency { get; set; }

        public int CountryId { get; set; }

        public string GroupIds { get; set; }

        public int OrderId { get; set; }

    }
}
