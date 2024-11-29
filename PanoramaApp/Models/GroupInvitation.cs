using Microsoft.AspNetCore.Identity;
namespace PanoramaApp.Models
{
    public class GroupInvitation
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string InvitedUserId { get; set; } = string.Empty;
    public string InvitedByUserId { get; set; } = string.Empty;
    public bool IsAccepted { get; set; }
    public DateTime InvitationDate { get; set; }
}
}