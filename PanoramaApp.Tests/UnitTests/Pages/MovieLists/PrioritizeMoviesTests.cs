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
using PanoramaApp.Services;
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

        // Act
        var prioritizedMovies = MoviePrioritizer.GetPrioritizedMovies(movies);

        // Assert
        Assert.Equal("Movie A", prioritizedMovies.First().Title);
        Assert.Equal("Movie B", prioritizedMovies.Last().Title);
    }
}