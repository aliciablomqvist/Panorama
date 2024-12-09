using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PanoramaApp.Data;
using PanoramaApp.Services;
using PanoramaApp.Models;
using PanoramaApp.Pages.MovieLists;
using Xunit;

public class WatchTrailerTests
{
    [Fact]
public void PlayTrailer_FetchesCorrectTrailerUrl()
{
    // Arrange
    var movie = new Movie { Title = "Test Movie", TrailerUrl = "http://trailer.com/testmovie" };
    var trailerPlayer = new TrailerPlayer();

    // Act
    var url = trailerPlayer.GetTrailerUrl(movie);

    // Assert
    Assert.Equal("http://trailer.com/testmovie", url);
}
}