using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.DsbLoadQueue
{
    class Program
    {
        private static int delayTime = 15000;
        private static int delayJob = 60000; //Should we keep for a minute or 2 minutes?
        private const string GetClassName = "CJob.DsbLoadQueue";

        private const string OnHoldSettingsKey = "JobsOnHold";

        private static List<int> onHoldJobs;
        private static string lastOnHoldJobs = string.Empty;

        private static DsbLoadQueueDomain dsbLoadQueueDomain = null;
        private static ScheduleBuilderDomain scheduleBuilderDomain = null;
        private static ProcessLoadQueueDetails processLoadQueueDetails = null;

        public static void Main(string[] args)
        {
            Console.WriteLine("CJob.DsbLoadQueue-started");
            int.TryParse(ConfigurationManager.AppSettings["DelayTime"].ToString(), out delayTime);

            ContextFactory.Register(new ApplicationContext());

            ReadAppSettingsForOnHold(); //Call once at start, to read if any disabled job, as async task can delay reading.

            dsbLoadQueueDomain = ContextFactory.Current.GetDomain<DsbLoadQueueDomain>();
            scheduleBuilderDomain = ContextFactory.Current.GetDomain<ScheduleBuilderDomain>();
            processLoadQueueDetails = new ProcessLoadQueueDetails();

            Process_Jobs();
        }
        
        #region ProcessJobs

        private static void Process_Jobs()
        {
            try
            {
                while (true)
                {

                    Parallel.Invoke(
                        new Action(Call_CheckOnHold),
                        new Action(Call_LoadQueue),
                        new Action(Call_SAPOrder)
                        );
                    //ideally this will never happen, unless all action have gone through exception case
                    WriteInfo(GetClassName, "Process_Jobs", "ALERT: Process is repeating itself.");
                }
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Process_Jobs", ex.Message, ex);
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

        private static void Call_LoadQueue()
        {
            try
            {
                while (true)
                {
                    var result = Task.Run(Process_LoadDSBQueue).Result;
                }
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Call_LoadQueue", ex.Message, ex);
            }
        }
        
        private static void Call_SAPOrder()
        {
            try
            {
                while (true)
                {
                    var result_SAPOrder = Task.Run(Process_SAPOrder).Result;
                }
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Call_SAPOrder", ex.Message, ex);
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

        private static async Task<bool> Process_LoadDSBQueue() 
        {
            try
            {
                if (CanContinue(JobsOnHold.DSBQueue))
                {
                    WriteInfo(GetClassName, "Process_LoadDSBQueue", "Start");
                    var watch = Stopwatch.StartNew();
                    await processLoadQueueDetails.ProcessLoadQueue(dsbLoadQueueDomain, scheduleBuilderDomain);
                    watch.Stop();
                    WriteInfo(GetClassName, "Process_LoadDSBQueue", $"End:TotalTime: {watch.ElapsedMilliseconds}");
                }
                await AwaitFor(delayTime);
                return true;
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Process_LoadDSBQueue", ex.Message, ex);
            }
            return false;
        }

        private static async Task<bool> Process_SAPOrder()
        {
            try
            {
                if (CanContinue(JobsOnHold.SAPOrder))
                {
                    WriteInfo(GetClassName, "Process_SAPOrder", "Start");
                    var watch = Stopwatch.StartNew();
                    ProcessSAPOrderStatus processSAPOrderStatus = new ProcessSAPOrderStatus();
                    await processSAPOrderStatus.ProcessOrderStatus();
                    watch.Stop();
                    WriteInfo(GetClassName, "Process_SAPOrder", $"End:TotalTime: {watch.ElapsedMilliseconds}");
                }
                await AwaitFor(delayJob); 
                return true;
            }
            catch (Exception ex)
            {
                WriteException(GetClassName, "Process_SAPOrder", ex.Message, ex);
            }
            return false;
        }

        #endregion ProcessJobs

        #region HelperMethods
        private static bool CanContinue(JobsOnHold joHold)
        {
            if (onHoldJobs?.Count > 0 && onHoldJobs.Contains((int) joHold))
                return false;
            else
                return true;
        }

        private static async Task AwaitFor(int milliseconds)
        {
            Task wait = Task.Delay(milliseconds);
            await wait;
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

        #endregion HelperMethods
    }
}
