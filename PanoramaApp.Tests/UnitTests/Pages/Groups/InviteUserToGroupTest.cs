using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Pages.Groups;
using Xunit;

public class InvitationsModelTests
{
    [Fact]
    public async Task OnPostAcceptAsync_ValidInvitationId_MarksAsAcceptedAndRedirects()
    {
        // Arrange (Given)
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDb_Invitations")
            .Options;

        using var context = new ApplicationDbContext(options);

        var invitation = new GroupInvitation
        {
            GroupId = 1,
            InvitedUserId = "user123",
            InvitedByUserId = "inviter123",
            IsAccepted = false,
            InvitationDate = DateTime.UtcNow
        };
        context.GroupInvitations.Add(invitation);
        await context.SaveChangesAsync();

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        var pageModel = new InvitationsModel(context, userManager.Object);

        // Act (When)
        var result = await pageModel.OnPostAcceptAsync(invitation.Id);

        // Assert (Then)
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Null(redirectResult.PageName); // Redirectar till samma sida

        var updatedInvitation = await context.GroupInvitations.FindAsync(invitation.Id);
        Assert.True(updatedInvitation.IsAccepted);
    }

    [Fact]
    public async Task OnPostAcceptAsync_InvalidInvitationId_ReturnsNotFound()
    {
        // Arrange (Given)
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDb_Invitations_Invalid")
            .Options;

        using var context = new ApplicationDbContext(options);

        var userStore = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        var pageModel = new InvitationsModel(context, userManager.Object);

        // Act (When)
        var result = await pageModel.OnPostAcceptAsync(999);

        // Assert (Then)
        Assert.IsType<NotFoundResult>(result);
    }
}
