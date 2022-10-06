using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Invoice.Pdf
{
    public class InvoicePdfFooterViewModel
    {
        public List<PdfFooterModel> InvoicePdfFooterList { get; set; }
    }

    public class PdfFooterModel
    {
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string Description { get; set; }

        public string BankingInstructions { get; set; }

        public string AdditionalDetails { get; set; }

        public string QRCodePath { get; set; }
    }
}
