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
    public class TankUnitTest
    {
        private TankDomain domain;
        private static bool _tankStatus;

        [TestInitialize]
        public void Initialize()
        {
            domain = new TankDomain(new TankRepository());
        }

        [TestMethod]
        public async Task SaveTankDetails()
        {
            var table = GetTankDetailsModel();
            var result = await domain.SaveTankDetails(table);
            Assert.IsTrue(result, "Success");
            _tankStatus = result;
        }

        private TankDetailsModel GetTankDetailsModel()
        {
            return new TankDetailsModel()
            {
                AssetId = 45,
                TankId = "Tank ID 45",
                TankName = "Tank Name 11",
                TankNumber = "Number 22",
                Manufacturer = "Volvo 122",
                FuelCapacity = 11000,
                ThresholdDeliveryRequest = 30,
                MinFill = 10,
                FillType = FillType.Percent,
                MaxFill = 20,
                MaxFillPercent = 11,
                MinFillPercent = 5,
                PhysicalPumpStop = 15,
                WaterLevel = 100,
                RunOutLevel = 22,
                NotificationUponUsageSwing = 11,
                NotificationUponUsageSwingValue = 1222,
                NotificationUponInventorySwing = 12,
                NotificationUponInventorySwingValue = 14444,
                TankType = TankType.AboveGround,
                DipTestMethod = null,
                ManiFolded = null,
                TankConstruction = null
            };
        }

        [TestMethod]
        public async Task UpdateTankDetails()
        {
            var table = GetTankDetailsForUpdateModel();
            var result = await domain.UpdateTankDetails(table);
            Assert.IsTrue(result, "Success");
            _tankStatus = result;
        }

        private TankDetailsModel GetTankDetailsForUpdateModel()
        {
            return new TankDetailsModel()
            {
                AssetId = 46,
                TankId = "Tank ID 1234",
                TankName = "Tank Name 11 New",
                TankNumber = "Number 22",
                Manufacturer = "Volvo",
                FuelCapacity = 11000,
                ThresholdDeliveryRequest = 30,
                MinFill = null,
                FillType = FillType.Percent,
                MaxFill = null,
                MaxFillPercent = 121,
                MinFillPercent = 5,
                PhysicalPumpStop = 15,
                WaterLevel = 100,
                RunOutLevel = 22,
                NotificationUponUsageSwing = 11,
                NotificationUponUsageSwingValue = 1222,
                NotificationUponInventorySwing = 12,
                NotificationUponInventorySwingValue = 14444,
                TankType = TankType.AboveGround,
                DipTestMethod = null,
                ManiFolded = null,
                TankConstruction = null
            };
        }

        [TestMethod]
        public async Task GetTankDetails()
        {
            var result = await domain.GetTankDetails(45);
            Assert.IsTrue(result.AssetId > 0, "Tank Details Available");
        }
    }
}
