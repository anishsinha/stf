namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public partial class CompanyIdentityService
    {
        [Key]
        public int CompanyIdentityServiceId { get; set; }
        public int CompanyId { get; set; }
        public int IdentityProviderId { get; set; }

        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("IdentityProviderId")]
        public virtual IdentityProvider IdentityProvider { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }



    }
}