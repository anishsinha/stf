using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Forcasting
{
    public class ForcastingPreferenceViewModel
    {
        public int Id { get; set; }
        public int? BuyerCompanyId { get; set; }
        public int? SupplierCompanyId { get; set; }
        public int ForcastingServicePreference { get; set; } = 1;
        public int ForcastingSettingId { get; set; }
        public ForcastingServiceSettingViewModel ForcastingServiceSetting { get; set; } = new ForcastingServiceSettingViewModel();
        public int ForcastingSettingLevel { get; set; }
        public int EntityId { get; set; }
        public int prevEntityId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsEditable { get; set; } = false;
        public string uomDetails { get; set; }
        public int IsAccountInactive { get; set; } = 0;
    }
}
