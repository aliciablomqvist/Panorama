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
        private readonly IMovieCalendarService _movieCalendarService;

        public MovieCalendarModel(IMovieCalendarService movieCalendarService)
        {
            _movieCalendarService = movieCalendarService;
        }

        public List<Movie> Movies { get; set; } = new List<Movie>();

        public List<MovieCalendar> ScheduledMovies { get; set; } = new List<MovieCalendar>();

        [BindProperty]
        public int MovieId { get; set; }

        [BindProperty]
        public DateTime ScheduledDate { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Movies = await _movieCalendarService.GetAllMoviesAsync();
            ScheduledMovies = await _movieCalendarService.GetScheduledMoviesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostScheduleMovieAsync()
        {
            try
            {
                await _movieCalendarService.ScheduleMovieAsync(MovieId, ScheduledDate);
                return RedirectToPage();
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return await OnGetAsync(); // Återställ sidan med data
            }
        }
    }
}