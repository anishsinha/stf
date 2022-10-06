using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Dispatcher;
using SiteFuel.Exchange.ViewModels.Job;
using SiteFuel.Exchange.ViewModels.Queue;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class DashboardDomain : BaseDomain
    {
        public DashboardDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public DashboardDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<DashboardViewModel> GetBuyerDashboardAsync(UserContext userContext, int jobId = 0, string groupIds = "", Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetBuyerDashboardAsync"))
            {
                DashboardViewModel response = new DashboardViewModel();
                var helperDomain = new HelperDomain(this);
                var masterDomain = new MasterDomain(this);

                try
                {
                    var isTaxExemptDisplayed = true;
                    var userData = await Context.DataContext.Users.
                       Select(x => new
                       {
                           User = x,
                           UserPageSettings = x.UserPageSettings.FirstOrDefault(t => t.UserId == userContext.Id && t.PageId == ApplicationConstants.BuyerDashboard)
                       }).SingleOrDefaultAsync(t => t.User.Id == userContext.Id);

                    var user = userData.User;
                    if (user != null)
                    {
                        isTaxExemptDisplayed = user.IsTaxExemptDisplayed;

                        response = new DashboardViewModel(Status.Success)
                        {
                            IsTaxExemptDisplayed = isTaxExemptDisplayed,
                            SelectedJobId = jobId
                        };
                        if (jobId > 0)
                        {
                            response.SelectedJobNextDeliverySchedule = await GetNextDeliveryScheduleForJob(user.Company.Id, jobId, response);
                        }

                        response.CompanyGroup.GroupIds = helperDomain.GetGroupList(groupIds);
                        if (response.CompanyGroup.GroupIds.Count == 0)
                        {
                            var companyGroups = masterDomain.GetCompanyGroupList(userContext.CompanyId, userContext.CompanySubTypeId);
                            if (companyGroups.Count > 0)
                                response.IsCompanyGroupAvailable = true;
                        }
                        else
                        {
                            response.IsCompanyGroupAvailable = true;
                        }


                        response.TileSetting = await GetDBTileSettings(userData.UserPageSettings, userContext);
                        // set dashboard tile setting
                        SetTileSetting(response, response.TileSetting, UserRoles.Buyer);

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageSuccess;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetBuyerDashboardAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public string DecryptData(string request)
        {
            var response = request;
            try
            {
                if (request != "" && request != "0" && request != "-1")
                {
                    byte[] bytes;
                    bytes = Convert.FromBase64String(request);
                    response = Encoding.ASCII.GetString(bytes);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "DecryptData: " + request, ex.Message, ex);
            }
            return response;
        }

        public async Task<List<MapViewModel>> GetBuyerJobLocationsForMap(int userId, int jobId = 0, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetBuyerJobLocationsForMap"))
            {
                var response = new List<MapViewModel>();
                try
                {
                    var openJobs = await GetBuyerOpenJobsAsync(userId, jobId, countryId, currency);
                    response = openJobs.Select(t => t.ToMapViewModel()).OrderByDescending(t => t.JobId).ToList();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetBuyerJobLocationsForMap", ex.Message, ex);
                }
                return response;
            }
        }

        private async Task<string> GetNextDeliveryScheduleForJob(int companyId, int jobId, DashboardViewModel dashboardViewModel)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetNextDeliveryScheduleForJob"))
            {
                string nextSchedule = string.Empty;
                List<DeliveryScheduleForJobViewModel> nextSchedules = new List<DeliveryScheduleForJobViewModel>();

                try
                {
                    var jobDetails = Context.DataContext.Jobs.SingleOrDefault(t => t.Id == jobId);
                    var jobLocationTime = DateTimeOffset.Now.ToTargetDateTimeOffset(jobDetails.TimeZoneName).Date;
                    var deliverySchedulesOfJob = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.OrderId != null && t.Order.FuelRequest.Job.Id == jobId
                                                    && t.Order.BuyerCompanyId == companyId && t.Order.OrderXStatuses.Any(t1 => t1.IsActive && t1.StatusId == (int)OrderStatus.Open)
                                                    && t.Date >= jobLocationTime && t.IsActive
                                                    && (t.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Accepted || t.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Rescheduled))
                                                    .ToListAsync();

                    //API logic
                    if (deliverySchedulesOfJob.Any())
                    {
                        var nextScheduleOfJob = deliverySchedulesOfJob.OrderBy(t => t.Date).ThenBy(t => t.StartTime).ThenBy(t => t.EndTime).First();
                        nextSchedule = nextScheduleOfJob.Date.DayOfWeek.ToString() + ": " + nextScheduleOfJob.Date.Date.ToString(Resource.constFormatDate)
                                            + Constants.Between + nextScheduleOfJob.StartTime.GetTimeInAmPmFormat() + Constants.And + nextScheduleOfJob.EndTime.GetTimeInAmPmFormat()
                                            + Constants.For + nextScheduleOfJob.Quantity.GetPreciseValue(2) + " " + nextScheduleOfJob.UoM.ToString();

                        dashboardViewModel.SelectedJobPoNumber = nextScheduleOfJob.Order.PoNumber;
                        dashboardViewModel.SelectedJobOrderId = nextScheduleOfJob.OrderId ?? 0;
                    }
                    //Web Application logic
                    nextSchedules = await GetDeliverySchedules(companyId, jobId, nextSchedules, jobLocationTime, deliverySchedulesOfJob);
                    dashboardViewModel.NextSchedulesOfJob = nextSchedules;

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetNextDeliveryScheduleForJob", ex.Message + " jobId: " + jobId, ex);
                }

                return nextSchedule;
            }
        }

        private async Task<List<DeliveryScheduleForJobViewModel>> GetDeliverySchedules(int companyId, int jobId, List<DeliveryScheduleForJobViewModel> nextSchedules, DateTime jobLocationTime, List<DeliveryScheduleXTrackableSchedule> deliverySchedulesOfJob)
        {
            var deliverySchedules = new List<DeliveryScheduleForJobViewModel>();
            //// for multiple fuel request delivery schedules
            if (deliverySchedulesOfJob.Any())
            {
                foreach (var item in deliverySchedulesOfJob)
                {
                    nextSchedules.Add(new DeliveryScheduleForJobViewModel()
                    {
                        ScheduleDate = item.Date,
                        GallonsOrdered = item.Quantity.GetPreciseValue(2),
                        OrderId = item.OrderId ?? 0,
                        PoNumber = item.Order.PoNumber,
                        ScheduleStartTime = item.StartTime.GetTimeInAmPmFormat().ToString(),
                        ScheduleEndTime = item.EndTime.GetTimeInAmPmFormat().ToString(),
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        UoM = item.UoM
                    });
                }
            }

            //// for Single fuel request delivery schedules
            var frSingleDeliveries = await Context.DataContext.Orders
                             .Where(o => o.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery &&
                                    o.FuelRequest.Job.Id == jobId && o.BuyerCompanyId == companyId && o.IsActive && o.FuelRequest.IsActive &&
                                    o.OrderXStatuses.Any(x => x.IsActive && x.StatusId == (int)OrderStatus.Open) &&
                                    o.FuelRequest.FuelRequestDetail.StartDate >= jobLocationTime).ToListAsync();

            if (frSingleDeliveries != null && frSingleDeliveries.Any())
            {
                foreach (var item in frSingleDeliveries)
                {
                    var frStatus = item.FuelRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive);
                    if (frStatus.StatusId == (int)FuelRequestStatus.Accepted || frStatus.StatusId == (int)FuelRequestStatus.CounterOfferAccepted)
                    {
                        nextSchedules.Add(new DeliveryScheduleForJobViewModel()
                        {
                            ScheduleDate = item.FuelRequest.FuelRequestDetail.StartDate,
                            GallonsOrdered = (item.BrokeredMaxQuantity ?? item.FuelRequest.MaxQuantity).GetPreciseValue(2),
                            OrderId = item.Id,
                            PoNumber = item.PoNumber,
                            ScheduleStartTime = item.FuelRequest.FuelRequestDetail.StartTime.GetTimeInAmPmFormat().ToString(),
                            ScheduleEndTime = item.FuelRequest.FuelRequestDetail.EndTime.GetTimeInAmPmFormat().ToString(),
                            StartTime = item.FuelRequest.FuelRequestDetail.StartTime,
                            EndTime = item.FuelRequest.FuelRequestDetail.EndTime
                        });
                    }
                }
            }

            //// get first latest schedule delivery and all deliveries in between
            if (nextSchedules.Any())
            {
                nextSchedules = nextSchedules.OrderBy(t => t.ScheduleDate).ThenBy(t => t.StartTime).ThenBy(t => t.EndTime).ToList();
                var latestDeliverySchedule = nextSchedules.First();
                var deliveryDate = latestDeliverySchedule.ScheduleDate;
                TimeSpan endTime = latestDeliverySchedule.EndTime;

                deliverySchedules = nextSchedules.Where(s => s.ScheduleDate == deliveryDate && s.StartTime <= endTime).ToList();
            }

            return deliverySchedules;
        }

        public async Task<DashboardViewModel> GetSupplierDashboardAsync(UserContext userContext, string groupIds = "")
        {
            using (var tracer = new Tracer("DashboardDomain", "GetSupplierDashboardAsync"))
            {
                DashboardViewModel response = new DashboardViewModel();
                var helperDomain = new HelperDomain(this);
                var masterDomain = new MasterDomain(helperDomain);

                try
                {
                    var isTaxExemptDisplayed = true;
                    var userData = await Context.DataContext.Users.
                        Select(x => new
                        {
                            User = x,
                            UserPageSettings = x.UserPageSettings.FirstOrDefault(t => t.UserId == userContext.Id && t.PageId == ApplicationConstants.SupplierDashboard)
                        }).SingleOrDefaultAsync(t => t.User.Id == userContext.Id);

                    var user = userData.User;
                    if (user != null)
                    {
                        isTaxExemptDisplayed = user.IsTaxExemptDisplayed;
                        response = new DashboardViewModel(Status.Success)
                        {
                            IsTaxExemptDisplayed = isTaxExemptDisplayed
                        };

                        response.CompanyGroup.GroupIds = helperDomain.GetGroupList(groupIds);
                        if (response.CompanyGroup.GroupIds.Count == 0)
                        {
                            var companyGroups = masterDomain.GetCompanyGroupList(userContext.CompanyId, userContext.CompanySubTypeId);
                            if (companyGroups.Count > 0)
                                response.IsCompanyGroupAvailable = true;
                        }
                        else
                        {
                            response.IsCompanyGroupAvailable = true;
                        }
                    }
                    response.Drivers = helperDomain.GetAllDrivers(userContext.CompanyId, true);
                    response.TileSetting = await GetDBTileSettings(userData.UserPageSettings, userContext);

                    // set dashboard tile setting
                    SetTileSetting(response, response.TileSetting, UserRoles.Supplier);

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.msgSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorMsgTileSettings;
                    LogManager.Logger.WriteException("DashboardDomain", "GetSupplierDashboardAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<bool> UpdateIsTaxExemptDisplayed(int userId)
        {
            using (var tracer = new Tracer("DashboardDomain", "UpdateIsTaxExemptDisplayed"))
            {
                var response = false;
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId);
                        if (user != null)
                        {
                            user.IsTaxExemptDisplayed = true;
                            Context.DataContext.Entry(user).State = EntityState.Modified;
                            await Context.CommitAsync();
                            transaction.Commit();
                            response = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("DashboardDomain", "UpdateIsTaxExemptDisplayed", ex.Message, ex);
                    }
                }
                return response;
            }
        }

        public async Task<DashboardJobsViewModel> GetBuyerDashboardJobsAsync(int userId, int jobId, int countryId = (int)Country.USA, Currency currency = Currency.USD, string groupIds = "")
        {
            using (var tracer = new Tracer("DashboardDomain", "GetBuyerDashboardJobsAsync"))
            {
                var response = new DashboardJobsViewModel();
                try
                {
                    var openJobs = await GetBuyerOpenJobsAsync(userId, jobId, countryId, currency, groupIds);
                    var user = await Context.DataContext.Users.Include(t => t.Company).SingleOrDefaultAsync(t => t.Id == userId);
                    if (user != null && user.Company != null && openJobs != null)
                    {
                        var jobDeliveredPercentage = openJobs.Where(t => t.JobBudget.Budget > 0).Select(t => new
                        {
                            Status = t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId,
                            DeliveredPercentage = ((t.HedgeDroppedAmount + t.SpotDroppedAmount) * 100) / t.JobBudget.Budget
                        });

                        response = new DashboardJobsViewModel(Status.Success)
                        {
                            UnderBudgetJobsCount = jobDeliveredPercentage.Count(t => t.DeliveredPercentage <= user.Company.BudgetAlertPercentage),
                            NoBudgetJobsCount = openJobs.Count(t => t.JobBudget.BudgetCalculationTypeId == (int)BudgetCalculationType.NoBudget),
                            OverBudgetJobsCount = jobDeliveredPercentage.Count(t => t.DeliveredPercentage >= user.Company.BudgetAlertPercentage),
                            BudgetAlertPercentage = user.Company.BudgetAlertPercentage,
                            TotalJobsCount = openJobs.Count,
                            TotalBudget = openJobs.Sum(t => t.JobBudget.Budget),
                            TotalHedgeDroppedAmount = openJobs.Sum(t => t.HedgeDroppedAmount),
                            TotalSpotDroppedAmount = openJobs.Sum(t => t.SpotDroppedAmount),
                        };

                        if (jobId > 0)
                        {
                            response.AssignedAssetsCount = Context.DataContext.JobXAssets.Where(t => t.JobId == jobId
                                                                                            && t.RemovedBy == null
                                                                                            && t.RemovedDate == null
                                                                                            && t.Job.Id == jobId
                                                                                            && t.Asset.IsActive)
                                                                                .Count();
                        }
                    }
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetBuyerDashboardJobsAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<DashboardOrdersViewModel> GetBuyerDashboardOrdersAsync(int userId, int companyId, int jobId = 0, int countryId = (int)Country.All, int currency = (int)Currency.None, string groupIds = "")
        {
            using (var tracer = new Tracer("DashboardDomain", "GetBuyerDashboardOrdersAsync"))
            {
                var response = new DashboardOrdersViewModel();
                try
                {
                    var activeOrders = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetBuyerLastActiveOrders(companyId, userId, jobId, countryId, currency, groupIds);
                    response.Last5ActiveOrders = activeOrders.Skip(1).ToList();
                    if (activeOrders != null && activeOrders.Any())
                    {
                        // 0th record from SP is for total , open , closed count
                        response.TotalOrderCount = activeOrders.ElementAt(0).TotalOrders;
                        response.OpenOrderCount = activeOrders.ElementAt(0).OpenOrders;
                        response.ClosedOrderCount = activeOrders.ElementAt(0).ClosedOrders;
                        response.CanceledOrderCount = activeOrders.ElementAt(0).CancelledOrders;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetBuyerDashboardOrdersAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<DashboardInvoicesViewModel> GetBuyerDashboardInvoicesAsync(int userId, int companyId, bool isBuyerAdmin, int jobId = 0, int allowedInvoiceType = 0, int countryId = (int)Country.All, Currency currency = Currency.None, string groupIds = "")
        {
            using (var tracer = new Tracer("DashboardDomain", "GetBuyerDashboardInvoicesAsync"))
            {
                var response = new DashboardInvoicesViewModel();
                try
                {
                    var helperDomain = new HelperDomain(this);
                    var jobIds = await helperDomain.GetJobIdsAsync(userId, groupIds);
                    if (jobIds != null)
                    {
                        var groupIdslist = helperDomain.GetGroupList(groupIds);
                        var invoices = Context.DataContext.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                                                && t.Order != null && !t.IsBuyPriceInvoice
                                                                                && ((groupIdslist.Count == 0 && t.Order.BuyerCompanyId == companyId) ||
                                                                                    (groupIdslist.Count > 0 && t.Order.BuyerCompany.SubCompanies.Any(t1 => t1.SubCompanyId == t.Order.BuyerCompanyId && groupIdslist.Contains(t1.CompanyGroupId))))
                                                                                && t.Order.FuelRequest.Job.IsActive
                                                                                && (jobId == 0 || t.Order.FuelRequest.Job.Id == jobId)
                                                                                && jobIds.Contains(t.Order.FuelRequest.Job.Id)
                                                                                && (countryId == (int)Country.All || (t.Currency == currency && t.Order.FuelRequest.Job.CountryId == countryId))
                                                                                && t.Order.FuelRequest.Job.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open);


                        response.InvoicesFromDropTicketCount = invoices.Count(t => t.ParentId != null);
                        if (allowedInvoiceType == (int)InvoiceType.DigitalDropTicketManual || allowedInvoiceType == (int)InvoiceType.DigitalDropTicketMobileApp)
                        {
                            invoices = invoices.Where(t => t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual ||
                                                     t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp);
                        }
                        else
                        {
                            invoices = invoices.Where(t => t.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual &&
                                                     t.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp);
                        }
                        var filteredInvoices = invoices.Select(t => new
                        {
                            t.ParentId,
                            InvoiceWorkflowApprovers = t.Order.FuelRequest.Job.JobXApprovalUsers,
                            InvoiceStatuses = t.InvoiceXInvoiceStatusDetails,
                            t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId,
                            t.InvoiceVersionStatusId,
                            t.InvoiceTypeId,
                            t.Order.FuelRequest.FuelRequestTypeId,
                            t.WaitingFor
                        }).ToList();

                        if (groupIdslist.Count == 0)
                        {
                            filteredInvoices = filteredInvoices.Where(t => CheckInvoiceWorkflow(userId, isBuyerAdmin, t.InvoiceWorkflowApprovers, t.InvoiceStatuses, t.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest)).ToList();
                        }
                        response.ReceivedInvoiceCount = filteredInvoices.Count(t => t.StatusId == (int)InvoiceStatus.Received);
                        response.ApprovedInvoiceCount = filteredInvoices.Count(t => t.StatusId == (int)InvoiceStatus.Approved);
                        response.NotApprovedInvoiceCount = filteredInvoices.Count(t => t.StatusId == (int)InvoiceStatus.Rejected && t.InvoiceStatuses.Any(t1 => t1.StatusId == (int)InvoiceStatus.Received));
                        response.UnconfirmedInvoiceCount = filteredInvoices.Count(t => t.StatusId == (int)InvoiceStatus.Unconfirmed);
                        response.TotalInvoiceCount = filteredInvoices.Count;
                        response.WaitingForPriceCount = filteredInvoices.Count(t => t.WaitingFor == (int)WaitingAction.UpdatedPrice);
                        if (allowedInvoiceType == (int)InvoiceType.DigitalDropTicketManual || allowedInvoiceType == (int)InvoiceType.DigitalDropTicketMobileApp)
                        {
                            var approvalCount = await GetBuyerDashboardWaitingForApprovalCountAsync(userId, companyId, jobId, countryId, currency, groupIds);
                            if (approvalCount != null)
                            {
                                response.DropTicketCount = approvalCount.DropTicketCount;
                                response.RejectedDropTicketCount = approvalCount.RejectedDropTicketCount;
                                response.TotalInvoiceCount += approvalCount.DropTicketCount + approvalCount.RejectedDropTicketCount;
                            }
                        }
                    }
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetBuyerDashboardInvoicesAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<DashboardDeliveryStatisticsViewModel> GetBuyerDeliveryStatisticsAsync(int buyerCompanyId, int userId, int selectedJobId = 0, int countryId = (int)Country.All, int currency = (int)Currency.None, string groupIds = "")
        {
            var response = new DashboardDeliveryStatisticsViewModel();
            try
            {
                var deliveries = await new StoredProcedureDomain(this).GetDeliveryStatisticsGrid(buyerCompanyId, selectedJobId, groupIds);
                deliveries.ForEach(
                    del =>
                    response.Deliveries.Add
                    (
                        new DashboardDeliveryStatisticsGridViewModel
                        {
                            GroupingId = del.GroupingId,
                            GroupingName = del.GroupingName,
                            OnTimeDeliveryPercentage = del.OnTimeDeliveryPercentage,
                            LateDeliveryPercentage = del.LateDeliveryPercentage,
                            AverageTimeDelay = del.AverageTimeDelay ?? Resource.lblHyphen,
                            TotalQuantityDelivered = del.TotalQuantityDelivered.GetCommaSeperatedValue(),
                            AverageQuantityPerDelivery = del.AverageQuantityPerDelivery.GetCommaSeperatedValue(),
                            TotalDeliveries = del.TotalDeliveries.GetCommaSeperatedValue()
                        }
                    ));

                if (deliveries != null && deliveries.Any())
                {
                    var averageDeliveryTime = TimeSpan.FromSeconds(deliveries.Average(t => t.AverageDeliveryTime));
                    response.GlobalAverageDeliveryTime = string.Format("{0} {1} {2}", averageDeliveryTime.Hours > 0 ? averageDeliveryTime.ToString(@"hh\h") : string.Empty,
                                                        averageDeliveryTime.Minutes > 0 ? averageDeliveryTime.ToString(@"mm\m") : string.Empty,
                                                        averageDeliveryTime.ToString(@"ss\s"));
                    response.GlobalTotalDeliveries = deliveries.Sum(t => t.TotalDeliveries).GetCommaSeperatedValue();
                    response.GlobalTotalOnTimeDeliveries = deliveries.Sum(t => t.TotalOnTimeDeliveries).GetCommaSeperatedValue();
                    response.GlobalTotalLateDeliveries = deliveries.Sum(t => t.TotalLateDeliveries).GetCommaSeperatedValue();
                    response.GlobalOnTimeDeliveryPercentage = Math.Round(deliveries.Sum(t => t.TotalOnTimeDeliveries) * 100 / deliveries.Sum(t => t.TotalDeliveries), 0, MidpointRounding.AwayFromZero); // AwayFromZero : 3.5 => 4
                    response.GlobalLateDeliveryPercentage = Math.Round(deliveries.Sum(t => t.TotalLateDeliveries) * 100 / deliveries.Sum(t => t.TotalDeliveries), 0); // 2.5 => 2
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "GetBuyerDeliveryStatisticsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<DashboardSupplierOrdersViewModel> GetSupplierDashboardOrdersAsync(int companyId, int countryId, int currency, string groupIds = "")
        {
            using (var tracer = new Tracer("DashboardDomain", "GetSupplierDashboardOrdersAsync"))
            {
                var response = new DashboardSupplierOrdersViewModel();
                try
                {
                    var activeOrders = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetSupplierLastActiveOrders(companyId, countryId, currency, groupIds);
                    response.Last5ActiveOrders = activeOrders.ToList();
                    if (activeOrders != null && activeOrders.Any())
                    {
                        // get total , open , closed count from first record
                        response.TotalOrderCount = activeOrders.First().TotalOrders;
                        response.OpenOrderCount = activeOrders.First().OpenOrders;
                        response.ClosedOrderCount = activeOrders.First().ClosedOrders;
                        response.TotalDrops = activeOrders.First().TotalDrops;
                        response.FiftyPlusPercentageDeliveredOrderCount = activeOrders.First().FiftyPlusPercentDelivered;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetSupplierDashboardOrdersAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<DashboardWaitingApprovalViewModel> GetSupplierDashboardApprovalCountAsync(int companyId, int userId, int countryId, Currency currency)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetSupplierDashboardApprovalCountAsync"))
            {
                var response = new DashboardWaitingApprovalViewModel();
                try
                {
                    Context.DataContext.Database.CommandTimeout = 180;//3 minutes timeout
                    var invoices = Context.DataContext.Invoices
                                                    .Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t.IsBuyPriceInvoice &&
                                                                ((t.OrderId == null && t.User.Company.Id == companyId) ||
                                                                (t.OrderId != null && t.Order.AcceptedCompanyId == companyId) ||
                                                                (t.Order.FuelRequest.User.Company.Id == companyId &&
                                                                     t.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest &&
                                                                     t.Order.BuyerCompanyId == companyId)) &&
                                                                     (countryId == (int)Country.All || (t.Order.FuelRequest.Job.CountryId == countryId && t.Currency == currency)) &&
                                                                (t.InvoiceXInvoiceStatusDetails.OrderByDescending(t1 => t1.Id).FirstOrDefault().StatusId == (int)InvoiceStatus.WaitingForApproval ||
                                                                (t.InvoiceXInvoiceStatusDetails.Any(t1 => t1.StatusId == (int)InvoiceStatus.Rejected) && !t.InvoiceXInvoiceStatusDetails.Any(t1 => t1.StatusId == (int)InvoiceStatus.Received))))
                                                    .Select(t => new { t.InvoiceTypeId, t.InvoiceXInvoiceStatusDetails, t.InvoiceHeader.InvoiceNumber });

                    response.DropTicketCount = await invoices.CountAsync(t => (t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual ||
                                                     t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp) &&
                                                     t.InvoiceXInvoiceStatusDetails.OrderByDescending(t1 => t1.Id).FirstOrDefault().StatusId == (int)InvoiceStatus.WaitingForApproval);

                    response.RejectedDropTicketCount = await invoices.CountAsync(t => (t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual ||
                                                                            t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp) &&
                                                                            (t.InvoiceXInvoiceStatusDetails.Any(t1 => t1.StatusId == (int)InvoiceStatus.Rejected) &&
                                                                            !t.InvoiceNumber.InvoiceHeaderDetails.SelectMany(t4 => t4.Invoices).Any(t2 => t2.InvoiceXInvoiceStatusDetails.Any(t3 => t3.StatusId == (int)InvoiceStatus.Received))));
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetSupplierDashboardApprovalCountAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<DashboardSupplierInvoicesViewModel> GetSupplierDashboardInvoicesAsync(int companyId, int userId, int countryId, Currency currency, string groupIds = "", int allowedInvoiceType = 0)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetSupplierDashboardInvoicesAsync"))
            {
                var response = new DashboardSupplierInvoicesViewModel();
                try
                {
                    var storedProcedureDomain = new StoredProcedureDomain(this);
                    var invoice = await storedProcedureDomain.GetSupplierInvoicesAndDDTForDashboard(companyId, countryId, (int)currency, groupIds);
                    if (invoice != null)
                    {
                        response.TotalInvoiceCount = invoice.Total - invoice.Received;
                        response.UnconfirmedInvoiceCount = invoice.Unconfirmed;
                        response.ApprovedInvoiceCount = invoice.Approved;
                        response.ReceivedInvoiceCount = invoice.Received;
                        response.NotApprovedInvoiceCount = invoice.NotApproved;
                        response.CreatedInvoiceCount = invoice.Created;
                        response.InvoicesFromDropTicketCount = invoice.InvoicesFromDDT;
                        response.WaitingForPriceCount = invoice.WaitingForPrice;


                        response.TotalDDTCount = invoice.TotalDDT - invoice.ReceivedDDT;
                        response.UnconfirmedDDTCount = invoice.UnconfirmedDDT;
                        response.ApprovedDDTCount = invoice.ApprovedDDT;
                        response.ReceivedDDTCount = invoice.ReceivedDDT;
                        response.NotApprovedDDTCount = invoice.NotApprovedDDT;
                        response.CreatedDDTCount = invoice.CreatedDDT;
                        //response.InvoicesFromDropTicketCount = invoice.InvoicesFromDDT; TODO: will be remove
                        response.WaitingForPriceDDTCount = invoice.WaitingForPriceDDT;


                    }
                    if (allowedInvoiceType == (int)InvoiceType.DigitalDropTicketManual || allowedInvoiceType == (int)InvoiceType.DigitalDropTicketMobileApp)
                    {
                        var approvalCount = await GetSupplierDashboardApprovalCountAsync(companyId, userId, countryId, currency);
                        if (approvalCount != null)
                        {
                            response.DropTicketCount = approvalCount.DropTicketCount;
                            response.DropTicketRejectedCount = approvalCount.RejectedDropTicketCount;
                            response.TotalInvoiceCount += approvalCount.DropTicketCount + approvalCount.RejectedDropTicketCount;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetSupplierDashboardInvoicesAsync", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<DashboardSupplierInvoicesViewModel> GetSupplierDashboardDropTicketsAsync(int companyId, int userId, int countryId, Currency currency, string groupIds = "", int allowedInvoiceType = 0)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetSupplierDashboardInvoicesAsync"))
            {
                var response = new DashboardSupplierInvoicesViewModel();
                try
                {
                    var storedProcedureDomain = new StoredProcedureDomain(this);
                    var invoice = await storedProcedureDomain.GetSupplierDashboardDropTickets(companyId, allowedInvoiceType, countryId, (int)currency, groupIds);
                    response.TotalInvoiceCount = invoice.Total - invoice.Received;
                    response.InvoicesFromDropTicketCount = invoice.InvoicesFromDDT;
                    response.CreatedInvoiceCount = invoice.Created;
                    response.DropTicketCount = invoice.DropTicketCount;
                    response.DropTicketRejectedCount = invoice.RejectedDropTicketCount;
                    response.TotalInvoiceCount += invoice.DropTicketCount + invoice.RejectedDropTicketCount;
                    response.WaitingForPriceCount = invoice.WaitingForPrice;
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetSupplierDashboardInvoicesAsync", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<DashboardSupplierFuelRequestGridViewModel> GetSupplierDashboardRecentFRAsync(int companyId, int userId, int countryId, Currency currency)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetSupplierDashboardRecentFRAsync"))
            {
                DashboardSupplierFuelRequestGridViewModel response = new DashboardSupplierFuelRequestGridViewModel();
                try
                {
                    var fuelRequestDomain = new FuelRequestDomain(this);
                    var fuelReruestStat = new USP_SupplierRequestsViewModel()
                    {
                        CompanyId = companyId,
                        UserId = userId,
                        dataTableSearchValues = null,
                        isCallFromDashboard = true,
                        CountryId = countryId,
                        CurrencyType = (int)currency
                    };
                    var fuelRequests = await fuelRequestDomain.GetSupplierFuelReqestGridAsync(fuelReruestStat);

                    response.RecentFuelRequests = fuelRequests.Where(t => t.Status == FuelRequestStatus.Open.ToString()).Take(5).ToList();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetSupplierDashboardRecentFRAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<DashboardSupplierQuoteRequestGridViewModel> GetSupplierDashboardQuotesAsync(int companyId, int userId, int countryId, int currency)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetSupplierDashboardQuotesAsync"))
            {
                var response = new DashboardSupplierQuoteRequestGridViewModel();
                try
                {
                    var quoteRequestDomain = new QuoteRequestDomain(this);
                    var quoteRequestStat = new USP_SupplierRequestsViewModel()
                    {
                        CompanyId = companyId,
                        UserId = userId,
                        dataTableSearchValues = null,
                        isCallFromDashboard = true,
                        CountryId = countryId,
                        CurrencyType = currency
                    };
                    response = await quoteRequestDomain.GetAllQuoteRequestsSupplierAsync(quoteRequestStat, fromDate: new DateTime(2016, 1, 1).ToShortDateString());
                    response.RecentQuoteRequests = response.RecentQuoteRequests.Where(t => t.Status == FuelRequestStatus.Open.ToString()).Take(5).ToList();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetSupplierDashboardQuotesAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<DashboardSupplierGallonsStatViewModel> GetSupplierDashboardGallonsStatAsync(int companyId, int userId, int fuelTypeId, int countryId, int currencyType)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetSupplierDashboardGallonsStatAsync"))
            {
                DashboardSupplierGallonsStatViewModel response = new DashboardSupplierGallonsStatViewModel();
                try
                {
                    var storedProcedureDomain = new StoredProcedureDomain(this);
                    var fuelRequests = await storedProcedureDomain.GetSupplierFuelRequestStatForDashboard(companyId, userId, fuelTypeId, countryId, currencyType);
                    response = GetSupplierDashboardGallonsStat(fuelRequests, fuelTypeId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetSupplierDashboardGallonsStatAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<DashboardJobAveragesViewModel> GetBuyerDashboardJobAvgsAsync(UserContext userContext, int jobId, int fuelTypeId, int countryId = (int)Country.All, Currency currency = Currency.None, string groupIds = "")
        {
            using (var tracer = new Tracer("DashboardDomain", "GetBuyerDashboardJobAvgsAsync"))
            {
                var response = new DashboardJobAveragesViewModel();
                try
                {
                    var helperDomain = new HelperDomain(this);

                    var jobIds = await helperDomain.GetJobIdsAsync(userContext.Id);

                    if (jobIds != null)
                    {
                        var orderCount = Context.DataContext.Orders.Count(t => t.IsActive
                                                                        && t.BuyerCompanyId == userContext.CompanyId
                                                                        && t.FuelRequest.Job.IsActive
                                                                        && (jobId == 0 || t.FuelRequest.Job.Id == jobId)
                                                                        && (fuelTypeId == 0 || t.FuelRequest.FuelTypeId == fuelTypeId)
                                                                        && (countryId == (int)Country.All
                                                                                || (t.FuelRequest.Job.CountryId == countryId
                                                                                && t.FuelRequest.Currency == currency))
                                                                        && jobIds.Contains(t.FuelRequest.Job.Id));

                        response.OrderCount = orderCount;
                        var invoices = Context.DataContext.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                                        && t.Order != null && !t.IsBuyPriceInvoice
                                                                        && t.Order.BuyerCompanyId == userContext.CompanyId
                                                                        && t.Order.FuelRequest.Job.IsActive
                                                                        && (jobId == 0 || t.Order.FuelRequest.Job.Id == jobId)
                                                                        && (fuelTypeId == 0 || t.Order.FuelRequest.FuelTypeId == fuelTypeId)
                                                                        && jobIds.Contains(t.Order.FuelRequest.Job.Id)
                                                                        && (countryId == (int)Country.All
                                                                                || (t.Order.FuelRequest.Job.CountryId == countryId
                                                                                    && t.Currency == currency))
                                                                        && t.Order.FuelRequest.Job.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open)
                                                                        .Select(t => new
                                                                        {
                                                                            InvoiceWorkflowApprovers = t.Order.FuelRequest.Job.JobXApprovalUsers,
                                                                            InvoiceStatuses = t.InvoiceXInvoiceStatusDetails,
                                                                            t.Order.FuelRequest.FuelRequestTypeId,
                                                                            t.OrderId,
                                                                            t.DroppedGallons,
                                                                            PricePerGallon = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail.PricePerGallon).FirstOrDefault(),
                                                                            t.Order.FuelRequest.FuelTypeId,
                                                                            Name = t.Order.FuelRequest.MstProduct.TfxProductId.HasValue ? t.Order.FuelRequest.MstProduct.MstTFXProduct.Name : t.Order.FuelRequest.MstProduct.Name
                                                                        })
                                                                        .ToList();

                        invoices = invoices.Where(t => CheckInvoiceWorkflow(userContext.Id, userContext.IsBuyerAdmin, t.InvoiceWorkflowApprovers, t.InvoiceStatuses, t.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest)).ToList();

                        response.FuelTypes.Add(new DropdownDisplayItem { Id = 0, Name = Resource.lblAll });
                        var distinctList = invoices.GroupBy(t => t.FuelTypeId).Select(t => t.FirstOrDefault()).ToList();

                        response.FuelTypes.AddRange(distinctList.Select(t => new DropdownDisplayItem
                        {
                            Id = t.FuelTypeId,
                            Name = t.Name
                        }).ToList());

                        response.TotalDrops = invoices.Count;
                        if (response.TotalDrops > 0)
                        {
                            response.AverageGallonsPerDrop = (invoices.Sum(t => t.DroppedGallons) / invoices.Count);
                            response.AveragePpgPerDrop = (invoices.Sum(t => t.PricePerGallon) / invoices.Count);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetBuyerDashboardJobAvgsAsync", ex.Message + " companyId: " + userContext.CompanyId + " fuelTypeId: " + fuelTypeId, ex);
                }

                return response;
            }
        }

        public DashboardSupplierGallonsStatViewModel GetSupplierDashboardGallonsStat(List<USP_SupplierFuelRequestStatViewModel> fuelRequests, int fuelTypeId = 0)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetSupplierDashboardGallonsStat"))
            {
                DashboardSupplierGallonsStatViewModel response = new DashboardSupplierGallonsStatViewModel();
                try
                {
                    response.FuelTypes.Add(new DropdownDisplayItem { Id = 0, Name = Resource.lblAll });

                    var distinctList = fuelRequests.GroupBy(t => t.FuelTypeId).Select(t => t.FirstOrDefault()).ToList();

                    response.FuelTypes.AddRange(distinctList.Select(t => new DropdownDisplayItem
                    {
                        Id = t.FuelTypeId,
                        Name = t.FuelType
                    }).ToList());

                    var allStats = fuelRequests.FirstOrDefault();
                    if (allStats != null)
                    {
                        response.AcceptedGallons = allStats.TotalAcceptedGallons;
                        response.ExpiredGallons = allStats.TotalExpiredGallons;
                        response.MissedGallons = allStats.TotalMissedGallons;
                        response.DeliveredGallons = allStats.TotalDeliveredGallons;
                        response.TotalRequestedGallons = allStats.TotalRequestedGallons;

                        response.AcceptedFrCount = allStats.AcceptedFrCount;
                        response.MissedFrCount = allStats.MissedFrCount;
                        response.ExpiredFrCount = allStats.ExpiredFrCount;
                        response.DeclinedFrCount = allStats.DeclinedFrCount;
                        response.CounterOfferCount = allStats.CounterOfferCount;

                        response.BusinessYouWon = allStats.BusinessYouWon;
                        response.BusinessYouMissed = allStats.BusinessYouMissed;
                        response.BusinessInYourArea = allStats.BusinessInYourArea;

                        response.TotalFrCount = allStats.TotalFrCount;

                        response.TotalQrCount = allStats.TotalQrCount;
                        response.OpenQrCount = allStats.OpenQrCount;
                        response.AcceptedQrCount = allStats.AcceptedQrCount;
                        response.DeclinedQrCount = allStats.DeclinedQrCount;
                        response.MissedQrCount = allStats.MissedQrCount;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetSupplierDashboardGallonsStat", ex.Message, ex);
                }

                return response;
            }
        }

        public DashboardSupplierBusinessStatViewModel GetSupplierDashboardBusinessStat(List<FuelRequestGridViewModel> fuelRequests)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetSupplierDashboardBusinessStat"))
            {
                DashboardSupplierBusinessStatViewModel response = new DashboardSupplierBusinessStatViewModel();
                try
                {
                    response.BusinessYouWon = Math.Round(fuelRequests.Where(t => t.Status.Equals(Enum.GetName(typeof(FuelRequestStatus), (int)FuelRequestStatus.Accepted))).Sum(t => t.FrTotalDollarValue));
                    response.BusinessYouMissed = Math.Round(fuelRequests.Where(t => t.Status.Equals(Constants.Missed)).Sum(t => t.FrTotalDollarValue));
                    response.BusinessInYourArea = Math.Round(fuelRequests.Sum(t => t.FrTotalDollarValue));
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetSupplierDashboardBusinessStat", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<DashboardWaitingApprovalViewModel> GetBuyerDashboardWaitingForApprovalCountAsync(int userId, int companyId, int jobId, int countryId, Currency currency, string groupIds = "")
        {
            using (var tracer = new Tracer("DashboardDomain", "GetBuyerDashboardWaitingForApprovalCountAsync"))
            {
                DashboardWaitingApprovalViewModel response = new DashboardWaitingApprovalViewModel();
                try
                {
                    var helperDomain = new HelperDomain(this);
                    var jobIds = await helperDomain.GetJobIdsAsync(userId, groupIds);
                    var groupIdslist = helperDomain.GetGroupList(groupIds);

                    if (jobIds != null)
                    {
                        var companyInvoices = Context.DataContext.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                                                        && t.Order != null && !t.IsBuyPriceInvoice
                                                                                        && ((groupIdslist.Count == 0 && t.Order.BuyerCompanyId == companyId) ||
                                                                                            (groupIdslist.Count > 0 && t.Order.BuyerCompany.SubCompanies.Any(t1 => t1.SubCompanyId == t.Order.BuyerCompanyId && groupIdslist.Contains(t1.CompanyGroupId))))
                                                                                        && (countryId == (int)Country.All || (t.Currency == currency &&
                                                                                            t.Order.FuelRequest.Job.CountryId == countryId))
                                                                                        && (t.InvoiceXInvoiceStatusDetails.OrderByDescending(t1 => t1.Id).FirstOrDefault().StatusId == (int)InvoiceStatus.WaitingForApproval ||
                                                                                            (t.InvoiceXInvoiceStatusDetails.Any(t1 => t1.StatusId == (int)InvoiceStatus.Rejected) && !t.InvoiceXInvoiceStatusDetails.Any(t1 => t1.StatusId == (int)InvoiceStatus.Received))));

                        var invoices = companyInvoices.Where(t => t.Order.FuelRequest.Job.IsActive
                                                                    && (jobId == 0 || t.Order.FuelRequest.Job.Id == jobId)
                                                                    && jobIds.Contains(t.Order.FuelRequest.Job.Id)
                                                                    && t.Order.FuelRequest.Job.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open)
                                                        .Select(t => new
                                                        {
                                                            InvoiceStatuses = t.InvoiceXInvoiceStatusDetails,
                                                            t.InvoiceHeader.InvoiceNumber,
                                                            t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId,
                                                            t.InvoiceTypeId
                                                        })
                                                        .ToList();

                        response.DropTicketCount = invoices.Count(t => (t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual ||
                                                 t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp) &&
                                       t.InvoiceStatuses.OrderByDescending(t1 => t1.Id).FirstOrDefault().StatusId == (int)InvoiceStatus.WaitingForApproval);
                        response.RejectedDropTicketCount = invoices.Count(t => (t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual ||
                                                 t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp) &&
                                        (t.InvoiceStatuses.Any(t1 => t1.StatusId == (int)InvoiceStatus.Rejected) &&
                                        !t.InvoiceNumber.InvoiceHeaderDetails.SelectMany(t4 => t4.Invoices).Any(t2 => t2.InvoiceXInvoiceStatusDetails.Any(t3 => t3.StatusId == (int)InvoiceStatus.Received))));
                    }
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }

                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetBuyerDashboardWaitingForApprovalCountAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<List<CalenderViewModel>> GetSupplierCalenderAsync(Usp_CalenderEventViewModel calEventData, int userId)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetSupplierCalenderAsync"))
            {
                List<CalenderViewModel> response = new List<CalenderViewModel>();
                try
                {
                    var user = await Context.DataContext.Users.Include(t => t.Company).FirstOrDefaultAsync(t => t.IsActive && t.Id == userId);
                    if (user != null && user.Company != null)
                    {
                        var calendarDomain = new CalendarDomain(this);
                        var uspOrderCalendarEvents = await calendarDomain.GetSupplierOrderCalendarEvents(calEventData);

                        var currentDate = DateTimeOffset.Now;
                        var orderEvents = uspOrderCalendarEvents.Where(t => t.CalendarEventType == (int)CalendarEventType.Order);
                        var orderCalenderEvents = GetCalendarOrders(orderEvents, currentDate); //Orders

                        //Single delivery orders as schedules
                        var missedSingleDeliveryOrders = orderEvents.Where(t => t.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery
                                                                                && t.StatusId == (int)OrderStatus.Open && t.Delivered <= 0
                                                                                && t.StartDate.Add(t.EndTime).Subtract(currentDate.ToTargetDateTimeOffset(t.TimeZone)).TotalMinutes < 0);
                        var missedSingleDeliveries = GetDeliveriesFromSingleDeliveryOrders(missedSingleDeliveryOrders, (int)TrackableDeliveryScheduleStatus.Missed);

                        var completedSingleDeliveryOrders = orderEvents.Where(t => t.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery && t.StatusId == (int)OrderStatus.Closed && t.InvoiceId.HasValue);
                        var completedSingleDeliveries = GetDeliveriesFromSingleDeliveryOrders(completedSingleDeliveryOrders, (int)TrackableDeliveryScheduleStatus.Completed);

                        //Past schedules for multiple delivery orders
                        var pastSchedules = GetCalendarSchedules(uspOrderCalendarEvents.Where(t => t.CalendarEventType == (int)CalendarEventType.DeliverySchedule));

                        //Invoices
                        var invoiceCalenderEvents = await calendarDomain.GetSupplierCalenderInvoicesAsync(calEventData);
                        invoiceCalenderEvents.ForEach(t => t.viewableIn = new string[] { Resource.lblCalendarMonthView });

                        response = orderCalenderEvents.Concat(missedSingleDeliveries).Concat(completedSingleDeliveries).Concat(pastSchedules).Concat(invoiceCalenderEvents).ToList();
                        //future schedules
                        if (calEventData.LastDayOfMonth.Subtract(currentDate.Date).Days > (ApplicationConstants.FutureSchedulesAvailableFor - 1))
                        {
                            var futureSchedules = GetCalendarFutureSchedules(calEventData);
                            response = response.Concat(futureSchedules).ToList();
                        }
                        response = AssignParentEventStatus(response);
                        response = response.OrderBy(t => t.calendarEventType).ToList();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetSupplierCalenderAsync", ex.Message, ex);
                }

                return response;
            }
        }

        private List<CalenderViewModel> GetCalendarFutureSchedules(Usp_CalenderEventViewModel calEventData)
        {
            IEnumerable<CalenderViewModel> schedules = new List<CalenderViewModel>();
            try
            {
                var calendarDomain = new CalendarDomain(this);
                var helperDomain = new HelperDomain(this);
                var futureSchedules = calendarDomain.GetCalendarFutureSchedules(calEventData);
                var orders = futureSchedules.GroupBy(t => t.Id);
                DateTimeOffset scheduleDate;
                TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);
                foreach (var orderGroup in orders)
                {
                    var order = orderGroup.First();
                    var jobLocationTime = DateTimeOffset.Now.ToTargetDateTimeOffset(order.TimeZone);
                    DateTime maxExistingTscheduleDate = jobLocationTime.Date.AddDays(ApplicationConstants.FutureSchedulesAvailableFor - 1);
                    var maxDate = order.OrderEndDate ?? DateTimeOffset.MaxValue;
                    List<UspSupplierOrderCalendarEvent> scheduleList = new List<UspSupplierOrderCalendarEvent>();
                    var remainingGallons = order.OrderQuantity - order.Delivered;
                    if (remainingGallons > 0)
                    {
                        foreach (var schedule in orderGroup)
                        {
                            decimal scheduleQuantity = 0;
                            if (schedule.ScheduleType == (int)DeliveryScheduleType.SpecificDates && schedule.StartDate > jobLocationTime.Date && schedule.StartDate <= maxDate.Date && schedule.StartDate > maxExistingTscheduleDate.Date)
                            {
                                scheduleList.Add(schedule);
                            }
                            else if (schedule.ScheduleType != (int)DeliveryScheduleType.SpecificDates)
                            {
                                int addDaysValue = trackableScheduleDomain.GetDaysToAdd(schedule.ScheduleType);
                                if ((schedule.StartDate.Date < jobLocationTime.Date || (schedule.StartDate.Date == jobLocationTime.Date && schedule.EndTime < jobLocationTime.DateTime.TimeOfDay)) || schedule.StartDate != maxExistingTscheduleDate.Date)
                                {
                                    var datediff = Math.Abs(schedule.StartDate.Subtract(maxExistingTscheduleDate).Days) % addDaysValue;
                                    scheduleDate = datediff == 0 ? maxExistingTscheduleDate.AddDays(addDaysValue) : maxExistingTscheduleDate.AddDays(addDaysValue - datediff);
                                }
                                else
                                {
                                    scheduleDate = schedule.StartDate;
                                }
                                while (scheduleQuantity <= remainingGallons && scheduleDate.Date <= maxDate.Date && scheduleDate.Date > maxExistingTscheduleDate.Date)
                                {
                                    if (scheduleDate.Date > calEventData.LastDayOfMonth || scheduleDate.Date.Date <= jobLocationTime.Date)
                                    {
                                        break;
                                    }

                                    var scheduleWithUpdatedDate = new UspSupplierOrderCalendarEvent(schedule, scheduleDate);
                                    scheduleList.Add(scheduleWithUpdatedDate);
                                    scheduleDate = scheduleDate.Date.AddDays(addDaysValue);
                                    scheduleQuantity += schedule.Quantity;
                                }
                            }
                        }

                        scheduleList = scheduleList.OrderBy(t => t.StartDate).ToList();
                        decimal sum = 0;
                        var finalList = (from schedule in scheduleList
                                         where (sum += schedule.Quantity) <= remainingGallons
                                         select schedule).ToList();

                        decimal totalQuantity = finalList.Sum(t => t.Quantity);
                        if (scheduleList.Count > finalList.Count && totalQuantity < remainingGallons)
                        {
                            finalList.Add(scheduleList[finalList.Count]);
                        }
                        var orderSchedules = finalList.Select(t =>
                                                     new CalenderViewModel(Status.Success)
                                                     {
                                                         id = t.Id,
                                                         title = $"{t.CompanyName} {Convert.ToDateTime(t.StartTime.ToString()).ToShortTimeString()} - " +
                                                                 $"{Convert.ToDateTime(t.EndTime.ToString()).ToShortTimeString()} - " +
                                                                 $" {helperDomain.GetQuantityRequested(t.Quantity)} {t.UoM} - " +
                                                                 (t.DriverId == null ? Resource.lblNoDriverAssigned : $"{t.DriverFirstName} {t.DriverLastName}"),
                                                         start = t.StartDate.Date.ToString(Resource.constFormatDate),
                                                         textColor = "#fff",
                                                         calendarEventType = (int)CalendarEventType.DeliverySchedule,
                                                         allDay = true,
                                                         parentStatus = 0,
                                                         subtitle = $"{t.JobCompanyName} " + Resource.lblSingleHyphen
                                                                         + $" {t.PoNumber} "
                                                                         + Resource.lblSingleHyphen
                                                                         + $" {t.JobStateCode} {t.JobZipCode}",
                                                         eventStatus = t.StatusId,
                                                         viewableIn = new List<string> { Resource.lblCalendarDayView }.ToArray()
                                                     });
                        schedules = schedules.Concat(orderSchedules);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "GetCalendarFutureSchedules", ex.Message, ex);
            }
            return schedules.ToList();
        }

        private List<CalenderViewModel> AssignParentEventStatus(List<CalenderViewModel> oldDeliveries)
        {
            var results = oldDeliveries.Where(t => t.calendarEventType != (int)CalendarEventType.Invoice).GroupBy(d => d.start);
            foreach (var group in results)
            {
                if (group.Any(t => t.eventStatus == (int)TrackableDeliveryScheduleStatus.Missed ||
                     t.eventStatus == (int)TrackableDeliveryScheduleStatus.RescheduledMissed ||
                     t.eventStatus == (int)TrackableDeliveryScheduleStatus.Pending ||
                     t.eventStatus == (int)TrackableDeliveryScheduleStatus.Canceled ||
                     (t.orderStatus != null && !t.orderStatus.Equals("open", StringComparison.OrdinalIgnoreCase) && !t.isInvoiceGenerated)))
                {
                    // red color
                    group.ToList().ForEach(t => t.parentStatus = (int)TrackableDeliveryScheduleStatus.Missed);
                }
                else if (group.Any(t => t.eventStatus == (int)TrackableDeliveryScheduleStatus.Canceled || t.eventStatus == (int)TrackableDeliveryScheduleStatus.Rescheduled ||
                                        t.eventStatus == (int)TrackableDeliveryScheduleStatus.MissedAndCanceled || t.eventStatus == (int)TrackableDeliveryScheduleStatus.MissedAndRescheduled))
                {
                    // red color with tick
                    group.ToList().ForEach(t => t.parentStatus = t.eventStatus);
                }
                else if (group.Any(t => t.eventStatus == (int)TrackableDeliveryScheduleStatus.Accepted))
                {
                    // orange color
                    group.ToList().ForEach(t => t.parentStatus = (int)TrackableDeliveryScheduleStatus.Pending);
                }
                else
                {
                    group.ToList().ForEach(t => t.parentStatus = (int)TrackableDeliveryScheduleStatus.Completed);
                }
            }
            return oldDeliveries;
        }

        private async Task<List<Job>> GetBuyerOpenJobsAsync(int userId, int jobId = 0, int countryId = (int)Country.All, Currency currency = Currency.None, string groupIds = "")
        {
            using (var tracer = new Tracer("DashboardDomain", "GetBuyerOpenJobsAsync"))
            {
                List<Job> response;
                HelperDomain helperDomain = new HelperDomain(this);
                List<int> jobIds = await helperDomain.GetJobIdsAsync(userId, groupIds);

                response = Context.DataContext.Jobs.Include(t => t.JobBudget).Include(t => t.MstState)
                                .Include(t => t.MstCountry).Include(t => t.Users1)
                                .Where(t => t.IsActive && (jobId == 0 || t.Id == jobId)
                                && ((countryId == (int)Country.All || t.CountryId == countryId) && (currency == Currency.None || t.Currency == currency)) &&
                                jobIds.Contains(t.Id) && t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open).ToList();

                return response;
            }
        }

        public async Task<DriverMySchedulesViewModel> GetDriverDeliverySchedulesAsync(int companyId, int userId, string startDate, string endDate = "")
        {
            using (var tracer = new Tracer("DashboardDomain", "GetDriverDeliverySchedulesAsync"))
            {
                DriverMySchedulesViewModel response = new DriverMySchedulesViewModel();
                
                try
                {
                    DateTime StartDate = Convert.ToDateTime(startDate);
                    DateTime EndDate = endDate == "" ? DateTimeOffset.Now.DateTime : Convert.ToDateTime(endDate);

                    var user = await Context.DataContext.Users.Include(t => t.Company).FirstOrDefaultAsync(t => t.IsActive && t.Id == userId);
                    if (user != null && user.Company != null)
                    {
                        var orderList = Context.DataContext.Orders.Include(t => t.OrderDeliverySchedules).Include(t => t.OrderXDrivers)
                                                                        .Include("OrderXDrivers.User").Include(t => t.FuelRequest.Job).Include(t => t.FuelRequest.FuelRequest1)
                                                                        .Include("OrderDeliverySchedules.DeliverySchedule").Include("OrderDeliverySchedules.DeliverySchedule.DeliveryScheduleXDrivers")
                                                                        .Include("OrderDeliverySchedules.DeliverySchedule.DeliveryScheduleXDrivers.User")
                                                                        .Include(t => t.FuelRequest).Include(t => t.FuelRequest.FuelRequestDetail)
                                                                        .Include(t => t.FuelRequest.User.Company).Include(t => t.FuelRequest.FuelRequest1.User.Company)
                                                                                    .Where(t => t.IsActive &&
                                                                                                t.AcceptedCompanyId == companyId &&
                                                                                                t.IsEndSupplier &&
                                                                                                t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                                                                                    .ToList();

                        var applocation = Context.DataContext.AppLocations.Where(t => t.UserId == userId).OrderByDescending(t => t.UpdatedDate).FirstOrDefault();
                        //orders without delivery schedules
                        var orders = orderList.Where(t => ((endDate == "" && t.FuelRequest.FuelRequestDetail.StartDate == StartDate) ||
                                                            (endDate != "" && t.FuelRequest.FuelRequestDetail.StartDate >= StartDate && t.FuelRequest.FuelRequestDetail.StartDate <= EndDate))
                                                        && (t.OrderDeliverySchedules.Where(t1 => t1.IsActive) == null || !t.OrderDeliverySchedules.Where(t1 => t1.IsActive).Any())
                                                        && t.OrderXDrivers.Where(t1 => t1.IsActive).Select(t1 => t1.DriverId).Contains(userId))
                                                    .Select(t => new DeliveryScheduleViewModel(Status.Success)
                                                    {
                                                        OrderId = t.Id,
                                                        PONumber = t.PoNumber,
                                                        ScheduleDate = t.FuelRequest.FuelRequestDetail.StartDate,
                                                        StrScheduleDate = t.FuelRequest.FuelRequestDetail.StartDate.ToString(Resource.constFormatDate),
                                                        DeliveryWindow = $"{Convert.ToDateTime(t.FuelRequest.FuelRequestDetail.StartTime.ToString()).ToShortTimeString()} - {Convert.ToDateTime(t.FuelRequest.FuelRequestDetail.EndTime.ToString()).ToShortTimeString()}",
                                                        ScheduleQuantity = t.BrokeredMaxQuantity ?? t.FuelRequest.MaxQuantity,
                                                        FuelType = t.FuelRequest.MstProduct.TfxProductId.HasValue ? t.FuelRequest.MstProduct.MstTFXProduct.Name : t.FuelRequest.MstProduct.Name,
                                                        CustomerName = $"{t.FuelRequest.User.FirstName} {t.FuelRequest.User.LastName}",
                                                        Location = $"{t.FuelRequest.Job.Address} {t.FuelRequest.Job.City} {t.FuelRequest.Job.MstState.Code} {t.FuelRequest.Job.ZipCode}",
                                                        SpecialInstructions = t.FuelRequest.SpecialInstructions.Select(t1 => t1.ToViewModel()).ToList(),
                                                        CustomerLatitude = t.FuelRequest.Job.Latitude,
                                                        CustomerLongitude = t.FuelRequest.Job.Longitude,
                                                        ScheduleStatus = t.Invoices.Any(t1 => t1.IsActive) ? DeliveryScheduleStatus.Completed.ToString() : Resource.lblOpen,
                                                        ContactPerson = t.FuelRequest.Job.PoContactId != null ? GetContactPersonDetails(t.FuelRequest.Job.PoContactId) : "",
                                                        DisplayUoM = t.FuelRequest.UoM.ToString(),
                                                        QuantityTypeId = t.FuelRequest.QuantityTypeId
                                                    }).ToList();


                        if ((endDate == "" && StartDate > DateTimeOffset.Now) || (EndDate > DateTimeOffset.Now))
                        {
                            //orders with delivery schedules
                            var ordersWithDeliverySchedules = orderList.Where(t => t.OrderDeliverySchedules != null && t.OrderDeliverySchedules.Any());
                            decimal percentThreshold, orderAmount, droppedGallons;
                            int addDaysValue = 0;
                            DateTimeOffset scheduleDate, maxDate;
                            TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);
                            foreach (var order in ordersWithDeliverySchedules)
                            {
                                var jobLocationTime = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName);
                                DateTime maxExistingTscheduleDate = jobLocationTime.Date.AddDays(ApplicationConstants.FutureSchedulesAvailableFor - 1);
                                var schedules = order.OrderDeliverySchedules.Select(t => t.DeliverySchedule);
                                maxDate = order.FuelRequest.FuelRequestDetail.EndDate ?? (order.FuelRequest.Job.EndDate ?? DateTimeOffset.MaxValue);
                                List<DeliverySchedule> scheduleList = new List<DeliverySchedule>();
                                percentThreshold = order.FuelRequest.OrderClosingThreshold ?? 100;
                                orderAmount = (percentThreshold * order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity) / 100;
                                droppedGallons = order.Invoices.Where(t => t.IsActiveInvoice && !t.IsBuyPriceInvoice).Sum(t => t.DroppedGallons) + order.DeliveryScheduleXTrackableSchedules.Where(Extensions.IsTrackableScheduleUnDeliveredFunc()).Where(t => t.IsActive && !t.Invoices.Any(t1 => t1.IsActiveInvoice)
                                                                            && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.MissedAndCanceled
                                                                            && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled).Sum(t => t.Quantity);
                                var remainingGallons = orderAmount - droppedGallons;
                                if (remainingGallons > 0)
                                {
                                    foreach (var schedule in schedules)
                                    {
                                        decimal scheduleQuantity = 0;

                                        if (schedule.Type == (int)DeliveryScheduleType.SpecificDates && schedule.Date > jobLocationTime.Date && schedule.Date > maxExistingTscheduleDate.Date &&
                                               ((endDate == "" && schedule.Date == StartDate) || (endDate != "" && schedule.Date >= StartDate && schedule.Date <= EndDate)))
                                        {
                                            scheduleList.Add(schedule);
                                        }
                                        else if (schedule.Type != (int)DeliveryScheduleType.SpecificDates)
                                        {
                                            addDaysValue = trackableScheduleDomain.GetDaysToAdd(schedule.Type);
                                            if ((schedule.Date.Date < jobLocationTime.Date || (schedule.Date.Date == jobLocationTime.Date && schedule.EndTime < jobLocationTime.DateTime.TimeOfDay)) || schedule.Date != maxExistingTscheduleDate.Date)
                                            {
                                                var datediff = Math.Abs(schedule.Date.Subtract(maxExistingTscheduleDate).Days) % addDaysValue;
                                                scheduleDate = datediff == 0 ? maxExistingTscheduleDate.AddDays(addDaysValue) : maxExistingTscheduleDate.AddDays(addDaysValue - datediff);
                                            }
                                            else
                                            {
                                                scheduleDate = schedule.Date;
                                            }
                                            while (scheduleQuantity <= remainingGallons && scheduleDate <= maxDate && scheduleDate.Date > maxExistingTscheduleDate.Date && ((endDate == "" && scheduleDate.Date.Date == StartDate) || (endDate != "" && scheduleDate.Date.Date >= StartDate && scheduleDate.Date.Date <= EndDate)))
                                            {
                                                var scheduleWithUpdatedDate = new DeliverySchedule();
                                                scheduleWithUpdatedDate.Id = schedule.Id;
                                                scheduleWithUpdatedDate.Quantity = schedule.Quantity;
                                                scheduleWithUpdatedDate.Type = schedule.Type;
                                                scheduleWithUpdatedDate.StartTime = schedule.StartTime;
                                                scheduleWithUpdatedDate.EndTime = schedule.EndTime;
                                                scheduleWithUpdatedDate.Date = scheduleDate;
                                                scheduleWithUpdatedDate.DeliveryScheduleXDrivers = schedule.DeliveryScheduleXDrivers;
                                                scheduleWithUpdatedDate.QuantityTypeId = schedule.QuantityTypeId;

                                                scheduleList.Add(scheduleWithUpdatedDate);
                                                scheduleDate = scheduleDate.Date.AddDays(addDaysValue);

                                                scheduleQuantity += schedule.Quantity;
                                            }
                                        }
                                    }
                                    scheduleList = scheduleList.OrderBy(t => t.Date).ToList();
                                    decimal sum = 0;
                                    var finalList = (from schedule in scheduleList
                                                     where (sum += schedule.Quantity) <= remainingGallons
                                                     select schedule).ToList();

                                    decimal totalQuantity = finalList.Sum(t => t.Quantity);
                                    if (scheduleList.Count > finalList.Count && totalQuantity < remainingGallons)
                                    {
                                        finalList.Add(scheduleList[finalList.Count]);
                                    }
                                    finalList = finalList.Where(t => t.DeliveryScheduleXDrivers.Any(t1 => t1.IsActive && t1.DriverId == userId)).ToList();
                                    finalList.ForEach(t => orders.Add(
                                                                new DeliveryScheduleViewModel(Status.Success)
                                                                {
                                                                    OrderId = order.Id,
                                                                    PONumber = order.PoNumber,
                                                                    ScheduleQuantity = t.Quantity,
                                                                    ScheduleDate = t.Date,
                                                                    StrScheduleDate = t.Date.ToString(Resource.constFormatDate),
                                                                    DeliveryWindow = $"{Convert.ToDateTime(t.StartTime.ToString()).ToShortTimeString()} - {Convert.ToDateTime(t.EndTime.ToString()).ToShortTimeString()}",
                                                                    FuelType = order.FuelRequest.MstProduct.TfxProductId.HasValue ? order.FuelRequest.MstProduct.MstTFXProduct.Name : order.FuelRequest.MstProduct.Name,
                                                                    CustomerName = $"{order.FuelRequest.User.FirstName} {order.FuelRequest.User.LastName}",
                                                                    Location = $"{order.FuelRequest.Job.Address} {order.FuelRequest.Job.City} {order.FuelRequest.Job.MstState.Code} {order.FuelRequest.Job.ZipCode}",
                                                                    SpecialInstructions = order.FuelRequest.SpecialInstructions.Select(t1 => t1.ToViewModel()).ToList(),
                                                                    CustomerLatitude = order.FuelRequest.Job.Latitude,
                                                                    CustomerLongitude = order.FuelRequest.Job.Longitude,
                                                                    ScheduleStatus = Resource.lblScheduled,
                                                                    ContactPerson = order.FuelRequest.Job.PoContactId != null ? GetContactPersonDetails(order.FuelRequest.Job.PoContactId) : "",
                                                                    DisplayUoM = t.UoM.ToString(),
                                                                    QuantityTypeId = order.FuelRequest.QuantityTypeId,
                                                                    ScheduleQuantityType = t.QuantityTypeId == null ? ScheduleQuantityType.Quantity : (ScheduleQuantityType)t.QuantityTypeId
                                                                }));
                                }
                            }
                        }

                        // past delivery schedules
                        var oldDeliveries = new List<DeliveryScheduleViewModel>();
                        var pastDeliverySchedules = Context.DataContext.DeliveryScheduleXTrackableSchedules
                                                    .Where(t => t.Order.AcceptedCompanyId == companyId && t.DriverId == userId &&
                                                    t.Order.IsEndSupplier &&
                                                    (
                                                        (endDate == "" && DbFunctions.TruncateTime(t.Date) == StartDate) ||
                                                        (endDate != "" && DbFunctions.TruncateTime(t.Date) >= StartDate && DbFunctions.TruncateTime(t.Date) <= EndDate)
                                                    ) &&
                                                    (
                                                        t.IsActive ||
                                                        t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Missed ||
                                                        t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.RescheduledMissed ||
                                                        t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.MissedAndRescheduled ||
                                                        t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.MissedAndCanceled ||
                                                        t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ||
                                                        t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Canceled
                                                    ) &&
                                                    (
                                                       t.Order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open ||
                                                       t.Invoices.Any(t1 => t1.IsActive && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                    )).ToList();

                        pastDeliverySchedules.ForEach(t => oldDeliveries.Add(new DeliveryScheduleViewModel(Status.Success)
                        {
                            OrderId = t.Order.Id,
                            PONumber = t.Order.PoNumber,
                            ScheduleDate = t.Date,
                            StrScheduleDate = t.Date.ToString(Resource.constFormatDate),
                            ScheduleQuantity = t.Quantity,
                            DeliveryWindow = $"{Convert.ToDateTime(t.StartTime.ToString()).ToShortTimeString()} - {Convert.ToDateTime(t.EndTime.ToString()).ToShortTimeString()}",
                            FuelType = t.Order.FuelRequest.MstProduct.TfxProductId.HasValue ? t.Order.FuelRequest.MstProduct.MstTFXProduct.Name : t.Order.FuelRequest.MstProduct.Name,
                            CustomerName = $"{t.Order.FuelRequest.User.FirstName} {t.Order.FuelRequest.User.LastName}",
                            Location = $"{t.Order.FuelRequest.Job.Address} {t.Order.FuelRequest.Job.City} {t.Order.FuelRequest.Job.MstState.Code} {t.Order.FuelRequest.Job.ZipCode}",
                            SpecialInstructions = t.Order.FuelRequest.SpecialInstructions.Select(t1 => t1.ToViewModel()).ToList(),
                            CustomerLatitude = t.Order.FuelRequest.Job.Latitude,
                            CustomerLongitude = t.Order.FuelRequest.Job.Longitude,
                            ScheduleStatus = t.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Accepted ? Resource.lblScheduled : t.MstDeliveryScheduleStatus.Name,
                            ContactPerson = t.Order.FuelRequest.Job.PoContactId != null ? GetContactPersonDetails(t.Order.FuelRequest.Job.PoContactId) : "",
                            DisplayUoM = t.UoM.ToString(),
                            QuantityTypeId = t.Order.FuelRequest.QuantityTypeId,
                            ScheduleQuantityType = t.QuantityTypeId == null ? ScheduleQuantityType.Quantity : (ScheduleQuantityType)t.QuantityTypeId
                        }));

                        response.deliveryScheduleViewModel = orders.Concat(oldDeliveries).OrderBy(t => t.ScheduleDate).ToList();
                        response.driverDetails.Latitude = applocation != null ? applocation.Latitude : 0;
                        response.driverDetails.Longitude = applocation != null ? applocation.Longitude : 0;
                        response.driverDetails.DriverName = $"{user.FirstName} {user.LastName}";

                        LatLongDetailsViewModel latLongDetails;
                        foreach (var item in response.deliveryScheduleViewModel)
                        {
                            latLongDetails = new LatLongDetailsViewModel();
                            latLongDetails.Latitude = item.CustomerLatitude;
                            latLongDetails.Longitude = item.CustomerLongitude;
                            response.latLongDetails.Add(latLongDetails);
                        }

                        if (response.driverDetails.Latitude != 0 && response.driverDetails.Longitude != 0)
                        {
                            response.latLongDetails.Add(new LatLongDetailsViewModel
                            {
                                Latitude = response.driverDetails.Latitude,
                                Longitude = response.driverDetails.Longitude
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetDriverDeliverySchedulesAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<List<CalenderViewModel>> GetDriverCalendarDataAsync(int companyId, int userId, int month, int year, int day)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetDriverCalendarDataAsync"))
            {
                List<CalenderViewModel> response = new List<CalenderViewModel>();
                var helperDomain = new HelperDomain(this);
                try
                {
                    var firstDayOfMonth = new DateTime(year == 0 ? DateTime.Now.Year : year, month == 0 ? DateTime.Now.Month : month, day);
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    var lastDayVisible = firstDayOfMonth.AddMonths(1).AddDays(4);

                    var user = await Context.DataContext.Users.Include(t => t.Company).FirstOrDefaultAsync(t => t.IsActive && t.Id == userId);
                    if (user != null && user.Company != null)
                    {
                        var orderList = Context.DataContext.Orders.Include(t => t.OrderDeliverySchedules).Include(t => t.OrderXDrivers)
                                                                        .Include("OrderXDrivers.User").Include(t => t.FuelRequest.Job).Include(t => t.FuelRequest.FuelRequest1)
                                                                        .Include("OrderDeliverySchedules.DeliverySchedule").Include("OrderDeliverySchedules.DeliverySchedule.DeliveryScheduleXDrivers")
                                                                        .Include("OrderDeliverySchedules.DeliverySchedule.DeliveryScheduleXDrivers.User")
                                                                        .Include(t => t.FuelRequest).Include(t => t.FuelRequest.FuelRequestDetail)
                                                                        .Include(t => t.FuelRequest.User.Company).Include(t => t.FuelRequest.FuelRequest1.User.Company)
                                                                                    .Where(t => t.IsActive &&
                                                                                                t.AcceptedCompanyId == companyId &&
                                                                                                t.IsEndSupplier &&
                                                                                                t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                                                                                    .ToList();

                        //orders without delivery schedules
                        var orders = orderList.Where(t => (t.OrderDeliverySchedules == null || !t.OrderDeliverySchedules.Any()) &&
                                                     t.OrderXDrivers.Where(t1 => t1.IsActive).Select(t1 => t1.DriverId).Contains(userId))
                                                    .Select(t => new CalenderViewModel(Status.Success)
                                                    {
                                                        id = t.Id,
                                                        title = $"{(t.FuelRequest.User.Company.CompanyTypeId == (int)CompanyType.Supplier ? t.FuelRequest.FuelRequest1.User.Company.Name : t.FuelRequest.User.Company.Name)} " +
                                                                $"{Convert.ToDateTime(t.FuelRequest.FuelRequestDetail.StartTime.ToString()).ToShortTimeString()} - " +
                                                                $"{Convert.ToDateTime(t.FuelRequest.FuelRequestDetail.EndTime.ToString()).ToShortTimeString()}" +
                                                                (t.OrderXDrivers.SingleOrDefault(t1 => t1.IsActive) == null ? string.Empty : $" - {t.OrderXDrivers.Single(t1 => t1.IsActive).User.FirstName} {t.OrderXDrivers.Single(t1 => t1.IsActive).User.LastName}"),
                                                        start = t.FuelRequest.FuelRequestDetail.StartDate.ToString(Resource.constFormatDate),
                                                        textColor = "#f6a344",
                                                        calendarEventType = (int)CalendarEventType.Order,
                                                        allDay = true,
                                                        parentStatus = 0,
                                                        eventStatus = t.DeliveryScheduleXTrackableSchedules.FirstOrDefault(t2 => t2.OrderId == t.Id) == null ? (int)DeliveryScheduleStatus.Pending : t.DeliveryScheduleXTrackableSchedules.FirstOrDefault(t2 => t2.OrderId == t.Id).DeliveryScheduleStatusId,
                                                        viewableIn = new List<string> { Resource.lblCalendarDayView }.ToArray()
                                                    }).ToList();

                        //orders with delivery schedules
                        var ordersWithDeliverySchedules = orderList.Where(t => t.OrderDeliverySchedules != null && t.OrderDeliverySchedules.Any());
                        decimal percentThreshold, orderAmount, droppedGallons;
                        int addDaysValue = 0;
                        DateTimeOffset scheduleDate, maxDate;
                        TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);

                        foreach (var order in ordersWithDeliverySchedules)
                        {
                            var jobLocationTime = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName);
                            DateTime maxExistingTscheduleDate = jobLocationTime.Date.AddDays(ApplicationConstants.FutureSchedulesAvailableFor - 1);
                            var schedules = order.OrderDeliverySchedules.Select(t => t.DeliverySchedule);
                            maxDate = order.FuelRequest.FuelRequestDetail.EndDate ?? (order.FuelRequest.Job.EndDate ?? DateTimeOffset.MaxValue);
                            List<DeliverySchedule> scheduleList = new List<DeliverySchedule>();
                            percentThreshold = order.FuelRequest.OrderClosingThreshold ?? 100;
                            orderAmount = (percentThreshold * order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity) / 100;
                            droppedGallons = order.Invoices.Where(t => t.IsActiveInvoice && !t.IsBuyPriceInvoice).Sum(t => t.DroppedGallons) + order.DeliveryScheduleXTrackableSchedules.Where(Extensions.IsTrackableScheduleUnDeliveredFunc()).Where(t => t.IsActive && !t.Invoices.Any(t1 => t1.IsActiveInvoice)
                                                                        && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.MissedAndCanceled
                                                                        && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled).Sum(t => t.Quantity);
                            var remainingGallons = orderAmount - droppedGallons;
                            if (remainingGallons > 0)
                            {
                                foreach (var schedule in schedules)
                                {
                                    decimal scheduleQuantity = 0;

                                    if (schedule.Type == (int)DeliveryScheduleType.SpecificDates && schedule.Date > jobLocationTime.Date && schedule.Date <= maxDate.Date && schedule.Date > maxExistingTscheduleDate.Date)
                                    {
                                        scheduleList.Add(schedule);
                                    }
                                    else if (schedule.Type != (int)DeliveryScheduleType.SpecificDates)
                                    {
                                        addDaysValue = trackableScheduleDomain.GetDaysToAdd(schedule.Type);
                                        if ((schedule.Date.Date < jobLocationTime.Date || (schedule.Date.Date == jobLocationTime.Date && schedule.EndTime < jobLocationTime.DateTime.TimeOfDay)) || schedule.Date != maxExistingTscheduleDate.Date)
                                        {
                                            var datediff = Math.Abs(schedule.Date.Subtract(maxExistingTscheduleDate).Days) % addDaysValue;
                                            scheduleDate = datediff == 0 ? maxExistingTscheduleDate.AddDays(addDaysValue) : maxExistingTscheduleDate.AddDays(addDaysValue - datediff);
                                        }
                                        else
                                        {
                                            scheduleDate = schedule.Date;
                                        }
                                        while (scheduleQuantity <= remainingGallons && scheduleDate.Date <= maxDate.Date && scheduleDate.Date > maxExistingTscheduleDate.Date)
                                        {
                                            if (scheduleDate.Date > lastDayOfMonth || scheduleDate.Date.Date <= jobLocationTime.Date)
                                            {
                                                break;
                                            }

                                            var scheduleWithUpdatedDate = new DeliverySchedule();
                                            scheduleWithUpdatedDate.Id = schedule.Id;
                                            scheduleWithUpdatedDate.Quantity = schedule.Quantity;
                                            scheduleWithUpdatedDate.Type = schedule.Type;
                                            scheduleWithUpdatedDate.StartTime = schedule.StartTime;
                                            scheduleWithUpdatedDate.EndTime = schedule.EndTime;
                                            scheduleWithUpdatedDate.Date = scheduleDate;
                                            scheduleWithUpdatedDate.DeliveryScheduleXDrivers = schedule.DeliveryScheduleXDrivers;

                                            scheduleList.Add(scheduleWithUpdatedDate);
                                            scheduleDate = scheduleDate.Date.AddDays(addDaysValue);

                                            scheduleQuantity += schedule.Quantity;
                                        }
                                    }
                                }
                                scheduleList = scheduleList.OrderBy(t => t.Date).ToList();
                                decimal sum = 0;
                                var finalList = (from schedule in scheduleList
                                                 where (sum += schedule.Quantity) <= remainingGallons
                                                 select schedule).ToList();

                                decimal totalQuantity = finalList.Sum(t => t.Quantity);
                                if (scheduleList.Count > finalList.Count && totalQuantity < remainingGallons)
                                {
                                    finalList.Add(scheduleList[finalList.Count]);
                                }
                                finalList = finalList.Where(t => t.DeliveryScheduleXDrivers.Any(t1 => t1.IsActive && t1.DriverId == userId)).ToList();
                                finalList.ForEach(t => orders.Add(
                                                            new CalenderViewModel(Status.Success)
                                                            {
                                                                id = order.Id,
                                                                title = $"{(order.FuelRequest.User.Company.CompanyTypeId == (int)CompanyType.Supplier ? order.FuelRequest.FuelRequest1.User.Company.Name : order.FuelRequest.User.Company.Name)} " +
                                                                        $"{Convert.ToDateTime(t.StartTime.ToString()).ToShortTimeString()} - " +
                                                                        $"{Convert.ToDateTime(t.EndTime.ToString()).ToShortTimeString()} - " +
                                                                        $" {helperDomain.GetQuantityRequested(t.Quantity)} {t.UoM} - " +
                                                                            (t.DeliveryScheduleXDrivers.SingleOrDefault(t1 => t1.IsActive) == null ? Resource.lblNoDriverAssigned : $"{t.DeliveryScheduleXDrivers.Single(t1 => t1.IsActive).User.FirstName} {t.DeliveryScheduleXDrivers.Single(t1 => t1.IsActive).User.LastName}"),
                                                                start = t.Date.Date.ToString(Resource.constFormatDate),
                                                                textColor = "#f6a344",

                                                                calendarEventType = (int)CalendarEventType.DeliverySchedule,
                                                                allDay = true,
                                                                parentStatus = 0,
                                                                subtitle = $"{order.FuelRequest.Job.Company.Name} " + Resource.lblSingleHyphen
                                                                                + $" {order.PoNumber} "
                                                                                + Resource.lblSingleHyphen
                                                                                + $" {order.FuelRequest.Job.MstState.Code} {order.FuelRequest.Job.ZipCode}",
                                                                eventStatus = (int)DeliveryScheduleStatus.Accepted,
                                                                viewableIn = new List<string> { Resource.lblCalendarDayView }.ToArray()
                                                            }));
                            }
                        }
                        // past delivery schedules
                        var oldDeliveries = new List<CalenderViewModel>();
                        var pastDeliverySchedules = Context.DataContext.DeliveryScheduleXTrackableSchedules
                                                    .Where(t => t.OrderId != null && t.Order.AcceptedCompanyId == companyId && t.DriverId == userId &&
                                                    t.Order.IsEndSupplier &&
                                                    DbFunctions.TruncateTime(t.Date) >= firstDayOfMonth &&
                                                    DbFunctions.TruncateTime(t.Date) <= lastDayVisible &&
                                                    (
                                                        t.IsActive ||
                                                        t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Missed ||
                                                        t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.RescheduledMissed ||
                                                        t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.MissedAndRescheduled ||
                                                        t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.MissedAndCanceled ||
                                                        t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ||
                                                        t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Canceled
                                                    ) &&
                                                    (
                                                       t.Order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open ||
                                                       t.Invoices.Any(t1 => t1.IsActive && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                    )).ToList();

                        pastDeliverySchedules.ForEach(t => oldDeliveries.Add(new CalenderViewModel(Status.Success)
                        {
                            id = t.Invoices.Any(t1 => t1.IsActiveInvoice) ? t.Invoices.First(t1 => t1.IsActiveInvoice).Id : t.OrderId ?? 0,
                            isInvoiceGenerated = t.Invoices.Any(t1 => t1.IsActiveInvoice),
                            title = $"{Convert.ToDateTime(t.DeliverySchedule.StartTime.ToString()).ToShortTimeString()} " + Resource.lblSingleHyphen +
                                        $" {Convert.ToDateTime(t.DeliverySchedule.EndTime.ToString()).ToShortTimeString()} " + Resource.lblSingleHyphen +
                                        $" {t.Quantity.GetCommaSeperatedValue()} {t.UoM} " + Resource.lblSingleHyphen +
                                        (t.DeliverySchedule.DeliveryScheduleXDrivers.FirstOrDefault(t1 => t1.IsActive) != null ? $" {t.DeliverySchedule.DeliveryScheduleXDrivers.FirstOrDefault(t1 => t1.IsActive).User.FirstName} {t.DeliverySchedule.DeliveryScheduleXDrivers.FirstOrDefault(t1 => t1.IsActive).User.LastName}" : $" No Driver Assigned"),
                            start = t.Date.ToString(Resource.constFormatDate),
                            textColor = "#fff",
                            calendarEventType = (int)CalendarEventType.DeliverySchedule,
                            allDay = true,
                            subtitle = $"{t.Order.FuelRequest.Job.Company.Name} " + Resource.lblSingleHyphen
                                        + $" {t.Order.PoNumber} "
                                        + Resource.lblSingleHyphen
                                        + $" {t.Order.FuelRequest.Job.MstState.Code} {t.Order.FuelRequest.Job.ZipCode}",
                            eventStatus = (t.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Rescheduled && t.IsActive) ?
                                        (int)DeliveryScheduleStatus.Accepted : t.DeliveryScheduleStatusId,
                            viewableIn = new List<string> { Resource.lblCalendarDayView }.ToArray()
                        }));

                        response = AssignParentEventStatus(orders.Concat(oldDeliveries).ToList());
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetDriverCalendarDataAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<DashboardSuperAdminCountViewModel> GetSuperAdminCountAsync()
        {
            var response = new DashboardSuperAdminCountViewModel();
            try
            {
                var superAdmins = await Context.DataContext.Users.Where(t => t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.SuperAdmin)).ToListAsync();
                response.TotalSuperAdminCount = superAdmins.Count;
                response.TotalActiveSuperAdminCount = superAdmins.Count(t => t.IsActive);
                response.TotalInactiveSuperAdminCount = superAdmins.Count(t => !t.IsActive);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "GetSuperAdminCountAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<DashboardSuperAdminCompanyCountViewModel> GetSuperAdminCompanyCountAsync()
        {
            var response = new DashboardSuperAdminCompanyCountViewModel();
            try
            {
                var companies = Context.DataContext.Companies.Where(t => !t.Name.Contains("-ToBe-Delete"));
                response.TotalCompanyCount = await companies.CountAsync();
                response.TotalBuyerCompanyCount = await companies.CountAsync(t => t.CompanyTypeId == (int)CompanyType.Buyer);
                response.TotalSupplierCompanyCount = await companies.CountAsync(t => t.CompanyTypeId == (int)CompanyType.Supplier);
                response.TotalBuyerAndSupplierCompanyCount = await companies.CountAsync(t => t.CompanyTypeId == (int)CompanyType.BuyerAndSupplier);
                response.TotalDriverCompanyCount = await companies.CountAsync(t => t.CompanyTypeId == (int)CompanyType.Carrier);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "GetSuperAdminCompanyCountAsync", ex.Message, ex);
            }
            return response;
        }

        private bool CheckInvoiceWorkflow(int userId, bool isBuyerAdmin, ICollection<JobXApprovalUser> approvers, ICollection<InvoiceXInvoiceStatusDetail> status, bool isBrokeredRequest)
        {
            var response = true;
            if (!isBrokeredRequest)
            {
                var approvalUser = approvers.FirstOrDefault(t => t.IsActive);
                var invoiceStatus = status.FirstOrDefault(t => t.IsActive);
                if (approvalUser != null && invoiceStatus != null)
                {
                    if (invoiceStatus.StatusId == (int)InvoiceStatus.WaitingForApproval && approvalUser.Id != userId && !isBuyerAdmin)
                    {
                        response = false;
                    }
                    if (invoiceStatus.StatusId == (int)InvoiceStatus.Rejected)
                    {
                        invoiceStatus = status.OrderByDescending(t => t.Id).FirstOrDefault(t => t.StatusId != (int)InvoiceStatus.Rejected);
                        if (invoiceStatus != null && invoiceStatus.StatusId == (int)InvoiceStatus.WaitingForApproval && approvalUser.Id != userId && !isBuyerAdmin)
                        {
                            response = false;
                        }
                    }
                }
            }
            return response;
        }

        private bool CheckInvoiceWorkflowForCompanyGroup(List<int> userIds, bool isBuyerAdmin, ICollection<JobXApprovalUser> approvers, ICollection<InvoiceXInvoiceStatusDetail> status, bool isBrokeredRequest)
        {
            var response = true;
            if (!isBrokeredRequest)
            {
                var approvalUser = approvers.FirstOrDefault(t => t.IsActive);
                var invoiceStatus = status.FirstOrDefault(t => t.IsActive);
                if (approvalUser != null && invoiceStatus != null)
                {
                    if (invoiceStatus.StatusId == (int)InvoiceStatus.WaitingForApproval && !userIds.Contains(approvalUser.Id) && !isBuyerAdmin)
                    {
                        response = false;
                    }
                    if (invoiceStatus.StatusId == (int)InvoiceStatus.Rejected)
                    {
                        invoiceStatus = status.OrderByDescending(t => t.Id).FirstOrDefault(t => t.StatusId != (int)InvoiceStatus.Rejected);
                        if (invoiceStatus != null && invoiceStatus.StatusId == (int)InvoiceStatus.WaitingForApproval && !userIds.Contains(approvalUser.Id) && !isBuyerAdmin)
                        {
                            response = false;
                        }
                    }
                }
            }
            return response;
        }

        public async Task<DashboardCompanyUsersCountViewModel> GetCompanyUsersCountAsync(int SelectedCompanyId)
        {
            var response = new DashboardCompanyUsersCountViewModel();
            try
            {
                var companyUsers = Context.DataContext.Users.Where(t => t.MstRoles.Any(t1 => t1.Id != (int)UserRoles.SuperAdmin && t1.Id != (int)UserRoles.InternalSalesPerson));
                if (SelectedCompanyId > 0)
                {
                    companyUsers = companyUsers.Where(t => t.Company.Id == SelectedCompanyId);
                }
                response.TotalCompanyUsersCount = await companyUsers.CountAsync();
                response.TotalActiveCompanyUsersCount = await companyUsers.CountAsync(t => t.IsActive);
                response.TotalInactiveCompanyUsersCount = await companyUsers.CountAsync(t => !t.IsActive);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "GetCompanyUsersCountAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<DashboardSuperAdminAdvertisementCountViewModel> GetSuperAdminAdWidgetCountAsync()
        {
            var response = new DashboardSuperAdminAdvertisementCountViewModel();
            try
            {
                var requestFuels = Context.DataContext.RequestFuels;
                response.TotalRequestPriceCount = await Context.DataContext.RequestPrices.CountAsync();
                response.TotalRequestFuelCount = await requestFuels.CountAsync();
                response.TotalCustomersContactedCount = await requestFuels.CountAsync(t => t.IsCustomerContacted && t.CustomerContactedDateTime != null);
                response.TotalBusinessDoneCompanyCount = await requestFuels.CountAsync(t => t.IsBusinessDone);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "GetSuperAdminAdWidgetCountAsync", ex.Message, ex);
            }
            return response;
        }

        public DashboardTotalGallonsCountViewModel GetGallonsOrderedCount(string startDate, string endDate, int selectedCompanyId)
        {
            DashboardTotalGallonsCountViewModel response = new DashboardTotalGallonsCountViewModel();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                response = storedProcedureDomain.GetGallonsOrderedCount(selectedCompanyId, startDate, endDate);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "GetGallonsOrderedCount", ex.Message, ex);
            }

            return response;
        }

        public DashboardTotalGallonsCountViewModel GetGallonsDeliveredCount(string startDate, string endDate, int selectedCompanyId)
        {
            DashboardTotalGallonsCountViewModel response = new DashboardTotalGallonsCountViewModel();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                response = storedProcedureDomain.GetGallonsDeliveredCount(selectedCompanyId, startDate, endDate);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "GetGallonsDeliveredCount", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetCustomers(int companyId, bool isDefaultOption = false, int driverId = -1, int countryId = (int)Country.USA, Currency currency = Currency.USD)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetCustomers"))
            {
                var response = new List<DropdownDisplayItem>();

                try
                {
                    if (isDefaultOption)
                    {
                        response.Add(new DropdownDisplayItem { Id = 0, Name = "All Customers" });
                    }
                    var orders = Context.DataContext.Orders.Where(t => t.IsActive && t.AcceptedCompanyId == companyId && t.IsEndSupplier &&
                                       t.FuelRequest.Currency == currency && t.FuelRequest.Job.CountryId == countryId &&
                   (
                       driverId == -1 ||
                       t.OrderDeliverySchedules.Any(t1 => t1.IsActive && t1.DeliverySchedule.DeliveryScheduleXDrivers.Any(t3 => t3.DriverId == driverId && t3.IsActive)) ||
                       t.OrderXDrivers.Any(t1 => t1.DriverId == driverId && t1.IsActive) ||
                       t.DeliveryScheduleXTrackableSchedules.Any(t1 => t1.DriverId == driverId && t1.IsActive)
                   ));

                    var orderList = await orders.GroupBy(p => p.BuyerCompanyId).Select(grp => grp.FirstOrDefault()).Select(x =>
                    new { x.BuyerCompanyId, x.BuyerCompany.Name }).ToListAsync();
                    orderList.ForEach(t => response.Add(new DropdownDisplayItem()
                    {
                        Id = t.BuyerCompanyId,
                        Name = t.Name
                    }));
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetCustomers", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<List<DropdownDisplayItem>> GetCustomerOrders(int userCompanyId, int driverId, int customerCompanyId, int countryId = (int)Country.USA, Currency currency = Currency.USD)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetCustomerOrders"))
            {
                var response = new List<DropdownDisplayItem>();

                try
                {
                    var orderVersions = await Context.DataContext.OrderVersionXDeliverySchedules
                                .Where(t => t.IsActive && t.Order.AcceptedCompanyId == userCompanyId && t.Order.IsEndSupplier
                                            && t.Order.FuelRequest.Currency == currency && t.Order.FuelRequest.Job.CountryId == countryId
                                            && (customerCompanyId == 0 || t.Order.BuyerCompanyId == customerCompanyId)
                                            && (driverId == -1 || t.Order.OrderXDrivers.Any(t1 => t1.IsActive && t1.DriverId == driverId)
                                            || t.DeliverySchedule.DeliveryScheduleXDrivers.Any(t2 => t2.IsActive && t2.DriverId == driverId))
                                       ).Select(x => new { x.OrderId, x.Order.PoNumber }).Distinct().ToListAsync();

                    orderVersions.ForEach(t => response.Add(new DropdownDisplayItem()
                    {
                        Id = t.OrderId,
                        Name = t.PoNumber
                    }));
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetCustomerOrders", ex.Message, ex);
                }

                return response;
            }
        }

        private string GetContactPersonDetails(int? userId)
        {
            string contactPerson = string.Empty;
            try
            {
                var user = Context.DataContext.Users.Where(t => t.Id == userId).FirstOrDefault();
                if (user != null)
                {
                    contactPerson = $"{user.FirstName}{user.LastName}<br/>{user.PhoneNumber}<br/>{user.Email}";
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "GetContactPersonDetails", ex.Message, ex);
            }
            return contactPerson;
        }
        public int GetMissedSchedulesCount(int driverId)
        {
            var missedSchedulesCnt = 0;
            try
            {
                missedSchedulesCnt = Context.DataContext.DeliveryScheduleXTrackableSchedules.Count(t => t.DriverId == driverId && (t.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Missed || t.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.RescheduledMissed));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "GetMissedSchedulesCount", ex.Message, ex);
            }
            return missedSchedulesCnt;
        }

        private List<CalenderViewModel> GetDeliveriesFromSingleDeliveryOrders(IEnumerable<UspSupplierOrderCalendarEvent> events, int statusId)
        {
            var response = events.Select(t => new CalenderViewModel(Status.Success)
            {
                id = t.InvoiceId.HasValue ? t.InvoiceId.Value : t.Id,
                isInvoiceGenerated = t.InvoiceId.HasValue,
                title = $"{Convert.ToDateTime(t.StartTime.ToString()).ToShortTimeString()} " + Resource.lblSingleHyphen +
                                        $" {Convert.ToDateTime(t.EndTime.ToString()).ToShortTimeString()} " + Resource.lblSingleHyphen +
                                        $" {t.Delivered.GetPreciseValue(2).GetCommaSeperatedValue()} {t.UoM} " + Resource.lblSingleHyphen + " " +
                                        (t.DriverId.HasValue ? $"{t.DriverFirstName} {t.DriverLastName}" : Resource.lblNoDriverAssigned),
                start = t.StartDate.ToString(Resource.constFormatDate),
                textColor = "#fff",
                calendarEventType = (int)CalendarEventType.DeliverySchedule,
                allDay = true,
                subtitle = $"{t.CompanyName} {Resource.lblSingleHyphen} {t.PoNumber} {Resource.lblSingleHyphen} {t.JobStateCode} {t.JobZipCode}",
                eventStatus = statusId,
                viewableIn = new List<string> { Resource.lblCalendarDayView }.ToArray()
            }).ToList();

            return response;
        }

        private List<CalenderViewModel> GetCalendarSchedules(IEnumerable<UspSupplierOrderCalendarEvent> events)
        {
            var helperDomain = new HelperDomain(this);
            var response = events.Select(t => new CalenderViewModel(Status.Success)
            {
                id = t.InvoiceId.HasValue ? t.InvoiceId.Value : t.Id,
                isInvoiceGenerated = t.InvoiceId.HasValue,
                title = $"{Convert.ToDateTime(t.StartTime.ToString()).ToShortTimeString()} " + Resource.lblSingleHyphen +
                                    $" {Convert.ToDateTime(t.EndTime.ToString()).ToShortTimeString()} " + Resource.lblSingleHyphen +
                                    $" {helperDomain.GetQuantityRequested(t.Quantity)} {t.UoM} " + Resource.lblSingleHyphen + " " +
                                    (t.DriverId.HasValue ? $"{t.DriverFirstName} {t.DriverLastName}" : Resource.lblNoDriverAssigned),
                start = t.StartDate.ToString(Resource.constFormatDate),
                textColor = "#fff",
                calendarEventType = (int)CalendarEventType.DeliverySchedule,
                allDay = true,
                subtitle = $"{t.CompanyName} {Resource.lblSingleHyphen} {t.PoNumber} {Resource.lblSingleHyphen} {t.JobStateCode} {t.JobZipCode}",
                eventStatus = t.StatusId,
                viewableIn = new List<string> { Resource.lblCalendarDayView }.ToArray()
            }).ToList();

            return response;
        }

        private List<CalenderViewModel> GetCalendarOrders(IEnumerable<UspSupplierOrderCalendarEvent> events, DateTimeOffset currentDate)
        {
            var helperDomain = new HelperDomain(this);
            var response = events.Select(t => new CalenderViewModel(Status.Success)
            {
                id = t.Id,
                title = helperDomain.GetQuantityRequested(t.Quantity) == Resource.lblNotSpecified ?
                                      $"{t.PoNumber} {Resource.lblSingleHyphen} " + $"{Resource.lblNotSpecified} "
                                      :
                                      $"{t.PoNumber} {Resource.lblSingleHyphen} " +
                                      $"{helperDomain.GetQuantityRequested(t.Quantity)} {t.UoM} {Resource.lblSingleHyphen} " +
                                      $"{Convert.ToInt32(t.Delivered / t.Quantity * 100)}% {Resource.lblCompleted}",
                subtitle = $"{t.StartDate.ToString(Resource.constFormatDate)} {Resource.lblSingleHyphen} " +
                                      $"{t.CompanyName} {Resource.lblSingleHyphen} {t.JobStateCode} {t.JobZipCode} {Resource.lblSingleHyphen} {(t.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery ? ApplicationConstants.SingleDelivery : ApplicationConstants.MultipleDelivery)}",
                isInvoiceGenerated = t.InvoiceId.HasValue,
                textColor = "#fff",
                orderStatus = t.OrderStatus,
                start = t.StartDate.ToString(Resource.constFormatDate),
                calendarEventType = (int)CalendarEventType.Order,
                allDay = true,
                eventStatus = t.StartDate.Add(t.EndTime) < currentDate.ToTargetDateTimeOffset(t.TimeZone) && t.Delivered <= 0 ?
                                (int)TrackableDeliveryScheduleStatus.Missed : (t.StatusId == (int)OrderStatus.Closed && t.InvoiceId.HasValue ?
                                (int)TrackableDeliveryScheduleStatus.Completed : (int)TrackableDeliveryScheduleStatus.Accepted),
                viewableIn = new List<string> { Resource.lblCalendarDayView }.ToArray()
            }).ToList();

            return response;
        }

        public async Task<List<UspSupplierPerformanceViewModel>> GetYourSuppliersPerformanceData(UserContext userContext)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetBuyerDashboardOrdersAsync"))
            {
                var response = new List<UspSupplierPerformanceViewModel>();
                try
                {
                    response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetYourSuppliersPerformanceData(userContext.CompanyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetBuyerDashboardOrdersAsync", ex.Message + " -companyId: " + userContext.CompanyId, ex);
                }

                return response;
            }
        }

        public async Task<List<UspBuyerPerformanceViewModel>> GetYourBuyersPerformanceData(UserContext userContext)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetYourBuyerPerformanceData"))
            {
                var response = new List<UspBuyerPerformanceViewModel>();                
                try
                {
                    response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetYourBuyerPerformanceData(userContext.CompanyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetYourBuyerPerformanceData", ex.Message + " -companyId: " + userContext.CompanyId, ex);
                }
                return response;
            }
        }
        public async Task<List<UspBuyerPerformanceViewModel>> setAccountingCompanyId(List<UspBuyerPerformanceViewModel> buyerperformancedata, int supplierCompanyId)
        {
            var accCompanyIds = await Context.DataContext.SupplierXBuyerDetails.Where(x => x.SupplierCompanyId == supplierCompanyId)
                                                      .Select(x1 => new
                                                      {
                                                          BuyerCompanyId = x1.BuyerCompanyId,
                                                          AccountingCompanyId = x1.AccountingCompanyId
                                                      }).ToListAsync();
            if (accCompanyIds.Count > 0 && accCompanyIds != null)
            {
                foreach (var buyerdata in buyerperformancedata)
                {
                    foreach (var accCompId in accCompanyIds)
                    {
                        if (buyerdata.BuyerCompanyId == accCompId.BuyerCompanyId)
                        {

                            if (accCompId.AccountingCompanyId != null || accCompId.AccountingCompanyId != string.Empty)// set only if its available/not empty
                            {
                                buyerdata.AccountingCompanyId = accCompId.AccountingCompanyId;
                            }
                        }
                    }
                }
                return buyerperformancedata;
            }
            else
            {
                return buyerperformancedata;
            }
        }

        public async Task<CustomerDetailsViewModel> GetBuyerDetails(int buyerCompanyId, int jobId, UserContext userContext)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetBuyerDetails"))
            {
                var response = new CustomerDetailsViewModel();
                try
                {
                    var exceptionDomain = new ExceptionDomain(this);
                    var settingsDomain = new SettingsDomain(this);
                    var isExceptionEnabled = exceptionDomain.IsExceptionEnabled(userContext);
                    var company = Context.DataContext.Companies.SingleOrDefault(t => t.Id == buyerCompanyId);
                    if (company != null)
                    {
                        SetSupplierCompanyDetails(response, company);

                        var orders = await Context.DataContext.Orders.Where(t => t.IsActive && t.AcceptedCompanyId == userContext.CompanyId && t.BuyerCompanyId == buyerCompanyId).ToListAsync();
                        if (orders != null && orders.Any())
                        {
                            SetOrderSectionDetails(userContext, response, orders);
                            await SetDeliverySectionDetails(userContext, response, orders);
                            SetPricingSectionDetails(response, orders);
                            SetApprovalSectionDetails(response, orders);
                            SetOtherSectionBuyerDetails(buyerCompanyId, response);
                        }
                    }
                    response.IsExceptionEnabled = await isExceptionEnabled;
                    var accountingCompanyId = settingsDomain.GetAccountingCompanyIdforOrder(buyerCompanyId, userContext.CompanyId);
                    response.AccountingCompanyId = accountingCompanyId;
                    response.CustomerCompanyId = buyerCompanyId;
                    response.IsSAPCreditCheckEnabled = await Context.DataContext.OnboardingPreferences.AnyAsync(t => t.CompanyId == userContext.CompanyId && t.IsActive && t.CreditCheckType == CreditCheckTypes.SAP);
                }

                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetBuyerDetails", ex.Message + " -buyerCompanyId: " + buyerCompanyId, ex);
                }

                return response;
            }
        }

        public async Task<ResponseViewModel> AssignTier(UserContext user, int buyerCompanyId, int tierId)
        {
            using (var tracer = new Tracer("DashboardDomain", "AssignTier"))
            {
                var response = new ResponseViewModel(Status.Success);

                try
                {
                    var currentTier = await Context.DataContext.OfferTierMappings.SingleOrDefaultAsync(t => t.SupplierCompanyId == user.CompanyId && t.BuyerCompanyId == buyerCompanyId && t.IsActive);
                    if (currentTier != null)
                    {
                        // deactivate current assigned tier
                        currentTier.IsActive = false;
                        Context.DataContext.Entry(currentTier).State = EntityState.Modified;
                    }
                    if (tierId > 0)
                    {
                        // add new tier assignment entry
                        var newTier = new OfferTierMapping
                        {
                            BuyerCompanyId = buyerCompanyId,
                            SupplierCompanyId = user.CompanyId,
                            IsActive = true,
                            CreatedBy = user.Id,
                            CreatedDate = DateTimeOffset.Now,
                            UpdatedBy = user.Id,
                            UpdatedDate = DateTimeOffset.Now,
                            TierId = tierId
                        };
                        Context.DataContext.OfferTierMappings.Add(newTier);
                    }
                    await Context.CommitAsync();
                    response.StatusMessage = Resource.errMessageTierAssignementSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageFailed;
                    LogManager.Logger.WriteException("DashboardDomain", "AssignTier", ex.Message, ex);
                }

                return response;
            }
        }

        private static void SetApprovalSectionDetails(CustomerDetailsViewModel response, List<Order> orders)
        {
            //supplier side - buyer approval section
            response.TotalApprovals = orders.Sum(t => t.Invoices
                                .Count(i => i.IsActive && i.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !i.IsBuyPriceInvoice
                                        && i.InvoiceXInvoiceStatusDetails.Single(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.WaitingForApproval));

            response.ApprovalDDTs = orders.Sum(t => t.Invoices
                                .Count(i => i.IsActive && i.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !i.IsBuyPriceInvoice
                                        && (i.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || i.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                                        && i.InvoiceXInvoiceStatusDetails.Single(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.WaitingForApproval));

            response.ApprovalInvoices = orders.Sum(t => t.Invoices
                                .Count(i => i.IsActive && i.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !i.IsBuyPriceInvoice
                                        && (i.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && i.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp)
                                        && i.InvoiceXInvoiceStatusDetails.Single(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.WaitingForApproval));

            response.RejectedDDTs = orders.Sum(t => t.Invoices
                                .Count(i => i.IsActive && i.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !i.IsBuyPriceInvoice
                                        && (i.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || i.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                                        && i.InvoiceXInvoiceStatusDetails.Single(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.Rejected
                                        && !i.InvoiceHeader.InvoiceNumber.InvoiceHeaderDetails.SelectMany(t4 => t4.Invoices).Any(t2 => t2.InvoiceXInvoiceStatusDetails.Any(t3 => t3.StatusId == (int)InvoiceStatus.Received))));

            response.RejectedInvoices = orders.Sum(t => t.Invoices
                                .Count(i => i.IsActive && i.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !i.IsBuyPriceInvoice
                                        && (i.InvoiceTypeId == (int)InvoiceType.Manual || i.InvoiceTypeId == (int)InvoiceType.MobileApp)
                                        && i.InvoiceXInvoiceStatusDetails.Single(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.Rejected
                                        && !i.InvoiceHeader.InvoiceNumber.InvoiceHeaderDetails.SelectMany(t4 => t4.Invoices).Any(t2 => t2.InvoiceXInvoiceStatusDetails.Any(t3 => t3.StatusId == (int)InvoiceStatus.Received))));

            response.TotalRejected = response.RejectedDDTs + response.RejectedInvoices;
        }

        public async Task<CustomerDetailsViewModel> GetSupplierDetails(int supplierCompanyId, int jobId, UserContext userContext)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetSupplierDetails"))
            {
                var response = new CustomerDetailsViewModel();
                try
                {
                    var exceptionDomain = new ExceptionDomain(this);
                    var isExceptionEnabled = exceptionDomain.IsExceptionEnabled(userContext);
                    var company = Context.DataContext.Companies.SingleOrDefault(t => t.Id == supplierCompanyId);
                    if (company != null)
                    {
                        SetSupplierCompanyDetails(response, company);
                        response.IsBuyerAccount = true;

                        var orders = await Context.DataContext.Orders.Where(t => t.IsActive && t.AcceptedCompanyId == supplierCompanyId && t.BuyerCompanyId == userContext.CompanyId
                                            && (jobId == 0 || t.FuelRequest.Job.Id == jobId)).ToListAsync();
                        if (orders != null && orders.Any())
                        {
                            SetOrderSectionDetails(userContext, response, orders);
                            await SetDeliverySectionDetails(userContext, response, orders);
                            SetPricingSectionDetails(response, orders);
                            SetOtherSectionSupplierDetails(supplierCompanyId, userContext, response, orders);
                        }
                    }
                    response.IsExceptionEnabled = await isExceptionEnabled;
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "GetSupplierDetails", ex.Message + " -supplierCompanyId: " + supplierCompanyId, ex);
                }

                return response;
            }
        }

        private void SetOtherSectionSupplierDetails(int supplierCompanyId, UserContext userContext, CustomerDetailsViewModel response, List<Order> orders)
        {
            var privateListByUser = Context.DataContext.PrivateSupplierLists.Where(t => t.CompanyId == userContext.CompanyId && t.Companies.FirstOrDefault(t1 => t1.Id == supplierCompanyId) != null);
            if (privateListByUser != null && privateListByUser.Any())
            {
                foreach (var item in privateListByUser)
                {
                    var lastUsedInFr = orders.Where(t => !t.FuelRequest.IsPublicRequest && t.FuelRequest.PrivateSupplierLists.Any(t1 => t1.Id == item.Id)).OrderBy(t2 => t2.AcceptedDate).ToList();
                    response.PrivateListSection.Add(new PrivateListSection()
                    {
                        ListName = item.Name,
                        CreatedDate = item.CreatedDate.Date,
                        LastUsed = lastUsedInFr.FirstOrDefault() != null ? lastUsedInFr.FirstOrDefault().AcceptedDate.Date.ToShortDateString() : Resource.lblHyphen
                    });
                }
            }
        }

        private void SetOtherSectionBuyerDetails(int buyerCompanyId, CustomerDetailsViewModel response)
        {
            response.IsTaxExemption = Context.DataContext.TaxExemptLicenses.Any(t => t.CompanyId == buyerCompanyId);
            //response.CreditApplication = 
        }

        private void SetPricingSectionDetails(CustomerDetailsViewModel response, List<Order> orders)
        {
            //pricing section
            response.TotalDdtCount = orders.Sum(t => t.Invoices.Count(t1 => t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t1.IsBuyPriceInvoice
                                    && (t1.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || t1.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)));
            response.TotalInvoiceCount = orders.Sum(t => t.Invoices.Count(t1 => t1.IsActive && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t1.IsBuyPriceInvoice
                                    && (t1.InvoiceTypeId == (int)InvoiceType.Manual || t1.InvoiceTypeId == (int)InvoiceType.MobileApp)));
            response.TotalDryRunCount = orders.Sum(t => t.Invoices.Count(t1 => t1.IsActive && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                    && (t1.InvoiceTypeId == (int)InvoiceType.DryRun)));

            var helperDomain = new HelperDomain(this);
            decimal totalPpg = 0;
            var invoiceCounts = 0;
            List<FuelSection> fuelSections = new List<FuelSection>();
            decimal gallonsDroppedExcludingNotSpecifiedOrders = 0;
            foreach (var item in orders)
            {
                decimal droppedGallonsForOrder = 0;
                decimal ppgForOrder = 0;
                int invoiceCount = 0;

                //need to add FR section details here
                foreach (var invoice in item.Invoices.Where(t => t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t.IsBuyPriceInvoice))
                {
                    //need to add check for DDt converted to Invoice
                    response.TotalFees += helperDomain.GetInvoiceTotalFees(invoice);
                    totalPpg += invoice.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail.PricePerGallon).FirstOrDefault();
                    if (invoice.InvoiceTypeId != (int)InvoiceType.DryRun)
                    {
                        invoiceCounts++;
                    }
                    if (invoice.Order.FuelRequest.QuantityTypeId != (int)QuantityType.NotSpecified)
                    {
                        gallonsDroppedExcludingNotSpecifiedOrders += invoice.DroppedGallons;
                    }

                    response.GallonsDelivered += invoice.DroppedGallons;
                    if (invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp)
                    {
                        response.TotalAmount += (invoice.BasicAmount + invoice.TotalTaxAmount - invoice.TotalDiscountAmount);
                    }
                    droppedGallonsForOrder += invoice.DroppedGallons;
                    ppgForOrder += invoice.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail.PricePerGallon).FirstOrDefault();
                    if (invoice.InvoiceTypeId != (int)InvoiceType.DryRun)
                    {
                        invoiceCount++;
                    }
                }

                fuelSections.Add(new FuelSection()
                {
                    FuelType = helperDomain.GetProductName(item.FuelRequest.MstProduct),
                    FuelTypeId = item.FuelRequest.FuelTypeId,
                    TotalOrdersOfFuelType = 1,
                    GallonsOrderedOfFuelType = item.FuelRequest.MaxQuantity == (int)ApplicationConstants.QuantityNotSpecified ? 0 : item.FuelRequest.MaxQuantity,
                    AvgGallonsPerDrop = droppedGallonsForOrder,
                    AvgPpgPerOrder = ppgForOrder,
                    InvoiceCountOfFuelType = invoiceCount
                });
            }

            if (fuelSections.Any())
            {
                response.FuelSection = fuelSections.GroupBy(t => t.FuelType)
                                        .Select(t => new { Items = t.ToList() })
                                        .Select(t => new FuelSection()
                                        {
                                            TotalOrdersOfFuelType = t.Items.Count,
                                            GallonsOrderedOfFuelType = t.Items.Sum(g => g.GallonsOrderedOfFuelType).GetPreciseValue(2),
                                            FuelType = t.Items.FirstOrDefault().FuelType,
                                            AvgPpgPerOrder = t.Items.Sum(g => g.InvoiceCountOfFuelType) > 0 ? (t.Items.Sum(g => g.AvgPpgPerOrder) / t.Items.Sum(g => g.InvoiceCountOfFuelType)).GetPreciseValue(2) : 0,
                                            AvgGallonsPerDrop = t.Items.Sum(g => g.InvoiceCountOfFuelType) > 0 ? (t.Items.Sum(g => g.AvgGallonsPerDrop) / t.Items.Sum(g => g.InvoiceCountOfFuelType)).GetPreciseValue(2) : 0
                                        }).ToList();
            }
            response.GallonsDelivered = response.GallonsDelivered.GetPreciseValue(2);
            response.TotalAmount = (response.TotalAmount + response.TotalFees).GetPreciseValue(2);
            if (invoiceCounts > 0)
            {
                response.AvgPpgPerDrop = (totalPpg / invoiceCounts).GetPreciseValue(2);
                response.AvgGallonsPerDrop = (response.GallonsDelivered / invoiceCounts).GetPreciseValue(2);
            }
            response.GallonsRemaining = (response.GallonsOrdered - gallonsDroppedExcludingNotSpecifiedOrders).GetPreciseValue(2);
        }

        private async Task SetDeliverySectionDetails(UserContext userContext, CustomerDetailsViewModel response, List<Order> orders)
        {
            //delivery section
            response.TotalDeliveries = orders.Sum(t => t.Invoices.Count(t1 => t1.IsActive && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t1.IsBuyPriceInvoice && t1.InvoiceTypeId != (int)InvoiceType.DryRun));
            OrderDomain orderDomain = new OrderDomain(this);
            List<NextDeliveryScheduleViewModel> nextDeliverySchedules = new List<NextDeliveryScheduleViewModel>();

            foreach (var item in orders)
            {
                nextDeliverySchedules.AddRange(await orderDomain.GetNextDeliveryScheduleDetails(item.Id));
                if (item.FuelRequest.QuantityTypeId != (int)QuantityType.NotSpecified)
                    response.GallonsOrdered += (item.BrokeredMaxQuantity ?? item.FuelRequest.MaxQuantity).GetPreciseValue(2);
            }
            var upcomingSchedule = nextDeliverySchedules.Where(t => t.Date > DateTime.Now.Date).OrderBy(t => t.ScheduleDate).ThenBy(t => t.ScheduleTime).FirstOrDefault();
            if (upcomingSchedule != null)
            {
                response.NextScheduledDelievery = upcomingSchedule.Date.DayOfWeek.ToString() + ": " + upcomingSchedule.Date.Date.ToString(Resource.constFormatDate)
                                        + Constants.Between + upcomingSchedule.ScheduleTime
                                        + Constants.For + upcomingSchedule.Quantity + " " + Resource.lblGallons;
            }

            response.ScheduledDeleveries = nextDeliverySchedules.Count;

            response.OntimeDeliveries = orders.Sum(t => t.DeliveryScheduleXTrackableSchedules
                .Count(t1 => t1.IsActive && t1.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Completed
                        || t1.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.RescheduledCompleted));
            response.LateDeliveries = orders.Sum(t => t.DeliveryScheduleXTrackableSchedules
                .Count(t1 => t1.IsActive && t1.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.CompletedLate
                        || t1.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.RescheduledLate));
            response.RescheduledByYou = orders.Sum(t => t.OrderDeliverySchedules.Select(x => new { x.Version, x.CreatedBy }).Distinct().Count(t1 => t1.CreatedBy == userContext.Id
            && t.DeliveryScheduleXTrackableSchedules.FirstOrDefault(t2 => t2.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Rescheduled) != null));
            response.RescheduledByCustomer = orders.Sum(t => t.OrderDeliverySchedules.Select(x => new { x.Version, x.CreatedBy }).Distinct().Count(t1 => t1.CreatedBy != userContext.Id
              && t.DeliveryScheduleXTrackableSchedules.FirstOrDefault(t2 => t2.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Rescheduled) != null));
        }

        private static void SetOrderSectionDetails(UserContext userContext, CustomerDetailsViewModel response, List<Order> orders)
        {
            //order section
            response.ConnectedSince = orders.FirstOrDefault().AcceptedDate.Date;
            response.TotalOrders = orders.Count;
            response.OpenOrders = orders.Count(t => t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open);
            response.ClosedOrders = orders.Count(t => t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed);
            response.CanceledOrdersByYou = orders.Sum(t => t.OrderXStatuses.Count(t1 => t1.IsActive && t1.UpdatedBy == userContext.Id && t1.StatusId == (int)OrderStatus.Canceled));
            response.CanceledOrdersByCustomer = orders.Sum(t => t.OrderXStatuses.Count(t1 => t1.IsActive && t1.User != null && t1.User.Company != null && t1.User.Company.Id != userContext.CompanyId && t1.StatusId == (int)OrderStatus.Canceled));
        }

        private static void SetSupplierCompanyDetails(CustomerDetailsViewModel response, Company company)
        {
            response.CustomerCompanyName = company.Name;
            response.SupplierCompanyId = company.Id;
            var adminUser = company.Users.Select(t => new { t.FirstName, t.LastName, t.Email, t.PhoneNumber, t.OnboardedTypeId }).FirstOrDefault();
            if (adminUser != null)
            {
                response.CustomerContact.Name = $"{adminUser.FirstName} {adminUser.LastName}";
                response.CustomerContact.Email = adminUser.Email;
                response.CustomerContact.PhoneNumber = adminUser.PhoneNumber;
                response.OnboardedTypeId = adminUser.OnboardedTypeId;
            }
            var companyDefaultAddress = company.CompanyAddresses.FirstOrDefault(t => t.IsDefault);
            if (companyDefaultAddress != null)
            {
                response.CustomerAddress.Address = companyDefaultAddress.Address;
                response.CustomerAddress.AddressLine2 = companyDefaultAddress.AddressLine2;
                response.CustomerAddress.AddressLine3 = companyDefaultAddress.AddressLine3;
                response.CustomerAddress.City = companyDefaultAddress.City;
                response.CustomerAddress.ZipCode = companyDefaultAddress.ZipCode;
                response.CustomerAddress.StateCode = companyDefaultAddress.MstState.Code;
            }

            if (adminUser != null && adminUser.OnboardedTypeId == (int)OnboardedType.ThirdPartyOrderOnboarded)
            {
                TaxExemtionLicenseDomain taxDomain = new TaxExemtionLicenseDomain();
                var directTaxDetails = Task.Run(() => taxDomain.GetDirectTaxDetails(company.Id, new CompanyTaxesViewModel(), (CompanyType)company.CompanyTypeId, (CompanyType)company.CompanyTypeId)).Result;

                response.IsDirectTax = directTaxDetails.IsDirectTax;
                response.DirectTaxes = directTaxDetails.DirectTaxes;
                response.IsEditDirectTax = directTaxDetails.IsEdit;
            }
        }

        public async Task<StatusViewModel> SaveDBTileSettings(string pageId, List<DashboardTileViewModel> settingsModel, UserContext userContext)
        {
            using (var tracer = new Tracer("DashboardDomain", "SaveDBTileSettings"))
            {
                var response = new StatusViewModel();
                List<DashboardTileViewModel> lstTileSetting;
                try
                {
                    if (settingsModel == null || settingsModel.Count == 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Status.Failed.ToString();
                        return response;
                    }

                    var existingSetting = await Context.DataContext.UserPageSettings.FirstOrDefaultAsync(t => t.UserId == userContext.Id && t.PageId == pageId);
                    if (existingSetting != null)
                    {
                        lstTileSetting = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DashboardTileViewModel>>(existingSetting.Setting);

                        if (lstTileSetting != null && lstTileSetting.Any())
                            lstTileSetting = new List<DashboardTileViewModel>();

                        foreach (var obj in settingsModel)
                        {
                            DashboardTileViewModel tile = new DashboardTileViewModel();
                            tile.RowIdx = obj.RowIdx;
                            tile.ColIdx = obj.ColIdx;
                            tile.IsClosed = obj.IsClosed;
                            tile.IsCollapsed = obj.IsCollapsed;
                            tile.TileName = obj.TileName;
                            tile.TileDisplayName = obj.TileDisplayName;

                            lstTileSetting.Add(tile);
                        }

                        //var tiles1 = lstTileSetting.Select(t => new { t.TileName }).ToList();
                        //var tiles2 = settingsModel.Select(t => new { t.TileName }).ToList();
                        //var removeTiles = tiles2.Except(tiles1).ToList();

                        //if (removeTiles != null && removeTiles.Any())
                        //    removeTiles.ForEach(t => lstTileSetting.RemoveAll(t1 => t1.TileName == t.TileName));

                        //foreach (var obj in settingsModel)
                        //{
                        //    var existingObj = lstTileSetting.FirstOrDefault(t => t.TileName.ToLower() == obj.TileName.ToLower());
                        //    if (existingObj != null)
                        //    {
                        //        existingObj.RowIdx = obj.RowIdx;
                        //        existingObj.ColIdx = obj.ColIdx;
                        //        existingObj.IsClosed = obj.IsClosed;
                        //        existingObj.IsCollapsed = obj.IsCollapsed;
                        //    }
                        //    else
                        //    {
                        //        DashboardTileViewModel tile = new DashboardTileViewModel();
                        //        tile.RowIdx = obj.RowIdx;
                        //        tile.ColIdx = obj.ColIdx;
                        //        tile.IsClosed = obj.IsClosed;
                        //        tile.IsCollapsed = obj.IsCollapsed;
                        //        tile.TileName = obj.TileName;

                        //        lstTileSetting.Add(tile);
                        //    }
                        //}

                        var tileSettingJson = Newtonsoft.Json.JsonConvert.SerializeObject(lstTileSetting);

                        existingSetting.Setting = tileSettingJson;
                        existingSetting.UpdatedBy = userContext.Id;
                        existingSetting.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.Entry(existingSetting).State = EntityState.Modified;
                    }
                    else
                    {
                        var tileSettingJson = Newtonsoft.Json.JsonConvert.SerializeObject(settingsModel);
                        var tileSetting = new UserPageSetting()
                        {
                            PageId = pageId,
                            UserId = userContext.Id,
                            CompanyId = userContext.CompanyId,
                            Setting = tileSettingJson,
                            IsActive = true,
                            CreatedBy = userContext.Id,
                            CreatedDate = DateTimeOffset.Now,
                            UpdatedBy = userContext.Id,
                            UpdatedDate = DateTimeOffset.Now,
                        };

                        Context.DataContext.UserPageSettings.Add(tileSetting);
                    }

                    await Context.CommitAsync();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMsgTileSettings;
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorMsgTileSettings;
                    LogManager.Logger.WriteException("DashboardDomain", "SaveDBTileSettings", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<UserPageSettingViewModel> GetDBTileSettings(UserPageSetting tileSettings, UserContext userContext)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetDBTileSettings"))
            {
                var response = new UserPageSettingViewModel();
                try
                {
                    if (tileSettings != null)
                    {
                        var lstTileSetting = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DashboardTileViewModel>>(tileSettings.Setting);
                        response.Id = tileSettings.Id;
                        response.UserId = tileSettings.UserId;
                        response.PageId = tileSettings.PageId;
                        response.TileDetails.AddRange(lstTileSetting);

                        bool isNewTilesAdded = false;
                        if (userContext.IsBuyerCompany)
                        {
                            isNewTilesAdded = checkAndAddNewTiles<BuyerDashboardTiles>(lstTileSetting, new BuyerDashboardTiles());
                        }
                        if (isNewTilesAdded)
                        {
                            await SaveDBTileSettings(tileSettings.PageId, lstTileSetting, userContext);
                        }

                    }
                    else
                    {
                        if (userContext.IsSupplierCompany)
                            response = CreateDefaultDashboardTileSettings<SupplierDashboardTiles>();
                        else if (userContext.IsBuyerCompany)
                            response = CreateDefaultDashboardTileSettings<BuyerDashboardTiles>();
                        else if (userContext.IsDriver)
                            response = CreateDefaultDashboardTileSettings<DriverDashboardTiles>();
                        else if (userContext.IsSupplierAdmin)
                            response = CreateDefaultDashboardTileSettings<SuperAdminDashboardTiles>();

                        return response;
                    }

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.msgSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Status.Failed.ToString();
                    LogManager.Logger.WriteException("DashboardDomain", "GetDBTileSettings", ex.Message, ex);
                }

                return response;
            }
        }

        private bool checkAndAddNewTiles<T>(List<DashboardTileViewModel> lstTileSetting, T enumType)
        {
            var isNewTilesAdded = false;
            var tileEnums = Enum.GetValues(typeof(T));
            if (lstTileSetting !=null && tileEnums.Length != lstTileSetting.Count)
            {
                foreach (var item in tileEnums)
                {
                    var isExistingTile = lstTileSetting.Any(t => t.TileName.Contains(item.ToString()));
                    if (!isExistingTile)
                    {

                        DashboardTileViewModel tile = new DashboardTileViewModel();
                        tile.RowIdx = lstTileSetting.Max(t => t.RowIdx + 1);
                        tile.ColIdx = lstTileSetting.Min(t => t.ColIdx);
                        tile.IsClosed = false;
                        tile.IsCollapsed = false;
                        tile.TileName = item.ToString() + ApplicationConstants.TileKey;
                        var type = typeof(T);
                        var member = type.GetMember(item.ToString());
                        var attributes = member[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                        var tileDisplayName = ((DisplayAttribute)attributes[0]).Name ?? item.ToString();
                        tile.TileDisplayName = tileDisplayName;
                        // Context.DataContext.UserPageSettings.Add();
                        lstTileSetting.Add(tile);
                        isNewTilesAdded = true;
                    }
                }
            }
            return isNewTilesAdded;
        }

        private UserPageSettingViewModel CreateDefaultDashboardTileSettings<T>()
        {
            var userPageSetting = new UserPageSettingViewModel();
            int c = 0, r = 0;
            var type = typeof(T);

            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var member = type.GetMember(item.ToString());
                var attributes = member[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                var tileDisplayName = ((DisplayAttribute)attributes[0]).Name ?? item.ToString();

                userPageSetting.TileDetails.Add(new
                DashboardTileViewModel
                {
                    ColIdx = c,
                    RowIdx = r,
                    TileName = item.ToString() + ApplicationConstants.TileKey,
                    TileDisplayName = tileDisplayName,
                    IsClosed = false
                });


                if (c == 1)
                    r++;
                c++;
                if (c == 2)
                    c = 0;
            }

            return userPageSetting;
        }

        private void SetTileSetting(DashboardViewModel dbModel, UserPageSettingViewModel settingModel, UserRoles userType)
        {
            try
            {
                if (userType == UserRoles.Supplier)
                {
                    foreach (var tile in settingModel.TileDetails)
                    {
                        SupplierDashboardTiles tileEnum = new SupplierDashboardTiles();
                        var tileName = tile.TileName.Replace(ApplicationConstants.TileKey, "");
                        try
                        {
                            tileEnum = (SupplierDashboardTiles)Enum.Parse(typeof(SupplierDashboardTiles), tileName);
                        }
                        catch
                        {
                            // handle exception if tile is removed from dashboard and data exists in DB
                            continue;
                        }

                        switch (tileEnum)
                        {
                            case SupplierDashboardTiles.Dispatch:
                                dbModel.IsDispatchTileClosed = tile.IsClosed;
                                dbModel.IsDispatchTileCollapsed = tile.IsCollapsed;
                                break;
                            case SupplierDashboardTiles.FuelRequests:
                                dbModel.IsFRTileClosed = tile.IsClosed;
                                dbModel.IsFRTileCollapsed = tile.IsCollapsed;
                                break;
                            case SupplierDashboardTiles.FuelRequestQuote:
                                dbModel.IsFRQuoteTileClosed = tile.IsClosed;
                                dbModel.IsFRQuoteTileCollapsed = tile.IsCollapsed;
                                break;
                            case SupplierDashboardTiles.Invoices:
                                dbModel.IsInvoiceTileClosed = tile.IsClosed;
                                dbModel.IsInvoiceTileCollapsed = tile.IsCollapsed;
                                break;
                            case SupplierDashboardTiles.GlobalFuelCost:
                                dbModel.IsGFCTileClosed = tile.IsClosed;
                                dbModel.IsGFCTileCollapsed = tile.IsCollapsed;
                                break;
                            case SupplierDashboardTiles.Orders:
                                dbModel.IsOrderTileClosed = tile.IsClosed;
                                dbModel.IsOrderTileCollapsed = tile.IsCollapsed;
                                break;
                            case SupplierDashboardTiles.GallonStats:
                                dbModel.IsGallonStatTileClosed = tile.IsClosed;
                                dbModel.IsGallonStatTileCollapsed = tile.IsCollapsed;
                                break;
                            case SupplierDashboardTiles.YourBusiness:
                                dbModel.IsYourBusinessTileClosed = tile.IsClosed;
                                dbModel.IsYourBusinessTileCollapsed = tile.IsCollapsed;
                                break;
                            case SupplierDashboardTiles.DropAverages:
                                dbModel.IsDropAvgTileClosed = tile.IsClosed;
                                dbModel.IsDropAvgTileCollapsed = tile.IsCollapsed;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else if (userType == UserRoles.Buyer)
                {
                    foreach (var tile in settingModel.TileDetails)
                    {
                        BuyerDashboardTiles tileEnum = new BuyerDashboardTiles();
                        var tileName = tile.TileName.Replace(ApplicationConstants.TileKey, "");
                        try
                        {
                            tileEnum = (BuyerDashboardTiles)Enum.Parse(typeof(BuyerDashboardTiles), tileName);
                        }
                        catch (Exception ex)
                        {
                            // handle exception if tile is removed from dashboard and data exists in DB
                            continue;
                        }

                        switch (tileEnum)
                        {
                            case BuyerDashboardTiles.RequestForQuote:
                                dbModel.IsFRQuoteTileClosed = tile.IsClosed;
                                dbModel.IsFRQuoteTileCollapsed = tile.IsCollapsed;
                                break;
                            case BuyerDashboardTiles.Deliveries:
                                dbModel.IsDeliveriesTileClosed = tile.IsClosed;
                                dbModel.IsDeliveriesTileCollapsed = tile.IsCollapsed;
                                break;
                            case BuyerDashboardTiles.FuelRequests:
                                dbModel.IsFRTileClosed = tile.IsClosed;
                                dbModel.IsFRTileCollapsed = tile.IsCollapsed;
                                break;
                            case BuyerDashboardTiles.Invoices:
                                dbModel.IsInvoiceTileClosed = tile.IsClosed;
                                dbModel.IsInvoiceTileCollapsed = tile.IsCollapsed;
                                break;
                            case BuyerDashboardTiles.JobAverages:
                                dbModel.IsJobAvgTileClosed = tile.IsClosed;
                                dbModel.IsJobAvgTileCollapsed = tile.IsCollapsed;
                                break;
                            case BuyerDashboardTiles.Orders:
                                dbModel.IsOrderTileClosed = tile.IsClosed;
                                dbModel.IsOrderTileCollapsed = tile.IsCollapsed;
                                break;
                            case BuyerDashboardTiles.DeliveryRequests:
                                dbModel.IsDeliveryRequestTileClosed = tile.IsClosed;
                                dbModel.IsDeliveryRequestTileCollapsed = tile.IsCollapsed;
                                break;
                            default:
                                break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "SetTileSetting", ex.Message, ex);
            }
        }

        public List<UspBuyerPerformanceViewModel> FilterSupplierCustomers(List<UspBuyerPerformanceViewModel> supplierCustomers, UserContext userContext, bool getTpoBuyers)
        {
            using (var tracer = new Tracer("DashboardDomain", "FilterSupplierCustomers"))
            {
                try
                {
                    var tpoCompanies = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetTPOCompanyList(userContext.Id, userContext.CompanyId);
                    if (tpoCompanies != null && tpoCompanies.Any())
                    {
                        var tpoCompanyIds = tpoCompanies.Select(t => t.Id).ToList();
                        if (getTpoBuyers)
                            supplierCustomers = supplierCustomers.Where(t => tpoCompanyIds.Contains(t.BuyerCompanyId)).ToList();
                        else
                            supplierCustomers = supplierCustomers.Where(t => !tpoCompanyIds.Contains(t.BuyerCompanyId)).ToList();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashboardDomain", "FilterSupplierCustomers", ex.Message, ex);
                }

                return supplierCustomers;
            }
        }

        public void AddErrorListToQueue(string validationErrors, string fileName, UserContext userContext)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var queueDomain = new QueueMessageDomain();
                var queueRequest = GetQueueEventForInvoiceBulkUploadErrors(userContext, fileName, validationErrors);
                var queueId = queueDomain.EnqeueMessage(queueRequest);
            }
        }

        private QueueMessageViewModel GetQueueEventForInvoiceBulkUploadErrors(UserContext userContext, string blobStoragePath, string errors)
        {
            var jsonViewModel = new InvoiceBulkUploadErrorProcessorViewModel();
            jsonViewModel.FileLineNumberToStart = 0;
            jsonViewModel.SupplierId = userContext.Id;
            jsonViewModel.SupplierCompanyId = userContext.CompanyId;
            jsonViewModel.FileUploadPath = blobStoragePath;
            jsonViewModel.Errors = errors;

            string json = JsonConvert.SerializeObject(jsonViewModel);

            var lengthOfJson = json.Length;
            if (lengthOfJson > 1024)
            {
                int extraChar = lengthOfJson - 1024;
                jsonViewModel.Errors = jsonViewModel.Errors.Replace(".... Too many errors in file", "").Substring(0, jsonViewModel.Errors.Length - extraChar - 35);
                jsonViewModel.Errors = string.Concat(jsonViewModel.Errors, ".... Too many errors in file");
            }
            json = JsonConvert.SerializeObject(jsonViewModel);
            return new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = QueueProcessType.DemandCaptureUpload,
                JsonMessage = json
            };
        }

        private void ProcessErrorList(List<string> errorInfo, string validationErrors, string fileUploadPath)
        {
            try
            {
                errorInfo.Add("<p><br><b>" + fileUploadPath + " processing result:</b>" + validationErrors + "</p><br>");
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "ProcessErrorList", "demand file status", ex);
            }
        }

        public string ProcessDemandCaptureUpload(InvoiceBulkUploadErrorProcessorViewModel bulkRequestViewModel, List<string> errorInfo)
        {
            using (var tracer = new Tracer("DashboardDomain", "ProcessDemandCaptureUpload"))
            {
                StringBuilder processMessage = new StringBuilder();

                try
                {
                    if (!string.IsNullOrWhiteSpace(bulkRequestViewModel.FileUploadPath))
                    {
                        //processing actual bulk file
                        if (!string.IsNullOrWhiteSpace(bulkRequestViewModel.Errors))
                        {
                            processMessage.Clear();
                            ProcessErrorList(errorInfo, bulkRequestViewModel.Errors, bulkRequestViewModel.FileUploadPath);
                        }
                        else
                        {
                            processMessage.Append(Resource.errMessageFailedToReadFileContent);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                        LogManager.Logger.WriteException("DashboardDomain", "ProcessDemandCaptureUpload", ex.Message, ex);
                    if (processMessage.Length == 0)
                    {
                        processMessage.Append(Constants.ErrorWhileProcessingBulkOrder);
                        errorInfo.Add(processMessage.ToString());
                    }
                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                }
                return processMessage.ToString();
            }
        }
        public async Task<bool> GetCarrierDiptestAvailability(int carrierCompanyId)
        {
            bool response = false;
            try
            {
                var jobIds = await new FreightServiceDomain(this).GetCarriersJobs(carrierCompanyId, 0);
                if (jobIds != null && jobIds.Any())
                {
                    response = Context.DataContext.JobXAssets.Where(t => jobIds.Contains(t.JobId)
                                                                                         && t.Asset.Type == (int)AssetType.Tank
                                                                                         && t.Asset.IsActive
                                                                                         && t.RemovedBy == null && t.RemovedDate == null)
                                                                               .Any();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "GetCarrierDiptestAvailability", ex.Message, ex);
            }
            return response;
        }

        #region Buyer Dashboard New
        public async Task<List<JobBuyerDashboardViewModel>> GetJobDetailsForBuyerDashboard(int companyID, int countryId)
        {
            var response = new List<JobBuyerDashboardViewModel>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                response = await storedProcedureDomain.GetJobDetailsForBuyerDashboard(companyID, countryId);
                if (response != null && response.Any())
                {
                    var jobIds = response.Select(t => t.JobID).Distinct().ToList();
                    await GetJobDRPrioritiesForBuyer(companyID, response, jobIds);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardDomain", "GetJobDetailsForBuyerDashboard", ex.Message, ex);
            }
            return response;
        }
        private async Task GetJobDRPrioritiesForBuyer(int companyID, List<JobBuyerDashboardViewModel> response, List<int> jobIds)
        {
            var freightServiceDomain = new FreightServiceDomain(this);
            var jobDRPriority = new JobDRPriorityInputModel();
            jobDRPriority.CompanyId = companyID;
            jobDRPriority.JobIds = jobIds;
            var deliveryRequestDetails = await freightServiceDomain.GetJobDRPrioritiesForBuyer(jobDRPriority);
            if (deliveryRequestDetails != null && deliveryRequestDetails.Any())
            {
                foreach (var item in deliveryRequestDetails)
                {
                    var recordFound = response.FirstOrDefault(top => top.JobID == item.JobId);
                    if (recordFound != null)
                    {
                        recordFound.jobDeliveryRequests.Add(new JobDRDetailsModel { JobId = item.JobId, Priority = item.Priority });
                    }
                }
            }
        }
        public async Task<List<BuyerLoadsForDashboardViewModel>> GetBuyerLoadsForDashboard(UserContext userContext, BuyerLoadsForDashboardInputModel input)
        {
            List<BuyerLoadsForDashboardViewModel> response = new List<BuyerLoadsForDashboardViewModel>();
            try
            {
                if (input.FromDate == DateTimeOffset.MinValue && input.ToDate == DateTimeOffset.MinValue)
                {
                    input.FromDate = DateTimeOffset.Now.Date;
                    input.ToDate = DateTimeOffset.Now.Date;
                }
                var spDomain = new StoredProcedureDomain(this);
                var loadsForDashboard = await spDomain.GetBuyerLoadsForDashboard(userContext.CompanyId, input);
                response = loadsForDashboard.Select(t => t.ToViewModel(null)).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardDomain", "GetBuyerLoadsForDashboard", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<LocationInventoryModel>> GetLocationInventory(int companyId, string jobId, int priority, int SelectedTab, int CountryId)
        {
            var response = new List<LocationInventoryModel>();
            var salesDomain = new SalesDomain(this);
            var buyerSalesData = await salesDomain.GetBuyerSalesDataAsync(companyId, jobId, priority, SelectedTab, CountryId);

            if (buyerSalesData != null && buyerSalesData.Any())
            {
                int recordCount = 5;
                var applicationDomain = new ApplicationDomain(this);
                var buyerDashboardCount = applicationDomain.GetApplicationSettingValue<int>(ApplicationConstants.KeyBuyerDashboardRecordsCount, recordCount);
                var prioritySalesData = buyerSalesData.GroupBy(t => t.Priority, (key, item) => item.OrderBy(record => (record.DaysRemaining == "--" || string.IsNullOrEmpty(record.DaysRemaining)) ? 9999 : decimal.Parse(record.DaysRemaining)).Take(buyerDashboardCount))
                    .SelectMany(t => t.ToList())
                    .ToList();
                if (prioritySalesData != null && prioritySalesData.Any())
                {
                    prioritySalesData.ForEach(t => response.Add(t.ToLocationInventoryModel()));
                }
            }
            return response;
        }
        public async Task<UserPageSettingViewModel> GetNewBuyerDashboardTileSettings(UserContext userContext)
        {
            using (var tracer = new Tracer("DashboardDomain", "GetNewBuyerDashboardTileSettings"))
            {
                var response = new UserPageSettingViewModel();
                try
                {
                    var userData = await Context.DataContext.Users.
                     Select(x => new
                     {
                         User = x,
                         UserPageSettings = x.UserPageSettings.FirstOrDefault(t => t.UserId == userContext.Id && t.PageId == ApplicationConstants.NewBuyerDashboard)
                     }).SingleOrDefaultAsync(t => t.User.Id == userContext.Id);

                    if (userData != null && userData.UserPageSettings != null)
                    {
                        var tileSettings = userData.UserPageSettings;
                        var lstTileSetting = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DashboardTileViewModel>>(tileSettings.Setting);
                        response.Id = tileSettings.Id;
                        response.UserId = tileSettings.UserId;
                        response.PageId = tileSettings.PageId;
                        response.TileDetails.AddRange(lstTileSetting);

                        bool isNewTilesAdded = false;
                        isNewTilesAdded = checkAndAddNewTiles<NewBuyerDashboardTiles>(lstTileSetting, new NewBuyerDashboardTiles());

                        if (isNewTilesAdded)
                        {
                            await SaveDBTileSettings(tileSettings.PageId, lstTileSetting, userContext);
                        }

                    }
                    else
                    {
                        response = CreateDefaultDashboardTileSettings<NewBuyerDashboardTiles>();
                        if (response != null)
                        {
                            response.PageId = ApplicationConstants.NewBuyerDashboard;
                            response.UserId = userContext.Id;
                        }
                        return response;
                    }

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.msgSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Status.Failed.ToString();
                    LogManager.Logger.WriteException("DashboardDomain", "GetNewBuyerDashboardTileSettings", ex.Message, ex);
                }

                return response;
            }
        }
        #endregion

        public async Task<StatusViewModel> SaveUpdateCustomerId(int buyerCompanyId, string customerId, UserContext userContext)
        {
            var supplierBuyerSetting = new SupplierXBuyerSetting();
            var response = new StatusViewModel { StatusCode = Status.Success, StatusMessage = Resource.msgMyCustomerIdUpdate };
            try
            {
                if (string.IsNullOrEmpty(customerId))
                {
                    response = new StatusViewModel { StatusCode = Status.Warning, StatusMessage = Resource.warningValidationMyCustomerId };
                    return response;
                }
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    supplierBuyerSetting = Context.DataContext.SupplierXBuyerSettings.FirstOrDefaultAsync(x => x.SupplierCompanyId == userContext.CompanyId
                                                                                                && x.BuyerCompanyId == buyerCompanyId && x.IsActive).Result;
                    if (supplierBuyerSetting != null)
                    {
                        supplierBuyerSetting.UpdatedBy = userContext.Id;
                        supplierBuyerSetting.CustomerId = customerId;
                        supplierBuyerSetting.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.Entry(supplierBuyerSetting).State = EntityState.Modified;
                        await Context.CommitAsync();
                    }
                    else
                    {
                        supplierBuyerSetting = new SupplierXBuyerSetting();
                        supplierBuyerSetting.IsActive = true;
                        supplierBuyerSetting.CustomerId = customerId;
                        supplierBuyerSetting.SupplierCompanyId = userContext.CompanyId;
                        supplierBuyerSetting.BuyerCompanyId = buyerCompanyId;
                        supplierBuyerSetting.CreatedBy = userContext.Id;
                        supplierBuyerSetting.CreatedDate = DateTimeOffset.Now;
                        Context.DataContext.Entry(supplierBuyerSetting).State = EntityState.Added;
                        await Context.CommitAsync();
                    }
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "SaveUpdateCustomerId", ex.Message, ex);
                response = new StatusViewModel { StatusCode = Status.Failed, StatusMessage = Resource.errUpdateMyCustomerId };
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteTpoBuyerCompany(int buyerCompanyId, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var spDomain = new StoredProcedureDomain();
                var result = await spDomain.DeleteTpoBuyerCompany(buyerCompanyId, userContext.CompanyId, userContext.Id);
                if (result.StatusCode == Status.Success)
                {
                    response.StatusCode = result.StatusCode;
                    response.StatusMessage = result.StatusMessage;

                    if (!string.IsNullOrWhiteSpace(result.OrderIds))
                    {
                        var orderIds = result.OrderIds.Split(',').Select(int.Parse).ToList();
                        if (orderIds.Any())
                        {
                            var orderDomain = new OrderDomain(spDomain);
                            foreach (var orderId in orderIds)
                            {
                                var orderCloseResponse = await orderDomain.CloseOrderAsync(userContext, orderId, userContext.Id, true);
                                if (orderCloseResponse.StatusCode != Status.Failed)
                                {
                                    var drCloseStatus = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().DeleteDeliveryRequestOnOrderClose(new List<int> { orderId }, userContext);
                                    if (drCloseStatus.StatusCode != (int)Status.Success)
                                    {
                                        response.StatusCode = drCloseStatus.StatusCode;
                                        response.StatusMessage = drCloseStatus.StatusMessage;
                                    }
                                    else
                                    {
                                        LogManager.Logger.WriteError("DashboardDomain", "DeleteDeliveryRequestOnOrderClose", response.StatusMessage);
                                    }
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(result.JobIds))
                    {
                        var jobIds = result.JobIds.Split(',').Select(int.Parse).ToList();
                        if (jobIds.Any())
                        {
                            var freightServiceDomain = new FreightServiceDomain(spDomain);
                            var freightServiceResponce = await freightServiceDomain.RemoveJobDetailsForCustomer(jobIds);
                            response.ResponseData = result;
                            response.StatusCode = Status.Success;
                            response.StatusMessage = result.StatusMessage;
                        }
                    }
                }
                else
                {
                    response.StatusCode = result.StatusCode;
                    response.StatusMessage = result.StatusMessage;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "DeleteTpoBuyerCompany", ex.Message, ex);
                response = new StatusViewModel { StatusCode = Status.Failed, StatusMessage = "Failed to delete Tpo buyer company" };
            }
            return response;
        }
        
        public async Task<StatusViewModel> GetInventoryDropdownData(UserContext userContext, int countryId = (int)Country.All)
        {
            var response = new StatusViewModel(Status.Failed);
            var masterDomain = new MasterDomain();
            var fsDomain = new FreightServiceDomain(masterDomain);
            try
            {
                var states = masterDomain.GetStatesEx(countryId);
                var regions = await fsDomain.GetRegionsDdl(userContext.CompanyId);
                var customers = masterDomain.GetYourCustomers(userContext.CompanyId);
                var products = masterDomain.GetAllSupplierProducts(userContext.CompanyId);

                var result = new
                {
                    States = states,
                    Regions = regions,
                    Customers = customers,
                    Products = products
                };

                response.StatusCode = Status.Success;
                response.ResponseData = result;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "GetInventoryDropdownData", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> GetInventoryDataForDashboard(InventoryDataViewModel filter, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var productIds = new List<int>();
                if (filter.SelectedProductIds.Count > 0)
                {
                    productIds = await Context.DataContext.MstProducts.Where(t => t.IsActive && t.TfxProductId != null &&
                                                                                  filter.SelectedProductIds.Any(prd => prd == t.TfxProductId.Value))
                                                                      .Select(t => t.Id).ToListAsync();
                }

                var allJobs = await Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == userContext.CompanyId
                                         && !t.BuyerCompany.IsDeleted
                                         && (filter.SelectedCustomerIds.Count == 0 || filter.SelectedCustomerIds.Any(buyer => buyer == t.BuyerCompanyId))
                                         && (filter.SelectedStateIds.Count == 0 || filter.SelectedStateIds.Any(state => state == t.FuelRequest.Job.StateId))
                                         && (productIds.Count == 0 || productIds.Any(prd => prd == t.FuelRequest.FuelTypeId))
                                         && (filter.IsCarrierMnagedLocations == false || t.FuelRequest.Job.LocationManagedType == LocationManagedType.FullyCarrierManaged)
                                         && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                         && t.IsActive
                                         //&&
                                         //(
                                         //   t.FuelRequest.Job.JobXAssets.Any(x => x.Asset.Type == (int)AssetType.Tank && x.RemovedBy == null && x.RemovedDate == null)
                                         //)
                                         )
                                        .Select(t => new CustomerJobsModel
                                        {
                                            CustomerId = t.BuyerCompanyId,
                                            CustomerName = t.BuyerCompany.Name,
                                            JobId = t.FuelRequest.JobId,
                                            StateCode = t.FuelRequest.Job.MstState.Code,
                                            ZipCode = t.FuelRequest.Job.ZipCode,
                                            LocationName = t.FuelRequest.Job.Name,
                                            TimeZoneName = t.FuelRequest.Job.TimeZoneName,
                                            LocationManagedType = (int)t.FuelRequest.Job.LocationManagedType
                                        })
                                        .Distinct()
                                        .ToListAsync();
                if (allJobs != null && allJobs.Any())
                {
                    var salesDomain = new SalesDomain();
                    filter.CustomerJobs = allJobs;
                    filter.CompanyId = userContext.CompanyId;

                    var respData = await salesDomain.GetInventoryDataForDashboard(filter);
                    if (respData != null && respData.StatusCode == Status.Success)
                    {
                        response.ResponseData = respData.InventoryData;
                        response.StatusCode = Status.Success;
                    }
                    else
                    {
                        response.StatusMessage = Status.Failed.ToString();
                    }
                }
                else
                {
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errorMsgLocationNotAvailable;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardDomain", "GetInventoryDataForDashboard", ex.Message, ex);
            }

            return response;
        }
    }
}

