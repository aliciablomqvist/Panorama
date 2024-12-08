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
    public class ShareListsTests
    {
       private readonly string _userId = "test-user-id";
        private readonly ApplicationDbContext _context;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly HttpContext _httpContext;
 
    public ShareListsTests()
    {
        _context = TestHelpers.GetInMemoryDbContext();
            _mockUserManager = TestHelpers.GetMockUserManager();
            _httpContext = TestHelpers.GetMockHttpContext(_userId);
    }

        [Fact]
public async Task ShareMovieList_ShouldAddUserToSharedList()
{
    // Arrange
   // Se över det här:
    var dbContext = GetInMemoryDbContext();
    var ownerId = "owner-id";
    var recipientId = "recipient-id";

    var owner = new IdentityUser { Id = ownerId, UserName = "owner" };
    var recipient = new IdentityUser { Id = recipientId, UserName = "recipient" };
    dbContext.Users.AddRange(owner, recipient);

    var movieList = new MovieList { Id = 1, Name = "Shared List", OwnerId = ownerId };
    dbContext.MovieLists.Add(movieList);
    await dbContext.SaveChangesAsync();

    var userManager = GetMockUserManager();
    var httpContext = GetMockHttpContext(ownerId);

    var pageModel = new ShareModel(dbContext, userManager)
    {
        PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
        {
            HttpContext = httpContext
        },
        ListId = movieList.Id,
        RecipientUserName = recipient.UserName
    };

    // Act
    var result = await pageModel.OnPostAsync();

    // Assert
    var sharedList = await dbContext.SharedMovieLists
        .FirstOrDefaultAsync(sml => sml.MovieListId == movieList.Id && sml.SharedWithUserId == recipientId);

    Assert.NotNull(sharedList);
    Assert.Equal(movieList.Id, sharedList.MovieListId);
    Assert.Equal(recipientId, sharedList.SharedWithUserId);
}
}
}
