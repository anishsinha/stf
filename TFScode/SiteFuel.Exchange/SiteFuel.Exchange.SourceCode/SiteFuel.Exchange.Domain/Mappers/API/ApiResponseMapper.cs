using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Collections.Generic;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class ApiResponseMapper
    {
        public static List<SalesCalculatorGridViewModel> ToGridViewModel(this SalesCalculatorApiResponse entity, List<SalesCalculatorGridViewModel> viewModel = null)
        {
            if (viewModel == null)
                viewModel = new List<SalesCalculatorGridViewModel>();

            if(entity.TerminalPrices != null && entity.TerminalPrices.Count > 0)
            {
                foreach (var item in entity.TerminalPrices)
                {
                    var gridRow = new SalesCalculatorGridViewModel();
                    gridRow.Abbreviation = item.Abbreviation;
                    gridRow.Amount = item.Amount;
                    gridRow.City = item.City;
                    gridRow.Currency = item.Currency;
                    gridRow.CurrencyName = item.CurrencyName;
                    gridRow.Distance = item.Distance;
                    gridRow.FeedTypeName = item.FeedTypeName;
                    gridRow.FuelClassTypeName = item.FuelClassTypeName;
                    gridRow.Name = item.Name;
                    gridRow.Address = item.Address;
                    gridRow.Price = item.Price;
                    gridRow.PriceAvg = item.PriceAvg;
                    gridRow.PriceHigh = item.PriceHigh;
                    gridRow.PriceLoadDate = item.PriceLoadDate;
                    gridRow.PriceLow = item.PriceLow;
                    gridRow.PriceTypeName = item.PriceTypeName;
                    gridRow.PricingDate = item.PricingDate;
                    gridRow.RackTypeName = item.RackTypeName;
                    gridRow.ReportedDate = item.ReportedDate;
                    gridRow.StateCode = item.StateCode;
                    gridRow.TaxValue = item.TaxValue;
                    gridRow.TerminalId = item.TerminalId;
                    gridRow.TotalCount = item.TotalCount;
                    gridRow.ZipCode = item.ZipCode;
                    viewModel.Add(gridRow);
                }
            }

            return viewModel;
        }

        public static MstState ToEntity(this StateViewModel viewModel, MstState entity = null)
        {
            if (entity == null)
                entity = new MstState();

            entity.Id = viewModel.Id;
            entity.Code = viewModel.Code;
            entity.Name = viewModel.Name;

            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;

            return entity;
        }
    }
}
