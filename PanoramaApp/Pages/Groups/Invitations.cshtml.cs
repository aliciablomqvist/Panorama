// <copyright file="Invitations.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Groups
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class InvitationsModel : PageModel
    {
        private readonly IInvitationService invitationService;
        private readonly IGroupService groupService;
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvitationsModel"/> class.
        /// </summary>
        /// <param name="invitationService">The invitation service.</param>
        /// <param name="groupService">The group service.</param>
        /// <param name="userService">The user service.</param>
        public InvitationsModel(IInvitationService invitationService, IGroupService groupService, IUserService userService)
        {
            this.invitationService = invitationService;
            this.groupService = groupService;
            this.userService = userService;
        }

        public List<GroupInvitation> Invitations { get; set; } = new ();

        public List<Group> Groups { get; set; } = new ();

        public List<IdentityUser> Users { get; set; } = new ();

        public string CurrentUserId { get; private set; } = string.Empty;

        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task OnGetAsync()
        {
            var currentUser = await this.userService.GetCurrentUserAsync();
            this.CurrentUserId = currentUser?.Id ?? string.Empty;

            this.Invitations = await this.invitationService.GetPendingInvitationsAsync(this.CurrentUserId);
            this.Groups = await this.groupService.GetDetailedGroupsForUserAsync(this.CurrentUserId);
            this.Users = await this.userService.GetAllUsersAsync();
        }

        /// <summary>
        /// Called when [post accept asynchronous].
        /// </summary>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<IActionResult> OnPostAcceptAsync(int invitationId)
        {
            var currentUser = await this.userService.GetCurrentUserAsync();
            await this.invitationService.AcceptInvitationAsync(invitationId, currentUser.Id);
            return this.RedirectToPage();
        }

        /// <summary>
        /// Called when [post invite asynchronous].
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="invitedUserId">The invited user identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<IActionResult> OnPostInviteAsync(int groupId, string invitedUserId)
        {
            var currentUser = await this.userService.GetCurrentUserAsync();

            if (currentUser.Id == invitedUserId)
            {
                this.ModelState.AddModelError(string.Empty, "Du kan inte bjuda in dig själv till en grupp.");
                await this.OnGetAsync(); // Reload data
                return this.Page();
            }

            await this.invitationService.SendInvitationAsync(groupId, invitedUserId, currentUser.Id);
            return this.RedirectToPage();
        }
    }
}
