using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Data.Tables;

namespace Cloud_B_Function
{
    public static class Table_Storage
    {
        [FunctionName("Table_Storage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string tableName = req.Query["tableName"];
            string partitionKey = req.Query["partitionKey"];
            string rowKey = req.Query["rowKey"];
            string data = req.Query["data"];

            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(partitionKey) || string.IsNullOrEmpty(rowKey) || string.IsNullOrEmpty(data))
            {
                return new BadRequestObjectResult("Table name, partition key, row key, and data must be provided.");
            }

            var connectionString = Environment.GetEnvironmentVariable("AzureStorage:DefaultEndpointsProtocol=https;AccountName=st10332707storageaccount;AccountKey=3h8DMrwa6hj/lmL3aq0RL8XRR+KwcyQGx4Mc+qhrlvnhDCZusbNgX4ZmbJxRDTuQTJI7zcobpvnj+AStjSrfAg==;EndpointSuffix=core.windows.net");
            var serviceClient = new TableServiceClient(connectionString);
            var tableClient = serviceClient.GetTableClient(tableName);
            await tableClient.CreateIfNotExistsAsync();

            var entity = new TableEntity(partitionKey, rowKey) { ["Data"] = data };
            await tableClient.AddEntityAsync(entity);

            return new OkObjectResult("Data added to table storage");
        }
    }
}