using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceFilterViewModel : BaseInputViewModel
    {
        public InvoiceFilterViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Filter = InvoiceFilterType.All;
        }

        public int JobId { get; set; }

        public InvoiceFilterType Filter { get; set; }

        public int AllowedInvoiceType { get; set; }

        public int OrderId { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public int CountryId { get; set; }

        public string PoNumber { get; set; }

        public int MaxInvoiceAttachmentsPerEmail { get; set; } = 5;

        public int MaxInvoiceCountPerSession { get; set; } = 30;

        public EmailDocumentViewModel InvoiceAttachment { get; set; } = new EmailDocumentViewModel();
        public int CarrierCompanyId { get; set; }
        public string ReportDate { get; set; }
        public List<int> CustomerIds { get; set; } = new List<int>();
        public List<int> LocationIds { get; set; } = new List<int>();
        public List<int> VesselIds { get; set; } = new List<int>();
        public OrderGridFilterDataViewModel InputFilterDataViewModel { get; set; } = new OrderGridFilterDataViewModel();
    }
}
