using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class QuoteRequestFilterViewModel : BaseInputViewModel
    {
        public QuoteRequestFilterType filter { get; set; }
    }
}
