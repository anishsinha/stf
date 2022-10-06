using SiteFuel.Exchange.Utilities;
using System;


namespace SiteFuel.Exchange.ViewModels
{
    public class FuelPriceRequestModel
    {
        public int? TerminalId { get; set; }
        public int ProductId { get; set; }
        public DateTime PriceDate { get; set; }
        public int RequestPriceDetailId { get; set; }
        public int? CityGroupTerminalId { get; set; }
        public decimal? SupplierCost { get; set; }
        public Guid Guid { get; set; }
        public WaitingAction WaitingFor { get; set; } = WaitingAction.Nothing;
        public decimal DroppedQuantity { get; set; }
        public bool CanForceTerminalForTierPricing { get; set; }
        public decimal TierMinQuantity { get; set; }
        public decimal TierMaxQuantity { get; set; }
        public UoM UoM { get; set; }
        public decimal? PricePerGallonToOverride { get; set; }
    }
}
