using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Pages.Groups;
using Xunit;

public class VoteFilmsModelTests
{
    [Fact]
    public async Task OnGetAsync_GivenValidGroupId_LoadsGroupAndMovies()
    {
        // Arrange (Given)
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("VoteFilmsTestDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        var group = new Group { Name = "VoteGroup" };
        var movie = new Movie { Title = "VoteMovie", Group = group };
        context.Groups.Add(group);
        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object,null,null,null,null,null,null,null,null);

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
            .Options;

        using var context = new ApplicationDbContext(options);

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object,null,null,null,null,null,null,null,null);

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
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase("VoteForMovieTestDb")
        .Options;

    using var context = new ApplicationDbContext(options);

    var user = new IdentityUser { Id = "user1", UserName = "test@example.com" };
    var movie = new Movie { Title = "Votable Movie" };
    context.Users.Add(user);
    context.Movies.Add(movie);
    await context.SaveChangesAsync();

    var userStore = new Mock<IUserStore<IdentityUser>>();
    var userManager = new Mock<UserManager<IdentityUser>>(userStore.Object, null, null, null, null, null, null, null, null);
    userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

    var pageModel = new VoteFilmsModel(context, userManager.Object);

    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    }, "TestAuth"));
    var httpContext = new DefaultHttpContext { User = claimsPrincipal };
    pageModel.PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
    {
        HttpContext = httpContext
    };

    var result = await pageModel.OnPostVoteAsync(1, movie.Id);

    Assert.IsType<RedirectToPageResult>(result);
    var vote = await context.Votes.FirstOrDefaultAsync(v => v.MovieId == movie.Id && v.UserId == user.Id);
    Assert.NotNull(vote);
}

    [Fact]
    public async Task OnPostVoteAsync_InvalidGroupId_RedirectsToError()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("VoteFilmsInvalidGroupVoteDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object,null,null,null,null,null,null,null,null);

        var pageModel = new VoteFilmsModel(context, userManager.Object);

        pageModel.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "testuser@example.com")
        }, "TestAuth"));

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
            .Options;

        using var context = new ApplicationDbContext(options);

        var group = new Group { Name = "VoteGroup" };
        var movie = new Movie { Title = "VoteMovie", Group = group };
        context.Groups.Add(group);
        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        context.Votes.Add(new Vote { MovieId = movie.Id, GroupId = group.Id, UserId = "user1" });
        context.Votes.Add(new Vote { MovieId = movie.Id, GroupId = group.Id, UserId = "user2" });
        await context.SaveChangesAsync();

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object,null,null,null,null,null,null,null,null);

        var pageModel = new VoteFilmsModel(context, userManager.Object);

        // Act
        var count = await pageModel.GetVotesForMovieAsync(movie.Id);

        // Assert
        Assert.Equal(2, count);
    }
}
