using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.LiftFile
{
    public class QuebecBillingBadgeCsvViewModel
    {
        [Name("Quebec Billing Badge")]
        public string QuebecBillingBadge { get; set; }
    }
    public class QuebecBillingBadgeCsvViewModelMap : ClassMap<QuebecBillingBadgeCsvViewModel>
    {
        public QuebecBillingBadgeCsvViewModelMap()
        {
            Map(m => m.QuebecBillingBadge).Name("Quebec Billing Badge");
        }
    }
}

