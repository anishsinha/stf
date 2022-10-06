namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MstNewsfeedMessage
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        [StringLength(512)]
        public string BuyerMessage { get; set; }

        [StringLength(512)]
        public string SupplierMessage { get; set; }

        [StringLength(128)]
        public string TargetUrl { get; set; }
    }
}
