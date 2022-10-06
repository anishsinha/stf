using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Asset;
using SiteFuel.Exchange.ViewModels.Dispatcher;
using SiteFuel.Exchange.ViewModels.DispatchScheduler;
using SiteFuel.Exchange.ViewModels.ExternalEntityMappings;
using SiteFuel.Exchange.ViewModels.Forcasting;
using SiteFuel.Exchange.ViewModels.Job;
using SiteFuel.Exchange.ViewModels.OttoSchedule;
using SiteFuel.Exchange.ViewModels.RouteInfo;
using SiteFuel.Exchange.ViewModels.SalesUser;
using SiteFuel.Exchange.ViewModels.Tank;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SiteFuel.Exchange.Domain
{
    public class FreightServiceDomain : DeliveryReqJobInfoDomain
    {

        public FreightServiceDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public FreightServiceDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<bool> SaveAdditionalJobDetails(JobViewModel viewModel, int jobId)
        {
            bool response = false;
            try
            {
                var inputModel = GetAdditonalJobDetailsViewModel(viewModel, jobId);
                response = await ApiPostCall<bool>(ApplicationConstants.UrlSaveAdditionalJobDetails, inputModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "SaveAdditionalJobDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<RecurringDeliveryRequest>> GetJobRecurringSchedules(List<int> jobIds, List<int> productTypeIds = null)
        {
            List<RecurringDeliveryRequest> response = new List<RecurringDeliveryRequest>();
            try
            {
                var input = new { JobIds = jobIds, ProductTypeIds = productTypeIds };
                response = await ApiPostCall<List<RecurringDeliveryRequest>>(ApplicationConstants.UrlGetRecurringSchedulesForBuyer, input);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetJobRecurringSchedules", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> DeleteJobTanks(DeleteTanksModel deleteAssetModel)
        {
            bool response = false;
            try
            {
                response = await ApiPostCall<bool>(ApplicationConstants.UrlDeleteJobTanks, deleteAssetModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "DeleteJobTanks", ex.Message, ex);
            }
            return response;
        }

        public async Task<int> GetAssignedCarrierCompanyId(int supplierCompId, int jobId)
        {
            int response = 0;
            try
            {
                var carrierList = await GetAssignedCarriersForSupplier(supplierCompId);
                if (carrierList != null && carrierList.Any(t => t.Jobs.Any(j => j.Job.Id == jobId)))
                {
                    response = carrierList.Where(t => t.Jobs.Any(j => j.Job.Id == jobId)).Select(t => t.Carrier.Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetAssignedCarrierCompanyId", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DipTestSummaryViewModel>> GetDiptestSummaryAsync(int lastUpdated, UserContext userContext)
        {
            List<DipTestSummaryViewModel> response = new List<DipTestSummaryViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var jobIds = await GetRetailJobIdsForCompany(userContext);
                var strJobIds = string.Join(",", jobIds);
                response = await spDomain.GetDiptestSummaryAsync(lastUpdated, strJobIds);
                if (response != null && response.Any())
                {
                    Dictionary<int, string> jobOnsiteContact = new Dictionary<int, string>();
                    var jobs = await Context.DataContext.Jobs.Where(t => jobIds.Contains(t.Id)).ToListAsync();

                    foreach (var item in response)
                    {
                        item.Capacity = $"{item.Capacity} {item.CapacityUom.GetDisplayName()}";
                        if (item.DiptestMethod != DipTestMethod.Select)
                        {
                            item.LastInventory = $"{item.LastInventory} {(item.DipTestUom == TankScaleMeasurement.None ? item.CapacityUom.GetDisplayName() : item.DipTestUom.GetDisplayName())}";
                            item.DisplayDiptestMethod = item.DiptestMethod.GetDisplayName();
                        }
                        else
                        {
                            item.DisplayDiptestMethod = "NA";
                        }

                        if (jobOnsiteContact.ContainsKey(item.LocationId))
                        {
                            item.ContactPerson = jobOnsiteContact[item.LocationId];
                        }
                        else
                        { // todo : optimize code : else part must be top and list should be saved in jobOnsiteContact.
                            var job = jobs.FirstOrDefault(t => t.Id == item.LocationId);
                            var onsiteContacts = job.Users1.Where(t => t.IsActive)
                                                    .Select(t => new { t.FirstName, t.LastName, t.PhoneNumber, t.Email, t.IsActive })
                                                    .ToList();
                            if (onsiteContacts != null && onsiteContacts.Any())
                            {
                                var contactDetails = "";
                                foreach (var contact in onsiteContacts)
                                {
                                    contactDetails = $"{contact.FirstName} {contact.LastName}" + " - " + contact.PhoneNumber + " " + contact.Email;
                                    item.ContactPerson += contactDetails + ";";
                                }
                                jobOnsiteContact.Add(item.LocationId, contactDetails);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDiptestSummaryAsync", ex.Message, ex);
            }
            return response;
        }


        public async Task<List<DipTestSummaryViewModel>> GetLocationInventoryDiptestSummaryAsync(InventoryDataCaptureType inventoryCaptureType,
            UserContext userContext)
        {
            List<DipTestSummaryViewModel> response = new List<DipTestSummaryViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var jobIds = await GetRetailJobIdsForCompany(userContext);
                var strJobIds = string.Join(",", jobIds);
                response = await spDomain.GetLocationInventoryDiptestSummaryAsync(inventoryCaptureType, strJobIds);
                if (response != null && response.Any())
                {
                    Dictionary<int, string> jobOnsiteContact = new Dictionary<int, string>();
                    var jobs = await Context.DataContext.Jobs.Where(t => jobIds.Contains(t.Id)).ToListAsync();

                    foreach (var item in response)
                    {
                        item.Capacity = $"{item.Capacity} {item.CapacityUom.GetDisplayName()}";
                        if (item.DiptestMethod != DipTestMethod.Select)
                        {
                            item.LastInventory = $"{item.LastInventory} {(item.DipTestUom == TankScaleMeasurement.None ? item.CapacityUom.GetDisplayName() : item.DipTestUom.GetDisplayName())}";
                            item.DisplayDiptestMethod = item.DiptestMethod.GetDisplayName();
                        }
                        else
                        {
                            item.DisplayDiptestMethod = "NA";
                        }

                        if (jobOnsiteContact.ContainsKey(item.LocationId))
                        {
                            item.ContactPerson = jobOnsiteContact[item.LocationId];
                        }
                        else
                        { // todo : optimize code : else part must be top and list should be saved in jobOnsiteContact.
                            var job = jobs.FirstOrDefault(t => t.Id == item.LocationId);
                            var onsiteContacts = job.Users1.Where(t => t.IsActive)
                                                    .Select(t => new { t.FirstName, t.LastName, t.PhoneNumber, t.Email, t.IsActive })
                                                    .ToList();
                            if (onsiteContacts != null && onsiteContacts.Any())
                            {
                                var contactDetails = "";
                                foreach (var contact in onsiteContacts)
                                {
                                    contactDetails = $"{contact.FirstName} {contact.LastName}" + " - " + contact.PhoneNumber + " " + contact.Email;
                                    item.ContactPerson += contactDetails + ";";
                                }
                                jobOnsiteContact.Add(item.LocationId, contactDetails);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetLocationInventoryDiptestSummaryAsync", ex.Message, ex);
            }
            return response;
        }

        /// <summary>
        /// SaveDiptestPendingNotificationEventsAync : save into Notification table which will be called while c'Job ContineousEmailNotifications-> GetPendingNotificationEvents
        /// </summary>
        /// <param name="dipTestMethodInventoryDataCapture"></param>
        /// <returns></returns>
        public async Task<Status> SaveDiptestPendingNotificationEventsAync(InventoryDataCaptureType dipTestMethodInventoryDataCapture)
        {
            Status response = Status.Failed;
            try
            {

                var spResponse = await new StoredProcedureDomain(this).GetLastUpdatedDiptestCompanies((int)dipTestMethodInventoryDataCapture);

                if (spResponse != null && spResponse.Any())
                {
                    var jobIds = spResponse.Select(t => t.JobId).Distinct().ToList();
                    if (jobIds != null && jobIds.Any())
                    {
                        //observation : in 24 hour approx 60 time server call and also support multiple jobIds , avg max response time is 111 milisecond. 
                        List<DipTestRequestModel> carrierCompanies = await GetCarrierDetailsByJob(jobIds);
                        if (carrierCompanies != null && carrierCompanies.Any())
                        {
                            spResponse.AddRange(carrierCompanies);
                        }
                    }
                    var notificationDomain = new NotificationDomain(this);

                    var spResponseByCompanyId = spResponse.GroupBy(t => t.CompanyId).Select(t1 => t1.FirstOrDefault()).ToList();

                    //var tasks = new List<Task>();

                    // on an average spResponseByCompanyId will be hudge list.bcz it contains result of GetLastUpdatedDiptestCompanies and GetCarrierDetailsByJob return.
                    //so soltuion smooth bulk call in foreach loop refer https://stackoverflow.com/questions/46345840/adding-tasks-to-a-listtask-executes-them-making-task-whenall-redundant 
                    //in order to smooth foreach itteration we should not wait for AddNotificationEventAsync execution or return. 
                    foreach (var company in spResponseByCompanyId)
                    {
                        var jsonMessage = JsonConvert.SerializeObject(new DipTestProcessViewModel { CompanyTypeId = company.CompanyTypeId, DipTestMethodInventoryDataCapture = dipTestMethodInventoryDataCapture });
                        await notificationDomain.AddNotificationEventAsync(EventType.DipTestNotUpdated, company.CompanyId, (int)SystemUser.System, null, jsonMessage);
                        // tasks.Add(notificationDomain.AddNotificationEventAsync(EventType.DipTestNotUpdated, company.CompanyId, (int)SystemUser.System, null, jsonMessage));
                    }
                    // await Task.WhenAll(tasks); //Creates a task that will complete when all of the Task  objects in an enumerable collection have completed.
                }
                response = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "ProcessDiptestAlertsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> UpdateAdditionalJobDetails(JobViewModel viewModel)
        {
            bool response = false;
            try
            {
                JobAdditionalDetailsViewModel inputModel = GetAdditonalJobDetailsViewModel(viewModel, viewModel.Id);
                response = await ApiPostCall<bool>(ApplicationConstants.UrlUpdateAdditionalJobDetails, inputModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateAdditionalJobDetails", ex.Message, ex);
            }
            return response;
        }

        private static JobAdditionalDetailsViewModel GetAdditonalJobDetailsViewModel(JobViewModel viewModel, int JobId)
        {
            var response = new JobAdditionalDetailsViewModel();
            response.SiteImageFilePath = viewModel.SiteImage?.FilePath;
            response.AdditionalImageFilePath = viewModel.AdditionalImage?.SiteImage?.FilePath;
            response.AdditionalImageDescription = viewModel.AdditionalImage?.Description;
            foreach (var Delivery in viewModel.DeliveryDaysList)
            {
                DeliveryDaysViewModel objDeliveryDays = new DeliveryDaysViewModel();
                if (Delivery != null)
                {
                    if (viewModel.DeliveryDaysList != null)
                    {
                        objDeliveryDays.DeliveryDays = Delivery.DeliveryDays;
                    }

                    if (Delivery.FromDeliveryTime != null)
                    {
                        objDeliveryDays.FromDeliveryTime = Convert.ToDateTime(Delivery.FromDeliveryTime).ToString();
                    }

                    if (Delivery.ToDeliveryTime != null)
                    {
                        objDeliveryDays.ToDeliveryTime = Convert.ToDateTime(Delivery.ToDeliveryTime).ToString();
                    }

                    objDeliveryDays.IsAcceptNightDeliveries = Delivery.IsAcceptNightDeliveries;
                }
                response.DeliveryDaysList.Add(objDeliveryDays);
            }


            response.JobId = JobId;
            response.DistanceCovered = viewModel.DistanceCovered;
            response.SiteId = viewModel.JobID;
            response.JobName = viewModel.Name;
            response.IsActive = true;
            response.IsAutoCreateDREnable = viewModel.IsAutoCreateDREnable;
            response.TrailerType = viewModel.TrailerType;
            return response;
        }

        public async Task<JobAdditionalDetailsViewModel> GetAdditionalJobDetails(int JobId, int SupplierCompanyId = 0)
        {
            var response = new JobAdditionalDetailsViewModel();
            try
            {
                var url = string.Format(ApplicationConstants.UrlGetAdditionalJobDetails, JobId, SupplierCompanyId);
                response = await ApiGetCall<JobAdditionalDetailsViewModel>(url);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetAdditionalJobDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DeliveryRequestViewModel>> GetDeliveryRequests(int companyId, string regionId, string selectedDate = null)
        {

            var response = new List<DeliveryRequestViewModel>();
            try
            {
                var url = string.Format(ApplicationConstants.UrlGetDeliveryRequests, companyId, regionId, selectedDate);
                var allDrs = await ApiGetCall<List<DeliveryRequestViewModel>>(url);
                if (allDrs != null)
                {
                    response = await FilterFavProductDr(companyId, regionId, allDrs);
                }

                if (response != null && response.Any())
                {
                    await new ScheduleBuilderDomain(this).SetProductSequenceToDelieveryRequests(response, companyId);
                    var orderIds = response.Where(t => t.OrderId > 0).Select(t => t.OrderId ?? 0).ToList();
                    var orders = await Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == companyId && orderIds.Contains(t.Id))
                                                                .Select(t => new { t.IsEndSupplier, t.FuelRequest.FuelRequestDetail.IsDispatchRetainedByCustomer, t.Id, t.IsFTL }).ToListAsync();

                    var countryGroupDetails = response.Where(x => x.BulkPlant != null).Select(x => x.BulkPlant.State.Code).ToList();
                    var countryGroups = await Context.DataContext.MstStates.Where(s => countryGroupDetails.Contains(s.Code) && s.CountryId == 4).Select(t => new { t.Code, McgId = t.MstCountryAsGroup.Id, McgCode = t.MstCountryAsGroup.Code }).ToListAsync();
                    foreach (var dr in response)
                    {
                        var order = orders.FirstOrDefault(t => t.Id == dr.OrderId);
                        if (order != null)
                        {
                            dr.IsBrokered = !order.IsEndSupplier && order.IsDispatchRetainedByCustomer;
                            dr.OrderType = order.IsFTL == true ? 1 : 2;
                        }
                        if (dr.BulkPlant != null && dr.BulkPlant.Country.Code == Country.CAR.ToString())
                        {
                            var countryGroup = countryGroups.FirstOrDefault(s => s.Code == dr.BulkPlant.State.Code);
                            dr.BulkPlant.CountryGroup.Id = countryGroup.McgId;
                            dr.BulkPlant.CountryGroup.Code = countryGroup.McgCode;

                        }
                    }
                    var storedProcedureDomain = new StoredProcedureDomain(this);
                    var responseDetails = await storedProcedureDomain.GetCustomerBrandANDLoadAttributeDetails(companyId, response.Select(t => t.JobId).Distinct().ToList());
                    SetCustomerBrandAndLoadDRAttributes(responseDetails, response, companyId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDeliveryRequests", ex.Message, ex);
            }
            return response;
        }

        private async Task<List<DeliveryRequestViewModel>> FilterFavProductDr(int companyId, string regionId, List<DeliveryRequestViewModel> allDrs)
        {
            var response = new List<DeliveryRequestViewModel>();

            //filter fav prod DR
            var favProduct = await GetRegionFavouriteProducts(null, regionId, companyId);
            if (favProduct != null && favProduct.TfxFavProductTypeId != RegionFavProductType.None)
            {
                if (favProduct.TfxFavProductTypeId == RegionFavProductType.ProductType && favProduct.TfxProductTypeIds != null && favProduct.TfxProductTypeIds.Any())
                {
                    response = allDrs.Where(t => favProduct.TfxProductTypeIds.Contains(t.ProductTypeId)).ToList();
                }
                else if (favProduct.TfxFavProductTypeId == RegionFavProductType.FuelType && favProduct.TfxFuelTypeIds != null && favProduct.TfxFuelTypeIds.Any())
                {
                    // filter drs with order
                    var favTfxProdIds = favProduct.TfxFuelTypeIds.Select(t => t.Id).ToList();
                    var drOrderIds = allDrs.Where(t => t.OrderId.HasValue).Select(t => t.OrderId).Distinct().ToList();
                    if (drOrderIds != null && drOrderIds.Any())
                    {
                        var filterOrderIds = Context.DataContext.Orders.Where(t => drOrderIds.Contains(t.Id) && t.FuelRequest.MstProduct.TfxProductId.HasValue && favTfxProdIds.Contains(t.FuelRequest.MstProduct.TfxProductId.Value)).Select(t => t.Id).ToList();
                        if (filterOrderIds != null && filterOrderIds.Any())
                        {
                            response = allDrs.Where(t => t.OrderId.HasValue && filterOrderIds.Contains(t.OrderId.Value)).ToList();
                        }
                    }
                    // filter remaining dr
                    var remainingDrs = allDrs.Where(t => !t.OrderId.HasValue && t.FuelTypeId.HasValue);
                    if (remainingDrs != null && remainingDrs.Any())
                    {
                        var remainingFuelTypeIds = remainingDrs.Select(t => t.FuelTypeId).ToList();
                        var filterFueltypeIds = Context.DataContext.MstProducts.Where(t => remainingFuelTypeIds.Contains(t.Id) && t.TfxProductId.HasValue && favTfxProdIds.Contains(t.TfxProductId.Value))
                                            .Select(t => t.Id).ToList();
                        if (filterFueltypeIds != null && filterFueltypeIds.Any())
                        {
                            response.AddRange(remainingDrs.Where(t => filterFueltypeIds.Contains(t.FuelTypeId.Value)));
                        }
                    }
                }
                else
                {
                    response = allDrs;
                }
            }
            else
            {
                response = allDrs;
            }

            return response;
        }

        public async Task<List<DeliveryRequestViewModel>> GetCalendarDeliveryRequests(int companyId, CalendarFilterModel inputModel)
        {
            var response = new List<DeliveryRequestViewModel>();
            try
            {
                var url = string.Format(ApplicationConstants.UrlGetCalendarDeliveryRequests, companyId);
                var allDrs = await ApiPostCall<List<DeliveryRequestViewModel>>(url, inputModel);
                if (allDrs != null)
                {
                    var groupDrs = allDrs.GroupBy(t => t.CreatedByRegionId).ToList();
                    foreach (var item in groupDrs)
                    {
                        var filterDrs = await FilterFavProductDr(companyId, item.Key, item.ToList());
                        if (filterDrs != null && filterDrs.Any())
                        {
                            response.AddRange(filterDrs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetCalendarDeliveryRequests", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> ProcessCarrierDeliveyForOttoAlerts()
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlProcessCarrierDeliveyForOttoAlerts);
                response = await ApiPostCall<StatusViewModel>(apiUrl, new { });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "ProcessCarrierDeliveyForOttoAlerts", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DeliveryRequestViewModel>> GetBrokeredDrRequestedToMe(int companyId, string regionId, string selectedDate = null)
        {

            var response = new List<DeliveryRequestViewModel>();
            try
            {
                var url = string.Format(ApplicationConstants.UrlGetBrokeredDrRequestedToMe, companyId, regionId, selectedDate);
                var deliveryRequests = await ApiGetCall<List<DeliveryRequestViewModel>>(url);
                if (deliveryRequests != null && deliveryRequests.Any())
                {
                    await SetCustomerBrandId(companyId, deliveryRequests);
                    SetLoadAndDRQueueAttributes(deliveryRequests, companyId);
                }
                //group DRs based on BlendedDRs
                var normalDeliveryRequests = deliveryRequests.Where(x => x.IsBlendedRequest == false).ToList();
                var blendedDRs = deliveryRequests.Where(x => x.IsBlendedRequest == true && x.IsBlendedDrParent == true).ToList();
                normalDeliveryRequests.ForEach(x =>
                {
                    response.Add(x);
                });
                blendedDRs.ForEach(x =>
                {
                    response.Add(x);
                });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetBrokeredDrRequestedToMe", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DeliveryRequestViewModel>> GetBrokeredDrRequestedByMe(int companyId, string regionId, string selectedDate = null)
        {

            var response = new List<DeliveryRequestViewModel>();
            try
            {
                var url = string.Format(ApplicationConstants.UrlGetBrokeredDrRequestedByMe, companyId, regionId, selectedDate);
                response = await ApiGetCall<List<DeliveryRequestViewModel>>(url);
                if (response != null && response.Any())
                {
                    await SetCustomerBrandId(companyId, response);
                    SetLoadAndDRQueueAttributes(response, companyId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetBrokeredDrRequestedByMe", ex.Message, ex);
            }
            return response;
        }

        public async Task<DeliveryRequestViewModel> GetDeliveryRequestByIdAsync(string deliveryRequestId)
        {
            var response = new DeliveryRequestViewModel();
            try
            {
                if (!string.IsNullOrEmpty(deliveryRequestId))
                {
                    var url = string.Format(ApplicationConstants.UrlGetDeliveryRequestById, deliveryRequestId);
                    response = await ApiGetCall<DeliveryRequestViewModel>(url);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDeliveryRequestByIdAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> SaveTankDetails(AssetViewModel viewModel)
        {
            var result = false;
            try
            {
                var apiUrl = ApplicationConstants.UrlSaveTankDetails;
                var requestModel = GetTankRequestObject(viewModel);
                result = await ApiPostCall<bool>(apiUrl, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "SaveTankDetails", ex.Message, ex);
            }
            return result;
        }

        public async Task<bool> UpdateTankDetails(AssetViewModel viewModel)
        {
            var result = false;
            try
            {
                var apiUrl = ApplicationConstants.UrlUpdateTankDetails;
                var requestModel = GetTankRequestObject(viewModel);
                result = await ApiPostCall<bool>(apiUrl, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateTankDetails", ex.Message, ex);
            }
            return result;
        }

        public async Task<StatusViewModel> CreateRegion(RegionViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlRegionCreate;
                response = await ApiPostCall<StatusViewModel>(apiUrl, viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "CreateRegion", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveJobRegionCarrierDetails(JobToRegionAssignViewModel jobToRegion, JobViewModel jobModel, List<SupplierCarrierViewModel> supplierCarriers)
        {
            var response = new StatusViewModel();
            try
            {
                JobAdditionalDetailsViewModel inputModel = null;
                if (jobModel != null)
                {
                    inputModel = GetAdditonalJobDetailsViewModel(jobModel, jobModel.Id);
                }
                var model = new { JobModel = inputModel, JobToRegion = jobToRegion, SupplierCarriers = supplierCarriers };
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlSaveJobRegionCarrierDetails, model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "SaveJobRegionCarrierDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveTankTypes(TankModalTypeViewModel tankTypes)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlSaveTanktypes;
                response = await ApiPostCall<StatusViewModel>(apiUrl, tankTypes);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "SaveTankTypes", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DsbNotificationViewModel>> GetDsbNotification(string regionId)
        {
            var result = new List<DsbNotificationViewModel>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetDsbNotification, regionId);
                result = await ApiGetCall<List<DsbNotificationViewModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDsbNotification", ex.Message, ex);
            }
            return result;
        }

        public async Task<int> GetDsbNotificationCount(string regionId)
        {
            var result = 0;
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetDsbNotificationCount, regionId);
                result = await ApiGetCall<int>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDsbNotificationCount", ex.Message, ex);
            }
            return result;
        }

        public async Task<StatusViewModel> UpdateDsbNotificationStatus(string id)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlUpdateDsbNotificationStatus;
                response = await ApiPostCall<StatusViewModel>(apiUrl, id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateDsbNotificationStatus", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DispatcherDashboardRegionModel>> GetDispatcherRegionsAsync(int companyId, int dispatcherId)
        {
            var result = new List<DispatcherDashboardRegionModel>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetDispatcherDashboardRegions, companyId, dispatcherId);
                result = await ApiGetCall<List<DispatcherDashboardRegionModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDispatcherRegionsAsync", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<string>> GetDispatcherRegionIdsAsync(int companyId, int dispatcherId)
        {
            var result = new List<string>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetDispatcherRegionIds, companyId, dispatcherId);
                result = await ApiGetCall<List<string>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDispatcherRegionIdsAsync", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<TankModalTypeViewModel>> GetTankTypesByCompany(int companyId)
        {
            var result = new List<TankModalTypeViewModel>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetTankTypeDetails, companyId);
                result = await ApiGetCall<List<TankModalTypeViewModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetTankTypesByCompany", ex.Message, ex);
            }
            return result;
        }

        public async Task<StatusViewModel> DeleteTankDipChartById(string id)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlDeleteTankDipChartById, id);
                response = await ApiGetCall<StatusViewModel>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "DeleteTankDipChartById", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<string>> GetAllTankTypeNameForDipChart(int companyId, string searchValue)
        {
            var response = new List<string>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetAllTankTypeNameForDipChart, companyId, searchValue);
                response = await ApiGetCall<List<string>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetAllTankTypeNameForDipChart", ex.Message, ex);
            }
            return response;
        }

        public async Task<RegionUpdateViewModel> UpdateRegion(RegionViewModel viewModel)
        {
            var response = new RegionUpdateViewModel();
            try
            {
                if (viewModel.Jobs != null && viewModel.Jobs.Any())
                {
                    var jobIds = new List<int>();
                    viewModel.Jobs.ForEach(t => jobIds.Add(t.Id));
                    var jobList = Context.DataContext.Jobs.Where(t1 => jobIds.Contains(t1.Id)).Select(s => new { s.Name, s.Id }).ToList();
                    viewModel.Jobs.ForEach(w => w.Name = jobList.FirstOrDefault(f => f.Id == w.Id)?.Name);
                }
                viewModel.IsDsbDriverSchedule = Context.DataContext.OnboardingPreferences.Where(top => top.IsActive == true && top.CompanyId == viewModel.CompanyId).OrderByDescending(top => top.Id).Select(x => x.IsDSBDriverSchedule).FirstOrDefault();
                var apiUrl = ApplicationConstants.UrlRegionUpdate;
                response = await ApiPostCall<RegionUpdateViewModel>(apiUrl, viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateRegion", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteRegion(string regionId, int deletedBy)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlRegionDelete, regionId, deletedBy);
                response = await ApiPostCall<StatusViewModel>(apiUrl, new { });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "DeleteRegion", ex.Message, ex);
            }
            return response;
        }
        public async Task<string> GetRegionName(string regionId)
        {
            var response = string.Empty;
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetRegionName, regionId);
                response = await ApiGetCall<string>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetRegionName", ex.Message, ex);
            }
            return response;
        }

        public async Task<RegionModel> GetRegions(UserContext userContext)
        {
            var response = new RegionModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetRegions, userContext.CompanyId);
                var regions = await ApiGetCall<List<RegionViewModel>>(apiUrl);

                response.Regions = regions;
                var marineJobs = await Context.DataContext.Orders.Where(w => w.IsActive && w.AcceptedCompanyId == userContext.CompanyId).Select(s => new { customerName = s.BuyerCompany.Name, jobId = s.FuelRequest.Job.Id }).Distinct().ToListAsync();
                if (marineJobs != null && marineJobs.Any())
                {
                    regions.ForEach(t => t.Jobs.ForEach(t1 => t1.Name = t1.Name + (marineJobs.Where(w => w.jobId == t1.Id).Any() ? " (" + marineJobs.Where(w => w.jobId == t1.Id).FirstOrDefault()?.customerName + ")" : "")));
                }
                response.UserId = userContext.Id;
                response.CompanyId = userContext.CompanyId;
                var defaultSlotPeriod = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingDefaultSlotPeriod).Select(t => t.Value).First();
                response.DefaultSlotPeriod = Convert.ToInt32(defaultSlotPeriod);
                response.CountryId = await Context.DataContext.CompanyAddresses.Where(t => t.IsActive && t.IsDefault && t.CompanyId == userContext.CompanyId)
                                                                               .Select(t => t.CountryId)
                                                                               .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetRegions", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<int>> GetDriverDetailsByCompanyId(int companyId, int dispacherId, string regionID)
        {
            var response = new List<int>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetDriverDetailsByCompanyId, companyId, dispacherId, regionID);
                response = await ApiGetCall<List<int>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDriverDetailsByCompanyId", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayExtended>> GetRegions(int companyId)
        {
            var response = new List<DropdownDisplayExtended>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetRegions, companyId);
                var regions = await ApiGetCall<List<RegionViewModel>>(apiUrl);
                response = regions.Select(t => new DropdownDisplayExtended() { Id = t.Id, Name = t.Name }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetRegions", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<SupplierCarrierViewModel>> GetAssignedCarriersForSupplier(int companyId, int carrierCompanyId = 0)
        {
            var response = new List<SupplierCarrierViewModel>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlSupplierCarriersGet, companyId, carrierCompanyId);
                response = await ApiGetCall<List<SupplierCarrierViewModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetAssignedCarriersForSupplier", ex.Message, ex);
            }
            return response;
        }

        public async Task<CarrierJobDetailsViewModel> GetAssignedCarrierUsers(int companyId)
        {
            var response = new CarrierJobDetailsViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlAssignedCarriersGet, companyId);
                response = await ApiGetCall<CarrierJobDetailsViewModel>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetAssignedCarrierUsers", ex.Message, ex);
            }
            return response;
        }

        //public async Task<List<SupplierCarrierViewModel>> GetAssignedCarriersForJob(int companyId)
        //{
        //    var response = new List<SupplierCarrierViewModel>();
        //    try
        //    {
        //        var apiUrl = string.Format(ApplicationConstants.UrlSupplierCarriersGet, companyId);
        //        response = await ApiGetCall<List<SupplierCarrierViewModel>>(apiUrl);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.Logger.WriteException("FreightServiceDomain", "GetAssignedCarriersForSupplier", ex.Message, ex);
        //    }
        //    return response;
        //}

        public async Task<StatusViewModel> AssignCarriers(List<SupplierCarrierViewModel> carriers)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlAssignCarriersPost;
                response = await ApiPostCall<StatusViewModel>(apiUrl, carriers);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "AssignCarriers", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateAssignedCarrier(SupplierCarrierViewModel carrier)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlUpdateAssignedCarriersPost;
                response = await ApiPostCall<StatusViewModel>(apiUrl, carrier);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateAssignedCarrier", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteAssignedCarrier(SupplierCarrierViewModel carrier)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlDeleteAssignedCarriersPost;
                response = await ApiPostCall<StatusViewModel>(apiUrl, carrier);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "DeleteAssignedCarrier", ex.Message, ex);
            }
            return response;
        }

        public async Task<TankAdditionalDetailViewModel> GetTankDetails(int assetId)
        {
            TankAdditionalDetailViewModel result = null;
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetTankDetails, assetId);
                result = await ApiGetCall<TankAdditionalDetailViewModel>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetTankDetails", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<TankDetailViewModel>> GetTankList(List<int> tanks)
        {
            List<TankDetailViewModel> result = null;
            try
            {
                var apiUrl = ApplicationConstants.UrlGetTankList;
                result = await ApiPostCall<List<TankDetailViewModel>>(apiUrl, tanks);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetTankList", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<TruckDetailViewModel>> GetAllTruckFuelRetainDetails(int companyId)
        {
            List<TruckDetailViewModel> result = null;
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetAllTruckRetainFuelDetails, companyId);
                result = await ApiGetCall<List<TruckDetailViewModel>>(apiUrl);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetTankList", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<int>> GetJobsAssignedToDriver(int driverId)
        {
            var jobList = new List<int>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetJobsAssignedToDriver, driverId);
                jobList = await ApiGetCall<List<int>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetJobsAssignedToDriver", ex.Message, ex);
            }
            return jobList;

        }

        public async Task<List<DropdownDisplayExtended>> GetTankModelType(List<int> companyId)
        {
            List<DropdownDisplayExtended> response = new List<DropdownDisplayExtended>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetTankTypes;
                response = await ApiPostCall<List<DropdownDisplayExtended>>(apiUrl, companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetTankModelType", ex.Message, ex);
            }
            return response;
        }

        public async Task<ScheduleOutputDetails> GetTankDetailsBySchedule(List<ScheduleInputDetails> scheduleInputDetails)
        {
            ScheduleOutputDetails result = null;
            try
            {
                var apiUrl = ApplicationConstants.UrlGetTankDetailsBySchedule;
                result = await ApiPostCall<ScheduleOutputDetails>(apiUrl, scheduleInputDetails);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetTankDetailsBySchedule", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<TBDRequestDetailModel>> GetTbdDeliveryRequestDetails(List<string> deliveryRequestIds)
        {
            List<TBDRequestDetailModel> result = new List<TBDRequestDetailModel>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetTbdDeliveryRequestDetails;
                result = await ApiPostCall<List<TBDRequestDetailModel>>(apiUrl, deliveryRequestIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetTbdDeliveryRequestDetails", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<JobTankDetailViewModel>> GetJobTankList(int jobId)
        {
            List<JobTankDetailViewModel> result = null;
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetJobTankList, jobId);
                result = await ApiGetCall<List<JobTankDetailViewModel>>(apiUrl);
                var job = Context.DataContext.Jobs.Where(t => t.Id == jobId).FirstOrDefault();
                if (job != null)
                {
                    result.ForEach(t => { t.JobName = job.Name; t.BuyerCompanyName = job.Company.Name; t.Address = job.Address; });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetJobTankList", ex.Message, ex);
            }
            return result;
        }

        public async Task<TankVolumeAndUllageViewModel> GetTankVolumeAndUllage(TankVolumeAndUllageInputModel requestModel)
        {
            TankVolumeAndUllageViewModel result = null;
            try
            {
                var apiUrl = ApplicationConstants.UrlGetTankVolumeAndUllage;
                result = await ApiPostCall<TankVolumeAndUllageViewModel>(apiUrl, requestModel);
                string jobTimeZone = await Context.DataContext.Jobs.Where(t => t.Id == requestModel.JobId).Select(t => t.TimeZoneName).FirstOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(jobTimeZone))
                {
                    result.CaptureTime = DateTimeOffset.Now.ToTargetDateTimeOffset(jobTimeZone).DateTime.ToString();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetTankVolumeAndUllage", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<DropQuantityByPrePostDipResponseModel>> GetDropQuantityByPrePostDip(List<DropQuantityByPrePostDipRequestModel> requestModel)
        {
            List<DropQuantityByPrePostDipResponseModel> result = null;
            try
            {
                var apiUrl = ApplicationConstants.UrlGetDropQuantityByPrePostDip;
                result = await ApiPostCall<List<DropQuantityByPrePostDipResponseModel>>(apiUrl, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDropQuantityByPrePostDip", ex.Message, ex);
            }
            return result;
        }

        public async Task<StatusViewModel> CreateDriverObject(DriverObjectModel model)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlDriverCreate;
                response = await ApiPostCall<StatusViewModel>(apiUrl, model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "CreateDriverObject", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateDriverObject(DriverObjectModel model)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlDriverUpdate;
                response = await ApiPostCall<StatusViewModel>(apiUrl, model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateDriverObject", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteDriverObject(int driverId, int companyId)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlDriverDelete, driverId, companyId);
                response = await ApiPostCall<StatusViewModel>(apiUrl, new { });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "DeleteDriverObject", ex.Message, ex);
            }
            return response;
        }

        public async Task<DriverObjectModel> GetDriverObject(int driverId, int companyId)
        {
            var response = new DriverObjectModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlDriverGet, driverId, companyId);
                response = await ApiGetCall<DriverObjectModel>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDriverObject", ex.Message, ex);
            }
            return response;
        }

        public async Task<DriverObjectModel> GetDriverObjectById(int driverId)
        {
            var response = new DriverObjectModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetDriverById, driverId);
                response = await ApiGetCall<DriverObjectModel>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDriverObjectById", ex.Message, ex);
            }
            return response;
        }
        public async Task<DriverAdditionalDetailModel> GetDriverAdditionalDetailsAsync(int driverId)
        {
            var response = new DriverAdditionalDetailModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlDriverGetAdditionalDetails, driverId);
                response = await ApiGetCall<DriverAdditionalDetailModel>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDriverAdditionalDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetCompanyShiftDdl(int companyId)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlRegionsGetShiftDdl, companyId);
                response = await ApiGetCall<List<DropdownDisplayExtendedItem>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetCompanyShiftDdl", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<TruckDetailViewModel>> GetAllTruckDetails(int companyId)
        {
            var response = new List<TruckDetailViewModel>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetAllTruckDetails, companyId);
                response = await ApiGetCall<List<TruckDetailViewModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetAllTruckDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<TruckDetailViewModel> GetTruckDetails(string truckId)
        {
            var response = new TruckDetailViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetTruckDetails, truckId);
                response = await ApiGetCall<TruckDetailViewModel>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetTruckDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<DropdownDisplayExtended> GetTruckRegionDetails(string truckId)
        {
            var response = new DropdownDisplayExtended();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetTruckRegionDetails, truckId);
                response = await ApiGetCall<DropdownDisplayExtended>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetTruckRegionDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteTruckAsync(TruckDetailViewModel inputData)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlDeleteTruck;
                response = await ApiPostCall<StatusViewModel>(apiUrl, inputData);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "DeleteTruckAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveTruckDetails(TruckDetailViewModel requestModel)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlSaveTruckDetails);
                response = await ApiPostCall<StatusViewModel>(apiUrl, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "SaveTruckDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateTruckDetails(TruckDetailViewModel requestModel)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlUpdateTruckDetails);
                response = await ApiPostCall<StatusViewModel>(apiUrl, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateTruckDetails", ex.Message, ex);
            }
            return response;
        }

        private object GetTankRequestObject(AssetViewModel viewModel)
        {
            string tankAcceptDelivery = string.Empty;
            string externalTankId = string.Empty;
            if (viewModel.AssetAdditionalDetail.TankAcceptDelivery != null && viewModel.AssetAdditionalDetail.TankAcceptDelivery.Count > 0)
            {
                tankAcceptDelivery = string.Join(",", viewModel.AssetAdditionalDetail.TankAcceptDelivery);
            }
            if (viewModel.AssetAdditionalDetail.DipTestMethod != null && viewModel.AssetAdditionalDetail.DipTestMethod.Value == DipTestMethod.Insight360)
            {
                externalTankId = viewModel.AssetAdditionalDetail.Insight360TankId;
            }
            if (viewModel.AssetAdditionalDetail.DipTestMethod != null && viewModel.AssetAdditionalDetail.DipTestMethod.Value == DipTestMethod.VeederRoot)
            {
                externalTankId = viewModel.AssetAdditionalDetail.VeederRootTankID;
            }
            var requestModel = new
            {
                AssetId = viewModel.AssetAdditionalDetail.AssetId,
                TankId = viewModel.AssetAdditionalDetail.TankId,
                StorageId = viewModel.AssetAdditionalDetail.StorageId,
                TankName = viewModel.Name,
                FuelCapacity = viewModel.AssetAdditionalDetail.FuelCapacity,
                TankNumber = viewModel.AssetAdditionalDetail.TankNumber,
                TankType = viewModel.AssetAdditionalDetail.TankType,
                DipTestMethod = viewModel.AssetAdditionalDetail.DipTestMethod,
                ThresholdDeliveryRequest = viewModel.AssetAdditionalDetail.ThresholdDeliveryRequest,
                FillType = viewModel.AssetAdditionalDetail.FillType,
                MinFill = viewModel.AssetAdditionalDetail.FillType == FillType.UoM ? viewModel.AssetAdditionalDetail.MinFill : null,
                MinFillPercent = viewModel.AssetAdditionalDetail.FillType == FillType.Percent ? viewModel.AssetAdditionalDetail.MinFill : null,
                MaxFill = viewModel.AssetAdditionalDetail.FillType == FillType.UoM ? viewModel.AssetAdditionalDetail.MaxFill : null,
                MaxFillPercent = viewModel.AssetAdditionalDetail.FillType == FillType.Percent ? viewModel.AssetAdditionalDetail.MaxFill : null,
                PhysicalPumpStop = viewModel.AssetAdditionalDetail.PhysicalPumpStop,
                WaterLevel = viewModel.AssetAdditionalDetail.WaterLevel,
                RunOutLevel = viewModel.AssetAdditionalDetail.RunOutLevel,
                Manufacturer = viewModel.AssetAdditionalDetail.Manufacturer,
                ManiFolded = viewModel.AssetAdditionalDetail.ManiFolded,
                TankModelTypeId = viewModel.AssetAdditionalDetail.TankModelTypeId,
                TankConstruction = viewModel.AssetAdditionalDetail.TankConstruction,
                NotificationUponUsageSwing = viewModel.AssetAdditionalDetail.NotificationUponUsageSwing,
                NotificationUponUsageSwingValue = viewModel.AssetAdditionalDetail.NotificationUponUsageSwingValue,
                NotificationUponInventorySwing = viewModel.AssetAdditionalDetail.NotificationUponInventorySwing,
                NotificationUponInventorySwingValue = viewModel.AssetAdditionalDetail.NotificationUponInventorySwingValue,
                TankAcceptDelivery = tankAcceptDelivery,
                FuelTypeId = viewModel.FuelType.Id,//productTypeID
                TFXFuelTypeId = viewModel.AssetTankFuelTypeId,
                ProductTypeName = viewModel.FuelType.Name,
                JobId = viewModel.JobId.Value,
                JobDisplayId = viewModel.JobDisplayId,
                JobName = viewModel.JobName,
                TanksConnected = viewModel.AssetAdditionalDetail.TanksConnected,
                TankSequence = viewModel.AssetAdditionalDetail.TankSequence,
                PedigreeAssetDBID = viewModel.AssetAdditionalDetail.PedigreeAssetDBID,
                SkyBitzRTUID = viewModel.AssetAdditionalDetail.SkyBitzRTUID,
                ExternalTankId = externalTankId,
                VeederRootIPAddress = viewModel.AssetAdditionalDetail.VeederRootIPAddress,
                Port = viewModel.AssetAdditionalDetail.Port,
                IsStopATGPolling = viewModel.AssetAdditionalDetail.IsStopATGPolling
            };
            return requestModel;
        }

        public async Task<List<DeliveryRequestViewModel>> CloneDrsForPreload(List<string> drIds, UserContext userContext)
        {
            var response = new List<DeliveryRequestViewModel>();
            try
            {
                var apiUrl = ApplicationConstants.UrlCloneDrsForPreload;
                response = await ApiPostCall<List<DeliveryRequestViewModel>>(apiUrl, drIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "CloneDrsForPreload", ex.Message, ex);
            }
            return response;
        }


        public async Task<List<TruckDetailViewModel>> GetAllTruckDetailsAsync(int companyId)
        {
            var response = new List<TruckDetailViewModel>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetAllTruckDetails, companyId);
                response = await ApiGetCall<List<TruckDetailViewModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetAllTruckDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        #region DipTest
        public async Task<List<ProductModelToCreateDR>> GetDipTest(int? jobId, string regionId, UserContext userContext, int buyerCompanyId = 0, bool requestFromBuyerWallyBoard = false, bool isCreateDR = true)
        {
            List<ProductModelToCreateDR> result = null;
            try
            {
                string buyerJobs = null;
                if (buyerCompanyId > 0 && !jobId.HasValue)
                {
                    var bjobs = Context.DataContext.Jobs.Where(t => t.CompanyId == buyerCompanyId && t.IsActive).Select(t => t.Id).ToList();
                    buyerJobs = string.Join(",", bjobs);
                }
                var inputModel = new DemandInputModel()
                {
                    CompanyId = userContext.CompanyId,
                    JobId = jobId,
                    RegionId = regionId,
                    BuyerJobs = buyerJobs,
                    IsCreateDR = isCreateDR
                };
                var apiResult = await ApiPostCall<CreateDRTankModel>(ApplicationConstants.UrlGetDipTest, inputModel);
                if (apiResult != null)
                {
                    result = apiResult.Tanks;
                }

                var storedProcedureDomain = new StoredProcedureDomain(this);
                var jobIdList = result.Select(t => t.JobId).Distinct().ToList();
                var jobSiteIdList = result.Select(t => t.SiteId).Distinct().ToList();

                var spResult = await storedProcedureDomain.GetCreateDRLocationInfo(userContext.CompanyId, jobIdList, jobSiteIdList);
                if (spResult != null)
                {
                    SetBuyerInfoToDipTestData(spResult, result);
                }
                result = result.OrderBy(t => t.JobName).ToList();

                //buyer raise dr
                if (requestFromBuyerWallyBoard && jobId != null && result.Any())
                {
                    var job = await storedProcedureDomain.GetBuyerJobsWithProductTypes(userContext.Id, userContext.CompanyId, jobId.GetValueOrDefault());
                    foreach (var tank in result)
                    {
                        tank.SupplierCompanies = job.Where(t => t.ProductTypeId == tank.FuelTypeId)
                            .GroupBy(t => t.AcceptedCompanyId)
                            .Select(t => t.FirstOrDefault())
                            .Where(t => t.AcceptedCompanyId > 0)
                            .Select(t => new DropdownDisplayItem { Id = t.AcceptedCompanyId, Name = t.SupplierCompanyName }).ToList();

                        if (tank.SupplierCompanies.Count == 1)
                        {
                            tank.SupplierCompanyId = tank.SupplierCompanies.Select(t => t.Id).FirstOrDefault();
                        }
                    }
                }
                else if (!requestFromBuyerWallyBoard && result.Any())
                {
                    var _jobIds = new List<long>();
                    if (jobId.GetValueOrDefault() > 0)
                    {
                        _jobIds.Add(jobId.GetValueOrDefault());
                    }
                    else
                    {
                        _jobIds.AddRange(result.Select(r => r.JobId).Distinct().ToList());
                    }
                    if (isCreateDR && apiResult != null)
                    {
                        await GetOrdersToCreateDr(_jobIds, result, DateTimeOffset.Now, userContext, apiResult.FavoriteProducts);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDipTest", ex.Message, ex);
            }
            result = result.Where(x => x.IsDispatchRetainedByCustomerDisplay).ToList();
            return result;
        }

        public async Task<bool> IsTankNotAvailableForOrderProducts(UserContext userContext, int buyerCompanyId, int jobId, string regionId, string productsToExclude, int endSupplier = 0)
        {
            bool response = false;
            try
            {
                List<int> jobIds = new List<int>();
                List<int> _productsToExclude = productsToExclude.Split(',').Select(int.Parse).ToList();

                if (jobId == 0)
                {
                    if (!string.IsNullOrWhiteSpace(regionId))
                    {
                        var jobs = await new FreightServiceDomain(this).GetJobListForCarrier(regionId, userContext);
                        jobIds = jobs.SelectMany(t => t.Jobs.Select(t1 => t1.Id)).Distinct().ToList();
                    }
                }
                else
                {
                    jobIds.Add(jobId);
                }

                response = Context.DataContext.Orders
                               .Any(t => t.BuyerCompanyId == buyerCompanyId
                                           && (t.IsEndSupplier || t.FuelRequest.FuelRequestDetail.IsDispatchRetainedByCustomer)
                                           && t.AcceptedCompanyId == userContext.CompanyId
                                           && t.IsActive
                                           && !_productsToExclude.Contains(t.FuelRequest.MstProduct.ProductTypeId)
                                           && (jobIds.Contains(t.FuelRequest.JobId) && t.FuelRequest.Job.IsActive)
                                           && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetOrdersToCreateDr", ex.Message, ex);
            }

            return response;
        }

        private async Task GetOrdersToCreateDr(List<long> jobIds, List<ProductModelToCreateDR> demands, DateTimeOffset? loadDate, UserContext userContext, RegionFavProductModel favProduct, int carrierStatus = -1)
        {
            StoredProcedureDomain spDomain = new StoredProcedureDomain(this);
            List<OrderPickupDetailModel> response = new List<OrderPickupDetailModel>();
            try
            {
                var productTypeIds = demands.Select(r => r.FuelTypeId).ToList();
                var lstFavFuelTypeIds = new List<int>();
                var lstFavProductTypeIds = new List<int>();
                if (favProduct != null)
                {
                    if (favProduct.TfxFavProductTypeId == RegionFavProductType.ProductType && favProduct.TfxProductTypeIds != null && favProduct.TfxProductTypeIds.Any())
                    {
                        lstFavProductTypeIds = favProduct.TfxProductTypeIds;
                    }
                    else if (favProduct.TfxFavProductTypeId == RegionFavProductType.FuelType && favProduct.TfxFuelTypeIds != null && favProduct.TfxFuelTypeIds.Any())
                    {
                        lstFavFuelTypeIds = favProduct.TfxFuelTypeIds.Select(t => t.Id).ToList();
                    }
                }
                var spResponse = await spDomain.GetProductAndOrderInfoToCreateDR(userContext.CompanyId, loadDate, jobIds, productTypeIds, lstFavFuelTypeIds, lstFavProductTypeIds);

                if (spResponse != null && spResponse.Orders != null)
                {
                    Parallel.ForEach(spResponse.Orders, item =>
                    {
                        response.Add(item);
                    });
                }

                if (spResponse != null && demands != null && response != null)
                {
                    foreach (var demand in demands)
                    {
                        var productTypes = new List<int>() { demand.FuelTypeId };
                        productTypes.AddRange(spResponse.ProductMappings.Where(t => t.ProductTypeId == demand.FuelTypeId).Select(t => t.MappedToProductTypeId).ToList());
                        demand.OrderPickupDetails = response.Where(o => o.JobId == demand.JobId && productTypes.Contains(o.ProductTypeId)).ToList();

                        var blendProductTypes = new List<int>() { demand.FuelTypeId };
                        blendProductTypes.AddRange(spResponse.BlendProductMappings.Where(t => t.ProductTypeId == demand.FuelTypeId).Select(t => t.MappedToProductTypeId).ToList());
                        demand.BlendOrderPickupDetails = response.Where(o => o.JobId == demand.JobId && blendProductTypes.Contains(o.ProductTypeId)).ToList();

                        if (response.Any(o => o.JobId == demand.JobId && productTypes.Contains(o.ProductTypeId) && o.IsOrderInfoDisplay == false))
                        {
                            demand.IsDispatchRetainedByCustomerDisplay = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetOrdersToCreateDr", ex.Message, ex);
            }
        }

        private void SetBuyerInfoToDipTestData(List<RaiseDrJobInfo> JobsInfo, List<ProductModelToCreateDR> result)
        {
            Parallel.ForEach(result, item =>
            {
                var jobdetails = JobsInfo.FirstOrDefault(t => t.Id == item.JobId);
                if (jobdetails == null && !string.IsNullOrWhiteSpace(item.SiteId))
                {
                    jobdetails = JobsInfo.FirstOrDefault(t => t.DisplayJobID == item.SiteId);
                }
                if (jobdetails != null)
                {
                    item.JobName = jobdetails.Name;
                    item.BuyerCompanyId = jobdetails.CompanyId;
                    item.BuyerCompanyName = jobdetails.CompanyName;
                    item.UoM = jobdetails.UoM.ToString();
                    item.JobId = jobdetails.Id;
                    item.IsTankAndAssetAvailableForJob = jobdetails.IsTankAndAssetAvailableForJob;
                    item.ExistingDR.ForEach(t => t.CreatedOn = t.CreatedDate.ToTargetDateTimeOffset(jobdetails.TimeZoneName).DateTime.ToString());
                    item.LocationManagedType = (int)LocationManagedType.FullyCarrierManaged;
                }
            });
        }

        private async Task<List<int>> GetRetailJobIdsForCompany(UserContext userContext)
        {
            List<int> JobIds = new List<int>();
            try
            {
                if (userContext.IsBuyerSupplierAndCarrierCompany || userContext.IsBuyerCompany || userContext.IsBuyerAndSupplierCompany)
                {
                    JobIds = await Context.DataContext.JobXAssets.Where(t => t.RemovedBy == null && t.Asset.Type == (int)AssetType.Tank
                                                                                && t.Job.CompanyId == userContext.CompanyId
                                                                                && t.Asset.IsActive && t.Job.IsActive
                                                                                && t.Job.DisplayJobID != null && t.Job.DisplayJobID.Trim() != "")
                                                                            .Select(t => t.JobId).Distinct().ToListAsync();
                }

                if (userContext.IsCarrierCompany || userContext.IsBuyerSupplierAndCarrierCompany || userContext.IsSupplierAndCarrierCompany)
                {
                    var apiUrl = string.Format(ApplicationConstants.UrlGetSiteList, userContext.CompanyId, string.Empty);
                    var cJobs = await ApiGetCall<List<DropdownDisplayExtendedItem>>(apiUrl);
                    if (cJobs != null)
                    {
                        JobIds.AddRange(cJobs.Select(t => t.Id));
                    }
                }

                if (userContext.IsSupplierCompany || userContext.IsBuyerSupplierAndCarrierCompany || userContext.IsSupplierAndCarrierCompany || userContext.IsBuyerAndSupplierCompany)
                {
                    var sJobIds = await Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == userContext.CompanyId && t.IsActive
                                                                && t.FuelRequest.Job.JobXAssets.Any(x1 => x1.RemovedBy == null && x1.Asset.Type == (int)AssetType.Tank
                                                                    && x1.Asset.IsActive && x1.Job.IsActive
                                                                    && x1.Job.DisplayJobID != null && x1.Job.DisplayJobID.Trim() != "")
                                                                ).Select(t1 => t1.FuelRequest.JobId).ToListAsync();
                    if (sJobIds != null)
                    {
                        JobIds.AddRange(sJobIds);
                    }
                }
                JobIds = JobIds.Distinct().ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetRetailJobIdsForCompany", ex.Message, ex);
            }
            return JobIds;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetSiteList(string regionId, UserContext userContext, string Prefix)
        {
            List<DropdownDisplayExtendedItem> result = null;
            try
            {
                List<DropdownDisplayExtendedItem> buyerJobs = new List<DropdownDisplayExtendedItem>();
                List<DropdownDisplayExtendedItem> carrierJobs = new List<DropdownDisplayExtendedItem>();
                if (userContext.IsBuyerSupplierAndCarrierCompany || userContext.IsBuyerCompany || userContext.IsBuyerAndSupplierCompany)
                {
                    buyerJobs = await Context.DataContext.JobXAssets.Where(t => t.RemovedBy == null && t.Asset.Type == (int)AssetType.Tank
                                                                                && t.Job.CompanyId == userContext.CompanyId
                                                                                && t.Asset.IsActive && t.Job.IsActive
                                                                                && t.Job.DisplayJobID != null && t.Job.DisplayJobID.Trim() != "" && (t.Job.DisplayJobID.Contains(Prefix) || (Prefix == string.Empty)))
                                                                            .Select(t => new DropdownDisplayExtendedItem { Id = t.JobId, Name = t.Job.Name, Code = t.Job.DisplayJobID }).Distinct().ToListAsync();

                    buyerJobs.ForEach(t => t.Name = !string.IsNullOrWhiteSpace(t.Code) ? $"{t.Name}-{t.Code}" : t.Name);
                }
                if (userContext.IsCarrierCompany || userContext.IsBuyerSupplierAndCarrierCompany || userContext.IsSupplierAndCarrierCompany || userContext.IsBuyerCompany)
                {
                    var apiUrl = string.Format(ApplicationConstants.UrlGetSiteList, userContext.CompanyId, regionId);
                    carrierJobs = await ApiGetCall<List<DropdownDisplayExtendedItem>>(apiUrl);
                }

                return buyerJobs.Union(carrierJobs).Distinct().OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetSiteList", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<CustomerJobsForCarrierViewModel>> GetJobListForCarrier(string regionId, UserContext userContext, bool isShowCarrierManaged = false, string carriers = "")
        {
            var result = new List<CustomerJobsForCarrierViewModel>();
            try
            {
                var buyerJobs = new List<CustomerJobForCarrierViewModel>();
                var carrierJobs = new List<CustomerJobForCarrierViewModel>();
                var regionJobs = new List<CustomerJobForCarrierViewModel>();
                List<int> carrierIds = new List<int>();
                List<int> locationIds = new List<int>();
                List<int> carrierJobIds = new List<int>();
                bool isBuyerCompany = false;
                if (!String.IsNullOrEmpty(carriers))
                    carrierIds = carriers.Split(',').Select(int.Parse).ToList();

                if (!string.IsNullOrEmpty(regionId) && (userContext.IsBuyerSupplierAndCarrierCompany || userContext.IsSupplierCompany || userContext.IsBuyerAndSupplierCompany || userContext.IsCarrierCompany || userContext.IsSupplierAndCarrierCompany))
                {
                    var apiUrl = string.Format(ApplicationConstants.UrlGetJobListForCarrier, userContext.CompanyId, regionId);
                    regionJobs = await ApiGetCall<List<CustomerJobForCarrierViewModel>>(apiUrl);
                }
                if (regionJobs != null)
                {
                    locationIds = regionJobs.Where(t => t.Job != null).Select(t => t.Job.Id).ToList();
                }
                if (userContext.IsBuyerSupplierAndCarrierCompany || userContext.IsBuyerCompany || userContext.IsBuyerAndSupplierCompany)
                {
                    isBuyerCompany = true;
                }
                RaiseDrLocationModel spResponse = await new StoredProcedureDomain(this).GetRaiseDRLocationInfo(userContext.CompanyId, isBuyerCompany, locationIds, carrierIds);
                if (spResponse != null)
                {
                    if (spResponse.JobDetails != null)
                    {
                        var jobs = spResponse.JobDetails;
                        buyerJobs = jobs.Select(t => new CustomerJobForCarrierViewModel()
                        {
                            CompanyName = userContext.CompanyName,
                            CompanyId = userContext.CompanyId,
                            Job = new JobRegionModel
                            {
                                Id = t.Id,
                                Name = t.Name,
                                Code = t.DisplayJobID,
                                DisplayName = t.DisplayName,
                                RegionId = regionJobs.Where(t1 => t1.Job.Id == t.Id).Select(t1 => t1.RegionId).FirstOrDefault()
                            }
                        }).Distinct().ToList();
                    }
                    if (spResponse.JobIds != null)
                    {
                        carrierJobIds = spResponse.JobIds;
                    }
                    if (spResponse.CarrierJobDetails != null)
                    {
                        carrierJobs = spResponse.CarrierJobDetails.Select(t => new CustomerJobForCarrierViewModel()
                        {
                            CompanyName = t.CompanyName,
                            CompanyId = t.CompanyId,
                            Job = new JobRegionModel
                            {
                                Id = t.Id,
                                Name = t.Name,
                                Code = t.DisplayJobID,
                                DisplayName = t.DisplayName,
                                RegionId = regionJobs.Where(t1 => t1.Job.Id == t.Id).Select(t1 => t1.RegionId).FirstOrDefault(),
                                LocationManagedType = carrierJobIds.Contains(t.Id) ? (int)LocationManagedType.FullyCarrierManaged : 0
                            }
                        }).Distinct().ToList();
                    }
                }
                result = buyerJobs.Union(carrierJobs).OrderBy(t => t.Job.Name).GroupBy(x => x.CompanyId)
                                            .Select(y => new CustomerJobsForCarrierViewModel
                                            {
                                                CompanyId = y.First().CompanyId,
                                                CompanyName = y.First().CompanyName,
                                                RegionIds = y.Select(t => t.RegionId).Distinct().ToList(),
                                                Jobs = y.Select(y1 => y1.Job).OrderBy(y1 => y1.Name).ToList()
                                            }).OrderBy(t => t.CompanyName).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetJobListForCarrier", ex.Message, ex);
            }
            return result;
        }



        public async Task<List<CustomerJobsForCarrierViewModel>> GetCreateLoadJobListForCarrier(string regionId, UserContext userContext)
        {
            var result = new List<CustomerJobsForCarrierViewModel>();
            try
            {
                var buyerJobs = new List<CustomerJobForCarrierViewModel>();
                var carrierJobs = new List<CustomerJobForCarrierViewModel>();

                if (userContext.IsBuyerSupplierAndCarrierCompany || userContext.IsBuyerCompany || userContext.IsBuyerAndSupplierCompany)
                {
                    buyerJobs = await Context.DataContext.Jobs.Where(t => t.CompanyId == userContext.CompanyId
                                                                                && t.IsActive)
                                                                .Select(t => new CustomerJobForCarrierViewModel
                                                                {
                                                                    CompanyName = userContext.CompanyName,
                                                                    CompanyId = userContext.CompanyId,
                                                                    Job = new JobRegionModel { Id = t.Id, Name = t.Name, Code = t.DisplayJobID }

                                                                }).Distinct().ToListAsync();

                    buyerJobs.ForEach(t => t.Job.Name = !string.IsNullOrWhiteSpace(t.Job.Code) ? $"{t.Job.Name}-{t.Job.Code}" : t.Job.Name);
                }

                if (!string.IsNullOrEmpty(regionId) && (userContext.IsBuyerSupplierAndCarrierCompany || userContext.IsSupplierCompany || userContext.IsBuyerAndSupplierCompany || userContext.IsCarrierCompany || userContext.IsSupplierAndCarrierCompany))
                {
                    var apiUrl = string.Format(ApplicationConstants.UrlGetBrokereJobListForCarrier, userContext.CompanyId, regionId);
                    var regionJobs = await ApiGetCall<List<CustomerJobForCarrierViewModel>>(apiUrl);
                    carrierJobs = await GetCarrierJobs(userContext, carrierJobs, regionJobs);
                    if (buyerJobs.Any())
                    {
                        foreach (var job in buyerJobs)
                        {
                            job.Job.RegionId = regionJobs.Where(t => t.Job.Id == job.Job.Id).Select(t => t.RegionId).FirstOrDefault();
                        }
                    }
                }

                var jobUnion = new List<CustomerJobForCarrierViewModel>();
                MergeBuyerAndSupplierJobs(out result, buyerJobs, carrierJobs, out jobUnion);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetCreateLoadJobListForCarrier", ex.Message, ex);
            }
            return result;
        }

        public async Task<StatusViewModel> RaiseDeliveryRequestsFromBuyerApp(List<ApiRaiseDeliveryRequestInput> raiseDeliveryRequests, UserContext userContext)
        {
            var response = new StatusViewModel();
            var notificationDomain = new NotificationDomain(this);
            try
            {
                var requests = GetDeliveryRequestModel(raiseDeliveryRequests, userContext);
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlRaiseDeliveryRequestsFromBuyerApp, requests.DeliveryRequests);
                foreach (var item in response.EntityIds)
                {
                    var message = new TankDeliveryRequestMessageViewModel { EntityId = item };
                    var jsonMessage = new JavaScriptSerializer().Serialize(message);
                    await notificationDomain.AddNotificationEventAsync(EventType.TankDeliveryRequestCreated, 0, userContext.Id, null, jsonMessage);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "RaiseDeliveryRequestsFromBuyerApp", ex.Message, ex);
            }
            return response;
        }

        public List<RaiseDeliveryRequestInput> GetRaiseDeliveryRequests(List<RaiseDeliveryRequestInput> raiseDeliveryRequests)
        {
            var response = new List<RaiseDeliveryRequestInput>();
            foreach (var dr in raiseDeliveryRequests)
            {
                if (dr.IsBlendedRequest)
                {
                    string json = JsonConvert.SerializeObject(dr);
                    foreach (var blendedDr in dr.BlendedRequests)
                    {
                        RaiseDeliveryRequestInput model = JsonConvert.DeserializeObject<RaiseDeliveryRequestInput>(json);
                        model.OrderId = blendedDr.OrderId;
                        model.Berth = blendedDr.Berth;
                        if (!dr.IsCommonPickupForBlend)
                        {
                            model.Bulkplant = blendedDr.BulkPlant;
                            model.Terminal = blendedDr.Terminal;
                            model.PickupLocationType = blendedDr.PickupLocationType;
                        }
                        model.IsAdditive = blendedDr.IsAdditive;
                        model.QuantityInPercent = blendedDr.QuantityInPercent;
                        model.PoNumber = blendedDr.PoNumber;
                        model.ProductType = blendedDr.ProductType;
                        model.ProductTypeId = blendedDr.ProductTypeId;
                        model.RequiredQuantity = blendedDr.RequiredQuantity;
                        model.BlendParentProductTypeId = dr.BlendParentProductTypeId > 0 ? dr.BlendParentProductTypeId : dr.ProductTypeId;
                        response.Add(model);
                    }
                }
                else
                {
                    response.Add(dr);
                }
            }
            return response;
        }

        public async Task<DeliveryRequestsViewModel> RaiseDeliveryRequests(List<RaiseDeliveryRequestInput> raiseDeliveryRequests, UserContext userContext)
        {
            var response = new DeliveryRequestsViewModel();
            var notificationDomain = new NotificationDomain(this);
            try
            {
                //check if product is available in region for specific location
                var IsValidProducts = await IsValidDRProducts(userContext.CompanyId, raiseDeliveryRequests);
                if (IsValidProducts.StatusCode == Status.Failed)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = IsValidProducts.StatusMessage;
                    return response;
                }

                raiseDeliveryRequests = raiseDeliveryRequests.FindAll(t => t.RequiredQuantity > 0 || t.ScheduleQuantityType != (int)ScheduleQuantityType.Quantity || (t.isRecurringSchedule && t.RecurringSchdule.Count > 0));
                if (raiseDeliveryRequests.Count == 0)
                {
                    return response;
                }
                if (raiseDeliveryRequests.Any(t => t.IsBlendedRequest))
                {
                    raiseDeliveryRequests = GetRaiseDeliveryRequests(raiseDeliveryRequests);
                }
                var requests = await SetDeliveryRequestsParameters(raiseDeliveryRequests, false, userContext);
                //generate unique DR Id
                GenerateUniqueDRId(raiseDeliveryRequests, userContext, requests);
                if (requests.StatusCode != Status.Failed)
                {
                    //post the delivery request which has isRecurringSchedule=false
                    response = await ApiPostCall<DeliveryRequestsViewModel>(ApplicationConstants.UrlRaiseDeliveryRequests, requests.DeliveryRequests);
                    foreach (var item in response.EntityIds)
                    {
                        var message = new TankDeliveryRequestMessageViewModel { EntityId = item };
                        var jsonMessage = new JavaScriptSerializer().Serialize(message);
                        await notificationDomain.AddNotificationEventAsync(EventType.TankDeliveryRequestCreated, 0, userContext.Id, null, jsonMessage);
                    }
                }
                else
                {
                    response.StatusCode = requests.StatusCode;
                    response.StatusMessage = requests.StatusMessage;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "RaiseDeliveryRequests", ex.Message, ex);
            }
            return response;
        }



        public async Task<StatusViewModel> UpdateHeldDrValidationStatus(string id, string message)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlUpdateHeldDrValidation, id, message);
                response = await ApiGetCall<StatusViewModel>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateHeldDrValidationStatus", ex.Message, ex);
            }
            return response;
        }

        public async Task<int> GetHeldDeliveryRequestCount(UserContext userContext)
        {
            int response = 0;
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetHeldDeliveryRequestCount, userContext.CompanyId);
                var _response = await ApiGetCall<int?>(apiUrl);

                if (_response != null)
                    response = _response.GetValueOrDefault();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetHeldDeliveryRequestCount", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ReCreateDeliveryRequests(ReCreateDeliveryRequestsViewModel viewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                viewModel.DeliveryRequests.ForEach(t => t.DelReqSource = (int)DRSource.Recreated);
                var inputData = new { ExistingDrIds = viewModel.ExistingDrIds, DeliveryRequests = viewModel.DeliveryRequests, UserId = userContext.Id };
                response = await ApiPostCall<DeliveryRequestsViewModel>(ApplicationConstants.UrlReCreateDeliveryRequests, inputData);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "ReCreateDeliveryRequests", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> BrokerDeliveryRequest(List<RaiseDeliveryRequestInput> raiseDeliveryRequests)
        {
            var response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlRaiseDeliveryRequests, raiseDeliveryRequests);
            return response;
        }

        private async Task<RaiseDeliveryRequestModel> SetDeliveryRequestsParameters(List<RaiseDeliveryRequestInput> raiseDeliveryRequests, bool skipMarineConversion, UserContext userContext)
        {
            var helperDomain = new HelperDomain(this);
            var deliveryRequestModel = new RaiseDeliveryRequestModel();
            var assignedCompanyId = 0; bool autoAssignDREnable = false;
            int supplierCompanyId = userContext.CompanyId;

            try
            {
                if (raiseDeliveryRequests != null && raiseDeliveryRequests.Count > 0)
                {
                    bool requestFromBuyerWallyBoard = raiseDeliveryRequests.Any(t => t.RequestFromBuyerWallyBoard);
                    List<int> jobIds = raiseDeliveryRequests.Select(t => t.JobId).Distinct().ToList();
                    List<int> orderIds = raiseDeliveryRequests.Where(t => t.OrderId != null).Select(t => t.OrderId.Value).Distinct().ToList();
                    List<string> tankIds = raiseDeliveryRequests.Where(t => t.TankId != null).Select(t => t.TankId).Distinct().ToList();
                    List<string> storageIds = raiseDeliveryRequests.Where(t => t.StorageId != null).Select(t => t.StorageId).Distinct().ToList();
                    List<int> productTypeIds = raiseDeliveryRequests.Where(t => t.ProductTypeId != 0).Select(t => t.ProductTypeId).ToList();

                    StoredProcedureDomain spDomain = new StoredProcedureDomain(this);
                    var spResponse = await spDomain.GetDataToCreateDR(orderIds, jobIds, tankIds, storageIds);

                    var jobOffsetInfo = new List<TimeZoneOffsetModel>();
                    if (spResponse != null && spResponse.Jobs != null)
                    {
                        jobOffsetInfo = spResponse.Jobs.Select(t => new TimeZoneOffsetModel() { Id = t.Id, TimeZoneName = t.TimeZoneName }).ToList();
                        GetOffsetForTimezones(jobOffsetInfo);
                    }
                    var supplierCompanyIds = new List<TankBuyerSupplierViewModel>();
                    if (userContext.CompanyTypeId == CompanyType.Buyer && !requestFromBuyerWallyBoard)
                    {
                        if (productTypeIds.Any())
                        {
                            productTypeIds = helperDomain.GetCompatibleProducts(productTypeIds);

                            supplierCompanyIds = Context.DataContext.Orders.Where(t => t.BuyerCompanyId == userContext.CompanyId &&
                                                    jobIds.Contains(t.FuelRequest.JobId) &&
                                                    (productTypeIds.Contains(t.FuelRequest.MstProduct.ProductTypeId) || productTypeIds.Contains(t.FuelRequest.MstProduct.MstTFXProduct.ProductTypeId)))
                                                   .OrderByDescending(t => t.Id).Select(t => new TankBuyerSupplierViewModel
                                                   {
                                                       JobId = t.FuelRequest.JobId,
                                                       ProductTypeId = t.FuelRequest.MstProduct.ProductTypeId,
                                                       SupplierCompanyId = t.AcceptedCompanyId,
                                                   }).Distinct().ToList();
                        }
                        else
                        {
                            supplierCompanyIds = Context.DataContext.Orders.Where(t => orderIds.Contains(t.Id))
                                             .OrderByDescending(t => t.Id).Select(t => new TankBuyerSupplierViewModel
                                             {
                                                 JobId = t.FuelRequest.JobId,
                                                 ProductTypeId = t.FuelRequest.MstProduct.ProductTypeId,
                                                 SupplierCompanyId = t.AcceptedCompanyId,
                                             }).Distinct().ToList();
                        }
                    }
                    var productIds = raiseDeliveryRequests.Select(x => x.ProductTypeId).ToList();
                    var productCodeDetails = await Context.DataContext.MstProductTypes.Where(x => productIds.Contains(x.Id) && x.IsActive).Select(x => new { x.Id, x.ProductCode }).ToListAsync();
                    foreach (var item in raiseDeliveryRequests)
                    {
                        var job = spResponse?.Jobs?.FirstOrDefault(t => t.Id == item.JobId);
                        var order = spResponse?.Orders?.FirstOrDefault(t => t.Id == item.OrderId);
                        var tank = spResponse?.Assets?.FirstOrDefault(t => t.VehicleId == item.TankId && t.Vendor == item.StorageId && t.JobId == item.JobId);
                        if (userContext.CompanyTypeId == CompanyType.Buyer && !requestFromBuyerWallyBoard)
                        {
                            if (productTypeIds.Any())
                            {
                                if (spResponse != null && spResponse.Assets != null && spResponse.Assets.Any() && tank != null)
                                {
                                    var supplierDetails = supplierCompanyIds.Where(t => t.JobId == item.JobId && productTypeIds.Contains(t.ProductTypeId)).OrderByDescending(top => top.SupplierCompanyId).Select(t => t.SupplierCompanyId).Distinct().ToList();
                                    if (supplierDetails.Any() && supplierCompanyIds.Any(top => top.ProductTypeId == tank.FuelType.Value))
                                    {
                                        foreach (var supplieritem in supplierDetails)
                                        {
                                            var regionId = Task.Run(() => GetRegionIdForJob(job.Id, supplieritem)).Result;
                                            if (!string.IsNullOrEmpty(regionId))
                                            {
                                                supplierCompanyId = supplieritem;
                                                break;
                                            }
                                        }
                                    }
                                    else if (!supplierCompanyIds.Any() || !supplierCompanyIds.Any(top => top.ProductTypeId == tank.FuelType.Value))
                                    {
                                        deliveryRequestModel.StatusCode = Status.Failed;
                                        deliveryRequestModel.StatusMessage = String.Format(Resource.valMessageNoOpenOrderFound, tank.Name);
                                        return deliveryRequestModel;
                                    }

                                }
                                else
                                {
                                    supplierCompanyId = supplierCompanyIds.Where(t => t.JobId == item.JobId && productTypeIds.Contains(t.ProductTypeId)).Select(t => t.SupplierCompanyId).FirstOrDefault();
                                }
                            }
                            else
                            {
                                supplierCompanyId = supplierCompanyIds.Where(t => t.JobId == item.JobId).Select(t => t.SupplierCompanyId).FirstOrDefault();
                            }
                        }

                        if (requestFromBuyerWallyBoard)
                        {
                            if (item.SupplierCompanyId > 0)
                            {
                                supplierCompanyId = item.SupplierCompanyId.Value;
                            }
                            else if (item.SupplierCompanyId.GetValueOrDefault() == 0)
                            {
                                deliveryRequestModel.StatusMessage = Resource.valMessageJobNotAssignedToRegion;
                                continue;
                            }
                        }
                        //  Check if auto assign DR to carrier enable and assigned the carrier assigned for the location.
                        if (userContext.CompanyTypeId == CompanyType.Supplier || userContext.CompanyTypeId == CompanyType.BuyerAndSupplier || userContext.CompanyTypeId == CompanyType.Buyer || userContext.IsBuyerAdmin || userContext.IsSupplierAdmin || userContext.IsSuperAdmin || userContext.CompanyTypeId == CompanyType.SupplierAndCarrier || userContext.CompanyTypeId == CompanyType.BuyerSupplierAndCarrier)
                        {
                            // get carrier id for that supplier for the matched job.
                            if (item.IsReAssignToCarrier)
                            {
                                assignedCompanyId = supplierCompanyId;
                            }
                            else
                            {
                                assignedCompanyId = Task.Run(() => GetAssignedCarrierCompanyId(supplierCompanyId, item.JobId)).Result;
                            }
                            if (assignedCompanyId == 0)
                            {
                                assignedCompanyId = supplierCompanyId;
                            }
                            else
                            {
                                if (item.OrderId.HasValue && item.OrderId.Value > 0)
                                {
                                    var childOrders = await spDomain.GetBrokeredChildOrders(item.OrderId.Value, (int)OrderStatus.Open, assignedCompanyId);
                                    if (childOrders != null && childOrders.Any())
                                    {
                                        item.OrderId = childOrders.Select(t => t.OrderId).OrderBy(t => t).FirstOrDefault();
                                    }
                                }
                                autoAssignDREnable = true;
                            }
                        }

                        if (item.PickupLocationType == PickupLocationType.BulkPlant)
                        {
                            if (item.Bulkplant != null && (item.Bulkplant.Country.Id == (int)Country.CAR || item.Bulkplant.Country.Code == Country.CAR.ToString())
                                && item.Bulkplant.IsMissingAddress())
                            {
                                var state = Context.DataContext.MstStates.First(t => t.Id == item.Bulkplant.State.Id).ToViewModel();
                                var country = Context.DataContext.MstCountries.First(t => t.Id == item.Bulkplant.Country.Id || t.Code.ToLower() == item.Bulkplant.Country.Code.Trim().ToLower()).ToViewModel();
                                item.Bulkplant.Address = string.IsNullOrWhiteSpace(item.Bulkplant.Address) ? (state.Name ?? Resource.lblCaribbean) : item.Bulkplant.Address;
                                item.Bulkplant.City = string.IsNullOrWhiteSpace(item.Bulkplant.City) ? (state.Name ?? Resource.lblCaribbean) : item.Bulkplant.City;
                                item.Bulkplant.ZipCode = string.IsNullOrWhiteSpace(item.Bulkplant.ZipCode) ? (state.Name ?? Resource.lblCaribbean) : item.Bulkplant.ZipCode;
                                item.Bulkplant.CountyName = string.IsNullOrWhiteSpace(item.Bulkplant.CountyName) ? (state.Name ?? Resource.lblCaribbean) : item.Bulkplant.CountyName;
                                if (item.Bulkplant.Latitude == 0 || item.Bulkplant.Longitude == 0)
                                {
                                    var point = GoogleApiDomain.GetGeocode($"{item.Bulkplant.Address} {item.Bulkplant.City} {state.Code} {country.Code} {item.Bulkplant.ZipCode}");
                                    if (point != null)
                                    {
                                        item.Bulkplant.Latitude = Convert.ToDecimal(point.Latitude);
                                        item.Bulkplant.Longitude = Convert.ToDecimal(point.Longitude);
                                    }
                                }
                            }

                        }

                        var request = new RaiseDeliveryRequestViewModel
                        {
                            AssignedTo = userContext.Id,
                            //AssignedToCompanyId = userContext.CompanyTypeId == CompanyType.Buyer ? supplierCompanyId : userContext.CompanyId,
                            AssignedToCompanyId = autoAssignDREnable ? assignedCompanyId : userContext.CompanyTypeId == CompanyType.Buyer ? supplierCompanyId : userContext.CompanyId,
                            CreatedBy = userContext.Id,
                            CreatedByCompanyId = userContext.CompanyId,
                            CreatedOn = DateTimeOffset.Now,
                            Priority = item.Priority == 0 ? DeliveryReqPriority.MustGo : item.Priority,
                            CurrentThreshold = item.CurrentThreshold,
                            RequiredQuantity = item.RequiredQuantity,
                            ScheduleQuantityType = item.ScheduleQuantityType,
                            SiteId = item.SiteId,
                            CreditApprovalFilePath = item.CreditApprovalFilePath,
                            Status = DeliveryReqStatus.Pending,
                            StorageId = item.StorageId,
                            TankId = item.TankId,
                            JobId = item.JobId,
                            IsBlendedRequest = item.IsBlendedRequest,
                            BlendedGroupId = item.BlendedGroupId,
                            IsAdditive = item.IsAdditive,
                            QuantityInPercent = item.QuantityInPercent,
                            SupplierCompanyId = supplierCompanyId,
                            CreatedByRegionId = item.CreatedByRegionId,
                            //AssignedToRegionId = item.CreatedByRegionId,
                            OrderId = item.OrderId,
                            TankMaxFill = item.TankMaxFill,
                            isRecurringSchedule = item.isRecurringSchedule,
                            RecurringSchdule = item.isRecurringSchedule == true ? item.RecurringSchdule == null ? new List<RecurringSchdule>() : item.RecurringSchdule : new List<RecurringSchdule>(),
                            BuyerCompanyId = item.BuyerCompanyId,
                            PoNumber = item.PoNumber,
                            DeliveryRequestFor = item.DeliveryRequestFor,
                            Notes = item.Notes,
                            Sap_OrderNo = item.Sap_OrderNo,
                            UniqueOrderNo = item.UniqueOrderNo,
                            DeliveryWindowInfo = await getDeliveryWindow(item, userContext),
                            BadgeNo1 = item.BadgeNo1,
                            BadgeNo2 = item.BadgeNo2,
                            BadgeNo3 = item.BadgeNo3,
                            NumOfSubDrs = item.NumOfSubDrs,
                            DispactherNote = item.DispatcherNote,
                            PickupLocationType = item.PickupLocationType,
                            Terminal = item.Terminal,
                            Bulkplant = item.Bulkplant,
                            IsAcceptNightDeliveries = item.IsAcceptNightDeliveries,
                            TrailerTypes = item.TrailerTypes,
                            IsMaxFillAllowed = item.IsMaxFillAllowed,
                            AssignedOn = DateTimeOffset.Now,
                            IsTBD = item.IsTBD,
                            TBDGroupId = item.TBDGroupId,
                            DeliveryDateStartTime = item.DeliveryDateStartTime,
                            Vessel = item.Vessel,
                            Berth = item.Berth,
                            IsMarine = item.IsMarine,
                            BlendParentProductTypeId = item.BlendParentProductTypeId,
                            SelectedDate = item.SelectedDate,
                            IsFutureDR = item.IsFutureDR,
                            IsCalendarView = item.IsCalendarView,
                            DeliveryLevelPO = item.DeliveryLevelPO,
                            ScheduleStartTime = item.ScheduleStartTime,
                            ScheduleEndTime = item.ScheduleEndTime,
                            IndicativePrice = item.IndicativePrice,
                        };

                        if (request.DeliveryRequestFor == DeliveryRequestFor.UnKnown)
                        {
                            if (!string.IsNullOrWhiteSpace(item.TankId) && !string.IsNullOrWhiteSpace(item.StorageId))
                            {
                                request.DeliveryRequestFor = DeliveryRequestFor.Tank;
                            }
                            else if (item.DeliveryRequestFor == DeliveryRequestFor.ProductType || (item.OrderId == null || item.OrderId == 0))
                            {
                                request.DeliveryRequestFor = DeliveryRequestFor.ProductType;
                            }
                            else if (item.OrderId > 0)
                            {
                                request.DeliveryRequestFor = DeliveryRequestFor.Order;
                            }
                        }
                        if (job != null && job.Id > 0)
                        {
                            request.UoM = (int)job.UoM;
                            request.JobName = job.Name;
                            request.JobAddress = job.LocationType != JobLocationTypes.Various ? job.Address : job.StateCode;
                            request.JobCity = job.LocationType != JobLocationTypes.Various ? job.City : job.StateCode;
                            request.CustomerCompany = string.IsNullOrWhiteSpace(item.CustomerCompany) ? job.CustomerCompany : item.CustomerCompany;
                            request.JobTimeZoneOffset = jobOffsetInfo.Where(t => t.Id == job.Id).Select(t => t.Offset).FirstOrDefault();
                            var regionId = Task.Run(() => GetRegionIdForJob(job.Id, request.AssignedToCompanyId)).Result;
                            request.AssignedToRegionId = regionId;
                            if (userContext.CompanyTypeId == CompanyType.Buyer)
                            {
                                request.CreatedByRegionId = request.AssignedToRegionId;
                            }
                            if (string.IsNullOrWhiteSpace(regionId))
                            {
                                deliveryRequestModel.StatusCode = Status.Failed;
                                // If carrier assigned then get the region id for the job, if job is present in that region.
                                if (autoAssignDREnable && assignedCompanyId > 0 && (request.AssignedToCompanyId != request.CreatedByCompanyId))
                                {
                                    deliveryRequestModel.StatusMessage = Resource.errorMessageLocationNotAssignedToRegionByCarrier;
                                }
                                else
                                {
                                    deliveryRequestModel.StatusMessage = Resource.valMessageJobNotAssignedToRegion;
                                }
                                return deliveryRequestModel;
                            }

                            //SetMarineNominationRelatedInfo
                            if (job.IsMarine)
                            {
                                var vessel = spResponse.Vessels.Where(v => v.OrderId == item.OrderId).FirstOrDefault();

                                if (string.IsNullOrEmpty(item.Vessel) && vessel != null && !string.IsNullOrEmpty(vessel.Name))
                                {
                                    request.Vessel = vessel.Name;
                                    request.IsMarine = true;
                                }
                                if (order != null)
                                {
                                    request.Berth = order.Berth;
                                }
                            }
                        }

                        if (!item.IsTBD)
                        {
                            if (tank != null && !string.IsNullOrWhiteSpace(tank.VehicleId) && !request.IsBlendedRequest)
                            {
                                request.ProductType = tank.Name;
                                request.ProductTypeId = tank.FuelType.Value;
                                if (order != null)
                                {
                                    request.ProductShortCode = order.ProductCode;
                                }
                            }
                            else if (item.OrderId != null)
                            {
                                //  var order = orders.FirstOrDefault(t => t.Id == item.OrderId);
                                request.ProductType = order.ProductTypeName;
                                request.ProductShortCode = order.ProductCode;
                                request.ProductTypeId = order.ProductTypeId;
                                request.FuelTypeId = order.FuelTypeId;
                                if (request.IsBlendedRequest)
                                {
                                    request.FuelType = order.FuelType;
                                }
                            }
                        }
                        else
                        {
                            request.ProductTypeId = item.ProductTypeId;
                            request.FuelTypeId = item.FuelTypeId;
                            request.FuelType = item.FuelType;
                            request.ProductType = item.ProductType;
                            request.UoM = item.UoM;
                            request.AssignedToRegionId = item.CreatedByRegionId;
                            var productCodeInfo = productCodeDetails.FirstOrDefault(x => x.Id == item.ProductTypeId);
                            if (productCodeInfo != null)
                            {
                                request.ProductShortCode = productCodeInfo.ProductCode;
                            }
                            request.DeliveryRequestFor = DeliveryRequestFor.Order;
                        }
                        if (request.RecurringSchdule != null)
                        {
                            request.RecurringSchdule.ForEach(top => top.TfxCompanyName = request.CustomerCompany);
                            if (tank != null && spResponse.Assets.Any(t => t.AssetId == tank.AssetId))
                            {
                                var tankAsset = spResponse.Assets.FirstOrDefault(t => t.AssetId == tank.AssetId);
                                if (tankAsset != null)
                                {
                                    request.RecurringSchdule.ForEach(top => top.AssetId = tankAsset.AssetId);
                                }
                            }
                        }
                        //check UOM for MFN
                        if (order != null)
                        {
                            request.UoM = (int)order.Uom; // set FR level UOM
                        }
                        if (!item.IsTBD)
                        {
                            var DefaultUom = spResponse.Jobs.FirstOrDefault(t => t.Id == request.JobId).DefaultUoM;

                            SetUomConversionForMarine(request, DefaultUom, skipMarineConversion);
                        }
                        deliveryRequestModel.DeliveryRequests.Add(request);
                        deliveryRequestModel.StatusCode = Status.Success;
                    }
                }

            }
            catch (Exception ex)
            {
                deliveryRequestModel.StatusCode = Status.Failed;
                deliveryRequestModel.StatusMessage = Status.Failed.ToString();
                LogManager.Logger.WriteException("FreightServiceDomain", "SetDeliveryRequestsParameters", ex.Message, ex);
            }
            return deliveryRequestModel;
        }

        public async Task<int> getUoMByCompany(int companyId)
        {
            var companyCountryCode = await Context.DataContext.CompanyAddresses.Where(t => t.IsActive && t.IsDefault && t.CompanyId == companyId).Select(t => t.MstCountry.Code).FirstOrDefaultAsync();
            if (!string.IsNullOrWhiteSpace(companyCountryCode))
            {
                if (companyCountryCode == Country.CAN.ToString())
                    return (int)UoM.Litres;
                else if (companyCountryCode == Country.USA.ToString())
                    return (int)UoM.Gallons;
            }
            return 0;
        }
        private async Task<DeliveryWindowInfoModel> getDeliveryWindow(RaiseDeliveryRequestInput item, UserContext userContext)
        {
            DeliveryWindowInfoModel deliveryWindowInfoModel = null;
            if (item.isRetainInfo && !string.IsNullOrEmpty(item.WindowStartTime))
            {
                deliveryWindowInfoModel = new DeliveryWindowInfoModel();
                deliveryWindowInfoModel.RetainTime = item.RetainTime;
                deliveryWindowInfoModel.RetainDate = Convert.ToDateTime(item.RetainDate);
                deliveryWindowInfoModel.StartTime = item.WindowStartTime;
                deliveryWindowInfoModel.StartDate = Convert.ToDateTime(item.WindowStartDate);
                deliveryWindowInfoModel.EndTime = item.WindowEndTime;
                deliveryWindowInfoModel.EndDate = Convert.ToDateTime(item.WindowEndDate);
            }
            else if (item.isTankExists && !item.IsRetainButtonClick)
            {
                deliveryWindowInfoModel = await CaculateRetainInfo(item, userContext, deliveryWindowInfoModel);
            }
            return deliveryWindowInfoModel;
        }

        private static async Task<DeliveryWindowInfoModel> CaculateRetainInfo(RaiseDeliveryRequestInput item, UserContext userContext, DeliveryWindowInfoModel deliveryWindowInfoModel)
        {
            var forcastinPreferance = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(userContext, (int)ForcastingSettingLevel.Tank, item.Id);
            if (forcastinPreferance.ForcastingServiceSetting.IsEnabled)
            {
                TankRetainWindowInfo tankRetainWindowInfo = new TankRetainWindowInfo();
                IntializeRetainWindowParamVales(item, forcastinPreferance, tankRetainWindowInfo);
                var result = await ContextFactory.Current.GetDomain<SalesDomain>().CalculateTankRetainWindowInfo(tankRetainWindowInfo);
                if (result != null && !string.IsNullOrEmpty(result.RetainTime))
                {
                    deliveryWindowInfoModel = new DeliveryWindowInfoModel();
                    deliveryWindowInfoModel.RetainTime = result.RetainTime;
                    deliveryWindowInfoModel.RetainDate = Convert.ToDateTime(result.RetainDate);
                    deliveryWindowInfoModel.StartTime = result.WindowStartTime;
                    deliveryWindowInfoModel.StartDate = Convert.ToDateTime(result.WindowStartDate);
                    deliveryWindowInfoModel.EndTime = result.WindowEndTime;
                    deliveryWindowInfoModel.EndDate = Convert.ToDateTime(result.WindowEndDate);
                }
            }

            return deliveryWindowInfoModel;
        }

        private static void IntializeRetainWindowParamVales(RaiseDeliveryRequestInput item, ForcastingPreferenceViewModel forcastinPreferance, TankRetainWindowInfo tankRetainWindowInfo)
        {
            tankRetainWindowInfo.Id = item.Id;
            tankRetainWindowInfo.siteId = item.SiteId;
            tankRetainWindowInfo.tankId = item.TankId;
            tankRetainWindowInfo.storageId = item.StorageId;
            tankRetainWindowInfo.startBufferUOM = forcastinPreferance.ForcastingServiceSetting.StartBufferUOM == null ? 1 : (int)forcastinPreferance.ForcastingServiceSetting.StartBufferUOM;
            tankRetainWindowInfo.startBuffer = forcastinPreferance.ForcastingServiceSetting.StartBuffer == null ? 0 : (int)forcastinPreferance.ForcastingServiceSetting.StartBuffer;

            tankRetainWindowInfo.endBufferUOM = forcastinPreferance.ForcastingServiceSetting.EndBufferUOM == null ? 1 : (int)forcastinPreferance.ForcastingServiceSetting.EndBufferUOM;
            tankRetainWindowInfo.endBuffer = forcastinPreferance.ForcastingServiceSetting.EndBuffer == null ? 0 : (int)forcastinPreferance.ForcastingServiceSetting.EndBuffer;

            int retainBufferUOM = forcastinPreferance.ForcastingServiceSetting.RetainTimeBufferUOM == null ? 1 : (int)forcastinPreferance.ForcastingServiceSetting.RetainTimeBufferUOM;
            int retainBuffer = forcastinPreferance.ForcastingServiceSetting.RetainTimeBuffer == null ? 0 : (int)forcastinPreferance.ForcastingServiceSetting.RetainTimeBuffer;

            if (tankRetainWindowInfo.startBufferUOM == retainBufferUOM)
            {
                tankRetainWindowInfo.maxBuffer = tankRetainWindowInfo.startBuffer > retainBuffer ? tankRetainWindowInfo.startBuffer : retainBuffer;
                tankRetainWindowInfo.maxBufferUOM = tankRetainWindowInfo.startBufferUOM == (int)RateOfConsumsionUOM.Hours ? (int)RateOfConsumsionUOM.Hours : (int)RateOfConsumsionUOM.Days;
            }
            else
            {
                if (tankRetainWindowInfo.startBufferUOM == (int)RateOfConsumsionUOM.Hours && retainBufferUOM == (int)RateOfConsumsionUOM.Days)
                {
                    tankRetainWindowInfo.maxBuffer = retainBuffer;
                    tankRetainWindowInfo.maxBuffer = (int)RateOfConsumsionUOM.Days;
                }
                else if (tankRetainWindowInfo.startBufferUOM == (int)RateOfConsumsionUOM.Days && retainBufferUOM == (int)RateOfConsumsionUOM.Hours)
                {
                    tankRetainWindowInfo.maxBuffer = tankRetainWindowInfo.startBuffer;
                    tankRetainWindowInfo.maxBuffer = (int)RateOfConsumsionUOM.Days;
                }
            }
        }

        private RaiseDeliveryRequestModel GetDeliveryRequestModel(List<ApiRaiseDeliveryRequestInput> raiseDeliveryRequests, UserContext userContext)
        {
            var deliveryRequestModel = new RaiseDeliveryRequestModel();
            if (raiseDeliveryRequests != null && raiseDeliveryRequests.Count > 0)
            {
                List<int> jobIds = raiseDeliveryRequests.Select(t => t.JobId).Distinct().ToList();
                var jobs = Context.DataContext.Jobs.Where(t => jobIds.Any() && jobIds.Contains(t.Id)).Select(t => new { t.IsRetailJob, t.DisplayJobID, t.Id, t.Name, t.Address, t.City, StateCode = t.MstState.Code, t.LocationType, Customer = t.Company.Name, t.TimeZoneName }).ToList();
                var orders = new List<JobOrderDetails>();

                var nonretail = jobs.Where(t => !t.IsRetailJob).Select(t => t.Id).ToList();
                var nonRetailDrs = raiseDeliveryRequests.Where(t => nonretail.Contains(t.JobId)).Select(t => new { t.JobId, t.SupplierCompanyId, t.FuelTypeId });
                var suppliers = nonRetailDrs.Select(t => t.SupplierCompanyId).ToList();
                var productTypeIds = nonRetailDrs.Select(t => t.FuelTypeId).ToList();
                if (jobs.Any(t => !t.IsRetailJob))
                {
                    orders = Context.DataContext.Orders.Where(t => t.IsEndSupplier && suppliers.Contains(t.AcceptedCompanyId) && t.BuyerCompanyId == userContext.CompanyId && nonretail.Contains(t.FuelRequest.JobId)
                                                                  && productTypeIds.Contains(t.FuelRequest.MstProduct.ProductTypeId) && t.OrderXStatuses.Any(t1 => t1.IsActive && t1.StatusId == (int)OrderStatus.Open))
                                                        .GroupBy(t => new { t.AcceptedCompanyId, t.FuelRequest.MstProduct.ProductTypeId }).Select(t => t.OrderByDescending(t1 => t1.Id).FirstOrDefault())
                                                        .Select(t => new JobOrderDetails { Id = t.Id, PoNumber = t.PoNumber, AcceptedCompanyId = t.AcceptedCompanyId, JobId = t.FuelRequest.JobId, ProductTypeId = t.FuelRequest.MstProduct.ProductTypeId, BuyerCompanyId = t.BuyerCompanyId }).ToList();

                }
                var jobOffsetInfo = jobs.Select(t => new TimeZoneOffsetModel() { Id = t.Id, TimeZoneName = t.TimeZoneName }).ToList();
                GetOffsetForTimezones(jobOffsetInfo);
                foreach (var item in raiseDeliveryRequests)
                {
                    var job = jobs.FirstOrDefault(t => t.Id == item.JobId);
                    var request = new RaiseDeliveryRequestViewModel
                    {
                        CreatedBy = userContext.Id,
                        CreatedByCompanyId = userContext.CompanyId,
                        CreatedOn = DateTimeOffset.Now,
                        Status = DeliveryReqStatus.Pending,
                        IsActive = true,
                        IsDeleted = false,
                        AssignedToCompanyId = item.SupplierCompanyId,
                        SupplierCompanyId = item.SupplierCompanyId,
                        SiteId = job.DisplayJobID,
                        ScheduleQuantityType = item.ScheduleQuantityType,
                        RequiredQuantity = item.RequiredQuantity,
                        JobId = item.JobId,
                        ProductTypeId = item.FuelTypeId,
                        JobName = item.JobName,
                        JobAddress = job.LocationType != JobLocationTypes.Various ? job.Address + " " + job.City : job.StateCode,
                        JobCity = job.LocationType != JobLocationTypes.Various ? job.City : job.StateCode,
                        CustomerCompany = job.Customer,
                        ProductType = item.ProductType,
                        UoM = item.UoM,
                        Priority = item.Priority,
                        DelReqSource = DRSource.BuyerApp,
                        isRecurringSchedule = item.RecurringSchdules != null && item.RecurringSchdules.Any(),
                        BuyerCompanyId = item.BuyerCompanyId,
                        IsDispatchRetainedByCustomer = false,
                        DeliveryRequestFor = DeliveryRequestFor.ProductType,
                        Notes = item.Notes,
                        DeliveryWindowInfo = item.DeliveryWindowInfo,
                        JobTimeZoneOffset = jobOffsetInfo.Where(t => t.Id == job.Id).Select(t => t.Offset).FirstOrDefault(),
                        DeliveryLevelPO = item.DeliveryLevelPO,
                    };
                    if (request.isRecurringSchedule)
                    {
                        request.RecurringSchdule = item.RecurringSchdules.Select(t => new RecurringSchdule()
                        {
                            Id = string.IsNullOrWhiteSpace(t.Id) ? null : t.Id,
                            ScheduleType = t.ScheduleType,
                            ScheduleQuantityType = t.ScheduleQuantityType,
                            WeekDayId = t.WeekDayId,
                            Date = t.Date,
                            TfxBuyerCompanyId = job.Customer,
                            RequiredQuantity = t.RequiredQuantity,
                            DeliveryLevelPO = t.DeliveryLevelPO
                        }).ToList();
                    }
                    if (!job.IsRetailJob)
                    {
                        var order = orders.FirstOrDefault(t => t.JobId == job.Id && t.AcceptedCompanyId == request.SupplierCompanyId && t.ProductTypeId == request.ProductTypeId && t.BuyerCompanyId == request.BuyerCompanyId);
                        if (order != null)
                        {
                            request.OrderId = order.Id;
                            request.PoNumber = order.PoNumber;
                        }
                    }
                    deliveryRequestModel.DeliveryRequests.Add(request);
                }
            }

            return deliveryRequestModel;
        }

        public async Task<StatusViewModel> CheckLocationAssignedToCarrier(int jobId, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            if (jobId <= 0)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = string.Format(Resource.valMessageInvalid, "Job");
                return response;
            }

            var carrierCompany = await Context.DataContext.JobCarrierDetails.Where(t => t.JobId == jobId && t.CarrierCompanyId != userContext.CompanyId && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive).OrderByDescending(x => x.Id)
                                                                            .Select(t => new { CompanyId = t.CarrierCompanyId, t.Company.Name })
                                                                            .FirstOrDefaultAsync();
            if (carrierCompany != null)
            {
                if (carrierCompany.CompanyId > 0 && (userContext.IsSupplier || userContext.IsSupplierAdmin))
                {
                    var obj = new { IsLocationAssignedToCarrier = true, CarrierCompanyName = carrierCompany.Name };
                    response.ResponseData = obj;

                    response.StatusCode = Status.Success;
                    response.StatusMessage = string.Format(Resource.warningMessageLocationAssignedToCompany, carrierCompany.Name);
                    return response;
                }
            }
            else
            {
                var obj = new { IsLocationAssignedToCarrier = false };
                response.ResponseData = obj;

                response.StatusCode = Status.Success;
                return response;
            }

            return response;
        }

        public async Task<StatusViewModel> CreateTankDipTest(List<DemandModel> demands, int supplierId)
        {
            var response = new StatusViewModel();
            var apiUrl = ApplicationConstants.UrlCreateTankDipTest + "?supplierId=" + supplierId;
            response = await ApiPostCall<StatusViewModel>(apiUrl, demands);

            return response;
        }

        public async Task<bool> CrateOrderTankMappings(List<OrderTankMappingViewModel> mappings)
        {
            var response = false;
            try
            {
                response = await ApiPostCall<bool>(ApplicationConstants.UrlCreateOrderTankMapping, mappings);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "CrateOrderTankMappings", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateDeliveryRequest(List<DeliveryRequestViewModel> requestModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                if (requestModel != null && requestModel.Any())
                {
                    requestModel.ForEach(t => t.CreatedByCompanyId = userContext.CompanyId);
                    response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlUpdateDeliveryRequest, requestModel);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateDeliveryRequest", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ChangeBrokeredDrStatus(string drId, BrokeredDrCarrierStatus status)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlChangeBrokeredDrStatus, drId, status);
                response = await ApiPostCall<StatusViewModel>(apiUrl, new { });
                if (response == null)
                {
                    response = new StatusViewModel();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "ChangeBrokeredDrStatus", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DeliveryRequestViewModel>> GetDeliveryRequestsByPriority(DeliveryReqPriority priority, UserContext userContext)
        {

            var response = new List<DeliveryRequestViewModel>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetDeliveryRequestsbyPriority, priority, userContext.CompanyId);
                response = await ApiGetCall<List<DeliveryRequestViewModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDeliveryRequestsByPriority", ex.Message, ex);
            }

            return response;

        }

        public DeliveryRequestFilterGridViewModel GetDeliveryRequestFilter(DeliveryReqPriority priority)
        {
            var response = new DeliveryRequestFilterGridViewModel();
            try
            {
                response.priority = priority;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDeliveryRequestFilter", ex.Message, ex);
            }
            return response;
        }
        #endregion

        public async Task<StatusViewModel> SaveTractorDetails(TractorDetailViewModel requestModel)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlSaveTractorDetails);
                response = await ApiPostCall<StatusViewModel>(apiUrl, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "SaveTractorDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateTractorDetails(TractorDetailViewModel requestModel)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlUpdateTractorDetails);
                response = await ApiPostCall<StatusViewModel>(apiUrl, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateTractorDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteTractorAsync(TractorDetailViewModel inputData)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlDeleteTractor;
                response = await ApiPostCall<StatusViewModel>(apiUrl, inputData);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "DeleteTruckAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<TractorDetailViewModel>> GetAllTractorDetails(int companyId)
        {
            var response = new List<TractorDetailViewModel>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetAllTractorDetails, companyId);
                response = await ApiGetCall<List<TractorDetailViewModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetAllTractorDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetAllDrivers(int companyId, string trailerTypeId)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetAllDrivers, companyId, trailerTypeId);
                response = await ApiGetCall<List<DropdownDisplayItem>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetAllDrivers", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DemandCaptureChartViewModel>> GetDemandCaptureChartData(string SiteId, int noOfDays, int tfxJobId, int companyId)
        {
            var response = new List<DemandCaptureChartViewModel>();
            try
            {
                if (!string.IsNullOrEmpty(SiteId) && noOfDays != 0)
                {
                    var apiUrl = string.Format(ApplicationConstants.UrlGetDemandCaptureChartData, SiteId, noOfDays, tfxJobId, companyId);
                    response = await ApiGetCall<List<DemandCaptureChartViewModel>>(apiUrl);

                }

                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDemandCaptureChartData", ex.Message, ex);
            }
            return null;
        }

        public async Task<List<List<DemandCaptureChartViewModel>>> GetDemandCaptureChartDataByTankAndSite(List<int> assetIds, string siteId, int noOfDays)
        {
            var response = new List<List<DemandCaptureChartViewModel>>();
            try
            {
                if (assetIds != null && noOfDays != 0 && siteId != null)
                {
                    var apiUrl = ApplicationConstants.UrlGetDemandCaptureChartDataByTankAndSite + "?siteId=" + siteId + "&noOfDays=" + noOfDays;
                    response = await ApiPostCall<List<List<DemandCaptureChartViewModel>>>(apiUrl, assetIds);
                }
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDemandCaptureChartData", ex.Message, ex);
            }
            return null;
        }
        public async Task<JobLocationRelatedDetailsViewModel> GetJobLocationRelatedDetails(int companyId, string jobIds, bool isBuyerCompany)
        {
            var response = new JobLocationRelatedDetailsViewModel();
            try
            {

                var inputModel = new { JobID = jobIds, CompanyID = companyId, IsBuyerCompany = isBuyerCompany };

                var apiUrl = ApplicationConstants.UrlGetJobLocationRelatedDetails;
                response = await ApiPostCall<JobLocationRelatedDetailsViewModel>(apiUrl, inputModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetJobLocationRelatedDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<JobDipChartDetails>> GetDipTestDetails(string siteId, string tankId, int noOfDays)
        {
            var response = new List<JobDipChartDetails>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetDipTestDetails, siteId, tankId, noOfDays);
                response = await ApiGetCall<List<JobDipChartDetails>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDipTestDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<JobDipChartDetails>> GetJobDemandCaptureChartData(DemandCaptureChartData demandCaptureChartViewModel)
        {
            var response = new List<JobDipChartDetails>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlJOBGetDemandCaptureChartData);
                response = await ApiPostCall<List<JobDipChartDetails>>(apiUrl, demandCaptureChartViewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDipTestDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<FreightDeliveryRequestDetail>> GetDeliveryRequestDetailsByIds(List<string> drIds)
        {
            var response = new List<FreightDeliveryRequestDetail>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetDeliveryRequestDetailsByIds;
                response = await ApiPostCall<List<FreightDeliveryRequestDetail>>(apiUrl, drIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDeliveryRequestDetailsByIds", ex.Message, ex);
            }
            return response;
        }

        public async Task<DeliveryRequestViewModel> GetDeliveryRequestDetailsById(string deliveryRequestId)
        {
            var response = new DeliveryRequestViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetDeliveryRequestDetailsById, deliveryRequestId);
                response = await ApiGetCall<DeliveryRequestViewModel>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDeliveryRequestDetailsById", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> AssignJobToRegion(JobToRegionAssignViewModel jobToUpdate)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var apiUrl = ApplicationConstants.UrlAssignJobToRegionPost;
                response = await ApiPostCall<StatusViewModel>(apiUrl, jobToUpdate);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "AssignJobToRegion", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DriverScheduleMappingViewModel>> GetShiftByDrivers(string driverList, int scheduleType)
        {
            var response = new List<DriverScheduleMappingViewModel>();
            try
            {

                var apiUrl = string.Format(ApplicationConstants.UrlGetShiftByDrivers, driverList, scheduleType);
                response = await ApiGetCall<List<DriverScheduleMappingViewModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetShiftByDrivers", ex.Message, ex);
            }
            return response;
        }

        public async Task<string> GetRegionIdForJob(int jobId, int companyId)
        {
            string regionId = string.Empty;
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetRegionIdForJob, jobId, companyId);
                regionId = await ApiGetCall<string>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetRegionIdForJob", ex.Message, ex);
            }
            return regionId;
        }

        public async Task<List<RegionsAndJobsModel>> GetJobsForAllRegions(int companyId)
        {
            var jobs = new List<RegionsAndJobsModel>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetJobsForAllRegions, companyId);
                jobs = await ApiGetCall<List<RegionsAndJobsModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetJobsForAllRegions", ex.Message, ex);
            }
            return jobs;
        }

        public async Task<StatusViewModel> AddDriverSchedule(DriverScheduleMappingViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlAddDriverSchedule;
                response = await ApiPostCall<StatusViewModel>(apiUrl, viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "AddDriverSchedule", ex.Message, ex);
            }
            return response;
        }

        public async Task<DriverScheduleUpdateViewModel> UpdateDriverSchedule(List<DriverScheduleMappingViewModel> viewModel)
        {
            var response = new DriverScheduleUpdateViewModel();
            try
            {
                if (viewModel.Any())
                {
                    var companyId = viewModel.FirstOrDefault().CompanyId;
                    var IsDsbDriverSchedule = Context.DataContext.OnboardingPreferences.Where(top => top.IsActive == true && top.CompanyId == companyId).OrderByDescending(top => top.Id).Select(x => x.IsDSBDriverSchedule).FirstOrDefault();
                    viewModel.ForEach(x =>
                    {
                        x.IsDsbDriverSchedule = IsDsbDriverSchedule;

                    });
                }
                var apiUrl = ApplicationConstants.UrlUpdateDriverSchedule;
                response = await ApiPostCall<DriverScheduleUpdateViewModel>(apiUrl, viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateDriverSchedule", ex.Message, ex);
            }
            return response;
        }

        public async Task<DriverScheduleUpdateViewModel> DeleteDriverSchedules(List<DriverScheduleMappingViewModel> driverScheduleMappingViewModels)
        {
            var response = new DriverScheduleUpdateViewModel();
            try
            {
                if (driverScheduleMappingViewModels.Any())
                {
                    var companyId = driverScheduleMappingViewModels.FirstOrDefault().CompanyId;
                    var IsDsbDriverSchedule = Context.DataContext.OnboardingPreferences.Where(top => top.IsActive == true && top.CompanyId == companyId).OrderByDescending(top => top.Id).Select(x => x.IsDSBDriverSchedule).FirstOrDefault();
                    driverScheduleMappingViewModels.ForEach(x =>
                    {
                        x.IsDsbDriverSchedule = IsDsbDriverSchedule;

                    });
                }
                var apiUrl = ApplicationConstants.UrlDeleteDriverSchedule;
                response = await ApiPostCall<DriverScheduleUpdateViewModel>(apiUrl, driverScheduleMappingViewModels);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "DeleteDriverSchedules", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> AddTrailerSchedule(TrailerScheduleViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlAddTrailerSchedule;
                response = await ApiPostCall<StatusViewModel>(apiUrl, viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "AddTrailerSchedule", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DriverObjectModel>> GetDriversByCompany(int companyId)
        {
            List<DriverObjectModel> response = new List<DriverObjectModel>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetDriverDetailsByCompany, companyId);
                response = await ApiGetCall<List<DriverObjectModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDriversByCompany", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetRegionsDdl(int companyId)
        {
            List<DropdownDisplayExtendedItem> response = new List<DropdownDisplayExtendedItem>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetRegionsDdl, companyId);
                response = await ApiGetCall<List<DropdownDisplayExtendedItem>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetRegionsDdl", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetCarriersAssignedToRegion(string regionId)
        {
            List<DropdownDisplayExtendedItem> response = new List<DropdownDisplayExtendedItem>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetCarriersAssignedToRegion, regionId);
                response = await ApiGetCall<List<DropdownDisplayExtendedItem>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetCarriersAssignedToRegion", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetDispatchersAssignedToRegion(List<string> regionId)
        {
            List<DropdownDisplayExtendedItem> response = new List<DropdownDisplayExtendedItem>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetDispatchersAssignedToRegion, regionId);
                response = await ApiPostCall<List<DropdownDisplayExtendedItem>>(apiUrl, regionId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDispatchersAssignedToRegion", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<int>> GetJobsAssignedToRegions(List<DropdownDisplayExtendedItem> jobs, int companyId)
        {
            var jobList = new List<int>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetJobsAssignedToRegions, companyId);
                jobList = await ApiGetCall<List<int>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetJobsAssignedToRegions", ex.Message, ex);
            }
            return jobList;

        }
        public async Task<StatusViewModel> UpdateTankSequence(TankDetailViewModel requestModel)
        {
            var response = new StatusViewModel();
            try
            {

                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlUpdateTankSequence, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateTankSequence", ex.Message, ex);
            }
            return response;
        }
        public async Task<bool> CheckDuplicateTankSequence(TankDetailViewModel requestModel)
        {
            var response = true;
            try
            {
                response = await ApiPostCall<bool>(ApplicationConstants.UrlCheckDuplicateTankSequence, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "CheckDuplicateTankSequence", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<RegionJobsModel>> GetRegionsForJobs(int companyId, string regionIds = "")
        {
            var response = new List<RegionJobsModel>();
            try
            {
                var input = new { CompanyId = companyId, RegionIds = regionIds };
                var apiUrl = string.Format(ApplicationConstants.UrlGetRegionsForJobs);
                response = await ApiPostCall<List<RegionJobsModel>>(apiUrl, input);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetRegionsForJobs", ex.Message, ex);
            }
            return response;
        }

        public async Task<JobAdditionalDetailsForSummary> GetJobSummary(int companyId, List<int> jobIds)
        {
            var response = new JobAdditionalDetailsForSummary();
            try
            {
                var input = new { CompanyId = companyId, JobIds = jobIds };
                response = await ApiPostCall<JobAdditionalDetailsForSummary>(ApplicationConstants.UrlGetJobDetailsForSupplier, input);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetJobSummary", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayExtended>> GetAssignedCarriers(int companyId, int carrierCompanyId = 0)
        {
            var response = new List<DropdownDisplayExtended>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlSupplierCarriersGet, companyId, carrierCompanyId);
                var carriers = await ApiGetCall<List<SupplierCarrierViewModel>>(apiUrl);
                response = carriers.Select(t => new DropdownDisplayExtended() { Id = t.Carrier?.Id.ToString(), Name = t.Carrier.Name }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetAssignedCarriers", ex.Message, ex);
            }
            return response;
        }


        public async Task<StatusViewModel> AssignCarrierToJob(SupplierCarrierViewModel carrier)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlAssignCarrierToJob;
                response = await ApiPostCall<StatusViewModel>(apiUrl, carrier);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "AssignedCarrierToJob", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<int>> GetCarriersJobs(int carrierCompanyId, int customerCompanyId)
        {
            var response = new List<int>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetCarriersJobs, carrierCompanyId, customerCompanyId);
                response = await ApiGetCall<List<int>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetCarriersJobs", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DipTestRequestModel>> GetCarrierDetailsByJob(List<int> jobIds)
        {
            var response = new List<DipTestRequestModel>();
            try
            {
                response = await ApiPostCall<List<DipTestRequestModel>>(ApplicationConstants.UrlGetCarrierDetailsByJob, jobIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetCarrierDetailsByJob", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> DeleteRecurringSchedule(string Id, int userId)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlDeleteRecurringSchedule, Id, userId);
                response = await ApiGetCall<StatusViewModel>(apiUrl);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "DeleteRecurringSchedule", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> CreateRouteInfo(RouteInformationModel routeInfo)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlRouteInfoCreate;
                response = await ApiPostCall<StatusViewModel>(apiUrl, routeInfo);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "CreateRouteInfo", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> UpdateRouteInfo(RouteInformationModel routeInfo)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlRouteInfoUpdate;
                response = await ApiPostCall<StatusViewModel>(apiUrl, routeInfo);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "CreateRouteInfo", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> DeleteRouteInfo(string RouteId, string RegionId, int CreatedBy)
        {
            RouteInformationModel routeInformationModel = new RouteInformationModel();
            routeInformationModel.Id = RouteId;
            routeInformationModel.RegionId = RegionId;
            routeInformationModel.CreatedBy = CreatedBy;
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlRouteInfoDelete;
                response = await ApiPostCall<StatusViewModel>(apiUrl, routeInformationModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "DeleteRouteInfo", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> GetRegionLocationDetails(int companyId, string regionId)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetRouteLocationDetails, companyId, regionId);
                var data = await ApiGetCall<List<DropdownDisplayExtended>>(apiUrl);
                response.StatusCode = Status.Success;
                response.ResponseData = data;
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.ResponseData = null;
                LogManager.Logger.WriteException("FreightServiceDomain", "GetRegionLocationDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> GetRouteLocationDetails(string Id, string regionId)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetRouteEditLocationDetails, Id, regionId);
                var data = await ApiGetCall<List<DropdownDisplayExtended>>(apiUrl);
                response.StatusCode = Status.Success;
                response.ResponseData = data;
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.ResponseData = null;
                LogManager.Logger.WriteException("FreightServiceDomain", "GetRegionLocationDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> GetRouteInfoDetails(int companyId, string regionId)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetRouteInfoDetails, companyId, regionId);
                var data = await ApiGetCall<List<RouteInformationModel>>(apiUrl);
                response.StatusCode = Status.Success;
                response.ResponseData = data;

            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.ResponseData = null;
                LogManager.Logger.WriteException("FreightServiceDomain", "GetRouteInfoDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<RouteCustomerLocationModel>> GetRouteInfoDetails(List<string> regionId)
        {
            var response = new List<RouteCustomerLocationModel>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetRouteInfoDetailsForCustomerLocation;
                response = await ApiPostCall<List<RouteCustomerLocationModel>>(apiUrl, regionId);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetRouteInfoDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> AssignJobToRoute(JobToRegionAssignViewModel jobToUpdate)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var apiUrl = ApplicationConstants.UrlAssignJobToRoutePost;
                response = await ApiPostCall<StatusViewModel>(apiUrl, jobToUpdate);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "AssignJobToRoute", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> UpdateShiftInfo(RouteInformationModel routeInfo)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlRouteUpdateShiftInfo;
                response = await ApiPostCall<StatusViewModel>(apiUrl, routeInfo);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateShiftInfo", ex.Message, ex);
            }
            return response;
        }
        public async Task<string> GetRouteIdForJob(int jobId, int companyId, string regionId)
        {
            string routeId = string.Empty;
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetRouteIdForJob, jobId, companyId, regionId);
                routeId = await ApiGetCall<string>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetRouteIdForJob", ex.Message, ex);
            }
            return routeId;
        }
        public async Task<List<RecurringSchedulesDetails>> GetRecurringScheduleDetails(string dayOfWeek, int currentDay, string date)
        {
            var response = new List<RecurringSchedulesDetails>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetRecurringScheduleDetails, dayOfWeek, currentDay, date);
                response = await ApiGetCall<List<RecurringSchedulesDetails>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetRecurringScheduleDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DeliveryRequestViewModel>> CreateDeliveryRequest(List<DeliveryRequestViewModel> deliveryRequestModels)
        {
            var response = new List<DeliveryRequestViewModel>();
            try
            {
                var apiUrl = ApplicationConstants.UrlCreateDRForRecurring;
                response = await ApiPostCall<List<DeliveryRequestViewModel>>(apiUrl, deliveryRequestModels);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "CreateDeliveryRequest", ex.Message, ex);
            }
            return response;
        }

        public async Task<ChildDeliveryRequestInfoViewModel> GetChildDeliveryRequestInfo(string drId)
        {
            var response = new ChildDeliveryRequestInfoViewModel();
            var apiUrl = string.Format(ApplicationConstants.UrlGetChildDeliveryRequestInfo, drId);
            var apiResult = await ApiGetCall<ChildDeliveryRequestInfoViewModel>(apiUrl);
            if (apiResult != null)
            {
                response = apiResult;
            }
            return response;
        }

        public async Task<StatusViewModel> RecallDeliveryRequest(string parentDrId, string childDrId, int tfxUserId)
        {
            var response = new StatusViewModel();
            var apiUrl = string.Format(ApplicationConstants.UrlRecallDeliveryRequest, parentDrId, childDrId, tfxUserId);
            var apiResult = await ApiPostCall<StatusViewModel>(apiUrl, new { });
            if (apiResult != null)
            {
                response = apiResult;
            }
            return response;
        }
        public async Task<List<TrailerCompartmentDetail>> GetTrailerCompartmentDetails(List<string> Id)
        {

            var response = new List<TrailerCompartmentDetail>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetTrailerCompartmentDetails;
                response = await ApiPostCall<List<TrailerCompartmentDetail>>(apiUrl, Id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetTrailerCompartmentDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<TrailerFuelRetainViewModel>> GetTrailerFuelRetainDetails(List<string> Id)
        {

            var response = new List<TrailerFuelRetainViewModel>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetTrailerFuelRetainInfo;
                response = await ApiPostCall<List<TrailerFuelRetainViewModel>>(apiUrl, Id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetTrailerFuelRetainDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<OttoTripModel>> GetOttoScheduleDetails(int companyId, string regionId, string shiftStartTime, string shiftEndTime, string date)
        {

            var response = new List<OttoTripModel>();
            try
            {
                var url = string.Format(ApplicationConstants.UrlGetOttoScheduleDetails, companyId, regionId, shiftStartTime, shiftEndTime, date);
                response = await ApiGetCall<List<OttoTripModel>>(url);

                if (response != null)
                {
                    await new ScheduleBuilderDomain(this).SetProductSequenceToDelieveryRequests(response.SelectMany(r => r.DeliveryRequests).ToList(), companyId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetOttoScheduleDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ShiftViewModel>> GetShifts(int companyId, string regionId)
        {
            var response = new List<ShiftViewModel>();
            try
            {
                var url = string.Format(ApplicationConstants.UrlGetShifts, companyId, regionId);
                response = await ApiGetCall<List<ShiftViewModel>>(url);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetShifts", ex.Message, ex);
            }
            return response;
        }

        #region privateMethod
        private async Task<List<CustomerJobForCarrierViewModel>> GetCarrierJobs(UserContext userContext, List<CustomerJobForCarrierViewModel> carrierJobs, List<CustomerJobForCarrierViewModel> regionJobs)
        {
            if (regionJobs != null && regionJobs.Any())
            {
                var jobList = regionJobs.Where(t => t.Job != null).Select(t => t.Job.Id).ToList();
                carrierJobs = await Context.DataContext.Orders
                                        .Where(t => t.AcceptedCompanyId == userContext.CompanyId
                                        && jobList.Contains(t.FuelRequest.JobId)
                                        && t.IsActive)
                                        .Select(x => new CustomerJobForCarrierViewModel
                                        {
                                            CompanyName = x.BuyerCompany.Name,
                                            CompanyId = x.BuyerCompanyId,
                                            Job = new JobRegionModel
                                            {
                                                Id = x.FuelRequest.JobId,
                                                Name = x.FuelRequest.Job.Name,
                                                Code = x.FuelRequest.Job.DisplayJobID,
                                                DisplayName = x.FuelRequest.Job.Name + "- " + x.FuelRequest.Job.Address + ", " + x.FuelRequest.Job.City + ((x.FuelRequest.Job.IsMarine == true) ? " (" + x.FuelRequest.Job.Company.Name + " )" : "")
                                            }
                                        })
                                        .Distinct()
                                        .ToListAsync();

                carrierJobs.ForEach(t => t.Job.Name = !string.IsNullOrWhiteSpace(t.Job.Code) ? $"{t.Job.Name}-{t.Job.Code}" : t.Job.Name);
                foreach (var customer in carrierJobs)
                {
                    customer.Job.RegionId = customer.RegionId = regionJobs.Where(t => customer.Job.Id == t.Job.Id).Select(t => t.RegionId).FirstOrDefault();
                }
            }

            return carrierJobs;
        }
        private static void MergeBuyerAndSupplierJobs(out List<CustomerJobsForCarrierViewModel> result, List<CustomerJobForCarrierViewModel> buyerJobs, List<CustomerJobForCarrierViewModel> carrierJobs, out List<CustomerJobForCarrierViewModel> jobUnion)
        {
            if (carrierJobs == null || !carrierJobs.Any())
            {
                jobUnion = buyerJobs.Distinct().OrderBy(t => t.Job.Name).ToList();
            }
            else
            {
                jobUnion = buyerJobs.Union(carrierJobs).Distinct().OrderBy(t => t.Job.Name).ToList();
            }

            result = jobUnion.GroupBy(x => x.CompanyId)
                                            .Select(y => new CustomerJobsForCarrierViewModel
                                            {
                                                CompanyId = y.First().CompanyId,
                                                CompanyName = y.First().CompanyName,
                                                RegionIds = y.Select(t => t.RegionId).Distinct().ToList(),
                                                Jobs = y.Select(y1 => y1.Job).OrderBy(y1 => y1.Name).ToList()
                                            }).OrderBy(t => t.CompanyName).ToList();
        }
        #endregion

        public async Task<List<ExternalVehicleMappingViewModel>> GetVehiclesForExternalMapping(int companyId)
        {
            var response = new List<ExternalVehicleMappingViewModel>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetVehiclesForExternalMapping, companyId);
                response = await ApiGetCall<List<ExternalVehicleMappingViewModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetVechiclesForExternalMapping", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> SaveExternalVehicleMapping(ExternalVehicleMappingViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlSaveExternalVehicleMapping;
                response = await ApiPostCall<StatusViewModel>(apiUrl, viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "SaveExternalVehicleMapping", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> CreateSplitDeliveryRequests(SplitDeliveryRequestModel splitDeliveryRequestModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                if (splitDeliveryRequestModel != null)
                {
                    var freightServiceDomain = new FreightServiceDomain(this);
                    var deliveryRequestDetails = await freightServiceDomain.GetDeliveryRequestDetailsById(splitDeliveryRequestModel.ParentDRId);
                    if (deliveryRequestDetails != null)
                    {
                        splitDeliveryRequestModel.RequiredQtyDetails.ForEach(x =>
                        {
                            x.UniqueOrderNo = GenerateUniqueDRId(deliveryRequestDetails, userContext);
                        });
                    }
                    //post the delivery request which has isRecurringSchedule=false
                    splitDeliveryRequestModel.UserId = userContext.Id;
                    response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlRaiseSplitDeliveryRequests, splitDeliveryRequestModel);

                    var notificationDomain = new NotificationDomain(this);
                    foreach (var item in response.EntityIds)
                    {
                        var message = new TankDeliveryRequestMessageViewModel { EntityId = item };
                        var jsonMessage = new JavaScriptSerializer().Serialize(message);
                        await notificationDomain.AddNotificationEventAsync(EventType.TankDeliveryRequestCreated, 0, userContext.Id, null, jsonMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "CreateSplitDeliveryRequests", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CreateSplitBlendDeliveryRequests(List<SplitDrArray> splitDrArray, UserContext userContext)
        {
            var response = new StatusViewModel();
            var notificationDomain = new NotificationDomain(this);
            try
            {
                //post the delivery request which has isRecurringSchedule=false
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlCreateSplitBlendDeliveryRequests, splitDrArray);
                foreach (var item in response.EntityIds)
                {
                    var message = new TankDeliveryRequestMessageViewModel { EntityId = item };
                    var jsonMessage = new JavaScriptSerializer().Serialize(message);
                    await notificationDomain.AddNotificationEventAsync(EventType.TankDeliveryRequestCreated, 0, userContext.Id, null, jsonMessage);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "CreateSplitDeliveryRequests", ex.Message, ex);
            }
            return response;
        }

        public void SetUomConversionForMarine(RaiseDeliveryRequestViewModel request, UoM defaultUom, bool skipMarineConversion)
        {
            if (!skipMarineConversion)
            {
                if (defaultUom == UoM.Gallons && request.UoM == (int)UoM.MetricTons)
                {
                    request.RequiredQuantity = request.RequiredQuantity * Convert.ToDecimal(ApplicationConstants.MetricTonsToGallons);
                    request.UoM = (int)UoM.Gallons;
                }
                else if (defaultUom == UoM.Litres && request.UoM == (int)UoM.MetricTons)
                {
                    request.RequiredQuantity = request.RequiredQuantity * Convert.ToDecimal(ApplicationConstants.MetricTonsToLiters);
                    request.UoM = (int)UoM.Litres;
                }
                else if (defaultUom == UoM.Gallons && request.UoM == (int)UoM.Barrels)
                {
                    request.RequiredQuantity = request.RequiredQuantity * Convert.ToDecimal(ApplicationConstants.BarrelToGallons);
                    request.UoM = (int)UoM.Gallons;
                }
                else if (defaultUom == UoM.Litres && request.UoM == (int)UoM.Barrels)
                {
                    request.RequiredQuantity = request.RequiredQuantity * Convert.ToDecimal(ApplicationConstants.BarrelToLiters);
                    request.UoM = (int)UoM.Litres;
                }
            }
            else
            {
                request.UoM = (int)defaultUom;
            }

        }

        private void SetUomConversionForMarineFromBuyer(ApiRaiseDeliveryRequestInput request, UoM defaultUom)
        {

            if (defaultUom == UoM.Gallons && request.UoM == (int)UoM.MetricTons)
            {
                request.RequiredQuantity = request.RequiredQuantity * Convert.ToDecimal(ApplicationConstants.MetricTonsToGallons);
                request.UoM = (int)UoM.Gallons;
            }
            else if (defaultUom == UoM.Litres && request.UoM == (int)UoM.MetricTons)
            {
                request.RequiredQuantity = request.RequiredQuantity * Convert.ToDecimal(ApplicationConstants.MetricTonsToLiters);
                request.UoM = (int)UoM.Litres;
            }
            else if (defaultUom == UoM.Gallons && request.UoM == (int)UoM.Barrels)
            {
                request.RequiredQuantity = request.RequiredQuantity * Convert.ToDecimal(ApplicationConstants.BarrelToGallons);
                request.UoM = (int)UoM.Gallons;
            }
            else if (defaultUom == UoM.Litres && request.UoM == (int)UoM.Barrels)
            {
                request.RequiredQuantity = request.RequiredQuantity * Convert.ToDecimal(ApplicationConstants.BarrelToLiters);
                request.UoM = (int)UoM.Litres;
            }
        }
        public async Task<StatusViewModel> CreateDeliveryRequestsFromBuyer(CreateDeliveryRequestModel job, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                UoM DefaultUom = Context.DataContext.Jobs.Where(t => t.Id == job.JobId).Select(d => d.MstCountry).FirstOrDefault().DefaultUoM;
                if (job != null && job.ProductTypes != null && job.ProductTypes.Any())
                {
                    List<ApiRaiseDeliveryRequestInput> requests = new List<ApiRaiseDeliveryRequestInput>();
                    foreach (var product in job.ProductTypes)
                    {
                        if ((product.isRecurringSchedule && product.RecurringSchdules.Any()) || (product.ScheduleQuantityType != 1 || product.RequiredQuantity > 0))
                        {
                            ApiRaiseDeliveryRequestInput input = new ApiRaiseDeliveryRequestInput()
                            {
                                BuyerCompanyId = userContext.CompanyId,
                                FuelTypeId = product.FuelTypeId,
                                JobName = job.JobName,
                                JobId = job.JobId,
                                isRecurringSchedule = product.isRecurringSchedule && product.RecurringSchdules != null && product.RecurringSchdules.Any(),
                                Priority = product.Priority,
                                ProductType = product.ProductType,
                                RequiredQuantity = product.RequiredQuantity,
                                ScheduleQuantityType = product.ScheduleQuantityType,
                                SupplierCompanyId = product.SupplierCompanyId,
                                UoM = product.UoM,
                                UserId = userContext.Id,
                                RecurringSchdules = product.RecurringSchdules,
                                Notes = product.Notes,
                                DeliveryWindowInfo = getDeliveryWindow(product),
                                OrderId = product.OrderId,
                                DeliveryLevelPO = product.DeliveryLevelPO,
                            };

                            SetUomConversionForMarineFromBuyer(input, DefaultUom);
                            requests.Add(input);
                        }
                    }
                    if (requests.Any())
                    {
                        response = await RaiseDeliveryRequestsFromBuyerApp(requests, userContext);
                    }
                }
                else
                {
                    response = new StatusViewModel { StatusCode = Status.Warning, StatusMessage = Resource.warningNoProductsForSelectedJob };
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "CreateDeliveryRequestsFromBuyer", ex.Message, ex);
            }
            return response;
        }

        private DeliveryWindowInfoModel getDeliveryWindow(JobProductTypeDetails item)
        {
            DeliveryWindowInfoModel deliveryWindowInfoModel = null;
            if (!string.IsNullOrEmpty(item.RetainTime) && !string.IsNullOrEmpty(item.RetainDate))
            {
                deliveryWindowInfoModel = new DeliveryWindowInfoModel();
                deliveryWindowInfoModel.RetainTime = item.RetainTime;
                deliveryWindowInfoModel.RetainDate = Convert.ToDateTime(item.RetainDate);
                deliveryWindowInfoModel.StartTime = item.StartTime;
                deliveryWindowInfoModel.StartDate = Convert.ToDateTime(item.StartDate);
                deliveryWindowInfoModel.EndTime = item.EndTime;
                deliveryWindowInfoModel.EndDate = Convert.ToDateTime(item.EndDate);
            }
            return deliveryWindowInfoModel;
        }
        //public async Task SetUomConversionForMarineForBuyer(ApiRaiseDeliveryRequestInput request)
        //{
        //    if (order != null)
        //        request.UoM = (int)order.Uom; // set FR level UOM
        //    UoM DefaultUom = jobs.FirstOrDefault(t => t.Id == request.JobId).DefaultUom;

        //    if (defaultUom == UoM.Gallons && request.UoM == (int)UoM.MetricTons)
        //    {
        //        request.RequiredQuantity = request.RequiredQuantity * Convert.ToDecimal(ApplicationConstants.MetricTonsToGallons);
        //        request.UoM = (int)UoM.Gallons;
        //    }
        //    else if (defaultUom == UoM.Litres && request.UoM == (int)UoM.MetricTons)
        //    {
        //        request.RequiredQuantity = request.RequiredQuantity * Convert.ToDecimal(ApplicationConstants.MetricTonsToLiters);
        //        request.UoM = (int)UoM.Litres;
        //    }
        //    else if (defaultUom == UoM.Gallons && request.UoM == (int)UoM.Barrels)
        //    {
        //        request.RequiredQuantity = request.RequiredQuantity * Convert.ToDecimal(ApplicationConstants.BarrelToGallons);
        //        request.UoM = (int)UoM.Gallons;
        //    }
        //    else if (defaultUom == UoM.Litres && request.UoM == (int)UoM.Barrels)
        //    {
        //        request.RequiredQuantity = request.RequiredQuantity * Convert.ToDecimal(ApplicationConstants.BarrelToLiters);
        //        request.UoM = (int)UoM.Litres;
        //    }
        //}      

        public async Task<StatusViewModel> SaveBulkUploadVehicleMapping(List<ExternalVehicleMappingViewModel> listExternalVehicles, int userId)
        {
            var response = new StatusViewModel();
            try
            {
                var input = new { UserId = userId, ListExternalVehicles = listExternalVehicles };
                var apiUrl = ApplicationConstants.UrlSaveBulkUploadVehicleMapping;
                response = await ApiPostCall<StatusViewModel>(apiUrl, input);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "SaveBulkUploadVehicleMapping", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<JobDRDetailsModel>> GetJobDRPrioritiesForBuyer(JobDRPriorityInputModel request)
        {
            var response = new List<JobDRDetailsModel>();
            try
            {
                //var inputModel = new { JobID = jobIds, CompanyID = companyId };
                //var apiUrl = string.Format(ApplicationConstants.UrlGetJobDRPrioritiesForBuyer, companyId, jobIds);
                //response = await ApiGetCall<List<JobDRDetailsModel>>(apiUrl);
                var apiUrl = ApplicationConstants.UrlGetJobDRPrioritiesForBuyer;
                response = await ApiPostCall<List<JobDRDetailsModel>>(apiUrl, request);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetJobDRPrioritiesForBuyer", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> UpdateDistanceCovered(int jobId, string DistanceCovered)
        {
            var response = new StatusViewModel { StatusCode = Status.Success, StatusMessage = Resource.SuccessDistanceCoveredUpdated };
            try
            {
                Match match = Regex.Match(DistanceCovered, @"^[0-9:]*$");
                if (!match.Success || string.IsNullOrEmpty(DistanceCovered))
                {
                    response = new StatusViewModel { StatusCode = Status.Warning, StatusMessage = Resource.warningValidationDistanceCovered };
                    return response;
                }

                var jobAdditionalDetailsModel = new { JobId = jobId, DistanceCovered = DistanceCovered };
                var apiUrl = ApplicationConstants.UrlUpdateDistanceCoveredOfAdditionalJobDetails;
                if (!await ApiPostCall<bool>(apiUrl, jobAdditionalDetailsModel))
                {
                    response = new StatusViewModel { StatusCode = Status.Failed, StatusMessage = Resource.ErrDistanceCoveredUpdated };
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateDistanceCovered", ex.Message, ex);
                response = new StatusViewModel { StatusCode = Status.Failed, StatusMessage = Resource.ErrDistanceCoveredUpdated };
            }
            return response;
        }
        public async Task<StatusViewModel> CreateLoadQueue(List<DSBLoadQueueModel> inputData, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                inputData.ForEach(x => { x.TfxUserId = userContext.Id; x.TfxCompanyId = userContext.CompanyId; });
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlCreateDsbLoadQueue, inputData);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "CreateLoadQueue", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> DeleteLoadQueue(List<string> inputData)
        {
            var response = new StatusViewModel();
            try
            {
                if (inputData != null && inputData.Any())
                {
                    response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlDeleteDsbLoadQueue, inputData);
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorNoDsbLoadQueue;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "DeleteLoadQueue", ex.Message, ex);
            }
            return response;
        }


        public async Task<StatusViewModel> AddRegionSchedule(RegionScheduleViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlAddRegionSchedule;
                response = await ApiPostCall<StatusViewModel>(apiUrl, viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "AddRouteSchedule", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<RegionScheduleViewModel>> GetResionShiftSchedulesDetails(string regionId, string routeId)
        {
            var response = new List<RegionScheduleViewModel>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetRegionShiftSchedule, regionId, routeId);
                response = await ApiGetCall<List<RegionScheduleViewModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetResionShiftSchedulesDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<DRReportFilterViewModel> GetDRReportDropDownFilters(int userId)
        {
            var response = new DRReportFilterViewModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetDRReportFilterData, userId);
                response = await ApiGetCall<DRReportFilterViewModel>(apiUrl);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDRReportDropDownFilters", ex.Message, ex);
            }
            return response;

        }
        public async Task<List<DeliveryRequestReportGridViewModel>> GetAllDeliveryRequests(DRReportFilterInputViewModel inputData, UserContext userContext)
        {
            var response = new List<DeliveryRequestReportGridViewModel>();
            try
            {

                var apiUrl = ApplicationConstants.UrlGetAllDeliveryRequestsForCompany;
                inputData.CompanyId = userContext.CompanyId;
                response = await ApiPostCall<List<DeliveryRequestReportGridViewModel>>(apiUrl, inputData);
                var orderIds = response.Where(t => t.OrderId > 0).Select(t => t.OrderId).ToList();

                var orders = await Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == userContext.CompanyId && orderIds.Contains(t.Id))
                                                            .Select(t => new
                                                            {
                                                                t.Id,
                                                                t.PoNumber,
                                                                t.TfxPoNumber,

                                                            }).ToListAsync();
                var jobIds = response.Select(t => t.TfxJobId).Distinct().ToList();
                var customerBrandIds = await new StoredProcedureDomain(this).GetCustomerBrandIdsByJobs(jobIds, userContext.CompanyId);
                foreach (var dr in response)
                {
                    var order = orders.FirstOrDefault(t => t.Id == dr.OrderId);
                    var customerBrandId = customerBrandIds.Where(t => t.JobId == dr.TfxJobId).FirstOrDefault();
                    if (customerBrandId != null)
                    {
                        dr.CustomerBrandID = string.IsNullOrWhiteSpace(customerBrandId.CustomerBrandID) ? Resource.lblHyphen : customerBrandId.CustomerBrandID;
                    }

                    if (order != null)
                    {
                        dr.PoNumber = string.IsNullOrWhiteSpace(order.PoNumber) ? order.TfxPoNumber : order.PoNumber;
                    }

                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetAllDeliveryRequests", ex.Message, ex);
            }
            return response;

        }

        public async Task<List<RegionScheduleMapping>> GetRegionShiftScheduleByRegionId(string regionId, int scheduleType)
        {
            var response = new List<RegionScheduleMapping>();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetRegionShiftScheduleByRegionId, regionId, scheduleType);
                response = await ApiGetCall<List<RegionScheduleMapping>>(apiUrl);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDRReportDropDownFilters", ex.Message, ex);
            }
            return response;

        }

        public async Task<DrFilterPreferencesModel> SaveDrFilterPreferences(DrFilterPreferencesModel model)
        {
            var response = new DrFilterPreferencesModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlSaveDrFilterPreferences;
                response = await ApiPostCall<DrFilterPreferencesModel>(apiUrl, model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "SaveDrFilterPreferences", ex.Message, ex);
            }
            return response;
        }
        public async Task<DrFilterPreferencesModel> GetDrFilterPreferences(int userId)
        {
            var response = new DrFilterPreferencesModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetDrFilterPreferences, userId);
                response = await ApiGetCall<DrFilterPreferencesModel>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDrFilterPreferences", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ResetFuelRetainDetails(TruckDetailViewModel truckDetailViewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                if (truckDetailViewModel.TruckId != null && truckDetailViewModel.TruckId != "")
                {
                    truckDetailViewModel.FuelResetUserId = userContext.Id.ToString();
                    truckDetailViewModel.FuelResetUserName = userContext.UserName;
                    response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlResetFuelRetainDetails, truckDetailViewModel);
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorInvalidTruckId;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "ResetFuelRetainDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateRetainFuelDetails(TruckDetailViewModel truckDetailViewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                if (truckDetailViewModel != null && truckDetailViewModel.TrailerFuelRetains.Count > 0)
                {
                    truckDetailViewModel.FuelResetUserId = userContext.Id.ToString();
                    truckDetailViewModel.FuelResetUserName = userContext.UserName;
                    response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlUpdateRetainFuelDetails, truckDetailViewModel);
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errInvalidInputRequest;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateRetainFuelDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ConfirmRetainFuelException(TruckDetailViewModel truckDetailViewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                if (truckDetailViewModel != null && truckDetailViewModel.TrailerFuelRetains.Count > 0)
                {
                    truckDetailViewModel.FuelResetUserId = userContext.Id.ToString();
                    truckDetailViewModel.FuelResetUserName = userContext.UserName;
                    response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlConfirmRetainFuelException, truckDetailViewModel);
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errInvalidInputRequest;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "ConfirmRetainFuelException", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<ShiftViewModel>> GetDriversShifts(int companyId, string regionId, string SelectedDate)
        {
            var response = new List<ShiftViewModel>();
            try
            {
                var IsDsbDriverSchedule = Context.DataContext.OnboardingPreferences.Where(top => top.IsActive == true && top.CompanyId == companyId).OrderByDescending(top => top.Id).Select(x => x.IsDSBDriverSchedule).FirstOrDefault();
                var url = string.Format(ApplicationConstants.UrlGetDriversShifts, companyId, regionId, SelectedDate, IsDsbDriverSchedule);
                response = await ApiGetCall<List<ShiftViewModel>>(url);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDriversShifts", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<CarrierRegionModel>> GetAllCarrierRegions(List<DropdownDisplayItem> Carriers)
        {
            var response = new List<CarrierRegionModel>();
            try
            {
                response = await ApiPostCall<List<CarrierRegionModel>>(ApplicationConstants.UrlGetAllCarrierRegions, Carriers);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UrlGetAllCarrierRegions", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> SaveDRCarrierSequence(List<DRCarrierSequenceModel> model)
        {
            var response = new StatusViewModel();
            try
            {
                if (model != null)
                {
                    var apiUrl = ApplicationConstants.UrlSaveDRCarrierSequence;
                    response = await ApiPostCall<StatusViewModel>(apiUrl, model);
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errInvalidInputRequest;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "SaveDRCarrierSequence", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> UpdateDRCarrierSequence(List<DRCarrierSequenceModel> model)
        {
            var response = new StatusViewModel();
            try
            {
                if (model != null)
                {
                    var apiUrl = ApplicationConstants.UrlUpdateDRCarrierSequence;
                    response = await ApiPostCall<StatusViewModel>(apiUrl, model);
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errInvalidInputRequest;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "SaveDRCarrierSequence", ex.Message, ex);
            }
            return response;
        }
        public async Task<DRCarrierSequenceModel> GetDRCarrierSequenceDetails(string deliveryReqId)
        {
            var response = new DRCarrierSequenceModel();
            try
            {
                if (!string.IsNullOrEmpty(deliveryReqId))
                {
                    var url = string.Format(ApplicationConstants.UrlGetDRCarrierSequence, deliveryReqId);
                    response = await ApiGetCall<DRCarrierSequenceModel>(url);
                }
            }
            catch (Exception ex)
            {
                response = null;
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDRCarrierSequenceDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> UpdateDRCarrierRejectList(DRCarrierRejectInfoModel model)
        {
            var response = new StatusViewModel();
            try
            {
                if (model != null)
                {
                    var apiUrl = ApplicationConstants.UrlUpdateDRCarrierRejectList;
                    response = await ApiPostCall<StatusViewModel>(apiUrl, model);
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errInvalidInputRequest;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateDRCarrierRejectList", ex.Message, ex);
            }
            return response;
        }
        public async Task<DRAvailableCarrierSequenceModel> GetAvailableDRCarrierDetails(string deliveryReqId)
        {
            var response = new DRAvailableCarrierSequenceModel();
            try
            {
                if (!string.IsNullOrEmpty(deliveryReqId))
                {
                    var url = string.Format(ApplicationConstants.UrlGetAvailableDRCarrierDetails, deliveryReqId);
                    response = await ApiGetCall<DRAvailableCarrierSequenceModel>(url);
                }
            }
            catch (Exception ex)
            {
                response = null;
                LogManager.Logger.WriteException("FreightServiceDomain", "GetAvailableDRCarrierDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> UpdateBrokerDeliveryRequestInfo(BrokeredDeliveryRequestInput model)
        {
            var response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlUpdateBrokerDeliveryRequestInfo, model);
            return response;
        }

        public async Task<StatusViewModel> RemoveJobDetailsForCustomer(List<int> jobIds)
        {
            var response = new StatusViewModel();
            var apiUrl = ApplicationConstants.UrlRemoveJobDetailsForCustomer;
            response = await ApiPostCall<StatusViewModel>(apiUrl, jobIds);

            return response;
        }

        public async Task<StatusViewModel> AddOptionalPickup(List<DSBColumnOptionalPickupInfoModel> model, UserContext userContext)
        {
            var response = new StatusViewModel();
            var dispatchDomain = new DispatchDomain(this);
            try
            {
                await InsertBulkPlantLocation(model, dispatchDomain, userContext);

                var apiUrl = ApplicationConstants.UrlAddOptionalPickup;
                response = await ApiPostCall<DrFilterPreferencesModel>(apiUrl, model);
                //insert record in bulk plant if any new records.
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "AddOptionalPickup", ex.Message, ex);
            }
            return response;
        }

        private static async Task InsertBulkPlantLocation(List<DSBColumnOptionalPickupInfoModel> model, DispatchDomain dispatchDomain, UserContext userContext)
        {
            var bulkPlantInfo = model.Where(x => x.DSBPickupLocationInfo.PickupLocationType == (int)PickupLocationType.BulkPlant).ToList();
            foreach (var bulkplantItem in bulkPlantInfo)
            {

                if (bulkplantItem.DSBPickupLocationInfo.TfxBulkPlant != null)
                {
                    var fuelDispatchLocation = bulkplantItem.DSBPickupLocationInfo.TfxBulkPlant.ToOptionalDispatchLocationEntity();
                    fuelDispatchLocation.CreatedBy = userContext.Id;
                    fuelDispatchLocation.CreatedDate = DateTime.Now;
                    int bulkPlantId = await dispatchDomain.SaveOptionalPickupBulkPlantLocation(fuelDispatchLocation, userContext.CompanyId);
                    bulkplantItem.DSBPickupLocationInfo.TfxBulkPlant.Id = bulkPlantId;
                }
            }
        }

        public async Task<List<DSBColumnOptionalPickupInfoModel>> GetOptionalPickupDetails(DSBColumnOptionalPickupInfoModel model)
        {
            var response = new List<DSBColumnOptionalPickupInfoModel>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetOptionalPickupDetails;
                response = await ApiPostCall<List<DSBColumnOptionalPickupInfoModel>>(apiUrl, model);
                int incrNumber = 1;
                response.ForEach(x =>
                {
                    x.incId = incrNumber;
                    incrNumber = incrNumber + 1;
                    x.isAdded = 0;
                });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetOptionalPickupDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> DeleteOptionalPickupDetails(string Id)
        {
            var response = new StatusViewModel();
            try
            {
                DSBColumnOptionalPickupInfoModel dSBColumnOptionalPickupInfoModel = new DSBColumnOptionalPickupInfoModel();
                dSBColumnOptionalPickupInfoModel.Id = Id;
                var apiUrl = ApplicationConstants.UrlDeleteOptionalPickupDetails;
                response = await ApiPostCall<StatusViewModel>(apiUrl, dSBColumnOptionalPickupInfoModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "DeleteOptionalPickupDetails", ex.Message, ex);
            }
            return response;
        }
        #region TBDDR
        public async Task<List<TBDDropdownDisplayItem>> GetMstProducts()
        {
            //Remove 'Regular Gas','Premium Gas','Midgrade Gas' MstProducts
            var olderProductIds = new List<int> { 14, 15, 16 };
            var response = new List<TBDDropdownDisplayItem>();
            try
            {
                response.AddRange(await Context.DataContext
                                .MstTfxProducts
                                .Where(t => t.IsActive && t.ProductDisplayGroupId != (int)ProductDisplayGroups.OtherFuelType && !olderProductIds.Contains(t.MstProductType.Id))
                                .Select(t => new TBDDropdownDisplayItem
                                {
                                    Id = t.Id,
                                    Name = t.Name,
                                    ProductTypeName = t.MstProductType != null ? t.MstProductType.Name : string.Empty,
                                    ProductTypeId = t.MstProductType != null ? t.MstProductType.Id : 0
                                }).ToListAsync()
                                );
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetMstProducts", ex.Message, ex);
            }
            return response;
        }
        #endregion
        #region PRELOADBOLRetain
        public async Task<List<PreBOLRetainDeliveryDetailsModel>> GetPreBOLRetainInfo(List<PreBOLRetainModel> model)
        {
            var response = new List<PreBOLRetainDeliveryDetailsModel>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetBOLRetainDetails;
                response = await ApiPostCall<List<PreBOLRetainDeliveryDetailsModel>>(apiUrl, model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetPreBOLRetainInfo", ex.Message, ex);
            }
            return response;
        }
        #endregion

        #region DeleteInvitedDriver
        public async Task<StatusViewModel> DeleteInvitedDriverFromRegion(RegionDriverRemoveModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlRemoveDriverFromRegion;
                response = await ApiPostCall<StatusViewModel>(apiUrl, viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "DeleteInvitedDriverFromRegion", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<InvitedDriverResponseModel>> CheckInvitedDriverScheduleExists(List<RegionDriverRemoveModel> viewModel)
        {
            var response = new List<InvitedDriverResponseModel>();
            try
            {
                var apiUrl = ApplicationConstants.UrlCheckInvitedDriverScheduleExists;
                response = await ApiPostCall<List<InvitedDriverResponseModel>>(apiUrl, viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "CheckInvitedDriverScheduleExists", ex.Message, ex);
            }
            return response;
        }
        #endregion

        #region SpiltDRs
        public async Task<StatusViewModel> UpdateSpiltDRsInfo(SpiltDeliveryRequestViewModel requestModel, DeliveryRequestViewModel inputData, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                requestModel.SpiltDRsViewModel.ForEach(x =>
                {
                    x.UniqueOrderNo = GenerateUniqueDRId(inputData, userContext);
                });
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlUpdateSpiltDRs, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "UpdateSpiltDRsInfo", ex.Message, ex);
            }
            return response;
        }
        #endregion
        #region CarrierXDelivery
        public async Task<CarrierXDeliveryRequestInfo> GetCompanyDeliveryRequestsDetails(List<int> requestModel)
        {
            var response = new CarrierXDeliveryRequestInfo();
            try
            {
                response = await ApiPostCall<CarrierXDeliveryRequestInfo>(ApplicationConstants.UrlGetCarrierXDeliveryInfo, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetCompanyDeliveryRequestsDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<JobTankAdditionalDetailModel>> GetJobsTankList(List<int> requestModel)
        {
            var response = new List<JobTankAdditionalDetailModel>();
            try
            {
                response = await ApiPostCall<List<JobTankAdditionalDetailModel>>(ApplicationConstants.UrlGetJobsTankInfo, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetJobsTankList", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DeliveryRequestViewModel>> GetDeliveryRequestDetails(List<int> requestModel)
        {
            var response = new List<DeliveryRequestViewModel>();
            try
            {
                response = await ApiPostCall<List<DeliveryRequestViewModel>>(ApplicationConstants.UrlGetCarrierDeliveryRequestDetails, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetDeliveryRequestDetails", ex.Message, ex);
            }
            return response;
        }
        #endregion

        public async Task<string> GetRegionByJobAndCompanyId(int portId, int companyId)
        {
            var response = string.Empty;
            try
            {
                if (portId > 0)
                {
                    var url = string.Format(ApplicationConstants.UrlGetRegionByJobAndCompanyId, portId, companyId);
                    response = await ApiGetCall<string>(url);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetRegionByJobAndCompanyId", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> ValidateDeliveryRequestInUse(List<string> DeliveryReqIds)
        {
            var response = new StatusViewModel();
            try
            {
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlValidateDeliveryRequestInUse, DeliveryReqIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "ValidateDeliveryRequestInUse", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DeliveryRequestViewModel>> GetBlendedGroupDeliveryRequestDetails(List<string> BlendedGroupIds)
        {
            var response = new List<DeliveryRequestViewModel>();
            try
            {
                response = await ApiPostCall<List<DeliveryRequestViewModel>>(ApplicationConstants.UrlGetBlendedGroupDeliveryRequestDetails, BlendedGroupIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetBlendedGroupDeliveryRequestDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<ChildDeliveryRequestInfoViewModel>> GetBlendedChildDeliveryRequestInfo(string blendedGroupId)
        {
            var response = new List<ChildDeliveryRequestInfoViewModel>();
            try
            {
                var url = string.Format(ApplicationConstants.UrlGetBlendedChildDeliveryRequestInfo, blendedGroupId);
                response = await ApiGetCall<List<ChildDeliveryRequestInfoViewModel>>(url);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetBlendedChildDeliveryRequestInfo", ex.Message, ex);
            }
            return response;
        }
        public async Task<RegionFavProductModel> GetRegionFavouriteProducts(int? jobId, string regionId, int companyId)
        {
            var response = new RegionFavProductModel();
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetRegionFavouriteProducts, jobId, regionId, companyId);
                response = await ApiGetCall<RegionFavProductModel>(apiUrl);
            }

            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetProductsForJobAssignedRegions", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DropdownDisplayExtendedListItem>> GeFavouriteProductsByJob(int jobId, int companyId)
        {
            var response = new List<DropdownDisplayExtendedListItem>();
            try
            {
                //var apiUrl = string.Format(ApplicationConstants.UrlGeFavouriteProductsByJob, jobId, companyId);
                var favProducts = await GetRegionFavouriteProducts(jobId, null, companyId);
                if (favProducts != null)
                {
                    if (favProducts.TfxFavProductTypeId == RegionFavProductType.ProductType && favProducts.TfxProductTypeIds != null && favProducts.TfxProductTypeIds.Any())
                    {
                        response = Context.DataContext.MstProducts.Where(t => favProducts.TfxProductTypeIds.Contains(t.ProductTypeId) && t.PricingSourceId == (int)PricingSource.Axxis)
                                                                    .Select(t => new DropdownDisplayExtendedListItem { Id = t.Id, Name = t.DisplayName ?? t.Name }).ToList();
                    }
                    else if (favProducts.TfxFavProductTypeId == RegionFavProductType.FuelType && favProducts.TfxFuelTypeIds != null && favProducts.TfxFuelTypeIds.Any())
                    {
                        response = Context.DataContext.MstProducts.Where(t => favProducts.TfxFuelTypeIds.Any(t1 => t1.Id == t.TfxProductId) && t.PricingSourceId == (int)PricingSource.Axxis)
                                                                    .Select(t => new DropdownDisplayExtendedListItem { Id = t.Id, Name = t.DisplayName ?? t.Name }).ToList();
                    }
                }
            }

            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GeFavouriteProductsByJob", ex.Message, ex);
            }
            return response;
        }

        public async Task<DeliveryReqPriority> GetPriorityForSalesDR(TankDetailsModel tank)
        {
            DeliveryReqPriority priority = DeliveryReqPriority.MustGo;
            try
            {
                var apiUrl = string.Format(ApplicationConstants.UrlGetPriorityForSalesDR);
                priority = await ApiPostCall<DeliveryReqPriority>(apiUrl, tank);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetPriorityForSalesDR", ex.Message, ex);
            }
            return priority;
        }
        private async Task<StatusViewModel> IsValidDRProducts(int companyId, List<RaiseDeliveryRequestInput> raiseDeliveryRequests)
        {
            var response = new StatusViewModel(Status.Success);
            var regionSelected = raiseDeliveryRequests.Select(t => t.CreatedByRegionId).FirstOrDefault();
            if (!string.IsNullOrEmpty(regionSelected))
            {
                var favProduct = await GetRegionFavouriteProducts(0, regionSelected, companyId);
                if (favProduct != null && favProduct.TfxFavProductTypeId != RegionFavProductType.None)
                {
                    var isProductValid = true;
                    if (favProduct.TfxFavProductTypeId == RegionFavProductType.ProductType && favProduct.TfxProductTypeIds != null && favProduct.TfxProductTypeIds.Any())
                    {
                        isProductValid = raiseDeliveryRequests.All(dr => dr.ProductTypeId == 0 || (dr.ProductTypeId > 0 && favProduct.TfxProductTypeIds.Contains(dr.ProductTypeId)));
                    }
                    else if (favProduct.TfxFavProductTypeId == RegionFavProductType.FuelType && favProduct.TfxFuelTypeIds != null && favProduct.TfxFuelTypeIds.Any())
                    {
                        // validate dr order fueltype present in fav or not
                        var allDrWithOrders = raiseDeliveryRequests.Where(t => t.OrderId.HasValue);
                        var favFueltypeIds = favProduct.TfxFuelTypeIds.Select(t => t.Id).ToList();
                        if (allDrWithOrders != null && allDrWithOrders.Any())
                        {
                            var selOrderIds = allDrWithOrders.Select(t => t.OrderId.Value);
                            var tfxProductIdOfOrders = Context.DataContext.Orders.Where(t => selOrderIds.Contains(t.Id)).Select(t => t.FuelRequest.MstProduct.TfxProductId).Distinct();
                            isProductValid = tfxProductIdOfOrders.Where(t => t.HasValue).All(t => favFueltypeIds.Contains(t.Value));
                        }
                        var remainingFueltypeDrs = raiseDeliveryRequests.Where(t => !t.OrderId.HasValue && t.FuelTypeId.HasValue);
                        if (isProductValid && remainingFueltypeDrs != null && remainingFueltypeDrs.Any())
                        {
                            isProductValid = remainingFueltypeDrs.All(dr => dr.FuelTypeId.HasValue && (dr.FuelTypeId > 0 && favFueltypeIds.Contains(dr.FuelTypeId.Value)));
                        }

                        // validate dr productType for no orderId
                        var remainingDrs = raiseDeliveryRequests.Where(t => !t.OrderId.HasValue && !t.FuelTypeId.HasValue);
                        if (isProductValid && remainingDrs != null && remainingDrs.Any())
                        {
                            isProductValid = remainingDrs.All(dr => dr.ProductTypeId == 0 || (dr.ProductTypeId > 0 && favProduct.TfxFuelTypeIds.Any(t => t.Code == dr.ProductTypeId.ToString())));
                        }
                    }
                    if (!isProductValid)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMsgProductNotAvailable;
                        return response;
                    }
                }
            }
            return response;
        }

        public async Task<bool> IsPublishedDR(int companyId, string productIds, string fuelTypeIds)
        {
            bool response = false;

            try
            {
                var orderIds = string.Empty;
                if (!string.IsNullOrEmpty(fuelTypeIds))
                {
                    var pList = fuelTypeIds.Split(',').Select(x => int.Parse(x.Trim())).ToList();
                    List<int> selectedOrderIds = await Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == companyId && t.IsActive
                    && t.OrderXStatuses.FirstOrDefault(x => x.IsActive).StatusId == (int)OrderStatus.Open
                    && t.FuelRequest.MstProduct.TfxProductId.HasValue
                    && pList.Contains(t.FuelRequest.MstProduct.TfxProductId.Value)).Select(t => t.Id).ToListAsync();
                    orderIds = string.Join(",", selectedOrderIds);
                }

                if (!string.IsNullOrEmpty(productIds) || !string.IsNullOrEmpty(orderIds))
                {
                    var apiUrl = string.Format(ApplicationConstants.UrlIsPublishedDR, companyId, productIds, orderIds);
                    return await ApiPostCall<bool>(apiUrl, new { });
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "IsPublishedDR", ex.Message, ex);
            }
            return response;

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
        private string GenerateUniqueDRId(DeliveryRequestViewModel requests, UserContext userContext)
        {
            string[] companyWordsDetails = userContext.CompanyName.Split(' ');
            var supplierName = GetCompanyWordInfo(userContext.CompanyName, companyWordsDetails);
            string[] customercompanyWordsDetails = requests.CustomerCompany.Split(' ');
            var customerName = GetCompanyWordInfo(requests.CustomerCompany, customercompanyWordsDetails);
            string productCode = string.Empty;
            var productCodeInfo = Context.DataContext.MstProductTypes.Where(x => x.Id == requests.ProductTypeId).FirstOrDefault();
            if (productCodeInfo != null)
            {
                productCode = productCodeInfo.ProductCode.ToUpper();
            }
            var dateFormat = DateTime.Now.ToString("MMddyy");
            var uniqueDRID = supplierName + customerName + productCode + dateFormat + GetUniqueKey();
            return uniqueDRID;
        }
        private void GenerateUniqueDRId(List<RaiseDeliveryRequestInput> raiseDeliveryRequests, UserContext userContext, RaiseDeliveryRequestModel requests)
        {
            if (raiseDeliveryRequests.Any(t => t.IsBlendedRequest) && raiseDeliveryRequests.Any(x => !x.IsTBD))
            {

                var blendedDRsDetails = requests.DeliveryRequests.Where(x => x.IsBlendedRequest).GroupBy(x => x.BlendedGroupId).Select(x => x.Key).ToList();
                foreach (var item in blendedDRsDetails)
                {
                    var blendedDRsGroupInfo = requests.DeliveryRequests.Where(x => x.IsBlendedRequest && x.BlendedGroupId == item).ToList();
                    if (blendedDRsGroupInfo.Any())
                    {
                        string[] companyWordsDetails = userContext.CompanyName.Split(' ');
                        var supplierName = GetCompanyWordInfo(userContext.CompanyName, companyWordsDetails);
                        string[] customercompanyWordsDetails = blendedDRsGroupInfo.FirstOrDefault().CustomerCompany.Split(' ');
                        var customerName = GetCompanyWordInfo(blendedDRsGroupInfo.FirstOrDefault().CustomerCompany, customercompanyWordsDetails);
                        var productCode = "BL";
                        var dateFormat = DateTime.Now.ToString("MMddyy");
                        var uniqueDRID = supplierName + customerName + productCode + dateFormat + GetUniqueKey();
                        blendedDRsGroupInfo.ForEach(x =>
                        {
                            x.UniqueOrderNo = uniqueDRID;
                        });
                    }
                }

            }
            if (raiseDeliveryRequests.Any(t => t.IsTBD))
            {
                var tbdDRGroupDetails = requests.DeliveryRequests.Where(x => x.IsTBD).GroupBy(x => x.TBDGroupId).Select(x => x.Key).ToList();
                var carrierCompanyIDsInfo = requests.DeliveryRequests.Where(x => x.IsTBD).Select(x => x.AssignedToCompanyId).ToList();
                var carrierCompanyInfos = Context.DataContext.Companies.Where(x => carrierCompanyIDsInfo.Contains(x.Id)).Select(x => new { x.Id, x.Name }).ToList();

                foreach (var item in tbdDRGroupDetails)
                {
                    var tbdDRsGroupInfo = requests.DeliveryRequests.Where(x => x.IsTBD && x.TBDGroupId == item).ToList();
                    if (tbdDRsGroupInfo.Any())
                    {
                        string[] companyWordsDetails = userContext.CompanyName.Split(' ');
                        var supplierName = GetCompanyWordInfo(userContext.CompanyName, companyWordsDetails);
                        string[] customercompanyWordsDetails = carrierCompanyInfos.FirstOrDefault(x1 => x1.Id == tbdDRsGroupInfo.FirstOrDefault().AssignedToCompanyId)?.Name.Split(' ');
                        var customerName = GetCompanyWordInfo(carrierCompanyInfos.FirstOrDefault(x1 => x1.Id == tbdDRsGroupInfo.FirstOrDefault().AssignedToCompanyId)?.Name, customercompanyWordsDetails);
                        var productCode = tbdDRsGroupInfo.FirstOrDefault().ProductShortCode.ToUpper();
                        var dateFormat = DateTime.Now.ToString("MMddyy");
                        var uniqueDRID = supplierName + customerName + productCode + dateFormat + GetUniqueKey();
                        tbdDRsGroupInfo.ForEach(x =>
                        {
                            x.UniqueOrderNo = uniqueDRID;
                        });
                    }
                }
            }
            requests.DeliveryRequests.Where(x => !x.IsTBD && !x.IsBlendedRequest && string.IsNullOrWhiteSpace(x.UniqueOrderNo)).ToList().ForEach(x =>
            {
                string[] companyWordsDetails = userContext.CompanyName.Split(' ');
                var supplierName = GetCompanyWordInfo(userContext.CompanyName, companyWordsDetails);
                string[] customercompanyWordsDetails = x.CustomerCompany.Split(' ');
                var customerName = GetCompanyWordInfo(x.CustomerCompany, customercompanyWordsDetails);
                var productCode = x.ProductShortCode;
                var dateFormat = DateTime.Now.ToString("MMddyy");
                var uniqueDRID = supplierName + customerName + productCode + dateFormat + GetUniqueKey();
                x.UniqueOrderNo = uniqueDRID;
            });
        }
    }
}

