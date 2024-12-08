using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moq;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Pages.Movies;
using Microsoft.AspNetCore.Identity;
using Xunit;
using System.Collections.Generic;

public class AddMovieModelTests
{
    [Fact]
    public async Task OnGetAsync_ValidListId_LoadsMovieListAndAvailableMovies()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("AddMovieGetTestDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        var list = new MovieList { Name = "TestList" };
        context.MovieLists.Add(list);
        await context.SaveChangesAsync();


        var movieInList = new Movie { Title = "InList" };
        var movieOutside = new Movie { Title = "Outside" };
        context.Movies.AddRange(movieInList, movieOutside);
        await context.SaveChangesAsync();

        var movieListItem = new MovieListItem { MovieListId = list.Id, MovieId = movieInList.Id };
        context.MovieListItems.Add(movieListItem);
        await context.SaveChangesAsync();

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object,null,null,null,null,null,null,null,null);

        var pageModel = new AddMovieModel(context, userManager.Object);

        // Act
        var result = await pageModel.OnGetAsync(list.Id);

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.NotNull(pageModel.MovieList);
        Assert.Single(pageModel.MovieList.Movies);
        Assert.Single(pageModel.MovieOptions); // Bara "Outside" 
        Assert.Equal("Outside", pageModel.MovieOptions.First().Text);
    }

    [Fact]
    public async Task OnGetAsync_InvalidListId_RedirectsToError()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("AddMovieGetInvalidTestDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object,null,null,null,null,null,null,null,null);

        var pageModel = new AddMovieModel(context, userManager.Object);

        // Act
        var result = await pageModel.OnGetAsync(999);

        // Assert
        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Error", redirect.PageName);
    }

    [Fact]
    public async Task OnPostAsync_GivenSelectedMovies_AddsToMovieListAndRedirects()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("AddMoviePostTestDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        var list = new MovieList { Name = "TestList" };
        var movie1 = new Movie { Title = "Movie1" };
        var movie2 = new Movie { Title = "Movie2" };
        context.MovieLists.Add(list);
        context.Movies.AddRange(movie1, movie2);
        await context.SaveChangesAsync();

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object,null,null,null,null,null,null,null,null);

        var pageModel = new AddMovieModel(context, userManager.Object)
        {
            SelectedMovieIds = new List<int> { movie1.Id, movie2.Id }
        };

        // Act
        var result = await pageModel.OnPostAsync(list.Id);

        // Assert
        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Movies/MovieListDetails", redirect.PageName);
        Assert.Equal(list.Id, redirect.RouteValues["id"]);

        var updatedList = await context.MovieLists
            .Include(ml => ml.Movies)
            .ThenInclude(mli => mli.Movie)
            .FirstAsync(ml => ml.Id == list.Id);

        Assert.Equal(2, updatedList.Movies.Count);
    }

    [Fact]
    public async Task OnPostAsync_InvalidListId_ReturnsNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("AddMoviePostNotFoundDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object,null,null,null,null,null,null,null,null);

        var pageModel = new AddMovieModel(context, userManager.Object)
        {
            SelectedMovieIds = new List<int> { 1, 2 }
        };

        // Act
        var result = await pageModel.OnPostAsync(999); // Ogiltigt listId

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
