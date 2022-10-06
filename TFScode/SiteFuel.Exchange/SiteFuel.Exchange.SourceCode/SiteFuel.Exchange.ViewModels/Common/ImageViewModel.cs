using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class ImageViewModel : StatusViewModel
    {
        readonly string filePathFormat = "https://{0}/{1}/{2}{3}";

        public ImageViewModel()
        {
        }

        public ImageViewModel(Status status)
            : base(status)
        {
            IsRemoved = false;
        }

        public int Id { get; set; }

		public byte[] Data { get; set; } = new byte[] { 0X20 };

        public bool IsRemoved { get; set; }

        public string Name { get; set; }

        public bool IsPdf { get; set; } //temporary use

        public string SignatureName { get; set; }

        private string _filePath = string.Empty;
        public string FilePath {
            get
            {
                return _filePath;
            }

            set
            {
                _filePath = value;
            }
        }

        public Stream InputStream { get; set; }

        public BlobContainerType BlobContainerType { get; set; }
        public string AzurePath
        {
            get
            {
				if (BlobContainerType == BlobContainerType.Orderbulkupload)
					return string.Empty;
                return GetAzureFilePath(BlobContainerType);
            }
        }

        public string GetAzureFilePath(BlobContainerType blobContainerType)
        {
            return string.Format(filePathFormat, AzureBlobStorage.GetStorageAccountName(), blobContainerType.ToString().ToLower(), FilePath, AzureBlobStorage.GetSaS(blobContainerType.ToString().ToLower()));
        }

        public bool IsNonImageFile
        {
            get
            {
                return IsFilePathTypeIsNonImage(FilePath);
            }
        }

        public static bool IsFilePathTypeIsNonImage(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }
            else
            {
                filePath = filePath.Trim();
            }
            return ApplicationConstants.NonImageSupportedFileExtensions.Any(x => filePath.ToLower().EndsWith(x));

        }
        /// <summary>
        /// upload image to Azure blob server.
        /// </summary>
        /// <param name="fileNamePrefix"></param>
        /// <returns></returns>
        public async Task UploadImageToAzureBlobService(string fileNamePrefix, BlobContainerType container)
        {
            if (Data != null)
            {
                string filePath = await FileHelper.UploadImagetoAzureBlobServer(Data, fileNamePrefix, container);
                Data = null;
                FilePath = filePath;
            }

        }


        public List<ImageViewModel> BreakFilePathToMany()
        {
            List<ImageViewModel> images;
            {
                images = FilePath.Split(ApplicationConstants.FilePathSeparation, System.StringSplitOptions.RemoveEmptyEntries).
                    Select(x => new ImageViewModel { FilePath = x, IsPdf = IsFilePathTypeIsNonImage(x), BlobContainerType = BlobContainerType }).ToList();
            }

            return images;
        }
    }
}
