namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MstEntityType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string EntityName { get; set; }
    }
}
