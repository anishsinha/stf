using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class DecimalResponseModel : StatusViewModel
    {
        public DecimalResponseModel()
        {
        }

        public DecimalResponseModel(Status status) : base(status)
        {
        }

        public decimal Result { get; set; }
    }
}
