using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiteFuel.BAL;
using SiteFuel.DAL;
using SiteFuel.Models;

namespace SiteFuel.Pricing.TestSuite
{
    [TestClass]
    public class PricingDomainTests
    {
        [TestMethod]
        public async Task GetTerminalPriceFromAxxisAsync()
        {
            var domain = new PricingDomain(new PricingRepository());
            var result = await (domain.GetTerminalPriceFromAxxisAsync(new PriceRequestModel
            {
                PriceDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)),
                ProductId = 1,
                TerminalId = 134,
                PricingSourceId = 1
            }));

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.AvgPrice);
            Assert.AreNotEqual(0, result.LowPrice);
            Assert.AreNotEqual(0, result.HighPrice);
        }

        [TestMethod]
        public void GetTerminalPriceFromOpisAsync()
        {
            var domain = new PricingDomain(new PricingRepository());
            var result = (domain.GetTerminalPriceFromOpisAsync(new PriceRequestModel
            {
                PriceDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)),
                ProductId = 389,
                TerminalId = 1534,
                PricingSourceId = 2
            })).Result;

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Price);
        }

        [TestMethod]
        public void GetTerminalPriceFromPlattsAsync()
        {
            var domain = new PricingDomain(new PricingRepository());
            var result = (domain.GetTerminalPriceFromPlattsAsync(new PriceRequestModel
            {
                PriceDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)),
                ProductId = 579,
                TerminalId = 1568,
                PricingSourceId = 3
            })).Result;

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Price);
        }
    }
}
