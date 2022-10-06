using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class QuickUpdateFilterViewModel
    {
        public int OfferTypeId { get; set; }
        public int FuelTypeId { get; set; }
        public int CityId { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string Customers { get; set; }
        public string Tiers { get; set; }
        public string Zips { get; set; }
        public int QuickUpdateType { get; set; }
        public int FeeTypeId { get; set; }

        public Country Country { get; set; } = Country.USA;
        public Currency CurrencyType { get; set; } = Currency.USD;

        public TruckLoadTypes TruckLoadType { get; set; } = TruckLoadTypes.LessTruckLoad;
        public PricingSource PricingSource { get; set; } = PricingSource.Axxis;
    }
}
