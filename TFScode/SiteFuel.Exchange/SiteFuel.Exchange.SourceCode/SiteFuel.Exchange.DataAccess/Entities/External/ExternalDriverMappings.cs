using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
  public  class ExternalDriverMappings
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DriverId { get; set; }

        [StringLength(512)]
        public string TargetDriverValue { get; set; }

        [Required]
        public int ThirdPartyId { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }
        public int CreatedByCompanyId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("DriverId")]
        public virtual User User { get; set; }

        [ForeignKey("ThirdPartyId")]
        public virtual MstExternalThirdPartyCompanies ExternalThirdPartyCompanies { get; set; }

        [ForeignKey("CreatedByCompanyId")]
        public virtual Company CreatedByCompany { get; set; }
    }
}
