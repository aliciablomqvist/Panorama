// <copyright file="MovieCalendar.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Movies
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;

    public class MovieCalendarModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public MovieCalendarModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Movie> Movies { get; set; } // Alla filmer som kan schemaläggas

        public List<MovieCalendar> ScheduledMovies { get; set; } // Schemalagda filmer

        [BindProperty]
        public int MovieId { get; set; }

        [BindProperty]
        public DateTime ScheduledDate { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Hämta alla filmer
            this.Movies = await this.context.Movies.ToListAsync();

            // Logga antalet hämtade filmer
            Console.WriteLine($"Number of movies fetched: {this.Movies.Count}");

            // Hämta schemalagda filmer
            this.ScheduledMovies = await this.context.MovieCalendars
                .Include(mc => mc.Movie)
                .ToListAsync();

            return this.Page();
        }

        public async Task<IActionResult> OnPostScheduleMovieAsync()
        {
            var movie = await this.context.Movies.FirstOrDefaultAsync(m => m.Id == this.MovieId);

            if (movie == null)
            {
                this.ModelState.AddModelError(string.Empty, "The movie does not exist.");
                return await this.OnGetAsync(); // Återställ sidan med data
            }

            var calendarEntry = new MovieCalendar
            {
                MovieId = this.MovieId,
                Date = this.ScheduledDate,
            };

            this.context.MovieCalendars.Add(calendarEntry);
            await this.context.SaveChangesAsync();

            return this.RedirectToPage();
        }
    }
}