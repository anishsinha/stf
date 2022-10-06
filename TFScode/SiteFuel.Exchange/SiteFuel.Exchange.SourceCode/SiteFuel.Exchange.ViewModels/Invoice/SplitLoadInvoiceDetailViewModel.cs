using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class SplitLoadInvoiceDetailViewModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int? Sequence { get; set; }
        public string ChainId { get; set; }
        public int InvoiceTypeId { get; set; }
    }
}
