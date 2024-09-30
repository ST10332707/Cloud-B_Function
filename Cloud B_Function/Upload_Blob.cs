using System;
using Azure.Storage.Blobs;
using System.IO;
using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Cloud_B_Function
{
    public static class Upload_Blob
    {
        [FunctionName("Upload_Blob")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string containerName = req.Query["containerName"];
            string blobName = req.Query["blobName"];


            if (string.IsNullOrEmpty(containerName) || string.IsNullOrEmpty(blobName))
            {
                return new BadRequestObjectResult("Container name and blob name must be provided.");
            }

            var connectionString = Environment.GetEnvironmentVariable("AzureStorage:DefaultEndpointsProtocol=https;AccountName=st10332707storageaccount;AccountKey=3h8DMrwa6hj/lmL3aq0RL8XRR+KwcyQGx4Mc+qhrlvnhDCZusbNgX4ZmbJxRDTuQTJI7zcobpvnj+AStjSrfAg==;EndpointSuffix=core.windows.net");
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(blobName);

            using var stream = req.Body;
            await blobClient.UploadAsync(stream, true);

            return new OkObjectResult("Blob uploaded");
        }
    }
}
