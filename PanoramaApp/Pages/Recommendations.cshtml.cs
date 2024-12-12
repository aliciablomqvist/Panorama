// <copyright file="Recommendations.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;
    using PanoramaApp.Services;

    public class RecommendationsModel : PageModel
    {
        private readonly TmdbService tmdbService;
        private readonly ApplicationDbContext dbContext;

        public Movie CurrentMovie { get; private set; }

        public List<Movie> Recommendations { get; private set; } = new List<Movie>();

        public RecommendationsModel(TmdbService tmdbService, ApplicationDbContext dbContext)
        {
            this.tmdbService = tmdbService;
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> OnGetAsync(int tmdbId)
        {
            if (tmdbId == 0)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid TMDb ID.");
                return this.Page();
            }

            this.CurrentMovie = await this.dbContext.Movies.FirstOrDefaultAsync(m => m.TmdbId == tmdbId);

            if (this.CurrentMovie == null)
            {
                this.ModelState.AddModelError(string.Empty, "Movie not found in the database.");
                return this.Page();
            }

            this.Recommendations = await this.tmdbService.GetRecommendationsAsync(tmdbId);

            if (this.Recommendations == null || !this.Recommendations.Any())
            {
                this.Recommendations = new List<Movie>();
            }

            return this.Page();
        }
    }
}