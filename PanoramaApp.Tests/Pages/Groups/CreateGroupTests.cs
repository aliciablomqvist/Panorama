using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Moq;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Pages.Groups;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Tests.Pages.Groups
{
    public class CreateGroupTests
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;

        public CreateGroupTests()
        {
            // In-memory-databas för test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            var userStore = new Mock<IUserStore<IdentityUser>>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                userStore.Object, null, null, null, null, null, null, null, null);
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
    }
}
