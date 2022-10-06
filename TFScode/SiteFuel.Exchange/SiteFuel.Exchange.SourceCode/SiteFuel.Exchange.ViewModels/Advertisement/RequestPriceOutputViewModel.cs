using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class RequestPriceOutputViewModel : ResponseViewModel
    {
        public int Id { get; set; }

        public decimal PricePerGallon { get; set; }
    }
}
