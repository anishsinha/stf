using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceGridViewModel : StatusViewModel
    {
        public InvoiceGridViewModel()
        {
        }

        public InvoiceGridViewModel(Utilities.Status status) 
            : base(status)
        {
        }

        public string Id { get; set; }

        public string OrderId { get; set; }

        public string InvoiceNumber { get; set; }

        public int InvoiceNumberId { get; set; }

        public string PoNumber { get; set; }

        public string BolNumber { get; set; }

        public string Supplier { get; set; }

        public string Buyer { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int AssetCount { get; set; }

        public string OrderType { get; set; }

        public decimal Quantity { get; set; }

        public string PricePerGallon { get; set; }

        public string OrderTotal { get; set; }

        public string FuelDeliveredPercentage { get; set; }

        public string Status { get; set; }

        public string FuelType { get; set; }

        public decimal InvoiceAmount { get; set; }

        public string DropStartDate { get; set; }

        public string DropDate { get; set; }

        public string DropTime { get; set; }

        public string InvoiceDate { get; set; }

        public string PaymentDueDate { get; set; }

        public string TerminalName { get; set; }

        public string DriverName { get; set; }

        public string Location { get; set; }

        public string RackPPG { get; set; }

        public string JobName { get; set; }

        public int JobId { get; set; }

        public int TotalCount { get; set; }

        public string DroppedGallons { get; set; }

        public string TotalDroppedGallons { get; set; }

        public string TotalInvoiceAmount { get; set; }

        public string BrokeredChainId { get; set; }

        public int? ParentId { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public string LastEditDate { get; set; }

        public string QbInvoiceNumber { get; set; }

        public string SupplierAccountOwner { get; set; }

        public string BuyerAccountOwner { get; set; }

        public string PaymentTerms { get; set; }

        public int WaitingFor { get; set; }

        public string CreationMethod { get; set; }
        public string LiftTicketNumber { get; set; }
        public string DropTicketNumber { get; set; }
        public string PickupAddress { get; set; }
        public string BadgeNumber { get; set; }
        public string Carrier { get; set; }
        public string LiftDate { get; set; }

        public string PDIDetailsDate { get; set; }

        public string PrePostValues { get; set; }
        public string TotalNetQuantity { get; set; }
        public string TotalGrossQuantity { get; set; }
        public string HasAttachments { get; set; }

        public int InvoiceHeaderId { get; set; }
        public string ExternalPDIException { get; set; }
        public string ConvertedQuantity { get; set; }
        public bool IsMarineLocation { get; set; }
        public string UnitOfMeasurement { get; set; }

        public string PDIOrderId { get; set; }
        public string SourcingRequestId { get; set; }
        public string VesselName { get; set; }
        public string BDRNumber { get; set; }
        public string DeliveryLevelPO { get; set; }
        public string TimeToInvoice { get; set; }
        public string UniqueDrId { get; set; }
    }

    public class InvoiceBolEditGrid
    {
        public int InvoiceHeaderId { get; set; }
        public int InvoiceId { get; set; }
        public int InvoiceFtlDetailId { get; set; }
        public string Driver { get; set; }
        public string DisplayNumber { get; set; } // DDT# OR INVOICE #
        public string BolOrLiftNumber { get; set; }
        public decimal DroppedQty { get; set; }
        public decimal? ConvertedQty { get; set; }
        public decimal? GrossQty { get; set; }
        public decimal? NetQty { get; set; }
        public string DropDate { get; set; }
        public decimal? ApiGravity { get; set; }
        public string Density { get; set; }
        public string Temperature { get; set; }
        public string SulfurContent { get; set; }
        public string Viscosity { get; set; }
        public string FlashPoint { get; set; }
        public bool IsEdited { get; set; } //only for UI purpose
        public UoM UoM { get; set; }
        public string DeliveryLevelPO { get; set; }
        public int TrackableScheduleId { get; set; }
        public int TerminalId { get; set; }
        public EbolMatchStatus EbolMatchStatus { get; set; } = EbolMatchStatus.NoMatch;
    }

    public class InvoiceBDNEditFilterViewModel
    {
        public int InvoiceHeaderId { get; set; }

        public InvoiceType InvoiceType { get; set; }

        public string DisplayInvoiceNumber { get; set; }

        public int OrderId { get; set; }

        public UoM UoM { get; set; }

        public bool CanEdit { get; set; }
    }
}
