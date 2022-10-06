using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiteFuel.BAL;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository;
using System;
using System.Collections.Generic;

namespace SiteFuel.Freight.TestSuite
{
    [TestClass]
    public class DeliveryRequestUnitTest
    {
        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod]
        public async void CreateDemandsTests()
        {
            var domain = new DeliveryRequestDomain(new DeliveryRequestRepository());

            var deliveryRequests = new List<DeliveryRequestViewModel>();
            deliveryRequests.Add(new DeliveryRequestViewModel() 
            {
                CreatedBy = 1,
                CreatedByCompanyId = 2,
                CreatedOn = DateTimeOffset.Now,
                CurrentQuantity = 4,
                ProductTypeId = 5,
                IsActive = true,
                IsDeleted = false,
                Priority =  Exchange.Utilities.DeliveryReqPriority.MustGo,
                RequiredQuantity = 2020,
                SiteId = "201",
                Status = Exchange.Utilities.DeliveryReqStatus.Pending,
                StorageId = "201",
                TankId = "201"
            });
            deliveryRequests.Add(new DeliveryRequestViewModel()
            {
                CreatedBy = 1,
                CreatedByCompanyId = 2,
                CreatedOn = DateTimeOffset.Now,
                CurrentQuantity = 5,
                ProductTypeId = 6,
                IsActive = true,
                IsDeleted = false,
                Priority = Exchange.Utilities.DeliveryReqPriority.ShouldGo,
                RequiredQuantity = 2020,
                SiteId = "202",
                Status = Exchange.Utilities.DeliveryReqStatus.Pending,
                StorageId = "202",
                TankId = "202"
            });
            var result = await domain.CreateDeliveryRequest(deliveryRequests);
            Assert.IsTrue(result.StatusCode == (int)Status.Success);
        }
    }
}
