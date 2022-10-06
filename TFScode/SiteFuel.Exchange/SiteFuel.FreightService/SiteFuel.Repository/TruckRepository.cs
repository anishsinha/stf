using MongoDB.Bson;
using MongoDB.Driver;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository.Mappers;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.MdbDataAccess.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using SiteFuel.Exchange.Core.StringResources;

namespace SiteFuel.FreightRepository
{
    public class TruckRepository : ITruckRepository
    {
        private readonly MdbContext mdbContext;
        public TruckRepository()
        {
            if (mdbContext == null)
            {
                mdbContext = new MdbContext();
            }
        }
        public TruckRepository(MdbContext _context)
        {
            mdbContext = _context;

        }

        public async Task<StatusModel> SaveTruckDetails(TruckDetailViewModel model)
        {
            var response = new StatusModel();
            // Create a session object that is used when leveraging transactions
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                // Begin transaction
                session.StartTransaction();

                try
                {
                    var details = mdbContext.TruckDetails.Find(t => t.TruckId == model.TruckId && t.TfxCompanyId == model.TfxCompanyId && !t.IsDeleted).FirstOrDefault();
                    if (details == null)
                    {
                        var entity = model.ToEntity();
                        await mdbContext.TruckDetails.InsertOneAsync(entity);
                        // Made it here without error? Let's commit the transaction
                        await session.CommitTransactionAsync();
                        response.StatusCode = (int)Status.Success;
                    }
                    else
                    {
                        response.StatusCode = (int)Status.Warning;
                        response.StatusMessage = model.TruckId + " trailerId already exists";
                    }


                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    response.StatusCode = (int)Status.Failed;
                    Exchange.Logger.LogManager.Logger.WriteException("TruckRepository", "SaveTruckDetails", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusModel> UpdateTruckDetails(TruckDetailViewModel model)
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
                        await mdbContext.TruckDetails.FindOneAndUpdateAsync(Builders<TruckDetail>.Filter.Eq(n => n.Id, bsonId),
                          Builders<TruckDetail>.Update
                          .Set(m => m.Name, entity.Name)
                          .Set(m => m.Owner, entity.Owner)
                          .Set(m => m.TruckId, entity.TruckId)
                          .Set(m => m.FuelCapacity, entity.FuelCapacity)
                          .Set(m => m.OptimizedCapacity, entity.OptimizedCapacity)
                          .Set(m => m.ContractNumber, entity.ContractNumber)
                          .Set(m => m.TrailerType, entity.TrailerType)
                          .Set(m => m.Compartments, entity.Compartments)
                          .Set(m => m.Status, entity.Status)
                          .Set(m => m.LicenceRequirement, entity.LicenceRequirement)
                          .Set(m => m.LicencePlate, entity.LicencePlate)
                          .Set(m => m.ExpirationDate, entity.ExpirationDate)
                          .Set(m => m.IsPump, entity.IsPump)
                          .Set(m => m.IsFilldCompatible, entity.IsFilldCompatible)
                          .Set(m => m.TfxUpdatedBy, entity.TfxCreatedBy)
                          .Set(m => m.UpdatedDate, DateTime.Now)
                          .Set(m => m.SmartDeviceId, entity.SmartDeviceId)
                          );
                        //update TruckId in region.
                        UpdateRegionTankDetails(model.Id, entity.TruckId);
                    }

                    // Made it here without error? Let's commit the transaction
                    await session.CommitTransactionAsync();

                    response.StatusCode = (int)Status.Success;
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    response.StatusCode = (int)Status.Failed;
                    Exchange.Logger.LogManager.Logger.WriteException("TruckRepository", "UpdateTruckDetails", ex.Message, ex);
                }
            }
            return response;
        }
        /// <summary>
        /// update the TruckId in region.
        /// </summary>
        /// <param name="tankId"></param>
        /// <param name="TruckId"></param>
        public void UpdateRegionTankDetails(string tankId, string TruckId)
        {
            var filter = Builders<Region>.Filter.Where(x => x.TfxTrailers.Any(i => i.Code == tankId));
            var update = Builders<Region>.Update.Set(x => x.TfxTrailers[-1].Name, TruckId);
            mdbContext.Regions.UpdateMany(filter, update);

        }
        public async Task<List<TruckDetailViewModel>> GetAllTruckDetails(int companyId)
        {
            try
            {
                var trucks = await mdbContext.TruckDetails.Find(x => x.TfxCompanyId == companyId && !x.IsDeleted).ToListAsync();
                if (trucks != null)
                {
                    var allTrucks = trucks.OrderByDescending(t => t.CreatedDate).ThenByDescending(t => t.UpdatedDate).ToList();
                    var response = new List<TruckDetailViewModel>();
                    allTrucks.ForEach(x => response.Add(x.ToViewModel()));
                    await GetTrailerFuelRetainDetails(allTrucks, response, false);
                    return response;
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("TruckRepository", "GetAllTruckDetails", ex.Message, ex);
            }
            return null;
        }
        public async Task<DropdownDisplayExtended> GetTruckRegionDetails(string truckId)
        {
            var response = new DropdownDisplayExtended();
            try
            {
                var reg = await mdbContext.Regions.Find(t => t.TfxTrailers.Any(t1 => t1.Code == truckId)).Project(t => new { t.Id, t.Name }).FirstOrDefaultAsync();
                if (reg != null)
                {
                    response.Id = reg.Id.ToString();
                    response.Name = reg.Name;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TruckRepository", "GetTruckRegionDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<TruckDetailViewModel>> GetAllTruckRetainFuelDetails(int companyId)
        {
            try
            {
                var trucks = await mdbContext.TruckDetails.Find(x => x.TfxCompanyId == companyId && !x.IsDeleted).ToListAsync();
                if (trucks != null)
                {
                    var allTrucks = trucks.OrderByDescending(t => t.CreatedDate).ThenByDescending(t => t.UpdatedDate).ToList();
                    var response = new List<TruckDetailViewModel>();
                    allTrucks.ForEach(x => response.Add(x.ToViewModel()));
                    await GetTrailerFuelRetainDetails(allTrucks, response, true);
                    response = response.Where(i => i.TrailerFuelRetains.Any()).ToList();
                    return response;
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("TruckRepository", "GetAllTruckDetails", ex.Message, ex);
            }
            return null;
        }


        public async Task<TruckDetailViewModel> GetTruckDetails(string truckId)
        {
            var response = new TruckDetailViewModel();
            try
            {
                var truckbsonId = ObjectId.Parse(truckId);
                var truck = await mdbContext.TruckDetails.Find(x => x.Id == truckbsonId).FirstOrDefaultAsync();
                if (truck != null)
                {
                    response = truck.ToViewModel();
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("TruckRepository", "GetTruckDetails", ex.Message, ex);
            }
            return response;
        }

        private async Task GetTrailerFuelRetainDetails(List<TruckDetail> allTrucks, List<TruckDetailViewModel> response, bool IsForException)
        {
            string defaultCompartmentId = "Compartment";
            var trailerFuelRetainDetails = new List<TrailerFuelRetain>();
            var objectIds = allTrucks.Select(top => top.Id).ToList();
            if (IsForException)
                trailerFuelRetainDetails = await mdbContext.TrailerFuelRetains.Find(x => objectIds.Contains(x.TrailerId) && !x.IsDeleted && x.IsActive && x.IsExceptionConfirmed != 1).ToListAsync();
            else
                trailerFuelRetainDetails = await mdbContext.TrailerFuelRetains.Find(x => objectIds.Contains(x.TrailerId) && !x.IsDeleted && x.IsActive).ToListAsync();
            foreach (var tFuelRetainDetails in trailerFuelRetainDetails)
            {
                var fuelRetainDetails = response.FirstOrDefault(top => top.Id == tFuelRetainDetails.TrailerId.ToString());
                if (fuelRetainDetails != null)
                {
                    fuelRetainDetails.TrailerFuelRetains.Add(new TrailerFuelRetainViewModel { TrailerId = tFuelRetainDetails.TrailerId.ToString(), CompartmentId = tFuelRetainDetails.CompartmentId == defaultCompartmentId ? string.Empty : tFuelRetainDetails.CompartmentId, Quantity = tFuelRetainDetails.Quantity, ProductType = tFuelRetainDetails.ProductType, UOM = tFuelRetainDetails.UOM, Id = tFuelRetainDetails.Id.ToString() });
                }
            }
        }
        public async Task<List<TrailerRetainDetails>> GetTrailersRetainDetails(List<ObjectId> TrailerIds)
        {
            var response = new List<TrailerRetainDetails>();
            try
            {
                var trailerInfo = await mdbContext.TruckDetails.Find(x => TrailerIds.Contains(x.Id) && !x.IsDeleted).Project(x => new { x.Id, x.TruckId }).ToListAsync();
                var trailerFuelRetainDetails = await mdbContext.TrailerFuelRetains.Find(x => TrailerIds.Contains(x.TrailerId) && !x.IsDeleted && x.IsActive).Project(t => new { t.TrailerId, t.TfxDriverId, t.ProductId }).ToListAsync();
                foreach (var tFuelRetainDetails in trailerFuelRetainDetails)
                {
                    var index = response.FindIndex(t => t.TrailerId == tFuelRetainDetails.TrailerId.ToString() && t.ProductId == tFuelRetainDetails.ProductId);
                    if (index == -1)
                    {
                        TrailerRetainDetails trailerRetainDetails = new TrailerRetainDetails();
                        trailerRetainDetails.DriverId = tFuelRetainDetails.TfxDriverId;
                        var truck = trailerInfo.FirstOrDefault(x => x.Id == tFuelRetainDetails.TrailerId);
                        if (truck != null)
                        {
                            trailerRetainDetails.Name = truck.TruckId;
                        }
                        trailerRetainDetails.TrailerId = tFuelRetainDetails.TrailerId.ToString();
                        trailerRetainDetails.ProductId = tFuelRetainDetails.ProductId;
                        trailerRetainDetails.RetainFuelCount = 1;
                        response.Add(trailerRetainDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("ScheduleBuilderRepository", "GetTrailerFuelRetainDetails", ex.Message, ex);
                throw;
            }
            return response;
        }

        public async Task<List<TruckDetailViewModel>> GetTruckDetails(List<ObjectId> truckIds)
        {
            var response = new List<TruckDetailViewModel>();
            try
            {
                var trucks = await mdbContext.TruckDetails.Find(x => truckIds.Contains(x.Id)).ToListAsync();
                if (trucks != null)
                {
                    var allTrucks = trucks.OrderByDescending(t => t.CreatedDate).ThenByDescending(t => t.UpdatedDate).ToList();
                    allTrucks.ForEach(x => response.Add(x.ToViewModel()));
                    ScheduleBuilderRepository scheduleBuilderRepository = new ScheduleBuilderRepository(mdbContext);
                    List<string> trailerIds = new List<string>();
                    truckIds.ForEach(t => trailerIds.Add(t.ToString()));
                    var FuelRetainDetails = await scheduleBuilderRepository.GetTrailerFuelRetainDetails(trailerIds);
                    if (FuelRetainDetails.Any())
                    {
                        response.ForEach(t =>
                        {
                            var retainDetails = FuelRetainDetails.Where(fuel => fuel.TrailerId == t.TruckId).ToList();
                            t.TrailerFuelRetains = retainDetails;
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("TruckRepository", "GetTruckDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusModel> DeleteTruckAsync(TruckDetailViewModel requestModel)
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
                        await mdbContext.TruckDetails.FindOneAndUpdateAsync(Builders<TruckDetail>.Filter.Eq(n => n.Id, bsonId),
                          Builders<TruckDetail>.Update.Set(m => m.IsDeleted, true).Set(m => m.Status, TruckStatus.InActive)
                                                        .Set(m => m.TfxCreatedBy, requestModel.TfxCreatedBy)
                                                        .Set(m => m.CreatedDate, requestModel.CreatedDate)
                                                        .Set(m => m.TfxCompanyId, requestModel.TfxCompanyId));

                        // Made it here without error? Let's commit the transaction
                        await session.CommitTransactionAsync();

                        response.StatusCode = (int)Status.Success;
                        response.StatusMessage = $"{requestModel.TruckId} trailer deleted Successfully";
                    }
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();

                    response.StatusCode = (int)Status.Failed;
                    Exchange.Logger.LogManager.Logger.WriteException("TruckRepository", "DeleteTruckAsync", ex.Message, ex);
                }
            }
            return response;
        }
        public List<ExternalVehicleMappingViewModel> GetVehiclesForExternalMapping(int companyId)
        {
            var response = new List<ExternalVehicleMappingViewModel>();
            try
            {

                var vehiclesMappings = (from truck in mdbContext.TruckDetails.AsQueryable()
                                        join vehicle in mdbContext.ExternalVehicleMappings.AsQueryable()
                                         on truck.TruckId equals vehicle.TruckId into grp
                                        from vehicleMapping in grp.DefaultIfEmpty()
                                        select new ExternalVehicleMappingViewModel
                                        {
                                            TruckName = truck.Name,
                                            TargetVehicleValue = vehicleMapping == null ? null : vehicleMapping.TargetVehicleValue,
                                            TruckId = truck.TruckId,
                                            ThirdPartyId = vehicleMapping == null ? 0 : vehicleMapping.ThirdPartyId
                                        }).OrderBy(t => t.TruckName).ToList();

                if (vehiclesMappings != null && vehiclesMappings.Any())
                {
                    vehiclesMappings = vehiclesMappings.Where(t => !string.IsNullOrEmpty(t.TruckName)).ToList();
                    response.AddRange(vehiclesMappings);
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("TruckRepository", "GetVehiclesForExternalMapping", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusModel> SaveExternalVehicleMapping(ExternalVehicleMappingViewModel viewModel)
        {
            StatusModel response = new StatusModel();
            try
            {
                var entity = viewModel.ToEntity();
                var prevField = await mdbContext.ExternalVehicleMappings.Find(t => t.TruckId == viewModel.TruckId && t.IsActive).FirstOrDefaultAsync();

                if (prevField != null)
                {
                    entity.Id = prevField.Id;
                    entity.UpdatedBy = viewModel.UserId;
                    entity.UpdatedDate = DateTimeOffset.UtcNow;
                    await mdbContext.ExternalVehicleMappings.ReplaceOneAsync(Builders<ExternalVehicleMappings>.Filter.Eq(t => t.Id, prevField.Id), entity);
                }
                else
                {
                    entity.CreatedBy = viewModel.UserId;
                    entity.CreatedDate = DateTimeOffset.UtcNow;
                    await mdbContext.ExternalVehicleMappings.InsertOneAsync(entity);
                }
                response.StatusCode = (int)Status.Success;
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("TruckRepository", "SaveExternalVehicleMapping", ex.Message, ex);
            }
            return response;
        }
        public void GetVehicleIds(List<ExternalVehicleMappingViewModel> listExternalVehicles)
        {
            var externalVehicleNames = listExternalVehicles.Select(t => t.TruckName).Distinct().ToList();
            var trucks = mdbContext.TruckDetails.Find(x => externalVehicleNames.Contains(x.Name)).ToList();


            foreach (var item in listExternalVehicles)
            {
                item.TruckId = trucks.Where(t => t.Name.ToLower().Trim() == item.TruckName.ToLower().Trim()).Select(t => t.TruckId).FirstOrDefault();
            }
        }

        public StatusModel ValidateVehicleMappingFileAsync(int userId, List<ExternalVehicleMappingViewModel> csvVehicleList)
        {

            StatusModel response = new StatusModel();
            try
            {
                StringBuilder errorList = new StringBuilder();
                if (csvVehicleList != null && csvVehicleList.Any())
                {
                    ValidateVehicleInvalidRecord(csvVehicleList, errorList);
                    if (errorList.Length > 0)
                    {
                        response.StatusCode = (int)Status.Failed;
                        response.StatusMessage = errorList.ToString();
                        if (response.StatusMessage.Length > 1000)
                        {
                            response.StatusMessage = response.StatusMessage.Substring(0, 999) + ".... Too many errors in file";
                        }
                    }
                    else
                    {
                        response.StatusCode = (int)Status.Success;
                    }
                }
                else
                {
                    response.StatusCode = (int)Status.Failed;
                    response.StatusMessage = "No Records found in csv.";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "ValidateVehicleMappingFileAsync", ex.Message, ex);
            }
            return response;
        }

        public void ValidateVehicleInvalidRecord(List<ExternalVehicleMappingViewModel> csvVehicleList, StringBuilder errorList)
        {
            var vehicles = csvVehicleList.Select(t => t.TruckName).Distinct().ToList();
            var validVehicles = mdbContext.TruckDetails.Find(t => vehicles.Contains(t.Name)).ToList();
            int lineNumberOfCSV = 1;

            foreach (var item in csvVehicleList)
            {
                if (!validVehicles.Any(t => t.Name.ToLower().Trim() == item.TruckName.ToLower().Trim()))
                {
                    if (errorList.Length > 0)

                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgInvalidVehicleForMapping, item.TruckName, lineNumberOfCSV));
                }
                lineNumberOfCSV++;
            }

        }

        public async Task<StatusModel> SaveBulkUploadVehicleMapping(int userId, List<ExternalVehicleMappingViewModel> listExternalVehicles)
        {
            StatusModel response = new StatusModel();
            try
            {
                response = ValidateVehicleMappingFileAsync(userId, listExternalVehicles);
                if (response.StatusCode == (int)Status.Success)
                {
                    GetVehicleIds(listExternalVehicles);

                    var vechicleIds = listExternalVehicles.Select(t => t.TruckId).ToList();
                    var listExternalVehiclesForUpdate = mdbContext.ExternalVehicleMappings.Find(t => vechicleIds.Contains(t.TruckId) && t.IsActive).ToList();

                    var vehicleIdsToUpdate = new List<string>();
                    if (listExternalVehiclesForUpdate != null && listExternalVehiclesForUpdate.Any())
                    {
                        vehicleIdsToUpdate = listExternalVehiclesForUpdate.Select(t => t.TruckId).ToList();
                        foreach (var item in listExternalVehiclesForUpdate)
                        {
                            var viewModel = listExternalVehicles.Where(t => t.TruckId == item.TruckId).FirstOrDefault();
                            var entity = viewModel.ToEntity();
                            var prevField = await mdbContext.ExternalVehicleMappings.Find(t => t.TruckId == viewModel.TruckId && t.IsActive).FirstOrDefaultAsync();
                            if (prevField != null)
                            {
                                entity.Id = prevField.Id;
                                entity.UpdatedBy = viewModel.UserId;
                                entity.UpdatedDate = DateTimeOffset.UtcNow;
                                await mdbContext.ExternalVehicleMappings.ReplaceOneAsync(Builders<ExternalVehicleMappings>.Filter.Eq(t => t.Id, prevField.Id), entity);
                            }
                            await mdbContext.ExternalVehicleMappings.ReplaceOneAsync(Builders<ExternalVehicleMappings>.Filter.Eq(t => t.Id, item.Id), entity);
                            response.StatusCode = (int)Status.Success;

                        }
                    }
                    var listExternalVehicleMappings = new List<ExternalVehicleMappings>();
                    var listExternalVehicleForInsert = listExternalVehicles.Where(t => !vehicleIdsToUpdate.Contains(t.TruckId)).ToList();
                    if (listExternalVehicleForInsert != null && listExternalVehicleForInsert.Any())
                    {
                        foreach (var item in listExternalVehicleForInsert)
                        {
                            ExternalVehicleMappings externalvehicleMappings = new ExternalVehicleMappings();
                            item.ThirdPartyId = (int)ExternalThirdPartyCompanies.PDI;
                            externalvehicleMappings = item.ToEntity();
                            externalvehicleMappings.CreatedBy = userId;
                            externalvehicleMappings.CreatedDate = DateTimeOffset.UtcNow;
                            listExternalVehicleMappings.Add(externalvehicleMappings);

                        }
                        await mdbContext.ExternalVehicleMappings.InsertManyAsync(listExternalVehicleMappings);
                        response.StatusCode = (int)Status.Success;

                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveBulkUploadVehicleMapping", ex.Message, ex);
            }
            return response;
        }

    }
}

