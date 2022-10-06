using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderDetails
    {
        public string OrderId { get; set; }
        public int JobId { get; set; }
        public string CustomerOrderNumber { get; set; }
        /// <summary>
        /// Fuel Detail ID (not fuel Type ID)
        /// </summary>
        public int FuelId { get; set; }
        public string FuelTypeName { get; set; }
        public string OrderName { get; set; }
        public string CustomerName { get; set; } // Username or company name? and buyer or seller?
        public decimal Quantity { get; set; }
        public int QuantityTypeId { get; set; }
        public decimal TotalOrderQuantity { get; set; }
        public decimal DroppedQuantity { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool WetHosing { get; set; }
        public bool OverWater { get; set; }
        public bool IsSplitTank { get; set; }
        public bool IsRentainFee { get; set; }
        public int AssetCount { get; set; }
        public bool IsAssetDropPicMandetory { get; set; }
        public bool IsFTL { get; set; }
        public bool IsDriverToUpdateBOL { get; set; }
        public bool IsDropImageRequired { get; set; }
        public bool IsBolImageRequired { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPerson { get; set; }
        public string SupplierName { get; set; }
        public bool IsExactMatch { get; set; }
        public bool IsDriverAssigned { get; set; }
        public bool IsDeliveryScheduleAdded { get; set; }
        public List<string> SpecialInstructionsToDriver { get; set; }
        public SpecialInstructionAttachmentViewModel SpecialInstruction { get; set; }
        public RunningMeterMode RunningMeterMode { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public long UtcStartDate { get; set; }
        public long UtcStartTime { get; set; }
        public long UtcEndTime { get; set; }
        public decimal InitalMeterReading { get; set; }
        public decimal FinalMeterReading { get; set; }
        public int? TrackableScheduleId { get; set; }
        public int DeliveryScheduleId { get; set; }
        public double Distance { get; set; }
        public int BuyerCompanyId { get; set; }
        public int OrderDeliveryType { get; set; }
        public bool IsAssetDropStatusEnabled { get; set; }
        public int UnitOfMeasurement { get; set; }
        public int Currency { get; set; }
        public bool CustomerSignatureRequired { get; set; }
        public int? ScheduleQuantityTypeId { get; set; }
        public string ScheduleQuantityTypeName { get; set; }
        public string SiteInstructions { get; set; }
        public bool IsPrePostDipEnabled { get; set; }
        public int OrderUoM { get; set; }
    }

    public class AssetDropDetails
    {
        public int AssetId { get; set; }
        public int OrderId { get; set; }
        public int InvoiceId { get; set; }
        public string AssetName { get; set; }
        public string AssetType { get; set; }
        public string VehicalId { get; set; }
        public string LicensePlateState { get; set; }
        public string LicensePlate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public int AssetDropId { get; set; }
        public string FuelType { get; set; }
        public decimal FuelCapacity { get; set; }
        public byte[] AssetImage { get; set; }
        public decimal primaryGallonsDropped { get; set; }
        public decimal PrimaryMeterStartReading { get; set; }
        public decimal PrimaryMeterEndReading { get; set; }
        public decimal secondaryGallonsDropped { get; set; }
        public decimal SecondaryMeterStartReading { get; set; }
        public decimal SecondaryMeterReading { get; set; }
        public bool isNoFuelNeeded { get; set; }
        public bool SpillOccurred { get; set; }
        public int SpillId { get; set; }
        public int JobXAssignmentId { get; set; }
        public int AssetImageId { get; set; }
        public decimal DroppedGallons { get; set; }
        public int DropStatus { get; set; }
        public List<AssetDropResponseViewModel> AssetDropDetail { get; set; }
    }

    public class NewAssetDetails : UserResponseModel
    {
        public int JobDetailId { get; set; }
        public string Name { get; set; }
        public int? FuelType { get; set; }
        public string CatClass { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public Nullable<decimal> FuelCapacity { get; set; }
        public string TelematicsProvider { get; set; }
        public string AdditionalDetails { get; set; }
        public string VehicleId { get; set; }
        public string LicensePlateState { get; set; }
        public string LicensePlate { get; set; }
        public string Vendor { get; set; }
        public long AssetAddedDateTime { get; set; }
        public string Image { get; set; }
    }

    public class AssetDropHistory
    {
        public int AssetId { get; set; }
        public byte[] Image { get; set; }
        public string Type { get; set; }
        public List<DropHistory> DropList { get; set; }
    }

    public class DropHistory
    {
        public double DropDateTime { get; set; }
        public string DropDate { get; set; }
        public string DropTime { get; set; }
        public decimal DroppedGallons { get; set; }
        public string TimeZoneName { get; set; }
    }

    public class AssetFuelTypeDetails
    {
        public string FuelTypeName { get; set; }
        public int FuelTypeId { get; set; }
    }

    public class StateDetails
    {
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public int StateId { get; set; }
    }

    public class SpillFuelDetails
    {
        public SpillFuelDetails()
        {
            ImageList = new List<AppImageViewModel>();
        }

        public int AssetId { get; set; }
        public int OrderId { get; set; }
        public string Notes { get; set; }
        public long SpillTime { get; set; }
        public int SpillId { get; set; }
        public int UserId { get; set; }
        public int CompanyID { get; set; }
        public int InvoiceId { get; set; }
        public int AssetFuelDropId { get; set; }
        public List<AppImageViewModel> ImageList { get; set; }
    }

    public class UpdateSpillFuelDetails
    {
        public UpdateSpillFuelDetails()
        {
            ImageList = new List<AppImageViewModel>();
        }

        public int SpillId { get; set; }
        public string Notes { get; set; }
        public long SpillTime { get; set; }
        public List<AppImageViewModel> ImageList { get; set; }
    }
    public class AppImageViewModel
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
    }

    public class FilledAssetDetails
    {
        public int AssetId { get; set; }
        public byte[] AssetImage { get; set; }
        public decimal primaryGallonsDropped { get; set; }
        public decimal PrimaryMeterStartReading { get; set; }
        public decimal PrimaryMeterEndReading { get; set; }
        public int primaryAssetDropId { get; set; }
        public decimal secondaryGallonsDropped { get; set; }
        public decimal SecondaryMeterStartReading { get; set; }
        public decimal SecondaryMeterReading { get; set; }
        public int secondaryAssetDropId { get; set; }
        public bool isNoFuelNeeded { get; set; }
        public bool SpillOccurred { get; set; }
        public int spillID { get; set; }
        public decimal DroppedGallons { get; set; }
    }
}
