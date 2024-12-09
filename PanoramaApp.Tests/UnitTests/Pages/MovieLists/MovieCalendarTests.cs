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
    var movie = new Movie { Title = "Test Movie" };
    var date = new DateTime(2024, 12, 1, 20, 0, 0);

    // Act
    movieCalendar.AddMovie(movie, DateTime.Now);

    // Assert
Assert.Equal(date, movieCalendar.Entries.First().Date);
Assert.Equal("Test Movie", movieCalendar.Entries.First().Movie.Title);

    //Assert.Equal(date, movieCalendar.Entries.First().DateTime);
}
}