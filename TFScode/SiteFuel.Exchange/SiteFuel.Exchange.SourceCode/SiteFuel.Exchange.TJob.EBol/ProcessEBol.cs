using System;
using SiteFuel.Exchange.Domain;
using System.IO;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.TJob.EBol
{
    public class ProcessEBol
    {
        public ProcessEBol()
        {
                //Register Context
                ContextFactory.Register(new ApplicationContext());            
        }
        public void ProcessEBolFile()
        {
                try
                {
                    var fileStream = ReadEBol();
                    fileStream.Seek(0, SeekOrigin.Begin);
                    string csvText = new StreamReader(fileStream).ReadToEnd();
                    if (!string.IsNullOrEmpty(csvText))
                    {
                        var response = SaveEBol(csvText);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ProcessEBol", "ProcessEBolFile", ex.Message, ex);
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }           

        }
        public Stream ReadEBol()
        {
            var fileStream = ContextFactory.Current.GetDomain<EBolDomain>().ReadEBol();
            return fileStream;

        }
       
        public StatusViewModel SaveEBol(string csvText)
        {
            var response = ContextFactory.Current.GetDomain<EBolDomain>().SaveEbol(csvText);
            return response;
        }
    }
}
