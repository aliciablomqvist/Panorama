// <copyright file="Create.cshtml.cs" company="PlaceholderCompany">
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

    public class CreateGroupModel : PageModel
    {
        private readonly IGroupService groupService;
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateGroupModel"/> class.
        /// </summary>
        /// <param name="groupService">The group service.</param>
        /// <param name="userService">The user service.</param>
        public CreateGroupModel(IGroupService groupService, IUserService userService)
        {
            this.groupService = groupService;
            this.userService = userService;
        }

        [BindProperty]
        public string Name { get; set; } = string.Empty;

        public List<IdentityUser> Users { get; set; } = new ();

        [BindProperty]
        public List<string> SelectedUsers { get; set; } = new ();

        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task OnGetAsync()
        {
            this.Users = await this.userService.GetAllUsersAsync();
        }

        /// <summary>
        /// Called when [post asynchronous].
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var currentUser = await this.userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return this.RedirectToPage("/Account/Login");
            }

            await this.groupService.CreateGroupAsync(this.Name, currentUser.Id, this.SelectedUsers);

            return this.RedirectToPage("/Groups/ViewGroups");
        }
    }
}
