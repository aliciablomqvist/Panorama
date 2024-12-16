// <copyright file="GroupChat.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Interfaces;
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Pages.Groups
{
    public class GroupChatModel : PageModel
    {
        private readonly IGroupChatService _chatService;
        private readonly IGroupService _groupService;
        private readonly UserManager<IdentityUser> _userManager;

        public GroupChatModel(IGroupChatService chatService, IGroupService groupService, UserManager<IdentityUser> userManager)
        {
            _chatService = chatService;
            _groupService = groupService;
            _userManager = userManager;
        }

        public List<ChatMessage> Messages { get; set; } = new();

        [BindProperty]
        public string MessageText { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public int GroupId { get; set; }

        public async Task<IActionResult> OnGetAsync(int groupId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Kontrollera medlemskap i gruppen
            var isMember = await _groupService.IsUserMemberOfGroupAsync(user.Id, groupId);
            if (!isMember)
            {
                return Forbid();
            }

            GroupId = groupId;
            Messages = await _chatService.GetMessagesForGroupAsync(groupId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(MessageText))
            {
                ModelState.AddModelError("MessageText", "Message cannot be empty.");
                return Page();
            }

            await _chatService.SendMessageAsync(MessageText, user.Id, user.UserName, GroupId);

            return RedirectToPage(new { GroupId });
        }
    }
}
