using Functions.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Xunit;

namespace bbwm.unittests
{
    public class GetCustomerPortfolioFunctionTests : FunctionTestHelper.FunctionTest
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void Should_return_two_tickers()
        {
            /*
            var request = TestFactory.CreateHttpRequest("customerId", "42");
            var response = (OkObjectResult)await GetCustomerPortfolioFunction.Run(request, logger);
            Assert.NotNull(response.Value);
            */
        }


        [Fact]
        public void Parsing_example()
        {
            string jsonResult = @"
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
            JToken token = JToken.Parse(jsonResult);
            decimal price = token["Global Quote"]["05. price"].Value<decimal>();

            Assert.Equal(112.5300m, price);
        }
    }
}
