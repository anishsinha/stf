using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class QuotationsGridViewModel
    {
        public QuotationsGridViewModel()
        {
            Documents = new List<DocumentViewModel>();
        }

        public int Id { get; set; }

        public int CustomerQuoteRequestId { get; set; }

        public string CustomerQuoteNumber { get; set; }

        public string QuoteNumber { get; set; }

        public string SupplierName { get; set; }

        public string RackPPG { get; set; }

        public string DeliveryFee { get; set; }

        public string OtherFees { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedDate { get; set; }

        public string Status { get; set; }

        public bool IsExcluded { get; set; }

        public int QuotationStatusId { get; set; }

        public int QuoteRequestStatusId { get; set; }
        
        public List<DocumentViewModel> Documents { get; set; }

        public int Priority { get; set; }
    }
}
