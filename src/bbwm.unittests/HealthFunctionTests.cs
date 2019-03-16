using Functions.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;

namespace bbwm.unittests
{
    public class HealthFunctionTests : FunctionTestHelper.FunctionTest
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void basic_healthcheck_returns_ok()
        {
            var request = TestFactory.CreateHttpRequest();
            var response = (OkObjectResult)await HealthFunction.Run(request, "basic", logger);
            Assert.Equal("healthy", response.Value);
        }
    }
}
