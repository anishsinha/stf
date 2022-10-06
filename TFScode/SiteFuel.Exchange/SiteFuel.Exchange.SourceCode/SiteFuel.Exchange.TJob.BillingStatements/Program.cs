using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SiteFuel.Exchange.Logger;

namespace SiteFuel.Exchange.TJob.BillingStatements
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CHANGED THIS JOBS FREQUENCY TO RUN AFTER EVERY 30MINS FOR LFV PROCESS

            LogManager.Logger.WriteInfo("TJob.BillingStatements.Program", "Main", "Start");

            TriggeredBillingStatements triggeredStatement = new TriggeredBillingStatements();
            var result = Task.Run(() => triggeredStatement.ProcessStatementGeneration()).Result;
            LogManager.Logger.WriteInfo($"TJob.BillingStatements.Program- {result}", "Main", "End");

        }
    }
}
