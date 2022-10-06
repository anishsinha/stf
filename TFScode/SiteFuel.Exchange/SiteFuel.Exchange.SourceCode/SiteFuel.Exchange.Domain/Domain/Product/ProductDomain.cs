using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class ProductDomain : BaseDomain
    {
        public ProductDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public ProductDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<int> SaveNonStandardProduct(string nonStandardFuelName, int createdBy, Company company, int pricingSourceId = (int)PricingSource.Axxis)
        {
            var response = 0;
            var nonStandardProduct = Context.DataContext.MstProducts.FirstOrDefault(t => t.Name.ToLower() == nonStandardFuelName.ToLower() && t.ProductTypeId == (int)ProductTypes.NonStandardFuel);
            if (nonStandardProduct == null)
            {
                var pricingService = await new PricingServiceDomain(this).SaveNonStandardProduct(nonStandardFuelName, createdBy, pricingSourceId);
                if (pricingService != null && pricingService.Result > 0)
                {
                    var storedProcedureDomain = new StoredProcedureDomain(this);
                    response = await storedProcedureDomain.SaveNewProductAsync(nonStandardFuelName, pricingService.Result, (int)ProductTypes.NonStandardFuel, (int)ProductDisplayGroups.OtherFuelType, pricingSourceId);
                }
            }
            else
            {
                //if (!nonStandardProduct.Companies.Any(t => t.Id == company.Id))
                //{
                //    nonStandardProduct.Companies.Add(company);
                //}
                response = nonStandardProduct.Id;
            }
            return response;
        }

        public async Task<Status> SaveOpisPlattsProduct(List<SyncPricingResponse> pricingResponse)
        {
            var response = Status.Failed;
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                foreach (var item in pricingResponse.Where(t => t.ProductId.HasValue))
                {
                    var newProduct = Context.DataContext.MstProducts.Where(t => t.Id == item.ProductId.Value).Select(t => t.Name).FirstOrDefault();
                    if (newProduct == null)
                    {
                        var productId = await storedProcedureDomain.SaveNewProductAsync(item.ProductName, item.ProductId.Value, (int)ProductTypes.Unleaded, (int)ProductDisplayGroups.CommonFuelType, item.SourceId.Value);
                        response = Status.Success;
                    }
                    else
                    {
                        LogManager.Logger.WriteException("ProductDomain", "SaveOpisPlattsProduct", $"Product {newProduct} already exist with {item.ProductId}, {item.ProductName} insertion failed", new Exception());
                    }
                }
                await Context.CommitAsync();
                response = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ProductDomain", "SaveOpisPlattsProduct", ex.Message, ex);
            }
            return response;
        }

        public async Task<Status> SaveActualOpisProduct(List<SyncPricingResponse> pricingResponse)
        {
            var response = Status.Failed;
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                foreach (var item in pricingResponse.Where(t => t.ProductId.HasValue))
                {
                    var newProduct = Context.DataContext.MstOPISProducts.Where(t => t.Id == item.ProductId.Value).Select(t => t.Name).FirstOrDefault();
                    if (newProduct == null)
                    {
                        var productResponse = await storedProcedureDomain.SaveNewOPISProductAsync(
                                                                             item.ProductName, item.ProductId.Value,
                                                                             item.SourceId.Value, item.ProductCode,
                                                                             item.TfxProductId,item.MstProductId
                                                                             );
                        response = Status.Success;
                    }
                    else
                    {
                        LogManager.Logger.WriteException("ProductDomain", "SaveActualOpisProduct", $"Product {newProduct} already exist with {item.ProductId}, {item.ProductName} insertion failed", new Exception());
                    }
                }
                await Context.CommitAsync();
                response = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ProductDomain", "SaveActualOpisProduct", ex.Message, ex);
                response = Status.Failed;
            }
            return response;
        }

        public async Task<StatusViewModel> SaveAdditiveBlendingProduct(AdditiveProductDetailsViewModel productVM, UserContext userContext, int pricingSourceId = (int)PricingSource.Axxis)
        {
            // var response = productVM.Id;
            var response = new StatusViewModel();
            if (productVM.Id > 0)
            {
                //update product name in Exchange and pricing - EDIT OR DELETE
                var nonStandardProduct = Context.DataContext.MstProducts.SingleOrDefault(t => t.Id == productVM.Id);
                if (!productVM.IsDeleted && !string.IsNullOrWhiteSpace(productVM.AdditiveProductName))
                {
                    var existingProduct = Context.DataContext.MstProducts
                                          .FirstOrDefault(t => t.IsActive && t.CompanyId == userContext.CompanyId && t.Name.Trim().ToLower() == productVM.AdditiveProductName.Trim().ToLower() && t.ProductTypeId == (int)ProductTypes.Additives);
                    if (existingProduct !=null )
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageProductAlreadyExists;
                        return response;
                    }

                }
                if (nonStandardProduct != null)
                {
                        
                        if (productVM.IsDeleted)
                        {
                            if (await IsOpenOrderExists(productVM.Id, userContext.CompanyId))
                            {
                               response.StatusCode = Status.Failed; // open order exists 
                               response.StatusMessage = Resource.errMessageOpenOrderExists;
                               return response;
                            }                            
                        }
                        productVM.AdditiveProductName = string.IsNullOrWhiteSpace(productVM.AdditiveProductName) ? nonStandardProduct.DisplayName : productVM.AdditiveProductName;
                        var pricingService = await new PricingServiceDomain(this).UpdateNonStandardProduct(productVM);
                        if (pricingService != null && pricingService.Result > 0)
                        {
                            var existingProduct = Context.DataContext.MstProducts.SingleOrDefault(t => t.Id == productVM.Id);
                            if (existingProduct != null)
                            {
                                existingProduct.Name = productVM.AdditiveProductName;
                                existingProduct.DisplayName = productVM.AdditiveProductName;
                                if (productVM.IsDeleted)
                                        existingProduct.IsActive = false;

                                Context.DataContext.Entry(existingProduct).State = EntityState.Modified;
                                await Context.CommitAsync();

                                response.StatusCode = Status.Success;
                                response.StatusMessage = productVM.IsDeleted ? Resource.successMsgAdditiveProductDelete : Resource.successMsgAdditiveProductUpdate;
                            }
                        }
                        else
                        {
                            response.StatusCode  = Status.Failed; //Call failed
                            response.StatusMessage = Resource.errMessageProductFailedToSave;
                        }
                }
            }
            else
            {
                var nonStandardProduct = Context.DataContext.MstProducts
                            .FirstOrDefault(t => t.IsActive && t.CompanyId == userContext.CompanyId && t.Name.ToLower() == productVM.AdditiveProductName.ToLower() && t.ProductTypeId == (int)ProductTypes.Additives);
                if (nonStandardProduct == null)
                {
                    var pricingService = await new PricingServiceDomain(this).SaveNonStandardProduct(productVM.AdditiveProductName, userContext.Id, pricingSourceId, userContext.CompanyId);
                    if (pricingService != null && pricingService.Result > 0)
                    {
                        var storedProcedureDomain = new StoredProcedureDomain(this);
                        var productId = await storedProcedureDomain.SaveNewProductAsync(productVM.AdditiveProductName, pricingService.Result, (int)ProductTypes.Additives, (int)ProductDisplayGroups.AdditiveFuelType, pricingSourceId,userContext.CompanyId);
                        if (productId > 0)
                        {
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMsgAdditiveProductAdded;
                        }
                        else
                        {
                            response.StatusCode = Status.Failed; //Call failed
                            response.StatusMessage = Resource.errMessageProductFailedToSave;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed; //Call failed
                        response.StatusMessage = Resource.errMessageProductFailedToSave;
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageProductAlreadyExists;                    
                }
                    
            }            
            return response;
        }

        public async Task<bool> IsOpenOrderExists(int mstproductId, int companyId)
        {
            var isExists = false;
            try
            {
                isExists = await Context.DataContext.Orders.AnyAsync(t => (t.AcceptedCompanyId == companyId && t.FuelRequest.FuelTypeId == mstproductId && t.IsActive)

                           && (t.OrderXStatuses.Any(t1 => t1.IsActive && t1.StatusId == (int)OrderStatus.Open)));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "IsOpenOrderExists", ex.Message, ex);
            }
            return isExists;
        }
    }
}
