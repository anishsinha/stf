namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MstCity")]
    public partial class MstCity
    {
        public int Id { get; set; }

        public int StateId { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public bool Incorporated { get; set; }

        public string ZipCodes { get; set; }

        [ForeignKey("StateId")]
        public virtual MstState MstState { get; set; }
    }
}
