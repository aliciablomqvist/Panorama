// <copyright file="AddToMovieList.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.MovieLists
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    using PanoramaApp.Data;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;
    using PanoramaApp.Services;

    public class AddMovieModel : PageModel
    {
        private readonly IMovieListService movieListService;
        private readonly UserManager<IdentityUser> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddMovieModel"/> class.
        /// </summary>
        /// <param name="movieListService">The movie list service.</param>
        /// <param name="userManager">The user manager.</param>
        public AddMovieModel(IMovieListService movieListService, UserManager<IdentityUser> userManager)
        {
            this.movieListService = movieListService;
            this.userManager = userManager;
        }

        public MovieList MovieList { get; private set; }

        [BindProperty]
        public List<int> SelectedMovieIds { get; set; } = new ();

        public List<SelectListItem> MovieOptions { get; private set; } = new ();

        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
        /// <param name="listId">The list identifier.</param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int listId)
        {
            this.MovieList = await this.movieListService.GetMovieListByIdAsync(listId);

            if (this.MovieList == null)
            {
                return this.RedirectToPage("/Error");
            }

            this.MovieOptions = await this.movieListService.GetAvailableMoviesForListAsync(listId);

            return this.Page();
        }

        /// <summary>
        /// Called when [post asynchronous].
        /// </summary>
        /// <param name="listId">The list identifier.</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(int listId)
        {
            await this.movieListService.AddMoviesToListAsync(listId, this.SelectedMovieIds);

            return this.RedirectToPage("/Movies/MovieListDetails", new { id = listId });
        }
    }
}
