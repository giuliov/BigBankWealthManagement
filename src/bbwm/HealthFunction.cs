using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace bbwm
{
    public static class HealthFunction
    {
        [FunctionName("health")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "health/{depth}")] HttpRequest req,
            string depth,
            ILogger log)
        {
            log.LogInformation("health endpoint called.");

            switch (depth.ToLowerInvariant())
            {
                case "dependencies":
                    var portfolioRepo = new CustomerPortfolioRepository(log);
                    await portfolioRepo.HealthCheck();
                    var quoteRepo = new QuoteRepository(log);
                    await quoteRepo.HealthCheck();
                    break;
                case "basic":
                    break;
                default:
                    goto case "basic";
            }

            return (ActionResult)new OkObjectResult($"healthy");
        }
    }
}
