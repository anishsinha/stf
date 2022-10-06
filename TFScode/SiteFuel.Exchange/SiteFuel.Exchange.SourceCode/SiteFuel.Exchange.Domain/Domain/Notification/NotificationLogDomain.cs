using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.DataAccess;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Core.StringResources;
using System.Data.Entity;

namespace SiteFuel.Exchange.Domain
{
    public class NotificationLogDomain : BaseDomain
    {
        public NotificationLogDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public NotificationLogDomain(BaseDomain domain) : base(domain)
        {
        }

        public void SaveEmailNotificationLog(NotificationViewModel viewModel, bool sendResponse)
        {
            try
            {
                var isEmailBodyLog = AppSettings.IsEmailBodyLog;

                var notificationLog = new NotificationLog();
                notificationLog.Subject = viewModel.Subject;
                notificationLog.Subject = CropString(viewModel.Subject, 100);
                if (isEmailBodyLog)
                {
                    notificationLog.Body = viewModel.BodyText;
                    notificationLog.Body = CropString(notificationLog.Body, 500);
                }

                notificationLog.To = string.Join(";", viewModel.To);
                notificationLog.To = CropString(notificationLog.To, 200);

                notificationLog.CC = string.Join(";", viewModel.CC);
                notificationLog.CC = CropString(notificationLog.CC, 200);

                notificationLog.BCC = string.Join(";", viewModel.BCC);
                notificationLog.BCC = CropString(notificationLog.BCC, 200);

                notificationLog.NotificationId = viewModel.NotificationId;
                notificationLog.LogDateTime = DateTimeOffset.Now;
                notificationLog.NotificationType = viewModel.SendNotificationType;
                if (sendResponse)
                    notificationLog.Status = (int)NotificationStatus.Success;
                else
                    notificationLog.Status = (int)NotificationStatus.Failed;
                Context.DataContext.NotificationLogs.Add(notificationLog);
                Context.Commit();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationLogDomain", "SaveEmailNotificationLog", ex.Message, ex);
            }
        }

        public void SaveSmsNotificationLog(NotificationViewModel viewModel, bool sendResponse, bool isSmsTextLog = false)
        {
            try
            {
                var isEmailBodyLog = AppSettings.IsEmailBodyLog;

                var notificationLog = new NotificationLog();
                notificationLog.Subject = viewModel.Subject;
                notificationLog.Subject = CropString(viewModel.Subject, 100);
                if (isEmailBodyLog || isSmsTextLog)
                {
                    notificationLog.Body = viewModel.SmsText;
                    notificationLog.Body = CropString(notificationLog.Body, 500);
                }

                notificationLog.To = string.Join(";", viewModel.To);
                notificationLog.To = CropString(notificationLog.To, 200);

                notificationLog.CC = string.Join(";", viewModel.CC);
                notificationLog.CC = CropString(notificationLog.CC, 200);

                notificationLog.BCC = string.Join(";", viewModel.BCC);
                notificationLog.BCC = CropString(notificationLog.BCC, 200);

                notificationLog.NotificationId = viewModel.NotificationId;
                notificationLog.LogDateTime = DateTimeOffset.Now;
                notificationLog.NotificationType = viewModel.SendNotificationType;
                if (sendResponse)
                    notificationLog.Status = (int)NotificationStatus.Success;
                else
                    notificationLog.Status = (int)NotificationStatus.Failed;
                Context.DataContext.NotificationLogs.Add(notificationLog);
                Context.Commit();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationLogDomain", "SaveSmsNotificationLog", ex.Message, ex);
            }
        }

        public async Task<StatusViewModel> UpdateSmsStatus(SmsResponseViewModel viewModel)
        {
            var response = new StatusViewModel(Status.Success);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var smsStatus = viewModel.SmsStatus.ToLower();
                    if (smsStatus == "failed" || smsStatus == "delivered" || smsStatus == "undelivered")
                    {
                        var isphoneNumberConfirmed = true;
                        var notificationLog = await Context.DataContext.NotificationLogs.SingleOrDefaultAsync(t => t.CC == viewModel.SmsSid &&
                                (t.NotificationType == (int)NotificationType.Sms || t.NotificationType == (int)NotificationType.EmailAndSms));
                        if (notificationLog != null)
                        {
                            if (smsStatus == "failed")
                            {
                                notificationLog.Status = (int)NotificationStatus.Failed;
                                isphoneNumberConfirmed = false;
                            }
                            else if (smsStatus == "delivered")
                            {
                                notificationLog.Status = (int)NotificationStatus.Delivered;
                                isphoneNumberConfirmed = true;
                            }
                            else if (smsStatus == "undelivered")
                            {
                                notificationLog.Status = (int)NotificationStatus.Undelivered;
                                isphoneNumberConfirmed = false;
                            }

                            Context.DataContext.Entry(notificationLog).State = EntityState.Modified;
                            await Context.CommitAsync();
                        }

                        if (viewModel.To.Length >= 10)
                        {
                            var sendTo = viewModel.To.Substring(viewModel.To.Length - 10);
                            var users = Context.DataContext.Users.Where(t => t.PhoneNumber.
                                        Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "") == sendTo).ToList();
                            foreach (var item in users)
                            {
                                item.IsPhoneNumberConfirmed = isphoneNumberConfirmed;
                                Context.DataContext.Entry(item).State = EntityState.Modified;
                                await Context.CommitAsync();
                            }
                        }
                        transaction.Commit();
                    }
                    response.StatusCode = Status.Success;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("NotificationLogDomain", "UpdateSmsStatus", ex.Message, ex);
                    response.StatusMessage = Resource.errMessageUpdateFailed;
                }
            }
            return response;
        }

        private string CropString(string input, int cropLength)
        {
            if (!string.IsNullOrEmpty(input))
                return input.Substring(0, input.Length > cropLength ? cropLength : input.Length);
            return input;
        }
    }
}
