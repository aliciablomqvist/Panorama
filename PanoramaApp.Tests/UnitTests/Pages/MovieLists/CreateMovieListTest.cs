using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Pages.Movies;
using Xunit;

public class CreateMovieListModelTests
{
[Fact]
public async Task OnPostAsync_LoggedInUser_CreatesMovieList()
{
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase("CreateMovieListTestDb")
        .Options;

    using var context = new ApplicationDbContext(options);

    var user = new IdentityUser { Id = "user1", UserName = "test@example.com" };
    context.Users.Add(user);
    await context.SaveChangesAsync();

    var userStore = new Mock<IUserStore<IdentityUser>>();
    var userManager = new Mock<UserManager<IdentityUser>>(userStore.Object, null, null, null, null, null, null, null, null);
    userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

    var pageModel = new CreateMovieListModel(context, userManager.Object)
    {
        Name = "TestList",
        IsShared = true
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

    var result = await pageModel.OnPostAsync();

    Assert.IsType<RedirectToPageResult>(result);
    var createdList = await context.MovieLists.FirstOrDefaultAsync(ml => ml.Name == "TestList");
    Assert.NotNull(createdList);
    Assert.Equal(user.Id, createdList.OwnerId);
    Assert.True(createdList.IsShared);
}

[Fact]
public async Task OnPostAsync_UserNotLoggedIn_Challenge()
{
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase("CreateMovieListNoUserDb")
        .Options;

    using var context = new ApplicationDbContext(options);

    var userStore = new Mock<IUserStore<IdentityUser>>();
    var userManager = new Mock<UserManager<IdentityUser>>(
        userStore.Object, null, null, null, null, null, null, null, null);

    userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((IdentityUser)null);

    var pageModel = new CreateMovieListModel(context, userManager.Object)
    {
        Name = "ShouldNotCreate",
        IsShared = false
    };

    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity()); // Ingen inloggad användare
    var httpContext = new DefaultHttpContext { User = claimsPrincipal };
    pageModel.PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
    {
        HttpContext = httpContext
    };

    var result = await pageModel.OnPostAsync();

    Assert.IsType<ChallengeResult>(result);
    Assert.Empty(context.MovieLists);
}
}
