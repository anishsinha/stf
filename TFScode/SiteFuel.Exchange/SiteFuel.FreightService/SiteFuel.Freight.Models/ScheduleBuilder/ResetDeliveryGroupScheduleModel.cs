using System.Collections.Generic;

namespace SiteFuel.FreightModels.ScheduleBuilder
{
    public class ResetDeliveryGroupScheduleModel : StatusModel
    {
        public List<int> DeliveryGroupIds { get; set; } = new List<int>();
        public List<int> DeliveryScheduleIds { get; set; } = new List<int>();
        public string ScheduleBuilderId { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public string UpdatedByName { get; set; }
        public List<string> DeliveryRequestIds { get; set; }
    }
}
