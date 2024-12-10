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

public class MovieCalendarTests
{
[Fact]
public void AddMovieToCalendar_AddsCorrectEntry()
{
    // Arrange
    var movieCalendar = new MovieCalendar();
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

    var date = new DateTime(2024, 12, 1, 20, 0, 0);

    movieCalendar.AddMovie(movie, date);

    // Assert
    Assert.Single(movieCalendar.Entries);
    Assert.Equal("Test Movie", movieCalendar.Entries.First().Movie.Title);
    Assert.Equal(date, movieCalendar.Entries.First().Date);
}

}