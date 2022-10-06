using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class OnboardingQuestionsMapper
    {
        public static OnboardingQuestionViewModel ToViewModel(this MstOnboardingQuestion entity, OnboardingQuestionViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new OnboardingQuestionViewModel();

            viewModel.Id = entity.Id;
            viewModel.Question = entity.Question;

            return viewModel;
        }
    }
}
