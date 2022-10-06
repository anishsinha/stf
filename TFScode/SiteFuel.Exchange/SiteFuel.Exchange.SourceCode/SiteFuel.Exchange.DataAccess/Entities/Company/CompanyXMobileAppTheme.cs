namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CompanyXMobileAppTheme
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CompanyId { get; set; }
		
        [Column(Order = 1)]
        public int ThemeId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AppTypeId { get; set; }

        public virtual Company Company { get; set; }

        public virtual MstAppType MstAppType { get; set; }

        public virtual MstMobileAppTheme MstMobileAppTheme { get; set; }
    }
}
