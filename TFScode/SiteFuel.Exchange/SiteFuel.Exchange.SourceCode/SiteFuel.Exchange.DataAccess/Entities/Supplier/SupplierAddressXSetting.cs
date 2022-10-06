namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SupplierAddressXSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AddressId { get; set; }

        public bool IsStateWideService { get; set; }

        public int Radius { get; set; }

        public bool IsLocationOwned { get; set; }

        public bool IsHedgeOrderAllowed { get; set; }

        public bool IsOverWaterRefuelingAllowed { get; set; }

        public virtual CompanyAddress CompanyAddress { get; set; }
    }
}
