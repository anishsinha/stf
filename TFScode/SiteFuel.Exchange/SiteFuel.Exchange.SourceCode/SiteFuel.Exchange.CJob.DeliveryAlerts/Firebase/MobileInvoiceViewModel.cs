using Google.Cloud.Firestore;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.DeliveryAlerts.Firebase
{
    [FirestoreData]
    public class MobileInvoiceViewModel
    {
        [FirestoreProperty]
        public MobileCustomerViewModel Customer { get; set; }
        [FirestoreProperty]
        public List<MobileInvoiceBolViewModel> BolDetails { get; set; } = new List<MobileInvoiceBolViewModel>();
        [FirestoreProperty]
        public List<MobileInvoiceLiftTicketViewModel> TicketDetails { get; set; } = new List<MobileInvoiceLiftTicketViewModel>();
        [FirestoreProperty]
        public List<MobileInvoiceDropViewModel> Drops { get; set; } = new List<MobileInvoiceDropViewModel>();
        [FirestoreProperty]
        public int DriverId { get; set; }
        [FirestoreProperty]
        public string Carrier { get; set; }
        [FirestoreProperty]
        public MobileImageViewModel InvoiceImage { get; set; }
        [FirestoreProperty]
        public MobileImageViewModel SignatureImage { get; set; }
        [FirestoreProperty]
        public MobileImageViewModel AdditionalImage { get; set; }
        [FirestoreProperty]
        public MobileImageViewModel InspectionVoucherImage { get; set; }
        [FirestoreProperty]
        public MobileDropAddressViewModel FuelDropLocation { get; set; }
        [FirestoreProperty]
        public int InvoiceStatusId { get; set; }
        [FirestoreProperty]
        public List<DemurrageDetail> DemurrageDetails { get; set; } = new List<DemurrageDetail>();
        [FirestoreProperty]
        public Dictionary<string, bool> SpecialInstructions { get; set; } = new Dictionary<string, bool>();
        [FirestoreProperty]
        public int DropStatus { get; set; }
        [FirestoreProperty]
        public string TraceId { get; set; }
    }

    [FirestoreData]
    public class MobileCustomerViewModel
    {
        [FirestoreProperty]
        public int CompanyId { get; set; }
        [FirestoreProperty]
        public string CompanyName { get; set; }
        [FirestoreProperty]
        public MobileJobLocationViewModel Location { get; set; }
        [FirestoreProperty]
        public string ContactName { get; set; }
        [FirestoreProperty]
        public string ContactEmail { get; set; }
        [FirestoreProperty]
        public string ContactPhone { get; set; }
    }

    [FirestoreData]
    public class MobileJobLocationViewModel
    {
        [FirestoreProperty]
        public int JobId { get; set; }
        [FirestoreProperty]
        public string SiteName { get; set; }
        [FirestoreProperty]
        public string Address { get; set; }
        [FirestoreProperty]
        public string City { get; set; }
        [FirestoreProperty]
        public string StateCode { get; set; }
        [FirestoreProperty]
        public string ZipCode { get; set; }
    }

    [FirestoreData]
    public class MobileInvoiceBolViewModel
    {
        [FirestoreProperty]
        public int Id { get; set; }
        [FirestoreProperty]
        public string BolNumber { get; set; }
        [FirestoreProperty]
        public long LiftDate { get; set; }
        [FirestoreProperty]
        public string BadgeNumber { get; set; }
        [FirestoreProperty]
        public List<MobileBolProductViewModel> Products { get; set; }
        [FirestoreProperty]
        public MobileImageViewModel Images { get; set; }
        [FirestoreProperty]
        public long? LiftStartTime { get; set; }
        [FirestoreProperty]
        public long? LiftEndTime { get; set; }
    }

    [FirestoreData]
    public class MobileBolProductViewModel
    {
        [FirestoreProperty]
        public int ProductId { get; set; }
        [FirestoreProperty]
        public string ProductName { get; set; }
        [FirestoreProperty]
        public double? NetQuantity { get; set; }
        [FirestoreProperty]
        public double? GrossQuantity { get; set; }
        [FirestoreProperty]
        public double? DeliveredQuantity { get; set; }
        [FirestoreProperty]
        public int? TerminalId { get; set; }
        [FirestoreProperty]
        public string TerminalName { get; set; }
        [FirestoreProperty]
        public List<MobileCompartmentInfoViewModel> CompartmentInfo { get; set; } = new List<MobileCompartmentInfoViewModel>();
    }

    [FirestoreData]
    public class MobileCompartmentInfoViewModel
    {
        [FirestoreProperty]
        public string TrailerId { get; set; }
        [FirestoreProperty]
        public string CompartmentId { get; set; }
        [FirestoreProperty]
        public double Quantity { get; set; }
        [FirestoreProperty]
        public int? TrackableScheduleId { get; set; }
    }

    [FirestoreData]
    public class MobileImageViewModel
    {
        [FirestoreProperty]
        public int Id { get; set; }
        [FirestoreProperty]
        public bool IsPdf { get; set; }
        [FirestoreProperty]
        public string FilePath { get; set; }
        [FirestoreProperty]
        public string SignatureName { get; set; }
    }

    [FirestoreData]
    public class MobileInvoiceLiftTicketViewModel
    {
        [FirestoreProperty]
        public int Id { get; set; }
        [FirestoreProperty]
        public string LiftTicketNumber { get; set; }
        [FirestoreProperty]
        public long LiftDate { get; set; }
        [FirestoreProperty]
        public string BadgeNumber { get; set; }
        [FirestoreProperty]
        public List<MobileLiftProductViewModel> Products { get; set; }
        [FirestoreProperty]
        public MobileImageViewModel Images { get; set; }
        [FirestoreProperty]
        public long? LiftStartTime { get; set; }
        [FirestoreProperty]
        public long? LiftEndTime { get; set; }
    }

    [FirestoreData]
    public class MobileLiftProductViewModel
    {
        [FirestoreProperty]
        public int ProductId { get; set; }
        [FirestoreProperty]
        public string ProductName { get; set; }
        [FirestoreProperty]
        public double? LiftQuantity { get; set; }
        [FirestoreProperty]
        public double? NetQuantity { get; set; }
        [FirestoreProperty]
        public double? GrossQuantity { get; set; }
        [FirestoreProperty]
        public double? DeliveredQuantity { get; set; }
        [FirestoreProperty]
        public int BulkPlantId { get; set; }
        [FirestoreProperty]
        public string BulkPlantName { get; set; }
        [FirestoreProperty]
        public MobileDropAddressViewModel Address { get; set; }
        [FirestoreProperty]
        public List<MobileCompartmentInfoViewModel> CompartmentInfo { get; set; } = new List<MobileCompartmentInfoViewModel>();
    }

    [FirestoreData]
    public class MobileDropAddressViewModel
    {
        [FirestoreProperty]
        public string Address { get; set; }
        [FirestoreProperty]
        public string City { get; set; }
        [FirestoreProperty]
        public int StateId { get; set; }
        [FirestoreProperty]
        public int CountryId { get; set; }
        [FirestoreProperty]
        public string ZipCode { get; set; }
        [FirestoreProperty]
        public string CountyName { get; set; }
        [FirestoreProperty]
        public double Latitude { get; set; }
        [FirestoreProperty]
        public double Longitude { get; set; }
        [FirestoreProperty]
        public string TimeZoneName { get; set; }
        [FirestoreProperty]
        public string JobName { get; set; }
        [FirestoreProperty]
        public int JobId { get; set; }
    }

    [FirestoreData]
    public class MobileInvoiceDropViewModel
    {
        [FirestoreProperty]
        public int OrderId { get; set; }
        [FirestoreProperty]
        public int TypeOfFuel { get; set; }
        [FirestoreProperty]
        public int FuelTypeId { get; set; }
        [FirestoreProperty]
        public double ActualDropQuantity { get; set; }
        [FirestoreProperty]
        public long DropDate { get; set; }
        [FirestoreProperty]
        public string StartTime { get; set; }
        [FirestoreProperty]
        public string EndTime { get; set; }
        [FirestoreProperty]
        public int? TrackableScheduleId { get; set; }
        [FirestoreProperty]
        public int? TerminalId { get; set; }
        [FirestoreProperty]
        public PickupLocationType PickupLocationType { get; set; }
        [FirestoreProperty]
        public MobileDropAddressViewModel PickUpAddress { get; set; }
        [FirestoreProperty]
        public int AssetCount { get; set; }
        [FirestoreProperty]
        public bool IsAssetDropOffline { get; set; }
        [FirestoreProperty]
        public List<MobileAssetDropViewModel> AssetDrops { get; set; } = new List<MobileAssetDropViewModel>();
        [FirestoreProperty]
        public string DropTicketNumber { get; set; }
        [FirestoreProperty]
        public MobileDropStatus DropStatus { get; set; }
        [FirestoreProperty]
        public bool IsFilldInvoke { get; set; }
        [FirestoreProperty]
        public long FilldStopId { get; set; }
        [FirestoreProperty]
        public MobileBDRDetailViewModel BdrDetails { get; set; }
        [FirestoreProperty]
        public string DeliveryLevelPO { get; set; }
    }

    [FirestoreData]
    public class MobileAssetDropViewModel
    {
        [FirestoreProperty]
        public int OrderId { get; set; }
        [FirestoreProperty]
        public string AssetName { get; set; }
        [FirestoreProperty]
        public int JobXAssetId { get; set; }
        [FirestoreProperty]
        public int DropStatusId { get; set; }
        [FirestoreProperty]
        public double DropGallons { get; set; }
        [FirestoreProperty]
        public long DropDate { get; set; }
        [FirestoreProperty]
        public string StartTime { get; set; }
        [FirestoreProperty]
        public string EndTime { get; set; }
        [FirestoreProperty]
        public double MeterStartReading { get; set; }
        [FirestoreProperty]
        public double MeterEndReading { get; set; }
        [FirestoreProperty]
        public bool IsNewAsset { get; set; }
        [FirestoreProperty]
        public double? PreDip { get; set; }
        [FirestoreProperty]
        public double? PostDip { get; set; }
        [FirestoreProperty]
        public int? TankScaleMeasure { get; set; }
        [FirestoreProperty]
        public bool IsOfflineMode { get; set; }
    }

    [FirestoreData]
    public class DemurrageDetail
    {
        [FirestoreProperty]
        public long StartTime { get; set; }
        [FirestoreProperty]
        public int StartOffset { get; set; }
        [FirestoreProperty]
        public long EndTime { get; set; }
        [FirestoreProperty]
        public int EndOffset { get; set; }
        [FirestoreProperty]
        public int FeeTypeId { get; set; }
    }

    [FirestoreData]
    public class MobileBDRDetailViewModel
    {
        [FirestoreProperty]
        public int InvoiceId { get; set; }
        [FirestoreProperty]
        public string BDRNumber { get; set; }
        [FirestoreProperty]
        public string PumpingStartTime { get; set; }
        [FirestoreProperty]
        public string PumpingStopTime { get; set; }
        [FirestoreProperty]
        public string OpenMeterReading { get; set; }
        [FirestoreProperty]
        public string CloseMeterReading { get; set; }
        [FirestoreProperty]
        public string Viscosity { get; set; }
        [FirestoreProperty]
        public string SulphurContent { get; set; }
        [FirestoreProperty]
        public string FlashPoint { get; set; }
        [FirestoreProperty]
        public string DensityInVaccum { get; set; }
        [FirestoreProperty]
        public string ObservedTemperature { get; set; }
        [FirestoreProperty]
        public string MeasuredVolume { get; set; }
        [FirestoreProperty]
        public string StandardVolume { get; set; }
        [FirestoreProperty]
        public string MarpolSampleNumbers { get; set; }
        [FirestoreProperty]
        public string MVMarpolSampleNumbers { get; set; }
        [FirestoreProperty]
        public bool IsEngineerInvitedToWitnessSample { get; set; }
        [FirestoreProperty]
        public bool IsNoticeToProtestIssued { get; set; }
    }
}
