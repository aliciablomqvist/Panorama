// <copyright file="Create.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;

public class CreateGroupModel : PageModel
{
    private readonly ApplicationDbContext context;
    private readonly UserManager<IdentityUser> userManager;

    public CreateGroupModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    public List<IdentityUser> Users { get; set; } = new List<IdentityUser>();

    [BindProperty]
    public List<string> SelectedUsers { get; set; } = new List<string>();

    public async Task OnGetAsync()
    {
        this.Users = await this.context.Users.ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        var currentUser = await this.userManager.GetUserAsync(this.User); // Get the current logged-in user
        if (currentUser == null)
        {
            // Handle the case where the user is not logged in (redirect to login page or show an error)
            return this.RedirectToPage("/Account/Login");
        }

        var newGroup = new Group
        {
            Name = this.Name,
            OwnerId = currentUser.Id, // Set the OwnerId to the current user's ID
        };

        this.context.Groups.Add(newGroup);
        await this.context.SaveChangesAsync();

        foreach (var userId in this.SelectedUsers)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var groupMember = new GroupMember
                {
                    GroupId = newGroup.Id,
                    UserId = user.Id,
                };
                this.context.GroupMembers.Add(groupMember);
            }
        }

        await this.context.SaveChangesAsync();

        return this.RedirectToPage("/Groups/ViewGroups");
    }
}