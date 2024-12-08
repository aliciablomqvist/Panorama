using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System;
using System.Security.Claims;
using PanoramaApp.Services;


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

public static Mock<UserManager<TUser>> GetMockUserManager<TUser>() where TUser : class
{
    var store = new Mock<IUserStore<TUser>>();
    return new Mock<UserManager<TUser>>(
        store.Object, null, null, null, null, null, null, null, null
    );
}

public static HttpContext GetMockHttpContext(string userId)
{
    var httpContext = new DefaultHttpContext();
    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.NameIdentifier, userId)
    }));
    httpContext.User = claimsPrincipal;
    return httpContext;
}

public static Mock<ReviewService> GetMockReviewService()
{
    var mockReviewService = new Mock<ReviewService>();
    // Behövs dessa också?
    // mockReviewService.Setup(s => s.AddReviewAsync(...)).ReturnsAsync(...);
    return mockReviewService;
}
    }
}
