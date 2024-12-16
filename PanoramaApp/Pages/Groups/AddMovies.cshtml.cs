// <copyright file="AddMovies.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace PanoramaApp.Pages.Groups
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class AddMoviesModel : PageModel
    {
        private readonly IGroupService groupService;
        private readonly IMovieService movieService;

        public AddMoviesModel(IGroupService groupService, IMovieService movieService)
        {
            this.groupService = groupService;
            this.movieService = movieService;
        }

        public Group Group { get; set; } = default!;

        public List<Movie> AvailableMovies { get; set; } = new ();

        [BindProperty]
        public int GroupId { get; set; }

        [BindProperty]
        public List<int> SelectedMovies { get; set; } = new ();

        public async Task<IActionResult> OnGetAsync(int groupId)
        {
            var group = await this.groupService.GetSpecificGroupByIdAsync(groupId);
            if (group == null)
            {
                return this.RedirectToPage("/Error");
            }

            this.Group = group;
            this.AvailableMovies = await this.movieService.GetAvailableMoviesForGroupAsync(groupId);

            return this.Page();
        }

        public async Task<IActionResult> OnPostAddMoviesAsync()
        {
            var group = await this.groupService.GetSpecificGroupByIdAsync(this.GroupId);
            if (group == null)
            {
                return this.RedirectToPage("/Error");
            }

            await this.movieService.AssignMoviesToGroupAsync(this.GroupId, this.SelectedMovies);

            return this.RedirectToPage("/Groups/ViewGroups");
        }
    }
}
