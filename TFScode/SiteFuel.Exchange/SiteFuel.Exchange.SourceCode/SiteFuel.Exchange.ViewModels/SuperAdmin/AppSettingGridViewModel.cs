using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class AppSettingGridViewModel
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public string UpdatedBy { get; set; }

        public string UpdatedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
