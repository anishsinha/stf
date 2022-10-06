using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class MstOnboardingQuestion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstOnboardingQuestion()
        {
            OnboardingQuestionAnswers = new HashSet<OnboardingQuestionAnswer>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(512)]
        public string Question { get; set; }

        public int QuestionTypeId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("QuestionTypeId")]
        public virtual MstOnboardingQuestionType MstOnboardingQuestionType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OnboardingQuestionAnswer> OnboardingQuestionAnswers { get; set; }
    }
}
