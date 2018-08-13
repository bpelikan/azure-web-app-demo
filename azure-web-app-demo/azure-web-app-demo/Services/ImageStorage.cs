using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace azure_web_app_demo.Services
{
    public class ImageStorage
    {
        private readonly IConfiguration configuration;

        CloudBlobClient blobClient;
        string baseBlobUri;
        string blobAccountName;
        string blobKeyValue;

        public ImageStorage(IConfiguration configuration)
        {
            this.configuration = configuration;
            baseBlobUri = configuration["blobServiceEndpoint"];
            blobAccountName = configuration["blobAccountName"];
            blobKeyValue = configuration["blobKeyValue"];

            var credential = new StorageCredentials(blobAccountName, blobKeyValue);

            blobClient = new CloudBlobClient(new Uri(baseBlobUri), credential);
        }

        public async Task<string> SaveImage(Stream imageStream, string imageFileName)
        {
            var imageId = Guid.NewGuid().ToString() + ".jpg";

            var container = blobClient.GetContainerReference("images");
            var blob = container.GetBlockBlobReference(imageId);
            blob.Properties.ContentType = "image/jpg";

            await blob.UploadFromStreamAsync(imageStream);

            return imageId;
        }

        public string UriFor(string imageId)
        {
            var sasPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-15),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15)
            };

            var container = blobClient.GetContainerReference("images");
            var blob = container.GetBlockBlobReference(imageId);
            var sas = blob.GetSharedAccessSignature(sasPolicy);

            return $"{baseBlobUri}images/{imageId}{sas}";
        }
    }
}
