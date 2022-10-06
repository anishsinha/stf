using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class RecipientFilterViewModel
    {
        public int SendersId { get; set; }

        public int SendersCompanyId { get; set; }

        public AppMessageQueryType Type { get; set; }

        public int Number { get; set; }
    }
}
