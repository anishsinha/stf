using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Invoice.Pdf
{
    public class UspInvoicePdfAssetDrop
    {
        public string AssetName { get; set; }
        public string VehicleId { get; set; }
        public int JobXAssetId { get; set; }
        public DateTimeOffset DropStartDate { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public decimal DroppedGallons { get; set; }
        public decimal PricePerGallon { get; set; }
        public string SubcontractorName { get; set; }
        public int DropStatus { get; set; }
        public bool IsNewAsset { get; set; }
        public int OrderId { get; set; }
        public int InvoiceId { get; set; }
        public string SpillNotes { get; set; }
        public decimal? PreDip { get; set; }
        public decimal? PostDip { get; set; }
        public int TankScaleMeasurement { get; set; }
        public string IMONumber { get; set; }
        public string DeliveryLevelPO { get; set; }
    }
}
