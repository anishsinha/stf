using SiteFuel.Exchange.CJob.DeliveryAlerts.Firebase;
using SiteFuel.Exchange.CJob.Notifications;
using SiteFuel.Exchange.CJob.Workflows;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Domain.Processors;
using SiteFuel.Exchange.Domain.QueueService;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.DeliveryAlerts
{
    public class Program
    {
        private const int delayJob = 60000;
        private const string GetClassName = "CJob";
        private const string OnHoldSettingsKey = "JobsOnHold";

        private static int delayQueueJob = 5000;
        private static int delayApiReprocessJob = 900000; //15mins = 15 * 60 * 1000
        private static List<int> onHoldJobs;
        private static string lastOnHoldJobs = string.Empty;
        private static QueueService qsFileUpload;

        public static void Main(string[] args)
        {
            WriteInfo(GetClassName, "Main", "Start");

            //ConfigurePrinter();

            ContextFactory.Register(new ApplicationContext());
            delayQueueJob =  new ApplicationDomain().GetKeySettingValue<int>(ApplicationConstants.KeyAppSettingQueueServiceRunTimePeriod, 5000);
            ReadAppSettingsForOnHold(); //Call once at start, to read if any disabled job, as async task can delay reading.

           Start_QueueService();

            //Init_FireStoreClient();

            Process_Jobs();

        }

        private static void Start_QueueService()
        {
            qsFileUpload = new QueueService();
            //QueueService qService = QueueService.GetInstance();
            //qService.StartProcessing();
        }

        private static void Init_FireStoreClient()
        {
            var myFirestoreClient = new MyFirestoreClient();
            myFirestoreClient.ListenForInvoiceDropChanges();
            myFirestoreClient.ListenPreLoadBolForChanges();
            myFirestoreClient.ListenForEditedPreLoadBolChanges();
            myFirestoreClient.ListenForDeletedPreLoadBolChanges();
            myFirestoreClient.ListenForFuelRetainChanges();
            myFirestoreClient.ListenPickupBolRetainForChanges();
            myFirestoreClient.ListenForCancelledSchedule();
        }

        #region ProcessJobs
        private static void Process_Jobs()
        {
            while (true)
            {
                try
                {
                    //
                    //Each Action method in Parallel process is run in its own while (true) condition to continue its process once it's individual cycle is complete
                    //and so that one action is not affected by other action running for long.
                    //Note: if we don't have while(true) for each of the action call method, than it will wait for all the action process to complete
                    //and come back to this loop to start again, which will result to wait and in a way sequential only (even when they will run parallely,
                    //but will wait for 2nd round of exeuction only when all other actions are complete, hence each provided with it's own while(true) loop)
                    //This will also ensure, if one of the process has technical failures or delay, then it won't affect other action Process.
                    Parallel.Invoke(
                        //new Action(Call_CheckOnHold),
                        //new Action(Call_Delivery),
                        //new Action(Call_Pricing),
                        //new Action(Call_QuickBookWorkflow),
                        //new Action(Call_Notifications),
                        //new Action(Call_WaitingForTax),
                        //new Action(Call_FailedApiReProcess),
                        //new Action(Call_PDIApiQueue),
                        new Action(Call_FileUploadQueue)
                        );
                    //ideally this will never happen, unless all action have gone through exception case
                    WriteInfo(GetClassName, "Process_Jobs", "ALERT: Process is repeating itself.");
                }
                catch (Exception ex)
                {
                    WriteException(GetClassName, "Process_Jobs", ex.Message, ex);
                }
            }
        }

        private static void ReadAppSettingsForOnHold()
        {
            try
            {
                ApplicationDomain appD = new ApplicationDomain();
                string asJobsOnHold = appD.GetKeySettingValue<string>(OnHoldSettingsKey, string.Empty);

                if (!string.IsNullOrEmpty(asJobsOnHold) && asJobsOnHold != lastOnHoldJobs)
                {
                    lastOnHoldJobs = asJobsOnHold;
                    String[] values = asJobsOnHold.Split(',');
                    onHoldJobs = new List<int>();
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (int.TryParse(values[i], out int currentValue)) //to avoid any non int value set accidently in app settings.
                            onHoldJobs.Add(currentValue);    //onHoldJobs = asJobsOnHold.Split(',')?.Select(int.Parse).ToList();
                    }                    
                }
                else
                    lastOnHoldJobs = asJobsOnHold;

                if (string.IsNullOrEmpty(lastOnHoldJobs) && (onHoldJobs?.Count > 0))
                    onHoldJobs = new List<int>();
            }
            catch (Exception ex)
            {
                onHoldJobs = null;
                WriteException(GetClassName, "ReadAppSettingsForOnHold", ex.Message, ex);
            }

        }

        private static void Call_CheckOnHold()
        {
            try
            {
                while (true)
                {
                    var result = Task.Run(Process_CheckOnHold).Result;
                }
            }
            catch
            {
                //DO NOTHING
            }
        }

        private static void Call_Delivery()
        {
            try
            {
                while (true)
                {
                    var result_Delivery = Task.Run(Process_Delivery).Result;
                }
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Call_Delivery", ex.Message, ex);
            }
        }

        private static void Call_Pricing()
        {
            try
            {
                while (true)
                {
                    var result_Pricing = Task.Run(Process_Pricing).Result;
                }
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Call_Pricing", ex.Message, ex);
            }
        }

        private static void Call_QuickBookWorkflow()
        {
            try
            {
                while (true)
                {
                    var result_QuickBooksWorkFlow = Task.Run(Process_QuickbooksWorkflow).Result;
                }
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Call_QuickBookWorkflow", ex.Message, ex);
            }
        }

        private static void Call_Notifications()
        {
            try
            {
                while (true)
                {
                    var result_Notifications = Task.Run(Process_Notifications).Result;
                }
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Call_Notifications", ex.Message, ex);
            }
        }

        private static void Call_WaitingForTax()
        {
            try
            {
                while (true)
                {
                    var result_WaitingForTax = Task.Run(Process_WaitingForTax).Result;
                }
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Call_WaitingForTax", ex.Message, ex);
            }
        }

        private static void Call_FailedApiReProcess()
        {
            try
            {
                while (true)
                {
                    var result_failedApiReprocess = Task.Run(Process_FailedApiReProcess).Result;
                }
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Process_FailedApiReProcess", ex.Message, ex);
            }
        }
        private static void Call_PDIApiQueue()
        {
            try
            {
                while (true)
                {
                    var result_WaitingForTax = Task.Run(Process_PDIApiQueue).Result;
                }
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Call_PDIApiQueue", ex.Message, ex);
            }
        }

        private static void Call_FileUploadQueue()
        {
            try
            {
                while (true)
                {
                    var result_FileUploadQueue = Task.Run(Process_FileUploadQueue).Result;
                }
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Call_FileUploadQueue", ex.Message, ex);
            }
        }

        private static async Task<bool> Process_CheckOnHold()
        {
            try
            {
                ReadAppSettingsForOnHold();
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Process_CheckOnHold", ex.Message, ex);
            }
            await Task.Delay(delayJob);
            return true;
        }

        private static async Task<bool> Process_Delivery()
        {
            try
            {
                if (CanContinue(JobsOnHold.DeliveryAlerts))
                {
                    WriteInfo(GetClassName, "Process_Delivery", "Start");
                    var watch = Stopwatch.StartNew();
                    await new ContinuousDeliveryAlters().ProcessDeliveryAlters();
                    WriteInfo(GetClassName, "Process_Delivery", $"End:TotalTime: { watch.ElapsedMilliseconds}");
                }
                await AwaitFor(delayJob);
                return true;
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Process_Delivery", ex.Message, ex);
            }
            return false;
        }

        private static async Task<bool> Process_Pricing()
        {
            try
            {
                if (CanContinue(JobsOnHold.Pricing))
                {
                    WriteInfo(GetClassName, "Process_Pricing", "Start");
                    var watch = Stopwatch.StartNew();
                    await new ExternalPricingDomain().ExecutePricingSync();
                    WriteInfo(GetClassName, "Process_Pricing", $"End:TotalTime: {watch.ElapsedMilliseconds}");
                }
                await AwaitFor(delayJob);
                return true;
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Process_Pricing", ex.Message, ex);
            }
            return false;
        }

        private static async Task<bool> Process_QuickbooksWorkflow()
        {
            try
            {
                if (CanContinue(JobsOnHold.QuickBookWorkflow))
                {
                    WriteInfo(GetClassName, "Process_QuickbooksWorkflow", "Start");
                    var watch = Stopwatch.StartNew();
                    new QuickbooksWorkflowController().StartQuickbooksWorkflow();
                    watch.Stop();
                    WriteInfo(GetClassName, "Process_QuickbooksWorkflow", $"End:TotalTime: {watch.ElapsedMilliseconds}");

                }
                await AwaitFor(delayJob);
                return true;
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Process_QuickbooksWorkflow", ex.Message, ex);
            }
            return false;
        }

        private static async Task<bool> Process_Notifications()
        {
            try
            {
                if (CanContinue(JobsOnHold.Notifications))
                {
                    WriteInfo(GetClassName, "Process_Notifications", "Start");
                    var watch = Stopwatch.StartNew();
                    await new ContinuousEmailNotifications().ProcessEmailNotifications();
                    watch.Stop();
                    WriteInfo(GetClassName, "Process_Notifications", $"End: TotalTime: {watch.ElapsedMilliseconds}");
                }
                await AwaitFor(delayJob);
                return true;
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Process_Notifications", ex.Message, ex);
            }
            return false;
        }

        private static async Task<bool> Process_WaitingForTax()
        {
            try
            {
                if (CanContinue(JobsOnHold.WaitingForTax))
                {
                    WriteInfo(GetClassName, "Process_WaitingForTax", "Start");
                    var watch = Stopwatch.StartNew();
                    await new ProcessWaitingForTaxEntity().ProcessWaitingForTaxDdt();
                    watch.Stop();
                    WriteInfo(GetClassName, "Process_WaitingForTax", $"End:TotalTime: {watch.ElapsedMilliseconds}");
                }
                await AwaitFor(delayJob);
                return true;
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Process_WaitingForTax", ex.Message, ex);
            }
            return false;
        }

        private static async Task<bool> Process_FailedApiReProcess()
        {
            try
            {
                if (CanContinue(JobsOnHold.FailedAPIReprocess))
                {
                    WriteInfo(GetClassName, "Process_FailedApiReProcess", "Start");
                    var watch = Stopwatch.StartNew();
                    await new ProcessWaitingForTaxEntity().ProcessFailedLocationCreateRequests();
                    watch.Stop();
                    WriteInfo(GetClassName, "Process_FailedApiReProcess", $"End:TotalTime: {watch.ElapsedMilliseconds}");

                    await AwaitFor(delayApiReprocessJob);
                }
                return true;
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Process_FailedApiReProcess", ex.Message, ex);
            }
            return false;
        }

        private static async Task<bool> Process_PDIApiQueue()
        {
            try
            {
                if (CanContinue(JobsOnHold.PDIApi))
                {
                    WriteInfo(GetClassName, "Process_PDIApiQueue", "Start");
                    var watch = Stopwatch.StartNew();
                    await Task.Run(() => DequeueAndProcessPDI());
                    watch.Stop();
                    WriteInfo(GetClassName, "Process_PDIApiQueue", $"End:TotalTime: {watch.ElapsedMilliseconds}");
                }
                await AwaitFor(delayQueueJob);
                return true;
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Process_PDIApiQueue", ex.Message, ex);
            }
            return false;            
        }

        private static async Task<bool> Process_FileUploadQueue()
        {
            try
            {
                //TODO: Can Continue:~ Keeping it simple now, and need to add queue process on hold key
                //separately for each File Upload process type into mst appsettings
                //For now, all file upload will be processed in sequential order. We can here simply change
                //and make parallel process for each different file upload type
                if (CanContinue(JobsOnHold.FileUpload)) 
                {
                    WriteInfo(GetClassName, "Process_FileUploadQueue", "Start");
                    var watch = Stopwatch.StartNew();
                    await Task.Run(() => DequeueAndProcessFileUpload(QueueMessageStatus.Pending));
                    watch.Stop();
                    WriteInfo(GetClassName, "Process_FileUploadQueue", $"End:TotalTime: {watch.ElapsedMilliseconds}");
                }
                await AwaitFor(delayQueueJob);
                return true;
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Process_FileUploadQueue", ex.Message, ex);
            }
            return false;
        }

        private static void DequeueAndProcessPDI()
        {
            bool canContinue = true;
            try
            {
                do
                {
                    QueueMessageDomain qmDomain = new QueueMessageDomain();
                    ViewModels.Queue.QueueMessageViewModel queueMessage = qmDomain.DequeueMessagePDI();
                    if (queueMessage != null)
                    {
                        ProcessQueueMessage(queueMessage, qmDomain);
                    }
                    else
                        canContinue = false;
                } while (canContinue);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException(GetClassName, "DequeueAndProcessPDI", ex.Message, ex);
            }
        }

        private static void ProcessQueueMessage(QueueMessageViewModel queueMessage, QueueMessageDomain qmDomain)
        {
            var errorInfo = new List<string>();
            string queueMessageJson;
            try
            {
                Domain.Processors.PDIAPIProcessor PDIApiProc = new Domain.Processors.PDIAPIProcessor();

                if (PDIApiProc.Process(queueMessage, out errorInfo, out queueMessageJson))
                {
                    qmDomain.SetMessageProcessed(queueMessage.MessageId, 1, errorInfo);
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
            catch (QueueMessageFatalException fatalException)
            {
                qmDomain.SetMessageAsFailed(queueMessage.MessageId, 1, fatalException.ErrorInfos);
                LogManager.Logger.WriteException(GetClassName, "ProcessQueueMessage", fatalException.Message, fatalException);
            }
            catch (Exception ex)
            {
                errorInfo.Add("Unhandled exception caught by queue service for this queue message");
                qmDomain.SetMessageAsFailed(queueMessage.MessageId, 1, errorInfo);
                LogManager.Logger.WriteException(GetClassName, "ProcessQueueMessage", ex.Message, ex);
            }
        }

        private static void DequeueAndProcessFileUpload(QueueMessageStatus qmStatus)
        {
            bool canContinue = true;
            try
            {
                do
                {
                    QueueMessageDomain qmDomain = new QueueMessageDomain();
                    QueueMessageViewModel queueMessage = qmDomain.DequeueMessageFileUpload(qmStatus);
                    if (queueMessage != null)
                    {
                        var processor = qsFileUpload.Processors.FirstOrDefault(x => (x.ProcessorName == queueMessage.QueueProcessType));
                        qsFileUpload.ProcessQueueMessage(queueMessage, processor, qmDomain);
                        canContinue = true;
                    }
                    else
                        canContinue = false;
                } while (canContinue);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException(GetClassName, "DequeueAndProcessFileUpload", ex.Message, ex);
            }
        }
              
        #endregion ProcessJobs

        #region Helper Methods
        private static bool CanContinue(JobsOnHold joHold)
        {
            if (onHoldJobs?.Count > 0 && onHoldJobs.Contains((int)joHold))
                return false;
            else
                return true;
        }

        private static async Task<bool> AwaitFor(int milliseconds)
        {
            await Task.Delay(milliseconds);
            return true;
        }

        private static void WriteInfo(string controllerClass, string actionMethod, string message)
        {
            #if (DEBUG)
                PrintMessage($"{controllerClass}: {actionMethod} - Info: {message}");
            #endif
            LogManager.Logger.WriteInfo(controllerClass, actionMethod, message);
        }

        private static void WriteException(string controllerClass, string actionMethod, string message, Exception ex)
        {
            #if (DEBUG)
                PrintMessage($"{controllerClass}: {actionMethod} - Exception: {message}");
            #endif
            LogManager.Logger.WriteException(controllerClass, actionMethod, message, ex);
        }

        private static void PrintMessage(string msg)
        {
            Console.WriteLine(msg);
        }
        
        //private static void ConfigurePrinter()
        //{
        //    //ConfigurationPrinter printer = new ConfigurationPrinter();
        //    //printer.PrintConnectionStrings();
        //    //printer.PrintAppSettings();
        //    //WriteInfo(GetClassName, "ConfigurePrinter", "Completed Printing appsettings");
        //}

        #endregion Helper Methods
    }
}
