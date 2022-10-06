using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class DsbLoadQueueDomain : BaseDomain
    {
        public DsbLoadQueueDomain()
              : base(ContextFactory.Current.ConnectionString)
        {
        }

        public DsbLoadQueueDomain(BaseDomain domain)
            : base(domain)
        {
        }
        public async Task<LoadQueueStatusViewModel> SaveDsbLoadQueue(List<DsbLoadQueueViewModel> loadQueueViewModel, UserContext userContext)
        {
            var statusModel = new LoadQueueStatusViewModel();
            statusModel.StatusCode = Status.Success;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (loadQueueViewModel != null && loadQueueViewModel.Any())
                    {
                        loadQueueViewModel.ForEach(x => { x.TfxUserId = userContext.Id; x.TfxCompanyId = userContext.CompanyId; x.CreatedBy = userContext.Id; x.TfxUserName = userContext.UserName; });
                        var loadQueueInfos = loadQueueViewModel.ToEntity();
                        var loadQueueFilterInfo = loadQueueViewModel.FirstOrDefault();
                        var dsbLoadQueueDetails = Context.DataContext.DsbLoadQueueDetails.Where(top => (top.Date == loadQueueFilterInfo.Date || top.ScheduleBuilderId == loadQueueFilterInfo.ScheduleBuilderId) && top.ShiftIndex == loadQueueFilterInfo.ShiftIndex && top.RegionId == loadQueueFilterInfo.RegionId && top.TfxCompanyId == loadQueueFilterInfo.TfxCompanyId && top.TfxUserId == loadQueueFilterInfo.TfxUserId && (top.ProcessStatus == (int)DsbLoadQueueStatus.New));
                        //verify the load queue info with already exists with status in new & inprogress.
                        VerifyDsbLoadQueueInfo(statusModel, loadQueueInfos, loadQueueFilterInfo, dsbLoadQueueDetails);
                        if (!statusModel.LoadQueueErrorInfo.Any())
                        {
                            Context.DataContext.DsbLoadQueueDetails.AddRange(loadQueueInfos);
                            await Context.CommitAsync();
                            statusModel.LoadQueueSuccessInfo = loadQueueViewModel.ToStatusEntity();
                        }
                    }
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    statusModel.StatusCode = Status.Failed;
                    LogManager.Logger.WriteException("DsbLoadQueueDomain", "SaveDsbLoadQueue", ex.Message, ex);
                }
            }
            return statusModel;
        }
        public async Task<List<DsbLoadQueueStatusViewModel>> ValidateLoadQueue(List<DsbLoadQueueViewModel> loadQueueViewModel, UserContext userContext)
        {
            var statusModel = new List<DsbLoadQueueStatusViewModel>();
            List<int> columnsValidation = new List<int>();
            if (loadQueueViewModel != null && loadQueueViewModel.Any())
            {
                loadQueueViewModel.ForEach(x => { x.TfxUserId = userContext.Id; x.TfxCompanyId = userContext.CompanyId; });
                var dsbLoadQueueFilterInfo = loadQueueViewModel.FirstOrDefault();
                var shiftColumnResult = loadQueueViewModel.ToLookup(x => x.ShiftIndex);
                var loadQueueDetails = await Context.DataContext.DsbLoadQueueDetails.Where(top => (top.Date == dsbLoadQueueFilterInfo.Date || top.ScheduleBuilderId == dsbLoadQueueFilterInfo.ScheduleBuilderId) && top.RegionId == dsbLoadQueueFilterInfo.RegionId && top.TfxCompanyId == dsbLoadQueueFilterInfo.TfxCompanyId && (top.ProcessStatus != (int)DsbLoadQueueStatus.Success || top.ProcessStatus != (int)DsbLoadQueueStatus.Failed)).ToListAsync();
                if (loadQueueDetails != null && loadQueueDetails.Any())
                {
                    var dsbLoadQueueInfo = loadQueueDetails.ToValidateEntity();
                    var getUserInfomation = await GetLoadUserDetails(dsbLoadQueueInfo.Select(top => top.TfxUserId).ToList());
                    foreach (var loadQueueItem in dsbLoadQueueInfo)
                    {
                        var userInfo = getUserInfomation.Where(top => top.Id == loadQueueItem.TfxUserId).FirstOrDefault();
                        if (userInfo != null)
                            loadQueueItem.TfxUserName = userInfo.UserName;
                    }
                    foreach (var groupShift in shiftColumnResult)
                    {
                        //validate the shift columns.
                        var shiftColIndexs = groupShift.Select(top => top.DriverColIndex).ToList();
                        var shiftColExists = dsbLoadQueueInfo.Where(top => top.ShiftIndex == groupShift.Key && top.TfxUserId != userContext.Id && shiftColIndexs.Contains(top.DriverColIndex)).ToList();
                        if (shiftColExists.Any())
                        {
                            ValidateShiftColumns(loadQueueViewModel, statusModel, shiftColExists);

                        }
                        //validate the shift drivers.
                        var shiftColExistsCheck = shiftColExists.Select(top => top.DriverColIndex).ToList();
                        shiftColExistsCheck.ForEach(x => columnsValidation.Add(x));
                        var shiftDriverDetails = groupShift.Select(top => top.TfxDriverId).ToList();
                        var shiftColDriverExists = dsbLoadQueueInfo.Where(top => top.ShiftIndex == groupShift.Key && top.TfxUserId != userContext.Id && shiftDriverDetails.Contains(top.TfxDriverId) && !shiftColExistsCheck.Contains(top.DriverColIndex)).ToList();
                        if (shiftColDriverExists.Any())
                        {
                            ValidateShiftDrivers(loadQueueViewModel, statusModel, shiftColDriverExists);
                        }
                        var shiftDriverExistsCheck = shiftColDriverExists.Select(top => top.DriverColIndex).ToList();
                        shiftDriverExistsCheck.ForEach(x => columnsValidation.Add(x));
                        //validate the shift trailers & delivery requests.
                        ValidateShiftTrailersAndDeliveryRequests(loadQueueViewModel, statusModel, dsbLoadQueueInfo, groupShift, columnsValidation.Distinct().ToList(), userContext);
                    }

                }
            }
            return statusModel;
        }
        public async Task<List<DSBLoadQueueNotificationResponse>> GetLoadQueueNotifications(List<DSBLoadQueueNotificationModel> loadQueueViewModel, UserContext userContext)
        {
            var statusModel = new List<DSBLoadQueueNotificationResponse>();
            if (loadQueueViewModel != null && loadQueueViewModel.Any())
            {
                loadQueueViewModel.ForEach(x => { x.TfxUserId = userContext.Id; x.TfxCompanyId = userContext.CompanyId; });
                var dsbLoadQueueNotificationInfo = loadQueueViewModel.FirstOrDefault();
                var dsbLoadQueueShiftIds = loadQueueViewModel.Select(top => top.ShiftId).ToList();
                var dsbLoadQueueShiftIndex = loadQueueViewModel.Select(top => top.ShiftIndex).ToList();
                var dsbLoadQueueDriverColIndex = loadQueueViewModel.Select(top => top.DriverColIndex).ToList();
                if (dsbLoadQueueNotificationInfo != null)
                {
                    var dsbNotifications = await Context.DataContext.DsbLoadQueueDetails.Where(top => (top.Date == dsbLoadQueueNotificationInfo.Date || top.ScheduleBuilderId == dsbLoadQueueNotificationInfo.ScheduleBuilderId) && top.RegionId == dsbLoadQueueNotificationInfo.RegionId && top.TfxCompanyId == dsbLoadQueueNotificationInfo.TfxCompanyId && top.TfxUserId == dsbLoadQueueNotificationInfo.TfxUserId).ToListAsync();
                    if (dsbNotifications.Any())
                    {
                        dsbNotifications = dsbNotifications.Where(top => dsbLoadQueueShiftIds.Contains(top.ShiftId) && dsbLoadQueueShiftIndex.Contains(top.ShiftIndex) && dsbLoadQueueDriverColIndex.Contains(top.DriverColIndex)).ToList();
                        if (dsbNotifications.Any())
                        {
                            foreach (var item in dsbNotifications)
                                IntializeDSBSaveModel(userContext, statusModel, item);
                        }
                    }
                }
            }
            return statusModel;
        }
        public async Task<List<DsbLoadQueueViewModel>> GetLoadQueueDetails()
        {
            var statusModel = new List<DsbLoadQueueViewModel>();
            var dsbLoadQueue = await Context.DataContext.DsbLoadQueueDetails.Where(top => (top.ProcessStatus == (int)DsbLoadQueueStatus.New || top.ProcessStatus == (int)DsbLoadQueueStatus.Failed) && top.FailedCount < 2).ToListAsync();
            statusModel = dsbLoadQueue.ToEntity();
            return statusModel;
        }
        public async Task<StatusViewModel> UpdateLoadQueueStatus(List<int> LoadQueueId, DsbLoadQueueStatus dsbLoadQueueStatus)
        {
            var statusModel = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (LoadQueueId.Any())
                    {
                        var dsbLoadQueue = await Context.DataContext.DsbLoadQueueDetails.Where(top => LoadQueueId.Contains(top.Id) && (top.ProcessStatus != (int)DsbLoadQueueStatus.Success || top.ProcessStatus != (int)DsbLoadQueueStatus.Failed)).ToListAsync();
                        if (dsbLoadQueue != null && dsbLoadQueue.Any())
                        {
                            dsbLoadQueue.ForEach(x => x.ProcessStatus = (int)dsbLoadQueueStatus);
                            await Context.DataContext.SaveChangesAsync();

                        }
                        else
                        {
                            statusModel.StatusCode = Status.Failed;
                        }
                    }
                    transaction.Commit();
                    statusModel.StatusCode = Status.Success;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    statusModel.StatusCode = Status.Failed;
                    LogManager.Logger.WriteException("DsbLoadQueueDomain", "UpdateLoadQueueStatus", ex.Message, ex);
                }
            }
            return statusModel;
        }
        public async Task<StatusViewModel> UpdateLoadQueueFailedStatus(int LoadQueueId, DsbLoadQueueStatus dsbLoadQueueStatus, string message)
        {
            var statusModel = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var dsbLoadQueue = await Context.DataContext.DsbLoadQueueDetails.FirstOrDefaultAsync(top => top.Id == LoadQueueId);
                    if (dsbLoadQueue != null)
                    {
                        dsbLoadQueue.ProcessStatus = (int)dsbLoadQueueStatus;
                        dsbLoadQueue.FailedCount = dsbLoadQueue.FailedCount + 1;
                        dsbLoadQueue.DriverColJsonResponse = message;
                        await Context.DataContext.SaveChangesAsync();
                    }
                    transaction.Commit();
                    statusModel.StatusCode = Status.Success;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    statusModel.StatusCode = Status.Failed;
                    LogManager.Logger.WriteException("DsbLoadQueueDomain", "UpdateLoadQueueFailedStatus", ex.Message, ex);
                }
            }
            return statusModel;
        }
        public async Task<List<DsbLoadQueueUserModel>> GetLoadUserDetails(List<int> UserIds)
        {
            var statusModel = new List<DsbLoadQueueUserModel>();
            if (UserIds.Any())
            {
                var dsbLoadQueueUserDetails = await Context.DataContext.Users.Where(top => UserIds.Contains(top.Id)).Select(x => new { x.Id, x.UserName }).ToListAsync();
                foreach (var item in dsbLoadQueueUserDetails)
                {
                    statusModel.Add(new DsbLoadQueueUserModel { Id = item.Id, UserName = item.UserName });
                }
            }
            return statusModel;
        }
        public async Task<bool> UpdateLoadQueueResponse(List<DsbLoadQueueSuccessModel> dsbLoadQueueModel)
        {
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var dsbLoadQueueIds = dsbLoadQueueModel.Select(top => top.Id).ToList();
                    var dsbLoadQueue = await Context.DataContext.DsbLoadQueueDetails.Where(top => dsbLoadQueueIds.Contains(top.Id)).ToListAsync();
                    if (dsbLoadQueue != null && dsbLoadQueue.Any())
                    {
                        foreach (var item in dsbLoadQueue)
                        {
                            var dsbColJsonResponse = dsbLoadQueueModel.FirstOrDefault(top => top.Id == item.Id) != null ? dsbLoadQueueModel.FirstOrDefault(top => top.Id == item.Id).DSBSaveModel : string.Empty;
                            if (!string.IsNullOrEmpty(dsbColJsonResponse))
                            {
                                item.ProcessStatus = (int)DsbLoadQueueStatus.Success;
                                item.DriverColJsonResponse = dsbColJsonResponse;
                            }
                        }
                        await Context.DataContext.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("DsbLoadQueueDomain", "UpdateLoadQueueResponse", ex.Message, ex);
                    return false;
                }
            }
            return true;
        }
        private static void ValidateShiftColumns(List<DsbLoadQueueViewModel> loadQueueViewModel, List<DsbLoadQueueStatusViewModel> statusModel, List<DsbLoadQueueValidateViewModel> driverColExists)
        {
            foreach (var shiftCol in driverColExists)
            {
                var strMessage = string.Format(Resource.valMessageLoadQueueColumnExists, shiftCol.ShiftIndex + 1, shiftCol.DriverColIndex + 1, shiftCol.TfxUserName);
                FormatShiftErrorMessage(statusModel, shiftCol, strMessage);
            }
        }
        private static void ValidateShiftDrivers(List<DsbLoadQueueViewModel> loadQueueViewModel, List<DsbLoadQueueStatusViewModel> statusModel, List<DsbLoadQueueValidateViewModel> driverColExists)
        {
            foreach (var shiftdriver in driverColExists)
            {
                string driverName = loadQueueViewModel.FirstOrDefault(top => top.TfxDriverId == shiftdriver.TfxDriverId) != null ? loadQueueViewModel.FirstOrDefault(top => top.TfxDriverId == shiftdriver.TfxDriverId).TfxDriverName : string.Empty;
                var strMessage = string.Format(Resource.valMessageLoadQueueDriverExists, driverName, shiftdriver.ShiftIndex + 1, shiftdriver.DriverColIndex + 1, shiftdriver.TfxUserName);
                FormatShiftErrorMessage(statusModel, shiftdriver, strMessage);
            }
        }
        private static void ValidateShiftTrailersAndDeliveryRequests(List<DsbLoadQueueViewModel> loadQueueViewModel, List<DsbLoadQueueStatusViewModel> statusModel, List<DsbLoadQueueValidateViewModel> dsbLoadQueueInfo, IGrouping<int, DsbLoadQueueViewModel> groupShift, List<int> columnsValidation, UserContext userContext)
        {
            
            foreach (var dsbgroupColumnInfo in groupShift)
            {
                var trailerDetails = dsbgroupColumnInfo.TrailerDetails.Select(top => top.Id).ToList();
                var shiftColTrailerExists = dsbLoadQueueInfo.Where(top => top.ShiftIndex == groupShift.Key && top.TfxUserId != userContext.Id && trailerDetails.Contains(top.TrailerInfo) && !columnsValidation.Contains(top.DriverColIndex)).ToList();
                foreach (var item in shiftColTrailerExists)
                {
                    var TrailerInfo = JsonConvert.DeserializeObject<List<TrailerInfo>>(item.TrailerInfo);
                    var commonTrailerElements = dsbgroupColumnInfo.TrailerDetails.Select(s1 => s1.TrailerId).ToList().Intersect(TrailerInfo.Select(s2 => s2.TrailerId).ToList()).ToList();
                    var strMessage = string.Format(Resource.valMessageLoadQueueTrailerExists, string.Join(",", commonTrailerElements), item.ShiftIndex + 1, item.DriverColIndex + 1, item.TfxUserName);
                    FormatShiftErrorMessage(statusModel, item, strMessage);
                }
                var deliveryRequestInfo = dsbgroupColumnInfo.DeliveryRequestDetails.ToList();
                var shiftColdeliveryRequestExists = dsbLoadQueueInfo.Where(top => top.ShiftIndex == groupShift.Key && top.TfxUserId != userContext.Id && deliveryRequestInfo.Contains(top.DeliveryRequestInfo)).ToList();
                foreach (var item in shiftColdeliveryRequestExists)
                {
                    var strMessage = string.Format(Resource.valMessageLoadQueueDeliveryRequestExists, item.ShiftIndex + 1, item.DriverColIndex + 1, item.TfxUserName);
                    FormatShiftErrorMessage(statusModel, item, strMessage);
                }
            }
        }
        private static void FormatShiftErrorMessage(List<DsbLoadQueueStatusViewModel> statusModel, DsbLoadQueueValidateViewModel item, string strMessage)
        {
            List<LoadQueueStatus> loadQueueStatuses = new List<LoadQueueStatus>();
            LoadQueueStatus loadQueueStatus = new LoadQueueStatus();
            loadQueueStatus.StatusMessage = strMessage;
            loadQueueStatuses.Add(loadQueueStatus);
            statusModel.Add(new DsbLoadQueueStatusViewModel { ShiftIndex = item.ShiftIndex, DriverColIndex = item.DriverColIndex, TfxCompanyId = item.TfxCompanyId, TfxUserId = item.TfxUserId, Messages = loadQueueStatuses });
        }
        private static void VerifyDsbLoadQueueInfo(LoadQueueStatusViewModel statusModel, List<DsbLoadQueueDetails> loadQueueInfos, DsbLoadQueueViewModel loadQueueFilterInfo, IQueryable<DataAccess.Entities.DsbLoadQueueDetails> dsbLoadQueueDetails)
        {
            foreach (var loadQueueitem in loadQueueInfos)
            {
                var dsbLoadQueueExists = dsbLoadQueueDetails.FirstOrDefault(top => top.ShiftIndex == loadQueueitem.ShiftIndex && top.DriverColIndex == loadQueueFilterInfo.DriverColIndex && top.TfxCompanyId == loadQueueFilterInfo.TfxCompanyId && top.TfxUserId == loadQueueFilterInfo.TfxUserId && (top.ProcessStatus == (int)DsbLoadQueueStatus.New || top.ProcessStatus == (int)DsbLoadQueueStatus.InProgress));
                if (dsbLoadQueueExists != null)
                {
                    DsbLoadQueueStatusViewModel dsbLoadQueueStatusModel = new DsbLoadQueueStatusViewModel();
                    dsbLoadQueueStatusModel.ShiftIndex = dsbLoadQueueExists.ShiftIndex;
                    dsbLoadQueueStatusModel.DriverColIndex = dsbLoadQueueExists.DriverColIndex;
                    dsbLoadQueueStatusModel.TfxUserId = dsbLoadQueueExists.TfxUserId;
                    dsbLoadQueueStatusModel.TfxCompanyId = dsbLoadQueueExists.TfxCompanyId;
                    var enumDisplayStatus = (DsbLoadQueueStatus)dsbLoadQueueExists.ProcessStatus;
                    string processStatus = enumDisplayStatus.ToString();
                    dsbLoadQueueStatusModel.Messages = new List<LoadQueueStatus>();
                    LoadQueueStatus queueStatus = new LoadQueueStatus();
                    queueStatus.StatusMessage = string.Format(Resource.valMessageLoadQueueExists, dsbLoadQueueExists.ShiftIndex, dsbLoadQueueExists.DriverColIndex, processStatus);
                    dsbLoadQueueStatusModel.Messages.Add(queueStatus);
                    statusModel.LoadQueueErrorInfo.Add(dsbLoadQueueStatusModel);
                }
            }
        }
        private static void IntializeDSBSaveModel(UserContext userContext, List<DSBLoadQueueNotificationResponse> statusModel, DsbLoadQueueDetails item)
        {
            if (!string.IsNullOrEmpty(item.DriverColJsonResponse) && item.ProcessStatus == (int)DsbLoadQueueStatus.Success)
            {
                var startTime = DateTime.Now;
                var dSBSaveModelObject = JsonConvert.DeserializeObject<DSBSaveModel>(item.DriverColJsonResponse);
                if (dSBSaveModelObject != null)
                {
                    if (dSBSaveModelObject.StatusCode != Status.Failed)
                    {
                        foreach (var trip in dSBSaveModelObject.Trips)
                        {
                            ScheduleBuilderConverter.InitializePickupLocation(trip);
                            ScheduleBuilderConverter.InitializeBadgeNumberForDeliveryRequests(trip);
                        }
                    }
                    dSBSaveModelObject.Status = DSBMethod.None;
                }
                else
                {
                    dSBSaveModelObject = new DSBSaveModel();
                    dSBSaveModelObject.StatusCode = Status.Failed;
                    dSBSaveModelObject.StatusMessage = Resource.valMessageServiceNotResponded;

                }
                var responseModelJson = JsonConvert.SerializeObject(dSBSaveModelObject);
                LogManager.Logger.WriteAPIInfo(userContext.UserName, "ScheduleBuilderController", "GetLoadQueueNotifications", item.DriverColJsonResponse, responseModelJson, 0, "Azure-AppService", startTime, DateTime.Now);
                statusModel.Add(new DSBLoadQueueNotificationResponse { Date = item.Date, DriverColIndex = item.DriverColIndex, RegionId = item.RegionId, ScheduleBuilderId = item.ScheduleBuilderId, ShiftId = item.ShiftId, ShiftIndex = item.ShiftIndex, TfxCompanyId = item.TfxCompanyId, TfxUserId = item.TfxUserId, Status = item.ProcessStatus, DSBSaveModel = dSBSaveModelObject });
            }
            else
            {
                statusModel.Add(new DSBLoadQueueNotificationResponse { Date = item.Date, DriverColIndex = item.DriverColIndex, RegionId = item.RegionId, ScheduleBuilderId = item.ScheduleBuilderId, ShiftId = item.ShiftId, ShiftIndex = item.ShiftIndex, TfxCompanyId = item.TfxCompanyId, TfxUserId = item.TfxUserId, Status = item.ProcessStatus });
            }
        }
        private void IntializeDsbLoadQueueDriverInfo(List<DsbLoadQueueViewModel> loadQueueViewModel, List<int> dsbLoadQueueDriverIds)
        {
            var dbDriverDetails = Context.DataContext.Users.Where(top => dsbLoadQueueDriverIds.Contains(top.Id)).Select(x => new { x.Id, x.UserName }).ToList();
            if (dbDriverDetails.Any())
            {
                foreach (var dsbDriveritem in loadQueueViewModel)
                {
                    var dsbDriverInfo = dbDriverDetails.FirstOrDefault(top => top.Id == dsbDriveritem.Id);
                    if (dsbDriverInfo != null)
                    {
                        dsbDriveritem.TfxDriverName = dsbDriverInfo.UserName;
                    }
                }
            }
        }
    }
}
