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
    using PanoramaApp.Services;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class WatchedModel : PageModel
    {
        private readonly IMovieListService _movieListService;
        private readonly UserManager<IdentityUser> _userManager;

        public WatchedModel(IMovieListService movieListService, UserManager<IdentityUser> userManager)
        {
            _movieListService = movieListService;
            _userManager = userManager;
        }

        public List<Movie> WatchedMovies { get; private set; } = new List<Movie>();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            WatchedMovies = await _movieListService.GetMoviesFromListAsync("Watched", user.Id);

            return Page();
        }
    }
}