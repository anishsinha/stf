using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class SourcingNotesMapper
    {
        public static SourcingNotesViewModel ToGeneralNotes(this UspGetGeneralNotesViewModel entity, SourcingNotesViewModel viewModel = null )
        {
            if (viewModel == null)
            {
                viewModel = new SourcingNotesViewModel();
            }

            viewModel.Id = entity.Id;
            viewModel.LeadRequestId = entity.LeadRequestId;
            viewModel.UserName = entity.UserName;
            viewModel.Note = entity.GeneralNote;
            viewModel.CreatedBy = entity.CreatedBy;
            viewModel.CreatedDate = Convert.ToDateTime(entity.CreatedDate.ToString()).ToShortDateString();

            return viewModel;
        }
    }
}
