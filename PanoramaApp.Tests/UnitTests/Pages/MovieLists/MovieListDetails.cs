using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Pages.Movies;
using Xunit;

public class MovieListDetailsModelTests
{
[Fact]
public async Task OnGetAsync_ValidMovie_LoadsDetails()
{
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase("MovieDetailsTestDb")
        .Options;

    using var context = new ApplicationDbContext(options);

    var user = new IdentityUser { Id = "user1", UserName = "test@example.com" };
    context.Users.Add(user);
    var movie = new Movie { Title = "Test Movie" };
    context.Movies.Add(movie);
    await context.SaveChangesAsync();

    var userStore = new Mock<IUserStore<IdentityUser>>();
    var userManager = new Mock<UserManager<IdentityUser>>(userStore.Object, null, null, null, null, null, null, null, null);
    userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

    var pageModel = new MovieDetailsModel(context, userManager.Object, null);

    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    }, "TestAuth"));
    var httpContext = new DefaultHttpContext { User = claimsPrincipal };
    pageModel.PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
    {
        HttpContext = httpContext
    };

    var result = await pageModel.OnGetAsync(movie.Id);

    Assert.IsType<PageResult>(result);
    Assert.NotNull(pageModel.Movie);
    Assert.Equal("Test Movie", pageModel.Movie.Title);
}

    [Fact]
    public async Task OnGetAsync_InvalidId_RedirectsToError()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MovieListDetailsErrorDb")
            .Options;

        using var context = new ApplicationDbContext(options);
        var logger = new Mock<ILogger<MovieListDetailsModel>>();
        var pageModel = new MovieListDetailsModel(context, logger.Object);

        var result = await pageModel.OnGetAsync(999);

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Error", redirect.PageName);
    }
}
