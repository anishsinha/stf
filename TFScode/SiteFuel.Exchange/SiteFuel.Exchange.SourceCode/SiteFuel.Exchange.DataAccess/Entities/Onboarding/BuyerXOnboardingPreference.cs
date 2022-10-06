namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class BuyerXOnboardingPreference
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BuyerXOnboardingPreference()
        {
        }

        [Key]
        public int Id { get; set; }
        public int OnboardingPreferenceId { get; set; }
        public int BuyerCompanyId { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("OnboardingPreferenceId")]
        public virtual OnboardingPreference OnboardingPreference { get; set; }
    }
}
