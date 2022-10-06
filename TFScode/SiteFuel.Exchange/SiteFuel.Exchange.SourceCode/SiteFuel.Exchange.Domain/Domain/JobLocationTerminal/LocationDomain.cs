using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.Domain
{
    public class LocationDomain : BaseDomain
    {
        public LocationDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public LocationDomain(BaseDomain domain) : base(domain)
        {
        }


        public async Task<List<PickupLocationDetailViewModel>> GetBulkPlantsAsync(int countryId, int companyId)
        {
            var response = new List<PickupLocationDetailViewModel>();
            response = await Context.DataContext.BulkPlantLocations.Where(t => t.CountryId == countryId && t.CompanyId == companyId && t.IsActive)
                                                 .Select(t => new PickupLocationDetailViewModel
                                                 {
                                                     Name = t.Name,
                                                     Address = t.Address,
                                                     City = t.City,
                                                     County = t.CountyName,
                                                     Latitude = t.Latitude,
                                                     Longitude = t.Longitude,
                                                     StateCode = t.StateCode,
                                                 })
                                                 .Distinct()
                                                 .ToListAsync();
            return response;
        }

        public async Task<StatusViewModel> SaveBulkPlantLocationAsync(PickupLocationDetailViewModel inputModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            if (inputModel != null)
            {
                if (inputModel.CountryId == (int)Country.CAR
                    && (string.IsNullOrWhiteSpace(inputModel.Address) ||
                    string.IsNullOrWhiteSpace(inputModel.City) ||
                    string.IsNullOrWhiteSpace(inputModel.ZipCode)
                    || inputModel.Latitude == 0 
                    || inputModel.Longitude == 0))
                {
                    var state = Context.DataContext.MstStates.Where(t => t.Id == inputModel.StateId).First();
                    var countryCode = Context.DataContext.MstCountries.First(t => t.Id == inputModel.CountryId).Code;
                    if (string.IsNullOrWhiteSpace(inputModel.Address))
                        inputModel.Address = state.Name ?? Resource.lblCaribbean;
                    if (string.IsNullOrWhiteSpace(inputModel.City))
                        inputModel.City = state.Name ?? Resource.lblCaribbean;
                    if (string.IsNullOrWhiteSpace(inputModel.ZipCode))
                        inputModel.ZipCode = state.Name ?? Resource.lblCaribbean;
                    if (string.IsNullOrWhiteSpace(inputModel.County))
                        inputModel.County = state.Name ?? Resource.lblCaribbean;
                    if (inputModel.Latitude == 0 || inputModel.Longitude == 0)
                    {
                        var point = GoogleApiDomain.GetGeocode($"{inputModel.Address} {inputModel.City} {state.Code} {countryCode} {inputModel.ZipCode}");
                        if (point != null)
                        {
                            inputModel.Latitude = Convert.ToDecimal(point.Latitude);
                            inputModel.Longitude = Convert.ToDecimal(point.Longitude);                           
                        }
                    }                    
                }
                var bulkPLantLocation = inputModel.ToBulkPlantLocationEntity(userContext);

                if (string.IsNullOrWhiteSpace(bulkPLantLocation.CountyName))
                    bulkPLantLocation.CountyName = bulkPLantLocation.City;
                var IsBulkPlantExist = Context.DataContext.BulkPlantLocations.Any(t => t.Name.ToLower() == bulkPLantLocation.Name.ToLower()
                                                                                                && t.IsActive && t.CompanyId == bulkPLantLocation.CompanyId);
                if (IsBulkPlantExist)
                {
                    response.StatusMessage = string.Format(Resource.valMessageAlreadyExist, Resource.lblBulkPlant);
                    response.StatusCode = Utilities.Status.Failed;
                }
                else
                {
                    Context.DataContext.BulkPlantLocations.Add(bulkPLantLocation);
                    await Context.CommitAsync();
                    response.StatusCode = Utilities.Status.Success;
                    response.StatusMessage = "Bulk-Plant saved successfully";
                }
            }
            return response;
        }
    }
}

