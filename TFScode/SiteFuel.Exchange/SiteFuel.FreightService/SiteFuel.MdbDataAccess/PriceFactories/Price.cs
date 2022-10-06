using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.PriceFactories
{
    public abstract class Price
    {
        public abstract int Type { get; }
        public abstract int CompanyId { get; set; }
        public abstract decimal Rate { get; set; }
        public abstract int Currency { get; set; }
        public abstract DateTimeOffset CreatedOn { get; set; }
        public abstract bool IsActive { get; set; }
        public abstract bool IsDeleted { get; set; }
    }
}
