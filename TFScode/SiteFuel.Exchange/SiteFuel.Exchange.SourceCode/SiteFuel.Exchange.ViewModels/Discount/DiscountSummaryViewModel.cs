using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DiscountSummaryViewModel
    {
        public DiscountSummaryViewModel()
        {
            DiscountLineItem = new List<DiscountLineItemViewModel>();
        }

        public string DealName { get; set; }

        public string CreatedBy { get; set; }

        public string FeeDetails { get; set; }
        
        public string DealStatus { get; set; }

        public string Notes { get; set; }

        public int CreatedCompanyId { get; set; }

        public int DiscountId { get; set; }

        public int InvoiceHeaderId { get; set; }

        public int DealStatusId { get; set; }

        public List<DiscountLineItemViewModel> DiscountLineItem { get; set; }
    }
}
