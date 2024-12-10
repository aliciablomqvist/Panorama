/*using Xunit;
using PanoramaApp.Data;
using PanoramaApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PanoramaApp.Tests.IntegrationTests
{

    //Testar integrationen av databasen
    public class DatabaseTests
    {
        [Fact]
        public async Task CreateGroup_ShouldPersistInDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            using var context = new ApplicationDbContext(options);
            var newGroup = new Group { Name = "Test Group" };

            // Act
            context.Groups.Add(newGroup);
            await context.SaveChangesAsync();

            // Assert
            var group = await context.Groups.FirstAsync();
            Assert.Equal("Test Group", group.Name);
        }
    }
}*/
