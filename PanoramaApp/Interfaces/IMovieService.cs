// <copyright file="IMovieService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PanoramaApp.Models;

    /// <summary>
    /// Interface for Movies,
    /// </summary>
    public interface IMovieService
    {
        /// <summary>
        /// Gets the movies asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<List<Movie>> GetMoviesAsync();

        /// <summary>
        /// Gets the movie by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Movie> GetMovieByIdAsync(int id);

        /// <summary>
        /// Gets the available movies for group asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        Task<List<Movie>> GetAvailableMoviesForGroupAsync(int groupId);

        /// <summary>
        /// Assigns the movies to group asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="movieIds">The movie ids.</param>
        /// <returns></returns>
        Task AssignMoviesToGroupAsync(int groupId, List<int> movieIds);
    }
}
