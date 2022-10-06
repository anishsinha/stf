using FileHelpers;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Job;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using SiteFuel.Exchange.ViewModels.Queue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SiteFuel.Exchange.Domain
{
    public class JobDomain : BaseDomain
    {
        public JobDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public JobDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public async Task<JobStepsViewModel> GetJobStepsAsync(int jobId = 0, int companyId = 0)
        {
            JobStepsViewModel response = new JobStepsViewModel();
            try
            {
                var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.IsActive && t.Id == jobId);
                if (job != null)
                {
                    response.Job = job.ToViewModel();
                    response.CompanyId = job.CompanyId;
                    response.CompanyTypeId = job.Company.CompanyTypeId;
                    response.Culture = new HelperDomain().SetEntityThreadCulture(job.Currency);
                    response.ContactPersons = job.CompanyXAdditionalUserInvites.Select(t => t.ToViewModel()).ToList();
                    response.Subcontractors = job.Subcontractors.Select(t => t.ToViewModel()).ToList();
                    if (job.JobBudget != null)
                    {
                        response.JobBudget = job.JobBudget.ToViewModel();
                        response.Job.IsAssetTracked = job.JobBudget.IsAssetTracked;
                        response.Job.IsAssetDropStatusEnabled = job.JobBudget.IsAssetDropStatusEnabled;
                    }
                    var invoices = Context.DataContext.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice && t.Order != null && t.Order.FuelRequest.Job.Id == jobId);
                    if (invoices.Any(t => t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.WaitingForApproval))
                        response.Job.IsWaitingForApprovalExists = true;
                    if (!response.Job.IsWaitingForApprovalExists)
                    {
                        InvoiceDomain invoiceDomain = new InvoiceDomain(this);
                        foreach (var invoice in invoices.Where(t => t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.Rejected))
                        {
                            if (invoiceDomain.GetInvoicePreviousStatus(invoice))
                            {
                                response.Job.IsWaitingForApprovalExists = true;
                                break;
                            }
                        }
                    }
                    response.IsAuditApplicable = job.Company.IsAuditApplicable;

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
                else
                {
                    var company = await Context.DataContext.Companies.FirstOrDefaultAsync(t => t.IsActive && t.Id == companyId);
                    if (company != null)
                    {
                        response.Job.IsAssetTracked = company.IsAssetTrackingEnabled;
                        response.Job.IsResaleEnabled = company.IsResaleEnabled;
                        response.Job.JobLicenses = company.TaxExemptLicenses.Where(t => t.IsDefault).Select(t => t.Id).ToList();
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.lblJobNotFound;
                    }
                    // response.Job.DeliveryDays = new List<int> { (int)WeekDay.Sunday, (int)WeekDay.Monday, (int)WeekDay.Tuesday, (int)WeekDay.Wednesday, (int)WeekDay.Thursday, (int)WeekDay.Friday, (int)WeekDay.Saturday };
                    // response.Job.FromDeliveryTime = Convert.ToDateTime(Constants.StartTime.ToString()).ToShortTimeString();
                    //   response.Job.ToDeliveryTime = Convert.ToDateTime(Constants.EndTime.ToString()).ToShortTimeString();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobStepsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<SaveJobStatusViewModel> SaveJobStepsAsync(UserContext userContext, JobStepsViewModel viewModel, bool isCompanySpecificPort = false)
        {
            SaveJobStatusViewModel response = new SaveJobStatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    viewModel.Job.CreatedBy = viewModel.UserId;
                    viewModel.Job.UpdatedBy = viewModel.UserId;
                    viewModel.JobBudget.UpdatedBy = viewModel.UserId;
                    viewModel.JobBudget.IsAssetTracked = viewModel.Job.IsAssetTracked;
                    viewModel.JobBudget.IsAssetDropStatusEnabled = viewModel.Job.IsAssetDropStatusEnabled;
                    if (viewModel.Job.IsAssetTracked)
                    {
                        response.IsAssetTrackingEnabled = true;
                    }
                    if (string.IsNullOrWhiteSpace(viewModel.Job.JobID))
                    {
                        viewModel.Job.JobID = viewModel.Job.ExternalRefId != null ? viewModel.Job.ExternalRefId : viewModel.Job.Name;
                    }

                    if (viewModel.Job.LocationType == JobLocationTypes.Various && viewModel.Job.Country.Id != (int)Country.CAR)
                    {
                        viewModel.Job.Address = Resource.lblVarious;
                        viewModel.Job.City = Resource.lblVarious;
                        viewModel.Job.ZipCode = Resource.lblVarious;
                        viewModel.Job.CountyName = Resource.lblVarious;
                    }
                    else if (viewModel.Job.Country.Id == (int)Country.CAR)
                    {
                        if (string.IsNullOrWhiteSpace(viewModel.Job.Address))
                            viewModel.Job.Address = viewModel.Job.State.Name ?? Resource.lblCaribbean;
                        if (string.IsNullOrWhiteSpace(viewModel.Job.City))
                            viewModel.Job.City = viewModel.Job.State.Name ?? Resource.lblCaribbean;
                        if (string.IsNullOrWhiteSpace(viewModel.Job.ZipCode))
                            viewModel.Job.ZipCode = viewModel.Job.State.Name ?? Resource.lblCaribbean;
                        if (string.IsNullOrWhiteSpace(viewModel.Job.CountyName))
                            viewModel.Job.CountyName = viewModel.Job.State.Name ?? Resource.lblCaribbean;
                    }

                    if ((viewModel.Job.Latitude == 0 || viewModel.Job.Longitude == 0) && viewModel.Job.LocationType != JobLocationTypes.Various)
                    {
                        var stateCode = Context.DataContext.MstStates.First(t => t.Id == viewModel.Job.State.Id).Code;
                        var countryCode = Context.DataContext.MstCountries.First(t => t.Id == viewModel.Job.Country.Id).Code;
                        var point = GoogleApiDomain.GetGeocode($"{viewModel.Job.Address} {viewModel.Job.City} {stateCode} {countryCode} {viewModel.Job.ZipCode}");
                        if (point != null)
                        {
                            viewModel.Job.Latitude = Convert.ToDecimal(point.Latitude);
                            viewModel.Job.Longitude = Convert.ToDecimal(point.Longitude);
                            if (string.IsNullOrWhiteSpace(viewModel.Job.CountyName))
                            {
                                viewModel.Job.CountyName = point.CountyName != null ? point.CountyName : viewModel.Job.City;
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageJobCreateFailedInvalidAddress;
                            return response;
                        }
                    }

                    if ((string.IsNullOrWhiteSpace(viewModel.Job.Address) ||
                        string.IsNullOrWhiteSpace(viewModel.Job.City) ||
                        viewModel.Job.State.Id == 0 ||
                        viewModel.Job.Country.Id == 0 ||
                        string.IsNullOrWhiteSpace(viewModel.Job.ZipCode) ||
                        string.IsNullOrWhiteSpace(viewModel.Job.CountyName)) && viewModel.Job.Country.Id != (int)Country.CAR)
                    {
                        var point = GoogleApiDomain.GetAddress(Convert.ToDouble(viewModel.Job.Latitude), Convert.ToDouble(viewModel.Job.Longitude));
                        if (point != null)
                        {
                            viewModel.Job.Address = point.Address;
                            viewModel.Job.City = point.City;
                            viewModel.Job.ZipCode = point.ZipCode;
                            viewModel.Job.CountyName = point.CountyName;
                            viewModel.Job.Country = Context.DataContext.MstCountries.Single(t => t.Name.ToLower().Contains(point.CountryName.ToLower())).ToViewModel();
                            viewModel.Job.State = Context.DataContext.MstStates.Single(t => t.Code.ToLower() == point.StateCode.ToLower() && t.CountryId == viewModel.Job.Country.Id).ToViewModel();
                        }
                        else
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageJobCreateFailedInvalidAddress;
                            return response;
                        }
                    }

                    if (string.IsNullOrEmpty(viewModel.Job.TimeZoneName))
                    {
                        string timeZoneName = GoogleApiDomain.GetTimeZone(viewModel.Job.Latitude, viewModel.Job.Longitude);
                        if (!string.IsNullOrEmpty(timeZoneName))
                        {
                            viewModel.Job.TimeZoneName = timeZoneName;
                        }
                        else
                        {
                            if (viewModel.Job.LocationType == JobLocationTypes.Port)
                            {
                                var timeZone = TimeZone.CurrentTimeZone;
                                if (timeZone != null && !string.IsNullOrWhiteSpace(timeZone.StandardName))
                                {
                                    viewModel.Job.TimeZoneName = timeZone.StandardName.ParseTimeZone();
                                }
                            }
                            else
                            {
                                transaction.Rollback();
                                response.StatusCode = Status.Failed;
                                return response;
                            }

                        }
                    }

                    viewModel.Job.TimeZoneName = viewModel.Job.TimeZoneName.ParseTimeZone();
                    var user = Context.DataContext.Users.FirstOrDefault(t => t.Id == viewModel.UserId);
                    var job = Context.DataContext.Jobs.SingleOrDefault(t => t.Id == viewModel.Job.Id);
                    var helperDomain = new HelperDomain(this);
                    if (job == null)
                    {
                        var duplicateEmails = viewModel.ContactPersons.GroupBy(i => new { i.Email }).SelectMany(g => g.Skip(1));
                        if (duplicateEmails.Any())
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageJobDuplicateContactPersonEmails;
                            return response;
                        }
                        var emails = viewModel.ContactPersons.Select(t => t.Email.Trim()).ToList();
                        var existingEmails = new List<string>();
                        if (user.Company != null && emails.Any())
                        {
                            existingEmails = await helperDomain.GetExistingJobContactsAsync(emails, user.Company.Id);
                        }

                        if (existingEmails.Count > 0)
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageEmailAlreadyInvited, string.Join(" ,", existingEmails));
                            return response;
                        }
                        var duplicateSubcontractors = viewModel.Subcontractors.GroupBy(i => new { i.Name }).SelectMany(g => g.Skip(1));
                        if (duplicateSubcontractors.Any())
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageJobDuplicateSubcontractors;
                            return response;
                        }

                        if (viewModel.Job.IsJobSpecificBillToEnabled || (!string.IsNullOrWhiteSpace(viewModel.Job.BillToInfo.Address)))
                        {
                            viewModel.Job.BillToInfo.CompanyId = userContext.CompanyId;
                            var billingAddress = viewModel.Job.BillToInfo.ToBillingAddressEntityFromTPO();

                            billingAddress.UpdatedBy = userContext.Id;
                            billingAddress.UpdatedDate = DateTimeOffset.Now;
                            Context.DataContext.BillingAddresses.Add(billingAddress);
                            await Context.CommitAsync();

                            viewModel.Job.BillToInfo.BillingAddressId = billingAddress.Id;
                        }

                        job = viewModel.Job.ToEntity(job);

                        //job.CreatedByCompanyId = userContext.CompanyId ==0;
                        //used only for API
                        if (viewModel.IsJobCreationFromAPI)
                            job.CreatedByCompanyId = viewModel.SupplierCompanyId;
                        else
                            job.CreatedByCompanyId = (job.LocationType == JobLocationTypes.Port && !isCompanySpecificPort) ? ApplicationConstants.SuperAdminCompanyId
                                                        : userContext.CompanyId;

                        if (user.Company != null)
                        {
                            job.Users1 = user.Company.Users.Where(t => viewModel.Job.OnsiteContacts.Contains(t.Id)).ToList();
                        }

                        //create job contacts
                        viewModel.ContactPersons.ForEach(t => t.InvitedById = viewModel.UserId);
                        foreach (var additionalUser in viewModel.ContactPersons)
                        {
                            if (additionalUser.RoleIds.Contains((int)UserRoles.Admin) && additionalUser.RoleIds.Count > 0)
                            {
                                additionalUser.RoleIds = new List<int> { (int)UserRoles.Admin };
                            }
                            var jobContact = additionalUser.ToEntity();
                            jobContact.MstRoles = Context.DataContext.MstRoles.Where(t => additionalUser.RoleIds.Contains(t.Id)).ToList();
                            job.CompanyXAdditionalUserInvites.Add(jobContact);
                        }

                        await CreateSubcontractorEntity(viewModel.Subcontractors, job);
                        SetCurrencyAndUoMToJobBudgetViewModel(viewModel, job);
                        job.JobBudget = viewModel.JobBudget.ToEntity(job.JobBudget);


                        if (user.Company != null)
                        {
                            var existingUsers = job.Users.ToList();
                            existingUsers.ForEach(t => job.Users.Remove(t));
                            job.Users = user.Company.Users.Where(t => viewModel.Job.AssignedTo.Contains(t.Id)).ToList();
                            // Job Licenses
                            job.TaxExemptLicenses = user.Company.TaxExemptLicenses.Where(t => viewModel.Job.JobLicenses.Contains(t.Id)).ToList();
                        }


                        var approvalUsers = new List<JobXApprovalUser>();
                        if (viewModel.Job.ApprovalUser.HasValue)
                        {
                            approvalUsers.Add(new JobXApprovalUser { UserId = viewModel.Job.ApprovalUser.Value, AssignedDate = DateTimeOffset.Now, IsActive = true });
                        }
                        job.JobXApprovalUsers = approvalUsers;
                        job.SignatureEnabled = viewModel.Job.SignatureEnabled;
                        //Resale
                        if (viewModel.Job.IsResaleEnabled)
                        {
                            var ResaleCustomer = new ResaleCustomer();
                            ResaleCustomer.Email = viewModel.Job.CustomerEmail;
                            ResaleCustomer.Name = viewModel.Job.CustomerName;
                            job.ResaleCustomers.Add(ResaleCustomer);
                        }

                        job.TerminalId = helperDomain.GetClosestTerminalId(job.Latitude, job.Longitude, job.StateId);
                        if (job.TerminalId == null || job.TerminalId == 0)
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageJobCreateNoTerminalFound;
                            return response;
                        }
                        if (user.Company != null)
                        {
                            user.Company.Jobs.Add(job);
                        }
                        else
                        {
                            if (job.LocationType == JobLocationTypes.Port)
                            {
                                job.CompanyId = ApplicationConstants.SuperAdminCompanyId;
                            }
                            Context.DataContext.Jobs.Add(job);
                        }

                    }

                    await Context.CommitAsync();

                    var saveAdditionalDetails = await new FreightServiceDomain(this).SaveAdditionalJobDetails(viewModel.Job, job.Id);
                    if (!saveAdditionalDetails)
                    {
                        transaction.Rollback();
                        if (job.LocationType == JobLocationTypes.Port)
                        {
                            response.StatusMessage = Resource.errMessageFailedToCreatePort;
                        }
                        else
                        {
                            response.StatusMessage = Resource.errMessageJobCreateFailed;
                        }
                        LogManager.Logger.WriteException("JobDomain", "SaveJobStepsAsync", "freight service response failed", new Exception());
                        return response;
                    }

                    transaction.Commit();

                    //Add an entry to notifications
                    if (job.JobXStatuses.FirstOrDefault(t => t.IsActive).StatusId != (int)JobStatus.Draft)
                    {
                        NotificationDomain notificationDomain = new NotificationDomain(this);
                        await notificationDomain.AddNotificationEventAsync(EventType.JobCreated, job.Id, viewModel.UserId);

                        //Add an entry to notifications
                        foreach (var contact in job.CompanyXAdditionalUserInvites)
                        {
                            await notificationDomain.AddNotificationEventAsync(EventType.AdditionalUserAdded, contact.Id, user.Id);
                        }
                    }

                    NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                    var status = job.JobXStatuses.FirstOrDefault(t => t.IsActive);
                    if (status.StatusId == (int)JobStatus.Open)
                    {
                        await newsfeedDomain.SetNewJobCreationNewsFeed(userContext, job, false);
                    }
                    if (viewModel.Job.Id == 0)
                    {
                        if (status.StatusId == (int)JobStatus.Draft)
                            await newsfeedDomain.SetNewJobCreationNewsFeed(userContext, job, true);
                    }
                    if (viewModel.Job.IsProFormaPoEnabled)
                    {
                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetProFormaEnableDisableForJobNewsfeed(userContext, job, viewModel.Job.IsProFormaPoEnabled);
                    }
                    viewModel.Job.Id = job.Id;
                    response.JobId = job.Id;
                    response.StatusCode = Status.Success;
                    response.StatusMessage = string.Format((job.LocationType != JobLocationTypes.Port) ? Resource.errMessageJobCreateSuccess : Resource.successMessagePortCreated, job.Name);
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageJobCreateFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("JobDomain", "SaveJobStepsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<SaveJobStatusViewModel> UpdateJobStepsAsync(UserContext userContext, JobStepsViewModel viewModel)
        {
            SaveJobStatusViewModel response = new SaveJobStatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    viewModel.Job.UpdatedBy = viewModel.UserId;
                    viewModel.JobBudget.UpdatedBy = viewModel.UserId;
                    viewModel.JobBudget.IsAssetTracked = viewModel.Job.IsAssetTracked;
                    viewModel.JobBudget.IsAssetDropStatusEnabled = viewModel.Job.IsAssetDropStatusEnabled;
                    var user = Context.DataContext.Users.FirstOrDefault(t => t.Id == viewModel.UserId);
                    var job = Context.DataContext.Jobs.SingleOrDefault(t => t.Id == viewModel.Job.Id);
                    bool isApprovalWorkFlowDisabled = false;
                    bool isProFormaStatusChanged = false;
                    if (job != null)
                    {
                        #region Set Newsfeed
                        if (job.JobBudget.IsAssetTracked != viewModel.JobBudget.IsAssetTracked)
                        {
                            if (viewModel.JobBudget.IsAssetTracked)
                            {
                                response.IsAssetTrackingEnabled = true;
                            }
                            await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetAssetTrackingEnableDisableNewsfeed(userContext, job, viewModel.JobBudget.IsAssetTracked);
                        }
                        if (job.JobBudget.IsDropPictureRequired != viewModel.JobBudget.IsDropPictureRequired)
                        {
                            await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetAssetPictureEnableDisableNewsfeed(userContext, job, viewModel.JobBudget.IsDropPictureRequired);
                        }
                        if (job.IsResaleEnabled != viewModel.Job.IsResaleEnabled)
                        {
                            await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetResaleEnableDisableNewsfeed(userContext, job, viewModel.Job.IsResaleEnabled);
                        }
                        if (job.IsProFormaPoEnabled != viewModel.Job.IsProFormaPoEnabled)
                        {
                            isProFormaStatusChanged = true;
                            await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetProFormaEnableDisableForJobNewsfeed(userContext, job, viewModel.Job.IsProFormaPoEnabled);
                        }
                        if (job.IsApprovalWorkflowEnabled != viewModel.Job.IsApprovalWorkFlowEnabled)
                        {
                            await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetApprovalWorkflowEnableDisableNewsfeed(userContext, job, viewModel.Job.ApprovalUser, viewModel.Job.IsApprovalWorkFlowEnabled);
                        }
                        if (job.JobBudget.IsTaxExempted != viewModel.JobBudget.IsTaxExempted)
                        {
                            await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetTaxExemptEnableDisableNewsfeed(userContext, job, viewModel.JobBudget.IsTaxExempted);
                        }
                        #endregion

                        var duplicateEmails = viewModel.ContactPersons.GroupBy(i => new { i.Email }).SelectMany(g => g.Skip(1));
                        if (duplicateEmails.Any())
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageJobDuplicateContactPersonEmails;
                            return response;
                        }
                        var emails = viewModel.ContactPersons.Select(t => t.Email.Trim()).ToList();
                        var existingEmails = new List<string>();
                        if (user.Company != null && emails.Any())
                        {
                            existingEmails = await ContextFactory.Current.GetDomain<HelperDomain>().GetExistingJobContactsAsync(emails, user.Company.Id);

                        }
                        if (existingEmails.Count > 0)
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageEmailAlreadyInvited, string.Join(" ,", existingEmails));
                            return response;
                        }

                        if (viewModel.Job.Country.Id != job.CountryId || viewModel.Job.Country.Currency != job.Currency)
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageUnableToUpdateCountryAndCurrency;
                            return response;
                        }

                        if (viewModel.Job.Latitude == 0 || viewModel.Job.Longitude == 0)
                        {
                            var stateCode = Context.DataContext.MstStates.First(t => t.Id == viewModel.Job.State.Id).Code;
                            var countryCode = Context.DataContext.MstCountries.First(t => t.Id == viewModel.Job.Country.Id).Code;
                            var point = GoogleApiDomain.GetGeocode($"{viewModel.Job.Address} {viewModel.Job.City} {stateCode} {countryCode} {viewModel.Job.ZipCode}");
                            if (point != null)
                            {
                                viewModel.Job.Latitude = Convert.ToDecimal(point.Latitude);
                                viewModel.Job.Longitude = Convert.ToDecimal(point.Longitude);
                                viewModel.Job.CountyName = point.CountyName;
                            }
                            else
                            {
                                transaction.Rollback();
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessageJobUpdateFailedInvalidAddress;
                                return response;
                            }
                        }
                        var subcontractorStatus = await CreateSubcontractorEntity(viewModel.Subcontractors, job);
                        if (subcontractorStatus.StatusCode == Status.Failed)
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Warning;
                            response.StatusMessage = subcontractorStatus.StatusMessage;
                            return response;
                        }
                        job.SignatureEnabled = viewModel.Job.SignatureEnabled;

                        var jobOrders = Context.DataContext.Orders.
                                        Where(t =>
                                                    t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                                    && t.FuelRequest.JobId == job.Id
                                            ).ToList();
                        jobOrders.ForEach(t => t.SignatureEnabled = job.SignatureEnabled);

                        if (job.IsApprovalWorkflowEnabled && !viewModel.Job.IsApprovalWorkFlowEnabled)
                        {
                            isApprovalWorkFlowDisabled = true;
                        }
                        var approvalUser = job.JobXApprovalUsers.OrderByDescending(t => t.Id).FirstOrDefault(t => t.IsActive);

                        //Resale Flow Starts
                        if (viewModel.Job.IsResaleEnabled)
                        {
                            var ResaleCustomer = job.ResaleCustomers.FirstOrDefault();
                            if (ResaleCustomer != null)
                            {
                                ResaleCustomer.Email = viewModel.Job.CustomerEmail;
                                ResaleCustomer.Name = viewModel.Job.CustomerName;
                            }
                            else
                            {
                                ResaleCustomer = new ResaleCustomer();
                                ResaleCustomer.Email = viewModel.Job.CustomerEmail;
                                ResaleCustomer.Name = viewModel.Job.CustomerName;
                                job.ResaleCustomers.Add(ResaleCustomer);
                            }
                        }

                        if (job.IsResaleEnabled && !viewModel.Job.IsResaleEnabled)
                        {
                            var ResaleCustomer = job.ResaleCustomers.FirstOrDefault();
                            if (ResaleCustomer != null)
                            {
                                job.ResaleCustomers.Remove(ResaleCustomer);
                                Context.DataContext.Entry(ResaleCustomer).State = EntityState.Deleted;
                            }
                        }
                        //Resale Flow Ends
                        if (job.JobBudget.IsTaxExempted != viewModel.JobBudget.IsTaxExempted)
                        {
                            AddAuditLogTaxExempt(userContext, job.Id, job.JobBudget.IsTaxExempted, viewModel.JobBudget.IsTaxExempted);
                        }
                        viewModel.Job.TimeZoneName = viewModel.Job.TimeZoneName.ParseTimeZone();

                        if (viewModel.Job.IsJobSpecificBillToEnabled || (!string.IsNullOrWhiteSpace(viewModel.Job.BillToInfo.Address)))
                        {
                            var billingAddress = Context.DataContext.BillingAddresses.FirstOrDefault(t => t.Id == viewModel.Job.BillToInfo.Id);

                            viewModel.Job.BillToInfo.CompanyId = userContext.CompanyId;
                            billingAddress = viewModel.Job.BillToInfo.ToBillingAddressEntityFromTPO(billingAddress);

                            billingAddress.UpdatedBy = userContext.Id;
                            billingAddress.UpdatedDate = DateTimeOffset.Now;

                            if (viewModel.Job.BillToInfo.Id == 0)
                                Context.DataContext.BillingAddresses.Add(billingAddress);
                            else
                                Context.DataContext.Entry(billingAddress).State = EntityState.Modified;

                            await Context.CommitAsync();

                            viewModel.Job.BillToInfo.BillingAddressId = billingAddress.Id;
                        }

                        job = viewModel.Job.ToEntity(job);

                        //Onsite persons
                        var existingOnsiteUsers = job.Users1.ToList();
                        existingOnsiteUsers.ForEach(t => job.Users1.Remove(t));
                        job.Users1 = user.Company.Users.Where(t => viewModel.Job.OnsiteContacts.Contains(t.Id)).ToList();

                        SetCurrencyAndUoMToJobBudgetViewModel(viewModel, job);
                        job.JobBudget = viewModel.JobBudget.ToEntity(job.JobBudget);
                        job.TerminalId = ContextFactory.Current.GetDomain<HelperDomain>().GetClosestTerminalId(job.Latitude, job.Longitude, job.StateId);
                        if (job.TerminalId == null || job.TerminalId == 0)
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageJobCreateNoTerminalFound;
                            return response;
                        }
                        var existingUsers = job.Users.ToList();
                        existingUsers.ForEach(t => job.Users.Remove(t));
                        job.Users = user.Company.Users.Where(t => viewModel.Job.AssignedTo.Contains(t.Id)).ToList();

                        // Job Licenses
                        var existingLicenses = job.TaxExemptLicenses.ToList();
                        existingLicenses.ForEach(t => job.TaxExemptLicenses.Remove(t));
                        job.TaxExemptLicenses = user.Company.TaxExemptLicenses.Where(t => viewModel.Job.JobLicenses.Contains(t.Id)).ToList();

                        //Job Contacts
                        viewModel.ContactPersons.ForEach(t => { t.InvitedById = viewModel.UserId; });
                        foreach (var additionalUser in viewModel.ContactPersons)
                        {
                            if (additionalUser.RoleIds.Contains((int)UserRoles.Admin) && additionalUser.RoleIds.Count > 0)
                            {
                                additionalUser.RoleIds = new List<int> { (int)UserRoles.Admin };
                            }
                            var jobContact = additionalUser.ToEntity();
                            jobContact.MstRoles = Context.DataContext.MstRoles.Where(t => additionalUser.RoleIds.Contains(t.Id)).ToList();
                            job.CompanyXAdditionalUserInvites.Add(jobContact);
                        }

                        //Approval Workflow
                        if (viewModel.Job.IsApprovalWorkFlowEnabled)
                        {
                            if (approvalUser != null && approvalUser.UserId != viewModel.Job.ApprovalUser.Value)
                            {
                                approvalUser.RemovedDate = DateTimeOffset.Now;
                                approvalUser.IsActive = false;
                            }
                            if ((approvalUser == null && viewModel.Job.ApprovalUser.HasValue) || (approvalUser != null && approvalUser.UserId != viewModel.Job.ApprovalUser.Value))
                            {
                                job.JobXApprovalUsers.Add(new JobXApprovalUser { UserId = viewModel.Job.ApprovalUser.Value, AssignedDate = DateTimeOffset.Now, IsActive = true });
                            }
                        }

                        if (isApprovalWorkFlowDisabled && approvalUser != null)
                        {
                            approvalUser.RemovedDate = DateTimeOffset.Now;
                            approvalUser.IsActive = false;
                        }

                        Context.DataContext.Entry(job).State = EntityState.Modified;
                    }

                    await Context.CommitAsync();

                    if (job.JobXStatuses.FirstOrDefault(t => t.IsActive).StatusId != (int)JobStatus.Draft)
                    {
                        if (!viewModel.Job.IsAssetTracked)
                        {
                            job.JobXAssets.Where(t => t.RemovedBy == null && t.RemovedDate == null && t.Asset.Type == (int)AssetType.Asset).ToList().ForEach(t =>
                              {
                                  t.RemovedBy = viewModel.UserId;
                                  t.RemovedDate = DateTimeOffset.Now;
                              });
                        }
                    }
                    await Context.CommitAsync();

                    var saveAdditionalDetails = await new FreightServiceDomain(this).UpdateAdditionalJobDetails(viewModel.Job);
                    if (!saveAdditionalDetails)
                    {
                        response.StatusMessage = Resource.errMessageUpdateFailed;
                        LogManager.Logger.WriteException("JobDomain", "UpdateJobStepsAsync", "freight service response failed", new Exception());
                        transaction.Rollback();
                        return response;
                    }

                    transaction.Commit();

                    //Add an entry to notifications
                    if (job.JobXStatuses.FirstOrDefault(t => t.IsActive).StatusId != (int)JobStatus.Draft)
                    {
                        if (isProFormaStatusChanged)
                        {
                            var ProFormaStatus = string.Empty;
                            if (viewModel.Job.IsProFormaPoEnabled)
                                ProFormaStatus = Resource.lblEnabled;
                            else
                                ProFormaStatus = Resource.lblDisabled;
                            var message = new JobMessageViewModel { ProFormaPoStatus = ProFormaStatus };
                            var jsonMessage = new JavaScriptSerializer().Serialize(message);
                            //invitation to all assigned user and admins
                            await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.JobUpdated, job.Id, viewModel.UserId, null, jsonMessage);
                        }
                        else
                        {
                            //invitation to all assigned user and admins
                            await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.JobUpdated, job.Id, viewModel.UserId);
                        }
                    }
                    //invitation to new job contacts
                    var existingContacts = Context.DataContext.Notifications.Where(t => t.EventTypeId == (int)EventType.AdditionalUserAdded).Select(t => t.EntityId).ToList();
                    var newContacts = job.CompanyXAdditionalUserInvites.Where(t => !existingContacts.Any(t1 => t1 == t.Id));
                    foreach (var contact in newContacts)
                    {
                        await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(
                                                                                        EventType.AdditionalUserAdded,
                                                                                        contact.Id,
                                                                                        user.Id);
                    }

                    await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetJobUpdateNewsFeed(userContext, job);

                    response.StatusCode = Status.Success;
                    response.StatusMessage = string.Format(Resource.errMessageJobUpdateSuccess, job.Name);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("JobDomain", "UpdateJobStepsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<JobStepsViewModelForSuperAdmin> GetJobStepsForSuperAdminAsync(int jobId = 0, int companyId = 0)
        {
            JobStepsViewModelForSuperAdmin response = new JobStepsViewModelForSuperAdmin();
            try
            {
                var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.IsActive && t.Id == jobId);
                if (job != null)
                {
                    response.Job = job.ToViewModelForSuperAdmin(companyId);

                    response.CompanyId = job.CompanyId;
                    response.CompanyTypeId = job.Company.CompanyTypeId;
                    response.Culture = new HelperDomain().SetEntityThreadCulture(job.Currency);
                    if (job.JobBudget != null)
                    {
                        response.Job.IsAssetTracked = job.JobBudget.IsAssetTracked;
                        response.Job.IsAssetDropStatusEnabled = job.JobBudget.IsAssetDropStatusEnabled;
                        response.Job.IsTaxExempted = job.JobBudget.IsTaxExempted;
                    }
                    response.Job.AccountingCompanyId = Context.DataContext.SupplierXBuyerDetails.Where(t => t.JobId == job.Id && t.SupplierCompanyId == companyId && t.BuyerCompanyId == job.CompanyId).Select(t => t.AccountingCompanyId).FirstOrDefault();
                    // response.Job.CreditCheckTypeId = (int)Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == companyId && t.IsActive).Select(t => t.CreditCheckType).FirstOrDefault();
                    var companyDomain = new CompanyDomain(this);

                    if (companyId > 0)
                        response.ProductSequencing = await companyDomain.GetProductSequence(companyId, ProductSequencingCreationMethod.Job, jobId);

                    var AdditionalJobDetails = await new FreightServiceDomain(this).GetAdditionalJobDetails(job.Id, 0);
                    if (AdditionalJobDetails != null)
                        response.Job.DistanceCovered = AdditionalJobDetails.DistanceCovered;

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
                else
                {
                    var company = await Context.DataContext.Companies.FirstOrDefaultAsync(t => t.IsActive && t.Id == companyId);
                    if (company != null)
                    {
                        response.Job.IsAssetTracked = company.IsAssetTrackingEnabled;
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.lblJobNotFound;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobStepsForSuperAdminAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<SaveJobStatusViewModel> UpdateJobStepsForSuperAdminAsync(UserContext userContext, JobStepsViewModelForSuperAdmin viewModel)
        {
            SaveJobStatusViewModel response = new SaveJobStatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    viewModel.Job.UpdatedBy = viewModel.UserId;
                    var user = Context.DataContext.Users.FirstOrDefault(t => t.Id == viewModel.UserId);
                    var job = Context.DataContext.Jobs.SingleOrDefault(t => t.Id == viewModel.Job.Id);
                    bool isProFormaStatusChanged = false;
                    if (job != null)
                    {
                        var existingJob = await Context.DataContext.Jobs.Where(t => t.IsActive && t.Name.ToLower() == viewModel.Job.Name.ToLower() && t.Id != viewModel.Job.Id && t.CompanyId == job.CompanyId)
                                                                        .Select(t => new { t.Id, JobName = t.Name })
                                                                        .OrderByDescending(t => t.Id)
                                                                        .FirstOrDefaultAsync();
                        if (existingJob != null)
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageJobNameAlreadyExists;
                            return response;
                        }
                        if (string.IsNullOrWhiteSpace(viewModel.Job.CountyName))
                        {
                            viewModel.Job.CountyName = viewModel.Job.City;
                        }
                        if (job.JobBudget.IsAssetTracked != viewModel.Job.IsAssetTracked)
                        {
                            if (viewModel.Job.IsAssetTracked)
                            {
                                response.IsAssetTrackingEnabled = true;
                            }
                            await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetAssetTrackingEnableDisableNewsfeed(userContext, job, viewModel.Job.IsAssetTracked);
                        }
                        if (job.IsProFormaPoEnabled != viewModel.Job.IsProFormaPoEnabled)
                        {
                            isProFormaStatusChanged = true;
                            await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetProFormaEnableDisableForJobNewsfeed(userContext, job, viewModel.Job.IsProFormaPoEnabled);
                        }
                        if (job.JobBudget.IsTaxExempted != viewModel.Job.IsTaxExempted)
                        {
                            await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetTaxExemptEnableDisableNewsfeed(userContext, job, viewModel.Job.IsTaxExempted);
                        }

                        if (viewModel.Job.Country.Id != (int)Country.CAR && (viewModel.Job.Country.Id != job.CountryId || viewModel.Job.Country.Currency != job.Currency))
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageUnableToUpdateCountryAndCurrency;
                            return response;
                        }

                        if (viewModel.Job.Latitude == 0 || viewModel.Job.Longitude == 0)
                        {
                            var stateCode = Context.DataContext.MstStates.First(t => t.Id == viewModel.Job.State.Id).Code;
                            var countryCode = Context.DataContext.MstCountries.First(t => t.Id == viewModel.Job.Country.Id).Code;
                            var point = GoogleApiDomain.GetGeocode($"{viewModel.Job.Address} {viewModel.Job.City} {stateCode} {countryCode} {viewModel.Job.ZipCode}");
                            if (point != null)
                            {
                                viewModel.Job.Latitude = Convert.ToDecimal(point.Latitude);
                                viewModel.Job.Longitude = Convert.ToDecimal(point.Longitude);
                                viewModel.Job.CountyName = point.CountyName;
                            }
                            else
                            {
                                transaction.Rollback();
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessageJobUpdateFailedInvalidAddress;
                                return response;
                            }
                        }

                        job.SignatureEnabled = viewModel.Job.SignatureEnabled;

                        var jobOrders = Context.DataContext.Orders.
                                        Where(t =>
                                                    t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                                    && t.FuelRequest.JobId == job.Id
                                            ).ToList();
                        jobOrders.ForEach(t => t.SignatureEnabled = job.SignatureEnabled);

                        if (job.JobBudget.IsTaxExempted != viewModel.Job.IsTaxExempted)
                        {
                            AddAuditLogTaxExempt(userContext, job.Id, job.JobBudget.IsTaxExempted, viewModel.Job.IsTaxExempted);
                        }
                        viewModel.Job.TimeZoneName = viewModel.Job.TimeZoneName.ParseTimeZone();
                        job = viewModel.Job.ToEntityForSuperAdmin(job);

                        if (viewModel.Job.Country.Id == (int)Country.CAR && viewModel.Job.IsMissingAddress())
                        {
                            var stateName = Context.DataContext.MstStates.First(t => t.Id == viewModel.Job.State.Id).Name;
                            if (string.IsNullOrWhiteSpace(viewModel.Job.Address))
                                job.Address = stateName ?? Resource.lblCaribbean;
                            if (string.IsNullOrWhiteSpace(viewModel.Job.City))
                                job.City = stateName ?? Resource.lblCaribbean;
                            if (string.IsNullOrWhiteSpace(viewModel.Job.ZipCode))
                                job.ZipCode = stateName ?? Resource.lblCaribbean;
                            if (string.IsNullOrWhiteSpace(viewModel.Job.CountyName))
                                job.CountyName = stateName ?? Resource.lblCaribbean;
                        }


                        job.TerminalId = ContextFactory.Current.GetDomain<HelperDomain>().GetClosestTerminalId(job.Latitude, job.Longitude, job.StateId);
                        if (job.TerminalId == null || job.TerminalId == 0)
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageJobCreateNoTerminalFound;
                            return response;
                        }

                        Context.DataContext.Entry(job).State = EntityState.Modified;
                    }

                    await Context.CommitAsync();
                    if (job.JobXStatuses.FirstOrDefault(t => t.IsActive).StatusId != (int)JobStatus.Draft)
                    {
                        if (!viewModel.Job.IsAssetTracked)
                        {
                            job.JobXAssets.Where(t => t.RemovedBy == null && t.RemovedDate == null && t.Asset.Type == (int)AssetType.Asset).ToList().ForEach(t =>
                            {
                                t.RemovedBy = viewModel.UserId;
                                t.RemovedDate = DateTimeOffset.Now;
                            });
                        }
                    }

                    job.JobBudget.IsAssetDropStatusEnabled = viewModel.Job.IsAssetDropStatusEnabled;
                    job.JobBudget.IsAssetTracked = viewModel.Job.IsAssetTracked;
                    job.JobBudget.IsTaxExempted = viewModel.Job.IsTaxExempted;

                    await Context.CommitAsync();

                    var jobViewModel = new JobViewModel()
                    {
                        Id = job.Id,
                        JobID = job.DisplayJobID,
                        IsAutoCreateDREnable = viewModel.Job.IsAutoCreateDREnable,
                        Name = job.Name,
                        SiteImage = new ImageViewModel()
                        {
                            FilePath = viewModel.Job.ImageDetails.SiteImage?.FilePath
                        },
                        AdditionalImage = new AdditionalSiteImage()
                        {
                            SiteImage = new ImageViewModel()
                            {
                                FilePath = viewModel.Job.ImageDetails.AdditionalImage.SiteImage?.FilePath,
                            },
                            Description = viewModel.Job.ImageDetails.AdditionalImage.Description
                        }

                    };
                    jobViewModel.DeliveryDaysList = viewModel.Job.DeliveryDaysList.ToList();
                    jobViewModel.TrailerType = viewModel.Job.TrailerType;
                    jobViewModel.DistanceCovered = viewModel.Job.DistanceCovered;

                    var request = new JobToRegionAssignViewModel()
                    {
                        JobId = viewModel.Job.Id,
                        JobName = viewModel.Job.Name,
                        RegionId = viewModel.Job.RegionId,
                        UpdatedBy = userContext.Id,
                        CompanyId = userContext.CompanyId
                    };
                    var updateAdditionalDetails = await new FreightServiceDomain(this).SaveJobRegionCarrierDetails(request, jobViewModel, null);
                    if (updateAdditionalDetails.StatusCode == Status.Failed)
                    {
                        response.StatusMessage = Resource.errFailedToSaveJobDetails;
                        LogManager.Logger.WriteException("JobDomain", "UpdateJobStepsForSuperAdminAsync", "freight service response failed", new Exception());
                        transaction.Rollback();
                        return response;
                    }

                    transaction.Commit();

                    //Add an entry to notifications
                    if (job.JobXStatuses.FirstOrDefault(t => t.IsActive).StatusId != (int)JobStatus.Draft)
                    {
                        if (isProFormaStatusChanged)
                        {
                            var ProFormaStatus = string.Empty;
                            if (viewModel.Job.IsProFormaPoEnabled)
                                ProFormaStatus = Resource.lblEnabled;
                            else
                                ProFormaStatus = Resource.lblDisabled;
                            var message = new JobMessageViewModel { ProFormaPoStatus = ProFormaStatus };
                            var jsonMessage = new JavaScriptSerializer().Serialize(message);
                            //invitation to all assigned user and admins
                            await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.JobUpdated, job.Id, viewModel.UserId, null, jsonMessage);
                        }
                        else
                        {
                            //invitation to all assigned user and admins
                            await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.JobUpdated, job.Id, viewModel.UserId);
                        }
                    }
                    //invitation to new job contacts
                    var existingContacts = Context.DataContext.Notifications.Where(t => t.EventTypeId == (int)EventType.AdditionalUserAdded).Select(t => t.EntityId).ToList();
                    var newContacts = job.CompanyXAdditionalUserInvites.Where(t => !existingContacts.Any(t1 => t1 == t.Id));
                    foreach (var contact in newContacts)
                    {
                        await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(
                                                                                        EventType.AdditionalUserAdded,
                                                                                        contact.Id,
                                                                                        user.Id);
                    }

                    await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetJobUpdateNewsFeed(userContext, job);
                    response.EntityId = job.Id;
                    response.CustomerCompanyId = job.CompanyId;
                    if (job.User != null)
                    {
                        response.CustomerId = job.User.Id;
                    }
                    response.StatusCode = Status.Success;
                    response.StatusMessage = string.Format(Resource.errMessageJobUpdateSuccess, job.Name);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("JobDomain", "UpdateJobStepsForSuperAdminAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteJobAsync(int jobId, int userId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.Id == jobId);
                    if (job != null)
                    {
                        var requestCount = job.FuelRequests.Count;
                        if (requestCount <= 0)
                        {
                            job.IsActive = false;
                            Context.DataContext.Entry(job).State = EntityState.Modified;

                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = (job.LocationType == JobLocationTypes.Port) ? Resource.SuccessMessagePortDeleted : Resource.errMessageJobDeleteSuccess;

                            //Add an entry to notifications
                            await ContextFactory.Current.GetDomain<NotificationDomain>()
                                  .AddNotificationEventAsync(EventType.JobDeleted, job.Id, userId);
                        }
                        else
                        {
                            response.StatusMessage = Resource.errMessageCannotDeleteJob;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("JobDomain", "DeleteJobAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<JobGridViewModel>> GetJobGridAsync(int userId, JobFilterViewModel filter = null)
        {
            var response = new List<JobGridViewModel>();
            try
            {
                var helperDomain = new HelperDomain(this);
                var jobIds = await helperDomain.GetJobIdsAsync(userId);

                var user = await Context.DataContext.Users.FirstOrDefaultAsync(t => t.Id == userId);
                if (user != null && jobIds != null)
                {
                    var jobs = Context.DataContext.Jobs
                                    .Include(t => t.JobBudget).Include(t => t.Users1)
                                    .Include(t => t.MstState).Include(t => t.JobXAssets).Include("JobXAssets.Asset")
                                    .Where(t => t.IsActive && jobIds.Contains(t.Id) && t.Currency == filter.Currency && t.CountryId == filter.CountryId);
                    jobs = ApplyJobFilters(filter, user, jobs);
                    jobs.OrderByDescending(t => t.Id).ToList().ForEach(t => response.Add(t.ToGridViewModel()));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobGridAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<JobGridViewModel>> GetBuyerJobsBySupplierAsync(JobFilterViewModel filter, UserContext userContext)
        {
            var response = new List<JobGridViewModel>();
            var spDomain = new StoredProcedureDomain(this);
            try
            {
                var jobs = await spDomain.GetBuyerJobsBySupplierAsync(userContext.CompanyId, filter.Id);
                jobs.ForEach(t => response.Add(t.ToGridViewModel()));
                response = await SetRegionDetailsForJob(response, userContext);
                var apiResponse = await new FreightServiceDomain(this).GetJobSummary(userContext.CompanyId, jobs.Select(t => t.Id).ToList());
                if (apiResponse != null && apiResponse.StatusCode == Status.Success)
                {
                    foreach (var job in apiResponse.JobDetails)
                    {
                        var jobDetails = response.FirstOrDefault(t => t.Id == job.JobId);
                        jobDetails.CarrierId = job.CarrierId;
                        jobDetails.CarrierName = job.CarrierName;
                        jobDetails.DistanceCovered = job.DistanceCovered;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetBuyerJobsBySupplierAsync", ex.Message, ex);
            }
            return response.Distinct().ToList();
        }
        private async Task<List<JobGridViewModel>> SetRegionDetailsForJob(List<JobGridViewModel> response, UserContext userContext)
        {
            var fsDomain = new FreightServiceDomain(this);
            var regionDetails = await fsDomain.GetRegions(userContext);
            if (regionDetails != null)
            {
                List<RegionViewModel> regions = regionDetails.Regions;
                foreach (var job in response)
                {
                    var region = regions.FirstOrDefault(t => t.Jobs.Any(t1 => t1.Id == job.Id));
                    if (region != null)
                    {
                        job.RegionId = region.Id;
                        job.RegionName = region.Name;
                    }
                }
                var regionIds = response.Where(top => top.RegionId != null).Select(top => top.RegionId).ToList();
                var routeInfoDetails = await new FreightServiceDomain(this).GetRouteInfoDetails(regionIds);
                if (routeInfoDetails.Count > 0)
                {
                    foreach (var routeInfo in response)
                    {
                        var routeDetails = (from item in routeInfoDetails
                                            where item.RegionId == routeInfo.RegionId
                                            select new
                                            {
                                                item.Id,
                                                item.Name,
                                                item.TfxJobs
                                            }).FirstOrDefault(t => t.TfxJobs.Any(t1 => t1.Id == routeInfo.Id));
                        if (routeDetails != null)
                        {
                            routeInfo.RouteId = routeDetails.Id;
                            routeInfo.RouteName = routeDetails.Name;
                        }
                        else
                        {
                            routeInfo.RouteId = string.Empty;
                            routeInfo.RouteName = string.Empty;
                        }
                    }
                }
            }
            return response;
        }
        public async Task<List<JobListViewModel>> GetJobListAsync(int userId, string searchCriteria, int offset = 0, int count = 0, long scheduleDate = 0, int supplierCompanyId = 0, int brandedSuppCompanyId = 0)
        {
            var storedProcedureDomain = new StoredProcedureDomain(this);
            var response = new List<JobListViewModel>();
            try
            {
                if (scheduleDate != 0)
                {
                    //Get all jobs whose delivery schedule is today's or specific date for logged in buyer
                    response = await storedProcedureDomain.GetJobsForSpecificScheduleDate(userId, supplierCompanyId, searchCriteria, scheduleDate);
                }
                else
                {
                    var jobIdList = await storedProcedureDomain.GetJobIdsOfUser(userId);
                    if (brandedSuppCompanyId > 0)
                    {
                        jobIdList = GetBrandedSupplierLocations(brandedSuppCompanyId, true, jobIdList);
                    }

                    response = await Context.DataContext.Jobs.Where(t => jobIdList.Contains(t.Id) && t.IsActive
                                                                        && t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open)
                                                                 .Select(t => new JobListViewModel()
                                                                 {
                                                                     Name = t.Name,
                                                                     Latitude = t.Latitude,
                                                                     Longitude = t.Longitude,
                                                                     JobId = t.Id,
                                                                     CountryId = t.CountryId,
                                                                     Currency = (int)t.Currency,
                                                                     UnitOfMeasurement = (int)t.UoM,
                                                                     IsRetailJob = t.IsRetailJob,
                                                                     IsMarine = t.IsMarine
                                                                 }).OrderByDescending(t => t.JobId).ToListAsync();

                    if (!string.IsNullOrEmpty(searchCriteria))
                    {
                        response = response.Where(t => t.Name.ToLower().Contains(searchCriteria.ToLower())).ToList();
                    }

                    if (offset > 0 && count > 0)
                        response = response.Skip(offset).Take(count).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobListAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<JobListWithTanksViewModel>> GetJobListWithTanksAsync(int userId, int companyId, decimal latitude, decimal longitude, int brandedSuppCompanyId = 0)
        {
            var storedProcedureDomain = new StoredProcedureDomain(this);
            var helperDomain = new HelperDomain(this);
            var response = new List<JobListWithTanksViewModel>();
            try
            {

                var jobIdList = await storedProcedureDomain.GetJobIdsOfUser(userId);
                if (brandedSuppCompanyId > 0)
                {
                    jobIdList = GetBrandedSupplierLocations(brandedSuppCompanyId, true, jobIdList);
                }

                response = await Context.DataContext.Jobs.Where(t => jobIdList.Contains(t.Id) && t.IsActive
                                                                    && t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open)
                                                             .Select(t => new JobListWithTanksViewModel()
                                                             {
                                                                 Name = t.Name,
                                                                 Latitude = t.Latitude,
                                                                 Longitude = t.Longitude,
                                                                 JobId = t.Id,
                                                                 Address = t.Address,
                                                                 City = t.City,
                                                                 State = t.MstState != null ? t.MstState.Code : String.Empty,
                                                                 ZipCode = t.ZipCode,
                                                                 Country = t.MstCountry.Code != null ? t.MstCountry.Code : String.Empty,
                                                             }).ToListAsync();

                foreach (var item in response)
                {
                    item.Distance = helperDomain.CalculateDistance(latitude, longitude, item.Latitude, item.Longitude);
                }

                response = response.OrderBy(t => t.Distance).ToList();

                var allAssets = Context.DataContext.Assets.Include(t => t.AssetAdditionalDetail).Include(t => t.Image).Include(t => t.JobXAssets).Include("JobXAssets.Job")
                                    .Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.Company.Id == companyId);

                allAssets = allAssets.Where(t => jobIdList.Any(t1 => t.JobXAssets.Any(t2 => t2.JobId == t1 && t2.RemovedDate == null && t2.RemovedBy == null)));

                var tankIds = allAssets.Select(t => t.Id).ToList();
                var tankAdditionalList = await new FreightServiceDomain(this).GetTankList(tankIds);

                List<ApiTankDetailViewModel> tankList = new List<ApiTankDetailViewModel>();
                foreach (var asset in allAssets)
                {
                    ApiTankDetailViewModel tank = new ApiTankDetailViewModel();
                    if (tankAdditionalList != null)
                    {
                        var tankViewModel = tankAdditionalList.FirstOrDefault(t => t.AssetId == asset.Id);
                        if (tankViewModel != null)
                        {
                            //Set JobXAssetId and UoM
                            var jobXAssetId = 0;
                            var activeJobXAsset = asset.JobXAssets.FirstOrDefault(t => t.RemovedBy == null && t.RemovedDate == null);
                            if (activeJobXAsset != null)
                            {
                                jobXAssetId = activeJobXAsset.Id;
                                tank.UoM = activeJobXAsset.Job.UoM.ToString();
                            }

                            tank = tankViewModel.ToApiTankViewModel(tank.UoM);
                            tank.JobXAssetId = jobXAssetId;
                            tank.TankSequence = tankViewModel.TankSequence;
                            tankList.Add(tank);
                        }
                    }
                }

                foreach (var item in response)
                {
                    item.TankList = tankList.Where(t => t.JobId == item.JobId).OrderBy(t1 => t1.TankSequence == null || t1.TankSequence == 0 ? 99999 : t1.TankSequence).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobListWithTanksAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<CreateDeliveryRequestModel> GetProductTypesForJob(int userId, int companyId, int jobId = 0, int brandedCompanyId = -1)
        {
            CreateDeliveryRequestModel response = new CreateDeliveryRequestModel();
            var storedProcedureDomain = new StoredProcedureDomain(this);
            var jobs = await storedProcedureDomain.GetBuyerJobsWithProductTypes(userId, companyId, jobId);
            if (brandedCompanyId > 0)
            {
                jobs = jobs.Where(top => top.AcceptedCompanyId == brandedCompanyId).ToList();
            }
            var tanks = await ContextFactory.Current.GetDomain<JobDomain>().GetTanksByJobId(companyId, jobId);
            if (tanks.Count() > 0)
            {
                jobs = (from item in jobs
                        join tankInfo in tanks
                        on item.JobId equals tankInfo.JobId
                        where tankInfo.AssetId == item.AssetId
                        select item).ToList();
            }

            foreach (var job in jobs.GroupBy(t => t.JobId))
            {
                var firstItem = job.FirstOrDefault();
                if (firstItem != null)
                {
                    foreach (var product in job.GroupBy(t => t.ProductTypeId))
                    {
                        UspJobProductModel firstProduct = new UspJobProductModel();
                        if (tanks.Count() > 0)
                        {
                            var productInfo = (from item in product
                                               join tankInfo in tanks
                                              on item.AssetId equals tankInfo.AssetId
                                               where item.ProductTypeId == tankInfo.TfxProductTypeId
                                               select item).ToList();
                            if (productInfo.Count > 0)
                            {
                                firstProduct = productInfo.FirstOrDefault();
                            }
                            else
                            {
                                firstProduct = product.FirstOrDefault();
                            }
                        }
                        else
                        {
                            firstProduct = product.FirstOrDefault();
                        }
                        if (firstProduct != null)
                        {
                            var tankdetails = tanks.Where(top => top.JobId == firstItem.JobId && top.AssetId == firstItem.AssetId).FirstOrDefault();
                            JobProductTypeDetails model = new JobProductTypeDetails();
                            response.JobId = firstItem.JobId;
                            response.JobName = firstItem.JobName;
                            model.FuelTypeId = firstProduct.ProductTypeId;
                            model.Priority = DeliveryReqPriority.MustGo;
                            model.JobId = firstItem.JobId;
                            model.UoM = firstItem.UoM;
                            model.ProductType = firstProduct.ProductTypeName;
                            product.GroupBy(t => t.AcceptedCompanyId).Select(t => t.FirstOrDefault()).ToList().ForEach(t => model.Orders.Add(new DropdownDisplayExtendedItem() { Id = t.OrderId, Name = t.TfxPoNumber, Code = t.AcceptedCompanyId.ToString() }));


                            if (brandedCompanyId > 0)
                            {
                                product.Where(top => top.AcceptedCompanyId == brandedCompanyId).GroupBy(t => t.AcceptedCompanyId).Select(t => t.FirstOrDefault()).ToList().ForEach(t => model.SupplierCompanies.Add(new DropdownDisplayItem() { Id = t.AcceptedCompanyId, Name = t.SupplierCompanyName }));
                            }
                            else
                            {
                                product.GroupBy(t => t.AcceptedCompanyId).Select(t => t.FirstOrDefault()).ToList().ForEach(t => model.SupplierCompanies.Add(new DropdownDisplayItem() { Id = t.AcceptedCompanyId, Name = t.SupplierCompanyName }));
                            }

                            if (model.SupplierCompanies.Count == 1)
                            {
                                model.SupplierCompanyId = model.SupplierCompanies.Select(t => t.Id).FirstOrDefault();
                            }
                            if (tankdetails != null)
                            {
                                model.IsRetainJob = true;
                                model.AssetId = tankdetails.AssetId;
                                model.TankId = tankdetails.TankId;
                                model.StorageId = tankdetails.StorageId;
                                model.JobDisplayId = tankdetails.JobDisplayId;
                            }
                            model.OtherProductsNames = await GetOtherFuelTypes(jobId);
                            response.ProductTypes.Add(model);
                        }
                    }
                }
            }

            return response;
        }

        public async Task<List<JobListWithProductTypes>> GetJobListWithProductTypes(int userId, int companyId, decimal latitude, decimal longitude, int jobId = 0, int brandedSuppCompId = 0)
        {
            var storedProcedureDomain = new StoredProcedureDomain(this);
            var helperDomain = new HelperDomain(this);
            var response = new List<JobListWithProductTypes>();
            var jobs = await storedProcedureDomain.GetBuyerJobsWithProductTypes(userId, companyId, jobId);
            var jobIds = jobs.Select(t => t.JobId).Distinct().ToList();
            if (brandedSuppCompId > 0)
            {
                jobIds = GetBrandedSupplierLocations(brandedSuppCompId, true, jobIds);
                jobs = jobs.Where(t => jobIds.Contains(t.JobId)).ToList();
            }

            var recurringSchedules = await new FreightServiceDomain(this).GetJobRecurringSchedules(jobIds);
            foreach (var job in jobs.GroupBy(t => t.JobId))
            {
                JobListWithProductTypes jobDetails = new JobListWithProductTypes() { JobId = job.Key };
                var firstItem = job.FirstOrDefault();
                if (firstItem != null)
                {
                    jobDetails.JobName = firstItem.JobName;
                    jobDetails.UoM = firstItem.UoM;
                    jobDetails.Distance = helperDomain.CalculateDistance(latitude, longitude, firstItem.Latitude, firstItem.Longitude);
                    foreach (var product in job.GroupBy(t => t.ProductTypeId))
                    {
                        JobProductTypes productType = new JobProductTypes();
                        var firstProduct = product.FirstOrDefault();
                        if (firstProduct != null)
                        {
                            productType.ProductTypeId = firstProduct.ProductTypeId;
                            productType.ProductTypeName = firstProduct.ProductTypeName;
                            product.GroupBy(t => t.AcceptedCompanyId).Select(t => t.FirstOrDefault()).ToList().ForEach(t => productType.Suppliers.Add(new DropdownDisplayItem() { Id = t.AcceptedCompanyId, Name = t.SupplierCompanyName }));
                            jobDetails.JobProductTypes.Add(productType);
                            productType.RecurringSchedules = recurringSchedules.Where(t => t.JobId == firstItem.JobId && t.ProductTypeId == firstProduct.ProductTypeId).ToList();
                        }
                    }
                    response.Add(jobDetails);
                }
            }
            response = response.OrderBy(t => t.Distance).ToList();
            return response;
        }

        public async Task<List<MapViewModel>> GetMapDataAsync(int userId, JobFilterViewModel filter = null)
        {
            var response = new List<MapViewModel>();
            try
            {
                var helperDomain = new HelperDomain(this);
                var jobIds = await helperDomain.GetJobIdsAsync(userId);

                var user = await Context.DataContext.Users.FirstOrDefaultAsync(t => t.Id == userId);
                if (user != null && jobIds != null)
                {
                    var jobs = Context.DataContext.Jobs.Include("MstCountry").Include("MstState").Include("Users1")
                                    .Where(t => t.IsActive && t.CountryId == filter.CountryId && t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open && jobIds.Contains(t.Id));
                    jobs = ApplyJobFilters(filter, user, jobs);
                    await jobs.OrderByDescending(t => t.Id).ForEachAsync(t => response.Add(t.ToMapViewModel()));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetMap", ex.Message, ex);
            }
            return response;
        }

        public async Task<ReOpenJobViewModel> GetReOpenAsync(int jobId)
        {
            ReOpenJobViewModel response = new ReOpenJobViewModel(Status.Success);
            try
            {
                var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.Id == jobId);
                if (job != null)
                {
                    response.EndDate = job.EndDate;
                    response.StartDate = Convert.ToString(job.StartDate.Date);
                    response.IsEndDate = job.EndDate == null ? false : true;
                    response.JobId = job.Id;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetReOpenAsync", ex.Message, ex);
            }
            return response;
        }
        public async Task<DeliveryDaysViewModel> GetObject(DeliveryDaysViewModel response, string count)
        {
            try
            {
                response.Count = Convert.ToInt32(count);
                response.FromDeliveryTime = "8.00 AM";
                response.ToDeliveryTime = "5.00 PM";
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetObject", ex.Message, ex);
            }
            return response;
        }

        public async Task<string> FindDaysCase(string valueCheck)
        {
            string dayEnum = "0";
            try
            {
                switch (valueCheck)
                {

                    case "Monday":
                        dayEnum = "1";
                        break;
                    case "Tuesday":
                        dayEnum = "2";
                        break;
                    case "Wednesday":
                        dayEnum = "3";
                        break;
                    case "Thursday":
                        dayEnum = "4";
                        break;
                    case "Friday":
                        dayEnum = "5";
                        break;
                    case "Saturday":
                        dayEnum = "6";
                        break;
                    case "Sunday":
                        dayEnum = "7";
                        break;
                    default:
                        dayEnum = "0";
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "FindDaysCase", ex.Message, ex);
            }

            return dayEnum;
        }

        public async Task<string> ConstructFinalStringDays(string count, int id)
        {
            string[] data = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            string[] words = count.Split('×');

            string[] c = data.Where(x => !words.Contains(x)).ToArray();
            string stringArray = string.Empty;
            string FinalString = string.Empty;
            try
            {
                for (int i = 0; i < c.Length; i++)
                {
                    FinalString = await ContextFactory.Current.GetDomain<JobDomain>().FindDaysCase(c[i]);

                    if (c.Length > 1 && i + 1 != c.Length)
                    {
                        stringArray += FinalString + ",";
                    }
                    else
                    {
                        stringArray += FinalString;
                    }
                    FinalString = string.Empty;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "ConstructFinalStringDays", ex.Message, ex);
            }

            return stringArray;
        }

        public async Task<StatusViewModel> ReOpenJobAsync(UserContext userContext, ReOpenJobViewModel viewModel, int userId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.Id == viewModel.JobId);
                    if (job != null)
                    {
                        if (viewModel.IsEndDate)
                        {
                            job.EndDate = viewModel.EndDate;
                        }
                        else
                        {
                            job.EndDate = null;
                        }

                        job.ReopenDate = DateTimeOffset.Now;
                        job.IsReopened = true;
                        job.JobXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                        JobXStatus jobStatus = new JobXStatus();
                        jobStatus.StatusId = (int)JobStatus.Open;
                        jobStatus.IsActive = true;
                        jobStatus.UpdatedBy = userId;
                        jobStatus.UpdatedDate = DateTimeOffset.Now;
                        job.JobXStatuses.Add(jobStatus);

                        Context.DataContext.Entry(job).State = EntityState.Modified;
                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageJobReopenedSuccess;
                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetJobReOpenedNewsFeed(userContext, job);

                        //Add an entry to notifications
                        //Commemted as JobReOpen Event is not yet Implemented
                        //await ContextFactory.Current.GetDomain<NotificationDomain>()
                        //      .AddNotificationEventAsync(EventType.JobReopen, job.Id, job.UpdatedBy);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("JobDomain", "ReOpenJobAsync", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> CloseJobAsync(UserContext userContext, int jobId, int userId)
        {
            StatusViewModel response = new StatusViewModel();

            try
            {
                var orders = Context.DataContext.Orders.Where(t => t.IsActive
                                                                && t.FuelRequest.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest
                                                                && t.FuelRequest.Job.Id == jobId);
                if (orders != null)
                {
                    var hasOpenOrders = orders.Any(t => t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open);
                    var hasNonApprovedInvoice = orders.Any(t =>
                                                        t.Invoices.Any(t1 => t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t1.IsBuyPriceInvoice &&
                                                        t1.InvoiceXInvoiceStatusDetails.Any(t2 => t2.IsActive &&
                                                        ((t2.StatusId == (int)InvoiceStatus.Received && t2.Invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual
                                                          && t2.Invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp) ||
                                                          t2.StatusId == (int)InvoiceStatus.Rejected || t2.StatusId == (int)InvoiceStatus.WaitingForApproval))));

                    if (hasOpenOrders)
                    {
                        response.StatusMessage = Resource.errMessageCannotCloseJobWithOpenOrders;
                    }
                    else if (hasNonApprovedInvoice)
                    {
                        response.StatusMessage = Resource.errMessageCannotCloseJobWithOpenInvoices;
                    }
                    else
                    {
                        var user = await Context.DataContext.Users.FirstOrDefaultAsync(t => t.Id == userId);
                        if (user != null)
                        {
                            var job = user.Company.Jobs.FirstOrDefault(t => t.Id == jobId && t.IsActive);
                            if (job != null)
                            {
                                job.JobXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                                JobXStatus jobStatus = new JobXStatus();
                                jobStatus.StatusId = (int)JobStatus.Closed;
                                jobStatus.IsActive = true;
                                jobStatus.UpdatedBy = userId;
                                jobStatus.UpdatedDate = DateTimeOffset.Now;
                                job.JobXStatuses.Add(jobStatus);

                                job.UpdatedBy = userId;
                                job.UpdatedDate = DateTimeOffset.Now;

                                job.FuelRequests.Where(t => t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Open
                                            && t.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest).ToList().ForEach(t =>
                                            {
                                                t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).IsActive = false;
                                                FuelRequestXStatus frStatus = new FuelRequestXStatus();
                                                frStatus.StatusId = (int)FuelRequestStatus.Canceled;
                                                frStatus.IsActive = true;
                                                frStatus.UpdatedBy = userId;
                                                frStatus.UpdatedDate = DateTimeOffset.Now;
                                                t.FuelRequestXStatuses.Add(frStatus);

                                                t.UpdatedBy = userId;
                                                t.UpdatedDate = DateTimeOffset.Now;
                                            });

                                Context.DataContext.Entry(job).State = EntityState.Modified;
                                await Context.CommitAsync();
                                await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetJobClosedNewsFeed(userContext, job);
                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.errMessageJobClosedSuccess;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "CloseJobAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<int>> GetOpenJobsWithEndDateAsync()
        {
            var response = new List<int>();
            try
            {
                response = await Context.DataContext.Jobs.Where(
                                                t =>
                                                t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open &&
                                                t.EndDate != null &&
                                                t.IsActive).Select(t => t.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetOpenJobsWithEndDateAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task ProcessJobClosureAsync(UserContext userContext, int id)
        {
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var entity = await Context.DataContext.Jobs.SingleOrDefaultAsync(t => t.Id == id);
                    if (entity != null)
                    {
                        var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(entity.TimeZoneName);
                        if (entity.EndDate.Value.Date < currentDate.Date)
                        {
                            if (entity.FuelRequests.Any(t => t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Accepted && t.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest))
                            {
                                var orders = entity
                                                .FuelRequests
                                                .Where(t => t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Accepted
                                                            && t.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest)
                                                .Select(t => t.Orders.Last())
                                                .ToList();
                                if (orders.Any(t1 => t1.OrderXStatuses.FirstOrDefault(t2 => t2.IsActive).StatusId == (int)OrderStatus.Open) ||
                                    orders.Any(t2 => t2.Invoices.Any(
                                                                t3 => t3.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t3.IsBuyPriceInvoice && t3.InvoiceXInvoiceStatusDetails.Any(t4 => t4.IsActive &&
                                                                ((t4.StatusId == (int)InvoiceStatus.Received && t4.Invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual
                                                                  && t4.Invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp) ||
                                                                  t4.StatusId == (int)InvoiceStatus.Rejected || t4.StatusId == (int)InvoiceStatus.WaitingForApproval))
                                                                    )))
                                {
                                    return;
                                }
                            }

                            if (entity.FuelRequests.Any(t => t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Open && t.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest))
                            {
                                var fuelRequests = entity
                                                    .FuelRequests
                                                    .Where(t => t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Open
                                                                && t.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest)
                                                    .ToList();
                                foreach (var item in fuelRequests)
                                {
                                    item.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).IsActive = false;
                                    FuelRequestXStatus frStatus = new FuelRequestXStatus();
                                    frStatus.StatusId = (int)FuelRequestStatus.Canceled;
                                    frStatus.IsActive = true;
                                    frStatus.UpdatedBy = (int)SystemUser.System;
                                    frStatus.UpdatedDate = DateTimeOffset.Now;
                                    item.FuelRequestXStatuses.Add(frStatus);
                                }
                            }

                            //If control came to this point then Job has to be closed
                            entity.JobXStatuses.First(t => t.IsActive).IsActive = false;
                            JobXStatus jobStatus = new JobXStatus();
                            jobStatus.StatusId = (int)JobStatus.Closed;
                            jobStatus.IsActive = true;
                            jobStatus.UpdatedBy = (int)SystemUser.System;
                            jobStatus.UpdatedDate = DateTimeOffset.Now;
                            entity.JobXStatuses.Add(jobStatus);

                            Context.DataContext.Entry(entity).State = EntityState.Modified;
                            await Context.CommitAsync();
                            await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetJobClosedNewsFeed(userContext, entity);

                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("JobDomain", "ProcessJobClosureAsync", ex.Message, ex);
                }
            }
        }

        public async Task<JobViewModel> GetJobDetailsAsync(int jobId)
        {
            JobViewModel response = new JobViewModel();

            try
            {
                var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.Id == jobId && t.IsActive);
                if (job != null)
                {
                    response = job.ToViewModel();
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.errMessageSuccess;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobDetailsAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<JobSelectionViewModel> GetJobByIdAsync(int jobId)
        {
            JobSelectionViewModel response = new JobSelectionViewModel();

            try
            {
                var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.Id == jobId && t.IsActive);
                if (job != null)
                {
                    response = job.ToJobViewModel();
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.errMessageSuccess;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobByIdAsync", ex.Message, ex);
            }

            return response;
        }

        public JobFilterViewModel GetJobFilterAsync(int jobId, JobFilterType filter)
        {
            var response = new JobFilterViewModel();
            try
            {
                response.Id = jobId;
                if (filter > 0)
                {
                    response.Filter = filter;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobFilterAsync", ex.Message, ex);
            }
            return response;
        }

        private IQueryable<Job> ApplyJobFilters(JobFilterViewModel filter, User user, IQueryable<Job> allJobs)
        {
            if (filter != null)
            {
                if (filter.Id > 0)
                {
                    allJobs = allJobs.Where(t => t.Id == filter.Id && t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open);
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter.StartDate))
                    {
                        var startDate = Convert.ToDateTime(filter.StartDate).Date;
                        allJobs = allJobs.Where(t => t.StartDate >= startDate || t.CreatedDate >= startDate);
                    }
                    if (!string.IsNullOrEmpty(filter.EndDate))
                    {
                        var endDate = Convert.ToDateTime(filter.EndDate).Date.AddDays(1);
                        allJobs = allJobs.Where(t => t.StartDate <= endDate || t.CreatedDate <= endDate);
                    }
                }

                if (filter.Filter == JobFilterType.OverBudget)
                {
                    allJobs = allJobs
                                .Where(t => t.JobBudget.Budget > 0 &&
                                t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open &&
                                (((t.HedgeDroppedAmount + t.SpotDroppedAmount) * 100) / t.JobBudget.Budget)
                                   >= user.Company.BudgetAlertPercentage);
                }
                else if (filter.Filter == JobFilterType.UnderBudget)
                {
                    allJobs = allJobs
                                .Where(t => t.JobBudget.Budget > 0 &&
                                t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open &&
                                (((t.HedgeDroppedAmount + t.SpotDroppedAmount) * 100) / t.JobBudget.Budget)
                                   <= 100);
                }
                else if (filter.Filter == JobFilterType.TotalBudget)
                {
                    allJobs = allJobs
                            .Where(t => t.JobBudget.Budget > 0 &&
                            t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open);
                }
                else if (filter.Filter == JobFilterType.NoBudget)
                {
                    allJobs = allJobs.Where(t => t.JobBudget.BudgetCalculationTypeId == (int)BudgetCalculationType.NoBudget && t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open);
                }
                else if (filter.Filter == JobFilterType.OpenJobs)
                {
                    allJobs = allJobs.Where(t => t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open);
                }
            }

            return allJobs;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetOpenJobsByCompanyType(CompanyType companyType, int userId, int companyId, int customerCompanyId)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                List<int> jobIds = new List<int>();

                if (companyType == CompanyType.Buyer || companyType == CompanyType.BuyerAndSupplier || companyType == CompanyType.BuyerSupplierAndCarrier)
                {
                    var helperDomain = new HelperDomain(this);
                    var bJobIds = await helperDomain.GetJobIdsAsync(userId);
                    if (bJobIds.Any())
                    {
                        jobIds.AddRange(bJobIds);
                    }
                }

                if ((companyType == CompanyType.Carrier || companyType == CompanyType.BuyerSupplierAndCarrier || companyType == CompanyType.SupplierAndCarrier) && (customerCompanyId > 0))
                {
                    var cJobIds = await new FreightServiceDomain(this).GetCarriersJobs(companyId, customerCompanyId);
                    if (cJobIds.Any())
                    {
                        jobIds.AddRange(cJobIds);
                    }
                }

                if (companyType == CompanyType.Supplier || companyType == CompanyType.BuyerSupplierAndCarrier || companyType == CompanyType.SupplierAndCarrier || companyType == CompanyType.BuyerAndSupplier)
                {
                    var sJobIds = await Context.DataContext.Orders.Where(t =>
                                t.AcceptedCompanyId == companyId && t.BuyerCompanyId == customerCompanyId && t.IsActive && t.FuelRequest.Job.JobXAssets
                                .Any(x1 => x1.RemovedBy == null && x1.Asset.Type == (int)AssetType.Tank && x1.Asset.IsActive && x1.Job.IsActive && x1.Job.DisplayJobID != null && x1.Job.DisplayJobID.Trim() != ""))
                                .Select(t1 => t1.FuelRequest.JobId).ToListAsync();

                    if (sJobIds.Any())
                    {
                        jobIds.AddRange(sJobIds);
                    }
                }
                if (jobIds.Any())
                {

                    jobIds = jobIds.Distinct().ToList();

                    response = Context.DataContext.Jobs
                          .Where(t => jobIds.Contains(t.Id) && !string.IsNullOrEmpty(t.DisplayJobID) && t.IsActive &&
                              t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open &&
                              t.JobXAssets.Any(t2 => t2.Asset.Type == (int)AssetType.Tank && t2.RemovedBy == null && t2.RemovedDate == null))
                                .Select(t =>
                                    new DropdownDisplayExtendedItem()
                                    {
                                        Id = t.Id,
                                        Name = t.Name,
                                        Code = t.DisplayJobID
                                    }).ToList();
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetOpenJobsByAssetType", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<JobViewModel>> GetJobsByCompanyType(CompanyType companyType, int userId, int companyId, int customerCompanyId, int brandedCompanyId = -1, bool isBuyerCompany = false)
        {
            var response = new List<JobViewModel>();
            try
            {
                List<int> jobIds = new List<int>();

                if (companyType == CompanyType.Buyer || companyType == CompanyType.BuyerAndSupplier || companyType == CompanyType.BuyerSupplierAndCarrier)
                {
                    var helperDomain = new HelperDomain(this);
                    var bJobIds = await helperDomain.GetJobIdsAsync(userId);
                    if (bJobIds.Any())
                    {
                        jobIds.AddRange(bJobIds);
                    }
                }

                if ((companyType == CompanyType.Carrier || companyType == CompanyType.BuyerSupplierAndCarrier || companyType == CompanyType.SupplierAndCarrier) && (customerCompanyId > 0))
                {
                    var cJobIds = await new FreightServiceDomain(this).GetCarriersJobs(companyId, customerCompanyId);
                    if (cJobIds.Any())
                    {
                        jobIds.AddRange(cJobIds);
                    }
                }

                if (companyType == CompanyType.Supplier || companyType == CompanyType.BuyerSupplierAndCarrier || companyType == CompanyType.SupplierAndCarrier || companyType == CompanyType.BuyerAndSupplier)
                {
                    var sJobIds = await Context.DataContext.Orders.Where(t =>
                                t.AcceptedCompanyId == companyId && t.BuyerCompanyId == customerCompanyId && t.IsActive && t.FuelRequest.Job.JobXAssets
                                .Any(x1 => x1.RemovedBy == null && x1.Asset.Type == (int)AssetType.Tank && x1.Asset.IsActive && x1.Job.IsActive && x1.Job.DisplayJobID != null && x1.Job.DisplayJobID.Trim() != ""))
                                .Select(t1 => t1.FuelRequest.JobId).ToListAsync();

                    if (sJobIds.Any())
                    {
                        jobIds.AddRange(sJobIds);
                    }
                }
                if (jobIds.Any())
                {

                    jobIds = jobIds.Distinct().ToList();
                    jobIds = GetBrandedSupplierLocations(brandedCompanyId, isBuyerCompany, jobIds);
                    response = Context.DataContext.Jobs
                          .Where(t => jobIds.Contains(t.Id) && !string.IsNullOrEmpty(t.DisplayJobID) && t.IsActive &&
                              t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open)
                                .Select(t =>
                                    new JobViewModel()
                                    {
                                        Id = t.Id,
                                        Name = t.Name,
                                        JobID = t.DisplayJobID,
                                        IsRetailJob = t.IsRetailJob
                                    }).ToList();
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobsByCompanyType", ex.Message, ex);
            }
            return response;
        }

        public List<int> GetBrandedSupplierLocations(int brandedCompanyId, bool isBuyerCompany, List<int> jobIds)
        {
            if (brandedCompanyId > 0 && isBuyerCompany)
            {
                jobIds = (from jobs in Context.DataContext.Jobs
                          join fuelRequest in Context.DataContext.FuelRequests
                          on jobs.Id equals fuelRequest.JobId
                          join orders in Context.DataContext.Orders
                          on fuelRequest.Id equals orders.FuelRequestId
                          where jobIds.Contains(jobs.Id) && jobs.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open
                          && orders.OrderXStatuses.Any(t1 => t1.IsActive && t1.StatusId == (int)OrderStatus.Open)
                          && orders.AcceptedCompanyId == brandedCompanyId
                          select new
                          {
                              jobs.Id
                          }).Distinct().Select(top => top.Id).ToList();
            }

            return jobIds;
        }

        public async Task<List<TankDetailViewModel>> GetTanksByJobId(int companyId, int jobId)
        {
            var response = new List<TankDetailViewModel>();
            try
            {
                var assetIds = await Context.DataContext.JobXAssets.Where(t =>
                                    t.Job.CompanyId == companyId &&
                                    t.JobId == jobId &&
                                    t.Asset.Type == (int)AssetType.Tank &&
                                    t.RemovedBy == null).Select(t => t.AssetId).Distinct().ToListAsync();

                if (assetIds.Any())
                {
                    List<TankDetailViewModel> result = await new FreightServiceDomain(this).GetTankList(assetIds);

                    if (result.Any())
                    {
                        response = result.GroupBy(x => x.AssetId).Select(x => x.FirstOrDefault()).ToList();

                        foreach (var item in response)
                        {
                            item.ProductTypeName = Enum.GetName(typeof(ProductTypes), item.FuelTypeId);
                            item.UoM = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => new { t.UoM }).FirstOrDefault().UoM;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetAssetsByJobId", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<int>> GetAssetIdsByJobForDipTest(int companyId, int jobId)
        {
            var response = new List<int>();
            try
            {
                response = await Context.DataContext.JobXAssets.Where(t =>
                                    t.Job.CompanyId == companyId &&
                                    t.JobId == jobId &&
                                    t.Asset.Type == (int)AssetType.Tank &&
                                    t.RemovedBy == null).Select(t => t.AssetId).Distinct().ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetTanksForTrend", ex.Message, ex);
            }
            return response;
        }
        public string GetDisplayJobIdByJobIdForJob(int jobId, ref string timeZoneName)
        {
            string response = "";
            try
            {
                var job = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => new { t.DisplayJobID, t.TimeZoneName }).FirstOrDefault();
                response = job.DisplayJobID;
                timeZoneName = job.TimeZoneName;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetDisplayJobIdByJobIdForJob", ex.Message, ex);
            }
            return response;
        }
        public async Task UploadJobAssetFileToBlob(UserContext userContext, int jobId, Stream fileStream, string fileName)
        {
            LogManager.Logger.WriteDebug("JobDomain", "UploadJobAssetFileToBlob", "Start");
            try
            {
                var azureBlob = new AzureBlobStorage();
                string filename = GenerateFileName(jobId);
                await azureBlob.SaveBlobAsync(fileStream, filename, BlobContainerType.JobAssetbulkupload.ToString().ToLower());
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "UploadJobAssetFileToBlob", ex.Message, ex);
            }
            LogManager.Logger.WriteDebug("JobDomain", "UploadJobAssetFileToBlob", "End");
        }

        private string GenerateFileName(int jobId)
        {
            return string.Concat(values: Constants.JobAssetBulk + Resource.lblSingleHyphen + jobId + Resource.lblSingleHyphen + DateTime.Now.Ticks + ".csv");
        }
        private string GenerateJobFileName(int userId)
        {
            return string.Concat(values: Constants.JobBulkUpload + Resource.lblSingleHyphen + userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + ".csv");
        }

        public async Task<StatusViewModel> SaveJobBulkAssetsAsync(string csvText, UserContext userContext, int jobId)
        {
            string fileName = GenerateFileName(jobId);
            LogManager.Logger.WriteDebug("JobDomain", "SaveJobBulkAssetsAsync", "Start : JobAssetBulkUpload - " + jobId + " - " + fileName);
            var response = new StatusViewModel();
            List<Asset> assets = new List<Asset>();
            List<string> failedRecords = new List<string>();
            try
            {
                LogManager.Logger.WriteInfo("JobDomain", "SaveJobBulkAssetsAsync", "\n\n[" + csvText + "]\n\n");

                var engine = new FileHelperEngine<JobAssetCsvViewModel>();
                var csvAssetList = engine.ReadString(csvText).ToList();
                csvAssetList = csvAssetList.Skip(1).ToList(); // skip header

                string operationPerformed = string.Empty;
                int addCount = 0;
                int updateCount = 0;
                int deleteCount = 0;
                AddAllSubcontractors(csvAssetList, userContext.Id, jobId, out addCount);
                var existingSubcontractors = Context.DataContext.Subcontractors.Where(t => t.Jobs.Any(t1 => t1.CompanyId == userContext.CompanyId)).ToList();
                var uniqueAssets = csvAssetList.Where(t => !string.IsNullOrWhiteSpace(t.EquipmentType) || !string.IsNullOrWhiteSpace(t.EquipmentId)).GroupBy(i => new { i.EquipmentType, i.EquipmentId }).Select(g => g.Last()).ToList();

                foreach (var item in uniqueAssets)
                {
                    var asset = GetAssetObject(item, userContext.Id, userContext.CompanyId, existingSubcontractors, out operationPerformed);
                    if (asset != null)
                    {
                        assets.Add(asset);
                    }
                    else
                    {
                        failedRecords.Add(item.EquipmentType);
                        response.StatusMessage = string.Format(Resource.errMessageJobAssetUploadFailedType,
                            string.Join(",", failedRecords));
                        return response;
                    }

                    switch (operationPerformed)
                    {
                        case ApplicationConstants.Update:
                            updateCount++;
                            break;
                        case ApplicationConstants.Delete:
                            deleteCount++;
                            break;
                        default:
                            break;
                    }
                }

                response = await SaveAssetList(userContext, jobId, assets);

                //// newsfeed bulk upload subcontractor
                var job = Context.DataContext.Jobs.FirstOrDefault(t => t.IsActive && t.Id == jobId);
                await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetSubcontractorAssetAssignNewsfeedBulkUpload(userContext, NewsfeedEvent.SubContractorBulkUploadAsset, job, addCount, updateCount, deleteCount);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "SaveJobBulkAssetsAsync", ex.Message, ex);
            }

            LogManager.Logger.WriteDebug("JobDomain", "SaveJobBulkAssetsAsync", "End");
            return response;
        }

        private async Task<StatusViewModel> SaveAssetList(UserContext userContext, int jobId, List<Asset> assets)
        {
            var response = new StatusViewModel();
            List<string> failedAssets = new List<string>();
            bool isAssetTrackingEnabled = false;
            int newAssetCnt = 0, assetExistButOtherJobCnt = 0, assetExistButNotAssignCnt = 0;
            var job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == jobId);
            try
            {
                var user = Context.DataContext.Users.FirstOrDefault(t => t.Id == userContext.Id);
                var uniqueAssets = assets.GroupBy(i => new { i.Name }).Select(g => g.Last()).ToList();

                foreach (var asset in uniqueAssets)
                {
                    try
                    {
                        if (!user.Company.Assets.Any(t => t.IsActive && t.Name.Equals(asset.Name, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            newAssetCnt++;
                            user.Company.Assets.Add(asset);
                            job.JobXAssets.Add(new JobXAsset
                            {
                                Asset = asset,
                                JobId = jobId,
                                AssignedBy = userContext.Id,
                                AssignedDate = DateTimeOffset.Now
                            });
                        }
                        else
                        {
                            var currentJobXAsset = Context.DataContext.JobXAssets.FirstOrDefault(t => t.AssetId == asset.Id && t.RemovedDate == null);// get current assignment
                            if (currentJobXAsset != null)
                            {
                                if (currentJobXAsset.JobId != jobId)
                                {
                                    assetExistButOtherJobCnt++;
                                    // asset already exists but it is assigned to different job than current job
                                    currentJobXAsset.RemovedBy = userContext.Id;
                                    currentJobXAsset.RemovedDate = DateTimeOffset.Now;
                                    job.JobXAssets.Add(new JobXAsset
                                    {
                                        Asset = asset,
                                        JobId = jobId,
                                        AssignedBy = userContext.Id,
                                        AssignedDate = DateTimeOffset.Now
                                    });
                                }
                            }
                            else
                            {
                                assetExistButNotAssignCnt++;
                                // asset already exists but it is not assigned to any job
                                job.JobXAssets.Add(new JobXAsset
                                {
                                    Asset = asset,
                                    JobId = jobId,
                                    AssignedBy = userContext.Id,
                                    AssignedDate = DateTimeOffset.Now
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        failedAssets.Add(asset.Name);
                        response.StatusMessage = string.Format(Resource.errMessageJobAssetUploadFailedTypeId, string.Join(",", failedAssets));
                        LogManager.Logger.WriteException("AssetDomain", "SaveBulkAssetsAsync", ex.Message, ex);
                        return response;
                    }
                }
                if (!job.JobBudget.IsAssetTracked)
                {
                    job.JobBudget.IsAssetTracked = true;
                    isAssetTrackingEnabled = true;
                }
            }
            catch (Exception ex)
            {
                if (failedAssets.Count > 0)
                {
                    response.StatusMessage = string.Format(Resource.errMessageJobAssetUploadFailedTypeId, string.Join(",", failedAssets));
                }
                LogManager.Logger.WriteException("AssetDomain", "SaveBulkAssetsAsync", ex.Message, ex);
                return response;
            }

            if (failedAssets.Count > 0)
            {
                response.StatusMessage = string.Format(Resource.errMessageJobAssetUploadFailedTypeId,
                    string.Join(",", failedAssets));
                return response;
            }

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    await Context.CommitAsync();
                    transaction.Commit();
                    if (isAssetTrackingEnabled)
                    {
                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetAssetTrackingEnableDisableNewsfeed(userContext, job, isAssetTrackingEnabled);
                    }
                    response.StatusCode = Status.Success;
                    response.StatusMessage = isAssetTrackingEnabled ? string.Format(Resource.errAssetsUploadedAndTrackingEnabled, new object[] { job.Name }) : Resource.errMessageBulkUploadSuccess;

                    if (newAssetCnt > 0 || assetExistButOtherJobCnt > 0 || assetExistButNotAssignCnt > 0)
                    {
                        StringBuilder resStatusMessage = new StringBuilder();
                        if (newAssetCnt > 0)
                        {
                            resStatusMessage.Append(string.Format(Resource.errMessageJobAssetsUploadedSuccessfully, newAssetCnt) + "<br/>");
                            if (newAssetCnt != assets.Count() && assetExistButNotAssignCnt == 0 && assetExistButOtherJobCnt == 0)
                            {
                                resStatusMessage.Append(string.Format(Resource.errMessageAssetsAlreadyExists, (assets.Count() - newAssetCnt)) + "<br/>");
                            }
                        }
                        if (assetExistButOtherJobCnt > 0)
                        {
                            resStatusMessage.Append(string.Format(Resource.errMessageAssetsExistsButAssignedToAnotherJob, assetExistButOtherJobCnt) + "<br/>");
                        }
                        if (assetExistButNotAssignCnt > 0)
                        {
                            resStatusMessage.Append(string.Format(Resource.errMessageAssetsExistsButNotAssignedToAnyJob, assetExistButNotAssignCnt) + "<br/>");
                        }
                        response.StatusMessage = isAssetTrackingEnabled ? response.StatusMessage + "<br/>" + resStatusMessage.ToString() : resStatusMessage.ToString();
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageJobAssetsAlreadyExists;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("AssetDomain", "SaveBulkAssetsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        private Asset GetAssetObject(JobAssetCsvViewModel csvRecord, int userId, int companyId, List<Subcontractor> subcontractors, out string operationPerformed, Asset entity = null)
        {
            LogManager.Logger.WriteDebug("JobDomain", "GetAssetObject", "Start");
            operationPerformed = string.Empty;
            try
            {
                bool addNew = false;
                string assetName = string.Format(@"{0}{1}{2}",
                                    csvRecord.EquipmentType,
                                    !string.IsNullOrWhiteSpace(csvRecord.EquipmentType) && !string.IsNullOrWhiteSpace(csvRecord.EquipmentId) ? Resource.lblSingleHyphen : string.Empty,
                                    csvRecord.EquipmentId);
                var matchingSubcontractor = subcontractors.FirstOrDefault(t => t.Name.Equals(csvRecord.Subcontractor, StringComparison.InvariantCultureIgnoreCase));
                entity = Context.DataContext.Assets.FirstOrDefault(t => t.Name.Equals(assetName, StringComparison.InvariantCultureIgnoreCase) && t.CompanyId == companyId);
                var assignSubcontractorToAsset = true;

                if (entity == null)
                {
                    entity = new Asset
                    {
                        Name = assetName,
                        CreatedDate = DateTimeOffset.Now,
                        IsActive = true,
                        UpdatedDate = DateTimeOffset.Now,
                        UpdatedBy = userId
                    };
                    entity.AssetAdditionalDetail = new AssetAdditionalDetail
                    {
                        Make = csvRecord.Make,
                        Model = csvRecord.Model,
                        VehicleId = csvRecord.EquipmentId,
                        IsActive = true,
                        UpdatedDate = DateTimeOffset.Now,
                        UpdatedBy = userId
                    };
                    if (!string.IsNullOrWhiteSpace(csvRecord.Contract))
                    {
                        AssetContractNumber assetContractNumber = new AssetContractNumber() { ContractNumber = csvRecord.Contract, AddedBy = userId, AddedDate = DateTimeOffset.Now, IsActive = true };
                        entity.AssetContractNumbers.Add(assetContractNumber);
                    }
                }
                else
                {
                    var contractNumber = entity.AssetContractNumbers.FirstOrDefault(t => t.IsActive);
                    if (contractNumber != null && contractNumber.ContractNumber != csvRecord.Contract)
                    {
                        addNew = true;
                        contractNumber.IsActive = false;
                        contractNumber.RemovedBy = userId;
                        contractNumber.RemovedDate = DateTimeOffset.Now;
                    }
                    if ((addNew || contractNumber == null) && !string.IsNullOrWhiteSpace(csvRecord.Contract))
                    {
                        AssetContractNumber assetContractNumber = new AssetContractNumber() { ContractNumber = csvRecord.Contract, AddedBy = userId, AddedDate = DateTimeOffset.Now, IsActive = true };
                        entity.AssetContractNumbers.Add(assetContractNumber);
                    }

                    entity.AssetAdditionalDetail.VehicleId = csvRecord.EquipmentId;
                    var currentAssetSubcontractor = Context.DataContext.AssetSubcontractors.FirstOrDefault(t => t.AssetId == entity.Id && t.IsActive);
                    if (currentAssetSubcontractor != null)
                    {
                        if (matchingSubcontractor == null) // blank subcontractor
                        {
                            // asset is already assigned to another subcontractor
                            currentAssetSubcontractor.IsActive = false;
                            currentAssetSubcontractor.RemovedBy = userId;
                            currentAssetSubcontractor.RemovedDate = DateTimeOffset.Now;
                            assignSubcontractorToAsset = false;
                            operationPerformed = ApplicationConstants.Delete;
                        }
                        else
                        {
                            if (currentAssetSubcontractor.SubcontractorId == matchingSubcontractor.Id)
                            {
                                assignSubcontractorToAsset = false;
                            }
                            else
                            {
                                // asset is already assigned to another subcontractor
                                currentAssetSubcontractor.IsActive = false;
                                currentAssetSubcontractor.RemovedBy = userId;
                                currentAssetSubcontractor.RemovedDate = DateTimeOffset.Now;
                                operationPerformed = ApplicationConstants.Update;
                            }
                        }
                    }
                }

                if (matchingSubcontractor != null && assignSubcontractorToAsset)
                {
                    AssetSubcontractor assetSubcontractor = new AssetSubcontractor()
                    {
                        SubcontractorId = matchingSubcontractor.Id,
                        AssignedBy = userId,
                        AssignedDate = DateTimeOffset.Now,
                        IsActive = true
                    };
                    entity.AssetSubcontractors.Add(assetSubcontractor);
                }
                LogManager.Logger.WriteDebug("JobDomain", "GetAssetObject", "End");
                return entity;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "ValidateCsvHeader", ex.Message, ex);
                return null;
            }
        }

        public StatusViewModel ValidateJobAssetCsvHeader(string csvText, string csvFilePath, string csvSfxAssetsFilePath, ref bool isSFXAssetsCsv)
        {
            LogManager.Logger.WriteDebug("JobDomain", "ValidateJobAssetCsvHeader", "Start");
            StatusViewModel response = new StatusViewModel();
            try
            {
                var csvHeaderLine = Regex.Matches(csvText.Trim(), @"^.*Equipment Type.*\n").Cast<Match>().FirstOrDefault();
                if (csvHeaderLine == null)
                {
                    csvHeaderLine = Regex.Matches(csvText.Trim(), @"^.*AssetName.*\n").Cast<Match>().FirstOrDefault();
                    csvFilePath = csvSfxAssetsFilePath;
                    isSFXAssetsCsv = true;
                }
                string[] lines = File.ReadAllLines(csvFilePath);
                string headerLine = lines.FirstOrDefault();
                if (csvHeaderLine.Value.Trim() == headerLine)
                {
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageBulkUploadHeaderMismatch;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "ValidateJobAssetCsvHeader", ex.Message, ex);
            }

            LogManager.Logger.WriteDebug("JobDomain", "ValidateJobAssetCsvHeader", "End");
            return response;
        }

        private StatusViewModel AddAllSubcontractors(List<JobAssetCsvViewModel> assets, int userId, int jobId, out int addCount)
        {
            LogManager.Logger.WriteDebug("JobDomain", "AddAllSubcontractors", "Start");

            addCount = 0;
            var response = new StatusViewModel();
            var uniqueSubcontractors = assets.Where(t => !string.IsNullOrWhiteSpace(t.Subcontractor)).GroupBy(t => t.Subcontractor).Select(t1 => t1.First());
            var existingSubcontractors = Context.DataContext.Subcontractors;

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == jobId);
                    foreach (var item in uniqueSubcontractors)
                    {
                        var subContractor = existingSubcontractors.FirstOrDefault(t => t.Name.Equals(item.Subcontractor, StringComparison.InvariantCultureIgnoreCase));
                        if (subContractor == null)
                        {
                            //// if subcontractor is not exist, add subcontractor and assign to job
                            var entity = new Subcontractor()
                            {
                                Name = item.Subcontractor,
                                IsActive = true,
                                UpdatedBy = userId,
                                UpdatedDate = DateTimeOffset.Now
                            };
                            entity.Jobs.Add(job);
                            Context.DataContext.Subcontractors.Add(entity);

                            addCount++;
                        }
                        else
                        {
                            //// if subcontractor is already exist, then assign that subcontractor to job
                            if (!subContractor.Jobs.Any(t => t.Id == job.Id))
                            {
                                subContractor.Jobs.Add(job);
                            }
                        }
                    }

                    Context.Commit();
                    transaction.Commit();

                    response.StatusCode = Status.Success;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusCode = Status.Failed;
                    LogManager.Logger.WriteException("JobDomain", "AddAllSubcontractors", ex.Message, ex);
                }
            }

            LogManager.Logger.WriteDebug("JobDomain", "AddAllSubcontractors", "End");
            return response;
        }

        public async Task<List<AuditReportViewModel>> GetAuditReportAsync(AuditDataTableViewModel auditDataViewModel, DataTableSearchModel dataTableSearchModel)
        {
            var response = new List<AuditReportViewModel>();
            using (var tracer = new Tracer("JobDomain", "GetAuditReportAsync"))
            {
                var spDomain = new StoredProcedureDomain(this);
                var dropDetails = await spDomain.GetAuditReportAsync(auditDataViewModel, dataTableSearchModel);
                foreach (var drop in dropDetails)
                {
                    var auditReport = new AuditReportViewModel();
                    var salesInput = new SalesCalculatorInputViewModel() { PricingDate = drop.DropStartDate, SrcLatitude = drop.Latitude, SrcLongitude = drop.Longitude, ExternalProductId = drop.ExternalProductId, RecordCount = 3 };
                    var terminalPrice = await new PricingServiceDomain().GetTerminalPricesForAuditAsync(salesInput);
                    foreach (var terminal in terminalPrice)
                    {
                        var nearestTerminal = new NearestTerminalsViewModel()
                        {
                            TerminalName = terminal.Name,
                            Distance = terminal.Distance.GetPreciseValue(2),
                            TerminalPPG = auditDataViewModel.PriceType == 2 ? terminal.PriceLow.GetPreciseValue(2) : auditDataViewModel.PriceType == 3 ? terminal.PriceHigh.GetPreciseValue(2) : terminal.PriceAvg.GetPreciseValue(2)
                        };
                        auditReport.NearestTerminals.Add(nearestTerminal);
                    }
                    auditReport.DropDetail = drop;
                    response.Add(auditReport);
                }
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetJobTankProductTypes(int jobId)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            response = await Context.DataContext.JobXAssets.Where(t => t.RemovedBy == null && t.JobId == jobId && t.Asset.Type == (int)AssetType.Tank).Select(t => new DropdownDisplayItem() { Id = t.Asset.MstProductType.Id, Name = t.Asset.MstProductType.Name }).ToListAsync();
            return response;
        }

        public async Task<List<DropdownDisplayExtendedId>> GetOrderProductTypes(int jobId, int supplierCompanyId)
        {
            List<DropdownDisplayExtendedId> response = new List<DropdownDisplayExtendedId>();
            response = await Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == supplierCompanyId && t.IsActive
                                                                    && t.FuelRequest.JobId == jobId && t.OrderXStatuses.Any(t1 => t1.IsActive && t1.StatusId == (int)OrderStatus.Open))
                                                        .Select(t => new DropdownDisplayExtendedId() { Id = t.Id, CodeId = t.FuelRequest.MstProduct.MstProductType.Id, Name = t.PoNumber + " - " + t.FuelRequest.MstProduct.MstProductType.Name }).ToListAsync();
            return response;
        }


        public async Task<List<ActivityViewModel>> GetActivityReportAsync(int companyId, int jobId, string fromDate, string toDate)
        {
            using (var tracer = new Tracer("JobDomain", "GetActivityReportAsync"))
            {
                var response = new List<ActivityViewModel>();
                var helperDomain = new HelperDomain(this);
                try
                {
                    var dropHistory = from invoice in Context.DataContext.Invoices.Include(t => t.Order).Include(t => t.InvoiceHeader)
                                      join asset in Context.DataContext.AssetDrops on invoice equals asset.Invoice into drops
                                      from drop in drops.DefaultIfEmpty()
                                      where invoice != null && invoice.Order != null
                                                && invoice.Order.BuyerCompanyId == companyId
                                                && invoice.Order.FuelRequest.Job.Id == jobId
                                                && invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                && invoice.IsActive && !invoice.IsBuyPriceInvoice
                                      select new { invoice, drop, invoice.Order.FuelRequest };

                    if (!string.IsNullOrEmpty(fromDate))
                    {
                        DateTimeOffset startDate = Convert.ToDateTime(fromDate).Date;
                        dropHistory = dropHistory.Where(t => t.invoice.DropEndDate >= startDate &&
                                                            (t.drop == null || t.drop.DropEndDate >= startDate));
                    }
                    if (!string.IsNullOrEmpty(toDate))
                    {
                        DateTimeOffset endDate = Convert.ToDateTime(toDate).Date.AddDays(1);
                        dropHistory = dropHistory.Where(t => t.invoice.DropEndDate < endDate &&
                                                            (t.drop == null || t.drop.DropEndDate < endDate));
                    }

                    foreach (var item in dropHistory)
                    {
                        var activity = new ActivityViewModel();

                        var vehicelId = item.drop != null ? item.drop.JobXAsset.Asset.AssetAdditionalDetail.VehicleId : string.Empty;
                        vehicelId = string.IsNullOrWhiteSpace(vehicelId) ? string.Empty : Resource.lblSingleHyphen + vehicelId;

                        activity.OrderId = item.invoice.OrderId ?? 0;
                        activity.InvoiceId = item.invoice.Id;
                        activity.PoNumber = item.invoice.Order != null ? (item.invoice.PoNumber) : Resource.lblHyphen;
                        activity.InvoiceNumber = item.invoice.DisplayInvoiceNumber;
                        activity.JobName = item.invoice.Order.FuelRequest.Job.Name;
                        activity.JobId = item.invoice.Order.FuelRequest.Job.Id;
                        activity.DisplayJobID = item.invoice.Order.FuelRequest.Job.DisplayJobID ?? Resource.lblHyphen;
                        activity.Date = item.drop != null ? item.drop.DropEndDate.ToString(Resource.constFormatDate) : item.invoice.DropEndDate.ToString(Resource.constFormatDate);
                        activity.StartTime = item.drop != null ? item.drop.DropStartDate.DateTime.ToShortTimeString() : item.invoice.DropStartDate.DateTime.ToShortTimeString();
                        activity.EndTime = item.drop != null ? item.drop.DropEndDate.DateTime.ToShortTimeString() : item.invoice.DropEndDate.DateTime.ToShortTimeString();
                        activity.Company = item.drop != null ? (item.drop.SubcontractorName ?? Resource.lblHyphen) : Resource.lblHyphen;
                        activity.AssetName = item.drop != null ? item.drop.JobXAsset.Asset.Name + vehicelId : Resource.lblHyphen;
                        activity.IsActive = item.drop != null && item.drop.JobXAsset.Asset.IsActive;
                        activity.AssetId = item.drop != null ? item.drop.JobXAsset.AssetId.ToString() : string.Empty;
                        activity.Service = item.invoice.InvoiceTypeId == (int)InvoiceType.DryRun ? Resource.lblDryRun : Resource.lblFuel;
                        activity.GallonsDelivered = item.invoice.InvoiceTypeId == (int)InvoiceType.DryRun ? string.Empty :
                                      (item.drop != null ? item.drop.DroppedGallons.GetCommaSeperatedValue() : item.invoice.DroppedGallons.GetCommaSeperatedValue());
                        activity.VehicleId = string.IsNullOrWhiteSpace(vehicelId) ? string.Empty : vehicelId.Remove(0, 1);
                        activity.FuelType = helperDomain.GetProductName(item.invoice.Order.FuelRequest.MstProduct);
                        activity.PricingFormat = GetPricingFormat(item.invoice, item.drop, null);
                        activity.Quantity = string.Empty;
                        if (item.invoice.InvoiceTypeId == (int)InvoiceType.DryRun)
                        {
                            activity.UnitCost = Resource.constSymbolCurrency + item.invoice.BasicAmount.ToString(ApplicationConstants.DecimalFormat4WOZero);
                            activity.Cost = item.invoice.BasicAmount;
                            activity.ResaleUnitCost = Resource.lblHyphen;
                            activity.ResaleCost = Resource.lblHyphen;
                            activity.ResaleContractNo = Resource.lblHyphen;
                            activity.AssetContractNo = Resource.lblHyphen;
                        }
                        else
                        {
                            if (!item.FuelRequest.FuelRequestPricingDetail.DisplayPrice.Contains(PricingType.Tier.ToString()))
                            {
                                var unitCost = GetUnitCost(item.invoice, item.drop, null);
                                activity.UnitCost = Resource.constSymbolCurrency + unitCost.ToString(ApplicationConstants.DecimalFormat4WOZero);
                                var quantity = GetQuantity(item.invoice, item.drop, null);
                                activity.Cost = quantity * unitCost;

                                var resaleUnitCost = GetResaleUnitCost(item.invoice);
                                activity.ResaleUnitCost = Resource.constSymbolCurrency + resaleUnitCost.ToString(ApplicationConstants.DecimalFormat4WOZero);
                                activity.ResaleCost = Resource.constSymbolCurrency + (resaleUnitCost * quantity).ToString(ApplicationConstants.DecimalFormat2);
                                activity.ResaleContractNo = !string.IsNullOrWhiteSpace(item.invoice.Order.FuelRequest.Job.ContractNumber) ? item.invoice.Order.FuelRequest.Job.ContractNumber : Resource.lblHyphen;
                                activity.AssetContractNo = item.drop != null && !string.IsNullOrWhiteSpace(item.drop.ContractNumber) ? item.drop.ContractNumber : Resource.lblHyphen;
                            }
                            else
                            {
                                activity.UnitCost = Resource.lblTier;
                                activity.Cost = item.invoice.BasicAmount;

                                activity.ResaleUnitCost = Resource.lblTier;
                                activity.ResaleCost = Resource.lblHyphen;
                                activity.ResaleContractNo = Resource.lblHyphen;
                                activity.AssetContractNo = Resource.lblHyphen;
                            }
                        }
                        response.Add(activity);
                    }

                    var dropsMade = Context.DataContext.Invoices.Include(t => t.FuelRequestFees).Include("FuelRequestFees.MstFeeType")
                                                                .Where(t => t.Order != null && t.Order.BuyerCompanyId == companyId
                                                                      && t.Order.FuelRequest.Job.Id == jobId
                                                                      && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                                      && t.FuelRequestFees.Any(t1 => t1.FeeSubTypeId != (int)FeeSubType.NoFee)
                                                                      && t.IsActive && !t.IsBuyPriceInvoice);

                    if (!string.IsNullOrEmpty(fromDate))
                    {
                        DateTimeOffset startDate = Convert.ToDateTime(fromDate).Date;
                        dropsMade = dropsMade.Where(t => t.DropEndDate >= startDate);
                    }
                    if (!string.IsNullOrEmpty(toDate))
                    {
                        DateTimeOffset endDate = Convert.ToDateTime(toDate).Date.AddDays(1);
                        dropsMade = dropsMade.Where(t => t.DropEndDate < endDate);
                    }
                    foreach (var drop in dropsMade)
                    {
                        var differentFees = drop.FuelRequestFees.Where(t => t.FeeSubTypeId != (int)FeeSubType.NoFee && t.FeeTypeId != (int)FeeType.DryRunFee);
                        foreach (var fee in differentFees)
                        {
                            if (fee.FeeSubTypeId == (int)FeeSubType.ByAssetCount)
                            {
                                var assets = drop.AssetDrops.GroupBy(t => t.JobXAssetId).Select(t => t.OrderByDescending(t1 => t1.UpdatedDate).First());
                                foreach (var asset in assets)
                                {
                                    var vehicelId = asset.JobXAsset.Asset.AssetAdditionalDetail.VehicleId;
                                    vehicelId = string.IsNullOrWhiteSpace(vehicelId) ? string.Empty : Resource.lblSingleHyphen + vehicelId;
                                    response.Add(new ActivityViewModel()
                                    {
                                        OrderId = drop.OrderId ?? 0,
                                        InvoiceId = drop.Id,
                                        PoNumber = drop.Order != null ? (drop.PoNumber) : Resource.lblHyphen,
                                        InvoiceNumber = drop.DisplayInvoiceNumber,
                                        JobName = drop.Order.FuelRequest.Job.Name,
                                        DisplayJobID = drop.Order.FuelRequest.Job.DisplayJobID ?? Resource.lblHyphen,
                                        Date = asset.DropEndDate.ToString(Resource.constFormatDate),
                                        StartTime = asset.DropStartDate.DateTime.ToShortTimeString(),
                                        EndTime = asset.DropEndDate.DateTime.ToShortTimeString(),
                                        Company = asset.SubcontractorName ?? Resource.lblHyphen,
                                        AssetName = $"{asset.JobXAsset.Asset.Name}{vehicelId}",
                                        AssetId = asset.JobXAsset.AssetId.ToString(),
                                        Service = GetService(fee),
                                        GallonsDelivered = string.Empty,
                                        Quantity = "1",
                                        PricingFormat = GetPricingFormat(drop, asset, fee),
                                        UnitCost = Resource.constSymbolCurrency + GetUnitCost(drop, asset, fee).ToString(ApplicationConstants.DecimalFormat4WOZero),
                                        Cost = GetCost(drop, asset, fee),
                                        ResaleUnitCost = Resource.lblHyphen,
                                        ResaleCost = Resource.lblHyphen,
                                        ResaleContractNo = Resource.lblHyphen,
                                        AssetContractNo = Resource.lblHyphen,
                                        VehicleId = string.IsNullOrWhiteSpace(vehicelId) ? string.Empty : vehicelId.Remove(0, 1),
                                        FuelType = drop.Order != null ? helperDomain.GetProductName(drop.Order.FuelRequest.MstProduct) : Resource.lblHyphen
                                    });
                                }
                            }
                            else
                            {
                                response.Add(new ActivityViewModel()
                                {
                                    OrderId = drop.OrderId ?? 0,
                                    InvoiceId = drop.Id,
                                    PoNumber = drop.Order != null ? (drop.PoNumber) : Resource.lblHyphen,
                                    InvoiceNumber = drop.DisplayInvoiceNumber,
                                    JobName = drop.Order.FuelRequest.Job.Name,
                                    DisplayJobID = drop.Order.FuelRequest.Job.DisplayJobID ?? Resource.lblHyphen,
                                    Date = drop.DropEndDate.ToString(Resource.constFormatDate),
                                    StartTime = drop.DropStartDate.DateTime.ToShortTimeString(),
                                    EndTime = drop.DropEndDate.DateTime.ToShortTimeString(),
                                    Company = Resource.lblHyphen,
                                    AssetName = Resource.lblHyphen,
                                    AssetId = string.Empty,
                                    Service = GetService(fee),
                                    GallonsDelivered = string.Empty,
                                    Quantity = fee.FeeSubTypeId == (int)FeeSubType.HourlyRate ? GetHourlyQuantity(drop, null, fee) : "1",
                                    PricingFormat = GetPricingFormat(drop, null, fee),
                                    UnitCost = Resource.constSymbolCurrency + GetUnitCost(drop, null, fee).ToString(ApplicationConstants.DecimalFormat4WOZero),
                                    Cost = GetCost(drop, null, fee),
                                    ResaleUnitCost = Resource.lblHyphen,
                                    ResaleContractNo = Resource.lblHyphen,
                                    AssetContractNo = Resource.lblHyphen,
                                    VehicleId = Resource.lblHyphen,
                                    FuelType = drop.Order != null ? helperDomain.GetProductName(drop.Order.FuelRequest.MstProduct) : Resource.lblHyphen
                                });
                            }
                        }
                    }
                    response = response.OrderByDescending(t => t.InvoiceId).ToList();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("JobDomain", "GetActivityReportAsync", ex.Message, ex);
                }

                return response;
            }
        }

        private string GetService(FuelFee fee)
        {
            string feeName = string.Empty;
            if (fee != null)
            {
                if (fee.FeeTypeId == (int)FeeType.AdditionalFee)
                {
                    if (string.IsNullOrWhiteSpace(fee.FeeDetails))
                        feeName = fee.MstFeeSubType.Name;
                    else
                        feeName = $"{fee.MstFeeSubType.Name} - {fee.FeeDetails}";
                }
                else
                {
                    feeName = fee.MstFeeType.Name;
                }
            }
            return feeName;
        }

        private string GetHourlyQuantity(Invoice invoice, AssetDrop assetDrop, FuelFee fee)
        {
            double time = 0;
            if (invoice.AssetDrops.Count > 0)
                time = invoice.AssetDrops.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop).Sum(t => t.DropEndDate.Subtract(t.DropStartDate).TotalMinutes);
            else
                time = invoice.DropEndDate.Subtract(invoice.DropStartDate).TotalMinutes;
            var hours = (int)(time / 60);
            var mins = (time - (hours * 60));
            return hours > 0 ? string.Format(Resource.constTimeFormat, hours, string.Format(Resource.constFormatDecimal2, mins))
                                                                : string.Format(Resource.constMinuteFormat, string.Format(Resource.constFormatDecimal2, mins));
        }

        private decimal GetQuantity(Invoice invoice, AssetDrop assetDrop, FuelFee fee)
        {
            if (fee != null)
            {
                if (fee.FeeTypeId == (int)FeeType.WetHoseFee || fee.FeeTypeId == (int)FeeType.OverWaterFee)
                {
                    if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                    {
                        double difference = 0;
                        if (invoice.AssetDrops.Count > 0)
                            difference = invoice.AssetDrops.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop).Sum(t => t.DropEndDate.Subtract(t.DropStartDate).TotalMinutes);
                        else
                            difference = invoice.DropEndDate.Subtract(invoice.DropStartDate).TotalMinutes;


                        return Convert.ToDecimal(difference) / 60;
                    }
                }
                return 1;
            }
            else if (assetDrop != null)
            {
                return assetDrop.DroppedGallons;
            }
            else
            {
                return invoice.DroppedGallons;
            }
        }

        private string GetPricingFormat(Invoice invoice, AssetDrop assetDrop, FuelFee fee)
        {
            if (invoice.InvoiceTypeId == (int)InvoiceType.DryRun)
            {
                return Resource.lblFlatFee;
            }
            if (fee != null)
            {
                if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                    return Resource.lblHourlyRate;
                else if (fee.FeeSubTypeId == (int)FeeSubType.ByAssetCount)
                    return Resource.lblPerAsset;
                else if (fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                    return Resource.lblByQuantity;
                else
                    return Resource.lblFlatFee;
            }
            else
            {
                var displayPriceString = invoice.Order.FuelRequest.FuelRequestPricingDetail?.DisplayPrice;
                int pricingType = !string.IsNullOrEmpty(displayPriceString) && displayPriceString.Contains(PricingType.Tier.ToString()) ? (int)PricingType.Tier :
                        (!string.IsNullOrEmpty(displayPriceString) && displayPriceString.Contains(PricingType.RackAverage.ToString()) ? (int)PricingType.RackAverage : (int)PricingType.PricePerGallon);
                if (pricingType == (int)PricingType.Tier)
                {
                    if (invoice.Order.FuelRequest.DifferentFuelPrices.Count > 0)
                    {
                        var quantity = assetDrop != null ? assetDrop.DroppedGallons : invoice.DroppedGallons;
                        var fuelFee = invoice.Order.FuelRequest.DifferentFuelPrices
                                                            .FirstOrDefault(t => (quantity > t.MinQuantity && quantity < t.MaxQuantity.Value)
                                                                                    || (t.MaxQuantity.Value == quantity || t.MinQuantity == quantity));
                        if (fuelFee != null)
                        {
                            pricingType = fuelFee.PricingTypeId;
                        }
                    }
                    else
                    {
                        return Resource.lblTier;
                    }
                }
                if (pricingType == (int)PricingType.PricePerGallon)
                {
                    return Resource.lbFlatPpg;
                }
                else if (pricingType == (int)PricingType.RackAverage)
                {
                    return Resource.lblRackAveragePpg;
                }
                else if (pricingType == (int)PricingType.RackLow)
                {
                    return Resource.lblRackLowPpg;
                }
                else if (pricingType == (int)PricingType.RackHigh)
                {
                    return Resource.lblRackHighPpg;
                }
                else if (pricingType == (int)PricingType.Suppliercost)
                {
                    return Resource.lblFuelCost;
                }
            }
            return Resource.lblHyphen;
        }

        private decimal GetUnitCost(Invoice invoice, AssetDrop assetDrop, FuelFee fee)
        {
            if (fee != null)
            {
                if (fee.FeeTypeId == (int)FeeType.DeliveryFee && fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                {
                    var quantity = assetDrop != null ? assetDrop.DroppedGallons : invoice.DroppedGallons;
                    var feeByQuantity = fee.FeeByQuantities.FirstOrDefault(t => (quantity > t.MinQuantity && quantity < t.MaxQuantity.Value)
                                                                                    || (t.MaxQuantity.Value == quantity || t.MinQuantity == quantity));
                    if (feeByQuantity != null)
                        return feeByQuantity.Fee;
                    else
                        return 0;
                }
                else
                {
                    return fee.Fee.GetPreciseValue(2);
                }
            }
            else
            {
                return invoice.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).First().PricePerGallon;
            }
        }

        private decimal GetCost(Invoice invoice, AssetDrop assetDrop, FuelFee fee)
        {
            return GetQuantity(invoice, assetDrop, fee) * GetUnitCost(invoice, assetDrop, fee);
        }

        private decimal GetResaleUnitCost(Invoice invoice)
        {
            HelperDomain helperDomain = new HelperDomain(this);

            var resale = invoice.Order.FuelRequest.Resales.FirstOrDefault();
            decimal resaleUnitCost = 0.0M;
            if (resale != null && invoice.Order != null)
            {
                if (resale.PricingTypeId == (int)PricingType.PricePerGallon)
                {
                    resaleUnitCost = resale.PricePerGallon;
                }
                else if (resale.PricingTypeId == (int)PricingType.RackAverage
                        || resale.PricingTypeId == (int)PricingType.RackLow
                        || resale.PricingTypeId == (int)PricingType.RackHigh)
                {
                    var rackAvgTypeId = resale.RackAvgTypeId ?? 0;
                    var rackPrice = invoice.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail.RackPrice).FirstOrDefault();
                    resaleUnitCost = helperDomain.GetCalculatedPricePerGallon(rackPrice, resale.PricePerGallon, rackAvgTypeId);
                }
            }
            return resaleUnitCost;
        }

        private async Task<StatusViewModel> CreateSubcontractorEntity(List<SubcontractorViewModel> subcontractors, Job job)
        {
            StatusViewModel statusViewModel = new StatusViewModel(Status.Success);
            var newSubcontractors = subcontractors.Select(t => new { Name = t.Name.ToLower(), Id = t.Id }).ToList();
            var removedSubcontractors = job.Subcontractors.Where(t => !newSubcontractors.Any(t1 => t1.Name == t.Name.ToLower() || t1.Id == t.Id)).Select(t => t.Id).ToList();
            var assetsWithRemovedSubcontractors = Context.DataContext.AssetSubcontractors.Where(t => t.IsActive && removedSubcontractors.Contains(t.SubcontractorId) && t.Asset.JobXAssets.Any(t1 => t1.RemovedBy == null && t1.JobId == job.Id)).Select(t => new { assetName = t.Asset.Name, subContrName = t.Subcontractor.Name }).ToList();
            if (!assetsWithRemovedSubcontractors.Any())
            {
                job.Subcontractors.Clear();
                var existingContractors = await Context.DataContext.Subcontractors.ToListAsync();
                foreach (var contractor in subcontractors)
                {
                    var newContractor = existingContractors.FirstOrDefault(t => t.Name.ToLower() == contractor.Name.ToLower() || t.Id == contractor.Id);
                    if (newContractor == null)
                    {
                        newContractor = contractor.ToEntity();
                    }
                    newContractor.Name = contractor.Name;
                    job.Subcontractors.Add(newContractor);
                }
            }
            else
            {
                statusViewModel.StatusCode = Status.Failed;
                statusViewModel.StatusMessage = string.Format(Resource.errMessageRemovingSubcontractor, new object[] { string.Join(" ,", assetsWithRemovedSubcontractors.Select(t => t.subContrName).Distinct().ToList()), string.Join(" ,", assetsWithRemovedSubcontractors.Select(t => t.assetName).Distinct().ToList()) });
            }
            return statusViewModel;
        }

        public List<ResaleGridViewModel> GetResaleGridData(int userId, int companyId, int jobId, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            List<ResaleGridViewModel> response = new List<ResaleGridViewModel>();
            try
            {
                HelperDomain helperDomain = new HelperDomain(this);
                var jobIds = Task.Run(() => helperDomain.GetJobIdsAsync(userId)).Result;
                if (jobIds != null)
                {
                    var allOrders = Context.DataContext.Orders
                                                                .Include(t => t.FuelRequest).Include(t => t.User).Include(t => t.FuelRequest.MstSupplierQualifications).Include(t => t.FuelRequest.MstProduct)
                                                                .Include(t => t.FuelRequest.Job).Include(t => t.FuelRequest.MstOrderType).Include(t => t.FuelRequest.Job.JobXAssets)
                                                                .Include("FuelRequest.Job.JobXAssets.Asset")
                                                                .Where(t => t.FuelRequest.Job.IsResaleEnabled
                                                                            && jobIds.Contains(t.FuelRequest.Job.Id)
                                                                            && t.FuelRequest.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest
                                                                            && (jobId == 0 || t.FuelRequest.Job.Id == jobId)
                                                                            && t.FuelRequest.Resales.Count > 0
                                                                            && t.BuyerCompanyId == companyId
                                                                            && t.ParentId == null
                                                                            && t.FuelRequest.Currency == currency
                                                                            && t.FuelRequest.Job.CountryId == countryId && t.IsActive)
                                                                 .OrderByDescending(t => t.Id);

                    foreach (var item in allOrders)
                    {
                        var order = new ResaleGridViewModel(Status.Success);
                        order.Id = item.Id;
                        order.OrderNumber = item.PoNumber;
                        order.JobId = item.FuelRequest.Job.Id;
                        var resaleCustomer = item.FuelRequest.ResaleCustomers.FirstOrDefault();
                        if (resaleCustomer != null && resaleCustomer.Name != null)
                            order.CustomerName = resaleCustomer.Name;
                        else
                            order.CustomerName = Resource.lblHyphen;
                        order.ContractNumber = item.FuelRequest.Job.ContractNumber;
                        order.Assets = item.FuelRequest.Job.JobXAssets.Count(t => t.RemovedBy == null && t.RemovedDate == null);
                        order.FuelType = helperDomain.GetProductName(item.FuelRequest.MstProduct);
                        order.Quantity = helperDomain.GetQuantityRequested(item.BrokeredMaxQuantity ?? item.FuelRequest.MaxQuantity);
                        order.RackPPGPaid = helperDomain.GetPricePerGallon(item.FuelRequest);
                        order.RackPPGSold = helperDomain.GetResalePrice(item.FuelRequest.Id);
                        order.CreatedBy = $"{item.FuelRequest.User.FirstName} {item.FuelRequest.User.LastName}";
                        if (item.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyCanceled || item.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyClosed)
                        {
                            var isOpenOrderExist = helperDomain.CheckForOpenBrokerOrder(item);
                            if (isOpenOrderExist)
                            {
                                order.Status = OrderStatus.Open.ToString();
                            }
                            else if (item.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyCanceled)
                            {
                                order.Status = OrderStatus.Canceled.ToString();
                            }
                            else
                            {
                                order.Status = OrderStatus.Closed.ToString();
                            }
                        }
                        else
                        {
                            var status = item.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).MstOrderStatus;
                            order.Status = status != null ? status.Name : "";
                        }

                        response.Add(order);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetResaleGridData", ex.Message, ex);
            }

            return response;
        }

        public List<DropdownDisplayItem> GetJobsForOfferPricing(int companyId, int offerPricingId, int userId, JobStatus jobStatus = JobStatus.Open)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var companyJobs = Context.DataContext.Jobs.Where(t => t.CompanyId == companyId && t.IsActive).AsEnumerable();
                if (companyJobs != null && companyJobs.Count() > 0)
                {
                    // offer location details
                    var offerPricing = Context.DataContext.OfferPricings.SingleOrDefault(t => t.Id == offerPricingId);
                    var offerPricingItems = offerPricing.OfferPricingItems;

                    var helperDomain = new HelperDomain(this);
                    var assignedJobs = Task.Run(() => helperDomain.GetJobIdsAsync(userId)).Result;
                    response = companyJobs
                                    .Where(t => t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)jobStatus
                                                && assignedJobs.Contains(t.Id)
                                                && (t.EndDate == null || t.EndDate.Value.Date >= DateTimeOffset.Now.ToTargetDateTimeOffset(t.TimeZoneName).Date)
                                                &&
                                                (
                                                    // job location exactly matches with offer location
                                                    offerPricingItems.Any(t1 => t1.StateId.HasValue && t1.StateId.Value == t.StateId && t1.CityId.HasValue && t1.MstCity.Name == t.City && t1.ZipCode != null && t1.ZipCode == t.ZipCode)
                                                    ||
                                                    // jobs state & city matches with offer location & offer zip is null
                                                    offerPricingItems.Any(t1 => t1.StateId.HasValue && t1.StateId.Value == t.StateId && t1.CityId.HasValue && t1.MstCity.Name == t.City && t1.ZipCode == null)
                                                    ||
                                                    // jobs state matches with offer location & offer city + zip is null
                                                    offerPricingItems.Any(t1 => t1.StateId.HasValue && t1.StateId.Value == t.StateId && !t1.CityId.HasValue && t1.ZipCode == null)
                                                //||
                                                //// offer exists for all states
                                                //offerPricingItems.All(t1 => !t1.StateId.HasValue && !t1.CityId.HasValue && t1.ZipCode == null && t1.IsActive)
                                                )
                                         )
                                    .OrderByDescending(t => t.Id)
                                    .Select(t => new DropdownDisplayItem
                                    {
                                        Id = t.Id,
                                        Name = t.Name
                                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobsForOfferPricing", ex.Message, ex);
            }
            return response;
        }

        private void SetCurrencyAndUoMToJobBudgetViewModel(JobStepsViewModel viewModel, Job job)
        {
            viewModel.JobBudget.UoM = job.UoM;
            viewModel.JobBudget.Currency = job.Currency;
            CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
            viewModel.JobBudget.ExchangeRate = currencyRateDomain.GetCurrencyRate(job.Currency, Currency.USD, DateTimeOffset.Now);
        }

        private void AddAuditLogTaxExempt(UserContext userContext, int jobId, bool from, bool to)
        {
            AuditLogger.AddAuditLog(userContext, new AuditLogViewModel()
            {
                CallSite = "JobDomain : UpdateJobStepsAsync",
                Message = $"{userContext.Name} modify the sales tax exempt from {from} to {to}",
                AuditEntityId = jobId,
                AuditEntityType = AuditEntityType.Job.ToString(),
                AuditEventType = AuditEventType.Update.ToString()
            });
        }

        public bool IsValidSiteId(int jobId, string siteId, int companyId)
        {
            var response = false;

            try
            {
                var job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id != jobId && t.DisplayJobID.ToLower() == siteId.ToLower() && t.IsActive && t.Company.Id == companyId);
                response = job == null;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "IsValidSiteId", ex.Message, ex);
            }

            return response;
        }

        public async Task<ApiNearestJobDetailViewModel> GetNearestJobsByCustomer(ApiNearestJobByCustomerModel viewModel)
        {
            var driverjobIds = new List<int>();
            if (viewModel.UserId > 0)
            {
                driverjobIds = await new FreightServiceDomain(this).GetJobsAssignedToDriver(viewModel.UserId);
            }

            ApiNearestJobDetailViewModel response = new ApiNearestJobDetailViewModel();
            var storedProcedureDomain = new StoredProcedureDomain(this);
            var jobsByCustomer = await storedProcedureDomain.GetNearestJobsByCustomer(viewModel.CustomerId, viewModel.SupplierId, viewModel.FuelTypeIds,
                            viewModel.Latitude, viewModel.Longitude, viewModel.Radius, driverjobIds);
            var jobDetailResonse = new List<ApiJobDetailViewModel>();

            var groupedJobList = jobsByCustomer.GroupBy(u => u.JobId)
                                        .Select(grp => grp.ToList())
                                        .ToList();
            foreach (var jobs in groupedJobList)
            {
                var item = jobs.FirstOrDefault();
                var jobDetail = new ApiJobDetailViewModel();
                jobDetail.JobId = item.JobId;
                jobDetail.JobName = item.JobName;
                jobDetail.UoM = item.UoM;
                jobDetail.Address = item.Address;
                jobDetail.City = item.City;
                jobDetail.StateCode = item.StateCode;
                jobDetail.ZipCode = item.ZipCode;
                jobDetail.CountryCode = item.CountryCode;
                jobDetail.Latitude = item.Latitude;
                jobDetail.Longitude = item.Longitude;
                jobDetail.Distance = item.Distance;
                jobDetail.AssetCount = item.AssetCount;

                var orders = new List<ApiOrderDetailsForJobViewModel>();
                foreach (var order in jobs)
                {
                    if (order.OrderId != 0)
                    {
                        var orderDetail = new ApiOrderDetailsForJobViewModel();
                        orderDetail.OrderId = order.OrderId;
                        orderDetail.PoNumber = order.PoNumber;
                        orderDetail.TerminalId = order.TerminalId;
                        orderDetail.FuelTypeId = order.FuelTypeId;
                        orderDetail.FuelType = order.FuelType;
                        orderDetail.ProductTypeId = order.ProductTypeId;
                        orderDetail.ProductType = order.ProductType;
                        orderDetail.IsPrePostDipEnabled = order.IsPrePostDipRequired;
                        orderDetail.IsDriverToUpdateBOL = order.IsDriverToUpdateBOL;
                        orderDetail.IsDropImageRequired = order.IsDropImageRequired;
                        orderDetail.IsBOLImageRequired = order.IsBOLImageRequired;
                        orderDetail.IsSignatureRequired = order.IsSignatureRequired;
                        orders.Add(orderDetail);
                    }
                }
                jobDetail.Orders = orders;

                jobDetailResonse.Add(jobDetail);

            }

            response.Jobs = jobDetailResonse;
            var jobIds = jobsByCustomer.Select(t => t.JobId).ToList();

            var allProductTypeMapping = Context.DataContext.ProductTypeCompatibilityMappings.Select(t => new DropdownDisplayExtendedId { Id = t.ProductTypeId, CodeId = t.MappedToProductTypeId }).ToList();
            var allAssets = Context.DataContext.Assets.Include(t => t.AssetAdditionalDetail).Include(t => t.Image).Include(t => t.JobXAssets).Include("JobXAssets.Job")
                                .Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.Company.Id == viewModel.CustomerId);

            allAssets = allAssets.Where(t => t.JobXAssets.Any(t1 => jobIds.Contains(t1.JobId) && t1.RemovedBy == null && t1.RemovedDate == null));

            var tankIds = allAssets.Select(t => t.Id).ToList();
            var tankAdditionalList = await new FreightServiceDomain(this).GetTankList(tankIds);

            List<ApiTankDetailViewModel> tankList = new List<ApiTankDetailViewModel>();
            foreach (var asset in allAssets)
            {
                ApiTankDetailViewModel tank = new ApiTankDetailViewModel();
                if (tankAdditionalList != null)
                {
                    var tankViewModel = tankAdditionalList.FirstOrDefault(t => t.AssetId == asset.Id);
                    if (tankViewModel != null)
                    {
                        //Set JobXAssetId and UoM
                        var jobXAssetId = 0;
                        var activeJobXAsset = asset.JobXAssets.FirstOrDefault(t => t.RemovedBy == null && t.RemovedDate == null);
                        if (activeJobXAsset != null)
                        {
                            jobXAssetId = activeJobXAsset.Id;
                            tank.UoM = activeJobXAsset.Job.UoM.ToString();
                        }

                        tank = tankViewModel.ToApiTankViewModel(tank.UoM);
                        tank.JobXAssetId = jobXAssetId;

                        //Set MappedToProductTypeId
                        tank.MappedToProductTypeId = allProductTypeMapping.Where(t => t.Id == tank.ProducTypeId).Select(t => t.CodeId).Distinct().ToList();
                        tankList.Add(tank);
                    }
                }
            }

            response.Tanks = tankList;

            return response;
        }

        public async Task<List<JobGridSupplierViewModel>> GetJobDetailsForSupplier(JobFilterViewModel filter, UserContext userContext)
        {
            var response = new List<JobGridSupplierViewModel>();
            var spDomain = new StoredProcedureDomain(this);

            try
            {
                var jobs = await spDomain.GetCustomersAndJobDetailsForSupplier(userContext.CompanyId, userContext.Id);
                jobs.ForEach(t => response.Add(t.ToJobGridSupplierViewModel()));
                if (jobs.Any())
                {
                    var apiResponse = await new FreightServiceDomain(this).GetJobSummary(userContext.CompanyId, jobs.Select(t => t.Id).ToList());
                    if (apiResponse != null && apiResponse.StatusCode == Status.Success)
                    {
                        foreach (var job in apiResponse.JobDetails)
                        {
                            var jobDetails = response.FirstOrDefault(t => t.Id == job.JobId);
                            jobDetails.RegionId = job.RegionId;
                            jobDetails.CarrierId = job.CarrierId;
                            jobDetails.RouteId = job.RouteId;
                            jobDetails.RegionName = job.RegionName;
                            jobDetails.CarrierName = job.CarrierName;
                            jobDetails.RouteName = job.RouteName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetCustomersAndJobDetailsForSupplier", ex.Message, ex);
            }
            return response.Distinct().ToList();
        }

        public async Task<StatusViewModel> UpdateJobType(int jobId, bool isRetail)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.Id == jobId);
                if (job != null)
                {
                    job.IsRetailJob = isRetail;
                    await Context.CommitAsync();
                    response.StatusMessage = string.Format(Resource.errMessageJobUpdateSuccess, job.Name);
                }
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageJobUpdateFailed;
                LogManager.Logger.WriteException("JobDomain", "UpdateJobType", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<BuyerLoadFilterModel>> GetBuyerLoadFilterData(int companyId, int userId, bool isShowCarrierManaged = false)
        {
            List<BuyerLoadFilterModel> response = new List<BuyerLoadFilterModel>();
            try
            {
                var jobIds = await new HelperDomain(this).GetJobIdsAsync(userId);
                if (isShowCarrierManaged)
                {
                    jobIds = await Context.DataContext.JobCarrierDetails.Where(t => jobIds.Contains(t.JobId) && t.IsActive).Select(t => t.JobId).ToListAsync();
                }
                var jobs = await Context.DataContext.Jobs.Where(t => jobIds.Contains(t.Id)).Select(t => new { t.Id, t.Name, t.StateId, StateName = t.MstState.Name }).ToListAsync();
                var orders = Context.DataContext.Orders
                               .Where(t => jobIds.Contains(t.FuelRequest.Job.Id)
                                   && t.OrderXStatuses.FirstOrDefault(t2 => t2.IsActive).StatusId == (int)OrderStatus.Open
                                   && t.BuyerCompanyId == companyId)
                               .Select(t => new { t.AcceptedCompanyId, CompanyName = t.Company.Name, t.FuelRequest.JobId })
                               .Distinct()
                               .ToList();

                var carrierDetails = Context.DataContext.JobCarrierDetails.Where(t => jobIds.Contains(t.JobId) && t.IsActive)
                    .Select(t => new { t.CarrierCompanyId, CompanyName = t.Company.Name, t.JobId }).Distinct().ToList();

                if (jobs != null && jobs.Any())
                {
                    foreach (var job in jobs)
                    {
                        BuyerLoadFilterModel filterModel = new BuyerLoadFilterModel();
                        filterModel.Id = job.Id;
                        filterModel.Name = job.Name;
                        filterModel.States.Add(new DropdownDisplayItem { Id = job.StateId, Name = job.StateName });
                        if (orders != null && orders.Any())
                        {
                            var suppliers = orders.Where(t => t.JobId == job.Id).Select(t => new DropdownDisplayItem { Id = t.AcceptedCompanyId, Name = t.CompanyName }).ToList();
                            filterModel.Suppliers.AddRange(suppliers);
                        }
                        if (carrierDetails != null && carrierDetails.Any())
                        {
                            var carriers = carrierDetails.Where(t => t.JobId == job.Id).Select(t => new DropdownDisplayItem { Id = t.CarrierCompanyId, Name = t.CompanyName }).ToList();
                            filterModel.Carriers.AddRange(carriers);
                        }
                        response.Add(filterModel);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetBuyerLoadFilterData", ex.Message, ex);
            }
            return response.OrderBy(t => t.Name).ToList();
        }

        //How to get carrier data for 
        public SupplierCarrierInfoDDL GetSuppliersCarriersForJob(List<int> jobIds, int companyId)
        {
            var response = new SupplierCarrierInfoDDL();
            try
            {
                if (jobIds != null && jobIds.Any())
                {
                    // t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Accepted
                    var orders = Context.DataContext.Orders
                                .Where(t => jobIds.Contains(t.FuelRequest.Job.Id)
                                 )
                                .Select(t => new { t.AcceptedCompanyId, CompanyName = t.Company.Name })
                                .Distinct()
                                .ToList();
                    if (orders != null && orders.Any())
                    {
                        foreach (var order in orders)
                        {
                            var supplierCompanyId = order.AcceptedCompanyId;
                            var supplierCompanyName = order.CompanyName;
                            response.supplierDetails.Add(new JobSupplierForBuyer { Id = supplierCompanyId, Name = supplierCompanyName });
                        }
                    }
                    var carrierDetails = GetCarriersForJob(jobIds);
                    if (carrierDetails != null && carrierDetails.Any())
                    {
                        response.carrierDetails = carrierDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetSuppliersCarriersForJob", ex.Message, ex);
            }
            return response;
        }

        public List<JobCarrierForBuyer> GetCarriersForJob(List<int> jobIds)
        {
            var response = new List<JobCarrierForBuyer>();
            var carrierDetails = Context.DataContext.JobCarrierDetails.Where(t => jobIds.Contains(t.JobId) && t.IsActive)
                                    .Select(t => new { t.CarrierCompanyId, CompanyName = t.Company.Name }).Distinct().ToList();
            if (carrierDetails != null && carrierDetails.Any())
            {
                foreach (var carrier in carrierDetails)
                {
                    var carrierCompanyId = carrier.CarrierCompanyId;
                    var carrierCompanyName = carrier.CompanyName;
                    response.Add(new JobCarrierForBuyer { Id = carrierCompanyId, Name = carrierCompanyName });
                }
            }
            return response;
        }

        public async Task<List<JobTankDetailsViewModel>> GetTankDetailsByCustomer(int companyId, CompanyType companyType, List<int> customerCompanies)
        {
            var response = new List<JobTankDetailsViewModel>();
            try
            {
                if (customerCompanies != null && customerCompanies.Any())
                {
                    var jobIds = new List<int>();
                    foreach (var customerCompanyId in customerCompanies)
                    {
                        if ((companyType == CompanyType.Carrier || companyType == CompanyType.BuyerSupplierAndCarrier || companyType == CompanyType.SupplierAndCarrier) && (customerCompanyId > 0))
                        {
                            var freightDomain = new FreightServiceDomain(this);
                            var cJobIds = await freightDomain.GetCarriersJobs(companyId, customerCompanyId);
                            if (cJobIds.Any())
                            {
                                jobIds.AddRange(cJobIds);
                            }
                        }

                        if (companyType == CompanyType.Supplier || companyType == CompanyType.BuyerSupplierAndCarrier || companyType == CompanyType.SupplierAndCarrier || companyType == CompanyType.BuyerAndSupplier)
                        {
                            var sJobIds = await Context.DataContext.Orders.Where(t =>
                                        t.AcceptedCompanyId == companyId && t.BuyerCompanyId == customerCompanyId && t.IsActive && t.FuelRequest.Job.JobXAssets
                                        .Any(x1 => x1.RemovedBy == null && x1.Asset.Type == (int)AssetType.Tank && x1.Asset.IsActive && x1.Job.IsActive && x1.Job.DisplayJobID != null && x1.Job.DisplayJobID.Trim() != ""))
                                        .Select(t1 => t1.FuelRequest.JobId).ToListAsync();

                            if (sJobIds.Any())
                            {
                                jobIds.AddRange(sJobIds);
                            }
                        }
                        if (jobIds.Any())
                        {
                            jobIds = jobIds.Distinct().ToList();
                            jobIds = Context.DataContext.Jobs.Where(t => jobIds.Contains(t.Id) && !string.IsNullOrEmpty(t.DisplayJobID) && t.IsActive &&
                                                                    t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open &&
                                                                    t.JobXAssets.Any(t2 => t2.Asset.Type == (int)AssetType.Tank && t2.RemovedBy == null && t2.RemovedDate == null))
                                                        .Select(t => t.Id)
                                                        .ToList();
                        }
                    }

                    if (jobIds != null && jobIds.Any())
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        response = await spDomain.GetTankDetailsByJobs(jobIds);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetTankDetailsByCustomer", ex.Message, ex);
            }
            return response;
        }

        public StatusViewModel SetCompanyOwnedLocation(int supplierCompanyId, int buyerCompanyId, int jobId, bool companyOwnedLocation, OrderCreationMethod creationMethod, UserContext userContext)
        {
            var response = new StatusViewModel();

            var buyerSupplierDetails = Context.DataContext.SupplierXBuyerDetails.FirstOrDefault(x => x.SupplierCompanyId == supplierCompanyId && x.BuyerCompanyId == buyerCompanyId && x.JobId == jobId && x.IsActive);
            if (buyerSupplierDetails != null)
            {
                buyerSupplierDetails.CompanyOwnedLocation = companyOwnedLocation;
                Context.DataContext.Entry(buyerSupplierDetails).State = EntityState.Modified;
                Context.Commit();
            }
            else
            {
                buyerSupplierDetails = new SupplierXBuyerDetails
                {
                    SupplierCompanyId = supplierCompanyId,
                    BuyerCompanyId = buyerCompanyId,
                    JobId = jobId,
                    CompanyOwnedLocation = companyOwnedLocation,
                    OrderCreationMethod = creationMethod,
                    LastModifiedDate = DateTimeOffset.Now,
                    CreatedBy = userContext.Id,
                    UpdatedBy = userContext.Id,
                    IsActive = true,
                };
                Context.DataContext.SupplierXBuyerDetails.Add(buyerSupplierDetails);
                Context.Commit();
            }
            response.StatusCode = Status.Success;
            response.StatusMessage = Resource.successCompanyOwned;
            return response;
        }

        public int GetJobIdByAsset(int assetId)
        {
            var jobId = 0;
            try
            {
                jobId = Context.DataContext.JobXAssets.Where(x => x.AssetId == assetId).Select(y => y.JobId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobIdByAsset", ex.Message, ex);
            }
            return jobId;
        }

        public async Task<LocationResponseModel> GetBuyerLocations(string token, string fromDate = "", string toDate = "", int userId = 0)
        {
            var response = new LocationResponseModel();
            try
            {
                var authDomain = new AuthenticationDomain(this);
                var apiUserContext = Task.Run(() => authDomain.GetUserContextFromTokenAsync(token)).Result;
                if (apiUserContext != null)
                {
                    if (apiUserContext.CompanyTypeId == CompanyType.Buyer || apiUserContext.CompanyTypeId == CompanyType.BuyerAndSupplier || apiUserContext.CompanyTypeId == CompanyType.BuyerSupplierAndCarrier)
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        var result = await spDomain.GetBuyerLocations(apiUserContext.CompanyId, userId, fromDate, toDate);

                        response.StatusCode = Status.Success;
                        if (result != null && result.Any())
                        {
                            response.ResponseData = result;
                            response.StatusMessage = Status.Success.ToString();
                        }
                        else
                        {
                            response.StatusMessage = Resource.lblNoDataAvailable;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMsgAccessDenied;
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMsgInvalidToken;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetBuyerLocations", ex.Message, ex);
                response.StatusCode = Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }

            return response;
        }

        public async Task<LocationResponseModel> GetCustomerLocations(string token, string fromDate = "", string toDate = "", int userId = 0)
        {
            var response = new LocationResponseModel();
            try
            {
                var authDomain = new AuthenticationDomain(this);
                var apiUserContext = Task.Run(() => authDomain.GetUserContextFromTokenAsync(token)).Result;
                if (apiUserContext != null)
                {
                    if (apiUserContext.CompanyTypeId != CompanyType.Buyer)
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        var result = await spDomain.GetCustomerLocations(apiUserContext.CompanyId, userId, fromDate, toDate);

                        response.StatusCode = Status.Success;
                        if (result != null && result.Any())
                        {
                            response.ResponseData = result;
                            response.StatusMessage = Status.Success.ToString();
                        }
                        else
                        {
                            response.StatusMessage = Resource.lblNoDataAvailable;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMsgAccessDenied;
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMsgInvalidToken;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetCustomerLocations", ex.Message, ex);
                response.StatusCode = Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }

            return response;
        }

        public async Task<string> GetOtherFuelTypes(int jobId)
        {
            var productName = string.Empty;
            if (jobId > 0)
            {
                var productNames = await Context.DataContext.Orders
                                                   .Where(t => t.FuelRequest.JobId == jobId &&
                                                   t.FuelRequest.MstProduct.ProductTypeId == (int)ProductTypes.NonStandardFuel)
                                                   .Select(t => t.FuelRequest.MstProduct.Name).Distinct().ToListAsync();

                productName = string.Join("</br>", productNames);
            }
            return productName;
        }
        public async Task<StatusViewModel> SaveInventoryTypeForJob(UserContext userContext, JobGridSupplierViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var jobId = Convert.ToInt32(viewModel.JobID);
                    var job = await Context.DataContext.Jobs.Where(t => t.Id == jobId
                                                                        && t.IsActive
                                                                  ).FirstOrDefaultAsync();

                    if (job != null)
                    {
                        job.InventoryDataCaptureType = viewModel.InventoryDataCaptureType;
                        Context.DataContext.Entry(job).State = EntityState.Modified;
                        await Context.CommitAsync();
                        transaction.Commit();
                        if (job.CreatedByCompanyId != userContext.CompanyId)
                        {
                            await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(
                                                                                           EventType.LocationAttributeChange,
                                                                                           job.Id,
                                                                                           userContext.Id
                                                                                           );
                        }
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.lblInventoryCaptureMethodSaved;
                    }
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.lblInventoryCaptureMethodFailed;
                    LogManager.Logger.WriteException("JobDomain", "SaveInventoryTypeForJob", ex.Message, ex);
                }
            }
            return response;
        }
        public StatusViewModel ValidateJobsCsvHeader(string csvText, string csvFilePath, UserContext userContext, CompanyType companyType)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var csvHeaderLine = Regex.Matches(csvText.Trim(), @"\A.*").Cast<Match>().FirstOrDefault();

                string[] lines = File.ReadAllLines(csvFilePath);
                string headerLine = lines.FirstOrDefault();
                if (csvHeaderLine.Value.Trim() != headerLine)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageBulkUploadHeaderMismatch;
                    return response;
                }
                if (lines.Where(t => !string.IsNullOrEmpty(t)).Count() <= 1)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageFileEmpty;
                }
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "ValidateJobsCsvHeader", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> UploadJobFileToBlob(UserContext userContext, Stream fileStream, string fileName, CompanyType companyType)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var azureBlob = new AzureBlobStorage();

                var filePath = await azureBlob.SaveBlobAsync(fileStream, GenerateJobFileName(userContext.Id), BlobContainerType.JobsBulkUpload.ToString().ToLower());
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    var queueDomain = new QueueMessageDomain();
                    var queueRequest = GetEnqueueMessageRequestViewModel(userContext, filePath, companyType);
                    var queueId = queueDomain.EnqeueMessage(queueRequest);

                    response.StatusCode = Status.Success;
                    response.StatusMessage = string.Format(Resource.successMessageOrderBulkWithRequestNo, string.Concat(Constants.JobBulkUpload, queueId.ToString("000")));
                }
                else
                    response.StatusMessage = Resource.errMessageErrorInAzureServer;
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageErrorInAzureServer;
                LogManager.Logger.WriteException("JobDomain", "UploadFileToBlob", ex.Message, ex);
            }
            return response;

        }

        private QueueMessageViewModel GetEnqueueMessageRequestViewModel(UserContext userContext, string blobStoragePath, CompanyType companyType)
        {
            var jsonViewModel = new JobsBulkUploadProcessorReqViewModel();
            jsonViewModel.FileLineNumberToStart = 0;
            jsonViewModel.UserId = userContext.Id;
            jsonViewModel.CompanyId = userContext.CompanyId;
            jsonViewModel.FileUploadPath = blobStoragePath;
            jsonViewModel.CompanyType = companyType;

            string json = JsonConvert.SerializeObject(jsonViewModel);

            return new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = QueueProcessType.JobsBulkUpload,
                JsonMessage = json
            };
        }
        public string ProcessJobsBulkUploadJsonMessage(JobsBulkUploadProcessorReqViewModel bulkRequestViewModel, List<string> errorInfo)
        {
            StringBuilder processMessage = new StringBuilder();
            try
            {
                if (!string.IsNullOrWhiteSpace(bulkRequestViewModel.FileUploadPath))
                {
                    var azureBlobe = new AzureBlobStorage();
                    var fileStream = azureBlobe.DownloadBlob(bulkRequestViewModel.FileUploadPath, BlobContainerType.JobsBulkUpload.ToString().ToLower());
                    if (fileStream != null)
                    {
                        ProcessJobBulkFile(bulkRequestViewModel, errorInfo, processMessage, fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!(ex is QueueMessageFatalException))
                    LogManager.Logger.WriteException("JobDomain", "ProcessJobsBulkUploadJsonMessage", ex.Message, ex);
                if (processMessage.Length == 0)
                {
                    processMessage.Append(Constants.ErrorWhileProcessingBulkOrder);
                    errorInfo.Add(processMessage.ToString());
                }
                throw new QueueMessageFatalException(errorInfo[0], errorInfo);
            }
            return processMessage.ToString();
        }

        public async Task<List<JobCoordinates>> GetJobCoordinates(List<int> jobIds)
        {
            var response = new List<JobCoordinates>();
            try
            {
                response = await Context.DataContext.Jobs.Where(t => jobIds.Contains(t.Id))
                                                            .Select(t => new JobCoordinates()
                                                            {
                                                                JobId = t.Id,
                                                                Latitude = t.Latitude,
                                                                Longitude = t.Longitude,
                                                            }).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobCoordinates", ex.Message, ex);
            }
            return response;

        }
        private void ProcessJobBulkFile(JobsBulkUploadProcessorReqViewModel bulkRequestViewModel, List<string> errorInfo, StringBuilder processMessage, Stream fileStream)
        {
            string csvText = new StreamReader(fileStream).ReadToEnd();
            if (!string.IsNullOrWhiteSpace(csvText))
            {
                var filteredCsvText = RemoveHeaderAndGuidelinesFromFile(csvText);
                var engine = new FileHelperEngine<JobsBulkUploadCsvViewModel>();
                var csvList = engine.ReadString(filteredCsvText).ToList();

                AuthenticationDomain authenticationDomain = new AuthenticationDomain(this);
                var context = authenticationDomain.GetUserContextAsync(bulkRequestViewModel.UserId, bulkRequestViewModel.CompanyType).Result;
                ProcessJobsList(errorInfo, csvList, context, processMessage, bulkRequestViewModel.CompanyType, fileStream);
            }
            else
            {
                processMessage.Append(Resource.errMessageFailedToReadFileContent);
            }
        }
        public string RemoveHeaderAndGuidelinesFromFile(string csvText)
        {
            csvText = Regex.Replace(csvText.Trim(), @"\A.*", string.Empty, RegexOptions.IgnoreCase);
            csvText = Regex.Replace(csvText.Trim(), @",\n", string.Empty, RegexOptions.IgnoreCase);
            csvText = csvText.TrimEnd(',');
            return csvText;
        }
        private void ProcessJobsList(List<string> errorInfo, List<JobsBulkUploadCsvViewModel> csvList, UserContext context, StringBuilder processMessage, CompanyType companyType, Stream fileStream)
        {
            var jobDomain = ContextFactory.Current.GetDomain<JobDomain>();

            StatusViewModel result = new StatusViewModel();
            var csvJobName = csvList.Select(t => t.Name).FirstOrDefault();
            if (csvList != null && csvList.Any())
            {
                int lineNumberOfCSV = 0;
                foreach (var item in csvList)
                {
                    processMessage.Clear();
                    lineNumberOfCSV++;

                    var response = ValidateExistingDuplicateJob(item, context, companyType, lineNumberOfCSV);
                    if (response.StatusCode == Status.Success)
                    {
                        response = ValidateJobsBulkUploadFile(item, context, companyType, lineNumberOfCSV);
                        if (response.StatusCode == Status.Success)
                        {
                            var viewModel = GetJobsViewModel(item, context, companyType);
                            if (viewModel.StatusCode == Status.Success)
                            {
                                result = jobDomain.SaveJobStepsAsync(context, viewModel).Result;

                                if (result.StatusCode == Status.Success)
                                    errorInfo.Add(SetSuccessProcessMessage(item.Name));
                                else
                                {
                                    SetFailedProcessMessage(processMessage, item.Name, result.StatusMessage);
                                    errorInfo.Add(processMessage.ToString());
                                }
                            }
                            else
                            {
                                SetFailedProcessMessage(processMessage, item.Name, viewModel.StatusMessage);
                                errorInfo.Add(processMessage.ToString());
                            }
                        }
                        else
                        {
                            SetFailedProcessMessage(processMessage, item.Name, response.StatusMessage);
                            errorInfo.Add(processMessage.ToString());
                        }
                    }
                    else
                    {
                        SetFailedProcessMessage(processMessage, item.Name, response.StatusMessage);
                        errorInfo.Add(processMessage.ToString());
                    }

                }
            }
            else
            {
                result.StatusCode = Status.Failed;
                result.StatusMessage = Resource.errMessageCanNotProcessZeroRecords;
                SetFailedProcessMessage(processMessage, csvJobName, result.StatusMessage);
                errorInfo.Add(processMessage.ToString());
                throw new QueueMessageFatalException(errorInfo[0], errorInfo);
            }
        }
        private bool CheckIfItsEmptyLine(JobsBulkUploadCsvViewModel job)
        {
            return string.IsNullOrWhiteSpace(job.Name) && string.IsNullOrWhiteSpace(job.StartDate) && string.IsNullOrWhiteSpace(job.Address) && string.IsNullOrWhiteSpace(job.ZipCode) && string.IsNullOrWhiteSpace(job.City) && string.IsNullOrWhiteSpace(job.State)
                && string.IsNullOrWhiteSpace(job.CountyName) && string.IsNullOrWhiteSpace(job.Country);
        }

        private bool IsRequiredFieldMissing(JobsBulkUploadCsvViewModel job)
        {
            if (string.IsNullOrWhiteSpace(job.Country))
            {
                return true;
            }
            else if (job.Country.Trim().ToLower() == Country.CAR.GetDisplayName().ToLower())
            {
                return (string.IsNullOrWhiteSpace(job.State)
                    && string.IsNullOrWhiteSpace(job.Latitude)
                    && string.IsNullOrWhiteSpace(job.Longitude)) || string.IsNullOrWhiteSpace(job.StartDate);
            }
            return string.IsNullOrWhiteSpace(job.Name) || string.IsNullOrWhiteSpace(job.StartDate) ||
                   string.IsNullOrWhiteSpace(job.Address) || string.IsNullOrWhiteSpace(job.ZipCode) ||
                   string.IsNullOrWhiteSpace(job.City) || string.IsNullOrWhiteSpace(job.State)
                   || string.IsNullOrWhiteSpace(job.CountyName);
        }

        public StatusViewModel ValidateExistingDuplicateJob(JobsBulkUploadCsvViewModel csvJobsList, UserContext userContext, CompanyType companyType, int lineNumberOfCSV)
        {
            var response = new StatusViewModel(Status.Success);
            var existingJob = Context.DataContext.Jobs.Where(t => t.IsActive && t.Name.ToLower() == csvJobsList.Name.ToLower()
                                                                   && t.Company.Id == userContext.CompanyId)
                                                                       .Select(t => t.Name)
                                                                       .FirstOrDefault();


            if (existingJob != null)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = string.Format(Resource.errMessageJobAlreadyExists, existingJob, lineNumberOfCSV);
                return response;
            }
            return response;
        }
        public StatusViewModel ValidateJobsBulkUploadFile(JobsBulkUploadCsvViewModel csvJob, UserContext userContext, CompanyType companyType, int lineNumberOfCSV)
        {
            var response = new StatusViewModel(Status.Success);
            try
            {
                if (csvJob != null)
                {
                    var allJobs = Context.DataContext.Jobs.Where(t => t.IsActive && t.CompanyId == userContext.CompanyId).Select(t => t.Name.ToLower().Trim()).ToList();
                    var allStates = Context.DataContext.MstStates.Where(t => t.IsActive).Select(t => t.Name.ToLower().Trim()).ToList();
                    var allCountries = Context.DataContext.MstCountries.Where(t => t.IsActive).Select(t => new { t.Code, t.Currency, t.DefaultUoM }).ToList();
                    var allCities = Context.DataContext.MstCities.Where(t => t.IsActive).Select(t => t.Name);
                    var zipcodeRegEx = ApplicationConstants.ZipValidationRegex;
                    var isCarribeanCountry = (csvJob.Country.Trim().ToLower() == Country.CAR.ToString().ToLower()) ? true : false;
                    //foreach (var jobs in csvJobsList)
                    //{
                    if (!CheckIfItsEmptyLine(csvJob))
                    {
                        //Required field validation
                        if (IsRequiredFieldMissing(csvJob))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageBulkUploadRequiredFieldsAreMissing, lineNumberOfCSV);
                            return response;
                        }
                        if (allJobs.Contains(csvJob.Name.ToLower().Trim()))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageAlreadyJobNameExists, csvJob.Name, lineNumberOfCSV);
                            return response;
                        }
                        if (!DateTime.TryParse(csvJob.StartDate, out DateTime startDateTime))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format("</br>{0} invalid format at line {1}", csvJob.StartDate, lineNumberOfCSV);
                            if (startDateTime > DateTime.Now.Date)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format("</br>{0} can not be greater than current date at line {1}", startDateTime, lineNumberOfCSV);
                            }
                        }
                        if (!string.IsNullOrEmpty(csvJob.EndDate))
                        {
                            if (!DateTime.TryParse(csvJob.EndDate, out DateTime endDateTime))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format("</br>{0} invalid format at line {1}", csvJob.EndDate, lineNumberOfCSV);
                                if (endDateTime < DateTime.Now.Date)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format("</br>{0} can not be Less than current date at line {1}", endDateTime, lineNumberOfCSV);
                                }
                            }
                        }
                        // cities are not added for carribean countries hence skipping this validation
                        if (!isCarribeanCountry)
                        {
                            if (!allCities.Contains(csvJob.City.ToLower().Trim()))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageCityInvalid, csvJob.City, lineNumberOfCSV);
                                return response;
                            }
                        }

                        if (!allStates.Contains(csvJob.State.ToLower().Trim()))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageStateInvalid, csvJob.State, lineNumberOfCSV);
                            return response;
                        }
                        if (!allCountries.Select(t => t.Code.ToLower()).Contains(csvJob.Country.ToLower().Trim()))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageCountryInvalid, csvJob.Country, lineNumberOfCSV);
                            return response;
                        }
                        if (!string.IsNullOrEmpty(csvJob.Currency))
                        {
                            if (!(string.Equals("usd", csvJob.Currency.ToLower().Trim()) || string.Equals("cad", csvJob.Currency.ToLower().Trim())))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageCurrencyInvalid, csvJob.Currency, lineNumberOfCSV);
                                return response;
                            }
                        }
                        if (!string.IsNullOrEmpty(csvJob.UoM))
                        {
                            if (!(string.Equals("gallons", csvJob.UoM.ToLower().Trim()) || string.Equals("litres", csvJob.UoM.ToLower().Trim())))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageUoMInvalid, csvJob.UoM, lineNumberOfCSV);
                                return response;
                            }
                        }
                        if (!isCarribeanCountry)
                        {
                            if (!Regex.Match(csvJob.ZipCode.ToLower().Trim(), zipcodeRegEx).Success)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageZipCodeInvalid, csvJob.ZipCode, lineNumberOfCSV);
                                return response;
                            }

                        }

                        if (!string.IsNullOrEmpty(csvJob.IsGeocodeUsed) && (string.Equals("yes", csvJob.IsGeocodeUsed.ToLower().Trim())))
                        {
                            if (string.IsNullOrEmpty(csvJob.Latitude) && string.IsNullOrEmpty(csvJob.Longitude))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errInvalidLatLong, lineNumberOfCSV);
                                return response;
                            }

                        }
                        if (!string.IsNullOrEmpty(csvJob.TimeZoneName))
                        {
                            if (!(string.Equals("pacific standard time", csvJob.TimeZoneName.ToLower().Trim()) || string.Equals("mountain standard time", csvJob.TimeZoneName.ToLower().Trim())
                                || string.Equals("central standard time", csvJob.TimeZoneName.ToLower().Trim()) || string.Equals("eastern standard time", csvJob.TimeZoneName.ToLower().Trim())))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageTimeZoneNameInvalid, csvJob.TimeZoneName, lineNumberOfCSV);
                                return response;
                            }
                        }
                        if (!string.IsNullOrEmpty(csvJob.InventoryDataCaptureType))
                        {
                            if (!(string.Equals("not specified", csvJob.InventoryDataCaptureType.ToLower()) || string.Equals("connected", csvJob.InventoryDataCaptureType.ToLower().Trim()) || string.Equals("manual", csvJob.InventoryDataCaptureType.ToLower().Trim())
                            || string.Equals("call-in", csvJob.InventoryDataCaptureType.ToLower().Trim()) || string.Equals("mixed", csvJob.InventoryDataCaptureType.ToLower().Trim())))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageInventoryDataTypeInvalid, csvJob.InventoryDataCaptureType, lineNumberOfCSV);
                                return response;
                            }
                        }
                        if (!string.IsNullOrEmpty(csvJob.IsProFormaPoEnabled))
                        {
                            if (!(string.Equals("yes", csvJob.IsProFormaPoEnabled.ToLower().Trim()) || string.Equals("no", csvJob.IsProFormaPoEnabled.ToLower().Trim())))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageProformaEnabledInvalid, lineNumberOfCSV);
                                return response;
                            }
                        }
                        if (!string.IsNullOrEmpty(csvJob.IsRetailJob))
                        {
                            if (!(string.Equals("yes", csvJob.IsRetailJob.ToLower().Trim()) || string.Equals("no", csvJob.IsRetailJob.ToLower().Trim())))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageRetailLocationInvalid, lineNumberOfCSV);
                                return response;
                            }
                        }
                        if (!string.IsNullOrEmpty(csvJob.IsAutoCreateDREnable))
                        {
                            if (!(string.Equals("yes", csvJob.IsAutoCreateDREnable.ToLower().Trim()) || string.Equals("no", csvJob.IsAutoCreateDREnable.ToLower().Trim())))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageAutoCreateDRInvalid, lineNumberOfCSV);
                                return response;
                            }
                        }
                        if (!string.IsNullOrEmpty(csvJob.IsTaxExempted))
                        {
                            if (!(string.Equals("yes", csvJob.IsTaxExempted.ToLower().Trim()) || string.Equals("no", csvJob.IsTaxExempted.ToLower().Trim())))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageSalesTaxExemptInvalid, lineNumberOfCSV);
                                return response;
                            }
                        }
                        if (!string.IsNullOrEmpty(csvJob.IsAssetTracked))
                        {
                            if (!(string.Equals("yes", csvJob.IsAssetTracked.ToLower().Trim()) || string.Equals("no", csvJob.IsAssetTracked.ToLower().Trim())))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageAssetTrackedInvalid, lineNumberOfCSV);
                                return response;
                            }
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageCanNotProcessZeroRecords;
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageCanNotProcessZeroRecords;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "ValidateJobsBulkUploadFile", ex.Message, ex);
            }
            return response;
        }
        private JobStepsViewModel GetJobsViewModel(JobsBulkUploadCsvViewModel csvJob, UserContext userContext, CompanyType companyType)
        {
            var response = new JobStepsViewModel(Status.Success);
            if (csvJob != null)
            {
                if (!string.IsNullOrWhiteSpace(csvJob.Name))
                {
                    response.Job.Name = csvJob.Name;
                    response.Job.JobID = csvJob.JobID;
                    response.Job.StartDate = DateTimeOffset.Parse(csvJob.StartDate);
                    response.Job.EndDate = !string.IsNullOrEmpty(csvJob.EndDate) ? Convert.ToDateTime(csvJob.EndDate) : (DateTime?)null;
                    response.Job.Address = csvJob.Address;
                    response.Job.ZipCode = csvJob.ZipCode;
                    response.Job.City = csvJob.City;
                    var getStateId = Context.DataContext.MstStates.Where(t => t.Name.ToLower().Trim() == csvJob.State.ToLower().Trim()).Select(t => t.Id).FirstOrDefault();
                    response.Job.State.Id = getStateId;
                    response.Job.State.Name = csvJob.State;
                    var getCountryId = Context.DataContext.MstCountries.Where(t => t.Code.ToLower().Trim() == csvJob.Country.ToLower().Trim()).Select(t => t.Id).FirstOrDefault();
                    response.Job.Country.Id = getCountryId;
                    response.Job.Country.Name = csvJob.Country;
                    response.Job.CountyName = csvJob.CountyName;
                    // set default currency  as USD and uom as Gallons for CAR country 
                    if ((response.Job.Country.Id == (int)Country.USA) || (response.Job.Country.Id == (int)Country.CAR))
                    {
                        response.Job.Country.Currency = !string.IsNullOrEmpty(csvJob.Currency) ? (Currency)Enum.Parse(typeof(Currency), csvJob.Currency) : (Currency)Enum.Parse(typeof(Currency), "USD");
                    }
                    else
                    {
                        response.Job.Country.Currency = !string.IsNullOrEmpty(csvJob.Currency) ? (Currency)Enum.Parse(typeof(Currency), csvJob.Currency) : (Currency)Enum.Parse(typeof(Currency), "CAD");
                    }
                    if ((response.Job.Country.Id == (int)Country.USA) || (response.Job.Country.Id == (int)Country.CAR))
                    {
                        response.Job.Country.UoM = !string.IsNullOrEmpty(csvJob.UoM) ? (UoM)Enum.Parse(typeof(UoM), csvJob.UoM) : (UoM)Enum.Parse(typeof(UoM), "Gallons");
                    }
                    else
                    {
                        response.Job.Country.UoM = !string.IsNullOrEmpty(csvJob.UoM) ? (UoM)Enum.Parse(typeof(UoM), csvJob.UoM) : (UoM)Enum.Parse(typeof(UoM), "Litres");
                    }
                    response.Job.IsGeocodeUsed = csvJob.IsGeocodeUsed.ToLower().Trim() == "yes" ? true : false;
                    response.Job.Latitude = !string.IsNullOrEmpty(csvJob.Latitude) ? Convert.ToDecimal(csvJob.Latitude) : 0;
                    response.Job.Longitude = !string.IsNullOrEmpty(csvJob.Longitude) ? Convert.ToDecimal(csvJob.Longitude) : 0;
                    response.Job.TimeZoneName = csvJob.TimeZoneName;

                    var inventoryDataType = csvJob.InventoryDataCaptureType == "Not Specified" ? csvJob.InventoryDataCaptureType.Replace(" ", "") : csvJob.InventoryDataCaptureType == "Call-In" ? csvJob.InventoryDataCaptureType.Replace("-", "") : csvJob.InventoryDataCaptureType;
                    response.Job.InventoryDataCaptureType = !string.IsNullOrEmpty(inventoryDataType) ? (InventoryDataCaptureType)Enum.Parse(typeof(InventoryDataCaptureType), inventoryDataType) : (InventoryDataCaptureType)Enum.Parse(typeof(InventoryDataCaptureType), "NotSpecified");
                    response.Job.IsProFormaPoEnabled = csvJob.IsProFormaPoEnabled.ToLower().Trim() == "yes" ? true : false;
                    response.Job.IsRetailJob = csvJob.IsRetailJob.ToLower().Trim() == "yes" ? true : false;
                    response.Job.IsAutoCreateDREnable = csvJob.IsAutoCreateDREnable.ToLower().Trim() == "yes" ? true : false;
                    response.JobBudget.IsTaxExempted = csvJob.IsTaxExempted.ToLower().Trim() == "yes" ? true : false;
                    response.Job.IsAssetTracked = csvJob.IsAssetTracked.ToLower().Trim() == "yes" ? true : false;
                    response.Job.StatusId = (int)JobStatus.Open;
                    response.Job.CreatedDate = DateTimeOffset.Now;
                    response.Job.ReopenDate = DateTimeOffset.Now;
                    response.Job.IsBackdatedJob = true;
                    response.Job.IsVarious = false;
                    response.Job.LocationType = JobLocationTypes.Location;
                    response.Job.LocationManagedType = LocationManagedType.NotSpecified;
                    response.UserId = userContext.Id;
                    response.CompanyId = userContext.CompanyId;
                }
            }
            return response;
        }
        private static string SetSuccessProcessMessage(string jobName)
        {
            StringBuilder processMessage = new StringBuilder();
            processMessage.Append("<p class='color-green'>").Append("<b>Info: </b>")
                        .Append($"Location Name: {jobName} <br><b>processed successfully</b></p><br>");
            return processMessage.ToString();
        }
        private static void SetFailedProcessMessage(StringBuilder processMessage, string jobName, string message)
        {
            processMessage.Append("<p class='color-maroon'>").Append("<b>Info: </b>")
                        .Append($"Location Name: {jobName} <br><b>Processing failed Reason:</b> {message}</p><br>");
        }

        #region Location from API
        public async Task<ApiResponseViewModel> CreateLocationFromAPI(string token, TPDLocationCreateModel locationCreateModel)
        {
            ApiResponseViewModel response = new ApiResponseViewModel();
            try
            {
                //get userdetails from token
                var authDomain = new AuthenticationDomain(this);

                var apiUserContext = await authDomain.GetUserContextFromTokenAsync(token);
                if (apiUserContext != null)
                {
                    ValidateRequiredParameters(response, locationCreateModel);
                    if (!response.Messages.Any())
                    {
                        List<JobStepsViewModel> jobStepList = new List<JobStepsViewModel>();
                        int buyerCompanyId = GetBuyerCompanyIdFromExref(locationCreateModel);
                        if (buyerCompanyId > 0)
                        {
                            var buyerCompanyUserId = Context.DataContext.Companies.Where(t => t.Id == buyerCompanyId).Select(t => t.CreatedBy).SingleOrDefault();
                            foreach (var item in locationCreateModel.Locations)
                            {
                                SetJobIdFromExrefId(item, response);

                                ValidateFormatandSetValues(item, buyerCompanyUserId, response);

                                ValidateReqParametersForLocation(response, item);

                                if (!response.Messages.Any())
                                {
                                    bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidJobName(item.JobId, item.LocationName, buyerCompanyId);
                                    if (!result)
                                    {
                                        response.Messages.Add(new ApiCodeMessages() { Message = string.Format(Resource.valMessageAlreadyExist, item.LocationName) });
                                        response.Status = Status.Failed;
                                    }

                                    if (IsValidLocationXRefId(item.JobId, item.LocationXRefID, buyerCompanyId, apiUserContext.CompanyId))
                                    {
                                        response.Messages.Add(new ApiCodeMessages() { Message = string.Format(Resource.valMessageAlreadyExist, item.LocationXRefID) });
                                        response.Status = Status.Failed;
                                    }

                                    jobStepList.Add(item.ToJobStepViewModel(apiUserContext.CompanyId));
                                }
                            }

                            if (!response.Messages.Any())
                            {
                                AuthenticationDomain authenticationDomain = new AuthenticationDomain(this);
                                UserContext buyerUserContext = null;

                                foreach (var location in jobStepList)
                                {
                                    if (buyerUserContext == null)
                                        buyerUserContext = await authenticationDomain.GetUserContextAsync(location.Job.CreatedBy, CompanyType.Buyer);

                                    if (location.Job.Id > 0)
                                    {
                                        //update location
                                        var jobResponse = await UpdateJobStepsAsync(buyerUserContext, location);
                                        response.Messages.Add(new ApiCodeMessages()
                                        { EntityId = "TFLO" + location.Job.Id.ToString(), Message = jobResponse.StatusMessage, Code = jobResponse.StatusCode == Status.Success ? Constants.ApiCodeRS01 : Constants.ApiCodeRS02 });
                                        response.Status = jobResponse.StatusCode == Status.Success ? Status.Success : Status.Failed;
                                    }
                                    else
                                    {
                                        //add location
                                        var jobResponse = await SaveJobStepsAsync(buyerUserContext, location);
                                        response.Messages.Add(new ApiCodeMessages()
                                        { EntityId = "TFLO" + jobResponse.JobId.ToString(), Message = jobResponse.StatusMessage, Code = jobResponse.StatusCode == Status.Success ? Constants.ApiCodeRS01 : Constants.ApiCodeRS02 });
                                        response.Status = jobResponse.StatusCode == Status.Success ? Status.Success : Status.Failed;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(locationCreateModel.TFXCompanyId))
                            {
                                response.Messages.Add(new ApiCodeMessages()
                                {
                                    Code = Constants.ApiCodeRQ02,
                                    Message = Resource.errMsgInvalidTFxCompanyId
                                });
                                response.Status = Status.Failed;
                            }
                            if (!string.IsNullOrWhiteSpace(locationCreateModel.ExternalRefID))
                            {
                                response.Messages.Add(new ApiCodeMessages()
                                {
                                    Code = Constants.ApiCodeRQ02,
                                    Message = Resource.errMsgInvalidExternalRefId
                                });
                                response.Status = Status.Failed;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "CreateLocationFromAPI", ex.Message, ex);
                response.Status = Status.Failed;
            }
            return response;
        }

        public bool IsValidLocationXRefId(int jobId, string locationXref, int buyerCompanyId, int supplierCompany)
        {
            return Context.DataContext.Jobs.Any(t => t.Id != jobId && t.IsActive && t.ExternalRefID.ToLower() == locationXref.ToLower()
                                            && t.CompanyId == buyerCompanyId && t.CreatedByCompanyId == supplierCompany);
        }

        private void ValidateFormatandSetValues(TPDLocationViewModel location, int buyerCompanyUserId, ApiResponseViewModel response)
        {
            if (location != null)
            {
                location.BuyerCompanyUserId = buyerCompanyUserId;
                CultureInfo enUS = new CultureInfo("en-US");

                if (!string.IsNullOrWhiteSpace(location.LocationStartDate))
                {
                    if (DateTimeOffset.TryParseExact(location.LocationStartDate, "MMddyyyy", enUS, DateTimeStyles.None, out DateTimeOffset startDate))
                    {
                        location.StartDate = startDate;
                    }
                    else
                    {
                        response.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRQ02,
                            Message = string.Format(Resource.errMsgParameterFormatIsInvalid, nameof(location.LocationStartDate))
                        });
                        response.Status = Status.Failed;
                    }
                }
                else if (location.JobId == 0)
                    location.StartDate = DateTimeOffset.Now;

                if (!string.IsNullOrWhiteSpace(location.LocationEndDate))
                {
                    if (DateTimeOffset.TryParseExact(location.LocationEndDate, "MMddyyyy", enUS, DateTimeStyles.None, out DateTimeOffset endDate))
                    {
                        location.EndDate = endDate;
                    }
                    else
                    {
                        response.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRQ02,
                            Message = string.Format(Resource.errMsgParameterFormatIsInvalid, nameof(location.LocationEndDate))
                        });
                        response.Status = Status.Failed;
                    }
                }

                if (!string.IsNullOrWhiteSpace(location.LocationAddressState))
                {
                    var state = Context.DataContext.MstStates.Where(t => t.Code.ToLower() == location.LocationAddressState.ToLower() || t.Name.ToLower() == location.LocationAddressState.ToLower()).Select(t => t.Id).FirstOrDefault();
                    if (state > 0)
                    {
                        location.StateId = state;
                    }
                    else
                    {
                        response.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRQ02,
                            Message = string.Format(Resource.errMsgParameterFormatIsInvalid, nameof(location.LocationAddressState))
                        });
                        response.Status = Status.Failed;
                    }
                }
            }
        }

        private void ValidateRequiredParameters(ApiResponseViewModel response, TPDLocationCreateModel locationCreateModel)
        {
            if (string.IsNullOrWhiteSpace(locationCreateModel.TFXCompanyId) && string.IsNullOrWhiteSpace(locationCreateModel.ExternalRefID))
            {
                response.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRQ02,
                    Message = string.Format(Resource.errMessageRequiredParameter, $"{nameof(locationCreateModel.TFXCompanyId)} or {nameof(locationCreateModel.ExternalRefID)}")
                });
                response.Status = Status.Failed;
            }
        }

        private void ValidateReqParametersForLocation(ApiResponseViewModel response, TPDLocationViewModel location)
        {
            if (string.IsNullOrWhiteSpace(location.LocationXRefID))
            {
                response.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRQ02,
                    Message = string.Format(Resource.errMessageRequiredParameter, $"{nameof(location.LocationXRefID)}")
                });
                response.Status = Status.Failed;
            }

            if (string.IsNullOrWhiteSpace(location.LocationName))
            {
                response.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRQ02,
                    Message = string.Format(Resource.errMessageRequiredParameter, $"{nameof(location.LocationName)}")
                });
                response.Status = Status.Failed;
            }

            if (string.IsNullOrWhiteSpace(location.LocationBillToAddressName) && !string.IsNullOrWhiteSpace(location.LocationBillToAddressLine1))
            {
                response.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRQ02,
                    Message = string.Format(Resource.errMessageRequiredParameter, $"{nameof(location.LocationBillToAddressName)}")
                });
                response.Status = Status.Failed;
            }

            if (location.LocationAddressLat == 0 && location.LocationAddressLong == 0)
            {
                if (string.IsNullOrWhiteSpace(location.LocationAddressLine1))
                {
                    response.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = string.Format(Resource.errMessageRequiredParameter, $"{nameof(location.LocationAddressLine1)}")
                    });
                    response.Status = Status.Failed;
                }
                if (string.IsNullOrWhiteSpace(location.LocationAddressCity))
                {
                    response.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = string.Format(Resource.errMessageRequiredParameter, $"{nameof(location.LocationAddressCity)}")
                    });
                    response.Status = Status.Failed;
                }
                if (string.IsNullOrWhiteSpace(location.LocationAddressState))
                {
                    response.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = string.Format(Resource.errMessageRequiredParameter, $"{nameof(location.LocationAddressState)}")
                    });
                    response.Status = Status.Failed;
                }
                if (string.IsNullOrWhiteSpace(location.LocationAddressZip))
                {
                    response.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = string.Format(Resource.errMessageRequiredParameter, $"{nameof(location.LocationAddressZip)}")
                    });
                    response.Status = Status.Failed;
                }
                //if (string.IsNullOrWhiteSpace(location.LocationAddressCounty))
                //{  
                //response.Messages.Add(new ApiCodeMessages()
                //{
                //    Code = Constants.ApiCodeRQ02,
                //    Message = string.Format(Resource.errMessageRequiredParameter, $"{nameof(location.LocationAddressCounty)}")
                //});
                //response.Status = Status.Failed;
                //}
                if (string.IsNullOrWhiteSpace(location.LocationAddressCountry))
                {
                    response.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = string.Format(Resource.errMessageRequiredParameter, $"{nameof(location.LocationAddressCountry)}")
                    });
                    response.Status = Status.Failed;
                }
            }
        }

        private int GetBuyerCompanyIdFromExref(TPDLocationCreateModel locationCreateModel)
        {
            int result = 0;
            if (!string.IsNullOrWhiteSpace(locationCreateModel.ExternalRefID))
            {
                result = Context.DataContext.CompanyXCreators
                                    .Where(t => t.IsActive && t.ExternalRefId.ToLower() == locationCreateModel.ExternalRefID.ToLower())
                                    .Select(t => t.CompanyId)
                                    .FirstOrDefault();
            }
            if (!string.IsNullOrWhiteSpace(locationCreateModel.TFXCompanyId))
            {
                var tfxcompanyId = locationCreateModel.TFXCompanyId.Replace("TFCU", "").TrimStart('0'); // test tfcu in lower case

                if (int.TryParse(tfxcompanyId, out int buyerCompanyId))
                {
                    result = buyerCompanyId;
                }
            }

            return result;
        }

        private void SetJobIdFromExrefId(TPDLocationViewModel location, ApiResponseViewModel response)
        {
            if (string.IsNullOrWhiteSpace(location.ThirdPartyLocationID))
            {
                location.ThirdPartyLocationID = location.LocationXRefID != null ? location.LocationXRefID : location.LocationName;
            }
            if (!string.IsNullOrWhiteSpace(location.TfxLocationID))
            {
                var tfxJobId = location.TfxLocationID.Replace("TFLO", "").TrimStart('0'); // test tfcu in lower case

                if (int.TryParse(tfxJobId, out int buyerJobId))
                {
                    var existingJob = Context.DataContext.Jobs.Where(t => t.Id == buyerJobId)
                        .Select(t => new
                        {
                            t.Id,
                            t.Name,
                            t.CreatedBy,
                            t.DisplayJobID,
                            t.Address,
                            t.City,
                            t.ZipCode,
                            t.SiteInstructions,
                            t.ExternalRefID,
                            t.StartDate,
                            t.Latitude,
                            t.Longitude,
                            t.StateId,
                            t.CountryId,
                            StateCode = t.MstState.Code,
                            CountryCode = t.MstCountry.Code,
                            t.CountyName,
                            t.TimeZoneName,
                            t.BillingAddressId,
                            t.BillingAddress
                        })
                        .SingleOrDefault();
                    if (existingJob == null)
                    {
                        response.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRQ02,
                            Message = string.Format(Resource.errMsgParameterIsRequired, $"{nameof(location.TfxLocationID)}")
                        });
                        response.Status = Status.Failed;
                    }
                    else
                    {
                        SetExisitngJobDetailsToVm(location, existingJob);
                        if (existingJob.BillingAddress != null)
                        {
                            SetExistingBillingAddressDetails(location, existingJob.BillingAddress);
                        }
                    }
                }
                else
                {
                    response.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = string.Format(Resource.errMsgParameterIsRequired, $"{nameof(location.TfxLocationID)}, value must be with TFLO")
                    });
                    response.Status = Status.Failed;
                }
            }
            else if (!string.IsNullOrWhiteSpace(location.LocationXRefID))
            {
                try
                {
                    var existingJob = Context.DataContext.Jobs.Where(t => t.ExternalRefID == location.LocationXRefID)
                        .Select(t => new
                        {
                            t.Id,
                            t.Name,
                            t.CreatedBy,
                            t.DisplayJobID,
                            t.Address,
                            t.City,
                            t.ZipCode,
                            t.SiteInstructions,
                            t.ExternalRefID,
                            t.StartDate,
                            t.Latitude,
                            t.Longitude,
                            t.StateId,
                            t.CountryId,
                            StateCode = t.MstState.Code,
                            CountryCode = t.MstCountry.Code,
                            t.CountyName,
                            t.TimeZoneName,
                            t.BillingAddressId,
                            t.BillingAddress
                        })
                        .SingleOrDefault();
                    if (existingJob != null)
                    {
                        SetExisitngJobDetailsToVm(location, existingJob);
                        if (existingJob.BillingAddress != null)
                        {
                            SetExistingBillingAddressDetails(location, existingJob.BillingAddress);
                        }
                    }
                }
                catch (InvalidOperationException ex)
                {
                    response.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = string.Format(Resource.errMsgParameterIsRequired, $"{nameof(location.LocationXRefID)}. Multiple entries found for this {location.LocationXRefID}")
                    });
                    LogManager.Logger.WriteException("JobDomain", "SetJobIdFromExrefId", ex.Message, ex);
                }
                catch (Exception ex)
                {
                    response.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = string.Format(Resource.errMsgParameterIsRequired, $"{nameof(location.LocationXRefID)}")
                    });
                    LogManager.Logger.WriteException("JobDomain", "SetJobIdFromExrefId", ex.Message, ex);
                }
            }
        }

        private void SetExistingBillingAddressDetails(TPDLocationViewModel location, BillingAddress billingAddress)
        {
            location.LocationBillToAddressName = location.LocationBillToAddressName.IfNullSetNewValue(billingAddress.Name);
            location.LocationBillToAddressLine1 = location.LocationBillToAddressLine1.IfNullSetNewValue(billingAddress.Address);
            location.LocationBillToAddressLine2 = location.LocationBillToAddressLine2.IfNullSetNewValue(billingAddress.AddressLine2);
            location.LocationBillToAddressLine3 = location.LocationBillToAddressLine3.IfNullSetNewValue(billingAddress.AddressLine3);
            location.LocationBillToAddressCity = location.LocationBillToAddressCity.IfNullSetNewValue(billingAddress.City);
            location.LocationBillToAddressZip = location.LocationBillToAddressZip.IfNullSetNewValue(billingAddress.ZipCode);
            location.LocationBillToAddressCountry = location.LocationBillToAddressCountry.IfNullSetNewValue(billingAddress.CountryName);
            location.LocationBillToAddressState = location.LocationBillToAddressState.IfNullSetNewValue(billingAddress.StateName);
        }

        private void SetExisitngJobDetailsToVm(TPDLocationViewModel location, dynamic existingJob)
        {
            location.JobId = existingJob.Id;
            if (string.IsNullOrWhiteSpace(location.LocationName))
                location.LocationName = existingJob.Name;

            location.BuyerCompanyUserId = existingJob.CreatedBy;
            location.BuyerCompanyUserId = existingJob.CreatedBy;
            if (string.IsNullOrWhiteSpace(location.ThirdPartyLocationID))
                location.ThirdPartyLocationID = existingJob.DisplayJobID;

            if (string.IsNullOrWhiteSpace(location.LocationAddressLine1))
                location.LocationAddressLine1 = existingJob.Address;

            if (string.IsNullOrWhiteSpace(location.LocationAddressCity))
                location.LocationAddressCity = existingJob.City;

            if (string.IsNullOrWhiteSpace(location.LocationAddressZip))
                location.LocationAddressZip = existingJob.ZipCode;

            if (location.LocationAddressLat == 0)
                location.LocationAddressLat = existingJob.Latitude;

            if (location.LocationAddressLong == 0)
                location.LocationAddressLong = existingJob.Longitude;

            if (string.IsNullOrWhiteSpace(location.SiteInstruction))
                location.SiteInstruction = existingJob.SiteInstructions;

            if (string.IsNullOrWhiteSpace(location.LocationXRefID))
                location.LocationXRefID = existingJob.ExternalRefID;

            if (string.IsNullOrWhiteSpace(location.LocationAddressState))
                location.LocationAddressState = existingJob.StateCode;

            location.StateId = existingJob.StateId;
            location.CountryId = existingJob.CountryId;
            location.TimeZoneName = existingJob.TimeZoneName;
            location.BillingAddressId = existingJob.BillingAddressId ?? 0;

            if (string.IsNullOrWhiteSpace(location.LocationAddressCountry))
                location.LocationAddressCountry = existingJob.CountryCode;

            if (string.IsNullOrWhiteSpace(location.LocationAddressCounty))
                location.LocationAddressCounty = existingJob.CountyName;

            location.StartDate = existingJob.StartDate;
        }
        #endregion
        public async Task<List<DropdownDisplayExtendedProperty>> GetJobsForBuyer(int companyId, bool IsMarine)
        {
            var jobs = new List<DropdownDisplayExtendedProperty>();
            try
            {
                jobs = await Context.DataContext.Jobs.Where(t => t.CompanyId == companyId && t.IsActive &&
                                                 t.IsMarine == IsMarine)
                                                 .Select(t => new DropdownDisplayExtendedProperty()
                                                 {
                                                     Id = t.Id,
                                                     Name = t.Name,
                                                     IsTrue = t.IsMarine
                                                 }).GroupBy(t => t.Id).Select(t => t.FirstOrDefault()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobsForBuyer", ex.Message, ex);
            }
            return jobs;
        }
        public async Task<List<OnsiteJobUserViewModel>> GetJobOnsiteContactDetails(int jobId)
        {
            var onsiteuserInfoList = new List<OnsiteJobUserViewModel>();
            try
            {
                var jobInfo = await Context.DataContext.Jobs.FirstOrDefaultAsync(x => x.Id == jobId && x.IsActive);
                if (jobInfo != null && jobInfo.Users1 != null)
                {
                    var onsiteUserInfoDetails = jobInfo.Users1.Where(x => x.OnboardedTypeId == (int)OnboardedType.ThirdPartyOrderOnboarded).ToList();
                    if (onsiteUserInfoDetails.Any())
                    {
                        onsiteUserInfoDetails.ForEach(x =>
                        {
                            var onsiteuserInfo = new OnsiteJobUserViewModel();
                            onsiteuserInfo.UserId = x.Id;
                            onsiteuserInfo.JobId = jobInfo.Id;
                            onsiteuserInfo.FirstName = x.FirstName;
                            onsiteuserInfo.LastName = x.LastName;
                            onsiteuserInfo.Email = x.Email;
                            onsiteuserInfo.ContactNumber = x.PhoneNumber;
                            onsiteuserInfo.OnboardedTypeId = x.OnboardedTypeId;
                            onsiteuserInfoList.Add(onsiteuserInfo);
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobOnsiteContactDetails", ex.Message, ex);
            }
            return onsiteuserInfoList;
        }
        public async Task<StatusViewModel> UpdateOnsiteJobContactDetails(OnsiteJobUserViewModel person)
        {

            string json = JsonConvert.SerializeObject(person);
            LogManager.Logger.WriteDebug("JobDomain", "UpdateOnsiteJobContactDetails", json);
            var response = new StatusViewModel();
            try
            {
                var userInfo = await Context.DataContext.Users.FirstOrDefaultAsync(x => x.Id == person.UserId && x.IsActive && !x.IsDeleted);
                if (userInfo != null)
                {
                    userInfo.FirstName = person.FirstName;
                    userInfo.LastName = person.LastName;
                    userInfo.PhoneNumber = person.ContactNumber;
                    userInfo.Email = person.Email;
                    userInfo.UserName = person.Email;
                }
                await Context.CommitAsync();
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.valsuccessOnsiteContactDetails;
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = ex.Message;
                LogManager.Logger.WriteException("JobDomain", "UpdateOnsiteJobContactDetails", ex.Message, ex);
            }
            return response;
        }
    }
}
