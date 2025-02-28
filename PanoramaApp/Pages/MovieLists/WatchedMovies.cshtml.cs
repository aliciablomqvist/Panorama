// <copyright file="WatchedMovies.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.MovieLists
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using PanoramaApp.Data;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;
    using PanoramaApp.Services;

    public class WatchedModel : PageModel
    {
        private readonly IMovieListService movieListService;
        private readonly UserManager<IdentityUser> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatchedModel"/> class.
        /// </summary>
        /// <param name="movieListService">The movie list service.</param>
        /// <param name="userManager">The user manager.</param>
        public WatchedModel(IMovieListService movieListService, UserManager<IdentityUser> userManager)
        {
            this.movieListService = movieListService;
            this.userManager = userManager;
        }

        /// <summary>
        /// Gets the watched movies.
        /// </summary>
        /// <value>
        /// The watched movies.
        /// </value>
        public List<Movie> WatchedMovies { get; private set; } = new List<Movie>();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.RedirectToPage("/Account/Login");
            }

            this.WatchedMovies = await this.movieListService.GetMoviesFromListAsync("Watched", user.Id);

            return this.Page();
        }
    }
}
