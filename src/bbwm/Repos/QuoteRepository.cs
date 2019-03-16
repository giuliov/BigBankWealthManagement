using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace bbwm
{
    public class QuoteRepository
    {
        // see https://github.com/Azure/azure-functions-host/wiki/Managing-Connections
        // and https://github.com/MicrosoftDocs/azure-docs/issues/14283#issuecomment-435995424
        public static HttpClient httpClient = new HttpClient();

        private static readonly string apikey = Environment.GetEnvironmentVariable("Bbwm_AlphAvantage_ApiKey", EnvironmentVariableTarget.Process);

        private ILogger log;

        public QuoteRepository(
            ILogger log)
        {
            this.log = log;
        }

        public async Task<bool> HealthCheck()
        {
            string url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=MSFT&apikey={apikey}&datatype=json";
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            return response.IsSuccessStatusCode;
        }

        public class Quote
        {
            public decimal Value { get; set; }
            public string FailureReason { get; set; }
        }

        public async Task<Quote> GetSymbolQuote(string symbol)
        {
            string url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={apikey}&datatype=json";
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            try
            {
                var response = await httpClient.GetAsync(url);
                var jsonResult = await response.Content.ReadAsStringAsync();
                JToken token = JToken.Parse(jsonResult);
                if (token["Global Quote"].HasValues)
                {
                    decimal symbolPrice = token["Global Quote"]["05. price"].Value<decimal>();
                    var result = new Quote
                    {
                        Value = symbolPrice,
                        FailureReason = string.Empty
                    };
                    return result;
                }
                else
                {
                    var result = new Quote
                    {
                        Value = default(decimal),
                        FailureReason = $"Unknown symbol '{symbol}'"
                    };
                    return result;
                }
            }
            catch (Exception ex)
            {
                log.LogError($"Cannot connect to Alpha Avantage service: {ex.Message}");

                var result = new Quote
                {
                    Value = default(decimal),
                    FailureReason = $"Cannot connect to Alpha Avantage service"
                };
                return result;
            }
        }
    }
}
