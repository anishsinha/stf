using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;

namespace SiteFuel.Exchange.Domain
{
    public class SourcingRequestDomain : BaseDomain
    {
        public SourcingRequestDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public SourcingRequestDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<SourcingRequestViewModel> GetPreferencesSettings(UserContext userContext)
        {
            var response = new SourcingRequestViewModel();
            try
            {
                var onboardingPreferencesSetting = await Context.DataContext.OnboardingPreferences
                                                                            .Where(t => t.IsActive && t.CompanyId == userContext.CompanyId)
                                                                            .OrderByDescending(t => t.Id)
                                                                            .Select(t => new
                                                                            {
                                                                                t.Id,
                                                                                t.DeliveryType,
                                                                                t.IsCustomerInvitationEnabled,
                                                                                t.IsBuySellEnabled,
                                                                                t.IsThirdPartyHardwareUsed,
                                                                                t.PreferencePricingMethod,
                                                                                t.FreightOnBoardType,
                                                                                t.IsSupressOrderPricing,
                                                                                t.IsDropTicketImageRequired,
                                                                                t.IsFreightOnlyOrderEnabled,
                                                                                t.IsDriverProdutDisplayEnable,
                                                                                t.LocationManagedType,
                                                                                t.IsBadgeMandatory
                                                                            })
                                                                            .FirstOrDefaultAsync();
                if (onboardingPreferencesSetting != null)
                {
                    response.FreightOnBoardType = onboardingPreferencesSetting.FreightOnBoardType;
                    response.TruckLoadType = onboardingPreferencesSetting.DeliveryType;
                    response.CustomerDetails.IsInvitationEnabled = onboardingPreferencesSetting.IsCustomerInvitationEnabled;
                    response.IsSupressOrderPricing = onboardingPreferencesSetting.IsSupressOrderPricing;
                }
                else
                {
                    response.FreightOnBoardType = FreightOnBoardTypes.Destination;
                }


                response.FuelDetails.FuelDisplayGroupId = (int)ProductDisplayGroups.CommonFuelType;
                response.FuelDetails.QuantityTypeId = (int)QuantityType.NotSpecified;
                var masterDomain = new MasterDomain(this);
                var defaultaddresscountryId = masterDomain.GetDefaultServingCountry(userContext.CompanyId);
                var defaultCurrency = masterDomain.GetDefaultCurrencyForCompany(userContext.CompanyId);
                var defaultUoM = masterDomain.GetDefaultUoMforCompany(userContext.CompanyId);
                response.AddressDetails.CountryId = defaultaddresscountryId;
                response.AddressDetails.Currency = defaultCurrency;
                response.AddressDetails.UOM = defaultUoM;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "InitializeSourcingViewModel", ex.Message, ex);
            }

