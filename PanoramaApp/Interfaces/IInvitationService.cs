using PanoramaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Interfaces
{
    public interface IInvitationService
    {
        Task<List<GroupInvitation>> GetPendingInvitationsAsync(string userId);
        Task AcceptInvitationAsync(int invitationId, string userId);
        Task SendInvitationAsync(int groupId, string invitedUserId, string invitedByUserId);
    }
}
