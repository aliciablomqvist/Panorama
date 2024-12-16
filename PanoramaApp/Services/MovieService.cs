// <copyright file="MovieService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Services
{
    using Microsoft.EntityFrameworkCore;

    using PanoramaApp.Data;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext context;

        public MovieService(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the movies asynchronous.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<List<Movie>> GetMoviesAsync()
        {
            return await this.context.Movies
                .OrderByDescending(m => m.ReleaseDate)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the movie by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            return await this.context.Movies
                 .Include(m => m.MovieListItems)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <summary>
        /// Gets the available movies for group asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<List<Movie>> GetAvailableMoviesForGroupAsync(int groupId)
        {
            var assignedMovieIds = await this.context.Movies
                .Where(m => m.GroupId == groupId)
                .Select(m => m.Id)
                .ToListAsync();

            return await this.context.Movies
                .Where(m => !assignedMovieIds.Contains(m.Id))
                .ToListAsync();
        }

        /// <summary>
        /// Assigns the movies to group asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="movieIds">The movie ids.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task AssignMoviesToGroupAsync(int groupId, List<int> movieIds)
        {
            var movies = await this.context.Movies
                .Where(m => movieIds.Contains(m.Id))
                .ToListAsync();

            foreach (var movie in movies)
            {
                movie.GroupId = groupId;
            }

            this.context.Movies.UpdateRange(movies);
            await this.context.SaveChangesAsync();
        }
    }
}
