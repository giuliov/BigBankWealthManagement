using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace bbwm
{
    public static class CustomerPortfolioFunctions
    {
        [FunctionName("customerPortfolio")]
        public static async Task<IActionResult> Upsert(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            string customerId,
            string symbol,
            ILogger log)
        {
            var repo = new CustomerPortfolioRepository(log);
            await repo.AddSymbolToCustomerPortfolio(customerId, symbol);

            return new OkObjectResult("ok");
        }

        [FunctionName("customerPortfolioValues")]
        public static async Task<IActionResult> GetValues(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            string customerId,
            ILogger log)
        {
            // TODO validate input

            var portfolioRepo = new CustomerPortfolioRepository(log);
            var portfolio = await portfolioRepo.GetCustomerPortfolioSymbols(customerId);
            log.LogInformation($"Customer {customerId} has '{portfolio}' symbols.");

            var quoteRepo = new QuoteRepository(log);

            var result = new List<StockValue>();

            foreach (var symbol in portfolio)
            {
                var quote = await quoteRepo.GetSymbolQuote(symbol);
                var value = new StockValue
                {
                    Symbol = symbol,
                    Quote = quote.Value,
                    FailureReason = quote.FailureReason
                };
                result.Add(value);
            }

            // TODO failure cases
            return new OkObjectResult(result);
        }
    }
}