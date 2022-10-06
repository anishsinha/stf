using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class NewsfeedListViewModel
    {
        public int Id { get; set; }

        public NewsfeedEvent EventId { get; set; }

        public int EntityId { get; set; }

        public EntityType EntityTypeId { get; set; }

        public int TargetEntityId { get; set; }

        public int RecipientCompanyId { get; set; }

        public string FeedMessage { get; set; }

        public int CreatedBy { get; set; }

        public string CreatedDate { get; set; }

        public bool IsRead { get; set; }

        public string TargetUrl { get; set; }

        public int TotalMessages { get; set; }
    }
}
