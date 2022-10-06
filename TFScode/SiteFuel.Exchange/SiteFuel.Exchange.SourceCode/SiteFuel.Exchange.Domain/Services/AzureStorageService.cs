using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Services
{
    public static class AzureStorageService
    {
        public static async Task<StatusViewModel> UploadImageToBlob(UserContext userContext, Stream fileStream, string fileName, BlobContainerType containerType)
        {
            using (var tracer = new Tracer("AzureStorageService", "UploadImageToBlob"))
            {
                var response = new StatusViewModel(Status.Failed);
                try
                {
                    var azureBlob = new AzureBlobStorage();

                    var azureFileName = GenerateImageName(userContext.Id, fileName);
                    var filePath = await azureBlob.SaveBlobAsync(fileStream, azureFileName, containerType.ToString().ToLower());
                    if (!string.IsNullOrWhiteSpace(filePath))
                    {
                        response.StatusCode = Status.Success;
                        response.StatusMessage = filePath;
                    }
                    else
                        response.StatusMessage = Resource.errMessageErrorInAzureServer;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageErrorInAzureServer;
                    LogManager.Logger.WriteException("AzureStorageService", "UploadImageToBlob", ex.Message, ex);
                }
                return response;
            }
        }

        private static string GenerateImageName(int userId, string fileName)
        {
            string[] filenames = fileName.Split('/');
            if (filenames != null && filenames.Length > 1)
            {
               string imgName= string.Concat(values: userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + Resource.lblSingleHyphen + filenames[filenames.Length-1]);
                return filenames.FirstOrDefault() + "/" + imgName;
            }
            return string.Concat(values: userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + Resource.lblSingleHyphen + fileName);
        }
    }
}
