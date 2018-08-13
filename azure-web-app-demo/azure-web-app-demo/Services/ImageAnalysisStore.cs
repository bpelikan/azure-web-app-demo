using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azure_web_app_demo.Services
{
    public class ImageAnalysisStore
    {
        private readonly IConfiguration configuration;
        private DocumentClient client;
        private Uri imageAnalysisLink;

        public ImageAnalysisStore(IConfiguration configuration)
        {
            this.configuration = configuration;

            var uri = new Uri(configuration["cosmosDbUrl"]);
            var key = configuration["cosmosDbKey"];
            client = new DocumentClient(uri, key);
            imageAnalysisLink = UriFactory.CreateDocumentCollectionUri("facedb", "images");
        }

        public dynamic GetImageAnalysis(string imageId)
        {
            var spec = new SqlQuerySpec();
            spec.QueryText = "SELECT * FROM c WHERE (c.ImageId = @imageid)";
            spec.Parameters.Add(new SqlParameter("@imageid", imageId));
            var result = client.CreateDocumentQuery(imageAnalysisLink, spec).ToList();
            return result;
        }
    }
}
