using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class DistanceRangePriceModel : PriceModel
    {
        private readonly PriceType _type;
        public DistanceRangePriceModel()
        {
            _type = PriceType.DistanceRange;
        }
        public override PriceType Type { get { return _type; } }
        public override int CompanyId { get; set; }
        public override decimal Rate { get; set; }
        public override Currency Currency { get; set; }
        public override DateTimeOffset CreatedOn { get; set; }
        public decimal MinDistance { get; set; }
        public decimal MaxDistance { get; set; }
        public override bool IsActive { get; set; }
        public override bool IsDeleted { get; set; }
    }
}
