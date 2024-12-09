using Microsoft.AspNetCore.Identity;
namespace PanoramaApp.Models
{
    public class ChatMessage
{
    public int Id { get; set; }
    public string MessageText { get; set; }

      public string UserId { get; set; }
    public string UserName { get; set; }
    public DateTime Timestamp { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; } 

}
}