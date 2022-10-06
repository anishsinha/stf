using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class AccessorialFeeInvoiceInputViewModel
    {
        public AccessorialFeeInvoiceInputViewModel()
        {
            OrderIds = new List<int>();
        }

        public int SupplierId { get; set; }
        
        public int? CustomerId { get; set; }

        public int? TerminalId { get; set; }

        public int? BulkPlantId { get; set; }

        public List<int> OrderIds { get; set; }
    }
}
