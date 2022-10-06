using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class SAPOrderStatus
    {
        [Key]
        public int Id { get; set; }

        public string JsonRequest { get; set; }

        public bool IsProcessed { get; set; }
    }
}
