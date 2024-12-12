// <copyright file="MovieListDetails.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.MovieLists
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using PanoramaApp.Data;
    using PanoramaApp.Models;

    public class MovieListDetailsModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<MovieListDetailsModel> logger;

        public MovieListDetailsModel(ApplicationDbContext context, ILogger<MovieListDetailsModel> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public MovieList MovieList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            this.logger.LogInformation("Fetching MovieList with ID {Id}", id);

            try
            {
                this.MovieList = await this.context.MovieLists
                    .Include(ml => ml.Movies)
                        .ThenInclude(mli => mli.Movie)
                    .FirstOrDefaultAsync(ml => ml.Id == id);

                if (this.MovieList == null)
                {
                    this.logger.LogWarning("MovieList with ID {Id} not found", id);
                    return this.RedirectToPage("/Error");
                }

                return this.Page();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while fetching MovieList with ID {Id}", id);
                return this.RedirectToPage("/Error");
            }
        }

        public async Task<IActionResult> OnPostDeleteListAsync(int id)
        {
            this.logger.LogInformation("Attempting to delete MovieList with ID {Id}", id);

            try
            {
                var movieList = await this.context.MovieLists
                    .Include(ml => ml.Movies)
                    .FirstOrDefaultAsync(ml => ml.Id == id);

                if (movieList == null)
                {
                    this.logger.LogWarning("MovieList with ID {Id} not found for deletion", id);
                    return this.RedirectToPage("/Error");
                }

                this.context.MovieLists.Remove(movieList);
                await this.context.SaveChangesAsync();

                this.logger.LogInformation("Successfully deleted MovieList with ID {Id}", id);
                return this.RedirectToPage("/MovieLists/ViewMovieLists");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while deleting MovieList with ID {Id}", id);
                return this.RedirectToPage("/Error");
            }
        }
    }
}