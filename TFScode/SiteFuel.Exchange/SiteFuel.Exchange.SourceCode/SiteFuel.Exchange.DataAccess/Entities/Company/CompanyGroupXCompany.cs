using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace SiteFuel.Exchange.DataAccess.Entities
{    
    public partial class CompanyGroupXCompany
    {
        public int Id { get; set; }

        public int CompanyGroupId { get; set; }

        public int SubCompanyId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("CompanyGroupId")]
        public virtual CompanyGroup CompanyGroup { get; set; }

        [ForeignKey("SubCompanyId")]
        public virtual Company SubCompany { get; set; }
    }
}
