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
using PanoramaApp.Tests.Helpers;
public class ChatMessageTests
{
[Fact]
public async Task SendMessage_AddsMessageToGroupChat()
{
    // Arrange
    var messages = new List<ChatMessage>
    {
        new ChatMessage { MessageText = "Hello Group!", UserName = "John Doe", GroupId = 1, Timestamp = DateTime.UtcNow }
    };

    var mockChatMessagesDbSet = messages.CreateMockDbSet();
    var mockDbContext = new Mock<ApplicationDbContext>();
    mockDbContext.Setup(db => db.ChatMessages).Returns(mockChatMessagesDbSet.Object);

    var groupChatService = new GroupChatService(mockDbContext.Object);

    // Act
    await groupChatService.SendMessageAsync("Hello Group!", "John Doe", 1);

    // Assert
    mockDbContext.Verify(db => db.ChatMessages.Add(It.IsAny<ChatMessage>()), Times.Once);
    mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
}


[Fact]
public async Task GetMessages_ReturnsMessagesInOrder()
{
    // Arrange
    var groupId = 1;
    var messages = new List<ChatMessage>
    {
        new ChatMessage { MessageText = "Message 1", GroupId = groupId, Timestamp = DateTime.UtcNow.AddMinutes(-10) },
        new ChatMessage { MessageText = "Message 2", GroupId = groupId, Timestamp = DateTime.UtcNow }
    };
    var mockChatMessagesDbSet = messages.CreateMockDbSet();
    var mockDbContext = new Mock<ApplicationDbContext>();
    mockDbContext.Setup(db => db.ChatMessages).Returns(mockChatMessagesDbSet.Object);

    var chatService = new GroupChatService(mockDbContext.Object);

    // Act
    var result = await chatService.GetMessagesForGroupAsync(groupId);

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Equal("Message 1", result.First().MessageText);
    Assert.Equal("Message 2", result.Last().MessageText);
}
}
