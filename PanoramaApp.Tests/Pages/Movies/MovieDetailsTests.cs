using Xunit;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Pages.Movies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Tests.Pages.Movies
{
    public class MovieDetailsTests
    {
       [Fact]
public async Task OnGetAsync_ValidId_ShouldReturnMovieListDetails()
{
    // Arrange
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase("TestDatabase")
        .Options;

    using var context = new ApplicationDbContext(options);

    // Skapa data för testet
    var movie = new Movie { Id = 1, Title = "Inception" };
    var movieList = new MovieList
    {
        Id = 1,
        Name = "Favorites",
        OwnerId = "test-owner-id", // Lägg till en giltig OwnerId
        Movies = new List<MovieListItem>
        {
            new MovieListItem { Id = 1, Title = "Inception" }
        }
    };

    context.Movies.Add(movie);
    context.MovieLists.Add(movieList);
    await context.SaveChangesAsync();

    var pageModel = new MovieDetailsModel(context);

    // Act
    var result = await pageModel.OnGetAsync(1);

    // Assert
    Assert.NotNull(pageModel.MovieList);
    Assert.Equal("Favorites", pageModel.MovieList.Name);
    Assert.Single(pageModel.MovieList.Movies);
    Assert.Equal("Inception", pageModel.MovieList.Movies.First().Title);
}
    }
}
