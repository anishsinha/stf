using Renci.SshNet;
using Renci.SshNet.Common;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.PricingSources.Processors;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.PricingSources
{
    public class FtpFileDownloader
    {
        private string _localPath;
        private string _remotePath;
        private SftpClient _sftpClient;
        public FtpFileDownloader()
        {
            _sftpClient = GetSftpClient();
            _localPath = ConfigurationManager.AppSettings.Get("PendingFilePath");
            _remotePath = ConfigurationManager.AppSettings.Get("RemoteDirectoryPath");
        }

        public List<string> GetRemoteFiles()
        {
            var result = new List<string>();
            try
            {
                _sftpClient.Connect();
                var files = _sftpClient.ListDirectory(_remotePath);
                files = files.Where(t => Path.GetExtension(t.FullName).Equals(".csv", StringComparison.OrdinalIgnoreCase));
                result = files.Where(t => !t.IsDirectory).Select(t => t.FullName).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FtpFileDownloader", "GetRemoteFiles", ex.Message, ex);
                LogManager.Logger.WriteDebug("FtpFileDownloader", "GetRemoteFiles", "Service Type: Pricing Sources");

            }
            finally
            {
                if (_sftpClient.IsConnected)
                    _sftpClient.Disconnect();
            }
            return result;
        }

        public void DownloadFiles(List<PendingFile> files)
        {
            try
            {
                _sftpClient.Connect();
                foreach (var file in files.Where(t => !t.IsDownloaded))
                {
                    var localFilePath = _localPath + file.Name;
                    var remoteFilePath = _remotePath + file.Name;
                    using (var fileStream = File.OpenWrite(localFilePath))
                    {
                        _sftpClient.DownloadFile(remoteFilePath, fileStream);
                        file.DownloadedOn = DateTimeOffset.Now;
                        file.IsDownloaded = true;
                        LogManager.Logger.WriteInfo("FtpFileDownloader", "DownloadFiles", "Downloaded: " + remoteFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FtpFileDownloader", "DownloadFiles", ex.Message, ex);
                LogManager.Logger.WriteDebug("FtpFileDownloader", "DownloadFiles", "Service Type: Pricing Sources");
            }
            finally
            {
                if (_sftpClient.IsConnected)
                    _sftpClient.Disconnect();
            }
        }

        private SftpClient GetSftpClient()
        {
            string host = ConfigurationManager.AppSettings.Get("FtpHost");
            string username = ConfigurationManager.AppSettings.Get("FtpUserName");
            string password = ConfigurationManager.AppSettings.Get("FtpPassword");
            SftpClient sftpClient = new SftpClient(host, username, password);
            return sftpClient;
        }
    }
}
