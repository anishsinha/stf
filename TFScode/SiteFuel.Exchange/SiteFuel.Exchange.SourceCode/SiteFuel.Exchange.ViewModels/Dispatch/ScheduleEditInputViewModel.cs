using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ScheduleEditInputViewModel : BaseViewModel
    {
        public int OrderId { get; set; }
        public bool IsFtlOrder { get; set; }
        public int? DeliveryScheduleId { get; set; }
        public int? TrackableScheduleId { get; set; }
        public int EnrouteStatus { get; set; }
        public bool IsModifySchedule { get; set; }
        public bool IsSplitLoad { get; set; }
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public Currency Currency { get; set; }
        public UoM UoM { get; set; }
        public RescheduleDeliveryViewModel DeliverySchedule { get; set; } = new RescheduleDeliveryViewModel();
        public List<SplitLoadAddressViewModel> SplitLoadAddresses { get; set; } = new List<SplitLoadAddressViewModel>();
        public DispatchTerminalViewModel TerminalDetails { get; set; } = new DispatchTerminalViewModel();
    }
}
