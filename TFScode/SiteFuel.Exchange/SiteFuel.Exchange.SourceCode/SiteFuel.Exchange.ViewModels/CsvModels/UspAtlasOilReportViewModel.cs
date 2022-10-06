using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspAtlasOilReportViewModel
    {
        public UspAtlasOilReportViewModel()
        {
        }

        public int OrderId { get; set; }

        public int InvoiceId { get; set; }

        public string InvoiceNumber { get; set; }

        public string Source { get; set; }

        public string Driver { get; set; }

        public string Account { get; set; }

        public string Product { get; set; }

        public decimal GrossVolume { get; set; }

        public decimal NetVolume { get; set; }

        public string UnitNumber { get; set; }

        public string DeliveryStart { get; set; }

        public string DeliveryEnd { get; set; }

        public decimal StartingTotalizer { get; set; }

        public decimal EndingTotalizer { get; set; }

        public decimal Lat { get; set; }

        public decimal Lon { get; set; }

        public decimal Price { get; set; }

        public string PONumber { get; set; }

        public string WarehouseID { get; set; }
    }
}
