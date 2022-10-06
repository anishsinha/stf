using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiteFuel.BAL;
using SiteFuel.DAL;
using SiteFuel.Models;
namespace SiteFuel.Pricing.TestSuites
{
	[TestClass]
	public class PricingDomainUnitTests
	{
        private PricingDomain _pricingDomain;
        private PriceRequestModel _priceRequestModelModel;
        private CityRackPricesRequestModel _cityRackPricesRequestModel;
        private SalesCalculatorRequestModel _salesCalculatorRequestModel;
        private TerminalPricesRequestModel _terminalPricesRequestModel;
        private TerminalRequestModel _terminalRequestModel;
        private PricingConfigModel _pricingConfigRequestModel;
        private int _configId = 1;

        [TestInitialize]
        public void Initialize()
        {
            _pricingDomain = new PricingDomain(new PricingRepository());

            _salesCalculatorRequestModel = new SalesCalculatorRequestModel()
            {
                BrandTypeId = 1,
                CityGroupTerminalIds = new System.Collections.Generic.List<int>() { 1482, 1585 },
                CountryCode = "USA",
                FeedTypeId = 1,
                PriceDate = DateTime.Now,
                PriceTypeId = 1,
                ProductId = 217,
                RecordCount = 5,
                SrcLatitude = 32.89477970M,
                SrcLongitude = -96.80147070M
            };

            SetTerminalPriceModels();
        }

        private void SetTerminalPriceModels()
        {
            _terminalPricesRequestModel = new TerminalPricesRequestModel()
            {
                ExternalProductId = 217,
                PricingDate = DateTime.Now.AddDays(-1),
                RecordCount = 5,
                SrcLatitude = 32.89477970M,
                SrcLongitude = -96.80147070M
            };

            _terminalRequestModel = new TerminalRequestModel()
            {
                CountryId = 1,
                PricingCodeId = 8,
                ProductId = 217,
                SearchStringTeminal = "",
                SrcLatitude = 32.89477970M,
                SrcLongitude = -96.80147070M
            };

            _priceRequestModelModel = new PriceRequestModel
            {
                PriceDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)),
                ProductId = 1,
                TerminalId = 134,
                PricingCodeId = 8
            };

            _cityRackPricesRequestModel = new CityRackPricesRequestModel()
            {
                CityTerminalPricingType = 1,
                ExternalProductId = 217,
                PriceDate = DateTime.Now,
                StateOrTerminalIds = "1482"
            };
        }

        [TestMethod]
        public async Task GetTerminalPriceFromSources()
        {
            await Test_GetTerminalPriceFromAxxisAsync_Success();
            await Test_GetTerminalPriceFromOpisAsync_Success();
            await Test_GetTerminalPriceFromPlattsAsync_Success();
        }

		private async Task Test_GetTerminalPriceFromAxxisAsync_Success()
		{
            _priceRequestModelModel.PricingCodeId = 8;
            var result = await _pricingDomain.GetTerminalPriceAsync(_priceRequestModelModel);
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Price > 0, "Price not found for Axxis");
		}

        private async Task Test_GetTerminalPriceFromOpisAsync_Success()
        {
            _priceRequestModelModel.PricingCodeId = 10;
            _priceRequestModelModel.TerminalId = 1488;
            _priceRequestModelModel.ProductId = 723;
            var result = await _pricingDomain.GetTerminalPriceAsync(_priceRequestModelModel);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Price > 0, "Price not found for opis..");
        }

        private async Task Test_GetTerminalPriceFromPlattsAsync_Success()
        {
            _priceRequestModelModel.PricingCodeId = 208;
            _priceRequestModelModel.TerminalId = 1488;
            _priceRequestModelModel.ProductId = 723;
            var result = await _pricingDomain.GetTerminalPriceAsync(_priceRequestModelModel);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Price > 0, "Price not found for platts..");
        }


        [TestMethod]
        public async Task Test_GetTerminalPricesForSalesCalculatorAsync_Axxis_Success()
        {
            _salesCalculatorRequestModel.PricingSourceId = 1; //Axxis
            var result = await _pricingDomain.GetClosestTerminalPriceAsync(_salesCalculatorRequestModel);
            Assert.IsTrue(result.Status == Exchange.Utilities.Status.Success, result.Message);
            Assert.IsTrue(result.TerminalPrices.Count > 0);
        }

        [TestMethod]
        public async Task Test_GetTerminalPricesForSalesCalculatorAsync_Opis_Success()
        {
            _salesCalculatorRequestModel.PricingSourceId = 2; //Opis
            var result = await _pricingDomain.GetOpisTerminalPricesForCalculatorAsync(_salesCalculatorRequestModel);
            Assert.IsTrue(result.Status == Exchange.Utilities.Status.Success, result.Message);
            Assert.IsTrue(result.TerminalPrices.Count > 0);
        }

        [TestMethod]
        public async Task Test_GetTerminalPricesForSalesCalculatorAsync_Platts_Success()
        {
            _salesCalculatorRequestModel.PricingSourceId = 3; //Platts
            var result = await _pricingDomain.GetPlattsTerminalPricesForCalculatorAsync(_salesCalculatorRequestModel);
            Assert.IsTrue(result.Status == Exchange.Utilities.Status.Success, result.Message);
            Assert.IsTrue(result.TerminalPrices.Count > 0);
        }

        [TestMethod]
        public async Task Test_GetCityRackTerminalPricesForCalculator_Sucess()
        {
            var result = await _pricingDomain.GetCityRackTerminalPricesForCalculator(_cityRackPricesRequestModel);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.TerminalPrices.Count > 0, "Price not found");
        }

        [TestMethod]
        public async Task Test_GetTerminalPricesForAuditAsync_Success()
        {
            var result = await _pricingDomain.GetTerminalPricesForAuditAsync(_terminalPricesRequestModel);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.TerminalPrices.Count > 0, "Price not found");
        }

        [TestMethod]
        public async Task Test_GetClosestTerminalsAsync_Success()
        {
            var result = await _pricingDomain.GetClosestTerminalsAsync(_terminalRequestModel);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Terminals.Count > 0, "Price not found");
        }

        [TestMethod]
        public async Task Test_GetPricingConfigDetailsByIdAsync_Success()
        {
            var result = await _pricingDomain.GetPricingConfigDetailsAsync(_configId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Config != null, "Pricing config setting not found for config Id => " + _configId);
        }

        [TestMethod]
        public async Task Test_GetPricingConfigDetailsAsync_Success()
        {
            var result = await _pricingDomain.GetPricingConfigDetailsAsync();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ConfigList != null, "Pricing config setting not available");
        }
    }
}
