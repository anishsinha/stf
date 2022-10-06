using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain.Processors;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Timers;

namespace SiteFuel.Exchange.Domain.QueueService
{
    public class QueueService
    {
        private static QueueService _instance;
        private static Timer aTimer;
        private static readonly object _sycnObjQS = new object();

        private readonly IEnumerable<IQueueMessageProcessor> processors;
        private readonly int TimerPeriod;

        /// <summary>
        /// Intent of singular instance is not served through static GetInstance, as constructor is/was public.
        /// However, now that the need is to let one instance run for default important sequential queue task
        /// and the other file upload to run in separate task, going to use the public constructor
        /// which will not use the GetInstance and only use to get the queue message and process new message
        /// directly and not through default timer run process.
        /// Two instance in use through CJOB:
        /// 1. Using GetInstance and utilizing Timer for default queue process
        /// 2. Will use the public constructor for the Excluded File Upload Process. Else 
        /// would have converted this constructor to private for the original intent
        /// </summary>
        public QueueService()
        {
            ContextFactory.Register(new ApplicationContext());
            TimerPeriod = new ApplicationDomain().GetKeySettingValue<int>(ApplicationConstants.KeyAppSettingQueueServiceRunTimePeriod, 5000);
            processors = (from t in Assembly.GetExecutingAssembly().GetTypes()
                          where t.GetInterfaces().Contains(typeof(IQueueMessageProcessor))
                                   && t.GetConstructor(Type.EmptyTypes) != null
                          select Activator.CreateInstance(t) as IQueueMessageProcessor).ToList();
        }

        private void StartTimer()
        {
            if (aTimer == null)
            {
                aTimer = new System.Timers.Timer();
                aTimer.Elapsed += new ElapsedEventHandler(Run);
            }
        }

        private void ActivateTimer(int period)
        {
            if (aTimer != null)
            {
                aTimer.Interval = period;
                aTimer.Enabled = true;
            }
        }

        private void DeActivateTimer()
        {
            if (aTimer != null)
                aTimer.Enabled = false;
        }

        public static QueueService GetInstance()
        {
            if (_instance == null)
            {
                lock (_sycnObjQS)
                {
                    if (_instance == null)
                    {
                        _instance = new QueueService();
                        _instance.StartTimer();
                    }
                }
            }
            return _instance;
        }

        public void StartProcessing()
        {
            ActivateTimer(TimerPeriod);
        }

        private void Run(object source, ElapsedEventArgs e)
        {
            DeActivateTimer();
            var continueLoop = true;
            try
            {                
                do
                {
                    if ( !DequeueAndProcess() )
                    {
                        ActivateTimer(TimerPeriod);
                        continueLoop = false;
                    }
                }
                while (continueLoop);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QueueService", "Run", $"Queue service is dying. Check inner exception for details.. {ex.Message}", ex);
                ActivateTimer(TimerPeriod * 10);
            }
        }
        
        private bool DequeueAndProcess()
        {
            QueueMessageDomain qmDomain = new QueueMessageDomain();
            QueueMessageViewModel queueMessage = qmDomain.DequeueMessage();
            if (queueMessage != null)
            {
                var processor = processors.FirstOrDefault(x => (x.ProcessorName == queueMessage.QueueProcessType));
                ProcessQueueMessage(queueMessage, processor, qmDomain);
                return true;
            }
            else
                return false;
        }

