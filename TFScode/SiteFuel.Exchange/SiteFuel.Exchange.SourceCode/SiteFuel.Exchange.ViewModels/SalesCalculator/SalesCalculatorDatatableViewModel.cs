using Foolproof;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.FuelPricingDatail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class SalesCalculatorDatatableViewModel : DataTableAjaxPostModel
    {
        public int PricingSourceId { get; set; } = (int)PricingSource.Axxis;

        public int? FeedTypeId { get; set; }

        public DateTime PriceDate { get; set; }

        public List<int> CityTerminalIds { get; set; }

        public int? ProductId { get; set; }

        public int? BrandTypeId { get; set; }

        public int? PriceTypeId { get; set; }

        public bool IsCustomPricing { get; set; }

        public int CustomPricing { get; set; }

        public decimal Amount { get; set; }

        public int PricingCodeId { get; set; }

        public string PricingCode { get; set; }
    }
}
