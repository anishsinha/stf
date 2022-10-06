using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class RescheduleDeliveryViewModel : BaseViewModel
    {
        public RescheduleDeliveryViewModel()
        {  
        }

        public RescheduleDeliveryViewModel(Status status)
            : base(status)
        {  
        }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int OrderId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int ScheduleId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int TrackableScheduleId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public DateTimeOffset DeliveryDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string StartTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]       
        public string EndTime { get; set; }

        public int? DriverId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal Quantity { get; set; }

        public DateTime JobCurrentTime { get; set; }

        public DateTime? JobEndDate { get; set; }

        public DateTime? FuelRequestEndDate { get; set; }

        public bool IsScheduleTab { get; set; }

        public CarrierViewModel Carrier { get; set; } = new CarrierViewModel();

        public SupplierSourceViewModel SupplierSource { get; set; } = new SupplierSourceViewModel();

        [Display(Name = nameof(Resource.lblLoadCode), ResourceType = typeof(Resource))]
        public string LoadCode { get; set; }

        public bool IsFtlOrder { get; set; }

        public ScheduleQuantityType ScheduleQuantityType { get; set; } = ScheduleQuantityType.Quantity;
    }
}
