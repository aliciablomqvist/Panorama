
namespace PanoramaApp.Tests.Pages.Movies
{
public class AddMovieToListTests
{
            private readonly string userId = "test-user-id";
    private readonly ApplicationDbContext _context;
    private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
  private readonly HttpContext _httpContext;
    public AddMovieToListTests()
    {
        _context = TestHelpers.GetInMemoryDbContext();
        _mockUserManager = TestHelpers.GetMockUserManager();
         _httpContext = TestHelpers.GetMockHttpContext(userId);
    }

        [Fact]
        public async Task AddMovieToList_ShouldAddMovieToUserList()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var userId = "test-user-id";


            var user = new IdentityUser { Id = userId, UserName = "testuser" };
            dbContext.Users.Add(user);

            var movieList = new MovieList { Id = 1, Name = "My List", OwnerId = userId };
            dbContext.MovieLists.Add(movieList);


            var movie = new Movie { Id = 1, Title = "Test Movie" };
            dbContext.Movies.Add(movie);

            await dbContext.SaveChangesAsync();
            var userManager = GetMockUserManager();
            var httpContext = GetMockHttpContext(userId);

            var pageModel = new AddMovieToListModel(dbContext, userManager)
            {
                PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
                {
                    HttpContext = httpContext
                },
                MovieId = movie.Id,
                ListId = movieList.Id
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            var movieListItem = await dbContext.MovieListItems
                .FirstOrDefaultAsync(mli => mli.MovieListId == movieList.Id && mli.MovieId == movie.Id);

            Assert.NotNull(movieListItem);
            Assert.Equal(movieList.Id, movieListItem.MovieListId);
            Assert.Equal(movie.Id, movieListItem.MovieId);
        }
    }
}