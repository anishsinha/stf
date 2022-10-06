using Newtonsoft.Json;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class EmailDocumentViewModel
    {
        public int OrderId { get; set; }

        public int InvoiceId { get; set; }

        public int InvoiceHeaderId { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string InvoiceIds { get; set; }

        public string InvoiceNumber { get; set; }

        public string PoNumber { get; set; }

        public string ToEmailAddress { get; set; }

        public string EmailBody { get; set; }

        [JsonIgnore]
        public List<int> SelectedInvoices { get; set; }

        public DocumentName DocumentName { get; set; }

        public CompanyType CompanyType { get; set; }

        public bool IncludeImagesInAttachment { get; set; }
    }
}
