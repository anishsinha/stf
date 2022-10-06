using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class MstOnboardingQuestionType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstOnboardingQuestionType()
        {
            MstOnboardingQuestions = new HashSet<MstOnboardingQuestion>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstOnboardingQuestion> MstOnboardingQuestions { get; set; }
    }
}
