using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class MstCompanyIdentifier
    {
        public int Id { get; set; }
        public int SupplierCompanyId { get; set; }
        [Required]
        [StringLength(8)]
        public string Identifier { get; set; }
    }
}
