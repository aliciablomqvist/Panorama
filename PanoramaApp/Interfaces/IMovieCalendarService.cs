// <copyright file="IMovieCalendarService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PanoramaApp.Models;
    using PanoramaApp.Services;

    public interface IMovieCalendarService
    {
        Task<List<Movie>> GetAllMoviesAsync();

        Task<List<MovieCalendar>> GetScheduledMoviesAsync();

        Task ScheduleMovieAsync(int movieId, DateTime scheduledDate);
    }
}
