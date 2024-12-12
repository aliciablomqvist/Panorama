// <copyright file="CreateMovieList.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.MovieLists
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;

    public class CreateMovieListModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public CreateMovieListModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [BindProperty]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        public List<int> SelectedGroupIds { get; set; } = new List<int>();

        public List<Group> UserGroups { get; set; } = new ();

        public async Task OnGetAsync()
        {
            var userId = this.userManager.GetUserId(this.User);
            this.UserGroups = await this.context.Groups
                .Where(g => g.Members.Any(m => m.UserId == userId))
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.Challenge();
            }

            var movieList = new MovieList
            {
                Name = this.Name,
                OwnerId = user.Id,
                IsShared = this.SelectedGroupIds.Count > 0,
            };

            this.context.MovieLists.Add(movieList);
            await this.context.SaveChangesAsync();

            foreach (var groupId in this.SelectedGroupIds)
            {
                var group = await this.context.Groups.FindAsync(groupId);
                if (group != null)
                {
                    group.MovieLists.Add(movieList);
                }
            }

            await this.context.SaveChangesAsync();

            return this.RedirectToPage("/MovieLists/ViewMovieLists");
        }
    }
}