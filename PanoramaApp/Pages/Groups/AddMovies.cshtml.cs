// <copyright file="AddMoviesModel.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Interfaces;
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Pages.Groups
{
    public class AddMoviesModel : PageModel
    {
        private readonly IGroupService _groupService;
        private readonly IMovieService _movieService;

        public AddMoviesModel(IGroupService groupService, IMovieService movieService)
        {
            _groupService = groupService;
            _movieService = movieService;
        }

        public Group Group { get; set; } = default!;
        public List<Movie> AvailableMovies { get; set; } = new();

        [BindProperty]
        public int GroupId { get; set; }

        [BindProperty]
        public List<int> SelectedMovies { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int groupId)
        {
            var group = await _groupService.GetSpecificGroupByIdAsync(groupId);
            if (group == null)
            {
                return RedirectToPage("/Error");
            }

            Group = group;
            AvailableMovies = await _movieService.GetAvailableMoviesForGroupAsync(groupId);

            return Page();
        }

        public async Task<IActionResult> OnPostAddMoviesAsync()
        {
            var group = await _groupService.GetSpecificGroupByIdAsync(GroupId);
            if (group == null)
            {
                return RedirectToPage("/Error");
            }

            await _movieService.AssignMoviesToGroupAsync(GroupId, SelectedMovies);

            return RedirectToPage("/Groups/ViewGroups");
        }
    }
}
