using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.Common
{
    public class BaseModel
    {
        public Status StatusCode { get; set; } = Status.Failed;

        public string StatusMessage { get; set; }
    }
}
