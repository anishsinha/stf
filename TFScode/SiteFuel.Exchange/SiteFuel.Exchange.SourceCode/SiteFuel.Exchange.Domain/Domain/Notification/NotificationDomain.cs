using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.SmsManager;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.DispatchScheduler;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using SiteFuel.Exchange.ViewModels.Notification;
using SiteFuel.Exchange.ViewModels.Quickbooks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class NotificationDomain : BaseDomain
    {
        private HelperDomain _helperDomain;


        public NotificationDomain() : base(ContextFactory.Current.ConnectionString)
        {
            _helperDomain = new HelperDomain(this);
        }

        public NotificationDomain(BaseDomain domain) : base(domain)
        {
            _helperDomain = new HelperDomain(this);
        }

        public NotificationSubscriptionViewModel IsUserSubscribedForEvent(string toEmailId, int? eventTypeId)
        {
            NotificationSubscriptionViewModel viewModel = new NotificationSubscriptionViewModel();
            var userxnotification = Context.DataContext.UserXNotificationSettings
                                    .Where(t => t.User.Email == toEmailId && t.EventTypeId == (eventTypeId ?? 0)
                                    && t.User.Company.AccountTypeId != (int)AccountType.SfxOwned)
                                    .Select(t => new
                                    {
                                        t.EventTypeId,
                                        t.IsEmail,
                                        t.IsSMS,
                                        t.User.PhoneNumber,
                                        t.MstEventType.Name,
                                        t.User.IsPhoneNumberConfirmed
                                    }).FirstOrDefault();
            if (userxnotification != null)
            {
                viewModel.IsEmailSubscribed = userxnotification.IsEmail;
                viewModel.IsSmsSubscribed = userxnotification.IsSMS;
                viewModel.ToPhoneNumber = userxnotification.PhoneNumber;
                viewModel.EventName = userxnotification.Name;
                viewModel.IsPhoneNumberConfirmed = userxnotification.IsPhoneNumberConfirmed;
            }
            return viewModel;
        }

        public NotificationSubscriptionViewModel GetUserDetailsForSms(string toEmailId)
        {
            NotificationSubscriptionViewModel viewModel = new NotificationSubscriptionViewModel();
            var userxnotification = Context.DataContext.UserXNotificationSettings
                                    .Where(t => t.User.Email == toEmailId && t.User.Company.AccountTypeId != (int)AccountType.SfxOwned)
                                    .Select(t => new
                                    {
                                        t.User.PhoneNumber,
                                        t.User.IsPhoneNumberConfirmed
                                    }).FirstOrDefault();
            if (userxnotification != null)
            {
                viewModel.ToPhoneNumber = userxnotification.PhoneNumber;
                viewModel.IsPhoneNumberConfirmed = userxnotification.IsPhoneNumberConfirmed;
            }
            return viewModel;
        }

        public NotificationViewModel GetEmailNotificationContent(EventType eventType, CompanyType companyTypeId, string serverUrl, string callbackUrl)
        {
            var response = new NotificationViewModel();
            try
            {
                response.CompanyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.email_HeaderLogo);
                response.CompanyText = Resource.email_HeaderText;
                response.BodyButtonUrl = string.Empty;
                response.ServerUrl = serverUrl;
                response.EventTypeId = (int)eventType;
                response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.BodyLogo_HandShake);
                if (!string.IsNullOrWhiteSpace(callbackUrl))
                {
                    response.BodyButtonUrl = _helperDomain.GetAbsoluteServerUrl(serverUrl, callbackUrl);
                }

                var template = Context.DataContext.MstCompanyUserRoleXEventTypes
                                .Where(t => t.EventTypeId == (int)eventType && t.CompanyTypeId == (int)companyTypeId && t.TemplateId != null)
                                .Select(t => t.Template).FirstOrDefault();
                if (template != null)
                {
                    response.Subject = template.Subject;
                    response.BodyText = template.Body;
                    response.BodyButtonText = template.ButtonText;
                    response.SmsText = template.SmsText;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetEmailNotificationContent", "Exception Details : ", ex);
            }
            return response;
        }

        public ApplicationTemplateViewModel GetApplicationTemplate(string supplierURL)
        {
            var helperDomain = new HelperDomain(this);
            var response = new ApplicationTemplateViewModel();
            bool isTemplateAvailable = false;

            if (!string.IsNullOrEmpty(supplierURL))
            {
                var applicationTemplate = Context.DataContext.MstApplicationTemplates.FirstOrDefault(t => t.URLName == supplierURL && t.IsActive);
                if (applicationTemplate != null)
                {
                    isTemplateAvailable = true;
                    var serverUrl = helperDomain.GetServerUrl();
                    response.Template = Convert.ToString(applicationTemplate.Template);
                    response.URLName = applicationTemplate.URLName;
                    response.BrandedCompanyName = applicationTemplate.BrandedCompanyName;
                    response.FromEmail = applicationTemplate.FromEmail;
                    response.CompanyLogo = helperDomain.GetAbsoluteServerUrl(serverUrl, applicationTemplate.CompanyLogo);
                    response.ApplicationTemplateId = applicationTemplate.Id;
                    response.SenderName = applicationTemplate.SenderName;
                }
            }

            if (!isTemplateAvailable)
            {
                response = SetDefaultTemplateDetails();
            }

            return response;
        }

        private ApplicationTemplateViewModel SetDefaultTemplateDetails()
        {
            var response = new ApplicationTemplateViewModel();
            var helperDomain = new HelperDomain(this);
            var serverUrl = helperDomain.GetServerUrl();
            response.Template = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingApplicationEventNotificationTemplate);
            response.BrandedCompanyName = Resource.lblTrueFill;
            response.ApplicationTemplateId = (int)ApplicationTemplate.TrueFill;
            response.CompanyLogo = helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.email_HeaderLogo);
            return response;
        }

        public ApplicationTemplateViewModel GetApplicationTemplate(int applicationTemplateId)
        {
            var helperDomain = new HelperDomain(this);
            var response = new ApplicationTemplateViewModel();
            var applicationTemplate = Context.DataContext.MstApplicationTemplates.FirstOrDefault(t => t.Id == applicationTemplateId);
            if (applicationTemplate != null)
            {
                var serverUrl = helperDomain.GetServerUrl();
                response.Template = Convert.ToString(applicationTemplate.Template);
                response.URLName = applicationTemplate.URLName;
                response.BrandedCompanyName = applicationTemplate.BrandedCompanyName;
                response.FromEmail = applicationTemplate.FromEmail;
                response.CompanyLogo = helperDomain.GetAbsoluteServerUrl(serverUrl, applicationTemplate.CompanyLogo);
                response.ApplicationTemplateId = applicationTemplate.Id;
                response.SenderName = applicationTemplate.SenderName;
            }
            else
            {
                response = SetDefaultTemplateDetails();
            }
            return response;
        }

        public NotificationViewModel GetNotificationTemplateDetails(string supplierURL, EventSubType eventSubTypeId)
        {
            var response = new NotificationViewModel();
            if (eventSubTypeId == EventSubType.CarrierDeliveries)
            {
                response.ApplicationTemplateId = 4;
            }
            else
            {
                response.ApplicationTemplateId = (int)ApplicationTemplate.TrueFill;
            }
            if (!string.IsNullOrEmpty(supplierURL))
            {
                var applicationTemplate = Context.DataContext.MstApplicationTemplates.FirstOrDefault(t => t.URLName == supplierURL && t.IsActive);
                if (applicationTemplate != null)
                {
                    response.ApplicationTemplateId = applicationTemplate.Id;
                }
            }

            var emailTemplateMapping = Context.DataContext.NotificationTemplateMappings.FirstOrDefault(t => t.ApplicationTemplateId == response.ApplicationTemplateId && t.EventSubTypeId == eventSubTypeId);
            var emailTemplate = emailTemplateMapping.MstNotificationTemplate;
            if (emailTemplate != null)
            {
                response.Subject = emailTemplate.Subject;
                response.BodyText = emailTemplate.Body;
                response.BodyButtonText = emailTemplate.ButtonText;
                response.BodyLogo = emailTemplate.BodyLogo;
                response.CompanyText = emailTemplate.CompanyText;
                response.SmsText = emailTemplate.SmsText;
            }
            return response;
        }
        public NotificationViewModel GetNotificationContent(EventSubType type, string serverUrl, string callbackUrl, int? eventTypeId = null, string supplierURL = "")
        {
            var response = new NotificationViewModel();
            try
            {
                response.CompanyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.email_HeaderLogo);
                response.CompanyText = Resource.email_HeaderText;
                response.BodyButtonUrl = string.Empty;
                response.ServerUrl = serverUrl;
                response.EventTypeId = eventTypeId;

                if (!string.IsNullOrWhiteSpace(callbackUrl))
                {
                    response.BodyButtonUrl = _helperDomain.GetAbsoluteServerUrl(serverUrl, callbackUrl);
                }

                switch (type)
                {
                    case EventSubType.ContactUs:
                        response.Subject = Resource.emailContactUs_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailContactUs_BodyLogo);
                        response.BodyText = Resource.emailContactUs_BodyText;
                        response.BodyButtonText = Resource.emailContactUs_BodyButtonText;
                        break;

                    case EventSubType.EmailVerification:
                    case EventSubType.ForgotPassword:
                    case EventSubType.ExternalCompanyInviteOnboarded_Invitee:
                    case EventSubType.AdditionalUserAdded_InvitedUser:
                    case EventSubType.AdditionalUserAdded_CompanyAdmin:
                    case EventSubType.AdditionalUserOnboarded_InvitedUser:
                    case EventSubType.AdditionalUserOnboarded_CompanyAdmin:
                    case EventSubType.DeliveryClosedSendBDN:
                    case EventSubType.CarrierDeliveries:
                    case EventSubType.InvitedUserAdded_CompanyAdmin:
                    case EventSubType.InvitedUserAdded_InvitedUser:
                    case EventSubType.InvitedUserAdded_NewInvitedUser:
                        var notificationTemplate = GetNotificationTemplateDetails(supplierURL, type);
                        response.Subject = notificationTemplate.Subject;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, notificationTemplate.BodyLogo);
                        response.BodyText = notificationTemplate.BodyText;
                        response.BodyButtonText = notificationTemplate.BodyButtonText;
                        response.CompanyText = notificationTemplate.CompanyText;
                        response.ApplicationTemplateId = notificationTemplate.ApplicationTemplateId;
                        response.SmsText = notificationTemplate.SmsText;
                        break;


                    //case EventSubType.EmailVerification:
                    //    response.Subject = Resource.emailConfirmEmail_SubjectText;
                    //    response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailConfirmEmail_BodyLogo);
                    //    response.BodyText = Resource.emailConfirmEmail_BodyText;
                    //    response.BodyButtonText = Resource.emailConfirmEmail_BodyButtonText;
                    //    response.CompanyText = Resource.emailConfirmEmail_HeaderText;

                    //case EventSubType.ForgotPassword:
                    //    response.Subject = Resource.emailForgotPassword_SubjectText;
                    //    response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailForgotPassword_BodyLogo);
                    //    response.BodyText = Resource.emailForgotPassword_BodyText;
                    //    response.BodyButtonText = Resource.emailForgotPassword_BodyButtonText;
                    //    break;

                    //case EventSubType.AdditionalUserAdded_InvitedUser:
                    //    response.Subject = Resource.emailAdditionalUserAdded_InvitedUser_SubjectText;
                    //    response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailAdditionalUserAdded_InvitedUser_BodyLogo);
                    //    response.BodyText = Resource.emailAdditionalUserAdded_InvitedUser_BodyText;
                    //    response.BodyButtonText = Resource.emailAdditionalUserAdded_InvitedUser_BodyButtonText;
                    //    response.CompanyText = Resource.emailAdditionalUserAdded_InvitedUser_HeaderText;
                    //    break;

                    //case EventSubType.AdditionalUserAdded_CompanyAdmin:
                    //    response.Subject = Resource.emailAdditionalUserAdded_CompanyAdmin_SubjectText;
                    //    response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailAdditionalUserAdded_CompanyAdmin_BodyLogo);
                    //    response.BodyText = Resource.emailAdditionalUserAdded_CompanyAdmin_BodyText;
                    //    response.BodyButtonText = Resource.emailAdditionalUserAdded_CompanyAdmin_BodyButtonText;
                    //    break;

                    case EventSubType.AdditionalUserUpdated_InvitedUser:
                        response.Subject = Resource.emailAdditionalUserUpdated_InvitedUser_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailAdditionalUserUpdated_InvitedUser_BodyLogo);
                        response.BodyText = Resource.emailAdditionalUserUpdated_InvitedUser_BodyText;
                        response.BodyButtonText = Resource.emailAdditionalUserUpdated_InvitedUser_BodyButtonText;
                        break;

                    case EventSubType.AdditionalUserUpdated_CompanyAdmin:
                        response.Subject = Resource.emailAdditionalUserUpdated_CompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailAdditionalUserUpdated_CompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailAdditionalUserUpdated_CompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailAdditionalUserUpdated_CompanyAdmin_BodyButtonText;
                        break;

                    //case EventSubType.AdditionalUserOnboarded_InvitedUser:
                    //    response.Subject = Resource.emailAdditionalUserOnboarded_InvitedUser_SubjectText;
                    //    response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailAdditionalUserOnboarded_InvitedUser_BodyLogo);
                    //    response.BodyText = Resource.emailAdditionalUserOnboarded_InvitedUser_BodyText;
                    //    response.BodyButtonText = Resource.emailAdditionalUserOnboarded_InvitedUser_BodyButtonText;
                    //    response.CompanyText = Resource.emailAdditionalUserOnboarded_InvitedUser_HeaderText;
                    //    break;

                    //case EventSubType.AdditionalUserOnboarded_CompanyAdmin:
                    //    response.Subject = Resource.emailAdditionalUserOnboarded_CompanyAdmin_SubjectText;
                    //    response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailAdditionalUserOnboarded_CompanyAdmin_BodyLogo);
                    //    response.BodyText = Resource.emailAdditionalUserOnboarded_CompanyAdmin_BodyText;
                    //    response.BodyButtonText = Resource.emailAdditionalUserOnboarded_CompanyAdmin_BodyButtonText;
                    //    response.CompanyText = Resource.emailAdditionalUserOnboarded_CompanyAdmin_HeaderText;
                    //    break;

                    case EventSubType.UserRolesUpdated_OnboardedUser:
                        response.Subject = Resource.emailUserRolesUpdated_OnboardedUser_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailUserRolesUpdated_OnboardedUser_BodyLogo);
                        response.BodyText = Resource.emailUserRolesUpdated_OnboardedUser_BodyText;
                        response.BodyButtonText = Resource.emailUserRolesUpdated_OnboardedUser_BodyButtonText;
                        break;

                    case EventSubType.UserRolesUpdated_CompanyAdmin:
                        response.Subject = Resource.emailUserRolesUpdated_CompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailUserRolesUpdated_CompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailUserRolesUpdated_CompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailUserRolesUpdated_CompanyAdmin_BodyButtonText;
                        break;

                    case EventSubType.ExternalCompanyInvited:
                        response.Subject = Resource.emailExternalCompanyInvited_InvitedUser_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailExternalCompanyInvited_InvitedUser_BodyLogo);
                        response.BodyText = Resource.emailExternalCompanyInvited_InvitedUser_BodyText;
                        response.BodyButtonText = Resource.emailExternalCompanyInvited_InvitedUser_BodyButtonText;
                        break;

                    case EventSubType.TPOUserInvitedForEULAAcceptance:
                        response.Subject = Resource.emailTPOUserInvitedForEULAAcceptance_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailTPOUserInvitedForEULAAcceptance_BodyLogo);
                        response.BodyText = Resource.emailTPOUserInvitedForEULAAcceptance_BodyText;
                        response.BodyButtonText = Resource.emailExternalCompanyInvited_InvitedUser_BodyButtonText;
                        break;

                    case EventSubType.ExternalCompanyInviteUpdated:
                        response.Subject = Resource.emailExternalCompanyInviteUpdated_InvitedUser_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailExternalCompanyInviteUpdated_InvitedUser_BodyLogo);
                        response.BodyText = Resource.emailExternalCompanyInviteUpdated_InvitedUser_BodyText;
                        response.BodyButtonText = Resource.emailExternalCompanyInviteUpdated_InvitedUser_BodyButtonText;
                        break;

                    //case EventSubType.ExternalCompanyInviteOnboarded_Invitee:
                    //    response.Subject = Resource.emailExternalCompanyInviteOnboarded_Invitee_SubjectText;
                    //    response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailExternalCompanyInviteOnboarded_Invitee_BodyLogo);
                    //    response.BodyText = Resource.emailExternalCompanyInviteOnboarded_Invitee_BodyText;
                    //    response.BodyButtonText = Resource.emailExternalCompanyInviteOnboarded_Invitee_BodyButtonText;
                    //    break;

                    case EventSubType.ExternalCompanyInviteOnboarded_Inviter:
                        response.Subject = Resource.emailExternalCompanyInviteOnboarded_Inviter_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailExternalCompanyInviteOnboarded_Inviter_BodyLogo);
                        response.BodyText = Resource.emailExternalCompanyInviteOnboarded_Inviter_BodyText;
                        response.BodyButtonText = Resource.emailExternalCompanyInviteOnboarded_Inviter_BodyButtonText;
                        break;

                    case EventSubType.ExternalCompanyInviteOnboarded_SalesPeople:
                        response.Subject = Resource.emailExternalCompanyInviteOnboarded_SalesPeople_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailExternalCompanyInviteOnboarded_SalesPeople_BodyLogo);
                        response.BodyText = Resource.emailExternalCompanyInviteOnboarded_SalesPeople_BodyText;
                        response.BodyButtonText = Resource.emailExternalCompanyInviteOnboarded_SalesPeople_BodyButtonText;
                        break;

                    case EventSubType.JobAssignment:
                        response.Subject = Resource.emailJobAssignment_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailJobAssignment_BodyLogo);
                        response.BodyText = Resource.emailJobAssignment_BodyText;
                        response.BodyButtonText = Resource.emailJobAssignment_BodyButtonText;
                        response.CompanyText = Resource.emailJobAssignment_HeaderText;
                        break;

                    case EventSubType.InvoiceAndDropTicketApprovalWorkFlowDisabled_Approver:
                        response.Subject = Resource.emailInvoiceAndDropTicketApprovalWorkFlowDisabled_Approver_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceAndDropTicketApprovalWorkFlowDisabled_Approver_BodyLogo);
                        response.BodyText = Resource.emailInvoiceAndDropTicketApprovalWorkFlowDisabled_Approver_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceAndDropTicketApprovalWorkFlowDisabled_Approver_BodyButtonText;
                        response.CompanyText = Resource.emailInvoiceAndDropTicketApprovalWorkFlowDisabled_Approver_HeaderText;
                        break;

                    case EventSubType.InvoiceAndDropTicketApprovalWorkFlowDisabled_Admins:
                        response.Subject = Resource.emailInvoiceAndDropTicketApprovalWorkFlowDisabled_Admins_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceAndDropTicketApprovalWorkFlowDisabled_Admins_BodyLogo);
                        response.BodyText = Resource.emailInvoiceAndDropTicketApprovalWorkFlowDisabled_Admins_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceAndDropTicketApprovalWorkFlowDisabled_Admins_BodyButtonText;
                        response.CompanyText = Resource.emailInvoiceAndDropTicketApprovalWorkFlowDisabled_Admins_HeaderText;
                        break;

                    case EventSubType.DropTicketCreated_Supplier:
                        response.Subject = Resource.emailDropTicketCreated_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.BodyLogo_HandShake);
                        response.BodyText = Resource.emailDropTicketCreated_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketApproved_SupplierCompanyAdmin_BodyButtonText;
                        break;

                    case EventSubType.FuelRequestCreated_Owner:
                        response.Subject = Resource.emailFuelRequestCreated_Owner_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailFuelRequestCreated_Owner_BodyLogo);
                        response.BodyText = Resource.emailFuelRequestCreated_Owner_BodyText;
                        response.BodyButtonText = Resource.emailFuelRequestCreated_Owner_BodyButtonText;
                        response.CompanyText = Resource.emailFuelRequestCreated_Owner_HeaderText;
                        break;

                    case EventSubType.FuelRequestCreated_CompanyAdmin:
                        response.Subject = Resource.emailFuelRequestCreated_CompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailFuelRequestCreated_CompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailFuelRequestCreated_CompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailFuelRequestCreated_CompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailFuelRequestCreated_CompanyAdmin_HeaderText;
                        break;

                    case EventSubType.FuelRequestUpdated_Owner:
                        response.Subject = Resource.emailFuelRequestUpdated_Owner_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailFuelRequestUpdated_Owner_BodyLogo);
                        response.BodyText = Resource.emailFuelRequestUpdated_Owner_BodyText;
                        response.BodyButtonText = Resource.emailFuelRequestUpdated_Owner_BodyButtonText;
                        response.CompanyText = Resource.emailFuelRequestUpdated_Owner_HeaderText;
                        break;

                    case EventSubType.FuelRequestUpdated_CompanyAdmin:
                        response.Subject = Resource.emailFuelRequestUpdated_CompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailFuelRequestUpdated_CompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailFuelRequestUpdated_CompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailFuelRequestUpdated_CompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailFuelRequestUpdated_CompanyAdmin_HeaderText;
                        break;

                    case EventSubType.FuelRequestCreated_Supplier:
                        response.Subject = Resource.emailFuelRequestCreated_Suppliers_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailFuelRequestCreated_Suppliers_BodyLogo);
                        response.BodyText = Resource.emailFuelRequestCreated_Suppliers_BodyText;
                        response.BodyButtonText = Resource.emailFuelRequestCreated_Suppliers_BodyButtonText;
                        response.CompanyText = Resource.emailFuelRequestCreated_Suppliers_HeaderText;
                        break;

                    case EventSubType.FuelRequestAccepted_Owner:
                        response.Subject = Resource.emailFuelRequestAccepted_Owner_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailFuelRequestAccepted_Owner_BodyLogo);
                        response.BodyText = Resource.emailFuelRequestAccepted_Owner_BodyText;
                        response.BodyButtonText = Resource.emailFuelRequestAccepted_Owner_BodyButtonText;
                        response.CompanyText = Resource.emailFuelRequestAccepted_Owner_HeaderText;
                        break;

                    case EventSubType.FuelRequestAccepted_OwnerCompanyAdmin:
                        response.Subject = Resource.emailFuelRequestAccepted_OwnerCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailFuelRequestAccepted_OwnerCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailFuelRequestAccepted_OwnerCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailFuelRequestAccepted_OwnerCompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailFuelRequestAccepted_OwnerCompanyAdmin_HeaderText;
                        break;

                    case EventSubType.FuelRequestAccepted_Supplier:
                        response.Subject = Resource.emailFuelRequestAccepted_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailFuelRequestAccepted_Supplier_BodyLogo);
                        response.BodyText = Resource.emailFuelRequestAccepted_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailFuelRequestAccepted_Supplier_BodyButtonText;
                        break;

                    case EventSubType.FuelRequestAccepted_SupplierCompanyAdmin:
                        response.Subject = Resource.emailFuelRequestAccepted_SupplierCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailFuelRequestAccepted_SupplierCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailFuelRequestAccepted_SupplierCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailFuelRequestAccepted_SupplierCompanyAdmin_BodyButtonText;
                        break;

                    case EventSubType.CounterOfferCreated_Buyer:
                        response.Subject = Resource.emailCounterOfferCreated_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailCounterOfferCreated_Buyer_BodyLogo);
                        response.BodyText = Resource.emailCounterOfferCreated_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailCounterOfferCreated_Buyer_BodyButtonText;
                        response.CompanyText = Resource.emailCounterOfferCreated_Buyer_HeaderText;
                        break;

                    case EventSubType.CounterOfferCreated_Supplier:
                        response.Subject = Resource.emailCounterOfferCreated_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailCounterOfferCreated_Supplier_BodyLogo);
                        response.BodyText = Resource.emailCounterOfferCreated_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailCounterOfferCreated_Supplier_BodyButtonText;
                        response.CompanyText = Resource.emailCounterOfferCreated_Supplier_HeaderText;
                        break;

                    case EventSubType.OrderClosed_Owner:
                        response.Subject = Resource.emailOrderClosed_Owner_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailOrderClosed_Owner_BodyLogo);
                        response.BodyText = Resource.emailOrderClosed_Owner_BodyText;
                        response.BodyButtonText = Resource.emailOrderClosed_Owner_BodyButtonText;
                        response.CompanyText = Resource.emailOrderClosed_HeaderText;
                        break;

                    case EventSubType.OrderClosed_OwnerCompanyAdmin:
                        response.Subject = Resource.emailOrderClosed_OwnerCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailOrderClosed_OwnerCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailOrderClosed_OwnerCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailOrderClosed_OwnerCompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailOrderClosed_HeaderText;
                        break;

                    case EventSubType.OrderClosed_Buyer:
                        response.Subject = Resource.emailOrderClosed_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailOrderClosed_Buyer_BodyLogo);
                        response.BodyText = Resource.emailOrderClosed_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailOrderClosed_Buyer_BodyButtonText;
                        response.CompanyText = Resource.emailOrderClosed_HeaderText;
                        break;

                    case EventSubType.OrderClosed_BuyerCompanyAdmin:
                        response.Subject = Resource.emailOrderClosed_BuyerCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailOrderClosed_BuyerCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailOrderClosed_BuyerCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailOrderClosed_BuyerCompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailOrderClosed_HeaderText;
                        break;

                    case EventSubType.OrderClosedAndFuelRequestResubmitted_Buyer:
                        response.Subject = Resource.emailOrderClosedAndFuelRequestResubmitted_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailOrderClosedAndFuelRequestResubmitted_Buyer_BodyLogo);
                        response.BodyText = Resource.emailOrderClosedAndFuelRequestResubmitted_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailOrderClosedAndFuelRequestResubmitted_Buyer_BodyButtonText;
                        response.CompanyText = Resource.emailOrderClosedAndFuelRequestResubmitted_Buyer_HeaderText;
                        break;

                    case EventSubType.OrderCancelled_Owner:
                        response.Subject = Resource.emailOrderCancelled_Owner_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailOrderCancelled_Owner_BodyLogo);
                        response.BodyText = Resource.emailOrderCancelled_Owner_BodyText;
                        response.BodyButtonText = Resource.emailOrderCancelled_Owner_BodyButtonText;
                        break;

                    case EventSubType.OrderCancelled_OwnerCompanyAdmin:
                        response.Subject = Resource.emailOrderCancelled_OwnerCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailOrderCancelled_OwnerCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailOrderCancelled_OwnerCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailOrderCancelled_OwnerCompanyAdmin_BodyButtonText;
                        break;

                    case EventSubType.OrderCancelled_Buyer:
                        response.Subject = Resource.emailOrderCancelled_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailOrderCancelled_Buyer_BodyLogo);
                        response.BodyText = Resource.emailOrderCancelled_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailOrderCancelled_Buyer_BodyButtonText;
                        break;

                    case EventSubType.OrderCancelled_BuyerCompanyAdmin:
                        response.Subject = Resource.emailOrderCancelled_BuyerCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailOrderCancelled_BuyerCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailOrderCancelled_BuyerCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailOrderCancelled_BuyerCompanyAdmin_BodyButtonText;
                        break;

                    case EventSubType.OrderCanceledAndFuelRequestResubmitted_Buyer:
                        response.Subject = Resource.emailOrderCanceledAndFuelRequestResubmitted_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailOrderCanceledAndFuelRequestResubmitted_Buyer_BodyLogo);
                        response.BodyText = Resource.emailOrderCanceledAndFuelRequestResubmitted_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailOrderCanceledAndFuelRequestResubmitted_Buyer_BodyButtonText;
                        break;

                    case EventSubType.OrderUpdated_Buyer:
                        response.Subject = Resource.emailOrderUpdated_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailOrderUpdated_Buyer_BodyLogo);
                        response.BodyText = Resource.emailOrderUpdated_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailOrderUpdated_Buyer_BodyButtonText;
                        break;

                    case EventSubType.OrderUpdated_Supplier:
                        response.Subject = Resource.emailOrderUpdated_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailOrderUpdated_Supplier_BodyLogo);
                        response.BodyText = Resource.emailOrderUpdated_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailOrderUpdated_Supplier_BodyButtonText;
                        break;

                    case EventSubType.InvoiceExceedingQuantityCreated_SupplierCompanyAdmin:
                        response.Subject = Resource.emailInvoiceExceedingQuantityCreated_SupplierCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceExceedingQuantityCreated_SupplierCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailInvoiceExceedingQuantityCreated_SupplierCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceExceedingQuantityCreated_SupplierCompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailInvoiceExceedingQuantity_HeaderText;
                        break;

                    case EventSubType.InvoiceExceedingQuantityCreated_Buyer:
                        response.Subject = Resource.emailInvoiceExceedingQuantityCreated_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceExceedingQuantityCreated_Buyer_BodyLogo);
                        response.BodyText = Resource.emailInvoiceExceedingQuantityCreated_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceExceedingQuantityCreated_Buyer_BodyButtonText;
                        response.CompanyText = Resource.emailInvoiceExceedingQuantity_HeaderText;
                        break;

                    case EventSubType.InvoiceExceedingQuantityCreated_BuyerCompanyAdmin:
                        response.Subject = Resource.emailInvoiceExceedingQuantityCreated_BuyerCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceExceedingQuantityCreated_BuyerCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailInvoiceExceedingQuantityCreated_BuyerCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceExceedingQuantityCreated_BuyerCompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailInvoiceExceedingQuantity_HeaderText;
                        break;

                    case EventSubType.InvoiceApproved_Supplier:
                        response.Subject = Resource.emailInvoiceApproved_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceApproved_Supplier_BodyLogo);
                        response.BodyText = Resource.emailInvoiceApproved_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceApproved_Supplier_BodyButtonText;
                        break;

                    case EventSubType.InvoiceApproved_SupplierCompanyAdmin:
                        response.Subject = Resource.emailInvoiceApproved_SupplierCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceApproved_SupplierCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailInvoiceApproved_SupplierCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceApproved_SupplierCompanyAdmin_BodyButtonText;
                        break;

                    case EventSubType.InvoiceApproved_Buyer:
                        response.Subject = Resource.emailInvoiceApproved_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceApproved_Buyer_BodyLogo);
                        response.BodyText = Resource.emailInvoiceApproved_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceApproved_Buyer_BodyButtonText;
                        break;

                    case EventSubType.InvoiceApproved_Approver:
                        response.Subject = Resource.emailInvoiceApproved_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceApproved_Buyer_BodyLogo);
                        response.BodyText = Resource.emailInvoiceApproved_Approver_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceApproved_Buyer_BodyButtonText;
                        break;

                    case EventSubType.InvoiceApproved_BuyerCompanyAdmin:
                        response.Subject = Resource.emailInvoiceApproved_BuyerCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceApproved_BuyerCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailInvoiceApproved_BuyerCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceApproved_BuyerCompanyAdmin_BodyButtonText;
                        break;

                    case EventSubType.InvoiceExceedingQuantityCreated_Supplier:
                        response.Subject = Resource.emailInvoiceExceedingQuantityCreated_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceExceedingQuantityCreated_Supplier_BodyLogo);
                        response.BodyText = Resource.emailInvoiceExceedingQuantityCreated_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceExceedingQuantityCreated_Supplier_BodyButtonText;
                        response.CompanyText = Resource.emailInvoiceExceedingQuantity_HeaderText;
                        break;

                    case EventSubType.InvoiceApprovalReminder_Buyer:
                    case EventSubType.InvoiceApprovalReminder_BuyerCompanyAdmin:
                        response.Subject = Resource.emailInvoiceApprovalReminder_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceApprovalReminder_Buyer_BodyLogo);
                        response.BodyText = Resource.emailInvoiceApprovalReminder_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceApprovalReminder_Buyer_BodyTextButton;
                        break;

                    case EventSubType.DropTicketApprovalReminder_Buyer:
                    case EventSubType.DropTicketApprovalReminder_BuyerCompanyAdmin:
                        response.Subject = Resource.emailDropTicketApprovalReminder_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDropTicketApprovalReminder_Buyer_BodyLogo);
                        response.BodyText = Resource.emailDropTicketApprovalReminder_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketApprovalReminder_Buyer_BodyTextButton;
                        break;

                    case EventSubType.InvoiceApprovalReminder_Supplier:
                    case EventSubType.InvoiceApprovalReminder_SupplierCompanyAdmin:
                        response.Subject = Resource.emailInvoiceApprovalReminder_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceApprovalReminder_Supplier_BodyLogo);
                        response.BodyText = Resource.emailInvoiceApprovalReminder_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceApprovalReminder_Supplier_BodyTextButton;
                        break;

                    case EventSubType.DropTicketApprovalReminder_Supplier:
                    case EventSubType.DropTicketApprovalReminder_SupplierCompanyAdmin:
                        response.Subject = Resource.emailDropTicketApprovalReminder_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDropTicketApprovalReminder_Supplier_BodyLogo);
                        response.BodyText = Resource.emailDropTicketApprovalReminder_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketApprovalReminder_Supplier_BodyTextButton;
                        break;

                    case EventSubType.InvoiceCreated_Supplier_AprovalWorkflow:
                        response.Subject = Resource.emailInvoiceCreated_Supplier_AprovalWorkflow_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceCreated_Supplier_AprovalWorkflow_BodyLogo);
                        response.BodyText = Resource.emailInvoiceCreated_Supplier_AprovalWorkflow_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceCreated_Supplier_AprovalWorkflow_BodyButtonText;
                        break;

                    case EventSubType.DropTicketCreated_Supplier_AprovalWorkflow:
                        response.Subject = Resource.emailDropTicketCreated_Supplier_AprovalWorkflow_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDropTicketCreated_Supplier_AprovalWorkflow_BodyLogo);
                        response.BodyText = Resource.emailDropTicketCreated_Supplier_AprovalWorkflow_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketCreated_Supplier_AprovalWorkflow_BodyButtonText;
                        break;

                    case EventSubType.InvoiceCreated_Buyer_AprovalWorkflow:
                        response.Subject = Resource.emailInvoiceCreated_Buyer_AprovalWorkflow_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceCreated_Buyer_AprovalWorkflow_BodyLogo);
                        response.BodyText = Resource.emailInvoiceCreated_Buyer_AprovalWorkflow_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceCreated_Buyer_AprovalWorkflow_BodyButtonText;
                        break;

                    case EventSubType.DropTicketCreated_Buyer_AprovalWorkflow:
                        response.Subject = Resource.emailDropTicketCreated_Buyer_AprovalWorkflow_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDropTicketCreated_Buyer_AprovalWorkflow_BodyLogo);
                        response.BodyText = Resource.emailDropTicketCreated_Buyer_AprovalWorkflow_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketCreated_Buyer_AprovalWorkflow_BodyButtonText;
                        break;

                    case EventSubType.InvoiceCreated_AprovalWorkflow_SupplierCompanyAdmin:
                        response.Subject = Resource.emailInvoiceCreated_SupplierCompanyAdmin_AprovalWorkflow_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceCreated_SupplierCompanyAdmin_AprovalWorkflow_BodyLogo);
                        response.BodyText = Resource.emailInvoiceCreated_SupplierCompanyAdmin_AprovalWorkflow_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceCreated_SupplierCompanyAdmin_AprovalWorkflow_BodyButtonText;
                        break;

                    case EventSubType.DropTicketCreated_AprovalWorkflow_SupplierCompanyAdmin:
                        response.Subject = Resource.emailDropTicketCreated_SupplierCompanyAdmin_AprovalWorkflow_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDropTicketCreated_SupplierCompanyAdmin_AprovalWorkflow_BodyLogo);
                        response.BodyText = Resource.emailDropTicketCreated_SupplierCompanyAdmin_AprovalWorkflow_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketCreated_SupplierCompanyAdmin_AprovalWorkflow_BodyButtonText;
                        break;

                    case EventSubType.InvoiceCreated_AprovalWorkflow_BuyerCompanyAdmin:
                        response.Subject = Resource.emailInvoiceCreated_BuyerCompanyAdmin_AprovalWorkflow_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceCreated_BuyerCompanyAdmin_AprovalWorkflow_BodyLogo);
                        response.BodyText = Resource.emailInvoiceCreated_BuyerCompanyAdmin_AprovalWorkflow_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceCreated_BuyerCompanyAdmin_AprovalWorkflow_BodyButtonText;
                        break;

                    case EventSubType.DropTicketCreated_AprovalWorkflow_BuyerCompanyAdmin:
                        response.Subject = Resource.emailDropTicketCreated_BuyerCompanyAdmin_AprovalWorkflow_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDropTicketCreated_BuyerCompanyAdmin_AprovalWorkflow_BodyLogo);
                        response.BodyText = Resource.emailDropTicketCreated_BuyerCompanyAdmin_AprovalWorkflow_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketCreated_BuyerCompanyAdmin_AprovalWorkflow_BodyButtonText;
                        break;

                    case EventSubType.InvoiceApproved_Buyer_AprovalWorkflow:
                    case EventSubType.InvoiceApproved_Supplier_AprovalWorkflow:
                    case EventSubType.InvoiceApproved_BuyerCompanyAdmin_AprovalWorkflow:
                    case EventSubType.InvoiceApproved_SupplierCompanyAdmin_AprovalWorkflow:
                        response.Subject = Resource.emailInvoiceApproved_Supplier_AprovalWorkflow_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceApproved_Supplier_AprovalWorkflow_BodyLogo);
                        response.BodyText = Resource.emailInvoiceApproved_Supplier_AprovalWorkflow_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceApproved_Supplier_AprovalWorkflow_BodyButtonText;
                        break;

                    case EventSubType.DropTicketApproved_Buyer_AprovalWorkflow:
                    case EventSubType.DropTicketApproved_Supplier_AprovalWorkflow:
                    case EventSubType.DropTicketApproved_BuyerCompanyAdmin_AprovalWorkflow:
                    case EventSubType.DropTicketApproved_SupplierCompanyAdmin_AprovalWorkflow:
                        response.Subject = Resource.emailDropTicketApproved_Supplier_AprovalWorkflow_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDropTicketApproved_Supplier_AprovalWorkflow_BodyLogo);
                        response.BodyText = Resource.emailDropTicketApproved_Supplier_AprovalWorkflow_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketApproved_Supplier_AprovalWorkflow_BodyButtonText;
                        break;

                    case EventSubType.DropTicketExceedingQuantityCreated_SupplierCompanyAdmin:
                        response.Subject = Resource.emailDropTicketExceedingQuantityCreated_SupplierCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDropTicketExceedingQuantityCreated_SupplierCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailDropTicketExceedingQuantityCreated_SupplierCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketExceedingQuantityCreated_SupplierCompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailDropTicketExceedingQuantity_HeaderText;
                        break;

                    case EventSubType.DropTicketExceedingQuantityCreated_Buyer:
                        response.Subject = Resource.emailDropTicketExceedingQuantityCreated_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDropTicketExceedingQuantityCreated_Buyer_BodyLogo);
                        response.BodyText = Resource.emailDropTicketExceedingQuantityCreated_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketExceedingQuantityCreated_Buyer_BodyButtonText;
                        response.CompanyText = Resource.emailDropTicketExceedingQuantity_HeaderText;
                        break;

                    case EventSubType.DropTicketExceedingQuantityCreated_BuyerCompanyAdmin:
                        response.Subject = Resource.emailDropTicketExceedingQuantityCreated_BuyerCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDropTicketExceedingQuantityCreated_BuyerCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailDropTicketExceedingQuantityCreated_BuyerCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketExceedingQuantityCreated_BuyerCompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailDropTicketExceedingQuantity_HeaderText;
                        break;

                    case EventSubType.DropTicketApproved_Supplier:
                        response.Subject = Resource.emailDropTicketApproved_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDropTicketApproved_Supplier_BodyLogo);
                        response.BodyText = Resource.emailDropTicketApproved_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketApproved_Supplier_BodyButtonText;
                        break;

                    case EventSubType.DropTicketApproved_SupplierCompanyAdmin:
                        response.Subject = Resource.emailDropTicketApproved_SupplierCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDropTicketApproved_SupplierCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailDropTicketApproved_SupplierCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketApproved_SupplierCompanyAdmin_BodyButtonText;
                        break;

                    case EventSubType.DropTicketApproved_Buyer:
                        response.Subject = Resource.emailDropTicketApproved_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDropTicketApproved_Buyer_BodyLogo);
                        response.BodyText = Resource.emailDropTicketApproved_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketApproved_Buyer_BodyButtonText;
                        break;

                    case EventSubType.DropTicketApproved_BuyerCompanyAdmin:
                        response.Subject = Resource.emailDropTicketApproved_BuyerCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDropTicketApproved_BuyerCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailDropTicketApproved_BuyerCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketApproved_BuyerCompanyAdmin_BodyButtonText;
                        break;

                    case EventSubType.DropTicketExceedingQuantityCreated_Supplier:
                        response.Subject = Resource.emailDropTicketExceedingQuantityCreated_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDropTicketExceedingQuantityCreated_Supplier_BodyLogo);
                        response.BodyText = Resource.emailDropTicketExceedingQuantityCreated_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketExceedingQuantityCreated_Supplier_BodyButtonText;
                        response.CompanyText = Resource.emailDropTicketExceedingQuantity_HeaderText;
                        break;

                    case EventSubType.BrokerFuelRequestCreated_Owner:
                        response.Subject = Resource.emailBrokerFuelRequestCreated_Owner_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailBrokerFuelRequestCreated_Owner_BodyLogo);
                        response.BodyText = Resource.emailBrokerFuelRequestCreated_Owner_BodyText;
                        response.BodyButtonText = Resource.emailBrokerFuelRequestCreated_Owner_BodyButtonText;
                        break;

                    case EventSubType.BrokerFuelRequestCreated_CompanyAdmin:
                        response.Subject = Resource.emailBrokerFuelRequestCreated_CompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailBrokerFuelRequestCreated_CompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailBrokerFuelRequestCreated_CompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailBrokerFuelRequestCreated_CompanyAdmin_BodyButtonText;
                        break;

                    case EventSubType.BrokerFuelRequestUpdated_Owner:
                        response.Subject = Resource.emailBrokerFuelRequestUpdated_Owner_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailBrokerFuelRequestUpdated_Owner_BodyLogo);
                        response.BodyText = Resource.emailBrokerFuelRequestUpdated_Owner_BodyText;
                        response.BodyButtonText = Resource.emailBrokerFuelRequestUpdated_Owner_BodyButtonText;
                        break;

                    case EventSubType.BrokerFuelRequestUpdated_CompanyAdmin:
                        response.Subject = Resource.emailBrokerFuelRequestUpdated_CompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailBrokerFuelRequestUpdated_CompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailBrokerFuelRequestUpdated_CompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailBrokerFuelRequestUpdated_CompanyAdmin_BodyButtonText;
                        break;

                    case EventSubType.BrokerFuelRequestCancelled_Owner:
                        response.Subject = Resource.emailBrokerFuelRequestCancelled_Owner_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailBrokerFuelRequestCancelled_Owner_BodyLogo);
                        response.BodyText = Resource.emailBrokerFuelRequestCancelled_Owner_BodyText;
                        response.BodyButtonText = Resource.emailBrokerFuelRequestCancelled_Owner_BodyButtonText;
                        break;

                    case EventSubType.BrokerFuelRequestCancelled_CompanyAdmin:
                        response.Subject = Resource.emailBrokerFuelRequestCancelled_CompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailBrokerFuelRequestCancelled_CompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailBrokerFuelRequestCancelled_CompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailBrokerFuelRequestCancelled_CompanyAdmin_BodyButtonText;
                        break;
                    case EventSubType.DeliveryRequestCreated_Buyer:
                        response.Subject = Resource.emailDeliveryRequestCreated_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestCreated_Buyer_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestCreated_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailDeliveryRequestCreated_Buyer_BodyButtonText;
                        response.CompanyText = Resource.emailDeliveryRequestCreated_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestCreatedForTPO_Buyer:
                        response.Subject = Resource.emailDeliveryRequestCreated_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestCreated_Buyer_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestCreatedForTPO_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailDeliveryRequestCreated_Buyer_BodyButtonText;
                        response.CompanyText = Resource.emailDeliveryRequestCreated_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestCreated_Supplier:
                        response.Subject = Resource.emailDeliveryRequestCreated_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestCreated_Supplier_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestCreated_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailDeliveryRequestCreated_Supplier_BodyButtonText;
                        response.CompanyText = Resource.emailDeliveryRequestCreated_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestUpdated_Supplier:
                        response.Subject = Resource.emailDeliveryRequestUpdated_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestUpdated_Supplier_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestUpdated_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailDeliveryRequestUpdated_Supplier_BodyButtonText;
                        response.CompanyText = Resource.emailDeliveryRequestUpdated_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestUpdated_Buyer:
                        response.Subject = Resource.emailDeliveryRequestUpdated_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestUpdated_Buyer_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestUpdated_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailDeliveryRequestUpdated_Buyer_BodyButtonText;
                        response.CompanyText = Resource.emailDeliveryRequestUpdated_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestUpdatedForTPO_Buyer:
                        response.Subject = Resource.emailDeliveryRequestUpdated_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestUpdated_Buyer_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestUpdatedForTPO_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailDeliveryRequestUpdated_Buyer_BodyButtonText;
                        response.CompanyText = Resource.emailDeliveryRequestUpdated_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestCreated_BuyerCompanyAdmin:
                        response.Subject = Resource.emailDeliveryRequestCreated_BuyerCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestCreated_BuyerCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestCreated_BuyerCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailDeliveryRequestCreated_BuyerCompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailDeliveryRequestCreated_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestCreatedForTPO_BuyerCompanyAdmin:
                        response.Subject = Resource.emailDeliveryRequestCreated_BuyerCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestCreated_BuyerCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestCreatedForTPO_BuyerCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailDeliveryRequestCreated_BuyerCompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailDeliveryRequestCreated_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestCreated_SupplierCompanyAdmin:
                        response.Subject = Resource.emailDeliveryRequestCreated_SupplierCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestCreated_SupplierCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestCreated_SupplierCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailDeliveryRequestCreated_SupplierCompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailDeliveryRequestCreated_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestUpdated_BuyerCompanyAdmin:
                        response.Subject = Resource.emailDeliveryRequestUpdated_BuyerCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestUpdated_BuyerCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestUpdated_BuyerCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailDeliveryRequestUpdated_BuyerCompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailDeliveryRequestUpdated_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestUpdatedForTPO_BuyerCompanyAdmin:
                        response.Subject = Resource.emailDeliveryRequestUpdated_BuyerCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestUpdated_BuyerCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestUpdatedForTPO_BuyerCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailDeliveryRequestUpdated_BuyerCompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailDeliveryRequestUpdated_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestUpdated_SupplierCompanyAdmin:
                        response.Subject = Resource.emailDeliveryRequestUpdated_SupplierCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestUpdated_SupplierCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestUpdated_SupplierCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailDeliveryRequestUpdated_SupplierCompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailDeliveryRequestUpdated_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestReminder_Buyer:
                        response.Subject = Resource.emailDeliveryRequestReminder_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestReminder_Buyer_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestReminder_Buyer_BodyText;
                        response.BodyButtonText = string.Empty;
                        response.CompanyText = Resource.emailDeliveryRequestReminder_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestReminder_BuyerCompanyAdmin:
                        response.Subject = Resource.emailDeliveryRequestReminder_BuyerCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestReminder_BuyerCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestReminder_BuyerCompanyAdmin_BodyText;
                        response.BodyButtonText = string.Empty;
                        response.CompanyText = Resource.emailDeliveryRequestReminder_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestReminder_Supplier:
                        response.Subject = Resource.emailDeliveryRequestReminder_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestReminder_Supplier_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestReminder_Supplier_BodyText;
                        response.BodyButtonText = string.Empty;
                        response.CompanyText = Resource.emailDeliveryRequestReminder_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestReminder_SupplierCompanyAdmin:
                        response.Subject = Resource.emailDeliveryRequestReminder_SupplierCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestReminder_SupplierCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestReminder_SupplierCompanyAdmin_BodyText;
                        response.BodyButtonText = string.Empty;
                        response.CompanyText = Resource.emailDeliveryRequestReminder_HeaderText;
                        break;
                    case EventSubType.DriverAssignedToDelivery_Driver:
                        response.Subject = Resource.emailDriverAssignmentToDelivery_Driver_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDriverAssignmentToDelivery_Driver_BodyLogo);
                        response.BodyText = Resource.emailDriverAssignmentToDelivery_Driver_BodyText;
                        response.BodyButtonText = Resource.emailDriverAssignmentToDelivery_Driver_BodyButtonText;
                        response.CompanyText = Resource.emailDriverAssignmentToDelivery_Driver_HeaderText;
                        break;
                    case EventSubType.DriverAssignedToDelivery_Supplier:
                        response.Subject = Resource.emailDriverAssignmentToDelivery_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDriverAssignmentToDelivery_Supplier_BodyLogo);
                        response.BodyText = Resource.emailDriverAssignmentToDelivery_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailDriverAssignmentToDelivery_Supplier_BodyButtonText;
                        response.CompanyText = Resource.emailDriverAssignmentToDelivery_Supplier_HeaderText;
                        break;
                    case EventSubType.DriverAssignedToDelivery_SupplierAdmin:
                        response.Subject = Resource.emailDriverAssignmentToDelivery_SupplierAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDriverAssignmentToDelivery_SupplierAdmin_BodyLogo);
                        response.BodyText = Resource.emailDriverAssignmentToDelivery_SupplierAdmin_BodyText;
                        response.BodyButtonText = Resource.emailDriverAssignmentToDelivery_SupplierAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailDriverAssignmentToDelivery_SupplierAdmin_HeaderText;
                        break;
                    case EventSubType.DriverRemovedFromDelivery_Driver:
                        response.Subject = Resource.emailDriverRemovedFromDelivery_Driver_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDriverRemovedFromDelivery_Driver_BodyLogo);
                        response.BodyText = Resource.emailDriverRemovedFromDelivery_Driver_BodyText;
                        response.BodyButtonText = Resource.emailDriverRemovedFromDelivery_Driver_BodyButtonText;
                        response.CompanyText = Resource.emailDriverRemovedFromDelivery_Driver_HeaderText;
                        break;
                    case EventSubType.DriverRemovedFromDelivery_Supplier:
                        response.Subject = Resource.emailDriverRemovedFromDelivery_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDriverRemovedFromDelivery_Supplier_BodyLogo);
                        response.BodyText = Resource.emailDriverRemovedFromDelivery_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailDriverRemovedFromDelivery_Supplier_BodyButtonText;
                        response.CompanyText = Resource.emailDriverRemovedFromDelivery_Supplier_HeaderText;
                        break;
                    case EventSubType.DriverRemovedFromDelivery_SupplierAdmin:
                        response.Subject = Resource.emailDriverRemovedFromDelivery_SupplierAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDriverRemovedFromDelivery_SupplierAdmin_BodyLogo);
                        response.BodyText = Resource.emailDriverRemovedFromDelivery_SupplierAdmin_BodyText;
                        response.BodyButtonText = Resource.emailDriverRemovedFromDelivery_SupplierAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailDriverRemovedFromDelivery_SupplierAdmin_HeaderText;
                        break;
                    case EventSubType.DriverAssignedToOrder_Driver:
                        response.Subject = Resource.emailDriverAssignmentToOrder_Driver_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDriverAssignmentToOrder_Driver_BodyLogo);
                        response.BodyText = Resource.emailDriverAssignmentToOrder_Driver_BodyText;
                        response.BodyButtonText = Resource.emailDriverAssignmentToOrder_Driver_BodyButtonText;
                        response.CompanyText = Resource.emailDriverAssignmentToOrder_Driver_HeaderText;
                        break;
                    case EventSubType.DriverAssignedToOrder_Supplier:
                        response.Subject = Resource.emailDriverAssignmentToOrder_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDriverAssignmentToOrder_Supplier_BodyLogo);
                        response.BodyText = Resource.emailDriverAssignmentToOrder_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailDriverAssignmentToOrder_Supplier_BodyButtonText;
                        response.CompanyText = Resource.emailDriverAssignmentToOrder_Supplier_HeaderText;
                        break;
                    case EventSubType.DriverAssignedToOrder_SupplierAdmin:
                        response.Subject = Resource.emailDriverAssignmentToOrder_SupplierAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDriverAssignmentToOrder_SupplierAdmin_BodyLogo);
                        response.BodyText = Resource.emailDriverAssignmentToOrder_SupplierAdmin_BodyText;
                        response.BodyButtonText = Resource.emailDriverAssignmentToOrder_SupplierAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailDriverAssignmentToOrder_SupplierAdmin_HeaderText;
                        break;
                    case EventSubType.DriverRemovedFromOrder_Driver:
                        response.Subject = Resource.emailDriverRemovedFromOrder_Driver_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDriverRemovedFromOrder_Driver_BodyLogo);
                        response.BodyText = Resource.emailDriverRemovedFromOrder_Driver_BodyText;
                        response.BodyButtonText = Resource.emailDriverRemovedFromOrder_Driver_BodyButtonText;
                        response.CompanyText = Resource.emailDriverRemovedFromOrder_Driver_HeaderText;
                        break;
                    case EventSubType.DriverRemovedFromOrder_Supplier:
                        response.Subject = Resource.emailDriverRemovedFromOrder_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDriverRemovedFromOrder_Supplier_BodyLogo);
                        response.BodyText = Resource.emailDriverRemovedFromOrder_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailDriverRemovedFromOrder_Supplier_BodyButtonText;
                        response.CompanyText = Resource.emailDriverRemovedFromOrder_Supplier_HeaderText;
                        break;
                    case EventSubType.DriverRemovedFromOrder_SupplierAdmin:
                        response.Subject = Resource.emailDriverRemovedFromOrder_SupplierAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDriverRemovedFromOrder_SupplierAdmin_BodyLogo);
                        response.BodyText = Resource.emailDriverRemovedFromOrder_SupplierAdmin_BodyText;
                        response.BodyButtonText = Resource.emailDriverRemovedFromOrder_SupplierAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailDriverRemovedFromOrder_SupplierAdmin_HeaderText;
                        break;
                    case EventSubType.SuperAdminCreatedNewUser:
                        response.Subject = Resource.emailSuperAdminCreatedNewUser_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailSuperAdminCreatedNewUser_BodyLogo);
                        response.BodyText = Resource.emailSuperAdminCreatedNewUser_BodyText;
                        response.BodyButtonText = Resource.emailSuperAdminCreatedNewUser_BodyButtonText;
                        response.CompanyText = Resource.emailSuperAdminCreatedNewUser_HeaderText;
                        break;
                    case EventSubType.SuperAdminOnboardedNewCompany:
                        response.Subject = Resource.emailSuperAdminOnboardedNewCompany_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailSuperAdminOnboardedNewCompany_BodyLogo);
                        response.BodyText = Resource.emailSuperAdminOnboardedNewCompany_BodyText;
                        response.BodyButtonText = Resource.emailSuperAdminOnboardedNewCompany_BodyButtonText;
                        response.CompanyText = Resource.emailSuperAdminOnboardedNewCompany_HeaderText;
                        break;
                    case EventSubType.NeedFuelIntimationCreated:
                        response.Subject = Resource.emailNeedFuelIntimationCreated_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailNeedFuelIntimationCreated_BodyLogo);
                        response.BodyText = Resource.emailNeedFuelIntimationCreated_BodyText;
                        response.BodyButtonText = Resource.emailNeedFuelIntimationCreated_BodyButtonText;
                        response.CompanyText = Resource.emailNeedFuelIntimationCreated_HeaderText;
                        break;
                    case EventSubType.InvoiceTaxValuesChanged_Buyer:
                        response.Subject = Resource.emailInvoiceTaxValuesChanged_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailInvoiceTaxValuesChanged_BodyLogo);
                        response.BodyText = Resource.emailInvoiceTaxValuesChanged_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceTaxValuesChanged_BodyButtonText;
                        response.CompanyText = Resource.emailInvoiceTaxValuesChanged_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestRescheduled_Buyer:
                        response.Subject = Resource.emailDeliveryRequestRescheduled_Buyer_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestRescheduled_Buyer_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestRescheduled_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailDeliveryRequestRescheduled_Buyer_BodyButtonText;
                        response.CompanyText = Resource.emailDeliveryRequestRescheduled_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestRescheduled_Supplier:
                        response.Subject = Resource.emailDeliveryRequestRescheduled_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestRescheduled_Supplier_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestRescheduled_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailDeliveryRequestRescheduled_Supplier_BodyButtonText;
                        response.CompanyText = Resource.emailDeliveryRequestRescheduled_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestRescheduled_BuyerCompanyAdmin:
                        response.Subject = Resource.emailDeliveryRequestRescheduled_BuyerCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestRescheduled_BuyerCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestRescheduled_BuyerCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailDeliveryRequestRescheduled_BuyerCompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailDeliveryRequestRescheduled_HeaderText;
                        break;
                    case EventSubType.DeliveryRequestRescheduled_SupplierCompanyAdmin:
                        response.Subject = Resource.emailDeliveryRequestRescheduled_SupplierCompanyAdmin_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestRescheduled_SupplierCompanyAdmin_BodyLogo);
                        response.BodyText = Resource.emailDeliveryRequestRescheduled_SupplierCompanyAdmin_BodyText;
                        response.BodyButtonText = Resource.emailDeliveryRequestRescheduled_SupplierCompanyAdmin_BodyButtonText;
                        response.CompanyText = Resource.emailDeliveryRequestRescheduled_HeaderText;
                        break;
                    case EventSubType.InvoiceDeleteRequested_Supplier:
                        response.Subject = Resource.InvoiceDeleteRequested_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.InvoiceDeleteRequested_BodyLogo);
                        response.BodyText = Resource.InvoiceDeleteRequested_BodyText;
                        response.BodyButtonText = Resource.InvoiceDeleteRequested_BodyButtonText;
                        response.CompanyText = Resource.InvoiceDeleteRequested_HeaderText;
                        break;
                    case EventSubType.CancelInvoiceDeleteRequest_Supplier:
                        response.Subject = Resource.CancelInvoiceDeleteRequest_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.CancelInvoiceDeleteRequest_BodyLogo);
                        response.BodyText = Resource.CancelInvoiceDeleteRequest_BodyText;
                        response.CompanyText = Resource.CancelInvoiceDeleteRequest_HeaderText;
                        break;
                    case EventSubType.InvoiceDeleted_SuperAdmin:
                        response.Subject = Resource.InvoiceDeleted_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.InvoiceDeleted_BodyLogo);
                        response.BodyText = Resource.InvoiceDeleted_BodyText;
                        response.CompanyText = Resource.InvoiceDeleted_HeaderText;
                        break;

                    case EventSubType.DropTicketCreatedAsInvoiceIsWaitingForUpdatedPrice_Supplier:
                    case EventSubType.DropTicketCreatedAsInvoiceIsWaitingForUpdatedPrice_Buyer:
                        response.Subject = Resource.emailDropTicketCreatedAsInvoiceIsWaiting_Supplier_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.BodyLogo_HandShake);
                        response.BodyText = Resource.emailDropTicketCreatedAsInvoiceIsWaiting_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketApproved_Supplier_AprovalWorkflow_BodyButtonText;
                        break;
                    case EventSubType.ProgressReport:
                        response.Subject = Resource.emailProgressReport_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailProgressReport_BodyLogo);
                        response.BodyButtonText = Resource.emailProgressReport_BodyButtonText;
                        break;
                    case EventSubType.LinkUnassignedDdtToOrder_Buyer:
                        response.Subject = Resource.emailUnassignedDdtLinkedToOrder_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestRescheduled_Supplier_BodyLogo);
                        response.BodyText = Resource.emailAssignedDdtToOrderLink_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketCreated_Buyer_AprovalWorkflow_BodyButtonText;
                        break;
                    case EventSubType.LinkUnassignedDdtToOrder_Supplier:
                        response.Subject = Resource.emailUnassignedDdtLinkedToOrder_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestRescheduled_Supplier_BodyLogo);
                        response.BodyText = Resource.emailAssignedDdtToOrderLink_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketCreated_Buyer_AprovalWorkflow_BodyButtonText;
                        break;
                    case EventSubType.LinkUnassignedDdtToOrderInvoiceGenerate_Buyer:
                        response.Subject = Resource.emailUnassignedDdtLinkedToOrderInvoiceGenerate_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestRescheduled_Supplier_BodyLogo);
                        response.BodyText = Resource.emailUnassignedDdtToOrderLinkInvoice_Buyer_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketCreated_Buyer_AprovalWorkflow_BodyButtonText;
                        break;
                    case EventSubType.LinkUnassignedDdtToOrderInvoiceGenerate_Supplier:
                        response.Subject = Resource.emailUnassignedDdtLinkedToOrderInvoiceGenerate_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestRescheduled_Supplier_BodyLogo);
                        response.BodyText = Resource.emailUnassignedDdtToOrderLinkInvoiceGenerate_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketCreated_Buyer_AprovalWorkflow_BodyButtonText;
                        break;
                    case EventSubType.CreateUnassignedDdt_Supplier:
                        response.Subject = Resource.emailCreateUnassignedDdt_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestRescheduled_Supplier_BodyLogo);
                        response.BodyText = Resource.emailCreateUnassignedDdt_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailUnassignedDDTCreated_Supplier_BodyButtonText;
                        break;
                    case EventSubType.DriverNotOnboarded_Supplier:
                        response.Subject = Resource.emailDriverNotOnboarded_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.emailDeliveryRequestRescheduled_Supplier_BodyLogo);
                        response.BodyText = Resource.emailDriverNotOnboarded_BodyText;
                        break;
                    case EventSubType.FuelRequestToExpireWithExpirationDate_SuperAdmin:
                        response.Subject = Resource.emailFuelRequestToExpireWithExpirationDate_SuperAdmin_SubjectText;
                        response.BodyText = Resource.emailFuelRequestToExpireWithExpirationDate_SuperAdmin_BodyText;
                        response.BodyButtonText = Resource.emailFuelRequestToExpireWithExpirationDate_SuperAdmin_BodyButtonText;
                        break;
                    case EventSubType.FuelRequestToExpireWithOrderStartDate_SuperAdmin:
                        response.Subject = Resource.emailFuelRequestToExpireWithOrderStartDate_SuperAdmin_SubjectText;
                        response.BodyText = Resource.emailFuelRequestToExpireWithOrderStartDate_SuperAdmin_BodyText;
                        response.BodyButtonText = Resource.emailFuelRequestToExpireWithOrderStartDate_SuperAdmin_BodyButtonText;
                        break;
                    case EventSubType.NewIncomingFuelRequest_SuperAdmin:
                        response.Subject = Resource.emailNewIncomingFuelRequest_SuperAdmin_SubjectText;
                        response.BodyText = Resource.emailNewIncomingFuelRequest_SuperAdmin_BodyText;
                        response.BodyButtonText = Resource.emailNewIncomingFuelRequest_SuperAdmin_BodyButtonText;
                        break;
                    case EventSubType.QuotationAwarded_Supplier:
                        response.Subject = Resource.emailQuotationAwarded_Supplier_SubjectText;
                        response.BodyText = Resource.emailQuotationAwarded_Supplier_BodyText;
                        response.BodyButtonText = Resource.emailQuotationAwarded_Supplier_BodyButtonText;
                        break;
                    case EventSubType.QuotationNotAwarded_Supplier:
                        response.Subject = Resource.emailQuotationNotAwarded_Supplier_SubjectText;
                        response.BodyText = Resource.emailQuotationNotAwarded_Supplier_BodyText;
                        break;
                    case EventSubType.DDTCreatedAsInvoiceIsWaitingForTaxes:
                        response.Subject = Resource.emailDDTCreatedAsInvoiceIsWaitingForTaxes_SubjectText;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.BodyLogo_HandShake);
                        response.BodyText = Resource.emailDDTCreatedAsInvoiceIsWaitingForTaxes_BodyText;
                        response.BodyButtonText = Resource.emailDropTicketCreated_SupplierCompanyAdmin_AprovalWorkflow_BodyButtonText;
                        break;
                    case EventSubType.InvoiceGeneratedEstablishConnectionWithAvalara:
                        response.Subject = Resource.emailInvoiceGeneratedWithAvalara_Subject;
                        response.BodyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.BodyLogo_HandShake);
                        response.BodyText = Resource.emailInvoiceGeneratedWithAvalara_BodyText;
                        response.BodyButtonText = Resource.emailInvoiceGeneratedWithAvalara_BodyButtonText;
                        break;
                    case EventSubType.QuickBooksSyncReport:
                        response.Subject = Resource.email_QbSyncReport_SubjectText;
                        response.BodyText = Resource.email_QbSyncReport_BodyText;
                        response.BodyButtonText = Resource.email_QbSyncReport_BodyButtonText;
                        break;
                    case EventSubType.DtnFileUploaded:
                        response.Subject = Resource.email_DtnFileUploaded_SubjectText;
                        response.BodyText = Resource.email_DtnFileUploaded_BodyText;
                        break;
                    case EventSubType.TelFuelException:
                        response.Subject = Resource.email_TelaFuelException_SubjectText;
                        response.BodyText = Resource.email_TelaFuelException_BodyText;
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetNotificationContent", "Exception Details : ", ex);
            }
            return response;
        }

        public async Task<bool> AddNotificationEventAsync(EventType eventTypeId, int entityId, int triggeredBy, List<int> companyIdList = null, string jsonMessage = null, int applicationTemplateId = 1, bool isManualTrigger = false)
        {
            try
            {
                var eventType = Context.DataContext.MstEventTypes.FirstOrDefault(t => t.Id == (int)eventTypeId);
                var notification = new Notification
                {
                    EventTypeId = (int)eventTypeId,
                    EntityId = entityId,
                    TriggeredBy = triggeredBy,
                    CreatedDate = DateTimeOffset.Now,
                    JsonMessage = jsonMessage,
                    NotificationType = eventType.NotificationType,
                    ApplicationTemplateId = applicationTemplateId,
                    IsManualTrigger = isManualTrigger
                };

                Context.DataContext.Notifications.Add(notification);
                await Context.CommitAsync();

                if (companyIdList != null && companyIdList.Count > 0)
                {
                    notification.Companies = Context.DataContext.Companies.Where(t => companyIdList.Contains(t.Id)).ToList();
                    Context.DataContext.Entry(notification).State = EntityState.Modified;
                    await Context.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "AddNotificationEventAsync", "Exception Details : ", ex);
            }
            return true;
        }

        public void AddNotificationEvent(EventType eventType, int entityId, int triggeredBy, List<int> companyIdList = null, int applicationTemplateId = 1)
        {
            try
            {
                var notification = new Notification
                {
                    EventTypeId = (int)eventType,
                    EntityId = entityId,
                    TriggeredBy = triggeredBy,
                    ApplicationTemplateId = applicationTemplateId
                };
                Context.DataContext.Notifications.Add(notification);

                if (companyIdList != null && companyIdList.Count > 0)
                {
                    notification.Companies = Context.DataContext.Companies.Where(t => companyIdList.Contains(t.Id)).ToList();
                    Context.DataContext.Entry(notification).State = EntityState.Modified;
                }
                Context.Commit();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "AddNotificationEvent", "Exception Details : ", ex);
            }
        }

        public List<NotificationEventViewModel> GetPendingNotificationEvents()
        {
            var response = new List<NotificationEventViewModel>();
            try
            {
                Context.DataContext
                              .Notifications
                              .Where(t =>
                                  (!t.IsEmailNotificationSent && (t.NotificationType == (int)NotificationType.Email ||
                                      t.NotificationType == (int)NotificationType.EmailAndSms))
                                      ||
                                      (!t.IsSmsNotificationSent && (t.NotificationType == (int)NotificationType.Sms ||
                                      t.NotificationType == (int)NotificationType.EmailAndSms))
                                  )
                              .ToList()
                              .ForEach(t => response.Add(t.ToViewModel()));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetPendingNotificationEvents", "Exception Details : ", ex);
            }
            return response;
        }

        public void UpdatePendingNotificationEvent(int id, int notificationType, bool isNotificationSent, bool status)
        {
            try
            {
                var notification = Context.DataContext.Notifications.SingleOrDefault(t => t.Id == id);
                if (notification != null)
                {
                    if (notificationType == (int)NotificationType.Email || notificationType == (int)NotificationType.EmailAndSms)
                        notification.IsEmailNotificationSent = isNotificationSent;

                    if (notificationType == (int)NotificationType.Sms || notificationType == (int)NotificationType.EmailAndSms)
                        notification.IsSmsNotificationSent = isNotificationSent;

                    if (status)
                        notification.Status = (int)Status.Success;
                    else
                        notification.Status = (int)Status.Failed;
                    Context.DataContext.Entry(notification).State = EntityState.Modified;
                    Context.Commit();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "UpdatePendingNotificationEvent", "Exception Details : ", ex);
            }
        }

        public InvitedUserNotificationViewModel GetUserInvite(int id)
        {
            var response = new InvitedUserNotificationViewModel();
            try
            {
                var addedInvite = Context.DataContext.UserXInvites.Where(t => t.Id == id)
                                    .Select(t => new
                                    {
                                        t.Id,
                                        t.FirstName,
                                        t.LastName,
                                        t.Email,
                                        t.Message,
                                        User = new
                                        {
                                            t.User.FirstName,
                                            t.User.LastName,
                                            t.User.CompanyId,
                                            CompanyName = t.User.Company.Name
                                        }
                                    }).SingleOrDefault();
                if (addedInvite != null)
                {
                    response = new InvitedUserNotificationViewModel()
                    {
                        User = new NotificationUserViewModel
                        {
                            Id = addedInvite.Id,
                            FirstName = addedInvite.FirstName,
                            LastName = addedInvite.LastName,
                            Email = addedInvite.Email,
                        },
                        InvitedByName = $"{addedInvite.User.FirstName} {addedInvite.User.LastName}",
                        InvitedCompanyId = addedInvite.User.CompanyId ?? 0,
                        InvitedCompanyName = addedInvite.User.CompanyName,
                        PersonalMessage = addedInvite.Message,
                    };
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetUserInvite", "Exception Details : ", ex);
            }
            return response;
        }

        public InvitedUserNotificationViewModel GetNotificationDetailsById(int id)
        {
            var response = new InvitedUserNotificationViewModel();
            try
            {
                var notification = Context.DataContext.Notifications.Where(t => t.Id == id)
                                    .Select(t => new
                                    {
                                        t.EntityId,
                                        t.JsonMessage,
                                        User = new
                                        {
                                            t.User.FirstName,
                                            t.User.LastName,
                                            t.User.CompanyId,
                                            t.User.Company.SupplierCode,
                                            CompanyName = t.User.Company.Name
                                        }
                                    }).SingleOrDefault();
                if (notification != null)
                {
                    response = new InvitedUserNotificationViewModel();

                    var user = Context.DataContext.Users.Where(t => t.Id == notification.EntityId)
                                .Select(t => new
                                {
                                    t.Id,
                                    t.FirstName,
                                    t.LastName,
                                    t.Email,
                                    t.CompanyId
                                }).SingleOrDefault();
                    if (user != null)
                    {
                        var TempPassword = string.Empty;
                        if (!string.IsNullOrEmpty(notification.JsonMessage))
                        {
                            TempPassword = CryptoHelperMethods.DecryptPassword(Constants.Key.ToString(), notification.JsonMessage.ToString());
                        }

                        response.User = new NotificationUserViewModel();
                        response.User.Id = user.Id;
                        response.User.FirstName = user.FirstName;
                        response.User.LastName = user.LastName;
                        response.User.Email = user.Email;
                        if (!string.IsNullOrEmpty(TempPassword))
                        {
                            response.User.Password = TempPassword;
                        }
                        else
                        {
                            response.User.Password = Constants.DefaultPassword;
                        }
                        response.User.CompanyId = user.CompanyId ?? 0;
                    }

                    response.InvitedByName = $"{notification.User.FirstName} {notification.User.LastName}";
                    response.InvitedCompanyId = notification.User.CompanyId ?? 0;
                    response.InvitedCompanyName = notification.User.CompanyName;
                    response.SupplierCode = notification.User.SupplierCode;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetUserInviteForTPO", "Exception Details : ", ex);
            }
            return response;
        }

        public void UpdateUserInviteStatus(int id, bool isNotificationSent)
        {
            try
            {
                var addedUser = Context.DataContext.UserXInvites.SingleOrDefault(t => t.Id == id);
                if (addedUser != null)
                {
                    addedUser.IsInvitationSent = isNotificationSent;

                    Context.DataContext.Entry(addedUser).State = EntityState.Modified;
                    Context.Commit();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "UpdateUserInviteStatus", "Exception Details : ", ex);
            }
        }

        public void UpdateInvitationLinkSentStatus(int buyerCompanyId, int supplierCompanyId)
        {
            try
            {
                var buyerCompany = Context.DataContext.Companies.SingleOrDefault(t => t.Id == buyerCompanyId);
                var supplierCompany = Context.DataContext.Companies.SingleOrDefault(t => t.Id == supplierCompanyId);
                if (!buyerCompany.Companies1.Any(t => t.Id == supplierCompanyId))
                {
                    buyerCompany.Companies1.Add(supplierCompany);
                    Context.Commit();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "UpdateInvitationLinkSentStatus", "Exception Details : ", ex);
            }
        }

        public InvitedUserNotificationViewModel GetAdditionalUser(int id)
        {
            var response = new InvitedUserNotificationViewModel();
            try
            {
                var addedUser = Context.DataContext.CompanyXAdditionalUserInvites.Where(t => t.Id == id)
                                .Select(t => new
                                {
                                    t.Id,
                                    t.FirstName,
                                    t.LastName,
                                    t.Email,
                                    User = new
                                    {
                                        t.User.FirstName,
                                        t.User.LastName,
                                        t.User.CompanyId,
                                        CompanyName = t.User.Company.Name
                                    },
                                    Roles = t.MstRoles.Select(t1 => t1.Name)
                                }).SingleOrDefault();
                if (addedUser != null)
                {
                    response = new InvitedUserNotificationViewModel()
                    {
                        User = new NotificationUserViewModel
                        {
                            Id = addedUser.Id,
                            FirstName = addedUser.FirstName,
                            LastName = addedUser.LastName,
                            Email = addedUser.Email,
                        },
                        InvitedByName = $"{addedUser.User.FirstName} {addedUser.User.LastName}",
                        InvitedCompanyId = addedUser.User.CompanyId.Value,
                        InvitedCompanyName = addedUser.User.CompanyName,
                        Roles = addedUser.Roles.ToList(),
                        PersonalMessage = string.Empty,
                    };
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetAdditionalUser", "Exception Details : ", ex);
            }
            return response;
        }

        public List<NotificationUserViewModel> GetEmailSubscribedCompanyAdmins(int companyId, EventType eventType)
        {
            var response = new List<NotificationUserViewModel>();
            try
            {
                response = Context.DataContext.Users.Where(t => t.IsActive &&
                            t.CompanyId == companyId && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.Admin) &&
                            t.UserXNotificationSettings.Any(t1 => t1.EventTypeId == (int)eventType && (t1.IsEmail || t1.IsSMS)))
                            .Select(t => new NotificationUserViewModel
                            {
                                Id = t.Id,
                                FirstName = t.FirstName,
                                LastName = t.LastName,
                                Email = t.Email
                            }).ToList();

                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetEmailSubscribedCompanyAdmins", "Exception Details : ", ex);
            }
            return response;
        }

        //As per invoice notification preferences in tpo in case of ftl order
        public List<NotificationUserViewModel> GetEmailSubscribedTpoBuyerAdmins(int companyId, EventType eventType)
        {
            var response = new List<NotificationUserViewModel>();
            try
            {
                response = Context.DataContext.Users.Where(t => t.CompanyId == companyId &&
                            t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.Admin) &&
                            t.UserXNotificationSettings.Any(t1 => t1.EventTypeId == (int)eventType && t1.IsEmail))
                            .Select(t => new NotificationUserViewModel()
                            {
                                Id = t.Id,
                                FirstName = t.FirstName,
                                LastName = t.LastName,
                                Email = t.Email
                            }).ToList();
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetEmailSubscribedTpoBuyerAdmins", "Exception Details : ", ex);
            }
            return response;
        }

        public NotificationJobViewModel GetJobNotificationDetails(int id)
        {
            var response = new NotificationJobViewModel();
            try
            {
                var jobDetails = Context.DataContext.Jobs.Where(t => t.Id == id)
                                .Select(t => new
                                {
                                    t.Id,
                                    t.Name,
                                    t.City,
                                    StateName = t.MstState.Name,
                                    t.ZipCode,
                                    t.CreatedBy,
                                    t.IsApprovalWorkflowEnabled,
                                    t.IsProFormaPoEnabled,
                                    t.InventoryDataCaptureType,
                                    Company = new
                                    {
                                        t.Company.Id,
                                        t.Company.Name
                                    },
                                    User = new
                                    {
                                        t.User.FirstName,
                                        t.User.LastName,
                                        t.User.Email
                                    },
                                    OnsitePersons = t.Users1.Select(t1 => new
                                    {
                                        t1.Id,
                                        t1.FirstName,
                                        t1.LastName,
                                        t1.Email,
                                        t1.IsActive
                                    }),
                                    AssignedTo = t.Users.Select(t1 => new
                                    {
                                        t1.Id,
                                        t1.FirstName,
                                        t1.LastName,
                                        t1.Email,
                                        t1.IsActive
                                    }),
                                    JobXApprovalUsers = t.JobXApprovalUsers.Select(t1 => new
                                    {
                                        t1.Id,
                                        t1.IsActive,
                                        t1.UserId,
                                        t1.User.FirstName,
                                        t1.User.LastName,
                                        t1.User.Email
                                    })
                                }).SingleOrDefault();
                if (jobDetails != null)
                {
                    response = new NotificationJobViewModel
                    {
                        Id = jobDetails.Id,
                        Name = jobDetails.Name,
                        Location = jobDetails.City + jobDetails.StateName + jobDetails.ZipCode,

                        CompanyId = jobDetails.Company.Id,
                        CompanyName = jobDetails.Company.Name
                    };

                    response.Creator.Id = jobDetails.CreatedBy;
                    response.Creator.Email = jobDetails.User.Email;
                    response.Creator.FirstName = jobDetails.User.FirstName;
                    response.Creator.LastName = jobDetails.User.LastName;

                    response.OnsitePersons = jobDetails.OnsitePersons.Where(t => t.IsActive).Select(t => new NotificationUserViewModel()
                    {
                        Email = t.Email,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        Id = t.Id
                    }).ToList();

                    response.AssignedTo = jobDetails.AssignedTo.Where(t => t.IsActive).Select(t => new NotificationUserViewModel()
                    {
                        Email = t.Email,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        Id = t.Id
                    }).ToList();

                    response.IsApprovalWorkflowEnabled = jobDetails.IsApprovalWorkflowEnabled;
                    response.IsProFormaPoEnabled = jobDetails.IsProFormaPoEnabled;
                    response.InventoryDataCaptureType = jobDetails.InventoryDataCaptureType;
                    if (jobDetails.JobXApprovalUsers.Any(t => t.IsActive))
                    {
                        var approver = jobDetails.JobXApprovalUsers.FirstOrDefault(t => t.IsActive);
                        response.ApproverUser.Id = approver.UserId;
                        response.ApproverUser.Email = approver.Email;
                        response.ApproverUser.FirstName = approver.FirstName;
                        response.ApproverUser.LastName = approver.LastName;
                    }

                    var previousUser = jobDetails.JobXApprovalUsers.OrderByDescending(t => t.Id).FirstOrDefault(t => !t.IsActive);
                    if (previousUser != null)
                    {
                        response.PreviousApprover.Id = previousUser.UserId;
                        response.PreviousApprover.Email = previousUser.Email;
                        response.PreviousApprover.FirstName = previousUser.FirstName;
                        response.PreviousApprover.LastName = previousUser.LastName;
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetJobNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        public NotificationJobViewModel GetJobAssignmentNotificationDetails(NotificationEventViewModel viewModel)
        {
            var response = new NotificationJobViewModel();
            try
            {
                var jobDetails = Context.DataContext.Jobs.Where(t => t.Id == viewModel.EntityId)
                                    .Select(t => new { t.Id, t.Name, t.UpdatedBy }).SingleOrDefault();
                if (jobDetails != null)
                {
                    response = new NotificationJobViewModel
                    {
                        Id = jobDetails.Id,
                        Name = jobDetails.Name
                    };
                    var user = Context.DataContext.Users.Where(t => t.Id == viewModel.TriggeredByUserId)
                                .Select(t => new { t.Id, t.FirstName, t.LastName, t.Email }).SingleOrDefault();
                    if (user != null)
                    {
                        response.Creator.Id = user.Id;
                        response.Creator.Email = user.Email;
                        response.Creator.FirstName = user.FirstName;
                        response.Creator.LastName = user.LastName;
                    }
                    var sender = Context.DataContext.Users.Where(t => t.Id == jobDetails.UpdatedBy)
                                    .Select(t => new { t.FirstName, t.LastName }).SingleOrDefault();
                    response.CompanyName = $"{sender.FirstName} {sender.LastName}";
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetJobAssignmentNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        public List<NotificationUserViewModel> GetBuyerDefaultRecievers(int buyerCompanyId, EventType eventType)
        {
            var users = (from a in Context.DataContext.MstCompanyUserRoleXEventTypes
                         from b in Context.DataContext.Users
                         where a.IsDefault && b.CompanyId == buyerCompanyId && a.EventTypeId == (int)eventType && b.MstRoles.Select(x => x.Id).Contains(a.RoleId)
                         select new NotificationUserViewModel
                         {
                             CompanyId = b.CompanyId.Value,
                             Id = b.Id,
                             Email = b.Email,
                             FirstName = b.FirstName,
                             LastName = b.LastName,
                         }).ToList();
            return users;
        }

        public void UpdateAdditionalUserStatus(int id, bool isNotificationSent)
        {
            try
            {
                var addedUser = Context.DataContext.CompanyXAdditionalUserInvites.SingleOrDefault(t => t.Id == id);
                if (addedUser != null)
                {
                    addedUser.IsInvitationSent = isNotificationSent;

                    Context.DataContext.Entry(addedUser).State = EntityState.Modified;
                    Context.Commit();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "UpdateAdditionalUserStatus", "Exception Details : ", ex);
            }
        }

        public InvitedUserNotificationViewModel GetOnboardedUser(int onboardedUserId, int inviterId)
        {
            var response = new InvitedUserNotificationViewModel();
            try
            {
                var onboardedUser = Context.DataContext.Users.Where(t => t.Id == onboardedUserId)
                                    .Select(t => new
                                    {
                                        t.Id,
                                        t.FirstName,
                                        t.LastName,
                                        t.Email,
                                        Roles = t.MstRoles.Select(t1 => t1.Name),
                                        Company = new
                                        {
                                            t.Company.Id,
                                            t.Company.Name
                                        }
                                    }).SingleOrDefault();
                if (onboardedUser != null)
                {
                    response = new InvitedUserNotificationViewModel()
                    {
                        User = new NotificationUserViewModel
                        {
                            Id = onboardedUser.Id,
                            FirstName = onboardedUser.FirstName,
                            LastName = onboardedUser.LastName,
                            Email = onboardedUser.Email,
                        },
                        Roles = onboardedUser.Roles.ToList(),
                        InvitedByName = string.Empty,
                        InvitedCompanyId = onboardedUser.Company.Id,
                        InvitedCompanyName = onboardedUser.Company.Name,
                        PersonalMessage = string.Empty,
                    };

                    var inviter = Context.DataContext.Users.Where(t => t.Id == inviterId)
                                    .Select(t => new
                                    {
                                        t.FirstName,
                                        t.LastName,
                                        Company = new
                                        {
                                            t.Company.Id,
                                            t.Company.Name
                                        }
                                    }).SingleOrDefault();
                    if (inviter != null)
                    {
                        response.InvitedByName = $"{inviter.FirstName} {inviter.LastName}";
                        response.InvitedCompanyId = inviter.Company.Id;
                        response.InvitedCompanyName = inviter.Company.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetOnboardedUser", "Exception Details : ", ex);
            }
            return response;
        }

        public NotificationRequestFuelViewModel GetNeedFuelIntimationDetails(int id)
        {
            var response = new NotificationRequestFuelViewModel();
            var helperDomain = new HelperDomain(this);
            try
            {
                var requestFuel = Context.DataContext.RequestFuels.Include(t => t.RequestPrice).SingleOrDefault(t => t.Id == id);
                if (requestFuel != null)
                {
                    var salesTeamEmails = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingSalesMailingList);
                    response = new NotificationRequestFuelViewModel
                    {
                        Id = requestFuel.Id,
                        CustomerName = $"{requestFuel.FirstName} {requestFuel.LastName}",
                        CompanyName = string.IsNullOrWhiteSpace(requestFuel.CompanyName) ? "NA" : requestFuel.CompanyName,
                        PhoneNumber = requestFuel.PhoneNumber,
                        Email = requestFuel.Email,
                        FuelType = helperDomain.GetProductName(requestFuel.RequestPrice.MstProduct),
                        Quantity = requestFuel.RequestPrice.Quantity.GetCommaSeperatedValue(),
                        PricePerGallon = requestFuel.RequestPrice.PricePerGallon.GetCommaSeperatedValue(),
                        Zipcode = requestFuel.RequestPrice.ZipCode,
                        EmailRecipients = salesTeamEmails.Split(';').ToList(),
                        Currency = requestFuel.RequestPrice.Currency,
                        UoM = requestFuel.RequestPrice.UoM
                    };
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetNeedFuelIntimationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        public void UpdateNeedFuelIntimationDetails(int id, bool isSent)
        {
            try
            {
                var requestFuel = Context.DataContext.RequestFuels.SingleOrDefault(t => t.Id == id);
                if (requestFuel != null)
                {
                    if (isSent)
                    {
                        requestFuel.IsEmailSentToSales = true;
                        requestFuel.EmailSentDateTime = DateTimeOffset.Now;
                    }
                    else
                    {
                        requestFuel.IsEmailSentToSales = false;
                        requestFuel.EmailSentDateTime = null;
                    }

                    Context.DataContext.Entry(requestFuel).State = EntityState.Modified;
                    Context.Commit();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "UpdateNeedFuelIntimationDetails", "Exception Details : ", ex);
            }
        }

        public string GetDisplayInvoiceNumberById(int invoiceId, int invoiceHeaderId = 0)
        {
            string displayInvoiceNumber = invoiceId.ToString();
            string invoiceNumber = null;
            try
            {
                if (invoiceHeaderId > 0)
                {
                    invoiceNumber = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId).OrderByDescending(t => t.Id).Select(t => t.DisplayInvoiceNumber).FirstOrDefault();
                }
                else
                {
                    invoiceNumber = Context.DataContext.Invoices.Where(t => t.Id == invoiceId).Select(t => t.DisplayInvoiceNumber).FirstOrDefault();
                }

                if (invoiceNumber != null)
                {
                    displayInvoiceNumber = invoiceNumber;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetDisplayInvoiceNumberById", "Exception Details : ", ex);
            }

            return displayInvoiceNumber;
        }

        public async Task<List<ImageViewModel>> GetInvoiceImagesById(int invoiceId)
        {
            List<ImageViewModel> response = new List<ImageViewModel>();
            try
            {
                var invoiceDomain = ContextFactory.Current.GetDomain<InvoiceDomain>();
                var invoice = Context.DataContext.Invoices.SingleOrDefault(t => t.Id == invoiceId);
                if (invoice != null)
                {
                    // get drop reading and asset drop invoice images
                    response = await invoiceDomain.GetInvoiceImagesAsync(invoiceId);

                    // get BOL image
                    var bolImgId = invoice.InvoiceXBolDetails.Any(t => t.InvoiceFtlDetail != null && t.InvoiceFtlDetail.ImageId != null && t.InvoiceFtlDetail.ImageId > 0)
                                    ? invoice.InvoiceXBolDetails.Where(t => t.InvoiceFtlDetail != null && t.InvoiceFtlDetail.ImageId != null && t.InvoiceFtlDetail.ImageId > 0).Select(t => t.InvoiceFtlDetail.ImageId).FirstOrDefault() : 0;
                    var bolImg = await Context.DataContext.Images.SingleOrDefaultAsync(t => t.Id == bolImgId);
                    if (bolImg != null)
                    {
                        var bolImageModel = bolImg.ToViewModel();
                        bolImageModel.Name = "bol-" + invoice.DisplayInvoiceNumber;
                        response.Add(bolImageModel);
                    }

                    // get signature image
                    var signImgId = (invoice.Signaure != null && invoice.Signaure.ImageId != null && invoice.Signaure.ImageId.Value > 0)
                                    ? invoice.Signaure.ImageId.Value : 0;
                    var signImg = await Context.DataContext.Images.SingleOrDefaultAsync(t => t.Id == signImgId);
                    if (signImg != null)
                    {
                        var signImageModel = signImg.ToViewModel();
                        signImageModel.Name = "sign-" + invoice.DisplayInvoiceNumber;
                        response.Add(signImageModel);
                    }

                    // get additional images
                    var additionalImgId = (invoice.InvoiceXAdditionalDetail != null && invoice.InvoiceXAdditionalDetail.AdditionalImageId != null &&
                                           invoice.InvoiceXAdditionalDetail.AdditionalImageId.Value > 0) ? invoice.InvoiceXAdditionalDetail.AdditionalImageId.Value : 0;
                    var additionalImg = await Context.DataContext.Images.SingleOrDefaultAsync(t => t.Id == additionalImgId);
                    if (additionalImg != null)
                    {
                        var additionalImageModel = additionalImg.ToViewModel();
                        additionalImageModel.Name = "additional-" + invoice.DisplayInvoiceNumber;
                        response.Add(additionalImageModel);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetInvoiceImagesById", "Exception Details : ", ex);
            }

            return response;
        }

        #region Counter Offer Staus Notifications

        public NotificationCounterOfferCreatedViewModel GetCounterOfferNotificationDetails(int id, EventType eventType)
        {
            var response = new NotificationCounterOfferCreatedViewModel();
            try
            {
                var originalFuelReuqestId = _helperDomain.GetFuelRequestFromCounterOffer(id);
                var originalFuelRequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == originalFuelReuqestId);
                var counterOffer = Context.DataContext.CounterOffers.SingleOrDefault(t => t.FuelRequestId == id);

                if (counterOffer != null)
                {
                    response = new NotificationCounterOfferCreatedViewModel()
                    {
                        Id = originalFuelRequest.Id,
                        FuelRequestNumber = originalFuelRequest.RequestNumber
                    };
                    response.Buyer = new NotificationUserViewModel()
                    {
                        Id = counterOffer.User.Id,
                        Email = counterOffer.User.Email,
                        FirstName = counterOffer.User.FirstName,
                        LastName = counterOffer.User.LastName
                    };
                    response.Supplier = new NotificationUserViewModel()
                    {
                        Id = counterOffer.User1.Id,
                        Email = counterOffer.User1.Email,
                        FirstName = counterOffer.User1.FirstName,
                        LastName = counterOffer.User1.LastName
                    };
                    if (counterOffer.BuyerId == counterOffer.FuelRequest.CreatedBy)
                    {
                        // this CO was created by buyer and is sent to the supplier
                        response.CreatorRole = UserRoles.Buyer;
                    }
                    else
                    {
                        // this CO was created by supplier and is sent to the buyer
                        response.CreatorRole = UserRoles.Supplier;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetCounterOfferNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        #endregion

        #region FuelRequest Staus Notifications
        public NotificationFuelRequestCreatedViewModel GetFuelRequestNotificationDetails(int id, EventType eventType)
        {
            var response = new NotificationFuelRequestCreatedViewModel();
            try
            {
                var fuelrequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == id
                                                    && t.FuelRequestTypeId != (int)FuelRequestType.CounteredFuelRequest);
                if (fuelrequest != null)
                {
                    var job = fuelrequest.Job;
                    response = new NotificationFuelRequestCreatedViewModel()
                    {
                        CompanyId = fuelrequest.User.Company.Id,
                        CompanyName = fuelrequest.User.Company.Name,
                        JobName = job.Name,
                        JobId = job.Id,
                        IsMarineLocation = job.IsMarine,
                        Id = fuelrequest.Id,
                        TypeId = fuelrequest.FuelRequestTypeId,
                        FuelRequestNumber = fuelrequest.RequestNumber,
                        ExpirationDate = fuelrequest.ExpirationDate.HasValue ? fuelrequest.ExpirationDate.Value.ToString(Resource.constFormatDate) : fuelrequest.FuelRequestDetail.StartDate.ToString(Resource.constFormatDate),
                        DeliveryStartDate = fuelrequest.FuelRequestDetail.StartDate.ToString(Resource.constFormatDate),
                        DeliveryStartTime = Convert.ToDateTime(fuelrequest.FuelRequestDetail.StartTime.ToString()).ToShortTimeString(),
                        ExternalPoNumber = fuelrequest.ExternalPoNumber,
                        Suppliers = new List<NotificationUserViewModel>()
                    };
                    if (eventType == EventType.FuelRequestUpdated || eventType == EventType.BrokerFuelRequestUpdated)
                    {
                        var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == fuelrequest.UpdatedBy);
                        response.Creator = new NotificationUserViewModel()
                        {
                            Id = user.Id,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName
                        };
                    }
                    else
                    {
                        response.Creator = new NotificationUserViewModel()
                        {
                            Id = fuelrequest.User.Id,
                            Email = fuelrequest.User.Email,
                            FirstName = fuelrequest.User.FirstName,
                            LastName = fuelrequest.User.LastName
                        };
                    }
                    var companies = GetEligibleCompaniesForFuelRequest(fuelrequest);
                    companies.ForEach(t => t.Users.Where(t2 => t2.IsActive && t2.MstRoles.Any(t3 => t3.Id == (int)UserRoles.Supplier || t3.Id == (int)UserRoles.Admin)).ToList().ForEach(
                                            t1 => response.Suppliers.Add(new NotificationUserViewModel()
                                            {
                                                Email = t1.Email,
                                                FirstName = t1.FirstName,
                                                LastName = t1.LastName,
                                                Id = t1.Id,
                                            }))
                                      );
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetFuelRequestNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        private List<Company> GetEligibleCompaniesForFuelRequest(FuelRequest fuelRequest)
        {
            var response = new List<Company>();
            var jobDetails = fuelRequest.Job;

            if (jobDetails != null)
            {
                var settingsDomain = new SettingsDomain(this);
                var blacklistedCompanyIds = Task.Run(() => settingsDomain.GetBlacklistedCompanyIdsAsync(fuelRequest.User.Company.Id)).Result;

                response = Context.DataContext.Companies.Include(t => t.CompanyAddresses).Where
                (
                    t => !blacklistedCompanyIds.Contains(t.Id) &&
                    t.MstCompanyType.Id != (int)CompanyType.Buyer &&
                    t.Id != jobDetails.Company.Id &&
                    t.Id != fuelRequest.User.Company.Id &&
                    t.IsActive &&
                    t.CompanyAddresses.Any
                    (
                        t1 => t1.IsActive &&
                        (
                            t1.SupplierAddressXSetting.IsHedgeOrderAllowed ||
                            (!t1.SupplierAddressXSetting.IsHedgeOrderAllowed && fuelRequest.OrderTypeId != (int)OrderType.Hedge)
                        ) &&
                        t1.MstProductTypes.Any(t2 => t2.Id == fuelRequest.MstProduct.ProductTypeId) &&
                        t1.MstStates.Any(t3 => t3.Id == jobDetails.StateId)
                    )
                ).ToList();

                response = response.Where
                (
                    t => t.CompanyAddresses.Any
                    (
                        t1 => t1.SupplierAddressXSetting != null &&
                              (t1.SupplierAddressXSetting.IsStateWideService ||
                              (
                                !t1.SupplierAddressXSetting.IsStateWideService &&
                                _helperDomain.CalculateDistance(t1.Latitude, t1.Longitude, jobDetails.Latitude, jobDetails.Longitude) <= t1.SupplierAddressXSetting.Radius
                              )) &&
                              !fuelRequest.MstSupplierQualifications.Except(t1.MstSupplierQualifications).Any() &&
                              (fuelRequest.IsPublicRequest || fuelRequest.PrivateSupplierLists.Any(t2 => t2.Companies.Contains(t)))
                    )
                ).ToList();

                if (fuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest)
                {
                    response = response.Where(t => !fuelRequest.GetBrokerChainCompanyIdList().Contains(t.Id)).ToList();

                    if ((fuelRequest.MstProduct.ProductTypeId == (int)ProductTypes.RedDyeDiesel || fuelRequest.MstProduct.ProductTypeId == (int)ProductTypes.RedDyeDiesel2) && jobDetails.StateId == ApplicationConstants.TexasStateId)
                    {
                        response = response.Where(t => t.CompanyAddresses.Any(t1 => t1.StateId == ApplicationConstants.TexasStateId)).ToList();
                    }
                }
            }
            //Get notification sent companies for fuelrequest
            var notificationSentCompanyList = GetNotificationSentCompanies(fuelRequest);
            if (notificationSentCompanyList.Count > 0)
            {
                response = response.Except(notificationSentCompanyList).ToList();
            }

            response = response.Distinct().ToList();
            return response;
        }

        private List<Company> GetEligibleCompaniesForQuoteRequest(QuoteRequest quoteRequest)
        {
            var response = new List<Company>();
            var jobDetails = quoteRequest.Job;

            if (jobDetails != null)
            {
                var settingsDomain = ContextFactory.Current.GetDomain<SettingsDomain>();
                var blacklistedCompanyIds = Task.Run(() => settingsDomain.GetBlacklistedCompanyIdsAsync(quoteRequest.User.Company.Id)).Result;

                response = Context.DataContext.Companies.Include(t => t.CompanyAddresses).Where
                (
                    t => !blacklistedCompanyIds.Contains(t.Id) &&
                    t.MstCompanyType.Id != (int)CompanyType.Buyer &&
                    t.Id != jobDetails.Company.Id &&
                    t.Id != quoteRequest.User.Company.Id &&
                    t.IsActive &&
                    t.CompanyAddresses.Any
                    (
                        t1 => t1.IsActive &&
                        (
                            t1.SupplierAddressXSetting.IsHedgeOrderAllowed ||
                            (!t1.SupplierAddressXSetting.IsHedgeOrderAllowed && quoteRequest.OrderTypeId != (int)OrderType.Hedge)
                        ) &&
                        t1.MstProductTypes.Any(t2 => t2.Id == quoteRequest.MstProduct.ProductTypeId) &&
                        t1.MstStates.Any(t3 => t3.Id == jobDetails.StateId)
                    )
                ).ToList();

                response = response.Where
                (
                    t => t.CompanyAddresses.Any
                    (
                        t1 => t1.SupplierAddressXSetting != null &&
                              (t1.SupplierAddressXSetting.IsStateWideService ||
                              (
                                !t1.SupplierAddressXSetting.IsStateWideService &&
                                _helperDomain.CalculateDistance(t1.Latitude, t1.Longitude, jobDetails.Latitude, jobDetails.Longitude) <= t1.SupplierAddressXSetting.Radius
                              )) &&
                              !quoteRequest.MstSupplierQualifications.Except(t1.MstSupplierQualifications).Any() &&
                              (quoteRequest.IsPublicRequest || quoteRequest.PrivateSupplierLists.Any(t2 => t2.Companies.Contains(t)))
                    )
                ).ToList();
            }
            //Get notification sent companies for fuelrequest
            var notificationSentCompanyList = GetNotificationSentCompanies(quoteRequest);
            if (notificationSentCompanyList.Count > 0)
            {
                response = response.Except(notificationSentCompanyList).ToList();
            }

            response = response.Distinct().ToList();
            return response;
        }

        private List<Company> GetNotificationSentCompanies(FuelRequest fuelRequest)
        {
            var response = Context.DataContext.Notifications.Where(t => t.EntityId == fuelRequest.Id
                                                                        && (t.EventTypeId == (int)EventType.FuelRequestCreated
                                                                        || t.EventTypeId == (int)EventType.BrokerFuelRequestCreated
                                                                        || t.EventTypeId == (int)EventType.FuelRequestUpdated
                                                                        || t.EventTypeId == (int)EventType.BrokerFuelRequestUpdated)
                                                                        && t.IsEmailNotificationSent)
                                                            .SelectMany(t => t.Companies)
                                                            .ToList();
            return response;
        }

        private List<Company> GetNotificationSentCompanies(QuoteRequest quoteRequest)
        {
            var response = Context.DataContext.Notifications.Where(t => t.EntityId == quoteRequest.Id
                                                                        && (t.EventTypeId == (int)EventType.QuoteRequestCreated)
                                                                        && t.IsEmailNotificationSent)
                                                            .SelectMany(t => t.Companies)
                                                            .ToList();
            return response;
        }

        public NotificationFuelRequestStatusViewModel GetFuelRequestStatusNotificationDetails(int entityId)
        {
            var response = new NotificationFuelRequestStatusViewModel();
            try
            {
                var fuelrequest = Context.DataContext.FuelRequests.SingleOrDefault(t => t.Id == entityId
                                                    && t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Accepted);
                if (fuelrequest != null && fuelrequest.Orders != null && fuelrequest.Orders.Count > 0)
                {
                    var order = fuelrequest.Orders.FirstOrDefault();
                    var job = fuelrequest.Job;
                    response = new NotificationFuelRequestStatusViewModel()
                    {
                        Creator = new NotificationUserViewModel()
                        {
                            Id = fuelrequest.User.Id,
                            Email = fuelrequest.User.Email,
                            FirstName = fuelrequest.User.FirstName,
                            LastName = fuelrequest.User.LastName
                        },
                        CompanyId = fuelrequest.User.Company.Id,
                        CompanyName = fuelrequest.User.Company.Name,
                        JobName = job.Name,
                        JobId = job.Id,
                        Id = fuelrequest.Id,
                        TypeId = fuelrequest.FuelRequestTypeId,
                        FuelRequestNumber = fuelrequest.RequestNumber,
                        Supplier = new NotificationUserViewModel()
                        {
                            Id = order.User.Id,
                            Email = order.User.Email,
                            FirstName = order.User.FirstName,
                            LastName = order.User.LastName,
                        },
                        SupplierCompanyId = order.User.Company.Id,
                        SupplierCompanyName = order.User.Company.Name,
                        OrderNumber = order.PoNumber,
                        OrderId = order.Id

                    };
                    var pdfSetting = Context.DataContext.Companies.Where(t => t.Id == response.CompanyId || t.Id == response.SupplierCompanyId).Select(t => new { t.Id, t.EnableOrderPDF }).ToList();
                    response.SendOrderAttachmentToBuyer = pdfSetting.Any(t => t.EnableOrderPDF && t.Id == response.CompanyId);
                    response.SendOrderAttachmentToSupplier = pdfSetting.Any(t => t.EnableOrderPDF && t.Id == response.SupplierCompanyId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetFuelRequestStatusNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }
        #endregion

        #region Order Status Notifications
        public NotificationOrderViewModel GetOrderNotificationDetails(int id, EventType eventType)
        {
            var response = new NotificationOrderViewModel();
            try
            {
                var orderDetails = Context.DataContext.Orders.SingleOrDefault(t => t.Id == id);
                if (orderDetails != null)
                {
                    var updatedByUser = Context.DataContext.Users.SingleOrDefault(t => t.Id == orderDetails.UpdatedBy);
                    var buyerUser = orderDetails.FuelRequest.User;
                    var buyerCompany = orderDetails.FuelRequest.User.Company;
                    var supplierUser = orderDetails.User;
                    var supplierCompany = orderDetails.User.Company;

                    if (updatedByUser != null)
                    {
                        var pdfSetting = Context.DataContext.Companies.Where(t => t.Id == buyerCompany.Id || t.Id == supplierCompany.Id).Select(t => new { t.Id, t.EnableInvoicePDF, t.EnableOrderPDF }).ToList();

                        response = new NotificationOrderViewModel()
                        {
                            Id = orderDetails.Id,
                            BuyerCompanyId = buyerCompany.Id,
                            BuyerCompanyName = buyerCompany.Name,
                            BuyerUser = new NotificationUserViewModel()
                            {
                                Id = buyerUser.Id,
                                Email = buyerUser.Email,
                                FirstName = buyerUser.FirstName,
                                LastName = buyerUser.LastName
                            },
                            SupplierCompanyId = supplierCompany.Id,
                            SupplierCompanyName = supplierCompany.Name,
                            SupplierUser = new NotificationUserViewModel()
                            {
                                Id = supplierUser.Id,
                                Email = supplierUser.Email,
                                FirstName = supplierUser.FirstName,
                                LastName = supplierUser.LastName
                            },
                            PoNumber = orderDetails.PoNumber,
                            IsUpdatedByBuyer = (buyerUser.Company.Id == updatedByUser.Company.Id),
                            SendInvoiceAttachmentToBuyer = pdfSetting.Any(t => t.EnableInvoicePDF && t.Id == buyerCompany.Id),
                            SendInvoiceAttachmentToSupplier = pdfSetting.Any(t => t.EnableInvoicePDF && t.Id == supplierCompany.Id),
                            SendOrderAttachmentToBuyer = pdfSetting.Any(t => t.EnableOrderPDF && t.Id == buyerCompany.Id),
                            SendOrderAttachmentToSupplier = pdfSetting.Any(t => t.EnableOrderPDF && t.Id == supplierCompany.Id)
                        };

                        response.IsBrokeredOrder = orderDetails.BuyerCompanyId != orderDetails.FuelRequest.Job.CompanyId;
                        response.UpdatedByUser = response.IsUpdatedByBuyer ? response.BuyerUser : response.SupplierUser;
                        response.IsProFormaPo = orderDetails.IsProFormaPo;
                        response.NewOrderVersionNumber = "V" + orderDetails.OrderDetailVersions.FirstOrDefault(t => t.IsActive).Version;
                        if (orderDetails.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest)
                            response.IsTpoOrder = true;
                        if (!response.IsUpdatedByBuyer && (eventType == EventType.OrderCancelled || eventType == EventType.OrderClosed))
                        {
                            response.IsOpenBrokerOrderExists = ContextFactory.Current.GetDomain<HelperDomain>().CheckForOpenBrokerOrder(orderDetails);
                        }
                        if (eventType == EventType.OrderClosedAndFuelRequestResubmitted || eventType == EventType.OrderCanceledAndFuelRequestResubmitted)
                        {
                            FuelRequest fuelRequest = ContextFactory.Current.GetDomain<HelperDomain>().GetFuelRequestConnectedWithBuyer(orderDetails);
                            response.BuyerCompanyId = fuelRequest.GetCompany().Id;
                            response.BuyerCompanyName = fuelRequest.GetCompany().Name;
                            response.BuyerUser = new NotificationUserViewModel()
                            {
                                Id = fuelRequest.User.Id,
                                Email = fuelRequest.User.Email,
                                FirstName = fuelRequest.User.FirstName,
                                LastName = fuelRequest.User.LastName
                            };
                        }
                        if (orderDetails.OrderXCancelationReason != null && orderDetails.OrderXCancelationReason.MstOrderCancelationReason != null)
                        {
                            response.CancellationReason = orderDetails.OrderXCancelationReason.MstOrderCancelationReason.Name + " - " + orderDetails.OrderXCancelationReason.AdditionalNotes;
                        }
                        response.PricingType = orderDetails.OrderAdditionalDetail.FuelSurchagePricingType.HasValue ? (FuelSurchagePricingType)orderDetails.OrderAdditionalDetail.FuelSurchagePricingType : FuelSurchagePricingType.Unknown;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetOrderNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        public NotificationOrderViewModel GetUpdatedOrderNotificationDetails(int id)
        {
            var response = new NotificationOrderViewModel();
            try
            {
                var orderDetails = Context.DataContext.Orders.SingleOrDefault(t => t.Id == id);
                if (orderDetails != null)
                {
                    response = new NotificationOrderViewModel()
                    {
                        Id = orderDetails.Id,
                        PoNumber = orderDetails.PoNumber,
                        IsUpdatedByBuyer = orderDetails.OrderXStatuses.FirstOrDefault(t => t.IsActive).StatusId == (int)OrderStatus.Open ? false : true
                    };
                    if (response.IsUpdatedByBuyer)
                    {
                        response.BuyerUser = new NotificationUserViewModel()
                        {
                            Id = orderDetails.FuelRequest.User.Id,
                            Email = orderDetails.FuelRequest.User.Email,
                            FirstName = orderDetails.FuelRequest.User.FirstName,
                            LastName = orderDetails.FuelRequest.User.LastName
                        };
                        if (orderDetails.Order1.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest)
                        {
                            response.IsOpenBrokerOrderExists = true;
                        }
                        response.Id = orderDetails.ParentId ?? 0;
                    }
                    else
                    {
                        response.BuyerUser = new NotificationUserViewModel()
                        {
                            Id = orderDetails.User.Id,
                            Email = orderDetails.User.Email,
                            FirstName = orderDetails.User.FirstName,
                            LastName = orderDetails.User.LastName
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetUpdatedOrderNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }
        #endregion

        public NotificationDispatchViewModel GetDispatchNotificationDetails(int id)
        {
            var response = new NotificationDispatchViewModel();
            try
            {
                var appLocationDetails = Context.DataContext.AppLocations.SingleOrDefault(t => t.Id == id);
                if (appLocationDetails != null && appLocationDetails.Order != null)
                {
                    var orderDetails = appLocationDetails.Order;
                    var fuelRequest = orderDetails.FuelRequest;
                    var buyerUser = (fuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? fuelRequest.CounterOffers.FirstOrDefault().User : fuelRequest.User);
                    var buyerCompany = buyerUser.Company;
                    var supplierUser = (fuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? fuelRequest.CounterOffers.FirstOrDefault().User1 : orderDetails.User);
                    var supplierCompany = supplierUser.Company;

                    response = new NotificationDispatchViewModel()
                    {
                        Id = orderDetails.Id,
                        BuyerCompanyId = buyerCompany.Id,
                        BuyerCompanyName = buyerCompany.Name,
                        BuyerUser = new NotificationUserViewModel()
                        {
                            Id = buyerUser.Id,
                            Email = buyerUser.Email,
                            FirstName = buyerUser.FirstName,
                            LastName = buyerUser.LastName
                        },
                        SupplierCompanyId = supplierCompany.Id,
                        SupplierCompanyName = supplierCompany.Name,
                        SupplierUser = new NotificationUserViewModel()
                        {
                            Id = supplierUser.Id,
                            Email = supplierUser.Email,
                            FirstName = supplierUser.FirstName,
                            LastName = supplierUser.LastName
                        },
                        PoNumber = orderDetails.PoNumber,
                        JobName = fuelRequest.Job.Name,
                        UserFirstName = appLocationDetails.User.FirstName,
                        UserLastName = appLocationDetails.User.LastName,
                        OnsitePersons = GetOnsiteContactUsers(fuelRequest.Job)
                    };

                    if (orderDetails.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest)
                        response.IsTpoOrder = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetDispatchNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        #region Invoice Status Notifications
        public NotificationInvoiceViewModel GetInvoiceNotificationDetails(int id)
        {
            var response = new NotificationInvoiceViewModel();
            var helperDomain = new HelperDomain(this);
            try
            {
                var invoiceDetails = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == id && t.Order != null).ToList();
                var invoice = invoiceDetails.FirstOrDefault();
                if (invoice != null)
                {
                    var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == invoice.UpdatedBy);
                    var buyerUser = (invoice.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? invoice.Order.FuelRequest.CounterOffers.FirstOrDefault().User : invoice.Order.FuelRequest.User);
                    var buyerCompany = buyerUser.Company;
                    var supplierUser = (invoice.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? invoice.Order.FuelRequest.CounterOffers.FirstOrDefault().User1 : invoice.Order.User);
                    var supplierCompany = supplierUser.Company;
                    var resaleCustomer = invoice.Order.FuelRequest.ResaleCustomers.Select(t => t.ToViewModel()).ToList();
                    var driver = Context.DataContext.Users.FirstOrDefault(t => t.Id == invoice.DriverId);
                    var ddtNumber = (invoice.Invoice1 != null && invoice.Invoice1.InvoiceHeader.InvoiceNumber != null) ?
                                                        invoice.Invoice1.DisplayInvoiceNumber : "";
                    var driverName = driver != null ? $"{driver.FirstName ?? ""} {driver.LastName ?? ""}" : "";
                    var updatedByUser = $"{user.FirstName ?? ""} {user.LastName ?? ""}";
                    response = new NotificationInvoiceViewModel()
                    {
                        BuyerUser = new NotificationUserViewModel()
                        {
                            Id = buyerUser.Id,
                            Email = buyerUser.Email,
                            FirstName = buyerUser.FirstName,
                            LastName = buyerUser.LastName
                        },
                        SupplierUser = new NotificationUserViewModel()
                        {
                            Id = supplierUser.Id,
                            Email = supplierUser.Email,
                            FirstName = supplierUser.FirstName,
                            LastName = supplierUser.LastName
                        },
                        SupplierCompanyId = supplierCompany.Id,
                        SupplierCompanyName = supplierCompany.Name,
                        Id = invoice.Id,
                        BuyerCompanyId = buyerCompany.Id,
                        BuyerCompanyName = buyerCompany.Name,
                        IsUpdatedByBuyer = user.CompanyId == buyerCompany.Id,
                        DueDate = invoice.PaymentDueDate.Date.Date,
                        InvoiceType = invoice.InvoiceTypeId,
                        IsInvoice = invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp,
                        JobName = invoice.Order.FuelRequest.Job.Name,
                        IsResaleEnabled = invoice.Order.FuelRequest.Job.IsResaleEnabled,
                        CreatedOn = invoice.CreatedDate,
                        InvoiceNumber = invoice.DisplayInvoiceNumber,
                        ResaleCustomer = resaleCustomer,
                        DriverName = driverName,
                        UpdatedByUserName = updatedByUser,
                        DdtNumberOfInvoice = ddtNumber,
                        IsPartOfStatement = invoiceDetails.Any(t => t.IsPartOfStatement),
                        IsProFormaPo = invoiceDetails.Any(t => t.Order.IsProFormaPo),
                        DropDate = invoice.DropStartDate.DateTime.ToString(Resource.constFormatDate),
                        DayOfWeek = invoice.DropEndDate.DayOfWeek.ToString(),
                        DropStartTime = invoiceDetails.Select(t => t.DropStartDate.DateTime).OrderBy(t => t).FirstOrDefault().ToString(Resource.constFormat12HourTime),
                        DropEndTime = invoiceDetails.Select(t => t.DropEndDate.DateTime).OrderByDescending(t => t).FirstOrDefault().ToString(Resource.constFormat12HourTime),
                    };
                    var orders = invoiceDetails.GroupBy(t => t.OrderId);
                    foreach (var order in orders)
                    {
                        var drop = order.FirstOrDefault();
                        response.DropAdditionalDetails.Add(new NotificationDropAdditionalViewModel()
                        {
                            DropQuantity = order.Sum(t => t.DroppedGallons).GetPreciseValue(6),
                            ConvertedQuantity = order.Sum(t => t.ConvertedQuantity).GetPreciseValue(6),
                            PoNumber = drop.PoNumber,
                            OrderId = drop.OrderId ?? 0,
                            FuelRequestTypeId = drop.Order.FuelRequest.FuelRequestTypeId,
                            FuelType = helperDomain.GetProductName(drop.Order.FuelRequest.MstProduct),
                            UoM = drop.UoM,
                            IsExceedingQuantity = IsInvoiceCreatedWithExceedingQuantity(order.ToList()),
                            IsTpoOrder = drop.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest
                        });
                    }
                    response.UsersAssignedToJob = GetJobAssignedUsers(invoice);
                    response.OnsitePersons = GetOnsiteContactUsers(invoice.Order.FuelRequest.Job);
                    response.IsBrokeredInvoice = invoice.Order.BuyerCompanyId != invoice.Order.FuelRequest.Job.CompanyId;
                    response.SupplierAccountingUsers = supplierCompany.Users.Where(t => t.IsActive && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.AccountingPerson))
                        .Select(t => new NotificationUserViewModel()
                        {
                            Email = t.Email,
                            FirstName = t.FirstName,
                            LastName = t.LastName,
                            Id = t.Id
                        }).ToList();
                    if (invoiceDetails.All(t => t.Order.OrderAdditionalDetail != null && t.Order.IsFTL))
                    {
                        response.DeliveryInstructionsExists = true;
                        response.InvoiceNotificationPreferenceId = invoice.Order.OrderAdditionalDetail.BOLInvoicePreferenceId;
                        response.ReplaceInvoiceWithDdt = invoice.ReplaceInvoiceWithDdt;
                    }
                    if (invoice.Order.BuyerCompanyId == invoice.Order.FuelRequest.Job.CompanyId)
                    {
                        var approvalUser = invoice.Order.FuelRequest.Job.JobXApprovalUsers.SingleOrDefault(t => t.IsActive);
                        if (approvalUser != null)
                        {
                            var approver = approvalUser.User;
                            response.InvoiceApprover = new NotificationUserViewModel()
                            {
                                Id = approver.Id,
                                Email = approver.Email,
                                FirstName = approver.FirstName,
                                LastName = approver.LastName
                            };

                            var invoiceApproved = invoice.InvoiceXInvoiceStatusDetails.FirstOrDefault(t => t.StatusId == (int)InvoiceStatus.Received);
                            if (invoiceApproved != null)
                            {
                                response.ApprovedOn = invoiceApproved.UpdatedDate;
                            }
                        }
                    }
                    GetInvoicePDFSetting(response);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetInvoiceNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        public NotificationBDNViewModel GetBDNNotificationDetails(int id)
        {
            var response = new NotificationBDNViewModel();
            try
            {
                var invoiceDetails = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == id && t.Order != null).OrderBy(t => t.Id).ToList();
                var invoice = invoiceDetails.FirstOrDefault();
                if (invoice != null)
                {
                    var buyerUser = (invoice.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? invoice.Order.FuelRequest.CounterOffers.FirstOrDefault().User : invoice.Order.FuelRequest.User);
                    var buyerCompany = buyerUser.Company;
                    var supplierUser = (invoice.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? invoice.Order.FuelRequest.CounterOffers.FirstOrDefault().User1 : invoice.Order.User);
                    var supplierCompany = supplierUser.Company;
                    response = new NotificationBDNViewModel();
                    response.BuyerUser = new NotificationUserViewModel()
                    {
                        Id = buyerUser.Id,
                        Email = buyerUser.Email,
                        FirstName = buyerUser.FirstName,
                        LastName = buyerUser.LastName
                    };
                    response.SupplierUser = new NotificationUserViewModel()
                    {
                        Id = supplierUser.Id,
                        Email = supplierUser.Email,
                        FirstName = supplierUser.FirstName,
                        LastName = supplierUser.LastName
                    };
                    response.SupplierCompanyId = supplierCompany.Id;
                    response.SupplierCompanyName = supplierCompany.Name;
                    response.Id = invoice.Id;
                    response.BuyerCompanyId = buyerCompany.Id;
                    response.BuyerCompanyName = buyerCompany.Name;
                    response.InvoiceType = invoice.InvoiceTypeId;
                    response.JobName = invoice.Order.FuelRequest.Job.Name;
                    var vessle = Context.DataContext.JobXAssets.Where(t => invoice.OrderId == t.OrderId).FirstOrDefault();
                    response.Vessle = vessle != null ? vessle.Asset.Name : string.Empty;
                    response.UoM = invoice.Order.FuelRequest.Job.UoM;
                    response.CreatedOn = invoice.CreatedDate;
                    response.InvoiceNumber = invoice.DisplayInvoiceNumber;
                    response.DropDate = invoiceDetails.LastOrDefault().DropStartDate.DateTime.ToString(Resource.constFormatDate);
                    response.DroppedQuantity = invoiceDetails.Sum(t => t.DroppedGallons).GetPreciseValue(2).GetCommaSeperatedValue();

                    var bdnBolDetails = new List<NotificationBDNBolDetailViewModel>();
                    foreach (var invoiceBol in invoiceDetails)
                    {
                        var bolDetails = invoiceBol.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail).ToList();
                        foreach (var item in bolDetails)
                        {
                            var bdnBolDetail = new NotificationBDNBolDetailViewModel();
                            bdnBolDetail.BolNumber = item.BolNumber ?? item.LiftTicketNumber;
                            bdnBolDetail.GrossQuantity = item.GrossQuantity.Value.GetPreciseValue(2).GetCommaSeperatedValue();
                            bdnBolDetail.NetQuantity = item.NetQuantity.Value.GetPreciseValue(2).GetCommaSeperatedValue();
                            bdnBolDetails.Add(bdnBolDetail);
                        }
                    }

                    response.TotalBolCount = bdnBolDetails.Count;
                    response.BDNBolDetails = bdnBolDetails;

                    var userIds = new List<int>();
                    var trackableSchedules = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.GroupParentDRId != null && t.GroupParentDRId == invoice.GroupParentDrId).ToList();
                    if (trackableSchedules == null)
                    {
                        var trackableScheduleIds = invoiceDetails.Select(t => t.TrackableScheduleId).ToList();
                        trackableSchedules = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => trackableScheduleIds.Contains(t.Id)).ToList();
                    }

                    if (trackableSchedules != null)
                    {
                        userIds = trackableSchedules.Where(t => t.DriverId.HasValue).Select(t => t.DriverId.Value).Distinct().ToList();
                        var regionIds = new List<string>();
                        foreach (var item in trackableSchedules)
                        {
                            var additionalInfo = JsonConvert.DeserializeObject<ScheduleAdditionalInfo>(item.AdditionalInfo);
                            regionIds.Add(additionalInfo.FsAssignedRegionId);
                            regionIds.Add(additionalInfo.FsRegionId);
                        }
                        regionIds = regionIds.Distinct().ToList();
                        if (regionIds != null && regionIds.Any())
                        {
                            var assignedDispatchers = ContextFactory.Current.GetDomain<FreightServiceDomain>().GetDispatchersAssignedToRegion(regionIds).Result;
                            if (assignedDispatchers != null)
                            {
                                var dispatcherIds = assignedDispatchers.Select(t => t.Id).Distinct().ToList();
                                userIds.AddRange(dispatcherIds);
                            }
                        }
                    }

                    response.Users = Context.DataContext.Users.Where(t => userIds.Contains(t.Id)).
                                    Select(t => new NotificationUserViewModel()
                                    {
                                        Email = t.Email,
                                        FirstName = t.FirstName,
                                        LastName = t.LastName,
                                        Id = t.Id
                                    }).Distinct().ToList();
                    BdnConsolidationCalculation(response, invoiceDetails);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetBDNNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }


        private void BdnConsolidationCalculation(NotificationBDNViewModel response, List<Invoice> invoices)
        {
            if (response != null)
            {
                var invoiceIds = invoices.Select(t => t.Id).ToList();
                var bdrDetails = Context.DataContext.BDRDetails.Where(t => invoiceIds.Contains(t.InvoiceId)).ToList();

                decimal consolidatedTemp = 0, consolidatedSulfurContent = 0, consolidatedViscocity = 0, consolidatedFlashPoint = 0, consolidatedDensityInVacuum = 0;
                decimal denominatorTotalDropQty = 0;

                foreach (var bdn in bdrDetails)
                {
                    decimal.TryParse(bdn.ObservedTemperature, out decimal t1);
                    consolidatedTemp += invoices.Where(t => t.Id == bdn.InvoiceId).Sum(t => t.DroppedGallons) * t1;

                    decimal.TryParse(bdn.SulphurContent, out decimal s1);
                    consolidatedSulfurContent += invoices.Where(t => t.Id == bdn.InvoiceId).Sum(t => t.DroppedGallons) * s1;

                    decimal.TryParse(bdn.Viscosity, out decimal v1);
                    consolidatedViscocity += invoices.Where(t => t.Id == bdn.InvoiceId).Sum(t => t.DroppedGallons) * v1;

                    decimal.TryParse(bdn.FlashPoint, out decimal f1);
                    consolidatedFlashPoint += invoices.Where(t => t.Id == bdn.InvoiceId).Sum(t => t.DroppedGallons) * f1;

                    decimal.TryParse(bdn.DensityInVaccum, out decimal d1);
                    consolidatedDensityInVacuum += invoices.Where(t => t.Id == bdn.InvoiceId).Sum(t => t.DroppedGallons) * d1;

                    denominatorTotalDropQty += invoices.Where(t => t.Id == bdn.InvoiceId).Sum(t => t.DroppedGallons);
                }

                if (denominatorTotalDropQty > 0)
                {
                    if (bdrDetails.Any(t => t.ObservedTemperature == null || t.ObservedTemperature == ""))
                        response.ObservedTemperature = Resource.messageNA;
                    else
                        response.ObservedTemperature = (consolidatedTemp / denominatorTotalDropQty).GetCommaSeperatedValue4Decimals();

                    if (bdrDetails.Any(t => t.SulphurContent == null || t.SulphurContent == ""))
                        response.SulphurContent = Resource.messageNA;
                    else
                        response.SulphurContent = (consolidatedSulfurContent / denominatorTotalDropQty).GetCommaSeperatedValue4Decimals();

                    if (bdrDetails.Any(t => t.Viscosity == null || t.Viscosity == ""))
                        response.Viscosity = Resource.messageNA;
                    else
                        response.Viscosity = (consolidatedViscocity / denominatorTotalDropQty).GetCommaSeperatedValue4Decimals();

                    if (bdrDetails.Any(t => t.FlashPoint == null || t.FlashPoint == ""))
                        response.FlashPoint = Resource.messageNA;
                    else
                        response.FlashPoint = (consolidatedFlashPoint / denominatorTotalDropQty).GetCommaSeperatedValue4Decimals();

                    if (bdrDetails.Any(t => t.DensityInVaccum == null || t.DensityInVaccum == ""))
                        response.DensityInVaccum = Resource.messageNA;
                    else
                        response.DensityInVaccum = (consolidatedDensityInVacuum / denominatorTotalDropQty).GetCommaSeperatedValue4Decimals();

                    // calculate consolidated API Gravity
                    var consolidatedApiGravity = invoices.Sum(t => ((t.Gravity ?? 0) * t.DroppedGallons));
                    response.CalculatedAPIGravity = (consolidatedApiGravity / denominatorTotalDropQty).GetCommaSeperatedValue4Decimals();

                }
            }
        }

        public NotificationBillingStatementViewModel GetBillingStatementDetails(int id, EventType eventType)
        {
            var response = new NotificationBillingStatementViewModel();
            try
            {
                var billingDetails = Context.DataContext.BillingStatements.Where(t => t.Id == id)
                    .Select(t => new
                    {
                        t.Id,
                        BillingStatementId = t.BillingSchedule != null ? t.BillingSchedule.BillingStatementId : "",
                        t.StatementNumber,
                        Frequency = t.FrequencyType.Name,
                        t.PaymentDueDate,
                        t.StartDate,
                        t.EndDate,
                        t.StatementChainId,
                        IsTpoOrder = t.BillingStatementXInvoices.Any(t1 => t1.Invoice.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest),
                        BuyerCompany = t.BillingStatementXInvoices.Select(t1 => t1.Invoice.Order.BuyerCompany).FirstOrDefault(),
                        SupplierCompany = t.CreatedByCompany,
                        SupplierUsers = t.CreatedByCompany.Users.Where(t1 => t1.IsActive && !t1.MstRoles.Any(t2 => t2.Id == (int)UserRoles.Admin)),
                        AllOrders = t.BillingStatementXInvoices.Where(t1 => t1.Invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).Select(t1 => t1.Invoice.Order).Distinct()
                    }).FirstOrDefault();

                var previousStatementNumber = string.Empty;
                if (eventType == EventType.BillingStatementUpdated)
                {
                    previousStatementNumber = Context.DataContext.BillingStatements.
                        Where(t => t.StatementChainId == billingDetails.StatementChainId && t.Id < billingDetails.Id).
                        OrderByDescending(t => t.Id).Select(t => t.StatementNumber.Number).FirstOrDefault();
                }

                if (billingDetails != null)
                {
                    response = new NotificationBillingStatementViewModel()
                    {
                        SupplierCompanyId = billingDetails.SupplierCompany.Id,
                        SupplierCompanyName = billingDetails.SupplierCompany.Name,
                        Id = billingDetails.Id,
                        BuyerCompanyId = billingDetails.BuyerCompany.Id,
                        BuyerCompanyName = billingDetails.BuyerCompany.Name,
                        Frequency = billingDetails.Frequency,
                        DueDate = billingDetails.PaymentDueDate.Date.Date,
                        StartDate = billingDetails.StartDate.ToString(Resource.constFormatDate),
                        EndDate = billingDetails.EndDate.ToString(Resource.constFormatDate),
                        StatementName = billingDetails.StatementNumber.Number,
                        PreviousStatementName = previousStatementNumber,
                        IsTpoOrder = billingDetails.IsTpoOrder
                    };

                    foreach (var item in billingDetails.AllOrders)
                    {
                        if (item != null && item.BuyerCompanyId == item.FuelRequest.Job.CompanyId)
                        {
                            var assignedUsers = item.FuelRequest.Job.Users.Where(t => t.IsActive)
                                .Select(t => new NotificationUserViewModel()
                                {
                                    Email = t.Email,
                                    FirstName = t.FirstName,
                                    LastName = t.LastName,
                                    Id = t.Id
                                }).ToList();
                            foreach (var user in assignedUsers)
                            {
                                if (!response.UsersAssignedToJob.Exists(t => t.Email == user.Email))
                                {
                                    response.UsersAssignedToJob.AddRange(assignedUsers);
                                }
                            }
                        }
                    }

                    response.SupplierUsers = billingDetails.SupplierUsers
                        .Select(t => new NotificationUserViewModel()
                        {
                            Email = t.Email,
                            FirstName = t.FirstName,
                            LastName = t.LastName,
                            Id = t.Id
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetBillingStatementDetails", "Exception Details : ", ex);
            }
            return response;
        }

        private List<NotificationUserViewModel> GetJobAssignedUsers(Invoice invoiceDetails)
        {
            var response = new List<NotificationUserViewModel>();
            if (invoiceDetails.Order.BuyerCompanyId == invoiceDetails.Order.FuelRequest.Job.CompanyId)
            {
                response = invoiceDetails.Order.FuelRequest.Job.Users.Where(t => t.IsActive)
                    .Select(t => new NotificationUserViewModel()
                    {
                        Email = t.Email,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        Id = t.Id
                    }).ToList();
            }
            return response;
        }

        private List<NotificationUserViewModel> GetOnsiteContactUsers(Job JobDetails)
        {
            var response = JobDetails.Users1.Where(t => t.IsActive)
                                            .Select(t => new NotificationUserViewModel()
                                            {
                                                Email = t.Email,
                                                FirstName = t.FirstName,
                                                LastName = t.LastName,
                                                Id = t.Id
                                            }).ToList();
            return response;
        }

        public NotificationInvoiceViewModel GetInvoiceNotificationDetailsForApprovedInvoice(int id, int approverUserId)
        {
            var response = new NotificationInvoiceViewModel();
            try
            {
                var invoiceDetails = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == id && t.Order != null).ToList();
                var invoice = invoiceDetails.FirstOrDefault();
                if (invoice != null)
                {
                    var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == invoice.UpdatedBy);
                    var buyerUser = (invoice.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? invoice.Order.FuelRequest.CounterOffers.FirstOrDefault().User : invoice.Order.FuelRequest.User);
                    var buyerCompany = buyerUser.Company;
                    var supplierUser = (invoice.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? invoice.Order.FuelRequest.CounterOffers.FirstOrDefault().User1 : invoice.Order.User);
                    var supplierCompany = supplierUser.Company;
                    var resaleCustomer = invoice.Order.FuelRequest.ResaleCustomers.Select(t => t.ToViewModel()).ToList();
                    var driver = Context.DataContext.Users.FirstOrDefault(t => t.Id == invoice.DriverId);
                    var ddtNumber = (invoice.Invoice1 != null && invoice.Invoice1.InvoiceHeader.InvoiceNumber != null) ?
                                                        invoice.Invoice1.DisplayInvoiceNumber : "";
                    var driverName = driver != null ? $"{driver.FirstName ?? ""} {driver.LastName ?? ""}" : "";
                    var updatedByUser = $"{user.FirstName ?? ""} {user.LastName ?? ""}";

                    response = new NotificationInvoiceViewModel()
                    {
                        BuyerUser = new NotificationUserViewModel()
                        {
                            Id = buyerUser.Id,
                            Email = buyerUser.Email,
                            FirstName = buyerUser.FirstName,
                            LastName = buyerUser.LastName
                        },
                        SupplierUser = new NotificationUserViewModel()
                        {
                            Id = supplierUser.Id,
                            Email = supplierUser.Email,
                            FirstName = supplierUser.FirstName,
                            LastName = supplierUser.LastName
                        },
                        SupplierCompanyId = supplierCompany.Id,
                        SupplierCompanyName = supplierCompany.Name,
                        Id = invoice.Id,
                        BuyerCompanyId = buyerCompany.Id,
                        BuyerCompanyName = buyerCompany.Name,
                        IsUpdatedByBuyer = user.Company.Id == buyerCompany.Id,
                        DueDate = invoice.PaymentDueDate.Date.Date,
                        InvoiceType = invoice.InvoiceTypeId,
                        JobName = invoice.Order.FuelRequest.Job.Name,
                        IsResaleEnabled = invoice.Order.FuelRequest.Job.IsResaleEnabled,
                        CreatedOn = invoice.CreatedDate,
                        InvoiceNumber = invoice.DisplayInvoiceNumber,
                        ResaleCustomer = resaleCustomer,
                        DriverName = driverName,
                        UpdatedByUserName = updatedByUser,
                        DdtNumberOfInvoice = ddtNumber,
                        IsPartOfStatement = invoiceDetails.Any(t => t.IsPartOfStatement),
                        DropDate = invoice.DropStartDate.DateTime.ToString(Resource.constFormatDate),
                        DropStartTime = invoiceDetails.Select(t => t.DropStartDate.DateTime).OrderBy(t => t).FirstOrDefault().ToString(Resource.constFormat12HourTime),
                        DropEndTime = invoiceDetails.Select(t => t.DropEndDate.DateTime).OrderByDescending(t => t).FirstOrDefault().ToString(Resource.constFormat12HourTime)
                    };
                    var orders = invoiceDetails.GroupBy(t => t.OrderId);
                    foreach (var order in orders)
                    {
                        var drop = order.FirstOrDefault();
                        response.DropAdditionalDetails.Add(new NotificationDropAdditionalViewModel()
                        {
                            DropQuantity = order.Sum(t => t.DroppedGallons).GetPreciseValue(6),
                            PoNumber = drop.PoNumber,
                            FuelRequestTypeId = drop.Order.FuelRequest.FuelRequestTypeId
                        });
                    }

                    if (invoice.Order.BuyerCompanyId == invoice.Order.FuelRequest.Job.CompanyId)
                    {
                        var approver = Context.DataContext.Users.FirstOrDefault(x => x.Id == approverUserId);
                        if (approver != null)
                        {
                            response.InvoiceApprover = new NotificationUserViewModel()
                            {
                                Id = approver.Id,
                                Email = approver.Email,
                                FirstName = approver.FirstName,
                                LastName = approver.LastName
                            };

                            var invoiceApproved = invoice.InvoiceXInvoiceStatusDetails.FirstOrDefault(t => t.StatusId == (int)InvoiceStatus.Received);
                            if (invoiceApproved != null)
                            {
                                response.ApprovedOn = invoiceApproved.UpdatedDate;
                            }
                        }
                    }
                    if (invoiceDetails.All(t => t.Order.OrderAdditionalDetail != null && t.Order.IsFTL))
                    {
                        response.DeliveryInstructionsExists = true;
                        response.InvoiceNotificationPreferenceId = invoice.Order.OrderAdditionalDetail.BOLInvoicePreferenceId;
                    }
                    GetInvoicePDFSetting(response);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetInvoiceNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        private void GetInvoicePDFSetting(NotificationInvoiceViewModel viewmodel)
        {
            if (viewmodel.BuyerCompanyId > 0 || viewmodel.SupplierCompanyId > 0)
            {
                var pdfSetting = Context.DataContext.Companies.Where(t => t.Id == viewmodel.BuyerCompanyId || t.Id == viewmodel.SupplierCompanyId).Select(t => new { t.Id, t.EnableInvoicePDF }).ToList();
                viewmodel.SendAttachmentToBuyer = pdfSetting.Any(t => t.EnableInvoicePDF && t.Id == viewmodel.BuyerCompanyId);
                viewmodel.SendAttachmentToSupplier = pdfSetting.Any(t => t.EnableInvoicePDF && t.Id == viewmodel.SupplierCompanyId);
            }
        }

        public NotificationInvoiceViewModel GetInvoiceDetailsForUnassignedDdtNotification(int id)
        {
            var response = new NotificationInvoiceViewModel();
            try
            {
                var invoiceDetails = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == id).ToList();
                var invoice = invoiceDetails.FirstOrDefault();
                if (invoiceDetails != null)
                {
                    var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == invoice.UpdatedBy);
                    var driver = Context.DataContext.Users.FirstOrDefault(t => t.Id == invoice.DriverId);
                    var driverName = driver != null ? $"{driver.FirstName ?? ""} {driver.LastName ?? ""}" : "";
                    var updatedByUser = $"{user.FirstName ?? ""} {user.LastName ?? ""}";
                    var driverCompany = driver != null ? driver.Company : null;

                    response = new NotificationInvoiceViewModel()
                    {
                        Id = invoice.Id,
                        DueDate = invoice.PaymentDueDate.Date.Date,
                        InvoiceType = invoice.InvoiceTypeId,
                        CreatedOn = invoice.CreatedDate,
                        InvoiceNumber = invoice.DisplayInvoiceNumber,
                        DriverName = driverName,
                        UpdatedByUserName = updatedByUser,
                        SupplierCompanyName = driverCompany != null ? driverCompany.Name : string.Empty,
                        SupplierCompanyId = driverCompany != null ? driverCompany.Id : 0,
                        DropStartTime = invoiceDetails.Select(t => t.DropStartDate.DateTime).OrderBy(t => t).FirstOrDefault().ToString(Resource.constFormat12HourTime),
                        DropEndTime = invoiceDetails.Select(t => t.DropEndDate.DateTime).OrderByDescending(t => t).FirstOrDefault().ToString(Resource.constFormat12HourTime),
                        DropDate = invoice.DropStartDate.DateTime.ToString(Resource.constFormatDate),
                    };
                    var orders = invoiceDetails.GroupBy(t => t.OrderId);
                    foreach (var order in orders)
                    {
                        var drop = order.FirstOrDefault();
                        response.DropAdditionalDetails.Add(new NotificationDropAdditionalViewModel()
                        {
                            DropQuantity = order.Sum(t => t.DroppedGallons).GetPreciseValue(6),
                            UoM = drop.UoM
                        });
                    }
                    GetInvoicePDFSetting(response);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetInvoiceNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        public NotificationInvoiceViewModel GetInvoiceUpdatedNotificationDetails(int id)
        {
            var response = new NotificationInvoiceViewModel();
            try
            {
                var invoiceDetails = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == id && t.Order != null).ToList();
                var invoice = invoiceDetails.FirstOrDefault();
                if (invoiceDetails != null)
                {
                    var user = Context.DataContext.Users.Where(t => t.Id == invoice.UpdatedBy).Select(n => new { n.FirstName, n.LastName }).FirstOrDefault();
                    var buyerUser = (invoice.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? invoice.Order.FuelRequest.CounterOffers.FirstOrDefault().User : invoice.Order.FuelRequest.User);
                    var supplierUser = (invoice.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? invoice.Order.FuelRequest.CounterOffers.FirstOrDefault().User1 : invoice.Order.User);
                    var updatedByUser = $"{user.FirstName ?? ""} {user.LastName ?? ""}";
                    var supplierCompany = supplierUser.Company;
                    response = new NotificationInvoiceViewModel()
                    {
                        BuyerUser = new NotificationUserViewModel()
                        {
                            Id = buyerUser.Id,
                            Email = buyerUser.Email,
                            FirstName = buyerUser.FirstName,
                            LastName = buyerUser.LastName
                        },
                        SupplierUser = new NotificationUserViewModel()
                        {
                            Id = supplierUser.Id,
                            Email = supplierUser.Email,
                            FirstName = supplierUser.FirstName,
                            LastName = supplierUser.LastName
                        },
                        Id = invoice.Id,
                        CreatedOn = invoice.CreatedDate,
                        InvoiceNumber = invoice.DisplayInvoiceNumber,
                        UpdatedDate = invoice.UpdatedDate.ToString(Resource.constFormatDate),
                        UpdatedTime = invoice.UpdatedDate.ToString(Resource.constFormat12HourTime),
                        IsUpdatedByBuyer = buyerUser.Company.CompanyTypeId != (int)CompanyType.Supplier,
                        InvoiceType = invoice.InvoiceTypeId,
                        UpdatedByUserName = updatedByUser,
                        SupplierCompanyId = supplierCompany.Id,
                        SupplierCompanyName = supplierCompany.Name,
                        BuyerCompanyId = buyerUser.Company.Id,
                        IsPartOfStatement = invoiceDetails.Any(t => t.IsPartOfStatement),
                        DayOfWeek = invoice.UpdatedDate.DayOfWeek.ToString()
                    };
                    var orders = invoiceDetails.GroupBy(t => t.OrderId);
                    foreach (var order in orders)
                    {
                        response.DropAdditionalDetails.Add(new NotificationDropAdditionalViewModel()
                        {
                            IsExceedingQuantity = IsInvoiceCreatedWithExceedingQuantity(order.ToList())
                        });
                    }
                    if (invoiceDetails.All(t => t.Order.OrderAdditionalDetail != null && t.Order.IsFTL))
                    {
                        response.DeliveryInstructionsExists = true;
                        response.InvoiceNotificationPreferenceId = invoice.Order.OrderAdditionalDetail.BOLInvoicePreferenceId;
                        response.ReplaceInvoiceWithDdt = invoice.ReplaceInvoiceWithDdt;
                    }
                    response.UsersAssignedToJob = GetJobAssignedUsers(invoice);
                    response.IsBrokeredInvoice = invoice.Order.BuyerCompanyId != invoice.Order.FuelRequest.Job.CompanyId;
                    response.SupplierAccountingUsers = supplierUser.Company.Users.Where(t => t.IsActive && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.AccountingPerson))
                        .Select(t => new NotificationUserViewModel()
                        {
                            Email = t.Email,
                            FirstName = t.FirstName,
                            LastName = t.LastName,
                            Id = t.Id
                        }).ToList();
                    GetInvoicePDFSetting(response);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetInvoiceNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        public NotificationInvoiceViewModel GetDryrunInvoiceNotificationDetails(int id)
        {
            var response = new NotificationInvoiceViewModel();
            try
            {
                var invoiceDetails = Context.DataContext.Invoices.SingleOrDefault(t => t.InvoiceHeaderId == id && t.Order != null);
                if (invoiceDetails != null)
                {
                    var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == invoiceDetails.UpdatedBy);
                    var buyerUser = (invoiceDetails.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? invoiceDetails.Order.FuelRequest.CounterOffers.FirstOrDefault().User : invoiceDetails.Order.FuelRequest.User);
                    var supplierUser = (invoiceDetails.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? invoiceDetails.Order.FuelRequest.CounterOffers.FirstOrDefault().User1 : invoiceDetails.Order.User);
                    var supplierCompany = supplierUser.Company;
                    var ddtNumber = (invoiceDetails.Invoice1 != null && invoiceDetails.Invoice1.InvoiceHeader.InvoiceNumber != null) ?
                                                        invoiceDetails.Invoice1.DisplayInvoiceNumber : "";
                    var updatedByUser = $"{user.FirstName ?? ""} {user.LastName ?? ""}";

                    response = new NotificationInvoiceViewModel()
                    {
                        BuyerUser = new NotificationUserViewModel()
                        {
                            Id = buyerUser.Id,
                            Email = buyerUser.Email,
                            FirstName = buyerUser.FirstName,
                            LastName = buyerUser.LastName
                        },
                        SupplierUser = new NotificationUserViewModel()
                        {
                            Id = supplierUser.Id,
                            Email = supplierUser.Email,
                            FirstName = supplierUser.FirstName,
                            LastName = supplierUser.LastName
                        },
                        SupplierCompanyName = supplierCompany.Name,
                        SupplierCompanyId = supplierCompany.Id,
                        BuyerCompanyId = buyerUser.Company.Id,
                        Id = invoiceDetails.Id,
                        DueDate = invoiceDetails.PaymentDueDate.Date.Date,
                        InvoiceNumber = invoiceDetails.DisplayInvoiceNumber,
                        UpdatedByUserName = updatedByUser,
                        DdtNumberOfInvoice = ddtNumber,
                        IsProFormaPo = invoiceDetails.Order.IsProFormaPo,
                        IsPartOfStatement = invoiceDetails.IsPartOfStatement,
                        IsInvoice = invoiceDetails.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && invoiceDetails.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp
                    };

                    response.UsersAssignedToJob = GetJobAssignedUsers(invoiceDetails);

                    response.SupplierAccountingUsers = supplierCompany.Users.Where(t => t.IsActive && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.AccountingPerson))
                        .Select(t => new NotificationUserViewModel()
                        {
                            Email = t.Email,
                            FirstName = t.FirstName,
                            LastName = t.LastName,
                            Id = t.Id
                        }).ToList();

                    if (invoiceDetails.Order.OrderAdditionalDetail != null && invoiceDetails.Order.IsFTL)
                    {
                        response.DeliveryInstructionsExists = true;
                        response.InvoiceNotificationPreferenceId = invoiceDetails.Order.OrderAdditionalDetail.BOLInvoicePreferenceId;
                    }

                    GetInvoicePDFSetting(response);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetDdtToInvoiceNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        #endregion

        public NotificationDipTestViewModel GetDipTestNotificationDetails(int companyId, string jsonMessage)
        {
            var response = new NotificationDipTestViewModel();
            try
            {
                var jsonData = JsonConvert.DeserializeObject<DipTestProcessViewModel>(jsonMessage);
                var userContext = new UserContext(0, "", "", "", companyId, "", jsonData.CompanyTypeId, 0, new List<int>(), false, false, false, 0);
                var freightServiceDomain = new FreightServiceDomain(this);
                response.CompanyType = jsonData.CompanyTypeId;
                response.CompanyId = companyId;

                response.DipTest = Task.Run(() => freightServiceDomain.GetLocationInventoryDiptestSummaryAsync(jsonData.DipTestMethodInventoryDataCapture, userContext)).Result;

                response.CompanyUsers = Context.DataContext.Users.Where(t => t.CompanyId == companyId && t.IsActive &&
                                        t.MstRoles.Any(x => x.Id == (int)UserRoles.Supplier || x.Id == (int)UserRoles.Admin
                                                            || x.Id == (int)UserRoles.Buyer || x.Id == (int)UserRoles.OnsitePerson || x.Id == (int)UserRoles.Dispatcher || x.Id == (int)UserRoles.Carrier))
                                        .Select(t => new NotificationUserViewModel { CompanyId = companyId, Id = t.Id, FirstName = t.FirstName, LastName = t.LastName, Email = t.Email })
                                        .ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetDipTestNotificationDetails", "Exception Details : " + ex.Message, ex);
            }
            return response;
        }


        #region QuickBooks Sync Report

        public async Task<NotificationQbReportViewModel> GetQuickBooksSyncReportNotificationDetail(int companyId, DateTimeOffset notificationCreatedDate)
        {
            var response = new NotificationQbReportViewModel();
            try
            {
                var reportDate = notificationCreatedDate;
                var qbReportDomain = new QbReportDomain(this);
                response.QbReport = await qbReportDomain.GetQuickBooksSyncReportViewModel(companyId, reportDate);
                response.SubscribedUsers = await GetEventSubscribedUsers(companyId, EventType.QuickBooksSyncReport);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetQuickBooksSyncReport", "Exception Details : ", ex);
            }
            return response;
        }

        public async Task<List<string>> GetEventSubscribedUsers(int companyId, EventType eventType)
        {
            var response = new List<string>();
            try
            {
                response = await Context.DataContext.UserXNotificationSettings
                            .Where(t => t.EventTypeId == (int)eventType && t.User.CompanyId == companyId && t.IsEmail)
                            .Select(t => t.User.Email).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetSubscribedUsers", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<QbCompanyViewModel>> GetQbConfiguredCompanies()
        {
            var response = new List<QbCompanyViewModel>();
            try
            {
                response = await (from qbProfile in Context.DataContext.QbProfiles
                                  join user in Context.DataContext.Users
                                  on qbProfile.CompanyId equals user.CompanyId
                                  join notifiation in Context.DataContext.UserXNotificationSettings
                                  on new { UserId = user.Id, EventTypeId = (int)EventType.QuickBooksSyncReport }
                                  equals new { notifiation.UserId, notifiation.EventTypeId }
                                  select new QbCompanyViewModel
                                  {
                                      Id = qbProfile.CompanyId,
                                      SyncReportTime = qbProfile.SyncReportTime,
                                      ReportTimeZone = qbProfile.ReportTimeZone
                                  }).Distinct().ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetSubscribedUsers", ex.Message, ex);
            }
            return response;
        }

        public bool IsQbReportTimeMatchToTimeZone(DateTimeOffset currentDateTime, string syncTime, string timeZone)
        {
            var response = false;
            try
            {
                var targetTime = TimeSpan.Parse(syncTime);
                var targetDateTime = currentDateTime.ToTargetDateTimeOffset(timeZone);
                response = targetDateTime.Hour == targetTime.Hours;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "IsQbReportTimeMatchToTimeZone", ex.Message, ex);
            }
            return response;
        }

        #endregion

        public NotificationDealViewModel GetDealNotificationDetails(int id)
        {
            var response = new NotificationDealViewModel();
            try
            {
                var dealDeatils = Context.DataContext.Discounts.SingleOrDefault(t => t.Id == id);
                if (dealDeatils != null)
                {
                    var invoiceDetails = dealDeatils.Invoice;
                    if (invoiceDetails != null)
                    {
                        var buyerUser = (invoiceDetails.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? invoiceDetails.Order.FuelRequest.CounterOffers.FirstOrDefault().User : invoiceDetails.Order.FuelRequest.User);
                        var buyerCompany = buyerUser.Company;
                        var supplierUser = (invoiceDetails.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? invoiceDetails.Order.FuelRequest.CounterOffers.FirstOrDefault().User1 : invoiceDetails.Order.User);
                        var supplierCompany = supplierUser.Company;
                        response = new NotificationDealViewModel()
                        {
                            BuyerUser = new NotificationUserViewModel()
                            {
                                Id = buyerUser.Id,
                                Email = buyerUser.Email,
                                FirstName = buyerUser.FirstName,
                                LastName = buyerUser.LastName
                            },
                            SupplierUser = new NotificationUserViewModel()
                            {
                                Id = supplierUser.Id,
                                Email = supplierUser.Email,
                                FirstName = supplierUser.FirstName,
                                LastName = supplierUser.LastName
                            },
                            SupplierCompanyId = supplierCompany.Id,
                            BuyerCompanyId = buyerCompany.Id,
                            InvoiceId = invoiceDetails.Id,
                            DealId = dealDeatils.Id,
                            InvoiceNumber = invoiceDetails.DisplayInvoiceNumber,
                            DealName = dealDeatils.DealName,
                            DealCreatedBy = $"{dealDeatils.User.FirstName ?? ""} {dealDeatils.User.LastName ?? ""}",
                        };

                        if ((dealDeatils.DealStatus == (int)DealStatus.Accepted || dealDeatils.DealStatus == (int)DealStatus.Declined) && dealDeatils.StatusChangedBy.HasValue)
                        {
                            response.DealStatusChangedBy = ContextFactory.Current.GetDomain<HelperDomain>().GetUserNameById(dealDeatils.StatusChangedBy.Value);
                        }
                        response.UsersAssignedToJob = GetJobAssignedUsers(invoiceDetails);

                        response.SupplierAccountingUsers = supplierCompany.Users.Where(t => t.IsActive && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.AccountingPerson))
                        .Select(t => new NotificationUserViewModel()
                        {
                            Email = t.Email,
                            FirstName = t.FirstName,
                            LastName = t.LastName,
                            Id = t.Id
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetDealNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        public NotificationInvoiceViewModel GetSuperAdminDetails(int id)
        {
            var response = new NotificationInvoiceViewModel();
            try
            {
                var invoiceDetails = Context.DataContext.Invoices.SingleOrDefault(t => t.Id == id && t.Order != null);
                if (invoiceDetails != null)
                {
                    var superAdminUser = Context.DataContext.Users.SingleOrDefault(t => t.Id == (int)SystemUser.System);
                    var supplierUser = (invoiceDetails.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? invoiceDetails.Order.FuelRequest.CounterOffers.FirstOrDefault().User1 : invoiceDetails.Order.User);

                    response = new NotificationInvoiceViewModel()
                    {
                        BuyerUser = new NotificationUserViewModel()
                        {
                            Id = superAdminUser.Id,
                            Email = superAdminUser.Email,
                            FirstName = superAdminUser.FirstName,
                            LastName = superAdminUser.LastName
                        },
                        SupplierUser = new NotificationUserViewModel()
                        {
                            Id = supplierUser.Id,
                            Email = supplierUser.Email,
                            FirstName = supplierUser.FirstName,
                            LastName = supplierUser.LastName
                        },
                        Id = invoiceDetails.Id,
                        InvoiceNumber = invoiceDetails.DisplayInvoiceNumber
                    };
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetSuperAdminDetails", "Exception Details : ", ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetUserEmailsForBrokeredDR(int assignedCompanyId = 0)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = (from user in Context.DataContext.Users
                            where user.CompanyId == assignedCompanyId && user.IsActive && user.MstRoles.Any(t1 => t1.Id == (int)UserRoles.Supplier ||
                                        t1.Id == (int)UserRoles.Admin || t1.Id == (int)UserRoles.Carrier || t1.Id == (int)UserRoles.CarrierAdmin)
                            select new DropdownDisplayExtendedItem { Id = user.Id, Code = user.Email, Name = user.FirstName }).Distinct().OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetUserEmailsForBrokeredDR", ex.Message, ex);
            }
            return response;
        }

        #region Tank Delivery Request Notifications
        public List<DropdownDisplayExtendedItem> GetCarrrierUserEmails(int jobId = 0)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = (from carrieremail in Context.DataContext.CarrierEmailSettings
                            join user in Context.DataContext.Users on carrieremail.UserId equals user.Id
                            where carrieremail.JobId == jobId && carrieremail.IsActive
                            select new DropdownDisplayExtendedItem { Id = user.Id, Code = user.Email, Name = user.FirstName }).Distinct().OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetCarrrierUserEmails", ex.Message, ex);
            }
            return response;
        }

        public async Task<NotificationDeliveryRequestViewModel> GetTankDeliveryRequestNotificationDetails(string id)
        {
            var response = new NotificationDeliveryRequestViewModel();
            try
            {
                var uRLDetails = await Context.DataContext.MstAppSettings.Where(x => x.Key == "SiteFuelExchangeUrl").Select(x => x.Value).FirstOrDefaultAsync();
                var freightServiceDomain = new FreightServiceDomain(this);
                var deliveryRequestDetails = await freightServiceDomain.GetDeliveryRequestDetailsById(id);
                bool isWithoutTank = false;

                if (deliveryRequestDetails != null && string.IsNullOrEmpty(deliveryRequestDetails.TankId) && deliveryRequestDetails.DeliveryRequestFor != DeliveryRequestFor.ProductType)
                {
                    if (deliveryRequestDetails.OrderId > 0)
                    {
                        isWithoutTank = true;
                        var order = await Context.DataContext.Orders.Where(t => t.Id == deliveryRequestDetails.OrderId).Select(t => new
                        {
                            BuyerCompanyName = t.BuyerCompany.Name,
                            FuelType = t.FuelRequest.MstProduct.MstTFXProduct.Name,
                        }).FirstOrDefaultAsync();

                        if (order != null)
                        {
                            response = new NotificationDeliveryRequestViewModel();
                            response.BuyerCompanyName = order.BuyerCompanyName;
                            response.FuelType = order.FuelType;
                            response.ProductType = deliveryRequestDetails.ProductType;
                            response.JobName = deliveryRequestDetails.JobName;
                            response.JobId = deliveryRequestDetails.JobId;
                            response.QuantityId = deliveryRequestDetails.ScheduleQuantityType;
                            response.Quantity = deliveryRequestDetails.RequiredQuantity;
                            response.UoM = (UoM)deliveryRequestDetails.UoM;
                            response.UniqueOrderNo = deliveryRequestDetails.UniqueOrderNo;
                            response.URLDetails = uRLDetails + "/Carrier/ScheduleBuilder";
                            response.TankName = "NA";
                        }
                    }
                }

                if (deliveryRequestDetails != null && !isWithoutTank)
                {
                    response = new NotificationDeliveryRequestViewModel();
                    response.BuyerCompanyName = deliveryRequestDetails.CustomerCompany;
                    response.ProductType = deliveryRequestDetails.ProductType;
                    response.JobName = deliveryRequestDetails.JobName;
                    response.JobId = deliveryRequestDetails.JobId;
                    response.QuantityId = deliveryRequestDetails.ScheduleQuantityType;
                    response.Quantity = deliveryRequestDetails.RequiredQuantity;
                    response.UoM = (UoM)deliveryRequestDetails.UoM;
                    response.FuelType = "NA";

                    if (deliveryRequestDetails.DeliveryRequestFor != DeliveryRequestFor.ProductType)
                    {
                        var assetDetails = await Context.DataContext.JobXAssets.Where(t => t.JobId == deliveryRequestDetails.JobId
                                                && t.Asset.AssetAdditionalDetail.Vendor == deliveryRequestDetails.StorageId
                                                && t.Asset.AssetAdditionalDetail.VehicleId == deliveryRequestDetails.TankId
                                                && t.RemovedBy == null && t.RemovedDate == null).
                                           Select(t => new
                                           {
                                               TankName = t.Asset.Name
                                           }).FirstOrDefaultAsync();

                        if (assetDetails != null)
                        {
                            response.TankName = assetDetails.TankName;
                        }
                    }
                    response.UniqueOrderNo = deliveryRequestDetails.UniqueOrderNo;
                    response.URLDetails = uRLDetails + "/Carrier/ScheduleBuilder";
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetTankDeliveryRequestNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        public async Task<NotificationOrderViewModel> GetOrderNotificationDetailsForCarrierEmail(int id)
        {
            var response = new NotificationOrderViewModel();
            try
            {
                var order = await Context.DataContext.Orders.Where(t => t.Id == id).Select(t => new
                {
                    t.Id,
                    BuyerCompanyName = t.BuyerCompany.Name,
                    SupplierCompanyName = t.Company.Name,
                    t.PoNumber,
                    OrderType = t.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery ? @Resource.lblSingle : @Resource.lblMultiple,
                    FuelType = t.FuelRequest.MstProduct.MstTFXProduct.Name,
                    OrderName = !string.IsNullOrEmpty(t.Name) ? t.Name : Resource.lblSingleHyphen,
                    JobId = t.FuelRequest.JobId,
                    JobName = t.FuelRequest.Job.Name,
                    TerminalName = t.TerminalId.HasValue ? t.MstExternalTerminal.Name : Resource.lblSingleHyphen,
                    DeliveryStartDate = t.FuelRequest.FuelRequestDetail.StartDate,
                    QuantityTypeId = t.FuelRequest.QuantityTypeId,
                    Quantity = t.FuelRequest.MaxQuantity,
                    AcceptedFirstName = t.User.FirstName,
                    AcceptedLastName = t.User.LastName,
                    SpecialInstructions = t.FuelRequest.SpecialInstructions.ToList()
                }).FirstOrDefaultAsync();

                if (order != null)
                {
                    response = new NotificationOrderViewModel();
                    response.BuyerCompanyName = order.BuyerCompanyName;
                    response.SupplierCompanyName = order.SupplierCompanyName;
                    response.PoNumber = order.PoNumber;
                    response.OrderType = order.OrderType;
                    response.FuelType = order.FuelType;
                    response.OrderName = order.OrderName;
                    response.JobId = order.JobId;
                    response.JobName = order.JobName;
                    response.TerminalName = order.TerminalName;
                    response.DeliveryStartDate = order.DeliveryStartDate.ToString(Resource.constFormatDate);

                    if (order.QuantityTypeId == (int)QuantityType.SpecificAmount || order.QuantityTypeId == (int)QuantityType.Range)
                    {
                        response.Quantity = order.Quantity.GetPreciseValue(2).GetCommaSeperatedValue();
                    }
                    else if (order.QuantityTypeId == (int)QuantityType.NotSpecified)
                    {
                        response.Quantity = Resource.lblNotSpecified;
                    }
                    if (order.SpecialInstructions.Count > 0)
                    {
                        response.SpecialInstructions = new List<string>();
                        foreach (var item in order.SpecialInstructions)
                        {
                            response.SpecialInstructions.Add(item.Instruction);
                        }
                    }

                    response.SupplierUser = new NotificationUserViewModel()
                    {
                        FirstName = order.AcceptedFirstName,
                        LastName = order.AcceptedLastName,
                    };
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetOrderNotificationDetailsForCarrierEmail", "Exception Details : ", ex);
            }
            return response;
        }

        #endregion

        #region Delivery Request Notifications
        public NotificationDeliveryRequestViewModel GetDeliveryRequestNotificationDetails(int id)
        {
            var response = new NotificationDeliveryRequestViewModel();
            try
            {
                var orderSchedule = Context.DataContext.OrderVersionXDeliverySchedules.SingleOrDefault(t => t.Id == id);
                if (orderSchedule != null)
                {
                    var order = orderSchedule.Order;
                    var currentVersion = orderSchedule.Version;
                    IEnumerable<DeliverySchedule> currentSchedules = null;
                    IEnumerable<DeliverySchedule> previousSchedules = new List<DeliverySchedule>();
                    var orderVersionsCount = order.OrderDeliverySchedules.Select(t => t.Version).Distinct().Count();
                    if (orderVersionsCount > 1)
                    {
                        var prevVersion = order.OrderDeliverySchedules.OrderByDescending(t => t.Version).FirstOrDefault(t => t.Version < currentVersion)?.Version;
                        var currSchedules = order.OrderDeliverySchedules.Where(t => t.Version == currentVersion).Select(t => t.DeliverySchedule);
                        var prevSchedules = order.OrderDeliverySchedules.Where(t => t.Version == prevVersion).Select(t => t.DeliverySchedule);

                        currentSchedules = currSchedules.Except(prevSchedules);
                        previousSchedules = prevSchedules.Except(currSchedules);
                    }
                    else
                    {
                        currentSchedules = order.OrderDeliverySchedules.Where(t => t.Version == currentVersion).Select(t => t.DeliverySchedule);
                    }

                    if (order.OrderXStatuses.FirstOrDefault(t => t.IsActive).StatusId == (int)OrderStatus.Open)
                    {
                        response = new NotificationDeliveryRequestViewModel()
                        {
                            Id = orderSchedule.Id,
                            OrderId = order.Id,
                            FuelRequestTypeId = order.FuelRequest.FuelRequestTypeId,
                            BuyerCompanyId = order.BuyerCompanyId,
                            BuyerCompanyName = order.BuyerCompany.Name,
                            SupplierCompanyName = order.Company.Name,
                            SupplierCompanyId = order.AcceptedCompanyId,
                            PoNumber = order.PoNumber
                        };
                        response.CurrentSchedules = currentSchedules
                        .GroupBy(t => t.GroupId).Select(g => new { Items = g.ToList() })
                        .Select(t => t.Items.ToViewModel()).Select(t => new DeliveryScheduleDetail
                        {
                            Date = t.ScheduleDate.ToString(Resource.constFormatDate),
                            DayNames = t.ScheduleDayNames,
                            Start = t.ScheduleStartTime,
                            End = t.ScheduleEndTime,
                            Gallons = t.ScheduleQuantity.GetPreciseValue(2).GetCommaSeperatedValue(),
                            GroupId = t.GroupId,
                            StatusId = t.StatusId,
                            Type = t.ScheduleType,
                            CreatedBy = t.CreatedBy,
                            RescheduledTrackableId = t.RescheduledTrackableId
                        });

                        if (previousSchedules.Any())
                        {
                            response.PreviousSchedules = previousSchedules
                            .GroupBy(t => t.GroupId).Select(g => new { Items = g.ToList() })
                            .Select(t => t.Items.ToViewModel()).Select(t => new DeliveryScheduleDetail
                            {
                                Date = t.ScheduleDate.ToString(Resource.constFormatDate),
                                DayNames = t.ScheduleDayNames,
                                Start = t.ScheduleStartTime,
                                End = t.ScheduleEndTime,
                                Gallons = t.ScheduleQuantity.GetPreciseValue(2).GetCommaSeperatedValue(),
                                GroupId = t.GroupId,
                                StatusId = t.StatusId,
                                Type = t.ScheduleType,
                                CreatedBy = t.CreatedBy,
                                RescheduledTrackableId = t.RescheduledTrackableId
                            });
                        }

                        var user = new NotificationUserViewModel()
                        {
                            Id = orderSchedule.User.Id,
                            Email = orderSchedule.User.Email,
                            FirstName = orderSchedule.User.FirstName,
                            LastName = orderSchedule.User.LastName
                        };
                        if (orderSchedule.User.Company == order.FuelRequest.User.Company)
                        {
                            response.UserRole = UserRoles.Buyer;
                            response.Buyer = user;
                            response.Supplier = new NotificationUserViewModel()
                            {
                                Id = order.User.Id,
                                Email = order.User.Email,
                                FirstName = order.User.FirstName,
                                LastName = order.User.LastName
                            };
                        }
                        else
                        {
                            response.UserRole = UserRoles.Supplier;
                            response.Supplier = user;
                            response.Buyer = new NotificationUserViewModel()
                            {
                                Id = order.FuelRequest.User.Id,
                                Email = order.FuelRequest.User.Email,
                                FirstName = order.FuelRequest.User.FirstName,
                                LastName = order.FuelRequest.User.LastName
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetDeliveryRequestNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        public List<int> GetOrdersForDeliverySchedule(int id)
        {
            return Context.DataContext.OrderVersionXDeliverySchedules
                                                            .Where(t => t.DeliveryRequestId == id &&
                                                                        t.IsActive)
                                                            .Select(t => t.OrderId)
                                                            .ToList();
        }

        public NotificationDeliveryRequestViewModel GetDeliveryReminderNotificationDetails(int id, int orderId)
        {
            using (var tracer = new Tracer("NotificationDomain", "GetDeliveryReminderNotificationDetails"))
            {
                var response = new NotificationDeliveryRequestViewModel();
                try
                {
                    var currentSchedules = Context.DataContext.OrderVersionXDeliverySchedules
                                                                 .Where(t => t.IsActive);
                    var orderVersionXSchedule = currentSchedules.SingleOrDefault(t => t.DeliveryRequestId == id && t.OrderId == orderId);
                    if (orderVersionXSchedule != null)
                    {
                        var deliveryRequestDetails = orderVersionXSchedule.DeliverySchedule;
                        var order = orderVersionXSchedule.Order;
                        if (order.OrderXStatuses.FirstOrDefault(t => t.IsActive).StatusId == (int)OrderStatus.Open)
                        {
                            DateTimeOffset estDate = deliveryRequestDetails.Date.Date;
                            DateTimeOffset nextDay = DateTimeOffset.Now.Date.AddDays(1);
                            if (estDate != nextDay)
                            {
                                if (deliveryRequestDetails.Type == (int)DeliveryScheduleType.Monthly)
                                {
                                    var nextDate = _helperDomain.GetNextDate(deliveryRequestDetails.Date, 30);
                                    if (nextDate.Date == nextDay.Date)
                                    {
                                        estDate = nextDay.Date;
                                    }
                                }
                                else if (deliveryRequestDetails.WeekDayId == ((int)DateTimeOffset.Now.DayOfWeek + 1))
                                {
                                    var nextDate = deliveryRequestDetails.Type == (int)DeliveryScheduleType.BiWeekly ?
                                                                                    _helperDomain.GetNextDate(deliveryRequestDetails.Date, 14) :
                                                                                    _helperDomain.GetNextDate(deliveryRequestDetails.Date, 7);
                                    if (nextDate.Date == nextDay.Date)
                                    {
                                        estDate = nextDay.Date;
                                    }
                                }
                            }
                            if (estDate == nextDay.Date)
                            {
                                var buyerUser = (order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? order.FuelRequest.CounterOffers.FirstOrDefault().User : order.FuelRequest.User);
                                var supplierUser = (order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest ? order.FuelRequest.CounterOffers.FirstOrDefault().User1 : order.User);
                                response = new NotificationDeliveryRequestViewModel()
                                {
                                    Id = deliveryRequestDetails.Id,
                                    OrderId = order.Id,
                                    BuyerCompanyId = order.BuyerCompanyId,
                                    BuyerCompanyName = order.BuyerCompany.Name,
                                    SupplierCompanyName = order.Company.Name,
                                    SupplierCompanyId = order.AcceptedCompanyId,
                                    PoNumber = order.PoNumber,
                                    Quantity = deliveryRequestDetails.Quantity.GetPreciseValue(6),
                                    Date = estDate.ToString(Resource.constFormatDate),
                                    Time = Convert.ToDateTime(deliveryRequestDetails.StartTime.ToString()).ToShortTimeString(),
                                    WeekDay = estDate.ToString("dddd"),
                                    Supplier = new NotificationUserViewModel()
                                    {
                                        Id = supplierUser.Id,
                                        Email = supplierUser.Email,
                                        FirstName = supplierUser.FirstName,
                                        LastName = supplierUser.LastName
                                    },
                                    Buyer = new NotificationUserViewModel()
                                    {
                                        Id = buyerUser.Id,
                                        Email = buyerUser.Email,
                                        FirstName = buyerUser.FirstName,
                                        LastName = buyerUser.LastName
                                    },
                                    SupplierContactNumber = supplierUser.PhoneNumber,
                                    UoM = deliveryRequestDetails.UoM
                                };
                                var driver = deliveryRequestDetails.DeliveryScheduleXDrivers.SingleOrDefault(t => t.IsActive);
                                response.DriverName = driver == null ? Resource.lblNoDriverAssigned : $"{driver.User.FirstName} {driver.User.LastName}";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("NotificationDomain", "GetDeliveryReminderNotificationDetails", "Exception Details : ", ex);
                }
                return response;
            }
        }
        #endregion

        #region Driver Assignment

        public NotificationDriverAssignedViewModel GetDriverDeliveryScheduleNotificationDetails(int entityId, bool IsAssigned)
        {
            var response = new NotificationDriverAssignedViewModel();
            try
            {
                var deliverySchedule = Context.DataContext.DeliverySchedules.Include(t => t.DeliveryScheduleXDrivers)
                    .Include("DeliveryScheduleXDrivers.User").SingleOrDefault(t => t.Id == entityId);
                if (deliverySchedule != null)
                {
                    var driver = deliverySchedule.DeliveryScheduleXDrivers.OrderByDescending(t => t.AssignedDate).First(t => t.IsActive == IsAssigned);
                    response.Driver.Id = driver.User.Id;
                    response.Driver.FirstName = driver.User.FirstName;
                    response.Driver.LastName = driver.User.LastName;
                    response.Driver.Email = driver.User.Email;

                    var orderVersionXDeliverySchedule = deliverySchedule.OrderVersionXDeliverySchedules.FirstOrDefault();
                    if (orderVersionXDeliverySchedule != null)
                    {
                        var orderDetails = orderVersionXDeliverySchedule.Order;
                        response.OrderId = orderDetails.Id;
                        response.PoNumber = orderDetails.PoNumber;

                        var supplierUser = orderDetails.User;

                        response.SupplierCompanyId = supplierUser.Company.Id;
                        response.SupplierCompanyName = supplierUser.Company.Name;
                        response.SupplierUser.Id = supplierUser.Id;
                        response.SupplierUser.FirstName = supplierUser.FirstName;
                        response.SupplierUser.LastName = supplierUser.LastName;
                        response.SupplierUser.Email = supplierUser.Email;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetDriverDeliveryScheduleNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        public NotificationDriverAssignedViewModel GetDriverOrderAssignmentNotificationDetails(int entityId, bool IsAssigned = true)
        {
            var response = new NotificationDriverAssignedViewModel();
            try
            {
                var order = Context.DataContext.Orders.Include(t => t.OrderXDrivers).Include("OrderXDrivers.User").SingleOrDefault(t => t.Id == entityId);
                if (order != null)
                {
                    var driver = order.OrderXDrivers.OrderByDescending(t => t.AssignedDate).FirstOrDefault(t => t.IsActive == IsAssigned);
                    if (driver != null)
                    {
                        response.Driver.Id = driver.User.Id;
                        response.Driver.FirstName = driver.User.FirstName;
                        response.Driver.LastName = driver.User.LastName;
                        response.Driver.Email = driver.User.Email;

                        response.OrderId = order.Id;
                        response.PoNumber = order.PoNumber;

                        var supplierUser = order.User;
                        response.SupplierCompanyId = supplierUser.Company.Id;
                        response.SupplierCompanyName = supplierUser.Company.Name;
                        response.SupplierUser.Id = supplierUser.Id;
                        response.SupplierUser.FirstName = supplierUser.FirstName;
                        response.SupplierUser.LastName = supplierUser.LastName;
                        response.SupplierUser.Email = supplierUser.Email;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetDriverOrderAssignmentNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        #endregion

        #region Credit App

        public NotificationCreditAppViewModel GetCreditAppNotificationDetails(int companyId, string buyerName)
        {
            var response = new NotificationCreditAppViewModel();
            try
            {
                var creditAppDetails = Context.DataContext.CreditAppDetails.SingleOrDefault(t => t.CompanyId == companyId);
                if (creditAppDetails != null)
                {
                    response.From = creditAppDetails.User.Email;
                    response.Subject = creditAppDetails.EmailSubject;
                    response.Body = ResourceMessages.GetMessage(Resource.emailCreditApp_Buyer_BodyText,
                        new[] { buyerName, creditAppDetails.EmailContent });
                }
                else
                {
                    var user = Context.DataContext.Companies.SingleOrDefault(t => t.Id == companyId).Users.FirstOrDefault(t => t.IsActive);
                    response.From = user.Email;
                    response.Subject = ResourceMessages.GetMessage(Resource.emailCreditApp_Buyer_SubjectText, new[] { user.Company.Name });
                    response.Body = string.Format(Resource.emailCreditApp_Buyer_BodyText, buyerName, ResourceMessages.GetMessage(Resource.txtCreditAppEmailBody, new[] { user.Email, $"{user.FirstName} {user.LastName}" }));
                }
                response.Body = response.Body.Replace("</br>", "<br>");
                var serverUrl = ContextFactory.Current.GetDomain<HelperDomain>().GetServerUrl();
                response.CompanyLogo = _helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.email_HeaderLogo);
                var documents = Context.DataContext.CreditAppDocuments.Where(t => t.CompanyId == companyId).ToList();
                response.Attachments = new List<System.Net.Mail.Attachment>();
                foreach (var document in documents)
                {
                    System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(document.FilePath);
                    attachment.Name = document.FileName;
                    response.Attachments.Add(attachment);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetCreditAppNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        #endregion

        public bool IsInvoiceCreatedWithExceedingQuantity(Invoice invoice)
        {
            var response = false;
            try
            {
                var order = invoice.Order;
                var fuelRemaining = (order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity) - order.Invoices.Where(t => t.Id != invoice.Id && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice).Sum(t => t.DroppedGallons);
                var remainingQuantity = !order.FuelRequest.IsOverageAllowed ? fuelRemaining : (fuelRemaining + ((order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity) * order.FuelRequest.OverageAllowedAmount) / 100);
                response = remainingQuantity < 0 || remainingQuantity < invoice.DroppedGallons;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "IsInvoiceCreatedWithExceedingQuantity", ex.Message, ex);
            }
            return response;
        }

        public bool IsInvoiceCreatedWithExceedingQuantity(List<Invoice> invoices)
        {
            var response = false;
            try
            {
                var order = invoices.Select(t => t.Order).FirstOrDefault();
                var invoiceIds = invoices.Select(t => t.Id).ToList();

                var fuelRemaining = (order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity) - order.Invoices.Where(t => !invoiceIds.Contains(t.Id) && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice).Sum(t => (t.UoM == UoM.MetricTons) ? (t.ConvertedQuantity ?? 0) : t.DroppedGallons);
                var remainingQuantity = !order.FuelRequest.IsOverageAllowed ? fuelRemaining : (fuelRemaining + ((order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity) * order.FuelRequest.OverageAllowedAmount) / 100);
                response = remainingQuantity < 0 || remainingQuantity < invoices.Sum(t => (t.UoM == UoM.MetricTons) ? (t.ConvertedQuantity ?? 0) : t.DroppedGallons);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "IsInvoiceCreatedWithExceedingQuantity", ex.Message, ex);
            }
            return response;
        }

        public string GetAddedScheduleDetails(IEnumerable<DeliveryScheduleDetail> CurrentSchedules)
        {
            return _helperDomain.GetAddedScheduleDetails(CurrentSchedules);
        }

        public string GetAddedScheduleDetailsForSms(IEnumerable<DeliveryScheduleDetail> CurrentSchedules)
        {
            return _helperDomain.GetAddedScheduleDetailsForSms(CurrentSchedules);
        }

        public string GetRescheduledScheduleDetails(IEnumerable<DeliveryScheduleDetail> CurrentSchedules)
        {
            return _helperDomain.GetRescheduledScheduleDetails(CurrentSchedules);
        }

        public string GetModifiedScheduleDetails(IEnumerable<DeliveryScheduleDetail> CurrentSchedules, IEnumerable<DeliveryScheduleDetail> PreviousSchedules)
        {
            return _helperDomain.GetModifiedScheduleDetails(CurrentSchedules, PreviousSchedules);
        }

        public NotificationQuoteViewModel GetQuoteNotificationDetails(int id, EventType eventType)
        {
            var response = new NotificationQuoteViewModel();
            try
            {
                var buyerQuote = Context.DataContext.QuoteRequests.FirstOrDefault(t => t.Id == id);
                if (buyerQuote != null)
                {
                    response.BuyerCompany = buyerQuote.User.Company.Name;
                    response.BuyerQuoteNumber = buyerQuote.RequestNumber;
                    response.QuoteId = buyerQuote.Id;
                    response.QuotesNeeded = buyerQuote.QuotesNeeded;
                    response.EndDate = buyerQuote.QuoteDueDate.ToString(Resource.constFormatDate);
                    response.QuotesReceived = buyerQuote.Quotations.Count(t => t.IsActive);
                }

                if (eventType == EventType.QuotationNotAwarded)
                {
                    response.SupplierEmail = Context.DataContext.Quotations.Where(t => t.QuoteRequestId == id &&
                                           t.QuotationStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)QuotationStatuses.Awarded).Select(t => t.User.Email).ToList();
                }
                else if (eventType == EventType.QuotationAwarded)
                {
                    var supplierQuotation = Context.DataContext.Quotations.FirstOrDefault(t => t.QuoteRequestId == id &&
                                       t.QuotationStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)QuotationStatuses.Awarded);
                    if (supplierQuotation != null)
                    {
                        response.SupplierFirstName = supplierQuotation.User.FirstName;
                        response.SupplierLastName = supplierQuotation.User.LastName;
                        response.SupplierEmail = new List<string> { supplierQuotation.User.Email };
                        response.SupplierQuoteNumber = supplierQuotation.QuoteNumber;
                        response.OrderId = supplierQuotation.FuelRequest.Orders.FirstOrDefault() != null ? supplierQuotation.FuelRequest.Orders.FirstOrDefault().Id : 0;
                    }
                }
                else if (eventType == EventType.QuoteRequestCreated)
                {
                    response.Suppliers = new List<NotificationUserViewModel>();
                    var companies = GetEligibleCompaniesForQuoteRequest(buyerQuote);
                    companies.ForEach(t => t.Users.Where(t2 => t2.IsActive
                                            && t2.MstRoles.Any(t3 => t3.Id == (int)UserRoles.Supplier || t3.Id == (int)UserRoles.Admin)
                                            && t2.UserXNotificationSettings.Any(t4 => t4.IsEmail && t4.EventTypeId == (int)EventType.QuoteRequestCreated)
                                            ).ToList().ForEach(
                                            t1 => response.Suppliers.Add(new NotificationUserViewModel()
                                            {
                                                Email = t1.Email,
                                                FirstName = t1.FirstName,
                                                LastName = t1.LastName,
                                                Id = t1.Id,
                                            }))
                                      );
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetQuoteNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        public bool IsPhoneNumberValid(string phoneNumber)
        {
            var appDomain = new ApplicationDomain();
            var response = false;
            var SmsSendingEnabled = appDomain.GetApplicationSettingValue<bool>(Constants.SmsSendingEnabled);
            if (SmsSendingEnabled)
            {
                var smsSendingCountryCode = appDomain.GetApplicationSettingValue<string>(Constants.SmsSendingCountryCode);
                var sendTo = $"{smsSendingCountryCode}{phoneNumber}";
                var smsModel = new ApplicationEventSmsNotificationViewModel
                {
                    To = new List<string> { sendTo },
                    SmsText = string.Empty,
                };

                smsModel.TwilioAccountSid = appDomain.GetApplicationSettingValue<string>(Constants.TwilioAccountSid);
                smsModel.TwilioAuthToken = appDomain.GetApplicationSettingValue<string>(Constants.TwilioAuthToken);
                smsModel.TwilioFromPhoneNumber = appDomain.GetApplicationSettingValue<string>(Constants.TwilioFromPhoneNumber);
                smsModel.Url = appDomain.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingSiteFuelExchangeUrl);
                response = Sms.GetClient().IsPhoneNumberValid(smsModel);
            }
            return response;
        }

        public bool IsNotificationExists(EventType eventType, DateTime date)
        {
            var notification = Context.DataContext.Notifications.Where(t => t.EventTypeId == (int)eventType).OrderByDescending(t => t.Id).FirstOrDefault();
            if (notification != null && notification.CreatedDate.HasValue && notification.CreatedDate.Value.Date.Date == date)
            {
                return true;
            }
            return false;
        }

        public string RemoveBodyContent(string bodyText, int i)
        {
            var end = "[e" + i + "]";
            var startIndex = bodyText.IndexOf("[s" + i + "]");
            if (startIndex >= 0)
            {
                var endIndex = bodyText.IndexOf(end) + end.Length;
                bodyText = bodyText.Remove(startIndex, endIndex - startIndex);
            }
            return bodyText;
        }

        public string ReplaceBodyContent(string bodyText, int i)
        {
            bodyText = bodyText.Replace("[s" + i + "]", string.Empty);
            bodyText = bodyText.Replace("[e" + i + "]", string.Empty);
            return bodyText;
        }

        public async Task<bool> ProcessDdtPendingToInvoiceNotifications()
        {
            var response = false;
            try
            {
                var ddtList = await (from invoice in Context.DataContext.Invoices
                                     where (invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp) &&
                                            (invoice.SupplierPreferredInvoiceTypeId == (int)InvoiceType.Manual || invoice.SupplierPreferredInvoiceTypeId == (int)InvoiceType.MobileApp) &&
                                             invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && invoice.IsActive
                                     select new NotificationPendingDdtToInvoiceConversionModel
                                     {
                                         DdtId = invoice.Id,
                                         DisplayInvoiceNumber = invoice.DisplayInvoiceNumber,
                                         JobId = invoice.Order.FuelRequest.JobId,
                                         JobName = invoice.Order.FuelRequest.Job.Name,
                                         BuyerCompanyId = invoice.Order.BuyerCompanyId,
                                         BuyerCompanyName = invoice.Order.FuelRequest.User.Company.Name,
                                         SupplierCompanyId = invoice.Order.AcceptedCompanyId,
                                         SupplierCompanyName = invoice.User.Company.Name,
                                         TimeZoneName = invoice.Order.FuelRequest.Job.TimeZoneName,
                                         CreatedBy = invoice.CreatedBy,
                                         InvoiceHeaderId = invoice.InvoiceHeaderId,
                                         CreatedDate = invoice.CreatedDate,
                                         DefaultNotificationPeriod = ApplicationConstants.DefaultNotificationPeriod
                                     })
                                     .Distinct().OrderByDescending(t => t.InvoiceHeaderId).ToListAsync();

                if (ddtList.Any())
                {
                    foreach (var ddt in ddtList)
                    {
                        var notificationPeriod = await Context.DataContext.OnboardingPreferences.Where(t => t.IsActive && t.CompanyId == ddt.SupplierCompanyId).Select(t => t.NotificationPeriod).FirstOrDefaultAsync();
                        var period = notificationPeriod == null || notificationPeriod <= 0 ? ApplicationConstants.DefaultNotificationPeriod : notificationPeriod.Value;
                        ddt.DefaultNotificationPeriod = period;
                        var currentTime = DateTimeOffset.Now.ToTargetDateTimeOffset(ddt.TimeZoneName);
                        var totalHrs = currentTime.Subtract(ddt.CreatedDate).TotalHours;

                        var notification = await Context.DataContext.Notifications.Where(t => t.EventTypeId == (int)EventType.DdtPendingToInvoiceNotification && t.EntityId == ddt.InvoiceHeaderId)
                                                                             .OrderByDescending(t => t.CreatedDate)
                                                                             .FirstOrDefaultAsync();
                        if (notification == null)
                        {
                            if (totalHrs > period)
                            {
                                var ddtJson = JsonConvert.SerializeObject(ddt);
                                await AddNotificationEventAsync(EventType.DdtPendingToInvoiceNotification, ddt.InvoiceHeaderId, (int)SystemUser.System, null, ddtJson);
                            }
                        }
                        else
                        {
                            totalHrs = currentTime.Subtract(notification.CreatedDate.Value).TotalHours;
                            if (totalHrs > period)
                            {
                                var ddtJson = JsonConvert.SerializeObject(ddt);
                                await AddNotificationEventAsync(EventType.DdtPendingToInvoiceNotification, ddt.InvoiceHeaderId, (int)SystemUser.System, null, ddtJson);
                            }
                        }
                    }
                }
                response = true;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "ProcessDdtPendingToInvoiceNotifications", ex.Message, ex);
            }

            return response;
        }

        public string GetCompanyName(int id)
        {
            string response = string.Empty;
            try
            {
                var company = Context.DataContext.Companies.FirstOrDefault(t => t.Id == id);
                if (company != null)
                {
                    response = $"{company.Name}";
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetCompanyName", ex.Message, ex);
            }
            return response;
        }

        public List<string> GetSuppliersForNominationAcknowledgement(int nominationId)
        {
            var suppliers = new List<string>();
            try
            {
                suppliers = Context.DataContext.Acknowledgements.Where(t => t.IsActive && !t.IsSent && t.EntityId == nominationId)
                                                                           .Select(t => t.Company.Name)
                                                                           .ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetSuppliersForNominationAcknowledgement", ex.Message, ex);
            }

            return suppliers;
        }

        public bool UpdateAcknowledgementSentStatus(int nominationId, bool isSent = false)
        {
            var response = false;
            try
            {
                var acknowledgement = Context.DataContext.Acknowledgements.Where(t => t.EntityId == nominationId)
                                                                          .FirstOrDefault();
                if (acknowledgement != null)
                {
                    acknowledgement.IsSent = isSent;
                    Context.DataContext.Entry(acknowledgement).State = EntityState.Modified;
                    Context.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "UpdateAcknowledgementSentStatus", ex.Message, ex);
            }

            return response;
        }

        public NotificationJobViewModel GetJobAttributeChangeDetails(int id, int userId)
        {
            var response = new NotificationJobViewModel();
            try
            {
                var jobDetails = Context.DataContext.Jobs.Where(t => t.Id == id)
                                .Select(t => new
                                {
                                    t.Id,
                                    t.Name,
                                    t.City,
                                    StateName = t.MstState.Name,
                                    t.ZipCode,
                                    t.CreatedBy,
                                    t.IsApprovalWorkflowEnabled,
                                    t.IsProFormaPoEnabled,
                                    t.InventoryDataCaptureType,
                                    t.UpdatedBy,
                                    Company = new
                                    {
                                        t.Company.Id,
                                        t.Company.Name
                                    },
                                    User = new
                                    {
                                        t.User.FirstName,
                                        t.User.LastName,
                                        t.User.Email
                                    },
                                    OnsitePersons = t.Users1.Select(t1 => new
                                    {
                                        t1.Id,
                                        t1.FirstName,
                                        t1.LastName,
                                        t1.Email,
                                        t1.IsActive
                                    }),
                                    AssignedTo = t.Users.Select(t1 => new
                                    {
                                        t1.Id,
                                        t1.FirstName,
                                        t1.LastName,
                                        t1.Email,
                                        t1.IsActive
                                    }),
                                    JobXApprovalUsers = t.JobXApprovalUsers.Select(t1 => new
                                    {
                                        t1.Id,
                                        t1.IsActive,
                                        t1.UserId,
                                        t1.User.FirstName,
                                        t1.User.LastName,
                                        t1.User.Email
                                    })
                                }).SingleOrDefault();
                if (jobDetails != null)
                {
                    response = new NotificationJobViewModel
                    {
                        Id = jobDetails.Id,
                        Name = jobDetails.Name,
                        Location = jobDetails.City + jobDetails.StateName + jobDetails.ZipCode,

                        CompanyId = jobDetails.Company.Id,
                        CompanyName = jobDetails.Company.Name
                    };

                    response.Creator.Id = jobDetails.CreatedBy;
                    response.Creator.Email = jobDetails.User.Email;
                    response.Creator.FirstName = jobDetails.User.FirstName;
                    response.Creator.LastName = jobDetails.User.LastName;

                    response.OnsitePersons = jobDetails.OnsitePersons.Where(t => t.IsActive).Select(t => new NotificationUserViewModel()
                    {
                        Email = t.Email,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        Id = t.Id
                    }).ToList();

                    response.AssignedTo = jobDetails.AssignedTo.Where(t => t.IsActive).Select(t => new NotificationUserViewModel()
                    {
                        Email = t.Email,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        Id = t.Id
                    }).ToList();


                    response.InventoryDataCaptureType = jobDetails.InventoryDataCaptureType;
                    if (jobDetails.JobXApprovalUsers.Any(t => t.IsActive))
                    {
                        var approver = jobDetails.JobXApprovalUsers.FirstOrDefault(t => t.IsActive);
                        response.ApproverUser.Id = approver.UserId;
                        response.ApproverUser.Email = approver.Email;
                        response.ApproverUser.FirstName = approver.FirstName;
                        response.ApproverUser.LastName = approver.LastName;
                    }

                    var previousUser = jobDetails.JobXApprovalUsers.OrderByDescending(t => t.Id).FirstOrDefault(t => !t.IsActive);
                    if (previousUser != null)
                    {
                        response.PreviousApprover.Id = previousUser.UserId;
                        response.PreviousApprover.Email = previousUser.Email;
                        response.PreviousApprover.FirstName = previousUser.FirstName;
                        response.PreviousApprover.LastName = previousUser.LastName;
                    }
                    var sender = Context.DataContext.Users.Where(t => t.Id == userId)
                                   .Select(t => new { t.FirstName, t.LastName }).SingleOrDefault();
                    response.CompanyName = $"{sender.FirstName} {sender.LastName}";

                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetJobNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }

        // will add bussiness logic in the below method. 
        public CarrierDeliveryNotificationViewModel GetCarrierDeliveryDetails(int timeout = 30)
        {
            CarrierDeliveryNotificationViewModel carrierDeliveryNotificationViewModel = new CarrierDeliveryNotificationViewModel();
            try
            {
                List<int> carrierCompanyIds = new List<int>();
                List<int> supplierCompanyIds = new List<int>();
                List<int> jobIds = new List<int>();
                List<int> TrackableSCId = new List<int>();
                var response = new List<CarrierXDeliveriesDetails>();
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierCarrierDeliveryXUserDetails", new { });
                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<CarrierXDeliveriesDetails>(input.Query, input.Params.ToArray()).ToList();
                if (response.Any())
                {
                    carrierCompanyIds = response.GroupBy(x => x.CarrierCompanyId).Select(x => x.Key).ToList();
                    supplierCompanyIds = response.GroupBy(x => x.SupplierCompanyId).Select(x => x.Key).ToList();

                    var carrierXEmailAddress = (from item in Context.DataContext.CarrierDeliveryXUserSettings
                                                join userxSetting in Context.DataContext.UserXNotificationSettings
                                                on item.UserId equals userxSetting.UserId
                                                where supplierCompanyIds.Contains(item.CompanyId) && userxSetting.EventTypeId == (int)EventType.CarrierDeliveries && userxSetting.IsEmail
                                                select new
                                                {
                                                    item.CompanyId,
                                                    item.UserId,
                                                    item.User.Email
                                                }
                                                    ).ToList();
                    jobIds = response.GroupBy(x => x.JobId).Select(x => x.Key).ToList();
                    TrackableSCId = response.Where(x => x.InvoiceId > 0).GroupBy(x => x.TrackableSCId).Select(x => x.Key).ToList();
                    //get carrier delivery request details.
                    var carrierDRDetails = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetCompanyDeliveryRequestsDetails(carrierCompanyIds)).Result;
                    //get carrier delivery request details.
                    var carrierTrackableScheduleDetails = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetDeliveryRequestDetails(TrackableSCId)).Result;
                    //get carrier delivery request details.
                    var jobsAssetInformation =
                         Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetJobsTankList(jobIds)).Result;
                    //carrier delivery failed records details by supplier wise.
                    foreach (var item in supplierCompanyIds)
                    {
                        //send email based on supplier company group.
                        var supplierDetails = response.FirstOrDefault(x => x.SupplierCompanyId == item);
                        if (supplierDetails != null)
                        {
                            CarrierDeliveryViewModel carrierDeliveryViewModel = new CarrierDeliveryViewModel();
                            carrierDeliveryViewModel.SupplierCompanyId = item;
                            carrierDeliveryViewModel.SupplierCompanyName = supplierDetails.SupplierCompanyName;
                            carrierDeliveryViewModel.ReportGenerateDate = response.FirstOrDefault(x => x.SupplierCompanyId == item).ReportDate;

                            var carrierDetails = response.Where(x => x.SupplierCompanyId == item).GroupBy(x => x.CarrierCompanyId).Select(x => x.Key).ToList();
                            if (carrierDetails.Any())
                            {
                                carrierDeliveryViewModel.CarrierCompanyIds.AddRange(carrierDetails);
                                foreach (var carrieritem in carrierDetails)
                                {
                                    CarrierDelXUserViewModel carrierDelXUserViewModel = new CarrierDelXUserViewModel();
                                    //Get Get Carrier No Of DeliveriesDetails
                                    GetCarrierNoOfDeliveriesDetails(response, carrieritem, carrierDelXUserViewModel, item);
                                    //Get CarrierXDelivery Failure
                                    GetCarrierXFailureReason(item, carrieritem, carrierDelXUserViewModel);
                                    //Get Carrier DR Information
                                    GetCarrierDRInformation(response, carrierDRDetails, carrieritem, carrierDelXUserViewModel);
                                    //Get Carrier Tank RunOut Information
                                    var carrierjobIds = response.Where(x => x.CarrierCompanyId == carrieritem).GroupBy(x => x.JobId).Select(x => x.Key).ToList();
                                    GetCarrierTankRunoutInfo(jobsAssetInformation, supplierDetails, carrierDelXUserViewModel, carrierjobIds);
                                    //Get Carrier Over/under deliveries
                                    GetCarrierOverUnderDeliveries(response, carrierDRDetails, carrieritem, supplierDetails, carrierTrackableScheduleDetails, jobsAssetInformation, carrierDelXUserViewModel, item);
                                    carrierDeliveryViewModel.CarrierDelXUserViewModel.Add(carrierDelXUserViewModel);
                                }
                            }
                            carrierDeliveryViewModel.EmailAddress = carrierXEmailAddress.Where(x => x.CompanyId == item).Select(x => x.Email).ToList();
                            carrierDeliveryNotificationViewModel.CarrierDeliveryViewModel.Add(carrierDeliveryViewModel);
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("NotificationDomain", "GetCarrierDeliveriesDetails", "Exception Details : ", ex);
            }
            return carrierDeliveryNotificationViewModel;
        }

        private void GetCarrierXFailureReason(int item, int carrieritem, CarrierDelXUserViewModel carrierDelXUserViewModel)
        {
            //carrierDelXUserViewModel.DeliveryUploadFailure
            List<DeliveryUploadFailure> deliveryUploadFailures = new List<DeliveryUploadFailure>();
            var date = DateTime.Now.AddDays(-7);
            var carrierXDeliveryFailuresDetails = Context.DataContext.CarrierXDeliveryFailures.Where(x => x.BuyerCompanyId != null && x.BuyerCompanyId.Value == item && x.SupplierCompanyId != null && x.SupplierCompanyId.Value == carrieritem && x.IsEndSupplier == 1 && DbFunctions.TruncateTime(x.CreatedDate) > date && x.RequestType == (int)CarrierXDeliveryFailureRequestType.TPDAPI).ToList();
            foreach (var carrierXDeliveryFailuresItem in carrierXDeliveryFailuresDetails)
            {

                var tpdInvoiceViewModel = JsonConvert.DeserializeObject<TPDInvoiceViewModel>(carrierXDeliveryFailuresItem.RequestJson);
                if (tpdInvoiceViewModel != null)
                {
                    int OrderId = 0;
                    if (!string.IsNullOrEmpty(carrierXDeliveryFailuresItem.FailureReason))
                    {
                        var failureReason = JsonConvert.DeserializeObject<List<ApiCodeMessages>>(carrierXDeliveryFailuresItem.FailureReason);
                        OrderId = failureReason.Select(x => x.OrderId.Value).FirstOrDefault();
                    }
                    if (tpdInvoiceViewModel.DropDetails.Any())
                    {
                        var dropDetails = tpdInvoiceViewModel.DropDetails.FirstOrDefault(x => x.OrderId == OrderId);
                        if (dropDetails != null)
                        {
                            DeliveryUploadFailure deliveryUploadFailure = new DeliveryUploadFailure();
                            deliveryUploadFailure.Carriername = carrierXDeliveryFailuresItem.SupplierCompany.Name;
                            deliveryUploadFailure.BOL = string.Join(",", dropDetails.BolDetails.Select(x1 => x1.BolNumber).ToList());
                            deliveryUploadFailure.ProductQty = dropDetails.TotalDropQuantity == 0 ? "N/A" : dropDetails.TotalDropQuantity.ToString();
                            deliveryUploadFailure.ProductType = string.IsNullOrEmpty(dropDetails.Product) ? "N/A" : dropDetails.Product;
                            if (tpdInvoiceViewModel.DropDetails.Any())
                            {
                                List<string> dropLocationName = new List<string> { tpdInvoiceViewModel.DropAddress1, tpdInvoiceViewModel.DropAddress2, tpdInvoiceViewModel.DropCity, tpdInvoiceViewModel.DropStateCode, tpdInvoiceViewModel.DropZip };
                                deliveryUploadFailure.DeliveryLocationName = string.Join(",", dropLocationName.Where(s => !String.IsNullOrEmpty(s)));
                            }

                            if (dropDetails.LiftDetails.Any() && dropDetails.LiftDetails.FirstOrDefault() != null)
                            {
                                var bulkPlanName = dropDetails.LiftDetails.FirstOrDefault().BulkPlantName;
                                if (!string.IsNullOrEmpty(bulkPlanName))
                                {
                                    List<string> pickupLocationName = new List<string> { bulkPlanName };
                                    deliveryUploadFailure.PickupLocationName = string.Join(",", pickupLocationName);
                                    deliveryUploadFailure.PickupLocationName = string.Join(",", pickupLocationName.Where(s => !String.IsNullOrEmpty(s)));
                                }
                                else
                                {
                                    List<string> pickupLocationName = new List<string> { dropDetails.LiftDetails.FirstOrDefault().LiftAddressStreet1, dropDetails.LiftDetails.FirstOrDefault().LiftAddressStreet2, dropDetails.LiftDetails.FirstOrDefault().LiftAddressCity, dropDetails.LiftDetails.FirstOrDefault().LiftAddressState, dropDetails.LiftDetails.FirstOrDefault().LiftAddressZip };
                                    deliveryUploadFailure.PickupLocationName = string.Join(",", pickupLocationName.Where(s => !String.IsNullOrEmpty(s)));
                                }
                                deliveryUploadFailure.LiftDate = dropDetails.LiftDetails.FirstOrDefault().LiftDate;
                            }
                            if (!string.IsNullOrEmpty(carrierXDeliveryFailuresItem.FailureReason))
                            {
                                var failureReason = JsonConvert.DeserializeObject<List<ApiCodeMessages>>(carrierXDeliveryFailuresItem.FailureReason);
                                if (failureReason.Any())
                                {
                                    var failureReasonList = failureReason.Select(x1 => x1.Message).ToList();
                                    deliveryUploadFailure.ReasonForFailure = string.Join(",", failureReasonList.Where(s => !String.IsNullOrEmpty(s)));
                                    deliveryUploadFailure.APIName = "Invoice-Create";
                                    deliveryUploadFailure.ExternalRefID = tpdInvoiceViewModel.ExternalRefID;
                                    deliveryUploadFailure.DateTime = carrierXDeliveryFailuresItem.CreatedDate.ToString(Resource.constFormatAPIExceptionDate);
                                    deliveryUploadFailure.Status = "Fail";
                                    deliveryUploadFailure.LocationID = tpdInvoiceViewModel.LocationId;
                                    if (tpdInvoiceViewModel.DropDetails.Any())
                                    {
                                        deliveryUploadFailure.DropDate = tpdInvoiceViewModel.DropDetails.FirstOrDefault().DropCompleteDate;
                                        deliveryUploadFailure.Request = carrierXDeliveryFailuresItem.RequestJson;
                                        deliveryUploadFailure.Response = deliveryUploadFailure.ReasonForFailure;
                                    }
                                    deliveryUploadFailures.Add(deliveryUploadFailure);
                                }
                            }

                        }
                    }
                }

            }
            carrierDelXUserViewModel.DeliveryUploadFailure = deliveryUploadFailures;
        }

        private static void GetCarrierOverUnderDeliveries(List<CarrierXDeliveriesDetails> response, CarrierXDeliveryRequestInfo carrierDRDetails, int carrieritem, CarrierXDeliveriesDetails supplierInfo, List<DeliveryRequestViewModel> carrierTrackableScheduleDetails, List<JobTankAdditionalDetailModel> jobsAssetInformation, CarrierDelXUserViewModel carrierDelXUserViewModel, int supplierCompanyId)
        {
            CarrierOverUnderDeliveryRequestInfo carrierOverUnderDeliveryRequestInfo = new CarrierOverUnderDeliveryRequestInfo();
            var carrierMissedDeliveryRequestDetails = carrierDRDetails.DeliveryRequestDetails.Where(x => x.AssignedToCompanyId == carrieritem && x.DelReqSource == (int)DRSource.MissedDR).ToList();
            carrierOverUnderDeliveryRequestInfo.TotalCount = response.Where(x => x.CarrierCompanyId == carrieritem && x.SupplierCompanyId == supplierCompanyId && x.DroppedGallons > 0 && x.DSQuantity > 0 && x.InvoiceId > 0).Count() + carrierMissedDeliveryRequestDetails.Count();
            carrierOverUnderDeliveryRequestInfo.OverDeliveries = response.Where(x => x.CarrierCompanyId == carrieritem && x.SupplierCompanyId == supplierCompanyId && x.DroppedGallons > 0 && x.DSQuantity > 0 && x.DroppedGallons > x.DSQuantity && x.InvoiceId > 0).Count();
            carrierOverUnderDeliveryRequestInfo.UnderDeliveries = response.Where(x => x.CarrierCompanyId == carrieritem && x.SupplierCompanyId == supplierCompanyId && x.DroppedGallons > 0 && x.DSQuantity > 0 && x.DroppedGallons < x.DSQuantity && x.InvoiceId > 0).Count();
            carrierOverUnderDeliveryRequestInfo.MissedDeliveries = carrierMissedDeliveryRequestDetails.Count();
            //add carrierUnderDeliveries delivery.
            var carrierUnderDeliveries = response.Where(x => x.CarrierCompanyId == carrieritem && x.DroppedGallons > 0 && x.DSQuantity > 0 && x.SupplierCompanyId == supplierCompanyId && x.DroppedGallons < x.DSQuantity && x.InvoiceId > 0).ToList();
            foreach (var item in carrierUnderDeliveries)
            {
                var carrierDSInfo = carrierTrackableScheduleDetails.FirstOrDefault(x => x.TrackableScheduleId == item.TrackableSCId);
                if (carrierDSInfo != null)
                {
                    var jobInfo = jobsAssetInformation.FirstOrDefault(x => x.SiteId == carrierDSInfo.SiteId && x.TankId == carrierDSInfo.TankId && x.StorageId == carrierDSInfo.StorageId);
                    CarrierOverUnderDeliveryRequestDetails carrierOverUnderDeliveryRequest = new CarrierOverUnderDeliveryRequestDetails();
                    carrierOverUnderDeliveryRequest.DeliveryType = 1;
                    carrierOverUnderDeliveryRequest.CustomerName = supplierInfo.SupplierCompanyName;
                    carrierOverUnderDeliveryRequest.LocationName = item.LocationName;
                    carrierOverUnderDeliveryRequest.RequiredQuantity = item.DSQuantity.ToString();
                    carrierOverUnderDeliveryRequest.ActualDeliveredQuantity = item.DroppedGallons.ToString();
                    carrierOverUnderDeliveryRequest.ProductType = carrierDSInfo.ProductType;
                    if (jobInfo != null)
                    {
                        carrierOverUnderDeliveryRequest.DaysRemaining = jobInfo.DaysRemaining;
                    }
                    else
                    {
                        carrierOverUnderDeliveryRequest.DaysRemaining = "N/A";
                    }
                    carrierOverUnderDeliveryRequestInfo.CarrierOverUnderDeliveryRequestDetails.Add(carrierOverUnderDeliveryRequest);
                }
                else
                {
                    var jobInfo = jobsAssetInformation.FirstOrDefault(x => x.JobId == item.JobId);
                    CarrierOverUnderDeliveryRequestDetails carrierOverUnderDeliveryRequest = new CarrierOverUnderDeliveryRequestDetails();
                    carrierOverUnderDeliveryRequest.DeliveryType = 1;
                    carrierOverUnderDeliveryRequest.CustomerName = supplierInfo.SupplierCompanyName;
                    carrierOverUnderDeliveryRequest.LocationName = item.LocationName;
                    carrierOverUnderDeliveryRequest.RequiredQuantity = item.DSQuantity.ToString();
                    carrierOverUnderDeliveryRequest.ActualDeliveredQuantity = item.DroppedGallons.ToString();
                    if (jobInfo != null)
                    {
                        carrierOverUnderDeliveryRequest.ProductType = jobInfo.TfxProductTypeName;
                        carrierOverUnderDeliveryRequest.DaysRemaining = jobInfo.DaysRemaining;
                    }
                    else
                    {
                        carrierOverUnderDeliveryRequest.DaysRemaining = "N/A";
                    }
                    carrierOverUnderDeliveryRequestInfo.CarrierOverUnderDeliveryRequestDetails.Add(carrierOverUnderDeliveryRequest);
                }
            }
            //add carrierOverDeliveries delivery.
            var carrierOverDeliveries = response.Where(x => x.CarrierCompanyId == carrieritem && x.DroppedGallons > 0 && x.DSQuantity > 0 && x.SupplierCompanyId == supplierCompanyId && x.DroppedGallons > x.DSQuantity && x.InvoiceId > 0).ToList();
            foreach (var item in carrierOverDeliveries)
            {
                var carrierDSInfo = carrierTrackableScheduleDetails.FirstOrDefault(x => x.TrackableScheduleId == item.TrackableSCId);
                if (carrierDSInfo != null)
                {
                    var jobInfo = jobsAssetInformation.FirstOrDefault(x => x.SiteId == carrierDSInfo.SiteId && x.TankId == carrierDSInfo.TankId && x.StorageId == carrierDSInfo.StorageId);
                    CarrierOverUnderDeliveryRequestDetails carrierOverUnderDeliveryRequest = new CarrierOverUnderDeliveryRequestDetails();
                    carrierOverUnderDeliveryRequest.DeliveryType = 2;
                    carrierOverUnderDeliveryRequest.CustomerName = supplierInfo.SupplierCompanyName;
                    carrierOverUnderDeliveryRequest.LocationName = item.LocationName;
                    carrierOverUnderDeliveryRequest.RequiredQuantity = item.DSQuantity.ToString();
                    carrierOverUnderDeliveryRequest.ActualDeliveredQuantity = item.DroppedGallons.ToString();
                    carrierOverUnderDeliveryRequest.ProductType = carrierDSInfo.ProductType;
                    if (jobInfo != null)
                    {
                        carrierOverUnderDeliveryRequest.DaysRemaining = jobInfo.DaysRemaining;
                    }
                    else
                    {
                        carrierOverUnderDeliveryRequest.DaysRemaining = "N/A";
                    }
                    carrierOverUnderDeliveryRequestInfo.CarrierOverUnderDeliveryRequestDetails.Add(carrierOverUnderDeliveryRequest);
                }
                else
                {
                    var jobInfo = jobsAssetInformation.FirstOrDefault(x => x.JobId == item.JobId);
                    CarrierOverUnderDeliveryRequestDetails carrierOverUnderDeliveryRequest = new CarrierOverUnderDeliveryRequestDetails();
                    carrierOverUnderDeliveryRequest.DeliveryType = 2;
                    carrierOverUnderDeliveryRequest.CustomerName = supplierInfo.SupplierCompanyName;
                    carrierOverUnderDeliveryRequest.LocationName = item.LocationName;
                    carrierOverUnderDeliveryRequest.RequiredQuantity = item.DSQuantity.ToString();
                    carrierOverUnderDeliveryRequest.ActualDeliveredQuantity = item.DroppedGallons.ToString();
                    if (jobInfo != null)
                    {
                        carrierOverUnderDeliveryRequest.ProductType = jobInfo.TfxProductTypeName;
                        carrierOverUnderDeliveryRequest.DaysRemaining = jobInfo.DaysRemaining;
                    }
                    else
                    {
                        carrierOverUnderDeliveryRequest.DaysRemaining = "N/A";
                    }
                    carrierOverUnderDeliveryRequestInfo.CarrierOverUnderDeliveryRequestDetails.Add(carrierOverUnderDeliveryRequest);
                }
            }

            //add missed delivery.
            foreach (var item in carrierMissedDeliveryRequestDetails)
            {
                var carrierDSInfo = carrierTrackableScheduleDetails.FirstOrDefault(x => x.TrackableScheduleId == item.TrackableScheduleId);
                if (carrierDSInfo != null)
                {
                    var jobInfo = jobsAssetInformation.FirstOrDefault(x => x.SiteId == carrierDSInfo.SiteId && x.TankId == carrierDSInfo.TankId && x.StorageId == carrierDSInfo.StorageId);
                    CarrierOverUnderDeliveryRequestDetails carrierOverUnderDeliveryRequest = new CarrierOverUnderDeliveryRequestDetails();
                    carrierOverUnderDeliveryRequest.DeliveryType = 3;
                    carrierOverUnderDeliveryRequest.CustomerName = supplierInfo.SupplierCompanyName;
                    carrierOverUnderDeliveryRequest.LocationName = item.JobName;
                    carrierOverUnderDeliveryRequest.RequiredQuantity = item.RequiredQuantity.ToString();
                    carrierOverUnderDeliveryRequest.ActualDeliveredQuantity = "N/A";
                    carrierOverUnderDeliveryRequest.ProductType = carrierDSInfo.ProductType;
                    if (jobInfo != null)
                    {
                        carrierOverUnderDeliveryRequest.DaysRemaining = jobInfo.DaysRemaining;
                    }
                    else
                    {
                        carrierOverUnderDeliveryRequest.DaysRemaining = "N/A";
                    }
                    carrierOverUnderDeliveryRequestInfo.CarrierOverUnderDeliveryRequestDetails.Add(carrierOverUnderDeliveryRequest);
                }
                else
                {
                    var jobInfo = jobsAssetInformation.FirstOrDefault(x => x.JobId == item.JobId);
                    CarrierOverUnderDeliveryRequestDetails carrierOverUnderDeliveryRequest = new CarrierOverUnderDeliveryRequestDetails();
                    carrierOverUnderDeliveryRequest.DeliveryType = 3;
                    carrierOverUnderDeliveryRequest.CustomerName = supplierInfo.SupplierCompanyName;
                    carrierOverUnderDeliveryRequest.LocationName = item.JobName;
                    carrierOverUnderDeliveryRequest.RequiredQuantity = item.RequiredQuantity.ToString();
                    carrierOverUnderDeliveryRequest.ActualDeliveredQuantity = "N/A";
                    if (jobInfo != null)
                    {
                        carrierOverUnderDeliveryRequest.ProductType = jobInfo.TfxProductTypeName;
                        carrierOverUnderDeliveryRequest.DaysRemaining = jobInfo.DaysRemaining;
                    }
                    else
                    {
                        carrierOverUnderDeliveryRequest.DaysRemaining = "N/A";
                    }
                    carrierOverUnderDeliveryRequestInfo.CarrierOverUnderDeliveryRequestDetails.Add(carrierOverUnderDeliveryRequest);
                }

            }

            carrierDelXUserViewModel.CarrierOverUnderDeliveryRequestInfo = carrierOverUnderDeliveryRequestInfo;
        }

        private void GetCarrierNoOfDeliveriesDetails(List<CarrierXDeliveriesDetails> response, int carrieritem, CarrierDelXUserViewModel carrierDelXUserViewModel, int supplierCompanyId)
        {
            var appDomain = new ApplicationDomain();
            var siteFuelExchangeUrl = appDomain.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingSiteFuelExchangeUrl);
            var reportDate = response.FirstOrDefault(x => x.CarrierCompanyId == carrieritem && x.SupplierCompanyId == supplierCompanyId).ReportDate;
            carrierDelXUserViewModel.URL = siteFuelExchangeUrl + "/Supplier/Invoice/View?supplierCompanyId=" + supplierCompanyId + "&carrierCompanyId=" + carrieritem + "&ReportDate=" + reportDate;
            carrierDelXUserViewModel.Name = response.FirstOrDefault(x => x.CarrierCompanyId == carrieritem && x.SupplierCompanyId == supplierCompanyId).CarrierCompanyName;
            carrierDelXUserViewModel.TotalDSCount = response.Where(x => x.CarrierCompanyId == carrieritem && x.DroppedGallons > 0 && x.SupplierCompanyId == supplierCompanyId).Count();
            carrierDelXUserViewModel.TotalQty = response.Where(x => x.CarrierCompanyId == carrieritem && x.DroppedGallons > 0 && x.SupplierCompanyId == supplierCompanyId).Sum(x1 => x1.DroppedGallons);
            var companyUOMInfo = Context.DataContext.CompanyAddresses.FirstOrDefault(x => x.CompanyId == carrieritem && x.IsActive && x.IsDefault);
            if (companyUOMInfo != null)
            {
                carrierDelXUserViewModel.UoM = companyUOMInfo.MstCountry.DefaultUoM;
            }
        }

        private static void GetCarrierTankRunoutInfo(List<JobTankAdditionalDetailModel> jobsAssetInformation, CarrierXDeliveriesDetails supplierDetails, CarrierDelXUserViewModel carrierDelXUserViewModel, List<int> carrierjobIds)
        {
            if (carrierjobIds.Any())
            {
                jobsAssetInformation = jobsAssetInformation.Where(x => x.ISRunOut).ToList();
                var carrierJobInformation = jobsAssetInformation.Where(x => carrierjobIds.Contains(x.JobId) && x.ISRunOut).ToList();
                if (carrierJobInformation.Any())
                {
                    //get tank rounout details.
                    List<CarrierTankRunOutInfo> carrierTankRunOutInfos = new List<CarrierTankRunOutInfo>();
                    carrierJobInformation.ForEach(x => carrierTankRunOutInfos.Add(new CarrierTankRunOutInfo { CustomerName = supplierDetails.SupplierCompanyName, LocationName = x.JobName, ProductType = x.TfxProductTypeName, StorageId = x.StorageId, TankName = x.TankName, StorageTypeId = x.TankId }));
                    carrierDelXUserViewModel.CarrierTankRunOutInfo = carrierTankRunOutInfos;
                }
            }
        }

        private static void GetCarrierDRInformation(List<CarrierXDeliveriesDetails> response, CarrierXDeliveryRequestInfo carrierDRDetails, int carrieritem, CarrierDelXUserViewModel carrierDelXUserViewModel)
        {

            //fetch the DR count by priority with information:
            if (carrierDRDetails != null && carrierDRDetails.DeliveryRequestDetails.Any())
            {
                CarrierDeliveryRequestInfo carrierDeliveryRequestInfo = new CarrierDeliveryRequestInfo();
                var carrierDeliveryRequestDetails = carrierDRDetails.DeliveryRequestDetails.Where(x => x.AssignedToCompanyId == carrieritem && x.DelReqSource != (int)DRSource.MissedDR).ToList();
                if (carrierDeliveryRequestDetails.Any())
                {
                    carrierDeliveryRequestDetails.ForEach(x => carrierDeliveryRequestInfo.CarrierDeliveryRequestDetails.Add(new CarrierDeliveryRequestDetails { AssignedToMe = false, CustomerName = x.CustomerCompany, LocationName = x.JobName, ProductType = x.ProductType, Quantity = x.UoM == (int)UoM.Gallons ? x.RequiredQuantity + "" + UoM.Gallons.ToString() : x.RequiredQuantity + "" + UoM.Litres.ToString(), Priority = (int)x.Priority }));
                }
                var assignedByMeDeliveryRequestDetails = carrierDRDetails.AssignedByMeDeliveryRequestDetails.Where(x => x.AssignedToCompanyId == carrieritem).ToList();
                if (assignedByMeDeliveryRequestDetails.Any())
                {
                    assignedByMeDeliveryRequestDetails.ForEach(x => carrierDeliveryRequestInfo.CarrierDeliveryRequestDetails.Add(new CarrierDeliveryRequestDetails { AssignedToMe = true, CustomerName = x.CustomerCompany, LocationName = x.JobName, ProductType = x.ProductType, Quantity = x.UoM == (int)UoM.Gallons ? x.RequiredQuantity + "" + UoM.Gallons.ToString() : x.RequiredQuantity + "" + UoM.Litres.ToString(), Priority = (int)x.Priority }));
                }
                carrierDeliveryRequestInfo.MustGo = carrierDeliveryRequestInfo.CarrierDeliveryRequestDetails.Where(x => x.AssignedToMe == false && x.Priority == 1).Count();
                carrierDeliveryRequestInfo.ShouldGo = carrierDeliveryRequestInfo.CarrierDeliveryRequestDetails.Where(x => x.AssignedToMe == false && x.Priority == 2).Count();
                carrierDeliveryRequestInfo.CouldGo = carrierDeliveryRequestInfo.CarrierDeliveryRequestDetails.Where(x => x.AssignedToMe == false && x.Priority == 3).Count();
                carrierDeliveryRequestInfo.AssignedToMe = carrierDeliveryRequestInfo.CarrierDeliveryRequestDetails.Where(x => x.AssignedToMe == true).Count();
                carrierDelXUserViewModel.CarrierDeliveryRequestInfo = carrierDeliveryRequestInfo;
            }
        }
        public InvitedUserNotifyViewModel GetInvitedUser(int id)
        {
            var response = new InvitedUserNotifyViewModel();
            try
            {
                var addedUser = Context.DataContext.InvitedUsers.Where(t => t.Id == id)
                                .Select(t => new
                                {
                                    t.Id,
                                    t.Title,
                                    t.FirstName,
                                    t.LastName,
                                    t.Email,
                                    t.InvitedBy,
                                    t.CompanyId,
                                    Company = new
                                    {
                                        //t.User.FirstName,
                                        //t.User.LastName,
                                        //t.User.CompanyId,
                                        //CompanyName = t.User.Company.Name
                                        t.Company.Name,
                                        t.Company.Id

                                    },
                                }).SingleOrDefault();
                if (addedUser != null)
                {
                    response = new InvitedUserNotifyViewModel()
                    {
                        User = new NotificationUserViewModel
                        {
                            Id = addedUser.Id,
                            FirstName = addedUser.FirstName,
                            LastName = addedUser.LastName,
                            Email = addedUser.Email,
                        },
                        InvitedByName = addedUser.Company.Name,
                        InvitedCompanyId = addedUser.Company.Id,
                        InvitedCompanyName = addedUser.Company.Name,
                    };
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetInvitedUser", "Exception Details : ", ex);
            }
            return response;
        }

        public InvitedUserNotificationViewModel GetInvitedUserAdded(int id)
        {
            var response = new InvitedUserNotificationViewModel();
            try
            {
                var addedUser = Context.DataContext.InvitedUsers.Where(t => t.Id == id)
                                .Select(t => new
                                {
                                    t.Id,
                                    t.FirstName,
                                    t.LastName,
                                    t.Email,
                                    User = new
                                    {
                                        t.User.FirstName,
                                        t.User.LastName,
                                        t.User.CompanyId,
                                        CompanyName = t.User.Company.Name
                                    },
                                    Roles = t.MstRoles.Select(t1 => t1.Name)
                                }).SingleOrDefault();
                if (addedUser != null)
                {
                    response = new InvitedUserNotificationViewModel()
                    {
                        User = new NotificationUserViewModel
                        {
                            Id = addedUser.Id,
                            FirstName = addedUser.FirstName,
                            LastName = addedUser.LastName,
                            Email = addedUser.Email,
                        },
                        InvitedByName = $"{addedUser.User.FirstName} {addedUser.User.LastName}",
                        InvitedCompanyId = addedUser.User.CompanyId.Value,
                        InvitedCompanyName = addedUser.User.CompanyName,
                        Roles = addedUser.Roles.ToList(),
                        PersonalMessage = string.Empty,
                    };
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetInvitedUserAdded", "Exception Details : ", ex);
            }
            return response;
        }

        public InvitedUserNotificationViewModel GetNewInvitedUserDetails(int id)
        {
            var response = new InvitedUserNotificationViewModel();
            try
            {
                var addedUser = Context.DataContext.ThirdPartyCompanyInvites.Where(t => t.Id == id).FirstOrDefault();
                if (addedUser.UserInfo != null)
                {
                    var UserInfo = JsonConvert.DeserializeObject<UserInfo>(addedUser.UserInfo);
                    if (UserInfo != null)
                    {
                        response = new InvitedUserNotificationViewModel()
                        {
                            User = new NotificationUserViewModel
                            {
                                Id = id,
                                Title = UserInfo.Title,
                                FirstName = UserInfo.FirstName,
                                LastName = UserInfo.LastName,
                                Email = UserInfo.Email,
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetNewInvitedUserDetails", "Exception Details : ", ex);
            }
            return response;
        }

        public async Task<List<LfRecordsCarrierReportsViewModel>> GetLFVDashboardData()
        {
            var response = new List<LfRecordsCarrierReportsViewModel>();
            List<int> CompanyIds = Context.DataContext.LiftFileDetails.Where(t => t.IsActive)
                                      .Select(t => t.CompanyId)
                                      .Distinct()
                                      .ToList();
            foreach (var companyId in CompanyIds)
            {
                // call report creation method  per company 
                var LFVRecords = await CreateLiftFileReportPerCompany(companyId);
                if (LFVRecords.Any())
                {
                    response.AddRange(LFVRecords);
                }
            }
            return response;
        }
        private async Task<List<LfRecordsCarrierReportsViewModel>> CreateLiftFileReportPerCompany(int companyId)
        {
            var response = new List<LfRecordsCarrierReportsViewModel>();
            try
            {
                List<LFRecordsGridViewModel> records = new List<LFRecordsGridViewModel>();
                var lfvParameter = GetLfvRequiredParameters(companyId);
                var daysToContinueMatchProcess = lfvParameter != null && lfvParameter.DaysToContinueMatchProcess > 0 ? lfvParameter.DaysToContinueMatchProcess : ApplicationConstants.DefaultNoMatchRecordDays;

                var startDate = DateTimeOffset.Now.AddDays(-daysToContinueMatchProcess);
                var endDate = DateTimeOffset.Now.AddDays(-1);
                records = await new StoredProcedureDomain(this).GetLfRecordsByDateTimeWindow(companyId, startDate.Date, endDate.Date);
                if (records != null && records.Any())
                {
                    records = records.Where(x => x.Status != (int)LFVRecordStatus.None && x.Status != (int)LFVRecordStatus.Clean && x.Status != (int)LFVRecordStatus.PendingMatch).ToList();
                    response = records.Select(t => t.ToCarrierLFVCsvViewModel()).ToList();
                    response.ForEach(x => x.CompanyId = companyId);
                    var mailingList = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingLiftFileReportMailingList).Select(t => t.Value).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(mailingList))
                    {

                        var emails = mailingList.Split(';').ToList();
                        response.ForEach(x => x.EmailList.AddRange(emails));
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LFVDomain", "CreateLiftFileReportPerCompany", ex.Message, ex);
            }
            return response;
        }
        private LfvParameterViewModel GetLfvRequiredParameters(int liftFileParamsForCompany)
        {
            var requiredLfvParams = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == liftFileParamsForCompany
                                                && t.IsActive && t.IsLiftFileValidationEnabled)
                                        .Select(t => t.LiftFileValidationParameters)
                                        .FirstOrDefault();

            if (requiredLfvParams != null)
            {
                var inputParameters = requiredLfvParams.Where(t => t.ParameterType == LFVParameterType.Input && t.IsActive).FirstOrDefault();
                if (inputParameters != null)
                {
                    return inputParameters.ToViewModel();
                }
            }

            return null;
        }
        public List<TerminalMappingDetails> GetTerminalMappingDetails(List<string> TerminalIds)
        {
            var uniqueTerminalMappingDetails = Context.DataContext.TerminalCompanyAliases.Where(x => TerminalIds.Contains(x.AssignedTerminalId) && x.IsActive == true).GroupBy(x => x.AssignedTerminalId).Where(g => g.Count() == 1).Select(x => x.Key).ToList();
            var terminalMapping = Context.DataContext.TerminalCompanyAliases.Where(x => uniqueTerminalMappingDetails.Contains(x.AssignedTerminalId)).Select(x => new TerminalMappingDetails
            {
                TerminalId = x.AssignedTerminalId,
                TerminalORBulkPlanName = x.IsBulkPlant == false ? x.MstExternalTerminal != null ? x.MstExternalTerminal.Name : string.Empty : x.BulkPlantLocation != null ? x.BulkPlantLocation.Name : string.Empty
            }).ToList();
            return terminalMapping;
        }
        public List<CarrierMappingDetails> GetCarrierMappingDetails(List<string> TerminalIds, List<string> CarrierIds)
        {
            var uniqueCarrierMappingDetails = Context.DataContext.CarrierMappings.Where(x => CarrierIds.Contains(x.AssignedCarrierId) && x.TerminalCompanyAlias != null && TerminalIds.Contains(x.TerminalCompanyAlias.AssignedTerminalId) && x.IsActive == true).GroupBy(x => new { x.AssignedCarrierId, x.TerminalCompanyAlias.AssignedTerminalId, x.CarrierName }).Where(g => g.Count() == 1).Select(x => new CarrierMappingDetails { CarrierId = x.Key.AssignedCarrierId, TerminalId = x.Key.AssignedTerminalId, CarrierName = x.Key.CarrierName }).ToList();
            return uniqueCarrierMappingDetails;
        }
        public async Task<NotificationDeliveryRequestViewModel> GetBlendedTankDeliveryRequestNotificationDetails(string blendedGroupId)
        {
            var response = new NotificationDeliveryRequestViewModel();
            try
            {
                var uRLDetails = await Context.DataContext.MstAppSettings.Where(x => x.Key == "SiteFuelExchangeUrl").Select(x => x.Value).FirstOrDefaultAsync();
                var freightServiceDomain = new FreightServiceDomain(this);
                List<DeliveryRequestViewModel> blendedDRsInfo = await freightServiceDomain.GetBlendedGroupDeliveryRequestDetails(new List<string> { blendedGroupId });
                blendedDRsInfo = blendedDRsInfo.Where(x => !string.IsNullOrEmpty(x.BrokeredParentId)).ToList();
                if (blendedDRsInfo.Any())
                {
                    var deliveryRequestDetails = blendedDRsInfo.FirstOrDefault();
                    bool isWithoutTank = false;

                    if (deliveryRequestDetails != null && string.IsNullOrEmpty(deliveryRequestDetails.TankId) && deliveryRequestDetails.DeliveryRequestFor != DeliveryRequestFor.ProductType)
                    {
                        if (deliveryRequestDetails.OrderId > 0)
                        {
                            isWithoutTank = true;
                            var order = await Context.DataContext.Orders.Where(t => t.Id == deliveryRequestDetails.OrderId).Select(t => new
                            {
                                BuyerCompanyName = t.BuyerCompany.Name,
                                FuelType = t.FuelRequest.MstProduct.MstTFXProduct.Name,
                            }).FirstOrDefaultAsync();

                            if (order != null)
                            {
                                response = new NotificationDeliveryRequestViewModel();
                                response.BuyerCompanyName = order.BuyerCompanyName;
                                response.FuelType = order.FuelType;
                                response.ProductType = deliveryRequestDetails.ProductType;
                                response.JobName = deliveryRequestDetails.JobName;
                                response.JobId = deliveryRequestDetails.JobId;
                                response.QuantityId = deliveryRequestDetails.ScheduleQuantityType;
                                response.Quantity = deliveryRequestDetails.RequiredQuantity;
                                response.UoM = (UoM)deliveryRequestDetails.UoM;
                                response.UniqueOrderNo = deliveryRequestDetails.UniqueOrderNo;
                                response.URLDetails = uRLDetails + "/Carrier/ScheduleBuilder";
                                response.TankName = "NA";
                            }
                        }
                    }

                    if (deliveryRequestDetails != null && !isWithoutTank)
                    {
                        response = new NotificationDeliveryRequestViewModel();
                        response.BuyerCompanyName = deliveryRequestDetails.CustomerCompany;
                        response.ProductType = deliveryRequestDetails.ProductType;
                        response.JobName = deliveryRequestDetails.JobName;
                        response.JobId = deliveryRequestDetails.JobId;
                        response.QuantityId = deliveryRequestDetails.ScheduleQuantityType;
                        response.Quantity = deliveryRequestDetails.RequiredQuantity;
                        response.UoM = (UoM)deliveryRequestDetails.UoM;
                        response.FuelType = "NA";

                        if (deliveryRequestDetails.DeliveryRequestFor != DeliveryRequestFor.ProductType)
                        {
                            var assetDetails = await Context.DataContext.JobXAssets.Where(t => t.JobId == deliveryRequestDetails.JobId
                                                    && t.Asset.AssetAdditionalDetail.Vendor == deliveryRequestDetails.StorageId
                                                    && t.Asset.AssetAdditionalDetail.VehicleId == deliveryRequestDetails.TankId
                                                    && t.RemovedBy == null && t.RemovedDate == null).
                                               Select(t => new
                                               {
                                                   TankName = t.Asset.Name
                                               }).FirstOrDefaultAsync();

                            if (assetDetails != null)
                            {
                                response.TankName = assetDetails.TankName;
                            }
                        }
                        response.UniqueOrderNo = deliveryRequestDetails.UniqueOrderNo;
                        response.URLDetails = uRLDetails + "/Carrier/ScheduleBuilder";
                    }
                    blendedDRsInfo.ForEach(x =>
                    {
                        string quantity = string.Empty;
                        if (x.ScheduleQuantityType == 0 || x.ScheduleQuantityType == (int)ScheduleQuantityType.Quantity)
                            quantity = x.RequiredQuantity + " " + (UoM)x.UoM;
                        else
                            quantity = EnumHelperMethods.GetDisplayName((ScheduleQuantityType)x.ScheduleQuantityType);

                        response.IsBlendedRequest = true;
                        response.BlendedProductDetails.Add(new BlendedProductDetails { FuelType = x.FuelType, ProductType = x.ProductType, Quantity = quantity });
                    });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "GetBlendedTankDeliveryRequestNotificationDetails", "Exception Details : ", ex);
            }
            return response;
        }
    }
}
