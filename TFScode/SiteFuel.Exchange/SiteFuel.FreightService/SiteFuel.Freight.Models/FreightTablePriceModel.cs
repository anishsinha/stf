using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class FreightTablePriceModel
    {
        public string Id { get; set; }
        public int CompanyId { get; set; }
        public string FreightTableId { get; set; }
        public List<PriceModel> FreightPrices { get; set; }
    }
}
