using Renci.SshNet;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.FileGenerator
{
    public class FtpFileDownloader
    {
        private string _localPath;
        private string _remoteTempDirectory;
        public FtpFileDownloader()
        {
            _localPath = ConfigurationManager.AppSettings.Get("EBolFilePath");
            _remoteTempDirectory = ConfigurationManager.AppSettings.Get("EBolFtpTempFolder");
        }
        public Stream DownloadEbolFile(EBolConfigurationJsonViewModel viewModel)
        {
            Stream stream = new MemoryStream();
            using (SftpClient _sftpClient = new SftpClient(viewModel.FtpUrl, viewModel.FtpUserName, viewModel.FtpPassword))
            {
                try
                {
                    _sftpClient.Connect();

                    if (_sftpClient.Exists(viewModel.RemoteFileName))
                    {
                        String localFilename = viewModel.DestinationFileName + DateTime.Now.ToString("yyyyMMddTHHmmss") + ".csv";
                        var localFilePath = _localPath + localFilename;
                        if (!Directory.Exists(_localPath))
                        {
                            Directory.CreateDirectory(_localPath);
                        }
                        if (!_sftpClient.Exists(_remoteTempDirectory))
                        {
                            _sftpClient.CreateDirectory(_remoteTempDirectory);
                        }

                        using (var _localFile = File.OpenWrite(localFilePath))
                        {
                            string remoteTempFileName = _remoteTempDirectory +"/dailymessage.csv";
                            if (_sftpClient.Exists(remoteTempFileName))
                            {
                                _sftpClient.Delete(remoteTempFileName);
                            }

                            _sftpClient.Create(remoteTempFileName);

                            using (var remoteTempFile = _sftpClient.OpenWrite(remoteTempFileName))
                            {
                                _sftpClient.DownloadFile(viewModel.RemoteFileName, remoteTempFile);
                                if(viewModel.IsDeleteFtpFile)
                                {
                                    _sftpClient.DeleteFile(viewModel.RemoteFileName);
                                }                             
                            }

                            _sftpClient.DownloadFile(remoteTempFileName, stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            stream.CopyTo(_localFile);
                            _sftpClient.DeleteFile(remoteTempFileName);
                        }

                    }
                    else
                    {
                        LogManager.Logger.WriteInfo(viewModel.RemoteFileName + " is not exists.", "FtpFileDownloader", "DownloadEbolFile");
                        Console.WriteLine(viewModel.RemoteFileName + " is not exists.");
                    }

                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    _sftpClient.Disconnect();
                }
                return stream;
            }
        }
    }
}
