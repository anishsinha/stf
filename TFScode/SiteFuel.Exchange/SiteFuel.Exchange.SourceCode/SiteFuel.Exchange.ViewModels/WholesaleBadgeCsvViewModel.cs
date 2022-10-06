using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class WholesaleBadgeCsvViewModel
    {
        [Name("Wholesale Badge#")]
        public string WholeSaleBadge { get; set; }
    }

    public class WholesaleBadgeCsvViewModelMap : ClassMap<WholesaleBadgeCsvViewModel>
    {
        public WholesaleBadgeCsvViewModelMap()
        {
            Map(m => m.WholeSaleBadge).Name("Wholesale Badge#");
        }
    }
}
