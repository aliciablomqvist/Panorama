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

public class MovieCalendarTests
{
    [Fact]
public void AddMovieToCalendar_AddsCorrectEntry()
{
    // Arrange
    var calendar = new MovieCalendar();
    var movie = new Movie { Title = "Test Movie" };
    var date = new DateTime(2023, 12, 1, 20, 0, 0);

    // Act
    calendar.AddMovie(movie, date);

    // Assert
    Assert.Single(calendar.Entries);
    Assert.Equal("Test Movie", calendar.Entries.First().Movie.Title);
    Assert.Equal(date, calendar.Entries.First().DateTime);
}
}