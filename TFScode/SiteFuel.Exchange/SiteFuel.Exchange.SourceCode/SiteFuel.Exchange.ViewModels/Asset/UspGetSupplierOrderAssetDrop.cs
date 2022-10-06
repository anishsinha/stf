using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Asset
{
    public class UspGetSupplierOrderAssetDrop
    {
        public string Customer { get; set; }
        public string PoNumber { get; set; }
        public string ContractNumber { get; set; }
        public string VehicleId { get; set; }
        public string AssetName { get; set; }
        public DateTimeOffset DropStartDate { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public decimal DroppedGallons { get; set; }
        public int InvoiceId { get; set; }
        public decimal PricePerGallon { get; set; }
        public decimal RackPrice { get; set; }
        public int PricingTypeId { get; set; }
        public string DisplayPrice { get; set; }
        public string InvoiceNumber { get; set; }
        public int InvoiceTypeId { get; set; }
        public int Currency { get; set; }
    }
}
