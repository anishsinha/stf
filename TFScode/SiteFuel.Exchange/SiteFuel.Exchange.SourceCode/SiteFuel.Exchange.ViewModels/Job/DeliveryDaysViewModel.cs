using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using SiteFuel.Exchange.Core.StringResources;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryDaysViewModel : BaseViewModel
    {
        public DeliveryDaysViewModel()
        {
            InstanceInitialize();
        }

        public DeliveryDaysViewModel(Status status) 
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Count = 1;
        }

        [Display(Name = nameof(Resource.headingDeliveryDays), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int? DeliveryDays { get; set; }
       
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string FromDeliveryTime { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string ToDeliveryTime { get; set; }
        
        public bool IsAcceptNightDeliveries { get; set; }
        
        public int Count { get; set; }
        public string FinalString
        {
            get
            {
                if (this.DeliveryDays.HasValue && DeliveryDays.Value > 0)
                {
                    var fromTime = Convert.ToDateTime(FromDeliveryTime).ToString("hh:mm tt");
                    var toTime = Convert.ToDateTime(ToDeliveryTime).ToString("hh:mm tt");
                    return $"{((WeekDay)DeliveryDays.Value).GetDisplayName()}: {fromTime} - {toTime}";
                }
                return string.Empty;
            }
        }
    }
}
