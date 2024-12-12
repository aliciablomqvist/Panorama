// <copyright file="Invitations.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Groups
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;

    public class InvitationsModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public InvitationsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public List<GroupInvitation> Invitations { get; set; } = new List<GroupInvitation>();

        public List<Group> Groups { get; set; } = new List<Group>();

        public List<IdentityUser> Users { get; set; } = new List<IdentityUser>();

        public string CurrentUserId { get; private set; } = string.Empty;

        public async Task OnGetAsync()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            this.CurrentUserId = currentUser?.Id ?? string.Empty;

            // Load invitations
            this.Invitations = await this.context.GroupInvitations
                .Where(i => i.InvitedUserId == currentUser.Id && !i.IsAccepted)
                .ToListAsync();

            // Load groups where the user is a member
            this.Groups = await this.context.Groups
                .Include(g => g.Members)
                .Where(g => g.Members.Any(m => m.UserId == this.CurrentUserId))
                .ToListAsync();

            // Load all users
            this.Users = await this.context.Users.ToListAsync();
        }

        public async Task<IActionResult> OnPostAcceptAsync(int invitationId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var invitation = await this.context.GroupInvitations
                .Include(i => i.Group)
                .FirstOrDefaultAsync(i => i.Id == invitationId);

            if (invitation == null || currentUser == null)
            {
                return this.NotFound();
            }

            // Mark invitation as accepted
            invitation.IsAccepted = true;
            this.context.GroupInvitations.Update(invitation);

            // Add user to group if not already a member
            var isAlreadyMember = await this.context.GroupMembers
                .AnyAsync(m => m.GroupId == invitation.GroupId && m.UserId == currentUser.Id);

            if (!isAlreadyMember)
            {
                var newMember = new GroupMember
                {
                    GroupId = invitation.GroupId,
                    UserId = currentUser.Id,
                };

                this.context.GroupMembers.Add(newMember);
            }

            await this.context.SaveChangesAsync();

            return this.RedirectToPage();
        }

        public async Task<IActionResult> OnPostInviteAsync(int groupId, string invitedUserId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            // Ensure the user is not inviting themselves
            if (currentUser.Id == invitedUserId)
            {
                this.ModelState.AddModelError(string.Empty, "Du kan inte bjuda in dig själv till en grupp.");
                await this.OnGetAsync(); // Reload data
                return this.Page();
            }

            var newInvitation = new GroupInvitation
            {
                GroupId = groupId,
                InvitedUserId = invitedUserId,
                InvitedByUserId = currentUser.Id,
                IsAccepted = false,
                InvitationDate = DateTime.UtcNow,
            };

            this.context.GroupInvitations.Add(newInvitation);
            await this.context.SaveChangesAsync();

            return this.RedirectToPage();
        }
    }
}