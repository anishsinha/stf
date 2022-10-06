using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class NoDataExceptionDdtGridViewModel
    {
        public int InvoiceHeaderId { get; set; }
        public string DropTicketNumber { get; set; }

        public string PoNumber { get; set; }

        public string FuelType { get; set; }

        public string Customer { get; set; }

        public string LocationId { get; set; }

        public string Location { get; set; }

        public string Carrier { get; set; }

        public string Driver { get; set; }

        public string DropDate { get; set; }

        public string DropTime { get; set; }

        public decimal DroppedGallons { get; set; }

        public int WaitingFor { get; set; }

        public bool IsNoDataActionAllowed { get; set; }

        public bool IsThirdPartyOrder { get; set; }

        public int? NoDataExceptionApprovalId { get; set; }

        public string Status { get; set; }

        public int TotalCount { get; set; }

        public string Supplier { get; set; }

        public bool IsEndSupplier { get; set; }

        public string BrokeredChainId { get; set; }
    }
}
