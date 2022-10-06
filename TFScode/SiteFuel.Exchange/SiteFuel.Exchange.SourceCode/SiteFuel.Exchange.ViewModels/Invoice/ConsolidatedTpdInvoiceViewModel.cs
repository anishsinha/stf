using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class ConsolidatedTpdInvoiceViewModel
    {
        public int UniqueId { get; set; }

        public int JobId { get; set; }

        public List<ConsolidatedDropDetailsViewModel> DropDetailsViewModel { get; set; }
    }
}
