using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class SupplierAssetDropHistoryViewModel : StatusViewModel
    {
        public SupplierAssetDropHistoryViewModel()
        {
        }

        public SupplierAssetDropHistoryViewModel(Status status)
            : base(status)
        {
        }

        public string CustomerName { get; set; }
        public string PONumber { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public int InvoiceTypeId { get; set; }
        public string AssetName { get; set; }
        public string AssetId { get; set; } // we treat this as Vehical Id
        public string AssetContractNumber { get; set; }
        public string FuelTypeName { get; set; }
        public string DropDate { get; set; }
        public string DropStartTime { get; set; }
        public string DropEndTime { get; set; }
        public decimal GallonsDelivered { get; set; }
        public string PricingFormat { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCostForAsset { get; set; }
        public int Currency { get; set; }
    }
}
