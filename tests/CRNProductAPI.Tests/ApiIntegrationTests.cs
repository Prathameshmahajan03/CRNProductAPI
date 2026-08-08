using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace CRNProductAPI.Tests
{
    public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<API.Program>>
    {
        private readonly HttpClient _client;

        public ApiIntegrationTests(WebApplicationFactory<API.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Swagger_ShouldReturnSuccess()
        {
            var response = await _client.GetAsync("/swagger/index.html");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}