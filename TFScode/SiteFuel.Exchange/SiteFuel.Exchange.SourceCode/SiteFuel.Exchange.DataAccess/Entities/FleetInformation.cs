using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class FleetInformation
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public FleetType FleetType{ get; set; }
        public int TrailerServiceType { get; set; }
        public int Capacity { get; set; }
        public bool DoesTrailerHasPump { get; set; }
        public bool IsTrailerMetered { get; set; }
        public int Count { get; set; }
        public bool IsPackagedGoods { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
    }
}
