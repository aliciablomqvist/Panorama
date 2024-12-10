using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Services;
using PanoramaApp.Models;
using PanoramaApp.Pages.Movies;
using Xunit;

public class ReviewsModelTests
{
    [Fact]
    public async Task OnGetAsync_ValidMovieId_LoadsMovieAndReviews()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ReviewsGetDb")
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new ApplicationDbContext(options);

                    var movie = new Movie
{
    Title = "ReviewableMovie",
    Description = "An example movie description",
    Genre = "Action",
    TrailerUrl = "http://example.com/trailer",
    ReleaseDate = DateTime.Now,
    Priority = 2
};
        context.Movies.Add(movie);
        var review = new Review { MovieId = movie.Id, Content = "Nice", Rating = 5, UserId = "user1" };
        context.Reviews.Add(review);
        await context.SaveChangesAsync();

        var pageModel = new ReviewsModel(context);
        var result = await pageModel.OnGetAsync(movie.Id);

        Assert.IsType<PageResult>(result);
        Assert.NotNull(pageModel.Movie);
        Assert.Single(pageModel.Reviews);
    }

    [Fact]
    public async Task OnGetAsync_InvalidMovieId_NotFound()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ReviewsGetNotFoundDb")
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new ApplicationDbContext(options);

        var pageModel = new ReviewsModel(context);
        var result = await pageModel.OnGetAsync(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task OnPostAsync_InvalidModel_ReturnsPage()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ReviewsPostInvalidModelDb")
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new ApplicationDbContext(options);

                    var movie = new Movie
{
    Title = "ReviewMoviePost",
    Description = "An example movie description",
    Genre = "Action",
    TrailerUrl = "http://example.com/trailer",
    ReleaseDate = DateTime.Now,
    Priority = 3
};
        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        var pageModel = new ReviewsModel(context)
        {
            Rating = 6, // Detta är en ogiltig rating
            ReviewContent = "Too long"
        };

        pageModel.ModelState.AddModelError("Rating", "Out of range");
        var result = await pageModel.OnPostAsync(movie.Id);

        Assert.IsType<PageResult>(result); 
    }


 [Fact]
public async Task OnPostAsync_LoggedInUser_AddsReviewSuccessfully()
{
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase("ReviewTestDb")
        .EnableSensitiveDataLogging()
        .Options;

    using var context = new ApplicationDbContext(options);

    var user = new IdentityUser { Id = "user8", UserName = "test@example.com" };
                var movie = new Movie
{
    Title = "ReviewableMovie",
    Description = "An example movie description",
    Genre = "Action",
    TrailerUrl = "http://example.com/trailer",
    ReleaseDate = DateTime.Now,
    Priority = 2
};


    context.Users.Add(user);
    context.Movies.Add(movie);
    await context.SaveChangesAsync();

    var pageModel = new ReviewsModel(context)
    {
        ReviewContent = "Great movie!",
        Rating = 5
    };

    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    }, "TestAuth"));
    var httpContext = new DefaultHttpContext { User = claimsPrincipal };
    pageModel.PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
    {
        HttpContext = httpContext
    };

    // Act
    var result = await pageModel.OnPostAsync(movie.Id);

    // Assert
    Assert.IsType<RedirectToPageResult>(result);
    var review = await context.Reviews.FirstOrDefaultAsync(r => r.MovieId == movie.Id);
    Assert.NotNull(review);
    Assert.Equal("Great movie!", review.Content);
    Assert.Equal(5, review.Rating);
}
}
