using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class SpecialInstructionMapper
    {
        public static SpecialInstruction ToEntity(this SpecialInstructionViewModel viewModel, SpecialInstruction entity = null)
        {
            if (entity == null)
                entity = new SpecialInstruction();

            entity.Id = viewModel.Id;
            entity.Instruction = viewModel.Instruction;
            return entity;
        }

        public static SpecialInstructionViewModel ToViewModel(this SpecialInstruction entity, SpecialInstructionViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new SpecialInstructionViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.Instruction = entity.Instruction;
            return viewModel;
        }
    }
}
