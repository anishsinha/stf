using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.FileGenerator
{
    public class FtpFileUploader
    {
        public void UploadDtnFile(string invoiceNumber, string dtnFileCsvString, string ftpUserName, string ftpPassword, string ftpUrl, string folderName)
        {
            if (string.IsNullOrEmpty(ftpUrl))
                return;
            using (SftpClient _sftpClient = new SftpClient(ftpUrl, ftpUserName, ftpPassword))
            {
                _sftpClient.Connect();
                _sftpClient.ChangeDirectory(folderName);
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(dtnFileCsvString));
                _sftpClient.UploadFile(stream, invoiceNumber + ".csv");
            }
        }
    }
}
