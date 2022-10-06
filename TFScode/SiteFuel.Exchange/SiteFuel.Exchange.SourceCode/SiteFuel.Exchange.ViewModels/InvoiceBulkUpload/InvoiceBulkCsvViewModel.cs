using FileHelpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SiteFuel.Exchange.ViewModels
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class InvoiceBulkCsvViewModel
    {
        [FieldQuoted]
        public string PONumber { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string DriverFirstName { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string DriverLastName { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string TruckNumber { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string BolNumber { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string BolCreationTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string BolNet { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string BolGross { get; set; }
     
        [FieldQuoted]
        [FieldOptional]
        public string BolCarrier { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftTicketNumber { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftQuantity { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftDate { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftArrivalTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftStartTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftEndTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftTicketCreationTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftAddressStreet1 { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftAddressStreet2 { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftAddressCity { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftAddressState { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftAddressZip { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftAddressLat { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftAddressLong { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string TerminalControlNumber { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string FuelCost { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string OnWayDrop1Time { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string OnWayDrop1Lat { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string OnWayDrop1Long { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1ArrivalDate { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1ArrivalTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1AddressStreet1 { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1AddressStreet2 { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1AddressCity { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1AddressState { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1AddressZip { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1AddressLat { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1AddressLong { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1CompleteTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1Notes { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1DemurrageTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1DemurrageFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1WethoseFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1FreightFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1SurchargeFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1DryRunCount { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1DryRunFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1LoadFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1EnvironmentalFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1ServiceFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1OverWaterFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1OtherFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1AssetId { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1Quantity { get; set; }



        [FieldQuoted]
        [FieldOptional]
        public string Drop1TicketNumber { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string OnWayDrop2Time { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string OnWayDrop2Lat { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string OnWayDrop2Long { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2ArrivalDate { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2ArrivalTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2AddressStreet1 { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2AddressStreet2 { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2AddressCity { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2AddressState { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2AddressZip { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2AddressLat { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2AddressLong { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2CompleteTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2Notes { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2DemurrageTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2DemurrageFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2WethoseFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2FreightFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2SurchargeFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2DryRunCount { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2DryRunFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2LoadFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2EnvironmentalFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2ServiceFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2OverWaterFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2OtherFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2AssetId { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2Quantity { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2TicketNumber { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string OnWayDrop3Time { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string OnWayDrop3Lat { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string OnWayDrop3Long { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3ArrivalDate { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3ArrivalTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3AddressStreet1 { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3AddressStreet2 { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3AddressCity { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3AddressState { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3AddressZip { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3AddressLat { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3AddressLong { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3CompleteTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3Notes { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3DemurrageTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3DemurrageFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3WethoseFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3FreightFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3SurchargeFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3DryRunCount { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3DryRunFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3LoadFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3EnvironmentalFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3ServiceFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3OverWaterFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3OtherFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3AssetId { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3Quantity { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3TicketNumber { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string OnWayDrop4Time { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string OnWayDrop4Lat { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string OnWayDrop4Long { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4ArrivalDate { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4ArrivalTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4AddressStreet1 { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4AddressStreet2 { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4AddressCity { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4AddressState { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4AddressZip { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4AddressLat { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4AddressLong { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4CompleteTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4Notes { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4DemurrageTime { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4DemurrageFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4WethoseFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4FreightFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4SurchargeFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4DryRunCount { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4DryRunFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4LoadFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4EnvironmentalFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4ServiceFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4OverWaterFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4OtherFees { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4AssetId { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4Quantity { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4TicketNumber { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string SupplierInvoiceNumber { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1AssetPreDip { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1AssetPostDip { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2AssetPreDip { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2AssetPostDip { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3AssetPreDip { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3AssetPostDip { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4AssetPreDip { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4AssetPostDip { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string CarrierOrderId { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LocationId { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1ProductId { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2ProductId { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3ProductId { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4ProductId { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string CustomerID { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LoadingBadge { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Tractor { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string OrderDate { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string OrderQuantity { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftNet { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftGross { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop1ApiGravity { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop2ApiGravity { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop3ApiGravity { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Drop4ApiGravity { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string BulkPlantName { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string BolDelivered { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string LiftDelivered { get; set; }

    }

    public class InvoiceBulkViewModel
    {
        public InvoiceBulkCsvViewModel CsvViewModel { get; set; } = new InvoiceBulkCsvViewModel();
        public List<AssetDropViewModel> AssetDropList { get; set; } = new List<AssetDropViewModel>();
        public bool IsSplitLoadInvoice { get; set; }
        public bool IsDryRunInvoice { get; set; }
    }

    public class FileModel
    {
        [Required(ErrorMessage = "Please select file.")]
        [Display(Name = "Browse File")]
        public List<HttpPostedFileBase> InvoiceFiles { get; set; } = new List<HttpPostedFileBase>();

    }

}
