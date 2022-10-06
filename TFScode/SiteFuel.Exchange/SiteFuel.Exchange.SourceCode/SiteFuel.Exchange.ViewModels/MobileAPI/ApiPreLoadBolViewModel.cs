using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiPreLoadBolViewModel
    {
        private decimal? _retainQuantity = null;
        private decimal? _netQuantity = null;
        private decimal? _grossQuantity = null;
        public int Id { get; set; }
        public string BolNumber { get; set; }
        public string BadgeNumber { get; set; }
        public bool IsBulkPlant { get; set; }
        public string LiftTicketNumber { get; set; }
        public DateTimeOffset? LiftDate { get; set; }
        public string TraceId { get; set; }
        public string Carrier { get; set; }
        public int PickupLocation { get; set; }
        public int FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public decimal? NetQuantity
        {
            get { return (_netQuantity == null || _netQuantity <= 0) && LiftQuantity > 0 ? LiftQuantity : _netQuantity; }
            set { _netQuantity = value; }
        }
        public decimal? GrossQuantity
        {
            get { return (_grossQuantity == null || _grossQuantity <= 0) && LiftQuantity > 0 ? LiftQuantity : _grossQuantity; }
            set { _grossQuantity = value; }
        }
        
        public int OrderId { get; set; }
        public int? DeliveryScheduleId { get; set; }
        public int? TrackableScheduleId { get; set; }

        public TimeSpan? LiftArrivalTime { get; set; }
        public TimeSpan? BolCreationTime { get; set; }

        //Terminal
        public int? TerminalId { get; set; }
        public string TerminalName { get; set; }

        //Bulk Plant
        public decimal? LiftQuantity { get; set; }
        public int BulkPlantId { get; set; }
        public string BulkPlantName { get; set; }
        public TimeSpan? LiftStartTime { get; set; }
        public TimeSpan? LiftEndTime { get; set; }

        //Address Details
        public string Address { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string CountyName { get; set; }
        public string StateCode { get; set; }
        public int? StateId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string ZipCode { get; set; }

        public string BolImage { get; set; }
        public decimal? PricePerGallon { get; set; }
        public decimal? RackPrice { get; set; }

        //COMPARTMENT INFO
        public List<TrailerWithCompartments> TrailerWithCompartments { get; set; }
        public string CompartmentInfo { get; set; }
        //Retain Info
        public string DeliveryReqId { get; set; }
        public decimal? RetainQuantity
        {
            get { return _retainQuantity == null && _retainQuantity > 0 ? RetainQuantity : _retainQuantity; }
            set { _retainQuantity = value; }
        }
        public List<PreBOLRetainDeliveryDetailsModel> BOLRetainDetails = new List<PreBOLRetainDeliveryDetailsModel>();
        public List<CompartmentsInfoViewModel> RetainCompartmentDetails { get; set; } = new List<CompartmentsInfoViewModel>();
        public decimal TotalRetainQty { get; set; }
    }
    public class PreBOLRetainModel
    {
        public string DeliveryReqId { get; set; }
        public int FuelTypeId { get; set; }
        public List<CompartmentsInfoViewModel> CompartmentInfo { get; set; } = new List<CompartmentsInfoViewModel>();
    }
    public class PreBOLRetainDeliveryDetailsModel
    {
        public string DeliveryReqId { get; set; }
        public string TrailerId { get; set; }
        public string CompartmentId { get; set; }
        public string ProductType { get; set; }
        public int FuelTypeId { get; set; }
        public decimal RetainQuantity { get; set; } = 0;
        public bool IsTrailerRetain { get; set; } = true;
    }
}
