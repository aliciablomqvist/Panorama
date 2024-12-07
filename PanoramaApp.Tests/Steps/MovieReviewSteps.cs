using TechTalk.SpecFlow;
using Xunit;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Pages.Movies;

[Binding]
public class MovieReviewsSteps
{
    private readonly ApplicationDbContext _dbContext;
    private ReviewsModel _reviewsModel;

    public MovieReviewsSteps()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _reviewsModel = new ReviewsModel(_dbContext);
    }

    [Given(@"a movie exists in the database")]
    public async Task GivenAMovieExistsInTheDatabase()
    {
        _dbContext.Movies.Add(new Movie { Id = 1, Title = "Test Movie" });
        await _dbContext.SaveChangesAsync();
    }

    [When(@"I submit a review with content ""(.*)"" and rating (.*)")]
    public async Task WhenISubmitAReviewWithContentAndRating(string content, int rating)
    {
        _reviewsModel.ReviewContent = content;
        _reviewsModel.Rating = rating;

        await _reviewsModel.OnPostAsync(1);
    }

    [Then(@"the review should be saved in the database")]
    public async Task ThenTheReviewShouldBeSavedInTheDatabase()
    {
        var review = await _dbContext.Reviews.FirstOrDefaultAsync();
        Assert.NotNull(review);
    }
}
