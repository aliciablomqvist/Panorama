using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Pages.MovieLists;
using Xunit;

public class WatchedModelTests
{
  [Fact]
public async Task OnGetAsync_UserWithWatchedMovies_LoadsCorrectMovies()
{
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase("WatchedMoviesTestDb")
        .EnableSensitiveDataLogging()
        .Options;

    using var context = new ApplicationDbContext(options);

    var user = new IdentityUser { Id = "user3", UserName = "test@example.com" };
    context.Users.Add(user);
    await context.SaveChangesAsync();

                   var movie = new Movie
        {
    Title = "watched Movie ",
    Description = "An example movie description",
    Genre = "Action",
    TrailerUrl = "http://example.com/trailer",
    ReleaseDate = DateTime.Now,
    Priority = 4
};
    var watchedList = new MovieList { Name = "Watched", OwnerId = user.Id };
    context.Movies.Add(movie);
    context.MovieLists.Add(watchedList);
    await context.SaveChangesAsync();

    var movieListItem = new MovieListItem { MovieListId = watchedList.Id, MovieId = movie.Id };
    context.MovieListItems.Add(movieListItem);
    await context.SaveChangesAsync();

    var userStore = new Mock<IUserStore<IdentityUser>>();
    var userManager = new Mock<UserManager<IdentityUser>>(userStore.Object, null, null, null, null, null, null, null, null);
    userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

    var pageModel = new WatchedModel(context, userManager.Object);

    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    }, "TestAuth"));
    var httpContext = new DefaultHttpContext { User = claimsPrincipal };
    pageModel.PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
    {
        HttpContext = httpContext
    };

    // Act
    var result = await pageModel.OnGetAsync();

    // Assert
    Assert.IsType<PageResult>(result);
    Assert.NotNull(pageModel.WatchedMovies);
    Assert.Single(pageModel.WatchedMovies);
    Assert.Equal("WatchedMovie", pageModel.WatchedMovies[0].Title);
}

    [Fact]
    public async Task OnGetAsync_UserNotLoggedIn_RedirectsToLogin()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("WatchedNoUserDb")
            .EnableSensitiveDataLogging()
            .Options;
        
        using var context = new ApplicationDbContext(options);

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object,null,null,null,null,null,null,null,null);
        
        // Ingen user
        userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((IdentityUser)null);

        var pageModel = new WatchedModel(context, userManager.Object);

        // Act
        var result = await pageModel.OnGetAsync();

        // Assert
        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Account/Login", redirect.PageName);
    }
}
