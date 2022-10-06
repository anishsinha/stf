using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using FileHelpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SiteFuel.Exchange.ViewModels
{
    public class PoFileCsvViewModel
    {
        [Name("SelfHaul PO")]
        public string SelfHaulingPoNumber { get; set; }
    }

    public class PoFileCsvViewModelMap : ClassMap<PoFileCsvViewModel>
    {
        public PoFileCsvViewModelMap()
        {
            Map(m => m.SelfHaulingPoNumber).Name("SelfHaul PO");
        }
    }
}
