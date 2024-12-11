using PanoramaApp.Data;
using PanoramaApp.DTO;
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

        public async Task<UserStatisticsDto> GetUserStatisticsAsync(string userId)
        {
            var watchedMovies = await _context.MovieLists
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
                GenreCount = genreCount
            };
        }

        public async Task<GroupStatisticsDto> GetGroupStatisticsAsync(int groupId)
        {
            var groupMovies = await _context.Groups
                .Where(g => g.Id == groupId)
                .Include(g => g.Movies)
                .ThenInclude(m => m.MovieListItems)
                .SelectMany(g => g.Movies.SelectMany(mli => mli.MovieListItems.Select(mli => mli.Movie)))
                .ToListAsync();

            var genreCount = groupMovies
                .GroupBy(m => m.Genre)
                .ToDictionary(g => g.Key, g => g.Count());

            var mostPopularDecade = groupMovies
                .GroupBy(m => (m.ReleaseDate.Year / 10) * 10)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key.ToString() ?? "N/A";

            return new GroupStatisticsDto
            {
                MostWatchedGenre = genreCount.OrderByDescending(g => g.Value).FirstOrDefault().Key,
                TotalMoviesWatchedByGroup = groupMovies.Count,
                MostPopularDecade = mostPopularDecade
            };
        }
    }
}
