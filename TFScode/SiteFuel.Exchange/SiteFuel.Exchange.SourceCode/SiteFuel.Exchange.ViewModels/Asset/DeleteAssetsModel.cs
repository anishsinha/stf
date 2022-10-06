using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Asset
{
    public class DeleteTanksModel
    {
        public List<int> JobIds { get; set; }
        public List<int> TankIds { get; set; }
    }
}
