using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiteFuel.BAL;
using SiteFuel.DAL;
using SiteFuel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Pricing.TestSuites
{
    [TestClass]
    public class PricingRequestDomainUnitTests
    {
        private IPricingRequestDomain _pricingReqDomain;
        private PricingRequestViewModel _pricingRequestViewModel;
        private PricingCodesRequestModel _searchCodesByPricingSourceModel;
        private PricingCodesRequestModel _searchOpisCodesModel;
        private PricingRequestViewModel _pricingRequestDetailViewModel;

        [TestInitialize]
        public void Initialize()
        {
            _pricingReqDomain = new PricingRequestDomain(new PricingRequestRepository());

            _pricingRequestViewModel = new PricingRequestViewModel()
            {
                BasePrice = 1,
                BaseSupplierCost = 2,
                Currency = 3,
                ExchangeRate = 4,
                Id = 5,
                Margin = 6,
                MarginTypeId = 7,
                PricePerGallon = 8,
                PricingCodeId = 9,
                RackAvgTypeId = 10,
                SupplierCost = 11,
                SupplierCostTypeId = 12,
                UoM = 13
            };

            _searchCodesByPricingSourceModel = new PricingCodesRequestModel()
            {
                PricingSourceId = 1
            };

            _searchOpisCodesModel = new PricingCodesRequestModel()
            {
                Search = "Opis"
            };

            _pricingRequestDetailViewModel = new PricingRequestViewModel()
            {
                Id = 1
            };
        }

        [TestMethod]
        public async Task Test_SaveRequestDetails_Success()
        {
            var result = await _pricingReqDomain.SaveRequestDetails(_pricingRequestViewModel);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result > 0, "Save failed");
        }

        [TestMethod]
        public async Task Test_GetCodesByPricingSourceId_Success()
        {
            var result = await _pricingReqDomain.GetPricingCodesAsync(_searchCodesByPricingSourceModel);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.PricingCodes.Count > 0, "No codes available for Axxis");
        }

        [TestMethod]
        public async Task Test_SearchOpisCodes_Success()
        {
            var result = await _pricingReqDomain.GetPricingCodesAsync(_searchOpisCodesModel);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.PricingCodes.Count > 0, "No codes available for Opis");
        }

        [TestMethod]
        public async Task Test_GetPricingRequestDetailByIdAsync_Success()
        {
            var result = await _pricingReqDomain.GetPricingRequestDetailByIdAsync(_pricingRequestDetailViewModel);
            Assert.IsNotNull(result);
            Assert.IsTrue(result != null, "No pricing details available");
        }
    }
}
