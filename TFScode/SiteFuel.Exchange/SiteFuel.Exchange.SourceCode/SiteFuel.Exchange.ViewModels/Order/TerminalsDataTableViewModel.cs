using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Core;
using System.ComponentModel.DataAnnotations;
using SiteFuel.Exchange.Core.StringResources;

namespace SiteFuel.Exchange.ViewModels
{
    public class TerminalsDataTableViewModel : DataTableAjaxPostModel
    {
        public int CountryId { get; set; }
    }
}
