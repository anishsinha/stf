using RestSharp;
using RestSharp.Authenticators;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.AvaTaxExciseWebService;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SiteFuel.Exchange.Domain
{
    public class HelperDomain : BaseDomain
    {
        public HelperDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public HelperDomain(SiteFuelUow SiteFuelDbContext)
          : base(SiteFuelDbContext)
        {
        }

        public HelperDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public async Task<bool> IsEmailExistAsync(string email, bool checkEmailConfirmation = false)
        {
            var response = false;
            try
            {
                if (checkEmailConfirmation)
                {
                    response = await Context.DataContext.Users.AnyAsync(t => t.Email.ToLower() == email.ToLower() && t.IsEmailConfirmed);
                }
                else
                {
                    response = await Context.DataContext.Users.AnyAsync(t => t.Email.ToLower() == email.ToLower());
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsEmailExistAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> IsEmailExistAsync(string email, int userId, bool checkEmailConfirmation = false)
        {
            var response = false;
            try
            {
                if (checkEmailConfirmation)
                {
                    response = await Context.DataContext.Users.AnyAsync(t => t.Id != userId && t.Email.ToLower() == email.ToLower() && t.IsEmailConfirmed);
                }
                else
                {
                    response = await Context.DataContext.Users.AnyAsync(t => t.Id != userId && t.Email.ToLower() == email.ToLower());
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsEmailExistAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> IsAdditionalUserEmailExistAsync(string email)
        {
            var response = false;
            try
            {
                response = await Context.DataContext.Users.AnyAsync(t => t.Email.ToLower() == email.ToLower() && !t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.Driver && t1.IsActive)); // while inviting driver we add it into user table but while registration it give validation issue.
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsAdditionalUserEmailExistAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> IsLicenseNumberExist(string licenseNumber, int id)
        {
            var response = false;
            if (string.IsNullOrWhiteSpace(licenseNumber) || id > 0)
            {
                return false;
            }
            try
            {
                response = await Context.DataContext.TaxExemptLicenses.AnyAsync(t => t.IsActive && t.LicenseNumber.Equals(licenseNumber, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsLicenseNumberExist", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> IsSourceRegionExist(string name, int id, int companyId)
        {
            var response = false;
            try
            {
                response = await Context.DataContext.SourceRegions.AnyAsync(t => t.Id != id && t.IsActive && t.Name.ToLower() == name.ToLower() && t.CompanyId == companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsSourceRegionExist", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<string>> GetExistingJobContactsAsync(List<string> emails, int companyId)
        {
            var response = new List<string>();
            try
            {
                response = await Context.DataContext.Users.Where(t => emails.Any(t1 => t1.ToLower() == t.Email.ToLower()))
                                                                    .Select(t => t.Email)
                                                                    .ToListAsync();
                var additionalUsers = await Context.DataContext.CompanyXAdditionalUserInvites.Where(t => t.CompanyId == companyId &&
                                                                                                    emails.Any(t1 => t1.ToLower() == t.Email.ToLower()))
                                                                                            .Select(t => t.Email)
                                                                                            .ToListAsync();
                response.AddRange(additionalUsers);
                response.Distinct();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsJobContactExistAsync", ex.Message, ex);
            }
            return response;
        }

        public int GetApplicationTemplateId(string supplierURL)
        {
            var response = 0;
            try
            {
                var applicationTemplate = Context.DataContext.MstApplicationTemplates.FirstOrDefault(t => t.URLName == supplierURL && t.IsActive);
                response = applicationTemplate != null ? applicationTemplate.Id : (int)ApplicationTemplate.TrueFill;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetApplicationTemplateId", ex.Message, ex);
            }
            return response;
        }

        public List<int> GetGroupList(string groupIds)
        {
            var response = new List<int>();
            try
            {
                if (!string.IsNullOrEmpty(groupIds) && groupIds != "0" && groupIds != "-1")
                {
                    response = groupIds.Split(',').Select(x => Convert.ToInt32(x)).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetGroupListAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> IsOnboardingComplete(int id)
        {
            var response = false;
            try
            {
                response = await Context.DataContext.Users.AnyAsync(t => t.Id == id && t.IsOnboardingComplete);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsOnboardingCompleteAsync", ex.Message, ex);
            }
            return response;
        }

        public bool IsValidAddress(string address)
        {

            var response = false;
            try
            {
                var point = GoogleApiDomain.GetGeocode(address);
                response = (point != null && point.Latitude != 0 && point.Longitude != 0);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsValidAddress", ex.Message, ex);
            }
            return response;
        }

        public bool IsValidJobName(int jobId, string jobName, int companyId)
        {
            var response = false;

            try
            {
                var job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id != jobId && t.IsActive && t.Name.ToLower() == jobName.ToLower() && t.Company.Id == companyId);
                response = job == null;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsValidJobName", ex.Message, ex);
            }

            return response;
        }

        public bool IsRetailJob(int jobId)
        {
            var response = false;

            try
            {
                var isRetailJob = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == jobId).IsRetailJob;
                return isRetailJob;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsRetailJob", ex.Message, ex);
            }

            return response;
        }

        public bool IsValidDealName(int invoiceId, string dealName)
        {
            var response = false;

            try
            {
                var job = Context.DataContext.Discounts.FirstOrDefault(t => t.DealName.ToLower() == dealName.ToLower() && t.InvoiceId == invoiceId);
                response = job == null;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsValidDealName", ex.Message, ex);
            }

            return response;
        }

        public bool IsValidFullAddress(AddressViewModel address)
        {
            var response = false;
            try
            {
                if (string.IsNullOrWhiteSpace(address.Address)
                || string.IsNullOrWhiteSpace(address.City)
                || string.IsNullOrWhiteSpace(address.StateCode)
                || string.IsNullOrWhiteSpace(address.CountryCode)
                || string.IsNullOrWhiteSpace(address.ZipCode))
                {
                    return response;
                }
                var state = Context.DataContext.MstStates.FirstOrDefault(t => t.Code == address.StateCode || t.Name == address.StateCode);
                if (state != null && (state.MstCountry.Code == address.CountryCode || state.MstCountry.Name == address.CountryCode))
                {
                    var fullAddress = $"{address.Address.Trim()} {address.City.Trim()} {address.StateCode} {address.CountryCode} {address.ZipCode.Trim()}";
                    response = IsValidAddress(fullAddress);
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsValidFullAddress", ex.Message, ex);
            }

            return response;
        }

        public bool IsValidPONumber(int fuelRequestId, string poNumber, int companyId, bool isCounterOffer = false)
        {
            var response = false;

            try
            {
                if (isCounterOffer)
                {
                    return true;
                }

                var fuelRequest = Context.DataContext.FuelRequests.Any(t => t.Id != fuelRequestId && t.ExternalPoNumber == poNumber && t.Job.CompanyId == companyId
                                                && t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Open);
                if (!fuelRequest)
                {
                    var order = Context.DataContext.Orders.Any(t => t.PoNumber == poNumber && t.BuyerCompanyId == companyId);
                    if (!order)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsValidPONumber", ex.Message, ex);
            }

            return response;
        }

        public bool IsValidPONumberInOrder(int orderId, int companyId, string poNumber)
        {
            var response = false;
            try
            {
                var order = Context.DataContext.Orders.Any(t => t.Id != orderId && t.PoNumber == poNumber && t.BuyerCompanyId == companyId);
                if (!order)
                {
                    var fuelRequest = Context.DataContext.FuelRequests.Any(t => t.ExternalPoNumber == poNumber && t.Job.CompanyId == companyId
                                                && t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Open);
                    if (!fuelRequest)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsValidPONumberInOrder", ex.Message, ex);
            }

            return response;
        }

        public bool IsValidPONumberInTPO(string poNumber, string companyName)
        {
            var response = false;

            try
            {
                if (!string.IsNullOrWhiteSpace(companyName))
                {
                    var company = Context.DataContext.Companies.SingleOrDefault(t => t.Name.Equals(companyName.Trim()));
                    if (company != null)
                    {
                        return IsValidPONumber(0, poNumber, company.Id);
                    }
                    else
                    {
                        response = true;
                    }
                }
                else
                {
                    response = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsValidPONumberInTPO", ex.Message, ex);
            }

            return response;
        }

        public bool IsValidProductDisplayName(int productId, string DisplayName)
        {
            var response = false;

            try
            {
                var asset = Context.DataContext.MstTfxProducts.FirstOrDefault(t => t.Id != productId && t.Name.Equals(DisplayName, StringComparison.InvariantCultureIgnoreCase) && t.IsActive);
                response = (asset == null);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsValidProductDisplayName", ex.Message, ex);
            }

            return response;
        }

        public bool IsAssignedProductNameAlreadyExist(int supplierProductId, int companyId, string AssignedName)
        {
            var response = false;

            try
            {
                var assignName = Context.DataContext.SupplierMappedProductDetails.FirstOrDefault(t => t.Id != supplierProductId && t.CompanyId == companyId && t.MyProductId.Equals(AssignedName, StringComparison.InvariantCultureIgnoreCase) && t.IsActive);
                response = (assignName == null);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsAssignedProductNameAlreadyExist", ex.Message, ex);
            }

            return response;
        }

        public bool IsTerminalNameAlreadyMapped(int terminalId, int companyId, List<ProductMappingFuelTypeDetailsViewModel> productMappingFuelTypeDetails, out string terminalName)
        {
            var response = false; terminalName = string.Empty; List<int> mappedSupplierProduct = null;
            try
            {
                terminalName = Context.DataContext.MstExternalTerminals.Where(x => x.Id == terminalId && x.IsActive).FirstOrDefault().Name;
                var productDetailsInfo = Context.DataContext.SupplierMappedProductDetails.Where(t => t.TerminalId == terminalId && t.CompanyId == companyId && t.IsActive).ToList();
                if (productDetailsInfo != null && productDetailsInfo.Count > 0)
                {
                    var supplierMappedFuelType = (from productDetails in Context.DataContext.SupplierMappedProductDetails
                                                  where (productDetails.CompanyId == companyId && productDetails.TerminalId == terminalId && productDetails.IsActive)
                                                  select productDetails.FuelTypeId).ToList();

                    if (supplierMappedFuelType != null)
                    {
                        mappedSupplierProduct = new List<int>();
                        foreach (var mappedFuel in productMappingFuelTypeDetails)
                        {
                            if (!supplierMappedFuelType.Contains(mappedFuel.FuelTypeId) || mappedFuel.Id > 0)
                            {
                                mappedSupplierProduct.Add(mappedFuel.FuelTypeId);
                            }
                        }

                        return !(productMappingFuelTypeDetails.Count == mappedSupplierProduct.Count);

                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsTerminalNameAlreadyMapped", ex.Message, ex);
            }

            return response;
        }
        public bool IsValidAssetName(int assetId, string assetName, int companyId, int assetType = (int)AssetType.Asset, int? jobId = null)
        {
            var response = false;

            try
            {
                var asset = Context.DataContext.Assets.FirstOrDefault(t => t.Id != assetId && t.Name.Equals(assetName, StringComparison.InvariantCultureIgnoreCase) && t.Company.Id == companyId && t.IsActive);
                if (asset != null && assetType == (int)AssetType.Tank && jobId != null)
                {
                    asset = Context.DataContext.JobXAssets.Where(t => t.Asset.Id != assetId && t.JobId == jobId && t.RemovedBy == null && t.Asset.Name.Equals(assetName, StringComparison.InvariantCultureIgnoreCase) && t.Asset.IsActive).Select(t => t.Asset).FirstOrDefault();
                }
                response = (asset == null);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsValidAssetName", ex.Message, ex);
            }

            return response;
        }


        public bool IsValidSupplierListName(string name, int companyId)
        {
            var response = false;

            try
            {
                var listName = Context.DataContext.PrivateSupplierLists.FirstOrDefault(t => t.Name.ToLower() == name.ToLower() && t.IsActive && t.CompanyId == companyId);
                response = (listName == null);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsValidSupplierListName", ex.Message, ex);
            }

            return response;
        }

        public int GetCountryFromState(int stateId)
        {
            var response = 0;

            try
            {
                var state = Context.DataContext.MstStates.FirstOrDefault(t => t.Id == stateId);
                if (state != null)
                {
                    response = state.CountryId;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetCountrFromState", ex.Message, ex);
            }

            return response;
        }

        public async Task<StateViewModel> GetState(string stateName)
        {
            var response = new StateViewModel();
            try
            {
                var state = await Context.DataContext.MstStates.FirstOrDefaultAsync(t => t.Name.ToLower() == stateName.ToLower());
                if (state != null)
                {
                    response = new StateViewModel { Id = state.Id, Code = state.Code, Name = state.Name };
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetState", ex.Message, ex);
            }
            return response;
        }

        public List<string> GetUserNamesById(List<int> users)
        {
            List<string> response = new List<string>();
            try
            {
                var userList = Context.DataContext.Users.Where(t => users.Contains(t.Id)).ToList();
                userList.ForEach(t => response.Add($"{t.FirstName} {t.LastName}"));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetUserNamesById", ex.Message, ex);
            }
            return response;
        }

        public string GetPoNumber(FuelRequest fuelRequest, bool isProFormaPoEnabled, int orderId)
        {
            var response = string.Empty;
            bool isValidPo = false;
            try
            {
                int companyId = 0;
                companyId = fuelRequest.User.CompanyId.Value;
                //companyId = fuelRequest.Job.CompanyId;

                if (isProFormaPoEnabled && fuelRequest.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest)
                {
                    int proFormaPoCount = Context.DataContext.Orders.Where(t => t.BuyerCompanyId == companyId && t.FuelRequest.JobId == fuelRequest.JobId && t.IsProFormaPo).Count();
                    response = ApplicationConstants.ProFormaPoNumberPrefix + "_" + fuelRequest.Job.Name + "_" + proFormaPoCount;
                    isValidPo = IsValidPONumber(0, response, companyId);
                }
                else if (!string.IsNullOrEmpty(fuelRequest.ExternalPoNumber))
                {
                    response = fuelRequest.ExternalPoNumber;
                    isValidPo = true;
                }
                else
                {
                    response = ApplicationConstants.PoNumberPrefix + orderId.ToString().PadLeft(7, '0');
                    isValidPo = IsValidPONumber(0, response, companyId);
                }

                if (!isValidPo)
                {
                    string randomString = GetRandomString();
                    response = response + "_" + randomString;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetPoNumber", ex.Message, ex);
            }
            return response;
        }

        public bool GetDropImageRequiredFromOrder(int orderId)
        {
            var response = false;
            try
            {
                var order = Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => new { t.FuelRequest.FuelRequestDetail.IsDropImageRequired }).FirstOrDefault();
                if (order != null)
                {
                    response = order.IsDropImageRequired;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetDropImageRequiredFromOrder", ex.Message, ex);
            }
            return response;
        }

        public List<int> GetCompatibleProducts(List<int> productTypeIds)
        {
            var response = new List<int>();
            try
            {
                response.AddRange(productTypeIds);
                var productTypeMappings = Context.DataContext.ProductTypeCompatibilityMappings.Where(t => productTypeIds.Contains(t.ProductTypeId)).Select(t => t.MappedToProductTypeId).ToList();
                if (productTypeMappings != null)
                {
                    response.AddRange(productTypeMappings);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetCompatibleProducts", ex.Message, ex);
            }
            response = response.Distinct().ToList();
            return response;
        }

        public bool GetDropImageRequired(int supplierCompanyId)
        {
            var response = false;
            try
            {
                var onboardingPreferences = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == supplierCompanyId && t.IsActive).OrderByDescending(t => t.Id).Select(t => new { t.IsDropTicketImageRequired }).FirstOrDefault();
                if (onboardingPreferences != null)
                {
                    response = onboardingPreferences.IsDropTicketImageRequired;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetDropImageRequired", ex.Message, ex);
            }
            return response;
        }

        public string GetUserNameById(int userId)
        {
            string response = string.Empty;
            try
            {
                var user = Context.DataContext.Users.FirstOrDefault(t => t.Id == userId);
                if (user != null)
                {
                    response = $"{user.FirstName} {user.LastName}";
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetUserNamesById", ex.Message, ex);
            }
            return response;
        }

        public string GetRandomString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[4];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }

        public async Task<ContactPersonViewModel> GetPoContactAsync(int userId)
        {
            var response = new ContactPersonViewModel();
            try
            {
                var user = await Context.DataContext.Users.Include(t => t.Company).Where(t => t.Id == userId)
                            .Select(t => new { t.FirstName, t.LastName, t.Email, t.PhoneNumber, t.CompanyId, CompanyName = t.Company.Name })
                            .SingleOrDefaultAsync();
                if (user != null)
                {
                    response = new ContactPersonViewModel
                    {
                        Name = $"{user.FirstName} {user.LastName}",
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        CompanyName = user.CompanyName,
                        CompanyId = user.CompanyId ?? 0
                    };
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetPoContactAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<ContactPersonViewModel> GetPoContactForInvoiceAsync(int invoiceId)
        {
            var response = new ContactPersonViewModel();
            try
            {
                var invoiceDetails = await Context.DataContext.InvoiceXAdditionalDetails.SingleOrDefaultAsync(t => t.InvoiceId == invoiceId);
                if (invoiceDetails != null)
                {
                    response = new ContactPersonViewModel
                    {
                        Name = invoiceDetails.PoContactName,
                        Email = invoiceDetails.PoContactEmail,
                        PhoneNumber = invoiceDetails.PoContactPhoneNumber,
                    };
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetPoContactForInvoiceAsync", ex.Message, ex);
            }
            return response;
        }

        public int GetClosestTerminalId(decimal latitude, decimal longitude, int stateId)
        {
            using (var tracer = new Tracer("HelperDomain", "GetClosestTerminalId"))
            {
                var response = 0;
                try
                {
                    var state = Context.DataContext.MstStates.FirstOrDefault(t => t.Id == stateId);
                    if (latitude != 0 && longitude != 0 && state != null)
                    {
                        var terminalList = Context.DataContext.MstExternalTerminals.Where(t => t.StateCode == state.Code && state.MstCountry.Code.Equals(t.CountryCode, StringComparison.OrdinalIgnoreCase)).ToList();
                        if (terminalList == null || !terminalList.Any())
                        {
                            terminalList = Context.DataContext.MstExternalTerminals.ToList();
                        }

                        if (terminalList != null && terminalList.Any())
                        {
                            var terminalListWithDistance = terminalList.Select(t => new
                            {
                                TerminalId = t.Id,
                                TerminalDistance = CalculateDistance(latitude, longitude, t.Latitude, t.Longitude)
                            }).OrderBy(t => t.TerminalDistance).ToList();

                            var closestTerminal = terminalListWithDistance.FirstOrDefault();
                            if (closestTerminal != null)
                            {
                                response = closestTerminal.TerminalId;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("HelperDomain", "GetClosestTerminalId", ex.Message, ex);
                }

                return response;
            }
        }

        public bool IsHidePricingEnabled(Invoice invoice, CompanyType companyType)
        {
            var response = false;
            try
            {
                if (invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    if (companyType == CompanyType.Supplier)
                    {
                        response = invoice.Order == null ? false :
                                                                                (invoice.Order.OrderXTogglePricingDetail == null ? false :
                                                                                invoice.Order.OrderXTogglePricingDetail.IsHidePricingEnabledForSupplier);
                    }
                    else
                    {
                        response = invoice.Order == null ? false :
                                                                                (invoice.Order.OrderXTogglePricingDetail == null ? false :
                                                                                invoice.Order.OrderXTogglePricingDetail.IsHidePricingEnabledForBuyer);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsHidePricingEnabled", ex.Message, ex);
            }
            return response;
        }

        public Tuple<int, string> GetCreditInvoice(int? originalInvoiceId)
        {
            if (!originalInvoiceId.HasValue || originalInvoiceId == 0)
                return null;
            try
            {
                var creditInvoice = Context.DataContext.InvoiceXAdditionalDetails.FirstOrDefault(t => t.OriginalInvoiceId.HasValue &&
                                    t.OriginalInvoiceId == originalInvoiceId
                                    && t.Invoice.InvoiceTypeId == (int)InvoiceType.CreditInvoice);
                if (creditInvoice != null)
                {
                    return Tuple.Create(creditInvoice.InvoiceId, creditInvoice.Invoice.DisplayInvoiceNumber);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetCreditInvoiceId", ex.Message, ex);
            }
            return null;
        }

        public double CalculateDistance(decimal srcLatitude, decimal srcLongitude, decimal destLatitude, decimal destLongitude)
        {
            double response = 0;
            try
            {
                if (srcLatitude != 0 && srcLongitude != 0 && destLatitude != 0 && destLongitude != 0)
                {
                    double DegToRad = 57.29577951, Ans = 0;
                    Ans = Math.Sin((double)srcLatitude / DegToRad) * Math.Sin((double)destLatitude / DegToRad)
                        + Math.Cos((double)srcLatitude / DegToRad) * Math.Cos((double)destLatitude / DegToRad)
                        * Math.Cos(Math.Abs((double)destLongitude - (double)srcLongitude) / DegToRad);

                    Ans = Math.Round(Ans, 6);
                    response = 3959 * Math.Atan(Math.Sqrt(1 - Math.Pow(Ans, 2)) / Ans);
                    response = Math.Abs(response);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "CalculateDistance", ex.Message, ex);
            }
            return response;
        }

        public string GetPrice(int fuelRequestId)
        {
            var response = string.Empty;
            try
            {
                var fuelRequest = Context.DataContext.FuelRequests.FirstOrDefault(t => t.Id == fuelRequestId);
                if (fuelRequest != null)
                {
                    response = fuelRequest.FuelRequestPricingDetail.DisplayPrice;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetPrice", ex.Message, ex);
            }
            return response;
        }

        public string GetInvoicePrice(int invoiceId)
        {
            var response = string.Empty;
            try
            {
                var invoice = Context.DataContext.Invoices.Where(t => t.Id == invoiceId && t.OrderId != null).Select(t => new
                {
                    //t.PricePerGallon,
                    //t.RackPrice,
                    //t.Order.FuelRequest.PricingTypeId,
                    //t.Order.FuelRequest.RackAvgTypeId,
                    t.InvoiceTypeId,
                    //fuelRequestPPG = t.Order.FuelRequest.PricePerGallon,
                    t.Order.FuelRequest.FuelRequestPricingDetail
                }).FirstOrDefault();

                if (invoice != null && invoice.FuelRequestPricingDetail != null)
                {
                    response = invoice.FuelRequestPricingDetail.DisplayPrice;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetInvoicePrice", ex.Message, ex);
            }
            return response;
        }

        public string GetResalePrice(int fuelRequestId)
        {
            var response = string.Empty;
            try
            {
                var fuelRequest = Context.DataContext.FuelRequests.FirstOrDefault(t => t.Id == fuelRequestId);
                if (fuelRequest != null)
                {
                    var resale = fuelRequest.Resales.FirstOrDefault();
                    if (resale != null)
                    {
                        response = GetPricePerGallon(resale.PricePerGallon, resale.PricingTypeId, resale.RackAvgTypeId ?? 0);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetResalePrice", ex.Message, ex);
            }
            return response;
        }

        public string GetPricePerGallon(FuelRequest fuelRequest)
        {
            string price = string.Empty;
            try
            {
                //price = GetPricePerGallon(fuelRequest.PricePerGallon, fuelRequest.PricingTypeId, fuelRequest.RackAvgTypeId ?? 0);
                if (fuelRequest.FuelRequestPricingDetail != null)
                    price = fuelRequest.FuelRequestPricingDetail.DisplayPrice;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetPricePerGallon", ex.Message, ex);
            }
            return price;
        }

        public string GetQuantityRequested(decimal quantity)
        {
            string response = string.Empty;
            try
            {
                response = quantity == ApplicationConstants.QuantityNotSpecified ? Resource.lblNotSpecified : quantity.GetPreciseValue(2).GetCommaSeperatedValue();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetQuantityRequested", ex.Message, ex);
            }
            return response;
        }

        public string GetUOM(int frUom)
        {
            string response = string.Empty;
            try
            {
                switch (frUom)
                {
                    case 1:
                        return UoM.Gallons.GetDisplayName();

                    case 2:
                        return UoM.Litres.GetDisplayName();

                    case 3:
                        return UoM.Barrels.GetDisplayName();

                    case 4:
                        return UoM.MetricTons.GetDisplayName();

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetUOM", ex.Message, ex);
            }
            return response;
        }

        public string GetProductName(MstProduct product)
        {
            string response = string.Empty;
            try
            {
                response = product.TfxProductId.HasValue ? product.MstTFXProduct.Name : product.Name;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetProductName", ex.Message, ex);
            }
            return response;
        }

        public string GetProductNameForDriver(int companyId, int? terminalId, bool isDriverProdutDisplayEnabled, MstProduct product)
        {
            string response = string.Empty;
            try
            {
                bool isAssignedNameValid = false;
                if (terminalId.HasValue && isDriverProdutDisplayEnabled)
                {
                    var driverProductId = Context.DataContext.SupplierMappedProductDetails
                                                        .Where(t => t.CompanyId == companyId && t.TerminalId == terminalId.Value && t.FuelTypeId == product.TfxProductId)
                                                        .OrderByDescending(t => t.Id)
                                                        .Select(t => t.DriverProductId)
                                                        .FirstOrDefault();

                    if (driverProductId != null)
                    {
                        response = driverProductId;
                        isAssignedNameValid = true;
                    }
                }

                if (!isAssignedNameValid)
                {
                    response = product.TfxProductId.HasValue ? product.MstTFXProduct.Name : product.Name;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetProductNameForDriver", ex.Message, ex);
            }
            return response;
        }

        public string GetQuantityRequested(decimal? brokeredMaxQuantity, decimal fuelRequestMaxQuantity)
        {
            string response = string.Empty;
            try
            {
                decimal quantity = brokeredMaxQuantity ?? fuelRequestMaxQuantity;
                response = quantity == ApplicationConstants.QuantityNotSpecified ? Resource.lblNotSpecified : quantity.GetPreciseValue(2).GetCommaSeperatedValue();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetQuantityRequested", ex.Message, ex);
            }
            return response;
        }

        public string CheckQuantityValid(decimal quantity, decimal? amount)
        {
            string response = string.Empty;
            try
            {
                response = (amount == null || quantity == (int)ApplicationConstants.QuantityNotSpecified) ? Resource.lblHyphen : amount.Value.GetPreciseValue(2).GetCommaSeperatedValue();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "CheckQuantityValid", ex.Message, ex);
            }
            return response;
        }

        public string GetPricePerGallon(decimal pricePerGallon, int pricingTypeId, int rackAvgTypeId)
        {
            string price = string.Empty;
            try
            {
                if (pricingTypeId == (int)PricingType.Tier)
                {
                    price = Resource.lblTier;
                }
                else
                if (pricingTypeId == (int)PricingType.PricePerGallon)
                {
                    price = Resource.constSymbolCurrency + pricePerGallon.GetPreciseValue(4);
                }
                else if (pricingTypeId == (int)PricingType.Suppliercost)
                {
                    switch (rackAvgTypeId)
                    {
                        case (int)RackPricingType.PlusPercent:
                            price = $"{Resource.lblFuelCostPlus} {pricePerGallon.GetPreciseValue(4)}{Resource.constSymbolPercent}";
                            break;
                        case (int)RackPricingType.MinusPercent:
                            price = $"{Resource.lblFuelCostMinus} {pricePerGallon.GetPreciseValue(4)}{Resource.constSymbolPercent}";
                            break;
                        case (int)RackPricingType.PlusDollar:
                            price = $"{Resource.lblFuelCostPlus} {Resource.constSymbolCurrency}{pricePerGallon.GetPreciseValue(4)}";
                            break;
                        case (int)RackPricingType.MinusDollar:
                            price = $"{Resource.lblFuelCostMinus} {Resource.constSymbolCurrency}{pricePerGallon.GetPreciseValue(4)}";
                            break;
                    }
                }
                else
                {
                    var rackText = pricingTypeId == (int)PricingType.RackHigh ? Resource.lblRackHigh : pricingTypeId == (int)PricingType.RackLow ? Resource.lblRackLow : Resource.lblRackAverage;

                    switch (rackAvgTypeId)
                    {
                        case (int)RackPricingType.PlusPercent:
                            price = $"{rackText} + {pricePerGallon.GetPreciseValue(4)}{Resource.constSymbolPercent}";
                            break;
                        case (int)RackPricingType.MinusPercent:
                            price = $"{rackText} - {pricePerGallon.GetPreciseValue(4)}{Resource.constSymbolPercent}";
                            break;
                        case (int)RackPricingType.PlusDollar:
                            price = $"{rackText} + {Resource.constSymbolCurrency}{pricePerGallon.GetPreciseValue(4)}";
                            break;
                        case (int)RackPricingType.MinusDollar:
                            price = $"{rackText} - {Resource.constSymbolCurrency}{pricePerGallon.GetPreciseValue(4)}";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetPricePerGallon", ex.Message, ex);
            }
            return price;
        }

        public decimal GetPricePerGallonForQb(decimal pricePerGallon, int pricingTypeId, decimal creationTimeRackPPG, decimal supplierCost, int rackAvgTypeId)
        {
            decimal price = 0;
            try
            {
                if (pricingTypeId == (int)PricingType.PricePerGallon)
                {
                    price = pricePerGallon;
                }
                else if (pricingTypeId == (int)PricingType.Suppliercost)
                {
                    switch (rackAvgTypeId)
                    {
                        case (int)RackPricingType.PlusPercent:
                            price = supplierCost + (supplierCost / 100 * pricePerGallon);
                            break;
                        case (int)RackPricingType.MinusPercent:
                            price = supplierCost - (supplierCost / 100 * pricePerGallon);
                            break;
                        case (int)RackPricingType.PlusDollar:
                            price = supplierCost + pricePerGallon;
                            break;
                        case (int)RackPricingType.MinusDollar:
                            price = supplierCost - pricePerGallon;
                            break;
                        default:
                            price = supplierCost;
                            break;

                    }
                }
                else
                {
                    switch (rackAvgTypeId)
                    {
                        case (int)RackPricingType.PlusPercent:
                            price = creationTimeRackPPG + (creationTimeRackPPG / 100 * pricePerGallon);
                            break;
                        case (int)RackPricingType.MinusPercent:
                            price = creationTimeRackPPG - (creationTimeRackPPG / 100 * pricePerGallon);
                            break;
                        case (int)RackPricingType.PlusDollar:
                            price = creationTimeRackPPG + pricePerGallon;
                            break;
                        case (int)RackPricingType.MinusDollar:
                            price = creationTimeRackPPG - pricePerGallon;
                            break;
                        default:
                            price = creationTimeRackPPG;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetPricePerGallonForQb", ex.Message, ex);
            }
            return price;
        }

        public UserRoles GetUserRole(SiteFuelUserFilterType filter)
        {
            var response = new UserRoles();
            try
            {
                if (filter == SiteFuelUserFilterType.AllSuperAdmin)
                {
                    return UserRoles.SuperAdmin;
                }
                else if (filter == SiteFuelUserFilterType.InternalSalesPerson)
                {
                    return UserRoles.InternalSalesPerson;
                }
                else if (filter == SiteFuelUserFilterType.ExternalVendor)
                {
                    return UserRoles.ExternalVendor;
                }
                else if (filter == SiteFuelUserFilterType.AccountSpecialist)
                {
                    return UserRoles.AccountSpecialist;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetUserRole", ex.Message, ex);
            }
            return response;
        }

        public List<string> GetRoleNamesById(List<int> roleIds)
        {
            List<string> response = new List<string>();
            try
            {
                response = Context.DataContext.MstRoles.Where(t => roleIds.Contains(t.Id)).Select(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetRoleNamesById", ex.Message, ex);
            }
            return response;
        }

        public List<string> GetQualificationNamesById(List<int> qualificationIds)
        {
            List<string> response = new List<string>();
            try
            {
                response = Context.DataContext.MstSupplierQualifications.Where(t => qualificationIds.Contains(t.Id)).Select(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetQualificationNamesById", ex.Message, ex);
            }
            return response;
        }

        public string GetDisadvantageBusinessEnterprise(List<MstSupplierQualification> qualifications)
        {
            try
            {
                if (qualifications != null && qualifications.Count > 0)
                {
                    return string.Join(",", qualifications.Select(t1 => t1.Name[0]));
                }
                else
                {
                    return Resource.lblHyphen;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetDisadvantageBusinessEnterprise", ex.Message, ex);
            }
            return string.Empty;
        }

        public Nullable<decimal> GetOrderTotalAmount(int pricingTypeId, int quantityTypeId, decimal gallonsOrdered, decimal pricePerGallon)
        {
            decimal? totalAmount = null;
            try
            {
                if (pricingTypeId != (int)PricingType.PricePerGallon || quantityTypeId == (int)QuantityType.NotSpecified)
                {
                    return null;
                }
                else
                {
                    totalAmount = gallonsOrdered * pricePerGallon;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrderTotalAmount", ex.Message, ex);
            }

            return totalAmount;
        }

        public string GetFuelDeliveredPercentage(Order order, decimal currentDrop = 0)
        {
            string response = Resource.lblHyphen;
            try
            {
                if (order.FuelRequest.QuantityTypeId != (int)QuantityType.NotSpecified)
                {
                    var deliveredPercentage = ((order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice)
                        .Sum(t => t.DroppedGallons) + currentDrop) / (order.FuelRequest.MaxQuantity == 0 ? 1 : order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity)) * 100;
                    response = deliveredPercentage.GetPreciseValue(2).GetCommaSeperatedValue() + Resource.constSymbolPercent;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetFuelDeliveredPercentage", ex.Message, ex);
            }

            return response;
        }

        public string GetFuelDeliveredPercentagePerInvoice(Invoice invoice)
        {
            string response = Resource.lblHyphen;
            try
            {
                if (invoice.Order != null && invoice.Order.FuelRequest.QuantityTypeId != (int)QuantityType.NotSpecified)
                {
                    decimal deliveredPercentage = 0;
                    deliveredPercentage = (invoice.DroppedGallons / (invoice.Order.FuelRequest.MaxQuantity == 0 ? 1 : invoice.Order.BrokeredMaxQuantity ?? invoice.Order.FuelRequest.MaxQuantity)) * 100;
                    response = deliveredPercentage.GetPreciseValue(2) + Resource.constSymbolPercent;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetFuelDeliveredPercentagePerInvoice", ex.Message, ex);
            }
            return response;
        }

        public decimal GetAvgGallonsPerDelivery(Order order)
        {
            decimal response = 0;
            decimal gallonsDelivered = 0;
            try
            {
                gallonsDelivered = order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice).Sum(t => t.DroppedGallons);
                var totalDrops = order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice).Count(t => t.InvoiceTypeId != (int)InvoiceType.DryRun);
                if (totalDrops > 0)
                {
                    response = gallonsDelivered / totalDrops;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetAvgGallonsPerDelivery", ex.Message, ex);
            }
            return response;
        }

        public decimal GetAverageFuelDropPercentagePerOrder(Order order)
        {
            decimal percentDelivered = 0;
            try
            {
                var quantityDelivered = order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice).Sum(t => t.DroppedGallons);
                if (order.FuelRequest.MaxQuantity > 0)
                {
                    percentDelivered = GetFuelDropPercentage(quantityDelivered, order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetAverageFuelDropPercentagePerOrder", ex.Message, ex);
            }
            return percentDelivered;
        }

        public decimal GetFuelDropPercentage(decimal droppedQuantity, decimal maxQuantity)
        {
            decimal percentDelivered = 0;
            try
            {
                if (maxQuantity > 0)
                {
                    percentDelivered = droppedQuantity / maxQuantity * 100;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetAverageFuelDropPercentagePerOrder", ex.Message, ex);
            }
            return percentDelivered;
        }

        public decimal GetAvgPricePerGallon(Order order)
        {
            decimal response = 0;
            try
            {
                var pricePerGallon = order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice).Sum(t => t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail.PricePerGallon).FirstOrDefault());
                var totalDrops = order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice).Count(t => t.InvoiceTypeId != (int)InvoiceType.DryRun);
                if (totalDrops > 0 && pricePerGallon > 0)
                {
                    response = pricePerGallon / totalDrops;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetAvgPricePerGallon", ex.Message, ex);
            }
            return response;
        }

        public decimal GetInvoiceAmount(Invoice invoice)
        {
            decimal response = 0;
            try
            {
                if (invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual &&
                    invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    response = invoice.InvoiceHeader.TotalBasicAmount + invoice.InvoiceHeader.TotalTaxAmount - invoice.InvoiceHeader.TotalDiscountAmount;
                }
                else
                {
                    response = invoice.InvoiceHeader.TotalBasicAmount;
                }
                if (invoice.InvoiceTypeId != (int)InvoiceType.DryRun)
                {
                    response += invoice.InvoiceHeader.TotalFeeAmount;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetInvoiceAmount", ex.Message, ex);
            }

            return response;
        }

        public decimal GetInvoiceAmount(int invoiceTypeId, decimal basicAmount, decimal totalTaxAmount, decimal totalDiscountAmount, decimal? totalFeeAmount)
        {
            decimal response = 0;
            try
            {
                if (invoiceTypeId != (int)InvoiceType.DigitalDropTicketManual &&
                    invoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    response = basicAmount + totalTaxAmount - totalDiscountAmount;
                }
                else
                {
                    response = basicAmount;
                }
                if (invoiceTypeId != (int)InvoiceType.DryRun)
                {
                    response += totalFeeAmount ?? 0;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetInvoiceAmount", ex.Message, ex);
            }

            return response;
        }

        public int GetPoContactId(Order order)
        {
            var poContactId = 0;
            try
            {
                if (order.FuelRequest.FuelRequestDetail.PoContactId != null)
                {
                    poContactId = order.FuelRequest.FuelRequestDetail.PoContactId.Value;
                }
                else
                {
                    poContactId = order.FuelRequest.Job.PoContactId ?? order.FuelRequest.CreatedBy;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetPoContactId", ex.Message, ex);
            }
            return poContactId;
        }

        public decimal GetInvoiceTotalFees(Invoice invoice)
        {
            decimal response = 0;
            try
            {
                return invoice.TotalFeeAmount ?? 0;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetInvoiceTotalFees", ex.Message, ex);
            }

            return response;
        }

        public decimal SetCalculatedInvoiceFeesTotal(Invoice invoice)
        {
            decimal response = 0;
            try
            {
                decimal droppedGallons = invoice.DroppedGallons;
                foreach (FuelFee feeDetails in invoice.FuelRequestFees.Where(t => t.DiscountLineItemId == null))
                {
                    feeDetails.InvoiceId = invoice.Id;

                    if (feeDetails.FeeTypeId == (int)FeeType.DeliveryFee || feeDetails.FeeTypeId == (int)FeeType.OtherFee)
                    {
                        if (feeDetails.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                        {
                            var hourlyFeeDetail = HourlyFeeCalculation(invoice, feeDetails);
                            feeDetails.TotalFee = hourlyFeeDetail.Fee;
                        }
                        else
                        {
                            feeDetails.TotalFee = GetDeliveryFee(invoice, droppedGallons, feeDetails);
                        }
                        response = response + feeDetails.TotalFee.Value;
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.WetHoseFee)
                    {
                        invoice.IsWetHosingDelivery = true;
                        feeDetails.TotalFee = CalucateHourlyOrPerAssetFee(invoice, feeDetails).Fee;
                        response = response + feeDetails.TotalFee.Value;
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.SurchargeFreightFee || feeDetails.FeeTypeId == (int)FeeType.FreightCost)
                    {
                        feeDetails.TotalFee = feeDetails.TotalFee;
                        response = response + feeDetails.TotalFee.Value;
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.OverWaterFee)
                    {
                        invoice.IsOverWaterDelivery = true;
                        feeDetails.TotalFee = CalucateHourlyOrPerAssetFee(invoice, feeDetails).Fee;
                        response = response + feeDetails.TotalFee.Value;
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.FreightFee)
                    {
                        feeDetails.TotalFee = GetFreightFee(invoice, feeDetails);
                        response = response + feeDetails.TotalFee.Value;
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.UnderGallonFee && (feeDetails.MinimumGallons ?? 0) > invoice.DroppedGallons)
                    {
                        feeDetails.TotalFee = feeDetails.Fee;
                        response = response + feeDetails.TotalFee.Value;
                        feeDetails.FeeSubQuantity = 1;
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.EnvironmentalFee || feeDetails.FeeTypeId == (int)FeeType.LoadFee || feeDetails.FeeTypeId == (int)FeeType.AdditiveFee || feeDetails.FeeTypeId == (int)FeeType.ProcessingFee)
                    {
                        feeDetails.TotalFee = GetFlatPerGallonFee(feeDetails, droppedGallons);
                        response = response + feeDetails.TotalFee.Value;
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.ServiceFee || feeDetails.FeeTypeId == (int)FeeType.SurchargeFee
                        || feeDetails.FeeTypeId == (int)FeeType.PumpCharge || feeDetails.FeeTypeId == (int)FeeType.SplitTank
                        || feeDetails.FeeTypeId == (int)FeeType.StopOffFee)
                    {
                        if (feeDetails.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                        {
                            var hourlyFeeDetail = HourlyFeeCalculation(invoice, feeDetails);
                            feeDetails.TotalFee = hourlyFeeDetail.Fee;
                        }
                        else
                        {
                            feeDetails.TotalFee = GetServiceSurchageFee(droppedGallons, feeDetails);
                        }
                        response = response + feeDetails.TotalFee.Value;
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.DemurrageFeeDestination || feeDetails.FeeTypeId == (int)FeeType.DemurrageFeeTerminal || feeDetails.FeeTypeId == (int)FeeType.DemurrageOther)
                    {
                        CalculateAndSetDemurrageFee(feeDetails);
                        response = response + feeDetails.TotalFee.Value;
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.Retain)
                    {
                        CalculateAndSetFuelTruckRetainFee(feeDetails);
                        response = response + feeDetails.TotalFee.Value;
                    }
                    if (feeDetails.IncludeInPPG && feeDetails.TotalFee.HasValue)
                    {
                        SetInvoiceFee(invoice, feeDetails.TotalFee.Value);
                        response -= feeDetails.TotalFee.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "SetCalculatedInvoiceFeesTotal", ex.Message, ex);
            }

            return response;
        }

        private static void CalculateAndSetDemurrageFee(FuelFee fee)
        {
            if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate && fee.EndTime.HasValue && fee.StartTime.HasValue)
            {
                var difference = fee.EndTime.Value.AddMilliseconds(-fee.EndTime.Value.Millisecond)
                                 .Subtract(fee.StartTime.Value.AddMilliseconds(-fee.StartTime.Value.Millisecond)).TotalSeconds;

                var calculatedDifference = CalcualateDifferenceAndSetTotalFee(fee, (decimal)difference);
                fee.FeeSubQuantity = calculatedDifference;
            }
            else if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate && fee.FeeSubQuantity.HasValue && fee.FeeSubQuantity.Value > 0)
            {
                var difference = CalcualateDifferenceAndSetTotalFee(fee, fee.FeeSubQuantity.Value);
                fee.FeeSubQuantity = difference;
            }
        }

        private static void CalculateAndSetFuelTruckRetainFee(FuelFee fee)
        {
            decimal difference = 0;
            if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate && fee.EndTime.HasValue && fee.StartTime.HasValue)
            {
                difference = (decimal)fee.EndTime.Value.AddMilliseconds(-fee.EndTime.Value.Millisecond)
                                 .Subtract(fee.StartTime.Value.AddMilliseconds(-fee.StartTime.Value.Millisecond)).TotalSeconds;
            }
            else if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate && fee.FeeSubQuantity.HasValue && fee.FeeSubQuantity.Value > 0)
            {
                difference = fee.FeeSubQuantity.Value;
            }

            if (difference > 0)
                fee.TotalFee = fee.Fee * Convert.ToDecimal(difference) / 3600;
            else
                fee.TotalFee = 0;

            fee.FeeSubQuantity = difference;
        }

        private static decimal CalcualateDifferenceAndSetTotalFee(FuelFee fee, decimal difference)
        {
            if (fee.WaiveOffTime.HasValue)
            {
                var waiveOffInSeconds = fee.WaiveOffTime.Value * 60;
                if (difference > waiveOffInSeconds)
                    difference = difference - waiveOffInSeconds;
                else
                    difference = 0;
            }

            if (difference > 0)
                fee.TotalFee = fee.Fee * Convert.ToDecimal(difference) / 3600;
            else
                fee.TotalFee = 0;

            return difference;
        }

        public decimal SetCalculatedInvoiceDiscountFeesTotal(Invoice invoice, decimal totalFeeAmount)
        {
            decimal response = 0;
            try
            {
                var feeDetail = invoice.FuelRequestFees.Where(t => t.DiscountLineItemId == null);
                foreach (FuelFee discountfeeDetails in invoice.FuelRequestFees.Where(t => t.DiscountLineItemId != null))
                {
                    if (discountfeeDetails.FeeTypeId != (int)FeeType.SubTotal)
                    {
                        var totalFee = feeDetail.Where(t => t.FeeTypeId == discountfeeDetails.FeeTypeId).Sum(t1 => t1.TotalFee);
                        if (totalFee.HasValue)
                        {
                            discountfeeDetails.InvoiceId = invoice.Id;
                            if (discountfeeDetails.FeeSubTypeId == (int)FeeSubType.Percent)
                            {
                                discountfeeDetails.TotalFee = totalFee.Value / 100 * discountfeeDetails.Fee;
                            }
                            else if (discountfeeDetails.FeeSubTypeId == (int)FeeSubType.FlatFee)
                            {
                                discountfeeDetails.TotalFee = discountfeeDetails.Fee;
                            }
                            response = response + discountfeeDetails.TotalFee.Value;
                        }
                    }
                    else
                    {
                        var subTotal = totalFeeAmount + invoice.BasicAmount;
                        discountfeeDetails.InvoiceId = invoice.Id;
                        if (discountfeeDetails.FeeSubTypeId == (int)FeeSubType.Percent)
                        {
                            discountfeeDetails.TotalFee = subTotal / 100 * discountfeeDetails.Fee;
                        }
                        else if (discountfeeDetails.FeeSubTypeId == (int)FeeSubType.FlatFee)
                        {
                            discountfeeDetails.TotalFee = discountfeeDetails.Fee;
                        }
                        response = response + discountfeeDetails.TotalFee.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "SetCalculatedInvoiceDiscountFeesTotal", ex.Message, ex);
            }

            return response;
        }

        private void SetInvoiceFee(Invoice response, decimal fee)
        {
            response.BasicAmount += Math.Round(fee, 6);
            var ftlDetail = response.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail).FirstOrDefault();
            ftlDetail.PricePerGallon = Math.Round(response.BasicAmount / response.DroppedGallons, 6);
        }

        private static decimal GetFreightFee(Invoice invoice, FuelFee feeDetails)
        {
            decimal response;
            if (feeDetails.FeeSubTypeId == (int)FeeSubType.ByAssetCount)
            {
                response = (feeDetails.Fee * invoice.AssetDrops.Count(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop));
                feeDetails.FeeSubQuantity = invoice.AssetDrops.Count(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop);
            }
            else if (feeDetails.FeeSubTypeId == (int)FeeSubType.PerGallon)
            {
                response = (feeDetails.Fee * invoice.DroppedGallons);
                feeDetails.FeeSubQuantity = invoice.DroppedGallons;
            }
            else
            {
                response = feeDetails.Fee;
                feeDetails.FeeSubQuantity = 1;
            }
            return response;
        }

        private static decimal GetDeliveryFee(Invoice invoice, decimal droppedGallons, FuelFee feeDetails)
        {
            var response = 0.0M;
            if (feeDetails.FeeSubTypeId == (int)FeeSubType.FlatFee)
            {
                response = feeDetails.Fee;
                feeDetails.FeeSubQuantity = 1;
            }
            else if (feeDetails.FeeSubTypeId == (int)FeeSubType.PerGallon)
            {
                response = feeDetails.Fee * droppedGallons;
                feeDetails.FeeSubQuantity = invoice.DroppedGallons;
            }
            else if (feeDetails.FeeSubTypeId == (int)FeeSubType.ByQuantity)
            {
                var byQantity = feeDetails.FeeByQuantities.FirstOrDefault(t => droppedGallons >= t.MinQuantity && droppedGallons <= (t.MaxQuantity ?? droppedGallons));
                if (byQantity != null)
                {
                    response = byQantity.Fee;
                    feeDetails.FeeSubQuantity = 1;
                }
            }

            return response;
        }

        public static decimal GetFlatPerGallonFee(FuelFee feeDetails, decimal droppedGallons)
        {
            var response = 0.0M;
            if (feeDetails.FeeSubTypeId == (int)FeeSubType.FlatFee)
            {
                response = feeDetails.Fee;
                feeDetails.FeeSubQuantity = 1;
            }
            else if (feeDetails.FeeSubTypeId == (int)FeeSubType.PerGallon)
            {
                response = feeDetails.Fee * droppedGallons;
                feeDetails.FeeSubQuantity = droppedGallons;
            }

            return response;
        }

        public static decimal GetServiceSurchageFee(decimal droppedGallons, FuelFee feeDetails)
        {
            var response = 0.0M;
            if (feeDetails.FeeSubTypeId == (int)FeeSubType.FlatFee)
            {
                response = feeDetails.Fee;
                feeDetails.FeeSubQuantity = 1;
            }
            else if (feeDetails.FeeSubTypeId == (int)FeeSubType.PerGallon)
            {
                response = feeDetails.Fee * droppedGallons;
                feeDetails.FeeSubQuantity = droppedGallons;
            }

            return response;
        }

        public WetHoseOverWaterCalculationViewModel CalucateHourlyOrPerAssetFee(Invoice invoice, FuelFee feeDetails)
        {
            WetHoseOverWaterCalculationViewModel response = new WetHoseOverWaterCalculationViewModel();

            if (feeDetails != null && feeDetails.Fee > 0)
            {
                if (feeDetails.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                {
                    int hours = 0; double mins = 0; double difference = 0;
                    var applicationDomain = new ApplicationDomain(this);
                    var WetHoseOverWaterFeeChangedDate = applicationDomain.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingWetHoseFeeChangedDate);
                    var WetHoseOverWaterFeeChangedDateTime = DateTimeOffset.Parse(WetHoseOverWaterFeeChangedDate);
                    if (invoice.CreatedDate >= WetHoseOverWaterFeeChangedDateTime)
                    {

                        difference = invoice.DropEndDate.AddMilliseconds(-invoice.DropEndDate.Millisecond)
                                    .Subtract(invoice.DropStartDate.AddMilliseconds(-invoice.DropStartDate.Millisecond)).TotalSeconds;
                    }
                    else
                    {
                        if (invoice.AssetDrops.Any())
                        {
                            difference = invoice.AssetDrops.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop)
                                .Sum(t => t.DropEndDate.AddMilliseconds(-t.DropEndDate.Millisecond).Subtract(t.DropStartDate.AddMilliseconds(-t.DropStartDate.Millisecond)).TotalSeconds);
                        }
                        else
                        {
                            difference = invoice.DropEndDate.AddMilliseconds(-invoice.DropEndDate.Millisecond).Subtract(invoice.DropStartDate.AddMilliseconds(-invoice.DropStartDate.Millisecond)).TotalSeconds;
                        }
                    }

                    hours = (int)(difference / 3600);
                    mins = (difference - (hours * 3600)) / 60;
                    response.Fee = feeDetails.Fee * Convert.ToDecimal(difference) / 3600;
                    feeDetails.FeeSubQuantity = (decimal)difference;
                    if (feeDetails.FeeTypeId == (int)FeeType.WetHoseFee)
                    {
                        response.WetHoseHours = hours > 0 ? string.Format(Resource.constTimeFormat, hours, string.Format(Resource.constFormatDecimal2, mins))
                                                                        : string.Format(Resource.constMinuteFormat, string.Format(Resource.constFormatDecimal2, mins));
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.OverWaterFee)
                    {
                        response.OverWaterHours = hours > 0 ? string.Format(Resource.constTimeFormat, hours, string.Format(Resource.constFormatDecimal2, mins))
                                                                        : string.Format(Resource.constMinuteFormat, string.Format(Resource.constFormatDecimal2, mins));
                    }
                }
                else if (feeDetails.FeeSubTypeId == (int)FeeSubType.ByAssetCount)
                {
                    var assetCount = invoice.AssetDrops.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop).Select(t => t.JobXAssetId).Distinct().Count();
                    response.Fee = (feeDetails.Fee * (assetCount));
                    feeDetails.FeeSubQuantity = assetCount;
                    if (feeDetails.FeeTypeId == (int)FeeType.WetHoseFee)
                    {
                        response.WetHoseAssetQuantity = assetCount;
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.OverWaterFee)
                    {
                        response.OverWaterAssetQuantity = assetCount;
                    }
                }
            }

            return response;
        }

        public HourlyFeeCalculationViewModel HourlyFeeCalculation(Invoice invoice, FuelFee feeDetails)
        {
            HourlyFeeCalculationViewModel response = new HourlyFeeCalculationViewModel();

            if (feeDetails != null && feeDetails.Fee > 0)
            {
                if (feeDetails.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                {
                    int hours = 0; double mins = 0; double difference = 0;
                    var applicationDomain = new ApplicationDomain(this);
                    var feeChangedDate = applicationDomain.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingWetHoseFeeChangedDate);
                    var feeChangedDateTime = DateTimeOffset.Parse(feeChangedDate);
                    if (invoice.CreatedDate >= feeChangedDateTime)
                    {

                        difference = invoice.DropEndDate.AddMilliseconds(-invoice.DropEndDate.Millisecond)
                                    .Subtract(invoice.DropStartDate.AddMilliseconds(-invoice.DropStartDate.Millisecond)).TotalSeconds;
                    }
                    else
                    {
                        if (invoice.AssetDrops.Any())
                        {
                            difference = invoice.AssetDrops.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop).Sum(t => t.DropEndDate.AddMilliseconds(-t.DropEndDate.Millisecond).Subtract(t.DropStartDate.AddMilliseconds(-t.DropStartDate.Millisecond)).TotalSeconds);
                        }
                        else
                        {
                            difference = invoice.DropEndDate.AddMilliseconds(-invoice.DropEndDate.Millisecond).Subtract(invoice.DropStartDate.AddMilliseconds(-invoice.DropStartDate.Millisecond)).TotalSeconds;
                        }
                    }

                    hours = (int)(difference / 3600);
                    mins = (difference - (hours * 3600)) / 60;
                    response.Fee = feeDetails.Fee * Convert.ToDecimal(difference) / 3600;
                    feeDetails.FeeSubQuantity = (decimal)difference;
                    response.TotalHours = hours > 0 ? string.Format(Resource.constTimeFormat, hours, string.Format(Resource.constFormatDecimal2, mins))
                                                                    : string.Format(Resource.constMinuteFormat, string.Format(Resource.constFormatDecimal2, mins));
                }
            }

            return response;
        }

        public OrderDetailVersion GetOrderDetailVersion(Order order, FuelRequest fuelRequest, int userId)
        {
            OrderDetailVersion OrderDetailVersion = new OrderDetailVersion
            {
                OrderId = order.Id,
                PoNumber = order.PoNumber,
                Version = 0,
                PaymentTermId = fuelRequest.PaymentTermId,
                NetDays = fuelRequest.NetDays,
                IsActive = true,
                CreatedBy = userId,
                CreatedDate = DateTimeOffset.Now,
                PaymentMethod = fuelRequest.FuelRequestDetail.PaymentMethod
            };
            return OrderDetailVersion;
        }

        public void CheckForProcessingFee(PaymentMethods paymentMethod, Order order, OrderDetailVersion currentActivePaymentTermsVersion)
        {
            if (currentActivePaymentTermsVersion.PaymentMethod != paymentMethod)
            {
                if (paymentMethod == PaymentMethods.CreditCard)
                {
                    // add processing fee
                    AddCreditCardProcessingFee(order);
                }
                else
                {
                    var processingFee = order.FuelRequest.FuelRequestFees.FirstOrDefault(t => t.FeeTypeId == (int)FeeType.ProcessingFee);
                    if (processingFee != null)
                    {
                        order.FuelRequest.FuelRequestFees.Remove(processingFee);
                    }
                }
            }
        }

        public void AddCreditCardProcessingFee(Order order)
        {
            // check if any fee is define at account level
            var companySetting = Context.DataContext.CompanySettings.FirstOrDefault(t => t.CompanyId == order.AcceptedCompanyId);
            if (companySetting != null && companySetting.ProcessingType.HasValue && companySetting.ProcessingFee > 0 && !order.FuelRequest.FuelRequestFees.Any(t => t.FeeTypeId == (int)FeeType.ProcessingFee))
            {
                FuelFee fuelFee = GetFuelFee((int)FeeType.ProcessingFee, companySetting.ProcessingType.Value, companySetting.ProcessingFee, order.FuelRequest.Currency, order.FuelRequest.UoM);
                order.FuelRequest.FuelRequestFees.Add(fuelFee);
            }
        }

        private FuelFee GetFuelFee(int feeType, int feeSubType, decimal fee, Currency currency, UoM uom)
        {
            return new FuelFee()
            {
                Currency = currency,
                Fee = fee,
                FeeSubTypeId = feeSubType,
                FeeTypeId = feeType,
                UoM = uom
            };
        }

        public decimal GetTotalTaxValue(TransactionResultSummary_5_27_0 avataxDetails)
        {
            decimal totalTax = 0;
            if (avataxDetails.NumberSuccess > 0)
            {
                foreach (var item in avataxDetails.TransactionResults)
                {
                    if (item.Status.Equals(Constants.Success))
                    {
                        totalTax = item.TotalTaxAmount;
                    }
                }
            }
            return totalTax;
        }

        public decimal GetCalculatedPricePerGallon(decimal rackPrice, decimal variablePrice, int rackAvgTypeId)
        {
            var response = rackPrice;
            try
            {
                if (rackAvgTypeId == (int)RackPricingType.PlusDollar)
                {
                    response = rackPrice + variablePrice;
                }
                else if (rackAvgTypeId == (int)RackPricingType.MinusDollar)
                {
                    response = rackPrice - variablePrice;
                }
                else if (rackAvgTypeId == (int)RackPricingType.PlusPercent)
                {
                    response = rackPrice + (rackPrice / 100 * variablePrice);
                }
                else if (rackAvgTypeId == (int)RackPricingType.MinusPercent)
                {
                    response = rackPrice - (rackPrice / 100 * variablePrice);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "CalculateRackAvgPrice", ex.Message, ex);
            }
            return response;
        }

        public string GetServerUrl()
        {
            return ApplicationDomain
                        .ApplicationSettings
                        .GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingSiteFuelExchangeUrl);
        }

        public string GetPdfServerUrl()
        {
            string pdfServerURL = ApplicationDomain
                                    .ApplicationSettings
                                    .GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingTFExchangePdfUrl);
            if (!string.IsNullOrEmpty(pdfServerURL))
                return pdfServerURL;
            else
                return GetServerUrl();
        }
        public string GetAbsoluteServerUrl(string relativePath)
        {
            var response = string.Empty;
            try
            {
                response = GetServerUrl();
                return Path.Combine(response,
                                    (relativePath.StartsWith("~/")
                                    ? relativePath.Substring(2)
                                    : (relativePath.StartsWith("/")
                                        ? relativePath.Substring(1)
                                        : relativePath)));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetAbsoluteServerUrl", ex.Message, ex);
            }
            return response;
        }

        public string GetAbsoluteServerUrl(string serverUrl, string relativePath)
        {
            string response = string.Empty;
            try
            {
                if (serverUrl != null && relativePath != null)
                {
                    response = Path.Combine(serverUrl,
                                       (relativePath.StartsWith("~/")
                                        ? relativePath.Substring(2)
                                        : (relativePath.StartsWith("/")
                                            ? relativePath.Substring(1)
                                            : relativePath)));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetAbsoluteServerUrl", ex.Message, ex);
            }
            return response;
        }

        public string GetApplicationEventNotificationTemplate()
        {
            return ApplicationDomain
                        .ApplicationSettings
                        .GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingApplicationEventNotificationTemplate);
        }

        public static decimal GetPriceWithMargin(decimal margin, decimal originalPrice, int marginType = 1)
        {
            try
            {
                if (margin > 0)
                {
                    if (marginType == (int)MarginType.SpecificAmount)
                    {
                        return originalPrice - margin;
                    }
                    return (originalPrice - (originalPrice * margin / 100));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetPriceWithMargin", ex.Message, ex);
            }
            return originalPrice;
        }

        public async Task<ImageViewModel> GetImage(int imageId)
        {
            var response = new ImageViewModel(Status.Success);
            try
            {
                var image = await Context.DataContext.Images.FirstOrDefaultAsync(t => t.Id == imageId);
                if (image != null)
                {
                    response = image.ToViewModel(response);
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageFailed;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetImage", ex.Message, ex);
            }
            return response;
        }

        public async Task<int> GetFuelRequestIdFromCounterOffer(int counterOfferFuelRequestId)
        {
            var fuelRequest = await Context.DataContext.FuelRequests.FirstOrDefaultAsync(m => m.Id == counterOfferFuelRequestId && m.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest);
            if (fuelRequest != null && fuelRequest.ParentId != null)
            {
                await GetFuelRequestIdFromCounterOffer(fuelRequest.ParentId.Value);
            }
            return fuelRequest != null ? fuelRequest.Id : 0;
        }

        public bool IsCounterOfferAvailable(int fuelRequestId, int supplierId = 0, int buyerId = 0)
        {
            using (var tracer = new Tracer("HelperDomain", "IsCounterOfferAvailable"))
            {
                bool response = false;
                try
                {
                    var counterOffers = Task.Run(() => GetCounterOffers(fuelRequestId)).Result;
                    if (counterOffers != null && counterOffers.Any())
                    {
                        if (supplierId > 0)
                        {
                            // if supplier
                            response = Context.DataContext.CounterOffers.Any
                                        (
                                            x => counterOffers.Contains(x.FuelRequestId) &&
                                            x.FuelRequest.IsActive &&
                                            x.SupplierId == supplierId
                                        );
                        }
                        else if (buyerId > 0)
                        {
                            response = Context.DataContext.CounterOffers.Any
                                        (
                                            x => counterOffers.Contains(x.FuelRequestId) &&
                                            x.FuelRequest.IsActive &&
                                            x.BuyerId == buyerId
                                        );
                        }
                        else
                        {
                            response = Context.DataContext.CounterOffers.Any();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("FuelRequestDomain", "IsCounterOfferAvailable", ex.Message, ex);
                }

                return response;
            }
        }

        public bool IsScheduleDateValid(DateTimeOffset scheduleDate, int scheduleType, DateTime deliveryDate, List<DateTimeOffset> allScheduleDate)
        {
            var IsScheduleDateValid = false;
            try
            {
                if ((scheduleType == (int)DeliveryScheduleType.SpecificDates && scheduleDate.Date == deliveryDate.Date) ||
                                    (scheduleType == (int)DeliveryScheduleType.Monthly && deliveryDate.Day == scheduleDate.Date.Day))
                {
                    IsScheduleDateValid = true;
                }
                else if (scheduleType == (int)DeliveryScheduleType.Weekly)
                {
                    foreach (var item in allScheduleDate)
                    {
                        if ((deliveryDate.Date - item.Date).TotalDays % 7 == 0)
                        {
                            IsScheduleDateValid = true;
                        }
                    }
                }
                else if (scheduleType == (int)DeliveryScheduleType.BiWeekly)
                {
                    foreach (var item in allScheduleDate)
                    {
                        if ((deliveryDate.Date - item.Date).TotalDays % 14 == 0)
                        {
                            IsScheduleDateValid = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsScheduleDateValid", ex.Message, ex);
            }
            return IsScheduleDateValid;
        }

        public bool IsGasolineProduct(int productTypeId)
        {
            var response = false;
            if (productTypeId == (int)ProductTypes.Unleaded || productTypeId == (int)ProductTypes.ConventionalGas || productTypeId == (int)ProductTypes.MidgradeGas || productTypeId == (int)ProductTypes.PremiumGas || productTypeId == (int)ProductTypes.RegularGas || productTypeId == (int)ProductTypes.OtherGas)
            {
                response = true;
            }
            return response;
        }

        public int GetFuelRequestFromCounterOffer(int counterOfferFuelRequestId)
        {
            var counterOffer = Context.DataContext.CounterOffers.FirstOrDefault(m => m.FuelRequestId == counterOfferFuelRequestId);
            return counterOffer != null ? counterOffer.OriginalFuelRequestId : counterOfferFuelRequestId;
        }

        private int GetOriginialFuelRequestId(FuelRequest entity)
        {
            try
            {
                var counterOffer = Context.DataContext.CounterOffers.FirstOrDefault(m => m.FuelRequestId == entity.Id);
                return counterOffer != null ? counterOffer.OriginalFuelRequestId : entity.Id;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetOriginialFuelRequestId", ex.Message, ex);
            }
            return entity.Id;
        }

        public string GetFuelRequestNumberFromCounterOffer(int counterOfferFuelRequestId)
        {
            var fuelRequest = Context.DataContext.FuelRequests.FirstOrDefault(m => m.Id == counterOfferFuelRequestId);
            return fuelRequest.RequestNumber;
        }

        private string GetOriginialFuelRequestNumber(FuelRequest entity)
        {
            try
            {
                if (entity.FuelRequest1 != null)
                {
                    return GetOriginialFuelRequestNumber(entity.FuelRequest1);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetOriginialFuelRequestNumber", ex.Message, ex);
            }
            return entity.RequestNumber;
        }

        public async Task<List<int>> GetCounterOffers(int fuelRequestId, List<int> counterOffers = null)
        {
            using (var tracer = new Tracer("HelperDomain", "GetCounterOffers"))
            {
                try
                {
                    if (counterOffers == null)
                    {
                        counterOffers = new List<int>();
                    }

                    var offers = await Context.DataContext.CounterOffers.Where(t => t.OriginalFuelRequestId == fuelRequestId).ToListAsync();
                    if (offers != null)
                    {
                        foreach (var item in offers)
                        {
                            counterOffers.Add(item.FuelRequestId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("HelperDomain", "GetCounterOffers", ex.Message, ex);
                }

                return counterOffers;
            }
        }

        public async Task<List<Tuple<int, int, int>>> GetCounterOffersWithStatus(int fuelRequestId)
        {
            var counterOffers = new List<Tuple<int, int, int>>();
            try
            {
                var offers = await Context.DataContext.CounterOffers.Where(t => t.OriginalFuelRequestId == fuelRequestId).ToListAsync();
                if (offers != null)
                {
                    foreach (var item in offers)
                    {
                        counterOffers.Add(new Tuple<int, int, int>(item.FuelRequestId, item.BuyerStatus.HasValue ? item.BuyerStatus.Value : 0, item.SupplierStatus.HasValue ? item.SupplierStatus.Value : 0));
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetCounterOffers", ex.Message, ex);
            }

            return counterOffers;

        }

        public async Task<bool> IsFuelRequestExistForJobStartDate(JobViewModel model)
        {
            try
            {
                var job = await Context.DataContext.Jobs.SingleOrDefaultAsync(t => t.Id == model.Id);
                if (job != null)
                {
                    List<int> activeFuelRequestStatus = new List<int>() { (int)FuelRequestStatus.Accepted, (int)FuelRequestStatus.Draft, (int)FuelRequestStatus.Open };
                    if (job.FuelRequests.Any(t => activeFuelRequestStatus.Contains(t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId) && t.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest && t.FuelRequestDetail.StartDate.Date < model.StartDate.Date))
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsFuelRequestExistForJobStartDate", ex.Message, ex);
                return false;
            }
            return true;
        }

        public async Task<bool> IsFuelRequestExistForJobEndDate(JobViewModel model)
        {
            try
            {
                var job = await Context.DataContext.Jobs.SingleOrDefaultAsync(t => t.Id == model.Id);
                if (job != null && model.EndDate.HasValue)
                {
                    List<int> activeFuelRequestStatus = new List<int>() { (int)FuelRequestStatus.Accepted, (int)FuelRequestStatus.Draft, (int)FuelRequestStatus.Open };
                    if (job.FuelRequests.Any(t => activeFuelRequestStatus.Contains(t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId) && t.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest &&
                                            ((t.FuelRequestDetail.EndDate != null && t.FuelRequestDetail.EndDate.Value.Date > model.EndDate.Value.Date) ||
                                            (t.ExpirationDate != null && t.ExpirationDate.Value.Date > model.EndDate.Value.Date))))
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsFuelRequestExistForJobEndDate", ex.Message, ex);
                return false;
            }
            return true;
        }

        public bool ApplyDateRangeFilterToFuelRequest(int fuelRequestId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            bool response = false;
            try
            {
                var fuelRequest = Context.DataContext.FuelRequests.SingleOrDefault(x => x.Id == fuelRequestId);
                if (fuelRequest != null)
                {
                    response = (fuelRequest.CreatedDate >= startDate || fuelRequest.FuelRequestDetail.StartDate >= startDate) &&
                               (fuelRequest.CreatedDate < endDate || fuelRequest.FuelRequestDetail.StartDate < endDate);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "ApplyDateRangeFilterToFuelRequest", ex.Message, ex);
            }
            return response;
        }

        public int GetWeekDayId(DateTimeOffset date)
        {
            int weekDayId = 0;
            try
            {
                weekDayId = date.DayOfWeek == 0 ? (int)WeekDay.Sunday : (int)date.DayOfWeek;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetWeekDayId", ex.Message, ex);
            }
            return weekDayId;
        }

        public decimal GetDryRunFee(ICollection<FuelFee> fuelRequestFees, DateTimeOffset dropDate)
        {
            var dryRunFee = 0.0M;
            try
            {
                // dry run fee can be normal dry run fee or special day dry run fee
                // priority for dry run fee 
                // matching special date > matching weekend > normal
                var dryRun = fuelRequestFees.Where(t => t.FeeTypeId == (int)FeeType.DryRunFee && t.FeeSubTypeId == (int)FeeSubType.FlatFee)
                            .OrderByDescending(t => t.FeeConstraintTypeId)
                            .FirstOrDefault(t => (t.SpecialDate.HasValue && t.SpecialDate.Value.Date == dropDate.Date)
                                                    ||
                                                    (t.FeeConstraintTypeId.HasValue && t.FeeConstraintTypeId.Value == (int)FeeConstraintType.Weekend &&
                                                    (dropDate.DayOfWeek == DayOfWeek.Saturday || dropDate.DayOfWeek == DayOfWeek.Sunday))
                                                    ||
                                                    (!t.FeeConstraintTypeId.HasValue));
                if (dryRun != null)
                {
                    dryRunFee = dryRun.Fee;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetDryRunFee", ex.Message, ex);
            }
            return dryRunFee;
        }

        public async Task<List<int>> GetJobIdsAsync(int userId, string groupIds = "", int timeInSecondsForCache = 10)
        {
            var cacheKey = $"GetJobIdsAsync_{userId}_{groupIds}";
            var response = CacheManager.Get<List<int>>(cacheKey);
            if (response == null)
            {
                response = await GetJobIdsOfUser(userId, groupIds);
                CacheManager.Set(cacheKey, response, timeInSecondsForCache);
            }
            return response;
        }

        public async Task<List<int>> GetAdminUsersOfGroupCompaniesAsync(List<int> groupIds)
        {
            var adminUsers = new List<int>();
            try
            {
                if (groupIds.Count > 0)
                {
                    adminUsers = await (from user in Context.DataContext.Users
                                        join company in Context.DataContext.Companies on user.CompanyId equals company.Id
                                        where user.MstRoles.Any(t => t.Id == (int)UserRoles.Admin) && company.SubCompanies.Any(t => t.SubCompanyId == company.Id && groupIds.Contains(t.CompanyGroupId))
                                        select user.Id).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetAdminUsersOfGroupCompaniesAsync", ex.Message, ex);
            }

            return adminUsers;
        }

        private async Task<List<int>> GetJobIdsOfUser(int userId, string groupIds = "")
        {
            List<int> response;
            using (var tracer = new Tracer("HelperDomain", "GetJobIdsOfUser"))
            {

                var storedProcedureDomain = ContextFactory.Current.GetDomain<StoredProcedureDomain>();
                response = await storedProcedureDomain.GetJobIdsOfUser(userId, groupIds);

                return response;
            }
        }

        public DateTimeOffset GetNextDate(DateTimeOffset currentDate, int days)
        {
            DateTimeOffset newDate = currentDate.Date.AddDays(days);
            if (newDate.Date < DateTimeOffset.Now.Date.AddDays(1))
            {
                return GetNextDate(newDate, days);
            }
            return newDate;
        }

        public List<DropdownDisplayItem> GetQualifiedDrivers(int companyId, bool includeAll, bool allOptionAtEnd = false)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();

            var eligibleRoles = new List<int> { (int)UserRoles.Admin, (int)UserRoles.Supplier, (int)UserRoles.Driver };

            var users = Context.DataContext.Users.Where(t => t.Company.IsActive && t.Company.Id == companyId &&
                            t.IsActive && t.MstRoles.Any(t1 => eligibleRoles.Contains(t1.Id)))
                            .Select(t => new { t.Id, t.FirstName, t.LastName }).ToList();
            if (includeAll && !allOptionAtEnd)
            {
                response.Add(new DropdownDisplayItem { Id = -1, Name = Resource.lblAll });
            }

            users.ForEach(t => response.Add(new DropdownDisplayItem { Id = t.Id, Name = $"{t.FirstName} {t.LastName}" }));

            if (includeAll && allOptionAtEnd)
            {
                if (!response.Exists(t => t.Id == -1))
                {
                    response.Add(new DropdownDisplayItem { Id = -1, Name = Resource.lblAll });
                }
            }

            return response;
        }

        public List<DropdownDisplayItem> GetAllDrivers(int companyId, bool includeAll = false)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            var eligibleRoles = new List<int> { (int)UserRoles.Admin, (int)UserRoles.Supplier, (int)UserRoles.Driver };

            var users = Context.DataContext.Users.Where(t => t.Company.Id == companyId && t.Company.IsActive &&
                            t.MstRoles.Any(t1 => eligibleRoles.Contains(t1.Id)) && !t.IsDeleted
                            && ((t.IsActive && t.IsOnboardingComplete) || (!t.IsActive && !t.IsOnboardingComplete)))
                            .Select(t => new { t.Id, t.FirstName, t.LastName, t.IsOnboardingComplete, t.IsEmailConfirmed }).ToList();
            if (includeAll)
            {
                response.Add(new DropdownDisplayItem { Id = -1, Name = Resource.lblAll });
            }
            //users.ForEach(t => response.Add(new DropdownDisplayItem { Id = t.Id, Name = t.IsOnboardingComplete ? $"{t.FirstName} {t.LastName}" : $"{t.FirstName} {t.LastName} - Pending" }));
            foreach (var user in users)
            {
                if (user.IsOnboardingComplete)
                {
                    response.Add(new DropdownDisplayItem { Id = user.Id, Name = $"{user.FirstName} {user.LastName}" });
                }
                else
                {
                    if (user.IsEmailConfirmed)
                    {
                        response.Add(new DropdownDisplayItem { Id = user.Id, Name = $"{user.FirstName} {user.LastName} {Resource.lblDriverEmailVerfied}" });
                    }
                    else
                    {
                        response.Add(new DropdownDisplayItem { Id = user.Id, Name = $"{user.FirstName} {user.LastName} {Resource.lblDriverInvited}" });
                    }
                }
            }

            return response;
        }
        public List<DropdownDisplayItem> GetAllDriversTractor(int companyId, bool includeAll = false)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            var eligibleRoles = new List<int> { (int)UserRoles.Admin, (int)UserRoles.Supplier, (int)UserRoles.Driver };

            var users = Context.DataContext.Users.Where(t => t.Company.Id == companyId && t.Company.IsActive &&
                            t.MstRoles.Any(t1 => eligibleRoles.Contains(t1.Id)) && !t.IsDeleted
                            && ((t.IsActive && t.IsOnboardingComplete) || (!t.IsActive && !t.IsOnboardingComplete)))
                            .Select(t => new { t.Id, t.FirstName, t.LastName, t.IsOnboardingComplete, t.IsEmailConfirmed }).ToList();
            //users.ForEach(t => response.Add(new DropdownDisplayItem { Id = t.Id, Name = t.FirstName + " " + t.LastName }));
            foreach (var user in users)
            {
                if (user.IsOnboardingComplete)
                {
                    response.Add(new DropdownDisplayItem { Id = user.Id, Name = $"{user.FirstName} {user.LastName}" });
                }
                else
                {
                    if (user.IsEmailConfirmed)
                    {
                        response.Add(new DropdownDisplayItem { Id = user.Id, Name = $"{user.FirstName} {user.LastName} {Resource.lblDriverEmailVerfied}" });
                    }
                    else
                    {
                        response.Add(new DropdownDisplayItem { Id = user.Id, Name = $"{user.FirstName} {user.LastName} {Resource.lblDriverInvited}" });
                    }
                }
            }

            return response;
        }
        public List<DropdownDisplayItem> GetSuppliers(int companyId)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            var eligibleRoles = new List<int> { (int)UserRoles.Admin, (int)UserRoles.Supplier };
            var users = Context.DataContext.Users.Where(t => t.Company.Id == companyId && t.IsActive
                                                                             && t.MstRoles.Any(t1 => eligibleRoles.Contains(t1.Id)))
                                                                            .ToList();

            users.ForEach(t => response.Add(new DropdownDisplayItem { Id = t.Id, Name = $"{t.FirstName} {t.LastName}" }));

            return response;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetCustomerJobs(int buyerCompanyId)
        {
            List<DropdownDisplayExtendedItem> response = new List<DropdownDisplayExtendedItem>();
            var jobs = await Context.DataContext.Jobs.Where(t => t.CompanyId == buyerCompanyId && t.IsActive).ToListAsync();
            jobs.ForEach(t => response.Add(new DropdownDisplayExtendedItem { Id = t.Id, Name = t.Name, Code = t.Address }));
            return response;
        }

        public List<DropdownDisplayItem> GetCompanyOnsiteConstacts(int companyId)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            var users = Context.DataContext.Users.Where(t => t.Company.Id == companyId && t.IsActive
                                                                             && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.OnsitePerson || t1.Id == (int)UserRoles.Admin))
                                                                            .ToList();

            users.ForEach(t => response.Add(new DropdownDisplayItem { Id = t.Id, Name = $"{t.FirstName} {t.LastName}" }));

            return response;
        }

        public List<DropdownDisplayItem> GetCompanyTaxExemptionLicenses(int companyId, int roleId)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();

            var licenses = Context.DataContext.TaxExemptLicenses.Where(t => t.CompanyId == companyId && t.BusinessType == roleId && t.IsActive && t.Status == (int)TaxExemptionLicenseStatus.Open).ToList();
            licenses.ForEach(t => response.Add(new DropdownDisplayItem { Id = t.Id, Name = string.IsNullOrWhiteSpace(t.LicenseNumber) ? t.IDCode : t.LicenseNumber }));

            return response;
        }

        public bool CheckForOpenBrokerOrder(Order order)
        {
            var brokeredFuelRequest = order.FuelRequest.FuelRequests1.OrderByDescending(t => t.Id).FirstOrDefault();
            if (brokeredFuelRequest != null && brokeredFuelRequest.GetFuelRequestLastOrder() != null)
            {
                var brokeredOrder = brokeredFuelRequest.GetFuelRequestLastOrder();
                if (brokeredOrder != null && brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                {
                    return true;
                }
                else
                {
                    if (brokeredOrder != null)
                    {
                        return CheckForOpenBrokerOrder(brokeredOrder);
                    }
                    return false;
                }
            }
            return false;
        }

        public FuelRequest GetFuelRequestConnectedWithBuyer(Order order)
        {
            FuelRequest fuelRequest = new FuelRequest();
            if (order.FuelRequest.FuelRequest1 != null)
            {
                if (order.FuelRequest.FuelRequest1.Orders.LastOrDefault().OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)OrderStatus.Open)
                {
                    return GetFuelRequestConnectedWithBuyer(order.FuelRequest.FuelRequest1.Orders.LastOrDefault());
                }
            }
            else
            {
                fuelRequest = order.FuelRequest;
            }
            return fuelRequest;
        }

        public string GetAssignedDriver(int userId)
        {
            var selectedDriver = Context.DataContext.Users.Where(t => t.Id == userId).FirstOrDefault();
            return $"{selectedDriver.FirstName} {selectedDriver.LastName}";
        }

        public void CloseZTRFuelRequestLoop(int fuelRequestId, List<AssetDropViewModel> assetDrops = null)
        {
            try
            {
                var fuelRequest = Context.DataContext.FuelRequests.FirstOrDefault(t => t.Id == fuelRequestId);
                if (fuelRequest != null)
                {
                    int originalId = GetOriginialFuelRequestId(fuelRequest);
                    var assetsInRequest = Context.DataContext.AssetDropRequests.Where(t => t.FuelRequestId == originalId && !t.IsThisRequestClosed);
                    if (assetsInRequest.Count() > 0)
                    {
                        var patchAssets = new List<AssetFuelRequestPatchViewModel>();
                        foreach (var item in assetsInRequest)
                        {
                            var tempObj = new AssetFuelRequestPatchViewModel
                            {
                                EquipmentId = item.AssetExternalId,
                                FuelDelivered = "0gal",
                                DeliveryDate = $"{DateTimeOffset.UtcNow.ToString("s")}Z"
                            };

                            if (assetDrops != null)
                            {
                                foreach (var drop in assetDrops)
                                {
                                    if (Context.DataContext.JobXAssets.Any(t => t.Id == drop.JobXAssetId && t.AssetId == item.AssetId))
                                    {
                                        var datetimeInISO8601 = (drop.DropDate == null || drop.DropDate == DateTimeOffset.MinValue)
                                                                        ? DateTimeOffset.UtcNow
                                                                        : drop.DropDate.Add(Convert.ToDateTime(drop.StartTime).TimeOfDay);
                                        tempObj.FuelDelivered = $"{drop.DropGallons ?? 0}gal";
                                        tempObj.DeliveryDate = $"{datetimeInISO8601.ToString("s")}Z";
                                        if (drop.DropGallons != null && drop.DropGallons > 0)
                                        {
                                            tempObj.FuelType = item.Asset.FuelType.HasValue
                                                                ? Context.DataContext.MstProductTypes.Single(t => t.Id == item.Asset.FuelType).Name
                                                                : fuelRequest.MstProduct.MstProductType.Name;

                                        }
                                        break;
                                    }
                                }
                            }
                            patchAssets.Add(tempObj);
                        }

                        //Send this to ZTR
                        var url = $"https://api-fuel.onei3.com/v1/fuel/vendors/sitefuel/requests/{originalId}/responseData";
                        var authToken = GetZTRAuthToken();
                        if (!string.IsNullOrWhiteSpace(authToken))
                        {
                            string patchResponseMessage = $"Patch failed for fuel request {originalId}";

                            var client = new RestClient(url);
                            var request = new RestRequest(Method.PATCH);
                            request.AddHeader("content-type", "application/json");
                            request.AddHeader("authorization", $"Bearer {authToken}");
                            request.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(patchAssets), ParameterType.RequestBody);
                            IRestResponse<ZtrFRPatchResponseViewModel> response = client.Execute<ZtrFRPatchResponseViewModel>(request);

                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var responseData = response.Data.ResponseData;

                                if (responseData != null && responseData.Count == patchAssets.Count)
                                {
                                    var reqEquipmentpIds = patchAssets.Select(p => p.EquipmentId).OrderBy(p => p).ToList();
                                    var resEqipmentIds = responseData.Select(t => t.EquipmentId).OrderBy(t => t).ToList();
                                    if (reqEquipmentpIds.SequenceEqual(resEqipmentIds))
                                    {
                                        patchResponseMessage = $"Patch successfull for fuel request {originalId}";
                                        Context.DataContext.AssetDropRequests.Where(t => t.FuelRequestId == originalId && !t.IsThisRequestClosed).ToList().ForEach(t =>
                                        {
                                            t.IsThisRequestClosed = true;
                                        });
                                        Context.Commit();
                                    }
                                }
                            }
                            else
                            {
                                LogManager.Logger.WriteError("ZTRController", "CloseZTRFuelRequestLoop", Newtonsoft.Json.JsonConvert.SerializeObject(patchAssets));
                                LogManager.Logger.WriteError("ZTRController", "CloseZTRFuelRequestLoop", Newtonsoft.Json.JsonConvert.SerializeObject(response.Data));
                                LogManager.Logger.WriteError("ZTRController", "CloseZTRFuelRequestLoop", $"Status Code : {response.StatusCode}, Status Description : {response.StatusDescription}, Error Message : {response.ErrorMessage}");
                            }

                            LogManager.Logger.WriteError("ZTRController", "CloseZTRFuelRequestLoop", patchResponseMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "CloseZTRFuelRequestLoop", ex.Message, ex);
            }
        }

        private string GetZTRAuthToken()
        {
            string token = string.Empty;
            var client = new RestClient("https://auth.onei3.com/core/connect/token")
            {
                Authenticator = new HttpBasicAuthenticator("812224378376275729-of1a7wfsdkdp97a4msyfwh1m1onyuck6", "SP-EvQ2aLukN7HtHXRV7v2z1")
            };

            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "scope=any&grant_type=client_credentials", ParameterType.RequestBody);
            IRestResponse<ZtrAuthResponseViewModel> response = client.Execute<ZtrAuthResponseViewModel>(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                token = response.Data.Access_token;
            }
            return token;
        }

        public void SetAvaTaxConfigSettings()
        {
            try
            {
                var appDomain = new ApplicationDomain(this);
                AvalaraConfigSettings.UserId = appDomain.GetApplicationSettingValue<string>(Constants.AvaTaxUserKey, Constants.AvaTaxUserKey);
                AvalaraConfigSettings.Password = appDomain.GetApplicationSettingValue<string>(Constants.AvaTaxPassKey, Constants.AvaTaxPassKey);
                AvalaraConfigSettings.CompanyName = appDomain.GetApplicationSettingValue<string>(Constants.AvaTaxCompanyKey, Constants.AvaTaxCompanyKey);
                AvalaraConfigSettings.LoginUrl = appDomain.GetApplicationSettingValue<string>(Constants.AvaTaxLoginUrlKey, Constants.AvaTaxLoginUrlKey);
                AvalaraConfigSettings.TaxUrl = appDomain.GetApplicationSettingValue<string>(Constants.AvaTaxTaxUrlKey, Constants.AvaTaxTaxUrlKey);
                AvalaraConfigSettings.TaxExemptionUrl = appDomain.GetApplicationSettingValue<string>(Constants.AvaTaxExemptionUrlKey, Constants.AvaTaxExemptionUrlKey);
                AvalaraConfigSettings.TaxExemptionEnabled = appDomain.GetApplicationSettingValue<bool>(Constants.AvaTaxExemptionEnabled);
                AvalaraConfigSettings.CanUseTaxService = appDomain.GetApplicationSettingValue<bool>(Constants.AvaTaxCanUseServiceKey);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "SetAvaTaxConfigSettings", ex.Message, ex);
            }
        }

        public MstExternalTerminal GetExternalTerminal(int terminalId)
        {
            try
            {
                var terminal = Context.DataContext.MstExternalTerminals.Include(t => t.MstState).SingleOrDefault(t => t.Id == terminalId);
                if (terminal != null)
                {
                    return terminal;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetExternalTerminal", ex.Message, ex);
            }

            return null;
        }

        public MstExternalTerminal GetExternalTerminal(string terminalName, string stateCode)
        {
            try
            {
                var terminal = Context.DataContext.MstExternalTerminals.Include(t => t.MstState).SingleOrDefault(t => t.Name.Contains(terminalName.Trim()) && t.StateCode == stateCode);
                if (terminal != null)
                {
                    return terminal;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetExternalTerminal", ex.Message, ex);
            }

            return null;
        }

        public void LogImpersonationActivity(int impersonatedUserId, int impersonatedBy, string controller, string action, string jsonData, string StartEnd)
        {
            using (var tracer = new Tracer("HelperDomain", "LogImpersonationActivity"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        ImpersonationActivityLog impersonation = new ImpersonationActivityLog
                        {
                            ImpersonatedUserId = impersonatedUserId,
                            ImpersonatedByUserId = impersonatedBy,
                            TimeStamp = DateTimeOffset.Now,
                            Description = string.Format("{0} : {1} : {2}", controller, action, StartEnd),
                            Data = jsonData
                        };
                        Context.DataContext.ImpersonationActivityLogs.Add(impersonation);

                        Context.Commit();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("HelperDomain", "LogImpersonationActivity", ex.Message, ex);
                    }
                }
            }
        }

        public List<DeliveryScheduleDropdownExtendedItem> GetCurrentDeliverySchedules(int orderId, int invoiceId = 0, string splitLoadChainId = "")
        {
            List<DeliveryScheduleDropdownExtendedItem> response = new List<DeliveryScheduleDropdownExtendedItem>();
            try
            {
                var timeZoneName = Context.DataContext.Orders.Where(t => t.Id == orderId)
                            .Select(t => t.FuelRequest.Job.TimeZoneName).SingleOrDefault();

                DateTimeOffset todayDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName).Date;

                var trackableSchedules = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.OrderId == orderId && t.IsActive
                                        && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.MissedAndCanceled
                                        && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.Canceled
                                        && ((!t.Invoices.Any(t1 => t1.IsActive && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active) && (t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.Completed
                                            && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.CompletedLate
                                            && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.RescheduledCompleted
                                            && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.RescheduledLate) || (splitLoadChainId != null && splitLoadChainId != "" && t.Invoices.Any(t1 => t1.InvoiceXAdditionalDetail.SplitLoadChainId == splitLoadChainId && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)))
                                            || t.Invoices.Any(t1 => t1.Id == invoiceId)
                                           ) && t.Date <= todayDate)
                                           .Select(t => new { t.Id, t.Date, t.StartTime, t.EndTime, t.Quantity, t.QuantityTypeId, t.BlendGroupId, t.DeliveryLevelPO }).ToList();
                trackableSchedules.OrderByDescending(t => t.Date).ThenBy(t => t.StartTime).ToList().ForEach(t =>
                {
                    var quantity = t.QuantityTypeId > 1 ? GetScheduleQuantityType(t.QuantityTypeId ?? 0) : t.Quantity.ToString(ApplicationConstants.DecimalFormat2);
                    response.Add(new DeliveryScheduleDropdownExtendedItem
                    {
                        Id = t.Id,
                        Code = t.BlendGroupId,
                        Name = $"{t.Date.Date.ToShortDateString()} { Convert.ToDateTime(t.StartTime.ToString()).ToShortTimeString()} - {Convert.ToDateTime(t.EndTime.ToString()).ToShortTimeString()} { quantity }",
                        DeliveryLevelPO = t.DeliveryLevelPO,
                    });
                });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetCurrentDeliverySchedules", ex.Message, ex);
            }
            return response;
        }

        private static string GetScheduleQuantityType(int quantityType)
        {
            var response = string.Empty;
            switch (quantityType)
            {
                case 2: response = "Balance"; break;
                case 3: response = "Full Load"; break;
                case 4: response = "Small Compartment"; break;
                case 5: response = "Not Specified"; break;
            }
            return response;
        }

        public string GetDeliverySchedule(Invoice invoice)
        {
            string deliverySchedule = string.Empty;
            if (invoice.TrackableSchedule != null)
            {
                deliverySchedule = $"{invoice.TrackableSchedule.Date.Date.ToShortDateString()}   { Convert.ToDateTime(invoice.TrackableSchedule.StartTime.ToString()).ToShortTimeString()} - {Convert.ToDateTime(invoice.TrackableSchedule.EndTime.ToString()).ToShortTimeString()}   { invoice.TrackableSchedule.Quantity.GetPreciseValue(2)} {invoice.UoM} ";
            }
            return deliverySchedule;
        }

        public List<DeliveryScheduleViewModel> GetUndeliveredSchedules(IList<DeliveryScheduleViewModel> schedules)
        {
            var response = new List<DeliveryScheduleViewModel>();
            try
            {
                var deliveryScheduleXTrackableSchedules = Context.DataContext.DeliveryScheduleXTrackableSchedules;
                foreach (var schedule in schedules)
                {
                    var IsDelivered = deliveryScheduleXTrackableSchedules.Where(Extensions.IsTrackableScheduleDelivered()).Any(t => t.DeliveryScheduleId == schedule.Id);
                    if (IsDelivered)
                    {
                        if (schedule.ScheduleType != (int)DeliveryScheduleType.SpecificDates)
                        {
                            response.Add(schedule);
                        }
                    }
                    else
                    {
                        if (schedule.ScheduleType != (int)DeliveryScheduleType.SpecificDates || !deliveryScheduleXTrackableSchedules.Any(t => t.DeliveryScheduleId == schedule.Id && (t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Missed || t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.MissedAndCanceled || t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Canceled || t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.RescheduledMissed)))
                        {
                            response.Add(schedule);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "GetUndeliveredSchedules", ex.Message, ex);
            }
            return response;
        }

        public bool AssignOrderLevelDriver(Order order, int userId, int? driverId, bool DetailTab = true)
        {
            bool isDriverModified = false;
            var notificationDomain = new NotificationDomain(this);
            var currentDriver = order.OrderXDrivers.SingleOrDefault(t => t.IsActive);
            if (currentDriver == null && driverId != null)
            {
                AssignDriverToOrder(order, userId, driverId.Value);
                if (DetailTab)
                {
                    notificationDomain.AddNotificationEvent(EventType.DriverAssignedToOrder, order.Id, userId);
                }
            }
            else if (currentDriver != null && driverId == null)
            {
                RemoveDriverFromOrder(currentDriver, userId);
                isDriverModified = true;
                if (DetailTab)
                {
                    notificationDomain.AddNotificationEvent(EventType.DriverRemovedFromOrder, order.Id, userId);
                }
            }
            else if (currentDriver != null && driverId != null && currentDriver.DriverId != driverId)
            {
                UpdateDriverToOrder(order, userId, driverId.Value, currentDriver);
                isDriverModified = true;
                if (DetailTab)
                {
                    notificationDomain.AddNotificationEvent(EventType.DriverAssignedToOrder, order.Id, userId);
                }
            }
            return isDriverModified;
        }

        public bool AssignDeliveryLevelDriver(DeliverySchedule delivery, int userId, int? driverId, int orderId, bool DeliveryTab = false)
        {
            var notificationDomain = new NotificationDomain(this);
            var deliveryDriver = delivery.DeliveryScheduleXDrivers.SingleOrDefault(t => t.IsActive);
            if (deliveryDriver == null && delivery.Id > 0)
            {
                deliveryDriver = Context.DataContext.DeliveryScheduleXDrivers.SingleOrDefault(t => t.IsActive
                                 && t.DeliveryScheduleId == delivery.Id);
            }
            bool isDriverModified = false;
            if (deliveryDriver == null && driverId != null) // New driver assignment
            {
                AssignDriverToSchedule(delivery, userId, driverId.Value, delivery.Id);
                isDriverModified = true;
                if (DeliveryTab)
                {
                    notificationDomain.AddNotificationEvent(EventType.DriverAssignedToDelivery, delivery.Id, userId);
                }
                UpdateDeliveryStatus(delivery.Id, DeliveryScheduleStatus.Assigned);
            }
            else if (deliveryDriver != null && driverId == null) // Driver un-assignment
            {
                RemoveDriverFromSchedule(deliveryDriver, userId);
                isDriverModified = true;
                if (DeliveryTab)
                {
                    notificationDomain.AddNotificationEvent(EventType.DriverRemovedFromDelivery, delivery.Id, userId);
                }
                UpdateDeliveryStatus(delivery.Id, DeliveryScheduleStatus.Unassigned);
            }
            else if (deliveryDriver != null && driverId != null && deliveryDriver.DriverId != driverId) // Driver update
            {
                UpdateDriverToSchedule(delivery, userId, driverId.Value, delivery.Id, deliveryDriver);
                delivery.StatusId = (int)DeliveryScheduleStatus.Reassigned;
                isDriverModified = true;
                if (DeliveryTab)
                {
                    notificationDomain.AddNotificationEvent(EventType.DriverRemovedFromDelivery, delivery.Id, userId);
                    notificationDomain.AddNotificationEvent(EventType.DriverAssignedToDelivery, delivery.Id, userId);
                }
                UpdateDeliveryStatus(delivery.Id, DeliveryScheduleStatus.Reassigned);
            }

            if (isDriverModified)
            {
                var trackableSchedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(Extensions.IsTrackableScheduleUnDelivered()).Where(t => t.DeliverySchedule.Id == delivery.Id
                                                                    && t.OrderId == orderId
                                                                    && !t.Invoices.Any(t1 => t1.IsActive && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                    && (t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Accepted
                                                                    || t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Rescheduled)
                                                                    && t.IsActive)
                                                                .ToList();
                if (trackableSchedule.Any())
                {
                    trackableSchedule.ForEach(t => t.DriverId = driverId);
                }
                Context.DataContext.SaveChanges();
            }

            return isDriverModified;
        }

        public string GetNewMessageReceivedTime(DateTimeOffset dtMessageReceived)
        {
            string newMessageReceived = string.Empty;
            int daysDifference = (DateTimeOffset.Now.Date - dtMessageReceived.Date).Days;

            if (daysDifference < 2)
            {
                switch (daysDifference)
                {
                    case 0:
                        newMessageReceived = "Today at " + dtMessageReceived.ToString(Resource.constFormat12HourTime) + " - " + dtMessageReceived.ToString(Resource.constFormatDate);
                        break;
                    case 1:
                        newMessageReceived = "Yesterday at " + dtMessageReceived.ToString(Resource.constFormat12HourTime) + " - " + dtMessageReceived.ToString(Resource.constFormatDate);
                        break;
                }
            }
            else
            {
                newMessageReceived = daysDifference + " days ago at " + dtMessageReceived.ToString(Resource.constFormat12HourTime) + " - " + dtMessageReceived.ToString(Resource.constFormatDate);
            }

            return newMessageReceived;
        }

        public string GetAddedScheduleDetails(IEnumerable<DeliveryScheduleDetail> CurrentSchedules)
        {
            StringBuilder deliveryWindow = new StringBuilder();
            try
            {
                var addedSchedules = CurrentSchedules.Where(t => t.StatusId == (int)DeliveryScheduleStatus.New);
                deliveryWindow.Append("<ul>");
                foreach (var item in addedSchedules)
                {
                    switch (item.Type)
                    {
                        case 1:
                            deliveryWindow.Append($"<li><b>{Resource.lblWeekly}: {string.Join(",", item.DayNames)} - Delivery Window: {item.Start} to {item.End} - {item.Gallons} Gallons</b></li>");
                            break;
                        case 2:
                            deliveryWindow.Append($"<li><b>{Resource.lblBiWeekly}: {string.Join(",", item.DayNames)} - Delivery Window: {item.Start} to {item.End} - {item.Gallons} Gallons</b></li>");
                            break;
                        case 3:
                            deliveryWindow.Append($"<li><b>{Resource.lblMonthly}: {item.Date} - Delivery Window: {item.Start} to {item.End} - {item.Gallons} Gallons</b></li>");
                            break;
                        case 4:
                            deliveryWindow.Append($"<li><b>{Resource.lblSpecificDates}: {item.Date} - Delivery Window: {item.Start} to {item.End} - {item.Gallons} Gallons</b></li>");
                            break;
                    }
                }
                deliveryWindow.Append("</ul>");
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetAddedScheduleDetails", ex.Message, ex);
            }

            return deliveryWindow.ToString();
        }

        public string GetAddedScheduleDetailsForSms(IEnumerable<DeliveryScheduleDetail> CurrentSchedules)
        {
            string deliveryWindow = string.Empty;
            try
            {
                var addedSchedules = CurrentSchedules.Where(t => t.StatusId == (int)DeliveryScheduleStatus.New);
                foreach (var item in addedSchedules)
                {
                    if (addedSchedules.Count() == 1)
                    {
                        switch (item.Type)
                        {
                            case 1:
                                deliveryWindow = $"{"a"} {Resource.lblWeekly} {Resource.lblDeliveryScheduleForSms}";
                                break;
                            case 2:
                                deliveryWindow = $"{"a"} {Resource.lblBiWeekly} {Resource.lblDeliveryScheduleForSms}";
                                break;
                            case 3:
                                deliveryWindow = $"{"a"} {Resource.lblMonthly} {Resource.lblDeliveryScheduleForSms}";
                                break;
                            case 4:
                                deliveryWindow = $"{"a"} {Resource.lblSpecificDates} {Resource.lblDeliveryScheduleForSms}";
                                break;
                        }
                    }
                    else if (addedSchedules.Count() > 1)
                    {
                        deliveryWindow = $" {Resource.lblMultipleDeliverySchedules}";
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetAddedScheduleDetailsForSms", ex.Message, ex);
            }

            return deliveryWindow;
        }

        public string GetModifiedScheduleDetails(IEnumerable<DeliveryScheduleDetail> CurrentSchedules, IEnumerable<DeliveryScheduleDetail> PreviousSchedules)
        {
            StringBuilder deliveryWindow = new StringBuilder();
            try
            {
                var modifiedSchedules = CurrentSchedules.Where(t => t.StatusId == (int)DeliveryScheduleStatus.Modified);
                var modifiedScheduleGroupIds = modifiedSchedules.Select(t => t.GroupId);
                var existingSchedules = PreviousSchedules.Where(t => modifiedScheduleGroupIds.Contains(t.GroupId));
                deliveryWindow.Append("<ul>");
                foreach (var modified in modifiedSchedules)
                {
                    var oldSchedule = existingSchedules.FirstOrDefault(t => t.GroupId == modified.GroupId);
                    if (oldSchedule != null)
                    {
                        switch (modified.Type)
                        {
                            case 1:
                                deliveryWindow.Append($"<li>From <b>{Resource.lblWeekly}: {string.Join(",", oldSchedule.DayNames)} - {oldSchedule.Start} to {oldSchedule.End} - {oldSchedule.Gallons} Gallons</b>");
                                deliveryWindow.Append($" To <b>{Resource.lblWeekly} {string.Join(",", modified.DayNames)} - {modified.Start} to {modified.End} - {modified.Gallons} Gallons</b></li>");
                                break;
                            case 2:
                                deliveryWindow.Append($"<li>From <b>{Resource.lblBiWeekly}: {string.Join(",", oldSchedule.DayNames)} - {oldSchedule.Start} to {oldSchedule.End} - {oldSchedule.Gallons} Gallons</b>");
                                deliveryWindow.Append($" To <b>{Resource.lblBiWeekly} {string.Join(",", modified.DayNames)} - {modified.Start} to {modified.End} - {modified.Gallons} Gallons</b></li>");
                                break;
                            case 3:
                                deliveryWindow.Append($"<li>From <b>{Resource.lblMonthly}: {oldSchedule.Date} - {oldSchedule.Start} to {oldSchedule.End} - {oldSchedule.Gallons} Gallons</b>");
                                deliveryWindow.Append($" To {Resource.lblMonthly} {modified.Date} - {modified.Start} to {modified.End} - {modified.Gallons} Gallons</b></li>");
                                break;
                            case 4:
                                deliveryWindow.Append($"<li>From <b>{Resource.lblSpecificDates}: {oldSchedule.Date} - {oldSchedule.Start} to {oldSchedule.End} - {oldSchedule.Gallons} Gallons</b>");
                                deliveryWindow.Append($" To <b>{Resource.lblSpecificDates} {modified.Date} - {modified.Start} to {modified.End} - {modified.Gallons} Gallons</b></li>");
                                break;
                        }
                    }
                }
                deliveryWindow.Append("</ul>");
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetModifiedScheduleDetails", ex.Message, ex);
            }

            return deliveryWindow.ToString();
        }

        public string GetRescheduledScheduleDetails(IEnumerable<DeliveryScheduleDetail> CurrentSchedules)
        {
            StringBuilder deliveryWindow = new StringBuilder();
            try
            {
                var rescheduledSchedules = CurrentSchedules.Where(t => t.StatusId == (int)DeliveryScheduleStatus.Rescheduled);
                var rescheduledTrackableSchedules = rescheduledSchedules.Select(t => t.RescheduledTrackableId);
                var existingSchedules = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => rescheduledTrackableSchedules.Contains(t.Id)).ToList();
                deliveryWindow.Append("<ul>");
                foreach (var rescheduled in rescheduledSchedules)
                {
                    var oldSchedule = existingSchedules.FirstOrDefault(t => t.Id == rescheduled.RescheduledTrackableId);
                    if (oldSchedule != null)
                    {
                        deliveryWindow.Append($"<li>From <b>{oldSchedule.Date.ToString(Resource.constFormatDate)} - {oldSchedule.Date.Add(oldSchedule.StartTime).ToString(Resource.constFormat12HourTime)} to {oldSchedule.Date.Add(oldSchedule.EndTime).ToString(Resource.constFormat12HourTime)}</b>");
                        deliveryWindow.Append($" To <b>{rescheduled.Date} - {rescheduled.Start} to {rescheduled.End}</b></li>");
                    }
                }
                deliveryWindow.Append("</ul>");
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetModifiedScheduleDetails", ex.Message, ex);
            }

            return deliveryWindow.ToString();
        }

        private void AssignDriverToSchedule(DeliverySchedule delivery, int userId, int driverId, int scheduleId)
        {
            var deliveryDriver = new DeliveryScheduleXDriver
            {
                DriverId = driverId,
                DeliveryScheduleId = scheduleId,
                AssignedBy = userId,
                AssignedDate = DateTimeOffset.Now,
                IsActive = true
            };

            if (delivery.Id > 0)
            {
                Context.DataContext.Entry(deliveryDriver).State = EntityState.Added;
            }
            else
            {
                delivery.DeliveryScheduleXDrivers.Add(deliveryDriver);
            }
        }

        public void RemoveDriverFromSchedule(DeliveryScheduleXDriver deliveryDriver, int userId)
        {
            if (deliveryDriver != null)
            {
                deliveryDriver.RemovedBy = userId;
                deliveryDriver.RemovedDate = DateTimeOffset.Now;
                deliveryDriver.IsActive = false;
            }
        }

        private void UpdateDriverToSchedule(DeliverySchedule delivery, int userId, int driverId, int scheduleId, DeliveryScheduleXDriver deliveryDriver)
        {
            RemoveDriverFromSchedule(deliveryDriver, userId);
            AssignDriverToSchedule(delivery, userId, driverId, scheduleId);
        }

        private void UpdateDeliveryStatus(int scheduleId, DeliveryScheduleStatus deliveryScheduleStatus)
        {
            if (scheduleId > 0)
            {
                var schedule = Context.DataContext.DeliverySchedules.SingleOrDefault(t => t.Id == scheduleId);
                schedule.StatusId = (int)deliveryScheduleStatus;
                Context.DataContext.Entry(schedule).State = EntityState.Modified;
            }
        }

        private void AssignDriverToOrder(Order order, int userId, int driverId)
        {
            if (order != null)
            {
                var orderDriver = new OrderXDriver
                {
                    DriverId = driverId,
                    AssignedBy = userId,
                    AssignedDate = DateTimeOffset.Now,
                    IsActive = true
                };
                order.OrderXDrivers.Add(orderDriver);
            }
        }

        public void RemoveDriverFromOrder(OrderXDriver orderDriver, int userId)
        {
            if (orderDriver != null)
            {
                orderDriver.RemovedBy = userId;
                orderDriver.RemovedDate = DateTimeOffset.Now;
                orderDriver.IsActive = false;
            }
        }

        private void UpdateDriverToOrder(Order order, int userId, int driverId, OrderXDriver orderDriver)
        {
            RemoveDriverFromOrder(orderDriver, userId);
            AssignDriverToOrder(order, userId, driverId);
        }

        public async Task<List<DropdownDisplayItem>> GetProductsByZip(string zipCode, decimal radius, PricingSource source, int companyId = 0)
        {
            List<DropdownDisplayItem> products = new List<DropdownDisplayItem>();
            try
            {
                decimal latitude = 0, longitude = 0;
                string countryCode = Constants.CountryUSA;
                if (zipCode != "")
                {
                    var geoCodes = GoogleApiDomain.GetGeocode(zipCode);
                    if (geoCodes != null)
                    {
                        latitude = Convert.ToDecimal(geoCodes.Latitude);
                        longitude = Convert.ToDecimal(geoCodes.Longitude);
                        countryCode = Convert.ToString(geoCodes.CountryCode);
                    }
                }
                var pricingServiceDomain = ContextFactory.Current.GetDomain<PricingServiceDomain>();
                products = await pricingServiceDomain.GetProductsInYourArea(radius, latitude, longitude, countryCode, companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetProductsByZip", ex.Message, ex);
            }
            return products;
        }

        public async Task<bool> SendAxxisPriceMonitorEmail()
        {
            bool response = false;
            try
            {
                var emailTemplate = GetApplicationEventNotificationTemplate();

                var emailList = ContextFactory.Current.GetDomain<ApplicationDomain>().GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingAxxisMonitoringEmailList);

                var emailModel = new ApplicationEventNotificationViewModel
                {
                    To = emailList.Split(';').ToList(),
                    Subject = "Axxis price updates failed",
                    BodyText = "Axxis price sync not done on " + DateTime.Now.Date.ToShortDateString(),
                    ShowHelpLineInfo = false,
                };

                response = await ContextFactory.Current.GetDomain<EmailDomain>().SendEmail(emailTemplate, emailModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "SendAxxisPriceMonitorEmail", ex.Message, ex);
            }

            return response;
        }

        public string SetEntityThreadCulture(Currency currency)
        {
            string culture = ApplicationConstants.Culture_USA;
            if (currency != Currency.None)
                culture = currency == Currency.USD ? ApplicationConstants.Culture_USA : ApplicationConstants.Culture_CANADA;

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            return culture;
        }

        public string SetCountryCulture(int countryId)
        {
            string culture = ApplicationConstants.Culture_USA;
            switch (countryId)
            {
                case (int)Country.USA:
                    culture = ApplicationConstants.Culture_USA;
                    break;
                case (int)Country.CAN:
                    culture = ApplicationConstants.Culture_CANADA;
                    break;
                default:
                    break;
            }
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            return culture;
        }

        public bool IsValidBillingStatementId(string billingStatementId, int createdByCompanyId, int id)
        {
            var response = false;
            try
            {
                response = !Context.DataContext.BillingSchedules
                                    .Any(t => t.CreatedByCompanyId == createdByCompanyId && t.IsActive
                                    && t.Id != id && t.BillingStatementId == billingStatementId);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsValidBillingStatementId", ex.Message, ex);
            }

            return response;
        }

        public DateTimeOffset GetPriceDate(DateTimeOffset date, int feedTypeId)
        {
            DateTimeOffset priceDate = date;
            if (feedTypeId == (int)PricingSourceFeedTypes.PreviousDay_5PM_EST
                || feedTypeId == (int)PricingSourceFeedTypes.PreviousDay_10AM_EST)
            {
                priceDate = date.AddDays(-1);
            }
            return priceDate;
        }
        public int GetFeedType(int feedTypeId)
        {
            if (feedTypeId == (int)PricingSourceFeedTypes.PreviousDay_5PM_EST)
            {
                feedTypeId = (int)PricingSourceFeedTypes.EOD_5PM_EST;
            }
            else if (feedTypeId == (int)PricingSourceFeedTypes.PreviousDay_10AM_EST)
            {
                feedTypeId = (int)PricingSourceFeedTypes.Contract_10AM_EST;
            }
            return feedTypeId;
        }
        public async Task<int> GetCompanyIdFromOrderId(int orderId)
        {
            int companyId = 0;
            try
            {
                companyId = await Context.DataContext.Orders.Where(m => m.Id == orderId).Select(m => m.FuelRequest.Job.CompanyId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetCompanyIdFromOrderId", ex.Message, ex);
            }
            return companyId;
        }

        public async Task<DropdownDisplayItem> GetAssignedDriverForSchedule(int scheduleId, int orderId)
        {
            DropdownDisplayItem response = new DropdownDisplayItem();

            try
            {
                var driverUser = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.Id == scheduleId && t.OrderId == orderId && t.DriverId != null).Select(t => t.User).FirstOrDefaultAsync();
                if (driverUser != null)
                {
                    response.Id = driverUser.Id;
                    if (driverUser.IsOnboardingComplete)
                    {
                        response.Name = $"{driverUser.FirstName} {driverUser.LastName}";
                    }
                    else
                    {
                        if (driverUser.IsEmailConfirmed)
                        {
                            response.Name = $"{driverUser.FirstName} {driverUser.LastName} {Resource.lblDriverEmailVerfied}";
                            //response.Add(new DropdownDisplayExtendedItem { Id = item.Id, Name = $"{item.FirstName} {item.LastName} {Resource.lblDriverEmailVerfied}" });
                        }
                        else
                        {
                            response.Name = $"{driverUser.FirstName} {driverUser.LastName} {Resource.lblDriverInvited}";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetAssignedDriverForSchedule", ex.Message, ex);
            }
            return response;
        }

        /// <summary>
        /// Gets the name of the supplier assigned product.
        /// </summary>
        /// <param name="supplierCompanyId">The supplier company identifier.</param>
        /// <param name="tfxFuelTypeId">The TFX fuel type identifier.</param>
        /// <param name="terminalId">The terminal identifier.</param>
        /// <returns></returns>
        public ProductMappingViewModel GetSupplierAssignedProductName(int supplierCompanyId, int tfxFuelTypeId, int terminalId)
        {
            var supplierMapping = (from productDetails in Context.DataContext.SupplierMappedProductDetails
                                   where (productDetails.CompanyId == supplierCompanyId
                                          && productDetails.TerminalId == terminalId
                                          && productDetails.FuelTypeId == tfxFuelTypeId
                                          && productDetails.IsActive)
                                   select new ProductMappingViewModel { Id = productDetails.Id, MyProductId = productDetails.MyProductId, BackOfficeProductId = productDetails.BackOfficeProductId, DriverProductId = productDetails.DriverProductId }).OrderByDescending(t => t.Id).FirstOrDefault();

            return supplierMapping;
        }
        public async Task SetDeletedPreLoadBolLastSyncDateTime(string updatedDateTime)
        {
            var mstAppSetting = await Context.DataContext.MstAppSettings.Where(m => m.Key == ApplicationConstants.KeyAppSettingGoogleFirebaseDeletedPreLoadBolSyncDateTime).FirstOrDefaultAsync();
            mstAppSetting.UpdatedDate = DateTimeOffset.Parse(updatedDateTime).UtcDateTime;
            await Context.CommitAsync();
        }
        public async Task SetEditedPreLoadBolLastSyncDateTime(string updatedDateTime)
        {
            var mstAppSetting = await Context.DataContext.MstAppSettings.Where(m => m.Key == ApplicationConstants.KeyAppSettingGoogleFirebaseEditedPreLoadBolSyncDateTime).FirstOrDefaultAsync();
            mstAppSetting.UpdatedDate = DateTimeOffset.Parse(updatedDateTime).UtcDateTime;
            await Context.CommitAsync();
        }
        public async Task SetFuelRetainLastSyncDateTime(string updatedDateTime)
        {
            var mstAppSetting = await Context.DataContext.MstAppSettings.Where(m => m.Key == ApplicationConstants.KeyAppSettingGoogleFirebaseFuelRetainSyncDateTime).FirstOrDefaultAsync();
            mstAppSetting.UpdatedDate = DateTimeOffset.Parse(updatedDateTime).UtcDateTime;
            await Context.CommitAsync();
        }
        public void setDriverUserOnboardingStatus(List<TractorDetailViewModel> tractorDetails)
        {
            try
            {
                if (tractorDetails != null && tractorDetails.Any())
                {
                    foreach (var tractorDetail in tractorDetails)
                    {
                        List<int> driverUsersIds = new List<int>();
                        if (tractorDetail.Drivers != null && tractorDetail.Drivers.Any())
                        {
                            tractorDetail.Drivers.ForEach(t => driverUsersIds.Add(t.Id));

                            if (driverUsersIds.Any())
                            {
                                var eligibleRoles = new List<int> { (int)UserRoles.Admin, (int)UserRoles.Supplier, (int)UserRoles.Driver };

                                var users = Context.DataContext.Users.Where(t => driverUsersIds.Contains(t.Id) &&
                                               t.MstRoles.Any(t1 => eligibleRoles.Contains(t1.Id))
                                               && ((t.IsActive && t.IsOnboardingComplete) || (!t.IsActive && !t.IsOnboardingComplete)))
                                                .Select(t => new { t.Id, t.FirstName, t.LastName, t.IsOnboardingComplete, t.IsEmailConfirmed }).ToList();
                                foreach (var driverUser in users)
                                {
                                    //var driver = users.Find(t => t.Id == driverUser.Id);
                                    var driverDetails = tractorDetail.Drivers.Find(t => t.Id == driverUser.Id);
                                    if (driverDetails != null)
                                    {
                                        if (driverUser.IsEmailConfirmed)
                                        {
                                            var name = driverDetails.Name;
                                            driverDetails.Name = name + Resource.lblDriverEmailVerfied;
                                        }
                                        else if (!driverUser.IsOnboardingComplete)
                                        {
                                            var name = driverDetails.Name;
                                            driverDetails.Name = name + Resource.lblDriverInvited;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "setDriverUserOnboardingStatus", ex.Message, ex);

            }
        }

        public string GetDisplayCumulationFrequencyLabel(int cumulationTypeId, DateTimeOffset cumulationResetDate, int cumulationResetDay)
        {
            string displayLabel = string.Empty;
            try
            {
                if (cumulationTypeId == (int)CumulationType.Weekly)
                {
                    displayLabel = $"{CumulationType.Weekly.GetDisplayName()}({(WeekDay)cumulationResetDay})";
                }
                else if (cumulationTypeId == (int)CumulationType.BiWeekly)
                {
                    displayLabel = $"{CumulationType.BiWeekly.GetDisplayName()}({(WeekDay)cumulationResetDay})";
                }
                else if (cumulationTypeId == (int)CumulationType.Monthly)
                {
                    var dayOfMonth = cumulationResetDate.Day;
                    var formattedDayStr = AddOrdinalToDigit(dayOfMonth);
                    displayLabel = $"{CumulationType.Monthly.GetDisplayName()}({formattedDayStr} day)";
                }
                else if (cumulationTypeId == (int)CumulationType.SpecificDates)
                {
                    displayLabel = $"{cumulationResetDate.Date}";
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetDisplayCumulationFrequencyLabel", ex.Message, ex);
            }
            return displayLabel;
        }

        public string AddOrdinalToDigit(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }
        }
        public List<DropdownDisplayItem> GetCityRackTerminalNameByIds(PricingRequestDetailResponseViewModel pricingDetails)
        {

            var cityRackTerminals = pricingDetails.TierPricings.Where(w => w.CityRackTerminalId != null && w.CityRackTerminalId != 0).Select(s => s.CityRackTerminalId).ToList();

            if (cityRackTerminals != null && cityRackTerminals.Any())
                return Context.DataContext.MstExternalTerminals.Where(w => cityRackTerminals.Contains(w.Id)).Select(s => new DropdownDisplayItem { Id = s.Id, Name = s.Name }).ToList();
            else
                return null;
            return null;
        }

        public async Task<bool> IsLiftFileValidationEnabled(int companyId)
        {
            var response = false;
            try
            {
                response = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == companyId && t.IsActive).Select(t => t.IsLiftFileValidationEnabled).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "IsLiftFileValidationEnabled", ex.Message, ex);

            }
            return response;
        }

        public async Task<bool> IsMatchesRegex(string regex, string validateString)
        {
            var isRegexMatch = false;
            try
            {
                if (Regex.IsMatch(validateString, regex))
                {
                    isRegexMatch = true;
                }
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("HelperDomain", "IsMatchesRegex", ex.Message, ex);

            }
            return isRegexMatch;
        }


        public List<IdentityProvider> GeActiveIdentityProvider()
        {
            return Context.DataContext.IdentityProviders.Where(t => t.IsActive).ToList();
        }

        public List<DropdownDisplayItem> GetExternalIdentityProviders(int companyId)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            try
            {
                var idps = Context.DataContext.CompanyIdentityServices
                    .Where(t => t.CompanyId == companyId && t.IsAvailable).ToList();
                if (idps != null)
                {
                    foreach (var idp in idps)
                    {
                        response.Add(new DropdownDisplayItem { Id = idp.CompanyIdentityServiceId, Name = idp.IdentityProvider.DisplayName });
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetExternalIdentityProviders", ex.Message, ex);
            }

            return response.Distinct().ToList();
        }

        //private static readonly ReadOnlyCollection<int> _excludedQueueJobProcessType = Array.AsReadOnly((QueueProcessType[])Enum.GetValues(typeof(QueueProcessType)));
        private static List<int> _excludedQueueJobProcessType;

        public static List<int> ExcludedQueueJobProcessType
        {
            get
            {
                InitIfNot_ExcludedQueueProcessType();
                return _excludedQueueJobProcessType;
            }
        }

        private static void InitIfNot_ExcludedQueueProcessType()
        {
            if (_excludedQueueJobProcessType == null || _excludedQueueJobProcessType.Count == 0)
            {
                _excludedQueueJobProcessType = new List<int>();
                _excludedQueueJobProcessType.Add((int)QueueProcessType.ThirdPartyOrderBulkUpload);
                _excludedQueueJobProcessType.Add((int)QueueProcessType.ExternalMeterDataUpload);
                //DtnFileGeneration
                _excludedQueueJobProcessType.Add((int)QueueProcessType.InvoiceBulkUpload);
                //InvoiceImageUpload
                //InvoiceUploadErrors
                _excludedQueueJobProcessType.Add((int)QueueProcessType.PoNumberBulkUpload);
                _excludedQueueJobProcessType.Add((int)QueueProcessType.TankBulkUpload);
                _excludedQueueJobProcessType.Add((int)QueueProcessType.DemandCaptureUpload);
                _excludedQueueJobProcessType.Add((int)QueueProcessType.ProductMappingBulkUpload);
                //BrokerInvoiceImageUpload
                _excludedQueueJobProcessType.Add((int)QueueProcessType.TerminalItemCodeMappingBulkUpload);
                _excludedQueueJobProcessType.Add((int)QueueProcessType.AssetBulkUpload);
                _excludedQueueJobProcessType.Add((int)QueueProcessType.JobsBulkUpload);
            }
        }
    }
}
