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
using PanoramaApp.Pages.Movies;
using PanoramaApp.Tests.Helpers;
using PanoramaApp.Services;
using Xunit;
using System.Collections.Generic;

public class MovieDetailsModelTests
{
[Fact]
public async Task OnGetAsync_ValidMovie_LoadsDetails()
{
    // Arrange
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase("MovieDetailsTestDb")
        .Options;

    using var context = new ApplicationDbContext(options);

    var user = new IdentityUser { Id = "user1", UserName = "test@example.com" };
    var movie = new Movie { Title = "Test Movie" };
    context.Users.Add(user);
    context.Movies.Add(movie);
    await context.SaveChangesAsync();

    var userStore = new Mock<IUserStore<IdentityUser>>();
    var userManager = new Mock<UserManager<IdentityUser>>(userStore.Object, null, null, null, null, null, null, null, null);
    userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

    var pageModel = new MovieDetailsModel(context, userManager.Object, null);

  TestHelper.SetUserAndHttpContext(pageModel, user.Id, user.UserName);


    // Act
    var result = await pageModel.OnGetAsync(movie.Id);

    // Assert
    Assert.IsType<PageResult>(result);
    Assert.NotNull(pageModel.Movie);
    Assert.Equal("Test Movie", pageModel.Movie.Title);
}


    [Fact]
    public async Task OnGetAsync_InvalidMovieId_ReturnsNotFound()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MovieDetailsNotFoundDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        var reviewService = new Mock<ReviewService>(null);
        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(userStore.Object,null,null,null,null,null,null,null,null);

        var pageModel = new MovieDetailsModel(context, userManager.Object, reviewService.Object);

        var result = await pageModel.OnGetAsync(999);

        Assert.IsType<NotFoundResult>(result);
    }
 
[Fact]
public async Task OnPostAddToFavoritesAsync_LoggedInUser_AddsMovieToFavorites()
{
    // Arrange
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase("AddToFavoritesDb")
        .Options;

    using var context = new ApplicationDbContext(options);

    var user = new IdentityUser { Id = "user1", UserName = "test@example.com" };
    var movie = new Movie { Title = "FavMovie" };
    context.Users.Add(user);
    context.Movies.Add(movie);
    await context.SaveChangesAsync();

    var userStore = new Mock<IUserStore<IdentityUser>>();
    var userManager = new Mock<UserManager<IdentityUser>>(userStore.Object, null, null, null, null, null, null, null, null);
    userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

    var pageModel = new MovieDetailsModel(context, userManager.Object, null);

   TestHelper.SetUserAndHttpContext(pageModel, user.Id, user.UserName);


    // Act
    var result = await pageModel.OnPostAddToFavoritesAsync(movie.Id);

    // Assert
    Assert.IsType<RedirectToPageResult>(result);
    var favoritesList = await context.MovieLists
        .Include(ml => ml.Movies)
        .FirstOrDefaultAsync(ml => ml.OwnerId == user.Id && ml.Name == "My Favorites");
    Assert.NotNull(favoritesList);
    Assert.Single(favoritesList.Movies);
    Assert.Equal(movie.Id, favoritesList.Movies.First().MovieId);
}

    [Fact]
    public async Task OnPostAddToFavoritesAsync_NotLoggedIn_RedirectsToLogin()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("AddToFavNoUserDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        var movie = new Movie { Title = "NoUserMovie" };
        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(userStore.Object,null,null,null,null,null,null,null,null);
        userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((IdentityUser)null);

        var reviewService = new Mock<ReviewService>(null);
        var pageModel = new MovieDetailsModel(context, userManager.Object, reviewService.Object);

        var result = await pageModel.OnPostAddToFavoritesAsync(movie.Id);

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Account/Login", redirect.PageName);
    }

[Fact]
public async Task OnPostMarkAsWatchedAsync_LoggedInUser_AddsMovieToWatched()
{
    // Arrange
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase("MarkAsWatchedDb")
        .Options;

    using var context = new ApplicationDbContext(options);

    var user = new IdentityUser { Id = "userId", UserName = "test@example.com" };
    var movie = new Movie { Title = "WatchedMovie" };
    context.Users.Add(user);
    context.Movies.Add(movie);
    await context.SaveChangesAsync();

    var userStore = new Mock<IUserStore<IdentityUser>>();
    var userManager = new Mock<UserManager<IdentityUser>>(userStore.Object, null, null, null, null, null, null, null, null);
    userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

    var pageModel = new MovieDetailsModel(context, userManager.Object, null);

    TestHelper.SetUserAndHttpContext(pageModel, user.Id, user.UserName);

    // Act
    var result = await pageModel.OnPostMarkAsWatchedAsync(movie.Id);

    // Assert
    Assert.IsType<RedirectToPageResult>(result);
    var watchedList = await context.MovieLists
        .Include(ml => ml.Movies)
        .FirstOrDefaultAsync(ml => ml.OwnerId == user.Id && ml.Name == "Watched");
    Assert.NotNull(watchedList);
    Assert.Single(watchedList.Movies);
    Assert.Equal(movie.Id, watchedList.Movies.First().MovieId);
}
[Fact]
public async Task OnPostAddReviewAsync_LoggedInUser_AddsReview()
{
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase("AddReviewDb")
        .Options;

    using var context = new ApplicationDbContext(options);

    var user = new IdentityUser { Id = "user3", UserName = "testuser@example.com" };
    var movie = new Movie { Title = "Reviewable Movie" };
    context.Users.Add(user);
    context.Movies.Add(movie);
    await context.SaveChangesAsync();

    var userStore = new Mock<IUserStore<IdentityUser>>();
    var userManager = new Mock<UserManager<IdentityUser>>(userStore.Object, null, null, null, null, null, null, null, null);
    userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

    var reviewService = new Mock<ReviewService>(null);
    reviewService.Setup(rs => rs.AddReviewAsync(movie.Id, user.Id, "Great movie", 5))
                 .Returns(Task.CompletedTask);

    var pageModel = new MovieDetailsModel(context, userManager.Object, reviewService.Object);

    TestHelper.SetUserAndHttpContext(pageModel, user.Id, user.UserName);

    var result = await pageModel.OnPostAddReviewAsync(movie.Id, "Great movie", 5);

    Assert.IsType<RedirectToPageResult>(result);
    reviewService.Verify(rs => rs.AddReviewAsync(movie.Id, user.Id, "Great movie", 5), Times.Once);
}


    [Fact]
    public async Task OnPostAddReviewAsync_NotLoggedIn_RedirectsToLogin()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("AddReviewNoUserDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        var movie = new Movie { Title = "ReviewNoUserMovie" };
        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        var reviewService = new Mock<ReviewService>(null);
        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(userStore.Object,null,null,null,null,null,null,null,null);

        var pageModel = new MovieDetailsModel(context, userManager.Object, reviewService.Object);

        var result = await pageModel.OnPostAddReviewAsync(movie.Id, "Nice", 5);

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Account/Login", redirect.PageName);
    }
}
