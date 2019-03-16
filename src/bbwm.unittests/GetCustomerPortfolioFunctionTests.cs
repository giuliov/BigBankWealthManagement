using Functions.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NSubstitute;
using RichardSzalay.MockHttp;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace bbwm.unittests
{
    public class GetCustomerPortfolioFunctionTests : FunctionTestHelper.FunctionTest
    {
        private readonly ILogger logger = TestFactory.CreateLogger();
        private string jsonResultMSFT = @"
{
    ""Global Quote"": {
        ""01. symbol"": ""MSFT"",
        ""02. open"": ""112.8900"",
        ""03. high"": ""113.0200"",
        ""04. low"": ""111.6650"",
        ""05. price"": ""112.5300"",
        ""06. volume"": ""23501169"",
        ""07. latest trading day"": ""2019-03-01"",
        ""08. previous close"": ""112.0300"",
        ""09. change"": ""0.5000"",
        ""10. change percent"": ""0.4463%""
    }
}
";
        private string jsonResultAAPL = @"
{
    ""Global Quote"": {
        ""01. symbol"": ""AAPL"",
        ""02. open"": ""112.8900"",
        ""03. high"": ""113.0200"",
        ""04. low"": ""111.6650"",
        ""05. price"": ""112.5300"",
        ""06. volume"": ""23501169"",
        ""07. latest trading day"": ""2019-03-01"",
        ""08. previous close"": ""112.0300"",
        ""09. change"": ""0.5000"",
        ""10. change percent"": ""0.4463%""
    }
}
";

        [Fact]
        public async void Should_return_two_tickers()
        {
            var mockDocClient = Substitute.For<IDocumentClientMockable>();
            mockDocClient
                .ReadDocumentAsync<CustomerPortfolio>(
                    UriFactory.CreateDocumentUri("BBMW", "customerPortfolio", "42"))
                .Returns(
                    new DocumentResponse<CustomerPortfolio>(
                        new CustomerPortfolio
                        {
                            CustomerId = "42",
                            Symbols = new List<string> { "MSFT", "AAPL" }
                        }
                    )
                );
            CustomerPortfolioRepository.docClient = mockDocClient;

            // God bless Richard Szalay for https://github.com/richardszalay/mockhttp
            var mockHttpMessageHandler = new MockHttpMessageHandler();
            mockHttpMessageHandler
                .When("https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=MSFT&apikey=&datatype=json")
                .Respond("application/json", jsonResultMSFT);
            mockHttpMessageHandler
                .When("https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=AAPL&apikey=&datatype=json")
                .Respond("application/json", jsonResultAAPL);
            QuoteRepository.httpClient = new HttpClient(mockHttpMessageHandler);


            var response = (OkObjectResult)await CustomerPortfolioFunctions.GetValues(TestFactory.CreateHttpRequest(), "42", logger);


            var trex = response.Value as List<StockValue>;
            Assert.NotNull(trex);
            Assert.Equal(2, trex.Count);
            Assert.Equal(112.53m, trex[0].Quote);
            Assert.Equal("AAPL", trex[1].Symbol);
        }


        [Fact]
        public void Parsing_example()
        {
            JToken token = JToken.Parse(jsonResultMSFT);
            decimal price = token["Global Quote"]["05. price"].Value<decimal>();

            Assert.Equal(112.5300m, price);
        }
    }
}
