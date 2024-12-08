
namespace PanoramaApp.Tests.UnitTests.Pages.Groups
{
    public class CreateGroupTests
    {
            private readonly string userId = "test-user-id";
    private readonly ApplicationDbContext _context;
    private readonly Mock<UserManager<IdentityUser>> _mockUserManager;

    private readonly HttpContext _httpContext;

    public CreateGroupTests()
    {
        _context = TestHelpers.GetInMemoryDbContext();
        _mockUserManager = TestHelpers.GetMockUserManager();
          _httpContext = TestHelpers.GetMockHttpContext(userId);
    }

        [Fact]
        public async Task OnPostAsync_ValidData_ShouldCreateGroup()
        {
            // Arrange
            var testGroupName = "Test Group";
            var pageModel = new CreateGroupModel(_context, _mockUserManager.Object)
            {
                Name = testGroupName
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Name == testGroupName);
            Assert.NotNull(group);
            Assert.Equal(testGroupName, group.Name);
        }

        [Fact]
        public async Task OnPostAsync_AddMembersToGroup()
        {
            // Arrange
            var testUsers = new List<string> { "user1", "user2" };
            var mockUser1 = new IdentityUser { Id = "user1" };
            var mockUser2 = new IdentityUser { Id = "user2" };

            _mockUserManager.Setup(u => u.FindByIdAsync("user1")).ReturnsAsync(mockUser1);
            _mockUserManager.Setup(u => u.FindByIdAsync("user2")).ReturnsAsync(mockUser2);

            var pageModel = new CreateGroupModel(_context, _mockUserManager.Object)
            {
                Name = "Test Group",
                SelectedUsers = testUsers
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Name == "Test Group");
            Assert.NotNull(group);

            var groupMembers = await _context.GroupMembers.ToListAsync();
            Assert.Equal(2, groupMembers.Count);
            Assert.Contains(groupMembers, gm => gm.UserId == "user1");
            Assert.Contains(groupMembers, gm => gm.UserId == "user2");
        }
        [Fact]
        public async Task CreateGroup_ShouldSetOwnerAndDefaultSettings()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var ownerId = "owner-id";
            var userManager = GetMockUserManager();
            var httpContext = GetMockHttpContext(ownerId);
            var pageModel = new CreateGroupModel(dbContext, userManager)
            {
                PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
                {
                    HttpContext = httpContext
                },
                Name = "New Group"
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            var group = await dbContext.Groups.FirstOrDefaultAsync(g => g.Name == "New Group");
            Assert.NotNull(group);
            Assert.Equal(ownerId, group.OwnerId);
        }
    }
}
