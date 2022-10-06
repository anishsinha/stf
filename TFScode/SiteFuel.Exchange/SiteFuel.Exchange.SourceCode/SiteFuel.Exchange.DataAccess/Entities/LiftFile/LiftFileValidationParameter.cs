using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class LiftFileValidationParameter
    {
        public int Id { get; set; }
        public int OnboardingPreferenceId { get; set; }
        public LFVParameterType ParameterType { get; set; }
        public bool IsTerminalCodeReq { get; set; }
        public bool IsBolReq { get; set; }
        public bool IsCarrierIdReq { get; set; }
        public bool IsCarrierNameReq { get; set; }
        public bool IsLoadDateReq { get; set; }
        public bool IsTermItemCodeReq { get; set; }
        public bool IsCorrectedQtyReq { get; set; }
        public bool IsGrossReq { get; set; }
        public bool IsUnsupportedDataToIncludeInCleanRecords { get; set; }
        public bool IsActive { get; set; }

        public int NoMatchRecordDays { get; set; }
        public bool IsIgnoreSelfHauling { get; set; }
        public bool IsReplacePoWithAccoutingId { get; set; }
        public bool IsIgnoreWholesalebadge { get; set; }
        public bool IsIgnoreNonRegisteredCarriers { get; set; }
        public bool IsIgnoreQuebecBillingBadges { get; set; }
        [ForeignKey("OnboardingPreferenceId")]
        public virtual OnboardingPreference OnboardingPreference { get; set; }
    }
}
