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

public class PrioritizeMoviesTests
{
    [Fact]
public async Task SendMessage_AddsMessageToGroupChat()
{
    // Arrange
    var group = new Group { Id = 1, Name = "Movie Group" };
    var message = new ChatMessage { Text = "Hello Group!", UserId = "user1", Timestamp = DateTime.UtcNow };

    var context = new Mock<GroupChatContext>();
    var chatService = new GroupChatService(context.Object);

    // Act
    await chatService.SendMessage(group.Id, message);

    // Assert
    var messages = await chatService.GetMessages(group.Id);
    Assert.Single(messages);
    Assert.Equal("Hello Group!", messages.First().Text);
}
}
