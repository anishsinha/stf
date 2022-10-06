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
    public class MobilePreLoadBolViewModel
    {
        [FirestoreProperty]
        public int Id { get; set; }
        [FirestoreProperty]
        public bool IsBulkPlant { get; set; }
        [FirestoreProperty]
        public string BolNumber { get; set; }
        [FirestoreProperty]
        public string LiftTicketNumber { get; set; }
        [FirestoreProperty]
        public long LiftDate { get; set; }
        [FirestoreProperty]
        public long PickupDate { get; set; }
        [FirestoreProperty]
        public string BadgeNumber { get; set; }
        [FirestoreProperty]
        public List<MobilePreLoadProductViewModel> Products { get; set; }
        [FirestoreProperty]
        public MobilePreLoadImageViewModel Images { get; set; }
        [FirestoreProperty]
        public int DriverId { get; set; }
        [FirestoreProperty]
        public int SupplierCompanyId { get; set; }
        [FirestoreProperty]
        public string Carrier { get; set; }
        [FirestoreProperty]
        public bool IsPreLoadBolCompleted { get; set; }
        [FirestoreProperty]
        public string TraceId { get; set; }

        [FirestoreProperty]
        public long? LiftStartTime { get; set; }
        [FirestoreProperty]
        public long? LiftEndTime { get; set; }
    }

    [FirestoreData]
    public class MobilePreLoadProductViewModel
    {
        [FirestoreProperty]
        public int FuelTypeId { get; set; }
        [FirestoreProperty]
        public double? NetQuantity { get; set; }
        [FirestoreProperty]
        public double? GrossQuantity { get; set; }
        [FirestoreProperty]
        public List<MobilePreLoadCompartmentInfoViewModel> CompartmentInfo { get; set; } = new List<MobilePreLoadCompartmentInfoViewModel>();

        [FirestoreProperty]
        public int OrderId { get; set; }
        [FirestoreProperty]
        public int? DeliveryScheduleId { get; set; }
        [FirestoreProperty]
        public int? TrackableScheduleId { get; set; }

        //Terminal Plank
        [FirestoreProperty]
        public int? TerminalId { get; set; }
        [FirestoreProperty]
        public string TerminalName { get; set; }


        //Bulk Plank
        [FirestoreProperty]
        public double? LiftQuantity { get; set; }
        [FirestoreProperty]
        public int BulkPlantId { get; set; }
        [FirestoreProperty]
        public string BulkPlantName { get; set; }
        [FirestoreProperty]
        public MobilePreLoadDropAddressViewModel Address { get; set; }
        [FirestoreProperty]
        public string ProductType { get; set; }
        [FirestoreProperty]
        public string FuelType { get; set; }
        
        [FirestoreProperty]
        public int ProductId { get; set; }
    }

    [FirestoreData]
    public class MobilePreLoadCompartmentInfoViewModel
    {
        [FirestoreProperty]
        public string TrailerId { get; set; }
        [FirestoreProperty]
        public string CompartmentId { get; set; }
        [FirestoreProperty]
        public double Quantity { get; set; }
        [FirestoreProperty]
        public int UOM { get; set; }
    }

    [FirestoreData]
    public class MobileCanceledScheduleModel
    {
        [FirestoreProperty]
        public List<int> TrackableScheduleIds { get; set; }
        [FirestoreProperty]
        public List<string> DeliveryRequestIds { get; set; }
        [FirestoreProperty]
        public List<string> GroupedParentDrIds { get; set; }
        [FirestoreProperty]
        public int DriverId { get; set; }

        [FirestoreProperty]
        public bool IsCancelAll { get; set; } = false;
    }

    [FirestoreData]
    public class MobilePreLoadImageViewModel
    {
        [FirestoreProperty]
        public int Id { get; set; }
        [FirestoreProperty]
        public bool IsPdf { get; set; }
        [FirestoreProperty]
        public string FilePath { get; set; }
    }

    [FirestoreData]
    public class MobilePreLoadDropAddressViewModel
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
}
