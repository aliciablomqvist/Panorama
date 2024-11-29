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
            // in-memory-databas för test
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
        
            var user1 = new IdentityUser { Id = "user1", UserName = "User 1" };
            var user2 = new IdentityUser { Id = "user2", UserName = "User 2" };
            await _context.Users.AddRangeAsync(user1, user2);
            await _context.SaveChangesAsync();

            var pageModel = new CreateGroupModel(_context, _mockUserManager.Object)
            {
                Name = "Test Group",
                SelectedUsers = new List<string> { "user1", "user2" }
            };

            _mockUserManager.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => _context.Users.Find(id));
                
            var result = await pageModel.OnPostAsync();
            var group = await _context.Groups.Include(g => g.Members).FirstOrDefaultAsync(g => g.Name == "Test Group");
            Assert.NotNull(group);
            Assert.Equal(2, group.Members.Count);
        }
    }
}
