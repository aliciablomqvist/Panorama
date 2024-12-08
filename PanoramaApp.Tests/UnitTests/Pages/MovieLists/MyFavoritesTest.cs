using System;
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

public class MyFavoritesModelTests
{
    [Fact]
    public async Task OnGetAsync_GivenLoggedInUserWithFavorites_LoadsMyFavorites()
    {
        // Arrange (Given)
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MyFavoritesTestDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        // Skapa användare och favoritlista
        var user = new IdentityUser { Id = "user1", UserName = "user1@example.com" };
        var movie = new Movie { Title = "FavoriteMovie" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var movieList = new MovieList { Name = "My Favorites", OwnerId = user.Id };
        context.MovieLists.Add(movieList);
        await context.SaveChangesAsync();

        var movieListItem = new MovieListItem { MovieListId = movieList.Id, Movie = movie };
        context.MovieListItems.Add(movieListItem);
        await context.SaveChangesAsync();

        // Mocka UserManager
        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        // Setup GetUserId
        userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(user.Id);

        var pageModel = new MyFavoritesModel(context, userManager.Object);
// Skapa en test-användare
var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
    new Claim(ClaimTypes.NameIdentifier, user.Id)
}, "TestAuth"));

// Skapa en HttpContext med användaren
var httpContext = new DefaultHttpContext
{
    User = claimsPrincipal
};

// Tilldela PageContext med HttpContext till din PageModel
pageModel.PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
{
    HttpContext = httpContext
};

        // Act (When)
        var result = await pageModel.OnGetAsync();

        // Assert (Then)
        Assert.IsType<PageResult>(result);
        Assert.NotNull(pageModel.MovieList);
        Assert.Single(pageModel.MovieList.Movies);
        Assert.Equal("FavoriteMovie", pageModel.MovieList.Movies.First().Movie.Title);
    }

    [Fact]
    public async Task OnGetAsync_UserNotLoggedIn_RedirectsToLogin()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MyFavoritesNoUserDb")
            .Options;
        using var context = new ApplicationDbContext(options);

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object,null,null,null,null,null,null,null,null);

        // Ingen inloggad användare
        var pageModel = new MyFavoritesModel(context, userManager.Object);

        // Act
        var result = await pageModel.OnGetAsync();

        // Assert
        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Account/Login", redirect.PageName);
    }
}
