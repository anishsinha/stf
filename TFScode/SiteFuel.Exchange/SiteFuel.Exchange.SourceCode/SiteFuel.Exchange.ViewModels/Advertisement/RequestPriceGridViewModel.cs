using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class RequestPriceGridViewModel
    {
        public int Id { get; set; }

        public string ZipCode { get; set; }

        public decimal Quantity { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string RequestDateTime { get; set; }

        public int TerminalId { get; set; }

        public string TerminalName { get; set; }

        public decimal PricePerGallon { get; set; }

        public string PricingDate { get; set; }
    }
}
