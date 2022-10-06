using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiteFuel.BAL;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.FreightTestSuite
{
    [TestClass]
    public class FreightTableUnitTest
    {
        private FreightTableDomain domain;
        private static string _freightTableId;

        [TestInitialize]
        public void Initialize()
        {
            domain = new FreightTableDomain(new FreightTableRepository());
        }

        [TestMethod]
        public async Task ClearFreightTables()
        {
            var result = await domain.DeleteAllRecords();
            Assert.IsTrue(result, "Unable to delete all records");
        }

        [TestMethod]
        public async Task AddFreightTable()
        {
            var table = GetFreightTableModel();
            var result = await domain.AddFreightTable(table);
            Assert.IsTrue(result.StatusCode == (int)Status.Success, result.StatusMessage);
            _freightTableId = result.Id;
        }

        [TestMethod]
        public async Task AddFreightTablePricings()
        {
            var prices = new List<FreightTablePriceModel>();
            prices.Add(GetFreightTablePriceModel());
            var result = await domain.AddFreightTablePricings(prices);
            Assert.IsTrue(result.StatusCode == (int)Status.Success, result.StatusMessage);
        }

        private FreightTableModel GetFreightTableModel()
        {
            return new FreightTableModel()
            {
                CompanyId = 2,
                CreatedOn = DateTimeOffset.Now,
                Description = "All points in U.S.",
                EndDate = DateTimeOffset.Now.AddYears(1),
                FuelType = 1,
                IsActive = true,
                IsDeleted = false,
                Name = "Rhinehart Oil Tariff Table 2019",
                StartDate = DateTimeOffset.Now,
                Type = TableType.Market
            };
        }

        private FreightTablePriceModel GetFreightTablePriceModel()
        {
            var model = new FreightTablePriceModel()
            {
                CompanyId = 2,
                FreightTableId = _freightTableId,
            };
            model.FreightPrices = new List<PriceModel>();
            model.FreightPrices.Add(GetPointToPointPriceModel());
            model.FreightPrices.Add(GetDistanceRangePriceModel());
            return model;
        }

        private PointToPointPriceModel GetPointToPointPriceModel()
        {
            return new PointToPointPriceModel()
            {
                CompanyId = 2,
                CreatedOn = DateTimeOffset.Now,
                Currency = Currency.USD,
                EndPoint = "Labrador City",
                Rate = 0.013M,
                StartPoint = "Lab City",
                IsActive = true,
                IsDeleted = false
            };
        }

        private DistanceRangePriceModel GetDistanceRangePriceModel()
        {
            return new DistanceRangePriceModel()
            {
                CompanyId = 2,
                CreatedOn = DateTimeOffset.Now,
                Currency = Currency.USD,
                MaxDistance = 80,
                Rate = 0.015M,
                MinDistance = 70,
                IsActive = true,
                IsDeleted = false
            };
        }
    }
}
