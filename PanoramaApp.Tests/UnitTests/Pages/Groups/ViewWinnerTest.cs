using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Pages.Groups;
using Xunit;

public class ViewWinnerModelTests
{
    [Fact]
    public async Task OnGetAsync_GivenGroupWithMoviesAndVotes_SetsWinningMovie()
    {
        // Arrange (Given)
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ViewWinnerTestDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        var group = new Group { Name = "VotingGroup" };
        var movie1 = new Movie { Title = "Movie1", Group = group };
        var movie2 = new Movie { Title = "Movie2", Group = group };
        context.Groups.Add(group);
        context.Movies.AddRange(movie1, movie2);
        await context.SaveChangesAsync();

        var votes = new List<Vote>
        {
            new Vote { GroupId = group.Id, MovieId = movie1.Id, UserId = "userA" },
            new Vote { GroupId = group.Id, MovieId = movie1.Id, UserId = "userB" },
            new Vote { GroupId = group.Id, MovieId = movie2.Id, UserId = "userC" }
        };
        context.Votes.AddRange(votes);
        await context.SaveChangesAsync();

        var pageModel = new ViewWinnerModel(context);

        // Act (When)
        await pageModel.OnGetAsync(group.Id);

        // Assert (Then)
        Assert.NotNull(pageModel.Group);
        Assert.NotNull(pageModel.WinningMovie);
        Assert.Equal("Movie1", pageModel.WinningMovie.Title);
        Assert.Equal(2, pageModel.WinningMovieVoteCount);
    }

    [Fact]
    public async Task OnGetAsync_GroupNotFoundOrNoMovies_WinningMovieIsNull()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ViewWinnerNoGroupTestDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        // Skapa en grupp utan filmer
        var emptyGroup = new Group { Name = "EmptyGroup" };
        context.Groups.Add(emptyGroup);
        await context.SaveChangesAsync();

        var pageModel = new ViewWinnerModel(context);

        // Scenario A: Ogiltigt grupp-id
        await pageModel.OnGetAsync(999);
        Assert.Null(pageModel.Group);
        Assert.Null(pageModel.WinningMovie);
        Assert.Equal(0, pageModel.WinningMovieVoteCount);

        // Scenario B: Grupp utan filmer
        await pageModel.OnGetAsync(emptyGroup.Id);
        Assert.NotNull(pageModel.Group);
        Assert.Empty(pageModel.Group.Movies);
        Assert.Null(pageModel.WinningMovie);
        Assert.Equal(0, pageModel.WinningMovieVoteCount);
    }
}
