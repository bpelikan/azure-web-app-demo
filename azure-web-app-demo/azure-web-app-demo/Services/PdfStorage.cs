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
    public class PdfStorage
    {
        private readonly IConfiguration configuration;

        CloudBlobClient blobClient;
        string baseBlobUri;
        string blobAccountName;
        string blobKeyValue;

        public PdfStorage(IConfiguration configuration)
        {
            this.configuration = configuration;
            baseBlobUri = configuration["blobServiceEndpoint"];
            blobAccountName = configuration["blobAccountName"];
            blobKeyValue = configuration["blobKeyValue"];

            var credential = new StorageCredentials(blobAccountName, blobKeyValue);

            blobClient = new CloudBlobClient(new Uri(baseBlobUri), credential);
        }

        public async Task<string> SavePdf(Stream pdfStream)
        {
            var pdfId = Guid.NewGuid().ToString() + ".pdf";

            var container = blobClient.GetContainerReference("pdf");
            var blob = container.GetBlockBlobReference(pdfId);
            blob.Properties.ContentType = "application/pdf";

            await blob.UploadFromStreamAsync(pdfStream);

            return pdfId;
        }

        public string UriFor(string pdfId)
        {
            var sasPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-15),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15)
            };

            var container = blobClient.GetContainerReference("pdf");
            var blob = container.GetBlockBlobReference(pdfId);

            var sas = blob.GetSharedAccessSignature(sasPolicy);

            return $"{baseBlobUri}pdf/{pdfId}{sas}";
        }
    }
}
