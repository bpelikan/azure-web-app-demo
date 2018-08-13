# Azure-web-app-demo 
This example uses: 
* **Web App**, 
* **SQL database** and 
* [azure-face-api-demo](https://github.com/bpelikan/azure-face-api-demo "azure-face-api-demo")
## Setting up
What you need:
* **Web App** with:
  * `Application settings`:
    * `"blobServiceEndpoint": "{primary_blob_service_endpoint}"`
    * `"blobAccountName": "{storage_account_name}"`
    * `"blobKeyValue": "{blob_access_key}"`
    * `"ConnectionStrings:DefaultConnection": "{connection_string_to_sql_database}"`
    * `"cosmosDbKey": "{azure_cosmos_db_access_key }"`
    * `"cosmosDbUri": "{azure_cosmos_db_uri}"`

* **SQL database**
* **Storage account** with:
  * `Blobs Container` named `images` and `pdf`
* **Azure Cosmos DB** with Collection:
  * `Database id` : `facedb`
  * `Collection Id` : `images`
* **[azure-face-api-demo](https://github.com/bpelikan/azure-face-api-demo "azure-face-api-demo")**
 
