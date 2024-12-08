using Xunit;
using PanoramaApp.Data;
using PanoramaApp.Pages.Movies;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace PanoramaApp.Tests.UnitTests.Pages.Movies
{
public class AddMovieToListTest
{
    [Fact]
    public async Task AddMovieToList_Success()
    {
        // Arrange
        var context = TestHelpers.GetInMemoryDbContext();
        var userManager = TestHelpers.GetMockUserManager<IdentityUser>();
        var httpContext = TestHelpers.GetMockHttpContext();

        var model = new AddMovieModel(context, userManager.Object, httpContext);

        // Act
        var result = await model.OnPostAsync(movieId: 1);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RedirectToPageResult>(result);
    }
}
}