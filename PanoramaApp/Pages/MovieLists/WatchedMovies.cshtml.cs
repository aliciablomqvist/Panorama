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
    using PanoramaApp.Models;

    public class WatchedModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public WatchedModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public List<Movie> WatchedMovies { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.RedirectToPage("/Account/Login");
            }

            var userId = user.Id;

            var watchedList = await this.context.MovieLists
                .Include(ml => ml.Movies)
                .ThenInclude(mli => mli.Movie)
                .FirstOrDefaultAsync(ml => ml.Name == "Watched" && ml.OwnerId == userId);

            this.WatchedMovies = watchedList?.Movies.Select(mli => mli.Movie).ToList() ?? new List<Movie>();

            return this.Page();
        }
    }
}