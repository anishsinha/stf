using Foolproof;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class BolDetailViewModel
    {
        private decimal? _netQuantity = null;
        private decimal? _grossQuantity = null;
        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblGrossQuantity), ResourceType = typeof(Resource))]
        public decimal? GrossQuantity
        {
            get { return (_grossQuantity == null || _grossQuantity <= 0) && LiftQuantity > 0 ? LiftQuantity : _grossQuantity; }
            set { _grossQuantity = value; }
        }

        [Display(Name = nameof(Resource.lblNetQuantity), ResourceType = typeof(Resource))]
        public decimal? NetQuantity
        {
            get { return (_netQuantity == null || _netQuantity <= 0) && LiftQuantity > 0 ? LiftQuantity : _netQuantity; }
            set { _netQuantity = value; }
        }

        [Display(Name = nameof(Resource.lblDeliveredQuantity), ResourceType = typeof(Resource))]
        public decimal? DeliveredQuantity  { get; set; }

        [Display(Name = nameof(Resource.lblBOL), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        public string BolNumber { get; set; }

        [Display(Name = nameof(Resource.lblCarrier), ResourceType = typeof(Resource))]
        public string Carrier { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public ImageViewModel Image { get; set; }

        public int? ImageId { get; set; }

        [Display(Name = nameof(Resource.lblLiftDate), ResourceType = typeof(Resource))]
        public DateTimeOffset? LiftDate { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public TimeSpan? LiftArrivalTime { get; set; }

        public TimeSpan? LiftStartTime { get; set; }

        public TimeSpan? LiftEndTime { get; set; }

        public TimeSpan? BolCreationTime { get; set; }

        [Display(Name = nameof(Resource.lblLiftQuantity), ResourceType = typeof(Resource))]
        public decimal? LiftQuantity { get; set; }

        [Display(Name = nameof(Resource.lblLiftTicketNumber), ResourceType = typeof(Resource))]
        public string LiftTicketNumber { get; set; }

        public int? TerminalId { get; set; }

        public int? CityGroupTerminalId { get; set; }

        public decimal PricePerGallon { get; set; }

        public decimal RackPrice { get; set; }

        public PickupLocationType PickupLocationType { get; set; }

        public int TypeofFuel { get; set; }

        public string TerminalName { get; set; }

        public string CityGroupTerminalName { get; set; }

        public int OrderId { get; set; }

        public int FuelTypeId { get; set; }

        public string FuelType { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public string SiteName { get; set; }
        public string Address { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public int? StateId { get; set; }
        public string ZipCode { get; set; }
        public string CountryCode { get; set; }
        public int CountryId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string CountyName { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public int InvoiceId { get; set; }
        [Display(Name = nameof(Resource.lblBadgeNumber), ResourceType = typeof(Resource))]
        public string BadgeNumber { get; set; }

        public List<TierPricingForBol> TierPricingForBol { get; set; } = new List<TierPricingForBol>();
        public EbolMatchStatus EbolMatchStatus { get; set; } = EbolMatchStatus.NoMatch;
        public BolDetailViewModel Clone(int userId)
        {
            var thisObject = (BolDetailViewModel)this.MemberwiseClone();
            thisObject.CreatedBy = userId;
            return thisObject;
        }

        public BolDetailViewModel CopyObject(BolDetailViewModel source)
        {
            var inputString = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<BolDetailViewModel>(inputString);
        }

        public bool IsBolAvailable()
        {
            return !string.IsNullOrWhiteSpace(BolNumber) && NetQuantity.HasValue && GrossQuantity.HasValue;
        }

        public bool IsLiftInfoAvailable()
        {
            return !string.IsNullOrWhiteSpace(LiftTicketNumber) && NetQuantity.HasValue && GrossQuantity.HasValue;
        }

        public bool IsBolEditForLfv { get; set; }
        public string BolEditedNotes { get; set; }
        public string RecordHistory { get; set; }
    }

    public class TierPricingForBol
    {
        public decimal Quantity { get; set; }
        public decimal PricePerGallon { get; set; } //this is final price
        public decimal TierMinQuantity { get; set; }
        public decimal TierMaxQuantity { get; set; }

    }
}
