using System.Net;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PanoramaApp.Tests.IntegrationTests
{
    public class EndPointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;

        public EndPointTests(WebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateDefaultClient();
        }

        [Theory]
        [InlineData("/Movies/ExploreMovies")] 
        [InlineData("/Groups/ViewWinner")]  
        [InlineData("/Identity/Account/Login")] 
        [InlineData("/MovieLists/MyFavorites")] 
        public async Task Endpoints_Exist_ShouldReturnSuccess(string url)
        {
            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK); // FluentAssertions
            response.Content.Headers.ContentType.ToString().Should().Contain("text/html");
        }
    }
}
