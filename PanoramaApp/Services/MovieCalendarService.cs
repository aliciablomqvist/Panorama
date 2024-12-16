// <copyright file="MovieCalendarService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PanoramaApp.Data;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class MovieCalendarService : IMovieCalendarService
    {
        private readonly ApplicationDbContext context;

        public MovieCalendarService(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets all movies asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            return await this.context.Movies.ToListAsync();
        }

        /// <summary>
        /// Gets the scheduled movies asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MovieCalendar>> GetScheduledMoviesAsync()
        {
            return await this.context.MovieCalendars
                .Include(mc => mc.Movie)
                .ToListAsync();
        }

        /// <summary>
        /// Schedules the movie asynchronous.
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <param name="scheduledDate">The scheduled date.</param>
        /// <exception cref="System.ArgumentException">The movie does not exist.</exception>
        public async Task ScheduleMovieAsync(int movieId, DateTime scheduledDate)
        {
            var movie = await this.context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                throw new ArgumentException("The movie does not exist.");
            }

            var calendarEntry = new MovieCalendar
            {
                MovieId = movieId,
                Date = scheduledDate,
            };

            this.context.MovieCalendars.Add(calendarEntry);
            await this.context.SaveChangesAsync();
        }
    }
}
