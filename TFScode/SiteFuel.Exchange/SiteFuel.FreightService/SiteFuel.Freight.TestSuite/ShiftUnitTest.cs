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
    public class ShiftUnitTest
    {
        private ShiftDomain domain;
        private static int companyId = 2;
        private static int userId = 2;
        private static string ShiftId = string.Empty;

        [TestInitialize]
        public void Initialize()
        {
            domain = new ShiftDomain(new ShiftRepository());
        }

        //[TestMethod]
        //public async Task ClearShifts()
        //{
        //    var result = await domain.DeleteAllShifts();
        //    Assert.IsTrue(result, "Unable to delete all Shifts");
        //}

        [TestMethod]
        public void GetShiftDropDownItems()
        {
            var result = domain.GetShiftDdl(companyId);
            Assert.IsTrue(result.Count > 0, "Collection of Shifts is empty");
        }
    }
}
