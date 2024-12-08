using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Pages.Movies;
using Xunit;

public class ExploreMoviesModelTests
{
    [Fact]
    public async Task OnGetAsync_MoviesSortedByReleaseDateDescending()
    {
        // Arrange (Given)
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ExploreMoviesTestDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        var movie1 = new Movie { Title = "OlderMovie", ReleaseDate = new DateTime(2000,1,1) };
        var movie2 = new Movie { Title = "NewerMovie", ReleaseDate = new DateTime(2020,1,1) };
        context.Movies.AddRange(movie1, movie2);
        await context.SaveChangesAsync();

        var pageModel = new ExploreMoviesModel(context);

        // Act (When)
        await pageModel.OnGetAsync();

        // Assert (Then)
        Assert.Equal(2, pageModel.Movies.Count);
        Assert.Equal("NewerMovie", pageModel.Movies[0].Title);
        Assert.Equal("OlderMovie", pageModel.Movies[1].Title);
    }

    [Fact]
    public async Task OnGetAsync_NoMovies_MoviesIsEmpty()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ExploreMoviesEmptyDb")
            .Options;

        using var context = new ApplicationDbContext(options);
        var pageModel = new ExploreMoviesModel(context);

        await pageModel.OnGetAsync();

        Assert.Empty(pageModel.Movies);
    }
}
