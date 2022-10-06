using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Dispatcher
{
    public class WhereIsMyDriverBuyerAppInputModel
    {
        public WhereIsMyDriverBuyerAppInputModel()
        {
            States = new List<int>();
            Cities = new List<string>();
            SupplierCompanyIds = new List<int>();
            CarrierCompanyIds = new List<int>();
            LocationIds = new List<int>();
        }
        public int CompanyId { get; set; }
        public long ScheduleDate { get; set; }
        public int UserTimeOffset { get; set; }
        public List<int> States { get; set; }
        public List<string> Cities { get; set; }
        public List<int> SupplierCompanyIds { get; set; }
        public List<int> CarrierCompanyIds { get; set; }
        public List<int> LocationIds { get; set; }

    }
}
