// <copyright file="StatisticService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Services
{
    using Microsoft.EntityFrameworkCore;

    using PanoramaApp.Data;
    using PanoramaApp.DTO;
    using PanoramaApp.Interfaces;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="PanoramaApp.Interfaces.IStatisticsService" />
    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext context;

        public StatisticsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the user statistics asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<UserStatisticsDto> GetUserStatisticsAsync(string userId)
        {
            var watchedMovies = await this.context.MovieLists
                .Where(ml => ml.Name == "Watched" && ml.OwnerId == userId)
                .Include(ml => ml.Movies)
                .ThenInclude(mli => mli.Movie)
                .SelectMany(ml => ml.Movies.Select(mli => mli.Movie))
                .ToListAsync();

            var genreCount = watchedMovies
                .GroupBy(m => m.Genre)
                .ToDictionary(g => g.Key, g => g.Count());

            var mostWatchedDecade = watchedMovies
                .GroupBy(m => (m.ReleaseDate.Year / 10) * 10)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key.ToString() ?? "N/A";

            return new UserStatisticsDto
            {
                FavoriteGenre = genreCount.OrderByDescending(g => g.Value).FirstOrDefault().Key,
                TotalMoviesWatched = watchedMovies.Count,
                MostWatchedDecade = mostWatchedDecade,
                GenreCount = genreCount,
            };
        }

        /// <summary>
        /// Gets the group statistics asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<GroupStatisticsDto> GetGroupStatisticsAsync(int groupId)
        {
            var groupMembers = await this.context.GroupMembers
                .Where(gm => gm.GroupId == groupId)
                .Select(gm => gm.UserId)
                .ToListAsync();

            if (!groupMembers.Any())
            {
                return new GroupStatisticsDto
                {
                    MostWatchedGenre = "N/A",
                    TotalMoviesWatchedByGroup = 0,
                    MostPopularDecade = "N/A",
                };
            }

            var watchedMovies = await this.context.MovieLists
                .Where(ml => ml.Name == "Watched" && groupMembers.Contains(ml.OwnerId))
                .SelectMany(ml => ml.Movies.Select(mli => mli.Movie))
                .ToListAsync();

            var favoriteMovies = await this.context.MovieLists
                .Where(ml => ml.Name == "My Favorites" && groupMembers.Contains(ml.OwnerId))
                .SelectMany(ml => ml.Movies.Select(mli => mli.Movie))
                .ToListAsync();

            var allMovies = watchedMovies.Concat(favoriteMovies).ToList();

            if (!allMovies.Any())
            {
                return new GroupStatisticsDto
                {
                    MostWatchedGenre = "N/A",
                    TotalMoviesWatchedByGroup = 0,
                    MostPopularDecade = "N/A",
                };
            }

            var mostWatchedGenre = allMovies
                .GroupBy(m => m.Genre)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key ?? "N/A";

            var mostPopularDecade = allMovies
                .GroupBy(m => (m.ReleaseDate.Year / 10) * 10)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key.ToString() ?? "N/A";

            return new GroupStatisticsDto
            {
                MostWatchedGenre = mostWatchedGenre,
                TotalMoviesWatchedByGroup = allMovies.Count,
                MostPopularDecade = mostPopularDecade,
            };
        }
    }
}
