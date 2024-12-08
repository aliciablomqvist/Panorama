using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Pages.Movies;
using Xunit;

public class ShareModelTests
{
    [Fact]
    public async Task OnGetAsync_ValidListId_ShowsShareLink()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ShareTestDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        var list = new MovieList { Name = "ShareList" };
        context.MovieLists.Add(list);
        await context.SaveChangesAsync();

        var pageModel = new ShareModel(context);

        pageModel.PageContext.HttpContext = new DefaultHttpContext();
        pageModel.PageContext.HttpContext.Request.Scheme = "https";

        var result = await pageModel.OnGetAsync(list.Id);

        Assert.IsType<PageResult>(result);
        Assert.NotNull(pageModel.MovieList);
        Assert.Contains("https://", pageModel.ShareableLink);
    }

    [Fact]
    public async Task OnGetAsync_InvalidListId_NotFound()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ShareNotFoundDb")
            .Options;

        using var context = new ApplicationDbContext(options);
        var pageModel = new ShareModel(context);

        var result = await pageModel.OnGetAsync(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
