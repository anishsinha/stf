
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class AuditViewModel
    {
        public int InvoiceId { get; set; }

        public int OrderId { get; set; }

        public DateTimeOffset DropStartDate { get; set; }

        public string DropDate { get; set; }

        public string DropTime { get; set; }

        public decimal Quantity { get; set; }

        public string ProductType { get; set; }

        public string TerminalName { get; set; }

        public int PricingTypeId  { get; set; }
        
        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string PPG { get; set; }

        public decimal InvoicePPG { get; set; }

        public int ExternalProductId { get; set; }

        public int TotalCount { get; set; }

        public string InvoiceNumber { get; set; }

        public string OrderNumber { get; set; }
    }

    public class NearestTerminalsViewModel
    {
        public string TerminalName { get; set; }

        public double Distance { get; set; }

        public decimal TerminalPPG { get; set; }
    }

    public class AuditReportViewModel
    {
        public AuditReportViewModel() {
            NearestTerminals = new List<NearestTerminalsViewModel>();
        }
        public AuditViewModel DropDetail { get; set; }

        public List<NearestTerminalsViewModel> NearestTerminals { get; set; }
    }
}
