using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository.Mappers;
using SiteFuel.MdbDataAccess;
using SiteFuel.MdbDataAccess.DbContext;

namespace SiteFuel.FreightRepository
{
    public class DSBLoadQueueRepository : IDSBLoadQueueRepository
    {
        private readonly MdbContext mdbContext;
        public DSBLoadQueueRepository(MdbContext _context)
        {
            mdbContext = _context;
        }
        public async Task<StatusModel> CreateDsbLoadQueue(List<DSBLoadQueueModel> dSBLoadQueue)
        {
            var response = new StatusModel();
            var dSBLoadQueueInfo = dSBLoadQueue.ToEntity();
            var dsbLoadQueueExistingList = new List<DSBLoadQueue>();
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    if (dSBLoadQueueInfo.Any())
                    {
                        var dsbLaodQueueFirst = dSBLoadQueueInfo.FirstOrDefault();
                        var dsbLoadQueueDetails = mdbContext.DSBLoadQueues.Find(top => (top.Date == dsbLaodQueueFirst.Date || top.ScheduleBuilderId == dsbLaodQueueFirst.ScheduleBuilderId) && top.RegionId == dsbLaodQueueFirst.RegionId && top.TfxUserId == dsbLaodQueueFirst.TfxUserId && top.TfxCompanyId == dsbLaodQueueFirst.TfxCompanyId).ToList();
                        var schebuilderInfo = mdbContext.ScheduleBuilders.Find(top => top.IsActive && !top.IsDeleted && top.DateFilter == dSBLoadQueueInfo.FirstOrDefault().Date).Project(x => new { x.Id }).FirstOrDefault();
                        if (schebuilderInfo != null)
                        {
                            dSBLoadQueueInfo.ForEach(x => x.ScheduleBuilderId = schebuilderInfo.Id);
                        }
                        foreach (var dsbLoadQueueItem in dsbLoadQueueDetails)
                        {
                            var dsbLoadQueueIndex = dSBLoadQueueInfo.FindIndex(top => top.ShiftIndex == dsbLoadQueueItem.ShiftIndex && top.DriverRowIndex == dsbLoadQueueItem.DriverRowIndex && top.TfxCompanyId == dsbLoadQueueItem.TfxCompanyId && top.TfxUserId == dsbLoadQueueItem.TfxUserId);
                            if (dsbLoadQueueIndex >= 0)
                            {
                                dSBLoadQueueInfo.RemoveAt(dsbLoadQueueIndex);
                                dsbLoadQueueExistingList.Add(dsbLoadQueueItem);
                            }
                        }
                    }
                    if (dSBLoadQueueInfo.Any())
                    {
                        await mdbContext.DSBLoadQueues.InsertManyAsync(dSBLoadQueueInfo);
                    }
                    //add existing list to dsbloadQueue
                    if (dsbLoadQueueExistingList.Any())
                    {
                        dSBLoadQueueInfo.AddRange(dsbLoadQueueExistingList);
                    }
                    dSBLoadQueueInfo.ForEach(item =>
                    {
                        response.DsbLoadQueueSuccess.Add(new DsbLoadQueueSuccess { Id = item.Id.ToString(), RegionId = item.RegionId.ToString(), ShiftId = item.ShiftId.ToString(), ShiftIndex = item.ShiftIndex, DriverRowIndex = item.DriverRowIndex });
                    });

                    await session.CommitTransactionAsync();
                    response.StatusCode = (int)Status.Success;
                }
                catch (Exception)
                {
                    await session.AbortTransactionAsync();
                    throw;
                }
            }

            return response;
        }

        public async Task<StatusModel> DeleteDsbLoadQueue(List<string> dSBLoadQueue)
        {
            var response = new StatusModel();
            List<ObjectId> objectIds = new List<ObjectId>();
            foreach (var item in dSBLoadQueue)
            {
                ObjectId dsbLoadObj = ObjectId.Empty;
                if (ObjectId.TryParse(item, out dsbLoadObj))
                {
                    objectIds.Add(dsbLoadObj);
                }
            }
            if (objectIds.Any())
            {
                using (var session = await mdbContext.Client.StartSessionAsync())
                {
                    session.StartTransaction();
                    try
                    {
                        var dsbLoadQueueFilter = Builders<DSBLoadQueue>.Filter.And(
                                                                   Builders<DSBLoadQueue>.Filter
                                                                  .Where(x => objectIds.Contains(x.Id)));
                        await mdbContext.DSBLoadQueues.DeleteManyAsync(dsbLoadQueueFilter);
                        await session.CommitTransactionAsync();
                        response.StatusCode = (int)Status.Success;
                    }
                    catch (Exception)
                    {
                        await session.AbortTransactionAsync();
                        throw;
                    }
                }
            }
            else
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.errorNoDsbLoadQueue;
            }
            return response;
        }
    }
}
