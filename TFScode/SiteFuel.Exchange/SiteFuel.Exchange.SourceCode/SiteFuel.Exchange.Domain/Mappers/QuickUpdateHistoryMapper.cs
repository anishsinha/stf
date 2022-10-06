using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class QuickUpdateHistoryMapper
    {
        public static QuickUpdateHistoryViewModel ToHistoryModel(this UspGetQuickUpdateHistory entity)
        {
            var viewModel = new QuickUpdateHistoryViewModel();
            viewModel.Id = entity.Id;
            viewModel.Nm = entity.Name;
            viewModel.OTyp = entity.OfferType;
            viewModel.Trs = entity.Tiers;
            viewModel.Cstmrs = entity.Customers;
            viewModel.FTyp = entity.FuelType;
            viewModel.Sts = entity.States;
            viewModel.Cts = entity.Cities;
            viewModel.ZpCds = entity.ZipCodes;
            viewModel.UTyp = entity.UpdateType;
            viewModel.UNm = entity.UpdateName;
            viewModel.Oprn = GetOperation(entity.MathOperationId, entity.UpdatedAmount);
            viewModel.UpdtdBy = entity.UpdatedBy;
            viewModel.UDt = entity.UpdatedDate;
            viewModel.UndoBy = entity.UndoBy;
            viewModel.UndoDt = entity.UndoDate;
            viewModel.IsVld = entity.IsValid;
            return viewModel;
        }
        private static string GetOperation(int mathOperationId, decimal updatedAmount)
        {
            string text = string.Empty;
            switch (mathOperationId)
            {
                case (int)RackPricingType.PlusDollar:
                    text = $"+ ${updatedAmount.GetPreciseValue(5)}";
                    break;
                case (int)RackPricingType.MinusDollar:
                    text = $"- ${updatedAmount.GetPreciseValue(5)}";
                    break;
                case (int)RackPricingType.PlusPercent:
                    text = $"+ {updatedAmount.GetPreciseValue(5)}%";
                    break;
                case (int)RackPricingType.MinusPercent:
                    text = $"- {updatedAmount.GetPreciseValue(5)}%";
                    break;
            }
            return text;
        }
    }
}
