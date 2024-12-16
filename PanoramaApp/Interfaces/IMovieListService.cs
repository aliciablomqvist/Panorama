// <copyright file="IMovieListService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PanoramaApp.Models;

    public interface IMovieListService
    {
        Task AddToListAsync(string listName, int movieId, string userId);

        Task<List<MovieList>> GetListsByUserAsync(string userId);

        Task<MovieList> GetMovieListByIdAsync(int id);

        Task DeleteMovieListAsync(int id);

        Task UpdateMoviePrioritiesAsync(List<MoviePriorityUpdate> updates);

        Task<List<Movie>> GetMoviesFromListAsync(string listName, string userId);

        Task<List<MovieList>> GetMovieListsForUserAsync(string userId);

        Task<MovieList> GetFavoritesListAsync(string userId);

        Task CreateMovieListAsync(string name, string userId, bool isShared);

        Task<MovieList> GetLastCreatedMovieListAsync(string userId);

        Task<List<SelectListItem>> GetAvailableMoviesForListAsync(int listId);

        Task AddMoviesToListAsync(int listId, List<int> movieIds);
    }

    public class MoviePriorityUpdate
    {
        public int Id { get; set; }

        public int Priority { get; set; }
    }
}