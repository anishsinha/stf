using SiteFuel.Exchange.Api.Mobile.Attributes;
using SiteFuel.Exchange.Api.Mobile.Common;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;

namespace SiteFuel.Exchange.Api.Mobile.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif

    [ValidateToken]
    public class JobController : ApiBaseController
    {
        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<CreateJobOutputViewModel> CreateJob(CreateJobInputViewModel job)
        {
            using (var tracer = new Tracer("JobController", "CreateJob"))
            {
                var response = new CreateJobOutputViewModel();
                try
                {
                    if (ModelState.IsValid)
                    {
                        response = IsValidJobDetails(job);
                        if (response.StatusCode == Status.Success)
                        {
                            JobStepsViewModel viewModel = ToViewModel(job);
                            var userContext = await GetUserContext(job.UserId);
                            var jobResponse = await ContextFactory.Current.GetDomain<JobDomain>().SaveJobStepsAsync(userContext, viewModel);
                            response.JobId = viewModel.Job.Id;
                            response.StatusCode = jobResponse.StatusCode;
                            response.StatusMessage = jobResponse.StatusMessage;
                        }
                    }
                    else
                    {
                        var geterror = new CommonMethods().GetErrorMessage(ModelState);
                        response.StatusCode = geterror.StatusCode;
                        response.StatusMessage = geterror.StatusMessage;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("JobController", "CreateJob", ex.Message, ex);
                }
                return response;
            }
        }

        private CreateJobOutputViewModel IsValidJobDetails(CreateJobInputViewModel job)
        {
            var response = new CreateJobOutputViewModel();
            response.StatusCode = Status.Success;
            response.StatusMessage = Resource.errMessageSuccess;
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidJobName(0, job.Name, job.CompanyId);
            if (!result)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = string.Format(Resource.valMessageAlreadyExist, job.Name);
            }
            return response;
        }

        private JobStepsViewModel ToViewModel(CreateJobInputViewModel job)
        {
            HelperDomain helperDomain = new HelperDomain();
            var viewModel = new JobStepsViewModel();
            viewModel.Job.CreatedBy = job.UserId;
            viewModel.Job.StatusId = (int)JobStatus.Open;

            viewModel.UserId = job.UserId;
            viewModel.Job.Name = job.Name;
            viewModel.Job.JobID = job.JobID;
            viewModel.Job.Address = job.Address;
            viewModel.Job.City = job.City;
            viewModel.Job.State.Id = job.StateId;
            viewModel.Job.ZipCode = job.ZipCode;
            viewModel.Job.OnsiteContacts = job.OnsiteContacts;
            viewModel.Job.Latitude = job.Latitude;
            viewModel.Job.Longitude = job.Longitude;
            viewModel.Job.StartDate = job.StartDate;
            viewModel.Job.EndDate = job.EndDate;
            var countryId = helperDomain.GetCountryFromState(job.StateId);
            viewModel.Job.Country.Id = countryId;
            if (countryId == (int)Country.CAN)
            {
                viewModel.Job.Country.UoM = UoM.Litres;
                viewModel.Job.Country.Currency = Currency.CAD;
            }
            else
            {
                viewModel.Job.Country.UoM = UoM.Gallons;
                viewModel.Job.Country.Currency = Currency.USD;
            }
            return viewModel;
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<JobListViewModel>> GetJobList(int userId, int offset = 0, int count = 0)
        {
            using (var tracer = new Tracer("JobController", "GetJobList"))
            {
                List<JobListViewModel> response = new List<JobListViewModel>();
                try
                {
                    if (userId > 0)
                    {
                        var brandedSuppCompanyId = GetBrandedSupplierCompId();
                        response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobListAsync(userId, string.Empty, offset, count, 0, 0, brandedSuppCompanyId);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("JobController", "GetJobList", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<JobListWithTanksViewModel>> GetJobListWithTanks(int userId, int companyId, decimal latitude, decimal longitude)
        {
            using (var tracer = new Tracer("JobController", "GetJobList"))
            {
                List<JobListWithTanksViewModel> response = new List<JobListWithTanksViewModel>();
                try
                {
                    var brandedSupCompanyId = GetBrandedSupplierCompId();
                    response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobListWithTanksAsync(userId, companyId, latitude, longitude, brandedSupCompanyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("JobController", "GetJobList", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<JobListWithProductTypes>> GetJobListWithProductTypes(int userId, int companyId, decimal latitude, decimal longitude)
        {
            using (var tracer = new Tracer("JobController", "GetJobListWithProductTypes"))
            {
                List<JobListWithProductTypes> response = new List<JobListWithProductTypes>();
                try
                {
                    var brandedSupCompanyId = GetBrandedSupplierCompId();
                    response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobListWithProductTypes(userId, companyId, latitude, longitude, 0, brandedSupCompanyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("JobController", "GetJobListWithProductTypes", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public List<DropdownDisplayItem> GetOnSiteContacts(int companyId)
        {
            using (var tracer = new Tracer("JobController", "GetOnSiteContacts"))
            {
                List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
                try
                {
                    if (companyId > 0)
                    {
                        response = ContextFactory.Current.GetDomain<HelperDomain>().GetCompanyOnsiteConstacts(companyId);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("JobController", "GetOnSiteContacts", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<JobListViewModel>> SearchJobs(int userId, string searchCriteria, int offset = 0, int count = 0)
        {
            using (var tracer = new Tracer("JobController", "SearchJobs"))
            {
                List<JobListViewModel> response = new List<JobListViewModel>();
                try
                {
                    if (userId > 0)
                    {
                        var brandedSuppCompanyId = GetBrandedSupplierCompId();

                        response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobListAsync(userId, searchCriteria, offset, count, 0, 0, brandedSuppCompanyId);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("JobController", "SearchJobs", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<JobListViewModel>> GetJobListByScheduleDate(int userId, int offset = 0, int count = 0, long scheduleDate = 0, int supplierCompanyId = 0)
        {
            using (var tracer = new Tracer("JobController", "GetJobListByScheduleDate"))
            {
                List<JobListViewModel> response = new List<JobListViewModel>();
                try
                {
                    if (userId > 0)
                    {
                        var brandedSuppCompanyId = GetBrandedSupplierCompId();
                        if (scheduleDate == 0)
                            scheduleDate = DateTimeOffset.Now.ToUnixTimeMilliseconds(); //if scheduleDate is passed 0 , consider today's date
                        response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobListAsync(userId, string.Empty, offset, count, scheduleDate, supplierCompanyId, brandedSuppCompanyId);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("JobController", "GetJobListByScheduleDate", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        public async Task<JobStepsViewModel> GetJobDetails(int jobId)
        {
            using (var tracer = new Tracer("JobController", "GetJobDetails"))
            {
                JobStepsViewModel response = new JobStepsViewModel();
                try
                {
                    if (jobId > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobStepsAsync(jobId);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("JobController", "GetJobDetails", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> CreateDipTest(ApiDipTestViewModel apiDipTestViewModel)
        {
            var response = new StatusViewModel();
            try
            {
                if (apiDipTestViewModel == null) { return response; }
                if (apiDipTestViewModel.Demands == null || apiDipTestViewModel.CompanyId == 0) { return response; }

                var demands = apiDipTestViewModel.Demands;
                var companyId = apiDipTestViewModel.CompanyId;

                if (demands.Any())
                {
                    var supplierId = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetAcceptedCompanyIdByJobId(int.Parse(demands[0].SiteId));
                    if (supplierId > 0)
                    {
                        string timeZoneName = string.Empty;
                        var jobId = ContextFactory.Current.GetDomain<JobDomain>().GetDisplayJobIdByJobIdForJob(int.Parse(demands[0].SiteId), ref timeZoneName);
                        DateTime jobTime = DateTimeOffset.Now.DateTime;
                        if (!string.IsNullOrWhiteSpace(timeZoneName))
                        {
                            jobTime = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName).DateTime;
                        }
                        foreach (var demand in demands)
                        {
                            demand.CaptureTime = jobTime;
                            demand.BuyerCompanyId = companyId;
                            demand.SiteId = jobId;
                            demand.DataSourceTypeId = (int)DipTestMethod.Manual;
                            demand.ProductName = Enum.GetName(typeof(ProductTypes), int.Parse(demand.ProductName));
                        }
                        response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().CreateTankDipTest(demands.ToList(), supplierId);
                        return response;
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorDipTestNotCreated;
                        return response;
                    }
                }
                else
                {
                    response.StatusCode = Status.Warning;
                    response.StatusMessage = Resource.warningNoTanksForSelectedJob;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobController", "CreateDipTest", ex.Message, ex);
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> GetNearestJobsByCustomer(int customerId, int supplierId, int fuelTypeId, decimal latitude, decimal longitude, double radius = 100, int userId = 0)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("JobController", $"GetNearestJobsByCustomer(customerId: {customerId}, latitude: {latitude}, longitude: {longitude}, radius: {radius})"))
            {
                try
                {
                    if (customerId <= 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "CustomerId required";
                        return response;
                    }
                    if (supplierId <= 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "SupplierId required";
                        return response;
                    }
                    if (fuelTypeId <= 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "FuelTypeId required";
                        return response;
                    }
                    if (latitude == 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "Latitude required";
                        return response;
                    }
                    if (longitude == 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "Longitude required";
                        return response;
                    }

                    var viewModel = new ApiNearestJobByCustomerModel();
                    viewModel.CustomerId = customerId;
                    viewModel.SupplierId = supplierId;
                    viewModel.FuelTypeIds.Add(fuelTypeId);
                    viewModel.Latitude = latitude;
                    viewModel.Longitude = longitude;
                    viewModel.Radius = radius;
                    viewModel.UserId = userId;

                    var jobsByCustomer = await ContextFactory.Current.GetDomain<JobDomain>().GetNearestJobsByCustomer(viewModel);
                    response.ResponseData = jobsByCustomer;
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Status.Success.ToString();
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = "Failed";
                    LogManager.Logger.WriteException("JobController", "GetNearestJobsByCustomer", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> GetNearestJobsByCustomerAndFuelTypes(ApiNearestJobByCustomerModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("JobController", $"GetNearestJobsByCustomerAndFuelTypes(customerId: {viewModel.CustomerId}, latitude: {viewModel.Latitude}, longitude: {viewModel.Longitude}, radius: {viewModel.Radius})"))
            {
                try
                {
                    if (viewModel.CustomerId <= 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "CustomerId required";
                        return response;
                    }
                    if (viewModel.SupplierId <= 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "SupplierId required";
                        return response;
                    }
                    if (!viewModel.FuelTypeIds.Any())
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "FuelTypeId required";
                        return response;
                    }
                    if (viewModel.Latitude == 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "Latitude required";
                        return response;
                    }
                    if (viewModel.Longitude == 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "Longitude required";
                        return response;
                    }
                    
                    var jobsByCustomer = await ContextFactory.Current.GetDomain<JobDomain>().GetNearestJobsByCustomer(viewModel);
                    response.ResponseData = jobsByCustomer;
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Status.Success.ToString();
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = "Failed";
                    LogManager.Logger.WriteException("JobController", "GetNearestJobsByCustomerAndFuelTypes", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> CreateDeliveryRequest(List<ApiRaiseDeliveryRequestInput> viewModel)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("JobController", "CreateDeliveryRequest"))
            {
                var customerData = viewModel.FirstOrDefault();
                if (customerData != null)
                {
                    var userContext = new UserContext() { Id = customerData.UserId, CompanyId = customerData.BuyerCompanyId };
                    response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().RaiseDeliveryRequestsFromBuyerApp(viewModel, userContext);
                }
            }
            return response;
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> DeleteRecurringDeliveryRequest(ApiDeleteRecurringSchedule input)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("JobController", "DeleteRecurringDeliveryRequest"))
            {               
                if (!string.IsNullOrWhiteSpace(input.Id))
                {
                    response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().DeleteRecurringSchedule(input.Id, input.UserId);
                }
            }
            return response;
        }
    }
}