using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class InvoiceXSpecialInstructionMapper
    {
        public static InvoiceXSpecialInstructionViewModel ToViewModel(this InvoiceXSpecialInstruction entity, InvoiceXSpecialInstructionViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new InvoiceXSpecialInstructionViewModel(Status.Success);

            viewModel.InvoiceId = entity.InvoiceId;
            viewModel.SpecialInstructionId = entity.SpecialInstructionId;
            viewModel.IsInstructionFollowed = entity.IsInstructionFollowed;
            viewModel.Instruction = entity.SpecialInstruction.Instruction;
            return viewModel;
        }

        public static InvoiceXSpecialInstruction ToEntity(this InvoiceXSpecialInstructionViewModel viewModel, InvoiceXSpecialInstruction entity = null)
        {
            if (entity == null)
                entity = new InvoiceXSpecialInstruction();

            entity.InvoiceId = viewModel.InvoiceId;
            entity.SpecialInstructionId = viewModel.SpecialInstructionId;
            entity.IsInstructionFollowed = viewModel.IsInstructionFollowed;

            return entity;
        }
    }
}
