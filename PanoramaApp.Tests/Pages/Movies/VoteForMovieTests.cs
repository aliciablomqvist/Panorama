using PanoramaApp.Tests.Helpers;

namespace PanoramaApp.Tests.Pages.Movies
{
    public class VoteForMovieTests
    {

            private readonly string userId = "test-user-id";
    private readonly ApplicationDbContext _context;
    private readonly Mock<UserManager<IdentityUser>> _mockUserManager;

       private readonly HttpContext _httpContext;


    public VoteForMovieTests()
    {
     _context = TestHelpers.GetInMemoryDbContext();
_mockUserManager = TestHelpers.GetMockUserManager();
_httpContext = TestHelpers.GetMockHttpContext(userId);

    }

   [Fact]
        public async Task VoteForMovie_ShouldRecordVote()
        {
            // Arrange
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new IdentityUser { Id = _userId });

            var userManager = _mockUserManager.Object;

            var pageModel = new VoteForMovieModel(_context, userManager)
            {
                PageContext = new PageContext
                {
                    HttpContext = _httpContext
                },
                MovieId = 1
            };

            // Act
            var result = await pageModel.OnPostAsync();

    // Assert
    var updatedMovie = await dbContext.Movies.FindAsync(movie.Id);
    Assert.Equal(1, updatedMovie.Votes);
}
}
}