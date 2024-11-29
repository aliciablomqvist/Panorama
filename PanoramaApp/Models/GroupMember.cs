using Microsoft.AspNetCore.Identity;
namespace PanoramaApp.Models
{
    public class GroupMember
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; }
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
}
}