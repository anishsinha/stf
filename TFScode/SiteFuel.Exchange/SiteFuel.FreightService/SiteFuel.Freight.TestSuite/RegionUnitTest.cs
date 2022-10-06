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
    public class RegionUnitTest
    {
        private RegionDomain domain;
        private static int companyId = 2;
        private static int userId = 2;
        private static string regionId = string.Empty;

        [TestInitialize]
        public void Initialize()
        {
            domain = new RegionDomain(new RegionRepository());
        }

        [TestMethod]
        public async Task ClearRegions()
        {
            var result = await domain.DeleteAllRegions();
            Assert.IsTrue(result, "Unable to delete all regions");
        }

        [TestMethod]
        public async Task CreateRegionWithoutShift()
        {
            var region = GetRegionModel();
            var result = await domain.CreateRegion(region);
            regionId = result.RegionId;
            Assert.IsTrue(result.StatusCode == (int)Status.Success, result.StatusMessage);
        }

        [TestMethod]
        public async Task UpdateRegionWithoutShift()
        {
            var region = GetRegionModel();
            region.Id = regionId;
            region.States.Add(new FreightModels.DropdownDisplayItem { Code = "CA", Name = "California" });
            var result = await domain.UpdateRegion(region);
            Assert.IsTrue(result.StatusCode == (int)Status.Success, result.StatusMessage);
        }

        [TestMethod]
        public async Task GetRegionWithoutShift()
        {
            var region = GetRegionModel();
            region.Id = regionId;
            region.States.Add(new FreightModels.DropdownDisplayItem { Code = "CA", Name = "California" });
            var result = await domain.GetRegion(companyId, regionId);
            Assert.IsTrue(result.States.Count == 4, "Failed to get region");
        }

        [TestMethod]
        public async Task CreateRegionWithShift()
        {
            var region = GetRegionModel();
            region.Name = $"{region.Name} - With Shift";
            region.Shifts = GetShiftsModel();
            var result = await domain.CreateRegion(region);
            regionId = result.RegionId;
            Assert.IsTrue(result.StatusCode == (int)Status.Success, result.StatusMessage);
        }

        private RegionViewModel GetRegionModel()
        {
            return new RegionViewModel()
            {
                Name = "Texas Retail Sites",
                Description = "Texas Retail Sites",
                CompanyId = companyId,
                States = new List<FreightModels.DropdownDisplayItem>()
                {
                    new FreightModels.DropdownDisplayItem { Code = "NM", Name = "New Mexico" }                ,
                    new FreightModels.DropdownDisplayItem { Code = "OK", Name="Oklahoma" },
                    new FreightModels.DropdownDisplayItem { Code ="TX", Name="Texas" }
                },
                CreatedOn = DateTimeOffset.Now,
                CreatedBy = userId,
                IsActive = true,
                IsDeleted = false
            };
        }

        private List<ShiftViewModel> GetShiftsModel()
        {
            var shifts = new List<ShiftViewModel>();
            shifts.Add(new ShiftViewModel
            {
                Name = "Morning Shift",
                CompanyId = companyId,
                CreatedBy = userId,
                CreatedOn = DateTimeOffset.Now,
                StartTime = TimeSpan.Parse("04:00:00").ToString(),
                EndTime = TimeSpan.Parse("16:00:00").ToString()
            });
            shifts.Add(new ShiftViewModel
            {
                Name = "Evening Shift",
                CompanyId = companyId,
                CreatedBy = userId,
                CreatedOn = DateTimeOffset.Now,
                StartTime = TimeSpan.Parse("16:00:00").ToString(),
                EndTime = TimeSpan.Parse("04:00:00").ToString()
            });
            return shifts;
        }
    }
}
