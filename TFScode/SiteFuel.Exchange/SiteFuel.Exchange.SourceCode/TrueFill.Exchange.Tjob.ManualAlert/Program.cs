using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Logger;
using System;
using System.Text;
using System.Threading.Tasks;

namespace TrueFill.Exchange.Tjob.ManualAlert
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogManager.Logger.WriteInfo("TJob.ManualAlert.Program", "Main", "Start");
            Console.WriteLine("TJob.ManualAlert.Program - Main - Start");

            ManualDiptestAlert manualDiptestAlert = new ManualDiptestAlert();
            var result = Task.Run(() => manualDiptestAlert.ProcessManualDiptestAlerts()).Result;
            
            Console.WriteLine("TJob.ManualAlert.Program - Main - End - Result : "+ result.ToString());
            LogManager.Logger.WriteInfo($"TJob.ManualAlert.Program - {result.ToString()}", "Main", "End");
        }
    }
}
