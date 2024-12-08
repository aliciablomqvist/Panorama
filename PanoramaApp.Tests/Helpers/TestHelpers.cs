using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Moq;
using PanoramaApp.Data;
using System.Security.Claims;

namespace PanoramaApp.Tests.Helpers
{
public static class TestHelper
{
    public static void SetUserAndHttpContext<TPageModel>(TPageModel pageModel, string userId, string userName)
        where TPageModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, userName)
        }, "TestAuth"));

        var httpContext = new DefaultHttpContext { User = claimsPrincipal };

        pageModel.PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
        {
            HttpContext = httpContext
        };
    }
}
}