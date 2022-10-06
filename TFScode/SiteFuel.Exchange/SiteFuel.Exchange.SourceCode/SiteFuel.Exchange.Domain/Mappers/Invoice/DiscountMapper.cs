using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class DiscountMapper
    {
        public static Discount ToEntity(this DiscountViewModel viewModel, Discount entity = null)
        {
            if (entity == null)
                entity = new Discount();

            entity.InvoiceId = viewModel.InvoiceId;
            entity.DealName = viewModel.DealName;
            entity.DealStatus = viewModel.DealStatus;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.CreatedCompanyId = viewModel.CreatedCompanyId;
            entity.StatusChangedBy = viewModel.StatusChangedBy;
            entity.StatusChangedDate = viewModel.StatusChangedDate;
            entity.StatusChangedCompanyId = viewModel.StatusChangedCompanyId;
            entity.OrderId = viewModel.OrderId;
            entity.Notes = viewModel.Notes;
            entity.DiscountLineItems = viewModel.DiscountLineItems.Select(t => t.ToEntity()).ToList();

            return entity;
        }

        public static DiscountLineItem ToEntity(this DiscountLineItemViewModel viewModel, DiscountLineItem entity = null)
        {
            if (entity == null)
                entity = new DiscountLineItem();

            entity.FeeTypeId = viewModel.FeeTypeId;
            entity.FeeSubTypeId = viewModel.FeeSubTypeId;
            entity.Amount = viewModel.Amount;
            if (viewModel.FeeTypeId == (int)FeeType.OtherFee)
                entity.FeeDetails = viewModel.FeeDetails;
            return entity;
        }

        public static DiscountViewModel ToViewModel(this Discount entity)
        {
            var viewModel = new DiscountViewModel
            {
                DealName = entity.DealName,
                DealStatus = entity.DealStatus,
                CreatedBy = entity.CreatedBy,
                CreatedCompanyId = entity.CreatedCompanyId,
                CreatedDate = entity.CreatedDate,
                StatusChangedBy = entity.StatusChangedBy,
                StatusChangedCompanyId = entity.StatusChangedCompanyId,
                StatusChangedDate = entity.StatusChangedDate,
                OrderId = entity.OrderId,
                Notes = entity.Notes,
                DiscountLineItems = entity.DiscountLineItems.Select(t => t.ToViewModel()).ToList()
            };
            return viewModel;
        }

        public static DiscountLineItemViewModel ToViewModel(this DiscountLineItem entity)
        {
            var viewModel = new DiscountLineItemViewModel();
            viewModel.Amount = entity.Amount;
            viewModel.FeeTypeId = entity.FeeTypeId;
            viewModel.FeeSubTypeId = entity.FeeSubTypeId;
            viewModel.FeeDetails = entity.FeeDetails;
            viewModel.OtherFeeTypeId = null;
            return viewModel;
        }
    }
}
