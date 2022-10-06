using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFill.Exchange.TJob.LFVServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("LFVServices Start");

            // lift file Detail Validation 
            LFVService lfvService = new LFVService();
            var lfvresult = Task.Run(() => lfvService.ProcessLiftFileValidate()).Result;
            Console.WriteLine($"Process lfv Data : {lfvresult}");

            var domain = new CarrierDomain();
            var isProcessed = Task.Run(() => domain.TriggerCarrierInventoryEmailExport()).Result;            

            LogManager.Logger.WriteInfo($"TJob.LFVServices.Program- {lfvresult}", "Main", "End");
        }
    }
}
