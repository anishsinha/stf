using MongoDB.Driver;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.MdbDataAccess.DbContext;
using SiteFuel.FreightRepository.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using SiteFuel.Exchange.Core.StringResources;

namespace SiteFuel.FreightRepository
{
    public class CarrierRepository : ICarrierRepository
    {
        private readonly MdbContext mdbContext;
        public CarrierRepository(MdbContext _context)
        {
            mdbContext = _context;
        }
        public List<CarrierViewModel> GetSupplierCarriers(int companyId, int carrierCompanyId)
        {
            var response = new List<CarrierViewModel>();

            var carrierList = (from carrier in mdbContext.Carriers.AsQueryable()
                               join job in mdbContext.CarrierJobs.AsQueryable()
                                on carrier.TfxCarrierCompanyId equals job.TfxCarrierCompanyId into grp
                               where ((carrierCompanyId == 0 && (carrier.TfxSupplierCompanyId == companyId || carrier.TfxCarrierCompanyId == companyId)) || 
                                       carrier.TfxCarrierCompanyId == carrierCompanyId)
                                     && carrier.IsActive 
                                     && !carrier.IsDeleted
                               select new
                               {
                                   Carrier = new
                                   {
                                       carrier.Id,
                                       carrier.TfxCarrierCompanyId,
                                       carrier.TfxCarrierCompanyName,
                                       carrier.TfxSupplierCompanyId,
                                       carrier.TfxSupplierCompanyName
                                   },
                                   Jobs = grp.Where(t => t.IsActive).Select(t => new
                                   {
                                       t.Id,
                                       t.TfxJobId,
                                       t.TfxJobCompanyId,
                                       t.TfxBuyerCompanyName,
                                       t.TfxJobName,
                                       t.TfxSupplierCompanyId
                                   })
                               }).ToList();

            foreach (var item in carrierList)
            {
                var model = new CarrierViewModel
                {
                    Id = item.Carrier.Id.ToString(),
                    Carrier = new FreightModels.DropdownDisplayItem() { Id = item.Carrier.TfxCarrierCompanyId, Name = item.Carrier.TfxCarrierCompanyName },
                    SupplierCompanyId = item.Carrier.TfxSupplierCompanyId,
                    SupplierCompanyName = item.Carrier.TfxSupplierCompanyName,
                    Jobs = item.Jobs.Where(top=>top.TfxSupplierCompanyId==companyId).Select(t => new JobViewModel { Job = new FreightModels.DropdownDisplayItem { Id = t.TfxJobId, Code = t.Id.ToString(), Name = t.TfxJobName }, BuyerCompanyId = t.TfxJobCompanyId, BuyerCompanyName = t.TfxBuyerCompanyName }).ToList()
                };
                response.Add(model);
            }
            return response;
        }

