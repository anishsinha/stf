using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class BuyAndSellPricingDetailViewModel
    {
        public decimal BasePrice { get; set; }

        public decimal BuyPrice { get; set; }

        public decimal SellPrice { get; set; }

        public string BuyPriceDetail { get; set; }

        public string SellPriceDetail { get; set; }

        public bool IsBuyPriceInvoice { get; set; }

        public decimal BrokerMarkUp { get; set; }

        public decimal SupplierMarkUp { get; set; }

        public Currency Currency { get; set; }
        public string ExternalBrokerName { get; set; }
    }
}
