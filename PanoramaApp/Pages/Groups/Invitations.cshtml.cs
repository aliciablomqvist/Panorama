using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;

namespace PanoramaApp.Pages.Groups
{
    public class InvitationsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public InvitationsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public List<GroupInvitation> Invitations { get; set; } = new List<GroupInvitation>();
        public List<Group> Groups { get; set; } = new List<Group>();
        public List<IdentityUser> Users { get; set; } = new List<IdentityUser>();

        public async Task OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            Invitations = await _context.GroupInvitations
                .Where(i => i.InvitedUserId == currentUser.Id && !i.IsAccepted)
                .ToListAsync();

            Groups = await _context.Groups.ToListAsync();

            Users = await _context.Users.ToListAsync();
        }

        public async Task<IActionResult> OnPostAcceptAsync(int invitationId)
        {
            var invitation = await _context.GroupInvitations.FindAsync(invitationId);

            if (invitation == null)
            {
                return NotFound();
            }

            invitation.IsAccepted = true;
            _context.GroupInvitations.Update(invitation);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostInviteAsync(int groupId, string invitedUserId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var newInvitation = new GroupInvitation
            {
                GroupId = groupId,
                InvitedUserId = invitedUserId,
                InvitedByUserId = currentUser.Id,
                IsAccepted = false,
                InvitationDate = DateTime.UtcNow
            };

            _context.GroupInvitations.Add(newInvitation);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
