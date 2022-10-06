using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class NewsfeedViewModel
    {
        public int Id { get; set; }

        public NewsfeedEvent EventId { get; set; }

        public int EntityId { get; set; }

        public EntityType EntityTypeId { get; set; }

        public int TargetEntityId { get; set; }

        public int RecipientCompanyId { get; set; }

        public string FeedMessage { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public bool IsRead { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
