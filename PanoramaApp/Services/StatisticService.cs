using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace PanoramaApp.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext _context;

        public StatisticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserStatistics> GetUserStatisticsAsync(string userId)
        {
            var watchedMovies = await _context.Movies
                .Where(m => m.MovieListItems.Any(ml => ml.MovieList.OwnerId == userId))
                .ToListAsync();

            var genreCounts = watchedMovies
                .GroupBy(m => m.Genre)
                .ToDictionary(g => g.Key, g => g.Count());

            var decadeCounts = watchedMovies
                .GroupBy(m => m.ReleaseDate.Year / 10 * 10)
                .ToDictionary(g => g.Key, g => g.Count());

            return new UserStatistics
            {
                MostWatchedGenre = genreCounts.OrderByDescending(g => g.Value).FirstOrDefault().Key ?? "Unknown",
                MostWatchedDecade = decadeCounts.OrderByDescending(d => d.Value).FirstOrDefault().Key,
                GenreCounts = genreCounts,
                DecadeCounts = decadeCounts,
                UserName = "User Name Placeholder" // Replace with actual user data if available
            };
        }

        public async Task<GroupStatistics> GetGroupStatisticsAsync(int groupId)
        {
            var watchedMovies = await _context.Movies
                .Where(m => m.MovieListItems.Any(ml => ml.MovieList.SharedWithGroups.Any(g => g.Id == groupId)))
                .ToListAsync();

            var genreCounts = watchedMovies
                .GroupBy(m => m.Genre)
                .ToDictionary(g => g.Key, g => g.Count());

            var decadeCounts = watchedMovies
                .GroupBy(m => m.ReleaseDate.Year / 10 * 10)
                .ToDictionary(g => g.Key, g => g.Count());

            return new GroupStatistics
            {
                MostWatchedGenre = genreCounts.OrderByDescending(g => g.Value).FirstOrDefault().Key ?? "Unknown",
                MostWatchedDecade = decadeCounts.OrderByDescending(d => d.Value).FirstOrDefault().Key,
                GenreCounts = genreCounts,
                DecadeCounts = decadeCounts,
                GroupName = "Group Name Placeholder" // Replace with actual group data if available
            };
        }
    }
}
