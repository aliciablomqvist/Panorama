// <copyright file="MovieListDetails.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Movies
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;

    public class MovieListDetailsModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public MovieListDetailsModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public MovieList MovieList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            this.MovieList = await this.context.MovieLists
                .Include(ml => ml.Movies)
                .ThenInclude(mli => mli.Movie)
                .FirstOrDefaultAsync(ml => ml.Id == id);

            if (this.MovieList == null)
            {
                return this.RedirectToPage("/Error");
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostSavePrioritiesAsync([FromBody] List<MoviePriorityUpdate> updates)
        {
            if (updates == null || !updates.Any())
            {
                return this.BadRequest("Invalid data received.");
            }

            foreach (var update in updates)
            {
                var movie = await this.context.Movies
                    .FirstOrDefaultAsync(m => m.Id == update.Id);

                if (movie != null)
                {
                    movie.Priority = update.Priority;
                }
            }

            await this.context.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }

        public class MoviePriorityUpdate
        {
            public int Id { get; set; }

            public int Priority { get; set; }
        }
    }
}