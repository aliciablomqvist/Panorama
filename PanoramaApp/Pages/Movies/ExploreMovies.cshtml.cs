// <copyright file="ExploreMovies.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Movies
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;
    using PanoramaApp.Services;

    public class ExploreMoviesModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public ExploreMoviesModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Movie> Movies { get; set; } = new List<Movie>();

        public async Task OnGetAsync()
        {
            this.Movies = await this.context.Movies
                .OrderByDescending(m => m.ReleaseDate)
                .ToListAsync();
        }
    }
}