using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Cloud_B_Function
{
    public class AzureFunctionRoots
    {
        private readonly ILogger<AzureFunctionRoots> _logger;
        //TEST COMMIT
        public AzureFunctionRoots(ILogger<AzureFunctionRoots> logger)
        {
            _logger = logger;
        }

        [Function("HTTPTest")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
