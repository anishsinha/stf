using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Helpers
{
    public class CommonHelperMethods
    {
        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCompanyBusinessTenures()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCompanyBusinessTenures();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCompanyFuelQuantities()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCompanyFuelQuantities();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCompanySizes()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCompanySizes();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCompanyTypes()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCompanyTypes();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCountries()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCountries();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetCountriesEx()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCountriesEx();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetCountriesGroupEx(int countryId = 0)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetMstCountryAsGroup(countryId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetMonths()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetMonths();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetMonthsEx()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetMonthsEx();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static bool IsTaxExemptionEnabled()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().IsTaxExemptionEnabled();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static bool IsMultipleServingStates(int companyId, int countryId)
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().IsMultipleServingStates(companyId, countryId)).Result;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetNumbers(int startNumber = 0, int endNumber = 10)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetNumbers(startNumber, endNumber);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetPhoneTypes()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetPhoneTypes();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetStates(int courntyId = 0)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetStates(courntyId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetStatesForOffer(bool withAllOption)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetStatesForOffer(withAllOption);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetStates(int countryId, bool withAllOption)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetStatesForOffer(countryId, withAllOption);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCities(int stateId, bool withAllOption = false)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCities(stateId, withAllOption);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetMultipleCities(string stateId, bool withAllOption = false)
        {
            var stateIds = stateId.Split(',').Select(Int32.Parse).ToList();
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCities(stateIds, withAllOption);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetMultipleZipcodes(string cityIds, bool withAllOption = false)
        {
            var cities = !string.IsNullOrEmpty(cityIds) ? cityIds.Split(',').Select(Int32.Parse).ToList() : new List<int>();
            return ContextFactory.Current.GetDomain<MasterDomain>().GetZipCodes(cities, withAllOption);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCityGroupCities(PricingSource sourceId = PricingSource.Axxis)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCityGroupCities(sourceId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetPoContact(int companyId, int companyTypeId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetPoContact(companyId, companyTypeId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetSourceRegions(int companyId)
        {
            var domain = ContextFactory.Current.GetDomain<RegionDomain>();
            var response = Task.Run(() => domain.GetAllSourceRegionsForDDL(companyId)).Result;
            return response;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetTerminalsAssignedToSourceRegions(int companyId, SourceRegionsViewModel sourceRegionModel)
        {
            var domain = ContextFactory.Current.GetDomain<RegionDomain>();
            var inputModel = new SourceRegionRequestModel();
            if (sourceRegionModel != null)
            {
                inputModel.FuelTypeId = sourceRegionModel.FuelTypeId.HasValue ? sourceRegionModel.FuelTypeId.Value : 0;
                inputModel.CountryId = sourceRegionModel.CountryId;
                inputModel.JobId = sourceRegionModel.JobId.HasValue ? sourceRegionModel.JobId.Value : 0;
                inputModel.Latitude = sourceRegionModel.Latitude;
                inputModel.Longitude = sourceRegionModel.Longitude;
                inputModel.PricingCodeId = sourceRegionModel.PricingCodeId;
                inputModel.PricingSourceId = sourceRegionModel.PricingSourceId;
                inputModel.SourceRegionIds = sourceRegionModel.SelectedSourceRegions;
            }
            var response = Task.Run(() => domain.GetTerminalsBySourceRegion(companyId, inputModel)).Result;
            return response;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetBulkPlantsAssignedToSourceRegions(int companyId, SourceRegionsViewModel sourceRegionModel)
        {
            var domain = ContextFactory.Current.GetDomain<RegionDomain>();
            var inputModel = new SourceRegionRequestModel();
            if (sourceRegionModel != null)
            {
                inputModel.FuelTypeId = sourceRegionModel.FuelTypeId.HasValue ? sourceRegionModel.FuelTypeId.Value : 0;
                inputModel.CountryId = sourceRegionModel.CountryId;
                inputModel.Latitude = sourceRegionModel.Latitude;
                inputModel.Longitude = sourceRegionModel.Longitude;
                inputModel.SourceRegionIds = sourceRegionModel.SelectedSourceRegions;
            }
            var response = Task.Run(() => domain.GetBulkPlantsBySourceRegion(companyId, inputModel)).Result;
            return response;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetBrokerPoContact(int companyId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetBrokerPoContact(companyId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetBusinessSubTypes()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetBusinessSubTypes();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCompanyBuyers(int companyId, int companyTypeId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCompanyBuyers(companyId, companyTypeId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCompanyAdmins(int companyId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCompanyAdmins(companyId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<StateDropdownExtendedItem> GetStatesEx(int countryId = (int)Country.All)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetStatesEx(countryId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetSupplierQualifications()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetSupplierQualifications();
        }
        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetPrivateSuppliers(int companyId, int brandedCompanyId = -1)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetPrivateSuppliers(companyId, brandedCompanyId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetYourSuppliers(int companyId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetYourSuppliers(companyId);
        }

        public static List<DropdownDisplayItem> GetServiceAreaTypes() 
        {
            var response = new List<DropdownDisplayItem>();
            foreach (int item in Enum.GetValues(typeof(ServiceAreaType)))
            { 
                response.Add(new DropdownDisplayItem() { Id = item, Name = ((ServiceAreaType)item).GetDisplayName() }); 
            }
            return response;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetYourCustomers(int companyId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetYourCustomers(companyId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetYourCustomersForDipTest(int companyId, CompanyType companyTypeId)
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetYourCustomersForDipTest(companyId, companyTypeId)).Result;
        }
        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static string GetApplicationSettingValue(string key)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetApplicationSettingValue(key);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetYears(int minusYears = 0, int plusYears = 10)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetYears(minusYears, plusYears);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetYearsEx(int minusYears = 0, int plusYears = 10)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetYearsEx(minusYears, plusYears);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetRolesByCompanyType(int companyTypeId, int companySubTypeId = 0)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetRolesByCompanyType(companyTypeId, companySubTypeId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetEventTypeByRoles(int companyTypeId, IList<int> roleIds, CompanyType companySubTypeId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetEventTypeByRoles(companyTypeId, roleIds, companySubTypeId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetProductTypes(int productTypeId = 0)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetProductTypes(productTypeId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetProductTypesForMapping()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetProductTypesForMapping();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetAssetFuelTypes()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetAssetFuelTypes();
        }

        public static List<string> GetRoleNamesById(List<int> roleIds)
        {
            return ContextFactory.Current.GetDomain<HelperDomain>().GetRoleNamesById(roleIds);
        }

        public static List<string> GetQualificationNamesById(List<int> qualificationIds)
        {
            return ContextFactory.Current.GetDomain<HelperDomain>().GetQualificationNamesById(qualificationIds);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetJobs(int userId, int countryId = 0, bool isMFN = false)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetJobs(userId, countryId, isMFN);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCarrierUserEmails(int assignedCarrierCompanyId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCarrierUserEmails(assignedCarrierCompanyId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetJobsForAsset(int companyId, int assetId, int jobId, int userId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetJobsForAsset(companyId, assetId, jobId, userId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetJobByFuelRequest(int fuelRequestId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetJobByFuelRequest(fuelRequestId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCustomersForCompany(int userCompanyId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCustomersForCompany(userCompanyId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetOrdersForCompany(int userCompanyId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetOrdersForCompany(userCompanyId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetInvoiceDeclineReasons(UoM uoM)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetInvoiceDeclineReasons(uoM);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetFuelProducts(ProductDisplayGroups productDisplayGroupId, int companyId = 0, int jobId = 0)
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetAxxisFuelProducts(productDisplayGroupId, companyId, jobId)).Result;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetAllFuelProducts()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetAllFuelProducts();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetAllCustomers(int companyId)
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<DashboardDomain>().GetCustomers(companyId)).Result;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetTPOSuppliers()
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetTPOSupplierCompanies()).Result;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetProductDisplayGroups(int companyId = 0, bool isTPO = false)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetProductDisplayGroups(companyId, isTPO);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetProductDisplayGroupsForMapping()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetProductDisplayGroupsForMapping();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetOrderTypes()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetOrderTypes();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetRackAvgPricingTypes()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetRackAvgPricingTypes();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetMarketBasedPricingTypes()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetMarketBasedPricingTypes();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetResaleFeeTypes()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetResaleFeeTypes();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetMathOperators()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetMathOperators();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetPricingTypes(bool TierPricing = false)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetPricingTypes(TierPricing);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetPricingTypesForOfferTiers(bool isWithSupplierCost = false)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetPricingTypesForOfferTiers(isWithSupplierCost);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetPaymentTerms()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetPaymentTerms();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetPrivateSupplierList(int companyId, int brandedCompanyId = -1)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetPrivateSupplierList(companyId, brandedCompanyId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetPrivateSupplierListByFuelRequest(int fuelRequestId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetPrivateSupplierListByFuelRequest(fuelRequestId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetAdditionalFee()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetAdditionalFee();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetTaxesForOtherProductFuelType()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetTaxesForOtherProductFuelType();
        }

        public static List<DropdownDisplayItem> GetCounterOfferFees(int feeType, bool isNoFeeRequired)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCounterOfferFees(feeType, isNoFeeRequired);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static string GetPricePerGallon(int fuelRequestId)
        {
            return ContextFactory.Current.GetDomain<HelperDomain>().GetPrice(fuelRequestId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static string GetInvoicePricePerGallon(int invoiceId)
        {
            return ContextFactory.Current.GetDomain<HelperDomain>().GetInvoicePrice(invoiceId);
        }

        public static string GetResalePricePerGallon(int fuelRequestId)
        {
            return ContextFactory.Current.GetDomain<HelperDomain>().GetResalePrice(fuelRequestId);
        }

        public static string GetQuotationPricePerGallon(decimal pricePerGallon, int pricingTypeId, int? rackAvgTypeId)
        {
            return ContextFactory.Current.GetDomain<HelperDomain>().GetPricePerGallon(pricePerGallon, pricingTypeId, rackAvgTypeId ?? 0);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetAddresses(int companyId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetAddresses(companyId);
        }

        //[OutputCache(CacheProfile = "OutputCacheMasterData")]
        //public static List<DropdownDisplayItem> GetClosestTerminals(int orderId, string terminal = "")
        //{
        //    return ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetClosestTerminals(orderId, terminal);
        //}

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetOrderCancelationReasons(int companyTypeId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetOrderCancelationReasons(companyTypeId);
        }

        public static string GetName<TEnum>(int id) where TEnum : IConvertible
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetName<TEnum>(id);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetWeekDays()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetMstWeekDays();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtended> GetTankModelType(int companyId)
        {
            var response = new List<DropdownDisplayExtended>();
            var companyIds = new List<int>();
            companyIds.Add(companyId);
            var tankModelTypes = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetTankModelType(companyIds)).Result;

            if (tankModelTypes != null && tankModelTypes.Any())
            {
                foreach (var item in tankModelTypes)
                {
                    var tankModel = new DropdownDisplayExtended();
                    tankModel.Id = item.Id;
                    tankModel.Name = item.Name + " - " + item.Code;
                    response.Add(tankModel);
                }
            }

            return response;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetMstScheduleTypes()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetMstScheduleTypes();
        }

        public static UserRoles GetUserRole(SiteFuelUserFilterType filter)
        {
            return ContextFactory.Current.GetDomain<HelperDomain>().GetUserRole(filter);
        }

        public static List<DropdownDisplayItem> GetQualifiedDrivers(int companyId, bool includeAll = false, bool allOptionAtTheEnd = false)
        {
            return ContextFactory.Current.GetDomain<HelperDomain>().GetQualifiedDrivers(companyId, includeAll, allOptionAtTheEnd);
        }

        public static List<DropdownDisplayItem> GetAllDrivers(int companyId)
        {
            return ContextFactory.Current.GetDomain<HelperDomain>().GetAllDrivers(companyId);
        }

        public static string GetAssignedDriver(int driverId)
        {
            return ContextFactory.Current.GetDomain<HelperDomain>().GetAssignedDriver(driverId);
        }

        public static List<string> GetUserNamesById(List<int> users)
        {
            return ContextFactory.Current.GetDomain<HelperDomain>().GetUserNamesById(users);
        }

        public static List<DropdownDisplayItem> GetSubContractors(int? jobId, int companyId)
        {
            return ContextFactory.Current.GetDomain<AssetDomain>().GetSubContractors(jobId, companyId);
        }

        public static List<DropdownDisplayItem> GetSuppliers(int companyId)
        {
            return ContextFactory.Current.GetDomain<HelperDomain>().GetSuppliers(companyId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCompanies()
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetCompanies()).Result;
        }

        public static List<DropdownDisplayItem> GetCompanyOnsiteConstacts(int companyId)
        {
            return ContextFactory.Current.GetDomain<HelperDomain>().GetCompanyOnsiteConstacts(companyId);
        }

        public static List<DropdownDisplayItem> GetCompanyTaxExemptionLicenses(int companyId, int roleId)
        {
            return ContextFactory.Current.GetDomain<HelperDomain>().GetCompanyTaxExemptionLicenses(companyId, roleId);
        }

        public static List<DropdownDisplayItem> GetSuperAdmins()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetSuperAdmins();
        }

        public static List<DropdownDisplayItem> GetAccountSpecialistUsers(bool isAccountSpecialist = false, bool isActiveUsers = false, int userId = 0)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetAccountSpecialistUsers(isAccountSpecialist, isActiveUsers, userId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetMailboxQueryTypes()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetMailboxQueryTypes();
        }

        public static List<DeliveryScheduleDropdownExtendedItem> GetCurrentDeliverySchedules(int orderId, int invoiceId, string splitLoadChainId = "")
        {
            return ContextFactory.Current.GetDomain<HelperDomain>().GetCurrentDeliverySchedules(orderId, invoiceId, splitLoadChainId);
        }

        public static List<DropdownDisplayItem> GetRecipients(int userId, List<int> recipientCompanies)
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<AppMessageDomain>().GetRecipientsAsync(userId, recipientCompanies)).Result;
        }

        public static int GetCompanyTypeId(int companyId)
        {
            return ContextFactory.Current.GetDomain<CompanyDomain>().GetCompanyTypeId(companyId);
        }

        public static List<DropdownDisplayItem> GetJobList(string companyName, bool isFtl, bool foAsTerminal, int supplierUserId, int supplierCompanyId, bool isPort = false, int countryId = (int)Country.USA)
        {
            return ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetJobList(companyName, isFtl, foAsTerminal, supplierUserId, supplierCompanyId, isPort, countryId);
        }

        public static List<DropdownDisplayItem> GetAllBuyerCompanies()
        {
            return ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetAllBuyerCompanies();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetAccountTypes()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetAccountTypes();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCustomersForBrokeredOrder(int companyId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCustomersForBrokeredOrder(companyId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetQbCompanies(int companyId)
        {
            return ContextFactory.Current.GetDomain<QbDomain>().GetQbCompanies(companyId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetThirdPartyNozzles()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetThirdPartyNozzles();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetAllFeeTypes(int companyId, Currency currency = Currency.None, int truckLoadType = (int)TruckLoadTypes.LessTruckLoad)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetAllFeeTypes(companyId, currency, truckLoadType);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetTankRentalFees(int companyId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetTankRentalFees(companyId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetAllMarginTypes()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetAllMarginTypes();
        }

        public static List<DropdownDisplayItem> GetAllFeeSubTypes(string feeTypeId, Currency currency = Currency.None)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetAllFeeSubTypes(feeTypeId, currency);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetDiscountFeeTypes(int invoiceId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetDiscountFeeTypes(invoiceId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetDiscountFeeSubTypes()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetDiscountFeeSubTypes();
        }

        public static List<DropdownDisplayItem> GetAllFeeConstraintTypes()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetAllFeeConstraintTypes();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<CountryState> GetStatesOfAllCountries(int countryId = (int)Country.All)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetStatesOfAllCountries(countryId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetCurrenyList()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCurrenyList();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetUoMList()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetUoMList();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetEventsGroupList()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetEventsGroupList();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<ChildCompanyViewModel> GetParentCompanies(int parentCompanyId = 0)
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetParentCompanies(parentCompanyId)).Result;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static CompanyGroupViewModel GetChildCompaniesByCompany(int companyId)
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<SuperAdminDomain>().GetGroupDetails(companyId)).Result;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCompanyTypesForGroup()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCompanyTypesForGroup();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCompanyGroupList(int companyId, CompanyType companySubType)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCompanyGroupList(companyId, companySubType);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static bool IsFeatureEnabledForCompany(int companyId, CompanyType companyType, FeatureTypes feature)
        {
            return ContextFactory.Current.GetDomain<CompanyDomain>().IsFeatureEnabledForCompany(companyId, companyType, feature);
        }

        public static List<int> ServingCountry(int companyId, CompanyType companyType, CompanyType companySubType)
        {
            if (companyType == CompanyType.Buyer || companySubType == CompanyType.Buyer)
            {
                return ContextFactory.Current.GetDomain<CompanyDomain>().MultiCountrySupportForBuyer(companyId);
            }
            else
            {
                return ContextFactory.Current.GetDomain<CompanyDomain>().MultiCountrySupportForSupplier(companyId);
            }
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static string GenerateTileId(string tileName)
        {
            return tileName + ApplicationConstants.TileKey;
        }

        #region PricingTable
        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetSupplierCustomers(int companyId, int countryId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetSupplierCustomers(companyId, countryId);
        }

        public static List<DropdownDisplayItem> GetSupplierCustomers(int companyId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetSupplierCustomers(companyId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetTiers()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetTierTypes();
        }
        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetSupplierProducts(int companyId, int countryId, int pricingSourceId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetSupplierProducts(companyId, countryId, pricingSourceId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetAllSupplierProducts(int companyId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetAllSupplierProducts(companyId);
        }
        #endregion


        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetSupplierOrdersForBuyer(int buyerCompanyId, int supplierCompanyId, int countryId, int billingScheduleId = 0)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetSupplierOrdersForBilling(buyerCompanyId, supplierCompanyId, countryId, billingScheduleId).Result;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetStatementFrequency()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetFrequency();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetSupplierCustomersForBilling(int companyId, int countyId, int customerId = 0)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetSupplierCustomersForBilling(companyId, countyId, customerId).Result;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetStatementTimeZone(int companyId, int countyId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetStatementTimeZone(companyId, countyId).Result;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> UpdateFrequencyTypes()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().UpdateFrequencyTypes();
        }


        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCustomersForStatements(int companyId, int countryId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetCustomersForStatements(companyId, countryId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetScheduleIdForStatements(int companyId, int countryId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetScheduleIdForStatements(companyId, countryId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetPricingSources()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetPricingSources();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetPricingSourceFeeds(int pricingSourceId = (int)PricingSource.Axxis)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetPricingSourceFeeds(pricingSourceId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetSurchargeTableTypes()
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            foreach (var tabletype in Enum.GetValues(typeof(TableTypes)))
            {
                TableTypes type = (TableTypes)tabletype;
                response.Add(new DropdownDisplayItem()
                {
                    Id = (int)tabletype,
                    Name = type.GetDisplayName()
                });
            }
            return response;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetFreightRateRuleTypes()
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            foreach (var ruletype in Enum.GetValues(typeof(FreightRateRuleType)))
            {
                if ((FreightRateRuleType)ruletype == FreightRateRuleType.Unknown)
                    continue;
                response.Add(new DropdownDisplayItem()
                {
                    Id = (int)ruletype,
                    Name = ruletype.ToString()
                });
            }
            return response;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetLoadQueueAttributes()
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            foreach (var loadqueue in Enum.GetValues(typeof(LoadQueueAttributes)))
            {
                LoadQueueAttributes item = (LoadQueueAttributes)loadqueue;
                response.Add(new DropdownDisplayItem()
                {
                    Id = (int)loadqueue,
                    Name = item.GetDisplayName()
                });
            }
            return response;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetDRQueueAttributes()
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            foreach (var drqueue in Enum.GetValues(typeof(DRQueueAttributes)))
            {
                DRQueueAttributes item = (DRQueueAttributes)drqueue;
                response.Add(new DropdownDisplayItem()
                {
                    Id = (int)drqueue,
                    Name = item.GetDisplayName()
                });
            }
            return response;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetMstProducts()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetMstProducts();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetMstProducts(int pricingSourceId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetMstProducts(pricingSourceId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetMstOPISProducts()
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetMstOPISProducts();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetMstOPISProducts(int pricingSourceId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetMstOPISProducts(pricingSourceId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetDipTestData(int userId, int companyId, CompanyType companyType, int customerCompanyId = 0)
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<JobDomain>().GetOpenJobsByCompanyType(companyType, userId, companyId, customerCompanyId)).Result;
        }
        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetSurchargeProductTypes()
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            foreach (var productType in Enum.GetValues(typeof(SurchargeProductTypes)))
            {
                if ((SurchargeProductTypes)productType == SurchargeProductTypes.Unknown)
                    continue;
                response.Add(new DropdownDisplayItem()
                {
                    Id = (int)productType,
                    Name = productType.ToString()
                });
            }
            return response;
        }

        public static List<DropdownDisplayItem> GetExistingDropLocation(int orderId, int? deliveryScheduleId = null, int? trackableScheduleId = null)
        {
            var addresses = Task.Run(() => ContextFactory.Current.GetDomain<OrderDomain>().GetSplitDropAddressesAsync(orderId, trackableScheduleId, deliveryScheduleId)).Result;
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            foreach (var location in addresses)
            {
                DropdownDisplayItem item = new DropdownDisplayItem();
                item.Id = location.Id;
                item.Name = location.Address;
                response.Add(item);
            }
            return response;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetAllTerminals()
        {
            var psDomain = ContextFactory.Current.GetDomain<PricingServiceDomain>();
            var response = Task.Run(() => psDomain.GetAllTerminals()).Result;
            return response;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetCompanyShifts(int companyId)
        {
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = Task.Run(() => fsDomain.GetCompanyShiftDdl(companyId)).Result;
            if (response == null)
                response = new List<DropdownDisplayExtendedItem>();
            return response;
        }
        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<SelectListItem> GetTrailerTypeEnumList()
        {
            return (Enum.GetValues(typeof(TrailerTypeStatus)).Cast<TrailerTypeStatus>().Select(
        enu => new SelectListItem() { Text = enu.ToString(), Value = enu.ToString() })).ToList();
        }
        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<SelectListItem> GetDriverLicenceTypeEnumList()
        {
            return (Enum.GetValues(typeof(LicenceRequirement)).Cast<LicenceRequirement>().Select(
        enu => new SelectListItem() { Text = enu.ToString() == LicenceRequirement.Class1.ToString() ? "Class 1" : "Class 3", Value = enu.ToString() })).ToList();
        }
        public static bool IsDispatcherExists(int companyId)
        {
            return ContextFactory.Current.GetDomain<CompanyDomain>().IsDispatcherExists(companyId);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetBulkPlants(int companyId, string prefix = "")
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<DispatchDomain>().GetBulkPlants(prefix, companyId)).Result;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetAssetOrTankListAsync(int userId, int? jobId, int type, int assetId = 0)
        {
            return ContextFactory.Current.GetDomain<AssetDomain>().GetAssetOrTankListAsync(userId, jobId, type, assetId);
        }


        public static List<DropdownDisplayExtended> GetRegionsForTPOOrder(int userId)
        {
            var sbDomain = ContextFactory.Current.GetDomain<ScheduleBuilderDomain>();
            var response = Task.Run(() => sbDomain.GetRegions(userId)).Result;
            if (response == null)
                response = new List<DropdownDisplayExtended>();
            return response;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetCarriers(int supplierCompanyId)
        {
            var domain = ContextFactory.Current.GetDomain<CompanyDomain>();
            var response = Task.Run(() => domain.GetCarriers(supplierCompanyId)).Result;
            return response;
        }

        public static List<DropdownDisplayItem> GetCarriersTPO(int supplierCompanyId, bool isJobEdit = false)
        {
            var domain = ContextFactory.Current.GetDomain<CompanyDomain>();
            var response = new List<DropdownDisplayItem>();
            if (isJobEdit)
                response = Task.Run(() => domain.GetCarriers(supplierCompanyId)).Result;
            return response;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetSupplierTerminals(int companyId)
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetSupplierTerminals(companyId)).Result;
            if (response == null)
                response = new List<DropdownDisplayItem>();
            return response;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetSupplierFuelTypes(int companyId)
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetSupplierFuelTypes(companyId)).Result;
            if (response == null)
                response = new List<DropdownDisplayItem>();
            return response;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtended> GetCarriersByCompanyId(int supplierCompanyId)
        {
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = Task.Run(() => fsDomain.GetAssignedCarriers(supplierCompanyId)).Result;
            if (response == null)
                response = new List<DropdownDisplayExtended>();
            return response;
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static Tuple<int, string> GetCompanyDetailsByJobId(int jobId)
        {
            return ContextFactory.Current.GetDomain<CompanyDomain>().GetCompanyDetailsByJobId(jobId);
        }
        public static class Enumeration
        {
            public static List<DropdownDisplayExtendedItem> GetAll<TEnum>() where TEnum : struct
            {
                var enumerationType = typeof(TEnum);

                if (!enumerationType.IsEnum)
                    throw new ArgumentException("Enumeration type is expected.");

                var dictionary = new List<DropdownDisplayExtendedItem>();

                foreach (int value in Enum.GetValues(enumerationType))
                {
                    var name = Enum.GetName(enumerationType, value);
                    var memberInfo = enumerationType.GetMember(name);
                    var attributes = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                    var description = ((DisplayAttribute)attributes[0]).Name;
                    dictionary.Add(new DropdownDisplayExtendedItem { Id = value, Name = description, Code = value.ToString() });
                }

                return dictionary;
            }

        }

        public static List<DropdownDisplayExtended> GetRoutesForTPOOrder(string regionId)
        {
            var sbDomain = ContextFactory.Current.GetDomain<ScheduleBuilderDomain>();
            var response = Task.Run(() => sbDomain.GetRouteDetails(regionId)).Result;
            if (response == null)
                response = new List<DropdownDisplayExtended>();
            return response;
        }
        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayExtendedItem> GetScheduleQtyType()
        {
            return Enumeration.GetAll<ScheduleQuantityType>();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<SelectListItem> GetLocationInventoryManagedByEnumList()
        {
            return (Enum.GetValues(typeof(LocationInventoryManagedBy)).Cast<LocationInventoryManagedBy>().Select(enu => new SelectListItem() { Text = enu.ToString(), Value = enu.ToString() })).ToList();
        }

        public static List<DropdownDisplayExtendedId> GetProductSequenceTypes(int supplierCompanyId, ProductSequencingCreationMethod sequenceMethod, ProductSequenceType sequenceType = ProductSequenceType.Product, int jobId = 0)
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<CompanyDomain>().GetProductSequenceTypes(supplierCompanyId, sequenceMethod, sequenceType, jobId)).Result;
        }
        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetforcastingInventoryUOM()
        {
            return EnumHelperMethods.EnumToList<InventoyUOM>();
        }
        public static List<DropdownDisplayItem> GetBandPeriodDropDown()
        {
            return new List<DropdownDisplayItem>()
            {
                new DropdownDisplayItem{ Id = 1, Name = "1" }, new DropdownDisplayItem{ Id = 2, Name = "2" }, new DropdownDisplayItem{ Id = 3, Name = "3" }, new DropdownDisplayItem{ Id = 4, Name = "4" }, new DropdownDisplayItem{ Id = 6, Name = "6" }, new DropdownDisplayItem{ Id = 8, Name = "8" }, new DropdownDisplayItem{ Id = 12, Name = "12" },
            };
        }
        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetforcastingRateOfConsumtionUOM()
        {
            return EnumHelperMethods.EnumToList<RateOfConsumsionUOM>();
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetMstProductsDropDownListForLFVBol(int pricingSourceId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetMstProductsDropDownListForLFVBol(pricingSourceId);
        }
        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetTankScaleMeasurementList(int assetType, int UoM, string tankMakeModel)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetTankScaleMeasurementList(assetType, UoM, tankMakeModel);
        }

        [OutputCache(CacheProfile = "OutputCacheMasterData")]
        public static List<DropdownDisplayItem> GetTFXFuelTypeByProductTypeId(int ProductTypeId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetTFXFuelTypeByProductTypeId(ProductTypeId);
        }
        public static List<DropdownDisplayItem> GetAllUoMlList()
        {
            return EnumHelperMethods.EnumToList<UoM>();
        }

        public static List<DropdownDisplayItem> GetBOlListForInvoiceHeader(int invoiceHeaderId)
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<InvoiceDomain>().GetBOlListForInvoiceHeader(invoiceHeaderId)).Result;
        }

        [OutputCache(CacheProfile = "OutputCompanySupplierAdminUsers")]
        public static List<DropdownDisplayItem> GetCompanySupplierAdminUsers(int companyId)
        {
            return  ContextFactory.Current.GetDomain<CompanyDomain>().GetCompanySupplierAdminUsers(companyId);
        }

        [OutputCache(CacheProfile = "OutputExternalIdentityProviders")]
        public static List<DropdownDisplayItem> GetExternalIdentityProviders(int companyId)
        {
            return ContextFactory.Current.GetDomain<HelperDomain>().GetExternalIdentityProviders(companyId);
        }

        public static List<DropdownDisplayExtendedItem> GetPortsList()
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<CompanyDomain>().GetPortsList()).Result;
        }

        public static List<DropdownDisplayExtendedItem> GetVesselList(int companyId=ApplicationConstants.SuperAdminCompanyId)
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<CompanyDomain>().GetVesselList(companyId)).Result;
        }

        public static List<DropdownDisplayExtendedItem> GetReasonCategories(int companyId)
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<SettingsDomain>().GetReasonCategoryListDDL(companyId)).Result;
        }
        public static List<DropdownDisplayExtendedProperty> GetJobsForBuyer(int companyId, bool IsMarine)
        {
            return Task.Run(() => ContextFactory.Current.GetDomain<JobDomain>().GetJobsForBuyer(companyId, IsMarine)).Result;
        }
        public static bool AnyAndNotNull<TSource>(List<TSource> list)
        {
            return (list?.Any() ?? false);
        }

        public static string GetCustomerTFCUId(int customerId)
        {
            return ApplicationConstants.CustomerNumberPrefix + customerId.ToString(ApplicationConstants.SevenDigit);
        }

        public static string GetTFXLocationId(int jobId)
        {
            return ApplicationConstants.TFXLocationIdPrefix + jobId.ToString();
        }

        public static List<DropdownDisplayItem> GetLoadOptimizationUsers(int companyId)
        {
            return ContextFactory.Current.GetDomain<MasterDomain>().GetLoadOptimizationUsers(companyId);
        }
    }
}