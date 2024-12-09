using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Pages.MovieLists;
using Xunit;
using PanoramaApp.Services;
public class ChatMessageTests
{
    [Fact]
public async Task SendMessage_AddsMessageToGroupChat()
{

    // Arrange
    var group = new Group { Id = 1, Name = "Movie Group" };
    var message = new ChatMessage { MessageText  = "Hello Group!", UserId = "user1", Timestamp = DateTime.UtcNow };

 
    var context = new Mock<ApplicationDbContext>();
var groupChatService = new GroupChatService(context.Object);

    // Act
    await groupChatService .SendMessage("Hello Group!", "John Doe", group.Id);

    // Assert
    var messages = await groupChatService .GetMessages(group.Id);
    Assert.Single(messages);
    //Assert.Equal("Hello, group!", "John Doe", 1, messages.First().MessageText );
}
}
