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
public class PrioritizeMoviesTests
{
    [Fact]
public void PrioritizeMovies_ReturnsMoviesOrderedByPriority()
{
    // Arrange
    var movies = new List<Movie>
    {
        new Movie { Title = "Movie A", Priority = 2 },
        new Movie { Title = "Movie B", Priority = 1 }
    };

    var prioritizer = new MoviePrioritizer();

    // Act
    var prioritizedMovies = prioritizer.Prioritize(movies);

    // Assert
    Assert.Equal("Movie B", prioritizedMovies.First().Title);
    Assert.Equal("Movie A", prioritizedMovies.Last().Title);
}
}