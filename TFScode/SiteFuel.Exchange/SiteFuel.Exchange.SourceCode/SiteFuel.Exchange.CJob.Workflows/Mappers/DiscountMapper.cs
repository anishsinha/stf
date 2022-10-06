using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.Workflows.Extensions;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Workflows.Mappers
{
    public static class DiscountMapper
    {
        public static OrderItemViewModel ToQbOrderItem(this DiscountLineItemViewModel viewModel)
        {
            var itemName = $"{QbConstants.DiscountOn} {viewModel.FeeTypeName}";
            if (viewModel.FeeTypeId == (int)FeeType.OtherFee)
            {
                itemName = $"{QbConstants.DiscountOn} {viewModel.FeeDetails}";
            }
            var orderItem = new OrderItemViewModel();
            orderItem.Name = itemName.ToQbItemName(viewModel.FeeTypeId);
            orderItem.Desc = $"{itemName} - {viewModel.FeeSubTypeName}";
            if (viewModel.FeeSubTypeId == (int)FeeSubType.Percent)
            {
                orderItem.Desc = $"{itemName} - {viewModel.Amount.GetPreciseValue(QbConstants.MaxDecimal)}{QbConstants.PercentSymbol}";
            }

            orderItem.Rate = viewModel.TotalFee.GetPreciseValue(QbConstants.MaxDecimal);

            return orderItem;
        }

        public static OrderItemViewModel ToQbInvoiceItem(this DiscountLineItemViewModel viewModel, List<DiscountLineItemViewModel> prevDiscountItems)
        {
            var orderItem = viewModel.ToQbOrderItem();
            orderItem.IsNewlyAdded = !prevDiscountItems.Any(t => t.FeeTypeId == viewModel.FeeTypeId && t.FeeSubTypeId == viewModel.FeeSubTypeId);

            return orderItem;
        }
    }
}
