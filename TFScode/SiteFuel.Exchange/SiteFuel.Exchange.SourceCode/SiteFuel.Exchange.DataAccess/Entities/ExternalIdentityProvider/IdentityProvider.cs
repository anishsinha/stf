namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public partial class IdentityProvider
    {
        [Key]
        public int IdentityProviderId { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string IdentityProviderIssuer { get; set; }

        [Required]
        public string IdentityProviderSsoUri { get; set; }
        public string Certificate { get; set; }
        public bool IsActive { get; set; }
        public bool IsSSO { get; set; }        
        public string Auth { get; set; }
    }
}
