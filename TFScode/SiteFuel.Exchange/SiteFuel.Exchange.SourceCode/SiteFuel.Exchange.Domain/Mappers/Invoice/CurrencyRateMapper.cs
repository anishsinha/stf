using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.AvaTaxExciseWebService;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class CurrencyRateMapper
    {
        public static CurrencyRateViewModel ToViewModel(this CurrencyRate entity)
        {
            var viewModel = new CurrencyRateViewModel();

            viewModel.FromCurrency = entity.FromCurrency;
            viewModel.ToCurrency = entity.ToCurrency;
            viewModel.ExchangeRate = entity.ExchangeRate;
            viewModel.CreatedDate = entity.CreatedDate;

            return viewModel;
        }

        public static TransactionExchangeRate_5_27_0 ToAvalaraViewModel(this CurrencyRateViewModel viewModel)
        {
            var avaViewModel = new TransactionExchangeRate_5_27_0();

            avaViewModel.FromCurrency = viewModel.FromCurrency;
            avaViewModel.ToCurrency = viewModel.ToCurrency;
            avaViewModel.ConversionFactor = viewModel.ExchangeRate;
            avaViewModel.EffectiveDate = viewModel.CreatedDate.DateTime;

            return avaViewModel;
        }
    }
}
