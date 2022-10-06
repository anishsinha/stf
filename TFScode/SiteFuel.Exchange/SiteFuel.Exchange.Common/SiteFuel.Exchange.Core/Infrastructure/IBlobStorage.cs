using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SiteFuel.Exchange.Core.Infrastructure
{
    public interface IBlobStorage
    {
        /// <summary>
        /// Saves the blob to target system. Make sure to dispose the stream on your own
        /// </summary>
        /// <param name="fileStram"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
         Task<string> SaveBlobAsync(Stream fileStram , string fileNameWithExtension, string container);

        /// <summary>
        /// Downloads the blob to a stream. Note- dispose the stream after usage on your own
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        Stream DownloadBlob(string blobName, string container);
    }
}
