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
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new ApplicationDbContext(options);

        var user = new IdentityUser { Id = "user2", UserName = "user1@example.com" };
        var movie = new Movie
        {
            Id = 3,
            Title = "favorite Movie testtest",
            Description = "An example movie description",
            Genre = "Action",
            TrailerUrl = "http://example.com/trailer",
            ReleaseDate = DateTime.Now,
            Priority = 4
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var movieList = new MovieList
        {
            Id = 1,
            Name = "Favorites",
            OwnerId = "user1"
        };


        context.MovieLists.Add(movieList);
        // context.Favorites.Add(favorite { MovieId = 1, UserId = user.Id }); Hur ska den vara?
        Assert.NotNull(movieList.OwnerId);
        await context.SaveChangesAsync();

        var movieListItem = new MovieListItem { MovieListId = movieList.Id, Movie = movie };
        context.MovieListItems.Add(movieListItem);
        await context.SaveChangesAsync();

        // Mocka UserManager
        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(user.Id);

        var pageModel = new MyFavoritesModel(context, userManager.Object);

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
    new Claim(ClaimTypes.NameIdentifier, user.Id)
        }, "TestAuth"));

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };


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
            .EnableSensitiveDataLogging()
            .Options;
        using var context = new ApplicationDbContext(options);

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        // Ingen inloggad användare
        var pageModel = new MyFavoritesModel(context, userManager.Object);

        // Act
        var result = await pageModel.OnGetAsync();

        // Assert
        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Account/Login", redirect.PageName);
    }
}
