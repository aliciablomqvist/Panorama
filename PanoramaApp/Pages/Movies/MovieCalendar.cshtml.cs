// <copyright file="MovieCalendar.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Movies
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class MovieCalendarModel : PageModel
    {
        private readonly IMovieCalendarService movieCalendarService;

        public MovieCalendarModel(IMovieCalendarService movieCalendarService)
        {
            this.movieCalendarService = movieCalendarService;
        }

        public List<Movie> Movies { get; set; } = new List<Movie>();

        public List<MovieCalendar> ScheduledMovies { get; set; } = new List<MovieCalendar>();

        [BindProperty]
        public int MovieId { get; set; }

        [BindProperty]
        public DateTime ScheduledDate { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            this.Movies = await this.movieCalendarService.GetAllMoviesAsync();
            this.ScheduledMovies = await this.movieCalendarService.GetScheduledMoviesAsync();
            return this.Page();
        }

        public async Task<IActionResult> OnPostScheduleMovieAsync()
        {
            try
            {
                await this.movieCalendarService.ScheduleMovieAsync(this.MovieId, this.ScheduledDate);
                return this.RedirectToPage();
            }
            catch (ArgumentException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return await this.OnGetAsync(); // Återställ sidan med data
            }
        }
    }
}