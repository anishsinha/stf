using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.DispatchScheduler;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using SiteFuel.Exchange.ViewModels.OttoSchedule;
using SiteFuel.Exchange.ViewModels.RouteInfo;
using SiteFuel.Exchange.ViewModels.ScheduleBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class ScheduleBuilderDomain : DeliveryReqJobInfoDomain
    {
        public ScheduleBuilderDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public ScheduleBuilderDomain(string connectionString) : base(connectionString)
        {
        }

        public ScheduleBuilderDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public async Task<List<DropdownDisplayExtended>> GetRegions(int userId)
        {
            List<DropdownDisplayExtended> response = new List<DropdownDisplayExtended>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetRegionsForDispatcher, userId);
                response = await ApiGetCall<List<DropdownDisplayExtended>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetRegions", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CheckAndLockDRs(List<string> drIds, DropdownDisplayItem user)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlCheckAndLockDrs, new { DrIds = drIds, User = user });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "CheckAndLockDRs", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CheckAndReleaseDRs(List<string> drIds, DropdownDisplayItem user)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlCheckAndReleaseDrs, new { DrIds = drIds, User = user });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "CheckAndReleaseDRs", ex.Message, ex);
            }
            return response;
        }


        public async Task<CreateDrPreferences> GetCreateDrSetting(int companyId)
        {
            CreateDrPreferences response = new CreateDrPreferences();
            try
            {

                var onboardingPreference = await Context.DataContext.OnboardingPreferences
                                                .Where(t => t.IsActive && t.CompanyId == companyId)
                                                .Select(t => new { t.Id, t.IsAdditiveBlendingEnabled, t.CreditCheckType, t.IsLoadOptimization })
                                               .OrderByDescending(t => t.Id)
                                               .FirstOrDefaultAsync();
                if (onboardingPreference != null)
                {
                    response.CreditCheckType = onboardingPreference.CreditCheckType;
                    response.IsAdditiveBlendingEnabled = onboardingPreference.IsAdditiveBlendingEnabled;
                    response.IsLoadOptimization = onboardingPreference.IsLoadOptimization;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetCreateDrSetting", ex.Message, ex);
            }
            return response;
        }


        public async Task<RegionDetailViewModel> GetRegionDetails(string regionId)
        {
            RegionDetailViewModel response = new RegionDetailViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetRegionDetails, regionId);
                response = await ApiGetCall<RegionDetailViewModel>(apiUrl);
                await SetRegionDriversOnboardingStatus(response.Drivers);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetRegionDetails", ex.Message, ex);
            }
            return response;
        }

        private async Task SetRegionDriversOnboardingStatus(List<DriverAdditionalDetailsViewModel> drivers)
        {
            try
            {
                if (drivers.Any())
                {
                    var ids = drivers.Select(t => t.Id).ToList();
                    var users = await Context.DataContext.Users.Where(t => ids.Contains(t.Id) && t.Company.IsActive
                                             && !t.IsActive && !t.IsOnboardingComplete)
                                             .Select(t => new { t.Id, t.FirstName, t.LastName, t.IsOnboardingComplete, t.IsEmailConfirmed }).ToListAsync();
                    foreach (var user in users)
                    {
                        var driverRecord = drivers.Find(t => t.Id == user.Id);
                        if (driverRecord != null)
                        {
                            if (!user.IsOnboardingComplete)
                            {
                                if (user.IsEmailConfirmed)
                                {
                                    driverRecord.Name = driverRecord.Name + "-" + Resource.lblEmailVerified;
                                }
                                else
                                {
                                    driverRecord.Name = driverRecord.Name + "-" + Resource.headingInvited;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "setDriverOnboardingStatus", ex.Message, ex);
            }
        }

        public async Task<CreateDeliveryReqForNonRetailModel> GetOrdersForJobOfCustomerAndSupplier(UserContext userContext, int buyerCompanyId, int jobId, string regionId, bool skipMarineConversion, int endSupplier = 0, string productsToExclude = "")
        {
            var response = new CreateDeliveryReqForNonRetailModel();
            try
            {
                List<int> _productsToExclude = new List<int>();
                if (productsToExclude != "")
                {
                    _productsToExclude = productsToExclude.Split(',').Select(int.Parse).ToList();
                }

                var freightServiceDomain = new FreightServiceDomain(this);
                List<int> jobIds = new List<int>();
                if (jobId == 0)
                {
                    if (!string.IsNullOrWhiteSpace(regionId))
                    {
                        var jobs = await freightServiceDomain.GetJobListForCarrier(regionId, userContext);
                        jobIds = jobs.SelectMany(t => t.Jobs.Select(t1 => t1.Id)).Distinct().ToList();
                    }
                }
                else
                {
                    jobIds.Add(jobId);
                }
                // get favFueltypes of the region
                var regFavProductTypes = new List<int>();
                var regFavTfxFuelTypes = new List<int>();
                var favProducts = await freightServiceDomain.GetRegionFavouriteProducts(jobId, regionId, userContext.CompanyId);
                if (favProducts != null)
                {
                    if (favProducts.TfxFavProductTypeId == RegionFavProductType.ProductType && favProducts.TfxProductTypeIds != null && favProducts.TfxProductTypeIds.Any())
                    {
                        regFavProductTypes = favProducts.TfxProductTypeIds;
                    }
                    else if (favProducts.TfxFavProductTypeId == RegionFavProductType.FuelType && favProducts.TfxFuelTypeIds != null && favProducts.TfxFuelTypeIds.Any())
                    {
                        regFavTfxFuelTypes = favProducts.TfxFuelTypeIds.Select(t => t.Id).Distinct().ToList();
                    }
                }
                StoredProcedureDomain spDomain = new StoredProcedureDomain(this);
                var spResponse = await spDomain.GetOrdersForJobOfCustomerAndSupplier(userContext.CompanyId, buyerCompanyId, endSupplier == 0 ? false : true, skipMarineConversion, jobIds, _productsToExclude, regFavTfxFuelTypes, regFavProductTypes);
                if (spResponse != null)
                {
                    if (spResponse.OrderList != null)
                    {
                        response.OrderList = spResponse.OrderList;
                    }
                    if (spResponse.DeliveryReqInput != null)
                    {
                        response.DeliveryReqInput = spResponse.DeliveryReqInput;
                    }
                    if (spResponse.OrderPickupDetails != null)
                    {
                        Parallel.ForEach(response.DeliveryReqInput, item =>
                        {
                            var pickup = spResponse.OrderPickupDetails.FirstOrDefault(t => t.OrderId == item.OrderId);
                            if (pickup != null)
                            {
                                item.OrderPickupDetails.Add(pickup);
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetOrdersForJobOfCustomerAndSupplier", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> GetDeliveryReqDemands(GroupDeliveryRequests input, UserContext user)
        {
            var response = new StatusViewModel() { StatusCode = Status.Success };
            try
            {
                if (input.DeliveryReqs != null)
                {
                    var freightDomain = new FreightServiceDomain(this);
                    var validateDRs = input.ExistingDrIds;
                    var validateDeliveryRequestInUse = await freightDomain.ValidateDeliveryRequestInUse(validateDRs);
                    if (validateDeliveryRequestInUse.StatusCode == Status.Failed)
                    {
                        validateDeliveryRequestInUse.StatusMessage = Resource.valMessageDelReqInUseScheduleBuilder;
                        return validateDeliveryRequestInUse;
                    }
                    var drs = input.DeliveryReqs.Where(t => !t.isRecurringSchedule && (t.CarrierStatus == 0 || t.CarrierStatus == 3 || t.CarrierStatus == 4)).ToList();
                    if (input.ExistingDrIds != null && input.ExistingDrIds.Any())
                    {

                        var inputToReCreate = new ReCreateDeliveryRequestsViewModel() { ExistingDrIds = input.ExistingDrIds, DeliveryRequests = drs };
                        var statusModel = await freightDomain.ReCreateDeliveryRequests(inputToReCreate, user);
                        if (statusModel.StatusCode != Status.Success)
                        {
                            return statusModel;
                        }
                    }
                    var locations = input.DeliveryReqs.Select(t => t.JobId).Distinct().ToList();
                    var activeLocations = await Context.DataContext.Jobs.Where(t => locations.Contains(t.Id) && t.IsActive).Select(t => new { t.Id, t.IsRetailJob }).ToListAsync();
                    List<int> activeLocationIds = activeLocations.Select(t => t.Id).ToList();
                    input.DeliveryReqs = input.DeliveryReqs.Where(t => activeLocationIds.Contains(t.JobId) || t.IsTBD).ToList();
                    var orderIds = input.DeliveryReqs.Where(t => t.OrderId > 0).Select(t => t.OrderId ?? 0).ToList();
                    bool isRetailJob = activeLocations.Any(t => t.IsRetailJob);
                    if (isRetailJob || orderIds.Any(t => t != 0))
                    {
                        var tanks = await Context.DataContext.JobXAssets.Where(t => activeLocationIds.Contains(t.JobId) && t.Asset.Type == (int)AssetType.Tank && t.RemovedBy == null && t.RemovedDate == null).Select(t => new { t.Asset.AssetAdditionalDetail.MaxFill, t.Asset.AssetAdditionalDetail.FillType, t.Asset.AssetAdditionalDetail.FuelCapacity, t.Asset.FuelType, t.JobId, TankId = t.Asset.AssetAdditionalDetail.VehicleId, StorageId = t.Asset.AssetAdditionalDetail.Vendor }).ToListAsync();

                        var orders = await Context.DataContext.Orders.Where(t => orderIds.Contains(t.Id) && t.OrderXStatuses.Any(t1 => t1.StatusId == (int)OrderStatus.Open && t1.IsActive)).Select(t => new { t.Id, t.FuelRequest.MstProduct.ProductTypeId }).ToListAsync();
                        foreach (var dr in input.DeliveryReqs)
                        {
                            if (!orders.Any(t => t.Id == dr.OrderId))
                            {
                                dr.OrderId = null;
                            }
                            var drTanks = tanks.Where(t => t.JobId == dr.JobId && t.FuelType == dr.ProductTypeId).ToList();
                            decimal tankMaxFill = 0;
                            foreach (var tank in drTanks)
                            {
                                if (tank.FillType == (int)FillType.UoM)
                                {
                                    tankMaxFill += tank.MaxFill ?? 0;
                                }
                                else
                                {
                                    tankMaxFill += (tank.MaxFill.HasValue && tank.FuelCapacity.HasValue) ? (tank.MaxFill.Value * (tank.FuelCapacity ?? 0) / 100) : 0;
                                }
                            }
                            dr.TankMaxFill = tankMaxFill;
                        }
                    }
                    input.DeliveryReqs = input.DeliveryReqs.Where(t => !t.IsDeleted).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetDeliveryReqDemands", ex.Message, ex);
            }
            return response;
        }

        public async Task<CreateDeliveryReqForNonRetailModel> GetOrdersForJob(int buyerCompanyId, int jobId)
        {
            var response = new CreateDeliveryReqForNonRetailModel();
            var orders = await Context.DataContext.Orders
                        .Where(t => t.BuyerCompanyId == buyerCompanyId
                                    //&& t.IsEndSupplier
                                    && t.AcceptedCompanyId != 0
                                    && t.IsActive
                                    && (jobId == 0 || (t.FuelRequest.JobId == jobId && t.FuelRequest.Job.IsActive))
                                    && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                    && (!t.FuelRequest.FuelRequestDetail.IsDispatchRetainedByCustomer || !t.IsEndSupplier))
                                .Select(t => new
                                {
                                    t.Id,
                                    t.PoNumber,
                                    t.FuelRequest.Job.DisplayJobID,
                                    t.FuelRequest.JobId,
                                    t.FuelRequest.Job.UoM,
                                    JobName = t.FuelRequest.Job.Name,
                                    CustomerCompanyName = t.BuyerCompany.Name,
                                    CustomerCompanyId = t.BuyerCompanyId,
                                    t.FuelRequest.Job.Address,
                                    t.FuelRequest.Job.City,
                                    t.FuelRequest.Job.MstState.Code,
                                    t.FuelRequest.Job.ZipCode,
                                    ProductType = t.FuelRequest.MstProduct.MstProductType.Name,
                                    FuelType = t.FuelRequest.MstProduct.DisplayName ?? t.FuelRequest.MstProduct.Name,
                                })
                                .Distinct()
                                .ToListAsync();
            orders.ForEach(t => response.OrderList.Add(new DropdownDisplayItem()
            {
                Id = t.Id,
                Name = $"{t.FuelType} - {t.PoNumber}"
            }));
            orders.ForEach(t => response.DeliveryReqInput.Add(new DemandModel()
            {
                OrderId = t.Id,
                SiteId = t.DisplayJobID,
                JobId = t.JobId,
                UoM = t.UoM.ToString(),
                JobName = $"{t.JobName} - {t.Address}, {t.City}, {t.ZipCode}, {t.Code}",
                BuyerCompanyId = t.CustomerCompanyId,
                BuyerCompanyName = t.CustomerCompanyName,
                ProductType = t.ProductType,
                ProductName = $"{t.FuelType} - {t.PoNumber}",
                PoNumber = t.PoNumber,
                Priority = DeliveryReqPriority.MustGo,

            }));
            return response;
        }
        public async Task<List<OrderPickupDetailModel>> GetOrderDetails(int jobId, int productTypeId, DateTimeOffset? loadDate, UserContext userContext, int carrierStatus = -1, List<TfxCarrierDropdownDisplayItem> carrier = null)
        {
            List<OrderPickupDetailModel> response = new List<OrderPickupDetailModel>();
            try
            {
                List<int> companyIds = new List<int>() { userContext.CompanyId };
                //if (userContext.IsCarrierCompany || userContext.IsBuyerSupplierAndCarrierCompany || userContext.IsSupplierAndCarrierCompany)
                //{
                //    var apiUrl = string.Format(ApplicationConstants.UrlGetSupplierCompanyList, jobId, userContext.CompanyId);
                //    var supplierCompaniesForCarrier = await ApiGetCall<List<int>>(apiUrl);
                //    companyIds.AddRange(supplierCompaniesForCarrier);
                //} //Because we will show only freightonly order to the carrier -- discussed with rajeev

                var mappedToProductTypeIds = new List<int>();
                mappedToProductTypeIds.Add(productTypeId);
                var productTypeMappings = Context.DataContext.ProductTypeCompatibilityMappings.Where(t => t.ProductTypeId == productTypeId).Select(t => t.MappedToProductTypeId).ToList();
                if (productTypeMappings != null)
                {
                    mappedToProductTypeIds.AddRange(productTypeMappings);
                }

                var supplierOrders = await Context.DataContext.Orders.Where(t => companyIds.Contains(t.AcceptedCompanyId) && t.FuelRequest.JobId == jobId && t.IsActive
                                                        && (loadDate == null || t.FuelRequest.FuelRequestDetail.StartDate <= loadDate)
                                                        && (mappedToProductTypeIds.Contains(t.FuelRequest.MstProduct.ProductTypeId)
                                                        || mappedToProductTypeIds.Contains(t.FuelRequest.MstProduct.MstTFXProduct.ProductTypeId))
                                                        && t.OrderXStatuses.Any(t1 => t1.StatusId == (int)OrderStatus.Open && t1.IsActive))
                                                       .Select(t => new
                                                       {
                                                           t.Id,
                                                           t.PoNumber,
                                                           OrderName = t.Name,
                                                           FuelType = t.FuelRequest.MstProduct.DisplayName ?? t.FuelRequest.MstProduct.Name,
                                                           t.TerminalId,
                                                           t.MstExternalTerminal.Name,
                                                           DRNotes = string.IsNullOrEmpty(t.OrderAdditionalDetail.DRNotes) ? string.Empty : t.OrderAdditionalDetail.DRNotes,
                                                           PickupLocation = t.FuelDispatchLocations.Where(t1 => t1.IsActive && !t1.IsSkipped
                                                           && t1.DeliveryScheduleId == null && t1.TrackableScheduleId == null && t1.LocationType == (int)LocationType.PickUp)
                                                           .OrderByDescending(t1 => t1.Id).Select(t1 => new
                                                           {
                                                               t1.LocationType,
                                                               t1.Address,
                                                               t1.City,
                                                               t1.SiteName,
                                                               t1.StateCode,
                                                               t1.StateId,
                                                               t1.CountryCode,
                                                               t1.TerminalId,
                                                               t1.MstExternalTerminal.Name,
                                                               t1.ZipCode,
                                                               t1.Latitude,
                                                               t1.Longitude,
                                                               t1.CountyName,
                                                           }).FirstOrDefault(),
                                                           t.FuelRequest.FuelRequestDetail.IsDispatchRetainedByCustomer,
                                                           t.FuelRequest.FuelRequestDetail.TruckLoadTypeId
                                                       }).OrderByDescending(t => t.Id)
                                                       .ToListAsync();
                if (carrierStatus == (int)BrokeredDrCarrierStatus.None || carrierStatus == (int)BrokeredDrCarrierStatus.Recalled)
                {
                    var brokeredOrderDetails = await GetBrokerJobOrderDetails(userContext.CompanyId, supplierOrders.Select(top => top.Id).ToList());
                    if (brokeredOrderDetails != null && brokeredOrderDetails.Count > 0)
                    {
                        foreach (var brokeredOrderId in brokeredOrderDetails)
                        {
                            var ordersToRemove = supplierOrders.Where(t => t.Id == brokeredOrderId && !t.IsDispatchRetainedByCustomer).ToList();
                            foreach (var item in ordersToRemove)
                            {
                                supplierOrders.Remove(item);
                            }
                        }
                    }
                }
                foreach (var order in supplierOrders)
                {
                    OrderPickupDetailModel orderDetail = new OrderPickupDetailModel()
                    {
                        OrderId = order.Id,
                        PoNumber = $"{(string.IsNullOrEmpty(order.OrderName) ? order.PoNumber : order.OrderName)}({order.FuelType})",
                        TerminalId = order.TerminalId ?? 0,
                        TerminalName = order.Name,
                        TruckLoadType = (TruckLoadTypes)order.TruckLoadTypeId
                    };
                    if (order.PickupLocation != null)
                    {
                        orderDetail.PickupLocationType = order.PickupLocation.LocationType;
                        if (order.PickupLocation.TerminalId != null && order.PickupLocation.TerminalId > 0)
                        {
                            orderDetail.TerminalId = order.PickupLocation.TerminalId.Value;
                            orderDetail.TerminalName = order.PickupLocation.Name;
                            orderDetail.PickupLocationType = (int)PickupLocationType.Terminal;
                        }
                        else
                        {
                            orderDetail.BulkplantName = order.PickupLocation.SiteName;
                            orderDetail.Address = order.PickupLocation.Address;
                            orderDetail.City = order.PickupLocation.City;
                            orderDetail.StateCode = order.PickupLocation.StateCode;
                            orderDetail.StateId = order.PickupLocation.StateId;
                            orderDetail.CountryCode = order.PickupLocation.CountryCode;
                            orderDetail.ZipCode = order.PickupLocation.ZipCode;
                            orderDetail.CountyName = order.PickupLocation.CountyName;
                            orderDetail.Latitude = order.PickupLocation.Latitude;
                            orderDetail.Longitude = order.PickupLocation.Longitude;
                            orderDetail.PickupLocationType = (int)PickupLocationType.BulkPlant;
                        }

                    }
                    if (carrier != null)
                    {
                        int OrderID = order.Id;
                        await GetBrokeredOrderSupplierToSupplierInfo(carrier, OrderID);
                    }
                    orderDetail.DRNote = order.DRNotes;
                    orderDetail.IsDispatchRetainedByCustomer = order.IsDispatchRetainedByCustomer;
                    response.Add(orderDetail);

                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetOrderDetails", ex.Message, ex);
            }
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="carrier"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        private async Task GetBrokeredOrderSupplierToSupplierInfo(List<TfxCarrierDropdownDisplayItem> carrier, int OrderID)
        {
            var brokerOrderDetails = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetBrokerOrderDetails(OrderID);
            if (brokerOrderDetails.Any())
            {
                foreach (var company in brokerOrderDetails)
                {
                    int index = carrier.FindIndex(t => t.Id == company.AcceptedCompanyId);
                    if (index == -1)
                    {
                        carrier.Add(new TfxCarrierDropdownDisplayItem { Id = company.AcceptedCompanyId, Name = company.AcceptedCompanyName });
                    }
                }
            }
        }

        public async Task<List<OrderPickupDetailModel>> GetOrderDetailsForEditDeliveryGroup(int jobId, int productTypeId, DateTimeOffset? loadDate, UserContext userContext, int carrierStatus = -1, bool isBlendReq = false)
        {
            List<OrderPickupDetailModel> response = new List<OrderPickupDetailModel>();
            try
            {
                // filter favFueltype orders of the region
                var freightServiceDomain = new FreightServiceDomain(this);
                var favProduct = await freightServiceDomain.GetRegionFavouriteProducts(jobId, null, userContext.CompanyId);
                var tfxFuelTypeIds = new List<int>();
                if (favProduct != null && favProduct.TfxFavProductTypeId != RegionFavProductType.None)
                {
                    if (favProduct.TfxFavProductTypeId == RegionFavProductType.FuelType && favProduct.TfxFuelTypeIds != null && favProduct.TfxFuelTypeIds.Any())
                    {
                        tfxFuelTypeIds = favProduct.TfxFuelTypeIds.Select(t => t.Id).ToList();
                    }
                }
                response = await new StoredProcedureDomain(this).GetOrderDetailsForEditDeliveryGroup(jobId, productTypeId, loadDate, userContext.CompanyId, isBlendReq, tfxFuelTypeIds);
                response = response.Where(t => t.ProductTypeId == productTypeId).ToList().Union(response.Where(t => t.ProductTypeId != productTypeId).ToList()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetOrderDetailsForEditDeliveryGroup", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<JobDetailsWithOrders>> GetJobWithOrders(string regionId, int tfxProductId, int productTypeId, int? terminalId, int? bulkplantId, DateTimeOffset? loadDate, UserContext userContext)
        {
            var response = new List<JobDetailsWithOrders>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetJobListForCarrier, userContext.CompanyId, regionId);
                var regionJobs = await ApiGetCall<List<CustomerJobForCarrierViewModel>>(apiUrl);
                var jobIds = regionJobs.Select(t => t.Job.Id).ToList();
                List<int> productIds = new List<int>();
                if (productTypeId != (int)ProductTypes.NonStandardFuel)
                {
                    productIds = await Context.DataContext.MstProducts.Where(t => t.TfxProductId == tfxProductId && t.IsActive).Select(P => P.Id).ToListAsync();
                }
                else
                {
                    productIds.Add(tfxProductId);
                }
                if (productIds.Any())
                {
                    var orders = await Context.DataContext.Orders.Where(t => productIds.Contains(t.FuelRequest.FuelTypeId) && t.FuelRequest.MstProduct.IsActive && t.IsActive
                                                                && t.AcceptedCompanyId == userContext.CompanyId && t.OrderXStatuses.Any(t1 => t1.StatusId == (int)OrderStatus.Open && t1.IsActive)
                                                                && jobIds.Contains(t.FuelRequest.JobId) && t.IsEndSupplier
                                                                    && (loadDate == null || t.FuelRequest.FuelRequestDetail.StartDate <= loadDate))
                                                           .Select(t1 => new
                                                           {
                                                               OrderId = t1.Id,
                                                               PoNumber = t1.PoNumber,
                                                               OrderName = t1.Name,
                                                               FuelType = t1.FuelRequest.MstProduct.DisplayName ?? t1.FuelRequest.MstProduct.Name,
                                                               JobId = t1.FuelRequest.JobId,
                                                               JobName = t1.FuelRequest.Job.Name,
                                                               t1.FuelRequest.FuelTypeId,
                                                               DisplayJobID = t1.FuelRequest.Job.DisplayJobID,
                                                               Address = t1.FuelRequest.Job.Address,
                                                               City = t1.FuelRequest.Job.City,
                                                               UoM = (int)t1.FuelRequest.UoM,
                                                               CompanyId = t1.BuyerCompany.Id,
                                                               CompanyName = t1.BuyerCompany.Name,
                                                               BadgeDetails = t1.OrderBadgeDetails.Where(t2 => (t2.IsCommonBadge || (terminalId > 0 && t2.TerminalId == terminalId.Value) || (bulkplantId > 0 && t2.BulkPlantId == bulkplantId.Value)) && t2.IsActive).Select(t2 => new
                                                               {
                                                                   t2.IsCommonBadge,
                                                                   t2.TerminalId,
                                                                   t2.BulkPlantId,
                                                                   t2.BadgeNo1,
                                                                   t2.BadgeNo2,
                                                                   t2.BadgeNo3
                                                               }).ToList()
                                                           }).ToListAsync();

                    foreach (var order in orders)
                    {
                        JobDetailsWithOrders orderDetails = new JobDetailsWithOrders()
                        {
                            OrderId = order.OrderId,
                            PoNumber = $"{(string.IsNullOrEmpty(order.OrderName) ? order.PoNumber : order.OrderName)}({order.FuelType})",
                            JobId = order.JobId,
                            JobName = order.JobName,
                            DisplayJobID = order.DisplayJobID,
                            Address = order.Address,
                            City = order.City,
                            UoM = order.UoM,
                            CompanyId = order.CompanyId,
                            CompanyName = order.CompanyName,
                            FuelTypeId = order.FuelTypeId
                        };
                        var commonBadgeInfo = order.BadgeDetails.FirstOrDefault(t => t.IsCommonBadge);
                        if (terminalId.HasValue && terminalId.Value > 0)
                        {
                            var badgeInfo = order.BadgeDetails.FirstOrDefault(t => t.TerminalId == terminalId.Value);
                            if (badgeInfo != null)
                            {
                                commonBadgeInfo = badgeInfo;
                            }
                        }
                        else if (bulkplantId.HasValue && bulkplantId.Value > 0)
                        {
                            var badgeInfo = order.BadgeDetails.FirstOrDefault(t => t.BulkPlantId == bulkplantId.Value);
                            if (badgeInfo != null)
                            {
                                commonBadgeInfo = badgeInfo;
                            }
                        }
                        if (commonBadgeInfo != null)
                        {
                            orderDetails.BadgeNo1 = commonBadgeInfo.BadgeNo1;
                            orderDetails.BadgeNo2 = commonBadgeInfo.BadgeNo2;
                            orderDetails.BadgeNo3 = commonBadgeInfo.BadgeNo3;
                        }
                        response.Add(orderDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetJobWithOrders", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ShiftDataModel>> GetScheduleCalendarData(string regionId, string date, string scheduleBuilderId, int view, int sbDsbView, UserContext userContext)
        {
            var response = new List<ShiftDataModel>();
            try
            {
                var apiResponse = await GetScheduleBuilderData(regionId, date, scheduleBuilderId, view, sbDsbView, userContext);
                if (apiResponse != null)
                {
                    var schedulebuilderData = ScheduleBuilderConverter.ConvertToDriverViewModel(apiResponse);
                    response = schedulebuilderData.Shifts.Select(x => new ShiftDataModel
                    {
                        Id = x.Id,
                        Name = "Shift - " + x.StartTime + " - " + x.EndTime,
                        Indexes = x.Schedules.SelectMany(y => y.Trips.Select(z => new IndexModel { LoadIndex = z.DriverColIndex ?? 0, ColumnIndex = z.DriverRowIndex ?? 0, LoadTime = z.StartTime, Driver = z.Drivers.Select(z1 => z1.Name).FirstOrDefault() })).ToList()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetScheduleCalendarData", ex.Message, ex);
            }
            return response;
        }

        public async Task<ScheduleBuilderViewModel> GetScheduleBuilderData(string regionId, string date, string scheduleBuilderId, int view, int sbDsbView, UserContext userContext)
        {
            ScheduleBuilderViewModel response = new ScheduleBuilderViewModel();
            try
            {
                var IsDsbDriverSchedule = Context.DataContext.OnboardingPreferences.Where(top => top.IsActive == true && top.CompanyId == userContext.CompanyId).OrderByDescending(top => top.Id).Select(x => x.IsDSBDriverSchedule).FirstOrDefault();
                var apiUrl = string.Format(ApplicationConstants.UrlGetSheduleBuilderDetails, userContext.CompanyId, userContext.Id, regionId, date, view, scheduleBuilderId, sbDsbView, IsDsbDriverSchedule);
                response = await ApiGetCall<ScheduleBuilderViewModel>(apiUrl);

                //set product sequencing
                if (response != null)
                {
                    response.IsDsbDriverSchedule = IsDsbDriverSchedule;
                    await SetProductSequenceToDelieveryRequests(response.Trips.SelectMany(t => t.DeliveryRequests).ToList(), userContext.CompanyId);
                    var storedProcedureDomain = new StoredProcedureDomain(this);
                    //set customer brand and DR queue attributes.
                    var responseDetails = await storedProcedureDomain.GetCustomerBrandANDLoadAttributeDetails(userContext.CompanyId, response.Trips.SelectMany(t => t.DeliveryRequests).Select(t => t.JobId).Distinct().ToList());
                    SetCustomerBrandAndLoadDRAttributes(responseDetails, response.Trips.SelectMany(t => t.DeliveryRequests).ToList(), userContext.CompanyId);

                    var lstCountryGroup = response.Trips.SelectMany(t => t.DeliveryRequests).Where(t => t.BulkPlant != null && t.BulkPlant.Country.Code == Country.CAR.ToString()).ToList();
                    foreach (var item in lstCountryGroup)
                    {
                        var countryGroup = await Context.DataContext.MstStates.Where(s => s.Code == item.BulkPlant.State.Code && s.CountryId == 4).FirstOrDefaultAsync();
                        item.BulkPlant.CountryGroup.Id = countryGroup.MstCountryAsGroup.Id;
                        item.BulkPlant.CountryGroup.Code = countryGroup.MstCountryAsGroup.Code;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetSheduleBuilderData", ex.Message, ex);
            }
            return response;
        }

        public void UpdateDeliveryRequestStatus(int? trackableScheduleId, int statusId, int userId)
        {
            if (trackableScheduleId != null && trackableScheduleId > 0)
            {
                var schedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.Id == trackableScheduleId
                                                                                    ).Select(t => new { FrDeliveryRequestId = t.FrDeliveryRequestId, StatusId = t.Order != null ? t.Order.OrderXStatuses.Where(t1 => t1.IsActive).Select(t1 => t1.StatusId).FirstOrDefault() : 0 }).FirstOrDefault();
                if (schedule != null && !string.IsNullOrWhiteSpace(schedule.FrDeliveryRequestId))
                {
                    var input = new DeliveryReqStatusUpdateModel() { DeliveryRequestId = schedule.FrDeliveryRequestId, OrderStatusId = schedule.StatusId, ScheduleEnrouteStatusId = statusId, UserId = userId };
                    UpdateDeliveryRequestStatus(new List<DeliveryReqStatusUpdateModel>() { input });
                }
            }
        }
        public void UpdateDeliveryRequestStatus(List<DeliveryReqStatusUpdateModel> statusModels)
        {
            if (statusModels != null && statusModels.Any())
            {
                SetScheduleStatus(statusModels);
                var response = Task.Run(() => ApiPostCall<StatusViewModel>(ApplicationConstants.UrlUpdateDeliveryRequestStatus, statusModels)).Result;
                if (response.StatusCode == Status.Failed)
                {
                    LogManager.Logger.WriteException("ScheduleBuilderDomain", "UpdateDeliveryRequestStatus", response.StatusMessage, null);
                }
            }
        }

        private void SetScheduleStatus(List<DeliveryReqStatusUpdateModel> statusModels)
        {
            foreach (var tr in statusModels)
            {
                if (tr.ScheduleStatusId > 0)
                {
                    tr.ScheduleStatusName = ((TrackableDeliveryScheduleStatus)tr.ScheduleStatusId).GetDisplayName();
                }
                else if (tr.ScheduleEnrouteStatusId > 0)
                {
                    tr.ScheduleStatusName = ((EnrouteDeliveryStatus)tr.ScheduleEnrouteStatusId).GetDisplayName();
                }
            }
        }

        public async Task<List<DipatchersRegionDetails>> GetRegionDispactherDetails(int driverId, int companyId, string regionId)
        {
            List<DipatchersRegionDetails> response = new List<DipatchersRegionDetails>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetDipatchersRegionDetails, driverId, companyId, regionId);
                response = await ApiGetCall<List<DipatchersRegionDetails>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetRegionDispactherDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveCanceledScheduleDetails(CancelScheduleModel model)
        {
            var response = new StatusViewModel();
            if (model != null && model.TrackableScheduleIds != null && model.TrackableScheduleIds.Any())
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var groupchanges = new List<ScheduleNotificationModel>();
                        var apiResponse = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlCancelSchedules, model);
                        if (apiResponse != null && apiResponse.StatusCode == Status.Success)
                        {
                            var trackableSchedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => (model.TrackableScheduleIds.Contains(t.Id) || apiResponse.EntityIds.Contains(t.FrDeliveryRequestId))
                                                                                    && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Completed
                                                                                     && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.CompletedLate
                                                                                      && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                                                      && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                                      && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledLate
                                                                                      && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.UnplannedDropCompleted).ToListAsync();

                            foreach (var schedule in trackableSchedules)
                            {
                                if (schedule.DriverId != model.DriverId || !model.TrackableScheduleIds.Contains(schedule.Id))
                                {
                                    groupchanges.Add(new ScheduleNotificationModel() { DriverId = schedule.DriverId, GroupId = schedule.DeliveryGroupId ?? 0, OrderId = schedule.OrderId ?? 0, ScheduleId = schedule.DeliveryScheduleId, TrackableScheduleId = schedule.Id, ScheduleStatus = (int)DeliveryScheduleStatus.Canceled });
                                }
                                schedule.DeliveryScheduleStatusId = (int)TrackableDeliveryScheduleStatus.Canceled;
                            }
                            await Context.CommitAsync();
                            await SaveRetainInfomation(trackableSchedules);
                            if (model.GroupedParentDrIds != null && model.GroupedParentDrIds.Any(t => !string.IsNullOrWhiteSpace(t)))
                            {
                                var consolidatedInvoiceDomain = new ConsolidatedInvoiceDomain(this);
                                var companyId = Context.DataContext.Users.Where(w => w.Id == model.DriverId).Select(s => s.CompanyId).FirstOrDefault();
                                model.GroupedParentDrIds.Where(t => t != null && t != "").ToList().ForEach(t =>
                                  {
                                      consolidatedInvoiceDomain.AddQueueMessageForDrCompletion(new UserContext() { Id = model.DriverId, CompanyId = companyId ?? 0 }, t);
                                  });
                            }
                            transaction.Commit();
                            response.StatusCode = Status.Success;
                        }
                        else
                        {
                            transaction.Rollback();
                        }

                        if (groupchanges.Any())
                        {
                            var pushNotificationResponse = await new PushNotificationDomain(this).PushSbChangesNotificationToDriver(groupchanges, string.Empty);
                            if (pushNotificationResponse.StatusCode == Status.Failed)
                            {
                                LogManager.Logger.WriteDebug("ScheduleBuilderDomain", "SaveCanceledScheduleDetails", "Failed to send notification :: Model" + JsonConvert.SerializeObject(groupchanges) + " " + pushNotificationResponse.StatusMessage);
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        LogManager.Logger.WriteException("ScheduleBuilderDomain", "SaveCanceledScheduleDetails :: Model" + JsonConvert.SerializeObject(model), ex.Message, ex);
                    }
                }
            }
            return response;
        }

        public async Task<List<DeliveryReqStatusUpdateModel>> GetScheduleStatus(List<int> trackableScheduleIds)
        {
            List<DeliveryReqStatusUpdateModel> response = new List<DeliveryReqStatusUpdateModel>();
            try
            {
                if (trackableScheduleIds != null && trackableScheduleIds.Any())
                {
                    var trackableScheduleStatuses = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => trackableScheduleIds.Contains(t.Id)).Select(t => new { Id = t.Id, DeliveryScheduleStatusId = t.DeliveryScheduleStatusId, FrDeliveryRequestId = t.FrDeliveryRequestId }).ToListAsync();
                    var enrouteStatuses = await Context.DataContext.EnrouteDeliveryHistories.Where(t => t.TrackableScheduleId != null && trackableScheduleIds.Contains(t.TrackableScheduleId.Value)).Select(t => new { EnrouteDate = t.EnrouteDate, TrackableScheduleId = t.TrackableScheduleId, StatusId = t.StatusId }).ToListAsync();
                    foreach (var tr in trackableScheduleStatuses)
                    {
                        int enrouteStatus = enrouteStatuses.Where(t => t.TrackableScheduleId == tr.Id).OrderByDescending(t => t.EnrouteDate).Select(t => t.StatusId).FirstOrDefault();
                        response.Add(new DeliveryReqStatusUpdateModel() { DeliveryRequestId = tr.FrDeliveryRequestId, ScheduleStatusId = tr.DeliveryScheduleStatusId, ScheduleEnrouteStatusId = enrouteStatus });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetScheduleStatus", ex.Message, ex);
            }
            return response;
        }

        public async Task<DSBSaveModel> SaveScheduleBuilder(DSBSaveModel scheduleBuilderViewModel, UserContext userContext)
        {
            DSBSaveModel response = scheduleBuilderViewModel;
            try
            {
                List<DSBSaveModel> scheduleBuilders = await ProcessAcrossTheDateDrsEdit(scheduleBuilderViewModel);
                var jobIds = scheduleBuilders.SelectMany(t => t.Trips.SelectMany(t1 => t1.DeliveryRequests)).Where(t => t.IsTBD).Select(t => t.JobId).Distinct().ToList();
                var jobInfo = Context.DataContext.Jobs.Where(t => jobIds.Contains(t.Id)).Select(t => new TimeZoneOffsetModel() { Id = t.Id, TimeZoneName = t.TimeZoneName }).ToList();
                GetOffsetForTimezones(jobInfo);
                foreach (var scheduleBuilder in scheduleBuilders)
                {
                    foreach (var trip in scheduleBuilder.Trips)
                    {
                        var shiftEndTime = GetShiftEndTime(trip, scheduleBuilder.Date);
                        foreach (var dr in trip.DeliveryRequests)
                        {
                            if (dr.IsTBD && dr.JobId > 0)
                            {
                                dr.JobTimeZoneOffset = jobInfo.Where(t => t.Id == dr.JobId).Select(t => t.Offset).FirstOrDefault();
                            }
                            dr.ScheduleShiftEndTime = shiftEndTime;
                        }
                    }
                }
                var saveResponse = await ApiPostCall<List<DSBSaveModel>>(ApplicationConstants.UrlSaveSheduleBuilder, scheduleBuilders);
                if (saveResponse != null && saveResponse.Any())
                {
                    response = saveResponse.First();
                }

                //set product sequence
                await SetProductSequenceToDelieveryRequests(response.Trips.SelectMany(t => t.DeliveryRequests).ToList(), userContext.CompanyId);
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "SaveSheduleBuilder", ex.Message, ex);
            }
            return response;
        }

        private async Task<List<DSBSaveModel>> ProcessAcrossTheDateDrsEdit(DSBSaveModel scheduleBuilderViewModel)
        {
            var scheduleBuilders = new List<DSBSaveModel>() { scheduleBuilderViewModel };
            var preloadedDrs = scheduleBuilderViewModel.Trips.SelectMany(t1 => t1.DeliveryRequests
                                    .Where(t2 => !string.IsNullOrWhiteSpace(t2.PreLoadedFor) && t2.PostLoadInfo == null)).ToList();
            if (preloadedDrs.Any())
            {
                var postloadedDrIds = preloadedDrs.Select(t => t.PreLoadedFor).ToList();
                var scheduleBuildersForEditDrs = await ApiPostCall<List<ScheduleBuilderViewModel>>(ApplicationConstants.UrlGetScheduleBuildersByDrIds, postloadedDrIds);
                if (scheduleBuildersForEditDrs != null && scheduleBuildersForEditDrs.Any())
                {
                    EditDrsInAcrossTheDateScheduleBuilders(scheduleBuilders, preloadedDrs, postloadedDrIds, scheduleBuildersForEditDrs);
                }
            }
            return scheduleBuilders;
        }

        public async Task<StatusViewModel> SaveCalendarDeliveryRequest(CalendarScheduleViewModel scheduleData, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                var apiResponse = await GetScheduleBuilderData(scheduleData.RegionId, scheduleData.Date, string.Empty, 1, 2, userContext);
                if (apiResponse != null && scheduleData.DeliveryRequests != null && scheduleData.DeliveryRequests.Any())
                {
                    var loadData = ScheduleBuilderConverter.ConvertToDriverViewModel(apiResponse);

                    var dsbSaveModel = GetDSBSaveModel(loadData);
                    dsbSaveModel.CompanyId = userContext.CompanyId;
                    dsbSaveModel.UserId = userContext.Id;

                    var shift = loadData.Shifts.FirstOrDefault(t => t.Id == scheduleData.ShiftId);
                    if (shift != null)
                    {
                        var thisTrip = shift.Schedules.Where(t => t.DriverRowIndex == scheduleData.DriverRowIndex)
                                            .Select(t => t.Trips.FirstOrDefault(t1 => t1.DriverRowIndex == scheduleData.DriverRowIndex && t1.DriverColIndex == scheduleData.DriverColIndex)).FirstOrDefault();
                        if (thisTrip != null)
                        {
                            thisTrip.DeliveryRequests.AddRange(scheduleData.DeliveryRequests);
                            if (thisTrip.DeliveryGroupPrevStatus == DeliveryGroupStatus.Published)
                            {
                                thisTrip.TripStatus = TripStatus.Modified;
                                thisTrip.DeliveryGroupStatus = DeliveryGroupStatus.Published;
                                thisTrip.DeliveryRequests.ForEach(t => { t.Status = (int)DeliveryReqStatus.ScheduleCreated; t.IsCalendarView = false; });
                            }
                            else
                            {
                                thisTrip.TripStatus = TripStatus.Modified;
                                thisTrip.DeliveryGroupStatus = DeliveryGroupStatus.Draft;
                                thisTrip.DeliveryGroupPrevStatus = DeliveryGroupStatus.None;
                                // Add selected DRs in the trip of schedule builder
                                thisTrip.DeliveryRequests.ForEach(t => { t.ScheduleStatus = 14; t.Status = (int)DeliveryReqStatus.Draft; t.IsCalendarView = false; });

                            }

                            dsbSaveModel.Trips.Add(thisTrip);
                        }
                        if (!apiResponse.Trips.Any(x => x.DeliveryGroupPrevStatus == DeliveryGroupStatus.Published))
                        {
                            var saveScheduleBuilder = await SaveScheduleBuilder(dsbSaveModel, userContext);
                            if (saveScheduleBuilder != null && saveScheduleBuilder.StatusCode == (int)Status.Success)
                                response.StatusCode = saveScheduleBuilder.StatusCode;
                        }
                        else
                        {
                            var saveScheduleBuilder = await PublishScheduleBuilder(dsbSaveModel, userContext);
                            if (saveScheduleBuilder != null && (saveScheduleBuilder.StatusCode == Status.Success || saveScheduleBuilder.StatusCode == Status.Warning))
                                response.StatusCode = Status.Success;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "SaveCalendarDeliveryRequest", ex.Message, ex);
            }
            return response;
        }

        private static void EditDrsInAcrossTheDateScheduleBuilders(List<DSBSaveModel> scheduleBuilders, List<DeliveryRequestViewModel> preloadedDrs, List<string> deliveryRequestIds, List<ScheduleBuilderViewModel> scheduleBuildersForDeleteDrs)
        {
            foreach (var drId in deliveryRequestIds)
            {
                var dsbForDeleteTrip = scheduleBuildersForDeleteDrs.Where(t1 => t1.Trips.Any(t2 => t2.DeliveryRequests.Any(t3 => t3.Id == drId))).FirstOrDefault();
                var dsbSaveModel = scheduleBuilders.Where(t1 => t1.Trips.Any(t2 => t2.DeliveryRequests.Any(t3 => t3.Id == drId))).FirstOrDefault();
                if (dsbSaveModel == null)
                {
                    if (dsbForDeleteTrip != null)
                    {
                        dsbSaveModel = dsbForDeleteTrip.ToDsbSaveModel();
                        dsbSaveModel.Trips = new List<TripViewModel>();
                    }
                }
                if (dsbSaveModel != null && dsbForDeleteTrip != null)
                {
                    var tripModel = dsbSaveModel.Trips.Where(t1 => t1.DeliveryRequests.Any(t2 => t2.Id == drId)).FirstOrDefault();
                    if (tripModel == null)
                    {
                        tripModel = dsbForDeleteTrip.Trips.Where(t1 => t1.DeliveryRequests.Any(t2 => t2.Id == drId)).FirstOrDefault();
                        if (tripModel != null)
                        {
                            dsbSaveModel.Trips.Add(tripModel);
                        }
                    }
                    if (tripModel != null && tripModel.DeliveryRequests.Any(t => t.Id == drId))
                    {
                        var modifiedPreloadedDr = preloadedDrs.First(t => t.PreLoadedFor == drId);
                        var modifiedPostloadedDr = tripModel.DeliveryRequests.First(t => t.Id == drId);
                        modifiedPostloadedDr.UpdateModifiedPostLoadedDrValues(modifiedPreloadedDr);
                        tripModel.TripStatus = TripStatus.Modified;
                        if (tripModel.DeliveryGroupPrevStatus == DeliveryGroupStatus.Published)
                        {
                            tripModel.DeliveryGroupStatus = DeliveryGroupStatus.Published;
                        }
                    }
                    if (!scheduleBuilders.Any(t => t.Id == dsbSaveModel.Id))
                    {
                        scheduleBuilders.Add(dsbSaveModel);
                    }
                }
            }
        }

        public async Task<DSBSaveModel> AssignDriverAndTrailer(DSBSaveModel sbModel, UserContext userContext)
        {
            if (sbModel.Trips != null && sbModel.Trips.Any(t => t.Drivers != null && t.Drivers.Any(t1 => t1.Id > 0)))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        sbModel.CompanyId = userContext.CompanyId;
                        sbModel.UserId = userContext.Id;
                        await AssignDriverAndTrailerToSchedules(sbModel, userContext);
                        var apiResponse = await ApiPostCall<DSBSaveModel>(ApplicationConstants.UrlAssignDriverAndTrailer, sbModel);
                        if (apiResponse != null)
                        {
                            sbModel = apiResponse;
                            if (apiResponse.StatusCode == Status.Success)
                            {
                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        sbModel.StatusMessage = Resource.valMessageErrorOccurred;
                        LogManager.Logger.WriteException("ScheduleBuilderDomain", "AssignDriverAndTrailer", ex.Message, ex);
                    }
                }
            }
            return sbModel;
        }
        public async Task<List<TerminalBulkBadgeViewModel>> GetOrderBadgesByTerminal(List<int> orderIds, int pickupLocationType, int pickupLocationId)
        {
            var response = new List<TerminalBulkBadgeViewModel>();

            try
            {
                var ordersBadgeDetails = await Context.DataContext.OrderBadgeDetails.Where(t => orderIds.Contains(t.OrderId) && t.IsActive).ToListAsync();
                foreach (var orderId in orderIds)
                {
                    var orderBadgeInfo = ordersBadgeDetails.Where(t => t.OrderId == orderId).ToList();
                    var orderBadge = new OrderBadgeDetail();
                    var badgeDetails = new TerminalBulkBadgeViewModel() { OrderId = orderId };
                    if (pickupLocationType == (int)PickupLocationType.Terminal)
                    {
                        orderBadge = orderBadgeInfo.FirstOrDefault(t => t.TerminalId == pickupLocationId);
                    }
                    else
                    {
                        orderBadge = orderBadgeInfo.FirstOrDefault(t => t.BulkPlantId == pickupLocationId);
                    }

                    if (orderBadge != null)
                    {
                        badgeDetails.BadgeNo1 = orderBadge.BadgeNo1;
                        badgeDetails.BadgeNo2 = orderBadge.BadgeNo2;
                        badgeDetails.BadgeNo3 = orderBadge.BadgeNo3;
                    }
                    else
                    {
                        orderBadge = orderBadgeInfo.FirstOrDefault(t => t.IsCommonBadge);
                        if (orderBadge != null)
                        {
                            badgeDetails.BadgeNo1 = orderBadge.BadgeNo1;
                            badgeDetails.BadgeNo2 = orderBadge.BadgeNo2;
                            badgeDetails.BadgeNo3 = orderBadge.BadgeNo3;
                        }
                    }
                    response.Add(badgeDetails);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetOrderBadgesByTerminal", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<TerminalBulkBadgeViewModel>> GetTerminalBulkBadgeDetailsAsync(List<int> orderIds)
        {
            var response = new List<TerminalBulkBadgeViewModel>();

            try
            {
                var orderBadgeDetails = await Context.DataContext.OrderBadgeDetails.Where(t => orderIds.Contains(t.OrderId) && t.IsActive).ToListAsync();
                foreach (var item in orderBadgeDetails)
                {
                    if (!item.IsCommonBadge)
                    {
                        var badgePickupDetail = new TerminalBulkBadgeViewModel();
                        badgePickupDetail.OrderId = item.OrderId;
                        badgePickupDetail.BadgeNo1 = item.BadgeNo1;
                        badgePickupDetail.BadgeNo2 = item.BadgeNo2;
                        badgePickupDetail.BadgeNo3 = item.BadgeNo3;
                        badgePickupDetail.PickupLocationType = item.PickupLocationType;
                        if (item.PickupLocationType == PickupLocationType.Terminal)
                        {
                            badgePickupDetail.TerminalId = item.TerminalId;
                            badgePickupDetail.TerminalBulkPlantName = item.MstExternalTerminal.Name;
                        }
                        else
                        {
                            badgePickupDetail.BulkPlantId = item.BulkPlantId;
                            badgePickupDetail.TerminalBulkPlantName = item.BulkPlantLocation.Name;
                        }
                        response.Add(badgePickupDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetTerminalBulkBadgeDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        private async Task AssignDriverAndTrailerToSchedules(DSBSaveModel sbViewModel, UserContext userContext)
        {
            int driverId = sbViewModel.Trips[0].Drivers[0].Id;
            var publishedGroupIds = sbViewModel.Trips.Where(t => t.GroupId > 0).Select(t => t.GroupId).ToList();
            var groups = await Context.DataContext.DeliveryGroups.Where(t => publishedGroupIds.Contains(t.Id)).ToListAsync();
            var deliveryScheduleIds = sbViewModel.Trips.SelectMany(t => t.DeliveryRequests).Where(t => t.DeliveryScheduleId != null && t.DeliveryScheduleId > 0).Select(t => t.DeliveryScheduleId.Value).ToList();
            foreach (var group in groups)
            {
                if (group.DriverId != driverId)
                {
                    group.DriverId = driverId;
                    group.UpdatedBy = userContext.Id;
                    group.UpdatedDate = DateTimeOffset.Now;
                }
                group.DeliveryScheduleXTrackableSchedules.Where(t => t.DriverId != driverId).ToList().ForEach(t => t.DriverId = driverId);
                UpdateTrailerInfo(sbViewModel, group);
                var dSchedulexDrivers = Context.DataContext.DeliveryScheduleXDrivers.Where(t => t.DeliverySchedule.Type == (int)DeliveryScheduleType.SpecificDates && publishedGroupIds.Contains(t.DeliverySchedule.DeliveryGroupId ?? -1) && t.IsActive)
                                                    .Select(t => t).ToList();

                foreach (var scheduleId in deliveryScheduleIds)
                {
                    var existingDriver = dSchedulexDrivers.FirstOrDefault(t => t.DeliveryScheduleId == scheduleId);
                    if (existingDriver != null)
                    {
                        existingDriver.DriverId = driverId;
                    }
                    else
                    {
                        var deliveryDriver = new DeliveryScheduleXDriver
                        {
                            DriverId = driverId,
                            DeliveryScheduleId = scheduleId,
                            AssignedBy = userContext.Id,
                            AssignedDate = DateTimeOffset.Now,
                            IsActive = true
                        };
                        Context.DataContext.DeliveryScheduleXDrivers.Add(deliveryDriver);
                    }
                }
                UpdateDeliveryCompartmentInfo(sbViewModel, group);
                await Context.CommitAsync();
            }
        }

        private static void UpdateTrailerInfo(DSBSaveModel sbViewModel, DeliveryGroup group)
        {
            var isTrailerExists = sbViewModel.Trips.Select(top => top.IsTrailerExists).Distinct().FirstOrDefault();
            if (isTrailerExists)
            {
                var trailerInfo = sbViewModel.Trips.SelectMany(top => top.Trailers).Select(top => top.TrailerId).Distinct().ToList();
                var deliveryScheduleXTrackableSchedules = group.DeliveryScheduleXTrackableSchedules.ToList();
                foreach (var item in deliveryScheduleXTrackableSchedules)
                {
                    ScheduleAdditionalInfo additionalInfo = new ScheduleAdditionalInfo();
                    additionalInfo = JsonConvert.DeserializeObject<ScheduleAdditionalInfo>(item.AdditionalInfo);
                    additionalInfo.FsTrailerDisplayId = string.Join(",", trailerInfo);
                    item.AdditionalInfo = JsonConvert.SerializeObject(additionalInfo);
                }
            }
        }

        private static void UpdateDeliveryCompartmentInfo(DSBSaveModel sbViewModel, DeliveryGroup group)
        {
            var isTrailerExists = sbViewModel.Trips.Select(top => top.IsTrailerExists).Distinct().FirstOrDefault();
            if (isTrailerExists)
            {
                var trailerInfo = sbViewModel.Trips.SelectMany(top => top.Trailers).Select(top => top.Id).Distinct().ToList();
                foreach (var item in group.DeliveryScheduleXTrackableSchedules)
                {
                    string jsonCompartmentInfo = string.Empty;
                    var compartmentInfo = new List<CompartmentsInfoViewModel>();
                    var compartmentDetails = string.IsNullOrEmpty(item.CompartmentInfo) ? new List<CompartmentsInfoViewModel>() : JsonConvert.DeserializeObject<List<CompartmentsInfoViewModel>>(item.CompartmentInfo);
                    foreach (var traileritem in trailerInfo)
                    {
                        var compartments = compartmentDetails.Find(top => top.TrailerId == traileritem);
                        if (compartments != null)
                        {
                            compartmentInfo.Add(compartments);
                        }
                    }
                    if (compartmentInfo.Count > 0)
                    {
                        jsonCompartmentInfo = JsonConvert.SerializeObject(compartmentInfo);
                    }
                    item.CompartmentInfo = jsonCompartmentInfo;
                }
            }
        }

        public async Task<DSBSaveModel> DeleteDeliveryGroup(DSBSaveModel scheduleBuilder, UserContext userContext)
        {
            var scheduleBuilders = new List<DSBSaveModel>();
            scheduleBuilders.Add(scheduleBuilder);
            if (scheduleBuilder.PreloadedDRs.Any() || scheduleBuilder.PostloadedDRs.Any())
            {
                // Delete DRs from multiple DSBs (For handling delete in across the date pre/post loads)
                var deliveryRequestIds = new List<string>();
                deliveryRequestIds.AddRange(scheduleBuilder.PreloadedDRs);
                deliveryRequestIds.AddRange(scheduleBuilder.PostloadedDRs);
                var scheduleBuildersForDeleteDrs = await ApiPostCall<List<ScheduleBuilderViewModel>>(ApplicationConstants.UrlGetScheduleBuildersByDrIds, deliveryRequestIds);
                if (scheduleBuildersForDeleteDrs != null && scheduleBuildersForDeleteDrs.Any())
                {
                    RemoveDrsAcrossTheDateScheduleBuilders(scheduleBuilders, deliveryRequestIds, scheduleBuildersForDeleteDrs);
                }
            }
            scheduleBuilders = await DeleteAndSaveDeliveryGroup(scheduleBuilders, userContext);
            return scheduleBuilders.First();
        }

        private static void RemoveDrsAcrossTheDateScheduleBuilders(List<DSBSaveModel> scheduleBuilders, List<string> deliveryRequestIds, List<ScheduleBuilderViewModel> scheduleBuildersForDeleteDrs)
        {
            foreach (var drId in deliveryRequestIds)
            {
                var dsbForDeleteTrip = scheduleBuildersForDeleteDrs.Where(t1 => t1.Trips.Any(t2 => t2.DeliveryRequests.Any(t3 => t3.Id == drId))).FirstOrDefault();
                var dsbSaveModel = scheduleBuilders.Where(t1 => t1.Trips.Any(t2 => t2.DeliveryRequests.Any(t3 => t3.Id == drId))).FirstOrDefault();
                if (dsbSaveModel == null)
                {
                    if (dsbForDeleteTrip != null)
                    {
                        dsbSaveModel = dsbForDeleteTrip.ToDsbSaveModel();
                        dsbSaveModel.Trips = new List<TripViewModel>();
                    }
                }
                if (dsbSaveModel != null && dsbForDeleteTrip != null)
                {
                    var tripModel = dsbSaveModel.Trips.Where(t1 => t1.DeliveryRequests.Any(t2 => t2.Id == drId)).FirstOrDefault();
                    if (tripModel == null)
                    {
                        tripModel = dsbForDeleteTrip.Trips.Where(t1 => t1.DeliveryRequests.Any(t2 => t2.Id == drId)).FirstOrDefault();
                        if (tripModel != null)
                        {
                            dsbSaveModel.Trips.Add(tripModel);
                        }
                    }
                    if (tripModel != null)
                    {
                        tripModel.DeliveryRequests.Remove(tripModel.DeliveryRequests.First(t => t.Id == drId));
                    }
                    if (!scheduleBuilders.Any(t => t.Id == dsbSaveModel.Id))
                    {
                        scheduleBuilders.Add(dsbSaveModel);
                    }
                }
            }
        }

        public async Task<List<DSBSaveModel>> DeleteAndSaveDeliveryGroup(List<DSBSaveModel> scheduleBuilders, UserContext userContext)
        {
            List<ScheduleNotificationModel> groupChangesForNotifications = new List<ScheduleNotificationModel>();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    List<CreateDeliveryGroupModel> createDeliveryGroupModels = new List<CreateDeliveryGroupModel>();
                    for (int index = 0; index < scheduleBuilders.Count; index++)
                    {
                        if (scheduleBuilders[index].Trips != null)
                        {
                            // SetEmptyTripsStatusAsDeleted(scheduleBuilders[index]);
                            foreach (var trip in scheduleBuilders[index].Trips.Where(t => t.GroupId > 0))
                            {
                                await DeleteDSGroup(trip.GroupId, groupChangesForNotifications, userContext);
                            }
                        }
                        var modifiedTrips = scheduleBuilders[index].Trips.Where(t => t.GroupId > 0 && t.DeliveryGroupStatus != DeliveryGroupStatus.Deleted).ToList();
                        if (modifiedTrips.Any())
                        {
                            CreateDeliveryGroupModel createDeliveryGroupModel = GetDeliveryGroupViewModel(modifiedTrips);
                            createDeliveryGroupModel.ScheduleBuilder = scheduleBuilders[index];
                            createDeliveryGroupModels.Add(createDeliveryGroupModel);
                        }
                    }
                    var modifyResponse = await ModifyDeliveryGroups(userContext, createDeliveryGroupModels);
                    if (modifyResponse != null && modifyResponse.StatusCode == Status.Success && IsTodayScheduleChanged(createDeliveryGroupModels))
                    {
                        var groupChanges = createDeliveryGroupModels.SelectMany(t => t.GroupChanges).ToList();
                        groupChangesForNotifications.AddRange(groupChanges);
                    }

                    List<DSBSaveModel> response = null;
                    response = await ApiPostCall<List<DSBSaveModel>>(ApplicationConstants.UrlDeleteTrip, scheduleBuilders);
                    if (response != null)
                    {
                        scheduleBuilders = response;
                        if (response.All(t => t.StatusCode == Status.Success))
                        {
                            transaction.Commit();
                            scheduleBuilders = response;
                            response.ForEach(t => t.StatusMessage = Resource.valSuccessMessageResetLoad);
                            if (groupChangesForNotifications.Any())
                            {
                                var pushNotificationResponse = await new PushNotificationDomain(this).PushSbChangesNotificationToDriver(groupChangesForNotifications, userContext.UserName);
                                if (pushNotificationResponse.StatusCode == Status.Failed)
                                {
                                    scheduleBuilders.ForEach(t => t.StatusCode = Status.Warning);
                                    scheduleBuilders.ForEach(t => t.StatusMessage = t.StatusMessage + " " + "Failed to send notification to driver.");
                                }
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    scheduleBuilders.ForEach(t => t.StatusCode = Status.Failed);
                    scheduleBuilders.ForEach(t => t.StatusMessage = Resource.valMessageErrorOccurred);
                    LogManager.Logger.WriteException("ScheduleBuilderDomain", "DeleteDeliveryGroupOfSingleDsb", ex.Message, ex);
                }
            }
            return scheduleBuilders;
        }

        private static void SetEmptyTripsStatusAsDeleted(DSBSaveModel scheduleBuilder)
        {
            for (int tripIndex = 0; tripIndex < scheduleBuilder.Trips.Count; tripIndex++)
            {
                if (scheduleBuilder.Trips[tripIndex].DeliveryRequests.Count == 0)
                {
                    scheduleBuilder.Trips[tripIndex].DeliveryGroupStatus = DeliveryGroupStatus.Deleted;
                    scheduleBuilder.Trips[tripIndex].TripStatus = TripStatus.Deleted;
                }
                if (scheduleBuilder.Trips[tripIndex].DeliveryGroupStatus == DeliveryGroupStatus.Deleted)
                {
                    scheduleBuilder.Trips[tripIndex].DeliveryRequests.Clear();
                }
            }
        }

        public async Task<DSBSaveModel> PublishScheduleBuilder(DSBSaveModel scheduleBuilder, UserContext userContext, string language = "en-US")
        {
            var response = scheduleBuilder;
            try
            {

                var apiResponse = await ApiPostCall<DSBSaveModel>(ApplicationConstants.UrlValidateScheduleBuilder, scheduleBuilder);
                if (apiResponse.StatusCode == Status.Failed)
                {
                    return apiResponse;
                }
                List<CreateDeliveryGroupModel> createDeliveryGroupModels = new List<CreateDeliveryGroupModel>();
                List<DSBSaveModel> scheduleBuilders = await ProcessAcrossTheDateDrsEdit(scheduleBuilder);
                //filter DS with ignore future date
                bool isDSwithFutureDate = await CheckDSwithOrderFutureDate(scheduleBuilder, scheduleBuilders);
                if (!isDSwithFutureDate)
                {
                    for (int index = 0; index < scheduleBuilders.Count; index++)
                    {
                        SetEmptyTripsStatusAsDeleted(scheduleBuilders[index]);
                        scheduleBuilders[index].Trips.ForEach(x =>
                        {

                            if (x.IsDispatcherDragDropSequence == true && x.IsDispatcherDragDropSequenceModified == true)
                            {
                                int dispatcherDragDropSequence = 1;
                                x.DeliveryRequests.ForEach(x1 =>
                                {
                                    x1.DispatcherDragDropSequence = dispatcherDragDropSequence;
                                    dispatcherDragDropSequence = dispatcherDragDropSequence + 1;
                                });

                            }
                        });
                        var createDeliveryGroupModel = GetDeliveryGroupViewModel(scheduleBuilders[index].Trips);
                        createDeliveryGroupModel.ScheduleBuilder = scheduleBuilders[index];
                        createDeliveryGroupModels.Add(createDeliveryGroupModel);
                    }
                    //CreateDeliveryGroupModel createDeliveryGroupModel = GetDeliveryGroupViewModel(scheduleBuilder.Trips);
                    var publishResponse = await PublishScheduleBuilder(scheduleBuilders, userContext, createDeliveryGroupModels, language);
                    if (publishResponse != null && publishResponse.Any())
                    {
                        response = publishResponse.First();
                    }
                    //set product sequence
                    if (response.StatusCode != Status.Failed)
                    {
                        await SetProductSequenceToDelieveryRequests(response.Trips.SelectMany(t => t.DeliveryRequests).ToList(), userContext.CompanyId);
                    }
                }
                else
                {
                    response = scheduleBuilder;
                    response.StatusMessage = Resource.valDSFutureDate;
                }
            }
            catch (Exception ex)
            {
                response = scheduleBuilder;
                response.StatusMessage = Resource.valMessageErrorOccurred;
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "PublishSheduleBuilder", ex.Message, ex);
            }
            return response;
        }

        private async Task<bool> CheckDSwithOrderFutureDate(DSBSaveModel scheduleBuilder, List<DSBSaveModel> scheduleBuilders)
        {
            bool isDSwithFutureDate = false;
            DateTimeOffset? loadDate = null;
            if (!string.IsNullOrWhiteSpace(scheduleBuilder.Date))
            {
                loadDate = Convert.ToDateTime(scheduleBuilder.Date);
            }

            if (scheduleBuilders.Any() && loadDate != null)
            {
                var ordersDetails = scheduleBuilders.SelectMany(x => x.Trips).SelectMany(x => x.DeliveryRequests).Select(x => x.OrderId.Value).ToList();
                var supplierCurrentOrdersInfo = await Context.DataContext.Orders.Where(t => ordersDetails.Contains(t.Id) && t.IsActive && t.FuelRequest.FuelRequestDetail.StartDate <= loadDate
                                                                        ).Select(x => x.Id).ToListAsync();
                var supplierFutureOrdersInfo = await Context.DataContext.Orders.Where(t => ordersDetails.Contains(t.Id) && t.IsActive && t.FuelRequest.FuelRequestDetail.StartDate > loadDate
                                                                        ).Select(x => x.Id).ToListAsync();
                foreach (var item in scheduleBuilders)
                {
                    foreach (var tripItem in item.Trips)
                    {
                        var deliveryitem = tripItem.DeliveryRequests.Where(x => supplierFutureOrdersInfo.Contains(x.OrderId.Value) && !supplierCurrentOrdersInfo.Contains(x.OrderId.Value) && x.TrackScheduleStatus != (int)DeliveryScheduleStatus.Canceled).ToList();
                        if (deliveryitem.Any())
                        {
                            isDSwithFutureDate = true;

                        }

                    }
                }
            }
            return isDSwithFutureDate;
        }

        public async Task<List<DeliveryRequestViewModel>> SetProductSequenceToDelieveryRequests(List<DeliveryRequestViewModel> drs, int companyId)
        {
            var drJobIds = drs.Select(t1 => t1.JobId).Distinct().ToList();
            var productSequences = await new CompanyDomain(this).GetProductSequenceForJobs(companyId, drJobIds);
            foreach (DeliveryRequestViewModel dr in drs)
            {
                dr.ProductSequence = productSequences.ProductSequence.FirstOrDefault(t => t.JobId == dr.JobId && t.DisplayName == dr.ProductType)?.Sequence ?? int.MaxValue;
            }
            return drs;
        }
        private CreateDeliveryGroupModel GetDeliveryGroupViewModel(List<TripViewModel> trips)
        {
            CreateDeliveryGroupModel createDeliveryGroupModel = new CreateDeliveryGroupModel();
            createDeliveryGroupModel.PublishedGroups = trips.Where(t => t.DeliveryGroupStatus == DeliveryGroupStatus.Published || (t.GroupId > 0 && t.DeliveryGroupStatus == DeliveryGroupStatus.Deleted)).ToList();
            if (Context.DataContext.DeliverySchedules.Any())
            {
                createDeliveryGroupModel.LatestSchGroupNumber = Context.DataContext.DeliverySchedules.Max(t => t.GroupId);
            }
            var terminalIds = createDeliveryGroupModel.PublishedGroups.Where(t => t.IsCommonPickup && t.PickupLocationType != PickupLocationType.BulkPlant && t.Terminal != null && t.Terminal.Id > 0).Select(t => t.Terminal.Id).ToList();
            terminalIds.AddRange(createDeliveryGroupModel.PublishedGroups.Where(t => !t.IsCommonPickup).SelectMany(t => t.DeliveryRequests.Where(t1 => t1.PickupLocationType != PickupLocationType.BulkPlant && t1.Terminal != null && t1.Terminal.Id > 0).Select(t1 => t1.Terminal.Id)));
            terminalIds = terminalIds.Distinct().ToList();
            var modifiedDeliveryReqs = createDeliveryGroupModel.PublishedGroups.SelectMany(t => t.DeliveryRequests).Where(t1 => (t1.OrderId != null || t1.IsTBD)
                                                                                        && (t1.PreviousStatus != (int)DeliveryReqStatus.ScheduleCreated || t1.ScheduleStatus == (int)DeliveryScheduleStatus.New || t1.ScheduleStatus == (int)DeliveryScheduleStatus.Modified))
                                                                     .Select(t1 => new ModifiedDeliveryReqs { OrderId = t1.OrderId, TrackableScheduleId = t1.TrackableScheduleId }).ToList();
            createDeliveryGroupModel.OrderIds = modifiedDeliveryReqs.Select(t => t.OrderId ?? 0).Distinct().ToList();

            createDeliveryGroupModel.TrackableScheduleIds = modifiedDeliveryReqs.Where(t => t.TrackableScheduleId != null).Select(t => t.TrackableScheduleId.Value).ToList();
            //createDeliveryGroupModel.TrackableScheduleIds = GetInProgressSchedules(createDeliveryGroupModel.TrackableScheduleIds);
            createDeliveryGroupModel.Orders = Context.DataContext.Orders.Where(t => createDeliveryGroupModel.OrderIds.Contains(t.Id)).Select(t => new OrderDetailModel() { Id = t.Id, Currency = t.FuelRequest.Currency, TimeZoneName = t.FuelRequest.Job.TimeZoneName }).ToList();
            var jobIds = trips.SelectMany(t => t.DeliveryRequests.Where(t1 => t1.IsTBD && t1.JobId > 0).Select(t1 => t1.JobId)).ToList();
            var jobInfo = Context.DataContext.Jobs.Where(t => jobIds.Contains(t.Id)).Select(t => new TimeZoneOffsetModel() { Id = t.Id, TimeZoneName = t.TimeZoneName }).ToList();
            GetOffsetForTimezones(jobInfo);
            foreach (var trip in trips)
            {
                foreach (var dr in trip.DeliveryRequests.Where(t => t.IsTBD && t.JobId > 0))
                {
                    dr.JobTimeZoneOffset = jobInfo.Where(t => t.Id == dr.JobId).Select(t => t.Offset).FirstOrDefault();
                }
            }
            createDeliveryGroupModel.Terminals = Context.DataContext.MstExternalTerminals.Where(t => terminalIds.Contains(t.Id)).Select(t => new DropAddressViewModel()
            {
                SiteId = t.Id,
                SiteName = t.Name,
                Address = t.Address,
                City = t.City,
                State = new StateViewModel() { Id = t.StateId, Code = t.StateCode },
                ZipCode = t.ZipCode,
                Country = new CountryViewModel() { Code = t.CountryCode },
                CountyName = t.CountyName,
                Latitude = t.Latitude,
                Longitude = t.Longitude
            }).ToList();
            return createDeliveryGroupModel;
        }
        private async Task<List<DSBSaveModel>> PublishScheduleBuilder(List<DSBSaveModel> scheduleBuilders, UserContext userContext, List<CreateDeliveryGroupModel> createDeliveryGroupModels, string language)
        {
            var response = new List<DSBSaveModel>();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {

                    Context.DataContext.Database.CommandTimeout = 180;//3 minutes timeout
                    var modifyResponse = await ModifyDeliveryGroups(userContext, createDeliveryGroupModels);
                    var apiResponse = await ApiPostCall<List<DSBSaveModel>>(ApplicationConstants.UrlSaveSheduleBuilder, scheduleBuilders);
                    if (apiResponse != null)
                    {
                        response = apiResponse;
                        if (response.All(t => t.StatusCode == Status.Success))
                        {
                            var trackableScheduleIds = createDeliveryGroupModels.SelectMany(t => t.GroupChanges.Where(t1 => t1.TrackableScheduleId > 0 && (t1.ScheduleStatus == (int)DeliveryScheduleStatus.New
                                                                                                || t1.ScheduleStatus == (int)DeliveryScheduleStatus.Accepted
                                                                                                || t1.ScheduleStatus == (int)DeliveryScheduleStatus.Modified)).Select(t1 => t1.TrackableScheduleId)).ToList();
                            if (trackableScheduleIds.Any())
                            {
                                await new StoredProcedureDomain(this).InsertBrokerSchedules(trackableScheduleIds);
                            }
                            if (IsTodayScheduleChanged(createDeliveryGroupModels))
                            {
                                var pushNotificationDomain = new PushNotificationDomain(this);
                                var groupChanges = createDeliveryGroupModels.SelectMany(t => t.GroupChanges).ToList();
                                var pushNotificationResponse = await pushNotificationDomain.PushSbChangesNotificationToDriver(groupChanges, userContext.UserName);
                                if (pushNotificationResponse.StatusCode == Status.Failed)
                                {
                                    response.ForEach(t => t.StatusCode = Status.Warning);
                                    response.ForEach(t => t.StatusMessage = t.StatusMessage + " " + pushNotificationResponse.StatusMessage);
                                }
                            }

                            await UpdateDeliveryScheduleDispatcherDragDropSeq(scheduleBuilders, createDeliveryGroupModels, response, userContext);
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                            response.ForEach(t => t.StatusCode = Status.Failed);
                            response.ForEach(t => t.StatusMessage = t.StatusMessage != Resource.errMessageFailed ? t.StatusMessage : Resource.valMessageSbPublishFailed);
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        response.ForEach(t => t.StatusCode = Status.Failed);
                        response.ForEach(t => t.StatusMessage = Resource.valMessageServiceNotResponded);
                    }
                }
                catch (Exception ex)
                {
                    response = scheduleBuilders;
                    response.ForEach(t => t.StatusCode = Status.Failed);
                    response.ForEach(t => t.StatusMessage = Resource.valMessageSbPublishFailed);
                    transaction.Rollback();
                    LogManager.Logger.WriteException("ScheduleBuilderDomain", "PublishScheduleBuilder", ex.Message, ex);
                }
            }

            await SendDeliverySchedulePublishMessage(userContext, createDeliveryGroupModels, response);

            return response;
        }
        private async Task<bool> UpdateDeliveryScheduleDispatcherDragDropSeq(List<DSBSaveModel> scheduleBuilders, List<CreateDeliveryGroupModel> createDeliveryGroupModels, List<DSBSaveModel> response, UserContext userContext)
        {
            List<ScheduleNotificationModel> sbChangesModel = new List<ScheduleNotificationModel>();
            for (int index = 0; index < scheduleBuilders.Count; index++)
            {
                var tripDeliveryReqInfo = scheduleBuilders[index].Trips.Where(x => x.IsDispatcherDragDropSequence == true && x.IsDispatcherDragDropSequenceModified == true).SelectMany(x => x.DeliveryRequests).Select(x => new ModifiedDeliveryReqs { TrackableScheduleId = x.TrackableScheduleId, DispatcherDragDropSequence = x.DispatcherDragDropSequence }).ToList();
                if (tripDeliveryReqInfo.Any())
                {
                    var trackableScheduleIds = tripDeliveryReqInfo.Select(x => x.TrackableScheduleId).ToList();
                    var deliveryTrackableScheduleDetails = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(x => trackableScheduleIds.Contains(x.Id)).ToListAsync();
                    if (deliveryTrackableScheduleDetails.Any())
                    {
                        deliveryTrackableScheduleDetails.ForEach(x =>
                        {
                            sbChangesModel.Add(new ScheduleNotificationModel { TrackableScheduleId = x.Id, ScheduleId = x.DeliveryScheduleId, OrderId = x.OrderId ?? 0, ScheduleStatus = (int)DeliveryScheduleStatus.Modified, PreviousDriverId = x.DriverId, DriverId = x.DriverId });
                            var deliveryTrackableSchedule = tripDeliveryReqInfo.Where(x1 => x1.TrackableScheduleId == x.Id).FirstOrDefault();
                            if (deliveryTrackableSchedule != null)
                            {
                                x.IsDispatcherDragDrop = true;
                                x.DispatcherDragDropSequence = deliveryTrackableSchedule.DispatcherDragDropSequence;
                            }
                        });
                        await Context.CommitAsync();
                    }
                }

            }
            if (!IsTodayScheduleChanged(createDeliveryGroupModels))
            {
                var pushNotificationDomain = new PushNotificationDomain(this);
                var pushNotificationResponse = await pushNotificationDomain.PushSbChangesNotificationToDriver(sbChangesModel, userContext.UserName);
                if (pushNotificationResponse.StatusCode == Status.Failed)
                {
                    response.ForEach(t => t.StatusCode = Status.Warning);
                    response.ForEach(t => t.StatusMessage = t.StatusMessage + " " + pushNotificationResponse.StatusMessage);
                }
            }
            return true;
        }

        private async Task SendDeliverySchedulePublishMessage(UserContext userContext, List<CreateDeliveryGroupModel> createDeliveryGroupModels, List<DSBSaveModel> response)
        {
            try
            {
                if (response != null && response.Any())
                {
                    if (response.First().StatusCode != Status.Failed)
                    {
                        var messageDomain = new AppMessageDomain(this);
                        var dSPublishMessageViewModels = GetBuyerMessageForDSPublished(createDeliveryGroupModels);
                        if (dSPublishMessageViewModels != null && dSPublishMessageViewModels.Any())
                        {
                            await messageDomain.SendDeliverySchedulePublishMessage(userContext, dSPublishMessageViewModels);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "SendDeliverySchedulePublishMessage", ex.Message, ex);
            }
        }

        private static List<DSPublishMessageViewModel> GetBuyerMessageForDSPublished(List<CreateDeliveryGroupModel> createDeliveryGroupModels)
        {
            List<DSPublishMessageViewModel> response = new List<DSPublishMessageViewModel>();
            if (createDeliveryGroupModels != null && createDeliveryGroupModels.Any())
            {
                var publishedGroups = createDeliveryGroupModels.SelectMany(t => t.PublishedGroups);
                if (publishedGroups != null && publishedGroups.Any())
                {
                    foreach (var item in publishedGroups)
                    {

                        DSPublishMessageViewModel dSPublishMessageViewModel = new DSPublishMessageViewModel();
                        dSPublishMessageViewModel.ShiftStartTime = item.ShiftStartTime;
                        dSPublishMessageViewModel.ShiftEndTime = item.ShiftEndTime;
                        dSPublishMessageViewModel.StartDate = item.StartDate;
                        dSPublishMessageViewModel.DeliveryGroupPrevStatus = item.DeliveryGroupPrevStatus;
                        foreach (var dr in item.DeliveryRequests)
                        {
                            dSPublishMessageViewModel.DeliveryRequests.Add(new DSDeliveryRequestMessageViewModel()
                            {
                                OrderId = dr.OrderId,
                                ProductType = dr.ProductType,
                                RequiredQuantity = dr.RequiredQuantity,
                                UoM = dr.UoM
                            });
                        }
                        response.Add(dSPublishMessageViewModel);
                    }
                }
            }
            return response;
        }
        private async Task<StatusViewModel> ModifyDeliveryGroups(UserContext userContext, List<CreateDeliveryGroupModel> createDeliveryGroupModels)
        {
            var response = new StatusViewModel(Status.Failed);
            var dispatchDomain = new DispatchDomain(this);
            for (int index = 0; index < createDeliveryGroupModels.Count; index++)
            {
                var createDeliveryGroupModel = createDeliveryGroupModels[index];

                var modifiedTrackableSchedules = new List<DeliveryScheduleXTrackableSchedule>();
                if (createDeliveryGroupModel.TrackableScheduleIds.Any())
                {
                    modifiedTrackableSchedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => createDeliveryGroupModel.TrackableScheduleIds.Contains(t.Id)).ToListAsync();
                    createDeliveryGroupModel.OrderIds.AddRange(modifiedTrackableSchedules.Select(t => t.OrderId ?? 0).ToList());
                }
                foreach (var group in createDeliveryGroupModel.PublishedGroups)
                {
                    var driver = group.Drivers.FirstOrDefault();
                    await GetDeliveryGroup(userContext, driver, group, createDeliveryGroupModel);
                    if (group.TripStatus == TripStatus.Added || group.TripStatus == TripStatus.Modified)
                    {
                        foreach (var deliveryRequest in group.DeliveryRequests.Where(t => t.PreviousStatus != (int)DeliveryReqStatus.ScheduleCreated || t.ScheduleStatus == (int)DeliveryScheduleStatus.New || t.ScheduleStatus == (int)DeliveryScheduleStatus.Modified))
                        {
                            OrderDetailModel order = null;
                            if (createDeliveryGroupModel != null && createDeliveryGroupModel.Orders != null && deliveryRequest.OrderId.HasValue)
                            {
                                order = createDeliveryGroupModel.Orders.FirstOrDefault(t => t.Id == deliveryRequest.OrderId.Value);
                            }
                            DeliverySchedule deliverySchedule = null;
                            DeliveryScheduleXTrackableSchedule trackableSchedule = null;
                            trackableSchedule = modifiedTrackableSchedules.FirstOrDefault(t => t.Id == deliveryRequest.TrackableScheduleId);
                            if (trackableSchedule != null)
                            {
                                deliverySchedule = trackableSchedule.DeliverySchedule;
                                if (trackableSchedule.OrderId != deliveryRequest.OrderId)
                                {
                                    createDeliveryGroupModel.GroupChanges.Add(new ScheduleNotificationModel() { ScheduleDate = trackableSchedule.Date, StartTime = trackableSchedule.StartTime, EndTime = trackableSchedule.EndTime, OrderId = trackableSchedule.OrderId ?? 0, PreviousDriverId = trackableSchedule.DriverId, GroupId = trackableSchedule.DeliveryGroupId ?? 0, ScheduleId = trackableSchedule.DeliveryScheduleId, ScheduleStatus = (int)TrackableDeliveryScheduleStatus.Canceled, TrackableScheduleId = trackableSchedule.Id });
                                }
                            }
                            else
                            {
                                ++createDeliveryGroupModel.LatestSchGroupNumber;
                            }
                            FuelDispatchLocation pickUpLocation = null;
                            if (trackableSchedule == null || !IsCompletedSchedule(trackableSchedule))
                            {
                                deliverySchedule = GetDeliverySchedule(userContext, createDeliveryGroupModel.LatestSchGroupNumber, group, deliveryRequest, driver, deliverySchedule);
                                trackableSchedule = await GetTrackableSchedule(driver, group, deliveryRequest, createDeliveryGroupModel.ScheduleBuilder, trackableSchedule);
                                pickUpLocation = GetFuelDispatchLocation(userContext, createDeliveryGroupModel.Terminals, group, deliveryRequest, order, trackableSchedule);

                                if (trackableSchedule.Id == 0)
                                {
                                    deliverySchedule.DeliveryScheduleXTrackableSchedules.Add(trackableSchedule);
                                    Context.DataContext.DeliverySchedules.Add(deliverySchedule);
                                    if (!string.IsNullOrEmpty(pickUpLocation.Address) && !string.IsNullOrEmpty(pickUpLocation.ZipCode))
                                    {
                                        deliverySchedule.FuelDispatchLocations.Add(pickUpLocation);
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(pickUpLocation.Address) && !string.IsNullOrEmpty(pickUpLocation.ZipCode) && pickUpLocation.Id == 0)
                                    {
                                        deliverySchedule.FuelDispatchLocations.Add(pickUpLocation);
                                    }
                                }
                            }
                            await SetCarrierAndSupplierSource(group.Carrier, group.SupplierSource, deliverySchedule, userContext, dispatchDomain);
                            await Context.CommitAsync();

                            if (pickUpLocation != null)
                            {
                                RemovePickupLocation(group, deliveryRequest, pickUpLocation);
                                await dispatchDomain.SaveBulkPlantLocation(pickUpLocation, userContext.CompanyId);
                            }


                            createDeliveryGroupModel.GroupChanges.Add(new ScheduleNotificationModel() { ScheduleDate = trackableSchedule.Date, StartTime = trackableSchedule.StartTime, EndTime = trackableSchedule.EndTime, OrderId = trackableSchedule.OrderId ?? 0, GroupId = group.GroupId, ScheduleId = trackableSchedule.DeliveryScheduleId, ScheduleStatus = trackableSchedule.DeliveryScheduleStatusId, TrackableScheduleId = trackableSchedule.Id, DriverId = trackableSchedule.DriverId });
                            deliveryRequest.ScheduleStatus = deliverySchedule.StatusId;
                            deliveryRequest.TrackScheduleStatus = trackableSchedule.DeliveryScheduleStatusId;
                            deliveryRequest.TrackScheduleStatusName = ((TrackableDeliveryScheduleStatus)trackableSchedule.DeliveryScheduleStatusId).GetDisplayName();
                            deliveryRequest.DeliveryScheduleId = deliverySchedule.Id;
                            deliveryRequest.TrackableScheduleId = trackableSchedule.Id;
                            deliveryRequest.DeliveryGroupId = group.GroupId;
                            deliveryRequest.ScheduleShiftEndTime = trackableSchedule.ShiftEndDateTime;
                            deliveryRequest.DispactherNote = group.RouteInfo;

                            SetBadgeNumberForDeliveryRequests(group, deliveryRequest);
                            if (!trackableSchedule.PostLoadedForId.HasValue && !string.IsNullOrWhiteSpace(deliveryRequest.PreLoadedFor))
                            {
                                // Case 2: When postloaded DR published first and then preloaded DR. Link them using PostLoadedForId
                                var postloadedTrackableSchedule = await Context.DataContext.DeliveryScheduleXTrackableSchedules
                                        .FirstOrDefaultAsync(t => t.OrderId == trackableSchedule.OrderId && t.FrDeliveryRequestId == deliveryRequest.PreLoadedFor);
                                if (postloadedTrackableSchedule != null)
                                {
                                    postloadedTrackableSchedule.PostLoadedForId = trackableSchedule.Id;
                                }
                            }
                        }
                        await Context.CommitAsync();
                    }
                    else if (group.TripStatus == TripStatus.Deleted)
                    {
                        group.GroupId = 0;
                    }
                }
                CreateOrdersNewVersion(createDeliveryGroupModel, userContext);
            }
            response.StatusCode = Status.Success;
            return response;
        }

        private static void RemovePickupLocation(TripViewModel group, DeliveryRequestViewModel deliveryRequest, FuelDispatchLocation pickUpLocation)
        {
            bool isCommonPickup = group.IsCommonPickup;
            if (isCommonPickup)
            {
                if (group.PickupLocationType == PickupLocationType.BulkPlant)
                {
                    if (group.BulkPlant == null || string.IsNullOrEmpty(group.BulkPlant.SiteName))
                    {
                        pickUpLocation.IsActive = false;
                    }
                    else
                    {
                        pickUpLocation.IsActive = true;
                    }
                }
                else
                {
                    if (group.Terminal == null || group.Terminal.Id == 0)
                    {
                        pickUpLocation.IsActive = false;
                    }
                    else
                    {
                        pickUpLocation.IsActive = true;
                    }
                }
            }
            else
            {
                if (deliveryRequest.PickupLocationType == PickupLocationType.BulkPlant)
                {
                    if (deliveryRequest.BulkPlant == null || string.IsNullOrEmpty(deliveryRequest.BulkPlant.SiteName))
                    {
                        pickUpLocation.IsActive = false;
                    }
                    else
                    {
                        pickUpLocation.IsActive = true;
                    }
                }
                else
                {
                    if (deliveryRequest.Terminal == null || deliveryRequest.Terminal.Id == 0)
                    {
                        pickUpLocation.IsActive = false;
                    }
                    else
                    {
                        pickUpLocation.IsActive = true;
                    }
                }
            }
        }

        private static void SetBadgeNumberForDeliveryRequests(TripViewModel group, DeliveryRequestViewModel deliveryRequest)
        {
            if (deliveryRequest.IsCommonBadge)
            {
                deliveryRequest.BadgeNo1 = group.BadgeNo1;
                deliveryRequest.BadgeNo2 = group.BadgeNo2;
                deliveryRequest.BadgeNo3 = group.BadgeNo3;
                deliveryRequest.IsCommonBadge = true;
            }
        }

        private async Task DeleteGroup(int groupId, List<ScheduleNotificationModel> groupChanges, UserContext user, CreateDeliveryGroupModel groupModel = null)
        {
            var group = Context.DataContext.DeliveryGroups.FirstOrDefault(t => t.Id == groupId);
            if (group != null)
            {
                group.IsActive = false;
                group.UpdatedBy = user.Id;
                group.UpdatedDate = DateTimeOffset.Now;
                var trackableSchedules = group.DeliveryScheduleXTrackableSchedules.ToList();
                trackableSchedules.ForEach(t => { t.DeliveryGroupId = null; t.DeliveryScheduleStatusId = (int)TrackableDeliveryScheduleStatus.Canceled; t.FrDeliveryRequestId = null; });
                var deliverySchedules = group.DeliverySchedules.ToList();
                if (groupModel == null)
                {
                    groupModel = new CreateDeliveryGroupModel() { OrderIds = new List<int>() };
                }
                trackableSchedules.ForEach(t => groupModel.GroupChanges.Add(new ScheduleNotificationModel() { TrackableScheduleId = t.Id, ScheduleId = t.DeliveryScheduleId, OrderId = t.OrderId ?? 0, ScheduleStatus = (int)DeliveryScheduleStatus.Canceled, PreviousDriverId = t.DriverId }));
                trackableSchedules.ForEach(t => groupModel.OrderIds.Add(t.OrderId ?? 0));
                foreach (var ds in deliverySchedules)
                {
                    deliverySchedules.ForEach(t => { t.DeliveryGroupId = null; t.StatusId = (int)DeliveryScheduleStatus.Canceled; });
                }
                await Context.CommitAsync();
                CreateOrdersNewVersion(groupModel, user);
                bool isTodayScheduleExists = false;
                foreach (var tr in trackableSchedules)
                {
                    DateTimeOffset jobTime = DateTimeOffset.Now;
                    int timeDifference = 12;
                    if (!tr.OrderId.HasValue || tr.OrderId == 0)
                    {
                        timeDifference = 24;
                    }
                    else
                    {
                        jobTime = jobTime.ToTargetDateTimeOffset(tr.Order.FuelRequest.Job.TimeZoneName);
                    }
                    double trackScheduleStartTimeDiff = Math.Abs((tr.Date.Date.Add(tr.StartTime) - jobTime.DateTime).TotalHours);
                    double trackScheduleEndTimeDiff = Math.Abs((tr.Date.Date.Add(tr.EndTime) - jobTime.DateTime).TotalHours);

                    if (tr.Date.Date == jobTime.Date || trackScheduleStartTimeDiff <= timeDifference || trackScheduleEndTimeDiff <= timeDifference)
                    {
                        isTodayScheduleExists = true;
                        break;
                    }
                }
                if (isTodayScheduleExists)
                {
                    trackableSchedules.ForEach(t => groupChanges.Add(new ScheduleNotificationModel() { TrackableScheduleId = t.Id, ScheduleId = t.DeliveryScheduleId, OrderId = t.OrderId ?? 0, ScheduleStatus = (int)DeliveryScheduleStatus.Canceled, PreviousDriverId = t.DriverId }));
                }
            }
        }

        protected void CreateOrdersNewVersion(CreateDeliveryGroupModel groupModel, UserContext user)
        {
            var orders = Context.DataContext.OrderVersionXDeliverySchedules.Where(t => groupModel.OrderIds.Contains(t.OrderId) && t.IsActive).GroupBy(t => t.OrderId).ToList();
            foreach (var order in orders)
            {
                var currentDateTime = DateTimeOffset.Now;
                if (order.Any())
                {
                    var timeZone = order.FirstOrDefault().Order.FuelRequest.Job.TimeZoneName;
                    currentDateTime = currentDateTime.ToTargetDateTimeOffset(timeZone).AddDays(-3);
                }
                var orderSchedules = order.Where(t => t.DeliveryRequestId == null || (t.DeliverySchedule.Type != (int)DeliveryScheduleType.SpecificDates ||
                                                        (t.DeliverySchedule.Date >= currentDateTime && !t.DeliverySchedule.DeliveryScheduleXTrackableSchedules.All(t1 =>
                                                         t1.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Completed ||
                                                         //t1.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Canceled ||
                                                         t1.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.CompletedLate
                                                         || t1.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                                                       || t1.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                                       || t1.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.RescheduledLate
                                                                                       || t1.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.UnplannedDropCompleted)))).ToList();
                orderSchedules.ForEach(t => { t.IsActive = false; });
                int latestVersion = orderSchedules.OrderByDescending(t => t.Version).Select(t => t.Version).FirstOrDefault();
                var deletedSchedules = groupModel.GroupChanges.Where(t => t.OrderId == order.Key && t.ScheduleStatus == (int)DeliveryScheduleStatus.Canceled).Select(t => t.ScheduleId).ToList();
                var addedSchedules = groupModel.GroupChanges.Where(t => t.OrderId == order.Key && (t.ScheduleStatus == (int)DeliveryScheduleStatus.New || t.ScheduleStatus == (int)DeliveryScheduleStatus.Accepted || t.ScheduleStatus == (int)DeliveryScheduleStatus.Modified)).Select(t => t.ScheduleId).ToList();
                foreach (var schedule in orderSchedules)
                {
                    if (schedule.DeliveryRequestId != null && !deletedSchedules.Contains(schedule.DeliveryRequestId.Value) && !addedSchedules.Contains(schedule.DeliveryRequestId.Value))
                    {
                        OrderVersionXDeliverySchedule dschedule = new OrderVersionXDeliverySchedule()
                        {
                            DeliveryRequestId = schedule.DeliveryRequestId,
                            OrderId = order.Key,
                            Version = latestVersion + 1,
                            CreatedBy = schedule.CreatedBy,
                            CreatedDate = schedule.CreatedDate,
                            AdditionalNotes = schedule.AdditionalNotes,
                            IsActive = true
                        };
                        Context.DataContext.OrderVersionXDeliverySchedules.Add(dschedule);
                    }
                }
                foreach (var schedule in addedSchedules)
                {
                    OrderVersionXDeliverySchedule dschedule = new OrderVersionXDeliverySchedule()
                    {
                        DeliveryRequestId = schedule,
                        OrderId = order.Key,
                        Version = latestVersion + 1,
                        CreatedBy = user.Id,
                        CreatedDate = DateTimeOffset.Now,
                        IsActive = true
                    };
                    Context.DataContext.OrderVersionXDeliverySchedules.Add(dschedule);
                }
            }
            Context.Commit();
        }

        protected async Task GetDeliveryGroup(UserContext userContext, DriverAdditionalDetailsViewModel driver, TripViewModel group, CreateDeliveryGroupModel additionalDetails)
        {
            DeliveryGroup deliveryGroup = null;
            if (group.GroupId == 0)
            {
                deliveryGroup = new DeliveryGroup()
                {
                    RouteNote = group.RouteInfo,
                    CompanyId = userContext.CompanyId,
                    CreatedBy = userContext.Id,
                    CreatedDate = DateTimeOffset.Now,
                    IsActive = true,
                    UpdatedBy = userContext.Id,
                    UpdatedDate = DateTimeOffset.Now,
                    LoadCode = group.LoadCode,
                    DriverId = driver.Id
                };
                Context.DataContext.DeliveryGroups.Add(deliveryGroup);
            }
            else if (group.TripStatus != TripStatus.None)
            {
                deliveryGroup = Context.DataContext.DeliveryGroups.FirstOrDefault(t => t.Id == group.GroupId);
                if (deliveryGroup != null)
                {
                    deliveryGroup.RouteNote = group.RouteInfo;
                    deliveryGroup.UpdatedBy = userContext.Id;
                    deliveryGroup.UpdatedDate = DateTimeOffset.Now;
                    deliveryGroup.IsActive = group.TripStatus == TripStatus.Deleted ? false : true;
                    deliveryGroup.LoadCode = group.LoadCode;
                    deliveryGroup.DriverId = driver.Id;
                    UpdateSchedulesAndNotificationModel(group, additionalDetails, deliveryGroup);
                }
            }
            await Context.CommitAsync();
            if (deliveryGroup != null)
            {
                group.GroupId = deliveryGroup.Id;
            }
        }

        private void UpdateSchedulesAndNotificationModel(TripViewModel group, CreateDeliveryGroupModel additionalDetails, DeliveryGroup deliveryGroup)
        {
            var newSchedules = group.DeliveryRequests.Select(t => t.DeliveryScheduleId).ToList();
            var newTrackableSchedules = group.DeliveryRequests.Select(t => t.TrackableScheduleId).ToList();
            var deletedSchedules = deliveryGroup.DeliverySchedules.Where(t => !newSchedules.Contains(t.Id)).ToList();
            var deletedTSchedules = deliveryGroup.DeliveryScheduleXTrackableSchedules.Where(t => !newTrackableSchedules.Contains(t.Id)).ToList();
            deletedSchedules.ForEach(t => { t.DeliveryGroupId = null; t.StatusId = (int)DeliveryScheduleStatus.Canceled; });
            deletedTSchedules.ForEach(t => { t.DeliveryGroupId = null; t.DeliveryScheduleStatusId = (int)TrackableDeliveryScheduleStatus.Canceled; t.FrDeliveryRequestId = null; });
            deletedTSchedules.ForEach(t => additionalDetails.GroupChanges.Add(new ScheduleNotificationModel() { ScheduleDate = t.Date, StartTime = t.StartTime, EndTime = t.EndTime, OrderId = t.OrderId ?? 0, PreviousDriverId = t.DriverId, GroupId = deliveryGroup.Id, ScheduleId = t.DeliveryScheduleId, ScheduleStatus = t.DeliveryScheduleStatusId, TrackableScheduleId = t.Id }));
            additionalDetails.OrderIds.AddRange(deletedTSchedules.Select(t => t.OrderId ?? 0).Distinct());
            Context.DataContext.Entry(deliveryGroup).State = EntityState.Modified;
        }

        private bool IsTodayScheduleChanged(List<CreateDeliveryGroupModel> groupModels)
        {
            bool isTodayScheduleExists = false;
            int notifyTime = 12;
            foreach (var groupModel in groupModels)
            {
                foreach (var schedule in groupModel.GroupChanges)
                {
                    string TimeZone = groupModel.Orders.Where(t => schedule.OrderId > 0 && t.Id == schedule.OrderId).Select(t => t.TimeZoneName).FirstOrDefault();
                    DateTimeOffset jobTime = DateTimeOffset.Now;
                    if (!string.IsNullOrWhiteSpace(TimeZone))
                    {
                        jobTime = jobTime.ToTargetDateTimeOffset(TimeZone);
                        notifyTime = 12;
                    }
                    else
                    {
                        notifyTime = 24;
                    }
                    double trackScheduleStartTimeDiff = Math.Abs((schedule.ScheduleDate.Date.Add(schedule.StartTime) - jobTime.DateTime).TotalHours);
                    double trackScheduleEndTimeDiff = Math.Abs((schedule.ScheduleDate.Date.Add(schedule.EndTime) - jobTime.DateTime).TotalHours);
                    if (schedule.ScheduleDate.Date == jobTime.Date || trackScheduleStartTimeDiff <= notifyTime || trackScheduleEndTimeDiff <= notifyTime)
                    {
                        isTodayScheduleExists = true;
                        break;
                    }
                }
            }
            return isTodayScheduleExists;
        }

        protected async Task SetCarrierAndSupplierSource(string carrier, string supplierSource, DeliverySchedule scheduleEntity, UserContext userContext, DispatchDomain dispatchDomain)
        {
            int? carrierId = null, supplierSourceId = null;
            if (!string.IsNullOrWhiteSpace(carrier))
            {
                var carrierModel = await dispatchDomain.AddCarrierIfNotExists(carrier, userContext.Id, userContext.CompanyId);
                carrierId = carrierModel.Id;
            }
            if (!string.IsNullOrWhiteSpace(supplierSource))
            {
                var supplierSourceViewModel = new SupplierSourceViewModel() { Name = supplierSource };
                var supplierSourceModel = await dispatchDomain.AddSupplierSourceIfNotExists(supplierSourceViewModel, userContext.Id, userContext.CompanyId);
                supplierSourceId = supplierSourceModel.Id;
            }
            if (carrierId != null && carrierId > 0)
            {
                scheduleEntity.CarrierId = carrierId;
                scheduleEntity.DeliveryScheduleXTrackableSchedules.ToList().ForEach(t => t.CarrierId = carrierId);
            }
            if (supplierSourceId != null && supplierSourceId > 0)
            {
                scheduleEntity.SupplierSourceId = supplierSourceId;
                scheduleEntity.DeliveryScheduleXTrackableSchedules.ToList().ForEach(t => t.SupplierSourceId = supplierSourceId);
            }
        }

        protected static DeliverySchedule GetDeliverySchedule(UserContext userContext, int maxGroupId, TripViewModel group, DeliveryRequestViewModel deliveryRequest, DriverAdditionalDetailsViewModel driver, DeliverySchedule deliverySchedule)
        {
            DateTimeOffset scheduleDate = Convert.ToDateTime(group.StartDate);

            if (deliverySchedule == null)
            {
                deliverySchedule = new DeliverySchedule();
                deliverySchedule.GroupId = maxGroupId;
                DeliveryScheduleXDriver scheduleXDriver = new DeliveryScheduleXDriver()
                {
                    DriverId = driver.Id,
                    AssignedBy = userContext.Id,
                    AssignedDate = DateTimeOffset.Now,
                    IsActive = true
                };
                deliverySchedule.DeliveryScheduleXDrivers.Add(scheduleXDriver);
                deliverySchedule.StatusId = (int)DeliveryScheduleStatus.New;
            }
            else
            {
                deliverySchedule.StatusId = (int)DeliveryScheduleStatus.Modified;
                deliverySchedule.DeliveryScheduleXDrivers.Where(t => t.IsActive).ToList().ForEach(t => { t.DriverId = driver.Id; t.AssignedBy = userContext.Id; t.AssignedDate = DateTimeOffset.Now; });
            }
            deliverySchedule.Type = (int)DeliveryScheduleType.SpecificDates;
            deliverySchedule.WeekDayId = scheduleDate.DayOfWeek == 0 ? (int)WeekDay.Sunday : (int)scheduleDate.DayOfWeek;
            deliverySchedule.Date = scheduleDate;
            deliverySchedule.StartTime = Convert.ToDateTime(group.StartTime).TimeOfDay;
            deliverySchedule.EndTime = Convert.ToDateTime(group.EndTime).TimeOfDay;
            deliverySchedule.Quantity = deliveryRequest.RequiredQuantity;
            deliverySchedule.CreatedBy = userContext.Id;
            deliverySchedule.IsRescheduled = false;
            deliverySchedule.UoM = (UoM)deliveryRequest.UoM;
            deliverySchedule.LoadCode = group.LoadCode;
            deliverySchedule.QuantityTypeId = deliveryRequest.ScheduleQuantityType;
            deliverySchedule.DeliveryGroupId = group.GroupId;
            return deliverySchedule;
        }

        protected async Task<DeliveryScheduleXTrackableSchedule> GetTrackableSchedule(DriverAdditionalDetailsViewModel driver, TripViewModel group, DeliveryRequestViewModel deliveryRequest, DSBSaveModel scheduleBuilder, DeliveryScheduleXTrackableSchedule trackableSchedule)
        {
            if (trackableSchedule == null)
            {
                trackableSchedule = new DeliveryScheduleXTrackableSchedule();
            }
            DateTime shiftEndTime = GetShiftEndTime(group, scheduleBuilder.Date);
            int? postLoadedForId = null;
            var scheduleType = TrackableScheduleType.PickupAndDrop;
            if (!string.IsNullOrWhiteSpace(deliveryRequest.PreLoadedFor))
            {
                scheduleType = TrackableScheduleType.PickupOnly;
            }
            else if (!string.IsNullOrWhiteSpace(deliveryRequest.PostLoadedFor))
            {
                // Case 1: When preloaded DR published first and then postloaded DR. Link them using PostLoadedForId
                postLoadedForId = await GetTrackableScheduleId(deliveryRequest.PostLoadedFor);
                scheduleType = TrackableScheduleType.DropOnly;
            }

            trackableSchedule.Date = Convert.ToDateTime(group.StartDate);
            trackableSchedule.StartTime = Convert.ToDateTime(group.StartTime).TimeOfDay;
            trackableSchedule.EndTime = Convert.ToDateTime(group.EndTime).TimeOfDay;
            trackableSchedule.Quantity = deliveryRequest.RequiredQuantity;
            trackableSchedule.IsActive = true;
            trackableSchedule.OrderId = deliveryRequest.OrderId > 0 ? deliveryRequest.OrderId.Value : (int?)null;
            trackableSchedule.DeliveryScheduleStatusId = trackableSchedule.Id == 0 ? (int)TrackableDeliveryScheduleStatus.Accepted : (int)TrackableDeliveryScheduleStatus.Modified;
            trackableSchedule.UoM = (UoM)deliveryRequest.UoM;
            trackableSchedule.LoadCode = group.LoadCode;
            trackableSchedule.DriverId = driver.Id;
            trackableSchedule.QuantityTypeId = deliveryRequest.ScheduleQuantityType;
            trackableSchedule.DeliveryGroupId = group.GroupId;
            trackableSchedule.FrDeliveryRequestId = deliveryRequest.Id;
            trackableSchedule.BlendGroupId = deliveryRequest.BlendedGroupId;
            trackableSchedule.AdditionalInfo = GetScheduleAdditionalDetails(group, deliveryRequest, scheduleBuilder);
            trackableSchedule.ShiftStartDate = Convert.ToDateTime(scheduleBuilder.Date);
            trackableSchedule.ShiftEndDateTime = shiftEndTime;
            trackableSchedule.BadgeNo1 = deliveryRequest.IsCommonBadge ? group.BadgeNo1 : deliveryRequest.BadgeNo1;
            trackableSchedule.BadgeNo2 = deliveryRequest.IsCommonBadge ? group.BadgeNo2 : deliveryRequest.BadgeNo2;
            trackableSchedule.BadgeNo3 = deliveryRequest.IsCommonBadge ? group.BadgeNo3 : deliveryRequest.BadgeNo3;
            trackableSchedule.DisPatcherNote = group.RouteInfo;
            trackableSchedule.IsCommonBadge = deliveryRequest.IsCommonBadge;
            trackableSchedule.DeliveryScheduleType = (int)scheduleType;
            trackableSchedule.PostLoadedForId = postLoadedForId;
            trackableSchedule.CarrierOrderId = deliveryRequest.CarrierOrderId;
            trackableSchedule.ExternalRefId = deliveryRequest.ExternalRefId;
            trackableSchedule.RouteAdditionalInfo = GetScheduleRouteInfo(deliveryRequest);
            trackableSchedule.RecurringAdditionalInfo = GetRecurringScheduleInfo(deliveryRequest);
            trackableSchedule.CompartmentInfo = GetCompartmentInfo(deliveryRequest);
            trackableSchedule.IsFilldInvoke = deliveryRequest.IsFilldInvoke;
            trackableSchedule.Notes = deliveryRequest.Notes;
            trackableSchedule.GroupParentDRId = string.IsNullOrEmpty(deliveryRequest.GroupParentDRId) ? String.Empty : deliveryRequest.GroupParentDRId;
            trackableSchedule.IsDispatcherDragDrop = deliveryRequest.IsDispatcherDragDrop;
            trackableSchedule.DispatcherDragDropSequence = deliveryRequest.DispatcherDragDropSequence;
            trackableSchedule.DeliveryLevelPO = deliveryRequest.DeliveryLevelPO;
            return trackableSchedule;
        }

        private static DateTime GetShiftEndTime(TripViewModel group, string dsbDate)
        {
            DateTime shiftStartTime = Convert.ToDateTime(dsbDate).Add(DateTime.Parse(group.ShiftStartTime).TimeOfDay);
            DateTime shiftEndTime = Convert.ToDateTime(dsbDate).Add(DateTime.Parse(group.ShiftEndTime).TimeOfDay);
            if (shiftEndTime <= shiftStartTime)
            {
                shiftEndTime = shiftEndTime.AddDays(1);
            }
            DateTime loadStartTime = Convert.ToDateTime(group.StartDate).Add(DateTime.Parse(group.StartTime).TimeOfDay);
            DateTime loadEndTime = Convert.ToDateTime(group.StartDate).Add(DateTime.Parse(group.EndTime).TimeOfDay);
            if (loadEndTime < loadStartTime)
            {
                loadEndTime = loadEndTime.AddDays(1);
            }
            if (loadEndTime > shiftEndTime)
            {
                shiftEndTime = loadEndTime;
            }

            return shiftEndTime;
        }

        private async Task<int?> GetTrackableScheduleId(string DrId)
        {
            int? trackableScheduleId = null;
            try
            {
                trackableScheduleId = await Context.DataContext.DeliveryScheduleXTrackableSchedules
                                    .Where(t => t.FrDeliveryRequestId == DrId)
                                    .Select(t => t.Id).FirstOrDefaultAsync();
            }
            catch
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetTrackableScheduleId", "trackableScheduleId not found for DrId:" + DrId, new Exception());
            }
            return trackableScheduleId;
        }

        private static string GetScheduleAdditionalDetails(TripViewModel group, DeliveryRequestViewModel deliveryRequest, DSBSaveModel scheduleBuilder)
        {
            ScheduleAdditionalInfo additionalInfo = null;
            if (group != null && group.Trailers != null && group.Trailers.Count > 0)
            {
                additionalInfo = new ScheduleAdditionalInfo();
                additionalInfo.FsTrailerDisplayId = string.Join(", ", group.Trailers.Select(t => t.TrailerId));
            }
            if (group != null && group.DriverColIndex.HasValue)
            {
                if (additionalInfo == null)
                {
                    additionalInfo = new ScheduleAdditionalInfo();
                }
                additionalInfo.LoadNumber = "Load " + (group.DriverColIndex.Value + 1);
                additionalInfo.TripId = group.TripId;
            }

            if (deliveryRequest != null)
            {
                if (additionalInfo == null)
                {
                    additionalInfo = new ScheduleAdditionalInfo();
                }
                additionalInfo.Sap_OrderNo = deliveryRequest.Sap_OrderNo;
                additionalInfo.UniqueOrderNo = deliveryRequest.UniqueOrderNo;
                additionalInfo.DR_Price = Convert.ToString(deliveryRequest.IndicativePrice ?? 0);
                additionalInfo.FsPriority = (int)deliveryRequest.Priority;
                additionalInfo.CreditApprovalFilePath = deliveryRequest.CreditApprovalFilePath;
                additionalInfo.FsAssignedRegionId = additionalInfo.FsRegionId = deliveryRequest.CreatedByRegionId;
                if (!string.IsNullOrWhiteSpace(deliveryRequest.AssignedToRegionId) && deliveryRequest.CreatedByRegionId != deliveryRequest.AssignedToRegionId)
                {
                    additionalInfo.FsAssignedRegionId = deliveryRequest.AssignedToRegionId;
                }
            }
            //used for fetch Option pickup information.
            additionalInfo = IntializeOptionalPickupInfo(group, scheduleBuilder, additionalInfo);
            string jsonAdditionalInfo = null;
            if (additionalInfo != null)
            {
                jsonAdditionalInfo = JsonConvert.SerializeObject(additionalInfo);
            }
            return jsonAdditionalInfo;
        }



        private static string GetScheduleRouteInfo(DeliveryRequestViewModel deliveryRequest)
        {
            RouteInfoDetails routeInfoDetails = null;
            if (deliveryRequest.RouteInfo != null)
            {
                routeInfoDetails = new RouteInfoDetails();
                routeInfoDetails.Id = deliveryRequest.RouteInfo.Id;
                routeInfoDetails.Name = deliveryRequest.RouteInfo.Name;
                routeInfoDetails.LocationSeqNo = deliveryRequest.RouteInfo.LocationSeqNo;
            }
            string jsonAdditionalInfo = string.Empty;
            if (routeInfoDetails != null)
            {
                jsonAdditionalInfo = JsonConvert.SerializeObject(routeInfoDetails);
            }
            return jsonAdditionalInfo;
        }
        private static string GetRecurringScheduleInfo(DeliveryRequestViewModel deliveryRequest)
        {
            RecurringScheduleInfo recurringInfoDetails = null;
            if (deliveryRequest.isRecurringSchedule)
            {
                recurringInfoDetails = new RecurringScheduleInfo();
                recurringInfoDetails.Id = deliveryRequest.RecurringScheduleId;
                recurringInfoDetails.SCQtyTypeText = GetSchduleQtyTypeText(deliveryRequest.ScheduleQuantityType);
                recurringInfoDetails.SCQtyType = deliveryRequest.ScheduleQuantityType;
            }
            string jsonAdditionalInfo = string.Empty;
            if (recurringInfoDetails != null)
            {
                jsonAdditionalInfo = JsonConvert.SerializeObject(recurringInfoDetails);
            }
            return jsonAdditionalInfo;
        }
        private static string GetCompartmentInfo(DeliveryRequestViewModel deliveryRequest)
        {
            List<CompartmentsInfoViewModel> compartmentsInfo = new List<CompartmentsInfoViewModel>();
            if (deliveryRequest.Compartments != null && deliveryRequest.Compartments.Count > 0)
            {
                compartmentsInfo = deliveryRequest.Compartments;
            }
            string jsonCompartmentInfo = string.Empty;
            if (compartmentsInfo.Count > 0)
            {
                jsonCompartmentInfo = JsonConvert.SerializeObject(compartmentsInfo);
            }
            return jsonCompartmentInfo;
        }
        private static string GetSchduleQtyTypeText(int ScheduleQuantityType)
        {
            string qtyText = string.Empty;
            var enumerationType = typeof(ScheduleQuantityType);
            var name = Enum.GetName(enumerationType, ScheduleQuantityType);
            var memberInfo = enumerationType.GetMember(name);
            if (memberInfo.Count() > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                if (attributes.Count() > 0)
                {
                    var description = ((DisplayAttribute)attributes[0]).Name;
                    qtyText = description;
                }
            }
            return qtyText;
        }
        protected static FuelDispatchLocation GetFuelDispatchLocation(UserContext userContext, List<DropAddressViewModel> terminals, TripViewModel group, DeliveryRequestViewModel deliveryRequest, OrderDetailModel order, DeliveryScheduleXTrackableSchedule trackableSchedule)
        {

            FuelDispatchLocation fuelDispatchLocation = new FuelDispatchLocation();
            if (trackableSchedule.FuelDispatchLocations.Any(t => t.IsActive))
            {
                fuelDispatchLocation = trackableSchedule.FuelDispatchLocations.FirstOrDefault(t => t.IsActive);
            }

            fuelDispatchLocation.OrderId = deliveryRequest.OrderId > 0 ? deliveryRequest.OrderId.Value : (int?)null;
            fuelDispatchLocation.CreatedBy = userContext.Id;
            fuelDispatchLocation.CreatedDate = DateTimeOffset.Now;
            fuelDispatchLocation.TrackableScheduleId = trackableSchedule.Id;
            fuelDispatchLocation.TimeZoneName = order != null ? order.TimeZoneName : null;
            fuelDispatchLocation.Currency = order != null ? order.Currency : Currency.None;
            fuelDispatchLocation.DeliveryGroupId = deliveryRequest.DeliveryGroupId;
            if (!deliveryRequest.IsAdditive)
            {
                if (group.IsCommonPickup)
                {
                    GetPickupLocation(terminals, group.Terminal, group.BulkPlant, group.PickupLocationType, fuelDispatchLocation);
                }
                else
                {
                    GetPickupLocation(terminals, deliveryRequest.Terminal, deliveryRequest.BulkPlant, deliveryRequest.PickupLocationType, fuelDispatchLocation);
                }
            }
            return fuelDispatchLocation;
        }

        private static void GetPickupLocation(List<DropAddressViewModel> terminals, DropdownDisplayItem terminal, DropAddressViewModel bulkPlant, PickupLocationType pickupLocationType, FuelDispatchLocation fuelDispatchLocation)
        {
            if (terminal != null && terminal.Id > 0 && pickupLocationType != PickupLocationType.BulkPlant)
            {
                fuelDispatchLocation.TerminalId = terminal.Id;
                var terminalDetails = terminals.FirstOrDefault(t => t.SiteId == terminal.Id);
                terminalDetails.ToDispatchLocationEntity(fuelDispatchLocation);
            }
            else if (bulkPlant != null && !string.IsNullOrWhiteSpace(bulkPlant.SiteName) && pickupLocationType == PickupLocationType.BulkPlant)
            {
                fuelDispatchLocation.TerminalId = null;
                bulkPlant.ToDispatchLocationEntity(fuelDispatchLocation);
            }
        }

        public async Task<List<DriverAdditionalDetailsViewModel>> GetCompanyDrivers(int companyIds, List<string> trailerTypeIds, string regionIds, string selectedDate)
        {
            List<DriverAdditionalDetailsViewModel> response = new List<DriverAdditionalDetailsViewModel>();
            try
            {
                var inputData = new { trailerTypeId = trailerTypeIds, companyId = companyIds, regionId = regionIds, selectedDate = selectedDate };
                var apiResult = await ApiPostCall<List<DriverAdditionalDetailsViewModel>>(ApplicationConstants.UrlGetAllDriverDetailsScheduleBuilder, inputData);
                response = await SetDriverOnboardingStatus(apiResult, companyIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetCompanyDrivers", ex.Message, ex);
            }

            return response;

        }

        private async Task<List<DriverAdditionalDetailsViewModel>> SetDriverOnboardingStatus(List<DriverAdditionalDetailsViewModel> drivers, int companyId)
        {
            var response = new List<DriverAdditionalDetailsViewModel>();
            try
            {
                if (drivers != null && drivers.Any())
                {
                    var eligibleRoles = new List<int> { (int)UserRoles.Driver };

                    var users = await Context.DataContext.Users.Where(t => t.Company.Id == companyId && t.Company.IsActive &&
                                    t.MstRoles.Any(t1 => eligibleRoles.Contains(t1.Id))
                                    && ((t.IsActive && t.IsOnboardingComplete) || (!t.IsActive && !t.IsOnboardingComplete)))
                                    .Select(t => new { t.Id, t.FirstName, t.LastName, t.IsOnboardingComplete, t.IsEmailConfirmed }).ToListAsync();
                    //var users = await Context.DataContext.Users.Where(t => t.Company.Id == companyId && t.Company.IsActive
                    //                         && ((t.IsActive && t.IsOnboardingComplete) || (!t.IsActive && !t.IsOnboardingComplete)))
                    //                         .Select(t => new { t.Id, t.FirstName, t.LastName, t.IsOnboardingComplete, t.IsEmailConfirmed }).ToListAsync();
                    foreach (var user in users)
                    {
                        // var driverRecord = drivers.Find(t => t.Id == user.Id);
                        var driverRecord = drivers.Where(t => t.Id == user.Id).FirstOrDefault();
                        if (driverRecord != null)
                        {
                            if (user.IsOnboardingComplete)
                            {
                                response.Add(driverRecord);
                            }
                            else
                            {
                                if (user.IsEmailConfirmed)
                                {
                                    driverRecord.Name = driverRecord.Name + Resource.lblDriverEmailVerfied;
                                }
                                else
                                {
                                    driverRecord.Name = driverRecord.Name + Resource.lblDriverInvited;
                                }
                                response.Add(driverRecord);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "setDriverOnboardingStatus", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SendPushNotificationDriver(string message, int driverId)
        {
            var pushNotificationDomain = new PushNotificationDomain(this);
            var viewModel = new DriverNotificationViewModel();
            viewModel.DriverIds = new List<int>();
            viewModel.Message.Body = message;
            viewModel.Message.Title = Resource.newChatMessage;
            viewModel.Message.NotificationCode = (int)NotificationCode.SendBirdChat;
            viewModel.DriverIds.Add(driverId);
            var response = await pushNotificationDomain.NotificationToDriver(viewModel);
            return response;
        }
        public async Task<List<DeliveryRequestViewModel>> ValidateTrailerJobCompatibility(List<TrailerModel> trailers, List<DeliveryRequestViewModel> deliveryRequests)
        {
            List<DeliveryRequestViewModel> response = new List<DeliveryRequestViewModel>();
            try
            {
                TrailersDeliveryRequestViewModel trailersDeliveryRequestViewModel = new TrailersDeliveryRequestViewModel();
                trailersDeliveryRequestViewModel.deliveryRequests = deliveryRequests.Where(t => t.IsTBD == false).ToList();
                trailersDeliveryRequestViewModel.trailers = trailers;
                response = await ApiPostCall<List<DeliveryRequestViewModel>>(ApplicationConstants.UrlValidateTrailerJobCompatibility, trailersDeliveryRequestViewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "ValidateTrailerJobCompatibility", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<TrailerJobNonCompatibleDrs>> ValidateTrailerJobCompatibilityForLoadQueue(List<TrailersDeliveryRequestViewModel> models)
        {
            List<TrailerJobNonCompatibleDrs> response = new List<TrailerJobNonCompatibleDrs>();
            try
            {
                response = await ApiPostCall<List<TrailerJobNonCompatibleDrs>>(ApplicationConstants.UrlValidateTrailerJobCompatibilityForLoadQueue, models);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "ValidateTrailerJobCompatibilityForLoadQueue", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<int?>> GetdeliverySchedules(List<DeliveryRequestViewModel> deliveryRequests)
        {
            List<int?> response = new List<int?>();
            try
            {
                var trackableScheduleIds = deliveryRequests.Where(t => t.TrackableScheduleId != null && t.TrackableScheduleId > 0).Select(t => t.TrackableScheduleId).ToList();
                if (trackableScheduleIds.Any())
                {
                    response = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.IsActive && trackableScheduleIds.Contains(t.Id)
                                                && (t.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Completed || t.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.CompletedLate || t.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.RescheduledCompleted || t.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.RescheduledLate))
                                                .Select(t => t.DriverId).ToListAsync();
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetdeliverySchedules", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DriverScheduleViewModel>> GetSelectedDateDriverScheduleByDriverId(int driverid, string selectedDate)
        {
            var response = new List<DriverScheduleViewModel>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetSelectedDateDriverScheduleByDriverId, driverid, selectedDate);
                response = await ApiGetCall<List<DriverScheduleViewModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "getSelectedDateDriverScheduleByDriverId", ex.Message, ex);
            }
            return response;
        }

        public async Task<PreLoadDrResponseViewModel> CreatePreloadForAcrossTheDate(PreLoadDrViewModel viewModel, UserContext userContext)
        {
            var response = new PreLoadDrResponseViewModel();
            var scheduleBuilder = await GetScheduleBuilderData(viewModel.RegionId, viewModel.ShiftEndDate, "", viewModel.SbView, viewModel.SbDsbView, userContext);
            if (scheduleBuilder != null)
            {
                // Clone the drs to create preload for the drs.
                var freightServiceDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
                var prelodDrIds = viewModel.PreloadDrs.Select(t => t.Id).ToList();
                var postLoadedDrs = await freightServiceDomain.CloneDrsForPreload(prelodDrIds, userContext);
                postLoadedDrs.ForEach(t => { t.ScheduleStatus = 14; t.Status = (int)DeliveryReqStatus.Draft; }); // Set ScheduleStatus to New DR
                if (viewModel.IsTrailerExists)
                {
                    response = await CreatePreloadForAssignedTrailer(viewModel, userContext, scheduleBuilder, postLoadedDrs, true);
                }
                else
                {
                    response = await CreatePreloadForAssignedDriver(viewModel, userContext, scheduleBuilder, postLoadedDrs, true);
                }
            }
            else
            {
                response.StatusMessage = string.Format(Resource.errorMessageSbNotFound, viewModel.ShiftEndDate);
            }
            return response;
        }

        private async Task<PreLoadDrResponseViewModel> CreatePreloadForAssignedTrailer(PreLoadDrViewModel viewModel, UserContext userContext, ScheduleBuilderViewModel scheduleBuilder, List<DeliveryRequestViewModel> postLoadedDrs, bool isPreload = false)
        {
            PreLoadDrResponseViewModel response = new PreLoadDrResponseViewModel();
            // Search for the load which is having trailer for preload
            var preloadTrailerIds = viewModel.PreloadTrailers.Select(t => t.Id).ToList();
            var preloadTrailers = scheduleBuilder.Trailers.Where(t => preloadTrailerIds.Contains(t.Id)).ToList();
            if (preloadTrailers != null && preloadTrailers.Any())
            {
                // Trailer found in the DSB, so search for the load which is having this trailer.
                var thisTrip = scheduleBuilder.Trips.Where(t1 => t1.Trailers.Any()
                                && t1.Trailers.Select(t2 => t2.Id).All(t3 => preloadTrailerIds.Any(t4 => t4 == t3)))
                                .OrderBy(t => t.StartDate).ThenBy(t => Convert.ToDateTime(t.StartTime).TimeOfDay).FirstOrDefault();
                if (thisTrip == null)
                {
                    // There is no trip which is assigned to preload trailer. So get the trip which does not have any trailer.
                    thisTrip = scheduleBuilder.Trips.Where(t1 => !t1.Trailers.Any())
                                .OrderBy(t => DateTime.Parse(t.StartDate + " " + t.StartTime)).FirstOrDefault();
                    if (thisTrip != null)
                    {
                        thisTrip.Trailers.AddRange(preloadTrailers);
                    }
                }
                if (thisTrip != null)
                {
                    UpdatePickupLocations(viewModel, postLoadedDrs);
                    SetDeliveryRequestsToTrip(postLoadedDrs, thisTrip);
                    var dsbSaveModel = scheduleBuilder.ToDsbSaveModel();
                    dsbSaveModel.IsPreloadSchedule = isPreload;
                    dsbSaveModel.Trips = new List<TripViewModel>();
                    dsbSaveModel.Trips.Add(thisTrip);
                    var preloadResponse = await SavePreloadAcrossTheDate(viewModel, userContext, postLoadedDrs, thisTrip, dsbSaveModel);
                    response = ConstructPreloadAcrossTheDateResponse(viewModel, postLoadedDrs, thisTrip, preloadResponse);
                }
                else
                {
                    response.StatusMessage = string.Format(Resource.errorMessageLoadNotFound, viewModel.ShiftEndDate);
                }
            }
            else
            {
                response.StatusMessage = Resource.errorMessageTraileNotFound;
            }
            return response;
        }

        private async Task<PreLoadDrResponseViewModel> CreatePreloadForAssignedDriver(PreLoadDrViewModel viewModel, UserContext userContext, ScheduleBuilderViewModel scheduleBuilder, List<DeliveryRequestViewModel> postLoadedDrs, bool isPreload = false)
        {
            PreLoadDrResponseViewModel response = new PreLoadDrResponseViewModel();
            // Search for the load which is having driver for preload
            var preloadDriver = viewModel.PreloadDrivers.FirstOrDefault();
            if (preloadDriver != null)
            {
                // Driver found in the DSB, so search for the load which is having this driver.
                var thisTrip = scheduleBuilder.Trips.Where(t1 => t1.Drivers.Select(t2 => t2.Id).Contains(preloadDriver.Id) && t1.ShiftId != viewModel.ShiftId)
                                .OrderBy(t => t.StartDate).ThenBy(t => Convert.ToDateTime(t.StartTime).TimeOfDay).FirstOrDefault();
                if (thisTrip == null)
                {
                    // There is no trip which is assigned to preload driver. So get the trip which does not have any driver.
                    thisTrip = scheduleBuilder.Trips.Where(t1 => !t1.Drivers.Any())
                                .OrderBy(t => DateTime.Parse(t.StartDate + " " + t.StartTime)).FirstOrDefault();
                    //if (thisTrip != null)
                    //	thisTrip.Drivers.Add(preloadDriver);
                }
                if (thisTrip != null)
                {
                    UpdatePickupLocations(viewModel, postLoadedDrs);
                    SetDeliveryRequestsToTrip(postLoadedDrs, thisTrip);
                    var dsbSaveModel = scheduleBuilder.ToDsbSaveModel();
                    dsbSaveModel.IsPreloadSchedule = isPreload;
                    dsbSaveModel.Trips = new List<TripViewModel>();
                    dsbSaveModel.Trips.Add(thisTrip);
                    var preloadResponse = await SavePreloadAcrossTheDate(viewModel, userContext, postLoadedDrs, thisTrip, dsbSaveModel);
                    response = ConstructPreloadAcrossTheDateResponse(viewModel, postLoadedDrs, thisTrip, preloadResponse);
                }
                else
                {
                    response.StatusMessage = string.Format(Resource.errorMessageLoadNotFound, viewModel.ShiftEndDate);
                }
            }
            else
            {
                response.StatusMessage = Resource.errorMessageTraileNotFound;
            }
            return response;
        }

        private static PreLoadDrResponseViewModel ConstructPreloadAcrossTheDateResponse(PreLoadDrViewModel viewModel, List<DeliveryRequestViewModel> postLoadedDrs, TripViewModel thisTrip, DSBSaveModel preloadResponse)
        {
            PreLoadDrResponseViewModel response = new PreLoadDrResponseViewModel();
            response.StatusCode = preloadResponse.StatusCode;
            response.StatusMessage = preloadResponse.StatusMessage;
            if (preloadResponse.StatusCode == (int)Status.Success)
            {
                var preloadInfo = postLoadedDrs.Select(t => new PreLoadDrModel
                {
                    Id = t.PostLoadedFor,
                    PreLoadedForId = t.Id
                });
                response.PreloadDrs.AddRange(preloadInfo);
                if (thisTrip.DeliveryGroupStatus == DeliveryGroupStatus.Published)
                {
                    response.StatusMessage = string.Format(Resource.successMessagePreloadCreatedWithPublish, viewModel.ShiftEndDate);
                }
                else
                {
                    response.StatusMessage = string.Format(Resource.successMessagePreloadCreated, viewModel.ShiftEndDate);
                }
            }
            return response;
        }

        private async Task<DSBSaveModel> SavePreloadAcrossTheDate(PreLoadDrViewModel viewModel, UserContext userContext, List<DeliveryRequestViewModel> postLoadedDrs, TripViewModel thisTrip, DSBSaveModel dsbSaveModel)
        {
            DSBSaveModel preloadResponse = dsbSaveModel;
            if (thisTrip.DeliveryGroupStatus == DeliveryGroupStatus.Published)
            {
                preloadResponse = await PublishScheduleBuilder(dsbSaveModel, userContext);
            }
            else
            {
                var scheduleBuilders = new List<DSBSaveModel>() { dsbSaveModel };
                var saveResponse = await ApiPostCall<List<DSBSaveModel>>(ApplicationConstants.UrlSaveSheduleBuilder, scheduleBuilders);
                if (saveResponse != null && saveResponse.Any())
                {
                    preloadResponse = saveResponse.First();
                }
            }
            return preloadResponse;
        }

        private static void UpdatePickupLocations(PreLoadDrViewModel viewModel, List<DeliveryRequestViewModel> postLoadedDrs)
        {
            for (int index = 0; index < postLoadedDrs.Count; index++)
            {
                var postloadedDr = postLoadedDrs[index];
                var dr = viewModel.PreloadDrs.FirstOrDefault(t => t.Id == postloadedDr.PostLoadedFor);
                if (dr != null)
                {
                    postloadedDr.OrderId = dr.OrderId;
                    postloadedDr.PickupLocationType = dr.PickupLocationType;
                    if (postloadedDr.PickupLocationType == PickupLocationType.BulkPlant)
                    {
                        postloadedDr.BulkPlant = dr.BulkPlant;
                    }
                    else
                    {
                        postloadedDr.Terminal = dr.Terminal;
                    }
                }
            }
        }

        private static void SetDeliveryRequestsToTrip(List<DeliveryRequestViewModel> postLoadedDrs, TripViewModel thisTrip)
        {
            if (thisTrip.DeliveryGroupPrevStatus == DeliveryGroupStatus.Published)
            {
                thisTrip.TripStatus = TripStatus.Modified;
                thisTrip.DeliveryGroupStatus = DeliveryGroupStatus.Published;
                postLoadedDrs.ForEach(t => t.Status = (int)DeliveryReqStatus.ScheduleCreated);
            }
            thisTrip.DeliveryRequests.InsertRange(0, postLoadedDrs);
        }

        public async Task<List<RecurringDRSchedule>> GetRecurringScheduleDetails(int jobId, string PoNumber, string siteId, int? productTypeId = null)
        {
            List<RecurringDRSchedule> recurringDRSchedules = new List<RecurringDRSchedule>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetRecurringSchedule, jobId);
                recurringDRSchedules = await ApiGetCall<List<RecurringDRSchedule>>(apiUrl);
                if (!string.IsNullOrEmpty(siteId) && recurringDRSchedules.Count > 0)
                {
                    recurringDRSchedules = recurringDRSchedules.Where(top => top.SiteId == siteId).ToList();
                }
                if (productTypeId.HasValue)
                {
                    recurringDRSchedules = recurringDRSchedules.Where(top => top.ProductTypeId == productTypeId).ToList();
                }
                else if (!string.IsNullOrEmpty(PoNumber))
                {
                    recurringDRSchedules = recurringDRSchedules.Where(top => top.PoNumber.Contains(PoNumber)).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetRecurringScheduleDetails", ex.Message, ex);
            }
            return recurringDRSchedules;
        }

        public async Task<List<DropdownDisplayExtended>> GetRouteDetails(string regionId)
        {
            List<DropdownDisplayExtended> response = new List<DropdownDisplayExtended>();
            try
            {
                if (!string.IsNullOrEmpty(regionId))
                {
                    var apiUrl = string.Format(ApplicationConstants.UrlGetTPORouteInfoDetails, regionId);
                    response = await ApiGetCall<List<DropdownDisplayExtended>>(apiUrl);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetRouteDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<InvoiceRouteInfo>> GetInvoiceRouteInfo(List<string> delReqId)
        {
            var response = new List<InvoiceRouteInfo>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetInvoiceRouteInfo;
                response = await ApiPostCall<List<InvoiceRouteInfo>>(apiUrl, delReqId);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetInvoiceRouteInfo", ex.Message, ex);
            }
            return response;
        }

        public async Task<UnassignDriverViewModel> UnAssignDriverFromShift(UnassignDriverViewModel removeDriver)
        {
            var response = new UnassignDriverViewModel();
            if (!string.IsNullOrWhiteSpace(removeDriver.sbId))
            {
                var apiResponse = await ApiPostCall<UnassignDriverViewModel>(ApplicationConstants.UrlUnassignDriverFromShift, removeDriver);
                if (apiResponse.StatusCode == Status.Success)
                {
                    response.Trips = apiResponse.Trips;
                    response.StatusCode = Status.Success;
                    response.StatusMessage = apiResponse.StatusMessage;
                }
                else
                {
                    response.Trips = apiResponse.Trips;
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = apiResponse.StatusMessage;
                }
            }
            return response;
        }
        public async Task<ScheduleBuilderViewModel> GetRecurringScheduleBuilderData(string regionId, string date, string scheduleBuilderId, int view, UserContext userContext, List<RecurringShiftDetails> shiftInformation, string scheduleBuilderviewId)
        {
            int defaultScheduleBuilderView = 2;
            var IsDsbDriverSchedule = Context.DataContext.OnboardingPreferences.Where(top => top.IsActive == true && top.CompanyId == userContext.CompanyId).OrderByDescending(top => top.Id).Select(x => x.IsDSBDriverSchedule).FirstOrDefault();
            ScheduleBuilderViewModel response = new ScheduleBuilderViewModel();
            try
            {
                RecurringScheduleBuilder input = new RecurringScheduleBuilder();
                input.RegionId = regionId;
                input.Date = date;
                input.ScheduleBuilderId = scheduleBuilderId;
                input.View = view == 1 ? defaultScheduleBuilderView : view;
                input.UserId = userContext.Id;
                input.CompanyId = userContext.CompanyId;
                input.ShiftInformation = shiftInformation;
                input.ScheduleBuilderViewId = scheduleBuilderviewId;
                input.IsDsbDriverSchedule = IsDsbDriverSchedule;
                var apiUrl = string.Format(ApplicationConstants.UrlGetRecurringSheduleBuilderDetails);
                response = await ApiPostCall<ScheduleBuilderViewModel>(apiUrl, input);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetRecurringSheduleBuilderData", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<TfxCarrierDropdownDisplayItem>> GetRegionCarriers(string regionId)
        {
            List<TfxCarrierDropdownDisplayItem> response = new List<TfxCarrierDropdownDisplayItem>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetAssignCarrierRegion, regionId);
                response = await ApiGetCall<List<TfxCarrierDropdownDisplayItem>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetRegionCarriers", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DropdownDisplayExtendedItem>> GetSupplierChildOrders(int OrderId)
        {
            List<DropdownDisplayExtendedItem> response = new List<DropdownDisplayExtendedItem>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var childOrders = await spDomain.GetBrokeredChildOrders(OrderId);
                childOrders = childOrders.GroupBy(u => u.FuelRequestId).Select(grp => grp.FirstOrDefault()).ToList();
                childOrders.Where(top => top.IsDispatchRetainedByCustomer == true).ToList().ForEach(x => response.Add(new DropdownDisplayExtendedItem { Id = x.BrokeredToCompanyId, Name = x.BrokeredToCompanyName }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetOrderSupplierDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<TrailerFuelRetainViewModel>> GetTrailerFuelRetainDetails(string trailerId)
        {
            List<TrailerFuelRetainViewModel> response = new List<TrailerFuelRetainViewModel>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetTrailerFuelRetainDetails, trailerId);
                response = await ApiGetCall<List<TrailerFuelRetainViewModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetTrailerFuelRetainDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateDeliveryRequestCompartmentInfo(List<DeliveryRequestCompartmentInfoViewModel> drRequestModels)
        {
            var response = new StatusViewModel();
            try
            {
                if (drRequestModels != null && drRequestModels.Any())
                {
                    response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlUpdateDeliveryRequestCompartmentInfo, drRequestModels);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "UpdateDeliveryRequestCompartmentInfo", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveTrailerFuelRetain(List<TrailerFuelRetainViewModel> trailerFuelRetainViewModel)
        {
            var response = new StatusViewModel();
            if (trailerFuelRetainViewModel != null)
            {
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlSaveTrailerFuelRetain, trailerFuelRetainViewModel);
            }
            else
            {
                response.StatusCode = Status.Warning;
            }
            return response;
        }
        private async Task<List<int>> GetBrokerJobOrderDetails(int companyId, List<int> OrderId)
        {
            var response = new List<int>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetBrokerJobOrderDetails + "?companyId=" + companyId;
                if (OrderId.Count() > 0)
                {
                    response = await ApiPostCall<List<int>>(apiUrl, OrderId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetBrokerJobOrderDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ScheduleOttoDRs(UserContext userContext, OttoBuilder ottoBuilder)
        {
            var response = new StatusViewModel();
            try
            {
                var scheduleBuilder = await GetScheduleBuilderData(ottoBuilder.RegionId, ottoBuilder.Date, null, 1, 2, userContext);
                if (scheduleBuilder != null)
                {
                    var sbDriverView = ScheduleBuilderConverter.ConvertToDriverViewModel(scheduleBuilder);
                    var dsbSaveModel = GetDSBSaveModel(sbDriverView);
                    dsbSaveModel.CompanyId = userContext.CompanyId;
                    dsbSaveModel.UserId = userContext.Id;

                    var shift = sbDriverView.Shifts.FirstOrDefault(t => t.Id == ottoBuilder.ShiftId);
                    if (shift != null)
                    {
                        var schedule = shift.Schedules[0];
                        foreach (var item in ottoBuilder.Loads)
                        {
                            var thisTrip = schedule.Trips.FirstOrDefault(t => t.DriverRowIndex == item.DriverRowIndex && t.DriverColIndex == item.DriverColIndex);
                            if (thisTrip != null && item.DeliveryRequests.Any())
                            {
                                // Add selected DRs in the trip of schedule builder
                                item.DeliveryRequests.ForEach(t => { t.ScheduleStatus = 14; t.Status = 5; });
                                thisTrip.DeliveryRequests.AddRange(item.DeliveryRequests);
                                SetTripStatus(thisTrip);
                                thisTrip.DeliveryGroupStatus = DeliveryGroupStatus.Draft;
                                thisTrip.Drivers = schedule.Drivers;
                                thisTrip.Trailers = schedule.Trailers;
                                dsbSaveModel.Trips.Add(thisTrip);
                            }
                        }
                        var scheduleBuilders = new List<DSBSaveModel>() { dsbSaveModel };
                        var saveResponse = await ApiPostCall<List<DSBSaveModel>>(ApplicationConstants.UrlSaveSheduleBuilder, scheduleBuilders);
                        if (saveResponse != null && saveResponse.Any())
                        {
                            response.StatusCode = saveResponse.First().StatusCode;
                            response.StatusMessage = saveResponse.First().StatusMessage;
                        }
                    }
                    else
                    {
                        response.StatusMessage = $"No shift found in the schedule builder.";
                    }
                }
                else
                {
                    response.StatusMessage = $"No schedule builder found for the date {ottoBuilder.Date}.";
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetBrokerJobOrderDetails", ex.Message, ex);
            }
            return response;
        }

        private DSBSaveModel GetDSBSaveModel(SbDriverViewModel sbModel)
        {
            var dataToSave = new DSBSaveModel();
            dataToSave.Id = sbModel.Id;
            dataToSave.Date = sbModel.Date;
            dataToSave.RegionId = sbModel.RegionId;
            dataToSave.ObjectFilter = sbModel.ObjectFilter;
            dataToSave.RegionFilter = sbModel.RegionFilter;
            dataToSave.DateFilter = sbModel.DateFilter;
            dataToSave.TimeStamp = sbModel.TimeStamp;
            dataToSave.Status = sbModel.Status;
            dataToSave.WindowMode = sbModel.WindowMode;
            dataToSave.ToggleRequestMode = sbModel.ToggleRequestMode;
            if (sbModel.Id == null)
            {
                for (var i = 0; i < sbModel.Shifts.Count; i++)
                {
                    var shift = new ShiftModel();
                    shift.Id = sbModel.Shifts[i].Id;
                    shift.StartTime = sbModel.Shifts[i].StartTime;
                    shift.EndTime = sbModel.Shifts[i].EndTime;
                    shift.SlotPeriod = sbModel.Shifts[i].SlotPeriod;
                    dataToSave.Shifts.Add(shift);
                }
            }
            // add other fields
            dataToSave.CompanyId = sbModel.CompanyId;
            dataToSave.Date = sbModel.Date;
            dataToSave.DSBFilter = sbModel.DSBFilter;
            dataToSave.DeletedDriverScheduleMappingId = sbModel.DeletedDriverScheduleMappingId;
            dataToSave.DeletedGroupId = sbModel.DeletedGroupId;
            dataToSave.DeletedTripId = sbModel.DeletedTripId;
            dataToSave.WindowMode = sbModel.WindowMode;
            dataToSave.IsDriverScheduleReset = sbModel.IsDriverScheduleReset;

            return dataToSave;
        }

        private void SetTripStatus(TripViewModel trip)
        {
            if (trip != null)
            {
                var tripPrevStatusId = trip.TripPrevStatus;
                var tripStatusId = TripStatus.Added;
                if (tripPrevStatusId == TripStatus.None)
                {
                    tripStatusId = TripStatus.Added;
                }
                else if (tripPrevStatusId == TripStatus.Added || tripPrevStatusId == TripStatus.Modified)
                {
                    tripStatusId = TripStatus.Modified;
                }
                trip.TripStatus = tripStatusId;
            }
        }
        public async Task<List<DsbLoadQueueDetails>> GetScheduleLoadQueueStatus(string SchedulebuilderId, string Date, string RegionId, int TfxCompanyId, int TfxUserId)
        {
            var dsbLoadQueueColumnsDetails = await Context.DataContext.DsbLoadQueueDetails.Where(top => (top.ScheduleBuilderId == SchedulebuilderId || top.Date == Date) && top.RegionId == RegionId && top.TfxCompanyId == TfxCompanyId && top.TfxUserId == TfxUserId).ToListAsync();
            return dsbLoadQueueColumnsDetails;
        }

        public async Task<BooleanResponseModel> IsFilldCompatibleOrder(List<int> orderIds)
        {
            var response = new BooleanResponseModel();
            try
            {
                var orderAssetDetails = await Context.DataContext.Orders.Where(t => orderIds.Contains(t.Id))
                                                            .Select(t => new
                                                            {
                                                                t.Id,
                                                                t.FuelRequest.MstProduct.MstProductType.Name,
                                                                AvailableAssets =
                                                                        t.FuelRequest.Job.JobXAssets.Any(t1 => t1.Asset.IsActive
                                                                        && t1.RemovedBy == null && t1.RemovedDate == null
                                                                        && t1.Asset.FuelType == t.FuelRequest.MstProduct.ProductTypeId)
                                                            }).ToListAsync();
                response.Result = true;
                var notcompatibleOrders = orderAssetDetails.Where(t => !t.AvailableAssets).ToList();
                if (notcompatibleOrders.Any())
                {
                    response.Result = false;
                    var fuelTypes = string.Join(", ", notcompatibleOrders.Select(t => t.Name));
                    response.Message = string.Format(Resource.errMessageAssetNotAvailable, fuelTypes);
                }
                response.Status = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "IsFilldCompatibleOrder", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DriverAdditionalDetailsViewModel>> GetCompanyDrivers(int companyIds, string regionIds, string otherRegion, string selectedDate, string shiftId)
        {
            List<DriverAdditionalDetailsViewModel> response = new List<DriverAdditionalDetailsViewModel>();
            try
            {
                var IsDsbDriverSchedule = Context.DataContext.OnboardingPreferences.Where(top => top.IsActive == true && top.CompanyId == companyIds).OrderByDescending(top => top.Id).Select(x => x.IsDSBDriverSchedule).FirstOrDefault();
                var inputData = new { companyId = companyIds, regionId = regionIds, otherRegion = otherRegion, selectedDate = selectedDate, shiftId = shiftId, IsDsbDriverSchedule = IsDsbDriverSchedule };
                var apiResult = await ApiPostCall<List<DriverAdditionalDetailsViewModel>>(ApplicationConstants.UrlGetAllShiftDriverDetailsScheduleBuilder, inputData);
                response = await SetDriverOnboardingStatus(apiResult, companyIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetGridViewCompanyDrivers", ex.Message, ex);
            }

            return response;

        }
        public async Task<List<DriverScheduleViewModel>> GetSelectedDateDriverScheduleByDriverId(int driverid, string selectedDate, string shiftId)
        {
            var response = new List<DriverScheduleViewModel>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetSelectedDateDriverScheduleByDriverIdGridView, driverid, selectedDate, shiftId);
                response = await ApiGetCall<List<DriverScheduleViewModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "getSelectedDateDriverScheduleByDriverId", ex.Message, ex);
            }
            return response;
        }
        #region ResetDsbScheduleBuilder
        public async Task<StatusViewModel> ResetDsbScheduleBuilderDriver(List<int> deliveryGroupIds, ScheduleBuilderViewModel scheduleBuilderView, bool publishedTrip, UserContext userContext)
        {
            StatusViewModel statusViewModel = new StatusViewModel();
            var IsDsbDriverSchedule = Context.DataContext.OnboardingPreferences.Where(top => top.IsActive == true && top.CompanyId == userContext.CompanyId).OrderByDescending(top => top.Id).Select(x => x.IsDSBDriverSchedule).FirstOrDefault();
            if (IsDsbDriverSchedule)
            {
                if (deliveryGroupIds.Any() && publishedTrip)
                {
                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            Context.DataContext.Database.CommandTimeout = 180;//3 minutes timeout
                            await RemoveDSFromDeliveryGroup(deliveryGroupIds, userContext);
                            DSBSaveModel apiResponse = await ResetDSDriverScheduleInfo(scheduleBuilderView);
                            if (apiResponse != null)
                            {
                                if (apiResponse.StatusCode == Status.Success)
                                {
                                    statusViewModel.StatusCode = Status.Success;
                                    transaction.Commit();
                                }
                                else
                                {
                                    transaction.Rollback();
                                }
                            }
                            else
                            {
                                transaction.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            statusViewModel.StatusCode = Status.Failed;
                            LogManager.Logger.WriteException("ScheduleBuilderDomain", "ResetDsbScheduleBuilderDriver", ex.Message, ex);
                        }
                    }
                }
                else
                {
                    scheduleBuilderView.IsDriverScheduleReset = true;
                    var dsbSaveModel = scheduleBuilderView.ToDsbSaveModel();
                    dsbSaveModel.Trips = new List<TripViewModel>();
                    dsbSaveModel.Trips.AddRange(scheduleBuilderView.Trips);
                    List<DSBSaveModel> scheduleBuilders = await ProcessAcrossTheDateDrsEdit(dsbSaveModel);
                    var apiResponse = await ApiPostCall<List<DSBSaveModel>>(ApplicationConstants.UrlSaveSheduleBuilder, scheduleBuilders);
                    if (apiResponse != null)
                    {
                        if (apiResponse != null)
                        {
                            if (apiResponse.All(t => t.StatusCode == Status.Success))
                            {
                                statusViewModel.StatusCode = Status.Success;
                            }
                            else
                            {
                                statusViewModel.StatusCode = Status.Failed;
                                statusViewModel.StatusMessage = apiResponse.FirstOrDefault().StatusMessage;
                            }
                        }

                    }

                }
            }
            return statusViewModel;
        }

        private async Task<DSBSaveModel> ResetDSDriverScheduleInfo(ScheduleBuilderViewModel scheduleBuilderView)
        {
            UserContext userContext = new UserContext();
            scheduleBuilderView.IsDriverScheduleReset = true;
            var dsbSaveModel = scheduleBuilderView.ToDsbSaveModel();
            dsbSaveModel.IsDriverScheduleReset = true;
            dsbSaveModel.Trips = new List<TripViewModel>();
            dsbSaveModel.Trips.AddRange(scheduleBuilderView.Trips);
            userContext.CompanyId = dsbSaveModel.CompanyId;
            userContext.Id = dsbSaveModel.UserId;
            var apiResponse = await SaveScheduleBuilder(dsbSaveModel, userContext);
            return apiResponse;
        }


        private async Task RemoveDSFromDeliveryGroup(List<int> deliveryGroupIds, UserContext userContext)
        {

            foreach (var item in deliveryGroupIds)
            {
                List<ScheduleNotificationModel> groupChangesForNotifications = new List<ScheduleNotificationModel>();
                await DeleteGroup(item, groupChangesForNotifications, userContext);
            }

        }
        #endregion

        public async Task<ResetDeliveryGroupScheduleModel> RemoveScheduleBuilderDrs(ResetDeliveryGroupScheduleModel model)
        {
            var response = new ResetDeliveryGroupScheduleModel();
            try
            {
                response = await ApiPostCall<ResetDeliveryGroupScheduleModel>(ApplicationConstants.UrlRemoveScheduleBuilderDrs, model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "RemoveScheduleBuilderDrs", ex.InnerException + " DrIds -" + string.Join(",", model.DeliveryRequestIds + string.Join(",", model.DeliveryRequestIds + " DSB ID -" + model.ScheduleBuilderId)), null);
            }
            return response;
        }
        public async Task<OnTheFlyLocationModel> GetPreferenceSettingForOnTheFlyLocation(UserContext userContext)
        {
            var response = new OnTheFlyLocationModel();
            try
            {
                var onboardingPreferencesSetting = await Context.DataContext.OnboardingPreferences
                                                                            .Where(t => t.IsActive && t.CompanyId == userContext.CompanyId)
                                                                            .OrderByDescending(t => t.Id)
                                                                            .Select(t => new
                                                                            {
                                                                                t.Id,
                                                                                t.DeliveryType,
                                                                                t.IsCustomerInvitationEnabled,
                                                                                t.FreightOnBoardType,
                                                                                t.IsSupressOrderPricing,
                                                                                t.IsDropTicketImageRequired,
                                                                                t.IsBadgeMandatory
                                                                            })
                                                                            .FirstOrDefaultAsync();
                if (onboardingPreferencesSetting != null)
                {
                    response.PreferenceSettingId = onboardingPreferencesSetting.Id;
                    response.IsSupressOrderPricing = onboardingPreferencesSetting.IsSupressOrderPricing;
                    response.FuelDetails.FuelPricing.FuelPricingDetails.FreightOnBoardTypes = onboardingPreferencesSetting.FreightOnBoardType;
                    response.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypes = onboardingPreferencesSetting.DeliveryType;
                    response.CustomerDetails.IsInvitationEnabled = onboardingPreferencesSetting.IsCustomerInvitationEnabled;
                    response.FuelDeliveryDetails.IsDropImageRequired = onboardingPreferencesSetting.IsDropTicketImageRequired;
                    response.IsBadgeMandatory = onboardingPreferencesSetting.IsBadgeMandatory;
                }
                else
                {
                    response.FuelDetails.FuelPricing.FuelPricingDetails.FreightOnBoardTypes = FreightOnBoardTypes.Destination;
                }
                response.FuelDeliveryDetails.DeliveryTypeId = (int)DeliveryType.MultipleDeliveries;
                response.FuelDetails.FuelQuantity.QuantityIndicatorTypes = QuantityIndicatorTypes.Net;
                response.FuelDetails.FuelDisplayGroupId = (int)ProductDisplayGroups.CommonFuelType;
                response.FuelDetails.FuelQuantity.QuantityTypeId = (int)QuantityType.NotSpecified;
                response.FuelDeliveryDetails.FuelRequestFee.DeliveryFeeByQuantity.Add(new DeliveryFeeByQuantityViewModel());
                var masterDomain = new MasterDomain(this);
                var defaultaddresscountryId = masterDomain.GetDefaultServingCountry(userContext.CompanyId);
                var defaultCurrency = masterDomain.GetDefaultCurrencyForCompany(userContext.CompanyId);
                var defaultUoM = masterDomain.GetDefaultUoMforCompany(userContext.CompanyId);
                response.AddressDetails.Country.Id = defaultaddresscountryId;
                response.AddressDetails.Country.Currency = defaultCurrency;
                response.AddressDetails.Country.UoM = defaultUoM;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetPreferenceSettingForOnTheFlyLocation", ex.Message, ex);
            }

            return response;
        }






        #region Reset Published Schedule

        public async Task<StatusViewModel> ResetDeliveryGroup(List<int> deliveryGroupIds, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    var groups = Context.DataContext.DeliveryGroups.Where(ds1 => deliveryGroupIds.Contains(ds1.Id)).Select(ds2 =>
                    new
                    {
                        DeliveryGroup = ds2,
                        ds2.DeliveryScheduleXTrackableSchedules,
                        DeliveryScheduleXDrivers = ds2.DeliverySchedules.Select(d => d.DeliveryScheduleXDrivers),
                        ds2.DeliverySchedules
                    }).FirstOrDefault();

                    //DeliveryGroup
                    groups.DeliveryGroup.IsActive = false;
                    groups.DeliveryGroup.UpdatedBy = 0;
                    groups.DeliveryGroup.UpdatedDate = DateTimeOffset.Now;

                    //DeliveryScheduleXTrackableSchedules
                    groups.DeliveryScheduleXTrackableSchedules.ToList().ForEach(t => { t.IsActive = false; t.DeliveryGroupId = null; t.DeliveryScheduleStatusId = (int)TrackableDeliveryScheduleStatus.Canceled; t.FrDeliveryRequestId = null; });

                    //DeliveryScheduleXDrivers
                    groups.DeliveryScheduleXDrivers.ToList().ForEach(c => c.ToList().ForEach(d => d.IsActive = false));

                    //DeliverySchedules
                    groups.DeliverySchedules.ToList().ForEach(t => { t.DeliveryGroupId = null; t.StatusId = (int)DeliveryScheduleStatus.Canceled; });

                    await Context.CommitAsync();
                    transaction.Commit();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;

                    //Notification
                    var groupChanges = new List<ScheduleNotificationModel>();

                    bool isTodayScheduleExists = false;
                    int notifyTime = 12;
                    foreach (var tr in groups.DeliveryScheduleXTrackableSchedules)
                    {
                        DateTimeOffset jobTime = DateTimeOffset.Now;
                        if (tr.OrderId != null)
                        {
                            jobTime = jobTime.ToTargetDateTimeOffset(tr.Order.FuelRequest.Job.TimeZoneName);
                            notifyTime = 12;
                        }
                        else
                        {
                            notifyTime = 24;
                        }
                        double trackScheduleStartTimeDiff = Math.Abs((tr.Date.Date.Add(tr.StartTime) - jobTime.DateTime).TotalHours);
                        double trackScheduleEndTimeDiff = Math.Abs((tr.Date.Date.Add(tr.EndTime) - jobTime.DateTime).TotalHours);
                        if (tr.Date.Date == jobTime.Date || trackScheduleStartTimeDiff <= notifyTime || trackScheduleEndTimeDiff <= notifyTime)
                        {
                            isTodayScheduleExists = true;
                            break;
                        }
                    }
                    if (isTodayScheduleExists)
                    {
                        groups.DeliveryScheduleXTrackableSchedules.ToList().ForEach(t => groupChanges.Add(new ScheduleNotificationModel() { TrackableScheduleId = t.Id, ScheduleId = t.DeliveryScheduleId, OrderId = t.OrderId ?? 0, ScheduleStatus = (int)DeliveryScheduleStatus.Canceled, PreviousDriverId = t.DriverId }));
                    }
                    if (groupChanges.Any())
                    {
                        await new PushNotificationDomain(this).PushSbChangesNotificationToDriver(groupChanges, userContext.UserName);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "ResetDeliveryGroup", ex.Message + " Delivery Group Ids - " + string.Join(",", deliveryGroupIds), ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ResetDeliverySchedules(List<int> deliveryScheduleIds, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    var schedules = Context.DataContext.DeliverySchedules.Where(ds1 => deliveryScheduleIds.Contains(ds1.Id)).Select(ds2 =>
                        new
                        {
                            ds2.DeliveryScheduleXTrackableSchedules,
                            ds2.DeliveryScheduleXDrivers,
                            DeliverySchedules = ds2
                        }).FirstOrDefault();

                    //DeliveryScheduleXTrackableSchedules
                    schedules.DeliveryScheduleXTrackableSchedules.ToList().ForEach(t => { t.IsActive = false; t.DeliveryGroupId = null; t.DeliveryScheduleStatusId = (int)TrackableDeliveryScheduleStatus.Canceled; t.FrDeliveryRequestId = null; });

                    //DeliveryScheduleXDrivers
                    schedules.DeliveryScheduleXDrivers.ToList().ForEach(c => c.IsActive = false);

                    //DeliverySchedules
                    schedules.DeliverySchedules.DeliveryGroupId = null;
                    schedules.DeliverySchedules.StatusId = (int)DeliveryScheduleStatus.Canceled;

                    await Context.CommitAsync();
                    transaction.Commit();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;


                    //Notification
                    var groupChanges = new List<ScheduleNotificationModel>();

                    bool isTodayScheduleExists = false;
                    int notifyTime = 12;
                    foreach (var tr in schedules.DeliveryScheduleXTrackableSchedules)
                    {
                        DateTimeOffset jobTime = DateTimeOffset.Now;
                        if (tr.OrderId != null)
                        {
                            jobTime = jobTime.ToTargetDateTimeOffset(tr.Order.FuelRequest.Job.TimeZoneName);
                            notifyTime = 12;
                        }
                        else
                        {
                            notifyTime = 24;
                        }
                        double trackScheduleStartTimeDiff = Math.Abs((tr.Date.Date.Add(tr.StartTime) - jobTime.DateTime).TotalHours);
                        double trackScheduleEndTimeDiff = Math.Abs((tr.Date.Date.Add(tr.EndTime) - jobTime.DateTime).TotalHours);
                        if (tr.Date.Date == jobTime.Date || trackScheduleStartTimeDiff <= notifyTime || trackScheduleEndTimeDiff <= notifyTime)
                        {
                            isTodayScheduleExists = true;
                            break;
                        }
                    }
                    if (isTodayScheduleExists)
                    {
                        schedules.DeliveryScheduleXTrackableSchedules.ToList().ForEach(t => groupChanges.Add(new ScheduleNotificationModel() { TrackableScheduleId = t.Id, ScheduleId = t.DeliveryScheduleId, OrderId = t.OrderId ?? 0, ScheduleStatus = (int)DeliveryScheduleStatus.Canceled, PreviousDriverId = t.DriverId }));
                    }
                    if (groupChanges.Any())
                    {
                        await new PushNotificationDomain(this).PushSbChangesNotificationToDriver(groupChanges, userContext.UserName);
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "ResetDeliverySchedules", ex.Message + " Delivery Schedule Ids - " + string.Join(",", deliveryScheduleIds), ex);
            }
            return response;
        }

        #endregion Reset Published Schedule
        #region OptionPickup
        private static ScheduleAdditionalInfo IntializeOptionalPickupInfo(TripViewModel group, DSBSaveModel scheduleBuilder, ScheduleAdditionalInfo additionalInfo)
        {
            if (scheduleBuilder != null && group != null)
            {
                if (additionalInfo == null)
                {
                    additionalInfo = new ScheduleAdditionalInfo();
                }
                additionalInfo.ScheduleBuilderId = scheduleBuilder.Id;
                additionalInfo.ShiftId = group.ShiftId;
                additionalInfo.ShiftIndex = group.ShiftIndex.GetValueOrDefault();
                additionalInfo.DriverColIndex = group.DriverRowIndex.GetValueOrDefault();
            }

            return additionalInfo;
        }
        public async Task<StatusViewModel> UpdateDeliveryRequestOptionalPickupInfo(List<ScheduleOptionalPickupModel> model)
        {
            var response = new StatusViewModel();
            try
            {
                if (model != null && model.Any())
                {
                    response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlUpdateDROptionalPickupInfo, model);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "UpdateDeliveryRequestOptionalPickupInfo", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> SendOptionalPickupRefreshPushNotificationDriver(string message, int driverId)
        {
            var pushNotificationDomain = new PushNotificationDomain(this);
            var viewModel = new DriverNotificationViewModel();
            viewModel.DriverIds = new List<int>();
            viewModel.Message.Body = message;
            viewModel.Message.Title = Resource.optionalPickupRefresh;
            viewModel.Message.NotificationCode = (int)NotificationCode.OptionalPickupRefresh;
            viewModel.DriverIds.Add(driverId);
            var response = await pushNotificationDomain.NotificationToDriver(viewModel);
            return response;
        }
        #endregion

        #region CancelDriverSchedule
        public async Task<DSBSaveModel> CancelDriverScheduleBuilder(DSBSaveModel scheduleBuilder, UserContext userContext, string language = "en-US")
        {
            var response = scheduleBuilder;
            try
            {
                var apiResponse = await ApiPostCall<DSBSaveModel>(ApplicationConstants.UrlValidateScheduleBuilder, scheduleBuilder);
                if (apiResponse.StatusCode == Status.Failed)
                {
                    return apiResponse;
                }
                List<CreateDeliveryGroupModel> createDeliveryGroupModels = new List<CreateDeliveryGroupModel>();
                List<DSBSaveModel> scheduleBuilders = await ProcessAcrossTheDateDrsEdit(scheduleBuilder);
                for (int index = 0; index < scheduleBuilders.Count; index++)
                {
                    SetEmptyTripsStatusAsDeleted(scheduleBuilders[index]);
                    var createDeliveryGroupModel = GetCancelDeliveryGroupViewModel(scheduleBuilders[index].Trips);
                    createDeliveryGroupModel.ScheduleBuilder = scheduleBuilders[index];
                    createDeliveryGroupModels.Add(createDeliveryGroupModel);
                }
                //CreateDeliveryGroupModel createDeliveryGroupModel = GetDeliveryGroupViewModel(scheduleBuilder.Trips);
                var publishResponse = await CancelScheduleBuilder(scheduleBuilders, userContext, createDeliveryGroupModels, language);
                if (publishResponse != null && publishResponse.Any())
                {
                    response = publishResponse.First();
                }
            }
            catch (Exception ex)
            {
                response = scheduleBuilder;
                response.StatusMessage = Resource.valMessageErrorOccurred;
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "CancelDriverScheduleBuilder", ex.Message, ex);
            }
            return response;
        }

        private async Task<List<DSBSaveModel>> CancelScheduleBuilder(List<DSBSaveModel> scheduleBuilders, UserContext userContext, List<CreateDeliveryGroupModel> createDeliveryGroupModels, string language)
        {
            var response = new List<DSBSaveModel>();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    Context.DataContext.Database.CommandTimeout = 180;//3 minutes timeout
                                                                      //here need to update the Cancelled status for DRs
                    await CancelDeliveryGroups(userContext, createDeliveryGroupModels);
                    var apiResponse = await ApiPostCall<List<DSBSaveModel>>(ApplicationConstants.UrlCancelSheduleBuilder, scheduleBuilders);
                    if (apiResponse != null)
                    {
                        response = apiResponse;
                        if (response.All(t => t.StatusCode == Status.Success))
                        {
                            var trackableScheduleIds = createDeliveryGroupModels.SelectMany(t => t.GroupChanges.Where(t1 => t1.TrackableScheduleId > 0)).Select(t1 => t1.TrackableScheduleId).ToList();
                            var publishDeliveryScheduleIds = createDeliveryGroupModels.SelectMany(t => t.PublishedGroups.SelectMany(t1 => t1.DeliveryRequests).Where(x1 => x1.TrackableScheduleId > 0).Select(x1 => x1.TrackableScheduleId).ToList()).ToList();
                            if (publishDeliveryScheduleIds.Any())
                            {
                                publishDeliveryScheduleIds.ForEach(x1 =>
                                {
                                    if (trackableScheduleIds.FindIndex(x => x == x1.Value) == -1)
                                    {
                                        trackableScheduleIds.Add(x1.Value);
                                    }
                                });
                            }
                            if (trackableScheduleIds.Any())
                            {
                                var deliveryScheduleXTrackableSchedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t1 => trackableScheduleIds.Contains(t1.Id) &&
                                                                        t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Completed
                                                                         && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.CompletedLate
                                                                          && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                                          && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                          && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledLate
                                                                          && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.UnplannedDropCompleted
                                                                          && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled
                                                                          ).ToListAsync();
                                if (deliveryScheduleXTrackableSchedules.Any())
                                {
                                    var postLoadDRsId = deliveryScheduleXTrackableSchedules.Where(x => x.PostLoadedForId != null).Select(x1 => x1.PostLoadedForId).ToList();
                                    if (postLoadDRsId.Any())
                                    {
                                        var postLoaddeliveryScheduleXTrackableSchedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t1 => postLoadDRsId.Contains(t1.Id) &&
                                                                       t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Completed
                                                                        && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.CompletedLate
                                                                         && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                                         && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                         && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledLate
                                                                         && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.UnplannedDropCompleted
                                                                         && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled
                                                                         ).ToListAsync();
                                        if (postLoaddeliveryScheduleXTrackableSchedules.Any())
                                        {
                                            postLoaddeliveryScheduleXTrackableSchedules.ForEach(x1 =>
                                            {
                                                deliveryScheduleXTrackableSchedules.Add(x1);
                                            });
                                        }
                                    }
                                    foreach (var item in deliveryScheduleXTrackableSchedules)
                                    {
                                        item.DeliveryScheduleStatusId = (int)DeliveryScheduleStatus.Canceled;
                                    }
                                    await Context.CommitAsync();
                                    if (IsTodayScheduleChanged(createDeliveryGroupModels))
                                    {
                                        var pushNotificationDomain = new PushNotificationDomain(this);
                                        var groupChanges = createDeliveryGroupModels.SelectMany(t => t.GroupChanges).ToList();
                                        groupChanges.ForEach(x => x.ScheduleStatus = (int)DeliveryScheduleStatus.Canceled);
                                        var pushNotificationResponse = await pushNotificationDomain.PushSbChangesNotificationToDriver(groupChanges, userContext.UserName);
                                        if (pushNotificationResponse.StatusCode == Status.Failed)
                                        {
                                            response.ForEach(t => t.StatusCode = Status.Warning);
                                            response.ForEach(t => t.StatusMessage = t.StatusMessage + " " + pushNotificationResponse.StatusMessage);
                                        }
                                    }
                                }
                                //Save Retain Information.
                                await SaveRetainInfomation(deliveryScheduleXTrackableSchedules);
                            }

                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                            response.ForEach(t => t.StatusCode = Status.Failed);
                            response.ForEach(t => t.StatusMessage = t.StatusMessage != Resource.errMessageFailed ? t.StatusMessage : Resource.valMessageSbPublishFailed);
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        response.ForEach(t => t.StatusCode = Status.Failed);
                        response.ForEach(t => t.StatusMessage = Resource.valMessageServiceNotResponded);
                    }
                }
                catch (Exception ex)
                {
                    response = scheduleBuilders;
                    response.ForEach(t => t.StatusCode = Status.Failed);
                    response.ForEach(t => t.StatusMessage = Resource.valMessageSbPublishFailed);
                    transaction.Rollback();
                    LogManager.Logger.WriteException("ScheduleBuilderDomain", "CancelScheduleBuilder", ex.Message, ex);
                }
            }

            return response;
        }

        private async Task SaveRetainInfomation(List<DeliveryScheduleXTrackableSchedule> deliveryScheduleXTrackableSchedules)
        {
            var trackableScheduleRetainIds = deliveryScheduleXTrackableSchedules.Select(x => x.Id).ToList();
            if (trackableScheduleRetainIds.Any())
            {
                var getRetainInfo = await Context.DataContext.PreLoadBolDetails.Where(x1 => trackableScheduleRetainIds.Contains(x1.TrackableScheduleId.Value) && x1.IsPickupBOLRetain == true).ToListAsync();
                if (getRetainInfo.Any())
                {
                    foreach (var retainitem in getRetainInfo)
                    {
                        try
                        {
                            var retainData = JsonConvert.DeserializeObject<List<TrailerFuelRetainViewModel>>(retainitem.TrailerRetainInfo);
                            if (retainData.Any())
                            {
                                foreach (var item in retainData)
                                {
                                    var trackableScheduleInfo = deliveryScheduleXTrackableSchedules.Where(x => x.Id == item.TrackableScheduleId).FirstOrDefault();
                                    if (trackableScheduleInfo != null)
                                    {
                                        item.DeliveryRequestId = trackableScheduleInfo.FrDeliveryRequestId;
                                    }
                                }
                                var statusModel = await SaveTrailerFuelRetain(retainData);
                                if (statusModel.StatusCode == Status.Failed)
                                {
                                    LogManager.Logger.WriteError("ScheduleBuilderDomain-CancelScheduleBuilder", "SaveTrailerFuelRetain-Issue", statusModel.StatusMessage);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            LogManager.Logger.WriteException("ScheduleBuilderDomain-CancelScheduleBuilder", "Retain-DeserializeObject-Issue", ex.Message, ex);
                        }

                    }
                }
            }
        }

        private async Task<StatusViewModel> CancelDeliveryGroups(UserContext userContext, List<CreateDeliveryGroupModel> createDeliveryGroupModels)
        {
            var response = new StatusViewModel(Status.Failed);

            for (int index = 0; index < createDeliveryGroupModels.Count; index++)
            {
                var createDeliveryGroupModel = createDeliveryGroupModels[index];

                var modifiedTrackableSchedules = new List<DeliveryScheduleXTrackableSchedule>();
                if (createDeliveryGroupModel.TrackableScheduleIds.Any())
                {
                    modifiedTrackableSchedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => createDeliveryGroupModel.TrackableScheduleIds.Contains(t.Id)).ToListAsync();
                }
                foreach (var group in createDeliveryGroupModel.PublishedGroups)
                {
                    if (group.TripStatus == TripStatus.Added || group.TripStatus == TripStatus.Modified)
                    {
                        List<int> enrouteInCompleted = new List<int> { 3, 7, 8, 9, 22, 23 };
                        List<int> enrouteInProgress = new List<int> { 1, 3, 11, 12, 13, 14, 15, 16, 17, 18 };
                        foreach (var deliveryRequest in group.DeliveryRequests.Where(t => enrouteInProgress.Contains(t.TrackScheduleEnrouteStatus) || enrouteInCompleted.Contains(t.TrackScheduleStatus)))
                        {
                            DeliverySchedule deliverySchedule = null;
                            DeliveryScheduleXTrackableSchedule trackableSchedule = null;
                            trackableSchedule = modifiedTrackableSchedules.FirstOrDefault(t => t.Id == deliveryRequest.TrackableScheduleId);
                            if (trackableSchedule != null)
                            {
                                deliverySchedule = trackableSchedule.DeliverySchedule;
                            }
                            else
                            {
                                ++createDeliveryGroupModel.LatestSchGroupNumber;
                            }
                            if (trackableSchedule != null)
                            {
                                createDeliveryGroupModel.GroupChanges.Add(new ScheduleNotificationModel() { ScheduleDate = trackableSchedule.Date, StartTime = trackableSchedule.StartTime, EndTime = trackableSchedule.EndTime, OrderId = trackableSchedule.OrderId ?? 0, GroupId = group.GroupId, ScheduleId = trackableSchedule.DeliveryScheduleId, ScheduleStatus = trackableSchedule.DeliveryScheduleStatusId, TrackableScheduleId = trackableSchedule.Id, DriverId = trackableSchedule.DriverId });
                            }
                        }
                    }

                }
            }
            response.StatusCode = Status.Success;
            return response;
        }
        private CreateDeliveryGroupModel GetCancelDeliveryGroupViewModel(List<TripViewModel> trips)
        {
            List<int> enrouteInCompleted = new List<int> { 3, 7, 8, 9, 22, 23 };
            List<int> enrouteInProgress = new List<int> { 1, 3, 11, 12, 13, 14, 15, 16, 17, 18 };
            CreateDeliveryGroupModel createDeliveryGroupModel = new CreateDeliveryGroupModel();
            createDeliveryGroupModel.PublishedGroups = trips.Where(t => t.DeliveryGroupStatus == DeliveryGroupStatus.Published || (t.GroupId > 0 && t.DeliveryGroupStatus == DeliveryGroupStatus.Deleted)).ToList();
            var modifiedDeliveryReqs = createDeliveryGroupModel.PublishedGroups.SelectMany(t => t.DeliveryRequests).Where(t1 =>
                                                                                       (enrouteInProgress.Contains(t1.TrackScheduleEnrouteStatus) || enrouteInCompleted.Contains(t1.TrackScheduleStatus)))
                                                                     .Select(t1 => new { OrderId = t1.OrderId, t1.TrackableScheduleId }).ToList();
            createDeliveryGroupModel.OrderIds = modifiedDeliveryReqs.Select(t => t.OrderId ?? 0).Distinct().ToList();
            createDeliveryGroupModel.TrackableScheduleIds = modifiedDeliveryReqs.Where(t => t.TrackableScheduleId != null).Select(t => t.TrackableScheduleId.Value).ToList();
            return createDeliveryGroupModel;
        }
        public async Task<List<CancelDeliverySchedule>> CancelDeliverySchedule(UserContext userContext, List<CancelDeliverySchedule> cancelDeliverySchedules)
        {
            List<CancelDeliverySchedule> response = new List<CancelDeliverySchedule>();

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    await IntializePostLoadCancelInfo(cancelDeliverySchedules);
                    var trackableScheduleIds = cancelDeliverySchedules.Select(x => x.TrackableScheduleId).ToList();

                    var deliveryScheduleXTrackableSchedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t1 => trackableScheduleIds.Contains(t1.Id)
                                                                      && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Completed
                                                                      && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.CompletedLate
                                                                      && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                                      && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                      && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledLate
                                                                      && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.UnplannedDropCompleted
                                                                      && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled
                    ).ToListAsync();
                    if (deliveryScheduleXTrackableSchedules.Any())
                    {
                        trackableScheduleIds = deliveryScheduleXTrackableSchedules.Select(x1 => x1.Id).ToList();
                        cancelDeliverySchedules = cancelDeliverySchedules.Where(x => trackableScheduleIds.Contains(x.TrackableScheduleId)).ToList();
                        if (cancelDeliverySchedules.Any())
                        {
                            var groupParentDrId = new List<string>();
                            foreach (var item in deliveryScheduleXTrackableSchedules)
                            {
                                item.DeliveryScheduleStatusId = (int)DeliveryScheduleStatus.Canceled;
                                groupParentDrId.Add(item.GroupParentDRId);
                            }
                            groupParentDrId = groupParentDrId.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
                            await Context.CommitAsync();
                            var delReqStatusUpdate = new List<DeliveryReqCancelScheduleUpdateModel>();

                            if (cancelDeliverySchedules.Any())
                            {
                                foreach (var item in cancelDeliverySchedules)
                                {
                                    delReqStatusUpdate.Add(new DeliveryReqCancelScheduleUpdateModel() { DriverColIndex = item.DriverColIndex, DriverId = item.DriverId, DriverRowIndex = item.DriverRowIndex, ScheduleBuilderId = item.ScheduleBuilderId, ShiftIndex = item.ShiftIndex, TrackableScheduleId = item.TrackableScheduleId, DeliveryReqId = item.DeliveryReqId, ScheduleStatusId = (int)DeliveryScheduleStatus.Canceled, UserId = userContext.Id, ShiftId = item.ShiftId });
                                }
                                response = await UpdateDeliveryRequestStatus(delReqStatusUpdate);
                                if (response.Any())
                                {
                                    var domain = ContextFactory.Current.GetDomain<ConsolidatedInvoiceDomain>();
                                    groupParentDrId.ForEach(t =>
                                    {
                                        domain.AddQueueMessageForDrCompletion(userContext, t);
                                    });
                                    transaction.Commit();
                                    List<ScheduleNotificationModel> groupChanges = new List<ScheduleNotificationModel>();
                                    response.ForEach(x =>
                                    {
                                        groupChanges.Add(new ScheduleNotificationModel { DriverId = x.DriverId, ScheduleStatus = (int)DeliveryScheduleStatus.Canceled });

                                    });
                                    await new PushNotificationDomain(this).PushSbChangesNotificationToDriver(groupChanges, userContext.UserName);
                                    //Save Retain Information.
                                    await SaveRetainInfomation(deliveryScheduleXTrackableSchedules);
                                }
                                else
                                {
                                    transaction.Rollback();
                                }
                            }
                            else
                            {
                                transaction.Commit();
                            }
                        }
                        else
                        {
                            transaction.Commit();
                        }
                    }
                    else
                    {
                        transaction.Commit();
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("ScheduleBuilderDomain", "CancelDeliverySchedule", ex.Message, ex);
                }
            }

            return response;
        }

        private async Task IntializePostLoadCancelInfo(List<CancelDeliverySchedule> cancelDeliverySchedules)
        {
            var postLoadtrackableScheduleIds = cancelDeliverySchedules.Where(x => x.TrackableScheduleId == -1).Select(x => x.DeliveryReqId).ToList();
            if (postLoadtrackableScheduleIds.Any())
            {
                var postdeliveryScheduleXTrackableSchedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t1 => postLoadtrackableScheduleIds.Contains(t1.FrDeliveryRequestId)
                                                                  && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Completed
                                                                  && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.CompletedLate
                                                                  && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                                  && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                  && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledLate
                                                                  && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.UnplannedDropCompleted
                                                                  && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled
                ).ToListAsync();
                if (postdeliveryScheduleXTrackableSchedules.Any())
                {
                    foreach (var item in postdeliveryScheduleXTrackableSchedules)
                    {
                        var cancelDeliverySchedulesInfo = cancelDeliverySchedules.Where(x => x.DeliveryReqId == item.FrDeliveryRequestId).FirstOrDefault();
                        if (cancelDeliverySchedulesInfo != null)
                        {
                            cancelDeliverySchedulesInfo.TrackableScheduleId = item.Id;
                            cancelDeliverySchedulesInfo.DriverId = item.DriverId.Value;
                            if (!string.IsNullOrEmpty(item.AdditionalInfo))
                            {
                                ScheduleAdditionalInfo additionalInfo = new ScheduleAdditionalInfo();
                                additionalInfo = JsonConvert.DeserializeObject<ScheduleAdditionalInfo>(item.AdditionalInfo);
                                if (additionalInfo != null)
                                {
                                    cancelDeliverySchedulesInfo.ShiftIndex = additionalInfo.ShiftIndex;
                                }
                            }
                        }
                    }
                }
            }
        }

        public async Task<List<CancelDeliverySchedule>> UpdateDeliveryRequestStatus(List<DeliveryReqCancelScheduleUpdateModel> statusModels)
        {
            List<CancelDeliverySchedule> response = new List<CancelDeliverySchedule>();
            if (statusModels != null && statusModels.Any())
            {
                SetScheduleStatus(statusModels);
                response = await ApiPostCall<List<CancelDeliverySchedule>>(ApplicationConstants.UrlUpdateDeliveryRequestCancelStatus, statusModels);
            }
            return response;
        }
        private void SetScheduleStatus(List<DeliveryReqCancelScheduleUpdateModel> statusModels)
        {
            foreach (var tr in statusModels)
            {
                if (tr.ScheduleStatusId > 0)
                {
                    tr.ScheduleStatusName = ((TrackableDeliveryScheduleStatus)tr.ScheduleStatusId).GetDisplayName();
                }
                else if (tr.ScheduleEnrouteStatusId > 0)
                {
                    tr.ScheduleStatusName = ((EnrouteDeliveryStatus)tr.ScheduleEnrouteStatusId).GetDisplayName();
                }
            }
        }

        public async Task<List<CancelDSDeliveryScheduleViewModel>> GetSubDRInfoCancelDS(CancelDSDeliveryScheduleInfo inputModel)
        {
            List<CancelDSDeliveryScheduleViewModel> response = new List<CancelDSDeliveryScheduleViewModel>();
            CancelDSDeliveryScheduleInfo cancelDSDeliverySchedule = new CancelDSDeliveryScheduleInfo();
            try
            {
                inputModel.CancelDSDeliverySchedules = inputModel.CancelDSDeliverySchedules.Where(x1 => !string.IsNullOrEmpty(x1.DeliveryReqId)).ToList();
                var deliveryScheduleIds = inputModel.CancelDSDeliverySchedules.Where(x => x.IsSubDR == false).Select(x => x.DeliveryReqId).ToList();
                var deliveyScheduleInfo = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t1 => deliveryScheduleIds.Contains(t1.FrDeliveryRequestId)
                                                                  && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Completed
                                                                  && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.CompletedLate
                                                                  && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                                  && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                  && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledLate
                                                                  && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.UnplannedDropCompleted
                                                                  && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled
                ).Select(x1 => new { x1.FrDeliveryRequestId, x1.PostLoadedForId }).ToListAsync();
                if (deliveyScheduleInfo.Any())
                {
                    var postLoadDRsInfo = deliveyScheduleInfo.Where(x => x.PostLoadedForId != null).Select(x1 => x1.PostLoadedForId).ToList();
                    await FindPostLoadCancelDS(cancelDSDeliverySchedule, postLoadDRsInfo);
                    deliveyScheduleInfo.ForEach(x =>
                    {
                        if (cancelDSDeliverySchedule.CancelDSDeliverySchedules.FindIndex(x1 => x1.DeliveryReqId == x.FrDeliveryRequestId) == -1)
                        {
                            cancelDSDeliverySchedule.CancelDSDeliverySchedules.Add(new CancelDSDeliverySchedule { IsSubDR = false, DeliveryReqId = x.FrDeliveryRequestId });
                        }
                    });

                }
                await FindSubDRsDS(inputModel, cancelDSDeliverySchedule);

                cancelDSDeliverySchedule.TfxCompanyId = inputModel.TfxCompanyId;
                cancelDSDeliverySchedule.RegionId = inputModel.RegionId;
                FindPreLoaddCancelDS(inputModel, cancelDSDeliverySchedule);
                response = await ApiPostCall<List<CancelDSDeliveryScheduleViewModel>>(ApplicationConstants.UrlGetDeliveryRequestCancelDRs, cancelDSDeliverySchedule);
                if (response.Any())
                {
                    response.ForEach(x1 =>
                    {
                        var preLoadDSInfo = inputModel.CancelDSDeliverySchedules.FirstOrDefault(x => x.IsPreLoadDR == true && x.DeliveryReqId == x1.DeliveryReqId);
                        if (preLoadDSInfo != null)
                        {
                            preLoadDSInfo.IsPreLoadDR = true;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetSubDRInfoCancelDS", ex.Message, ex);
            }

            return response;
        }

        private static void FindPreLoaddCancelDS(CancelDSDeliveryScheduleInfo inputModel, CancelDSDeliveryScheduleInfo cancelDSDeliverySchedule)
        {
            var preLoaddeliverySchedule = inputModel.CancelDSDeliverySchedules.Where(x => x.IsSubDR == false && x.IsPreLoadDR == true).Select(x => x.DeliveryReqId).ToList();
            if (preLoaddeliverySchedule.Any())
            {
                preLoaddeliverySchedule.ForEach(x1 =>
                {
                    if (cancelDSDeliverySchedule.CancelDSDeliverySchedules.FindIndex(x => x.DeliveryReqId == x1) == -1)
                    {
                        cancelDSDeliverySchedule.CancelDSDeliverySchedules.Add(new CancelDSDeliverySchedule { IsSubDR = false, DeliveryReqId = x1 });
                    }

                });

            }
        }

        private async Task FindSubDRsDS(CancelDSDeliveryScheduleInfo inputModel, CancelDSDeliveryScheduleInfo cancelDSDeliverySchedule)
        {
            var deliveryScheduleSubDRs = inputModel.CancelDSDeliverySchedules.Where(x => x.IsSubDR == true).Select(x => x.DeliveryReqId).ToList();
            var childSubDRsInfo = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t1 => deliveryScheduleSubDRs.Contains(t1.GroupParentDRId)
                                                             && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Completed
                                                             && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.CompletedLate
                                                             && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                             && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                             && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledLate
                                                             && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.UnplannedDropCompleted
                                                             && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled
                                                             ).Select(x1 => x1.FrDeliveryRequestId).ToListAsync();
            if (childSubDRsInfo.Any())
            {
                childSubDRsInfo.ForEach(x =>
                {
                    if (cancelDSDeliverySchedule.CancelDSDeliverySchedules.FindIndex(x1 => x1.DeliveryReqId == x) == -1)
                    {
                        cancelDSDeliverySchedule.CancelDSDeliverySchedules.Add(new CancelDSDeliverySchedule { IsSubDR = true, DeliveryReqId = x });
                    }
                });

            }
        }

        private async Task FindPostLoadCancelDS(CancelDSDeliveryScheduleInfo cancelDSDeliverySchedule, List<int?> postLoadDRsInfo)
        {
            if (postLoadDRsInfo.Any())
            {
                var postLoadSchedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t1 => postLoadDRsInfo.Contains(t1.Id)
                                                              && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Completed
                                                              && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.CompletedLate
                                                              && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                              && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                              && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledLate
                                                              && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.UnplannedDropCompleted
                                                              && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled
                            ).Select(x1 => new { x1.FrDeliveryRequestId }).ToListAsync();
                if (postLoadSchedules.Any())
                {
                    foreach (var item in postLoadSchedules)
                    {
                        if (cancelDSDeliverySchedule.CancelDSDeliverySchedules.FindIndex(x1 => x1.DeliveryReqId == item.FrDeliveryRequestId) == -1)
                        {
                            cancelDSDeliverySchedule.CancelDSDeliverySchedules.Add(new CancelDSDeliverySchedule { IsSubDR = false, DeliveryReqId = item.FrDeliveryRequestId });
                        }
                    }
                }
            }
        }

        // below method -  will return all Sub Drs Trackable status with parent Id
        public async Task<List<SubDRStatus>> getSubDrsStatusByParentId(List<string> groupParentDrIds)
        {
            var response = new List<SubDRStatus>();
            try
            {
                if (groupParentDrIds.Any())
                {
                    var uniqParentDrIds = groupParentDrIds.Distinct().ToList();
                    response = await Context.DataContext.DeliveryScheduleXTrackableSchedules
                                       .Where(t => uniqParentDrIds.Contains(t.GroupParentDRId))
                                       .Select(t => new SubDRStatus() { GroupParentDRId = t.GroupParentDRId, DeliveryScheduleStatusId = t.DeliveryScheduleStatusId }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "checkSubDrsStatusByParentId", ex.Message, ex);
            }
            return response;
        }

        private async Task DeleteDSGroup(int groupId, List<ScheduleNotificationModel> groupChanges, UserContext user, CreateDeliveryGroupModel groupModel = null)
        {
            var group = Context.DataContext.DeliveryGroups.FirstOrDefault(t => t.Id == groupId);
            if (group != null)
            {
                group.IsActive = false;
                group.UpdatedBy = user.Id;
                group.UpdatedDate = DateTimeOffset.Now;
                var trackableSchedules = group.DeliveryScheduleXTrackableSchedules.Where(t1 => t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Completed
                                                                      && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.CompletedLate
                                                                      && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                                      && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                      && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledLate
                                                                      && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled
                                                                      && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.UnplannedDropCompleted).ToList();
                trackableSchedules.ForEach(t => { t.DeliveryGroupId = null; t.DeliveryScheduleStatusId = (int)TrackableDeliveryScheduleStatus.Canceled; t.FrDeliveryRequestId = null; });
                var deliverySchedules = group.DeliverySchedules.ToList();
                if (groupModel == null)
                {
                    groupModel = new CreateDeliveryGroupModel() { OrderIds = new List<int>() };
                }
                trackableSchedules.ForEach(t => groupModel.GroupChanges.Add(new ScheduleNotificationModel() { TrackableScheduleId = t.Id, ScheduleId = t.DeliveryScheduleId, OrderId = t.OrderId ?? 0, ScheduleStatus = (int)DeliveryScheduleStatus.Canceled, PreviousDriverId = t.DriverId }));
                trackableSchedules.ForEach(t => groupModel.OrderIds.Add(t.OrderId ?? 0));
                foreach (var ds in deliverySchedules)
                {
                    deliverySchedules.ForEach(t => { t.DeliveryGroupId = null; t.StatusId = (int)DeliveryScheduleStatus.Canceled; });
                }
                await Context.CommitAsync();
                CreateOrdersNewVersion(groupModel, user);
                bool isTodayScheduleExists = false;
                foreach (var tr in trackableSchedules)
                {
                    DateTimeOffset jobTime = DateTimeOffset.Now;
                    int timeDifference = 12;
                    if (!tr.OrderId.HasValue || tr.OrderId == 0)
                    {
                        timeDifference = 24;
                    }
                    else
                    {
                        jobTime = jobTime.ToTargetDateTimeOffset(tr.Order.FuelRequest.Job.TimeZoneName);
                    }
                    double trackScheduleStartTimeDiff = Math.Abs((tr.Date.Date.Add(tr.StartTime) - jobTime.DateTime).TotalHours);
                    double trackScheduleEndTimeDiff = Math.Abs((tr.Date.Date.Add(tr.EndTime) - jobTime.DateTime).TotalHours);

                    if (tr.Date.Date == jobTime.Date || trackScheduleStartTimeDiff <= timeDifference || trackScheduleEndTimeDiff <= timeDifference)
                    {
                        isTodayScheduleExists = true;
                        break;
                    }
                }
                if (isTodayScheduleExists)
                {
                    trackableSchedules.ForEach(t => groupChanges.Add(new ScheduleNotificationModel() { TrackableScheduleId = t.Id, ScheduleId = t.DeliveryScheduleId, OrderId = t.OrderId ?? 0, ScheduleStatus = (int)DeliveryScheduleStatus.Canceled, PreviousDriverId = t.DriverId }));
                }
            }
        }
        #endregion

        public async Task<StatusViewModel> CancelDriverScheduleAfterBDNConfirmation(UserContext userContext, int orderId, int InvoiceId, int invoiceHeaderId)
        {

            StatusViewModel response = new StatusViewModel(Status.Success);
            try
            {
                var invoices = await Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId
                                                                && (t.WaitingFor == (int)WaitingAction.BDNConfirmation
                                                                        || t.WaitingFor == (int)WaitingAction.InvoiceConfirmation
                                                                        || t.WaitingFor == (int)WaitingAction.AllDRCompletion) && t.IsActive
                                                                && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).ToListAsync();
                var orderIdList = new List<int?>();
                invoices.ForEach(f => orderIdList.Add(f.OrderId));
                var currentDate = DateTimeOffset.Now;
                var trackableScheduleList = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t1 => orderIdList.Contains(t1.OrderId) && t1.Date >= currentDate.Date
                                                                    && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Completed
                                                                    && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.CompletedLate
                                                                    && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                                    && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                    && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledLate
                                                                    && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.UnplannedDropCompleted
                                                                    && t1.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled).ToListAsync();
                if (invoices.Any())
                {
                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            bool isWaitingForBDN = false;

                            if (trackableScheduleList.Any())
                            {
                                trackableScheduleList.ForEach(t => { t.DeliveryScheduleStatusId = (int)DeliveryScheduleStatus.Canceled; t.Date = currentDate.Date; });
                                // Context.DataContext.Entry(trackableScheduleList).State = EntityState.Modified;
                                List<DeliveryReqStatusUpdateModel> statusModels = new List<DeliveryReqStatusUpdateModel>();
                                trackableScheduleList.ForEach(t => statusModels.Add(new DeliveryReqStatusUpdateModel() { TrackableScheduleId = t.Id, ScheduleStatusId = (int)DeliveryScheduleStatus.Canceled, UserId = userContext.Id }));
                                SetScheduleStatus(statusModels);
                                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlUpdateDeliveryRequestStatusByTrackableScheduleId, statusModels);
                                if (response.StatusCode == Status.Failed)
                                {
                                    transaction.Rollback();
                                    LogManager.Logger.WriteException("ScheduleBuilderDomain", "CancelDriverScheduleAfterBDNConfirmation", response.StatusMessage, null);
                                }
                            }
                            //var invoices = Context.DataContext.Invoices.Where(w => w.InvoiceHeaderId == invoiceHeaderId).ToList();
                            if (invoices != null && invoices.Any())
                            {
                                if (invoices.Any(t => t.WaitingFor == (int)WaitingAction.BDNConfirmation))
                                    isWaitingForBDN = true;

                                if (invoices.Any(t => t.WaitingFor != (int)WaitingAction.InvoiceConfirmation))
                                    invoices.ForEach(t => { t.WaitingFor = (int)WaitingAction.InvoiceConfirmation; t.UpdatedBy = userContext.Id; t.UpdatedDate = currentDate; });

                            }

                            //BROKER case 
                            var brokeredInvoicesChainIds = invoices.Where(t => t.BrokeredChainId != null).Select(t => t.BrokeredChainId).ToList();
                            if (brokeredInvoicesChainIds.Any())
                            {
                                var brokeredInvoices = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId != invoiceHeaderId
                                                                && brokeredInvoicesChainIds.Contains(t.BrokeredChainId)
                                                                && (t.WaitingFor == (int)WaitingAction.BDNConfirmation || t.WaitingFor == (int)WaitingAction.InvoiceConfirmation) && t.IsActive
                                                                && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                        .ToList();
                                if (brokeredInvoices.Any())
                                {
                                    if (brokeredInvoices.Any(t => t.WaitingFor != (int)WaitingAction.InvoiceConfirmation))
                                        brokeredInvoices.ForEach(t => { t.WaitingFor = (int)WaitingAction.InvoiceConfirmation; t.UpdatedDate = currentDate; });
                                }
                            }

                            await Context.CommitAsync();
                            transaction.Commit();
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageSuccess;
                            if (trackableScheduleList.Any())
                            {
                                //send notification
                                List<ScheduleNotificationModel> groupChanges = new List<ScheduleNotificationModel>();
                                trackableScheduleList.Where(w => w.Date.Day == currentDate.Date.Day && w.Date.Month == currentDate.Date.Month && w.Date.Year == currentDate.Date.Year).Select(s => s.DriverId).Distinct().ToList().ForEach(x =>
                                          {
                                              groupChanges.Add(new ScheduleNotificationModel { DriverId = x, ScheduleStatus = (int)DeliveryScheduleStatus.Canceled });

                                          });

                                if (isWaitingForBDN)
                                    await Task.Run(() => new PushNotificationDomain(this).PushSbChangesNotificationToDriver(groupChanges, userContext.UserName));
                            }
                            try
                            {
                                if (isWaitingForBDN)
                                {
                                    var notificationDomain = new NotificationDomain(this);
                                    await notificationDomain.AddNotificationEventAsync(EventType.DeliveryClosedSendBDN, invoiceHeaderId, userContext.Id);
                                }
                            }
                            catch (Exception ex)
                            {
                                response.StatusCode = Status.Failed;
                                LogManager.Logger.WriteException("ScheduleBuilderDomain", "DriverScheduleAfterBDNConfirmation", ex.Message, ex);
                            }
                        }
                        catch (Exception ex)
                        {
                            response.StatusCode = Status.Failed;
                            transaction.Rollback();
                            LogManager.Logger.WriteException("ScheduleBuilderDomain", "CancelDriverScheduleAfterBDNConfirmation", ex.Message, ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "CancelDriverScheduleAfterBDNConfirmation", ex.Message, ex);
            }
            return response;
        }

        public async Task UpdateBulkPlantAddressForCarribean(DSBSaveModel scheduleBuilder)
        {
            if (scheduleBuilder != null)
            {
                // Fetching here to avoid DB Calls in for loop.
                var country = Context.DataContext.MstCountries.First(t => t.Id == (int)Country.CAR).ToViewModel();
                var states = await Context.DataContext.MstStates.Where(t => t.CountryId == (int)Country.CAR)
                             .Select(t => new { t.Id, t.Code, t.Name }).ToListAsync();
                foreach (var tripItem in scheduleBuilder.Trips)
                {

                    if (tripItem.BulkPlant != null && (tripItem.BulkPlant.Country.Id == (int)Country.CAR || tripItem.BulkPlant.Country.Code == Country.CAR.ToString())
                                && tripItem.BulkPlant.IsMissingAddress())
                    {

                        var state = states.FirstOrDefault(t => t.Id == tripItem.BulkPlant.State.Id);
                        tripItem.BulkPlant.Address = string.IsNullOrWhiteSpace(tripItem.BulkPlant.Address) ? (state.Name ?? Resource.lblCaribbean) : tripItem.BulkPlant.Address;
                        tripItem.BulkPlant.City = string.IsNullOrWhiteSpace(tripItem.BulkPlant.City) ? (state.Name ?? Resource.lblCaribbean) : tripItem.BulkPlant.City;
                        tripItem.BulkPlant.ZipCode = string.IsNullOrWhiteSpace(tripItem.BulkPlant.ZipCode) ? (state.Name ?? Resource.lblCaribbean) : tripItem.BulkPlant.ZipCode;
                        tripItem.BulkPlant.CountyName = string.IsNullOrWhiteSpace(tripItem.BulkPlant.CountyName) ? (state.Name ?? Resource.lblCaribbean) : tripItem.BulkPlant.CountyName;

                        if (tripItem.BulkPlant.Latitude == 0 || tripItem.BulkPlant.Longitude == 0)
                        {
                            var point = GoogleApiDomain.GetGeocode($"{tripItem.BulkPlant.Address} {tripItem.BulkPlant.City} {state.Code} {country.Code} {tripItem.BulkPlant.ZipCode}");
                            if (point != null)
                            {
                                tripItem.BulkPlant.Latitude = Convert.ToDecimal(point.Latitude);
                                tripItem.BulkPlant.Longitude = Convert.ToDecimal(point.Longitude);
                            }
                        }
                    }
                    if (tripItem.DeliveryRequests.Any())
                    {
                        foreach (var deliveryReqitem in tripItem.DeliveryRequests)
                        {
                            if (deliveryReqitem.BulkPlant != null && (deliveryReqitem.BulkPlant.Country.Id == (int)Country.CAR || deliveryReqitem.BulkPlant.Country.Code == Country.CAR.ToString())
                                && deliveryReqitem.BulkPlant.IsMissingAddress())
                            {
                                var state = states.FirstOrDefault(t => t.Id == deliveryReqitem.BulkPlant.State.Id);
                                deliveryReqitem.BulkPlant.Address = string.IsNullOrWhiteSpace(deliveryReqitem.BulkPlant.Address) ? (state.Name ?? Resource.lblCaribbean) : deliveryReqitem.BulkPlant.Address;
                                deliveryReqitem.BulkPlant.City = string.IsNullOrWhiteSpace(deliveryReqitem.BulkPlant.City) ? (state.Name ?? Resource.lblCaribbean) : deliveryReqitem.BulkPlant.City;
                                deliveryReqitem.BulkPlant.ZipCode = string.IsNullOrWhiteSpace(deliveryReqitem.BulkPlant.ZipCode) ? (state.Name ?? Resource.lblCaribbean) : deliveryReqitem.BulkPlant.ZipCode;
                                deliveryReqitem.BulkPlant.CountyName = string.IsNullOrWhiteSpace(deliveryReqitem.BulkPlant.CountyName) ? (state.Name ?? Resource.lblCaribbean) : deliveryReqitem.BulkPlant.CountyName;

                                if (deliveryReqitem.BulkPlant.Latitude == 0 || deliveryReqitem.BulkPlant.Longitude == 0)
                                {
                                    var point = GoogleApiDomain.GetGeocode($"{deliveryReqitem.BulkPlant.Address} {deliveryReqitem.BulkPlant.City} {state.Code} {country.Code} {deliveryReqitem.BulkPlant.ZipCode}");
                                    if (point != null)
                                    {
                                        deliveryReqitem.BulkPlant.Latitude = Convert.ToDecimal(point.Latitude);
                                        deliveryReqitem.BulkPlant.Longitude = Convert.ToDecimal(point.Longitude);
                                    }
                                }
                            }

                        }
                    }

                }
            }
        }
        public async Task<StatusViewModel> DeleteDeliveryRequestOnOrderClose(List<int> OrderIds, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    Context.DataContext.Database.CommandTimeout = 180;//3 minutes timeout
                    var spDomain = new StoredProcedureDomain(this);
                    List<int> childOrderIds = new List<int>();
                    foreach (var item in OrderIds)
                    {
                        var childOrders = await spDomain.GetBrokeredChildOrders(item);
                        if (childOrders.Any())
                        {
                            childOrders.ForEach(x => childOrderIds.Add(x.OrderId));
                        }
                    }
                    if (childOrderIds.Any())
                    {
                        //foreach (var item in childOrderIds)
                        //{
                        //    await ContextFactory.Current.GetDomain<OrderDomain>().CloseOrderAsync(userContext, item, userContext.Id);
                        //}
                        OrderIds.AddRange(childOrderIds);
                    }
                    var deliveryScheduleXTrackableSchedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t1 => OrderIds.Contains(t1.OrderId.Value) && t1.OrderId != null
                                                                      && (t1.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Accepted
                                                                      || t1.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Modified)
                    ).ToListAsync();
                    if (deliveryScheduleXTrackableSchedules.Any())
                    {
                        deliveryScheduleXTrackableSchedules.ForEach(x => x.IsActive = false);
                        await Context.CommitAsync();
                        var deliveryRequestOrderCloseStatus = await DeleteFreightDeliveryRequestOnOrderClose(OrderIds);
                        if (deliveryRequestOrderCloseStatus.StatusCode == (int)Status.Success)
                        {
                            response.StatusCode = (int)Status.Success;
                            transaction.Commit();
                            List<ScheduleNotificationModel> groupChanges = new List<ScheduleNotificationModel>();
                            var driverDetails = deliveryScheduleXTrackableSchedules.Select(x => x.DriverId).Distinct().ToList();
                            driverDetails.ForEach(x =>
                            {
                                groupChanges.Add(new ScheduleNotificationModel { DriverId = x, ScheduleStatus = (int)DeliveryScheduleStatus.Canceled });

                            });
                            if (groupChanges.Any())
                            {
                                await new PushNotificationDomain(this).PushSbChangesNotificationToDriver(groupChanges, userContext.UserName);
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.waringErrorDSOrderClose;
                            transaction.Rollback();
                        }
                    }
                    else
                    {
                        transaction.Commit();
                        var deliveryRequestOrderCloseStatus = await DeleteFreightDeliveryRequestOnOrderClose(OrderIds);
                        if (deliveryRequestOrderCloseStatus.StatusCode == (int)Status.Success)
                        {
                            response.StatusCode = (int)Status.Success;
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.waringErrorDSOrderClose;

                        }
                    }

                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = ex.Message;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("ScheduleBuilderDomain", "DeleteDeliveryRequestOnOrderClose", ex.Message, ex);
                }
            }

            return response;
        }
        public async Task<StatusViewModel> DeleteFreightDeliveryRequestOnOrderClose(List<int> OrderIds)
        {
            StatusViewModel response = new StatusViewModel();
            if (OrderIds != null && OrderIds.Any())
            {
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlDeleteDeliveryRequestOnOrderClose, OrderIds);
            }
            return response;
        }
        public async Task<List<DeliveryRequestBlendGroupDetails>> GetBlendedSchedules(string blendGroupId)
        {
            List<DeliveryRequestBlendGroupDetails> response = new List<DeliveryRequestBlendGroupDetails>();
            try
            {
                if (!string.IsNullOrEmpty(blendGroupId))
                    response = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.BlendGroupId == blendGroupId).Where(Extensions.IsOpenMissedTrackableSchedule())
                        .Select(t => new DeliveryRequestBlendGroupDetails { Id = t.Id, OrderId = t.OrderId, DeliveryLevelPO = t.DeliveryLevelPO }).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetBlendedSchedules", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<RecurringSchedulesDetails>> RemoveCloseOrderSchedules(List<RecurringSchedulesDetails> recurringSchedulesDetails, List<int> OrderIds)
        {
            var orderCloseDetails = await Context.DataContext.Orders
                        .Where(t => (OrderIds.Contains(t.Id)
                                    && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)OrderStatus.Open)).Select(x => x.Id).ToListAsync();
            if (orderCloseDetails.Any())
            {
                var scheduleCancelList = recurringSchedulesDetails.Where(x => orderCloseDetails.Contains(x.OrderId)).ToList();
                if (scheduleCancelList.Any())
                {
                    UserContext userContext = new UserContext();
                    foreach (var item in scheduleCancelList)
                    {
                        userContext.Id = item.TfxUserId;
                        var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().DeleteRecurringSchedule(item.Id, userContext.Id);
                    }
                }
                return recurringSchedulesDetails.Where(x => !orderCloseDetails.Contains(x.OrderId)).ToList();
            }
            return recurringSchedulesDetails;
        }
        public async Task<StatusViewModel> DeleteDeliveryRequests(List<string> delReqIds)
        {
            StatusViewModel response = new StatusViewModel();
            if (delReqIds != null && delReqIds.Any())
            {
                try
                {
                    response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlDeleteDeliveryRequests, delReqIds);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ScheduleBuilderDomain", "DeleteDeliveryRequests", ex.Message, ex);
                }
                return response;
            }
            return response;
        }
        public async Task<ResetDeliveryGroupScheduleModel> RemoveDeliverySchedule(ResetDeliveryGroupScheduleModel model)
        {
            var response = new ResetDeliveryGroupScheduleModel();
            try
            {
                response = await ApiPostCall<ResetDeliveryGroupScheduleModel>(ApplicationConstants.UrlRemoveDeliverySchedule, model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "RemoveDeliverySchedule", ex.InnerException + " DrIds -" + string.Join(",", model.DeliveryRequestIds + string.Join(",", model.DeliveryRequestIds + " DSB ID -" + model.ScheduleBuilderId)), null);
            }
            return response;
        }

        public bool IsCompletedSchedule(DeliveryScheduleXTrackableSchedule schedule)
        {
            return schedule.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Completed
                                                                         || schedule.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.CompletedLate
                                                                         || schedule.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                                         || schedule.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                         || schedule.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.RescheduledLate
                                                                         || schedule.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.UnplannedDropCompleted;

        }

        private void SetCustomerBrandAndLoadDRAttributes(USPCustomerLoadQueueDetails customerDetails, List<DeliveryRequestViewModel> response, int companyId)
        {
            //Set Customer Brand Details.
            if (customerDetails.customerBrandDetails.Any() && customerDetails.jobDetails.Any())
            {
                foreach (var dr in response)
                {
                    var BuyerCompId = customerDetails.jobDetails.Where(t => t.JobId == dr.JobId).Select(t => t.CompanyId).FirstOrDefault();
                    var customerBrandId = customerDetails.customerBrandDetails.Where(t => t.BuyerCompanyId == BuyerCompId && t.SupplierCompanyId == companyId).Select(t => t.CustomerId).FirstOrDefault();
                    dr.CustomerBrandId = customerBrandId;


                }
            }
            //Set Customer Load Queue Attributes.
            if (customerDetails.customerLoadQueueAttributes != null)
            {
                bool isDRQueueSettingExists = false;
                bool isLoadQueueSetttingExists = false;
                var drQueueAttributes = new DRQueueAttributesViewModel();
                var loadQueueAttributes = new LoadQueueAttributesViewModel();
                if (!string.IsNullOrWhiteSpace(customerDetails.customerLoadQueueAttributes.LoadQueueAttributes))
                {
                    var loadQueuesetting = JsonConvert.DeserializeObject<LoadQueueAttributesViewModel>(customerDetails.customerLoadQueueAttributes.LoadQueueAttributes);
                    loadQueueAttributes.CustomerName = loadQueuesetting.CustomerName;
                    loadQueueAttributes.Driver = loadQueuesetting.Driver;
                    loadQueueAttributes.LocationName = loadQueuesetting.LocationName;
                    loadQueueAttributes.TrailerName = loadQueuesetting.TrailerName;
                    isLoadQueueSetttingExists = true;
                }
                if (!string.IsNullOrWhiteSpace(customerDetails.customerLoadQueueAttributes.DRQueueAttributes))
                {
                    var drQueuesetting = JsonConvert.DeserializeObject<DRQueueAttributesViewModel>(customerDetails.customerLoadQueueAttributes.DRQueueAttributes);
                    drQueueAttributes.CustomerName = drQueuesetting.CustomerName;
                    drQueueAttributes.DeliveryShift = drQueuesetting.DeliveryShift;
                    drQueueAttributes.HoursToCoverDistance = drQueuesetting.HoursToCoverDistance;
                    drQueueAttributes.TrailerCompatibility = drQueuesetting.TrailerCompatibility;
                    isDRQueueSettingExists = true;
                }
                if (!isDRQueueSettingExists)
                {
                    drQueueAttributes.CustomerName = true;
                    drQueueAttributes.DeliveryShift = true;
                    drQueueAttributes.HoursToCoverDistance = true;
                    drQueueAttributes.TrailerCompatibility = true;
                }
                if (!isLoadQueueSetttingExists)
                {
                    loadQueueAttributes.CustomerName = true;
                    loadQueueAttributes.Driver = true;
                    loadQueueAttributes.LocationName = true;
                    loadQueueAttributes.TrailerName = true;
                }
                foreach (DeliveryRequestViewModel item in response)
                {
                    item.LoadQueueAttributes = loadQueueAttributes;
                    item.DRQueueAttributes = drQueueAttributes;
                }
            }
        }
    }
}