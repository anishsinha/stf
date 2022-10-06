using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.SalesUser;
using SiteFuel.Exchange.ViewModels.Tank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Domain.SalesUser
{
    public class DREntryDomain : BaseDomain
    {
        public DREntryDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public DREntryDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<CustomersModel> GetCustomersAndJobList(UserContext userContext)
        {
            var response = new CustomersModel();
            try
            {
                //1. Get All Region ID for Current Company
                var freightServiceDomain = new FreightServiceDomain(this);
                var regionsXJobs = await freightServiceDomain.GetJobsForAllRegions(userContext.CompanyId);
                
                //Get Unique JobId
                var jobIds = new HashSet<int>(regionsXJobs.SelectMany(t => t.Jobs.Select(x => x.Id)).Distinct());

                //Filter Companies for Unique JobIds
                var customersAndJobs = Context.DataContext.Jobs.Where(t => jobIds.Contains(t.Id)).Select(t1 => new CustomersAndJobs { CustomerId = t1.Company.Id, CustomerName = t1.Company.Name, JobId = t1.Id, JobName = t1.Name }).ToList();

                response = new CustomersModel()
                {
                    regionsAndJobsModels = regionsXJobs,
                    customersandJobs = customersAndJobs
                };
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("DREntryDomain", "GetCustomersAndJobList", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<FuelProducts>> GetProductsForSalesDR(int CompanyId, int JobId, UserContext userContext)
        {
            var response = new List<FuelProducts>();
            try
            {
                //Case 1: where Company and JobId are null or 0 initially
                if (CompanyId == 0 || JobId == 0)
                    response = Context.DataContext.MstProducts.Where(t => t.IsActive && t.PricingSourceId == (int)PricingSource.Axxis).Select(t => new FuelProducts() { Id = t.Id, Name = t.DisplayName ?? t.Name }).OrderBy(x=> x.Name).ToList();
                else
                {
                    //Get Favorite Products 
                    var freightServiceDomain = new FreightServiceDomain(this);
                    var favoriteProductsList = await freightServiceDomain.GeFavouriteProductsByJob(JobId, userContext.CompanyId);
                    if (favoriteProductsList.Count > 0)
                    {
                        //Show all fuel products irrespective of orders
                        response = favoriteProductsList.Select(t => new FuelProducts { Id = t.Id, Name = t.Name }).OrderBy(x => x.Name).ToList();
                        //show only Favorite products for active orders
                        //response = Context.DataContext.Orders.Where(t => t.IsActive && t.AcceptedCompanyId == userContext.CompanyId && t.BuyerCompanyId == CompanyId && t.FuelRequest.JobId == JobId && favProdIds.Contains(t.FuelRequest.MstProduct.Id) && t.OrderXStatuses.Any(t1 => t1.StatusId == (int)OrderStatus.Open && t1.IsActive)).OrderByDescending(t => t.Id).Select(t => new FuelProducts() { Id = t.FuelRequest.MstProduct.Id, Name = t.FuelRequest.MstProduct.DisplayName ?? t.FuelRequest.MstProduct.Name }).Distinct().ToList();
                    }
                    else
                    {
                        //Show all the products for active orders
                        response = Context.DataContext.Orders.Where(t => t.IsActive && t.AcceptedCompanyId == userContext.CompanyId && t.BuyerCompanyId == CompanyId && t.FuelRequest.JobId == JobId && t.OrderXStatuses.Any(t1 => t1.StatusId == (int)OrderStatus.Open && t1.IsActive)).OrderByDescending(t => t.Id).Select(t => new FuelProducts() { Id = t.FuelRequest.MstProduct.Id, Name = t.FuelRequest.MstProduct.DisplayName ?? t.FuelRequest.MstProduct.Name }).Distinct().OrderBy(x => x.Name).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DREntryDomain", "GetProductsForSalesDR", ex.Message, ex);
            }
            
            return response;

        }

        public async Task<SalesUserDRValidationModel> ValidateDREntry(UserContext userContext, SalesUserDRViewModel salesUserDRViewModel)
        {
            var response = new SalesUserDRValidationModel();
            try
            {
                //Get Region ID for DR
                var productStatus = new SalesUserDRProductStatus();
                var freightServiceDomain = new FreightServiceDomain(this);
                var regionId = await freightServiceDomain.GetRegionIdForJob(salesUserDRViewModel.JobId, userContext.CompanyId);
                if (string.IsNullOrEmpty(regionId))
                {
                    productStatus.Status.State = SalesUserDRStatus.RegionNotFound;
                    productStatus.Status.Message = "No Region Exists, DR won't be created.";
                    response.ProductStatuses.Add(productStatus);
                    return response;
                }


                foreach (var item in salesUserDRViewModel.Products)
                {
                    var validatedResponse = await ValidateOrderForDR(userContext, salesUserDRViewModel, item, regionId);
                    response.ProductStatuses.Add(validatedResponse.ProductStatuses);
                    if(validatedResponse.ProductStatuses.Status.State == SalesUserDRStatus.Success && validatedResponse.RaiseDeliveryRequestInput.JobId != 0)
                        response.RaiseDeliveryRequestInput.Add(validatedResponse.RaiseDeliveryRequestInput);
                }

                         
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DREntryDomain", "ValidateDREntry", ex.Message, ex);
            }
            return response;
        }

        public async Task<DRValidationModel> ValidateOrderForDR(UserContext userContext, SalesUserDRViewModel salesUserDRViewModel, ProductDetails product, string regionId)
        {
            var response = new DRValidationModel();
            var productValidations = new SalesUserDRProductStatus();
            //productValidations.Status = new SalesUserDRStatusModel();
            //Set the product
            productValidations.Product = product;

            //Check if Order Exists for the Product
            var ordersList = Context.DataContext.Orders.Where(t => t.IsActive && t.AcceptedCompanyId == userContext.CompanyId && t.BuyerCompanyId == salesUserDRViewModel.CompanyId && t.FuelRequest.JobId == salesUserDRViewModel.JobId && t.FuelRequest.FuelTypeId == product.FuelTypeId && t.FuelRequest.UoM == product.UoM && t.OrderXStatuses.Any(t1 => t1.StatusId == (int)OrderStatus.Open && t1.IsActive)).OrderByDescending(t => t.Id).Select(t => new { orderId = t.Id, frId = t.FuelRequestId, deliveryDate = t.FuelRequest.FuelRequestDetail.StartDate, FuelName = t.FuelRequest.MstProduct.Name, ProductTypeId = t.FuelRequest.MstProduct.ProductTypeId, IsMarine = t.FuelRequest.Job.IsMarine, DisplayJobId = t.FuelRequest.Job.DisplayJobID}).ToList();
            if (ordersList == null || !ordersList.Any())
            {
                productValidations.Status.State = SalesUserDRStatus.OrderNotFound;
                productValidations.Status.Message = "No Order Exists, DR won't be created.";
                response.ProductStatuses = productValidations;
                return response;//with error msg
            }

            //Order Exists -> Populate Input model for raise delivery requests:
            var deliveryRequestInput = new RaiseDeliveryRequestInput()
            {
                //single order - then pass order id
                //if multiple orders - fetch the tank if there is single tank you have to pass tank id and storage id
                JobId = salesUserDRViewModel.JobId,
                RequiredQuantity = product.Quantity,
                UserId = userContext.Id,
                AssignedToRegionId = regionId,
                CreatedByRegionId = regionId,
                BuyerCompanyId = salesUserDRViewModel.CompanyId,
                UoM = (int)product.UoM,
                FuelTypeId = product.FuelTypeId,
                FuelType = product.FuelName,
                ScheduleQuantityType = (int)QuantityType.SpecificAmount,
                CustomerCompany = salesUserDRViewModel.CompanyName,
                //DeliveryDateStartTime = product.StartTime == null ? string.Empty: product.StartTime.ToString(),
                //WindowStartTime = product.StartTime == null ? string.Empty : product.StartTime.ToString(),
                //WindowEndTime = product.EndTime == null ? string.Empty : product.EndTime.ToString(),
                DispatcherNote = salesUserDRViewModel.DRNotes,
                Notes = salesUserDRViewModel.DRNotes,
                DeliveryLevelPO = product.DRPO,
                //WindowStartDate = product.StartDate == null ? string.Empty : product.StartDate.ToString(),
                JobName = salesUserDRViewModel.JobName,
                SupplierCompanyId = userContext.CompanyId//, // order.AcceptedCompanyId
                //IsAdditive = salesUserDRViewModel.IsAdditive
            };

            //Set Order Details
            var order = ordersList.FirstOrDefault();
            deliveryRequestInput.OrderId = order.orderId;
            //Do not Show Delivery Date Start Time, So removed it from input
            //deliveryRequestInput.DeliveryDateStartTime = order.deliveryDate.ToString();
            deliveryRequestInput.ProductTypeId = order.ProductTypeId;
            deliveryRequestInput.SiteId = order.DisplayJobId;
            //deliveryRequestInput.IsMarine = order.IsMarine;

            //Set order to Calendar if Future Date
            if(!string.IsNullOrWhiteSpace(product.StartDate) && DateTime.TryParse(product.StartDate, out DateTime selectedDate) /*&& DateTime.Compare(Convert.ToDateTime(product.StartDate), DateTime.UtcNow) > 0 && (Convert.ToDateTime(product.StartDate) - DateTime.UtcNow).TotalDays > 2*/)
            {
                deliveryRequestInput.IsFutureDR = true;
                deliveryRequestInput.IsCalendarView = true;
                deliveryRequestInput.SelectedDate = product.StartDate;
                deliveryRequestInput.ScheduleStartTime = product.StartTime;
                deliveryRequestInput.ScheduleEndTime = product.EndTime;
                //deliveryRequestInput.Priority = DeliveryReqPriority.CouldGo;
            }

            //If only Single Order -> Add Order Details for DR Input
            if (ordersList.Count() == 1)
            {
                deliveryRequestInput.DeliveryRequestFor = DeliveryRequestFor.Order;
            }
            else //Multiple orders present, search for Tanks if exists then use tanks for order creation
            {
                //search for tanks here -> If found Add FirstorDefault Tank Details else Add FirstOrDefault Order Details to DR Input
                var tanks = Context.DataContext.Assets.Where(t => t.Type == (int)AssetType.Tank && t.JobXAssets.Any(t1 => t1.JobId == salesUserDRViewModel.JobId && t1.AssetId == t.Id && t1.RemovedBy == null)).OrderByDescending(t => t.Id).Select(t => new { AssetId = t.Id, StorageId = t.AssetAdditionalDetail.VehicleId ?? string.Empty, TankId = t.AssetAdditionalDetail.Vendor ?? string.Empty, MinFill = t.AssetAdditionalDetail.MinFill, MaxFill = t.AssetAdditionalDetail.MaxFill, FuelCapacity = t.AssetAdditionalDetail.FuelCapacity, ThresholdDeliveryRequest = t.AssetAdditionalDetail.ThresholdDeliveryRequest }).ToList();
                if (tanks != null && tanks.Any())
                {

                    var tank = tanks.FirstOrDefault();
                    //Get Priority 
                    TankDetailsModel tankDetails = new TankDetailsModel()
                    {
                        MinFill = tank.MinFill,
                        MaxFill = tank.MaxFill,
                        FuelCapacity = tank.FuelCapacity,
                        TankId = tank.TankId,
                        StorageId = tank.StorageId,
                        ThresholdDeliveryRequest = tank.ThresholdDeliveryRequest,
                        JobName = salesUserDRViewModel.JobName
                    };
                    var freightServiceDomain = new FreightServiceDomain(this);
                    var priority = await freightServiceDomain.GetPriorityForSalesDR(tankDetails);
                    deliveryRequestInput.DeliveryRequestFor = DeliveryRequestFor.Tank;
                    deliveryRequestInput.TankId = tank.TankId;
                    deliveryRequestInput.StorageId = tank.StorageId;
                    deliveryRequestInput.Priority = priority;
                }
                else
                {
                    deliveryRequestInput.DeliveryRequestFor = DeliveryRequestFor.Order;
                }
            }

            productValidations.Status.State = SalesUserDRStatus.Success;
            productValidations.Status.Message = "DR input is Ready";
            response.ProductStatuses = productValidations;
            response.RaiseDeliveryRequestInput = deliveryRequestInput;
            return response;
        }

        public async Task<StatusViewModel> CreateDRFromSalesUser(UserContext userContext, List<RaiseDeliveryRequestInput> deliveryRequest)
        {
            var response = new StatusViewModel();
            try
            {
                if(deliveryRequest != null && deliveryRequest.Count() > 0)
                {
                    response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().RaiseDeliveryRequests(deliveryRequest, userContext);
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = "NO DR to Create";
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DREntryDomain", "CreateDRFromSalesUser", ex.Message, ex);
            }

            return response;
        }
    }
}
