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
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;
    using PanoramaApp.Services;

    public class CreateMovieListModel : PageModel
    {
        private readonly IMovieListService movieListService;
        private readonly IGroupService groupService;
        private readonly UserManager<IdentityUser> userManager;

        public CreateMovieListModel(
            IMovieListService movieListService,
            IGroupService groupService,
            UserManager<IdentityUser> userManager)
        {
            this.movieListService = movieListService;
            this.groupService = groupService;
            this.userManager = userManager;
        }

        [BindProperty]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        public List<int> SelectedGroupIds { get; set; } = new ();

        public List<Group> UserGroups { get; private set; } = new ();

        public async Task OnGetAsync()
        {
            var userId = this.userManager.GetUserId(this.User);
            this.UserGroups = await this.groupService.GetUserGroupsAsync(userId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.Challenge();
            }

            await this.movieListService.CreateMovieListAsync(this.Name, user.Id, this.SelectedGroupIds.Count > 0);

            var movieList = await this.movieListService.GetLastCreatedMovieListAsync(user.Id);
            if (movieList != null)
            {
                await this.groupService.AddMovieListToGroupsAsync(movieList, this.SelectedGroupIds);
            }

            return this.RedirectToPage("/MovieLists/ViewMovieLists");
        }
    }
}
