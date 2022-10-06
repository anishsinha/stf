using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class LiftFileCarrierNames
    {

        public int Id { get; set; }
        public int AddedByCompanyId { get; set; }

        [StringLength(512)]
        public string CarrierName { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset AddedDate { get; set; }
    }
}
