using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.PricingSources.DataAccess;
using SiteFuel.Exchange.PricingSources.Processors;
using SiteFuel.Exchange.PricingSources.WebApi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;

namespace SiteFuel.Exchange.PricingSources
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG
            var culture = new System.Globalization.CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
#endif
            try
            {
                var theadSleepingTimeInMillis = GetSleepTime();
                var isLocalServer = IsLocalServer();
                var fileFilter = new FileFilter();
                var pendingFiles = new List<PendingFile>();
                if (isLocalServer)
                {
                    var pendingDirectoryLoc = new PendingDirectory();
                    //Console.WriteLine("Getting pending files to process...");
                    LogManager.Logger.WriteInfo("Program", "Main", "Getting pending files to process...");
                    pendingFiles = fileFilter.GetPendingFiles(pendingDirectoryLoc.Files);
                }
                else
                {
                    pendingFiles = GetPendingFilesFromFTP(fileFilter);
                }

                //Console.WriteLine("Saving pending files into database...");
                LogManager.Logger.WriteInfo("Program", "Main", "Saving pending files into database...");
                fileFilter.SavePendingFiles(pendingFiles);

                //Console.WriteLine("Processing pending files...");
                LogManager.Logger.WriteInfo("Program", "Main", "Processing pending files... file count : " + pendingFiles.Count);
                var fileProcessor = new CsvFileProcessor();
                var processedFiles = new List<int>();
                foreach (var pending in pendingFiles)
                {
                    processedFiles = ProcessPendingFiles(fileProcessor, pending);
                    fileFilter.UpdateStatusToProcessed(processedFiles);
                    MoveProcessedFilesToDirectory(processedFiles, pending);
                    SyncPricingApi(theadSleepingTimeInMillis, processedFiles);
                }

                //Console.WriteLine("Completed...");
                LogManager.Logger.WriteInfo("Program", "Main", "Service Status: Completed");
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("Program", "Main", ex.Message, ex );
                Console.WriteLine(ex);
                LogManager.Logger.WriteInfo("Program", "Main", "Service Status: Error");
            }
        }

        private static List<int> ProcessPendingFiles(CsvFileProcessor fileProcessor, PendingFile pending)
        {
            //Console.WriteLine("Processing file: " + pending.FullPath);
            LogManager.Logger.WriteInfo("Program", "Main", "Processing pending files..." + pending.FullPath);
            var currentFile = new List<PendingFile> { pending };
            return fileProcessor.ProcessFiles(currentFile);
        }

        private static void SyncPricingApi(int theadSleepingTimeInMillis, List<int> processedFiles)
        {
            if (processedFiles.Any())
            {
                LogManager.Logger.WriteDebug("Program", "Main", "Start syncing");
                var syncPricing = new SyncPricingApi();
                syncPricing.StartSyncing();
            }
            if (processedFiles.Count > 1)
            {
                Console.WriteLine("Service Status: Sleeped for " + theadSleepingTimeInMillis);
                LogManager.Logger.WriteInfo("Program", "Main", "Service Status: Sleeped for " + theadSleepingTimeInMillis);
                Thread.Sleep(theadSleepingTimeInMillis);
            }
        }

        private static void MoveProcessedFilesToDirectory(List<int> processedFiles, PendingFile pending)
        {
            //Console.WriteLine("Moving pending files to processed directory...");
            LogManager.Logger.WriteInfo("Program", "Main", "Moving pending files to processed directory... processed files count :  " + processedFiles.Count);
            var processedDirectory = new ProcessedDirectory(new List<string> { pending.FullPath });
            processedDirectory.MoveFiles();
        }

        private static List<PendingFile> GetPendingFilesFromFTP(FileFilter fileFilter)
        {
            List<PendingFile> pendingFiles;
            //Console.WriteLine("Getting remote file list before download...");
            LogManager.Logger.WriteDebug("Program", "Main", "Getting remote file list before download...");
            var fileDownloader = new FtpFileDownloader();
            var remoteFiles = fileDownloader.GetRemoteFiles();

            //Console.WriteLine("Getting pending files to process...");
            pendingFiles = fileFilter.GetPendingFiles(remoteFiles);

            //Console.WriteLine("Downloading pending files...");
            fileDownloader.DownloadFiles(pendingFiles);
            return pendingFiles;
        }

        private static bool IsLocalServer()
        {
            var result = false;
            try
            {
                var isLocal = ConfigurationManager.AppSettings.Get("IsLocal");
                result = Convert.ToBoolean(isLocal);
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public static int GetSleepTime()
        {
            var result = 300000;
            try
            {
                var isLocal = ConfigurationManager.AppSettings.Get("TheadSleepingTimeInMillis");
                result = Convert.ToInt32(isLocal);
            }
            catch (Exception)
            {
                result = 300000;
            }
            return result;
        }
    }
}
