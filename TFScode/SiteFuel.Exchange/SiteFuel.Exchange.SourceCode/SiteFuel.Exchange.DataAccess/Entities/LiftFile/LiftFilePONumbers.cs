using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class LiftFilePONumbers
    {
        public int Id { get; set; }
        public int AddedByCompanyId { get; set; }

        [StringLength(512)]
        public string SelfHaulingPoNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset AddedDate { get; set; }
    }
}
