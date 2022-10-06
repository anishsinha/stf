using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class CompanyGroup
    {
        public int Id { get; set; }

        [MaxLength(500)]
        public string GroupName { get; set; }

        public CompanyGroupType CompanyGroupTypeId { get; set; }

        [Required]
        public int OwnerCompanyId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("OwnerCompanyId")]
        public virtual Company OwnerCompany { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyGroupXCompany> CompanyGroupXCompanies { get; set; }
    }
}
