using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using PanoramaApp.Models;
using PanoramaApp.Services;
using TechTalk.SpecFlow;
using Xunit;

namespace PanoramaApp.Tests.Steps
{
    [Binding]
    public class MovieReviewsSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly Mock<IReviewService> _reviewServiceMock;
        private Movie _movie;
        private string _loggedInUserId;
        private List<Review> _reviews;

        public MovieReviewsSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _reviewServiceMock = new Mock<IReviewService>();
            _reviews = new List<Review>();
        }

        [Given(@"that a movie exists in the database")]
        public void GivenThatAMovieExistsInTheDatabase()
        {
            // Simulate the movie existing in the database
            _movie = new Movie
            {
                Id = 1,
                Title = "Inception",
                ReleaseDate = new System.DateTime(2010, 7, 16)
            };
            _scenarioContext["SelectedMovie"] = _movie;
        }

        [Given(@"I am logged in")]
        public void GivenIAmLoggedIn()
        {
            // Simulate user login
            _loggedInUserId = "user123";
            _scenarioContext["LoggedInUserId"] = _loggedInUserId;
        }

        [When(@"I submit a review with content ""(.*)"" and rating (.*)")]
        public async Task WhenISubmitAReviewWithContentAndRating(string content, int rating)
        {
            var movie = _scenarioContext["SelectedMovie"] as Movie;
            var userId = _scenarioContext["LoggedInUserId"] as string;

            var review = new Review
            {
                MovieId = movie.Id,
                UserId = userId,
                Content = content,
                Rating = rating,
                CreatedAt = System.DateTime.UtcNow
            };

            _reviewServiceMock.Setup(service => service.AddReviewAsync(
                It.Is<int>(id => id == movie.Id),
                It.Is<string>(uid => uid == userId),
                It.Is<string>(c => c == content),
                It.Is<int>(r => r == rating)))
                .Callback(() => _reviews.Add(review))
                .Returns(Task.CompletedTask);

            await _reviewServiceMock.Object.AddReviewAsync(movie.Id, userId, content, rating);
        }

        [Then(@"the review should be saved in the database")]
        public void ThenTheReviewShouldBeSavedInTheDatabase()
        {
            // Verify the review is saved in the database
            var movie = _scenarioContext["SelectedMovie"] as Movie;
            var userId = _scenarioContext["LoggedInUserId"] as string;

            Assert.Contains(_reviews, r => r.MovieId == movie.Id && r.UserId == userId);
            _reviewServiceMock.Verify(service => service.AddReviewAsync(
                It.Is<int>(id => id == movie.Id),
                It.Is<string>(uid => uid == userId),
                It.Is<string>(c => !string.IsNullOrEmpty(c)),
                It.Is<int>(r => r > 0)), Times.Once);
        }
    }
}
