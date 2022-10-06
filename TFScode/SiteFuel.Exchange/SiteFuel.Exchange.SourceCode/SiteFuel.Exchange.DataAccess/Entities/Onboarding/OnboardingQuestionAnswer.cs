using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class OnboardingQuestionAnswer
    {
        [Key]
        [Column(Order = 0)]
        public int CompanyId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int OnboardingQuestionId { get; set; }

        [Required]
        [StringLength(512)]
        public string QuestionAnswer { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [ForeignKey("OnboardingQuestionId")]
        public virtual MstOnboardingQuestion MstOnboardingQuestion { get; set; }
    }
}
