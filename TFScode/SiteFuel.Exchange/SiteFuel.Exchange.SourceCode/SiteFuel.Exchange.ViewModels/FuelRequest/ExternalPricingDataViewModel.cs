using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class ExternalPricingDataViewModel : StatusViewModel
    {
        public ExternalPricingDataViewModel()
        {
        }

        public ExternalPricingDataViewModel(Status status)
            : base(status)
        { 
        }

        public int TerminalId { get; set; }

        public int ProductId { get; set; }

        public int ExternalProductId { get; set; }

        public decimal AvgPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal HighPrice { get; set; }

        public string TerminalCity { get; set; }
        public string TerminalZipCode { get; set; }
        public string TerminalState { get; set; }
        public string TerminalCounty { get; set; }

        public DateTimeOffset TerminalPricingDate { get; set; }

        public decimal TerminalPrice { get; set; }

        public Currency Currency { get; set; }

        public DateTimeOffset PriceLastUpdatedDate { get; set; }
    }
}
