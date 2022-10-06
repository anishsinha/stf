using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFill.Exchange.TJob.DiptestServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("DiptestServices Start");

            DiptestService diptestService = new DiptestService();
            // Process is360 
            var Is360Response = Task.Run(() => diptestService.ProcessIs360Data()).Result;
            Console.WriteLine($"Process Process Is360 Data : {Is360Response}");

            LogManager.Logger.WriteInfo($"TJob.DiptestServices.Program- {Is360Response}", "Main", "End");
        }
    }
}
