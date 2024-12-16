// <copyright file="Invitations.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Interfaces;
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PanoramaApp.Pages.Groups
{
    public class InvitationsModel : PageModel
    {
        private readonly IInvitationService _invitationService;
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;

        public InvitationsModel(IInvitationService invitationService, IGroupService groupService, IUserService userService)
        {
            _invitationService = invitationService;
            _groupService = groupService;
            _userService = userService;
        }

        public List<GroupInvitation> Invitations { get; set; } = new();
        public List<Group> Groups { get; set; } = new();
        public List<IdentityUser> Users { get; set; } = new();
        public string CurrentUserId { get; private set; } = string.Empty;

        public async Task OnGetAsync()
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            CurrentUserId = currentUser?.Id ?? string.Empty;

            Invitations = await _invitationService.GetPendingInvitationsAsync(CurrentUserId);
            Groups = await _groupService.GetDetailedGroupsForUserAsync(CurrentUserId);
            Users = await _userService.GetAllUsersAsync();
        }

        public async Task<IActionResult> OnPostAcceptAsync(int invitationId)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            await _invitationService.AcceptInvitationAsync(invitationId, currentUser.Id);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostInviteAsync(int groupId, string invitedUserId)
        {
            var currentUser = await _userService.GetCurrentUserAsync();

            if (currentUser.Id == invitedUserId)
            {
                ModelState.AddModelError(string.Empty, "Du kan inte bjuda in dig själv till en grupp.");
                await OnGetAsync(); // Reload data
                return Page();
            }

            await _invitationService.SendInvitationAsync(groupId, invitedUserId, currentUser.Id);
            return RedirectToPage();
        }
    }
}
