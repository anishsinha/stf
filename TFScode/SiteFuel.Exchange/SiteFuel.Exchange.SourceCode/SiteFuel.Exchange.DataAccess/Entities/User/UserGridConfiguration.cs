using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class UserGridConfiguration
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int GridId { get; set; }

        public string Setting { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
