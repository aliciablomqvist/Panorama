using Microsoft.AspNetCore.Identity;
namespace PanoramaApp.Models
{
public class GroupChatContext
{
    public int Id { get; set; }
    public string GroupName { get; set; }
    public ICollection<ChatMessage> Messages { get; set; }
}
}
