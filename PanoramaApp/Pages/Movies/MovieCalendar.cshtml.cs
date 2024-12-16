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

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class MovieCalendarModel : PageModel
    {
        private readonly IMovieCalendarService movieCalendarService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieCalendarModel"/> class.
        /// </summary>
        /// <param name="movieCalendarService">The movie calendar service.</param>
        public MovieCalendarModel(IMovieCalendarService movieCalendarService)
        {
            this.movieCalendarService = movieCalendarService;
        }

        /// <summary>
        /// Gets or sets the movies.
        /// </summary>
        /// <value>
        /// The movies.
        /// </value>
        public List<Movie> Movies { get; set; } = new List<Movie>();

        /// <summary>
        /// Gets or sets the scheduled movies.
        /// </summary>
        /// <value>
        /// The scheduled movies.
        /// </value>
        public List<MovieCalendar> ScheduledMovies { get; set; } = new List<MovieCalendar>();

        /// <summary>
        /// Gets or sets the movie identifier.
        /// </summary>
        /// <value>
        /// The movie identifier.
        /// </value>
        [BindProperty]
        public int MovieId { get; set; }

        /// <summary>
        /// Gets or sets the scheduled date.
        /// </summary>
        /// <value>
        /// The scheduled date.
        /// </value>
        [BindProperty]
        public DateTime ScheduledDate { get; set; }

        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            this.Movies = await this.movieCalendarService.GetAllMoviesAsync();
            this.ScheduledMovies = await this.movieCalendarService.GetScheduledMoviesAsync();
            return this.Page();
        }

        /// <summary>
        /// Called when [post schedule movie asynchronous].
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
                return await this.OnGetAsync();
            }
        }
    }
}
