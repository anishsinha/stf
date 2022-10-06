using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class EBolAPIResponseModel
    {
        public string BOLNumber { get; set; }
        public bool IsBOLNumberMatch { get; set; }
        public List<EBolViewModel> Details { get; set; } = new List<EBolViewModel>();
    }

    public class EBolViewModel
    {
        public string TerminalName { get; set; }
        public int TerminalId { get; set; }
        public string BOLNumber { get; set; }
        public Decimal GrossGallons { get; set; }
        public decimal NetGallons { get; set; }
    }
    public class EBolAPIRequestModel
    {
        public List<EBolAPIRequestDetails> eBOLApiRequestList { get; set; }   
    }
    public class EBolAPIRequestDetails
    {
        public DateTime InvoiceDate { get; set; }
        public int FuelTypeId { get; set; }
        public string BOLNumber { get; set; }
        public int TerminalId { get; set; }
    }

}
