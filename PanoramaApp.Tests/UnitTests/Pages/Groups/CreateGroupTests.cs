using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PanoramaApp.Data;
using PanoramaApp.Models;
using Xunit;

public class CreateGroupModelTests
{
[Fact]
public async Task OnPostAsync_GivenValidData_CreatesGroupAndMembersAndRedirects()
{
    // Arrange
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: "TestDb_CreateGroup")
        .Options;

    using var context = new ApplicationDbContext(options);

    var user1 = new IdentityUser { Id = "user1", UserName = "user1@example.com" };
    var user2 = new IdentityUser { Id = "user2", UserName = "user2@example.com" };
    context.Users.AddRange(user1, user2);
    await context.SaveChangesAsync();

    // Mock UserManager
    var userStore = new Mock<IUserStore<IdentityUser>>();
    var userManager = new Mock<UserManager<IdentityUser>>(
        userStore.Object, null, null, null, null, null, null, null, null);

    userManager.Setup(u => u.FindByIdAsync("user1")).ReturnsAsync(user1);
    userManager.Setup(u => u.FindByIdAsync("user2")).ReturnsAsync(user2);

    var group = new Group
    {
        Name = "TestGroup",
        OwnerId = "user1" // Tilldela OwnerId direkt
    };

    context.Groups.Add(group);
    await context.SaveChangesAsync();

    var pageModel = new CreateGroupModel(context, userManager.Object)
    {
        Name = "TestGroup",
        SelectedUsers = new List<string> { "user1", "user2" }
    };

    // Act
    var result = await pageModel.OnPostAsync();

    // Assert
    var redirectResult = Assert.IsType<RedirectToPageResult>(result);
    Assert.Equal("/Groups/ViewGroups", redirectResult.PageName);

    var savedGroup = await context.Groups.FirstOrDefaultAsync(g => g.Name == "TestGroup");
    Assert.NotNull(savedGroup);
    Assert.Equal("user1", savedGroup.OwnerId); // Kontrollera OwnerId

    var members = await context.GroupMembers.ToListAsync();
    Assert.Equal(2, members.Count);
    Assert.Contains(members, m => m.UserId == "user1");
    Assert.Contains(members, m => m.UserId == "user2");
}

    [Fact]
    public async Task OnPostAsync_GivenInvalidModelState_ReturnsPage()
    {
        // Arrange (Given)
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_CreateGroup_Invalid")
            .Options;

        using var context = new ApplicationDbContext(options);
        
        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        var pageModel = new CreateGroupModel(context, userManager.Object)
        {
            Name = "" // Ogiltig data
        };
        pageModel.ModelState.AddModelError("Name", "Name is required");

        // Act (When)
        var result = await pageModel.OnPostAsync();

        // Assert (Then)
        // PageResult eftersom ModelState är ogiltig
        Assert.IsType<PageResult>(result);
    }
}
