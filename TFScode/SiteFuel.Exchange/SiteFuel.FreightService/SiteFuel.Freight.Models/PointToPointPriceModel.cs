using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class PointToPointPriceModel : PriceModel
    {
        private readonly PriceType _type;
        public PointToPointPriceModel()
        {
            _type = PriceType.PointToPoint;
        }
        public override PriceType Type { get { return _type; } }
        public override int CompanyId { get; set; }
        public override decimal Rate { get; set; }
        public override Currency Currency { get; set; }
        public override DateTimeOffset CreatedOn { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public override bool IsActive { get; set; }
        public override bool IsDeleted { get; set; }
    }
}
