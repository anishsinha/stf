using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class AppSettingViewModel : ResponseViewModel
    {
        public AppSettingViewModel()
            : base(Status.Failed)
        {
        }

        public AppSettingViewModel(Status status)
            : base(status)
        {
        }

        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
