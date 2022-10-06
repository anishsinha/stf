using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class CommonFields
    {
        public DateTimeOffset CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
