using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Core.Utilities
{
    /// <summary>
    /// Create file helper class for get the file extension from the base 64 string.
    /// </summary>
    public static class FileHelper
    {
        static AzureBlobStorage azureBlobStorage = new AzureBlobStorage();
        static string filePathFolder = @"OldFiles/";
        /// <summary>
        /// To demonstrate extraction of file extension from base64 string.
        /// </summary>
        /// <param name="base64String">base64 string.</param>
        /// <returns>Henceforth file extension from string.</returns>
        public static string GetFileExtension(string base64String)
        {
            if (!string.IsNullOrEmpty(base64String) && base64String.Length > 5)
            {
                string data = base64String.Substring(0, 5);

                switch (data.ToUpper())
                {
                    case "IVBOR":
                        return "png";
                    case "/9J/4":
                        return "jpg";
                    case "JVBER":
                        return "pdf";
                    case "R0LGO":
                        return "gif";
                    case "QK22K":
                        return "bmp";
                    default:
                        return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
        public static async Task<string> UploadImagetoAzureBlobServer(byte[] imageData, string filePrefix, BlobContainerType container)
        {
            try
            {
                //convert byte to base64 string.
                if (imageData != null)
                {
                    Stream streamByte = new MemoryStream(imageData);

                    string base64String = Convert.ToBase64String(imageData, 0, imageData.Length);

                    //get file extension based on the base64 string.
                    string extension = GetFileExtension(base64String);

                    //create the file path.
                    string filename = filePrefix + "_" + DateTime.Now.Ticks + "." + extension;

                    if (!string.IsNullOrEmpty(extension))
                    {
                        var result = await azureBlobStorage.SaveBlobAsync(streamByte, filename, container.ToString().ToLower());
                        if (!string.IsNullOrWhiteSpace(result))
                        {
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FileHelper", "UploadImagetoAzureBlobServer", ex.Message, ex);
            }
            return string.Empty;
        }
        public static string GetValidFileName(string fileName)
        {
            // remove any invalid character from the filename.
            string ret = Regex.Replace(fileName.Trim(), "[^|A-Za-z0-9_. ]+", "", RegexOptions.Compiled);
            return ret.Replace(" ", String.Empty);
        }
    }
}
