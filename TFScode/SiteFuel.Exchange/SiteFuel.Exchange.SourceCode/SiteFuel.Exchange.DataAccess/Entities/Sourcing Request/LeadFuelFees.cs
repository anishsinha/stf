using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class LeadFuelFee
    {
        [Key]
        public int Id { get; set; }
        public int LeadFuelDetailsId { get; set; }
        public int FeeTypeId { get; set; }
        public int FeeSubTypeId { get; set; }
        public decimal Fee { get; set; }
        [StringLength(256)]
        public string FeeDetails { get; set; }
        [DefaultValue(false)]
        public bool IncludeInPPG { get; set; }
        public Nullable<int> FeeConstraintTypeId { get; set; }
        [ForeignKey("LeadFuelDetailsId")]
        public virtual LeadFuelDetail LeadFuelDetails { get; set; }
    }
}
