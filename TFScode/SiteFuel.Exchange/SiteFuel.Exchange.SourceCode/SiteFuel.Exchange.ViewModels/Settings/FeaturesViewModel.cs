using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class FeaturesViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
        [Display(Name = " ")]
        public bool IsEnabled { get; set; }

        public string Description { get; set; }

        public CompanyType CompanyType { get; set; }
    }
}
 