using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Queue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.Domain
{
    public class QueueMessageDomain : BaseDomain
    {
        #region Constructor
        
        public QueueMessageDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }
        public QueueMessageDomain(BaseDomain domain) : base(domain)
        {
        }
        
        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Enqueues a NEW message in the queue
        /// </summary>
        /// <param name="message"></param>
        public int EnqeueMessage(QueueMessageViewModel message)
        {
            var queueMessage = new QueueMessage
            {
                JsonMessage = message.JsonMessage,
                CreatedBy = message.CreatedBy,
                Status = (int)QueueMessageStatus.Pending,
                ProcessTypeId = (int)message.QueueProcessType,
                RetryCount = 0,
                CreatedDate = DateTimeOffset.Now,
                UpdatedDate = DateTimeOffset.Now,
                UpdatedBy = message.CreatedBy
            };
            Context.DataContext.QueueMessages.Add(queueMessage);
            Context.Commit();

            return queueMessage.Id;
        }

        /// <summary>
        /// Returns the oldest messages with pending status
        /// Get oldest pending process which is not PDIAPIDeliveryDetails type
        /// and not in excluded queuey process type.
        /// </summary>
        /// <returns>QueueMessageViewModel</returns>
        public QueueMessageViewModel DequeueMessage()
        {
            var message = Context.DataContext.QueueMessages.
                    Where   (x => (x.Status == (int)QueueMessageStatus.Pending)
                                &&  (x.ProcessTypeId != (int)QueueProcessType.PDIAPIDeliveryDetails)
                                &&  (!HelperDomain.ExcludedQueueJobProcessType.Contains(x.ProcessTypeId))
                            ).OrderBy(x => x.UpdatedDate).FirstOrDefault();

            return GetQueueMessageViewModelAndUpdateMessageToInProgress(message);
        }

        /// <summary>
        /// Returns the oldest PDIApiMessage with pending status
        /// </summary>
        /// <returns>QueueMessageViewModel</returns>
        public QueueMessageViewModel DequeueMessagePDI()
        {
            var message = Context.DataContext.QueueMessages.
                    Where   (x => (x.Status == (int)QueueMessageStatus.Pending)
                                &&  (x.ProcessTypeId == (int)QueueProcessType.PDIAPIDeliveryDetails)
                            ).OrderBy(x => x.UpdatedDate).FirstOrDefault();
            return GetQueueMessageViewModelAndUpdateMessageToInProgress(message);
        }

        /// <summary>
        /// Returns the oldest excludedQueueMessageProcessType with pending status
        /// </summary>
        /// <param name="qmsToProcess">Keeping it open for different status processing support based on OnHold in future</param>
        /// <returns></returns>
        public QueueMessageViewModel DequeueMessageFileUpload(QueueMessageStatus qmsToProcess )
        {
            //Get oldest pending process which is not PDIAPIDeliveryDetails type and are in the excluded list from 
            //prioritized main sequenced queue process
            var message = Context.DataContext.QueueMessages.
                    Where   (x => (x.Id== 558129)
                                &&  (HelperDomain.ExcludedQueueJobProcessType.Contains(x.ProcessTypeId))
                            ).OrderBy(x => x.UpdatedDate).FirstOrDefault();
            return GetQueueMessageViewModelAndUpdateMessageToInProgress(message);
        }

        /// <summary>
        /// Sets the message as processed and will not dequeued again for processing
        /// </summary>
        /// <param name="messageId"></param>
        public void SetMessageProcessed(int messageId, int updatedBy, List<string> errorInfo)
        {
            var message = Context.DataContext.QueueMessages.Where(x => x.Id == messageId).FirstOrDefault();
            if (message != null)
            {
                message.Status = (int)QueueMessageStatus.Completed;
                message.UpdatedDate = DateTimeOffset.Now;
                message.UpdatedBy = updatedBy;
                errorInfo = errorInfo.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
                errorInfo.ForEach(x =>
                message.QueueResults.Add(new QueueResult
                {
                    RetryCount = message.RetryCount,
                    RetryMessage = x
                }));
                Context.Commit();
            }

        }

        public void SetInProcessQueueMessageStatus(int messageId, int updatedBy, string errorInfo)
        {
            var message = Context.DataContext.QueueMessages.Where(x => x.Id == messageId).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(errorInfo) && message != null)
            {
                message.Status = (int)QueueMessageStatus.InProcess;
                message.UpdatedDate = DateTimeOffset.Now;
                message.UpdatedBy = updatedBy;
                message.QueueResults.Add(new QueueResult { RetryCount = 0, RetryMessage = errorInfo });
                Context.Commit();
            }
        }

        public void SetInprocessedMessageProcessed(int messageId)
        {
            var message = Context.DataContext.QueueMessages.Where(x => x.Id == messageId).FirstOrDefault();
            int totalBulkOrdersCount = Context.DataContext.BulkOrderDetails.Where(t => t.FileID == messageId).Count();
            var totalFailedBulkOrderCount = Context.DataContext.BulkOrderDetails.Where(t => t.FileID == messageId && t.Status == (int)BulkOrderDetailsStatus.Failed)?.Count();

            if (message != null)
            {
                if(totalBulkOrdersCount == totalFailedBulkOrderCount)//All Msgs failed set status to failed
                {
                    message.Status = (int)QueueMessageStatus.Failed;
                    message.UpdatedDate = DateTimeOffset.Now;
                    Context.Commit();
                }
                else if(totalFailedBulkOrderCount > 0) // Some Msgs Failed set status to Partially Completed with Errors
                {
                    message.Status = (int)QueueMessageStatus.PartiallyCompletedWithErrors;
                    message.UpdatedDate = DateTimeOffset.Now;
                    Context.Commit();
                }
                else //All Msgs Completed Successfully
                {
                    message.Status = (int)QueueMessageStatus.Completed;
                    message.UpdatedDate = DateTimeOffset.Now;
                    Context.Commit();
                }
                
            }
        }

        /// <summary>
        /// Increases the retry count and sets the status as Pending again
        /// </summary>
        /// <param name="messageId"></param>
        public void SetMessageToBeProcessedAgain(QueueMessageViewModel queueMessageViewModel, int updatedBy, List<string> lastErrorInfos)
        {
            if (lastErrorInfos == null)
                throw new ArgumentNullException("lastErrorInfos");
            var message = Context.DataContext.QueueMessages.Where(x => x.Id == queueMessageViewModel.MessageId).FirstOrDefault();
            if (message != null)
            {
                message.Status = (int)QueueMessageStatus.Pending;
                message.RetryCount += 1;
                message.JsonMessage = queueMessageViewModel.JsonMessage;
                message.UpdatedDate = DateTimeOffset.Now;
                message.UpdatedBy = updatedBy;
                lastErrorInfos = lastErrorInfos.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
                lastErrorInfos.ForEach(x =>
                message.QueueResults.Add(new QueueResult
                {
                    RetryCount = message.RetryCount,
                    RetryMessage = x
                }));
                Context.Commit();
            }
        }

        /// <summary>
        /// Will set the message as failed and will not be dequeued again
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="updatedBy"></param>
        public void SetMessageAsFailed(int messageId, int updatedBy, List<string> failureErrorInfos)
        {
            var message = Context.DataContext.QueueMessages.
                Where(x => x.Id == messageId).FirstOrDefault();
            if (message != null)
            {
                message.Status = (int)QueueMessageStatus.Failed;
                message.RetryCount += 1;
                message.UpdatedDate = DateTimeOffset.Now;
                message.UpdatedBy = updatedBy;
                failureErrorInfos = failureErrorInfos.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
                if (failureErrorInfos != null)
                    failureErrorInfos.ForEach(x =>
                       message.QueueResults.Add(new QueueResult
                       {
                           RetryCount = message.RetryCount,
                           RetryMessage = x
                       })
                      );
                Context.Commit();
            }
        }
        
        /// <summary>
        /// Returns all the requests for a user for a processType
        /// </summary>
        /// <param name="requestedUserId"></param>
        /// <returns></returns>
        public List<QueueMessageViewModel> GetMessagesRequestedByUser(int requestedUserId, List<QueueProcessType> processTypes)
        {
            var messages = Context.DataContext.QueueMessages.
                Where(x => x.CreatedBy == requestedUserId && processTypes.Any(t => (int)t == x.ProcessTypeId)).OrderByDescending(x => x.Id).ToList();

            var queueMessages = messages.Select(message =>
              new QueueMessageViewModel
              {
                  QueueProcessType = (QueueProcessType)message.ProcessTypeId,
                  MessageId = message.Id,
                  Status = (QueueMessageStatus)message.Status,
                  RetryCount = message.RetryCount,
                  JsonMessage = message.JsonMessage,
                  TimeRequested = message.CreatedDate,
                  UpdatedDate = message.UpdatedDate,
                  CreatedBy = message.CreatedBy,
                  UpdatedBy = message.UpdatedBy,
                  MessageResults = message.QueueResults.OrderBy(x => x.Id).Select(x =>
                    new QueueMessageResult
                    {
                        ErrorInfo = x.RetryMessage,
                        Id = x.Id,
                        QueueMessageId = message.Id,
                        RetryCount = x.RetryCount
                    }).ToList()
              });

            return queueMessages.ToList();
        }

        #endregion  Public Methods

        #region Private Methods

        private QueueMessageViewModel GetQueueMessageViewModelAndUpdateMessageToInProgress(QueueMessage message)
        {
            if (message == null)
                return null;
            else
            {
                //Step1: Convert the queuemessage current state to queuemessage viewmodel
                var queueMessageVm = GetQueueMessageViewModelFromQueueMessage(message);

                //Step2: Post queue message conversion to queuemessageviewmodel
                //update the status of queuemessage to in Process and commit straight away.
                UpdateMessageStatusToInProgress(message);

                return queueMessageVm;
            }
        }

        private QueueMessageViewModel GetQueueMessageViewModelFromQueueMessage(QueueMessage message)
        {
            return new QueueMessageViewModel
            {
                QueueProcessType = (QueueProcessType)message.ProcessTypeId,
                MessageId = message.Id,
                Status = (QueueMessageStatus)message.Status,
                RetryCount = message.RetryCount,
                JsonMessage = message.JsonMessage,
                TimeRequested = message.CreatedDate,
                UpdatedDate = message.UpdatedDate,
                CreatedBy = message.CreatedBy,
                UpdatedBy = message.UpdatedBy
            };
        }

        private void UpdateMessageStatusToInProgress(QueueMessage message)
        {
            message.UpdatedDate = DateTimeOffset.Now;
            message.Status = (int)QueueMessageStatus.InProcess;
            Context.Commit();
        }

        #endregion
    }
}
