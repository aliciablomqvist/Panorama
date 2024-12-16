// <copyright file="Create.cshtml.cs" company="PlaceholderCompany">
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
    public class CreateGroupModel : PageModel
    {
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;

        public CreateGroupModel(IGroupService groupService, IUserService userService)
        {
            _groupService = groupService;
            _userService = userService;
        }

        [BindProperty]
        public string Name { get; set; } = string.Empty;

        public List<IdentityUser> Users { get; set; } = new();

        [BindProperty]
        public List<string> SelectedUsers { get; set; } = new();

        public async Task OnGetAsync()
        {
            Users = await _userService.GetAllUsersAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return RedirectToPage("/Account/Login");
            }

            await _groupService.CreateGroupAsync(Name, currentUser.Id, SelectedUsers);

            return RedirectToPage("/Groups/ViewGroups");
        }
    }
}
