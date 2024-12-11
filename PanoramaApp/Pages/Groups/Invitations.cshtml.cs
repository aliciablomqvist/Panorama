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
        public string CurrentUserId { get; private set; } = string.Empty;

        public async Task OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            CurrentUserId = currentUser?.Id ?? string.Empty;

            // Load invitations
            Invitations = await _context.GroupInvitations
                .Where(i => i.InvitedUserId == currentUser.Id && !i.IsAccepted)
                .ToListAsync();

            // Load groups where the user is a member
            Groups = await _context.Groups
                .Include(g => g.Members)
                .Where(g => g.Members.Any(m => m.UserId == CurrentUserId))
                .ToListAsync();

            // Load all users
            Users = await _context.Users.ToListAsync();
        }

        public async Task<IActionResult> OnPostAcceptAsync(int invitationId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var invitation = await _context.GroupInvitations
                .Include(i => i.Group)
                .FirstOrDefaultAsync(i => i.Id == invitationId);

            if (invitation == null || currentUser == null)
            {
                return NotFound();
            }

            // Mark invitation as accepted
            invitation.IsAccepted = true;
            _context.GroupInvitations.Update(invitation);

            // Add user to group if not already a member
            var isAlreadyMember = await _context.GroupMembers
                .AnyAsync(m => m.GroupId == invitation.GroupId && m.UserId == currentUser.Id);

            if (!isAlreadyMember)
            {
                var newMember = new GroupMember
                {
                    GroupId = invitation.GroupId,
                    UserId = currentUser.Id
                };

                _context.GroupMembers.Add(newMember);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostInviteAsync(int groupId, string invitedUserId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Ensure the user is not inviting themselves
            if (currentUser.Id == invitedUserId)
            {
                ModelState.AddModelError(string.Empty, "Du kan inte bjuda in dig själv till en grupp.");
                await OnGetAsync(); // Reload data
                return Page();
            }

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
