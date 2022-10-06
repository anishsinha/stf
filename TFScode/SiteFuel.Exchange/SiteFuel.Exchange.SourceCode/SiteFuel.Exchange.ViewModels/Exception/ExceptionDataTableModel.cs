using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Core;

namespace SiteFuel.Exchange.ViewModels
{
    public class ExceptionDataTableModel : DataTableAjaxPostModel
    {
        public string FromDateTime { get; set; }
        public string ToDateTime { get; set; }
    }
}
