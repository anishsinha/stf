using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class GeoCodeViewModel : StatusViewModel
    {
        public GeoCodeViewModel()
        {
        }

        public GeoCodeViewModel(Status status)
            : base(status)
        {
        }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
