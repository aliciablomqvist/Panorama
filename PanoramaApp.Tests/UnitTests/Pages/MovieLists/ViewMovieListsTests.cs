using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PanoramaApp.Data;
using PanoramaApp.Models;
using Xunit;

public class ViewMovieListsModelTests
{
    [Fact]
    public async Task OnGetAsync_NoLists_LogsWarning()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ViewMovieListsEmptyDb")
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new ApplicationDbContext(options);

        var logger = new Mock<ILogger<ViewMovieListsModel>>();
        var pageModel = new ViewMovieListsModel(context, logger.Object);

        await pageModel.OnGetAsync();

        Assert.Empty(pageModel.MovieLists);
        logger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("No MovieLists found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }

    [Fact]
    public async Task OnGetAsync_ListsExist_LogsInformation()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ViewMovieListsDb")
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new ApplicationDbContext(options);

        var list = new MovieList { Name = "TestList" };
        context.MovieLists.Add(list);
        await context.SaveChangesAsync();

        var logger = new Mock<ILogger<ViewMovieListsModel>>();
        var pageModel = new ViewMovieListsModel(context, logger.Object);

        await pageModel.OnGetAsync();

        Assert.Single(pageModel.MovieLists);
        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Fetched 1 MovieLists")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }
}
