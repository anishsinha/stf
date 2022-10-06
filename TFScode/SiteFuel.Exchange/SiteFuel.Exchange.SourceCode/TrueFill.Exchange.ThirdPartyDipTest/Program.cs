using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFill.Exchange.Tjob.ThirdPartyDiptestAlert
{
    class Program
    {
        public static void Main(string[] args)
        {
            LogManager.Logger.WriteInfo("TJob.ThirdPartyDiptestAlert.Program", "Main", "Start");

            Console.WriteLine("TJob.ThirdPartyDiptestAlert.Program - Main - Start");

            ThirdPartyDiptestAlert thirdpartyDiptestAlert = new ThirdPartyDiptestAlert();
            var result = Task.Run(() => thirdpartyDiptestAlert.ProcessThirdPartyDiptestAlerts()).Result;
            
            Console.WriteLine("TJob.ThirdPartyDiptestAlert.Program - Main - End - Result : " + result.ToString());
            LogManager.Logger.WriteInfo($"TJob.ThirdPartyDiptestAlert.Program - {result.ToString()}", "Main", "End");
        }
    }
}
