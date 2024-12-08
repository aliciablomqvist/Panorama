
namespace PanoramaApp.Tests.UnitTests.Pages.Groups
{
    public class InviteUserToGroupTests
    {

            private readonly string userId = "test-user-id";
    private readonly ApplicationDbContext _context;
    private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
    private readonly HttpContext _httpContext;

    public InviteUserToGroupTests()
    {
        _context = TestHelpers.GetInMemoryDbContext();
        _mockUserManager = TestHelpers.GetMockUserManager();
         _httpContext = TestHelpers.GetMockHttpContext(userId);
    }


[Fact]
public async Task InviteUserToGroup_ShouldAddUserToGroupMembers()
{
    // Arrange
    var dbContext = GetInMemoryDbContext();
    var ownerId = "owner-id";
    var inviteeId = "invitee-id";

    var owner = new IdentityUser { Id = ownerId, UserName = "owner" };
    var invitee = new IdentityUser { Id = inviteeId, UserName = "invitee" };
    dbContext.Users.AddRange(owner, invitee);

    var group = new Group { Id = 1, Name = "Test Group", OwnerId = ownerId };
    dbContext.Groups.Add(group);
    await dbContext.SaveChangesAsync();

    var userManager = GetMockUserManager();
    var httpContext = GetMockHttpContext(ownerId);

    var pageModel = new InvitationsModel(dbContext, userManager)
    {
        PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
        {
            HttpContext = httpContext
        },
        GroupId = group.Id,
        InviteeUserName = invitee.UserName
    };

    // Act
    var result = await pageModel.OnPostAsync();

    // Assert
    var groupMember = await dbContext.GroupMembers
        .FirstOrDefaultAsync(gm => gm.GroupId == group.Id && gm.UserId == inviteeId);

    Assert.NotNull(groupMember);
    Assert.Equal(group.Id, groupMember.GroupId);
    Assert.Equal(inviteeId, groupMember.UserId);
}
}
}