using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class UserFilterSetting
    { 
        public int Id { get; set; }

        public TfxModule ModuleId { get; set; }
 
        public int UserId { get; set; }

        public string Filter { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
    }
}
