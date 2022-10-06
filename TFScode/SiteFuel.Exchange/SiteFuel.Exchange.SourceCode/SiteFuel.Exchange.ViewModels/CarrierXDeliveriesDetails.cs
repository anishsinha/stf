using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class CarrierXDeliveriesDetails
    {
        public int CarrierCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int JobId { get; set; }
        public decimal DroppedGallons { get; set; }
        public decimal DSQuantity { get; set; }
        public int InvoiceId { get; set; }
        public int TrackableSCId { get; set; }
        public string SupplierCompanyName { get; set; }
        public string CarrierCompanyName { get; set; }
        public string LocationName { get; set; }
        public string ReportDate { get; set; }
    }
    public class CarrierXDeliveriesEmailModel
    {
        public int SupplierCompanyId { get; set; }
        public List<int> CarrierCompanyId { get; set; } = new List<int>();
        public List<string> CarrierXUserEmails { get; set; } = new List<string>();
        public int TotalDS { get; set; }
        public decimal TotalDSDropQty { get; set; }
    }
    public class TerminalMappingDetails
    {
        public string TerminalId { get; set; }
        public string TerminalORBulkPlanName { get; set; }
    }
    public class CarrierMappingDetails
    {
        public string TerminalId { get; set; }
        public string CarrierId { get; set; }
        public string CarrierName { get; set; }
    }
}
