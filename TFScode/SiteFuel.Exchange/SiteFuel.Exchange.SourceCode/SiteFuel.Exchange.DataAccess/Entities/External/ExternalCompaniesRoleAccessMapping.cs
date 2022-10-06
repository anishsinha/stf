using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class ExternalCompaniesRoleAccessMapping
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ThirdPartyId { get; set; }

        [Required]
        public int RoleId { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("RoleId")]
        public virtual MstRole MstRole { get; set; }

        [ForeignKey("ThirdPartyId")]
        public virtual MstExternalThirdPartyCompanies MstExternalThirdPartyCompanies { get; set; }
    }
}
