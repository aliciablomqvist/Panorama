// <copyright file="IMovieCalendarService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PanoramaApp.Models;
    using PanoramaApp.Services;

    /// <summary>
    /// Interface for movie calendars.
    /// </summary>
    public interface IMovieCalendarService
    {
        /// <summary>
        /// Gets all movies asynchronous.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Movie>> GetAllMoviesAsync();

        /// <summary>
        /// Gets the scheduled movies asynchronous.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<MovieCalendar>> GetScheduledMoviesAsync();

        /// <summary>
        /// Schedules the movie asynchronous.
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <param name="scheduledDate">The scheduled date.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task ScheduleMovieAsync(int movieId, DateTime scheduledDate);
    }
}
