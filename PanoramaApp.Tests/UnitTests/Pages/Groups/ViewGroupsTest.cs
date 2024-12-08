using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Pages.Groups;
using Xunit;

public class ViewGroupsModelTests
{
    [Fact]
    public async Task OnGetAsync_GivenGroupsExist_LoadsGroupsWithMembersAndMovies()
    {
        // Arrange (Given)
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ViewGroupsTestDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        // Skapa testdata
        var group = new Group { Name = "TestGroup" };
        var member = new GroupMember { Group = group, UserId = "user1" };
        var movie = new Movie { Title = "TestMovie", Group = group };

        context.Groups.Add(group);
        context.GroupMembers.Add(member);
        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        var pageModel = new ViewGroupsModel(context);

        // Act (When)
        await pageModel.OnGetAsync();

        // Assert (Then)
        Assert.NotEmpty(pageModel.Groups);
        var loadedGroup = pageModel.Groups.FirstOrDefault(g => g.Name == "TestGroup");
        Assert.NotNull(loadedGroup);
        Assert.NotEmpty(loadedGroup.Members);
        Assert.NotEmpty(loadedGroup.Movies);
    }

    [Fact]
    public async Task OnGetAsync_NoGroupsExist_GroupsIsEmpty()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ViewGroupsEmptyTestDb")
            .Options;

        using var context = new ApplicationDbContext(options);
        var pageModel = new ViewGroupsModel(context);

        // Act
        await pageModel.OnGetAsync();

        // Assert
        Assert.Empty(pageModel.Groups);
    }
}
