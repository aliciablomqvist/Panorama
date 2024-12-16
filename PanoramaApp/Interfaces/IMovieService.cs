// <copyright file="IMovieService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PanoramaApp.Models;

    public interface IMovieService
    {
        Task<List<Movie>> GetMoviesAsync();

        Task<Movie> GetMovieByIdAsync(int id);

        Task<List<Movie>> GetAvailableMoviesForGroupAsync(int groupId);

        Task AssignMoviesToGroupAsync(int groupId, List<int> movieIds);
    }
}
