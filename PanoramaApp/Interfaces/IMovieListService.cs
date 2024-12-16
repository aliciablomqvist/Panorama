// <copyright file="IMovieListService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using PanoramaApp.Models;

    /// <summary>
    /// Interface for managing movie lists and related operations.
    /// </summary>
    public interface IMovieListService
    {
        /// <summary>
        /// Adds to list asynchronous.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="movieId">The movie identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AddToListAsync(string listName, int movieId, string userId);

        /// <summary>
        /// Gets the lists by user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<MovieList>> GetListsByUserAsync(string userId);

        /// <summary>
        /// Gets the movie list by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<MovieList> GetMovieListByIdAsync(int id);

        /// <summary>
        /// Deletes the movie list asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task DeleteMovieListAsync(int id);

        /// <summary>
        /// Updates the movie priorities asynchronous.
        /// </summary>
        /// <param name="updates">The updates.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task UpdateMoviePrioritiesAsync(List<MoviePriorityUpdate> updates);

        /// <summary>
        /// Gets the movies from list asynchronous.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Movie>> GetMoviesFromListAsync(string listName, string userId);

        /// <summary>
        /// Gets the movie lists for user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<MovieList>> GetMovieListsForUserAsync(string userId);

        /// <summary>
        /// Gets the favorites list asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<MovieList> GetFavoritesListAsync(string userId);

        /// <summary>
        /// Creates the movie list asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="isShared">if set to <c>true</c> [is shared].</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task CreateMovieListAsync(string name, string userId, bool isShared);

        /// <summary>
        /// Gets the last created movie list asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<MovieList> GetLastCreatedMovieListAsync(string userId);

        /// <summary>
        /// Gets the available movies for list asynchronous.
        /// </summary>
        /// <param name="listId">The list identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<SelectListItem>> GetAvailableMoviesForListAsync(int listId);

        /// <summary>
        /// Adds the movies to list asynchronous.
        /// </summary>
        /// <param name="listId">The list identifier.</param>
        /// <param name="movieIds">The movie ids.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AddMoviesToListAsync(int listId, List<int> movieIds);
    }

    /// <summary>
    /// Represents a movie priority update.
    /// </summary>
    public class MoviePriorityUpdate
    {
        public int Id { get; set; }

        public int Priority { get; set; }
    }
}
