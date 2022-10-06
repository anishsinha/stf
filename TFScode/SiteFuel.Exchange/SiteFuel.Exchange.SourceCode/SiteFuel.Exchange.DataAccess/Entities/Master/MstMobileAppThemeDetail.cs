namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MstMobileAppThemeDetail
    {
        public int Id { get; set; }

        public int ThemeId { get; set; }

        [Required]
        [StringLength(256)]
        public string Key { get; set; }

        [Required]
        [StringLength(256)]
        public string Value { get; set; }

        public virtual MstMobileAppTheme MstMobileAppTheme { get; set; }
    }
}
