using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using Stripe;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SiteFuel.Exchange.Domain
{
    public class OnboardingDomain : BaseDomain
    {
        public OnboardingDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public OnboardingDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<OnboardingViewModel> GetOnboardingViewModelAsync(int userId)
        {
            OnboardingViewModel response = new OnboardingViewModel();
            try
            {
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId);
                if (user != null && user.Company != null)
                {
                    string staticPassword = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingApplicationUserDefaultPassword, "First#1234");
                    response.User = new RegisterViewModel() { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, MobileNumber = user.PhoneNumber, Password = staticPassword, ConfirmPassword = staticPassword, Title = user.Title };
                    response.User.Company = user.Company.ToViewModel();

                    response.Card = new PaymentCardViewModel
                    {
                        IsPrimary = true
                    };

                    response.CompanyAddress.IsOnboarding = true;
                    response.User.Company.IsOnboarding = true;
                    response.PreferencesSetting.ForcastingPreference.IsEditable = true;

                    //if (user.Company.CompanyTypeId != (int)CompanyType.Buyer)
                    //{
                    var companyAddress = user.Company.CompanyAddresses.FirstOrDefault(t => t.IsActive); // in wizard case
                    if (companyAddress != null)
                    {
                        response.CompanyAddress = companyAddress.ToViewModel();
                        if (user.Company.CompanyTypeId != (int)CompanyType.Buyer)
                        {
                            response.CompanyAddress.ServiceOffering = companyAddress.CompanyXServingLocations.ToList().ToViewModel();
                        }
                    }
                    else
                    {
                        if (user.Company.CompanyTypeId != (int)CompanyType.Buyer)
                        {
                            response.CompanyAddress.ServiceOffering = Enum.GetValues(typeof(ServiceOfferingType)).Cast<ServiceOfferingType>().Select(c => new CompanyServiceAreaModel { ServiceDeliveryType = c }).ToList();
                        }
                    }
                    //}
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OnboardingDomain", "GetOnboardingViewModelAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<Response> SaveOnboardingViewModelAsync(OnboardingViewModel viewModel, UserContext userContext)
        {
            Response response = new Response(Status.Failed);
            using (var trasaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    CompanyAddress existingCompanyAddress;
                    User user;
                    if (viewModel.IsExternalSupplier || viewModel.User.Company.Id == 0)
                    {
                        SuperAdminDomain superAdminDomain = new SuperAdminDomain(this);
                        if (!viewModel.IsExternalSupplier)
                        {
                            user = await superAdminDomain.CreateUserCompany(viewModel, userContext.Id);
                        }
                        else
                        {
                            user = await superAdminDomain.ExternalSupplierToInternal(viewModel, userContext.Id, userContext.Name);
                        }
                        viewModel.User.Company.Id = user.Company.Id;
                        viewModel.User.Id = user.Id;
                        viewModel.CompanyId = user.Company.Id;
                    }
                    else
                    {
                        user = await Context.DataContext.Users.Include(t => t.Company).Include(t => t.Company.CompanyAddresses)
                                        .Include(t => t.Company.Image)
                                        .Include(t => t.Company.BillingAddresses).SingleOrDefaultAsync(t => t.Id == viewModel.User.Id);
                    }
                    if (user != null)
                    {
                        user.Company = viewModel.User.Company.ToEntity(user.Company);
                        if (viewModel.User.Company.CompanyLogo.Id == 0 && !string.IsNullOrWhiteSpace(viewModel.User.Company.CompanyLogo?.FilePath))
                        {
                            user.Company.Image = viewModel.User.Company.CompanyLogo.ToEntity();
                        }

                        var stateCode = Context.DataContext.MstStates.First(t => t.Id == viewModel.CompanyAddress.State.Id).Code;
                        var countryCode = Context.DataContext.MstCountries.First(t => t.Id == viewModel.CompanyAddress.Country.Id).Code;
                        var point = GoogleApiDomain.GetGeocode($"{viewModel.CompanyAddress.Address} {viewModel.CompanyAddress.Address2} {viewModel.CompanyAddress.Address3} {viewModel.CompanyAddress.City} {stateCode} {countryCode} {viewModel.CompanyAddress.ZipCode}");


                        var companyAddress = viewModel.CompanyAddress.ToEntity();
                        if (point != null)
                        {
                            companyAddress.Latitude = Convert.ToDecimal(point.Latitude);
                            companyAddress.Longitude = Convert.ToDecimal(point.Longitude);
                            existingCompanyAddress = user.Company.CompanyAddresses.FirstOrDefault(t => t.Address.ToLower() == viewModel.CompanyAddress.Address.ToLower() &&
                                                                                                       t.City.ToLower() == viewModel.CompanyAddress.City.ToLower() &&
                                                                                                       t.StateId == viewModel.CompanyAddress.State.Id &&
                                                                                                       t.CountryId == viewModel.CompanyAddress.Country.Id &&
                                                                                                       t.ZipCode == viewModel.CompanyAddress.ZipCode);
                            if (existingCompanyAddress != null)
                            {
                                user.Company.CompanyAddresses.ToList().ForEach(t => t.IsDefault = false);
                                user.Company.CompanyAddresses.Where(t => t.Id == existingCompanyAddress.Id).First().IsDefault = true;
                            }
                            else
                            {
                                user.Company.CompanyAddresses.ToList().ForEach(t => t.IsDefault = false);
                                user.Company.CompanyAddresses.Add(companyAddress);
                            }
                        }
                        else
                        {
                            trasaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessages.Add(Resource.errMessageSaveFailedInvalidAddress);
                            return response;
                        }

                        //// save company billing address
                        if (user.Company.BillingAddresses == null || !user.Company.BillingAddresses.Any() && !string.IsNullOrWhiteSpace(viewModel.BillingAddress.Address))
                        {
                            if (userContext.IsSuperAdmin || userContext.IsBuyerAdmin || userContext.IsSupplierAdmin || userContext.IsAccountingPerson || userContext.IsCarrierAdmin)
                            {
                                //var billingStateCode = Context.DataContext.MstStates.First(t => t.Id == viewModel.BillingAddress.State.Id).Code;
                                //var billingCountryCode = Context.DataContext.MstCountries.First(t => t.Id == viewModel.BillingAddress.Country.Id).Code;
                                var billingPoint = GoogleApiDomain.GetGeocode($"{viewModel.BillingAddress.Address} {viewModel.CompanyAddress.Address2} {viewModel.CompanyAddress.Address3} {viewModel.BillingAddress.City} {viewModel.BillingAddress.County} {viewModel.BillingAddress.ZipCode} {viewModel.BillingAddress.State.Name} {viewModel.BillingAddress.Country.Name}");
                                //if (billingPoint == null)
                                //{
                                //    trasaction.Rollback();
                                //    response.StatusCode = Status.Failed;
                                //    response.StatusMessages.Add(Resource.errMessageSaveFailedInvalidBillingAddress);
                                //    return response;
                                //}

                                var companyBillingAddress = viewModel.BillingAddress.ToEntity();
                                companyBillingAddress.IsDefault = true;
                                if (billingPoint != null)
                                {
                                    companyBillingAddress.Latitude = Convert.ToDecimal(billingPoint.Latitude);
                                    companyBillingAddress.Longitude = Convert.ToDecimal(billingPoint.Longitude);
                                }

                                user.Company.BillingAddresses.Add(companyBillingAddress);
                            }
                        }

                        if (user.Company.CompanyTypeId != (int)CompanyType.Buyer)
                        {
                            var compAddress = user.Company.CompanyAddresses.First(t => t.IsDefault);
                            compAddress.MstProductTypes.Clear();
                            var productTypes = Context.DataContext.MstProductTypes.Where(t => viewModel.CompanyAddress.SupplierProductTypes.Contains(t.Id)).ToList();
                            productTypes.ForEach(t => compAddress.MstProductTypes.Add(t));

                            foreach (var pAddress in compAddress.CompanyXServingLocations.ToList())
                            {
                                Context.DataContext.CompanyXServingLocations.Remove(pAddress);
                            }
                            Context.Commit();
                            var settingDomain = new SettingsDomain(this);
                            var serviceOffering = await settingDomain.GetServiceOfferingModel(viewModel.CompanyAddress.ServiceOffering);
                            var companyServingLocation = serviceOffering.Where(t => t.IsEnable).ToList().ToEntity();
                            foreach (var location in companyServingLocation)
                            {
                                compAddress.CompanyXServingLocations.Add(location);
                            }
                            await Context.CommitAsync();
                            response.StatusCode = Status.Success;
                            user.Company.SupplierCode = GetRandomString();
                        }

                        viewModel.Card.IsPrimary = true;

                        var state = await Context.DataContext.MstStates.SingleOrDefaultAsync(t => t.Id == viewModel.State.Id);
                        var country = await Context.DataContext.MstCountries.SingleOrDefaultAsync(t => t.Id == viewModel.Country.Id);
                        if (state != null)
                        {
                            viewModel.State.Code = state.Code;
                        }

                        if (country != null)
                        {
                            viewModel.Country.Code = country.Code;
                        }

                        //if (!viewModel.Card.BypassPaymentDetails)
                        //{
                        //    var companyUserStripeCard = viewModel.ToCompanyUserXStripeCardEntity();
                        //    Context.DataContext.CompanyXStripeCards.Add(companyUserStripeCard);
                        //}

                        if (response.StatusMessages.Count == 0)
                        {
                            user.IsOnboardingComplete = true;
                            user.OnboardedDate = DateTimeOffset.Now;
                            user.IsTaxExemptDisplayed = true;

                            Context.DataContext.UserXInvites.Where(t => t.Email == user.Email).ToList().ForEach(t =>
                            {
                                t.FirstName = user.FirstName;
                                t.LastName = user.LastName;
                                t.IsOnboarded = true;
                                t.IsInvitationSent = true;
                            });

                            if (viewModel.User.Company.CompanyTypeId == (int)CompanyType.Carrier || viewModel.User.Company.CompanyTypeId == (int)CompanyType.SupplierAndCarrier)
                            {
                                //user.Company.OnboardingQuestionAnswers = GetOnboardingQuestionAnswers(userContext, user, viewModel);
                                user.Company.HaulerPricingMatrices = GetHaulerPricingMatrices(userContext, user, viewModel);
                            }
                            else
                            {
                                //user.Company.OnboardingQuestionAnswers.Clear();
                                user.Company.HaulerPricingMatrices.Clear();
                            }

                            // save onboarding preferences settings
                            if (!viewModel.PreferencesSetting.IsByPassPreferencesSetting)
                            {
                                viewModel.PreferencesSetting.CreatedBy = userContext.Id;
                                viewModel.PreferencesSetting.CreatedDate = DateTimeOffset.Now;
                                var prefResponse = await SavePreferencesSetting(viewModel.PreferencesSetting, userContext, user);
                                if (prefResponse.StatusCode == Status.Failed)
                                {
                                    trasaction.Rollback();
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessages.Add(prefResponse.StatusMessage);

                                    return response;
                                }
                            }

                            await Context.CommitAsync();
                            trasaction.Commit();

                            var notificationDomain = new NotificationDomain(this);
                            var triggeredBy = user.Id;
                            if (Context.DataContext.UserXInvites.Any(t => t.Email == user.Email))
                            {
                                var invitedBy = Context.DataContext.UserXInvites.FirstOrDefault(t => t.Email == user.Email);
                                triggeredBy = invitedBy.InvitedBy;
                            }

                            if (!string.IsNullOrEmpty(viewModel.User.SupplierURL))
                            {
                                var message = new AddUserMessageViewModel { SupplierURL = viewModel.User.SupplierURL };
                                var jsonMessage = new JavaScriptSerializer().Serialize(message);
                                await notificationDomain.AddNotificationEventAsync(EventType.ExternalCompanyInviteOnboarded, user.Id, triggeredBy, null, jsonMessage, userContext.ApplicationTemplateId);
                            }
                            else
                            {
                                await notificationDomain.AddNotificationEventAsync(EventType.ExternalCompanyInviteOnboarded, triggeredBy, user.Id, null, null, userContext.ApplicationTemplateId);
                            }
                            if (user.Company.OnboardingPreferences.Where(top => top.IsActive).FirstOrDefault() != null)
                            {
                                response.EntityId = user.Company.OnboardingPreferences.Where(top => top.IsActive).FirstOrDefault().Id;
                            }
                            response.StatusCode = Status.Success;
                            response.StatusMessages.Add(Resource.errMessageOnboardingUserSuccess);
                        }
                    }
                }
                catch (StripeException ex)
                {
                    trasaction.Rollback();
                    if (ex.StripeError.DeclineCode == "live_mode_test_card" && ex.StripeError.ErrorType == "card_error")
                    {
                        response.StatusMessages.Add(Resource.errMessageTestCardUsedInLive);
                    }
                    LogManager.Logger.WriteException("OnboardingDomain", "SaveOnboardingViewModelAsync", ex.Message, ex);
                }
                catch (Exception ex)
                {
                    trasaction.Rollback();
                    LogManager.Logger.WriteException("OnboardingDomain", "SaveOnboardingViewModelAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<OnboardingPreferenceViewModel> SavePreferencesSetting(OnboardingPreferenceViewModel viewModel, UserContext userContext, User user)
        {
            try
            {
                viewModel.CompanyId = userContext.CompanyId;
                viewModel.UserId = userContext.Id;
                viewModel.UpdatedBy = userContext.Id;
                viewModel.UpdatedDate = DateTimeOffset.Now;
                viewModel.IsActive = true;

                var onboardingPreference = viewModel.ToEntity();
                user.Company.OnboardingPreferences.Add(onboardingPreference);

                if (viewModel.BuyersToSendReceipts != null && viewModel.BuyersToSendReceipts.Any())
                {
                    await Context.CommitAsync();
                    foreach (var buyer in viewModel.BuyersToSendReceipts)
                    {
                        buyer.OnboardingPreferenceId = onboardingPreference.Id;
                        buyer.IsActive = true;
                        var buyerReceiptEntity = buyer.ToEntity();
                        onboardingPreference.BuyerXOnboardingPreferences.Add(buyerReceiptEntity);
                    }
                }

                viewModel.StatusCode = Status.Success;
                viewModel.StatusMessage = Resource.successMessageSavePreferencesSetting;
            }
            catch (Exception ex)
            {
                viewModel.StatusCode = Status.Failed;
                viewModel.StatusMessage = Resource.errorMessageSaveReferencesSetting;
                LogManager.Logger.WriteException("OnboardingDomain", "SavePreferencesSetting", ex.Message, ex);
            }

            return viewModel;
        }

        public async Task SentCredentialsEmailToCompanyUser(int userId, bool IsAccountSfxOwned, int companyId)
        {
            try
            {
                var serverUrl = ContextFactory.Current.GetDomain<HelperDomain>().GetServerUrl();
                var callbackUrl = $"~/Account/Login";
                var notification = ContextFactory.Current.GetDomain<NotificationDomain>().GetNotificationContent(EventSubType.SuperAdminOnboardedNewCompany, serverUrl, callbackUrl);
                var emailTemplate = ContextFactory.Current.GetDomain<HelperDomain>().GetApplicationEventNotificationTemplate();
                var companyUser = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetUserByIdAsync(userId);
                var StaticPassword = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingApplicationUserDefaultPassword, "First#1234");
                var RandomPassword = CryptoHelperMethods.GeneratePassword(Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings[ApplicationConstants.RandomPasswordLength]), StaticPassword);
                await ContextFactory.Current.GetDomain<AuthenticationDomain>().UpdatePasswordAsync(userId, RandomPassword);

                if (IsAccountSfxOwned)
                {
                    // if SFX Owned, block all emails notifications
                    await ContextFactory.Current.GetDomain<SettingsDomain>().ToggleBlockUserNotificationsAsync(companyId, true);
                }
                else
                {
                    var emailModel = new ApplicationEventNotificationViewModel
                    {
                        To = new List<string> { companyUser.Email },
                        Subject = notification.Subject,
                        CompanyLogo = notification.CompanyLogo,
                        CompanyText = notification.CompanyText,
                        BodyLogo = notification.BodyLogo,
                        BodyText = string.Format(notification.BodyText, $"{companyUser.FirstName} {companyUser.LastName}", companyUser.Email, RandomPassword),
                        BodyButtonText = notification.BodyButtonText,
                        BodyButtonUrl = notification.BodyButtonUrl
                    };
                    await ContextFactory.Current.GetDomain<EmailDomain>().SendEmail(emailTemplate, emailModel);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OnboardingDomain", "SentCredentialsEmailToCompanyUser", ex.Message, ex);
            }
        }


        //private List<OnboardingQuestionAnswer> GetOnboardingQuestionAnswers(UserContext userContext, User user, OnboardingViewModel viewModel)
        //{
        //    var onboardingQuestions = new List<OnboardingQuestionAnswer>();
        //    var companyQuestionAnswers = viewModel.User.Company.Questions.Where(t => !string.IsNullOrWhiteSpace(t.Answer)).
        //        Select(t => new OnboardingQuestionAnswer
        //        {
        //            CompanyId = user.Company.Id,
        //            OnboardingQuestionId = t.Id,
        //            QuestionAnswer = t.Answer,
        //            UpdatedBy = userContext.Id,
        //            UpdatedDate = DateTimeOffset.Now
        //        });
        //    var serviceQuestionAnswers = viewModel.ServiceQuestions.Where(t => !string.IsNullOrWhiteSpace(t.Answer)).
        //        Select(t => new OnboardingQuestionAnswer
        //        {
        //            CompanyId = user.Company.Id,
        //            OnboardingQuestionId = t.Id,
        //            QuestionAnswer = t.Answer,
        //            UpdatedBy = userContext.Id,
        //            UpdatedDate = DateTimeOffset.Now
        //        });
        //    onboardingQuestions.AddRange(companyQuestionAnswers);
        //    onboardingQuestions.AddRange(serviceQuestionAnswers);

        //    return onboardingQuestions;
        //}

        private ICollection<HaulerPricingMatrix> GetHaulerPricingMatrices(UserContext userContext, User user, OnboardingViewModel viewModel)
        {
            var pricingMatrices = new List<HaulerPricingMatrix>();
            foreach (var item in viewModel.HaulerPricingMatrices)
            {
                var pricingMatrix = new HaulerPricingMatrix();
                pricingMatrix.Id = item.Id;
                pricingMatrix.CompanyId = user.CompanyId.Value;
                pricingMatrix.MinGallons = item.MinGallons;
                pricingMatrix.MaxGallons = item.MaxGallons;
                pricingMatrix.Price = item.Price;
                pricingMatrix.IsActive = true;
                pricingMatrix.CreatedBy = userContext.Id;
                pricingMatrix.CreatedDate = DateTimeOffset.Now;
                pricingMatrices.Add(pricingMatrix);
            }
            return pricingMatrices;
        }

        private string GetRandomString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[5];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return "S" + finalString;
        }

        #region Customer API
        public async Task<ApiResponseViewModel> CreateCustomerFromApi(TPDCustomerViewModel viewModel, string token)
        {
            ApiResponseViewModel apiResponse = new ApiResponseViewModel();

            try
            {
                //get userdetails from token
                var authDomain = new AuthenticationDomain(this);
                var apiUserContext = await authDomain.GetUserContextFromTokenAsync(token);
                if (apiUserContext != null)
                {
                    var customerCompanyId = await GetApiCustomerCompanyIdByRefIdOrTfxCompanyId(viewModel.TFXCompanyId, viewModel.ExternalRefID, apiUserContext, apiResponse);
                    if (customerCompanyId > 0)
                    {
                        //update flow
                        CompanyAddress compAddress = null;

                        //validate required parameters
                        ValidateRequiredParameters(viewModel, apiResponse, apiUserContext);
                        if (!apiResponse.Messages.Any())
                        {
                            //ValidateTFXCompanyIdAndExternalRefId(viewModel, apiResponse, apiUserContext);
                            await ValidateCompanyName(apiResponse, viewModel, customerCompanyId);
                            ValidateAddress(apiResponse, viewModel, ref compAddress, apiUserContext);
                            var existingBillingAddress = Context.DataContext.BillingAddresses.Where(t => t.CompanyId == customerCompanyId && t.IsActive).ToList();
                            ValidateAndAddBillingAddressesToList(viewModel, apiUserContext, existingBillingAddress, apiResponse, customerCompanyId);
                            ValidateUsersList(apiResponse, viewModel, customerCompanyId);

                            if (!apiResponse.Messages.Any())
                            {
                                await UpdateCustomerCompanyDetails(apiResponse, viewModel, customerCompanyId, apiUserContext, existingBillingAddress);
                            }
                        }
                    }
                    else
                    {
                        //create flow

                        //default viewmodel/entities required after validaiton
                        CompanyAddress compAddress = null;
                        List<BillingAddress> billingAddresses = new List<BillingAddress>();

                        //validate viewmodel
                        await ValidateCompanyName(apiResponse, viewModel);
                        ValidateAddress(apiResponse, viewModel, ref compAddress, apiUserContext);
                        ValidateUsersList(apiResponse, viewModel);
                        ValidateContactNumber(apiResponse, viewModel);
                        await ValidateExternalRefId(apiResponse, viewModel, apiUserContext);
                        ValidateAndAddBillingAddressesToList(viewModel, apiUserContext, billingAddresses, apiResponse, customerCompanyId);

                        //save input request
                        int LengthOfPassword = Constants.LengthOfPassword;
                        var RandomPassword = CryptoHelperMethods.GenerateRandomPassword(LengthOfPassword);
                        var TempPassword = CryptoHelperMethods.EncryptPassword(Constants.Key.ToString(), RandomPassword);
                        var buyerUser = new List<User>();

                        if (!apiResponse.Messages.Any())
                        {
                            foreach (var customer in viewModel.Users)
                            {
                                var existingUser = Context.DataContext.Users.FirstOrDefault(t => t.Email.ToLower().Equals(customer.Email.ToLower().Trim()));
                                if (existingUser == null)
                                {
                                    var salt = CryptoHelperMethods.GenerateSalt();
                                    User user = new User
                                    {
                                        FirstName = customer.FirstName,
                                        LastName = customer.LastName,
                                        UserName = customer.Email.Trim().ToLower(), //viewModel.CustomerDetails.Email.Trim().ToLower(),
                                        Email = customer.Email.Trim().ToLower(),//viewModel.CustomerDetails.Email.Trim().ToLower(),
                                        IsEmailConfirmed = false,
                                        PhoneNumber = customer.PhoneNumber,
                                        IsPhoneNumberConfirmed = ContextFactory.Current.GetDomain<NotificationDomain>().IsPhoneNumberValid(customer.PhoneNumber),
                                        IsTwoFactorEnabled = false,
                                        AccessFailedCount = 0,
                                        IsLockoutEnabled = true,
                                        LockoutEndDateUtc = null,
                                        PasswordHash = CryptoHelperMethods.GenerateHash(RandomPassword, salt),
                                        SecurityStamp = salt,
                                        FingerPrint = CryptoHelperMethods.GenerateHash(customer.Email, CryptoHelperMethods.GenerateSalt()),
                                        IsOnboardingComplete = false,
                                        IsActive = true,
                                        CreatedBy = apiUserContext.Id,
                                        CreatedDate = DateTimeOffset.Now,
                                        UpdatedBy = apiUserContext.Id,
                                        UpdatedDate = DateTimeOffset.Now,
                                        OnboardedTypeId = (int)OnboardedType.ThirdPartyOrderOnboarded
                                    };
                                    buyerUser.Add(user);
                                }
                                else
                                {
                                    apiResponse.Messages.Add(new ApiCodeMessages()
                                    {
                                        Code = Constants.ApiCodeRQ02,
                                        Message = Resource.valMessageEmailAlreadyExists + " " + customer.Email
                                    });
                                    apiResponse.Status = Status.Failed;
                                }
                            }

                            if (!apiResponse.Messages.Any())
                                await AddCompanyUsersAndAddresses(viewModel, apiResponse, authDomain, apiUserContext, compAddress, billingAddresses, TempPassword, buyerUser);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                apiResponse.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeEV01,
                    Message = Resource.errMsgCustomerAPIProcessing
                }); ;
                apiResponse.Status = Status.Failed;
                LogManager.Logger.WriteException("OnboardingDomain", "CreateCustomerFromApi", ex.Message, ex);
            }

            return apiResponse;
        }

        private async Task AddCompanyUsersAndAddresses(TPDCustomerViewModel viewModel, ApiResponseViewModel apiResponse, AuthenticationDomain authDomain, UserContext apiUserContext, CompanyAddress compAddress, List<BillingAddress> billingAddresses, string TempPassword, List<User> buyerUser)
        {
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    //add company and company users
                    var buyerCompany = new Company
                    {
                        Name = viewModel.CustomerCompanyName,
                        CompanyTypeId = (int)CompanyType.Buyer,
                        CompanySizeId = 1,
                        BusinessTenureId = 1,
                        FuelQuantityId = 1,
                        IsActive = true,
                        CreatedDate = DateTimeOffset.Now,
                        UpdatedDate = DateTimeOffset.Now
                    };

                    //Add user and default role as admin
                    NotificationDomain notificationDomain = new NotificationDomain(this);
                    if (buyerUser.Any())
                    {
                        foreach (var oUsers in buyerUser)
                        {
                            if (oUsers.Id == 0)
                            {
                                var role = viewModel.Users.Where(t => t.Email.ToLower() == oUsers.Email.ToLower()).FirstOrDefault().Role;
                                oUsers.MstRoles.Add(Context.DataContext.MstRoles.Single(t => t.Id == role));
                                Context.DataContext.Users.Add(oUsers);
                                await Context.CommitAsync();

                                string eventTypeId = string.Empty;
                                if (viewModel.SendInvitationLinkToUser)
                                {
                                    var userRoleIds = oUsers.MstRoles.Select(t => t.Id).ToList();
                                    var defaultEnabled = authDomain.GetDefaultEnabledNotifications(userRoleIds, buyerCompany.CompanyTypeId);
                                    if (defaultEnabled != null && defaultEnabled.Any())
                                    {
                                        eventTypeId = string.Join(",", defaultEnabled);
                                    }
                                }

                                var eventTypeIds = eventTypeId.TrimStart(',').Split(',').Distinct().ToList();
                                var sqlQuery = Context.DataContext.MstCompanyUserRoleXEventTypes.Where(t => t.NotificationType != (int)NotificationType.Nothing
                                                && t.RoleId == (int)UserRoles.Admin && t.CompanyTypeId == buyerCompany.CompanyTypeId)
                                                .Select(t => new { t.EventTypeId });

                                var notificationSettings = new List<UserXNotificationSetting>();
                                foreach (var item in sqlQuery)
                                {
                                    bool isEmail = false;
                                    if (eventTypeIds.Contains(item.EventTypeId.ToString()))
                                    {
                                        isEmail = true;
                                    }
                                    var setting = new AuthenticationDomain(this).GetNotificationSetting(oUsers.Id, item.EventTypeId, isEmail);
                                    notificationSettings.Add(setting);
                                }
                                oUsers.UserXNotificationSettings = notificationSettings.Distinct().ToList();
                            }

                            if (buyerCompany.Id == 0)
                            {
                                buyerCompany.CreatedBy = oUsers.Id;
                                buyerCompany.UpdatedBy = oUsers.Id;
                                Context.DataContext.Companies.Add(buyerCompany);
                                await Context.CommitAsync();

                                if (compAddress != null && compAddress.StateId > 0)
                                {
                                    buyerCompany.CompanyAddresses.Add(compAddress);
                                    Context.DataContext.Entry(buyerCompany).State = EntityState.Modified;
                                }

                                foreach (var item in billingAddresses)
                                {
                                    if (item != null && !string.IsNullOrWhiteSpace(item.Address))
                                    {
                                        buyerCompany.BillingAddresses.Add(item);
                                    }
                                }

                                if (buyerCompany.BillingAddresses.Any())
                                    buyerCompany.BillingAddresses.FirstOrDefault().IsDefault = true;

                                Context.DataContext.Entry(buyerCompany).State = EntityState.Modified;

                            }

                            if (viewModel.SendInvitationLinkToUser)
                            {
                                await notificationDomain.AddNotificationEventAsync(EventType.TPOUserInvitedForEULAAcceptance, oUsers.Id, apiUserContext.Id, null, TempPassword);
                                // send buyer onboard invitation email
                                var tpDomain = new ThirdPartyOrderDomain(this);
                                await tpDomain.SaveInvitationSendDetailsAsync(oUsers.Email, oUsers.Id, apiUserContext.Id, oUsers.FirstName, oUsers.LastName);
                                // update buyer user for onboarding
                                oUsers.IsEmailConfirmed = true;
                                oUsers.IsOnboardingComplete = true;
                                oUsers.IsActive = true;
                            }

                            if (oUsers.Company == null)
                            {
                                oUsers.Company = buyerCompany;
                            }
                        }

                        if (buyerCompany.Id > 0) // Company created successfully
                        {
                            AddCompnayXCreator(viewModel, apiUserContext, buyerCompany.Id);

                            UspCarrierCustomerMapping customerMapping = new UspCarrierCustomerMapping();
                            customerMapping.BuyerCompanyId = buyerCompany.Id;
                            customerMapping.CarrierAssignedCustomerId = viewModel.ExternalRefID;

                            var ordDomain = new OrderDomain(this);
                            var duplicateXrefCheckresult = ordDomain.CheckDuplicateCustomerId(customerMapping, apiUserContext);
                            if (duplicateXrefCheckresult.StatusCode == Status.Success)
                            {
                                var response = await ordDomain.SaveCarrierCustomerMapping(customerMapping, apiUserContext);
                            }
                            else
                            {
                                apiResponse.Messages.Add(new ApiCodeMessages()
                                {
                                    Code = Constants.ApiCodeRQ02,
                                    Message = duplicateXrefCheckresult.StatusMessage,
                                });
                                throw new Exception(duplicateXrefCheckresult.StatusMessage);
                            }

                            Context.Commit();
                            transaction.Commit();

                            apiResponse.Messages.Add(new ApiCodeMessages()
                            {
                                Code = Constants.ApiCodeRS01,
                                Message = Resource.SuccessMsgCustomerCompanyCreated,
                                EntityId = ApplicationConstants.CustomerNumberPrefix + buyerCompany.Id.ToString(ApplicationConstants.SevenDigit)
                            });
                            apiResponse.Status = Status.Success;
                        }

                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OnboardingDomain", "CreateCustomerFromApi", ex.Message, ex);

                }
            }
        }

        private void AddCompnayXCreator(TPDCustomerViewModel viewModel, UserContext apiUserContext, int buyerCompanyId)
        {
            //save external ref id
            CompanyXCreators entity = new CompanyXCreators();
            entity.CreatedByCompanyId = apiUserContext.CompanyId;
            entity.CompanyId = buyerCompanyId;
            if (!string.IsNullOrWhiteSpace(viewModel.ExternalRefID))
            {
                entity.ExternalRefId = viewModel.ExternalRefID.Trim();

            }
            entity.IsActive = true;
            entity.UpdatedBy = apiUserContext.Id;
            entity.UpdatedDate = DateTimeOffset.Now;
            Context.DataContext.CompanyXCreators.Add(entity);
        }

        private void ValidateAndAddBillingAddressesToList(TPDCustomerViewModel viewModel, UserContext apiUserContext, List<BillingAddress> billingAddresses, ApiResponseViewModel apiResponse, int customerCompanyId)
        {
            foreach (var item in viewModel.CompanyBillingAddresses)
            {
                if (string.IsNullOrWhiteSpace(item.CompanyBillingAddressName) && (!string.IsNullOrWhiteSpace(item.CompanyBillingAddressLine1)))
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = string.Format(Resource.errMsgParameterIsRequired, "Billing Address Name")
                    });
                    apiResponse.Status = Status.Failed;
                }

                if (!string.IsNullOrWhiteSpace(item.CompanyBillingAddressName) && (string.IsNullOrWhiteSpace(item.CompanyBillingAddressLine1)))
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = string.Format(Resource.errMsgParameterIsRequired, "Billing Address Line1")
                    });
                    apiResponse.Status = Status.Failed;
                }

                if (!string.IsNullOrWhiteSpace(item.CompanyBillingAddressName) && !string.IsNullOrWhiteSpace(item.CompanyBillingAddressLine1))
                {
                    var isNameExists = Context.DataContext.BillingAddresses.Any(t => t.IsActive && (t.CompanyId == customerCompanyId || customerCompanyId == 0) && t.Name.ToLower().Equals(item.CompanyBillingAddressName.ToLower()));
                    if (!isNameExists)
                    {
                        var billingAddress = new BillingAddress();
                        billingAddress.Name = string.IsNullOrWhiteSpace(item.CompanyBillingAddressName) ? null : item.CompanyBillingAddressName.Trim();
                        billingAddress.Address = string.IsNullOrWhiteSpace(item.CompanyBillingAddressLine1) ? null : item.CompanyBillingAddressLine1.Trim();
                        billingAddress.City = string.IsNullOrWhiteSpace(item.CompanyBillingAddressCity) ? null : item.CompanyBillingAddressCity.Trim();
                        billingAddress.County = string.IsNullOrWhiteSpace(item.CompanyBillingAddressCounty) ? null : item.CompanyBillingAddressCounty.Trim();
                        billingAddress.StateName = string.IsNullOrWhiteSpace(item.CompanyBillingAddressState) ? null : item.CompanyBillingAddressState.Trim();
                        billingAddress.ZipCode = string.IsNullOrWhiteSpace(item.CompanyBillingAddressZip) ? null : item.CompanyBillingAddressZip.Trim();
                        billingAddress.PhoneTypeId = 1;
                        billingAddress.CountryName = string.IsNullOrWhiteSpace(item.CompanyBillingAddressCountry) ? null : item.CompanyBillingAddressCountry.Trim();
                        var phoneNumber = (string.IsNullOrWhiteSpace(item.CompanyBillingAddressOfficeNumer) && string.IsNullOrWhiteSpace(item.CompanyBillingAddressMobileNumber)) ? null : (item.CompanyBillingAddressMobileNumber ?? item.CompanyBillingAddressOfficeNumer);
                        billingAddress.PhoneNumber = phoneNumber;
                        billingAddress.IsActive = true;
                        billingAddress.IsDefault = true;
                        billingAddress.UpdatedBy = apiUserContext.Id;
                        billingAddress.UpdatedDate = DateTimeOffset.Now;

                        billingAddresses.Add(billingAddress);
                    }
                    else if(customerCompanyId > 0)
                    {
                        //update billing address
                        var addressToUpdate = billingAddresses.FirstOrDefault(t => t.Name.ToLower() == item.CompanyBillingAddressName);
                        if(addressToUpdate != null)
                        {
                            addressToUpdate.Name = string.IsNullOrWhiteSpace(item.CompanyBillingAddressName) ? null : item.CompanyBillingAddressName.Trim();
                            addressToUpdate.Address = string.IsNullOrWhiteSpace(item.CompanyBillingAddressLine1) ? null : item.CompanyBillingAddressLine1.Trim();
                            addressToUpdate.City = string.IsNullOrWhiteSpace(item.CompanyBillingAddressCity) ? null : item.CompanyBillingAddressCity.Trim();
                            addressToUpdate.County = string.IsNullOrWhiteSpace(item.CompanyBillingAddressCounty) ? null : item.CompanyBillingAddressCounty.Trim();
                            addressToUpdate.StateName = string.IsNullOrWhiteSpace(item.CompanyBillingAddressState) ? null : item.CompanyBillingAddressState.Trim();
                            addressToUpdate.ZipCode = string.IsNullOrWhiteSpace(item.CompanyBillingAddressZip) ? null : item.CompanyBillingAddressZip.Trim();
                            addressToUpdate.PhoneTypeId = 1;
                            addressToUpdate.CountryName = string.IsNullOrWhiteSpace(item.CompanyBillingAddressCountry) ? null : item.CompanyBillingAddressCountry.Trim();
                            var phoneNumber = (string.IsNullOrWhiteSpace(item.CompanyBillingAddressOfficeNumer) && string.IsNullOrWhiteSpace(item.CompanyBillingAddressMobileNumber)) ? null : (item.CompanyBillingAddressMobileNumber ?? item.CompanyBillingAddressOfficeNumer);
                            addressToUpdate.PhoneNumber = phoneNumber;
                            addressToUpdate.IsActive = true;
                            addressToUpdate.IsDefault = true;
                            addressToUpdate.UpdatedBy = apiUserContext.Id;
                            addressToUpdate.UpdatedDate = DateTimeOffset.Now;
                        }
                    }
                    else
                    {
                        apiResponse.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRQ02,
                            Message = string.Format(Resource.errMsgEntityAlreadyExists, item.CompanyBillingAddressName)
                        });
                        apiResponse.Status = Status.Failed;
                    }
                }
            }

        }

        private void ValidateUsersList(ApiResponseViewModel response, TPDCustomerViewModel viewModel, int customerCompanyId = 0)
        {
            var helperDomain = new HelperDomain();
            var buyerUserRoles = new List<int>()
                {
                    (int)UserRoles.Admin,
                    (int)UserRoles.Buyer,
                    (int)UserRoles.OnsitePerson,
                    (int)UserRoles.ReportingPerson,
                    (int)UserRoles.AccountSpecialist
                };
            if (customerCompanyId <= 0)
            {
                if (viewModel.Users == null)
                {
                    response.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = "Atleast one User is required"
                    });
                    response.Status = Status.Failed;
                }
                else
                {
                    if (!viewModel.Users.Any(t => t.Role == (int)UserRoles.Admin))
                    {
                        response.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRQ02,
                            Message = "Atleast one User with Role=2 (Admin) is required"
                        });
                        response.Status = Status.Failed;
                    }
                    if (viewModel.Users.Any())
                    {
                        if (viewModel.Users.Count != viewModel.Users.Select(t => t.Email).Distinct().Count())
                        {
                            response.Messages.Add(new ApiCodeMessages()
                            {
                                Code = Constants.ApiCodeRQ02,
                                Message = "Same Email can not be used for multiple user(s)"
                            });
                            response.Status = Status.Failed;
                        }
                    }

                    foreach (var user in viewModel.Users)
                    {
                        if (!IsValidUserRole(user.Role, buyerUserRoles))
                        {
                            response.Messages.Add(new ApiCodeMessages()
                            {
                                Code = Constants.ApiCodeRQ02,
                                Message = "Invalid User Role - " + user.Role
                            });
                            response.Status = Status.Failed;
                        }

                        if (string.IsNullOrWhiteSpace(user.PhoneNumber))
                        {
                            response.Messages.Add(new ApiCodeMessages()
                            {
                                Code = Constants.ApiCodeRQ02,
                                Message = string.Format(Resource.errMsgParameterIsRequired, "User PhoneNumber")
                            });
                            response.Status = Status.Failed;
                        }
                        else
                        {
                            if (!helperDomain.IsMatchesRegex(ApplicationConstants.TenDigitNoHypenPhoneNumberRegex, user.PhoneNumber.Trim()).Result)
                            {
                                response.Messages.Add(new ApiCodeMessages()
                                {
                                    Code = Constants.ApiCodeRQ02,
                                    Message = "Invalid User PhoneNumber - " + user.PhoneNumber
                                }); ;
                                response.Status = Status.Failed;
                            }
                        }
                    }
                }
            }
            else if (customerCompanyId > 0)
            {
                if (viewModel.Users != null && viewModel.Users.Any())
                {
                    var companyUsers = Context.DataContext.Companies.Where(t => t.Id == customerCompanyId).FirstOrDefault().Users.ToList();

                    if (companyUsers != null && companyUsers.Any())
                    {
                        var emailIds = new List<string>();
                        companyUsers.ForEach(user => emailIds.Add(user.Email.Trim().ToLower()));

                        foreach (var user in viewModel.Users)
                        {
                            // implies new user added.
                            if (!emailIds.Contains(user.Email.Trim().ToLower()))
                            {
                                var existingUser = Context.DataContext.Users.FirstOrDefault(t => t.Email.Trim().ToLower().Equals(user.Email.Trim().ToLower().Trim()));
                                if (existingUser == null)
                                {
                                    if (string.IsNullOrWhiteSpace(user.FirstName))
                                    {
                                        response.Messages.Add(new ApiCodeMessages()
                                        {
                                            Code = Constants.ApiCodeRQ02,
                                            Message = string.Format(Resource.errMsgMissingRequiredParametersForUser, Resource.lblFirstName, user.Email)
                                        }); ;
                                        response.Status = Status.Failed;
                                    }
                                    if (string.IsNullOrWhiteSpace(user.LastName))
                                    {
                                        response.Messages.Add(new ApiCodeMessages()
                                        {
                                            Code = Constants.ApiCodeRQ02,
                                            Message = string.Format(Resource.errMsgMissingRequiredParametersForUser, Resource.lblLastName, user.Email)
                                        }); ;
                                        response.Status = Status.Failed;
                                    }
                                    if (string.IsNullOrWhiteSpace(user.PhoneNumber))
                                    {
                                        response.Messages.Add(new ApiCodeMessages()
                                        {
                                            Code = Constants.ApiCodeRQ02,
                                            Message = string.Format(Resource.errMsgMissingRequiredParametersForUser, Resource.lblPhoneNumber, user.Email)
                                        }); ;
                                        response.Status = Status.Failed;
                                    }
                                    if (!IsValidUserRole(user.Role, buyerUserRoles))
                                    {
                                        response.Messages.Add(new ApiCodeMessages()
                                        {
                                            Code = Constants.ApiCodeRQ02,
                                            Message = "Invalid User Role - " + user.Role
                                        });
                                        response.Status = Status.Failed;
                                    }
                                    if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
                                    {
                                        if (!helperDomain.IsMatchesRegex(ApplicationConstants.TenDigitNoHypenPhoneNumberRegex, user.PhoneNumber.Trim()).Result)
                                        {
                                            response.Messages.Add(new ApiCodeMessages()
                                            {
                                                Code = Constants.ApiCodeRQ02,
                                                Message = string.Format(Resource.valMessageInvalid, nameof(user.PhoneNumber))
                                            }); ;
                                            response.Status = Status.Failed;
                                        }
                                    }

                                }
                                else
                                {
                                    response.Messages.Add(new ApiCodeMessages()
                                    {
                                        Code = Constants.ApiCodeRQ02,
                                        Message = Resource.valMessageEmailAlreadyExists + user.Email
                                    });
                                    response.Status = Status.Failed;
                                }

                            }
                            else // validate phone number if given for existing company user
                            {
                                if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
                                {
                                    if (!helperDomain.IsMatchesRegex(ApplicationConstants.TenDigitNoHypenPhoneNumberRegex, user.PhoneNumber.Trim()).Result)
                                    {
                                        response.Messages.Add(new ApiCodeMessages()
                                        {
                                            Code = Constants.ApiCodeRQ02,
                                            Message = "Invalid PhoneNumber - " + user.PhoneNumber
                                        }); ;
                                        response.Status = Status.Failed;
                                    }
                                }
                            }
                        }
                    }
                }
            }




        }

        //from this method we can get address view model instead of creating/fetching lat-long again while processing
        private void ValidateAddress(ApiResponseViewModel response, TPDCustomerViewModel viewModel, ref CompanyAddress entity, UserContext apiUserContext)
        {
            if (!string.IsNullOrWhiteSpace(viewModel.CompanyAddressLine1))
            {
                var state = Context.DataContext.MstStates.Where(t => t.Code.ToLower() == viewModel.CompanyAddressState.ToLower()
                           || t.Name.ToLower() == viewModel.CompanyAddressState.ToLower()).Select(t => new { t.Id, t.Code }).FirstOrDefault();
                var countryCode = Context.DataContext.MstCountries.Where(t => t.Code.ToLower() == viewModel.CompanyAddressCountry.ToLower()
                                || t.Name.ToLower() == viewModel.CompanyAddressCountry.ToLower()
                                || t.IsoCode.ToLower() == viewModel.CompanyAddressCountry.ToLower())
                    .Select(t => new { t.Id, t.Code }).FirstOrDefault();

                Geocode point = null;

                if (state == null || countryCode == null)
                {
                    response.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = Resource.errMsgInvalidCombinationOfAddress
                    });
                    response.Status = Status.Failed;
                }
                else
                {
                    point = GoogleApiDomain.GetGeocode($"{viewModel.CompanyAddressLine1}, {viewModel.CompanyAddressCity}, {state.Code}, {countryCode.Code}, {viewModel.CompanyAddressZip}");
                    
                    if (string.IsNullOrWhiteSpace(viewModel.CompanyAddressCounty))
                    {
                        viewModel.CompanyAddressCounty = point.CountyName != null ? point.CountyName : viewModel.CompanyAddressCity;
                    }
                    if (point == null)
                        response.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRQ02,
                            Message = Resource.errMsgInvalidCombinationOfAddress
                        });
                        response.Status = Status.Failed;
                }

                if (!response.Messages.Any())
                {

                    if (entity == null)
                        entity = new CompanyAddress();
                    if (state != null)
                    {
                        entity.StateId = state.Id;
                    }
                    entity.Address = viewModel.CompanyAddressLine1;
                    entity.City = viewModel.CompanyAddressCity;
                    if (countryCode != null)
                    {
                        entity.CountryId = countryCode.Id;
                    }
                    entity.ZipCode = viewModel.CompanyAddressZip;
                    if (point != null)
                    {
                        entity.Latitude = (decimal)point.Latitude;
                        entity.Longitude = (decimal)point.Longitude;
                    }
                    entity.PhoneTypeId = string.IsNullOrWhiteSpace(viewModel.CompanyAddressOfficeNumber) ? 1 : 2;
                    entity.PhoneNumber = string.IsNullOrWhiteSpace(viewModel.CompanyAddressOfficeNumber) ? viewModel.CompanyAddressMobileNumber : viewModel.CompanyAddressOfficeNumber;
                    entity.IsDefault = true;
                    entity.IsActive = true;
                    entity.UpdatedBy = apiUserContext.Id;
                    entity.UpdatedDate = DateTimeOffset.Now;

                    if (countryCode.Id == (int)Country.CAR)
                    {
                        if (string.IsNullOrWhiteSpace(viewModel.CompanyAddressLine1))
                            entity.Address = viewModel.CompanyAddressLine1 ?? Resource.lblCaribbean;
                        if (string.IsNullOrWhiteSpace(viewModel.CompanyAddressCity))
                            entity.City = viewModel.CompanyAddressCity ?? Resource.lblCaribbean;
                        if (string.IsNullOrWhiteSpace(viewModel.CompanyAddressZip))
                            entity.ZipCode = viewModel.CompanyAddressZip ?? Resource.lblCaribbean;
                        //if (string.IsNullOrWhiteSpace(viewModel.CompanyAddressCounty))
                    }
                }
            }
        }

        private async Task ValidateCompanyName(ApiResponseViewModel response, TPDCustomerViewModel viewModel,int customerCompanyId =0)
        {
            //update case
            if (customerCompanyId > 0)
            {
                if (!string.IsNullOrWhiteSpace(viewModel.CustomerCompanyName))
                {
                    var isNameExists = await Context.DataContext.Companies.AnyAsync(t => t.Name.ToLower() == viewModel.CustomerCompanyName.ToLower() && t.Id != customerCompanyId);
                    if (isNameExists)
                    {
                        response.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRQ02,
                            Message = string.Format(Resource.errCompanyExistsOnExchange, viewModel.CustomerCompanyName)
                        });
                        response.Status = Status.Failed;
                    }

                }
            }
            else // create case
            {
                if (!string.IsNullOrWhiteSpace(viewModel.CustomerCompanyName))
                {
                    var isNameExists = await Context.DataContext.Companies.AnyAsync(t => t.Name.ToLower() == viewModel.CustomerCompanyName.ToLower());
                    if (isNameExists)
                    {
                        response.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRQ02,
                            Message = string.Format(Resource.errCompanyExistsOnExchange, viewModel.CustomerCompanyName)
                        });
                        response.Status = Status.Failed;
                    }

                }
            }
            

        }

        private void ValidateContactNumber(ApiResponseViewModel response, TPDCustomerViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.CompanyAddressMobileNumber) && string.IsNullOrWhiteSpace(viewModel.CompanyAddressOfficeNumber))
            {
                response.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRQ02,
                    Message = Resource.errMessageOfficeMobileNumberRequired
                }); ;
                response.Status = Status.Failed;
            }
            if (!string.IsNullOrWhiteSpace(viewModel.CompanyAddressMobileNumber))
            {
                var helperDomain = new HelperDomain();
                if (!helperDomain.IsMatchesRegex(ApplicationConstants.TenDigitNoHypenPhoneNumberRegex, viewModel.CompanyAddressMobileNumber.Trim()).Result)
                {
                    response.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = "Invalid Company Address Mobile Number - " + viewModel.CompanyAddressMobileNumber
                    }); ; ;
                    response.Status = Status.Failed;
                }
            }
            if (viewModel.CompanyBillingAddresses.Any())
            {
                foreach (var item in viewModel.CompanyBillingAddresses)
                {
                    if (!string.IsNullOrWhiteSpace(item.CompanyBillingAddressMobileNumber))
                    {
                        var helperDomain = new HelperDomain();
                        if (!helperDomain.IsMatchesRegex(ApplicationConstants.TenDigitNoHypenPhoneNumberRegex, item.CompanyBillingAddressMobileNumber.Trim()).Result)
                        {
                            response.Messages.Add(new ApiCodeMessages()
                            {
                                Code = Constants.ApiCodeRQ02,
                                Message = "Invalid Company Billing Address Mobile Number - " + item.CompanyBillingAddressMobileNumber
                            }); ; ;

                            response.Status = Status.Failed;
                        }
                    }
                }
            }
        }

        public async Task<ApiResponseViewModel> UpdateCustomerFromApi(TPDCustomerViewModel viewModel, string token)
        {
            ApiResponseViewModel apiResponse = new ApiResponseViewModel();
            try
            {
                //get userdetails from token
                var authDomain = new AuthenticationDomain(this);
                var apiUserContext = await authDomain.GetUserContextFromTokenAsync(token);
                if (apiUserContext != null)
                {
                    CompanyAddress compAddress = null;

                    //validate required parameters
                    ValidateRequiredParameters(viewModel, apiResponse, apiUserContext);
                    if (!apiResponse.Messages.Any())
                    {

                        ValidateTFXCompanyIdAndExternalRefId(viewModel, apiResponse, apiUserContext);
                        await ValidateCompanyName(apiResponse, viewModel);
                        ValidateAddress(apiResponse, viewModel, ref compAddress, apiUserContext);

                        if (!apiResponse.Messages.Any())
                        {
                            var customerCompanyId = await GetApiCustomerCompanyIdByRefIdOrTfxCompanyId(viewModel.TFXCompanyId, viewModel.ExternalRefID, apiUserContext, apiResponse);
                            if (customerCompanyId > 0)
                            {
                                ValidateUsersList(apiResponse, viewModel, customerCompanyId);
                                //await UpdateCustomerCompanyDetails(apiResponse, viewModel, customerCompanyId, apiUserContext);

                            }
                            else
                            {
                                apiResponse.Messages.Add(new ApiCodeMessages()
                                {
                                    Code = Constants.ApiCodeRQ02,
                                    Message = Resource.errMsgInvalidExternalRefAndTfxCompanyId
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                apiResponse.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeEV01,
                    Message = Resource.errMsgCustomerAPIProcessing
                }); ;
                LogManager.Logger.WriteException("OnboardingDomain", "UpdateCustomerFromApi", ex.Message, ex);
            }
            return apiResponse;
        }

        private void ValidateRequiredParameters(TPDCustomerViewModel viewModel, ApiResponseViewModel apiResponse, UserContext userContext)
        {
            // TFXCompanyId or external ref id is required
            if (string.IsNullOrWhiteSpace(viewModel.TFXCompanyId) && string.IsNullOrWhiteSpace(viewModel.ExternalRefID))
            {
                apiResponse.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRQ02,
                    Message = Resource.errMsgTfxCompanyIdOrExternalRefIdRequired
                }); ;
                apiResponse.Status = Status.Failed;
            }

            // email is required for each user
            if (viewModel.Users != null && viewModel.Users.Any())
            {
                foreach (var user in viewModel.Users)
                {
                    if (string.IsNullOrWhiteSpace(user.Email))
                    {
                        apiResponse.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRQ02,
                            Message = Resource.valMessageEmailIsRequired
                        }); ;
                        apiResponse.Status = Status.Failed;

                    }
                }
            }
            //if any company address line present then all address fields are required
            if (!string.IsNullOrWhiteSpace(viewModel.CompanyAddressLine1) || !string.IsNullOrWhiteSpace(viewModel.CompanyAddressCity) || !string.IsNullOrWhiteSpace(viewModel.CompanyAddressCounty)
                || !string.IsNullOrWhiteSpace(viewModel.CompanyAddressCountry) || !string.IsNullOrWhiteSpace(viewModel.CompanyAddressState) || !string.IsNullOrWhiteSpace(viewModel.CompanyAddressZip))
            {
                if (string.IsNullOrWhiteSpace(viewModel.CompanyAddressLine1))
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = string.Format(Resource.valMessageRequired, nameof(viewModel.CompanyAddressLine1))
                    }); ;
                    apiResponse.Status = Status.Failed;
                }
                if (string.IsNullOrWhiteSpace(viewModel.CompanyAddressCity))
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = string.Format(Resource.valMessageRequired, nameof(viewModel.CompanyAddressCity))
                    }); ;
                    apiResponse.Status = Status.Failed;
                }
                //if (string.IsNullOrWhiteSpace(viewModel.CompanyAddressCounty))
                //{
                //    apiResponse.Messages.Add(new ApiCodeMessages()
                //    {
                //        Code = Constants.ApiCodeRQ02,
                //        Message = string.Format(Resource.valMessageRequired, nameof(viewModel.CompanyAddressCounty))
                //    }); ;
                //    apiResponse.Status = Status.Failed;
                //}
                if (string.IsNullOrWhiteSpace(viewModel.CompanyAddressCountry))
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = string.Format(Resource.valMessageRequired, nameof(viewModel.CompanyAddressCountry))
                    }); ;
                    apiResponse.Status = Status.Failed;
                }
                if (string.IsNullOrWhiteSpace(viewModel.CompanyAddressState))
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = string.Format(Resource.valMessageRequired, nameof(viewModel.CompanyAddressState))
                    }); ;
                    apiResponse.Status = Status.Failed;
                }
                if (string.IsNullOrWhiteSpace(viewModel.CompanyAddressZip))
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = string.Format(Resource.valMessageRequired, nameof(viewModel.CompanyAddressZip))
                    }); ;
                    apiResponse.Status = Status.Failed;
                }
            }

           
        }

        private void ValidateTFXCompanyIdAndExternalRefId(TPDCustomerViewModel viewModel, ApiResponseViewModel apiResponse, UserContext userContext)
        {

            int buyerCompanyIdByCustomerId = 0;
            int buyerCompanyIdByRefId = 0;
            if (!string.IsNullOrWhiteSpace(viewModel.TFXCompanyId))
            {
                if (viewModel.TFXCompanyId.Trim().StartsWith("TFCU"))
                {

                    var buyerCompanyId = viewModel.TFXCompanyId.Replace("TFCU", "").TrimStart('0');
                    if (int.TryParse(buyerCompanyId, out int tfxcompanyId))
                    {
                        var company = Context.DataContext.Companies.Where(t => t.Id == tfxcompanyId).FirstOrDefault();
                        if (company != null)
                        {
                            buyerCompanyIdByCustomerId = tfxcompanyId;
                        }
                        else
                        {
                            apiResponse.Messages.Add(new ApiCodeMessages()
                            {
                                Code = Constants.ApiCodeRQ02,
                                Message = Resource.errMsgInvalidTFxCompanyId
                            }); ;
                        }
                    }
                    else
                    {
                        apiResponse.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRQ02,
                            Message = Resource.errMsgInvalidTFxCompanyId
                        }); ;
                    }
                }
                else
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = Resource.errMsgInvalidTFxCompanyId
                    }); ;
                }
            }

            if (!string.IsNullOrWhiteSpace(viewModel.ExternalRefID))
            {
                var buyerCompanyCreator = Context.DataContext.CompanyXCreators.Where(t => t.CreatedByCompanyId == userContext.CompanyId && t.ExternalRefId != null && t.ExternalRefId.Trim().ToLower() == viewModel.ExternalRefID.Trim().ToLower() && t.IsActive).FirstOrDefault();
                if (buyerCompanyCreator != null)
                {
                    if (buyerCompanyCreator.CompanyId > 0)
                    {
                        buyerCompanyIdByRefId = buyerCompanyCreator.CompanyId;
                    }
                    else
                    {
                        apiResponse.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRQ02,
                            Message = Resource.errMsgInvalidExternalRefId
                        }); ;
                    }
                }
                else
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = Resource.errMsgInvalidExternalRefId
                    }); ;
                }

            }

            if (!string.IsNullOrWhiteSpace(viewModel.ExternalRefID) && !string.IsNullOrWhiteSpace(viewModel.TFXCompanyId))
            {
                if (buyerCompanyIdByCustomerId != buyerCompanyIdByRefId)
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = Resource.errMsgInvalidExternalRefAndTfxCompanyId
                    }); ;
                }
            }
        }

        public async Task<int> GetApiCustomerCompanyIdByRefIdOrTfxCompanyId(string tfxCompanyId, string externalRefId, UserContext userContext, ApiResponseViewModel apiResponse)
        {
            int response = 0;

            if (!string.IsNullOrWhiteSpace(tfxCompanyId))
            {

                var buyerCompanyId = tfxCompanyId.Replace("TFCU", "").TrimStart('0');
                if (int.TryParse(buyerCompanyId, out int tfxcompanyId))
                {
                    var company = await Context.DataContext.Companies.Where(t => t.Id == tfxcompanyId).FirstOrDefaultAsync();
                    if (company != null)
                    {
                        response = tfxcompanyId;
                        return response;
                    }

                    if (tfxcompanyId == 0)
                    {
                        apiResponse.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRQ02,
                            Message = Resource.errMsgInvalidExternalRefAndTfxCompanyId
                        }); ;
                        apiResponse.Status = Status.Failed;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(externalRefId))
            {
                var companyCreator = await Context.DataContext.CompanyXCreators.Where(t => t.ExternalRefId != null && t.ExternalRefId.Trim().ToLower() == externalRefId.Trim().ToLower() && t.IsActive && t.CreatedByCompanyId == userContext.CompanyId).FirstOrDefaultAsync();
                if (companyCreator != null)
                {
                    response = companyCreator.CompanyId;
                    return response;
                }
            }
            return response;
        }

        private bool IsValidUserRole(int roleId, List<int> compatibleUserRoles)
        {
            return compatibleUserRoles.Contains(roleId);
        }

        private async Task UpdateCustomerCompanyDetails(ApiResponseViewModel response, TPDCustomerViewModel viewModel, int customerCompanyId, UserContext userContext, List<BillingAddress> existingBillingAddress)
        {
            if (!response.Messages.Any())
            {
                Company company = await Context.DataContext.Companies.Where(t => t.Id == customerCompanyId).FirstOrDefaultAsync();
                if (company != null)
                {
                    company.Name = string.IsNullOrWhiteSpace(viewModel.CustomerCompanyName) ? company.Name : viewModel.CustomerCompanyName.Trim();
                    CompanyAddress companyAddress = company.CompanyAddresses.Where(t => t.IsActive && t.IsDefault).FirstOrDefault();
                    List<User> companyUsers = company.Users.ToList();

                    List<User> newUsers = new List<User>();

                    company.BillingAddresses = existingBillingAddress;

                    //update company address
                    if (!string.IsNullOrWhiteSpace(viewModel.CompanyAddressLine1))
                    {
                        var state = Context.DataContext.MstStates.First(t => t.Code.ToLower() == viewModel.CompanyAddressState.ToLower()
                          || t.Name.ToLower() == viewModel.CompanyAddressState.ToLower());
                        var countryCode = Context.DataContext.MstCountries.First(t => t.Code.ToLower() == viewModel.CompanyAddressCountry.ToLower()
                                        || t.Name.ToLower() == viewModel.CompanyAddressCountry.ToLower());

                        Geocode point = null;
                        point = GoogleApiDomain.GetGeocode($"{viewModel.CompanyAddressLine1} {viewModel.CompanyAddressCity} {state.Code} {countryCode.Code} {viewModel.CompanyAddressZip}");

                        companyAddress.Address = string.IsNullOrWhiteSpace(viewModel.CompanyAddressLine1) ? companyAddress.Address : viewModel.CompanyAddressLine1.Trim();
                        companyAddress.City = string.IsNullOrWhiteSpace(viewModel.CompanyAddressCity) ? companyAddress.City : viewModel.CompanyAddressCity.Trim();
                        companyAddress.StateId = state.Id;
                        companyAddress.CountryId = countryCode.Id;
                        companyAddress.ZipCode = string.IsNullOrWhiteSpace(viewModel.CompanyAddressZip) ? companyAddress.ZipCode : viewModel.CompanyAddressZip.Trim();
                        companyAddress.Latitude = (point != null && point.Latitude > 0) ? (decimal)point.Latitude : companyAddress.Latitude;
                        companyAddress.Longitude = (point != null && point.Longitude > 0) ? (decimal)point.Longitude : companyAddress.Longitude;

                        companyAddress.PhoneNumber = string.IsNullOrWhiteSpace(viewModel.CompanyAddressOfficeNumber) ? (viewModel.CompanyAddressMobileNumber ?? companyAddress.PhoneNumber) : (viewModel.CompanyAddressOfficeNumber ?? companyAddress.PhoneNumber);
                        companyAddress.UpdatedBy = userContext.Id;
                        companyAddress.UpdatedDate = DateTimeOffset.Now;

                        if (string.IsNullOrWhiteSpace(viewModel.CompanyAddressCounty))
                        {
                            viewModel.CompanyAddressCounty = point !=null && point.CountyName != null ? point.CountyName : viewModel.CompanyAddressCity;
                        }
                    }


                    int LengthOfPassword = Constants.LengthOfPassword;
                    var RandomPassword = CryptoHelperMethods.GenerateRandomPassword(LengthOfPassword);

                    // update/add user
                    if (viewModel.Users != null && viewModel.Users.Any())
                    {
                        var emailIds = new List<string>();

                        companyUsers.ForEach(user => emailIds.Add(user.Email.Trim().ToLower()));

                        foreach (var apiUser in viewModel.Users)
                        {
                            // implies new user added.
                            if (!emailIds.Contains(apiUser.Email.Trim().ToLower()))
                            {
                                var salt = CryptoHelperMethods.GenerateSalt();
                                User user = new User
                                {
                                    FirstName = apiUser.FirstName,
                                    LastName = apiUser.LastName,
                                    UserName = apiUser.Email.Trim().ToLower(),
                                    Email = apiUser.Email.Trim().ToLower(),
                                    IsEmailConfirmed = false,
                                    PhoneNumber = apiUser.PhoneNumber,
                                    IsPhoneNumberConfirmed = ContextFactory.Current.GetDomain<NotificationDomain>().IsPhoneNumberValid(apiUser.PhoneNumber),
                                    IsTwoFactorEnabled = false,
                                    AccessFailedCount = 0,
                                    IsLockoutEnabled = true,
                                    LockoutEndDateUtc = null,
                                    PasswordHash = CryptoHelperMethods.GenerateHash(RandomPassword, salt),
                                    SecurityStamp = salt,
                                    FingerPrint = CryptoHelperMethods.GenerateHash(apiUser.Email, CryptoHelperMethods.GenerateSalt()),
                                    IsOnboardingComplete = false,
                                    IsActive = true,
                                    CreatedBy = userContext.Id,
                                    CreatedDate = DateTimeOffset.Now,
                                    UpdatedBy = userContext.Id,
                                    UpdatedDate = DateTimeOffset.Now,
                                    OnboardedTypeId = (int)OnboardedType.ThirdPartyOrderOnboarded
                                };
                                user.MstRoles.Add(Context.DataContext.MstRoles.Single(t => t.Id == apiUser.Role));
                                newUsers.Add(user);
                            }
                            else
                            {
                                var user = company.Users.Where(t => t.Email.Trim().ToLower().Equals(apiUser.Email.Trim().ToLower())).FirstOrDefault();
                                if (user != null)
                                {
                                    user.FirstName = string.IsNullOrWhiteSpace(apiUser.FirstName) ? user.FirstName : apiUser.FirstName.Trim();
                                    user.LastName = string.IsNullOrWhiteSpace(apiUser.LastName) ? user.LastName : apiUser.LastName.Trim();
                                    user.Email = string.IsNullOrWhiteSpace(apiUser.Email) ? user.Email : apiUser.Email.Trim();
                                    user.PhoneNumber = string.IsNullOrWhiteSpace(apiUser.PhoneNumber) ? user.PhoneNumber : apiUser.PhoneNumber.Trim();
                                    // not updating user roles for from api only company admin can update user rol
                                    //user.MstRoles.FirstOrDefault().Id = apiUser.Role;
                                    user.UpdatedBy = userContext.Id;
                                    user.UpdatedDate = DateTimeOffset.Now;
                                }
                            }
                        }
                    }

                    if (newUsers != null && newUsers.Any())
                    {
                        newUsers.ForEach(user => user.Company = company);
                        Context.DataContext.Users.AddRange(newUsers);

                    }

                    //AddCompnayXCreator(viewModel, userContext, customerCompanyId);

                    Context.DataContext.Entry(company).State = EntityState.Modified;
                    await Context.CommitAsync();

                    
                    response.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRS01,
                        Message = Resource.SuccessMsgCustomerCompanyUpdate,
                        EntityId = ApplicationConstants.CustomerNumberPrefix + customerCompanyId.ToString(ApplicationConstants.SevenDigit)
                    }); ;
                    response.Status = Status.Success;
                }
            }
        }

        private async Task ValidateExternalRefId(ApiResponseViewModel apiResponse, TPDCustomerViewModel viewModel, UserContext apiUserContext)
        {
            if (string.IsNullOrWhiteSpace(viewModel.ExternalRefID))
            {
                apiResponse.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRQ02,
                    Message = Resource.errMsgTfxCompanyIdOrExternalRefIdRequired
                });
            }

            if (!string.IsNullOrWhiteSpace(viewModel.ExternalRefID))
            {
                var existingRefId = await Context.DataContext.CompanyXCreators
                                 .Where(t => t.CreatedByCompanyId == apiUserContext.CompanyId
                                 && t.ExternalRefId != null && t.ExternalRefId.Trim().ToLower() == viewModel.ExternalRefID.Trim().ToLower() && t.IsActive)
                                 .FirstOrDefaultAsync();
                if (existingRefId != null)
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = "ExternalRefId already exists - " + viewModel.ExternalRefID
                    }); ; ;
                }
            }
        }
        #endregion
    }
}
