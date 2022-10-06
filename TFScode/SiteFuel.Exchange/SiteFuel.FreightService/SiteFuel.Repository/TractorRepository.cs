using MongoDB.Bson;
using MongoDB.Driver;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository.Mappers;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.MdbDataAccess.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public class TractorRepository : ITractorRepository
    {
        private readonly MdbContext mdbContext;
        public TractorRepository(MdbContext _context)
        {
            mdbContext = _context;
        }

        public async Task<StatusModel> SaveTractorDetails(TractorDetailViewModel model)
        {
            var response = new StatusModel();

            // Create a session object that is used when leveraging transactions
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                // Begin transaction
                session.StartTransaction();
                try
                {
                    var details = mdbContext.TractorDetails.Find(n => n.TractorId == model.TractorId && n.TfxCompanyId == model.TfxCompanyId && !n.IsDeleted).FirstOrDefault();
                    if (details == null)
                    {
                        var entity = model.ToEntity();
                        await mdbContext.TractorDetails.InsertOneAsync(entity);

                        // Made it here without error? Let's commit the transaction
                        await session.CommitTransactionAsync();

                        response.StatusCode = (int)Status.Success;
                    }
                    else
                    {
                        response.StatusCode = (int)Status.Warning;

                        response.StatusMessage = model.TractorId + " tractorId already exists";
                    }
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    response.StatusCode = (int)Status.Failed;
                    Exchange.Logger.LogManager.Logger.WriteException("TractorRepository", "SaveTractorDetails", ex.Message, ex);
                }
            }
            return response;
        }
        public async Task<StatusModel> UpdateTractorDetails(TractorDetailViewModel model)
        {
            var response = new StatusModel();
            // Create a session object that is used when leveraging transactions
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                // Begin transaction
                session.StartTransaction();
                try
                {
                    var entity = model.ToEntity();
                    if (!string.IsNullOrEmpty(model.Id))
                    {
                        var bsonId = ObjectId.Parse(model.Id);
                        await mdbContext.TractorDetails.FindOneAndUpdateAsync(Builders<TractorDetail>.Filter.Eq(n => n.Id, bsonId),
                          Builders<TractorDetail>.Update
                          .Set(m => m.TractorId, entity.TractorId)
                          .Set(m => m.VIN, entity.VIN)
                          .Set(m => m.ExpirationDate, entity.ExpirationDate)
                          .Set(m => m.Plate, entity.Plate)
                          .Set(m => m.TrailerType, entity.TrailerType)
                          .Set(m => m.Owner, entity.Owner)
                          .Set(m => m.Drivers, entity.Drivers)
                          .Set(m => m.Status, entity.Status)
                          .Set(m => m.Owner, entity.Owner)
                          .Set(m => m.TfxUpdatedy, entity.TfxCreatedBy)
                          .Set(m => m.UpdatedDate, DateTime.Now)
                          );
                    }

                    // Made it here without error? Let's commit the transaction
                    await session.CommitTransactionAsync();

                    response.StatusCode = (int)Status.Success;
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    response.StatusCode = (int)Status.Failed;
                    Exchange.Logger.LogManager.Logger.WriteException("TractorRepository", "UpdateTractorDetails", ex.Message, ex);
                }
            }
            return response;
        }
        public async Task<List<TractorDetailViewModel>> GetAllTractorDetails(int companyId)
        {
            try
            {
                var trucks = await mdbContext.TractorDetails.Find(x => x.TfxCompanyId == companyId && !x.IsDeleted).ToListAsync();
                if (trucks != null)
                {
                    var allTrucks = trucks.OrderByDescending(t => t.CreatedDate).ThenByDescending(top=>top.UpdatedDate).ToList();
                    var response = new List<TractorDetailViewModel>();
                    allTrucks.ForEach(x => response.Add(x.ToViewModel()));
                    return response;
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("TractorRepository", "GetAllTruckDetails", ex.Message, ex);
            }
            return null;
        }

        public async Task<StatusModel> DeleteTractor(TractorDetailViewModel requestModel)
        {
            var response = new StatusModel();
            // Create a session object that is used when leveraging transactions
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                // Begin transaction
                session.StartTransaction();
                try
                {
                    if (!string.IsNullOrEmpty(requestModel.Id))
                    {
                        var bsonId = ObjectId.Parse(requestModel.Id);
                        await mdbContext.TractorDetails.FindOneAndUpdateAsync(Builders<TractorDetail>.Filter.Eq(n => n.Id, bsonId),
                          Builders<TractorDetail>.Update.Set(m => m.IsDeleted, true).Set(m => m.Status, TractorStatus.InActive)
                                                        .Set(m => m.TfxCreatedBy, requestModel.TfxCreatedBy)
                                                        .Set(m => m.CreatedDate, requestModel.CreatedDate)
                                                        .Set(m => m.TfxCompanyId, requestModel.TfxCompanyId));

                        // Made it here without error? Let's commit the transaction
                        await session.CommitTransactionAsync();

                        response.StatusCode = (int)Status.Success;
                        response.StatusMessage = $"{requestModel.TractorId} tractor deleted Successfully";
                    }
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    response.StatusCode = (int)Status.Failed;
                    Exchange.Logger.LogManager.Logger.WriteException("TractorRepository", "DeleteTractorAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<Exchange.Utilities.DropdownDisplayItem>> GetAllDrivers(int companyId, string trailerTypeId)
        {
            var response = new List<Exchange.Utilities.DropdownDisplayItem>();

            try
            {
                var driverdetails = await mdbContext.Drivers.Find(x => x.CompanyId == companyId && !x.IsDeleted).ToListAsync();
                if (driverdetails != null)
                {
                    if (!string.IsNullOrEmpty(trailerTypeId))
                    {
                        foreach (var item in trailerTypeId.Split(','))
                        {
                            TrailerTypeStatus trailerTypeStatus = (TrailerTypeStatus)Enum.Parse(typeof(TrailerTypeStatus), item);
                            var trailerAssociatedDriver = driverdetails.Where(top => top.TrailerType.Contains(trailerTypeStatus)).ToList();
                            foreach (var driveritem in trailerAssociatedDriver)
                            {
                                response.Add(new Exchange.Utilities.DropdownDisplayItem { Id = driveritem.DriverId, Name = driveritem.DriverName });
                            }
                        }

                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("TractorRepository", "GetAllTruckDetails", ex.Message, ex);
            }
            return response;
        }


    }
}
