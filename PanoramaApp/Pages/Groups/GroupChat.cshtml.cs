// <copyright file="GroupChat.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Groups
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;
    using PanoramaApp.Services;

    public class GroupChatModel : PageModel
    {
        private readonly GroupChatService chatService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext context;

        public GroupChatModel(ApplicationDbContext context, GroupChatService chatService, UserManager<IdentityUser> userManager)
        {
            this.chatService = chatService;
            this.userManager = userManager;
            this.context = context;
        }

        public List<ChatMessage> Messages { get; set; } = new ();

        [BindProperty]
        public string MessageText { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public int GroupId { get; set; }

        public async Task<IActionResult> OnGetAsync(int groupId)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.Unauthorized();
            }

            // Check if the user is a member of the group
            var isMember = await this.context.GroupMembers
                .AnyAsync(m => m.GroupId == groupId && m.UserId == user.Id);

            if (!isMember)
            {
                return this.Forbid(); // Restrict access if the user is not a member of the group
            }

            this.GroupId = groupId;
            this.Messages = await this.chatService.GetMessagesForGroupAsync(groupId);

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(this.MessageText))
            {
                this.ModelState.AddModelError("MessageText", "Message cannot be empty.");
                return this.Page();
            }

            await this.chatService.SendMessageAsync(this.MessageText, user.Id, user.UserName, this.GroupId);

            // Redirect back to the same page to update the chat history.
            return this.RedirectToPage(new { this.GroupId });
        }
    }
}