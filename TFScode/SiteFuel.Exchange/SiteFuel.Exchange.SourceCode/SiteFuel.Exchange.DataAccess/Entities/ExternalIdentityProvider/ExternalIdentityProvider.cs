 namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public partial class ExternalIdentityProvider
    {
        [Key]
        public int CompanyId { get; set; }

        [Required]
        public string IdentityProviderIssuer { get; set; }

        public string IdentityProviderSsoUri { get; set; }

        public string Certificate { get; set; }

        public bool IsActive { get; set; }

        public bool IsLoginPrompted { get; set; }

        public bool ShouldBypassPassword { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

    }
}
