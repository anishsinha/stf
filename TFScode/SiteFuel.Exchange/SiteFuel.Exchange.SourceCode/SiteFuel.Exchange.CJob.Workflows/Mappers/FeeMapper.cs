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
    public static class FeeMapper
    {
        public static OrderItemViewModel ToQbOrderItem(this FeesViewModel viewModel, decimal gallonsOrdered)
        {
            var orderItem = new OrderItemViewModel();
            int.TryParse(viewModel.FeeTypeId, out int feeTypeId);

            orderItem.Name = viewModel.FeeTypeName.ToQbItemName(feeTypeId);
            orderItem.Desc = $"{viewModel.FeeTypeName} - {viewModel.FeeSubTypeName}";
            if (feeTypeId == (int)FeeType.OtherFee)
            {
                orderItem.Name = viewModel.OtherFeeDescription.ToQbItemName(viewModel.OtherFeeTypeId ?? feeTypeId);
                orderItem.Desc = $"{viewModel.OtherFeeDescription} - {viewModel.FeeSubTypeName}";
            }
            if (feeTypeId == (int)FeeType.TankRental)
            {
                orderItem.Desc = viewModel.OtherFeeDescription;
            }
            orderItem.Rate = viewModel.Fee.Value.GetPreciseValue(QbConstants.MaxDecimal);
            orderItem.Quantity = 1;
            if (viewModel.FeeSubTypeId == (int)FeeSubType.PerGallon)
            {
                orderItem.Quantity = gallonsOrdered.GetPreciseValue(QbConstants.MaxDecimal);
            }
            else if (viewModel.FeeSubTypeId == (int)FeeSubType.Percent)
            {
                orderItem.Rate = viewModel.TotalFee.GetPreciseValue(QbConstants.MaxDecimal);
                orderItem.Desc += $" ({viewModel.Fee.GetPreciseValue(QbConstants.MaxDecimal)}%)";
            }
            return orderItem;
        }
        
        public static OrderItemViewModel ToQbInvoiceItem(this FeesViewModel viewModel, List<FeesViewModel> FrFees)
        {
            var orderItem = new OrderItemViewModel();
            var gallonsDropped = viewModel.DroppedGallons;
            var isCommonFee = int.TryParse(viewModel.FeeTypeId, out int feeTypeId);

            orderItem.Name = viewModel.FeeTypeName.ToQbItemName(feeTypeId);
            orderItem.Desc = $"{viewModel.FeeTypeName} - {viewModel.FeeSubTypeName}";
            if (!isCommonFee || feeTypeId == (int)FeeType.OtherFee)
            {
                orderItem.Name = viewModel.OtherFeeDescription.ToQbItemName(viewModel.OtherFeeTypeId ?? 0);
                orderItem.Desc = $"{viewModel.OtherFeeDescription} - {viewModel.FeeSubTypeName}";
            }
            if (feeTypeId == (int)FeeType.TankRental)
            {
                orderItem.Desc = viewModel.OtherFeeDescription;
            }
            orderItem.Quantity = 1;
            orderItem.Rate = viewModel.Fee.Value.GetPreciseValue(QbConstants.MaxDecimal);
            if (viewModel.FeeTypeId == ((int)FeeType.SurchargeFreightFee).ToString())
            {
                orderItem.Rate = Math.Abs(viewModel.TotalFee).GetPreciseValue(QbConstants.MaxDecimal);
            }
            else if (viewModel.FeeSubTypeId == (int)FeeSubType.PerGallon)
            {
                orderItem.Quantity = Math.Abs(gallonsDropped).GetPreciseValue(QbConstants.MaxDecimal);
            }
            else if (viewModel.FeeSubTypeId == (int)FeeSubType.ByQuantity)
            {
                var feeByQuantity = viewModel.DeliveryFeeByQuantity.FirstOrDefault(t => gallonsDropped >= t.MinQuantity && gallonsDropped <= (t.MaxQuantity ?? gallonsDropped));
                if (feeByQuantity != null)
                {
                    orderItem.Rate = feeByQuantity.Fee.GetPreciseValue(QbConstants.MaxDecimal);
                    orderItem.Desc += $" From {feeByQuantity.MinQuantity} gallons to {feeByQuantity.MaxQuantity} gallons";
                }
            }
            else if (viewModel.FeeSubTypeId == (int)FeeSubType.ByAssetCount)
            {
                orderItem.Quantity = Math.Abs(viewModel.FeeSubQuantity ?? 0).GetPreciseValue(QbConstants.MaxDecimal);
            }
            else if (viewModel.FeeSubTypeId == (int)FeeSubType.HourlyRate)
            {
                orderItem.Quantity = 1;
                orderItem.Rate = Math.Abs(viewModel.TotalFee).GetPreciseValue(QbConstants.MaxDecimal);
                orderItem.Desc += $" ({viewModel.TotalHours})";
            }
            else if (viewModel.FeeSubTypeId == (int)FeeSubType.Percent)
            {
                orderItem.Rate = Math.Abs(viewModel.TotalFee).GetPreciseValue(QbConstants.MaxDecimal);
                orderItem.Desc += $" ({viewModel.Fee.GetPreciseValue(QbConstants.MaxDecimal)}%)";
            }
            orderItem.IsNewlyAdded = !FrFees.Any(t => t.FeeTypeId == viewModel.FeeTypeId && t.FeeSubTypeId == viewModel.FeeSubTypeId
                                      && t.OtherFeeDescription == viewModel.OtherFeeDescription && t.OtherFeeTypeId == viewModel.OtherFeeTypeId);
            return orderItem;
        }
    }
}
