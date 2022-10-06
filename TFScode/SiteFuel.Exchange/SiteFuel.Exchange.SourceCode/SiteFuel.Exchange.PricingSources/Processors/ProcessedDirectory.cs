using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.PricingSources.Processors
{
    public class ProcessedDirectory
    {
        private readonly string _fileExtension = "*.csv";
        public ProcessedDirectory(List<string> processedfilePaths)
        {
            var basePath = ConfigurationManager.AppSettings.Get("ProcessedFilePath");
            SourceFiles = processedfilePaths;
            TargetFiles = processedfilePaths.Select(t => basePath + Path.GetFileName(t)).ToList();
        }
        public List<string> SourceFiles { get; set; }
        public List<string> TargetFiles { get; set; }
        public void MoveFiles()
        {
            for (int index = 0; index < SourceFiles.Count; index++)
            {
                var sourceFile = SourceFiles[index];
                var targetFile = TargetFiles[index];
                try
                {
                    if (File.Exists(targetFile))
                    {
                        File.Delete(targetFile);
                    }
                    File.Move(sourceFile, targetFile);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ProcessedDirectory", "MoveFiles", $"{sourceFile}:{targetFile} => " + ex.Message, ex);
                }
            }
        }
    }
}
