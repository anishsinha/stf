
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.LiftFile
{
    public class CarrierNamesCsvViewModel
    {

        [Name("CarrierName")]
        public string CarrierName { get; set; }
    }
    public class CarrierNamesCsvViewModelMap : ClassMap<CarrierNamesCsvViewModel>
    {
        public CarrierNamesCsvViewModelMap()
        {
            Map(m => m.CarrierName).Name("CarrierName");
        }
    }
}
