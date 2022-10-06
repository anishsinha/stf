using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Logger;

namespace SiteFuel.Exchange.TJob.EBol
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //LogManager.Logger.WriteInfo("TJob.EBol.Program", "Main", "Start");

            ProcessEBol processEBol = new ProcessEBol();
            processEBol.ProcessEBolFile();

            //LogManager.Logger.WriteInfo("TJob.EBol.Program", "Main", "End");
        }
    }
}