        public CarrierJobDetailsViewModel GetCarrierUsers(int companyId)
        {
            var response = new CarrierJobDetailsViewModel();
           
            var carrierList = (from carrier in mdbContext.Carriers.AsQueryable()
                               join job in mdbContext.CarrierJobs.AsQueryable()
                                on carrier.TfxCarrierCompanyId equals job.TfxCarrierCompanyId into grp
                               where (carrier.TfxSupplierCompanyId == companyId || carrier.TfxCarrierCompanyId == companyId) && carrier.IsActive && !carrier.IsDeleted
                               select new
                               {
                                   CarrierCompanyId = carrier.TfxCarrierCompanyId,
                                   CarrierCompanyName = carrier.TfxCarrierCompanyName,
                                   LocationCount = grp.Where(t1 => t1.IsActive).Count(),
                                   Locations = grp.Where(t1 => t1.IsActive).Take(4).Select(t => t.TfxJobName)
                               }).ToList();
            
            foreach (var item in carrierList)
            {
                var cuser = new CarrierJobDetailsModel() {
                    CarrierCompanyId = item.CarrierCompanyId,
                    CarrierCompanyName = item.CarrierCompanyName,
                    LocationCount = item.LocationCount,
                    AssignedLocations = item.LocationCount > 0 ? string.Join(",",item.Locations) + "..." : Resource.lblHyphen
                };
                response.Carriers.Add(cuser);
            }
            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<StatusModel> AssignToSupplier(List<CarrierViewModel> supplierCarriers)
        {
            var response = new StatusModel();
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    foreach (var carrier in supplierCarriers)
                    {
                        Carrier entity = carrier.ToEntity();
                        bool carrierExists = mdbContext.Carriers.Find(t => t.TfxCarrierCompanyId == carrier.Carrier.Id && t.TfxSupplierCompanyId == carrier.SupplierCompanyId && t.IsActive && !t.IsDeleted).Any();
                        if (!carrierExists)
                        {
                            await mdbContext.Carriers.InsertOneAsync(entity);
                        }
                        if (carrier.Jobs != null)
                        {
                            foreach (var job in carrier.Jobs)
                            {
                                bool carrierJobExists = mdbContext.CarrierJobs.Find(t => t.TfxCarrierCompanyId == carrier.Carrier.Id && t.TfxSupplierCompanyId == carrier.SupplierCompanyId && t.TfxJobId == job.Job.Id && t.IsActive).Any();
                                if (!carrierJobExists)
                                {
                                    var carrierJob = new CarrierJob() { TfxCarrierCompanyId = carrier.Carrier.Id, TfxJobId = job.Job.Id, TfxJobName = job.Job.Name, TfxJobCompanyId = job.BuyerCompanyId, TfxBuyerCompanyName = job.BuyerCompanyName, TfxSupplierCompanyId = carrier.SupplierCompanyId, IsActive = true };
                                    await mdbContext.CarrierJobs.InsertOneAsync(carrierJob);
                                }                                
                            }
                        }
                    }
                    await session.CommitTransactionAsync();
                    response.StatusCode = (int)Status.Success;
                }
                catch (Exception)
                {
                    await session.AbortTransactionAsync();
                    response.StatusCode = (int)Status.Failed;
                    throw;
                }
            }
            return response;
        }

