using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspAtlasOilCarrierReportViewModel
    {
        public UspAtlasOilCarrierReportViewModel()
        {
        }

        public int OrderId { get; set; }

        public int InvoiceId { get; set; }

        public string InvoiceNumber { get; set; }

        public string Source { get; set; }

        public string Product { get; set; }

        public string StateCode { get; set; }

        public decimal GrossVolume { get; set; }

        public decimal NetVolume { get; set; }

        public string LoadingStartDate { get; set; }

        public string LoadingStartTime { get; set; }

        public string LoadingEndDate { get; set; }

        public string LoadingEndTime { get; set; }

        public string CarrierName { get; set; }
    }
}
