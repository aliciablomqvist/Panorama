using Xunit;
using Moq;
using PanoramaApp.Models;
using PanoramaApp.Pages.Movies;
using PanoramaApp.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace PanoramaApp.Tests.UnitTests.Pages.Movies
{
    public class MovieDetailsTests
    {
        private readonly string _userId = "test-user-id";
        private readonly ApplicationDbContext _context;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly HttpContext _httpContext;

        public MovieDetailsTests()
        {
            _context = TestHelpers.GetInMemoryDbContext();
            _mockUserManager = TestHelpers.GetMockUserManager();
            _httpContext = TestHelpers.GetMockHttpContext(_userId);
        }

        [Fact]
        public async Task OnPostAddToFavoritesAsync_CreatesFavoritesListAndAddsMovie()
        {
            // Arrange
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new IdentityUser { Id = _userId });

            var userManager = _mockUserManager.Object;

            var movie = new Movie { Id = 1, Title = "Test Movie" };
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            var pageModel = new MovieDetailsModel(_context, userManager)
            {
                PageContext = new PageContext
                {
                    HttpContext = _httpContext
                }
            };

            // Act
            var result = await pageModel.OnPostAddToFavoritesAsync(movie.Id);

            // Assert
            var favoritesList = await _context.MovieLists
                .Include(ml => ml.Movies)
                .FirstOrDefaultAsync(ml => ml.Name == "My Favorites" && ml.OwnerId == _userId);

            Assert.NotNull(favoritesList);
            Assert.Single(favoritesList.Movies);
            Assert.Equal(movie.Id, favoritesList.Movies.First().MovieId);
        }



        [Fact]
        public async Task OnPostAddToFavoritesAsync_AddsMovieToExistingFavoritesList()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();

            var movie = new Movie { Id = 1, Title = "Test Movie" };
            var userId = "test-user-id";
            var favoritesList = new MovieList { Id = 1, Name = "My Favorites", OwnerId = userId };
            dbContext.Movies.Add(movie);
            dbContext.MovieLists.Add(favoritesList);
            await dbContext.SaveChangesAsync();

            var userManager = GetMockUserManager();

            var httpContext = new DefaultHttpContext();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            }, "TestAuthentication"));

            httpContext.User = claimsPrincipal;

            var pageModel = new MovieDetailsModel(dbContext, userManager)
            {
                PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
                {
                    HttpContext = httpContext
                }
            };

            // Act
            var result = await pageModel.OnPostAddToFavoritesAsync(movie.Id);

            // Assert
            favoritesList = await dbContext.MovieLists
                .Include(ml => ml.Movies)
                .FirstOrDefaultAsync(ml => ml.Name == "My Favorites" && ml.OwnerId == userId);

            Assert.NotNull(favoritesList);
            Assert.Single(favoritesList.Movies);
            Assert.Equal(movie.Id, favoritesList.Movies.First().MovieId);
        }

        [Fact]
        public async Task OnPostAddToFavoritesAsync_DoesNotDuplicateMovieInFavorites()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();

            var movie = new Movie { Id = 1, Title = "Test Movie" };
            var userId = "test-user-id";
            var favoritesList = new MovieList
            {
                Id = 1,
                Name = "My Favorites",
                OwnerId = userId,
                Movies = new List<MovieListItem>
                {
                    new MovieListItem { MovieId = movie.Id, MovieListId = 1 }
                }
            };
            dbContext.Movies.Add(movie);
            dbContext.MovieLists.Add(favoritesList);
            await dbContext.SaveChangesAsync();

            var userManager = GetMockUserManager();

            var httpContext = new DefaultHttpContext();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            }, "TestAuthentication"));

            httpContext.User = claimsPrincipal;

            var pageModel = new MovieDetailsModel(dbContext, userManager)
            {
                PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
                {
                    HttpContext = httpContext
                }
            };

            // Act
            var result = await pageModel.OnPostAddToFavoritesAsync(movie.Id);

            // Assert
            favoritesList = await dbContext.MovieLists
                .Include(ml => ml.Movies)
                .FirstOrDefaultAsync(ml => ml.Name == "My Favorites" && ml.OwnerId == userId);

            Assert.NotNull(favoritesList);
            Assert.Single(favoritesList.Movies); 
        }

        [Fact]
        public async Task OnGetAsync_ReturnsPageWithMovieDetails()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();

            var movie = new Movie { Id = 1, Title = "Test Movie", Description = "Test Description" };
            dbContext.Movies.Add(movie);
            await dbContext.SaveChangesAsync();

            var userManager = GetMockUserManager();

            var pageModel = new MovieDetailsModel(dbContext, userManager);

            // Act
            var result = await pageModel.OnGetAsync(movie.Id);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(pageModel.Movie);
            Assert.Equal(movie.Title, pageModel.Movie.Title);
            Assert.Equal(movie.Description, pageModel.Movie.Description);
        }

        [Fact]
        public async Task OnGetAsync_ReturnsNotFoundWhenMovieDoesNotExist()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var pageModel = new MovieDetailsModel(dbContext, userManager);

            // Act
            var result = await pageModel.OnGetAsync(999); // Non-existent movie ID

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task OnGetAsync_ReturnsPageWithFavorites()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();

            var userId = "test-user-id";
            var movie = new Movie { Id = 1, Title = "Test Movie" };
            var favoritesList = new MovieList
            {
                Id = 1,
                Name = "My Favorites",
                OwnerId = userId,
                Movies = new List<MovieListItem>
                {
                    new MovieListItem { MovieId = movie.Id, MovieListId = 1, Movie = movie }
                }
            };
            dbContext.Movies.Add(movie);
            dbContext.MovieLists.Add(favoritesList);
            await dbContext.SaveChangesAsync();

            var userManager = GetMockUserManager();

            var httpContext = new DefaultHttpContext();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            }, "TestAuthentication"));

            httpContext.User = claimsPrincipal;

            var pageModel = new PanoramaApp.Pages.MovieLists.MyFavoritesModel(dbContext, userManager)
            {
                PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
                {
                    HttpContext = httpContext
                }
            };

            // Act
            var result = await pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(pageModel.MovieList);
            Assert.Single(pageModel.MovieList.Movies);
            Assert.Equal(movie.Title, pageModel.MovieList.Movies.First().Movie.Title);
        }
    }
}
