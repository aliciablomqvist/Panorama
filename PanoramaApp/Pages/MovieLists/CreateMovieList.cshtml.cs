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
    using PanoramaApp.Services;
    using PanoramaApp.Interfaces;

    public class CreateMovieListModel : PageModel
    {
        private readonly IMovieListService _movieListService;
        private readonly IGroupService _groupService;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateMovieListModel(
            IMovieListService movieListService,
            IGroupService groupService,
            UserManager<IdentityUser> userManager)
        {
            _movieListService = movieListService;
            _groupService = groupService;
            _userManager = userManager;
        }

        [BindProperty]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        public List<int> SelectedGroupIds { get; set; } = new();

        public List<Group> UserGroups { get; private set; } = new();

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            UserGroups = await _groupService.GetUserGroupsAsync(userId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            await _movieListService.CreateMovieListAsync(Name, user.Id, SelectedGroupIds.Count > 0);

            var movieList = await _movieListService.GetLastCreatedMovieListAsync(user.Id);
            if (movieList != null)
            {
                await _groupService.AddMovieListToGroupsAsync(movieList, SelectedGroupIds);
            }

            return RedirectToPage("/MovieLists/ViewMovieLists");
        }
    }
}