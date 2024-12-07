using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Services;
using Xunit;

namespace PanoramaApp.Tests
{
    public class ReviewTests
    {
        private readonly ApplicationDbContext _context;

        public ReviewTests()
        {
        
            _context = TestHelpers.GetInMemoryDbContext();
        }

        [Fact]
        public async Task AddReview_ShouldSaveReviewToDatabase()
        {
         
            var userId = "test-user-id";
            var movie = new Movie { Id = 1, Title = "Test Movie" };
            var user = new IdentityUser { Id = userId, UserName = "testuser" };
            _context.Users.Add(user);
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            var reviewService = new ReviewService(_context);

            await reviewService.AddReviewAsync(movie.Id, userId, "Great movie!", 5);


            var savedReview = await _context.Reviews.FirstOrDefaultAsync(r => r.MovieId == movie.Id);
            Assert.NotNull(savedReview);
            Assert.Equal("Great movie!", savedReview.Content);
            Assert.Equal(5, savedReview.Rating);
            Assert.Equal(userId, savedReview.UserId);
        }

        [Fact]
        public async Task AddReview_ShouldFailForNonExistentMovie()
        {
    
            var userId = "test-user-id";
            var user = new IdentityUser { Id = userId, UserName = "testuser" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var reviewService = new ReviewService(_context);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await reviewService.AddReviewAsync(999, userId, "Great movie!", 5);
            });
        }

        [Fact]
        public async Task AddReview_ShouldFailForUnauthenticatedUser()
        {
          
            var movie = new Movie { Id = 1, Title = "Test Movie" };
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            var reviewService = new ReviewService(_context);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await reviewService.AddReviewAsync(movie.Id, null, "Great movie!", 5);
            });
        }
[Fact]
public async Task GetReviews_ShouldReturnReviewsForMovie()
{
    // Arrange
    var dbContext = TestHelpers.GetInMemoryDbContext();
    var movie = new Movie { Id = 1, Title = "Test Movie" };
    dbContext.Movies.Add(movie);
    var user = new IdentityUser { Id = "test-user", UserName = "testuser" };
    dbContext.Users.Add(user);
    var review = new Review
    {
        Content = "Great movie!",
        Rating = 5,
        MovieId = movie.Id,
        UserId = user.Id
    };
    dbContext.Reviews.Add(review);
    await dbContext.SaveChangesAsync();

    var pageModel = new ReviewsModel(dbContext);

    // Act
    await pageModel.OnGetAsync(movie.Id);

    // Assert
    Assert.NotNull(pageModel.Reviews);
    Assert.Single(pageModel.Reviews);
    Assert.Equal("Great movie!", pageModel.Reviews.First().Content);
}

        [Fact]
        public async Task GetReviews_ShouldReturnAllReviewsForAMovie()
        {
           
            var movie = new Movie { Id = 1, Title = "Test Movie" };
            var user1 = new IdentityUser { Id = "user1", UserName = "user1" };
            var user2 = new IdentityUser { Id = "user2", UserName = "user2" };

            _context.Movies.Add(movie);
            _context.Users.AddRange(user1, user2);
            await _context.SaveChangesAsync();

            var reviewService = new ReviewService(_context);

            await reviewService.AddReviewAsync(movie.Id, user1.Id, "Amazing!", 5);
            await reviewService.AddReviewAsync(movie.Id, user2.Id, "Not bad.", 3);

            var reviews = await reviewService.GetReviewsForMovieAsync(movie.Id);

            Assert.Equal(2, reviews.Count);
            Assert.Contains(reviews, r => r.Content == "Amazing!" && r.Rating == 5);
            Assert.Contains(reviews, r => r.Content == "Not bad." && r.Rating == 3);
        }
    }
}
