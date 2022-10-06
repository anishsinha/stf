using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverOrderViewModel : StatusViewModel
    {
        public DriverOrderViewModel()
        {
            ContactPerson = new ContactPersonViewModel();
        }

        public DriverOrderViewModel(Status status)
            : base(status)
        {
            ContactPerson = new ContactPersonViewModel(status);
        }

        public int JobId { get; set; }

        public int FuelId { get; set; }

        public int OrderId { get; set; }

        public string OrderName { get; set; }

        public string CustomerOrderNumber { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public ContactPersonViewModel ContactPerson { get; set; }

        public string SupplierName { get; set; }

        public string FuelTypeName { get; set; }

        public decimal Quantity { get; set; }

        public decimal TotalOrderQuantity { get; set; }

        public decimal DroppedQuantity { get; set; }

        public int QuantityTypeId { get; set; }

        public int WetHosing { get; set; }

        public int OverWater { get; set; }

        public int AssetCount { get; set; }

        public bool IsAssetDropPicMandetory { get; set; }

        public bool IsFTL { get; set; }

        public bool IsDriverToUpdateBOL { get; set; }

        public bool IsDropImageRequired { get; set; }

        public bool IsBolImageRequired { get; set; }
        
        public int IsExactMatch { get; set; }

        public List<string> SpecialInstructions { get; set; }

        public int RunningMeterMode { get; set; }

        public int IsDriverAssigned { get; set; }

        public int IsDeliveryScheduleAdded { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

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

        public bool SignatureEnabled { get; set; }

        public string FileDetails { get; set; }

        public int CountryId { get; set; }

        public ScheduleQuantityType ScheduleQuantityType { get; set; } = ScheduleQuantityType.Quantity;
        public bool IsPrePostDipRequired { get; set; }
    }
}
