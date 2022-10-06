using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class SiteFuelDataContext
    {
        public SiteFuelDataContext(string connection)
            : base(connection)
        {
        }
    }
}