            return response;
        }
        public async Task<StatusViewModel> Create(UserContext userContext, SourcingRequestViewModel sourcingRequestViewModel)
        {
            var response = new StatusViewModel();
            try
            {
                if (sourcingRequestViewModel != null)
                {
                    var sourcingRequest = sourcingRequestViewModel.ToEntity();
                    if (sourcingRequestViewModel.FuelDetails.Fees != null)
                    {
                        sourcingRequest.FuelFees = sourcingRequestViewModel.FuelDetails.Fees.ToFeesEntity();
                    }
                    if (sourcingRequestViewModel.GeneralNote != null)
                    {
                        LeadNote leadNote = new LeadNote()
                        {
                            GeneralNote = sourcingRequestViewModel.GeneralNote,
                            CreatedBy = userContext.Id,
                            CreatedDate = DateTimeOffset.Now
                        };
                        sourcingRequest.LeadNotes.Add(leadNote);
                    }
                    if (sourcingRequestViewModel.FuelPricingDetails.IsTierPricingRequired)
                    {
                        sourcingRequestViewModel.FuelPricingDetails.PricingTypeId = (int)PricingType.Tier;
                        //sourcingRequestViewModel.FuelPricingDetails.TierPricing.Pricings.ForEach(t => t.PricingTypeId = (int)PricingType.Tier);
                        var tierPricingDetails = GetPricingRequestdetails(sourcingRequestViewModel.FuelPricingDetails, sourcingRequestViewModel.AddressDetails.UOM);

                        var priceRequestEntity = tierPricingDetails.ToEntity();
                        priceRequestEntity.IsTierPricingRequired = sourcingRequestViewModel.FuelPricingDetails.IsTierPricingRequired;
                        if (priceRequestEntity.PricingTypeId == (int)PricingType.Tier)
                        {
                            priceRequestEntity.LeadPricingDetails = tierPricingDetails.TierPricing.ToTierPriceDtlEntity();
                            if (tierPricingDetails.TierPricing.IsResetCumulation && tierPricingDetails.TierPricing.TierPricingType == TierPricingType.VolumeBased)
                                priceRequestEntity.LeadCumulationDetails = tierPricingDetails.TierPricing.ResetCumulationSetting.ToCumulationDtlEntity();

                            sourcingRequest.LeadRequestPriceDetails.Add(priceRequestEntity);
                        }
                    }
                    else
                    {
                        var leadRequestPriceDetails = new LeadRequestPriceDetails();
                        leadRequestPriceDetails.IsTierPricingRequired = sourcingRequestViewModel.FuelPricingDetails.IsTierPricingRequired;
                        leadRequestPriceDetails.PricingTypeId = sourcingRequestViewModel.FuelPricingDetails.PricingTypeId;
                        leadRequestPriceDetails.ExchangeRate = sourcingRequestViewModel.FuelPricingDetails.ExchangeRate;
                        leadRequestPriceDetails.UoM = (int)sourcingRequestViewModel.AddressDetails.UOM;
                        leadRequestPriceDetails.TierTypeId = 0;
                        leadRequestPriceDetails.IsSuppressPricing = sourcingRequestViewModel.IsSupressOrderPricing;

                        var leadPricingDetail = new LeadPricingDetail();
                        leadPricingDetail.RackAvgTypeId = sourcingRequestViewModel.FuelPricingDetails.RackAvgTypeId;
                        leadPricingDetail.RackPrice = sourcingRequestViewModel.FuelPricingDetails.RackPrice;
                        leadPricingDetail.SupplierCostMarkupTypeId = sourcingRequestViewModel.FuelPricingDetails.SupplierCostMarkupTypeId;
                        leadPricingDetail.SupplierCostMarkupValue = sourcingRequestViewModel.FuelPricingDetails.SupplierCostMarkupValue;
                        leadPricingDetail.TerminalId = sourcingRequestViewModel.FuelPricingDetails.TerminalId ?? 0;
                        leadPricingDetail.TerminalName = sourcingRequestViewModel.FuelPricingDetails.TerminalName;
                        leadPricingDetail.PricingTypeId = sourcingRequestViewModel.FuelPricingDetails.PricingTypeId;
                        leadPricingDetail.PricePerGallon = sourcingRequestViewModel.FuelPricingDetails.PricePerGallon;
                        leadPricingDetail.CityGroupTerminalId = sourcingRequestViewModel.FuelPricingDetails.CityGroupTerminalId ?? 0;
                        leadPricingDetail.CityGroupTerminalName = sourcingRequestViewModel.FuelPricingDetails.CityGroupTerminalName;
                        leadPricingDetail.CityGroupTerminalStateId = sourcingRequestViewModel.FuelPricingDetails.CityGroupTerminalStateId ?? 0;
                        leadPricingDetail.CodeDescription = sourcingRequestViewModel.FuelPricingDetails.CodeDescription;
                        leadPricingDetail.CodeId = sourcingRequestViewModel.FuelPricingDetails.CodeId;
                        leadPricingDetail.Code = sourcingRequestViewModel.FuelPricingDetails.Code;
                        leadPricingDetail.EnableCityRack = sourcingRequestViewModel.FuelPricingDetails.EnableCityRack;
                        leadRequestPriceDetails.LeadPricingDetails.Add(leadPricingDetail);

                        sourcingRequest.LeadRequestPriceDetails.Add(leadRequestPriceDetails);
                    }
                    sourcingRequest.IsActive = true;
                    sourcingRequest.Status = SourcingRequestStatus.Submitted;
                    sourcingRequest.CreatedDate = DateTimeOffset.Now;
                    sourcingRequest.CreatedBy = userContext.Id;
                    sourcingRequest.SalesUserId = userContext.Id;
                    sourcingRequest.ViewedModified = true;
                    Context.DataContext.LeadRequests.Add(sourcingRequest);
                    await Context.CommitAsync();
                    sourcingRequest.DisplayRequestID = ApplicationConstants.SourceRequestPrefix + sourcingRequest.Id.ToString().PadLeft(7, '0');
                    await Context.CommitAsync();
                    response.StatusCode = Status.Success;

                }

            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("SourcingRequestDomain", "Create", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> Update(UserContext userContext, SourcingRequestViewModel sourcingRequestViewModel)
        {
            var response = new StatusViewModel();
            try
            {
                if (sourcingRequestViewModel != null && sourcingRequestViewModel.Id > 0)
                {
                    var sourcingRequest = await Context.DataContext.LeadRequests.FirstOrDefaultAsync(t => t.Id == sourcingRequestViewModel.Id);
                    if (sourcingRequest != null)
                    {
                        sourcingRequest = sourcingRequestViewModel.ToUpdateEntity(sourcingRequest);

                        if (sourcingRequest.LeadCustomerInformations != null && sourcingRequest.LeadCustomerInformations.Any())
                        {
                            Context.DataContext.LeadCustomerInformations.RemoveRange(sourcingRequest.LeadCustomerInformations);
                            var leadCustomerInformation = new LeadCustomerInformation();
                            leadCustomerInformation.CompanyId = sourcingRequestViewModel.CustomerDetails.CompanyId.HasValue ? sourcingRequestViewModel.CustomerDetails.CompanyId.Value : 0;
                            leadCustomerInformation.CompanyName = sourcingRequestViewModel.CustomerDetails.CompanyName;
                            leadCustomerInformation.Email = sourcingRequestViewModel.CustomerDetails.Email;
                            leadCustomerInformation.PhoneNumber = sourcingRequestViewModel.CustomerDetails.PhoneNumber;
                            leadCustomerInformation.IsInvitationEnabled = sourcingRequestViewModel.CustomerDetails.IsInvitationEnabled;
                            leadCustomerInformation.IsNotifyDeliveries = sourcingRequestViewModel.CustomerDetails.IsNotifyDeliveries;
                            leadCustomerInformation.IsNotifySchedules = sourcingRequestViewModel.CustomerDetails.IsNotifySchedules;
                            leadCustomerInformation.Name = sourcingRequestViewModel.CustomerDetails.Name;
                            leadCustomerInformation.UserId = sourcingRequestViewModel.CustomerDetails.UserId.HasValue ? sourcingRequestViewModel.CustomerDetails.UserId.Value : 0;
                            sourcingRequest.LeadCustomerInformations.Add(leadCustomerInformation);

                            foreach (var contactPerson in sourcingRequestViewModel.CustomerDetails.ContactPersons)
                            {
                                LeadCustomerInformation customerInformation = new LeadCustomerInformation()
                                {
                                    CompanyId = sourcingRequestViewModel.CustomerDetails.CompanyId.HasValue ? sourcingRequestViewModel.CustomerDetails.CompanyId.Value : 0,
                                    CompanyName = sourcingRequestViewModel.CustomerDetails.CompanyName,
                                    Email = contactPerson.Email,
                                    PhoneNumber = contactPerson.PhoneNumber,
                                    IsInvitationEnabled = sourcingRequestViewModel.CustomerDetails.IsInvitationEnabled,
                                    IsNotifyDeliveries = sourcingRequestViewModel.CustomerDetails.IsNotifyDeliveries,
                                    IsNotifySchedules = sourcingRequestViewModel.CustomerDetails.IsNotifySchedules,
                                    Name = contactPerson.Name,
                                    UserId = contactPerson.Id
                                };
                                sourcingRequest.LeadCustomerInformations.Add(customerInformation);
                            }
                        };
                        if (sourcingRequestViewModel.FuelDetails.Fees != null)
                        {
                            var sourceFeesModel = sourcingRequestViewModel.FuelDetails.Fees;
                            sourcingRequest.FuelFees = sourceFeesModel.ToUpdateFeesEntity(sourcingRequest.FuelFees.ToList());
                        }
                        //if (sourcingRequestViewModel.FuelPricingDetails.PricingNote != null && sourcingRequest.CreatedBy != userContext.Id)
                        //{
                        //    if (sourcingRequest.LeadPricingDetails != null)
                        //    {
                        //        var leadPricingDetails = sourcingRequest.LeadPricingDetails.First();
                        //        leadPricingDetails.PricingNotes = sourcingRequestViewModel.FuelPricingDetails.PricingNote;
                        //    }
                        //}
                        if (sourcingRequestViewModel.GeneralNote != null)
                        {
                            var leadNote = new LeadNote();
                            leadNote.GeneralNote = sourcingRequestViewModel.GeneralNote;
                            leadNote.CreatedBy = userContext.Id;
                            leadNote.CreatedDate = DateTimeOffset.Now;
                            sourcingRequest.LeadNotes.Add(leadNote);
                        }

                        if (sourcingRequest.LeadRequestPriceDetails != null && sourcingRequest.LeadRequestPriceDetails.Any())
                        {
                            var leadRequestPricingDetails = sourcingRequest.LeadRequestPriceDetails.First();

                            leadRequestPricingDetails.IsTierPricingRequired = sourcingRequestViewModel.FuelPricingDetails.IsTierPricingRequired;
                            leadRequestPricingDetails.Currency = (int)sourcingRequestViewModel.AddressDetails.Currency;
                            leadRequestPricingDetails.ExchangeRate = sourcingRequestViewModel.FuelPricingDetails.ExchangeRate;
                            leadRequestPricingDetails.TierTypeId = (int)sourcingRequestViewModel.FuelPricingDetails.TierPricing.TierPricingType;
                            leadRequestPricingDetails.IsSuppressPricing = sourcingRequestViewModel.IsSupressOrderPricing;

                            if (sourcingRequestViewModel.FuelPricingDetails.TierPricing.IsResetCumulation
                                                        && sourcingRequestViewModel.FuelPricingDetails.TierPricing.TierPricingType == TierPricingType.VolumeBased)
                            {
                                leadRequestPricingDetails.CumulationTypeId = (int)sourcingRequestViewModel.FuelPricingDetails.TierPricing.ResetCumulationSetting.CumulationType;
                                leadRequestPricingDetails.CumulationResetDate = sourcingRequestViewModel.FuelPricingDetails.TierPricing.ResetCumulationSetting.Date;
                                leadRequestPricingDetails.CumulationResetDay = (int)sourcingRequestViewModel.FuelPricingDetails.TierPricing.ResetCumulationSetting.Day;
                            }
                            if (leadRequestPricingDetails != null && sourcingRequestViewModel.FuelPricingDetails.IsTierPricingRequired)
                            {
                                leadRequestPricingDetails.PricingTypeId = (int)PricingType.Tier;
                                leadRequestPricingDetails.LeadPricingDetails = sourcingRequestViewModel.FuelPricingDetails.TierPricing.Pricings.ToUpdateTierEntity(leadRequestPricingDetails.LeadPricingDetails.ToList());
                            }
                            else
                            {
                                Context.DataContext.LeadPricingDetails.RemoveRange(leadRequestPricingDetails.LeadPricingDetails);
                                var leadPricingDetail = new LeadPricingDetail();
                                leadPricingDetail.RackAvgTypeId = sourcingRequestViewModel.FuelPricingDetails.RackAvgTypeId ?? 0;
                                leadPricingDetail.RackPrice = sourcingRequestViewModel.FuelPricingDetails.RackPrice;
                                leadPricingDetail.SupplierCostMarkupTypeId = sourcingRequestViewModel.FuelPricingDetails.SupplierCostMarkupTypeId;
                                leadPricingDetail.SupplierCostMarkupValue = sourcingRequestViewModel.FuelPricingDetails.SupplierCostMarkupValue;
                                leadPricingDetail.TerminalId = sourcingRequestViewModel.FuelPricingDetails.TerminalId;
                                leadPricingDetail.TerminalName = sourcingRequestViewModel.FuelPricingDetails.TerminalName;
                                leadRequestPricingDetails.PricingTypeId = sourcingRequestViewModel.FuelPricingDetails.PricingTypeId;
                                leadPricingDetail.PricingTypeId = sourcingRequestViewModel.FuelPricingDetails.PricingTypeId;
                                leadPricingDetail.PricePerGallon = sourcingRequestViewModel.FuelPricingDetails.PricePerGallon;
                                leadPricingDetail.CityGroupTerminalId = sourcingRequestViewModel.FuelPricingDetails.CityGroupTerminalId;
                                leadPricingDetail.CityGroupTerminalName = sourcingRequestViewModel.FuelPricingDetails.CityGroupTerminalName;
                                leadPricingDetail.CityGroupTerminalStateId = sourcingRequestViewModel.FuelPricingDetails.CityGroupTerminalStateId;
                                leadPricingDetail.CodeDescription = sourcingRequestViewModel.FuelPricingDetails.CodeDescription;
                                leadPricingDetail.CodeId = sourcingRequestViewModel.FuelPricingDetails.CodeId;
                                leadPricingDetail.Code = sourcingRequestViewModel.FuelPricingDetails.Code;
                                leadPricingDetail.EnableCityRack = sourcingRequestViewModel.FuelPricingDetails.EnableCityRack;
                                leadRequestPricingDetails.LeadPricingDetails.Add(leadPricingDetail);
                            }
                        }

                        if (sourcingRequest.Status == SourcingRequestStatus.Accepted)
                        {
                            sourcingRequest.Status = SourcingRequestStatus.AcceptedAndModified;
                        }
                        else if (sourcingRequest.Status == SourcingRequestStatus.Submitted && sourcingRequest.CreatedBy != userContext.Id)
                        {
                            sourcingRequest.Status = SourcingRequestStatus.Modified;
                        }
                        if ((sourcingRequest.Status == SourcingRequestStatus.Modified || sourcingRequest.Status == SourcingRequestStatus.AcceptedAndModified) && sourcingRequest.UpdatedBy != userContext.Id)
                        {
                            sourcingRequest.ViewedModified = false;
                        }
                        sourcingRequest.UpdatedDate = DateTimeOffset.Now;
                        sourcingRequest.UpdatedBy = userContext.Id;
                        Context.DataContext.Entry(sourcingRequest).State = EntityState.Modified;
                        await Context.CommitAsync();
                        response.StatusCode = Status.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SourcingRequestDomain", "Update", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DropdownDisplayItem>> GetSourcingCompanyContactPersons(int companyId)
        {
            List<DropdownDisplayItem> dropdownDisplayItems = new List<DropdownDisplayItem>();
            try
            {
                dropdownDisplayItems = await Context.DataContext.Users.Where(t => t.CompanyId == companyId).
                                        Select(t => new DropdownDisplayItem
                                        {
                                            Id = t.Id,
                                            Name = t.FirstName + " " + t.LastName
                                        }).OrderBy(t => t.Name).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SourcingRequestDomain", "GetSourcingCompanyContactPersons", ex.Message, ex);
            }
            return dropdownDisplayItems;
        }
        public async Task<SourceCustomerViewModel> GetSourcingContactPersonDetails(int userId)
        {
            SourceCustomerViewModel contactPersonViewModel = new SourceCustomerViewModel(Status.Success);
            try
            {
                var user = await Context.DataContext.Users.Where(t => t.Id == userId).FirstOrDefaultAsync();
                if (user != null)
                {
                    contactPersonViewModel.PhoneNumber = user.PhoneNumber;
                    contactPersonViewModel.Email = user.Email;
                    contactPersonViewModel.Name = $"{user.FirstName} {user.LastName}";
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SourcingRequestDomain", "GetSourcingContactPersonDetails", ex.Message, ex);
            }
            return contactPersonViewModel;
        }

        public async Task<SourcingRequestViewModel> GetSourcingJobDetails(string jobName, string companyName, UserContext userContext)
        {
            SourcingRequestViewModel response = new SourcingRequestViewModel();
            try
            {
                var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.IsActive && t.Name == jobName && t.Company.Name == companyName);
                if (job != null)
                {
                    response.AddressDetails = job.ToSourceViewModel();
                    response.AddressDetails.IsCompanyOwned = Context.DataContext.SupplierXBuyerDetails.Any(t => t.JobId == job.Id
                                                                                                    && t.BuyerCompanyId == job.CompanyId
                                                                                                    && t.SupplierCompanyId == userContext.CompanyId
                                                                                                    && t.CompanyOwnedLocation && t.IsActive);
                    response.AccountingCompanyId = Context.DataContext.SupplierXBuyerDetails.Where(t => t.JobId == job.Id
                                                                                                    && t.SupplierCompanyId == userContext.CompanyId
                                                                                                    && t.BuyerCompanyId == job.CompanyId)
                                                                                                    .Select(t => t.AccountingCompanyId).FirstOrDefault();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SourcingRequestDomain", "GetSourcingJobDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<SourcingRequestGridViewModel>> GetSourcingRequestGrid(int companyId, SourcingRequestDisplayStatus RequestStatus, bool isFromDashboard)
        {
            var response = new List<SourcingRequestGridViewModel>();
            try
            {

                var GetSourcingRequestGrid = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetSourcingRequestGrid(companyId, isFromDashboard);

                if (GetSourcingRequestGrid != null && GetSourcingRequestGrid.Any())
                {
                    GetSourcingRequestGrid.ForEach(t => response.Add(t.ToGridViewModel()));
                    if (response != null && response.Any())
                    {
                        if (RequestStatus == SourcingRequestDisplayStatus.New)
                        {

                            response = response.FindAll(t => t.Status == EnumHelperMethods.GetDisplayName(SourcingRequestStatus.Submitted));
                        }
                        else if (RequestStatus == SourcingRequestDisplayStatus.WIP)
                        {
                            response = response.FindAll(t => t.Status == SourcingRequestStatus.Modified.ToString());
                        }
                        else if (RequestStatus == SourcingRequestDisplayStatus.Sourced)
                        {
                            response = response.Where(t => t.Status.Contains(SourcingRequestStatus.Accepted.ToString())).ToList();
                        }
                        else if (RequestStatus == SourcingRequestDisplayStatus.Lost)
                        {
                            response = response.FindAll(t => t.Status == SourcingRequestStatus.Lost.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("SourcingRequestDomain", "GetSourcingRequestGrid", ex.Message, ex);
            }
            return response;
        }
        public async Task<SourcingRequestViewModel> GetRequestDetails(int Id)
        {
            SourcingRequestViewModel sourcingRequestViewModel = new SourcingRequestViewModel();
            try
            {
                var sourcingRequest = await Context.DataContext.LeadRequests.FirstOrDefaultAsync(t => t.Id == Id);
                if (sourcingRequest != null)
                {
                    sourcingRequestViewModel = sourcingRequest.ToViewModel();
                }
                if (sourcingRequest != null && sourcingRequest.FuelFees != null)
                {
                    sourcingRequestViewModel.FuelDetails.Fees = sourcingRequest.FuelFees.ToSourcingFeesViewModel();
                }

                var getGeneralNotes = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetSourcingNotes(Id);
                if (getGeneralNotes != null && getGeneralNotes.Any())
                {
                    getGeneralNotes.ForEach(t => sourcingRequestViewModel.GeneralNotesHistory.Add(t.ToGeneralNotes()));
                }
                sourcingRequestViewModel.StatusCode = Status.Success;
                sourcingRequestViewModel.StatusMessage = Resource.errMessageSuccess;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SourcingRequestDomain", "GetRequestDetails", ex.Message, ex);
            }
            return sourcingRequestViewModel;
        }
        public async Task<SourcingDetailViewModel> GetSourcingDetails(int Id)
        {
            SourcingDetailViewModel sourcingDetailsViewModel = new SourcingDetailViewModel();
            try
            {
                var sourcingRequest = await Context.DataContext.LeadRequests.FirstOrDefaultAsync(t => t.Id == Id);
                if (sourcingRequest != null)
                {
                    sourcingDetailsViewModel = sourcingRequest.ToModel();
                    var leadAddressDetails = sourcingRequest.LeadAddressDetail.First();
                    sourcingDetailsViewModel.AddressDetails.State = await GetStateById(leadAddressDetails.StateId);
                    sourcingDetailsViewModel.AddressDetails.DispatchRegion = GetDispatcherDetails(leadAddressDetails.DispatchRegionId);
                }
                if (sourcingRequest != null && sourcingRequest.FuelFees != null && sourcingRequest.LeadFuelDetails != null)
                {
                    sourcingDetailsViewModel.FuelDetails.sourceFeesViewModel = sourcingRequest.FuelFees.ToFeesModel();
                    var leadFuelDetails = sourcingRequest.LeadFuelDetails.First();
                    sourcingDetailsViewModel.FuelDetails.FuelType = await GetProductDetails(leadFuelDetails.FuelTypeId);
                }

                var getGeneralNotes = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetSourcingNotes(Id);
                if (getGeneralNotes != null && getGeneralNotes.Any())
                {
                    getGeneralNotes.ForEach(t => sourcingDetailsViewModel.GeneralNotesHistory.Add(t.ToGeneralNotes()));
                }
                sourcingDetailsViewModel.StatusCode = Status.Success;
                sourcingDetailsViewModel.StatusMessage = Resource.errMessageSuccess;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SourcingRequestDomain", "GetSourcingDetails", ex.Message, ex);
            }
            return sourcingDetailsViewModel;
        }
        public async Task<List<SalesUserDashboardOrderViewModel>> GetSalesUserOrders(int companyId)
        {
            var response = new List<SalesUserDashboardOrderViewModel>();
            try
            {
                var orders = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetSalesUserOrders(companyId);
                if (orders != null && orders.Any())
                {
                    orders.ForEach(t => response.Add(t.ToOrdersModel()));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SourcingRequestDomain", "GetSalesUserOrders", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ChangesSourcingRequestStatus(SourcingRequestStatus sourcingRequestStatus, int Id, int UserId)
        {
            var response = new StatusViewModel();
            try
            {
                var request = await Context.DataContext.LeadRequests.Where(t => t.Id == Id).FirstOrDefaultAsync();
                if (request != null)
                {
                    request.Status = sourcingRequestStatus;
                    await Context.CommitAsync();
                    response.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SourcingRequestDomain", "SetSourcingRequestStatusToAccepted", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> CreateOrderFromSourcingRequest(UserContext userContext, SourcingRequestViewModel sourcingRequestViewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var thirdPartyOrderViewModel = sourcingRequestViewModel.ToTPOViewmodel();
                response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().CreateThirdPartyOrder(userContext, thirdPartyOrderViewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SourcingRequestDomain", "CreateOrderFromSourcingRequest", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> UpdateViewedStatus(int Id, int UserId, bool IsViewed = true)
        {
            var response = new StatusViewModel();
            try
            {
                var request = await Context.DataContext.LeadRequests.Where(t => t.Id == Id).FirstOrDefaultAsync();
                if (request != null && request.UpdatedBy != UserId)
                {
                    request.ViewedModified = IsViewed;
                    await Context.CommitAsync();
                    response.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SourcingRequestDomain", "UpdateViewedStatus", ex.Message, ex);
            }
            return response;
        }

        private SourcingPricingRequestViewModel GetPricingRequestdetails(SourcingPricingDetailsViewModel fuelPricing, UoM Uom)
        {
            SetTierPricingRequestDetails(fuelPricing);
            if (fuelPricing.PricingTypeId == (int)PricingType.PricePerGallon)
            {
                var viewModel = new SourcingPricingRequestViewModel()
                {
                    PricePerGallon = fuelPricing.PricePerGallon,
                    PricingCodeId = fuelPricing.FuelPricingDetails.PricingCode.Id,
                    Currency = (int)fuelPricing.Currency,
                    UoM = (int)Uom,
                    ExchangeRate = fuelPricing.ExchangeRate,
                    FuelTypeId = fuelPricing.FuelTypeId,
                    PricingTypeId = fuelPricing.PricingTypeId,
                    TerminalId = fuelPricing.TerminalId,
                    ParameterJson = fuelPricing.ParameterJSON,
                };
                return viewModel;
            }
            else if (fuelPricing.PricingTypeId == (int)PricingType.Suppliercost)
            {
                var viewModel = new SourcingPricingRequestViewModel()
                {
                    PricePerGallon = fuelPricing.SupplierCostMarkupValue,
                    PricingCodeId = fuelPricing.FuelPricingDetails.PricingCode.Id,
                    Currency = (int)fuelPricing.Currency,
                    UoM = (int)Uom,
                    ExchangeRate = fuelPricing.ExchangeRate,
                    RackAvgTypeId = fuelPricing.SupplierCostMarkupTypeId,
                    SupplierCostTypeId = (int)SupplierCostTypes.GlobalCost,
                    SupplierCost = fuelPricing.SupplierCost,
                    BaseSupplierCost = fuelPricing.SupplierCost.HasValue ? MoneyConverter.GetBaseAmount(fuelPricing.Currency, fuelPricing.SupplierCost.Value, fuelPricing.ExchangeRate) : 0,
                    FuelTypeId = fuelPricing.FuelTypeId,
                    PricingTypeId = fuelPricing.PricingTypeId,
                    TerminalId = fuelPricing.TerminalId,
                    ParameterJson = fuelPricing.ParameterJSON,
                };
                return viewModel;
            }
            else if (fuelPricing.PricingTypeId == (int)PricingType.Tier)
            {
                var viewModel = new SourcingPricingRequestViewModel()
                {
                    Currency = (int)fuelPricing.Currency,
                    UoM = (int)Uom,
                    PricingTypeId = fuelPricing.PricingTypeId,
                    ExchangeRate = fuelPricing.ExchangeRate,
                    TierPricing = fuelPricing.TierPricing,

                };
                return viewModel;
            }
            else
            {
                var viewModel = new SourcingPricingRequestViewModel()
                {
                    PricePerGallon = fuelPricing.RackPrice,
                    PricingCodeId = fuelPricing.FuelPricingDetails.PricingCode.Id,
                    RackAvgTypeId = fuelPricing.RackAvgTypeId,
                    Currency = (int)fuelPricing.Currency,
                    UoM = (int)Uom,
                    ExchangeRate = fuelPricing.ExchangeRate,
                    FuelTypeId = fuelPricing.FuelTypeId,
                    PricingTypeId = fuelPricing.PricingTypeId,
                    TerminalId = fuelPricing.TerminalId,
                    ParameterJson = fuelPricing.ParameterJSON,
                };

                return viewModel;
            }
        }
        private void SetTierPricingRequestDetails(SourcingPricingDetailsViewModel fuelPricing)
        {
            if (fuelPricing != null && fuelPricing.PricingTypeId == (int)PricingType.Tier)
            {
                foreach (var item in fuelPricing.TierPricing.Pricings)
                {
                    if (item.PricingTypeId == (int)PricingType.PricePerGallon)
                    {
                        item.PricePerGallon = item.PricePerGallon;
                    }
                    else if (item.PricingTypeId == (int)PricingType.Suppliercost)
                    {
                        item.PricePerGallon = item.SupplierCostMarkupValue;
                        item.RackAvgTypeId = fuelPricing.SupplierCostMarkupTypeId;
                        item.SupplierCostTypeId = (int)SupplierCostTypes.GlobalCost;
                        item.SupplierCost = fuelPricing.SupplierCost;
                        item.BaseSupplierCost = fuelPricing.SupplierCost.HasValue ? MoneyConverter.GetBaseAmount(fuelPricing.Currency, fuelPricing.SupplierCost.Value, fuelPricing.ExchangeRate) : 0;
                    }
                    else
                    {
                        item.PricePerGallon = item.PricePerGallon;
                    }
                }
            }
        }
        public async Task<string> GetStateById(int stateId)
        {
            var response = string.Empty;
            try
            {
                response = await Context.DataContext.MstStates.Where(t => t.Id == stateId).Select(t => t.Name).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SourcingRequestDomain", "GetStateById", ex.Message, ex);
            }
            return response;
        }
        public async Task<string> GetProductDetails(int productId)
        {
            var response = string.Empty;
            try
            {
                response = await Context.DataContext.MstTfxProducts.Where(t => t.IsActive && t.Id == productId).Select(t => t.Name).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SourcingRequestDomain", "GetProductDetails", ex.Message, ex);
            }
            return response;
        }
        public string GetDispatcherDetails(string DispatchRegionId)
        {
            var regionName = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(DispatchRegionId))
                {
                    var freightServiceDomain = new FreightServiceDomain(this);
                    regionName = Task.Run(() => freightServiceDomain.GetRegionName(DispatchRegionId)).Result;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatcherDomain", "GetDispatcherDetailsAsync", ex.Message, ex);
            }
            return regionName;
        }
        public async Task<List<InvoiceGridSalesUserDashboardModel>> GetSalesInvoiceDashboard(int companyId, int InvoiceTypeId)
        {
            var response = new List<InvoiceGridSalesUserDashboardModel>();
            try
            {
                var invoices = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetSalesInvoiceDashboard(companyId, InvoiceTypeId);
                if (invoices != null && invoices.Any())
                {
                    invoices.ForEach(t => response.Add(t.ToInvoiceModel()));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SourcingRequestDomain", "GetSalesUserOrders", ex.Message, ex);
            }
            return response;
        }
    }
}
