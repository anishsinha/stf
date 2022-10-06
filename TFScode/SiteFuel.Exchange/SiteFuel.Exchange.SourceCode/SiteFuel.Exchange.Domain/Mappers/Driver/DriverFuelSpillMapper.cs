using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class DriverFuelSpillMapper
    {
        public static DriverFuelSpillViewModel ToViewModel(this Spill entity, DriverFuelSpillViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new DriverFuelSpillViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.SpillDate = entity.SpillDate;
            viewModel.SpilledBy = entity.SpilledBy;
            viewModel.Notes = entity.Notes;
            viewModel.AssetId = entity.AssetId;
            viewModel.OrderId = entity.OrderId;
            viewModel.InvoiceId = entity.InvoiceId;
            viewModel.SpillImages = entity.Images.Select(t => t.ToViewModel()).ToList();

            return viewModel;
        }

        public static Spill ToEntity(this DriverFuelSpillViewModel viewModel, Spill entity = null)
        {
            if (entity == null)
                entity = new Spill();

            entity.Id = viewModel.Id;
            entity.SpillDate = viewModel.SpillDate;
            entity.SpilledBy = viewModel.SpilledBy;
            entity.Notes = viewModel.Notes;
            entity.AssetId = viewModel.AssetId;
            entity.OrderId = viewModel.OrderId;
            entity.InvoiceId = viewModel.InvoiceId;
            entity.Images = viewModel.SpillImages.Select(t => t.ToEntity()).ToList();

            return entity;
        }
    }
}
