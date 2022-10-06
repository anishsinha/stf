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
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SiteFuel.Exchange.Domain
{
    public class RegionDomain : BaseDomain
    {
        public RegionDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public RegionDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetJobsForCarrierAsync(UserContext userContext)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            var assignedCarrierJobsForSupplier = new List<DropdownDisplayExtendedItem>();
            var supplierOrderJobs = new List<DropdownDisplayExtendedItem>();

            try
            {
                var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
                var carriers = await fsDomain.GetAssignedCarriersForSupplier(userContext.CompanyId);

                if (carriers != null)
                {
                    foreach (var jobList in carriers)
                    {
                        if (jobList != null)
                        {
                            var jobs = jobList.Jobs.Where(t => t.Job != null)
                                                    .Select(t => new { JobId = t.Job.Id, JobName = t.Job.Name })
                                                    .Select(t1 => new
                                                    {
                                                        Job = new { t1.JobId, t1.JobName }
                                                    }).ToList();
                            
                                var jobIds = new List<int>();
                                jobs.ForEach(t => jobIds.Add(t.Job.JobId));
                            //  var marineJobs = Context.DataContext.Jobs.Where(t => t.IsMarine && jobIds.Contains(t.Id)).Select(s=> new{Id=s.Id,customerName=s.Company.Name }).ToList();
                            //+(marineJobs.Where(w => w.Id == t.Job.JobId) != null ? " (" + marineJobs.Where(w => w.Id == t.Job.JobId).FirstOrDefault()?.customerName + " )" : "")})
                            jobs.ForEach(t => assignedCarrierJobsForSupplier.Add(new DropdownDisplayExtendedItem() { Id = t.Job.JobId, Name = t.Job.JobName } ));
                        }
                    }
                }

                var allJobsForCurrentCompany = await Context.DataContext.Orders.Include(t => t.FuelRequest.Job)
                                                                .Where(t => t.IsActive && t.ParentId == null && t.AcceptedCompanyId == userContext.CompanyId)
                                                                .Select(t => new { JobId = t.FuelRequest.Job.Id, JobName = t.FuelRequest.Job.Name + " ("+t.BuyerCompany.Name+")"  })
                                                                .ToListAsync();
                if (allJobsForCurrentCompany != null && allJobsForCurrentCompany.Any())
                {
                    allJobsForCurrentCompany.ForEach(t => supplierOrderJobs.Add(new DropdownDisplayExtendedItem() { Id = t.JobId, Name = t.JobName }));
                }

                // merge both list
                var mergedJobs = assignedCarrierJobsForSupplier.Union(supplierOrderJobs).ToList();
                var result = mergedJobs.GroupBy(t => new { t.Id, t.Name }).Select(t => new DropdownDisplayExtendedItem() { Id = t.Key.Id, Name = t.Key.Name }).Distinct().ToList();
                response  =  await GetJobsAssignedToRegions(result, userContext.CompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetJobsForCarrierAsync", ex.Message, ex);
            }
            return response;
        }

        private async Task<List<DropdownDisplayExtendedItem>> GetJobsAssignedToRegions(List<DropdownDisplayExtendedItem> jobs, int companyId)
        {
            List<int> regionJobIds = new List<int>();
            try
            {
                regionJobIds =  await new FreightServiceDomain(this).GetJobsAssignedToRegions(jobs, companyId);
                if (regionJobIds.Any() && jobs.Any())
                {
                    foreach(int Id in regionJobIds)
                    {
                        var jobToRemove = jobs.Where(t => t.Id == Id).ToList();
                        if (jobToRemove != null && jobToRemove.Any())
                        {
                            foreach (var item in jobToRemove)
                            {
                                jobs.Remove(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightServiceDomain", "GetJobsAssignedToRegions", ex.Message, ex);
            }
            return jobs;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetDriversForCarrierAsync(UserContext userContext)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                var eligibleRoles = new List<int> { (int)UserRoles.Driver };
                var drivers = await Context.DataContext.Users
                                        .Where(t => t.Company.Id == userContext.CompanyId && t.Company.IsActive &&
                                                    t.MstRoles.Any(t1 => eligibleRoles.Contains(t1.Id)) && !t.IsDeleted &&
                                                    ((t.IsActive && t.IsOnboardingComplete) || (!t.IsActive && !t.IsOnboardingComplete)))
                                        .Select(t => new { t.Id, t.FirstName, t.LastName,t.IsEmailConfirmed,t.IsOnboardingComplete })
                                        .ToListAsync();
                //drivers.ForEach(t => response.Add(new DropdownDisplayExtendedItem { Id = t.Id, Name = $"{t.FirstName} {t.LastName}" }));
                foreach (var driver in drivers)
                {
                    if (driver.IsOnboardingComplete)
                    {
                        response.Add(new DropdownDisplayExtendedItem { Id = driver.Id, Name = $"{driver.FirstName} {driver.LastName}" });
                    }
                    else
                    {
                        if (driver.IsEmailConfirmed)
                        {
                            response.Add(new DropdownDisplayExtendedItem { Id = driver.Id, Name = $"{driver.FirstName} {driver.LastName} - {Resource.lblEmailVerified}" });
                        }
                        else
                        {
                            response.Add(new DropdownDisplayExtendedItem { Id = driver.Id, Name = $"{driver.FirstName} {driver.LastName} - {Resource.headingInvited}" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetDriversForCarrierAsync", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DropdownDisplayExtendedItem>> GetDriversForRegionCarrierAsync(UserContext userContext, List<int> existingdriverdetails)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                var eligibleRoles = new List<int> { (int)UserRoles.Driver };
                var drivers = await Context.DataContext.Users
                                        .Where(t => t.Company.Id == userContext.CompanyId && t.Company.IsActive &&
                                                    t.MstRoles.Any(t1 => eligibleRoles.Contains(t1.Id)) && !t.IsDeleted && 
                                                    ((t.IsActive && t.IsOnboardingComplete) || (!t.IsActive && !t.IsOnboardingComplete)))
                                        .Select(t => new { t.Id, t.FirstName, t.LastName, t.IsOnboardingComplete,t.IsEmailConfirmed})
                                        .ToListAsync();
                if (existingdriverdetails.Any())
                {
                    foreach (var item in drivers)
                    {
                        bool recordExists = existingdriverdetails.Any(top => top == item.Id);
                        if (!recordExists)
                        {
                            //response.Add(new DropdownDisplayExtendedItem { Id = item.Id, Name = $"{item.FirstName} {item.LastName}" });
                            if (item.IsOnboardingComplete)
                            {
                                response.Add(new DropdownDisplayExtendedItem { Id = item.Id, Name = $"{item.FirstName} {item.LastName}" });
                            }
                            else
                            {
                                if (item.IsEmailConfirmed)
                                {
                                    response.Add(new DropdownDisplayExtendedItem { Id = item.Id, Name = $"{item.FirstName} {item.LastName} {Resource.lblDriverEmailVerfied}" });
                                }
                                else
                                {
                                    response.Add(new DropdownDisplayExtendedItem { Id = item.Id, Name = $"{item.FirstName} {item.LastName} {Resource.lblDriverInvited}" });
                                }

                            }
                        }
                    }
                }
                else
                {
                    //drivers.ForEach(t => response.Add(new DropdownDisplayExtendedItem { Id = t.Id, Name = $"{t.FirstName} {t.LastName}" }));
                    foreach (var driver in drivers)
                    {
                        if (driver.IsOnboardingComplete)
                        {
                            response.Add(new DropdownDisplayExtendedItem { Id = driver.Id, Name = $"{driver.FirstName} {driver.LastName}" });
                        }
                        else
                        {
                            if (driver.IsEmailConfirmed)
                            {
                                response.Add(new DropdownDisplayExtendedItem { Id = driver.Id, Name = $"{driver.FirstName} {driver.LastName} {Resource.lblDriverEmailVerfied}" });
                            }
                            else
                            {
                                response.Add(new DropdownDisplayExtendedItem { Id = driver.Id, Name = $"{driver.FirstName} {driver.LastName} {Resource.lblDriverInvited}" });
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetDriversForCarrierAsync", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DropdownDisplayExtendedItem>> GetDispatchersForCarrierAsync(UserContext userContext)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                var eligibleRoles = new List<int> { (int)UserRoles.Dispatcher, (int)UserRoles.Admin };
                var dispatchers = await Context.DataContext.Users
                                        .Where(t => t.Company.Id == userContext.CompanyId && t.Company.IsActive &&
                                                    t.MstRoles.Any(t1 => eligibleRoles.Contains(t1.Id)) &&
                                                    ((t.IsActive && t.IsOnboardingComplete) || (!t.IsActive && !t.IsOnboardingComplete)))
                                        .Select(t => new { t.Id, t.FirstName, t.LastName })
                                        .ToListAsync();

                dispatchers.ForEach(t => response.Add(new DropdownDisplayExtendedItem { Id = t.Id, Name = $"{t.FirstName} {t.LastName}" }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetDispatchersForCarrierAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetTruckAndTrailersForCarrierAsync(UserContext userContext)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
                var trucks = await fsDomain.GetAllTruckDetailsAsync(userContext.CompanyId);

                if (trucks != null && trucks.Any())
                {
                    trucks.ForEach(t => response.Add(new DropdownDisplayExtendedItem() { Code = t.Id, Name = t.TruckId }));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetTruckAndTrailersForCarrierAsync", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DropdownDisplayExtendedItem>> GetCompanyShifts(int companyId)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
                response = Task.Run(() => fsDomain.GetCompanyShiftDdl(companyId)).Result;

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetCompanyShifts", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CreateSourceRegion(UserContext userContext, SourceRegionViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var sourceRegion = new SourceRegion();
                    sourceRegion.Name = viewModel.Name;
                    sourceRegion.CreatedBy = userContext.Id;
                    sourceRegion.CreatedDate = DateTime.Now;
                    sourceRegion.Description = viewModel.Description;
                    sourceRegion.CompanyId = userContext.CompanyId;
                    sourceRegion.UpdatedBy = userContext.Id;
                    sourceRegion.UpdatedDate = DateTime.Now;
                    sourceRegion.IsActive = true;
                    Context.DataContext.SourceRegions.Add(sourceRegion);
                    await Context.CommitAsync();

                    //--------Source Region Carriers----------------------
                    if (viewModel.Carriers != null)
                    {
                        foreach (var item in viewModel.Carriers)
                        {
                            var sourceRegionCarrier = new SourceRegionCarrier();
                            sourceRegionCarrier.SourceRegionId = sourceRegion.Id;
                            sourceRegionCarrier.CarrierId = item.Id;
                            Context.DataContext.SourceRegionCarriers.Add(sourceRegionCarrier);
                        }
                    }

                    await Context.CommitAsync();
                    //--------Source Region Carriers----------------------


                    //--------Source Region Address----------------------
                    var sourceRegionAddress = new SourceRegionAddress();
                    sourceRegionAddress.SourceRegionId = sourceRegion.Id;

                    var states = viewModel.States.Select(t => new DropdownDisplayItem
                    {
                        Id = t.Id,
                        Name = t.Name
                    }).ToList();

                    var stateJsonMessage = new JavaScriptSerializer().Serialize(states);
                    sourceRegionAddress.States = stateJsonMessage;

                    if (viewModel.Cities != null && viewModel.Cities.Count > 0)
                    {
                        var cities = viewModel.Cities.Select(t => new DropdownDisplayExtendedItem
                        {
                            Code = t.Name,
                            Name = t.Name
                        }).ToList();

                        var citiesJsonMessage = new JavaScriptSerializer().Serialize(cities);
                        sourceRegionAddress.Cities = citiesJsonMessage;
                    }

                    Context.DataContext.SourceRegionAddresses.Add(sourceRegionAddress);
                    await Context.CommitAsync();
                    //--------Source Region Address----------------------


                    //--------Terminals----------------------
                    if (viewModel.Terminals != null)
                    {
                        foreach (var item in viewModel.Terminals)
                        {
                            var sourceRegionPickupLocation = new SourceRegionPickupLocation();
                            sourceRegionPickupLocation.SourceRegionId = sourceRegion.Id;
                            sourceRegionPickupLocation.TerminalId = item.Id;
                            Context.DataContext.SourceRegionPickupLocations.Add(sourceRegionPickupLocation);
                        }
                    }

                    await Context.CommitAsync();
                    //--------Terminals----------------------


                    //--------BulkPlants----------------------
                    if (viewModel.BulkPlants != null)
                    {
                        foreach (var item in viewModel.BulkPlants)
                        {
                            var sourceRegionPickupLocation = new SourceRegionPickupLocation();
                            sourceRegionPickupLocation.SourceRegionId = sourceRegion.Id;
                            sourceRegionPickupLocation.BulkPlantId = item.Id;
                            Context.DataContext.SourceRegionPickupLocations.Add(sourceRegionPickupLocation);
                        }

                        await Context.CommitAsync();
                    }
                    //--------BulkPlants----------------------


                    transaction.Commit();

                    response.StatusCode = Status.Success;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("RegionDomain", "CreateSourceRegion", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateSourceRegion(UserContext userContext, SourceRegionViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var sourceRegion = await Context.DataContext.SourceRegions.FirstOrDefaultAsync(t => t.Id == viewModel.Id);

                    sourceRegion.Name = viewModel.Name;
                    sourceRegion.Description = viewModel.Description;
                    sourceRegion.UpdatedBy = userContext.Id;
                    sourceRegion.UpdatedDate = DateTime.Now;

                    Context.DataContext.Entry(sourceRegion).State = EntityState.Modified;
                    await Context.CommitAsync();

                    //--------Source Region Carriers----------------------
                    Context.DataContext.SourceRegionCarriers.RemoveRange(sourceRegion.SourceRegionCarrier);
                    if (viewModel.Carriers != null)
                    {
                        foreach (var item in viewModel.Carriers)
                        {
                            var sourceRegionCarrier = new SourceRegionCarrier();
                            sourceRegionCarrier.SourceRegionId = sourceRegion.Id;
                            sourceRegionCarrier.CarrierId = item.Id;
                            Context.DataContext.SourceRegionCarriers.Add(sourceRegionCarrier);
                        }
                    }

                    await Context.CommitAsync();
                    //--------Source Region Carriers----------------------


                    //--------Source Region Address----------------------
                    var sourceRegionAddress = sourceRegion.SourceRegionAddress;

                    var states = viewModel.States.Select(t => new DropdownDisplayItem
                    {
                        Id = t.Id,
                        Name = t.Name
                    }).ToList();

                    var stateJsonMessage = new JavaScriptSerializer().Serialize(states);
                    sourceRegionAddress.States = stateJsonMessage;

                    if (viewModel.Cities != null && viewModel.Cities.Count > 0)
                    {
                        var cities = viewModel.Cities.Select(t => new DropdownDisplayExtendedItem
                        {
                            Code = t.Name,
                            Name = t.Name
                        }).ToList();

                        var citiesJsonMessage = new JavaScriptSerializer().Serialize(cities);
                        sourceRegionAddress.Cities = citiesJsonMessage;
                    }
                    else
                    {
                        sourceRegionAddress.Cities = string.Empty;
                    }

                    Context.DataContext.Entry(sourceRegionAddress).State = EntityState.Modified;
                    await Context.CommitAsync();
                    //--------Source Region Address----------------------


                    Context.DataContext.SourceRegionPickupLocations.RemoveRange(sourceRegion.SourceRegionPickupLocation);
                    if (viewModel.Terminals != null)
                    {
                        //--------Terminals----------------------
                        foreach (var item in viewModel.Terminals)
                        {
                            var sourceRegionPickupLocation = new SourceRegionPickupLocation();
                            sourceRegionPickupLocation.SourceRegionId = sourceRegion.Id;
                            sourceRegionPickupLocation.TerminalId = item.Id;
                            Context.DataContext.SourceRegionPickupLocations.Add(sourceRegionPickupLocation);
                        }
                        //--------Terminals----------------------
                    }
                    await Context.CommitAsync();


                    //--------BulkPlants----------------------
                    if (viewModel.BulkPlants != null)
                    {
                        foreach (var item in viewModel.BulkPlants)
                        {
                            var sourceRegionPickupLocation = new SourceRegionPickupLocation();
                            sourceRegionPickupLocation.SourceRegionId = sourceRegion.Id;
                            sourceRegionPickupLocation.BulkPlantId = item.Id;
                            Context.DataContext.SourceRegionPickupLocations.Add(sourceRegionPickupLocation);
                        }
                        await Context.CommitAsync();
                    }
                    //--------BulkPlants----------------------


                    transaction.Commit();

                    response.StatusCode = Status.Success;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("RegionDomain", "UpdateSourceRegion", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<SourceRegionModel> GetSourceRegion(UserContext userContext)
        {
            SourceRegionModel response = new SourceRegionModel();
            try
            {
                response.CompanyId = userContext.CompanyId;
                response.UserId = userContext.Id;
                response.CountryId = await Context.DataContext.CompanyAddresses.Where(t => t.IsActive && t.IsDefault && t.CompanyId == userContext.CompanyId)
                                                                               .Select(t => t.CountryId)
                                                                               .FirstOrDefaultAsync();

                var sourceRegionModels = new List<SourceRegionViewModel>();
                var sourceRegions = Context.DataContext.SourceRegions.Where(t => t.CompanyId == userContext.CompanyId && t.IsActive).OrderByDescending(t => t.Id).ToList();
                foreach (var item in sourceRegions)
                {
                    var sourceRegionModel = new SourceRegionViewModel();
                    sourceRegionModel.Id = item.Id;
                    sourceRegionModel.Name = item.Name;
                    sourceRegionModel.CompanyId = userContext.CompanyId;
                    sourceRegionModel.Description = item.Description;

                    var sourceRegionAddress = item.SourceRegionAddress;
                    if (sourceRegionAddress != null)
                    {
                        var jsonStates = sourceRegionAddress.States;
                        var statesMessage = JsonConvert.DeserializeObject<List<DropdownDisplayItem>>(jsonStates);
                        sourceRegionModel.States = statesMessage;

                        var jsonCities = sourceRegionAddress.Cities;
                        if (jsonCities != null)
                        {
                            var citiesMessage = JsonConvert.DeserializeObject<List<DropdownDisplayExtendedItem>>(jsonCities);
                            sourceRegionModel.Cities = citiesMessage;
                        }
                    }

                    if (item.SourceRegionCarrier != null)
                    {
                        var sourceRegionCarriers = item.SourceRegionCarrier.Select(t => new DropdownDisplayItem
                        {
                            Id = t.CarrierId,
                            Name = t.Company.Name
                        }).ToList();

                        sourceRegionModel.Carriers = sourceRegionCarriers;
                    }

                    if (item.SourceRegionPickupLocation != null)
                    {
                        var dropDownItems = new List<DropdownDisplayItem>();
                        var sourceRegionTerminal = item.SourceRegionPickupLocation.Where(t => t.TerminalId != null).ToList();
                        foreach (var pickupLocation in sourceRegionTerminal)
                        {
                            var terminalItem = new DropdownDisplayItem();
                            terminalItem.Id = pickupLocation.TerminalId.Value;
                            terminalItem.Name = pickupLocation.MstExternalTerminal.Name;
                            dropDownItems.Add(terminalItem);
                        }
                        sourceRegionModel.Terminals = dropDownItems;

                        dropDownItems = new List<DropdownDisplayItem>();
                        var sourceRegionBulkPlant = item.SourceRegionPickupLocation.Where(t => t.BulkPlantId != null).ToList();
                        foreach (var pickupLocation in sourceRegionBulkPlant)
                        {
                            var bulkPlantItem = new DropdownDisplayItem();
                            bulkPlantItem.Id = pickupLocation.BulkPlantId.Value;
                            bulkPlantItem.Name = pickupLocation.BulkPlantLocation.Name;
                            dropDownItems.Add(bulkPlantItem);
                        }

                        sourceRegionModel.BulkPlants = dropDownItems;
                    }

                    sourceRegionModels.Add(sourceRegionModel);
                    response.Regions = sourceRegionModels;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetSourceRegion", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteSourceRegion(int userId, int sourceRegionId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var sourceRegion = await Context.DataContext.SourceRegions.FirstOrDefaultAsync(t => t.Id == sourceRegionId);
                    if (sourceRegion != null)
                    {
                        sourceRegion.IsActive = false;
                        sourceRegion.UpdatedBy = userId;
                        sourceRegion.UpdatedDate = DateTimeOffset.Now;

                        Context.DataContext.Entry(sourceRegion).State = EntityState.Modified;

                        response.StatusCode = Status.Success;
                        response.StatusMessage = string.Format(Resource.errMessageDeleteSourceRegionSuccess);

                    }
                    await Context.CommitAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageDeleteSourceRegionFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("RegionDomain", "DeleteSourceRegion", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetAllSourceRegionsForDDL(int companyId)
        {
            var ddlItems = new List<DropdownDisplayExtendedItem>();
            try
            {
                ddlItems = await Context.DataContext.SourceRegions.Where(t => t.CompanyId == companyId && t.IsActive)
                                                                     .Select(t => new DropdownDisplayExtendedItem
                                                                     {
                                                                         Id = t.Id,
                                                                         Name = t.Name
                                                                     })
                                                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetAllSourceRegionsForDDL", ex.Message, ex);
            }

            return ddlItems;
        }

        public async Task<SourceRegionResponseModel> GetTerminalsAndBulkPlantsByRegion(int companyId, SourceRegionRequestModel inputModel)
        {
            var response = new SourceRegionResponseModel(Status.Failed);
            try
            {
                if (inputModel.SourceRegionIds == null || inputModel.SourceRegionIds.Count == 0)
                    return response;

                var sourceRegions = await Context.DataContext.SourceRegions.Where(t => t.CompanyId == companyId && t.IsActive &&
                                                                                       inputModel.SourceRegionIds.Any(regionId => regionId == t.Id))
                                                                           .ToListAsync();

                if (sourceRegions.Any())
                {
                    foreach (var item in sourceRegions)
                    {
                        var sourceRegionTerminals = item.SourceRegionPickupLocation.Where(t => t.TerminalId != null).ToList();
                        foreach (var pickupLocation in sourceRegionTerminals)
                        {
                            var terminalItem = new DropdownDisplayExtendedItem();
                            terminalItem.Id = pickupLocation.TerminalId.Value;
                            terminalItem.Name = pickupLocation.MstExternalTerminal.Name;
                            response.Terminals.Add(terminalItem);
                        }

                        var sourceRegionBulkPlants = item.SourceRegionPickupLocation.Where(t => t.BulkPlantId != null).ToList();
                        foreach (var pickupLocation in sourceRegionBulkPlants)
                        {
                            var bulkPlantItem = new DropdownDisplayExtendedItem();
                            bulkPlantItem.Id = pickupLocation.BulkPlantId.Value;
                            bulkPlantItem.Name = pickupLocation.BulkPlantLocation.Name;
                            response.BulkPlants.Add(bulkPlantItem);
                        }
                    }

                    int companyCountryId = await Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetDefaultServingCountry(companyId));
                    if (response.Terminals.Any())
                    {
                        var frDomain = new FuelRequestDomain(this);
                        var pricingDomain = new ExternalPricingDomain(frDomain);

                        inputModel.TerminalIds = response.Terminals.Select(t => t.Id).ToList();
                        inputModel.FuelTypeId = frDomain.GetFuelTypeId(inputModel.FuelTypeId, inputModel.PricingSourceId);
                        var terminals = await pricingDomain.GetClosestTerminalsForSourceRegions(companyId, companyCountryId, inputModel);
                        if(terminals != null && terminals.Any())
                        {
                            response.Terminals = terminals;
                        }
                        else
                        {
                            response.Terminals = new List<DropdownDisplayExtendedItem>();
                        }
                    }

                    if(response.BulkPlants.Any())
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        var bulkplantModel = new BulkPlantRequestModel();

                        bulkplantModel.BulkPlantIds = string.Join(",", response.BulkPlants.Select(t => t.Id));
                        bulkplantModel.JobLatitude = inputModel.Latitude;
                        bulkplantModel.JobLongitude = inputModel.Longitude;
                        bulkplantModel.CountryId = inputModel.CountryId;
                        bulkplantModel.CompanyCountryId = companyCountryId;
                        bulkplantModel.CompanyId = companyId;
                        var bulkPlants = spDomain.GetClosestBulkPlantByDistance(bulkplantModel);
                        if (bulkPlants != null && bulkPlants.Any())
                        {
                            response.BulkPlants = new List<DropdownDisplayExtendedItem>();
                            response.BulkPlants.AddRange(bulkPlants.Select(t => new DropdownDisplayExtendedItem
                            {
                                Id = t.Id,
                                Name = $"{t.Name} : {t.Distance.ToString("N2")}{(companyCountryId == (int)Country.CAN ? Resource.lblKiloMeters : Resource.lblMiles.ToLower())}"
                            }));
                        }
                    }

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Status.Success.ToString();
                }
                else
                {
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.valMessageSourceRegionNotAvailable;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = "Error occured while getting source regions";
                LogManager.Logger.WriteException("RegionDomain", "GetTerminalsAndBulkPlantsByRegion", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetTerminalsBySourceRegion(int companyId, SourceRegionRequestModel inputModel)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                if (inputModel.SourceRegionIds == null || inputModel.SourceRegionIds.Count == 0)
                    return response;

                var sourceRegions = await Context.DataContext.SourceRegions.Where(t => t.CompanyId == companyId && t.IsActive &&
                                                                                       inputModel.SourceRegionIds.Any(regionId => regionId == t.Id))
                                                                           .ToListAsync();

                if (sourceRegions.Any())
                {
                    foreach (var item in sourceRegions)
                    {
                        var sourceRegionTerminals = item.SourceRegionPickupLocation.Where(t => t.TerminalId != null).ToList();
                        foreach (var pickupLocation in sourceRegionTerminals)
                        {
                            var terminalItem = new DropdownDisplayExtendedItem();
                            terminalItem.Id = pickupLocation.TerminalId.Value;
                            terminalItem.Name = pickupLocation.MstExternalTerminal.Name;
                            response.Add(terminalItem);
                        }
                    }

                    int companyCountryId = await Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetDefaultServingCountry(companyId));
                    if (response.Any())
                    {
                        var frDomain = new FuelRequestDomain(this);
                        var pricingDomain = new ExternalPricingDomain(frDomain);

                        inputModel.TerminalIds = response.Select(t => t.Id).ToList();
                        inputModel.FuelTypeId = frDomain.GetFuelTypeId(inputModel.FuelTypeId, inputModel.PricingSourceId);
                        var terminals = await pricingDomain.GetClosestTerminalsForSourceRegions(companyId, companyCountryId, inputModel);
                        if (terminals != null && terminals.Any())
                        {
                            response = terminals;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetTerminalsBySourceRegion", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetBulkPlantsBySourceRegion(int companyId, SourceRegionRequestModel inputModel)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                if (inputModel.SourceRegionIds == null || inputModel.SourceRegionIds.Count == 0)
                    return response;

                var sourceRegions = await Context.DataContext.SourceRegions.Where(t => t.CompanyId == companyId && t.IsActive &&
                                                                                       inputModel.SourceRegionIds.Any(regionId => regionId == t.Id))
                                                                           .ToListAsync();

                if (sourceRegions.Any())
                {
                    foreach (var item in sourceRegions)
                    {
                        var sourceRegionBulkPlants = item.SourceRegionPickupLocation.Where(t => t.BulkPlantId != null).ToList();
                        foreach (var pickupLocation in sourceRegionBulkPlants)
                        {
                            var bulkPlantItem = new DropdownDisplayExtendedItem();
                            bulkPlantItem.Id = pickupLocation.BulkPlantId.Value;
                            bulkPlantItem.Name = pickupLocation.BulkPlantLocation.Name;
                            response.Add(bulkPlantItem);
                        }
                    }

                    int companyCountryId = await Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetDefaultServingCountry(companyId));
                    if (response.Any())
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        var bulkplantModel = new BulkPlantRequestModel();

                        bulkplantModel.BulkPlantIds = string.Join(",", response.Select(t => t.Id));
                        bulkplantModel.JobLatitude = inputModel.Latitude;
                        bulkplantModel.JobLongitude = inputModel.Longitude;
                        bulkplantModel.CountryId = inputModel.CountryId;
                        bulkplantModel.CompanyCountryId = companyCountryId;
                        bulkplantModel.CompanyId = companyId;
                        var bulkPlants = spDomain.GetClosestBulkPlantByDistance(bulkplantModel);
                        if (bulkPlants != null && bulkPlants.Any())
                        {
                            response.AddRange(bulkPlants.Select(t => new DropdownDisplayExtendedItem
                            {
                                Id = t.Id,
                                Name = $"{t.Name} : {t.Distance.ToString("N2")}{(companyCountryId == (int)Country.CAN ? Resource.lblKiloMeters : Resource.lblMiles.ToLower())}"
                            }));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetBulkPlantsBySourceRegion", ex.Message, ex);
            }
            return response;
        }
    }
}
