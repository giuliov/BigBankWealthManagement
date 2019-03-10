using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace bbwm
{
    class CustomerPortfolioRepository
    {
        private static readonly string endpointUri = Environment.GetEnvironmentVariable("Bbwm_CosmosDb_EndpointUri", EnvironmentVariableTarget.Process);
        private static readonly string primaryKey = Environment.GetEnvironmentVariable("Bbwm_CosmosDb_PrimaryKey", EnvironmentVariableTarget.Process);
        private static readonly DocumentClient docClient = new DocumentClient(new Uri(endpointUri), primaryKey);
        private const string DatabaseName = "BBMW";
        private const string CollectionName = "customerPortfolio";

        private ILogger log;

        public CustomerPortfolioRepository(
            ILogger log)
        {
            this.log = log;
        }

        public async Task HealthCheck()
        {
            await docClient.OpenAsync();
        }

        public async Task AddSymbolToCustomerPortfolio(
            string customerId,
            string symbol)
        {
            await docClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseName), new DocumentCollection { Id = CollectionName });

            try
            {
                var documentUri = UriFactory.CreateDocumentUri(DatabaseName, CollectionName, customerId);
                var response = await docClient.ReadDocumentAsync<CustomerPortfolio>(documentUri);
                var portfolio = response.Document;
                if (!portfolio.Symbols.Contains(symbol))
                {
                    portfolio.Symbols.Add(symbol);
                }
                await docClient.ReplaceDocumentAsync(documentUri, portfolio);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    var portfolio = new CustomerPortfolio { CustomerId = customerId, Symbols = new List<string> { symbol } };
                    await docClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), portfolio);
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<string>> GetCustomerPortfolioSymbols(string customerId)
        {
            if (String.IsNullOrEmpty(customerId))
                throw new ArgumentException(nameof(customerId));

            log.LogInformation($"Getting tickers form customer {customerId}.");

            var documentUri = UriFactory.CreateDocumentUri(DatabaseName, CollectionName, customerId);
            var response = await docClient.ReadDocumentAsync<CustomerPortfolio>(documentUri);
            var portfolio = response.Document;

            return portfolio.Symbols;
        }
    }
}
