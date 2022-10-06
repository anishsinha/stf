using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class CarrierMapping
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CarrierMapping()
        {
        }
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string AssignedTerminalId { get; set; }
        public string CarrierName { get; set; }
        public string AssignedCarrierId { get; set; }
        public int? CarrierCompanyId { get; set; }              
        public int TerminalCompanyAliasId { get; set; }            
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.Now;
        public bool IsActive { get; set; }

        public int CountryId { get; set; }
        
        [ForeignKey("CarrierCompanyId")]
        public virtual Company CarrierCompany { get; set; }

        [ForeignKey("TerminalCompanyAliasId")]
        public virtual TerminalCompanyAlias TerminalCompanyAlias { get; set; }

        //[ForeignKey("TerminalId")]
        //public virtual MstExternalTerminal ExternalTerminalMapping { get; set; }

        //[ForeignKey("BulkPlantId")]
        //public virtual BulkPlantLocation BulkPlantLocation { get; set; }

        //public int? TerminalId { get; set; }
        //public int? BulkPlantId { get; set; }


    }
}
