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
        public async void Http_trigger_should_return_known_string()
        {
            var request = TestFactory.CreateHttpRequest("not-used", null);
            var response = (OkObjectResult)await HealthFunction.Run(request, "", logger);
            Assert.Equal("healthy", response.Value);
        }
    }
}
