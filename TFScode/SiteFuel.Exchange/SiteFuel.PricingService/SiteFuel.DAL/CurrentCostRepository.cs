using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Models;
using SiteFuel.Exchange.Utilities;
using SiteFuel.DataAccess.Entities;
using SiteFuel.Exchange.Logger;
using System.Data.Entity;

namespace SiteFuel.DAL
{
    public class CurrentCostRepository : ICurrentCostRepository
    {
        private DataContext dbContext;
        public CurrentCostRepository()
        {
            dbContext = new DataContext();
        }

        public async Task<CurrentCostResponseModel> UpdateSupplierCostToPriceDetail(CurrentCostRequestModel requestModel)
        {
            var response = new CurrentCostResponseModel();
            try
            {
               // var requestPriceDetails = await dbContext.RequestPriceDetails.Where(t => requestModel.RequestPriceDetailIds.Contains(t.Id)).ToListAsync();
                var requestPriceDetails = await dbContext.PricingDetails.Where(t => requestModel.RequestPriceDetailIds.Contains(t.RequestPriceDetailId) && t.IsActive).ToListAsync();
                if (requestPriceDetails.Any())
                {
                    foreach (var item in requestPriceDetails)
                    {
                        var originalPricing = new CostResponseModel();
                        originalPricing.previousCost = item.SupplierCost ?? 0;
                        originalPricing.previousCostType = item.SupplierCostTypeId ?? 0;
                        //originalPricing.PriceDetailId = item.Id;
                        originalPricing.PriceDetailId = item.RequestPriceDetailId;
                        response.Cost.Add(originalPricing);

                        //update pricing
                        item.SupplierCostTypeId = requestModel.SupplierCostType;
                        item.SupplierCost = requestModel.Cost;
                        item.BaseSupplierCost = requestModel.Cost;
                        dbContext.Entry(item).State = EntityState.Modified;
                    }
                    await dbContext.SaveChangesAsync();
                    response.Status = Status.Success;
                }
            }
            catch (Exception ex)
            {
                response.Status = Status.Failed;
                LogManager.Logger.WriteException("CurrentCostRepository", "UpdateSupplierCostToPriceDetail", ex.Message, ex);
            }
            return response;
        }
    }
}
