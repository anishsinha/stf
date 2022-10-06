using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class PaymentDiscountMapper
    {
        public static PaymentDiscount ToEntity(this PaymentDiscountViewModel viewModel, PaymentDiscount entity = null)
        {
            if (entity == null)
                entity = new PaymentDiscount();

            entity.Id = viewModel.Id;
            entity.DiscountPercentage = viewModel.DiscountPercent;
            entity.WithInDays = viewModel.WithinDays;
            return entity;
        }

        public static PaymentDiscountViewModel ToViewModel(this PaymentDiscount entity, PaymentDiscountViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new PaymentDiscountViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.DiscountPercent = entity.DiscountPercentage;
            viewModel.IsDiscountOnEarlyPayment = (entity.Id > 0 && entity.DiscountPercentage > 0);
            viewModel.WithinDays = entity.WithInDays;
            return viewModel;
        }
    }
}
