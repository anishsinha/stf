using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Offer
{
	public class OfferSummaryViewModel : BaseInputViewModel
    {
		public List<string> Customers { get; set; } = new List<string>();

		public List<OfferViewModel> OfferList { get; set; } = new List<OfferViewModel>();
	}
}
