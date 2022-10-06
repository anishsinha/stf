using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace SiteFuel.Exchange.ViewModels
{
    public partial class BuyerXOnboardingPreferenceViewModel
    {
        public BuyerXOnboardingPreferenceViewModel()
        {
        }

        public int Id { get; set; }
        public int OnboardingPreferenceId { get; set; }
        public int BuyerCompanyId { get; set; }
        public bool IsActive { get; set; }
    }
}
