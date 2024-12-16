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
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;
    using PanoramaApp.Services;

    public class MyFavoritesModel : PageModel
    {
        private readonly IMovieListService movieListService;
        private readonly UserManager<IdentityUser> userManager;

        public MyFavoritesModel(IMovieListService movieListService, UserManager<IdentityUser> userManager)
        {
            this.movieListService = movieListService;
            this.userManager = userManager;
        }

        public MovieList MovieList { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = this.userManager.GetUserId(this.User);

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToPage("/Account/Login");
            }

            this.MovieList = await this.movieListService.GetFavoritesListAsync(userId);

            return this.Page();
        }
    }
}
