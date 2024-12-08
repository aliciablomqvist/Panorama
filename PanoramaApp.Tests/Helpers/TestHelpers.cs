using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Moq;
using PanoramaApp.Data;
using System.Security.Claims;

namespace PanoramaApp.Tests.Helpers
{
public static class TestHelpers
{
public static ApplicationDbContext GetInMemoryDbContext()
{
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

    return new ApplicationDbContext(options);
}

public static Mock<UserManager<IdentityUser>> GetMockUserManager()
{
    var store = new Mock<IUserStore<IdentityUser>>();
    var userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
    return userManager;
}



    public static HttpContext GetMockHttpContext(string userId)
    {
        var context = new DefaultHttpContext();
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
        context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthType"));
        return context;
    }
}
}