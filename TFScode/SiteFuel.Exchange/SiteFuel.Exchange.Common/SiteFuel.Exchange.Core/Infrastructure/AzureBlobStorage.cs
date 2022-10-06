using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SiteFuel.Exchange.Logger;
using System;
using System.Configuration;
using System.IO;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Core.Infrastructure
{
	public class AzureBlobStorage : IBlobStorage
	{
		readonly static string AccountConnectionString = ConfigurationManager.AppSettings["webjobstorage"].ToString();

		static CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AccountConnectionString);

		private static ObjectCache Cache
		{
			get
			{
				return MemoryCache.Default;
			}
		}

		/// <summary>
		/// Initialize BLOB and Queue Here
		/// </summary>
		public AzureBlobStorage()
		{
		}


		public static string GetStorageAccountName()
		{
			//DefaultEndpointsProtocol=https;AccountName=sitefuelqadiag996;AccountKey=XCpmAABpeaerX8246k096lNDQKbvV7HZO1Sk7wEoaISyR7Pt62VcWfgpwtkfq/FeDDwPQeXPXGpQ4PmeBr5Ozg==;EndpointSuffix=core.windows.net
			return AccountConnectionString.Split(';')[1].Split('=')[1] + ".blob.core.windows.net";
		}

		/// <summary>
		/// Method to Upload the BLOB
		/// </summary>
		/// <param name=""profileFile"">
		/// <returns></returns>
		public async Task<string> SaveBlobAsync(Stream fileStram, string fileNameWithExtension, string container)
		{
			string blobName = string.Empty;

			if (!string.IsNullOrWhiteSpace(fileNameWithExtension))
			{
				blobName = fileNameWithExtension;

				CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

				// Get the blob container reference.
				var profileBlobContainer = blobClient.GetContainerReference(container);
				//Create Blob Container if not exist
				profileBlobContainer.CreateIfNotExists();
				// GET a blob reference. 
				CloudBlockBlob profileBlob = profileBlobContainer.GetBlockBlobReference(blobName);
				// Uploading a local file and Create the blob.
				fileStram.Position = 0;
				await profileBlob.UploadFromStreamAsync(fileStram);
			}
			return blobName;
		}

		/// <summary>
		/// Downloads the blob to a stream
		/// </summary>
		/// <param name="blobName"></param>
		/// <param name="container"></param>
		/// <returns></returns>
		public Stream DownloadBlob(string blobName, string container)
		{
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			// Get the blob container reference.
			var profileBlobContainer = blobClient.GetContainerReference(container);

			// GET a blob reference. 
			CloudBlockBlob profileBlob = profileBlobContainer.GetBlockBlobReference(blobName);
			// Uploading a local file and Create the blob.
			MemoryStream memoryStream = new MemoryStream();
			profileBlob.DownloadToStream(memoryStream);
			memoryStream.Position = 0;
			return memoryStream;
		}

		/// <summary>
		/// Method to delete the BLOB
		/// </summary>
		/// <param name="documentName">Document/File name should be matching name of document/file at the time of creating BLOB</param>
		/// <param name="container"></param>
		/// <returns></returns>
		public async Task<string> DeleteBlobAsync(string documentName, string container)
		{
			string blobName = string.Empty;

			if (!string.IsNullOrWhiteSpace(documentName))
			{
				try
				{
					blobName = documentName;
					CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

					// Get the blob container reference.
					var blobContainer = blobClient.GetContainerReference(container);

					// GET a blob reference. 
					CloudBlockBlob blob = blobContainer.GetBlockBlobReference(blobName);

					// delete the blob.
					await blob.DeleteIfExistsAsync();
				}
				catch (Exception ex)
				{
					LogManager.Logger.WriteException("AzureBlobStorage", "DeleteBlobAsync", ex.Message, ex);
					return string.Empty;
				}
			}
			return blobName;
		}


		public static string GetSaS(string containerName, string storedPolicyName = null)
		{
			string sasContainerToken;
			var key = "Sas" + containerName;
			var value = Cache.Get(key);
			if (value != null)
			{
				return value.ToString();
			}

			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
			var blobContainer = blobClient.GetContainerReference(containerName.ToLower());

			// If no stored policy is specified, create a new access policy and define its constraints.
			if (storedPolicyName == null)
			{
				// Note that the SharedAccessBlobPolicy class is used both to define the parameters of an ad hoc SAS, and
				// to construct a shared access policy that is saved to the container's shared access policies.
				SharedAccessBlobPolicy adHocPolicy = new SharedAccessBlobPolicy()
				{
					// When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
					// Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
					SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
					Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List
				};

				// Generate the shared access signature on the container, setting the constraints directly on the signature.
				sasContainerToken = blobContainer.GetSharedAccessSignature(adHocPolicy, null);

			}
			else
			{
				// Generate the shared access signature on the container. In this case, all of the constraints for the
				// shared access signature are specified on the stored access policy, which is provided by name.
				// It is also possible to specify some constraints on an ad hoc SAS and others on the stored access policy.
				sasContainerToken = blobContainer.GetSharedAccessSignature(null, storedPolicyName);
			}

			CacheItemPolicy policy = new CacheItemPolicy();
			policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(3600 * 12);
			Cache.Add(new CacheItem(key, sasContainerToken), policy);
			return sasContainerToken;
		}

		public string GetContainerSasUri(string containerName, string storedPolicyName = null)
		{
			string sasContainerToken;

			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
			var blobContainer = blobClient.GetContainerReference(containerName.ToLower());

			// If no stored policy is specified, create a new access policy and define its constraints.
			if (storedPolicyName == null)
			{
				// Note that the SharedAccessBlobPolicy class is used both to define the parameters of an ad hoc SAS, and
				// to construct a shared access policy that is saved to the container's shared access policies.
				SharedAccessBlobPolicy adHocPolicy = new SharedAccessBlobPolicy()
				{
					// When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
					// Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
					SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
					Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List
				};

				// Generate the shared access signature on the container, setting the constraints directly on the signature.
				sasContainerToken = blobContainer.GetSharedAccessSignature(adHocPolicy, null);

			}
			else
			{
				// Generate the shared access signature on the container. In this case, all of the constraints for the
				// shared access signature are specified on the stored access policy, which is provided by name.
				// It is also possible to specify some constraints on an ad hoc SAS and others on the stored access policy.
				sasContainerToken = blobContainer.GetSharedAccessSignature(null, storedPolicyName);
			}

			// Return the URI string for the container, including the SAS token.
			return blobContainer.Uri + sasContainerToken;

		}
	}
}
