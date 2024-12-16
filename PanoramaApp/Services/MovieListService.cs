// <copyright file="MovieListService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    using PanoramaApp.Data;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;
    using PanoramaApp.Services;

    public class MovieListService : IMovieListService
    {
        private readonly ApplicationDbContext context;

        public MovieListService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddToListAsync(string listName, int movieId, string userId)
        {
            var list = await this.context.MovieLists
                .Include(ml => ml.Movies)
                .FirstOrDefaultAsync(ml => ml.Name == listName && ml.OwnerId == userId);

            if (list == null)
            {
                list = new MovieList { Name = listName, OwnerId = userId };
                this.context.MovieLists.Add(list);
                await this.context.SaveChangesAsync();
            }

            if (!list.Movies.Any(mli => mli.MovieId == movieId))
            {
                var movieListItem = new MovieListItem { MovieListId = list.Id, MovieId = movieId };
                list.Movies.Add(movieListItem);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<List<MovieList>> GetListsByUserAsync(string userId)
        {
            return await this.context.MovieLists
                .Where(ml => ml.OwnerId == userId)
                .ToListAsync();
        }

        public async Task<MovieList> GetMovieListByIdAsync(int id)
        {
            return await this.context.MovieLists
                .Include(ml => ml.Movies)
                .ThenInclude(mli => mli.Movie)
                .FirstOrDefaultAsync(ml => ml.Id == id);
        }

        public async Task UpdateMoviePrioritiesAsync(List<MoviePriorityUpdate> updates)
        {
            foreach (var update in updates)
            {
                var movie = await this.context.Movies
                    .FirstOrDefaultAsync(m => m.Id == update.Id);

                if (movie != null)
                {
                    movie.Priority = update.Priority;
                }
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<List<Movie>> GetMoviesFromListAsync(string listName, string userId)
        {
            var movieList = await this.context.MovieLists
                .Include(ml => ml.Movies)
                .ThenInclude(mli => mli.Movie)
                .FirstOrDefaultAsync(ml => ml.Name == listName && ml.OwnerId == userId);

            return movieList?.Movies.Select(mli => mli.Movie).ToList() ?? new List<Movie>();
        }

        public async Task<List<MovieList>> GetMovieListsForUserAsync(string userId)
        {
            return await this.context.MovieLists
                .Include(ml => ml.Movies)
                .Include(ml => ml.SharedWithGroups)
                .Where(ml => ml.OwnerId == userId ||
                             ml.SharedWithGroups.Any(g => g.Members.Any(m => m.UserId == userId)))
                .ToListAsync();
        }

        public async Task<MovieList> GetFavoritesListAsync(string userId)
        {
            return await this.context.MovieLists
                .Include(ml => ml.Movies)
                    .ThenInclude(mli => mli.Movie)
                .FirstOrDefaultAsync(ml => ml.Name == "My Favorites" && ml.OwnerId == userId);
        }

        public async Task DeleteMovieListAsync(int id)
        {
            var movieList = await this.context.MovieLists
                .Include(ml => ml.Movies)
                .FirstOrDefaultAsync(ml => ml.Id == id);

            if (movieList == null)
            {
                throw new ArgumentException($"MovieList with ID {id} not found.");
            }

            this.context.MovieLists.Remove(movieList);
            await this.context.SaveChangesAsync();
        }

        public async Task CreateMovieListAsync(string name, string userId, bool isShared)
        {
            var movieList = new MovieList
            {
                Name = name,
                OwnerId = userId,
                IsShared = isShared,
            };

            this.context.MovieLists.Add(movieList);
            await this.context.SaveChangesAsync();
        }

        public async Task<MovieList> GetLastCreatedMovieListAsync(string userId)
        {
            return await this.context.MovieLists
                .Where(ml => ml.OwnerId == userId)
                .OrderByDescending(ml => ml.Id) // Anta att ID:t ökar med varje ny lista
                .FirstOrDefaultAsync();
        }

        public async Task<List<SelectListItem>> GetAvailableMoviesForListAsync(int listId)
        {
            var movieList = await this.GetMovieListByIdAsync(listId);

            if (movieList == null)
            {
                return new List<SelectListItem>();
            }

            var existingMovieIds = movieList.Movies.Select(mli => mli.MovieId).ToList();

            var movies = await this.context.Movies
                .Where(m => !existingMovieIds.Contains(m.Id))
                .ToListAsync();

            return movies.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Title,
            }).ToList();
        }

        public async Task AddMoviesToListAsync(int listId, List<int> movieIds)
        {
            var movieList = await this.GetMovieListByIdAsync(listId);

            if (movieList == null || movieIds == null || !movieIds.Any())
            {
                return;
            }

            foreach (var movieId in movieIds)
            {
                var movie = await this.context.Movies.FindAsync(movieId);

                if (movie != null)
                {
                    var movieListItem = new MovieListItem
                    {
                        MovieListId = listId,
                        MovieId = movieId,
                        Movie = movie,
                    };

                    movieList.Movies.Add(movieListItem);
                }
            }

            await this.context.SaveChangesAsync();
        }
    }
}
