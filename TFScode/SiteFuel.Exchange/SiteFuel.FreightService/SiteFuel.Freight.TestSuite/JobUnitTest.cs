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
    public class JobUnitTest
    {
        private JobDomain domain;

        [TestInitialize]
        public void Initialize()
        {
            domain = new JobDomain(new JobRepository());
        }

        [TestMethod]
        public async Task RemoveJobAdditionalDetails()
        {
            var inputModel = GetAdditionalJobDetailModel();
            var result = await domain.RemoveJobAdditionalDetails(inputModel.JobId);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task SaveAdditionalJobDetails()
        {
            var inputModel = GetAdditionalJobDetailModel();
            var result = await domain.SaveAdditionalJobDetails(inputModel);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GetJobAdditionalDetails()
        {
            var expectedResponse = GetAdditionalJobDetailModel();
            var result = await domain.GetAdditionalJobDetails(expectedResponse.JobId);
            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.AdditionalImageDescription, expectedResponse.AdditionalImageDescription);
            Assert.AreEqual(result.AdditionalImageFilePath, expectedResponse.AdditionalImageFilePath);
            Assert.AreEqual(result.DeliveryDays.Count, expectedResponse.DeliveryDays.Count);
            Assert.AreEqual(result.IsActive, expectedResponse.IsActive);
            Assert.AreEqual(result.JobId, expectedResponse.JobId);
            Assert.AreEqual(result.SiteImageFilePath, expectedResponse.SiteImageFilePath);
            Assert.AreEqual((result.ToDeliveryTime - result.FromDeliveryTime).TotalHours, 1);
        }

        [TestMethod]
        public async Task UpdateAdditionalJobDetails()
        {
            var updatedFilePath = "2-1-637104517386109065.pdf";
            var inputModel = GetAdditionalJobDetailModel();
            inputModel.SiteImageFilePath = updatedFilePath;
            var result = await domain.UpdateAdditionalJobDetails(inputModel);
            Assert.IsTrue(result);
            
        }
        
        private JobAdditionalDetailsModel GetAdditionalJobDetailModel()
        {
            var datetime = DateTimeOffset.Now;
            var response = new JobAdditionalDetailsModel()
            {
                AdditionalImageDescription = "TestImageDescription",
                AdditionalImageFilePath = "2-637104517386697474-2-637104517386109065.pdf",
                SiteImageFilePath = "1-637104517386109065.pdf",
                FromDeliveryTime = datetime,
                ToDeliveryTime = datetime.AddHours(1),
                JobId = 99999999,
                DeliveryDays = new List<int>(),
                IsActive = true
            };
            response.DeliveryDays.Add(1);
            response.DeliveryDays.Add(4);
            return response;
        }
    }
}
