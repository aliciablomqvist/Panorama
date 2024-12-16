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

    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class GroupChatModel : PageModel
    {
        private readonly IGroupChatService chatService;
        private readonly IGroupService groupService;
        private readonly UserManager<IdentityUser> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupChatModel"/> class.
        /// </summary>
        /// <param name="chatService">The chat service.</param>
        /// <param name="groupService">The group service.</param>
        /// <param name="userManager">The user manager.</param>
        public GroupChatModel(IGroupChatService chatService, IGroupService groupService, UserManager<IdentityUser> userManager)
        {
            this.chatService = chatService;
            this.groupService = groupService;
            this.userManager = userManager;
        }

        public List<ChatMessage> Messages { get; set; } = new ();

        [BindProperty]
        public string MessageText { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public int GroupId { get; set; }

        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<IActionResult> OnGetAsync(int groupId)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.Unauthorized();
            }

            // Kontrollera medlemskap i gruppen
            var isMember = await this.groupService.IsUserMemberOfGroupAsync(user.Id, groupId);
            if (!isMember)
            {
                return this.Forbid();
            }

            this.GroupId = groupId;
            this.Messages = await this.chatService.GetMessagesForGroupAsync(groupId);

            return this.Page();
        }

        /// <summary>
        /// Called when [post asynchronous].
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

            return this.RedirectToPage(new { this.GroupId });
        }
    }
}
