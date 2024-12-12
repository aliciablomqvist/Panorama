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

    public class MyFavoritesModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public MyFavoritesModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public MovieList MovieList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = this.userManager.GetUserId(this.User);

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToPage("/Account/Login");
            }

            this.MovieList = await this.context.MovieLists
                .Include(ml => ml.Movies)
                    .ThenInclude(mli => mli.Movie)
                .FirstOrDefaultAsync(ml => ml.Name == "My Favorites" && ml.OwnerId == userId);

            return this.Page();
        }
    }
}