        public void ProcessQueueMessage(QueueMessageViewModel queueMessage, IQueueMessageProcessor processor, QueueMessageDomain qmDomain)
        {
            var errorInfo = new List<string>();
            string queueMessageJson;
            try
            {
                if (processor != null)
                {
                    processor = Activator.CreateInstance(processor.GetType()) as IQueueMessageProcessor;
                    if (processor.Process(queueMessage, out errorInfo, out queueMessageJson))
                    {
                        switch (processor.ProcessorName)
                        {
                            case QueueProcessType.ThirdPartyOrderBulkUpload:
                                if(errorInfo.Count() > 0)
                                {
                                    qmDomain.SetMessageToBeProcessedAgain(queueMessage, 1, errorInfo);
                                }
                                else
                                {
                                    qmDomain.SetInprocessedMessageProcessed(queueMessage.MessageId);
                                }
                                break;
                            //case QueueProcessType.ExternalMeterDataUpload:
                            //    break;
                            //case QueueProcessType.InvoiceCreated:
                            //    break;
                            //case QueueProcessType.OrderCreated:
                            //    break;
                            //case QueueProcessType.FuelRequestCreated:
                            //    break;
                            //case QueueProcessType.OfferCreated:
                            //    break;
                            //case QueueProcessType.DispatchLocation:
                            //    break;
                            //case QueueProcessType.DtnFileGeneration:
                            //    break;
                            //case QueueProcessType.TankRentalInvoice:
                            //    break;
                            //case QueueProcessType.ReceivePayment:
                            //    break;
                            //case QueueProcessType.InvoiceBulkUpload:
                            //    break;
                            //case QueueProcessType.CreditInvoiceCreation:
                            //    break;
                            //case QueueProcessType.BrokerSplitInvoiceCreation:
                            //    break;
                            //case QueueProcessType.RebillInvoiceCreation:
                            //    break;
                            //case QueueProcessType.InvoiceImageUpload:
                            //    break;
                            //case QueueProcessType.InvoiceUploadErrors:
                            //    break;
                            //case QueueProcessType.PoNumberBulkUpload:
                            //    break;
                            //case QueueProcessType.TankBulkUpload:
                            //    break;
                            //case QueueProcessType.DemandCaptureUpload:
                            //    break;
                            //case QueueProcessType.CreateInvoiceUsingJsonFile:
                            //    break;
                            //case QueueProcessType.CreateMobileInvoiceUsingJsonFile:
                            //    break;
                            //case QueueProcessType.CreateTankOrderMappingInFreightService:
                            //    break;
                            //case QueueProcessType.CreateFreightOnlyOrder:
                            //    break;
                            //case QueueProcessType.TelaFuelServiceOrderAdd:
                            //    break;
                            //case QueueProcessType.ProductMappingBulkUpload:
                            //    break;
                            //case QueueProcessType.CloseFreightOnlyOrder:
                            //    break;
                            //case QueueProcessType.EditBrokerInvoice:
                            //    break;
                            //case QueueProcessType.BrokerInvoiceImageUpload:
                            //    break;
                            //case QueueProcessType.CreateInvoiceForNoData:
                            //    break;
                            //case QueueProcessType.ConvertBrokeredInvoiceForDipData:
                            //    break;
                            //case QueueProcessType.TerminalItemCodeMappingBulkUpload:
                            //    break;
                            //case QueueProcessType.PDIAPIDeliveryDetails:
                            //    break;
                            //case QueueProcessType.FilldAPIDetails:
                            //    break;
                            //case QueueProcessType.ConsolidationForDrCompletion:
                            //    break;
                            //case QueueProcessType.AssetBulkUpload:
                            //    break;
                            //case QueueProcessType.JobsBulkUpload:
                            //    break;
                            //case QueueProcessType.DRCreation:
                            //    break;
                            //case QueueProcessType.SAPAPIDeliveryDetails:
                            //    break;
                            //case QueueProcessType.SAPOrderCreation:
                            //    break;
                            default:
                                qmDomain.SetMessageProcessed(queueMessage.MessageId, 1, errorInfo);
                                break;
                        }
                    }
                    else
                    {
                        if (queueMessageJson != null)
                        {
                            queueMessage.JsonMessage = queueMessageJson;
                        }
                        qmDomain.SetMessageToBeProcessedAgain(queueMessage, 1, errorInfo);
                    }
                }
            }
            catch (QueueMessageFatalException fatalException)
            {
                qmDomain.SetMessageAsFailed(queueMessage.MessageId, 1, fatalException.ErrorInfos);
                LogManager.Logger.WriteException("QueueService", "ProcessQueueMessage", fatalException.Message, fatalException);
            }
            catch (Exception ex)
            {
                errorInfo.Add("Unhandled exception caught by queue service for this queue message");
                qmDomain.SetMessageAsFailed(queueMessage.MessageId, 1, errorInfo);
                LogManager.Logger.WriteException("QueueService", "ProcessQueueMessage", ex.Message, ex);
            }
        }

        public IEnumerable<IQueueMessageProcessor> Processors
        {
            get
            {
                return processors;
            }
        }
    }
}
