using Xunit;
using PanoramaApp.Pages.Vote;
using PanoramaApp.Models;
using System.Linq; 
using PanoramaApp.Data;
using PanoramaApp.Pages.Groups;

namespace PanoramaApp.Tests.Groups
{
    public class GroupTests 
     {

        [Fact]
public void CreateGroup_AddsGroupToMockData()
{
    // Arrange
    var model = new CreateGroupModel();
    model.Name = "Test Group";
    model.SelectedMovies = new List<int> { 1 };
    model.SelectedUsers = new List<int> { 1 };

    // Act
    model.OnPost();

    // Assert
    Assert.Contains(MockData.Groups, g => g.Name == "Test Group");
}
}
}
