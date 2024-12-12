// <copyright file="MyFavorites.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.MovieLists
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;
    using PanoramaApp.Services;
    using PanoramaApp.Interfaces;

    public class MyFavoritesModel : PageModel
    {
        private readonly IMovieListService _movieListService;
        private readonly UserManager<IdentityUser> _userManager;

        public MyFavoritesModel(IMovieListService movieListService, UserManager<IdentityUser> userManager)
        {
            _movieListService = movieListService;
            _userManager = userManager;
        }

        public MovieList MovieList { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            MovieList = await _movieListService.GetFavoritesListAsync(userId);

            return Page();
        }
    }
}