        public async Task<StatusModel> UpdateAssignedCarriers(CarrierViewModel carrier)
        {
            var response = new StatusModel();

            var updateFields = Builders<CarrierJob>.Update
                .Set(t => t.IsActive, false);

            var existingJobs = mdbContext.CarrierJobs.Find(t => t.TfxCarrierCompanyId == carrier.Carrier.Id && t.TfxSupplierCompanyId==carrier.SupplierCompanyId && t.IsActive).ToList();
            var deletedJobs = existingJobs.Where(t => carrier.Jobs == null || !carrier.Jobs.Any(t1 => t1.Job.Id == t.TfxJobId)).Select(t => t.Id).ToList();
            var newJobs = new List<JobViewModel>();
            if (carrier.Jobs != null)
            {
                newJobs = carrier.Jobs.Where(t => !existingJobs.Any(t1 => t1.TfxJobId == t.Job.Id)).ToList();
            }
            var filter = Builders<CarrierJob>.Filter.And(
                    Builders<CarrierJob>.Filter.Where(x => x.TfxCarrierCompanyId == carrier.Carrier.Id),
                    Builders<CarrierJob>.Filter.Where(x => deletedJobs.Contains(x.Id)),
                    Builders<CarrierJob>.Filter.Where(x => x.IsActive)
                );

            await mdbContext.CarrierJobs.UpdateManyAsync(filter, updateFields);

            foreach (var job in newJobs)
            {
                var carrierJob = new CarrierJob() { TfxCarrierCompanyId = carrier.Carrier.Id, TfxJobId = job.Job.Id, TfxJobName = job.Job.Name, TfxJobCompanyId = job.BuyerCompanyId, TfxBuyerCompanyName = job.BuyerCompanyName, TfxSupplierCompanyId = carrier.SupplierCompanyId, IsActive = true };
                await mdbContext.CarrierJobs.InsertOneAsync(carrierJob);
            }
            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<StatusModel> DeleteAssignedCarriers(CarrierViewModel carrier)
        {
            var response = new StatusModel();

            var updateFields = Builders<Carrier>.Update
                .Set(t => t.IsActive, false).Set(t => t.IsDeleted, true);

            var filter = Builders<Carrier>.Filter.And(
                 Builders<Carrier>.Filter.Where(x => x.Id == ObjectId.Parse(carrier.Id))
                    );

            await mdbContext.Carriers.UpdateOneAsync(filter, updateFields);

            var updateJobFields = Builders<CarrierJob>.Update
               .Set(t => t.IsActive, false);

            if (carrier.Jobs != null && carrier.Jobs.Any())
            {
                var jobIds = carrier.Jobs.Select(t => t.Job.Code).ToList();
                var jobObjectIds = jobIds.Select(t => ObjectId.Parse(t)).ToList();
                var jobFilter = Builders<CarrierJob>.Filter.And(
                     Builders<CarrierJob>.Filter.Where(x => jobObjectIds.Contains(x.Id)));

                await mdbContext.CarrierJobs.UpdateManyAsync(jobFilter, updateJobFields);
            }

            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<StatusModel> AssignCarrierToJob(CarrierViewModel carrier)
        {
            var response = new StatusModel();
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    var carrierEntity = carrier.ToEntity();
                    if (carrier.Jobs != null)
                    {
                        foreach (var job in carrier.Jobs)
                        {
                            var carrierJobExists = await mdbContext.CarrierJobs.Find(t => t.TfxJobId == job.Job.Id && t.TfxSupplierCompanyId == carrier.SupplierCompanyId && t.TfxCarrierCompanyId != carrier.Carrier.Id && t.IsActive).FirstOrDefaultAsync();
                            if (carrierJobExists == null)
                            {
                                var carrierJob = new CarrierJob() { TfxCarrierCompanyId = carrier.Carrier.Id, TfxJobId = job.Job.Id, TfxJobName = job.Job.Name, TfxJobCompanyId = job.BuyerCompanyId, TfxBuyerCompanyName = job.BuyerCompanyName, TfxSupplierCompanyId = carrier.SupplierCompanyId, IsActive = true };
                                await mdbContext.CarrierJobs.InsertOneAsync(carrierJob);
                            }
                            else
                            {
                                await mdbContext.CarrierJobs.UpdateOneAsync(Builders<CarrierJob>.Filter.Eq(t => t.Id, carrierJobExists.Id), Builders<CarrierJob>.Update.Set(t => t.TfxCarrierCompanyId, carrierEntity.TfxCarrierCompanyId));
                            }
                        }
                    }

                    await session.CommitTransactionAsync();
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = response.StatusCode == (int)Status.Success ? "Success" : "Failed";
                }
                catch (Exception)
                {
                    await session.AbortTransactionAsync();
                    response.StatusCode = (int)Status.Failed;
                }
            }
            return response;
        }
        public async Task<List<int>> GetCarriersJobs(int carrierCompanyId, int customerCompanyId)
        {
            List<int> response = await mdbContext.CarrierJobs.Find(t => t.IsActive && t.TfxCarrierCompanyId == carrierCompanyId && (customerCompanyId == 0 || t.TfxJobCompanyId == customerCompanyId)).Project(t => t.TfxJobId).ToListAsync();
            return response;
        }
        public async Task<List<DipTestRequestModel>> GetCarrierDetailsByJob(List<int> jobIds)
        {
            List<DipTestRequestModel> response = new List<DipTestRequestModel>();
            var carriers = await mdbContext.CarrierJobs.Find(t =>  t.IsActive && jobIds.Contains(t.TfxJobId)).Project(t => new DipTestRequestModel() { CompanyId = t.TfxCarrierCompanyId, CompanyTypeId = CompanyType.Carrier }).ToListAsync();
            if(carriers != null && carriers.Any())
            {
                response = carriers.Distinct().ToList();
            }         
            return response;
        }
    }
}
