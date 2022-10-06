using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class CommonFieldsModel : StatusModel
    {
        public DateTimeOffset CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
