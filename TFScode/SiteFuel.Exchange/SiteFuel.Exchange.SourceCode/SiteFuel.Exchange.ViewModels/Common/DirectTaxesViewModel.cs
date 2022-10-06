using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class DirectTaxesViewModel
    {
        public List<int> StateIds { get; set; } = new List<int>();

        public int CountryId { get; set; } 

        public List<CountryState> StateList { get; set; } = new List<CountryState>();
    }
}
