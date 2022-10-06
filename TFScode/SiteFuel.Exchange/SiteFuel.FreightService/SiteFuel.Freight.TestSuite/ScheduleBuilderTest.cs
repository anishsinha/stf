using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiteFuel.BAL;
using SiteFuel.FreightRepository;
using System.Threading.Tasks;

namespace SiteFuel.Freight.TestSuite
{
    [TestClass]
    public class ScheduleBuilder
    {
        private ScheduleBuilderDomain domain;

        [TestInitialize]
        public void Initialize()
        {
            domain = new ScheduleBuilderDomain(new ScheduleBuilderRepository());
        }

        [TestMethod]
        public async Task GetScheduleBuilderDetails()
        {
            var result = await domain.GetScheduleBuilderDetails(3050, 4470, "5e46973ba395990df86256bf", "02/17/2020", 1, "");
            Assert.IsTrue(result.StatusCode == 0, "Success");
        }

        [TestMethod]
        public async Task GetRegions()
        {           
            var result = await domain.GetRegions(79);
            Assert.IsTrue(result.Count > 0, "Success");
        }

        [TestMethod]
        public async Task GetRegionDetails()
        {
            var result = await domain.GetRegionDetails("5e33f825a395990858bc9971");
            Assert.IsTrue(result.StatusCode == 0, "Success");
        }       
    }
}
