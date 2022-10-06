using SiteFuel.BAL;
using SiteFuel.Models;
using SiteFuel.PricingService.Attributes;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace SiteFuel.PricingService.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif
    public class CurrencyController : ApiController
    {

        private readonly ICurrencyRateDomain _currencyRateDomain;

        public CurrencyController(ICurrencyRateDomain currencyRateDomain)
        {
            _currencyRateDomain = currencyRateDomain;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<BaseResponseModel> SaveExchangeRateAsync(SaveCurrencyRateRequestModel inputModel)
        {
            BaseResponseModel response = await _currencyRateDomain.SaveExchangeRate(inputModel.ConversionRates, inputModel.ExchangeDate);
            return response;
        }
    }
}