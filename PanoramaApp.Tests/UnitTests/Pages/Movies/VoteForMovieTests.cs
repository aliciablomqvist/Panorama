using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PanoramaApp.Services;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Pages.Groups;
using PanoramaApp.Tests.Helpers;
using Xunit;

public class VoteFilmsModelTests
{
    [Fact]
    public async Task OnGetAsync_GivenValidGroupId_LoadsGroupAndMovies()
    {
        // Arrange (Given)
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("VoteFilmsTestDb")
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new ApplicationDbContext(options);

        var group = new Group
        {
            Id = 4,
            Name = "Voting group",
            OwnerId = "user1"
        };

        var movie = new Movie
        {
            Id = 1,
            Title = "Test Movie",
            GroupId = group.Id, // Matcha med gruppens Id
            Description = "This is a description.",
            Genre = "Action",
            TrailerUrl = "http://example.com/trailer",
            ReleaseDate = DateTime.Now,
            Priority = 1
        };


        context.Groups.Add(group);
        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        var pageModel = new VoteFilmsModel(context, userManager.Object);

        // Act (When)
        var result = await pageModel.OnGetAsync(group.Id);

        // Assert (Then)
        Assert.IsType<PageResult>(result);
        Assert.NotNull(pageModel.Group);
        Assert.Single(pageModel.Group.Movies);
    }

    [Fact]
    public async Task OnGetAsync_InvalidGroupId_RedirectsToError()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("VoteFilmsInvalidGroupDb")
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new ApplicationDbContext(options);

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        var pageModel = new VoteFilmsModel(context, userManager.Object);

        // Act
        var result = await pageModel.OnGetAsync(999);

        // Assert
        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Error", redirect.PageName);
    }

    [Fact]
    public async Task OnPostVoteAsync_ValidVote_AddsVote()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("VoteForMovieTestDb")
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new ApplicationDbContext(options);

        var user1 = new IdentityUser { Id = "user1", UserName = "test@example.com" };
        var movie = new Movie
        {
            Id = 1,
            Title = "Test Movie",
            Description = "This is a description.",
            Genre = "Action",
            TrailerUrl = "http://example.com/trailer",
            ReleaseDate = DateTime.Now,
            Priority = 1
        };


        context.Users.Add(user1);
        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(userStore.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user1);

        var pageModel = new VoteFilmsModel(context, userManager.Object);

        // Använd hjälpmetoden
        TestHelper.SetUserAndHttpContext(pageModel, user1.Id, user1.UserName);

        // Act
        var result = await pageModel.OnPostVoteAsync(1, movie.Id);

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
        var vote = await context.Votes.FirstOrDefaultAsync(v => v.MovieId == movie.Id && v.UserId == user1.Id);
        Assert.NotNull(vote);
    }

    [Fact]
    public async Task OnPostVoteAsync_InvalidGroupId_RedirectsToError()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("VoteFilmsInvalidGroupVoteDb")
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new ApplicationDbContext(options);

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        var pageModel = new VoteFilmsModel(context, userManager.Object);

        TestHelper.SetUserAndHttpContext(pageModel, "user1", "test@example.com");

        // Act
        var result = await pageModel.OnPostVoteAsync(999, 1); // ogiltig grupp

        // Assert
        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Error", redirect.PageName);
    }

    [Fact]
    public async Task GetVotesForMovieAsync_ReturnsCorrectCount()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("VoteFilmsCountDb")
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new ApplicationDbContext(options);

        var group = new Group
        {
            Id = 5,
            Name = "Voting group",
            OwnerId = "user1"
        };

        var movie = new Movie
        {
            Id = 1,
            Title = "Test Movie",
            Description = "This is a description.",
            Genre = "Action",
            Group = group,
            TrailerUrl = "http://example.com/trailer",
            ReleaseDate = DateTime.Now,
            Priority = 1
        };
        context.Groups.Add(group);
        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        context.Votes.Add(new Vote { MovieId = movie.Id, GroupId = group.Id, UserId = "user1" });
        context.Votes.Add(new Vote { MovieId = movie.Id, GroupId = group.Id, UserId = "user2" });
        await context.SaveChangesAsync();

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        var pageModel = new VoteFilmsModel(context, userManager.Object);

        // Act
        var count = await pageModel.GetVotesForMovieAsync(movie.Id);

        // Assert
        Assert.Equal(2, count);
    }
}
