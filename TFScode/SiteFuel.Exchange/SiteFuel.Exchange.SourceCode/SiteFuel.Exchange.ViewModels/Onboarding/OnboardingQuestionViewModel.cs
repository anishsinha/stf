using SiteFuel.Exchange.Core.StringResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class OnboardingQuestionViewModel
    {
        public int Id { get; set; }
        [Display(Name = nameof(Resource.lblQuestion), ResourceType = typeof(Resource))]
        public string Question { get; set; }

        public string Answer { get; set; }
    }
}
