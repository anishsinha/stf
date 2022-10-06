using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.MobileAPI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class MasterDomain : BaseDomain
    {
        public MasterDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public MasterDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public List<DropdownDisplayItem> GetCompanyBusinessTenures()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstBusinessTenures
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCompanyBusinessTenures", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetCompanyFuelQuantities()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstFuelQuantities
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
                response.ForEach(t => t.Name = t.Name.ReplaceGallonToGallonsLitres());
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCompanyFuelQuantities", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetCompanySizes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstCompanySizes
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCompanySizes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetCompanyTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstCompanyTypes
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCompanyTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetLoadOptimizationUsers(int companyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .Users
                            .Where(t => t.CompanyId == companyId && t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.FirstName + " " + t.LastName
                            }).OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetLoadOptimizationUsers", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetFuelSurchargeLookupAsync(int countryId, int LookupId)
        {
            List<DropdownDisplayExtendedItem> response = new List<DropdownDisplayExtendedItem>();

            try
            {
                response = await Context.DataContext
                            .MstLookupType
                            .Where(t => t.IsActive && t.LookupId == LookupId && t.CountryId == countryId)
                            .Select(t => new DropdownDisplayExtendedItem()
                            {
                                Id = t.Id,
                                Name = t.Name,
                                Code = t.Value.ToString()
                            }).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetFuelSurchargeLookup", ex.Message, ex);
            }
            return response.ToList();
        }

        public List<DropdownDisplayItem> GetCountries()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstCountries
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCountries", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetMstCountryAsGroup(int countryId)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = Context.DataContext
                            .MstCountryAsGroups
                            .Where(t => t.IsActive && (countryId == 0 || t.CountryId == countryId))
                            .Select(t => new DropdownDisplayExtendedItem
                            {
                                Id = t.Id,
                                Code = t.Code,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMstCountryAsGroup", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetCountriesEx()
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = Context.DataContext
                            .MstCountries
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayExtendedItem
                            {
                                Id = t.Id,
                                Code = t.Code,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCountriesEx", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetMonths()
        {

            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Enumerable
                            .Range(1, 12)
                            .Select(x => new DropdownDisplayItem
                            {
                                Id = x,
                                Name = new DateTimeFormatInfo().GetAbbreviatedMonthName(x)
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMonths", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetMonthsEx()
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = Enumerable
                            .Range(1, 12)
                            .Select(x => new DropdownDisplayExtendedItem
                            {
                                Id = x,
                                Code = x.ToString("D2"),
                                Name = new DateTimeFormatInfo().GetAbbreviatedMonthName(x)
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMonthsEx", ex.Message, ex);
            }
            return response;
        }

        public bool IsTaxExemptionEnabled()
        {
            var response = false;
            try
            {
                var appSetting = Context.DataContext
                            .MstAppSettings
                            .SingleOrDefault(t => t.Key.Equals(Constants.AvaTaxExemptionEnabled));
                if (appSetting != null)
                {
                    response = appSetting.IsActive;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "IsTaxExemptionEnabled", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> IsMultipleServingStates(int companyId, int countryId)
        {
            var response = false;
            try
            {
                var companyDomain = new CompanyDomain(this);
                var servingStates = await companyDomain.GetCompanyServingStates(companyId, countryId);
                if (servingStates.Count > 1)
                {
                    response = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "IsMultipleServingStates", ex.Message, ex);
            }
            return response;
        }

        public string GetApplicationSettingValue(string key)
        {
            var response = string.Empty;
            try
            {
                var appSetting = Context.DataContext.MstAppSettings.SingleOrDefault(t => t.Key.Equals(key));
                if (appSetting != null)
                {
                    response = appSetting.Value;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetApplicationSettingValue", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetNumbers(int startNumber = 0, int endNumber = 10)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Enumerable
                            .Range(startNumber, endNumber - startNumber)
                            .Select(x => new DropdownDisplayItem
                            {
                                Id = x,
                                Name = x.ToString($"D{endNumber.ToString().Length}")
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetNumbers", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetPhoneTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstPhoneTypes
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetPhoneTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetStates(int countryId = 0)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var states = Context.DataContext.MstStates.Where(t => t.CountryId == countryId || countryId == 0);
                response = states
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetStates", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetTerminalStates(int countryId = 0)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (countryId == (int)Country.CAR)
                {
                    response = Context.DataContext.MstStates.Where(t => t.CountryGroupId != null && t.CountryId == (int)Country.CAR)
                                .Select(t => new DropdownDisplayItem() { Id = t.Id, Name = t.Name }).Distinct().OrderBy(t => t.Name).ToList();
                }
                else
                    response = (from terminal in Context.DataContext.MstExternalTerminals
                                join state in Context.DataContext.MstStates on terminal.StateId equals state.Id
                                join country in Context.DataContext.MstCountries on terminal.CountryCode equals country.Code
                                where country.Id == countryId && country.IsActive && state.IsActive && terminal.IsActive
                                select new DropdownDisplayItem { Id = state.Id, Name = state.Name }).Distinct().OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetTerminalStates", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetTerminalCities(List<int> stateIds)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = (from terminal in Context.DataContext.MstExternalTerminals
                            join state in Context.DataContext.MstStates on terminal.StateId equals state.Id
                            //join city in Context.DataContext.MstCities on terminal.StateId equals city.StateId
                            where stateIds.Contains(terminal.StateId) && state.IsActive && terminal.IsActive && terminal.ControlNumber != "-"
                            select new DropdownDisplayExtendedItem { Code = terminal.City.Replace(" ", ""), Name = terminal.City }).Distinct().OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetTerminalCities", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetStates(List<int> countryId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var states = Context.DataContext.MstStates.Where(t => countryId.Contains(t.CountryId));
                response = states
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetStates", ex.Message, ex);
            }
            return response;
        }

        public string RemoveWhiteSpaces(string str)
        {
            return str.Replace(" ", "");
        }

        public List<DropdownDisplayItem> GetStatesForOffer(bool withAllOption)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = GetStates((int)Country.USA);
                if (withAllOption)
                    response.Insert(0, new DropdownDisplayItem() { Id = 0, Name = "Select All" });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetStates", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetStatesForOffer(int countryId, bool withAllOption)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = GetStates(countryId);
                if (withAllOption)
                    response.Insert(0, new DropdownDisplayItem() { Id = 0, Name = "Select All" });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetStatesForOffer", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetCities(int stateId, bool withAllOption)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext.MstCities
                                    .Where(t => t.StateId == stateId).
                                    OrderBy(x => x.Name)
                                    .Select(t => new DropdownDisplayItem
                                    {
                                        Id = t.Id,
                                        Name = t.Name
                                    }).ToList();

                if (withAllOption)
                    response.Insert(0, new DropdownDisplayItem() { Id = 0, Name = "Select All" });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCities", ex.Message, ex);
            }
            return response;
        }
        public List<DropdownDisplayItem> GetCities(List<int> stateId, bool withAllOption = false)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext.MstCities
                                    .Where(t => stateId.Contains(t.StateId)).
                                    OrderBy(x => x.Name)
                                    .Select(t => new DropdownDisplayItem
                                    {
                                        Id = t.Id,
                                        Name = t.Name
                                    }).ToList();
                if (withAllOption)
                    response.Insert(0, new DropdownDisplayItem() { Id = 0, Name = "Select All" });

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCities", ex.Message, ex);
            }
            return response;
        }
        public List<DropdownDisplayItem> GetZipCodes(List<int> CityIds, bool withAllOption = false)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var zips = Context.DataContext.MstCities
                                    .Where(t => CityIds.Contains(t.Id) && t.ZipCodes != null)
                                    .Select(t => t.ZipCodes).ToList();
                foreach (var zip in zips)
                {
                    zip.Split(',').ToList().ForEach(t => response.Add(new DropdownDisplayItem { Name = t.Trim() }));
                }
                response = response.OrderBy(t => t.Name).Distinct().ToList();
                if (withAllOption)
                    response.Insert(0, new DropdownDisplayItem() { Id = 0, Name = "Select All" });

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCities", ex.Message, ex);
            }
            return response;
        }
        public List<DropdownDisplayItem> GetCityGroupCities(PricingSource sourceId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var dummyZip = Constants.DummyAxxisZipCode;
                if (sourceId == PricingSource.OPIS)
                {
                    dummyZip = Constants.DummyOpisZipCode;
                }
                else if (sourceId == PricingSource.PLATTS)
                {
                    dummyZip = Constants.DummyPlattsZipCode;
                }
                response = Context.DataContext
                            .MstExternalTerminals
                            .Where(t => t.IsActive && t.PricingSourceId == (int)sourceId && t.ZipCode == dummyZip)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.City + "," + t.StateCode
                            }).Distinct().ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCityGroupCities", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetPoContact(int companyId, int companyTypeId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var company = Context.DataContext.Companies.SingleOrDefault(t => t.Id == companyId && t.IsActive);
                if (company != null)
                {
                    if (companyTypeId == (int)CompanyType.BuyerAndSupplier)
                    {
                        var companyRoles = Context.DataContext.MstCompanyTypeXRoles.Where(t => t.CompanyTypeId == (int)CompanyType.Buyer && t.IsActive).Select(t => t.RoleId).ToList();
                        response = company
                                        .Users
                                        .Where(t => t.IsActive && t.MstRoles.Select(t1 => t1.Id).Intersect(companyRoles).Any())
                                        .OrderByDescending(t => t.Id)
                                        .Select(t => new DropdownDisplayItem
                                        {
                                            Id = t.Id,
                                            Name = t.FirstName + ' ' + t.LastName
                                        }).ToList();
                    }
                    else
                    {
                        response = company
                                        .Users
                                        .Where(t => t.IsActive)
                                        .OrderByDescending(t => t.Id)
                                        .Select(t => new DropdownDisplayItem
                                        {
                                            Id = t.Id,
                                            Name = t.FirstName + ' ' + t.LastName
                                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetPoContact", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetBrokerPoContact(int companyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var company = Context.DataContext.Companies.SingleOrDefault(t => t.Id == companyId && t.IsActive);
                if (company != null)
                {
                    response = company
                                    .Users
                                    .Where(t => t.IsActive && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.Supplier ||
                                        t1.Id == (int)UserRoles.Admin || t1.Id == (int)UserRoles.AccountingPerson))
                                    .OrderByDescending(t => t.Id)
                                    .Select(t => new DropdownDisplayItem
                                    {
                                        Id = t.Id,
                                        Name = t.FirstName + ' ' + t.LastName
                                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetBrokerPoContact", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetBusinessSubTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstBusinessSubTypes
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Jurisdiction + " - " + t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetBusinessSubTypes", ex.Message, ex);
            }
            return response;
        }
        public List<DropdownDisplayItem> GetCompanyBuyers(int companyId, int companyTypeId)
        {
            var response = new List<DropdownDisplayItem>();

            var company = Context.DataContext.Companies.Include(t => t.Users).SingleOrDefault(t => t.Id == companyId && t.IsActive);
            if (company != null)
            {
                if (companyTypeId == (int)CompanyType.BuyerAndSupplier)
                {
                    var companyRoles = Context.DataContext.MstCompanyTypeXRoles.Where(t => t.CompanyTypeId == (int)CompanyType.Buyer && t.IsActive).Select(t => t.RoleId).ToList();
                    response = company
                                    .Users
                                    .Where(t => t.IsActive && t.MstRoles.Select(t1 => t1.Id).Intersect(companyRoles).Any() && !t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.Admin))
                                    .OrderByDescending(t => t.Id).Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.FirstName + ' ' + t.LastName }).ToList();
                }
                else
                {
                    response = company.Users.Where(t => t.IsActive && !t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.Admin))
                                .Select(t => new DropdownDisplayItem { Id = t.Id, Name = $"{t.FirstName} {t.LastName}" })
                                .ToList();
                }
            }

            return response;
        }

        public List<DropdownDisplayItem> GetCompanyAdmins(int companyId)
        {
            var response = new List<DropdownDisplayItem>();

            var company = Context.DataContext.Companies.Include(t => t.Users).SingleOrDefault(t => t.Id == companyId && t.IsActive);
            if (company != null)
            {
                var admins = company.Users.Where(t => t.IsActive && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.Admin))
                            .Select(t => new DropdownDisplayItem { Id = t.Id, Name = $"{t.FirstName} {t.LastName}" });
                response.AddRange(admins);
            }

            return response;
        }

        public List<StateDropdownExtendedItem> GetStatesEx(int countryId = (int)Country.All)
        {
            var response = new List<StateDropdownExtendedItem>();
            try
            {
                response = Context.DataContext
                            .MstStates
                            .Where(t => t.IsActive && (countryId == (int)Country.All || t.CountryId == countryId))
                            .Select(t => new StateDropdownExtendedItem
                            {
                                Id = t.Id,
                                Code = t.Code,
                                Name = t.Name,
                                CountryGroupId = t.CountryGroupId,
                                CountryId = t.CountryId

                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetStatesEx", ex.Message, ex);
            }
            return response;
        }

        public List<StateListViewModel> GetStateList()
        {
            var response = new List<StateListViewModel>();
            try
            {
                response = Context.DataContext
                            .MstStates
                            .Where(t => t.IsActive)
                            .Select(t => new StateListViewModel
                            {
                                Id = t.Id,
                                Code = t.Code,
                                Name = t.Name,
                                CountryId = t.CountryId
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetStateList", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetSupplierQualifications()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstSupplierQualifications
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetSupplierQualifications", ex.Message, ex);
            }
            return response;
        }
        //if user login with branded supplier company URL then user will only see only branded private supplier.
        //we exclude the other supplier
        public List<DropdownDisplayItem> GetPrivateSuppliers(int companyId, int brandedCompanyId = -1)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var settingsDomain = ContextFactory.Current.GetDomain<SettingsDomain>();
                var blacklistedCompanyIds = Task.Run(() => settingsDomain.GetBlacklistedCompanyIdsAsync(companyId)).Result;

                response = Context.DataContext.Companies.Where(t => t.IsActive && t.Id != companyId
                            && !blacklistedCompanyIds.Contains(t.Id)
                            && (t.CompanyTypeId == (int)CompanyType.BuyerAndSupplier
                            || t.CompanyTypeId == (int)CompanyType.Supplier || t.CompanyTypeId == (int)CompanyType.SupplierAndCarrier || t.CompanyTypeId == (int)CompanyType.BuyerSupplierAndCarrier))
                            .Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.Name }).ToList();
                if (brandedCompanyId > 0)
                {
                    response = response.Where(top => top.Id == brandedCompanyId).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetPrivateSuppliers", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetYourSuppliers(int companyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var companies = Context.DataContext.Orders.Where(t => t.IsActive && t.BuyerCompanyId == companyId)
                            .Select(t => t.AcceptedCompanyId).ToList();

                response = Context.DataContext.Companies.Where(t => t.IsActive && !t.Name.Contains("Deleted") && companies.Contains(t.Id))
                           .Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.Name }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetYourSuppliers", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetYourCustomers(int companyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var companies = Context.DataContext.Orders.Where(t => t.IsActive && t.AcceptedCompanyId == companyId)
                             .Select(t => t.BuyerCompanyId).ToList();

                response = Context.DataContext.Companies.Where(t => !t.Name.Contains("Deleted") && companies.Contains(t.Id))
                           .Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.Name }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetYourCustomers", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DropdownDisplayItem>> GetYourCustomersForDipTest(int companyId, CompanyType companyTypeId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                List<int> jobIds = new List<int>();
                if (companyTypeId == CompanyType.Carrier || companyTypeId == CompanyType.BuyerSupplierAndCarrier || companyTypeId == CompanyType.SupplierAndCarrier)
                {
                    jobIds = await new FreightServiceDomain(this).GetCarriersJobs(companyId, 0);
                }
                List<DropdownDisplayItem> companyListWithDetails = new List<DropdownDisplayItem>();
                if (companyTypeId != CompanyType.Carrier)
                {
                    var companyList = Context.DataContext.Orders.Where(t => t.IsActive && t.AcceptedCompanyId == companyId)
                        .Select(t => t.BuyerCompanyId).ToList();
                    companyListWithDetails = Context.DataContext.Companies.Where(t => !t.Name.Contains("Deleted") && companyList.Contains(t.Id))
                                              .Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.Name }).ToList();
                    response = companyListWithDetails;
                }
                if (jobIds != null && jobIds.Any())
                {
                    var companyListWithDetailsForCarrier = Context.DataContext.Jobs.Where(t => t.IsActive && (jobIds.Any() && jobIds.Contains(t.Id))
                                                                                                      && !t.Company.Name.Contains("Deleted")
                                                                                                      && t.Company.IsActive)
                                                                                                      .Select(t => new DropdownDisplayItem { Id = t.Company.Id, Name = t.Company.Name }).Distinct().ToList();
                    if (companyListWithDetailsForCarrier != null && companyListWithDetailsForCarrier.Any())
                    {
                        companyListWithDetails.AddRange(companyListWithDetailsForCarrier);
                    }
                    response = companyListWithDetails.Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetYourCustomersForDipTest", ex.Message, ex);
            }
            return response;
        }
        public List<DropdownDisplayItem> GetYears(int minusYears = 0, int plusYears = 10)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Enumerable
                            .Range(DateTimeOffset.Now.Year - minusYears, plusYears)
                            .Select(x => new DropdownDisplayItem
                            {
                                Id = x,
                                Name = x.ToString("D4")
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetYears", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetYearsEx(int minusYears = 0, int plusYears = 10)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = Enumerable
                            .Range(DateTimeOffset.Now.Year - minusYears, plusYears)
                            .Select(x => new DropdownDisplayExtendedItem
                            {
                                Id = x,
                                Code = x.ToString("D4").Substring(2, 2),
                                Name = x.ToString("D4")
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetYearsEx", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetRolesByCompanyType(int companyTypeId, int companySubtypeId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (companyTypeId == (int)CompanyType.BuyerAndSupplier && companySubtypeId > 0)
                {
                    companyTypeId = companySubtypeId;
                }

                response = Context.DataContext
                                .MstCompanyTypeXRoles
                                .Where(t => t.CompanyTypeId == companyTypeId && t.IsActive)
                                .Select(t => new DropdownDisplayItem
                                {
                                    Id = t.MstRole.Id,
                                    Name = t.MstRole.Name
                                }).ToList();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetRolesByCompanyType", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetEventTypeByRoles(int companyTypeId, IList<int> roleIds, CompanyType companySubTypeId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (companyTypeId == (int)CompanyType.BuyerAndSupplier)
                {
                    //companyTypeId = (int)companySubTypeId;

                    response = Context.DataContext
                               .MstCompanyUserRoleXEventTypes
                               .Where(t => roleIds.Contains(t.RoleId) &&
                                (t.NotificationType == (int)NotificationType.Sms || t.NotificationType == (int)NotificationType.EmailAndSms))
                               .Select(t => new DropdownDisplayItem
                               {
                                   Id = t.MstEventType.Id,
                                   Name = t.MstEventType.Name
                               }).Distinct().ToList();
                }
                else
                {
                    response = Context.DataContext
                                    .MstCompanyUserRoleXEventTypes
                                    .Where(t => t.CompanyTypeId == companyTypeId && roleIds.Contains(t.RoleId) &&
                                     (t.NotificationType == (int)NotificationType.Sms || t.NotificationType == (int)NotificationType.EmailAndSms))
                                    .Select(t => new DropdownDisplayItem
                                    {
                                        Id = t.MstEventType.Id,
                                        Name = t.MstEventType.Name
                                    }).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetEventTypeByRoles", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetProductTypes(int productTypeId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstProductTypes
                            .Where(t => t.IsActive && t.Id != productTypeId)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetProductTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetProductTypes(List<int> exlcudedproductTypeIds)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstProductTypes
                            .Where(t => t.IsActive && !exlcudedproductTypeIds.Contains(t.Id))
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetProductTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetProductTypesForMapping()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstProductTypes
                            .Where(t => t.IsActive && t.Id != (int)ProductTypes.NonStandardFuel && t.Id != (int)ProductTypes.Additives)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetProductTypesForMapping", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetAssetFuelTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstProductTypes
                            .Where(t => t.IsActive && t.Id != (int)ProductTypes.Additives)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAssetFuelTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetJobByFuelRequest(int fuelRequestId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var fuelRequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == fuelRequestId && t.IsActive);
                if (fuelRequest != null)
                {
                    response.Add(new DropdownDisplayItem()
                    {
                        Id = fuelRequest.Job.Id,
                        Name = fuelRequest.Job.Name
                    });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetJobByFuelRequest", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetJobs(int userId, int countryId = (int)Country.All, bool isMFN = false)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var helperDomain = new HelperDomain(this);
                var jobIds = Task.Run(() => helperDomain.GetJobIdsAsync(userId)).Result;
                if (jobIds != null)
                {
                    if (!isMFN)
                    {
                        var jobs = Context.DataContext.Jobs.Where(t => t.IsActive &&
                                                              t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open
                                                              && (countryId == (int)Country.All || countryId == t.CountryId)
                                                              && jobIds.Contains(t.Id)
                                                              ).ToList();

                        response = jobs.Where(t => t.EndDate == null || t.EndDate.Value.Date >= DateTimeOffset.Now.ToTargetDateTimeOffset(t.TimeZoneName).Date)
                                        .Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.Name })
                                        .OrderByDescending(t => t.Id)
                                        .ToList();
                    }
                    else
                    {
                        var result = Context.DataContext.Jobs.Where(t => t.IsActive &&
                                                              t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open
                                                              && (countryId == (int)Country.All || countryId == t.CountryId)
                                                              && t.LocationType == JobLocationTypes.Port && (jobIds.Contains(t.Id) || t.CreatedByCompanyId == ApplicationConstants.SuperAdminCompanyId))
                                                              .Select(t => new DropdownDisplayExtendedItem { Id = t.Id, Name = t.Name, Code = t.CreatedByCompanyId.ToString() })
                                          .OrderByDescending(t => t.Id)
                                          .ToList();
                        List<DropdownDisplayItem> jobList = result.Where(w => w.Code != ApplicationConstants.SuperAdminCompanyId.ToString()).Select(s => new DropdownDisplayItem() { Id = s.Id, Name = s.Name }).ToList();
                        List<DropdownDisplayItem> superAdminJobList = result.Where(w => w.Code == ApplicationConstants.SuperAdminCompanyId.ToString()).Select(s => new DropdownDisplayItem() { Id = s.Id, Name = s.Name }).ToList();
                        jobList.ForEach(t => superAdminJobList.RemoveAll(w => w.Name.ToLower() == t.Name.ToLower()));
                        jobList.AddRange(superAdminJobList);
                        response.AddRange(jobList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetJobs", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetJobsForAsset(int companyId, int assetId, int jobId, int userId, JobStatus jobStatus = JobStatus.Open)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var asset = Context.DataContext.Assets.SingleOrDefault(t => t.Id == assetId && t.IsActive);
                if (asset != null)
                {
                    var helperDomain = new HelperDomain(this);
                    var assignedJobs = Task.Run(() => helperDomain.GetJobIdsAsync(userId)).Result;
                    if (asset.Type == (int)AssetType.Asset)
                    {
                        response = Context.DataContext
                                        .Jobs
                                        .Where(t => t.IsActive && t.JobBudget.IsAssetTracked && t.Id != jobId
                                                    && t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)jobStatus
                                                    && assignedJobs.Contains(t.Id) && t.CompanyId == companyId
                                                    && !t.IsRetailJob)
                                        .OrderByDescending(t => t.Id)
                                        .Select(t => new DropdownDisplayItem
                                        {
                                            Id = t.Id,
                                            Name = t.Name
                                        }).ToList();
                    }
                    else if (asset.Type == (int)AssetType.Tank)
                    {
                        response = Context.DataContext
                                        .Jobs
                                        .Where(t => t.IsActive && t.Id != jobId
                                                    && t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)jobStatus
                                                    && assignedJobs.Contains(t.Id) && t.CompanyId == companyId
                                                    && t.IsRetailJob)
                                        .OrderByDescending(t => t.Id)
                                        .Select(t => new DropdownDisplayItem
                                        {
                                            Id = t.Id,
                                            Name = t.Name
                                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetJobsForAsset", ex.Message, ex);
            }
            return response;
        }

        public async Task<ServiceBaseViewModel> GetFreightServiceParameters(int companyId, int userId)
        {
            ServiceBaseViewModel response = null;
            try
            {
                response = await Context.DataContext
                            .MstAppSettings
                            .Where(t => t.IsActive && t.Key.Equals("FreightServiceBaseUrl"))
                            .Select(t => new ServiceBaseViewModel
                            {
                                BaseUrl = t.Value,
                                Token = "token"
                            }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetFreightServiceParameters", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetOrdersForCompany(int userCompanyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var orders = Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == userCompanyId).ToList();

                for (int cntOrders = 0; cntOrders < orders.Count; cntOrders++)
                {
                    if (orders[cntOrders].FuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && !orders[cntOrders].IsEndSupplier)
                    {
                        orders.RemoveAt(cntOrders);
                    }
                }
                orders.ForEach(t => response.Add(new DropdownDisplayItem()
                {
                    Id = t.Id,
                    Name = t.PoNumber
                }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetOrdersForCompany", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetCustomersForCompany(int userCompanyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response.Add(new DropdownDisplayItem { Id = 0, Name = "All Customers" });
                var orders = Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == userCompanyId).ToList();

                for (int cntOrders = 0; cntOrders < orders.Count; cntOrders++)
                {
                    if (orders[cntOrders].FuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && !orders[cntOrders].IsEndSupplier)
                    {
                        orders.RemoveAt(cntOrders);
                    }
                }

                orders = orders.GroupBy(p => p.BuyerCompanyId).Select(grp => grp.FirstOrDefault()).ToList();
                orders.ForEach(t => response.Add(new DropdownDisplayItem()
                {
                    Id = t.BuyerCompanyId,
                    Name = t.BuyerCompany.Name
                }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCustomersForCompany", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetAllFuelProducts(bool IsDefaultOption = false)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (IsDefaultOption)
                {
                    response.Add(new DropdownDisplayItem { Id = 0, Name = "All Fuel Types" });
                }
                response.AddRange(Context.DataContext
                               .MstTfxProducts
                               .Where(t => t.IsActive && t.ProductDisplayGroupId != (int)ProductDisplayGroups.OtherFuelType)
                               .Select(t => new DropdownDisplayItem
                               {
                                   Id = t.Id,
                                   Name = t.Name
                               }).ToList()
                               );
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAllFuelProducts", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetProducts(string searchText = "")
        {
            var response = new List<DropdownDisplayItem>();
            searchText = string.IsNullOrWhiteSpace(searchText) ? "" : searchText.ToLower();
            try
            {
                var products = await Context.DataContext
                                 .MstProducts
                                 .Where(t => t.IsActive && t.MstProductDisplayGroup.Id != (int)ProductDisplayGroups.OtherFuelType && (searchText == "" || t.Name.ToLower().Contains(searchText)))
                                 .Select(t => new DropdownDisplayItem
                                 {
                                     Id = t.Id,
                                     Name = t.Name
                                 }).ToListAsync();

                response.AddRange(products);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetProducts", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetInvoiceDeclineReasons(UoM uoM = UoM.Gallons)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var declinedReasons = Context.DataContext
                            .MstInvoiceDeclineReasons
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name,
                            }).ToList();
                if (uoM == UoM.Litres)
                {
                    declinedReasons.ForEach(t => t.Name = t.Name.ReplaceGallonToLitre());
                }
                var other = declinedReasons.Where(t => t.Id == (int)InvoiceDeclinedReason.Other).ToList();
                response = declinedReasons.Except(other).ToList();
                response.AddRange(other);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetInvoiceDeclineReasons", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetProductDisplayGroups(int companyId = 0, bool isTPO = false)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstProductDisplayGroups
                            .Where(t => t.IsActive && t.Id != (int)ProductDisplayGroups.OtherFuelType
                                  && (!isTPO || (isTPO && t.Id != (int)ProductDisplayGroups.FuelTypesInYourArea && t.Id != (int)ProductDisplayGroups.FavoriteFuelType)))
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name,
                            }).ToList();

                if (companyId > 0 && !isTPO && Context.DataContext.CompanyFavoriteFuels.Any(t => t.CompanyId == companyId && t.RemovedBy == null))
                {
                    response.Insert(0, new DropdownDisplayItem { Id = 0, Name = Resource.lblFavoriteFuelType });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetProductDisplayGroups", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetProductDisplayGroupsForMapping()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstProductDisplayGroups
                            .Where(t => t.IsActive &&
                                        (t.Id != (int)ProductDisplayGroups.FuelTypesInYourArea &&
                                         t.Id != (int)ProductDisplayGroups.FavoriteFuelType &&
                                         t.Id != (int)ProductDisplayGroups.OtherFuelType &&
                                         t.Id != (int)ProductDisplayGroups.AdditiveFuelType))
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name,
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetProductDisplayGroupsForMapping", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetOrderTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstOrderTypes
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name,
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetOrderTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetRackAvgPricingTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstRackAvgPricingTypes
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name,
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetRackAvgPricingTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetMarketBasedPricingTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstPricingTypes
                            .Where(t => t.IsActive && (t.Id == (int)PricingType.RackAverage || t.Id == (int)PricingType.RackHigh || t.Id == (int)PricingType.RackLow))
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMarketBasedPricingTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetPricingSources()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext.MstPricingSources.Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetPricingSources", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetPricingSourceFeeds(int pricingSourceId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext.MstFeedTypes.Where(t => t.IsActive && t.PricingSourceId == pricingSourceId)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetPricingSourceFeeds", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetResaleFeeTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstFeeXFeeSubTypes
                            .Where(t => t.FeeTypeId == (int)FeeType.ResaleFee && t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.MstFeeSubType.Id,
                                Name = t.MstFeeSubType.Name,
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetResaleFeeTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetMathOperators()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstMathOperators
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Description
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMathOperators", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetPricingTypes(bool isTierPricingRequired = false)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstPricingTypes
                            .Where
                            (
                                t => t.IsActive &&
                                (!isTierPricingRequired ? t.Id != (int)PricingType.Tier : true)
                            )
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetPricingTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetPricingTypesForOfferTiers(bool isWithSupplierCost)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstPricingTypes
                            .Where
                            (
                                t => t.IsActive &&
                                (!isWithSupplierCost ? t.Id != (int)PricingType.Tier && t.Id != (int)PricingType.Suppliercost : true)
                            )
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetPricingTypesForOfferTiers", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetPaymentTerms()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstPaymentTerms
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).OrderBy(x => x.Id).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetPaymentTerms", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetDeliveryScheduleStatuses()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstDeliveryScheduleStatuses
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).OrderBy(x => x.Id).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetDeliveryScheduleStatuses", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetJobStatuses()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstJobStatuses
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).OrderBy(x => x.Id).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetJobStatuses", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetFuelRequestStatuses()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstFuelRequestStatuses
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).OrderBy(x => x.Id).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetFuelRequestStatuses", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetOrderStatuses()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstOrderStatuses
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).OrderBy(x => x.Id).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetOrderStatuses", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetQuantityTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstQuantityTypes
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).OrderBy(x => x.Id).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetQuantityTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetDeliveryTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstDeliveryTypes
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).OrderBy(x => x.Id).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetDeliveryTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetMstWeekDays()
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = Context.DataContext
                            .MstWeekDays
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayExtendedItem
                            {
                                Id = t.Id,
                                Name = t.Name,
                                Code = t.Code
                            }).OrderBy(x => x.Id).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMstWeekDays", ex.Message, ex);
            }
            return response;
        }
        //if user login with branded supplier company URL then user will only see only branded private supplier.
        //we exclude the other supplier
        public List<DropdownDisplayItem> GetPrivateSupplierList(int companyId, int brandedCompanyId = -1)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var company = Context.DataContext.Companies.SingleOrDefault(t => t.Id == companyId && t.IsActive);
                if (company != null)
                {
                    var settingsDomain = ContextFactory.Current.GetDomain<SettingsDomain>();
                    var blacklistedCompanyIds = Task.Run(() => settingsDomain.GetBlacklistedCompanyIdsAsync(companyId)).Result;

                    var privateSupplierLists = company.PrivateSupplierLists.Where(t => t.IsActive && t.Companies.Any(t1 => !blacklistedCompanyIds.Contains(t1.Id))).ToList();
                    if (brandedCompanyId > 0)
                    {
                        privateSupplierLists = privateSupplierLists.Where(top => top.Companies.Any(x => x.Id == brandedCompanyId)).ToList();
                    }
                    privateSupplierLists.ForEach(t => response.Add(new DropdownDisplayItem
                    {
                        Id = t.Id,
                        Name = t.Name
                    }));

                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetPrivateSupplierList", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetPrivateSupplierListByFuelRequest(int fuelRequestId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var fuelRequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == fuelRequestId);
                if (fuelRequest != null)
                {
                    var settingsDomain = ContextFactory.Current.GetDomain<SettingsDomain>();
                    var blacklistedCompanyIds = Task.Run(() => settingsDomain.GetBlacklistedCompanyIdsAsync(fuelRequest.User.Company.Id)).Result;

                    var privateSupplierLists = fuelRequest.PrivateSupplierLists.Where(t => t.IsActive && t.Companies.Any(t1 => !blacklistedCompanyIds.Contains(t1.Id))).ToList();
                    privateSupplierLists.ForEach(t => response.Add(new DropdownDisplayItem
                    {
                        Id = t.Id,
                        Name = t.Name
                    }));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetPrivateSupplierListByFuelRequest", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetAdditionalFee()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstFeeXFeeSubTypes
                            .Where(t => t.FeeTypeId == (int)FeeType.AdditionalFee && t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.MstFeeSubType.Id,
                                Name = t.MstFeeSubType.Name,
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAdditionalFee", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetTaxesForOtherProductFuelType(int orderId = 0)
        {
            var response = new List<DropdownDisplayItem>();
            // var result = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstTaxPricingTypes
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name,
                            }).ToList();
                if (orderId > 0)
                {
                    response = ChangeUoMCurrencyAsPerCountry(response, orderId);
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetTaxesForOtherProductFuelType", ex.Message, ex);
            }
            return response;
        }

        private List<DropdownDisplayItem> ChangeUoMCurrencyAsPerCountry(List<DropdownDisplayItem> otherProductTaxDdls, int orderId)
        {
            try
            {
                var currency = Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => t.FuelRequest.Currency).FirstOrDefault();
                if (currency == Currency.CAD && currency != Currency.None)
                {
                    foreach (var otherProductTaxDdl in otherProductTaxDdls)
                    {
                        if (otherProductTaxDdl.Name == TaxPricingTypes.PercentPerGallon.GetDisplayName())
                        {
                            otherProductTaxDdl.Name = TaxPricingTypes.PercentPerLitre.GetDisplayName();
                        }
                        if (otherProductTaxDdl.Name == TaxPricingTypes.AmountPerGallon.GetDisplayName())
                        {
                            otherProductTaxDdl.Name = TaxPricingTypes.AmountPerLitre.GetDisplayName();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "ChangeUoMCurrencyAsPerCountry", ex.Message, ex);

            }
            return otherProductTaxDdls;
        }

        public List<MasterDataFeesViewModel> GetFeeTypes()
        {
            var response = new List<MasterDataFeesViewModel>();
            try
            {
                var exculdedFees = new List<int>() { (int)FeeType.ResaleFee, (int)FeeType.FreightFee,
                                            (int)FeeType.EnvironmentalFee,(int)FeeType.ServiceFee,(int)FeeType.LoadFee,
                                            (int)FeeType.SurchargeFee,(int)FeeType.ProcessingFee,(int)FeeType.OtherFee};
                response = Context.DataContext
                            .MstFeeTypes
                            .Where(t => t.IsActive && !exculdedFees.Contains(t.Id))
                            .Select(t => new MasterDataFeesViewModel
                            {
                                Id = t.Id,
                                Name = t.Name,
                                TruckLoadType = t.TruckLoadCategoryId
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetFeeTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetFeeSubTypes(int feeTypeId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstFeeXFeeSubTypes
                            .Where(t => t.FeeTypeId == feeTypeId && t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.MstFeeSubType.Id,
                                Name = t.MstFeeSubType.Name,
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetFeeSubTypes", ex.Message, ex);
            }
            return response;
        }

        public List<FeeSubTypeDdl> GetAllFeeSubTypes(int orderId)
        {
            var response = new List<FeeSubTypeDdl>();
            var result = new List<FeeSubTypeDdl>();
            try
            {
                response = Context.DataContext
                            .MstFeeXFeeSubTypes
                            .Where(t => t.IsActive)
                            .Select(t => new FeeSubTypeDdl
                            {
                                FeeTypeId = t.FeeTypeId.ToString(),
                                FeeSubTypeId = t.FeeSubTypeId,
                                SubTypeName = t.MstFeeSubType.Name,
                            }).ToList();


                response.Add(new FeeSubTypeDdl { FeeTypeId = "14", FeeSubTypeId = 17, SubTypeName = Constants.PerGallon });
                response.Add(new FeeSubTypeDdl { FeeTypeId = "14", FeeSubTypeId = 5, SubTypeName = Constants.PerHour });
                response.Add(new FeeSubTypeDdl { FeeTypeId = "14", FeeSubTypeId = 2, SubTypeName = Constants.FlatFee });
                result = ChangeFeeSubTypesNameAsPerCountry(response, orderId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAllFeeSubTypes", ex.Message, ex);
            }
            return result;
        }

        private List<FeeSubTypeDdl> ChangeFeeSubTypesNameAsPerCountry(List<FeeSubTypeDdl> feeSubTypeDdls, int orderId)
        {
            try
            {
                var currency = (from O in Context.DataContext.Orders
                                where O.Id == orderId
                                join fr in Context.DataContext.FuelRequests
                                on O.FuelRequestId equals
                                fr.Id
                                select fr.Currency).FirstOrDefault();
                if (currency != Currency.None)
                {
                    if (currency == Currency.CAD)
                    {
                        foreach (var feeSubTypeDdl in feeSubTypeDdls)
                        {
                            if (feeSubTypeDdl.SubTypeName == Constants.PerGallon)
                            {
                                feeSubTypeDdl.SubTypeName = Constants.PerLitre;
                            }
                        }
                        return feeSubTypeDdls;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "ChangeFeeSubTypesNameAsPerCountry", ex.Message, ex);

            }
            return feeSubTypeDdls;
        }
        public List<DropdownDisplayItem> GetAdditionalFeeSubTypes(int feeTypeId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstFeeXFeeSubTypes
                            .Where(t => t.FeeTypeId == feeTypeId)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.MstFeeSubType.Id,
                                Name = t.MstFeeSubType.Name,
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAdditionFeeSubTypes", ex.Message, ex);
            }
            return response;
        }


        public List<FeeSubTypeDdl> GetAllFeeSubTypes(string currency)
        {
            var response = new List<FeeSubTypeDdl>();
            var result = new List<FeeSubTypeDdl>();
            try
            {
                response = Context.DataContext
                            .MstFeeXFeeSubTypes
                            .Where(t => t.IsActive)
                            .Select(t => new FeeSubTypeDdl
                            {
                                FeeTypeId = t.FeeTypeId.ToString(),
                                FeeSubTypeId = t.FeeSubTypeId,
                                SubTypeName = t.MstFeeSubType.Name,
                            }).ToList();


                response.Add(new FeeSubTypeDdl { FeeTypeId = "14", FeeSubTypeId = 17, SubTypeName = Constants.PerGallon });
                response.Add(new FeeSubTypeDdl { FeeTypeId = "14", FeeSubTypeId = 5, SubTypeName = Constants.PerHour });
                response.Add(new FeeSubTypeDdl { FeeTypeId = "14", FeeSubTypeId = 2, SubTypeName = Constants.FlatFee });
                result = ChangeFeeSubTypesNameAsPerCountry(response, currency);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAllFeeSubTypes", ex.Message, ex);
            }
            return result;
        }

        private List<FeeSubTypeDdl> ChangeFeeSubTypesNameAsPerCountry(List<FeeSubTypeDdl> feeSubTypeDdls, string currency)
        {
            try
            {


                if (!string.IsNullOrEmpty(currency))
                {
                    if (currency == "CAD")
                    {
                        foreach (var feeSubTypeDdl in feeSubTypeDdls)
                        {
                            if (feeSubTypeDdl.SubTypeName == Constants.PerGallon)
                            {
                                feeSubTypeDdl.SubTypeName = Constants.PerLitre;
                            }
                        }
                        return feeSubTypeDdls;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "ChangeFeeSubTypesNameAsPerCountry", ex.Message, ex);

            }
            return feeSubTypeDdls;
        }
        public List<DropdownDisplayItem> GetCounterOfferFees(int feeType, bool isNoFeeRequired)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstFeeXFeeSubTypes
                            .Where(
                                t =>
                                t.FeeTypeId == feeType &&
                                (!isNoFeeRequired ? t.FeeSubTypeId != (int)FeeSubType.NoFee : true) &&
                                t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.MstFeeSubType.Id,
                                Name = t.MstFeeSubType.Name,
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCounterOfferFees", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetAddresses(int companyId)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            try
            {
                if (Context.DataContext.Companies.Any(t => t.Id == companyId && t.IsActive))
                {
                    response.Add(new DropdownDisplayItem { Id = 0, Name = Resource.lblAll });
                    Context.DataContext
                        .CompanyAddresses
                        .Where(t => t.CompanyId == companyId && t.IsActive)
                        .ToList()
                        .ForEach(t => response.Add(new DropdownDisplayItem
                        {
                            Id = t.Id,
                            Name = $"{t.Address} {t.City} {t.MstState.Code} {t.ZipCode}"
                        }));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAddresses", ex.Message, ex);
            }
            return response;
        }

        public UoM GetCompanyDefaultCurrency(int companyId)
        {
            UoM response = UoM.Gallons;
            try
            {
                if (companyId > 0)
                {
                    response = Context.DataContext
                        .CompanyAddresses
                        .Where(t => t.CompanyId == companyId && t.IsDefault && t.IsActive)
                        .Select(t => t.MstCountry.DefaultUoM)
                        .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCompanyDefaultCurrency", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetOrderCancelationReasons(int companyTypeId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (Context.DataContext.MstCompanyTypes.Any(t => t.Id == companyTypeId && t.IsActive))
                {
                    Context.DataContext
                        .MstOrderCancelationReasons
                        .Where(t => t.CompanyTypeId == companyTypeId && t.IsActive).ToList()
                        .ForEach(t => response.Add(new DropdownDisplayItem
                        {
                            Id = t.Id,
                            Name = t.Name
                        }));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetOrderCancelationReasons", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetMstScheduleTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                                            .MstDeliveryScheduleTypes
                                            .Where(t => t.IsActive)
                                            .Select(t => new DropdownDisplayItem
                                            {
                                                Id = t.Id,
                                                Name = t.Name
                                            }).OrderBy(x => x.Id).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMstScheduleTypes", ex.Message, ex);
            }
            return response;
        }

        public string GetName<TEnum>(int id) where TEnum : IConvertible
        {
            var response = string.Empty;

            var target = typeof(TEnum).Name;
            switch (target)
            {
                case nameof(BudgetCalculationType):
                    response = Context.DataContext.MstBudgetCalculationTypes.Single(t => t.Id == id).Name;
                    break;

                case nameof(DeliveryType):
                    response = Context.DataContext.MstDeliveryTypes.Single(t => t.Id == id).Name;
                    break;

                case nameof(FeeSubType):
                    response = Context.DataContext.MstFeeSubTypes.Single(t => t.Id == id).Name;
                    break;

                case nameof(FuelRequestStatus):
                    response = Context.DataContext.MstFuelRequestStatuses.Single(t => t.Id == id).Name;
                    break;

                case nameof(InvoiceStatus):
                    response = Context.DataContext.MstInvoiceStatuses.Single(t => t.Id == id).Name;
                    break;

                case nameof(OrderType):
                    response = Context.DataContext.MstOrderTypes.Single(t => t.Id == id).Name;
                    break;

                case nameof(OrderStatus):
                    response = Context.DataContext.MstOrderStatuses.Single(t => t.Id == id).Name;
                    break;

                case nameof(PaymentTerms):
                    response = Context.DataContext.MstPaymentTerms.Single(t => t.Id == id).Name;
                    break;

                case nameof(PricingType):
                    response = Context.DataContext.MstPricingTypes.Single(t => t.Id == id).Name;
                    break;

                case nameof(DeliveryScheduleType):
                    response = Context.DataContext.MstDeliveryScheduleTypes.Single(t => t.Id == id).Name;
                    break;

                case nameof(OtherProductTaxPricingType):
                    response = Context.DataContext.MstTaxPricingTypes.Single(t => t.Id == id).Name;
                    break;
            }

            return response;
        }


        public async Task<List<DropdownDisplayItem>> GetTPOSupplierCompanies()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var orders = Context.DataContext.Orders.Where(t => t.ExternalBrokerId.HasValue);
                response = await (from o in orders
                                  join com in Context.DataContext.Companies on o.AcceptedCompanyId equals com.Id
                                  select new DropdownDisplayItem() { Id = com.Id, Name = com.Name }).Distinct().ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetSupplierCompanies", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetSuperAdmins()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext.Users.Where(t => t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.SuperAdmin))
                           .Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.FirstName + " " + t.LastName }).OrderByDescending(t => t.Id).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetSuperAdmins", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetCarrierUserEmails(int assignedCarrierCompanyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext.Users.Where(t => t.CompanyId == assignedCarrierCompanyId
                                && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.Carrier || t1.Id == (int)UserRoles.Admin))
                           .Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.FirstName + " " + t.LastName + " (" + t.Email + ")" }).OrderByDescending(t => t.Id).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCarrierUserEmails", ex.Message, ex);
            }
            return response;
        }

        public DropdownDisplayWithSelectedItem GetCarrierAndSelectedUserEmails(int assignedCarrierCompanyId, int jobId)
        {
            var response = new DropdownDisplayWithSelectedItem();
            response.SelectedItems = new List<int>();
            try
            {
                response.Items = Context.DataContext.Users.Where(t => t.CompanyId == assignedCarrierCompanyId && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.Carrier || t1.Id == (int)UserRoles.Admin))
                           .Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.FirstName + " " + t.LastName + " (" + t.Email + ")" }).OrderByDescending(t => t.Id).ToList();

                if (jobId > 0)
                    response.SelectedItems = Context.DataContext.CarrierEmailSettings.Where(t => t.CarrierCompanyId == assignedCarrierCompanyId && t.JobId == jobId && t.IsActive).Select(t => t.UserId).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetSelectedCarrierUserEmails", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetTiers()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext.MstTierTypes.Where(t => t.IsActive)
                           .Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.Name }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetTiers", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetAccountSpecialistUsers(bool isAccountSpecialist = false, bool isActiveUsers = false, int userId = 0)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var users = Context.DataContext.Users.Where(t => t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.AccountSpecialist || t1.Id == (int)UserRoles.SuperAdmin)).ToList();

                if (isAccountSpecialist && isActiveUsers)
                {
                    users = users.Where(t => t.Id == userId).ToList();
                }

                if (!isAccountSpecialist && isActiveUsers)
                {
                    users = users.Where(t => t.IsActive).ToList();
                }

                response = users.Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.FirstName + " " + t.LastName }).OrderByDescending(t => t.Id).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAccountSpecialistUsers", ex.Message, ex);
            }
            return response.OrderBy(t => t.Name).ToList();
        }

        public async Task<List<AppSettingGridViewModel>> GetAppSettingsAsync()
        {
            var response = new List<AppSettingGridViewModel>();
            try
            {
                var appSettings = Context.DataContext.MstAppSettings.Where(t => t.IsActive && !t.Key.ToLower().Contains("template"));
                await appSettings.ForEachAsync(t => response.Add(new AppSettingGridViewModel
                {
                    Id = t.Id,
                    Key = t.Key,
                    Value = t.Value,
                    Description = t.Description,
                    UpdatedBy = Context.DataContext.Users.Where(t1 => t1.Id == t.UpdatedBy).Select(t1 => (t1.FirstName + " " + t1.LastName)).FirstOrDefault(),
                    UpdatedDate = t.UpdatedDate.ToString(@Resource.constFormatDate),
                    IsActive = t.IsActive
                }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAppSettingsAsync", ex.Message, ex);
            }
            return response;
        }

        public AppSettingViewModel GetAppSettings(int id)
        {
            var response = new AppSettingViewModel(Status.Success);
            try
            {
                var appSettings = Context.DataContext.MstAppSettings.SingleOrDefault(t => t.Id == id);
                if (appSettings != null)
                {
                    response = new AppSettingViewModel
                    {
                        Id = appSettings.Id,
                        Key = appSettings.Key,
                        Value = appSettings.Value,
                        Description = appSettings.Description,
                        UpdatedBy = appSettings.UpdatedBy,
                        UpdatedDate = appSettings.UpdatedDate,
                        IsActive = appSettings.IsActive
                    };
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAppSettings", ex.Message, ex);
            }
            return response;
        }
        public AppSettingViewModel GetAppSettings(string Key)
        {
            var response = new AppSettingViewModel(Status.Success);
            try
            {
                var appSettings = Context.DataContext.MstAppSettings.SingleOrDefault(t => t.Key.ToLower().Trim() == Key.ToLower().Trim());
                if (appSettings != null)
                {
                    response = new AppSettingViewModel
                    {
                        Id = appSettings.Id,
                        Key = appSettings.Key,
                        Value = appSettings.Value,
                        Description = appSettings.Description,
                        UpdatedBy = appSettings.UpdatedBy,
                        UpdatedDate = appSettings.UpdatedDate,
                        IsActive = appSettings.IsActive,
                        StatusCode = Status.Success,
                        StatusMessage = Status.Success.ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("MasterDomain", "GetAppSettings", ex.Message, ex);
            }
            return response;
        }
        public StatusViewModel SaveAppSettings(AppSettingViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var appSettings = Context.DataContext.MstAppSettings.SingleOrDefault(t => t.Id == viewModel.Id);
                if (appSettings != null)
                {
                    appSettings.Value = viewModel.Value;
                    appSettings.UpdatedBy = viewModel.UpdatedBy;
                    appSettings.UpdatedDate = viewModel.UpdatedDate;

                    Context.DataContext.Entry(appSettings).State = EntityState.Modified;
                    Context.Commit();

                    var appDomain = new ApplicationDomain(this);
                    appDomain.RemoveCacheForAppSettings(Constants.ApplicationSettingsValue);
                    appDomain.SetCacheForAppSettings(Constants.ApplicationSettingsValue);

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "SaveAppSettings", ex.Message, ex);
            }
            return response;
        }

        public StatusViewModel SaveAppSettingByKey(AppSettingViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var appSettings = Context.DataContext.MstAppSettings.FirstOrDefault(t => t.Key.ToLower() == viewModel.Key.ToLower() && t.IsActive);
                if (appSettings != null)
                {
                    appSettings.Value = viewModel.Value;
                    appSettings.UpdatedBy = viewModel.UpdatedBy;
                    appSettings.UpdatedDate = viewModel.UpdatedDate;

                    Context.DataContext.Entry(appSettings).State = EntityState.Modified;
                    Context.Commit();

                    var appDomain = new ApplicationDomain(this);
                    appDomain.RemoveCacheForAppSettings(Constants.ApplicationSettingsValue);
                    appDomain.SetCacheForAppSettings(Constants.ApplicationSettingsValue);

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "SaveAppSettingByKey", ex.Message, ex);
            }
            return response;
        }

        public NotificationSettingsViewModel GetDefaultNotificationSettings()
        {
            var response = new NotificationSettingsViewModel();
            try
            {
                var eventGroup = Context.DataContext.MstEventGroups.FirstOrDefault(t => t.IsActive);
                if (eventGroup != null)
                {
                    response.EventGroupIds.Add(eventGroup.Id);
                    response.EventGroupDetails.Add(new NotificationGroupViewModel
                    {
                        EventGroupName = eventGroup.Name,
                        EventDetails = GetEventsList(eventGroup.Id)
                    });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetDefaultNotificationSettings", ex.Message, ex);
            }
            return response;
        }

        public NotificationSettingsViewModel GetNotificationGroupDetails(string eventGroupId)
        {
            var response = new NotificationSettingsViewModel();
            try
            {
                var ids = getIntArrayFromJson(eventGroupId);
                if (ids != null)
                {
                    var eventGroup = Context.DataContext.MstEventGroups.Where(t => t.IsActive && ids.Contains(t.Id));
                    foreach (var group in eventGroup)
                    {
                        response.EventGroupIds.Add(group.Id);
                        response.EventGroupDetails.Add(new NotificationGroupViewModel
                        {
                            EventGroupName = group.Name,
                            EventDetails = GetEventsList(group.Id)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetNotificationGroupDetails", ex.Message, ex);
            }
            return response;
        }

        public StatusViewModel UpdateRolesForNotification(int createdBy, int companyTypeId, int eventTypeId, string roles, bool isEmail, bool isSms)
        {
            var response = new StatusViewModel();
            try
            {
                var roleIds = getIntArrayFromJson(roles);
                var definedRoles = getDefinedUserRoleNotification(companyTypeId, eventTypeId);
                if (roleIds.Count > definedRoles.Count)
                {
                    //When user selects first role for an event
                    if (roleIds.Count == 1 && definedRoles.Count == 0)
                    {
                        var eventType = Context.DataContext.MstEventTypes.FirstOrDefault(t => t.Id == eventTypeId);

                        if (eventType.NotificationType == (int)NotificationType.Email || eventType.NotificationType == (int)NotificationType.EmailAndSms)
                            isEmail = true;

                        if (eventType.NotificationType == (int)NotificationType.Sms || eventType.NotificationType == (int)NotificationType.EmailAndSms)
                            isSms = true;
                    }

                    var addedRole = roleIds.Except(definedRoles).FirstOrDefault();
                    response = UpdateUserRoleForEvent(createdBy, companyTypeId, eventTypeId, addedRole, isEmail, isSms);
                }
                else
                {
                    var removedRole = definedRoles.Except(roleIds).FirstOrDefault();
                    response = UpdateUserRoleForEvent(createdBy, companyTypeId, eventTypeId, removedRole, false, false);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "UpdateRolesForNotification", ex.Message, ex);
            }
            return response;
        }

        public StatusViewModel UpdateUserRolesForSelectedEvent(int createdBy, int companyTypeId, int eventTypeId, string roles, bool isEmail, bool isSms)
        {
            var response = new StatusViewModel();
            var roleIds = getIntArrayFromJson(roles);
            var userXRoles = Context.DataContext.MstCompanyUserRoleXEventTypes.Where(t =>
                                    (t.CompanyTypeId == companyTypeId || t.CompanyTypeId == (int)CompanyType.BuyerAndSupplier)
                                    && t.EventTypeId == eventTypeId && roleIds.Contains(t.RoleId)).ToList();

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (roleIds.Count > 0)
                    {
                        var notificationType = GetNotificationType(isEmail, isSms);
                        foreach (var item in userXRoles)
                        {
                            item.NotificationType = notificationType;
                        }

                        Context.Commit();
                        transaction.Commit();
                    }

                    var eventType = Context.DataContext.MstEventTypes.FirstOrDefault(t => t.Id == eventTypeId);
                    if (!isEmail && (eventType.NotificationType == (int)NotificationType.Email || eventType.NotificationType == (int)NotificationType.EmailAndSms))
                    {
                        var userNotifications = Context.DataContext.UserXNotificationSettings.Where(t =>
                                t.User.IsActive && t.EventTypeId == eventTypeId && t.IsEmail &&
                                t.User.Company.CompanyTypeId == companyTypeId && t.User.MstRoles.Any(t1 => roleIds.Contains(t1.Id))).ToList();
                        foreach (var userNotification in userNotifications)
                        {
                            userNotification.IsEmail = false;
                        }
                    }

                    if (!isSms && (eventType.NotificationType == (int)NotificationType.Sms || eventType.NotificationType == (int)NotificationType.EmailAndSms))
                    {
                        var userNotifications = Context.DataContext.UserXNotificationSettings.Where(t =>
                                t.User.IsActive && t.EventTypeId == eventTypeId && t.IsSMS &&
                                t.User.Company.CompanyTypeId == companyTypeId && t.User.MstRoles.Any(t1 => roleIds.Contains(t1.Id))).ToList();
                        foreach (var userNotification in userNotifications)
                        {
                            userNotification.IsSMS = false;
                        }
                    }

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessageNotificationUpdatedSuccessfully;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("MasterDomain", "UpdateUserRolesForSelectedEvent", ex.Message, ex);
                }
            }
            return response;
        }

        public List<NotificationTemplateViewModel> GetNotificationEventTemplate(int eventId, int notificationType)
        {
            var response = new List<NotificationTemplateViewModel>();
            try
            {
                var template = Context.DataContext.MstCompanyUserRoleXEventTypes.Where(t => t.EventTypeId == eventId && t.CompanyTypeId != (int)CompanyType.BuyerAndSupplier && t.Template != null).
                        Select(x => new { CompanyTypeId = x.CompanyTypeId, Template = x.Template }).Distinct();
                foreach (var item in template)
                {
                    var newTemplate = new TemplateViewModel() { NotificationType = notificationType };
                    response.Add(new NotificationTemplateViewModel { CompanyTypeId = item.CompanyTypeId, Template = item.Template.ToViewModel(newTemplate) });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetNotificationEventTemplate", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetMailboxQueryTypes()
        {

            var response = new List<DropdownDisplayItem>();
            try
            {
                response.Add(new DropdownDisplayItem { Id = 1, Name = "Order" });
                response.Add(new DropdownDisplayItem { Id = 2, Name = "Invoice" });
                response.Add(new DropdownDisplayItem { Id = 3, Name = "Digital Drop Ticket" });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMailboxQueryTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetAccountTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstAccountTypes
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAccountTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetCustomersForBrokeredOrder(int companyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .ExternalBrokers
                            .Where(t => t.IsActive && (t.SupplierCompanyId == null || t.SupplierCompanyId == companyId))
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.CompanyName
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCustomersForBrokeredOrder", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetThirdPartyNozzles()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext.MstThirdPartyNozzles.Where(t => t.IsActive).
                    Select(t => new DropdownDisplayItem
                    {
                        Id = t.Id,
                        Name = t.Name,
                    }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetThirdPartyNozzles", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetAllFeeTypes(int companyId, Currency currency = Currency.None, int truckLoadType = (int)TruckLoadTypes.LessTruckLoad)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = Context.DataContext.MstFeeTypes.Where(t => t.IsActive && t.TruckLoadCategoryId.HasValue && t.FeeCategoryId == (int)FeeCategory.CommonFee
                            &&
                            (
                                (truckLoadType == (int)TruckLoadTypes.FullTruckLoad &&
                                (
                                    t.TruckLoadCategoryId.Value == (int)TruckLoadFeeCategories.FTLAndLTL ||
                                    t.TruckLoadCategoryId.Value == (int)TruckLoadFeeCategories.OnlyFTL ||
                                    t.TruckLoadCategoryId.Value == (int)TruckLoadFeeCategories.FTLWaiver
                                )) ||
                                (truckLoadType == (int)TruckLoadTypes.LessTruckLoad &&
                                (
                                    t.TruckLoadCategoryId.Value == (int)TruckLoadFeeCategories.OnlyLTL || t.TruckLoadCategoryId.Value == (int)TruckLoadFeeCategories.FTLAndLTL
                                ))
                            )).
                           Select(t => new DropdownDisplayExtendedItem
                           {
                               Code = t.Id.ToString(),
                               Name = currency == Currency.CAD ? t.Name.Replace(Constants.Gallon, Constants.Litre) : t.Name
                           }).ToList();

                var otherFeeTypes = Context.DataContext.MstOtherFeeTypes.Where(t => t.IsActive && t.CompanyId == companyId).
                                    Select(t => new DropdownDisplayExtendedItem
                                    {
                                        Code = t.Code,
                                        Name = currency == Currency.CAD ? t.Name.Replace(Constants.Gallon, Constants.Litre) : t.Name,
                                    }).ToList();

                if (otherFeeTypes != null && otherFeeTypes.Any())
                    response.AddRange(otherFeeTypes);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAllFeeTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetFeeTypesAsync(int companyId, int orderId, Currency currency = Currency.None, bool isFromAccesorialFees = false)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                if (isFromAccesorialFees)
                {
                    response = Context.DataContext.MstFeeTypes.Where(t => t.IsActive
                           && t.TruckLoadCategoryId.HasValue && t.FeeCategoryId == (int)FeeCategory.CommonFee
                            && (t.Id != (int)FeeType.DeliveryFee) && (t.Id != (int)FeeType.FreightFee)
                            && (t.Id != (int)FeeType.UnderGallonFee) && (t.Id != (int)FeeType.SurchargeFee))
                           .Select(t => new DropdownDisplayExtendedItem
                           {
                               Code = t.Id.ToString(),
                               Name = currency == Currency.CAD ? t.Name.Replace(Constants.Gallon, Constants.Litre) : t.Name
                           }).ToList();
                }
                else
                {
                    response = Context.DataContext.MstFeeTypes.Where(t => t.IsActive
                          && t.TruckLoadCategoryId.HasValue && t.FeeCategoryId == (int)FeeCategory.CommonFee)
                          .Select(t => new DropdownDisplayExtendedItem
                          {
                              Code = t.Id.ToString(),
                              Name = currency == Currency.CAD ? t.Name.Replace(Constants.Gallon, Constants.Litre) : t.Name
                          }).ToList();
                }

                if (response.Any())
                {
                    if (orderId > 0)
                    {
                        Currency ordercurrency = Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => t.FuelRequest.Currency).FirstOrDefault();
                        if (ordercurrency == Currency.CAD)
                        {
                            response.Where(t => t.Code == "8").ToList().ForEach(t => { t.Name = Resource.lblMinlitreFee; });
                        }
                    }

                }
                var otherFeeTypes = Context.DataContext.MstOtherFeeTypes.Where(t => t.IsActive && t.CompanyId == companyId).
                                    Select(t => new DropdownDisplayExtendedItem
                                    {
                                        Code = t.Code,
                                        Name = currency == Currency.CAD ? t.Name.Replace(Constants.Gallon, Constants.Litre) : t.Name,
                                    }).ToList();

                if (otherFeeTypes != null && otherFeeTypes.Any())
                    response.AddRange(otherFeeTypes);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetFeeTypesAsync", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetTankRentalFees(int companyId)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = Context.DataContext.MstFeeTypes.Where(t => t.IsActive && t.FeeCategoryId == (int)FeeCategory.TankRental).
                           Select(t => new DropdownDisplayExtendedItem
                           {
                               Code = t.Id.ToString(),
                               Name = t.Name
                           }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetTankRentalFees", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetDiscountFeeTypes(int invoiceId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response.Add(new DropdownDisplayItem { Id = (int)FeeType.SubTotal, Name = Resource.lblDiscountOnSubTotal });

                var fuelRequestFees = Context.DataContext.FuelRequestFees
                                .Where(t => t.Invoices.Select(t1 => t1.Id).Contains(invoiceId)
                                && t.FeeSubTypeId != (int)FeeSubType.NoFee
                                && t.FeeTypeId != (int)FeeType.DryRunFee && t.FeeTypeId != (int)FeeType.ProcessingFee && t.DiscountLineItemId == null)
                                .Select(t => new
                                {
                                    t.MstFeeType.Name,
                                    t.FeeDetails,
                                    t.FeeTypeId,
                                    OtherName = t.MstOtherFeeType == null ? null : t.MstOtherFeeType.Name
                                });

                foreach (var item in fuelRequestFees)
                {
                    if (item.FeeTypeId == (int)FeeType.OtherFee)
                    {
                        var feeName = item.FeeDetails ?? item.OtherName;
                        response.Add(new DropdownDisplayItem { Id = item.FeeTypeId, Name = feeName });
                    }
                    else
                    {
                        response.Add(new DropdownDisplayItem { Id = item.FeeTypeId, Name = item.Name });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetDiscountFeeTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetDiscountFeeSubTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext.MstFeeSubTypes.Where(t => t.IsActive &&
                                    (t.Id == (int)FeeSubType.FlatFee /*|| t.Id == (int)FeeSubType.Percent*/))
                                    .Select(t => new DropdownDisplayItem
                                    {
                                        Id = t.Id,
                                        Name = t.Name.Replace(Resource.lblFlatFee, Resource.lblFlat)
                                    }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetDiscountFeeSubTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetAllMarginTypes()
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = Context.DataContext.MstMarginTypes.Where(t => t.IsActive && t.Id != (int)MarginType.SpecificAmount).
                           Select(t => new DropdownDisplayExtendedItem
                           {
                               Code = t.Id.ToString(),
                               Name = t.Description,
                           }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAllMarginTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetAllFeeSubTypes(string feeTypeId, Currency currency)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                int.TryParse(feeTypeId, out int id);
                if (id > 0 && id != (int)FeeType.OtherFee)
                {
                    response = Context.DataContext.MstFeeXFeeSubTypes.Include(t => t.MstFeeSubType)
                                        .Where(t => t.IsActive && t.FeeSubTypeId != (int)FeeSubType.NoFee && t.FeeTypeId == id && t.MstFeeSubType.IsActive)
                                        .Select(t => new DropdownDisplayItem
                                        {
                                            Id = t.MstFeeSubType.Id,
                                            Name = currency == Currency.CAD ? t.MstFeeSubType.Name.Replace(Constants.Gallon, Constants.Litre) : t.MstFeeSubType.Name
                                        }).ToList();
                }
                else
                {
                    if (currency == Currency.CAD)
                        response.Add(new DropdownDisplayItem { Id = 17, Name = Constants.PerLitre });
                    else
                        response.Add(new DropdownDisplayItem { Id = 17, Name = Constants.PerGallon });

                    response.Add(new DropdownDisplayItem { Id = 5, Name = Constants.PerHour });
                    response.Add(new DropdownDisplayItem { Id = 2, Name = Constants.FlatFee });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAllFeeSubTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetAllFeeConstraintTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext.MstFeeConstraintTypes.Where(t => t.IsActive)
                                    .Select(t => new DropdownDisplayItem
                                    {
                                        Id = t.Id,
                                        Name = t.Name
                                    }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAllFeeConstraintTypes", ex.Message, ex);
            }
            return response;
        }
        public List<DropdownDisplayExtendedItem> GetFeeTypesAsync(int companyId, Currency currency)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = Context.DataContext.MstFeeTypes.Where(t => t.IsActive
                            && t.TruckLoadCategoryId.HasValue && t.FeeCategoryId == (int)FeeCategory.CommonFee)
                            .Select(t => new DropdownDisplayExtendedItem
                            {
                                Code = t.Id.ToString(),
                                Name = currency == Currency.CAD ? t.Name.Replace(Constants.Gallon, Constants.Litre) : t.Name
                            }).ToList();
                if (response.Any() && currency == Currency.CAD)
                {
                    response.Where(t => t.Code == "8").ToList().ForEach(t => { t.Name = Resource.lblMinlitreFee; });
                }
                var otherFeeTypes = Context.DataContext.MstOtherFeeTypes.Where(t => t.IsActive && t.CompanyId == companyId).
                                    Select(t => new DropdownDisplayExtendedItem
                                    {
                                        Code = t.Code,
                                        Name = currency == Currency.CAD ? t.Name.Replace(Constants.Gallon, Constants.Litre) : t.Name,
                                    }).ToList();

                if (otherFeeTypes != null && otherFeeTypes.Any())
                    response.AddRange(otherFeeTypes);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetFeeTypesAsync", ex.Message, ex);
            }
            return response;
        }
        public List<int> GetFTLFees()
        {
            var response = new List<int>();
            try
            {
                response = Context.DataContext.MstFeeTypes.Where(t => t.IsActive &&
                (
                    t.TruckLoadCategoryId.Value == (int)TruckLoadFeeCategories.FTLAndLTL ||
                    t.TruckLoadCategoryId.Value == (int)TruckLoadFeeCategories.OnlyFTL ||
                    t.TruckLoadCategoryId.Value == (int)TruckLoadFeeCategories.FTLWaiver
                )
                ).Select(t => t.Id).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetFTLFees", ex.Message, ex);
            }
            return response;
        }

        public List<int> GetWaiverApplicableFees()
        {
            var response = new List<int>();
            try
            {
                response = Context.DataContext.MstFeeTypes.Where(t => t.IsActive &&
                            t.TruckLoadCategoryId.Value == (int)TruckLoadFeeCategories.FTLWaiver)
                            .Select(t => t.Id).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetWaiverApplicableFees", ex.Message, ex);
            }
            return response;
        }

        public List<CountryState> GetStatesOfAllCountries(int countryId = (int)Country.All)
        {
            var response = Context.DataContext.MstStates.Where(t => t.IsActive && (countryId == (int)Country.All || t.CountryId == countryId) && t.MstCountry.IsActive)
                .Select(t => new CountryState
                {
                    StateId = t.Id,
                    StateName = t.Name,
                    StateCode = t.Code,
                    CountryId = t.CountryId,
                    CountryGroupId = t.CountryGroupId ?? 0,
                    CountryCode = t.MstCountry.Code,
                    QuantityIndicatorId = t.QuantityIndicatorTypeId
                }).ToList();
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetCurrenyList()
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = Context.DataContext.MstCountries.Where(t => t.IsActive)
                                    .Select(t => new DropdownDisplayExtendedItem
                                    {
                                        Id = (int)t.Currency,
                                        Code = t.Currency.ToString(),
                                        Name = t.Currency.ToString()
                                    })
                                    .Distinct()
                                    .ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCurrenyList", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetUoMList()
        {

            List<DropdownDisplayExtendedItem> response = new List<DropdownDisplayExtendedItem>();
            try
            {
                foreach (var enumUOM in Enum.GetValues(typeof(UoM)))
                {
                    UoM type = (UoM)enumUOM;
                    if (type != UoM.None)
                    {
                        response.Add(new DropdownDisplayExtendedItem()
                        {
                            Id = (int)enumUOM,
                            Code = type.GetDisplayName(),
                            Name = type.GetDisplayName()
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetUoMList", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetEventsGroupList()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext.MstEventGroups.Where(t => t.IsActive)
                                    .Select(t => new DropdownDisplayItem
                                    {
                                        Id = t.Id,
                                        Name = t.Name
                                    }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetEventsList", ex.Message, ex);
            }
            return response;
        }

        public List<CompanyUserRoleEventViewModel> GetEventsList(int eventGroupId)
        {
            var response = new List<CompanyUserRoleEventViewModel>();
            try
            {
                response = Context.DataContext.MstEventTypes.Where(t => t.IsActive && t.EventGroupId == eventGroupId)
                                    .Select(t => new CompanyUserRoleEventViewModel()
                                    {
                                        Id = t.Id,
                                        Name = t.Name,
                                        BuyerUsers = t.MstCompanyUserRoleXEventTypes.Where(t1 => t1.CompanyTypeId == (int)CompanyType.Buyer && t1.NotificationType != (int)NotificationType.Nothing).Select(t2 => t2.RoleId).ToList(),
                                        SupplierUsers = t.MstCompanyUserRoleXEventTypes.Where(t1 => t1.CompanyTypeId == (int)CompanyType.Supplier && t1.NotificationType != (int)NotificationType.Nothing).Select(t2 => t2.RoleId).ToList(),
                                        IsForBuyerUsers = t.MstCompanyUserRoleXEventTypes.Any(t1 => t1.CompanyTypeId == (int)CompanyType.Buyer),
                                        IsForSupplierUsers = t.MstCompanyUserRoleXEventTypes.Any(t1 => t1.CompanyTypeId == (int)CompanyType.Supplier),
                                        IsEmail = t.MstCompanyUserRoleXEventTypes.Any(t1 => t1.NotificationType == (int)NotificationType.Email || t1.NotificationType == (int)NotificationType.EmailAndSms),
                                        IsSms = t.MstCompanyUserRoleXEventTypes.Any(t1 => t1.NotificationType == (int)NotificationType.Sms || t1.NotificationType == (int)NotificationType.EmailAndSms),
                                        IsEmailEnabled = (t.NotificationType == (int)NotificationType.Email || t.NotificationType == (int)NotificationType.EmailAndSms),
                                        IsSmsEnabled = (t.NotificationType == (int)NotificationType.Sms || t.NotificationType == (int)NotificationType.EmailAndSms)
                                    })
                                    .ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetEventsList", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<MobileFeeType>> GetMobileFeeTypes(int companyId, Currency currency = Currency.None)
        {
            var response = new List<MobileFeeType>();
            try
            {
                var feeTypes = await Context.DataContext.MstFeeTypes.Include("MstFeeXFeeSubTypes.MstFeeSubType")
                                .Where(t => t.IsActive && (t.FeeCategoryId == (int)FeeCategory.CommonFee || t.Id == (int)FeeType.OtherFee))
                                .Select(t => new MobileFeeType()
                                {
                                    Id = t.Id.ToString(),
                                    Name = currency == Currency.CAD ? t.Name.Replace(Constants.Gallon, Constants.Litre) : t.Name,
                                    TruckLoadTypeId = t.TruckLoadCategoryId,
                                    SubTypes = t.MstFeeXFeeSubTypes.Where(t1 => t1.FeeSubTypeId != (int)FeeSubType.NoFee)
                                    .Select(t1 => new MobileFeeSubType()
                                    {
                                        Id = t1.MstFeeSubType.Id,
                                        Name = t1.MstFeeSubType.Name
                                    })
                                }).ToListAsync();
                var otherFee = feeTypes.FirstOrDefault(t => t.Id == ((int)FeeType.OtherFee).ToString());
                response.AddRange(feeTypes.Where(t => t.Id != otherFee.Id));

                var otherfeeSubTypes = GetOtherFeeSubTypes();
                var otherAddedFeeTypes = await Context.DataContext.MstOtherFeeTypes
                                    .Where(t => t.IsActive && t.CompanyId == companyId)
                                    .Select(t => new MobileFeeType
                                    {
                                        Id = t.Code,
                                        Name = currency == Currency.CAD ? t.Name.Replace(Constants.Gallon, Constants.Litre) : t.Name,
                                        CommonFee = false
                                    }).ToListAsync();
                otherAddedFeeTypes.ForEach(t => t.SubTypes = otherfeeSubTypes);
                response.AddRange(otherAddedFeeTypes);

                otherFee.SubTypes = otherfeeSubTypes;
                otherFee.CommonFee = false;
                response.Add(otherFee);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMobileFeeTypes", ex.Message, ex);
            }
            return response;
        }

        private List<MobileFeeSubType> GetOtherFeeSubTypes(Currency currency = Currency.None)
        {
            return new List<MobileFeeSubType>()
                    {
                        new MobileFeeSubType { Id = 2, Name = Constants.FlatFee },
                        new MobileFeeSubType { Id = 5, Name = Constants.PerHour },
                        currency == Currency.CAD ?
                        new MobileFeeSubType { Id=17, Name = Constants.PerLitre }:
                        new MobileFeeSubType { Id=17, Name = Constants.PerGallon }
                    };
        }

        private List<int> getIntArrayFromJson(string jsonData)
        {
            return JsonConvert.DeserializeObject<List<int>>(jsonData);
        }

        private List<int> getDefinedUserRoleNotification(int companyTypeId, int eventTypeId)
        {
            var response = new List<int>();
            try
            {
                response = Context.DataContext.MstCompanyUserRoleXEventTypes.Where(t => t.CompanyTypeId == companyTypeId
                            && t.EventTypeId == eventTypeId && t.NotificationType != (int)NotificationType.Nothing).Select(t1 => t1.RoleId).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "getDefinedUserRoleNotification", ex.Message, ex);
            }
            return response;
        }

        private StatusViewModel UpdateUserRoleForEvent(int createdBy, int companyTypeId, int eventId, int roleId, bool isEmail, bool isSms)
        {
            var response = new StatusViewModel();
            try
            {
                var existingRole = Context.DataContext.MstCompanyUserRoleXEventTypes.Where(t => t.EventTypeId == eventId && t.RoleId == roleId);
                if (existingRole.Any(t => t.CompanyTypeId == companyTypeId))
                {
                    UpdateExistingNotificationEvent(companyTypeId, eventId, roleId, existingRole, isEmail, isSms);
                    // add in audit log
                    AddAuditLog(createdBy, companyTypeId, eventId, roleId, AuditEventType.Update, isEmail, isSms);
                }
                else
                {
                    var templateId = Context.DataContext.MstCompanyUserRoleXEventTypes.FirstOrDefault(t => t.CompanyTypeId == companyTypeId && t.EventTypeId == eventId && t.TemplateId.HasValue)?.TemplateId;
                    // assign new template
                    if (templateId.HasValue)
                    {
                        AddNewNotificationForRole(companyTypeId, eventId, roleId, templateId, isEmail, isSms);

                        if (!existingRole.Any(t => t.CompanyTypeId == (int)CompanyType.BuyerAndSupplier))
                        {
                            AddNewNotificationForRole((int)CompanyType.BuyerAndSupplier, eventId, roleId, templateId, isEmail, isSms);
                        }
                        // add in audit log
                        AddAuditLog(createdBy, companyTypeId, eventId, roleId, AuditEventType.Insert, isEmail, isSms);
                    }
                    else
                    {
                        response.StatusMessage = Resource.errMessageTemplateNotPresent;
                        return response;
                    }
                }
                Context.Commit();
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMessageNotificationUpdatedSuccessfully;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "UpdateUserRoleForEvent", ex.Message, ex);
            }
            return response;
        }

        private void UpdateExistingNotificationEvent(int companyTypeId, int eventId, int roleId, IQueryable<MstCompanyUserRoleXEventType> existingRole, bool isEmail, bool isSms)
        {
            var isSameRoleExistForAnotherCompany = existingRole.Any(t => t.CompanyTypeId != companyTypeId && t.CompanyTypeId != (int)CompanyType.BuyerAndSupplier);
            foreach (var role in existingRole.Where(t => t.CompanyTypeId == companyTypeId || t.CompanyTypeId == (int)CompanyType.BuyerAndSupplier))
            {
                // is same role exist for another company
                if (role.CompanyTypeId == (int)CompanyType.BuyerAndSupplier && isSameRoleExistForAnotherCompany)
                {
                    continue;
                }
                else
                {
                    role.NotificationType = GetNotificationType(isEmail, isSms);
                    role.UpdatedDate = DateTimeOffset.Now;

                    var eventType = Context.DataContext.MstEventTypes.FirstOrDefault(t => t.Id == eventId);
                    if (!isEmail && (eventType.NotificationType == (int)NotificationType.Email || eventType.NotificationType == (int)NotificationType.EmailAndSms))
                    {
                        var userNotifications = Context.DataContext.UserXNotificationSettings.Where(t =>
                                t.User.IsActive && t.EventTypeId == eventId && t.IsEmail &&
                                t.User.Company.CompanyTypeId == companyTypeId && t.User.MstRoles.Any(t1 => t1.Id == roleId)).ToList();
                        foreach (var userNotification in userNotifications)
                        {
                            userNotification.IsEmail = false;
                        }
                    }

                    if (!isSms && (eventType.NotificationType == (int)NotificationType.Sms || eventType.NotificationType == (int)NotificationType.EmailAndSms))
                    {
                        var userNotifications = Context.DataContext.UserXNotificationSettings.Where(t =>
                                t.User.IsActive && t.EventTypeId == eventId && t.IsSMS &&
                                t.User.Company.CompanyTypeId == companyTypeId && t.User.MstRoles.Any(t1 => t1.Id == roleId)).ToList();
                        foreach (var userNotification in userNotifications)
                        {
                            userNotification.IsSMS = false;
                        }
                    }

                    Context.DataContext.Entry(role).State = EntityState.Modified;
                }
            }
            if (!existingRole.Any(t => t.CompanyTypeId == (int)CompanyType.BuyerAndSupplier))
            {
                var templateId = existingRole.FirstOrDefault(t => t.CompanyTypeId == companyTypeId).TemplateId;
                AddNewNotificationForRole(companyTypeId, eventId, roleId, templateId, isEmail, isSms);
            }
        }

        private int GetNotificationType(bool isEmail, bool isSms)
        {
            int notificationType = (int)NotificationType.Nothing;
            if (isEmail && isSms)
                notificationType = (int)NotificationType.EmailAndSms;
            else if (isEmail)
                notificationType = (int)NotificationType.Email;
            else if (isSms)
                notificationType = (int)NotificationType.Sms;
            return notificationType;
        }

        private void AddNewNotificationForRole(int companyTypeId, int eventId, int roleId, int? templateId, bool isEmail, bool isSms)
        {
            var newRole = new MstCompanyUserRoleXEventType()
            {
                CompanyTypeId = companyTypeId,
                EventTypeId = eventId,
                RoleId = roleId,
                TemplateId = templateId,
                UpdatedBy = (int)SystemUser.System,
                UpdatedDate = DateTimeOffset.Now,
                NotificationType = GetNotificationType(isEmail, isSms)
            };
            Context.DataContext.MstCompanyUserRoleXEventTypes.Add(newRole);
        }
        public List<DropdownDisplayItem> GetAllSupplierProducts(int companyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {

                response = Context.DataContext
                            .CompanyAddresses
                            .Where(t => t.CompanyId == companyId)
                            .SelectMany(t => t.MstProductTypes)
                            .Distinct()
                            .Where(t => t.Id != (int)ProductTypes.NonStandardFuel)
                            .SelectMany(p => p.MstTfxProducts)
                            .OrderBy(p => p.Id)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetSupplierProducts", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetSupplierProducts(int companyId, int countryId, int sourceId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (sourceId == (int)PricingSource.Axxis)
                {
                    if (countryId == (int)Country.USA)
                    {
                        response = Context.DataContext
                                    .CompanyAddresses
                                    .Where(t => t.CompanyId == companyId)
                                    .SelectMany(t => t.MstProductTypes)
                                    .Distinct()
                                    .Where(t => t.Id != (int)ProductTypes.NonStandardFuel)
                                    .SelectMany(p => p.MstProducts)
                                    .Where(p => p.Id < 143)  //NEED TO REMOVE THIS WHEN WE SUPPORT CANADA FOR OFFER
                                    .OrderBy(p => p.Id)
                                    .Select(t => new DropdownDisplayItem
                                    {
                                        Id = t.Id,
                                        Name = t.Name
                                    }).ToList();
                    }
                    else
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        response = Task.Run(() => spDomain.GetAllProductsForCountry((int)Country.CAN)).Result;
                    }
                }
                else
                {
                    var spDomain = new StoredProcedureDomain(this);
                    response = spDomain.GetSourceBasedProducts((PricingSource)sourceId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetSupplierProducts", ex.Message, ex);
            }
            return response;
        }


        public List<DropdownDisplayItem> GetSupplierCustomers(int companyId, int countryId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var buyerCompanies = Context.DataContext
                            .Orders
                            .Where(t => t.AcceptedCompanyId == companyId)
                            .Select(t => t.BuyerCompanyId)
                            .Distinct()
                            .ToList();

                response = Context.DataContext.Companies
                            .Where(t => buyerCompanies.Contains(t.Id) && t.IsActive && t.CompanyAddresses.Any(t1 => t1.CountryId == countryId && t1.IsActive)
                                    && (t.CompanyTypeId == (int)CompanyType.Buyer || t.CompanyTypeId == (int)CompanyType.BuyerAndSupplier))
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetSupplierCustomers", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetSupplierCustomers(int companyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var buyerCompanies = Context.DataContext
                            .Orders
                            .Where(t => t.AcceptedCompanyId == companyId && (t.BuyerCompany.IsActive ||
                            t.BuyerCompany.Users.Any(t1 => t1.OnboardedTypeId == (int)OnboardedType.ThirdPartyOrderOnboarded)))
                            .Select(t => t.BuyerCompanyId)
                            .Distinct()
                            .ToList();

                response = Context.DataContext.Companies
                            .Where(t => buyerCompanies.Contains(t.Id)).OrderBy(t => t.Name)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetSupplierCustomers", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetSupplierCustomers(int companyId, List<int> tiers)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var buyerCompanies = Context.DataContext
                                    .Orders
                                    .Where(t => t.AcceptedCompanyId == companyId)
                                    .Select(t => t.BuyerCompanyId)
                                    .Distinct()
                                    .ToList();

                List<int> tierMappings = new List<int>();
                if (tiers != null)
                    tierMappings = Context.DataContext.OfferTierMappings
                                    .Where(t => t.SupplierCompanyId == companyId && tiers.Contains(t.TierId) && t.IsActive)
                                    .Select(t => t.BuyerCompanyId).Distinct().ToList();

                response = await Context.DataContext.Companies
                            .Where(t => buyerCompanies.Contains(t.Id) && t.IsActive && !tierMappings.Contains(t.Id)
                                    && (t.CompanyTypeId == (int)CompanyType.Buyer || t.CompanyTypeId == (int)CompanyType.BuyerAndSupplier))
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetSupplierCustomers", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetTierTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstTierTypes
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetTierTypes", ex.Message, ex);
            }
            return response;
        }

        private static void AddAuditLog(int createdBy, int companyTypeId, int eventId, int roleId, AuditEventType auditEvent, bool isEmail, bool isSms)
        {
            AuditLogger.AddAuditLog(new UserContext() { Id = createdBy }, new AuditLogViewModel()
            {
                Message = $"Update Email for EventId:{eventId}, CompanyId:{companyTypeId}, RoleId:{roleId} to {isEmail}" +
                          $" and SMS for EventId:{eventId}, CompanyId:{companyTypeId}, RoleId:{roleId} to {isSms}",
                CallSite = "MasterDomain : UpdateUserRoleForEvent",
                AuditEventType = auditEvent.ToString(),
                AuditEntityType = AuditEntityType.Notification.ToString()
            });
        }

        public List<DropdownDisplayItem> GetCompanyTypesForGroup()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstCompanyTypes
                            .Where(t => t.IsActive && (t.Id == (int)CompanyType.Buyer || t.Id == (int)CompanyType.Supplier))
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();

                response.Insert(0, new DropdownDisplayItem() { Id = 0, Name = "Select Country Group" });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCompanyTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetCompanyGroupList(int companyId, CompanyType companySubType)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .CompanyGroups.Include(t => t.CompanyGroupXCompanies)
                            .Where(t => t.IsActive && t.OwnerCompanyId == companyId && t.CompanyGroupXCompanies.Any() && (companySubType == CompanyType.Unknown || (int)t.CompanyGroupTypeId == (int)companySubType))
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.GroupName
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCompanyGroupList", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetSupplierOrdersForBilling(int buyerCompanyId, int supplierCompanyId, int countyId, int billingScheduleId = 0)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var last30dayOrders = DateTimeOffset.Now.AddDays(-30);
                response = Context.DataContext.Orders.Where(t => t.IsActive && t.BuyerCompanyId == buyerCompanyId && t.AcceptedCompanyId == supplierCompanyId
                                    && t.FuelRequest.Job.CountryId == countyId
                                    && t.OrderXStatuses.Any(o => o.StatusId == (int)OrderStatus.Open && o.IsActive)
                                    && !t.BillingScheduleXCustomerOrders.Any(b => b.BillingSchedule.IsActive && b.IsActive)
                                    || (t.BuyerCompanyId == buyerCompanyId && t.AcceptedCompanyId == supplierCompanyId
                                            && t.FuelRequest.Job.CountryId == countyId
                                            && t.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery
                                            && !t.Invoices.Any()
                                            && t.OrderXStatuses.Any(o => o.StatusId == (int)OrderStatus.Closed && o.UpdatedDate >= last30dayOrders)))
                                .Select(t => new DropdownDisplayItem
                                {
                                    Id = t.Id,
                                    Name = t.PoNumber
                                }).ToList();

                if (billingScheduleId > 0)
                {
                    var existingOrders = Context.DataContext.Orders.Where(t => t.BillingScheduleXCustomerOrders.Any(b => b.BillingScheduleId == billingScheduleId))
                                .Select(t => new DropdownDisplayItem
                                {
                                    Id = t.Id,
                                    Name = t.PoNumber
                                }).ToList();
                    response.AddRange(existingOrders);
                }

                response = response.OrderBy(t => t.Name).ToList();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetSupplierOrdersForBilling", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetFrequency()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext.MstFrequencyTypes.Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetFrequency", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetSupplierCustomersForBilling(int companyId, int countryId, int customerId = 0)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (customerId > 0)
                {
                    response = Context.DataContext.Companies
                                .Where(t => t.Id == customerId)
                                .Select(t => new DropdownDisplayItem
                                {
                                    Id = t.Id,
                                    Name = t.Name
                                }).ToList();
                }
                else
                {
                    var buyerCompanies = Context.DataContext
                                .Orders
                                .Where(t => t.AcceptedCompanyId == companyId && t.OrderXStatuses.Any(o => o.StatusId == (int)OrderStatus.Open && o.IsActive)
                                        && !t.BillingScheduleXCustomerOrders.Any(o => o.IsActive))
                                .Select(t => t.BuyerCompanyId)
                                .Distinct()
                                .ToList();

                    response = Context.DataContext.Companies
                                .Where(t => buyerCompanies.Contains(t.Id) //&& t.IsActive - commented to get TPO customers
                                        && t.CompanyAddresses.Any(a => a.CountryId == countryId))
                                .Select(t => new DropdownDisplayItem
                                {
                                    Id = t.Id,
                                    Name = t.Name
                                }).OrderBy(t => t.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetSupplierCustomersForBilling", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetStatementTimeZone(int companyId, int countryId)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                var contryCode = countryId == (int)Country.USA ? Constants.TimeZoneCountryUS : Constants.TimeZoneCountryCanada;
                var timezones = TimeZoneInfo.GetSystemTimeZones().ToList().Where(zone => zone.DisplayName.Contains(contryCode));
                foreach (var item in timezones)
                {
                    response.Add(new DropdownDisplayExtendedItem() { Code = item.Id, Name = item.StandardName });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetStatementTimeZone", ex.Message, ex);
            }
            return response;
        }
        public List<DropdownDisplayItem> UpdateFrequencyTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response.Add(new DropdownDisplayItem() { Id = 1, Name = "Hours" });
                response.Add(new DropdownDisplayItem() { Id = 2, Name = "Days" });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "UpdateFrequencyTypes", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetCustomersForStatements(int companyId, int countryId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext.BillingStatements
                               .Where(t => t.IsActive && t.IsGenerated && t.CreatedCompany == companyId && t.BillingStatementXInvoices.Any(t1 => t1.Invoice.Order.FuelRequest.Job.CountryId == countryId))
                               .Select(t => new DropdownDisplayItem
                               {
                                   Id = t.BillingStatementXInvoices.Select(t1 => t1.Invoice.Order.BuyerCompanyId).FirstOrDefault(),
                                   Name = t.BillingStatementXInvoices.Select(t1 => t1.Invoice.Order.BuyerCompany.Name).FirstOrDefault()
                               }).OrderBy(t => t.Name).Distinct().ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCustomersForStatements", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetScheduleIdForStatements(int companyId, int countryId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext.BillingStatements
                               .Where(t => t.IsActive && t.IsGenerated && t.BillingSchedule.CreatedByCompanyId == companyId && t.BillingSchedule.CountryId == countryId)
                               .Select(t => new DropdownDisplayItem
                               {
                                   Id = t.BillingSchedule.Id,
                                   Name = t.BillingSchedule.BillingStatementId
                               }).OrderBy(t => t.Name).Distinct().ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetScheduleIdForStatements", ex.Message, ex);
            }
            return response;
        }

        public List<DateTime> GetHolidayList(string appSettingKey)
        {
            List<DateTime> holidayDates = new List<DateTime>();
            try
            {
                var appDomain = new ApplicationDomain();
                //if its not sunday or public holiday, send email to exchange management about price update fail
                var publicHolidayList = appDomain.GetKeySettingValue(appSettingKey, string.Empty);
                holidayDates = GetDatelistFromString(holidayDates, publicHolidayList);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetHolidayList", ex.Message, ex);
            }
            return holidayDates;
        }

        public List<DropdownDisplayExtendedItem> GetMstProducts()
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = Context.DataContext
                            .MstProducts
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayExtendedItem
                            {
                                Id = t.Id,
                                Name = t.Name,
                                Code = t.ProductTypeId.ToString()
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMstProducts", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetMstProducts(int pricingSourceId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstProducts
                            .Where(t => t.IsActive && t.PricingSourceId == pricingSourceId && t.ProductTypeId != (int)ProductTypes.NonStandardFuel)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name,
                            }).OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMstProducts", ex.Message, ex);
            }
            return response;
        }

        /// <summary>
        /// Gets the MST opis products.
        /// </summary>
        /// <returns><see cref="List{DropdownDisplayExtendedItem}"/>.</returns>
        public List<DropdownDisplayExtendedItem> GetMstOPISProducts()
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = Context.DataContext
                            .MstOPISProducts
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayExtendedItem
                            {
                                Id = t.Id,
                                Name = t.Name,
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMstOPISProducts", ex.Message, ex);
            }
            return response;
        }

        /// <summary>
        /// Gets the MST opis products.
        /// </summary>
        /// <param name="pricingSourceId">The pricing source id.</param>
        /// <returns><see cref="List{DropdownDisplayItem}"/>.</returns>
        public List<DropdownDisplayItem> GetMstOPISProducts(int pricingSourceId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstOPISProducts
                            .Where(t => t.IsActive && t.PricingSourceId == pricingSourceId)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name,
                            }).OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMstOPISProducts", ex.Message, ex);
            }
            return response;
        }

        private static List<DateTime> GetDatelistFromString(List<DateTime> holidayDates, string publicHolidayList)
        {
            List<string> holidays = new List<string>();
            if (publicHolidayList != null)
            {
                holidays = publicHolidayList.TrimEnd(';').Split(';').ToList();
                holidayDates = holidays.Select(date => DateTime.Parse(date)).ToList();
            }

            return holidayDates;
        }
        public List<string> GetRoleName(IList<int> roleId)
        {
            List<string> roleName = new List<string>();
            try
            {
                roleName = Context.DataContext
                            .MstRoles
                            .Where(t => roleId.Contains(t.Id) && t.IsActive).Select(t => t.Name).ToList();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetRoleName", ex.Message, ex);
            }
            return roleName;
        }

        public async Task<object> GetMasterDataForFreightTable()
        {
            try
            {
                var productTypes = await Context.DataContext.MstProductTypes
                                    .Select(t => new { ProductTypeId = t.Id, ProductTypeName = t.Name }).ToListAsync();
                var fuelTypes = await Context.DataContext.MstTfxProducts
                                    .Select(t => new { FuelTypeId = t.Id, FuelTypeName = t.Name, t.ProductTypeId, ProductTypeName = t.MstProductType.Name }).ToListAsync();
                var countries = await Context.DataContext.MstCountries
                                .Select(t => new { CountryId = t.Id, CountryName = t.Name, CountryCode = t.Code }).ToListAsync();
                return new { productTypes, fuelTypes, countries };
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMasterDataForFreightTable", ex.Message, ex);
            }
            return null;
        }

        public List<DropdownDisplayItem> GetFuelTypes(int productTypeId = 1)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response.AddRange(Context.DataContext
                                .MstTfxProducts
                                .Where(t => t.IsActive && t.ProductDisplayGroupId != (int)ProductDisplayGroups.OtherFuelType && t.ProductTypeId == productTypeId)
                                .Select(t => new DropdownDisplayItem
                                {
                                    Id = t.Id,
                                    Name = t.Name
                                }).ToList()
                                );
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetFuelTypes", ex.Message, ex);
            }
            return response;
        }

        public object GetTerminals(int countryId, int stateId)
        {
            try
            {
                var country = ((Country)countryId).ToString();

                return Context.DataContext
                                .MstExternalTerminals
                                .Where(t => t.IsActive && (t.StateId == stateId || stateId == 0)
                                && (t.CountryCode.ToLower() == country.ToLower() || country.ToLower() == Country.All.ToString().ToLower())
                                && t.ZipCode != "00001" && t.ZipCode != "00002" && t.ZipCode != "00003")
                                .Select(t => new
                                {
                                    t.Id,
                                    t.Name,
                                    t.Latitude,
                                    t.Longitude,
                                    t.PricingSourceId,
                                    t.ZipCode,
                                    t.StateId,
                                    CountryId = t.CountryCode,
                                    Address = t.Address + ", " + t.City + ", " + t.StateCode + ", " + t.ZipCode
                                }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetTerminals", ex.Message, ex);
            }
            return null;
        }
        public List<DropdownDisplayItem> GetCities(int stateId = 0)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var states = Context.DataContext.MstCities.Where(t => t.StateId == stateId);
                response = states
                            .Where(t => t.IsActive)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCities", ex.Message, ex);
            }
            return response;
        }
        public List<DropdownDisplayItem> GetProductTypes()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext.MstProductTypes
                                    .Where(t => t.IsActive).
                                    OrderBy(x => x.Name)
                                    .Select(t => new DropdownDisplayItem
                                    {
                                        Id = t.Id,
                                        Name = t.Name
                                    }).ToList();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetProductTypes", ex.Message, ex);
            }
            return response;
        }



        public int GetDefaultServingCountry(int companyId)
        {
            int response = 0;
            try
            {
                response = Context.DataContext.CompanyAddresses.Where(t => t.CompanyId == companyId && t.IsDefault).Select(t => t.CountryId).FirstOrDefault();
            }

            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetDefaultServingCountries", ex.Message, ex);
            }
            return response;
        }

        public Currency GetDefaultCurrencyForCompany(int companyId)
        {
            var response = new Currency();
            try
            {
                response = Context.DataContext.CompanyAddresses.Where(t => t.CompanyId == companyId && t.IsDefault).Select(t => t.MstCountry.Currency).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("MasterDomain", "GetDefaultCurrencyForCompany", ex.Message, ex);
            }
            return response;

        }

        public UoM GetDefaultUoMforCompany(int companyId)
        {
            var response = new UoM();

            try
            {
                response = Context.DataContext.CompanyAddresses.Where(t => t.CompanyId == companyId && t.IsDefault).Select(t => t.MstCountry.DefaultUoM).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("MasterDomain", "GetDefaultUoMforCompany", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<int>> GetCustomersWithSupplierTolerance(int companyId, CompanyType companyType, decimal threshold)
        {
            var response = new List<int>();
            try
            {
                var masterDomain = new MasterDomain();
                var customerList = await masterDomain.GetYourCustomersForDipTest(companyId, companyType);
                if (customerList != null && customerList.Any())
                {
                    var buyers = customerList.Select(t => t.Id).Distinct().ToList();
                    response.AddRange(buyers);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCustomersWithSupplierTolerance", ex.Message, ex);
            }
            return response;
        }

        public async Task<int> GetMissingScheduleWaitingPeriod()
        {
            int waitingPeriod = 0;
            try
            {
                var waitingPeriodToMissSchedule = await Context.DataContext.MstAppSettings.Where(t => t.Key == "MissedScheduleWaitingPeriod").Select(t => t.Value).FirstOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(waitingPeriodToMissSchedule))
                {
                    waitingPeriod = Convert.ToInt32(waitingPeriodToMissSchedule);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMissingScheduleWaitingPeriod", ex.Message, ex);
            }
            return waitingPeriod;
        }

        public async Task<int> GetOrderCloseWaitingPeriod()
        {
            int waitingPeriod = 0;
            try
            {
                var waitingPeriodToCloseOrder = await Context.DataContext.MstAppSettings.Where(t => t.Key == "OrderCloseWaitingPeriod").Select(t => t.Value).FirstOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(waitingPeriodToCloseOrder))
                {
                    waitingPeriod = Convert.ToInt32(waitingPeriodToCloseOrder);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetOrderCloseWaitingPeriod", ex.Message, ex);
            }
            return waitingPeriod;
        }

        public List<DropdownDisplayItem> GetMstProductsDropDownListForLFVBol(int pricingSourceId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstProducts
                            .Where(t => t.IsActive && t.PricingSourceId == pricingSourceId && t.ProductTypeId != (int)ProductTypes.NonStandardFuel)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.DisplayName != string.Empty && t.DisplayName != null ? t.DisplayName : t.Name,
                            }).OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMstProductsDropDownListForLFVBol", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetTankScaleMeasurementList(int assetType, int Uom, string tankMakeModel)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (assetType == (int)AssetType.Asset)
                {
                    if (Uom == (int)UoM.Gallons)
                    {
                        var result = new DropdownDisplayItem();
                        result.Id = (int)TankScaleMeasurement.Gallons;
                        result.Name = TankScaleMeasurement.Gallons.GetDisplayName();
                        response.Add(result);
                    }
                    else if (Uom == (int)UoM.Litres)
                    {
                        var result = new DropdownDisplayItem();
                        result.Id = (int)TankScaleMeasurement.Litres;
                        result.Name = TankScaleMeasurement.Litres.GetDisplayName();
                        response.Add(result);
                    }
                }
                else if (assetType == (int)AssetType.Tank)
                {
                    if (string.IsNullOrWhiteSpace(tankMakeModel))
                    {
                        if (Uom == (int)UoM.Gallons)
                        {
                            var result = new DropdownDisplayItem();
                            result.Id = (int)TankScaleMeasurement.Gallons;
                            result.Name = TankScaleMeasurement.Gallons.GetDisplayName();
                            response.Add(result);
                        }
                        else if (Uom == (int)UoM.Litres)
                        {
                            var result = new DropdownDisplayItem();
                            result.Id = (int)TankScaleMeasurement.Litres;
                            result.Name = TankScaleMeasurement.Litres.GetDisplayName();
                            response.Add(result);
                        }
                    }
                    else
                    {
                        var cm = new DropdownDisplayItem();
                        cm.Id = (int)TankScaleMeasurement.Cm;
                        cm.Name = TankScaleMeasurement.Cm.GetDisplayName();
                        response.Add(cm);
                        var inches = new DropdownDisplayItem();
                        inches.Id = (int)TankScaleMeasurement.Inches;
                        inches.Name = TankScaleMeasurement.Inches.GetDisplayName();
                        response.Add(inches);
                        var gallons = new DropdownDisplayItem();
                        gallons.Id = Uom == (int)UoM.Gallons ? (int)TankScaleMeasurement.Gallons : (int)TankScaleMeasurement.Litres;
                        gallons.Name = Uom == (int)UoM.Gallons ? TankScaleMeasurement.Gallons.GetDisplayName() : TankScaleMeasurement.Litres.GetDisplayName();
                        response.Add(gallons);
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetTankScaleMeasurementList", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetTFXFuelTypeByProductTypeId(int productTypeId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                            .MstTfxProducts
                            .Where(t => t.IsActive && t.ProductTypeId == productTypeId)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Name
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetTFXFuelTypeByProductTypeId", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetTerminalSupplierList(int countryId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var termSuppliers = await Context.DataContext.MstTerminalSuppliers.Where(t => t.IsActive && t.CountryId == (Country)countryId).Select(t => new { t.Id, t.Name }).ToListAsync();
                if (termSuppliers != null && termSuppliers.Any())
                {
                    termSuppliers.ForEach(t => response.Add(new DropdownDisplayItem { Id = t.Id, Name = t.Name }));
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetTerminalSupplierList", ex.Message, ex);

            }
            return response;

        }

        public async Task<List<DropdownDisplayItem>> GetAssignedTerminalIdsForMapping(UserContext userContext)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
                response = await storedProcedureDomain.GetAssignedTerminalIds(userContext.CompanyId);

            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("MasterDomain", "GetAssignedTerminalIdsForMapping", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetAllProductsWithAdditives(int companyId)
        {
            //Remove 'Regular Gas','Premium Gas','Midgrade Gas' MstProducts
            var olderProductIds = new List<int> { 14, 15, 16 };
            var response = new List<DropdownDisplayItem>();
            try
            {
                response.AddRange(Context.DataContext
                               .MstProducts
                               .Where(t => t.IsActive && t.ProductDisplayGroupId != (int)ProductDisplayGroups.OtherFuelType
                               && (t.CompanyId == null || t.CompanyId == companyId) && !olderProductIds.Contains(t.MstProductType.Id))
                               .Select(t => new DropdownDisplayItem
                               {
                                   Id = t.Id,
                                   Name = !(t.DisplayName == null || t.DisplayName == string.Empty) ? t.DisplayName : t.Name
                               }).ToList()
                               );
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAllProductsWithAdditives", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DropdownDisplayExtendedItem>> GetMstFuelProducts()
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = await Context.DataContext
                            .MstTfxProducts
                            .Where(t => t.IsActive
                                        && t.ProductDisplayGroupId != (int)ProductDisplayGroups.OtherFuelType
                                        && t.ProductDisplayGroupId != (int)ProductDisplayGroups.AdditiveFuelType)
                            .Select(t => new DropdownDisplayExtendedItem
                            {
                                Id = t.Id,
                                Name = t.Name,
                                Code = t.ProductTypeId.ToString()
                            }).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMstFuelProducts", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetCustomerJobs(List<int> customerIds, bool IsMarine, int supplierCompanyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (customerIds != null)
                {
                    response = await Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == supplierCompanyId
                                              && (!customerIds.Any() || customerIds.Contains(t.BuyerCompanyId))
                                              && t.IsActive && t.FuelRequest.Job.IsMarine == IsMarine
                                              && t.OrderXStatuses.FirstOrDefault().StatusId == (int)OrderStatus.Open)
                                             .Select(t => new DropdownDisplayItem()
                                             {
                                                 Id = t.FuelRequest.Job.Id,
                                                 Name = t.FuelRequest.Job.Name,
                                             }).GroupBy(t => t.Id).Select(t => t.FirstOrDefault()).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCustomerJobs", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DropdownDisplayExtendedProperty>> GetAssetAndTankForOrders(List<int> jobIds)
        {
            List<DropdownDisplayExtendedProperty> response = new List<DropdownDisplayExtendedProperty>();
            try
            {
                if (jobIds != null && jobIds.Any())
                {
                    response = await Context.DataContext.JobXAssets.Where(t => jobIds.Contains(t.JobId)
                                         && t.RemovedBy == null && t.RemovedDate == null)
                                            .Select(t => new DropdownDisplayExtendedProperty
                                            {
                                                Id = t.Asset.Id,
                                                Name = t.Asset.Name,
                                                CodeId = t.Asset.Type,
                                                IsTrue = t.Asset.IsMarine
                                            }).GroupBy(t => t.Id).Select(t => t.FirstOrDefault()).ToListAsync();


                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAssetAndTankForOrders", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveTerminalDetails(PickupLocationDetailViewModel viewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                //update terminal
                if (!ValidateTerminal(viewModel, response))
                {
                    viewModel.UpdatedBy = userContext.Id;
                    viewModel.StateCode = Context.DataContext.MstStates.First(t => t.Id == viewModel.StateId).Code;
                    viewModel.CountryCode = Context.DataContext.MstCountries.First(t => t.Id == viewModel.CountryId).Code;

                    var orginalTerminaldetails = Context.DataContext.MstExternalTerminals.Where(t => t.Id == viewModel.Id).FirstOrDefault();


                    var pricingService = await new PricingServiceDomain(this).SaveTerminalDetails(viewModel);
                    if (viewModel.Id > 0)
                    {
                        if (pricingService.Status == Status.Success && pricingService.Result > 0)
                        {
                            response.StatusCode = Status.Success;
                            response.StatusMessage = "Terminal updated successfully";
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = "Error while updating Terminal details. Please try again";
                        }
                    }
                    else
                    {
                        if (pricingService.Status == Status.Success && pricingService.Result > 0)
                        {
                            response.StatusCode = Status.Success;
                            response.StatusMessage = "Terminal Added successfully";
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = "Error while adding Terminal details. Please try again";
                        }
                    }

                    if (pricingService.Status == Status.Success && pricingService.Result > 0 && viewModel.Id > 0) //save history in update call 
                    {
                        if (orginalTerminaldetails != null)
                        {
                            var terminalHistory = orginalTerminaldetails.ToTerminalViewModel();
                            var terminalHistoryJson = JsonConvert.SerializeObject(terminalHistory);
                            LogManager.Logger.WriteDebug("MasterDomain", "SaveTerminalDetails", "TerminalId updated:" + pricingService.Result.ToString() + "Old Record Json:" + terminalHistoryJson);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "SaveTerminalDetails", ex.Message, ex);
                response.StatusCode = Status.Failed;
                response.StatusMessage = "Error occurred when processing your request";
            }
            return response;
        }

        private bool ValidateTerminal(PickupLocationDetailViewModel viewModel, StatusViewModel response)
        {
            bool isExists = false;

            if (!string.IsNullOrWhiteSpace(viewModel.Name))
            {
                isExists = Context.DataContext.MstExternalTerminals.Any(t => t.Id != viewModel.Id && t.Name.Trim().ToLower() == viewModel.Name.Trim().ToLower());
                if (isExists)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = $"Terminal {viewModel.Name} already exists.";
                }
            }

            if (!isExists && !string.IsNullOrWhiteSpace(viewModel.ControlNumber) && viewModel.ControlNumber != "-")
            {
                isExists = Context.DataContext.MstExternalTerminals.Any(t => t.Id != viewModel.Id && t.ControlNumber.Trim().ToLower() == viewModel.ControlNumber.Trim().ToLower());
                if (isExists)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = $"Terminal Control Number {viewModel.ControlNumber} already exists.";
                }
            }

            if (!isExists && !string.IsNullOrWhiteSpace(viewModel.Abbreviation) && viewModel.Abbreviation != "-")
            {
                isExists = Context.DataContext.MstExternalTerminals.Any(t => t.Id != viewModel.Id && t.Abbreviation.Trim().ToLower() == viewModel.Abbreviation.Trim().ToLower());
                if (isExists)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = $"Abbreviation {viewModel.Abbreviation} already exists.";
                }
            }

            return isExists;
        }


        public async Task<List<TerminalProductMappingDetailsViewModel>> GetTerminalProductMappingDetails(int countryId, int pricingSourceId)
        {
            var response = new List<TerminalProductMappingDetailsViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var terminalMappingDetails = await spDomain.GetTerminalProductMappingDetails(countryId, pricingSourceId);
                if (terminalMappingDetails != null && terminalMappingDetails.Any())
                {
                    foreach (var mapping in terminalMappingDetails)
                    {
                        if (!string.IsNullOrWhiteSpace(mapping.AssignedProducts))
                        {
                            mapping.MappedProducts = JsonConvert.DeserializeObject<List<DropdownDisplayItem>>(mapping.AssignedProducts);
                        }

                    }
                    response.AddRange(terminalMappingDetails);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetTerminalProductMappingDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetMstProductsForTerminalMapping(int pricingSourceId = (int)PricingSource.Axxis)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = await Context.DataContext
                            .MstProducts
                            .Where(t => t.IsActive && t.PricingSourceId == pricingSourceId && t.ProductTypeId != (int)ProductTypes.NonStandardFuel
                             && t.ProductTypeId != (int)ProductTypes.Additives
                             && t.ProductTypeId != (int)ProductTypes.PremiumGas)
                            .Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = !(t.DisplayName == null || t.DisplayName == string.Empty) ? t.DisplayName : t.Name,
                            }).OrderBy(t => t.Name).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetMstProductsForTerminalMapping", ex.Message, ex);
            }
            return response;
        }
    }
}
