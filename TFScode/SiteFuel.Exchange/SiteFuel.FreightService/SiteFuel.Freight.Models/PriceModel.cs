using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public abstract class PriceModel
    {
        public abstract PriceType Type { get; }
        public abstract int CompanyId { get; set; }
        public abstract decimal Rate { get; set; }
        public abstract Currency Currency { get; set; }
        public abstract DateTimeOffset CreatedOn { get; set; }
        public abstract bool IsActive { get; set; }
        public abstract bool IsDeleted { get; set; }
    }
}
