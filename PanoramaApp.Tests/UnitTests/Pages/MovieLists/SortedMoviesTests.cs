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

public class SortedMoviesTests
{
    [Fact]
public void SortMovies_ByTitle_ReturnsSortedMovies()
{
    // Arrange
    var movies = new List<Movie>
    {
        new Movie { Title = "B Movie", ReleaseDate = new DateTime(2020, 1, 1) },
        new Movie { Title = "A Movie", ReleaseDate = new DateTime(2019, 1, 1) }
    };

    var sorter = new MovieSorter();

    // Act
    var sortedMovies = sorter.Sort(movies, "title");

    // Assert
    Assert.Equal("A Movie", sortedMovies.First().Title);
    Assert.Equal("B Movie", sortedMovies.Last().Title);
}
}
