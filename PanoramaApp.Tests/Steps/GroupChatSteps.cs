using System.Linq;
using System.Threading.Tasks;
using Xunit;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Services;
using Moq;
using TechTalk.SpecFlow;

[Binding]
public class GroupChatSteps
{
    private readonly GroupChatService _groupChatService;
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private Group _group;
    private ChatMessage _sentMessage;
    private List<ChatMessage> _chatHistory;

    public GroupChatSteps()
    {
        _dbContextMock = new Mock<ApplicationDbContext>();

        _groupChatService = new GroupChatService(_dbContextMock.Object);
        _chatHistory = new List<ChatMessage>();
    }

    [Given(@"that I am a member of a group")]
    public void GivenThatIAmAMemberOfAGroup()
    {
        _group = new Group { Id = 1, Name = "Movie Group" };
        _dbContextMock.Setup(db => db.Groups.FindAsync(_group.Id))
                      .ReturnsAsync(_group);
    }



    [Given(@"the group exists in the database")]
    public void GivenTheGroupExistsInTheDatabase()
    {
        var groupList = new List<Group> { _group };
        var mockGroupSet = groupList.CreateMockDbSet();
        _dbContextMock.Setup(db => db.Groups).Returns(mockGroupSet.Object);
    }

    [When(@"I send a message saying ""(.*)""")]
    public async Task WhenISendAMessageSaying(string message)
    {
        _sentMessage = new ChatMessage
        {
            MessageText = message,
            UserId = "user1",
            UserName = "John Doe",
            GroupId = _group.Id,
            Timestamp = DateTime.Now
        };

        await _groupChatService.SendMessageAsync(message, "John Doe", _group.Id);
    }

    [Then(@"the message should be saved in the group's chat history")]
    public void ThenTheMessageShouldBeSavedInTheGroupSChatHistory()
    {
        _dbContextMock.Verify(db => db.ChatMessages.Add(It.Is<ChatMessage>(
            m => m.MessageText == _sentMessage.MessageText &&
                 m.UserName == _sentMessage.UserName &&
                 m.GroupId == _group.Id)), Times.Once);

        _dbContextMock.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }

    [Given(@"the group has chat messages in its history")]
    public void GivenTheGroupHasChatMessagesInItsHistory()
    {
        var messages = new List<ChatMessage>
    {
        new ChatMessage { MessageText = "This is a previous message", GroupId = _group.Id, Timestamp = DateTime.UtcNow.AddMinutes(-5) }
    };
        var mockMessageSet = messages.CreateMockDbSet();
        _dbContextMock.Setup(db => db.ChatMessages).Returns(mockMessageSet.Object);
    }

    [When(@"I view the group chat")]
    public async Task WhenIViewTheGroupChat()
    {
        _chatHistory = await _groupChatService.GetMessagesForGroupAsync(_group.Id);
    }

    [Then(@"I should see all previous messages in the order they were sent")]
    public void ThenIShouldSeeAllPreviousMessagesInTheOrderTheyWereSent()
    {
        Assert.True(_chatHistory.SequenceEqual(_chatHistory.OrderBy(m => m.Timestamp)));
        Assert.Contains(_chatHistory, m => m.MessageText == "This is a previous message");
    }
}
