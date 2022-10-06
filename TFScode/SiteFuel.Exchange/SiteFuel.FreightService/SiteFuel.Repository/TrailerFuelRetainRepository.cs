using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository.Mappers;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.MdbDataAccess.DbContext;

namespace SiteFuel.FreightRepository
{
    public class TrailerFuelRetainRepository : ITrailerFuelRetainRepository
    {
        private readonly MdbContext mdbContext;
        public TrailerFuelRetainRepository(MdbContext _context)
        {
            mdbContext = _context;
        }

        public async Task<StatusModel> SaveTrailerFuelRetain(List<TrailerFuelRetainViewModel> model)
        {
            var response = new StatusModel();

            if (model != null)
            {
                // Create a session object that is used when leveraging transactions
                using (var session = await mdbContext.Client.StartSessionAsync())
                {
                    // Begin transaction
                    session.StartTransaction();
                    try
                    {
                        foreach (var item in model)
                        {
                            var entity = item.ToEntity();
                            await mdbContext.TrailerFuelRetains.InsertOneAsync(entity);
                        }
                        // Made it here without error? Let's commit the transaction
                        await session.CommitTransactionAsync();

                        response.StatusCode = (int)Status.Success;
                    }
                    catch (Exception ex)
                    {
                        await session.AbortTransactionAsync();
                        response.StatusCode = (int)Status.Failed;
                        response.StatusMessage = ex.Message;
                        Exchange.Logger.LogManager.Logger.WriteException("TrailerFuelRetainRepository", "SaveTrailerFuelRetain", ex.Message, ex);
                    }
                }

            }
            return response;
        }
        public async Task<List<TrailerFuelRetainViewModel>> GetTrailerFuelRetainDetails(string trailerId)
        {
            var response = new List<TrailerFuelRetainViewModel>();

            if (!string.IsNullOrEmpty(trailerId))
            {
                try
                {
                    var objectId = ObjectId.Parse(trailerId);
                    var trailerFuelRetainDetails = await mdbContext.TrailerFuelRetains.Find(x => x.TrailerId == objectId && !x.IsDeleted && x.IsActive).ToListAsync();
                    if (trailerFuelRetainDetails != null)
                    {
                        response = trailerFuelRetainDetails.ToEntity();
                    }
                }
                catch (Exception ex)
                {
                    Exchange.Logger.LogManager.Logger.WriteException("TrailerFuelRetainRepository", "GetTrailerFuelRetainDetails", ex.Message, ex);
                }
            }
            return response;
        }
        public async Task<StatusModel> ResetTrailerFuelRetained(TruckDetailViewModel truckDetailViewModel)
        {
            var response = new StatusModel();

            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    var truck = mdbContext.TruckDetails.Find(x => x.TruckId == truckDetailViewModel.TruckId && x.IsDeleted == false).FirstOrDefaultAsync().Result;
                    if (truck != null)
                    {
                        var fuelRetainDetails = Builders<TrailerFuelRetain>.Filter.Where(x => x.TrailerId == truck.Id && x.IsDeleted == false && x.IsActive);
                        var UpdateFuelRetainDetails = Builders<TrailerFuelRetain>.Update.Set(x => x.IsDeleted, true).Set(x => x.IsActive, false);
                        if (fuelRetainDetails != null && UpdateFuelRetainDetails != null)
                        {
                            UpdateResult updateResult = mdbContext.TrailerFuelRetains.UpdateMany(fuelRetainDetails, UpdateFuelRetainDetails);
                            if (updateResult.IsAcknowledged && updateResult.IsModifiedCountAvailable)
                            {
                                response.StatusCode = (int)Status.Success;
                                response.StatusMessage = "Successfully reset trailer retained fuel";
                                var updateTruckDetails = Builders<TruckDetail>.Filter.Where(x => x.TruckId == truckDetailViewModel.TruckId && x.IsDeleted == false);
                                var UpdatedTruckDetails = Builders<TruckDetail>.Update.Set(x => x.FuelResetUserId, truckDetailViewModel.FuelResetUserId).Set(x => x.FuelResetUserName, truckDetailViewModel.FuelResetUserName);
                                mdbContext.TruckDetails.UpdateOne(updateTruckDetails, UpdatedTruckDetails);
                                await session.CommitTransactionAsync();
                            }
                            else
                            {
                                response.StatusCode = (int)Status.Failed;
                                response.StatusMessage = "";
                                await session.AbortTransactionAsync();
                            }
                            return response;
                        }
                        else
                        {
                            response.StatusCode = (int)Status.Failed;
                            response.StatusMessage = "No records found for reset trailer fuel retained";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exchange.Logger.LogManager.Logger.WriteException("TrailerFuelRetainRepository", "ResetTrailerFuelRetained", ex.Message, ex); ;
                    await session.AbortTransactionAsync();
                }
            }
            return response;
        }
        public async Task<StatusModel> UpdateTrailerFuelRetainDetails(TruckDetailViewModel truckDetailViewModel)
        {
            var response = new StatusModel();
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    foreach (var fuelDetails in truckDetailViewModel.TrailerFuelRetains)
                    {
                        var objectId = ObjectId.Parse(fuelDetails.Id);

                        var RetainFuelDetails = Builders<TrailerFuelRetain>.Filter.Where(x => x.IsActive && !x.IsDeleted && x.Id == objectId && x.IsExceptionConfirmed != 1);

                        if (fuelDetails.Quantity > 0)
                        {
                            var UpdateFuelDetails = Builders<TrailerFuelRetain>.Update.Set(x => x.Quantity, fuelDetails.Quantity)
                                                                               .Set(x => x.UpdatedBy, Convert.ToInt32(truckDetailViewModel.FuelResetUserId))
                                                                               .Set(x => x.UpdatedOn, DateTime.Now)
                                                                               .Set(x => x.IsExceptionConfirmed, 1);
                            mdbContext.TrailerFuelRetains.UpdateOne(RetainFuelDetails, UpdateFuelDetails);
                        }
                        else
                        {
                            var UpdateFuelDetails = Builders<TrailerFuelRetain>.Update.Set(x => x.Quantity, fuelDetails.Quantity)
                                                                               .Set(x => x.UpdatedBy, Convert.ToInt32(truckDetailViewModel.FuelResetUserId))
                                                                               .Set(x => x.UpdatedOn, DateTime.Now)
                                                                               .Set(x => x.IsActive, false)
                                                                               .Set(x => x.IsDeleted, true)
                                                                               .Set(x => x.IsExceptionConfirmed, 1); ;

                            mdbContext.TrailerFuelRetains.UpdateOne(RetainFuelDetails, UpdateFuelDetails);
                        }

                    }
                    await session.CommitTransactionAsync();
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = "Successfully updated trailer retained fuel details";
                }
                catch (Exception ex)
                {
                    Exchange.Logger.LogManager.Logger.WriteException("TrailerFuelRetainRepository", "UpdateTrailerFuelRetainDetails", ex.Message, ex); ;
                    await session.AbortTransactionAsync();
                    response.StatusCode = (int)Status.Failed;
                }
            }
            return response;
        }
        public async Task<StatusModel> ConfirmTrailerFuelRetainException(TruckDetailViewModel truckDetailViewModel)
        {
            var response = new StatusModel();
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    foreach (var fuelDetails in truckDetailViewModel.TrailerFuelRetains)
                    {
                        var objectId = ObjectId.Parse(fuelDetails.Id);

                        var RetainFuelDetails = Builders<TrailerFuelRetain>.Filter.Where(x => x.IsActive && !x.IsDeleted && x.Id == objectId && x.IsExceptionConfirmed != 1);

                        var UpdateFuelDetails = Builders<TrailerFuelRetain>.Update.Set(x => x.UpdatedBy, Convert.ToInt32(truckDetailViewModel.FuelResetUserId))
                                                                                  .Set(x => x.UpdatedOn, DateTime.Now)
                                                                                  .Set(x => x.IsExceptionConfirmed, 1);
                        mdbContext.TrailerFuelRetains.UpdateOne(RetainFuelDetails, UpdateFuelDetails);
                        await session.CommitTransactionAsync();
                        response.StatusCode = (int)Status.Success;
                        response.StatusMessage = "Successfully confirmed trailer retained fuel exception";
                    }
                }
                catch (Exception ex)
                {
                    Exchange.Logger.LogManager.Logger.WriteException("TrailerFuelRetainRepository", "ConfirmTrailerFuelRetainException", ex.Message, ex); ;
                    await session.AbortTransactionAsync();
                    response.StatusCode = (int)Status.Failed;
                }
                return response;
            }
        }
    }
}
