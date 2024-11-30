using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;

namespace PanoramaApp.Tests
{
    //Hjälpklass för mocks
    public static class TestHelpers
    {
        public static Mock<ApplicationDbContext> CreateDbContextMock()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            var mockContext = new Mock<ApplicationDbContext>(options);
            return mockContext;
        }

        public static Mock<UserManager<IdentityUser>> CreateUserManagerMock()
        {
            var userStore = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(
                userStore.Object, null, null, null, null, null, null, null, null);
        }
    }
}
